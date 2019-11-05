using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccess.UnitsOfWork;
using Domain.Models;
using Domain.Repositories;

namespace DataAccess.MsSql.Repositories
{
    public class CustomerRepository : Repository, ICustomerRepository
    {
        public CustomerRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public Task<int> InsertAsync(Customer customer)
        {
            var commandText = "INSERT INTO Customers (Id, FirstName, LastName, Email) VALUES (@Id, @FirstName, @LastName, @Email)";
            var commandParameters = new DbParameter[4]
            {
                new SqlParameter { ParameterName = "@Id", DbType = DbType.Guid, Value = customer.Id.Value },
                new SqlParameter { ParameterName = "@FirstName", DbType = DbType.String, Value = customer.Name.First },
                new SqlParameter { ParameterName = "@LastName", DbType = DbType.String, Value = customer.Name.Last },
                new SqlParameter { ParameterName = "@Email", DbType = DbType.String, Value = customer.Email.Value }
            };

            return UnitOfWork.ExecuteNonQueryAsync(commandText, commandParameters);
        }

        public async Task<Customer> FindByEmailAsync(string email)
        {
            Customer customer = null;
            var commandText = "SELECT * FROM Customers WHERE Email = @Email";
            var commandParameters = new DbParameter[1]
            {
                new SqlParameter { ParameterName = "@Email", DbType = DbType.String, Value = email }
            };

            using (var reader = await UnitOfWork.ExecuteReaderAsync(commandText, commandParameters))
            {
                while (reader.Read())
                {
                    var id = (Guid)reader["Id"];
                    var firstName = (string)reader["firstName"];
                    var lastName = (string)reader["lastName"];

                    var customerId = new CustomerId(id);
                    var name = new Name(firstName, lastName);
                    var emailVo = new Email(email);

                    customer = new Customer(customerId, name, emailVo);
                }
            }

            return customer;
        }
    }
}
