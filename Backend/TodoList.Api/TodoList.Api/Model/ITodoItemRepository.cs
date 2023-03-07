using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoList.Api.Model
{
    public interface ITodoItemRepository
    {
        Task<TodoItem> GetTodoItemById(Guid Id);
        Task<TodoItem> AddTodoItem(TodoItem todoItem);
        Task<IEnumerable<TodoItem>> GetTodoItems();
        Task UpdateTodoItem(Guid id, TodoItem todoItem);


    }
}
