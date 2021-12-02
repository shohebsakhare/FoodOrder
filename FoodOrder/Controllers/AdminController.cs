/***************** DEVLOPER INFO **********************/
//
//
//
//Created By GithubSource
//Update By Shoheb on 29-11-2021 for adding comments and adding method to update location of order
//controller for Login,Ivoice details,order details,location update for admin
//
//
//
/***************** DEVLOPER INFO **********************/
using FoodWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodWeb.Controllers
{
    public class AdminController : Controller
    {
        AppFoodDbContext db = new AppFoodDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                //If  cookie found redirect to dashboard 
                return View();
            }
            else
            {
                //check cookie belonging to customer or admin and redirect 
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    //User redirect
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    //Admin redirect to login as admin cookie not found
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }

        }
        [HttpGet]
        public ActionResult LoginAdmin()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                //If  cookie found redirect to dashboard 
                return RedirectToAction("Index", "Admin"); ;
            }
            else
            {
                //check cookie belonging to customer or admin and redirect 
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null && userInCookie.Value != "")
                {
                    //User redirect
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    //Admin redirect to login as admin cookie not found
                    return View();
                }
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAdmin(AdminLogin model)
        {
            //check login details and validate from database

            var data = db.adminLogin.Where(s => s.Email.Equals(model.Email) && s.Password.Equals(model.Password)).ToList();
            if (data.Count() > 0)
            {
                //If login details are matching record in database set in cookie 
                HttpCookie cooskie = new HttpCookie("AdminInfo");
                cooskie.Values["idAdmin"] = Convert.ToString(data.FirstOrDefault().adminid);
                cooskie.Values["Email"] = Convert.ToString(data.FirstOrDefault().Email);
                cooskie.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(cooskie);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Message = "Login failed";
                return RedirectToAction("LoginAdmin");
            }
        }
        public ActionResult LogoutAdmin()
        {
            //destroy cookie  and logout 
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("AdminInfo"))
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["AdminInfo"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            return RedirectToAction("LoginAdmin");
        }

        public ActionResult ListOfOrders()
        {
            // List of all orders from user
            var adminInCookie = Request.Cookies["AdminInfo"];
            // check admin in cookie or else logout automatically
            if (adminInCookie != null)
            {
                //cookie available 
                float t = 0;
                List<Order> order = db.orders.ToList<Order>();
                foreach (var item in order)
                {
                    t += item.Order_Bill;
                }
                TempData["OrderTotal"] = t;
                return View(order);
            }
            else
            {
                //cookie not available 
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
        public ActionResult ListOfInvoices()
        {
            //Display list of invoices to admin
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                //cookie  available 
                float t = 0;
                MainModel viewModel = new MainModel();
                viewModel.invList = db.invoiceModel.ToList<InvoiceModel>();
                viewModel.locationList = db.HubLoc.ToList<HubLocation>();

                foreach (var item in viewModel.invList)
                {
                    t += item.Total_Bill;
                }
                TempData["InvoiceTotal"] = t;
                return View(viewModel);
            }
            else
            {
                //cookie not available 
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
        public ActionResult UpdateLocation(UpdateLocation model)
        {
            //update location of order
            try
            {
                InvoiceModel mdel = new InvoiceModel();
                mdel = db.invoiceModel.SingleOrDefault(s => s.ID.Equals(model.InvoiceId));
                mdel.LocationId = model.LocationId;
                db.Entry(mdel).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ERROR:" + ex.Message.ToString();
                return Json(false);
            }

            return Json(true);
        }
    }
}