using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;

namespace BAL
{
   public  class CCABal
    {
         string constr = "";
        SQLServer db = new SQLServer(Global.GetConnectionString());
        DataSet ds;
        public CCABal()
        {
            constr = Global.GetConnectionString();
        }

        #region GetCCANumber
        public DataSet GetCCANumber()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetCurrentCCANumber");
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

        #region SaveCCAProcessing
        public string SaveCCAProcessing(object[] AcEq)
        {
            try
            {
                string[] ColumnNames = new string[37];
                SqlDbType[] DataType = new SqlDbType[37];
                Object[] Values = new object[37];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("AWBPrefix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

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

                ColumnNames.SetValue("Discount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RevisedDiscount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ValCharges", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RevisedValCharges", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                
                ColumnNames.SetValue("Status", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CCANumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FLightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CommodityCodeOrg", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CommodityCodeRev", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(AcEq.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("spSaveCCAAWBProcessingNewTest", ColumnNames, Values, DataType);
                return res;
            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        #endregion SaveCCAProcessing

        #region Fill Current CCA
        public DataSet FillCurrentCCA(string AWBNumber,string InvoiceNumber)
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
                DataSet ds = da.SelectRecords("spFillCurrentCCANew", ParamNames, paramvalue, ParamTypes);
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

        #region List CCA
        public DataSet ListCCA(object[] objCCA)
        {
            try
            {
                string[] ParamNames = new string[] { "CCANumber", "AWBNumber", "InvoiceNo", "AgentCode","FromDate","ToDate","CCAType","Status" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SpGetCCAProcessing", ParamNames, objCCA, ParamTypes);
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

                ds = da.SelectRecords("Sp_GetAgentDetailsCCA", paramname, paramvalue, paramtype);
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

        #region Update Status
        public bool UpdateStatus(object[] Values)
        {
            try
            {
                string[] QueryNames = { "CCANumber", "AWBNo","Status","InvoiceNo" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar };
                if (db.UpdateData("SP_UpdateCCAStatus", QueryNames, QueryTypes, Values))
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        #endregion


        #region CCA Details

        public DataSet GetCCADetails(string CCANo, string AWBNo,string FlightNo,DateTime FlightDate)
        {
            try
            {
                string[] QueryNames = new string[4];
                object[] QueryValues = new object[4];
                SqlDbType[] QueryTypes = new SqlDbType[4];

                QueryNames[0] = "CCANo";
                QueryNames[1] = "AWBNo";
                QueryNames[2] = "FlightNo";
                QueryNames[3] = "FlightDate";

                QueryValues[0] = CCANo;
                QueryValues[1] = AWBNo;
                QueryValues[2] = FlightNo;
                QueryValues[3] = FlightDate;

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.DateTime;

                ds = db.SelectRecords("SPGetCCADetails", QueryNames, QueryValues, QueryTypes);
                
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetCCADetails(string CCANo, string AWBNo)
        {
            try
            {
                string[] QueryNames = new string[2];
                object[] QueryValues = new object[2];
                SqlDbType[] QueryTypes = new SqlDbType[2];

                QueryNames[0] = "CCANo";
                QueryNames[1] = "AWBNo";

                QueryValues[0] = CCANo;
                QueryValues[1] = AWBNo;

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;

                ds = db.SelectRecords("SPGetCCADetails", QueryNames, QueryValues, QueryTypes);

                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #endregion


    }
}
