using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;
using System.Configuration;

namespace BAL
{
    public class UploadMastersBAL
    {
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
    #region Rates
        public bool  InsertAllTypesRateLines(string RateCard, string Origin, string Destination, string commcode, string ProductType, string flightno,
            string AgentCode, string Currency, decimal Minimum, decimal Normal, decimal Fourtyfive, decimal Hundred, decimal TwoFifty, decimal ThreeHundred,
            decimal FiveHundred, decimal Thousand, decimal TwoThosand, decimal ThreeTousand, decimal FiveThousand, DateTime FromDt, DateTime ToDt,
            decimal AgentComm, decimal Discount, decimal Tax, bool AllInRate, bool IsTact, bool IsHeavy, string IssueCarrier, string FlightCarrier,
            string SHC, string DayOfWeek, string DepIntFrom, string DepIntTo, ref string errormessage,string ShipperCode, decimal Fifty)
        {
            try
            {
                errormessage = "";

                string[] param = {"RateCard", "Origin", "Destination", "CommCode", "ProductType", "FlightNumber", "AgentCode",
                                     "Currency", "FromDate", "ToDate", "AgentComm", "Discount", "Tax", "AllInRate", "IsTact", "IsHeavy",
                                     "IssueCarrier", "FlightCarrier", "SHC", "DayOfWeek", "DepIntFrom",	"DepIntTo","ShipperCode"
                                };

                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,
                                        SqlDbType.VarChar,SqlDbType.DateTime,SqlDbType.DateTime,SqlDbType.Decimal,SqlDbType.Decimal,SqlDbType.Decimal,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar
                                      };
                object[] values = { RateCard, Origin.Trim(), Destination.Trim(), commcode.Trim(), ProductType.Trim(), flightno.Trim(), AgentCode.Trim(),
                                      Currency.Trim(), FromDt, ToDt, AgentComm, Discount,Tax,AllInRate,IsTact,IsHeavy,
                                      IssueCarrier.Trim(),FlightCarrier.Trim(),SHC.Trim(),DayOfWeek.Trim(),DepIntFrom.Trim(),DepIntTo.Trim(),ShipperCode
                                  };

                SQLServer da = new SQLServer(constr);
                DataSet dsResult = da.SelectRecords("SP_InsertAllTypesRateLines", param, values, dbtypes);

                if (dsResult == null || dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                {
                    // error
                    errormessage = "Error while inserting rateline header";
                    return false;
                }
                else
                {
                    // Slabs
                    int id;

                    id = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());

                    //delete rate line slabs before insert or update for RateLineSrNo
                    param = new string[] { "RateLineSrNo" };
                    dbtypes = new SqlDbType[] { SqlDbType.Int };
                    values = new object[] { id };

                    if (!da.ExecuteProcedure("SP_DeleteRateLineSlabs", param, dbtypes, values))
                    {
                        errormessage = "Error while deleting previous slabs(" + id + ")";
                        return false;
                    }

                    param = new string[] { "RateLineSrNo", "SlabName", "Weight", "Charge" };
                    dbtypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.Float };
                    values = new object[] { id, "M", Convert.ToDouble("0"), Convert.ToDouble("" + Minimum) };

                    if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                    {
                        errormessage = "Error while inserting slab M(" + id + ")";
                        return false;
                    }

                    if (Normal > 0)
                    {
                        values = new object[] { id, "N", Convert.ToDouble("0"), Convert.ToDouble("" + Normal) };

                        if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while inserting slab N(" + id + ")";
                            return false;
                        }
                    }

                    if (Convert.ToDouble("" + Fourtyfive) > 0)
                    {
                        values = new object[] { id, "Q", Convert.ToDouble("45"), Convert.ToDouble("" + Fourtyfive) };

                        if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while inserting slab Q(" + id + ")";
                            return false;
                        }
                    }

                    if (Convert.ToDouble("" + Fifty) > 0)
                    {
                        values = new object[] { id, "Q", Convert.ToDouble("50"), Convert.ToDouble("" + Fifty) };

                        if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while inserting slab Q(" + id + ")";
                            return false;
                        }
                    }

                    if (Convert.ToDouble("" + Hundred) > 0)
                    {
                        values = new object[] { id, "Q", Convert.ToDouble("100"), Convert.ToDouble("" + Hundred) };

                        if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while inserting slab Q(" + id + ")";
                            return false;
                        }
                    }

                    if (Convert.ToDouble("" + TwoFifty) > 0)
                    {
                        values = new object[] { id, "Q", Convert.ToDouble("250"), Convert.ToDouble("" + TwoFifty) };

                        if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while inserting slab Q(" + id + ")";
                            return false;
                        }
                    }

                    if (Convert.ToDouble("" + ThreeHundred) > 0)
                    {
                        values = new object[] { id, "Q", Convert.ToDouble("300"), Convert.ToDouble("" + ThreeHundred) };

                        if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while inserting slab Q(" + id + ")";
                            return false;
                        }
                    }

                    if (Convert.ToDouble("" + FiveHundred) > 0)
                    {
                        values = new object[] { id, "Q", Convert.ToDouble("500"), Convert.ToDouble("" + FiveHundred) };

                        if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while inserting slab Q(" + id + ")";
                            return false;
                        }
                    }

                    if (Convert.ToDouble("" + Thousand) > 0)
                    {
                        values = new object[] { id, "Q", Convert.ToDouble("1000"), Convert.ToDouble("" + Thousand) };

                        if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while inserting slab Q(" + id + ")";
                            return false;
                        }
                    }

                    if (Convert.ToDouble("" + TwoThosand) > 0)
                    {
                        values = new object[] { id, "Q", Convert.ToDouble("2000"), Convert.ToDouble("" + TwoThosand) };

                        if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while inserting slab Q(" + id + ")";
                            return false;
                        }
                    }

                    if (Convert.ToDouble("" + ThreeTousand) > 0)
                    {
                        values = new object[] { id, "Q", Convert.ToDouble("3000"), Convert.ToDouble("" + ThreeTousand) };

                        if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while inserting slab Q(" + id + ")";
                            return false;
                        }
                    }

                    if (Convert.ToDouble("" + FiveThousand) > 0)
                    {
                        values = new object[] { id, "Q", Convert.ToDouble("5000"), Convert.ToDouble("" + FiveThousand) };

                        if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", param, dbtypes, values))
                        {
                            errormessage = "Error while inserting slab Q(" + id + ")";
                            return false;
                        }
                    }

                    return true;
                }

            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }
        # endregion Rates
        //new rates excel


        # region InsertAllTypesRateLinesNew

        public bool InsertAllTypesRateLinesNew(string RateCard, string Origin, string Destination, string commcode, string ProductType, string flightno,
            string AgentCode, string Currency, DateTime FromDt, DateTime ToDt, decimal AgentComm, decimal Discount, decimal Tax,
             bool AllInRate, bool IsTact, bool IsHeavy, string IssueCarrier, string FlightCarrier, string SHC,
            string DayOfWeek, string DepIntFrom, string DepIntTo, string GLCode, string SPAMarkup, string EquipmentType, string TransitStation,
            string ShipperCode, string RateBase, ref string errormessage, ref int RateLineID, string updateRateLineID)
       {
           


           try
               {
                errormessage = "";

                string[] param = {"RateCard", "Origin", "Destination", "CommCode", "ProductType", "FlightNumber", "AgentCode",
                                     "Currency", "FromDate", "ToDate", "AgentComm", "Discount", "Tax", "AllInRate", "IsTact", "IsHeavy",
                                     "IssueCarrier", "FlightCarrier", "SHC", "DayOfWeek", "DepIntFrom",	"DepIntTo","ShipperCode","RateBase","GLCode","SPAMarkup"
                                     ,"EquipmentType","TransitStation","updateRateLineID"
                                };

                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,
                                        SqlDbType.VarChar,SqlDbType.DateTime,SqlDbType.DateTime,SqlDbType.Decimal,SqlDbType.Decimal,SqlDbType.Decimal,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar
                                      };
                object[] values = {RateCard, Origin.Trim(), Destination.Trim(), commcode.Trim(), ProductType.Trim(), flightno.Trim(), AgentCode.Trim(),
                                      Currency.Trim(), FromDt, ToDt, AgentComm, Discount,Tax,AllInRate,IsTact,IsHeavy,
                                      IssueCarrier.Trim(),FlightCarrier.Trim(),SHC.Trim(),DayOfWeek.Trim(),DepIntFrom.Trim(),DepIntTo.Trim(),ShipperCode,RateBase.Trim(),GLCode
                                      ,SPAMarkup,EquipmentType.Trim(),TransitStation.Trim(),updateRateLineID 
                                  };

                SQLServer da = new SQLServer(constr);
                DataSet dsResult = da.SelectRecords("InsertAllTypesRateLinesNew", param, values, dbtypes);

                if (dsResult == null || dsResult.Tables[0].Rows.Count == 0)
                {
                    errormessage = "Error while inserting rateline header";
                    return false;
                }
                else
                {
                    RateLineID = int.Parse((dsResult.Tables[0].Rows[0][0].ToString()));
                    return true;

                    //Duplication

                   
                }

            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }
# endregion NewRates


       // #region Updatealltypes of RatelinesNew

       // public bool UpdateAllTypesRateLinesNew_Ex(string updateRateLineID, string RateCardSrNo, string Origin, string Destination, string commcode, string ProductType, string flightno,
       //     string AgentCode, string CurrencyID, DateTime FromDt, DateTime ToDt, decimal AgentComm, decimal Discount, decimal Tax,
       //      bool AllInRate, bool IsTact, bool IsHeavy, string IssueCarrier, string FlightCarrier, string SHC,
       //     string DayOfWeek, string DepIntFrom, string DepIntTo, string ShipperCode, string GLCode,string SPAMarkup,string EquipmentType,
       //     string TransitStation,  string RateBase,ref string errormessage)
       //{
           


       //    try
       //        {
       //         errormessage = "";

       //         string[] param = {"rateID","RateCard", "Origin", "Destination", "CommCode", "ProductType", "FlightNumber", "AgentCode",
       //                              "Currency", "FromDate", "ToDate", "AgentComm", "Discount", "Tax", "AllInRate", "IsTact", "IsHeavy",
       //                              "IssueCarrier", "FlightCarrier", "SHC", "DayOfWeek", "DepIntFrom",	"DepIntTo","ShipperCode","RateBase","GLCode","SPAMarkup"
       //                              ,"EquipmentType","TransitStation"
       //                         };

       //         SqlDbType[] dbtypes = { SqlDbType.VarChar,SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,
       //                                 SqlDbType.VarChar,SqlDbType.DateTime,SqlDbType.DateTime,SqlDbType.Decimal,SqlDbType.Decimal,SqlDbType.Decimal,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,
       //                                 SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,
       //                                 SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar
       //                               };
       //         object[] values = {updateRateLineID, RateCardSrNo, Origin.Trim(), Destination.Trim(), commcode.Trim(), ProductType.Trim(), flightno.Trim(), AgentCode.Trim(),
       //                               CurrencyID.Trim(), FromDt, ToDt, AgentComm, Discount,Tax,AllInRate,IsTact,IsHeavy,
       //                               IssueCarrier.Trim(),FlightCarrier.Trim(),SHC.Trim(),DayOfWeek.Trim(),DepIntFrom.Trim(),DepIntTo.Trim(),ShipperCode,RateBase.Trim(),GLCode
       //                               ,SPAMarkup,EquipmentType.Trim(),TransitStation.Trim()
       //                           };

       //         SQLServer da = new SQLServer(constr);
       //         DataSet dsResult = da.SelectRecords("UpdateAllTypesRateLinesNew_Ex", param, values, dbtypes);

       //         if (dsResult == null || dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
       //         {
       //             errormessage = "Error while inserting rateline header";
       //             return false;
       //         }
       //         else 
       //         {
       //            // updateRateLineID = int.Parse(dsResult.Tables[0].Rows[0][1].ToString());
       //             return true;

       //             //Duplication

                   
       //         }

       //     }
       //     catch (Exception ex)
       //     {
       //         errormessage = "" + ex.Message;
       //         return false;
       //     }

       // }


       // #endregion 

        #region delete record
        public bool DeleteRecord(int  RateLineSrNo)
         { 
             try
             {
              string errormessage = "";
             SQLServer da = new SQLServer(constr);
             
             string[] parameters = { "RateLineSrNo" };
             SqlDbType[] dbtypes = { SqlDbType.Int };
             object[] values = { RateLineSrNo };

             if (RateLineSrNo > 0)
                 {
                     values = new object[] { RateLineSrNo};//,ULDType,SlabName, Convert.ToDouble(+pweight), Convert.ToDouble("" + pcharge) };
                     if (!da.ExecuteProcedure("SP_Deleterecord", parameters, dbtypes, values))
                     {
                         errormessage = "";

                         errormessage = "Error while inserting slab Q(" + RateLineSrNo + ")";
                         return false;
                     }
                 }
                 return true;
             }
             catch (Exception ex)
             { return false; }
         }

#endregion 
 #region CompareFields
         public bool CompareFields(string RateCardID ,ref int rateLineID)
         
        {
            try
            {
                string param = "RateID";

                SqlDbType dbtypes = SqlDbType.VarChar;


                object values = RateCardID;

                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();
                    ds= da.SelectRecords("sp_CompareFields", param, values, dbtypes);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                  
                    return true;
                }
                else
                {
                  rateLineID=int.Parse(ds.Tables[0].Rows[0][1].ToString());
                    return true;

                }

            }
            catch (Exception ex)
            {
                //errormessage = "" + ex.Message;
                return false;
            }
             return true;
         
         }
#endregion
         
         public bool InsertAllTypesRateLinesSlabs(int RateLineSrNo, string SlabName, decimal weight, decimal charge,string errormessage)
         {
             try 
             {
                 SQLServer da = new SQLServer(constr);
                 
                 string[] parameters = { "RateLineSrNo", "SlabName", "Weight", "Charge" };
                 SqlDbType[] dbtypes = { SqlDbType.Int,SqlDbType.VarChar,SqlDbType.Decimal,SqlDbType.Decimal};
                 object[] values = { RateLineSrNo,"Q",weight,charge};

                 if (RateLineSrNo > 0)
                 {
                     values = new object[] { RateLineSrNo, "Q", Convert.ToDouble(+weight), Convert.ToDouble("" +charge) };
                     if (!da.ExecuteProcedure("SP_InsertRateLineSlabs", parameters, dbtypes, values))
                     {
                         errormessage = "";
                         
                        errormessage = "Error while inserting slab Q(" + RateLineSrNo + ")";
                         return false;
                     }
                 }
                 
                 
                 
                 
                 
                 return true; 
             }
             catch (Exception ex)
             {
                 return false;
             }
         }

         #region ULDSLABS
         public bool InsertAllTypesULDSlabs(int RateLineSrNo, string ULDType, string SlabName, decimal pweight, decimal pcharge, string errormessage)
         {
             try
             {
                 SQLServer da = new SQLServer(constr);
                 
                 string[] parameters = { "RateLineSrNo", "ULDtype", "SlabName", "Weight", "Charge" };
                 SqlDbType[] dbtypes = { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Decimal, SqlDbType.Decimal };
                 object[] values = { RateLineSrNo, ULDType, SlabName, pweight, pcharge };

                 if (RateLineSrNo > 0)
                 {
                     values = new object[] { RateLineSrNo,ULDType,SlabName, Convert.ToDouble(+pweight), Convert.ToDouble("" + pcharge) };
                     if (!da.ExecuteProcedure("SP_InsertULDSlabs", parameters, dbtypes, values))
                     {
                         errormessage = "";

                         errormessage = "Error while inserting slab Q(" + RateLineSrNo + ")";
                         return false;
                     }
                 }
                 string[] parameter = { "RateLineSrNo"};
                 SqlDbType[] dbtype = { SqlDbType.Int};
                 object[] value = { RateLineSrNo};
                 if (ULDType == "")
                 {
                     values = new object[] { RateLineSrNo };
                     
                     if (!da.ExecuteProcedure("SP_Deleterecord", parameter, dbtype, value))
                     {
                         errormessage = "";

                         errormessage = "Error while inserting slab Q(" + ULDType + ")";
                         return false;
                     }
                 
                 }
                 return true;
             }
             catch (Exception ex)
             { return false; }
         }
         #endregion ULDSLABS


         #region Update ULDSlabs

         public bool UpdateAllTypesULDSlabs(string RateLineSrNo, string ULDType, string SlabName, decimal pweight, decimal pcharge, string errormessage)
         {
             try
             {
                 SQLServer da = new SQLServer(constr);

                 string[] parameters = { "RateLineSrNo", "ULDtype", "SlabName", "Weight", "Charge" };
                 SqlDbType[] dbtypes = { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Decimal, SqlDbType.Decimal };
                 object[] values = { RateLineSrNo, ULDType, SlabName, pweight, pcharge };

                 if (RateLineSrNo.Length > 0)
                 {
                     values = new object[] { RateLineSrNo, ULDType, SlabName, Convert.ToDouble(+pweight), Convert.ToDouble("" + pcharge) };
                     if (!da.ExecuteProcedure("SP_UpdateULDSlabs", parameters, dbtypes, values))
                     {
                         errormessage = "";

                         errormessage = "Error while inserting slab Q(" + RateLineSrNo + ")";
                         return false;
                     }
                 }
                 string[] parameter = { "RateLineSrNo" };
                 SqlDbType[] dbtype = { SqlDbType.Int };
                 object[] value = { RateLineSrNo };
                 if (ULDType == "")
                 {
                     values = new object[] { RateLineSrNo };

                     if (!da.ExecuteProcedure("SP_Deleterecord", parameter, dbtype, value))
                     {
                         errormessage = "";

                         errormessage = "Error while inserting slab Q(" + ULDType + ")";
                         return false;
                     }

                 }
                 return true;
             }
             catch (Exception ex)
             { return false; }
         }
#endregion
         //Partner Excel
        #region Save Airline Data
        public bool SaveAirline(object[] QueryValues)
        {
            try
            {

                string[] QueryNames = new string[35];
                QueryNames[0] = "PartnerName";
                QueryNames[1] = "PartnerLegalName";
                QueryNames[2] = "PartnerPrefix";
                QueryNames[3] = "DesignatorCode";
                QueryNames[4] = "PartnerLocationID";
                QueryNames[5] = "PartnerAccountingCode";
                QueryNames[6] = "RegistrationID";
                QueryNames[7] = "DigitalSignature";
                QueryNames[8] = "Suspend";
                QueryNames[9] = "President";
                QueryNames[10] = "CFO";
                QueryNames[11] = "CurrencyListing";
                QueryNames[12] = "Language";
                QueryNames[13] = "TaxRegistrationID";
                QueryNames[14] = "AdditionalTaxRegistrationID";
                QueryNames[15] = "SettlementMethod";
                QueryNames[16] = "Address";
                QueryNames[17] = "Country";
                QueryNames[18] = "City";
                QueryNames[19] = "PostalCode";
                QueryNames[20] = "CurrencyBilling";
                QueryNames[21] = "UpdatedBy";
                QueryNames[22] = "PartnerType";
                QueryNames[23] = "ValidFrom";
                QueryNames[24] = "ValidTo";
                QueryNames[25] = "IsScheduled";
                QueryNames[26] = "SITAID";
                QueryNames[27] = "EmailID";
                QueryNames[28] = "PartialAcceptance";
                QueryNames[29] = "Tolerance";
                QueryNames[30] = "OtherCharges";
                QueryNames[31] = "CustomsMsg";
                QueryNames[32] = "onbilling";
                QueryNames[33] = "AutoGenInvoice";
                QueryNames[34] = "BillType";


                SqlDbType[] QueryTypes = new SqlDbType[35];

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
                QueryTypes[5] = SqlDbType.VarChar;
                QueryTypes[6] = SqlDbType.VarChar;
                QueryTypes[7] = SqlDbType.Bit;
                QueryTypes[8] = SqlDbType.Bit;
                QueryTypes[9] = SqlDbType.VarChar;
                QueryTypes[10] = SqlDbType.VarChar;
                QueryTypes[11] = SqlDbType.VarChar;
                QueryTypes[12] = SqlDbType.VarChar;
                QueryTypes[13] = SqlDbType.VarChar;
                QueryTypes[14] = SqlDbType.VarChar;
                QueryTypes[15] = SqlDbType.VarChar;
                QueryTypes[16] = SqlDbType.VarChar;
                QueryTypes[17] = SqlDbType.VarChar;
                QueryTypes[18] = SqlDbType.VarChar;
                QueryTypes[19] = SqlDbType.VarChar;
                QueryTypes[20] = SqlDbType.VarChar;
                QueryTypes[21] = SqlDbType.VarChar;
                QueryTypes[22] = SqlDbType.VarChar;
                QueryTypes[23] = SqlDbType.DateTime;
                QueryTypes[24] = SqlDbType.DateTime;
                QueryTypes[25] = SqlDbType.VarChar;
                QueryTypes[26] = SqlDbType.VarChar;
                QueryTypes[27] = SqlDbType.VarChar;
                QueryTypes[28]=SqlDbType.VarChar;
                QueryTypes[29]=SqlDbType.Decimal;
                QueryTypes[30] = SqlDbType.Bit;
                QueryTypes[31] = SqlDbType.Bit;
                QueryTypes[32] = SqlDbType.VarChar;
                QueryTypes[33] = SqlDbType.Bit;
                QueryTypes[34]=SqlDbType.VarChar;
              //  QueryTypes[35] = SqlDbType.VarChar;


                SQLServer da = new SQLServer(constr);
                if (da.ExecuteProcedure("SP_SavePartnerData", QueryNames, QueryTypes, QueryValues))
                { return true; }
                else
                { return false; }




            }
            catch (Exception ex)
            { return false; }
        }
        #endregion

        //Airport Excel
        # region AddAirport
        public int AddAirport(object[] AirportInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);

                string[] ColumnNames = new string[46];
                SqlDbType[] DataType = new SqlDbType[46];
                Object[] Values = new object[46];
                int i = 0;

                //0
                ColumnNames.SetValue("AirportCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //1
                ColumnNames.SetValue("AirportName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //2
                ColumnNames.SetValue("CountryCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //3 
                ColumnNames.SetValue("RegionCode", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //4
                ColumnNames.SetValue("City", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //5
                ColumnNames.SetValue("IsActive", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //6
                ColumnNames.SetValue("StationMailId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //7
                ColumnNames.SetValue("ManagerName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //8
                ColumnNames.SetValue("ManagerEmailId", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //9
                ColumnNames.SetValue("ShiftMobNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //10
                ColumnNames.SetValue("LandlineNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //11
                ColumnNames.SetValue("ManagerMobNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //12
                ColumnNames.SetValue("Counter", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //13
                ColumnNames.SetValue("GHAName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //14
                ColumnNames.SetValue("GHAAddress", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //15
                ColumnNames.SetValue("GHAPhoneNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //16
                ColumnNames.SetValue("GHAMobileNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //17
                ColumnNames.SetValue("GHAFAXNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //18
                ColumnNames.SetValue("GHAEmailID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //19
                ColumnNames.SetValue("GSAName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //20
                ColumnNames.SetValue("GSAAddress", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //21
                ColumnNames.SetValue("GSAPhoneNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //22
                ColumnNames.SetValue("GSAMobileNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //23
                ColumnNames.SetValue("GSAFAXNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //24
                ColumnNames.SetValue("GSAEmailID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //25
                ColumnNames.SetValue("APMName", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //26
                ColumnNames.SetValue("APMAddress", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //27
                ColumnNames.SetValue("APMPhoneNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //28
                ColumnNames.SetValue("APMMobileNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //29
                ColumnNames.SetValue("APMFAXNo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //30
                ColumnNames.SetValue("APMEmailID", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //31
                ColumnNames.SetValue("AdditionalInfo", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //32
                ColumnNames.SetValue("TransitTime", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //33
                ColumnNames.SetValue("CutOffTime", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;


                //34
                ColumnNames.SetValue("IsTaxExempted", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //35
                ColumnNames.SetValue("BookingCurrency", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //36
                ColumnNames.SetValue("BookingCurrencyType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //37
                ColumnNames.SetValue("InvoiceCurrency", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //38
                ColumnNames.SetValue("InvoiceCurrencyType", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //39
                ColumnNames.SetValue("citytype", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //40
                ColumnNames.SetValue("GLAccount", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;
                //41
                ColumnNames.SetValue("TimeZones", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;
                //42
                ColumnNames.SetValue("Latitude", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //43
                ColumnNames.SetValue("Longitude", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                //44
                ColumnNames.SetValue("IsULDEnabled", i);
                DataType.SetValue(SqlDbType.Bit, i);
                Values.SetValue(AirportInfo.GetValue(i), i);
                i++;

                ColumnNames.SetValue("UTCTIMEDIFF", i);
                DataType.SetValue(SqlDbType.VarChar, i);
                Values.SetValue(AirportInfo.GetValue(i), i);

                if (!da.ExecuteProcedure("SpAddAirportRecordsNew", ColumnNames, DataType, Values))
                    // AddCountryMaster();
                    return (-1);
                else
                {
                    return (0);
                }
            }
            catch (Exception ex)
            {
                return (-1);
            }

        }
        # endregion AddAirport



        //Agent Excel
        #region Add Agent
        public bool AddAgent(object[] AgentInfo)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                int i = 0;

                string[] paramname = new string[39];
                paramname[0] = "AgentCode";
                paramname[1] = "IATAAgentCode";
                paramname[2] = "AgentName";
                paramname[3] = "ValidFrom";
                paramname[4] = "ValidTo";
                paramname[5] = "CustomerCode";
                paramname[6] = "Station";
                paramname[7] = "Country";
                paramname[8] = "City";
                paramname[9] = "Adress";
                paramname[10] = "Email";
                paramname[11] = "PersonContact";
                paramname[12] = "MobileNumber";
                paramname[13] = "Remark";
                paramname[14] = "strFlag";
                paramname[15] = "NormalComm";
                paramname[16] = "ControllingLocator";
                paramname[17] = "AccountCode";
                //paramname[18] = "TDSOnCommision";
                //paramname[19] = "TDSOnFreight";
                paramname[18] = "ControllingLocatorCode";
                paramname[19] = "BuildTo";
                paramname[20] = "AccountMail";
                paramname[21] = "SalesMail";
                paramname[22] = "AgentType";
                paramname[23] = "BillType";
                paramname[24] = "PanCardNumber";
                paramname[25] = "ServiceTaxNumber";
                paramname[26] = "ValidBG";
                paramname[27] = "CurrencyCode";
                paramname[28] = "IsFOC";
                paramname[29] = "threshold";
                paramname[30] = "AgentReferenceCode";
                paramname[31] = "RatePreference";
                paramname[32] = "AutoGenInv";
                paramname[33] = "chkStnList";
                paramname[34] = "IACCode ";
                paramname[35] = "CCSFCode";
                paramname[36] = "TDSOnCommision";
                paramname[37] = "TDSOnFreight";
                paramname[38] = "KnownShipper";
                SqlDbType[] paramtype = new SqlDbType[39];
                paramtype[0] = SqlDbType.NVarChar;
                paramtype[1] = SqlDbType.NVarChar;
                paramtype[2] = SqlDbType.NVarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.NVarChar;
                paramtype[6] = SqlDbType.NVarChar;
                paramtype[7] = SqlDbType.NVarChar;
                paramtype[8] = SqlDbType.NVarChar;
                paramtype[9] = SqlDbType.NVarChar;
                paramtype[10] = SqlDbType.NVarChar;
                paramtype[11] = SqlDbType.NVarChar;
                paramtype[12] = SqlDbType.VarChar;
                paramtype[13] = SqlDbType.NVarChar;
                paramtype[14] = SqlDbType.NVarChar;
                paramtype[15] = SqlDbType.NVarChar;
                paramtype[16] = SqlDbType.NVarChar;
                paramtype[17] = SqlDbType.NVarChar;
                // paramtype[18] = SqlDbType.NVarChar;
                //  paramtype[19] = SqlDbType.NVarChar;
                paramtype[18] = SqlDbType.NVarChar;
                paramtype[19] = SqlDbType.NVarChar;
                paramtype[20] = SqlDbType.NVarChar;
                paramtype[21] = SqlDbType.NVarChar;
                paramtype[22] = SqlDbType.NVarChar;
                paramtype[23] = SqlDbType.NVarChar;
                paramtype[24] = SqlDbType.VarChar;
                paramtype[25] = SqlDbType.VarChar;
                paramtype[26] = SqlDbType.VarChar;
                paramtype[27] = SqlDbType.VarChar;
                paramtype[28] = SqlDbType.Bit;
                paramtype[29] = SqlDbType.VarChar;
                paramtype[30] = SqlDbType.VarChar;
                paramtype[31] = SqlDbType.VarChar;
                paramtype[32] = SqlDbType.Bit;
                paramtype[33] = SqlDbType.VarChar;
                paramtype[34] = SqlDbType.VarChar;
                paramtype[35] = SqlDbType.VarChar;
                paramtype[36] = SqlDbType.VarChar;
                paramtype[37] = SqlDbType.VarChar;
                paramtype[38] = SqlDbType.Bit;
                bool ds = da.InsertData("Sp_InsertAgentDetails", paramname, paramtype,AgentInfo);
                return (ds);

            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool AddAgentCredits(object[] AgentCredit)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                int i = 0;

                string[] paramname = new string[17];
                paramname[0] = "AgentCode";
                paramname[1] = "BankName";
                paramname[2] = "BankGuranteeNumber";
                paramname[3] = "BankGuranteeAmount";
                paramname[4] = "ValidFrom";
                paramname[5] = "ValidTo";
                paramname[6] = "FinalAmt";
                paramname[7] = "CreditAmount";
                paramname[8] = "InvoiceBalance";
                paramname[9] = "CreditRemaining";
                paramname[10] = "Expired";
                paramname[11] = "TresholdLimit";
                paramname[12] = "TransactionType";
                paramname[13] = "CreditDays";
                paramname[14] = "TresholdLimitDays";
                paramname[15] = "BankAddress";
                paramname[16] = "SerialNumber";
                SqlDbType[] paramtype = new SqlDbType[17];
                paramtype[0] = SqlDbType.NVarChar;
                paramtype[1] = SqlDbType.NVarChar;
                paramtype[2] = SqlDbType.NVarChar;
                paramtype[3] = SqlDbType.Float;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.VarChar;
                paramtype[6] = SqlDbType.VarChar;
                paramtype[7] = SqlDbType.Float;
                paramtype[8] = SqlDbType.Float;
                paramtype[9] = SqlDbType.Float;
                paramtype[10] = SqlDbType.NVarChar;
                paramtype[11] = SqlDbType.NVarChar;
                paramtype[12] = SqlDbType.NVarChar;
                paramtype[13] = SqlDbType.VarChar;
                paramtype[14] = SqlDbType.Int;
                paramtype[15] = SqlDbType.VarChar;
                paramtype[16] = SqlDbType.Int;
                bool res = da.InsertData("Sp_InsertCreditData", paramname, paramtype, AgentCredit);
                return (res);

            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public string AddAgentDeal(object[] AgentDeals)
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

                string result = da.GetStringByProcedure("SpSaveAgentDealsDetails", paramname, AgentDeals, paramtype);
                return result;
            }
            catch (Exception)
            {

                return null;
            }

        }
        #endregion Add Agent

        public bool InsertAllTypesOtherCharges(string ChargeHeadCode, string ChargeHeadName, DateTime FromDT, DateTime ToDT, string origin, string Destination, string Currency,
    string dueon, string paymentType, decimal AgentComm, decimal Discount, decimal Tax, decimal TDS, string ChargeHeadBasis, decimal mincharge, decimal perunit, string WeightType,
    string ViaStation, string chargedAt, string Commodity, string Product, string Flight, string Agent, string SHC, string DayOfWeek, string Handler, string Equipment, ref string errormessage,
    string GLAccount, string IssueCarrier, string FlightCarrier, string AirlineCode, string ShipperCode, ref int srno, int OCId)
        {
            try
            {
                errormessage = "";

                string[] param = {"ChargeHeadCode","ChargeHeadName","FromDT","ToDT","origin","Destination","Currency","dueon",
                                    "paymentType","AgentComm","Discount","Tax","TDS" ,"ChargeHeadBasis","mincharge","perunit",
                                    "WeightType","ViaStation","chargedAt","Commodity","Product",
                                    "Flight","Agent","SHC","DayOfWeek","Handler","Equipment",
                                    "GLAccountNo","FlightCarrier", "IssueCarrier", "AirlineCode", "ShipperCode","OCId"
                                };

                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.VarChar,
                                        SqlDbType.VarChar,SqlDbType.Decimal,SqlDbType.Decimal,SqlDbType.Decimal,SqlDbType.Decimal,SqlDbType.VarChar,SqlDbType.Decimal,SqlDbType.Decimal,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar
                                        ,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Int
                                      };
                object[] values = {ChargeHeadCode,ChargeHeadName,FromDT,ToDT,origin,Destination,Currency,dueon,
                                    paymentType,AgentComm,Discount,Tax,TDS ,ChargeHeadBasis,mincharge,perunit,WeightType,ViaStation,chargedAt,Commodity,
                                    Product,Flight,Agent, SHC,DayOfWeek,Handler,Equipment,
                                  GLAccount,FlightCarrier,IssueCarrier,AirlineCode,ShipperCode,OCId};

                SQLServer da = new SQLServer(constr);
                DataSet dsResult = da.SelectRecords("SP_UploadOtherCharges", param, values, dbtypes);

                if (dsResult == null || dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                {
                    // error
                    errormessage = "Error while inserting rateline header";
                    return false;
                }
                else
                {

                    srno = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());

                    #region wtslabscommented
                    //if (ChargeHeadBasis.Equals("K", StringComparison.OrdinalIgnoreCase) || ChargeHeadBasis.Equals("S", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    int SrNo;

                    //    SrNo = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());

                    //    //delete rate line slabs before insert or update for RateLineSrNo
                    //    param = new string[] { "OthSrNo" };
                    //    dbtypes = new SqlDbType[] { SqlDbType.Int };
                    //    values = new object[] { SrNo };

                    //    if (!da.ExecuteProcedure("SP_DeleteOthLineSlabs", param, dbtypes, values))
                    //    {
                    //        errormessage = "Error while deleting previous slabs(" + SrNo + ")";
                    //        return false;
                    //    }

                    //    param = new string[] { "OthSrNo", "SlabName", "Weight", "Charge", "Cost" };
                    //    dbtypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float };
                    //    values = new object[] { SrNo, "M", Convert.ToDouble("0"), Convert.ToDouble("" + min), Convert.ToDouble("0") };

                    //    if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                    //    {
                    //        errormessage = "Error while inserting slab M(" + SrNo + ")";
                    //        return false;
                    //    }


                    //    values = new object[] { SrNo, "N", Convert.ToDouble("0"), Convert.ToDouble("" + normal), Convert.ToDouble("0") };

                    //    if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                    //    {
                    //        errormessage = "Error while inserting slab N(" + SrNo + ")";
                    //        return false;
                    //    }

                    //    if (Convert.ToDouble("" + fourtyfive) > 0)
                    //    {
                    //        values = new object[] { SrNo, "Q", Convert.ToDouble("45"), Convert.ToDouble("" + fourtyfive), Convert.ToDouble("0") };

                    //        if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                    //        {
                    //            errormessage = "Error while inserting slab Q(" + SrNo + ")";
                    //            return false;
                    //        }
                    //    }

                    //    if (Convert.ToDouble("" + hundred) > 0)
                    //    {
                    //        values = new object[] { SrNo, "Q", Convert.ToDouble("100"), Convert.ToDouble("" + hundred), Convert.ToDouble("0") };

                    //        if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                    //        {
                    //            errormessage = "Error while inserting slab Q(" + SrNo + ")";
                    //            return false;
                    //        }
                    //    }

                    //    if (Convert.ToDouble("" + TwoFifty) > 0)
                    //    {
                    //        values = new object[] { SrNo, "Q", Convert.ToDouble("250"), Convert.ToDouble("" + TwoFifty), Convert.ToDouble("0") };

                    //        if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                    //        {
                    //            errormessage = "Error while inserting slab Q(" + SrNo + ")";
                    //            return false;
                    //        }
                    //    }

                    //    if (Convert.ToDouble("" + ThreeHundred) > 0)
                    //    {
                    //        values = new object[] { SrNo, "Q", Convert.ToDouble("300"), Convert.ToDouble("" + ThreeHundred), Convert.ToDouble("0") };

                    //        if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                    //        {
                    //            errormessage = "Error while inserting slab Q(" + SrNo + ")";
                    //            return false;
                    //        }
                    //    }

                    //    if (Convert.ToDouble("" + FiveHun) > 0)
                    //    {
                    //        values = new object[] { SrNo, "Q", Convert.ToDouble("500"), Convert.ToDouble("" + FiveHun), Convert.ToDouble("0") };

                    //        if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                    //        {
                    //            errormessage = "Error while inserting slab Q(" + SrNo + ")";
                    //            return false;
                    //        }
                    //    }

                    //    if (Convert.ToDouble("" + Thosand) > 0)
                    //    {
                    //        values = new object[] { SrNo, "Q", Convert.ToDouble("1000"), Convert.ToDouble("" + Thosand), Convert.ToDouble("0") };

                    //        if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                    //        {
                    //            errormessage = "Error while inserting slab Q(" + SrNo + ")";
                    //            return false;
                    //        }
                    //    }

                    //    if (Convert.ToDouble("" + TwoThousand) > 0)
                    //    {
                    //        values = new object[] { SrNo, "Q", Convert.ToDouble("2000"), Convert.ToDouble("" + TwoThousand), Convert.ToDouble("0") };

                    //        if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                    //        {
                    //            errormessage = "Error while inserting slab Q(" + SrNo + ")";
                    //            return false;
                    //        }
                    //    }

                    //    if (Convert.ToDouble("" + ThreeThousand) > 0)
                    //    {
                    //        values = new object[] { SrNo, "Q", Convert.ToDouble("3000"), Convert.ToDouble("" + ThreeThousand), Convert.ToDouble("0") };

                    //        if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                    //        {
                    //            errormessage = "Error while inserting slab Q(" + SrNo + ")";
                    //            return false;
                    //        }
                    //    }

                    //    if (Convert.ToDouble("" + FiveThousand) > 0)
                    //    {
                    //        values = new object[] { SrNo, "Q", Convert.ToDouble("5000"), Convert.ToDouble("" + FiveThousand), Convert.ToDouble("0") };

                    //        if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                    //        {
                    //            errormessage = "Error while inserting slab Q(" + SrNo + ")";
                    //            return false;
                    //        }
                    //    }

                    //    return true;
                    //}
                    //else
                    //{
                    //    return true;
                    //}
                    #endregion wtslabscommented
                }
                return true;

            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

        #region InsertExchangeRates
        public string InsertExchangeRates(string CurrencyCode, double CurrencyIATARate, string Type, string ValidFromDt, string ValidToDt)
        {
            try
            {
                //errormessage = "";

                string[] param = { "CurrencyCode", "CurrencyIATARate", "Type", "ValidFromDate", "ValidToDate", "Result" };

                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.Float, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { CurrencyCode.Trim(), CurrencyIATARate, Type.Trim(), ValidFromDt, ValidToDt, "" };

                SQLServer da = new SQLServer(constr);
                string res = da.GetStringByProcedure("SP_InsertExchangeRates", param, values, dbtypes);
                return res;

            }
            catch (Exception ex)
            {
                return "error";
            }
        }
        #endregion InsertExchangeRates

        #region InsertGLAccounts
        public string InsertGLAccounts(string GLCode, string GLDescription)
        {
            try
            {
                //errormessage = "";

                string[] param = { "GLCode", "GLDescription", "Result" };

                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar };
                object[] values = { GLCode.Trim(), GLDescription.Trim(), "" };

                SQLServer da = new SQLServer(constr);
                string res = da.GetStringByProcedure("SP_InsertGLAccounts", param, values, dbtypes);
                return res;

            }
            catch (Exception ex)
            {
                return "error";
            }
        }
        #endregion InsertGLAccounts

        #region Flagforupload
        public string FlagUpload(bool flag, string uploadsheet)
        {
            try
            {
                //errormessage = "";

                string[] param = { "flag", "uploadsheet" };

                SqlDbType[] dbtypes = { SqlDbType.Bit, SqlDbType.VarChar };
                object[] values = { flag, uploadsheet };

                SQLServer da = new SQLServer(constr);
                string res = da.GetStringByProcedure("sp_uploadflag", param, values, dbtypes);
                return res;

            }
            catch (Exception ex)
            {
                return "error";
            }
        }
        #endregion InsertGLAccounts



    }
}
