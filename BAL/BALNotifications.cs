using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using QID.DataAccess;

namespace BAL
{
    public class BALNotifications
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion

        #region Get UserList
        public DataSet GetUserList()
        {
            DataSet ds = new DataSet();
            try
            {
                
                ds = db.SelectRecords("SP_GetUserNameList");

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
                if(ds!=null)
                {
                    ds.Dispose();
                }
            }
            return null;
        
        }

        #endregion

        #region Get Role List
        public DataSet GetRoleList()
        {
            DataSet ds = new DataSet();
            try
            {

                ds = db.SelectRecords("SP_GetRollList");

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
            return null;

        }

        #endregion

        #region Get Notification List
        public DataSet GetNotificationList(string FrmDate, string ToDate, string Importance, string UserName,string Station, string Role)
        {
            DataSet ds = new DataSet();

            try
            {
                string[]  Pname= new string[6];
                Pname[0] = "FrmDate";
                Pname[1] = "ToDate";
                Pname[2] = "Importance";
                Pname[3] = "UserName";
                Pname[4] = "Station";
                Pname[5] = "Role";

                object[] Pvalue = new object[6];
                Pvalue[0]= FrmDate;
                Pvalue[1]= ToDate;
                Pvalue[2]= Importance;
                Pvalue[3] = UserName;
                Pvalue[4] = Station;
                Pvalue[5] = Role;

                SqlDbType[] Ptype = new SqlDbType[6];
                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;

                ds = db.SelectRecords("Sp_GetNotificationMsg", Pname, Pvalue, Ptype);
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
            return null;
        }

        #endregion

        #region Save Notification Detials

        public bool SetNotificationDetails(string Importance, string NotificationType, string FrmDate, string ToDate, string NotificationMsg, 
            bool IsActive, string UpdatedBy, string Subject1, string UserRole, string UserNm, int RowId, DateTime TimeStamp)
        {
            //DataSet ds = new DataSet();
            bool flag=false;

            try
            {
                string[] Pname = new string[12];
                Pname[0] = "Importance";
                Pname[1] = "NotificationType";
                Pname[2] = "FrmDate";
                Pname[3] = "ToDate";
                Pname[4] = "NotificationMsg";
                Pname[5] = "IsActive";
                Pname[6] = "UpdatedBy";
                Pname[7] = "Subject1";
                Pname[8] = "UserRole";
                Pname[9] = "UserNm";
                Pname[10] = "TimeStamp";
                Pname[11] = "RowID";

                object[] Pvalue = new object[12];
                Pvalue[0] = Importance;
                Pvalue[1] = NotificationType;
                Pvalue[2] = FrmDate;
                Pvalue[3] = ToDate;
                Pvalue[4] = NotificationMsg;
                Pvalue[5] = IsActive;
                Pvalue[6] = UpdatedBy;
                Pvalue[7] = Subject1;
                Pvalue[8] = UserRole;
                Pvalue[9] = UserNm;
                Pvalue[10] = TimeStamp;
                Pvalue[11] = RowId;

                SqlDbType[] Ptype = new SqlDbType[12];
                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.Bit;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.DateTime;
                Ptype[11] = SqlDbType.Int;

                flag = db.InsertData("Sp_SetNotificationMsg",Pname,Ptype,Pvalue);
                return true;               
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        #endregion

        #region Get All Stations
        public DataSet GetAllStations()
        {
            DataSet ds = null;

            try
            {
                SQLServer da = new SQLServer(constr);

                ds = da.SelectRecords("spGetAirportCodes");
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
            }
            return (null);
        }
        #endregion Get All Stations

        #region Get Notification List
        public DataSet GetNotificationListForHome(DateTime Date, string UserName, string Station, string Role)
        {
            DataSet ds = new DataSet();

            try
            {
                string[] Pname = new string[4];
                Pname[0] = "Date";
                Pname[1] = "UserName";
                Pname[2] = "Station";
                Pname[3] = "Role";

                object[] Pvalue = new object[4];
                Pvalue[0] = Date;
                Pvalue[1] = UserName;
                Pvalue[2] = Station;
                Pvalue[3] = Role;

                SqlDbType[] Ptype = new SqlDbType[4];
                Ptype[0] = SqlDbType.DateTime;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;

                ds = db.SelectRecords("Sp_GetNotificationMsgHome", Pname, Pvalue, Ptype);
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
            return null;
        }

        #endregion
    }
}
