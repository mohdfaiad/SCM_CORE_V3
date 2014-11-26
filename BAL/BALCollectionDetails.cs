using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Configuration;
using System.Data;

namespace BAL
{
    public class BALCollectionDetails
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        #region Get Collection details Data
        public DataSet GetCollectionDetailsdata(string AgentCode, string BillingType, string Origin, string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] QueryPname = new string[5];
                object[] QueryValue = new object[5];
                SqlDbType[] QueryType = new SqlDbType[5];

                //select palletnumber,brand,batchno,Actualquantity,ispicked,isplanned,updatedby,updatedon

                QueryPname[0] = "AgentCode";
                QueryPname[1] = "BillingType";
                QueryPname[2] = "Origin";
                QueryPname[3] = "FromDate";
                QueryPname[4] = "ToDate";


                QueryType[0] = SqlDbType.VarChar;
                QueryType[1] = SqlDbType.VarChar;
                QueryType[2] = SqlDbType.VarChar;
                QueryType[3] = SqlDbType.VarChar;
                QueryType[4] = SqlDbType.VarChar;


                QueryValue[0] = AgentCode;
                QueryValue[1] = BillingType;
                QueryValue[2] = Origin;
                QueryValue[3] = FromDate;
                QueryValue[4] = ToDate;

                ds = da.SelectRecords("SP_GetCollectionDetailsdata", QueryPname, QueryValue, QueryType);


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
        #endregion Get Collection details Data

        #region Get Collection Master Data
        public DataSet GetCollectionMasterData(string AgentCode, string BillingType, string Origin, string FromDate, string ToDate, string InvoiceNumber, string AWBNumber,string Collection)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] QueryPname = new string[8];
                object[] QueryValue = new object[8];
                SqlDbType[] QueryType = new SqlDbType[8];

                //select palletnumber,brand,batchno,Actualquantity,ispicked,isplanned,updatedby,updatedon

                QueryPname[0] = "AgentCode";
                QueryPname[1] = "BillingType";
                QueryPname[2] = "Origin";
                QueryPname[3] = "FromDate";
                QueryPname[4] = "ToDate";
                QueryPname[5] = "InvoiceNumber";
                QueryPname[6] = "AWBNumber";
                QueryPname[7] = "Collection";

                QueryType[0] = SqlDbType.VarChar;
                QueryType[1] = SqlDbType.VarChar;
                QueryType[2] = SqlDbType.VarChar;
                QueryType[3] = SqlDbType.VarChar;
                QueryType[4] = SqlDbType.VarChar;
                QueryType[5] = SqlDbType.VarChar;
                QueryType[6] = SqlDbType.VarChar;
                QueryType[7] = SqlDbType.VarChar;

                QueryValue[0] = AgentCode;
                QueryValue[1] = BillingType;
                QueryValue[2] = Origin;
                QueryValue[3] = FromDate;
                QueryValue[4] = ToDate;
                QueryValue[5] = InvoiceNumber ;
                QueryValue[6] = AWBNumber;
                QueryValue[7] = Collection;


                ds = da.SelectRecords("SP_GetCollectionMasterDataNew", QueryPname, QueryValue, QueryType);


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
        #endregion Get Collection master Data

        #region Get Collection WalkIn Master Data
        public DataSet GetCollectionWalkInMasterData(string AgentCode, string BillingType, string Origin, string FromDate, string ToDate, string InvoiceNumber, string AWBNumber,string Collection)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] QueryPname = new string[8];
                object[] QueryValue = new object[8];
                SqlDbType[] QueryType = new SqlDbType[8];

                //select palletnumber,brand,batchno,Actualquantity,ispicked,isplanned,updatedby,updatedon

                QueryPname[0] = "AgentCode";
                QueryPname[1] = "BillingType";
                QueryPname[2] = "Origin";
                QueryPname[3] = "FromDate";
                QueryPname[4] = "ToDate";
                QueryPname[5] = "InvoiceNumber";
                QueryPname[6] = "AWBNumber";
                QueryPname[7] = "Collection";

                QueryType[0] = SqlDbType.VarChar;
                QueryType[1] = SqlDbType.VarChar;
                QueryType[2] = SqlDbType.VarChar;
                QueryType[3] = SqlDbType.VarChar;
                QueryType[4] = SqlDbType.VarChar;
                QueryType[5] = SqlDbType.VarChar;
                QueryType[6] = SqlDbType.VarChar;
                QueryType[7] = SqlDbType.VarChar;

                QueryValue[0] = AgentCode;
                QueryValue[1] = BillingType;
                QueryValue[2] = Origin;
                QueryValue[3] = FromDate;
                QueryValue[4] = ToDate;
                QueryValue[5] = InvoiceNumber;
                QueryValue[6] = AWBNumber;
                QueryValue[7] = Collection;



                ds = da.SelectRecords("SP_GetCollectionWalkInMasterDataNew", QueryPname, QueryValue, QueryType);


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
        #endregion Get Collection WalkIn master Data

        #region Get Collection WalkIn New
        public DataSet GetCollectionWalkInDataNew(string FromDate, string ToDate, string UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] QueryPname = new string[3];
                object[] QueryValue = new object[3];
                SqlDbType[] QueryType = new SqlDbType[3];

                //select palletnumber,brand,batchno,Actualquantity,ispicked,isplanned,updatedby,updatedon

                QueryPname[0] = "FromDate";
                QueryPname[1] = "ToDate";
                QueryPname[2] = "UserID";
                
                QueryType[0] = SqlDbType.VarChar;
                QueryType[1] = SqlDbType.VarChar;
                QueryType[2] = SqlDbType.VarChar;

                QueryValue[0] = FromDate;
                QueryValue[1] = ToDate;
                QueryValue[2] = UserID;


                ds = da.SelectRecords("sp_GetWalkinCollectionDetails", QueryPname, QueryValue, QueryType);


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
        #endregion Get Collection WalkIn master Data

        #region Get Collection Dest Master Data
        public DataSet GetCollectionDestMasterData(string AgentCode, string BillingType, string Origin, string FromDate, string ToDate, string InvoiceNumber, string AWBNumber,string Collection)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] QueryPname = new string[8];
                object[] QueryValue = new object[8];
                SqlDbType[] QueryType = new SqlDbType[8];

                //select palletnumber,brand,batchno,Actualquantity,ispicked,isplanned,updatedby,updatedon

                QueryPname[0] = "AgentCode";
                QueryPname[1] = "BillingType";
                QueryPname[2] = "Origin";
                QueryPname[3] = "FromDate";
                QueryPname[4] = "ToDate";
                QueryPname[5] = "InvoiceNumber";
                QueryPname[6] = "AWBNumber";
                QueryPname[7] = "Collection";


                QueryType[0] = SqlDbType.VarChar;
                QueryType[1] = SqlDbType.VarChar;
                QueryType[2] = SqlDbType.VarChar;
                QueryType[3] = SqlDbType.VarChar;
                QueryType[4] = SqlDbType.VarChar;
                QueryType[5] = SqlDbType.VarChar;
                QueryType[6] = SqlDbType.VarChar;
                QueryType[7] = SqlDbType.VarChar;


                QueryValue[0] = AgentCode;
                QueryValue[1] = BillingType;
                QueryValue[2] = Origin;
                QueryValue[3] = FromDate;
                QueryValue[4] = ToDate;
                QueryValue[5] = InvoiceNumber;
                QueryValue[6] = AWBNumber;
                QueryValue[7] = Collection;

                ds = da.SelectRecords("SP_GetCollectionDestMasterDataNew", QueryPname, QueryValue, QueryType);


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
        #endregion Get Collection Dest master Data

        # region  Update collection details
        public string UpdateInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PendingAmount", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_UpdateInvoiceCollectionDetails", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Update collection details

        # region  Add collection details
        public string AddInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[17];
                SqlDbType[] DataType = new SqlDbType[17];
                Object[] Values = new object[17];
                int i = 0;
                i = 0;
                //0
                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                //1
                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                //2
                ColumnNames.SetValue("TDSAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                //3
                ColumnNames.SetValue("PaymentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                //4
                ColumnNames.SetValue("ChequeDdNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                //5
                ColumnNames.SetValue("ChequeDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                //6
                ColumnNames.SetValue("BankName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                //7
                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                //8
                //added by jayant
                ColumnNames.SetValue("PPRemarks", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                ColumnNames.SetValue("Amt194C", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                ColumnNames.SetValue("TransactionId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                ColumnNames.SetValue("DepositDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("VATAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TINNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ORNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                

                


                string res = db.GetStringByProcedure("SP_AddInvoiceCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Add collection details

        # region  Add WalkIn collection details
        public string AddWalkInInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[16];
                SqlDbType[] DataType = new SqlDbType[16];
                Object[] Values = new object[16];
                int i = 0;
                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PaymentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDdNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("BankName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //added by jayant
                ColumnNames.SetValue("PPRemarks", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                //end

                ColumnNames.SetValue("Amt194C", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TransactionId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DepositDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("VATAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ORNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_AddWalkInInvoiceCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Add WalkIn collection details

        # region  Add Dest collection details
        public string AddDestInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[17];
                SqlDbType[] DataType = new SqlDbType[17];
                Object[] Values = new object[17];
                int i = 0;
                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PaymentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDdNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("BankName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PPRemarks", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Amt194C", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TransactionId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DepositDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("VATAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TINNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ORNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_AddDestInvoiceCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Add Dest collection details

        # region  Add DCM collection details
        public string AddInvoiceDCMCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[8];
                SqlDbType[] DataType = new SqlDbType[8];
                Object[] Values = new object[8];
                int i = 0;
                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DCMNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DCMAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DCMType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DCMReason", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                i++;

                ColumnNames.SetValue("UpdtOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                

                string res = db.GetStringByProcedure("SP_AddInvoiceDCMCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Update collection details

        # region  Add WalkIn DCM collection details
        public string AddWalkInInvoiceDCMCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[8];
                SqlDbType[] DataType = new SqlDbType[8];
                Object[] Values = new object[8];
                int i = 0;
                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DCMNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DCMAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DCMType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DCMReason", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                i++;

                ColumnNames.SetValue("UpdtOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);

                string res = db.GetStringByProcedure("SP_AddWalkInInvoiceDCMCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Add WalkIn DCM collection details

        # region  Add Dest DCM collection details
        public string AddDestInvoiceDCMCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[8];
                SqlDbType[] DataType = new SqlDbType[8];
                Object[] Values = new object[8];
                int i = 0;
                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DCMNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DCMAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DCMType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DCMReason", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                i++;

                ColumnNames.SetValue("UpdtOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);


                string res = db.GetStringByProcedure("SP_AddDestInvoiceDCMCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Add Dest DCM collection details

        # region  Edit collection details
        public string EditInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[14];
                SqlDbType[] DataType = new SqlDbType[14];
                Object[] Values = new object[14];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("SerialNumber", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PaymentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDdNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("BankName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                //added by jayant
                ColumnNames.SetValue("PPRemarks", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Amt194C", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DepositDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("VATAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                //i++;

                string res = db.GetStringByProcedure("SP_EditInvoiceCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Edit collection details

        # region  Edit WalkIn collection details
        public string EditWalkInInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[14];
                SqlDbType[] DataType = new SqlDbType[14];
                Object[] Values = new object[14];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("SerialNumber", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PaymentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDdNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("BankName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PPRemarks", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Amt194C", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DepositDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("VATAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_EditWalkInInvoiceCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Edit WalkIn collection details

        # region  Edit Dest collection details
        public string EditDestInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[14];
                SqlDbType[] DataType = new SqlDbType[14];
                Object[] Values = new object[14];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("SerialNumber", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PaymentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDdNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("BankName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PPRemarks", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Amt194C", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DepositDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("VATAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);



                string res = db.GetStringByProcedure("SP_EditDestInvoiceCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Edit Dest collection details

        # region  Delete collection details
        public string DeleteInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[9];
                SqlDbType[] DataType = new SqlDbType[9];
                Object[] Values = new object[9];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("SerialNumber", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PaymentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDdNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ORNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_DeleteInvoiceCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Delete collection details

        # region  Delete WalkIn collection details
        public string DeleteWalkInInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[7];
                SqlDbType[] DataType = new SqlDbType[7];
                Object[] Values = new object[7];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("SerialNumber", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PaymentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDdNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_DeleteWalkInInvoiceCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Delete WalkIn collection details

        # region  Delete Dest collection details
        public string DeleteDestInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[9];
                SqlDbType[] DataType = new SqlDbType[9];
                Object[] Values = new object[9];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("SerialNumber", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PaymentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDdNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ORNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);


                string res = db.GetStringByProcedure("SP_DeleteDestInvoiceCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Delete Dest collection details

        #region ListHandoverDetails
        public DataSet ListHandoverDetails(string HandoverBy )
        { 
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);
               string[] QueryPname = new string[1];
                object[] QueryValue = new object[1];
                SqlDbType[] QueryType = new SqlDbType[1];

                //select palletnumber,brand,batchno,Actualquantity,ispicked,isplanned,updatedby,updatedon

                QueryPname[0] = "HandoverBy";

                QueryType[0] = SqlDbType.VarChar;

                QueryValue[0] = HandoverBy;

                ds = da.SelectRecords("sp_GetHandoverDetails",QueryPname,QueryValue,QueryType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                    else
                    {
                        return null;
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Insert Handover Details
        public bool InsertHandoverDetails(DateTime HandoverDate,string HandoverBy,string HandoverStation,string HandoverTo,double HandoverAmount,string Remarks,string UpdatedBy,DateTime UpdatedOn)
        {
            try
            {
                string[] ColumnNames = new string[8];
                SqlDbType[] DataType = new SqlDbType[8];
                Object[] Values = new object[8];
                int i = 0;
                i = 0;

                ColumnNames.SetValue("HandoverDate", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(HandoverDate, i);
                i++;

                ColumnNames.SetValue("HandoverBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HandoverBy, i);
                i++;

                ColumnNames.SetValue("HandoverStation", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HandoverStation, i);
                i++;

                ColumnNames.SetValue("HandoverTo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(HandoverTo, i);
                i++;

                ColumnNames.SetValue("HandoverAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(HandoverAmount, i);
                i++;

                ColumnNames.SetValue("Remarks", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(Remarks, i);
                i++;

                ColumnNames.SetValue("UpdatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UpdatedBy, i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(UpdatedOn, i);

                bool res = db.ExecuteProcedure("sp_PutHandoverDetails", ColumnNames, DataType, Values);
                return res;
            }

            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Get All Payment Types
        public DataSet GetAllPaymentTypes()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SPGetPaymentTypes");
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
        #endregion Get All Payment Types

        #region Getting OR Reciept Details
        public DataSet GetORDetailsRegularCollection(string ORNumber, string Station, DateTime IssuingDate, string CollectionType)
        {
            try
            {
                DataSet ds;
                string[] QueryNames = { "ORNumber", "Station", "Date" };
                object[] QueryValues = { ORNumber, Station, IssuingDate };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime };
                if (CollectionType == "Walkin")
                {
                    ds = db.SelectRecords("sp_GetORDetails_WalkinCollection", QueryNames, QueryValues, QueryTypes);
                }
                else
                    if (CollectionType == "Destination")
                    {
                        ds = db.SelectRecords("sp_GetORDetails_DestCollection", QueryNames, QueryValues, QueryTypes);
                    }
                    else
                    {
                        ds = db.SelectRecords("sp_GetORDetails_RegularCollection", QueryNames, QueryValues, QueryTypes);
                    }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                { return ds; }
                else {return null; }

            }
            catch (Exception ex)
            { return null; }

        }
        #endregion

        #region Getting OR Reciept Details Calculation
        public DataSet GetORDetailsRegularCollectionCalculation(string ORNumber, string Station, DateTime IssuingDate, string CollectionType)
        {
            try
            {
                DataSet ds;
                string[] QueryNames = { "ORNumber", "Station", "Date" };
                object[] QueryValues = { ORNumber, Station, IssuingDate };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime };
                if (CollectionType == "Walkin")
                {
                    ds = db.SelectRecords("sp_GetORDetails_WalkinCollectionCalc", QueryNames, QueryValues, QueryTypes);
                }
                else
                    if (CollectionType == "Destination")
                    {
                        ds = db.SelectRecords("sp_GetORDetails_DestCollectionCalc", QueryNames, QueryValues, QueryTypes);
                    }
                    else
                    {
                        ds = db.SelectRecords("sp_GetORDetails_RegularCollectionCalc", QueryNames, QueryValues, QueryTypes);
                    }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                { return ds; }
                else { return null; }

            }
            catch (Exception ex)
            { return null; }

        }
        #endregion

        # region "Cash Management"
        public void GetUserOpeningBalance(string UserName, string InvoiceType, ref decimal OpeningBal, ref string DataIds)
        {
            try
            {
                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;
                DataSet objDS = null;
                
                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserName, i);
                i++;

                ColumnNames.SetValue("InvoiceType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(InvoiceType, i);

                objDS = db.SelectRecords("sp_GetUserOpeningBalance", ColumnNames, Values, DataType);

                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    OpeningBal= Convert.ToDecimal(objDS.Tables[0].Rows[0]["Opening Balance"]);
                }

                if (objDS != null && objDS.Tables.Count > 1 && objDS.Tables[1].Rows.Count > 0)
                {
                    DataIds = Convert.ToString(objDS.Tables[1].Rows[0]["SerialNumbers"]);
                }
            }

            catch (Exception ex)
            {
            }
        }

        public bool RecordPostingAmount(string DataIds, string UserName, DateTime TimeStamp, string InvoiceType)
        {
            string[] ColumnNames = new string[4];
            SqlDbType[] DataType = new SqlDbType[4];
            Object[] Values = new object[4];
            bool blnResult = false;
            int i = 0;

            try
            {   
                ColumnNames.SetValue("DataIds", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(DataIds, i);
                i++;

                ColumnNames.SetValue("PostedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserName, i);
                i++;

                ColumnNames.SetValue("PostedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(TimeStamp, i);
                i++;

                ColumnNames.SetValue("InvoiceType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(InvoiceType, i);

                blnResult = db.ExecuteProcedure("sp_RecordPostingAmount", ColumnNames, DataType, Values);
            }

            catch (Exception ex)
            {
                blnResult = false;
            }

            return blnResult;
        }
        # endregion Add WalkIn DCM collection details

        #region Get All Billing Invoice Types
        public DataSet GetAllBillingInvoiceTypes()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SPGetBillingInvoiceTypes");
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
        #endregion Get All Billing Invoice Types

        # region  Add AWB level collection details
        public string AddAWBCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[19];
                SqlDbType[] DataType = new SqlDbType[19];
                Object[] Values = new object[19];
                int i = 0;
                i = 0;

                ColumnNames.SetValue("SrNo", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                ColumnNames.SetValue("TDSAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                ColumnNames.SetValue("PaymentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                ColumnNames.SetValue("ChequeDdNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                ColumnNames.SetValue("ChequeDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                ColumnNames.SetValue("BankName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                
                ColumnNames.SetValue("PPRemarks", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Amt194C", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TransactionId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ORNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DepositDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("VATAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TINNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);


                string res = db.GetStringByProcedure("SP_AddAgentAWBCollection", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Add AWB level collection details

        # region  Update Invoice level collection from AWB level collection details
        public string UpdateAgentInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[5];
                SqlDbType[] DataType = new SqlDbType[5];
                Object[] Values = new object[5];
                int i = 0;
                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ORNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);


                string res = db.GetStringByProcedure("SP_UpdateAgentInvoiceAWBCollection", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Update Invoice level collection from AWB level collection details

        # region get AWB Level Collection Value
        public string getAWBLevelCollectionValue()
        {
            try
            {
                return db.GetString("select top 1 isnull(value,'') from tblConfiguration where Parameter = 'AWBLevelCollection'");
            }
            catch (Exception ex)
            {
                return "";

            }
        }
        # endregion get AWB Level Collection Value

        #region Check AWB level Collection exists
        public string CheckAWBCollectionForInvoice(string InvoiceNumber)
        {
            string res = "";
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] QueryPname = new string[1];
                object[] QueryValue = new object[1];
                SqlDbType[] QueryType = new SqlDbType[1];

                QueryPname[0] = "InvoiceNumber";
                QueryType[0] = SqlDbType.VarChar;
                QueryValue[0] = InvoiceNumber;

                res = da.GetStringByProcedure("SP_CheckAWBCollectionForInvoice", QueryPname, QueryValue, QueryType);

                return res;
            }
            catch (Exception ex)
            {
                return "error";
            }
        }
        #endregion Check AWB level Collection exists

        # region  get next OR number
        public string GetNextORNumber(string strRotationID, int intYear, string username)
        {
            try
            {
                string[] PName = new string[4];
                PName[0] = "strRotationId";
                PName[1] = "intYear";
                PName[2] = "strUserName";
                PName[3] = "strOutput";

                object[] PValue = new object[4];
                PValue[0] = strRotationID;
                PValue[1] = 0;//txtFrmDate.Text;
                PValue[2] = username;//txtToDate.Text;
                PValue[3] = ParameterDirection.Output;

                SqlDbType[] PType = new SqlDbType[4];
                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.SmallInt;
                PType[2] = SqlDbType.VarChar;
                PType[3] = SqlDbType.VarChar;

                
                DataSet res = db.SelectRecords("spGenerateRotationNoNew", PName, PValue, PType);
                string s = res.Tables[0].Rows[0][0].ToString();
                return s;

            }
            catch (Exception ex)
            {
                return "";
            }
        }
        # endregion  get next OR number

        # region  Add WalkIn collection details for Multiple AWBs
        public string AddMultipleWalkInInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[17];
                SqlDbType[] DataType = new SqlDbType[17];
                Object[] Values = new object[17];
                int i = 0;
                i = 0;

                ColumnNames.SetValue("ORRecieptNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PaymentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDdNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("BankName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //added by jayant
                ColumnNames.SetValue("PPRemarks", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;
                //end

                ColumnNames.SetValue("Amt194C", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TransactionId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DepositDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("VATAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TINNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_AddMultipleWalkInInvoiceCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Add WalkIn collection details for Multiple AWBs

        # region  Delete Multiple WalkIn collection details
        public string DeleteMultipleWalkInInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[9];
                SqlDbType[] DataType = new SqlDbType[9];
                Object[] Values = new object[9];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("SerialNumber", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PaymentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDdNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ORNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_DeleteMultipleWalkInInvoiceCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Delete Multiple WalkIn collection details

        # region  Edit Multiple WalkIn collection details
        public string EditMultipleWalkInInvoiceCollectionDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[14];
                SqlDbType[] DataType = new SqlDbType[14];
                Object[] Values = new object[14];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("SerialNumber", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CollectedAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PaymentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDdNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChequeDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("BankName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("PPRemarks", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Amt194C", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DepositDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("VATAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_EditMultipleWalkInInvoiceCollectionMaster", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Edit Multiple WalkIn collection details

        # region  AddDestInvoiceWaivedOffDetails
        public string AddDestInvoiceWaivedOffDetails(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[7];
                SqlDbType[] DataType = new SqlDbType[7];
                Object[] Values = new object[7];
                int i = 0;
                i = 0;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("WaiveAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("IssueBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Reason", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AddedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                
                i++;
                ColumnNames.SetValue("InvoiceType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                
                i++;
                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_AddWaiveOffCharges", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Add Dest DCM collection detailss

        #region ValidateDCMNumber
        public bool ValidateDCMNumber(string DCMNumber, string InvoiceNumber)
        {
            DataSet dsResult = null;
            SQLServer da = null;

            try
            {
                string[] pname = { "DCMNumber", "InvoiceNumber" };
                object[] pvalue = { DCMNumber, InvoiceNumber };
                SqlDbType[] ptype = { SqlDbType.VarChar, SqlDbType.VarChar };
                da = new SQLServer(Global.GetConnectionString());
                dsResult = da.SelectRecords("SP_DCMValidation", pname, pvalue, ptype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {

                        if (dsResult.Tables[0].Rows.Count != 0)
                        {
                            return bool.Parse(dsResult.Tables[0].Rows[0][0].ToString());
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                dsResult = null;
                da = null;
            }
        }
        #endregion
    }
}
