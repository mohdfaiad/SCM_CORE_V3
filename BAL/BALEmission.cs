using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BALEmission
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());

        #region Getting Emission Report
        public DataSet GetEmissionReport(string Origin, string Destination, string FromDate, string ToDate)
        {
            try
            {
                string[] PName = new string[4];
                PName[0] = "Origin";
                PName[1] = "Destination";
                PName[2] = "FromDate";
                PName[3] = "ToDate";

                SqlDbType[] paramtype = new SqlDbType[4];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;

                object[] ParamValues = { Origin, Destination, FromDate, ToDate };

                DataSet ds = db.SelectRecords("sp_EmissionReport", PName, ParamValues, paramtype);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return ds;
                        }
                        else
                        { return null; }
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Get Origin Code
        public DataSet GetOriginCodeList(string OriginCodeList)
        {
            DataSet ds = null;
            try
            {
                //SQLServer da = new SQLServer(constr);
                ds = new DataSet();
                ds = db.SelectRecords("[GetExceptionOriginCodeList]");
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
        # endregion

        # region Get Destination Code
        public DataSet GetDestinationCodeList(string DestinationCodeList)
        {
            DataSet ds = null;
            try
            {
                ds = new DataSet();
                ds = db.SelectRecords("GetExceptionOriginCodeList");
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
        # endregion 
    }
}
