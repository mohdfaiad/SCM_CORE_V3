using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using QID.DataAccess;

namespace BAL
{
    public class UserCreationBAL
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        #region Get All Roles
        public DataSet GetAllRoles()
        {
            DataSet ds = null;
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("SP_GetAllRoles");
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
            return (null);
        }
        #endregion Get All Roles

        #region Get All Stations
        //old
        public DataSet GetAllStations()
        {
            DataSet ds = null;
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("SP_GetAllStationCodeName");
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
            return (null);
        }
       
        #region Get All Stations
        public DataSet GetAllStations(string level)
        {
            DataSet ds = null;
            string[] QueryPname = new string[1];
            object[] QueryValue = new object[1];
            SqlDbType[] QueryType = new SqlDbType[1];

            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(constr);

                QueryPname[0] = "level";
                QueryType[0] = SqlDbType.VarChar;
                QueryValue[0] = level;

                ds = da.SelectRecords("SP_GetAllStationCodeName", QueryPname, QueryValue, QueryType);
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
                QueryPname = null;
                QueryValue = null;
                QueryType = null;
            }
            return (null);
        }
        #endregion Get All Stations

        #endregion Get All Stations

        # region AddModifyUserDetails
        public string AddModifyUserDetails(object[] UserInfo)
        {
            string[] ColumnNames = new string[15];
            SqlDbType[] DataType = new SqlDbType[15];
            Object[] Values = new object[15];
            try
            {

                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("LoginID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("Password", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("EmailID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("RoleID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("Stations", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("CreatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("Flag", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;

                //10
                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                i++;

                //11
                ColumnNames.SetValue("MobileNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;

                //12
                ColumnNames.SetValue("Expiry", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;

                //13
                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(UserInfo.GetValue(i), i);
                i++;


                string res = db.GetStringByProcedure("SP_AddModifyUserDetails", ColumnNames, Values, DataType);
                return res;
            }
            catch (Exception ex)
            {
                return "error";
            }
            finally
            {
                ColumnNames = null;
                Values = null;
                DataType = null;
                UserInfo = null;
            }
        }
        # endregion AddModifyUserDetails

        #region Get User List Data
        public DataSet GetUserListData(string UserID, int RoleID, string StationCode)
        {
            DataSet ds = null;
            string[] QueryPname = new string[3];
            object[] QueryValue = new object[3];
            SqlDbType[] QueryType = new SqlDbType[3];
            try
            {
                SQLServer da = new SQLServer(constr);

                QueryPname[0] = "UserID";
                QueryPname[1] = "RoleID";
                QueryPname[2] = "StationCode";

                QueryType[0] = SqlDbType.VarChar;
                QueryType[1] = SqlDbType.Int;
                QueryType[2] = SqlDbType.VarChar;

                QueryValue[0] = UserID;
                QueryValue[1] = RoleID;
                QueryValue[2] = StationCode;

                ds = da.SelectRecords("SP_GetUserListdata", QueryPname, QueryValue, QueryType);


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
                QueryPname = null;
                QueryValue = null;
                QueryType = null;
            }
            return null;
        }
        #endregion Get User List Data

        #region Get User Details
        public DataSet GetUserDetails(string LoginID)
        {
            DataSet ds = null;
            string[] QueryPname = new string[1];
            object[] QueryValue = new object[1];
            SqlDbType[] QueryType = new SqlDbType[1];
            try
            {
                SQLServer da = new SQLServer(constr);
                QueryPname[0] = "LoginID";
                QueryType[0] = SqlDbType.VarChar;
                QueryValue[0] = LoginID;

                ds = da.SelectRecords("SP_GetUserDetails", QueryPname, QueryValue, QueryType);
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
                QueryPname = null;
                QueryValue = null;
                QueryType = null;
            }
            return (null);
        }
        #endregion Get User Details

        #region GetUser
        public DataSet GetUserLogActivity(string UserName,DateTime FromDt,DateTime ToDt)
        {
            DataSet ds = null;
            string[] QueryPname = new string[3];
            object[] QueryValue = new object[3];
            SqlDbType[] QueryType = new SqlDbType[3];
            try
            {
                SQLServer da = new SQLServer(constr);
                QueryPname[0] = "UserName";
                QueryPname[1] = "FromDt";
                QueryPname[2] = "ToDt";

                QueryType[0] = SqlDbType.VarChar;
                QueryType[1] = SqlDbType.DateTime;
                QueryType[2] = SqlDbType.DateTime;

                QueryValue[0] = UserName;
                QueryValue[1] = FromDt;
                QueryValue[2] = ToDt;

                ds = da.SelectRecords("SP_GetUserLogDetails", QueryPname, QueryValue, QueryType);


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
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
                QueryPname = null;
                QueryValue = null;
                QueryType = null;
            }
            return (null);
        }

        #endregion GetUser

        #region Get Stations For Country
        public DataSet GetAllStationsForCountry(string country)
        {
            DataSet ds = null;
            string[] QueryPname = new string[1];
            object[] QueryValue = new object[1];
            SqlDbType[] QueryType = new SqlDbType[1];
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(constr);

                QueryPname[0] = "country";
                QueryType[0] = SqlDbType.VarChar;
                QueryValue[0] = country;

                ds = da.SelectRecords("SP_GetAllStationCodeForCountry", QueryPname, QueryValue, QueryType);
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
                QueryPname = null;
                QueryValue = null;
                QueryType = null;
            }
            return (null);
        }
        #endregion Get Stations For Country
    }
}
