using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public  class BALAWBtracking
    {
        
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        #endregion Variables
         
        #region Get AWB Tracking Data
        public DataSet GetAWBTrackingData(string AWBPrefix, string AWBno)
        {
            DataSet ds = new DataSet ();
            try
            {
                string[] awbnos = AWBno.Split(',');
                string FormattedAWBlist = "", FinalAWBlist = "";

                for (int i = 0; i < awbnos.Length; i++)
                {
                    FormattedAWBlist += "" + awbnos.GetValue(i) + ",";
                }
                FinalAWBlist = FormattedAWBlist.Remove(FormattedAWBlist.Length - 1, 1);

                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int j = 0;

                ColumnNames.SetValue("AWBPrefix", j);
                DataType.SetValue(SqlDbType.VarChar, j);
                Values.SetValue(AWBPrefix, j);
                j++;

                ColumnNames.SetValue("FinalAWBlist", j);
                DataType.SetValue(SqlDbType.VarChar, j);
                Values.SetValue(FinalAWBlist, j);

                ds = da.SelectRecords("SPAWBtrackingGetAWBMdata", ColumnNames, Values, DataType);
                
                ColumnNames = null;
                DataType = null;
                Values = null;

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
                return ds;
            }
            catch (Exception)
            {
                return ds;
            }        
        }

        #endregion Get AWB Tracking Data


        #region Getting DONumbers

        public DataSet IsCollect(string AWBPrefix, string AWBNumber)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] QueryNames = { "AWBPrefix", "AWBNumber" };
                object[] QueryValues = { AWBPrefix, AWBNumber };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar };
                DataSet ds = da.SelectRecords("SP_IsCollect", QueryNames, QueryValues, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                return (ds);
                            }
                            else
                                return null;
                        }
                        else
                            return null;
                    }
                    else
                        return null;
                }
                return ds;
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion
        #region Getting DONumbers

        public DataSet GetDoNumbersAWBTracking(string AWBPrefix, string AWBNumber)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] QueryNames = { "AWBPrefix", "AWBNumber" };
                object[] QueryValues = { AWBPrefix, AWBNumber };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar };

                DataSet ds = da.SelectRecords("sp_GetDoNumbersAWBTracking", QueryNames, QueryValues, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                return (ds);
                            }
                            else
                                return null;
                        }
                        else
                            return null;
                    }
                    else
                        return null;
                }
                return ds;
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

    }
}
