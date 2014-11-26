using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;

namespace BAL
{
   public class BALException
   {
        #region Variables
       SQLServer da = new SQLServer(Global.GetConnectionString());
       #endregion Variables
       
        #region Get Origin Code
       public DataSet GetOriginCodeList(string OriginCodeList)
        {
            DataSet ds = null;
            try
            {
                //SQLServer da = new SQLServer(constr);
                ds = new DataSet();
                ds = da.SelectRecords("[GetExpAirportCodeList]");
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


       
       
       public DataSet GetCountryCodeList(string CountryCodeList)
       {
           DataSet ds = null;
           try
           {
               //SQLServer da = new SQLServer(constr);
               ds = new DataSet();
               ds = da.SelectRecords("[SP_GetCountryCode]");
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

       # region Get Destination Code
       public DataSet GetDestinationCodeList(string DestinationCodeList)
        {
            DataSet ds = null;
            try
            {
                ds = new DataSet();
                ds = da.SelectRecords("GetExceptionOriginCodeList");
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

        # region Get Agent Code
        public DataSet GetAgentCodeList(string AgentCodeList)
        {
            DataSet ds = null;
            try
            {
                ds = new DataSet();
                ds = da.SelectRecords("GetExceptionOriginCodeList");
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
        # endregion Get Agent Code 

        #region Get Airport Code
        public DataSet GetAirportCodeList(string AirportCodeList)
        {
            DataSet ds = null;
            try
            {
                ds = new DataSet();
                ds = da.SelectRecords("GetExpAirportCodeList");
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
