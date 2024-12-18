using Microsoft.AspNetCore.Mvc;
using PersistenciaContato.Controllers;
using Xunit;

namespace PersistenciaContato.Tests
{
    public class ContatoPersistenciaControllerTests
    {
        [Fact]
        public void Up_ShouldReturnOkWithMessage()
        {
            // Arrange
            var controller = new ContatoPersistenciaController(null);

            // Act
            var result = controller.Up();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("API is running", okResult.Value);
        }
    }
}
