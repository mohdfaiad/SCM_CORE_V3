using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BillingInvoiceMatchingBAL
    {
        #region Variables
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());

        #endregion Variables

        #region Get All Agents
        /// <summary>
        /// Get list of all the agents based on entered value.
        /// </summary>
        /// <returns>Agent code list as Array.</returns>
        public DataSet GetAllAgents()
        {
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("SPGetAgentName");
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
        #endregion Get All Agents

        # region Get AWB Import rate list
        public DataSet GetAWBImportRateList(object[] RateLineInfo, string PayMode)
        {
            try
            {

                string[] ColumnNames = new string[12];
                SqlDbType[] DataType = new SqlDbType[12];
                Object[] Values = new object[12];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("FromDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("Origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("Destination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Status", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("SpotRate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("PaymentMode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("IncludePartial", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ShipmentType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                

                DataSet ds;
                if(PayMode == "PX")
                    ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingV2", ColumnNames, Values, DataType);
                else if (PayMode == "PP")
                    ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingPPV2", ColumnNames, Values, DataType);
                else //if (PayMode == "CC")
                    ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingCCV2", ColumnNames, Values, DataType);

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
        # endregion Get AWB rate list

        
        # region Get AWB Export rate list
        public DataSet GetAWBExportRateList(object[] RateLineInfo, string SPName, string PayMode, bool ExportAll)
        {
            try
            {

                string[] ColumnNames = new string[13];
                SqlDbType[] DataType = new SqlDbType[13];
                Object[] Values = new object[13];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("FromDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("Origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("Destination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Status", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("SpotRate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("PaymentMode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("IncludePartial", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(Convert.ToBoolean(RateLineInfo.GetValue(i)), i);
                i++;
                
                ColumnNames.SetValue("UpdatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                
                i++;
                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                
                DataSet ds;
                if (SPName == "")
                {
                    //DataSet ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingExport", ColumnNames, Values, DataType);
                    if(ExportAll == true)
                        ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingExpAll_V2", ColumnNames, Values, DataType);
                    else if(PayMode == "PX")
                        ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingExp_V2", ColumnNames, Values, DataType);
                    else if (PayMode == "PP")
                        ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingExpPP_V2", ColumnNames, Values, DataType);
                    else //if (PayMode == "CC")
                        ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingExpCC_V2", ColumnNames, Values, DataType);
                }
                else if (SPName == "SP_GetListAWBInvoiceMatchingExp_G8")
                {
                    //DataSet ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingExport", ColumnNames, Values, DataType);
                    if (ExportAll == true)
                        ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingExpAll_G8", ColumnNames, Values, DataType);
                    else if (PayMode == "PX")
                        ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingExp_G8", ColumnNames, Values, DataType);
                    else if (PayMode == "PP")
                        ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingExpPP_G8", ColumnNames, Values, DataType);
                    else //if (PayMode == "CC")
                        ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingExpCC_G8", ColumnNames, Values, DataType);
                }
                else
                {
                    //if (PayMode == "PX") //SP_GetListAWBInvoiceMatchingExpLegWise
                        ds = db.SelectRecords(SPName, ColumnNames, Values, DataType);

                    //else if (PayMode == "PP")
                    //    ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingExpLegWisePP", ColumnNames, Values, DataType);
                    //else //if (PayMode == "CC")
                    //    ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingExpLegWiseCC", ColumnNames, Values, DataType);
                }




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

        # endregion Get AWB Export list


        # region Get AWB commodity rate list
        public DataSet GetAWBCommodityRateList(object[] RateLineInfo, string PayMode)
        {
            try
            {

                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("FromDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                DataSet ds;
                if(PayMode == "PX")
                    ds = db.SelectRecords("SP_GetListAWBInvoiceMatchingCommodity_V2", ColumnNames, Values, DataType);
                else if (PayMode == "PP")
                    ds = db.SelectRecords("SP_GetListWalkInAWBCommodityPP", ColumnNames, Values, DataType);
                else //if (PayMode == "CC")
                    ds = db.SelectRecords("SP_GetListDestAWBCommodityCC", ColumnNames, Values, DataType);

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

        # endregion Get AWB commodity rate list


        # region ConfirmSingleAWBInvMatch
        public string ConfirmSingleAWBInvMatch(object[] RateCardInfo, string PayMode)
        {
            try
            {
                string[] ColumnNames = new string[28];
                SqlDbType[] DataType = new SqlDbType[28];
                Object[] Values = new object[28];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CommodityCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Dimensions", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("GrossWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("ChargableWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("FreightRate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("SpotRate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("OCDC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("OCDA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Total", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Discount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DiscountAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TAD", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Commission", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("CommissionAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("STOnComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("STOnCommAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("OCCommAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("RevisedTotal", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnFreight", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnFreightAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSOnComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnCommAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ServiceTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FinalAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                

                string res = "";
                if(PayMode == "PX")
                    res = db.GetStringByProcedure("SP_ConfirmSingleAWBInvMatchFrontEnd_V2", ColumnNames, Values, DataType);
                else if (PayMode == "PP")
                    res = db.GetStringByProcedure("SP_ConfirmSingleAWBInvMatchFrontEndPP_V2", ColumnNames, Values, DataType);
                else //if (PayMode == "CC")
                    res = db.GetStringByProcedure("SP_ConfirmSingleAWBInvMatchFrontEndCC_V2", ColumnNames, Values, DataType);

                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion ConfirmSingleAWBInvMatch


        # region FinalizeSingleAWBInvMatch
        public string FinalizeSingleAWBInvMatch(object[] RateCardInfo, string PayMode)
        {
            try
            {
                string[] ColumnNames = new string[28];
                SqlDbType[] DataType = new SqlDbType[28];
                Object[] Values = new object[28];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("CommodityCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Dimensions", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("GrossWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("ChargableWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //3
                ColumnNames.SetValue("FreightRate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("SpotRate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("OCDC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("OCDA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Total", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Discount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DiscountAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TAD", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Commission", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("CommissionAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("STOnComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("STOnCommAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("OCCommAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("RevisedTotal", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnFreight", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnFreightAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("TDSOnComm", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("TDSOnCommAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ServiceTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FinalAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                

                string res = "";
                if (PayMode == "PX")
                    res = db.GetStringByProcedure("SP_FinalizeSingleAWBInvMatchFrontEnd_V2", ColumnNames, Values, DataType);
                else if (PayMode == "PP")
                    res = db.GetStringByProcedure("SP_FinalizeSingleAWBInvMatchFrontEndPP_V2", ColumnNames, Values, DataType);
                else //if (PayMode == "CC")
                    res = db.GetStringByProcedure("SP_FinalizeSingleAWBInvMatchFrontEndCC_V2", ColumnNames, Values, DataType);
                
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion FinalizeSingleAWBInvMatch


        # region Finalize selected AWB Invoice Match
        public string FinalizeSelectedAWBInvMatch(object[] RateCardInfo, string PayMode)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn ", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);
                

                string res = "";
                if (PayMode == "PX")
                    res = db.GetStringByProcedure("SP_FinalizeSelectedAWBInvMatch", ColumnNames, Values, DataType);
                else if (PayMode  == "PP")
                    res = db.GetStringByProcedure("SP_FinalizeSelectedAWBInvMatchPP", ColumnNames, Values, DataType);
                else //if (PayMode == "CC")
                    res = db.GetStringByProcedure("SP_FinalizeSelectedAWBInvMatchCC", ColumnNames, Values, DataType);

                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Finalize selected AWB Invoice Match


        # region CalculateAndUpdateSpotRate
        public string CalculateAndUpdateSpotRate(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_CalculateAndUpdateSpotRate", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion CalculateAndUpdateSpotRate


        # region Undo Finalize single AWB Invoice Match
        public string UndoFinalizeSingleAWBInvMatch(object[] RateCardInfo, string PayMode)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = "";
                if (PayMode == "PX")
                    res = db.GetStringByProcedure("SP_UndoFinalizeSingleAWBInvMatch", ColumnNames, Values, DataType);
                else if (PayMode == "PP")
                    res = db.GetStringByProcedure("SP_UndoFinalizeSingleAWBInvMatchPP", ColumnNames, Values, DataType);
                else //if (PayMode == "CC")
                    res = db.GetStringByProcedure("SP_UndoFinalizeSingleAWBInvMatchCC", ColumnNames, Values, DataType);
                
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Undo Finalize single AWB Invoice Match


        # region Undo Finalize Selected AWB Invoice Match
        public string UndoFinalizeSelectedAWBInvMatch(object[] RateCardInfo, string PayMode)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);


                string res = "";
                if (PayMode == "PX")
                    res = db.GetStringByProcedure("SP_UndoFinalizeSelectedAWBInvMatch", ColumnNames, Values, DataType);
                else if (PayMode == "PP")
                    res = db.GetStringByProcedure("SP_UndoFinalizeSelectedAWBInvMatchPP", ColumnNames, Values, DataType);
                else //if (PayMode == "CC")
                    res = db.GetStringByProcedure("SP_UndoFinalizeSelectedAWBInvMatchCC", ColumnNames, Values, DataType);
              
                return res;

                //if (!db.ExecuteProcedure("SP_InsertRateCard", ColumnNames, DataType, Values))
                //    return (-1);
                //else
                //{
                //    return (0);
                //}


            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Undo Finalize Selected AWB Invoice Match


        # region Generate Bunch Invoice Numbers Invoice Match
        public string GenerateBunchInvoiceNumInvMatch(object[] RateCardInfo, string PayMode)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);


                string res = "";
                if (PayMode == "PX")
                    res = db.GetStringByProcedure("SP_GenerateBunchInvoiceNumInvMatchNew", ColumnNames, Values, DataType);
                else if (PayMode == "PP")
                    res = db.GetStringByProcedure("SP_GenerateBunchInvoiceNumWalkInAgentPP", ColumnNames, Values, DataType);
                else //if (PayMode == "CC")
                    res = db.GetStringByProcedure("SP_GenerateBunchInvoiceNumDestAgentCC", ColumnNames, Values, DataType);

                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Generate Bunch Invoice Numbers Invoice Match

        # region Generate Bunch Supplementary Invoice Numbers Invoice Match
        public string GenerateBunchSupplementaryInvoiceNumInvMatch(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_GenerateBunchSupplementaryInvoiceNumInvMatch", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Generate Bunch Invoice Numbers Invoice Match

        # region Generate Bunch Invoice Numbers Invoice Match
        public string GenerateBunchProformaInvoiceNumInvMatch(object[] RateCardInfo, string PayMode)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);


                string res = "";
                if (PayMode == "PX")
                    res = db.GetStringByProcedure("SP_GenerateBunchProInvoiceNumInvMatch", ColumnNames, Values, DataType);
                else if (PayMode == "PP" || PayMode == "CC")
                    res = "Not Applicable";

                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Generate Bunch Proforma Invoice Numbers Invoice Match


        # region Get Invoice Numbers of AWB Invoice Match
        public DataSet GetInvoiceNumInvMatch(object[] RateLineInfo, string PayMode)
        {
            try
            {

                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);


                DataSet ds = null;
                if(PayMode =="PX")
                    ds = db.SelectRecords("SP_GetInvoiceNumInvMatch", ColumnNames, Values, DataType);
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
        # endregion Get Invoice Numbers of AWB Invoice Match

        # region Get Proforma Invoice Numbers of AWB Invoice Match
        public DataSet GetProformaInvoiceNumInvMatch(object[] RateLineInfo, string PayMode)
        {
            try
            {

                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NText, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);

                i++;
                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);
                
                DataSet ds = null;
                if (PayMode == "PX")
                    ds = db.SelectRecords("SP_GetProformaInvoiceNumInvMatch", ColumnNames, Values, DataType);
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
        # endregion Get Proforma Invoice Numbers of AWB Invoice Match


        # region UpdateBillingInvoiceMatchSummary
        public string UpdateBillingInvoiceMatchSummary(object[] RateCardInfo, string PayMode)
        {
            try
            {
                string[] ColumnNames = new string[4];
                SqlDbType[] DataType = new SqlDbType[4];
                Object[] Values = new object[4];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UserName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = "";
                if (PayMode == "PX")
                    res = db.GetStringByProcedure("SP_UpdateBillingInvoiceMatchSummary", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion UpdateBillingInvoiceMatchSummary


        # region UpdateBillingProformaInvoiceMatchSummary
        public string UpdateBillingProformaInvoiceMatchSummary(object[] RateCardInfo, string PayMode)
        {
            try
            {
                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.DateTime, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = "";
                if(PayMode == "PX")
                    res = db.GetStringByProcedure("SP_UpdateBillingProformaInvoiceMatchSummary", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion UpdateBillingInvoiceMatchSummary


        # region UpdateBillingInvoiceAmtFromCCADCM
        public string UpdateBillingInvoiceAmtFromCCADCM(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_UpdateBillingInvoiceAmtFromCCADCM", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion UpdateBillingInvoiceAmtFromCCADCM


        # region UpdateBillingProformaInvoiceAmtFromCCADCM
        public string UpdateBillingProformaInvoiceAmtFromCCADCM(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("InvoiceNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_UpdateBillingProformaInvoiceAmtFromCCADCM", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion UpdateBillingProformaInvoiceAmtFromCCADCM



        # region Add Agent file data for Invoice Matching
        public string AddAgentFileForInvoiceMatch(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[19];
                SqlDbType[] DataType = new SqlDbType[19];
                Object[] Values = new object[19];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBDate", i);
                DataType.SetValue(SqlDbType.Date, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Sector", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Pieces", i);
                DataType.SetValue(SqlDbType.Int, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("GrossWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("ChargableWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Rate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Freight", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Surcharge", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("OCDC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("AWBDO", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("OCDA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("SectorCharge", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Total", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ServiceTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("AWBTotalAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Remarks", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_InsertAgentInvoiceMatchFile", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion Add Agent file data for Invoice Matching


        # region Insert Agent file data for Invoice Matching
        public string InsertAgentFileForInvoiceMatching(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[19];
                SqlDbType[] DataType = new SqlDbType[19];
                Object[] Values = new object[19];
                int i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AWBDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FlightNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FlightDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Origin", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Destination", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ChargableWt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Rate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Freight", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FSC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("AWB", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("HandlingCharges", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("SectorCharges", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("OtherOCDC", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("OCDA", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("Total", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ServiceTax", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;


                ColumnNames.SetValue("AWBTotalAmt", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_InsertAgentFileForInvoiceMatching", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }

        }
        # endregion Insert Agent file data for Invoice Matching


        # region Get All pending Invoices of Agent
        public DataSet GetAllPendingInvOfAgent(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);


                DataSet ds = db.SelectRecords("SP_GetAllPendingInvOfAgentInvMatch", ColumnNames, Values, DataType);
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

        # endregion Get All pending Invoices of Agent


        # region Match All Invoices Of Agent
        public string MatchAllInvoicesOfAgent(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AWBTotalAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_MatchAllInvOfAgentInvMatch", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Match All Invoices Of Agent


        # region Match Invoiced AWB With Agent Invoice
        public string MatchInvoicedAWBWithAgentInvoice(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                int i = 0;

                i = 0;
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("AWBTotalAmount", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Result", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue("", i);

                string res = db.GetStringByProcedure("SP_MatchInvoicedAWBWithAgentInvoice", ColumnNames, Values, DataType);
                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion Match Invoiced AWB With Agent Invoice


        # region Get Invoiced AWB For Invoice Matching
        public DataSet GetInvoicedAWBForInvoiceMatching(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("AgentCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateLineInfo.GetValue(i), i);


                DataSet ds = db.SelectRecords("SP_GetInvoicedAWBForInvoiceMatching", ColumnNames, Values, DataType);
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

        # endregion Get Invoiced AWB For Invoice Matching


        #region get OCDC OCDA details to edit breakup
        public bool GetBillingAWBDetails(string AWBNumber, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                dsResult = da.SelectRecords("SP_GetBillingAWBDetails", "AWBNumber", AWBNumber, SqlDbType.VarChar);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error :(GetAWBDetails) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAWBDetails) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errormessage = "Error :(GetAWBDetails)" + ex.Message;
                return false;
            }
        }
        #endregion get OCDC OCDA details to edit breakup


        #region Save OCDC OCDA changes
        public bool SaveAWBRatesChanges(string type, object[] values)
        {
            try
            {
                if (type == "Freight")
                {
                    string[] param = { "AWBNumber", "RateLineSrNo", "Type", "DiscountPercent", 
                                       "CommPercent", "TaxPercent", "Discount", "Commission", "Tax","Charge","CommCode"};
                    SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Float, 
                                            SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float,SqlDbType.Float,SqlDbType.VarChar };

                    SQLServer db = new SQLServer(Global.GetConnectionString());
                    return db.ExecuteProcedure("SP_SaveAWBFrRatesDetails", param, dbtypes, values);
                }
                else
                {
                    string[] param = { "AWBNumber", "ChargeHeadCode", "ChargeType", "DiscountPercent", 
                                       "CommPercent", "TaxPercent", "Discount", "Comission", "Tax","Charge","CommCode"};
                    SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Float, 
                                            SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float,SqlDbType.Float,SqlDbType.VarChar};

                    SQLServer db = new SQLServer(Global.GetConnectionString());
                    return db.ExecuteProcedure("SP_SaveBillingOCRatesDetails", param, dbtypes, values);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion


        #region get AWB Flight details to edit
        public bool GetAWBFlightDetails(string AWBNumber, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                dsResult = da.SelectRecords("SP_GetAWBFlightDetails", "AWBNumber", AWBNumber, SqlDbType.VarChar);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        errormessage = "Error :(GetAWBFlightDetails) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error :(GetAWBFlightDetails) Code I";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errormessage = "Error :(GetAWBFlightDetails)" + ex.Message;
                return false;
            }
        }
        #endregion get AWB Flight details to edit


        #region Save AWB Flight details
        public bool SaveAWBFlightDetails(object[] values)
        {
            try
            {
                string[] param = { "AWBNumber", "FlightNumber", "NewFlightNumber", "FlightDate", "ChargedWeight", 
                                       "RatePerKg", "Freight","CreatedBy","CreatedOn"};
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Float, 
                                            SqlDbType.Float, SqlDbType.Float, SqlDbType.VarChar, SqlDbType.VarChar };

                SQLServer db = new SQLServer(Global.GetConnectionString());
                return db.ExecuteProcedure("SP_SaveAWBFlightDetailsNew", param, dbtypes, values);

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        # region get Configured Billing Export SP
        public string getConfiguredBillingExpSP()
        {
            try
            {
                return db.GetString("select top 1 value from tblConfiguration where Parameter = 'BillingExportSP'");
            }
            catch (Exception ex)
            {
                return "error";

            }
        }
        # endregion get Configured Billing Export SP

        # region GetPayModeOfAWB
        public string GetPayModeOfAWB(string AWBNumber)
        {
            try
            {
                string[] ColumnNames = new string[1];
                SqlDbType[] DataType = new SqlDbType[1];
                Object[] Values = new object[1];
                int i = 0;

                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AWBNumber, i);


                string res = "";
                res = db.GetStringByProcedure("SP_GetAWBPayMode", ColumnNames, Values, DataType);

                return res;

            }

            catch (Exception ex)
            {
                return "error";
            }
        }
        # endregion GetPayModeOfAWB

    }
}

