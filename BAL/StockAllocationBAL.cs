using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using QID.DataAccess;

namespace BAL
{
    public class StockAllocationBAL
    {
       
        #region Variable
        string constr = "";
        #endregion Variable

        #region Constructor
        public StockAllocationBAL()
        {
           constr = Global.GetConnectionString();
        }
        #endregion Constructor

        #region GetCityCode
        public DataSet GetCityCode()
        {
            DataSet ds = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("Sp_GetCityNameRegion");
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
        #endregion

        # region GetCityCode
        public DataSet GetCityCodeStock(string Station, int IsSuperUser)
        {
            DataSet ds = new DataSet();
            string[] Pname = new string[2];
            object[] Pvalue = new object[2];
            SqlDbType[] Ptype = new SqlDbType[2];
            try
            {

                SQLServer da = new SQLServer(constr);
                Pname[0] = "Station";
                Pname[1] = "IsSuperUser";

                Pvalue[0] = Station;
                Pvalue[1] = IsSuperUser;

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;

                ds = da.SelectRecords("Sp_GetCityNameRegion_StockAllocation", Pname, Pvalue, Ptype);
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
            finally
            {
                Pname = null;
                Pvalue = null;
                Ptype = null;
            }
            return (null);
        }
        #endregion

        #region GetAgentCode
        public DataSet GetAgentCode()
        {
            DataSet ds = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("SP_GetAgentCode");
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
        #endregion
        #region Station
        public DataSet GetStation()
        {
            DataSet ds = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("SP_GetStation");
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
        #endregion


        #region Get Country Code
        public DataSet GetCountryCode()
        {
            DataSet ds = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("SP_GetCountryCode");
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
        #endregion

        #region GetRegionCode
        public DataSet GetRegionCode()
        {
            DataSet ds = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("SP_GetRegionCodeName");
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
        #endregion

        #region GetAWBStock
        public DataSet GetAWBStock(object[] AWB)
        {
            DataSet ds = null;
            string[] ParamNames = null;
            SqlDbType[] ParamTypes = null;
            try
            {
                ParamNames = new string[] { "AType", "ACode", "AWB", "AWBNo" };
                ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("SpGetStockAllocation", ParamNames, AWB, ParamTypes);
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
            finally
            {
                ParamNames = null;
                ParamTypes = null;
                AWB = null;
            }
            return (null);
        }
        #endregion

        # region GetAWBStockHistory
        public DataSet GetAWBStockHistory(object[] AWB)
        {
            DataSet ds = null;
            string[] ParamNames = null;
            SqlDbType[] ParamTypes = null;
            try
            {

                ParamNames = new string[] { "AType", "ACode", "ADateFrom", "ADateTo", "AwbNumber", "Rangefrom", "Rangeto" };
                ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar  };

                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("SpGetStockAllocationHistory", ParamNames, AWB, ParamTypes);
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
            finally
            {
                ParamNames = null;
                ParamTypes = null;
                AWB = null;
            }
            return (null);


        }
        #endregion

        #region AllocateAWBStock
        public string AllocateAWBStock(object[] AcEq)
        {
            string[] ParamNames = null;
            SqlDbType[] ParamTypes = null;
            try
            {
                ParamNames = new string[] { "ALevel", "AFrom", "ATo", "ParName", "ParType", "User", "Time", "AWB", "AWBType","CNType","AWBstockType" };
                ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                
                string res = da.GetStringByProcedure("spDirectStockAllocation", ParamNames, AcEq, ParamTypes);
                
                return res;

            }
            catch (Exception)
            {
                return "Error";
            }
            finally
            {
                ParamNames = null;
                ParamTypes = null;
                AcEq = null;
            }
        }
        #endregion

        #region BlacklistAWBStock
        public string BlacklistAWBStock(object[] AWB)
        {
            string[] ParamNames = null;
            SqlDbType[] ParamTypes = null;
            try
            {
                ParamNames = new string[] { "ALevel", "AFrom", "ATo", "ParName", "ParType", "User", "AWB", "CNType" };
                ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                string res = da.GetStringByProcedure("SpStockAllocationBlacklist", ParamNames,AWB, ParamTypes );
                return res;
            }
            catch (Exception)
            {
                return "Error";
            }
            finally
            {
                ParamNames = null;
                ParamTypes = null;
                AWB = null;
            }
        }
        #endregion

        #region ReturnAWBStock
        public string ReturnAWBStock(object[] AWB)
        {
            string[] ParamNames = null;
            SqlDbType[] ParamTypes = null;
            try
            {
                ParamNames = new string[] { "ALevel", "AFrom", "ATo", "ParName", "ParType", "User" ,"AWB"};
                ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                string res = da.GetStringByProcedure("SpStockAllocationReturn", ParamNames, AWB, ParamTypes);
                return res;
            }
            catch (Exception)
            {
                return "Error";
            }
            finally
            {
                ParamNames = null;
                ParamTypes = null;
                AWB = null;
            }
        }
        #endregion

        #region RevokeAWBStock
        public string RevokeAWBStock(object[] AWB)
        {
            string[] ParamNames = null;
            SqlDbType[] ParamTypes = null;
            try
            {
                ParamNames = new string[] { "ALevel", "AFrom", "ATo", "ParName", "ParType", "User", "AWB", "AWBstockType", "CNType" };
                ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar  };
                SQLServer da = new SQLServer(constr);
                string res = da.GetStringByProcedure("SpStockAllocationRevoke", ParamNames, AWB, ParamTypes);
                return res;
            }
            catch (Exception)
            {
                return "Error";
            }
            finally
            {
                ParamNames = null;
                ParamTypes = null;
                AWB = null;
            }
        }
        #endregion

        #region GetConsecutiveStockAWB
        public string GetConsecutiveStockAWB(int AwbFrom)
        {
            DataSet ds = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("spGetConsecutiveStockAWB", "AWBuser", AwbFrom, SqlDbType.Int);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            return ((ds.Tables[0].Rows[0][0]).ToString());
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
        #endregion

        # region GetUnusedAWB
        public DataSet GetUnusedAWB(object[] AWB)
        {
            DataSet ds = null; 
            string[] ParamNames = null;
            SqlDbType[] ParamTypes = null;
            try
            {
                ParamNames = new string[] { "fromAWB", "toAWB" };
                ParamTypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.Int };

                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("sp_GetUnusedAWB", ParamNames, AWB, ParamTypes);
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
            finally
            {
                ParamNames = null;
                ParamTypes = null;
                AWB = null;
            }
            return (null);
        }
        #endregion

        # region GetCnoteCode
        public DataSet GetCnoteCode()
        {
            DataSet ds = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("SP_GetCnoteCode");
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
        #endregion

        #region GetParentDetails
        public DataSet GetParentDetails(object[] AWB)
        {
            DataSet ds = null;
            string[] ParamNames = null;
            SqlDbType[] ParamTypes = null;
            try
            {
                ParamNames = new string[] { "ACode","AType" };
                ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };

                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("SpGetGetParentDetailsStock", ParamNames, AWB, ParamTypes);
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
            finally
            {
                ParamNames = null;
                ParamTypes = null;
                AWB = null;
            }
            return (null);
        }
        #endregion

        # region Get AWB type
        public DataSet GetAWBtype()
        {
            DataSet ds = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("SP_GetAWBtype");
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
        #endregion

        # region GetFlagFor SubAgent
        public DataSet GetSubAgent()
        {
            DataSet ds = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                ds = da.SelectRecords("GetSubAgent");
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
        #endregion

        #region getfilterdagentcode     
        public DataSet GetFilteredAgentCode(string subAgent)
        {
            string[] paramType = null;
            object[] paramVal = null;
            SqlDbType[] sqlType = null;
            DataSet ds = null;
            try
            {
                SQLServer da = new SQLServer(constr);
                paramType = new string[] { "EnableFiltering", "isControlingLocator", "CA" };
                paramVal = new object[] { true, false, subAgent };
                sqlType = new SqlDbType[] { SqlDbType.Bit, SqlDbType.Bit, SqlDbType.VarChar };

                ds = da.SelectRecords("SP_GetAgentCode_SubAgent", paramType, paramVal, sqlType);
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
            finally
            {
                paramType = null;
                paramVal = null;
                sqlType = null;
            }
            return (null);
        }
        #endregion

        #region Get AWB Stock Type
        public string GetAWBStockType(string AWBFrom)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string awbtype = da.GetStringByProcedure("GetAWBStockType", "AWBFrom", AWBFrom, SqlDbType.Decimal);
                return (awbtype);
            }
            catch (Exception)
            {
            }
            return (null);
        }
        #endregion

        #region Get AWB  Type
        public string GetAWBStockTypeWithPrefix(string AWBPrefix, string AWBFrom)
        {
            string[] paramType = null;
            object[] paramVal = null;
            SqlDbType[] sqlType = null;
            try
            {
                paramType = new string[] { "AWBFrom", "AWBPrefix" };
                paramVal = new object[] { AWBFrom, AWBPrefix };
                sqlType = new SqlDbType[] { SqlDbType.Decimal, SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                string awbtype = da.GetStringByProcedure("GetAWBStockType", paramType, paramVal, sqlType);
                return (awbtype);
            }
            catch (Exception)
            {
            }
            finally
            {
                paramType = null;
                paramVal = null;
                sqlType = null;
            }
            return (null);
        }
        #endregion
        #region
        public string GetAWBStatus(string AWBNo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string awbstatus = da.GetStringByProcedure("SPGetAWBStatus", "AWBNo", AWBNo, SqlDbType.Decimal);
                return (awbstatus);
            }
            catch
            {
            }

            return (null);
        }
        #endregion

        #region Get AWB stck Type
        public string GetAWBStockTypeNew(string AWBFrom)
        {

            try
            {
                SQLServer da = new SQLServer(constr);
                string awbStocktype = da.GetStringByProcedure("GetAWBStockTypeNew", "AWBFrom", AWBFrom, SqlDbType.Decimal);
                return (awbStocktype);
            }
            catch (Exception)
            {

            }
            finally
            {

            }
            return (null);
        }
        #endregion
    
    }
}
