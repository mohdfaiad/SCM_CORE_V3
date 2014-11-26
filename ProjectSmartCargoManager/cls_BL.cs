using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
//using clsDataLib;
using BAL;
using System.Text;
using System.IO;
using QID.DataAccess;
using System.Globalization;
/// <summary>
/// Summary description for cls_BL
/// </summary>
public class cls_BL
{

    #region Constructor
    public cls_BL()
	{
		//
		// TODO: Add constructor logic here
		//
    }
    #endregion

    #region getAcceptedBookingData_ByVijay
    public static DataSet getAcceptedBookingData(string pName)
    {
        DataSet ds = new DataSet();
        try
        {
            SQLServer dtb = new SQLServer(Global.GetConnectionString());
            string procedure = pName;

            ds = dtb.SelectRecords(pName);

        }
        catch (Exception ex)
        { }
        return ds;
    }
    #endregion

    #region EncodeFFRAndPrepareMsg
    public static bool elcodeFFRAndPrepareMsg(DataSet dsFFR, string fromEmailID, string toEmailID)
    {
        bool flag = false;
        try
        {

            MessageData.ffrinfo objFFRInfo = new MessageData.ffrinfo("");
            MessageData.ULDinfo[] objULDInfo = new MessageData.ULDinfo[0];

            if (dsFFR != null)
            {
                if (dsFFR.Tables.Count > 0)
                {
                    if (dsFFR.Tables[0].Rows.Count > 0)
                    {
                        DataRow drAWBRateMaster = dsFFR.Tables[0].Rows[0];
                        DataRow drAWBRouteMaster = dsFFR.Tables[1].Rows[0];
                        DataRow drAWBShipperConsigneeDetails = dsFFR.Tables[2].Rows[0];
                        DataRow drAWBSummaryMaster = dsFFR.Tables[3].Rows[0];


                        DateTime dtTo = new DateTime(); DateTime dtfrom = new DateTime();
                        string dt = (drAWBRouteMaster["FltDate"].ToString());


                        //dt = dt + " " + DateTime.Now.ToShortTimeString();
                        //dtfrom = DateTime.ParseExact(dt,"dd-MM-yyyy",null);
                        dtTo = DateTime.ParseExact(drAWBRouteMaster["FltDate"].ToString(), "dd/MM/yyyy", null);
                        //ToDt = dt.ToString();
                        string day = dt.Substring(0, 2);
                        string mon = dtTo.ToString("MMM");
                        //string mon = dt.Substring(3, 2);
                        string yr = dt.Substring(6, 4);

                        #region PrepareFFRStructureObject

                        //line 1 
                        objFFRInfo.ffrversionnum = "6";

                        #region Consigment Section
                        //line 2
                        //objFFRInfo.airlineprefix= drAWBSummaryMaster["AWBPrefix"].ToString();
                        //objFFRInfo.awbnum=drAWBSummaryMaster["AWBNumber"].ToString();;
                        //objFFRInfo.origin = drAWBSummaryMaster["OriginCode"].ToString();
                        //objFFRInfo.dest=drAWBSummaryMaster["DestinationCode"].ToString(); ;
                        //objFFRInfo.consigntype="T";
                        //objFFRInfo.pcscnt = drAWBRateMaster["Pieces"].ToString();
                        //objFFRInfo.weightcode="K";
                        //objFFRInfo.weight = drAWBSummaryMaster["GrossWeight"].ToString();
                        //objFFRInfo.volumecode="";
                        //objFFRInfo.volumeamt="";
                        //objFFRInfo.densityindicator="";
                        //objFFRInfo.densitygrp="";
                        //objFFRInfo.shpdesccode="";
                        //objFFRInfo.numshp = "";//drAWBRateMaster["Pieces"].ToString();
                        ////objFFRInfo.manifestdesc = drAWBRateMaster["CommodityDesc"].ToString().Length > 1 ? drAWBRateMaster["CommodityDesc"].ToString() : "GEN";
                        //objFFRInfo.manifestdesc = drAWBRateMaster["CommodityCode"].ToString().Length > 1 ? drAWBRateMaster["CommodityCode"].ToString() : "GEN";
                        //objFFRInfo.manifestdesc += "-";
                        //objFFRInfo.manifestdesc += drAWBRateMaster["CommodityDesc"].ToString().Length > 0 ? drAWBRateMaster["CommodityDesc"].ToString() : "GEN";
                        //objFFRInfo.splhandling="";
                        #endregion


                        //line 3
                        #region FLTROUTE
                        //objFFRInfo.carriercode=drAWBSummaryMaster["AWBPrefix"].ToString();
                        // objFFRInfo.fltnum=drAWBRouteMaster["FltNumber"].ToString().Substring(2);
                        // objFFRInfo.date=day.ToString();
                        // objFFRInfo.month=mon.ToString();
                        // objFFRInfo.fltdept=drAWBRouteMaster["FltOrigin"].ToString();
                        // objFFRInfo.fltarrival = drAWBRouteMaster["FltDestination"].ToString();
                        // objFFRInfo.spaceallotmentcode="";
                        // objFFRInfo.allotidentification="LL";
                        // try
                        // {
                        //     string AWBStatus = "";
                        //     AWBStatus = drAWBRouteMaster["Status"].ToString();
                        //     if (AWBStatus.Trim() != "")
                        //     {
                        //         if (AWBStatus.Trim() == "Q")
                        //         {
                        //             objFFRInfo.spaceallotmentcode = "LL";
                        //         }
                        //         else if (AWBStatus.Trim() == "C")
                        //         {
                        //             objFFRInfo.spaceallotmentcode = "KK";
                        //         }
                        //     }
                        //     else
                        //     {
                        //         objFFRInfo.spaceallotmentcode = drAWBRouteMaster["Status"].ToString(); ;
                        //     }
                        // }
                        // catch (Exception ex)
                        // { }
                        #endregion

                        //line 4
                        objFFRInfo.noofuld = "";
                        //line 5 
                        objFFRInfo.specialservicereq1 = "";
                        objFFRInfo.specialservicereq2 = "";
                        //line 6
                        objFFRInfo.otherserviceinfo1 = "";
                        objFFRInfo.otherserviceinfo2 = "";
                        //line 7
                        objFFRInfo.bookingrefairport = drAWBSummaryMaster["OriginCode"].ToString();
                        objFFRInfo.officefundesignation = "FF";
                        objFFRInfo.companydesignator = "XX";
                        objFFRInfo.bookingfileref = "";
                        objFFRInfo.participentidetifier = "";
                        objFFRInfo.participentcode = "";
                        objFFRInfo.participentairportcity = "";
                        // objFFRInfo.participentairportcity = drAWBSummaryMaster["OriginCode"].ToString();
                        // objFFRInfo.participentcode = "";
                        // objFFRInfo.participentidetifier = "";

                        //line 8
                        #region Dimension
                        //if (dsFFR.Tables.Count > 3)
                        //{
                        //    objFFRInfo.line8weightcode = "K";
                        //    objFFRInfo.line8weight = drAWBRateMaster["GrossWeight"].ToString(); ;
                        //    //objFFRInfo.line8mesurunitcode = "";
                        //    //objFFRInfo.line8length = "";
                        //    //objFFRInfo.line8width = "";
                        //    //objFFRInfo.line8height = "";
                        //    //objFFRInfo.line8piecenum = "";
                        //    // objFFRInfo.line8height = "";
                        //    //objFFRInfo.line8length = "";
                        //    //objFFRInfo.line8mesurunitcode = "";
                        //    //objFFRInfo.line8piecenum = drAWBRateMaster["Pieces"].ToString();
                        //    //objFFRInfo.line8weight = 
                        //    //objFFRInfo.line8weightcode = "K";
                        //    //objFFRInfo.line8width = "";
                        //}

                        // Dimensions

                        //if (dsFFR.Tables[4].Rows.Count > 0)
                        //{
                        //    DataRow drAWBDimensions = dsFFR.Tables[4].Rows[0];
                        //    if (drAWBDimensions[0].ToString().Trim() != "")
                        //    {
                        //        if (drAWBDimensions[0].ToString().Trim().ToUpper() == "CMS")
                        //        {
                        //            objFFRInfo.line8mesurunitcode = "CMT";
                        //        }
                        //        else if (drAWBDimensions[0].ToString().Trim().ToUpper() == "INCHES")
                        //        {
                        //            objFFRInfo.line8mesurunitcode = "INH";
                        //        }
                        //    }

                        //    objFFRInfo.line8length = drAWBDimensions[1].ToString();
                        //    objFFRInfo.line8width = drAWBDimensions[2].ToString();
                        //    objFFRInfo.line8height = drAWBDimensions[3].ToString();
                        //    objFFRInfo.line8piecenum = "";
                        //}
                        #endregion
                        //line 9 
                        objFFRInfo.servicecode = "";
                        objFFRInfo.rateclasscode = "";
                        objFFRInfo.commoditycode = "";
                        //objFFRInfo.rateclasscode = "";
                        //objFFRInfo.servicecode = "A";
                        //objFFRInfo.commoditycode = drAWBRateMaster["CommodityCode"].ToString();

                        //line 10                      
                        objFFRInfo.shipperaccnum = "";
                        objFFRInfo.shippername = drAWBShipperConsigneeDetails["ShipperName"].ToString();
                        objFFRInfo.shipperadd = drAWBShipperConsigneeDetails["ShipperAddress"].ToString();
                        objFFRInfo.shipperplace = "";//drAWBShipperConsigneeDetails["ShipperAddress"].ToString();
                        objFFRInfo.shipperstate = "";
                        objFFRInfo.shippercountrycode = drAWBShipperConsigneeDetails["ShipperCountry"].ToString().Substring(0, 2);
                        objFFRInfo.shipperpostcode = "";
                        objFFRInfo.shippercontactidentifier = "TE";
                        objFFRInfo.shippercontactnum = drAWBShipperConsigneeDetails["ShipperTelephone"].ToString();

                        //line 11
                        objFFRInfo.consaccnum = "";
                        objFFRInfo.consname = drAWBShipperConsigneeDetails["ConsigneeName"].ToString();
                        objFFRInfo.consadd = drAWBShipperConsigneeDetails["ConsigneeAddress"].ToString();
                        objFFRInfo.consplace = drAWBShipperConsigneeDetails["ConsigneeAddress"].ToString();
                        objFFRInfo.consstate = "";
                        objFFRInfo.conscountrycode = drAWBShipperConsigneeDetails["ConsigneeCountry"].ToString().Substring(0, 2);
                        objFFRInfo.conspostcode = "";
                        objFFRInfo.conscontactidentifier = "TE";
                        objFFRInfo.conscontactnum = drAWBShipperConsigneeDetails["ConsigneeTelephone"].ToString();

                        //line 12
                        objFFRInfo.custaccnum = "";
                        objFFRInfo.iatacargoagentcode = drAWBSummaryMaster["AgentCode"].ToString();
                        objFFRInfo.cargoagentcasscode = "";
                        objFFRInfo.custparticipentidentifier = "";
                        objFFRInfo.custname = "";
                        objFFRInfo.custplace = "";
                        //line 13
                        objFFRInfo.shiprefnum = "";
                        objFFRInfo.supplemetryshipperinfo1 = "";
                        objFFRInfo.supplemetryshipperinfo2 = "";


                        #endregion

                        string strMsg = cls_Encode_Decode.encodeFFRforsend(ref objFFRInfo, ref objULDInfo);
                        if (strMsg != null)
                        {
                            if (strMsg.Trim() != "")
                            {
                                flag = InsertFFRToOutBox(strMsg, fromEmailID, toEmailID);
                                //flag = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return flag;
    }
    #endregion

    #region InsertFFRToOutBox
    private static bool InsertFFRToOutBox(string Body, string FromId, string ToID)
    {
        //SQLServer da = new SQLServer(constr);
        SQLServer dtb = new SQLServer(Global.GetConnectionString());
        DataSet ds = new DataSet();
        DataSet objDS = null;
        bool flag = false;
        try
        {
            string[] Pname = new string[3];
            object[] Pvalue = new object[3];
            SqlDbType[] Ptype = new SqlDbType[3];

            Pname[0] = "body";
            Ptype[0] = SqlDbType.VarChar;
            Pvalue[0] = Body;

            Pname[1] = "fromId";
            Ptype[1] = SqlDbType.VarChar;
            Pvalue[1] = FromId;

            Pname[2] = "toId";
            Ptype[2] = SqlDbType.VarChar;
            Pvalue[2] = ToID;

            objDS = dtb.SelectRecords("spSaveFFRToOutbox", Pname, Pvalue, Ptype);

            if (objDS != null)
            {
                if (objDS.Tables.Count > 0)
                {
                    if (objDS.Tables[0].Rows.Count > 0)
                    {
                        flag = true;
                    }
                }
            }
            //return Convert.ToString(objDS.Tables[0].Rows[0][0]);
            //else
            //    return string.Empty;
        }
        catch (Exception ex)
        {
            flag = false;
        }
        finally
        {
            objDS = null;
            //da = null;
        }
        return flag;
    }
    #endregion

    #region EncodeFBLForSend
    public static bool EncodeFBLForSend(string POL, string FlightNo, string FlightDate, string FromEmailID, string ToEmailID)
    {
        MessageData.fblinfo objFBLInfo = new MessageData.fblinfo ("");
        MessageData.unloadingport[] objUnloadingPort = new MessageData.unloadingport[0];
        MessageData.consignmnetinfo[] objConsInfo = new MessageData.consignmnetinfo[0];
        int count1 = 0;
        int count2 = 0;
        bool flag = false;
        try
        {
            DataSet dsData = new DataSet();

            dsData = getFBLData(POL, FlightNo, DateTime.Parse(FlightDate).ToString("dd/MM/yyyy"));
            
            if (dsData != null)
            {
                if (dsData.Tables.Count > 1)
                {
                    objFBLInfo.fblversion = "3";
                    objFBLInfo.messagesequencenum = "1";
                    objFBLInfo.carriercode = FlightNo.Substring(0, 2);
                    objFBLInfo.fltnum = FlightNo.Substring(2, FlightNo.Length - 2);
                    //objFBLInfo.date = DateTime.Parse(FlightDate).Day.ToString();
                    //objFBLInfo.month = DateTime.Parse(FlightDate).Month.ToString("");
                    DateTime dtFlight = DateTime.Parse(FlightDate);//, "dd/MM/yyyy", null);
                    objFBLInfo.date = DateTime.Parse(FlightDate).Day.ToString().PadLeft(2,'0');
                    objFBLInfo.month = dtFlight.ToString("MMM");
                    objFBLInfo.fltairportcode = POL;
                    objFBLInfo.endmesgcode = "LAST";

                    //flight details
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        count1 = 1;
                        foreach(DataRow dr in dsData.Tables[0].Rows)
                        {
                            MessageData.unloadingport objTempUnloadingPort = new MessageData.unloadingport("");
                            objTempUnloadingPort.unloadingairport = dr[2].ToString();
                            Array.Resize(ref objUnloadingPort, count1);
                            objUnloadingPort[count1-1] = objTempUnloadingPort;
                            count1++;
                        }
                    }
                    //awb details
                    if (dsData.Tables[1].Rows.Count < 1)
                    {
                        count2 = 1;
                        for (int m = 0; m < objUnloadingPort.Length; m++)
                            objUnloadingPort[m].nilcargocode = "NIL";
                    }
                    if (dsData.Tables[1].Rows.Count > 0)
                    {
                        count2 = 1;
                        foreach (DataRow dr in dsData.Tables[1].Rows)
                        {
                            MessageData.consignmnetinfo objTempConsInfo = new MessageData.consignmnetinfo("");
                            string AWBNumber = dr[0].ToString().Trim();
                            objTempConsInfo.airlineprefix = AWBNumber.Substring(0,3);
                            objTempConsInfo.awbnum = AWBNumber.Substring(3, AWBNumber.Length-3);
                            objTempConsInfo.origin = dr[11].ToString().Trim();
                            objTempConsInfo.dest = dr[5].ToString().Trim();
                            objTempConsInfo.consigntype = "T";
                            objTempConsInfo.pcscnt = dr[3].ToString().Trim();
                            objTempConsInfo.weightcode = "K";
                            objTempConsInfo.weight = dr[4].ToString().Trim();
                            objTempConsInfo.manifestdesc = dr[8].ToString().Trim() + "-" + dr[6].ToString().Trim();

                            Array.Resize(ref objConsInfo, count2);
                            objConsInfo[count2 - 1] = objTempConsInfo;
                            count2++;
                        }
                    }
                    if (count1 > 0 && count2 > 0)
                    { 
                        MessageData.dimensionnfo[] objDimenInfo = new MessageData.dimensionnfo[0];
                        MessageData.consignmentorigininfo[] objConsOriginInfo = new MessageData.consignmentorigininfo[0];
                        MessageData.ULDinfo[] objULDInfo = new MessageData.ULDinfo[0];
                        MessageData.otherserviceinfo objOtherInfo = new MessageData.otherserviceinfo("");
                        string FBLMsg = cls_Encode_Decode.EncodeFBLforsend(objFBLInfo, objUnloadingPort, objConsInfo, objDimenInfo, objConsOriginInfo, objULDInfo, objOtherInfo);
                        if (FBLMsg != null)
                        {
                            if (FBLMsg.Trim() != "")
                            {
                                //bool flag = false;
                                flag = addMsgToOutBox("FBL", FBLMsg, FromEmailID, ToEmailID);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return flag;
    }
    #endregion

    #region EncodeFFMForSend
    public static bool EncodeFFMForSend(string DepartureAirport, string FlightNo, string FlightDate, string FromEmailID, string ToEmailID)
    {
        bool flag = false;
        try
        {
            MessageData.ffminfo objFFMInfo = new MessageData.ffminfo("");
            MessageData.unloadingport[] objUnloadingPort = new MessageData.unloadingport[0];
            MessageData.consignmnetinfo[] objConsInfo = new MessageData.consignmnetinfo[0];
            int count1 = 0;
            int count2 = 0;
            //bool flag = false;
            DataSet dsData = new DataSet();
            DataSet ds = new DataSet();
            ds = getFFMUnloadingPort(DepartureAirport, FlightNo, DateTime.Parse(FlightDate).ToString("MM/dd/yyyy"));
            dsData = getFFMData(DepartureAirport, FlightNo, DateTime.Parse(FlightDate).ToString("MM/dd/yyyy"));
            if (ds != null) 
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)//(dsData.Tables[0].Rows.Count > 0)
                    {
                        count1 = 1;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            MessageData.unloadingport objTempUnloadingPort = new MessageData.unloadingport("");
                            objTempUnloadingPort.unloadingairport = dr[0].ToString();
                            Array.Resize(ref objUnloadingPort, count1);
                            objUnloadingPort[count1 - 1] = objTempUnloadingPort;
                            count1++;
                        }
                    }
                }
            }
            if (dsData != null)
            {
                if (dsData.Tables.Count > 0)
                {
                    objFFMInfo.ffmversionnum = "8";
                    objFFMInfo.messagesequencenum = "1";
                    objFFMInfo.carriercode = FlightNo.Substring(0, 2);
                    objFFMInfo.fltnum = FlightNo.Substring(2, FlightNo.Length - 2);
                    //objFFMInfo.date = DateTime.Parse(FlightDate).Day.ToString();
                    //objFFMInfo.month = DateTime.Parse(FlightDate).Month.ToString("");
                    DateTime dtFlight = DateTime.Parse(FlightDate);//, "dd/MM/yyyy", null);
                    objFFMInfo.fltdate = dtFlight.ToString("dd");//DateTime.Parse(FlightDate).Day.ToString().PadLeft(2, '0');
                    objFFMInfo.month = dtFlight.ToString("MMM");
                    objFFMInfo.fltairportcode = DepartureAirport;
                    objFFMInfo.endmesgcode = "LAST";

                    //flight details
                 
                    //awb details
                    if (dsData.Tables[1].Rows.Count > 0)
                    {
                        count2 = 1;
                        foreach (DataRow dr in dsData.Tables[1].Rows)
                        {
                            MessageData.consignmnetinfo objTempConsInfo = new MessageData.consignmnetinfo("");
                            string AWBNumber = dr[5].ToString().Trim();
                            objTempConsInfo.airlineprefix = AWBNumber.Substring(0, 3);
                            objTempConsInfo.awbnum = AWBNumber.Substring(3, AWBNumber.Length - 3);
                            objTempConsInfo.origin = dr[13].ToString().Trim();
                            objTempConsInfo.dest = dr[3].ToString().Trim();
                            objTempConsInfo.consigntype = "T";
                            objTempConsInfo.pcscnt = dr[7].ToString().Trim();
                            objTempConsInfo.weightcode = "K";
                            objTempConsInfo.weight = dr[8].ToString().Trim();
                            try
                            {
                                objTempConsInfo.manifestdesc = dr[12].ToString().Trim().Substring(0, 14);//dr[6].ToString().Trim() + "-" + dr[12].ToString().Trim();
                            }
                            catch (Exception ex) { }
                            for (int k = 0; k < objUnloadingPort.Length; k++) 
                            {
                                if (objUnloadingPort[k].unloadingairport.Equals(dr[2].ToString().Trim(), StringComparison.OrdinalIgnoreCase) || objUnloadingPort[k].unloadingairport.Equals(dr[3].ToString().Trim(), StringComparison.OrdinalIgnoreCase))
                                {
                                    objTempConsInfo.portsequence = (k+1).ToString();
                                }
                            }
                            Array.Resize(ref objConsInfo, count2);
                            objConsInfo[count2 - 1] = objTempConsInfo;
                            count2++;
                        }
                    }
                    if (count1 > 0 && count2 > 0)
                    {
                        MessageData.dimensionnfo[] objDimenInfo = new MessageData.dimensionnfo[0];
                        MessageData.consignmentorigininfo[] objConsOriginInfo = new MessageData.consignmentorigininfo[0];
                        MessageData.ULDinfo[] objULDInfo = new MessageData.ULDinfo[0];
                        MessageData.otherserviceinfo[] objOtherInfo = new MessageData.otherserviceinfo[0];
                        MessageData.movementinfo[] objMovemInfo = new MessageData.movementinfo[0];
                        MessageData.customsextrainfo[] objcustInfo = new MessageData.customsextrainfo[0];
                        //string FFMMsg = "";
                        string FFMMsg = cls_Encode_Decode.EncodeFFMforsend(ref objFFMInfo, ref objUnloadingPort, ref objConsInfo, ref objDimenInfo, 
                                                                           ref objMovemInfo, ref objOtherInfo, ref objcustInfo, ref objULDInfo);
                        if (FFMMsg != null)
                        {
                            if (FFMMsg.Trim() != "" && FFMMsg.Length>0)
                            {
                                //bool flag = false;                               
                                flag = addMsgToOutBox("FFM", FFMMsg, FromEmailID, ToEmailID);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return flag;
    }
    #endregion

    #region EncodeFFM
    public static string EncodeFFM(string DepartureAirport, string FlightNo, string FlightDate)
    {
        string FFMMsg = "";
        try
        {
            MessageData.ffminfo objFFMInfo = new MessageData.ffminfo("");
            MessageData.unloadingport[] objUnloadingPort = new MessageData.unloadingport[0];
            MessageData.consignmnetinfo[] objConsInfo = new MessageData.consignmnetinfo[0];
            MessageData.ULDinfo[] objULDInfo = new MessageData.ULDinfo[0];
                        
            int count1 = 0;
            int count2 = 0;
            //bool flag = false;
            DataSet dsData = new DataSet();
            DataSet ds = new DataSet();
            ds = getFFMUnloadingPort(DepartureAirport, FlightNo, DateTime.Parse(FlightDate).ToString("MM/dd/yyyy"));
            dsData = getFFMData(DepartureAirport, FlightNo, DateTime.Parse(FlightDate).ToString("MM/dd/yyyy"));
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)//(dsData.Tables[0].Rows.Count > 0)
                    {
                        count1 = 1;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            MessageData.unloadingport objTempUnloadingPort = new MessageData.unloadingport("");
                            objTempUnloadingPort.unloadingairport = dr[0].ToString();                            
                            Array.Resize(ref objUnloadingPort, count1);
                            objUnloadingPort[count1 - 1] = objTempUnloadingPort;
                            count1++;
                        }
                    }
                }
            }
            if (dsData != null)
            {
                if (dsData.Tables.Count > 0)
                {
                    objFFMInfo.ffmversionnum = "8";
                    objFFMInfo.messagesequencenum = "1";
                    objFFMInfo.carriercode = FlightNo.Substring(0, 2);
                    objFFMInfo.fltnum = FlightNo.Substring(2, FlightNo.Length - 2);
                    //objFFMInfo.date = DateTime.Parse(FlightDate).Day.ToString();
                    //objFFMInfo.month = DateTime.Parse(FlightDate).Month.ToString("");
                    DateTime dtFlight = DateTime.Parse(FlightDate);//, "dd/MM/yyyy", null);
                    objFFMInfo.fltdate = dtFlight.ToString("dd");//DateTime.Parse(FlightDate).Day.ToString().PadLeft(2, '0');
                    objFFMInfo.month = dtFlight.ToString("MMM").ToUpper();
                    objFFMInfo.fltairportcode = DepartureAirport;
                    objFFMInfo.endmesgcode = "LAST";
                    count2 = 1;
                    //flight details

                    //awb details
                    if (dsData.Tables[0].Rows.Count < 1) 
                    {
                        for (int m = 0; m < objUnloadingPort.Length; m++)
                            objUnloadingPort[m].nilcargocode = "NIL";
                    }
                    //ULD Table if exists
                    try
                    {
                        if (dsData.Tables.Count > 2) 
                        {
                            if (dsData.Tables[2].Rows.Count > 0)
                            {
                                Array.Resize(ref objULDInfo, dsData.Tables[2].Rows.Count);
                                for (int k = 0; k < dsData.Tables[2].Rows.Count; k++)
                                {
                                    MessageData.ULDinfo objTempULDInfo = new MessageData.ULDinfo("");
                                    objTempULDInfo.uldsrno = dsData.Tables[2].Rows[k]["ULDNo"].ToString();
                                    objTempULDInfo.refuld = k.ToString();
                                    for (int cnt = 0; cnt < objUnloadingPort.Length; cnt++)
                                    {
                                        if (objUnloadingPort[cnt].unloadingairport.Equals(dsData.Tables[2].Rows[k]["POU"].ToString().Trim(), StringComparison.OrdinalIgnoreCase))
                                        {
                                            objTempULDInfo.portsequence = (cnt + 1).ToString();
                                        }
                                    }
                                    objULDInfo[k] = objTempULDInfo;
                                   
                                }
                            }
                        }
                    }
                    catch (Exception ex) { }
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        
                        foreach (DataRow dr in dsData.Tables[0].Rows)
                        {
                            MessageData.consignmnetinfo objTempConsInfo = new MessageData.consignmnetinfo("");
                            string AWBNumber = dr[5].ToString().Trim().Replace("-","");
                            objTempConsInfo.airlineprefix = AWBNumber.Substring(0, 3);
                            objTempConsInfo.awbnum = AWBNumber.Substring(3, AWBNumber.Length - 3);
                            objTempConsInfo.origin = dr[13].ToString().Trim();
                            objTempConsInfo.dest = dr[3].ToString().Trim();
                            try
                            {
                                objTempConsInfo.consigntype = dr["consigntype"].ToString();
                            }
                            catch (Exception) 
                            {
                                objTempConsInfo.consigntype = "T";
                            }
                            objTempConsInfo.pcscnt = dr["PCS"].ToString().Trim();
                            objTempConsInfo.weightcode = dr["UOM"].ToString();
                            objTempConsInfo.weight = dr["GrossWgt"].ToString().Trim();
                            objTempConsInfo.splhandling = dr["SHCCodes"].ToString();
                            try 
                            {
                                if (dr["consigntype"].ToString() != "T") 
                                {
                                    objTempConsInfo.shpdesccode = "T";
                                    objTempConsInfo.numshp = dr["AWBPcs"].ToString().Trim();
                                }
                            }
                            catch (Exception) { }
                            try
                            {
                                int length = 14;
                                if (dr[12].ToString().Length < 14) 
                                {
                                    length = dr[12].ToString().Length;
                                }
                                objTempConsInfo.manifestdesc = dr[12].ToString().Substring(0,length); //dr[6].ToString().Trim() + "-" + dr[12].ToString().Trim();
                            }
                            catch (Exception ex) { }
                            try 
                            {
                                for (int k = 0; k < objULDInfo.Length; k++)
                                {
                                    if (objULDInfo[k].uldsrno.Equals(dr[0].ToString().Trim(), StringComparison.OrdinalIgnoreCase))
                                    {
                                        objTempConsInfo.uldsequence = objULDInfo[k].refuld.ToString();
                                    }
                                }
                            }
                            catch (Exception ex) 
                            {
                                objTempConsInfo.uldsequence = "";
                            }
                            for (int k = 0; k < objUnloadingPort.Length; k++)
                            {
                                if (objUnloadingPort[k].unloadingairport.Equals(dr[2].ToString().Trim(), StringComparison.OrdinalIgnoreCase) || objUnloadingPort[k].unloadingairport.Equals(dr[3].ToString().Trim(), StringComparison.OrdinalIgnoreCase))
                                {
                                    objTempConsInfo.portsequence = (k + 1).ToString();
                                }
                            }
                            Array.Resize(ref objConsInfo, count2);
                            objConsInfo[count2 - 1] = objTempConsInfo;
                            count2++;
                        }
                    }
                    if (count1 > 0 && count2 > 0)
                    {
                        MessageData.dimensionnfo[] objDimenInfo = new MessageData.dimensionnfo[0];
                        MessageData.consignmentorigininfo[] objConsOriginInfo = new MessageData.consignmentorigininfo[0];
                        MessageData.otherserviceinfo[] objOtherInfo = new MessageData.otherserviceinfo[0];
                        MessageData.movementinfo[] objMovemInfo = new MessageData.movementinfo[0];
                        MessageData.customsextrainfo[] objcustInfo = new MessageData.customsextrainfo[0];
                        //string FFMMsg = "";
                         FFMMsg = cls_Encode_Decode.EncodeFFMforsend(ref objFFMInfo, ref objUnloadingPort, ref objConsInfo, ref objDimenInfo,
                                                                           ref objMovemInfo, ref objOtherInfo, ref objcustInfo, ref objULDInfo);
                        if (FFMMsg != null)
                        {
                            if (FFMMsg.Trim() != "" && FFMMsg.Length > 0)
                            {
                                //bool flag = false;

                                return FFMMsg;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        { }
        
        return FFMMsg;
    }
    #endregion

    #region EncodeFBL
    public static string EncodeFBL(string POL, string FlightNo, string FlightDate)
    {
        MessageData.fblinfo objFBLInfo = new MessageData.fblinfo("");
        MessageData.unloadingport[] objUnloadingPort = new MessageData.unloadingport[0];
        MessageData.consignmnetinfo[] objConsInfo = new MessageData.consignmnetinfo[0];
        int count1 = 0;
        int count2 = 0;
        string FBLMsg = "";
        try
        {
            DataSet dsData = new DataSet();

            dsData = getFBLData(POL, FlightNo, DateTime.Parse(FlightDate).ToString("dd/MM/yyyy"));

            if (dsData != null)
            {
                if (dsData.Tables.Count > 1)
                {
                    objFBLInfo.fblversion = "3";
                    objFBLInfo.messagesequencenum = "1";
                    objFBLInfo.carriercode = FlightNo.Substring(0, 2);
                    objFBLInfo.fltnum = FlightNo.Substring(2, FlightNo.Length - 2);
                    //objFBLInfo.date = DateTime.Parse(FlightDate).Day.ToString();
                    //objFBLInfo.month = DateTime.Parse(FlightDate).Month.ToString("");
                    DateTime dtFlight = DateTime.Parse(FlightDate);//, "dd/MM/yyyy", null);
                    objFBLInfo.date = DateTime.Parse(FlightDate).Day.ToString().PadLeft(2, '0');
                    objFBLInfo.month = dtFlight.ToString("MMM").ToUpper();
                    objFBLInfo.fltairportcode = POL;
                    objFBLInfo.endmesgcode = "LAST";

                    //flight details
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        count1 = 1;
                        foreach (DataRow dr in dsData.Tables[0].Rows)
                        {
                            MessageData.unloadingport objTempUnloadingPort = new MessageData.unloadingport("");
                            objTempUnloadingPort.unloadingairport = dr[2].ToString();
                            Array.Resize(ref objUnloadingPort, count1);
                            objUnloadingPort[count1 - 1] = objTempUnloadingPort;
                            count1++;
                        }
                    }
                    //awb details

                    if (dsData.Tables[1].Rows.Count < 1)
                    {
                        count2 = 1;
                        for (int m = 0; m < objUnloadingPort.Length; m++)
                            objUnloadingPort[m].nilcargocode = "NIL";
                    }
                    if (dsData.Tables[1].Rows.Count > 0)
                    {
                        count2 = 1;
                        foreach (DataRow dr in dsData.Tables[1].Rows)
                        {
                            MessageData.consignmnetinfo objTempConsInfo = new MessageData.consignmnetinfo("");
                            string AWBNumber = dr[0].ToString().Trim();
                            objTempConsInfo.airlineprefix = AWBNumber.Substring(0, 3);
                            objTempConsInfo.awbnum = AWBNumber.Substring(3, AWBNumber.Length - 3);
                            objTempConsInfo.origin = dr[11].ToString().Trim();
                            objTempConsInfo.dest = dr[5].ToString().Trim();
                            objTempConsInfo.consigntype = "T";
                            objTempConsInfo.pcscnt = dr[3].ToString().Trim();
                            objTempConsInfo.weightcode = dr["UOM"].ToString();
                            objTempConsInfo.weight = dr[4].ToString().Trim();
                            try
                            {
                                objTempConsInfo.manifestdesc = dr[6].ToString().Trim().Substring(0,14); //dr[8].ToString().Trim() + "-" + dr[6].ToString().Trim();
                            }
                            catch (Exception ex) { }
                            Array.Resize(ref objConsInfo, count2);
                            objConsInfo[count2 - 1] = objTempConsInfo;
                            count2++;
                        }
                    }
                    if (count1 > 0 && count2 > 0)
                    {
                        MessageData.dimensionnfo[] objDimenInfo = new MessageData.dimensionnfo[0];
                        MessageData.consignmentorigininfo[] objConsOriginInfo = new MessageData.consignmentorigininfo[0];
                        MessageData.ULDinfo[] objULDInfo = new MessageData.ULDinfo[0];
                        MessageData.otherserviceinfo objOtherInfo = new MessageData.otherserviceinfo("");
                        FBLMsg = cls_Encode_Decode.EncodeFBLforsend(objFBLInfo, objUnloadingPort, objConsInfo, objDimenInfo, objConsOriginInfo, objULDInfo, objOtherInfo);
                        if (FBLMsg != null)
                        {
                            if (FBLMsg.Trim() != "")
                            {
                                //bool flag = false;
                                return FBLMsg;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            FBLMsg = "";
        }
        return FBLMsg;
    }
    #endregion
    
    #region EncodeFFAForSend
    public static bool EncodeFFAForSend(DataSet dsData, string FromEmailID, string ToEmailID)
    {
        bool flag = false;
        try
        {
            if (dsData != null)
            {
                if (dsData.Tables.Count > 0)
                {
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        int i = 0;
                        MessageData.ffainfo objFFA = new MessageData.ffainfo();
                        MessageData.FltRoute[] FltRoute = new MessageData.FltRoute[0];
                        objFFA.ffaversionnum = "4";
                        objFFA.airlineprefix = dsData.Tables[0].Rows[i]["AWBPrefix"].ToString();
                        objFFA.awbnum = dsData.Tables[0].Rows[i]["AWBNumber"].ToString();
                        objFFA.origin = dsData.Tables[0].Rows[i]["OriginCode"].ToString().ToUpper();
                        objFFA.dest = dsData.Tables[0].Rows[i]["DestinationCode"].ToString().ToUpper();
                        objFFA.consigntype = "T";
                        objFFA.pcscnt = dsData.Tables[0].Rows[i]["PiecesCount"].ToString();
                        objFFA.weightcode = "K";
                        objFFA.weight = dsData.Tables[0].Rows[i]["GrossWeight"].ToString();
                        objFFA.shpdesccode = "";
                        objFFA.numshp = "";
                        objFFA.manifestdesc = dsData.Tables[0].Rows[i]["CommodityDesc"].ToString();
                        objFFA.splhandling = "";// dsData.Tables[0].Rows[i]["CommodityCode"].ToString();


                        Array.Resize(ref FltRoute, dsData.Tables[1].Rows.Count);
                        for (int k = 0; k < dsData.Tables[1].Rows.Count; k++)
                        {
                            FltRoute[k].carriercode = "";
                            FltRoute[k].fltnum = dsData.Tables[1].Rows[k]["FltNumber"].ToString();
                            string[] str = dsData.Tables[1].Rows[k]["FltDate"].ToString().Split('/');
                            FltRoute[k].date = str[0];
                            FltRoute[k].month = str[1];
                            FltRoute[k].fltdept = dsData.Tables[1].Rows[k]["FltOrigin"].ToString().ToUpper();
                            FltRoute[k].fltarrival = dsData.Tables[1].Rows[k]["FltDestination"].ToString().ToUpper();
                            string stat = dsData.Tables[1].Rows[k]["Status"].ToString();
                            #region Status Code
                            if (stat.Length < 1)
                            {
                                stat = "LL";
                            }
                            if (stat.Length > 0 && stat.Equals("Q", StringComparison.OrdinalIgnoreCase))
                            {
                                stat = "LL";
                            }
                            if (stat.Length > 0 || stat.Equals("C", StringComparison.OrdinalIgnoreCase))
                            {
                                stat = "KK";
                            }
                            #endregion
                            FltRoute[k].spaceallotmentcode = stat;
                        }

                        objFFA.specialservicereq1 = "";
                        objFFA.specialservicereq2 = "";
                        objFFA.otherserviceinfo1 = "";
                        objFFA.otherserviceinfo2 = "";
                        objFFA.bookingrefairport = "";
                        objFFA.officefundesignation = "";
                        objFFA.companydesignator = "";
                        objFFA.bookingfileref = "";
                        objFFA.participentidetifier = "";
                        objFFA.participentcode = "";
                        objFFA.participentairportcity = dsData.Tables[0].Rows[i]["OriginCode"].ToString();
                        objFFA.shiprefnum = "";
                        objFFA.supplemetryshipperinfo1 = "";
                        objFFA.supplemetryshipperinfo2 = "";

                        string ffa = cls_Encode_Decode.encodeFFAforsend(ref objFFA, ref FltRoute);
                        clsLog.WriteLog(ToEmailID);
                        if (addMsgToOutBox("FFA", ffa, FromEmailID, ToEmailID))
                        {
                            flag = true;
                        }
                        else
                        {
                            clsLog.WriteLog("Error: Not inserted in outbox");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsLog.WriteLog("Error in encode FFA:" + ex.Message);
        }
        return flag;
    }
    #endregion

    #region EncodeFSAForSend
    public static bool EncodeFSAForSend(string AirlinePrefix, string AWBNo, string CarrierCode, string FlightNo, string OperType, string FromEmailID, string ToEmailID)
    {
        bool flag = false;
        try
        {
            DataSet dsAWB = GetAWBDetailsForFSA(AWBNo, OperType);
            DataSet dsFlight = getFlightDetailsForFSA(AWBNo, CarrierCode.Trim() + FlightNo.Trim());
            if (dsAWB != null)
            {
                if (dsAWB.Tables.Count > 0)
                {
                    if (dsAWB.Tables[0].Rows.Count > 0)
                    {
                        DataRow drAWB = dsAWB.Tables[0].Rows[0];
                        DataRow drFlight = dsFlight.Tables[0].Rows[0];
                        MessageData.FSAInfo objFSA = new MessageData.FSAInfo("");
                        MessageData.CommonStruct[] objFSANode = new MessageData.CommonStruct[0];
                        MessageData.CommonStruct objComnStruct = new MessageData.CommonStruct("");
                        MessageData.customsextrainfo[] objCustomInfo = new MessageData.customsextrainfo[0];
                        MessageData.ULDinfo[] objULDInfo = new MessageData.ULDinfo[0];
                        MessageData.otherserviceinfo[] objOtherSercInfo = new MessageData.otherserviceinfo[0];

                        #region PrepareStructure

                        try
                        {
                            objFSA.airlineprefix = AirlinePrefix;
                            objFSA.fsaversion = "14";
                            objFSA.awbnum = AWBNo;
                            objFSA.origin = drAWB["OriginCode"].ToString().ToUpper();
                            objFSA.dest = drAWB["DestinationCode"].ToString().ToUpper();
                            objFSA.pcscnt = drAWB["PiecesCount"].ToString();
                            objFSA.consigntype = "T";
                            objFSA.weight = drAWB["GrossWeight"].ToString();
                            objFSA.weightcode = drAWB["UOM"].ToString();
                            //objFSA.totalpcscnt = drAWB["PiecesCount"].ToString();

                            DateTime FlightDate = DateTime.ParseExact(drFlight["FltDate"].ToString(), "dd/MM/yyyy", null);
                            //dtTo = DateTime.ParseExact(drAWBRouteMaster["FltDate"].ToString(), "dd/MM/yyyy", null);

                            #region SwitchCase
                            switch (OperType)
                            {
                                case "RCT":
                                case "DIS":
                                case "FOH":
                                case "RCS":
                                    {
                                        #region RCS
                                        objComnStruct.messageprefix = OperType.Trim();
                                        objComnStruct.carriercode = CarrierCode.Trim();
                                        objComnStruct.fltday = FlightDate.ToString("dd");
                                        objComnStruct.fltmonth = FlightDate.ToString("MMM");
                                        objComnStruct.flttime = "";
                                        objComnStruct.airportcode = drFlight["FltOrigin"].ToString();
                                        objComnStruct.pcsindicator = "T";
                                        objComnStruct.numofpcs = drAWB["PiecesCount"].ToString();
                                        objComnStruct.weightcode = drAWB["UOM"].ToString();
                                        objComnStruct.weight = drAWB["GrossWeight"].ToString();
                                        objComnStruct.name = "";
                                        #endregion
                                    }
                                    break;
                                case "NFD":
                                case "AWD":
                                case "CCD":
                                case "DLV":
                                case "DDL":
                                case "TGC":
                                case "TFD":
                                    {
                                        #region NFD
                                        objComnStruct.messageprefix = OperType.Trim();
                                        objComnStruct.carriercode = CarrierCode.Trim();
                                        objComnStruct.fltday = FlightDate.ToString("dd");
                                        objComnStruct.fltmonth = FlightDate.ToString("MMM");
                                        objComnStruct.flttime = "";
                                        objComnStruct.airportcode = drFlight["FltOrigin"].ToString();
                                        objComnStruct.pcsindicator = "T";
                                        objComnStruct.numofpcs = drAWB["PiecesCount"].ToString();
                                        objComnStruct.weightcode = drAWB["UOM"].ToString();
                                        objComnStruct.weight = drAWB["GrossWeight"].ToString();
                                        objComnStruct.name = "";
                                        objComnStruct.transfermanifestnumber = "";
                                        #endregion
                                    }
                                    break;
                                case "RCF":
                                case "MAN":
                                case "ARR":
                                case "AWR":
                                case "DEP":
                                case "PRE":
                                    {
                                        #region RCF/MAN/DEP/PRE
                                        objComnStruct.messageprefix = OperType.Trim();
                                        objComnStruct.carriercode = CarrierCode.Trim();
                                        objComnStruct.flightnum = FlightNo.Trim();
                                        objComnStruct.fltday = FlightDate.ToString("dd");
                                        objComnStruct.fltmonth = FlightDate.ToString("MMM");
                                        objComnStruct.airportcode = drFlight["FltOrigin"].ToString();
                                        objComnStruct.pcsindicator = "T";
                                        objComnStruct.numofpcs = drAWB["PiecesCount"].ToString();
                                        objComnStruct.weightcode = drAWB["UOM"].ToString();
                                        objComnStruct.weight = drAWB["GrossWeight"].ToString();
                                        objComnStruct.daychangeindicator = "";
                                        objComnStruct.timeindicator = "S,S";
                                        objComnStruct.depttime = drFlight["SchDeptTime"].ToString().Replace(":", "");
                                        objComnStruct.arrivaltime = drFlight["SchArrTime"].ToString().Replace(":", "");
                                        #endregion
                                    }
                                    break;
                                case "BKD":
                                    {
                                        #region BKD
                                        objComnStruct.messageprefix = OperType.Trim();
                                        objComnStruct.carriercode = CarrierCode.Trim();
                                        objComnStruct.flightnum = FlightNo.Trim();
                                        objComnStruct.fltday = FlightDate.ToString("dd");
                                        objComnStruct.fltmonth = FlightDate.ToString("MMM");
                                        objComnStruct.fltorg = drFlight["FltOrigin"].ToString();
                                        objComnStruct.fltdest = drFlight["FltDestination"].ToString();
                                        objComnStruct.pcsindicator = "T";
                                        objComnStruct.numofpcs = drAWB["PiecesCount"].ToString();
                                        objComnStruct.weightcode = drAWB["UOM"].ToString();
                                        objComnStruct.weight = drAWB["GrossWeight"].ToString();
                                        objComnStruct.daychangeindicator = "";
                                        objComnStruct.depttime = drFlight["SchDeptTime"].ToString().Replace(":", "");
                                        objComnStruct.timeindicator = "S,S";
                                        objComnStruct.arrivaltime = drFlight["SchArrTime"].ToString().Replace(":", "");
                                        objComnStruct.volumecode = "";
                                        objComnStruct.volumeamt = "";
                                        objComnStruct.densityindicator = "";
                                        objComnStruct.densitygroup = "";
                                        #endregion

                                    } break;
                                case "TRM":
                                    {
                                        #region TRM
                                        objComnStruct.messageprefix = OperType.Trim();
                                        objComnStruct.carriercode = CarrierCode.Trim();
                                        objComnStruct.fltorg = drFlight["FltOrigin"].ToString();
                                        objComnStruct.fltdest = drFlight["FltDestination"].ToString();
                                        objComnStruct.pcsindicator = "T";
                                        objComnStruct.numofpcs = drAWB["PiecesCount"].ToString();
                                        objComnStruct.weightcode = drAWB["UOM"].ToString();
                                        objComnStruct.weight = drAWB["GrossWeight"].ToString();
                                        #endregion
                                    } break;


                                case "CRC":
                                    {
                                        #region CRC
                                        objComnStruct.messageprefix = OperType.Trim();
                                        objComnStruct.fltday = FlightDate.ToString("dd");
                                        objComnStruct.fltmonth = FlightDate.ToString("MMM");
                                        objComnStruct.airportcode = drFlight["FltOrigin"].ToString();
                                        objComnStruct.pcsindicator = "T";
                                        objComnStruct.numofpcs = drAWB["PiecesCount"].ToString();
                                        objComnStruct.weightcode = drAWB["UOM"].ToString();
                                        objComnStruct.weight = drAWB["GrossWeight"].ToString();
                                        objComnStruct.carriercode = CarrierCode.Trim();
                                        objComnStruct.flightnum = FlightNo.Trim();
                                        objComnStruct.fltorg = drFlight["FltOrigin"].ToString();
                                        objComnStruct.fltdest = drFlight["FltDestination"].ToString();
                                        #endregion
                                    } break;

                            }
                            #endregion


                        }
                        catch (Exception ex)
                        { }

                        Array.Resize(ref objFSANode, objFSANode.Length + 1);
                        objFSANode[objFSANode.Length - 1] = objComnStruct;
                        #endregion

                        string msgFSA = cls_Encode_Decode.EncodeFSAforSend(ref objFSA, ref objFSANode, ref objCustomInfo, ref objULDInfo, ref objOtherSercInfo);

                        flag = addMsgToOutBox("FSA", msgFSA, FromEmailID, ToEmailID);

                    }
                }
            }
        }
        catch (Exception ex)
        { }
        return flag;
    }
    #endregion

    #region EncodeFWBForSend
    public static bool EncodeFWBForSend(string AirlinePrefix, string AWBNo, string FlightNo, string FromEmailID, string ToEmailID)
    {
        bool flag = false;
        try
        {
            //DataSet dsAWB = GetAWBDetailsForFWB(AWBNo);
            BookingBAL objBLL = new BookingBAL();
            DataSet dsAWB = new DataSet();
            string ErrorMsg = "";
            objBLL.GetAWBDetails(AWBNo,AirlinePrefix, ref dsAWB, ref ErrorMsg);

            //DataSet dsFlight = getFlightDetailsForFWB(AWBNo, FlightNo.Trim());

            MessageData.fwbinfo FWBData = new MessageData.fwbinfo("");
            MessageData.othercharges[] othData = new MessageData.othercharges[0];
            MessageData.otherserviceinfo[] othSrvData = new MessageData.otherserviceinfo[0];
            MessageData.RateDescription[] fwbrates = new MessageData.RateDescription[0];
            MessageData.customsextrainfo[] custominfo = new MessageData.customsextrainfo[0];
            #region Prepare Structure
            try
            {
                if (dsAWB != null)
                {
                    if (dsAWB.Tables.Count > 0)
                    {
                        #region Other Charges
                        try
                        {
                            DataTable DTOtherCh = dsAWB.Tables[5].Copy();
                            if (DTOtherCh.Rows.Count > 0)
                            {
                                for (int i = 0; i < DTOtherCh.Rows.Count; i++)
                                {
                                    MessageData.othercharges tempothData = new MessageData.othercharges("");
                                    //string[] tempcode = DTOtherCh.Rows[i]["ChargeHeadCode"].ToString().Trim().Split('/');
                                    //if (tempcode.Length > 0)
                                    //{
                                    //    tempothData.otherchargecode = tempcode[0].Trim();
                                    //}
                                    string ChargeType = DTOtherCh.Rows[i]["ChargeType"].ToString();
                                    if (ChargeType.Trim() == "DA")
                                    {
                                        tempothData.entitlementcode = "A";
                                        tempothData.otherchargecode = "MA";
                                    }
                                    else if (ChargeType.Trim() == "DC")
                                    {
                                        tempothData.entitlementcode = "C";
                                        tempothData.otherchargecode = "MC";
                                    }
                                    tempothData.indicator = "P";
                                    tempothData.chargeamt = DTOtherCh.Rows[i]["Charge"].ToString();
                                    Array.Resize(ref othData, othData.Length + 1);
                                    othData[othData.Length - 1] = tempothData;
                                }
                            }
                        }
                        catch (Exception ex)
                        { }
                        #endregion

                        DataTable DT1 = dsAWB.Tables[0].Copy();
                        DataTable DT2 = dsAWB.Tables[1].Copy();
                        DataTable DT3 = dsAWB.Tables[2].Copy();
                        DataTable DT4 = dsAWB.Tables[3].Copy();
                        DataTable DT5 = dsAWB.Tables[4].Copy();
                        DataTable DT7 = dsAWB.Tables[6].Copy();
                        DataTable DT8 = dsAWB.Tables[7].Copy();  //Rate Charges
                        DataTable DT9 = dsAWB.Tables[8].Copy();

                        #region RateDescription
                        try 
                        {
                            if (DT8 != null) 
                            {
                                if (DT8.Rows.Count > 0) 
                                {
                                                                        
                                    for (int i = 0; i < DT8.Rows.Count; i++) 
                                    {
                                        MessageData.RateDescription rate = new MessageData.RateDescription("");
                                        rate.linenum = (i + 1).ToString();
                                        rate.pcsidentifier = "P";
                                        rate.numofpcs = DT8.Rows[i]["Pcs"].ToString();
                                        rate.weightindicator = "K";
                                        rate.weight = DT8.Rows[i]["Weight"].ToString();
                                        rate.rateclasscode = DT8.Rows[i]["RateClass"].ToString();//RateClass
                                        rate.commoditynumber = DT8.Rows[i]["CommCode"].ToString();
                                        rate.awbweight = DT8.Rows[i]["ChargedWeight"].ToString();
                                        rate.chargerate = DT8.Rows[i]["RatePerKg"].ToString();
                                        rate.chargeamt = DT8.Rows[i]["Total"].ToString();
                                        rate.goodsnature = DT2.Rows[0]["CodeDescription"].ToString();
                                        Array.Resize(ref fwbrates, fwbrates.Length+1);
                                        fwbrates[fwbrates.Length - 1] = rate;
                                    }
                                }
                            }
                        }
                        catch (Exception ex) { }
                        #endregion


                        FWBData.fwbversionnum = "16";
                        FWBData.airlineprefix = AirlinePrefix.Trim();
                        FWBData.awbnum = AWBNo;
                        FWBData.origin = DT1.Rows[0]["OriginCode"].ToString().Trim();
                        FWBData.dest = DT1.Rows[0]["DestinationCode"].ToString().Trim();
                        FWBData.pcscnt = DT1.Rows[0]["PiecesCount"].ToString().Trim();
                        FWBData.weightcode = "K";
                        FWBData.weight = DT1.Rows[0]["ChargedWeight"].ToString().Trim();
                        //FWBData.volumecode = "";
                        //FWBData.volumeamt = "";
                        FWBData.densityindicator = "";
                        FWBData.densitygrp = "";
                        FWBData.carriercode = FlightNo.Substring(0, 2);
                        FWBData.fltnum = FlightNo.Substring(2, FlightNo.Length - 2);
                        DateTime FlightDate = DateTime.Parse(DT4.Rows[0]["FltDate"].ToString());
                        FWBData.fltday = FlightDate.Day.ToString().PadLeft(2, '0');
                        FWBData.agentIATAnumber = DT1.Rows[0]["AgentCode"].ToString();
                        FWBData.agentname = DT1.Rows[0]["AgentName"].ToString();
                        FWBData.agentname = FWBData.origin;
                        try
                        {
                            FWBData.currency = DT2.Rows[0]["Currency"].ToString();
                            FWBData.declaredvalue="NVD";
                            FWBData.declaredcustomvalue="NCV";
                            FWBData.insuranceamount = "XXX";
                            string PaymentMode = "";
                            FWBData.chargedec = PaymentMode;
                            PaymentMode = DT2.Rows[0]["PaymentMode"].ToString();
                            if (PaymentMode.Trim() == "PP")
                            {
                                try
                                {
                                    FWBData.PPweightCharge = DT8.Rows[0]["FrIATA"].ToString();
                                    FWBData.PPTaxesCharge = DT8.Rows[0]["ServTax"].ToString();
                                    FWBData.PPOCDC = DT8.Rows[0]["OCDueCar"].ToString();
                                    FWBData.PPOCDA = DT8.Rows[0]["OCDueAgent"].ToString();
                                    FWBData.PPTotalCharges = DT8.Rows[0]["Total"].ToString();
                                }
                                catch (Exception ex)
                                { }
                            }
                            else
                            {
                                try
                                {
                                    FWBData.CCweightCharge = DT8.Rows[0]["FrIATA"].ToString();
                                    FWBData.CCTaxesCharge = DT8.Rows[0]["ServTax"].ToString();
                                    FWBData.CCOCDC = DT8.Rows[0]["OCDueCar"].ToString();
                                    FWBData.CCOCDA = DT8.Rows[0]["OCDueAgent"].ToString();
                                    FWBData.CCTotalCharges = DT8.Rows[0]["Total"].ToString();
                                }
                                catch (Exception ex)
                                { }
                            }
                        }
                        catch (Exception ex)
                        { }
                        //string ExecutionDate = DateTime.Parse(DT1.Rows[0]["ExecutionDate"].ToString()).ToString("dd/MM/yyyy");
                        //string ExecutionDate = DT1.Rows[0]["ExecutionDate"].ToString();
                        //ExecutionDate = DateTime.Parse(ExecutionDate.ToString("MM/dd/yyyy"));
                        try
                        {
                            string[] ExecutionDate = DT1.Rows[0]["ExecutionDate"].ToString().Split('/');
                            if (ExecutionDate.Length >= 3)
                            {
                                FWBData.carrierdate = ExecutionDate[0].PadLeft(2, '0'); ;
                                FWBData.carriermonth = ExecutionDate[1].PadLeft(2, '0');
                                FWBData.carrieryear = ExecutionDate[2].PadLeft(4, '0').Substring(2, 2);
                            }
                        }
                        catch (Exception ex)
                        { }

                        FWBData.carrierplace = DT1.Rows[0]["ExecutedAt"].ToString();

                        try
                        {
                            FWBData.shippername = DT7.Rows[0]["ShipperName"].ToString();
                            FWBData.shipperadd = DT7.Rows[0]["ShipperAddress"].ToString();
                            FWBData.shippercountrycode = DT7.Rows[0]["ShipperCountry"].ToString();
                            FWBData.shippercontactnum = DT7.Rows[0]["ShipperTelephone"].ToString();
                            FWBData.consname = DT7.Rows[0]["ConsigneeName"].ToString();
                            FWBData.consadd = DT7.Rows[0]["ConsigneeAddress"].ToString();
                            FWBData.conscountrycode = DT7.Rows[0]["ConsigneeCountry"].ToString();
                            FWBData.conscontactnum = DT7.Rows[0]["ConsigneeTelephone"].ToString();
                            FWBData.senderairport = DT1.Rows[0]["OriginCode"].ToString().Trim();
                            FWBData.senderParticipentIdentifier="AGT";
                            FWBData.senderParticipentCode=DT1.Rows[0]["AgentCode"].ToString();
                        }
                        catch (Exception ex)
                        { }

                    }
                }
            }
            catch (Exception ex)
            { }
            #endregion

            string FWBMsg = "";
            FWBMsg = cls_Encode_Decode.EncodeFWBForSend(ref FWBData, ref othData, ref othSrvData, ref fwbrates, ref custominfo);
            //EncodeFWBForSend(ref MessageData.fwbinfo fwbdata, ref MessageData.othercharges[] fwbOtherCharge, ref MessageData.otherserviceinfo othinfoarray, ref MessageData.RateDescription[] fwbrate)

            if (FWBMsg.Trim() != "")
            {
                flag = addMsgToOutBox("FWB", FWBMsg, FromEmailID, ToEmailID);
            }

        }
        catch (Exception ex)
        { }
        return flag;
    }
    #endregion

    #region getFBLData
    public static DataSet getFBLData(string POL, string FlightNo, string FlightDate)
    {
        //bool flag = false;
        DataSet dsData = new DataSet();
        try
        {
            //DateTime newFlightDate = DateTime.Parse(FlightDate);
            SQLServer dtb = new SQLServer(Global.GetConnectionString());
            string procedure = "spGetFBLDataForSend";
            
            string[] paramname = new string[] { 
                                                "FlightNo",
                                                "FltDate" };

            object[] paramvalue = new object[] { 
                                                 FlightNo,
                                                 FlightDate};

            SqlDbType[] paramtype = new SqlDbType[] { 
                                                      SqlDbType.VarChar,
                                                      SqlDbType.VarChar };

            dsData = dtb.SelectRecords(procedure, paramname, paramvalue, paramtype);
        }
        catch (Exception ex)
        { }
        //return dsData;
        return dsData;
    }
    #endregion

    #region getFFMUnloadingPort
    public static DataSet getFFMUnloadingPort(string DepartureAirport, string FlightNo, string FlightDate) 
    {
        DataSet ds = new DataSet();
        try 
        {
            DateTime newFlightDate = DateTime.Parse(FlightDate);
            SQLServer dtb = new SQLServer(Global.GetConnectionString());
            string[] pname = new string[3]
            {
                "FlightID",
                "Source",
                "FlightDate"
            };
            object[] pvalue = new object[3]
            {
                FlightNo,
                DepartureAirport,
                newFlightDate.Month.ToString().PadLeft(2,'0')+"/"+ newFlightDate.Day.ToString().PadLeft(2,'0')+"/"+newFlightDate.Year.ToString()

            };
            SqlDbType[] ptype = new SqlDbType[3]
            {
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.DateTime
            };
            ds = dtb.SelectRecords("spExpManiGetAirlineSch1", pname, pvalue, ptype);
            
        }
        catch (Exception ex) 
        { }
        return ds;
    }
    #endregion

    #region getUpdatedFFMData
    public static DataSet getUpdatedFFMData(string DepartureAirport, string FlightNo, string FlightDate)
    {
        DataSet ds = new DataSet();
        DataSet dsData = new DataSet();
        try
        {
            DateTime newFlightDate = DateTime.Parse(FlightDate);
            SQLServer dtb = new SQLServer(Global.GetConnectionString());
            string[] pname = new string[3]
            {
                "FlightID",
                "Source",
                "FlightDate"
            };
            object[] pvalue = new object[3]
            {
                FlightNo,
                DepartureAirport,
                newFlightDate.Month.ToString().PadLeft(2,'0')+"/"+ newFlightDate.Day.ToString().PadLeft(2,'0')+"/"+newFlightDate.Year.ToString()

            };
            SqlDbType[] ptype = new SqlDbType[3]
            {
                SqlDbType.VarChar,
                SqlDbType.VarChar,
                SqlDbType.DateTime
            };
            ds=dtb.SelectRecords("spExpManiGetAirlineSch1",pname,pvalue,ptype);
            if (ds != null) 
            {
                if (ds.Tables.Count > 0) 
                {
                    if (ds.Tables[0].Rows.Count > 0) 
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++) 
                        {
                            DataRow dr = ds.Tables[0].Rows[i];
                            string procedure = "spGetFFMDataForSend";

                            string[] paramname = new string[] { "FltNo",
                                                "ManifestdateFrom",
                                                "ManifestdateTo",
                                                "DepartureAirport",
                                                "dest"};

                            object[] paramvalue = new object[] { FlightNo,
                                                 newFlightDate.Month.ToString().PadLeft(2,'0')+"/"+ newFlightDate.Day.ToString().PadLeft(2,'0')+"/"+newFlightDate.Year.ToString(),
                                                 "",
                                                 DepartureAirport,
                                                 dr[0].ToString()};

                            SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar,
                                                      SqlDbType.DateTime,                                      
                                                      SqlDbType.VarChar,
                                                      SqlDbType.VarChar};

                            dsData = dtb.SelectRecords(procedure, paramname, paramvalue, paramtype);
                        }
                    }
                }
            }
            
        }
        catch (Exception ex)
        { }
        return dsData;
    }
    #endregion

    #region GetAWBDetailsForFSA
    private static DataSet GetAWBDetailsForFSA(string AWBNo,string msg)
    {
        DataSet dsAWB = new DataSet();
        try
        {
            SQLServer dtb = new SQLServer(Global.GetConnectionString());
            string procedure = "spGetAWBDetailsForFSA1";
            string[] paramname = new string[] {"AWBNo","msg"};
            object[] paramvalue = new object[] {AWBNo,msg};
            SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
            dsAWB = dtb.SelectRecords(procedure, paramname, paramvalue, paramtype);
        }
        catch (Exception ex)
        {
            dsAWB = null;
        }
        return dsAWB;
    }
    #endregion

    #region getFlightDetailsForFSA
    private static DataSet getFlightDetailsForFSA(string AWBNo, string FlightNo)
    {
        DataSet dsFlight = new DataSet();
        try
        {
            SQLServer dtb = new SQLServer(Global.GetConnectionString());
            string procedure = "spGetFlightDetailsForFSA";
            string[] paramname = new string[] { "AWBNo","FlightNo"};
            object[] paramvalue = new object[] { AWBNo, FlightNo };
            SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
            dsFlight = dtb.SelectRecords(procedure, paramname, paramvalue, paramtype);
        }
        catch (Exception ex)
        {
            dsFlight = null;
        }
        return dsFlight;
    }
    #endregion

    #region addMsgToOutBox
    public static bool addMsgToOutBox(string subject,string Msg, string FromEmailID, string ToEmailID)
    {
        bool flag = false;
        try
        {
            string procedure = "spInsertMsgToOutbox";
            SQLServer dtb = new SQLServer(Global.GetConnectionString());
            
            string[] paramname = new string[] { "Subject",
                                                "Body",
                                                "FromEmailID",
                                                "ToEmailID",
                                                "@CreatedOn" };
            
            object[] paramvalue = new object[] {subject,
                                                Msg,
                                                FromEmailID,
                                                ToEmailID,
                                                System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")};
            
            SqlDbType[] paramtype = new SqlDbType[] {SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.DateTime };

            flag = dtb.InsertData(procedure, paramname, paramtype, paramvalue);
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }

    public static bool addMsgToOutBox(string subject, string Msg, string FromEmailID, string ToEmailID, string agent, int refNo)
    {
        bool flag = false;
        try
        {
            string procedure = "spInsertMsgToOutbox";
            SQLServer dtb = new SQLServer(Global.GetConnectionString());

            string[] paramname = new string[] { "Subject",
                                                "Body",
                                                "FromEmailID",
                                                "ToEmailID",
                                                "CreatedOn",
                                                "Agentcode",
                                                "refNo"};

            object[] paramvalue = new object[] {subject,
                                                Msg,
                                                FromEmailID,
                                                ToEmailID,
                                                System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                agent,
                                                refNo};

            SqlDbType[] paramtype = new SqlDbType[] {SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.DateTime,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.Int};

            flag = dtb.InsertData(procedure, paramname, paramtype, paramvalue);
         
        }
        catch (Exception ex)
        {
            //clsLog.WriteLog("Error:"+ex.Message);
            flag = false;
        }
        return flag;
    }

    public static bool addMsgToOutBox(string subject, string Msg, string FromEmailID, string ToEmailID, MemoryStream[] Attachments,string[] AttachmentName, string[] AttachmentExtension)
    {
        bool flag = false;
        try
        {
            string procedure = "spInsertMsgToOutbox";
            SQLServer dtb = new SQLServer(Global.GetConnectionString());

            string[] paramname = new string[] { "Subject",
                                                "Body",
                                                "FromEmailID",
                                                "ToEmailID",
                                                "CreatedOn" };

            object[] paramvalue = new object[] {subject,
                                                Msg,
                                                FromEmailID,
                                                ToEmailID,
                                                System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")};

            SqlDbType[] paramtype = new SqlDbType[] {SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.DateTime };

            flag = dtb.InsertData(procedure, paramname, paramtype, paramvalue);
            string[] QueryNames = { "Subject", "Body", "Attachment", "Extension","AttachmentName" };
            SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarBinary, SqlDbType.VarChar,SqlDbType.VarChar };
            if (flag)
            {
                if (Attachments != null && AttachmentExtension != null && AttachmentName!=null)
                {
                       for (int j = 0; j < Attachments.Length; j++)
                        {
                            flag = false;
                            object[] QueryValues = { subject, Msg, Attachments[j].ToArray(), AttachmentExtension[j], AttachmentName[j] };
                            flag = dtb.InsertData("sp_AddAttachmentMessage", QueryNames, QueryTypes, QueryValues);
                        }
                    
                }
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }

    public static bool addMsgToOutBox(string subject, string Msg, string FromEmailID, string ToEmailID, MemoryStream[] Attachments, string[] AttachmentName, string[] AttachmentExtension,DateTime Sysdate)
    {
        bool flag = false;
        try
        {
            string procedure = "spInsertMsgToOutbox";
            SQLServer dtb = new SQLServer(Global.GetConnectionString());

            string[] paramname = new string[] { "Subject",
                                                "Body",
                                                "FromEmailID",
                                                "ToEmailID",
                                                "CreatedOn" };

            object[] paramvalue = new object[] {subject,
                                                Msg,
                                                FromEmailID,
                                                ToEmailID,
                                                Sysdate.ToString("yyyy-MM-dd HH:mm:ss")};

            SqlDbType[] paramtype = new SqlDbType[] {SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.DateTime };

            flag = dtb.InsertData(procedure, paramname, paramtype, paramvalue);
            string[] QueryNames = { "Subject", "Body", "Attachment", "Extension", "AttachmentName" };
            SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarBinary, SqlDbType.VarChar, SqlDbType.VarChar };
            if (flag)
            {
                if (Attachments != null && AttachmentExtension != null && AttachmentName != null)
                {
                    for (int j = 0; j < Attachments.Length; j++)
                    {
                        flag = false;
                        object[] QueryValues = { subject, Msg, Attachments[j].ToArray(), AttachmentExtension[j], AttachmentName[j] };
                        flag = dtb.InsertData("sp_AddAttachmentMessage", QueryNames, QueryTypes, QueryValues);
                    }

                }
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }

    public static bool addMsgToOutBox(string subject, string Msg, string FromEmailID, string ToEmailID,bool isInternal)
    {
        bool flag = false;
        try
        {
            string procedure = "spInsertMsgToOutbox";
            SQLServer dtb = new SQLServer(Global.GetConnectionString());

            string[] paramname = new string[] { "Subject",
                                                "Body",
                                                "FromEmailID",
                                                "ToEmailID",
                                                "isInternal",
                                                "@CreatedOn" };

            object[] paramvalue = new object[] {subject,
                                                Msg,
                                                FromEmailID,
                                                ToEmailID,
                                                isInternal,
                                                System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")};

            SqlDbType[] paramtype = new SqlDbType[] {SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.Bit,
                                                     SqlDbType.DateTime };

            flag = dtb.InsertData(procedure, paramname, paramtype, paramvalue);
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }
    #endregion

    #region InsertFFAInOutbox
    public static bool InsertFFAInOutbox(string arr, string UserName)
    {
        bool flag = false;
        try
        {

            #region Prepare Parameters
            object[] RateCardInfo = new object[2];
            int i = 0;

            RateCardInfo.SetValue(arr, i);
            i++;

            //string UserName = Session["UserName"].ToString();
            RateCardInfo.SetValue(UserName, i);

            #endregion Prepare Parameters

            //string res = "";
            flag = InsertFFAIntoOutbox(RateCardInfo);

            #region Comment
            //if (res != "error")
            //{
            //    lblStatus.Text = res;
            //    lblStatus.ForeColor = Color.Green;
            //}
            //else
            //{
            //    lblStatus.Text = res;
            //    lblStatus.ForeColor = Color.Red;
            //}
            #endregion

        }
        catch (Exception ex)
        {
            flag = false;
            //throw;
        }
        return flag;
    }
    #endregion

    # region InsertFFAIntoOutbox
    private static bool InsertFFAIntoOutbox(object[] RateCardInfo)
    {
        bool flag = false;
        try
        {
            //SQLServer da = new SQLServer(constr);
            SQLServer da = new SQLServer(Global.GetConnectionString());

            DataSet ds = new DataSet();


            string[] ColumnNames = new string[3];
            SqlDbType[] DataType = new SqlDbType[3];
            Object[] Values = new object[3];
            int i = 0;

            i = 0;
            //0
            ColumnNames.SetValue("body", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(RateCardInfo.GetValue(i), i);
            i++;

            ColumnNames.SetValue("username", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue(RateCardInfo.GetValue(i), i);
            i++;

            ColumnNames.SetValue("result", i);
            DataType.SetValue(SqlDbType.VarChar, i);
            Values.SetValue("", i);

            //string res = da.GetStringByProcedure("spSaveFFAToOutbox", ColumnNames, Values, DataType);
            flag = da.UpdateData("spSaveFFAToOutbox", ColumnNames, DataType, Values);
            
            return flag;

            //if (!db.ExecuteProcedure("SP_InsertRateCard", ColumnNames, DataType, Values))
            //    return (-1);
            //else
            //{
            //    return (0);
            //}

        }

        catch (Exception ex)
        {
            return flag;
        }
    }
    # endregion ConfirmSingleAWBInvMatch

    #region getFFMData
    public static DataSet getFFMData(string DepartureAirport, string FlightNo, string FlightDate)
    {
        DataSet dsData = new DataSet();
        try
        {
            DateTime newFlightDate = DateTime.Parse(FlightDate);
            SQLServer dtb = new SQLServer(Global.GetConnectionString());
            string procedure = "SPExpManiGetManifestDetailsforViaFlight";//"spGetFFMDataForSend";

            string[] paramname = new string[] { "FltNo",
                                                "ManifestdateFrom",
                                                "ManifestdateTo",
                                                "DepartureAirport" };

            object[] paramvalue = new object[] { FlightNo,
                                                 newFlightDate.Month.ToString().PadLeft(2,'0')+"/"+ newFlightDate.Day.ToString().PadLeft(2,'0')+"/"+newFlightDate.Year.ToString(),
                                                 "",
                                                 DepartureAirport };

            SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar,
                                                      SqlDbType.DateTime,                                      
                                                      SqlDbType.VarChar,
                                                      SqlDbType.VarChar};

            dsData = dtb.SelectRecords(procedure, paramname, paramvalue, paramtype);
        }
        catch (Exception ex)
        { }
        return dsData;
    }
    #endregion

    #region Encode FHLForSend
    public static string EncodeFHLForSend(string AWBNo,string AWBPrefix,ref string Error)
    {
        string FHLMsg = "" ;
        try
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(Global.GetConnectionString());
            string[] paramname = new string[] { "MAWBNo" ,"MAWBPrefix"};          
            object[] paramvalue = new object[]{AWBNo,AWBPrefix};
            SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
            ds = da.SelectRecords("spGetHAWBSummary", paramname, paramvalue, paramtype);
            //ds = GetFHLData(AWBNo);

            MessageData.fhlinfo fhl = new MessageData.fhlinfo("");
            MessageData.consignmnetinfo[] objTempConsInfo = new MessageData.consignmnetinfo[1];
            MessageData.customsextrainfo[] custominfo = new MessageData.customsextrainfo[0];
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    string WeightCode = string.Empty;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //Master AWB number
                        DataRow dr = ds.Tables[0].Rows[0];
                        fhl.fhlversionnum = "4";
                        fhl.airlineprefix = dr["AWBPrefix"].ToString();
                        fhl.awbnum = dr["AWBNumber"].ToString();
                        fhl.origin = dr["OriginCode"].ToString();
                        fhl.dest = dr["DestinationCode"].ToString();
                        fhl.consigntype = "T";
                        fhl.pcscnt = dr["PiecesCount"].ToString();
                        fhl.weightcode = dr["UOM"].ToString();
                        fhl.weight = dr["GrossWeight"].ToString();
                        WeightCode = dr["UOM"].ToString();
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++) 
                        {
                            DataRow dr = ds.Tables[1].Rows[i];
                            objTempConsInfo[0] = new MessageData.consignmnetinfo("");
                            objTempConsInfo[0].awbnum = dr["HAWBNo"].ToString();
                            objTempConsInfo[0].origin = dr["Origin"].ToString();
                            objTempConsInfo[0].dest = dr["Destination"].ToString();
                            objTempConsInfo[0].consigntype = "T";
                            objTempConsInfo[0].pcscnt = dr["HAWBPcs"].ToString();
                            objTempConsInfo[0].weightcode = WeightCode != "" ? WeightCode : "K";
                            objTempConsInfo[0].weight = dr["HAWBWt"].ToString();
                            objTempConsInfo[0].manifestdesc = dr["Description"].ToString();
                            objTempConsInfo[0].splhandling = dr["SHC"].ToString().Length > 0 ? dr["SHC"].ToString() : "";
                            objTempConsInfo[0].slac = dr["SLAC"].ToString();
                            if (dr["Description"].ToString().Length > 0)
                            {
                                objTempConsInfo[0].freetextGoodDesc = dr["Description"].ToString();
                            }
                            fhl.shippername = dr["ShipperName"].ToString();
                            string ShipAdd = dr["ShipperAddress"].ToString() + dr["ShipperAdd2"].ToString();
                            if (ShipAdd.Length > 35) 
                            {
                                fhl.shipperadd = ShipAdd.Substring(0,35);
                            }
                            else
                            {
                                fhl.shipperadd = dr["ShipperAddress"].ToString() + dr["ShipperAdd2"].ToString();
                            }
                            fhl.shipperplace = dr["ShipperCity"].ToString();
                            fhl.shipperstate = dr["ShipperState"].ToString();
                            fhl.shippercountrycode = dr["ShipperCountry"].ToString();
                            fhl.shipperpostcode = dr["ShipperPincode"].ToString();
                            fhl.shippercontactnum = dr["ShipperTelephone"].ToString();

                            //6 consignee info                    
                            fhl.consname = dr["ConsigneeName"].ToString();
                            string consAdd = dr["ConsigneeAddress"].ToString() + dr["ConsigneeAddress2"].ToString();
                            if (consAdd.Length > 35) 
                            {
                                fhl.consadd = consAdd.Substring(0, 35);
                            }
                            else
                            {
                                fhl.consadd = dr["ConsigneeAddress"].ToString() + dr["ConsigneeAddress2"].ToString();
                            }
                            fhl.consplace = dr["ConsigneeCity"].ToString();
                            fhl.consstate = dr["ConsigneeState"].ToString();
                            fhl.conscountrycode = dr["ConsigneeCountry"].ToString();
                            fhl.conspostcode = dr["ConsigneePincode"].ToString();
                            fhl.conscontactnum = dr["ConsigneeTelephone"].ToString();

                            string msg = cls_Encode_Decode.EncodeFHLforsend(ref fhl, ref objTempConsInfo, ref custominfo);
                            if (msg.Length > 0)
                            {
                                addMsgToOutBox("FHL", msg, "", "");
                                FHLMsg = Environment.NewLine+msg +  "\r\n"+FHLMsg + Environment.NewLine;
                            }
                        }
                        ////shipper-consignee info
                        //DataRow dr = ds.Tables[1].Rows[0];
                        ////5 shipper info

                        //fhl.shippername = dr["ShipperName"].ToString();
                        //fhl.shipperadd = dr["ShipperAddress"].ToString() + dr["ShipperAdd2"].ToString();
                        //fhl.shipperplace = dr["ShipperCity"].ToString();
                        //fhl.shipperstate = dr["ShipperState"].ToString();
                        //fhl.shippercountrycode = dr["ShipperCountry"].ToString();
                        //fhl.shipperpostcode = dr["ShipperPincode"].ToString();
                        //fhl.shippercontactnum = dr["ShipperTelephone"].ToString();

                        ////6 consignee info                    
                        //fhl.consname = dr["ConsigneeName"].ToString();
                        //fhl.consadd = dr["ConsigneeAddress"].ToString() + dr["ConsigneeAddress2"].ToString();
                        //fhl.consplace = dr["ConsigneeCity"].ToString();
                        //fhl.consstate = dr["ConsigneeState"].ToString();
                        //fhl.conscountrycode = dr["ConsigneeCountry"].ToString();
                        //fhl.conspostcode = dr["ConsigneePincode"].ToString();
                        //fhl.conscontactnum = dr["ConsigneeTelephone"].ToString();

                    }
                    //if (ds.Tables[2].Rows.Count > 0)
                    //{
                    //    //Consignment info(houseAWB numbers)

                    //    Array.Resize(ref objTempConsInfo, ds.Tables[2].Rows.Count);
                    //    int i = 0;
                    //    foreach (DataRow dr in ds.Tables[2].Rows)
                    //    {
                    //        objTempConsInfo[i] = new MessageData.consignmnetinfo("");
                    //        objTempConsInfo[i].awbnum = dr["HAWBNo"].ToString();
                    //        objTempConsInfo[i].origin = dr["Origin"].ToString();
                    //        objTempConsInfo[i].dest = dr["Destination"].ToString();
                    //        objTempConsInfo[i].consigntype = "T";
                    //        objTempConsInfo[i].pcscnt = dr["HAWBPcs"].ToString();
                    //        objTempConsInfo[i].weightcode = WeightCode != "" ? WeightCode : "K";
                    //        objTempConsInfo[i].weight = dr["HAWBWt"].ToString();
                    //        objTempConsInfo[i].manifestdesc = dr["Description"].ToString();
                    //        objTempConsInfo[i].splhandling = dr["SHC"].ToString().Length > 0 ? dr["SHC"].ToString() : "";
                    //        i++;
                    //    }

                    //    FHLMsg = cls_Encode_Decode.EncodeFHLforsend(ref fhl, ref objTempConsInfo, ref custominfo);
                    //    if (retval.Length > 0)
                        //{
                        //    flag = addMsgToOutBox("FHL", retval, FromEmailID, ToEmailID);
                        //}
                    //}
                    else 
                    {
                        Error = "Required Data Not Availabe for Message";
                        return FHLMsg;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        return FHLMsg;
    }
    #endregion

    #region GetFHL Data
    public static DataSet GetFHLData(string MAWBNo)
    {
        try
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(Global.GetConnectionString());
            string[] paramname = new string[1];
            paramname[0] = "MAWBNo";
            object[] paramvalue = new object[1];
            paramvalue[0] = MAWBNo;
            SqlDbType[] paramtype = new SqlDbType[1];
            paramtype[0] = SqlDbType.VarChar;
            ds = da.SelectRecords("spGetFHLData", paramname, paramvalue, paramtype);
            return ds;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    #endregion

    #region validateAndInsertFHLData
    private static bool validateAndInsertFHLData(ref MessageData.fhlinfo fhl, ref MessageData.consignmnetinfo[] consinfo, ref MessageData.customsextrainfo[] customextrainfo)
    {
        bool flag = false;
        try
        {
            string AWBNum = fhl.awbnum;
            string AWBPrefix = fhl.airlineprefix;
            BAL.BALHAWBDetails HAWB = new BALHAWBDetails();
            for (int i = 0; i < consinfo.Length; i++)
            {
                string HAWBNo = consinfo[i].awbnum;
                int HAWBPcs = Convert.ToInt16(consinfo[i].pcscnt.ToString());
                int HAWBWt = Convert.ToInt16(consinfo[i].weight.ToString());
                string Origin = consinfo[i].origin;
                string Destination = consinfo[i].dest;
                string description = consinfo[i].manifestdesc;
                string commodity = consinfo[i].commodity;
                string txtDesc = consinfo[i].freetextGoodDesc;
                string SHC = consinfo[i].splhandling;
                string CustID = "";
                string CustName = "";
                string CustAddress = "";
                string City = "";
                string Zipcode = "";
                HAWB.PutHAWBDetails(AWBNum, HAWBNo, HAWBPcs, HAWBWt, description, CustID, CustName, CustAddress, 
                    City, Zipcode, Origin, Destination, SHC,"",AWBPrefix,"","","","",DateTime.Now.ToString());
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }
    #endregion

    #region RemoveSpecialCharacters
    public static string RemoveSpecialCharacters(string str)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in str)
        {
            // if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')

            if ((c >= '0' && c <= '9') || c == '.' || c == '_')
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
    #endregion

    #region EncodeFFRAndPrepareMsg
    public static string EncodeFFR(DataSet ds,ref string Error)
    {
        string strMsg = "";
        try
        {
            MessageData.ffrinfo objFFRInfo = new MessageData.ffrinfo("");
            MessageData.consignmnetinfo consigment = new MessageData.consignmnetinfo("");
            MessageData.FltRoute[] fltRoute = new MessageData.FltRoute[0];
            MessageData.ULDinfo[] objULDInfo = new MessageData.ULDinfo[0];
            MessageData.dimensionnfo[] dimension = new MessageData.dimensionnfo[0];
            string agentcode = "", awbnum = "";
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drAWBRateMaster = ds.Tables[1].Rows[0];

                        DataRow drAWBSummaryMaster = ds.Tables[0].Rows[0];

                        #region PrepareFFRStructureObject

                        //line 1 
                        objFFRInfo.ffrversionnum = "6";

                        #region Consigment Section
                        //line 2
                        awbnum = drAWBSummaryMaster["AWBNumber"].ToString();
                        consigment.airlineprefix = drAWBSummaryMaster["AWBPrefix"].ToString();
                        consigment.awbnum = drAWBSummaryMaster["AWBNumber"].ToString();
                        consigment.origin = drAWBSummaryMaster["OriginCode"].ToString();
                        consigment.dest = drAWBSummaryMaster["DestinationCode"].ToString(); ;
                        consigment.consigntype = "T";
                        consigment.pcscnt = drAWBRateMaster["Pieces"].ToString();
                        consigment.weightcode = "K";
                        consigment.weight = drAWBSummaryMaster["GrossWeight"].ToString();
                        consigment.volumecode = "";
                        consigment.volumeamt = "";
                        consigment.densityindicator = "";
                        consigment.densitygrp = "";
                        consigment.shpdesccode = "";
                        consigment.numshp = "";//drAWBRateMaster["Pieces"].ToString();
                        //objFFRInfo.manifestdesc = drAWBRateMaster["CommodityDesc"].ToString().Length > 1 ? drAWBRateMaster["CommodityDesc"].ToString() : "GEN";
                        consigment.manifestdesc = drAWBRateMaster["CommodityCode"].ToString().Length > 0 ? drAWBRateMaster["CommodityCode"].ToString() : "GEN";
                        consigment.splhandling = "";
                        #endregion


                        //line 3
                        #region FLTROUTE
                        if (ds.Tables.Count > 3)
                        {
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                                {
                                    DataRow drAWBRouteMaster = ds.Tables[3].Rows[i];
                                    MessageData.FltRoute route = new MessageData.FltRoute("");
                                    try
                                    {
                                        DateTime dtTo = new DateTime();
                                        DateTime dtfrom = new DateTime();
                                        string dt = (drAWBRouteMaster["FltDate"].ToString());
                                        //dt = dt + " " + DateTime.Now.ToShortTimeString();
                                        //dtfrom = DateTime.ParseExact(dt,"dd-MM-yyyy",null);
                                        dtTo = DateTime.ParseExact(drAWBRouteMaster["FltDate"].ToString(), "dd/MM/yyyy", null);
                                        //ToDt = dt.ToString();
                                        string day = dt.Substring(0, 2);
                                        string mon = dtTo.ToString("MMM");
                                        //string mon = dt.Substring(3, 2);
                                        string yr = dt.Substring(6, 4);
                                        route.date = day.ToString();
                                        route.month = mon.ToString();

                                    }
                                    catch (Exception ex) { }
                                    route.carriercode = "";
                                    route.fltnum = drAWBRouteMaster["FltNumber"].ToString();

                                    route.fltdept = drAWBRouteMaster["FltOrigin"].ToString();
                                    route.fltarrival = drAWBRouteMaster["FltDestination"].ToString();
                                    route.spaceallotmentcode = "";
                                    route.allotidentification = "";
                                    try
                                    {
                                        string AWBStatus = "";
                                        AWBStatus = drAWBRouteMaster["Status"].ToString();
                                        if (AWBStatus.Trim() != "")
                                        {
                                            if (AWBStatus.Trim().Equals("Q", StringComparison.OrdinalIgnoreCase))
                                            {
                                                route.spaceallotmentcode = "LL";
                                            }
                                            else if (AWBStatus.Trim().Equals("C", StringComparison.OrdinalIgnoreCase))
                                            {
                                                route.spaceallotmentcode = "KK";
                                            }
                                        }
                                        else
                                        {
                                            route.spaceallotmentcode = drAWBRouteMaster["Status"].ToString(); ;
                                        }
                                    }
                                    catch (Exception ex)
                                    { }

                                    Array.Resize(ref fltRoute, fltRoute.Length + 1);
                                    fltRoute[fltRoute.Length - 1] = route;
                                }
                            }
                        }
                        #endregion

                        //line 4
                        objFFRInfo.noofuld = "";
                        //line 5 
                        objFFRInfo.specialservicereq1 = "";
                        objFFRInfo.specialservicereq2 = "";
                        //line 6
                        objFFRInfo.otherserviceinfo1 = "";
                        objFFRInfo.otherserviceinfo2 = "";
                        //line 7
                        objFFRInfo.bookingrefairport = drAWBSummaryMaster["OriginCode"].ToString();
                        objFFRInfo.officefundesignation = "FF";
                        objFFRInfo.companydesignator = "XX";
                        objFFRInfo.bookingfileref = "";
                        objFFRInfo.participentidetifier = "";
                        objFFRInfo.participentcode = "";
                        objFFRInfo.participentairportcity = "";
                        // objFFRInfo.participentairportcity = drAWBSummaryMaster["OriginCode"].ToString();
                        // objFFRInfo.participentcode = "";
                        // objFFRInfo.participentidetifier = "";

                        //line 8
                        #region Dimension
                        //please don't send the dimensions for Auto FFR Setting
                        try
                        {
                            /*
                            if (ds.Tables.Count > 2)
                            {
                                   
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                                    {
                                        MessageData.dimensionnfo dim = new MessageData.dimensionnfo("");
                                        DataRow drAWBDimensions = ds.Tables[2].Rows[i];
                                        dim.weightcode = "";
                                        dim.weight = "";
                                        dim.mesurunitcode = "";
                                        if (drAWBDimensions["MeasureUnit"].ToString().Trim().ToUpper() == "CMS")
                                        {
                                            dim.mesurunitcode = "CMT";
                                        }
                                        else if (drAWBDimensions["MeasureUnit"].ToString().Trim().ToUpper() == "INCHES")
                                        {
                                            dim.mesurunitcode = "INH";
                                        }
                                        dim.length = drAWBDimensions["Length"].ToString();
                                        dim.width = drAWBDimensions["Breadth"].ToString();
                                        dim.height = drAWBDimensions["Height"].ToString();
                                        dim.piecenum = drAWBDimensions["PcsCount"].ToString();
                                        Array.Resize(ref dimension, dimension.Length + 1);
                                        dimension[dimension.Length - 1] = dim;

                                    }
                                }
                            }*/
                        }
                        catch (Exception ex) { }
                        #endregion
                        //line 9 
                        objFFRInfo.servicecode = "";
                        objFFRInfo.rateclasscode = "";
                        objFFRInfo.commoditycode = "";
                        //line 10    
                        try
                        {
                            if (ds.Tables[6].Rows.Count > 0)
                            {
                                DataRow drAWBShipperConsigneeDetails = ds.Tables[6].Rows[0];
                                objFFRInfo.shipperaccnum = "";//[ShipperAccCode]
                                objFFRInfo.shippername = drAWBShipperConsigneeDetails["ShipperName"].ToString();
                                objFFRInfo.shipperadd = drAWBShipperConsigneeDetails["ShipperAddress"].ToString() + " " + drAWBShipperConsigneeDetails["ShipperAdd2"].ToString();
                                objFFRInfo.shipperplace = drAWBShipperConsigneeDetails["ShipperCity"].ToString();
                                objFFRInfo.shipperstate = drAWBShipperConsigneeDetails["ShipperState"].ToString();
                                objFFRInfo.shippercountrycode = drAWBShipperConsigneeDetails["ShipperCountry"].ToString().Substring(0, 2);
                                objFFRInfo.shipperpostcode = drAWBShipperConsigneeDetails["ShipperPincode"].ToString();
                                objFFRInfo.shippercontactidentifier = "TE";
                                objFFRInfo.shippercontactnum = drAWBShipperConsigneeDetails["ShipperTelephone"].ToString();

                                //line 11
                                objFFRInfo.consaccnum = "";//[ConsigAccCode]
                                objFFRInfo.consname = drAWBShipperConsigneeDetails["ConsigneeName"].ToString();
                                objFFRInfo.consadd = drAWBShipperConsigneeDetails["ConsigneeAddress"].ToString() + " " + drAWBShipperConsigneeDetails["ConsigneeAddress2"].ToString(); ;
                                objFFRInfo.consplace = drAWBShipperConsigneeDetails["ConsigneeCity"].ToString();
                                objFFRInfo.consstate = drAWBShipperConsigneeDetails["ConsigneeState"].ToString();
                                objFFRInfo.conscountrycode = drAWBShipperConsigneeDetails["ConsigneeCountry"].ToString().Substring(0, 2);
                                objFFRInfo.conspostcode = drAWBShipperConsigneeDetails["ConsigneePincode"].ToString(); ;
                                objFFRInfo.conscontactidentifier = "TE";
                                objFFRInfo.conscontactnum = drAWBShipperConsigneeDetails["ConsigneeTelephone"].ToString();
                            }
                        }
                        catch (Exception ex) { }
                        //line 12
                        objFFRInfo.custaccnum = "";
                        try
                        {
                            string agtcode =RemoveSpecialCharacters(drAWBSummaryMaster["AgentCode"].ToString());
                            if (agentcode.Length > 7)
                            {
                                objFFRInfo.iatacargoagentcode = agtcode.Substring(0, 7);
                            }
                            else 
                            {
                                objFFRInfo.iatacargoagentcode = agtcode;
                            }
                        }
                        catch (Exception ex) { }
                        objFFRInfo.cargoagentcasscode = "";
                        objFFRInfo.custparticipentidentifier = "";
                        objFFRInfo.custname = "";
                        objFFRInfo.custplace = "";
                        agentcode = drAWBSummaryMaster["AgentCode"].ToString();
                        //line 13
                        objFFRInfo.shiprefnum = "";
                        objFFRInfo.supplemetryshipperinfo1 = "";
                        objFFRInfo.supplemetryshipperinfo2 = "";


                        #endregion

                        strMsg = cls_Encode_Decode.encodeFFRforsend(ref objFFRInfo, ref objULDInfo, ref consigment, ref fltRoute, ref dimension);
                        if (strMsg.Length < 1) {
                            Error = "Error in FFR message";
                        }
                        /*if (strMsg != null)
                        {
                            if (strMsg.Trim() != "")
                            {
                                
                                //flag = addMsgToOutBox("FFR", strMsg, "swapnil@qidtech.com", "", agentcode, 0);
                                //flag = FTP.Saveon72FTP(strMsg, awbnum);                        
                            }
                        }*/
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Error=ex.Message;
        }
        return strMsg;
    }
    #endregion

    #region EncodeFWB
    public static string EncodeFWB(DataSet dsAWB,ref string Error)
    {//(string AirlinePrefix, string AWBNo, string FlightNo, string FromEmailID, string ToEmailID)
        string FWBMsg = "";
        try
        {
            string ErrorMsg = "";
            string agentcode = "";
            MessageData.fwbinfo FWBData = new MessageData.fwbinfo("");
            MessageData.othercharges[] othData = new MessageData.othercharges[0];
            MessageData.otherserviceinfo[] othSrvData = new MessageData.otherserviceinfo[0];
            MessageData.RateDescription[] fwbrates = new MessageData.RateDescription[0];
            MessageData.customsextrainfo[] custominfo = new MessageData.customsextrainfo[0];
            #region Prepare Structure
            try
            {
                if (dsAWB != null)
                {
                    if (dsAWB.Tables.Count > 0)
                    {
                        #region Other Charges
                        try
                        {
                            DataTable DTOtherCh = dsAWB.Tables[5].Copy();
                            if (DTOtherCh.Rows.Count > 0)
                            {
                                for (int i = 0; i < DTOtherCh.Rows.Count; i++)
                                {
                                    MessageData.othercharges tempothData = new MessageData.othercharges("");
                                    //string[] tempcode = DTOtherCh.Rows[i]["ChargeHeadCode"].ToString().Trim().Split('/');
                                    //if (tempcode.Length > 0)
                                    //{
                                    //    tempothData.otherchargecode = tempcode[0].Trim();
                                    //}
                                    string ChargeType = DTOtherCh.Rows[i]["ChargeType"].ToString();
                                    if (ChargeType.Trim() == "DA")
                                    {
                                        tempothData.entitlementcode = "A";
                                        tempothData.otherchargecode = "MA";
                                    }
                                    else if (ChargeType.Trim() == "DC")
                                    {
                                        tempothData.entitlementcode = "C";
                                        tempothData.otherchargecode = "MC";
                                    }
                                    tempothData.indicator = "P";
                                    tempothData.chargeamt = DTOtherCh.Rows[i]["Charge"].ToString();
                                    Array.Resize(ref othData, othData.Length + 1);
                                    othData[othData.Length - 1] = tempothData;
                                }
                            }
                        }
                        catch (Exception ex)
                        { }
                        #endregion

                        DataTable DT1 = dsAWB.Tables[0].Copy();
                        DataTable DT2 = dsAWB.Tables[1].Copy();
                        DataTable DT3 = dsAWB.Tables[2].Copy();
                        DataTable DT4 = dsAWB.Tables[3].Copy();
                        DataTable DT5 = dsAWB.Tables[4].Copy();
                        DataTable DT7 = dsAWB.Tables[6].Copy();
                        DataTable DT8 = dsAWB.Tables[7].Copy();  //Rate Charges
                        DataTable DT9 = dsAWB.Tables[8].Copy();

                        #region RateDescription
                        try
                        {
                            if (DT8 != null)
                            {
                                if (DT8.Rows.Count > 0)
                                {

                                    for (int i = 0; i < DT8.Rows.Count; i++)
                                    {
                                        MessageData.RateDescription rate = new MessageData.RateDescription("");
                                        rate.linenum = (i + 1).ToString();
                                        rate.pcsidentifier = "T";
                                        rate.numofpcs = DT8.Rows[i]["Pcs"].ToString();
                                        rate.weightindicator = "K";
                                        rate.weight = DT8.Rows[i]["Weight"].ToString();
                                        rate.rateclasscode = DT8.Rows[i]["RateClass"].ToString();//RateClass
                                        rate.commoditynumber = DT8.Rows[i]["CommCode"].ToString();
                                        rate.awbweight = DT8.Rows[i]["ChargedWeight"].ToString();
                                        rate.chargerate = DT8.Rows[i]["RatePerKg"].ToString();
                                        rate.chargeamt = DT8.Rows[i]["Total"].ToString();
                                        if (DT2.Rows[0]["CodeDescription"].ToString().Length > 20)
                                            rate.goodsnature = DT2.Rows[0]["CodeDescription"].ToString().Substring(0, 20);
                                        else
                                            rate.goodsnature = DT2.Rows[0]["CodeDescription"].ToString();
                                        
                                        Array.Resize(ref fwbrates, fwbrates.Length + 1);
                                        fwbrates[fwbrates.Length - 1] = rate;
                                    }
                                }
                            }
                        }
                        catch (Exception ex) { }
                        #endregion


                        FWBData.fwbversionnum = "16";
                        FWBData.airlineprefix = DT1.Rows[0]["AWBPrefix"].ToString().Trim();
                        FWBData.awbnum = DT1.Rows[0]["AWBNumber"].ToString().Trim();
                        FWBData.origin = DT1.Rows[0]["OriginCode"].ToString().Trim();
                        FWBData.dest = DT1.Rows[0]["DestinationCode"].ToString().Trim();
                        FWBData.pcscnt = DT1.Rows[0]["PiecesCount"].ToString().Trim();
                        FWBData.weightcode = DT1.Rows[0]["UOM"].ToString().Trim();
                        FWBData.weight = DT1.Rows[0]["GrossWeight"].ToString().Trim();
                        FWBData.consigntype = "T";
                        //FWBData.volumecode = "";
                        //FWBData.volumeamt = "";
                        FWBData.densityindicator = "";
                        FWBData.densitygrp = "";
                        string FlightNo = "";
                        try
                        {
                            FlightNo = DT4.Rows[DT4.Rows.Count - 1]["FltNumber"].ToString();
                            FWBData.carriercode = FlightNo.Substring(0, 2);
                            FWBData.fltnum = FlightNo.Substring(2, FlightNo.Length - 2);

                        }
                        catch (Exception ex) { }
                        try
                        {
                            DateTime FlightDate = DateTime.ParseExact(DT4.Rows[0]["FltDate"].ToString(), "dd/MM/yyyy", null);
                            FWBData.fltday = FlightDate.Day.ToString().PadLeft(2, '0');
                        }
                        catch (Exception) { }
                           agentcode = DT1.Rows[0]["AgentCode"].ToString();
                        try
                         {
                            string agtcode =RemoveSpecialCharacters(DT1.Rows[0]["AgentCode"].ToString());
                            if (agentcode.Length > 7)
                            {
                                FWBData.agentIATAnumber = agtcode.Substring(0, 7);
                            }
                            else 
                            {
                                FWBData.agentIATAnumber = agtcode.PadLeft(7,'0');
                            }
                        }
                        catch (Exception ex) { }
                            
                        FWBData.agentname = DT1.Rows[0]["AgentName"].ToString();
                        FWBData.agentname = FWBData.origin;
                        try
                        {
                            FWBData.currency = DT2.Rows[0]["Currency"].ToString();
                            FWBData.declaredvalue = "NVD";
                            FWBData.declaredcustomvalue = "NCV";                            
                            FWBData.insuranceamount = "XXX";
                            string PaymentMode = "";
                            FWBData.chargedec = PaymentMode;
                            PaymentMode = DT2.Rows[0]["PaymentMode"].ToString();
                            if (PaymentMode.Trim() == "PP")
                            {
                                try
                                {
                                    FWBData.PPweightCharge = DT8.Rows[0]["FrIATA"].ToString();
                                    FWBData.PPTaxesCharge = DT8.Rows[0]["ServTax"].ToString();
                                    FWBData.PPOCDC = DT8.Rows[0]["OCDueCar"].ToString();
                                    FWBData.PPOCDA = DT8.Rows[0]["OCDueAgent"].ToString();
                                    FWBData.PPTotalCharges = DT8.Rows[0]["Total"].ToString();
                                }
                                catch (Exception ex)
                                { }
                            }
                            else
                            {
                                try
                                {
                                    FWBData.CCweightCharge = DT8.Rows[0]["FrIATA"].ToString();
                                    FWBData.CCTaxesCharge = DT8.Rows[0]["ServTax"].ToString();
                                    FWBData.CCOCDC = DT8.Rows[0]["OCDueCar"].ToString();
                                    FWBData.CCOCDA = DT8.Rows[0]["OCDueAgent"].ToString();
                                    FWBData.CCTotalCharges = DT8.Rows[0]["Total"].ToString();
                                }
                                catch (Exception ex)
                                { }
                            }
                        }
                        catch (Exception ex)
                        { }
                        //string ExecutionDate = DateTime.Parse(DT1.Rows[0]["ExecutionDate"].ToString()).ToString("dd/MM/yyyy");
                        //string ExecutionDate = DT1.Rows[0]["ExecutionDate"].ToString();
                        //ExecutionDate = DateTime.Parse(ExecutionDate.ToString("MM/dd/yyyy"));
                        try
                        {
                            string[] ExecutionDate = DT1.Rows[0]["ExecutionDate"].ToString().Split('/');
                            if (ExecutionDate.Length >= 3)
                            {
                                FWBData.carrierdate = ExecutionDate[0].PadLeft(2, '0'); ;
                                FWBData.carriermonth = new DateTime(2010, int.Parse(ExecutionDate[1].PadLeft(2, '0').ToString()), 1).ToString("MMM", CultureInfo.InvariantCulture).ToUpper();                               
                                FWBData.carrieryear = ExecutionDate[2].PadLeft(4, '0').Substring(2, 2);
                            }
                        }
                        catch (Exception ex)
                        { }

                        FWBData.carrierplace = DT1.Rows[0]["ExecutedAt"].ToString();

                        try
                        {
                            if (DT7.Rows.Count > 0)
                            {
                                FWBData.shippername = DT7.Rows[0]["ShipperName"].ToString();
                                if (DT7.Rows[0]["ShipperAddress"].ToString().Length > 35)
                                    FWBData.shipperadd = DT7.Rows[0]["ShipperAddress"].ToString().Substring(0, 35);
                                else
                                    FWBData.shipperadd = DT7.Rows[0]["ShipperAddress"].ToString();

                                FWBData.shipperplace = DT7.Rows[0]["ShipperCity"].ToString();
                                FWBData.shipperstate = DT7.Rows[0]["ShipperState"].ToString();
                                FWBData.shippercountrycode = DT7.Rows[0]["ShipperCountry"].ToString();
                                FWBData.shippercontactnum = DT7.Rows[0]["ShipperTelephone"].ToString();
                                

                                FWBData.consname = DT7.Rows[0]["ConsigneeName"].ToString();
                                if (DT7.Rows[0]["ConsigneeAddress"].ToString().Length > 35)
                                    FWBData.consadd = DT7.Rows[0]["ConsigneeAddress"].ToString().Substring(0, 35);

                                else
                                    FWBData.consadd = DT7.Rows[0]["ConsigneeAddress"].ToString();
                                FWBData.consplace = DT7.Rows[0]["ConsigneeCity"].ToString();
                                FWBData.consstate = DT7.Rows[0]["ConsigneeState"].ToString();
                                FWBData.conscountrycode = DT7.Rows[0]["ConsigneeCountry"].ToString();
                                FWBData.conscontactnum = DT7.Rows[0]["ConsigneeTelephone"].ToString();
                               
                               
                                if(FWBData.shippercontactnum.Length > 0){ FWBData.shippercontactidentifier = "TE";}
                                if (FWBData.conscontactnum.Length > 0) { FWBData.conscontactidentifier = "TE"; }
                                try 
                                {
                                    //FWBData.senderairport = DT1.Rows[0]["OriginCode"].ToString().Trim();
                                    FWBData.senderPariticipentAirport = DT1.Rows[0]["OriginCode"].ToString().Trim();
                                    FWBData.senderParticipentIdentifier = "AGT";
                                    string str=RemoveSpecialCharacters(DT1.Rows[0]["AgentCode"].ToString());
                                    if (str.Length > 7)
                                        FWBData.senderParticipentCode = str.Substring(0, 7);
                                    else
                                        FWBData.senderParticipentCode = str.PadLeft(7,'0');

                                }
                                catch (Exception ex) { }
                            }
                            else 
                            {
                                Error = "No Shipper/Consignee Info Availabe for FWB";
                                return FWBMsg;
                            }
                        }
                        catch (Exception ex)
                        { }

                    }
                }
            }
            catch (Exception ex)
            { }
            #endregion

            
            FWBMsg = cls_Encode_Decode.EncodeFWBForSend(ref FWBData, ref othData, ref othSrvData, ref fwbrates, ref custominfo);
            //EncodeFWBForSend(ref MessageData.fwbinfo fwbdata, ref MessageData.othercharges[] fwbOtherCharge, ref MessageData.otherserviceinfo othinfoarray, ref MessageData.RateDescription[] fwbrate)

            //if (FWBMsg.Trim() != "")
            //{
            //    flag = addMsgToOutBox("FWB", FWBMsg, "swapnil@qidtech.com", "", agentcode, refNo);
            //}

        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        return FWBMsg;
    }
    #endregion

    #region Encode FHLForSend
    public static string EncodeFHLForSend(DataSet ds,ref string Error)//(string AWBNo, string FromEmailID, string ToEmailID)
    {
        string FHLMsg = "";
        try
        {
            //   ds = GetFHLData(AWBNo);
            string agentcode = "";
            MessageData.fhlinfo fhl = new MessageData.fhlinfo("");
            MessageData.consignmnetinfo[] objTempConsInfo = new MessageData.consignmnetinfo[0];
            MessageData.customsextrainfo[] custominfo = new MessageData.customsextrainfo[0];
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {//Master AWB number
                        DataRow dr = ds.Tables[0].Rows[0];
                        fhl.airlineprefix = dr["AWBPrefix"].ToString();
                        fhl.awbnum = dr["AWBNumber"].ToString();
                        fhl.origin = dr["OriginCode"].ToString();
                        fhl.dest = dr["DestinationCode"].ToString();
                        fhl.consigntype = "T";
                        fhl.pcscnt = dr["PiecesCount"].ToString();
                        fhl.weightcode = "K";
                        fhl.weight = dr["GrossWeight"].ToString();
                        agentcode = dr["AgentCode"].ToString();
                    }
                    if (ds.Tables[6].Rows.Count > 0)
                    {
                        //shipper-consignee info
                        DataRow dr = ds.Tables[6].Rows[0];

                        //5 shipper info

                        fhl.shippername = dr["ShipperName"].ToString();
                        fhl.shipperadd = dr["ShipperAddress"].ToString();
                        fhl.shipperplace = "";
                        fhl.shipperstate = "";
                        fhl.shippercountrycode = dr["ShipperCountry"].ToString();
                        fhl.shipperpostcode = "";
                        fhl.shippercontactnum = dr["ShipperTelephone"].ToString();

                        //6 consignee info                    
                        fhl.consname = dr["ConsigneeName"].ToString();
                        fhl.consadd = dr["ConsigneeAddress"].ToString();
                        fhl.consplace = "";
                        fhl.consstate = "";
                        fhl.conscountrycode = dr["ConsigneeCountry"].ToString();
                        fhl.conspostcode = "";
                        fhl.conscontactnum = dr["ConsigneeTelephone"].ToString();

                    }
                    if (ds.Tables[10].Rows.Count > 0)
                    {
                        //Consignment info(houseAWB numbers)

                        Array.Resize(ref objTempConsInfo, ds.Tables[10].Rows.Count);
                        int i = 0;
                        foreach (DataRow dr in ds.Tables[10].Rows)
                        {
                            objTempConsInfo[i] = new MessageData.consignmnetinfo("");
                            objTempConsInfo[i].awbnum = dr["HAWBNo"].ToString();
                            objTempConsInfo[i].origin = dr["Origin"].ToString();
                            objTempConsInfo[i].dest = dr["Destination"].ToString();
                            objTempConsInfo[i].consigntype = "";
                            objTempConsInfo[i].pcscnt = dr["HAWBPcs"].ToString();
                            objTempConsInfo[i].weightcode = "K";
                            objTempConsInfo[i].weight = dr["HAWBWt"].ToString();
                            objTempConsInfo[i].manifestdesc = dr["Description"].ToString();
                            objTempConsInfo[i].splhandling = dr["SHC"].ToString().Length > 0 ? dr["SHC"].ToString() : "";
                            i++;
                        }

                         FHLMsg = cls_Encode_Decode.EncodeFHLforsend(ref fhl, ref objTempConsInfo, ref custominfo);
                        //if (retval.Length > 0)
                        //{
                        //    flag = addMsgToOutBox("FHL", retval, "swapnil@qidtech.com", "", agentcode, refNo);
                        //}
                    }
                    else
                    {
                        Error = "Required Data Not Availabe for Message";
                        return FHLMsg;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        return FHLMsg;
    }
    #endregion

    public static bool EncodeFSUOffloadpublic(string ActFlightNo, string OffloadFltNo, string OffloadLoc, string AWBNo, int ActPcs, double ActWt, int OffloadPcs, double OffloadWt, string Offloadedby, string POL, string POU, string FlightVersion, DateTime OffLoadFltDt, string Remarks, string Mode, DateTime ActFlightDate, DateTime UpdatedOn, string PartnerCode, string PartnerType, string ULDNo) 
    {
        bool flag = false;
        string OperType="DIS";
        try 
        {
            string CarrierCode =PartnerCode;
            MessageData.FSAInfo objFSA = new MessageData.FSAInfo("");
            MessageData.CommonStruct[] objFSANode = new MessageData.CommonStruct[0];
            MessageData.CommonStruct objComnStruct = new MessageData.CommonStruct("");
            MessageData.customsextrainfo[] objCustomInfo = new MessageData.customsextrainfo[0];
            MessageData.ULDinfo[] objULDInfo = new MessageData.ULDinfo[0];
            MessageData.otherserviceinfo[] objOtherSercInfo = new MessageData.otherserviceinfo[0];

            #region PrepareStructure

            try
            {
                objFSA.airlineprefix = AWBNo.Substring(0,3);
                objFSA.fsaversion = "14";
                objFSA.awbnum = AWBNo.Substring(3);
                objFSA.origin = POL.ToUpper();
                objFSA.dest = POU.ToUpper();
                objFSA.pcscnt = ActPcs.ToString();
                objFSA.consigntype = "T";
                objFSA.weight = ActWt.ToString();
                objFSA.weightcode = "K";
                //objFSA.totalpcscnt = drAWB["PiecesCount"].ToString();

                DateTime FlightDate = System.DateTime.Today;
                try
                {
                    FlightDate = ActFlightDate;
                }
                catch (Exception ex)
                {
                    FlightDate = System.DateTime.Today;
                }

                #region DIS
                objComnStruct.messageprefix = OperType.Trim();
                objComnStruct.carriercode = CarrierCode.Trim();
                objComnStruct.fltday = FlightDate.ToString("dd");
                objComnStruct.fltmonth = FlightDate.ToString("MMM").ToUpper();
                objComnStruct.flttime = "";
                objComnStruct.airportcode = OffloadLoc.ToUpper();// drFlight["FltOrigin"].ToString();
                objComnStruct.pcsindicator = "T";
                objComnStruct.numofpcs = OffloadPcs.ToString();
                objComnStruct.weightcode = "K";
                objComnStruct.weight = OffloadWt.ToString();
                objComnStruct.name = "";
                objComnStruct.infocode = "OFLD";
                #endregion
                    
                    
                

            }
            catch (Exception ex)
            {
                clsLog.WriteLog("Error FSU Prepare:" + ex.Message);
            }

            Array.Resize(ref objFSANode, objFSANode.Length + 1);
            objFSANode[objFSANode.Length - 1] = objComnStruct;
            #endregion

            string msgFSA = cls_Encode_Decode.EncodeFSUforSend(ref objFSA, ref objFSANode, ref objCustomInfo, ref objULDInfo, ref objOtherSercInfo);
            //clsLog.WriteLog(msgFSA);
            if (addMsgToOutBox("FSU/" + OperType, msgFSA, "", ""))
            {
                flag = true;
            }
            else
            {
                clsLog.WriteLog("Error: FSU/RCS not addded for send");
            }

                
                
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }

    #region Encode PHLForSend
    public static string EncodePHLForSend(DataSet ds, ref string Error)//(string AWBNo, string FromEmailID, string ToEmailID)
    {
        string PHLMsg = "";
        try
        {
            //   ds = GetPHLData(AWBNo);
            string agentcode = "";
            MessageData.fhlinfo phl = new MessageData.fhlinfo("");
            MessageData.consignmnetinfo[] objTempConsInfo = new MessageData.consignmnetinfo[0];
            MessageData.customsextrainfo[] custominfo = new MessageData.customsextrainfo[0];
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {//Master AWB number
                        DataRow dr = ds.Tables[0].Rows[0];
                        phl.airlineprefix = dr["AWBPrefix"].ToString();
                        phl.awbnum = dr["AWBNumber"].ToString();
                        phl.origin = dr["OriginCode"].ToString();
                        phl.dest = dr["DestinationCode"].ToString();
                        phl.consigntype = "T";
                        phl.pcscnt = dr["PiecesCount"].ToString();
                        phl.weightcode = "K";
                        phl.weight = dr["GrossWeight"].ToString();
                        agentcode = dr["AgentCode"].ToString();
                    }
                    if (ds.Tables[6].Rows.Count > 0)
                    {
                        //shipper-consignee info
                        DataRow dr = ds.Tables[6].Rows[0];

                        //5 shipper info

                        phl.shippername = dr["ShipperName"].ToString();
                        phl.shipperadd = dr["ShipperAddress"].ToString();
                        phl.shipperplace = "";
                        phl.shipperstate = "";
                        phl.shippercountrycode = dr["ShipperCountry"].ToString();
                        phl.shipperpostcode = "";
                        phl.shippercontactnum = dr["ShipperTelephone"].ToString();

                        //6 consignee info                    
                        phl.consname = dr["ConsigneeName"].ToString();
                        phl.consadd = dr["ConsigneeAddress"].ToString();
                        phl.consplace = "";
                        phl.consstate = "";
                        phl.conscountrycode = dr["ConsigneeCountry"].ToString();
                        phl.conspostcode = "";
                        phl.conscontactnum = dr["ConsigneeTelephone"].ToString();

                    }
                    if (ds.Tables[10].Rows.Count > 0)
                    {
                        //Consignment info(houseAWB numbers)

                        Array.Resize(ref objTempConsInfo, ds.Tables[10].Rows.Count);
                        int i = 0;
                        foreach (DataRow dr in ds.Tables[10].Rows)
                        {
                            objTempConsInfo[i] = new MessageData.consignmnetinfo("");
                            objTempConsInfo[i].awbnum = dr["HAWBNo"].ToString();
                            objTempConsInfo[i].origin = dr["Origin"].ToString();
                            objTempConsInfo[i].dest = dr["Destination"].ToString();
                            objTempConsInfo[i].consigntype = "";
                            objTempConsInfo[i].pcscnt = dr["HAWBPcs"].ToString();
                            objTempConsInfo[i].weightcode = "K";
                            objTempConsInfo[i].weight = dr["HAWBWt"].ToString();
                            objTempConsInfo[i].manifestdesc = dr["Description"].ToString();
                            objTempConsInfo[i].splhandling = dr["SHC"].ToString().Length > 0 ? dr["SHC"].ToString() : "";
                            i++;
                        }

                        PHLMsg = cls_Encode_Decode.EncodePHLforsend(ref phl, ref objTempConsInfo, ref custominfo);
                        //if (retval.Length > 0)
                        //{
                        //    flag = addMsgToOutBox("PHL", retval, "swapnil@qidtech.com", "", agentcode, refNo);
                        //}
                    }
                    else
                    {
                        Error = "Required Data Not Availabe for Message";
                        return PHLMsg;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        return PHLMsg;
    }
    #endregion

    #region EncodePWB
    public static string EncodePWB(DataSet dsAWB, ref string Error)
    {//(string AirlinePrefix, string AWBNo, string FlightNo, string FromEmailID, string ToEmailID)
        string PWBMsg = "";
        try
        {
            string ErrorMsg = "";
            string agentcode = "";
            MessageData.fwbinfo PWBData = new MessageData.fwbinfo("");
            MessageData.othercharges[] othData = new MessageData.othercharges[0];
            MessageData.otherserviceinfo[] othSrvData = new MessageData.otherserviceinfo[0];
            MessageData.RateDescription[] fwbrates = new MessageData.RateDescription[0];
            MessageData.customsextrainfo[] custominfo = new MessageData.customsextrainfo[0];
            #region Prepare Structure
            try
            {
                if (dsAWB != null)
                {
                    if (dsAWB.Tables.Count > 0)
                    {
                        #region Other Charges
                        try
                        {
                            DataTable DTOtherCh = dsAWB.Tables[5].Copy();
                            if (DTOtherCh.Rows.Count > 0)
                            {
                                for (int i = 0; i < DTOtherCh.Rows.Count; i++)
                                {
                                    MessageData.othercharges tempothData = new MessageData.othercharges("");
                                    //string[] tempcode = DTOtherCh.Rows[i]["ChargeHeadCode"].ToString().Trim().Split('/');
                                    //if (tempcode.Length > 0)
                                    //{
                                    //    tempothData.otherchargecode = tempcode[0].Trim();
                                    //}
                                    string ChargeType = DTOtherCh.Rows[i]["ChargeType"].ToString();
                                    if (ChargeType.Trim() == "DA")
                                    {
                                        tempothData.entitlementcode = "A";
                                        tempothData.otherchargecode = "MA";
                                    }
                                    else if (ChargeType.Trim() == "DC")
                                    {
                                        tempothData.entitlementcode = "C";
                                        tempothData.otherchargecode = "MC";
                                    }
                                    tempothData.indicator = "P";
                                    tempothData.chargeamt = DTOtherCh.Rows[i]["Charge"].ToString();
                                    Array.Resize(ref othData, othData.Length + 1);
                                    othData[othData.Length - 1] = tempothData;
                                }
                            }
                        }
                        catch (Exception ex)
                        { }
                        #endregion

                        DataTable DT1 = dsAWB.Tables[0].Copy();
                        DataTable DT2 = dsAWB.Tables[1].Copy();
                        DataTable DT3 = dsAWB.Tables[2].Copy();
                        DataTable DT4 = dsAWB.Tables[3].Copy();
                        DataTable DT5 = dsAWB.Tables[4].Copy();
                        DataTable DT7 = dsAWB.Tables[6].Copy();
                        DataTable DT8 = dsAWB.Tables[7].Copy();  //Rate Charges
                        DataTable DT9 = dsAWB.Tables[8].Copy();

                        #region RateDescription
                        try
                        {
                            if (DT8 != null)
                            {
                                if (DT8.Rows.Count > 0)
                                {

                                    for (int i = 0; i < DT8.Rows.Count; i++)
                                    {
                                        MessageData.RateDescription rate = new MessageData.RateDescription("");
                                        rate.linenum = (i + 1).ToString();
                                        rate.pcsidentifier = "P";
                                        rate.numofpcs = DT8.Rows[i]["Pcs"].ToString();
                                        rate.weightindicator = "K";
                                        rate.weight = DT8.Rows[i]["Weight"].ToString();
                                        rate.rateclasscode = "";//RateClass
                                        rate.commoditynumber = DT8.Rows[i]["CommCode"].ToString();
                                        rate.awbweight = DT8.Rows[i]["ChargedWeight"].ToString();
                                        rate.chargerate = DT8.Rows[i]["RatePerKg"].ToString();
                                        rate.chargeamt = DT8.Rows[i]["Total"].ToString();
                                        rate.goodsnature = DT2.Rows[0]["CodeDescription"].ToString();
                                        Array.Resize(ref fwbrates, fwbrates.Length + 1);
                                        fwbrates[fwbrates.Length - 1] = rate;
                                    }
                                }
                            }
                        }
                        catch (Exception ex) { }
                        #endregion


                        PWBData.fwbversionnum = "16";
                        PWBData.airlineprefix = DT1.Rows[0]["AWBPrefix"].ToString().Trim();
                        PWBData.awbnum = DT1.Rows[0]["AWBNumber"].ToString().Trim();
                        PWBData.origin = DT1.Rows[0]["OriginCode"].ToString().Trim();
                        PWBData.dest = DT1.Rows[0]["DestinationCode"].ToString().Trim();
                        PWBData.pcscnt = DT1.Rows[0]["PiecesCount"].ToString().Trim();
                        PWBData.weightcode = "K";
                        PWBData.weight = DT1.Rows[0]["ChargedWeight"].ToString().Trim();
                        //PWBData.volumecode = "";
                        //PWBData.volumeamt = "";
                        PWBData.densityindicator = "";
                        PWBData.densitygrp = "";
                        string FlightNo = "";
                        try
                        {
                            FlightNo = DT4.Rows[0]["FltNumber"].ToString();
                            PWBData.carriercode = FlightNo.Substring(0, 2);
                            PWBData.fltnum = FlightNo.Substring(2, FlightNo.Length - 2);

                        }
                        catch (Exception ex) { }
                        try
                        {
                            DateTime FlightDate = DateTime.ParseExact(DT4.Rows[0]["FltDate"].ToString(), "dd/MM/yyyy", null);
                            PWBData.fltday = FlightDate.Day.ToString().PadLeft(2, '0');
                        }
                        catch (Exception) { }
                        agentcode = DT1.Rows[0]["AgentCode"].ToString();
                        try
                        {
                            PWBData.agentIATAnumber = RemoveSpecialCharacters(DT1.Rows[0]["AgentCode"].ToString()).Substring(0, 7);
                        }
                        catch (Exception ex) { }
                        PWBData.agentname = DT1.Rows[0]["AgentName"].ToString();
                        PWBData.agentname = PWBData.origin;
                        try
                        {
                            PWBData.currency = DT2.Rows[0]["Currency"].ToString();
                            PWBData.declaredvalue = "NVD";
                            PWBData.declaredcustomvalue = "NCV";
                            PWBData.insuranceamount = "XXX";
                            string PaymentMode = "";
                            PWBData.chargedec = PaymentMode;
                            PaymentMode = DT2.Rows[0]["PaymentMode"].ToString();
                            if (PaymentMode.Trim() == "PP")
                            {
                                try
                                {
                                    PWBData.PPweightCharge = DT8.Rows[0]["FrIATA"].ToString();
                                    PWBData.PPTaxesCharge = DT8.Rows[0]["ServTax"].ToString();
                                    PWBData.PPOCDC = DT8.Rows[0]["OCDueCar"].ToString();
                                    PWBData.PPOCDA = DT8.Rows[0]["OCDueAgent"].ToString();
                                    PWBData.PPTotalCharges = DT8.Rows[0]["Total"].ToString();
                                }
                                catch (Exception ex)
                                { }
                            }
                            else
                            {
                                try
                                {
                                    PWBData.CCweightCharge = DT8.Rows[0]["FrIATA"].ToString();
                                    PWBData.CCTaxesCharge = DT8.Rows[0]["ServTax"].ToString();
                                    PWBData.CCOCDC = DT8.Rows[0]["OCDueCar"].ToString();
                                    PWBData.CCOCDA = DT8.Rows[0]["OCDueAgent"].ToString();
                                    PWBData.CCTotalCharges = DT8.Rows[0]["Total"].ToString();
                                }
                                catch (Exception ex)
                                { }
                            }
                        }
                        catch (Exception ex)
                        { }
                        //string ExecutionDate = DateTime.Parse(DT1.Rows[0]["ExecutionDate"].ToString()).ToString("dd/MM/yyyy");
                        //string ExecutionDate = DT1.Rows[0]["ExecutionDate"].ToString();
                        //ExecutionDate = DateTime.Parse(ExecutionDate.ToString("MM/dd/yyyy"));
                        try
                        {
                            string[] ExecutionDate = DT1.Rows[0]["ExecutionDate"].ToString().Split('/');
                            if (ExecutionDate.Length >= 3)
                            {
                                PWBData.carrierdate = ExecutionDate[0].PadLeft(2, '0'); ;
                                PWBData.carriermonth = ExecutionDate[1].PadLeft(2, '0');
                                PWBData.carrieryear = ExecutionDate[2].PadLeft(4, '0').Substring(2, 2);
                            }
                        }
                        catch (Exception ex)
                        { }

                        PWBData.carrierplace = DT1.Rows[0]["ExecutedAt"].ToString();

                        try
                        {
                            if (DT7.Rows.Count > 0)
                            {
                                PWBData.shippername = DT7.Rows[0]["ShipperName"].ToString();
                                PWBData.shipperadd = DT7.Rows[0]["ShipperAddress"].ToString();
                                PWBData.shippercountrycode = DT7.Rows[0]["ShipperCountry"].ToString();
                                PWBData.shippercontactnum = DT7.Rows[0]["ShipperTelephone"].ToString();
                                PWBData.consname = DT7.Rows[0]["ConsigneeName"].ToString();
                                PWBData.consadd = DT7.Rows[0]["ConsigneeAddress"].ToString();
                                PWBData.conscountrycode = DT7.Rows[0]["ConsigneeCountry"].ToString();
                                PWBData.conscontactnum = DT7.Rows[0]["ConsigneeTelephone"].ToString();
                                PWBData.senderairport = DT1.Rows[0]["OriginCode"].ToString().Trim();

                                if (PWBData.shippercontactnum.Length > 1) { PWBData.shippercontactidentifier = "TE"; }
                                if (PWBData.conscontactnum.Length > 1) { PWBData.conscontactidentifier = "TE"; }
                                try
                                {
                                    PWBData.senderParticipentIdentifier = "AGT";
                                    PWBData.senderParticipentCode = RemoveSpecialCharacters(DT1.Rows[0]["AgentCode"].ToString()).Substring(0, 7);
                                }
                                catch (Exception ex) { }
                            }
                            else
                            {
                                Error = "No Shipper/Consignee Info Availabe for PWB";
                                return PWBMsg;
                            }
                        }
                        catch (Exception ex)
                        { }

                    }
                }
            }
            catch (Exception ex)
            { }
            #endregion


            PWBMsg = cls_Encode_Decode.EncodePWBForSend(ref PWBData, ref othData, ref othSrvData, ref fwbrates, ref custominfo);
            //EncodePWBForSend(ref MessageData.fwbinfo fwbdata, ref MessageData.othercharges[] fwbOtherCharge, ref MessageData.otherserviceinfo othinfoarray, ref MessageData.RateDescription[] fwbrate)

            //if (PWBMsg.Trim() != "")
            //{
            //    flag = addMsgToOutBox("PWB", PWBMsg, "swapnil@qidtech.com", "", agentcode, refNo);
            //}

        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        return PWBMsg;
    }
    #endregion

    #region Encode FHLForSend
    public static string EncodePHLForSend(string AWBNo, ref string Error)
    {
        string PHLMsg = "";
        try
        {
            DataSet ds = new DataSet();
            ds = GetFHLData(AWBNo);

            MessageData.fhlinfo fhl = new MessageData.fhlinfo("");
            MessageData.consignmnetinfo[] objTempConsInfo = new MessageData.consignmnetinfo[0];
            MessageData.customsextrainfo[] custominfo = new MessageData.customsextrainfo[0];
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //Master AWB number
                        DataRow dr = ds.Tables[0].Rows[0];
                        fhl.airlineprefix = dr["AWBPrefix"].ToString();
                        fhl.awbnum = dr["AWBNumber"].ToString();
                        fhl.origin = dr["OriginCode"].ToString();
                        fhl.dest = dr["DestinationCode"].ToString();
                        fhl.consigntype = "T";
                        fhl.pcscnt = dr["PiecesCount"].ToString();
                        fhl.weightcode = "K";
                        fhl.weight = dr["GrossWeight"].ToString();
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        //shipper-consignee info
                        DataRow dr = ds.Tables[1].Rows[0];
                        //5 shipper info

                        fhl.shippername = dr["ShipperName"].ToString();
                        fhl.shipperadd = dr["ShipperAddress"].ToString() + dr["ShipperAdd2"].ToString();
                        fhl.shipperplace = dr["ShipperCity"].ToString();
                        fhl.shipperstate = dr["ShipperState"].ToString();
                        fhl.shippercountrycode = dr["ShipperCountry"].ToString();
                        fhl.shipperpostcode = dr["ShipperPincode"].ToString();
                        fhl.shippercontactnum = dr["ShipperTelephone"].ToString();

                        //6 consignee info                    
                        fhl.consname = dr["ConsigneeName"].ToString();
                        fhl.consadd = dr["ConsigneeAddress"].ToString() + dr["ConsigneeAddress2"].ToString();
                        fhl.consplace = dr["ConsigneeCity"].ToString();
                        fhl.consstate = dr["ConsigneeState"].ToString();
                        fhl.conscountrycode = dr["ConsigneeCountry"].ToString();
                        fhl.conspostcode = dr["ConsigneePincode"].ToString();
                        fhl.conscontactnum = dr["ConsigneeTelephone"].ToString();

                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        //Consignment info(houseAWB numbers)

                        Array.Resize(ref objTempConsInfo, ds.Tables[2].Rows.Count);
                        int i = 0;
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            objTempConsInfo[i] = new MessageData.consignmnetinfo("");
                            objTempConsInfo[i].awbnum = dr["HAWBNo"].ToString();
                            objTempConsInfo[i].origin = dr["Origin"].ToString();
                            objTempConsInfo[i].dest = dr["Destination"].ToString();
                            objTempConsInfo[i].consigntype = "";
                            objTempConsInfo[i].pcscnt = dr["HAWBPcs"].ToString();
                            objTempConsInfo[i].weightcode = "K";
                            objTempConsInfo[i].weight = dr["HAWBWt"].ToString();
                            objTempConsInfo[i].manifestdesc = dr["Description"].ToString();
                            objTempConsInfo[i].splhandling = dr["SHC"].ToString().Length > 0 ? dr["SHC"].ToString() : "";
                            i++;
                        }

                        PHLMsg = cls_Encode_Decode.EncodePHLforsend(ref fhl, ref objTempConsInfo, ref custominfo);
                        //if (retval.Length > 0)
                        //{
                        //    flag = addMsgToOutBox("PHL", retval, FromEmailID, ToEmailID);
                        //}
                    }
                    else
                    {
                        Error = "Required Data Not Availabe for Message";
                        return PHLMsg;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        return PHLMsg;
    }
    #endregion

    #region SCM Encoding for Send
    public static bool SCMEncodingForSend(DataSet ds, string FromEmailID, string ToEmailID)
    {
        bool flag = false;
        try
        {

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //clsLog.WriteLog("Encode FSU for Send");
                        DataRow drULD = ds.Tables[0].Rows[0];
                        //DataRow drFlight = dsFlight.Tables[0].Rows[0];
                        MessageData.SCM objSCM = new MessageData.SCM();
                        MessageData.ULDStockInfo[] objULDStockInfo = new MessageData.ULDStockInfo[0];


                        #region PrepareStructure

                        try
                        {
                            objSCM.MessageIdentifier = "SCM";
                            objSCM.AirportCode = drULD["AirportCode"].ToString();
                            objSCM.Day = drULD["Day"].ToString();
                            objSCM.Month = drULD["Month"].ToString();
                            objSCM.Time = drULD["Time"].ToString();
                            objSCM.SupplemenataryInfoId = "SI";
                            string[] InfoText = new string[ds.Tables[0].Rows.Count];
                            for (int f = 0; f < ds.Tables[0].Rows.Count; f++)
                            {
                                InfoText[f] = ds.Tables[0].Rows[f]["Text"].ToString();
                            }
                            objSCM.ULDInfoText = InfoText;

                            //Looping through different ULD Types
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                //Filtering by ULDType
                                DataTable dtULD = ds.Tables[2].Select("ULDType='" + ds.Tables[1].Rows[i]["ULDType"].ToString() + "'").CopyToDataTable();
                                //Getting count of ULDInfo 
                                int ULDrows = Int32.Parse(Math.Round((Convert.ToDouble(dtULD.Rows.Count / 6)), 0).ToString());
                                //Getting last row Total count of ULDInfo
                                int LastRowTotalCnt = Convert.ToInt32(Math.Round((Convert.ToDouble(dtULD.Rows.Count % 6)), 0).ToString());
                                ULDrows = LastRowTotalCnt != 0 ? (ULDrows + 1) : ULDrows;
                                Array.Resize(ref objULDStockInfo, objULDStockInfo.Length + ULDrows);

                                int currentULDInfoRowCnt = 0;
                                int uldRowCount = 0;
                                for (int k = 0; k < objULDStockInfo.Length; k++)
                                {
                                    uldRowCount++;
                                    k = uldRowCount == 1 ? (objULDStockInfo.Length - ULDrows) : k;
                                    objULDStockInfo[k] = new MessageData.ULDStockInfo();
                                    int count = 0;

                                    MessageData.ULDStockInfoSub[] objULDStockInfoSub = new MessageData.ULDStockInfoSub[0];

                                    currentULDInfoRowCnt++;
                                    for (int j = 0; j < dtULD.Rows.Count; j++)
                                    {
                                        if (count == 0)
                                        {
                                            if (currentULDInfoRowCnt <= ULDrows)
                                            {
                                                j = 6 * ((currentULDInfoRowCnt - 1));
                                            }
                                        }

                                        count++;


                                        if (currentULDInfoRowCnt == ULDrows)
                                        {
                                            if (LastRowTotalCnt > 0)
                                            {
                                                if (count == LastRowTotalCnt)
                                                {
                                                    objULDStockInfo[k] = new MessageData.ULDStockInfo();
                                                    objULDStockInfo[k].ULDTypeCode = dtULD.Rows[j]["ULDType"].ToString();
                                                    objULDStockInfo[k].NumberOfULD = LastRowTotalCnt.ToString();
                                                    objULDStockInfo[k].TotalIdentifier = "T";
                                                }
                                            }
                                            else
                                            {
                                                objULDStockInfo[k] = new MessageData.ULDStockInfo();
                                                objULDStockInfo[k].ULDTypeCode = dtULD.Rows[j]["ULDType"].ToString();
                                                objULDStockInfo[k].NumberOfULD = "6";
                                                objULDStockInfo[k].TotalIdentifier = "T";
                                            }

                                            Array.Resize(ref objULDStockInfoSub, objULDStockInfoSub.Length + 1);
                                            objULDStockInfoSub[objULDStockInfoSub.Length - 1] = new MessageData.ULDStockInfoSub();
                                            objULDStockInfoSub[objULDStockInfoSub.Length - 1].ULDSerialNo = dtULD.Rows[j]["ULDSerialNo"].ToString();
                                            objULDStockInfoSub[objULDStockInfoSub.Length - 1].ULDOwnerCode = dtULD.Rows[j]["ULDOwnerCode"].ToString();
                                        }
                                        else
                                        {
                                            if (count < 7)
                                            {

                                                if (count == 6)
                                                {
                                                    objULDStockInfo[k] = new MessageData.ULDStockInfo();
                                                    objULDStockInfo[k].ULDTypeCode = dtULD.Rows[j]["ULDType"].ToString();
                                                    objULDStockInfo[k].NumberOfULD = count.ToString();
                                                    objULDStockInfo[k].TotalIdentifier = "T";

                                                }
                                                Array.Resize(ref objULDStockInfoSub, objULDStockInfoSub.Length + 1);
                                                objULDStockInfoSub[objULDStockInfoSub.Length - 1] = new MessageData.ULDStockInfoSub();
                                                objULDStockInfoSub[objULDStockInfoSub.Length - 1].ULDSerialNo = dtULD.Rows[j]["ULDSerialNo"].ToString();
                                                objULDStockInfoSub[objULDStockInfoSub.Length - 1].ULDOwnerCode = dtULD.Rows[j]["ULDOwnerCode"].ToString();

                                            }
                                            else
                                                break;

                                        }

                                    }
                                    objULDStockInfo[k].ULDInfoSub = objULDStockInfoSub;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            clsLog.WriteLog("Error FSU Prepare:" + ex.Message);
                        }


                        #endregion
                        objSCM.ULDInfo = objULDStockInfo;
                        string msgFSA = cls_Encode_Decode.EncodeSCMforsend(objSCM);
                        //clsLog.WriteLog(msgFSA);
                        if (addMsgToOutBox("SCM", msgFSA, FromEmailID, ToEmailID))
                        {
                            flag = true;
                        }
                        else
                        {
                            clsLog.WriteLog("Error: SCM not addded for send");
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }
    #endregion

    public static int DumpInterfaceInformation(string subject, string Msg, DateTime TimeStamp, string MessageType, string ErrorDesc, bool IsBlog,
        string FromEmailId, string ToEmailId, MemoryStream Attachments, string AttachmentExtension)
    {
        int SerialNo = 0;

        try
        {
            string procedure = "spDumpInterfaceCommunication";
            SQLServer dtb = new SQLServer(Global.GetConnectionString());
            DataSet objDS = null;
            byte[] objBytes = null;

            if (Attachments != null)
                objBytes = Attachments.ToArray();

            string[] paramname = new string[] { "Subject",
                                                "Body",
                                                "TimeStamp",
                                                "MessageType",
                                                "ErrorDesc",
                                                "IsBlog",
            "FromId", "ToId","Attachment","Extension"};

            object[] paramvalue = new object[] {subject,
                                                Msg,
                                                TimeStamp,
                                                MessageType,
                                                ErrorDesc,
                                                IsBlog, FromEmailId,ToEmailId, objBytes,AttachmentExtension};

            SqlDbType[] paramtype = new SqlDbType[] {SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.DateTime,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.VarChar,
                                                     SqlDbType.Bit,SqlDbType.VarChar,SqlDbType.VarChar,SqlDbType.VarBinary,SqlDbType.VarChar};

            objDS = dtb.SelectRecords(procedure, paramname, paramvalue, paramtype);

            if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                SerialNo = Convert.ToInt32(objDS.Tables[0].Rows[0][0]);
        }
        catch (Exception ex)
        {
            SerialNo = 0;
        }

        return SerialNo;
    }

}
