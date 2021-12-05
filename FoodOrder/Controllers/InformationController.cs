/***************** DEVLOPER AND PAGE INFO **********************/
//
//
//
//Created By GithubSource
//Update By Shoheb on 29-11-2021 for adding comments
//Information controller for contact details
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
    public class InformationController : Controller
    {
        AppFoodDbContext db = new AppFoodDbContext();
        // GET: Information
        public ActionResult ContactUs()
        {
            //show contact us page on load
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(ContactModel contact)
        {
            //Query data from database of contact details
            db.contactModels.Add(contact);
            db.SaveChanges();
            return View();
        }
        public ActionResult MessageList()
        {
            //Display list of messages
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                List<ContactModel> contacts = db.contactModels.ToList<ContactModel>();
                return View(contacts);
                
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Products", "Index");

                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }
        public ActionResult AboutUs()
        {
            //Display AboutUs page
            return View();
        }
        public ActionResult Blogs()
        {
            //Display AboutUs page
            return View();
        }

    }
}