using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
    public class BALEmailID
    {
        SQLServer db = new SQLServer("");
        string constr = "";
        DataSet res;

        public BALEmailID()
        {
            constr = Global.GetConnectionString();


        }

        #region GetEmail
        public DataSet GetEmail(string Origin, string Destination, string MessageType, string FlightNumber, string PartnerCode)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[5];
                object[] Pvalue = new object[5];
                SqlDbType[] Ptype = new SqlDbType[5];

                Pname[0] = "Origin";
                Pname[1] = "Destination";
                Pname[2] = "MessageType";
                Pname[3] = "FlightNumber";
                Pname[4] = "PartnerCode";

                Pvalue[0] = Origin;
                Pvalue[1] = Destination;
                Pvalue[2] = MessageType;
                Pvalue[3] = FlightNumber;
                Pvalue[4] = PartnerCode;

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;

                res = da.SelectRecords("SpGetEmailDetails", Pname, Pvalue, Ptype);
                if (res != null)
                {
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                return res;
            }
            return res;
        }
        #endregion GetEmail
    }
}
