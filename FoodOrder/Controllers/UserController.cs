/***************** DEVLOPER AND PAGE INFO **********************/
//
//
//
//Created By GithubSource
//Update By Shoheb on 29-11-2021 for adding comments
//Login controller for user
//
//
//
/***************** DEVLOPER AND PAGE INFO **********************/


using FoodWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodWeb.Controllers
{
    public class UserController : Controller
    {
        AppFoodDbContext db = new AppFoodDbContext();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Signup()
        {
            //Display Signup page
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null && userInCookie.Value!="")
            {
                //redirect to products order page id user exists
                return RedirectToAction("Index", "Products");
            }
            else
            {
                var adminInCookie = Request.Cookies["AdminInfo"];
                if (adminInCookie != null)
                {
                    //redirect to admin dashboard if admin user exists
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    //Show sign up page
                    return View();
                }

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(SignupLogin signup)
        {
            //Save user data in database 
            if (ModelState.IsValid)
            {
                //Check for email already registered
                var isEmailAlreadyExists = db.SignupLogin.Any(x => x.Email == signup.Email);
                if (isEmailAlreadyExists)
                {
                    //If email existing then show following message
                    ViewBag.Message = "Email Already Registered. Please Try Again With Another Email";
                    return View();
                }
                else
                {
                    //Insert user in database
                    db.SignupLogin.Add(signup);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Products");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            //Display login page
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null && userInCookie.Value!="")
            {
                return RedirectToAction("Index", "Products");
            }
            else
            {
                var adminInCookie = Request.Cookies["AdminInfo"];
                if (adminInCookie != null)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return View();
                }
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(SignupLogin model)
        {
            //Check user in database by matching details entered
            var data = db.SignupLogin.Where(s => s.Email.Equals(model.Email) && s.Password.Equals(model.Password)).ToList();
            if (data.Count() > 0)
            {
                //If details match save user in cookie and session and login
                Session["uid"] = data.FirstOrDefault().userid;
                HttpCookie cooskie = new HttpCookie("UserInfo");
                cooskie.Values["idUser"] = Convert.ToString(data.FirstOrDefault().userid);
                cooskie.Values["FullName"] = Convert.ToString(data.FirstOrDefault().Name);
                cooskie.Values["Email"] = Convert.ToString(data.FirstOrDefault().Email);
                cooskie.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(cooskie);
                return RedirectToAction("Index", "Products");
            }
            else
            {
                //If no match redirect to login
                ViewBag.Message = "Login failed";
                return RedirectToAction("Login");
            }
        }
        public ActionResult Logout()
        {
            //Destroy cookie and session and logout
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("UserInfo"))
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["UserInfo"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                HttpContext.Response.Cookies.Set(new HttpCookie("UserInfo") { Value = string.Empty });
            }
            Session.Clear();
            return RedirectToAction("Login");
        }


    }
}