using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
 
using System.Data;
using QID.DataAccess;
  //18-7-2012
namespace BAL
{
    public class AgentBAL
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

        //spGetSelfAgent
        #region Get Agent
        /// <summary>
        /// Get list of all the agents based on entered value.
        /// </summary>
        /// <returns>Agent code list as Array.</returns>
        public DataSet GetAgent(string agentCode)
        {
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetSelfAgent", "AgentCode", agentCode, SqlDbType.VarChar);
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
        #endregion Get Agent

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
       
        #region Get Agent List
        /// <summary>
        /// Get list of all the agents based on entered value.
        /// </summary>
        /// <returns>Agent code list as Array.</returns>
        public DataSet GetAgentListNew(string agentCode)
        {
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetAgentByPrefixNew", "AgentCode", agentCode, SqlDbType.VarChar);
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
        
        #region GetCreditDetails
        public DataSet GetCreditDetails(string creditNo)
        {
            //int rateCardId = Convert.ToInt32(rateCardName);
            string[] ColumnNames = new string[1];
            SqlDbType[] DataType = new SqlDbType[1];
            Object[] Values = new object[1];
            SQLServer da = new SQLServer(constr);

            ColumnNames[0] = "SerialNumber";
            DataType[0] = SqlDbType.VarChar;
            Values[0] = creditNo;

            DataSet dsRateCard = new DataSet();

            dsRateCard = da.SelectRecords("SPGetCreditDetails", ColumnNames, Values, DataType);
            return dsRateCard;
        }
        #endregion GetCreditDetails

        #region Get Credit Master Details
        public DataSet dsGetCreditMasterDetails(string AgentCode, string FromDt, string ToDt)
        {
            DataSet dsRateCard = new DataSet();
            //int rateCardId = Convert.ToInt32(rateCardName);
            try
            {
                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                SQLServer da = new SQLServer(constr);

                ColumnNames[0] = "AgentCode";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = AgentCode;

                ColumnNames[1] = "FromDt";
                DataType[1] = SqlDbType.VarChar;
                Values[1] = FromDt;

                ColumnNames[2] = "ToDt";
                DataType[2] = SqlDbType.VarChar;
                Values[2] = ToDt;

                dsRateCard = da.SelectRecords("sp_GetCreditMasterDtls", ColumnNames, Values, DataType);
                return dsRateCard;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (dsRateCard != null)
                {
                    dsRateCard.Dispose();
                }
            }

        }
 
        #endregion

        #region Get BG Collection Master Details
        public DataSet dsGetBGCollectionMasterDetails(string AgentCode, DateTime FromDt, DateTime ToDt, string AWBNumber, string TraType)
        {
            //int rateCardId = Convert.ToInt32(rateCardName);
            try
            {
                string[] ColumnNames = new string[5];
                SqlDbType[] DataType = new SqlDbType[5];
                Object[] Values = new object[5];
                SQLServer da = new SQLServer(constr);

                ColumnNames[0] = "AgentCode";
                DataType[0] = SqlDbType.VarChar;
                Values[0] = AgentCode;

                ColumnNames[1] = "FromDt";
                DataType[1] = SqlDbType.DateTime;
                Values[1] = FromDt;

                ColumnNames[2] = "ToDt";
                DataType[2] = SqlDbType.DateTime;
                Values[2] = ToDt;

                ColumnNames[3] = "AWBNumber";
                DataType[3] = SqlDbType.VarChar;
                Values[3] = AWBNumber;

                ColumnNames[4] = "TraType";
                DataType[4] = SqlDbType.VarChar;
                Values[4] = TraType;

                DataSet dsRateCard = new DataSet();

                dsRateCard = da.SelectRecords("sp_GetBGCollectionDtls", ColumnNames, Values, DataType);
                return dsRateCard;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #endregion

        #region Get Rates
        /// <summary>
        /// Get list of all the agents based on entered value.
        /// </summary>
        /// <returns>Agent code list as Array.</returns>
        public DataSet GetAgentRates(string agentCode)
        {
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SP_AgentMaster_GetRates", "AgentCode", agentCode, SqlDbType.VarChar);
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

        #region Deals
        public string SaveDealsSummary(object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] paramname = new string[4];
                SqlDbType[] paramtype = new SqlDbType[4];
                //    object[] paramvalue = new object[4];

                paramname[0] = "AgentCode";
                paramname[1] = "Type";
                paramname[2] = "DateFrom";
                paramname[3] = "DateTo";



                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                string result = da.GetStringByProcedure("SpSaveAgentDealsSummary", paramname, paramvalue, paramtype);
                return result;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public string SaveDealsDetails(object[] paramvalue)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] paramname = new string[5];
                SqlDbType[] paramtype = new SqlDbType[5];
                // object[] paramvalue = new object[4];

                paramname[0] = "AgentCode";
                paramname[1] = "AFrom";
                paramname[2] = "ATo";
                paramname[3] = "Percent";
                paramname[4] = "Value";

                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.Int;
                paramtype[2] = SqlDbType.Int;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.VarChar;

                string result = da.GetStringByProcedure("SpSaveAgentDealsDetails", paramname, paramvalue, paramtype);
                return result;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public DataSet GetDeals(string AgentCode)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SpgetAgentDealsDetails", "AgentCode", AgentCode, SqlDbType.VarChar);
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
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }
        #endregion

        # region Get Currency Code Shashi
        public DataSet GetCurrencyCodeList(string CurrencyCodeList)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                ds = da.SelectRecords("GetCurrencyCodeList");
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
            catch (Exception ex)
            {
            }
            return (null);
        }

        # endregion Get Currency Code Shashi

        #region ValidateDuplicate

        public bool CheckDuplicate(object[] paramvalue1, ref bool IsDuplicate, ref string errormessage)
        {
            try
            {
                string[] param = { "AgentCode",
                                   "AgentName",
                                   "FlightNo",
                                   "FromDate",
                                   "ToDate",
                                   "ComodityCode",
                                   "FreightWeight",
                                   "FreightRate",
                                   //"UpdatedBy",
                                   //"UpdatedOn",
                                   "StationCode",
                                   "DayOfWeek"
                                };

                SqlDbType[] dbtypes = { SqlDbType.VarChar,
                                        SqlDbType.VarChar,
                                        SqlDbType.VarChar,
                                        SqlDbType.DateTime,
                                        SqlDbType.DateTime,
                                        SqlDbType.VarChar,
                                        SqlDbType.VarChar,
                                        SqlDbType.VarChar,
                                        //SqlDbType.VarChar,
                                        //SqlDbType.DateTime,
                                        SqlDbType.VarChar,
                                        SqlDbType.VarChar 
                                      };

                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet dsResult = da.SelectRecords("SP_CheckDuplicateAgentCapacity", param, paramvalue1, dbtypes);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {

                        if (dsResult.Tables[0].Rows.Count != 0)
                        {
                            IsDuplicate = bool.Parse(dsResult.Tables[0].Rows[0][0].ToString());
                            return true;
                        }
                        else
                        {
                            errormessage = "Error : (CheckDuplicate) Code III";
                            return false;
                        }
                    }
                    else
                    {
                        errormessage = "Error : (CheckDuplicate) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error : (CheckDuplicate) Code I";
                    return false;
                }

                return false;

            }
            catch (Exception ex)
            {
                errormessage = "Error :(CheckDuplicate)" + ex.Message;
                return false;
            }

        }

        #endregion

        #region Get IAC Code

        public DataSet GetIACCode()
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;
            try
            {
                //Get FlightPrefix codes...

                ds = da.SelectRecords("sp_GetIACCode");
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
                ds = null;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }
            return (null);
        }
        #endregion 

        
        #region Get CCSF Code

        public DataSet GetCCSFCode()
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = null;
            try
            {
                //Get FlightPrefix codes...

                ds = da.SelectRecords("sp_GetCCSFCode");
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
                ds = null;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }
            return (null);
        }
        #endregion 

        public DataSet GetAgentList(string agent,string origin,string date)
        {
            DataSet ds=null;
            try
            {
                   SQLServer objSQL = new SQLServer(constr);
                   string[] PName = {"agent","origin","date" };
                   SqlDbType[] PType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                   object[] PValue = { agent, origin,date };
                   ds = objSQL.SelectRecords("spGetAgentList", PName, PValue, PType);
            }
            catch(Exception ex)
            {
                ds = null;
            }
            return ds;
        }

        #region Save/Add ULD AWb Details
        public DataSet AddTDSOnCommission(string AgentCode, int SrNo, string FromDate, string ToDate, float TDSOnCommPercent, string UpdatedBy, string UpdatedOn, ref string Result)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;

                string[] Pname = new string[7];
                object[] Pvalue = new object[7];
                SqlDbType[] Ptype = new SqlDbType[7];

                Pname[0] = "AgentCode";
                Pname[1] = "SrNo";
                Pname[2] = "FromDate";
                Pname[3] = "ToDate";
                Pname[4] = "TDSOnCommPercent";
                Pname[5] = "UpdatedBy";
                Pname[6] = "UpdatedOn";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.Int;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.Float;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;

                Pvalue[0] = AgentCode;
                Pvalue[1] = SrNo;
                Pvalue[2] = FromDate;
                Pvalue[3] = ToDate;
                Pvalue[4] = TDSOnCommPercent;
                Pvalue[5] = UpdatedBy;
                Pvalue[6] = UpdatedOn;


                ds = da.SelectRecords("SPSaveTDSOnCommission", Pname, Pvalue, Ptype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Result = ds.Tables[0].Rows[0][0].ToString();
                    return ds;
                }
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Tab AWB Details
        public DataSet getTDSOnCommissionInfo(string AgentCode)
        {
            SQLServer da = new SQLServer(constr);
            DataSet ds = new DataSet();
            try
            {
                string[] Pname = new string[1];
                object[] Pvalue = new object[1];
                SqlDbType[] Ptype = new SqlDbType[1];

                Pname[0] = "AgentCode";
                Ptype[0] = SqlDbType.NVarChar;
                Pvalue[0] = AgentCode;

                ds = da.SelectRecords("SPgetTDSOnCommissionInfo", Pname, Pvalue, Ptype);


                return ds;

            }
            catch (Exception ex)
            {
                return ds;
            }

        }
        #endregion Tab AWB Details

        #region Update TDS On Commission Flag
        public string UpdateTDSOnCommissionFlag(string AgentCode, string UpdatedBy, string UpdatedOn)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = null;

                string[] Pname = new string[3];
                object[] Pvalue = new object[3];
                SqlDbType[] Ptype = new SqlDbType[3];

                Pname[0] = "AgentCode";
                Pname[1] = "UpdatedBy";
                Pname[2] = "UpdatedOn";

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;

                Pvalue[0] = AgentCode;
                Pvalue[1] = UpdatedBy;
                Pvalue[2] = UpdatedOn;

                string result = da.GetStringByProcedure("SPUpdateTDSOnCommissionFlag", Pname, Pvalue, Ptype);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion Update TDS On Commission Flag
    }
}
