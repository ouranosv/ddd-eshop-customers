using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DataAccess.MsSql.Repositories;
using DataAccess.UnitsOfWork;
using Domain.Models;
using ExpectedObjects;
using Moq;
using Xunit;

namespace DataAccess.MsSql.Tests.Repositories
{
    public class CustomerRepositoryTests
    {
        #region InsertAsync

        [Fact]
        public async void InsertAsync_WithCustomer_ExecutesInsertQueryAndReturnsAffectedRows()
        {
            //arrange
            var guid = Guid.NewGuid();
            var id = new CustomerId(guid);
            var name = new Name("first", "last");
            var email = new Email("email@test.com");
            var customer = new Customer(id, name, email);

            var commandText = "INSERT INTO Customers (Id, FirstName, LastName, Email) VALUES (@Id, @FirstName, @LastName, @Email)";
            var commandParameters = new DbParameter[4]
            {
                new SqlParameter { ParameterName = "@Id", DbType = DbType.Guid, Value = guid },
                new SqlParameter { ParameterName = "@FirstName", DbType = DbType.String, Value = "first" },
                new SqlParameter { ParameterName = "@LastName", DbType = DbType.String, Value = "last" },
                new SqlParameter { ParameterName = "@Email", DbType = DbType.String, Value = "email@test.com" }
            };

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(t => t.ExecuteNonQueryAsync(
                commandText,
                It.Is<DbParameter[]>(a => commandParameters.ToExpectedObject().Equals(a))
            )).ReturnsAsync(1);

            var customerRepository = new CustomerRepository(unitOfWork.Object);

            //act
            var actual = await customerRepository.InsertAsync(customer);

            //assert
            Assert.Equal(1, actual);

            unitOfWork.Verify(t => t.ExecuteNonQueryAsync(
                commandText,
                It.Is<DbParameter[]>(a => commandParameters.ToExpectedObject().Equals(a))
            ), Times.Once);
        }

        #endregion

        #region FindByEmailAsync

        [Fact]
        public async void FindByEmailAsync_WithEmail_ReturnsCustomerWithSpecifiedEmail()
        {
            //arrange
            var guid = Guid.Parse("08062E33-889E-472A-870A-650F3F018DE7");
            var id = new CustomerId(guid);
            var name = new Name("first", "last");
            var email = new Email("email@test.com");
            var expected = new Customer(id, name, email);

            var columns = new DataColumn[]
            {
                new DataColumn("Id", typeof(Guid)),
                new DataColumn("FirstName", typeof(string)),
                new DataColumn("LastName", typeof(string)),
                new DataColumn("Email", typeof(string))
            };

            var dt = new DataTable();
            dt.Columns.AddRange(columns);
            dt.Rows.Add(guid, "first", "last", "email@test.com");

            var reader = dt.CreateDataReader();

            var commandText = "SELECT * FROM Customers WHERE Email = @Email";
            var commandParameters = new DbParameter[1]
            {
                new SqlParameter { ParameterName = "@Email", DbType = DbType.String, Value = email.Value }
            };

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(t => t.ExecuteReaderAsync(
                commandText,
                It.Is<DbParameter[]>(a => commandParameters.ToExpectedObject().Equals(a))
            )).ReturnsAsync(reader);

            var customerRepository = new CustomerRepository(unitOfWork.Object);

            //act
            var actual = await customerRepository.FindByEmailAsync(email.Value);

            //assert
            expected.ToExpectedObject().ShouldEqual(actual);

            unitOfWork.Verify(t => t.ExecuteReaderAsync(
                commandText,
                It.Is<DbParameter[]>(a => commandParameters.ToExpectedObject().Equals(a))
            ), Times.Once);
        }

        #endregion
    }
}
