using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;


/*
2012-05-14 vinayak
*/

namespace BAL
{
    public class SpotRateTransactionBAL
    {


        #region Get AWB Commodites

        public bool GetAWBCommodites(string AWBNumber, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                errormessage = "";

                SQLServer da = new SQLServer(Global.GetConnectionString());
                dsResult = da.SelectRecords("SP_SPOTRateTrn_GetAWBCommodities", "AWBNumber", AWBNumber, SqlDbType.VarChar);
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return (true);                        
                    }
                }
                return false;
            }
            catch (Exception ex)            
            {
                errormessage = "" + ex.Message;
                return false;
            }
            
        }
        #endregion

        #region Get AWB Comm Details

        public bool GetAWBCommDetails(string AWBNumber,object[] values, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                errormessage = "";

                SQLServer da = new SQLServer(Global.GetConnectionString());
                string[] param = { "AWBNumber", "CommCode" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar };


                dsResult = da.SelectRecords("SP_SPOTRateTrn_GetAWBCommDetails", param, values, dbtypes);
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return (true);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }
        #endregion


        #region Save Spot Rate 

        public bool SaveSpotRates(object[] values, DataSet dsAWBDetails)
        {
            try
            {

                string[] param = { "SpotRateCategory", "SpotRate", "Currency", "Station", "ReqDate", "FWDName", "Remark", "IssuedBy", "IssuedDate", "AuthorizedBy", "AutherizedDate", "ValidFrom", "ValidTo" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.Float, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                SQLServer db = new SQLServer(Global.GetConnectionString());
                DataSet dsResult = db.SelectRecords("SP_SPOTRateTrn_SaveSpotRateManagement", param, values, dbtypes);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {

                        if (dsResult.Tables[0].Rows.Count != 0)
                        {

                            int ID = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());


                            for (int i = 0; i < dsAWBDetails.Tables[0].Rows.Count; i++)
                            {
                                DataRow row = dsAWBDetails.Tables[0].Rows[i];

                                param = new string[] { "SpotRateManagementID", "AWBNumber", "CommodityCode", "SpotRate" };
                                dbtypes = new SqlDbType[] { SqlDbType.Int,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Float};
                                values = new object[] { ID, row["AWBNumber"], row["CommodityCode"], decimal.Parse(row["SpotRate"].ToString()) };

                                db.ExecuteProcedure("SP_SPOTRateTrn_SaveSpotRateManagementDetails", param, dbtypes, values);

                            }

                            return true;

                        }

                    }

                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion



    }
}
