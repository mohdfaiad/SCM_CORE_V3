using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Configuration;
using System.Data;

namespace BAL
{
    public class BALDCMListing
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        # region GetDCMPrintDetails
        public DataSet GetDCMPrintDetails(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                ColumnNames.SetValue("DCMNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);


                //DataSet ds = db.SelectRecords("SP_GetInvoiceDataImport", ColumnNames, Values, DataType);
                DataSet ds = db.SelectRecords("SP_GetDCMPrintDetails", ColumnNames, Values, DataType);
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

        # endregion GetDCMPrintDetails

        # region GetDCMperAWBPrintDetails
        public DataSet GetDCMperAWBPrintDetails(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                int i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CurrentFlightNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CurrentFlightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                //DataSet ds = db.SelectRecords("SP_GetInvoiceDataImport", ColumnNames, Values, DataType);
                DataSet ds = db.SelectRecords("SP_GetDCMperAWBPrintDetails", ColumnNames, Values, DataType);
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

        # endregion GetDCMperAWBPrintDetails

        # region GetDCMAWBDealsPrintDetails
        public DataSet GetDCMAWBDealsPrintDetails(string InvoiceNo)
        {
            try
            {
                //DataSet ds = db.SelectRecords("SP_GetInvoiceDataImport", ColumnNames, Values, DataType);
                DataSet ds = db.SelectRecords("SP_GetDCMAWBDealsPrintDetails", "InvoiceNumber", InvoiceNo, SqlDbType.VarChar);
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

        # endregion GetDCMperAWBPrintDetails

        # region get Configured Invoice Report Name
        public string getConfiguredInvoiceReportName()
        {
            try
            {
                return db.GetString("select top 1 value from tblConfiguration where Parameter = 'DCMPerAWBReportName'");
            }
            catch (Exception ex)
            {
                return "";

            }
        }
        # endregion get Configured Invoice Report Name

    }
}
