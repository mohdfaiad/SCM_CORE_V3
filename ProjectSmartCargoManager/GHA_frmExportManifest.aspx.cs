//Changes on 21 may
//Changes on 12 july
//Modified on 14 july 
//28 Sept updated
//29 Sept Updated
//4 oct
//6 oct

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Drawing;
using System.Collections;
using System.Data.SqlClient;
using QID.DataAccess;
using BAL;
using System.Text;
using SCM.Common.Struct;

namespace ProjectSmartCargoManager
{
    public partial class frmExportManifest_GHA : System.Web.UI.Page
    {
        #region Variables
        //ShowFlightsBAL objBAL = new ShowFlightsBAL();
        //BookingBAL objBLL = new BookingBAL();
        BLExpManifest objExpMani = new BLExpManifest();
        //ConBooking_GHA objBook = new ConBooking_GHA();
        DataTable MemDetails = new DataTable("Exp_Manifest_MemDetails");
        DataTable tabMenifestDetails = new DataTable("Exp_Manifest_tabMenifestDetails");
        bool chkduplicate = false;
        string SelectedULDnos, Destination;

        int TotalULD = 0, totalULDAWB = 0, totalULDPCS = 0, totalAWB = 0, totalAWBPCS = 0;
        float totalAWBWt = 0, totalAWBVol = 0, totalULDVol = 0, totalULDWt = 0;
        //ArrayList ULDDestpt = new ArrayList();
        //EMAILOUT Em = new EMAILOUT();
        //MasterBAL objMst = new MasterBAL();
        DateTime dtCurrentDate = DateTime.Now;
        string fblmsg;
        string ffmmsg = string.Empty;
        bool ChkSuperUserOffload = false;
        #endregion

        #region Form Load
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                dtCurrentDate = (DateTime)Session["IT"];
                ChangeButtonStatus();
                if (!IsPostBack)
                {
                    #region Operation Time Popup Config Check
                    try
                    {
                        //LoginBL objConfig = new LoginBL();
                        //btnOpsTime.Visible = Convert.ToBoolean(objConfig.GetMasterConfiguration("enableActualOpsTime"));
                        btnOpsTime.Visible = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "enableActualOpsTime"));
                        //objConfig = null;
                    }
                    catch (Exception ex)
                    { }
                    #endregion
                    Session["ExpManifest_FltOrigin"] = null;
                    btnFetchAWB.Enabled = true;
                    btnAssignFetchedAWB.Enabled = true;
                    btnFetchClear.Enabled = true;
                    try
                    {
                        //Shubhankar
                        //if (ddlTailNo.Items.Count <= 0)
                        //    ddlTailNo.Items.Add("Select");

                        if (Session["AirlinePrefix"] != null)
                            txtFlightCode.Text = Convert.ToString(Session["AirlinePrefix"]);

                        if (Session["awbPrefix"] == null)
                        {
                            MasterBAL objBal = new MasterBAL();
                            Session["awbPrefix"] = objBal.awbPrefix();
                        }
                        txtAWBPrefix.Text = Convert.ToString(Session["awbPrefix"]);
                        txtFetchAWBPrefix.Text = Convert.ToString(Session["awbPrefix"]);

                        ClearSession();
                        LoadSystemParameters();
                        hdnManifestFlag.Value = "";

                        //LoadOperationTimeConfig();

                        lblRoute.Text = "";
                        lblDate.Text = "";
                        //Session["Username"] = "bomdemo";
                        //Session["StationCode"] = "BOM";

                        string UserName = Session["Username"].ToString();
                        string StationCode = Session["Station"].ToString();

                        lblDepAirport.Text = Convert.ToString(Session["Station"]);
                        /* Clear Session on form load*/

                        TextBoxdate.Text = dtCurrentDate.ToString("dd/MM/yyyy");// System.DateTime.Now.ToString("dd/MM/yyyy");

                        //ShowULDAWBSummary();

                        #region Temp table for AWB Tab
                        DataTable MemDetails = new DataTable("Exp_Manifest_PageLoad_MemDetails");

                        MemDetails.Columns.Add("ULDno");
                        MemDetails.Columns.Add("POU");
                        MemDetails.Columns.Add("TareWeight");

                        Session["DataTableTemp"] = (DataTable)MemDetails;
                        #endregion Temp table for AWB Tab

                        #region Temp table for Manifest grid
                        DataTable tabMenifestDetails = new DataTable("Exp_Manifest_PageLoad_tabMenifestDetails");

                        tabMenifestDetails.Columns.Add("ULDno");
                        tabMenifestDetails.Columns.Add("POU");
                        tabMenifestDetails.Columns.Add("POL");
                        tabMenifestDetails.Columns.Add("ULDdest");
                        tabMenifestDetails.Columns.Add("Counter");
                        tabMenifestDetails.Columns.Add("AWBno");
                        tabMenifestDetails.Columns.Add("SCC");
                        tabMenifestDetails.Columns.Add("PCS");
                        tabMenifestDetails.Columns.Add("GrossWgt");
                        tabMenifestDetails.Columns.Add("Vol");

                        tabMenifestDetails.Columns.Add("BookedPCS");
                        tabMenifestDetails.Columns.Add("BookedWgt");

                        tabMenifestDetails.Columns.Add("StatedPCS");
                        tabMenifestDetails.Columns.Add("StatedWgt");
                        tabMenifestDetails.Columns.Add("Desc");
                        tabMenifestDetails.Columns.Add("Orign");
                        tabMenifestDetails.Columns.Add("Dest");
                        tabMenifestDetails.Columns.Add("Manifested");
                        tabMenifestDetails.Columns.Add("LoadingPriority");
                        tabMenifestDetails.Columns.Add("Remark");
                        tabMenifestDetails.Columns.Add("Bonded");
                        tabMenifestDetails.Columns.Add("Location");
                        tabMenifestDetails.Columns.Add("FltNo");
                        tabMenifestDetails.Columns.Add("CartNumber");

                        Session["ManifestGridData"] = (DataTable)tabMenifestDetails;
                        #endregion Temp table for Manifest grid
                        LoadGridRoutingDetail();
                        FillIrregularityCode();
                        CheckPartnerStatus();

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>return disable();</script>", false);

                    }
                    catch (Exception ex)
                    {

                    }

                    try
                    {
                        //LoginBL lBal = new LoginBL();
                        //bool CartNO = Convert.ToBoolean(lBal.GetMasterConfiguration("ShowCartNo"));
                        bool CartNO = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowCartNo"));
                        if (!CartNO)
                        {
                            gdvULDLoadPlanAWB.Columns[1].Visible = false;
                            gdvULDDetails.Columns[4].Visible = false;
                        }
                    }
                    catch (Exception ex)
                    { }
                }

                //Check for Airline Prefix

                if (Session["AirlinePrefix"] != null && Convert.ToString(Session["AirlinePrefix"]) != "" && txtFlightCode.Text.Trim() != "")
                {
                    if (Convert.ToString(Session["AirlinePrefix"]) == txtFlightCode.Text.Trim())
                        btnprintUCR.Enabled = false;
                }

                //End
                try
                {
                    if (Session["ULDACT"].ToString().ToUpper() == "FALSE")
                    {
                        TabPanelULD.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                }
                if (Request["__EVENTTARGET"] == "RefreshList")
                {
                    BtnList_Click(null, null);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Form Load

        #region Flight List Button Click
        protected void BtnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            lblFetchStatus.Text = "";
            DataSet dsddldata = new DataSet("Exp_Manifest_btnListClick_dsddldata");
            DataTable tabMenifestDetails = new DataTable("Exp_Manifest_btnListClick_tabMenifestDetails");
            DataSet ds = new DataSet("Exp_Manifest_btnListClick_ds");
            DataSet dsFlightDetails = new DataSet("Exp_Manifest_btnListClick_dsFlightDetails");
            DataSet fltPOU = new DataSet("Exp_Manifest_btnListClick_fltPOU");

            try
            {
                //Session["Split"] = "";
                hdnManifestFlag.Value = "";
                string Deptapt = "";

                if (ValidateFlightPrtefix() == false)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid Flight Code";
                    txtFlightCode.Focus();
                    return;
                }

                //Shubhankar
                #region Get Tail No
                //GetTailNo();
                #endregion

                #region Get Ver ULD POU POL DDL data
                lblStatus.Text = "";

                btnOffload.Enabled = true;
                btnUnassign.Enabled = true;
                btnSplitUnassign.Enabled = true;
                btnSplitAssign.Enabled = true;
                BtnAddtoManifest.Enabled = true;
                btnFetchClear.Enabled = true;
                btnFetchAWB.Enabled = true;
                btnAssignFetchedAWB.Enabled = true;

                string FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                Session["ULDData"] = "";
                Session["DataTableTemp"] = "";
                // Session["ManifestGridData"] = "";


                string ManifestDate = "", ManifestdateFrom = "", ManifestdateTo = "";

                ManifestDate = TextBoxdate.Text.ToString();
                if (ManifestDate.Length > 10)
                {
                    ManifestdateFrom = ManifestDate.Substring(0, 10) + " 00:00:00";
                    ManifestdateTo = ManifestDate.Substring(0, 10) + " 23:59:59";
                }
                else
                {
                    ManifestdateFrom = ManifestDate + " 00:00:00";
                    ManifestdateTo = ManifestDate + " 23:59:59";
                }

                ClearGrid();//clear manifest
                //DateTime dtMani = new DateTime();

                //  FillAWBGrid();//fill AWB grid 6change
                dsddldata = objExpMani.GetDDLVerULDPOUPOLdata(FLTno, DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null), ManifestdateTo, lblDepAirport.Text.Trim());

                Session["IsDeparted"] = null;
                Session["IsPreManifested"] = null;
                if (dsddldata != null && dsddldata.Tables.Count > 0 && dsddldata.Tables[4].Rows.Count > 0)
                {
                    Session["IsDeparted"] = dsddldata.Tables[4].Rows[0]["IsDeparted"];
                    Session["IsManifested"] = dsddldata.Tables[4].Rows[0]["IsManifested"];
                }
                else
                {
                    Session["IsDeparted"] = null;
                    Session["IsManifested"] = null;
                }

                try
                {
                    if (dsddldata != null && dsddldata.Tables.Count > 0 && dsddldata.Tables[5].Rows.Count > 0)
                    {
                        Deptapt = Convert.ToString(dsddldata.Tables[5].Rows[0]["DeptAirport"]);
                        if (Deptapt != lblDepAirport.Text)
                        {
                            Session["IsPreManifested"] = dsddldata.Tables[5].Rows[0]["IsManifested"];
                        }
                    }
                    else
                    {
                        Session["IsPreManifested"] = null;
                    }
                }
                catch (Exception ex)
                {
                }
                lblVersionID.Text = "V01";
                if (dsddldata != null && dsddldata.Tables.Count > 0 && dsddldata.Tables[0].Rows.Count > 0)
                {
                    //Session["IsDeparted"] = dsddldata.Tables[0].Rows[0]["IsDeparted"];

                    ddlVersion.Items.Add("Select");
                    ddlULD.Items.Add("Select");
                    ddlPOLDetails.Items.Add("Select");
                    //ddlPOU.Items.Add("Select");

                    // ddlSelectULD.Items.Add("Bulk");

                    try
                    {
                        if (dsddldata.Tables.Count > 4 && dsddldata.Tables[4].Rows.Count > 0)
                        {
                            ddlIrregularityCode.SelectedValue = Convert.ToString(dsddldata.Tables[4].Rows[0]["IrregularityID"]);                            
                        }
                    }
                    catch
                    {
                        ddlIrregularityCode.SelectedIndex = 0;
                    }
                    ddlVersion.DataSource = dsddldata;
                    ddlVersion.DataTextField = "VerNo";
                    ddlVersion.DataValueField = "VerNo";
                    ddlVersion.DataBind();
                    string checkversion = dsddldata.Tables[0].Rows[0][0].ToString();
                    if (checkversion == "")
                    {
                        lblVersionID.Text = "V01";

                    }
                    else
                    {
                        checkversion = checkversion.Remove(0, 1);
                        //  checkversion = checkversion.Remove(checkversion.Length - 2, 2);
                        int ver = int.Parse(checkversion) + 1;
                        checkversion = ver.ToString();
                        checkversion = checkversion.PadLeft(2, '0');
                        lblVersionID.Text = "V" + checkversion;
                    }

                    ddlULD.DataSource = dsddldata.Tables[1];
                    ddlULD.DataTextField = "ULDno";
                    ddlULD.DataValueField = "ULDno";
                    ddlULD.DataBind();


                    ddlPOLDetails.DataSource = dsddldata.Tables[2];
                    ddlPOLDetails.DataTextField = "POL";
                    ddlPOLDetails.DataValueField = "POL";
                    ddlPOLDetails.DataBind();

                    //ddlPOU.DataSource = dsddldata.Tables[3];
                    //ddlPOU.DataTextField = "POU";
                    //ddlPOU.DataValueField = "POU";
                    //ddlPOU.DataBind();

                }

                if (Session["IsManifested"] != null && (bool)Session["IsManifested"] == false)
                {
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "Flight Manifested.";
                    //btnSave.Enabled = false;
                    //btnDepartFit.Enabled = false;
                    //btnReopenFit.Enabled = true;                    

                    if ((string)Session["IsDeparted"] == "D")
                    {
                        btnSave.Enabled = false;
                        btnDepartFit.Enabled = false;
                        btnReopenFit.Enabled = true;
                        btnOffload.Enabled = false;
                        lblStatus.Text = "Flight Departed.";
                        BtnAddtoManifest.Enabled = false;
                        btnSplitAssign.Enabled = false;
                        txtTailNo.Enabled = false; //ddlTailNo.Enabled = false; //Shubhankar
                        btnFinalize.Enabled = false;
                        btnFetchClear.Enabled = false;
                        btnFetchAWB.Enabled = false;
                        btnAssignFetchedAWB.Enabled = false;
                    }
                    else if ((string)Session["IsDeparted"] == "R")
                    {
                        btnSave.Enabled = true;
                        btnDepartFit.Enabled = true;
                        btnReopenFit.Enabled = false;
                        btnOffload.Enabled = true;
                        lblStatus.Text = "Flight Re-Opened.";
                        BtnAddtoManifest.Enabled = true;
                        btnSplitAssign.Enabled = true;
                        txtTailNo.Enabled = true; //ddlTailNo.Enabled = true; //Shubhankar
                        btnFinalize.Enabled = true;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnDepartFit.Enabled = true;
                        btnReopenFit.Enabled = false;
                        btnOffload.Enabled = true;
                        BtnAddtoManifest.Enabled = true;
                        btnSplitAssign.Enabled = true;
                        txtTailNo.Enabled = true; //ddlTailNo.Enabled = true; //Shubhankar
                        btnFinalize.Enabled = true;
                    }
                }
                else
                {
                    if (Session["IsManifested"] != null && (bool)Session["IsManifested"] == true)
                    {
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Text = "Flight Manifested.";
                        btnSave.Enabled = true;
                        btnDepartFit.Enabled = true;
                        btnReopenFit.Enabled = false;
                        btnOffload.Enabled = true;
                        BtnAddtoManifest.Enabled = true;
                        btnSplitAssign.Enabled = true;
                        txtTailNo.Enabled = true; //ddlTailNo.Enabled = true; //Shubhankar
                        btnFinalize.Enabled = true;
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Text = "New Flight.";
                        btnSave.Enabled = true;
                        btnFinalize.Enabled = false;
                        btnDepartFit.Enabled = false;
                        btnReopenFit.Enabled = false;
                        try
                        {
                            if (Convert.ToString(Session["EmptyFlight"]) == "M")
                            {
                                #region Empty Flight
                                try
                                {
                                    //LoginBL objConfig = new LoginBL();
                                    //btnDepartFit.Enabled = Convert.ToBoolean(objConfig.GetMasterConfiguration("ManifestEmptyFlight"));
                                    btnDepartFit.Enabled = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ManifestEmptyFlight"));
                                    //objConfig = null;
                                    Session["IsManifested"] = false;
                                }
                                catch (Exception ex)
                                { }
                                #endregion

                            }
                        }
                        catch (Exception ex)
                        { }
                        try
                        {
                            if (Deptapt != lblDepAirport.Text.Trim())
                            {
                                if (Session["IsPreManifested"] == null || (bool)Session["IsPreManifested"] == true)
                                {
                                    //lblStatus.ForeColor = Color.Red;
                                    //lblStatus.Text = "Flight is not manifested at previous station";
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                #endregion


                //if (Session["ManifestGridData"] == null)
                //{
                #region Temp table for Manifest grid

                tabMenifestDetails.Columns.Add("ULDno");
                tabMenifestDetails.Columns.Add("POU");
                tabMenifestDetails.Columns.Add("POL");
                tabMenifestDetails.Columns.Add("AWBDest");
                tabMenifestDetails.Columns.Add("Counter");
                tabMenifestDetails.Columns.Add("AWBno");
                tabMenifestDetails.Columns.Add("SCC");
                tabMenifestDetails.Columns.Add("PCS");
                tabMenifestDetails.Columns.Add("GrossWgt");
                tabMenifestDetails.Columns.Add("Vol");

                tabMenifestDetails.Columns.Add("BookedPCS");
                tabMenifestDetails.Columns.Add("BookedWgt");

                tabMenifestDetails.Columns.Add("StatedPCS");
                tabMenifestDetails.Columns.Add("StatedWgt");
                tabMenifestDetails.Columns.Add("Desc");
                tabMenifestDetails.Columns.Add("Orign");
                tabMenifestDetails.Columns.Add("Dest");
                tabMenifestDetails.Columns.Add("Manifested");
                tabMenifestDetails.Columns.Add("LoadingPriority");
                tabMenifestDetails.Columns.Add("Remark");
                tabMenifestDetails.Columns.Add("Bonded");
                tabMenifestDetails.Columns.Add("Location");
                tabMenifestDetails.Columns.Add("FltNo");
                tabMenifestDetails.Columns.Add("CartNumber");

                Session["ManifestGridData"] = (DataTable)tabMenifestDetails;
                #endregion Temp table for Manifest grid

                //  }

                gdvULDLoadPlanAWB.DataSource = null;
                gdvULDLoadPlanAWB.DataBind();

                gdvULDLoadPlan.DataSource = null;
                gdvULDLoadPlan.DataBind();

                txtFlightID.Enabled = false;
                TextBoxdate.Enabled = false;
                txtFlightCode.Enabled = false;

                try
                {
                    ChangeButtonStatus();
                }
                catch (Exception)
                {


                }

                string DepartureAirport = lblDepAirport.Text;
                string Updatedby = Convert.ToString(Session["Username"]);
                string IsBonded = string.Empty;

                //Manifest date
                DateTime dt = new DateTime();
                //DateTime dt1 = new DateTime();
                dt = DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null);
                // dt = DateTime.Parse(TextBoxdate.Text);

                // dt1 = Convert.ToDateTime(TextBoxdate.Text);
                ds = objExpMani.GetManifestDetails(FLTno, dt, ManifestdateTo, DepartureAirport);//return manifest data

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    Session["MD"] = ds.Tables[0];
                    Session["AT"] = ds.Tables[1];
                   
                }
                else
                {
                    Session["MD"] = null;
                    Session["AT"] = null;
                }

                dsFlightDetails = objExpMani.GetFlightTabdetails(lblDepAirport.Text, FLTno, dt);//fill labels with source & destiny

                string strdestination = string.Empty;

                // Appended 25Apr
                fltPOU = objExpMani.GetPOUAirlineSchedule(FLTno, DepartureAirport, dt);
                string RouteDetails = string.Empty;

                if (dsFlightDetails != null && dsFlightDetails.Tables.Count > 0 && dsFlightDetails.Tables[0].Rows.Count > 0)
                    RouteDetails = "Route:" + dsFlightDetails.Tables[0].Rows[0][0].ToString() + "-";

                if (fltPOU != null && fltPOU.Tables.Count > 0 && fltPOU.Tables[0].Rows.Count > 0)
                {
                    for (int route = 0; route < fltPOU.Tables[0].Rows.Count; route++)
                    {
                        RouteDetails = RouteDetails + fltPOU.Tables[0].Rows[route][0] + "-";
                        strdestination = fltPOU.Tables[0].Rows[route][0].ToString();
                        Session["Dest"] = strdestination;

                        if (route == 0)
                            Session["FtlPOU"] = fltPOU.Tables[0].Rows[route][0];
                    }
                    if (txtTailNo.Text == "" && fltPOU.Tables.Count > 1)
                        txtTailNo.Text = fltPOU.Tables[1].Rows[0][0].ToString();

                    //Set origin of flight if different than current station.
                    if (fltPOU.Tables.Count > 2 && fltPOU.Tables[2].Rows.Count > 0)
                    {
                        Session["ExpManifest_FltOrigin"] = fltPOU.Tables[2].Rows[0][0].ToString();
                    }
                }

                if (dsFlightDetails != null && dsFlightDetails.Tables.Count > 0 && dsFlightDetails.Tables[1].Rows.Count > 0)
                    Session["Identifier"] = Convert.ToString(dsFlightDetails.Tables[1].Rows[0]["Identifier"]);

                if (RouteDetails.Length > 0)
                {
                    RouteDetails = RouteDetails.Remove(RouteDetails.Length - 1, 1);
                }
                lblRoute.Text = RouteDetails;

                //  lblRoute.Text = "Route:" + dsFlightDetails.Tables[0].Rows[0][0] + "-" + dsFlightDetails.Tables[0].Rows[0][1];
                if (dsFlightDetails != null && dsFlightDetails.Tables.Count > 0 && dsFlightDetails.Tables[0].Rows.Count > 0)
                {
                    lblDate.Text = "|" + Convert.ToDateTime(dsFlightDetails.Tables[0].Rows[0][2]).ToString("dd/MMM/yyyy HH:mm"); //+ " " + dsFlightDetails.Tables[0].Rows[0][3];
                    lblRoute.Visible = true;
                    lblDate.Visible = true;
                    if (dsFlightDetails != null && dsFlightDetails.Tables.Count > 0 && dsFlightDetails.Tables[0].Rows.Count > 0)
                        Destination = dsFlightDetails.Tables[0].Rows[0][1].ToString();//add destination to string
                    Session["POLairport"] = lblDepAirport.Text;
                    Session["FltNumber"] = FLTno;

                    Session["Fltdate"] = dt.ToString("yyyy-MM-dd");
                    //FrommailId = "";

                    if (fltPOU != null && fltPOU.Tables.Count > 0 && fltPOU.Tables[0].Rows.Count > 0)
                    {
                        fltPOU.Tables[0].Rows.Add("ALL");
                        ddlMainPOU.DataSource = fltPOU.Tables[0];
                        ddlMainPOU.DataTextField = "Dest";
                        ddlMainPOU.DataValueField = "Dest";
                        ddlMainPOU.DataBind();
                        ddlMainPOU.SelectedIndex = (ddlMainPOU.Items.Count - 1);

                        ddlPOU.DataSource = fltPOU.Tables[0];
                        ddlPOU.DataTextField = "Dest";
                        ddlPOU.DataValueField = "Dest";
                        ddlPOU.DataBind();
                        ddlPOU.SelectedIndex = (ddlPOU.Items.Count - 1);
                    }
                    FillAWBGridMain("");
                }
                else
                {
                    txtFlightID.Enabled = true;
                    TextBoxdate.Enabled = true;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Flight does not belong to destination given Or Flight does not scheduled for the day.";
                    txtFlightID.Enabled = true;
                    return;
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string ULDno = "", AWBno = "", POU = "", POL = "", AWBDest = "", AWBOrigin = "";
                    int PCS = 0, AVLPCS = 0;
                    double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0;
                    string Updatedon = "", SCC = "", Counter = "";
                    bool IsManifest = false;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //txtTailNo.Text = ds.Tables[0].Rows[i]["TailNo"].ToString();
                        try
                        {
                            //ddlTailNo.Text = ds.Tables[0].Rows[i]["TailNo"].ToString();
                            if (i == 0 && ds.Tables[0].Rows[i]["TailNo"] != null &&
                                ds.Tables[0].Rows[i]["TailNo"].ToString() != "")
                            {
                                txtTailNo.Text = ds.Tables[0].Rows[i]["TailNo"].ToString();
                            }
                        }
                        catch
                        {
                            //ddlTailNo.Text = "Select";
                        }
                        txtEMG.Text = ds.Tables[0].Rows[i]["IMENo"].ToString();

                        ULDno = ds.Tables[0].Rows[i].ItemArray.GetValue(0).ToString();
                        POL = ds.Tables[0].Rows[i].ItemArray.GetValue(1).ToString();
                        //POL = ULDno = ds.Tables[0].Rows[i].ItemArray.GetValue(2).ToString();
                        //POU = ULDno = ds.Tables[0].Rows[i].ItemArray.GetValue(3).ToString();
                        AWBDest = ds.Tables[0].Rows[i].ItemArray.GetValue(3).ToString();
                        Counter = ds.Tables[0].Rows[i].ItemArray.GetValue(4).ToString();
                        //  POL =ds.Tables[0].Rows[i].ItemArray.GetValue(2).ToString();
                        POU = ds.Tables[0].Rows[i].ItemArray.GetValue(2).ToString();

                        AWBno = ds.Tables[0].Rows[i].ItemArray.GetValue(5).ToString();
                        SCC = ds.Tables[0].Rows[i].ItemArray.GetValue(6).ToString();
                        PCS = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray.GetValue(7).ToString());
                        WGT = Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray.GetValue(8).ToString());
                        AVLPCS = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray.GetValue(10).ToString());
                        AVLWGT = Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray.GetValue(11).ToString());
                        IsManifest = bool.Parse(ds.Tables[0].Rows[i]["Manifested"].ToString());
                        AWBOrigin = ds.Tables[0].Rows[i]["Org"].ToString();
                        IsBonded = ds.Tables[0].Rows[i]["IsBonded"].ToString();
                        // Updatedon = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

                        //adding collected data to gridview rows one by one

                        tabMenifestDetails = (DataTable)Session["ManifestGridData"];

                        DataRow l_Datarow = tabMenifestDetails.NewRow();

                        l_Datarow["ULDno"] = ULDno;
                        l_Datarow["POU"] = POU;
                        l_Datarow["POL"] = POL;
                        l_Datarow["AWBDest"] = AWBDest;
                        l_Datarow["Counter"] = Counter;
                        l_Datarow["AWBno"] = AWBno;
                        l_Datarow["SCC"] = SCC;
                        l_Datarow["PCS"] = PCS;
                        l_Datarow["GrossWgt"] = WGT;
                        l_Datarow["VOL"] = ds.Tables[0].Rows[i]["Vol"].ToString();
                        l_Datarow["StatedPCS"] = AVLPCS;
                        l_Datarow["StatedWgt"] = AVLWGT;

                        l_Datarow["BookedPCS"] = ds.Tables[0].Rows[i]["AWBPcs"].ToString();
                        l_Datarow["BookedWgt"] = ds.Tables[0].Rows[i]["AWBGwt"].ToString();

                        l_Datarow["Desc"] = ds.Tables[0].Rows[i]["Desc"].ToString();
                        l_Datarow["Orign"] = AWBOrigin;
                        l_Datarow["Dest"] = AWBDest;
                        l_Datarow["Manifested"] = IsManifest;
                        l_Datarow["LoadingPriority"] = "";
                        l_Datarow["Remark"] = ds.Tables[0].Rows[i]["Remark"].ToString(); 
                        l_Datarow["Bonded"] = IsBonded;
                        l_Datarow["Location"] = ds.Tables[0].Rows[i]["Location"].ToString();
                        l_Datarow["FltNo"] = ds.Tables[0].Rows[i]["FltNo"].ToString();
                        l_Datarow["CartNumber"] = ds.Tables[0].Rows[i]["CartNumber"].ToString();

                        tabMenifestDetails.Rows.Add(l_Datarow);

                        if (tabMenifestDetails.Rows[i][0].ToString().ToUpper() == "BULK")
                        {
                            totalAWB = 1 + totalAWB;
                            totalAWBPCS = totalAWBPCS + int.Parse(tabMenifestDetails.Rows[i][7].ToString());
                            totalAWBWt = totalAWBWt + float.Parse(tabMenifestDetails.Rows[i][8].ToString());
                            totalAWBVol = totalAWBVol + float.Parse(tabMenifestDetails.Rows[i][9].ToString());
                        }
                        else
                        {
                            TotalULD = 1 + TotalULD;
                            totalULDAWB = 1 + totalULDAWB;
                            totalULDPCS = totalULDPCS + int.Parse(tabMenifestDetails.Rows[i][7].ToString());
                            totalULDWt = totalULDWt + float.Parse(tabMenifestDetails.Rows[i][8].ToString());
                            totalULDVol = totalULDVol + float.Parse(tabMenifestDetails.Rows[i][9].ToString());

                        }


                        gdvULDDetails.DataSource = tabMenifestDetails;// ds;
                        gdvULDDetails.DataBind();
                        try
                        {
                            ChangeButtonStatus();
                        }
                        catch (Exception)
                        {


                        }


                    }
                    //ShowULDAWBSummary();

                    //  gdvULDDetails.DataSource = ds;
                    gdvULDDetails.DataBind();
                    try
                    {
                        ChangeButtonStatus();
                    }
                    catch (Exception)
                    {


                    }
                    gdvULDDetails.Visible = true;
                    btnSplitUnassign.Enabled = true;

                    //Make the Data Grid rows in color when the values are in -Ve

                    DataSet LoadPlan = new DataSet("Exp_Manifest_btnListClick_LoadPlan");
                    LoadPlan = (DataSet)Session["dsLoad"];

                    if (LoadPlan != null && LoadPlan.Tables.Count > 0 && LoadPlan.Tables[0].Rows.Count > 0)
                    {
                        for (int intCount = 0; intCount < LoadPlan.Tables[0].Rows.Count; intCount++)
                        {
                            if (Convert.ToInt32(LoadPlan.Tables[0].Rows[intCount]["PiecesCount"]) < 0)
                            {
                                gdvULDLoadPlanAWB.Rows[intCount].BackColor = CommonUtility.ColorHighlightedGrid;
                                string AWBNumber = Convert.ToString(LoadPlan.Tables[0].Rows[intCount]["AWBNumber"]);
                                for (int intMan = 0; intMan < gdvULDDetails.Rows.Count; intMan++)
                                {
                                    if (((Label)gdvULDDetails.Rows[intMan].FindControl("lblAWBno")).Text.Trim() == AWBNumber &&
                                            ((Label)gdvULDDetails.Rows[intMan].FindControl("lblULDno")).Text.Trim().ToUpper() == "BULK")
                                    {
                                        gdvULDDetails.Rows[intMan].BackColor = CommonUtility.ColorHighlightedGrid;
                                    }
                                }
                            }
                        }
                    }

                    if (LoadPlan != null)
                        LoadPlan.Dispose();

                    //End
                }
                HighlightDataGridRows();

                #region Update Epouch Uploaded Count
                try
                {

                    EpouchBAL objEpouch = new EpouchBAL();
                    btnePouch.Text = "ePouch (" + objEpouch.GetEpouchFlightsCount(txtFlightCode.Text.Trim() + txtFlightID.Text.Trim(), TextBoxdate.Text.Trim()) + ")";
                    objEpouch = null;
                }
                catch (Exception Ex)
                { }
                #endregion
                #region RgnNotes 
                bool flag = CommonUtility.ShowNotes(string.Empty, string.Empty, txtFlightCode.Text.Trim() + txtFlightID.Text.Trim(), (TextBoxdate.Text.Trim()));
                    imgNotebtn.Visible = flag;
              
                #endregion
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (dsddldata != null)
                    dsddldata.Dispose();
                if (tabMenifestDetails != null)
                    tabMenifestDetails.Dispose();
                if (ds != null)
                    ds.Dispose();
                if (dsFlightDetails != null)
                    dsFlightDetails.Dispose();
                if (fltPOU != null)
                    fltPOU.Dispose();
            }
        }
        #endregion Flight List Button Click

        #region Flight Clear Button  Click
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            txtFlightID.Text = "";
            TextBoxdate.Text = dtCurrentDate.ToString("dd/MM/yyyy");// System.DateTime.Now.ToString("dd/MM/yyyy");
            txtFlightID.Focus();
            Session["ExpManifest_FltOrigin"] = null;
            //Need to add the logic to clear the screen.
            Response.Redirect("~/GHA_frmExportManifest.aspx");
            //End
        }
        #endregion Flight Clear Button  Click

        #region Load Plan List Button Click
        protected void BtnLoadPlanRefList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                FillSelectULDDDL();

                FillAWBGridMain(ddlMainPOU.SelectedValue);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion Load Plan List Button Click

        #region Load Plan AWB Grid Fill

        public void FillAWBGrid()
        {
            DataSet dsawb = new DataSet("Exp_Manifest_FillAWBGrid_dsawb");
            try
            {
                string flightID = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                DateTime FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);

                //  dsawb = objExpMani.GetAwbTabdetails(ddlMainPOU.SelectedItem.Value.ToString(), flightID);//further research to be done

                string ULDNumber = string.Empty;

                if (ddlULDNo.Text.ToUpper() == "SELECT")
                    ULDNumber = "";

                dsawb = objExpMani.GetAwbTabdetails_GHA(lblDepAirport.Text, flightID, FlightDate, txtAWBPrefix.Text.Trim(), txtAWBNo.Text.Trim(), ULDNumber);


                // dsawb1 = dsawb.Copy();
                if (dsawb != null)
                {
                    gdvULDLoadPlanAWB.DataSource = dsawb;
                    gdvULDLoadPlanAWB.DataBind();
                    try
                    {
                        ChangeButtonStatus();
                    }
                    catch (Exception)
                    {


                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dsawb != null)
                    dsawb.Dispose();
            }
        }


        #endregion Load Plan AWB Grid Fill

        public void FillAWBGridMain(string POU)
        {
            DataSet dsawb = new DataSet("Exp_Manifest_FillAWBGridMain_dsawb");
            DataTable MemDetails = new DataTable("Exp_Manifest_FillAWBGridMain_MemDetails");

            try
            {
                string flightID = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();

                //DateTime dt=DateTime.Parse(TextBoxdate.Text.Trim();
                DateTime FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);
                //11 may addded

                string ULDNumber = string.Empty;

                if (ddlULDNo.Text.ToUpper() == "SELECT" || ddlULDNo.Text.ToUpper() == "SELECT ULD")
                    ULDNumber = "";

                if (POU == "ALL")
                    POU = "";

                if (POU == "")
                    dsawb = objExpMani.GetAwbTabdetails_GHA(lblDepAirport.Text, flightID, FlightDate, txtAWBPrefix.Text.Trim(), txtAWBNo.Text.Trim(), ULDNumber);
                else
                    dsawb = objExpMani.GetAwbTabdetails_GHA(lblDepAirport.Text, flightID, FlightDate, txtAWBPrefix.Text.Trim(), txtAWBNo.Text.Trim(), ULDNumber, POU);
                //  dsawb = objExpMani.GetAwbTabdetails(Destination.ToString(), flightID);//further research to be done
                Session["dsLoad"] = dsawb;

                MemDetails.Columns.Add("lblCartNumber");
                MemDetails.Columns.Add("lblAWBno");
                MemDetails.Columns.Add("lblPieces");
                MemDetails.Columns.Add("lblWeight");
                MemDetails.Columns.Add("lblAvlPCS");
                MemDetails.Columns.Add("lblAvlWgt");

                MemDetails.Columns.Add("lblTotPCS");
                MemDetails.Columns.Add("lblTotWgt");

                //MemDetails.Rows.Add(MemDetails.NewRow());
                for (int j = 0; j < dsawb.Tables[0].Rows.Count; j++)
                {
                    DataRow l_Datarow = MemDetails.NewRow();

                    MemDetails.Rows.Add(l_Datarow);
                }
                gdvULDLoadPlanAWB.DataSource = (DataTable)MemDetails;
                gdvULDLoadPlanAWB.DataBind();
                try
                {
                    ChangeButtonStatus();

                    if (dsawb != null)
                    {
                        Session["ULDData"] = dsawb.Tables[3];
                        if (dsawb.Tables[3].Rows.Count > 0)
                        {
                            gdvULDLoadPlan.DataSource = dsawb.Tables[3];
                            Session["ULDData"] = dsawb.Tables[3];
                            Session["ULDAWBData"] = dsawb.Tables[4];
                            gdvULDLoadPlan.DataBind();

                        }
                    }

                }
                catch (Exception)
                {

                }


                if (dsawb != null)
                {
                    // gdvULDLoadPlanAWB.DataSource = dsawb;


                    for (int i = 0; i < dsawb.Tables[0].Rows.Count; i++)
                    {

                        //  tabMenifestDetails.Rows.Add(l_Datarow);
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAWBno")).Text = dsawb.Tables[0].Rows[i][0].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblPieces")).Text = dsawb.Tables[0].Rows[i][1].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblWeight")).Text = dsawb.Tables[0].Rows[i][2].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlPCS")).Text = dsawb.Tables[0].Rows[i][3].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlWgt")).Text = dsawb.Tables[0].Rows[i][4].ToString();

                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotPCS")).Text = dsawb.Tables[0].Rows[i]["AWBPcs"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotWgt")).Text = dsawb.Tables[0].Rows[i]["AWBGwt"].ToString();

                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblFltFlag")).Text = dsawb.Tables[0].Rows[i]["FltNo"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblCartNumber")).Text = dsawb.Tables[0].Rows[i]["CartNumber"].ToString();
                        //MemDetails.Rows.Add(l_Datarow);

                        //gdvULDLoadPlanAWB.DataSource = (DataTable)MemDetails;
                        //gdvULDLoadPlanAWB.DataBind();

                    }
                    Session["AWBdata"] = dsawb.Tables[0];

                    Session["AddToManifestAWBdata"] = dsawb.Tables[0];
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (dsawb != null)
                    dsawb.Dispose();
                if (MemDetails != null)
                    MemDetails.Dispose();
            }
        }

        #region Show ULD AWB Summary
        public void ShowULDAWBSummary()
        {
            int ULDCount = 0, ULDPcs = 0, BulkCount = 0, BulkPcs = 0;
            decimal ULDWt = 0, BulkWt = 0, ULDVol = 0, BulkVol = 0;
            string Bulk = string.Empty;

            for (int intCount = 0; intCount < gdvULDDetails.Rows.Count; intCount++)
            {
                Bulk = ((Label)gdvULDDetails.Rows[intCount].FindControl("lblULDno")).Text.Trim();

                if (Bulk.ToUpper() == "BULK")
                {
                    BulkCount = BulkCount + 1;
                    BulkPcs = BulkPcs + Convert.ToInt32(((Label)gdvULDDetails.Rows[intCount].FindControl("lblMftPcs")).Text.Trim());
                    BulkWt = BulkWt + Convert.ToDecimal(((Label)gdvULDDetails.Rows[intCount].FindControl("lblMftWt")).Text.Trim());
                    //BulkVol = BulkVol + Convert.ToDecimal(gdvULDDetails.Rows[intCount].Cells[21].Text.Trim());
                }
                else
                {
                    ULDCount = ULDCount + 1;
                    ULDPcs = ULDPcs + Convert.ToInt32(((Label)gdvULDDetails.Rows[intCount].FindControl("lblMftPcs")).Text.Trim());
                    ULDWt = ULDWt + Convert.ToDecimal(((Label)gdvULDDetails.Rows[intCount].FindControl("lblMftWt")).Text.Trim());
                    //ULDVol = ULDVol + Convert.ToDecimal(gdvULDDetails.Rows[intCount].Cells[21].Text.Trim());
                }
            }

            lblULDs.Text = ULDCount.ToString();
            lblULDAWBs.Text = ULDCount.ToString();
            lblULDPCS.Text = ULDPcs.ToString();
            lblULDWt.Text = ULDWt.ToString();
            lblULDVol.Text = ULDVol.ToString();
            lblAWBCnt.Text = BulkCount.ToString();
            lblAWBWt.Text = BulkWt.ToString();
            lblAWBPCS.Text = BulkPcs.ToString();
            lblAWBVol.Text = BulkVol.ToString();
        }
        #endregion

        #region Select ULD DDl Fill
        public void FillSelectULDDDL()
        {
            DataSet dsULD = new DataSet("Exp_Manifest_FillSelectULDDL_dsULD");
            try
            {
                ddlSelectULD.Items.Clear();
                string flightID = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();

                dsULD = objExpMani.GetULDTabdetails(ddlMainPOU.SelectedItem.Value.ToString(), flightID);
                Session["ULDData"] = dsULD.Tables[0];
                if (dsULD != null)
                {
                    ddlSelectULD.Items.Add("Select");
                    // ddlSelectULD.Items.Add("Bulk");

                    ddlSelectULD.DataSource = dsULD;
                    ddlSelectULD.DataTextField = "ULDno";
                    ddlSelectULD.DataValueField = "ULDno";


                    ddlSelectULD.DataBind();

                }
                else
                {
                    ddlSelectULD.Items.Add("Select ULD");
                    //ddlSelectULD.Items.Add("Bulk");
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dsULD != null)
                    dsULD.Dispose();
            }
        }

        #endregion  Select ULD DDl Fill

        #region Load Plan Clear Button Click
        protected void BtnLoadPlanRefClear_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            // ddlMainPOU.SelectedItem.Value.ToString() = "";
            ddlMainPOU.SelectedIndex = 0; //clear ddlMainPOU

            txtPOU.Focus();
        }
        #endregion Load Plan Clear Button Click

        #region Clear Gridview ULD and all textbox
        protected void ClearGrid()
        {
            gdvULDDetails.DataSource = null;
            gdvULDDetails.DataBind();
            try
            {
                ChangeButtonStatus();
            }
            catch (Exception)
            {


            }
            lblULDs.Text = lblULDAWBs.Text = lblULDPCS.Text = lblULDWt.Text = lblULDVol.Text = lblAWBCnt.Text = lblAWBPCS.Text = lblAWBWt.Text = lblAWBVol.Text = "";


        }
        #endregion

        #region ULD Grid add rows
        protected bool addgrid(string memid, string memname)
        {
            DataTable DSULDadddata = new DataTable("Exp_Manifest_addgrid_DSULDadddata");
            try
            {
                //check the datagrid for already existing ULD(to avoid duplicate data)

                for (int j = 0; j < gdvULDLoadPlan.Rows.Count; j++)
                {
                    if (memname == gdvULDLoadPlan.Rows[j].Cells[1].Text)
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "AlreadyAvailable();", true);
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "ULD Already Available";

                        return true;
                    }

                }

                if (Session["ULDData"] != null)
                {
                    // MemDetails = (DataTable)Session["ULDData"];
                    // DataTable DSULDadddata = (DataTable)Session["ULDData"];
                    // createPO();
                    DSULDadddata = (DataTable)Session["ULDData"];

                    for (int i = 0; i < DSULDadddata.Rows.Count; i++)
                    {
                        if (DSULDadddata.Rows[i][0].ToString() == memname)
                        {

                            MemDetails = (DataTable)Session["DataTableTemp"];

                            DataRow l_Datarow = MemDetails.NewRow();
                            l_Datarow["ULDno"] = DSULDadddata.Rows[i][0].ToString();
                            l_Datarow["POU"] = DSULDadddata.Rows[i][1].ToString();

                            //Double tareWeight=0.0;
                            //if (DSULDadddata.Rows[i][2].ToString() != "" )
                            //{
                            //    tareWeight=Convert.ToDouble(DSULDadddata.Rows[i][2]);
                            //}
                            //l_Datarow["TareWeight"] = tareWeight;
                            l_Datarow["TareWeight"] = Convert.ToDouble(DSULDadddata.Rows[i][2]);

                            MemDetails.Rows.Add(l_Datarow);
                            if (MemDetails != null && MemDetails.Rows.Count > 0)
                            {
                                Session["DataTableTemp"] = MemDetails;
                            }
                            loadgrid();
                        }
                    }
                }
                else
                {
                    createPO();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (DSULDadddata != null)
                    DSULDadddata.Dispose();
            }
            return false;
        }
        #endregion

        protected void createPO()
        {
            MemDetails = new DataTable("Exp_Manifest_createPO_MemDetails");

            MemDetails.Columns.Add("ULDno");
            MemDetails.Columns.Add("POU");
            MemDetails.Columns.Add("TareWeight");
        }

        #region Load ULD Load plan gridview
        protected void loadgrid()
        {
            if (Session["DataTableTemp"] != null)
            {
                gdvULDLoadPlan.DataSource = (DataTable)Session["DataTableTemp"];

            }
            else
            {
                gdvULDLoadPlan.DataSource = null;
            }
            gdvULDLoadPlan.DataBind();
            try
            {
                ChangeButtonStatus();
            }
            catch (Exception)
            {


            }
        }

        #endregion Load ULD Load plan gridview

        protected void btnULDSummary1_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
        }

        #region ADD TO Manifest

        protected void BtnAddtoManifest_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            
            if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                    lblDepAirport.Text) == "D")
            {
                BtnList_Click(this, new EventArgs());
                lblStatus.Text = "Flight is departed. Please reopen flight.";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            DataTable dsULDData = new DataTable("Exp_Manifest_BtnAddtoManifest_dsULDData");
            DataTable dsULDAWBData = new DataTable("Exp_Manifest_BtnAddtoManifest_dsULDAWBData");
            DataSet dsDestDetails = new DataSet("Exp_Manifest_BtnAddToManifest_dsDestDetails");
            try
            {
                SelectedULDnos = "";

                int CntAWBChk = 0;
                for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                    {
                        CntAWBChk += 1;
                    }
                }

                int CntULDChk = 0;
                for (int i = 0; i < gdvULDLoadPlan.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDLoadPlan.Rows[i].FindControl("Check1")).Checked == true)
                    {
                        CntULDChk += 1;
                        SelectedULDnos += "'" + (gdvULDLoadPlan.Rows[i].Cells[1].Text) + "',";
                    }

                }
                //Added on 26 sept
                if (gdvULDLoadPlan.Rows.Count > 0)
                {
                    if (CntAWBChk == 0 && CntULDChk == 0)//if none of AWB ror ULD is selectrd n  addto manifest is clicked
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "alertSelectAWBULD();", true);
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Select ULD from Tab And/Or AWB from TAB to add to Manifest";
                        return;
                    }
                }

                #region "Process Selected AWB for Manifest"

                if (CntAWBChk != 0)
                {
                    Session["Split"] = "A";
                    //btnSave.Enabled = true;
                    //btnDepartFit.Enabled = true;

                    AddAWBDatarowsToManifestGrid();

                    //to remove the awbnos and ulds from current grid once added to manifest details grid. 
                    for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
                    {
                        if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                        {
                            gdvULDLoadPlanAWB.Rows[i].Visible = false;
                        }
                    }
                }

                #endregion "Process Selected AWB for Manifest"

                //if (CntAWBChk != 0 && CntULDChk != 0)//to add AWB and ULD rows to manifest if both are selected
                //{
                //    AddULDnAWBDatarowsToManifestGrid();
                //}

                hdnManifestFlag.Value = "1";

                //Add ULD to manifest

                #region "Process Selected ULD for Manifest"

                if (CntULDChk != 0)
                {
                    Session["Split"] = "A";
                    if (Session["ULDData"] != null)
                    {
                        try
                        {
                            string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby = "", AWBDest = "", CommoCode = "", AWBOrigin = "";
                            int PCS = 0, AVLPCS = 0, AWBPcs = 0;
                            double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, AWBGwt = 0;
                            //string Updatedon = "";
                            string desc = "", Vol = "", Location = "";
                            bool IsManifest = false;

                            FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                            POL = Convert.ToString(Session["Station"]);
                            // POU = ddlMainPOU.SelectedItem.Value.ToString();
                            if (ddlMainPOU.Items.Count > 0)
                                POU = ddlMainPOU.Items[0].Value.ToString();
                            Updatedby = Convert.ToString(Session["Username"]);
                            ULDno = "BULK";
                            ULDwgt = 0;
                            DateTime FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);
                            // dsawbData = objExpMani.GetAwbTabdetails(lblDepAirport.Text, FLTno, FlightDate);

                            //  dsawb1 = (DataTable)Session["AddToManifestAWBdata"];// Session["AWBdata"];
                            // }
                            if (Session["ULDData"] != null)
                            {
                                dsULDData = (DataTable)Session["ULDData"];
                                dsULDAWBData = (DataTable)Session["ULDAWBData"];
                            }

                            IsManifest = false;
                            //for (int j = 0; j < gdvULDLoadPlan.Rows.Count; j++)
                            tabMenifestDetails = (DataTable)Session["ManifestGridData"];
                            for (int j = gdvULDLoadPlan.Rows.Count - 1; j >= 0; j--)
                            {
                                if (((CheckBox)gdvULDLoadPlan.Rows[j].FindControl("Check1")).Checked == true)
                                {
                                    for (int i = 0; i < dsULDAWBData.Rows.Count; i++)
                                    {

                                        // ((CheckBox)gdvULDLoadPlan.Rows[j].FindControl("Check1")).Checked = false;
                                        //   gdvULDLoadPlanAWB.Rows[i].Visible = false;
                                        //Appended 25apr
                                        ULDno = gdvULDLoadPlan.Rows[j].Cells[1].Text.ToString().Trim();
                                        string ULDCheck = gdvULDLoadPlan.Rows[j].Cells[0].Text;
                                        if (ULDno == dsULDAWBData.Rows[i]["ULDNo"].ToString())
                                        {
                                            AWBno = dsULDAWBData.Rows[i]["AWBNo"].ToString();
                                            PCS = Convert.ToInt32(dsULDAWBData.Rows[i]["Pieces"].ToString());
                                            WGT = Convert.ToDouble(dsULDAWBData.Rows[i]["Gweight"].ToString());
                                            AVLPCS = Convert.ToInt32(dsULDAWBData.Rows[i]["Pieces"].ToString());
                                            AVLWGT = Convert.ToDouble(dsULDAWBData.Rows[i]["Gweight"].ToString());

                                            AWBPcs = Convert.ToInt32(dsULDAWBData.Rows[i]["BookedPcs"].ToString());
                                            AWBGwt = Convert.ToDouble(dsULDAWBData.Rows[i]["BookedWgt"].ToString());

                                            desc = dsULDAWBData.Rows[i]["CommodityDesc"].ToString();
                                            Vol = dsULDAWBData.Rows[i]["VolumetricWeight"].ToString();
                                            AWBOrigin = dsULDAWBData.Rows[i]["ULDOrigin"].ToString();
                                            AWBDest = dsULDAWBData.Rows[i]["ULDDest"].ToString();
                                            CommoCode = dsULDAWBData.Rows[i]["CommodityCode"].ToString();
                                            Location = dsULDAWBData.Rows[i]["Location"].ToString();
                                            string FltNo = dsULDAWBData.Rows[i]["FltNo"].ToString();
                                            string CartNumber = dsULDAWBData.Rows[i]["CartNumber"].ToString();

                                            //Add ULD Info to Grid
                                            DataRow l_Datarow = tabMenifestDetails.NewRow();

                                            //Validate ULD POU against POUs of Flights (CS-420)
                                            if (ULDno.ToUpper() != "BULK" && (ddlMainPOU.Items.Count <= 0 || 
                                                !ddlMainPOU.Items.Contains(new ListItem(AWBDest.ToUpper(),AWBDest.ToUpper()))))
                                            {
                                                lblStatus.Text = "POU of " + ULDno.ToUpper() + " not matching with Flight POUs. " +
                                                "You cannot add this ULD in Manifest.";
                                                lblStatus.ForeColor = Color.Red;
                                                return;
                                            }

                                            //If valid POU is present.
                                            if (POU.Trim() == "")
                                            {
                                                POU = AWBDest;
                                            }

                                            l_Datarow["ULDno"] = ULDno;
                                            l_Datarow["POU"] = POU;
                                            l_Datarow["POL"] = POL;
                                            l_Datarow["AWBDest"] = AWBDest;
                                            l_Datarow["Counter"] = "";
                                            l_Datarow["AWBno"] = AWBno;
                                            l_Datarow["SCC"] = CommoCode;
                                            l_Datarow["PCS"] = PCS;
                                            l_Datarow["GrossWgt"] = WGT;
                                            l_Datarow["VOL"] = Vol;
                                            l_Datarow["StatedPCS"] = AVLPCS;
                                            l_Datarow["StatedWgt"] = AVLWGT;

                                            l_Datarow["BookedPCS"] = AWBPcs;
                                            l_Datarow["BookedWgt"] = AWBGwt;

                                            l_Datarow["Desc"] = desc;
                                            l_Datarow["Orign"] = AWBOrigin;
                                            l_Datarow["Dest"] = AWBDest;
                                            l_Datarow["Manifested"] = IsManifest;
                                            l_Datarow["LoadingPriority"] = "";
                                            l_Datarow["Remark"] = "";
                                            l_Datarow["Bonded"] = "";
                                            l_Datarow["Location"] = Location;
                                            l_Datarow["FltNo"] = FltNo;
                                            l_Datarow["CartNumber"] = CartNumber;

                                            tabMenifestDetails.Rows.Add(l_Datarow);

                                            //End
                                            dsULDAWBData.Rows[i].Delete();
                                            //dsULDAWBData.AcceptChanges();
                                        }                                        
                                    }

                                    //tabMenifestDetails = (DataTable)Session["ManifestGridData"];
                                    dsULDAWBData.AcceptChanges();
                                    if (tabMenifestDetails != null && tabMenifestDetails.Rows.Count > 0)
                                    {
                                        Session["ManifestGridData"] = tabMenifestDetails;
                                    }

                                    gdvULDDetails.DataSource = "";

                                    gdvULDDetails.DataSource = (DataTable)Session["ManifestGridData"];
                                    gdvULDDetails.DataBind();

                                    Session["GDVULDDetails"] = tabMenifestDetails;

                                    dsULDData.Rows[j].Delete();
                                    dsULDData.AcceptChanges();
                                }
                            }

                            gdvULDLoadPlan.DataSource = null;
                            gdvULDLoadPlan.DataBind();

                            gdvULDLoadPlan.DataSource = dsULDData;
                            gdvULDLoadPlan.DataBind();
                            //Added on 27 Sept

                            Session["ULDAWBdata"] = dsULDAWBData;// dsawbData.Tables[1];
                            Session["AddToManifestAWBdata"] = dsULDAWBData;

                            for (int j = 0; j < tabMenifestDetails.Rows.Count; j++)
                            {
                                if (tabMenifestDetails.Rows[j][0].ToString().ToUpper() == "BULK")
                                {
                                    totalAWB = 1 + totalAWB;
                                    totalAWBPCS = totalAWBPCS + int.Parse(tabMenifestDetails.Rows[j][7].ToString());
                                    totalAWBWt = totalAWBWt + float.Parse(tabMenifestDetails.Rows[j][8].ToString());
                                    totalAWBVol = totalAWBVol + int.Parse(tabMenifestDetails.Rows[j][9].ToString());
                                }
                            }
                            //ShowULDAWBSummary();

                            hdnManifestFlag.Value = "1";
                        }
                        catch (Exception ex)
                        { }
                    }
                }

                #endregion "Process Selected ULD for Manifest"

                HighlightDataGridRows();

                btnSave_Click(null, null);
            }
            catch (Exception ex)
            { }
            finally
            {
                if (dsULDData != null)
                    dsULDData.Dispose();
                if (dsULDAWBData != null)
                    dsULDAWBData.Dispose();
                if (dsDestDetails != null)
                    dsDestDetails.Dispose();
            }
        }

        #endregion ADD TO Manifest

        #region Add Checked AWB Data rows to Manifest Grid

        public void AddAWBDatarowsToManifestGrid()
        {
            DataTable dsawb1 = new DataTable("Exp_Manifest_AddAWBDatarowsToManifestGrid_dsawb1");
            DataTable dsawb2 = new DataTable("Exp_Manifest_AddAWBDatrowsToManifestGrid_dsawb2");
            DataSet dsawbData = new DataSet("Exp_Manifest_AddAWBDatarowsToManifestGrid_dsawbData");
            try
            {
                string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby = "", AWBDest = "", CommoCode = "", AWBOrigin = "";
                int PCS = 0, AVLPCS = 0, AWBPcs = 0;
                double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, AWBGwt = 0;
                //string Updatedon = "";
                string desc = "", Vol = "";
                bool IsManifest = false;
                string IsBonded = string.Empty, AWBType = string.Empty, Location = string.Empty, FltNo = string.Empty;
                string CartNumber = string.Empty;string Remark =string.Empty;

                FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                POL = Convert.ToString(Session["Station"]);
                // POU = ddlMainPOU.SelectedItem.Value.ToString();
                //POU = ddlMainPOU.Items[0].Value.ToString();
                if (ddlMainPOU.Items.Count > 0)
                    POU = ddlMainPOU.Items[0].Value.ToString();
                Updatedby = Convert.ToString(Session["Username"]);
                ULDno = "BULK";
                ULDwgt = 0;
                DateTime FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);

                dsawbData = objExpMani.GetAwbTabdetails_GHA(lblDepAirport.Text, FLTno, FlightDate, "", "", "");

                dsawb1 = (DataTable)Session["AddToManifestAWBdata"];// Session["AWBdata"];

                if (Session["AWBdata"] != null)
                {
                    dsawb1 = (DataTable)Session["AWBdata"];
                    dsawb2 = dsawb1.Copy();
                }

                IsManifest = false;
                for (int i = 0; i < dsawb2.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                    {
                        ((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked = false;
                        //   gdvULDLoadPlanAWB.Rows[i].Visible = false;
                        //Appended 25apr
                        AWBno = ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAWBno")).Text; //gdvULDLoadPlanAWB.Rows[i].Cells[1].Text;
                        PCS = Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblPieces")).Text);
                        WGT = Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblWeight")).Text);
                        AVLPCS = Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlPCS")).Text);
                        AVLWGT = Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlWgt")).Text);

                        AWBPcs = Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotPCS")).Text);
                        AWBGwt = Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotWgt")).Text);
                        CartNumber = ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblCartNumber")).Text.Trim();

                        // Updatedon = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                        try
                        {
                            if (dsawb1.Rows.Count > 0)
                            {
                                for (int t = 0; t < dsawb1.Rows.Count; t++)
                                {
                                    if (dsawb1.Rows[t][0].ToString() == AWBno &&
                                        dsawb1.Rows[t]["CartNumber"].ToString().Trim().ToUpper() == CartNumber)
                                    {
                                        AWBDest = dsawbData.Tables[1].Rows[t][5].ToString();
                                        desc = dsawbData.Tables[1].Rows[t][6].ToString();
                                        Vol = dsawbData.Tables[1].Rows[t][7].ToString();
                                        AWBOrigin = dsawbData.Tables[1].Rows[t]["OrginCode"].ToString();

                                        CommoCode = dsawbData.Tables[1].Rows[t]["CommodityCode"].ToString();
                                        IsBonded = dsawbData.Tables[1].Rows[t]["IsBonded"].ToString();
                                        AWBType = dsawbData.Tables[1].Rows[t]["Type"].ToString();
                                        Location = dsawbData.Tables[1].Rows[t]["Location"].ToString();
                                        FltNo = dsawbData.Tables[1].Rows[t]["FltNo"].ToString();
                                        POU = dsawbData.Tables[1].Rows[t]["FltDestination"].ToString();
                                        Remark = "";
                                       try
                                        {
                                         
                                            string awbnumber = "";
                                            if (dsawbData.Tables[2].Rows.Count > 0 && dsawbData.Tables[2]!=null)
                                            {
                                                for (int k = 0; k < dsawbData.Tables[2].Rows.Count; k++)
                                                {
                                                   
                                                    string awb = AWBno.Substring(AWBno.Length - 8);
                                                    awbnumber = dsawbData.Tables[2].Rows[k]["AWBNo"].ToString();
                                                    if (awbnumber == awb)
                                                    {
                                                        Remark = dsawbData.Tables[2].Rows[k]["Remarks"].ToString();
                                                        break;
                                                    }
                                                }
                                            }


                                        }
                                        catch(Exception ex)
                                        {}
                                        
                                        //Added on 27 sept
                                        //dsawb1.Tables[1].Rows.
                                        dsawb1.Rows[t].Delete();
                                        dsawbData.Tables[1].Rows[t].Delete();
                                    }
                                }
                                dsawb1.AcceptChanges();
                                dsawbData.Tables[1].AcceptChanges();
                                //adding collected data to gridview rows one by one
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                        tabMenifestDetails = (DataTable)Session["ManifestGridData"];
                        bool IsAvialble = false;
                        try
                        {
                            if (tabMenifestDetails.Rows.Count > 0)
                            {
                                for (int n = 0; n < tabMenifestDetails.Rows.Count; n++)
                                {
                                    if (AWBno == tabMenifestDetails.Rows[n]["AWBNo"].ToString() && 
                                        tabMenifestDetails.Rows[n]["ULDNo"].ToString().Trim().ToUpper() == "BULK" &&
                                        CartNumber == tabMenifestDetails.Rows[n]["CartNumber"].ToString().Trim().ToUpper())
                                    {
                                        IsAvialble = true;
                                        tabMenifestDetails.Rows[n][7] = (Convert.ToInt32(tabMenifestDetails.Rows[n][7].ToString()) + PCS);
                                        tabMenifestDetails.Rows[n][8] = Convert.ToDouble(tabMenifestDetails.Rows[n][8].ToString()) + WGT;
                                        //break from loop if matching row is found.
                                        //Set is manifested = 'false' for resepctive AWB row.
                                        tabMenifestDetails.Rows[n]["Manifested"] = false;
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        if (IsAvialble == false)
                        {
                            try
                            {
                                string ActAWB = AWBno.Substring(AWBno.Length - 8);

                                DataSet dsDestDetails = new DataSet("Exp_Manifest_AddAWBDatarowToManifestGrid_dsDestDetails");
                                dsDestDetails = objExpMani.GetULDAWBData(ActAWB);
                                if (dsDestDetails.Tables[0].Rows.Count > 0)
                                {
                                    AWBDest = dsDestDetails.Tables[0].Rows[0][0].ToString();
                                    desc = dsDestDetails.Tables[0].Rows[0][1].ToString();
                                    Vol = dsDestDetails.Tables[0].Rows[0][2].ToString();
                                    CommoCode = dsDestDetails.Tables[0].Rows[0]["CommodityCode"].ToString();
                                    AWBOrigin = dsDestDetails.Tables[0].Rows[0]["OriginCode"].ToString();
                                }
                            }
                            catch (Exception ex)
                            {
                            }

                            DataRow l_Datarow = tabMenifestDetails.NewRow();

                            l_Datarow["ULDno"] = ULDno;
                            l_Datarow["POU"] = POU;
                            l_Datarow["POL"] = POL;
                            l_Datarow["AWBDest"] = AWBDest;
                            l_Datarow["Counter"] = AWBType;
                            l_Datarow["AWBno"] = AWBno;
                            l_Datarow["SCC"] = CommoCode;
                            l_Datarow["PCS"] = PCS;
                            l_Datarow["GrossWgt"] = WGT;
                            l_Datarow["VOL"] = Vol;
                            l_Datarow["StatedPCS"] = AVLPCS;
                            l_Datarow["StatedWgt"] = AVLWGT;
                            l_Datarow["BookedPCS"] = AWBPcs;
                            l_Datarow["BookedWgt"] = AWBGwt;
                            l_Datarow["Desc"] = desc;
                            l_Datarow["Orign"] = AWBOrigin;
                            l_Datarow["Dest"] = AWBDest;
                            l_Datarow["Manifested"] = IsManifest;
                            l_Datarow["LoadingPriority"] ="";
                            l_Datarow["Remark"] = Remark;
                            l_Datarow["Bonded"] = IsBonded;
                            l_Datarow["Location"] = Location;
                            l_Datarow["FltNo"] = FltNo;
                            l_Datarow["CartNumber"] = CartNumber;

                            tabMenifestDetails.Rows.Add(l_Datarow);
                        }

                        if (tabMenifestDetails != null && tabMenifestDetails.Rows.Count > 0)
                        {
                            Session["ManifestGridData"] = tabMenifestDetails;
                        }

                        gdvULDDetails.DataSource = "";

                        gdvULDDetails.DataSource = (DataTable)Session["ManifestGridData"];
                        gdvULDDetails.DataBind();
                        try
                        {
                            ChangeButtonStatus();
                        }
                        catch (Exception)
                        {
                        }

                        Session["GDVULDDetails"] = tabMenifestDetails;
                    }
                }

                gdvULDLoadPlanAWB.DataSource = null;
                gdvULDLoadPlanAWB.DataBind();

                gdvULDLoadPlanAWB.DataSource = dsawb1;
                gdvULDLoadPlanAWB.DataBind();
                try
                {
                    ChangeButtonStatus();
                }
                catch (Exception)
                {

                }
                //Added on 27 Sept

                if (dsawb1.Rows.Count > 0)
                {
                    for (int k = 0; k < dsawb1.Rows.Count; k++)
                    {
                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAWBno")).Text = dsawb1.Rows[k][0].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text = dsawb1.Rows[k][1].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWeight")).Text = dsawb1.Rows[k][2].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlPCS")).Text = dsawb1.Rows[k][3].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlWgt")).Text = dsawb1.Rows[k][4].ToString();

                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblTotPCS")).Text = dsawb1.Rows[k]["AWBPcs"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblTotWgt")).Text = dsawb1.Rows[k]["AWBGwt"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblFltFlag")).Text = dsawb1.Rows[k]["FltNo"].ToString();
                    }
                }
                Session["AWBdata"] = dsawb1;// dsawbData.Tables[1];
                Session["AddToManifestAWBdata"] = dsawb1;

                for (int j = 0; j < tabMenifestDetails.Rows.Count; j++)
                {
                    if (tabMenifestDetails.Rows[j][0].ToString().ToUpper() == "BULK")
                    {
                        totalAWB = 1 + totalAWB;
                        totalAWBPCS = totalAWBPCS + int.Parse(tabMenifestDetails.Rows[j][7].ToString());
                        totalAWBWt = totalAWBWt + float.Parse(tabMenifestDetails.Rows[j][8].ToString());
                        totalAWBVol = totalAWBVol + int.Parse(tabMenifestDetails.Rows[j][9].ToString());
                    }
                }
                //ShowULDAWBSummary();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (dsawb1 != null)
                    dsawb1.Dispose();
                if (dsawb2 != null)
                    dsawb2.Dispose();
                if (dsawbData != null)
                    dsawbData.Dispose();
            }
        }

        #endregion Add Checked AWB Data rows to Manifest Grid

        #region Add Checked ULD Data rows to Manifest Grid

        public void AddULDDatarowsToManifestGrid()
        {
            DataSet dsawb1 = new DataSet("Exp_Manifest_AddULDDatarowsToManifestGrid_dsawb1");
            DataSet dsGetULDAWBassdata = new DataSet("Exp_Manifest_AddULDDatarowsToManifestGrid_dsGetULDAWBassdata");
            try
            {
                string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby, AWBDest = ""; ;
                int PCS = 0, AVLPCS = 0;
                double WGT = 0.0, AVLWGT = 0.0;
                //string Updatedon = "";
                string vol = "", Desc = "", Location = string.Empty;

                FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                POL = Convert.ToString(Session["Station"]);
                //POU = ddlMainPOU.SelectedItem.Value.ToString();
                POU = ddlMainPOU.Items[0].Value.ToString();
                Updatedby = Convert.ToString(Session["Username"]);

                SelectedULDnos = SelectedULDnos.Remove(SelectedULDnos.Length - 1, 1);
                DateTime FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);

                dsawb1 = objExpMani.GetAwbTabdetails_GHA(lblDepAirport.Text, FLTno, FlightDate, "", "", "");

                bool IsManifest = false;
                string CommCode = string.Empty;

                for (int j = 0; j < gdvULDLoadPlan.Rows.Count; j++)
                {
                    if (((CheckBox)gdvULDLoadPlan.Rows[j].FindControl("Check1")).Checked == true)
                    {
                        string SelectedULD = gdvULDLoadPlan.Rows[j].Cells[1].Text;

                        dsGetULDAWBassdata = objExpMani.GetULDAWBassocitionData(SelectedULD);

                        if (dsGetULDAWBassdata != null && dsGetULDAWBassdata.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsGetULDAWBassdata.Tables[0].Rows.Count; i++)
                            {
                                ULDno = dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(0).ToString();

                                AWBno = dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(3).ToString();
                                PCS = Convert.ToInt32(dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(4).ToString());
                                WGT = Convert.ToDouble(dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(5).ToString());
                                AVLPCS = Convert.ToInt32(dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(6).ToString());
                                AVLWGT = Convert.ToDouble(dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(7).ToString());

                                try
                                {
                                    if (dsawb1.Tables[1].Rows[i][0].ToString() == AWBno)
                                    {
                                        AWBDest = dsawb1.Tables[1].Rows[i][5].ToString();
                                        Desc = dsawb1.Tables[1].Rows[i][6].ToString();
                                        vol = dsawb1.Tables[1].Rows[i][7].ToString();
                                        CommCode = dsawb1.Tables[1].Rows[i]["CommodityCode"].ToString();
                                        Location = dsawb1.Tables[4].Rows[i]["Location"].ToString();
                                        //Added on 27 sept
                                        //dsawb1.Tables[1].Rows.
                                        dsawb1.Tables[1].Rows[i].Delete();
                                    }
                                    //adding collected data to gridview rows one by one
                                }
                                catch (Exception ex)
                                {
                                }

                                //adding collected data to gridview rows one by one

                                tabMenifestDetails = (DataTable)Session["ManifestGridData"];

                                DataRow l_Datarow = tabMenifestDetails.NewRow();

                                l_Datarow["ULDno"] = ULDno;
                                l_Datarow["POU"] = POU;
                                l_Datarow["POL"] = POL;
                                l_Datarow["AWBDest"] = AWBDest;
                                l_Datarow["Counter"] = "";
                                l_Datarow["AWBno"] = AWBno;
                                l_Datarow["SCC"] = CommCode;
                                l_Datarow["PCS"] = PCS;
                                l_Datarow["GrossWgt"] = WGT;
                                l_Datarow["VOL"] = vol;
                                l_Datarow["StatedPCS"] = AVLPCS;
                                l_Datarow["StatedWgt"] = AVLWGT;

                                l_Datarow["Desc"] = Desc;
                                l_Datarow["Orign"] = POL;
                                l_Datarow["Dest"] = AWBDest;
                                l_Datarow["Manifested"] = IsManifest;
                                l_Datarow["LoadingPriority"] = "";
                                l_Datarow["Remark"] = "";
                                l_Datarow["Bonded"] = "";
                                l_Datarow["Location"] = Location;
                                l_Datarow["CartNumber"] = "";

                                tabMenifestDetails.Rows.Add(l_Datarow);
                                if (tabMenifestDetails != null && tabMenifestDetails.Rows.Count > 0)
                                {
                                    Session["ManifestGridData"] = tabMenifestDetails;
                                }
                                if (tabMenifestDetails.Rows[i][0].ToString().ToUpper() == "BULK")
                                {
                                    totalAWB = 1 + totalAWB;
                                    totalAWBPCS = totalAWBPCS + int.Parse(tabMenifestDetails.Rows[i][7].ToString());
                                    totalAWBWt = totalAWBWt + int.Parse(tabMenifestDetails.Rows[i][8].ToString());
                                    totalAWBVol = totalAWBVol + int.Parse(tabMenifestDetails.Rows[i][9].ToString());
                                }
                                else
                                {
                                    TotalULD = 1 + TotalULD;
                                    totalULDAWB = 1 + totalULDAWB;
                                    totalULDPCS = totalULDPCS + int.Parse(tabMenifestDetails.Rows[i][7].ToString());
                                    totalULDWt = totalULDWt + int.Parse(tabMenifestDetails.Rows[i][8].ToString());
                                    totalULDVol = totalULDVol + int.Parse(tabMenifestDetails.Rows[i][9].ToString());
                                }

                                gdvULDDetails.DataSource = (DataTable)Session["ManifestGridData"];
                                gdvULDDetails.DataBind();
                                try
                                {
                                    ChangeButtonStatus();
                                }
                                catch (Exception)
                                {

                                }

                                //Added on 27 Sept

                                if (dsawb1.Tables[1].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsawb1.Tables[1].Rows.Count; i++)
                                    {
                                        //  tabMenifestDetails.Rows.Add(l_Datarow);
                                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAWBno")).Text = dsawb1.Tables[1].Rows[i][0].ToString();
                                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblPieces")).Text = dsawb1.Tables[1].Rows[i][1].ToString();
                                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblWeight")).Text = dsawb1.Tables[1].Rows[i][2].ToString();
                                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlPCS")).Text = dsawb1.Tables[1].Rows[i][3].ToString();
                                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlWgt")).Text = dsawb1.Tables[1].Rows[i][4].ToString();

                                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotPCS")).Text = dsawb1.Tables[1].Rows[i]["AWBPcs"].ToString();
                                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotWgt")).Text = dsawb1.Tables[1].Rows[i]["AWBGwt"].ToString();

                                    }
                                }
                                Session["AWBdata"] = dsawb1.Tables[1];
                            }
                        }
                    }
                }
                //ShowULDAWBSummary();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dsawb1 != null)
                    dsawb1.Dispose();
                if (dsGetULDAWBassdata != null)
                    dsGetULDAWBassdata.Dispose();
            }
        }

        #endregion

        #region ADD ULD AND AWB Data To Manifest Grid

        public void AddULDnAWBDatarowsToManifestGrid()
        {
            AddULDDatarowsToManifestGrid();

            AddAWBDatarowsToManifestGrid();

        }

        #endregion ADD ULD AND AWB Data To Manifest Grid

        #region Save Manifest Grid data To DB
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool res = false;
            string strMode = "";
            lblStatus.Text = "";

            //if (txtTailNo.Text == "")
            //{
            //    lblStatus.Text = "Please enter Tail No";
            //    lblStatus.ForeColor = Color.Red;
            //    txtTailNo.Focus();
            //    return;
            //}
            try
            {
                if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text, 
                    lblDepAirport.Text) == "D")
                {
                    BtnList_Click(this, new EventArgs());
                    lblStatus.Text = "Flight is already departed.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                object[] manDetails = new object[4];
                manDetails[0] = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                manDetails[1] = lblDepAirport.Text;
                manDetails[2] = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);
                manDetails[3] = lblVersionID.Text;

                objExpMani.DeleteManifestDetails(manDetails);

                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby, counter = "", SCC = "", Desc = "";
                    int PCS = 0, AVLPCS = 0;
                    double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, VOL = 0;
                    string Updatedon = "", IMENo = "";
                    string LoadingPriority = "", Remark = "", ULDDest = "", AWBOrigin = "", IsBonded = "", AWBPrefix = "";

                    FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                    Updatedby = Convert.ToString(Session["Username"]);

                    IMENo = txtEMG.Text.Trim();

                    ULDno = ((Label)gdvULDDetails.Rows[i].FindControl("lblULDno")).Text.Trim();
                    POU = ((Label)gdvULDDetails.Rows[i].FindControl("lblPOU")).Text.Trim();
                    //POL = gdvULDDetails.Rows[i].Cells[1].Text.Trim();// Session["Station"].ToString(); 
                    POL = ((Label)gdvULDDetails.Rows[i].FindControl("lblPOL")).Text.Trim();

                    //gdvULDDetails.Rows[i].Cells[3].Text;
                    ULDDest = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBDest")).Text.Trim();
                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblcounter")).Text.Trim() == "&nbsp;")
                    {
                        counter = "0";
                    }
                    else
                    {
                        counter = ((Label)gdvULDDetails.Rows[i].FindControl("lblcounter")).Text.Trim();
                    }

                    string Number = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text.Trim();

                    if (Number.Replace("&nbsp;", "") != "")
                    {
                        AWBPrefix = Number.Substring(0, Number.IndexOf('-'));
                        AWBno = Number.Substring(Number.IndexOf('-') + 1, 8);
                    }
                    else
                    {
                        AWBPrefix = "";
                        AWBno = "";
                    }

                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblSCC")).Text == "&nbsp;")
                    {
                        SCC = "";
                    }
                    else
                    {
                        SCC = ((Label)gdvULDDetails.Rows[i].FindControl("lblSCC")).Text;
                    }

                    PCS = Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblMftPcs")).Text);
                    WGT = Convert.ToDouble(((Label)gdvULDDetails.Rows[i].FindControl("lblMftWt")).Text);
                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblVOL")).Text.Trim() == "&nbsp;" || ((Label)gdvULDDetails.Rows[i].FindControl("lblVOL")).Text.Trim() == "")
                    {
                        VOL = 0;
                    }
                    else
                    {
                        VOL = Convert.ToDouble(((Label)gdvULDDetails.Rows[i].FindControl("lblVOL")).Text);
                    }
                    AVLPCS = Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblStatedPCS")).Text);
                    AVLWGT = Convert.ToDouble(((Label)gdvULDDetails.Rows[i].FindControl("lblStatedWgt")).Text);
                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblCommDesc")).Text == "&nbsp;")
                    {
                        Desc = "";
                    }
                    else
                    {
                        Desc = ((Label)gdvULDDetails.Rows[i].FindControl("lblCommDesc")).Text;
                        Desc = Desc.Replace("&amp;", "&");
                        //Desc for commodity code
                        // Desc = Desc.Substring(3, Desc.Length);
                    }

                    if (((DropDownList)gdvULDDetails.Rows[i].FindControl("ddlLoadingPriority")).Text == "&nbsp;")
                    {
                        LoadingPriority = "";
                    }
                    else
                    {
                        LoadingPriority = ((DropDownList)gdvULDDetails.Rows[i].FindControl("ddlLoadingPriority")).SelectedItem.Text.ToString();
                    }

                    if (((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text == "&nbsp;")
                    {
                        Remark = "";
                    }
                    else
                    {
                        Remark = ((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text.ToString();
                    }

                    string Location = ((TextBox)gdvULDDetails.Rows[i].FindControl("txtLocation")).Text.ToString();

                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblBonded")).Text == "&nbsp;")
                    {
                        IsBonded = "";
                    }
                    else
                    {
                        IsBonded = ((Label)gdvULDDetails.Rows[i].FindControl("lblBonded")).Text.Trim();
                    }

                    string CurrLoc = ((Label)gdvULDDetails.Rows[i].FindControl("lblPOL")).Text; ;// Session["Station"].ToString();

                    Updatedon = Convert.ToDateTime(Session["IT"]).ToString(); //DateTime.Now.ToString("yyyy/MM/dd");
                    strMode = Convert.ToString(Session["Split"]);
                    bool IsManifest = false;
                    IsManifest = bool.Parse(((Label)gdvULDDetails.Rows[i].FindControl("lblManifested")).Text);
                    DateTime FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);
                    AWBOrigin = ((Label)gdvULDDetails.Rows[i].FindControl("lblOrigin")).Text.Trim();

                    //if (txtTailNo.Text.Trim() == "")
                    //{
                    //    lblStatus.Text = "Please enter Tail Number.";
                    //    lblStatus.ForeColor = Color.Red;
                    //    txtTailNo.Focus();
                    //    return;
                    //}

                    //Shubhankar
                    string tailNo = txtTailNo.Text.Trim();                    

                    string FltFlag = ((Label)gdvULDDetails.Rows[i].FindControl("lblFltFlag")).Text.Trim();
                    string CartNumber = ((Label)gdvULDDetails.Rows[i].FindControl("lblCartNumber")).Text.Trim();

                    if (strMode != "O")
                    {
                        //If condition added for save POL AWB's match with this location  
                        if (Session["Station"].ToString() == POL)
                        {
                            string strResult = string.Empty;
                            string strIdentifier = Convert.ToString(Session["Identifier"]);
                            //appended 25apr -add awb to manifest completed so they dont appear in awb again
                            bool result = objExpMani.AddULDAWBassocition(FLTno, POL, POU, ULDno, ULDwgt, AWBno, PCS, WGT, AVLPCS, AVLWGT, Updatedby, dtCurrentDate, FltDate, strIdentifier, ref strResult, AWBPrefix, FltFlag, CartNumber);                    //

                            if (strResult.Length > 0)
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = strResult;
                                return;
                            }
                            res = objExpMani.SaveManifestdata(FLTno, ULDno, POU, POL, ULDDest, counter, AWBno, SCC, VOL, PCS, WGT, AVLPCS, AVLWGT,
                                Desc, LoadingPriority, Remark, Updatedby, Updatedon, IsManifest, FltDate, CurrLoc, AWBOrigin,
                                (int.Parse(ddlIrregularityCode.SelectedValue)), IsBonded, tailNo, IMENo, AWBPrefix, Location, CartNumber);
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
            #region Nil Flight Depart Check
            try
            {
                //LoginBL objConfig = new LoginBL();
                //if (Convert.ToBoolean(objConfig.GetMasterConfiguration("ManifestEmptyFlight")))
                if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ManifestEmptyFlight")))
                {
                    if (gdvULDDetails.Rows.Count <= 0)
                    {
                        string POU = "";
                        //Find out last POU of the flight.
                        if (ddlMainPOU.Items.Count > 1)
	                    {
                            POU = ddlMainPOU.Items[ddlMainPOU.Items.Count - 2].Text;
	                    }
                        SaveNilManifest(txtFlightCode.Text.Trim() + txtFlightID.Text.Trim(), "BULK",
                            Session["Station"].ToString(), POU, Session["UserName"].ToString(),
                            DateTime.ParseExact(TextBoxdate.Text,"dd/MM/yyyy",null),
                            Convert.ToDateTime(Session["IT"]));

                    }
                    strMode = "O";
                }
                //objConfig = null;
            }
            catch (Exception ex)
            {

            }
            #endregion

            if (res == true || strMode == "O")
            {
                BtnList_Click(null, null);
                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = "Manifest Version Saved Successfully";
                btnFinalize.Enabled = true;
                btnDepartFit.Enabled = false;
                Session["Split"] = "";
                return;
            }

        }
        #endregion Save Manifest Grid data To DB

        #region Button list Details Click
        protected void BtnListDetails_Click1(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("Exp_Manifest_btnListDetails1_ds");
            //DataSet dsawb3 = null;
            lblStatus.Text = "";
            try
            {
                Session["AWBdata"] = null;
                gdvULDLoadPlanAWB.DataSource = null;
                gdvULDLoadPlanAWB.DataBind();
                try
                {
                    ChangeButtonStatus();
                }
                catch (Exception)
                {

                }

                string FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                string ManifestDate = "", ManifestdateFrom = "", ManifestdateTo = "";

                ManifestDate = TextBoxdate.Text.ToString();
                if (ManifestDate.Length > 10)
                {
                    ManifestdateFrom = ManifestDate.Substring(0, 10) + " 00:00:00";
                    ManifestdateTo = ManifestDate.Substring(0, 10) + " 23:59:59";
                }
                else
                {
                    ManifestdateFrom = ManifestDate;// +" 00:00:00";
                    ManifestdateTo = ManifestDate + " 23:59:59";
                }

                string version = ddlVersion.SelectedItem.Value.ToString();
                string UldNo = ddlULD.SelectedItem.Value.ToString();
                string ValPOL = ddlPOLDetails.SelectedItem.Value.ToString();
                string ValPOU = ddlPOU.SelectedItem.Value.ToString();
                string DepartureAirport = lblDepAirport.Text;

                if (ValPOU == "ALL")
                    ValPOU = "";

                ds = objExpMani.GetManifestDetailsRevised(FLTno, ManifestdateFrom, ManifestdateTo, DepartureAirport, version, UldNo, ValPOL, ValPOU);//return manifest data
                DateTime FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);
                //dsawb3 = objExpMani.GetAwbTabdetails_GHA(lblDepAirport.Text, FLTno, FlightDate, "", "", "");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string ULDno = "", AWBno = "", POU = "", POL = "", AWBDest = "", CommCode = "";
                    int PCS = 0, AVLPCS = 0, AWBPcs = 0;
                    double WGT = 0.0, AVLWGT = 0.0, VolWt = 0.0, AWBGwt = 0;
                    string Desc = "";

                    btnOffload.Enabled = false;
                    btnUnassign.Enabled = false;
                    btnSplitUnassign.Enabled = false;
                    btnSplitUnassign.Enabled = false;
                    BtnAddtoManifest.Enabled = false;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ULDno = ds.Tables[0].Rows[i].ItemArray.GetValue(0).ToString();

                        AWBno = ds.Tables[0].Rows[i]["AWBno"].ToString();
                        PCS = Convert.ToInt32(ds.Tables[0].Rows[i]["PCS"]);
                        WGT = Convert.ToDouble(ds.Tables[0].Rows[i]["GrossWgt"]);
                        AVLPCS = Convert.ToInt32(ds.Tables[0].Rows[i]["StatedPCS"]);
                        AVLWGT = Convert.ToDouble(ds.Tables[0].Rows[i]["StatedWgt"]);

                        AWBPcs = Convert.ToInt32(ds.Tables[0].Rows[i]["BookedPCS"]);
                        AWBGwt = Convert.ToDouble(ds.Tables[0].Rows[i]["BookedWgt"]);

                        // Updatedon = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                        VolWt = Convert.ToDouble(ds.Tables[0].Rows[i]["Vol"]);
                        Desc = ds.Tables[0].Rows[i]["Desc"].ToString();
                        bool IsManifest = bool.Parse(ds.Tables[0].Rows[i]["Manifested"].ToString());
                        AWBDest = ds.Tables[0].Rows[i]["AWBDest"].ToString();
                        CommCode = ds.Tables[0].Rows[i]["CommodityCode"].ToString();
                        string CartNumber = ds.Tables[0].Rows[i]["CartNumber"].ToString();
                        
                        tabMenifestDetails = (DataTable)Session["ManifestGridData"];

                        if (tabMenifestDetails == null)
                            tabMenifestDetails = new DataTable("Exp_Manifest_BtnListDetails_tabMenifestDetails");

                        DataRow l_Datarow = tabMenifestDetails.NewRow();

                        l_Datarow["ULDno"] = ULDno;
                        l_Datarow["POU"] = POU;
                        l_Datarow["POL"] = POL;
                        l_Datarow["AWBDest"] = AWBDest;
                        l_Datarow["Counter"] = "";
                        l_Datarow["AWBno"] = AWBno;
                        l_Datarow["SCC"] = CommCode;
                        l_Datarow["PCS"] = PCS;
                        l_Datarow["GrossWgt"] = WGT;
                        l_Datarow["VOL"] = VolWt;
                        l_Datarow["StatedPCS"] = AVLPCS;
                        l_Datarow["StatedWgt"] = AVLWGT;

                        l_Datarow["BookedPCS"] = AWBPcs;
                        l_Datarow["BookedWgt"] = AWBGwt;

                        l_Datarow["Desc"] = Desc;
                        l_Datarow["Orign"] = POL;
                        l_Datarow["Dest"] = AWBDest;
                        l_Datarow["Manifested"] = IsManifest;
                        l_Datarow["LoadingPriority"] = "";
                        l_Datarow["Remark"] = "";
                        l_Datarow["Bonded"] = "";
                        l_Datarow["CartNumber"] = CartNumber;


                        tabMenifestDetails.Rows.Add(l_Datarow);

                        if (tabMenifestDetails.Rows[i][0].ToString().ToUpper() == "BULK")
                        {
                            totalAWB = 1 + totalAWB;
                            totalAWBPCS = totalAWBPCS + int.Parse(tabMenifestDetails.Rows[i]["PCS"].ToString());
                            totalAWBWt = totalAWBWt + int.Parse(tabMenifestDetails.Rows[i]["GrossWgt"].ToString());
                            totalAWBVol = totalAWBVol + float.Parse(tabMenifestDetails.Rows[i]["Vol"].ToString());
                        }
                        else
                        {
                            TotalULD = 1 + TotalULD;
                            totalULDAWB = 1 + totalULDAWB;
                            totalULDPCS = totalULDPCS + int.Parse(tabMenifestDetails.Rows[i]["PCS"].ToString());
                            totalULDWt = totalULDWt + int.Parse(tabMenifestDetails.Rows[i]["GrossWgt"].ToString());
                            totalULDVol = totalULDVol + float.Parse(tabMenifestDetails.Rows[i]["Vol"].ToString());
                        }

                        gdvULDDetails.DataSource = ds;
                        gdvULDDetails.DataBind();
                        try
                        {
                            ChangeButtonStatus();
                        }
                        catch (Exception)
                        {

                        }

                        Session["GDVULDDetails"] = ds.Tables[0];
                    }
                    //ShowULDAWBSummary();
                }
                else
                {
                    gdvULDDetails.DataSource = ds;
                    gdvULDDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                //if (dsawb3 != null)
                //    dsawb3.Dispose();
            }
        }
        #endregion

        #region On Print Manifest button Click
        protected void btnPrintMFT_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                if (txtTailNo.Text.Trim() == "")
                {
                    txtTailNo.Focus();
                    lblStatus.Text = "Please enter Tail No";
                    lblStatus.ForeColor = Color.Red;
                    txtTailNo.Focus();
                    return;
                }
                int intAWBTypes = 0;
                string strAWBType = string.Empty;
                int RowCount = 0;
                //lblStatus.Text = "";

                if (Session["AT"] != null)
                {
                    intAWBTypes = ((DataTable)Session["AT"]).Rows.Count;
                }

                for (int intCount = 0; intCount < gdvULDDetails.Rows.Count; intCount++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[intCount].FindControl("Check0")).Checked == true)
                    {
                        RowCount = RowCount + 1;
                    }
                }
                if (gdvULDDetails.Rows.Count > 0)
                {
                    if (RowCount == 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please select atleast one shipment to Print Manifest.";
                        return;
                    }
                }

                string hidPrintMFTURLs = "";
                if (intAWBTypes > 0)
                {
                    for (int m = 0; m < intAWBTypes; m++)
                    {
                        strAWBType = ((DataTable)Session["AT"]).Rows[m][0].ToString();
                        hidPrintMFTURLs = hidPrintMFTURLs + showManifestData(strAWBType, m) + "|";
                        //System.Threading.Thread.Sleep(2000);
                    }
                    HidPrintMFT.Value = hidPrintMFTURLs.Substring(0, hidPrintMFTURLs.Length - 1);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>LoopPrintMFT();</script>", false);
                    //showManifestData(strAWBType, 0);

                }
                else
                    showManifestData("", 0);
            }
            catch (Exception ex)
            {

            }
            return;
        }
        #endregion

        #region Function to Show manifest Data
        private string showManifestData(string strAWBType, int Iteration)
        {
            try
            {
                int chkNilMan = 0;
                ArrayList arPOU = new ArrayList();
                string strLoadingPt = Session["Station"].ToString();
                string strUnloadPt = "";
                for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                {
                    string strOrg1 = ((Label)gdvULDDetails.Rows[j].FindControl("lblPOL")).Text.Trim();

                    if (!arPOU.Contains(((Label)gdvULDDetails.Rows[j].FindControl("lblPOU")).Text))
                    {
                        arPOU.Add(((Label)gdvULDDetails.Rows[j].FindControl("lblPOU")).Text);
                    }

                    if (strOrg1 != strLoadingPt)
                    {
                        chkNilMan = chkNilMan + 1;

                    }

                }

                Session["DTExport"] = null;
                Session["DTAWBDetails"] = null;
                Session["DTTransitExport"] = null;

                //img for report

                System.IO.MemoryStream Logo = null;
                try
                {
                    Logo = CommonUtility.GetImageStream(Page.Server, txtFlightCode.Text.Trim());
                }
                catch (Exception ex)
                {
                    Logo = new System.IO.MemoryStream();
                }

                //end

                DataTable DTAWBDetails = new DataTable("Exp_Manifest_showManifestData_DTAWBDetails");

                DTAWBDetails.Columns.Add("AWBNo");
                DTAWBDetails.Columns.Add("NoOfPcs");
                DTAWBDetails.Columns.Add("ActualPCS");

                DTAWBDetails.Columns.Add("Description");
                DTAWBDetails.Columns.Add("Weight");
                DTAWBDetails.Columns.Add("Org");
                DTAWBDetails.Columns.Add("Dest");
                DTAWBDetails.Columns.Add("NextFlight");
                DTAWBDetails.Columns.Add("SCC");
                DTAWBDetails.Columns.Add("Remark");
                DTAWBDetails.Columns.Add("TotalPCS");
                DTAWBDetails.Columns.Add("TotalWeight");
                DTAWBDetails.Columns.Add("AccptedWt");
                DTAWBDetails.Columns.Add("ULD");
                DTAWBDetails.Columns.Add("Bonded");


                if (gdvULDDetails.Rows.Count < 1)
                {
                    DataRow drAWBDetails = DTAWBDetails.NewRow();

                    drAWBDetails["AWBNo"] = "";
                    drAWBDetails["NoOfPcs"] = 0;
                    drAWBDetails["ActualPCS"] = 0;
                    drAWBDetails["Description"] = "";
                    drAWBDetails["Weight"] = 0;
                    drAWBDetails["Org"] = "";
                    drAWBDetails["Dest"] = "";
                    drAWBDetails["NextFlight"] = "";
                    drAWBDetails["SCC"] = "";
                    drAWBDetails["Remark"] = "";
                    drAWBDetails["TotalPCS"] = 0;
                    drAWBDetails["TotalWeight"] = 0;
                    drAWBDetails["AccptedWt"] = 0;
                    drAWBDetails["ULD"] = "";
                    drAWBDetails["Bonded"] = "";

                    DTAWBDetails.Rows.Add(drAWBDetails);
                }

                string strOwner = "";
                //Get airline details from below dataset
                try
                {

                    if (Session["FtlPOU"] != null)
                        strUnloadPt = (string)Session["FtlPOU"];

                    MasterBAL objMst = new MasterBAL();
                    DataSet dsAirlineDetails = new DataSet("Exp_Manifest_showManifestData_dsAirlineDetails");
                    dsAirlineDetails = objMst.GetAirlineDetails(strLoadingPt, strUnloadPt);
                    objMst = null;
                    if (dsAirlineDetails.Tables.Count > 0)
                    {
                        if (dsAirlineDetails.Tables[0].Rows.Count > 0)
                        {
                            for (int p = 0; p < dsAirlineDetails.Tables[0].Rows.Count; p++)
                            {
                                strOwner = dsAirlineDetails.Tables[0].Rows[0][0].ToString();
                                break;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                }

                string strNationality = "";
                string strFlightNo = txtFlightCode.Text + txtFlightID.Text + "   " + TextBoxdate.Text;

                //ddlMainPOU.Items[m].Text;// ddlMainPOU.Text;//need to change
                float strTotalWeight = 0;
                string strPreparedBy = Session["Username"].ToString();
                int strTotal = 0;
                string UnloadAt = "";

                //Shubhankar
                string TailNo = txtTailNo.Text.Trim();

                string EGMNo = txtEMG.Text.Trim();

                DateTime dt = new DateTime();

                try
                {
                    //DateTime dt1 = new DateTime();
                    dt = DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null);


                    DataSet DsNationality = new DataSet("Exp_Manifest_showManifestData_DsNationality");
                    DsNationality = objExpMani.GetPOUAirlineSchedule(txtFlightCode.Text + txtFlightID.Text, strLoadingPt, dt);


                    //strNationality = DsNationality.Tables[1].Rows[0][0].ToString();

                    //Shubhankar
                    //if (ddlTailNo.SelectedIndex > 0)
                    strNationality = txtTailNo.Text.Trim();// ddlTailNo.SelectedItem.Text;
                }
                catch (Exception ex)
                {

                }

                DataTable DTExport = new DataTable("Exp_Manifest_showManifestData_DTExport");

                DTExport.Columns.Add("Owner");
                DTExport.Columns.Add("Nationality");
                DTExport.Columns.Add("FlightNo");
                DTExport.Columns.Add("LoadingPt");
                DTExport.Columns.Add("UnloadPt");
                DTExport.Columns.Add("TotalWeight");
                DTExport.Columns.Add("PreparedBy");
                DTExport.Columns.Add("Total");
                DTExport.Columns.Add("UnloadAt");
                DTExport.Columns.Add("TailNo");
                DTExport.Columns.Add("EGMNo");

                DTExport.Columns.Add("ClientLogo", System.Type.GetType("System.Byte[]"));
                DTExport.Columns.Add("AWBType");

                DataTable DTTransitExport = new DataTable("Exp_Manifest_showManifestData_DTTransitExport");

                DTTransitExport.Columns.Add("AWBNo");
                DTTransitExport.Columns.Add("NoOfPcs");
                DTTransitExport.Columns.Add("ActualPCS");
                DTTransitExport.Columns.Add("Description");
                DTTransitExport.Columns.Add("Weight");
                DTTransitExport.Columns.Add("Org");
                DTTransitExport.Columns.Add("Dest");
                DTTransitExport.Columns.Add("NextFlight");
                DTTransitExport.Columns.Add("SCC");
                DTTransitExport.Columns.Add("Remark");
                DTTransitExport.Columns.Add("ToatalPcs");
                DTTransitExport.Columns.Add("TotalWeight");
                DTTransitExport.Columns.Add("AccptedWt");
                DTTransitExport.Columns.Add("ULD");
                DTTransitExport.Columns.Add("Bonded");

                // if (gdvULDDetails.Rows.Count < 1)
                if (gdvULDDetails.Rows.Count < 1 || chkNilMan > 0)
                {
                    SaveNilManifest(txtFlightCode.Text + txtFlightID.Text, "BULK", strLoadingPt, strUnloadPt, strPreparedBy, dt, dtCurrentDate);
                    if (gdvULDDetails.Rows.Count < 1)
                    {
                        DataRow DtRow = DTExport.NewRow();
                        DtRow["Owner"] = strOwner;

                        //Shubhankar
                        //if (ddlTailNo.SelectedIndex == 0)
                        //    DtRow["Nationality"] = "";//strNationality;
                        //else
                        //    DtRow["Nationality"] = ddlTailNo.SelectedItem.Text;
                        DtRow["Nationality"] = txtTailNo.Text.Trim();

                        DtRow["FlightNo"] = strFlightNo;
                        DtRow["LoadingPt"] = strLoadingPt;
                        DtRow["UnloadPt"] = strUnloadPt;
                        DtRow["TotalWeight"] = "Nil";
                        DtRow["PreparedBy"] = strPreparedBy;
                        DtRow["Total"] = "Nil";
                        DtRow["UnloadAt"] = strUnloadPt;

                        DtRow["TailNo"] = txtTailNo.Text.Trim();
                        DtRow["EGMNo"] = txtEMG.Text.Trim();
                        DtRow["ClientLogo"] = Logo.ToArray();


                        DTExport.Rows.Add(DtRow);

                        Session["DTAWBDetails" + 0] = DTAWBDetails;
                        Session["DTExport" + 0] = DTExport;
                        Session["DTTransitExport" + 0] = DTTransitExport;


                        string query = "'ShowEMAWBULD.aspx?ID=" + 0 + "'";
                        //Response.Write("<script>");
                        //Response.Write("window.open(" + query + ",'_blank')");
                        //Response.Write("</script>");

                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open(" + query + ");", true);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "newwindow.focus();", true);

                        return string.Empty;
                    }

                }

                string FlightNo = txtFlightCode.Text + txtFlightID.Text;

                DTAWBDetails.Rows.Clear();
                DTExport.Rows.Clear();
                DTTransitExport.Rows.Clear();

                for (int l = 0; l < gdvULDDetails.Rows.Count; l++)
                {
                    if (((DataTable)Session["MD"]).Rows[l]["DocumentType"].ToString() == strAWBType)
                    {
                        string pou = arPOU[0].ToString();
                        if (pou == ((Label)gdvULDDetails.Rows[l].FindControl("lblPOU")).Text.Trim())
                        {
                            //int Totalpcs = 0;
                            int localPCS = 0;
                            int transitPCS = 0;
                            float localWeight = 0;
                            float transitWeight = 0;
                            //float AcctepedWt = 0;
                            ArrayList ULDDestpt = new ArrayList();

                            for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                            {
                                if (((DataTable)Session["MD"]).Rows[i]["DocumentType"].ToString() == strAWBType && ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text.Trim() != "")
                                {
                                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
                                    {
                                        //if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
                                        //{
                                        string stPOU = Convert.ToString(Session["Dest"]);// arPOU[m].ToString();
                                        //if (stPOU == gdvULDDetails.Rows[i].Cells[2].Text)
                                        //{
                                        string strOrg = ((Label)gdvULDDetails.Rows[i].FindControl("lblOrigin")).Text.Trim();

                                        string strNextflight = "Local";
                                        if (strOrg != strLoadingPt)
                                        {
                                            strNextflight = "Transit";
                                        }

                                        //if (!ULDDestpt.Contains(gdvULDDetails.Rows[i].Cells[2].Text))
                                        //{
                                        string strDest = "";
                                        //if (!ULDDestpt.Contains(gdvULDDetails.Rows[i].Cells[2].Text))
                                        // {
                                        strDest = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBDest")).Text.Trim();
                                        // }

                                        if (!ULDDestpt.Contains(((Label)gdvULDDetails.Rows[i].FindControl("lblPOU")).Text.Trim()))
                                        {
                                            //UnloadAt = gdvULDDetails.Rows[i].Cells[2].Text + "," + UnloadAt;
                                            ULDDestpt.Add(((Label)gdvULDDetails.Rows[i].FindControl("lblPOU")).Text);
                                            UnloadAt = (string)Session["FtlPOU"];
                                            TailNo = txtTailNo.Text.Trim();
                                            EGMNo = txtEMG.Text.Trim();
                                        }
                                        string ULDNo = "BULK";
                                        ULDNo = ((Label)gdvULDDetails.Rows[i].FindControl("lblULDno")).Text.Trim();
                                        string strAWBNo = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text.Trim().Replace("&nbsp;", "");

                                        //string AWBNo = strAWBNo.Substring(3, 8);
                                        //string AWBPrefix = strAWBNo.Replace(AWBNo, "");
                                        //strAWBNo = AWBPrefix + "-" + AWBNo;

                                        // ULDDest = gdvULDDetails.Rows[i].Cells[4].Text;
                                        string strSCC = "";

                                        if (((Label)gdvULDDetails.Rows[i].FindControl("lblSCC")).Text == "&nbsp;")
                                        {
                                            strSCC = "0";
                                        }
                                        else
                                        {
                                            strSCC = ((Label)gdvULDDetails.Rows[i].FindControl("lblSCC")).Text;
                                        }


                                        int strNoOfPCS = int.Parse(((Label)gdvULDDetails.Rows[i].FindControl("lblMftPcs")).Text);
                                        int actPcs = int.Parse(((Label)gdvULDDetails.Rows[i].FindControl("lblStatedPCS")).Text);
                                        string strWeight = ((Label)gdvULDDetails.Rows[i].FindControl("lblMftWt")).Text;
                                        strTotalWeight = strTotalWeight + float.Parse(strWeight);
                                        strTotal = strTotal + strNoOfPCS;
                                        string AcceptPCS = ((Label)gdvULDDetails.Rows[i].FindControl("lblStatedPCS")).Text;
                                        string AcceptWGT = ((Label)gdvULDDetails.Rows[i].FindControl("lblStatedWgt")).Text;

                                        string BookedPcs = ((Label)gdvULDDetails.Rows[i].FindControl("lblBookedPCS")).Text;
                                        string BookedGwt = ((Label)gdvULDDetails.Rows[i].FindControl("lblBookedWgt")).Text;

                                        string strDescription = "";
                                        if (((Label)gdvULDDetails.Rows[i].FindControl("lblCommDesc")).Text == "&nbsp;")
                                        {
                                            strDescription = "";
                                        }
                                        else
                                        {
                                            strDescription = ((Label)gdvULDDetails.Rows[i].FindControl("lblCommDesc")).Text;
                                            strDescription = strDescription.Replace("&amp;", "&");
                                        }


                                        string strRemark = "";

                                        if (((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text == "&nbsp;")
                                        {
                                            strRemark = "";
                                        }
                                        else
                                        {
                                            strRemark = ((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text.ToString();
                                        }

                                        string IsBonded = ((Label)gdvULDDetails.Rows[i].FindControl("lblBonded")).Text.Trim().Replace("&nbsp;", "");

                                        if (strOrg != strLoadingPt)
                                        {
                                            //Verify if AWB is already present as Bulk in the Grid.
                                            DataRow[] drTransitRowArray = DTTransitExport.Select("AWBNo='" + strAWBNo + "' AND ULD='" + ULDNo + "'");
                                            if (drTransitRowArray != null && drTransitRowArray.Length > 0)
                                            {
                                                int index = DTTransitExport.Rows.IndexOf((DataRow)drTransitRowArray.GetValue(0));
                                                if (index >= 0)
                                                {
                                                    //DTTransitExport.Rows[index].BeginEdit();
                                                    DTTransitExport.Rows[index]["NoOfPcs"] = strNoOfPCS +
                                                        Convert.ToInt32(DTTransitExport.Rows[index]["NoOfPCS"].ToString());

                                                    DTTransitExport.Rows[index]["Weight"] =
                                                        Convert.ToString(
                                                        Convert.ToDecimal(DTTransitExport.Rows[index]["Weight"].ToString()) +
                                                        Convert.ToDecimal(strWeight));
                                                    //DTTransitExport.Rows[index].EndEdit();
                                                }
                                            }
                                            else
                                            {
                                                DTTransitExport.Rows.Add(strAWBNo, strNoOfPCS, BookedPcs, strDescription, strWeight, strOrg, strDest, strNextflight, strSCC, strRemark, transitPCS, transitWeight, BookedGwt, ULDNo, IsBonded);
                                            }
                                            transitPCS = transitPCS + strNoOfPCS;
                                            transitWeight = transitWeight + float.Parse(strWeight);
                                        }
                                        else
                                        {
                                            //Verify if AWB is already present as Bulk in the Grid.
                                            DataRow[] drTransitRowArray = DTAWBDetails.Select("AWBNo='" + strAWBNo + "' AND ULD='" + ULDNo + "'");
                                            if (drTransitRowArray != null && drTransitRowArray.Length > 0)
                                            {
                                                int index = DTAWBDetails.Rows.IndexOf((DataRow)drTransitRowArray.GetValue(0));
                                                if (index >= 0)
                                                {
                                                    //DTTransitExport.Rows[index].BeginEdit();
                                                    DTAWBDetails.Rows[index]["NoOfPcs"] = strNoOfPCS +
                                                        Convert.ToInt32(DTAWBDetails.Rows[index]["NoOfPcs"].ToString());

                                                    DTAWBDetails.Rows[index]["Weight"] =
                                                        Convert.ToString(
                                                        Convert.ToDecimal(DTAWBDetails.Rows[index]["Weight"].ToString()) +
                                                        Convert.ToDecimal(strWeight));
                                                    //DTTransitExport.Rows[index].EndEdit();
                                                }
                                            }
                                            else
                                            {
                                                DTAWBDetails.Rows.Add(strAWBNo, strNoOfPCS, BookedPcs, strDescription, strWeight, strOrg, strDest, strNextflight, strSCC, strRemark, localPCS, localWeight, BookedGwt, ULDNo, IsBonded);
                                            }
                                            localPCS = localPCS + strNoOfPCS;
                                            localWeight = localWeight + float.Parse(strWeight);
                                        }
                                        // }
                                    }
                                }
                                // }
                            }

                            ULDDestpt = null;
                            if (UnloadAt != "")
                            {
                                //UnloadAt = UnloadAt.Remove(UnloadAt.Length - 1, 1);
                                UnloadAt = (string)Session["FtlPOU"];
                                TailNo = txtTailNo.Text.Trim();
                                EGMNo = txtEMG.Text.Trim();

                            }


                            if (DTTransitExport.Rows.Count > 0)
                            {
                                for (int k = 0; k < DTTransitExport.Rows.Count; k++)
                                {
                                    DTTransitExport.Rows[k]["ToatalPcs"] = transitPCS;
                                    DTTransitExport.Rows[k]["TotalWeight"] = transitWeight;
                                }
                            }
                            else
                            {
                                DTTransitExport.Rows.Add("-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-");
                            }

                            for (int k = 0; k < DTAWBDetails.Rows.Count; k++)
                            {
                                DTAWBDetails.Rows[k]["TotalPCS"] = localPCS;
                                DTAWBDetails.Rows[k]["TotalWeight"] = localWeight;
                            }

                            DTExport.Rows.Add(strOwner, strNationality, strFlightNo, strLoadingPt, strUnloadPt, strTotalWeight, strPreparedBy, strTotal, arPOU[0].ToString(), TailNo, EGMNo, Logo.ToArray(), strAWBType);
                            Session["DTExport" + Iteration] = DTExport.Copy();
                            //  dsDTExport.Tables.Add(DTExport);

                            if (UnloadAt == "")
                            {
                                DTAWBDetails.Rows.Add("", "", "", "", "", "", "", "", "");
                                Session["DTAWBDetails" + Iteration] = DTAWBDetails.Copy();
                            }
                            break;

                        }
                        //break;
                    } // End of If
                }
                Session["DTAWBDetails" + Iteration] = DTAWBDetails.Copy();
                Session["DTExport" + Iteration] = DTExport.Copy();

                Session["DTTransitExport" + Iteration] = DTTransitExport.Copy();

                string query1 = "ShowEMAWBULD.aspx?ID=" + Iteration + "";

                ScriptManager.RegisterStartupScript(this, GetType(), "OpenWindow", "window.open(" + query1 + ");", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenWindow", "newwindow.focus();", true);

                return query1;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Function to Show NOTC
        private void showNOTOCData()
        {
            ArrayList arPOU = new ArrayList();
            System.IO.MemoryStream Logo = null;
            System.IO.MemoryStream Logo1 = null;
            DataTable DTNOTCDetails = new DataTable("Exp_Manifest_showNOTOCData_DTNOTCDetails");
            DataSet DsNationality = new DataSet("Exp_Manifest_showNOTOCData_DsNationality");
            DataTable DTNOTOCAWBs = new DataTable("Exp_Manifest_showNOTOCData_DTNOTOCAWBs");
            DataTable DTNOTOCOtherLoad = new DataTable("Exp_Manifest_showNOTOCData_DTNOTOCOtherLoad");
            DataSet ds = new DataSet("Exp_Manifest_showNOTOCData_ds");
            DataTable DTDGR = new DataTable("Exp_Manifest_showNOTOCData_DTDGR");
            DataTable DTSPC = new DataTable("Exp_Manifest_showNOTOCData_DTSPC");

            try
            {
                Session["DTNOTOC"] = null;
                Session["DTNOTCAWBs"] = null;
                //img for report

                try
                {
                    Logo = CommonUtility.GetImageStream(Page.Server);
                }
                catch (Exception ex)
                {
                    Logo = new System.IO.MemoryStream();
                }
               
                //end

                try
                {
                    Logo1 = CommonUtility.getPartnerImage(Page.Server);
                }
                catch (Exception ex)
                {
                    Logo1 = new System.IO.MemoryStream();
                }
                DTNOTCDetails.Columns.Add("NotocID");
                DTNOTCDetails.Columns.Add("LoadingStation");
                DTNOTCDetails.Columns.Add("FlightNumber");
                DTNOTCDetails.Columns.Add("Date");
                DTNOTCDetails.Columns.Add("Registration");
                DTNOTCDetails.Columns.Add("PreparedBy");
                DTNOTCDetails.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                DTNOTCDetails.Columns.Add("Logo1", System.Type.GetType("System.Byte[]"));
                DateTime dt = new DateTime();
                string strNationality = "";
                try
                {
                    //DateTime dt1 = new DateTime();
                    dt = DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null);
                    string strLoadingPt = Session["Station"].ToString();

                    DsNationality = objExpMani.GetPOUAirlineSchedule(txtFlightCode.Text + txtFlightID.Text, strLoadingPt, dt);
                    strNationality = txtTailNo.Text;//DsNationality.Tables[1].Rows[0][0].ToString();
                }
                catch (Exception ex)
                {

                }

                if (gdvULDDetails.Rows.Count >= 0)
                {
                    DataRow drNotocDetails = DTNOTCDetails.NewRow();

                    drNotocDetails["NotocID"] = "";
                    drNotocDetails["LoadingStation"] = Session["Station"].ToString();
                    drNotocDetails["FlightNumber"] = txtFlightCode.Text + txtFlightID.Text;
                    drNotocDetails["Date"] = lblDate.Text.TrimStart('|');
                    drNotocDetails["Registration"] = strNationality;
                    drNotocDetails["PreparedBy"] = Session["Username"].ToString();
                    drNotocDetails["Logo"] = Logo.ToArray();
                    drNotocDetails["Logo1"] = Logo1.ToArray();
                    DTNOTCDetails.Rows.Add(drNotocDetails);
                }

                string strFlightNo = txtFlightCode.Text + txtFlightID.Text + "        " + TextBoxdate.Text;
                string strPreparedBy = Session["Username"].ToString();

                DTNOTOCAWBs.Columns.Add("POU");
                DTNOTOCAWBs.Columns.Add("AWBNo");
                DTNOTOCAWBs.Columns.Add("ShipperName");
                DTNOTOCAWBs.Columns.Add("DivisionClass1");
                DTNOTOCAWBs.Columns.Add("UNID");
                DTNOTOCAWBs.Columns.Add("SubRisk");
                DTNOTOCAWBs.Columns.Add("NoOfPkgs");
                DTNOTOCAWBs.Columns.Add("Quantity");
                DTNOTOCAWBs.Columns.Add("RadioActiveCategory");
                DTNOTOCAWBs.Columns.Add("PkgGroup");
                DTNOTOCAWBs.Columns.Add("CodeReverse");
                DTNOTOCAWBs.Columns.Add("CAO");
                DTNOTOCAWBs.Columns.Add("LoadedComp");
                DTNOTOCAWBs.Columns.Add("isnotoc");
                DTNOTOCAWBs.Columns.Add("PG");
                DTNOTOCAWBs.Columns.Add("ULDNo");
                DTNOTOCAWBs.Columns.Add("Description");
                DTNOTOCAWBs.Columns.Add("SHCDescription");
                DTNOTOCAWBs.Columns.Add("IMPCOde");
                DTNOTOCAWBs.Columns.Add("ERGCode");
                


                DTNOTOCOtherLoad.Columns.Add("POU");
                DTNOTOCOtherLoad.Columns.Add("AirWayBillNumber");
                DTNOTOCOtherLoad.Columns.Add("Description");
                DTNOTOCOtherLoad.Columns.Add("NoOfPackages");
                DTNOTOCOtherLoad.Columns.Add("Quantity");
                DTNOTOCOtherLoad.Columns.Add("Information");
                DTNOTOCOtherLoad.Columns.Add("Code");
                DTNOTOCOtherLoad.Columns.Add("LoadingCompartment");
                DTNOTOCOtherLoad.Columns.Add("LaodedAsShown");
                DTNOTOCOtherLoad.Columns.Add("OtherInformation");
                DTNOTOCOtherLoad.Columns.Add("IMPCOde");
                DTNOTOCOtherLoad.Columns.Add("ERGCode");
                DTNOTOCOtherLoad.Columns.Add("RadioActiveCategory");



                if (gdvULDDetails.Rows.Count > 0)
                {
                    for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                    {
                        DataRow[] DtRow = new DataRow[0];
                        //DataRow DtRow = DTNOTOCAWBs.NewRow();
                        //DtRow["POU"] = ((Label)gdvULDDetails.Rows[i].FindControl("lblPOU")).Text;
                        //DtRow["AWBNo"] = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text;
                        string AWBNo = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text;
                        string DGRAWB = "";


                        string UNID = "", DivisionClass = "", Description = "", SHCDescription = "", ShippingName = "", Subrisk = "", NoOfPkgs = "", Quantity = "", RadioCategory = "", PkgGroup = "", CodeReverse = "", CAO = "-", LoadingComp = "-", IsNotoc = "", PG = "", ULDNo = "";
                        //For DGR Cargo
                        try
                        {
                            ds = objExpMani.GetDGRCargoDetails(AWBNo.Substring(AWBNo.Length - 8));
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                                {

                                    Array.Resize(ref DtRow, DtRow.Length + 1);

                                    DtRow[DtRow.Length - 1] = DTNOTOCAWBs.NewRow();

                                    if (ds.Tables[0].Rows[ii]["AWBNumber"].ToString() == AWBNo.Substring(AWBNo.Length - 8))
                                    {
                                        DtRow[DtRow.Length - 1]["POU"] = ((Label)gdvULDDetails.Rows[i].FindControl("lblPOU")).Text;
                                        DtRow[DtRow.Length - 1]["AWBNo"] = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text;
                                        DtRow[DtRow.Length - 1]["ShipperName"] = ds.Tables[0].Rows[ii]["ShippingName"].ToString();
                                        DtRow[DtRow.Length - 1]["DivisionClass1"] = ds.Tables[0].Rows[ii]["ClassDiv"].ToString();
                                        DtRow[DtRow.Length - 1]["UNID"] = ds.Tables[0].Rows[ii]["UNID"].ToString();
                                        DtRow[DtRow.Length - 1]["SubRisk"] = ds.Tables[0].Rows[ii]["SubRisk"].ToString();
                                        DtRow[DtRow.Length - 1]["NoOfPkgs"] = ds.Tables[0].Rows[ii]["Pieces"].ToString();
                                        DtRow[DtRow.Length - 1]["Quantity"] = ds.Tables[0].Rows[ii]["Weight"].ToString();
                                        DtRow[DtRow.Length - 1]["RadioActiveCategory"] = ds.Tables[0].Rows[ii]["RadioCategory"].ToString();
                                        DtRow[DtRow.Length - 1]["PkgGroup"] = ds.Tables[0].Rows[ii]["PG"].ToString();
                                        DtRow[DtRow.Length - 1]["CodeReverse"] = ds.Tables[0].Rows[ii]["IMPCode"].ToString();
                                        DtRow[DtRow.Length - 1]["CAO"] = ds.Tables[0].Rows[ii]["CAO"].ToString();
                                        DtRow[DtRow.Length - 1]["LoadedComp"] = LoadingComp;
                                        DtRow[DtRow.Length - 1]["isnotoc"] = ds.Tables[0].Rows[ii]["isnotoc"].ToString();
                                        DtRow[DtRow.Length - 1]["PG"] = ds.Tables[0].Rows[ii]["PG"].ToString();
                                        if (((Label)gdvULDDetails.Rows[i].FindControl("lblULDno")).Text!="")
                                        {
                                            DtRow[DtRow.Length - 1]["ULDNo"] = ((Label)gdvULDDetails.Rows[i].FindControl("lblULDno")).Text;
                                        }
                                        else

                                            DtRow[DtRow.Length - 1]["ULDNo"] = ds.Tables[0].Rows[ii]["ULDNo"].ToString();
                                        
                                        DtRow[DtRow.Length - 1]["Description"] = ds.Tables[0].Rows[ii]["Description"].ToString();
                                        DtRow[DtRow.Length - 1]["SHCDescription"] = ds.Tables[0].Rows[ii]["SHCDescription"].ToString();
                                        DtRow[DtRow.Length - 1]["IMPCOde"] = ds.Tables[0].Rows[ii]["IMPCOde"].ToString();
                                        DtRow[DtRow.Length - 1]["ERGCode"] = ds.Tables[0].Rows[ii]["ERGCode"].ToString();
                                       // DtRow[DtRow.Length - 1]["RadioActiveCategory"] = ds.Tables[0].Rows[ii]["RadioActiveCategory"].ToString();
                                        DTNOTOCAWBs.Rows.Add(DtRow[DtRow.Length - 1]);
                                    }

                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        
                        //Other Load
                        string OtherAWBNo = "";
                        string CommDesc = "";
                        try
                        {

                            ds = objExpMani.GetDGRCargoDetails(AWBNo.Substring(AWBNo.Length - 8));
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                OtherAWBNo = ds.Tables[1].Rows[0][0].ToString();
                                CommDesc = ds.Tables[1].Rows[0][2].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        DataRow DtRowOther = DTNOTOCOtherLoad.NewRow();
                        if (OtherAWBNo == AWBNo.Substring(AWBNo.Length - 8))
                        {
                            DtRowOther["POU"] = ((Label)gdvULDDetails.Rows[i].FindControl("lblPOU")).Text;
                            DtRowOther["AirWayBillNumber"] = OtherAWBNo;
                            DtRowOther["Description"] = CommDesc;
                            DtRowOther["NoOfPackages"] = ((Label)gdvULDDetails.Rows[i].FindControl("lblMftPcs")).Text;
                            DtRowOther["Quantity"] = ((Label)gdvULDDetails.Rows[i].FindControl("lblMftWt")).Text;
                            DtRowOther["Information"] = "";
                            DtRowOther["Code"] = "";
                            DtRowOther["LoadingCompartment"] = "";
                            DtRowOther["LaodedAsShown"] = "";
                            DtRowOther["OtherInformation"] = "";

                            DTNOTOCOtherLoad.Rows.Add(DtRowOther);
                        }
                    }
                }
                try
                {
                    DTDGR = DTNOTOCAWBs.Select("isnotoc='DGR'").CopyToDataTable();//DTNOTOCAWBs[0].Select("DbCrAccountType='DEBIT' and AWBNumber='" + AWBNumber + "'").CopyToDataTable();

                }
                catch (Exception ex)
                {
                    DTDGR = DTNOTOCAWBs.Clone();
                    DataRow DtDGRRow = DTDGR.NewRow();
                    DtDGRRow["AWBNo"] = string.Empty;
                    DtDGRRow["POU"] = string.Empty;
                    DtDGRRow["ShipperName"] = string.Empty;
                    DtDGRRow["DivisionClass1"] = string.Empty;
                    DtDGRRow["UNID"] = string.Empty;
                    DtDGRRow["SubRisk"] = string.Empty;
                    DtDGRRow["NoOfPkgs"] = string.Empty;
                    DtDGRRow["Quantity"] = string.Empty;
                    DtDGRRow["RadioActiveCategory"] = string.Empty;
                    DtDGRRow["PkgGroup"] = string.Empty;
                    DtDGRRow["CodeReverse"] = string.Empty;
                    DtDGRRow["CAO"] = string.Empty;
                    DtDGRRow["LoadedComp"] = string.Empty;
                    DtDGRRow["isnotoc"] = string.Empty;
                    DtDGRRow["PG"] = string.Empty;
                    DtDGRRow["ULDNo"] = string.Empty;
                    DtDGRRow["Description"] = string.Empty;
                    DtDGRRow["SHCDescription"] = string.Empty;
                    DTDGR.Rows.Add(DtDGRRow);
                    DtDGRRow = null;
                }
                try
                {
                    DTSPC = DTNOTOCAWBs.Select("isnotoc='Special Cargo'").CopyToDataTable();
                }
                catch (Exception ex)
                {
                    DTSPC = DTNOTOCAWBs.Clone();

                    DataRow DtDGRRow = DTSPC.NewRow();

                    DtDGRRow["AWBNo"] = string.Empty;
                    DtDGRRow["POU"] = string.Empty;
                    DtDGRRow["ShipperName"] = string.Empty;
                    DtDGRRow["DivisionClass1"] = string.Empty;
                    DtDGRRow["UNID"] = string.Empty;
                    DtDGRRow["SubRisk"] = string.Empty;
                    DtDGRRow["NoOfPkgs"] = string.Empty;
                    DtDGRRow["Quantity"] = string.Empty;
                    DtDGRRow["RadioActiveCategory"] = string.Empty;
                    DtDGRRow["PkgGroup"] = string.Empty;
                    DtDGRRow["CodeReverse"] = string.Empty;
                    DtDGRRow["CAO"] = string.Empty;
                    DtDGRRow["LoadedComp"] = string.Empty;
                    DtDGRRow["isnotoc"] = string.Empty;
                    DtDGRRow["PG"] = string.Empty;
                    DtDGRRow["ULDNo"] = string.Empty;
                    DtDGRRow["Description"] = string.Empty;
                    DtDGRRow["SHCDescription"] = string.Empty;
                    DTSPC.Rows.Add(DtDGRRow);
                    DtDGRRow = null;
                }
                Session["DTNOTOC"] = DTNOTCDetails;
                try
                {
                    Session["DTNOTCAWBs"] = DTDGR;
                }
                catch (Exception ex)
                {
                    Session["DTNOTCAWBs"] = DTNOTOCAWBs;
                }
                Session["DTNOtherCode"] = DTNOTOCOtherLoad;
                try
                {
                    Session["DTNOTOCSPC"] = DTSPC;
                }
                catch (Exception ex)
                { }

                string query = "'showNotocDoc.aspx'";

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open(" + query + ");", true);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "newwindow.focus();", true);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                arPOU = null;
                Logo = null;
                Logo1 = null;

                if (DTNOTCDetails != null)
                    DTNOTCDetails.Dispose();
                if (DsNationality != null)
                    DsNationality.Dispose();
                if (DTNOTOCAWBs != null)
                    DTNOTOCAWBs.Dispose();
                if (DTNOTOCOtherLoad != null)
                    DTNOTOCOtherLoad.Dispose();
                if (ds != null)
                    ds.Dispose();
            }
        }
        #endregion

        protected void btnSplitUnassign_Click(object sender, EventArgs e)
        {
            if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                lblDepAirport.Text) == "D")
            {
                BtnList_Click(this, new EventArgs());
                lblStatus.Text = "Flight is departed. Please reopen flight.";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            DataTable dt = new DataTable("Exp_Manifest_btnSplitUnsassign_dt");
            DataTable DTAWBDetails = new DataTable("Exp_Manifest_btnSplitUnassign_DTAWBDetails");
            DataTable dt1 = new DataTable("Exp_Manifest_btnSplitUnassign_dt1");
            DataTable dtCurrentTable = new DataTable("Exp_Manifest_btnSplitUnassign_dtCurrentTable");
            lblStatus.Text = "";
            try
            {
                ddlReason.Visible = false;
                lblStatus.Text = "";
                int check = 0;
                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
                    {
                        check = check + 1;

                        if (((Label)gdvULDDetails.Rows[i].FindControl("lblFltFlag")).Text.Trim() == "" && ((Label)gdvULDDetails.Rows[i].FindControl("lblULDno")).Text.Trim().ToUpper() == "BULK")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Splitting can not happen on AWB - " + ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text.Trim();
                            return;
                        }
                    }
                }

                for (int k = 0; k < gdvULDDetails.Rows.Count; k++)
                {
                    string txt = ((Label)gdvULDDetails.Rows[k].FindControl("lblManifested")).Text;
                    if (((CheckBox)gdvULDDetails.Rows[k].FindControl("Check0")).Checked == true)
                    {
                        if (bool.Parse(((Label)gdvULDDetails.Rows[k].FindControl("lblManifested")).Text))
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "This AWB is Manifested so we can't Split and UnAssign this AWB";
                            return;
                        }
                        if (((Label)gdvULDDetails.Rows[k].FindControl("lblULDno")).Text.Trim().ToUpper() != "BULK")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "ULD can not be Splitted and Un-Assigned";
                            return;
                        }
                    }
                }

                if (check == 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select One AWB from Manifest TAB for Split and UnAssign";
                    return;
                }
                if (check > 1)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select only AWB ULD from Manifest TAB for Split and UnAssign";
                    return;
                }

                //Added by Poorna
                Session["Split"] = "U";
                btnAddManifest.Text = "Split Pieces";
                lblNextFlight.Visible = false;
                txtNextFlight.Visible = false;

                lblNFltDate.Visible = false;
                txtNFltDate.Visible = false;

                lblReason.Visible = false;
                txtReason.Visible = false;


                grdAWBs.Columns[2].Visible = true;

                grdAWBs.Columns[3].Visible = true;
                grdAWBs.Columns[5].Visible = false;

                grdAWBs.Columns[6].Visible = false;
                UpdatePanelRouteDetails.Visible = false;

                DTAWBDetails.Columns.Add("AWBNo");
                DTAWBDetails.Columns.Add("Pieces");
                DTAWBDetails.Columns.Add("Weight");
                DTAWBDetails.Columns.Add("AvlPCS");
                DTAWBDetails.Columns.Add("AvlWgt");
                DTAWBDetails.Columns.Add("ULDNo");
                DTAWBDetails.Columns.Add("CartNumber");


                for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[j].FindControl("Check0")).Checked == true)
                    {
                        DataRow dr;

                        dr = DTAWBDetails.NewRow();
                        dr[0] = ((Label)gdvULDDetails.Rows[j].FindControl("lblAWBno")).Text;
                        dr[1] = ((Label)gdvULDDetails.Rows[j].FindControl("lblMftPcs")).Text;
                        dr[2] = ((Label)gdvULDDetails.Rows[j].FindControl("lblMftWt")).Text;
                        dr[3] = ((Label)gdvULDDetails.Rows[j].FindControl("lblMftPcs")).Text;
                        dr[4] = ((Label)gdvULDDetails.Rows[j].FindControl("lblMftWt")).Text;
                        dr[5] = ((Label)gdvULDDetails.Rows[j].FindControl("lblULDno")).Text;
                        dr[6] = ((Label)gdvULDDetails.Rows[j].FindControl("lblCartNumber")).Text;
                        DTAWBDetails.Rows.Add(dr);
                    }
                }

                Session["AWBdata1"] = DTAWBDetails;

                pnlGrid.Visible = true;
                // pnlGrid.Attributes.item("style") = "Z-INDEX: 176; LEFT: 584px; POSITION: absolute; TOP: 176px";
                pnlGrid.Style["Z-INDEX"] = "150";
                pnlGrid.Style["LEFT"] = "342px";
                pnlGrid.Style["POSITION"] = "absolute";
                pnlGrid.Style["TOP"] = "170px";

                try
                {
                    LoadAWBGrid();
                    dt1 = DTAWBDetails;
                    dtCurrentTable = (DataTable)Session["AWBdata1"];
                    grdAWBs.DataSource = dtCurrentTable;
                    grdAWBs.DataBind();

                    try
                    {
                        ChangeButtonStatus();
                    }
                    catch (Exception)
                    {


                    }

                    if (dt1.Rows.Count > 0)
                    {
                        DataRow drCurrentRow = null;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            drCurrentRow = dtCurrentTable.NewRow();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text = dt1.Rows[i][0].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text = dt1.Rows[i][1].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = dt1.Rows[i][2].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = dt1.Rows[i][3].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = dt1.Rows[i][4].ToString();
                            ((Label)grdAWBs.Rows[i].FindControl("lblULDNO")).Text = dt1.Rows[i][5].ToString();
                            ((Label)grdAWBs.Rows[i].FindControl("lblCartNumber")).Text = dt1.Rows[i][6].ToString();
                            ViewState["CurrentTable1"] = dtCurrentTable;
                        }
                    }

                    AllButtonStatus(false);
                }
                catch (Exception)
                {

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                if (DTAWBDetails != null)
                    DTAWBDetails.Dispose();
                if (dt1 != null)
                    dt1.Dispose();
                if (dtCurrentTable != null)
                    dtCurrentTable.Dispose();
            }
        }

        protected void btnUnassign_Click(object sender, EventArgs e)
        {
            if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                lblDepAirport.Text) == "D")
            {
                BtnList_Click(this, new EventArgs());
                lblStatus.Text = "Flight is departed. Please reopen flight.";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            DataTable DTAWBDetails = new DataTable("Exp_Manifest_btnUnassign_DTAWBDetails");
            DataTable DTULDDetails = new DataTable("Exp_Manifest_btnUnassign_DTULDDetails");
            DataTable dtULDDetails = new DataTable("Exp_Manifest_btnUnassign_dtULDDetails");
            lblStatus.Text = "";
            try
            {
                lblStatus.Text = "";
                int check = 0;
                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
                    {
                        check = check + 1;
                    }
                }
                for (int k = 0; k < gdvULDDetails.Rows.Count; k++)
                {
                    string txt = ((Label)gdvULDDetails.Rows[k].FindControl("lblManifested")).Text;
                    if (((CheckBox)gdvULDDetails.Rows[k].FindControl("Check0")).Checked == true)
                    {
                        if (bool.Parse(((Label)gdvULDDetails.Rows[k].FindControl("lblManifested")).Text))
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "This AWB is Manifested so we can't UnAssign this AWB";
                            return;
                        }
                        if (((Label)gdvULDDetails.Rows[k].FindControl("lblPOL")).Text != lblDepAirport.Text)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "This AWB is Transit so we can't UnAssign this AWB";
                            return;
                        }
                    }

                }
                if (check == 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select One ULD from Manifest TAB for UnAssign";
                    return;
                }
                if (check > 1)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select only One ULD from Manifest TAB for UnAssign";
                    return;
                }

                bool blnFlag = false;

                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
                    {
                        if (((Label)gdvULDDetails.Rows[i].FindControl("lblULDno")).Text.Trim().ToUpper() == "BULK")
                        {
                            for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                            {
                                if (((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBNo")).Text.Trim() == ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text.Trim() &&
                                    ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblCartNumber")).Text.Trim() == ((Label)gdvULDDetails.Rows[i].FindControl("lblCartNumber")).Text.Trim())
                                {
                                    if (gdvULDLoadPlanAWB.Rows[j].Visible == true)
                                    {
                                        ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text = (Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text) + Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblMftPcs")).Text)).ToString();

                                        ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text = (Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text) + Convert.ToDouble(((Label)gdvULDDetails.Rows[i].FindControl("lblMftWt")).Text)).ToString();
                                        blnFlag = true;
                                    }
                                    else
                                    {
                                        ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text = ((Label)gdvULDDetails.Rows[i].FindControl("lblMftPcs")).Text;
                                        ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text = ((Label)gdvULDDetails.Rows[i].FindControl("lblMftWt")).Text;
                                        ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlPCS")).Text = ((Label)gdvULDDetails.Rows[i].FindControl("lblMftPcs")).Text;
                                        ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlWgt")).Text = ((Label)gdvULDDetails.Rows[i].FindControl("lblMftWt")).Text;

                                        ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblTotWgt")).Text = ((Label)gdvULDDetails.Rows[i].FindControl("lblBookedPCS")).Text;
                                        ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblTotWgt")).Text = ((Label)gdvULDDetails.Rows[i].FindControl("lblBookedWgt")).Text;
                                        ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblFltFlag")).Text = ((Label)gdvULDDetails.Rows[i].FindControl("lblFltFlag")).Text.Trim();
                                        ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblCartNumber")).Text = ((Label)gdvULDDetails.Rows[i].FindControl("lblCartNumber")).Text.Trim();
                                        gdvULDLoadPlanAWB.Rows[j].Visible = true;
                                        blnFlag = true;
                                    }
                                }
                            }

                            if (blnFlag == false)
                            {
                                //Added by Poorna

                                DTAWBDetails = (DataTable)Session["AWBdata"];

                                if (DTAWBDetails == null)
                                {
                                    DTAWBDetails = new DataTable("Exp_Manifest_btnUnassign_DTAWBDetails");

                                    DTAWBDetails.Columns.Add("AWBNo");
                                    DTAWBDetails.Columns.Add("Pieces");
                                    DTAWBDetails.Columns.Add("Weight");
                                    DTAWBDetails.Columns.Add("AvlPCS");
                                    DTAWBDetails.Columns.Add("AvlWgt");
                                    DTAWBDetails.Columns.Add("BookedPCS");
                                    DTAWBDetails.Columns.Add("BookedWgt");
                                    DTAWBDetails.Columns.Add("FltNo");
                                    DTAWBDetails.Columns.Add("CartNumber");
                                }


                                //for (int x = 0; x < gdvULDDetails.Rows.Count; x++)
                                //{
                                if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
                                {
                                    DataRow dr;

                                    dr = DTAWBDetails.NewRow();
                                    dr[0] = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text;
                                    dr[1] = ((Label)gdvULDDetails.Rows[i].FindControl("lblMftPcs")).Text;
                                    dr[2] = ((Label)gdvULDDetails.Rows[i].FindControl("lblMftWt")).Text;
                                    dr[3] = ((Label)gdvULDDetails.Rows[i].FindControl("lblStatedPCS")).Text;
                                    dr[4] = ((Label)gdvULDDetails.Rows[i].FindControl("lblStatedWgt")).Text;

                                    dr[6] = ((Label)gdvULDDetails.Rows[i].FindControl("lblBookedPCS")).Text;
                                    dr[7] = ((Label)gdvULDDetails.Rows[i].FindControl("lblBookedWgt")).Text;
                                    dr["FltNo"] = ((Label)gdvULDDetails.Rows[i].FindControl("lblFltFlag")).Text.Trim();
                                    dr["CartNumber"] = ((Label)gdvULDDetails.Rows[i].FindControl("lblCartNumber")).Text.Trim();

                                    DTAWBDetails.Rows.Add(dr);
                                }
                            }

                            Session["AWBdata"] = DTAWBDetails;
                            if (DTAWBDetails != null)
                                LoadAWBDataGrid(DTAWBDetails);

                            dtULDDetails = (DataTable)Session["ManifestGridData"];// (DataTable)Session["GDVULDDetails"];

                            if (dtULDDetails != null)
                                dtULDDetails.Rows.RemoveAt(i);

                            gdvULDDetails.DataSource = dtULDDetails;
                            gdvULDDetails.DataBind();
                            try
                            {
                                ChangeButtonStatus();
                            }
                            catch (Exception)
                            {
                            }

                            Session["GDVULDDetails"] = dtULDDetails;
                            Session["ManifestGridData"] = dtULDDetails;

                            //ShowULDAWBSummary();
                            hdnManifestFlag.Value = "1";
                        }
                        else
                        {
                            //Add ULd to left grid
                            for (int j = 0; j < gdvULDLoadPlan.Rows.Count; j++)
                            {
                                if (gdvULDLoadPlan.Rows[j].Cells[1].Text == ((Label)gdvULDDetails.Rows[i].FindControl("lblULDno")).Text)
                                {
                                    if (gdvULDLoadPlan.Rows[j].Visible == true)
                                    {
                                        gdvULDLoadPlan.Rows[j].Cells[2].Text = (Convert.ToInt32(gdvULDLoadPlan.Rows[j].Cells[2].Text) + 1).ToString();
                                        gdvULDLoadPlan.Rows[j].Cells[3].Text = (Convert.ToInt32(gdvULDLoadPlan.Rows[j].Cells[3].Text) + Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblMftPcs")).Text)).ToString();

                                        gdvULDLoadPlan.Rows[j].Cells[4].Text = (Convert.ToDouble(gdvULDLoadPlan.Rows[j].Cells[4].Text) + Convert.ToDouble(((Label)gdvULDDetails.Rows[i].FindControl("lblMftWt")).Text)).ToString();
                                        blnFlag = true;
                                    }
                                    else
                                    {
                                        gdvULDLoadPlan.Rows[j].Visible = true;
                                        blnFlag = true;
                                    }
                                }
                            }

                            if (blnFlag == false)
                            {
                                DTULDDetails = (DataTable)Session["ULDdata"];

                                if (DTULDDetails == null)
                                {
                                    DTULDDetails = new DataTable("Exp_Manifest_btnUnassign_DTULDDetails");
                                    DTULDDetails.Columns.Add("ULDNo");
                                    DTULDDetails.Columns.Add("Count");
                                    DTULDDetails.Columns.Add("PCS");
                                    DTULDDetails.Columns.Add("Weight");
                                    DTULDDetails.Columns.Add("FltNo");
                                }

                                if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
                                {
                                    DataRow dr;

                                    dr = DTULDDetails.NewRow();
                                    dr[0] = ((Label)gdvULDDetails.Rows[i].FindControl("lblULDno")).Text;
                                    dr[1] = 1;
                                    dr[2] = ((Label)gdvULDDetails.Rows[i].FindControl("lblMftPcs")).Text;
                                    dr[3] = ((Label)gdvULDDetails.Rows[i].FindControl("lblMftWt")).Text;
                                    dr[4] = ((Label)gdvULDDetails.Rows[i].FindControl("lblFltFlag")).Text.Trim();

                                    DTULDDetails.Rows.Add(dr);
                                }
                            }

                            Session["ULDdata"] = DTULDDetails;
                            if (DTULDDetails != null)
                                LoadULDDataGrid(DTULDDetails);

                            dtULDDetails = (DataTable)Session["ManifestGridData"];// (DataTable)Session["GDVULDDetails"];

                            if (dtULDDetails != null)
                                dtULDDetails.Rows.RemoveAt(i);

                            gdvULDDetails.DataSource = dtULDDetails;
                            gdvULDDetails.DataBind();
                            try
                            {
                                ChangeButtonStatus();
                            }
                            catch (Exception)
                            {

                            }

                            Session["GDVULDDetails"] = dtULDDetails;
                            Session["ManifestGridData"] = dtULDDetails;

                            //ShowULDAWBSummary();
                            hdnManifestFlag.Value = "1";
                        }
                    }
                }
                //HighlightDataGridRows();
                btnSave_Click(null, null);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (DTAWBDetails != null)
                    DTAWBDetails.Dispose();
                if (DTULDDetails != null)
                    DTULDDetails.Dispose();
                if (dtULDDetails != null)
                    dtULDDetails.Dispose();
            }
        }

        protected void btnSplitAssign_Click(object sender, EventArgs e)
        {
            if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                lblDepAirport.Text) == "D")
            {
                BtnList_Click(this, new EventArgs());
                lblStatus.Text = "Flight is departed. Please reopen flight.";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            DataTable dt = new DataTable("Exp_Manifest_btnSplitAssign_dt");
            DataTable DTAWBDetails = new DataTable("Exp_Manifest_btnSplitAssign_DTAWBDetails");
            DataTable dt1 = new DataTable("Exp_Manifest_btnSplitAssign_dt1");
            DataTable dtCurrentTable = new DataTable("Exp_Manifest_btnSplitAssign_dtCurrentTable");
            lblStatus.Text = "";
            try
            {
                ddlReason.Visible = false;
                int check = 0, ULdCheck = 0;
                for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                    {
                        check = check + 1;

                        if (((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblFltFlag")).Text.Trim() == "")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Splitting can not happen on AWB - " + ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAWBno")).Text.Trim();
                            return;
                        }
                    }

                }
                for (int i = 0; i < gdvULDLoadPlan.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDLoadPlan.Rows[i].FindControl("Check1")).Checked == true)
                    {
                        ULdCheck = ULdCheck + 1;
                    }
                }

                if (ULdCheck > 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "ULD can not be split.";
                    return;
                }

                if (check > 1)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select only one shipment for Split.";
                    return;
                }

                if (check == 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select AWB for Split and Assign";
                    return;
                }

                Session["Split"] = "A";
                btnAddManifest.Text = "Add to Manifest";
                lblNextFlight.Visible = false;
                txtNextFlight.Visible = false;

                grdAWBs.Columns[2].Visible = true;
                grdAWBs.Columns[3].Visible = true;
                grdAWBs.Columns[5].Visible = false;
                grdAWBs.Columns[6].Visible = false;
                UpdatePanelRouteDetails.Visible = false;
                lblNFltDate.Visible = false;
                txtNFltDate.Visible = false;

                lblReason.Visible = false;
                txtReason.Visible = false;

                DTAWBDetails.Columns.Add("AWBNo");
                DTAWBDetails.Columns.Add("Pieces");
                DTAWBDetails.Columns.Add("Weight");
                DTAWBDetails.Columns.Add("AvlPCS");
                DTAWBDetails.Columns.Add("AvlWgt");
                DTAWBDetails.Columns.Add("ULDNo");
                DTAWBDetails.Columns.Add("CartNumber");

                for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
                    {
                        DataRow dr;

                        dr = DTAWBDetails.NewRow();
                        dr[0] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text;// gdvULDLoadPlanAWB.Rows[j].Cells[1].Text;
                        dr[1] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text;//gdvULDLoadPlanAWB.Rows[j].Cells[2].Text;
                        dr[2] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text;
                        dr[3] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlPCS")).Text;//((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlPCS")).Text;
                        dr[4] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlWgt")).Text;//((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlWgt")).Text;
                        dr[5] = "BULK";
                        dr[6] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblCartNumber")).Text.Trim();

                        DTAWBDetails.Rows.Add(dr);
                    }
                }

                Session["AWBdata1"] = DTAWBDetails;

                pnlGrid.Visible = true;
                // pnlGrid.Attributes.item("style") = "Z-INDEX: 176; LEFT: 584px; POSITION: absolute; TOP: 176px";
                pnlGrid.Style["Z-INDEX"] = "150";
                pnlGrid.Style["LEFT"] = "342px";
                pnlGrid.Style["POSITION"] = "absolute";
                pnlGrid.Style["TOP"] = "170px";

                try
                {
                    LoadAWBGrid();
                    dt1 = DTAWBDetails;
                    dtCurrentTable = (DataTable)Session["AWBdata1"];
                    grdAWBs.DataSource = dtCurrentTable;
                    grdAWBs.DataBind();
                    if (dt1.Rows.Count > 0)
                    {
                        DataRow drCurrentRow = null;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            drCurrentRow = dtCurrentTable.NewRow();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text = dt1.Rows[i][0].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text = dt1.Rows[i][1].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = dt1.Rows[i][2].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = dt1.Rows[i][3].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = dt1.Rows[i][4].ToString();
                            ((Label)grdAWBs.Rows[i].FindControl("lblULDNO")).Text = dt1.Rows[i][5].ToString();
                            ((Label)grdAWBs.Rows[i].FindControl("lblCartNumber")).Text = dt1.Rows[i][6].ToString();

                            ViewState["CurrentTable1"] = dtCurrentTable;
                        }
                    }

                    AllButtonStatus(false);
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                if (DTAWBDetails != null)
                    DTAWBDetails.Dispose();
                if (dt1 != null)
                    dt1.Dispose();
                if (dtCurrentTable != null)
                    dtCurrentTable.Dispose();
            }
        }

        public void ShowSplitAWBGrid()
        {
            DataTable dtSplitData = new DataTable("Exp_Manifest_ShowSplitAWBGrid_dtSplitData");
            try
            {
                dtSplitData = (DataTable)Session["AWBSplitData"];

                string s = txtFlightID.Text;
                if (dtSplitData.Rows.Count >= 0)
                {
                    for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
                    {
                        if (gdvULDLoadPlanAWB.Rows[i].Cells[1].Text == dtSplitData.Rows[i][0].ToString())
                        {
                            gdvULDLoadPlanAWB.Rows[i].Cells[2].Text = dtSplitData.Rows[i][1].ToString();
                            gdvULDLoadPlanAWB.Rows[i].Cells[3].Text = dtSplitData.Rows[i][2].ToString();
                            gdvULDLoadPlanAWB.Rows[i].Cells[4].Text = dtSplitData.Rows[i][3].ToString();
                            gdvULDLoadPlanAWB.Rows[i].Cells[5].Text = dtSplitData.Rows[i][4].ToString();
                        }
                    }
                    gdvULDLoadPlanAWB.DataBind();
                    try
                    {
                        ChangeButtonStatus();
                    }
                    catch (Exception)
                    {


                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dtSplitData != null)
                    dtSplitData.Dispose();
            }
        }

        protected void btnAddManifest_Click(object sender, EventArgs e)
        {

            BLExpManifest objExpManifest = new BLExpManifest();
            ShowFlightsBAL objBAL = new ShowFlightsBAL();
            DataSet objDS = new DataSet("Exp_Manifest_btnAddManifest_objDS");
            DataSet dsawbData = new DataSet("Exp_Manifest_btnAddManifest_dsawbData");
            DataTable dsawb1 = new DataTable("Exp_Manifest_btnAddManifest_dsawb1");
            string CartNumber = string.Empty;
            lblStatus.Text = "";
            try
            {
                Label1.Text = "";
                int PCS = 0;

                //Added on 28 sept 

                for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[j].FindControl("Check0")).Checked == true)
                    {
                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblMftPcs")).Text))
                        {
                            Label1.Text = "Please enter valid Pcs";
                            Label1.ForeColor = Color.Red;
                            return;
                        }
                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblMftWt")).Text))
                        {
                            Label1.Text = "Please enter valid Weight";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblMftPcs")).Text))
                        {
                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) < Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblMftWt")).Text))
                            {
                                Label1.Text = "Please enter valid Pcs";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                        }

                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) == Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblMftWt")).Text))
                        {
                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblMftPcs")).Text))
                            {
                                Label1.Text = "Please enter valid Weight";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                        }
                        break;
                    }
                }


                string strMode = Convert.ToString(Session["Split"]);
                double OffloadWGT = 0.0;

                #region "Return TO Shipper"

                if (strMode == "RSI") // RSI - Return to Shipper
                {
                    string ActualFltNo = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                    DateTime ActualFltDate = DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null);
                    string OffloadLoc = lblDepAirport.Text.Trim();

                    string Number = ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim();
                    string AWBPrefix = string.Empty;
                    string strAWBno = string.Empty;
                    //string CartNumber = ((Label)grdAWBs.Rows[0].FindControl("lblCartNumber")).Text.Trim();

                    if (Number.Replace("&nbsp;", "") != "")
                    {
                        AWBPrefix = Number.Substring(0, Number.IndexOf('-'));
                        strAWBno = Number.Substring(Number.IndexOf('-') + 1, 8);
                    }

                    int APCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text.Trim());
                    double AWGT = double.Parse(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text.Trim());

                    if (((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text.Trim() == "")
                    {
                        Label1.Text = "Please enter valid Pcs";
                        Label1.ForeColor = Color.Red;
                        return;
                    }

                    if (((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text.Trim() == "")
                    {
                        Label1.Text = "Please enter valid Weight";
                        Label1.ForeColor = Color.Red;
                        return;
                    }

                    int OffloadPCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text.Trim());
                    OffloadWGT = double.Parse(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text.Trim());
                    string UserName = Session["UserName"].ToString();

                    string POLoading = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                    string POUnloading = ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text.Trim();
                    string strULDNo = ((Label)grdAWBs.Rows[0].FindControl("lblULDNO")).Text.Trim();
                    CartNumber = ((Label)grdAWBs.Rows[0].FindControl("lblCartNumber")).Text.Trim();

                    if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text))
                    {
                        Label1.Text = "Please enter valid Pcs";
                        Label1.ForeColor = Color.Red;
                        return;
                    }

                    if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text))
                    {
                        Label1.Text = "Please enter valid Weight";
                        Label1.ForeColor = Color.Red;
                        return;
                    }
                    string PartnerCode = ((DropDownList)grdRouting.Rows[0].FindControl("ddlPartner")).SelectedValue;
                    string Partnertype = ((DropDownList)grdRouting.Rows[0].FindControl("ddlPartnerType")).SelectedValue;

                    bool blnResult = objExpManifest.OffLoadShipmentinManifestForGHA(txtFlightCode.Text.Trim() + txtFlightID.Text.Trim(), "", OffloadLoc, strAWBno, APCS, AWGT, OffloadPCS, OffloadWGT,
                       UserName, POLoading, POUnloading, ddlVersion.Text.ToString(), Convert.ToDateTime(Session["IT"]), ddlReason.SelectedValue, strMode, DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null), Convert.ToDateTime(Session["IT"]), PartnerCode, Partnertype,
                       strULDNo, Convert.ToDateTime(Session["IT"]), AWBPrefix, "", HidFltFlag.Value, CartNumber);

                    objExpManifest = null;

                    if (blnResult)
                    {

                        //Code Added by Deepak for Customs Messaging
                        try
                        {

                            CustomsImportBAL objCustoms = new CustomsImportBAL();
                            object[] QueryValues = new object[3];
                            QueryValues[0] = AWBPrefix + "-" + strAWBno;
                            QueryValues[1] = ActualFltNo;
                            QueryValues[2] = ActualFltDate.ToString("dd/MM/yyyy");
                            string[] AWBDetails = QueryValues[0].ToString().Split('-');
                            DataSet dCust = new DataSet("Exp_Manifest_btnAddManifest_dCust");
                            dCust = objCustoms.CheckCustomsAWBAvailability(QueryValues);
                            if (dCust != null)
                            {
                                if (dCust.Tables[0].Rows[0]["Validate"].ToString() == "True" && dCust.Tables[1].Rows[0]["Validate"].ToString() == "True")
                                {

                                    StringBuilder sb = objCustoms.EncodingFRXMessage(QueryValues);
                                    object[] QueryVal = { QueryValues[0], QueryValues[1], QueryValues[2], sb.ToString().ToUpper() };
                                    objCustoms.UpdateFRXMessageOffload(QueryVal);
                                    if (sb != null)
                                    {
                                        if (sb.ToString() != "")
                                        {
                                            object[] QueryValMail = { "FRX", ActualFltNo, ActualFltDate.ToString("dd/MM/yyyy") };
                                            //Getting MailID for FRX Message
                                            DataSet dMail = new DataSet("Exp_Manifest_btnAddManifest_dMail");
                                            dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                            string MailID = string.Empty;
                                            if (dMail != null)
                                            {
                                                MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                            }
                                            cls_BL.addMsgToOutBox("FRX", sb.ToString().ToUpper(), "", MailID);
                                        }
                                    }
                                }
                            }
                            objCustoms = null;
                            dCust.Dispose();

                        }
                        catch (Exception ex)
                        { }
                        pnlGrid.Visible = false;
                        BtnList_Click(null, null);
                        lblStatus.Text = "Return to shipper successful !";
                        lblStatus.ForeColor = Color.Green;
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "Return to shipper failed. Please try again..";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                #endregion "Return To Shipper"

                #region "Split & UnAssign"

                if (strMode == "U") //Split & Unassign
                {
                    if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                        lblDepAirport.Text) == "D")
                    {
                        BtnList_Click(this, new EventArgs());
                        lblStatus.Text = "Flight is departed. Please reopen flight.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                    {
                        if (((CheckBox)gdvULDDetails.Rows[j].FindControl("Check0")).Checked == true)
                        {
                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblMftPcs")).Text))
                            {
                                Label1.Text = "Please enter valid Pcs";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblMftWt")).Text))
                            {
                                Label1.Text = "Please enter valid Weight";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblMftPcs")).Text))
                            {
                                if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) < Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblMftWt")).Text))
                                {
                                    Label1.Text = "Please enter valid Weight";
                                    Label1.ForeColor = Color.Red;
                                    return;
                                }
                            }

                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) == Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblMftWt")).Text))
                            {
                                if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblMftPcs")).Text))
                                {
                                    Label1.Text = "Please enter valid PCS";
                                    Label1.ForeColor = Color.Red;
                                    return;
                                }
                            }

                            break;
                        }
                    }
                    SplitAndUnAssignfunction();
                    AllButtonStatus(true);
                    btnSave_Click(null, null);
                    return;
                }

                #endregion "Split & UnAssign"

                #region "Offload Shipment"

                if (strMode == "O") //Offload Shipment
                {
                    if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                        lblDepAirport.Text) == "D")
                    {
                        BtnList_Click(this, new EventArgs());
                        lblStatus.Text = "Flight is departed. Please reopen flight.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    if (UpdatePanelRouteDetails.Visible == true)
                    {
                        int chkPCS = 0;
                        double chkWGT = 0.0;
                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == "")
                            {
                                Label1.Text = "Please enter valid Origin";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim() == "")
                            {
                                Label1.Text = "Please enter valid Destination";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim())
                            {
                                Label1.Text = "Please enter valid Origin & Destination";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            string Org = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                            if (Org != lblDepAirport.Text)
                            {
                                Label1.Text = "Please enter valid Origin";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim().ToUpper() != ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim().ToUpper())
                            {
                                Label1.Text = "Please enter valid Destination";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == null)
                            {
                                Label1.Text = "Please enter valid PCS";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == null)
                            {
                                Label1.Text = "Please enter valid PCS";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == null)
                            {
                                Label1.Text = "Please enter valid Weight ";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (lblDepAirport.Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim())
                            {
                                chkPCS = chkPCS + Convert.ToInt32(((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim());
                                chkWGT = chkWGT + Convert.ToDouble(((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim());
                            }
                            if (chkPCS > Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text))
                            {
                                Label1.Text = "Please enter valid PCS";
                                Label1.ForeColor = Color.Red;
                                return;

                            }
                            if (chkWGT > Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text))
                            {
                                Label1.Text = "Please enter valid Weight";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (chkPCS == 0)
                            {
                                Label1.Text = "Please enter valid PCS ";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                            if (chkWGT == 0)
                            {
                                Label1.Text = "Please enter valid Weight ";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (chkPCS == Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text.Trim()) &&
                                chkWGT != Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text.Trim()))
                            {
                                Label1.Text = "Please enter valid Weight ";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (
                                chkWGT == Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text.Trim()) &&
                                chkPCS != Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text.Trim()))
                            {
                                Label1.Text = "Please enter valid Pieces";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
                            {
                                Label1.ForeColor = Color.Red;
                                Label1.Text = "Please enter Next Flight Details.";
                                return;
                            }

                            if (ddlReason.SelectedItem.Text == "Others" && txtReason.Text.Trim() == "")
                            {
                                Label1.Text = "Please enter Offload reason";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                        }
                    }

                    if (txtNextFlight.Text.ToUpper().Trim() != "")
                    {
                        string Origin = Convert.ToString(Session["Station"]);
                        string FltNo = txtNextFlight.Text.ToUpper().Trim();
                        string Dest = ddlMainPOU.SelectedItem.Value.ToString();
                        DateTime dt = DateTime.ParseExact(txtNFltDate.Text.Trim(), "dd/MM/yyyy", null);

                        int diff = (dt - dtCurrentDate.Date).Days;

                        for (int ICount = 0; ICount < gdvULDDetails.Rows.Count; ICount++)
                        {
                            if (((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim() == ((Label)gdvULDDetails.Rows[ICount].FindControl("lblAWBno")).Text.Trim())
                            {
                                Dest = ((Label)gdvULDDetails.Rows[ICount].FindControl("lblAWBDest")).Text.Trim();
                                break;
                            }
                        }

                        string errormessage = string.Empty;
                        string flightID = txtFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();
                        string AWBNo = ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim();
                        AWBNo = AWBNo.Substring(AWBNo.Length - 8);
                        objBAL.GetFlightListForManifest(Origin, Dest, diff, ref objDS, ref errormessage, dtCurrentDate.Date, AWBNo, flightID, DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null));
                        bool blnExists = false;

                        if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                        {
                            for (int ICount = 0; ICount < objDS.Tables[0].Rows.Count; ICount++)
                            {
                                if (FltNo == objDS.Tables[0].Rows[ICount]["FltNumber"].ToString().Trim())
                                {
                                    blnExists = true;
                                    break;
                                }
                            }
                        }

                        objBAL = null;
                        objDS = null;

                        if (blnExists == false)
                        {
                            Label1.ForeColor = Color.Red;
                            Label1.Text = "Please enter valid flight number.";
                            return;
                        }
                    }
                    //

                    SaveOffLoadDetails();
                    AllButtonStatus(true);
                    //Enforce user to Manifest the flight
                    btnDepartFit.Enabled = false;

                    //Button Cancel called to clear off the previous data entry
                    btnCancel_Click(null, null);
                    return;
                }

                #endregion "Offload Shipment"

                #region "Re-Assign Shipment"

                if (strMode == "R") //Re-Assign Shipment
                {
                    if (UpdatePanelRouteDetails.Visible == true)
                    {
                        int chkPCS = 0;
                        double chkWGT = 0.0;
                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            //if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text == "Select")
                            //{
                            //    Label1.Text = "Please enter valid flight";
                            //    Label1.ForeColor = Color.Red;
                            //    return;
                            //}

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == "")
                            {
                                Label1.Text = "Please enter valid Origin";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim() == "")
                            {
                                Label1.Text = "Please enter valid Destination";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim())
                            {
                                Label1.Text = "Please enter valid Origin & Destination";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                            string Org = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                            if (Org != lblDepAirport.Text)
                            {
                                Label1.Text = "Please enter valid Origin";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                            if (((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim() != ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim())
                            {   //If flight destination is not matching with Route destination then check if it matches destination of AWB.
                                string awbNo = ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim();
                                //Find destination of AWB.
                                string awbDest = objExpManifest.GetAWBDestination(awbNo.Substring(0,awbNo.IndexOf('-')), 
                                    awbNo.Substring(awbNo.IndexOf('-') + 1));
                                if (awbDest != null && awbDest != "")
                                {
                                    if (((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim() != awbDest)
                                    {
                                        Label1.Text = "Please enter valid Destination";
                                        Label1.ForeColor = Color.Red;
                                        return;
                                    }

                                }
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == null)
                            {
                                Label1.Text = "Please enter valid PCS";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == null)
                            {
                                Label1.Text = "Please enter valid PCS";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                            if (((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == null)
                            {
                                Label1.Text = "Please enter valid Weight ";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (lblDepAirport.Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim())
                            {
                                chkPCS = chkPCS + Convert.ToInt32(((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim());
                                chkWGT = chkWGT + Convert.ToDouble(((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim());
                            }
                            if (chkPCS > Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text))
                            {
                                Label1.Text = "Please enter valid PCS";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                            if (chkWGT > Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text))
                            {
                                Label1.Text = "Please enter valid Weight";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (chkPCS == 0)
                            {
                                Label1.Text = "Please enter valid PCS ";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                            if (chkWGT == 0)
                            {
                                Label1.Text = "Please enter valid Weight ";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
                            {
                                Label1.ForeColor = Color.Red;
                                Label1.Text = "Please enter Next Flight Details.";
                                return;
                            }

                            if (ddlReason.SelectedValue == "Others" && txtReason.Text == "")
                            {
                                Label1.Text = "Please enter Offload reason";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                        }
                    }

                    if (txtNextFlight.Text.ToUpper().Trim() != "")
                    {
                        string Origin = Convert.ToString(Session["Station"]);
                        string FltNo = txtNextFlight.Text.ToUpper().Trim();
                        string Dest = Convert.ToString(Session["Dest"]);/// ddlMainPOU.SelectedItem.Value.ToString();
                        DateTime dt = DateTime.ParseExact(txtNFltDate.Text.Trim(), "dd/MM/yyyy", null);

                        int diff = (dt - dtCurrentDate.Date).Days;

                        string errormessage = string.Empty;
                        string flightID = txtFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();

                        string AWBNo = ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim();
                        AWBNo = AWBNo.Substring(AWBNo.Length - 8);

                        objBAL.GetFlightListForManifest(Origin, Dest, diff, ref objDS, ref errormessage, dtCurrentDate.Date, AWBNo, flightID, DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null));
                        bool blnExists = false;

                        if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                        {
                            for (int ICount = 0; ICount < objDS.Tables[0].Rows.Count; ICount++)
                            {
                                if (FltNo == objDS.Tables[0].Rows[ICount]["FltNumber"].ToString())
                                {
                                    blnExists = true;
                                    break;
                                }
                            }
                        }

                        if (blnExists == false)
                        {
                            Label1.ForeColor = Color.Red;
                            Label1.Text = "Please enter valid flight number.";
                            return;
                        }
                    }
                    //

                    SaveOffLoadDetails();
                    AllButtonStatus(true);
                    return;
                }

                #endregion "Re-Assign Shipment"

                //Validate flight departed status before updating data in database.
                if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                    lblDepAirport.Text) == "D")
                {
                    BtnList_Click(this, new EventArgs());
                    lblStatus.Text = "Flight is departed. Please reopen flight.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby = "", AWBDest = "", CommCode = "";
                int AVLPCS = 0, AWBPcs = 0;
                double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, AWBGwt = 0;
                //string Updatedon = "";
                string LoadingPriority = "", Remark = "", Desc = "", Vol = "";
                string SCC = "";

                for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                {
                    //  if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
                    if (((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text == ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text)
                    {
                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                        {
                            Label1.Text = "Please enter valid Pcs";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                        {
                            Label1.Text = "Please enter valid Pcs";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                        {
                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) < Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                            {
                                Label1.Text = "Please enter valid Weight";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                        }

                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) == Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                        {
                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                            {
                                Label1.Text = "Please enter valid Pcs";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                        }
                        break;
                    }
                }

                for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                {
                    //  if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
                    if (((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text == AWBno)
                    {
                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                        {
                            Label1.Text = "Please enter valid Pcs";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                        {
                            Label1.Text = "Please enter valid Pcs";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                        {
                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) < Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                            {
                                Label1.Text = "Please enter valid Weight";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                        }

                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) == Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                        {
                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                            {
                                Label1.Text = "Please enter valid Pcs";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                        }
                        break;
                    }
                }

                FLTno = txtFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();
                POL = Convert.ToString(Session["Station"]);
                //POU = ddlMainPOU.SelectedItem.Value.ToString();
                POU = ddlMainPOU.Items[0].Value.ToString();
                Updatedby = Convert.ToString(Session["Username"]);
                ULDno = "BULK";
                ULDwgt = 0;
                string IsBonded = string.Empty, AWBType = string.Empty, FltFlag = string.Empty;

                DateTime FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);

                dsawbData = objExpMani.GetAwbTabdetails_GHA(lblDepAirport.Text, FLTno, FlightDate, "", "", "");

                if (Session["AWBdata"] != null)
                {
                    dsawb1 = (DataTable)Session["AWBdata"];
                }
                else
                {
                    dsawb1 = dsawbData.Tables[0];
                }

                for (int i = 0; i < grdAWBs.Rows.Count; i++)
                {
                    //Appended 25apr
                    AWBno = ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text.Trim();
                    string GridULDNo = ((Label)grdAWBs.Rows[i].FindControl("lblULDNO")).Text.Trim();

                    PCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text);
                    WGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text);
                    CartNumber = ((Label)grdAWBs.Rows[i].FindControl("lblCartNumber")).Text.Trim();

                    for (int k = 0; k < dsawb1.Rows.Count; k++)
                    {
                        if (((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAWBno")).Text == AWBno)
                        {
                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text))
                            {
                                if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) < Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWeight")).Text))
                                {
                                    Label1.Text = "Please enter valid Weight";
                                    Label1.ForeColor = Color.Red;
                                    return;
                                }
                            }

                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) == Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWeight")).Text))
                            {
                                if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text))
                                {
                                    Label1.Text = "Please enter valid Pcs";
                                    Label1.ForeColor = Color.Red;
                                    return;
                                }
                            }

                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text = (int.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text) - PCS).ToString();
                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWeight")).Text = (double.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWeight")).Text) - WGT).ToString();
                            AVLPCS = Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlPCS")).Text);
                            AVLWGT = Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlWgt")).Text);

                            AWBPcs = Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblTotPCS")).Text);
                            AWBGwt = Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblTotWgt")).Text);

                            dsawb1.Rows[k][1] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text;
                            dsawb1.Rows[k][2] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWeight")).Text;
                            dsawbData.Tables[1].Rows[k][1] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text;
                            dsawbData.Tables[1].Rows[k][2] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWeight")).Text;
                            // Updatedon = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                            FltFlag = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblFltFlag")).Text.Trim();
                            try
                            {
                                for (int m = 0; m < dsawb1.Rows.Count; m++)
                                {
                                    //if (dsawb1.Rows[m][0].ToString() == AWBno)
                                    if (dsawbData.Tables[1].Rows[m][0].ToString() == AWBno)
                                    {
                                        AWBDest = dsawbData.Tables[1].Rows[m][5].ToString();
                                        Desc = dsawbData.Tables[1].Rows[m][6].ToString();
                                        Vol = dsawbData.Tables[1].Rows[m][7].ToString();
                                        SCC = dsawbData.Tables[1].Rows[m][6].ToString();
                                        CommCode = dsawbData.Tables[1].Rows[m]["CommodityCode"].ToString();
                                        IsBonded = dsawbData.Tables[1].Rows[m]["IsBonded"].ToString();
                                        AWBType = dsawbData.Tables[1].Rows[m]["Type"].ToString();
                                        POU = dsawbData.Tables[1].Rows[m]["FltDestination"].ToString();
                                        break;
                                    }
                                }

                                //adding collected data to gridview rows one by one
                            }
                            catch (Exception ex)
                            {
                            }


                            if (int.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text) == 0)
                            {
                                dsawb1.Rows[k].Delete();
                                dsawbData.Tables[1].Rows[k].Delete();

                                dsawb1.AcceptChanges();
                                dsawbData.Tables[1].AcceptChanges();
                            }
                        }
                    }

                    tabMenifestDetails = (DataTable)Session["ManifestGridData"];

                    int duplicnt = 0;
                    bool ismatch = false;
                    for (int chk = 0; chk < tabMenifestDetails.Rows.Count; chk++)
                    {
                        if (tabMenifestDetails.Rows[chk]["AWBno"].ToString() == AWBno && tabMenifestDetails.Rows[chk]["ULDno"].ToString() == GridULDNo)
                        {
                            tabMenifestDetails.Rows[chk]["PCS"] = (int.Parse(tabMenifestDetails.Rows[chk]["PCS"].ToString()) + PCS).ToString();
                            tabMenifestDetails.Rows[chk]["GrossWgt"] = (double.Parse(tabMenifestDetails.Rows[chk]["GrossWgt"].ToString()) + WGT).ToString();
                            duplicnt = duplicnt + 1;
                            ismatch = true;
                            //break from loop if matching row is found.
                            //Set is manifested = 'false' for resepctive AWB row.
                            tabMenifestDetails.Rows[chk]["Manifested"] = false;
                            break;
                        }
                    }
                    if (ismatch == false)
                    {
                        DataRow l_Datarow = tabMenifestDetails.NewRow();
                        bool IsManifest = false;

                        l_Datarow["ULDno"] = ULDno;
                        l_Datarow["POU"] = POU;
                        l_Datarow["POL"] = POL;
                        l_Datarow["AWBDest"] = AWBDest;
                        l_Datarow["Counter"] = AWBType;
                        l_Datarow["AWBno"] = AWBno;
                        l_Datarow["SCC"] = CommCode;
                        l_Datarow["PCS"] = PCS;
                        l_Datarow["GrossWgt"] = WGT;
                        l_Datarow["VOL"] = Vol;
                        l_Datarow["StatedPCS"] = AVLPCS;
                        l_Datarow["StatedWgt"] = AVLWGT;

                        l_Datarow["BookedPCS"] = AWBPcs;
                        l_Datarow["BookedWgt"] = AWBGwt;

                        l_Datarow["Desc"] = Desc;
                        l_Datarow["Orign"] = POL;
                        l_Datarow["Dest"] = AWBDest;
                        l_Datarow["Manifested"] = IsManifest;
                        l_Datarow["LoadingPriority"] = LoadingPriority;

                        l_Datarow["Remark"] = Remark;
                        l_Datarow["Bonded"] = IsBonded;

                        /* ADDED AND COMMENTED BY VISHAL 04/04/2014
                        l_Datarow["FltNo"] = txtFlightCode.Text + txtFlightID.Text;
                        ADDED AND COMMENTED BY VISHAL 04/04/2014 */

                        l_Datarow["FltNo"] = FltFlag;
                        l_Datarow["CartNumber"] = CartNumber;

                        tabMenifestDetails.Rows.Add(l_Datarow);
                    }

                    if (tabMenifestDetails != null && tabMenifestDetails.Rows.Count > 0)
                    {
                        Session["ManifestGridData"] = tabMenifestDetails;
                    }

                    //added on 28 sept

                    gdvULDLoadPlanAWB.DataSource = null;
                    gdvULDLoadPlanAWB.DataBind();
                    gdvULDLoadPlanAWB.DataSource = dsawb1;
                    gdvULDLoadPlanAWB.DataBind();

                    if (dsawb1.Rows.Count > 0)
                    {
                        for (int k = 0; k < dsawb1.Rows.Count; k++)
                        {
                            //  tabMenifestDetails.Rows.Add(l_Datarow);
                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAWBno")).Text = dsawb1.Rows[k][0].ToString();
                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text = dsawb1.Rows[k][1].ToString();
                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWeight")).Text = dsawb1.Rows[k][2].ToString();
                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlPCS")).Text = dsawb1.Rows[k][3].ToString();
                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlWgt")).Text = dsawb1.Rows[k][4].ToString();

                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblTotPCS")).Text = dsawb1.Rows[k]["AWBPcs"].ToString();
                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblTotWgt")).Text = dsawb1.Rows[k]["AWBGwt"].ToString();
                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblCartNumber")).Text = dsawb1.Rows[k]["CartNumber"].ToString();
                        }
                    }
                    Session["AWBdata"] = dsawb1;// dsawbData.Tables[1];

                    gdvULDDetails.DataSource = "";

                    gdvULDDetails.DataSource = (DataTable)Session["ManifestGridData"];
                    gdvULDDetails.DataBind();
                }

                try
                {
                    for (int k = 0; k < gdvULDDetails.Rows.Count; k++)
                    {
                        if (((Label)gdvULDDetails.Rows[k].FindControl("lblAWBno")).Text.Trim() == tabMenifestDetails.Rows[k]["AWBNo"].ToString())
                        {
                            if (((TextBox)gdvULDDetails.Rows[k].FindControl("txtRemark")).Text == "")
                            {
                                ((TextBox)gdvULDDetails.Rows[k].FindControl("txtRemark")).Text = tabMenifestDetails.Rows[k]["Remark"].ToString();

                                ((DropDownList)gdvULDDetails.Rows[k].FindControl("ddlLoadingPriority")).SelectedValue = tabMenifestDetails.Rows[k]["LoadingPriority"].ToString();
                            }
                        }
                    }

                    for (int m = 0; m < gdvULDDetails.Rows.Count; m++)
                    {
                        for (int r = 0; r < dsawbData.Tables[2].Rows.Count; r++)
                        {
                            if (dsawbData.Tables[2].Rows.Count > 0)
                            {
                                string AWB = ((Label)gdvULDDetails.Rows[m].FindControl("lblAWBno")).Text.Trim();
                                if (dsawbData.Tables[2].Rows[r]["AWBNo"].ToString() == AWB.Substring(AWB.Length - 8))//AWBno.SubString(AWB.Length-8)) 
                                {
                                    Remark = dsawbData.Tables[2].Rows[r]["Remarks"].ToString();
                                    ((TextBox)gdvULDDetails.Rows[m].FindControl("txtRemark")).Text = Remark;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                //                ShowULDAWBSummary();
                hdnManifestFlag.Value = "1";
                //Session[""] = (DataTable);
                btnSave_Click(null, null);
            }
            catch (Exception)
            { }
            finally
            {
                objExpManifest = null;
                objBAL = null;
                if (objDS != null)
                    objDS.Dispose();
                if (dsawbData != null)
                    dsawbData.Dispose();
                if (dsawb1 != null)
                    dsawb1.Dispose();
                //HighlightDataGridRows();
            }

            pnlGrid.Visible = false;

            AllButtonStatus(true);
        }

        #region Loadgrid Intial Row
        private void LoadAWBGrid()
        {

            DataTable myDataTable = new DataTable("Exp_Manifest_LoadAWBGrid_myDataTable");
            DataColumn myDataColumn;
            //DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AWBNo";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Pieces";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Weight";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AvlPCS";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AvlWgt";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "ULDNo";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "CartNumber";
            myDataTable.Columns.Add(myDataColumn);

            DataRow dr;
            dr = myDataTable.NewRow();
            //dr["RowNumber"] = 1;
            dr["AWBNo"] = "";//"5";
            dr["Pieces"] = "";// "5";
            dr["Weight"] = "";
            dr["AvlPCS"] = "";
            dr["AvlWgt"] = "";
            dr["ULDNo"] = "";
            dr["CartNumber"] = "";

            myDataTable.Rows.Add(dr);
            ViewState["CurrentTable11"] = myDataTable;
            //Bind the DataTable to the Grid

            grdAWBs.DataSource = null;
            grdAWBs.DataSource = myDataTable;
            grdAWBs.DataBind();
            try
            {
                ChangeButtonStatus();
            }
            catch (Exception)
            {

            }
            finally
            {
                if (myDataTable != null)
                    myDataTable.Dispose();
            }
        }
        #endregion

        public bool IsInputValid()
        {
            DataTable tabMenifestDetails = new DataTable("Exp_Manifest_IsInputValid_tabMenifestDetails");
            DataTable dtAWB = new DataTable("Exp_Manifest_IsInputValid_dtAWB");

            try
            {
                Label1.Text = "";
                int pcscount = 0;
                float Weight = 0;

                for (int i = 0; i < grdAWBs.Rows.Count; i++)
                {
                    try
                    {
                        pcscount = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text);
                    }
                    catch
                    {
                        Label1.Text = "Enter valid PCS row:" + (i + 1);
                        Label1.ForeColor = Color.Red;
                        ((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text = "";
                        return false;
                    }

                    try
                    {
                        Weight = float.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text);
                    }
                    catch
                    {
                        Label1.Text = "Enter valid Weight in row:" + (i + 1);
                        Label1.ForeColor = Color.Red;
                        ((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text = "";
                        return false;
                    }

                    try
                    {
                        if (((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text.Trim() == "")
                        {
                            Label1.Text = "Please Enter PCS count for row:" + (i + 1);
                            Label1.ForeColor = Color.Red;
                            return false;
                        }
                        if (((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text.Trim() == "")
                        {
                            Label1.Text = "Please Enter PCS count for row:" + (i + 1);
                            Label1.ForeColor = Color.Red;
                            return false;
                        }
                        if (int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text.Trim()) <= 0)
                        {
                            Label1.Text = "Please Enter valid PCS count for row:" + (i + 1);
                            Label1.ForeColor = Color.Red;
                            return false;
                        }
                        pcscount = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text);

                    }
                    catch
                    {
                        Label1.Text = "Enter valid pcscount row:" + i;
                        Label1.ForeColor = Color.Red;
                        ((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text = "";
                        return false;
                    }
                    string CheckSession = "";
                    CheckSession = Session["Split"].ToString();
                    if (CheckSession == "0" || CheckSession == "U")
                    {
                        tabMenifestDetails = (DataTable)Session["ManifestGridData"];
                        pcscount = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text);

                        try
                        {
                            for (int j = 0; j < tabMenifestDetails.Rows.Count; j++)
                            {
                                if (((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text == tabMenifestDetails.Rows[j][5].ToString())
                                {
                                    if (int.Parse(tabMenifestDetails.Rows[j]["PCS"].ToString()) < pcscount)
                                    {
                                        Label1.Text = "Enter valid pcscount row:" + (i + 1);
                                        Label1.ForeColor = Color.Red;
                                        ((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text = tabMenifestDetails.Rows[j]["PCS"].ToString();
                                        return false;
                                    }
                                    else
                                    {
                                        int AvlPCS = int.Parse(tabMenifestDetails.Rows[j]["PCS"].ToString());
                                        //  AvlPCS = int.Parse(dtAWB.Rows[j][1].ToString()) - pcscount;
                                        AvlPCS = int.Parse(tabMenifestDetails.Rows[j]["PCS"].ToString()) - pcscount;

                                        ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = AvlPCS.ToString();

                                        //  int AvlWeight = (int.Parse(dtAWB.Rows[j][2].ToString())) / (int.Parse(dtAWB.Rows[j][1].ToString()));
                                        int AvlWeight = int.Parse(tabMenifestDetails.Rows[i]["GrossWgt"].ToString()) / (int.Parse(tabMenifestDetails.Rows[j]["PCS"].ToString()));

                                        AvlWeight = AvlWeight * AvlPCS;
                                        ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = AvlWeight.ToString();
                                        //  int ActWeight = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text);
                                        int ActWeight = int.Parse(tabMenifestDetails.Rows[i]["GrossWgt"].ToString()) - AvlWeight;
                                        ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = ActWeight.ToString();
                                    }

                                }
                            }

                            //grdAWBs.r
                            //grdAWBs.DataBind();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (CheckSession == "A")
                    {
                        dtAWB = (DataTable)Session["AWBdata"];

                        pcscount = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text);

                        try
                        {
                            for (int j = 0; j < dtAWB.Rows.Count; j++)
                            {
                                if (((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text == dtAWB.Rows[j][0].ToString())
                                {
                                    if (int.Parse(dtAWB.Rows[j][1].ToString()) < pcscount)
                                    {
                                        Label1.Text = "Enter valid pcscount row:" + (i + 1);
                                        Label1.ForeColor = Color.Red;
                                        ((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text = dtAWB.Rows[j][1].ToString();
                                        return false;

                                    }
                                    else
                                    {
                                        int AvlPCS = int.Parse(dtAWB.Rows[j][1].ToString());//((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text));
                                        //  AvlPCS = int.Parse(dtAWB.Rows[j][1].ToString()) - pcscount;
                                        AvlPCS = int.Parse(dtAWB.Rows[j][1].ToString()) - pcscount;

                                        ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = AvlPCS.ToString();

                                        //  int AvlWeight = (int.Parse(dtAWB.Rows[j][2].ToString())) / (int.Parse(dtAWB.Rows[j][1].ToString()));
                                        int AvlWeight = (int.Parse(dtAWB.Rows[j][2].ToString())) / (int.Parse(dtAWB.Rows[j][1].ToString()));

                                        AvlWeight = AvlWeight * AvlPCS;
                                        ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = AvlWeight.ToString();
                                        //  int ActWeight = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text);
                                        int ActWeight = int.Parse(dtAWB.Rows[j][2].ToString()) - AvlWeight;
                                        ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = ActWeight.ToString();
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                Label1.Text = "";
                return true;
            }
            catch (Exception ex)
            {
                Label1.Text = "Error : while checking input validation.";
                Label1.ForeColor = Color.Red;
                return false;
            }
            finally
            {
                if (tabMenifestDetails != null)
                    tabMenifestDetails.Dispose();
                if (dtAWB != null)
                    dtAWB.Dispose();
            }
        }

        protected void btnShowEAWB_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                pnlGrid.Visible = false;
                grdRouting.DataSource = null;
                grdRouting.DataBind();
                LoadGridRoutingDetail();
                ddlReason.SelectedIndex = 0;
                txtReason.Text = string.Empty;
                txtReason.Enabled = true;
                AllButtonStatus(true);
            }
            catch (Exception ex)
            {
            }
        }

        public static string RemoveSpecialCharacters(string str)
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= '0' && str[i] <= '9') || (str[i] >= 'A' && str[i] <= 'z'))
                    sb.Append(str[i]);
            }

            return sb.ToString();
        }

        #region btnOK_Click
        protected void btnOK_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                if (txtEmailID.Text.Length < 1)
                {
                    lblStatus.Text = "Please Configure Mail to sent IATA Message";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (cls_BL.addMsgToOutBox(lblMsg.Text.ToString(), txtMessageBody.Text, "swapnil@qidtech.com", txtEmailID.Text.Trim()))
                {
                    lblStatus.Text = "Message Sent Successfully";
                    lblStatus.ForeColor = Color.Green;
                    return;
                }
                else
                {
                    lblStatus.Text = "Error in Message Sending. Please try Again";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex)
            { }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);

        }
        #endregion

        #region Send FFM
        protected void btnSendFFM_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            DataSet ds = new DataSet("Exp_Manifest_btnSendFFM_ds");
            BALEmailID ObjEmail = new BALEmailID();
            try
            {
                string FlightNumber = txtFlightCode.Text.ToString() + txtFlightID.Text.ToString();

                try
                {
                    ds = ObjEmail.GetEmail(lblDepAirport.Text, Session["Dest"].ToString(), "FFM", FlightNumber, txtFlightCode.Text.ToString());
                    txtEmailID.Text = ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
                    lblMsgCommType.Text = ds.Tables[0].Rows[0]["MsgCommType"].ToString();
                    if (lblMsgCommType.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) || lblMsgCommType.Text.Equals("SITA", StringComparison.OrdinalIgnoreCase))
                    {
                        GenerateSITAHeader(ds.Tables[0].Rows[0]["PartnerSITAiD"].ToString());
                    }
                }
                catch (Exception ex) { }
                ffmmsg = "ffm";
                lblMsg.Text = "FFM";
                txtMessageBody.Text = cls_BL.EncodeFFM(Session["POLairport"].ToString(), Session["FltNumber"].ToString(), Session["Fltdate"].ToString());
                Session["ffmmsg"] = "ffm";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
            }
            catch (Exception)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                ObjEmail = null;
            }
        }
        #endregion

        private void LoadAWBDataGrid(DataTable pi_objDataTable)
        {
            DataTable MemDetails = new DataTable("Exp_Manifest_LoadAWBDataGrid_MemDetails");
            try
            {
                MemDetails.Columns.Add("lblAWBno");
                MemDetails.Columns.Add("lblPieces");
                MemDetails.Columns.Add("lblWeight");
                MemDetails.Columns.Add("lblAvlPCS");
                //MemDetails.Columns.Add("lblCommodity");
                MemDetails.Columns.Add("lblAvlWgt");
                MemDetails.Columns.Add("lblTotPCS");
                MemDetails.Columns.Add("lblTotWgt");
                MemDetails.Columns.Add("lblFltFlag");
                MemDetails.Columns.Add("lblCartNumber");

                //MemDetails.Rows.Add(MemDetails.NewRow());
                for (int j = 0; j < pi_objDataTable.Rows.Count; j++)
                {
                    DataRow l_Datarow = MemDetails.NewRow();

                    MemDetails.Rows.Add(l_Datarow);
                }
                gdvULDLoadPlanAWB.DataSource = (DataTable)MemDetails;
                gdvULDLoadPlanAWB.DataBind();
                try
                {
                    ChangeButtonStatus();
                }
                catch (Exception)
                {


                }

                if (pi_objDataTable != null)
                {
                    for (int i = 0; i < pi_objDataTable.Rows.Count; i++)
                    {
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAWBno")).Text = pi_objDataTable.Rows[i][0].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblPieces")).Text = pi_objDataTable.Rows[i][1].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblWeight")).Text = pi_objDataTable.Rows[i][2].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlPCS")).Text = pi_objDataTable.Rows[i][3].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlWgt")).Text = pi_objDataTable.Rows[i][4].ToString();

                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotPCS")).Text = pi_objDataTable.Rows[i][6].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotWgt")).Text = pi_objDataTable.Rows[i][7].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblFltFlag")).Text = pi_objDataTable.Rows[i]["FltNo"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblCartNumber")).Text = pi_objDataTable.Rows[i]["CartNumber"].ToString();

                    }
                    Session["AWBdata"] = pi_objDataTable;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (MemDetails != null)
                    MemDetails.Dispose();
                if (pi_objDataTable != null)
                    pi_objDataTable.Dispose();
            }
        }

        private void LoadULDDataGrid(DataTable pi_objDataTable)
        {
            DataTable MemDetails = new DataTable("Exp_Manifest_LoadULDDataGrid_MemDetails");
            try
            {
                MemDetails.Columns.Add("ULDNo");
                MemDetails.Columns.Add("AWBCount");
                MemDetails.Columns.Add("PCS");
                MemDetails.Columns.Add("WGT");
                MemDetails.Columns.Add("FltNo");

                for (int j = 0; j < pi_objDataTable.Rows.Count; j++)
                {
                    DataRow l_Datarow = MemDetails.NewRow();

                    MemDetails.Rows.Add(l_Datarow);
                }
                gdvULDLoadPlan.DataSource = (DataTable)MemDetails;
                gdvULDLoadPlan.DataBind();

                try
                {
                    ChangeButtonStatus();
                }
                catch (Exception)
                {


                }

                if (pi_objDataTable != null)
                {
                    for (int i = 0; i < pi_objDataTable.Rows.Count; i++)
                    {
                        gdvULDLoadPlan.Rows[i].Cells[1].Text = pi_objDataTable.Rows[i][0].ToString();
                        gdvULDLoadPlan.Rows[i].Cells[2].Text = pi_objDataTable.Rows[i][1].ToString();
                        gdvULDLoadPlan.Rows[i].Cells[3].Text = pi_objDataTable.Rows[i][2].ToString();
                        gdvULDLoadPlan.Rows[i].Cells[4].Text = pi_objDataTable.Rows[i][3].ToString();
                        gdvULDLoadPlan.Rows[i].Cells[5].Text = pi_objDataTable.Rows[i][4].ToString();
                        if (pi_objDataTable.Rows[i][4].ToString().Trim() == "")
                            gdvULDLoadPlan.Rows[i].BackColor = CommonUtility.ColorHighlightedGrid;
                    }
                    //HighlightDataGridRows();

                    Session["ULDdata"] = pi_objDataTable;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (MemDetails != null)
                    MemDetails.Dispose();
                if (pi_objDataTable != null)
                    pi_objDataTable.Dispose();
            }
        }

        private void SplitAndUnAssignfunction()
        {
            DataTable dtULDDetails = new DataTable("Exp_Manifest_SplitAndUnAssignfucntion_dtULDDetails");
            dtULDDetails = (DataTable)Session["GDVULDDetails"];
            DataTable ManifestData = new DataTable("Exp_Manifest_SplitAndUnAssignfunction_ManifestData");
            ManifestData = (DataTable)Session["ManifestGridData"];
            DataTable DTAWBDetails = new DataTable("Exp_Manifest_SplitAndUnAssignfunction_DTAWBDetails");

            try
            {
                string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby = "";//, AWBDest = "";
                int PCS = 0, AVLPCS = 0;
                double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0;
                string CartNumber = string.Empty;
                //string Updatedon = "";
                //string LoadingPriority = "", Remark = "";

                FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                POL = Convert.ToString(Session["Station"]);
                //POU = ddlMainPOU.SelectedItem.Value.ToString();
                POU = ddlMainPOU.Items[0].Value.ToString();
                Updatedby = Convert.ToString(Session["Username"]);
                ULDno = "";
                ULDwgt = 0;


                PCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text);
                WGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text);
                //DataSet dsawb1 = objExpMani.GetAwbTabdetails(lblDepAirport.Text, FLTno);
                ULDno = ((Label)grdAWBs.Rows[0].FindControl("lblULDNO")).Text;
                DTAWBDetails = (DataTable)Session["AWBdata"];
                CartNumber = ((Label)grdAWBs.Rows[0].FindControl("lblCartNumber")).Text;

                if (DTAWBDetails == null)
                {
                    DTAWBDetails = new DataTable("Exp_Manifest_SplitAndUnAssignfunction_DTAWBDetails");

                    DTAWBDetails.Columns.Add("AWBNo");
                    DTAWBDetails.Columns.Add("Pieces");
                    DTAWBDetails.Columns.Add("Weight");
                    DTAWBDetails.Columns.Add("AvlPCS");
                    DTAWBDetails.Columns.Add("AvlWgt");

                    DTAWBDetails.Columns.Add("BookedPCS");
                    DTAWBDetails.Columns.Add("BookedWgt");
                    DTAWBDetails.Columns.Add("CartNumber");
                }

                DataRow drNew = null;
                for (int i = 0; i < grdAWBs.Rows.Count; i++)
                {
                    bool ismacth = false;
                    int actPCS = 0;
                    double Actwgt = 0;
                    int AWBPcs = 0;
                    double AWBWT = 0;


                    for (int k = 0; k < DTAWBDetails.Rows.Count; k++)
                    {
                        ///if (DTAWBDetails.Rows[k][6] == AWBno)
                        if (Convert.ToString(DTAWBDetails.Rows[k][0]) == ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text.Trim())
                        {
                            string PCScountshow = (Convert.ToInt32(DTAWBDetails.Rows[k][1]) + PCS).ToString();
                            string WGTCntShow = (double.Parse(DTAWBDetails.Rows[k][2].ToString()) + WGT).ToString();

                            DTAWBDetails.Rows[k][1] = PCScountshow;
                            DTAWBDetails.Rows[k][2] = WGTCntShow;

                            ismacth = true;

                        }
                    }
                    for (int t = 0; t < gdvULDDetails.Rows.Count; t++)
                    {
                        if (((Label)gdvULDDetails.Rows[t].FindControl("lblAWBno")).Text.Trim() == ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text.Trim() &&
                            ((Label)gdvULDDetails.Rows[t].FindControl("lblULDno")).Text.Trim() == ((Label)grdAWBs.Rows[i].FindControl("lblULDNO")).Text.Trim() &&
                            ((Label)gdvULDDetails.Rows[t].FindControl("lblCartNumber")).Text.Trim() == ((Label)grdAWBs.Rows[i].FindControl("lblCartNumber")).Text.Trim())
                        {
                            ((Label)gdvULDDetails.Rows[t].FindControl("lblMftPcs")).Text = (Convert.ToInt32(((Label)gdvULDDetails.Rows[t].FindControl("lblMftPcs")).Text) - PCS).ToString();
                            ((Label)gdvULDDetails.Rows[t].FindControl("lblMftWt")).Text = (double.Parse(((Label)gdvULDDetails.Rows[t].FindControl("lblMftWt")).Text) - WGT).ToString();

                            ManifestData.Rows[t][7] = ((Label)gdvULDDetails.Rows[t].FindControl("lblMftPcs")).Text;
                            ManifestData.Rows[t][8] = ((Label)gdvULDDetails.Rows[t].FindControl("lblMftWt")).Text;
                            actPCS = Convert.ToInt32(((Label)gdvULDDetails.Rows[t].FindControl("lblStatedPCS")).Text);
                            Actwgt = double.Parse(((Label)gdvULDDetails.Rows[t].FindControl("lblStatedWgt")).Text);

                            AWBPcs = Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblBookedPCS")).Text.Trim());
                            AWBWT = double.Parse(((Label)gdvULDDetails.Rows[i].FindControl("lblBookedWgt")).Text.Trim());
                        }

                        DTAWBDetails.AcceptChanges();
                    }

                    if (ismacth == false)
                    {//Added for Not match AWB in datatable on 2 oct
                        drNew = DTAWBDetails.NewRow();
                        DTAWBDetails.Rows.Add(drNew);

                        int count = DTAWBDetails.Rows.Count;

                        AWBno = ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text;

                        PCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text);
                        WGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text);

                        DTAWBDetails.Rows[count - 1][0] = AWBno;
                        DTAWBDetails.Rows[count - 1][1] = PCS;
                        DTAWBDetails.Rows[count - 1][2] = WGT;
                        DTAWBDetails.Rows[count - 1][3] = actPCS;

                        DTAWBDetails.Rows[count - 1][4] = Actwgt;
                        DTAWBDetails.Rows[count - 1][6] = AWBPcs;
                        DTAWBDetails.Rows[count - 1][7] = AWBWT;
                        DTAWBDetails.Rows[count - 1]["CartNumber"] = CartNumber;
                        // DTAWBDetails.Rows[count - 1][13] = ;
                    }

                    LoadAWBDataGrid(DTAWBDetails);

                    //for (int l = 0; l < gdvULDDetails.Rows.Count; l++)
                    //{
                    for (int l = 0; l < ManifestData.Rows.Count; l++)
                    {
                        if (((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text.Trim() == ((Label)gdvULDDetails.Rows[l].FindControl("lblAWBno")).Text.Trim()
                            && ((Label)grdAWBs.Rows[i].FindControl("lblULDNO")).Text.Trim() == ((Label)gdvULDDetails.Rows[l].FindControl("lblULDno")).Text.Trim()
                            && ((Label)grdAWBs.Rows[i].FindControl("lblCartNumber")).Text.Trim() == ((Label)gdvULDDetails.Rows[l].FindControl("lblCartNumber")).Text.Trim())
                        {
                            int UnassignPCS = Convert.ToInt32(((Label)gdvULDDetails.Rows[l].FindControl("lblMftPcs")).Text);

                            if (UnassignPCS <= 0)
                            {
                                ManifestData.Rows[l].Delete();
                            }
                        }
                    }

                    ManifestData.AcceptChanges();

                    Session["ManifestGridData"] = ManifestData;

                    gdvULDDetails.DataSource = ManifestData;
                    gdvULDDetails.DataBind();
                    try
                    {
                        ChangeButtonStatus();
                    }
                    catch (Exception)
                    {


                    }
                }

                //ShowULDAWBSummary();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (dtULDDetails != null)
                    dtULDDetails.Dispose();
                if (ManifestData != null)
                    ManifestData.Dispose();
                if (DTAWBDetails != null)
                    DTAWBDetails.Dispose();
            }
            pnlGrid.Visible = false;
        }

        protected void btnOffload_Click(object sender, EventArgs e)
        {
            if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                lblDepAirport.Text) == "D")
            {
                BtnList_Click(this, new EventArgs());
                lblStatus.Text = "Flight is departed. Please reopen flight.";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            lblStatus.Text = "";
            DataTable dt = new DataTable("Exp_Manifest_btnOffload_dt");
            DataTable DTAWBDetails = new DataTable("Exp_Manifest_btnOffload_DTAWBDetails");
            DataTable dt1 = new DataTable("Exp_Manifest_btnOffload_dt1");
            DataTable dtCurrentTable = new DataTable("Exp_Manifest_btnOffload_dtCurrentTable");
            string AWBNo = string.Empty;

            try
            {
                ddlReason.Visible = true;
                int check = 0;
                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
                    {
                        check = check + 1;
                    }

                }
                for (int k = 0; k < gdvULDDetails.Rows.Count; k++)
                {
                    string txt = ((Label)gdvULDDetails.Rows[k].FindControl("lblManifested")).Text;
                    if (((CheckBox)gdvULDDetails.Rows[k].FindControl("Check0")).Checked == true)
                    {
                        if (!bool.Parse(((Label)gdvULDDetails.Rows[k].FindControl("lblManifested")).Text))
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "This AWB is not Manifested so we can't Offload this AWB (PCS)";
                            return;
                        }
                    }

                }

                if (check == 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select One ULD from Manifested TAB for Offload";
                    return;
                }
                //if (check > 1)
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please Select only One ULD from Manifest TAB for Offload";
                //    return;
                //}

                //Added by Poorna
                Session["Split"] = "O";
                btnAddManifest.Text = "Save";

                LoadAirlineCode("");
                UpdatePartnerCode(1);
                # region Code to get Offload reasons

                DataSet ds = new DataSet("Exp_Manifest_btnOffload_ds");
                ds = objExpMani.GetOffloadReasons();
                {
                    try
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                txtReason.Enabled = false;
                            }
                            else
                            {
                                txtReason.Enabled = true;
                            }
                        }
                        else
                        {
                            txtReason.Enabled = true;
                        }
                        //ddlReason.DataTextField = "Reason";
                        //ds.Tables[0].Rows.Add("Others");
                        //ddlReason.DataSource = ds.Tables[0];
                       //ds.Tables[0].Rows.Add("Others");
                        ddlReason.DataSource = ds.Tables[0];
                        ddlReason.DataTextField = "Reason";
                        ddlReason.DataValueField = "ReasonCode";
                        ddlReason.DataBind();
                        ddlReason.SelectedIndex = 0;
                        txtReason.Enabled = true;
                    }
                    catch (Exception ex) { }
                }
                #endregion


                lblReason.Visible = true;
                //   txtReason.Visible = true;
                grdAWBs.Columns[2].Visible = false;
                grdAWBs.Columns[3].Visible = false;
                grdAWBs.Columns[5].Visible = true;
                grdAWBs.Columns[6].Visible = true;

                DTAWBDetails.Columns.Add("AWBNo");
                DTAWBDetails.Columns.Add("Pieces");
                DTAWBDetails.Columns.Add("Weight");
                DTAWBDetails.Columns.Add("AvlPCS");
                DTAWBDetails.Columns.Add("AvlWgt");
                DTAWBDetails.Columns.Add("Origin");
                DTAWBDetails.Columns.Add("Destination");
                DTAWBDetails.Columns.Add("ULDNo");
                DTAWBDetails.Columns.Add("CartNumber");
                string OffloadPsc = "";
                string OffloadWgt = "";
              
                string ULDNumber = "";
                string CartNumber = "";
                for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[j].FindControl("Check0")).Checked == true)
                    {
                        DataRow dr;
                      
                        dr = DTAWBDetails.NewRow();
                        //string POL = gdvULD-Details.Rows[j].Cells[13].Text; Commented by poorna to change POL from AWB Origin to AWB Flight Origin
                        //string POU = gdvULDDetails.Rows[j].Cells[14].Text; Commented by poorna to change POU from AWB Dest to AWB Flight Dest
                        string POL = ((Label)gdvULDDetails.Rows[j].FindControl("lblPOL")).Text.Trim();
                        string POU = (((Label)gdvULDDetails.Rows[j].FindControl("lblPOL")).Text.Trim() == ((Label)gdvULDDetails.Rows[j].FindControl("lblOrigin")).Text.Trim()) ? ((Label)gdvULDDetails.Rows[j].FindControl("lblAWBDest")).Text.Trim() : ((Label)gdvULDDetails.Rows[j].FindControl("lblPOU")).Text.Trim();
                        AWBNo = ((Label)gdvULDDetails.Rows[j].FindControl("lblAWBno")).Text.Trim();

                        dr[0] = ((Label)gdvULDDetails.Rows[j].FindControl("lblAWBno")).Text;
                        dr[1] = ((Label)gdvULDDetails.Rows[j].FindControl("lblMftPcs")).Text;
                        dr[2] = ((Label)gdvULDDetails.Rows[j].FindControl("lblMftWt")).Text;
                        dr[3] = ((Label)gdvULDDetails.Rows[j].FindControl("lblMftPcs")).Text;
                        dr[4] = ((Label)gdvULDDetails.Rows[j].FindControl("lblMftWt")).Text;

                        dr[7] = ((Label)gdvULDDetails.Rows[j].FindControl("lblULDno")).Text;
                        dr[8] = ((Label)gdvULDDetails.Rows[j].FindControl("lblCartNumber")).Text;

                        ULDNumber = ((Label)gdvULDDetails.Rows[j].FindControl("lblULDno")).Text.Trim();
                        CartNumber = ((Label)gdvULDDetails.Rows[j].FindControl("lblCartNumber")).Text.Trim();
                        OffloadPsc = ((Label)gdvULDDetails.Rows[j].FindControl("lblMftPcs")).Text;
                         OffloadWgt = ((Label)gdvULDDetails.Rows[j].FindControl("lblMftWt")).Text;
              
                        if (Session["dsMani"] != null)
                        {
                            DataSet dsManifest = new DataSet("Exp_Manifest_btnOffload_dsManifest");
                            dsManifest = (DataSet)Session["dsMani"];
                            if (dsManifest.Tables[2].Rows.Count > 0)
                            {
                                for (int k = 0; k < dsManifest.Tables[2].Rows.Count; k++)
                                {
                                    if (dsManifest.Tables[2].Rows[k]["AWBNumber"].ToString() == AWBNo)
                                    {
                                        POL = dsManifest.Tables[2].Rows[k]["POL"].ToString();
                                        POU = dsManifest.Tables[2].Rows[k]["POU"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        dr[5] = POL;
                        dr[6] = POU;
                        DTAWBDetails.Rows.Add(dr);
                        //break;
                }
                }

                Session["AWBdata"] = DTAWBDetails;

                txtReason.Text = "";

                string AWBNumber = string.Empty, AWBPrefix = string.Empty;
                string FlightNo = txtFlightCode.Text + txtFlightID.Text;
                string FlightDate = TextBoxdate.Text;
                
                  AWBPrefix = AWBNo.Substring(0, AWBNo.IndexOf("-"));
                AWBNumber = AWBNo.Substring(AWBNo.IndexOf("-") + 1, 8);
                BookingBAL objBLL = new BookingBAL();
                objBLL.CheckforFinalStatusNew(AWBNumber, AWBPrefix, FlightNo, FlightDate, OffloadPsc, OffloadWgt);
                objBLL = null;
                //{
                //    lblStatus.Text = "AWB number is finalized in billing. Offloading can not be done.";
                //    lblStatus.ForeColor = Color.Red;
                //    return;
                //}
                //Verify if AWB is manifested for 2nd leg.---
                BLExpManifest objExpManifest = new BLExpManifest();
                string res = "";
                objExpMani.IsNextLegReOpenedforOffload(AWBNumber, AWBPrefix, txtFlightCode.Text + txtFlightID.Text, 
                    DateTime.ParseExact(TextBoxdate.Text,"dd/MM/yyyy",null), ULDNumber, CartNumber, out res);
                if (res != null && res != "")
                {
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                pnlGrid.Visible = true;
                UpdatePanelRouteDetails.Visible = true;

                if (UpdatePanelRouteDetails.Visible == true)
                {
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text = "";
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text = "";
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text = "";

                        if ((DataTable)Session["AWBdata"] != null && ((DataTable)Session["AWBdata"]).Rows.Count > 1)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowOffloadPopup('O');</SCRIPT>", false);
                            pnlGrid.Visible = false;
                            return;
                        }
                    }
                }
                // pnlGrid.Attributes.item("style") = "Z-INDEX: 176; LEFT: 584px; POSITION: absolute; TOP: 176px";
                pnlGrid.Style["Z-INDEX"] = "150";
                pnlGrid.Style["LEFT"] = "342px";
                pnlGrid.Style["POSITION"] = "absolute";
                pnlGrid.Style["TOP"] = "170px";
                pnlGrid.Style["Width"] = "700px";


                try
                {
                    LoadAWBGrid();
                    dt1 = DTAWBDetails;
                    dtCurrentTable = (DataTable)Session["AWBdata"];
                    grdAWBs.DataSource = dtCurrentTable;
                    grdAWBs.DataSource = dtCurrentTable;
                    grdAWBs.DataBind();

                    

                    if (dt1.Rows.Count > 0)
                    {
                        DataRow drCurrentRow = null;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            drCurrentRow = dtCurrentTable.NewRow();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text = dt1.Rows[i][0].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text = dt1.Rows[i][1].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = dt1.Rows[i][2].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = dt1.Rows[i][3].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = dt1.Rows[i][4].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtOrigin")).Text = dt1.Rows[i][5].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtDestination")).Text = dt1.Rows[i][6].ToString();
                            ((Label)grdAWBs.Rows[i].FindControl("lblULDNO")).Text = dt1.Rows[i][7].ToString();
                            ((Label)grdAWBs.Rows[i].FindControl("lblCartNumber")).Text = dt1.Rows[i]["CartNumber"].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Enabled = false;

                            ViewState["CurrentTable1"] = dtCurrentTable;
                        }
                    }

                    btnOffload.Attributes.Add("onClick", "showDiv('toggle');");
                }
                catch (Exception ex)
                {

                }
                BtnList_Click(null, null);
                AllButtonStatus(false);
                CheckPartnerFlightSupport();
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                if (DTAWBDetails != null)
                    DTAWBDetails.Dispose();
                if (dt1 != null)
                    dt1.Dispose();
                if (dtCurrentTable != null)
                    dtCurrentTable.Dispose();
            }
        }

        private void SaveAddToShipper()
        {
            BLExpManifest objExpManifest = new BLExpManifest();
            try
            {
                string AWBno = "", POU = "", POL = "", ActFLTno = "", Updatedby = "", OffLoadFltNo = "", OffloadLoc = "";
                int OffloadPCS = 0, AVLPCS = 0;
                double OffloadWGT = 0.0, AVLWGT = 0.0;
                DateTime dtOffLoadDate;
                string strMode = string.Empty;

                ActFLTno = txtFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();
                POL = Convert.ToString(Session["Station"]);
                // POU = ddlMainPOU.SelectedItem.Value.ToString();
                POU = ddlMainPOU.Items[0].Value.ToString();
                Updatedby = Convert.ToString(Session["Username"]);
                OffLoadFltNo = txtNextFlight.Text.ToUpper().Trim();
                OffloadLoc = Convert.ToString(Session["Station"]);

                dtOffLoadDate = dtCurrentDate;

                //For Arrival Date 
                for (int i = 0; i < grdAWBs.Rows.Count; i++)
                {
                    AWBno = ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text;
                    OffloadPCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text);
                    OffloadWGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text);
                    AVLPCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text);
                    AVLWGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text);
                }

                for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                {
                    //  if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
                    if (((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text == AWBno)
                    {
                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                        {
                            Label1.Text = "Please enter valid Pcs to return to Shipper";
                            Label1.ForeColor = Color.Red;
                            return;
                        }
                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                        {
                            Label1.Text = "Please enter valid Pcs to return to Shipper";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                        {
                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) < Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                            {
                                Label1.Text = "Please enter valid Weight";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                        }

                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) == Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                        {
                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                            {
                                Label1.Text = "Please enter valid Pcs to return to Shipper";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                        }
                        break;
                    }
                }

                AWBno = AWBno.Substring(AWBno.Length - 8);

                //Changing Manifest pcs and manifest wt
                for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                {
                    if (((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim() == ((Label)gdvULDDetails.Rows[j].FindControl("lblAWBno")).Text.Trim())
                    {
                        AVLPCS = Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblStatedPCS")).Text);
                        AVLWGT = Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblStatedWgt")).Text);
                    }
                }

                bool blnResult = objExpManifest.ReturnToShipper(ActFLTno, ActFLTno, OffloadLoc, AWBno, AVLPCS, AVLWGT, OffloadPCS, OffloadWGT,
                    Updatedby, POL, POU, ddlVersion.Text.ToString(), dtOffLoadDate, txtReason.Text.Trim(), strMode);

                if (blnResult)
                {
                    pnlGrid.Visible = false;
                    BtnList_Click(null, null);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                objExpManifest = null;
            }
        }

        private void SaveOffLoadDetails()
        {
            BLExpManifest objExpManifest = new BLExpManifest();
            CustomsImportBAL objCustoms = new CustomsImportBAL();
            DataSet dCust = new DataSet("Exp_Manifest_SaveOffLoadDetails_dCust");
            try
            {
                string AWBno = "", POU = "", POL = "", ActFLTno = "", Updatedby = "", OffLoadFltNo = "", OffloadLoc = "";
                int OffloadPCS = 0, AVLPCS = 0;
                double OffloadWGT = 0.0, AVLWGT = 0.0;
                DateTime dtOffLoadDate = new DateTime();
                string strMode = string.Empty;

                ActFLTno = txtFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();
                strMode = (string)Session["Split"];

                if (strMode != "R")
                {
                    if (UpdatePanelRouteDetails.Visible == true)
                    {
                        int chkPCS = 0;
                        double chkWGT = 0.0;
                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == "")
                            {
                                Label1.Text = "Please enter valid Origin";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim() == "")
                            {
                                Label1.Text = "Please enter valid Destination";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim())
                            {
                                Label1.Text = "Please enter valid Origin & Destination";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            string Org = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                            if (Org != lblDepAirport.Text)
                            {
                                Label1.Text = "Please enter valid Origin";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim().ToUpper() != ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim().ToUpper())
                            {
                                Label1.Text = "Please enter valid Destination";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == null)
                            {
                                Label1.Text = "Please enter valid PCS";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == null)
                            {
                                Label1.Text = "Please enter valid PCS";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == null)
                            {
                                Label1.Text = "Please enter valid Weight ";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (lblDepAirport.Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim())
                            {
                                chkPCS = chkPCS + Convert.ToInt32(((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim());
                                chkWGT = chkWGT + Convert.ToDouble(((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim());
                            }

                            if (chkPCS > Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text))
                            {
                                Label1.Text = "Please enter valid PCS";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (chkWGT > Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text))
                            {
                                Label1.Text = "Please enter valid Weight";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (chkPCS == 0)
                            {
                                Label1.Text = "Please enter valid PCS";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (chkWGT == 0)
                            {
                                Label1.Text = "Please enter valid Weight ";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text != "" 
                                && ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
                            {
                                Label1.ForeColor = Color.Red;
                                Label1.Text = "Please enter Next Flight Details.";
                                return;
                            }

                            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text == "" &&
                                ((TextBox)grdRouting.Rows[i].FindControl("txtRouteLocation")).Text.Trim() == "")
                            {
                                Label1.ForeColor = Color.Red;
                                Label1.Text = "Please select either Flight # OR enter Location.";
                                return;
                            }
                            //Set flight date to current day if not selected.
                            if (((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
                            {
                                ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text =
                                    Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                            }

                            if (ddlReason.SelectedValue == "Others" && txtReason.Text == "")
                            {
                                Label1.Text = "Please enter Offload reason";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                        }
                    }
                }
                
                string ULDno = "";
                string AWBPrefix = string.Empty, Location = string.Empty, CartNumber = string.Empty;

                //For Arrival Date 
                for (int i = 0; i < grdAWBs.Rows.Count; i++)
                {
                    string Number = ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text.Trim();

                    if (Number.Replace("&nbsp;", "") != "")
                    {
                        AWBPrefix = Number.Substring(0, Number.IndexOf('-'));
                        AWBno = Number.Substring(Number.IndexOf('-') + 1, 8);
                    }
                    else
                    {
                        AWBPrefix = "";
                        AWBno = "";
                    }

                    AVLPCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text);
                    AVLWGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text);
                    ULDno = ((Label)grdAWBs.Rows[i].FindControl("lblULDNo")).Text;
                    CartNumber = ((Label)grdAWBs.Rows[i].FindControl("lblCartNumber")).Text;
                }

                //Changing Manifest pcs and manifest wt
                for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                {
                    if (((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim() == 
                        ((Label)gdvULDDetails.Rows[j].FindControl("lblAWBno")).Text.Trim() &&
                        ((Label)grdAWBs.Rows[0].FindControl("lblCartNumber")).Text.Trim() ==
                        ((Label)gdvULDDetails.Rows[j].FindControl("lblCartNumber")).Text.Trim())
                    {
                        AVLPCS = Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblStatedPCS")).Text);
                        AVLWGT = Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblStatedWgt")).Text);
                    }
                }

                DateTime dtActualDate = DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null);

                bool blnResult = false;

                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    POL = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
                    //POU = ddlMainPOU.SelectedItem.Value.ToString();
                    POU = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text; ;
                    Location = ((TextBox)grdRouting.Rows[i].FindControl("txtRouteLocation")).Text; ;
                    Updatedby = Convert.ToString(Session["Username"]);

                    OffLoadFltNo = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                    if (OffLoadFltNo == "Select")
                        OffLoadFltNo = "";

                    OffloadLoc = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
                    string strDate = ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text;
                    //Checks Nextflightdate is blank or not
                    //Swapnil--------------------------------------------------------
                    HomeBL objHome = new HomeBL();
                    int RoleId = Convert.ToInt32(Session["RoleID"]);
                    DataSet objDS = new DataSet("Exp_Manifest_SaveOffLoadDetails_objDS");
                    objDS = objHome.GetUserPermissions(((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath, RoleId);
                    objHome = null;

                    if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < objDS.Tables[0].Rows.Count; j++)
                        {
                            if (objDS.Tables[0].Rows[j]["ControlId"].ToString() == "OffloadForSuperUser")
                            {
                                ChkSuperUserOffload = true;
                            }
                        }
                    }
                    objDS = null;

                    //----------------------------------------------------------------

                    if (ChkSuperUserOffload == false)
                    {
                        if (strDate != "")
                        {
                            if (DateTime.ParseExact(((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text, "dd/MM/yyyy", null) < dtCurrentDate.Date.AddDays(-2))
                            {
                                Label1.ForeColor = Color.Red;
                                Label1.Text = "We can't assign Offloaded PCS to previous day flight";
                                return;
                            }
                        }
                    }

                    if (OffLoadFltNo == "" || strDate == "")
                        dtOffLoadDate = DateTime.ParseExact(Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                    //dtOffLoadDate = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
                    else
                        dtOffLoadDate = DateTime.ParseExact(((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text, "dd/MM/yyyy", null);

                    if (strDate.Trim() == "" && Location.Trim() == "")
                    {
                        Label1.ForeColor = Color.Red;
                        Label1.Text = "Please specify the valid location.";
                        return;
                    }

                    if (lblDepAirport.Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim())
                    {
                        OffloadPCS = OffloadPCS + Convert.ToInt32(((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim());
                        OffloadWGT = OffloadWGT + Convert.ToDouble(((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim());
                    }

                    string Reason = "";
                    if (ddlReason.SelectedItem.Text.Trim() == "Select")
                    {
                        lblStatus.Text = "Please select Reason";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    if (txtReason.Text.Trim() != "")
                    {
                        Reason = txtReason.Text.Trim();
                    }
                    else
                    {
                        Reason = ddlReason.SelectedItem.Text;//ddlReason.SelectedValue;
                    }

                    string PartnerCode = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedValue;
                    string Partnertype = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedValue;

                    if (PartnerCode == "Other")
                        OffLoadFltNo = ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text;

                    //Code Added to Check if the AWB is Valid for Customs Messaging
                    object[] QueryCheck = { AWBPrefix + "-" + AWBno, ActFLTno, dtActualDate.ToString("dd/MM/yyyy") };
                    dCust = objCustoms.CheckCustomsAWBAvailability(QueryCheck);

                    blnResult = objExpManifest.OffLoadShipmentinManifestForGHA(ActFLTno, OffLoadFltNo, OffloadLoc, AWBno, AVLPCS, AVLWGT, OffloadPCS, OffloadWGT,
                       Updatedby, POL, POU, ddlVersion.Text.ToString(), dtOffLoadDate, Reason, strMode, dtActualDate, dtCurrentDate, PartnerCode, Partnertype, ULDno,
                       Convert.ToDateTime(Session["IT"]), AWBPrefix, Location, "", CartNumber);

                }
                if (blnResult)
                {
                    //Code Added by Deepak for Customs Messaging
                    try
                    {

                        object[] QueryValues = new object[3];

                        QueryValues[0] = AWBPrefix + "-" + AWBno;
                        QueryValues[1] = ActFLTno;
                        QueryValues[2] = TextBoxdate.Text;//dtOffLoadDate.ToString("dd/MM/yyyy");
                        string[] AWBDetails = AWBno.Split('-');
                        if (dCust != null)
                        {
                            if (dCust.Tables[0].Rows[0]["Validate"].ToString() == "True" && dCust.Tables[1].Rows[0]["Validate"].ToString() == "True")
                            {
                                StringBuilder sb = objCustoms.EncodingFRCMessage(QueryValues);
                                object[] QueryVal = { QueryValues[0], QueryValues[1], QueryValues[2], sb.ToString().ToUpper() };
                                if (objCustoms.UpdateFRCMessageOffload(QueryVal))
                                {
                                    if (sb != null)
                                    {
                                        if (sb.ToString() != "")
                                        {
                                            object[] QueryValMail = { "FRC", ActFLTno, TextBoxdate.Text };
                                            //Getting MailID for FRC Message
                                            DataSet dMail = new DataSet("Exp_Manifest_SaveOffLoadDetails_dMail");
                                            dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                            string MailID = string.Empty;
                                            if (dMail != null)
                                            {
                                                MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                            }
                                            cls_BL.addMsgToOutBox("FRC", sb.ToString().ToUpper(), "", MailID);
                                        }
                                    }
                                    DataSet dFRX = new DataSet("Exp_Manifest_SaveOffLoadDetails_dFRX");
                                    dFRX = objCustoms.CheckFRXValidityOffload(QueryValues);
                                    if (dFRX != null)
                                    {
                                        if (dFRX.Tables[0].Rows.Count > 0)
                                        {
                                            if (dFRX.Tables[0].Rows[0]["Validate"].ToString() == "True")
                                            {
                                                StringBuilder sbFRX = objCustoms.EncodingFRXMessage(QueryValues);
                                                object[] QueryValFRX = { QueryValues[0], QueryValues[1], QueryValues[2], sbFRX.ToString().ToUpper() };
                                                if (objCustoms.UpdateFRXMessageOffload(QueryValFRX))
                                                {
                                                    if (sb != null)
                                                    {
                                                        if (sb.ToString() != "")
                                                        {
                                                            object[] QueryValMail = { "FRX", ActFLTno, TextBoxdate.Text };
                                                            //Getting MailID for FRX Message
                                                            DataSet dMail = new DataSet("Exp_Manifest_SaveOffLoadDetails_dMail");
                                                            dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                                            string MailID = string.Empty;
                                                            if (dMail != null)
                                                            {
                                                                MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                            }
                                                            cls_BL.addMsgToOutBox("FRX", sbFRX.ToString().ToUpper(), "", MailID);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        objCustoms = null;
                        dCust.Dispose();


                    }
                    catch (Exception)
                    { }
                    pnlGrid.Visible = false;
                    //BtnList_Click(null, null);
                    btnSave_Click(this, new EventArgs());
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                objExpManifest = null;
            }
        }

        protected void btnDepartFit_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                    lblDepAirport.Text) == "D")
            {
                BtnList_Click(this, new EventArgs());
                lblStatus.Text = "Flight is already departed.";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            if (Session["IsManifested"] != null && (bool)Session["IsManifested"] == false)
            {

                if (txtTailNo.Text == "")
                {
                    lblStatus.Text = "Please enter Tail Number";
                    lblStatus.ForeColor = Color.Red;
                    txtTailNo.Focus();
                    return;
                }
                UpdateDepartedFlight("D");

                btnSave.Enabled = false;
                btnDepartFit.Enabled = false;
                btnReopenFit.Enabled = true;
                btnOffload.Enabled = false;
                BtnAddtoManifest.Enabled = false;
                btnSplitAssign.Enabled = false;
                btnFetchClear.Enabled = false;
                btnFetchAWB.Enabled = false;
                btnAssignFetchedAWB.Enabled = false;

                bool res = false;

                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    //if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
                    //{
                    string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby, counter = "", SCC = "", Desc = "";
                    int PCS = 0, AVLPCS = 0;
                    double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, VOL = 0;
                    string Updatedon = "";
                    string LoadingPriority = "", Remark = "", ULDDest = "";

                    FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                    Updatedby = Convert.ToString(Session["Username"]);

                    ULDno = ((Label)gdvULDDetails.Rows[i].FindControl("lblULDno")).Text;
                    POU = ((Label)gdvULDDetails.Rows[i].FindControl("lblPOU")).Text;
                    POL = ((Label)gdvULDDetails.Rows[i].FindControl("lblPOL")).Text;
                    ULDDest = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBDest")).Text;
                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblcounter")).Text == "&nbsp;")
                    {
                        counter = "0";
                    }
                    else
                    {
                        counter = ((Label)gdvULDDetails.Rows[i].FindControl("lblcounter")).Text;
                    }
                    //AWBno = gdvULDDetails.Rows[i].Cells[6].Text.Trim();

                    //string AWBPrefix = gdvULDDetails.Rows[i].Cells[6].Text.Trim().Replace(AWBno, "");

                    string Number = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text.Trim();

                    string AWBPrefix = string.Empty;

                    if (Number.Replace("&nbsp;", "") != "")
                    {
                        AWBPrefix = Number.Substring(0, Number.IndexOf('-'));
                        AWBno = Number.Substring(Number.IndexOf('-') + 1, 8);
                    }
                    else
                    {
                        AWBPrefix = "";
                        AWBno = "";
                    }

                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblSCC")).Text == "&nbsp;")
                    {
                        SCC = "0";
                    }
                    else
                    {
                        SCC = ((Label)gdvULDDetails.Rows[i].FindControl("lblSCC")).Text;
                    }

                    PCS = Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblMftPcs")).Text);
                    WGT = Convert.ToDouble(((Label)gdvULDDetails.Rows[i].FindControl("lblMftWt")).Text);
                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblVOL")).Text.Trim() == "&nbsp;" || ((Label)gdvULDDetails.Rows[i].FindControl("lblVOL")).Text.Trim() == "")
                    {
                        VOL = 0;
                    }
                    else
                    {
                        VOL = Convert.ToDouble(((Label)gdvULDDetails.Rows[i].FindControl("lblVOL")).Text);
                    }
                    AVLPCS = Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblStatedPCS")).Text);
                    AVLWGT = Convert.ToDouble(((Label)gdvULDDetails.Rows[i].FindControl("lblStatedWgt")).Text);
                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblCommDesc")).Text == "&nbsp;")
                    {
                        Desc = "";
                    }
                    else
                    {
                        Desc = ((Label)gdvULDDetails.Rows[i].FindControl("lblCommDesc")).Text;
                    }

                    if (((DropDownList)gdvULDDetails.Rows[i].FindControl("ddlLoadingPriority")).Text == "&nbsp;")
                    {
                        LoadingPriority = "";
                    }
                    else
                    {
                        LoadingPriority = ((DropDownList)gdvULDDetails.Rows[i].FindControl("ddlLoadingPriority")).SelectedItem.Text.ToString();
                    }

                    bool IsManifest = false;
                    if ((string)Session["IsDeparted"] == "D")
                    {
                        IsManifest = true;
                    }

                    if (((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text.Trim() == "&nbsp;")
                    {
                        Remark = "";
                    }
                    else
                    {
                        Remark = ((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text.ToString();
                    }

                    string CartNumber = ((Label)gdvULDDetails.Rows[i].FindControl("lblCartNumber")).Text.ToString();

                    // Updatedon = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    Updatedon = Convert.ToDateTime(Session["IT"]).ToString();//DateTime.Now.ToString("yyyy/MM/dd");
                    DateTime dtflightdate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);
                    res = objExpMani.CommitManifestdata(FLTno, ULDno, POU, POL, ULDDest, counter, AWBno, SCC, VOL, PCS, WGT, AVLPCS, AVLWGT, Desc, LoadingPriority, Remark, Updatedby, Updatedon, dtflightdate, AWBPrefix, CartNumber);
                    if (res == true)
                    {
                        try
                        {
                            CustomsImportBAL objCustoms = new CustomsImportBAL();
                            object[] QueryValues = new object[8];


                            QueryValues[0] = AWBno;
                            QueryValues[1] = FLTno.Substring(0, 2);
                            QueryValues[2] = FLTno.Substring(2, FLTno.Length - 2);
                            QueryValues[3] = ((DateTime)Session["IT"]).ToString();
                            QueryValues[4] = Session["UserName"].ToString();
                            QueryValues[5] = AWBPrefix;
                            QueryValues[6] = dtflightdate;
                            object[] QueryVal = { AWBPrefix + "-" + AWBno, FLTno, TextBoxdate.Text };
                            DataSet dCust = new DataSet("Exp_Manifest_btnDepartFlt_dCust");
                            dCust = objCustoms.CheckCustomsAWBAvailability(QueryVal);
                            if (dCust != null)
                            {
                                if (dCust.Tables[0].Rows[0]["Validate"].ToString() == "True" && dCust.Tables[1].Rows[0]["Validate"].ToString() == "True")
                                {
                                    #region ACAS PRI Message Automation Deepak(15/04/2014)
                                    try
                                    {
                                        ACASBAL objACAS = new ACASBAL();
                                        //LoginBL objLogin = new LoginBL();
                                        //if (Convert.ToBoolean(objLogin.GetMasterConfiguration("ACASAutomation")))
                                        if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ACASAutomation")))
                                        {
                                            if (objACAS.ACASFRITriggerPointValidation() == "DP")
                                            {


                                                StringBuilder sbPRI = objACAS.EncodingPRIMessage(QueryVal);

                                                object[] QueryValPRI = { AWBPrefix + "-" + AWBno, 1, FLTno, TextBoxdate.Text, sbPRI.ToString().ToUpper() };

                                                if (objACAS.UpdatePRIMessage(QueryValPRI))
                                                {
                                                    if (sbPRI != null)
                                                    {
                                                        if (sbPRI.ToString() != "")
                                                        {
                                                            object[] QueryValMail = { "PRI", FLTno, TextBoxdate.Text };
                                                            //Getting MailID for PRI Message
                                                            DataSet dMail = new DataSet("Exp_Manifest_btnDepartFlt_dMail");
                                                            dMail = objACAS.GetCustomMessagesMailID(QueryValMail);
                                                            string MailID = string.Empty;
                                                            if (dMail != null)
                                                            {
                                                                MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                            }
                                                            cls_BL.addMsgToOutBox("PRI", sbPRI.ToString().ToUpper(), "", MailID);
                                                        }
                                                    }
                                                }



                                            }
                                        }
                                    }
                                    catch (Exception Ex)
                                    { }

                                    #endregion

                                    StringBuilder sbFDM = objCustoms.EncodingFDMMessage(QueryVal);
                                    QueryValues[7] = sbFDM.ToString().ToUpper();
                                    objCustoms.UpdateFDMMessage(QueryValues);

                                    //StringBuilder sbFRI = objCustoms.EncodingFRIMessage(QueryVal);
                                    //object[] QueryValFRI = { AWBPrefix + AWBno, 1, FLTno, TextBoxdate.Text,sbFRI.ToString().ToUpper() };

                                    //objCustoms.UpdateFRIMessage(QueryValFRI);

                                    if (sbFDM != null)
                                    {
                                        if (sbFDM.ToString() != "")
                                        {
                                            object[] QueryValEmail = { "FDM", FLTno, TextBoxdate.Text };
                                            DataSet dsEmail = new DataSet("Exp_Manifest_btnDepartFlt_dsEmail");
                                            dsEmail = objCustoms.GetCustomMessagesMailID(QueryValEmail);
                                            string EmailID = string.Empty;
                                            if (dsEmail != null)
                                            { EmailID = dsEmail.Tables[0].Rows[0]["EmailID"].ToString(); }
                                            cls_BL.addMsgToOutBox("FDM", sbFDM.ToString().ToUpper(), "", EmailID);
                                        }
                                    }
                                    //if (sbFRI != null)
                                    //{
                                    //    if (sbFRI.ToString() != "")
                                    //    {
                                    //        cls_BL.addMsgToOutBox("SCM", sbFRI.ToString().ToUpper(), "", "");
                                    //    }
                                    //}
                                }
                            }
                        }
                        catch (Exception ex)
                        { }
                        //Show popup to save actual operation time.
                        if (CommonUtility.ShowOperationTimeOnSave != null && CommonUtility.ShowOperationTimeOnSave == true)
                            SaveOperationTime("DEP", true);
                        else
                        {
                            //Save Operational date and By Default
                            SaveOperationTime("DEP", false);
                            BALCommon objBALCommon = new BALCommon();
                            System.Collections.Generic.List<SCM.Common.Struct.clsOperationTimeStamp> LstOperation = null;
                            LstOperation = (System.Collections.Generic.List<SCM.Common.Struct.clsOperationTimeStamp>)Session["listOperationTime"];

                            if (LstOperation != null && LstOperation.Count > 0)
                            {
                                if (LstOperation[0].UpdatedBy == null)
                                {
                                    LstOperation[0].UpdatedBy = Convert.ToString(Session["UserName"]);
                                    LstOperation[0].UpdatedOn = Convert.ToDateTime(Session["IT"]);
                                }

                                objBALCommon.SaveOperationalTimeStamp(LstOperation);
                            }
                            objBALCommon = null;
                            LstOperation = null;
                            //End
                        }

                        try
                        {
                            //Vijay - Code to check BillingEvent flag (EX- execution, AC- Acceptance, DP- Departure)
                            string BillingEvent = "";
                            ConBooking_GHA objBook = new ConBooking_GHA();
                            BookingBAL objBLL = new BookingBAL();
                            BillingEvent = objBLL.getConfiguredBillingEvent(AWBPrefix + "-" + AWBno);

                            //If BillingEvent is EX, record should be inserted in Billing tables
                            if (BillingEvent.ToUpper() == "DP")
                            {
                                objBook.AddBillingDetails(AWBPrefix + "-" + AWBno, BillingEvent.ToUpper());
                            }
                            objBook = null;
                            objBLL = null;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                try
                {
                    AutoFFMMessage();
                }
                catch (Exception ex)
                { }
            }
            else
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please Manifest Flight";
            }
        }

        /// <summary>
        /// Sets data for showing Operation Time Popup.
        /// </summary>
        private void SaveOperationTime(string MessageType, bool ShowOperationsPopuup)
        {
            try
            {

                //Set dataset for AWBs in AWB grid.                    
                List<SCM.Common.Struct.clsOperationTimeStamp> objListOpsTime = new List<SCM.Common.Struct.clsOperationTimeStamp>();

                SCM.Common.Struct.clsOperationTimeStamp objOpsTimeStamp;
                new SCM.Common.Struct.clsOperationTimeStamp();

                //Set data of AWB for updating Arrival time stamp.
                foreach (GridViewRow gvr in gdvULDDetails.Rows)
                {
                    objOpsTimeStamp = new SCM.Common.Struct.clsOperationTimeStamp();

                    if (((Label)gvr.FindControl("lblULDno")).Text.ToUpper() == "BULK")
                    {   //If AWB is not allocated to ULD.
                        if (((Label)gvr.FindControl("lblAWBno")).Text != "" &&
                            (((Label)gvr.FindControl("lblAWBno")).Text.Contains("-")) &&
                            (((Label)gvr.FindControl("lblAWBno")).Text.Length > ((Label)gvr.FindControl("lblAWBno")).Text.IndexOf('-')))
                        {
                            objOpsTimeStamp.AWBPrefix = ((Label)gvr.FindControl("lblAWBno")).Text.Substring(0,
                                ((Label)gvr.FindControl("lblAWBno")).Text.IndexOf('-'));
                            objOpsTimeStamp.AWBNumber = ((Label)gvr.FindControl("lblAWBno")).Text.Substring(
                                ((Label)gvr.FindControl("lblAWBno")).Text.IndexOf('-') + 1);
                        }
                        else
                        {
                            objOpsTimeStamp.AWBPrefix = "";
                            objOpsTimeStamp.AWBNumber = ((Label)gvr.FindControl("lblAWBno")).Text;
                        }
                        objOpsTimeStamp.ULDNumber = "";
                    }
                    else
                    {   //AWB is associated to ULD.
                        objOpsTimeStamp.ULDNumber = ((Label)gvr.FindControl("lblULDno")).Text.ToUpper();
                    }

                    objOpsTimeStamp.Description = "";
                    DateTime dt = System.DateTime.Now;
                    if (DateTime.TryParseExact(TextBoxdate.Text, "dd/MM/yyyy", null,
                        System.Globalization.DateTimeStyles.None, out dt))
                    {
                        objOpsTimeStamp.FlightDt = dt;
                    }
                    else
                    {
                        objOpsTimeStamp.FlightDt = DateTime.Now;
                    }
                    objOpsTimeStamp.FlightNo = txtFlightCode.Text + txtFlightID.Text;
                    objOpsTimeStamp.OperationalStatus = MessageType;
                    objOpsTimeStamp.OperationalType = MessageType;
                    objOpsTimeStamp.OperationDate = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy"); //DateTime.Now.ToString("dd/MM/yyyy");
                    objOpsTimeStamp.OperationTime = ((DateTime)Session["IT"]).ToString("HH:mm"); //DateTime.Now.ToString("HH:mm");
                    int pieceCount = 0;
                    if (int.TryParse(((Label)gvr.FindControl("lblMftPcs")).Text, out pieceCount))
                    {
                        objOpsTimeStamp.Pieces = pieceCount;
                    }
                    else
                    {
                        objOpsTimeStamp.Pieces = 0;
                    }
                    decimal weight = 0;
                    if (decimal.TryParse(((Label)gvr.FindControl("lblMftWt")).Text, out weight))
                    {
                        objOpsTimeStamp.Weight = weight;
                    }
                    else
                    {
                        objOpsTimeStamp.Weight = 0;
                    }
                    objOpsTimeStamp.Station = Session["Station"].ToString();

                    //Add individual item to list.
                    objListOpsTime.Add(objOpsTimeStamp);

                    objOpsTimeStamp = null;

                }

                //Check if list is built.
                if (objListOpsTime.Count > 0)
                {
                    Session["listOperationTime"] = objListOpsTime;
                    //Show popup for saving time stamp.
                    if (ShowOperationsPopuup)
                    {
                        //Check if data is available for updating Time Stamp.
                        txtOpsDate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                        txtOpsTimeHr.Text = Convert.ToDateTime(Session["IT"]).ToString("HH");
                        txtOpsTimeMin.Text = Convert.ToDateTime(Session["IT"]).ToString("mm");

                        lblPnlError.Text = "Please select Actual Operation time (if different than current local time)";
                        lblPnlError.ForeColor = System.Drawing.Color.Blue;

                        //Check if only 1 row is available in session to show last Updated On time stamp.
                        if (Session["listOperationTime"] != null)
                        {
                            List<clsOperationTimeStamp> objListOpsTimeChild = (List<clsOperationTimeStamp>)Session["listOperationTime"];
                            if (objListOpsTimeChild != null)
                            {   //If only 1 row is available in the session then fetch Last Update On timestamp.
                                if (objListOpsTimeChild.Count == 1)
                                {
                                    //Call function to get previous time stamp.
                                    BALCommon objCommon = new BALCommon();
                                    DateTime dtLast = objCommon.GetLastOperationalTimeStamp(objListOpsTimeChild);
                                    if (dtLast != null)
                                    {   //If valid previous udpate time received.
                                        if (dtLast != Convert.ToDateTime("01-JAN-1900"))
                                        {   //Show date time on screen.
                                            txtOpsDate.Text = dtLast.ToString("dd/MM/yyyy");
                                            txtOpsTimeHr.Text = dtLast.ToString("HH");
                                            txtOpsTimeMin.Text = dtLast.ToString("mm");

                                            lblPnlError.Text = "Last Operation time is as shown below:";
                                            lblPnlError.ForeColor = System.Drawing.Color.Blue;

                                        }
                                    }

                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "timePopup", "<SCRIPT LANGUAGE='javascript'>SetOperationTime();</script>", false);
                    }
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }
        }

        protected void btnReopenFit_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                lblDepAirport.Text) == "R")
            {
                BtnList_Click(this, new EventArgs());
                lblStatus.Text = "Flight is already reopened.";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            if (Session["RoleName"].ToString() != "Super User")
            {
                bool blnResult = VerifyReopenWithInterval();
                if (blnResult == false)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Allowed time for Re-Opening the flight has been Crossed.";
                    return;
                }
            }

            UpdateDepartedFlight("R");
            ////DeleteMileStone("Manifested");
            ////DeleteMileStone("Departed");


            //BtnList_Click(null, null);
        }

        private string UpdateDepartedFlight(string FlightStatus)
        {
            string ULDno = "", POU = "", POL = "", FLTno = "", Updatedby, Version = "", strfltdate = "";
            DateTime dtFlightdate;
            BLExpManifest objManifest = new BLExpManifest();

            FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
            Updatedby = Convert.ToString(Session["Username"]);

            //Added by Vijay
            string day = TextBoxdate.Text.Substring(0, 2);
            string mon = TextBoxdate.Text.Substring(3, 2);
            string yr = TextBoxdate.Text.Substring(6, 4);
            strfltdate = mon + "-" + day + "-" + yr;
            dtFlightdate = Convert.ToDateTime(strfltdate);
            //dtFlightdate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);

            ULDno = ddlULD.Text;
            POU = ddlPOU.Text;
            POL = lblDepAirport.Text.Trim();//ddlPOLDetails.Text;
            Version = ddlVersion.Text;

            string fltOrigin = "";
            if (Session["ExpManifest_FltOrigin"] != null)
	        {
                fltOrigin = Session["ExpManifest_FltOrigin"].ToString();
	        }

            string str = objManifest.ReOpenManifestdata(FLTno, ULDno, POU, POL, Updatedby, dtCurrentDate, FlightStatus,
                dtFlightdate, txtTailNo.Text, fltOrigin);

            lblStatus.ForeColor = Color.Green;
            if (str.Trim() == "" || str.Trim() == null)
            {
                if (FlightStatus == "D")
                {
                    lblStatus.Text = "Flight Departed Successfully.";
                    //                    ddlTailNo.Enabled = false;
                    txtTailNo.Enabled = false;

                    btnSave.Enabled = false;
                    btnDepartFit.Enabled = false;
                    btnReopenFit.Enabled = true;
                    btnOffload.Enabled = false;
                    BtnAddtoManifest.Enabled = false;
                    btnSplitAssign.Enabled = false;
                    btnFinalize.Enabled = false;
                    btnFetchClear.Enabled = false;
                    btnFetchAWB.Enabled = false;
                    btnAssignFetchedAWB.Enabled = false;
                }
                else
                {
                    lblStatus.Text = "Flight Re-Opened Successfully.";
                    //ddlTailNo.Enabled = true;
                    txtTailNo.Enabled = true;

                    btnSave.Enabled = true;
                    btnDepartFit.Enabled = true;
                    btnReopenFit.Enabled = false;
                    btnOffload.Enabled = true;
                    BtnAddtoManifest.Enabled = true;
                    btnSplitAssign.Enabled = true;
                    btnFinalize.Enabled = true;
                }

                return str;
            }
            else
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = str.Trim();
                return str;
            }
        }

        private bool SaveNilManifest(string FlightNo, string ULDNo, string POL, string POU, string UserName, DateTime FlightDate, DateTime SysDate)
        {
            BLExpManifest objManifest = new BLExpManifest();
            string tailNumber = "";
            //if (ddlTailNo.SelectedIndex > 0)
            tailNumber = txtTailNo.Text;
            bool blnResult = objManifest.SaveNilManifest(FlightNo, ULDNo, POL, POU, UserName, FlightDate, SysDate, tailNumber);
            objManifest = null;
            return blnResult;
        }

        protected void gdvULDDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnNOTOC_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            if (Session["IsManifested"] != null && (bool)Session["IsManifested"] == false)
            {
                showNOTOCData();
            }
        }

        protected void btnReAssign_Click(object sender, EventArgs e)
        {

            lblStatus.Text = "";
            DataTable dt = new DataTable("Exp_Manifest_btnReAssign_dt");
            DataTable DTAWBDetails = new DataTable("Exp_Manifest_btnReAssign_DTAWBDetails");
            DataTable dt1 = new DataTable("Exp_Manifest_btnReAssign_dt1");
            DataTable dtCurrentTable = new DataTable("Exp_Manifest_btnReAssign_dtCurrentTable");
            try
            {
                ddlReason.Visible = true;

                int AWBcheck = 0, ULDCheck = 0;
                for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                    {
                        AWBcheck = AWBcheck + 1;

                        if (((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblFltFlag")).Text.Trim() == "")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "AWB - " + ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAWBno")).Text.Trim() + " can't be Re-Assigned.";
                            return;
                        }
                    }
                }

                for (int i = 0; i < gdvULDLoadPlan.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDLoadPlan.Rows[i].FindControl("Check1")).Checked == true)
                    {
                        ULDCheck = ULDCheck + 1;

                        if (((Label)gdvULDLoadPlan.Rows[i].FindControl("lblFltFlag")).Text.Trim() == "")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "ULD - " + gdvULDLoadPlan.Rows[i].Cells[1].Text.Trim() + " can't be Re-Assigned.";
                            return;
                        }
                    }
                }

                if (AWBcheck == 0 && ULDCheck == 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select One ULD / AWB for Re-Assign";
                    return;
                }

                //if (ULDCheck > 1 || AWBcheck > 1)
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please Select Only One ULD / AWB for Re-Assign";
                //    return;
                //}

                if (ULDCheck > 0 && AWBcheck > 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select Only One ULD / AWB for Re-Assign";
                    return;
                }

                LoadAirlineCode("");

                //Added by Poorna
                Session["Split"] = "R";
                btnAddManifest.Text = "Re-Assign";

                lblReason.Visible = true;
                //txtReason.Visible = true;

                grdAWBs.Columns[2].Visible = false;
                grdAWBs.Columns[3].Visible = false;
                grdAWBs.Columns[5].Visible = true;
                grdAWBs.Columns[6].Visible = true;
                UpdatePanelRouteDetails.Visible = true;

                DTAWBDetails.Columns.Add("AWBNo");
                DTAWBDetails.Columns.Add("Pieces");
                DTAWBDetails.Columns.Add("Weight");
                DTAWBDetails.Columns.Add("AvlPCS");
                DTAWBDetails.Columns.Add("AvlWgt");
                DTAWBDetails.Columns.Add("Origin");
                DTAWBDetails.Columns.Add("Destination");
                DTAWBDetails.Columns.Add("ULDNo");
                DTAWBDetails.Columns.Add("CartNumber");

                if (AWBcheck > 0)
                {
                    for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                    {
                        if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
                        {
                            DataRow dr;

                            string POL = lblDepAirport.Text;
                            string POU = "";
                            if (Session["dsLoad"] != null)
                            {
                                DataSet dsLoad = new DataSet("Exp_Manifest_btnReAssign_dsLoad");
                                dsLoad = (DataSet)Session["dsLoad"];
                                if (dsLoad.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsLoad.Tables[0].Rows.Count; k++)
                                    {
                                        string awbno = dsLoad.Tables[0].Rows[k]["AWBNumber"].ToString();
                                        if (awbno == ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text.Trim())
                                        {
                                            POU = dsLoad.Tables[0].Rows[k]["POU"].ToString();

                                            break;
                                        }
                                    }
                                }
                            }

                            // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                            dr = DTAWBDetails.NewRow();
                            dr[0] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text;
                            dr[1] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text;
                            dr[2] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text;
                            dr[3] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text;
                            dr[4] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text;
                            dr[5] = POL;
                            dr[6] = POU;
                            dr[7] = "";
                            dr[8] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblCartNumber")).Text;

                            DTAWBDetails.Rows.Add(dr);
                            //break;
                        }
                    }
                }
                else if (ULDCheck > 0)
                {
                    for (int j = 0; j < gdvULDLoadPlan.Rows.Count; j++)
                    {
                        if (((CheckBox)gdvULDLoadPlan.Rows[j].FindControl("Check1")).Checked == true)
                        {
                            DataRow dr;

                            string POL = lblDepAirport.Text;
                            string POU = "";

                            if (Session["dsLoad"] != null)
                            {
                                DataSet dsLoad = new DataSet("Exp_Manifest_btnReAssign_dsLoad");
                                dsLoad = (DataSet)Session["dsLoad"];
                                if (dsLoad.Tables[4].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsLoad.Tables[4].Rows.Count; k++)
                                    {
                                        string ULDNo = dsLoad.Tables[4].Rows[k]["Uldno"].ToString();
                                        if (ULDNo == gdvULDLoadPlan.Rows[j].Cells[1].Text.Trim())
                                        {
                                            POU = dsLoad.Tables[4].Rows[k]["ULDDest"].ToString();
                                            break;
                                        }
                                    }
                                }
                            }

                            // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                            dr = DTAWBDetails.NewRow();
                            dr[0] = "";
                            dr[1] = gdvULDLoadPlan.Rows[j].Cells[3].Text.Trim();
                            dr[2] = gdvULDLoadPlan.Rows[j].Cells[4].Text.Trim();
                            dr[3] = gdvULDLoadPlan.Rows[j].Cells[3].Text.Trim();
                            dr[4] = gdvULDLoadPlan.Rows[j].Cells[4].Text.Trim();
                            dr[5] = POL;
                            dr[6] = POU;
                            dr[7] = gdvULDLoadPlan.Rows[j].Cells[1].Text.Trim();

                            DTAWBDetails.Rows.Add(dr);
                            break;
                        }
                    }
                }

                Session["AWBdata"] = DTAWBDetails;
                txtReason.Text = "";

                if ((DataTable)Session["AWBdata"] != null && ((DataTable)Session["AWBdata"]).Rows.Count > 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowOffloadPopup('R');</SCRIPT>", false);
                    pnlGrid.Visible = false;
                    return;
                }

                pnlGrid.Visible = true;
                // pnlGrid.Attributes.item("style") = "Z-INDEX: 176; LEFT: 584px; POSITION: absolute; TOP: 176px";
                pnlGrid.Style["Z-INDEX"] = "150";
                pnlGrid.Style["LEFT"] = "342px";
                pnlGrid.Style["POSITION"] = "absolute";
                pnlGrid.Style["TOP"] = "170px";
                Label1.Text = string.Empty;
                try
                {
                    LoadAWBGrid();
                    dt1 = DTAWBDetails;
                    dtCurrentTable = (DataTable)Session["AWBdata"];
                    grdAWBs.DataSource = dtCurrentTable;
                    grdAWBs.DataBind();
                    if (dt1.Rows.Count > 0)
                    {
                        DataRow drCurrentRow = null;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            drCurrentRow = dtCurrentTable.NewRow();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text = dt1.Rows[i][0].ToString();

                            ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text = dt1.Rows[i][1].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = dt1.Rows[i][2].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = dt1.Rows[i][3].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = dt1.Rows[i][4].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtOrigin")).Text = dt1.Rows[i][5].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtDestination")).Text = dt1.Rows[i][6].ToString();
                            ((Label)grdAWBs.Rows[i].FindControl("lblULDNO")).Text = dt1.Rows[i][7].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Enabled = false;
                            ((Label)grdAWBs.Rows[i].FindControl("lblCartNumber")).Text = dt1.Rows[i]["CartNumber"].ToString();

                            ViewState["CurrentTable1"] = dtCurrentTable;
                        }
                    }

                    # region Code to get Offload reasons

                    DataSet ds = new DataSet("Exp_Manifest_btnReAssign_ds");
                    ds = objExpMani.GetOffloadReasons();
                    {
                        try
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    txtReason.Enabled = false;
                                }
                                else
                                {
                                    txtReason.Enabled = true;
                                }
                            }
                            else
                            {
                                txtReason.Enabled = true;
                            }

                            //ds.Tables[0].Rows.Add("Others");
                            ddlReason.DataSource = ds.Tables[0];
                            ddlReason.DataTextField = "Reason";
                            ddlReason.DataValueField = "ReasonCode";
                            ddlReason.DataBind();
                            ddlReason.SelectedIndex = 0;
                            txtReason.Enabled = true;
                        }
                        catch (Exception ex) { }
                    }
                    #endregion
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                if (DTAWBDetails != null)
                    DTAWBDetails.Dispose();
                if (dt1 != null)
                    dt1.Dispose();
                if (dtCurrentTable != null)
                    dtCurrentTable.Dispose();
            }
        }

        private void AllButtonStatus(bool res)
        {
            BtnAddtoManifest.Enabled = res;
            //BtnClear.Enabled = res;
            //btnCommManifest.Enabled = res;
            btnDepartFit.Enabled = res;
            BtnList.Enabled = res;
            BtnListDetails.Enabled = res;
            BtnLoadPlanRefClear.Enabled = res;
            BtnLoadPlanRefList.Enabled = res;
            btnMailManifest.Enabled = res;
            btnNOTOC.Enabled = res;
            btnOffload.Enabled = res;
            btnPrintMFT.Enabled = res;
            btnReAssign.Enabled = res;
            btnReopenFit.Enabled = res;
            btnSave.Enabled = res;
            btnSendFFM.Enabled = res;
            btnShowEAWB.Enabled = res;
            btnSplitAssign.Enabled = res;
            btnSplitUnassign.Enabled = res;
            btnUnassign.Enabled = res;
        }

        protected void btnReturnToShipper_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable("Exp_Manifest_btnReturnToShipper_dt");
            DataTable DTAWBDetails = new DataTable("Exp_Manifest_btnReturnToShipper_DTAWBDetails");
            DataTable dt1 = new DataTable("Exp_Manifest_btnReturnToShipper_dt1");
            DataTable dtCurrentTable = new DataTable("Exp_Manifest_btnReturnToShipper_dtCurrentTable");
            lblStatus.Text = "";
            try
            {
                int AWBcheck = 0, ULDCheck = 0;
                for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                    {
                        AWBcheck = AWBcheck + 1;
                    }

                }

                for (int i = 0; i < gdvULDLoadPlan.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDLoadPlan.Rows[i].FindControl("Check1")).Checked == true)
                    {
                        ULDCheck = ULDCheck + 1;
                    }
                }

                if (AWBcheck == 0 && ULDCheck == 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select One ULD / AWB for Return to Shipper";
                    return;
                }

                if (ULDCheck > 1 || AWBcheck > 1)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select Only One ULD / AWB for Return to Shipper";
                    return;
                }

                if (ULDCheck > 0 && AWBcheck > 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select Only One ULD / AWB for Return to Shipper";
                    return;
                }

                Session["Split"] = "RSI";
                btnAddManifest.Text = "Return To Shipper";
                lblNextFlight.Visible = false;
                txtNextFlight.Visible = false;

                grdAWBs.Columns[2].Visible = true;
                grdAWBs.Columns[3].Visible = true;
                grdAWBs.Columns[5].Visible = false;
                grdAWBs.Columns[6].Visible = false;
                UpdatePanelRouteDetails.Visible = false;
                lblNFltDate.Visible = false;
                txtNFltDate.Visible = false;
                lblReason.Visible = false;
                txtReason.Visible = false;

                DTAWBDetails.Columns.Add("AWBNo");
                DTAWBDetails.Columns.Add("Pieces");
                DTAWBDetails.Columns.Add("Weight");
                DTAWBDetails.Columns.Add("AvlPCS");
                DTAWBDetails.Columns.Add("AvlWgt");
                DTAWBDetails.Columns.Add("ULDNo");
                DTAWBDetails.Columns.Add("CartNumber");

                # region Code to get Return To Shipper reasons

                DataSet ds = new DataSet("Exp_Manifest_btnReturnToShipper_ds");
                ds = objExpMani.GetReturnToShipperReasons();
                {
                    try
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                txtReason.Enabled = false;
                            }
                            else
                            {
                                txtReason.Enabled = true;
                            }
                        }
                        else
                        {
                            txtReason.Enabled = true;
                        }
                        //ddlReason.DataTextField = "Reason";
                        //ds.Tables[0].Rows.Add("Others");
                        //ddlReason.DataSource = ds.Tables[0];
                        ds.Tables[0].Rows.Add("Others");
                        ddlReason.DataSource = ds.Tables[0];
                        ddlReason.DataTextField = "Reason";
                        ddlReason.DataValueField = "ReasonCode";
                        ddlReason.DataBind();
                        ddlReason.SelectedIndex = 0;
                        txtReason.Visible = true;
                        txtReason.Enabled = true;
                    }
                    catch (Exception ex) { }
                }
                #endregion

                if (AWBcheck > 0)
                {
                    for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                    {
                        if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
                        {
                            DataRow dr;

                            // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                            dr = DTAWBDetails.NewRow();
                            dr[0] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text;// gdvULDLoadPlanAWB.Rows[j].Cells[1].Text;
                            dr[1] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text;//gdvULDLoadPlanAWB.Rows[j].Cells[2].Text;
                            dr[2] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text;
                            dr[3] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlPCS")).Text;//((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlPCS")).Text;
                            dr[4] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlWgt")).Text;//((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlWgt")).Text;
                            dr[5] = "";
                            dr[6] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblCartNumber")).Text.Trim();

                            DTAWBDetails.Rows.Add(dr);
                            HidFltFlag.Value = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblFltFlag")).Text.Trim();
                        }
                    }
                }
                else if (ULDCheck > 0)
                {
                    for (int j = 0; j < gdvULDLoadPlan.Rows.Count; j++)
                    {
                        if (((CheckBox)gdvULDLoadPlan.Rows[j].FindControl("Check1")).Checked == true)
                        {
                            DataRow dr;
                            //1
                            dr = DTAWBDetails.NewRow();
                            dr[0] = "";// gdvULDLoadPlanAWB.Rows[j].Cells[1].Text;
                            dr[1] = gdvULDLoadPlan.Rows[j].Cells[3].Text.Trim();
                            dr[2] = gdvULDLoadPlan.Rows[j].Cells[4].Text.Trim();
                            dr[3] = gdvULDLoadPlan.Rows[j].Cells[3].Text.Trim();
                            dr[4] = gdvULDLoadPlan.Rows[j].Cells[4].Text.Trim();
                            dr[5] = gdvULDLoadPlan.Rows[j].Cells[1].Text.Trim();
                            dr[5] = "";

                            DTAWBDetails.Rows.Add(dr);
                            HidFltFlag.Value = ((Label)gdvULDLoadPlan.Rows[j].FindControl("lblFltFlag")).Text.Trim();
                        }
                    }
                }


                Session["AWBdata1"] = DTAWBDetails;

                pnlGrid.Visible = true;
                // pnlGrid.Attributes.item("style") = "Z-INDEX: 176; LEFT: 584px; POSITION: absolute; TOP: 176px";
                pnlGrid.Style["Z-INDEX"] = "150";
                pnlGrid.Style["LEFT"] = "342px";
                pnlGrid.Style["POSITION"] = "absolute";
                pnlGrid.Style["TOP"] = "170px";

                try
                {
                    LoadAWBGrid();
                    dt1 = DTAWBDetails;
                    dtCurrentTable = (DataTable)Session["AWBdata1"];
                    grdAWBs.DataSource = dtCurrentTable;
                    grdAWBs.DataBind();
                    if (dt1.Rows.Count > 0)
                    {
                        DataRow drCurrentRow = null;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            drCurrentRow = dtCurrentTable.NewRow();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text = dt1.Rows[i][0].ToString();

                            ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text = dt1.Rows[i][1].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = dt1.Rows[i][2].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = dt1.Rows[i][3].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = dt1.Rows[i][4].ToString();
                            ((Label)grdAWBs.Rows[i].FindControl("lblULDNO")).Text = dt1.Rows[i][5].ToString();
                            ((Label)grdAWBs.Rows[i].FindControl("lblCartNumber")).Text = dt1.Rows[i]["CartNumber"].ToString();

                            ViewState["CurrentTable1"] = dtCurrentTable;
                        }
                    }
                    AllButtonStatus(false);
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                if (DTAWBDetails != null)
                    DTAWBDetails.Dispose();
                if (dt1 != null)
                    dt1.Dispose();
                if (dtCurrentTable != null)
                    dtCurrentTable.Dispose();
            }
        }

        private void ChangeButtonStatus()
        {
            try
            {
                btnSendFFM.Enabled = true;
                btnSendFBL.Enabled = true;                
            }
            catch (Exception ex)
            {


            }
        }

        protected void btnAddULDToManifest_Click(object sender, EventArgs e)
        {
            if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                lblDepAirport.Text) == "D")
            {
                BtnList_Click(this, new EventArgs());
                lblStatus.Text = "Flight is departed. Please reopen flight.";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            lblStatus.Text = "";
            DataTable dsULDData = new DataTable("Exp_Manifest_btnAddULDToManifest_dsULDData");
            DataTable dsULDAWBData = new DataTable("Exp_Manifest_btnAddULDToManifest_dsULDAWBData");
            try
            {
                int CntAWBChk = 0;
                for (int i = 0; i < gdvULDLoadPlan.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDLoadPlan.Rows[i].FindControl("Check1")).Checked == true)
                    {
                        CntAWBChk += 1;
                    }
                }

                int CntULDChk = 0;

                if (ddlSelectULD.SelectedIndex > 0)
                {
                    {
                        CntULDChk += 1;
                        if (CntAWBChk > 0)//ULD to be added to ULD tab only on association with awb.
                        {
                            chkduplicate = addgrid(ddlSelectULD.SelectedValue.ToString(), ddlSelectULD.SelectedItem.Text.ToString());
                        }
                    }
                }

                if (CntAWBChk != 0)
                {
                    if (Session["ULDData"] != null)
                    {
                        try
                        {
                            string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby = "", AWBDest = "", CommoCode = "", AWBOrigin = "";
                            int PCS = 0, AVLPCS = 0, AWBPcs = 0;
                            double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, AWBGwt = 0;
                            //string Updatedon = "";
                            string LoadingPriority = "", Remark = "", desc = "", Vol = "";
                            bool IsManifest = false;

                            FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                            POL = Convert.ToString(Session["Station"]);
                            // POU = ddlMainPOU.SelectedItem.Value.ToString();
                            POU = ddlMainPOU.Items[0].Value.ToString();
                            Updatedby = Convert.ToString(Session["Username"]);
                            ULDno = "BULK";
                            ULDwgt = 0;
                            DateTime FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);

                            if (Session["ULDData"] != null)
                            {
                                dsULDData = (DataTable)Session["ULDData"];
                                dsULDAWBData = (DataTable)Session["ULDAWBData"];
                            }

                            IsManifest = false;
                            for (int j = 0; j < gdvULDLoadPlan.Rows.Count; j++)
                            //for (int j = gdvULDLoadPlan.Rows.Count - 1; j >= 0; j--)
                            {
                                if (((CheckBox)gdvULDLoadPlan.Rows[j].FindControl("Check1")).Checked == true)
                                {
                                    for (int i = 0; i < dsULDAWBData.Rows.Count; i++)
                                    {
                                        ULDno = dsULDAWBData.Rows[i]["Uldno"].ToString();
                                        string ULDCheck = gdvULDLoadPlan.Rows[j].Cells[0].Text;
                                        if (ULDno == dsULDData.Rows[j][0].ToString())
                                        {
                                            AWBno = dsULDAWBData.Rows[i]["AWBNo"].ToString();
                                            PCS = Convert.ToInt32(dsULDAWBData.Rows[i]["Pieces"].ToString());
                                            WGT = Convert.ToDouble(dsULDAWBData.Rows[i]["Gweight"].ToString());
                                            AVLPCS = Convert.ToInt32(dsULDAWBData.Rows[i]["Pieces"].ToString());
                                            AVLWGT = Convert.ToDouble(dsULDAWBData.Rows[i]["Gweight"].ToString());

                                            AWBPcs = Convert.ToInt32(dsULDAWBData.Rows[i]["Pieces"].ToString());
                                            AWBGwt = Convert.ToDouble(dsULDAWBData.Rows[i]["Gweight"].ToString());
                                            // Updatedon = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                                            try
                                            {
                                                if (dsULDAWBData.Rows.Count > 0)
                                                {
                                                    for (int t = 0; t < dsULDAWBData.Rows.Count; t++)
                                                    {
                                                        if (dsULDAWBData.Rows[t][0].ToString() == AWBno)
                                                        {
                                                            AWBDest = dsULDAWBData.Rows[t][5].ToString();
                                                            desc = dsULDAWBData.Rows[t][6].ToString();
                                                            Vol = dsULDAWBData.Rows[t][7].ToString();
                                                            AWBOrigin = dsULDAWBData.Rows[t]["OrginCode"].ToString();
                                                            CommoCode = dsULDAWBData.Rows[t]["CommodityCode"].ToString();
                                                            dsULDAWBData.Rows[t].Delete();
                                                            dsULDAWBData.Rows[t].Delete();

                                                        }
                                                    }
                                                    dsULDAWBData.AcceptChanges();
                                                    dsULDAWBData.AcceptChanges();
                                                    dsULDData.AcceptChanges();
                                                    //adding collected data to gridview rows one by one
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                            }

                                            tabMenifestDetails = (DataTable)Session["ManifestGridData"];
                                            bool IsAvialble = false;
                                            try
                                            {
                                                if (tabMenifestDetails.Rows.Count > 0)
                                                {
                                                    for (int n = 0; n < tabMenifestDetails.Rows.Count; n++)
                                                    {
                                                        if (AWBno == tabMenifestDetails.Rows[n][5].ToString() && ULDno == dsULDData.Rows[j][0].ToString())
                                                        {
                                                            IsAvialble = true;

                                                            tabMenifestDetails.Rows[n][7] = (Convert.ToInt32(tabMenifestDetails.Rows[n][7].ToString()) + PCS);

                                                            tabMenifestDetails.Rows[n][8] = Convert.ToDouble(tabMenifestDetails.Rows[n][8].ToString()) + WGT;
                                                            break;
                                                        }

                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                            if (IsAvialble == false)
                                            {
                                                try
                                                {
                                                    string ActAWB = AWBno.Substring(AWBno.Length - 8);

                                                    DataSet dsDestDetails = new DataSet("Exp_Manifest_btnAddULDToManifest_dsDestDetails");
                                                    dsDestDetails = objExpMani.GetULDAWBData(ActAWB);
                                                    if (dsDestDetails.Tables[0].Rows.Count > 0)
                                                    {
                                                        AWBDest = dsDestDetails.Tables[0].Rows[0][0].ToString();
                                                        desc = dsDestDetails.Tables[0].Rows[0][1].ToString();
                                                        Vol = dsDestDetails.Tables[0].Rows[0][2].ToString();
                                                        CommoCode = dsDestDetails.Tables[0].Rows[0]["CommodityCode"].ToString();
                                                        AWBOrigin = dsDestDetails.Tables[0].Rows[0]["OriginCode"].ToString();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                }

                                                DataRow l_Datarow = tabMenifestDetails.NewRow();

                                                l_Datarow["ULDno"] = ULDno;
                                                l_Datarow["POU"] = POU;
                                                l_Datarow["POL"] = POL;
                                                l_Datarow["AWBDest"] = AWBDest;
                                                l_Datarow["Counter"] = "";
                                                l_Datarow["AWBno"] = AWBno;
                                                l_Datarow["SCC"] = CommoCode;
                                                l_Datarow["PCS"] = PCS;
                                                l_Datarow["GrossWgt"] = WGT;
                                                l_Datarow["VOL"] = Vol;
                                                l_Datarow["StatedPCS"] = AVLPCS;
                                                l_Datarow["StatedWgt"] = AVLWGT;

                                                l_Datarow["BookedPCS"] = AWBPcs;
                                                l_Datarow["BookedWgt"] = AWBGwt;

                                                l_Datarow["Desc"] = desc;
                                                l_Datarow["Orign"] = AWBOrigin;
                                                l_Datarow["Dest"] = AWBDest;
                                                l_Datarow["Manifested"] = IsManifest;
                                                l_Datarow["LoadingPriority"] = "";
                                                l_Datarow["Remark"] = "";
                                                l_Datarow["Bonded"] = "";

                                                tabMenifestDetails.Rows.Add(l_Datarow);
                                            }

                                        }
                                    }

                                    if (tabMenifestDetails != null && tabMenifestDetails.Rows.Count > 0)
                                    {
                                        Session["ManifestGridData"] = tabMenifestDetails;
                                    }

                                    gdvULDDetails.DataSource = "";

                                    gdvULDDetails.DataSource = (DataTable)Session["ManifestGridData"];
                                    gdvULDDetails.DataBind();

                                    Session["GDVULDDetails"] = tabMenifestDetails;

                                    dsULDData.Rows[j].Delete();
                                    dsULDData.AcceptChanges();
                                }
                            }

                            gdvULDLoadPlan.DataSource = null;
                            gdvULDLoadPlan.DataBind();

                            gdvULDLoadPlan.DataSource = dsULDData;
                            gdvULDLoadPlan.DataBind();
                            //Added on 27 Sept

                            Session["ULDAWBdata"] = dsULDAWBData;// dsawbData.Tables[1];
                            Session["AddToManifestAWBdata"] = dsULDAWBData;

                            for (int j = 0; j < tabMenifestDetails.Rows.Count; j++)
                            {
                                if (tabMenifestDetails.Rows[j][0].ToString().Trim().ToUpper() == "BULK")
                                {
                                    totalAWB = 1 + totalAWB;
                                    totalAWBPCS = totalAWBPCS + int.Parse(tabMenifestDetails.Rows[j][7].ToString());
                                    totalAWBWt = totalAWBWt + float.Parse(tabMenifestDetails.Rows[j][8].ToString());
                                    totalAWBVol = totalAWBVol + int.Parse(tabMenifestDetails.Rows[j][9].ToString());
                                }

                            }
                            //ShowULDAWBSummary();
                            //Session[""] = (DataTable);

                            hdnManifestFlag.Value = "1";
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (dsULDData != null)
                    dsULDData.Dispose();
                if (dsULDAWBData != null)
                    dsULDAWBData.Dispose();
            }
        }

        protected void btnSendFBL_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                string FlightNumber = txtFlightCode.Text.ToString() + txtFlightID.Text.ToString();
                DataSet ds = new DataSet("Exp_Manifest_btnSendFBL_ds");
                try
                {
                    BALEmailID ObjEmail = new BALEmailID();
                    ds = ObjEmail.GetEmail(lblDepAirport.Text, Session["Dest"].ToString(), "FBL", FlightNumber, txtFlightCode.Text.ToString());
                    txtEmailID.Text = ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
                    lblMsgCommType.Text = ds.Tables[0].Rows[0]["MsgCommType"].ToString();
                    if (lblMsgCommType.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) || lblMsgCommType.Text.Equals("SITA", StringComparison.OrdinalIgnoreCase))
                    {
                        GenerateSITAHeader(ds.Tables[0].Rows[0]["PartnerSITAiD"].ToString());
                    }
                }
                catch (Exception ex) { }

                ffmmsg = "";
                fblmsg = "fbl";
                Session["fbl"] = fblmsg;
                string Date = Session["Fltdate"].ToString().Trim();
                string Origin = Session["POLairport"].ToString().Trim();
                string FltNumber = Session["FltNumber"].ToString().Trim();
                //string ToId = Session["ToID"].ToString().Trim();
                txtMessageBody.Text = cls_BL.EncodeFBL(Origin, FltNumber, Date);

                lblMsg.Text = "FBL";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
            }
            catch (Exception ex)
            { }
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                lblDepAirport.Text) == "D")
            {
                BtnList_Click(this, new EventArgs());
                lblStatus.Text = "Flight is departed. Please reopen flight.";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            lblStatus.Text = "";
            try
            {
                string FlightNumber;
                DateTime FlightDate;
                //int Totpcs, PcsToAssign;
                //Decimal TotWgt, WgtToAssign;

                if (Session["AWBdata"] == null)
                    return;
                DataTable dtAWB = new DataTable("Exp_Manifest_btnAssign_dtAWB");
                dtAWB = (DataTable)Session["AWBdata"];
                DataTable dtCreditInfoA = new DataTable("Exp_Manifest_btnAssign_dtCreditInfoA");
                dtCreditInfoA.Columns.Add("AWB");
                dtCreditInfoA.Columns.Add("TotPcs");
                dtCreditInfoA.Columns.Add("TotWgt");
                dtCreditInfoA.Columns.Add("PcsToAssign");
                dtCreditInfoA.Columns.Add("WgttoAssign");
                dtCreditInfoA.Columns.Add("Origin");
                dtCreditInfoA.Columns.Add("Destination");
                dtCreditInfoA.Columns.Add("FlightNumber");
                dtCreditInfoA.Columns.Add("FliightDate");

                FlightNumber = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                // string  fltDate = Convert.ToDateTime 
                FlightDate = DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null);


                if (dtAWB != null && dtAWB.Rows.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < dtAWB.Rows.Count; i++)
                        {
                            DataRow rw = dtCreditInfoA.NewRow();
                            string AWBNo = dtAWB.Rows[i]["AWBNumber"].ToString();
                            if (AWBNo.Length > 8)
                            {
                                AWBNo = AWBNo.Substring(AWBNo.Length - 8);
                            }
                            rw["AWB"] = AWBNo;
                            rw["TotPcs"] = dtAWB.Rows[i]["PiecesCount"];
                            rw["TotWgt"] = dtAWB.Rows[i]["ChargedWeight"];
                            rw["PcsToAssign"] = dtAWB.Rows[i]["PiecesCount"];
                            rw["WgttoAssign"] = dtAWB.Rows[i]["ChargedWeight"];
                            rw["Origin"] = dtAWB.Rows[i]["FltOrigin"];
                            rw["Destination"] = dtAWB.Rows[i]["FltDestination"];
                            rw["FlightNumber"] = FlightNumber;
                            rw["FliightDate"] = FlightDate;

                            dtCreditInfoA.Rows.Add(rw);
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    Session["AWBForULDAssoc"] = dtCreditInfoA;
                    if (Session["AWBForULDAssoc"] != null)
                    {
                        //Response.Redirect("frmULDToAWBAssoc.aspx",false);
                        ClientScript.RegisterStartupScript(this.GetType(), "", "callexportULD();", true);
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        #region allocFirstLine
        protected void allocFirstLine(string AWBNo, int Totpcs, decimal TotWgt, int PcsToAssign, decimal WgtToAssign, string Origin, string Destination, string FlightNumber, DateTime FlightDate)
        {
            try
            {
                DataTable dtCreditInfoA = new DataTable("Exp_Manifest_allocFirstLine_dtCreditInfoA");
                dtCreditInfoA.Columns.Add("AWB");
                dtCreditInfoA.Columns.Add("TotPcs");
                dtCreditInfoA.Columns.Add("TotWgt");
                dtCreditInfoA.Columns.Add("PcsToAssign");
                dtCreditInfoA.Columns.Add("WgttoAssign");
                dtCreditInfoA.Columns.Add("Origin");
                dtCreditInfoA.Columns.Add("Destination");
                dtCreditInfoA.Columns.Add("FlightNumber");
                dtCreditInfoA.Columns.Add("FliightDate");

                DataRow rw = dtCreditInfoA.NewRow();
                rw["AWB"] = AWBNo;
                rw["TotPcs"] = Totpcs;
                rw["TotWgt"] = TotWgt;
                rw["PcsToAssign"] = PcsToAssign;
                rw["WgttoAssign"] = WgtToAssign;
                rw["Origin"] = Origin;
                rw["Destination"] = Destination;
                rw["FlightNumber"] = FlightNumber;
                rw["FliightDate"] = FlightDate;
                dtCreditInfoA.Rows.Add(rw);
                Session["AWBForULDAssoc"] = dtCreditInfoA;
                if (Session["AWBForULDAssoc"] != null)
                {
                    //Response.Redirect("frmULDToAWBAssoc.aspx",false);
                    ClientScript.RegisterStartupScript(this.GetType(), "", "callexportULD();", true);
                }

            }
            catch (Exception ex)
            {
            }

        }
        #endregion allocFirstLine

        #region AddRouteDetails
        protected void btnAddRouteDetails_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                string prevdest = "", prevtime = "", strDate = "";

                if (grdRouting.Rows.Count != 0)
                {
                    if (((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim() == "")
                    {
                        LBLRouteStatus.Text = "Set previous destination first.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                        return;
                    }

                    else
                    {
                        prevdest = ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim();
                        if (prevdest == BookingBAL.DestStation)
                        {
                            prevdest = BookingBAL.OrgStation;
                        }

                    }
                }

                strDate = ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFdate")).Text.Trim();

                SaveRouteDetails();

                DataSet dsRoutDetails = new DataSet("Exp_Manifest_btnAddRouteDetails_dsRoutDetails");
                dsRoutDetails = ((DataSet)Session["dsRoutDetails"]).Copy();
                DataRow row = dsRoutDetails.Tables[0].NewRow();

                row["FltOrigin"] = prevdest;
                row["FltDate"] = strDate;  //dtCurrentDate.ToString("dd/MM/yyyy");
                row["Airline"] = txtFlightCode.Text.ToString();

                dsRoutDetails.Tables[0].Rows.Add(row);

                Session["dsRoutDetails"] = dsRoutDetails.Copy();
                grdRouting.DataSource = null;
                grdRouting.DataSource = dsRoutDetails.Copy();
                grdRouting.DataBind();

                //Validation by Vijay
                ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).ReadOnly = true;
                // ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltOrig")).ReadOnly = true;

                LoadDropDownAndCheckBoxRouteDetails();
                LoadAirlineCode("");
                // Session["Mod"] = "1";
                CheckPartnerFlightSupport();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        }
        #endregion AddRouteDetails

        #region DeleteRow
        protected void btnDeleteRoute_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                SaveRouteDetails();

                DataSet dsRouteDetailsTemp = new DataSet("Exp_Manifest_btnDeleteRoute_dsRouteDetailsTemp");
                dsRouteDetailsTemp = ((DataSet)Session["dsRoutDetails"]).Clone();
                DataSet dsRouteDetails = new DataSet("Exp_Manifest_btnDeleteRoute_dsRouteDetails");
                dsRouteDetails = (DataSet)Session["dsRoutDetails"];

                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (!((CheckBox)grdRouting.Rows[i].FindControl("CHKSelect")).Checked)
                    {
                        DataRow row = dsRouteDetailsTemp.Tables[0].NewRow();

                        row["FltOrigin"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
                        row["FltDestination"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text;
                        row["FltNumber"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem;
                        row["FltDate"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text;
                        row["Pcs"] = ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim();
                        row["Wt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim();
                        row["Airline"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem;
                        row["PartnerType"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedItem;
                        row["Location"] = ((TextBox)grdRouting.Rows[i].FindControl("txtRouteLocation")).Text.Trim();
                        dsRouteDetailsTemp.Tables[0].Rows.Add(row);
                    }
                }

                Session["dsRoutDetails"] = dsRouteDetailsTemp.Copy();
                grdRouting.DataSource = null;
                grdRouting.DataSource = dsRouteDetailsTemp.Copy();
                grdRouting.DataBind();

                LoadDropDownAndCheckBoxRouteDetails();

                Session["Mod"] = "1";
                CheckPartnerFlightSupport();
            }
            catch (Exception ex)
            {
                LBLRouteStatus.Text = "" + ex.Message;
            }
        }
        #endregion DeleteRow

        #region SaveRouteDetails
        public void SaveRouteDetails()
        {
            DataSet dsRoutDetails = new DataSet("Exp_Manifest_SaveRouteDetails_dsRoutDetails");
            dsRoutDetails = ((DataSet)Session["dsRoutDetails"]).Clone();

            for (int i = 0; i < grdRouting.Rows.Count; i++)
            {
                DataRow row = dsRoutDetails.Tables[0].NewRow();

                row["FltOrigin"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
                row["FltDestination"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text;
                row["FltNumber"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
                row["FltTime"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Value;
                row["FltDate"] = ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text;
                row["Pcs"] = ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim();
                row["Wt"] = ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" ? "0" : ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim();
                row["Airline"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedItem.Text;
                row["PartnerType"] = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedItem.Text;
                row["Location"] = ((TextBox)grdRouting.Rows[i].FindControl("txtRouteLocation")).Text.Trim();
                dsRoutDetails.Tables[0].Rows.Add(row);
            }

            Session["dsRoutDetails"] = dsRoutDetails.Copy();

        }
        #endregion SaveRouteDetails

        #region LoadDropdown
        public void LoadDropDownAndCheckBoxRouteDetails()
        {
            try
            {
                DataSet dsRouteDetails = new DataSet("Exp_Manifest_LoadDropDownAndCheckBox_dsRouteDetails");
                dsRouteDetails = (DataSet)Session["dsRoutDetails"];
                for (int i = 0; i < dsRouteDetails.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["PartnerType"].ToString().Trim()));
                        DropDownList routedroplist = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType"));
                        routedroplist.Text = dsRouteDetails.Tables[0].Rows[i]["PartnerType"].ToString().Trim();
                    }
                    catch (Exception ex) { }

                    try
                    {
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["Airline"].ToString().Trim()));
                        DropDownList routedroplist = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner"));
                        routedroplist.Text = dsRouteDetails.Tables[0].Rows[i]["Airline"].ToString().Trim();

                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Clear();
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["FltNumber"].ToString().Trim(), dsRouteDetails.Tables[0].Rows[i]["FltTime"].ToString().Trim()));



                    }
                    catch (Exception ex) { }
                    ///ddlPartner

                }
            }
            catch (Exception ex)
            {
                LBLRouteStatus.Text = "" + ex.Message;
            }

        }
        #endregion LoadDropdown

        protected void txtFltDest_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;

                //validation by Vijay
                LBLRouteStatus.Text = "";

                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text == ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text)
                    {
                        LBLRouteStatus.Text = "kindly check Flight origin and destination in route details.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text = "";
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Focus();
                        return;
                    }

                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")) == ((TextBox)sender) || ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")) == ((TextBox)sender))
                    {
                        rowindex = i;
                    }

                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")) == ((TextBox)sender))
                    {
                        rowindex = i;

                        //validation by Vijay
                        if (rowindex < grdRouting.Rows.Count - 1 && ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim() == ((TextBox)grdRouting.Rows[0].FindControl("txtOrigin")).Text.Trim())
                        {
                            LBLRouteStatus.Text = "kindly check Flight origin and destination in route details.";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                            ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Focus();
                            return;
                        }

                        if (rowindex < grdRouting.Rows.Count - 1)
                        {
                            ((TextBox)grdRouting.Rows[rowindex + 1].FindControl("txtFltOrig")).Text = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text;
                        }
                    }
                }

                BookingBAL.OrgStation = BookingBAL.OrgStation.Trim();

                ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper().Trim();
                ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper().Trim();

                //string errormessage = "";
                UpdatePartnerCode(rowindex);
                GetFlightRouteData(rowindex);

                Session["Mod"] = "1";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        #region GetFlightList
        public DataSet GetFlightList(string Origin, string Dest, string strdate, int hr, int min, int AllowedHr, ref string errormessage)
        {
            DataSet dsResult = new DataSet("Exp_Manifest_GetFlightList_dsResult");
            if (strdate.Trim() == "")
            {
                if (new ShowFlightsBAL().GetFlightList(Origin, Dest, 0, ref dsResult, ref errormessage, dtCurrentDate))
                {
                    FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                    return dsResult;
                }
                else
                {
                    return null;
                }
            }
            else
            {

                string[] splitdate = strdate.Split(new char[] { '/' });
                int year = int.Parse(splitdate[2]);
                int month = int.Parse(splitdate[1]);
                int day = int.Parse(splitdate[0]);
                DateTime dt = new DateTime(year, month, day);

                int diff = (dt - dtCurrentDate.Date).Days;

                if (new ShowFlightsBAL().GetFlightList(Origin, Dest, diff, ref dsResult, ref errormessage, dtCurrentDate.Date))
                {
                    FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                    return dsResult;
                }
                else
                {
                    return null;
                }



            }

        }
        #endregion GetFlightList

        #region  FormatRecords
        public void FormatRecords(string org, string dest, ref DataSet dsResult, int PrevHr, int PrevMin, int AllowedHr)
        {
            int i = 0;
            string ScheduleID = "";
            DataSet dsNewResult = new DataSet("Exp_Manifest_FormatRecords_dsNewResult");
            dsNewResult = dsResult.Clone();
            bool blOrignFlound, blDestFound;
            blOrignFlound = blDestFound = false;

            foreach (DataRow row in dsResult.Tables[0].Rows)
            {
                if (ScheduleID == "")
                {
                    if (row["FltOrigin"].ToString() != org)
                    {
                        continue;
                    }
                    else
                    {
                        blOrignFlound = true;
                    }

                    ScheduleID = row["ScheduleID"].ToString();
                    DataRow rw = dsNewResult.Tables[0].NewRow();

                    for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                    {
                        rw[j] = row[j];
                    }

                    if (row["FltDestination"].ToString() == dest)
                    {
                        blDestFound = true;
                    }

                    dsNewResult.Tables[0].Rows.Add(rw);

                }
                else if (ScheduleID.Trim() == row["ScheduleID"].ToString())
                {
                    if (!blDestFound)
                    {
                        dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["FltDestination"] = row["FltDestination"].ToString();
                        dsNewResult.Tables[0].Rows[dsNewResult.Tables[0].Rows.Count - 1]["ArrTime"] = row["ArrTime"].ToString();

                        if (row["FltDestination"].ToString() == dest)
                        {
                            blDestFound = true;
                        }

                    }

                }
                else
                {
                    if (row["FltOrigin"].ToString() != org)
                    {
                        continue;
                    }
                    else
                    {
                        blOrignFlound = true;
                        blDestFound = false;
                    }

                    ScheduleID = row["ScheduleID"].ToString();


                    DataRow rw = dsNewResult.Tables[0].NewRow();

                    for (int j = 0; j < dsNewResult.Tables[0].Columns.Count; j++)
                    {
                        rw[j] = row[j];
                    }

                    if (row["FltDestination"].ToString() == dest)
                    {
                        blDestFound = true;
                    }

                    dsNewResult.Tables[0].Rows.Add(rw);
                }

                i++;

            }

            dsResult = dsNewResult.Copy();
            DataView dv = new DataView(dsResult.Tables[0].Copy());
            dv.Sort = "DeptTime";

            dsResult = new DataSet("Exp_Manifest_FormatRecords_dsResult");
            dsResult.Tables.Add(dv.ToTable().Copy());


            //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);


            DataTable dt = new DataTable("Exp_Manifest_FormatRecords_dt");
            dt = dsResult.Tables[0].Clone();
            foreach (DataRow row in dsResult.Tables[0].Rows)
            {
                string depttime = row["DeptTime"].ToString();
                int hr = int.Parse(depttime.Substring(0, depttime.IndexOf(":")));
                int min = int.Parse(depttime.Substring(depttime.IndexOf(":") + 1));

                string[] strDate = row["FltDate"].ToString().Split('/');
                int intFltDate = int.Parse(strDate[0]);
                int intCurrentDt = dtCurrentDate.Day;

                bool canAdd = true;
                //if (hr < (PrevHr + AllowedHr) && intFltDate == intCurrentDt)
                //{
                //    if (hr > PrevHr)
                //    {
                //        int hrdiff = hr - PrevHr;

                //        if (((hrdiff * 60) - PrevMin + min) > (AllowedHr * 60))
                //            canAdd = true;
                //    }
                //}
                //else
                //    canAdd = true;


                if (canAdd)
                {
                    DataRow rw = dt.NewRow();

                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        rw[k] = row[k];
                    }

                    dt.Rows.Add(rw);
                }
            }

            dsResult = new DataSet("Exp_Manifest_FormatRecords_dsResult");
            dsResult.Tables.Add(dt);
        }
        #endregion FormatRecords

        #region FltDate
        protected void txtFdate_TextChanged(object sender, EventArgs e)
        {
            //if (HidChangeDate.Value != "Y")
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            //    return;
            //}

            try
            {
                int rowindex = 0;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFdate")) == ((TextBox)sender) || ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")) == ((TextBox)sender))
                    {
                        rowindex = i;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Focus();
                    }
                }

                int hr = 0, min = 0, AllowedHr = 0;

                string errormessage = "";
                DataSet dsresult = new DataSet("Exp_Manifest_txtFdateText_dsresult");
                dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, hr, min, AllowedHr, ref errormessage);

                if (dsresult != null && dsresult.Tables.Count != 0)
                {
                    Session["Flt"] = dsresult.Copy();
                    DataRow row = dsresult.Tables[0].NewRow();
                    row["FltNumber"] = "";
                    row["ArrTime"] = "";

                    dsresult.Tables[0].Rows.Add(row);

                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataTextField = "FltNumber";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataValueField = "ArrTime";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataSource = dsresult.Tables[0].Copy();
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataBind();
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).SelectedIndex = dsresult.Tables[0].Rows.Count - 1;

                }
                else
                {
                    LBLRouteStatus.Text = "no record found";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).Items.Clear();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                    return;
                }

            }
            catch (Exception ex)
            {

            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        }
        #endregion FltDate

        #region FltNumber
        protected void txtFltNumber_TextChanged(object sender, EventArgs e)
        {
            int rowindex = 0;
            for (int i = 0; i < grdRouting.Rows.Count; i++)
            {
                if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")) == ((DropDownList)sender))
                {
                    rowindex = i;
                }
            }

            DataSet ds = new DataSet("Exp_Manifest_txtFltNumberText_ds");
            ds = (DataSet)Session["Flt"];

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string Origin = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper();
                string Dest = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper();
                string FlightNo = ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).SelectedItem.ToString();
                int ScheduleID = 0;

                for (int intCount = 0; intCount < ds.Tables[0].Rows.Count; intCount++)
                {
                    if (Origin == ds.Tables[0].Rows[intCount]["FltOrigin"].ToString() && Dest == ds.Tables[0].Rows[intCount]["FltDestination"].ToString()
                        && FlightNo == ds.Tables[0].Rows[intCount]["FltNumber"].ToString())
                    {
                        ScheduleID = int.Parse(ds.Tables[0].Rows[intCount]["ScheduleID"].ToString());
                    }
                }

                ((HiddenField)grdRouting.Rows[rowindex].FindControl("HidScheduleID")).Value = ScheduleID.ToString();
                //Session["Mod"] = "1";
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }
        #endregion FltNumber

        #region loadgridroutingdetails
        public void LoadGridRoutingDetail()
        {
            DataTable myDataTable = new DataTable("Exp_Manifest_LoadGridRoutingDetail_myDataTable");
            DataColumn myDataColumn;
            DataSet Ds = new DataSet("Exp_Manifest_LoadGridRouingDetail_Ds");

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FltOrigin";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FltDestination";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FltNumber";
            myDataTable.Columns.Add(myDataColumn);


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FltTime";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FltDate";
            myDataTable.Columns.Add(myDataColumn);


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Pcs";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Wt";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "PartnerType";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Airline";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Location";
            myDataTable.Columns.Add(myDataColumn);

            DataRow dr;
            dr = myDataTable.NewRow();
            dr["FltOrigin"] = Session["Station"].ToString();
            dr["FltDestination"] = "";// "DEL";
            dr["FltNumber"] = "";// "IT101";
            dr["FltDate"] = dtCurrentDate.ToString("dd/MM/yyyy");// "14/Jan/2012";
            dr["Pcs"] = "";// 
            dr["Wt"] = "";// 
            
            myDataTable.Rows.Add(dr);

            grdRouting.DataSource = null;
            grdRouting.DataSource = myDataTable;
            grdRouting.DataBind();

            //((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltOrig")).ReadOnly = true;

            DataSet dsRoutDetails = new DataSet("Exp_Manifest_LoadGridRoutingDetail_dsRoutDetails");
            dsRoutDetails.Tables.Add(myDataTable);
            Session["dsRoutDetails"] = dsRoutDetails.Copy();

            // SetRouteGridValues(myDataTable);
        }
        #endregion loadgridroutingdetails

        protected void btnFinalize_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                    lblDepAirport.Text) == "D")
                {
                    BtnList_Click(this, new EventArgs());
                    lblStatus.Text = "Flight is already departed.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                ////btnSave_Click(null, null);
                if (txtTailNo.Text.Trim() == "")
                {
                    txtTailNo.Focus();
                    lblStatus.Text = "Please enter Tail No";
                    lblStatus.ForeColor = Color.Red;
                    txtTailNo.Focus();
                    return;
                }

                if (Session["IsCommit"] != null)
                {
                    string str = Convert.ToString(Session["IsCommit"]);
                    if (str == "True")
                    {
                        return;
                    }
                }


                bool res = false;

                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    //if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
                    //{
                    string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby, counter = "", SCC = "", Desc = "";
                    int PCS = 0, AVLPCS = 0;
                    double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, VOL = 0;
                    string Updatedon = "";
                    string LoadingPriority = "", Remark = "", ULDDest = "";
                    //bool IsManifest = false;
                    FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                    Updatedby = Convert.ToString(Session["Username"]);

                    ULDno = ((Label)gdvULDDetails.Rows[i].FindControl("lblULDno")).Text.Trim();
                    POU = ((Label)gdvULDDetails.Rows[i].FindControl("lblPOU")).Text.Trim();
                    POL = ((Label)gdvULDDetails.Rows[i].FindControl("lblPOL")).Text.Trim();
                    ULDDest = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBDest")).Text.Trim();
                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblcounter")).Text == "&nbsp;")
                    {
                        counter = "0";
                    }
                    else
                    {
                        counter = ((Label)gdvULDDetails.Rows[i].FindControl("lblcounter")).Text;
                    }
                    string Number = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text.Trim();
                    //AWBno = AWBno.Substring(AWBno.Length - 8);

                    //string AWBPrefix = gdvULDDetails.Rows[i].Cells[6].Text.Trim().Replace(AWBno, "");
                    string AWBPrefix = string.Empty;

                    if (Number.Replace("&nbsp;", "") != "")
                    {
                        AWBPrefix = Number.Substring(0, Number.IndexOf('-'));
                        AWBno = Number.Substring(Number.IndexOf('-') + 1, 8);
                    }
                    else
                    {
                        AWBPrefix = "";
                        AWBno = "";
                    }

                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblSCC")).Text == "&nbsp;")
                    {
                        SCC = "0";
                    }
                    else
                    {
                        SCC = ((Label)gdvULDDetails.Rows[i].FindControl("lblSCC")).Text;
                    }


                    PCS = Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblMftPcs")).Text);
                    WGT = Convert.ToDouble(((Label)gdvULDDetails.Rows[i].FindControl("lblMftWt")).Text);
                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblVOL")).Text.Trim() == "&nbsp;" || ((Label)gdvULDDetails.Rows[i].FindControl("lblVOL")).Text.Trim() == "")
                    {
                        VOL = 0;
                    }
                    else
                    {
                        VOL = Convert.ToDouble(((Label)gdvULDDetails.Rows[i].FindControl("lblVOL")).Text);
                    }
                    AVLPCS = Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblStatedPCS")).Text);
                    AVLWGT = Convert.ToDouble(((Label)gdvULDDetails.Rows[i].FindControl("lblStatedWgt")).Text);
                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblCommDesc")).Text == "&nbsp;")
                    {
                        Desc = "";
                    }
                    else
                    {
                        Desc = ((Label)gdvULDDetails.Rows[i].FindControl("lblCommDesc")).Text;
                        Desc = Desc.Replace("&amp;", "&");
                    }
                    bool IsManifest = bool.Parse(((Label)gdvULDDetails.Rows[i].FindControl("lblManifested")).Text.Trim());


                    if (((DropDownList)gdvULDDetails.Rows[i].FindControl("ddlLoadingPriority")).Text == "&nbsp;")
                    {
                        LoadingPriority = "";
                    }
                    else
                    {
                        LoadingPriority = ((DropDownList)gdvULDDetails.Rows[i].FindControl("ddlLoadingPriority")).SelectedItem.Text.ToString();
                    }

                    if (((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text == "&nbsp;")
                    {
                        Remark = "";
                    }
                    else
                    {
                        Remark = ((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text.ToString();
                    }

                    string CartNumber = ((Label)gdvULDDetails.Rows[i].FindControl("lblCartNumber")).Text.ToString();

                    // Updatedon = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    Updatedon = Convert.ToDateTime(Session["IT"]).ToString(); // DateTime.Now.ToString("yyyy/MM/dd");
                    DateTime dtflightdate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);
                    //Added code to commit only AWB's Loaded from POL
                    if (Session["Station"].ToString() == POL)
                    {
                        res = objExpMani.CommitManifestdata(FLTno, ULDno, POU, POL, ULDDest, counter, AWBno, SCC, VOL, 
                            PCS, WGT, AVLPCS, AVLWGT, Desc, LoadingPriority, Remark, Updatedby, Updatedon, dtflightdate, 
                            AWBPrefix, CartNumber, txtTailNo.Text.Trim().ToUpper());
                    }

                    //Code Added to Send Cutoms FRI Message 
                    #region Send FRI MESSAGE CUSTOMS
                    try
                    {
                        CustomsImportBAL objCustoms = new CustomsImportBAL();
                        object[] QueryValues = new object[8];


                        QueryValues[0] = AWBno;
                        QueryValues[1] = FLTno.Substring(0, 2);
                        QueryValues[2] = FLTno.Substring(2, FLTno.Length - 2);
                        QueryValues[3] = ((DateTime)Session["IT"]).ToString();
                        QueryValues[4] = Session["UserName"].ToString();
                        QueryValues[5] = AWBPrefix;
                        QueryValues[6] = dtflightdate;
                        object[] QueryVal = { AWBPrefix + "-" + AWBno, FLTno, TextBoxdate.Text };
                        DataSet dCust = new DataSet("Exp_Manifest_btnFinalize_dCust");
                        dCust = objCustoms.CheckCustomsAWBAvailability(QueryVal);
                        if (dCust != null)
                        {
                            if (dCust.Tables[0].Rows[0]["Validate"].ToString() == "True" && dCust.Tables[1].Rows[0]["Validate"].ToString() == "True")
                            {

                                StringBuilder sbFRI = objCustoms.EncodingFRIMessage(QueryVal);
                                object[] QueryValFRI = { AWBPrefix + AWBno, 1, FLTno, TextBoxdate.Text, sbFRI.ToString().ToUpper() };

                                objCustoms.UpdateFRIMessage(QueryValFRI);

                                if (sbFRI != null)
                                {
                                    if (sbFRI.ToString() != "")
                                    {
                                        object[] QueryValMail = { "FRI", FLTno, TextBoxdate.Text };
                                        //Getting MailID for FRI Message
                                        DataSet dMail = new DataSet("Exp_Manifest_btnFinalize_dMail");
                                        dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                        string MailID = string.Empty;
                                        if (dMail != null)
                                        {
                                            MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                        }
                                        cls_BL.addMsgToOutBox("FRI", sbFRI.ToString().ToUpper(), "", MailID);
                                    }
                                }

                                #region ACAS PRI Message Automation Deepak(15/04/2014)
                                try
                                {
                                    //LoginBL objLogin = new LoginBL();
                                    //if (Convert.ToBoolean(objLogin.GetMasterConfiguration("ACASAutomation")))
                                    if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ACASAutomation")))
                                    {
                                        ACASBAL objACAS = new ACASBAL();

                                        if (objACAS.ACASFRITriggerPointValidation() == "MF")
                                        {


                                            StringBuilder sbPRI = objACAS.EncodingPRIMessage(QueryVal);

                                            object[] QueryValPRI = { AWBPrefix + "-" + AWBno, 1, FLTno, TextBoxdate.Text, sbPRI.ToString().ToUpper() };

                                            if (objACAS.UpdatePRIMessage(QueryValPRI))
                                            {
                                                if (sbPRI != null)
                                                {
                                                    if (sbPRI.ToString() != "")
                                                    {
                                                        object[] QueryValMail = { "PRI", FLTno, TextBoxdate.Text };
                                                        //Getting MailID for PRI Message
                                                        DataSet dMail = new DataSet("Exp_Manifest_btnFinalize_dMail");
                                                        dMail = objACAS.GetCustomMessagesMailID(QueryValMail);
                                                        string MailID = string.Empty;
                                                        if (dMail != null)
                                                        {
                                                            MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                        }
                                                        cls_BL.addMsgToOutBox("PRI", sbPRI.ToString().ToUpper(), "", MailID);
                                                    }
                                                }
                                            }



                                        }

                                    }
                                }
                                catch (Exception Ex)
                                { }

                                #endregion
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                    #endregion


                }

                if (res == true)
                {
                    // ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Successfull();", true);
                    cls_BL.addMsgToOutBox("SCM", "Flight - " + txtFlightCode.Text.Trim() + txtFlightCode.Text.Trim() + "-" + TextBoxdate.Text.Trim() + " Finalized.", "", "");
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "Manifest Version Commited Successfully";
                    // return;
                    btnSave.Enabled = false;
                    btnDepartFit.Enabled = false;
                    btnFetchClear.Enabled = false;
                    btnFetchAWB.Enabled = false;
                    btnAssignFetchedAWB.Enabled = false;
                    btnReopenFit.Enabled = true;
                }

                hdnManifestFlag.Value = "";
                #region Nil Flight Depart Check
                try
                {
                    //LoginBL objConfig = new LoginBL();
                    //if (Convert.ToBoolean(objConfig.GetMasterConfiguration("ManifestEmptyFlight")))
                    if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ManifestEmptyFlight")))
                    {
                        res = true;
                        Session["EmptyFlight"] = "M";
                    }
                    //objConfig = null;
                }
                catch (Exception ex)
                {

                }
                #endregion
                BtnList_Click(null, null);


                if (res == true)
                {
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "Flight Manifested.";
                }
            }
            catch (Exception ex)
            {

            }
            return;

        }

        #region btnTesting Event
        protected void btnTesting_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Fill Irregularity Code
        private void FillIrregularityCode()
        {
            LoginBL Bal = new LoginBL();
            DataTable dsIrr = new DataTable("Exp_Manifest_FillIrregularityCode_dsIrr");

            try
            {
                dsIrr = Bal.LoadSystemMasterDataNew("IC").Tables[0];
                if (dsIrr != null && dsIrr.Rows.Count > 0)
                {
                    ddlIrregularityCode.Items.Clear();
                    //drpWWR.Items.Add("Select");
                    ddlIrregularityCode.DataSource = dsIrr;
                    ddlIrregularityCode.DataTextField = "IrregularityCode";
                    ddlIrregularityCode.DataValueField = "ID";
                    ddlIrregularityCode.DataBind();
                    ddlIrregularityCode.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Bal = null;
                dsIrr = null;
            }
        }
        #endregion
              

        #region GenerateSITAHeader
        private void GenerateSITAHeader(string receiverSITAID)
        {
            try
            {
                receiverSITAID = receiverSITAID.Replace(" ", ",");
                string[] sitareciver = receiverSITAID.Split(',');
                string Header = "=PRIORITY" + "\r\n" + "QP" + "\r\n" + "=DESTINATION TYPE B" + "\r\n";
                for (int i = 0; i < sitareciver.Length; i++)
                {
                    Header = Header + "STX," + sitareciver[i] + "\r\n";
                }
                Header = Header + "=SUBJ" + "\r\n" + "CIMP Message" + "\r\n" + "=TEXT";
                txtSITAHeader.Text = Header;
                txtSITAHeader.Visible = true;
                
            }
            catch (Exception ex) { }
        }
        #endregion

        protected void btnSitaUpload_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                if (txtMessageBody.Text.Length > 0 || txtSITAHeader.Text.Length > 0)
                {
                    FTP.SaveSITAMsg(txtSITAHeader.Text.ToString() + "\r\n" + txtMessageBody.Text.ToString(), lblMsg.Text.ToString() + "File" + System.DateTime.Now.ToString("hhmmss"));
                    cls_BL.addMsgToOutBox("SITA:" + lblMsg.Text.ToString(), txtSITAHeader.Text.ToString() + "\r\n" + txtMessageBody.Text.ToString(), "", "SITAFTP");
                }
            }
            catch (Exception ex) { }
        }

        protected void btnFTPUpload_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                if (txtMessageBody.Text.Length > 0)
                {
                    FTP.Saveon72FTP(txtMessageBody.Text.ToString(), lblMsg.Text.ToString() + "File" + System.DateTime.Now.ToString("hhmmss"));
                }
            }
            catch (Exception ex) { }
        }

        protected void ClearSession()
        {
            Session["DataTableTemp"] = null;
            Session["ManifestGridData"] = null;
            Session["ULDData"] = null;
            Session["IsDeparted"] = null;
            Session["IsPreManifested"] = null;
            Session["IsManifested"] = null;
            Session["MD"] = null;
            Session["AT"] = null;
            Session["dsLoad"] = null;
            Session["ULDAWBData"] = null;
            Session["AWBdata"] = null;
            Session["AddToManifestAWBdata"] = null;
            Session["MsgConf"] = null;
            Session["FFM"] = null;
            Session["EmptyFlight"] = null;
        }

        #region UpdatePartnerCode
        private void UpdatePartnerCode(int rowindex)
        {
            try
            {
                string errormessage = "";
                DataSet dsResult = new DataSet("Exp_Manifest_UpdatePartnerCode_dsResult");

                if (CommonUtility.PartnerMaster == null)
                {
                    BookingBAL objBookingBal = new BookingBAL();
                    CommonUtility.PartnerMaster = objBookingBal.GetAvailabePartners();
                    objBookingBal = null;
                }

                dsResult = CommonUtility.PartnerMaster;

                //if (objBLL.GetAvailabePartners(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), dtCurrentDate, ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartnerType")).Text.ToUpper(), ref dsResult, ref errormessage))
                //{
                    if (dsResult != null)
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            DropDownList ddl = ((DropDownList)(grdRouting.Rows[rowindex].FindControl("ddlPartner")));
                            TextBox txtdest = ((TextBox)(grdRouting.Rows[rowindex].FindControl("txtFltDest")));

                            if (dsResult != null)
                            {
                                if (dsResult.Tables.Count > 0)
                                {
                                    ddl.Items.Clear();
                                    //ddl.Items.Add("Select");
                                    for (int j = 0; j < dsResult.Tables.Count; j++)
                                    {
                                        if (dsResult.Tables[j].Rows.Count > 0)
                                        {
                                            for (int k = 0; k < dsResult.Tables[j].Rows.Count; k++)
                                            {
                                                ddl.Items.Add(dsResult.Tables[j].Rows[k][0].ToString());
                                            }
                                        }
                                    }
                                    try
                                    {
                                        if (ddl.Items.Count < 1)
                                        {
                                            ddl.Items.Add("Select");
                                        }
                                        ddl.Items.Add("Other");
                                    }
                                    catch (Exception ex) { }
                                }
                            }

                        }
                    }
                //}

            }
            catch (Exception ex) { }
        }
        #endregion

        #region GetFlightRouteData
        private void GetFlightRouteData(int rowindex)
        {
            try
            {
                string strPartnerCode = ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartner")).Text.Trim();

                string errormessage = "";
                // DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, hr, min, AllowedHr, ref errormessage);
                DataSet dsresult = new DataSet("Exp_Manifest_GetFlightRouteData_dsresult");
                dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, 0, 0, 0, ref errormessage, strPartnerCode);

                if (dsresult != null && dsresult.Tables.Count != 0)
                {
                    DataSet ds = new DataSet("Exp_Manifest_GetFlightRoutData_ds");
                    ds = (DataSet)Session["Flt"];
                    if (ds != null)
                    {
                        string name = "Table" + rowindex.ToString();
                        try
                        {
                            if (ds.Tables.Count > rowindex)
                            {
                                try
                                {
                                    if (ds.Tables[name] != null && ds.Tables[name].Rows.Count > 0)
                                    {
                                        ds.Tables.Remove(name);
                                        DataTable dt = new DataTable("Exp_Manifest_GetFlightRouteDetails_dt");
                                        dt = dsresult.Tables[0].Copy();
                                        dt.TableName = name;
                                        ds.Tables.Add(dt);
                                        ds.AcceptChanges();
                                        Session["Flt"] = ds.Copy();
                                    }
                                }
                                catch (Exception ex) { }

                            }
                            else if (ds.Tables.Count == 1)
                            {
                                Session["Flt"] = dsresult.Copy();
                            }
                        }
                        catch (Exception ex) { }
                    }
                    else
                    {
                        Session["Flt"] = dsresult.Copy();
                    }
                    DataRow row = dsresult.Tables[0].NewRow();
                    row["FltNumber"] = "";
                    row["ArrTime"] = "";

                    dsresult.Tables[0].Rows.Add(row);

                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataTextField = "FltNumber";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataValueField = "ArrTime";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataSource = dsresult.Tables[0].Copy();
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataBind();

                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).SelectedIndex = dsresult.Tables[0].Rows.Count - 1;

                    //HidProcessFlag.Value = "1";
                }
                else
                {
                    LBLRouteStatus.Text = "no record found";
                    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).Items.Clear();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                    return;
                }

            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region ddlPartnerType_SelectionChange
        protected void ddlPartnerType_SelectionChange(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    if (((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")) == ((DropDownList)sender))
                    {
                        rowindex = i;
                    }
                }
                UpdatePartnerCode(rowindex);
                GetFlightRouteData(rowindex);


            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region ddlPartner_SelectionChange
        protected void ddlPartner_SelectionChange(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {

                    if ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner") == (DropDownList)sender)
                    {
                        DropDownList ddl = (DropDownList)grdRouting.Rows[i].FindControl("ddlPartner");
                        DropDownList drop = (DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum");
                        TextBox txtbox = (TextBox)grdRouting.Rows[i].FindControl("txtFlightID");

                        if (ddl.Text.ToString().Equals("other", StringComparison.OrdinalIgnoreCase))
                        {
                            drop.Visible = false;
                            txtbox.Visible = true;
                        }
                        else
                        {
                            drop.Visible = true;
                            txtbox.Visible = false;
                        }
                        rowindex = i;
                        break;
                    }
                }
                GetFlightRouteData(rowindex);
                ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy");
                //((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).Text = "";
            }
            catch (Exception ex) { }
        }
        #endregion

        #region Load Airline code
        public void LoadAirlineCode(string filter)
        {
            try
            {
                //DataSet ds = objBLL.GetPartnerType(chkInterline.Checked);
                //DataSet ds = objBLL.GetPartnerType(true);
                DataSet ds = new DataSet("Exp_Manifest_LoadAirlineCode_ds");

                if (CommonUtility.PartnerTypeMaster == null)
                {
                    BookingBAL objBookingBal = new BookingBAL();
                    CommonUtility.PartnerTypeMaster = objBookingBal.GetPartnerType(true);
                    objBookingBal = null;
                }

                ds = CommonUtility.PartnerTypeMaster;

                DropDownList ddl = new DropDownList();
                TextBox txtdest = new TextBox();
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    ddl = ((DropDownList)(grdRouting.Rows[i].FindControl("ddlPartnerType")));
                    txtdest = ((TextBox)(grdRouting.Rows[i].FindControl("txtFltDest")));
                    if (txtdest.Text.Length < 1 || txtdest.Text == "")//(ddl.Text.Equals("SG",StringComparison.OrdinalIgnoreCase)|| ddl.Text=="")
                    {
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                ddl.Items.Clear();
                                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                {
                                    ddl.Items.Add(ds.Tables[0].Rows[k][0].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        public DataSet GetFlightList(string Origin, string Dest, string strdate, int hr, int min, int AllowedHr, ref string errormessage, string PartnerCode)
        {
            DataSet dsResult = new DataSet("Exp_Manifest_GetFlightList_dsResult");
            bool blnSelfAirline = false;
            DataSet dsAWBPrefixs = new DataSet("Exp_Manifest_GetFlightList_dsAWBPrefixs");
            dsAWBPrefixs = CommonUtility.AWBPrefixMaster;

            if (PartnerCode != "")
            {
                if (dsAWBPrefixs != null && dsAWBPrefixs.Tables.Count > 0 && dsAWBPrefixs.Tables[0].Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < dsAWBPrefixs.Tables[0].Rows.Count; intCount++)
                    {
                        if (PartnerCode.ToUpper() == Convert.ToString(dsAWBPrefixs.Tables[0].Rows[intCount]["AirlinePrefix"]).ToUpper())
                        {
                            blnSelfAirline = true;
                            dsAWBPrefixs = null;
                            break;
                        }
                    }
                }
            }

            if (strdate.Trim() == "")
            {
                if (blnSelfAirline)
                {
                    if (new ShowFlightsBAL().GetFlightListforDay(Origin, Dest, ref dsResult, ref errormessage, dtCurrentDate, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (new ShowFlightsBAL().GetPartnerFlightList(Origin, Dest, ref dsResult, ref errormessage, dtCurrentDate, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {

                string[] splitdate = strdate.Split(new char[] { '/' });
                int year = int.Parse(splitdate[2]);
                int month = int.Parse(splitdate[1]);
                int day = int.Parse(splitdate[0]);
                DateTime dt = new DateTime(year, month, day);

                int diff = (dt - dtCurrentDate.Date).Days;

                if (blnSelfAirline)
                {
                    if (new ShowFlightsBAL().GetFlightListforDay(Origin, Dest, ref dsResult, ref errormessage, dt, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (new ShowFlightsBAL().GetPartnerFlightList(Origin, Dest, ref dsResult, ref errormessage, dt, PartnerCode))
                    {
                        FormatRecords(Origin, Dest, ref dsResult, hr, min, AllowedHr);
                        return dsResult;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }

        #region AutoMessagingFunction
        private void AutoFFMMessage()
        {
            try
            {
                string Message = cls_BL.EncodeFFM(Session["POLairport"].ToString(), Session["FltNumber"].ToString(), Session["Fltdate"].ToString());
                if (Message.Length > 3)
                {
                    if (txtMessageBody.Text.Length > 0 || txtSITAHeader.Text.Length > 0)
                    {
                        FTP.SaveSITAMsg(txtSITAHeader.Text.ToString() + "\r\n" + Message,  "FFMFile" + System.DateTime.Now.ToString("hhmmss"));
                        cls_BL.addMsgToOutBox("SITA:" + "FFM", txtSITAHeader.Text.ToString() + "\r\n" + txtMessageBody.Text.ToString(), "", "SITAFTP");
                    }
                    else
                    {
                        cls_BL.addMsgToOutBox("FFM", Message, "", "");
                    }

                }
            }
            catch (Exception ex) { }
        }
        #endregion

        public void LoadSystemParameters()
        {
            //LoginBL objBL = new LoginBL();
            string SmartKargoInstance = string.Empty;
            string ManifestPartner = string.Empty;

            //if (CommonUtility.SmartKargoInstance == null || Convert.ToString(Session["SKI"]) == "")
            //{
            //    //SmartKargoInstance = objBL.GetMasterConfiguration("SmartKargoInstance");
            //    SmartKargoInstance = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "SmartKargoInstance");

            //    if (SmartKargoInstance != "")
            //        Session["SKI"] = SmartKargoInstance; // Smart Kargo Instance
            //    else
            //        Session["SKI"] = "AR";
            //}

            if (Session["MPA"] == null || Convert.ToString(Session["MPA"]) == "")
            {
                //ManifestPartner = objBL.GetMasterConfiguration("ManifestPartner");
                ManifestPartner = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ManifestPartner");

                if (ManifestPartner != "")
                    Session["MPA"] = Convert.ToBoolean(ManifestPartner); // Manifest Partner Airlines
                else
                    Session["MPA"] = false;
            }
            try
            {
                //if (!Convert.ToBoolean(objBL.GetMasterConfiguration("handleCIMPMsg")))
                if (!Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "handleCIMPMsg")))
                {
                    btnSendFBL.Visible = false;
                    btnSendFDM.Visible = false;
                    btnSendFFM.Visible = false;
                    btnSendFRI.Visible = false;
                    btnSendPRI.Visible = false;
                }
            }
            catch (Exception ex) { }
            //objBL = null;
        }

        public bool ValidateFlightPrtefix()
        {
            DataSet ds = new DataSet("Exp_Manifest_ValidateFlightPrtefix_ds");
            LoginBL Bal = new LoginBL();
            bool IsMatched = false;

            if (CommonUtility.SmartKargoInstance == "GH" && Convert.ToBoolean(Session["MPA"]) == true)
            {
                IsMatched = true;
            }
            else
            {
                ds = Bal.LoadSystemMasterDataNew("DC");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int intCount = 0; intCount < ds.Tables[0].Rows.Count; intCount++)
                    {
                        if (txtFlightCode.Text.Trim().ToUpper() == ds.Tables[0].Rows[intCount]["DesignatorCode"].ToString().ToUpper())
                        {
                            IsMatched = true;
                            break;
                        }
                    }
                }
            }

            return IsMatched;
        }

        public void FillULDDetails()
        {
            txtFlightCode.Text = txtFlightCode.Text.Trim().ToUpper();
            txtFlightID.Text = txtFlightID.Text.Trim().ToUpper();
            string flightID = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
            DateTime FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);
            DataSet dsawb = new DataSet("Exp_Manifest_FillULDDetails_dsawb");

            dsawb = objExpMani.GetAwbTabdetails_GHA(lblDepAirport.Text, flightID, FlightDate, "", "", "");

            if (dsawb != null && dsawb.Tables.Count > 3 && dsawb.Tables[3].Rows.Count > 0)
            {
                ddlULDNo.DataSource = dsawb.Tables[3];
                dsawb.Tables[3].Rows.Add("Select", 0, 0, 0);
                ddlULDNo.DataTextField = "UldNo";
                ddlULDNo.DataValueField = "UldNo";
                ddlULDNo.DataBind();
                ddlULDNo.SelectedValue = "Select";
            }
            else
            {
                ddlULDNo.DataSource = null;
                ddlULDNo.DataBind();
            }
        }

        protected void txtFlightID_TextChanged(object sender, EventArgs e)
        {
            FillULDDetails();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        protected void btnOpsTime_Click(object sender, ImageClickEventArgs e)
        {
            lblStatus.Text = "";
            SaveOperationTime("DEP", true);
        }

        private void HighlightDataGridRows()
        {
            for (int intRow = 0; intRow < gdvULDLoadPlanAWB.Rows.Count; intRow++)
            {
                string FltFlag = ((Label)gdvULDLoadPlanAWB.Rows[intRow].FindControl("lblFltFlag")).Text.Trim();

                if (Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[intRow].FindControl("lblPieces")).Text.Trim()) < 0 ||
                    FltFlag == "")
                {
                    gdvULDLoadPlanAWB.Rows[intRow].BackColor = CommonUtility.ColorHighlightedGrid;
                }
            }

            for (int intRow = 0; intRow < gdvULDLoadPlan.Rows.Count; intRow++)
            {
                if (((Label)gdvULDLoadPlan.Rows[intRow].FindControl("lblFltFlag")).Text.Trim() == "")
                {
                    gdvULDLoadPlan.Rows[intRow].BackColor = CommonUtility.ColorHighlightedGrid;
                }
            }

            for (int intRow = 0; intRow < gdvULDDetails.Rows.Count; intRow++)
            {
                if (((Label)gdvULDDetails.Rows[intRow].FindControl("lblFltFlag")).Text.Trim() == "")
                {
                    gdvULDDetails.Rows[intRow].BackColor = CommonUtility.ColorHighlightedGrid;
                }
            }
        }

        #region Button Send FDM
        protected void btnSendFDM_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                int Count = 0;
                lblStatus.Text = "";
                CustomsImportBAL objCustoms = new CustomsImportBAL();
                //string AWB = ((HyperLink)grdMessages.Rows[0].FindControl("lnkAWBNumber")).Text;
                //string[] AWBDetails = AWB.Split('-');
                //string AWBNumber = AWBDetails[0] + AWBDetails[1];
                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked)
                    {
                        Count++;
                        //object[] QueryValues = new object[3];
                        string Number = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text.Trim();
                        string AWBPrefix = string.Empty;
                        string AWBno = string.Empty;
                        if (Number.Replace("&nbsp;", "") != "")
                        {
                            AWBPrefix = Number.Substring(0, Number.IndexOf('-'));
                            AWBno = Number.Substring(Number.IndexOf('-') + 1, 8);

                            string FlightNo = ((Label)gdvULDDetails.Rows[i].FindControl("lblFltFlag")).Text.Trim();
                            string FlightDate = TextBoxdate.Text;
                            //QueryValues[0] = AWBPrefix +"-"+ AWBno;
                            //QueryValues[1] = FlightNo;
                            //QueryValues[2] = FlightDate;

                            //if (!objCustoms.ValidateCustomsMsg(QueryValues, "FDM"))
                            //{
                            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Customs", "CustomsPopup('FDM','" + AWBPrefix + "-" + AWBno + "','" + FlightNo + "','" + FlightDate + "')", true);
                            //}

                            try
                            {
                                object[] QueryValues = new object[8];


                                QueryValues[0] = AWBno;
                                QueryValues[1] = FlightNo.Substring(0, 2);
                                QueryValues[2] = FlightNo.Substring(2, FlightNo.Length - 2);
                                QueryValues[3] = ((DateTime)Session["IT"]).ToString();
                                QueryValues[4] = Session["UserName"].ToString();
                                QueryValues[5] = AWBPrefix;
                                QueryValues[6] = DateTime.ParseExact(FlightDate, "dd/MM/yyyy", null);
                                object[] QueryVal = { AWBPrefix + "-" + AWBno, FlightNo, TextBoxdate.Text };
                                DataSet dCust = new DataSet("Exp_Manifest_btnSendFDM_dCust");
                                dCust = objCustoms.CheckCustomsAWBAvailability(QueryVal);
                                if (dCust != null)
                                {
                                    StringBuilder sbFDM = objCustoms.EncodingFDMMessage(QueryVal);

                                    if (dCust.Tables[1].Rows[0]["Validate"].ToString() == "True")
                                    {
                                        QueryValues[7] = sbFDM.ToString().ToUpper();
                                        if (objCustoms.UpdateFDMMessage(QueryValues))
                                        {
                                            if (sbFDM != null)
                                            {
                                                if (sbFDM.ToString() != "")
                                                {
                                                    object[] QueryValMail = { "FDM", FlightNo, FlightDate };
                                                    //Getting MailID for FDM Message
                                                    DataSet dMail = new DataSet("Exp_Manifest_btnSendFDM_dMail");
                                                    dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                                    string MailID = string.Empty;
                                                    if (dMail != null)
                                                    {
                                                        MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                    }
                                                    cls_BL.addMsgToOutBox("FDM", sbFDM.ToString().ToUpper(), "", MailID);
                                                }
                                            }
                                            lblStatus.Text = "FDM Message Sent Successfully!";
                                            lblStatus.ForeColor = Color.Green;
                                        }

                                    }
                                }
                            }
                            catch (Exception ex)
                            { }
                        }
                        else
                        {
                            AWBPrefix = "";
                            AWBno = "";
                        }
                    }

                }
                if (Count == 0)
                {
                    lblStatus.Text = "No Records Selected!";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }


            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button Send FRI
        protected void btnSendFRI_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                lblStatus.Text = "";
                CustomsImportBAL objCustoms = new CustomsImportBAL();
                int count = 0;

                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked)
                    {
                        count++;
                        string Number = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text.Trim();
                        string AWBPrefix = string.Empty;
                        string AWBno = string.Empty;
                        if (Number.Replace("&nbsp;", "") != "")
                        {
                            AWBPrefix = Number.Substring(0, Number.IndexOf('-'));
                            AWBno = Number.Substring(Number.IndexOf('-') + 1, 8);

                            string FlightNo = ((Label)gdvULDDetails.Rows[i].FindControl("lblFltFlag")).Text.Trim();
                            string FlightDate = TextBoxdate.Text;

                            try
                            {
                                object[] QueryValues = new object[3];


                                QueryValues[0] = AWBPrefix + "-" + AWBno;
                                QueryValues[1] = FlightNo;
                                QueryValues[2] = FlightDate;


                                DataSet dCust = new DataSet("Exp_Manifest_btnSendFRI_dCust");
                                dCust = objCustoms.CheckCustomsAWBAvailability(QueryValues);
                                if (dCust != null)
                                {
                                    if (dCust.Tables[1].Rows[0]["Validate"].ToString() == "True")
                                    {
                                        StringBuilder sbFRI = objCustoms.EncodingFRIMessage(QueryValues);

                                        object[] QueryVal = { AWBPrefix + AWBno, 1, FlightNo, FlightDate, sbFRI.ToString().ToUpper() };

                                        if (objCustoms.UpdateFRIMessage(QueryVal))
                                        {
                                            if (sbFRI != null)
                                            {
                                                if (sbFRI.ToString() != "")
                                                {
                                                    object[] QueryValMail = { "FRI", FlightNo, FlightDate };
                                                    //Getting MailID for FRI Message
                                                    DataSet dMail = new DataSet("Exp_Manifest_btnSendFRI_dMail");
                                                    dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                                    string MailID = string.Empty;
                                                    if (dMail != null)
                                                    {
                                                        MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                    }
                                                    cls_BL.addMsgToOutBox("FRI", sbFRI.ToString().ToUpper(), "", MailID);
                                                }
                                            }
                                            lblStatus.Text = "FRI Message Sent Successfully!";
                                            lblStatus.ForeColor = Color.Green;
                                        }

                                    }
                                }
                                dCust = null;
                            }
                            catch (Exception ex)
                            { }
                        }
                        else
                        {
                            AWBPrefix = "";
                            AWBno = "";
                        }
                    }

                }
                if (count == 0)
                {
                    lblStatus.Text = "No Records Selected!";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                objCustoms = null;
            }
            catch (Exception ex)
            { }
        }
        #endregion

        //Shubhankar
        protected void GetTailNo()
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet dsTailNo = new DataSet("Exp_Manifest_GetTailNo_dsTailNo");
            try
            {
                DateTime dt = DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null);

                object[] pValue = { txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text.Trim(), Session["Station"].ToString() };
                string[] pName = { "fltNo", "FlightDate", "stn" };
                SqlDbType[] pType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };

                dsTailNo = da.SelectRecords("spGetTailNo", pName, pValue, pType);

                if (dsTailNo != null && dsTailNo.Tables[0].Rows.Count > 0)
                {
                    //ddlTailNo.Items.Clear();
                    ddlTailNo.DataSource = null;
                    ddlTailNo.DataBind();
                    ddlTailNo.DataSource = dsTailNo.Tables[0];
                    ddlTailNo.DataTextField = "TailNo";
                    ddlTailNo.DataBind();
                    ddlTailNo.Items.Insert(0, "Select");
                }

            }
            catch (Exception ex)
            { }
        }

        private void CheckPartnerFlightSupport()
        {
            //LoginBL objBL = new LoginBL();
            try
            {
                //if (!Convert.ToBoolean(objBL.GetMasterConfiguration("SupportPartnerFlight")))
                if (!Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "SupportPartnerFlight")))
                {
                    for (int intCount = 0; intCount < grdRouting.Rows.Count; intCount++)
                    {
                        ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlPartnerType"))).Enabled = false;
                        ((DropDownList)(grdRouting.Rows[intCount].FindControl("ddlPartner"))).Enabled = false;
                    }
                }
            }
            catch (Exception ex) { }
            finally
            {
                //objBL = null;
            }
        }

        private void CheckPartnerStatus()
        {
            //LoginBL objBL = new LoginBL();
            try
            {
                string strOutput = string.Empty;

                if (Session["Man_Flt_Prefix"] == null)
                {
                    //strOutput = objBL.GetMasterConfiguration("SupportPartnerFlight");
                    strOutput = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "SupportPartnerFlight");
                    if (strOutput != "")
                        Session["Man_Flt_Prefix"] = Convert.ToBoolean(strOutput);
                    else
                        Session["Man_Flt_Prefix"] = true;
                }

                if (Session["Man_AWB_Prefix"] == null)
                {
                    //strOutput = objBL.GetMasterConfiguration("AcceptPartnerAWB");
                    strOutput = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "AcceptPartnerAWB");
                    if (strOutput != "")
                        Session["Man_AWB_Prefix"] = Convert.ToBoolean(strOutput);
                    else
                        Session["Man_AWB_Prefix"] = true;
                }

                if (Session["Man_DEMO_System"] == null)
                {
                    //strOutput = objBL.GetMasterConfiguration("DemoInstance");
                    strOutput = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "DemoInstance");
                    if (strOutput != "")
                        Session["Man_DEMO_System"] = Convert.ToBoolean(strOutput);
                    else
                        Session["Man_DEMO_System"] = true;
                }

                if (Convert.ToBoolean(Session["Man_Flt_Prefix"]) == false)
                    txtFlightCode.Enabled = false;

                if (Convert.ToBoolean(Session["Man_AWB_Prefix"]) == false)
                    txtAWBPrefix.Enabled = false;

                //if (Convert.ToBoolean(Session["Man_DEMO_System"]) == false)
                //    btnCommManifest.Visible= false;

            }
            catch (Exception ex) { }
            finally
            {
                //objBL = null;
            }
        }

        #region Button ePouch
        protected void btnePouch_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                Session["ePouchFlightNo"] = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                Session["ePouchFlightDate"] = TextBoxdate.Text.Trim();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('ePouchFlights.aspx','_blank')", true);
            }
            catch (Exception ex)
            { }
        }
        #endregion

        private bool VerifyReopenWithInterval()
        {
            BLExpManifest objBAL = new BLExpManifest();
            DataSet Ds = new DataSet("Exp_Manifest_VerifyReopenWithInterval_Ds");
            int IntervalHrs = 0, DepartureHrs = 0;

            try
            {
                Ds = objBAL.GetFlightReopenTimeInterval(txtFlightCode.Text.Trim() + txtFlightID.Text.Trim(), DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null),
                    Convert.ToString(Session["Station"]), "FlightReOpenInterval");

                if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                {
                    if (Ds.Tables[0].Rows[0]["IntervalHrs"].ToString() == "0")
                        return true;
                    else
                    {
                        IntervalHrs = Convert.ToInt32(Ds.Tables[0].Rows[0]["IntervalHrs"]);
                        DepartureHrs = Convert.ToInt32(Ds.Tables[0].Rows[0]["DepartureHrs"]);

                        DateTime dt = DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null);
                        dt = dt.AddHours(IntervalHrs + DepartureHrs);

                        if (dt > Convert.ToDateTime(Session["IT"]))
                            return true;
                        else
                            return false;
                    }
                }
                else
                    return true;
            }
            catch
            {
                return true;
            }
            finally
            {
                objBAL = null;
                if (Ds != null)
                    Ds.Dispose();
            }
        }

        #region Send PRI Button
        protected void btnSendPRI_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                lblStatus.Text = "";
                ACASBAL objACAS = new ACASBAL();
                int count = 0;

                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked)
                    {
                        count++;
                        string Number = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text.Trim();
                        string AWBPrefix = string.Empty;
                        string AWBno = string.Empty;
                        if (Number.Replace("&nbsp;", "") != "")
                        {
                            AWBPrefix = Number.Substring(0, Number.IndexOf('-'));
                            AWBno = Number.Substring(Number.IndexOf('-') + 1, 8);

                            string FlightNo = ((Label)gdvULDDetails.Rows[i].FindControl("lblFltFlag")).Text.Trim();
                            string FlightDate = TextBoxdate.Text;

                            try
                            {
                                object[] QueryValues = new object[3];


                                QueryValues[0] = AWBPrefix + "-" + AWBno;
                                QueryValues[1] = FlightNo;
                                QueryValues[2] = FlightDate;


                                DataSet dCust = new DataSet("Exp_Manifest_btnSendPRI_dCust");
                                dCust = objACAS.CheckACASAWBAvailability(QueryValues);
                                if (dCust != null)
                                {
                                    if (dCust.Tables[1].Rows[0]["Validate"].ToString() == "True")
                                    {
                                        StringBuilder sbFRI = objACAS.EncodingPRIMessage(QueryValues);

                                        object[] QueryVal = { AWBPrefix + AWBno, 1, FlightNo, FlightDate, sbFRI.ToString().ToUpper() };

                                        if (objACAS.UpdatePRIMessage(QueryVal))
                                        {
                                            if (sbFRI != null)
                                            {
                                                if (sbFRI.ToString() != "")
                                                {
                                                    object[] QueryValMail = { "PRI", FlightNo, FlightDate };
                                                    //Getting MailID for FRI Message
                                                    DataSet dMail = new DataSet("Exp_Manifest_btnSendPRI_dMail");
                                                    dMail = objACAS.GetCustomMessagesMailID(QueryValMail);
                                                    string MailID = string.Empty;
                                                    if (dMail != null)
                                                    {
                                                        MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                    }
                                                    cls_BL.addMsgToOutBox("PRI", sbFRI.ToString().ToUpper(), "", MailID);
                                                }
                                            }
                                            lblStatus.Text = "PRI Message Sent Successfully!";
                                            lblStatus.ForeColor = Color.Green;
                                        }

                                    }
                                }
                                dCust = null;
                            }
                            catch (Exception ex)
                            { }
                        }
                        else
                        {
                            AWBPrefix = "";
                            AWBno = "";
                        }
                    }

                }
                if (count == 0)
                {
                    lblStatus.Text = "No Records Selected!";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                objACAS = null;
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region btnAssignFetchedAWB_Click
        protected void btnAssignFetchedAWB_Click(object sender, EventArgs e)
        {
            if (objExpMani.GetFlightStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                lblDepAirport.Text) == "D")
            {
                BtnList_Click(this, new EventArgs());
                lblStatus.Text = "Flight is departed. Please reopen flight.";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            lblFetchStatus.Text = "";
            try
            {
                int intNewPcs = 0;
                if (!int.TryParse(txtFetchPcs.Text,out intNewPcs))
                {
                    lblFetchStatus.Text = "Please enter valid pieces.";
                    lblFetchStatus.ForeColor = Color.Red;
                    txtFetchPcs.Focus();
                    return;
                }
                decimal decNewWt = 0;
                if (!decimal.TryParse(txtFetchWt.Text, out decNewWt))
                {
                    lblFetchStatus.Text = "Please enter valid weight.";
                    lblFetchStatus.ForeColor = Color.Red;
                    txtFetchWt.Focus();
                    return;
                }
                if (txtFetchAWBPrefix.Text == "" || txtFetchAWBNumber.Text == "" || lblFetchAvlPcs.Text == "" ||
                    lblFetchAvlPcs.Text == "0" || lblFetchAvlWt.Text == "" || lblFetchAvlWt.Text == "0" || 
                    lblFetchFlt.Text == "")
                {
                    lblFetchStatus.Text = "Please fetch AWB information to Assign.";
                    lblFetchStatus.ForeColor = Color.Blue;
                    return;
                }
                int intOldPcs = 0;
                if (!int.TryParse(lblFetchAvlPcs.Text, out intOldPcs))
                {
                    lblFetchStatus.Text = "Please fetch AWB information to Assign.";
                    lblFetchStatus.ForeColor = Color.Blue;
                    return;
                }
                decimal decOldWt = 0;
                if (!decimal.TryParse(lblFetchAvlWt.Text, out decOldWt))
                {
                    lblFetchStatus.Text = "Please fetch AWB information to Assign.";
                    lblFetchStatus.ForeColor = Color.Blue;
                    return;
                }

                if (intNewPcs == 0 || decNewWt == 0)
                {
                    lblFetchStatus.Text = "Pieces or Weight can not be 0";
                    lblFetchStatus.ForeColor = Color.Red;
                    return;
                }

                if (intNewPcs > intOldPcs || decNewWt > decOldWt)
                {
                    lblFetchStatus.Text = "Pieces or Weight can not be more than available.";
                    lblFetchStatus.ForeColor = Color.Red;
                    return;
                }

                if ((intNewPcs == intOldPcs && decNewWt != decOldWt) || (intNewPcs != intOldPcs && decNewWt == decOldWt))
                {
                    lblFetchStatus.Text = "Pieces & Weight mismatch.";
                    lblFetchStatus.ForeColor = Color.Red;
                    return;
                }


                DataSet dsReassigned = new DataSet("Exp_Manifest_btnAssignFetched_dsReassigned");
                dsReassigned = objExpMani.ReassignFetchedAWB(txtFetchAWBPrefix.Text, txtFetchAWBNumber.Text,
                    lblFetchFlt.Text, lblFetchFltDate.Text, txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                    intOldPcs, decOldWt, intNewPcs, decNewWt, Session["Station"].ToString(),
                    Convert.ToDateTime(Session["IT"]),Session["UserName"].ToString());
                if (dsReassigned != null)
                {
                    if (dsReassigned.Tables != null && dsReassigned.Tables.Count > 0)
                    {
                        if (dsReassigned.Tables[0].Rows.Count > 0)
                        {
                            btnFetchClear_Click(null, null);
                            if (dsReassigned.Tables[0].Rows[0][0].ToString() == "")
                            {
                                BtnList_Click(null,null);
                                lblFetchStatus.Text = "AWB Assigned to current flight successfully !";
                                lblFetchStatus.ForeColor = Color.Green;
                            }
                            else
                            {
                                lblFetchStatus.Text = dsReassigned.Tables[0].Rows[0][0].ToString();
                                lblFetchStatus.ForeColor = Color.Red;
                            }
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            lblFetchStatus.Text = "AWB could not be reassigned. Please try again later.";
            lblFetchStatus.ForeColor = Color.Red;
        }
        #endregion btnAssignFetchedAWB_Click

        #region btnFetchAWB_Click
        protected void btnFetchAWB_Click(object sender, EventArgs e)
        {
            DataSet dsFetchAWB = new DataSet("Exp_Manifest_btnFetchAWB_dsFetchAWB");
            lblFetchStatus.Text = "";
            try
            {
                if (txtFetchAWBPrefix.Text == "")
                {
                    lblFetchStatus.Text = "Please enter AWB Prefix.";
                    lblFetchStatus.ForeColor = Color.Blue;
                    txtFetchAWBPrefix.Focus();
                    return;
                }
                if (txtFetchAWBNumber.Text == "")
                {
                    lblFetchStatus.Text = "Please enter AWB Number.";
                    lblFetchStatus.ForeColor = Color.Blue;
                    txtFetchAWBNumber.Focus();
                    return;
                }
                if (txtFlightCode.Text == "")
                {
                    lblFetchStatus.Text = "Please enter Flight Prefix.";
                    lblFetchStatus.ForeColor = Color.Blue;
                    txtFlightCode.Focus();
                    return;
                }

                //Validate flight date.
                DateTime dtFltDate = Convert.ToDateTime(Session["IT"].ToString());
                if (!DateTime.TryParseExact(TextBoxdate.Text,"dd/MM/yyyy",null, System.Globalization.DateTimeStyles.None, 
                    out dtFltDate))
                {
                    lblFetchStatus.Text = "Please select or enter flight date in DD/MM/YYYY format.";
                    lblFetchStatus.ForeColor = Color.Blue;
                    TextBoxdate.Focus();
                    return;
                }

                dsFetchAWB = objExpMani.GetAWBForReassign(txtFetchAWBPrefix.Text, txtFetchAWBNumber.Text,
                    txtFlightCode.Text + "" + txtFlightID.Text, TextBoxdate.Text,Session["Station"].ToString());
                    
                if (dsFetchAWB != null)
                {
                    if (dsFetchAWB.Tables != null && dsFetchAWB.Tables.Count > 0)
                    {
                        if (dsFetchAWB.Tables[0].Rows.Count > 0)
                        {
                            lblFetchFlt.Text = dsFetchAWB.Tables[0].Rows[0]["FlightNo"].ToString();
                            lblFetchFltDate.Text = dsFetchAWB.Tables[0].Rows[0]["FlightDate"].ToString();
                            lblFetchAvlPcs.Text = dsFetchAWB.Tables[0].Rows[0]["Pcs"].ToString();
                            lblFetchAvlWt.Text = dsFetchAWB.Tables[0].Rows[0]["Wt"].ToString();
                            txtFetchPcs.Text = dsFetchAWB.Tables[0].Rows[0]["Pcs"].ToString();
                            txtFetchWt.Text = dsFetchAWB.Tables[0].Rows[0]["Wt"].ToString();
                            txtFetchAWBPrefix.Enabled = false;
                            txtFetchAWBNumber.Enabled = false;
                            return;
                        }
                        if (dsFetchAWB.Tables.Count > 1)
                        {   //AWB found but not available for assignment.
                            lblFetchFlt.Text = dsFetchAWB.Tables[1].Rows[0]["FlightNo"].ToString();
                            lblFetchFltDate.Text = dsFetchAWB.Tables[1].Rows[0]["FlightDate"].ToString();
                            lblFetchAvlPcs.Text = dsFetchAWB.Tables[1].Rows[0]["Pcs"].ToString();
                            lblFetchAvlWt.Text = dsFetchAWB.Tables[1].Rows[0]["Wt"].ToString();
                            txtFetchPcs.Text = dsFetchAWB.Tables[1].Rows[0]["Pcs"].ToString();
                            txtFetchWt.Text = dsFetchAWB.Tables[1].Rows[0]["Wt"].ToString();
                            lblFetchStatus.Text = "AWB not available for assignment.";
                            lblFetchStatus.ForeColor = Color.Blue;
                            txtFetchAWBPrefix.Enabled = false;
                            txtFetchAWBNumber.Enabled = false;
                            btnAssignFetchedAWB.Enabled = false;
                            btnFetchAWB.Enabled = false;
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dsFetchAWB != null)
                {
                    dsFetchAWB.Dispose();
                }
            }
            txtFetchAWBPrefix.Enabled = true;
            txtFetchAWBNumber.Enabled = true;
            lblFetchStatus.Text = "No record found.";
            lblFetchStatus.ForeColor = Color.Blue;
        }
        #endregion btnFetchAWB_Click

        protected void btnFetchClear_Click(object sender, EventArgs e)
        {
            txtFetchAWBPrefix.Text = Convert.ToString(Session["awbPrefix"]);
            txtFetchAWBPrefix.Enabled = true;
            txtFetchAWBNumber.Text = "";
            txtFetchAWBNumber.Enabled = true;
            lblFetchAvlPcs.Text = "0";
            lblFetchAvlWt.Text = "0";
            lblFetchFlt.Text = "";
            lblFetchFltDate.Text = "";
            txtFetchPcs.Text = "";
            txtFetchWt.Text = "";
            lblFetchStatus.Text = "";
            btnAssignFetchedAWB.Enabled = true;
            btnFetchAWB.Enabled = true;
        }

        protected void btnOpsSave_Click(object sender, EventArgs e)
        {
            lblPnlError.Text = "";
            try
            {
                //Validate Date
                DateTime dt = DateTime.Now;
                if (!DateTime.TryParseExact(txtOpsDate.Text + " " + txtOpsTimeHr.Text.PadLeft(2, '0') + ":" +
                    txtOpsTimeMin.Text.PadLeft(2, '0') + ":00",
                    "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out dt))
                {
                    lblPnlError.Text = "Please enter valid Operation Date & Time.";
                    lblPnlError.ForeColor = System.Drawing.Color.Red;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "timePopup", "<SCRIPT LANGUAGE='javascript'>SetOperationTime();</script>", false);
                    return;
                }
                if (dt > Convert.ToDateTime(Session["IT"].ToString()))
                {
                    lblPnlError.Text = "Please enter date and time which is not a future date.";
                    lblPnlError.ForeColor = System.Drawing.Color.Red;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "timePopup", "<SCRIPT LANGUAGE='javascript'>SetOperationTime();</script>", false);
                    return;
                }
                //Validate if Time is out of configured allowed variation time.
                string roles = "";
                roles = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "OpsTimeOverrideAllowedFor");
                if (!roles.Contains(Session["RoleName"].ToString()))
                {
                    int AllowedTimeDiff = 240;
                    roles = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "MaxOpsTimeDiffInMins");
                    if (int.TryParse(roles, out AllowedTimeDiff))
                    {   // Find out allowed time difference.
                        if (DateTime.Compare(Convert.ToDateTime(Session["IT"]).AddMinutes(-1 * AllowedTimeDiff), dt) > 0)
                        {
                            lblPnlError.Text = "You cannot select Time older than current time by " + AllowedTimeDiff.ToString() + " minutes";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "timePopup", "<SCRIPT LANGUAGE='javascript'>SetOperationTime();</script>", false);
                            return;
                        }
                    }
                }

                //Check if data is available for updating time stamp.
                if (Session["listOperationTime"] != null)
                {
                    List<clsOperationTimeStamp> objListOpsTime = (List<clsOperationTimeStamp>)Session["listOperationTime"];
                    if (objListOpsTime != null)
                    {
                        if (objListOpsTime.Count > 0)
                        {
                            ((clsOperationTimeStamp)objListOpsTime[0]).OperationDate = txtOpsDate.Text;
                            ((clsOperationTimeStamp)objListOpsTime[0]).OperationTime = txtOpsTimeHr.Text.PadLeft(2, '0')
                                + ":" + txtOpsTimeMin.Text.PadLeft(2, '0');
                            ((clsOperationTimeStamp)objListOpsTime[0]).UpdatedBy = Session["UserName"].ToString();
                            ((clsOperationTimeStamp)objListOpsTime[0]).UpdatedOn = Convert.ToDateTime(Session["IT"].ToString());
                            //Call function to save time stamp.
                            BALCommon objCommon = new BALCommon();
                            objCommon.SaveOperationalTimeStamp(objListOpsTime);
                            lblPnlError.Text = "Actual operation time saved successfully !";
                            lblPnlError.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "closeTimePopup", "<SCRIPT LANGUAGE='javascript'>CloseWindow();</script>", false);
            }
            catch (Exception ex)
            {
                lblPnlError.Text = "Error: " + ex.Message;
                lblPnlError.ForeColor = System.Drawing.Color.Red;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "timePopup", "<SCRIPT LANGUAGE='javascript'>SetOperationTime();</script>", false);
            }
            finally
            {
                Session["listOperationTime"] = null;
            }
        }

        protected void btnOpsCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Session["listOperationTime"] = null;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "closetimePopup", "<SCRIPT LANGUAGE='javascript'>CloseWindow();</script>", false);
            }
            catch (Exception)
            {
            }
        }

    }
}
