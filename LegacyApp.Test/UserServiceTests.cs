using LegacyApp.Imeplenentations;
using LegacyApp.Interfaces;
using LegacyApp.Models;
using Moq;

namespace LegacyApp.Test
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IClientRepository> _clientRepositoryMock;
        private Mock<IUserCreditService> _userCreditServiceClientMock;
        private Mock<IUserDataAccessWrapper> _userDataAccessWrapperMock;
        private IUserService _userService;
        private static readonly string VeryImportantClient = "VeryImportantClient";
        private static readonly string ImportantClient = "ImportantClient";
        private static readonly string RegularClient = "RegularClient";

        [SetUp]
        public void SetUp()
        {
            _clientRepositoryMock = new Mock<IClientRepository>();
            _userCreditServiceClientMock = new Mock<IUserCreditService>();
            _userDataAccessWrapperMock = new Mock<IUserDataAccessWrapper>();

            _userService = new UserService(_clientRepositoryMock.Object, 
                _userCreditServiceClientMock.Object, 
                _userDataAccessWrapperMock.Object);
        }

        [Test]
        public void AddUser_InvalidFirstNameOrSurname_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => _userService.AddUser("", "Varier", "t@t.com", new DateTime(2000, 1, 1), 1));
            Assert.ThrowsAsync<InvalidOperationException>(() => _userService.AddUser("Mahes", "", "t@t.com", new DateTime(2000, 1, 1), 1));
        }

        [Test]
        public void AddUser_InvalidEmail_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => _userService.AddUser("Mahes", "Varier", "invalidemail", new DateTime(2000, 1, 1), 1));
        }

        [Test]
        public void AddUser_UserUnder21_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(() => _userService.AddUser("Mahes", "Varier", "t@t.com", DateTime.Now.AddYears(-20), 1));
        }

        [Test]
        public async Task AddUser_VeryImportantClient_SkipsCreditCheck()
        {
            var client = new Client { Name = VeryImportantClient };
            _clientRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(client);

            var user = await _userService.AddUser("Mahes", "Varier", "t@t.com", new DateTime(2000, 1, 1), 1);

            Assert.IsFalse(user.HasCreditLimit);
        }

        [Test]
        public async Task AddUser_ImportantClient_DoublesCreditLimit()
        {
            var client = new Client { Name = ImportantClient };
            _clientRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(client);
            _userCreditServiceClientMock.Setup(service => service.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(500);

            var user = await _userService.AddUser("Mahes", "Varier", "t@t.com", new DateTime(2000, 1, 1), 1);

            Assert.IsTrue(user.HasCreditLimit);
            Assert.That(user.CreditLimit, Is.EqualTo(1000));
        }

        [Test]
        public async Task AddUser_RegularClient_PerformsCreditCheck()
        {
            var client = new Client { Name = RegularClient };
            _clientRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(client);
            _userCreditServiceClientMock.Setup(service => service.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(500);

            var user = await _userService.AddUser("Mahes", "Varier", "t@t.com", new DateTime(2000, 1, 1), 1);

            Assert.IsTrue(user.HasCreditLimit);
            Assert.That(user.CreditLimit, Is.EqualTo(500));
        }

        [Test]
        public void AddUser_InsufficientCreditLimit_ThrowsInvalidOperationException()
        {
            var client = new Client { Name = RegularClient };
            _clientRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(client);
            _userCreditServiceClientMock.Setup(service => service.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).Returns(400);

            Assert.ThrowsAsync<InvalidOperationException>(() => _userService.AddUser("Mahes", "Varier", "t@t.com", new DateTime(2000, 1, 1), 1));
        }
    }
}
