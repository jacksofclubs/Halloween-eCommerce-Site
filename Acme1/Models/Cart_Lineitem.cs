using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace Acme1.Models
{
    public class CartLineItem
    {
        [Required]
        public int CartNumber { get; set; }
        [Required]
        [MaxLength(10)]
        public string ProductId { get; set; }
        [Required]
        [Range(minimum: 1, maximum: 99, ErrorMessage = "Qty must be between 1 and 99")]
        public int Quantity { get; set; }

        public static int CUDCart(SqlConnection dbcon, string CUDAction, int cartid, string prodid, int qty)
        {
            SqlCommand cmd = new SqlCommand();
            if (CUDAction == "create")
            {
                cmd.CommandText =
                "insert into cart_linetitems (CartNumber, ProductId, Quantity) values (@cartid,@prodid,@qty)";
                cmd.Parameters.AddWithValue("@cartid", SqlDbType.Int).Value = cartid;
                cmd.Parameters.AddWithValue("@prodid", SqlDbType.VarChar).Value = prodid;
                cmd.Parameters.AddWithValue("@qty", SqlDbType.Int).Value = qty;
            } else if (CUDAction == "update")
            {
                cmd.CommandText = "update cart_lineitems set quantity = @qty " +
                                   "where cartnumber = @cartid and productid = @prodid";
                cmd.Parameters.AddWithValue("@qty", SqlDbType.Int).Value = qty;
                cmd.Parameters.AddWithValue("@cartid", SqlDbType.Int).Value = cartid;
                cmd.Parameters.AddWithValue("@prodid", SqlDbType.VarChar).Value = prodid;
            } else if (CUDAction == "delete")
            {
                cmd.CommandText = "delete cart_lineitems where cartnumber = @cartid and productid = @prodid";
                cmd.Parameters.AddWithValue("@cartid", SqlDbType.Int).Value = cartid;
                cmd.Parameters.AddWithValue("@prodid", SqlDbType.VarChar).Value = prodid;
            }
            cmd.Connection = dbcon;
            int intResult = cmd.ExecuteNonQuery();
            return intResult;
        }

        public static Int32 CartUpSert(SqlConnection dbcon, CartLineItem cart)
        {
            SqlCommand cmd = new SqlCommand("sp_cart_upsert", dbcon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@cartid", SqlDbType.Int).Value = cart.CartNumber;
            cmd.Parameters.Add("@prodid", SqlDbType.VarChar).Value = cart.ProductId;
            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = cart.Quantity;
            int intCnt = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return intCnt;
        }
    }
}