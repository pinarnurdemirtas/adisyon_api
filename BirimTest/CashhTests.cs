using adisyon.Controllers;
using adisyon.Data;
using adisyon.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BirimTesti
{
    public class CashTests
    {
        private Mock<ICashDAO> _cashDaoMock;
        private CashController _controller;

        [SetUp]
        public void Setup()
        {
            _cashDaoMock = new Mock<ICashDAO>();
            _controller = new CashController(_cashDaoMock.Object);
        }

        [Test]
        public async Task GetOrdersFromOccupiedTables_WhenNoOrders_ReturnsNotFound()
        {
            
            _cashDaoMock.Setup(dao => dao.GetOrdersFromFullTables()).ReturnsAsync(new List<OrderCash>());

            // Act
            var result = await _controller.GetOrdersFromOccupiedTables();

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task GetOrdersFromOccupiedTables_WhenOrdersExist_ReturnsOrders()
        {
            // Arrange
            var orders = new List<OrderCash> { new OrderCash { Order_id = 1, Status = "Hazırlandı" } };
            _cashDaoMock.Setup(dao => dao.GetOrdersFromFullTables()).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetOrdersFromOccupiedTables();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(orders, okResult.Value);
        }

        [Test]
        public async Task MarkOrdersAsPaidByTable_WhenNoOrders_ReturnsNotFound()
        {
            // Arrange
            int tableNumber = 1;
            _cashDaoMock.Setup(dao => dao.GetOrdersByTableNumber(tableNumber)).ReturnsAsync(new List<OrderCash>());

            // Act
            var result = await _controller.MarkOrdersAsPaidByTable(tableNumber);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task MarkOrdersAsPaidByTable_WhenOrdersExist_UpdatesStatus()
        {
            // Arrange
            int tableNumber = 1;
            var orders = new List<OrderCash>
            {
                new OrderCash { Order_id = 1, Table_number = tableNumber, Status = "Hazırlandı" }
            };

            _cashDaoMock.Setup(dao => dao.GetOrdersByTableNumber(tableNumber)).ReturnsAsync(orders);

            // Act
            var result = await _controller.MarkOrdersAsPaidByTable(tableNumber);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            _cashDaoMock.Verify(dao => dao.UpdateOrders(It.IsAny<IEnumerable<OrderCash>>()), Times.Once);
            _cashDaoMock.Verify(dao => dao.UpdateTableStatus(tableNumber, "Boş"), Times.Once);
        }

        [Test]
        public async Task GetPaidOrdersByDate_WhenNoOrders_ReturnsNotFound()
        {
            // Arrange
            DateTime date = DateTime.Now;
            _cashDaoMock.Setup(dao => dao.GetPaidOrdersByDate(date)).ReturnsAsync(new List<OrderCash>());

            // Act
            var result = await _controller.GetPaidOrdersByDate(date);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task GetPaidOrdersByDate_WhenOrdersExist_ReturnsOrders()
        {
            // Arrange
            DateTime date = DateTime.Now;
            var orders = new List<OrderCash> { new OrderCash { Order_id = 1, Status = "Ödendi", Order_date = date } };
            _cashDaoMock.Setup(dao => dao.GetPaidOrdersByDate(date)).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetPaidOrdersByDate(date);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(orders, okResult.Value);
        }
    }
}
