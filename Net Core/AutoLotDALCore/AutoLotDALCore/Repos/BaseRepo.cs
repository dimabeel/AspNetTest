using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoLotDALCore.EF;
using AutoLotDALCore.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AutoLotDALCore.Repos
{
    class BaseRepo<T> : IDisposable, IRepo<T> where T:EntityBase, new()
    {
        public BaseRepo() : this(new AutoLotContext())
        {

        }

        public BaseRepo(AutoLotContext context)
        {
            this.context = context;
            table = context.Set<T>();
        }

        public int Add(T entity)
        {
            table.Add(entity);
            return SaveChanges();
        }

        public int Add(IList<T> entities)
        {
            table.AddRange(entities);
            return SaveChanges();
        }

        public int Update(T entity)
        {
            table.Update(entity);
            return SaveChanges();
        }

        public int Update(IList<T> entities)
        {
            table.UpdateRange(entities);
            return SaveChanges();
        }

        public int Delete(int id, byte[] timeStamp)
        {
            context.Entry(new T()
            {
                Id = id,
                Timestamp = timeStamp,
            }).State = EntityState.Deleted;
            return SaveChanges();
        }

        public int Delete(T entity)
        {
            context.Entry(entity).State = EntityState.Deleted;
            return SaveChanges();
        }

        internal int SaveChanges()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException _)
            {
                // Parallelism err
                throw;
            }
            catch (RetryLimitExceededException _)
            {
                // Max attempts count
                throw;
            }
            catch (DbUpdateException _)
            {
                // Err during update DB.
                throw;
            }
            catch (Exception _)
            {
                // Another one exception
                throw;
            }
        }

        public void Dispose()
        {
            context?.Dispose();
        }

        public List<T> GetSome(Expression<Func<T, bool>> where) =>
            table.Where(where).ToList();

        public T GetOne(int? id) => table.Find(id);

        public virtual List<T> GetAll() => table.ToList();

        public List<T> GetAll<TSortField>(
            Expression<Func<T, TSortField>> orderBy, bool ascending) =>
            (ascending ? table.OrderBy(orderBy) : table.OrderByDescending(orderBy))
            .ToList();

        protected AutoLotContext Context => context;

        private readonly DbSet<T> table;
        private readonly AutoLotContext context;
    }
}
