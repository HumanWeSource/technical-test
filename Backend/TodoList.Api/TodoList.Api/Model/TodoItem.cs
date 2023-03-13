using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Api.Model
{
    public class TodoItem
    {
        [Required]
        public Guid Id { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
