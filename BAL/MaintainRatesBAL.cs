using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;

/*
 
 2012-07-10 vinayak
 
 */


namespace BAL
{
    public class MaintainRatesBAL
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());

        public bool FillDdl(string DDL, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                errormessage = "";
                switch (DDL)
                {
                    case "DdlRateCard":

                        dsResult=db.SelectRecords("SP_GetRateCards");
                        if (dsResult != null)
                        {

                            if (dsResult.Tables.Count >0)
                            {
                                return true;
                            }
                            else
                            {
                                errormessage = "Error : (SP_GetRateCards) table count is zero";
                                return false;
                            }

                        }
                        else
                        {
                            errormessage = "Error : (SP_GetRateCards) ds is null";
                            return false;
                        }

                        break;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
        }


        #region GetRateCard
        public DataSet GetRateLine(string rateLineId)
        {
            //int rateLineId = Convert.ToInt32(rateLineId);
            string[] ColumnNames = new string[1];
            SqlDbType[] DataType = new SqlDbType[1];
            Object[] Values = new object[1];

            ColumnNames[0] = "RateLineId";
            DataType[0] = SqlDbType.VarChar;
            Values[0] = rateLineId;

            DataSet dsRateLine = new DataSet();

            dsRateLine = db.SelectRecords("SP_GetRateLine", ColumnNames, Values, DataType);
            return dsRateLine;
        }
        #endregion GetRateCard

        #region GetDefaultValues
        public bool GetDefaultValues(ref DataSet dsDefaultValues,ref string erromessage)
        {
            try
            {
                dsDefaultValues = db.SelectRecords("SP_GetDefaultValues");

                if (dsDefaultValues != null)
                {
                    if (dsDefaultValues.Tables.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        erromessage = "Error :(GetDefaultValues) CODE-II";
                        return false;
                    }
                }
                else
                {
                    erromessage = "Error :(GetDefaultValues) CODE-I";
                    return false;
                }

                return false;

            }
            catch (Exception ex)
            {
                erromessage = "Error : (GetDefaultValues) " + ex.Message;
                return false;                    
            }
        }
        #endregion GetRateCard

        #region ValidateDuplicate

        public bool CheckDuplicate(object[] values,ref bool IsDuplicate,ref string errormessage)
        {
            try
            {
                string[] param = {"RateCardSrNo","OriginLevel","Origin","DestinationLevel",
                                  "Destination","StartDate","EndDate",                                  
                                  "FlightNumber","FlightCarrier","HandlingCode","AirlineCode","IATACommCode","AgentCode","ShipperCode",
                                  "FNInc","FCInc","HCInc","ACInc","CCInc","ADInc","SCInc","RateLineSrNo","ContrRef","CurrencyId","Status",
                                  "RateBase","AgentCommPercent","MaxDiscountPercent","ServiceTax","IsALLIn","isTact","IsULD","IsHeavy",
                                  "wkDays","wkDaysInc","DepInterval","DepIntervalInc","ProductType","TransitStation","PTInc","TSInc",
                                  "Proration","PROInc","IssueingCarrier","ICInc","SPAMarkup","EquipType","ETInc"
                                };

                SqlDbType[] dbtypes = { SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Int,
                                        SqlDbType.VarChar,SqlDbType.DateTime,SqlDbType.DateTime,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,
                                        SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Int,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Float,SqlDbType.Float,SqlDbType.Float,
                                        SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit
                                      };

                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet dsResult = da.SelectRecords("SP_CheckDuplicateRateLine", param, values, dbtypes);

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


    }
}
