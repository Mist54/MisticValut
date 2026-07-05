using System;
using System.Collections.Generic;
using System.Text;

namespace MisticVault.Core.Todo.Enums
{
    public class TodoEnums
    {
        public enum TodoStatus
        {
            NotStarted,
            InProgress,
            Completed,
            OnHold,
            Cancelled
        }

        public enum TodoPriority
        {
            Low,
            Medium,
            High,
            Critical
        }
    }

}
