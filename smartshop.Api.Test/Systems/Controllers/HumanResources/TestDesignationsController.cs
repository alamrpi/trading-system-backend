using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using smartshop.Api.Controllers.HumanResources;
using smartshop.Api.Test.MoqData.HumanResources;
using smartshop.Business.IServices;
using smartshop.Common.QueryParams;

namespace smartshop.Api.Test.Systems.Controllers.HumanResources
{
    public class TestDesignationsController
    {
        [Fact]
        public async Task Get_shouldReturn200Status()
        {
            // Arrange
            var _designationService = new Mock<IDesignationService>();
            _designationService.Setup(x => x.Get(1, 1, 30)).Returns(DesignationMockData.Gets());

            var sut = new DesignationsController(_designationService.Object);

            // Act
            var result = (ObjectResult) sut.Get(new PaginateQueryParams { Page = 1, Size = 30 }).Result;

            // Assert
            result.StatusCode.Should().Be(200);
        }
    }
}
