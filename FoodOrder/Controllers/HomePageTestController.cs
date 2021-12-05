/***************** DEVLOPER AND PAGE INFO **********************/
//
//
//
//Created By GithubSource
//Update By Shoheb on 29-11-2021 for adding comments
//Home Page controller for showing products to user
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
    public class HomePageTestController : Controller
    {
        AppFoodDbContext db = new AppFoodDbContext();
        // GET: HomePageTest
        public ActionResult Index()
        {
            //Display list of all products
                List<Products> products = db.Products.ToList<Products>();
                return View(products);
           
          
        }
    }
}