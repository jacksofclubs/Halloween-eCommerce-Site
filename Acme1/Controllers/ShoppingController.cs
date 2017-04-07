using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Acme1.Models;
using Acme1.Models.ViewModels;
using System.Text.RegularExpressions;

namespace Acme1.Controllers
{
    public class ShoppingController : Controller
    {
        SqlConnection dbcon;
        public SqlConnection GetConnection()
        {
            dbcon = new SqlConnection(ConfigurationManager.
            ConnectionStrings["ACMEdb"].ConnectionString.ToString());
            return dbcon;
        }
        // GET: Shopping
        public ActionResult Index(string id)
        {
            List<Product> prodlist = new List<Product>();
            if (id == null) id = "";
            try
            {
                if (Regex.IsMatch(id, @"^[A-Za-z0-9]{2,10}$"))
                {
                    dbcon = GetConnection();
                    dbcon.Open();
                    prodlist = Product.GetProductList(dbcon, "categoryid = '" + id + "'");
                    ViewBag.Categoryid = id.ToUpper();
                    dbcon.Close();
                }
                else
                    ViewBag.Categoryid = "Invalid Data!";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(prodlist);
        }
        public ActionResult Order(string id)
        {
            Product prod = new Product();
            if (id == null) id = "";
            try
            {
                if (Regex.IsMatch(id, @"^[A-Za-z0-9]{2,10}$"))
                {
                    dbcon = GetConnection();
                    dbcon.Open();
                    prod = Product.GetProductSingle(dbcon, id);
                    dbcon.Close();
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(prod);
        }

        [HttpPost]
        public ActionResult Order(CartLineItem cart)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbcon = GetConnection();
                    dbcon.Open();
                    cart.CartNumber = 100;
                    int intresult = CartLineItem.CartUpSert(dbcon, cart);
                    dbcon.Close();
                } catch (Exception ex) { throw new Exception(ex.Message); }
            }
            return RedirectToAction("cart");
        }

        // GET: Shopping
        public ActionResult Cart()
        {
            List<Cartvm1> cart;
            int cartid = 100;
            try
            {
                dbcon = GetConnection();
                dbcon.Open();
                cart = Cartvm1.GetCartList(dbcon, cartid);
                dbcon.Close();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(cart);
        }
        [HttpPost]
        public ActionResult Cart(FormCollection fc)
        {
            string straction = fc["action"].ToString();
            int rowid = Convert.ToInt32(fc["rowid"].ToString());
            string prodid = fc["prod:" + rowid].ToString();
            string strqty = fc["qty:" + rowid].ToString();

            // Validate
            dbcon = GetConnection();
            dbcon.Open();
            CartLineItem.CUDCart(dbcon, straction, rowid, prodid, Int32.Parse(strqty));
            dbcon.Close();  

            return RedirectToAction("Cart");
        }
    }
}