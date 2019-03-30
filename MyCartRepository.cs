using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFM1.Models
{
    public class MyCartRepository :IMyCartRepository
    {
        private readonly AppDbContext _appDbContext;

        public MyCartRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddMyCart(MyCart myCart)
        {
            _appDbContext.MyCarts.Add(myCart);
            _appDbContext.SaveChanges();
        }
    }
}
