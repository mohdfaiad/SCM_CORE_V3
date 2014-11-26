using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class LoginBL
    {

        #region Variables
        string constr = "";
        DataSet res1;
        DataSet ds;
        #endregion Variables

        #region Constructor
        public LoginBL()
        {
           constr = Global.GetConnectionString();
        }
        #endregion Constructor

        #region Get User Details
        /// <summary>
        /// Get information of User based on passed values.
        /// </summary>
        /// <returns>Information as list.</returns>
        public DataSet GetUserDetails(object[] LoginInfo)
        {
            try
            {
                //Prepare column names and datatypes for search parameters...
                string[] paramNames = new string[3];
                SqlDbType[] dataTypes = new SqlDbType[3];
                int i = 0;

                //0
                paramNames.SetValue("LoginName", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //1
                paramNames.SetValue("Password", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //2
                paramNames.SetValue("Station", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                


                //Get user details...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetUserDetailsTest", paramNames, LoginInfo, dataTypes);
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
        #endregion Get User Details

        #region Get TimeZone
        /// <summary>
        /// Get information of User based on passed values.
        /// </summary>
        /// <returns>Information as list.</returns>
        public string GetTimeZone(string TImeZoneInfo)
        {
            try
            {
                
                SQLServer da = new SQLServer(constr);
                
                string TZ = da.GetStringByProcedure("spGetTimeZone","Station",TImeZoneInfo,SqlDbType.VarChar);
                return TZ;
            }
            catch (Exception)
            {
            }
            return (null);
        }
        #endregion GetTimeZone

        #region GetAuthenticationCode
        public string GetAuthenticationCode(string LoginName, string Station)
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                string[] colNames = new string[2];
                string[] values = new string[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;
                colNames.SetValue("AgentName", i);
                values.SetValue(LoginName, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("station", i);
                values.SetValue(Station, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                DataSet ds = da.SelectRecords("SpGenerateRandamNo",colNames,values,dataTypes);
                string AuthenticationCode = "";
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                       
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            AuthenticationCode = AuthenticationCode + dr["AuthentcationCode"].ToString();
                        }
                    }
                }
                
                return AuthenticationCode;
            }
            catch (Exception ex)
            {
                return ("");
            }
 
         
        }

        #endregion GetAuthenticationCode
        #region GetMobileNo
        public string GetMobileNo(string LoginName,string station)
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                string[] colNames = new string[2];
                string[] values = new string[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;
                colNames.SetValue("LoginName", i);
                values.SetValue(LoginName, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("station", i);
                values.SetValue(station, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                DataSet ds = da.SelectRecords("SpGetMobileNo", colNames, values, dataTypes);
                string MobileNo = "";
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            MobileNo = MobileNo + dr["MobileNumber"].ToString();
                        }
                    }
                }

                return MobileNo;
            }
            catch (Exception ex)
            {
                return ("");
            }
        
        
        }
        #endregion GetMobileNo
        /// <summary>
        ///Validate the authntication code 
        /// </summary>
        /// <param name="authenticationcode"></param>
        /// <param name="LoginName"></param>
        /// <param name="Station"></param>
        /// <returns></returns>
        #region Validte
        public bool Validate(string authenticationcode,string LoginName,string Station)
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                string[] colNames = new string[3];
                string[] values = new string[3];
                SqlDbType[] dataTypes = new SqlDbType[3];
                int i = 0;
                colNames.SetValue("Autentication", i);
                values.SetValue(authenticationcode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("LoginName", i);
                values.SetValue(LoginName, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("Station", i);
                values.SetValue(Station, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);


                DataSet ds = da.SelectRecords("SpVerifyRandom",colNames,values,dataTypes);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
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
        
        }
        #endregion Valdate

        #region GetEmail
        public DataSet GetEmail(string LoginName,string Station)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                //GetEmailUser;
                string[] colNames = new string[2];
                string[] values = new string[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;
                colNames.SetValue("LoginName", i);
                values.SetValue(LoginName, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("station", i);
                values.SetValue(Station, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);

                DataSet ds = da.SelectRecords("GetEmailUser", colNames, values, dataTypes);
                
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
                return ds; ;
            }
            return ds;

        }
        #endregion GetEmail

        #region SaveUserLoginDetails

        public string SaveUserLoginDetails(string UserName,string Station,DateTime dtCurrentDate,string IpAddress,bool isLogin)
        {
            try
            {
               // DateTime dtCurrentDate = (DateTime)Session["IT"];
               // Session["Station"] = txtStation.Text.ToUpper().Trim();



                object[] UsDetails = new object[5];
                UsDetails.SetValue(UserName, 0);
                UsDetails.SetValue(Station, 1);
                UsDetails.SetValue(dtCurrentDate, 2);
                UsDetails.SetValue(IpAddress, 3);
                UsDetails.SetValue(isLogin, 4);


               // string res=SaveUserLoginDetails(UsDetails);



                string[] ParamNames = new string[] { "UserId", "Location", "Time", "IPaddress", "isLogin" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.Bit };
                SQLServer da = new SQLServer(constr);
                string res = da.GetStringByProcedure("spSaveUserActivityLog", ParamNames, UsDetails, ParamTypes);

                return res;

            }
            catch (Exception)
            {
                return "Error";
            }

        }
        #endregion SaveAircraftEquipment

        public DataSet LoadSystemMasterData()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.GetDataset("sp_GetSystemMasterData");
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

        public DataSet LoadSystemMasterDataNew(string Parameter)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.GetDataset("sp_GetSystemMasterData_New '" + Parameter + "'");
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

        public string GetMasterConfiguration(string Parameter)
        {
            string ParameterValue = string.Empty;
            SQLServer da = new SQLServer(constr);
            string[] QName = new string[] { "PType" };
            object[] QValues = new object[] { Parameter };
            SqlDbType[] QType = new SqlDbType[] { SqlDbType.VarChar };
            ParameterValue = da.GetStringByProcedure("spGetSystemParameter", QName, QValues, QType);
            if (ParameterValue == null)
                ParameterValue = "";
            da = null;
            QName = null;
            QValues = null;
            QType = null;

            return ParameterValue;
        }

        #region Update Last Access Station & Session
        public bool UpdateAccessStationSession(string LoginName, string Station, string SessionID)
        {
            bool res = false;
            try
            {
                string ParameterValue = string.Empty;
                SQLServer da = new SQLServer(constr);
                string[] QName = new string[] { "LoginName","Station","SessionID" };
                object[] QValues = new object[] { LoginName, Station, SessionID };
                SqlDbType[] QType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                res = da.UpdateData("spUpdateUserSessionStation", QName, QType, QValues);

                da = null;
                QName = null;
                QValues = null;
                QType = null;

            }
            catch (Exception)
            {
                res = false;
            }
            return (res);
        }
        #endregion Update Last Access Station & Session

        #region Get User Details By Session ID
        /// <summary>
        /// Get information of User based on passed values.
        /// </summary>
        /// <returns>Information as list.</returns>
        public DataSet GetUserDetailsBySessionID(object[] LoginInfo)
        {
            try
            {
                //Prepare column names and datatypes for search parameters...
                string[] paramNames = new string[2];
                SqlDbType[] dataTypes = new SqlDbType[2];
                int i = 0;

                //0
                paramNames.SetValue("SessionID", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                //1
                paramNames.SetValue("SessionTimeMins", i);
                dataTypes.SetValue(SqlDbType.Int, i);
                i++;

                //Get user details...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetUserDetailsBySessionID", paramNames, LoginInfo, dataTypes);
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
        #endregion Get User Details By Session ID

        public string GetPartnerAcceptMoreOrLess(string Parameter)
        {
            string ParameterValue = string.Empty;
            SQLServer da = new SQLServer(constr);
            string[] QName = new string[] { "PType" };
            object[] QValues = new object[] { Parameter };
            SqlDbType[] QType = new SqlDbType[] { SqlDbType.VarChar };
            ParameterValue = da.GetStringByProcedure("spGetPartnerAcceptMoreOrLess", QName, QValues, QType);
            if (ParameterValue == null)
                ParameterValue = "";
            da = null;
            QName = null;
            QValues = null;
            QType = null;

            return ParameterValue;
        }
    }
}
