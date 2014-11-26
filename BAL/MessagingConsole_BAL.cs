using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data.SqlClient;
using System.Data;

namespace BAL
{
    public class MessagingConsole_BAL
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());
        //SQLServer db = new SQLServer("Data Source=72.167.41.153;Initial Catalog=SCM_V2;User ID=sa;Password=QID#tech#123");

        #region GetAWBDetails
        public DataSet GetAWBDetails(string Location, string FlightNumber, DateTime FlightDate)
        {
            //SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            try
            {

                string[] Pname = new string[3];
                object[] Pvalue = new object[3];
                SqlDbType[] Ptype = new SqlDbType[3];


                Pname[0] = "POL";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = Location;

                Pname[1] = "FlightNo";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FlightNumber;

                Pname[2] = "FltDate";
                Ptype[2] = SqlDbType.DateTime;
                Pvalue[2] = FlightDate;



                ds = db.SelectRecords("SPExpManiGetTabAWBdata_MessageCenter", Pname, Pvalue, Ptype);


                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }

        }
        #endregion

        #region GetAllAWBDetails

        public DataSet GetALLAWBDetails(string AWBNumber)
        {
            DataSet dsResult = new DataSet();
            try
            {
                
                string[] param = {  "AWBNumber"};
                SqlDbType[] sqldbtype = {  SqlDbType.VarChar };
                object[] values = { AWBNumber };


                dsResult = db.SelectRecords("SP_GetAWBDetails_MessageCenter", param, values, sqldbtype);

          
                return dsResult;
             

            }
            catch (Exception ex)
            {
                return dsResult;
            }
           
            
        }

        public DataSet GetALLAWBDetails_FFA(string AWBNumber)
        {
            DataSet dsResult = new DataSet();
            try
            {

                string[] param = { "AWBNumber" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar };
                object[] values = { AWBNumber };


                dsResult = db.SelectRecords("SP_GetAWBDetails_MessageCenter_New", param, values, sqldbtype);


                return dsResult;


            }
            catch (Exception ex)
            {
                return dsResult;
            }


        }
        #endregion

        #region Get Manifest Details
        public DataSet GetManifestDetails(string Location, string FlightNumber, DateTime FlightDate)
        {
            //SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            try
            {

                string[] Pname = new string[3];
                object[] Pvalue = new object[3];
                SqlDbType[] Ptype = new SqlDbType[3];


                Pname[0] = "DepartureAirport";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = Location;

                Pname[1] = "FLTno";
                Ptype[1] = SqlDbType.NVarChar;
                Pvalue[1] = FlightNumber;

                Pname[2] = "Manifestdate";
                Ptype[2] = SqlDbType.DateTime;
                Pvalue[2] = FlightDate;



                ds = db.SelectRecords("SPExpManiGetManifestDetails_MessageCenter", Pname, Pvalue, Ptype);


                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }

        }


        #endregion

    }
}
