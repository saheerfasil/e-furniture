using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Models
{ 
    public class CartMain
    {
        public int cart_id { get; set; }
        public DateTime cart_date { get; set; }
        public  int user_id { get; set; }
        public double total { get; set; }
        public double Discount { get; set; }
    }

    public class CartSub
    {
        public int subcr_id { get; set; }
        public int cart_id { get; set; }
        public int pro_id { get; set; }
        public int quantity { get; set; }
        public int total { get; set; }
    }

    public class ProductDBContext : DbContext
    {

        public DbSet<Product> Products { get; set; }
    }
    
}