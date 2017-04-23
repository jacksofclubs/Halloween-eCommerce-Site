using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Acme1.Models.ViewModels;
using Acme1.Models;
using System.Web.Security;



namespace Acme1.Controllers
{
    public class AccountController : Controller
    {

        SqlConnection dbcon;
        public SqlConnection GetConnection()
        {
            dbcon = new SqlConnection(ConfigurationManager.
            ConnectionStrings["ACMEdb"].ConnectionString.ToString());
            return dbcon;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            if (Request.QueryString["returnurl"] == null)
                return RedirectToAction("Index", "Home");
            LoginVM loginvm = new LoginVM();
            ViewBag.message = "";
            return View(loginvm);
            //LoginVM loginvm = new LoginVM();
            //ViewBag.returnurl = Request.QueryString["returnurl"].ToString();
            //ViewBag.message = "";
            //return View(loginvm);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginVM login)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbcon = GetConnection();
                    dbcon.Open();
                    Customer cust = Customer.GetCustomerSingle(dbcon, 0, login.Username);
                    dbcon.Close();
                    if (cust.CustNumber > 0 && cust.PWD == login.Password)
                    {
                        string ReturnUrl = Request.QueryString["returnurl"].ToString();
                        if (ReturnUrl.Length > 1 && Url.IsLocalUrl(ReturnUrl))
                        {
                            Session["custid"] = cust.CustNumber;
                            // Session.Add("custid", cust.CustNumber);
                            FormsAuthentication.SetAuthCookie(login.Username, false);
                            return Redirect(ReturnUrl);
                        } else
                            return RedirectToAction("Index", "Home");
                    }
                } catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            ViewBag.message = "Credentials are not valid";
            return View(login);
        }

        //[HttpGet]
        //public ActionResult MyOrders(int id = 0)
        //{
        //    int custid = Convert.ToInt32(Session["custid"].ToString());
        //    try
        //    {
        //        dbcon = GetConnection();
        //        dbcon.Open();
        //        var ordervm = new MyOrdersVM();
        //        ordervm.Invoices = Invoice.GetInVoiceList(dbcon, custid);
        //        ordervm.SelectedInvoiceNumber = id;
        //        if (id > 0)
        //        {
        //            ordervm.LineItems = Invoice_Lineitem.GetLineitemsList(dbcon, id);
        //            ordervm.SelectedInvoice = ordervm.Invoices.Find(x => x.InvoiceNumber == id);
        //            if (ordervm.SelectedInvoice == null)
        //                ordervm.SelectedInvoiceNumber = 0;
        //        }
        //        dbcon.Close();
        //        return View(ordervm);
        //    } catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

    }

}