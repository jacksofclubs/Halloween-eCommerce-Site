using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Acme1.Models;
using System.IO;

namespace Acme1.Controllers
{
    public class ProductController : Controller
    {
        SqlConnection dbcon;
        public SqlConnection GetConnection()
        {
            dbcon = new SqlConnection(ConfigurationManager.
            ConnectionStrings["ACMEdb"].ConnectionString.ToString());
            return dbcon;
        }
        // GET: Product
        public ActionResult Index()
        {
            List<Product> prodlist;
            try
            {
                dbcon = GetConnection();
                dbcon.Open();
                prodlist = Product.GetProductList(dbcon, "1=1");
                dbcon.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(prodlist);
        }

        // GET: Product
        public ActionResult Edit(string id)
        {
            Product prod;
            try
            {
                dbcon = GetConnection();
                dbcon.Open();
                prod = Product.GetProductSingle(dbcon, id);
                dbcon.Close();
            } 
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(prod);
        }
        [HttpPost]
        public ActionResult Edit(Product prod, HttpPostedFileBase uploadfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadfile != null && uploadfile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(uploadfile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Images/products"), fileName);
                        uploadfile.SaveAs(path);
                        prod.ImageFile = fileName;
                    }
                    dbcon = GetConnection();
                    dbcon.Open();
                    int intresult = Product.CUDProduct(dbcon, "update", prod);
                    dbcon.Close();
                    return RedirectToAction("Index");
                } 
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return View(prod);
        }
        // GET: Product
        public ActionResult Create()
        {
            Product prod = new Product();
            prod.ImageFile = "nopic.jpg";
            return View(prod);
        }
        [HttpPost]
        public ActionResult Create(Product prod, HttpPostedFileBase uploadfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadfile != null && uploadfile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(uploadfile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Images/products"), fileName);
                        uploadfile.SaveAs(path);
                        prod.ImageFile = fileName;
                    }
                    dbcon = GetConnection();
                    dbcon.Open();
                    int intresult = Product.CUDProduct(dbcon, "create", prod);
                    dbcon.Close();
                    return RedirectToAction("Index");
                } catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return View(prod);
        }
        // Delete
        public ActionResult Delete(string id)
        {
            Product prod;
            try
            {
                dbcon = GetConnection();
                dbcon.Open();
                prod = Product.GetProductSingle(dbcon, id);
                dbcon.Close();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View(prod);
        }
        [HttpPost]
        public ActionResult Delete(Product prod, HttpPostedFileBase uploadfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbcon = GetConnection();
                    dbcon.Open();
                    int intresult = Product.CUDProduct(dbcon, "delete", prod);
                    dbcon.Close();
                    return RedirectToAction("Index");
                } catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return View(prod);
        }
    }
}