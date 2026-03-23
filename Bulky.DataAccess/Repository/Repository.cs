using Bulky.DataAccess.Repository.IRepository;
using Bulkyweb.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext db;
        public readonly DbSet<T> dbSet;
        public Repository(ApplicationDbContext _db)
        {
            db = _db;
            this.dbSet = db.Set<T>();
            db.Products.Include(p => p.Category).Include(p => p.CategoryId);
        }
        public void Add(T Entity)
        {
            dbSet.Add(Entity);
        }
        public T Get(Expression<Func<T, bool>> Filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(Filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }
            return query.ToList();
        }

        public void Remove(T Entity)
        {
            db.Remove(Entity);
        }

        public void RemoveRange(IEnumerable<T> Entity)
        {
            db.RemoveRange(Entity);
        }
    }
}
