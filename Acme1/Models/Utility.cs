using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Acme1.Models
{
    public class Utility
    {
        //copy the appropriate using statements
        public static int GetIdNumber(SqlConnection dbcon, string ctlkey)
        {
            string strquery = "SELECT idnumber FROM controltable" +
            " WHERE ctlkey = '" + ctlkey + "';";
            SqlCommand myCmd = new SqlCommand(strquery, dbcon);
            int count = Convert.ToInt32(myCmd.ExecuteScalar().ToString()) + 1;
            strquery = "UPDATE controltable SET idnumber = " + count +
            " where ctlkey = '" + ctlkey + "';";
            myCmd = new SqlCommand(strquery, dbcon);
            myCmd.ExecuteNonQuery();
            myCmd.Dispose();
            return count;
        }
    }

    
}