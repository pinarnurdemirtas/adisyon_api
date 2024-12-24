using adisyon.Controllers;
using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace BirimTesti
{
    public class KitchenTests
    {
        private Mock<IKitchenDAO> _kitchenDaoMock;
        private KitchenController _controller;

        [SetUp]
        public void Setup()
        {
            _kitchenDaoMock = new Mock<IKitchenDAO>();
            _controller = new KitchenController(_kitchenDaoMock.Object);
        }

        [Test]
        public async Task GetPreparingOrders_WhenNoOrders_ReturnsNotFound()
        {
            // Arrange
            _kitchenDaoMock.Setup(dao => dao.GetOrdersByStatusAsync("Hazırlanıyor")).ReturnsAsync(new List<Orders>());

            // Act
            var result = await _controller.GetPreparingOrders();

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task GetPreparingOrders_WhenOrdersExist_ReturnsOrders()
        {
            // Arrange
            var orders = new List<Orders> { new Orders { Order_id = 1, Status = "Hazırlanıyor" } };
            _kitchenDaoMock.Setup(dao => dao.GetOrdersByStatusAsync("Hazırlanıyor")).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetPreparingOrders();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(orders, okResult.Value);
        }

        [Test]
        public async Task UpdateOrderStatus_WhenOrderNotFound_ReturnsNotFound()
        {
            // Arrange
            int orderId = 1;
            _kitchenDaoMock.Setup(dao => dao.GetOrderByIdAsync(orderId)).ReturnsAsync((Orders)null);

            // Act
            var result = await _controller.UpdateOrderStatus(orderId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task UpdateOrderStatus_WhenProductNotFound_ReturnsNotFound()
        {

            int orderId = 1;
            var mockOrder = new Orders { Order_id = orderId, Product_id = 1, Quantity = 2, Status = "Hazırlanıyor" };
            _kitchenDaoMock.Setup(dao => dao.GetOrderByIdAsync(orderId)).ReturnsAsync(mockOrder);
            _kitchenDaoMock.Setup(dao => dao.GetProductByIdAsync(mockOrder.Product_id)).ReturnsAsync((Products)null);

            // Act
            var result = await _controller.UpdateOrderStatus(orderId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
    }
}
