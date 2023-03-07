using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Api.Model
{
    public class TodoItemRepository :ITodoItemRepository
    {
        private readonly TodoContext _context;
        public TodoItemRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<TodoItem> AddTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<TodoItem> GetTodoItemById(Guid Id)
        {
            return await _context.TodoItems.FindAsync(Id);
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItems()
        {
            return await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
        }

        public async Task UpdateTodoItem(Guid id, TodoItem todoItem)
        {
            _context.Entry(todoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
