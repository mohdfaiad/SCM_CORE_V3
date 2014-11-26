using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using QID.DataAccess;

namespace BAL
{
    public class DCMGenerateBAL
    {
        string constr = "";
        SQLServer db = new SQLServer(Global.GetConnectionString());
        DataSet ds;

        public DCMGenerateBAL()
        {
            constr = Global.GetConnectionString();
        }

        #region GetDCMNumber
        public DataSet GetDCMNumber()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetCurrentDCMNumber");
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

        #region Save DCM Processing
        public string SaveDCMProcessing(object[] AcEq)
        {
            try
            {
                string[] ColumnNames = new string[33];
                SqlDbType[] DataType = new SqlDbType[33];
                Object[] Values = new object[33];
                int i = 0;

                //0
                i = 0;
                ColumnNames.SetValue("DCMType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //1
                ColumnNames.SetValue("AWBPrefix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //2
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //3
                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //4
                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //5
                ColumnNames.SetValue("grossWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //6
                ColumnNames.SetValue("ChargbleWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //7
                ColumnNames.SetValue("FreightRate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //8
                ColumnNames.SetValue("OCDC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //9
                ColumnNames.SetValue("OCDA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //10
                ColumnNames.SetValue("ServiceTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //11
                ColumnNames.SetValue("Commission", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //12
                ColumnNames.SetValue("TDSComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //13
                ColumnNames.SetValue("STComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //14
                ColumnNames.SetValue("Total", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //15
                ColumnNames.SetValue("RevisedFlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //16
                ColumnNames.SetValue("RevisedgrossWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //17
                ColumnNames.SetValue("RevisedChargbleWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //18
                ColumnNames.SetValue("RevisedFreightRate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //19
                ColumnNames.SetValue("RevisedOCDC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //20
                ColumnNames.SetValue("RevisedOCDA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //21
                ColumnNames.SetValue("RevisedServiceTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //22
                ColumnNames.SetValue("RevisedCommission", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //23
                ColumnNames.SetValue("RevisedTDSComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //24
                ColumnNames.SetValue("RevisedSTComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //25
                ColumnNames.SetValue("RevisedTotal", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //26
                ColumnNames.SetValue("CreatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //27
                ColumnNames.SetValue("Remarks", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //28
                ColumnNames.SetValue("FlightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //29
                ColumnNames.SetValue("RevisedFlightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;
                //30
                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                i++;
                //31
                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(AcEq.GetValue(i-1), i);
                i++;
                ColumnNames.SetValue("DCMNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i-1), i);
                i++;
                string res = db.GetStringByProcedure("spSaveDCMAWBAndDeals", ColumnNames, Values, DataType);
                return res;

            }
            catch (Exception ex)
            {
                return "error";
            }
        }
        #endregion SaveDCMProcessing

        #region Fill Current DCM
        public DataSet FillCurrentDCM(string AWBNumber, string InvoiceNumber, string FlightNo, string FlightDate)
        {
            try
            {
                string[] ParamNames = new string[] 
                {
                     "AWBNumber", "InvoiceNo", "FlightNo","FlightDate" 
                };
                DateTime FlightDt = DateTime.ParseExact(FlightDate, "dd/MM/yyyy", null);
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] paramvalue = new object[4];
                paramvalue[0] = AWBNumber;
                paramvalue[1] = InvoiceNumber;
                paramvalue[2] = FlightNo;
                paramvalue[3] = FlightDt.ToString();


                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spFillCurrentDCM", ParamNames, paramvalue, ParamTypes);
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
        #region Fill Prev AND Current Details
        public DataSet FillDCMDetails(string strDCMNumber, string strAWBPre, string strAWBNumber)
        {
            try
            {
                string[] ParamNames = new string[] {  "DCMNumber","AWBPre","AWBNumber" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                object[] paramvalue = new object[3] { strDCMNumber,strAWBPre,strAWBNumber};

                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("[SP_SelectDCMDetails]", ParamNames, paramvalue, ParamTypes);
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

        #region Fill Current DCM Deals
        public DataSet FillCurrentDCMDeals(string InvoiceNumber)
        {
            try
            {
                string[] ParamNames = new string[] 
                {
                     "InvoiceNo"
                };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar};
                object[] paramvalue = new object[1];
                paramvalue[0] = InvoiceNumber;

                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spFillCurrentDCMDeals", ParamNames, paramvalue, ParamTypes);
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

        #region List DCM
        public DataSet ListDCM(object[] objDCM)
        {
            try
            {
                string[] ParamNames = new string[] { "DCMNumber", "AWBNumber", "InvoiceNo", "AgentCode","FromDate","ToDate","DCMType" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SpGetDCMProcessing", ParamNames, objDCM, ParamTypes);
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

        #region List DCM deals
        public DataSet ListDCMDeals(object[] objDCM)
        {
            try
            {
                string[] ParamNames = new string[] { "DCMNumber", "AWBNumber", "InvoiceNo", "AgentCode", "FromDate", "ToDate", "DCMType" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SpGetDCMAWBDeal", ParamNames, objDCM, ParamTypes);
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

        #region GetDataFromAgent
        public DataSet GetDataFromAgent(string AWBno,string InvoiceNo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] paramname = new string[2];
                paramname[0] = "AWBno";
                paramname[1] = "InvoiceNo";

                object[] paramvalue = new object[2];
                paramvalue[0] = AWBno;
                paramvalue[1] = InvoiceNo ;

                SqlDbType[] paramtype = new SqlDbType[2];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;

                ds = da.SelectRecords("Sp_GetAgentDetailsDCMNew", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                return ds;
            }
            return ds;
        }
        #endregion GetDataFromAgent

        #region Get DCM List
        public DataSet GetDCMList(string DCMNumber, string FromDate, string ToDate, string DCMType)
        {
            DataSet ds = new DataSet();
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] QueryPname = new string[4];
                object[] QueryValue = new object[4];
                SqlDbType[] QueryType = new SqlDbType[4];

                //select palletnumber,brand,batchno,Actualquantity,ispicked,isplanned,updatedby,updatedon

                QueryPname[0] = "DCMNumber";
                QueryPname[1] = "FromDate";
                QueryPname[2] = "ToDate";
                QueryPname[3] = "DCMType";

                QueryType[0] = SqlDbType.VarChar;
                QueryType[1] = SqlDbType.VarChar;
                QueryType[2] = SqlDbType.VarChar;
                QueryType[3] = SqlDbType.VarChar;

                QueryValue[0] = DCMNumber;
                QueryValue[1] = FromDate;
                QueryValue[2] = ToDate;
                QueryValue[3] = DCMType;

                ds = da.SelectRecords("SP_GetDCMListData", QueryPname, QueryValue, QueryType);


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
        #endregion Get DCM List


        #region GetInvoiceNumber
        public string GetInvoiceNumber(string AWBNo)
        {
            try
            {
                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBNo, i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("sp_GetInvoiceNoFromAWBNo", ColumnNames, Values, DataType);
                return res;
            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        #endregion GetInvoiceNumber

        #region Export To ERP
        public DataSet GetExportERPData(string[] ParamName, object[] ParamVal, SqlDbType[] ParamType)
        {
            try
            {
                DataSet ds = db.SelectRecords("spExportERPDCM",ParamName, ParamVal,ParamType);
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


    }
}
