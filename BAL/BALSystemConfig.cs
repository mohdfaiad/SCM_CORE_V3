using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using QID.DataAccess;


namespace BAL
{
    public class BALSystemConfig
    {
        #region Variables

        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());

        #endregion Variables

        #region Save Data
        public DataSet SaveData(string Parameter, string Value, string App_Key,string Description)
        {
            SQLServer db = new SQLServer(constr);

            try
            {
                string[] paramname = new string[4];
                
                paramname[0] = "Parameter";
                paramname[1] = "Value";
                paramname[2] = "App_Key";
                paramname[3] = "Description";


                object[] paramvalue = new object[4];
                
                paramvalue[0] = Parameter;
                paramvalue[1] = Value;
                paramvalue[2] = App_Key;
                paramvalue[3] = Description;


                SqlDbType[] paramtype = new SqlDbType[4];
                
                paramtype[0] = SqlDbType.NVarChar;
                paramtype[1] = SqlDbType.NVarChar;
                paramtype[2] = SqlDbType.NVarChar;
                paramtype[3] = SqlDbType.NVarChar;


                DataSet ds = new DataSet();
                ds = db.SelectRecords("SP_GetSystemConfig", paramname, paramvalue, paramtype);
                return ds;


            }
            catch (Exception ex)
            {
                return null;
            }

        }
        
        #endregion

        #region List Data
        public DataSet SearchList(string Parameter,string App_Key)
        {
            SQLServer db = new SQLServer(constr);

            try
            {
                DataSet ds = new DataSet();
                string [] paramname=new string[2];
                paramname[0] = "Parameter";
                paramname[1] = "App_Key";
                SqlDbType [] paramtype=new SqlDbType[2];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                object [] paramvalue=new object[2];
                paramvalue[1] =App_Key;
                paramvalue[0] = Parameter;
                ds = db.SelectRecords("SP_GetSystemConfigList", paramname, paramvalue, paramtype);
               // ds = db.GetDataset("exec SP_GetSystemConfigList");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        public DataSet SearchListByKey(object[] SystemInforma)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                //0
                ColumnNames.SetValue("App_Key", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(SystemInforma.GetValue(i), i);
                i++;

                ////1
                //ColumnNames.SetValue("App_Key", i);
                //DataType.SetValue(SqlDbType.VarChar, i);
                //Values.SetValue(SystemInfo.GetValue(i), i);

                DataSet ds = new DataSet();
                ds = da.SelectRecords("sp_ListSysParamBykey", ColumnNames, Values, DataType);
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
        # region list filter
        public DataSet getList()
        {
            SQLServer db = new SQLServer(constr);

            try
            {
                DataSet ds = new DataSet();
                ds = db.GetDataset("exec sp_getparamterlist");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        
        }
        # endregion
        #region list criteria
        public DataSet GetParameterList(object[] SystemInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                //0
                ColumnNames.SetValue("Parameter", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(SystemInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("App_Key", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(SystemInfo.GetValue(i), i);

                DataSet ds = new DataSet();
                ds = da.SelectRecords("sp_ListSysParam", ColumnNames, Values, DataType);
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

         #endregion
        # region add Update System Configuration
        public string addUpdateSystemConfiguration(string Parameter, string Value, string AppKey, string flag, string Description)
        {
            try
            {
                string[] Pname = new string[5];
                object[] Pvalue = new object[5];
                SqlDbType[] Ptype = new SqlDbType[5];


                Pname[0] = "Parameter";
                Pname[1] = "Value";
                Pname[2] = "AppKey";
                Pname[3] = "Flag";
                Pname[4] = "Description";


                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;


                Pvalue[0] = Parameter;
                Pvalue[1] = Value;
                Pvalue[2] = AppKey;
                Pvalue[3] = flag;
                Pvalue[4] = Description;

                string res = db.GetStringByProcedure("SP_AddUpdateSystemConfig", Pname, Pvalue, Ptype);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion add Update System Configuration
    }
}
