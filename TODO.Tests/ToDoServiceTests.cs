using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODO.Contacts.Repository;
using TODO.Contacts.Services;
using TODO.Models;
using TODO.Repo;
using TODO.Repo.Models;
using TODO.Repo.Repository;
using TODO.Service;

namespace TODO.Tests
{
    public class ToDoServiceTests
    {
        private TodoModel _todoModel;
        private IRepo<ToDo> _todoRepo;
        private ITodoService _todoService;
        [SetUp]
        public void Setup()
        {
            _todoModel = new TodoModel { TaskName = "testTask", UserId = 1 };
            _todoRepo = new GenericRepo<ToDo>(GetDatabaseContext());
            _todoService = new ToDoService(_todoRepo);
        }

        [Test]
        public async Task TestForAddTodo()
        {
            // Act
            _todoModel.TaskId = await _todoService.Add(_todoModel.TaskName, _todoModel.UserId);

            Assert.IsTrue(_todoModel.TaskId > 0);
        }

        [Test]
        public async Task TestForListAllTodo()
        {
            // Arrange
            _todoModel.TaskId = await _todoService.Add(_todoModel.TaskName, _todoModel.UserId);

            // Act
            var result = await _todoService.GetAll(_todoModel.UserId);
            Assert.IsTrue(result.Any());
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
