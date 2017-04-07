using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace Acme1.Models
{
    public class Customer
    {
        [Required]
        [Key]
        public int CustNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 1)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 5)]
        public string Address { get; set; }
        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 2)]
        public string City { get; set; }
        [Required]
        [RegularExpression("^[A-Z]{2}$", ErrorMessage = "State code is not valid")]
        public string State { get; set; }
        [Required]
        [StringLength(maximumLength: 9, MinimumLength = 5)]
        public string ZipCode { get; set; }
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 7)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string PWD { get; set; }

        public static Customer GetCustomerSingle(SqlConnection dbcon, int custid, string email)
        {
            Customer obj = new Customer();
            SqlCommand cmd;
            if (custid != 0)
            {
                cmd = new SqlCommand("select * from customers where CustNumber = @custid", dbcon);
                cmd.Parameters.AddWithValue("@custid", SqlDbType.Int).Value = custid;
            } else
            {
                cmd = new SqlCommand("select * from customers where email = @email", dbcon);
                cmd.Parameters.AddWithValue("@email", SqlDbType.VarChar).Value = email;
            }
            SqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                obj.CustNumber = Convert.ToInt32(myReader["Custnumber"].ToString());
                obj.Email = myReader["email"].ToString();
                obj.LastName = myReader["lastname"].ToString();
                obj.FirstName = myReader["firstname"].ToString();
                obj.Address = myReader["address"].ToString();
                obj.City = myReader["city"].ToString();
                obj.State = myReader["state"].ToString();
                obj.ZipCode = myReader["zipcode"].ToString();
                obj.PhoneNumber = myReader["phonenumber"].ToString();
                obj.PWD = myReader["pwd"].ToString();
            }
            myReader.Close();
            return obj;
        }

        public static int CUDCustomer(SqlConnection dbcon, string CUDAction, Customer cust)
        {
            SqlCommand cmd = new SqlCommand();
            if (CUDAction == "create")
            {
                cmd.CommandText =
                "insert into customers (Email,LastName,FirstName,Address,City,State,ZipCode,PhoneNUmber,PWD) " +
                "values (@email,@lname,@fname,@address,@city,@state,@zipcode,@phone,@pwd)";
                cmd.Parameters.AddWithValue("@email", SqlDbType.VarChar).Value = cust.Email;
                cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = cust.LastName;
                cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = cust.FirstName;
                cmd.Parameters.AddWithValue("@address", SqlDbType.VarChar).Value = cust.Address;
                cmd.Parameters.AddWithValue("@city", SqlDbType.VarChar).Value = cust.City;
                cmd.Parameters.AddWithValue("@state", SqlDbType.VarChar).Value = cust.State;
                cmd.Parameters.AddWithValue("@zipcode", SqlDbType.VarChar).Value = cust.ZipCode;
                cmd.Parameters.AddWithValue("@phone", SqlDbType.VarChar).Value = cust.PhoneNumber;
                cmd.Parameters.AddWithValue("@pwd", SqlDbType.VarChar).Value = cust.PWD;
            } else if (CUDAction == "update")
            {
                cmd.CommandText = "update customers set email = @email, lastname = @lname, " +
                "firstname = @fname, address = @address, city = @city, state = @state, zipcode = @zipcode, " +
                "phonenumber = @phone, pwd = @pwd where custnumber = @custid";
                cmd.Parameters.AddWithValue("@email", SqlDbType.VarChar).Value = cust.Email;
                cmd.Parameters.AddWithValue("@lname", SqlDbType.VarChar).Value = cust.LastName;
                cmd.Parameters.AddWithValue("@fname", SqlDbType.VarChar).Value = cust.FirstName;
                cmd.Parameters.AddWithValue("@address", SqlDbType.VarChar).Value = cust.Address;
                cmd.Parameters.AddWithValue("@city", SqlDbType.VarChar).Value = cust.City;
                cmd.Parameters.AddWithValue("@state", SqlDbType.VarChar).Value = cust.State;
                cmd.Parameters.AddWithValue("@zipcode", SqlDbType.VarChar).Value = cust.ZipCode;
                cmd.Parameters.AddWithValue("@phone", SqlDbType.VarChar).Value = cust.PhoneNumber;
                cmd.Parameters.AddWithValue("@pwd", SqlDbType.VarChar).Value = cust.PWD;
                cmd.Parameters.AddWithValue("@custid", SqlDbType.Int).Value = cust.CustNumber;
            } else if (CUDAction == "delete")
            {
                cmd.CommandText = "delete customers where custnumber = @custid";
                cmd.Parameters.AddWithValue("@custid", SqlDbType.Int).Value = cust.CustNumber;
            }
            cmd.Connection = dbcon;
            int intResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return intResult;
        }
    }
}