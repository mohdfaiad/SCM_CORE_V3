using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Configuration;
using System.Data;

namespace BAL
{
    public class BALInterlineInvoiceListing
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        #region Get Invoice Data
        public DataSet GetInterlineInvoiceList(string InvoiceType, string AgentCode, string BillingType, string Origin, string FromDate, string ToDate, string InvoiceNumber, string AWBNumber, string PartnerCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] QueryPname = new string[9];
                object[] QueryValue = new object[9];
                SqlDbType[] QueryType = new SqlDbType[9];

                //select palletnumber,brand,batchno,Actualquantity,ispicked,isplanned,updatedby,updatedon

                QueryPname[0] = "InvoiceType";
                QueryPname[1] = "AgentCode";
                QueryPname[2] = "BillingType";
                QueryPname[3] = "Origin";
                QueryPname[4] = "FromDate";
                QueryPname[5] = "ToDate";
                QueryPname[6] = "InvoiceNumber";
                QueryPname[7] = "AWBNumber";
                QueryPname[8] = "PartnerCode";


                QueryType[0] = SqlDbType.VarChar;
                QueryType[1] = SqlDbType.VarChar;
                QueryType[2] = SqlDbType.VarChar;
                QueryType[3] = SqlDbType.VarChar;
                QueryType[4] = SqlDbType.VarChar;
                QueryType[5] = SqlDbType.VarChar;
                QueryType[6] = SqlDbType.VarChar;
                QueryType[7] = SqlDbType.VarChar;
                QueryType[8] = SqlDbType.VarChar;

                QueryValue[0] = InvoiceType;
                QueryValue[1] = AgentCode;
                QueryValue[2] = BillingType;
                QueryValue[3] = Origin;
                QueryValue[4] = FromDate;
                QueryValue[5] = ToDate;
                QueryValue[6] = InvoiceNumber;
                QueryValue[7] = AWBNumber;
                QueryValue[8] = PartnerCode;

                ds = da.SelectRecords("SP_GetInterlineInvoiceList", QueryPname, QueryValue, QueryType);


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

                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }
        #endregion

        # region GetInterlineInvoiceDataForPrint
        public DataSet GetInterlineInvoiceDataForPrint(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                DataSet ds = db.SelectRecords("SP_GetInterlineInvoiceDataForPrint", ColumnNames, Values, DataType);
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

        # endregion GetInterlineInvoiceDataForPrint

        # region GetInterlineCreditNoteDataForPrint
        public DataSet GetInterlineCreditNoteDataForPrint(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                DataSet ds = db.SelectRecords("SP_GetInterlineCreditNoteDataForPrint", ColumnNames, Values, DataType);
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

        # endregion GetInterlineCreditNoteDataForPrint

        # region GetInterlineProformaInvoiceDataForPrint
        public DataSet GetInterlineProformaInvoiceDataForPrint(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                DataSet ds = db.SelectRecords("SP_GetInterlineProformaInvoiceDataForPrint", ColumnNames, Values, DataType);
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

        # endregion GetInterlineProformaInvoiceDataForPrint

        # region GetInterlineProformaCreditNoteDataForPrint
        public DataSet GetInterlineProformaCreditNoteDataForPrint(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                DataSet ds = db.SelectRecords("SP_GetInterlineProformaCreditNoteDataForPrint", ColumnNames, Values, DataType);
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

        # endregion GetInterlineProformaCreditNoteDataForPrint
    }
}
