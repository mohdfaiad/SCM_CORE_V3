using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;


/*
 * 2012-08-03  vinayak
*/

namespace BAL
{
    public class ShowFlightsBAL
    {

        public bool GetFlightList(string origin, string destination, ref DataSet dsResult, ref string errormessage, 
            DateTime DtCurrentDate,int dateOffset)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());

            string[] param = { "Origin", "Dest", "Date", "dtCurrentDate" };
            SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.DateTime };
            object[] values = { origin, destination, dateOffset, DtCurrentDate };

            try
            {
                //Get flight codes...                

                dsResult = da.SelectRecords("spGetFlightListFromOrgToDestNew", param, values, sqldbtype);

                //values[2] = 1;
                //DataSet dsResultNextDate = da.SelectRecords("spGetFlightListFromOrgToDestNew", param, values, sqldbtype);

                //dsResult.Merge(dsResultNextDate.Copy());

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        //if (dsResult.Tables[0].Rows.Count > 0)
                        //{
                        //    return true;
                        //}
                        //else
                        //{
                        //    errormessage = "No record found";
                        //    return false;
                        //}
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
            finally
            {
                da = null;
                param = null;
                sqldbtype = null;
                values = null;
            }

        }

        public bool GetFlightListForManifest(string origin, string destination, int date, ref DataSet dsResult, 
            ref string errormessage, DateTime DtCurrentDt, string AWBNo, string FlightId, DateTime FlightDate)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            string[] param = { "Origin", "Dest", "Date", "dtCurrentDate", "AWBNo", "FlightId", "FlightDate" };
            SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime };
            object[] values = { origin, destination, date, DtCurrentDt, AWBNo, FlightId, FlightDate };

            try
            {
                //Get flight codes...                

                dsResult = da.SelectRecords("[spGetFlightListFromOrgToDestNewforManifest]", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
            finally
            {
                da = null;
                param = null;
                sqldbtype = null;
                values = null;
            }
        }
        
        public bool GetFlightList(string origin, string destination, int date, ref DataSet dsResult, 
            ref string errormessage,DateTime DtCurrentDt)
        {
            try
            {
                //Get flight codes...
                SQLServer da = new SQLServer(Global.GetConnectionString());

                string[] param = { "Origin", "Dest", "Date", "dtCurrentDate" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.DateTime};
                object[] values = { origin, destination, date, DtCurrentDt};

                dsResult = da.SelectRecords("spGetFlightListFromOrgToDestNew", param, values, sqldbtype);

                //values[2] = 1;
                //DataSet dsResultNextDate = da.SelectRecords("spGetFlightListFromOrgToDestNew", param, values, sqldbtype);

                //dsResult.Merge(dsResultNextDate.Copy());

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        //if (dsResult.Tables[0].Rows.Count > 0)
                        //{
                        //    return true;
                        //}
                        //else
                        //{
                        //    errormessage = "No record found";
                        //    return false;
                        //}
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

        public bool GetAllDestinationsForOrigin(string origin, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                //Get flight codes...
                SQLServer da = new SQLServer(Global.GetConnectionString());

                string[] param = { "Source" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar };
                object[] values = { origin };

                dsResult = da.SelectRecords("spGetAllDestinationsFromOrigin", param, values, sqldbtype);
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            errormessage = "No record found";
                            return false;
                        }
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

        public bool GetRouteData(string origin, string destination, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                //Get flight codes...
                SQLServer da = new SQLServer(Global.GetConnectionString());

                string[] param = { "Origin", "Dest" };
                SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { origin, destination };

                dsResult = da.SelectRecords("spGetRouteDataFromOrgToDest", param, values, sqldbtype);
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            errormessage = "No record found";
                            return false;
                        }
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

        public bool GetPartnerFlightList(string origin, string destination, ref DataSet dsResult, ref string errormessage, DateTime DtCurrentDate, string PartnerId)
        {
            //Get flight codes...
            SQLServer da = new SQLServer(Global.GetConnectionString());

            string[] param = { "Origin", "Dest", "Date", "dtCurrentDate", "Partnerid" };
            SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.DateTime, SqlDbType.VarChar };
            object[] values = { origin, destination, 0, DtCurrentDate, PartnerId };

            try
            {
                dsResult = da.SelectRecords("spGetPartnerFlightListFromOrgToDestNew", param, values, sqldbtype);

               // values[2] = 1;
               // DataSet dsResultNextDate = da.SelectRecords("spGetPartnerFlightListFromOrgToDestNew", param, values, sqldbtype);

               // dsResult.Merge(dsResultNextDate.Copy());

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        //if (dsResult.Tables[0].Rows.Count > 0)
                        //{
                        //    return true;
                        //}
                        //else
                        //{
                        //    errormessage = "No record found";
                        //    return false;
                        //}
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
            finally
            {
                da = null;
                param = null;
                sqldbtype = null;
                values = null;
            }
        }

        /*
        public bool GetFlightListforDay(string origin, string destination, ref DataSet dsResult, ref string errormessage, DateTime DtCurrentDate)
        {
            //Get flight codes...
            SQLServer da = new SQLServer(Global.GetConnectionString());

            string[] param = { "Origin", "Dest", "Date", "dtCurrentDate" };
            SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.DateTime };
            object[] values = { origin, destination, 0, DtCurrentDate };

            try
            {   

                dsResult = da.SelectRecords("spGetFlightListFromOrgToDestNew", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
            finally
            {
                da = null;

                param = null;
                sqldbtype = null;
                values = null;
            }
        }

         */
         
        public bool GetFlightListforDay(string origin, string destination, ref DataSet dsResult, ref string errormessage, DateTime DtCurrentDate,string AirlineCode)
        {
            //Get flight codes...
            SQLServer da = new SQLServer(Global.GetConnectionString());

            string[] param = { "Origin", "Dest", "Date", "dtCurrentDate", "AirlineCode" };
            SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.DateTime, SqlDbType.VarChar };
            object[] values = { origin, destination, 0, DtCurrentDate, AirlineCode };

            try
            {

                dsResult = da.SelectRecords("spGetFlightListFromOrgToDestNew", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
            finally
            {
                da = null;

                param = null;
                sqldbtype = null;
                values = null;
            }
        }

        public bool GetFlightListforDay_QB(string origin, string destination, ref DataSet dsResult, ref string errormessage, DateTime DtCurrentDate, string AirlineCode)
        {
            //Get flight codes...
            SQLServer da = new SQLServer(Global.GetConnectionString());

            string[] param = { "Origin", "Dest", "Date", "dtCurrentDate", "AirlineCode" };
            SqlDbType[] sqldbtype = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.DateTime, SqlDbType.VarChar };
            object[] values = { origin, destination, 0, DtCurrentDate, AirlineCode };

            try
            {

                dsResult = da.SelectRecords("spGetFlightListFromOrgToDestNew_QB", param, values, sqldbtype);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
            finally
            {
                da = null;

                param = null;
                sqldbtype = null;
                values = null;
            }
        }
    }
}
