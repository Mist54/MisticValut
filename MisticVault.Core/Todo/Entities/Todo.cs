using System;
using System.Collections.Generic;
using System.Text;
using MisticVault.Core.Todo.Enums;

namespace MisticVault.Core.Todo.Entities
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public TodoEnums.TodoStatus Status { get; set; }
        public TodoEnums.TodoPriority Priority { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public Guid CategoryId { get; set; }

        public TodoCategory? Category { get; set; }

        public Todo() { }
    }
}
