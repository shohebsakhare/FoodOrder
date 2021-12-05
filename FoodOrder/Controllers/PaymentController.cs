/***************** DEVLOPER AND PAGE INFO **********************/
//
//
//
//Created By Shoheb on 29-11-2021
//Update By Shoheb on 29-11-2021 for adding comments
//Payment API controller for user to carry out payment and save order details in DB
//
//
//
/***************** DEVLOPER AND PAGE INFO **********************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodWeb.Models;
using Stripe;

namespace FoodOrder.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        AppFoodDbContext db = new AppFoodDbContext();
        public string TotalAmount { get; set; }
        public ActionResult Index()
        {
           
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null && userInCookie.Value!="")
            {
                //Load payment screen on screen with amount info
                ViewBag.GBPAmount = (float)TempData["Total"];
                ViewBag.total = Convert.ToInt64((float)TempData["Total"]);
                long total = ViewBag.total;
                TotalAmount = total.ToString();
                TempData.Keep("total");
                return View();
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
                    return RedirectToAction("Login", "User");
                }

            }
           
        }
        
        public ActionResult ProcessPayment(string stripeToken, string stripeEmail, string id)
        {
            //Details to capture after click on pay button
            var userInCookie1 = Request.Cookies["UserInfo"];
            string[] cookieval = userInCookie1.Value.Split('&');
            var userval = cookieval[0].Split('=');
            var optionsCust = new CustomerCreateOptions
            {
                //customer data
                Email = stripeEmail,
                Name = "Robert",
                Phone = "04-234567"

            };
            var optionsReq = new RequestOptions
            {
                //Api key to connect to stripe account
                ApiKey = System.Configuration.ConfigurationManager.AppSettings["stripe_secret_key"]

            };
            var serviceCust = new CustomerService();
            Customer customer = serviceCust.Create(optionsCust, optionsReq);
            var optionsCharge = new ChargeCreateOptions
            {
                //Insert Payment details in variable and execute payment

                Amount = Convert.ToInt64(TempData["Total"]) * 100,
                Currency = "GBP",
                Description = "Buying Organic Food",
                Source = stripeToken,
                ReceiptEmail = "shohebsakhare@gmail.com",

            };
            var service = new ChargeService();
            Charge charge = service.Create(optionsCharge, optionsReq);
            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                ViewBag.AmountPaid = Convert.ToDecimal(charge.Amount) % 100 / 100 + (charge.Amount) / 100;
                ViewBag.BalanceTxId = BalanceTransactionId;
                ViewBag.Customer = customer.Name;
                //return View();
               

                //Save Payment details
                FoodWeb.Models.PaymentInfo pay = new FoodWeb.Models.PaymentInfo();
                pay.userId = Convert.ToInt32(userval[1].ToString());
                pay.ch_tx_id = charge.Id;
                pay.amount = ViewBag.AmountPaid;
                pay.card_type = charge.PaymentMethodDetails.Card.Funding;
                pay.email_id = stripeEmail;
                pay.exp_date = Convert.ToString(charge.PaymentMethodDetails.Card.ExpMonth) + '/' + Convert.ToString(charge.PaymentMethodDetails.Card.ExpYear);
                pay.tx_status = "Success";

                db.payments.Add(pay);
                db.SaveChanges();
            }

            // Save product data in database
            #region SaveCheckoutData
            var userInCookie = Request.Cookies["UserInfo"];
            int iduser = Convert.ToInt32(userInCookie["idUser"]);
            List<Cart> li = TempData["cart"] as List<Cart>;
            InvoiceModel invoice = new InvoiceModel();
            invoice.FKUserID = iduser;
            invoice.DateInvoice = System.DateTime.Now;
            invoice.Total_Bill = (float)TempData["Total"];
            db.invoiceModel.Add(invoice);
            db.SaveChanges();
            foreach (var item in li)
            {
                FoodWeb.Models.Order odr = new FoodWeb.Models.Order();
                odr.FkProdId = item.productId;
                odr.FkInvoiceID = invoice.ID;
                odr.Order_Date = System.DateTime.Now;
                odr.Qty = item.qty;
                odr.Unit_Price = (int)item.price;
                odr.Order_Bill = item.bill;
                db.orders.Add(odr);
                db.SaveChanges();
            }
            // Save product data in database end
            TempData.Remove("total");
            TempData.Remove("cart");
            TempData.Keep();
            #endregion
            // Show success page
            return View();
        }

    }
}