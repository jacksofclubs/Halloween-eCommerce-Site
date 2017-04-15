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
                    //cart.CartNumber = 100;
                    // if no cart number exists
                    if (Session["cartnumber"] == null)
                        Session["cartnumber"] = Utility.GetIdNumber(dbcon, "CartNumber");
                    cart.CartNumber = Convert.ToInt32(Session["cartnumber"].ToString());
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
            // int cartid = 100;
            int cartid = 0;
            if (Session["cartnumber"] != null)
                cartid = Convert.ToInt32(Session["cartnumber"].ToString());
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
            try
            {
                if (!Valid_CartData(fc)) throw new Exception("Cart data invalid");
                int intresult = 0;
                int rowid = Convert.ToInt32(fc["rowid"].ToString());
                string strprodid = fc["prod:" + rowid].ToString();
                int cartid = 100;
                //int cartid = 0;
                if (Session["cartnumber"] != null)
                    cartid = Convert.ToInt32(Session["cartnumber"].ToString());
                dbcon = GetConnection();
                dbcon.Open();
                if (fc["action"] == "update")
                {
                    int qty = Convert.ToInt32(fc["qty:" + rowid].ToString());
                    intresult = CartLineItem.CUDCart(dbcon, "update", cartid, strprodid, qty);
                } else
                    intresult = CartLineItem.CUDCart(dbcon, "delete", cartid, strprodid, 0);
                dbcon.Close();
                return RedirectToAction("Cart");
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Boolean Valid_CartData(FormCollection fc)
        {
            if (fc["rowid"] == null || fc["action"] == null) return false;
            if (Regex.IsMatch(fc["rowid"].ToString(), @"^([1-9]|[1-9][0-9])$"))
            {
                int rowid = Convert.ToInt32(fc["rowid"].ToString());
                if (fc["prod:" + rowid] == null) return false;
                if (fc["action"] == "update" &&
                Regex.IsMatch(fc["qty:" + rowid].ToString(), @"^([1-9]|[1-9][0-9])$"))
                    return true;
                else if (fc["action"] == "delete")
                    return true;
            }
            return false;
        }
    }
}