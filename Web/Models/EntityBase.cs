using System;

namespace Web.Models
{
    public abstract class EntityBase
    {
        public virtual Guid Id { get; set; }
    }
}
