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
    public class ShoppingCartRepository : Repository<ShoppingCart> , IShoppingCartRepository
    {
        private readonly ApplicationDbContext db;
        public ShoppingCartRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }
        public void Update(ShoppingCart obj)
        {
            db.ShoppingCarts.Update(obj);
        }
    }
}
