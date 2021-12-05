/***************** DEVLOPER AND PAGE INFO **********************/
//
//
//
//Created By Shoheb
//Update By Shoheb on 29-11-2021 
//Hub Location controller to create new locations and delete existing location for admin
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

namespace FoodOrder.Controllers
{
    public class HubLocationController : Controller
    {
        // GET: HubLocation
        AppFoodDbContext db = new AppFoodDbContext();
        public ActionResult Index()
        {
            //Query HubLocation from database and show in list on page lod
            List<HubLocation> LocList = db.HubLoc.ToList<HubLocation>();
            return View(LocList);

        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            //Query and delete record of HubLocation from database
            HubLocation Loc = db.HubLoc.Find(id);
            db.HubLoc.Remove(Loc);
            db.SaveChanges();
            return RedirectToAction("Index", "HubLocation");
        }
        [HttpGet]
        public ActionResult CreateNewHubLocation()
        {
            //Display page to create new record of product
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                //show page is cookie exist
                return View();
            }
            else
            {
                //redirect to login page if cookie not exist
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
        public ActionResult CreateNewHubLocation(HubLocation Loc)
        {
            try
            {
                //Create new Location
                if (ModelState.IsValid)
                {
                    var data = db.HubLoc.Where(s => s.Locations.Equals(Loc.Locations) && s.Address.Equals(Loc.Address)).ToList();
                    if (data.Count() > 0)
                    {
                        //Show message if location already present in db
                        ViewBag.Message = "Location already exists";
                        return View();
                    }
                    else
                    {
                        //Enter new record in db table
                        db.HubLoc.Add(Loc);
                        db.SaveChanges();
                    }
                  
                }
               
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ERROR:" + ex.Message.ToString();
            }
            return RedirectToAction("Index","HubLocation");
        }

    }
}