using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using TodoList.Api.Controllers;
using TodoList.Api.Model;

namespace TodoList.Api.UnitTests.ControllerTests
{
    public class ControllerTests
    {
        Mock<ILogger<TodoItemsController>> mockLogger;
        Mock<ITodoItemRepository> mockTodoRepository;
        
        #region validTodoItemList
        List<TodoItem> validIncompleteList = new List<TodoItem>()
        {
            new TodoItem
            {
                Description = "Description",
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                IsCompleted = false
            },
            new TodoItem
            {
                Description = "Description4",
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa2"),
                IsCompleted = false
            },
            new TodoItem
            {
                Description = "Description3",
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa2"),
                IsCompleted = false
            },
            new TodoItem
            {
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afb9"),
                IsCompleted = false,
                Description = "Just a testing description"
            }
        };

        TodoItem validTodoItemRequest = new TodoItem
        {
            Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa2")
        };
        TodoItem notFoundTodoItemRequest = new TodoItem
        {
            Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afb9")
        };
        TodoItem dbUpdateConcurrencyExceptionRequest = new TodoItem
        {
            Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afb9")
        };

        TodoItem validTodoItemrResponse = new TodoItem
        {
            Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa2"),
            IsCompleted = false,
            Description = "Just a testing description"
        };
        TodoItem validTodoItemrRequest = new TodoItem
        {
            Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afb9"),
            IsCompleted = false,
            Description = "Just a testing description"
        };
        #endregion


        [SetUp]
        public void Setup()
        {
            mockLogger = new Mock<ILogger<TodoItemsController>>();
            mockTodoRepository = new Mock<ITodoItemRepository>();
        }

        [Test]
        public async Task GivenCorrectIdThenReturnCorrectTodoItem()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
               .UseInMemoryDatabase(databaseName: "FekaConnectionString")
               .Options;
            var mockTodoContext = new Mock<TodoContext>(options);

            mockTodoContext.Setup<DbSet<TodoItem>>(x => x.TodoItems).ReturnsDbSet(validIncompleteList);

            mockTodoRepository.Setup(x => x.GetTodoItemById(It.IsAny<Guid>())).ReturnsAsync(validTodoItemrResponse);

            var controller = new TodoItemsController(mockTodoContext.Object, mockLogger.Object, mockTodoRepository.Object);

            var result = await controller.GetTodoItem(validTodoItemRequest.Id);
            var objectResult = result as OkResult;
            var statusCodeResult = result as StatusCodeResult;
            Assert.IsNotNull(result);
            
        }

        [Test]
        public async Task GivenIncorrectTodoItemThenThrowExceptionNotFound()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: "FekaConnectionString")
                .Options;
            var mockTodoContext = new Mock<TodoContext>(options);

            mockTodoContext.Setup<DbSet<TodoItem>>(x => x.TodoItems).ReturnsDbSet(validIncompleteList);
            var controller = new TodoItemsController(mockTodoContext.Object, mockLogger.Object, mockTodoRepository.Object);

            var result = await controller.GetTodoItem(notFoundTodoItemRequest.Id);
            var objectResult = result as StatusCodeResult;
            Assert.AreEqual(404, objectResult.StatusCode);
        }

        [Test]
        public async Task GivenIncorrectTodoItemThenThrpwDbUpdateConcurrencyException()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: "FekaConnectionString")
            .Options;
            var mockTodoContext = new Mock<TodoContext>(options);

            mockTodoContext.Setup<DbSet<TodoItem>>(x => x.TodoItems).ReturnsDbSet(validIncompleteList);
            mockTodoRepository.Setup(x => x.UpdateTodoItem(It.IsAny<Guid>(), It.IsAny<TodoItem>())).ThrowsAsync(new DbUpdateConcurrencyException());
            var controller = new TodoItemsController(mockTodoContext.Object, mockLogger.Object, mockTodoRepository.Object);

            var result =  controller.PutTodoItem(dbUpdateConcurrencyExceptionRequest.Id, validTodoItemrRequest);
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await result);
        }
    }
}
