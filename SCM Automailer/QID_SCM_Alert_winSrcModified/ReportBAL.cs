using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using clsDataLib;
using System.Data;

namespace QID_SCM_Alert_winSrcModified
{
    // SQLServer db = new SQLServer(Global.GetConnectionString());
     

    class ReportBAL
    {

        Database db = new Database();
     

        # region Get Station wise  Summary report using  selecetd criteria
        public DataSet GetStationWiseAWBSummary(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, DateTime frmDate, DateTime ToDt, string AWBstatus)
        {
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                DataSet ds = db.SelectRecords("Sp_rptGetAWBStationwiseReport", colNames, values, dataTypes);
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

        # endregion


        # region Get Agent wise  Summary report using  selecetd criteria
        public DataSet GetAgentWiseAWBSummary(string AgentCode, string PaymentType, string contrLocatorCode, string level, string levelCode, DateTime frmDate, DateTime ToDt, string AWBstatus)
        {
            try
            {

                string[] colNames = new string[8];
                object[] values = new object[8];
                SqlDbType[] dataTypes = new SqlDbType[8];
                int i = 0;

                colNames.SetValue("agentCode", i);
                values.SetValue(AgentCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("paymentType", i);
                values.SetValue(PaymentType, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;


                colNames.SetValue("contrLocatorCode", i);
                values.SetValue(contrLocatorCode, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("level", i);
                values.SetValue(level, i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                i++;

                colNames.SetValue("lveleCode", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(levelCode, i);

                i++;


                colNames.SetValue("frmDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(frmDate, i);

                i++;

                colNames.SetValue("toDate", i);
                dataTypes.SetValue(SqlDbType.DateTime, i);
                values.SetValue(ToDt, i);


                i++;

                colNames.SetValue("AWBStatus", i);
                dataTypes.SetValue(SqlDbType.VarChar, i);
                values.SetValue(AWBstatus, i);

                DataSet ds = db.SelectRecords("Sp_rptGetAWBAgentwiseReport", colNames, values, dataTypes);
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

        # endregion 

    }
}
