using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BALAutoReportsConfig
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables 

        #region get Current Report Config
        public DataSet getCurrentReportConfig(string ReportName, string FromDate, string ToDate)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            try
            {
                string[] Pname = new string[3];
                object[] Pvalue = new object[3];
                SqlDbType[] Ptype = new SqlDbType[3];

                Pname[0] = "ReportName";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = ReportName;

                Pname[1] = "FromDate";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FromDate;

                Pname[2] = "ToDate";
                Ptype[2] = SqlDbType.NVarChar;
                Pvalue[2] = ToDate;

                ds = da.SelectRecords("SPgetAutoReportConfig", Pname, Pvalue, Ptype);
                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }

        }
        #endregion get Current Report Config


        #region Save/Add ULD AWb Details
        public DataSet SaveAutoReportsConfig(int SrNo, string ReportName, string ToEmail, string FromDate, string ToDate, string SPName, string Frequency, bool isActive, string UpdatedBy, string UpdatedOn, ref string Result)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;

                string[] Pname = new string[10];
                object[] Pvalue = new object[10];
                SqlDbType[] Ptype = new SqlDbType[10];

                Pname[0] = "SrNo";
                Pname[1] = "ReportName";
                Pname[2] = "ToEmail";
                Pname[3] = "FromDate";
                Pname[4] = "ToDate";
                Pname[5] = "SPName";
                Pname[6] = "Frequency";
                Pname[7] = "IsActive";
                Pname[8] = "UpdatedBy";
                Pname[9] = "UpdatedOn";

                Ptype[0] = SqlDbType.Int;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.Bit;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;

                Pvalue[0] = SrNo;
                Pvalue[1] = ReportName;
                Pvalue[2] = ToEmail;
                Pvalue[3] = FromDate;
                Pvalue[4] = ToDate;
                Pvalue[5] = SPName;
                Pvalue[6] = Frequency;
                Pvalue[7] = isActive;
                Pvalue[8] = UpdatedBy;
                Pvalue[9] = UpdatedOn;


                ds = da.SelectRecords("SPSaveAutoReportsConfig", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }
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
