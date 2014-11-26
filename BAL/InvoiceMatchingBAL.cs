using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using QID.DataAccess;
namespace BAL
{
   public class InvoiceMatchingBAL
    {
        string constr = "";
        public InvoiceMatchingBAL()
        {
            constr = Global.GetConnectionString();
        }

        #region Get Invoice Details

        public DataSet InvoiceDetails(object[] Inv)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ParamNames = new string[] { "InvoiceNumber" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar };
                DataSet ds = da.SelectRecords("spGetInvoiceDetails", ParamNames, Inv, ParamTypes);
                    if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            return (ds);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return (null);


        }

        #endregion 

    }
}
