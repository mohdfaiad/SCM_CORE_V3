using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;
using System.Collections;

namespace BAL
{
    public class BALProcessRates
    {
        public DataSet ProcessRates(string AgentCode, string Commodity, string Currency, DateTime ExecutionDate, string PayMode, string Origin,
            string Destination, bool IsVoided, int Pieces,decimal GrossWt, decimal ChargeableWt, string FlightDetails, string SHC, string ProductType,
            string ULDInfo, string AWBNumber, string TriggetPt, decimal DVPrice,string PieceDetails,string RoutePieceDetails,bool Interline,string Handler,string AWBPrefix,
            bool isExport,bool XrayReq,string IssueCarrier,string shipper,string consignee,decimal SLAC,string PackagingInfo,bool AllInRate)
        {
            
            try
            {
                decimal Freight = 0;
                bool ALLIn = false;
                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet dsCharges = new DataSet();
                decimal IATAcharge=0, IATAtax=0, MKTcharge=0, MKTTax=0;
                float OCDC=0, OCDA=0, OCTax=0;
                #region Freight Charges

                string[] param = { "AgentCode", 
                                     "Commodity", 
                                     "Currency", 
                                     "ExecDate", 
                                     "PayMode", 
                                     "AWBOrigin", 
                                     "AWBDest", 
                                     "IsVoided",
                                     "GWt",
                                     "CWt",
                                     "FltDetails",
                                     "SHC",
                                     "AWBPieces",
                                     "ProductType",
                                     "ULDInfo",
                                     "AWBNumber",
                                     "PieceDetails",
                                     "RoutePieceDetails",
                                     "Interline",
                                 "AWBPrefix",
                                 "IsExport",
                                 "Shipper",
                                 "Consignee",
                                 "DVPrice",
                                 "AllInRate"};
                SqlDbType[] sqldbtype = { SqlDbType.VarChar,
                                            SqlDbType.VarChar,
                                            SqlDbType.VarChar,
                                            SqlDbType.DateTime,
                                            SqlDbType.VarChar,
                                            SqlDbType.VarChar,
                                            SqlDbType.VarChar,
                                            SqlDbType.Bit,
                                            SqlDbType.Decimal,
                                            SqlDbType.Decimal,
                                            SqlDbType.VarChar,
                                            SqlDbType.NVarChar,
                                            SqlDbType.Int,
                                            SqlDbType.VarChar,
                                            SqlDbType.NVarChar,
                                            SqlDbType.VarChar,
                                        SqlDbType.NVarChar,
                                        SqlDbType.NVarChar,
                                        SqlDbType.Bit,
                                        SqlDbType.VarChar,
                                        SqlDbType.Bit,
                                        SqlDbType.VarChar,
                                        SqlDbType.VarChar,
                                        SqlDbType.Decimal,
                                        SqlDbType.Bit};
                object[] values = { AgentCode,
                                      Commodity,
                                      Currency,
                                      ExecutionDate,
                                      PayMode,
                                      Origin,
                                      Destination,
                                      IsVoided, 
                                      GrossWt,
                                      ChargeableWt,
                                      FlightDetails,
                                      SHC,
                                      Pieces,
                                      ProductType,
                                      ULDInfo,
                                      AWBNumber,
                                  PieceDetails,
                                  RoutePieceDetails,
                                  Interline,
                                  AWBPrefix,
                                  isExport,
                                  shipper,
                                  consignee,
                                  DVPrice,
                                  AllInRate};
                string storeProcedure="";//"sp_CalculateFreightChargesV1
                try 
                {
                    string[] QName = new string[] { "PType", "Airline", "Process" };
                    object[] QValues = new object[] { "Freight", "", "Booking" };
                    SqlDbType[] QType = new SqlDbType[] {SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarChar };
                    storeProcedure = da.GetStringByProcedure("spGetProcessRateSP", QName, QValues, QType);
                }
                catch (Exception ex) 
                {
                    storeProcedure = "sp_CalculateFreightChargesV1";
                }
                DataSet dsFreight = da.SelectRecords(storeProcedure, param, values, sqldbtype);
                
                if (dsFreight != null)
                {
                    for (int i = 0; i < dsFreight.Tables.Count; i++)
                    {
                        dsCharges.Tables.Add(dsFreight.Tables[i].Copy());
                    }
                    try
                    {
                        if (dsFreight.Tables.Count > 0)
                        {
                            if (dsFreight.Tables[0].Rows.Count > 0)
                            {
                                Freight = Convert.ToDecimal(dsFreight.Tables[0].Rows[0]["Charge"].ToString());
                                ALLIn = Convert.ToBoolean(dsFreight.Tables[0].Rows[0]["IsALLIn"].ToString());
                                MKTcharge = Convert.ToDecimal(dsFreight.Tables[0].Rows[1]["Charge"].ToString());
                                Currency = dsFreight.Tables[0].Rows[0]["Currency"].ToString();
                                //if only MKTcharge exists in system
                                if (Freight == 0 && MKTcharge > 0)
                                    Freight = MKTcharge;
                            }
                        }
                    }
                    catch (Exception ex) { }
                }

                dsFreight = null;

                #endregion

                #region other charges
                //if (!AllInRate)
                //{                    
                    string[] pname = new string[] 
                    {   "AgentCode",
                        "Commodity", 
                        "Currency", 
                        "ExecDate", 
                        "PayMode",  
                        "AWBOrigin", 
                        "AWBDest", 
                        "IsVoided", 
                        "GWt", 
                        "CWt",  
                        "FltDetails",
                        "SHC",
                        "ProductType",
                        "Pieces",
                        "Fright",
                        "DVPrice",
                        "Trigger",                    
                        "AWBNumber",
                        "Handler",
                        "isExport",
                        "XRayReq",
                        "AWBPrefix",
                        "IssueCarrier",
                        "Shipper",
                        "Consignee",
                        "ULDInfo",
                        "SLAC",
                        "PackagingInfo",
                        "AllInRate"
                    };

                    object[] pvalues = new object[]
                    {
                        AgentCode,
                        Commodity,
                        Currency,
                        ExecutionDate,
                        PayMode,
                        Origin,
                        Destination,
                        IsVoided,
                        GrossWt,
                        ChargeableWt,
                        FlightDetails,
                       SHC,
                       ProductType,
                       Pieces,
                       Freight,
                       DVPrice,
                       TriggetPt,
                       AWBNumber,
                       Handler,
                       isExport,
                       XrayReq,
                       AWBPrefix,
                       IssueCarrier,
                       shipper,
                       consignee,
                       ULDInfo,
                       SLAC,
                       PackagingInfo,
                       ALLIn
                    };
                    SqlDbType[] ptype = new SqlDbType[] 
                    {
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.DateTime,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.Bit,
                        SqlDbType.Decimal,
                        SqlDbType.Decimal,
                        SqlDbType.NText,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.Int,
                        SqlDbType.Float,
                        SqlDbType.Float, 
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.Bit,
                        SqlDbType.Bit,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.NText,
                        SqlDbType.Decimal,
                        SqlDbType.VarChar,
                        SqlDbType.Bit

                    };
                    string ocProcedure = "";//sp_CalculateOtherCharges_V2
                    try
                    {
                        string[] QName = new string[] { "PType", "Airline","Process" };
                        object[] QValues = new object[] { "OC", "", "Booking" };
                        SqlDbType[] QType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.VarChar };
                        ocProcedure = da.GetStringByProcedure("spGetProcessRateSP", QName, QValues, QType);
                    }
                    catch (Exception ex)
                    {
                        ocProcedure = "sp_CalculateOtherCharges_V2";
                    }
                    DataSet dsOtherChrgs = da.SelectRecords(ocProcedure, pname, pvalues, ptype);

                    if (dsOtherChrgs != null)
                    {
                        for (int i = 0; i < dsOtherChrgs.Tables.Count; i++)
                        {
                            DataTable dt = new DataTable();
                            dt = dsOtherChrgs.Tables[i].Copy();
                            dt.TableName = "Oth" + i.ToString();
                            dsCharges.Tables.Add(dt);
                            dsCharges.AcceptChanges();

                        }
                        SetOtherChargesSummary(dsOtherChrgs, ref OCDC, ref OCDA, ref OCTax);
                    }
                    dsOtherChrgs = null;
                //}
                dsFreight = null;
              
                    #endregion

                #region Tax Calculation
                
                    string[] parname = new string[] 
                    {   "AgentCode",
                        "Commodity", 
                        "Currency", 
                        "ExecDate", 
                        "PayMode",  
                        "AWBOrigin", 
                        "AWBDest", 
                        "IsVoided", 
                        "GWt", 
                        "CWt",  
                        "FltDetails",
                        "SHC",
                        "ProductType",
                        "Pieces",
                        "Fright",
                        "DVPrice",
                        "Trigger",                    
                        "AWBNumber",
                        "Handler",
                        "isExport",
                        "XRayReq",
                        "AWBPrefix",
                        "IssueCarrier",
                        "MKTFrt",
                        "OCDC",
                        "OCDA",
                        "Commission",
                        "Discount",
                        "SpotFreight",
                        "Shipper",
                        "Consignee"
                    };

                    object[] parvalues = new object[]
                    {
                        AgentCode,
                        Commodity,
                        Currency,
                        ExecutionDate,
                        PayMode,
                        Origin,
                        Destination,
                        IsVoided,
                        GrossWt,
                        ChargeableWt,
                        FlightDetails,
                       SHC,
                       ProductType,
                       Pieces,
                       Freight,
                       DVPrice,
                       TriggetPt,
                       AWBNumber,
                       Handler,
                       isExport,
                       XrayReq,
                       AWBPrefix,
                       IssueCarrier,
                       MKTcharge,
                       OCDC,
                       OCDA,
                       0,
                       0,
                       0,
                       shipper,
                       consignee
                       
                    };
                    SqlDbType[] partype = new SqlDbType[] 
                    {
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.DateTime,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.Bit,
                        SqlDbType.Decimal,
                        SqlDbType.Decimal,
                        SqlDbType.NText,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.Int,
                        SqlDbType.Float,
                        SqlDbType.Float, 
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.Bit,
                        SqlDbType.Bit,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar,
                        SqlDbType.Float,
                        SqlDbType.Float,
                        SqlDbType.Float,
                        SqlDbType.Float,
                        SqlDbType.Float,
                        SqlDbType.Float,
                        SqlDbType.VarChar,
                        SqlDbType.VarChar
                    };
                    string taxProcedure = "";//spCalculateTaxV1
                    try
                    {
                        string[] QName = new string[] { "PType", "Airline", "Process" };
                        object[] QValues = new object[] { "TaxCalculation", "", "Booking" };
                        SqlDbType[] QType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                        taxProcedure = da.GetStringByProcedure("spGetProcessRateSP", QName, QValues, QType);
                    }
                    catch (Exception ex)
                    {
                        taxProcedure = "spCalculateTaxV1";
                    }
                    DataSet dsTaxes = da.SelectRecords(taxProcedure, parname, parvalues, partype);

                    if (dsTaxes != null)
                    {
                        for (int i = 0; i < dsTaxes.Tables.Count; i++)
                        {
                            DataTable dt = new DataTable();
                            dt = dsTaxes.Tables[i].Copy();
                            dt.TableName = "Tax" + i.ToString();
                            dsCharges.Tables.Add(dt);
                            dsCharges.AcceptChanges();

                        }
                    }
                    dsTaxes = null;
                
                dsFreight = null;

                #endregion
                return dsCharges;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
      
    
        /*
         *  Get Viability For Interline
         */

        public DataSet GetViabilityResult(string origin,string destination,string fltnumber,string carrier,float wt,string commodity,
            string SHC,DateTime date,string agentcode,string ProductType,string IssueCarrier,string Currency) 
        {
            DataSet dsResult = null;
            try 
            {
                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                string[] PName = new string[] 
                {
                    "Origin",
                    "Destination",
                    "FltNumber",
                    "Carrier",
                    "Wt",
                    "Commodity",
                    "SHC",
                    "ExecDate",
                    "AgentCode",
                    "ProductType",
                    "IssueCarrier",
                    "Currency"
                };
                SqlDbType[] PType = new SqlDbType[] 
                {
                    SqlDbType.VarChar,  
                    SqlDbType.VarChar,  
                    SqlDbType.VarChar,  
                    SqlDbType.VarChar,  
                    SqlDbType.Float,
                    SqlDbType.VarChar,  
                    SqlDbType.VarChar,  
                    SqlDbType.DateTime, 
                    SqlDbType.VarChar,  
                    SqlDbType.VarChar,  
                    SqlDbType.VarChar,  
                    SqlDbType.VarChar
                };
                object[] PValue = new object[] 
                {
                    origin,
                    destination,
                    fltnumber,
                    carrier,
                    wt,
                    commodity,
                    SHC,
                    date,
                    agentcode,
                    ProductType,
                    IssueCarrier,
                    Currency
                };
                dsResult = objSQL.SelectRecords("sp_GetViabilityRateLine_Booking", PName, PValue, PType);

            }
            catch (Exception ex) 
            {
                dsResult = null;
            }
            return dsResult;
        }

        #region SetOtherChargesSummary
        public void SetOtherChargesSummary(DataSet dsDetails, ref float OCDC, ref float OCDA, ref float OCTax)
        {
            
            DataSet dsDetailsFinal = null;
            try
            {   
                dsDetailsFinal = dsDetails.Clone();
                ArrayList list = new ArrayList();

                foreach (DataRow row in dsDetails.Tables[0].Rows)
                {
                    if (row["Charge Type"].ToString() == "DC" || row["Charge Type"].ToString() == "DA")
                    {
                        if (!list.Contains(row["Charge Head Code"].ToString()))
                        {

                            DataRow newrow = dsDetailsFinal.Tables[0].NewRow();
                            for (int i = 0; i < dsDetailsFinal.Tables[0].Columns.Count; i++)
                            {
                                newrow[i] = row[i];
                            }

                            dsDetailsFinal.Tables[0].Rows.Add(newrow);
                            list.Add(row["Charge Head Code"].ToString());
                        }
                        else
                        {
                            foreach (DataRow rw in dsDetailsFinal.Tables[0].Rows)
                            {

                                if (rw["Charge Head Code"].ToString() == row["Charge Head Code"].ToString())
                                {
                                    rw["Charge"] = "" + (float.Parse(rw["Charge"].ToString()) + float.Parse(row["Charge"].ToString()));
                                    rw["Tax"] = "" + (float.Parse(rw["Tax"].ToString()) + float.Parse(row["Tax"].ToString()));
                                    rw["Discount"] = "" + (float.Parse(rw["Discount"].ToString()) + float.Parse(row["Discount"].ToString()));
                                    rw["Commission"] = "" + (float.Parse(rw["Commission"].ToString()) + float.Parse(row["Commission"].ToString()));
                                }
                            }
                        }
                    }
                    else
                    {
                        DataRow newrow = dsDetailsFinal.Tables[0].NewRow();
                        for (int i = 0; i < dsDetailsFinal.Tables[0].Columns.Count; i++)
                        {
                            newrow[i] = row[i];
                        }
                        dsDetailsFinal.Tables[0].Rows.Add(newrow);
                    }
                }
                foreach (DataRow dr in dsDetailsFinal.Tables[0].Rows)
                {
                    if (dr["Charge Type"].ToString() == "DC")
                    {
                        OCDC += float.Parse(dr["Charge"].ToString());
                        OCTax += float.Parse(dr["Tax"].ToString());

                    }
                    else if (dr["Charge Type"].ToString() == "DA")
                    {
                        OCDA += float.Parse(dr["Charge"].ToString());
                        OCTax += float.Parse(dr["Tax"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                dsDetails = null;
                dsDetailsFinal = null;
            }
            finally
            {
                if (dsDetails != null)
                    dsDetails.Dispose();
                if (dsDetailsFinal != null)
                    dsDetailsFinal.Dispose();
            }
        }
        #endregion

        public DataSet GetReprocessRates(string AWBNumber, string Trigger)
        {
            DataSet dsResult = null;
            try
            {
                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                string[] PName = new string[] { "AWBNumber", "Trigger" };
                object[] PValue = new object[] { AWBNumber, Trigger };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };

                dsResult = objSQL.SelectRecords("sp_CalculateAWBRates", PName, PValue, PType);
            }
            catch (Exception ex)
            {
                dsResult = null;
            }
            return dsResult;
        }

        #region GetRoundoffvalue
        public string GetRoundoffvalue(string Origin, string Destination, string param, string value, string date,string agent,string shipper,string consignee)
        {
            string val = "";
            SQLServer objSQL = new SQLServer(Global.GetConnectionString());
            try
            {
                if (value.Length > 0 && value != "")
                {

                    string[] PName = new string[] { "Origin", "Destination", "Parameter", "Value", "Date", "Agent", "Shipper", "Consignee"};
                    object[] PValue = new object[] { Origin, Destination, param, value, date,agent,shipper,consignee };
                    SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                    val = objSQL.GetStringByProcedure("spGetRoundingValue", PName, PValue, PType);
                }
            }
            catch (Exception ex) { }
            finally
            {
                objSQL = null;
            }
            return val;
        }
        #endregion
    }


}
