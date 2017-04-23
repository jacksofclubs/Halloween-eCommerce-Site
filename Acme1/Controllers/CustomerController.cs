using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Acme1.Models;

namespace Acme1.Controllers
{
    public class CustomerController : Controller
    {
        SqlConnection dbcon;
        public SqlConnection GetConnection()
        {
            dbcon = new SqlConnection(ConfigurationManager.
            ConnectionStrings["ACMEdb"].ConnectionString.ToString());
            return dbcon;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Update()
        {
            Customer cust;
            int custid = 136;
            if (Session["custid"] != null && Session["custid"] != "custid")
                custid = Convert.ToInt32(Session["custid"].ToString());
            try
            {
                dbcon = GetConnection();
                dbcon.Open();
                TempData["statelist"] = GetStatesDropDown(dbcon);
                cust = Customer.GetCustomerSingle(dbcon, custid, "");
                ViewBag.message = "Make your changes and click Update button";
                dbcon.Close();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(cust);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Update(Customer ncust)
        {
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(Session["custid"].ToString());
                int intresult = 0;
                Customer ocust, tcust;
                try
                {
                    dbcon = GetConnection();
                    dbcon.Open();
                    ocust = Customer.GetCustomerSingle(dbcon, id, "");
                    if (ncust.Email == ocust.Email)
                    {
                        intresult = Customer.CUDCustomer(dbcon, "update", ncust);
                        ViewBag.message = "Profile updated - Click Menu button to continue";
                    } else
                    {
                        tcust = Customer.GetCustomerSingle(dbcon, 0, ncust.Email);
                        if (tcust.CustNumber == 0)
                        {
                            intresult = Customer.CUDCustomer(dbcon, "update", ncust);
                            ViewBag.message = "Profile updated - Click Menu button to continue";
                        } else ViewBag.error = "Update cancelled - Email Address already exists";
                    }
                    dbcon.Close();
                    return View(ncust);
                } catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            ViewBag.error = "Serious Error";
            return View(ncust);
        }


        [Authorize]
        [HttpPost]
        public ActionResult Update2(Customer ncust)
        {
            if (ModelState.IsValid)
            {
                int id = Convert.ToInt32(Session["custid"].ToString());
                int intresult = 0;
                Customer ocust, tcust;
                try
                {
                    dbcon = GetConnection();
                    dbcon.Open();
                    ocust = Customer.GetCustomerSingle(dbcon, id, "");
                    if (ncust.Email == ocust.Email)
                    {
                        intresult = Customer.CUDCustomer(dbcon, "update", ncust);
                        ViewBag.message = "Profile updated - Click Menu button to continue";
                    } else
                    {
                        tcust = Customer.GetCustomerSingle(dbcon, 0, ncust.Email);
                        if (tcust.CustNumber == 0)
                        {
                            intresult = Customer.CUDCustomer(dbcon, "update", ncust);
                            ViewBag.message = "Profile updated - Click Menu button to continue";
                        } else ViewBag.error = "Update cancelled - Email Address already exists";
                    }
                    dbcon.Close();
                    return View(ncust);
                } catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            ViewBag.error = "Serious Error";
            return View(ncust);
        }

        public IEnumerable<SelectListItem> GetStatesDropDown(SqlConnection dbcon)
        {
            IList<SelectListItem> ddlist = new List<SelectListItem>();
            string strsql = "select * from States";
            SqlCommand cmd = new SqlCommand(strsql, dbcon);
            SqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                ddlist.Add(new SelectListItem() {
                    Text = myReader["S_NAME"].ToString(),
                    Value = myReader["S_ABBREVIATION"].ToString()
                });
            }
            myReader.Close();
            return ddlist;
        }
    }
}