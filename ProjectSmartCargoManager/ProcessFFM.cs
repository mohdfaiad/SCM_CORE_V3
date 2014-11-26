using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using QID.DataAccess;
//using clsDataLib;
using ProjectSmartCargoManager;

/// <summary>
/// Summary description for ProcessFFM
/// </summary>
public class ProcessFFM
{
    
    private string constr;
    WebSerBookingBAL _objBooking = new WebSerBookingBAL();

    #region Constructor
    public ProcessFFM()
    {
        //
        // TODO: Add constructor logic here
        //
        constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
    }
    #endregion

    #region ProcessffmMessage
    public void ProcessffmMessage(ref MessageData.ffminfo ffmdata, ref MessageData.consignmnetinfo[] consinfo, ref MessageData.unloadingport[] unloadingport)
    {
        try
        {
            string ffmType = "";
            string Fltdest = unloadingport[unloadingport.Length - 1].unloadingairport;
            DateTime dtFlt;
            dtFlt = Convert.ToDateTime(ffmdata.fltdate + "/" + ffmdata.month);


            if (consinfo.Length > 0)
            {

                int c = 0;
                while (c < consinfo.Length)
                {
                    if (CreateBooking_ImportFFM(ffmdata.fltairportcode, consinfo[c].dest, ffmdata.fltairportcode, Fltdest, consinfo[c].awbnum, consinfo[c].splhandling.Trim(',') == "" ? "GEN" : consinfo[c].splhandling.Trim(','), consinfo[c].volumeamt, consinfo[c].pcscnt, consinfo[c].weight, consinfo[c].manifestdesc == "" ? "GEN" : consinfo[c].manifestdesc, dtFlt, consinfo[c].airlineprefix, ffmdata.carriercode + ffmdata.fltnum) == true)
                    {
                        // Create Booking of AWB's
                        if ((StoreImportFFM_Details(consinfo[c].origin, consinfo[c].dest, ffmdata.fltairportcode, Fltdest, consinfo[c].awbnum, consinfo[c].splhandling.Trim(',') == "" ? "GEN" : consinfo[c].splhandling.Trim(','), consinfo[c].volumeamt, consinfo[c].pcscnt, consinfo[c].weight, consinfo[c].manifestdesc == "" ? "GEN" : consinfo[c].manifestdesc, dtFlt) == true) &&
                            (ULDawbAssociation(ffmdata.carriercode + ffmdata.fltnum, consinfo[c].origin, consinfo[c].dest, consinfo[c].awbnum, consinfo[c].pcscnt, consinfo[c].weight, dtFlt) == true))
                        { }
                            //Log.WriteLog("AWB Details Added & Booked [" + DateTime.Now + "]");
                        //else
                            //Log.WriteLog("Failed Booking AWB Details[" + DateTime.Now + "]");

                    }
                    //else
                        //Log.WriteLog("Failed Adding AWB Details[" + DateTime.Now + "]");
                    c++;
                }


            }
            //else
            //{

            if (StoreImportFFM_Summary(ffmdata.carriercode + ffmdata.fltnum, ffmdata.fltairportcode, Fltdest, dtFlt) == true)
            { }    
            //Log.WriteLog("FFM Flight Summary Added [" + DateTime.Now + "]");
            //else
                //Log.WriteLog("Failed adding FFM Flight Summary[" + DateTime.Now + "]");

            //}
        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion

    #region Manifest
    public bool StoreImportFFM_Summary(string FlightNo, string POL, string POU, DateTime FltDate)
    {
        try
        {

            SQLServer db = new SQLServer(Global.GetConnectionString());
            string[] param = { "FLTno", "POL", "POU", "FLTDate" };
            SqlDbType[] sqldbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime };
            object[] values = { FlightNo, POL, POU, FltDate };

            bool res;
            res = db.InsertData("SPImpManiSaveManifest", param, sqldbtypes, values);

            return res;
        }
        catch (Exception ex)
        {
            //Log.WriteLog("Error saving ImportFFM [" + DateTime.Now + "]");
            return false;
        }
    }

    public bool StoreImportFFM_Details(string POL, string POU, string ORG, string DES, string AWBno, string SCC, string VOL, string PCS, string WGT, string Desc, DateTime FltDate)
    {
        try
        {

            SQLServer db = new SQLServer(Global.GetConnectionString());
            string[] param = { "POL", "POU", "ORG", "DES", "AWBno", "SCC", "VOL", "PCS", "WGT", "Desc", "FLTDate" };
            SqlDbType[] sqldbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime };
            object[] values = { POL, POU, ORG, DES, AWBno, SCC, VOL, PCS, WGT, Desc, FltDate };

            bool res;
            res = db.InsertData("SPImpManiSaveManifestDetails", param, sqldbtypes, values);

            return res;
        }
        catch (Exception ex)
        {
            //Log.WriteLog("Error saving ImportFFM [" + DateTime.Now + "]");
            return false;
        }
    }


    public bool ULDawbAssociation(string FltNo, string POL, string POU, string AWBno, string PCS, string WGT, DateTime FltDate)
    {
        try
        {

            SQLServer db = new SQLServer(Global.GetConnectionString());
            string[] param = { "ULDtripid", "ULDNo", "AWBNumber", "POL", "POU", "FltNo", "Pcs", "Wgt", "AvlPcs", "AvlWgt", "Updatedon", "Updatedby", "Status", "Manifested", "FltDate" };

            int _pcs = int.Parse(PCS);
            float _wgt = float.Parse(WGT);
            SqlDbType[] sqldbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar
                                             , SqlDbType.Int, SqlDbType.Float, SqlDbType.Int, SqlDbType.Float,SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.Bit,SqlDbType.Bit, SqlDbType.DateTime };
            object[] values = { "", "", AWBno, POL, POU, FltNo, 0, 0, _pcs, _wgt, DateTime.Now, "FFM", false, false, FltDate };

            bool res;
            res = db.InsertData("SPImpManiSaveUldAwbAssociation", param, sqldbtypes, values);

            return res;
        }
        catch (Exception ex)
        {
            //Log.WriteLog("Error saving ImportFFM [" + DateTime.Now + "]");
            return false;
        }
    }


    #endregion

    #region Booking

    public bool CreateBooking_ImportFFM(string POL, string POU, string ORG, string DES, string AWBno, string SCC, string VOL, string PCS, string WGT, string Desc, DateTime FLTDate, string preAWB, string FltNo)
    {
        try
        {
            DataSet ds = new DataSet();
            #region Geting Agent Code Based on AWB
            string AgentCode = "";
            string AgentName = "";

            SQLServer da = new SQLServer(constr);
            //DataSet ds = da.SelectRecords("SP_GetAWBInfoToProcessRates", "AWBNumber", AWBNo, SqlDbType.VarChar);
            string agentQuery = "select Alevel from StockAllocation where convert(int,SUBSTRING('" + AWBno + "',0,8)) between AFrom and ATo and ParType='City'";
            AgentCode = da.GetString(agentQuery);
            #endregion

            #region Geting Agent Nmae Based on Agent Code
            if (AgentCode != "")
            {
                da = new SQLServer(constr);
                string agentNameQuery = "select agentname from agentmaster where agentcode='" + AgentCode + "'";
                AgentName = da.GetString(agentNameQuery);

            }
            #endregion

            #region Get ScheduleID
            string SchId = "";
            string scheduleIdQuery = "select max(a.ScheduleID) from AirlineScheduleRoute a inner join AirlineScheduleRoute b on a.ScheduleID=b.ScheduleID";
            scheduleIdQuery += " where a.FlightID='" + FltNo + "' and  a.Source='" + ORG + "' and b.Dest='" + DES + "' and a.IsActive=1  and ('" + FLTDate + "' between a.FromDt and b.ToDt)";
            SchId = da.GetString(scheduleIdQuery);
            #endregion
            decimal IATACharge = 0; decimal IATATax = 0; decimal MKTRate = 0; decimal MKTTax = 0;
            float OCDC, OCDA, OCTax;
            OCDC = OCDA = OCTax = 0;
            DataSet dsOther = null;
            bool res;
            //ds = ObjBooking.ProcessRates(AWBno, SCC, Decimal.Parse(WGT),ORG,DES,FLTDate,FltNo);
            // ds = ObjBooking.ProcessRatesByFlightNo(AWBno, SCC, Decimal.Parse(WGT), FltNo, ORG, DES, FLTDate);
            res = _objBooking.GetAWBCharges(ORG, DES, FltNo, SCC, AgentCode, preAWB, Decimal.Parse(WGT), ref IATACharge, ref  IATATax, ref MKTRate, ref MKTTax, ref OCDC, ref  OCDA, ref OCTax, ref dsOther, FLTDate);
            if (dsOther.Tables[0].Rows.Count == 0)
            {
                //Log.WriteLog("Error in Booking AWB" + DateTime.Now + "]");
                return false;
            }
            else
            {


                #region Save AWB Summary
                //Save AWB Summary data in database for new AWB.

                try
                {


                    object[] awbInfo = new object[23];
                    int i = 0;



                    awbInfo.SetValue(0, i);
                    i++;

                    awbInfo.SetValue("AWB", i);
                    i++;

                    awbInfo.SetValue(preAWB, i);
                    i++;

                    awbInfo.SetValue(AWBno, i);
                    i++;

                    awbInfo.SetValue(int.Parse(PCS), i);
                    i++;

                    awbInfo.SetValue(float.Parse(WGT), i);
                    i++;


                    awbInfo.SetValue(float.Parse(VOL.Length>0?VOL:"0"), i);
                    i++;

                    awbInfo.SetValue(float.Parse(WGT), i);
                    i++;

                    awbInfo.SetValue(ORG, i);
                    i++;


                    awbInfo.SetValue(DES, i);
                    i++;

                    awbInfo.SetValue(AgentCode, i);
                    i++;

                    awbInfo.SetValue(AgentName, i);
                    i++;

                    awbInfo.SetValue(5, i);
                    i++;

                    awbInfo.SetValue("", i);
                    i++;

                    //14
                    awbInfo.SetValue(DateTime.Now.ToString("dd/MM/yyyy"), i);
                    i++;

                    //15
                    awbInfo.SetValue("FFM", i);
                    i++;

                    //16
                    awbInfo.SetValue(ORG, i);
                    i++;


                    awbInfo.SetValue("FFM", i);
                    i++;


                    awbInfo.SetValue(DateTime.Now, i);
                    i++;

                    awbInfo.SetValue(false, i);
                    i++;

                    awbInfo.SetValue(false, i);
                    i++;


                    awbInfo.SetValue(false, i);
                    i++;

                    awbInfo.SetValue(false, i);
                    i++;



                    //Update database.
                    if (_objBooking.SaveAWBSummary(awbInfo) < 0)
                    {
                        // "AWB Save failed. Please try again...";
                        return false;
                    }
                    //Updating AWB status To 'Executed'
                    else
                    {
                        da = new SQLServer(constr);
                        string UpdateAWBstatus_Query = "update  awbsummarymaster set AWBStatus='E' where awbnumber='" + AWBno + "'";
                        da.UpdateData(UpdateAWBstatus_Query);

                    }

                }
                catch (Exception ex)
                {
                    //Log.WriteLog("Error saving AWB Summary" + ex.Message + DateTime.Now + "]");
                    return false;
                }
                #endregion Save AWB Summary


                #region Save AWB Material
                //Save material information.
                try
                {

                    //Add material info...

                    int i = 0;

                    object[] awbInfo = new object[10];

                    awbInfo.SetValue(AWBno, i);
                    i++;
                    awbInfo.SetValue(SCC, i);
                    i++;
                    awbInfo.SetValue(int.Parse(PCS), i);
                    i++;
                    awbInfo.SetValue(float.Parse(WGT), i);
                    i++;
                    awbInfo.SetValue("0", i);
                    i++;
                    awbInfo.SetValue(float.Parse(VOL.Length > 0 ? VOL : "0"), i);
                    i++;
                    awbInfo.SetValue(float.Parse(WGT), i);
                    i++;
                    awbInfo.SetValue("FFM", i);
                    i++;
                    awbInfo.SetValue(DateTime.Now.Date, i);
                    i++;
                    awbInfo.SetValue(Desc, i);
                    i++;


                    //Call SP to update database.
                    if (_objBooking.SaveAWBMaterial(awbInfo) < 0)
                    {
                        //Log.WriteLog("Error saving AWB Material Information [" + DateTime.Now + "]");
                        return false;
                    }


                }
                catch (Exception ex)
                {
                    //Log.WriteLog("Error saving AWB Material Information" + ex.Message + "[" + DateTime.Now + "]");
                    return false;
                }
                #endregion Save AWB Material

                #region Save AWB Routing
                //Save routing information.
                try
                {
                    object[] awbInfo = new object[15];
                    int i = 0;
                    awbInfo.SetValue(AWBno, i);
                    i++;
                    awbInfo.SetValue(ORG, i);
                    i++;
                    awbInfo.SetValue(DES, i);
                    i++;
                    awbInfo.SetValue(FltNo, i);
                    i++;
                    awbInfo.SetValue(FLTDate.ToString("dd/MM/yyyy"), i);
                    i++;
                    awbInfo.SetValue(int.Parse(PCS), i);
                    i++;
                    awbInfo.SetValue(float.Parse(WGT), i);
                    i++;

                    awbInfo.SetValue("C", i);
                    i++;
                    awbInfo.SetValue("Y", i);
                    i++;
                    awbInfo.SetValue(int.Parse(PCS), i);
                    i++;
                    awbInfo.SetValue(float.Parse(WGT), i);
                    i++;

                    awbInfo.SetValue("FFM", i);
                    i++;
                    awbInfo.SetValue(DateTime.Now.Date, i);
                    i++;
                    awbInfo.SetValue(int.Parse(SchId), i);
                    i++;

                    awbInfo.SetValue(float.Parse(WGT), i);
                    i++;



                    if (_objBooking.SaveAWBRoute(awbInfo) < 0)
                    {
                        //Log.WriteLog("Error saving Route information [" + DateTime.Now + "]");
                        return false;
                    }
                }

                catch (Exception ex)
                {
                    //Log.WriteLog("Error saving Route information " + ex.Message + "[" + DateTime.Now + "]");
                    return false;
                }
                #endregion Save AWB Routing

                #region AWB Rates

                //for (int k = 0; k < GRDRates.Rows.Count; k++)
                //{
                string CommCode = SCC;// tBox)GRDRates.Rows[k].FindControl("TXTCommCode")).Text;
                string PayMode = "PP"; //((DropDownList)GRDRates.Rows[k].FindControl("ddlPayMode")).Text;

                int Pcs = int.Parse(PCS);//int.Parse(((TextBox)GRDRates.Rows[k].FindControl("TXTPcs")).Text);

                decimal Wt = Decimal.Parse(WGT); //Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTWt")).Text);
                decimal FrIATA = IATACharge; //Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTFrIATA")).Text);
                decimal FrMKT = MKTRate; //Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTFrMKT")).Text);
                decimal ValCharge = 0; //Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTValCharg")).Text);
                decimal OcDueCar = Convert.ToDecimal(OCDC); //Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTOcDueCar")).Text);
                decimal OcDueAgent = Convert.ToDecimal(OCDA); //Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTOcDueAgent")).Text);
                decimal SpotRate = 0; //Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTSpotRate")).Text);
                decimal DynRate = 0; //Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTDynRate")).Text);
                decimal ServiceTax = IATATax + Convert.ToDecimal(OCTax); //Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTServiceTax")).Text);
                decimal Total = IATACharge + OcDueCar + OcDueAgent + ServiceTax; //Convert.ToDecimal(((TextBox)GRDRates.Rows[k].FindControl("TXTTotal")).Text);

                object[] values = { AWBno, CommCode, PayMode, Pcs, Wt, FrIATA, FrMKT, ValCharge, OcDueCar, OcDueAgent, SpotRate, DynRate, ServiceTax, Total };


                if (!_objBooking.SaveAWBRates(values))
                {
                    //Log.WriteLog("Error Saving Rate information" + DateTime.Now + "]");

                    return false;
                }

                //}


                DataSet dsDetails = dsOther.Copy();
                foreach (DataRow row in dsDetails.Tables[0].Rows)
                {
                    //dsDetailsRow["Charge Type"] = "RateLineIATA";
                    if (row["Charge Type"].ToString().Trim() == "RateLineIATA")
                    {

                        // Charge Head Code,Charge Type,Charge,Tax%,Tax,DiscountPercent,Discount,CommPercent,Commission



                        object[] value = { AWBno,int.Parse(row["Charge Head Code"].ToString()),
                                           "IATA", decimal.Parse(row["DiscountPercent"].ToString()), 
                                           decimal.Parse(row["CommPercent"].ToString()), decimal.Parse(row["TaxPercent"].ToString()), 
                                           decimal.Parse(row["Discount"].ToString()), decimal.Parse(row["Commission"].ToString()), 
                                           decimal.Parse(row["Tax"].ToString()),decimal.Parse(row["Charge"].ToString()),row["Commodity Code"].ToString() };

                        if (!_objBooking.SaveAWBRatesDetails("Freight", value))
                        {
                            //Log.WriteLog("Error Saving Rate information Other Charge" + DateTime.Now + "]");
                            return false;
                        }
                    }
                    else if (row["Charge Type"].ToString().Trim() == "RateLineMKT")
                    {

                        // Charge Head Code,Charge Type,Charge,Tax%,Tax,DiscountPercent,Discount,CommPercent,Commission

                        object[] value = { AWBno,int.Parse(row["Charge Head Code"].ToString()),
                                           "MKT", decimal.Parse(row["DiscountPercent"].ToString()), 
                                           decimal.Parse(row["CommPercent"].ToString()), decimal.Parse(row["TaxPercent"].ToString()), 
                                           decimal.Parse(row["Discount"].ToString()), decimal.Parse(row["Commission"].ToString()), 
                                           decimal.Parse(row["Tax"].ToString()),decimal.Parse(row["Charge"].ToString()),row["Commodity Code"].ToString() };

                        if (!_objBooking.SaveAWBRatesDetails("Freight", value))
                        {
                            //Log.WriteLog("Error Saving Rate information Other Charge" + DateTime.Now + "]");
                            return false;
                        }
                    }
                    else
                    {
                        object[] value = { AWBno,row["Charge Head Code"].ToString(),
                                           row["Charge Type"].ToString().Trim(), decimal.Parse(row["DiscountPercent"].ToString()), 
                                           decimal.Parse(row["CommPercent"].ToString()), decimal.Parse(row["TaxPercent"].ToString()), 
                                           decimal.Parse(row["Discount"].ToString()), decimal.Parse(row["Commission"].ToString()), 
                                           decimal.Parse(row["Tax"].ToString()),decimal.Parse(row["Charge"].ToString()),row["Commodity Code"].ToString() };

                        if (!_objBooking.SaveAWBRatesDetails("OC", value))
                        {
                            //Log.WriteLog("Error Saving Rate information Other Charge" + DateTime.Now + "]");
                            return false;
                        }


                    }

                }

                #endregion

                #region AWB Dimensions


                //try
                //{



                //        object[] value ={AWBno,0,0,0,0,int.Parse(PCS),"cms"};

                //        if (!ObjBooking.SaveAWBDimensions(value))
                //        {
                //            Log.WriteLog("Error saving Dimensions [" + DateTime.Now + "]");
                //            return false;

                //        }

                //}
                //catch (Exception ex)
                //{

                //    Log.WriteLog("Error saving Dimensions " + ex.Message + "[" + DateTime.Now + "]");
                //    return false;  
                //}

                #endregion

                #region AWB Shipper/Consignee

                try
                {

                    if (!_objBooking.SaveAWBShipperConsignee(new object[] { AWBno, AgentName, "", "", "", AgentName, "", "", "" }))
                    {
                        //Log.WriteLog("Error saving Shipper/Consignee [" + DateTime.Now + "]");
                        return false;

                    }
                }
                catch (Exception ex)
                {


                    //Log.WriteLog("Error saving Shipper/Consignee " + ex.Message + "[" + DateTime.Now + "]");
                    return false;
                }

                #endregion


                #region DeductAvailableCount //Currently Disabled-Will be activated When Poorna implement In booking

                //  string DeductAvailableCount_Query = "update stockallocation set Available=Available-1 where convert(int,SUBSTRING('"+AWBno+"',0,8)) between AFrom and ATo and ParType='City'";

                //if (da.UpdateData(DeductAvailableCount_Query)==false)
                //{
                //    Log.WriteLog("Error deducting AWB from Stock[ " + DateTime.Now+ "]");
                //    return false;
                //}
                #endregion

            }

            return true;

        }
        catch (Exception)
        {

            return false;
        }


    }
    #endregion

}
