using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using QID.DataAccess;


namespace BAL
{
    public class AWBMilestonesBAL
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion Variables

        # region ADD AWB Milestone
        public string AddAWBMilestone(object[] MilestoneInfo)
        {
            try
            { 
                string[] ColumnNames = new string[6];
                SqlDbType[] DataType = new SqlDbType[6];
                Object[] Values = new object[6];
                int i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Pieces", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Milestone", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Weight", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_AddAWBMilestone", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion ADD AWB Milestone



        # region delete AWB Milestone when flight Repoen
        public string DeleteAWBMilestone(object[] MilestoneInfo)
        {
            try
            {
                string[] ColumnNames = new string[6];
                SqlDbType[] DataType = new SqlDbType[6];
                Object[] Values = new object[6];
                int i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Pieces", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Milestone", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Weight", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_DeleteAWBMilestone", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion 


        # region Add  AWB Milestone for Manifest
        public string AddAWBMaifestMilestone(object[] MilestoneInfo)
        {
            try
            {
                string[] ColumnNames = new string[6];
                SqlDbType[] DataType = new SqlDbType[6];
                Object[] Values = new object[6];
                int i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Pieces", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Milestone", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Weight", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(MilestoneInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_AddManifestMilestone", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion ADD AWB Milestone

    }
}
