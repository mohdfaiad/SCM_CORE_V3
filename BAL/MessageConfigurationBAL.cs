using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using QID.DataAccess;
namespace BAL
{
    public class MessageConfigurationBAL
    {

        string constr = "";
        DataSet resmessageData = new DataSet();
       
        public MessageConfigurationBAL()
        {
            constr = Global.GetConnectionString();

        }
        
        #region MessageConfiguration
        public string SaveConfiguratin(object[] Conf)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] ParamNames = new string[] { "Origin", "Destinatin", "FlightNo", "MailFFM", "MailFTX", "MailFSU" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                string res = da.GetStringByProcedure("spSaveMessageConf", ParamNames, Conf, ParamTypes);
                Conf = null;
                ParamNames = null;
                ParamTypes = null;
                return res;

            }
            catch (Exception)
            { }
            finally 
            {
                db = null;
            }
            return null;
        }
        #endregion

        # region EditMailList
        public DataSet ListConfiguration(object[] Conf)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] ParamNames = new string[] { "Origin", "Destinatin", "FlightNo" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                DataSet ds = db.SelectRecords("spGetMessageConf", ParamNames, Conf, ParamTypes);
                ParamTypes = null;
                ParamNames = null;
                Conf = null;
                return ds;

            }
            catch (Exception)
            { }
            finally 
            {
                db = null;
            }
            return null;
        }
        #endregion

        #region ListEmail
        public DataSet ListEmail(object[] Conf)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;
            try
            {
                string[] ParamNames = new string[] { "Origin", "Destinatin", "FlightNo" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                 ds = db.SelectRecords("spGetEmailList", ParamNames, Conf, ParamTypes);
                Conf = null;
                ParamNames = null;
                ParamTypes = null;
                return ds;
            }
            catch (Exception)
            { }
            finally 
            {
                db = null;
            }
            return null;
        }
        #endregion

        #region GetMessageConfig
        public DataSet getMessageData(string PartnerCode,string MessageType,string Origin,string Destination,string TransitDestination,string FlightNumber,string PartnerType)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] Pname = new string[7];
                object[] Pvalue = new object[7];
                SqlDbType[] Ptype = new SqlDbType[7];


                Pname[0] = "PartnerCode";
                Pname[1] = "MessageType";
                Pname[2] = "Origin";
                Pname[3] = "Destination";
                Pname[4] = "TransitDestination";
                Pname[5] = "FlightNumber";
                Pname[6] = "PartnerType";


                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;


                Pvalue[0] = PartnerType;
                Pvalue[1] = MessageType;
                Pvalue[2] = Origin;
                Pvalue[3] = Destination;
                Pvalue[4] = TransitDestination;
                Pvalue[5] = FlightNumber;
                Pvalue[6] = PartnerType;



                resmessageData = db.SelectRecords("GetMessageData", Pname, Pvalue, Ptype);
                Pname = null;
                Pvalue = null;
                Ptype = null;
                if (resmessageData != null)
                {
                    if (resmessageData.Tables != null)
                    {
                        if (resmessageData.Tables.Count > 0)
                        {
                            return resmessageData;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return resmessageData;
            }
            finally 
            {
                db = null;
            }
            return resmessageData;
        }

        #endregion getMessageConfig

        #region Save MessageConfiguration
        public string SaveConfiguratinMessage(string PartnerCode, string MessageType, string Origin, string Destination, string TransitDestination, string FlightNumber, string SitaID, string MessageCommType, string EmailId, string PartnerType, string createdBy, string FTPID, string FTPUserNm, string FTPPwd,bool AutoGen,string StartChar,string EndChar)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            try
            {
                string[] Pname = new string[17];
                object[] Pvalue = new object[17];
                SqlDbType[] Ptype = new SqlDbType[17];


                Pname[0] = "PartnerType";
                Pname[1] = "MessageCommType";
                Pname[2] = "Origin";
                Pname[3] = "Destination";
                Pname[4] = "TransitDestination";
                Pname[5] = "SitaID";
                Pname[6] = "EmailId";
                Pname[7] = "MessageType";
                Pname[8] = "PartnerCode";
                Pname[9] = "FlightNumber";
                Pname[10] = "createdBy";
                Pname[11] = "FTPID";
                Pname[12] = "FTPUserName";
                Pname[13] = "FTPPassword";
                Pname[14] = "AutoGen";
                Pname[15] = "StartChar";
                Pname[16] = "EndChar";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.VarChar;
                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.Bit;
                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.VarChar;


                Pvalue[0] = PartnerType;
                Pvalue[1] = MessageCommType;
                Pvalue[2] = Origin;
                Pvalue[3] = Destination;
                Pvalue[4] = TransitDestination;
                Pvalue[5] = SitaID;
                Pvalue[6] = EmailId;
                Pvalue[7] = MessageType;
                Pvalue[8] = PartnerCode;
                Pvalue[9] = FlightNumber;
                Pvalue[10] = createdBy;
                Pvalue[11] = FTPID;
                Pvalue[12] = FTPUserNm;
                Pvalue[13] = FTPPwd;
                Pvalue[14] = AutoGen;
                Pvalue[15] = StartChar;
                Pvalue[16] = EndChar;

                string res = db.GetStringByProcedure("spSaveMessageConfiguration", Pname, Pvalue, Ptype);
                Pname = null;
                Pvalue = null;
                Ptype = null;
                return res;

            }
            catch (Exception)
            {
            }
            finally 
            {
                db = null;
            }
            return null;
        }
        #endregion Save MessageConfiguration

        #region Save MessageConfiguration
        public string UpdateConfiguratinMessage(string id,string PartnerCode, string MessageType, string Origin, string Destination, string TransitDestination, string FlightNumber, string SitaID, string MessageCommType, string EmailId, string PartnerType, string createdBy, string FTPID, string FTPUserNm, string FTPPwd,bool AutoGen,string StartChar,string EndChar)
        {
             SQLServer db = new SQLServer(Global.GetConnectionString());
             try
             {
                 string[] Pname = new string[18];
                 object[] Pvalue = new object[18];
                 SqlDbType[] Ptype = new SqlDbType[18];


                 Pname[0] = "PartnerType";
                 Pname[1] = "MessageCommType";
                 Pname[2] = "Origin";
                 Pname[3] = "Destination";
                 Pname[4] = "TransitDestination";
                 Pname[5] = "SitaID";
                 Pname[6] = "EmailId";
                 Pname[7] = "MessageType";
                 Pname[8] = "PartnerCode";
                 Pname[9] = "FlightNumber";
                 Pname[10] = "createdBy";
                 Pname[11] = "FTPID";
                 Pname[12] = "FTPUserName";
                 Pname[13] = "FTPPassword";
                 Pname[14] = "id";
                 Pname[15] = "AutoGen";
                 Pname[16] = "StartChar";
                 Pname[17] = "EndChar";

                 Ptype[0] = SqlDbType.VarChar;
                 Ptype[1] = SqlDbType.VarChar;
                 Ptype[2] = SqlDbType.VarChar;
                 Ptype[3] = SqlDbType.VarChar;
                 Ptype[4] = SqlDbType.VarChar;
                 Ptype[5] = SqlDbType.VarChar;
                 Ptype[6] = SqlDbType.VarChar;
                 Ptype[7] = SqlDbType.VarChar;
                 Ptype[8] = SqlDbType.VarChar;
                 Ptype[9] = SqlDbType.VarChar;
                 Ptype[10] = SqlDbType.VarChar;
                 Ptype[11] = SqlDbType.VarChar;
                 Ptype[12] = SqlDbType.VarChar;
                 Ptype[13] = SqlDbType.VarChar;
                 Ptype[14] = SqlDbType.VarChar;
                 Ptype[15] = SqlDbType.Bit;
                 Ptype[16] = SqlDbType.VarChar;
                 Ptype[17] = SqlDbType.VarChar;

                 Pvalue[0] = PartnerType;
                 Pvalue[1] = MessageCommType;
                 Pvalue[2] = Origin;
                 Pvalue[3] = Destination;
                 Pvalue[4] = TransitDestination;
                 Pvalue[5] = SitaID;
                 Pvalue[6] = EmailId;
                 Pvalue[7] = MessageType;
                 Pvalue[8] = PartnerCode;
                 Pvalue[9] = FlightNumber;
                 Pvalue[10] = createdBy;
                 Pvalue[11] = FTPID;
                 Pvalue[12] = FTPUserNm;
                 Pvalue[13] = FTPPwd;
                 Pvalue[14] = id;
                 Pvalue[15] = AutoGen;
                 Pvalue[16] = StartChar;
                 Pvalue[17] = EndChar;


                 string res = db.GetStringByProcedure("spUpdateMessageConfiguration", Pname, Pvalue, Ptype);
                 Pname = null;
                 Pvalue = null;
                 Ptype = null;
                 return res;

             }
             catch (Exception)
             { }
             finally 
             {
                 db = null;
             }
            return null;
        }
        #endregion Update MessageConfiguration
        //by manjusha
        #region DelConfigMessage 
        public int DelConfiguratinMessage(int SrNo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //1
                ColumnNames.SetValue("srno", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(SrNo, i);
                i++;


                if (!da.ExecuteProcedure("spDeleteConfigMsg", ColumnNames, DataType, Values))
                    return (-1);
                else
                {
                    return (0);
                }
            }
            catch (Exception ex)
            {
                return (-1);
            }
        }

        #endregion 
        #region GetMsgList
        public DataSet GetMsgList(string Id)
        {
            //int rateLineId = Convert.ToInt32(rateLineId);
            SQLServer db = new SQLServer(Global.GetConnectionString());
            DataSet dsRateLine = null;

            try
            {
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];

                ColumnNames[0] = "Id";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = Id;

                dsRateLine = db.SelectRecords("SP_GetMsgList", ColumnNames, Values, DataType);
                ColumnNames = null;
                Values = null;
                DataType = null;
            }
            catch (Exception) { }
            finally { db = null; }
            return dsRateLine;
        }
        #endregion GetMsgList

    }
}
