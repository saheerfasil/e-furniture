using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFM1.Models
{
    public interface IMyCartRepository
    {
        void AddMyCart(MyCart myCart);
    }
}
