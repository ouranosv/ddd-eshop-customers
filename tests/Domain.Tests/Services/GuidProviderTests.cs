using System;
using Domain.Services;
using Xunit;

namespace Domain.Tests.Services
{
    public class GuidProviderTests
    {
        [Fact]
        public void NewGuid_ReturnsNewGuid()
        {
            //arrange
            var guidProvider = new GuidProvider();

            //act
            var actual = guidProvider.NewGuid();

            //assert
            Assert.NotEqual(Guid.Empty, actual);
        }
    }
}
