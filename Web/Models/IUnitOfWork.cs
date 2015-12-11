using System;
using System.Collections.Generic;

namespace Web.Models
{
    public interface IUnitOfWork
    {
        bool SaveEntity<T>(T enity) where T : EntityBase;

        bool DeleteEntity<T>(Guid id) where T : EntityBase;

        T GetEntity<T>(Guid id) where T : EntityBase;

        IEnumerable<T> GetEntitys<T>() where T : EntityBase;
    }
}