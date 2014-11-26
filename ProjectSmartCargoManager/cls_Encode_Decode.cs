using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System.IO;
using System.Reflection;
using System.Data;
using BAL;
using QID.DataAccess;
using System.Security.Cryptography;

/// <summary>
/// Summary description for cls_Decode
/// </summary>
public class cls_Encode_Decode
{

    #region Variables
    
    static string unloadingportsequence = "";
    static string uldsequencenum = "";
    static string awbref = "";
    CustomsImportBAL ObjCustom = new CustomsImportBAL();
    SQLServer db = new SQLServer(Global.GetConnectionString());
    #endregion

    #region Constructor
    public cls_Encode_Decode()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    #endregion

    //FFR
    
    #region Decode FFR
    public static bool decodereceivedffr(string ffrmsg, ref MessageData.ffrinfo data, ref MessageData.ULDinfo[] uld, ref MessageData.consignmnetinfo[] consinfo,ref MessageData.FltRoute[] fltroute,ref MessageData.dimensionnfo[] objDimension)
    {
        bool flag = false;
        string lastrec = "NA";
        int line = 0;
        try
        {
            if (ffrmsg.StartsWith("FFR", StringComparison.OrdinalIgnoreCase))
            {
                ffrmsg = ffrmsg.Replace("\r\n","$");
                //string[] str = Regex.Split(ffrmsg, "\r\n");//ffrmsg.Split('$');
                //string[] str = Regex.Split(ffrmsg, "\n");//ffrmsg.Split('$');
                //string[] str = ffrmsg.Split('\n');
                string[] str = ffrmsg.Split('$');
                if (str.Length >= 3)
                {
                    for (int i = 0; i < str.Length; i++)
                    {
                        flag = true;

                        #region Line 1
                        if (str[i].StartsWith("FFR", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                data.ffrversionnum = msg[1];
                            }
                            catch (Exception ex) { }
                        }
                        #endregion

                        #region Line Consigment Details 
                        if (i>0)
                        {
                            string[] msg = str[i].Split('/');                         
                            if (msg[0].Contains('-'))
                            {//Decode consigment info
                                decodeconsigmentdetails(str[i], ref consinfo);
                            }
                        }
                        #endregion

                        #region Line 3 flight details
                        if (i > 1 && !str[i].StartsWith("/") && !str[i].Contains('-'))
                        {
                            try
                            {
                                MessageData.FltRoute flight = new MessageData.FltRoute("");
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 0 && msg[0].Length>3)
                                {
                                    flight.carriercode = msg[0].Substring(0, 2);
                                    flight.fltnum = msg[0].Substring(2);
                                    flight.date = msg[1].Substring(0, 2);
                                    flight.month = msg[1].Substring(2);
                                    flight.fltdept = msg[2].Substring(0, 3);
                                    flight.fltarrival = msg[2].Substring(3);                                    
                                    try
                                    {
                                        flight.spaceallotmentcode = msg[3].Length > 0 ? msg[3] : "";
                                        if (msg[4].Length > 0) 
                                        {
                                            flight.allotidentification = msg[4].ToString();
                                        }
                                        
                                    }
                                    catch (Exception ex) { }
                                    Array.Resize(ref fltroute, fltroute.Length + 1);
                                    fltroute[fltroute.Length - 1] = flight;
                                }

                            }
                            catch (Exception e3)
                            {
                                continue;
                            }
                        }
                        #endregion

                        #region Line 4 ULD Specification
                        if (str[i].StartsWith("ULD", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                int uldnum = 0;
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 1)
                                {
                                    data.noofuld = msg[1];
                                    if (int.Parse(msg[1]) > 0)
                                    {
                                        for (int k = 2; k < msg.Length; k += 2)
                                        {
                                            string[] splitstr = msg[k].Split('-');
                                            uld[uldnum].uldtype = splitstr[0].Substring(0, 3);
                                            uld[uldnum].uldsrno = splitstr[0].Substring(3, splitstr[0].Length - 6);
                                            uld[uldnum].uldowner = splitstr[0].Substring(splitstr[0].Length - 3, 3);
                                            uld[uldnum].uldloadingindicator = splitstr[1];
                                            uld[uldnum].uldweightcode = msg[k + 1].Substring(0, 1);
                                            uld[uldnum].uldweight = msg[k + 1].Substring(1);
                                        }
                                    }
                                }
                            }
                            catch (Exception e4)
                            { }
                        }
                        #endregion

                        #region Line 5 Special Service request
                        if (str[i].StartsWith("SSR", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                lastrec = msg[0];
                                line = 0;
                                if (msg[1].Length > 0)
                                {
                                    data.specialservicereq1 = msg[1];
                                }

                            }
                            catch (Exception e5)
                            { }
                        }
                        #endregion

                        #region Line 6 Other service info
                        if (str[i].StartsWith("OSI", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                lastrec = msg[0];
                                line = 0;
                                if (msg[1].Length > 0)
                                {
                                    data.otherserviceinfo1 = msg[1];
                                }
                            }
                            catch (Exception e6) { }
                        }
                        #endregion

                        #region Line 7 booking reference
                        if (str[i].StartsWith("REF", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 1)
                                {
                                    data.bookingrefairport = msg[1].Substring(0, 3);
                                    data.officefundesignation = msg[1].Substring(3, 2);
                                    data.companydesignator = msg[1].Substring(5, 2);
                                    data.participentidetifier = msg[2].Length > 0 ? msg[2] : null;
                                    data.participentcode = msg[3].Length > 0 ? msg[3] : null;
                                    data.participentairportcity = msg[4].Length > 0 ? msg[4] : null;
                                }
                            }
                            catch (Exception e7)
                            { }
                        }
                        #endregion

                        #region Line 8 Dimendion info
                        if (str[i].StartsWith("DIM", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                lastrec = "DIM";
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 1)
                                {
                                    MessageData.dimensionnfo dimension = new MessageData.dimensionnfo("");
                                    if (msg[1].Substring(0, 1).Equals("K", StringComparison.OrdinalIgnoreCase) || msg[1].Substring(0, 1).Equals("L", StringComparison.OrdinalIgnoreCase))
                                    {
                                        dimension.weightcode = msg[1].Substring(0, 1);
                                        dimension.weight = msg[1].Substring(1);
                                    }
                                    if (msg.Length > 0)
                                    {
                                        string select = "";
                                        for (int n = 0; n < msg.Length; n++) 
                                        {
                                            if (msg[n].Contains('-')) 
                                            {
                                                select = msg[n];
                                            }
                                        }
                                        string[] dimstr = select.Split('-');
                                        dimension.mesurunitcode = dimstr[0].Substring(0, 3);
                                        dimension.length = dimstr[0].Substring(3);
                                        dimension.width= dimstr[1];
                                        dimension.height = dimstr[2];
                                    }
                                    try
                                    {
                                        int val;
                                        if (int.TryParse(msg[msg.Length - 1], out val))
                                        { 
                                            dimension.piecenum = msg[msg.Length - 1]; 
                                        }
                                    }
                                    catch (Exception ex) { }
                                    Array.Resize(ref objDimension, objDimension.Length + 1);
                                    objDimension[objDimension.Length - 1] = dimension;

                                }
                            }
                            catch (Exception e8) { }
                        }
                        #endregion

                        #region Line 9 Product information
                        if (str[i].StartsWith("PID", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 1)
                                {
                                    data.servicecode = msg[1];
                                    data.rateclasscode = msg[2];
                                    data.commoditycode = msg[3];
                                }
                            }
                            catch (Exception e9)
                            {
                            }
                        }
                        #endregion

                        #region Line 10 Shipper Infor
                        if (str[i].StartsWith("SHP", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                lastrec = msg[0];
                                line = 0;
                                if (msg.Length > 1)
                                {
                                    data.shipperaccnum = msg[1];

                                }
                            }
                            catch (Exception e10)
                            { }
                        }
                        #endregion

                        #region Line 11 Consignee
                        if (str[i].StartsWith("CNE", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                lastrec = msg[0];
                                line = 0;
                                if (msg.Length > 1)
                                {
                                    data.consaccnum = msg[1];
                                }
                            }
                            catch (Exception e10)
                            { }
                        }
                        #endregion

                        #region Line 12 Customer Identification
                        if (str[i].StartsWith("CUS", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                lastrec = msg[0];
                                if (msg.Length > 1)
                                {
                                    data.custaccnum = msg[1].Length > 0 ? msg[1] : "";
                                    data.iatacargoagentcode = msg[2].Length > 0 ? msg[2] : "";
                                    data.cargoagentcasscode = msg[3].Length > 0 ? msg[3] : "";
                                    data.participentidetifier = msg[4].Length > 0 ? msg[4] : "";

                                }
                            }
                            catch (Exception e12) { }
                        }
                        #endregion

                        #region Line 13 shipment refence info
                        if (str[i].StartsWith("SRI", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 1)
                                {
                                    data.shiprefnum = msg[1].Length > 0 ? msg[1] : null;
                                    data.supplemetryshipperinfo1 = msg[2].Length > 0 ? msg[2] : null;
                                    data.supplemetryshipperinfo2 = msg[3].Length > 0 ? msg[3] : null;
                                }
                            }
                            catch (Exception e13) { }
                        }
                        #endregion

                        #region Other Info
                        if (str[i].StartsWith("/"))
                        {
                            string[] msg = str[i].Split('/');
                            try
                            {
                                if (lastrec == "SSR")
                                {
                                    data.specialservicereq2 = msg[1].Length > 0 ? msg[1] : "";
                                }
                                if (lastrec == "OSI")
                                {
                                    data.otherserviceinfo2 = msg[1].Length > 0 ? msg[1] : "";
                                }
                                #region SHP Data
                                if (lastrec == "SHP")
                                {
                                    line++;
                                    if (line == 1)
                                    {
                                        data.shippername = msg[1].Length > 0 ? msg[1] : "";
                                    }
                                    if (line == 2)
                                    {
                                        data.shipperadd = msg[1].Length > 0 ? msg[1] : "";

                                    }
                                    if (line == 3)
                                    {
                                        data.shipperplace = msg[1].Length > 0 ? msg[1] : "";
                                        data.shipperstate = msg[2].Length > 0 ? msg[2] : "";
                                    }
                                    if (line == 4)
                                    {
                                        data.shippercountrycode = msg[1].Length > 0 ? msg[1] : "";
                                        data.shipperpostcode = msg[2].Length > 0 ? msg[2] : "";
                                        data.shippercontactidentifier = msg[3].Length > 0 ? msg[3] : "";
                                        data.shippercontactnum = msg[4].Length > 0 ? msg[4] : "";

                                    }

                                }
                                #endregion

                                #region CNE Data
                                if (lastrec == "CNE")
                                {
                                    line++;
                                    if (line == 1)
                                    {
                                        data.consname = msg[1].Length > 0 ? msg[1] : "";
                                    }
                                    if (line == 2)
                                    {
                                        data.consadd = msg[1].Length > 0 ? msg[1] : "";
                                    }
                                    if (line == 3)
                                    {
                                        data.consplace = msg[1].Length > 0 ? msg[1] : "";
                                        data.consstate = msg[2].Length > 0 ? msg[2] : "";
                                    }
                                    if (line == 4)
                                    {
                                        data.conscountrycode = msg[1].Length > 0 ? msg[1] : "";
                                        data.conspostcode = msg[2].Length > 0 ? msg[2] : "";
                                        data.conscontactidentifier = msg[3].Length > 0 ? msg[3] : "";
                                        data.conscontactnum = msg[4].Length > 0 ? msg[4] : "";
                                    }

                                }
                                #endregion

                                #region CUS data
                                if (lastrec == "CUS")
                                {
                                    line++;
                                    if (line == 1)
                                    {
                                        data.custname = msg[1].Length > 0 ? msg[1] : "";

                                    }
                                    if (line == 2)
                                    {
                                        data.custplace = msg[1].Length > 0 ? msg[1] : "";
                                    }
                                }
                                #endregion

                                #region DIM info
                                if (lastrec.Equals("DIM", StringComparison.OrdinalIgnoreCase))
                                {
                                    try
                                    {                                       
                                        if (msg.Length > 1)
                                        {
                                            MessageData.dimensionnfo dimension = new MessageData.dimensionnfo("");
                                            dimension.weightcode = msg[1].Substring(0, 1);
                                            dimension.weight = msg[1].Substring(1);
                                            if (msg.Length > 0)
                                            {
                                                string[] dimstr = msg[2].Split('-');
                                                dimension.mesurunitcode = dimstr[0].Substring(0, 3);
                                                dimension.length = dimstr[0].Substring(3);
                                                dimension.width = dimstr[1];
                                                dimension.height = dimstr[2];
                                            }
                                            try
                                            {
                                                dimension.piecenum = msg[3];
                                            }
                                            catch (Exception ex) { }
                                            Array.Resize(ref objDimension, objDimension.Length + 1);
                                            objDimension[objDimension.Length - 1] = dimension;

                                        }
                                    }
                                    catch (Exception e8) { }
                                }
                                #endregion
                            }
                            catch (Exception e13)
                            { }
                        }
                        #endregion
                    }
                }

            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show("Exception:" + ex.Message);
        }
        return flag;
    }
    #endregion

    #region Encode FFR
    public static string encodeFFRforsend(ref MessageData.ffrinfo data, ref MessageData.ULDinfo[] uld)
    {
        string ffr = null;
        try
        {
            #region Line 1
            string line1 = "FFR" + "/" + data.ffrversionnum;
            #endregion

            #region Line 2 AWB Data
            string splhandling = "";
            //if (data.splhandling.Length > 0 && data.splhandling != null)
            //{
            //    splhandling = data.splhandling.Replace(',', '/');
            //    splhandling = "/" + splhandling;
            //}
            string line2 = string.Empty;// data.airlineprefix + "-" + data.awbnum + data.origin + data.dest + "/" + data.consigntype + data.pcscnt + data.weightcode + data.weight + data.volumecode + data.volumeamt + data.densityindicator + data.densitygrp + data.shpdesccode + data.numshp + "/" + data.manifestdesc + splhandling;
            #endregion

            #region Line 3 Flight Schedule
            string line3 = "";//data.carriercode + data.fltnum + "/" + data.date + data.month + "/" + data.fltdept + data.fltarrival + "/" + data.spaceallotmentcode + "/" + data.allotidentification;
            #endregion

            #region Line 4
            string line4 = "";
            if (data.noofuld.Length > 0)
            {
                line4 = "ULD/" + data.noofuld + "/";
                string uldinfo = null;
                for (int i = 0; i < int.Parse(data.noofuld); i++)
                {
                    uldinfo = null;
                    uldinfo = uld[i].uldtype + uld[i].uldsrno + uld[i].uldowner + "-" + uld[i].uldloadingindicator + "/" + uld[i].uldweightcode + uld[i].uldweight;
                    if (uldinfo.Length > 2)
                        line4 = line4 + uldinfo + "/";
                }
            }
            #endregion

            #region Line 5
            string line5 = "";
            if (data.specialservicereq1.Length > 0 || data.specialservicereq2.Length > 0)
            {
                line5 = "SSR/" + data.specialservicereq1 + "\r\n" + "/" + data.specialservicereq2;
            }
            #endregion

            #region Line 6
            string line6 = "";
            if (data.otherserviceinfo1.Length > 0 || data.otherserviceinfo2.Length > 0)
            {
                line6 = "SSR/" + data.otherserviceinfo1 + "\r\n" + "/" + data.otherserviceinfo2;
            }
            #endregion

            #region Line 7
            string line7 = "";
            line7 = "REF/" + data.bookingrefairport + data.officefundesignation + data.companydesignator;
            if (data.bookingfileref.Length > 0)
            {
                line7 = line7 + "/" + data.bookingfileref;
            }
            if (data.participentidetifier.Length > 0)
            {
                line7 = line7 + "/" + data.participentidetifier;
            }
            if (data.participentcode.Length > 0)
            {
                line7 = line7 + "/" + data.participentcode;
            }
            if (data.participentairportcity.Length > 0)
            {
                line7 = line7 + "/" + data.participentairportcity;
            }
            #endregion

            #region Line 8 Dimension Info
            string line8 = "";

            //if (data.line8height.Length > 0 || data.line8length.Length > 0 || data.line8piecenum.Length > 0 || data.line8weight.Length > 0 || data.line8width.Length > 0)
            //{
            //    line8 = "DIM/" + data.line8weightcode + data.weight + "/" + data.line8mesurunitcode + data.line8length + "-" + data.line8width + "-" + data.line8height + "/" + data.line8piecenum;
            //}
            #endregion

            #region Line 9
            string line9 = "";
            if (data.servicecode.Length > 0 || data.rateclasscode.Length > 0 || data.commoditycode.Length > 0)
            {
                line9 = "PID/" + data.servicecode + "/" + data.rateclasscode + "/" + data.commoditycode;
            }
            #endregion

            #region Line 10
            string line10 = "";
            string str1 = "", str2 = "", str3 = "", str4 = "";
            try
            {
                if (data.shippername.Length > 0)
                {
                    str1 = "/" + data.shippername;
                }
                if (data.shipperadd.Length > 0)
                {
                    str2 = "/" + data.shipperadd;
                }

                if (data.shipperplace.Length > 0 || data.shipperstate.Length > 0)
                {
                    str3 = "/" + data.shipperplace + "/" + data.shipperstate;
                }
                if (data.shippercountrycode.Length > 0 || data.shipperpostcode.Length > 0 || data.shippercontactidentifier.Length > 0 || data.shippercontactnum.Length > 0)
                {
                    str4 = "/" + data.shippercountrycode + "/" + data.shipperpostcode + "/" + data.shippercontactidentifier + "/" + data.shippercontactnum;
                }

                if (data.shipperaccnum.Length > 0 || str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line10 = "SHP/"+data.shipperaccnum;
                    if (str4.Length > 0)
                    {
                        line10 = line10.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/') + "\r\n/" + str4.Trim('/');
                    }
                    else if (str3.Length > 0)
                    {
                        line10 = line10.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line10 = line10.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line10 = line10.Trim() + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            #region Line 11
            string line11 = "";
            str1 = "";
            str2 = "";
            str3 = "";
            str4 = "";
            try
            {
                if (data.consname.Length > 0)
                {
                    str1 = "/" + data.consname;
                }
                if (data.consadd.Length > 0)
                {
                    str2 = "/" + data.consadd;
                }

                if (data.consplace.Length > 0 || data.consstate.Length > 0)
                {
                    str3 = "/" + data.custplace + "/" + data.consstate;
                }
                if (data.conscountrycode.Length > 0 || data.conspostcode.Length > 0 || data.conscontactidentifier.Length > 0 || data.conscontactnum.Length > 0)
                {
                    str4 = "/" + data.conscountrycode + "/" + data.conspostcode + "/" + data.conscontactidentifier + "/" + data.conscontactnum;
                }

                if (data.consaccnum.Length > 0 || str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line11 = "CNE/" + data.consaccnum;
                    if (str4.Length > 0)
                    {
                        line11 = line11.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/') + "\r\n/" + str4.Trim('/');
                    }
                    else if (str3.Length > 0)
                    {
                        line11 = line11.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line11 = line11.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line11 = line11.Trim() + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            #region Line 12
            string line12 = "";
            str1 = "";
            str2 = "";
            try
            {
                if (data.custname.Length > 0)
                {
                    str1 = "/" + data.custname;
                }
                if (data.custplace.Length > 0)
                {
                    str2 = "/" + data.custplace;
                }
                if (data.custaccnum.Length > 0 || data.iatacargoagentcode.Length > 0 || data.cargoagentcasscode.Length > 0 || data.participentidetifier.Length > 0 || str1.Length > 0 || str2.Length > 0)
                {
                    line12 = "CUS/" + data.shipperaccnum + "/" + data.iatacargoagentcode + "/" + data.cargoagentcasscode + "/" + data.participentidetifier;
                    if (str2.Length > 0)
                    {
                        line12 = line12.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line12 = line12.Trim() + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            #region Line 13
            string line13 = "";
            if (data.shiprefnum.Length > 0 || data.supplemetryshipperinfo1.Length > 0 || data.supplemetryshipperinfo2.Length > 0)
            {
                line13 = "SRI/" + data.shiprefnum + "/" + data.supplemetryshipperinfo1 + "/" + data.supplemetryshipperinfo2;
            }
            #endregion

            #region BuildFFR
            ffr = line1.Trim('/') + "\r\n" + line2.Trim('/') + "\r\n" + line3.Trim('/');
            if (line4.Length > 0)
            {
                ffr = ffr + "\r\n" + line4.Trim('/');
            }
            if (line5.Length > 0)
            {
                ffr = ffr + "\r\n" + line5.Trim('/');
            }
            if (line6.Length > 0)
            {
                ffr = ffr + "\r\n" + line6.Trim('/');
            }
            if (line7.Length > 0)
            {
                ffr = ffr + "\r\n" + line7.Trim('/');
            }
            if (line8.Length > 0)
            {
                ffr = ffr + "\r\n" + line8.Trim('/');
            }
            if (line9.Length > 0)
            {
                ffr = ffr + "\r\n" + line9.Trim('/');
            }
            if (line10.Length > 0)
            {
                ffr = ffr + "\r\n" + line10.Trim('/');
            }
            if (line11.Length > 0)
            {
                ffr = ffr + "\r\n" + line11.Trim('/');
            }
            if (line12.Length > 0)
            {
                ffr = ffr + "\r\n" + line12.Trim('/');
            }
            if (line13.Length > 0)
            {
                ffr = ffr + "\r\n" + line13.Trim('/');
            }
            #endregion

        }
        catch (Exception ex)
        {
            ffr = "ERR";
        }
        return ffr;
    }
   
    public static string encodeFFRforsend(ref MessageData.ffrinfo data, ref MessageData.ULDinfo[] uld, ref MessageData.consignmnetinfo consigment, ref MessageData.FltRoute[] FltRoute, ref MessageData.dimensionnfo[] dimension)
    {
        string ffr = null;
        try
        {
            #region Line 1
            string line1 = "FFR" + "/" + data.ffrversionnum;
            #endregion

            #region Line 2 AWB Data
            string splhandling = "";

            //if (data.splhandling.Length > 0 && data.splhandling != null)
            //{
            //    splhandling = data.splhandling.Replace(',', '/');
            //    splhandling = "/" + splhandling;
            //}
            string line2 = consigment.airlineprefix + "-" + consigment.awbnum + consigment.origin + consigment.dest + "/" + consigment.consigntype + consigment.pcscnt + consigment.weightcode + consigment.weight + consigment.volumecode + consigment.volumeamt + consigment.densityindicator + consigment.densitygrp + consigment.shpdesccode + consigment.numshp + "/" + consigment.manifestdesc + consigment.splhandling;
            #endregion

            #region Line 3 Flight Schedule
            string line3 = "";
            if (FltRoute.Length > 0)
            {
                for (int i = 0; i < FltRoute.Length; i++)
                {
                    line3 = line3 + FltRoute[i].carriercode + FltRoute[i].fltnum + "/" + FltRoute[i].date + FltRoute[i].month + "/" + FltRoute[i].fltdept + FltRoute[i].fltarrival + "/" + FltRoute[i].spaceallotmentcode +(FltRoute[i].allotidentification.Length>0?( "/" + FltRoute[i].allotidentification):"") + "$";
                }
            }
            line3 = line3.Trim('$');
            line3 = line3.Replace("$", "\r\n");
            //
            #endregion

            #region Line 4
            string line4 = "";
            if (data.noofuld.Length > 0)
            {
                line4 = "ULD/" + data.noofuld + "/";
                string uldinfo = null;
                for (int i = 0; i < int.Parse(data.noofuld); i++)
                {
                    uldinfo = null;
                    uldinfo = uld[i].uldtype + uld[i].uldsrno + uld[i].uldowner + "-" + uld[i].uldloadingindicator + "/" + uld[i].uldweightcode + uld[i].uldweight;
                    if (uldinfo.Length > 2)
                        line4 = line4 + uldinfo + "/";
                }
            }
            #endregion

            #region Line 5
            string line5 = "";
            if (data.specialservicereq1.Length > 0 || data.specialservicereq2.Length > 0)
            {
                line5 = "SSR/" + data.specialservicereq1 + "\r\n" + "/" + data.specialservicereq2;
            }
            #endregion

            #region Line 6
            string line6 = "";
            if (data.otherserviceinfo1.Length > 0 || data.otherserviceinfo2.Length > 0)
            {
                line6 = "SSR/" + data.otherserviceinfo1 + "\r\n" + "/" + data.otherserviceinfo2;
            }
            #endregion

            #region Line 7
            string line7 = "";
            line7 = "REF/" + data.bookingrefairport + data.officefundesignation + data.companydesignator;
            if (data.bookingfileref.Length > 0)
            {
                line7 = line7 + "/" + data.bookingfileref;
            }
            if (data.participentidetifier.Length > 0)
            {
                line7 = line7 + "/" + data.participentidetifier;
            }
            if (data.participentcode.Length > 0)
            {
                line7 = line7 + "/" + data.participentcode;
            }
            if (data.participentairportcity.Length > 0)
            {
                line7 = line7 + "/" + data.participentairportcity;
            }
            #endregion

            #region Line 8 Dimension Info
            string line8 = "";
            if (dimension.Length > 0)
            {
                for (int i = 0; i < dimension.Length; i++)
                {
                    if (dimension[i].weight.Length > 0 || dimension[i].length.Length > 0 || dimension[i].width.Length > 0 || dimension[i].height.Length > 0 || dimension[i].piecenum.Length > 0)
                    {
                        line8 = line8 + dimension[i].weightcode + dimension[i].weight + "/" + dimension[i].mesurunitcode + dimension[i].length + "-" + dimension[i].width + "-" + dimension[i].height + "/" + dimension[i].piecenum + "$";
                    }
                }
            }
            line8 = line8.Trim('$');
            if (line8.Length > 0)
            {
                line8 = "DIM/" + line8.Replace("$", "\r\n");
            }
            #endregion

            #region Line 9
            string line9 = "";
            if (data.servicecode.Length > 0 || data.rateclasscode.Length > 0 || data.commoditycode.Length > 0)
            {
                line9 = "PID/" + data.servicecode + "/" + data.rateclasscode + "/" + data.commoditycode;
            }
            #endregion

            #region Line 10
            string line10 = "";
            string str1 = "", str2 = "", str3 = "", str4 = "";
            try
            {
                if (data.shippername.Length > 0)
                {
                    str1 = "/" + data.shippername;
                }
                if (data.shipperadd.Length > 0)
                {
                    str2 = "/" + data.shipperadd;
                }

                if (data.shipperplace.Length > 0 || data.shipperstate.Length > 0)
                {
                    str3 = "/" + data.shipperplace + "/" + data.shipperstate;
                }
                if (data.shippercountrycode.Length > 0 || data.shipperpostcode.Length > 0 || data.shippercontactidentifier.Length > 0 || data.shippercontactnum.Length > 0)
                {
                    str4 = "/" + data.shippercountrycode + "/" + data.shipperpostcode + "/" + data.shippercontactidentifier + "/" + data.shippercontactnum;
                }

                if (data.shipperaccnum.Length > 0 || str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line10 = "SHP/" + data.shipperaccnum;
                    if (str4.Length > 0)
                    {
                        line10 = line10.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/') + "\r\n/" + str4.Trim('/');
                    }
                    else if (str3.Length > 0)
                    {
                        line10 = line10.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line10 = line10.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line10 = line10.Trim() + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            #region Line 11
            string line11 = "";
            str1 = "";
            str2 = "";
            str3 = "";
            str4 = "";
            try
            {
                if (data.consname.Length > 0)
                {
                    str1 = "/" + data.consname;
                }
                if (data.consadd.Length > 0)
                {
                    str2 = "/" + data.consadd;
                }

                if (data.consplace.Length > 0 || data.consstate.Length > 0)
                {
                    str3 = "/" + data.custplace + "/" + data.consstate;
                }
                if (data.conscountrycode.Length > 0 || data.conspostcode.Length > 0 || data.conscontactidentifier.Length > 0 || data.conscontactnum.Length > 0)
                {
                    str4 = "/" + data.conscountrycode + "/" + data.conspostcode + "/" + data.conscontactidentifier + "/" + data.conscontactnum;
                }

                if (data.consaccnum.Length > 0 || str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line11 = "CNE/" + data.consaccnum;
                    if (str4.Length > 0)
                    {
                        line11 = line11.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/') + "\r\n/" + str4.Trim('/');
                    }
                    else if (str3.Length > 0)
                    {
                        line11 = line11.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line11 = line11.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line11 = line11.Trim() + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            #region Line 12
            string line12 = "";
            str1 = "";
            str2 = "";
            try
            {
                if (data.custname.Length > 0)
                {
                    str1 = "/" + data.custname;
                }
                if (data.custplace.Length > 0)
                {
                    str2 = "/" + data.custplace;
                }
                if (data.custaccnum.Length > 0 || data.iatacargoagentcode.Length > 0 || data.cargoagentcasscode.Length > 0 || data.participentidetifier.Length > 0 || str1.Length > 0 || str2.Length > 0)
                {
                    line12 = "CUS/" + data.shipperaccnum + "/" + data.iatacargoagentcode + "/" + data.cargoagentcasscode + "/" + data.participentidetifier;
                    if (str2.Length > 0)
                    {
                        line12 = line12.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line12 = line12.Trim() + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            #region Line 13
            string line13 = "";
            if (data.shiprefnum.Length > 0 || data.supplemetryshipperinfo1.Length > 0 || data.supplemetryshipperinfo2.Length > 0)
            {
                line13 = "SRI/" + data.shiprefnum + "/" + data.supplemetryshipperinfo1 + "/" + data.supplemetryshipperinfo2;
            }
            #endregion

            #region BuildFFR
            ffr = line1.Trim('/') + "\r\n" + line2.Trim('/') + "\r\n" + line3.Trim('/');
            if (line4.Length > 0)
            {
                ffr = ffr + "\r\n" + line4.Trim('/');
            }
            if (line5.Length > 0)
            {
                ffr = ffr + "\r\n" + line5.Trim('/');
            }
            if (line6.Length > 0)
            {
                ffr = ffr + "\r\n" + line6.Trim('/');
            }
            if (line7.Length > 0)
            {
                ffr = ffr + "\r\n" + line7.Trim('/');
            }
            if (line8.Length > 0)
            {
                ffr = ffr + "\r\n" + line8.Trim('/');
            }
            if (line9.Length > 0)
            {
                ffr = ffr + "\r\n" + line9.Trim('/');
            }
            if (line10.Length > 0)
            {
                ffr = ffr + "\r\n" + line10.Trim('/');
            }
            if (line11.Length > 0)
            {
                ffr = ffr + "\r\n" + line11.Trim('/');
            }
            if (line12.Length > 0)
            {
                ffr = ffr + "\r\n" + line12.Trim('/');
            }
            if (line13.Length > 0)
            {
                ffr = ffr + "\r\n" + line13.Trim('/');
            }
            #endregion

        }
        catch (Exception ex)
        {
            ffr = "ERR";
        }
        return ffr;
    }
    #endregion

    //FFA

    #region Decode FFA
    public static bool decodereceivedffa(string ffamsg, ref MessageData.ffainfo ffadata)
    {
        bool flag = false;
        string lastrec = "NA";
        int line = 0;
        try
        {
            if (ffamsg.StartsWith("FFA", StringComparison.OrdinalIgnoreCase))
            {
                // ffrmsg = ffrmsg.Replace("\r\n","$");
                //string[] str = Regex.Split(ffamsg, "\r\n");//ffrmsg.Split('$');
                string[] str = ffamsg.Split('$');
                if (str.Length > 3)
                {
                    for (int i = 0; i < str.Length; i++)
                    {
                        flag = true;

                        #region Line 1
                        if (str[i].StartsWith("FFA", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                ffadata.ffaversionnum = msg[1];
                            }
                            catch (Exception ex) { }
                        }
                        #endregion

                        #region Line 2
                        if (i == 1)
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                //0th element
                                string[] decmes = msg[0].Split('-');
                                ffadata.airlineprefix = decmes[0];
                                ffadata.awbnum = decmes[1].Substring(0, decmes[1].Length - 6);
                                ffadata.origin = decmes[1].Substring(decmes[1].Length - 6, 3);
                                ffadata.dest = decmes[1].Substring(decmes[1].Length - 3, 3);
                                //1
                                if (msg[1].Length > 0)
                                {
                                    int k = 0;
                                    char lastchr = 'A';
                                    char[] arr = msg[1].ToCharArray();
                                    string[] strarr = new string[arr.Length];
                                    for (int j = 0; j < arr.Length; j++)
                                    {
                                        if ((char.IsNumber(arr[j])) || (arr[j].Equals('.')))
                                        {//number                            
                                            if (lastchr == 'N')
                                                k--;
                                            strarr[k] = strarr[k] + arr[j].ToString();
                                            lastchr = 'N';
                                        }
                                        if (char.IsLetter(arr[j]))
                                        {//letter
                                            if (lastchr == 'L')
                                                k--;
                                            strarr[k] = strarr[k] + arr[j].ToString();
                                            lastchr = 'L';
                                        }
                                        k++;
                                    }
                                    ffadata.consigntype = strarr[0];
                                    ffadata.pcscnt = strarr[1];//int.Parse(strarr[1]);
                                    ffadata.weightcode = strarr[2];
                                    ffadata.weight = strarr[3];//float.Parse(strarr[3]);
                                    for (k = 4; k < strarr.Length; k += 2)
                                    {
                                        if (strarr[k] != null)
                                        {
                                            if (strarr[k] == "T")
                                            {
                                                ffadata.shpdesccode = strarr[k];
                                                ffadata.numshp = strarr[k + 1];
                                                k = strarr.Length + 1;
                                            }
                                        }
                                    }
                                }
                                if (msg.Length > 2)
                                {
                                    //2
                                    ffadata.manifestdesc = msg[2];

                                }
                                if (msg.Length > 2)
                                {//3
                                    ffadata.splhandling = "";
                                    for (int j = 3; j < msg.Length; j++)
                                        ffadata.splhandling = ffadata.splhandling + msg[j] + ",";
                                }

                            }
                            catch (Exception e)
                            {
                                continue;
                            }
                        }
                        #endregion

                        #region Line 3 flight details
                        if (i == 2)
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 0)
                                {
                                    ffadata.carriercode = msg[0].Substring(0, 2);
                                    ffadata.fltnum = msg[0].Substring(2);
                                    ffadata.date = msg[1].Substring(0, 2);
                                    ffadata.month = msg[1].Substring(2);
                                    ffadata.fltdept = msg[2].Substring(0, 3);
                                    ffadata.fltarrival = msg[2].Substring(3);
                                    ffadata.spaceallotmentcode = msg[3].Length > 0 ? msg[3] : null;
                                }

                            }
                            catch (Exception e3)
                            {
                                continue;
                            }
                        }
                        #endregion

                        #region Line 5 Special Service request
                        if (str[i].StartsWith("SSR", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                lastrec = msg[0];
                                line = 0;
                                if (msg[1].Length > 0)
                                {
                                    ffadata.specialservicereq1 = msg[1];
                                }

                            }
                            catch (Exception e5)
                            { }
                        }
                        #endregion

                        #region Line 6 Other service info
                        if (str[i].StartsWith("OSI", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                lastrec = msg[0];
                                line = 0;
                                if (msg[1].Length > 0)
                                {
                                    ffadata.otherserviceinfo1 = msg[1];
                                }
                            }
                            catch (Exception e6) { }
                        }
                        #endregion

                        #region Line 7 booking reference
                        if (str[i].StartsWith("REF", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 1)
                                {
                                    ffadata.bookingrefairport = msg[1].Substring(0, 3);
                                    ffadata.officefundesignation = msg[1].Substring(3, 2);
                                    ffadata.companydesignator = msg[1].Substring(5, 2);
                                    ffadata.participentidetifier = msg[2].Length > 0 ? msg[2] : null;
                                    ffadata.participentcode = msg[3].Length > 0 ? msg[3] : null;
                                    ffadata.participentairportcity = msg[4].Length > 0 ? msg[4] : null;
                                }
                            }
                            catch (Exception e7)
                            { }
                        }
                        #endregion

                        #region Line 8 shipment refence info
                        if (str[i].StartsWith("SRI", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 1)
                                {
                                    ffadata.shiprefnum = msg[1].Length > 0 ? msg[1] : null;
                                    ffadata.supplemetryshipperinfo1 = msg[2].Length > 0 ? msg[2] : null;
                                    ffadata.supplemetryshipperinfo2 = msg[3].Length > 0 ? msg[3] : null;
                                }
                            }
                            catch (Exception e13) { }
                        }
                        #endregion

                        #region Other Info
                        if (str[i].StartsWith("/"))
                        {
                            string[] msg = str[i].Split('/');
                            try
                            {
                                if (lastrec == "SSR")
                                {
                                    ffadata.specialservicereq2 = msg[1].Length > 0 ? msg[1] : "";
                                }
                                if (lastrec == "OSI")
                                {
                                    ffadata.otherserviceinfo2 = msg[1].Length > 0 ? msg[1] : "";
                                }

                            }
                            catch (Exception e13)
                            { }
                        }
                        #endregion
                    }
                }

            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show("Exception:" + ex.Message);
        }
        return flag;
    }
    #endregion

    #region Encode FFA
    public static string encodeFFAforsend( ref MessageData.ffainfo ffadata,ref MessageData.FltRoute[] FltRoute)
    {
        string ffa = null;
        try
        {
            #region Line 1
            string line1 = "FFA" + "/" + ffadata.ffaversionnum;
            #endregion

            #region Line 2
            string splhandling = "";
            if (ffadata.splhandling.Length > 0 && ffadata.splhandling != null)
            {
                splhandling = ffadata.splhandling.Replace(',', '/');
                splhandling = "/" + splhandling;
            }
            string line2 = ffadata.airlineprefix + "-" + ffadata.awbnum + ffadata.origin + ffadata.dest + "/" + ffadata.consigntype + ffadata.pcscnt + ffadata.weightcode + ffadata.weight + ffadata.shpdesccode + ffadata.numshp + "/" + ffadata.manifestdesc + splhandling;
            #endregion line 2

            #region Line 3
            string line3 = "";// ffadata.carriercode + ffadata.fltnum + "/" + ffadata.date + ffadata.month + "/" + ffadata.fltdept + ffadata.fltarrival + "/" + ffadata.spaceallotmentcode;
            for (int i = 0; i < FltRoute.Length; i++) 
            {
                line3 = line3+FltRoute[i].carriercode + FltRoute[i].fltnum + "/" + FltRoute[i].date + (new DateTime(2010, int.Parse(FltRoute[i].month), 1).ToString("MMM", CultureInfo.InvariantCulture)).ToUpper()+"/"+FltRoute[i].fltdept+FltRoute[i].fltarrival+"/"+FltRoute[i].spaceallotmentcode+"$";
            }
            line3 = line3.Trim('$');
            line3 = line3.Replace("$","\r\n");
            #endregion

            #region Line 4
            string line4 = "";
            if (ffadata.specialservicereq1.Length > 0 || ffadata.specialservicereq2.Length > 0)
            {
                line4 = "SSR/" + ffadata.specialservicereq1 + "\r\n" + "/" + ffadata.specialservicereq2;
            }
            #endregion

            #region Line 5
            string line5 = "";
            if (ffadata.otherserviceinfo1.Length > 0 || ffadata.otherserviceinfo2.Length > 0)
            {
                line5 = "SSR/" + ffadata.otherserviceinfo1 + "\r\n" + "/" + ffadata.otherserviceinfo2;
            }
            #endregion

            #region Line 6
            string line6 = "";
            line6 = "REF/" + ffadata.bookingrefairport + ffadata.officefundesignation + ffadata.companydesignator;
            if (ffadata.bookingfileref.Length > 0)
            {
                line6 = line6 + "/" + ffadata.bookingfileref;
            }
            if (ffadata.participentidetifier.Length > 0)
            {
                line6 = line6 + "/" + ffadata.participentidetifier;
            }
            if (ffadata.participentcode.Length > 0)
            {
                line6 = line6 + "/" + ffadata.participentcode;
            }
            if (ffadata.participentairportcity.Length > 0)
            {
                line6 = line6 + "/" + ffadata.participentairportcity;
            }
            #endregion

            #region Line 7
            string line7 = "";
            if (ffadata.shiprefnum.Length > 0 || ffadata.supplemetryshipperinfo1.Length > 0 || ffadata.supplemetryshipperinfo2.Length > 0)
            {
                line7 = "SRI/" + ffadata.shiprefnum + "/" + ffadata.supplemetryshipperinfo1 + "/" + ffadata.supplemetryshipperinfo2;
            }
            #endregion

            #region BuildFFA
            ffa = line1.Trim('/') + "\r\n" + line2.Trim('/') + "\r\n" + line3.Trim('/');
            if (line4.Length > 0)
            {
                ffa = ffa + "\r\n" + line4.Trim('/');
            }
            if (line5.Length > 0)
            {
                ffa = ffa + "\r\n" + line5.Trim('/');
            }
            if (line6.Length > 0)
            {
                ffa = ffa + "\r\n" + line6.Trim('/');
            }
            if (line7.Length > 0)
            {
                ffa = ffa + "\r\n" + line7.Trim('/');
            }

            #endregion

        }
        catch (Exception ex)
        {
            ffa = "ERR";
        }
        return ffa;
    }
    #endregion

    //FBL

    #region decode FBL
    private bool decodereceiveFBL(string fblmsg, ref MessageData.fblinfo fbldata, ref MessageData.unloadingport[] unloadingport, ref MessageData.dimensionnfo[] dimensioinfo, ref MessageData.ULDinfo[] uld, ref MessageData.otherserviceinfo othinfo, ref MessageData.consignmentorigininfo[] consorginfo, ref MessageData.consignmnetinfo[] consinfo)
    {
        bool flag = false;
        try
        {
            string lastrec = "NA";
            int line = 0;//, consignmnetnum = 0; 
            //int count=0;
            try
            {
                if (fblmsg.StartsWith("FBL", StringComparison.OrdinalIgnoreCase))
                {
                    // ffrmsg = ffrmsg.Replace("\r\n","$");
                    string[] str = Regex.Split(fblmsg, "$");//ffrmsg.Split('$');
                    if (str.Length > 3)
                    {
                        for (int i = 0; i < str.Length; i++)
                        {

                            flag = true;
                            #region Line 1
                            if (str[i].StartsWith("FBL", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    fbldata.fblversion = msg[1];
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region line 2 flight data
                            if (i == 1)
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 1)
                                    {
                                        fbldata.messagesequencenum = msg[0];
                                        fbldata.carriercode = msg[1].Substring(0, 2);
                                        fbldata.fltnum = msg[1].Substring(2);
                                        fbldata.date = msg[2].Substring(0, 2);
                                        fbldata.month = msg[2].Substring(2);
                                        fbldata.fltairportcode = msg[3];
                                        fbldata.aircraftregistration = msg[4];
                                    }
                                }
                                catch (Exception ex)
                                { }
                            }
                            #endregion

                            #region line 3 point of unloading
                            if (i >= 2)
                            {
                                MessageData.unloadingport unloading = new MessageData.unloadingport("");
                                if (str[i].Contains('/'))
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length == 2)
                                    {
                                        if (msg[0].Length > 0 && !msg[0].Equals("SSR", StringComparison.OrdinalIgnoreCase) && !msg[0].Equals("OSI", StringComparison.OrdinalIgnoreCase))
                                        {
                                            unloading.unloadingairport = msg[0];
                                            unloading.nilcargocode = msg[1];
                                            Array.Resize(ref unloadingport, unloadingport.Length + 1);
                                            unloadingport[unloadingport.Length - 1] = unloading;
                                        }
                                    }
                                }
                                else
                                {
                                    if (str[i].Trim().Length == 3)
                                    {
                                        unloading.unloadingairport = str[i];
                                        Array.Resize(ref unloadingport, unloadingport.Length + 1);
                                        unloadingport[unloadingport.Length - 1] = unloading;
                                    }
                                }
                            }
                            #endregion

                            #region  line 4 onwards check consignment details
                            if (i > 1)
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    //0th element
                                    if (msg[0].Contains('-'))
                                    {
                                        decodeconsigmentdetails(str[i], ref consinfo);
                                    }

                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            #region Line 5 Dimendion info
                            if (str[i].StartsWith("DIM", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    int total = msg.Length / 3;
                                    Array.Resize(ref dimensioinfo, dimensioinfo.Length + total + 1);
                                    for (int cnt = 0; cnt < total; cnt++)
                                    {
                                        int place = 3 * cnt;
                                        MessageData.dimensionnfo dimension = new MessageData.dimensionnfo("");
                                        dimension.weightcode = msg[place + 1].Substring(0, 1);
                                        dimension.weight = msg[place + 1].Substring(1);
                                        if (msg.Length > 0)
                                        {
                                            string[] dimstr = msg[place + 2].Split('-');
                                            dimension.mesurunitcode = dimstr[0].Substring(0, 3);
                                            dimension.length = dimstr[0].Substring(3);
                                            dimension.weight = dimstr[1];
                                            dimension.height = dimstr[2];
                                        }
                                        dimension.piecenum = msg[place + 3];
                                        dimensioinfo[cnt] = dimension;
                                    }

                                }
                                catch (Exception e8) { }
                            }
                            #endregion

                            #region Line 7 ULD Specification
                            if (str[i].StartsWith("ULD", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    int uldnum = 0;
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 1)
                                    {
                                        fbldata.noofuld = msg[1];
                                        if (int.Parse(msg[1]) > 0)
                                        {
                                            Array.Resize(ref uld, uld.Length + 1 + int.Parse(msg[1]));
                                            for (int k = 2; k < msg.Length; k += 2)
                                            {
                                                MessageData.ULDinfo ulddata = new MessageData.ULDinfo("");
                                                string[] splitstr = msg[k].Split('-');
                                                ulddata.uldno = splitstr[0];
                                                ulddata.uldtype = splitstr[0].Substring(0, 3);
                                                ulddata.uldsrno = splitstr[0].Substring(3, splitstr[0].Length - 6);
                                                ulddata.uldowner = splitstr[0].Substring(splitstr[0].Length - 3, 3);
                                                ulddata.uldloadingindicator = splitstr[1];
                                                ulddata.uldweightcode = msg[k + 1].Substring(0, 1);
                                                ulddata.uldweight = msg[k + 1].Substring(1);
                                                uld[uldnum++] = ulddata;
                                            }
                                        }
                                    }
                                }
                                catch (Exception e4)
                                { }
                            }
                            #endregion

                            #region Line 8 Special Service request
                            if (str[i].StartsWith("SSR", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg[1].Length > 0)
                                    {
                                        fbldata.specialservicereq1 = msg[1];
                                    }

                                }
                                catch (Exception e5)
                                { }
                            }
                            #endregion

                            #region Line 9 Other service info
                            if (str[i].StartsWith("OSI", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg[1].Length > 0)
                                    {
                                        othinfo.otherserviceinfo1 = msg[1];
                                    }
                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Last line
                            if (i > str.Length - 2)
                            {
                                if (str[i].Trim().Length == 4 || str[i].Trim().Equals("LAST", StringComparison.OrdinalIgnoreCase) || str[i].Trim().Equals("CONT", StringComparison.OrdinalIgnoreCase))
                                {
                                    fbldata.endmesgcode = str[i].Trim();
                                }

                            }
                            #endregion

                            #region Other Info
                            if (str[i].StartsWith("/"))
                            {
                                string[] msg = str[i].Split('/');
                                try
                                {
                                    #region line 6 consigment origin info
                                    if (msg.Length > 0 && msg[0].Length == 0 && lastrec == "NA")
                                    {
                                        MessageData.consignmentorigininfo consorg = new MessageData.consignmentorigininfo();
                                        try
                                        {
                                            consorg.abbrivatedname = msg[1];
                                            consorg.carriercode = msg[2].Length > 0 ? msg[2].Substring(0, 2) : "";
                                            consorg.flightnum = msg[2].Length > 0 ? msg[2].Substring(2) : "";
                                            consorg.day = msg[3].Length > 0 ? msg[3].Substring(0, 2) : "";
                                            consorg.month = msg[3].Length > 0 ? msg[3].Substring(2) : "";
                                            consorg.airportcode = msg[4];
                                            consorg.movementprioritycode = msg[5];
                                        }
                                        catch (Exception ez) { }
                                        Array.Resize(ref consorginfo, consorginfo.Length + 1);
                                        consorginfo[consorginfo.Length - 1] = consorg;
                                    }
                                    #endregion

                                    #region SSR 2
                                    if (lastrec == "SSR")
                                    {
                                        fbldata.specialservicereq2 = msg[1].Length > 0 ? msg[1] : "";
                                        lastrec = "NA";
                                    }
                                    #endregion

                                    #region OSI 2
                                    if (lastrec == "OSI")
                                    {
                                        othinfo.otherserviceinfo2 = msg[1].Length > 0 ? msg[1] : "";
                                        lastrec = "NA";
                                    }
                                    #endregion
                                }
                                catch (Exception e13)
                                { }
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }      
    #endregion
       
    #region Decode Consigment Details
    public static void decodeconsigmentdetails(string inputstr, ref MessageData.consignmnetinfo[] consinfo)
    {
        try
        {
            MessageData.consignmnetinfo consig = new MessageData.consignmnetinfo("");
            string[] msg = inputstr.Split('/');
            string[] decmes = msg[0].Split('-');
            //consinfo[num] = new MessageData.consignmnetinfo("");
            consig.airlineprefix = decmes[0];
            string[] sptarr = stringsplitter(decmes[1]);
            if (sptarr.Length > 0)
            {
                try 
                {
                    consig.awbnum = sptarr[0];
                    if (sptarr[1].Length == 3)
                    {
                        consig.origin = "";
                        consig.dest = sptarr[1];
                    }
                    else if (sptarr[1].Length==6)
                    {
                        consig.origin = sptarr[1].Substring(0,3);
                        consig.dest = sptarr[1].Substring(3); ;
                    }
                }
                catch (Exception ex) 
                { }
            }
            else
            {
                try
                {
                    consig.awbnum = decmes[1].Substring(0, decmes[1].Length - 6);
                    consig.origin = decmes[1].Substring(decmes[1].Length - 6, 3);
                    consig.dest = decmes[1].Substring(decmes[1].Length - 3, 3);
                }
                catch (Exception ex) 
                { }
            }
            //1
            if (msg[1].Length > 0)
            {
                try
                {
                    int k = 0;
                    char lastchr = 'A';
                    char[] arr = msg[1].ToCharArray();
                    string[] strarr = new string[arr.Length];
                    for (int j = 0; j < arr.Length; j++)
                    {
                        if ((char.IsNumber(arr[j])) || (arr[j].Equals('.')))
                        {//number                            
                            if (lastchr == 'N')
                                k--;
                            strarr[k] = strarr[k] + arr[j].ToString();
                            lastchr = 'N';
                        }
                        if (char.IsLetter(arr[j]))
                        {//letter
                            if (lastchr == 'L')
                                k--;
                            strarr[k] = strarr[k] + arr[j].ToString();
                            lastchr = 'L';
                        }
                        k++;
                    }
                    consig.consigntype = strarr[0];
                    consig.pcscnt = strarr[1];//int.Parse(strarr[1]);
                    consig.weightcode = strarr[2];
                    consig.weight = strarr[3];//float.Parse(strarr[3]);
                    for (k = 4; k < strarr.Length; k += 2)
                    {
                        if (strarr[k] != null)
                        {
                            if (strarr[k] == "T")
                            {
                                consig.shpdesccode = strarr[k];
                                consig.numshp = strarr[k + 1];
                                k = strarr.Length + 1;
                            }
                            else if (strarr[k] == "DG")
                            {
                                consig.densityindicator = strarr[k];
                                consig.densitygrp = strarr[k + 1];
                            }
                            else
                            {
                                consig.volumecode = strarr[k];
                                consig.volumeamt = strarr[k + 1];
                            }
                        }
                    }
                }
                catch (Exception ex) { }
            }
            if (msg.Length > 2)
            {
                try
                {
                    //2 Manifest Description
                    consig.manifestdesc = msg[2];
                }
                catch (Exception ex) { }

            }
            if (msg.Length > 3)
            {//3 SHC- special handling code
                try
                {
                    consig.splhandling = "";
                    for (int j = 3; j < msg.Length; j++)
                        consig.splhandling = consig.splhandling + msg[j] + ",";
                }
                catch (Exception ex) { }
            }
            try
            {
                if (unloadingportsequence.Length > 0)
                    consig.portsequence = unloadingportsequence;
                if (uldsequencenum.Length > 0)
                    consig.uldsequence = uldsequencenum;
            }
            catch (Exception ec) { }
            Array.Resize(ref consinfo, consinfo.Length + 1);
            consinfo[consinfo.Length - 1] = consig;
            awbref = consinfo.Length.ToString();
        }
        catch (Exception ex) { }
    }
    #endregion

    #region encodeFBL
    public static string EncodeFBLforsend(MessageData.fblinfo fbldata, MessageData.unloadingport[] unloadingport, MessageData.consignmnetinfo[] consinfo, MessageData.dimensionnfo[] dimensioinfo, MessageData.consignmentorigininfo[] consorginfo, MessageData.ULDinfo[] uld, MessageData.otherserviceinfo othinfo)
    {
        string fbl = null;
        try
        {

            #region Line 1
            string line1 = "FBL" + "/" + fbldata.fblversion;
            #endregion

            #region Line 2
            string line2 = "";
            line2 = fbldata.messagesequencenum + "/" + fbldata.carriercode + fbldata.fltnum + "/" + fbldata.date + fbldata.month + "/" + fbldata.fltairportcode + (fbldata.aircraftregistration.Length > 1 ? ("/" + fbldata.aircraftregistration) : "");
            #endregion

            #region Line 3
            string line3 = "";
            if (unloadingport.Length > 0)
            {
                for (int i = 0; i < unloadingport.Length; i++)
                {
                    line3 = line3 + unloadingport[i].unloadingairport + (unloadingport[i].nilcargocode.Length > 0 ? ("/" + unloadingport[i].nilcargocode) : "") + "$";
                }
                line3 = line3.Trim('$');
                line3 = line3.Replace("$", "\r\n");
            }
            #endregion

            #region line 4
            string line4 = "";
            if (consinfo.Length > 0)
            {
                for (int i = 0; i < consinfo.Length; i++)
                {
                    string splhandling = "";
                    if (consinfo[i].splhandling.Length > 0 && consinfo[i].splhandling != null)
                    {
                        splhandling = consinfo[i].splhandling.Replace(",", "");
                        splhandling = "/" + splhandling;
                    }
                    line4 = line4 + consinfo[i].airlineprefix + "-" + consinfo[i].awbnum + consinfo[i].origin + consinfo[i].dest + "/" + consinfo[i].consigntype + consinfo[i].pcscnt + consinfo[i].weightcode + consinfo[i].weight + consinfo[i].volumecode + consinfo[i].volumeamt + consinfo[i].densityindicator + consinfo[i].densitygrp + consinfo[i].shpdesccode + consinfo[i].numshp + "/" + consinfo[i].manifestdesc + splhandling;
                    line4 = line4.Trim('/') + "$";

                }
                line4 = line4.Trim('$');
                line4 = line4.Replace("$", "\r\n");
            }
            #endregion

            #region Line 5
            string line5 = "";
            if (dimensioinfo.Length > 0)
            {
                line5 = "DIM/";
                for (int i = 0; i < dimensioinfo.Length; i++)
                {
                    if (dimensioinfo[i].height.Length > 0 || dimensioinfo[i].length.Length > 0 || dimensioinfo[i].piecenum.Length > 0 || dimensioinfo[i].weight.Length > 0 || dimensioinfo[i].width.Length > 0)
                    {
                        line5 = line5 + dimensioinfo[i].weightcode + dimensioinfo[i].weight + "/" + dimensioinfo[i].mesurunitcode + dimensioinfo[i].length + "-" + dimensioinfo[i].width + "-" + dimensioinfo[i].height + "/" + dimensioinfo[i].piecenum;
                        line5 = line5.Trim('/') + "$";
                    }
                }
                line5 = line5.Trim('$');
                line5 = line5.Replace("$", "\r\n");
            }
            #endregion

            #region Line6 consignment origin info
            string line6 = "";
            if (consorginfo.Length > 1)
            {
                for (int i = 0; i < consorginfo.Length; i++)
                {

                    if (consorginfo[i].abbrivatedname.Length > 0 || consorginfo[i].carriercode.Length > 0 || consorginfo[i].flightnum.Length > 0 || consorginfo[i].day.Length > 0 || consorginfo[i].month.Length > 0 || consorginfo[i].airportcode.Length > 0 || consorginfo[i].movementprioritycode.Length > 0)
                    {
                        line6 = line6 + consorginfo[i].abbrivatedname + "/" + consorginfo[i].carriercode + consorginfo[i].flightnum + "/" + consorginfo[i].day + consorginfo[i].month + "/" + consorginfo[i].airportcode + "/" + consorginfo[i].movementprioritycode;
                        line6 = line6.Trim('/') + "$";
                    }

                }
                line6 = line6.Trim('$');
                line6 = "/" + line6.Replace("$", "\r\n/");
            }

            #endregion

            #region Line7 ULD
            string line7 = "";
            if (fbldata.noofuld.Length > 0)
            {
                line7 = "ULD/" + fbldata.noofuld + "/";
                string uldinfo = null;
                for (int i = 0; i < int.Parse(fbldata.noofuld); i++)
                {
                    uldinfo = null;
                    uldinfo = uld[i].uldtype + uld[i].uldsrno + uld[i].uldowner + "-" + uld[i].uldloadingindicator + "/" + uld[i].uldweightcode + uld[i].uldweight;
                    if (uldinfo.Length > 2)
                        line7 = line7 + uldinfo + "/";
                }
            }
            #endregion

            #region Line 8 SSR
            string line8 = "";
            if (fbldata.specialservicereq1.Length > 0)
            {
                line8 = "SSR/" + fbldata.specialservicereq1;
                if (fbldata.specialservicereq2.Length > 0)
                {
                    line8 = line8 + "\r\n" + "/" + fbldata.specialservicereq2;
                }
            }
            #endregion

            #region Line 9 other service info
            string line9 = "";
            if (othinfo.otherserviceinfo1.Length > 0 || othinfo.otherserviceinfo2.Length > 0)
            {
                line9 = "OSI/" + othinfo.otherserviceinfo1 + "\r\n" + "/" + othinfo.otherserviceinfo2;
            }
            #endregion

            #region BuildFFR
            fbl = line1.Trim('/') + "\r\n" + line2.Trim() + "\r\n" + line3.Trim() + "\r\n" + line4.Trim();
            if (line5.Length > 1)
            {
                fbl = fbl + "\r\n" + line5.Trim();
            }
            if (line6.Length > 1)
            {
                fbl = fbl + "\r\n" + line6.Trim();
            }
            if (line7.Length > 1)
            {
                fbl = fbl + "\r\n" + line7.Trim();
            }
            if (line8.Length > 1)
            {
                fbl = fbl + "\r\n" + line8.Trim();
            }
            if (line9.Length > 1)
            {
                fbl = fbl + "\r\n" + line9.Trim();
            }
            fbl = fbl + "\r\n" + fbldata.endmesgcode;
            #endregion

        }
        catch (Exception ex)
        {
            fbl = "ERR";
        }
        return fbl;
    }
    #endregion

    //FFM

    #region Decode FFM message
    public static bool decodereceiveFFM(string ffmmsg, ref MessageData.ffminfo ffmdata, ref MessageData.unloadingport[] unloadingport, ref MessageData.consignmnetinfo[] consinfo, ref MessageData.dimensionnfo[] dimensioinfo, ref MessageData.ULDinfo[] uld, ref MessageData.otherserviceinfo[] othinfoarray, ref MessageData.customsextrainfo[] custominfo, ref MessageData.movementinfo[] movementinfo)//(string ffmmsg)
    {

        bool flag = false;
        try
        {
            string lastrec = "NA";
            int line = 0;
            uldsequencenum = "";
            unloadingportsequence = "";
            try
            {
                if (ffmmsg.StartsWith("FFM", StringComparison.OrdinalIgnoreCase))
                {                    
                    string[] str = ffmmsg.Split('$');//Regex.Split(ffmmsg, "\r\n");//ffrmsg.Split('$');
                    if (str.Length > 3)
                    {
                        for (int i = 0; i < str.Length; i++)
                        {

                            flag = true;
                            #region Line 1
                            if (str[i].StartsWith("FFM", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    ffmdata.ffmversionnum = msg[1];
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region line 2 flight data
                            if (i == 1)
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 1)
                                    {
                                        ffmdata.messagesequencenum = msg[0];
                                        ffmdata.carriercode = msg[1].Substring(0, 2);
                                        ffmdata.fltnum = msg[1].Substring(2);
                                        ffmdata.fltdate = msg[2].Substring(0, 2);
                                        ffmdata.month = msg[2].Substring(2, 3);
                                        ffmdata.time = msg[2].Substring(5).Length > 0 ? msg[2].Substring(5) : "";
                                        ffmdata.fltairportcode = msg[3];
                                        ffmdata.aircraftregistration = msg[4];
                                        if (msg.Length > 4)
                                        {
                                            ffmdata.countrycode = msg[5];
                                            ffmdata.fltdate1 = msg[6].Substring(0, 2);
                                            ffmdata.fltmonth1 = msg[6].Substring(2, 3);
                                            ffmdata.flttime1 = msg[6].Substring(5); ;
                                            ffmdata.fltairportcode1 = msg[7];
                                        }
                                    }
                                }
                                catch (Exception ex)
                                { }
                            }
                            #endregion

                            #region line 3 point of unloading
                            if (i >= 2)
                            {
                                MessageData.unloadingport unloading = new MessageData.unloadingport("");
                                if (str[i].Contains('/'))
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length >= 2)
                                    {
                                        if (msg[0].Length > 0 && !msg[0].StartsWith("/") && !msg[0].Contains('-')
                                            && !msg[0].Equals("SSR", StringComparison.OrdinalIgnoreCase)
                                            && !msg[0].Equals("SCI", StringComparison.OrdinalIgnoreCase) 
                                            && !msg[0].Equals("OSI", StringComparison.OrdinalIgnoreCase) 
                                            && !msg[0].Equals("ULD", StringComparison.OrdinalIgnoreCase)                                             
                                            && !msg[0].Equals("COR", StringComparison.OrdinalIgnoreCase))
                                        {
                                            unloading.unloadingairport = msg[0];
                                            unloading.nilcargocode = msg[1];
                                            try
                                            {
                                                if (msg.Length > 2)
                                                {
                                                    unloading.day = msg[2].Substring(0, 2);
                                                    unloading.month = msg[2].Substring(2, 3);
                                                    unloading.time = msg[2].Substring(5);
                                                    unloading.day1 = msg[3].Substring(0, 2);
                                                    unloading.month1 = msg[3].Substring(2, 3);
                                                    unloading.time1 = msg[3].Substring(5);
                                                }
                                            }
                                            catch (Exception ex) { }
                                            Array.Resize(ref unloadingport, unloadingport.Length + 1);
                                            unloadingport[unloadingport.Length - 1] = unloading;
                                            //for sequence app
                                            unloadingport[unloadingport.Length - 1].sequencenum = unloadingport.Length.ToString();
                                            unloadingportsequence = unloadingport.Length.ToString();
                                            uldsequencenum = "";
                                        }
                                    }
                                }
                                else
                                {
                                    if (str[i].Trim().Length == 3)
                                    {
                                        unloading.unloadingairport = str[i];
                                        Array.Resize(ref unloadingport, unloadingport.Length + 1);
                                        unloadingport[unloadingport.Length - 1] = unloading;
                                        //for sequence app
                                        unloadingport[unloadingport.Length - 1].sequencenum = unloadingport.Length.ToString();
                                        unloadingportsequence = unloadingport.Length.ToString();
                                        uldsequencenum = "";
                                    }
                                }
                            }
                            #endregion

                            #region  line 4 onwards check consignment details
                            if (i > 1)
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    //0th element
                                    if (msg[0].Contains('-'))
                                    {
                                        try
                                        {//Version below 5 - 1 row of AWB information (AWB+SHC)
                                            //V5 - 2 Rows - 1.AWB info 2. SHC
                                            if (Convert.ToInt16(ffmdata.ffmversionnum) > 4)
                                            {
                                                lastrec = "AWB";
                                            }
                                            else
                                            {
                                                lastrec = "NA";
                                            }
                                        }
                                        catch (Exception ex) { }
                                        line = 0;
                                        decodeconsigmentdetails(str[i],ref consinfo);
                                    }

                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            #region Line 5 Dimendion info
                            if (str[i].StartsWith("DIM", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    int total = msg.Length / 3;
                                    Array.Resize(ref dimensioinfo, dimensioinfo.Length + total + 1);
                                    for (int cnt = 0; cnt < total; cnt++)
                                    {
                                        int place = 3 * cnt;
                                        MessageData.dimensionnfo dimension = new MessageData.dimensionnfo("");
                                        dimension.weightcode = msg[place + 1].Substring(0, 1);
                                        dimension.weight = msg[place + 1].Substring(1);
                                        if (msg.Length > 0)
                                        {
                                            string[] dimstr = msg[place + 2].Split('-');
                                            dimension.mesurunitcode = dimstr[0].Substring(0, 3);
                                            dimension.length = dimstr[0].Substring(3);
                                            dimension.weight = dimstr[1];
                                            dimension.height = dimstr[2];
                                        }
                                        dimension.piecenum = msg[place + 3];
                                        dimension.consigref = awbref;
                                        dimensioinfo[cnt] = dimension;

                                    }

                                }
                                catch (Exception e8) { }
                            }
                            #endregion

                            #region Line 10 ULD Specification
                            if (str[i].StartsWith("ULD", StringComparison.OrdinalIgnoreCase))
                            {
                                MessageData.ULDinfo ulddata = new MessageData.ULDinfo("");
                                try
                                {
                                    string[] msg = str[i].Trim().Split('/');
                                    if (msg.Length > 1)
                                    {

                                        string[] splitstr = msg[1].Split('-');                                        
                                        ulddata.uldno = splitstr[0];
                                        ulddata.uldtype = splitstr[0].Substring(0, 3);
                                        ulddata.uldsrno = splitstr[0].Substring(3, splitstr[0].Length - 5);
                                        ulddata.uldowner = splitstr[0].Substring(splitstr[0].Length - 2, 2);
                                        if (splitstr.Length > 0)
                                        {
                                            ulddata.uldloadingindicator = splitstr[1];
                                        }
                                        if (msg.Length > 1)
                                        {
                                            ulddata.uldremark = msg[2];
                                        }

                                    }

                                }
                                catch (Exception e4)
                                { }
                                Array.Resize(ref uld, uld.Length + 1);
                                uld[uld.Length - 1] = ulddata;
                                if (int.Parse(unloadingportsequence) > 0)
                                {
                                    uld[uld.Length - 1].portsequence = unloadingportsequence;
                                    uld[uld.Length - 1].refuld = uld.Length.ToString();
                                    uldsequencenum = uld.Length.ToString();
                                }
                            }
                            #endregion

                            #region Line 7 Other service info
                            if (str[i].StartsWith("OSI", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg[1].Length > 0)
                                    {
                                        Array.Resize(ref othinfoarray, othinfoarray.Length + 1);
                                        othinfoarray[othinfoarray.Length - 1].otherserviceinfo1 = msg[1];
                                        othinfoarray[othinfoarray.Length - 1].consigref = awbref;

                                    }

                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 8 COR
                            if (str[i].StartsWith("COR", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                if (msg[1].Length > 0)
                                {
                                    consinfo[int.Parse(awbref) - 1].customorigincode = msg[1];
                                }
                            }
                            #endregion

                            #region Line9 custom extra info
                            if (str[i].StartsWith("OCI", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 0)
                                {
                                    lastrec = "OCI";
                                    MessageData.customsextrainfo custom = new MessageData.customsextrainfo("");
                                    custom.IsoCountryCodeOci = msg[1];
                                    custom.InformationIdentifierOci = msg[2];
                                    custom.CsrIdentifierOci = msg[3];
                                    custom.SupplementaryCsrIdentifierOci = msg[4];
                                    custom.consigref = awbref;
                                    Array.Resize(ref custominfo, custominfo.Length + 1);
                                    custominfo[custominfo.Length - 1] = custom;
                                }
                            }
                            #endregion

                            #region Line11 special custom information
                            if (str[i].StartsWith("SCI", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 0)
                                    {
                                        consinfo[int.Parse(awbref) - 1].customorigincode = msg[2];
                                        consinfo[int.Parse(awbref) - 1].customref = msg[1];

                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Last line
                            if (i > str.Length - 3)
                            {
                                if (str[i].Trim().Length == 4 || str[i].Trim().Equals("LAST", StringComparison.OrdinalIgnoreCase) || str[i].Trim().Equals("CONT", StringComparison.OrdinalIgnoreCase))
                                {
                                    ffmdata.endmesgcode = str[i].Trim();
                                }

                            }
                            #endregion

                            #region Other Info
                            if (str[i].StartsWith("/"))
                            {
                                string[] msg = str[i].Split('/');
                                try
                                {
                                    #region line 6 movementinfo info
                                    try
                                    {
                                        if (msg.Length > 0 && msg[0].Length == 0 && lastrec == "NA")
                                        {
                                            lastrec = "MOV";
                                            MessageData.movementinfo movement = new MessageData.movementinfo("");
                                            try
                                            {
                                                movement.AirportCode = msg[1].Substring(0, 3);
                                                movement.CarrierCode = "";
                                                movement.FlightNumber = msg[1].Substring(3);//Carrier+FLT
                                                movement.FlightDay = msg[2].Substring(0, 2);
                                                movement.FlightMonth = msg[2].Substring(2);
                                                movement.consigref = awbref;
                                            }
                                            catch (Exception ez) { }
                                            Array.Resize(ref movementinfo, movementinfo.Length + 1);
                                            movementinfo[movementinfo.Length - 1] = movement;
                                        }

                                        if (lastrec == "MOV")
                                        {
                                            if (msg[1].Length > 0)
                                            {
                                                movementinfo[movementinfo.Length - 1].PriorityorVolumecode = msg[1];

                                            }
                                            lastrec = "NA";
                                        }
                                    }
                                    catch (Exception ex) { }
                                    #endregion

                                    #region SSR 2
                                    if (lastrec == "SSR")
                                    {
                                        ffmdata.specialservicereq2 = msg[1].Length > 0 ? msg[1] : "";
                                        lastrec = "NA";
                                    }
                                    #endregion

                                    #region OSI 2
                                    if (lastrec == "OSI")
                                    {
                                        othinfoarray[othinfoarray.Length - 1].otherserviceinfo2 = msg[1].Length > 0 ? msg[1] : "";
                                        lastrec = "NA";
                                    }
                                    #endregion

                                    #region Splhandling
                                    if (lastrec == "AWB")
                                    {
                                        try
                                        {
                                            if (str[i].Length > 1)
                                            {
                                                consinfo[consinfo.Length - 1].splhandling = str[i].Replace('/', ',');
                                            }
                                        }
                                        catch (Exception ex) { }
                                        lastrec = "NA";
                                    }
                                    #endregion

                                    #region OCI
                                    if (lastrec == "OCI")
                                    {
                                        string[] msgdata = str[i].Split('/');
                                        if (msgdata.Length > 0)
                                        {
                                            lastrec = "OCI";
                                            MessageData.customsextrainfo custom = new MessageData.customsextrainfo("");
                                            custom.IsoCountryCodeOci = msgdata[1];
                                            custom.InformationIdentifierOci = msgdata[2];
                                            custom.CsrIdentifierOci = msgdata[3];
                                            custom.SupplementaryCsrIdentifierOci = msgdata[4];
                                            Array.Resize(ref custominfo, custominfo.Length + 1);
                                            custominfo[custominfo.Length - 1] = custom;
                                        }
                                    }
                                    #endregion
                                }
                                catch (Exception e13)
                                { }
                            }
                            #endregion
                        }
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }
    #endregion

    #region Encode FFM
    public static string EncodeFFMforsend(ref MessageData.ffminfo ffmdata, ref MessageData.unloadingport[] unloadingport, ref MessageData.consignmnetinfo[] consinfo, ref MessageData.dimensionnfo[] dimensioinfo, ref MessageData.movementinfo[] movementinfo, ref MessageData.otherserviceinfo[] othinfoarray, ref MessageData.customsextrainfo[] custominfo, ref MessageData.ULDinfo[] uld)
    {
        string ffm = null, flightcons = "", uldcons = "";
        try
        {
            #region Line 1
            string line1 = "FFM" + "/" + ffmdata.ffmversionnum;
            #endregion

            #region Line 2
            string line2 = "";
            line2 = ffmdata.messagesequencenum + "/" + ffmdata.carriercode + ffmdata.fltnum + "/" + ffmdata.fltdate + ffmdata.month + ffmdata.time + "/" + ffmdata.fltairportcode + (ffmdata.aircraftregistration.Length > 1 ? ("/" + ffmdata.aircraftregistration) : "");
            if (ffmdata.countrycode.Length > 0 || ffmdata.fltdate1.Length > 0)
            {
                line2 = line2 + "/" + ffmdata.countrycode + "/" + ffmdata.fltdate1 + ffmdata.fltmonth1 + ffmdata.flttime1 + "/" + ffmdata.fltairportcode1;
            }
            #endregion

            #region Line 3 point of unloading
            string line3 = "";
            if (unloadingport.Length > 0)
            {
                for (int i = 0; i < unloadingport.Length; i++)
                {
                    line3 = line3+"$"+ unloadingport[i].unloadingairport + (unloadingport[i].nilcargocode.Length > 0 ? ("/" + unloadingport[i].nilcargocode) : "");
                    if (unloadingport[i].day.Length > 0)
                    {
                        line3 = line3 + "/" + unloadingport[i].day + "/" + unloadingport[i].month + "/" + unloadingport[i].time;
                    }
                    if (unloadingport[i].day1.Length > 0)
                    {
                        line3 = line3 + "/" + unloadingport[i].day1 + "/" + unloadingport[i].month1 + "/" + unloadingport[i].time1;
                    }
                    line3 = line3 + "$";
                    flightcons = FFMPartBuilder((i + 1).ToString(), "", ref consinfo, ref dimensioinfo, ref movementinfo, ref othinfoarray, ref custominfo);
                    if (flightcons.Length > 3)
                    {
                        line3 = line3 + flightcons;
                    }
                    if (uld.Length > 0)
                    {
                        string uldstr = "";
                        for (int k = 0; k < uld.Length; k++)
                        {
                            if (uld[k].portsequence.Equals((i + 1).ToString()))
                            {//uld is there
                                uldstr = uldstr.Trim() + "$ULD/" + uld[k].uldtype + uld[k].uldsrno + uld[k].uldowner + (uld[k].uldloadingindicator.Length > 0 ? ("-" + uld[k].uldloadingindicator) : "") + (uld[k].uldremark.Length > 0 ? ("/" + uld[k].uldremark) : "") + "$";
                                uldcons = FFMPartBuilder((i + 1).ToString(), uld[k].refuld, ref consinfo, ref dimensioinfo, ref movementinfo, ref othinfoarray, ref custominfo);
                                if (uldcons.Length > 3)
                                {
                                    uldstr = uldstr.Trim('$') + "\r\n" + uldcons;
                                }
                            }
                        }
                        uldstr = uldstr.Trim('$');
                        uldstr = uldstr.Replace("$", "\r\n");
                        if (uldstr.Length > 0)
                        {
                            line3 = line3 + "$" + uldstr;
                        }
                    }
                }
                line3 = line3.Replace("$$","$");
                line3 = line3.Trim('$');
                line3 = line3.Replace("$", "\r\n");
            }
            #endregion

            #region Line 10 ULD
            //string line10 = "";
            //if (uld.Length > 0)
            //{
            //    line10 = "";                    
            //    for (int i = 0; i < uld.Length; i++)
            //    {

            //        line10 = line10.Trim() + "ULD/" + uld[i].uldtype + uld[i].uldsrno + uld[i].uldowner + (uld[i].uldloadingindicator.Length > 0 ? ("-" + uld[i].uldloadingindicator) : "") + (uld[i].uldremark.Length > 0 ? ("/" + uld[i].uldremark) : "") + "$";                        
            //    }
            //    line10=line10.Trim('$');
            //    line10=line10.Replace("$","\r\n");
            //}
            #endregion

            #region BuildFFM
            ffm = line1.Trim('/') + "\r\n" + line2.Trim() + "\r\n" + line3.Trim() + "\r\n" + ffmdata.endmesgcode;
            #endregion

        }
        catch (Exception ex)
        {
            ffm = "ERR";
        }
        return ffm;
    }

    public static string FFMPartBuilder(string flightref, string uldref, ref MessageData.consignmnetinfo[] consinfo, ref MessageData.dimensionnfo[] dimensioinfo, ref MessageData.movementinfo[] movementinfo, ref MessageData.otherserviceinfo[] othinfoarray, ref MessageData.customsextrainfo[] custominfo)
    {
        string output = "";
        try
        {
            #region line 4 Consigment INfo
            string line4 = "";
            if (consinfo.Length > 0)
            {
                for (int i = 0; i < consinfo.Length; i++)
                {

                    string splhandling = "";
                    if (consinfo[i].portsequence.Equals(flightref) && consinfo[i].uldsequence.Equals(uldref))
                    {
                        if (consinfo[i].splhandling.Length > 0 && consinfo[i].splhandling != null)
                        {
                            splhandling = consinfo[i].splhandling.Replace(",", "/");
                            splhandling = "\r\n/" + splhandling;
                        }
                        line4 = line4 + consinfo[i].airlineprefix + "-" + consinfo[i].awbnum + consinfo[i].origin + consinfo[i].dest + "/" + consinfo[i].consigntype + consinfo[i].pcscnt + consinfo[i].weightcode + consinfo[i].weight + consinfo[i].volumecode + consinfo[i].volumeamt + consinfo[i].densityindicator + consinfo[i].densitygrp + consinfo[i].shpdesccode + consinfo[i].numshp + "/" + consinfo[i].manifestdesc + splhandling;
                        line4 = line4.Trim('/') + "$";

                        #region Line 5 Dimension info
                        string line5 = "";
                        if (dimensioinfo.Length > 0)
                        {
                            line5 = "DIM/";
                            for (int j = 0; j < dimensioinfo.Length; j++)
                            {
                                if (dimensioinfo[j].consigref.Equals((i + 1).ToString()))
                                {
                                    if (dimensioinfo[j].height.Length > 0 || dimensioinfo[j].length.Length > 0 || dimensioinfo[j].piecenum.Length > 0 || dimensioinfo[j].weight.Length > 0 || dimensioinfo[j].width.Length > 0)
                                    {
                                        line5 = line5 + dimensioinfo[j].weightcode + dimensioinfo[j].weight + "/" + dimensioinfo[j].mesurunitcode + dimensioinfo[j].length + "-" + dimensioinfo[j].width + "-" + dimensioinfo[j].height + "/" + dimensioinfo[j].piecenum;
                                        line5 = line5.Trim('/') + "$";
                                    }
                                }
                            }
                            line5 = line5.Trim('$');
                            line5 = line5.Replace("$", "\r\n");
                            if (line5.Length > 0)
                            {
                                output = output + line5 + "\r\n";
                            }
                        }
                        #endregion

                        #region line6 movement info
                        string line6 = "";
                        if (movementinfo.Length > 0)
                        {
                            for (int k = 0; k < movementinfo.Length; k++)
                            {
                                if (movementinfo[k].consigref.Equals((i + 1).ToString()))
                                {
                                    line6 = line6 + "/" + movementinfo[k].AirportCode + movementinfo[k].CarrierCode + movementinfo[k].FlightNumber + "/" + movementinfo[k].FlightDay + movementinfo[k].FlightMonth + "$" + "/" + movementinfo[k].PriorityorVolumecode;
                                }
                            }
                            line6 = line6.Replace("$", "\r\n");
                            if (line6.Length > 0)
                            {
                                output = output + line6 + "\r\n";
                            }
                        }

                        #endregion

                        #region Line 7 other service info
                        string line7 = "";
                        if (othinfoarray.Length > 0)
                        {
                            for (int j = 0; j < othinfoarray.Length; j++)
                            {
                                if (othinfoarray[j].consigref.Equals((i + 1).ToString()))
                                {
                                    if (othinfoarray[j].otherserviceinfo1.Length > 0)
                                    {
                                        line7 = "OSI/" + othinfoarray[j].otherserviceinfo1 + "$";
                                        if (othinfoarray[j].otherserviceinfo2.Length > 0)
                                        {
                                            line7 = line7 + "/" + othinfoarray[j].otherserviceinfo2 + "$";
                                        }
                                    }
                                }
                            }
                            line7 = line7.Trim('$');
                            line7 = line7.Replace("$", "\r\n");
                            if (line7.Length > 0)
                            {
                                output = output + line7 + "\r\n";
                            }
                        }
                        #endregion

                        #region Line 8 Custom origin
                        if (consinfo[i].customorigincode.Length > 0)
                        {
                            output = output + "COR/" + consinfo[i].customorigincode + "\r\n";
                        }
                        #endregion

                        #region Line 9
                        string line9 = "";
                        if (custominfo.Length > 0)
                        {
                            for (int k = 0; k < custominfo.Length; k++)
                            {
                                if (custominfo[k].consigref.Equals((i + 1).ToString()))
                                {
                                    line9 = "/" + custominfo[k].IsoCountryCodeOci + "/" + custominfo[k].InformationIdentifierOci + "/" + custominfo[k].CsrIdentifierOci + "/" + custominfo[k].SupplementaryCsrIdentifierOci + "$";
                                }

                            }
                            line9 = "OCI" + line9.Trim('$');
                            line9 = line9.Replace("$", "\r\n");
                            if (line9.Length > 0)
                            {
                                output = output + line9 + "\r\n";
                            }
                        }
                        #endregion
                    }


                }
                line4 = line4.Trim('$');
                line4 = line4.Replace("$", "\r\n");
            }
            #endregion
            if (output.Length > 0)
            {
                output = line4 + "\r\n" + output;
            }
            else
            {
                output = line4;
            }

        }
        catch (Exception ex)
        {
            output = "ERR";
        }
        return output;
    }

    #endregion

    //FSA

    #region Decoed FSA Message
    public static bool decodeReceiveFSA(string fsamsg, ref MessageData.FSAInfo fsadata, ref MessageData.CommonStruct[] fsanodes, ref MessageData.customsextrainfo[] custominfo, ref MessageData.ULDinfo[] uld, ref MessageData.otherserviceinfo[] othinfoarray)
    {
        bool flag = false;
        string lastrec = "";
        try
        {
            if (fsamsg.StartsWith("FSA", StringComparison.OrdinalIgnoreCase) || fsamsg.StartsWith("FSU", StringComparison.OrdinalIgnoreCase))
            {                
                string[] str = fsamsg.Split('$');//Regex.Split(fsamsg, "\r\n");//ffrmsg.Split('$');
                if (str.Length > 2)
                {
                    flag = true;

                    #region Line 1
                    if (str[0].StartsWith("FSA", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            string[] msg = str[0].Split('/');
                            fsadata.fsaversion = msg[1];
                        }
                        catch (Exception ec) { }
                    }
                    #endregion

                    #region Line 2 awb consigment details
                    try
                    {
                        string[] msg = str[1].Split('/');
                        //0th element
                        string[] decmes = msg[0].Split('-');
                        fsadata.airlineprefix = decmes[0];
                        fsadata.awbnum = decmes[1].Substring(0, decmes[1].Length - 6);
                        fsadata.origin = decmes[1].Substring(decmes[1].Length - 6, 3);
                        fsadata.dest = decmes[1].Substring(decmes[1].Length - 3, 3);
                        //1
                        if (msg[1].Length > 0)
                        {
                            int k = 0;
                            char lastchr = 'A';
                            char[] arr = msg[1].ToCharArray();
                            string[] strarr = new string[arr.Length];
                            for (int j = 0; j < arr.Length; j++)
                            {
                                if ((char.IsNumber(arr[j])) || (arr[j].Equals('.')))
                                {//number                            
                                    if (lastchr == 'N')
                                        k--;
                                    strarr[k] = strarr[k] + arr[j].ToString();
                                    lastchr = 'N';
                                }
                                if (char.IsLetter(arr[j]))
                                {//letter
                                    if (lastchr == 'L')
                                        k--;
                                    strarr[k] = strarr[k] + arr[j].ToString();
                                    lastchr = 'L';
                                }
                                k++;
                            }
                            fsadata.consigntype = strarr[0];
                            fsadata.pcscnt = strarr[1];//int.Parse(strarr[1]);
                            fsadata.weightcode = strarr[2];
                            fsadata.weight = strarr[3];//float.Parse(strarr[3]);
                            for (k = 4; k < strarr.Length; k += 2)
                            {
                                if (strarr[k] != null)
                                {
                                    if (strarr[k] == "T")
                                    {
                                        fsadata.totalpcscnt = strarr[k + 1];
                                    }
                                }
                            }
                        }


                    }
                    catch (Exception e)
                    { }
                    #endregion

                    for (int i = 2; i < str.Length; i++)
                    {
                        #region Decode
                        if (str[i].Length > 0)
                        {
                            string[] msg = str[i].Split('/');
                            MessageData.CommonStruct recdata = new MessageData.CommonStruct("");
                            switch (msg[0])
                            {
                                case "FOH":
                                case "RCS":
                                    {
                                        #region RCS
                                        if (msg.Length > 0)
                                        {
                                            try
                                            {
                                                recdata.messageprefix = msg[0];
                                                recdata.fltday = msg[1].Substring(0, 2);
                                                recdata.fltmonth = msg[1].Substring(2, 3);
                                                recdata.flttime = msg[1].Substring(5);
                                                recdata.airportcode = msg[2];
                                                string[] arr = stringsplitter(msg[3]);
                                                if (arr.Length > 1)
                                                {
                                                    recdata.pcsindicator = arr[0];
                                                    recdata.numofpcs = arr[1];
                                                    recdata.weightcode = arr[2];
                                                    recdata.weight = arr[3];
                                                }
                                                if (msg.Length > 4)
                                                {
                                                    recdata.name = msg[4];
                                                    string[] strarr = stringsplitter(msg[5]);
                                                    if (strarr.Length > 0)
                                                    {
                                                        recdata.volumecode = strarr[0];
                                                        recdata.volumeamt = strarr[1];
                                                        recdata.densityindicator = strarr[2].Substring(0, 2);
                                                        recdata.densitygroup = strarr[2].Substring(2);
                                                    }
                                                }

                                            }
                                            catch (Exception ex) { }
                                            Array.Resize(ref fsanodes, fsanodes.Length + 1);
                                            fsanodes[fsanodes.Length - 1] = recdata;
                                        }
                                        #endregion
                                    }
                                    break;
                                case "RCT":
                                    {
                                        #region RCT
                                        if (msg.Length > 0)
                                        {
                                            try
                                            {
                                                recdata.messageprefix = msg[0];
                                                recdata.carriercode = msg[1];
                                                recdata.fltday = msg[2].Substring(0, 2);
                                                recdata.fltmonth = msg[2].Substring(2, 3);
                                                recdata.flttime = msg[2].Substring(5);
                                                recdata.airportcode = msg[3];
                                                string[] arr = stringsplitter(msg[4]);
                                                if (arr.Length > 1)
                                                {
                                                    try
                                                    {
                                                        recdata.pcsindicator = arr[0];
                                                        recdata.numofpcs = arr[1];
                                                        recdata.weightcode = arr[2];
                                                        recdata.weight = arr[3].Length > 0 ? arr[3] : "";
                                                    }
                                                    catch (Exception ec) { }
                                                }
                                                if (msg.Length > 5)
                                                {
                                                    recdata.seccarriercode = msg[5].Length > 0 ? msg[5] : "";
                                                }
                                                if (msg.Length > 6)
                                                {
                                                    recdata.name = msg[6].Length > 0 ? msg[6] : "";
                                                }
                                            }
                                            catch (Exception ex) { }
                                            Array.Resize(ref fsanodes, fsanodes.Length + 1);
                                            fsanodes[fsanodes.Length - 1] = recdata;
                                        }
                                        #endregion
                                    } break;
                                case "DIS":
                                    {
                                        #region DIS
                                        if (msg.Length > 0)
                                        {
                                            try
                                            {
                                                recdata.messageprefix = msg[0];
                                                recdata.flightnum = msg[1];
                                                recdata.fltday = msg[2].Substring(0, 2);
                                                recdata.fltmonth = msg[2].Substring(2, 3);
                                                recdata.flttime = msg[2].Substring(5);
                                                recdata.airportcode = msg[3];
                                                recdata.infocode = msg[4];//Discrepency Code
                                                string[] arr = stringsplitter(msg[5]);
                                                if (arr.Length > 1)
                                                {
                                                    try
                                                    {
                                                        recdata.pcsindicator = arr[0];
                                                        recdata.numofpcs = arr[1];
                                                        recdata.weightcode = arr[2];
                                                        recdata.weight = arr[3].Length > 0 ? arr[3] : "";
                                                    }
                                                    catch (Exception ec) { }
                                                }
                                                if (msg.Length > 6)
                                                {
                                                    recdata.seccarriercode = msg[6].Length > 0 ? msg[6] : "";
                                                }
                                                if (msg.Length > 7)
                                                {
                                                    recdata.name = msg[7].Length > 0 ? msg[7] : "";
                                                }
                                            }
                                            catch (Exception ex) { }
                                            Array.Resize(ref fsanodes, fsanodes.Length + 1);
                                            fsanodes[fsanodes.Length - 1] = recdata;
                                        }
                                        #endregion
                                    } break;
                                case "NFD":
                                case "AWD":
                                case "CCD":
                                case "DLV":
                                case "DDL":
                                case "TGC":
                                    {
                                        #region NFD
                                        if (msg.Length > 0)
                                        {
                                            try
                                            {
                                                recdata.messageprefix = msg[0];
                                                recdata.fltday = msg[1].Substring(0, 2);
                                                recdata.fltmonth = msg[1].Substring(2, 3);
                                                recdata.flttime = msg[1].Substring(5);
                                                recdata.airportcode = msg[2];
                                                string[] arr = stringsplitter(msg[3]);
                                                if (arr.Length > 1)
                                                {
                                                    try
                                                    {
                                                        recdata.pcsindicator = arr[0];
                                                        recdata.numofpcs = arr[1];
                                                        recdata.weightcode = arr[2];
                                                        recdata.weight = arr[3].Length > 0 ? arr[3] : "";
                                                    }
                                                    catch (Exception ec) { }
                                                }
                                                if (msg.Length > 4)
                                                {
                                                    recdata.name = msg[4].Length > 0 ? msg[4] : "";
                                                }
                                            }
                                            catch (Exception ex) { }
                                            Array.Resize(ref fsanodes, fsanodes.Length + 1);
                                            fsanodes[fsanodes.Length - 1] = recdata;
                                        }
                                        #endregion
                                    } break;
                                case "RCF":
                                case "MAN":
                                case "ARR":
                                case "AWR":
                                case "DEP":
                                case "PRE":
                                    {
                                        #region RCF/MAN/DEP/PRE
                                        if (msg.Length > 0)
                                        {
                                            try
                                            {
                                                //0
                                                recdata.messageprefix = msg[0];
                                                //1
                                                recdata.carriercode = msg[1].Substring(0, 2);
                                                recdata.flightnum = msg[1].Substring(2);
                                                //2
                                                string[] split = msg[2].Split('-');
                                                recdata.fltday = split[0].Substring(0, 2);
                                                recdata.fltmonth = split[0].Substring(2, 3);
                                                recdata.flttime = split[0].Substring(5);
                                                if (split.Length > 1)
                                                {
                                                    recdata.daychangeindicator = split[1] + ",";
                                                }
                                               //3
                                                if (msg[3].Length > 0)
                                                {
                                                    try
                                                    {
                                                        if (msg[3].Length > 3)
                                                        {
                                                            recdata.fltorg = msg[3].Substring(0, 3);
                                                            recdata.fltdest = msg[3].Substring(3);
                                                        }
                                                        else
                                                        {
                                                            recdata.airportcode = msg[3];
                                                        }
                                                    }
                                                    catch (Exception ex) { }
                                                }
                                                //4 PCS Indicator
                                                if(msg[4].Length>0)
                                                {
                                                    try
                                                    {
                                                        string[] arr = stringsplitter(msg[4]);
                                                        if (arr!=null && arr.Length > 0)
                                                        {
                                                            recdata.pcsindicator = arr[0];
                                                            recdata.numofpcs = arr[1];
                                                            recdata.weightcode = arr[2];
                                                            recdata.weight = arr[3];

                                                        }

                                                    }
                                                    catch (Exception ex) { }
                                                }
                                                try
                                                {
                                                    if (msg.Length > 5)
                                                    {
                                                        try
                                                        {
                                                            if (msg[5].Contains('-'))
                                                            {
                                                                string[] strarr = msg[5].Split('-');
                                                                recdata.timeindicator = recdata.timeindicator + strarr[0].Substring(0, 1) + ",";
                                                                recdata.depttime = strarr[0].Substring(1);
                                                                if (strarr.Length > 1)
                                                                {
                                                                    recdata.daychangeindicator = recdata.daychangeindicator + strarr[1] + ",";
                                                                }
                                                            }
                                                            else 
                                                            {
                                                                recdata.timeindicator = recdata.timeindicator + msg[5].Substring(0,1) + ",";
                                                                recdata.depttime = msg[5].Substring(1);
                                                            }
                                                        }
                                                        catch (Exception ex) { }
                                                    }
                                                    if (msg.Length > 6)
                                                    {
                                                        try
                                                        {
                                                            if (msg[6].Contains('-'))
                                                            {
                                                                string[] strarr = msg[6].Split('-');
                                                                recdata.timeindicator = recdata.timeindicator + strarr[0].Substring(0, 1) + ",";
                                                                recdata.arrivaltime = strarr[0].Substring(1);
                                                                if (strarr.Length > 1)
                                                                {
                                                                    recdata.daychangeindicator = recdata.daychangeindicator + strarr[1] + ",";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                recdata.timeindicator = recdata.timeindicator + msg[6].Substring(0, 1) + ",";
                                                                recdata.arrivaltime = msg[6].Substring(1);
                                                            }
                                                        }
                                                        catch (Exception ex) { }
                                                    }
                                                }
                                                catch (Exception ex) { }

                                            }
                                            catch (Exception ex) { }
                                            Array.Resize(ref fsanodes, fsanodes.Length + 1);
                                            fsanodes[fsanodes.Length - 1] = recdata;
                                        }
                                        #endregion
                                    }
                                    break;
                                case "BKD":
                                    {
                                        #region BKD
                                        if (msg.Length > 0)
                                        {
                                            try
                                            {
                                                recdata.messageprefix = msg[0];
                                                recdata.carriercode = msg[1];
                                                string[] split = msg[2].Split('-');
                                                recdata.fltday = split[0].Substring(0, 2);
                                                recdata.fltmonth = split[0].Substring(2, 3);
                                                recdata.flttime = split[0].Substring(5);
                                                recdata.daychangeindicator = split[1] + ",";
                                                recdata.fltdest = msg[3].Substring(0, 3);
                                                recdata.fltorg = msg[3].Substring(3);
                                                string[] arr = stringsplitter(msg[4]);
                                                if (arr.Length > 0)
                                                {
                                                    recdata.pcsindicator = arr[0];
                                                    recdata.numofpcs = arr[1];
                                                    recdata.weightcode = arr[2];
                                                    recdata.weight = arr[3];
                                                }
                                                if (msg.Length > 5)
                                                {
                                                    string[] strarr = msg[5].Split('-');
                                                    recdata.timeindicator = recdata.timeindicator + strarr[0].Substring(0, 1) + ",";
                                                    recdata.depttime = strarr[0].Substring(1);
                                                    recdata.daychangeindicator = recdata.daychangeindicator + strarr[1] + ",";
                                                }
                                                if (msg.Length > 6)
                                                {
                                                    string[] strarr = msg[6].Split('-');
                                                    recdata.timeindicator = recdata.timeindicator + strarr[0].Substring(0, 1) + ",";
                                                    recdata.arrivaltime = strarr[0].Substring(1);
                                                    recdata.daychangeindicator = recdata.daychangeindicator + strarr[1] + ",";
                                                }
                                                if (msg.Length > 7)
                                                {
                                                    string[] strarr = stringsplitter(msg[7]);
                                                    if (strarr.Length > 0)
                                                    {
                                                        recdata.volumecode = strarr[0];
                                                        recdata.volumeamt = strarr[1];
                                                        recdata.densityindicator = "DG";
                                                        recdata.densitygroup = strarr[3].Length > 0 ? strarr[3] : "";
                                                    }
                                                }

                                            }
                                            catch (Exception ex) { }
                                            Array.Resize(ref fsanodes, fsanodes.Length + 1);
                                            fsanodes[fsanodes.Length - 1] = recdata;
                                        }
                                        #endregion

                                    } break;
                                case "TRM":
                                    {
                                        #region TRM
                                        if (msg.Length > 1)
                                        {
                                            try
                                            {
                                                recdata.messageprefix = msg[0];
                                                recdata.carriercode = msg[1];
                                                recdata.airportcode = msg[2];
                                                string[] arr = stringsplitter(msg[3]);
                                                if (arr.Length > 0)
                                                {
                                                    recdata.pcsindicator = arr[0];
                                                    recdata.numofpcs = arr[1];
                                                    recdata.weightcode = arr[2];
                                                    recdata.weight = arr[3];
                                                }
                                            }
                                            catch (Exception ex) { }
                                            Array.Resize(ref fsanodes, fsanodes.Length + 1);
                                            fsanodes[fsanodes.Length - 1] = recdata;
                                        }
                                        #endregion
                                    } break;
                                case "TFD":
                                    {
                                        #region TFD
                                        if (msg.Length > 0)
                                        {
                                            try
                                            {
                                                recdata.messageprefix = msg[0];
                                                recdata.carriercode = msg[1];
                                                recdata.fltday = msg[2].Substring(0, 2);
                                                recdata.fltmonth = msg[2].Substring(2, 3);
                                                recdata.flttime = msg[2].Substring(5);
                                                recdata.airportcode = msg[3];
                                                string[] arr = stringsplitter(msg[4]);
                                                if (arr.Length > 0)
                                                {
                                                    try
                                                    {
                                                        recdata.pcsindicator = arr[0];
                                                        recdata.numofpcs = arr[1];
                                                        recdata.weightcode = arr[2];
                                                        recdata.weight = arr[3].Length > 0 ? arr[3] : "";
                                                    }
                                                    catch (Exception ec) { }
                                                }
                                                if (msg.Length > 5)
                                                {
                                                    recdata.transfermanifestnumber = msg[5].Length > 0 ? msg[5] : "";
                                                }
                                                if (msg.Length > 6)
                                                {
                                                    recdata.seccarriercode = msg[6].Length > 0 ? msg[6] : "";
                                                }
                                                if (msg.Length > 7)
                                                {
                                                    recdata.name = msg[7].Length > 0 ? msg[7] : "";
                                                }
                                            }
                                            catch (Exception ex) { }
                                            Array.Resize(ref fsanodes, fsanodes.Length + 1);
                                            fsanodes[fsanodes.Length - 1] = recdata;
                                        }
                                        #endregion
                                    } break;

                                case "CRC":
                                    {
                                        #region CRC
                                        if (msg.Length > 0)
                                        {
                                            try
                                            {
                                                recdata.messageprefix = msg[0];
                                                recdata.fltday = msg[1].Substring(0, 2);
                                                recdata.fltmonth = msg[1].Substring(2, 3);
                                                recdata.flttime = msg[1].Substring(5);
                                                recdata.airportcode = msg[2];
                                                string[] arr = stringsplitter(msg[3]);
                                                if (arr.Length > 0)
                                                {
                                                    recdata.pcsindicator = arr[0];
                                                    recdata.numofpcs = arr[1];
                                                    recdata.weightcode = arr[2];
                                                    recdata.weight = arr[3];
                                                }
                                                if (msg.Length > 4)
                                                {
                                                    recdata.carriercode = msg[4].Substring(0, 2);
                                                    recdata.flightnum = msg[4].Substring(2);
                                                }
                                                if (msg.Length > 5)
                                                {
                                                    recdata.fltday = msg[5].Substring(0, 2);
                                                    recdata.fltmonth = msg[5].Substring(2);
                                                }
                                                if (msg.Length > 6)
                                                {
                                                    recdata.fltdest = msg[6].Substring(0, 3);
                                                    recdata.fltorg = msg[6].Substring(3);
                                                }

                                            }
                                            catch (Exception ex) { }
                                            Array.Resize(ref fsanodes, fsanodes.Length + 1);
                                            fsanodes[fsanodes.Length - 1] = recdata;
                                        }
                                        #endregion
                                    } break;

                                case "OCI":
                                    {
                                        #region OCI
                                        if (msg.Length > 0)
                                        {
                                            lastrec = "OCI";
                                            MessageData.customsextrainfo custom = new MessageData.customsextrainfo("");
                                            try
                                            {
                                                custom.IsoCountryCodeOci = msg[1];
                                                custom.InformationIdentifierOci = msg[2];
                                                custom.CsrIdentifierOci = msg[3];
                                                custom.SupplementaryCsrIdentifierOci = msg[4];
                                                custom.consigref = awbref;
                                            }
                                            catch (Exception ex) { }
                                            Array.Resize(ref custominfo, custominfo.Length + 1);
                                            custominfo[custominfo.Length - 1] = custom;
                                        }
                                        #endregion
                                    } break;
                                case "ULD":
                                    {
                                        #region ULD
                                        try
                                        {
                                            int uldnum = 0;
                                            if (msg.Length > 1)
                                            {
                                                if (int.Parse(msg[1]) > 0)
                                                {
                                                    Array.Resize(ref uld, str.Length);
                                                    for (int k = 1; k < msg.Length; k++)
                                                    {
                                                        string[] splitstr = msg[k].Split('-');
                                                        uld[uldnum].uldtype = splitstr[0].Substring(0, 3);
                                                        uld[uldnum].uldsrno = splitstr[0].Substring(3, splitstr[0].Length - 6);
                                                        uld[uldnum].uldowner = splitstr[0].Substring(splitstr[0].Length - 3, 3);
                                                        uld[uldnum].uldloadingindicator = splitstr[1];
                                                        uldnum++;
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception e4)
                                        { }
                                        #endregion

                                    } break;
                                case "OSI":
                                    {
                                        #region OSI
                                        try
                                        {
                                            lastrec = msg[0];
                                            if (msg[1].Length > 0)
                                            {
                                                Array.Resize(ref othinfoarray, othinfoarray.Length + 1);
                                                othinfoarray[othinfoarray.Length - 1].otherserviceinfo1 = msg[1];

                                            }
                                        }
                                        catch (Exception e6) { }
                                        #endregion

                                    } break;
                                default:
                                    {
                                        #region Other Info
                                        if (str[i].StartsWith("/"))
                                        {
                                            string[] msgdata = str[i].Split('/');
                                            try
                                            {
                                                #region OSI 2
                                                if (lastrec == "OSI")
                                                {
                                                    othinfoarray[othinfoarray.Length - 1].otherserviceinfo2 = msgdata[1].Length > 0 ? msgdata[1] : "";
                                                    lastrec = "NA";
                                                }
                                                #endregion

                                                #region OCI
                                                if (lastrec == "OCI")
                                                {

                                                    if (msgdata.Length > 0)
                                                    {
                                                        lastrec = "OCI";
                                                        MessageData.customsextrainfo custom = new MessageData.customsextrainfo("");
                                                        custom.IsoCountryCodeOci = msgdata[1];
                                                        custom.InformationIdentifierOci = msgdata[2];
                                                        custom.CsrIdentifierOci = msgdata[3];
                                                        custom.SupplementaryCsrIdentifierOci = msgdata[4];
                                                        Array.Resize(ref custominfo, custominfo.Length + 1);
                                                        custominfo[custominfo.Length - 1] = custom;
                                                    }
                                                }
                                                #endregion
                                            }
                                            catch (Exception e13)
                                            { }
                                        }
                                        #endregion
                                    } break;
                            }
                        }
                        #endregion
                    }
                }
            }
        }
        catch (Exception ex)
        {
            flag = false;
            //MessageBox.Show("ErrMessage:" + ex.Message);
        }
        return flag;
    }
    #endregion

    #region StringSplitter
    public static string[] stringsplitter(string str)
    {
        char[] arr = str.ToCharArray();
        string[] strarr = new string[arr.Length];

        try
        {
            if (str.Length > 0)
            {
                int k = 0;
                char lastchr = 'A';
                for (int j = 0; j < arr.Length; j++)
                {
                    if ((char.IsNumber(arr[j])) || (arr[j].Equals('.')))
                    {//number                            
                        if (lastchr == 'N')
                            k--;
                        strarr[k] = strarr[k] + arr[j].ToString();
                        lastchr = 'N';
                    }
                    if (char.IsLetter(arr[j]))
                    {//letter
                        if (lastchr == 'L')
                            k--;
                        strarr[k] = strarr[k] + arr[j].ToString();
                        lastchr = 'L';
                    }
                    k++;
                }
            }
        }
        catch (Exception ex)
        {
            strarr = null;
        }
        return strarr;
    }
    #endregion

    #region EncodeFSA
    public static string EncodeFSAforSend(ref MessageData.FSAInfo fsadata, ref MessageData.CommonStruct[] fsanodes, ref MessageData.customsextrainfo[] custominfo, ref MessageData.ULDinfo[] uld, ref MessageData.otherserviceinfo[] othinfoarray)
    {
        string FSAStr = null;
        try
        {
            #region Line 1
            string line1 = "FSA/" + fsadata.fsaversion;
            #endregion

            #region line 2 consigment detials
            string line2 = "";
            line2 = line2 + fsadata.airlineprefix + "-" + fsadata.awbnum + fsadata.origin + fsadata.dest + "/" + fsadata.consigntype + fsadata.pcscnt + fsadata.weightcode + fsadata.weight + (fsadata.totalpcscnt.Length > 0 ? "T" + fsadata.totalpcscnt : "");
            #endregion

            #region Line 3 Encode message
            string line3 = "", str = "";
            if (fsanodes.Length > 0)
            {
                for (int i = 0; i < fsanodes.Length; i++)
                {
                    switch (fsanodes[i].messageprefix.Trim())
                    {
                        case "FOH":
                        case "RCS":
                            {
                                #region RCS
                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + fsanodes[i].flttime + "/" + fsanodes[i].airportcode + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + (fsanodes[i].name.Length > 0 ? "/" + fsanodes[i].name : "") + "/" + fsanodes[i].volumecode + fsanodes[i].volumeamt + fsanodes[i].daychangeindicator + fsanodes[i].densitygroup;
                                #endregion
                            }
                            break;
                        case "RCT":
                        case "DIS":
                            {
                                #region RCT
                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].carriercode + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + fsanodes[i].flttime + "/" + fsanodes[i].airportcode + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + "/" + (fsanodes[i].name.Length > 0 ? fsanodes[i].name : "");
                                #endregion
                            }
                            break;
                        case "NFD":
                        case "AWD":
                        case "CCD":
                        case "DLV":
                        case "DDL":
                        case "TGC":
                            {
                                #region RCT
                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + fsanodes[i].flttime + "/" + fsanodes[i].airportcode + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + "/" + (fsanodes[i].name.Length > 0 ? fsanodes[i].name : "");
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
                                string[] daychange = new string[0];
                                string[] timechange = new string[0];
                                if (fsanodes[i].daychangeindicator.Length > 0)
                                {
                                    daychange = fsanodes[i].daychangeindicator.Split(',');
                                }

                                if (fsanodes[i].timeindicator.Length > 0)
                                {
                                    timechange = fsanodes[i].timeindicator.Split(',');
                                }

                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].carriercode + fsanodes[i].flightnum + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + fsanodes[i].flttime + (daychange.Length > 0 ? "-" + daychange[0] : "") + "/" + fsanodes[i].airportcode + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + (timechange.Length > 0 ? "/" + timechange[0] : "") + fsanodes[i].depttime + (daychange.Length > 1 ? "-" + daychange[1] : "") + (timechange.Length > 1 ? "/" + timechange[1] : "") + fsanodes[i].arrivaltime + (daychange.Length > 2 ? "-" + daychange[2] : "");
                                #endregion
                            }
                            break;
                        case "BKD":
                            {
                                #region BKD
                                string[] daychange = new string[0];
                                string[] timechange = new string[0];
                                if (fsanodes[i].daychangeindicator.Length > 0)
                                {
                                    daychange = fsanodes[i].daychangeindicator.Split(',');
                                }

                                if (fsanodes[i].timeindicator.Length > 0)
                                {
                                    timechange = fsanodes[i].timeindicator.Split(',');
                                }

                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].carriercode + fsanodes[i].flightnum + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + "/" + fsanodes[i].fltdest + fsanodes[i].fltorg + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + (timechange.Length > 0 ? "/" + timechange[0] : "") + fsanodes[i].depttime + (daychange.Length > 1 ? "-" + daychange[1] : "") + (timechange.Length > 1 ? "/" + timechange[1] : "") + fsanodes[i].arrivaltime + (daychange.Length > 2 ? "-" + daychange[2] : "") + "/" + fsanodes[i].volumecode + fsanodes[i].volumeamt + fsanodes[i].daychangeindicator + fsanodes[i].densitygroup;

                                #endregion

                            } break;
                        case "TRM":
                            {
                                #region TRM
                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].fltdest + fsanodes[i].fltorg + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight;
                                #endregion
                            } break;
                        case "TFD":
                            {
                                #region TFD
                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].carriercode + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + fsanodes[i].flttime + "/" + fsanodes[i].airportcode + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + "/" + (fsanodes[i].transfermanifestnumber.Length > 0 ? fsanodes[i].transfermanifestnumber : "") + "/" + fsanodes[i].carriercode + "/" + fsanodes[i].name;
                                #endregion
                            } break;

                        case "CRC":
                            {
                                #region CRC
                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + fsanodes[i].flttime + "/" + fsanodes[i].airportcode + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + "/" + fsanodes[i].carriercode + fsanodes[i].flightnum + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + "/" + fsanodes[i].fltdest + fsanodes[i].fltorg;
                                #endregion
                            } break;

                    }
                    line3 = line3.Trim('/') + "$";
                }
                line3 = line3.Trim('$');
                line3 = line3.Replace("$", "\r\n");
            }
            #endregion

            #region Line 4 OCI
            string line4 = "";
            if (custominfo.Length > 0)
            {
                for (int i = 0; i < custominfo.Length; i++)
                {
                    line4 = "/" + custominfo[i].IsoCountryCodeOci + "/" + custominfo[i].InformationIdentifierOci + "/" + custominfo[i].CsrIdentifierOci + "/" + custominfo[i].SupplementaryCsrIdentifierOci + "$";
                }
                line4 = "OCI" + line4.Trim('$');
                line4 = line4.Replace("$", "\r\n");
            }
            #endregion

            #region Line 5 ULD
            string line5 = "";
            if (uld.Length > 0)
            {
                line5 = "ULD";
                for (int i = 0; i < uld.Length; i++)
                {

                    line5 = line5.Trim() + "/" + uld[i].uldtype + uld[i].uldsrno + uld[i].uldowner + (uld[i].uldloadingindicator.Length > 0 ? ("-" + uld[i].uldloadingindicator) : "");
                }
            }
            #endregion

            #region Line 6 OSI
            string line6 = "";
            if (othinfoarray.Length > 0)
            {
                for (int i = 0; i < othinfoarray.Length; i++)
                {
                    if (othinfoarray[i].otherserviceinfo1.Length > 0)
                    {
                        line6 = "OSI/" + othinfoarray[i].otherserviceinfo1 + "$";
                        if (othinfoarray[i].otherserviceinfo2.Length > 0)
                        {
                            line6 = line6 + "/" + othinfoarray[i].otherserviceinfo2 + "$";
                        }
                    }
                }
                line6 = line6.Trim('$');
                line6 = line6.Replace("$", "\r\n");
            }
            #endregion

            #region Build FSA
            FSAStr = FSAStr + line1.Trim('/') + "\r\n" + line2.Trim('/') + "\r\n" + line3.Trim('/');
            if (line4.Length > 0)
            {
                FSAStr = FSAStr + "\r\n" + line4.Trim('/');
            }
            if (line5.Length > 0)
            {
                FSAStr = FSAStr + "\r\n" + line5.Trim('/');
            }
            if (line6.Length > 0)
            {
                FSAStr = FSAStr + "\r\n" + line6.Trim('/');
            }
            #endregion
        }
        catch (Exception ex)
        {
            FSAStr = "ERR";
        }
        return FSAStr;
    }
    #endregion

    #region EncodeFSU
    public static string EncodeFSUforSend(ref MessageData.FSAInfo fsadata, ref MessageData.CommonStruct[] fsanodes, ref MessageData.customsextrainfo[] custominfo, ref MessageData.ULDinfo[] uld, ref MessageData.otherserviceinfo[] othinfoarray)
    {
        string FSAStr = null;
        try
        {
            #region Line 1
            string line1 = "FSU/" + fsadata.fsaversion;
            #endregion

            #region line 2 consigment detials
            string line2 = "";
            line2 = line2 + fsadata.airlineprefix + "-" + fsadata.awbnum + fsadata.origin + fsadata.dest + "/" + fsadata.consigntype + fsadata.pcscnt + fsadata.weightcode + fsadata.weight + (fsadata.totalpcscnt.Length > 0 ? "T" + fsadata.totalpcscnt : "");
            #endregion

            #region Line 3 Encode message
            string line3 = "", str = "";
            if (fsanodes.Length > 0)
            {
                for (int i = 0; i < fsanodes.Length; i++)
                {
                    switch (fsanodes[i].messageprefix.Trim())
                    {
                        case "FOH":
                        case "RCS":
                            {
                                #region RCS
                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + fsanodes[i].flttime + "/" + fsanodes[i].airportcode + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + (fsanodes[i].name.Length > 0 ? "/" + fsanodes[i].name : "") + "/" + fsanodes[i].volumecode + fsanodes[i].volumeamt + fsanodes[i].daychangeindicator + fsanodes[i].densitygroup;
                                #endregion
                            }
                            break;
                        case "RCT":
                            {
                                #region RCT
                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].carriercode + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + fsanodes[i].flttime + "/" + fsanodes[i].airportcode + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + "/" + (fsanodes[i].name.Length > 0 ? fsanodes[i].name : "");
                                #endregion
                            }
                            break;
                        case "DIS":
                            {
                                #region DIS
                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].carriercode + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + fsanodes[i].flttime + "/" + fsanodes[i].airportcode +"/"+fsanodes[i].infocode+ "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + "/" + (fsanodes[i].name.Length > 0 ? fsanodes[i].name : "");
                                #endregion
                            }
                            break;
                        case "NFD":
                        case "AWD":
                        case "CCD":
                        case "DLV":
                        case "DDL":
                        case "TGC":
                            {
                                #region RCT
                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + fsanodes[i].flttime + "/" + fsanodes[i].airportcode + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + "/" + (fsanodes[i].name.Length > 0 ? fsanodes[i].name : "");
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
                                string[] daychange = new string[0];
                                string[] timechange = new string[0];
                                if (fsanodes[i].daychangeindicator.Length > 0)
                                {
                                    daychange = fsanodes[i].daychangeindicator.Split(',');
                                }

                                if (fsanodes[i].timeindicator.Length > 0)
                                {
                                    timechange = fsanodes[i].timeindicator.Split(',');
                                }

                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].carriercode + fsanodes[i].flightnum + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + fsanodes[i].flttime + (daychange.Length > 0 ? "-" + daychange[0] : "") + "/" + fsanodes[i].airportcode + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + (timechange.Length > 0 ? "/" + timechange[0] : "") + fsanodes[i].depttime + (daychange.Length > 1 ? "-" + daychange[1] : "") + (timechange.Length > 1 ? "/" + timechange[1] : "") + fsanodes[i].arrivaltime + (daychange.Length > 2 ? "-" + daychange[2] : "");
                                #endregion
                            }
                            break;
                        case "BKD":
                            {
                                #region BKD
                                string[] daychange = new string[0];
                                string[] timechange = new string[0];
                                if (fsanodes[i].daychangeindicator.Length > 0)
                                {
                                    daychange = fsanodes[i].daychangeindicator.Split(',');
                                }

                                if (fsanodes[i].timeindicator.Length > 0)
                                {
                                    timechange = fsanodes[i].timeindicator.Split(',');
                                }

                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].carriercode + fsanodes[i].flightnum + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + "/" + fsanodes[i].fltdest + fsanodes[i].fltorg + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + (timechange.Length > 0 ? "/" + timechange[0] : "") + fsanodes[i].depttime + (daychange.Length > 1 ? "-" + daychange[1] : "") + (timechange.Length > 1 ? "/" + timechange[1] : "") + fsanodes[i].arrivaltime + (daychange.Length > 2 ? "-" + daychange[2] : "") + "/" + fsanodes[i].volumecode + fsanodes[i].volumeamt + fsanodes[i].daychangeindicator + fsanodes[i].densitygroup;

                                #endregion

                            } break;
                        case "TRM":
                            {
                                #region TRM
                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].fltdest + fsanodes[i].fltorg + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight;
                                #endregion
                            } break;
                        case "TFD":
                            {
                                #region TFD
                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].carriercode + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + fsanodes[i].flttime + "/" + fsanodes[i].airportcode + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + "/" + (fsanodes[i].transfermanifestnumber.Length > 0 ? fsanodes[i].transfermanifestnumber : "") + "/" + fsanodes[i].carriercode + "/" + fsanodes[i].name;
                                #endregion
                            } break;

                        case "CRC":
                            {
                                #region CRC
                                line3 = line3 + fsanodes[i].messageprefix + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + fsanodes[i].flttime + "/" + fsanodes[i].airportcode + "/" + fsanodes[i].pcsindicator + fsanodes[i].numofpcs + fsanodes[i].weightcode + fsanodes[i].weight + "/" + fsanodes[i].carriercode + fsanodes[i].flightnum + "/" + fsanodes[i].fltday + fsanodes[i].fltmonth + "/" + fsanodes[i].fltdest + fsanodes[i].fltorg;
                                #endregion
                            } break;

                    }
                    line3 = line3.Trim('/') + "$";
                }
                line3 = line3.Trim('$');
                line3 = line3.Replace("$", "\r\n");
            }
            #endregion

            #region Line 4 OCI
            string line4 = "";
            if (custominfo.Length > 0)
            {
                for (int i = 0; i < custominfo.Length; i++)
                {
                    line4 = "/" + custominfo[i].IsoCountryCodeOci + "/" + custominfo[i].InformationIdentifierOci + "/" + custominfo[i].CsrIdentifierOci + "/" + custominfo[i].SupplementaryCsrIdentifierOci + "$";
                }
                line4 = "OCI" + line4.Trim('$');
                line4 = line4.Replace("$", "\r\n");
            }
            #endregion

            #region Line 5 ULD
            string line5 = "";
            if (uld.Length > 0)
            {
                line5 = "ULD";
                for (int i = 0; i < uld.Length; i++)
                {

                    line5 = line5.Trim() + "/" + uld[i].uldtype + uld[i].uldsrno + uld[i].uldowner + (uld[i].uldloadingindicator.Length > 0 ? ("-" + uld[i].uldloadingindicator) : "");
                }
            }
            #endregion

            #region Line 6 OSI
            string line6 = "";
            if (othinfoarray.Length > 0)
            {
                for (int i = 0; i < othinfoarray.Length; i++)
                {
                    if (othinfoarray[i].otherserviceinfo1.Length > 0)
                    {
                        line6 = "OSI/" + othinfoarray[i].otherserviceinfo1 + "$";
                        if (othinfoarray[i].otherserviceinfo2.Length > 0)
                        {
                            line6 = line6 + "/" + othinfoarray[i].otherserviceinfo2 + "$";
                        }
                    }
                }
                line6 = line6.Trim('$');
                line6 = line6.Replace("$", "\r\n");
            }
            #endregion

            #region Build FSA
            FSAStr = FSAStr + line1.Trim('/') + "\r\n" + line2.Trim('/') + "\r\n" + line3.Trim('/');
            if (line4.Length > 0)
            {
                FSAStr = FSAStr + "\r\n" + line4.Trim('/');
            }
            if (line5.Length > 0)
            {
                FSAStr = FSAStr + "\r\n" + line5.Trim('/');
            }
            if (line6.Length > 0)
            {
                FSAStr = FSAStr + "\r\n" + line6.Trim('/');
            }
            #endregion
        }
        catch (Exception ex)
        {
           WriteLog("ERR in FSU Encode:"+ex.Message);
        }
        return FSAStr;
    }
    #endregion

    //FWB

    #region Encode FWB OLD
    //public static string EncodeFWBForSendOLD(ref MessageData.fwbinfo fwbData, ref MessageData.othercharges[] fwbOtherCharge, ref MessageData.otherserviceinfo othData, ref MessageData.RateDescription[] fwbrate)
    //{
    //    string FWBStr = null;
    //    try
    //    {
    //        //FWB
    //        #region Line 1
    //        FWBStr = "FWB/" + fwbData.fwbversionnum;
    //        FWBStr += "\r\n";
    //        #endregion

    //        #region Line 2
    //        FWBStr += fwbData.airlineprefix + "-" + fwbData.awbnum + "" + fwbData.origin + "" + fwbData.dest + "/T" + fwbData.pcscnt + "" + fwbData.weightcode + "" + fwbData.weight + "";
    //        FWBStr += fwbData.volumecode + "" + fwbData.volumeamt + "" + fwbData.densityindicator + "" + fwbData.densitygrp + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //FLT
    //        #region Line 3
    //        FWBStr += "FLT/" + fwbData.carriercode + "" + fwbData.fltnum + "/" + fwbData.fltday.PadLeft(2, '0') + "/" + fwbData.carriercode + "" + fwbData.fltnum + "/" + fwbData.fltday.PadLeft(2, '0') + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //RTG
    //        #region Line 4
    //        FWBStr += "RTG/" + fwbData.origin + "" + fwbData.carriercode + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //SHP
    //        #region Line 5
    //        FWBStr += "SHP/" + fwbData.shipperaccnum + "" + fwbData.shippername + "/" + fwbData.shipperadd + "/" +
    //               fwbData.shipperplace + "/" + fwbData.shipperstate + "/" + fwbData.shippercountrycode + "/" +
    //               fwbData.shipperpostcode + "/" + fwbData.shippercontactidentifier + "/" + fwbData.shippercontactnum + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //CNE
    //        #region Line 6
    //        FWBStr += "CNE/" + fwbData.consaccnum + "/" + fwbData.consname + "/" + fwbData.consadd + "/" + fwbData.consplace + "/" + fwbData.consstate + "/" +
    //               fwbData.conscountrycode + "/" + fwbData.conspostcode + "/" + fwbData.conscontactidentifier + "/" + fwbData.conscontactnum + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //AGT
    //        #region Line 7
    //        FWBStr += "AGT/" + fwbData.agentaccnum + "/" + fwbData.agentIATAnumber + "/" + fwbData.agentCASSaddress + "/" +
    //               fwbData.agentParticipentIdentifier + "/" + fwbData.agentname + "/" + fwbData.agentplace + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //SSR
    //        #region Line 8
    //        FWBStr += "SSR/" + fwbData.specialservicereq1 + "/" + fwbData.specialservicereq2 + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //NFY
    //        #region Line 9
    //        FWBStr += "NFY/" + fwbData.notifyname + "/" + fwbData.notifyadd + "/" + fwbData.notifyplace + "/" +
    //               fwbData.notifystate + "/" + fwbData.notifycountrycode + "/" + fwbData.notifypostcode + "/" +
    //               fwbData.notifycontactidentifier + "/" + fwbData.notifycontactnum + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //ACC
    //        #region Line 10
    //        FWBStr += "ACC/" + fwbData.accountinginfoidentifier + "/" + fwbData.accountinginfo + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //CVD
    //        #region Line 11
    //        FWBStr += "CVD/" + fwbData.currency + "/" + fwbData.chargecode + "/PP/" + fwbData.declaredvalue + "/" + fwbData.declaredcustomvalue + "/" + fwbData.insuranceamount + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //RTD
    //        #region Line 12     Pending
    //        FWBStr += "RTD/1/P" + fwbData.pcscnt + "/K" + fwbData.weight + "/C";
    //        FWBStr += "\r\n";

    //        #region Line 12-1
    //        #endregion

    //        #region Line 12-2
    //        #endregion

    //        #region Line 12-3
    //        #endregion

    //        #region Line 12-4
    //        #endregion

    //        #region Line 12-5
    //        #endregion

    //        #region Line 12-6
    //        #endregion

    //        #region Line 12-7
    //        #endregion

    //        #region Line 12-8
    //        #endregion
    //        #endregion

    //        //OTH
    //        #region Line 13
    //        FWBStr += "OTH/P/";
    //        for (int i = 0; i < othData.Length; i++)
    //        {
    //            if (i > 0)
    //            {
    //                if (i % 3 == 0)
    //                {
    //                    if (i != othData.Length)
    //                    {
    //                        FWBStr += "\r\nP";
    //                    }
    //                }
    //            }
    //            FWBStr += othData[i].otherchargecode + "" + othData[i].entitlementcode + "" + othData[i].chargeamt;
    //            //if (i % 3 == 0)
    //            //{
    //            //    if(i != othData.Length)
    //            //    {
    //            //        FWBStr += "\r\nP";
    //            //    }
    //            //}
    //        }
    //        FWBStr += "\r\n";
    //        #endregion

    //        //PPD
    //        #region Line 14
    //        FWBStr += "PPD/WT" + fwbData.PPweightCharge + "/VC" + fwbData.PPValuationCharge + "/TX" + fwbData.PPTaxesCharge + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //COL
    //        #region Line 15
    //        FWBStr += "COL/WT" + fwbData.CCweightCharge + "/VC" + fwbData.CCValuationCharge + "/TX" + fwbData.CCTaxesCharge + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //CER
    //        #region Line 16
    //        FWBStr += "CER/" + fwbData.shippersignature + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //ISU
    //        #region Line 17
    //        FWBStr += "ISU/" + fwbData.carrierdate.PadLeft(2, '0') + "" + fwbData.carriermonth.PadLeft(2, '0') + "" + fwbData.carrieryear.PadLeft(2, '0') + "/" + fwbData.carrierplace + "/" + fwbData.carriersignature + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //OSI
    //        #region Line 18
    //        FWBStr += "OSI/" + otherServInfo.otherserviceinfo1 + "/" + otherServInfo.otherserviceinfo2 + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //CDC
    //        #region Line 19     Pending
    //        FWBStr += "CDC/";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //REF
    //        #region Line 20
    //        FWBStr += "REF/" + fwbData.senderairport + "" + fwbData.senderofficedesignator + "" + fwbData.sendercompanydesignator + "/" + fwbData.senderFileref + "/" +
    //            fwbData.senderParticipentIdentifier + "/" + fwbData.senderParticipentCode + "/" + fwbData.senderPariticipentAirport + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //COR
    //        #region Line 21
    //        FWBStr += "COR/" + fwbData.customorigincode + "";
    //        #endregion

    //        //COI
    //        #region Line 22
    //        FWBStr += "COI/" + fwbData.commisioncassindicator + "/" + fwbData.commisionCassSettleAmt + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //SII
    //        #region Line 23
    //        FWBStr += "SII/" + fwbData.saleschargeamt + "/" + fwbData.salescassindicator + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //ARD
    //        #region Line 24
    //        FWBStr += "ARD/" + fwbData.agentfileref + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //SPH
    //        #region Line 25     Pending
    //        FWBStr += "SPH/";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //NOM
    //        #region Line 26     Pending
    //        FWBStr += "NOM/";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //SRI
    //        #region Line 27     Pending
    //        FWBStr += "SRI/";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //OPI
    //        #region Line 28
    //        FWBStr += "OPI/" + fwbData.othparticipentname + "/" + fwbData.othparticipentairport + "/" +
    //            fwbData.othofficedesignator + "" + fwbData.othcompanydesignator + "/" + fwbData.othfilereference + "/" +
    //            fwbData.othparticipentidentifier + "/" + fwbData.othparticipentcode + "/" + fwbData.othparticipentairport + "";
    //        FWBStr += "\r\n";
    //        #endregion

    //        //OCI
    //        #region Line 29     Pending
    //        FWBStr += "OCI/";
    //        FWBStr += "\r\n";
    //        #endregion

    //    }
    //    catch (Exception ex)
    //    {
    //        FWBStr = "ERR";
    //    }
    //    return FWBStr;
    //}
    #endregion

    #region Decode FWB Message
    public static bool decodeReceiveFWB(string fwbmsg,ref MessageData.fwbinfo fwbdata, ref MessageData.othercharges[] fwbOtherCharge, ref MessageData.otherserviceinfo[] othinfoarray, ref MessageData.RateDescription[] fwbrate,ref MessageData.customsextrainfo[] custominfo,ref MessageData.dimensionnfo[] objDimension)
    {
        bool flag = false;
        try
        {
            string lastrec = "NA";
            int line = 0;//, consignmnetnum = 0; 
            //int count=0;
            try
            {
                if (fwbmsg.StartsWith("FWB", StringComparison.OrdinalIgnoreCase))
                {
                    // ffrmsg = ffrmsg.Replace("\r\n","$");
                    string[] str = fwbmsg.Split('$');
                    if (str.Length > 3)
                    {
                        for (int i = 0; i < str.Length; i++)
                        {

                            flag = true;

                            #region Line 1
                            if (str[i].StartsWith("FWB", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                fwbdata.fwbversionnum = msg[1];
                            }
                            #endregion

                            #region Line 2 awb consigment details
                            if (i == 1)
                            {
                                try
                                {
                                    lastrec = "AWB";
                                    line = 0;
                                    string[] msg = str[i].Split('/');
                                    //0th element
                                    string[] decmes = msg[0].Split('-');
                                    fwbdata.airlineprefix = decmes[0];
                                    fwbdata.awbnum = decmes[1].Substring(0, decmes[1].Length - 6);
                                    fwbdata.origin = decmes[1].Substring(decmes[1].Length - 6, 3);
                                    fwbdata.dest = decmes[1].Substring(decmes[1].Length - 3, 3);
                                    //1
                                    if (msg[1].Length > 0)
                                    {
                                        int k = 0;
                                        char lastchr = 'A';
                                        char[] arr = msg[1].ToCharArray();
                                        string[] strarr = new string[arr.Length];
                                        for (int j = 0; j < arr.Length; j++)
                                        {
                                            if ((char.IsNumber(arr[j])) || (arr[j].Equals('.')))
                                            {//number                            
                                                if (lastchr == 'N')
                                                    k--;
                                                strarr[k] = strarr[k] + arr[j].ToString();
                                                lastchr = 'N';
                                            }
                                            if (char.IsLetter(arr[j]))
                                            {//letter
                                                if (lastchr == 'L')
                                                    k--;
                                                strarr[k] = strarr[k] + arr[j].ToString();
                                                lastchr = 'L';
                                            }
                                            k++;
                                        }
                                        fwbdata.consigntype = strarr[0];
                                        fwbdata.pcscnt = strarr[1];//int.Parse(strarr[1]);
                                        fwbdata.weightcode = strarr[2];
                                        fwbdata.weight = strarr[3];//float.Parse(strarr[3]);
                                        for (k = 4; k < strarr.Length; k += 2)
                                        {
                                            if (strarr[k] != null)
                                            {
                                                if (strarr[k] == "DG")
                                                {
                                                    fwbdata.densityindicator = strarr[k];
                                                    fwbdata.densitygrp = strarr[k];
                                                }
                                                else//if (strarr[k + 1].Length > 3)
                                                {
                                                    fwbdata.volumecode = strarr[k];
                                                    fwbdata.volumeamt = strarr[k + 1];
                                                }
                                            }
                                        }
                                    }


                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            #region Line 3 Flight Booking
                            if (str[i].StartsWith("FLT", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 1)
                                    {
                                        fwbdata.carriercode = msg[1].Substring(0, 2);
                                        fwbdata.fltnum = msg[1].Substring(2);
                                        fwbdata.fltday = msg[2];
                                        if (msg.Length > 2)
                                        {
                                            fwbdata.carriercode = fwbdata.carriercode + "," + msg[3].Substring(0, 2);
                                            fwbdata.fltnum = fwbdata.fltnum + "," + msg[3].Substring(2);
                                            fwbdata.fltday = fwbdata.fltday + "," + msg[4];
                                        }
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 4 Routing
                            if (str[i].StartsWith("RTG", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string airportcity = "", carrier = "";
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 1)
                                    {
                                        for (int k = 1; k < msg.Length; k++)
                                        {
                                            airportcity = airportcity + msg[k].Substring(0, 3) + ",";
                                            carrier = carrier + msg[k].Substring(3) + ",";
                                        }
                                        fwbdata.routingairportcitycode = airportcity.Trim(',');
                                        fwbdata.routingcarriercode = carrier.Trim(',');
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 5 Shipper Infor
                            if (str[i].StartsWith("SHP", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg.Length > 1)
                                    {
                                        fwbdata.shipperaccnum = msg[1];

                                    }
                                }
                                catch (Exception e10)
                                { }
                            }
                            #endregion

                            #region Line 6 Consignee
                            if (str[i].StartsWith("CNE", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg.Length > 1)
                                    {
                                        fwbdata.consaccnum = msg[1];
                                    }
                                }
                                catch (Exception e10)
                                { }
                            }
                            #endregion

                            #region Line 7 Agent
                            if (str[i].StartsWith("AGT", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg.Length > 1)
                                    {
                                        fwbdata.agentaccnum = msg[1];
                                        fwbdata.agentIATAnumber = msg[2].Length > 0 ? msg[2] : "";
                                        if (msg.Length > 2)
                                        {
                                            fwbdata.agentCASSaddress = msg[3].Length > 0 ? msg[3] : "";
                                        }
                                        if (msg.Length > 3)
                                        {
                                            fwbdata.agentParticipentIdentifier = msg[4].Length > 0 ? msg[4] : "";
                                        }
                                    }
                                }
                                catch (Exception e10)
                                { }
                            }
                            #endregion

                            #region Line 8 Special Service request
                            if (str[i].StartsWith("SSR", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg[1].Length > 0)
                                    {
                                        fwbdata.specialservicereq1 = msg[1];
                                    }

                                }
                                catch (Exception e5)
                                { }
                            }
                            #endregion

                            #region Line 9 Notify
                            if (str[i].StartsWith("NFY", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg.Length > 0)
                                    {
                                        fwbdata.notifyname = msg[1].Length > 0 ? msg[1] : "";
                                    }
                                }
                                catch (Exception e10)
                                { }
                            }
                            #endregion

                            #region Line 10 Accounting Information
                            if (str[i].StartsWith("ACC", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                lastrec = msg[0];
                                line = 0;
                                if (msg.Length > 1)
                                {
                                    fwbdata.accountinginfoidentifier = fwbdata.accountinginfoidentifier + msg[1] + ",";
                                    fwbdata.accountinginfo = fwbdata.accountinginfo + msg[2] + ",";
                                }
                            }
                            #endregion

                            #region Line 11 Charge declaration
                            if (str[i].StartsWith("CVD", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 1)
                                    {
                                        fwbdata.currency = msg[1];
                                        fwbdata.chargecode = msg[2].Length > 0 ? msg[2] : "";
                                        fwbdata.chargedec = msg[3].Length > 0 ? msg[3] : "";
                                        fwbdata.declaredvalue = msg[4];
                                        fwbdata.declaredcustomvalue = msg[5];
                                        fwbdata.insuranceamount = msg[6];
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 12 Rate Description
                            if (str[i].StartsWith("RTD", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 1)
                                {
                                    lastrec = msg[0];
                                    line = 0;
                                    MessageData.RateDescription rate = new MessageData.RateDescription("");
                                    try
                                    {
                                        rate.linenum = msg[1];
                                        for (int k = 2; k < msg.Length; k++)
                                        {
                                            if (msg[k].Substring(0, 1).Equals("P", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.pcsidentifier = msg[k].Substring(0, 1);
                                                rate.numofpcs = msg[k].Substring(1);
                                            }
                                            if (msg[k].Substring(0, 1).Equals("K", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.weightindicator = msg[k].Substring(0, 1);
                                                rate.weight = msg[k].Substring(1).Length > 0 ? msg[k].Substring(1) : "0";
                                            }
                                            if (msg[k].Substring(0, 1).Equals("C", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.rateclasscode = msg[k].Substring(1);
                                            }
                                            if (msg[k].Substring(0, 1).Equals("S", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.commoditynumber = msg[k].Substring(1);
                                            }
                                            if (msg[k].Substring(0, 1).Equals("W", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.awbweight = msg[k].Substring(1);
                                            }
                                            if (msg[k].Substring(0, 1).Equals("R", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.chargerate = msg[k].Substring(1);
                                            }
                                            if (msg[k].Substring(0, 1).Equals("T", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.chargeamt = msg[k].Substring(1);
                                            }
                                        }
                                        Array.Resize(ref fwbrate, fwbrate.Length + 1);
                                        fwbrate[fwbrate.Length - 1] = rate;
                                    }
                                    catch (Exception ex) { }
                                }
                            }
                            #endregion

                            #region Line 13 Other Charges
                            if (str[i].StartsWith("OTH", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    lastrec = "OTH";
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 1)
                                    {
                                        string[] opstr = stringsplitter(msg[2]);
                                        for (int k = 0; k < opstr.Length; k = k + 2)
                                        {
                                            if (opstr[k].Length > 0)
                                            {
                                                MessageData.othercharges oth = new MessageData.othercharges("");
                                                oth.otherchargecode = opstr[k].Substring(0, 2);
                                                oth.entitlementcode = opstr[k].Substring(2);
                                                oth.chargeamt = opstr[k + 1];
                                                Array.Resize(ref fwbOtherCharge, fwbOtherCharge.Length + 1);
                                                fwbOtherCharge[fwbOtherCharge.Length - 1] = oth;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 14 Prepaid Charge Summery
                            if (str[i].StartsWith("PPD", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    lastrec = "PPD";
                                    string[] msg = str[i].Split('/');
                                    for (int k = 1; k < msg.Length; k++)
                                    {
                                        if (msg[k].Substring(0, 2).Equals("WT", StringComparison.OrdinalIgnoreCase))
                                        {
                                            fwbdata.PPweightCharge = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("VC", StringComparison.OrdinalIgnoreCase))
                                        {
                                            fwbdata.PPValuationCharge = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("TX", StringComparison.OrdinalIgnoreCase))
                                        {
                                            fwbdata.PPTaxesCharge = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("OA", StringComparison.OrdinalIgnoreCase))
                                        {
                                            fwbdata.PPOCDA = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("OC", StringComparison.OrdinalIgnoreCase))
                                        {
                                            fwbdata.PPOCDC = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("CT", StringComparison.OrdinalIgnoreCase))
                                        {
                                            fwbdata.PPTotalCharges = msg[k].Substring(2);
                                        }
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 15 Collect Charge Summery
                            if (str[i].StartsWith("COL", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    lastrec = "COL";
                                    string[] msg = str[i].Split('/');
                                    for (int k = 1; k < msg.Length; k++)
                                    {
                                        if (msg[k].Substring(0, 2).Equals("WT", StringComparison.OrdinalIgnoreCase))
                                        {
                                            fwbdata.CCweightCharge = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("VC", StringComparison.OrdinalIgnoreCase))
                                        {
                                            fwbdata.CCValuationCharge = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("TX", StringComparison.OrdinalIgnoreCase))
                                        {
                                            fwbdata.CCTaxesCharge = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("OA", StringComparison.OrdinalIgnoreCase))
                                        {
                                            fwbdata.CCOCDA = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("OC", StringComparison.OrdinalIgnoreCase))
                                        {
                                            fwbdata.CCOCDC = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("CT", StringComparison.OrdinalIgnoreCase))
                                        {
                                            fwbdata.CCTotalCharges = msg[k].Substring(2);
                                        }
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 16 Shipper Certification
                            if (str[i].StartsWith("CER", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                if (msg[1].Length > 0)
                                {
                                    fwbdata.shippersignature = msg[1];
                                }
                            }
                            #endregion

                            #region Line 17 Carrier Execution
                            if (str[i].StartsWith("ISU", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                try
                                {
                                    if (msg.Length > 0)
                                    {

                                        fwbdata.carrierdate = msg[1].Substring(0, 2);
                                        fwbdata.carriermonth = msg[1].Substring(2, 3);
                                        fwbdata.carrieryear = msg[1].Substring(5);
                                        fwbdata.carrierplace = msg[2];
                                        fwbdata.carriersignature = msg[3];
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 18 Other service info
                            if (str[i].StartsWith("OSI", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg[1].Length > 0)
                                    {
                                        Array.Resize(ref othinfoarray, othinfoarray.Length + 1);
                                        othinfoarray[othinfoarray.Length - 1].otherserviceinfo1 = msg[1];

                                    }

                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 19 Charge in destination currency
                            if (str[i].StartsWith("CDC", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 0)
                                    {
                                        fwbdata.cccurrencycode = msg[1].Substring(0, 3);
                                        fwbdata.ccexchangerate = msg[1].Substring(3);
                                        for (int j = 2; j < msg.Length; j++)
                                            fwbdata.ccchargeamt += msg[j] + ",";
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 20 Sender Reference
                            if (str[i].StartsWith("REF", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 0)
                                    {

                                        if (msg[1].Length > 1)
                                        {
                                            try
                                            {
                                                fwbdata.senderairport = msg[1].Substring(0, 3);
                                                fwbdata.senderofficedesignator = msg[1].Substring(3, 2);
                                                fwbdata.sendercompanydesignator = msg[1].Substring(5);
                                            }
                                            catch (Exception ex) { }
                                        }
                                        fwbdata.senderFileref = msg[2];
                                        fwbdata.senderParticipentIdentifier = msg[3];
                                        fwbdata.senderParticipentCode = msg[4];
                                        fwbdata.senderPariticipentAirport = msg[5];
                                    }

                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 21 Custom Origin
                            if (str[i].StartsWith("COR", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg[1].Length > 0)
                                    {
                                        fwbdata.customorigincode = msg[1];
                                    }

                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 22 Commission Information
                            if (str[i].StartsWith("COI", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 0)
                                    {
                                        fwbdata.commisioncassindicator = msg[1];
                                        for (int k = 2; k < msg.Length; k++)
                                            fwbdata.commisionCassSettleAmt += msg[k] + ",";
                                    }

                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 23 Sales Incentive Info
                            if (str[i].StartsWith("SII", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg[1].Length > 0)
                                    {
                                        fwbdata.saleschargeamt = msg[1];
                                        fwbdata.salescassindicator = msg[2];
                                    }

                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 24 Agent Reference
                            if (str[i].StartsWith("ARD", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg[1].Length > 0)
                                    {
                                        fwbdata.agentfileref = msg[1];
                                    }

                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 25 Special Handling
                            if (str[i].StartsWith("SPH", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg[1].Length > 0)
                                    {
                                        string temp = str[i].Replace("/", "");
                                        fwbdata.splhandling = temp.Replace("SPH", "");
                                    }

                                }
                                catch (Exception e5)
                                { }
                            }
                            #endregion

                            #region Line 26 Nominated Handling Party
                            if (str[i].StartsWith("NOM", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 0)
                                    {
                                        fwbdata.handlingname = msg[1];
                                        fwbdata.handlingplace = msg[2];
                                    }

                                }
                                catch (Exception e5)
                                { }
                            }
                            #endregion

                            #region Line 27 Shipment Reference Info
                            if (str[i].StartsWith("SRI", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 0)
                                    {
                                        fwbdata.shiprefnum = msg[1];
                                        fwbdata.supplemetryshipperinfo1 = msg[2];
                                        fwbdata.supplemetryshipperinfo2 = msg[3];
                                    }
                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 28 Other Service Information
                            if (str[i].StartsWith("OPI", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 0)
                                    {
                                        lastrec = msg[0];
                                        fwbdata.othparticipentname = msg[1];
                                    }

                                }
                                catch (Exception e5)
                                { }
                            }
                            #endregion

                            #region Line 29 custom extra info
                            if (str[i].StartsWith("OCI", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 0)
                                {
                                    lastrec = "OCI";
                                    MessageData.customsextrainfo custom = new MessageData.customsextrainfo("");
                                    custom.IsoCountryCodeOci = msg[1];
                                    custom.InformationIdentifierOci = msg[2];
                                    custom.CsrIdentifierOci = msg[3];
                                    custom.SupplementaryCsrIdentifierOci = msg[4];
                                    Array.Resize(ref custominfo, custominfo.Length + 1);
                                    custominfo[custominfo.Length - 1] = custom;
                                }
                            }
                            #endregion

                            #region Second Line
                            if (str[i].StartsWith("/"))
                            {
                                string[] msg = str[i].Split('/');
                                try
                                {
                                    #region SHP Data
                                    if (lastrec == "SHP")
                                    {
                                        line++;
                                        if (line == 1)
                                        {
                                            fwbdata.shippername = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                        if (line == 2)
                                        {
                                            fwbdata.shipperadd = msg[1].Length > 0 ? msg[1] : "";

                                        }
                                        if (line == 3)
                                        {
                                            fwbdata.shipperplace = msg[1].Length > 0 ? msg[1] : "";
                                            fwbdata.shipperstate = msg[2].Length > 0 ? msg[2] : "";
                                        }
                                        if (line == 4)
                                        {
                                            fwbdata.shippercountrycode = msg[1].Length > 0 ? msg[1] : "";
                                            fwbdata.shipperpostcode = msg[2].Length > 0 ? msg[2] : "";
                                            fwbdata.shippercontactidentifier = msg[3].Length > 0 ? msg[3] : "";
                                            fwbdata.shippercontactnum = msg[4].Length > 0 ? msg[4] : "";

                                        }

                                    }
                                    #endregion

                                    #region CNE Data
                                    if (lastrec == "CNE")
                                    {
                                        line++;
                                        if (line == 1)
                                        {
                                            fwbdata.consname = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                        if (line == 2)
                                        {
                                            fwbdata.consadd = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                        if (line == 3)
                                        {
                                            fwbdata.consplace = msg[1].Length > 0 ? msg[1] : "";
                                            fwbdata.consstate = msg[2].Length > 0 ? msg[2] : "";
                                        }
                                        if (line == 4)
                                        {
                                            fwbdata.conscountrycode = msg[1].Length > 0 ? msg[1] : "";
                                            fwbdata.conspostcode = msg[2].Length > 0 ? msg[2] : "";
                                            fwbdata.conscontactidentifier = msg[3].Length > 0 ? msg[3] : "";
                                            fwbdata.conscontactnum = msg[4].Length > 0 ? msg[4] : "";
                                        }

                                    }
                                    #endregion

                                    #region AgentData
                                    if (lastrec == "AGT")
                                    {
                                        line++;
                                        if (line == 1)
                                        {
                                            fwbdata.agentname = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                        if (line == 2)
                                        {
                                            fwbdata.agentplace = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                    }
                                    #endregion

                                    #region SSR 2
                                    if (lastrec == "SSR")
                                    {
                                        fwbdata.specialservicereq2 = msg[1].Length > 0 ? msg[1] : "";
                                        lastrec = "NA";
                                    }
                                    #endregion

                                    #region Also notify Data
                                    if (lastrec == "NFY")
                                    {
                                        line++;
                                        if (line == 1)
                                        {
                                            fwbdata.notifyadd = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                        if (line == 2)
                                        {
                                            fwbdata.notifyplace = msg[1].Length > 0 ? msg[1] : "";
                                            fwbdata.notifystate = msg[2].Length > 0 ? msg[2] : "";
                                        }
                                        if (line == 3)
                                        {
                                            fwbdata.notifycountrycode = msg[1].Length > 0 ? msg[1] : "";
                                            fwbdata.notifypostcode = msg[2].Length > 0 ? msg[2] : "";
                                            fwbdata.notifycontactidentifier = msg[3].Length > 0 ? msg[3] : "";
                                            fwbdata.notifycontactnum = msg[4].Length > 0 ? msg[4] : "";
                                        }
                                    }
                                    #endregion

                                    #region Account Info
                                    if (lastrec.Equals("ACC", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (msg.Length > 1)
                                        {
                                            fwbdata.accountinginfoidentifier = fwbdata.accountinginfoidentifier + msg[1] + ",";
                                            fwbdata.accountinginfo = fwbdata.accountinginfo + msg[2] + ",";
                                        }
                                    }
                                    #endregion

                                    #region RateData
                                    if (lastrec.Equals("RTD", StringComparison.OrdinalIgnoreCase))
                                    {
                                        try
                                        {
                                            if (msg.Length > 1)
                                            {
                                                int res, k = 1;
                                                if (int.TryParse(msg[k].ToString(), out res))
                                                {
                                                    k++;
                                                }
                                                if (msg[k].Equals("NG", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbrate[fwbrate.Length - 1].goodsnature = msg[k + 1];
                                                }
                                                if (msg[k].Equals("NC", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbrate[fwbrate.Length - 1].goodsnature1 = msg[k + 1];
                                                }
                                                if (msg[k].Equals("ND", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    MessageData.dimensionnfo dimension = new MessageData.dimensionnfo("");
                                                    if (msg.Length > 1) 
                                                    {
                                                        if (msg[k + 1].Substring(0, 1).Equals("K", StringComparison.OrdinalIgnoreCase) || msg[k + 1].Substring(0, 1).Equals("L", StringComparison.OrdinalIgnoreCase)) 
                                                        {
                                                            dimension.weight = msg[k + 1].Substring(1);
                                                            k++;
                                                        }
                                                        if (msg[k + 1].Contains('-')) 
                                                        {
                                                            string[] substr = msg[k + 1].Split('-');
                                                            try
                                                            {
                                                                if (substr.Length > 0)
                                                                {
                                                                    dimension.mesurunitcode = substr[0].Substring(0, 3);
                                                                    dimension.length = substr[0].Substring(3);
                                                                    dimension.width = substr[1];
                                                                    dimension.height = substr[2];
                                                                    k++;
                                                                }
                                                            }
                                                            catch (Exception ez) { }
                                                        }
                                                        int val;
                                                        if (int.TryParse(msg[k + 1], out val)) 
                                                        {
                                                            dimension.piecenum = msg[k + 1];
                                                        }
                                                        Array.Resize(ref objDimension, objDimension.Length + 1);
                                                        objDimension[objDimension.Length - 1] = dimension;
                                                    }
                                                   
                                                }
                                                if (msg[k].Equals("NV", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbrate[fwbrate.Length - 1].volcode = msg[k + 1].Substring(0, 2);
                                                    fwbrate[fwbrate.Length - 1].volamt = msg[k + 1].Substring(2);
                                                }
                                                if (msg[k].Equals("NU", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbrate[fwbrate.Length - 1].uldtype = msg[k + 1].Substring(0, 3);
                                                    fwbrate[fwbrate.Length - 1].uldserialnum = msg[k + 1].Substring(3, msg[k + 1].Length - 5);
                                                    fwbrate[fwbrate.Length - 1].uldowner = msg[k + 1].Substring(msg[k + 1].Length - 2);
                                                }
                                                if (msg[k].Equals("NS", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbrate[fwbrate.Length - 1].slac = msg[k + 1];
                                                }
                                                if (msg[k].Equals("NH", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbrate[fwbrate.Length - 1].hermonisedcomoditycode = msg[k + 1];
                                                }
                                                if (msg[k].Equals("NO", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbrate[fwbrate.Length - 1].isocountrycode = msg[k + 1];
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        { }
                                    }
                                    #endregion

                                    #region Other Charges
                                    if (lastrec.Equals("OTH", StringComparison.OrdinalIgnoreCase))
                                    {
                                        try
                                        {
                                            string[] opstr = stringsplitter(msg[2]);
                                            for (int k = 0; k < opstr.Length; k = k + 2)
                                            {
                                                MessageData.othercharges oth = new MessageData.othercharges("");
                                                oth.otherchargecode = opstr[k].Substring(0, 2);
                                                oth.entitlementcode = opstr[k].Substring(2);
                                                oth.chargeamt = opstr[k + 1];
                                                Array.Resize(ref fwbOtherCharge, fwbOtherCharge.Length + 1);
                                                fwbOtherCharge[fwbOtherCharge.Length - 1] = oth;
                                            }
                                        }
                                        catch (Exception ex)
                                        { }
                                    }
                                    #endregion

                                    #region Line 14 Collect Charge Summery
                                    if (lastrec.Equals("PPD", StringComparison.OrdinalIgnoreCase))
                                    {
                                        try
                                        {
                                            for (int k = 1; k < msg.Length; k++)
                                            {
                                                if (msg[k].Substring(0, 2).Equals("WT", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbdata.PPweightCharge = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("VC", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbdata.PPValuationCharge = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("TX", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbdata.PPTaxesCharge = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("OA", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbdata.PPOCDA = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("OC", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbdata.PPOCDC = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("CT", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbdata.PPTotalCharges = msg[k].Substring(2);
                                                }
                                            }
                                        }
                                        catch (Exception ex) { }
                                    }
                                    #endregion

                                    #region Line 15 Prepaid Charge Summery
                                    if (lastrec.Equals("COL", StringComparison.OrdinalIgnoreCase))
                                    {
                                        try
                                        {
                                            for (int k = 1; k < msg.Length; k++)
                                            {
                                                if (msg[k].Substring(0, 2).Equals("WT", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbdata.CCweightCharge = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("VC", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbdata.CCValuationCharge = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("TX", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbdata.CCTaxesCharge = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("OA", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbdata.CCOCDA = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("OC", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbdata.CCOCDC = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("CT", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    fwbdata.CCTotalCharges = msg[k].Substring(2);
                                                }
                                            }
                                        }
                                        catch (Exception ex) { }
                                    }
                                    #endregion

                                    #region Line 18 OSI 2
                                    if (lastrec == "OSI")
                                    {
                                        othinfoarray[othinfoarray.Length - 1].otherserviceinfo2 = msg[1].Length > 0 ? msg[1] : "";
                                        lastrec = "NA";
                                    }
                                    #endregion


                                    #region OCI
                                    if (lastrec.Equals("OCI", StringComparison.OrdinalIgnoreCase))
                                    {
                                        string[] msgdata = str[i].Split('/');
                                        if (msgdata.Length > 0)
                                        {
                                            MessageData.customsextrainfo custom = new MessageData.customsextrainfo("");
                                            custom.IsoCountryCodeOci = msgdata[1];
                                            custom.InformationIdentifierOci = msgdata[2];
                                            custom.CsrIdentifierOci = msgdata[3];
                                            custom.SupplementaryCsrIdentifierOci = msgdata[4];
                                            Array.Resize(ref custominfo, custominfo.Length + 1);
                                            custominfo[custominfo.Length - 1] = custom;
                                        }
                                    }
                                    #endregion

                                    #region OPI
                                    if (lastrec.Equals("OPI", StringComparison.OrdinalIgnoreCase))
                                    {
                                        string[] msgdata = str[i].Split('/');
                                        if (msgdata.Length > 0)
                                        {
                                            fwbdata.othairport = msgdata[1].Substring(0, 3);
                                            fwbdata.othofficedesignator = msgdata[1].Substring(3, 2);
                                            fwbdata.othcompanydesignator = msgdata[1].Substring(5);
                                            fwbdata.othfilereference = msgdata[2];
                                            fwbdata.othparticipentidentifier = msgdata[3];
                                            fwbdata.othparticipentcode = msgdata[4];
                                            fwbdata.othparticipentairport = msgdata[5];
                                        }
                                    }
                                    #endregion
                                }
                                catch (Exception e13)
                                { }
                            }
                            #endregion
                        }
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }
    #endregion

    #region EncodeFWBForSend
    public static string EncodeFWBForSend(ref MessageData.fwbinfo fwbdata, ref MessageData.othercharges[] fwbOtherCharge, ref MessageData.otherserviceinfo[] othinfoarray, ref MessageData.RateDescription[] fwbrate,ref MessageData.customsextrainfo[] custominfo)
    {
        string fwbstr = null;
        try
        {
            //FWB
            #region Line 1
            string line1 = "FWB/" + fwbdata.fwbversionnum;
            #endregion

            #region Line 2
            string line2 = fwbdata.airlineprefix + "-" + fwbdata.awbnum + fwbdata.origin + fwbdata.dest + "/" + fwbdata.consigntype + fwbdata.pcscnt + fwbdata.weightcode + fwbdata.weight + fwbdata.volumecode + fwbdata.volumeamt + fwbdata.densityindicator + fwbdata.densitygrp;
            #endregion line 2
            //FLT
            #region Line 3
            string line3 = "";
            if (fwbdata.carriercode.Trim(',').Contains(','))
            {
                string[] carriersplit = fwbdata.carriercode.Split(',');
                string[] fltsplit = fwbdata.fltnum.Split(',');
                string[] daysplit = fwbdata.fltday.Split(',');
                for (int k = 0; k < carriersplit.Length; k++)
                {
                    line3 = line3 + carriersplit[k] + fltsplit[k] + "/" + daysplit[k] + "/";
                }
            }
            else
            {
                line3 = fwbdata.carriercode.Trim(',') + fwbdata.fltnum.Trim(',') + "/" + fwbdata.fltday.Trim(',');
            }
            if (line3.Length > 1)
            {
                line3 = "FLT/" + line3.Trim('/');
            }
            #endregion

            //RTG
            #region Line 4
            string line4 = fwbdata.dest + fwbdata.carriercode;
            if (line4.Length > 1)
            {
                line4 = "RTG/" + line4;
            }
            #endregion

            //SHP
            #region Line 5
            string line5 = "";
            string str1 = "", str2 = "", str3 = "", str4 = "";
            try
            {
                if (fwbdata.shippername.Length > 0)
                {
                    str1 = "/" + fwbdata.shippername;
                }
                if (fwbdata.shipperadd.Length > 0)
                {
                    str2 = "/" + fwbdata.shipperadd;
                }

                if (fwbdata.shipperplace.Length > 0 || fwbdata.shipperstate.Length > 0)
                {
                    str3 = "/" + fwbdata.shipperplace + "/" + fwbdata.shipperstate;
                }
                if (fwbdata.shippercountrycode.Length > 0 || fwbdata.shipperpostcode.Length > 0 || fwbdata.shippercontactidentifier.Length > 0 || fwbdata.shippercontactnum.Length > 0)
                {
                    str4 = "/" + fwbdata.shippercountrycode + "/" + fwbdata.shipperpostcode + "/" + fwbdata.shippercontactidentifier + "/" + fwbdata.shippercontactnum;
                }

                if (fwbdata.shipperaccnum.Length > 0 || str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line5 = "SHP/" + fwbdata.shipperaccnum;
                    
                    if (str4.Length > 0)
                    {
                        line5 = line5.Trim('/') + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/') + "\r\n/" + str4.Trim('/');
                    }
                    else if (str3.Length > 0)
                    {
                        line5 = line5.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line5 = line5.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line5 = line5.Trim() + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            //CNE
            #region Line 6
            string line6 = "";
            str1 = "";
            str2 = "";
            str3 = "";
            str4 = "";
            try
            {
                if (fwbdata.consname.Length > 0)
                {
                    str1 = "/" + fwbdata.consname;
                }
                if (fwbdata.consadd.Length > 0)
                {
                    str2 = "/" + fwbdata.consadd;
                }

                if (fwbdata.consplace.Length > 0 || fwbdata.consstate.Length > 0)
                {
                    str3 = "/" + fwbdata.consplace + "/" + fwbdata.consstate;
                }
                if (fwbdata.conscountrycode.Length > 0 || fwbdata.conspostcode.Length > 0 || fwbdata.conscontactidentifier.Length > 0 || fwbdata.conscontactnum.Length > 0)
                {
                    str4 = "/" + fwbdata.conscountrycode + "/" + fwbdata.conspostcode + "/" + fwbdata.conscontactidentifier + "/" + fwbdata.conscontactnum;
                }

                if (fwbdata.consaccnum.Length > 0 || str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line6 = "CNE/" + fwbdata.consaccnum;
                    if (str4.Length > 0)
                    {
                        line6 = line6.Trim('/') + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/') + "\r\n/" + str4.Trim('/');
                    }
                    else if (str3.Length > 0)
                    {
                        line6 = line6.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line6 = line6.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line6 = line6.Trim() + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            //AGT
            #region Line 7
            string line7 = "";
            str1 = "";
            str2 = "";
            try
            {
                if (fwbdata.agentname.Length > 0)
                {
                    str1 = "/" + fwbdata.agentname;
                }
                if (fwbdata.agentplace.Length > 0)
                {
                    str2 = "/" + fwbdata.agentplace;
                }
                if (fwbdata.agentaccnum.Length > 0 || fwbdata.agentIATAnumber.Length > 0 || fwbdata.agentCASSaddress.Length > 0 || fwbdata.agentParticipentIdentifier.Length > 0 || str1.Length > 0 || str2.Length > 0)
                {
                    line7 = "AGT/" + fwbdata.agentaccnum + "/" + fwbdata.agentIATAnumber + "/" + fwbdata.agentCASSaddress + "/" + fwbdata.agentParticipentIdentifier;
                    if (str2.Length > 0)
                    {
                        line7 = line7.Trim('/') + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line7 = line7.Trim('/') + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            //SSR
            #region Line 8
            string line8 = "";
            if (fwbdata.specialservicereq1.Length > 0 || fwbdata.specialservicereq2.Length > 0)
            {
                line8 = "SSR/" + fwbdata.specialservicereq1 + "$" + fwbdata.specialservicereq2;
            }
            line8 = line8.Trim('$');
            line8 = line8.Replace("$", "\r\n");
            #endregion

            //NFY
            #region Line 9
            string line9 = "";
            str1 = str2 = str3 = str4 = "";
            try
            {
                if (fwbdata.notifyadd.Length > 0)
                {
                    str1 = "/" + fwbdata.notifyadd;
                }
                if (fwbdata.notifyplace.Length > 0 || fwbdata.notifystate.Length > 0)
                {
                    str2 = "/" + fwbdata.notifyplace + "/" + fwbdata.notifystate;
                }
                if (fwbdata.notifycountrycode.Length > 0 || fwbdata.notifypostcode.Length > 0 || fwbdata.notifycontactidentifier.Length > 0 || fwbdata.notifycontactnum.Length > 0)
                {
                    str3 = "/" + fwbdata.notifycountrycode + "/" + fwbdata.notifypostcode + "/" + fwbdata.notifycontactidentifier + "/" + fwbdata.notifycontactnum;
                }

                if (fwbdata.notifyname.Length > 0 || str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line9 = "NFY/" + fwbdata.shipperaccnum;
                    if (str3.Length > 0)
                    {
                        line9 = line9.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line9 = line9.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line9 = line9.Trim() + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            //ACC
            #region Line 10
            string line10 = "";
            if (fwbdata.accountinginfoidentifier.Length > 0 || fwbdata.accountinginfo.Length > 0)
            {
                line10 = "ACC/" + fwbdata.accountinginfoidentifier + "/" + fwbdata.accountinginfo + "";
            }
            #endregion

            //CVD
            #region Line 11
            string line11 = "";
            line11 = "CVD/" + fwbdata.currency + "/" + fwbdata.chargecode + "/PP/" + fwbdata.declaredvalue + "/" + fwbdata.declaredcustomvalue + "/" + fwbdata.insuranceamount + "";
            #endregion

            //RTD
            #region Line 12
            string line12 = buildRateNode(ref fwbrate);
            if (line12 == null)
            {
                return null;
            }
            #endregion

            //OTH
            #region Line 13
            string line13 = "";
            for (int i = 0; i < fwbOtherCharge.Length; i++)
            {
                if (i > 0)
                {
                    if (i % 3 == 0)
                    {
                        if (i != fwbOtherCharge.Length)
                        {
                            line13 += "\r\n/" + fwbOtherCharge[0].indicator+"/";
                        }
                    }
                }
                line13 += fwbOtherCharge[i].otherchargecode + "" + fwbOtherCharge[i].entitlementcode + "" + fwbOtherCharge[i].chargeamt;
                //if (i % 3 == 0)
                //{
                //    if(i != othData.Length)
                //    {
                //        FWBStr += "\r\nP";
                //    }
                //}
            }
            if (line13.Length > 1)
            {
                line13 = "OTH/P/" + line13;
            }
            #endregion

            //PPD
            #region Line 14
            string line14 = "", subline14 = "";

            if (fwbdata.PPweightCharge.Length > 0)
            {
                line14 = line14 + "/WT" + fwbdata.PPweightCharge;
            }
            if (fwbdata.PPValuationCharge.Length > 0)
            {
                line14 = line14 + "/VC" + fwbdata.PPValuationCharge;
            }
            if (fwbdata.PPTaxesCharge.Length > 0)
            {
                line14 = line14 + "/TX" + fwbdata.PPTaxesCharge;
            }
            if (fwbdata.PPOCDA.Length > 0)
            {
                subline14 = subline14 + "/OA" + fwbdata.PPOCDA;
            }
            if (fwbdata.PPOCDC.Length > 0)
            {
                subline14 = subline14 + "/OC" + fwbdata.PPOCDC;
            }
            if (fwbdata.PPTotalCharges.Length > 0)
            {
                subline14 = subline14 + "/CT" + fwbdata.PPTotalCharges;
            }
            if (line14.Length > 0 || subline14.Length > 0)
            {
                line14 = "PPD" + line14 + "$" + subline14;
            }
            line14 = line14.Trim('$');
            line14 = line14.Replace("$", "\r\n");
            #endregion

            //COL
            #region Line 15
            string line15 = "", subline15 = "";
            if (fwbdata.CCweightCharge.Length > 0)
            {
                line15 = line15 + "/WT" + fwbdata.CCweightCharge;
            }
            if (fwbdata.CCValuationCharge.Length > 0)
            {
                line15 = line15 + "/VC" + fwbdata.CCValuationCharge;
            }
            if (fwbdata.CCTaxesCharge.Length > 0)
            {
                line15 = line15 + "/TX" + fwbdata.CCTaxesCharge;
            }
            if (fwbdata.CCOCDA.Length > 0)
            {
                subline15 = subline15 + "/OA" + fwbdata.CCOCDA;
            }
            if (fwbdata.CCOCDC.Length > 0)
            {
                subline15 = subline15 + "/OC" + fwbdata.CCOCDC;
            }
            if (fwbdata.CCTotalCharges.Length > 0)
            {
                subline15 = subline15 + "/CT" + fwbdata.CCTotalCharges;
            }
            if (line15.Length > 0 || subline15.Length > 0)
            {
                line15 = "COL" + line15 + "$" + subline15;
            }
            line15 = line15.Trim('$');
            line15 = line15.Replace("$", "\r\n");
            #endregion

            //CER
            #region Line 16
            string line16 = "";
            if (fwbdata.shippersignature.Length > 0)
            {
                line16 = "CER/" + fwbdata.shippersignature;
            }
            #endregion

            //ISU
            #region Line 17
            string line17 = "";
            line17 = "ISU/" + fwbdata.carrierdate.PadLeft(2, '0') + fwbdata.carriermonth.PadLeft(2, '0') + fwbdata.carrieryear.PadLeft(2, '0') + "/" + fwbdata.carrierplace + "/" + fwbdata.carriersignature;
            #endregion

            //OSI
            #region Line 18
            string line18 = "";
            if (othinfoarray.Length > 0)
            {
                for (int i = 0; i < othinfoarray.Length; i++)
                {
                    if (othinfoarray[i].otherserviceinfo1.Length > 0)
                    {
                        line18 = "OSI/" + othinfoarray[i].otherserviceinfo1 + "$";
                        if (othinfoarray[i].otherserviceinfo2.Length > 0)
                        {
                            line18 = line18 + "/" + othinfoarray[i].otherserviceinfo2 + "$";
                        }
                    }
                }
                line18 = line18.Trim('$');
                line18 = line18.Replace("$", "\r\n");
            }
            #endregion

            //CDC
            #region Line 19
            string line19 = "";
            if (fwbdata.cccurrencycode.Length > 0 || fwbdata.ccexchangerate.Length > 0 || fwbdata.ccchargeamt.Length > 0)
            {
                string[] exchnagesplit = fwbdata.ccexchangerate.Split(',');
                string[] chargesplit = fwbdata.ccchargeamt.Split(',');
                if (exchnagesplit.Length == chargesplit.Length)
                {
                    for (int k = 0; k < exchnagesplit.Length; k++)
                    {
                        line19 = line19 + exchnagesplit[k] + "/" + chargesplit[k] + "/";
                    }
                }
                line19 = "CDC/" + fwbdata.cccurrencycode + line19.Trim('/');
            }
            #endregion

            //REF
            #region Line 20
            string line20 = "";
            line20 = fwbdata.senderairport + "" + fwbdata.senderofficedesignator + "" + fwbdata.sendercompanydesignator + "/" + fwbdata.senderFileref + "/" + fwbdata.senderParticipentIdentifier + "/" + fwbdata.senderParticipentCode + "/" + fwbdata.senderPariticipentAirport + "";
            //line20 = line20.Trim('/');
            if (line20.Length > 1)
            {
                line20 = "REF/" + line20;
            }

            #endregion

            //COR
            #region Line 21
            string line21 = "";
            if (fwbdata.customorigincode.Length > 0)
            {
                line21 = "COR/" + fwbdata.customorigincode + "";
            }
            #endregion

            //COI
            #region Line 22
            string line22 = "";
            if (fwbdata.commisioncassindicator.Length > 0 || fwbdata.commisionCassSettleAmt.Length > 0)
            {
                line22 = "COI/" + fwbdata.commisioncassindicator + "/" + fwbdata.commisionCassSettleAmt.Replace(',', '/') + "";
            }
            #endregion

            //SII
            #region Line 23
            string line23 = "";
            if (fwbdata.saleschargeamt.Length > 0 || fwbdata.salescassindicator.Length > 0)
            {
                line23 = "SII/" + fwbdata.saleschargeamt + "/" + fwbdata.salescassindicator + "";
            }
            #endregion

            //ARD
            #region Line 24
            string line24 = "";
            if (fwbdata.agentfileref.Length > 0)
            {
                line24 = "ARD/" + fwbdata.agentfileref + "";
            }
            #endregion

            //SPH
            #region Line 25
            string line25 = "";
            if (fwbdata.splhandling.Replace(",", "").Length > 0)
            {
                line25 = "SPH/" + fwbdata.splhandling.Replace(',', '/');
            }
            #endregion

            //NOM
            #region Line 26
            string line26 = "";
            if (fwbdata.handlingname.Length > 0 || fwbdata.handlingplace.Length > 0)
            {
                line26 = "NOM/" + fwbdata.handlingname + "/" + fwbdata.handlingplace;
            }
            #endregion

            //SRI
            #region Line 27
            string line27 = "";
            if (fwbdata.shiprefnum.Length > 0 || fwbdata.supplemetryshipperinfo1.Length > 0 || fwbdata.supplemetryshipperinfo2.Length > 0)
            {
                line27 = "SRI/" + fwbdata.shiprefnum + "/" + fwbdata.supplemetryshipperinfo1 + "/" + fwbdata.supplemetryshipperinfo2;
            }
            #endregion

            //OPI
            #region Line 28
            str1 = "";
            string line28 = "";
            if (fwbdata.othairport.Length > 0 || fwbdata.othofficedesignator.Length > 0 || fwbdata.othcompanydesignator.Length > 0 || fwbdata.othfilereference.Length > 0 || fwbdata.othparticipentidentifier.Length > 0 || fwbdata.othparticipentcode.Length > 0 || fwbdata.othparticipentairport.Length > 0)
            {
                str1 = "/" + fwbdata.othparticipentairport + "/" +
                fwbdata.othofficedesignator + "" + fwbdata.othcompanydesignator + "/" + fwbdata.othfilereference + "/" +
                fwbdata.othparticipentidentifier + "/" + fwbdata.othparticipentcode + "/" + fwbdata.othparticipentairport + "";
                str1 = str1.Trim('/');
            }

            if (fwbdata.othparticipentname.Length > 0 || str1.Length > 0)
            {
                line28 = "OPI/" + fwbdata.othparticipentname + "$" + str1;
            }
            line28 = line28.Trim('$');
            line28 = line28.Replace("$", "\r\n");
            #endregion

            //OCI
            #region Line 29
            string line29 = "";
            if (custominfo.Length > 0)
            {
                for (int i = 0; i < custominfo.Length; i++)
                {
                    line29 = "/" + custominfo[i].IsoCountryCodeOci + "/" + custominfo[i].InformationIdentifierOci + "/" + custominfo[i].CsrIdentifierOci + "/" + custominfo[i].SupplementaryCsrIdentifierOci + "$";
                }
                line29 = "OCI" + line4.Trim('$');
                line29 = line4.Replace("$", "\r\n");
            }
            #endregion

            #region Build FWB
            fwbstr = "";           
            fwbstr = line1.Trim('/');
            if (line2.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line2.Trim('/');
            }
            if (line3.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line3.Trim('/');
            }
            fwbstr += "\r\n" + line4.Trim('/') + "\r\n" + line5.Trim('/') + "\r\n" + line6.Trim('/');
            if (line7.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line7.Trim('/');
            }
            if (line8.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line8.Trim('/');
            }
            if (line9.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line9.Trim('/');
            }
            if (line10.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line10.Trim('/');
            }
            fwbstr += "\r\n" + line11.Trim('/') + "\r\n" + line12.Trim('/');
            if (line13.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line13.Trim('/');
            }
            if (line14.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line14.Trim('/');
            }
            if (line15.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line15.Trim('/');
            }
            if (line16.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line16.Trim('/');
            }
            if (line17.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line17.Trim('/');
            }
            if (line18.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line18.Trim('/');
            }
            if (line19.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line19.Trim('/');
            }
            if (line20.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line20.Trim('/');
            }
            if (line21.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line21.Trim('/');
            }
            if (line22.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line22.Trim('/');
            }
            if (line23.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line23.Trim('/');
            }
            if (line24.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line24.Trim('/');
            }
            if (line25.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line25.Trim('/');
            }
            if (line26.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line26.Trim('/');
            }
            if (line27.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line27.Trim('/');
            }
            if (line28.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line28.Trim('/');
            }
            if (line29.Trim('/').Length > 0)
            {
                fwbstr += "\r\n" + line29.Trim('/');
            }
            #endregion
        }
        catch (Exception ex)
        {
            fwbstr = "ERR";
        }
        return fwbstr;
    }

    public static string buildRateNode(ref MessageData.RateDescription[] fwbrate)
    {
        string Ratestr = null;
        try
        {
            string str1, str2, str3, str4, str5, str6, str7, str8;
            for (int i = 0; i < fwbrate.Length; i++)
            {
                int cnt = 1;
                str1 = str2 = str3 = str4 = str5 = str6 = str7 = str8 = "";
                if (fwbrate[i].goodsnature.Length > 0)
                {
                    if (cnt > 1)
                    {
                        str1 = "/" + (cnt++) + "/NG/" + fwbrate[i].goodsnature;
                    }
                    else
                    {
                        str1 = "/NG/" + fwbrate[i].goodsnature;
                        cnt++;
                    }
                }
                if (fwbrate[i].goodsnature1.Length > 0)
                {
                    if (cnt > 1)
                    {
                        str2 = "/" + (cnt++) + "/NC/" + fwbrate[i].goodsnature1;
                    }
                    else
                    {
                        str2 = "/NC/" + fwbrate[i].goodsnature1;
                        cnt++;
                    }
                }
                if (fwbrate[i].weight.Length > 0 || fwbrate[i].length.Length > 0 || fwbrate[i].width.Length > 0 || fwbrate[i].height.Length > 0 || fwbrate[i].pcscnt.Length > 0)
                {
                    if (cnt > 1)
                    {
                        if (fwbrate[i].length.Length > 0 || fwbrate[i].width.Length > 0 || fwbrate[i].height.Length > 0 || fwbrate[i].pcscnt.Length > 0)
                            str3 = "/" + (cnt++) + "/ND/" + fwbrate[i].weightindicator + fwbrate[i].weight + "/" + fwbrate[i].unit + fwbrate[i].length + "-" + fwbrate[i].width + "-" + fwbrate[i].height + "/" + fwbrate[i].pcscnt;
                        else
                            str3 = "/" + (cnt++) + "/ND/" + fwbrate[i].weightindicator + fwbrate[i].weight + "/NDA"; 

                    }
                    else
                    {
                        if (fwbrate[i].length.Length > 0 || fwbrate[i].width.Length > 0 || fwbrate[i].height.Length > 0 || fwbrate[i].pcscnt.Length > 0)
                            str3 = "/ND/" + fwbrate[i].weightindicator + fwbrate[i].weight + "/" + fwbrate[i].unit + fwbrate[i].length + "-" + fwbrate[i].width + "-" + fwbrate[i].height + "/" + fwbrate[i].pcscnt;
                        else
                            str3 = "/ND/" + fwbrate[i].weightindicator + fwbrate[i].weight + "/NDA";

                        cnt++;
                    }
                }
                else
                {
                    if (cnt > 1)
                    {
                        str3 = "/" + (cnt++) + "/ND/" + "/" + "NDA";
                    }
                }
                str3 = str3.Replace("--", "");
                if (fwbrate[i].volcode.Length > 0 || fwbrate[i].volamt.Length > 0)
                {
                    if (cnt > 1)
                    {
                        str4 = "/" + (cnt++) + "/NV/" + fwbrate[i].volcode + fwbrate[i].volamt;
                    }
                    else
                    {
                        str4 = "/NV/" + fwbrate[i].volcode + fwbrate[i].volamt;
                        cnt++;
                    }
                }
                if (fwbrate[i].uldtype.Length > 0 || fwbrate[i].uldserialnum.Length > 0 || fwbrate[i].uldowner.Length > 0)
                {
                    if (cnt > 1)
                    {
                        str5 = "/" + (cnt++) + "/NU/" + fwbrate[i].uldtype + fwbrate[i].uldserialnum + fwbrate[i].uldowner;
                    }
                    else
                    {
                        str5 = "/NU/" + fwbrate[i].uldtype + fwbrate[i].uldserialnum + fwbrate[i].uldowner;
                        cnt++;

                    }
                }
                if (fwbrate[i].slac.Length > 0)
                {
                    if (cnt > 1)
                    {
                        str6 = "/" + (cnt++) + "/NS/" + fwbrate[i].slac;
                    }
                    else
                    {
                        str6 = "/NS/" + fwbrate[i].slac;
                        cnt++;
                    }
                }
                if (fwbrate[i].hermonisedcomoditycode.Length > 0)
                {
                    if (cnt > 1)
                    {
                        str7 = "/" + (cnt++) + "/NH/" + fwbrate[i].hermonisedcomoditycode;
                    }
                    else
                    {
                        str7 = "/NH/" + fwbrate[i].hermonisedcomoditycode;
                        cnt++;
                    }
                }
                if (fwbrate[i].isocountrycode.Length > 0)
                {
                    if (cnt > 1)
                    {
                        str8 = "/" + (cnt++) + "/NO/" + fwbrate[i].isocountrycode;
                    }
                    else
                    {
                        str8 = "/NO/" + fwbrate[i].isocountrycode;
                        cnt++;
                    }
                }
                Ratestr += "RTD/" + (i + 1) + "/" + fwbrate[i].pcsidentifier + fwbrate[i].numofpcs + "/" + fwbrate[i].weightindicator + fwbrate[i].awbweight + "/C" + fwbrate[i].rateclasscode + "/S" + fwbrate[i].commoditynumber + "/W" + fwbrate[i].awbweight + "/R" + fwbrate[i].chargerate + "/T" + fwbrate[i].chargeamt;
                Ratestr = Ratestr.Trim('/') + "$" + str1.Trim() + "$" + str2.Trim() + "$" + str3.Trim() + "$" + str4.Trim() + "$" + str5.Trim() + "$" + str6.Trim() + "$" + str7.Trim() + "$" + str8.Trim();
                Ratestr = Ratestr.Replace("$$", "$");
                Ratestr = Ratestr.Trim('$');
                Ratestr = Ratestr.Replace("$", "\r\n");
            }

        }
        catch (Exception ex)
        {
            Ratestr = null;
        }
        return Ratestr;
    }
    #endregion

    //FHL Messaging
    #region Decode FHL message
    public static bool decodereceiveFHL(string fhlmsg, ref MessageData.fhlinfo fhldata, ref MessageData.consignmnetinfo[] consinfo, ref MessageData.customsextrainfo[] custominfo)
    {

        bool flag = false;
        try
        {
            string lastrec = "NA";
            int line = 0;
            try
            {
                if (fhlmsg.StartsWith("FHL", StringComparison.OrdinalIgnoreCase))
                {                   
                    string[] str = fhlmsg.Split('$');
                    if (str.Length > 3)
                    {
                        for (int i = 0; i < str.Length; i++)
                        {

                            flag = true;
                            #region Line 1 version
                            if (str[i].StartsWith("FHL", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    fhldata.fhlversionnum = msg[1];
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 2 awb consigment details
                            if (str[i].StartsWith("MBI", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    lastrec = "AWB";
                                    line = 0;
                                    string[] msg = str[i].Split('/');
                                    //0th element
                                    string[] decmes = msg[1].Split('-');
                                    fhldata.airlineprefix = decmes[0];
                                    fhldata.awbnum = decmes[1].Substring(0, decmes[1].Length - 6);
                                    fhldata.origin = decmes[1].Substring(decmes[1].Length - 6, 3);
                                    fhldata.dest = decmes[1].Substring(decmes[1].Length - 3, 3);
                                    //1
                                    if (msg[2].Length > 0)
                                    {
                                        int k = 0;
                                        char lastchr = 'A';
                                        char[] arr = msg[2].ToCharArray();
                                        string[] strarr = new string[arr.Length];
                                        for (int j = 0; j < arr.Length; j++)
                                        {
                                            if ((char.IsNumber(arr[j])) || (arr[j].Equals('.')))
                                            {//number                            
                                                if (lastchr == 'N')
                                                    k--;
                                                strarr[k] = strarr[k] + arr[j].ToString();
                                                lastchr = 'N';
                                            }
                                            if (char.IsLetter(arr[j]))
                                            {//letter
                                                if (lastchr == 'L')
                                                    k--;
                                                strarr[k] = strarr[k] + arr[j].ToString();
                                                lastchr = 'L';
                                            }
                                            k++;
                                        }
                                        fhldata.consigntype = strarr[0];
                                        fhldata.pcscnt = strarr[1];//int.Parse(strarr[1]);
                                        fhldata.weightcode = strarr[2];
                                        fhldata.weight = strarr[3];//float.Parse(strarr[3]);                                            
                                    }
                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            #region  line 3 onwards check consignment details
                            if (str[i].StartsWith("HBS", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    lastrec = "AWB";
                                    line = 0;
                                    decodeFHLconsigmentdetails(str[i], ref consinfo);
                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            #region  line 4 Free Text
                            if (str[i].StartsWith("TXT", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = "TXT";
                                    line = 0;
                                    consinfo[consinfo.Length - 1].freetextGoodDesc = consinfo[consinfo.Length - 1].freetextGoodDesc + msg[1] + ",";
                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            #region  line 4 Harmonised Tariff Schedule
                            if (str[i].StartsWith("HTS", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = "HTS";
                                    line = 0;
                                    consinfo[consinfo.Length - 1].commodity = consinfo[consinfo.Length - 1].commodity + msg[1] + ",";
                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            #region Line5 custom extra info
                            if (str[i].StartsWith("OCI", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 0)
                                {
                                    lastrec = "OCI";
                                    MessageData.customsextrainfo custom = new MessageData.customsextrainfo("");
                                    custom.IsoCountryCodeOci = msg[1];
                                    custom.InformationIdentifierOci = msg[2];
                                    custom.CsrIdentifierOci = msg[3];
                                    custom.SupplementaryCsrIdentifierOci = msg[4];
                                    custom.consigref = awbref;
                                    Array.Resize(ref custominfo, custominfo.Length + 1);
                                    custominfo[custominfo.Length - 1] = custom;
                                }
                            }
                            #endregion

                            #region Line 5 Shipper Infor
                            if (str[i].StartsWith("SHP", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg.Length > 1)
                                    {
                                        fhldata.shippername = msg[1];

                                    }
                                }
                                catch (Exception e10)
                                { }
                            }
                            #endregion

                            #region Line 6 Consignee
                            if (str[i].StartsWith("CNE", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg.Length > 1)
                                    {
                                        fhldata.consname = msg[1];
                                    }
                                }
                                catch (Exception e10)
                                { }
                            }
                            #endregion

                            #region Line 11 Charge declaration
                            if (str[i].StartsWith("CVD", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 1)
                                {
                                    try
                                    {
                                        fhldata.currency = msg[1];
                                        fhldata.chargecode = msg[2].Length > 0 ? msg[2] : "";
                                        fhldata.chargedec = msg[3].Length > 0 ? msg[3] : "";
                                        fhldata.declaredvalue = msg[4];
                                        fhldata.declaredcustomvalue = msg[5];
                                        fhldata.insuranceamount = msg[6];
                                    }
                                    catch (Exception ex) { }
                                }
                            }
                            #endregion

                            #region Other Info
                            if (str[i].StartsWith("/"))
                            {
                                string[] msg = str[i].Split('/');
                                try
                                {
                                    #region SHP Data
                                    if (lastrec == "SHP")
                                    {
                                        line++;
                                        if (line == 1)
                                        {
                                            fhldata.shipperadd = msg[1].Length > 0 ? msg[1] : "";

                                        }
                                        if (line == 2)
                                        {
                                            fhldata.shipperplace = msg[1].Length > 0 ? msg[1] : "";
                                            fhldata.shipperstate = msg[2].Length > 0 ? msg[2] : "";
                                        }
                                        if (line == 3)
                                        {
                                            fhldata.shippercountrycode = msg[1].Length > 0 ? msg[1] : "";
                                            fhldata.shipperpostcode = msg[2].Length > 0 ? msg[2] : "";
                                            fhldata.shippercontactidentifier = msg[3].Length > 0 ? msg[3] : "";
                                            fhldata.shippercontactnum = msg[4].Length > 0 ? msg[4] : "";

                                        }

                                    }
                                    #endregion

                                    #region CNE Data
                                    if (lastrec == "CNE")
                                    {
                                        line++;
                                        if (line == 1)
                                        {
                                            fhldata.consadd = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                        if (line == 2)
                                        {
                                            fhldata.consplace = msg[1].Length > 0 ? msg[1] : "";
                                            fhldata.consstate = msg[2].Length > 0 ? msg[2] : "";
                                        }
                                        if (line == 3)
                                        {
                                            fhldata.conscountrycode = msg[1].Length > 0 ? msg[1] : "";
                                            fhldata.conspostcode = msg[2].Length > 0 ? msg[2] : "";
                                            fhldata.conscontactidentifier = msg[3].Length > 0 ? msg[3] : "";
                                            fhldata.conscontactnum = msg[4].Length > 0 ? msg[4] : "";
                                        }

                                    }
                                    #endregion

                                    #region Commodity
                                    if (lastrec == "HTS")
                                    {
                                        if (str[i].Length > 1)
                                        {
                                            consinfo[consinfo.Length - 1].commodity = consinfo[consinfo.Length - 1].commodity + msg[1] + ",";
                                        }
                                    }
                                    #endregion

                                    #region freetextGoodDesc
                                    if (lastrec == "TXT")
                                    {
                                        if (str[i].Length > 1)
                                        {
                                            consinfo[consinfo.Length - 1].freetextGoodDesc = consinfo[consinfo.Length - 1].freetextGoodDesc + msg[1];
                                        }
                                    }
                                    #endregion

                                    #region Splhandling
                                    if (lastrec == "AWB")
                                    {
                                        if (str[i].Length > 1)
                                        {
                                            consinfo[consinfo.Length - 1].splhandling = str[i].Replace('/', ',');
                                        }
                                        lastrec = "NA";
                                    }
                                    #endregion

                                    #region OCI
                                    if (lastrec == "OCI")
                                    {
                                        string[] msgdata = str[i].Split('/');
                                        if (msgdata.Length > 0)
                                        {
                                            lastrec = "OCI";
                                            MessageData.customsextrainfo custom = new MessageData.customsextrainfo("");
                                            custom.IsoCountryCodeOci = msgdata[1];
                                            custom.InformationIdentifierOci = msgdata[2];
                                            custom.CsrIdentifierOci = msgdata[3];
                                            custom.SupplementaryCsrIdentifierOci = msgdata[4];
                                            Array.Resize(ref custominfo, custominfo.Length + 1);
                                            custominfo[custominfo.Length - 1] = custom;
                                        }
                                    }
                                    #endregion
                                }
                                catch (Exception e13)
                                { }
                            }
                            #endregion
                        }
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }
    #endregion

    #region Encode FHL
    public static string EncodeFHLforsend(ref MessageData.fhlinfo fhldata, ref MessageData.consignmnetinfo[] consinfo, ref MessageData.customsextrainfo[] custominfo)
    {
        string fhl = null, flightcons = "";
        try
        {
            #region Line 1
            string line1 = "FHL" + "/" + fhldata.fhlversionnum;
            #endregion

            #region Line 2
            string line2 = "MBI/" + fhldata.airlineprefix + "-" + fhldata.awbnum + fhldata.origin + fhldata.dest + "/" + fhldata.consigntype + fhldata.pcscnt + fhldata.weightcode + fhldata.weight;
            #endregion line 2

            #region Line3
            string line3 = FHLPartBuilder(ref consinfo, ref custominfo);
            #endregion

            //SHP
            #region Line 5
            string line5 = "";
            string str1 = "", str2 = "", str3 = "", str4 = "";
            try
            {
                if (fhldata.shippername.Length > 0)
                {
                    str1 = "/" + fhldata.shippername;
                }
                if (fhldata.shipperadd.Length > 0)
                {
                    str2 = "/" + fhldata.shipperadd;
                }

                if (fhldata.shipperplace.Length > 0 || fhldata.shipperstate.Length > 0)
                {
                    str3 = "/" + fhldata.shipperplace + "/" + fhldata.shipperstate;
                }
                if (fhldata.shippercountrycode.Length > 0 || fhldata.shipperpostcode.Length > 0 || fhldata.shippercontactidentifier.Length > 0 || fhldata.shippercontactnum.Length > 0)
                {
                    str4 = "/" + fhldata.shippercountrycode + "/" + fhldata.shipperpostcode + "/" + fhldata.shippercontactidentifier + "/" + fhldata.shippercontactnum;
                }

                if (str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line5 = "SHP/";
                    if (str4.Length > 0)
                    {
                        line5 = line5.Trim() + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/') + "\r\n/" + str4.Trim('/');
                    }
                    else if (str3.Length > 0)
                    {
                        line5 = line5.Trim() + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line5 = line5.Trim() + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line5 = line5.Trim() + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            //CNE
            #region Line 6
            string line6 = "";
            str1 = "";
            str2 = "";
            str3 = "";
            str4 = "";
            try
            {
                if (fhldata.consname.Length > 0)
                {
                    str1 = "/" + fhldata.consname;
                }
                if (fhldata.consadd.Length > 0)
                {
                    str2 = "/" + fhldata.consadd;
                }

                if (fhldata.consplace.Length > 0 || fhldata.consstate.Length > 0)
                {
                    str3 = "/" + fhldata.consplace + "/" + fhldata.consstate;
                }
                if (fhldata.conscountrycode.Length > 0 || fhldata.conspostcode.Length > 0 || fhldata.conscontactidentifier.Length > 0 || fhldata.conscontactnum.Length > 0)
                {
                    str4 = "/" + fhldata.conscountrycode + "/" + fhldata.conspostcode + "/" + fhldata.conscontactidentifier + "/" + fhldata.conscontactnum;
                }

                if (str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line6 = "CNE/";
                    if (str4.Length > 0)
                    {
                        line6 = line6.Trim() + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/') + "\r\n/" + str4.Trim('/');
                    }
                    else if (str3.Length > 0)
                    {
                        line6 = line6.Trim() + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line6 = line6.Trim() + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line6 = line6.Trim() + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            //CVD
            #region Line 9
            string line9 = "";
            if (fhldata.currency.Length > 0 || fhldata.chargecode.Length > 0 || fhldata.declaredvalue.Length > 0 || fhldata.declaredcustomvalue.Length > 0 || fhldata.insuranceamount.Length > 0)
            {
                line9 = "CVD/" + fhldata.currency + "/" + fhldata.chargecode + "/PP/" + fhldata.declaredvalue + "/" + fhldata.declaredcustomvalue + "/" + fhldata.insuranceamount + "";
            }
            #endregion


            #region BuildFHL
            fhl = line1.Trim('/') + "\r\n" + line2.Trim() + "\r\n" + line3.Trim();
            if (line5.Length > 0)
            {
                fhl = fhl + "\r\n" + line5.Trim('/');
            }
            if (line6.Length > 0)
            {
                fhl = fhl + "\r\n" + line6.Trim('/');
            }
            if (line9.Length > 0)
            {
                fhl = fhl + "\r\n" + line9.Trim('/');
            }
            #endregion

        }
        catch (Exception ex)
        {
            fhl = "";
        }
        return fhl;
    }

    public static string FHLPartBuilder(ref MessageData.consignmnetinfo[] consinfo, ref MessageData.customsextrainfo[] custominfo)
    {
        string output = "";
        try
        {
            #region line 4 Consigment INfo
            string line4 = "";
            if (consinfo.Length > 0)
            {
                for (int i = 0; i < consinfo.Length; i++)
                {
                    string splhandling = "";

                    if (consinfo[i].splhandling.Length > 0 && consinfo[i].splhandling != null)
                    {
                        splhandling = consinfo[i].splhandling.Replace(",", "/");
                        //splhandling = "\r\n" + splhandling;
                    }
                    line4 = line4 + "HBS/" + consinfo[i].airlineprefix + consinfo[i].awbnum +"/"+ consinfo[i].origin + consinfo[i].dest + "/" + consinfo[i].consigntype + consinfo[i].pcscnt + "/" + consinfo[i].weightcode + consinfo[i].weight + "/" + consinfo[i].slac + "/" + consinfo[i].manifestdesc + ((splhandling.Length) > 0 ? ("$/" + splhandling) : "");

                    line4 = line4.Trim('/') + "$";
                    if (consinfo[i].freetextGoodDesc.Length > 0)
                    {
                        line4 = line4 + "TXT/" + consinfo[i].freetextGoodDesc + "$";
                    }
                    if (consinfo[i].commodity.Length > 0)
                    {
                        line4 = line4 + "HTS/" + consinfo[i].commodity + "$";
                    }
                    #region Line 9 OCI
                    string line9 = "";
                    if (custominfo.Length > 0)
                    {
                        for (int k = 0; k < custominfo.Length; k++)
                        {
                            if (custominfo[k].consigref.Equals((i + 1).ToString()))
                            {
                                line9 = "/" + custominfo[k].IsoCountryCodeOci + "/" + custominfo[k].InformationIdentifierOci + "/" + custominfo[k].CsrIdentifierOci + "/" + custominfo[k].SupplementaryCsrIdentifierOci + "$";
                            }

                        }
                        line9 = "OCI" + line9.Trim('$');
                        line9 = line9.Replace("$", "\r\n");
                        if (line9.Length > 0)
                        {
                            output = output + line9 + "\r\n";
                        }
                    }
                    #endregion


                }
                line4 = line4.Trim('$');
                line4 = line4.Replace("$", "\r\n");
            }
            #endregion

            if (output.Length > 0)
            {
                output = line4 + "\r\n" + output;
            }
            else
            {
                output = line4;
            }

        }
        catch (Exception ex)
        {
            output = "ERR";
        }
        return output;
    }

    #endregion

    #region Decode Consigment Details
    public static void decodeFHLconsigmentdetails(string inputstr, ref MessageData.consignmnetinfo[] consinfo)
    {
        MessageData.consignmnetinfo consig = new MessageData.consignmnetinfo("");
        try
        {
            string[] msg = inputstr.Split('/');

            //consinfo[num] = new MessageData.consignmnetinfo("");
            //consig.airlineprefix = msg[1].Substring(0, 3);
            consig.awbnum = msg[1];

            consig.origin = msg[2].Substring(0, 3);
            consig.dest = msg[2].Substring(3);

            consig.consigntype = "";
            consig.pcscnt = msg[3];//int.Parse(strarr[1]);
            consig.weightcode = msg[4].Substring(0, 1);
            consig.weight = msg[4].Substring(1);

            if (msg.Length > 4)
            {
                consig.slac = msg[5];
            }
            if (msg.Length > 5)
            {
                consig.manifestdesc = msg[6];
            }
           
        }
        catch (Exception ex) { }
        Array.Resize(ref consinfo, consinfo.Length + 1);
        consinfo[consinfo.Length - 1] = consig;
    }
    #endregion

    #region WriteLog
    public static void WriteLog(String Message)
    {
        try
        {
            string APP_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\" + "Alert_Log.txt";
            long length = 0;
            StreamWriter sw1;
            if (File.Exists(APP_PATH))
            {
                FileInfo file = new FileInfo(APP_PATH);
                length = file.Length;
            }
            if (length > 10000000)
                sw1 = new StreamWriter(APP_PATH, false);
            else
                sw1 = new StreamWriter(APP_PATH, true);
            sw1.WriteLine(Message);
            sw1.Close();
        }
        catch (Exception e)
        {
        }
    }
    #endregion

    //UCM messaging
    #region Decode UCM message
    public static bool decodereceiveUCM(string UCMmsg,ref MessageData.UCMInfo ucmdata,ref MessageData.ULDinfo[] uld)
    {

        bool flag = false;
        try
        {
            string movement = "",flight = "", AirlinePrefix="";
            int line = 0;
            try
            {
                if (UCMmsg.StartsWith("UCM", StringComparison.OrdinalIgnoreCase))
                {
                    UCMmsg = UCMmsg.Replace("..",".");
                    // ffrmsg = ffrmsg.Replace("\r\n","$");
                    string[] str = UCMmsg.Split('$');
                    if (str.Length > 3)
                    {
                        if (str[0].StartsWith("UCM", StringComparison.OrdinalIgnoreCase))
                        {
                            for (int i = 1; i < str.Length; i++)
                            {
                                #region Line 1
                                if (i == 1)
                                {
                                    string[] splitStr = str[i].Split('/');
                                    try
                                    {
                                        for (int j = 0; j < splitStr.Length; j++)
                                        {

                                            if (splitStr.Length > 0 && j == 0)
                                            {
                                                ucmdata.FltNo = splitStr[0];
                                                AirlinePrefix = ucmdata.FltNo.ToString().Substring(0, 2);
                                            }
                                            try
                                            {
                                                if (j > 1)
                                                {
                                                    int val = int.MaxValue;
                                                    if (int.TryParse(splitStr[j], out val))
                                                    {
                                                        flight = splitStr[j];
                                                        flight = AirlinePrefix + flight;
                                                    }
                                                    if (splitStr[j].Contains(AirlinePrefix))
                                                    {
                                                        flight = splitStr[j];
                                                    }
                                                }
                                                if (j > 1 && splitStr[j].Contains('.'))
                                                {
                                                    string[] localStr = splitStr[i].Split('.');
                                                    if (localStr.Length > 0)
                                                    {
                                                        ucmdata.Date = localStr[0];
                                                        ucmdata.FltRegNo = localStr[1];
                                                        ucmdata.StationCode = localStr[2];
                                                    }
                                                }
                                            }
                                            catch (Exception ex) { }
                                        }
                                    }
                                    catch (Exception ex)
                                    { }

                                    try
                                    {
                                        if (flight.Length > 0)
                                        {
                                            ucmdata.OutFltNo = flight;
                                        }
                                        else
                                        {
                                            ucmdata.OutFltNo = ucmdata.FltNo;
                                        }
                                    }
                                    catch (Exception ex) { }
                                }
                                #endregion

                                #region ULD movement Portion
                                if (str[i].StartsWith("IN", StringComparison.OrdinalIgnoreCase) || str[i].StartsWith("OUT", StringComparison.OrdinalIgnoreCase))
                                {
                                    movement = str[i];
                                }
                                #endregion

                                #region line Starts with dot(.) ULD Data
                                if (str[i].StartsWith(".", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (str[i].Length > 2)
                                    {
                                        string[] splitStr = str[i].Split('.');
                                        if (splitStr.Length > 0)
                                        {
                                            for (int k = 0; k < splitStr.Length; k++)
                                            {
                                                if (splitStr[k].Length > 0)
                                                {
                                                    MessageData.ULDinfo ulddata = new MessageData.ULDinfo("");
                                                    if (splitStr[k].ToString().Contains('/'))
                                                    {
                                                        string[] localStr = splitStr[k].ToString().Split('/');
                                                        ulddata.uldno = localStr[0];
                                                        ulddata.uldtype = localStr[0].Substring(0, 3);
                                                        ulddata.uldsrno = localStr[0].Substring(3, localStr[0].Length - 5);
                                                        ulddata.uldowner = localStr[0].Substring(localStr[0].Length - 2, 2);
                                                        ulddata.movement = movement;
                                                        try
                                                        {
                                                            if (localStr.Length > 0)
                                                            {
                                                                ulddata.stationcode = localStr[1];
                                                                ulddata.uldloadingindicator = localStr[2];
                                                            }
                                                        }
                                                        catch (Exception ex) { }
                                                    }
                                                    else
                                                    {
                                                        ulddata.uldno = splitStr[k];
                                                        ulddata.uldtype = splitStr[k].Substring(0, 3);
                                                        ulddata.uldsrno = splitStr[k].Substring(3, splitStr[k].Length - 5);
                                                        ulddata.uldowner = splitStr[k].Substring(splitStr[k].Length - 2, 2);
                                                        ulddata.movement = movement;
                                                    }
                                                    Array.Resize(ref uld, uld.Length + 1);
                                                    uld[uld.Length - 1] = ulddata;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }

                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }
    #endregion

    #region Encode UCM
    private string EncodeUCMforsend(ref MessageData.UCMInfo ucmdata,ref MessageData.ULDinfo[] uld)
    {
        string UCM = null, flightcons = "";
        try
        {
            #region Line 1
            string line1 = "UCM";
            #endregion

            #region Line 2
            string line2 = ucmdata.FltNo + "/" + ucmdata.Date + "." + ucmdata.FltRegNo + "." + ucmdata.StationCode;
            line2 = line2.Trim('.');
            line2 = line2.Trim('/');
            #endregion line 2

            #region Line3
            string line3 = "IN";
            #endregion

            #region Line4
            string line4 = "";
            if (uld.Length > 0)
            {
                for (int i = 0; i < uld.Length; i++)
                {
                    if (uld[i].movement.Equals("IN", StringComparison.OrdinalIgnoreCase))
                    {
                        if (uld[i].uldno.Length > 0)
                        {
                            line4 = line4 + "." + uld[i].uldno + "/" + uld[i].stationcode + "/" + uld[i].uldloadingindicator;
                            line4 = line4.Trim('/');
                        }
                    }
                    if (((i + 1) % 3) == 0)
                    {
                        line4 = line4 + "$";
                    }
                }
                line4 = line4.Trim('$');
                if (line4.Length > 0)
                {
                    line4 = line4.Replace("$", "\r\n");
                }
                else
                {
                    line4 = ".N";
                }
            }
            #endregion

            #region Line5
            string line5 = "OUT";
            #endregion

            #region Line6
            string line6 = "";
            if (uld.Length > 0)
            {
                for (int i = 0; i < uld.Length; i++)
                {
                    if (uld[i].movement.Equals("OUT", StringComparison.OrdinalIgnoreCase))
                    {
                        if (uld[i].uldno.Length > 0)
                        {
                            line6 = line6 + "." + uld[i].uldno + "/" + uld[i].stationcode + "/" + uld[i].uldloadingindicator;
                            line6 = line6.Trim('/');
                        }
                    }
                    if (((i + 1) % 3) == 0)
                    {
                        line6 = line6 + "$";
                    }
                }
                line6 = line6.Trim('$');
                if (line6.Length > 0)
                {
                    line6 = line6.Replace("$", "\r\n");
                }
                else
                {
                    line6 = ".N";
                }
            }
            #endregion

            #region BuildUCM
            UCM = line1.Trim('/') + "\r\n" + line2.Trim() + "\r\n" + line3.Trim();
            if (line4.Length > 0)
            {
                UCM = UCM + "\r\n" + line4.Trim('/');
            }
            if (line5.Length > 0)
            {
                UCM = UCM + "\r\n" + line5.Trim('/');
            }
            if (line6.Length > 0)
            {
                UCM = UCM + "\r\n" + line6.Trim('/');
            }
            #endregion

        }
        catch (Exception ex)
        {
            UCM = "ERR";
        }
        return UCM;
    }
    #endregion

    //Custom Messaging

    #region Custom Message Decoding


    #region Decode  Message
    public bool DecodeCustomsMessage(string MessageBody)
    {
        try 
        {
            MessageData.CustomMessage Message = new MessageData.CustomMessage();
           
            if (MessageBody!=null)
            {

                Message.Message = MessageBody;
                MessageBody=MessageBody.Replace("\r\n","$");
                char[] charSeparator = new char[] { '$' };
                //charSeparator[0]='$';
                string[] CustomMessage = MessageBody.Split(charSeparator,StringSplitOptions.RemoveEmptyEntries);
                int count = 0;
                    foreach (string str in CustomMessage)
                    {
                        count++;
                                //Getting Message Type
                        if (count == 1)
                        {
                            Message.MessageType = str.Trim().Substring(0, 3);
                        }

                        #region DECODING FSC Message
                        if (Message.MessageType == "FSC")
                            {
                                if (count == 2)
                                {
                                    //Getting AWB data if available
                                    if (str.Contains("-"))
                                    {
                                        string[] AWBInfo = str.Split('-');
                                        Message.AWBPrefix = AWBInfo[0].Trim();
                                        Message.AWBNumber = AWBInfo[1].Trim();
                                    }
                                    //Getting CCL Data if available
                                    else
                                    {
                                        Message.DestinionCode = str.Trim().Substring(0, 3).Trim();
                                        Message.ArrivalAirport = str.Trim().Substring(0, 3).Trim();
                                        Message.ImportingCarrier = str.Trim().Substring(3).Trim();
                                    }
                                }
                                if (count == 3)
                                {
                                    //Getting AWB data if available
                                    if (str.Contains("-"))
                                    {
                                        string[] AWBInfo = str.Split('-');
                                        Message.AWBPrefix = AWBInfo[0].Trim();
                                        Message.AWBNumber = AWBInfo[1].Trim();
                                    }
                                }
                                //Getting Arrival Details
                                if (str.StartsWith("ARR"))
                                {
                                    string[] ArrivalInfo = str.Split('/');
                                    Message.ImportingCarrier = ArrivalInfo[1].Trim().Substring(0, 2).Trim();
                                    Message.FlightNumber = ArrivalInfo[1].Trim().Substring(2).Trim();
                                    Message.ArrivalDate = ArrivalInfo[2].Trim();
                                    if (Message.ArrivalDate != "")
                                    {
                                       DateTime dt;
                                       dt = DateTime.ParseExact(Message.ArrivalDate+"13", "ddMMMyy", null);
                                       Message.ArrivalDate = dt.ToString("dd-MMM-yyyy");
                                    }
                                }
                                //Getting Freight Status Condition (FSC)
                                if (str.StartsWith("FSC") && str.Contains("/"))
                                {

                                    string[] FSC = str.Split('/');
                                    if (FSC.Length > 0)
                                    {
                                        Message.StatusAnswerCode = FSC[1].Trim();

                                    }

                                }
                                //Getting TXT Details
                                if (str.StartsWith("TXT"))
                                {
                                    string[] TXTInfo = str.Split('/');
                                    Message.Information = TXTInfo[1].Trim();
                                }
                                if (str.StartsWith("TRN"))
                                {

                                }
                            }
                        #endregion

                        #region DECODING FSI Message
                        if (Message.MessageType == "FSI")
                        {
                            if (count == 2)
                            {
                                //Getting AWB data if available
                                if (str.Contains("-"))
                                {
                                    string[] AWBInfo = str.Split('-');
                                    Message.AWBPrefix = AWBInfo[0].Trim();
                                    Message.AWBNumber = AWBInfo[1].Trim();
                                }
                                //Getting CCL Data if available
                                else
                                {
                                    Message.DestinionCode = str.Trim().Substring(0, 3).Trim();
                                    Message.ArrivalAirport = str.Trim().Substring(0, 3).Trim();
                                    Message.ImportingCarrier = str.Trim().Substring(3).Trim();
                                }
                            }
                            if (count == 3)
                            {
                                //Getting AWB data if available
                                if (str.Contains("-"))
                                {
                                    string[] AWBInfo = str.Split('-');
                                    Message.AWBPrefix = AWBInfo[0].Trim();
                                    Message.AWBNumber = AWBInfo[1].Trim();
                                }
                            }
                            //Getting Arrival Details
                            if (str.StartsWith("ARR"))
                            {
                                string[] ArrivalInfo = str.Split('/');
                                Message.ImportingCarrier = ArrivalInfo[1].Trim().Substring(0, 2).Trim();
                                Message.FlightNumber = ArrivalInfo[1].Trim().Substring(2).Trim();
                                Message.ArrivalDate = ArrivalInfo[2].Trim();
                                if (Message.ArrivalDate != "")
                                {
                                    DateTime dt;
                                    dt = DateTime.ParseExact(Message.ArrivalDate + "13", "ddMMMyy", null);
                                    Message.ArrivalDate = dt.ToString("dd-MMM-yyyy");
                                }
                            }
                            //Getting (CSN)
                            if (str.StartsWith("CSN"))
                            {

                                string[] CSN = str.Split('/');
                                if (CSN.Length > 0)
                                {
                                    string[] ActionCodeDetails = CSN[1].Split('-');
                                    Message.CSNActionCode = ActionCodeDetails[0].Trim();
                                    Message.CSNPieces = ActionCodeDetails[1].Trim();
                                    Message.TransactionDate = CSN[2].Trim().Substring(0, 5);
                                    Message.TransactionTime = CSN[2].Trim().Substring(5).Trim();
                                    Message.TransactionTime = Message.TransactionTime.Insert(2, ":");


                                }

                            }
                        }
                        #endregion

                        #region DECODING FRH Message
                        if (Message.MessageType == "FRH")
                        {
                            if (count == 2)
                            {
                                //Getting AWB data if available
                                if (str.Contains("-"))
                                {
                                    string[] AWBInfo = str.Split('-');
                                    Message.AWBPrefix = AWBInfo[0].Trim();
                                    Message.AWBNumber = AWBInfo[1].Trim();
                                }
                                //Getting CCL Data if available
                                else
                                {
                                    Message.DestinionCode = str.Trim().Substring(0, 3).Trim();
                                    Message.ArrivalAirport = str.Trim().Substring(0, 3).Trim();
                                    Message.ImportingCarrier = str.Trim().Substring(3).Trim();
                                }
                            }
                            if (count == 3)
                            {
                                //Getting AWB data if available
                                if (str.Contains("-"))
                                {
                                    string[] AWBInfo = str.Split('-');
                                    Message.AWBPrefix = AWBInfo[0].Trim();
                                    Message.AWBNumber = AWBInfo[1].Trim();
                                }
                            }
                            //Getting Arrival Details
                            if (str.StartsWith("ARR"))
                            {
                                string[] ArrivalInfo = str.Split('/');
                                Message.ImportingCarrier = ArrivalInfo[1].Trim().Substring(0, 2).Trim();
                                Message.FlightNumber = ArrivalInfo[1].Trim().Substring(2).Trim();
                                Message.ArrivalDate = ArrivalInfo[2].Trim();
                                if (Message.ArrivalDate != "")
                                {
                                    DateTime dt;
                                    dt = DateTime.ParseExact(Message.ArrivalDate + "13", "ddMMMyy", null);
                                    Message.ArrivalDate = dt.ToString("dd-MMM-yyyy");
                                }
                            }
                            //Getting (HLD)
                            if (str.StartsWith("HLD"))
                            {

                                string[] HLD = str.Split('/');
                                if (HLD.Length > 0)
                                {
                                    string[] HoldDetails = HLD[1].Split('/');
                                    Message.RequestType = HoldDetails[0].Trim();
                                    if (HoldDetails.Length > 2)
                                    {
                                        Message.RequestExplanation = HoldDetails[1].Trim();
                                    }

                                }

                            }
                        }
                        #endregion

                        #region DECODING FSN Message
                        if (Message.MessageType == "FSN")
                        {
                            if (count == 2)
                            {
                                //Getting AWB data if available
                                if (str.Contains("-"))
                                {
                                    string[] AWBInfo = str.Split('-');
                                    Message.AWBPrefix = AWBInfo[0].Trim();
                                    Message.AWBNumber = AWBInfo[1].Trim();
                                }
                                //Getting CCL Data if available
                                else
                                {
                                    Message.DestinionCode = str.Trim().Substring(0, 3).Trim();
                                    Message.ArrivalAirport = str.Trim().Substring(0, 3).Trim();
                                    Message.ImportingCarrier = str.Trim().Substring(3).Trim();
                                }
                            }
                            if (count == 3)
                            {
                                //Getting AWB data if available
                                if (str.Contains("-"))
                                {
                                    string[] AWBInfo = str.Split('-');
                                    Message.AWBPrefix = AWBInfo[0].Trim();
                                    Message.AWBNumber = AWBInfo[1].Trim();
                                }
                            }
                            //Getting Arrival Details
                            if (str.StartsWith("ARR"))
                            {
                                string[] ArrivalInfo = str.Split('/');
                                Message.ImportingCarrier = ArrivalInfo[1].Trim().Substring(0, 2).Trim();
                                Message.FlightNumber = ArrivalInfo[1].Trim().Substring(2).Trim();
                                Message.ArrivalDate = ArrivalInfo[2].Trim();
                                if (Message.ArrivalDate != "")
                                {
                                    DateTime dt;
                                    dt = DateTime.ParseExact(Message.ArrivalDate + "13", "ddMMMyy", null);
                                    Message.ArrivalDate = dt.ToString("dd-MMM-yyyy");
                                }
                            }
                            //Getting (CSN)
                            if (str.StartsWith("CSN"))
                            {

                                string[] CSN = str.Split('/');
                                if (CSN.Length > 0)
                                {
                                    string[] ActionCodeDetails = CSN[1].Split('-');
                                    Message.CSNActionCode = ActionCodeDetails[0].Trim();
                                    Message.CSNPieces = ActionCodeDetails[1].Trim();
                                    Message.TransactionDate = CSN[2].Trim().Substring(0, 5);
                                    Message.TransactionTime = CSN[2].Trim().Substring(5).Trim();
                                    Message.TransactionTime = Message.TransactionTime.Insert(2, ":");


                                }

                            }
                        }
                        #endregion

                    }
                    if (Message.Message != "")
                    {
                        if (Message.MessageType == "FSI" || Message.MessageType == "FSC" || Message.MessageType == "FRH" || Message.MessageType == "FSN")
                        {
                            if (DecodeCustomMessage(Message))
                            { return true; }
                        }

                    }

                    return false;
                }
                
            
            return false;
        }
        catch (Exception ex)
        { return false; }
    }
    #endregion

    #region Decoding Messages
    public bool DecodeCustomMessage(MessageData.CustomMessage Message)
    {
        try
        {

            //Preparing Parameters to save the Message Details Against the AWB
            string[] QueryNames = new string[82];
            object[] QueryValues = new object[82];
            SqlDbType[] QueryTypes = new SqlDbType[82];

            int i = 0;
            QueryNames[i++] = "AWBPrefix";
            QueryNames[i++] = "AWBNumber";
            QueryNames[i++] = "MessageType";
            QueryNames[i++] = "HAWBNumber";
            QueryNames[i++] = "ConsolidationIdentifier";
            QueryNames[i++] = "PackageTrackingIdentifier";
            QueryNames[i++] = "AWBPartArrivalReference";
            QueryNames[i++] = "ArrivalAirport";
            QueryNames[i++] = "AirCarrier";
            QueryNames[i++] = "Origin";
            QueryNames[i++] = "DestinionCode";
            QueryNames[i++] = "WBLNumberOfPieces";
            QueryNames[i++] = "WBLWeightIndicator";
            QueryNames[i++] = "WBLWeight";
            QueryNames[i++] = "WBLCargoDescription";
            QueryNames[i++] = "ArrivalDate";
            QueryNames[i++] = "PartArrivalReference";
            QueryNames[i++] = "BoardedQuantityIdentifier";
            QueryNames[i++] = "BoardedPieceCount";
            QueryNames[i++] = "BoardedWeight";
            QueryNames[i++] = "ImportingCarrier";
            QueryNames[i++] = "FlightNumber";
            QueryNames[i++] = "ARRPartArrivalReference";
            QueryNames[i++] = "RequestType";
            QueryNames[i++] = "RequestExplanation";
            QueryNames[i++] = "EntryType";
            QueryNames[i++] = "EntryNumber";
            QueryNames[i++] = "AMSParticipantCode";
            QueryNames[i++] = "ShipperName";
            QueryNames[i++] = "ShipperAddress";
            QueryNames[i++] = "ShipperCity";
            QueryNames[i++] = "ShipperState";
            QueryNames[i++] = "ShipperCountry";
            QueryNames[i++] = "ShipperPostalCode";
            QueryNames[i++] = "ConsigneeName";
            QueryNames[i++] = "ConsigneeAddress";
            QueryNames[i++] = "ConsigneeCity";
            QueryNames[i++] = "ConsigneeState";
            QueryNames[i++] = "ConsigneeCountry";
            QueryNames[i++] = "ConsigneePostalCode";
            QueryNames[i++] = "TransferDestAirport";
            QueryNames[i++] = "DomesticIdentifier";
            QueryNames[i++] = "BondedCarrierID";
            QueryNames[i++] = "OnwardCarrier";
            QueryNames[i++] = "BondedPremisesIdentifier";
            QueryNames[i++] = "InBondControlNumber";
            QueryNames[i++] = "OriginOfGoods";
            QueryNames[i++] = "DeclaredValue";
            QueryNames[i++] = "CurrencyCode";
            QueryNames[i++] = "CommodityCode";
            QueryNames[i++] = "LineIdentifier";
            QueryNames[i++] = "AmendmentCode";
            QueryNames[i++] = "AmendmentExplanation";
            QueryNames[i++] = "DeptImportingCarrier";
            QueryNames[i++] = "DeptFlightNumber";
            QueryNames[i++] = "DeptScheduledArrivalDate";
            QueryNames[i++] = "LiftoffDate";
            QueryNames[i++] = "LiftoffTime";
            QueryNames[i++] = "DeptActualImportingCarrier";
            QueryNames[i++] = "DeptActualFlightNumber";
            QueryNames[i++] = "ASNStatusCode";
            QueryNames[i++] = "ASNActionExplanation";
            QueryNames[i++] = "CSNActionCode";
            QueryNames[i++] = "CSNPieces";
            QueryNames[i++] = "TransactionDate";
            QueryNames[i++] = "TransactionTime";
            QueryNames[i++] = "CSNEntryType";
            QueryNames[i++] = "CSNEntryNumber";
            QueryNames[i++] = "CSNRemarks";
            QueryNames[i++] = "ErrorCode";
            QueryNames[i++] = "ErrorMessage";
            QueryNames[i++] = "StatusRequestCode";
            QueryNames[i++] = "StatusAnswerCode";
            QueryNames[i++] = "Information";
            QueryNames[i++] = "ERFImportingCarrier";
            QueryNames[i++] = "ERFFlightNumber";
            QueryNames[i++] = "ERFDate";
            QueryNames[i++] = "Message";
            QueryNames[i++] = "UpdatedOn";
            QueryNames[i++] = "UpdatedBy";
            QueryNames[i++] = "CreatedOn";
            QueryNames[i++] = "CreatedBy";

            int k = 0;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.Int;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.Decimal;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.Int;
            QueryTypes[k++] = SqlDbType.Decimal;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.BigInt;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.DateTime;
            QueryTypes[k++] = SqlDbType.VarChar;
            QueryTypes[k++] = SqlDbType.DateTime;
            QueryTypes[k++] = SqlDbType.VarChar;

            int j = 0;

            QueryValues[j++] = Message.AWBPrefix;
            QueryValues[j++] = Message.AWBNumber;
            QueryValues[j++] = Message.MessageType;
            QueryValues[j++] = Message.HAWBNumber;
            QueryValues[j++] = Message.ConsolidationIdentifier;
            QueryValues[j++] = Message.PackageTrackingIdentifier;
            QueryValues[j++] = Message.AWBPartArrivalReference;
            QueryValues[j++] = Message.ArrivalAirport;
            QueryValues[j++] = Message.AirCarrier;
            QueryValues[j++] = Message.Origin;
            QueryValues[j++] = Message.DestinionCode;
            if (Message.WBLNumberOfPieces == "")
            { QueryValues[j++] = "0"; }
            else
            {
                QueryValues[j++] = Message.WBLNumberOfPieces;
            }

            QueryValues[j++] = Message.WBLWeightIndicator;
            if (Message.WBLWeight == "")
            { QueryValues[j++] = "0"; }
            else
            {
                QueryValues[j++] = Message.WBLWeight;
            }
            QueryValues[j++] = Message.WBLCargoDescription;
            QueryValues[j++] = Message.ArrivalDate;
            QueryValues[j++] = Message.PartArrivalReference;
            QueryValues[j++] = Message.BoardedQuantityIdentifier;
            if (Message.BoardedPieceCount == "")
            { QueryValues[j++] = "0"; }
            else
            {
                QueryValues[j++] = Message.BoardedPieceCount;
            }
            if (Message.BoardedWeight == "")
            {
                QueryValues[j++] = "0";
            }
            else
            {
                QueryValues[j++] = Message.BoardedWeight;
            }
            QueryValues[j++] = Message.ImportingCarrier;
            QueryValues[j++] = Message.FlightNumber;
            QueryValues[j++] = Message.ARRPartArrivalReference;
            QueryValues[j++] = Message.RequestType;
            QueryValues[j++] = Message.RequestExplanation;
            QueryValues[j++] = Message.EntryType;
            QueryValues[j++] = Message.EntryNumber;
            QueryValues[j++] = Message.AMSParticipantCode;
            QueryValues[j++] = Message.ShipperName;
            QueryValues[j++] = Message.ShipperAddress;
            QueryValues[j++] = Message.ShipperCity;
            QueryValues[j++] = Message.ShipperState;
            QueryValues[j++] = Message.ShipperCountry;
            QueryValues[j++] = Message.ShipperPostalCode;
            QueryValues[j++] = Message.ConsigneeName;
            QueryValues[j++] = Message.ConsigneeAddress;
            QueryValues[j++] = Message.ConsigneeCity;
            QueryValues[j++] = Message.ConsigneeState;
            QueryValues[j++] = Message.ConsigneeCountry;
            QueryValues[j++] = Message.ConsigneePostalCode;
            QueryValues[j++] = Message.TransferDestAirport;
            QueryValues[j++] = Message.DomesticIdentifier;
            QueryValues[j++] = Message.BondedCarrierID;
            QueryValues[j++] = Message.OnwardCarrier;
            QueryValues[j++] = Message.BondedPremisesIdentifier;
            QueryValues[j++] = Message.InBondControlNumber;
            QueryValues[j++] = Message.OriginOfGoods;
            if (Message.OriginOfGoods == "")
            { QueryValues[j++] = "0"; }
            else
            {
                QueryValues[j++] = Message.DeclaredValue;
            }
            QueryValues[j++] = Message.CurrencyCode;
            QueryValues[j++] = Message.CommodityCode;
            QueryValues[j++] = Message.LineIdentifier;
            QueryValues[j++] = Message.AmendmentCode;
            QueryValues[j++] = Message.AmendmentExplanation;
            QueryValues[j++] = Message.DeptImportingCarrier;
            QueryValues[j++] = Message.DeptFlightNumber;
            QueryValues[j++] = Message.DeptScheduledArrivalDate;
            QueryValues[j++] = Message.LiftoffDate;
            QueryValues[j++] = Message.LiftoffTime;
            QueryValues[j++] = Message.DeptActualImportingCarrier;
            QueryValues[j++] = Message.DeptActualFlightNumber;
            QueryValues[j++] = Message.ASNStatusCode;
            QueryValues[j++] = Message.ASNActionExplanation;
            QueryValues[j++] = Message.CSNActionCode;
            QueryValues[j++] = Message.CSNPieces;
            QueryValues[j++] = Message.TransactionDate;
            QueryValues[j++] = Message.TransactionTime;
            QueryValues[j++] = Message.CSNEntryType;
            QueryValues[j++] = Message.CSNEntryNumber;
            QueryValues[j++] = Message.CSNRemarks;
            QueryValues[j++] = Message.ErrorCode;
            QueryValues[j++] = Message.ErrorMessage;
            QueryValues[j++] = Message.StatusRequestCode;
            QueryValues[j++] = Message.StatusAnswerCode;
            QueryValues[j++] = Message.Information;
            QueryValues[j++] = Message.ERFImportingCarrier;
            QueryValues[j++] = Message.ERFFlightNumber;
            QueryValues[j++] = Message.ERFDate;
            QueryValues[j++] = Message.Message;
            QueryValues[j++] = DateTime.Now.ToString();
            QueryValues[j++] = "Air AMS";
            QueryValues[j++] = DateTime.Now.ToString();
            QueryValues[j++] = "Air AMS";


            if (db.InsertData("SP_UpdateInboxCustomsMessage", QueryNames, QueryTypes, QueryValues))
            {
                return true;
            }
            else
            { return false; }
            return false;

        }
        catch (Exception ex)
        { return false; }
    }
        #endregion
    #endregion

    //PHL Messaging
    #region Decode PHL message
    public static bool decodereceivePHL(string phlmsg, ref MessageData.fhlinfo phldata, ref MessageData.consignmnetinfo[] consinfo, ref MessageData.customsextrainfo[] custominfo)
    {

        bool flag = false;
        try
        {
            string lastrec = "NA";
            int line = 0;
            try
            {
                if (phlmsg.StartsWith("PHL", StringComparison.OrdinalIgnoreCase))
                {
                    string[] str = phlmsg.Split('$');
                    if (str.Length > 3)
                    {
                        for (int i = 0; i < str.Length; i++)
                        {

                            flag = true;
                            #region Line 1 version
                            if (str[i].StartsWith("PHL", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    phldata.fhlversionnum = msg[1];
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 2 awb consigment details
                            if (str[i].StartsWith("MBI", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    lastrec = "AWB";
                                    line = 0;
                                    string[] msg = str[i].Split('/');
                                    //0th element
                                    string[] decmes = msg[1].Split('-');
                                    phldata.airlineprefix = decmes[0];
                                    phldata.awbnum = decmes[1].Substring(0, decmes[1].Length - 6);
                                    phldata.origin = decmes[1].Substring(decmes[1].Length - 6, 3);
                                    phldata.dest = decmes[1].Substring(decmes[1].Length - 3, 3);
                                    //1
                                    if (msg[2].Length > 0)
                                    {
                                        int k = 0;
                                        char lastchr = 'A';
                                        char[] arr = msg[2].ToCharArray();
                                        string[] strarr = new string[arr.Length];
                                        for (int j = 0; j < arr.Length; j++)
                                        {
                                            if ((char.IsNumber(arr[j])) || (arr[j].Equals('.')))
                                            {//number                            
                                                if (lastchr == 'N')
                                                    k--;
                                                strarr[k] = strarr[k] + arr[j].ToString();
                                                lastchr = 'N';
                                            }
                                            if (char.IsLetter(arr[j]))
                                            {//letter
                                                if (lastchr == 'L')
                                                    k--;
                                                strarr[k] = strarr[k] + arr[j].ToString();
                                                lastchr = 'L';
                                            }
                                            k++;
                                        }
                                        phldata.consigntype = strarr[0];
                                        phldata.pcscnt = strarr[1];//int.Parse(strarr[1]);
                                        phldata.weightcode = strarr[2];
                                        phldata.weight = strarr[3];//float.Parse(strarr[3]);                                            
                                    }
                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            #region  line 3 onwards check consignment details
                            if (str[i].StartsWith("HBS", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    lastrec = "AWB";
                                    line = 0;
                                    decodePHLconsigmentdetails(str[i], ref consinfo);
                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            #region  line 4 Free Text
                            if (str[i].StartsWith("TXT", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = "TXT";
                                    line = 0;
                                    consinfo[consinfo.Length - 1].freetextGoodDesc = consinfo[consinfo.Length - 1].freetextGoodDesc + msg[1] + ",";
                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            #region  line 4 Harmonised Tariff Schedule
                            if (str[i].StartsWith("HTS", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = "HTS";
                                    line = 0;
                                    consinfo[consinfo.Length - 1].commodity = consinfo[consinfo.Length - 1].commodity + msg[1] + ",";
                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            #region Line5 custom extra info
                            if (str[i].StartsWith("OCI", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 0)
                                {
                                    lastrec = "OCI";
                                    MessageData.customsextrainfo custom = new MessageData.customsextrainfo("");
                                    custom.IsoCountryCodeOci = msg[1];
                                    custom.InformationIdentifierOci = msg[2];
                                    custom.CsrIdentifierOci = msg[3];
                                    custom.SupplementaryCsrIdentifierOci = msg[4];
                                    custom.consigref = awbref;
                                    Array.Resize(ref custominfo, custominfo.Length + 1);
                                    custominfo[custominfo.Length - 1] = custom;
                                }
                            }
                            #endregion

                            #region Line 5 Shipper Infor
                            if (str[i].StartsWith("SHP", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg.Length > 1)
                                    {
                                        phldata.shippername = msg[1];

                                    }
                                }
                                catch (Exception e10)
                                { }
                            }
                            #endregion

                            #region Line 6 Consignee
                            if (str[i].StartsWith("CNE", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg.Length > 1)
                                    {
                                        phldata.consname = msg[1];
                                    }
                                }
                                catch (Exception e10)
                                { }
                            }
                            #endregion

                            #region Line 11 Charge declaration
                            if (str[i].StartsWith("CVD", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 1)
                                {
                                    try
                                    {
                                        phldata.currency = msg[1];
                                        phldata.chargecode = msg[2].Length > 0 ? msg[2] : "";
                                        phldata.chargedec = msg[3].Length > 0 ? msg[3] : "";
                                        phldata.declaredvalue = msg[4];
                                        phldata.declaredcustomvalue = msg[5];
                                        phldata.insuranceamount = msg[6];
                                    }
                                    catch (Exception ex) { }
                                }
                            }
                            #endregion

                            #region Other Info
                            if (str[i].StartsWith("/"))
                            {
                                string[] msg = str[i].Split('/');
                                try
                                {
                                    #region SHP Data
                                    if (lastrec == "SHP")
                                    {
                                        line++;
                                        if (line == 1)
                                        {
                                            phldata.shipperadd = msg[1].Length > 0 ? msg[1] : "";

                                        }
                                        if (line == 2)
                                        {
                                            phldata.shipperplace = msg[1].Length > 0 ? msg[1] : "";
                                            phldata.shipperstate = msg[2].Length > 0 ? msg[2] : "";
                                        }
                                        if (line == 3)
                                        {
                                            phldata.shippercountrycode = msg[1].Length > 0 ? msg[1] : "";
                                            phldata.shipperpostcode = msg[2].Length > 0 ? msg[2] : "";
                                            phldata.shippercontactidentifier = msg[3].Length > 0 ? msg[3] : "";
                                            phldata.shippercontactnum = msg[4].Length > 0 ? msg[4] : "";

                                        }

                                    }
                                    #endregion

                                    #region CNE Data
                                    if (lastrec == "CNE")
                                    {
                                        line++;
                                        if (line == 1)
                                        {
                                            phldata.consadd = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                        if (line == 2)
                                        {
                                            phldata.consplace = msg[1].Length > 0 ? msg[1] : "";
                                            phldata.consstate = msg[2].Length > 0 ? msg[2] : "";
                                        }
                                        if (line == 3)
                                        {
                                            phldata.conscountrycode = msg[1].Length > 0 ? msg[1] : "";
                                            phldata.conspostcode = msg[2].Length > 0 ? msg[2] : "";
                                            phldata.conscontactidentifier = msg[3].Length > 0 ? msg[3] : "";
                                            phldata.conscontactnum = msg[4].Length > 0 ? msg[4] : "";
                                        }

                                    }
                                    #endregion

                                    #region Commodity
                                    if (lastrec == "HTS")
                                    {
                                        if (str[i].Length > 1)
                                        {
                                            consinfo[consinfo.Length - 1].commodity = consinfo[consinfo.Length - 1].commodity + msg[1] + ",";
                                        }
                                    }
                                    #endregion

                                    #region freetextGoodDesc
                                    if (lastrec == "TXT")
                                    {
                                        if (str[i].Length > 1)
                                        {
                                            consinfo[consinfo.Length - 1].freetextGoodDesc = consinfo[consinfo.Length - 1].freetextGoodDesc + msg[1];
                                        }
                                    }
                                    #endregion

                                    #region Splhandling
                                    if (lastrec == "AWB")
                                    {
                                        if (str[i].Length > 1)
                                        {
                                            consinfo[consinfo.Length - 1].splhandling = str[i].Replace('/', ',');
                                        }
                                        lastrec = "NA";
                                    }
                                    #endregion

                                    #region OCI
                                    if (lastrec == "OCI")
                                    {
                                        string[] msgdata = str[i].Split('/');
                                        if (msgdata.Length > 0)
                                        {
                                            lastrec = "OCI";
                                            MessageData.customsextrainfo custom = new MessageData.customsextrainfo("");
                                            custom.IsoCountryCodeOci = msgdata[1];
                                            custom.InformationIdentifierOci = msgdata[2];
                                            custom.CsrIdentifierOci = msgdata[3];
                                            custom.SupplementaryCsrIdentifierOci = msgdata[4];
                                            Array.Resize(ref custominfo, custominfo.Length + 1);
                                            custominfo[custominfo.Length - 1] = custom;
                                        }
                                    }
                                    #endregion
                                }
                                catch (Exception e13)
                                { }
                            }
                            #endregion
                        }
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }
    #endregion

    #region Encode PHL
    public static string EncodePHLforsend(ref MessageData.fhlinfo phldata, ref MessageData.consignmnetinfo[] consinfo, ref MessageData.customsextrainfo[] custominfo)
    {
        string phl = null, flightcons = "";
        try
        {
            #region Line 1
            string line1 = "PHL" + "/" + phldata.fhlversionnum;
            #endregion

            #region Line 2
            string line2 = "MBI/" + phldata.airlineprefix + "-" + phldata.awbnum + phldata.origin + phldata.dest + "/" + phldata.consigntype + phldata.pcscnt + phldata.weightcode + phldata.weight;
            #endregion line 2

            #region Line3
            string line3 = FHLPartBuilder(ref consinfo, ref custominfo);
            #endregion

            //SHP
            #region Line 5
            string line5 = "";
            string str1 = "", str2 = "", str3 = "", str4 = "";
            try
            {
                if (phldata.shippername.Length > 0)
                {
                    str1 = "/" + phldata.shippername;
                }
                if (phldata.shipperadd.Length > 0)
                {
                    str2 = "/" + phldata.shipperadd;
                }

                if (phldata.shipperplace.Length > 0 || phldata.shipperstate.Length > 0)
                {
                    str3 = "/" + phldata.shipperplace + "/" + phldata.shipperstate;
                }
                if (phldata.shippercountrycode.Length > 0 || phldata.shipperpostcode.Length > 0 || phldata.shippercontactidentifier.Length > 0 || phldata.shippercontactnum.Length > 0)
                {
                    str4 = "/" + phldata.shippercountrycode + "/" + phldata.shipperpostcode + "/" + phldata.shippercontactidentifier + "/" + phldata.shippercontactnum;
                }

                if (str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line5 = "SHP/";
                    if (str4.Length > 0)
                    {
                        line5 = line5.Trim() + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/') + "\r\n/" + str4.Trim('/');
                    }
                    else if (str3.Length > 0)
                    {
                        line5 = line5.Trim() + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line5 = line5.Trim() + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line5 = line5.Trim() + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            //CNE
            #region Line 6
            string line6 = "";
            str1 = "";
            str2 = "";
            str3 = "";
            str4 = "";
            try
            {
                if (phldata.consname.Length > 0)
                {
                    str1 = "/" + phldata.consname;
                }
                if (phldata.consadd.Length > 0)
                {
                    str2 = "/" + phldata.consadd;
                }

                if (phldata.consplace.Length > 0 || phldata.consstate.Length > 0)
                {
                    str3 = "/" + phldata.consplace + "/" + phldata.consstate;
                }
                if (phldata.conscountrycode.Length > 0 || phldata.conspostcode.Length > 0 || phldata.conscontactidentifier.Length > 0 || phldata.conscontactnum.Length > 0)
                {
                    str4 = "/" + phldata.conscountrycode + "/" + phldata.conspostcode + "/" + phldata.conscontactidentifier + "/" + phldata.conscontactnum;
                }

                if (str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line6 = "CNE/";
                    if (str4.Length > 0)
                    {
                        line6 = line6.Trim() + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/') + "\r\n/" + str4.Trim('/');
                    }
                    else if (str3.Length > 0)
                    {
                        line6 = line6.Trim() + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line6 = line6.Trim() + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line6 = line6.Trim() + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            //CVD
            #region Line 9
            string line9 = "";
            if (phldata.currency.Length > 0 || phldata.chargecode.Length > 0 || phldata.declaredvalue.Length > 0 || phldata.declaredcustomvalue.Length > 0 || phldata.insuranceamount.Length > 0)
            {
                line9 = "CVD/" + phldata.currency + "/" + phldata.chargecode + "/PP/" + phldata.declaredvalue + "/" + phldata.declaredcustomvalue + "/" + phldata.insuranceamount + "";
            }
            #endregion


            #region BuildPHL
            phl = line1.Trim('/') + "\r\n" + line2.Trim() + "\r\n" + line3.Trim();
            if (line5.Length > 0)
            {
                phl = phl + "\r\n" + line5.Trim('/');
            }
            if (line6.Length > 0)
            {
                phl = phl + "\r\n" + line6.Trim('/');
            }
            if (line9.Length > 0)
            {
                phl = phl + "\r\n" + line9.Trim('/');
            }
            #endregion

        }
        catch (Exception ex)
        {
            phl = "";
        }
        return phl;
    }

    #endregion

    #region Decode Consigment Details
    public static void decodePHLconsigmentdetails(string inputstr, ref MessageData.consignmnetinfo[] consinfo)
    {
        MessageData.consignmnetinfo consig = new MessageData.consignmnetinfo("");
        try
        {
            string[] msg = inputstr.Split('/');

            //consinfo[num] = new MessageData.consignmnetinfo("");
            //consig.airlineprefix = msg[1].Substring(0, 3);
            consig.awbnum = msg[1];

            consig.origin = msg[2].Substring(0, 3);
            consig.dest = msg[2].Substring(3);

            consig.consigntype = "";
            consig.pcscnt = msg[3];//int.Parse(strarr[1]);
            consig.weightcode = msg[4].Substring(0, 1);
            consig.weight = msg[4].Substring(1);

            if (msg.Length > 4)
            {
                consig.slac = msg[5];
            }
            if (msg.Length > 5)
            {
                consig.manifestdesc = msg[6];
            }

        }
        catch (Exception ex) { }
        Array.Resize(ref consinfo, consinfo.Length + 1);
        consinfo[consinfo.Length - 1] = consig;
    }
    #endregion

    //PWB Messaging
    #region Decode PWB Message
    public static bool decodeReceivePWB(string pwbmsg, ref MessageData.fwbinfo pwbdata, ref MessageData.othercharges[] pwbOtherCharge, ref MessageData.otherserviceinfo[] othinfoarray, ref MessageData.RateDescription[] pwbrate, ref MessageData.customsextrainfo[] custominfo, ref MessageData.dimensionnfo[] objDimension)
    {
        bool flag = false;
        try
        {
            string lastrec = "NA";
            int line = 0;//, consignmnetnum = 0; 
            //int count=0;
            try
            {
                if (pwbmsg.StartsWith("PWB", StringComparison.OrdinalIgnoreCase))
                {
                    // ffrmsg = ffrmsg.Replace("\r\n","$");
                    string[] str = pwbmsg.Split('$');
                    if (str.Length > 3)
                    {
                        for (int i = 0; i < str.Length; i++)
                        {

                            flag = true;

                            #region Line 1
                            if (str[i].StartsWith("PWB", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                pwbdata.fwbversionnum = msg[1];
                            }
                            #endregion

                            #region Line 2 awb consigment details
                            if (i == 1)
                            {
                                try
                                {
                                    lastrec = "AWB";
                                    line = 0;
                                    string[] msg = str[i].Split('/');
                                    //0th element
                                    string[] decmes = msg[0].Split('-');
                                    pwbdata.airlineprefix = decmes[0];
                                    pwbdata.awbnum = decmes[1].Substring(0, decmes[1].Length - 6);
                                    pwbdata.origin = decmes[1].Substring(decmes[1].Length - 6, 3);
                                    pwbdata.dest = decmes[1].Substring(decmes[1].Length - 3, 3);
                                    //1
                                    if (msg[1].Length > 0)
                                    {
                                        int k = 0;
                                        char lastchr = 'A';
                                        char[] arr = msg[1].ToCharArray();
                                        string[] strarr = new string[arr.Length];
                                        for (int j = 0; j < arr.Length; j++)
                                        {
                                            if ((char.IsNumber(arr[j])) || (arr[j].Equals('.')))
                                            {//number                            
                                                if (lastchr == 'N')
                                                    k--;
                                                strarr[k] = strarr[k] + arr[j].ToString();
                                                lastchr = 'N';
                                            }
                                            if (char.IsLetter(arr[j]))
                                            {//letter
                                                if (lastchr == 'L')
                                                    k--;
                                                strarr[k] = strarr[k] + arr[j].ToString();
                                                lastchr = 'L';
                                            }
                                            k++;
                                        }
                                        pwbdata.consigntype = strarr[0];
                                        pwbdata.pcscnt = strarr[1];//int.Parse(strarr[1]);
                                        pwbdata.weightcode = strarr[2];
                                        pwbdata.weight = strarr[3];//float.Parse(strarr[3]);
                                        for (k = 4; k < strarr.Length; k += 2)
                                        {
                                            if (strarr[k] != null)
                                            {
                                                if (strarr[k] == "DG")
                                                {
                                                    pwbdata.densityindicator = strarr[k];
                                                    pwbdata.densitygrp = strarr[k];
                                                }
                                                else//if (strarr[k + 1].Length > 3)
                                                {
                                                    pwbdata.volumecode = strarr[k];
                                                    pwbdata.volumeamt = strarr[k + 1];
                                                }
                                            }
                                        }
                                    }


                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }
                            #endregion

                            #region Line 3 Flight Booking
                            if (str[i].StartsWith("FLT", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 1)
                                    {
                                        pwbdata.carriercode = msg[1].Substring(0, 2);
                                        pwbdata.fltnum = msg[1].Substring(2);
                                        pwbdata.fltday = msg[2];
                                        if (msg.Length > 2)
                                        {
                                            pwbdata.carriercode = pwbdata.carriercode + "," + msg[3].Substring(0, 2);
                                            pwbdata.fltnum = pwbdata.fltnum + "," + msg[3].Substring(2);
                                            pwbdata.fltday = pwbdata.fltday + "," + msg[4];
                                        }
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 4 Routing
                            if (str[i].StartsWith("RTG", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string airportcity = "", carrier = "";
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 1)
                                    {
                                        for (int k = 1; k < msg.Length; k++)
                                        {
                                            airportcity = airportcity + msg[k].Substring(0, 3) + ",";
                                            carrier = carrier + msg[k].Substring(3) + ",";
                                        }
                                        pwbdata.routingairportcitycode = airportcity.Trim(',');
                                        pwbdata.routingcarriercode = carrier.Trim(',');
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 5 Shipper Infor
                            if (str[i].StartsWith("SHP", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg.Length > 1)
                                    {
                                        pwbdata.shipperaccnum = msg[1];

                                    }
                                }
                                catch (Exception e10)
                                { }
                            }
                            #endregion

                            #region Line 6 Consignee
                            if (str[i].StartsWith("CNE", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg.Length > 1)
                                    {
                                        pwbdata.consaccnum = msg[1];
                                    }
                                }
                                catch (Exception e10)
                                { }
                            }
                            #endregion

                            #region Line 7 Agent
                            if (str[i].StartsWith("AGT", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg.Length > 1)
                                    {
                                        pwbdata.agentaccnum = msg[1];
                                        pwbdata.agentIATAnumber = msg[2].Length > 0 ? msg[2] : "";
                                        if (msg.Length > 2)
                                        {
                                            pwbdata.agentCASSaddress = msg[3].Length > 0 ? msg[3] : "";
                                        }
                                        if (msg.Length > 3)
                                        {
                                            pwbdata.agentParticipentIdentifier = msg[4].Length > 0 ? msg[4] : "";
                                        }
                                    }
                                }
                                catch (Exception e10)
                                { }
                            }
                            #endregion

                            #region Line 8 Special Service request
                            if (str[i].StartsWith("SSR", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg[1].Length > 0)
                                    {
                                        pwbdata.specialservicereq1 = msg[1];
                                    }

                                }
                                catch (Exception e5)
                                { }
                            }
                            #endregion

                            #region Line 9 Notify
                            if (str[i].StartsWith("NFY", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg.Length > 0)
                                    {
                                        pwbdata.notifyname = msg[1].Length > 0 ? msg[1] : "";
                                    }
                                }
                                catch (Exception e10)
                                { }
                            }
                            #endregion

                            #region Line 10 Accounting Information
                            if (str[i].StartsWith("ACC", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                lastrec = msg[0];
                                line = 0;
                                if (msg.Length > 1)
                                {
                                    pwbdata.accountinginfoidentifier = pwbdata.accountinginfoidentifier + msg[1] + ",";
                                    pwbdata.accountinginfo = pwbdata.accountinginfo + msg[2] + ",";
                                }
                            }
                            #endregion

                            #region Line 11 Charge declaration
                            if (str[i].StartsWith("CVD", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 1)
                                    {
                                        pwbdata.currency = msg[1];
                                        pwbdata.chargecode = msg[2].Length > 0 ? msg[2] : "";
                                        pwbdata.chargedec = msg[3].Length > 0 ? msg[3] : "";
                                        pwbdata.declaredvalue = msg[4];
                                        pwbdata.declaredcustomvalue = msg[5];
                                        pwbdata.insuranceamount = msg[6];
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 12 Rate Description
                            if (str[i].StartsWith("RTD", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 1)
                                {
                                    lastrec = msg[0];
                                    line = 0;
                                    MessageData.RateDescription rate = new MessageData.RateDescription("");
                                    try
                                    {
                                        rate.linenum = msg[1];
                                        for (int k = 2; k < msg.Length; k++)
                                        {
                                            if (msg[k].Substring(0, 1).Equals("P", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.pcsidentifier = msg[k].Substring(0, 1);
                                                rate.numofpcs = msg[k].Substring(1);
                                            }
                                            if (msg[k].Substring(0, 1).Equals("K", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.weightindicator = msg[k].Substring(0, 1);
                                                rate.weight = msg[k].Substring(1).Length > 0 ? msg[k].Substring(1) : "0";
                                            }
                                            if (msg[k].Substring(0, 1).Equals("C", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.rateclasscode = msg[k].Substring(1);
                                            }
                                            if (msg[k].Substring(0, 1).Equals("S", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.commoditynumber = msg[k].Substring(1);
                                            }
                                            if (msg[k].Substring(0, 1).Equals("W", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.awbweight = msg[k].Substring(1);
                                            }
                                            if (msg[k].Substring(0, 1).Equals("R", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.chargerate = msg[k].Substring(1);
                                            }
                                            if (msg[k].Substring(0, 1).Equals("T", StringComparison.OrdinalIgnoreCase))
                                            {
                                                rate.chargeamt = msg[k].Substring(1);
                                            }
                                        }
                                        Array.Resize(ref pwbrate, pwbrate.Length + 1);
                                        pwbrate[pwbrate.Length - 1] = rate;
                                    }
                                    catch (Exception ex) { }
                                }
                            }
                            #endregion

                            #region Line 13 Other Charges
                            if (str[i].StartsWith("OTH", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    lastrec = "OTH";
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 1)
                                    {
                                        string[] opstr = stringsplitter(msg[2]);
                                        for (int k = 0; k < opstr.Length; k = k + 2)
                                        {
                                            if (opstr[k].Length > 0)
                                            {
                                                MessageData.othercharges oth = new MessageData.othercharges("");
                                                oth.otherchargecode = opstr[k].Substring(0, 2);
                                                oth.entitlementcode = opstr[k].Substring(2);
                                                oth.chargeamt = opstr[k + 1];
                                                Array.Resize(ref pwbOtherCharge, pwbOtherCharge.Length + 1);
                                                pwbOtherCharge[pwbOtherCharge.Length - 1] = oth;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 14 Prepaid Charge Summery
                            if (str[i].StartsWith("PPD", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    lastrec = "PPD";
                                    string[] msg = str[i].Split('/');
                                    for (int k = 1; k < msg.Length; k++)
                                    {
                                        if (msg[k].Substring(0, 2).Equals("WT", StringComparison.OrdinalIgnoreCase))
                                        {
                                            pwbdata.PPweightCharge = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("VC", StringComparison.OrdinalIgnoreCase))
                                        {
                                            pwbdata.PPValuationCharge = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("TX", StringComparison.OrdinalIgnoreCase))
                                        {
                                            pwbdata.PPTaxesCharge = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("OA", StringComparison.OrdinalIgnoreCase))
                                        {
                                            pwbdata.PPOCDA = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("OC", StringComparison.OrdinalIgnoreCase))
                                        {
                                            pwbdata.PPOCDC = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("CT", StringComparison.OrdinalIgnoreCase))
                                        {
                                            pwbdata.PPTotalCharges = msg[k].Substring(2);
                                        }
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 15 Collect Charge Summery
                            if (str[i].StartsWith("COL", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    lastrec = "COL";
                                    string[] msg = str[i].Split('/');
                                    for (int k = 1; k < msg.Length; k++)
                                    {
                                        if (msg[k].Substring(0, 2).Equals("WT", StringComparison.OrdinalIgnoreCase))
                                        {
                                            pwbdata.CCweightCharge = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("VC", StringComparison.OrdinalIgnoreCase))
                                        {
                                            pwbdata.CCValuationCharge = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("TX", StringComparison.OrdinalIgnoreCase))
                                        {
                                            pwbdata.CCTaxesCharge = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("OA", StringComparison.OrdinalIgnoreCase))
                                        {
                                            pwbdata.CCOCDA = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("OC", StringComparison.OrdinalIgnoreCase))
                                        {
                                            pwbdata.CCOCDC = msg[k].Substring(2);
                                        }
                                        if (msg[k].Substring(0, 2).Equals("CT", StringComparison.OrdinalIgnoreCase))
                                        {
                                            pwbdata.CCTotalCharges = msg[k].Substring(2);
                                        }
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 16 Shipper Certification
                            if (str[i].StartsWith("CER", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                if (msg[1].Length > 0)
                                {
                                    pwbdata.shippersignature = msg[1];
                                }
                            }
                            #endregion

                            #region Line 17 Carrier Execution
                            if (str[i].StartsWith("ISU", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                try
                                {
                                    if (msg.Length > 0)
                                    {

                                        pwbdata.carrierdate = msg[1].Substring(0, 2);
                                        pwbdata.carriermonth = msg[1].Substring(2, 3);
                                        pwbdata.carrieryear = msg[1].Substring(5);
                                        pwbdata.carrierplace = msg[2];
                                        pwbdata.carriersignature = msg[3];
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 18 Other service info
                            if (str[i].StartsWith("OSI", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    lastrec = msg[0];
                                    line = 0;
                                    if (msg[1].Length > 0)
                                    {
                                        Array.Resize(ref othinfoarray, othinfoarray.Length + 1);
                                        othinfoarray[othinfoarray.Length - 1].otherserviceinfo1 = msg[1];

                                    }

                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 19 Charge in destination currency
                            if (str[i].StartsWith("CDC", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 0)
                                    {
                                        pwbdata.cccurrencycode = msg[1].Substring(0, 3);
                                        pwbdata.ccexchangerate = msg[1].Substring(3);
                                        for (int j = 2; j < msg.Length; j++)
                                            pwbdata.ccchargeamt += msg[j] + ",";
                                    }
                                }
                                catch (Exception ex) { }
                            }
                            #endregion

                            #region Line 20 Sender Reference
                            if (str[i].StartsWith("REF", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 0)
                                    {

                                        if (msg[1].Length > 1)
                                        {
                                            try
                                            {
                                                pwbdata.senderairport = msg[1].Substring(0, 3);
                                                pwbdata.senderofficedesignator = msg[1].Substring(3, 2);
                                                pwbdata.sendercompanydesignator = msg[1].Substring(5);
                                            }
                                            catch (Exception ex) { }
                                        }
                                        pwbdata.senderFileref = msg[2];
                                        pwbdata.senderParticipentIdentifier = msg[3];
                                        pwbdata.senderParticipentCode = msg[4];
                                        pwbdata.senderPariticipentAirport = msg[5];
                                    }

                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 21 Custom Origin
                            if (str[i].StartsWith("COR", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg[1].Length > 0)
                                    {
                                        pwbdata.customorigincode = msg[1];
                                    }

                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 22 Commission Information
                            if (str[i].StartsWith("COI", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 0)
                                    {
                                        pwbdata.commisioncassindicator = msg[1];
                                        for (int k = 2; k < msg.Length; k++)
                                            pwbdata.commisionCassSettleAmt += msg[k] + ",";
                                    }

                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 23 Sales Incentive Info
                            if (str[i].StartsWith("SII", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg[1].Length > 0)
                                    {
                                        pwbdata.saleschargeamt = msg[1];
                                        pwbdata.salescassindicator = msg[2];
                                    }

                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 24 Agent Reference
                            if (str[i].StartsWith("ARD", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg[1].Length > 0)
                                    {
                                        pwbdata.agentfileref = msg[1];
                                    }

                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 25 Special Handling
                            if (str[i].StartsWith("SPH", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg[1].Length > 0)
                                    {
                                        string temp = str[i].Replace("/", "");
                                        pwbdata.splhandling = temp.Replace("SPH", "");
                                    }

                                }
                                catch (Exception e5)
                                { }
                            }
                            #endregion

                            #region Line 26 Nominated Handling Party
                            if (str[i].StartsWith("NOM", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 0)
                                    {
                                        pwbdata.handlingname = msg[1];
                                        pwbdata.handlingplace = msg[2];
                                    }

                                }
                                catch (Exception e5)
                                { }
                            }
                            #endregion

                            #region Line 27 Shipment Reference Info
                            if (str[i].StartsWith("SRI", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 0)
                                    {
                                        pwbdata.shiprefnum = msg[1];
                                        pwbdata.supplemetryshipperinfo1 = msg[2];
                                        pwbdata.supplemetryshipperinfo2 = msg[3];
                                    }
                                }
                                catch (Exception e6) { }
                            }
                            #endregion

                            #region Line 28 Other Service Information
                            if (str[i].StartsWith("OPI", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    string[] msg = str[i].Split('/');
                                    if (msg.Length > 0)
                                    {
                                        lastrec = msg[0];
                                        pwbdata.othparticipentname = msg[1];
                                    }

                                }
                                catch (Exception e5)
                                { }
                            }
                            #endregion

                            #region Line 29 custom extra info
                            if (str[i].StartsWith("OCI", StringComparison.OrdinalIgnoreCase))
                            {
                                string[] msg = str[i].Split('/');
                                if (msg.Length > 0)
                                {
                                    lastrec = "OCI";
                                    MessageData.customsextrainfo custom = new MessageData.customsextrainfo("");
                                    custom.IsoCountryCodeOci = msg[1];
                                    custom.InformationIdentifierOci = msg[2];
                                    custom.CsrIdentifierOci = msg[3];
                                    custom.SupplementaryCsrIdentifierOci = msg[4];
                                    Array.Resize(ref custominfo, custominfo.Length + 1);
                                    custominfo[custominfo.Length - 1] = custom;
                                }
                            }
                            #endregion

                            #region Second Line
                            if (str[i].StartsWith("/"))
                            {
                                string[] msg = str[i].Split('/');
                                try
                                {
                                    #region SHP Data
                                    if (lastrec == "SHP")
                                    {
                                        line++;
                                        if (line == 1)
                                        {
                                            pwbdata.shippername = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                        if (line == 2)
                                        {
                                            pwbdata.shipperadd = msg[1].Length > 0 ? msg[1] : "";

                                        }
                                        if (line == 3)
                                        {
                                            pwbdata.shipperplace = msg[1].Length > 0 ? msg[1] : "";
                                            pwbdata.shipperstate = msg[2].Length > 0 ? msg[2] : "";
                                        }
                                        if (line == 4)
                                        {
                                            pwbdata.shippercountrycode = msg[1].Length > 0 ? msg[1] : "";
                                            pwbdata.shipperpostcode = msg[2].Length > 0 ? msg[2] : "";
                                            pwbdata.shippercontactidentifier = msg[3].Length > 0 ? msg[3] : "";
                                            pwbdata.shippercontactnum = msg[4].Length > 0 ? msg[4] : "";

                                        }

                                    }
                                    #endregion

                                    #region CNE Data
                                    if (lastrec == "CNE")
                                    {
                                        line++;
                                        if (line == 1)
                                        {
                                            pwbdata.consname = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                        if (line == 2)
                                        {
                                            pwbdata.consadd = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                        if (line == 3)
                                        {
                                            pwbdata.consplace = msg[1].Length > 0 ? msg[1] : "";
                                            pwbdata.consstate = msg[2].Length > 0 ? msg[2] : "";
                                        }
                                        if (line == 4)
                                        {
                                            pwbdata.conscountrycode = msg[1].Length > 0 ? msg[1] : "";
                                            pwbdata.conspostcode = msg[2].Length > 0 ? msg[2] : "";
                                            pwbdata.conscontactidentifier = msg[3].Length > 0 ? msg[3] : "";
                                            pwbdata.conscontactnum = msg[4].Length > 0 ? msg[4] : "";
                                        }

                                    }
                                    #endregion

                                    #region AgentData
                                    if (lastrec == "AGT")
                                    {
                                        line++;
                                        if (line == 1)
                                        {
                                            pwbdata.agentname = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                        if (line == 2)
                                        {
                                            pwbdata.agentplace = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                    }
                                    #endregion

                                    #region SSR 2
                                    if (lastrec == "SSR")
                                    {
                                        pwbdata.specialservicereq2 = msg[1].Length > 0 ? msg[1] : "";
                                        lastrec = "NA";
                                    }
                                    #endregion

                                    #region Also notify Data
                                    if (lastrec == "NFY")
                                    {
                                        line++;
                                        if (line == 1)
                                        {
                                            pwbdata.notifyadd = msg[1].Length > 0 ? msg[1] : "";
                                        }
                                        if (line == 2)
                                        {
                                            pwbdata.notifyplace = msg[1].Length > 0 ? msg[1] : "";
                                            pwbdata.notifystate = msg[2].Length > 0 ? msg[2] : "";
                                        }
                                        if (line == 3)
                                        {
                                            pwbdata.notifycountrycode = msg[1].Length > 0 ? msg[1] : "";
                                            pwbdata.notifypostcode = msg[2].Length > 0 ? msg[2] : "";
                                            pwbdata.notifycontactidentifier = msg[3].Length > 0 ? msg[3] : "";
                                            pwbdata.notifycontactnum = msg[4].Length > 0 ? msg[4] : "";
                                        }
                                    }
                                    #endregion

                                    #region Account Info
                                    if (lastrec.Equals("ACC", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (msg.Length > 1)
                                        {
                                            pwbdata.accountinginfoidentifier = pwbdata.accountinginfoidentifier + msg[1] + ",";
                                            pwbdata.accountinginfo = pwbdata.accountinginfo + msg[2] + ",";
                                        }
                                    }
                                    #endregion

                                    #region RateData
                                    if (lastrec.Equals("RTD", StringComparison.OrdinalIgnoreCase))
                                    {
                                        try
                                        {
                                            if (msg.Length > 1)
                                            {
                                                int res, k = 1;
                                                if (int.TryParse(msg[k].ToString(), out res))
                                                {
                                                    k++;
                                                }
                                                if (msg[k].Equals("NG", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbrate[pwbrate.Length - 1].goodsnature = msg[k + 1];
                                                }
                                                if (msg[k].Equals("NC", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbrate[pwbrate.Length - 1].goodsnature1 = msg[k + 1];
                                                }
                                                if (msg[k].Equals("ND", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    MessageData.dimensionnfo dimension = new MessageData.dimensionnfo("");
                                                    if (msg.Length > 1)
                                                    {
                                                        if (msg[k + 1].Substring(0, 1).Equals("K", StringComparison.OrdinalIgnoreCase) || msg[k + 1].Substring(0, 1).Equals("L", StringComparison.OrdinalIgnoreCase))
                                                        {
                                                            dimension.weight = msg[k + 1].Substring(1);
                                                            k++;
                                                        }
                                                        if (msg[k + 1].Contains('-'))
                                                        {
                                                            string[] substr = msg[k + 1].Split('-');
                                                            try
                                                            {
                                                                if (substr.Length > 0)
                                                                {
                                                                    dimension.mesurunitcode = substr[0].Substring(0, 3);
                                                                    dimension.length = substr[0].Substring(3);
                                                                    dimension.width = substr[1];
                                                                    dimension.height = substr[2];
                                                                    k++;
                                                                }
                                                            }
                                                            catch (Exception ez) { }
                                                        }
                                                        int val;
                                                        if (int.TryParse(msg[k + 1], out val))
                                                        {
                                                            dimension.piecenum = msg[k + 1];
                                                        }
                                                        Array.Resize(ref objDimension, objDimension.Length + 1);
                                                        objDimension[objDimension.Length - 1] = dimension;
                                                    }

                                                }
                                                if (msg[k].Equals("NV", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbrate[pwbrate.Length - 1].volcode = msg[k + 1].Substring(0, 2);
                                                    pwbrate[pwbrate.Length - 1].volamt = msg[k + 1].Substring(2);
                                                }
                                                if (msg[k].Equals("NU", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbrate[pwbrate.Length - 1].uldtype = msg[k + 1].Substring(0, 3);
                                                    pwbrate[pwbrate.Length - 1].uldserialnum = msg[k + 1].Substring(3, msg[k + 1].Length - 5);
                                                    pwbrate[pwbrate.Length - 1].uldowner = msg[k + 1].Substring(msg[k + 1].Length - 2);
                                                }
                                                if (msg[k].Equals("NS", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbrate[pwbrate.Length - 1].slac = msg[k + 1];
                                                }
                                                if (msg[k].Equals("NH", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbrate[pwbrate.Length - 1].hermonisedcomoditycode = msg[k + 1];
                                                }
                                                if (msg[k].Equals("NO", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbrate[pwbrate.Length - 1].isocountrycode = msg[k + 1];
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        { }
                                    }
                                    #endregion

                                    #region Other Charges
                                    if (lastrec.Equals("OTH", StringComparison.OrdinalIgnoreCase))
                                    {
                                        try
                                        {
                                            string[] opstr = stringsplitter(msg[2]);
                                            for (int k = 0; k < opstr.Length; k = k + 2)
                                            {
                                                MessageData.othercharges oth = new MessageData.othercharges("");
                                                oth.otherchargecode = opstr[k].Substring(0, 2);
                                                oth.entitlementcode = opstr[k].Substring(2);
                                                oth.chargeamt = opstr[k + 1];
                                                Array.Resize(ref pwbOtherCharge, pwbOtherCharge.Length + 1);
                                                pwbOtherCharge[pwbOtherCharge.Length - 1] = oth;
                                            }
                                        }
                                        catch (Exception ex)
                                        { }
                                    }
                                    #endregion

                                    #region Line 14 Collect Charge Summery
                                    if (lastrec.Equals("PPD", StringComparison.OrdinalIgnoreCase))
                                    {
                                        try
                                        {
                                            for (int k = 1; k < msg.Length; k++)
                                            {
                                                if (msg[k].Substring(0, 2).Equals("WT", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbdata.PPweightCharge = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("VC", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbdata.PPValuationCharge = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("TX", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbdata.PPTaxesCharge = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("OA", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbdata.PPOCDA = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("OC", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbdata.PPOCDC = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("CT", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbdata.PPTotalCharges = msg[k].Substring(2);
                                                }
                                            }
                                        }
                                        catch (Exception ex) { }
                                    }
                                    #endregion

                                    #region Line 15 Prepaid Charge Summery
                                    if (lastrec.Equals("COL", StringComparison.OrdinalIgnoreCase))
                                    {
                                        try
                                        {
                                            for (int k = 1; k < msg.Length; k++)
                                            {
                                                if (msg[k].Substring(0, 2).Equals("WT", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbdata.CCweightCharge = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("VC", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbdata.CCValuationCharge = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("TX", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbdata.CCTaxesCharge = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("OA", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbdata.CCOCDA = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("OC", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbdata.CCOCDC = msg[k].Substring(2);
                                                }
                                                if (msg[k].Substring(0, 2).Equals("CT", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    pwbdata.CCTotalCharges = msg[k].Substring(2);
                                                }
                                            }
                                        }
                                        catch (Exception ex) { }
                                    }
                                    #endregion

                                    #region Line 18 OSI 2
                                    if (lastrec == "OSI")
                                    {
                                        othinfoarray[othinfoarray.Length - 1].otherserviceinfo2 = msg[1].Length > 0 ? msg[1] : "";
                                        lastrec = "NA";
                                    }
                                    #endregion


                                    #region OCI
                                    if (lastrec.Equals("OCI", StringComparison.OrdinalIgnoreCase))
                                    {
                                        string[] msgdata = str[i].Split('/');
                                        if (msgdata.Length > 0)
                                        {
                                            MessageData.customsextrainfo custom = new MessageData.customsextrainfo("");
                                            custom.IsoCountryCodeOci = msgdata[1];
                                            custom.InformationIdentifierOci = msgdata[2];
                                            custom.CsrIdentifierOci = msgdata[3];
                                            custom.SupplementaryCsrIdentifierOci = msgdata[4];
                                            Array.Resize(ref custominfo, custominfo.Length + 1);
                                            custominfo[custominfo.Length - 1] = custom;
                                        }
                                    }
                                    #endregion

                                    #region OPI
                                    if (lastrec.Equals("OPI", StringComparison.OrdinalIgnoreCase))
                                    {
                                        string[] msgdata = str[i].Split('/');
                                        if (msgdata.Length > 0)
                                        {
                                            pwbdata.othairport = msgdata[1].Substring(0, 3);
                                            pwbdata.othofficedesignator = msgdata[1].Substring(3, 2);
                                            pwbdata.othcompanydesignator = msgdata[1].Substring(5);
                                            pwbdata.othfilereference = msgdata[2];
                                            pwbdata.othparticipentidentifier = msgdata[3];
                                            pwbdata.othparticipentcode = msgdata[4];
                                            pwbdata.othparticipentairport = msgdata[5];
                                        }
                                    }
                                    #endregion
                                }
                                catch (Exception e13)
                                { }
                            }
                            #endregion
                        }
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }
    #endregion

    #region EncodePWBForSend
    public static string EncodePWBForSend(ref MessageData.fwbinfo pwbdata, ref MessageData.othercharges[] pwbOtherCharge, ref MessageData.otherserviceinfo[] othinfoarray, ref MessageData.RateDescription[] pwbrate, ref MessageData.customsextrainfo[] custominfo)
    {
        string pwbstr = null;
        try
        {
            //PWB
            #region Line 1
            string line1 = "PWB/" + pwbdata.fwbversionnum;
            #endregion

            #region Line 2
            string line2 = pwbdata.airlineprefix + "-" + pwbdata.awbnum + pwbdata.origin + pwbdata.dest + "/" + pwbdata.consigntype + pwbdata.pcscnt + pwbdata.weightcode + pwbdata.weight + pwbdata.volumecode + pwbdata.volumeamt + pwbdata.densityindicator + pwbdata.densitygrp;
            #endregion line 2
            //FLT
            #region Line 3
            string line3 = "";
            if (pwbdata.carriercode.Trim(',').Contains(','))
            {
                string[] carriersplit = pwbdata.carriercode.Split(',');
                string[] fltsplit = pwbdata.fltnum.Split(',');
                string[] daysplit = pwbdata.fltday.Split(',');
                for (int k = 0; k < carriersplit.Length; k++)
                {
                    line3 = line3 + carriersplit[k] + fltsplit[k] + "/" + daysplit[k] + "/";
                }
            }
            else
            {
                line3 = pwbdata.carriercode.Trim(',') + pwbdata.fltnum.Trim(',') + "/" + pwbdata.fltday.Trim(',');
            }
            if (line3.Length > 1)
            {
                line3 = "FLT/" + line3.Trim('/');
            }
            #endregion

            //RTG
            #region Line 4
            string line4 = pwbdata.origin + pwbdata.carriercode;
            if (line4.Length > 1)
            {
                line4 = "RTG/" + line4;
            }
            #endregion

            //SHP
            #region Line 5
            string line5 = "";
            string str1 = "", str2 = "", str3 = "", str4 = "";
            try
            {
                if (pwbdata.shippername.Length > 0)
                {
                    str1 = "/" + pwbdata.shippername;
                }
                if (pwbdata.shipperadd.Length > 0)
                {
                    str2 = "/" + pwbdata.shipperadd;
                }

                if (pwbdata.shipperplace.Length > 0 || pwbdata.shipperstate.Length > 0)
                {
                    str3 = "/" + pwbdata.shipperplace + "/" + pwbdata.shipperstate;
                }
                if (pwbdata.shippercountrycode.Length > 0 || pwbdata.shipperpostcode.Length > 0 || pwbdata.shippercontactidentifier.Length > 0 || pwbdata.shippercontactnum.Length > 0)
                {
                    str4 = "/" + pwbdata.shippercountrycode + "/" + pwbdata.shipperpostcode + "/" + pwbdata.shippercontactidentifier + "/" + pwbdata.shippercontactnum;
                }

                if (pwbdata.shipperaccnum.Length > 0 || str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line5 = "SHP/" + pwbdata.shipperaccnum;
                    if (str4.Length > 0)
                    {
                        line5 = line5.Trim('/') + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/') + "\r\n/" + str4.Trim('/');
                    }
                    else if (str3.Length > 0)
                    {
                        line5 = line5.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line5 = line5.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line5 = line5.Trim() + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            //CNE
            #region Line 6
            string line6 = "";
            str1 = "";
            str2 = "";
            str3 = "";
            str4 = "";
            try
            {
                if (pwbdata.consname.Length > 0)
                {
                    str1 = "/" + pwbdata.consname;
                }
                if (pwbdata.consadd.Length > 0)
                {
                    str2 = "/" + pwbdata.consadd;
                }

                if (pwbdata.consplace.Length > 0 || pwbdata.consstate.Length > 0)
                {
                    str3 = "/" + pwbdata.consplace + "/" + pwbdata.consstate;
                }
                if (pwbdata.conscountrycode.Length > 0 || pwbdata.conspostcode.Length > 0 || pwbdata.conscontactidentifier.Length > 0 || pwbdata.conscontactnum.Length > 0)
                {
                    str4 = "/" + pwbdata.conscountrycode + "/" + pwbdata.conspostcode + "/" + pwbdata.conscontactidentifier + "/" + pwbdata.conscontactnum;
                }

                if (pwbdata.consaccnum.Length > 0 || str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line6 = "CNE/" + pwbdata.consaccnum;
                    if (str4.Length > 0)
                    {
                        line6 = line6.Trim('/') + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/') + "\r\n/" + str4.Trim('/');
                    }
                    else if (str3.Length > 0)
                    {
                        line6 = line6.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line6 = line6.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line6 = line6.Trim() + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            //AGT
            #region Line 7
            string line7 = "";
            str1 = "";
            str2 = "";
            try
            {
                if (pwbdata.agentname.Length > 0)
                {
                    str1 = "/" + pwbdata.agentname;
                }
                if (pwbdata.agentplace.Length > 0)
                {
                    str2 = "/" + pwbdata.agentplace;
                }
                if (pwbdata.agentaccnum.Length > 0 || pwbdata.agentIATAnumber.Length > 0 || pwbdata.agentCASSaddress.Length > 0 || pwbdata.agentParticipentIdentifier.Length > 0 || str1.Length > 0 || str2.Length > 0)
                {
                    line7 = "AGT/" + pwbdata.agentaccnum + "/" + pwbdata.agentIATAnumber + "/" + pwbdata.agentCASSaddress + "/" + pwbdata.agentParticipentIdentifier;
                    if (str2.Length > 0)
                    {
                        line7 = line7.Trim('/') + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line7 = line7.Trim('/') + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            //SSR
            #region Line 8
            string line8 = "";
            if (pwbdata.specialservicereq1.Length > 0 || pwbdata.specialservicereq2.Length > 0)
            {
                line8 = "SSR/" + pwbdata.specialservicereq1 + "$" + pwbdata.specialservicereq2;
            }
            line8 = line8.Trim('$');
            line8 = line8.Replace("$", "\r\n");
            #endregion

            //NFY
            #region Line 9
            string line9 = "";
            str1 = str2 = str3 = str4 = "";
            try
            {
                if (pwbdata.notifyadd.Length > 0)
                {
                    str1 = "/" + pwbdata.notifyadd;
                }
                if (pwbdata.notifyplace.Length > 0 || pwbdata.notifystate.Length > 0)
                {
                    str2 = "/" + pwbdata.notifyplace + "/" + pwbdata.notifystate;
                }
                if (pwbdata.notifycountrycode.Length > 0 || pwbdata.notifypostcode.Length > 0 || pwbdata.notifycontactidentifier.Length > 0 || pwbdata.notifycontactnum.Length > 0)
                {
                    str3 = "/" + pwbdata.notifycountrycode + "/" + pwbdata.notifypostcode + "/" + pwbdata.notifycontactidentifier + "/" + pwbdata.notifycontactnum;
                }

                if (pwbdata.notifyname.Length > 0 || str1.Length > 0 || str2.Length > 0 || str3.Length > 0 || str4.Length > 0)
                {
                    line9 = "NFY/" + pwbdata.shipperaccnum;
                    if (str3.Length > 0)
                    {
                        line9 = line9.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/') + "\r\n/" + str3.Trim('/');
                    }
                    else if (str2.Length > 0)
                    {
                        line9 = line9.Trim() + "\r\n/" + str1.Trim('/') + "\r\n/" + str2.Trim('/');
                    }
                    else if (str1.Length > 0)
                    {
                        line9 = line9.Trim() + "\r\n/" + str1.Trim('/');
                    }
                }
            }
            catch (Exception ex) { }
            #endregion

            //ACC
            #region Line 10
            string line10 = "";
            if (pwbdata.accountinginfoidentifier.Length > 0 || pwbdata.accountinginfo.Length > 0)
            {
                line10 = "ACC/" + pwbdata.accountinginfoidentifier + "/" + pwbdata.accountinginfo + "";
            }
            #endregion

            //CVD
            #region Line 11
            string line11 = "";
            line11 = "CVD/" + pwbdata.currency + "/" + pwbdata.chargecode + "/PP/" + pwbdata.declaredvalue + "/" + pwbdata.declaredcustomvalue + "/" + pwbdata.insuranceamount + "";
            #endregion

            //RTD
            #region Line 12
            string line12 = buildRateNode(ref pwbrate);
            if (line12 == null)
            {
                return null;
            }
            #endregion

            //OTH
            #region Line 13
            string line13 = "";
            for (int i = 0; i < pwbOtherCharge.Length; i++)
            {
                if (i > 0)
                {
                    if (i % 3 == 0)
                    {
                        if (i != pwbOtherCharge.Length)
                        {
                            line13 += "\r\n/" + pwbOtherCharge[0].indicator + "/";
                        }
                    }
                }
                line13 += pwbOtherCharge[i].otherchargecode + "" + pwbOtherCharge[i].entitlementcode + "" + pwbOtherCharge[i].chargeamt;
                //if (i % 3 == 0)
                //{
                //    if(i != othData.Length)
                //    {
                //        PWBStr += "\r\nP";
                //    }
                //}
            }
            if (line13.Length > 1)
            {
                line13 = "OTH/P/" + line13;
            }
            #endregion

            //PPD
            #region Line 14
            string line14 = "", subline14 = "";

            if (pwbdata.PPweightCharge.Length > 0)
            {
                line14 = line14 + "/WT" + pwbdata.PPweightCharge;
            }
            if (pwbdata.PPValuationCharge.Length > 0)
            {
                line14 = line14 + "/VC" + pwbdata.PPValuationCharge;
            }
            if (pwbdata.PPTaxesCharge.Length > 0)
            {
                line14 = line14 + "/TX" + pwbdata.PPTaxesCharge;
            }
            if (pwbdata.PPOCDA.Length > 0)
            {
                subline14 = subline14 + "/OA" + pwbdata.PPOCDA;
            }
            if (pwbdata.PPOCDC.Length > 0)
            {
                subline14 = subline14 + "/OC" + pwbdata.PPOCDC;
            }
            if (pwbdata.PPTotalCharges.Length > 0)
            {
                subline14 = subline14 + "/CT" + pwbdata.PPTotalCharges;
            }
            if (line14.Length > 0 || subline14.Length > 0)
            {
                line14 = "PPD" + line14 + "$" + subline14;
            }
            line14 = line14.Trim('$');
            line14 = line14.Replace("$", "\r\n");
            #endregion

            //COL
            #region Line 15
            string line15 = "", subline15 = "";
            if (pwbdata.CCweightCharge.Length > 0)
            {
                line15 = line15 + "/WT" + pwbdata.CCweightCharge;
            }
            if (pwbdata.CCValuationCharge.Length > 0)
            {
                line15 = line15 + "/VC" + pwbdata.CCValuationCharge;
            }
            if (pwbdata.CCTaxesCharge.Length > 0)
            {
                line15 = line15 + "/TX" + pwbdata.CCTaxesCharge;
            }
            if (pwbdata.CCOCDA.Length > 0)
            {
                subline15 = subline15 + "/OA" + pwbdata.CCOCDA;
            }
            if (pwbdata.CCOCDC.Length > 0)
            {
                subline15 = subline15 + "/OC" + pwbdata.CCOCDC;
            }
            if (pwbdata.CCTotalCharges.Length > 0)
            {
                subline15 = subline15 + "/CT" + pwbdata.CCTotalCharges;
            }
            if (line15.Length > 0 || subline15.Length > 0)
            {
                line15 = "COL" + line15 + "$" + subline15;
            }
            line15 = line15.Trim('$');
            line15 = line15.Replace("$", "\r\n");
            #endregion

            //CER
            #region Line 16
            string line16 = "";
            if (pwbdata.shippersignature.Length > 0)
            {
                line16 = "CER/" + pwbdata.shippersignature;
            }
            #endregion

            //ISU
            #region Line 17
            string line17 = "";
            line17 = "ISU/" + pwbdata.carrierdate.PadLeft(2, '0') + pwbdata.carriermonth.PadLeft(2, '0') + pwbdata.carrieryear.PadLeft(2, '0') + "/" + pwbdata.carrierplace + "/" + pwbdata.carriersignature;
            #endregion

            //OSI
            #region Line 18
            string line18 = "";
            if (othinfoarray.Length > 0)
            {
                for (int i = 0; i < othinfoarray.Length; i++)
                {
                    if (othinfoarray[i].otherserviceinfo1.Length > 0)
                    {
                        line18 = "OSI/" + othinfoarray[i].otherserviceinfo1 + "$";
                        if (othinfoarray[i].otherserviceinfo2.Length > 0)
                        {
                            line18 = line18 + "/" + othinfoarray[i].otherserviceinfo2 + "$";
                        }
                    }
                }
                line18 = line18.Trim('$');
                line18 = line18.Replace("$", "\r\n");
            }
            #endregion

            //CDC
            #region Line 19
            string line19 = "";
            if (pwbdata.cccurrencycode.Length > 0 || pwbdata.ccexchangerate.Length > 0 || pwbdata.ccchargeamt.Length > 0)
            {
                string[] exchnagesplit = pwbdata.ccexchangerate.Split(',');
                string[] chargesplit = pwbdata.ccchargeamt.Split(',');
                if (exchnagesplit.Length == chargesplit.Length)
                {
                    for (int k = 0; k < exchnagesplit.Length; k++)
                    {
                        line19 = line19 + exchnagesplit[k] + "/" + chargesplit[k] + "/";
                    }
                }
                line19 = "CDC/" + pwbdata.cccurrencycode + line19.Trim('/');
            }
            #endregion

            //REF
            #region Line 20
            string line20 = "";
            line20 = pwbdata.senderairport + "" + pwbdata.senderofficedesignator + "" + pwbdata.sendercompanydesignator + "/" + pwbdata.senderFileref + "/" + pwbdata.senderParticipentIdentifier + "/" + pwbdata.senderParticipentCode + "/" + pwbdata.senderPariticipentAirport + "";
            //line20 = line20.Trim('/');
            if (line20.Length > 1)
            {
                line20 = "REF/" + line20;
            }

            #endregion

            //COR
            #region Line 21
            string line21 = "";
            if (pwbdata.customorigincode.Length > 0)
            {
                line21 = "COR/" + pwbdata.customorigincode + "";
            }
            #endregion

            //COI
            #region Line 22
            string line22 = "";
            if (pwbdata.commisioncassindicator.Length > 0 || pwbdata.commisionCassSettleAmt.Length > 0)
            {
                line22 = "COI/" + pwbdata.commisioncassindicator + "/" + pwbdata.commisionCassSettleAmt.Replace(',', '/') + "";
            }
            #endregion

            //SII
            #region Line 23
            string line23 = "";
            if (pwbdata.saleschargeamt.Length > 0 || pwbdata.salescassindicator.Length > 0)
            {
                line23 = "SII/" + pwbdata.saleschargeamt + "/" + pwbdata.salescassindicator + "";
            }
            #endregion

            //ARD
            #region Line 24
            string line24 = "";
            if (pwbdata.agentfileref.Length > 0)
            {
                line24 = "ARD/" + pwbdata.agentfileref + "";
            }
            #endregion

            //SPH
            #region Line 25
            string line25 = "";
            if (pwbdata.splhandling.Replace(",", "").Length > 0)
            {
                line25 = "SPH/" + pwbdata.splhandling.Replace(',', '/');
            }
            #endregion

            //NOM
            #region Line 26
            string line26 = "";
            if (pwbdata.handlingname.Length > 0 || pwbdata.handlingplace.Length > 0)
            {
                line26 = "NOM/" + pwbdata.handlingname + "/" + pwbdata.handlingplace;
            }
            #endregion

            //SRI
            #region Line 27
            string line27 = "";
            if (pwbdata.shiprefnum.Length > 0 || pwbdata.supplemetryshipperinfo1.Length > 0 || pwbdata.supplemetryshipperinfo2.Length > 0)
            {
                line27 = "SRI/" + pwbdata.shiprefnum + "/" + pwbdata.supplemetryshipperinfo1 + "/" + pwbdata.supplemetryshipperinfo2;
            }
            #endregion

            //OPI
            #region Line 28
            str1 = "";
            string line28 = "";
            if (pwbdata.othairport.Length > 0 || pwbdata.othofficedesignator.Length > 0 || pwbdata.othcompanydesignator.Length > 0 || pwbdata.othfilereference.Length > 0 || pwbdata.othparticipentidentifier.Length > 0 || pwbdata.othparticipentcode.Length > 0 || pwbdata.othparticipentairport.Length > 0)
            {
                str1 = "/" + pwbdata.othparticipentairport + "/" +
                pwbdata.othofficedesignator + "" + pwbdata.othcompanydesignator + "/" + pwbdata.othfilereference + "/" +
                pwbdata.othparticipentidentifier + "/" + pwbdata.othparticipentcode + "/" + pwbdata.othparticipentairport + "";
                str1 = str1.Trim('/');
            }

            if (pwbdata.othparticipentname.Length > 0 || str1.Length > 0)
            {
                line28 = "OPI/" + pwbdata.othparticipentname + "$" + str1;
            }
            line28 = line28.Trim('$');
            line28 = line28.Replace("$", "\r\n");
            #endregion

            //OCI
            #region Line 29
            string line29 = "";
            if (custominfo.Length > 0)
            {
                for (int i = 0; i < custominfo.Length; i++)
                {
                    line29 = "/" + custominfo[i].IsoCountryCodeOci + "/" + custominfo[i].InformationIdentifierOci + "/" + custominfo[i].CsrIdentifierOci + "/" + custominfo[i].SupplementaryCsrIdentifierOci + "$";
                }
                line29 = "OCI" + line4.Trim('$');
                line29 = line4.Replace("$", "\r\n");
            }
            #endregion

            #region Build PWB
            pwbstr = "";
            pwbstr = line1.Trim('/');
            if (line2.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line2.Trim('/');
            }
            if (line3.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line3.Trim('/');
            }
            pwbstr += "\r\n" + line4.Trim('/') + "\r\n" + line5.Trim('/') + "\r\n" + line6.Trim('/');
            if (line7.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line7.Trim('/');
            }
            if (line8.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line8.Trim('/');
            }
            if (line9.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line9.Trim('/');
            }
            if (line10.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line10.Trim('/');
            }
            pwbstr += "\r\n" + line11.Trim('/') + "\r\n" + line12.Trim('/');
            if (line13.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line13.Trim('/');
            }
            if (line14.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line14.Trim('/');
            }
            if (line15.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line15.Trim('/');
            }
            if (line16.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line16.Trim('/');
            }
            if (line17.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line17.Trim('/');
            }
            if (line18.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line18.Trim('/');
            }
            if (line19.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line19.Trim('/');
            }
            if (line20.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line20.Trim('/');
            }
            if (line21.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line21.Trim('/');
            }
            if (line22.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line22.Trim('/');
            }
            if (line23.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line23.Trim('/');
            }
            if (line24.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line24.Trim('/');
            }
            if (line25.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line25.Trim('/');
            }
            if (line26.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line26.Trim('/');
            }
            if (line27.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line27.Trim('/');
            }
            if (line28.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line28.Trim('/');
            }
            if (line29.Trim('/').Length > 0)
            {
                pwbstr += "\r\n" + line29.Trim('/');
            }
            #endregion
        }
        catch (Exception ex)
        {
            pwbstr = "ERR";
        }
        return pwbstr;
    }

 
    #endregion

    //SCM Messaging
    #region Encode SCM
    public static string EncodeSCMforsend(MessageData.SCM SCMData)
    {
        StringBuilder SCM = new StringBuilder();
        try
        {
            #region Line 1
            SCM.AppendLine(SCMData.MessageIdentifier);
            #endregion

            #region Line 2
            SCM.AppendLine(SCMData.AirportCode + "." + SCMData.Day + SCMData.Month + "/" + SCMData.Time);
            #endregion line 2

            #region Line3
            foreach (MessageData.ULDStockInfo StockInfo in SCMData.ULDInfo)
            {
                StringBuilder StockULDSubInfo = new StringBuilder();
                int subInfoCount = 0;
                foreach (MessageData.ULDStockInfoSub StockInfoSub in StockInfo.ULDInfoSub)
                {
                    subInfoCount++;

                    StockULDSubInfo.Append((subInfoCount == 1 ? string.Empty : "/") + StockInfoSub.ULDSerialNo + StockInfoSub.ULDOwnerCode);
                }
                SCM.AppendLine("." + StockInfo.ULDTypeCode + "." + StockULDSubInfo.ToString() + "." + StockInfo.TotalIdentifier + StockInfo.NumberOfULD);
            }
            #endregion

            #region Line4
            int count = 0;
            StringBuilder sbULDInfoText = new StringBuilder();
            foreach (string s in SCMData.ULDInfoText)
            {
                count++;
                if (count == 1)
                {
                    if (s != string.Empty)
                        sbULDInfoText.AppendLine("SI " + s);
                }
                else
                {
                    if (s != string.Empty)
                        sbULDInfoText.AppendLine(s);
                }
            }
            SCM.AppendLine(sbULDInfoText.ToString());
            #endregion

        }
        catch (Exception ex)
        {
            return "ERR";
        }
        return SCM.ToString();
    }
    #endregion

    //Encoding Decoding String
    public static class StringCipher
    {
        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        public static string Encrypt(string plainText, string passPhrase)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            try
            {
                byte[] cipherTextBytes;
                try
                {
                    cipherTextBytes = Convert.FromBase64String(cipherText);
                }
                catch (Exception ex)
                {
                    cipherText = cipherText + "=";
                    cipherTextBytes = Convert.FromBase64String(cipherText);
                }
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
                {
                    byte[] keyBytes = password.GetBytes(keysize / 8);
                    using (RijndaelManaged symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.Mode = CipherMode.CBC;
                        using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                        {
                            using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                            {
                                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { return "Err"; }
        }

    }


    
}

