using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Configuration;
using System.Data;

namespace BAL
{
   public  class BALInvoiceListing
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables


        //GetAgentList
        #region Get Agent List
        public DataSet GetAgentList()
        {
            DataSet ds = new DataSet();
            try
            {                
                SQLServer da = new SQLServer(constr);

                ds = da.SelectRecords("SPGetAgentName");
                
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

        #endregion GetAgentList


        #region Get Invoice Data
       public DataSet GetInvoiceData(string InvoiceType, string AgentCode,string BillingType, string Origin, string FromDate,string ToDate,string InvoiceNumber, string AWBNumber, string InvoiceStatus)
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
            QueryPname[8] = "InvoiceStatus";
            
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
            QueryValue[8] = InvoiceStatus;

            ds = da.SelectRecords("SP_GetInvoiceListdataNew", QueryPname, QueryValue, QueryType);


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

       # region GetInvoiceData
       public DataSet GetInvoiceDataImport(object[] RateLineInfo)
       {
           try
           {
               DataSet ds;

               string[] ColumnNames = new string[1];
               SqlDbType[] DataType = new SqlDbType[1];
               Object[] Values = new object[1];
               int i = 0;
               
               ColumnNames.SetValue("InvoiceNumber", i);
               DataType.SetValue(SqlDbType.VarChar, i);
               Values.SetValue(RateLineInfo.GetValue(i), i);

               //DataSet ds = db.SelectRecords("SP_GetInvoiceDataImport", ColumnNames, Values, DataType);
               ds = db.SelectRecords("SP_GetInvoiceDataInvMatchTest", ColumnNames, Values, DataType);

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

       # endregion GetInvoiceDetails

       # region GetProformaInvoiceData
       public DataSet GetProformaInvoiceDataImport(object[] RateLineInfo)
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


               //DataSet ds = db.SelectRecords("SP_GetInvoiceDataImport", ColumnNames, Values, DataType);
               DataSet ds = db.SelectRecords("SP_GetProformaInvoiceDataInvMatch", ColumnNames, Values, DataType);
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

       # endregion GetInvoiceDetails



       # region GetWalkInAgentInvoiceData
       public DataSet GetWalkInAgentInvoiceData(object[] RateLineInfo)
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


               //DataSet ds = db.SelectRecords("SP_GetInvoiceDataImport", ColumnNames, Values, DataType);
               DataSet ds = db.SelectRecords("SP_GetWalkInInvoiceDataInvMatch", ColumnNames, Values, DataType);
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
       #endregion GetWalkInAgentInvoiceData


       # region GetDestAgentInvoiceData
       public DataSet GetDestAgentInvoiceData(object[] RateLineInfo)
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


               //DataSet ds = db.SelectRecords("SP_GetInvoiceDataImport", ColumnNames, Values, DataType);
               DataSet ds = db.SelectRecords("SP_GetDestInvoiceDataInvMatch", ColumnNames, Values, DataType);
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
       #endregion GetDestAgentInvoiceData


       # region GetInvoiceDataForPrint
       public DataSet GetInvoiceDataForPrint(object[] RateLineInfo)
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

               DataSet ds = db.SelectRecords("SP_GetInvoiceDataForPrint", ColumnNames, Values, DataType);
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

       # endregion GetInvoiceDataForPrint

       # region GetProformaInvoiceDataForPrint
       public DataSet GetProformaInvoiceDataForPrint(object[] RateLineInfo)
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

               DataSet ds = db.SelectRecords("SP_GetProformaInvoiceDataForPrint", ColumnNames, Values, DataType);
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

       # endregion GetProformaInvoiceDataForPrint

       # region GetWalkInInvoiceDataForPrint
       public DataSet GetWalkInInvoiceDataForPrint(object[] RateLineInfo)
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

               DataSet ds = db.SelectRecords("SP_GetWalkInInvoiceDataForPrint", ColumnNames, Values, DataType);
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

       # endregion GetWalkInInvoiceDataForPrint

       # region GetDestInvoiceDataForPrint
       public DataSet GetDestInvoiceDataForPrint(object[] RateLineInfo)
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

               DataSet ds = db.SelectRecords("SP_GetDestInvoiceDataForPrint", ColumnNames, Values, DataType);
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

       # endregion GetDestInvoiceDataForPrint

       # region get Configured Invoice Report Name
       public string getConfiguredInvoiceReportName()
       {
           try
           {
               return db.GetString("select top 1 value from tblConfiguration where Parameter = 'InvoiceReportName'");
           }
           catch (Exception ex)
           {
               return "";

           }
       }
       # endregion get Configured Invoice Report Name

        #region Export to ERP Data
       public DataSet getExportERPData(string[] ParamName,object[] ParamVal,SqlDbType[] ParamType)
       {


           try
           {
               //string[] QueryNames = { "InvoiceNumber", "InvoiceType","FromDate","ToDate"};
               //object[] QueryValues = { InvoiceNumber, InvoiceType, };
               //SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar };

               DataSet ds = db.SelectRecords("sp_GetERPExportData", ParamName, ParamVal, ParamType);

               if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
               {
                   return ds;
               }
               else
                   return null;
 
           }
           catch (Exception ex)
           { return null; }
       }

        #endregion

       #region Close invoices
       public string SetInvoiceStatustoClose(string InvoiceNumbers)
       {
           DataSet ds = new DataSet();
           try
           {
               SQLServer da = new SQLServer(constr);

               string[] QueryPname = new string[1];
               object[] QueryValue = new object[1];
               SqlDbType[] QueryType = new SqlDbType[1];

               //select palletnumber,brand,batchno,Actualquantity,ispicked,isplanned,updatedby,updatedon

               QueryPname[0] = "InvoiceNumber";
               QueryType[0] = SqlDbType.VarChar;
               QueryValue[0] = InvoiceNumbers;

               string res = db.GetStringByProcedure("SP_SetInvoiceStatustoClosed", QueryPname, QueryValue, QueryType);
               return res;

           }
           catch (Exception ex)
           {
               return "error";
           }
       }
       #endregion

    }
}
