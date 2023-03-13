using FluentValidation;
using System.Linq;
using System;
using TodoList.Api.Model;
using System.Threading.Tasks;

namespace TodoList.Api.Validators
{
    public class TodoItemsValidations: AbstractValidator<TodoItem>
    {
        private readonly ITodoItemRepository _todoItemRepository;
        public TodoItemsValidations(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("Description is required");

            RuleFor(x => x.Id)
                .MustAsync(async (y, cancellation) =>
                {
                    bool exists = await _todoItemRepository.TodoItemIdExists(y);
                    return !exists;
                })
                .WithMessage("ID already exists");

            RuleFor(x => x.Description)
                .MustAsync(async (y, cancellation) =>
                {
                    bool exists = await _todoItemRepository.TodoItemDescriptionExists(y);
                    return !exists;
                })
                .WithMessage("Description already exists");
        }
    }
}
