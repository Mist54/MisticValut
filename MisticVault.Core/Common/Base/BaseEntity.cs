using System;
using System.Collections.Generic;
using System.Text;

namespace MisticVault.Core.Common.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
