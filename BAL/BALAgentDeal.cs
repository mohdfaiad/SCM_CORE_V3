﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using System.Configuration;
namespace BAL
{
    public class BALAgentDeal
    {


        SQLServer db = new SQLServer(Global.GetConnectionString());

        string constr = "";
        DataSet res;
        DataSet result;
        public BALAgentDeal()
        {
            constr = Global.GetConnectionString();

        }

        #region SaveAgentDeal
        public DataSet SaveAgentDeal(string AgentCode, string OriginType, string Origin, string DestinationType, string Destination, string FlightNo, string Commodity, string Rate,
                                  string RateSlab, string SpotRateTonnageIsInclude, string Commissionable, string updatedBy, DateTime ApplicableFrom, DateTime ApplicableTo, DateTime UpdatedOn,
                                   double Tonnage, double KickbackAmount, double Threshold, bool M, bool N, bool Q1, bool Q2)
        {
            try
            {

                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[22];
                object[] Pvalue = new object[22];
                SqlDbType[] Ptype = new SqlDbType[22];

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.DateTime;
                Ptype[13] = SqlDbType.DateTime;
                Ptype[14] = SqlDbType.DateTime;
                Ptype[15] = SqlDbType.Float;
                Ptype[16] = SqlDbType.Float;
                Ptype[17] = SqlDbType.Float;
                Ptype[18] = SqlDbType.Bit;
                Ptype[19] = SqlDbType.Bit;
                Ptype[20] = SqlDbType.Bit;
                Ptype[21] = SqlDbType.Bit;


                Pname[0] = "AgentCode";
                Pname[1] = "OriginType";
                Pname[2] = "Origin";
                Pname[3] = "DestinationType";
                Pname[4] = "Destination";
                Pname[5] = "FlightNo";
                Pname[6] = "Commodity";
                Pname[7] = "Rate";
                Pname[8] = "RateSlab";
                Pname[9] = "SpotRateTonnageIsInclude";
                Pname[10] = "Commissionable";
                Pname[11] = "updatedBy";
                Pname[12] = "ApplicableFrom";
                Pname[13] = "ApplicableTo";
                Pname[14] = "UpdatedOn";
                Pname[15] = "Tonnage";
                Pname[16] = "KickbackAmount";
                Pname[17] = "Threshold";
                Pname[18] = "M";
                Pname[19] = "N";
                Pname[20] = "Q1";
                Pname[21] = "Q2";

                Pvalue[0] = AgentCode;
                Pvalue[1] = OriginType;
                Pvalue[2] = Origin;
                Pvalue[3] = DestinationType;
                Pvalue[4] = Destination;
                Pvalue[5] = FlightNo;
                Pvalue[6] = Commodity;
                Pvalue[7] = Rate;
                Pvalue[8] = RateSlab;
                Pvalue[9] = SpotRateTonnageIsInclude;
                Pvalue[10] = Commissionable;
                Pvalue[11] = updatedBy;
                Pvalue[12] = ApplicableFrom;
                Pvalue[13] = ApplicableTo;
                Pvalue[14] = UpdatedOn;
                Pvalue[15] = Tonnage;
                Pvalue[16] = KickbackAmount;
                Pvalue[17] = Threshold;
                Pvalue[18] = M;
                Pvalue[19] = N;
                Pvalue[20] = Q1;
                Pvalue[21] = Q2;

                res = da.SelectRecords("SpSaveAgentDeal", Pname, Pvalue, Ptype);

                return res;

            }
            catch (Exception ex)
            {
                return res;
            }
            return res;
        }
        #endregion SaveAgentDeal


        #region SaveAgentDealNew
        public DataSet SaveAgentDealNew(string AgentCode, string OriginType, string Origin, string DestinationType, string Destination, string FlightNo, string Commodity, string Rate,
                                  string RateSlab, string SpotRateTonnageIsInclude, string Commissionable, string updatedBy, DateTime ApplicableFrom, DateTime ApplicableTo, DateTime UpdatedOn,
                                   double Tonnage, double KickbackAmount, double Threshold, bool M, bool N, bool Q1, bool Q2, string dealType,bool IsLocal,double FlatAmount,string Currency)
        {
            try
            {

                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[26];
                object[] Pvalue = new object[26];
                SqlDbType[] Ptype = new SqlDbType[26];

                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.VarChar;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.VarChar;
                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.DateTime;
                Ptype[13] = SqlDbType.DateTime;
                Ptype[14] = SqlDbType.DateTime;
                Ptype[15] = SqlDbType.Float;
                Ptype[16] = SqlDbType.Float;
                Ptype[17] = SqlDbType.Float;
                Ptype[18] = SqlDbType.Bit;
                Ptype[19] = SqlDbType.Bit;
                Ptype[20] = SqlDbType.Bit;
                Ptype[21] = SqlDbType.Bit;
                Ptype[22] = SqlDbType.VarChar;
                Ptype[23] = SqlDbType.Bit;
                Ptype[24] = SqlDbType.Float;
                Ptype[25] = SqlDbType.VarChar;


                Pname[0] = "AgentCode";
                Pname[1] = "OriginType";
                Pname[2] = "Origin";
                Pname[3] = "DestinationType";
                Pname[4] = "Destination";
                Pname[5] = "FlightNo";
                Pname[6] = "Commodity";
                Pname[7] = "Rate";
                Pname[8] = "RateSlab";
                Pname[9] = "SpotRateTonnageIsInclude";
                Pname[10] = "Commissionable";
                Pname[11] = "updatedBy";
                Pname[12] = "ApplicableFrom";
                Pname[13] = "ApplicableTo";
                Pname[14] = "UpdatedOn";
                Pname[15] = "Tonnage";
                Pname[16] = "KickbackAmount";
                Pname[17] = "Threshold";
                Pname[18] = "M";
                Pname[19] = "N";
                Pname[20] = "Q1";
                Pname[21] = "Q2";
                Pname[22] = "DealType";
                Pname[23] = "IsLocal";
                Pname[24] = "FlatAmount";
                Pname[25] = "Currency";

                Pvalue[0] = AgentCode;
                Pvalue[1] = OriginType;
                Pvalue[2] = Origin;
                Pvalue[3] = DestinationType;
                Pvalue[4] = Destination;
                Pvalue[5] = FlightNo;
                Pvalue[6] = Commodity;
                Pvalue[7] = Rate;
                Pvalue[8] = RateSlab;
                Pvalue[9] = SpotRateTonnageIsInclude;
                Pvalue[10] = Commissionable;
                Pvalue[11] = updatedBy;
                Pvalue[12] = ApplicableFrom;
                Pvalue[13] = ApplicableTo;
                Pvalue[14] = UpdatedOn;
                Pvalue[15] = Tonnage;
                Pvalue[16] = KickbackAmount;
                Pvalue[17] = Threshold;
                Pvalue[18] = M;
                Pvalue[19] = N;
                Pvalue[20] = Q1;
                Pvalue[21] = Q2;
                Pvalue[22] = dealType;
                Pvalue[23] = IsLocal;
                Pvalue[24] = FlatAmount;
                Pvalue[25] = Currency;

                res = da.SelectRecords("SpSaveAgentDealNew", Pname, Pvalue, Ptype);

                return res;

            }
            catch (Exception ex)
            {
                return res;
            }
            return res;
        }
        #endregion SaveAgentDealNew

        # region SaveAgentDealSlab
        public string SaveAgentDealSlab(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[7];
                SqlDbType[] DataType = new SqlDbType[7];
                Object[] Values = new object[7];
                int i = 0;

                i = 0;

                ColumnNames.SetValue("DealId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Tonnage", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Rate", i);
                DataType.SetValue(SqlDbType.Float, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("DealType", i);
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
                i++;


                string res = db.GetStringByProcedure("spSaveAgentDealSlab", ColumnNames, Values, DataType);
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
        # endregion ConfirmSingleAWBInvMatch

        #region Save Del Exceptions
        public bool SaveDealExceptions(string DealId, string FlightNumber, bool FNInc, string FlightCarrier, bool FCInc, string HandlingCode, bool HCInc, string AirlineCode, bool ACInc, string IATACommCode, bool CCInc, string AgentCode, bool ADInc, string ShipperCode, bool SCInc, string ParamSource, bool SourceInc, string ParamDest, bool DestInc, string UpdatedBy, DateTime UpdatedOn, string IsHeavy, bool HeavyInc, string ProductType, bool ProductTypeInc, string MinMetro, bool MinMetroInc, string MinNonMetro, bool MinNonMetroInc,string MinMetroValue,string MinNonMetroValue,bool IsPrime)
        {
            bool res = false;
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] Pname = new string[33];
                object[] Pvalue = new object[33];
                SqlDbType[] Ptype = new SqlDbType[33];

                Ptype[0] = SqlDbType.VarChar;

                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.Bit;

                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.Bit;

                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.Bit;

                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.Bit;

                Ptype[9] = SqlDbType.VarChar;
                Ptype[10] = SqlDbType.Bit;

                Ptype[11] = SqlDbType.VarChar;
                Ptype[12] = SqlDbType.Bit;

                Ptype[13] = SqlDbType.VarChar;
                Ptype[14] = SqlDbType.Bit;

                Ptype[15] = SqlDbType.VarChar;
                Ptype[16] = SqlDbType.Bit;

                Ptype[17] = SqlDbType.VarChar;
                Ptype[18] = SqlDbType.Bit;

                Ptype[19] = SqlDbType.VarChar;
                Ptype[20] = SqlDbType.DateTime;

                Ptype[21] = SqlDbType.VarChar;
                Ptype[22] = SqlDbType.Bit;

                Ptype[23] = SqlDbType.VarChar;
                Ptype[24] = SqlDbType.Bit;

                Ptype[25] = SqlDbType.VarChar;
                Ptype[26] = SqlDbType.Bit;

                Ptype[27] = SqlDbType.VarChar;
                Ptype[28] = SqlDbType.Bit;

                Ptype[29] = SqlDbType.VarChar;
                Ptype[30] = SqlDbType.VarChar;

                Ptype[31] = SqlDbType.VarChar;
                Ptype[32] = SqlDbType.Bit;

                Pname[0] = "DealId";
                Pname[1] = "FlightNumber";
                Pname[2] = "FNInc";
                Pname[3] = "FlightCarrier";
                Pname[4] = "FCInc";
                Pname[5] = "HandlingCode";
                Pname[6] = "HCInc";
                Pname[7] = "AirlineCode";
                Pname[8] = "ACInc";
                Pname[9] = "IATACommCode";
                Pname[10] = "CCInc";
                Pname[11] = "AgentCode";
                Pname[12] = "ADInc";
                Pname[13] = "ShipperCode";
                Pname[14] = "SCInc";
                Pname[15] = "ParamSource";
                Pname[16] = "SourceInc";
                Pname[17] = "ParamDest";
                Pname[18] = "DestInc";
                Pname[19] = "UpdatedBy";
                Pname[20] = "UpdatedOn";
                Pname[21] = "IsHeavy";
                Pname[22] = "HeavyInc";
                Pname[23] = "ProductType";
                Pname[24] = "ProductTypeInc";
                Pname[25] = "MinMetro";
                Pname[26] = "MinMetroInc";
                Pname[27] = "MinNonMetro";
                Pname[28] = "MinNonMetroInc";
                Pname[29] = "MinMetroValue";
                Pname[30] = "MinNonMetroValue";
                Pname[31] = "PrimeFlight";
                Pname[32] = "PrimeFlightInc";

                Pvalue[0] = DealId;
                Pvalue[1] = FlightNumber;
                Pvalue[2] = FNInc;
                Pvalue[3] = FlightCarrier;
                Pvalue[4] = FCInc;
                Pvalue[5] = HandlingCode;
                Pvalue[6] = HCInc;
                Pvalue[7] = AirlineCode;
                Pvalue[8] = ACInc;
                Pvalue[9] = IATACommCode;
                Pvalue[10] = CCInc;
                Pvalue[11] = AgentCode;
                Pvalue[12] = ADInc;
                Pvalue[13] = ShipperCode;
                Pvalue[14] = SCInc;
                Pvalue[15] = ParamSource;
                Pvalue[16] = SourceInc;
                Pvalue[17] = ParamDest;
                Pvalue[18] = DestInc;
                Pvalue[19] = UpdatedBy;
                Pvalue[20] = UpdatedOn;
                Pvalue[21] = IsHeavy;
                Pvalue[22] = HeavyInc;
                Pvalue[23] = ProductType;
                Pvalue[24] = ProductTypeInc;
                Pvalue[25] = MinMetro;
                Pvalue[26] = MinMetroInc;
                Pvalue[27] = MinNonMetro;
                Pvalue[28] = MinNonMetroInc;
                Pvalue[29] = MinMetroValue;
                Pvalue[30] = MinNonMetroValue;
                Pvalue[31] = "PrimeFlight";
                Pvalue[32] = IsPrime;

                res = da.ExecuteProcedure("sp_InsertDealExceptionsNew", Pname, Ptype, Pvalue);

                return res;
            }
            catch (Exception ex)
            {
                return res;
            }
        }
        #endregion


        #region Update
        public bool UpdateStatusApproval(string SerialNumber, string Rate)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] paramname = new string[3];
                paramname[0] = "SerialNumber";
                paramname[1] = "Status";
                paramname[2] = "Rate";

                object[] paramvalue = new object[3];
                paramvalue[0] = SerialNumber;
                paramvalue[1] = "Approved";
                paramvalue[2] = Rate;

                SqlDbType[] paramtype = new SqlDbType[3];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;

                bool res = da.InsertData("SpUpdateApproveStatusDeal", paramname, paramtype, paramvalue);
                if (res == true)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
        #endregion Update
        #region List
        public DataSet LIstDeals(string origin, string destination, string FlightNo, string AgentCode, string ApplicableFrom, string ApplicableTo, string DealId)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] paramname = new string[7];
                paramname[0] = "Origin";
                paramname[1] = "destination";
                paramname[2] = "FlightNo";
                paramname[3] = "AgentCode";
                paramname[4] = "ApplicableFrom";
                paramname[5] = "ApplicableTo";
                paramname[6] = "DealId";

                //           @Origin varchar(5),
                //@destinaton varchar(5),
                //@FlightNo varchar(20),
                //@AgentCode varchar(10),
                //@ApplicableFrom varchar(20),
                //@ApplicableTo varchar(20)


                object[] paramvalue = new object[7];
                paramvalue[0] = origin;
                paramvalue[1] = destination;
                paramvalue[2] = FlightNo;
                paramvalue[3] = AgentCode;
                paramvalue[4] = ApplicableFrom;
                paramvalue[5] = ApplicableTo;
                paramvalue[6] = DealId;


                SqlDbType[] paramtype = new SqlDbType[7];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.VarChar;
                paramtype[6] = SqlDbType.VarChar;


                result = da.SelectRecords("spGetDeals", paramname, paramvalue, paramtype);
                if (result != null)
                {
                    if (result.Tables[0].Rows.Count > 0)
                    {
                        return result;
                    }
                }

            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }
        #endregion List


        # region Get Available Deal List
        public DataSet GetAvailableDealList(object[] RateLineInfo)
        {
            try
            {

                string[] ColumnNames = new string[3];
                SqlDbType[] DataType = new SqlDbType[3];
                Object[] Values = new object[3];
                int i = 0;

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

                DataSet ds = db.SelectRecords("sp_GetDealDetailsWithParams", ColumnNames, Values, DataType);
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

        # region Apply agent deals
        public DataSet ApplyAgentDeals(object[] RateCardInfo)
        {
            try
            {
                string[] ColumnNames = new string[6];
                SqlDbType[] DataType = new SqlDbType[6];
                Object[] Values = new object[6];
                int i = 0;

                ColumnNames.SetValue("DealId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("FromDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("ToDate", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedBy", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UpdatedOn", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("Selection", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(RateCardInfo.GetValue(i), i);

                DataSet ds = db.SelectRecords("SP_ApplyDealsNew2", ColumnNames, Values, DataType);
                return ds;

            }

            catch (Exception ex)
            {
                return null;
            }
        }
        # endregion Apply agent deals

    }

}
