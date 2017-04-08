using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace Acme1.Models
{
    public class Product
    {
        [Required]
        [Key]
        [MaxLength(10)]
        public string ProductID { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string ShortDescription { get; set; }

        [MaxLength(200)]
        public string LongDescription { get; set; }

        [MaxLength(10)]
        public string CategoryID { get; set; }

        [MaxLength(30)]
        public string ImageFile { get; set; }

        public decimal UnitPrice { get; set; }

        public int OnHand { get; set; }

        public static Product GetProductSingle(SqlConnection dbcon, string id)
        {
            Product obj   = new Product();
            string strsql = "select * from Products where productid = '" + id + "'";
            var cmd       = new SqlCommand(strsql, dbcon);
            SqlDataReader myReader;
            myReader      = cmd.ExecuteReader();
            while (myReader.Read())
            {
                obj.ProductID        = myReader["productid"].ToString();
                obj.Name             = myReader["name"].ToString();
                obj.ShortDescription = myReader["shortdescription"].ToString();
                obj.LongDescription  = myReader["longdescription"].ToString();
                obj.ImageFile        = myReader["imagefile"].ToString();
                obj.CategoryID       = myReader["categoryid"].ToString();
                obj.UnitPrice        = Convert.ToDecimal(myReader["unitprice"].ToString());
                obj.OnHand           = Convert.ToInt32(myReader["onhand"].ToString());
            }
            myReader.Close();
            return obj;
        }
        public static List<Product> GetProductList(SqlConnection dbcon, string id)
        {
            List<Product> itemlist = new List<Product>();
            string strsql          = "select * from Products where " + id;
            var cmd                = new SqlCommand(strsql, dbcon);
            SqlDataReader myReader;
            myReader               = cmd.ExecuteReader();
            while (myReader.Read())
            {
                Product obj          = new Product();
                obj.ProductID        = myReader["productid"].ToString();
                obj.Name             = myReader["name"].ToString();
                obj.ShortDescription = myReader["shortdescription"].ToString();
                obj.LongDescription  = myReader["longdescription"].ToString();
                obj.ImageFile        = myReader["imagefile"].ToString();
                obj.CategoryID       = myReader["categoryid"].ToString();
                obj.UnitPrice        = Convert.ToDecimal(myReader["unitprice"].ToString());
                obj.OnHand           = Convert.ToInt32(myReader["onhand"].ToString());
                itemlist.Add(obj);
            }
            myReader.Close();
            return itemlist;
        }
        public static int CUDProduct(SqlConnection dbcon, string CUDAction, Product prod)
        {
            SqlCommand cmd = new SqlCommand();
            if (CUDAction == "create")
            {
                cmd.CommandText = "insert into Products values (@prodid, @name, @shortdescrip, @longdescrip, @categoryid, @imagefile, @unitprice, @onhand)";
                cmd.Parameters.AddWithValue("@prodid"      , SqlDbType.VarChar).Value = prod.ProductID;
                cmd.Parameters.AddWithValue("@name"        , SqlDbType.VarChar).Value = prod.Name;
                cmd.Parameters.AddWithValue("@shortdescrip", SqlDbType.VarChar).Value = prod.ShortDescription;
                cmd.Parameters.AddWithValue("@longdescrip" , SqlDbType.VarChar).Value = prod.LongDescription;
                cmd.Parameters.AddWithValue("@categoryid"  , SqlDbType.VarChar).Value = prod.CategoryID;
                cmd.Parameters.AddWithValue("@imagefile"   , SqlDbType.VarChar).Value = prod.ImageFile;
                cmd.Parameters.AddWithValue("@unitprice"   , SqlDbType.Decimal).Value = prod.UnitPrice;
                cmd.Parameters.AddWithValue("@onhand"      , SqlDbType.Int).Value     = prod.OnHand;
            } else if (CUDAction == "update")
            {
                cmd.CommandText = "update Products set ProductID = @prodid, Name = @name, ShortDescription = @shortdescrip, LongDescription = @longdescrip, CategoryID = @categoryid, ImageFile = @imagefile, UnitPrice = @unitprice, OnHand = @onhand where ProductID = @prodid";
                cmd.Parameters.AddWithValue("@prodid"      , SqlDbType.VarChar).Value = prod.ProductID;
                cmd.Parameters.AddWithValue("@name"        , SqlDbType.VarChar).Value = prod.Name;
                cmd.Parameters.AddWithValue("@shortdescrip", SqlDbType.VarChar).Value = prod.ShortDescription;
                cmd.Parameters.AddWithValue("@longdescrip" , SqlDbType.VarChar).Value = prod.LongDescription;
                cmd.Parameters.AddWithValue("@categoryid"  , SqlDbType.VarChar).Value = prod.CategoryID;
                cmd.Parameters.AddWithValue("@imagefile"   , SqlDbType.VarChar).Value = prod.ImageFile;
                cmd.Parameters.AddWithValue("@unitprice"   , SqlDbType.Decimal).Value = prod.UnitPrice;
                cmd.Parameters.AddWithValue("@onhand"      , SqlDbType.Int).Value     = prod.OnHand;
            } else if (CUDAction == "delete")
            {
                cmd.CommandText = "delete from Products where ProductID = @prodid";
                cmd.Parameters.AddWithValue("@prodid", SqlDbType.Int).Value = prod.ProductID;
            }
            cmd.Connection = dbcon;
            int intResult  = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return intResult;
        }
    }
}