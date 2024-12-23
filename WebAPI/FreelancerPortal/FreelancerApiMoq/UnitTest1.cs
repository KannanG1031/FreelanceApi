using Castle.Core.Logging;
using FreelancerPortal.Models;
using FreelancerPortal.Repository;
using FreelancerPortal.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace FreelancerApiMoq
{
    public class UserServiceTest
    {
        private readonly UserService _usrUT;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly Mock<IUserRepository> _mockUserRepository;
        public UserServiceTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<UserService>>();

            _usrUT = new UserService(_mockUserRepository.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task GetAllUsersAsyn_ShouldReturnAllCustomers()
        {
            //Arrange

            var users = new List<User> {
             new User { Id = 1, Username = "GK", Email = "GK@example.com",PhoneNumber="1234555" },
            new User { Id = 2, Username = "RGK", Email = "Kannan@example.com" ,PhoneNumber="5646467"}
            };

            _mockUserRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _usrUT.GetAllUsersAsync();


            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockUserRepository.Verify(repo => repo.GetAllUsersAsync(), Times.Once);
        }



    }
}
