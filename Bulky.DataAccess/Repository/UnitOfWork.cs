using Bulky.DataAccess.Repository.IRepository;
using Bulkyweb.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;
        public ICategoryRepository Category { get; set; }
        public IProductRepository Product { get; set; }

        public UnitOfWork(ApplicationDbContext _db)
        {
            db = _db;
            Category = new CategoryRepository(db);
            Product = new ProductRepository(db);
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}
