using System;
using System.IO;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;

namespace DataAccess.MsSql.Database
{
    public static class Program
    {
        private static IConfigurationRoot _configuration;

        public static int Main(string[] args)
        {
            InitializeApplication();

            var assembly = Assembly.GetExecutingAssembly();
            var connectionString = args.Length == 0 ? DatabaseHelper.GetConnectionString(_configuration) : args[0];
            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(assembly)
                    .WithTransactionPerScript()
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();

                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();

            return 0;
        }

        private static void InitializeApplication()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", true);

            _configuration = builder.Build();
        }
    }
}
