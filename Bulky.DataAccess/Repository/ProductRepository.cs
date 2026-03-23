using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulkyweb.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext db;
        public ProductRepository(ApplicationDbContext _db) : base(_db) 
        {
            db = _db;
        }
        public void Update(Product obj)
        {
            var objFromDb = db.Products.FirstOrDefault(d => d.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Author = obj.Author;
                objFromDb.Price = obj.Price;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Price50 = obj.Price50;
                objFromDb.CategoryId = obj.CategoryId;

                if (objFromDb.ImageURl != null)
                {
                    objFromDb.ImageURl = obj.ImageURl;
                }

            }
        }
    }
}
