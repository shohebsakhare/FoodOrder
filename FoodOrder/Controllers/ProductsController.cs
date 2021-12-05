/***************** DEVLOPER AND PAGE INFO **********************/
//
//
//
//Created By GithubSource
//Update By Shoheb on 29-11-2021 for adding comments
//Product controller for managing products for admin side and for user side it is used to view and add products to cart
//
//
//
/***************** DEVLOPER AND PAGE INFO **********************/

using FoodWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodWeb.Controllers
{
    public class ProductsController : Controller
    {
        AppFoodDbContext db = new AppFoodDbContext();
        // GET: Products
        public ActionResult Index()
        {
            //Query products from database and show in list on page lod
                List<Products> products = db.Products.ToList<Products>();
                return View(products);
           
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            //Delete record of product from database
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }
        [HttpGet]
        public ActionResult CreateNewProduct()
        {
            //Display page to create new record of product
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                //If cookie exists show page
                return View();
            }
            else
            {
                //If no cookie redirect to login
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
           
        }
        [HttpPost]
        public ActionResult CreateNewProduct(HttpPostedFileBase file , Products products)
        {
            //save uploaded pic of product in local file
            //Check if user uploaded file
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Images"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    string filename= file.FileName;
                   
                    products.ProductPicture = "Images/"+ filename;

                    //Save product data in database
                    db.Products.Add(products);
                    db.SaveChanges();
                    ViewBag.Message = "Data uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                //if no file record will not be create user will get following message
                ViewBag.Message = "You have not specified a file.";
            } 
            return View();
        }
    
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            //Open product page in edit view of particular product
            var adminInCookie = Request.Cookies["AdminInfo"];
            //check user present in cookie
            if (adminInCookie != null)
            {
                //Query from database
                Products products = db.Products.Find(id);
                if (products == null)
                {
                    return HttpNotFound();
                }
                return View(products);
            }
            else
            {
                //Logout if user not present
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
            
        }
        [HttpPost]
        public ActionResult EditProduct(HttpPostedFileBase file, Products products)
        {
            //Save product data after edit and update in database
            //Check if user uploaded file
            if (file != null && file.ContentLength > 0)
                try
                {
                    //save new pic uploaded
                    string path = Path.Combine(Server.MapPath("~/Images"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    string filename = file.FileName;
                    
                    products.ProductPicture = "Images/" + filename;

                    //save data in db
                    db.Entry(products).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Data uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                //if no file record will not be create user will get following message
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
        }
        public ActionResult ViewProductsAdmin()
        {
            //View product data
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                List<Products> products = db.Products.ToList<Products>();
                return View(products);
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
           
        }
        #region UserActions
        //Start User actions
        public ActionResult addToCart(int? Id)
        {
            //Dispplay product page
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    //Find product in db
                    Products products = db.Products.Find(Id);
                    return View(products);

                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
        
        }

        List<Cart> li = new List<Cart>();

        [HttpPost]
        public ActionResult addToCart(int Id, string number)
        {
            //Add product to cart
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null)
            {
                //Product is saved temporary in TempData
                Products products = db.Products.Find(Id);
                Cart cart = new Cart();
                cart.productId = products.id;
                cart.productName = products.ProductName;
                cart.productPic = products.ProductPicture;
                cart.price = products.ProductPrice;
                cart.qty = Convert.ToInt32(number);
                cart.bill = cart.price * cart.qty;
                if (TempData["cart"] == null)
                {
                    //Save first product
                    li.Add(cart);
                    TempData["cart"] = li;
                }
                else
                {
                    //Save rest products
                    List<Cart> li2 = TempData["cart"] as List<Cart>;
                    int flag = 0;
                    foreach(var item in li2)
                    {
                        if(item.productId == cart.productId)
                        {
                            item.qty += cart.qty;
                            item.bill += cart.bill;
                            flag = 1;
                        }
                    }
                    if(flag==0)
                    {
                        li2.Add(cart);
                    }
                    TempData["cart"] = li2;
                }

                TempData.Keep();
                return RedirectToAction("Index");

            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public ActionResult Checkout()
        {
            //Display Checkout Page

            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    //Calculate total and insert in tempdata
                    TempData.Keep();
                    if (TempData["cart"] != null)
                    {
                        float x = 0;
                        List<Cart> li2 = TempData["cart"] as List<Cart>;
                        foreach (var item in li2)
                        {
                            x += item.bill;
                        }
                        TempData["total"] = x;
                    }
                    TempData.Keep();
                    return View();

                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }

           
        }
        [HttpPost]
        public ActionResult Checkout(Order order)
        {
            //Redirect to payment gateway page 
            return Redirect("/Payment/Index");
        }
        public ActionResult Remove(int? id)
        {
            //Remove product from cart
            List<Cart> li2 = TempData["cart"] as List<Cart>;
            Cart c = li2.Where(x => x.productId == id).SingleOrDefault();
            li2.Remove(c);
            float h = 0;
            //Recalculate amount for billing
            foreach(var item in li2)
            {
                h += item.bill;
            }
            
            TempData["total"] = h;
            return RedirectToAction("Checkout");
        }
        //End User actions
        #endregion
    }
}