using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using QID.DataAccess;

namespace BAL
{
    
    public class DebitCreditProcessingBAL
    {
        string constr = "";
        SQLServer db = new SQLServer(Global.GetConnectionString());
        DataSet ds;
        public DebitCreditProcessingBAL()
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

        #region SaveDCMProcessing
        public string SaveDCMProcessing(object[] AcEq)
        {
            try
            {
                string[] ColumnNames = new string[25];
                SqlDbType[] DataType = new SqlDbType[25];
                Object[] Values = new object[25];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("grossWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("ChargbleWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("FreightRate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("OCDC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("OCDA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;


                ColumnNames.SetValue("ServiceTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Commission", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("STComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Total", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                
                ColumnNames.SetValue("RevisedgrossWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;


                ColumnNames.SetValue("RevisedChargbleWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;


                ColumnNames.SetValue("RevisedFreightRate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RevisedOCDC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;


                ColumnNames.SetValue("RevisedOCDA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;


                ColumnNames.SetValue("RevisedServiceTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RevisedCommission", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RevisedTDSComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;


                ColumnNames.SetValue("RevisedSTComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RevisedTotal", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Remarks", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CreatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("spSaveDCMAWBProcessingNew", ColumnNames, Values, DataType);
                return res;
            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        #endregion SaveDCMProcessing

        #region Fill Current DCM
        public DataSet FillCurrentDCM(string AWBNumber,string InvoiceNumber)
        {
            try
            {
                string[] ParamNames = new string[] 
                {
                     "AWBNumber", "InvoiceNo" 
                };
                SqlDbType[] ParamTypes = new SqlDbType[]
                { SqlDbType.VarChar, SqlDbType.VarChar};
                object[] paramvalue = new object[2];
                paramvalue[0] = AWBNumber;
                paramvalue[1] = InvoiceNumber;  

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

                ds = da.SelectRecords("Sp_GetAgentDetailsDCM", paramname, paramvalue, paramtype);
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


    }
}
