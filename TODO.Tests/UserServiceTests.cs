using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TODO.Contacts.Repository;
using TODO.Contacts.Services;
using TODO.Repo;
using TODO.Repo.Models;
using TODO.Repo.Repository;
using TODO.Service;

namespace TODO.Tests
{
    public class UserServiceTests
    {
 
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestForUserWithValidUserId()
        {
            // Arrange
            var userId = 1;
            IRepo<User> userRepo = new GenericRepo<User>(GetDatabaseContext());
            IUserService userService = new UserService(userRepo);

            // Act
            var result = await userService.GetUserByid(userId);

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task TestForUserWithInvalidValidUserId()
        {
            // Arrange
            var userId = 0;
            IRepo<User> userRepo = new GenericRepo<User>(GetDatabaseContext());
            IUserService userService = new UserService(userRepo);

            // Act
            var result = await userService.GetUserByid(userId);
            Assert.IsNull(result);
        }
        [Test]
        public async Task TestForUserLoginWithValidUserNameAndPassword()
        {
            // Arrange
            IRepo<User> userRepo = new GenericRepo<User>(GetDatabaseContext());
            IUserService userService = new UserService(userRepo);

            // Act
            var result = await userService.Login(new Models.UserModel { UserName = "test", Password = "pwd123" });
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task TestForUserLoginWithInvalidValidUserNameAndPassword()
        {
            // Arrange
            IRepo<User> userRepo = new GenericRepo<User>(GetDatabaseContext());
            IUserService userService = new UserService(userRepo);

            // Act
            var result = await userService.Login(new Models.UserModel { UserName = "admin", Password = "test" });
            Assert.IsNull(result);
        }

        private ToDoDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ToDoDbContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }
       
    }
}