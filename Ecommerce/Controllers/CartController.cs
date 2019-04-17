using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ecommerce.Models;
using Ecommerce.Helpers;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Membership.OpenAuth;
using System.Collections.Specialized;
using DotNetOpenAuth.GoogleOAuth2;
using PayPal.Api;

namespace Ecommerce.Controllers
{

    public class ToDoItem
    { 
        public int quantity { get; set; }
        public string selColor { get; set; } 
        public string type { get; set; }
        public DateTime date_added { get; set; } 
    }
    public class CartController : Controller
    {
        private ProductDBContext db = new ProductDBContext();

        // GET: Cart
        public ActionResult Index()
        {
            GlobalMethods.MaybeInitializeSession();
            
            /* Query all products by list of cart item IDs stored in session. */
            var cart_items = db.Products.Where(item => SessionSingleton.Current.Cart.Keys.Contains(item.id));

            ViewBag.Session = SessionSingleton.Current.Cart;

            return View(cart_items.ToList());
        }
        

             [HttpGet]
        public ActionResult Cash()
        {
            
            return View();
        }
        // GET: Cart
        [HttpGet]
        public ActionResult Checkout()
        {
            var user = (ApplicationUser)Session["user"];
            if (user == null)
            {
                return RedirectToAction("Login", "Account");

            }
            GlobalMethods.MaybeInitializeSession(); 
            CartViewModel vm = new CartViewModel(); 
            /* Query all products by list of cart item IDs store in session. */
            var cart_items = db.Products.Where(item => SessionSingleton.Current.Cart.Keys.Contains(item.id));
            vm.Products = cart_items.ToList();
            ViewBag.Session = SessionSingleton.Current.Cart;
            vm.DeliveryDetails = new DeliveryDetails();
            vm.cartmain = new CartMain();
            return View(vm);
        }
        
        [HttpPost]
        public ActionResult Checkout(FormCollection collection)
        {
            /* Add your logic for handling the sending of payment to the selected payment gateway. */
            ProductDBContext db = new ProductDBContext();

            DeliveryDetails deldet = new DeliveryDetails();
            deldet.billing_city = collection["billing_city"];
            deldet.billing_country = collection["billing_country"];
            deldet.billing_email = collection["billing_email"];
            deldet.billing_first_name = collection["billing_first_name"];
            deldet.billing_last_name = collection["billing_last_name"];
            deldet.billing_notes = collection["billing_notes"];
            deldet.billing_phone = collection["billing_phone"];
            deldet. billing_state= collection["billing_state"];
            deldet.billing_stret_1 = collection["billing_stret_1"];
            deldet.billing_stret_2 = collection["billing_stret_2"];
            deldet.billing_zip = collection["billing_zip"];
            deldet.payment_gateway = collection["payment_gateway"];            
            db.DeliveryDetails.Add(deldet);
            CartMain main = new CartMain();
            main.DeliveryDetails = deldet;
            main.cart_date = DateTime.Now;
            main.Discount = 0;
            db.CartMains.Add(main);
            CartSub sub = new CartSub();
            var cart_items = db.Products.Where(item => SessionSingleton.Current.Cart.Keys.Contains(item.id));

            var items = SessionSingleton.Current.Cart;
            foreach (var item in items)
            {
                sub = new CartSub();
                var its = item.Value;

                JavaScriptSerializer js = new JavaScriptSerializer();
                var  persons = js.Deserialize<ToDoItem>(its);
                sub.pro_id = item.Key;
                sub.quantity = persons.quantity;
                sub.type = persons.type;
                sub.color = persons.selColor;
                db.CartSubs.Add(sub);

            }

            db.SaveChanges();
            SessionSingleton.setclear();
            if (deldet.payment_gateway== "paypal")
            {
                
                    return RedirectToAction("PaymentWithPaypal", "Cart");
            }
            return RedirectToAction("Cash", "Cart");
        }
    
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Actions(FormCollection collection)
        {
            GlobalMethods.MaybeInitializeSession();

            string msg = "";
            int status = 0;
            int item_id = Convert.ToInt32(collection["item_id"]);
            var item = db.Products.First(i => i.id == item_id);
            var session = SessionSingleton.Current.Cart;

            if(item != null) { 
                /* Add Item to Cart */
                if (collection["action"] == "add-item-to-cart")
                {
                    if(session.ContainsKey(item_id))
                    {
                        msg = "Item already exist in Cart";
                    }
                    else if (string.IsNullOrEmpty(collection["quantity"]))
                    {
                        msg = "Quantity must be greater than zero";
                    }
                    else
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            quantity = collection["quantity"],
                            selColor = collection["selColor"],
                            type = collection["type"],
                            date_added = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
                        });
                        session.Add(item_id, json);
                        msg = "Item successfully added to cart";
                        status = 1;
                    }
                }

                /* Remove Item from Cart */
                if (collection["action"] == "remove-item-from-cart")
                {
                    if (session.ContainsKey(item_id))
                    {
                        session.Remove(item_id);
                        msg = "Item successfully removed from cart";
                        status = 1;
                    }
                    else
                    {
                        msg = "Item does not exist in cart";
                    }
                }

                /* Update Item Quantity in Cart */
                if (collection["action"] == "update-item-quantity-in-cart")
                {
                    if (!session.ContainsKey(item_id))
                    {
                        msg = "Item does not exist in cart";
                    }
                    else if (string.IsNullOrEmpty(collection["quantity"]))
                    {
                        msg = "Quantity must be greater than zero";
                    }
                    else if (Convert.ToInt32(collection["quantity"]) > item.quantity)
                    {
                        msg = "Product: " + item.name + " only have '" + item.quantity + "' items left in stock";
                    }
                    else
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            quantity = collection["quantity"],
                            date_added = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
                        });

                        session[item_id] = json;
                        msg = "Quantity successfully updated";
                        status = 1;
                    }
                }
            }
            else
            {
                msg = "The item you are trying to process is invalid";
            }

            dynamic query_string = new { status = status, msg = HttpUtility.UrlEncode(msg) };

            if (collection["redirect_page"] != null)
            {
                if (collection["redirect_page"] == "Cart")
                {
                    return RedirectToAction("Index", "Cart", query_string);
                }
                else if (collection["redirect_page"] == "Single")
                {
                    return RedirectToAction("Details/" + item_id, "Products", query_string);
                }
            }
            
            return RedirectToAction("Index", "Products", query_string);
        }
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Name comes here",
                currency = "USD",
                price = "1",
                quantity = "1",
                sku = "sku"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = "3", // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = "your generated invoice number", //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
    }
}
