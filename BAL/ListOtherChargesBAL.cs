using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class ListOtherChargesBAL
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

                string[] ColumnNames = new string[17];
                SqlDbType[] DataType = new SqlDbType[17];
                Object[] Values = new object[17];
                int i = 0;

                i = 0;

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

                ColumnNames.SetValue("ChargeCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChargeType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;
                ColumnNames.SetValue("ChargeName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;
                ColumnNames.SetValue("Parameter", i);
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

                ColumnNames.SetValue("ExpiresFrom", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                i++;


                ColumnNames.SetValue("ExpiresTo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("chargedAt", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("OCID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                
                DataSet ds = db.SelectRecords("SP_GetOtherChrgesFilterd", ColumnNames, Values, DataType);
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
        #region UpdateOC
        public bool UpdatOtherCharges(object[] RateLineInfo)
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

                bool res = db.UpdateData("sp_updateOtherChargesDates", ColumnNames, DataType, Values);
                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion UpdateOC
        #region getExpOC
        public DataSet getExpOC(string srno)
        {
            try
            {
                DataSet ds = db.SelectRecords("sp_getExportOtherCharges", "serialnumber", srno, SqlDbType.NVarChar);
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

        # region GetListOC_ALL
        public DataSet GetListOC_ALL()
        {
            try
            {

                DataSet ds = db.SelectRecords("SP_GetListOCAll");
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

        #region GetCurrencyCode
        public DataSet GetCurrencyCode(string OriginList)
        {
            try
            {

                DataSet ds = db.SelectRecords("[spGetOtherCurrencyCode]");
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

        #endregion GetCurrencyCode


    }
}
