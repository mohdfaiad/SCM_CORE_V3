using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
//28-7-2012
namespace BAL
{

    public class BLArrival
    {

        #region Variables
        string constr = "";
        #endregion Variables

        public BLArrival()
        {
           constr = Global.GetConnectionString();
        }

        #region Offload Shipment in Manifest

        public bool OffLoadShipmentinManifest(string ActFlightNo, string OffloadFltNo, string OffloadLoc, string AWBNo, int ActPcs, double ActWt, int OffloadPcs, double OffloadWt, string Offloadedby,
            string POL, string POU, string FlightVersion, string statusreassign, DateTime dtdate, DateTime ActFlightDate)
        {
            string[] Pname = null;
            object[] Pvalue = null;
            SqlDbType[] Ptype = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                
                Pname = new string[16];
                Pvalue = new object[16];
                Ptype = new SqlDbType[16];

                Pname[0] = "ActFLTno";
                Pname[1] = "OffloadFLTno";
                Pname[2] = "OffloadLoc";
                Pname[3] = "AWBno";
                Pname[4] = "PCS";
                Pname[5] = "WGT";
                Pname[6] = "OffloadPCS";
                Pname[7] = "OffloadWGT";
                Pname[8] = "Offloadeddby";
                Pname[9] = "Offloadedon";
                Pname[10] = "POL";
                Pname[11] = "POU";
                Pname[12] = "FltVersion";
                Pname[13] = "statusreassign";
                Pname[14] = "dtdate";
                Pname[15] = "ActFlightDate";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.Int;
                Ptype[5] = SqlDbType.Float;
                Ptype[6] = SqlDbType.Int;
                Ptype[7] = SqlDbType.Float;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.DateTime;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.DateTime;
                Ptype[15] = SqlDbType.DateTime;

                Pvalue[0] = ActFlightNo;
                Pvalue[1] = OffloadFltNo;
                Pvalue[2] = OffloadLoc;
                Pvalue[3] = AWBNo;
                Pvalue[4] = ActPcs;
                Pvalue[5] = ActWt;
                Pvalue[6] = OffloadPcs;
                Pvalue[7] = OffloadWt;
                Pvalue[8] = Offloadedby;
                Pvalue[9] = DateTime.Now.ToString("yyyy-MM-dd");
                Pvalue[10] = POL;
                Pvalue[11] = POU;
                Pvalue[12] = FlightVersion;
                Pvalue[13] = statusreassign;
                Pvalue[14] = dtdate;
                Pvalue[15] = ActFlightDate;

                bool res = da.InsertData("SPSaveReassign", Pname, Ptype, Pvalue);

                return res;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
        }


        #endregion Offload Shipment in Manifest

        #region CompleteAWB
        public DataSet GetIsClosedtatus(string AWBNumber,string POU)
        {
            SQLServer da = new SQLServer(constr);
            DataSet res1 = null;
            string[] Pname = new string[2];
            object[] Pvalue = new object[2];
            SqlDbType[] Ptype = new SqlDbType[2];
            try
            {
                Pname[0] = "AWBNo";
                Pname[1] = "POU";
                Pvalue[0] = AWBNumber;
                Pvalue[1] = POU;
                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                res1 = da.SelectRecords("SpGetIsClosed", Pname, Pvalue, Ptype);
                if (res1 != null)
                {
                    if (res1.Tables != null)
                    {
                        if (res1.Tables.Count > 0)
                        {
                            return res1;
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (res1 != null)
                {
                    res1.Dispose();
                }
            }
            finally
            {
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
            return null;
        
        }
        #endregion CompleteAWB

        #region DeliverdAWB
        public DataSet DeliveredAWB(string AWBNo,string FltNo,string FltDate)
        {
            SQLServer da = new SQLServer(constr);
            string[] Pname = new string[3];
            object[] Pvalue = new object[3];
            SqlDbType[] Ptype = new SqlDbType[3];
            DataSet res = null;
            try
            {

                Pname[0] = "AWBNumber";
                Pname[1] = "FltNo";
                Pname[2] = "FltDate";

                Pvalue[0] = AWBNo;
                Pvalue[1] = FltNo;
                Pvalue[2] = FltDate;

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;

                res = da.SelectRecords("SpGetDoPieces", Pname, Pvalue, Ptype);
                if (res != null)
                {
                    if (res.Tables != null)
                    {
                        if (res.Tables.Count > 0)
                        {
                            return res;
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (res != null)
                {
                    res.Dispose();
                }
            }
            finally
            {
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
            return null;
        }
        #endregion DeliverdAWB

        #region CompleteAWB
        public DataSet GetDoPieces(string AWBNumber,string FltNo,DateTime FltDate)
        {
            SQLServer da = new SQLServer(constr);
            DataSet res = null;
            string[] Pname = new string[3];
            object[] Pvalue = new object[3];
            SqlDbType[] Ptype = new SqlDbType[3];
            try
            {

                Pname[0] = "AWBNumber";
                Pname[1] = "FltNo";
                Pname[2] = "FltDate";

                Pvalue[0] = AWBNumber;
                Pvalue[1] = FltNo;
                Pvalue[2] = FltDate;

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.DateTime;

                res = da.SelectRecords("SpGetStatus", Pname, Pvalue, Ptype);
                if (res != null)
                {
                    if (res.Tables != null)
                    {
                        if (res.Tables.Count > 0)
                        {
                            return res;
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (res != null)
                {
                    res.Dispose();
                }
            }
            finally
            {
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
            return null;
        
        }
        #endregion CompleteAWB

        public DataSet GetCCAmount(string AWBNumber)
        {
            SQLServer da = new SQLServer(constr);
            DataSet res = null;
            try
            {
                res = da.SelectRecords("Sp_GetAmount", "AWBNumber", AWBNumber, SqlDbType.VarChar);
                if (res != null)
                {
                    if (res.Tables != null)
                    {
                        if (res.Tables.Count > 0)
                        {
                            return res;
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (res != null)
                {
                    res.Dispose();
                }
            }
            return null;
        }

        # region Get Arrived CC AWB Data
        public DataSet GetArrivedCCAWBData(object[] RateCardInfo)
        {
            string[] ColumnNames = new string[3];
            SqlDbType[] DataType = new SqlDbType[3];
            Object[] Values = new object[3];
            DataSet ds = null;
            try
            {
                SQLServer da = new SQLServer(constr);

                int i = 0;
                //0
                ColumnNames.SetValue("FlightNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FlightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                ds = da.SelectRecords("SP_GetArrivedCCAWBData", ColumnNames, Values, DataType);
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
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            finally
            {
                ColumnNames = null;
                Values = null;
                DataType = null;
            }
            return (null);
        }
        # endregion Get Arrived CC AWB Data

        #region Get Consignee
        public string GetConsignee(string AWBNo)
        {
            string result = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                result = da.GetStringByProcedure("spGetConsignee", "AWBNo", AWBNo, SqlDbType.VarChar);
            }
            catch (Exception)
            {
            }
            return (result);
        }
        #endregion Get Consignee

        #region Get Shipper Consignee
        public DataSet GetShipperConsignee(string AccountCode)
        {
            DataSet result = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                result = da.SelectRecords("sp_GetShipConsDetailsByAcctNum", "AccountCode", AccountCode, SqlDbType.VarChar);
            }
            catch (Exception)
            {
                result = null;
            }
            return (result);
        }
        #endregion Get Shipper Consignee

        #region Get Shipper Consignee By AWB
        public DataSet GetShipperConsigneeByAWB(string AWBNumber,DateTime timeStamp)
        {
            DataSet result = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] Params = new string[] { "AccountCode", "AWBNumber", "TimeStamp" };
                object[] Vals = new object[] { "", AWBNumber,timeStamp };
                SqlDbType[] DataTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.DateTime };
                result = da.SelectRecords("sp_GetShipConsDetailsByAcctNum",Params,Vals,DataTypes);
            }
            catch (Exception)
            {
                result = null;
            }
            return (result);
        }
        #endregion Get Shipper Consignee

        #region get scc
        public DataSet getscc(string awbno)
        {
            SQLServer da = new SQLServer(constr);
            DataSet res1 = null;
            string[] Pname = new string[1];
            object[] Pvalue = new object[1];
            SqlDbType[] Ptype = new SqlDbType[1];
            try
            {

                Pname[0] = "AWBNo";
                Pvalue[0] = awbno;
                Ptype[0] = SqlDbType.VarChar;

                res1 = da.SelectRecords("GetScc", Pname, Pvalue, Ptype);
                if (res1 != null)
                {
                    if (res1.Tables != null)
                    {
                        if (res1.Tables.Count > 0)
                        {
                            return res1;
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (res1 != null)
                {
                    res1.Dispose();
                }
            }
            finally
            {
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
            return null;
        }

        #endregion gt scc

        #region get scc GHA
        public DataSet getsccGHA(string uldno)
        {
            SQLServer da = new SQLServer(constr);
            DataSet res1 = null;
            string[] Pname = new string[1];
            object[] Pvalue = new object[1];
            SqlDbType[] Ptype = new SqlDbType[1];
            try
            {

                Pname[0] = "ULDNo";

                Pvalue[0] = uldno;

                Ptype[0] = SqlDbType.VarChar;

                res1 = da.SelectRecords("GetSccGHA", Pname, Pvalue, Ptype);
                if (res1 != null)
                {
                    if (res1.Tables != null)
                    {
                        if (res1.Tables.Count > 0)
                        {
                            return res1;
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (res1 != null)
                {
                    res1.Dispose();
                }
            }
            finally
            {
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
            return null;
        }

        #endregion gt scc

        # region InsertArrivedCCAWBData
        public string InsertArrivedCCAWBData(object[] RateCardInfo)
        {
            string[] ColumnNames = new string[5];
            SqlDbType[] DataType = new SqlDbType[5];
            Object[] Values = new object[5];
            try
            {
                SQLServer da = new SQLServer(constr);

                int i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                i++;

                ColumnNames.SetValue("StationCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                
                
                string res = da.GetStringByProcedure("SP_InsertArrivedCCAWBData", ColumnNames, Values, DataType);
                return res;

            }
            catch (Exception)
            {
                return "error";
            }
            finally
            {
                ColumnNames = null;
                Values = null;
                DataType = null;
            }
        }
        # endregion InsertArrivedCCAWBData

        # region Generate Bunch Invoice Numbers For Dest Agent AWBs
        public string GenerateBunchInvoiceNumDestAgent(object[] RateCardInfo)
        {
            //Check if UpdatedOn is received from calling function.
            int Count = RateCardInfo.Length + 2;
            string[] ColumnNames = new string[Count];
            SqlDbType[] DataType = new SqlDbType[Count];
            Object[] Values = new object[Count];
            try
            {
                SQLServer da = new SQLServer(constr);

                int i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                i++;

                ColumnNames.SetValue("Suffix", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                i++;

                if (Count > 2)
                {   //If updated on is received.
                    ColumnNames.SetValue("UpdatedOn", i);
                    DataType.SetValue(SqlDbType.DateTime, i);
                    Values.SetValue(RateCardInfo.GetValue(i - 2), i);
                }

                string res = da.GetStringByProcedure("SP_GenerateBunchInvoiceNumDestAgentNew", ColumnNames, Values, DataType);
                return res;

            }
            catch (Exception)
            {
                return "error";
            }
            finally
            {
                ColumnNames = null;
                Values = null;
                DataType = null;
            }
        }
        # endregion Generate Bunch Invoice Numbers For Dest Agent AWBs

        //2-10-2012
        /// <summary>
        /// This function is used to fetch the data from the booking when any piece is coming to the arrival
        /// </summary>
        /// <param name="AWBNumber"></param>
        /// <returns></returns>
        #region GetData 
        public DataSet Getdata(string AWBNumber)
        {
            SQLServer da = new SQLServer(constr);
            DataSet res = null;
            try
            {
                res = da.SelectRecords("Sp_GetDataArrival", "AWBNumber", AWBNumber, SqlDbType.VarChar);
                if (res.Tables != null)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        return res;
                    }
                }
            }
            catch (Exception)
            {
                if (res != null)
                {
                    res.Dispose();
                }
            }
            return null;
        
        }

        public DataSet GetdataULd(string ULDNumber)
        {
            SQLServer da = new SQLServer(constr);
            DataSet res = null;
            try
            {
                res = da.SelectRecords("Sp_GetDataULDArrival", "AWBNumber", ULDNumber, SqlDbType.VarChar);
                if (res.Tables != null)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        return res;
                    }
                }
            }
            catch (Exception)
            {
                if (res != null)
                {
                    res.Dispose();
                }
            }
            return null;
        }

        #endregion GetData

        public DataSet GetPOUAirlineScheduleforArrival(string FLTiD, string Source, DateTime dtFlightDate)
        {

            SQLServer da = new SQLServer(constr);
            DataSet Ds = null;
            string[] Pname = new string[3];
            object[] Pvalue = new object[3];
            SqlDbType[] Ptype = new SqlDbType[3];
            try
            {

                Pname[0] = "FlightID";
                Pname[1] = "Source";
                Pname[2] = "FlightDate";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.DateTime;

                Pvalue[0] = FLTiD;
                Pvalue[1] = Source;
                Pvalue[2] = dtFlightDate;

                Ds = da.SelectRecords("spExpManiGetAirlineSchforArrival", Pname, Pvalue, Ptype);

                return Ds;
            }
            catch (Exception)
            {
                if (Ds != null)
                {
                    Ds.Dispose();
                }
            }
            finally
            {
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
            return (null);
        }

        #region GetSignatuure
        public DataSet GetSignature(string AWBNumber)
        {
            SQLServer da = new SQLServer(constr);
            DataSet res = null;
            try
            {
                res = da.SelectRecords("Sp_GetDOSign", "AWBNumber", AWBNumber, SqlDbType.VarChar);
                if (res != null)
                {
                    if (res.Tables != null)
                    {
                        if (res.Tables.Count > 0)
                        {
                            return res;
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (res != null)
                {
                    res.Dispose();
                }
            }
            return null;
        }
        #endregion GetSignature

        #region GetData
        public DataSet GetdataTracking(string AWBNumber,string Station, string DONumber)
        {
            SQLServer da = new SQLServer(constr);
            DataSet res = null;
            string[] Pname = new string[3];
            object[] Pvalue = new object[3];
            SqlDbType[] Ptype = new SqlDbType[3];
            try
            {

                Pname[0] = "AWBNumber";
                Pname[1] = "StationCode";
                Pname[2] = "DONumber";

                Pvalue[0] = AWBNumber;
                Pvalue[1] = Station;
                Pvalue[2] = DONumber;

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;

                res = da.SelectRecords("Sp_GetDODetailsTracking", Pname, Pvalue, Ptype);

                if (res.Tables != null)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        return res;
                    }
                }
            }
            catch (Exception)
            {
                if (res != null)
                {
                    res.Dispose();
                }
            }
            finally
            {
                Pname = null;
                Pvalue = null;
                Ptype = null;

            }
            return null;

        }
        #endregion GetData

        public DataTable GetULDChildAWB(object[] paramvalue)
        {
            DataSet ds = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                DataTable dt;
                string[] paramname = new string[4];
                SqlDbType[] paramtype = new SqlDbType[4];

                paramname[0] = "Fltno";
                paramname[1] = "FltDate";
                paramname[2] = "StationCode";
                paramname[3] = "ULDNo";

                //4/3/2012
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;

                ds = da.SelectRecords("Sp_GetArrival_ChildAWB", paramname, paramvalue, paramtype);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0].Copy();
                    return (dt);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            return (null);
        }

    }
}
