using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QID.DataAccess;
using System.Data;
using System.Configuration;

namespace BAL
{
    public class CreditBAL
    {

        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        #endregion Variables


        #region Get Bank List
        /// <summary>
        /// Get list of all the Bank based on entered value.
        /// </summary>
        /// <returns>Bank Name list as Array.</returns>
        public DataSet GetBankList(string BankName)
        {
            try
            {
                //Get commodity codes...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("Sp_BankName");
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
        #endregion GetBank List

        #region Get Agent List
        /// <summary>
        /// Get list of all the agents based on entered value.
        /// </summary>
        /// <returns>Agent code list as Array.</returns>
        public DataSet GetAgentList(string agentCode)
        {
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetAgentByPrefix", "AgentCode", agentCode, SqlDbType.VarChar);
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
        #endregion Get Agent List

    }
}
