using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using TodoList.Api.Model;
using TodoList.Api.Validators;

namespace TodoList.Api.UnitTests.ValidatorTests
{
    public class Tests
    {
        private TodoItemsValidations validator;
        Mock<TodoContext> mockTodoContext;
        Mock<ITodoItemRepository> mockTodoRespository;


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
            }
        };

        TodoItem validTodoItemRequest = new TodoItem
        {
            Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa2"),
            IsCompleted = false,
            Description = "Just a testing description"
        };
        #endregion

        #region invalidTodoItemList
        List<TodoItem> inValidIncompleteList = new List<TodoItem>()
        {
            new TodoItem
            {
                Description = "Description",
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa2"),
                IsCompleted = false
            },
            new TodoItem
            {
                Description = "Description",
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa2"),
                IsCompleted = false
            },
            new TodoItem
            {
                Description = "Description",
                Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa2"),
                IsCompleted = false
            }
        };

        TodoItem inValidTodoItemRequest = new TodoItem
        {
            Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa2"),
            IsCompleted = false,
            Description = "Description"
        };

        #endregion


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(databaseName: "FekaConnectionString")
            .Options;
            mockTodoContext = new Mock<TodoContext>(options);
            mockTodoRespository = new Mock<ITodoItemRepository>();
            mockTodoContext.Setup<DbSet<TodoItem>>(x => x.TodoItems).ReturnsDbSet(validIncompleteList);

            validator = new TodoItemsValidations(mockTodoRespository.Object);
        }

        [Test]
        public void GivenItemIsValidThenReturn200Ok()
        {
            var model = validTodoItemRequest;
            var result = validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(x => x);
        }

        [Test]
        public void GivenItemIsInvalidThenReturnBadRequest()
        {
            mockTodoContext.Setup<DbSet<TodoItem>>(x => x.TodoItems).ReturnsDbSet(inValidIncompleteList);
            mockTodoRespository.Setup(x => x.TodoItemDescriptionExists(It.IsAny<string>())).ReturnsAsync(true);
            mockTodoRespository.Setup(x => x.TodoItemIdExists(It.IsAny<Guid>())).ReturnsAsync(true);
            var model = inValidTodoItemRequest;
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Id);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
    }
}