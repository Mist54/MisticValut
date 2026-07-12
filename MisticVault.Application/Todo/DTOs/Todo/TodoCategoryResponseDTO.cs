using System;
using System.Collections.Generic;
using System.Text;

namespace MisticVault.Application.Todo.DTOs.Todo
{
    public class TodoCategoryResponseDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Color { get; set; } = "#CCCCCC";
    }
}
