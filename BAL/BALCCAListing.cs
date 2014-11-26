using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Configuration;
using System.Data;

namespace BAL
{
   public class BALCCAListing
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        # region GetCCAPrintDetails
        public DataSet GetCCAPrintDetails(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                ColumnNames.SetValue("CCANumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);


                //DataSet ds = db.SelectRecords("SP_GetInvoiceDataImport", ColumnNames, Values, DataType);
                DataSet ds = db.SelectRecords("SP_GetCCAPrintDetails", ColumnNames, Values, DataType);
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

        # endregion GetCCAPrintDetails

        # region GetCCAperAWBPrintDetails
        public DataSet GetCCAperAWBPrintDetails(object[] RateLineInfo)
        {
            try
            {
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //ColumnNames.SetValue("CurrentFlightNumber", i);
                //DataType.SetValue(SqlDbType.VarChar, i);
                //Values.SetValue(RateLineInfo.GetValue(i), i);
                //i++;

                //ColumnNames.SetValue("CurrentFlightDate", i);
                //DataType.SetValue(SqlDbType.VarChar, i);
                //Values.SetValue(RateLineInfo.GetValue(i), i);

                //DataSet ds = db.SelectRecords("SP_GetInvoiceDataImport", ColumnNames, Values, DataType);
                DataSet ds = db.SelectRecords("SP_GetCCAperAWBPrintDetails", ColumnNames, Values, DataType);
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

        # endregion GetCCAperAWBPrintDetails
    }
}
