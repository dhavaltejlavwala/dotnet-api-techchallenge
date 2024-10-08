using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ProductCatalog.Api.Controllers;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ProductCatalog.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public void FindUserReturnsCorrectDetails_ForJohn()
        {
            // Act
            var result = new UserController().FindUser();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            objectResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            var userResponseModel = Assert.IsType<UserResponseModel>(objectResult.Value);
            userResponseModel.Name.Should().Be("John Smith");
            userResponseModel.Token.Should().Be("25a4f06f-8fd5-49b3-a711-c013c156f8c8");
        }

        [Fact]
        public async Task UserEndpointIsConfiguredAndReturnsCorrectJsonResponse()
        {
            // Arrange
            var httpClient = new WebApplicationFactory<ProductCatalog.Api.Startup>().Server.CreateClient();

            // Act
            var httpResponseMessage = await httpClient.GetAsync("/user");

            // Assert
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            var readAsStringAsync = await httpResponseMessage.Content.ReadAsStringAsync();
            var userResponseModel = JsonConvert.DeserializeObject<UserResponseModel>(readAsStringAsync);
            userResponseModel.Name.Should().Be("John Smith");
            userResponseModel.Token.Should().Be("25a4f06f-8fd5-49b3-a711-c013c156f8c8");
        }
    }
}