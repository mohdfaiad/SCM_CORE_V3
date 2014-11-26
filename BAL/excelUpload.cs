using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class excelUpload
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());

        public  bool UploadPaxLoad(object[] Paxdata)
        {
            bool IsInsert = false;
            try
            {
                string[] paramNames = new string[14];
                SqlDbType[] dataTypes = new SqlDbType[14];
                int i = 0;

                //0
                paramNames.SetValue("EquipmentType", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //1
                paramNames.SetValue("DepartureDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //2
                paramNames.SetValue("Origin", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //3
                paramNames.SetValue("Destination", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //4
                paramNames.SetValue("CarrierCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //5
                paramNames.SetValue("FlightID", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //6
                paramNames.SetValue("OpSuffix", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //7
                paramNames.SetValue("LidOrCap", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //8
                paramNames.SetValue("Sold", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //9
                paramNames.SetValue("NonStopSold", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //10
                paramNames.SetValue("ThruSold", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;

                //11
                paramNames.SetValue("CnxSold", i);
                dataTypes.SetValue(SqlDbType.Float, i);
                i++;


                //12
                paramNames.SetValue("CreatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;

                //13
                paramNames.SetValue("CreatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
               

                 IsInsert = db.InsertData("spSaveFlightPaxLoad", paramNames, dataTypes, Paxdata);
                           
            
            }
            catch (Exception ex)
            {
                IsInsert = false;
            }

            return IsInsert;

        }

        public DataSet paxLoadList(DateTime frmDate, DateTime toDate)
        {
            DataSet ds = new DataSet();
            try
            {
               
                string[] colNames = new string[2];
                object[] values = new object[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;

                colNames.SetValue("frmDate", i);
                values.SetValue(frmDate, i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                i++;


                colNames.SetValue("toDate", i);
                values.SetValue(toDate, i);
                dataTypes.SetValue(SqlDbType.DateTime, i);


                ds = db.SelectRecords("spGetPaxLoadDetails", colNames, values, dataTypes);
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
            catch (Exception ex)
            {
            }
            return ds;
        }

        public bool CreateUploadError(string FileName, string TableName, int RecordNo, string ErrorDesp, DateTime TimeStamp, string UserName)
        {
            bool IsInsert = false;
            try
            {
                string[] paramNames = new string[6];
                SqlDbType[] dataTypes = new SqlDbType[6];
                string[] Paxdata = new string[6];
                int i = 0;

                //0
                paramNames.SetValue("FileName", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                Paxdata.SetValue(FileName,i);
                i++;

                //1
                paramNames.SetValue("TableName", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                Paxdata.SetValue(TableName, i);
                i++;

                //2
                paramNames.SetValue("RecordNumber", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                Paxdata.SetValue(RecordNo.ToString(), i);
                i++;

                //3
                paramNames.SetValue("ErrorDesc", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                Paxdata.SetValue(ErrorDesp, i);
                i++;

                //4
                paramNames.SetValue("CreatedOn", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                Paxdata.SetValue(TimeStamp.ToString(), i);
                i++;

                //5
                paramNames.SetValue("CreatedBy", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                Paxdata.SetValue(UserName, i);

                IsInsert = db.InsertData("sp_CreateUploadError", paramNames, dataTypes, Paxdata);
            }
            catch (Exception ex)
            {
                IsInsert = false;
            }

            return IsInsert;

        }

        public bool CheckforAirport(string Airport)
        {
            DataSet dsResult = null;
            bool blnResult = false;

            try
            {
                string Query = "Select top 1 1 from AirportMaster where AirportCode = '" + Airport + "'";
                dsResult = db.GetDataset(Query);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    blnResult = true;
                else
                    blnResult = false;
            }
            catch (Exception ex)
            {
                dsResult = null;
            }

            return blnResult;

        }

        public bool CheckforCurrency(string Currency)
        {
            DataSet dsResult = null;
            bool blnResult = false;

            try
            {
                string Query = "Select top 1 1 from dbo.CurrencyMaster where CurrencyCode = '" + Currency + "'";
                dsResult = db.GetDataset(Query);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    blnResult = true;
                else
                    blnResult = false;
            }
            catch (Exception ex)
            {
                dsResult = null;
            }

            return blnResult;

        }

        public bool CheckforCountryCode(string CountryCode)
        {
            DataSet dsResult = null;
            bool blnResult = false;

            try
            {
                string Query = "Select top 1 1 from tblCountryMaster where CountryCode = '" + CountryCode + "'";
                dsResult = db.GetDataset(Query);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    blnResult = true;
                else
                    blnResult = false;
            }
            catch (Exception ex)
            {
                dsResult = null;
            }

            return blnResult;

        }

        public bool CheckforCommodityCode(string CommodityCode)
        {
            DataSet dsResult = null;
            bool blnResult = false;

            try
            {
                string Query = "Select top 1 1 from CommodityMaster where IsActive = 1 and CommodityCode = '" + CommodityCode + "'";
                dsResult = db.GetDataset(Query);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    blnResult = true;
                else
                    blnResult = false;
            }
            catch (Exception ex)
            {
                dsResult = null;
            }

            return blnResult;

        }

        public bool CheckforProductCode(string ProductCode)
        {
            DataSet dsResult = null;
            bool blnResult = false;

            try
            {
                string Query = "Select top 1 1 from dbo.ProductTypeMaster where IsActive = 1 and ProductType = '" + ProductCode + "'";
                dsResult = db.GetDataset(Query);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    blnResult = true;
                else
                    blnResult = false;
            }
            catch (Exception ex)
            {
                dsResult = null;
            }

            return blnResult;

        }

        public bool CheckforFlightNumber(string FlightNumber)
        {
            DataSet dsResult = null;
            bool blnResult = false;

            try
            {
                string Query = "Select top 1 1 from dbo.AirlineScheduleRoute where FlightID = '" + FlightNumber + "'";
                dsResult = db.GetDataset(Query);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    blnResult = true;
                else
                    blnResult = false;
            }
            catch (Exception ex)
            {
                dsResult = null;
            }

            return blnResult;

        }

        public bool CheckforAgengtCode(string AgengtCode)
        {
            DataSet dsResult = null;
            bool blnResult = false;

            try
            {
                string Query = "Select top 1 1 from dbo.AgentMaster where AgentCode = '" + AgengtCode + "'";
                dsResult = db.GetDataset(Query);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    blnResult = true;
                else
                    blnResult = false;
            }
            catch (Exception ex)
            {
                dsResult = null;
            }

            return blnResult;

        }

        public bool CheckforShipperode(string ShipperCode)
        {
            DataSet dsResult = null;
            bool blnResult = false;

            try
            {
                string Query = "Select top 1 1 from dbo.AccountMaster WHERE ParticipationType IN ('Shipper','S','Both','B') AND AgentCode = '" + ShipperCode + "'";
                dsResult = db.GetDataset(Query);

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                    blnResult = true;
                else
                    blnResult = false;
            }
            catch (Exception ex)
            {
                dsResult = null;
            }

            return blnResult;

        }
    }
}
