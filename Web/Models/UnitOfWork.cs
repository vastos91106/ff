using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        public bool SaveEntity<T>(T enity) where T : EntityBase
        {
            try
            {
                if (enity.Id == Guid.Empty)
                {
                    enity.Id = Guid.NewGuid();
                    _dbContext.Set<T>().Add(enity);
                }
                else
                {
                    _dbContext.Set<T>().Attach(enity);
                    _dbContext.Entry(enity).State = System.Data.Entity.EntityState.Modified;
                }

                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public bool DeleteEntity<T>(Guid id) where T : EntityBase
        {
            try
            {
                var model = GetEntity<T>(id);

                if (model != null)
                {
                    _dbContext.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                    _dbContext.Set<T>().Remove(model);
                    _dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public T GetEntity<T>(Guid id) where T : EntityBase
        {
            try
            {
                return _dbContext.Set<T>().Find(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<T> GetEntitys<T>() where T : EntityBase
        {
            return _dbContext.Set<T>().ToArray();
        }
    }
}
