using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using QID.DataAccess;
using System.Data;


namespace BAL
{
    public class BALRejectionMemo
    {

        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        #region Generate Rejection Memo
        public bool GenerateRejectionMemo(object[] ParamValues)
        {
            try
            {
                string[] QueryNames = new string[2];
                SqlDbType[] QueryTypes = new SqlDbType[2];

                QueryNames[0] = "SerialNo";
                QueryNames[1] = "UpdatedBy";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;

                if (db.UpdateData("SP_GenerateRejectionMemo", QueryNames, QueryTypes, ParamValues))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            { return false; }
        }
        #endregion

        #region List CCA
        public DataSet ListCCA(object[] objCCA)
        {
            try
            {
                string[] ParamNames = new string[] { "AWBNumber", "InvoiceNo", "FromDate", "ToDate", "CCAType" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit };
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SpGetRMProcessing", ParamNames, objCCA, ParamTypes);
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

                return null;

            }
            return null;
        }

        #endregion

        #region List RM
        public DataSet ListRM(object[] objRM)
        {
            try
            {
                string[] ParamNames = new string[] { "RMNumber", "AWBNumber", "InvoiceNo", "AgentCode", "FromDate", "ToDate", "RMType", "Status", "Interline" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Bit };
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SpGetRMProcessingList", ParamNames, objRM, ParamTypes);
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

                return null;

            }
            return null;
        }

        #endregion

        # region GetRMPrintDetails
        public DataSet GetRMPrintDetails(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                ColumnNames.SetValue("RMNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);


                //DataSet ds = db.SelectRecords("SP_GetInvoiceDataImport", ColumnNames, Values, DataType);
                DataSet ds = db.SelectRecords("SP_GetRMPrintDetails", ColumnNames, Values, DataType);
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

        # region GetRMperAWBPrintDetails
        public DataSet GetRMperAWBPrintDetails(object[] RateLineInfo)
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
                DataSet ds = db.SelectRecords("SP_GetRMperAWBPrintDetails", ColumnNames, Values, DataType);
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
