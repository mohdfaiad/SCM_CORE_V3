using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;

// 2012-07-17 vinayak

namespace BAL
{
    public class ListRateLineBAL
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());


        #region Get Origin List
        public DataSet GetOriginList(string OriginList)
        {
            try
            {

                DataSet ds = db.SelectRecords("SP_GetOriginForListRateLine");
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

        #endregion Get Origin List

        #region Get Destination List
        public DataSet GetDestinationList(string DestinationList)
        {
            try
            {

                DataSet ds = db.SelectRecords("SP_GetDestinationForListRateLine");
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

        #endregion Get Destination List

        # region GetListRateLine
        public DataSet GetListRateLine(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[21];
                SqlDbType[] DataType = new SqlDbType[21];
                Object[] Values = new object[21];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("ORGLevel", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DestLevel", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Destination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("FromDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                i++;


                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                i++;


                ColumnNames.SetValue("Status", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;
                //added by jayant
                ColumnNames.SetValue("AllIn", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TACT", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ULD", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Heavy", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RtcardID", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Param", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ShipperCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CommCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ProductType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;
                //end

                ColumnNames.SetValue("ExpiresFrom", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                i++;


                ColumnNames.SetValue("ExpiresTo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RateID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RateType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                DataSet ds = db.SelectRecords("SP_GetListRateLine", ColumnNames, Values, DataType);
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


        # endregion GetListRateLine
        #region updateRateDates
        public bool UpdateRateLine(object[] RateLineInfo)
        {
            string[] ColumnNames = new string[3];
            SqlDbType[] DataType = new SqlDbType[3];
            Object[] Values = new object[3];
            int i = 0;
            try
            {

                ColumnNames.SetValue("SrNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FrmDate", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                bool res = db.UpdateData("sp_updateRateLineDates", ColumnNames, DataType, Values);
                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion updateRateDates

        #region getExpRateLin
        public DataSet getExpRateLine(string srno)
        {
            try
            {
                DataSet ds = db.SelectRecords("sp_getExportRateline", "serialnumber", srno, SqlDbType.NVarChar);
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


        #endregion getExpRateLin
        # region GetListRateLine_ALL
        public DataSet GetListRateLine_ALL()
        {
            try
            {

              DataSet ds = db.SelectRecords("SP_GetListRateLine_ALL");
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

        # endregion GetListRateLine_ALL
    }
}
