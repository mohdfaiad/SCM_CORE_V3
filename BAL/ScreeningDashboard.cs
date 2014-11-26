using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;

namespace BAL
{
 public  class ScreeningDashboard
    {
        public DataSet fillPrefixdata()
        {
            DataSet dsPre = new DataSet();
            SQLServer db = new SQLServer(Global.GetConnectionString());
            dsPre = db.SelectRecords("spgetPrefix");
            return dsPre;
        }
        public DataSet getDataforDropDownInGrid(string Station)
        {
            DataSet dsFillGrid = new DataSet();
            SQLServer db = new SQLServer(Global.GetConnectionString());

            string[] ColumnNames = new string[1];
            SqlDbType[] DataType = new SqlDbType[1];
            Object[] Values = new object[1];

            ColumnNames[0] = "StationCode";
            DataType[0] = SqlDbType.VarChar;
            Values[0] = Station;

            dsFillGrid = db.SelectRecords("spgetSecurityType", ColumnNames, Values, DataType);
            return dsFillGrid;
        }
        public bool UpdateScreenTag(string station, string awbno, string tagID, string dest, string ccsf, string xray, string xraytime, string k9, string k9time, string etd, string etdtime, string physical, string physicaltime, string issubtag, string updatedBy, string updatedOn, string totalsca, string xraycnt, string k9cnt, string etdcnt, string phycnt, string isrfid, string rem, string addrem, string FlightNo, string FlightDate,string Location,string ScreeningException)
        {
            bool res = false;
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[28];
                SqlDbType[] DataType = new SqlDbType[28];
                Object[] Values = new object[28];

                ColumnNames[0] = "Station";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = station;

                ColumnNames[1] = "AWBNo";
                DataType[1] = SqlDbType.VarChar;
                Values[1] = awbno;

                ColumnNames[2] = "TagID";
                DataType[2] = SqlDbType.VarChar;
                Values[2] = tagID;

                ColumnNames[3] = "Dest";
                DataType[3] = SqlDbType.VarChar;
                Values[3] = dest;

                ColumnNames[4] = "CCSF";
                DataType[4] = SqlDbType.Bit;
                Values[4] = ccsf;

                ColumnNames[5] = "Xray";
                DataType[5] = SqlDbType.VarChar;
                Values[5] = xray;

                ColumnNames[6] = "XrayTime";
                DataType[6] = SqlDbType.DateTime;
                Values[6] = xraytime;

                ColumnNames[7] = "K9";
                DataType[7] = SqlDbType.VarChar;
                Values[7] = k9;

                ColumnNames[8] = "K9Time";
                DataType[8] = SqlDbType.DateTime;
                Values[8] = k9time;

                ColumnNames[9] = "ETD";
                DataType[9] = SqlDbType.VarChar;
                Values[9] = etd;

                ColumnNames[10] = "ETDTime";
                DataType[10] = SqlDbType.DateTime;
                Values[10] = etdtime;

                ColumnNames[11] = "Physical";
                DataType[11] = SqlDbType.VarChar;
                Values[11] = physical;

                ColumnNames[12] = "PhysicalTime";
                DataType[12] = SqlDbType.DateTime;
                Values[12] = physicaltime;

                ColumnNames[13] = "IsSubTag";
                DataType[13] = SqlDbType.Bit;
                Values[13] = issubtag;

                ColumnNames[14] = "UpdatedBy";
                DataType[14] = SqlDbType.VarChar;
                Values[14] = updatedBy;

                ColumnNames[15] = "UpdatedOn";
                DataType[15] = SqlDbType.DateTime;
                Values[15] = updatedOn;

                ColumnNames[16] = "totalscan";
                DataType[16] = SqlDbType.Int;
                Values[16] = totalsca;

                ColumnNames[17] = "xraycnt";
                DataType[17] = SqlDbType.Int;
                Values[17] = xraycnt;

                ColumnNames[18] = "k9cnt";
                DataType[18] = SqlDbType.Int;
                Values[18] = k9cnt;

                ColumnNames[19] = "etdcnt";
                DataType[19] = SqlDbType.Int;
                Values[19] = etdcnt;

                ColumnNames[20] = "phycnt";
                DataType[20] = SqlDbType.Int;
                Values[20] = phycnt;

                ColumnNames[21] = "isRfid";
                DataType[21] = SqlDbType.Bit;
                Values[21] = isrfid;

                ColumnNames[22] = "remark";
                DataType[22] = SqlDbType.VarChar;
                Values[22] = rem;

                ColumnNames[23] = "addremark";
                DataType[23] = SqlDbType.VarChar;
                Values[23] = addrem;

                ColumnNames[24] = "FlightNo";
                DataType[24] = SqlDbType.VarChar;
                Values[24] = FlightNo;

                ColumnNames[25] = "FlightDate";
                DataType[25] = SqlDbType.VarChar;
                Values[25] = FlightDate;


                ColumnNames[26] = "Location";
                DataType[26] = SqlDbType.VarChar;
                Values[26] = Location;

                ColumnNames[27] = "ScrException";
                DataType[27] = SqlDbType.VarChar;
                Values[27] = ScreeningException;

                res = db.InsertData("spUpdateScreenTagforRandNR", ColumnNames, DataType, Values);

            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool SaveTag(string station, string awbno, string tagID, string dest, string ccsf, string xray, string xraytime,string k9,string k9time,string etd,string etdtime,string physical,string physicaltime,string issubtag,string updatedBy,string updatedOn,string totalsca,string xraycnt,string k9cnt,string etdcnt,string phycnt,string isrfid,string remark,string addremark,string FlightNo,string FlightDate,string Location,string screxception)
        {
            bool res = false;
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[28];
                SqlDbType[] DataType = new SqlDbType[28];
                Object[] Values = new object[28];

                ColumnNames[0] = "Station";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = station;

                ColumnNames[1] = "AWBNo";
                DataType[1] = SqlDbType.VarChar;
                Values[1] = awbno;

                ColumnNames[2] = "TagID";
                DataType[2] = SqlDbType.VarChar;
                Values[2] = tagID;

                ColumnNames[3] = "Dest";
                DataType[3] = SqlDbType.VarChar;
                Values[3] = dest;

                ColumnNames[4] = "CCSF";
                DataType[4] = SqlDbType.Bit;
                Values[4] = ccsf;

                ColumnNames[5] = "Xray";
                DataType[5] = SqlDbType.VarChar;
                Values[5] = xray;

                ColumnNames[6] = "XrayTime";
                DataType[6] = SqlDbType.DateTime;
                Values[6] = xraytime;

                ColumnNames[7] = "K9";
                DataType[7] = SqlDbType.VarChar;
                Values[7] = k9;

                ColumnNames[8] = "K9Time";
                DataType[8] = SqlDbType.DateTime;
                Values[8] = k9time;

                ColumnNames[9] = "ETD";
                DataType[9] = SqlDbType.VarChar;
                Values[9] = etd;

                ColumnNames[10] = "ETDTime";
                DataType[10] = SqlDbType.DateTime;
                Values[10] = etdtime;

                ColumnNames[11] = "Physical";
                DataType[11] = SqlDbType.VarChar;
                Values[11] = physical;

                ColumnNames[12] = "PhysicalTime";
                DataType[12] = SqlDbType.DateTime;
                Values[12] = physicaltime;

                ColumnNames[13] = "IsSubTag";
                DataType[13] = SqlDbType.Bit;
                Values[13] = issubtag;

                ColumnNames[14] = "UpdatedBy";
                DataType[14] = SqlDbType.VarChar;
                Values[14] = updatedBy;

                ColumnNames[15] = "UpdatedOn";
                DataType[15] = SqlDbType.DateTime;
                Values[15] = updatedOn;

                ColumnNames[16] = "totalscan";
                DataType[16] = SqlDbType.Int;
                Values[16] = totalsca;

                ColumnNames[17] = "xraycnt";
                DataType[17] = SqlDbType.Int;
                Values[17] = xraycnt;

                ColumnNames[18] = "k9cnt";
                DataType[18] = SqlDbType.Int;
                Values[18] = k9cnt;

                ColumnNames[19] = "etdcnt";
                DataType[19] = SqlDbType.Int;
                Values[19] = etdcnt;

                ColumnNames[20] = "phycnt";
                DataType[20] = SqlDbType.Int;
                Values[20] = phycnt;

                ColumnNames[21] = "isRfid";
                DataType[21] = SqlDbType.Bit;
                Values[21] = isrfid;

                ColumnNames[22] = "remark";
                DataType[22] = SqlDbType.VarChar;
                Values[22] = remark;

                ColumnNames[23] = "addremark";
                DataType[23] = SqlDbType.VarChar;
                Values[23] = addremark;

                ColumnNames[24] = "FlightNo";
                DataType[24] = SqlDbType.VarChar;
                Values[24] = FlightNo;

                ColumnNames[25] = "FlightDate";
                DataType[25] = SqlDbType.VarChar;
                Values[25] = FlightDate;

                ColumnNames[26] = "Location";
                DataType[26] = SqlDbType.VarChar;
                Values[26] = Location;

                //ColumnNames[16] = "Location";
                //DataType[16] = SqlDbType.VarChar;
                //Values[16] = location;

                ColumnNames[27] = "ScrException";
                DataType[27] = SqlDbType.VarChar;
                Values[27] = screxception;

                res = db.InsertData("spSaveUnScreenTagforRandNRForReport", ColumnNames, DataType, Values);

            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }

        public bool Reject(string station, string awbno, string tagID, string dest, string ccsf, string xray, string xraytime, string k9, string k9time, string etd, string etdtime, string physical, string physicaltime, string issubtag, string updatedBy, string updatedOn, string totalsca, string xraycnt, string k9cnt, string etdcnt, string phycnt, string isrfid,string rejectReason,string FlightNo,string FlightDate,string Location)
        {
            bool res = false;
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[26];
                SqlDbType[] DataType = new SqlDbType[26];
                Object[] Values = new object[26];

                ColumnNames[0] = "Station";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = station;

                ColumnNames[1] = "AWBNo";
                DataType[1] = SqlDbType.VarChar;
                Values[1] = awbno;

                ColumnNames[2] = "TagID";
                DataType[2] = SqlDbType.VarChar;
                Values[2] = tagID;

                ColumnNames[3] = "Dest";
                DataType[3] = SqlDbType.VarChar;
                Values[3] = dest;

                ColumnNames[4] = "CCSF";
                DataType[4] = SqlDbType.Bit;
                Values[4] = ccsf;

                ColumnNames[5] = "Xray";
                DataType[5] = SqlDbType.VarChar;
                Values[5] = xray;

                ColumnNames[6] = "XrayTime";
                DataType[6] = SqlDbType.DateTime;
                Values[6] = xraytime;

                ColumnNames[7] = "K9";
                DataType[7] = SqlDbType.VarChar;
                Values[7] = k9;

                ColumnNames[8] = "K9Time";
                DataType[8] = SqlDbType.DateTime;
                Values[8] = k9time;

                ColumnNames[9] = "ETD";
                DataType[9] = SqlDbType.VarChar;
                Values[9] = etd;

                ColumnNames[10] = "ETDTime";
                DataType[10] = SqlDbType.DateTime;
                Values[10] = etdtime;

                ColumnNames[11] = "Physical";
                DataType[11] = SqlDbType.VarChar;
                Values[11] = physical;

                ColumnNames[12] = "PhysicalTime";
                DataType[12] = SqlDbType.DateTime;
                Values[12] = physicaltime;

                ColumnNames[13] = "IsSubTag";
                DataType[13] = SqlDbType.Bit;
                Values[13] = issubtag;

                ColumnNames[14] = "UpdatedBy";
                DataType[14] = SqlDbType.VarChar;
                Values[14] = updatedBy;

                ColumnNames[15] = "UpdatedOn";
                DataType[15] = SqlDbType.DateTime;
                Values[15] = updatedOn;

                ColumnNames[16] = "totalscan";
                DataType[16] = SqlDbType.Int;
                Values[16] = totalsca;

                ColumnNames[17] = "xraycnt";
                DataType[17] = SqlDbType.Int;
                Values[17] = xraycnt;

                ColumnNames[18] = "k9cnt";
                DataType[18] = SqlDbType.Int;
                Values[18] = k9cnt;

                ColumnNames[19] = "etdcnt";
                DataType[19] = SqlDbType.Int;
                Values[19] = etdcnt;

                ColumnNames[20] = "phycnt";
                DataType[20] = SqlDbType.Int;
                Values[20] = phycnt;

                ColumnNames[21] = "isRfid";
                DataType[21] = SqlDbType.Bit;
                Values[21] = isrfid;

                ColumnNames[22] = "RejectReason";
                DataType[22] = SqlDbType.VarChar;
                Values[22] = rejectReason;

                ColumnNames[23] = "FlightNo";
                DataType[23] = SqlDbType.VarChar;
                Values[23] = FlightNo;

                ColumnNames[24] = "FlightDate";
                DataType[24] = SqlDbType.VarChar;
                Values[24] = FlightDate;

                ColumnNames[25] = "Location";
                DataType[25] = SqlDbType.VarChar;
                Values[25] = Location;

                res = db.InsertData("spRejectUnScreenTagforRandNR", ColumnNames, DataType, Values);

            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }

        public bool MarkUnscreen(string awbno, string tagID, string dest,  string isCCSF, string isSubTag,string location,string isrfid,int retpcs,int id,string FlightNo,string FlightDate)
        {
            bool res = false;
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[11];
                SqlDbType[] DataType = new SqlDbType[11];
                Object[] Values = new object[11];

                ColumnNames[0] = "AWBNo";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = awbno;

                ColumnNames[1] = "tagID";
                DataType[1] = SqlDbType.VarChar;
                Values[1] = tagID;

                ColumnNames[2] = "Dest";
                DataType[2] = SqlDbType.VarChar;
                Values[2] = dest;

                ColumnNames[3] = "isCCSF";
                DataType[3] = SqlDbType.Bit;
                Values[3] = isCCSF;

                ColumnNames[4] = "IsSubTag";
                DataType[4] = SqlDbType.Bit;
                Values[4] = isSubTag;

                ColumnNames[5] = "Location";
                DataType[5] = SqlDbType.VarChar;
                Values[5] = location;

                ColumnNames[6] = "isRFID";
                DataType[6] = SqlDbType.Bit;
                Values[6] = isrfid;

                //new parameter
                ColumnNames[7] = "retpcs";
                DataType[7] = SqlDbType.Int;
                Values[7] = retpcs;

                ColumnNames[8] = "scrid";
                DataType[8] = SqlDbType.Int;
                Values[8] = id;

                ColumnNames[9] = "FlightNo";
                DataType[9] = SqlDbType.VarChar;
                Values[9] = FlightNo;

                ColumnNames[10] = "FlightDate";
                DataType[10] = SqlDbType.VarChar;
                Values[10] = FlightDate;

                res = db.InsertData("spUnMarkOrDeleteScreenTagforRandNRForReport", ColumnNames, DataType, Values);

            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool RejectTag(string station,string awbno,string tagID,string rejectedBy,string rejectedOn,string isCCSF,string isSubTag)
        {
            bool res = false;
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[7];
                SqlDbType[] DataType = new SqlDbType[7];
                Object[] Values = new object[7];

                ColumnNames[0] = "Station";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = station;

                ColumnNames[1] = "AWBNo";
                DataType[1] = SqlDbType.VarChar;
                Values[1] = awbno;

                ColumnNames[2] = "TagID";
                DataType[2] = SqlDbType.VarChar;
                Values[2] = tagID;

                ColumnNames[3] = "isCCSF";
                DataType[3] = SqlDbType.Bit;
                Values[3] = isCCSF;

                ColumnNames[4] = "RejectedBy";
                DataType[4] = SqlDbType.VarChar;
                Values[4] = rejectedBy;

                ColumnNames[5] = "RejectedOn";
                DataType[5] = SqlDbType.DateTime;
                Values[5] = rejectedOn;

                ColumnNames[6] = "IsSubTag";
                DataType[6] = SqlDbType.Bit;
                Values[6] = isSubTag;

              res =db.InsertData("spInsertRejectUnScreen", ColumnNames, DataType, Values);

            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool IgnoreAWB(string station, string awbno, string updatedOn,string isRFID)
        {
            bool res = false;
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];

                ColumnNames[0] = "Station";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = station;

                ColumnNames[1] = "AWBNo";
                DataType[1] = SqlDbType.VarChar;
                Values[1] = awbno;

                ColumnNames[2] = "UpdatedOn";
                DataType[2] = SqlDbType.DateTime;
                Values[2] = updatedOn;

                ColumnNames[3] = "isRFID";
                DataType[3] = SqlDbType.Bit;
                Values[3] = isRFID;

                res = db.InsertData("spUpdateIgnoreforRandNR", ColumnNames, DataType, Values);

            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public bool MarkClosed(string awbno, string tagID, string dest, string isCCSF, string isSubTag, string location,string isRFID)
        {
            bool res = false;
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[7];
                SqlDbType[] DataType = new SqlDbType[7];
                Object[] Values = new object[7];

                ColumnNames[0] = "AWBNo";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = awbno;

                ColumnNames[1] = "tagID";
                DataType[1] = SqlDbType.VarChar;
                Values[1] = tagID;

                ColumnNames[2] = "Dest";
                DataType[2] = SqlDbType.VarChar;
                Values[2] = dest;

                ColumnNames[3] = "isCCSF";
                DataType[3] = SqlDbType.Bit;
                Values[3] = isCCSF;

                ColumnNames[4] = "IsSubTag";
                DataType[4] = SqlDbType.Bit;
                Values[4] = isSubTag;

                ColumnNames[5] = "Location";
                DataType[5] = SqlDbType.VarChar;
                Values[5] = location;

                ColumnNames[6] = "isRFID";
                DataType[6] = SqlDbType.Bit;
                Values[6] = isRFID;

                res = db.InsertData("spClosedScreenTagforRandNR", ColumnNames, DataType, Values);

            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        public DataSet getScreenAndUnScreen(string flightNo,string flightDate,string AWBNo,string Dest,string Duration,string Station,string isRfid,string xrayType,string Status)
        {
            DataSet getScAndUnSc = new DataSet();
            SQLServer db = new SQLServer(Global.GetConnectionString());
            string[] ColumnNames = new string[9];
            SqlDbType[] DataType = new SqlDbType[9];
            Object[] Values = new object[9];

            ColumnNames[0] = "FlightNo";
            DataType[0] = SqlDbType.VarChar;
            Values[0] = flightNo;

            ColumnNames[1] = "FlightDate";
            DataType[1] = SqlDbType.VarChar;
            Values[1] = flightDate;

            ColumnNames[2] = "AWBNo";
            DataType[2] = SqlDbType.VarChar;
            Values[2] = AWBNo;

            ColumnNames[3] = "Dest";
            DataType[3] = SqlDbType.VarChar;
            Values[3] = Dest;

            ColumnNames[4] = "Duration";
            DataType[4] = SqlDbType.VarChar;
            Values[4] = Duration;

            ColumnNames[5] = "Station";
            DataType[5] = SqlDbType.VarChar;
            Values[5] = Station;

            ColumnNames[6] = "isRFID";
            DataType[6] = SqlDbType.Bit;
            Values[6] = isRfid;

            ColumnNames[7] = "XrayType";
            DataType[7] = SqlDbType.NVarChar;
            Values[7] = xrayType;

            ColumnNames[8] = "Status";
            DataType[8] = SqlDbType.NVarChar;
            Values[8] = Status;
            //getScAndUnSc = db.SelectRecords("spgetScreenAndUnScreenforRandNR", ColumnNames, Values, DataType);
            //getScAndUnSc = db.SelectRecords("spgetScreenAndUnScreenforRandNRforReport", ColumnNames, Values, DataType);   
            getScAndUnSc = db.SelectRecords("spgetScreenAndUnScreenforRandNRforReport_New", ColumnNames, Values, DataType);   
            return getScAndUnSc;
        }

        #region Update Lbl
        public bool UpdateLbl(string id,string awbno,string station,int totscan,string FlightNo,string FlightDate)
        {
            bool res = false;
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[6];
                SqlDbType[] DataType = new SqlDbType[6];
                Object[] Values = new object[6];

                ColumnNames[0] = "id";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = id;

                ColumnNames[1] = "awbno";
                DataType[1] = SqlDbType.VarChar;
                Values[1] = awbno;

                ColumnNames[2] = "station";
                DataType[2] = SqlDbType.VarChar;
                Values[2] = station;

                ColumnNames[3] = "lblcount";
                DataType[3] = SqlDbType.Int;
                Values[3] = totscan;

                ColumnNames[4] = "FlightNo";
                DataType[4] = SqlDbType.VarChar;
                Values[4] = FlightNo;

                ColumnNames[5] = "FlightDate";
                DataType[5] = SqlDbType.VarChar;
                Values[5] = FlightDate;


                res = db.InsertData("spUpdateLblCount", ColumnNames, DataType, Values);

            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }
        #endregion Update Lbl

       #region get and insert reject labels

        public DataSet getRejectLbl(string station, int lblfrm,int lblto)
        {
            DataSet getReject = new DataSet();
            SQLServer db = new SQLServer(Global.GetConnectionString());
            string[] ColumnNames = new string[3];
            SqlDbType[] DataType = new SqlDbType[3];
            Object[] Values = new object[3];

            ColumnNames[0] = "stn";
            DataType[0] = SqlDbType.VarChar;
            Values[0] = station;

            ColumnNames[1] = "lblfrm";
            DataType[1] = SqlDbType.VarChar;
            Values[1] = lblfrm;

            ColumnNames[2] = "lblto";
            DataType[2] = SqlDbType.VarChar;
            Values[2] = lblto;

            getReject = db.SelectRecords("spGetRejectLbl", ColumnNames, Values, DataType);
            return getReject;
        }

        public DataSet getRejectPcId(string station,string awbno)
        {
            DataSet getReject = new DataSet();
            SQLServer db = new SQLServer(Global.GetConnectionString());
            string[] ColumnNames = new string[2];
            SqlDbType[] DataType = new SqlDbType[2];
            Object[] Values = new object[2];

            ColumnNames[0] = "stn";
            DataType[0] = SqlDbType.VarChar;
            Values[0] = station;

            ColumnNames[1] = "awbno";
            DataType[1] = SqlDbType.VarChar;
            Values[1] = awbno;

            getReject = db.SelectRecords("spGetRejectPcsID", ColumnNames, Values, DataType);
            return getReject;
        }

     //insert rejected pieece id
        public bool InsertRejectLbl(string station,string awbnum,int lblno,string reason)
        {
            bool res = false;
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];

                ColumnNames[0] = "stn";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = station;

                ColumnNames[1] = "awbnum";
                DataType[1] = SqlDbType.VarChar;
                Values[1] = awbnum;

                ColumnNames[2] = "lblno";
                DataType[2] = SqlDbType.Int;
                Values[2] = lblno;

                ColumnNames[3] = "reason";
                DataType[3] = SqlDbType.VarChar;
                Values[3] = reason;


                res = db.InsertData("spInsertRejectLbl", ColumnNames, DataType, Values);

            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }

       #endregion

        # region Get Piece Id
        public DataSet GetPcsId(string awbnum)
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //1
                ColumnNames.SetValue("awbno", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(awbnum, i);
                i++;

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetPcsId", ColumnNames, Values, DataType);
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
            return (null);
        }
        # endregion Get Piece Id

        #region Labels
        public DataSet GetLbls(string awbnum,int id,string FlightNo,string FlightDate)
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                //1
                ColumnNames.SetValue("awbno", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(awbnum, i);
                i++;

                //1
                ColumnNames.SetValue("id", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(id, i);
                i++;

                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightNo, i);
                i++;

                ColumnNames.SetValue("FlightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(FlightDate, i);
                i++;

                DataSet ds = new DataSet();
                ds = da.SelectRecords("spGetlbls", ColumnNames, Values, DataType);
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
            return (null);
        }
        #endregion Labels

        //old unscreen
        public bool MarkUnscreenOld(string awbno, string tagID, string dest, string isCCSF, string isSubTag, string location, string isrfid,string FlightNo,string FlightDate)
        {
            bool res = false;
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[9];
                SqlDbType[] DataType = new SqlDbType[9];
                Object[] Values = new object[9];

                ColumnNames[0] = "AWBNo";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = awbno;

                ColumnNames[1] = "tagID";
                DataType[1] = SqlDbType.VarChar;
                Values[1] = tagID;

                ColumnNames[2] = "Dest";
                DataType[2] = SqlDbType.VarChar;
                Values[2] = dest;

                ColumnNames[3] = "isCCSF";
                DataType[3] = SqlDbType.Bit;
                Values[3] = isCCSF;

                ColumnNames[4] = "IsSubTag";
                DataType[4] = SqlDbType.Bit;
                Values[4] = isSubTag;

                ColumnNames[5] = "Location";
                DataType[5] = SqlDbType.VarChar;
                Values[5] = location;

                ColumnNames[6] = "isRFID";
                DataType[6] = SqlDbType.Bit;
                Values[6] = isrfid;

                ColumnNames[7] = "FlightNo";
                DataType[7] = SqlDbType.VarChar;
                Values[7] = FlightNo;

                ColumnNames[8] = "FlightDate";
                DataType[8] = SqlDbType.VarChar;
                Values[8] = FlightDate;

                //new parameter
                //ColumnNames[7] = "retpcs";
                //DataType[7] = SqlDbType.Int;
                //Values[7] = retpcs;


                res = db.InsertData("spUnMarkOrDeleteScreenTagforRandNR", ColumnNames, DataType, Values);

            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }

        //# region Get Lbl Details
        //public DataSet GetLblDetails(object[] PrefixInfo)
        //{
        //    try
        //    {
        //        SQLServer da = new SQLServer(Global.GetConnectionString());
        //        string[] ColumnNames = new string[1];
        //        SqlDbType[] DataType = new SqlDbType[1];
        //        Object[] Values = new object[1];
        //        int i = 0;

        //        //1
        //        ColumnNames.SetValue("stncode", i);
        //        DataType.SetValue(SqlDbType.VarChar, i);
        //        Values.SetValue(PrefixInfo.GetValue(i), i);
        //        i++;

        //        DataSet ds = new DataSet();
        //        ds = da.SelectRecords("spGetXrayLblDetails", ColumnNames, Values, DataType);
        //        if (ds != null)
        //        {
        //            if (ds.Tables != null)
        //            {
        //                if (ds.Tables.Count > 0)
        //                {
        //                    return (ds);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return (null);
        //}

        //# endregion Get Lbl Details

        public bool SaveReturnToShipperDetails(string AWBPrefix, string AWBNumber, int Pieces, decimal Weight, string Station, bool IsScreened, 
            string UserName, DateTime dtTimeStamp,string FlightNo,string FlightDate)
        {
            bool res = false;
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] ColumnNames = new string[10];
                SqlDbType[] DataType = new SqlDbType[10];
                Object[] Values = new object[10];

                ColumnNames[0] = "AWBPrefix";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = AWBPrefix;

                ColumnNames[1] = "AWBNumber";
                DataType[1] = SqlDbType.VarChar;
                Values[1] = AWBNumber;

                ColumnNames[2] = "Pieces";
                DataType[2] = SqlDbType.Int;
                Values[2] = Pieces;

                ColumnNames[3] = "Weight";
                DataType[3] = SqlDbType.Decimal;
                Values[3] = Weight;

                ColumnNames[4] = "Station";
                DataType[4] = SqlDbType.VarChar;
                Values[4] = Station;

                ColumnNames[5] = "IsScreened";
                DataType[5] = SqlDbType.Bit;
                Values[5] = IsScreened;

                ColumnNames[6] = "UserName";
                DataType[6] = SqlDbType.VarChar;
                Values[6] = UserName;
                
                ColumnNames[7] = "TimeStamp";
                DataType[7] = SqlDbType.DateTime;
                Values[7] = dtTimeStamp;

                ColumnNames[8] = "FlightNo";
                DataType[8] = SqlDbType.VarChar;
                Values[8] = FlightNo;

                ColumnNames[9] = "FlightDate";
                DataType[9] = SqlDbType.VarChar;
                Values[9] = FlightDate;
                
                res = db.InsertData("Sp_SaveReturntoShipper", ColumnNames, DataType, Values);

            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }

        #region SendFSU

        public bool SendFSUScreening(string AWBNumber)
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                if (da.UpdateData("sp_SendFSUScreening", "AWBNumber", SqlDbType.VarChar, AWBNumber))
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            { return false; }
        }
        #endregion


    }
}
