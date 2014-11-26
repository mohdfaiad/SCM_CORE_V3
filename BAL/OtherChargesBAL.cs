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
    public class OtherChargesBAL
    {
        #region Get Agent List
        /// <summary>
        /// Get list of all the agents based on entered value.
        /// </summary>
        /// <returns>Agent code list as Array.</returns>
        public bool GetAgentList(ref DataSet dsResult)
        {
            try
            {
                //Get agent codes...
                SQLServer da = new SQLServer(Global.GetConnectionString());
                dsResult = da.SelectRecords("spGetAllAgents");
                if (dsResult != null)
                {
                    if (dsResult.Tables != null)
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            return (true);
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        #endregion Get Agent List

        public int SaveOtherCharges(string SerialNumber,string ChargeHeadCode, string ChargeHeadName, string ParticipationType, bool Refundable,
            DateTime StartDate, DateTime EndDate, int LocationLevel, string Location, int OriginLevel, string Origin,
            int DestinationLevel, string Destination, string CurrencyID, string PaymentType, string ChargeType, decimal DiscountPercent,
            decimal CommPercent, decimal ServiceTax, string ChargeHeadBasis, decimal MinimumCharge, decimal PerUnitCharge,
            string WeightType, string Status, string UpdatedBy, string FlightNumber, string FlightCarrier, string HandlingCode,string AirlineCode,
            string IATACommCode,string AgentCode,string ShipperCode,bool FNInc,bool FCInc,bool HCInc,bool ACInc,bool CCInc,bool ADInc,bool SCInc,DataSet dsExceptions, ref string errormessage,string Type,
            string VIAStattion, string CCode, string PSource, string PDest, bool SourceInc, bool DestInc, string wkDaysval, bool wkDaysInc, DataSet dsSlabs,string UserName, string comment,string Dt,
            string trigger, int time, bool we, bool ph, bool ch, bool sh, string basedon, float baserate, decimal MinimumCost, decimal PerUnitCost,
            string CountrySource, string CountryDest, string ProductType, string Handler, bool CountrySInc, bool CountryDInc, bool PTInc, bool HandInc,string EquipType,bool ETInc,string GLCode,
            string IssueCarrier, bool ICInc, decimal MaxVal, decimal MaxCost, string OCCOde, bool IgnoreCCSF,bool IsPackagingCharge)
        {
            int SrNo = 0;
            try
            {

                string[] param = {"SerialNumber","ChargeHeadCode", "ChargeHeadName", "ParticipationType", "Refundable", "StartDate", "EndDate", "LocationLevel", "Location", "OriginLevel", "Origin", 
                                  "DestinationLevel", "Destination", "CurrencyID", "PaymentType", "ChargeType", "DiscountPercent", "CommPercent", "ServiceTax",  
                                  "ChargeHeadBasis", "MinimumCharge", "PerUnitCharge", "WeightType", "Status", "UpdatedBy",
                                  "FlightNumber","FlightCarrier","HandlingCode","AirlineCode","IATACommCode","AgentCode","ShipperCode",
                                  "FNInc","FCInc","HCInc","ACInc","CCInc","ADInc","SCInc","Type","ViaStation",
                                  "CCode","ParamSource","ParamDest","SourceInc","DestInc","wkDays","wkDaysInc","BasedOn","Baserate","MCost","Cost",
                                  "CountrySource","CountryDest","ProductType","Handler","CountrySInc","CountryDInc","PTInc","HandInc","EquipType","ETInc","GLCode",
                                  "IssueCarrier","ICInc","MaxVal","MaxCost","OCCOde","chkIgnoreCCSF","IsPackaging"
                                };

                SqlDbType[] dbtypes = { SqlDbType.Int,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.DateTime,SqlDbType.DateTime,SqlDbType.Int,SqlDbType.VarChar,SqlDbType.Int,SqlDbType.VarChar,
                                        SqlDbType.Int,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Float,SqlDbType.Float,SqlDbType.Float,
                                        SqlDbType.VarChar,SqlDbType.Float,SqlDbType.Float,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,

                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,
                                        SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.VarChar,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Float,SqlDbType.Float,SqlDbType.Float,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,
                                        SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.Decimal,SqlDbType.Decimal,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.Bit
                                      };
                object[] values = { 
                                    int.Parse(SerialNumber),ChargeHeadCode, ChargeHeadName, ParticipationType, Refundable, StartDate, EndDate, LocationLevel, Location, OriginLevel, Origin, 
                                    DestinationLevel, Destination, CurrencyID, PaymentType, ChargeType, DiscountPercent, CommPercent, ServiceTax,  
                                    ChargeHeadBasis, MinimumCharge, PerUnitCharge, WeightType, Status, UpdatedBy,
                                    FlightNumber,FlightCarrier,HandlingCode,AirlineCode,IATACommCode,AgentCode,ShipperCode,
                                    FNInc,FCInc,HCInc,ACInc,CCInc,ADInc,SCInc,Type,VIAStattion,
                                    CCode,PSource,PDest,SourceInc,DestInc,wkDaysval,wkDaysInc,basedon,baserate,MinimumCost,PerUnitCost,CountrySource,
                                    CountryDest,ProductType,Handler,CountrySInc,CountryDInc,PTInc,HandInc,EquipType,ETInc,GLCode,IssueCarrier,ICInc,
                                    MaxVal,MaxCost,OCCOde,IgnoreCCSF,IsPackagingCharge
                                  };


                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet dsResult = da.SelectRecords("SP_SaveOtherChargesMasterNew", param, values, dbtypes);

                if (dsResult == null || dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                {
                    // error
                    errormessage = "Record insert failed.";
                    return 0;
                }
                else
                {

                    SrNo = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());
                    //return true;

                    foreach (DataRow row in dsExceptions.Tables[0].Rows)
                    {
                        // ChargeHeadSrNo,AgentCode,CommissionPercent,DiscountPercent,ServiceTax,UpdatedBy

                        param = new string[]{"ChargeHeadSrNo","AgentCode","CommissionPercent","DiscountPercent","ServiceTax","UpdatedBy" };

                        dbtypes = new SqlDbType[]{ SqlDbType.Int,SqlDbType.VarChar,SqlDbType.Float,SqlDbType.Float,SqlDbType.Float,SqlDbType.VarChar  };
                        values = new object[] { SrNo, row["Agent"].ToString(), Convert.ToDecimal(row["Commision"].ToString()), Convert.ToDecimal(row["Discount"].ToString()), Convert.ToDecimal(row["Tax"].ToString()), "" };

                        if (row["Agent"].ToString().Trim() != "Select")
                        {
                            if (!da.ExecuteProcedure("SP_SaveOtherChargesExceptions", param, dbtypes, values))
                            {
                                //error

                            }
                        }

                    }

                    #region Insert Slab in db
                    try
                    {
                        if (dsSlabs != null)
                        {
                            if (dsSlabs.Tables.Count > 0)
                            {
                                param = new string[] { "OthSrNo" };
                                dbtypes = new SqlDbType[] { SqlDbType.Int };
                                values = new object[] { SrNo };

                                if (da.ExecuteProcedure("SP_DeleteOthLineSlabs", param, dbtypes, values))
                                {
                                    for (int i = 0; i < dsSlabs.Tables[0].Rows.Count; i++)
                                    {
                                        param = new string[] { "OthSrNo", "SlabName", "Weight", "Charge","Cost" };
                                        dbtypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Float, SqlDbType.Float, SqlDbType.Float };
                                        values = new object[] { SrNo, dsSlabs.Tables[0].Rows[i]["Type"].ToString(), Convert.ToDouble(dsSlabs.Tables[0].Rows[i]["Weight"].ToString()), Convert.ToDouble(dsSlabs.Tables[0].Rows[i]["Charge"].ToString()), Convert.ToDouble(dsSlabs.Tables[0].Rows[i]["Cost"].ToString()) };

                                        if (!da.ExecuteProcedure("SP_InsertOthLineSlabs", param, dbtypes, values))
                                        {
                                            // error
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex) { }
                    #endregion 

                    #region Add trigger if Any
                    try 
                    {
                        if (SrNo > 0)
                        {
                            if (trigger.Length > 0 && (we || ph || ch || sh))
                            {
                                param = new string[] { "ChargeHeadSrNo", "TriggerON", "Time", "WeekEnd", "PubHoliday", "CompHoliday", "StationHoliday", "UpdatedBy" };
                                dbtypes = new SqlDbType[] { SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.Bit, SqlDbType.VarChar };
                                values = new object[] { SrNo, trigger, time, we, ph, ch, sh, UpdatedBy };

                                if (!da.ExecuteProcedure("SP_InsertOthLineTrigger", param, dbtypes, values))
                                {
                                    // error
                                }
                            }
                        }
                    }
                    catch (Exception ex) { }
                    #endregion

                    #region Add remark in table
                    if (comment != "")
                        Remarks(UserName, comment, SrNo, Dt);
                    #endregion

                    //#region BindRepeator 
                    //BindRepeaterData(SrNo);
                    //#endregion

                }


                return SrNo;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool GetOtherChargeInfo(string code,ref DataSet dsResult,ref string errormessage)
        {
            try
            {
                errormessage = "";

                string[] param = {"ChargeHeadCode" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar};
                object[] values = { code };


                SQLServer da = new SQLServer(Global.GetConnectionString());
                dsResult = da.SelectRecords("SP_GetOtherChargesInfo", param, values, dbtypes);

                if (dsResult == null || dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                {
                    // error
                    errormessage = "Record not found.";
                    return false;
                }
                else
                {
                    return true;
                }


            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

        #region GetDefaultValues
        public bool GetDefaultValues(ref DataSet dsDefaultValues, ref string erromessage)
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                dsDefaultValues = da.SelectRecords("SP_GetDefaultValues");

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
        public bool CheckDuplicate(object[] values, ref bool IsDuplicate, ref string errormessage)
        {
            try
            {
                string[] param = {"ChargeHeadCode", "ChargeHeadName",  "StartDate", "EndDate",  "OriginLevel", "Origin", 
                                  "DestinationLevel", "Destination",                                  
                                  "FlightNumber","FlightCarrier","HandlingCode","AirlineCode","IATACommCode","AgentCode","ShipperCode",
                                  "FNInc","FCInc","HCInc","ACInc","CCInc","ADInc","SCInc","SerialNumber", "ViaStation","CountrySource","CountryDest",
                                  "ProductType","Handler","CountrySInc","CountryDInc","PTInc","HandInc","EquipType","ETInc"
                                };

                SqlDbType[] dbtypes = { SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.DateTime,SqlDbType.DateTime,SqlDbType.Int,SqlDbType.VarChar,
                                        SqlDbType.Int,SqlDbType.VarChar,                                        
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,
                                        SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Int,SqlDbType.VarChar,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,
                                        SqlDbType.VarChar,SqlDbType.Bit
                                      };
               
                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet dsResult = da.SelectRecords("SP_CheckDuplicateOtherCharge", param, values, dbtypes);

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

        #region Check Duplicate TaxLine
        public bool CheckDuplicateTaxLine(object[] values, ref bool IsDuplicate, ref string errormessage)
        {
            try
            {
                string[] param = {"SerialNumber","TaxCode","TaxName","StartDate","EndDate",
                                  "Level","Location",
                                  "OriginLevel","Origin",
                                  "DestinationLevel","Destination",
                                  "CurrId","GLCode",
                                  "TaxPerc","Min","Max","AppOn",
                                  "FlightCarrier","FCInc",
                                  "IssueCarrier","ICInc",
                                  "AirlineCode","ACInc",
                                  "Org","OrgInc","CountrySource","CountrySourcIncs",
                                  "Dest","DestIncs","CountryDest","CountryDestInc",
                                  "FltNo","FNInc",
                                  "WeekDays","WeekDaysInc",
                                  "AgentCode","ADInc",
                                  "ShipperCode","SCInc",
                                  "IATACommCode","CCInc",
                                  "ProductType","PTInc",
                                  "SHC","SHCInc",
                                  "Handler","HandInc",
                                  "EquipType","ETInc",
                                  "OCCode","OCInc",
                                  "AppliedAt","AddInTotal"
                                 };


                SqlDbType[] dbtypes = {SqlDbType.Int,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.DateTime,SqlDbType.DateTime,
                                       SqlDbType.Int,SqlDbType.VarChar,//level
                                       SqlDbType.Int,SqlDbType.VarChar,//origin
                                       SqlDbType.Int,SqlDbType.VarChar,//destination
                                       SqlDbType.VarChar,SqlDbType.VarChar,
                                       SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,
                                       SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,
                                       SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,
                                       SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,
                                       SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,
                                       SqlDbType.VarChar,SqlDbType.Bit,
                                       SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,
                                       SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,
                                       SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,
                                       SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,
                                       SqlDbType.VarChar,SqlDbType.Bit
                                      };

                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet dsResult = da.SelectRecords("SP_CheckDuplicateTaxLine", param, values, dbtypes);

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

        #region GetOtherChargesDetails
        public bool GetOtherChargesDetails(int SrNo, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                string[] param = { "SrNo" };
                SqlDbType[] dbtypes = { SqlDbType.Int};
                object[] values = { SrNo };

                SQLServer da = new SQLServer(Global.GetConnectionString());
                dsResult = da.SelectRecords("SP_GetOtherChargeDetails", param, values, dbtypes);

                if (dsResult != null)
                {
                    if (dsResult.Tables.Count != 0)
                    {

                        if (dsResult.Tables[0].Rows.Count != 0 && dsResult.Tables[1].Rows.Count!=0)
                        {
                            return true;                            
                        }
                        else
                        {
                            errormessage = "Error : (GetOtherChargesDetails) Code III";
                            return false;
                        }
                    }
                    else
                    {
                        errormessage = "Error : (GetOtherChargesDetails) Code II";
                        return false;
                    }
                }
                else
                {
                    errormessage = "Error : (GetOtherChargesDetails) Code I";
                    return false;
                }

               
            }
            catch (Exception ex)
            {
                errormessage = "Error :(GetOtherChargesDetails)" + ex.Message;
                return false;
            }

        }

        #endregion

        #region GetChargeType
        public DataSet GetChargeType()
        {
            DataSet ds = null;
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                ds = da.SelectRecords("spGetOtherChargeType");
            }
            catch (Exception ec) { } 
            return ds;
        }
        #endregion

        protected void Remarks(string UserName,string comment,int othid, string Date)
        {
            try
            {
                //string Date = DateTime.Now.ToString();
                //string Date = Session
                SQLServer da = new SQLServer(Global.GetConnectionString());
                string[] param = { "name", "comments", "date", "ChargeHeadCode" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { UserName, comment, Date, othid };
                DataSet dsadd = da.SelectRecords("SP_InsertRemarksOtherCharges", param, values, dbtypes);
            }
            catch (Exception ex) 
            { }
        }

        public int SaveTaxLine(string SerialNumber,
            string ChargeHeadCode, string ChargeHeadName, bool Refundable,
            string StartDate, string EndDate, int LocationLevel, string Location, int OriginLevel,
            string Origin, int DestinationLevel, string Destination, string CurrencyID,
            string PaymentType, string ChargeType, decimal MinimumCharge, decimal PerUnitCharge,
            string WeightType, string Status, string UpdatedBy, string FlightNumber,
            string FlightCarrier, string HandlingCode, string AirlineCode,
            string IATACommCode, string AgentCode, string ShipperCode, bool FNInc, bool FCInc,
            bool HCInc, bool ACInc, bool CCInc, bool ADInc, bool SCInc,
            DataSet dsExceptions, ref string errormessage, string Type,
            string VIAStattion, string CCode, string PSource, string PDest, bool SourceInc,
            bool DestInc, string wkDaysval, bool wkDaysInc,
            DataSet dsSlabs, string UserName, string comment, string Dt,
            string trigger, int time, bool we, bool ph, bool ch, bool sh,
            string basedon, float baserate, decimal MinimumCost, decimal PerUnitCost,
            string CountrySource, string CountryDest, string ProductType, string Handler,
            bool CountrySInc, bool CountryDInc, bool PTInc, bool HandInc, string EquipType, bool ETInc,
            string IssueCarrier, bool ICInc, string OCCode, bool OCInc, string AppliedOn, decimal maximum, string GlCode, string taxPercent, string AppAt, bool AddInTotal, string TaxType)
        {
            int SrNo = 0;
            try
            {

                string[] param = {"SerialNumber","TaxCode", "TaxName", "Refundable", "StartDate", "EndDate", "LocationLevel", "Location", "OriginLevel", "Origin", 
                                  "DestinationLevel", "Destination", "CurrencyID", "PaymentType", "ChargeType","MinimumCharge", "PerUnitCharge", "WeightType", "Status", "UpdatedBy",
                                  "FlightNumber","FlightCarrier","HandlingCode","AirlineCode","IATACommCode","AgentCode","ShipperCode",
                                  "FNInc","FCInc","HCInc","ACInc","CCInc","ADInc","SCInc","Type","ViaStation",
                                  "CCode","ParamSource","ParamDest","SourceInc","DestInc","wkDays","wkDaysInc","BasedOn","Baserate","MCost","Cost",
                                  "CountrySource","CountryDest","ProductType","Handler","CountrySInc","CountryDInc","PTInc","HandInc","EquipType","ETInc","IssueCarrier","ICInc",
                                  "GLCode","AppOn","OCCOde","OCINC","maximum","TaxPercent","AppliedAt","AddInTotal","TaxType"
                                 };

                SqlDbType[] dbtypes = { SqlDbType.Int,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Int,SqlDbType.VarChar,SqlDbType.Int,SqlDbType.VarChar,
                                        SqlDbType.Int,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,
                                        SqlDbType.VarChar,SqlDbType.Float,SqlDbType.Float,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,

                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,
                                        SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.VarChar,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Float,SqlDbType.Float,SqlDbType.Float,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.Float,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar};
                object[] values = { 
                                    int.Parse(SerialNumber),ChargeHeadCode, ChargeHeadName, Refundable, StartDate, EndDate, LocationLevel, Location, OriginLevel, Origin, 
                                    DestinationLevel, Destination, CurrencyID, PaymentType, ChargeType,MinimumCharge, PerUnitCharge, WeightType, Status, UpdatedBy,
                                    FlightNumber,FlightCarrier,HandlingCode,AirlineCode,IATACommCode,AgentCode,ShipperCode,
                                    FNInc,FCInc,HCInc,ACInc,CCInc,ADInc,SCInc,Type,VIAStattion,
                                    CCode,PSource,PDest,SourceInc,DestInc,wkDaysval,wkDaysInc,basedon,baserate,MinimumCost,PerUnitCost,CountrySource,
                                    CountryDest,ProductType,Handler,CountrySInc,CountryDInc,PTInc,HandInc,EquipType,ETInc,IssueCarrier,ICInc,GlCode,AppliedOn,OCCode,OCInc,maximum,taxPercent,
                                    AppAt,AddInTotal,TaxType
                                  };


                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet dsResult = da.SelectRecords("SP_SaveTaxLineMaster", param, values, dbtypes);

                #region Add remark in table
                if (comment != "")
                    Remarks(UserName, comment, SrNo, Dt);
                #endregion
                if (dsResult == null || dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                {
                    // error
                    errormessage = "Record insert failed.";
                    return 0;
                }
                else
                {

                    SrNo = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());
                }

                return SrNo;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool GetTaxInfo(string code, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                errormessage = "";

                string[] param = { "TaxCode" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar };
                object[] values = { code };


                SQLServer da = new SQLServer(Global.GetConnectionString());
                dsResult = da.SelectRecords("SP_GetTaxInfo", param, values, dbtypes);

                if (dsResult == null || dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                {
                    // error
                    errormessage = "Record not found.";
                    return false;
                }
                else
                {
                    return true;
                }


            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

        public bool GetVolumetricinfo(string code, ref DataSet dsResult, ref string errormessage)
        {
            try
            {
                errormessage = "";

                string[] param = { "VolExempCode" };
                SqlDbType[] dbtypes = { SqlDbType.VarChar };
                object[] values = { code };


                SQLServer da = new SQLServer(Global.GetConnectionString());
                dsResult = da.SelectRecords("SP_GetVolumetricExemption", param, values, dbtypes);

                if (dsResult == null || dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                {
                    // error
                    errormessage = "Record not found.";
                    return false;
                }
                else
                {
                    return true;
                }


            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }

        }

        public int SaveVolumetricinfo(string SerialNumber,
            string StartDate, string EndDate, bool Status, string UpdatedBy, DateTime UpdatedOn, 
            string FlightNumber, string FlightCarrier, string HandlingCode, string AirlineCode, string IATACommCode, string AgentCode, string ShipperCode, 
            bool FNInc, bool FCInc, bool HCInc, bool ACInc, bool CCInc, bool ADInc, bool SCInc,
            string PSource, string PDest, bool SourceInc, bool DestInc, string wkDaysval,
            bool wkDaysInc, string CountrySource, string CountryDest, string ProductType, string Handler, bool CountrySInc, 
            bool CountryDInc, bool PTInc, bool HandInc, string EquipType, bool ETInc,string IssueCarrier, bool ICInc, 
            string OCCode, bool OCInc, DataSet dsExceptions, ref string errormessage)
        {
            int SrNo = 0;
            try
            {
                string[] param = {"SerialNumber", "StartDate", "EndDate", "Status", "UpdatedBy", "UpdatedOn",
                                  "FlightNumber","FlightCarrier","HandlingCode","AirlineCode","IATACommCode","AgentCode","ShipperCode",
                                  "FNInc","FCInc","HCInc","ACInc","CCInc","ADInc","SCInc", 
                                  "ParamSource","ParamDest","SourceInc","DestInc","wkDays",
                                  "wkDaysInc", "CountrySource","CountryDest","ProductType","Handler","CountrySInc",
                                  "CountryDInc","PTInc","HandInc","EquipType","ETInc","IssueCarrier","ICInc",
                                  "OCCOde","OCINC"
                                 };

                SqlDbType[] dbtypes = { 
                                        SqlDbType.Int,SqlDbType.DateTime,SqlDbType.DateTime,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.DateTime,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,
                                        SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,
                                        SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.VarChar,
                                        SqlDbType.Bit, SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,
                                        SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit, SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.Bit,
                                        SqlDbType.VarChar,SqlDbType.Bit};
                object[] values = { 
                                    int.Parse(SerialNumber),StartDate, EndDate, Status, UpdatedBy,UpdatedOn,
                                    FlightNumber,FlightCarrier,HandlingCode,AirlineCode,IATACommCode,AgentCode,ShipperCode,
                                    FNInc,FCInc,HCInc,ACInc,CCInc,ADInc,SCInc, 
                                    PSource,PDest,SourceInc,DestInc,wkDaysval,
                                    wkDaysInc, CountrySource, CountryDest,ProductType,Handler,CountrySInc,
                                    CountryDInc,PTInc,HandInc,EquipType,ETInc,IssueCarrier,ICInc,
                                    OCCode,OCInc
                                  };


                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet dsResult = da.SelectRecords("SP_SaveVolumetricExemption", param, values, dbtypes);

                if (dsResult == null || dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                {
                    errormessage = "Record insert failed.";
                    return 0;
                }
                else
                {

                    SrNo = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());
                }

                return SrNo;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int SaveConfigLine(string SerialNumber, string ConfigCode, string ConfigDesc, string StartDate, string EndDate, int OriginLevel,
            string Origin, int DestinationLevel, string Destination, string FlightNumber, string FlightCarrier, string HandlingCode, string AirlineCode,
            string IATACommCode, string AgentCode, string ShipperCode, string PSource, string PDest, string CountrySource, string CountryDest, string ProductType, string Handler,
            string EquipType, string IssueCarrier, string wkDaysval, bool FNInc, bool FCInc, bool HCInc, bool ACInc, bool CCInc, bool ADInc, bool SCInc, bool SourceInc, bool DestInc,
            bool CountrySInc, bool CountryDInc, bool PTInc, bool HandInc, bool ETInc, bool ICInc, bool wkDaysInc, string UserName, string Dt,
            string SCMParam, string Format, string DateFormat, string NextRoundOff, string DecimalAllow, ref string errormessage)
        {
            int SrNo = 0;
            try
            {

                string[] param = {"SerialNumber","ConfigCode" ,"ConfigDesc" ,"StartDate","EndDate","OriginLevel",
             "Origin","DestinationLevel","Destination","FlightNumber","FlightCarrier","HandlingCode","AirlineCode",
             "IATACommCode","AgentCode","ShipperCode","PSource","PDest", "CountrySource", "CountryDest", "ProductType","Handler",
            "EquipType","IssueCarrier","wkDaysval","FNInc","FCInc","HCInc","ACInc","CCInc","ADInc","SCInc","SourceInc","DestInc",
            "CountrySInc","CountryDInc","PTInc","HandInc","ETInc","ICInc","wkDaysInc","UserName","Dt",
                                 "SCMParam" ,"Format" ,"DateFormat" ,"NextRoundOff" ,"DecimalAllow"};

                SqlDbType[] Types = new SqlDbType[] { SqlDbType.Int,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Int,
                    SqlDbType.VarChar,SqlDbType.Int,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,
            SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,
            SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,
            SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.VarChar,
            SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar};

                object[] values = { 
                                    int.Parse(SerialNumber), ConfigCode,  ConfigDesc, StartDate,  EndDate,  OriginLevel,
             Origin,  DestinationLevel,  Destination, FlightNumber, FlightCarrier, HandlingCode, AirlineCode,
             IATACommCode, AgentCode, ShipperCode, PSource, PDest, CountrySource, CountryDest, ProductType, Handler,
             EquipType, IssueCarrier, wkDaysval, FNInc, FCInc, HCInc, ACInc, CCInc, ADInc, SCInc, SourceInc, DestInc,
             CountrySInc, CountryDInc, PTInc, HandInc, ETInc, ICInc, wkDaysInc, UserName, Dt,
             SCMParam ,Format ,DateFormat ,NextRoundOff ,DecimalAllow};


                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet dsResult = da.SelectRecords("SP_SaveSCMConfigLine", param, values, Types);


                if (dsResult == null || dsResult.Tables.Count == 0 || dsResult.Tables[0].Rows.Count == 0)
                {
                    // error
                    errormessage = "Record insert failed.";
                    return 0;
                }
                else
                {

                    SrNo = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());
                }

                return SrNo;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
