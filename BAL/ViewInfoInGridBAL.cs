using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class ViewInfoInGridBAL
    {

        public bool GetDistinctNamesOfAllCharges(string type, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                errormessage = "";

                string[] param = { "type" };
                SqlDbType[] dbtype = { SqlDbType.VarChar };
                object[] values = { type };

                SQLServer da = new SQLServer(Global.GetConnectionString());
                dsResult = da.SelectRecords("SP_GetDistinctNamesOfALLCharges", param, values, dbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error : Code(II)(GetDistinctNamesOfAllCharges)";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error : Code(I)(GetDistinctNamesOfAllCharges)";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
        }
    }
}
