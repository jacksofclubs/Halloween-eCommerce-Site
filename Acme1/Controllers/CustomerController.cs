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
        // GET: Customer
        public ActionResult Update()
        {
            Customer cust;
            int custid = 136;
            if (Session["custid"] != null)
                custid = Convert.ToInt32(Session["custid"].ToString());
            try
            {
                dbcon = GetConnection();
                dbcon.Open();
                cust = Customer.GetCustomerSingle(dbcon, custid, "");
                dbcon.Close();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(cust);
        }
    }
}