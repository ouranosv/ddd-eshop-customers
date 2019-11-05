using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Application;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.AppSettings;
using DataAccess.MsSql;
using DataAccess.MsSql.UnitsOfWork;
using DataAccess.UnitsOfWork;
using Domain.Repositories;
using Domain.Services;
using GreenPipes;
using Infrastructure.Services.Diagnostics;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Swagger;
using WebApi.Middleware;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Customer Api", Version = "v1" });
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "WebApi.xml");
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<AppSettings>(Configuration);

            var builder = new ContainerBuilder();

            builder.RegisterType<Logger>()
                .As<ILogger>()
                .SingleInstance();

            builder.Register(c => new DataAccess.MsSql.ConnectionFactory(DatabaseHelper.GetConnectionString(Configuration)))
                .As<DataAccess.MsSql.ConnectionFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(GetAssemblyByName("Application"))
                .AssignableTo<IApplicationService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(GetAssemblyByName("Domain"))
                .AssignableTo<IDomainService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(GetAssemblyByName("DataAccess.MsSql"))
                .AssignableTo<IRepository>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Register(context =>
            {
                return Bus.Factory.CreateUsingRabbitMq(rmq =>
                {
                    var host = rmq.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    rmq.ExchangeType = ExchangeType.Fanout;
                });
            })
            .As<IBusControl>()
            .As<IBus>()
            .As<IPublishEndpoint>()
            .SingleInstance();

            builder.Populate(services);
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        //public void ConfigureContainer(ContainerBuilder builder)
        //{

        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Api V1");
                c.RoutePrefix = string.Empty;
            });

            UpgradeDatabase();

            var bus = ApplicationContainer.Resolve<IBusControl>();
            var busHandle = TaskUtil.Await(() => bus.StartAsync());
            lifetime.ApplicationStopping.Register(() => busHandle.Stop());
        }

        private Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().Single(assembly => assembly.GetName().Name == name);
        }

        private void UpgradeDatabase()
        {
            var connectionString = DatabaseHelper.GetConnectionString(Configuration);
            DataAccess.MsSql.Database.Program.Main(new[] { connectionString });
        }
    }
}
