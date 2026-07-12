using MisticVault.Core.Todo.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MisticVault.Core.Todo.DTOs.Todo
{
    public class TodoResponseDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public TodoEnums.TodoStatus Status { get; set; }

        public TodoEnums.TodoPriority Priority { get; set; }

        public TodoCategoryResponseDTO? Category { get; set; }
    }
}
