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
    public class OrderDetailRepository : Repository<OrderDetail> , IOrderDetailRepository
    {
        public readonly ApplicationDbContext db;
        public OrderDetailRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }
        public void Update(OrderDetail obj)
        {
            db.OrderDetails.Update(obj);
        }
    }
}
