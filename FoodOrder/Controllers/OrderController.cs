/***************** DEVLOPER AND PAGE INFO **********************/
//
//
//
//Created By Shoheb
//Update By Shoheb on 29-11-2021 for adding maps to show order location and to view order details
//order controller for user to track order location and view location on map
//
//
//
/***************** DEVLOPER AND PAGE INFO **********************/

using FoodWeb.Models;
using GoogleMaps.LocationServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace FoodOrder.Controllers
{
    public class OrderController : Controller
    {
        AppFoodDbContext db = new AppFoodDbContext();
        // GET: Order
        public ActionResult Index()
        {
            //Show order page with dropdown

            //Check if user in cookie or else redirect to main page
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null && userInCookie.Value != "")
            {
                
                string[] cookieval = userInCookie.Value.Split('&');
                int userval = Convert.ToInt32(cookieval[0].Split('=')[1]);

                //Query list of invoice based on user id from database
                var data = db.invoiceModel.Where(s => s.FKUserID == userval).ToList();
                List<InvoiceModel> invMdl = data;
                //Query order data of user and send to view to show
                return View(invMdl);
            }
            else
            {
                return RedirectToAction("Index", "Products");
            }

        }

        //Code to get geo co-ordinates from address
        [HttpPost]
        public ActionResult GetLocation(UpdateLocation model)
        {
            try
            {
                //Get location Id from view and query record from database
                HubLocation data = db.HubLoc.SingleOrDefault(s => s.Id == model.LocationId);
                var address = data.Address;
                string location = data.Locations;
                string status = "0";
                if (data.Id == 10)
                {
                    //If order status is update to delivered then show location and details of user address
                    SignupLogin dataUser = db.SignupLogin.SingleOrDefault(s => s.userid == model.UserId);
                    address = dataUser.Address;
                    status = "1";
                }

                //URL to hit and get geo co-ordinates
                var url = "https://open.mapquestapi.com/geocoding/v1/address?key=NPWCj7kXyiEhKRsnPog7rvKZwwe4NyrM&location=" + address + "";


                WebRequest request = HttpWebRequest.Create(url);

                WebResponse response = request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream());

                string responseText = reader.ReadToEnd();


                dynamic stuff = JsonConvert.DeserializeObject(responseText);
                Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(responseText);

                //make data of co-ordinates and order details and send to view
                var dataJson = new latLng
                {
                    lat = obj.results[0].locations[0].latLng.lat,
                    lng = obj.results[0].locations[0].latLng.lng,
                    Status = status,
                    Address = address,
                    LocDet = location
                };


                return Json(dataJson);//send date in JSON format to view
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ERROR:" + ex.Message.ToString();
            }
            return Json(false);
        }


    }
}
