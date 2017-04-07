using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace Acme1.Models.ViewModels
{
    public class Cartvm1
    {
        public int CartNumber { get; set; }
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Imagefile { get; set; }
        public decimal TotalCost { get; set; }
        public static List<Cartvm1> GetCartList(SqlConnection dbcon, int id)
        {
            List<Cartvm1> itemlist = new List<Cartvm1>();
            string strsql = "select * from cart_view02 where cartnumber = " + id;
            var cmd = new SqlCommand(strsql, dbcon);
            SqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                Cartvm1 obj = new Cartvm1();
                obj.CartNumber = id;
                obj.ProductID = myReader["productid"].ToString();
                obj.Name = myReader["name"].ToString();
                obj.Imagefile = myReader["imagefile"].ToString();
                obj.ShortDescription = myReader["shortdescription"].ToString();
                obj.UnitPrice = Convert.ToDecimal(myReader["unitprice"].ToString());
                obj.Quantity = Convert.ToInt32(myReader["quantity"].ToString());
                obj.TotalCost = Convert.ToDecimal(myReader["extension"].ToString());
                itemlist.Add(obj);
            }
            myReader.Close();
            return itemlist;
        }

    }
}