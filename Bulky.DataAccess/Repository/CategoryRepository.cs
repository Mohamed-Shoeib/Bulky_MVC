using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulkyweb.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category> , ICategoryRepository
    {
        public readonly ApplicationDbContext db;
        public CategoryRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }
        public void Update(Category obj)
        {
            db.Categories.Update(obj);
        }
    }
}
