using adisyon.Controllers;
using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BirimTesti
{
    public class WaiterTests
    {
        private Mock<IWaiterDAO> _waiterDaoMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private WaiterController _controller;

        [SetUp]
        public void Setup()
        {
            _waiterDaoMock = new Mock<IWaiterDAO>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            // Mocking the HttpContext and User claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1") // Setting a user ID for the current user
            };

            var identity = new ClaimsIdentity(claims, "Test");
            var user = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext { User = user };

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            // Creating the controller and injecting mocks
            _controller = new WaiterController(_waiterDaoMock.Object, _httpContextAccessorMock.Object);
        }

        [Test]
        public async Task CreateOrder_WhenProductNotFound_ReturnsBadRequest()
        {

            var order = new CreateOrder { Product_id = 1, Quantity = 2, Table_number = 3 };
            _waiterDaoMock.Setup(dao => dao.GetProductById(order.Product_id))
                .ReturnsAsync((Products)null); // Simulating product not found

            // Act
            var result = await _controller.CreateOrder(order);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result); // Check if BadRequest is returned
        }
    }
}
