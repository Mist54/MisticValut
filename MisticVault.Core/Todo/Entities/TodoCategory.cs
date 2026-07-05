using System;
using System.Collections.Generic;
using System.Text;

namespace MisticVault.Core.Todo.Entities
{
    public class TodoCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = "#CCCCCC";

        public ICollection<Todo> Todos { get; set; } = new List<Todo>();

    }
}
