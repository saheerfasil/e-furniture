using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Models
{
    public class CartMain
    {
        [Key]
        public int cart_id { get; set; }
        public DateTime cart_date { get; set; }
        public string user_id { get; set; }
        public double total { get; set; }
        public double Discount { get; set; }
        [ForeignKey("DeliveryDetails")]
        public int delivery_id { get; set; }
        public virtual DeliveryDetails DeliveryDetails { get; set; }
    }

    public class CartSub
    {
        [Key]
        public int subcr_id { get; set; }
        [ForeignKey("CartMain")]
        public int cart_id { get; set; }
        public virtual CartMain CartMain { get; set; }
        [ForeignKey("Product")]
        public int pro_id { get; set; }
        public virtual Product Product { get; set; } 
        public int quantity { get; set; }
        public string type { get; set; }
        public string color { get; set; }
        public int total { get; set; }
    }

    public class  DeliveryDetails
    {
        [Key]
        public int Id { get; set; }
        public string billing_first_name { get; set; }
        public string billing_last_name { get; set; }
        public string billing_stret_1 { get; set; }
        public string billing_stret_2 { get; set; }
        public string billing_city { get; set; }
        public string billing_state { get; set; }
        public string billing_country { get; set; }
        public string billing_zip { get; set; }
        public string billing_phone { get; set; }
        public string billing_email { get; set; }
        public string billing_notes { get; set; }
        public string payment_gateway { get; set; } 
        public string user_id { get; set; }

    }
    public class ProductDBContext : DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<CartSub> CartSubs { get; set; }
        public DbSet<CartMain> CartMains { get; set; }
        public DbSet<DeliveryDetails> DeliveryDetails { get; set; }
    }
    
}