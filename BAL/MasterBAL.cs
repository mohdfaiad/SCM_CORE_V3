using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QID.DataAccess;
using System.Data;

namespace BAL
{
    public class MasterBAL
    {

        SQLServer db = new SQLServer(Global.GetConnectionString());

        #region Get Airline Details
        public DataSet GetAirlineDetails(string Origin, string Dest)
        {
            DataSet ds = new DataSet();
            string[] Pname = new string[2];
            object[] Pvalue = new object[2];
            SqlDbType[] Ptype = new SqlDbType[2];

            if (db == null)
                db = new SQLServer(Global.GetConnectionString());

            try
            {                

                Pname[0] = "Origin";
                Ptype[0] = SqlDbType.VarChar;
                Pvalue[0] = Origin;

                Pname[1] = "Destination";
                Ptype[1] = SqlDbType.VarChar;
                Pvalue[1] = Dest;

                ds = db.SelectRecords("SPGetAirlineDetails", Pname, Pvalue, Ptype);
            }
            catch (Exception ex)
            {
                return ds;
            }
            finally
            {
                db = null;
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
            return ds;


        }

        #endregion Get Airline Details

        #region GetPrefix
        public string awbPrefix()
        {
            try
            {
                
                return db.GetString("select top 1 prefix from awbprefixmaster");
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        #endregion
       
        
        #region GetPrefix
        public string awbPrefix(string agentCode)
        {
            try
            {
               return GetAWBprefixForAgent(agentCode);
                // return db.GetString("select top 1 prefix from awbprefixmaster");
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        #endregion

        #region Get Service Tax
        public string getServiceTax()
        {
            try
            {
                return db.GetString("select ParameterValue from DefaultValues where ParameterName = 'ServiceTax'");
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        #endregion

        #region Get Service Tax
        public string getServiceTax(string Origin, string dest, string type)
        {
            try
            {
                string[] PName = new string[] { "Origin", "Dest", "Charges" };
                object[] PValue = new object[] { Origin, dest, type };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                return db.GetStringByProcedure("spGetServiceTaxValue", PName, PValue, PType);
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        #endregion

        #region AirlinePrefix
        public string AirlinePrefix()
        {
            try
            {
                return db.GetString("select top 1 AirlinePrefix from awbprefixmaster");
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        #endregion

        #region Get Agent List
        /// <summary>
        /// Get awb for agents based on entered agent code.
        /// </summary>

        public string GetAWBprefixForAgent(string agentCode)
        {
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(Global.GetConnectionString());
                string ds = da.GetStringByProcedure("spGetAWBprefixAgentStock", "agent", agentCode, SqlDbType.VarChar);
                //if (ds != null)
                //{
                //    if (ds.Tables != null)
                //    {
                //        if (ds.Tables.Count > 0)
                //        {
                //            return (ds);
                //        }
                //    }
                //}
                return ds;
            }
            catch (Exception)
            {
            }
            return null;
        }
        #endregion Get Agent List

        #region CheckConfiguration
        public bool CheckConfiguration(string AppKey,string parameter) 
        {
            bool flag = false;
            try 
            {
                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                DataSet ds = objSQL.SelectRecords("GetConfiguration", "Appkey", AppKey, SqlDbType.VarChar);
                if (ds != null) 
                {
                    if (ds.Tables.Count > 0) 
                    {
                        if (ds.Tables[0].Rows.Count > 0) 
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++) 
                            {
                                DataRow dr = ds.Tables[0].Rows[i];
                                if (dr["Parameter"].ToString().Equals(parameter, StringComparison.OrdinalIgnoreCase)) 
                                {
                                    flag = true;
                                    i = (ds.Tables[0].Rows.Count) + 1;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
        #endregion

        #region Client Name
        public string clientName()
        {
            try
            {

                return db.GetString("select top 1 clientname from awbprefixmaster");
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        #endregion Client Name

        #region Client Address
        public string clientAddress()
        {
            try
            {

                return db.GetString("select top 1 clientaddress from Clientmaster");
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        #endregion Client Name

        #region Contact URL
        public string ContactURL()
        {
            try
            {

                return db.GetString("select top 1 ContactURL from ClientMaster");
            }
            catch (Exception)
            {
                return null;

            }
        }
        #endregion Contact URL

    }
}
