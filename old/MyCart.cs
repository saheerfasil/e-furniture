using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFM1.Models
{
    public class MyCart
    {
         public int MyCartId { get; set; }

         public int FurnitureId { get; set; }

         public decimal Price { get; set; }

         public string ShippingAdd { get; set; }

         public bool Desp { get; set; }
    }
}
