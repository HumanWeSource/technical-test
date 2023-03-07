using FluentValidation;
using System.Linq;
using System;
using TodoList.Api.Model;
using System.Threading.Tasks;

namespace TodoList.Api.Validators
{
    public class TodoItemsValidations: AbstractValidator<TodoItem>
    {
        private readonly TodoContext _context;
        public TodoItemsValidations(TodoContext context)
        {
            _context = context;
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("Description is required");

            RuleFor(x => x.Id)
                .MustAsync(async (y, cancellation) =>
                {
                    bool exists = await TodoItemIdExists(y);
                    return !exists;
                })
                .WithMessage("ID already exists");

            RuleFor(x => x.Description)
                .MustAsync(async (y, cancellation) =>
                {
                    bool exists = await TodoItemDescriptionExists(y);
                    return !exists;
                })
                .WithMessage("Description already exists");
        }
        protected async Task<bool> TodoItemIdExists(Guid id)
        {
            return _context.TodoItems.Any(x => x.Id == id);
        }
        protected async Task<bool> TodoItemDescriptionExists(string description)
        {
            return _context.TodoItems
                   .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }
    }
}
