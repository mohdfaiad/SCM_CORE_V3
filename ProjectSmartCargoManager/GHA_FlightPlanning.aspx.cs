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
using Microsoft.Reporting.WebForms;
using System.IO;
using nsBalInterface;
using ProjectSmartCargoManager.ServiceReference4;
using System.Net.Mail;
using System.Net;
using ProjectSmartCargoManager.PaxService;

namespace ProjectSmartCargoManager
{
    public partial class GHA_FlightPlanning : System.Web.UI.Page
    {
        #region Variables
        bool f_FETCHNAVITIRE = true;
        DateTime dtCurrentDate = DateTime.Now;
        BLAssignULD objBuildULD = new BLAssignULD();
        BookingBAL objBLL = new BookingBAL();
        DataTable tabMenifestDetails;
        DataTable tabAssignedBulkAWBDetails;
        BLExpManifest objExpMani = new BLExpManifest();
        //DataTable dtTable1 = new DataTable("FltPlan_dtTable1");
        DataTable dtTable2 = new DataTable("FltPlan_dtTable2");
        DataTable dtTable3 = new DataTable("FltPlan_dtTable3");
        DataTable dtTable4 = new DataTable("FltPlan_dtTable4");
        DataTable dtCartSummary = new DataTable("FltPlan_dtCartSummary");
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataTable dtTable2_NoToc = new DataTable("FltPlan_dtTable2_NoToc");
        DataTable dtTable4_NoToc = new DataTable("FltPlan_dtTable4_NoToc");
        #endregion Variables

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dtCurrentDate = (DateTime)Session["IT"];

                //ChangeButtonStatus();
                if (!IsPostBack)
                {
                    try
                    {
                        Session["FlightPlanning_ShowDest"] = 0;
                        Session["GHA_FlightPlanning_HAWBAddToULD"] = null;
                        Session["GHA_FlightPlanning_HAWBOriginal"] = null;
                        Session["GHA_FlightPlanning_HAWBAddToBulk"] = null;
                        Session["FlightPlan_FlightStatus"] = null;
                        txtFlightCode.Text = Session["AirlinePrefix"] != null ? Session["AirlinePrefix"].ToString() : string.Empty;
                        //txtAWBPrefix.Text = Session["awbPrefix"] != null ? Session["awbPrefix"].ToString() : string.Empty;

                        ClearSession();
                        hfULDNumber.Value = "";
                        hdnManifestFlag.Value = "";

                        string UserName = Session["Username"].ToString();
                        string StationCode = Session["Station"].ToString();

                        lblDepAirport.Text = Convert.ToString(Session["Station"]);

                        TextBoxdate.Text = dtCurrentDate.ToString("dd/MM/yyyy");
                        txtFetchAWBPrefix.Text = Convert.ToString(Session["awbPrefix"]);

                        ShowULDAWBSummary("");

                        HideULDAWBDetails();

                        #region Temp table for AWB Tab
                        DataTable MemDetails = new DataTable("FltPlan_MemDetails");

                        MemDetails.Columns.Add("ULDno");
                        MemDetails.Columns.Add("POU");
                        MemDetails.Columns.Add("TareWeight");

                        Session["FlightPlanning_DataTableTemp"] = (DataTable)MemDetails;
                        #endregion Temp table for AWB Tab


                        #region Temp table for Manifest grid
                        DataTable tabMenifestDetails = new DataTable("FltPlan_tabManifestDetails");

                        tabMenifestDetails.Columns.Add("AWBno");
                        tabMenifestDetails.Columns.Add("ULDno");
                        tabMenifestDetails.Columns.Add("TotalPcs");
                        tabMenifestDetails.Columns.Add("TotalWt");
                        tabMenifestDetails.Columns.Add("AcceptedPcs");
                        tabMenifestDetails.Columns.Add("AcceptedWt");
                        tabMenifestDetails.Columns.Add("BuiltPcs");
                        tabMenifestDetails.Columns.Add("BuiltWt");
                        tabMenifestDetails.Columns.Add("Origin");
                        tabMenifestDetails.Columns.Add("Dest");
                        tabMenifestDetails.Columns.Add("Location");
                        tabMenifestDetails.Columns.Add("AWBLoadingPriority");
                        tabMenifestDetails.Columns.Add("AWBBuilderName");
                        tabMenifestDetails.Columns.Add("FlightExists");
                        tabMenifestDetails.Columns.Add("FltNo");
                        tabMenifestDetails.Columns.Add("FltDate");
                        tabMenifestDetails.Columns.Add("ProductType");
                        tabMenifestDetails.Columns.Add("Description");
                        tabMenifestDetails.Columns.Add("CommodityCode");
                        tabMenifestDetails.Columns.Add("SHC");


                        Session["FlightPlanning_ManifestBulkAWBData"] = (DataTable)tabMenifestDetails;
                        #endregion Temp table for Manifest grid

                        #region Temp table for Assigned Bulk AWB grid
                        DataTable tabBulkAWBDetails = new DataTable("FltPlan_tabBulkAWBDetails");

                        tabBulkAWBDetails.Columns.Add("AWBno");
                        tabBulkAWBDetails.Columns.Add("ULDno");
                        tabBulkAWBDetails.Columns.Add("TotalPcs");
                        tabBulkAWBDetails.Columns.Add("TotalWt");
                        tabBulkAWBDetails.Columns.Add("AcceptedPcs");
                        tabBulkAWBDetails.Columns.Add("AcceptedWt");
                        tabBulkAWBDetails.Columns.Add("BuiltPcs");
                        tabBulkAWBDetails.Columns.Add("BuiltWt");
                        tabBulkAWBDetails.Columns.Add("Origin");
                        tabBulkAWBDetails.Columns.Add("Dest");
                        tabBulkAWBDetails.Columns.Add("Location");
                        tabBulkAWBDetails.Columns.Add("AWBLoadingPriority");
                        tabBulkAWBDetails.Columns.Add("AWBBuilderName");
                        tabBulkAWBDetails.Columns.Add("FlightExists");
                        tabBulkAWBDetails.Columns.Add("CartNumber");
                        tabBulkAWBDetails.Columns.Add("FltNo");
                        tabBulkAWBDetails.Columns.Add("FltDate");
                        tabBulkAWBDetails.Columns.Add("ProductType");
                        tabBulkAWBDetails.Columns.Add("Description");
                        tabBulkAWBDetails.Columns.Add("CommodityCode");
                        tabBulkAWBDetails.Columns.Add("SHC");
                        tabBulkAWBDetails.Columns.Add("DocumentType");


                        Session["FlightPlanning_ManifestBulkAWBData"] = (DataTable)tabBulkAWBDetails;
                        Session["FlightPlanning_PrintLoadPlan"] = (DataTable)tabBulkAWBDetails;
                        #endregion Temp table for Assigned Bulk AWB grid

                        LoadGridRoutingDetail();
                        //AWBDesignators();
                        //Deleted textboxes
                        //txtULDNumber.ReadOnly = false;
                        //txtULDNumber.Text = "";

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>return disable();</script>", false);

                        //Load the parameters which are coming in Querystring.
                        string FlightNo = string.Empty;
                        string FlightDt = string.Empty;

                        if (Request.QueryString["FltNo"] != null)
                            FlightNo = Request.QueryString["FltNo"];

                        if (Request.QueryString["FltDt"] != null)
                            FlightDt = Request.QueryString["FltDt"];

                        if (FlightNo != "")
                        {
                            txtFlightCode.Text = FlightNo.Substring(0, 2);
                            txtFlightID.Text = FlightNo.Replace(txtFlightCode.Text, "");

                            TextBoxdate.Text = FlightDt;
                            BtnList_Click(null, null);
                        }

                        //End
                        try
                        {
                            if (Session["ULDACT"].ToString().ToUpper() == "FALSE")
                            {
                                lbluld.Visible = false;
                                pnlULDList.Visible = false;
                                btnNewULD.Visible = false;
                                Save1.Visible = false;
                                btnDeleteULD.Visible = false;
                                btnPrintWtStmt.Visible = false;
                                btnPrnULDPlan.Visible = false;
                                btnFinalizeULD.Visible = false;
                                btnReOpenULD.Visible = false;
                                btnRemove.Visible = false;
                                BtnAddtoManifest.Visible = false;
                                Pnlgrd.Visible = false;
                                lbluld1.Visible = false;
                                DivBulk.Style.Add("height", "300px");
                                TabContainer2.Style.Add("height", "250px");
                                btnSave.Visible = false;
                                btnUnassign.Visible = false;
                                btnPrintLoadPlan.Visible = false;
                                btnExportToManifest.Visible = false;

                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        LoginBL lBal = new LoginBL();

                        //bool scroll = Convert.ToBoolean(lBal.GetMasterConfiguration("ULDActivate"));
                        //if (scroll == true)
                        //{
                        //    btnPrintLoadPlan1.Visible = false;
                        //    btnExportmft.Visible = false;
                        //    lblAWBHeader.Text = "Accepted Freight";
                        //}
                        //else
                        //{
                        //    btnPrintLoadPlan1.Visible = true;
                        //    btnExportmft.Visible = true;
                        //}


                    }

                    catch (Exception ex)
                    {

                    }
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

        protected void ClearSession()
        {
            Session["FlightPlanning_DataTableTemp"] = null;
            Session["FlightPlanning_dsLoad"] = null;
            Session["FlightPlanning_AWBdata"] = null;
            Session["FlightPlanning_AddToManifestAWBdata"] = null;
            Session["FlightPlanning_ULDList"] = null;
            Session["FlightPlanning_ULDNEW"] = null;
            Session["FlightPlanning_ManifestBulkAWBData"] = null;
            Session["FlightPlanning_GDVBULKAWBDetails"] = null;
            Session["FlightPlanning_fltpln"] = null;

            Session["FlightPlanning_CartList"] = null;
            Session["FlightPlanning_PrintLoadPlan"] = null;
        }

        #region Show ULD AWB Summary
        public void ShowULDAWBSummary(string ULDNumber)
        {
            if (ULDNumber == "")
            {
                return;
            }
            int BulkCount = 0, BulkPcs = 0;
            decimal BulkWt = 0;
            string Bulk = string.Empty;

            for (int intCount = 0; intCount < grdBulkAssignedAWB.Rows.Count; intCount++)
            {
                if (((Label)grdBulkAssignedAWB.Rows[intCount].FindControl("lblBuiltPcs")).Text.Trim() == ULDNumber)
                {
                    BulkCount = BulkCount + 1;
                    if (((Label)grdBulkAssignedAWB.Rows[intCount].FindControl("lblBuiltPcs")).Text.Trim() != "&nbsp;")
                        BulkPcs = BulkPcs + Convert.ToInt32(((Label)grdBulkAssignedAWB.Rows[intCount].FindControl("lblBuiltPcs")).Text.Trim());

                    if (((Label)grdBulkAssignedAWB.Rows[intCount].FindControl("lblBuiltWt")).Text.Trim() != "&nbsp;")
                        BulkWt = BulkWt + Convert.ToDecimal(((Label)grdBulkAssignedAWB.Rows[intCount].FindControl("lblBuiltWt")).Text.Trim());
                }
            }

            lblAWBCnt.Text = BulkCount.ToString();
            lblAWBWt.Text = BulkWt.ToString();
            lblAWBPCS.Text = BulkPcs.ToString();

        }
        #endregion

        #region loadgridroutingdetails
        public void LoadGridRoutingDetail()
        {
            DataTable myDataTable = new DataTable("FltPlan_myDataTable");
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

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
            dr["Location"] = "";// 


            myDataTable.Rows.Add(dr);

            grdRouting.DataSource = null;
            grdRouting.DataSource = myDataTable;
            grdRouting.DataBind();

            //((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltOrig")).ReadOnly = true;

            DataSet dsRoutDetails = new DataSet("FltPlan_dsRoutDetails");
            dsRoutDetails.Tables.Add(myDataTable);
            Session["FlightPlanning_dsRoutDetails"] = dsRoutDetails.Copy();

            // SetRouteGridValues(myDataTable);
        }
        #endregion loadgridroutingdetails

        protected void txtFlightID_TextChanged(object sender, EventArgs e)
        {
            //txtFlightID.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            //BtnList_Click(null, null);
        }

        protected void BtnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            lblFetchStatus.Text = "";
            lblULDStatus.Text = "";
            Session["GHA_FlightPlanning_HAWBAddToULD"] = null;
            Session["GHA_FlightPlanning_HAWBOriginal"] = null;
            Session["GHA_FlightPlanning_HAWBAddToBulk"] = null;
            Session["FlightPlan_FlightStatus"] = null;
            gdvULDLoadPlanAWB.DataSource = null;
            gdvULDLoadPlanAWB.DataBind();

            DataSet dsFlightDetails = new DataSet("FltPlan_dsFlightDetails");
            DataSet fltPOU = new DataSet("FltPlan_fltPOU");
            try
            {
                txtFlightCode.Text = txtFlightCode.Text.Trim();
                txtFlightID.Text = txtFlightID.Text.Trim();
                string FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                DateTime dt = new DateTime();
                string DepartureAirport = lblDepAirport.Text;
                //DateTime dt1 = new DateTime();
                dt = DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null);
                dsFlightDetails = objExpMani.GetFlightTabdetails(lblDepAirport.Text, FLTno, dt);//fill labels with source & destiny

                string strdestination = string.Empty;

                // Appended 25Apr
                fltPOU = objExpMani.GetPOUAirlineSchedule(FLTno, DepartureAirport, dt);
                string RouteDetails = string.Empty;

                if (dsFlightDetails != null && dsFlightDetails.Tables.Count > 0 && dsFlightDetails.Tables[0].Rows.Count > 0)
                    RouteDetails = dsFlightDetails.Tables[0].Rows[0][0].ToString() + "-";

                if (fltPOU != null && fltPOU.Tables.Count > 0 && fltPOU.Tables[0].Rows.Count > 0)
                {
                    for (int route = 0; route < fltPOU.Tables[0].Rows.Count; route++)
                    {
                        RouteDetails = RouteDetails + fltPOU.Tables[0].Rows[route][0] + "-";
                        strdestination = fltPOU.Tables[0].Rows[route][0].ToString();
                        Session["FlightPlanning_Dest"] = strdestination;

                        if (route == 0)
                            Session["FtlPOU"] = fltPOU.Tables[0].Rows[route][0];
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
                    txtTailNo.Text = dsFlightDetails.Tables[0].Rows[0]["TailNo"].ToString();
                    lblRoute.Visible = true;
                    lblDate.Visible = true;
                    btnFltRoute.Text = lblRoute.Text;
                    btnFlightNumber.Text = txtFlightCode.Text + txtFlightID.Text;
                    btnFltRoute.Visible = true;
                }
                if (lblRoute.Text == "")
                {
                    lblStatus.Text = "Flight details not found. Please verify Flight #, Date and Dep. Airport";
                    lblStatus.ForeColor = Color.Red;
                    btnFltRoute.Visible = true;
                    return;
                }

                BindAWBLoadPlanULD(Convert.ToInt16(Session["FlightPlanning_ShowDest"]));
                BindAWBManifestCart("", lblDepAirport.Text, txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text);

                txtFlightID.Enabled = false;
                TextBoxdate.Enabled = false;
                //btnNewULD.Visible = true;
                btnPrintWtStmt.Enabled = true;

                if (Convert.ToInt16(Session["FlightPlanning_ShowDest"]) == 1)
                {
                    btnReAssign.Enabled = false;
                    btnFltRoute.BackColor = Color.Blue;
                    btnFltRoute.ForeColor = Color.White;
                    btnFlightNumber.BackColor = Color.Transparent;
                    btnFlightNumber.ForeColor = Color.Black;
                }
                else
                {
                    btnReAssign.Enabled = true;
                    btnFlightNumber.BackColor = Color.Blue;
                    btnFlightNumber.ForeColor = Color.White;
                    btnFltRoute.BackColor = Color.Transparent;
                    btnFltRoute.ForeColor = Color.Black;
                }

                //Visibility of show notes icon--SWati
                #region shownotes

                bool flag = false;
                flag = CommonUtility.ShowNotes("", "", FLTno, TextBoxdate.Text.Trim());
                imgNotebtn.Visible = flag;

                #endregion show notes

            }
            catch (Exception)
            { }

        }

        #region Bind Load Plan AWB
        protected void BindAWBLoadPlanULD(int FlightFilter)
        {
            DataSet dsawb = new DataSet("FltPlan_dsawb");
            DataTable MemDetails = new DataTable("FltPlan_MembDetails1");
            DataTable ULDListDetails = new DataTable("FltPlan_ULDListDetails");
            DataTable CartListDetails = new DataTable("FltPlan_CartListDetails");

            try
            {

                string flightID;
                if (txtFlightID.Text.Trim() != "")
                    flightID = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                else
                    flightID = "";

                string FlightDate = TextBoxdate.Text;
                Session["FlightPlanning_FltDet"] = flightID + "-" + FlightDate; //Shubh
                string AWBPrefix = txtAWBPrefix.Text.Trim();
                string AWBNumber = txtAWBNo.Text.Trim();
                string ULDNumber = "";
                bool ShowByDest = false;    //This is used to fetch AWBs based on Flight or Destination.
                if (FlightFilter == 0)
                {
                    ShowByDest = false;
                }
                else
                {
                    ShowByDest = true;
                }

                dsawb = objBuildULD.GetAWBLoadPlanULDDetails(lblDepAirport.Text, flightID, FlightDate, AWBPrefix, AWBNumber,
                    ULDNumber, ShowByDest);
                //  dsawb = objExpMani.GetAwbTabdetails(Destination.ToString(), flightID);//further research to be done
                if (FlightFilter == 0)
                {
                    Session["FlightPlanning_dsLoad"] = dsawb;
                    Session["FlightPlanning_ULDNEW"] = dsawb.Tables[2];
                }
                MemDetails.Columns.Add("lblAWBno");
                MemDetails.Columns.Add("lblPieces");
                MemDetails.Columns.Add("lblWeight");
                MemDetails.Columns.Add("lblBuiltPcs");
                MemDetails.Columns.Add("lblBuiltWt");
                MemDetails.Columns.Add("txtRemainingPcs");
                MemDetails.Columns.Add("txtRemainingWt");
                MemDetails.Columns.Add("lblLocation");
                MemDetails.Columns.Add("BookedPieces");
                MemDetails.Columns.Add("BookedWeight");
                MemDetails.Columns.Add("lblFlightExists");
                MemDetails.Columns.Add("FltNo");
                MemDetails.Columns.Add("FltDate");
                MemDetails.Columns.Add("AWBNo");
                MemDetails.Columns.Add("ProductType");
                MemDetails.Columns.Add("Description");
                MemDetails.Columns.Add("CommodityCode");
                MemDetails.Columns.Add("SHC");
                //MemDetails.Rows.Add(MemDetails.NewRow());
                if (dsawb != null)
                {
                    for (int j = 0; j < dsawb.Tables[0].Rows.Count; j++)
                    {
                        DataRow l_Datarow = MemDetails.NewRow();
                        MemDetails.Rows.Add(l_Datarow);
                        MemDetails.Rows[j]["FltNo"] = dsawb.Tables[0].Rows[j]["FltNo"];
                        MemDetails.Rows[j]["FltDate"] = dsawb.Tables[0].Rows[j]["FltDate"];
                        MemDetails.Rows[j]["AWBNo"] = dsawb.Tables[0].Rows[j]["AWBNo"];
                        MemDetails.Rows[j]["SHC"] = dsawb.Tables[0].Rows[j]["SHC"];
                    }

                    gdvULDLoadPlanAWB.DataSource = (DataTable)MemDetails;
                    gdvULDLoadPlanAWB.DataBind();

                }
                if (dsawb.Tables[0].Rows.Count <= 0)
                    btnReAssign.Enabled = false;


                if (dsawb != null)
                {
                    for (int i = 0; i < dsawb.Tables[0].Rows.Count; i++)
                    {
                        //  tabMenifestDetails.Rows.Add(l_Datarow);
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAWBno")).Text = dsawb.Tables[0].Rows[i]["AWBNumber"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblPieces")).Text = dsawb.Tables[0].Rows[i]["TotalPieces"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblWeight")).Text = dsawb.Tables[0].Rows[i]["TotalWeight"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblBuiltPcs")).Text = dsawb.Tables[0].Rows[i]["BuiltPcs"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblBuiltWt")).Text = dsawb.Tables[0].Rows[i]["BuiltWt"].ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[i].FindControl("txtRemainingPcs")).Text = dsawb.Tables[0].Rows[i]["RemainingPcs"].ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[i].FindControl("txtRemainingWt")).Text = dsawb.Tables[0].Rows[i][6].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblLocation")).Text = dsawb.Tables[0].Rows[i]["Location"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblBookedPcs")).Text = dsawb.Tables[0].Rows[i]["BookedPieces"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblBookedWt")).Text = dsawb.Tables[0].Rows[i]["BookedWeight"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblFlightExists")).Text = dsawb.Tables[0].Rows[i]["FlightExists"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblProductType")).Text = dsawb.Tables[0].Rows[i]["ProductType"].ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[i].FindControl("lblCommodityDesc")).Text = dsawb.Tables[0].Rows[i]["description"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblCommodityCode")).Text = dsawb.Tables[0].Rows[i]["CommodityCode"].ToString();
                        if (dsawb.Tables[0].Rows[i]["FlightExists"].ToString() == "FN")
                        {
                            gdvULDLoadPlanAWB.Rows[i].BackColor = CommonUtility.ColorHighlightedGrid;
                        }
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblFlightNumber")).Text = dsawb.Tables[0].Rows[i]["FltNo"].ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[i].FindControl("lblShipperName")).Text = dsawb.Tables[0].Rows[i]["ShipperName"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAcceptedOn")).Text = dsawb.Tables[0].Rows[i]["AcceptedOn"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblFltDate")).Text = dsawb.Tables[0].Rows[i]["FltDate"].ToString();

                        //Deleted textboxes
                        //txtOrigin.Text = dsawb.Tables[0].Rows[i][7].ToString();
                        //txtDestination.Text = dsawb.Tables[0].Rows[i][8].ToString();
                    }
                    Session["FlightPlanning_AWBdata"] = dsawb.Tables[0];
                    Session["FlightPlanning_AddToManifestAWBdata"] = dsawb.Tables[0];

                }

                #region Show ULD list

                //To show ULD list
                ULDListDetails.Columns.Add("Uldno");
                ULDListDetails.Columns.Add("ULDOrigin");
                ULDListDetails.Columns.Add("ULDDest");
                ULDListDetails.Columns.Add("AWBCount");
                ULDListDetails.Columns.Add("Location");
                ULDListDetails.Columns.Add("LoadingPriority");
                ULDListDetails.Columns.Add("TareWt");
                ULDListDetails.Columns.Add("ULDWt");
                ULDListDetails.Columns.Add("ScaleWeight");
                ULDListDetails.Columns.Add("ContainerType");
                ULDListDetails.Columns.Add("ULDStatus");
                ULDListDetails.Columns.Add("ULDBuilderName");
                ULDListDetails.Columns.Add("IsReceived");
                ULDListDetails.Columns.Add("DollyWt");
                ULDListDetails.Columns.Add("AWBNumber");

                Session["FlightPlanning_ULDList"] = ULDListDetails;

                if (dsawb != null)
                {
                    if (dsawb.Tables.Count > 0)
                    {
                        if (dsawb.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsawb.Tables[1].Rows.Count; i++)
                            {
                                DataRow l_Datarow = ULDListDetails.NewRow();
                                l_Datarow["ULDno"] = dsawb.Tables[1].Rows[i]["Uldno"].ToString();
                                l_Datarow["ULDOrigin"] = dsawb.Tables[1].Rows[i]["ULDOrigin"].ToString();
                                l_Datarow["ULDDest"] = dsawb.Tables[1].Rows[i]["ULDDest"].ToString();
                                l_Datarow["AWBCount"] = dsawb.Tables[1].Rows[i]["AWBCount"].ToString();
                                l_Datarow["Location"] = dsawb.Tables[1].Rows[i]["Location"].ToString();
                                l_Datarow["LoadingPriority"] = dsawb.Tables[1].Rows[i]["LoadingPriority"].ToString();
                                l_Datarow["TareWt"] = dsawb.Tables[1].Rows[i]["TareWt"].ToString();
                                l_Datarow["ULDWt"] = dsawb.Tables[1].Rows[i]["ULDWt"].ToString();
                                l_Datarow["ScaleWeight"] = dsawb.Tables[1].Rows[i]["ScaleWeight"].ToString();
                                l_Datarow["ContainerType"] = dsawb.Tables[1].Rows[i]["ContainerType"].ToString();
                                l_Datarow["ULDStatus"] = dsawb.Tables[1].Rows[i]["ULDStatus"].ToString();
                                l_Datarow["ULDBuilderName"] = dsawb.Tables[1].Rows[i]["ULDBuilderName"].ToString();
                                l_Datarow["IsReceived"] = dsawb.Tables[1].Rows[i]["IsReceived"].ToString();
                                l_Datarow["DollyWt"] = dsawb.Tables[1].Rows[i]["DollyWt"].ToString();
                                l_Datarow["AWBNumber"] = dsawb.Tables[1].Rows[i]["AWBNumber"].ToString();

                                ULDListDetails.Rows.Add(l_Datarow);
                            }
                            grdULDList.DataSource = dsawb.Tables[1];
                            grdULDList.DataBind();
                            Session["FlightPlanning_ULDList"] = ULDListDetails;

                            if (hfULDNumber.Value == "" && hfCartNumber.Value == "")
                            {
                                if (dsawb.Tables.Count > 2 && dsawb.Tables[3].Rows.Count == 0)
                                {
                                    pnlBulkAssignedAWB.Visible = false;
                                    grdBulkAssignedAWB.DataSource = null;
                                    grdBulkAssignedAWB.DataBind();

                                    //((CheckBox)grdULDList.Rows[0].FindControl("rdULDList")).Checked = true;
                                    string uld = ULDListDetails.Rows[0]["Uldno"].ToString();
                                    BindAWBManifestULD(uld, lblDepAirport.Text, flightID, FlightDate);

                                }
                            }
                            else
                            {

                                for (int t = 0; t < dsawb.Tables[3].Rows.Count; t++)
                                {
                                    if (grdULDList.Rows.Count > t)
                                        ((CheckBox)grdULDList.Rows[t].FindControl("rdULDList")).Checked = false;
                                }

                                BindAWBManifestULD(hfULDNumber.Value, lblDepAirport.Text, flightID, FlightDate);

                                //for (int t = 0; t < dsawb.Tables[1].Rows.Count; t++)
                                //{
                                //    if (dsawb.Tables[1].Rows[t]["Uldno"].ToString() == hfULDNumber.Value)
                                //    {
                                //        ((CheckBox)grdULDList.Rows[t].FindControl("rdULDList")).Checked = true;
                                //    }
                                //}

                                HideCartAWBDetails();

                            }

                            ShowULDAWBSummary("");

                        }
                        else //If no ULD exists
                        {
                            hfULDNumber.Value = "";

                            grdULDList.DataSource = null;
                            grdULDList.DataBind();
                        }
                    }
                }

                if (dsawb != null)
                {
                    if (dsawb.Tables.Count > 0)
                    {
                        if (dsawb.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsawb.Tables[1].Rows.Count; i++)
                            {
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDNo")).Text = dsawb.Tables[1].Rows[i][0].ToString();
                                ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text = dsawb.Tables[1].Rows[i][0].ToString();
                                //((TextBox)grdULDList.Rows[i].FindControl("txtULDNo")).Enabled = false;
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOL")).Text = dsawb.Tables[1].Rows[i][1].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOU")).Text = dsawb.Tables[1].Rows[i][2].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDLocation")).Text = dsawb.Tables[1].Rows[i][3].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDLoadingPriority")).Text = dsawb.Tables[1].Rows[i][4].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtScaleWeight")).Text = dsawb.Tables[1].Rows[i][9].ToString();
                                ((DropDownList)grdULDList.Rows[i].FindControl("ddlContainerType")).SelectedValue = dsawb.Tables[1].Rows[i][10].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDBuilderName")).Text = dsawb.Tables[1].Rows[i][11].ToString();
                                ((Label)grdULDList.Rows[i].FindControl("lblIsReceived")).Text = dsawb.Tables[1].Rows[i]["IsReceived"].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtDollyWt")).Text = dsawb.Tables[1].Rows[i]["DollyWt"].ToString();
                                ((Label)grdULDList.Rows[i].FindControl("lblAWBNumber")).Text = dsawb.Tables[1].Rows[i]["AWBNumber"].ToString();
                            }
                        }
                    }
                }
                try
                {

                    for (int cnt = 0; cnt < grdULDList.Rows.Count; cnt++)
                    {
                        if (((Label)grdULDList.Rows[cnt].FindControl("lblIsReceived")).Text.Trim().Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            grdULDList.Rows[cnt].BackColor = CommonUtility.ColorHighlightedGrid;
                        }

                    }
                }
                catch (Exception ex) { }

                #endregion Show ULD list


                #region Show Cart list

                //To show Cart list
                CartListDetails.Columns.Add("Cartno");
                CartListDetails.Columns.Add("LoadingPriority");
                CartListDetails.Columns.Add("CartStatus");
                CartListDetails.Columns.Add("CartBuilderName");
                CartListDetails.Columns.Add("ScaleWeight");
                CartListDetails.Columns.Add("BulkWeight");

                Session["FlightPlanning_CartList"] = CartListDetails;


                if (dsawb != null)
                {
                    if (dsawb.Tables.Count > 0)
                    {
                        if (dsawb.Tables[3].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsawb.Tables[3].Rows.Count; i++)
                            {
                                DataRow l_Datarow = CartListDetails.NewRow();
                                l_Datarow["Cartno"] = dsawb.Tables[3].Rows[i]["Cartno"].ToString();
                                l_Datarow["LoadingPriority"] = dsawb.Tables[3].Rows[i]["LoadingPriority"].ToString();
                                l_Datarow["CartBuilderName"] = dsawb.Tables[3].Rows[i]["CartBuilderName"].ToString();
                                l_Datarow["ScaleWeight"] = dsawb.Tables[3].Rows[i]["ScaleWeight"].ToString();
                                l_Datarow["BulkWeight"] = dsawb.Tables[3].Rows[i]["BulkWeight"].ToString();

                                CartListDetails.Rows.Add(l_Datarow);
                            }
                            grdCartList.DataSource = dsawb.Tables[3];
                            grdCartList.DataBind();
                            Session["FlightPlanning_CartList"] = CartListDetails;

                            if (hfCartNumber.Value == "" && hfULDNumber.Value == "")
                            {
                                //if uld is not present and cart is present
                                if (dsawb.Tables.Count > 0)
                                {
                                    //Pnlgrd.Visible = false;
                                    //gdvULDDetails.DataSource = null;
                                    //gdvULDDetails.DataBind();

                                    //btnSave.Visible = false;
                                    //btnUnassign.Visible = false;

                                    HideULDAWBDetails();

                                    //((CheckBox)grdCartList.Rows[0].FindControl("rdCartList")).Checked = true;
                                    string cart = CartListDetails.Rows[0]["Cartno"].ToString();
                                    BindAWBManifestCart(cart, lblDepAirport.Text, flightID, FlightDate);
                                }
                            }
                            else if (hfULDNumber.Value == "" || dsawb.Tables[1].Rows.Count == 0)
                            {
                                for (int t = 0; t < dsawb.Tables[1].Rows.Count; t++)
                                {
                                    ((CheckBox)grdULDList.Rows[t].FindControl("rdULDList")).Checked = false;
                                }

                                BindAWBManifestCart(hfCartNumber.Value, lblDepAirport.Text, flightID, FlightDate);

                                //for (int t = 0; t < dsawb.Tables[3].Rows.Count; t++)
                                //{
                                //    if (dsawb.Tables[3].Rows[t]["Cartno"].ToString() == hfCartNumber.Value)
                                //    {
                                //        ((CheckBox)grdCartList.Rows[t].FindControl("rdCartList")).Checked = true;
                                //    }
                                //}

                                HideULDAWBDetails();
                            }

                            ShowULDAWBSummary("");

                        }
                        else //If no Cart exists
                        {
                            grdCartList.DataSource = null;
                            grdCartList.DataBind();

                            grdBulkAssignedAWB.DataSource = null;
                            grdBulkAssignedAWB.DataBind();

                            if (dsawb.Tables[1].Rows.Count > 0 && hfULDNumber.Value != "") //if ULD exists hide Cart AWB details
                            {
                                //pnlBulkAssignedAWB.Visible = true;
                                //Pnlgrd.Visible = false;
                                //btnSave.Visible = false;
                                //btnUnassign.Visible = false;
                                //btnSaveBulkAWB.Visible = true;
                                //btnUnAssignBulkAWB.Visible = true;
                                HideCartAWBDetails();

                                for (int t = 0; t < dsawb.Tables[3].Rows.Count; t++)
                                {
                                    ((CheckBox)grdCartList.Rows[t].FindControl("rdCartList")).Checked = false;
                                }

                                BindAWBManifestULD(hfULDNumber.Value, lblDepAirport.Text, flightID, FlightDate);

                                //for (int t = 0; t < dsawb.Tables[3].Rows.Count; t++)
                                //{
                                //    if (dsawb.Tables[1].Rows[t]["Cartno"].ToString() == hfULDNumber.Value)
                                //    {
                                //        ((CheckBox)grdULDList.Rows[t].FindControl("rdULDList")).Checked = true;
                                //    }
                                //}
                            }
                            else
                            {
                                //pnlBulkAssignedAWB.Visible = false;
                                //Pnlgrd.Visible = true;
                                //btnSave.Visible = true;
                                //btnUnassign.Visible = true;
                                //btnSaveBulkAWB.Visible = false;
                                //btnUnAssignBulkAWB.Visible = false;
                                HideULDAWBDetails();
                            }
                        }
                    }
                }

                if (dsawb != null)
                {
                    if (dsawb.Tables.Count > 0)
                    {
                        if (dsawb.Tables[3].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsawb.Tables[3].Rows.Count; i++)
                            {
                                ((Label)grdCartList.Rows[i].FindControl("lblOldCartNo")).Text = dsawb.Tables[3].Rows[i]["CartNo"].ToString();
                                ((TextBox)grdCartList.Rows[i].FindControl("txtCartNo")).Text = dsawb.Tables[3].Rows[i]["CartNo"].ToString();
                                //((TextBox)grdCartList.Rows[i].FindControl("txtCartNo")).Enabled = false;
                                ((TextBox)grdCartList.Rows[i].FindControl("txtCartLoadingPriority")).Text = dsawb.Tables[3].Rows[i]["LoadingPriority"].ToString();
                                ((TextBox)grdCartList.Rows[i].FindControl("txtCartBuilderName")).Text = dsawb.Tables[3].Rows[i]["CartBuilderName"].ToString();
                                ((TextBox)grdCartList.Rows[i].FindControl("txtScaleWeight")).Text = dsawb.Tables[3].Rows[i]["ScaleWeight"].ToString();

                            }
                        }
                    }
                }

                #endregion Show Cart list

                //Set flight status..
                if (dsawb.Tables.Count > 4 && dsawb.Tables[4].Rows.Count > 0)
                {
                    Session["FlightPlan_FlightStatus"] = dsawb.Tables[4].Rows[0][0].ToString();
                }
                else
                {
                    Session["FlightPlan_FlightStatus"] = "";
                }
                CheckFlightStatus(Convert.ToString(Session["FlightPlan_FlightStatus"]));
            }
            catch { }
            finally
            {
                if (dsawb != null)
                    dsawb.Dispose();
                if (MemDetails != null)
                    MemDetails.Dispose();
                if (ULDListDetails != null)
                    ULDListDetails.Dispose();
                if (CartListDetails != null)
                    CartListDetails.Dispose();
            }
        }
        #endregion Bind Load Plan AWB

        #region Bind Bulk Assigned AWB
        protected void BindBulkAssignedAWB()
        {
            DataSet dsbulkawbData = new DataSet("FltPlan_dsbulkawbData");
            try
            {
                string flightID;
                if (txtFlightID.Text.Trim() != "")
                    flightID = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                else
                    flightID = "";

                string FlightDate = TextBoxdate.Text;
                Session["FlightPlanning_FltDet"] = flightID + "-" + FlightDate; //Shubh
                string AWBPrefix = txtAWBPrefix.Text.Trim();
                string AWBNumber = txtAWBNo.Text.Trim();
                string ULDNumber = "";

                dsbulkawbData = objBuildULD.GetBulkAssignedAWBDetails(lblDepAirport.Text, flightID, FlightDate, AWBPrefix, AWBNumber, ULDNumber);

                if (dsbulkawbData.Tables.Count > 0)
                {
                    if (dsbulkawbData.Tables[0].Rows.Count > 0)
                    {
                        tabAssignedBulkAWBDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];
                        tabAssignedBulkAWBDetails.Rows.Clear();

                        for (int i = 0; i < dsbulkawbData.Tables[0].Rows.Count; i++)
                        {
                            DataRow l_Datarow = tabAssignedBulkAWBDetails.NewRow();

                            l_Datarow["AWBno"] = dsbulkawbData.Tables[0].Rows[i]["AWBNo"].ToString();
                            l_Datarow["ULDno"] = dsbulkawbData.Tables[0].Rows[i]["Uldno"].ToString();
                            l_Datarow["TotalPcs"] = dsbulkawbData.Tables[0].Rows[i]["TotalPcs"].ToString();
                            l_Datarow["TotalWt"] = dsbulkawbData.Tables[0].Rows[i]["TotalWt"].ToString();
                            l_Datarow["AcceptedPcs"] = dsbulkawbData.Tables[0].Rows[i]["AcceptedPcs"].ToString();
                            l_Datarow["AcceptedWt"] = dsbulkawbData.Tables[0].Rows[i]["AcceptedWt"].ToString();
                            l_Datarow["BuiltPcs"] = dsbulkawbData.Tables[0].Rows[i]["pieces"].ToString();
                            l_Datarow["BuiltWt"] = dsbulkawbData.Tables[0].Rows[i]["GWeight"].ToString();
                            l_Datarow["Origin"] = dsbulkawbData.Tables[0].Rows[i]["ULDOrigin"].ToString(); ;
                            l_Datarow["Dest"] = dsbulkawbData.Tables[0].Rows[i]["ULDDest"].ToString();
                            l_Datarow["Location"] = dsbulkawbData.Tables[0].Rows[i]["Location"].ToString();
                            l_Datarow["AWBLoadingPriority"] = dsbulkawbData.Tables[0].Rows[i]["LoadingPriority"].ToString();
                            l_Datarow["AWBBuilderName"] = dsbulkawbData.Tables[0].Rows[i]["AWBBuilderName"].ToString();
                            l_Datarow["FlightExists"] = dsbulkawbData.Tables[0].Rows[i]["FlightExists"].ToString();
                            l_Datarow["CartNumber"] = dsbulkawbData.Tables[0].Rows[i]["CartNo"].ToString();
                            l_Datarow["FltNo"] = dsbulkawbData.Tables[0].Rows[i]["FltNo"].ToString();
                            l_Datarow["FltDate"] = dsbulkawbData.Tables[0].Rows[i]["FltDate"].ToString();

                            tabAssignedBulkAWBDetails.Rows.Add(l_Datarow);

                            if (tabAssignedBulkAWBDetails != null && tabAssignedBulkAWBDetails.Rows.Count > 0)
                            {
                                Session["FlightPlanning_ManifestBulkAWBData"] = tabAssignedBulkAWBDetails;
                            }

                            //Deleted textboxes
                            //txtULDNumber.Text = dsawbData.Tables[0].Rows[i]["Uldno"].ToString();
                            //txtULDNumber.ReadOnly = true;

                            grdBulkAssignedAWB.DataSource = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];
                            grdBulkAssignedAWB.DataBind();

                            Session["FlightPlanning_GDVBULKAWBDetails"] = tabAssignedBulkAWBDetails;
                        }

                    }
                    else
                    {
                        grdBulkAssignedAWB.DataSource = null;
                        grdBulkAssignedAWB.DataBind();

                        tabAssignedBulkAWBDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];
                        tabAssignedBulkAWBDetails.Rows.Clear();
                    }

                    if (dsbulkawbData.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsbulkawbData.Tables[0].Rows.Count; i++)
                        {
                            ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("txtAWBLocation")).Text = dsbulkawbData.Tables[0].Rows[i]["Location"].ToString();
                            ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("txtAWBLoadingPriority")).Text = dsbulkawbData.Tables[0].Rows[i]["LoadingPriority"].ToString();
                            ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("txtAWBBuilderName")).Text = dsbulkawbData.Tables[0].Rows[i]["AWBBuilderName"].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (dsbulkawbData != null)
                    dsbulkawbData.Dispose();
                if (tabAssignedBulkAWBDetails != null)
                    tabAssignedBulkAWBDetails.Dispose();
            }
        }
        #endregion Bind Bulk Assigned AWB

        #region Check flight status
        protected void CheckFlightStatus(string FlightStatus)
        {
            try
            {

                if (FlightStatus != "")
                {
                    switch (FlightStatus.ToUpper())
                    {
                        case "DEPARTED":
                            FlightStatus = "Flight departed.";
                            break;
                        case "EXPORTED":
                            FlightStatus = "Flight exported to manifest.";
                            break;
                        default:
                            FlightStatus = "";
                            break;
                    }
                    lblStatus.ForeColor = Color.Blue;
                    lblStatus.Text = FlightStatus;

                    BtnAddtoManifest.Enabled = false;
                    btnAssignBulk.Enabled = false;
                    //btnReAssign.Enabled = false;
                    btnSaveBulkAWB.Enabled = false;
                    btnUnAssignBulkAWB.Enabled = false;
                    btnNewULD.Enabled = false;
                    Save1.Enabled = false;
                    btnDeleteULD.Enabled = false;
                    btnFinalizeULD.Enabled = false;
                    btnReOpenULD.Enabled = false;
                    btnRemove.Enabled = false;
                    btnSave.Enabled = false;
                    btnUnassign.Enabled = false;
                    btnExportToManifest.Enabled = false;
                    btnNewCart.Enabled = false;
                    btnSaveCart.Enabled = false;
                    btnDeleteCart.Enabled = false;
                    btnReassignCart.Enabled = false;
                    btnAssignToCartTop.Enabled = false;
                    btnAssignToULDBottom.Enabled = false;
                    btnFetchAWB.Enabled = false;
                    btnFetchClear.Enabled = false;
                    btnAssignFetchedAWB.Enabled = false;
                    btnAssignToCartTop.Enabled = false;
                    btnAssignToULDBottom.Enabled = false;
                }
                else
                {
                    lblStatus.Text = "";
                    lblULDStatus.Text = "";
                    BtnAddtoManifest.Enabled = true;
                    btnAssignBulk.Enabled = true;
                    btnReAssign.Enabled = true;
                    btnSaveBulkAWB.Enabled = true;
                    btnUnAssignBulkAWB.Enabled = true;
                    btnNewULD.Enabled = true;
                    Save1.Enabled = true;
                    btnDeleteULD.Enabled = true;
                    btnFinalizeULD.Enabled = true;
                    btnReOpenULD.Enabled = true;
                    btnRemove.Enabled = true;
                    btnSave.Enabled = true;
                    btnUnassign.Enabled = true;
                    btnExportToManifest.Enabled = true;

                    btnNewCart.Enabled = true;
                    btnSaveCart.Enabled = true;
                    btnDeleteCart.Enabled = true;
                    btnReassignCart.Enabled = true;
                    btnAssignToCartTop.Enabled = true;
                    btnAssignToULDBottom.Enabled = true;
                    btnFetchAWB.Enabled = true;
                    btnFetchClear.Enabled = true;
                    btnAssignFetchedAWB.Enabled = true;
                }
            }
            catch (Exception ex)
            {
            }

        }
        #endregion Check flight status

        #region Check flight export status
        protected void CheckFlightExportStatus()
        {
            try
            {
                string FLTno = "", FltDate = "";

                if (txtFlightID.Text.Trim() != "")
                    FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                else
                    FLTno = "";

                if (TextBoxdate.Text.Trim() == "")
                    FltDate = "";
                else
                    FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                string FlightStatus = objBuildULD.checkFlightExportStatus(FLTno, FltDate, Session["Station"].ToString());
                if (FlightStatus == "EXPORTED")
                {
                    lblStatus.ForeColor = Color.Blue;
                    lblStatus.Text = "Flight exported to manifest";

                    BtnAddtoManifest.Enabled = false;
                    btnAssignBulk.Enabled = false;
                    //btnReAssign.Enabled = false;
                    btnSaveBulkAWB.Enabled = false;
                    btnUnAssignBulkAWB.Enabled = false;
                    btnNewULD.Enabled = false;
                    Save1.Enabled = false;
                    btnDeleteULD.Enabled = false;
                    btnFinalizeULD.Enabled = false;
                    btnReOpenULD.Enabled = false;
                    btnRemove.Enabled = false;
                    btnSave.Enabled = false;
                    btnUnassign.Enabled = false;
                    btnExportToManifest.Enabled = false;
                    btnNewCart.Enabled = false;
                    btnSaveCart.Enabled = false;
                    btnDeleteCart.Enabled = false;
                    btnReassignCart.Enabled = false;
                }
                else
                {
                    lblStatus.Text = "";
                    lblULDStatus.Text = "";
                    BtnAddtoManifest.Enabled = true;
                    btnAssignBulk.Enabled = true;
                    btnReAssign.Enabled = true;
                    btnSaveBulkAWB.Enabled = true;
                    btnUnAssignBulkAWB.Enabled = true;
                    btnNewULD.Enabled = true;
                    Save1.Enabled = true;
                    btnDeleteULD.Enabled = true;
                    btnFinalizeULD.Enabled = true;
                    btnReOpenULD.Enabled = true;
                    btnRemove.Enabled = true;
                    btnSave.Enabled = true;
                    btnUnassign.Enabled = true;
                    btnExportToManifest.Enabled = true;

                    btnNewCart.Enabled = true;
                    btnSaveCart.Enabled = true;
                    btnDeleteCart.Enabled = true;
                    btnReassignCart.Enabled = true;
                }
            }
            catch (Exception ex)
            {
            }

        }
        #endregion Check flight status

        protected void BindAWBManifestULD(string ULDNumber, string POL, string FLTNo, string FlightDate)
        {
            DataSet dsawbData = new DataSet("FltPlan_dsawbData");
            try
            {
                dsawbData = objBuildULD.GetAWBDetailsFromULD(ULDNumber, POL, FLTNo, FlightDate);

                if (dsawbData.Tables.Count > 0)
                {
                    if (dsawbData.Tables[0].Rows.Count > 0)
                    {
                        tabMenifestDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];
                        tabMenifestDetails.Rows.Clear();

                        for (int i = 0; i < dsawbData.Tables[0].Rows.Count; i++)
                        {
                            DataRow l_Datarow = tabMenifestDetails.NewRow();

                            l_Datarow["AWBno"] = dsawbData.Tables[0].Rows[i]["AWBNo"].ToString();
                            l_Datarow["ULDno"] = dsawbData.Tables[0].Rows[i]["Uldno"].ToString();
                            l_Datarow["TotalPcs"] = dsawbData.Tables[0].Rows[i]["TotalPcs"].ToString();
                            l_Datarow["TotalWt"] = dsawbData.Tables[0].Rows[i]["TotalWt"].ToString();
                            l_Datarow["AcceptedPcs"] = dsawbData.Tables[0].Rows[i]["AcceptedPcs"].ToString();
                            l_Datarow["AcceptedWt"] = dsawbData.Tables[0].Rows[i]["AcceptedWt"].ToString();
                            l_Datarow["BuiltPcs"] = dsawbData.Tables[0].Rows[i]["pieces"].ToString();
                            l_Datarow["BuiltWt"] = dsawbData.Tables[0].Rows[i]["GWeight"].ToString();
                            l_Datarow["Origin"] = dsawbData.Tables[0].Rows[i]["ULDOrigin"].ToString(); ;
                            l_Datarow["Dest"] = dsawbData.Tables[0].Rows[i]["ULDDest"].ToString();
                            l_Datarow["Location"] = dsawbData.Tables[0].Rows[i]["Location"].ToString();
                            l_Datarow["AWBLoadingPriority"] = dsawbData.Tables[0].Rows[i]["LoadingPriority"].ToString();
                            l_Datarow["AWBBuilderName"] = dsawbData.Tables[0].Rows[i]["AWBBuilderName"].ToString();
                            l_Datarow["FlightExists"] = dsawbData.Tables[0].Rows[i]["FlightExists"].ToString();
                            l_Datarow["FltNo"] = dsawbData.Tables[0].Rows[i]["FltNo"].ToString();
                            l_Datarow["FltDate"] = dsawbData.Tables[0].Rows[i]["FltDate"].ToString();
                            l_Datarow["ProductType"] = dsawbData.Tables[0].Rows[i]["ProductType"].ToString();
                            l_Datarow["Description"] = dsawbData.Tables[0].Rows[i]["Description"].ToString();
                            l_Datarow["CommodityCode"] = dsawbData.Tables[0].Rows[i]["CommodityCode"].ToString();
                            l_Datarow["SHC"] = dsawbData.Tables[0].Rows[i]["SHC"].ToString();

                            tabMenifestDetails.Rows.Add(l_Datarow);

                            if (tabMenifestDetails != null && tabMenifestDetails.Rows.Count > 0)
                            {
                                Session["FlightPlanning_ManifestBulkAWBData"] = tabMenifestDetails;
                            }

                            //Deleted textboxes
                            //txtULDNumber.Text = dsawbData.Tables[0].Rows[i]["Uldno"].ToString();
                            //txtULDNumber.ReadOnly = true;

                            grdBulkAssignedAWB.DataSource = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];
                            grdBulkAssignedAWB.DataBind();

                        }

                    }
                    else
                    {

                        tabMenifestDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];
                        tabMenifestDetails.Rows.Clear();
                    }

                    if (dsawbData.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsawbData.Tables[0].Rows.Count; i++)
                        {
                            ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("txtAWBLocation")).Text = dsawbData.Tables[0].Rows[i]["Location"].ToString();
                            ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("txtAWBLoadingPriority")).Text = dsawbData.Tables[0].Rows[i]["LoadingPriority"].ToString();
                            ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("txtAWBBuilderName")).Text = dsawbData.Tables[0].Rows[i]["AWBBuilderName"].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (dsawbData != null)
                    dsawbData.Dispose();
                if (tabMenifestDetails != null)
                    tabMenifestDetails.Dispose();
            }
        }


        #region Event to add HAWB(s) to the AWBGrid
        protected void gdvULDLoadPlanAWB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                try
                {
                    GridViewRow row = e.Row;
                    if (row.DataItem == null)
                    {
                        return;
                    }
                    DataSet dt = new DataSet("FltPlan_dt0");
                    object[] paramvalue = new object[4];

                    paramvalue[0] = ((DataRowView)e.Row.DataItem)["FltNo"].ToString().Trim();

                    paramvalue[1] = ((DataRowView)e.Row.DataItem)["FltDate"].ToString().Trim();

                    paramvalue[2] = Session["Station"].ToString();
                    paramvalue[3] = ((DataRowView)e.Row.DataItem)["AWBNo"].ToString().Trim();
                    if (Session["GHA_FlightPlanning_HAWBOriginal"] == null)
                    {

                        dt = objBuildULD.GetChildHAWB(txtAWBPrefix.Text.Trim(), txtAWBNo.Text.Trim(), paramvalue[0].ToString(), paramvalue[1].ToString());

                        if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                        {

                            GridView gv = new GridView();
                            gv = (GridView)row.FindControl("GVSubHAWB");
                            GridViewRow gr = (GridViewRow)e.Row;
                            ////((TextBox)gr.FindControl("txtRemainingPcs")).ReadOnly = true;
                            ////((TextBox)gr.FindControl("txtRemainingWt")).ReadOnly = true;
                            //Bind the DataTable to the Grid 
                            Session["GHA_FlightPlanning_HAWBOriginal"] = dt;
                            gv.DataSource = null;
                            gv.DataSource = dt.Tables[0].Select("AWBNumber='" + paramvalue[3] + "' AND (RemainingPcs>0 OR ISNULL(IsBulkOrULD,'')='') AND ISNULL(Bulk,'')=''").CopyToDataTable();
                            gv.DataBind();
                        }
                    }
                    else
                    {
                        if (Session["GHA_FlightPlanning_HAWBAddToULD"] != null)
                        {
                            //DataTable dtULD = ((DataTable)Session["GHA_AssignULD_HAWBAddToULD"]).Select("AWBNumber='" + paramvalue[3] + "'").CopyToDataTable();
                            //if (dtULD.Rows.Count > 1)
                            //{
                            //    GridView gv = new GridView();
                            //    gv = (GridView)row.FindControl("GVSubHAWB");
                            //    //Bind the DataTable to the Grid 
                            //    gv.DataSource = null;
                            //    gv.DataSource = ((DataTable)Session["GHA_AssignULD_HAWBAddToULD"]).Select("AWBNumber='" + paramvalue[3] + "' AND (RemainingPcs>0 AND ISNULL(IsBulkOrULD,'')='')").CopyToDataTable();
                            //    gv.DataBind();
                            //    return;
                            //}
                            //else
                            //{
                            //    GridView gv = new GridView();
                            //    gv = (GridView)row.FindControl("GVSubHAWB");
                            //    //Bind the DataTable to the Grid 
                            //    gv.DataSource = null;
                            //    gv.DataSource = ((DataTable)Session["GHA_AssignULD_HAWBAddToULD"]).Select("AWBNumber='" + paramvalue[3] + "' AND (RemainingPcs>0 OR ISNULL(IsBulkOrULD,'')='')").CopyToDataTable();
                            //    gv.DataBind();
                            //    return;
                            //}
                            GridView gv = new GridView();
                            gv = (GridView)row.FindControl("GVSubHAWB");
                            //Bind the DataTable to the Grid 
                            gv.DataSource = null;
                            gv.DataSource = ((DataTable)Session["GHA_FlightPlanning_HAWBAddToULD"]).Select("AWBNumber='" + paramvalue[3] + "' AND (RemainingPcs>0 OR ISNULL(IsBulkOrULD,'')='') AND ISNULL(Bulk,'')=''").CopyToDataTable();
                            gv.DataBind();
                            return;
                        }
                        if (Session["GHA_FlightPlanning_HAWBAddToBulk"] != null)
                        {
                            //DataTable dtBulk = ((DataTable)Session["GHA_AssignULD_HAWBAddToBulk"]).Select("AWBNumber='" + paramvalue[3] + "'").CopyToDataTable();
                            //if (dtBulk.Rows.Count > 1)
                            //{
                            //    GridView gv = new GridView();
                            //    gv = (GridView)row.FindControl("GVSubHAWB");
                            //    //Bind the DataTable to the Grid 
                            //    gv.DataSource = null;
                            //    gv.DataSource = ((DataTable)Session["GHA_AssignULD_HAWBAddToBulk"]).Select("AWBNumber='" + paramvalue[3] + "' AND (RemainingPcs>0 AND ISNULL(IsBulkOrULD,'')='')").CopyToDataTable();
                            //    gv.DataBind();
                            //    return;
                            //}
                            //else
                            //{
                            //    GridView gv = new GridView();
                            //    gv = (GridView)row.FindControl("GVSubHAWB");
                            //    //Bind the DataTable to the Grid 
                            //    gv.DataSource = ((DataTable)Session["GHA_AssignULD_HAWBAddToBulk"]).Select("AWBNumber='" + paramvalue[3] + "' AND (RemainingPcs>0 OR ISNULL(IsBulkOrULD,'')='')").CopyToDataTable();
                            //    gv.DataBind();
                            //    return;
                            //}
                            GridView gv = new GridView();
                            gv = (GridView)row.FindControl("GVSubHAWB");
                            //Bind the DataTable to the Grid 
                            gv.DataSource = null;
                            gv.DataSource = ((DataTable)Session["GHA_FlightPlanning_HAWBAddToBulk"]).Select("AWBNumber='" + paramvalue[3] + "' AND (RemainingPcs>0 OR ISNULL(IsBulkOrULD,'')='') AND ISNULL(Bulk,'')=''").CopyToDataTable();
                            gv.DataBind();
                            return;
                        }
                        if (Session["GHA_FlightPlanning_HAWBOriginal"] != null)
                        {
                            //DataTable dtOrg = ((DataSet)Session["GHA_AssignULD_HAWBOriginal"]).Tables[0].Select("AWBNumber='" + paramvalue[3] + "'").CopyToDataTable();
                            //if (dtOrg.Rows.Count > 1)
                            //{
                            //    GridView gv = new GridView();
                            //    gv = (GridView)row.FindControl("GVSubHAWB");
                            //    //Bind the DataTable to the Grid 
                            //    //gv.DataSource = null;
                            //    gv.DataSource = ((DataSet)Session["GHA_AssignULD_HAWBOriginal"]).Tables[0].Select("AWBNumber='" + paramvalue[3] + "' AND (RemainingPcs>0 AND ISNULL(IsBulkOrULD,'')='')").CopyToDataTable();
                            //    gv.DataBind();
                            //    return;
                            //}
                            //else
                            //{
                            //    GridView gv = new GridView();
                            //    gv = (GridView)row.FindControl("GVSubHAWB");
                            //    //Bind the DataTable to the Grid 
                            //    //gv.DataSource = null;
                            //    gv.DataSource = ((DataSet)Session["GHA_AssignULD_HAWBOriginal"]).Tables[0].Select("AWBNumber='" + paramvalue[3] + "' AND (RemainingPcs>0 OR ISNULL(IsBulkOrULD,'')='')").CopyToDataTable();
                            //    gv.DataBind();
                            //    return;
                            //}
                            GridView gv = new GridView();
                            gv = (GridView)row.FindControl("GVSubHAWB");
                            //Bind the DataTable to the Grid 
                            //gv.DataSource = null;
                            gv.DataSource = ((DataSet)Session["GHA_FlightPlanning_HAWBOriginal"]).Tables[0].Select("AWBNumber='" + paramvalue[3] + "' AND (RemainingPcs>0 OR ISNULL(IsBulkOrULD,'')='') AND ISNULL(Bulk,'')=''").CopyToDataTable();
                            gv.DataBind();
                            return;
                        }
                    }

                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Event to add HAWB(s) to the BulkGrid
        protected void grdBulkAssignedAWB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                try
                {
                    GridViewRow row = e.Row;
                    if (row.DataItem == null)
                    {
                        return;
                    }

                    DataSet dt = new DataSet("FltPlan_dt1");
                    object[] paramvalue = new object[4];

                    paramvalue[0] = ((DataRowView)e.Row.DataItem)["FltNo"].ToString().Trim();

                    paramvalue[1] = ((DataRowView)e.Row.DataItem)["FltDate"].ToString().Trim();

                    paramvalue[2] = Session["Station"].ToString();
                    paramvalue[3] = ((DataRowView)e.Row.DataItem)["AWBNo"].ToString().Trim();
                    if (Session["GHA_FlightPlanning_HAWBOriginal"] == null)
                    {
                        dt = objBuildULD.GetChildHAWB(txtAWBPrefix.Text.Trim(), txtAWBNo.Text.Trim(), paramvalue[0].ToString(), paramvalue[1].ToString());

                        if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                        {
                            Session["GHA_FlightPlanning_HAWBOriginal"] = null;
                            GridView gv = new GridView();
                            gv = (GridView)row.FindControl("GVSubHAWB");
                            //Bind the DataTable to the Grid 
                            Session["GHA_FlightPlanning_HAWBOriginal"] = dt;
                            gv.DataSource = null;
                            gv.DataSource = dt.Tables[0].Select("AWBNumber='" + paramvalue[3] + "' AND IsBulkOrULD='B'").CopyToDataTable();
                            gv.DataBind();
                            return;
                        }
                    }
                    else
                        if (Session["GHA_FlightPlanning_HAWBAddToBulk"] != null)
                        {
                            GridView gv = new GridView();
                            gv = (GridView)row.FindControl("GVSubHAWB");
                            //Bind the DataTable to the Grid 
                            gv.DataSource = null;
                            gv.DataSource = ((DataTable)Session["GHA_FlightPlanning_HAWBAddToBulk"]).Select("AWBNumber='" + paramvalue[3] + "' AND (IsBulkOrULD='B')").CopyToDataTable();
                            gv.DataBind();
                            return;
                        }
                        else
                            if (Session["GHA_FlightPlanning_HAWBOriginal"] != null)
                            {
                                GridView gv = new GridView();
                                gv = (GridView)row.FindControl("GVSubHAWB");
                                //Bind the DataTable to the Grid 
                                gv.DataSource = null;
                                gv.DataSource = ((DataSet)Session["GHA_FlightPlanning_HAWBOriginal"]).Tables[0].Select("AWBNumber='" + paramvalue[3] + "' AND (IsBulkOrULD='B')").CopyToDataTable();
                                gv.DataBind();
                                return;
                            }



                }
                catch (Exception)
                {
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        protected void rdULDList_CheckedChanged(Object sender, System.EventArgs e)
        {
            hfCartNumber.Value = "";
            lblStatus.Text = "";
            lblULDStatus.Text = "";
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int index = gvRow.RowIndex;

            for (int i = 0; i < grdULDList.Rows.Count; i++)
            {
                if (i != index)
                {
                    ((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked = false;
                }
            }

            string ULDNumber = "";
            //for (int i = 0; i < grdULDList.Rows.Count; i++)
            //{
            //    if (((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
            //    {
            //        ULDNumber = ((TextBox)grdULDList.Rows[i].FindControl("txtULDNo")).Text;
            //    }
            //}
            ULDNumber = ((Label)grdULDList.Rows[index].FindControl("lblOldULDNo")).Text;

            string flightID;
            if (txtFlightID.Text.Trim() != "")
                flightID = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
            else
                flightID = "";

            string FltDate = TextBoxdate.Text;

            HideCartAWBDetails();

            for (int i = 0; i < grdCartList.Rows.Count; i++)
            {
                ((CheckBox)grdCartList.Rows[i].FindControl("rdCartList")).Checked = false;
            }

            BindAWBManifestULD(ULDNumber, lblDepAirport.Text, flightID, FltDate);

            ShowULDAWBSummary("");

            CheckFlightStatus(Convert.ToString(Session["FlightPlan_FlightStatus"]));
            lblStatus.Text = "";
            lblULDStatus.Text = "";
        }

        #region Event to add HAWB(s) to the ULDGrid
        protected void gdvULDDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                try
                {
                    GridViewRow row = e.Row;
                    if (row.DataItem == null)
                    {
                        return;
                    }

                    DataSet dt = new DataSet("FltPlan_dt2");
                    object[] paramvalue = new object[4];

                    paramvalue[0] = ((DataRowView)e.Row.DataItem)["FltNo"].ToString().Trim();

                    paramvalue[1] = ((DataRowView)e.Row.DataItem)["FltDate"].ToString().Trim();

                    paramvalue[2] = Session["Station"].ToString();
                    paramvalue[3] = ((DataRowView)e.Row.DataItem)["AWBNo"].ToString().Trim();
                    string ULDNo = e.Row.Cells[3].Text != "&nbsp;" ? e.Row.Cells[3].Text : string.Empty;
                    if (Session["GHA_FlightPlanning_HAWBOriginal"] == null)
                    {
                        dt = objBuildULD.GetChildHAWB(txtAWBPrefix.Text.Trim(), txtAWBNo.Text.Trim(), paramvalue[0].ToString(), paramvalue[1].ToString());

                        if (dt != null && dt.Tables.Count > 0 && dt.Tables[0].Rows.Count > 0)
                        {
                            Session["GHA_FlightPlanning_HAWBOriginal"] = null;
                            GridView gv = new GridView();
                            gv = (GridView)row.FindControl("GVSubHAWB");
                            //Bind the DataTable to the Grid 
                            gv.DataSource = null;
                            Session["GHA_FlightPlanning_HAWBOriginal"] = dt;
                            gv.DataSource = dt.Tables[0].Select("AWBNumber='" + paramvalue[3] + "' AND ISNULL(IsBulkOrULD,'')='U' AND ULDNo='" + ULDNo + "'").CopyToDataTable();
                            gv.DataBind();
                            return;
                        }
                    }
                    else
                    {
                        if (Session["GHA_FlightPlanning_HAWBAddToULD"] != null)
                        {
                            GridView gv = new GridView();
                            gv = (GridView)row.FindControl("GVSubHAWB");
                            //Bind the DataTable to the Grid 
                            gv.DataSource = null;
                            gv.DataSource = ((DataTable)Session["GHA_FlightPlanning_HAWBAddToULD"]).Select("AWBNumber='" + paramvalue[3] + "' AND (ISNULL(IsBulkOrULD,'')='U') AND ULDNo='" + ULDNo + "'").CopyToDataTable();
                            gv.DataBind();
                            return;
                        }
                        if (Session["GHA_FlightPlanning_HAWBOriginal"] != null)
                        {
                            GridView gv = new GridView();
                            gv = (GridView)row.FindControl("GVSubHAWB");
                            //Bind the DataTable to the Grid 
                            gv.DataSource = null;
                            gv.DataSource = ((DataSet)Session["GHA_FlightPlanning_HAWBOriginal"]).Tables[0].Select("AWBNumber='" + paramvalue[3] + "' AND (ISNULL(IsBulkOrULD,'')='U') AND ULDNo='" + ULDNo + "'").CopyToDataTable();
                            gv.DataBind();
                            return;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

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

                Session["FlightPlanning_Mod"] = "1";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        #region UpdatePartnerCode
        private void UpdatePartnerCode(int rowindex)
        {
            try
            {
                DataSet dsResult = new DataSet("FltPlan_dsResult");

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
                DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, 0, 0, 0, ref errormessage, strPartnerCode);

                if (dsresult != null && dsresult.Tables.Count != 0)
                {
                    DataSet ds = new DataSet("FltPlan_ds0");
                    ds = (DataSet)Session["FlightPlanning_Flt"];
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
                                        DataTable dt = new DataTable("FltPlan_dt0");
                                        dt = dsresult.Tables[0].Copy();
                                        dt.TableName = name;
                                        ds.Tables.Add(dt);
                                        ds.AcceptChanges();
                                        Session["FlightPlanning_Flt"] = ds.Copy();
                                    }
                                }
                                catch (Exception ex) { }

                            }
                            else if (ds.Tables.Count == 1)
                            {
                                Session["FlightPlanning_Flt"] = dsresult.Copy();
                            }
                        }
                        catch (Exception ex) { }

                    }
                    else
                    {
                        Session["FlightPlanning_Flt"] = dsresult.Copy();
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

        #region FltDate
        protected void txtFdate_TextChanged(object sender, EventArgs e)
        {
            if (HidChangeDate.Value != "Y")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                return;
            }

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
                //if (((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.Trim() == BookingBAL.OrgStation)
                //{
                //    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                //    DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

                //    hr = dtIndianTime.Hour;
                //    min = dtIndianTime.Minute;
                //    //AllowedHr = 2;
                //}
                //else
                //{
                //    if (rowindex == 0)
                //    {
                //        LBLRouteStatus.Text = "Origin is invalid.";
                //        return;
                //    }
                //    string time = ((DropDownList)grdRouting.Rows[rowindex - 1].FindControl("ddlFltNum")).SelectedValue;
                //    if (!time.Contains(":"))
                //    {
                //        //LBLRouteStatus.Text = "Previous destination arrival time is invalid.";
                //        //return;
                //        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                //        DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

                //        hr = dtIndianTime.Hour;
                //        min = dtIndianTime.Minute;
                //        //AllowedHr = 1;
                //    }
                //    else
                //    {
                //        hr = int.Parse(time.Substring(0, time.IndexOf(":")));
                //        min = int.Parse(time.Substring(time.IndexOf(":") + 1));

                //        //AllowedHr = 1;
                //    }
                //}

                string errormessage = "";
                DataSet dsresult = new DataSet("FltPlan_dsresult1");
                dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text, 
                    ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text, 
                    ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, hr, min, AllowedHr, 
                    ref errormessage);

                if (dsresult != null && dsresult.Tables.Count != 0)
                {
                    Session["FlightPlanning_Flt"] = dsresult.Copy();
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

            DataSet ds = new DataSet("FltPlan_ds2");
            ds = (DataSet)Session["FlightPlanning_Flt"];

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

        #region GetFlightList
        public DataSet GetFlightList(string Origin, string Dest, string strdate, int hr, int min, int AllowedHr, ref string errormessage)
        {
            DataSet dsResult = new DataSet("FltPlan_dsResult2");
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

        public DataSet GetFlightList(string Origin, string Dest, string strdate, int hr, int min, int AllowedHr, ref string errormessage, string PartnerCode)
        {
            DataSet dsResult = new DataSet("FltPlan_dsResult3");
            bool blnSelfAirline = false;
            DataSet dsAWBPrefixs = CommonUtility.AWBPrefixMaster;

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

        #region  FormatRecords
        public void FormatRecords(string org, string dest, ref DataSet dsResult, int PrevHr, int PrevMin, int AllowedHr)
        {
            int i = 0;
            string ScheduleID = "";
            DataSet dsNewResult = new DataSet("FltPlan_dsNewResult");
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

            dsResult = new DataSet("FltPlan_FormatRecords_dsResult");
            dsResult.Tables.Add(dv.ToTable().Copy());


            //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);


            DataTable dt = new DataTable("FltPlan_dt4");
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

            dsResult = new DataSet("FltPlan_dsResult5");
            dsResult.Tables.Add(dt);
        }
        #endregion FormatRecords

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

        protected void BtnAddtoManifest_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GetFlightPlanningStatus())
                    return;

                string ULDNumber = "", ULDOrigin = "";
                lblStatus.Text = "";
                ULDNumber = txtULDNumber.Text;

                for (int i = 0; i < grdULDList.Rows.Count; i++)
                {
                    if (((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
                    {
                        if (((Label)grdULDList.Rows[i].FindControl("lblULDStatus")).Text == "Full")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please reopen ULD before assign.";//check = check + 1;
                            return;
                        }
                    }
                }

                if (ULDNumber == "")
                {
                    lblStatus.Text = "Please select ULD number to assign AWB";
                    return;
                }

                ULDOrigin = lblDepAirport.Text;

                #region Validate ULD against ULD Master and Usage
                if (ULDNumber != string.Empty)
                {
                    string Result = objBuildULD.ValidateULDAdvanced(ULDNumber);
                    if (Result != "Success")
                    {
                        lblStatus.Text = Result;
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                }
                #endregion

                string ULDDest = Session["FlightPlanning_Dest"].ToString();
                string ValidULD = "";
                string ScaleWeight = "0";
                string FLTno = txtFlightCode.Text + txtFlightID.Text;
                string FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
                #region Check if ULD valid or not for save
                ValidULD = objBuildULD.checkValidULDForSave(ULDNumber.ToUpper(), FLTno.ToUpper(),
                    FltDate, ULDOrigin.ToUpper(), ULDDest.ToUpper(), Session["Station"].ToString().ToUpper(),
                    Convert.ToDecimal(ScaleWeight));

                if (ValidULD.ToUpper() == "LOCATION INVALID")
                {
                    //BtnList_Click(sender, e);
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "ULD not in current location";
                    //return;
                }
                else if (ValidULD.ToUpper() == "ULD NOT EMPTY")
                {
                    //BtnList_Click(sender, e);
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "ULD getting used for another flight";
                    //return;
                }
                else if (ValidULD.ToUpper().Contains("SCALE WT"))
                {
                    //BtnList_Click(sender, e);
                    lblULDStatus.ForeColor = Color.Red;
                    lblULDStatus.Text = ValidULD;
                    //return;
                }
                #endregion Check if ULD valid or not for save

                #region Check ULD destination valid or not
                string strResult = "";
                DataSet dsValidDest = new DataSet("FltPlan_dsValidDest");
                dsValidDest = objBuildULD.CheckValidDestination(ULDNumber.ToUpper(), FLTno.ToUpper(), FltDate,
                    ULDOrigin.ToUpper(), ULDDest.ToUpper(), ref strResult);
                if (dsValidDest != null)
                {
                    if (dsValidDest.Tables != null)
                    {
                        if (dsValidDest.Tables.Count > 0)
                        {
                            if (dsValidDest.Tables[0].Rows[0][0].ToString().ToUpper() == "INVALID")
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "ULD destination is invalid";
                                //return;
                            }
                        }
                    }
                }
                #endregion Check ULD destination valid or not

                //lblStatus.Text = "";
                lblULDStatus.Text = "";

                AssignAWBs(ULDNumber, true, "", "", ULDOrigin, Session["FlightPlanning_Dest"].ToString(), "", 0, "0");
                return;

            }
            catch (Exception)
            { }
        }

        public void AddAWBDatarowsToManifestGrid(string ULDNumber)
        {
            DataSet dsawbData = new DataSet("FltPlan_dsawbData2");
            DataTable dsawb1 = new DataTable("FltPlan_dsawb12");
            DataTable dsawb2 = new DataTable("FltPlan_dsawb13");
            DataTable dsawb1_HAWB = new DataTable("FltPlan_dsawb1_HAWB_1");
            DataTable dsawb2_HAWB = new DataTable("FltPlan_dsawb2_HAWB_2");

            try
            {
                Label1.Text = "";
                string AWBno = "", ULDOrigin = "", ULDDest = "", Location = "", BuilderName = "", FlightExists = "";
                int BookedPcs = 0, RemainingPCS = 0, BuiltPcs = 0, AcceptedPcs = 0;
                double BookedGwt = 0, RemainingWGT = 0, BuiltWt = 0, AcceptedWt = 0;
                string FlightNo = string.Empty, FDate = string.Empty, CommodityCode = string.Empty, SHC = string.Empty, CommodityDesc = string.Empty, ProductType = string.Empty;

                for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
                    {
                        if (Convert.ToInt32(((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text) > Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                        {
                            lblStatus.Text = "Please enter valid Pcs";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        if (Convert.ToDouble(((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text) > Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                        {
                            lblStatus.Text = "Please enter valid Weight";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        break;
                    }
                }

                //dsawbData = objBuildULD.GetAWBDetailsFromULD(ULDNumber);
                string flightID;
                if (txtFlightID.Text.Trim() != "")
                    flightID = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                else
                    flightID = "";

                string FlightDate = TextBoxdate.Text;

                string AWBPrefix = txtAWBPrefix.Text.Trim();
                string AWBNumber = txtAWBNo.Text.Trim();
                bool showByDest = false;
                if (Session["FlightPlanning_ShowDest"] != null &&
                    Convert.ToString(Session["FlightPlanning_ShowDest"]) != "")
                {
                    showByDest = Convert.ToBoolean(Session["FlightPlanning_ShowDest"]);
                }
                dsawbData = objBuildULD.GetAWBLoadPlanULDDetails(lblDepAirport.Text, flightID, FlightDate, AWBPrefix,
                    AWBNumber, ULDNumber, showByDest);

                //for (int cnt = 0; cnt < dsawbData.Tables[0].Rows.Count; cnt++)
                //{
                //    if (((CheckBox)gdvULDLoadPlanAWB.Rows[cnt].FindControl("Check2")).Checked == true)
                //    {
                //        if (dsawbData.Tables[0].Rows[cnt]["FlightExists"].ToString() == "FN")
                //        {
                //            BtnList_Click(null, null);
                //            lblStatus.Text = "AWB cannot be assigned to ULD";
                //            lblStatus.ForeColor = Color.Red;
                //            return;
                //        }
                //    }

                //}
                //dsawbData_HAWB = objBuildULD.GetChildHAWB(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBNo")).Text, flightID, FlightDate);
                if (Session["FlightPlanning_AWBdata"] != null)
                {
                    dsawb1 = (DataTable)Session["FlightPlanning_AWBdata"];
                    dsawb2 = dsawb1.Copy();
                }
                else
                {
                    dsawb1 = dsawbData.Tables[0];
                }

                for (int k = 0; k < dsawb2.Rows.Count; k++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[k].FindControl("Check2")).Checked == true)
                    {
                        try
                        {
                            #region Code To Add HAWB Grid To Manifest Deepak Walmiki(22/05/2014)
                            {
                                dsawb1_HAWB = Session["GHA_FlightPlanning_HAWBAddToULD"] != null ? ((DataTable)Session["GHA_FlightPlanning_HAWBAddToULD"]).Copy() : ((DataSet)Session["GHA_FlightPlanning_HAWBOriginal"]).Tables[0].Copy();
                                dsawb2_HAWB = ((DataSet)Session["GHA_FlightPlanning_HAWBOriginal"]).Tables[0].Copy();

                                GridView gvHAWB = (GridView)gdvULDLoadPlanAWB.Rows[k].FindControl("GVSubHAWB");
                                foreach (GridViewRow row in gvHAWB.Rows)
                                {
                                    if (((CheckBox)row.FindControl("Check2")).Checked)
                                    {
                                        foreach (DataRow drow in dsawb1_HAWB.Rows)
                                        {
                                            if (((Label)row.FindControl("lblHAWBNo")).Text == drow["HAWBNo"].ToString() && ((Label)row.FindControl("lblAWBNo")).Text == drow["AWBNumber"].ToString() && drow["IsBulkOrULD"].ToString() != "B")
                                            {
                                                drow["BuiltPcs"] = (int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text) + int.Parse(((TextBox)row.FindControl("txtRemainingPcs")).Text));
                                                drow["BuiltWt"] = (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text) + double.Parse(((TextBox)row.FindControl("txtRemainingWt")).Text));
                                                //((TextBox)row.FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)row.FindControl("lblPieces")).Text) - int.Parse(drow["BuiltPcs"].ToString())).ToString();
                                                //((TextBox)row.FindControl("txtRemainingWt")).Text = (double.Parse(((Label)row.FindControl("lblWeight")).Text) - double.Parse(drow["BuiltWt"].ToString())).ToString();
                                                drow["RemainingPcs"] = (int.Parse(((Label)row.FindControl("lblPieces")).Text) - int.Parse(drow["BuiltPcs"].ToString())).ToString();
                                                drow["RemainingWt"] = (double.Parse(((Label)row.FindControl("lblWeight")).Text) - double.Parse(drow["BuiltWt"].ToString())).ToString();
                                                drow["IsChecked"] = "1";
                                                drow["IsBulkOrULD"] = "U";
                                                if (grdBulkAssignedAWB.Rows.Count > 0)
                                                {
                                                    foreach (GridViewRow ULDRow in grdBulkAssignedAWB.Rows)
                                                    {
                                                        if (drow["AWBNumber"].ToString() == ULDRow.Cells[2].Text)
                                                        {
                                                            drow["ULDNo"] = ULDRow.Cells[3].Text;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //foreach (DataRow drow in dsawb2_HAWB.Rows)
                                        //{
                                        //    drow["BuiltPcs"] = (int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text) + int.Parse(((TextBox)row.FindControl("txtRemainingPcs")).Text));
                                        //    drow["BuiltWt"] = (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text) + double.Parse(((TextBox)row.FindControl("txtRemainingWt")).Text));
                                        //    ((TextBox)row.FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)row.FindControl("lblPieces")).Text) - (int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text) - int.Parse(((TextBox)row.FindControl("txtRemainingPcs")).Text))).ToString();
                                        //    ((TextBox)row.FindControl("txtRemainingWt")).Text = (double.Parse(((Label)row.FindControl("lblWeight")).Text) - (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text) - double.Parse(((TextBox)row.FindControl("txtRemainingWt")).Text))).ToString();
                                        //    drow["RemainingPcs"] = int.Parse(((TextBox)row.FindControl("txtRemainingPcs")).Text);
                                        //    drow["RemainingWt"] = double.Parse(((TextBox)row.FindControl("txtRemainingWt")).Text);
                                        //}
                                    }
                                }
                                Session["GHA_FlightPlanning_HAWBAddToULD"] = dsawb1_HAWB;
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        { }


                        ((CheckBox)gdvULDLoadPlanAWB.Rows[k].FindControl("Check2")).Checked = false;

                        AWBno = dsawb1.Rows[k]["AWBNumber"].ToString();
                        BookedPcs = int.Parse(dsawb1.Rows[k]["BookedPieces"].ToString());
                        BookedGwt = double.Parse(dsawb1.Rows[k]["BookedWeight"].ToString());
                        ULDDest = dsawb1.Rows[k]["DestinationCode"].ToString();
                        ULDOrigin = dsawb1.Rows[k]["OriginCode"].ToString();
                        Location = dsawb1.Rows[k]["Location"].ToString();
                        AcceptedPcs = int.Parse(dsawb1.Rows[k]["TotalPieces"].ToString());
                        AcceptedWt = double.Parse(dsawb1.Rows[k]["TotalWeight"].ToString());
                        FlightExists = dsawb1.Rows[k]["FlightExists"].ToString();
                        FDate = dsawb1.Rows[k]["FltDate"].ToString();
                        FlightNo = dsawb1.Rows[k]["FltNo"].ToString();
                        CommodityCode = dsawb1.Rows[k]["CommodityCode"].ToString();
                        CommodityDesc = dsawb1.Rows[k]["Description"].ToString();
                        ProductType = dsawb1.Rows[k]["ProductType"].ToString();
                        SHC = dsawb1.Rows[k]["SHC"].ToString();


                        //if (dsawb1.Rows[k]["AWBBuilderName"].ToString() == null || dsawb1.Rows[k]["AWBBuilderName"].ToString() == "")
                        //    BuilderName = dsawbData.Tables[1].Rows[0]["ULDBuilderName"].ToString();
                        //else
                        //    BuilderName = dsawb1.Rows[k]["AWBBuilderName"].ToString();

                        for (int i = 0; i < grdULDList.Rows.Count; i++)
                        {
                            if (((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
                            {
                                BuilderName = ((TextBox)grdULDList.Rows[i].FindControl("txtULDBuilderName")).Text.Trim();
                                break;
                            }
                        }

                        RemainingPCS = hdRemainingPcs.Value != string.Empty ? int.Parse(hdRemainingPcs.Value) : int.Parse(((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingPcs")).Text);
                        RemainingWGT = double.Parse(((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingWt")).Text);

                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltPcs")).Text = (int.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltPcs")).Text) + RemainingPCS).ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltWt")).Text = (double.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltWt")).Text) + RemainingWGT).ToString();

                        BuiltPcs = int.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltPcs")).Text);
                        BuiltWt = double.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltWt")).Text);


                        //((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text) - RemainingPCS).ToString();
                        //((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingWt")).Text = (double.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWeight")).Text) - RemainingWGT).ToString();

                        ((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text) - BuiltPcs).ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingWt")).Text = (double.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWeight")).Text) - BuiltWt).ToString();

                        dsawb1.Rows[k][3] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltPcs")).Text;
                        dsawb1.Rows[k][4] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltWt")).Text;
                        dsawb1.Rows[k][5] = ((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingPcs")).Text;
                        dsawb1.Rows[k][6] = ((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingWt")).Text;

                        dsawbData.Tables[0].Rows[k][3] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltPcs")).Text;
                        dsawbData.Tables[0].Rows[k][4] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltWt")).Text;
                        dsawbData.Tables[0].Rows[k][5] = ((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingPcs")).Text;
                        dsawbData.Tables[0].Rows[k][6] = ((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingWt")).Text;

                        tabMenifestDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];

                        int duplicnt = 0;
                        bool ismatch = false;
                        for (int chk = 0; chk < tabMenifestDetails.Rows.Count; chk++)
                        {
                            if (tabMenifestDetails.Rows[chk]["AWBno"].ToString() == ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAWBno")).Text
                                && tabMenifestDetails.Rows[chk]["ULDno"].ToString() == ULDNumber)
                            {
                                tabMenifestDetails.Rows[chk]["BuiltPcs"] = (int.Parse(tabMenifestDetails.Rows[chk]["BuiltPcs"].ToString()) + RemainingPCS).ToString();
                                tabMenifestDetails.Rows[chk]["BuiltWt"] = (double.Parse(tabMenifestDetails.Rows[chk]["BuiltWt"].ToString()) + RemainingWGT).ToString();
                                duplicnt = duplicnt + 1;
                                ismatch = true;
                                break;
                            }
                        }
                        if (ismatch == false)
                        {
                            DataRow l_Datarow = tabMenifestDetails.NewRow();

                            l_Datarow["AWBno"] = AWBno;
                            l_Datarow["TotalPcs"] = BookedPcs;
                            l_Datarow["TotalWt"] = BookedGwt;
                            l_Datarow["AcceptedPcs"] = AcceptedPcs;
                            l_Datarow["AcceptedWt"] = AcceptedWt;
                            //l_Datarow["BuiltPcs"] = BuiltPcs;
                            //l_Datarow["BuiltWt"] = BuiltWt;
                            l_Datarow["BuiltPcs"] = RemainingPCS;
                            l_Datarow["BuiltWt"] = RemainingWGT;
                            l_Datarow["ULDno"] = ULDNumber;
                            l_Datarow["AWBLoadingPriority"] = "";
                            l_Datarow["Location"] = Location;
                            l_Datarow["Origin"] = ULDOrigin;
                            l_Datarow["Dest"] = ULDDest;
                            l_Datarow["AWBBuilderName"] = BuilderName;
                            l_Datarow["FlightExists"] = FlightExists;
                            l_Datarow["FltNo"] = FlightNo;
                            l_Datarow["FltDate"] = FDate;
                            l_Datarow["CommodityCode"] = CommodityCode;
                            l_Datarow["Description"] = CommodityDesc;
                            l_Datarow["ProductType"] = ProductType;
                            l_Datarow["SHC"] = SHC;

                            tabMenifestDetails.Rows.Add(l_Datarow);
                        }

                        if (tabMenifestDetails != null && tabMenifestDetails.Rows.Count > 0)
                        {
                            Session["FlightPlanning_ManifestBulkAWBData"] = tabMenifestDetails;
                        }
                        grdBulkAssignedAWB.DataSource = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];
                        grdBulkAssignedAWB.DataBind();

                    }
                }


                for (int a = 0; a < dsawb2.Rows.Count; a++)
                {
                    if (int.Parse(((TextBox)gdvULDLoadPlanAWB.Rows[a].FindControl("txtRemainingPcs")).Text) == 0)
                    {
                        dsawb1.Rows[a].Delete();
                        dsawbData.Tables[0].Rows[a].Delete();
                    }
                }
                dsawb1.AcceptChanges();
                dsawbData.Tables[0].AcceptChanges();


                gdvULDLoadPlanAWB.DataSource = null;
                gdvULDLoadPlanAWB.DataBind();
                gdvULDLoadPlanAWB.DataSource = dsawb1;
                gdvULDLoadPlanAWB.DataBind();

                if (dsawb1.Rows.Count > 0)
                {
                    for (int m = 0; m < dsawb1.Rows.Count; m++)
                    {
                        //  tabMenifestDetails.Rows.Add(l_Datarow);
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblAWBno")).Text = dsawb1.Rows[m]["AWBNumber"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblPieces")).Text = dsawb1.Rows[m]["TotalPieces"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblWeight")).Text = dsawb1.Rows[m]["TotalWeight"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblBuiltPcs")).Text = dsawb1.Rows[m]["BuiltPcs"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblBuiltWt")).Text = dsawb1.Rows[m]["BuiltWt"].ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[m].FindControl("txtRemainingPcs")).Text = dsawb1.Rows[m]["RemainingPcs"].ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[m].FindControl("txtRemainingWt")).Text = dsawb1.Rows[m]["RemainingWt"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblLocation")).Text = dsawb1.Rows[m]["Location"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblBookedPcs")).Text = dsawb1.Rows[m]["BookedPieces"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblBookedWt")).Text = dsawb1.Rows[m]["BookedWeight"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblFlightExists")).Text = dsawb1.Rows[m]["FlightExists"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblProductType")).Text = dsawb1.Rows[m]["ProductType"].ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[m].FindControl("lblCommodityDesc")).Text = dsawb1.Rows[m]["Description"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblCommodityCode")).Text = dsawb1.Rows[m]["CommodityCode"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblSHC")).Text = dsawb1.Rows[m]["SHC"].ToString();
                        if (dsawb1.Rows[m][12].ToString() == "FN")
                        {
                            gdvULDLoadPlanAWB.Rows[m].BackColor = CommonUtility.ColorHighlightedGrid;
                        }
                    }
                }


                Session["FlightPlanning_AWBdata"] = dsawb1;// dsawbData.Tables[1];
                Session["FlightPlanning_AddToManifestAWBdata"] = dsawb1;

                ShowULDAWBSummary("");
                hdnManifestFlag.Value = "1";
                //Session[""] = (DataTable);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dsawbData != null)
                    dsawbData.Dispose();
                if (dsawb1 != null)
                    dsawb1.Dispose();
                if (dsawb2 != null)
                    dsawb2.Dispose();
                if (tabMenifestDetails != null)
                    tabMenifestDetails.Dispose();
            }

            pnlGrid.Visible = false;

            AllButtonStatus(true);
        }

        private void AllButtonStatus(bool res)
        {
            BtnAddtoManifest.Enabled = res;
            BtnClear.Enabled = res;
            BtnList.Enabled = res;
            btnShowEAWB.Enabled = res;
            btnSplitUnassign.Enabled = res;
            btnUnassign.Enabled = res;
        }

        protected void btnAssignBulk_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GetFlightPlanningStatus())
                    return;

                string CartNumber = "";
                
                if (txtCartNumber.Text == "")
                {
                    lblStatus.Text = "Please enter Cart number to assign AWB";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                CartNumber = txtCartNumber.Text;
                if (CartNumber == "")
                {
                    lblStatus.Text = "Please enter Cart number to assign AWB";
                    return;
                }
                #region Check if Cart is associated with flight which is not departed
                string ValidCart = objBuildULD.checkCartValidForSave(CartNumber, txtFlightCode.Text.ToUpper() +
                    txtFlightID.Text.ToUpper(), TextBoxdate.Text, Session["Station"].ToString());
                if (ValidCart == "INVALID")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Invalid Cart";
                    return;
                }
                else if (ValidCart == "NOT DEPARTED")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Cart associated with non departed flight";
                    return;
                }
                else if (ValidCart == "DIFFERENT FLIGHT")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Cart associated with another flight";
                    return;
                }
                #endregion Check if Cart is associated with flight which is not departed

                AssignAWBs(CartNumber, false, "", "", Session["Station"].ToString(), Session["FlightPlanning_Dest"].ToString(), "", 0, "0");
                return;

            }
            catch (Exception)
            { }
        }

        #region add AWBs to assigned Bulk AWBs
        public void AddAWBsToBulkAssignedAWBGrid()
        {
            DataSet dsawbData = new DataSet("FltPlan_AddAWBtoBulk_dsawbData");
            DataTable dsawb1 = new DataTable("FltPlan_AddAWBtoBulk_dsawb1");
            DataTable dsawb2 = new DataTable("FltPlan_AddAWBtoBulk_dsawb2");
            DataTable dsawb1_HAWB = new DataTable("FltPlan_AddAWBtoBulk_dsawb1_HAWB");
            DataTable dsawb2_HAWB = new DataTable("FltPlan_AddAWBtoBulk_dsawb2_HAWB");

            try
            {
                Label1.Text = "";
                string AWBno = "", ULDOrigin = "", ULDDest = "", Location = "", BuilderName = "", FlightExists = "", CartNumber = "";
                int BookedPcs = 0, RemainingPCS = 0, BuiltPcs = 0, AcceptedPcs = 0;
                double BookedGwt = 0, RemainingWGT = 0, BuiltWt = 0, AcceptedWt = 0;
                string FlightNo = string.Empty, FDate = string.Empty, CommodityDesc = string.Empty, SHC = string.Empty, CommodityCode = string.Empty, ProductType = string.Empty;

                //AWBs on Flight
                for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
                    {
                        if (Convert.ToInt32(((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text) > Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                        {
                            lblStatus.Text = "Please enter valid Pcs";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        if (Convert.ToDouble(((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text) > Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                        {
                            lblStatus.Text = "Please enter valid Weight";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        break;
                    }
                }

                //dsawbData = objBuildULD.GetAWBDetailsFromULD(ULDNumber);
                string flightID;
                if (txtFlightID.Text.Trim() != "")
                    flightID = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                else
                    flightID = "";

                string FlightDate = TextBoxdate.Text;

                string AWBPrefix = txtAWBPrefix.Text.Trim();
                string AWBNumber = txtAWBNo.Text.Trim();
                bool showByDest = false;
                if (Session["FlightPlanning_ShowDest"] != null &&
                    Convert.ToString(Session["FlightPlanning_ShowDest"]) != "")
                {
                    showByDest = Convert.ToBoolean(Session["FlightPlanning_ShowDest"]);
                }
                dsawbData = objBuildULD.GetAWBLoadPlanULDDetails(lblDepAirport.Text, flightID, FlightDate, AWBPrefix,
                    AWBNumber, "", showByDest);

                if (Session["FlightPlanning_AWBdata"] != null)
                {
                    dsawb1 = (DataTable)Session["FlightPlanning_AWBdata"];
                    dsawb2 = dsawb1.Copy();
                }
                else
                {
                    dsawb1 = dsawbData.Tables[0];
                }

                for (int k = 0; k < dsawb2.Rows.Count; k++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[k].FindControl("Check2")).Checked == true)
                    {
                        try
                        {
                            #region Code To Add HAWB Grid To Manifest Deepak Walmiki(22/05/2014)
                            {
                                dsawb1_HAWB = Session["GHA_FlightPlanning_HAWBAddToBulk"] != null ? ((DataTable)Session["GHA_FlightPlanning_HAWBAddToBulk"]).Copy() : ((DataSet)Session["GHA_FlightPlanning_HAWBOriginal"]).Tables[0].Copy();
                                dsawb2_HAWB = ((DataSet)Session["GHA_FlightPlanning_HAWBOriginal"]).Tables[0].Copy();

                                GridView gvHAWB = (GridView)gdvULDLoadPlanAWB.Rows[k].FindControl("GVSubHAWB");
                                foreach (GridViewRow row in gvHAWB.Rows)
                                {
                                    if (((CheckBox)row.FindControl("Check2")).Checked)
                                    {
                                        foreach (DataRow drow in dsawb1_HAWB.Rows)
                                        {
                                            if (((Label)row.FindControl("lblHAWBNo")).Text == drow["HAWBNo"].ToString() && ((Label)row.FindControl("lblAWBNo")).Text == drow["AWBNumber"].ToString() && drow["IsBulkOrULD"].ToString() != "U")
                                            {
                                                drow["TotalPieces"] = int.Parse(((TextBox)row.FindControl("txtRemainingPcs")).Text);
                                                drow["TotalWeight"] = double.Parse(((TextBox)row.FindControl("txtRemainingWt")).Text);
                                                //drow["BuiltPcs"] = (int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text) + int.Parse(((TextBox)row.FindControl("txtRemainingPcs")).Text));
                                                //drow["BuiltWt"] = (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text) + double.Parse(((TextBox)row.FindControl("txtRemainingWt")).Text));
                                                //((TextBox)row.FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)row.FindControl("lblPieces")).Text) - int.Parse(drow["BuiltPcs"].ToString())).ToString();
                                                //((TextBox)row.FindControl("txtRemainingWt")).Text = (double.Parse(((Label)row.FindControl("lblWeight")).Text) - double.Parse(drow["BuiltWt"].ToString())).ToString();
                                                drow["RemainingPcs"] = (int.Parse(((Label)row.FindControl("lblPieces")).Text) - int.Parse(drow["TotalPieces"].ToString())).ToString();
                                                drow["RemainingWt"] = (double.Parse(((Label)row.FindControl("lblWeight")).Text) - double.Parse(drow["TotalWeight"].ToString())).ToString();
                                                drow["IsChecked"] = "1";
                                                drow["IsBulkOrULD"] = "B";
                                            }
                                        }
                                        //foreach (DataRow drow in dsawb2_HAWB.Rows)
                                        //{
                                        //    drow["BuiltPcs"] = (int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text) + int.Parse(((TextBox)row.FindControl("txtRemainingPcs")).Text));
                                        //    drow["BuiltWt"] = (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text) + double.Parse(((TextBox)row.FindControl("txtRemainingWt")).Text));
                                        //    ((TextBox)row.FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)row.FindControl("lblPieces")).Text) - (int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text) - int.Parse(((TextBox)row.FindControl("txtRemainingPcs")).Text))).ToString();
                                        //    ((TextBox)row.FindControl("txtRemainingWt")).Text = (double.Parse(((Label)row.FindControl("lblWeight")).Text) - (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text) - double.Parse(((TextBox)row.FindControl("txtRemainingWt")).Text))).ToString();
                                        //    drow["RemainingPcs"] = int.Parse(((TextBox)row.FindControl("txtRemainingPcs")).Text);
                                        //    drow["RemainingWt"] = double.Parse(((TextBox)row.FindControl("txtRemainingWt")).Text);
                                        //}
                                    }
                                }
                                Session["GHA_FlightPlanning_HAWBAddToBulk"] = dsawb1_HAWB;
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        { }

                        ((CheckBox)gdvULDLoadPlanAWB.Rows[k].FindControl("Check2")).Checked = false;

                        AWBno = dsawb1.Rows[k]["AWBNumber"].ToString();
                        BookedPcs = int.Parse(dsawb1.Rows[k]["BookedPieces"].ToString());
                        BookedGwt = double.Parse(dsawb1.Rows[k]["BookedWeight"].ToString());
                        ULDDest = dsawb1.Rows[k]["DestinationCode"].ToString();
                        ULDOrigin = dsawb1.Rows[k]["OriginCode"].ToString();
                        Location = dsawb1.Rows[k]["Location"].ToString();
                        AcceptedPcs = int.Parse(dsawb1.Rows[k]["TotalPieces"].ToString());
                        AcceptedWt = double.Parse(dsawb1.Rows[k]["TotalWeight"].ToString());
                        FlightExists = dsawb1.Rows[k]["FlightExists"].ToString();
                        FDate = dsawb1.Rows[k]["FltDate"].ToString();
                        FlightNo = dsawb1.Rows[k]["FltNo"].ToString();
                        ProductType = dsawb1.Rows[k]["ProductType"].ToString();
                        CommodityDesc = dsawb1.Rows[k]["Description"].ToString();
                        CommodityCode = dsawb1.Rows[k]["CommodityCode"].ToString();
                        SHC = dsawb1.Rows[k]["SHC"].ToString();
                        CartNumber = txtCartNumber.Text;


                        RemainingPCS = int.Parse(((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingPcs")).Text);
                        RemainingWGT = double.Parse(((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingWt")).Text);

                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltPcs")).Text = (int.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltPcs")).Text) + RemainingPCS).ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltWt")).Text = (double.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltWt")).Text) + RemainingWGT).ToString();

                        BuiltPcs = int.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltPcs")).Text);
                        BuiltWt = double.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltWt")).Text);


                        //((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text) - RemainingPCS).ToString();
                        //((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingWt")).Text = (double.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWeight")).Text) - RemainingWGT).ToString();

                        ((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text) - BuiltPcs).ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingWt")).Text = (double.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWeight")).Text) - BuiltWt).ToString();

                        dsawb1.Rows[k][3] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltPcs")).Text;
                        dsawb1.Rows[k][4] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltWt")).Text;
                        dsawb1.Rows[k][5] = ((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingPcs")).Text;
                        dsawb1.Rows[k][6] = ((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingWt")).Text;

                        dsawbData.Tables[0].Rows[k][3] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltPcs")).Text;
                        dsawbData.Tables[0].Rows[k][4] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblBuiltWt")).Text;
                        dsawbData.Tables[0].Rows[k][5] = ((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingPcs")).Text;
                        dsawbData.Tables[0].Rows[k][6] = ((TextBox)gdvULDLoadPlanAWB.Rows[k].FindControl("txtRemainingWt")).Text;

                        tabAssignedBulkAWBDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];

                        int duplicnt = 0;
                        bool ismatch = false;
                        for (int chk = 0; chk < tabAssignedBulkAWBDetails.Rows.Count; chk++)
                        {
                            if (tabAssignedBulkAWBDetails.Rows[chk]["AWBno"].ToString() == ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAWBno")).Text
                                && tabAssignedBulkAWBDetails.Rows[chk]["CartNumber"].ToString() == CartNumber)
                            {
                                tabAssignedBulkAWBDetails.Rows[chk]["BuiltPcs"] = (int.Parse(tabAssignedBulkAWBDetails.Rows[chk]["BuiltPcs"].ToString()) + RemainingPCS).ToString();
                                tabAssignedBulkAWBDetails.Rows[chk]["BuiltWt"] = (double.Parse(tabAssignedBulkAWBDetails.Rows[chk]["BuiltWt"].ToString()) + RemainingWGT).ToString();
                                duplicnt = duplicnt + 1;
                                ismatch = true;
                                break;
                            }
                        }
                        if (ismatch == false)
                        {
                            DataRow l_Datarow = tabAssignedBulkAWBDetails.NewRow();

                            l_Datarow["AWBno"] = AWBno;
                            l_Datarow["TotalPcs"] = BookedPcs;
                            l_Datarow["TotalWt"] = BookedGwt;
                            l_Datarow["AcceptedPcs"] = AcceptedPcs;
                            l_Datarow["AcceptedWt"] = AcceptedWt;
                            //l_Datarow["BuiltPcs"] = BuiltPcs;
                            //l_Datarow["BuiltWt"] = BuiltWt;
                            l_Datarow["BuiltPcs"] = RemainingPCS;
                            l_Datarow["BuiltWt"] = RemainingWGT;
                            l_Datarow["ULDno"] = "";
                            l_Datarow["AWBLoadingPriority"] = "";
                            l_Datarow["Location"] = Location;
                            l_Datarow["Origin"] = ULDOrigin;
                            l_Datarow["Dest"] = ULDDest;
                            l_Datarow["AWBBuilderName"] = BuilderName;
                            l_Datarow["FlightExists"] = FlightExists;
                            l_Datarow["CartNumber"] = CartNumber;
                            l_Datarow["FltNo"] = FlightNo;
                            l_Datarow["FltDate"] = FDate;
                            l_Datarow["ProductType"] = ProductType;
                            l_Datarow["Description"] = CommodityDesc;
                            l_Datarow["CommodityCode"] = CommodityCode;
                            l_Datarow["SHC"] = SHC;

                            tabAssignedBulkAWBDetails.Rows.Add(l_Datarow);
                        }

                        if (tabAssignedBulkAWBDetails != null && tabAssignedBulkAWBDetails.Rows.Count > 0)
                        {
                            Session["FlightPlanning_ManifestBulkAWBData"] = tabAssignedBulkAWBDetails;
                        }

                        grdBulkAssignedAWB.DataSource = "";

                        grdBulkAssignedAWB.DataSource = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];
                        grdBulkAssignedAWB.DataBind();

                        Session["FlightPlanning_GDVBULKAWBDetails"] = tabAssignedBulkAWBDetails;
                    }
                }


                for (int a = 0; a < dsawb2.Rows.Count; a++)
                {
                    if (int.Parse(((TextBox)gdvULDLoadPlanAWB.Rows[a].FindControl("txtRemainingPcs")).Text) == 0)
                    {
                        dsawb1.Rows[a].Delete();
                        dsawbData.Tables[0].Rows[a].Delete();
                    }
                }
                dsawb1.AcceptChanges();
                dsawbData.Tables[0].AcceptChanges();


                gdvULDLoadPlanAWB.DataSource = null;
                gdvULDLoadPlanAWB.DataBind();
                gdvULDLoadPlanAWB.DataSource = dsawb1;
                gdvULDLoadPlanAWB.DataBind();

                if (dsawb1.Rows.Count > 0)
                {
                    for (int m = 0; m < dsawb1.Rows.Count; m++)
                    {
                        //  tabMenifestDetails.Rows.Add(l_Datarow);
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblAWBno")).Text = dsawb1.Rows[m]["AWBNumber"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblPieces")).Text = dsawb1.Rows[m]["TotalPieces"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblWeight")).Text = dsawb1.Rows[m]["TotalWeight"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblBuiltPcs")).Text = dsawb1.Rows[m]["BuiltPcs"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblBuiltWt")).Text = dsawb1.Rows[m]["BuiltWt"].ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[m].FindControl("txtRemainingPcs")).Text = dsawb1.Rows[m]["RemainingPcs"].ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[m].FindControl("txtRemainingWt")).Text = dsawb1.Rows[m][6].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblBookedPcs")).Text = dsawb1.Rows[m]["BookedPieces"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblBookedWt")).Text = dsawb1.Rows[m]["BookedWeight"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblFlightExists")).Text = dsawb1.Rows[m]["FlightExists"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblProductType")).Text = dsawb1.Rows[m]["ProductType"].ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[m].FindControl("lblCommodityDesc")).Text = dsawb1.Rows[m]["description"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[m].FindControl("lblCommodityCode")).Text = dsawb1.Rows[m]["CommodityCode"].ToString();
                        if (dsawb1.Rows[m]["FlightExists"].ToString() == "FN")
                        {
                            gdvULDLoadPlanAWB.Rows[m].BackColor = CommonUtility.ColorHighlightedGrid;
                        }
                    }
                }

                Session["FlightPlanning_AWBdata"] = dsawb1;// dsawbData.Tables[1];
                Session["FlightPlanning_AddToManifestAWBdata"] = dsawb1;

                //ShowULDAWBSummary();
                hdnManifestFlag.Value = "1";

            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dsawbData != null)
                    dsawbData.Dispose();
                if (dsawb1 != null)
                    dsawb1.Dispose();
                if (dsawb2 != null)
                    dsawb2.Dispose();
                if (tabAssignedBulkAWBDetails != null)
                    tabAssignedBulkAWBDetails.Dispose();
            }

            pnlGrid.Visible = false;

            AllButtonStatus(true);
        }
        #endregion add AWBs to assigned Bulk AWBs

        protected void btnReAssign_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable("FltPlan_btnReassign_dt");
            DataTable DTAWBDetails = new DataTable("FltPlan_btnReassign_DTAWBDetails");
            DataTable dt1 = new DataTable("FltPlan_btnReassign_dt1");
            DataTable dtCurrentTable = new DataTable("FltPlan_btnReassign__dtCurrentTable");
            Button btn = (Button)sender;
            try
            {
                lblStatus.Text = "";
                lblULDStatus.Text = "";
                ddlReason.Visible = true;

                int AWBcheck = 0, ULDCheck = 0;
                for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
                {
                    if (btn.CommandArgument.ToString() == "AWB")
                    {
                        if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                        {
                            if (((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblFlightExists")).Text == "FN")
                            {
                                lblStatus.Text = "AWB without flight number can not be reassigned";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }

                            AWBcheck = AWBcheck + 1;
                        }
                    }
                }

                //for (int i = 0; i < grdULDList.Rows.Count; i++)
                //{
                //    if (btn.CommandArgument.ToString() == "ULD")
                //    {
                //        if (((RadioButton)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
                //        {
                //            ULDCheck = ULDCheck + 1;
                //        }
                //    }
                //}

                if (AWBcheck == 0 && ULDCheck == 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select AWB(s) to Re-Assign";
                    return;
                }

                //if (AWBcheck == 0 || AWBcheck > 1)
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please Select Only one AWB for Re-Assign";
                //    return;
                //}

                //if (ULDCheck > 0 && AWBcheck > 0)
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please Select Only One ULD / AWB for Re-Assign";
                //    return;
                //}

                LoadAirlineCode("");
                grdRouting.Rows[0].Cells[7].Enabled = true;
                grdRouting.Rows[0].Cells[8].Enabled = true;
                //Added by Poorna
                Session["FlightPlanning_Split"] = "R";
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
                            if (Session["FlightPlanning_dsLoad"] != null)
                            {
                                DataSet dsLoad = new DataSet("FltPlan_btnReassign_dsLoad");
                                dsLoad = (DataSet)Session["FlightPlanning_dsLoad"];
                                if (dsLoad.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsLoad.Tables[0].Rows.Count; k++)
                                    {
                                        string awbno = dsLoad.Tables[0].Rows[k]["AWBNumber"].ToString();
                                        if (awbno == ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text.Trim())
                                        {
                                            POU = dsLoad.Tables[0].Rows[k]["DestinationCode"].ToString();

                                            break;
                                        }
                                    }
                                }
                            }

                            // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                            dr = DTAWBDetails.NewRow();
                            dr[0] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text;
                            dr[1] = ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text;
                            dr[2] = ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text;
                            dr[3] = ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text;
                            dr[4] = ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text;
                            dr[5] = POL;
                            dr[6] = POU;
                            dr[7] = "";
                            dr[8] = "";

                            DTAWBDetails.Rows.Add(dr);
                        }
                    }
                }
                else if (ULDCheck > 0)
                {
                    for (int j = 0; j < grdULDList.Rows.Count; j++)
                    {
                        if (((CheckBox)grdULDList.Rows[j].FindControl("rdULDList")).Checked == true)
                        {
                            DataRow dr;

                            string POL = lblDepAirport.Text;
                            string POU = "";

                            if (Session["FlightPlanning_dsLoad"] != null)
                            {
                                DataSet dsLoad = (DataSet)Session["FlightPlanning_dsLoad"];
                                if (dsLoad.Tables[1].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsLoad.Tables[1].Rows.Count; k++)
                                    {
                                        string ULDNo = dsLoad.Tables[1].Rows[k]["Uldno"].ToString();
                                        if (ULDNo == ((Label)grdULDList.Rows[j].FindControl("lblOldULDNo")).Text.Trim())
                                        {
                                            POU = dsLoad.Tables[1].Rows[k]["ULDDest"].ToString();
                                            break;
                                        }
                                    }
                                }
                            }

                            // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                            dr = DTAWBDetails.NewRow();
                            dr[0] = "";
                            dr[1] = ((Label)grdULDList.Rows[j].FindControl("lblAWBCount")).Text.Trim();
                            dr[2] = ((Label)grdULDList.Rows[j].FindControl("lblULDWt")).Text.Trim();
                            dr[3] = ((Label)grdULDList.Rows[j].FindControl("lblAWBCount")).Text.Trim();
                            dr[4] = ((Label)grdULDList.Rows[j].FindControl("lblULDWt")).Text.Trim();
                            dr[5] = POL;
                            dr[6] = POU;
                            dr[7] = ((Label)grdULDList.Rows[j].FindControl("lblOldULDNo")).Text.Trim();
                            dr[8] = "";

                            DTAWBDetails.Rows.Add(dr);
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
                pnlGrid.Style["TOP"] = "260px";

                try
                {
                    LoadAWBGrid();
                    dt1 = DTAWBDetails;
                    //dtCurrentTable = (DataTable)Session["FlightPlanning_AWBdata"];
                    dtCurrentTable = DTAWBDetails.Copy();
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

                            ViewState["CurrentTable1"] = dtCurrentTable;
                            //if (ULDCheck > 0)
                            //{
                            //    for (int z = 0; z < grdAWBs.Rows.Count; z++)
                            //    {
                            //        grdAWBs.Columns[0].Visible = false;
                            //        grdAWBs.Columns[1].Visible = false;
                            //        grdAWBs.Columns[2].Visible = false;
                            //        grdAWBs.Columns[3].Visible = false;
                            //        grdAWBs.Columns[4].Visible = false;
                            //    }
                            //}
                        }
                    }

                    # region Code to get Offload reasons

                    DataSet ds = new DataSet("FltPlan_btnReassign_ds");
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
                            ddlReason.DataTextField = "Reason";
                            ds.Tables[0].Rows.Add("Others");
                            ddlReason.DataSource = ds.Tables[0];
                            ddlReason.DataBind();
                            ddlReason.SelectedIndex = 0;

                        }
                        catch (Exception ex) { }
                    }
                    #endregion

                    //Clear Route
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text = string.Empty;
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedIndex = 0;
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Clear();
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Clear();
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text = string.Empty;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text = string.Empty;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtLocation")).Text = string.Empty;
                    }
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

        public void LoadAirlineCode(string filter)
        {
            BookingBAL objBLL = new BookingBAL();
            DataSet ds = new DataSet("FltPlan_LoadAirlineCode_ds");
            DropDownList ddl = new DropDownList();
            TextBox txtdest = new TextBox();

            try
            {
                //ds = objBLL.GetPartnerType(true);

                if (CommonUtility.PartnerTypeMaster == null)
                {
                    BookingBAL objBookingBal = new BookingBAL();
                    CommonUtility.PartnerTypeMaster = objBookingBal.GetPartnerType(true);
                    objBookingBal = null;
                }

                ds = CommonUtility.PartnerTypeMaster;

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
            finally
            {
                objBLL = null;
                if (ds != null)
                    ds.Dispose();
                ddl = null;
                txtdest = null;
            }
        }

        private void LoadAWBGrid()
        {

            DataTable myDataTable = new DataTable("FltPlan_LoadAAWBGrid_myDataTable");
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

            DataRow dr;
            dr = myDataTable.NewRow();
            //dr["RowNumber"] = 1;
            dr["AWBNo"] = "";//"5";
            dr["Pieces"] = "";// "5";
            dr["Weight"] = "";
            dr["AvlPCS"] = "";
            dr["AvlWgt"] = "";
            dr["ULDNo"] = "";

            myDataTable.Rows.Add(dr);
            ViewState["CurrentTable11"] = myDataTable;
            //Bind the DataTable to the Grid

            grdAWBs.DataSource = null;
            grdAWBs.DataSource = myDataTable;
            grdAWBs.DataBind();
            try
            {

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

        #region Un assign bulk AWBs
        protected void btnUnAssignBulkAWB_Click(object sender, EventArgs e)
        {
            if (!GetFlightPlanningStatus())
                return;

            DataTable DTAWBDetails = new DataTable("FltPlan_btnUnAssignBulk_DTAWBDetails");
            DataTable dtULDDetails = new DataTable("FltPlan_btnUnAssignBulk_dtULDDetails");

            try
            {

                UnassignAWBs();
                return;

                string strResult = string.Empty;
                lblStatus.Text = "";
                lblULDStatus.Text = "";
                int check = 0;
                string FLTno = "", FlightDate = "";
                if (txtFlightID.Text.Trim() != "")
                    FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                else
                    FLTno = "";

                if (TextBoxdate.Text.Trim() == "")
                    FlightDate = "";
                else
                    FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                int uldCount = 0;
                for (int i = 0; i < grdBulkAssignedAWB.Rows.Count; i++)
                {
                    if (((CheckBox)grdBulkAssignedAWB.Rows[i].FindControl("Check0")).Checked == true)
                    {
                        if (((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("lblULDNo")).Text != "")
                        {
                            uldCount = uldCount + 1;
                        }
                        else
                        {
                            check = check + 1;
                        }
                    }
                }

                if (check == 0)
                {
                    if (uldCount > 0)
                    {
                        btnUnassign_Click(this, new EventArgs());
                    }
                    return;
                }
                //if (check > 1)
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please Select only One AWB for UnAssign";
                //    return;
                //}


                bool blnFlag = false;
                DataSet dsresult = new DataSet("FltPlan_btnUnAssignBulk_dsresult");
                bool result = false;

                for (int i = 0; i < grdBulkAssignedAWB.Rows.Count; i++)
                {
                    if (((CheckBox)grdBulkAssignedAWB.Rows[i].FindControl("Check0")).Checked == true)
                    {
                        for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                        {
                            if (((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBNo")).Text ==
                                ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAWBNo")).Text &&
                                ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("lblULDNo")).Text == "")
                            {
                                if (gdvULDLoadPlanAWB.Rows[j].Visible == true)
                                {
                                    //to update bulk pieces label
                                    ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text = (Convert.ToInt32(((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text) + Convert.ToInt32(((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltPcs")).Text)).ToString();
                                    ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text = (Convert.ToDouble(((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text) + Convert.ToDouble(((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltWt")).Text)).ToString();

                                    //to update remaining pieces textbox
                                    ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text = (Convert.ToInt32(((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text) + Convert.ToInt32(((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltPcs")).Text)).ToString();
                                    ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text = (Convert.ToDouble(((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text) + Convert.ToDouble(((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltWt")).Text)).ToString();

                                    //to update Built pieces label
                                    ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblBuiltPcs")).Text = (Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text) - Convert.ToInt32(((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text)).ToString();
                                    ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblBuiltWt")).Text = (Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text) - Convert.ToDouble(((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text)).ToString();

                                    #region Code For Unassigning HAWB Deepak 23/05/2014
                                    try
                                    {

                                        DataTable dsawb1_HAWB = new DataTable("FltPlan_btnUnAssignBulk_dsawb1_HAWB");
                                        dsawb1_HAWB  = Session["GHA_FlightPlanning_HAWBAddToBulk"] != null ? ((DataTable)Session["GHA_FlightPlanning_HAWBAddToBulk"]).Copy() : ((DataSet)Session["GHA_FlightPlanning_HAWBOriginal"]).Tables[0].Copy();

                                        foreach (GridViewRow row in ((GridView)grdBulkAssignedAWB.Rows[j].FindControl("GVSubHAWB")).Rows)
                                        {
                                            //if (((CheckBox)row.FindControl("Check2")).Checked)
                                            //{
                                            foreach (DataRow drow in dsawb1_HAWB.Rows)
                                            {
                                                if (((Label)row.FindControl("lblHAWBNo")).Text == drow["HAWBNo"].ToString() && ((Label)row.FindControl("lblAWBNo")).Text == drow["AWBNumber"].ToString())
                                                {
                                                    foreach (GridViewRow SRow in ((GridView)gdvULDLoadPlanAWB.Rows[j].FindControl("GVSubHAWB")).Rows)
                                                    {
                                                        if (((Label)SRow.FindControl("lblHAWBNo")).Text == ((Label)row.FindControl("lblHAWBNo")).Text)
                                                        {
                                                            drow["TotalPieces"] = (int.Parse(((Label)SRow.FindControl("lblPieces")).Text) + int.Parse(((Label)row.FindControl("lblPieces")).Text));
                                                            drow["TotalWeight"] = (double.Parse(((Label)SRow.FindControl("lblWeight")).Text) + (double.Parse(((Label)row.FindControl("lblWeight")).Text)));
                                                            // drow["BuiltPcs"] = (int.Parse(((Label)SRow.FindControl("lblBuiltPcs")).Text) - int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text));
                                                            //drow["BuiltWt"] = (double.Parse(((Label)SRow.FindControl("lblBuiltWt")).Text) - (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text)));
                                                            //((TextBox)row.FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)row.FindControl("lblPieces")).Text) - int.Parse(drow["BuiltPcs"].ToString())).ToString();
                                                            //((TextBox)row.FindControl("txtRemainingWt")).Text = (double.Parse(((Label)row.FindControl("lblWeight")).Text) - double.Parse(drow["BuiltWt"].ToString())).ToString();
                                                            drow["RemainingPcs"] = (int.Parse(((TextBox)SRow.FindControl("txtRemainingPcs")).Text) + int.Parse(((Label)row.FindControl("lblPieces")).Text));
                                                            drow["RemainingWt"] = (double.Parse(((TextBox)SRow.FindControl("txtRemainingWt")).Text) + (double.Parse(((Label)row.FindControl("lblWeight")).Text)));
                                                            drow["IsChecked"] = "0";
                                                            drow["IsBulkOrULD"] = "";
                                                        }
                                                        //drow["BuiltPcs"] = (int.Parse(((Label)SRow.FindControl("lblBuiltPcs")).Text) - int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text));
                                                        //drow["BuiltWt"] = (double.Parse(((Label)SRow.FindControl("lblBuiltWt")).Text) - (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text)));
                                                        ////((TextBox)row.FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)row.FindControl("lblPieces")).Text) - int.Parse(drow["BuiltPcs"].ToString())).ToString();
                                                        ////((TextBox)row.FindControl("txtRemainingWt")).Text = (double.Parse(((Label)row.FindControl("lblWeight")).Text) - double.Parse(drow["BuiltWt"].ToString())).ToString();
                                                        //drow["RemainingPcs"] = (int.Parse(((TextBox)SRow.FindControl("txtRemainingPcs")).Text) + int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text));
                                                        //drow["RemainingWt"] = (double.Parse(((TextBox)SRow.FindControl("txtRemainingWt")).Text) + (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text)));
                                                        //drow["IsChecked"] = "0";
                                                        //drow["IsBulkOrULD"] = "";
                                                    }
                                                    //// drow["BulkPcs"] = (int.Parse(((TextBox)row.FindControl("txtRemainingPcs")).Text) + int.Parse(((Label)row.FindControl("lblPieces")).Text));
                                                    // //drow["BulkWt"] = (double.Parse(((TextBox)row.FindControl("txtRemainingWt")).Text) + double.Parse(((Label)row.FindControl("lblWeight")).Text));
                                                    // drow["BuiltPcs"] = (int.Parse(((TextBox)row.FindControl("txtRemainingPcs")).Text) + int.Parse(((Label)row.FindControl("lblPieces")).Text));
                                                    // drow["BuiltWt"] = (double.Parse(((TextBox)row.FindControl("txtRemainingWt")).Text) + double.Parse(((Label)row.FindControl("lblWeight")).Text));
                                                    // //((TextBox)row.FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)row.FindControl("lblPieces")).Text) - int.Parse(drow["BuiltPcs"].ToString())).ToString();
                                                    // //((TextBox)row.FindControl("txtRemainingWt")).Text = (double.Parse(((Label)row.FindControl("lblWeight")).Text) - double.Parse(drow["BuiltWt"].ToString())).ToString();
                                                    // drow["RemainingPcs"] = (int.Parse(((Label)row.FindControl("lblPieces")).Text) - (int.Parse(((TextBox)row.FindControl("txtRemainingPcs")).Text)));
                                                    // drow["RemainingWt"] = (double.Parse(((Label)row.FindControl("lblWeight")).Text) - (double.Parse(((TextBox)row.FindControl("txtRemainingWt")).Text)));
                                                    // drow["IsChecked"] = "0";
                                                    // drow["IsBulkOrULD"] = "";
                                                }
                                            }
                                            //}
                                        }
                                        Session["GHA_FlightPlanning_HAWBAddToBulk"] = dsawb1_HAWB;
                                    }
                                    catch (Exception ex)
                                    { }
                                    #endregion
                                    blnFlag = true;
                                }
                                else
                                {
                                    //Commented by Vijay 24/12/2013
                                    //((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text = gdvULDDetails.Rows[i].Cells[5].Text;
                                    //((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text = gdvULDDetails.Rows[i].Cells[6].Text;
                                    //((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblBulksBagsPcs")).Text = gdvULDDetails.Rows[i].Cells[5].Text;
                                    //((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblBulksBagsWt")).Text = gdvULDDetails.Rows[i].Cells[6].Text;
                                    ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltPcs")).Text;
                                    ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltWt")).Text;

                                    gdvULDLoadPlanAWB.Rows[j].Visible = true;
                                    blnFlag = true;
                                    #region Code For Unassigning HAWB Deepak 23/05/2014
                                    try
                                    {
                                        DataTable dsawb1_HAWB = new DataTable("FltPlan_btnUnAssignBulk_dsawb1_HAWB");
                                        dsawb1_HAWB = Session["GHA_FlightPlanning_HAWBAddToBulk"] != null ? ((DataTable)Session["GHA_FlightPlanning_HAWBAddToBulk"]).Copy() : ((DataSet)Session["GHA_FlightPlanning_HAWBOriginal"]).Tables[0].Copy();

                                        foreach (GridViewRow row in ((GridView)gdvULDLoadPlanAWB.Rows[j].FindControl("GVSubHAWB")).Rows)
                                        {
                                            //if (((CheckBox)row.FindControl("Check2")).Checked)
                                            //{
                                            foreach (DataRow drow in dsawb1_HAWB.Rows)
                                            {
                                                if (((Label)row.FindControl("lblHAWBNo")).Text == drow["HAWBNo"].ToString() && ((Label)row.FindControl("lblAWBNo")).Text == drow["AWBNumber"].ToString())
                                                {
                                                    drow["RemainingPcs"] = int.Parse(((Label)row.FindControl("lblPieces")).Text);
                                                    drow["RemainingWt"] = double.Parse(((Label)row.FindControl("lblWeight")).Text);
                                                    drow["IsChecked"] = "0";
                                                    drow["IsBulkOrULD"] = "";
                                                }
                                            }
                                            //}
                                        }
                                        Session["GHA_FlightPlanning_HAWBAddToBulk"] = dsawb1_HAWB;
                                    }
                                    catch (Exception ex)
                                    { }
                                    #endregion
                                }
                            }
                        }

                        //Commented by Vijay - 24/12/2013
                        if (blnFlag == false)
                        {
                            //Added by Poorna

                            DTAWBDetails = new DataTable("FltPlan_btnUnAssignBulk_DTAWBDetails");
                            DTAWBDetails = (DataTable)Session["FlightPlanning_AWBdata"];

                            if (DTAWBDetails == null)
                            {
                                DTAWBDetails = new DataTable("FltPlan_btnUnAssignBulk_DTAWBDetails");

                                DTAWBDetails.Columns.Add("lblAWBno");
                                DTAWBDetails.Columns.Add("lblPieces");
                                DTAWBDetails.Columns.Add("lblWeight");
                                DTAWBDetails.Columns.Add("lblBuiltPcs");
                                DTAWBDetails.Columns.Add("lblBuiltWt");
                                DTAWBDetails.Columns.Add("txtRemainingPcs");
                                DTAWBDetails.Columns.Add("txtRemainingWt");
                                DTAWBDetails.Columns.Add("lblOrigin");
                                DTAWBDetails.Columns.Add("lblDest");
                                DTAWBDetails.Columns.Add("lblLocation");
                                DTAWBDetails.Columns.Add("lblBookedPcs");
                                DTAWBDetails.Columns.Add("lblBookedWt");
                                DTAWBDetails.Columns.Add("lblFlightExists");
                                DTAWBDetails.Columns.Add("FltNo");
                                DTAWBDetails.Columns.Add("FltDate");
                                DTAWBDetails.Columns.Add("AWBNo");

                            }

                            if (((CheckBox)grdBulkAssignedAWB.Rows[i].FindControl("Check0")).Checked == true)
                            {
                                DataRow dr;

                                dr = DTAWBDetails.NewRow();
                                dr[0] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAWBNo")).Text;
                                dr[1] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAcceptedPcs")).Text;
                                dr[2] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAcceptedWt")).Text;
                                dr[3] = 0;
                                dr[4] = 0;
                                dr[5] = int.Parse(((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAcceptedPcs")).Text);
                                dr[6] = double.Parse(((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAcceptedWt")).Text);
                                dr[7] = "";
                                dr[8] = "";
                                //dr[9] = grdBulkAssignedAWB.Rows[i].Cells[7].Text;
                                dr[9] = ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("txtAWBLocation")).Text;
                                dr[10] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblTotalPcs")).Text;
                                dr[11] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblTotalWt")).Text;
                                dr[12] = "N";

                                DTAWBDetails.Rows.Add(dr);

                                #region Code For Unassigning HAWB Deepak 23/05/2014
                                try
                                {
                                    DataTable dsawb1_HAWB = new DataTable("FltPlan_btnUnAssignBulk_dsawb1_HAWB");
                                    dsawb1_HAWB = Session["GHA_FlightPlanning_HAWBAddToBulk"] != null ? ((DataTable)Session["GHA_FlightPlanning_HAWBAddToBulk"]).Copy() : ((DataSet)Session["GHA_FlightPlanning_HAWBOriginal"]).Tables[0].Copy();

                                    foreach (GridViewRow row in ((GridView)grdBulkAssignedAWB.Rows[i].FindControl("GVSubHAWB")).Rows)
                                    {
                                        //if (((CheckBox)row.FindControl("Check2")).Checked)
                                        //{
                                        foreach (DataRow drow in dsawb1_HAWB.Rows)
                                        {
                                            if (((Label)row.FindControl("lblHAWBNo")).Text == drow["HAWBNo"].ToString() && ((Label)row.FindControl("lblAWBNo")).Text == drow["AWBNumber"].ToString())
                                            {
                                                drow["TotalPieces"] = (int.Parse(((Label)row.FindControl("lblPieces")).Text));
                                                drow["TotalWeight"] = (double.Parse(((Label)row.FindControl("lblWeight")).Text));
                                                drow["IsChecked"] = "0";
                                                drow["IsBulkOrULD"] = "";
                                            }
                                        }
                                        //}
                                    }
                                    Session["GHA_FlightPlanning_HAWBAddToBulk"] = dsawb1_HAWB;
                                }
                                catch (Exception ex)
                                { }
                                #endregion
                            }
                        }


                        Session["FlightPlanning_AWBdata"] = DTAWBDetails;

                        //Commented by Vijay - 24/12/2013
                        if (DTAWBDetails != null)
                            LoadAWBDataGrid(DTAWBDetails);

                        dtULDDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];// (DataTable)Session["GDVULDDetails"];

                        if (dtULDDetails != null)
                        {
                            if (dtULDDetails.Rows[i]["AWBno"].ToString() != "")
                            {
                                //dsresult = objBuildULD.DeleteBulkAssignedAWBDetails(dtULDDetails.Rows[i]["AWBno"].ToString(), int.Parse(dtULDDetails.Rows[i]["BuiltPcs"].ToString()), float.Parse(dtULDDetails.Rows[i]["BuiltWt"].ToString()), FLTno, FlightDate, dtULDDetails.Rows[i]["Origin"].ToString(), dtULDDetails.Rows[i]["Dest"].ToString(), "D", ref strResult);
                                dsresult = objBuildULD.DeleteBulkAssignedAWBDetailsFromCart(dtULDDetails.Rows[i]["AWBno"].ToString(), int.Parse(dtULDDetails.Rows[i]["BuiltPcs"].ToString()), float.Parse(dtULDDetails.Rows[i]["BuiltWt"].ToString()), FLTno, FlightDate, dtULDDetails.Rows[i]["Origin"].ToString(), dtULDDetails.Rows[i]["Dest"].ToString(), "D", dtULDDetails.Rows[i]["CartNumber"].ToString(), ref strResult);
                                #region Saving HAWBDetails After Unassign
                                try
                                {
                                    //if (Session["GHA_AssignULD_HAWBAddToBulk"] != null)
                                    //{
                                    //    foreach (DataRow row in ((DataTable)Session["GHA_AssignULD_HAWBAddToBulk"]).Rows)
                                    //    {
                                    //        if (row["AWBNumber"].ToString() == dtULDDetails.Rows[i]["AWBno"].ToString() && row["isBulkOrULD"].ToString() == string.Empty)
                                    //        {
                                    //            objBuildULD.DeleteBulkAssignedAWBDetails(row["HAWBNo"].ToString(), int.Parse(row["TotalPieces"].ToString()), float.Parse(row["TotalWeight"].ToString()), FLTno, FlightDate, row["OriginCode"].ToString(),
                                    //                row["DestinationCode"].ToString(), "D", ref strResult);
                                    //        }
                                    //    }
                                    //}
                                    foreach (GridViewRow grow in ((GridView)grdBulkAssignedAWB.Rows[i].FindControl("GVSubHAWB")).Rows)
                                    {
                                        if (dtULDDetails.Rows[i]["AWBno"].ToString() == ((Label)grow.FindControl("lblAWBNo")).Text)
                                        {
                                            objBuildULD.DeleteBulkAssignedAWBDetails(((Label)grow.FindControl("lblHAWBNo")).Text, int.Parse(((Label)grow.FindControl("lblPieces")).Text), float.Parse(((Label)grow.FindControl("lblWeight")).Text), FLTno, FlightDate, dtULDDetails.Rows[i]["Origin"].ToString(),
                                                dtULDDetails.Rows[i]["Dest"].ToString(), "D", ref strResult);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                { }
                                #endregion

                            }

                            dtULDDetails.Rows.RemoveAt(i);
                        }

                        if (dsresult != null)
                        {
                            if (dsresult.Tables != null)
                            {
                                if (dsresult.Tables.Count > 0)
                                {
                                    if (dsresult.Tables[0].Rows[0][0].ToString() == "Bulk unassigned")
                                    {
                                        result = true;
                                    }
                                }

                            }
                        }
                        if (result == true)
                        {
                            BtnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Bulk AWB unassigned successfully";
                        }
                        else
                        {
                            BtnList_Click(null, null);
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Bulk AWB not unassigned";
                            if (uldCount > 0)
                            {
                                btnUnassign_Click(this, new EventArgs());
                            }
                            return;
                        }

                        //grdBulkAssignedAWB.DataSource = dtULDDetails;
                        //grdBulkAssignedAWB.DataBind();

                        //Session["GDVBULKAWBDetails"] = dtULDDetails;
                        //Session["ManifestBulkAWBData"] = dtULDDetails;

                        //ShowULDAWBSummary();
                        //hdnManifestFlag.Value = "1";
                    }
                }
                if (uldCount > 0)
                {
                    btnUnassign_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (DTAWBDetails != null)
                    DTAWBDetails.Dispose();
                if (dtULDDetails != null)
                    dtULDDetails.Dispose();
            }
        }
        #endregion Un assign bulk AWBs

        private void LoadAWBDataGrid(DataTable pi_objDataTable)
        {
            DataTable MemDetails = new DataTable("FltPlan_LoadAWBGrid_MemDetails"); // (DataTable)Session["AWBdata"];

            try
            {
                MemDetails.Columns.Add("lblAWBno");
                MemDetails.Columns.Add("lblPieces");
                MemDetails.Columns.Add("lblWeight");
                MemDetails.Columns.Add("lblBuiltPcs");
                MemDetails.Columns.Add("lblBuiltWt");
                MemDetails.Columns.Add("txtRemainingPcs");
                MemDetails.Columns.Add("txtRemainingWt");
                MemDetails.Columns.Add("lblLocation");
                MemDetails.Columns.Add("BookedPieces");
                MemDetails.Columns.Add("BookedWeight");
                MemDetails.Columns.Add("lblFlightExists");
                MemDetails.Columns.Add("FltNo");
                MemDetails.Columns.Add("FltDate");
                MemDetails.Columns.Add("AWBNo");

                for (int j = 0; j < pi_objDataTable.Rows.Count; j++)
                {
                    DataRow l_Datarow = MemDetails.NewRow();

                    MemDetails.Rows.Add(l_Datarow);
                }
                gdvULDLoadPlanAWB.DataSource = (DataTable)MemDetails;
                gdvULDLoadPlanAWB.DataBind();

                if (pi_objDataTable != null)
                {
                    for (int i = 0; i < pi_objDataTable.Rows.Count; i++)
                    {
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAWBno")).Text = pi_objDataTable.Rows[i][0].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblPieces")).Text = pi_objDataTable.Rows[i][1].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblWeight")).Text = pi_objDataTable.Rows[i][2].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblBuiltPcs")).Text = pi_objDataTable.Rows[i][3].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblBuiltWt")).Text = pi_objDataTable.Rows[i][4].ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[i].FindControl("txtRemainingPcs")).Text = pi_objDataTable.Rows[i][5].ToString();
                        ((TextBox)gdvULDLoadPlanAWB.Rows[i].FindControl("txtRemainingWt")).Text = pi_objDataTable.Rows[i][6].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblLocation")).Text = pi_objDataTable.Rows[i][9].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblBookedPcs")).Text = pi_objDataTable.Rows[i][10].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblBookedWt")).Text = pi_objDataTable.Rows[i][11].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblFlightExists")).Text = pi_objDataTable.Rows[i][12].ToString();
                        //((DataRowView)gdvULDDetails.Rows[i].DataItem)["FltDate"] = pi_objDataTable.Rows[i]["FltDate"].ToString();
                        //((DataRowView)gdvULDDetails.Rows[i].DataItem)["FltNo"] = pi_objDataTable.Rows[i]["FltNo"].ToString();
                        //((DataRowView)gdvULDDetails.Rows[i].DataItem)["AWBNo"] = pi_objDataTable.Rows[i][0].ToString(); 

                        if (pi_objDataTable.Rows[i][12].ToString() == "FN")
                        {
                            gdvULDLoadPlanAWB.Rows[i].BackColor = CommonUtility.ColorHighlightedGrid;
                        }

                    }
                    Session["FlightPlanning_AWBdata"] = pi_objDataTable;
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

        #region btnSaveBulkAWB_Click
        protected void btnSaveBulkAWB_Click(object sender, EventArgs e)
        {
            if (!GetFlightPlanningStatus())
                return;

            //Session["FltPlan_SelectCartRow"] = -1;
            SaveAssignedAWB();
            return;
        }
        #endregion btnSaveBulkAWB_Click

        #region Save All AWBs
        protected void SaveAWBsByCart(string CartNumber)
        {
            bool res = false;
            string FLTno = txtFlightCode.Text + txtFlightID.Text;
            string FltDate = TextBoxdate.Text;
            string POL = Session["Station"].ToString();
            try
            {
                //To save bulk assigned AWBs
                tabAssignedBulkAWBDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];

                if (grdBulkAssignedAWB.Rows.Count > 0)
                {
                    CartNumber = txtCartNumber.Text.ToUpper();
                    #region Check if Cart is associated with flight which is not departed
                    string ValidCart = objBuildULD.checkCartValidForSave(CartNumber, FLTno, FltDate, POL);
                    if (ValidCart == "INVALID")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Invalid Cart";
                        return;
                    }
                    else if (ValidCart == "NOT DEPARTED")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Cart associated with non departed flight";
                        return;
                    }
                    else if (ValidCart == "DIFFERENT FLIGHT")
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Cart associated with another flight";
                        return;
                    }
                    #endregion Check if Cart is associated with flight which is not departed
                    for (int i = 0; i < grdBulkAssignedAWB.Rows.Count; i++)
                    {
                        if (((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("lblCartNo")).Text != txtCartNumber.Text)
                        { //If AWB does not belong to current Cart.
                            continue;
                        }
                        else
                        {
                            res = SaveCartAWB(i);
                        }
                    }
                }

                if (res == true)
                {
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "Cart Saved Successfully";
                    return;
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion btnSaveBulkAWB_Click

        #region SaveCartAWB
        protected bool SaveCartAWB(int indexAssignedAWBs)
        {
            bool res = false;
            DataSet dsBulkResult = new DataSet("FltPlan_SaveCartAWB_dsBulkResult");
            string CartNumber = "", CartLoadingPriority = "", CartBuilderName = "";
            double ScaleWeight = 0.0;
            string FLTno = "", Updatedby, POL = "", FltDate;
            string Updatedon = "";

            try
            {
                //To save bulk assigned AWBs
                tabAssignedBulkAWBDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];

                if (indexAssignedAWBs >= 0)
                {
                    CartNumber = txtCartNumber.Text.ToUpper();

                    string AWBno = "", AWBPrefix = "", AWBLocation = "";
                    int BuiltPcs = 0;
                    double BuiltWt = 0.0;
                    string AWBLoadingPriority = "", AWBBuilderName = "";
                    string ULDOrigin = "", ULDDest = "", CartNo = "";

                    if (txtFlightID.Text.Trim() != "")
                        FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                    else
                        FLTno = "";
                    Updatedby = Convert.ToString(Session["Username"]);

                    POL = Session["Station"].ToString();

                    AWBno = ((Label)grdBulkAssignedAWB.Rows[indexAssignedAWBs].FindControl("lblAWBNo")).Text;

                    AWBPrefix = AWBno.Substring(0, AWBno.IndexOf("-"));
                    AWBno = AWBno.Substring(AWBno.Length - 8);
                    BuiltPcs = Convert.ToInt32(((Label)grdBulkAssignedAWB.Rows[indexAssignedAWBs].FindControl("lblBuiltPcs")).Text);
                    BuiltWt = Convert.ToDouble(((Label)grdBulkAssignedAWB.Rows[indexAssignedAWBs].FindControl("lblBuiltWt")).Text);
                    AWBLoadingPriority = ((TextBox)grdBulkAssignedAWB.Rows[indexAssignedAWBs].FindControl("txtAWBLoadingPriority")).Text;
                    AWBLocation = ((TextBox)grdBulkAssignedAWB.Rows[indexAssignedAWBs].FindControl("txtAWBLocation")).Text;
                    ULDOrigin = tabAssignedBulkAWBDetails.Rows[indexAssignedAWBs]["Origin"].ToString();
                    ULDDest = tabAssignedBulkAWBDetails.Rows[indexAssignedAWBs]["Dest"].ToString();
                    Updatedon = Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss");
                    AWBBuilderName = ((TextBox)grdBulkAssignedAWB.Rows[indexAssignedAWBs].FindControl("txtAWBBuilderName")).Text;
                    //added by jayant on 21/03/2014 for cart number
                    //CartNo = ((DropDownList)grdBulkAssignedAWB.Rows[i].FindControl("ddlCart")).SelectedItem.Text;
                    //end cartno
                    tabAssignedBulkAWBDetails.Rows[indexAssignedAWBs]["Location"] = AWBLocation;
                    tabAssignedBulkAWBDetails.Rows[indexAssignedAWBs]["AWBLoadingPriority"] = AWBLoadingPriority;
                    tabAssignedBulkAWBDetails.Rows[indexAssignedAWBs]["AWBBuilderName"] = AWBBuilderName;

                    if (TextBoxdate.Text.Trim() == "")
                        FltDate = "";
                    else
                        FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                    if (Session["Station"].ToString() == POL)
                    {
                        string strResult = string.Empty;

                        //dsBulkResult = objBuildULD.AddBulkAssignedAWBDetails(AWBPrefix, AWBno, BuiltPcs, BuiltWt, FLTno, FltDate, ULDOrigin, ULDDest, Updatedon, Updatedby, AWBLocation, AWBLoadingPriority, AWBBuilderName, ref strResult, CartNumber, CartLoadingPriority, CartBuilderName);
                        dsBulkResult = objBuildULD.AddBulkAssignedAWBDetailsFromCart(AWBPrefix, AWBno, BuiltPcs,
                            BuiltWt, FLTno, FltDate, ULDOrigin, ULDDest, Updatedon, Updatedby, AWBLocation,
                            AWBLoadingPriority, AWBBuilderName, ref strResult, CartNumber, CartLoadingPriority,
                            CartBuilderName, ScaleWeight, "", "");
                        #region Saving HAWB
                        foreach (GridViewRow row in ((GridView)grdBulkAssignedAWB.Rows[indexAssignedAWBs].FindControl("GVSubHAWB")).Rows)
                        {
                            string HAWBNumber = ((Label)row.FindControl("lblHAWBNo")).Text;
                            if (HAWBNumber.Contains('-'))
                            {
                                string[] HAWBDetails = ((Label)row.FindControl("lblHAWBNo")).Text.Split('-');
                                objBuildULD.AddBulkAssignedAWBDetails(HAWBDetails[0], HAWBDetails[1], int.Parse(((Label)row.FindControl("lblPieces")).Text), double.Parse(((Label)row.FindControl("lblWeight")).Text), FLTno, FltDate, ULDOrigin, ULDDest, Updatedon, Updatedby, AWBLocation, AWBLoadingPriority, AWBBuilderName, ref strResult, CartNo);
                            }
                            else
                            {
                                objBuildULD.AddBulkAssignedAWBDetails("", HAWBNumber, int.Parse(((Label)row.FindControl("lblPieces")).Text), double.Parse(((Label)row.FindControl("lblWeight")).Text), FLTno, FltDate, ULDOrigin, ULDDest, Updatedon, Updatedby, AWBLocation, AWBLoadingPriority, AWBBuilderName, ref strResult, CartNo);
                            }
                        }
                        #endregion
                        //cls_BL.addMsgToOutBox("SCM", "Started ULD Build for " + ULDNumber, "", "");
                        if (dsBulkResult != null)
                        {
                            if (dsBulkResult.Tables != null)
                            {
                                if (dsBulkResult.Tables.Count > 0)
                                {
                                    if (dsBulkResult.Tables[0].Rows[0][0].ToString() == "Bulk AWBs saved successfully")
                                    {
                                        res = true;
                                    }
                                    else
                                    {
                                        res = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return (res);
        }
        #endregion SaveCartAWB

        #region btnNewULD_Click
        protected void btnNewULD_Click(object sender, EventArgs e)
        {

            if (!GetFlightPlanningStatus())
                return;
            //remove previous data from session of AWBs assigned to ULD
            ((DataTable)Session["FlightPlanning_ManifestBulkAWBData"]).Rows.Clear();

            DataTable dtNewULDList = new DataTable("FltPlan_btnNewULD_dtNewULDList");
            dtNewULDList = (DataTable)Session["FlightPlanning_ULDList"];
            DataSet dsNewULD = new DataSet("FltPlan_btnNewULD_dsNewULD");

            try
            {
                lblStatus.Text = "";
                lblULDStatus.Text = "";
                string flightID;
                if (txtFlightID.Text.Trim() != "")
                    flightID = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                else
                    flightID = "";

                string FlightDate = TextBoxdate.Text;

                string AWBPrefix = txtAWBPrefix.Text.Trim();
                string AWBNumber = txtAWBNo.Text.Trim();
                string ULDNumber = "";
                dsNewULD = objBuildULD.GetAWBLoadPlanULDDetails(lblDepAirport.Text, flightID, FlightDate, AWBPrefix, AWBNumber, ULDNumber);

                DataRow l_Datarow = dtNewULDList.NewRow();
                l_Datarow["ULDno"] = "";
                //l_Datarow["ULDOrigin"] = dsNewULD.Tables[0].Rows[0]["OriginCode"].ToString();
                l_Datarow["ULDOrigin"] = Convert.ToString(Session["Station"]);
                //l_Datarow["ULDDest"] = dsNewULD.Tables[0].Rows[0]["DestinationCode"].ToString();
                l_Datarow["ULDDest"] = "";
                l_Datarow["AWBCount"] = "0";
                l_Datarow["Location"] = "";
                l_Datarow["LoadingPriority"] = "";
                l_Datarow["TareWt"] = "0";
                l_Datarow["ULDWt"] = "0";
                l_Datarow["ScaleWeight"] = "0";
                l_Datarow["ContainerType"] = "";
                l_Datarow["ULDStatus"] = "Empty";
                l_Datarow["ULDBuilderName"] = "";
                l_Datarow["AWBNumber"] = "-";

                dtNewULDList.Rows.Add(l_Datarow);

                grdULDList.DataSource = dtNewULDList;
                grdULDList.DataBind();
                Session["FlightPlanning_ULDList"] = dtNewULDList;

                if (dtNewULDList != null)
                {
                    if (dtNewULDList.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtNewULDList.Rows.Count; i++)
                        {
                            if (dtNewULDList.Rows[i][0].ToString() != "")
                            {
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDNo")).Text = dtNewULDList.Rows[i]["ULDno"].ToString();
                                ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text = dtNewULDList.Rows[i]["ULDno"].ToString();
                                //((TextBox)grdULDList.Rows[i].FindControl("txtULDNo")).Enabled = false;
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOL")).Text = dtNewULDList.Rows[i]["ULDOrigin"].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOU")).Text = dtNewULDList.Rows[i]["ULDDest"].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDLocation")).Text = dtNewULDList.Rows[i]["Location"].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDLoadingPriority")).Text = dtNewULDList.Rows[i]["LoadingPriority"].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtScaleWeight")).Text = dtNewULDList.Rows[i]["ScaleWeight"].ToString();

                                if (dtNewULDList.Rows[i][9].ToString() != "")
                                    ((DropDownList)grdULDList.Rows[i].FindControl("ddlContainerType")).SelectedItem.Text = dtNewULDList.Rows[i]["ContainerType"].ToString();

                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDBuilderName")).Text = dtNewULDList.Rows[i]["ULDBuilderName"].ToString();
                                ((Label)grdULDList.Rows[i].FindControl("lblAWBNumber")).Text = dtNewULDList.Rows[i]["AWBNumber"].ToString();
                            }
                            else
                            {
                                ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text = dtNewULDList.Rows[i]["ULDno"].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDNo")).Text = dtNewULDList.Rows[i]["ULDno"].ToString();
                                //((TextBox)grdULDList.Rows[i].FindControl("txtULDNo")).Enabled = true;
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOL")).Text = dtNewULDList.Rows[i]["ULDOrigin"].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOU")).Text = dtNewULDList.Rows[i]["ULDDest"].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDLocation")).Text = dtNewULDList.Rows[i]["Location"].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDLoadingPriority")).Text = dtNewULDList.Rows[i]["LoadingPriority"].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtScaleWeight")).Text = dtNewULDList.Rows[i]["ScaleWeight"].ToString();
                                ((TextBox)grdULDList.Rows[i].FindControl("txtULDBuilderName")).Text = dtNewULDList.Rows[i]["ULDBuilderName"].ToString();
                                ((Label)grdULDList.Rows[i].FindControl("lblAWBNumber")).Text = dtNewULDList.Rows[i]["AWBNumber"].ToString();
                            }
                        }
                        ((CheckBox)grdULDList.Rows[dsNewULD.Tables[1].Rows.Count].FindControl("rdULDList")).Checked = true;

                        //uncheck all radio buttons of Cart list when new ULD created
                        for (int k = 0; k < grdCartList.Rows.Count; k++)
                        {
                            ((CheckBox)grdCartList.Rows[k].FindControl("rdCartList")).Checked = false;
                        }
                    }
                }

                //To show blank manifest grid when new ULD row is added and checked.
                //BindAWBManifestULD("", "", "", "");

                HideCartAWBDetails();

                ShowULDAWBSummary("");
            }
            catch { }
            finally
            {
                if (dtNewULDList != null)
                    dtNewULDList.Dispose();
                if (dsNewULD != null)
                    dsNewULD.Dispose();

            }
        }
        #endregion btnNewULD_Click

        protected void Save1_Click(object sender, EventArgs e)
        {

            SaveULDs();
            return;
            DataSet dsResult = new DataSet("FltPlan_Save1_dsResult");
            DataSet dsValidDest = new DataSet("FltPlan_Save1_dsValidDest");
            lblStatus.Text = "";
            lblULDStatus.Text = "";
            try
            {

                string ULDLocation = "", ULDLoadingPriority = "", ULDBuilderName = "";
                string ULDNumber = "", ContainerType = "";
                double ScaleWeight = 0.0;
                string ULDOrigin = "", ULDDest = "", DollyWt = "";
                TextBox txtScaleWt = null;
                for (int i = 0; i < grdULDList.Rows.Count; i++)
                {
                    if (((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
                    {
                        ULDNumber = ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text.ToUpper();
                        hfULDNumber.Value = ULDNumber.ToUpper();
                        ULDLocation = ((TextBox)grdULDList.Rows[i].FindControl("txtULDLocation")).Text;
                        ULDLoadingPriority = ((TextBox)grdULDList.Rows[i].FindControl("txtULDLoadingPriority")).Text;

                        ContainerType = ((DropDownList)grdULDList.Rows[i].FindControl("ddlContainerType")).SelectedItem.Text.ToString();
                        if (!double.TryParse(((TextBox)grdULDList.Rows[i].FindControl("txtScaleWeight")).Text, out ScaleWeight))
                            ScaleWeight = 0;
                        txtScaleWt = (TextBox)grdULDList.Rows[i].FindControl("txtScaleWeight");

                        ULDOrigin = ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOL")).Text.ToUpper();
                        ULDDest = ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOU")).Text.ToUpper();

                        ULDBuilderName = ((TextBox)grdULDList.Rows[i].FindControl("txtULDBuilderName")).Text;
                        DollyWt = ((TextBox)grdULDList.Rows[i].FindControl("txtDollyWt")).Text;
                        break;
                    }
                }

                //To check if ULD number is already present in grid
                for (int t = 0; t < grdULDList.Rows.Count; t++)
                {
                    if (((CheckBox)grdULDList.Rows[t].FindControl("rdULDList")).Checked == false)
                    {
                        if (ULDNumber.ToUpper() == ((Label)grdULDList.Rows[t].FindControl("lblOldULDNo")).Text.ToUpper())
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "ULD number already present";
                            return;
                        }
                    }
                }

                if (ULDNumber == "")
                {
                    lblStatus.Text = "Please enter ULD Number";
                    return;
                }

                if (ULDOrigin == "")
                {
                    lblStatus.Text = "Please enter ULD Origin";
                    return;
                }

                if (objBuildULD.CheckULDValidity(ULDNumber.ToUpper()) == false)
                {
                    lblStatus.Text = "ULD number format not valid";
                    return;
                }

                #region Validate ULD against ULD Master and Usage
                if (ULDNumber != string.Empty)
                {
                    string Result = objBuildULD.ValidateULDAdvanced(ULDNumber);
                    if (Result != "Success")
                    {
                        lblStatus.Text = Result;
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                }
                #endregion

                bool res = false;
                tabMenifestDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];

                //To save ULD with associated AWBs
                if (grdBulkAssignedAWB.Rows.Count > 0)
                {
                    for (int i = 0; i < grdBulkAssignedAWB.Rows.Count; i++)
                    {
                        if (((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("lblULDNo")).Text.Trim() == "")
                        {
                            continue;
                        }
                        else
                        {
                            string AWBno = "", AWBPrefix = "", AWBLocation = "", FLTno = "", Updatedby, POL = "";
                            int BuiltPcs = 0;
                            double BuiltWt = 0.0;
                            string Updatedon = "";
                            string AWBLoadingPriority = "", AWBBuilderName = "";

                            //FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                            if (txtFlightID.Text.Trim() != "")
                                FLTno = txtFlightCode.Text.Trim().ToUpper() + txtFlightID.Text.Trim();
                            else
                                FLTno = "";
                            Updatedby = Convert.ToString(Session["Username"]);

                            POL = Session["Station"].ToString().ToUpper();

                            Updatedon = Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss");

                            tabMenifestDetails.Rows[i]["Location"] = AWBLocation;
                            tabMenifestDetails.Rows[i]["AWBLoadingPriority"] = AWBLoadingPriority;
                            tabMenifestDetails.Rows[i]["AWBBuilderName"] = AWBBuilderName;

                            string FltDate;
                            if (TextBoxdate.Text.Trim() == "")
                                FltDate = "";
                            else
                                FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                            if (Session["Station"].ToString().ToUpper() == POL.ToUpper())
                            {
                                string strResult = string.Empty;
                                //appended 25apr -add awb to manifest completed so they dont appear in awb again

                                #region Check if ULD valid or not for save
                                string ValidULD = objBuildULD.checkValidULDForSave(ULDNumber.ToUpper(), FLTno.ToUpper(),
                                    FltDate, ULDOrigin.ToUpper(), ULDDest.ToUpper(), Session["Station"].ToString().ToUpper(),
                                    Convert.ToDecimal(ScaleWeight));

                                if (ValidULD.ToUpper() == "LOCATION INVALID")
                                {
                                    BtnList_Click(sender, e);
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "ULD not in current location";
                                    return;
                                }
                                else if (ValidULD.ToUpper() == "ULD NOT EMPTY")
                                {
                                    BtnList_Click(sender, e);
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "ULD getting used for another flight";
                                    return;
                                }
                                else if (ValidULD.ToUpper().Contains("SCALE WT"))
                                {
                                    //BtnList_Click(sender, e);
                                    lblULDStatus.ForeColor = Color.Red;
                                    lblULDStatus.Text = ValidULD;
                                    if (txtScaleWt != null)
                                        txtScaleWt.ForeColor = Color.Red;
                                    //return;
                                }
                                #endregion Check if ULD valid or not for save

                                #region Check ULD destination valid or not
                                dsValidDest = objBuildULD.CheckValidDestination(ULDNumber.ToUpper(), FLTno.ToUpper(), FltDate,
                                    ULDOrigin.ToUpper(), ULDDest.ToUpper(), ref strResult);
                                if (dsValidDest != null)
                                {
                                    if (dsValidDest.Tables != null)
                                    {
                                        if (dsValidDest.Tables.Count > 0)
                                        {
                                            if (dsValidDest.Tables[0].Rows[0][0].ToString().ToUpper() == "INVALID")
                                            {
                                                lblStatus.ForeColor = Color.Red;
                                                lblStatus.Text = "ULD destination is invalid";
                                                return;
                                            }
                                        }
                                    }
                                }
                                #endregion Check ULD destination valid or not

                                SaveULDNoinMaster(ULDNumber.ToUpper(), DollyWt);

                                ///dsResult = objBuildULD.AddULDAWBDetails(ULDNumber, AWBPrefix, AWBno, BuiltPcs, BuiltWt, FLTno, FltDate, "", "", ULDOrigin, ULDDest, Updatedon, Updatedby, AWBLocation, AWBLoadingPriority, ULDLocation, ULDLoadingPriority, ScaleWeight, ContainerType, AWBBuilderName, ULDBuilderName, "N", ref strResult, DollyWt);

                                //if (tabMenifestDetails.Rows[i]["FlightExists"].ToString() != "FN") //To save AWBs (with flight) into ULD
                                //    dsResult = objBuildULD.AddULDAWBDetails(ULDNumber, AWBPrefix, AWBno, BuiltPcs, BuiltWt, FLTno, FltDate, "", "", ULDOrigin, ULDDest, Updatedon, Updatedby, AWBLocation, AWBLoadingPriority, ULDLocation, ULDLoadingPriority, ScaleWeight, ContainerType, AWBBuilderName, ULDBuilderName, "N", ref strResult, DollyWt);
                                //else //To save bulk AWBs (without flight) into ULD
                                dsResult = objBuildULD.AddULDBulkAWBDetails(ULDNumber.ToUpper(), AWBPrefix, AWBno, BuiltPcs,
                                    BuiltWt, FLTno.ToUpper(), FltDate, "", "", ULDOrigin.ToUpper(), ULDDest.ToUpper(),
                                    Updatedon, Updatedby, AWBLocation, AWBLoadingPriority, ULDLocation, ULDLoadingPriority,
                                    ScaleWeight, ContainerType, AWBBuilderName, ULDBuilderName, "N", ref strResult, DollyWt);

                                #region Saving HAWB
                                try
                                {
                                    foreach (GridViewRow row in ((GridView)grdBulkAssignedAWB.Rows[i].FindControl("GVSubHAWB")).Rows)
                                    {
                                        string HAWBNumber = ((Label)row.FindControl("lblHAWBNo")).Text;
                                        if (HAWBNumber.Contains('-'))
                                        {
                                            string[] HAWBDetails = ((Label)row.FindControl("lblHAWBNo")).Text.Split('-');
                                            objBuildULD.AddULDBulkAWBDetails(ULDNumber.ToUpper(), HAWBDetails[0], HAWBDetails[1], int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text), double.Parse(((Label)row.FindControl("lblBuiltWt")).Text), FLTno, FltDate, "", "", ULDOrigin, ULDDest, Updatedon, Updatedby, AWBLocation, AWBLoadingPriority, ULDLocation, ULDLoadingPriority, ScaleWeight, ContainerType, AWBBuilderName, ULDBuilderName, "N", ref strResult, DollyWt);
                                        }
                                        else
                                        {
                                            objBuildULD.AddULDBulkAWBDetails(ULDNumber.ToUpper(), "", HAWBNumber, int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text), double.Parse(((Label)row.FindControl("lblBuiltWt")).Text), FLTno, FltDate, "", "", ULDOrigin, ULDDest, Updatedon, Updatedby, AWBLocation, AWBLoadingPriority, ULDLocation, ULDLoadingPriority, ScaleWeight, ContainerType, AWBBuilderName, ULDBuilderName, "N", ref strResult, DollyWt);
                                        }

                                    }
                                }
                                catch (Exception ex)
                                { }
                                #endregion

                                cls_BL.addMsgToOutBox("SCM", "Started ULD Build for " + ULDNumber.ToUpper(), "", "");
                                if (dsResult != null)
                                {
                                    if (dsResult.Tables != null)
                                    {
                                        if (dsResult.Tables.Count > 1)
                                        {
                                            if (dsResult.Tables[0].Rows[0][0].ToString() == "ULD saved successfully")
                                            {
                                                res = true;
                                                //Deleted textboxes
                                                //txtULDWeight.Text = dsResult.Tables[1].Rows[0][0].ToString();
                                            }
                                            else
                                            {
                                                res = false;
                                                lblStatus.ForeColor = Color.Red;
                                                lblStatus.Text = "ULD not saved";
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //To save ULD without associated AWBs
                else
                {
                    string FLTno = "", Updatedby, POL = "";
                    int BuiltPcs = 0;
                    double BuiltWt = 0.0;
                    string Updatedon = "";


                    for (int i = 0; i < grdULDList.Rows.Count; i++)
                    {
                        if (((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
                        {
                            ULDNumber = ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text.ToUpper();
                            hfULDNumber.Value = ULDNumber.ToUpper();
                            ULDLocation = ((TextBox)grdULDList.Rows[i].FindControl("txtULDLocation")).Text;
                            ULDLoadingPriority = ((TextBox)grdULDList.Rows[i].FindControl("txtULDLoadingPriority")).Text;

                            ULDOrigin = ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOL")).Text.ToUpper(); //Session["Station"].ToString()
                            ULDDest = ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOU")).Text.ToUpper();

                            ContainerType = ((DropDownList)grdULDList.Rows[i].FindControl("ddlContainerType")).SelectedItem.Text.ToString();
                            if (!double.TryParse(((TextBox)grdULDList.Rows[i].FindControl("txtScaleWeight")).Text, out ScaleWeight))
                                ScaleWeight = 0;
                            txtScaleWt = (TextBox)grdULDList.Rows[i].FindControl("txtScaleWeight");

                            ULDBuilderName = ((TextBox)grdULDList.Rows[i].FindControl("txtULDBuilderName")).Text;
                            DollyWt = ((TextBox)grdULDList.Rows[i].FindControl("txtDollyWt")).Text;
                            break;
                        }
                    }

                    //FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                    if (txtFlightID.Text.Trim() != "")
                        FLTno = txtFlightCode.Text.Trim().ToUpper() + txtFlightID.Text.Trim();
                    else
                        FLTno = "";
                    Updatedby = Convert.ToString(Session["Username"]);

                    POL = Session["Station"].ToString().ToUpper();

                    Updatedon = Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss");

                    string FltDate;
                    if (TextBoxdate.Text.Trim() == "")
                        FltDate = "";
                    else
                        FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                    if (Session["Station"].ToString().ToUpper() == POL.ToUpper())
                    {
                        string strResult = string.Empty;
                        //appended 25apr -add awb to manifest completed so they dont appear in awb again

                        #region Check if ULD valid or not for save
                        string ValidULD = objBuildULD.checkValidULDForSave(ULDNumber, FLTno, FltDate, ULDOrigin, ULDDest,
                            Session["Station"].ToString(), Convert.ToDecimal(ScaleWeight));
                        if (ValidULD == "LOCATION INVALID")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "ULD not in current location";
                            return;
                        }
                        else if (ValidULD == "ULD NOT EMPTY")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "ULD getting used for another flight";
                            return;
                        }
                        else if (ValidULD.ToUpper().Contains("SCALE WT"))
                        {
                            //BtnList_Click(sender, e);
                            lblULDStatus.ForeColor = Color.Red;
                            lblULDStatus.Text = ValidULD;
                            if (txtScaleWt != null)
                                txtScaleWt.ForeColor = Color.Red;
                            //return;
                        }
                        #endregion Check if ULD valid or not for save

                        #region Check ULD destination valid or not
                        dsValidDest = objBuildULD.CheckValidDestination(ULDNumber, FLTno, FltDate, ULDOrigin, ULDDest, ref strResult);
                        if (dsValidDest != null)
                        {
                            if (dsValidDest.Tables != null)
                            {
                                if (dsValidDest.Tables.Count > 0)
                                {
                                    if (dsValidDest.Tables[0].Rows[0][0].ToString() == "INVALID")
                                    {
                                        lblStatus.ForeColor = Color.Red;
                                        lblStatus.Text = "ULD destination is invalid";
                                        return;
                                    }
                                }
                            }
                        }
                        #endregion Check ULD destination valid or not

                        SaveULDNoinMaster(ULDNumber, DollyWt);
                        dsResult = objBuildULD.AddULDDetails(ULDNumber, BuiltPcs, BuiltWt, FLTno, FltDate, "", "", ULDOrigin, ULDDest, Updatedon, Updatedby, ULDLocation, ULDLoadingPriority, ScaleWeight, ContainerType, ULDBuilderName, "N", ref strResult, DollyWt);
                        cls_BL.addMsgToOutBox("SCM", "Started ULD Build for " + ULDNumber, "", "");
                        if (dsResult != null)
                        {
                            if (dsResult.Tables != null)
                            {
                                if (dsResult.Tables.Count > 1)
                                {
                                    if (dsResult.Tables[0].Rows[0][0].ToString() == "ULD saved successfully")
                                    {
                                        res = true;
                                        //Deleted textboxes
                                        //txtULDWeight.Text = dsResult.Tables[1].Rows[0][0].ToString();
                                    }
                                    else
                                    {
                                        res = false;
                                        lblStatus.ForeColor = Color.Red;
                                        lblStatus.Text = "ULD not saved";
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }

                if (res == true)
                {
                    string statusMsg = lblStatus.Text;
                    string ULDStatus = lblULDStatus.Text;
                    BtnList_Click(sender, e);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "ULD Saved Successfully";
                    if (statusMsg.ToUpper().Contains("ULD"))
                    {
                        lblStatus.Text += " [" + statusMsg + "]";
                        //lblStatus.ForeColor = Color.Blue;
                    }
                    if (ULDStatus != "")
                    {
                        lblULDStatus.Text = ULDStatus;
                        lblULDStatus.ForeColor = Color.Red;
                        if (txtScaleWt != null)
                        {
                            txtScaleWt.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        lblULDStatus.Text = "";
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "ULD not saved. Please try again.";
                return;
            }
            finally
            {
                if (dsResult != null)
                    dsResult.Dispose();
                if (tabMenifestDetails != null)
                    tabMenifestDetails.Dispose();
            }
        }

        private void SaveULDs()
        {
            DataSet dsValidDest = new DataSet("FltPlan_SaveULDs_dsValidDest");
            DataSet dsResult = new DataSet("FltPlan_SaveULDs_dsResult");
            try
            {

                string FLTno = "", Updatedby, POL = "", ULDNumber = "", ULDLocation = "", ULDLoadingPriority = "";
                string ULDOrigin = "", ULDDest = "", ContainerType = "", ULDBuilderName = "", NewULDNo = "";
                double ScaleWeight = 0;
                string DollyWt = "0";
                int BuiltPcs = 0;
                double BuiltWt = 0.0;
                string Updatedon = "";
                bool res = false;
                for (int i = 0; i < grdULDList.Rows.Count; i++)
                {
                    if (((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
                    {
                        ULDNumber = ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text.ToUpper();
                        NewULDNo = ((TextBox)grdULDList.Rows[i].FindControl("txtULDNo")).Text.ToUpper();

                        if (NewULDNo == "")
                        {
                            lblStatus.Text = "Please enter valid ULD Number";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        hfULDNumber.Value = ULDNumber.ToUpper();
                        ULDLocation = ((TextBox)grdULDList.Rows[i].FindControl("txtULDLocation")).Text;
                        ULDLoadingPriority = ((TextBox)grdULDList.Rows[i].FindControl("txtULDLoadingPriority")).Text;

                        ULDOrigin = ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOL")).Text.ToUpper(); //Session["Station"].ToString()
                        ULDDest = Session["FlightPlanning_Dest"].ToString();

                        ContainerType = ((DropDownList)grdULDList.Rows[i].FindControl("ddlContainerType")).SelectedItem.Text.ToString();

                        if (!double.TryParse(((TextBox)grdULDList.Rows[i].FindControl("txtScaleWeight")).Text, out ScaleWeight))
                            ScaleWeight = 0;

                        ULDBuilderName = ((TextBox)grdULDList.Rows[i].FindControl("txtULDBuilderName")).Text;
                        DollyWt = ((TextBox)grdULDList.Rows[i].FindControl("txtDollyWt")).Text;

                        //FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                        if (txtFlightID.Text.Trim() != "")
                            FLTno = txtFlightCode.Text.Trim().ToUpper() + txtFlightID.Text.Trim();
                        else
                            FLTno = "";
                        Updatedby = Convert.ToString(Session["Username"]);

                        POL = Session["Station"].ToString().ToUpper();

                        Updatedon = Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss");

                        string FltDate;
                        if (TextBoxdate.Text.Trim() == "")
                            FltDate = "";
                        else
                            FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                        #region Validate ULD against ULD Master and Usage
                        if (NewULDNo != string.Empty)
                        {
                            string Result = objBuildULD.ValidateULDAdvanced(NewULDNo);
                            if (Result != "Success")
                            {
                                lblStatus.Text = Result;
                                lblStatus.ForeColor = Color.Blue;
                                return;
                            }
                        }
                        #endregion

                        if (Session["Station"].ToString().ToUpper() == POL.ToUpper())
                        {
                            string strResult = string.Empty;
                            //appended 25apr -add awb to manifest completed so they dont appear in awb again

                            #region Check if ULD valid or not for save
                            string ValidULD = objBuildULD.checkValidULDForSave(NewULDNo, FLTno, FltDate, ULDOrigin, ULDDest,
                                Session["Station"].ToString(), Convert.ToDecimal(ScaleWeight));
                            if (ValidULD == "LOCATION INVALID")
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "ULD not in current location";
                                //return;
                            }
                            else if (ValidULD == "ULD NOT EMPTY")
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "ULD getting used for another flight";
                                //return;
                            }
                            else if (ValidULD.ToUpper().Contains("SCALE WT"))
                            {
                                //BtnList_Click(sender, e);
                                lblULDStatus.ForeColor = Color.Red;
                                lblULDStatus.Text = ValidULD;
                            }
                            #endregion Check if ULD valid or not for save

                            #region Check ULD destination valid or not
                            dsValidDest = objBuildULD.CheckValidDestination(NewULDNo, FLTno, FltDate, ULDOrigin, ULDDest,
                                ref strResult);
                            if (dsValidDest != null)
                            {
                                if (dsValidDest.Tables != null)
                                {
                                    if (dsValidDest.Tables.Count > 0)
                                    {
                                        if (dsValidDest.Tables[0].Rows[0][0].ToString() == "INVALID")
                                        {
                                            lblStatus.ForeColor = Color.Red;
                                            lblStatus.Text = "ULD destination is invalid";
                                            //return;
                                        }
                                    }
                                }
                            }
                            #endregion Check ULD destination valid or not

                            SaveULDNoinMaster(NewULDNo, DollyWt);

                            dsResult = objBuildULD.AddULDDetails(ULDNumber, BuiltPcs, BuiltWt, FLTno, FltDate, "", "", ULDOrigin,
                                ULDDest, Updatedon, Updatedby, ULDLocation, ULDLoadingPriority, ScaleWeight, ContainerType,
                                ULDBuilderName, "N", ref strResult, DollyWt, NewULDNo);
                            cls_BL.addMsgToOutBox("SCM", "Started ULD Build for " + ULDNumber, "", "");
                            if (dsResult != null)
                            {
                                if (dsResult.Tables != null)
                                {
                                    if (dsResult.Tables.Count > 1)
                                    {
                                        if (dsResult.Tables[0].Rows[0][0].ToString() == "ULD saved successfully")
                                        {
                                            res = true;
                                        }
                                        else
                                        {
                                            res = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (res == true)
                {
                    //Store ULD related error message in variable.
                    string statusMsg = lblStatus.Text;
                    BtnList_Click(null, null);
                    string ULDStatus = lblULDStatus.Text;
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "ULD Saved Successfully";
                    if (statusMsg.ToUpper().Contains("ULD"))
                    {
                        lblStatus.Text += " [" + statusMsg + "]";
                        //lblStatus.ForeColor = Color.Blue;
                    }
                    if (ULDStatus != "")
                    {
                        lblULDStatus.Text = ULDStatus;
                        lblULDStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblULDStatus.Text = "";
                    }
                    return;
                }
                else
                {
                    BtnList_Click(null, null);
                    string ULDStatus = lblULDStatus.Text;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "ULD could not be saved. Please try again.";
                    if (ULDStatus != "")
                    {
                        lblULDStatus.Text = ULDStatus;
                        lblULDStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblULDStatus.Text = "";
                    }
                    return;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dsResult != null)
                {
                    dsResult.Dispose();
                }
                if (dsValidDest != null)
                {
                    dsValidDest.Dispose();
                }
            }
        }


        #region Save ULD AWB
        private bool SaveULDAWB(int indexAssigned)
        {
            DataSet dsResult = new DataSet("FltPlan_SaveULDAWB_dsResult");
            DataSet dsValidDest = new DataSet("FltPlan_SaveULDAWB_dsValidDest");
            lblStatus.Text = "";
            lblULDStatus.Text = "";
            try
            {

                string ULDLocation = "", ULDLoadingPriority = "", ULDBuilderName = "";
                string ULDNumber = "", ContainerType = "";
                double ScaleWeight = 0.0;
                string ULDOrigin = "", ULDDest = "", DollyWt = "";
                TextBox txtScaleWt = null;
                string strResult = "";

                ULDNumber = ((TextBox)grdBulkAssignedAWB.Rows[indexAssigned].FindControl("lblULDNo")).Text.ToUpper();

                bool res = false;
                tabMenifestDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];

                string AWBno = "", AWBPrefix = "", AWBLocation = "", FLTno = "", Updatedby, POL = "";
                int BuiltPcs = 0;
                double BuiltWt = 0.0;
                string Updatedon = "";
                string AWBLoadingPriority = "", AWBBuilderName = "";

                //FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                if (txtFlightID.Text.Trim() != "")
                    FLTno = txtFlightCode.Text.Trim().ToUpper() + txtFlightID.Text.Trim();
                else
                    FLTno = "";
                Updatedby = Convert.ToString(Session["Username"]);


                POL = Session["Station"].ToString().ToUpper();
                ULDDest = Session["FlightPlanning_Dest"].ToString().ToUpper();

                AWBno = ((Label)grdBulkAssignedAWB.Rows[indexAssigned].FindControl("lblAWBNo")).Text.Trim();
                //AWBPrefix = AWBno.Substring(0, 3);
                AWBPrefix = AWBno.Substring(0, AWBno.IndexOf("-"));
                AWBno = AWBno.Substring(AWBno.Length - 8);
                BuiltPcs = Convert.ToInt32(((Label)grdBulkAssignedAWB.Rows[indexAssigned].FindControl("lblBuiltPcs")).Text.Trim());
                BuiltWt = Convert.ToDouble(((Label)grdBulkAssignedAWB.Rows[indexAssigned].FindControl("lblBuiltWt")).Text.Trim());
                AWBLoadingPriority = ((TextBox)grdBulkAssignedAWB.Rows[indexAssigned].FindControl("txtAWBLoadingPriority")).Text;
                AWBLocation = ((TextBox)grdBulkAssignedAWB.Rows[indexAssigned].FindControl("txtAWBLocation")).Text;
                Updatedon = Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss");
                AWBBuilderName = ((TextBox)grdBulkAssignedAWB.Rows[indexAssigned].FindControl("txtAWBBuilderName")).Text;

                tabMenifestDetails.Rows[indexAssigned]["Location"] = AWBLocation;
                tabMenifestDetails.Rows[indexAssigned]["AWBLoadingPriority"] = AWBLoadingPriority;
                tabMenifestDetails.Rows[indexAssigned]["AWBBuilderName"] = AWBBuilderName;

                string FltDate;
                if (TextBoxdate.Text.Trim() == "")
                    FltDate = "";
                else
                    FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");


                dsResult = objBuildULD.AddULDBulkAWBDetails(ULDNumber.ToUpper(), AWBPrefix, AWBno, BuiltPcs,
                    BuiltWt, FLTno.ToUpper(), FltDate, "", "", POL.ToUpper(), ULDDest.ToUpper(),
                    Updatedon, Updatedby, AWBLocation, AWBLoadingPriority, ULDLocation, ULDLoadingPriority,
                    ScaleWeight, ContainerType, AWBBuilderName, ULDBuilderName, "N", ref strResult, DollyWt);

                #region Saving HAWB
                try
                {
                    foreach (GridViewRow row in ((GridView)grdBulkAssignedAWB.Rows[indexAssigned].FindControl("GVSubHAWB")).Rows)
                    {
                        string HAWBNumber = ((Label)row.FindControl("lblHAWBNo")).Text;
                        if (HAWBNumber.Contains('-'))
                        {
                            string[] HAWBDetails = ((Label)row.FindControl("lblHAWBNo")).Text.Split('-');
                            objBuildULD.AddULDBulkAWBDetails(ULDNumber.ToUpper(), HAWBDetails[0], HAWBDetails[1], int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text), double.Parse(((Label)row.FindControl("lblBuiltWt")).Text), FLTno, FltDate, "", "", ULDOrigin, ULDDest, Updatedon, Updatedby, AWBLocation, AWBLoadingPriority, ULDLocation, ULDLoadingPriority, ScaleWeight, ContainerType, AWBBuilderName, ULDBuilderName, "N", ref strResult, DollyWt);
                        }
                        else
                        {
                            objBuildULD.AddULDBulkAWBDetails(ULDNumber.ToUpper(), "", HAWBNumber, int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text), double.Parse(((Label)row.FindControl("lblBuiltWt")).Text), FLTno, FltDate, "", "", ULDOrigin, ULDDest, Updatedon, Updatedby, AWBLocation, AWBLoadingPriority, ULDLocation, ULDLoadingPriority, ScaleWeight, ContainerType, AWBBuilderName, ULDBuilderName, "N", ref strResult, DollyWt);
                        }

                    }
                }
                catch (Exception ex)
                { }
                #endregion

                cls_BL.addMsgToOutBox("SCM", "Started ULD Build for " + ULDNumber.ToUpper(), "", "");
                if (dsResult != null)
                {
                    if (dsResult.Tables != null)
                    {
                        if (dsResult.Tables.Count > 1)
                        {
                            if (dsResult.Tables[0].Rows[0][0].ToString() == "ULD saved successfully")
                            {
                                res = true;
                                //Deleted textboxes
                                //txtULDWeight.Text = dsResult.Tables[1].Rows[0][0].ToString();
                            }
                            else
                            {
                                res = false;
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "ULD not saved";
                            }
                        }
                    }
                }
                return (res);

            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "ULD not saved. Please try again.";
            }
            finally
            {
                if (dsResult != null)
                    dsResult.Dispose();
                if (tabMenifestDetails != null)
                    tabMenifestDetails.Dispose();
            }
            return (false);
        }
        #endregion Save ULD AWB

        #region Save AWBs ULD
        private bool SaveAWBsByULD(string ULDNumber)
        {
            DataSet dsResult = new DataSet("FltPlan_SaveAWBsByULD_dsResult");
            DataSet dsValidDest = new DataSet("FltPlan_SaveAWBsByULD_dsValidDest");
            lblStatus.Text = "";
            lblULDStatus.Text = "";
            try
            {

                string ULDLocation = "", ULDLoadingPriority = "", ULDBuilderName = "", ContainerType = "";
                double ScaleWeight = 0.0;
                string ULDOrigin = "", ULDDest = "", DollyWt = "";
                TextBox txtScaleWt = null;

                hfULDNumber.Value = ULDNumber.ToUpper();
                ULDLocation = "";
                ULDLoadingPriority = "";

                ContainerType = "Standard";
                ScaleWeight = 0;
                //txtScaleWt.Text = "0";

                ULDOrigin = lblDepAirport.Text;
                ULDDest = Session["FlightPlanning_Dest"].ToString();

                ULDBuilderName = "";
                DollyWt = "0";

                if (ULDNumber == "")
                {
                    lblStatus.Text = "Please enter ULD Number";
                    return false;
                }

                if (ULDOrigin == "")
                {
                    lblStatus.Text = "Please enter ULD Origin";
                    return false;
                }

                if (objBuildULD.CheckULDValidity(ULDNumber.ToUpper()) == false)
                {
                    lblStatus.Text = "ULD number format not valid";
                    return (false);
                }

                #region Validate ULD against ULD Master and Usage
                if (ULDNumber != string.Empty)
                {
                    string Result = objBuildULD.ValidateULDAdvanced(ULDNumber);
                    if (Result != "Success")
                    {
                        lblStatus.Text = Result;
                        lblStatus.ForeColor = Color.Blue;
                        return (false);
                    }
                }
                #endregion

                string AWBLocation = "", FLTno = "", Updatedby, POL = "";
                int BuiltPcs = 0;
                double BuiltWt = 0.0;
                string Updatedon = "";
                string AWBLoadingPriority = "", AWBBuilderName = "";

                //FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                if (txtFlightID.Text.Trim() != "")
                    FLTno = txtFlightCode.Text.Trim().ToUpper() + txtFlightID.Text.Trim();
                else
                    FLTno = "";
                Updatedby = Convert.ToString(Session["Username"]);

                POL = Session["Station"].ToString().ToUpper();

                Updatedon = Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss");

                string FltDate;
                if (TextBoxdate.Text.Trim() == "")
                    FltDate = "";
                else
                    FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                string strResult = string.Empty;

                #region Check if ULD valid or not for save
                string ValidULD = objBuildULD.checkValidULDForSave(ULDNumber.ToUpper(), FLTno.ToUpper(),
                    FltDate, ULDOrigin.ToUpper(), ULDDest.ToUpper(), Session["Station"].ToString().ToUpper(),
                    Convert.ToDecimal(ScaleWeight));

                if (ValidULD.ToUpper() == "LOCATION INVALID")
                {
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "ULD not in current location";
                    return (false);
                }
                else if (ValidULD.ToUpper() == "ULD NOT EMPTY")
                {
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "ULD getting used for another flight";
                    return (false);
                }
                else if (ValidULD.ToUpper().Contains("SCALE WT"))
                {
                    //BtnList_Click(sender, e);
                    lblULDStatus.ForeColor = Color.Red;
                    lblULDStatus.Text = ValidULD;
                    if (txtScaleWt != null)
                        txtScaleWt.ForeColor = Color.Red;
                    //return;
                }
                #endregion Check if ULD valid or not for save

                #region Check ULD destination valid or not
                dsValidDest = objBuildULD.CheckValidDestination(ULDNumber.ToUpper(), FLTno.ToUpper(), FltDate,
                    ULDOrigin.ToUpper(), ULDDest.ToUpper(), ref strResult);
                if (dsValidDest != null)
                {
                    if (dsValidDest.Tables != null)
                    {
                        if (dsValidDest.Tables.Count > 0)
                        {
                            if (dsValidDest.Tables[0].Rows[0][0].ToString().ToUpper() == "INVALID")
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "ULD destination is invalid";
                                return (false);
                            }
                        }
                    }
                }
                #endregion Check ULD destination valid or not

                SaveULDNoinMaster(ULDNumber.ToUpper(), DollyWt);

                bool res = false;

                tabMenifestDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];

                //To save ULD with associated AWBs
                if (grdBulkAssignedAWB.Rows.Count > 0)
                {
                    for (int i = 0; i < grdBulkAssignedAWB.Rows.Count; i++)
                    {
                        if (((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("lblULDNo")).Text.Trim() == ULDNumber)
                        {
                            res = SaveULDAWB(i);
                        }
                    }
                }

                if (res == true)
                {
                    string ULDStatus = lblULDStatus.Text;
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "AWBs assigned successfully";
                    if (ULDStatus != "")
                    {
                        lblULDStatus.Text = ULDStatus;
                        lblULDStatus.ForeColor = Color.Red;
                        if (txtScaleWt != null)
                        {
                            txtScaleWt.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        lblULDStatus.Text = "";
                    }
                }
                else
                {
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "AWBs assign failed. Please try again.";
                }
                return (res);
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "ULD not saved. Please try again.";
                return (false);
            }
            finally
            {
                if (dsResult != null)
                    dsResult.Dispose();
                if (tabMenifestDetails != null)
                    tabMenifestDetails.Dispose();
            }
        }
        #endregion Save AWBs ULD

        private bool SaveULDNoinMaster(string UDLNumber, string DollyWt)
        {
            string strULDPrefix = UDLNumber.Trim().Substring(0, 3);
            string strULDSuffix = UDLNumber.Trim().Substring(UDLNumber.Trim().Length - 2, 2);
            string strULDSerial = UDLNumber.Trim().Replace(strULDPrefix, "").Replace(strULDSuffix, "");

            BALULDMaster blULD = new BALULDMaster();

            blULD.SelectRecords(UDLNumber, strULDSuffix, 0, "0", "0", 0, 0, 0, "0", "", "", "", false, "", strULDSerial, Convert.ToString(Session["Station"]), Convert.ToString(Session["UserName"]), Convert.ToDateTime(Session["IT"]),
                "", 0, "0", "0", Convert.ToDateTime(Session["IT"]), "", "", "", false, "", "N", DollyWt);

            blULD = null;

            return true;
        }

        protected void btnUnassign_Click(object sender, EventArgs e)
        {
            if (!GetFlightPlanningStatus())
                return;

            DataTable DTAWBDetails = new DataTable("FltPlan_btnUnassign_DTAWBDetails");
            DataTable dtULDDetails = new DataTable("FltPlan_btnUnassign_dtULDDetails");

            try
            {
                string strResult = string.Empty;
                lblStatus.Text = "";
                lblULDStatus.Text = "";
                int check = 0;
                string FLTno = "", FlightDate = "";
                if (txtFlightID.Text.Trim() != "")
                    FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                else
                    FLTno = "";

                if (TextBoxdate.Text.Trim() == "")
                    FlightDate = "";
                else
                    FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                //for (int i = 0; i < grdBulkAssignedAWB.Rows.Count; i++)
                //{
                //    if (((CheckBox)grdBulkAssignedAWB.Rows[i].FindControl("Check0")).Checked == true)
                //    {
                //        check = check + 1;
                //    }
                //}

                for (int i = 0; i < grdULDList.Rows.Count; i++)
                {
                    if (((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
                    {
                        if (grdULDList.Rows[i].Cells[11].Text == "Full")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please reopen ULD before unassign.";//check = check + 1;
                            return;
                        }
                    }
                }

                //if (check == 0)
                //{
                //lblStatus.ForeColor = Color.Red;
                //lblStatus.Text = "Please Select One AWB for UnAssign";
                //return;
                //}
                //}
                //if (check > 1)
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please Select only One AWB for UnAssign";
                //    return;
                //}


                bool blnFlag = false;
                DataSet dsresult = new DataSet("FltPlan_btnUnassign_dsResult");
                bool result = false;

                for (int i = 0; i < grdBulkAssignedAWB.Rows.Count; i++)
                {
                    if (((CheckBox)grdBulkAssignedAWB.Rows[i].FindControl("Check0")).Checked == true)
                    {
                        for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                        {
                            if (((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBNo")).Text == ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAWBNo")).Text
                                && ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("lblULDNo")).Text != "")
                            {

                                if (gdvULDLoadPlanAWB.Rows[j].Visible == true)
                                {
                                    //to update Built pieces label
                                    ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblBuiltPcs")).Text = (Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblBuiltPcs")).Text) - Convert.ToInt32(((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltPcs")).Text)).ToString();
                                    ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblBuiltWt")).Text = (Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblBuiltWt")).Text) - Convert.ToDouble(((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltWt")).Text)).ToString();

                                    //to update remaining pieces textbox
                                    ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text = (Convert.ToInt32(((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text) + Convert.ToInt32(((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltPcs")).Text)).ToString();
                                    ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text = (Convert.ToDouble(((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text) + Convert.ToDouble(((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltWt")).Text)).ToString();


                                    #region Code For Unassigning HAWB Deepak 23/05/2014
                                    try
                                    {
                                        DataTable dsawb1_HAWB = new DataTable("FltPlan_btnUnassign_dsawb1_HAWB");
                                        dsawb1_HAWB = Session["GHA_FlightPlanning_HAWBAddToULD"] != null ? ((DataTable)Session["GHA_FlightPlanning_HAWBAddToULD"]).Copy() : ((DataSet)Session["GHA_FlightPlanning_HAWBOriginal"]).Tables[0].Copy();

                                        foreach (GridViewRow row in ((GridView)grdBulkAssignedAWB.Rows[j].FindControl("GVSubHAWB")).Rows)
                                        {
                                            //if (((CheckBox)row.FindControl("Check2")).Checked)
                                            //{
                                            foreach (DataRow drow in dsawb1_HAWB.Rows)
                                            {
                                                if (((Label)row.FindControl("lblHAWBNo")).Text == drow["HAWBNo"].ToString() && ((Label)row.FindControl("lblAWBNo")).Text == drow["AWBNumber"].ToString())
                                                {
                                                    foreach (GridViewRow SRow in ((GridView)gdvULDLoadPlanAWB.Rows[j].FindControl("GVSubHAWB")).Rows)
                                                    {
                                                        if (((Label)SRow.FindControl("lblHAWBNo")).Text == ((Label)row.FindControl("lblHAWBNo")).Text)
                                                        {
                                                            drow["BuiltPcs"] = (int.Parse(((Label)SRow.FindControl("lblBuiltPcs")).Text) + int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text));
                                                            drow["BuiltWt"] = (double.Parse(((Label)SRow.FindControl("lblBuiltWt")).Text) + (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text)));
                                                            //((TextBox)row.FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)row.FindControl("lblPieces")).Text) - int.Parse(drow["BuiltPcs"].ToString())).ToString();
                                                            //((TextBox)row.FindControl("txtRemainingWt")).Text = (double.Parse(((Label)row.FindControl("lblWeight")).Text) - double.Parse(drow["BuiltWt"].ToString())).ToString();
                                                            drow["RemainingPcs"] = (int.Parse(((TextBox)SRow.FindControl("txtRemainingPcs")).Text) + int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text));
                                                            drow["RemainingWt"] = (double.Parse(((TextBox)SRow.FindControl("txtRemainingWt")).Text) + (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text)));
                                                            drow["IsChecked"] = "0";
                                                            drow["IsBulkOrULD"] = "";
                                                            drow["ULDNo"] = "";
                                                        }
                                                        //drow["BuiltPcs"] = (int.Parse(((Label)SRow.FindControl("lblBuiltPcs")).Text) - int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text));
                                                        //drow["BuiltWt"] = (double.Parse(((Label)SRow.FindControl("lblBuiltWt")).Text) - (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text)));
                                                        ////((TextBox)row.FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)row.FindControl("lblPieces")).Text) - int.Parse(drow["BuiltPcs"].ToString())).ToString();
                                                        ////((TextBox)row.FindControl("txtRemainingWt")).Text = (double.Parse(((Label)row.FindControl("lblWeight")).Text) - double.Parse(drow["BuiltWt"].ToString())).ToString();
                                                        //drow["RemainingPcs"] = (int.Parse(((TextBox)SRow.FindControl("txtRemainingPcs")).Text) + int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text));
                                                        //drow["RemainingWt"] = (double.Parse(((TextBox)SRow.FindControl("txtRemainingWt")).Text) + (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text)));
                                                        //drow["IsChecked"] = "0";
                                                        //drow["IsBulkOrULD"] = "";
                                                    }
                                                    //drow["BuiltPcs"] = (int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text) - int.Parse(((TextBox)row.FindControl("txtRemainingPcs")).Text != string.Empty ? ((TextBox)row.FindControl("txtRemainingPcs")).Text : "0"));
                                                    //drow["BuiltWt"] = (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text) - double.Parse(((TextBox)row.FindControl("txtRemainingWt")).Text));
                                                    ////((TextBox)row.FindControl("txtRemainingPcs")).Text = (int.Parse(((Label)row.FindControl("lblPieces")).Text) - int.Parse(drow["BuiltPcs"].ToString())).ToString();
                                                    ////((TextBox)row.FindControl("txtRemainingWt")).Text = (double.Parse(((Label)row.FindControl("lblWeight")).Text) - double.Parse(drow["BuiltWt"].ToString())).ToString();
                                                    //drow["RemainingPcs"] = (int.Parse(((Label)row.FindControl("lblPieces")).Text) + int.Parse(drow["BuiltPcs"].ToString())).ToString();
                                                    //drow["RemainingWt"] = (double.Parse(((Label)row.FindControl("lblWeight")).Text) + double.Parse(drow["BuiltWt"].ToString())).ToString();
                                                    //drow["IsChecked"] = "0";
                                                    //drow["IsBulkOrULD"] = "";
                                                }
                                            }
                                            //}
                                        }
                                        Session["GHA_FlightPlanning_HAWBAddToULD"] = dsawb1_HAWB;
                                    }
                                    catch (Exception ex)
                                    { }
                                    #endregion
                                    blnFlag = true;
                                }
                                else
                                {
                                    ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltPcs")).Text;
                                    ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltWt")).Text;

                                    gdvULDLoadPlanAWB.Rows[j].Visible = true;
                                    #region Code For Unassigning HAWB Deepak 23/05/2014
                                    try
                                    {
                                        DataTable dsawb1_HAWB = Session["GHA_FlightPlanning_HAWBAddToULD"] != null ? ((DataTable)Session["GHA_FlightPlanning_HAWBAddToULD"]).Copy() : ((DataSet)Session["GHA_FlightPlanning_HAWBOriginal"]).Tables[0].Copy();

                                        foreach (GridViewRow row in ((GridView)gdvULDLoadPlanAWB.Rows[j].FindControl("GVSubHAWB")).Rows)
                                        {
                                            if (((CheckBox)row.FindControl("Check2")).Checked)
                                            {
                                                foreach (DataRow drow in dsawb1_HAWB.Rows)
                                                {
                                                    if (((Label)row.FindControl("lblHAWBNo")).Text == drow["HAWBNo"].ToString() && ((Label)row.FindControl("lblAWBNo")).Text == drow["AWBNumber"].ToString())
                                                    {
                                                        drow["RemainingPcs"] = (int.Parse(((Label)row.FindControl("lblPieces")).Text) + int.Parse(drow["BuiltPcs"].ToString())).ToString();
                                                        drow["RemainingWt"] = (double.Parse(((Label)row.FindControl("lblWeight")).Text) + double.Parse(drow["BuiltWt"].ToString())).ToString();
                                                        drow["IsChecked"] = "0";
                                                        drow["IsBulkOrULD"] = "";
                                                        drow["ULDNo"] = "";
                                                    }
                                                }
                                            }
                                        }
                                        Session["GHA_FlightPlanning_HAWBAddToULD"] = dsawb1_HAWB;
                                    }
                                    catch (Exception ex)
                                    { }
                                    #endregion
                                    blnFlag = true;
                                }
                            }
                        }

                        //Commented by Vijay - 24/12/2013
                        if (blnFlag == false)
                        {
                            //Added by Poorna
                            DTAWBDetails = new DataTable("FltPlan_btnUnassign_DTAWBDetails");
                            DTAWBDetails = (DataTable)Session["FlightPlanning_AWBdata"];

                            if (DTAWBDetails == null)
                            {
                                DTAWBDetails = new DataTable("FltPlan_btnUnassign_DTAWBDetails");

                                DTAWBDetails.Columns.Add("lblAWBno");
                                DTAWBDetails.Columns.Add("lblPieces");
                                DTAWBDetails.Columns.Add("lblWeight");
                                DTAWBDetails.Columns.Add("lblBuiltPcs");
                                DTAWBDetails.Columns.Add("lblBuiltWt");
                                DTAWBDetails.Columns.Add("txtRemainingPcs");
                                DTAWBDetails.Columns.Add("txtRemainingWt");
                                DTAWBDetails.Columns.Add("lblOrigin");
                                DTAWBDetails.Columns.Add("lblDest");
                                DTAWBDetails.Columns.Add("lblLocation");
                                DTAWBDetails.Columns.Add("lblBookedPcs");
                                DTAWBDetails.Columns.Add("lblBookedWt");
                                DTAWBDetails.Columns.Add("lblFlightExists");
                                DTAWBDetails.Columns.Add("FltNo");
                                DTAWBDetails.Columns.Add("FltDate");
                                DTAWBDetails.Columns.Add("AWBNo");

                            }

                            if (((CheckBox)grdBulkAssignedAWB.Rows[i].FindControl("Check0")).Checked == true)
                            {
                                DataRow dr;

                                dr = DTAWBDetails.NewRow();
                                dr[0] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAWBNo")).Text;//gdvULDDetails.Rows[i].Cells[2].Text
                                dr[1] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAcceptedPcs")).Text;
                                dr[2] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAcceptedWt")).Text;
                                dr[3] = 0;
                                dr[4] = 0;
                                dr[5] = int.Parse(((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAcceptedPcs")).Text);
                                dr[6] = double.Parse(((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAcceptedWt")).Text);
                                dr[7] = "";
                                dr[8] = "";
                                //dr[9] = gdvULDDetails.Rows[i].Cells[7].Text;
                                //dr[9] = gdvULDDetails.Rows[i].Cells[9].Text;
                                dr[9] = ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("txtAWBLocation")).Text;
                                dr[10] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblTotalPcs")).Text;
                                dr[11] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblTotalWt")).Text;
                                dr[12] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblFlightExists")).Text;

                                DTAWBDetails.Rows.Add(dr);
                                #region Code For Unassigning HAWB Deepak 23/05/2014
                                try
                                {
                                    DataTable dsawb1_HAWB = new DataTable("FltPlan_btnUnassign_dsawb1_HAWB");
                                    dsawb1_HAWB = Session["GHA_FlightPlanning_HAWBAddToULD"] != null ? ((DataTable)Session["GHA_FlightPlanning_HAWBAddToULD"]).Copy() : ((DataSet)Session["GHA_FlightPlanning_HAWBOriginal"]).Tables[0].Copy();

                                    foreach (GridViewRow row in ((GridView)grdBulkAssignedAWB.Rows[i].FindControl("GVSubHAWB")).Rows)
                                    {
                                        //if (((CheckBox)row.FindControl("Check2")).Checked)
                                        //{
                                        foreach (DataRow drow in dsawb1_HAWB.Rows)
                                        {
                                            if (((Label)row.FindControl("lblHAWBNo")).Text == drow["HAWBNo"].ToString() && ((Label)row.FindControl("lblAWBNo")).Text == drow["AWBNumber"].ToString())
                                            {
                                                drow["BuiltPcs"] = (int.Parse(((Label)row.FindControl("lblBuiltPcs")).Text));
                                                drow["BuiltWt"] = (double.Parse(((Label)row.FindControl("lblBuiltWt")).Text));
                                                drow["IsChecked"] = "0";
                                                drow["IsBulkOrULD"] = "";
                                                drow["ULDNo"] = "";
                                            }
                                        }
                                        //}
                                    }
                                    Session["GHA_FlightPlanning_HAWBAddToULD"] = dsawb1_HAWB;
                                }
                                catch (Exception ex)
                                { }
                                #endregion
                            }
                        }


                        Session["FlightPlanning_AWBdata"] = DTAWBDetails;

                        //Commented by Vijay - 24/12/2013
                        if (DTAWBDetails != null)
                            LoadAWBDataGrid(DTAWBDetails);

                        dtULDDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];// (DataTable)Session["GDVULDDetails"];


                        /////////////////////////////////////////////
                        if (dtULDDetails != null)
                        {
                            if (dtULDDetails.Rows[i]["ULDno"].ToString() != "")
                            {
                                //bool result = objBuildULD.DeleteULDAWBDetails(dtULDDetails.Rows[i]["ULDno"].ToString(), dtULDDetails.Rows[i]["AWBno"].ToString(), int.Parse(dtULDDetails.Rows[i]["BuiltPcs"].ToString()), float.Parse(dtULDDetails.Rows[i]["BuiltWt"].ToString()), FLTno, FlightDate, dtULDDetails.Rows[i]["Origin"].ToString(), dtULDDetails.Rows[i]["Dest"].ToString(), "D", ref strResult);
                                //if (dtULDDetails.Rows[i]["FlightExists"].ToString() == "FN")
                                //    dsresult = objBuildULD.DeleteULDAWBDetails(dtULDDetails.Rows[i]["ULDno"].ToString(), dtULDDetails.Rows[i]["AWBno"].ToString(), int.Parse(dtULDDetails.Rows[i]["BuiltPcs"].ToString()), float.Parse(dtULDDetails.Rows[i]["BuiltWt"].ToString()), FLTno, FlightDate, dtULDDetails.Rows[i]["Origin"].ToString(), dtULDDetails.Rows[i]["Dest"].ToString(), "D", ref strResult);
                                //else
                                dsresult = objBuildULD.DeleteULDBulkAWBDetails(dtULDDetails.Rows[i]["ULDno"].ToString(), dtULDDetails.Rows[i]["AWBno"].ToString(), int.Parse(dtULDDetails.Rows[i]["BuiltPcs"].ToString()), float.Parse(dtULDDetails.Rows[i]["BuiltWt"].ToString()), FLTno, FlightDate, dtULDDetails.Rows[i]["Origin"].ToString(), dtULDDetails.Rows[i]["Dest"].ToString(), "D", ref strResult);

                                #region Saving HAWBDetails After Unassign
                                try
                                {
                                    if (Session["GHA_FlightPlanning_HAWBAddToULD"] != null)
                                    {
                                        //foreach (DataRow row in ((DataTable)Session["GHA_AssignULD_HAWBAddToULD"]).Rows)
                                        //{
                                        //if (row["AWBNumber"].ToString() == dtULDDetails.Rows[i]["AWBno"].ToString() && row["isBulkOrULD"].ToString() == string.Empty)
                                        //{
                                        foreach (GridViewRow grow in ((GridView)grdBulkAssignedAWB.Rows[i].FindControl("GVSubHAWB")).Rows)
                                        {
                                            if (dtULDDetails.Rows[i]["AWBno"].ToString() == ((Label)grow.FindControl("lblAWBNo")).Text)
                                            {
                                                objBuildULD.DeleteULDBulkAWBDetails(dtULDDetails.Rows[i]["ULDno"].ToString(), ((Label)grow.FindControl("lblHAWBNo")).Text, int.Parse(((Label)grow.FindControl("lblBuiltPcs")).Text), float.Parse(((Label)grow.FindControl("lblBuiltWt")).Text), FLTno, FlightDate, dtULDDetails.Rows[i]["Origin"].ToString(),
                                                    dtULDDetails.Rows[i]["Dest"].ToString(), "D", ref strResult);
                                            }
                                        }
                                        //}
                                        //}
                                    }
                                }
                                catch (Exception ex)
                                { }
                                #endregion
                            }
                            else
                            {
                                result = true;
                            }
                            dtULDDetails.Rows.RemoveAt(i);
                        }

                        if (dsresult != null)
                        {
                            if (dsresult.Tables != null)
                            {
                                if (dsresult.Tables.Count > 0)
                                {
                                    if (dsresult.Tables[0].Rows[0][0].ToString() == "Unassigned successfull")
                                    {
                                        result = true;
                                    }
                                }

                            }
                        }
                        if (result == true)
                        {
                            BtnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "AWB unassigned successfully";
                        }
                        else
                        {
                            BtnList_Click(null, null);
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "AWB not unassigned";
                            return;
                        }

                        /////////////////////////////////////////////
                        //Commented by Vijay - 23/01/2014
                        //if (dtULDDetails != null)
                        //{
                        //    //Vijay 29/11/2013 Deleting AWB from database if ULD number is generated for it
                        //    //if (gdvULDDetails.Rows[i].Cells[8].Text.Trim() != "")
                        //    if (dtULDDetails.Rows[i]["ULDno"].ToString() != "")
                        //    {
                        //        bool result;
                        //        //bool result = objBuildULD.DeleteULDAWBDetails(dtULDDetails.Rows[i]["ULDno"].ToString(), dtULDDetails.Rows[i]["AWBno"].ToString(), int.Parse(dtULDDetails.Rows[i]["BuiltPcs"].ToString()), float.Parse(dtULDDetails.Rows[i]["BuiltWt"].ToString()), FLTno, FlightDate, dtULDDetails.Rows[i]["Origin"].ToString(), dtULDDetails.Rows[i]["Dest"].ToString(), "D", ref strResult);
                        //        if(dtULDDetails.Rows[i]["FlightExists"].ToString() != "FN")
                        //            result = objBuildULD.DeleteULDAWBDetails(dtULDDetails.Rows[i]["ULDno"].ToString(), dtULDDetails.Rows[i]["AWBno"].ToString(), int.Parse(dtULDDetails.Rows[i]["BuiltPcs"].ToString()), float.Parse(dtULDDetails.Rows[i]["BuiltWt"].ToString()), FLTno, FlightDate, dtULDDetails.Rows[i]["Origin"].ToString(), dtULDDetails.Rows[i]["Dest"].ToString(), "D", ref strResult);
                        //        else
                        //            result = objBuildULD.DeleteULDBulkAWBDetails(dtULDDetails.Rows[i]["ULDno"].ToString(), dtULDDetails.Rows[i]["AWBno"].ToString(), int.Parse(dtULDDetails.Rows[i]["BuiltPcs"].ToString()), float.Parse(dtULDDetails.Rows[i]["BuiltWt"].ToString()), FLTno, FlightDate, dtULDDetails.Rows[i]["Origin"].ToString(), dtULDDetails.Rows[i]["Dest"].ToString(), "D", ref strResult);
                        //    }
                        //    dtULDDetails.Rows.RemoveAt(i);
                        //}

                        //gdvULDDetails.DataSource = dtULDDetails;
                        //gdvULDDetails.DataBind();

                        ////Vijay 02/12/2013
                        ////if (dtULDDetails.Rows.Count == 0)
                        ////{
                        ////    txtULDNumber.Text = "";
                        ////    txtULDNumber.ReadOnly = false;
                        ////    txtULDWeight.Text = "";
                        ////}

                        //Session["GDVULDDetails"] = dtULDDetails;
                        //Session["ManifestGridData"] = dtULDDetails;
                        ////  DataTable dtManifest=(DataTable)Session["ManifestGridData"] ;

                        //// AllButtonStatus(false);
                        //ShowULDAWBSummary();
                        //hdnManifestFlag.Value = "1";
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (DTAWBDetails != null)
                    DTAWBDetails.Dispose();
                if (dtULDDetails != null)
                    dtULDDetails.Dispose();
            }
        }

        #region Assign AWBs
        private void AssignAWBs(string ParentNumber, bool IsULD, string AWBLoadingPriority, string AWBBuilderName,
                        string ULDOrigin, string ULDDest, string AWBLocation, double ScaleWt, string dollyWt)
        {
            try
            {
                string result = "";
                bool blnRes = false;
                int pcs = 0;
                float wt = 0;
                string FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
                foreach (GridViewRow gvr in gdvULDLoadPlanAWB.Rows)
                {
                    if (((CheckBox)gvr.FindControl("Check2")).Checked)
                    {
                        if (!int.TryParse(((TextBox)gvr.FindControl("txtRemainingPcs")).Text, out pcs))
                        {
                            BtnList_Click(null, null);
                            lblStatus.Text = "Please enter valid Rem Pcs";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (!float.TryParse(((TextBox)gvr.FindControl("txtRemainingWt")).Text, out wt))
                        {
                            BtnList_Click(null, null);
                            lblStatus.Text = "Please enter valid Rem Wt";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        //Check if remaining wt exceeds available pcs.
                        if (pcs > int.Parse(((Label)gvr.FindControl("lblPieces")).Text))
                        {
                            BtnList_Click(null, null);
                            lblStatus.Text = "Pcs to assign can not be greater than available pieces";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        //Check if remaining wt exceeds available wt.
                        if (wt > float.Parse(((Label)gvr.FindControl("lblWeight")).Text))
                        {
                            BtnList_Click(null, null);
                            lblStatus.Text = "Weight to assign can not be greater than available weight";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        string AWBno = "", AWBPrefix = "";
                        AWBno = ((Label)gvr.FindControl("lblAWBNo")).Text;
                        AWBPrefix = AWBno.Substring(0, AWBno.IndexOf("-"));
                        AWBno = AWBno.Substring(AWBno.Length - 8);

                        if (!IsULD)
                        {
                            //Check if row already exists for the AWB
                            foreach (GridViewRow gvrAWBs in grdBulkAssignedAWB.Rows)
                            {
                                if (((Label)gvrAWBs.FindControl("lblAWBNo")).Text == AWBPrefix + "-" + AWBno &&
                                    ((TextBox)gvrAWBs.FindControl("lblCartNo")).Text.ToUpper() == ParentNumber.ToUpper())
                                {
                                    pcs = pcs + int.Parse(((Label)gvrAWBs.FindControl("lblBuiltPcs")).Text);
                                    wt = wt + float.Parse(((Label)gvrAWBs.FindControl("lblBuiltWt")).Text);
                                    break;
                                }
                            }

                            DataSet dsRes = new DataSet("FltPlan_AssignAWBs_dsRes");
                            dsRes = objBuildULD.AddBulkAssignedAWBDetailsFromCart(AWBPrefix, AWBno, pcs,
                            wt, txtFlightCode.Text.ToUpper() + txtFlightID.Text.ToUpper(), FlightDate,
                            Session["Station"].ToString(), ULDDest, Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss"),
                            Convert.ToString(Session["Username"]), AWBLocation, AWBLoadingPriority, AWBBuilderName,
                            ref result, ParentNumber, AWBLoadingPriority, AWBBuilderName, ScaleWt,
                            ((Label)gvr.FindControl("lblFlightNumber")).Text, ((Label)gvr.FindControl("lblFltDate")).Text);
                            blnRes = false;
                            if (dsRes != null)
                            {
                                if (dsRes.Tables != null)
                                {
                                    if (dsRes.Tables.Count > 0)
                                    {
                                        if (dsRes.Tables[0].Rows[0][0].ToString() == "Bulk AWBs saved successfully")
                                        {
                                            blnRes = true;
                                        }
                                    }
                                }
                            }
                            if (dsRes != null)
                            {
                                dsRes.Dispose();
                            }
                        }
                        else
                        {
                            //Check if row already exists for the AWB
                            foreach (GridViewRow gvrAWBs in grdBulkAssignedAWB.Rows)
                            {
                                if (((Label)gvrAWBs.FindControl("lblAWBNo")).Text == AWBPrefix + "-" + AWBno &&
                                    ((TextBox)gvrAWBs.FindControl("lblULDNo")).Text.ToUpper() == ParentNumber.ToUpper())
                                {
                                    pcs = pcs + int.Parse(((Label)gvrAWBs.FindControl("lblBuiltPcs")).Text);
                                    wt = wt + float.Parse(((Label)gvrAWBs.FindControl("lblBuiltWt")).Text);
                                    break;
                                }
                            }
                            DataSet dsRes = new DataSet("FltPlan_AssignAWBs_dsRes");
                            dsRes = objBuildULD.AddULDBulkAWBDetails(ParentNumber, AWBPrefix, AWBno, pcs, wt,
                            txtFlightCode.Text.ToUpper() + txtFlightID.Text.ToUpper(), FlightDate, "", "", Session["Station"].ToString(),
                            ULDDest.ToUpper(), Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss"),
                            Session["UserName"].ToString(), AWBLocation, AWBLoadingPriority, AWBLocation, AWBLoadingPriority,
                            ScaleWt, "", AWBBuilderName, AWBBuilderName, "N", ref result, dollyWt,
                            ((Label)gvr.FindControl("lblFlightNumber")).Text, ((Label)gvr.FindControl("lblFltDate")).Text);

                            blnRes = false;
                            if (dsRes != null)
                            {
                                if (dsRes.Tables != null)
                                {
                                    if (dsRes.Tables.Count > 0)
                                    {
                                        if (dsRes.Tables[0].Rows[0][0].ToString() == "ULD saved successfully")
                                        {
                                            blnRes = true;
                                        }
                                    }
                                }
                            }
                            if (dsRes != null)
                            {
                                dsRes.Dispose();
                            }
                        }
                    }
                }
                if (blnRes == true)
                {
                    string statusMsg = lblStatus.Text;
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "AWBs assigned successfully.";
                    if (statusMsg.ToUpper().Contains("ULD"))
                    {
                        lblStatus.Text += " [" + statusMsg + "]";
                        //lblStatus.ForeColor = Color.Green;
                    }
                }
                else
                {
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "All AWBs could not be assigned. Please try again.";
                    return;
                }

            }
            catch (Exception)
            {
            }
        }
        #endregion Unassign Cargo

        #region Unassign Cargo
        private void UnassignAWBs()
        {
            try
            {
                string result = "";
                bool blnRes = false;
                bool isSelected = false;
                int pcs = 0;
                float wt = 0;
                string FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
                foreach (GridViewRow gvr in grdBulkAssignedAWB.Rows)
                {
                    if (((CheckBox)gvr.FindControl("Check0")).Checked)
                    {
                        isSelected = true;
                        if (!int.TryParse(((Label)gvr.FindControl("lblBuiltPcs")).Text, out pcs))
                        {
                            BtnList_Click(null, null);
                            lblStatus.Text = "Please enter valid Rem Pcs";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        if (!float.TryParse(((Label)gvr.FindControl("lblBuiltWt")).Text, out wt))
                        {
                            BtnList_Click(null, null);
                            lblStatus.Text = "Please enter valid Rem Wt";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        if (((TextBox)gvr.FindControl("lblULDNo")).Text == "")
                        {
                            DataSet dsRes = new DataSet("FltPlan_AssignAWBs_dsRes");
                            dsRes = objBuildULD.DeleteBulkAssignedAWBDetailsFromCart(((Label)gvr.FindControl("lblAWBNo")).Text,
                                pcs, wt, txtFlightCode.Text + txtFlightID.Text, FlightDate, Session["Station"].ToString(),
                                Session["FlightPlanning_Dest"].ToString(), "D", ((Label)gvr.FindControl("lblOldCartNo")).Text, ref result);
                            blnRes = false;
                            if (dsRes != null)
                            {
                                if (dsRes.Tables != null)
                                {
                                    if (dsRes.Tables.Count > 0)
                                    {
                                        if (dsRes.Tables[0].Rows[0][0].ToString() == "Bulk unassigned")
                                        {
                                            blnRes = true;
                                        }
                                    }
                                }
                            }
                            if (dsRes != null)
                            {
                                dsRes.Dispose();
                            }
                        }
                        else
                        {
                            DataSet dsRes = new DataSet("FltPlan_AssignAWBs_dsRes");
                            dsRes = objBuildULD.DeleteULDBulkAWBDetails(((Label)gvr.FindControl("lblOldULDNo")).Text,
                                ((Label)gvr.FindControl("lblAWBNo")).Text, pcs, wt, txtFlightCode.Text + txtFlightID.Text,
                                FlightDate, Session["Station"].ToString(), Session["FlightPlanning_Dest"].ToString(), "D", ref result);
                            blnRes = false;
                            if (dsRes != null)
                            {
                                if (dsRes.Tables != null)
                                {
                                    if (dsRes.Tables.Count > 0)
                                    {
                                        if (dsRes.Tables[0].Rows[0][0].ToString() == "Unassigned successfull")
                                        {
                                            blnRes = true;
                                        }
                                    }
                                }
                            }
                            if (dsRes != null)
                            {
                                dsRes.Dispose();
                            }
                        }
                    }
                }
                if (blnRes == true)
                {
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "AWBs unassigned successfully.";
                }
                else
                {
                    if (!isSelected)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please select AWB(s) to be unassigned.";
                        return;
                    }
                    else
                    {
                        BtnList_Click(null, null);
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "All AWBs could not be unassigned. Please try again.";
                        return;
                    }
                }

            }
            catch (Exception)
            {
            }
        }
        #endregion Unassign Cargo

        #region Save Assigned AWB
        private void SaveAssignedAWB()
        {
            try
            {
                string result = "";
                bool rowSelected = false;
                bool blnRes = false;
                int pcs = 0;
                float wt = 0;
                string FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
                int rowIndex = -1;
                foreach (GridViewRow gvr in grdBulkAssignedAWB.Rows)
                {
                    rowIndex++;
                    if (((CheckBox)gvr.FindControl("Check0")).Checked)
                    {
                        rowSelected = true;
                        if (!int.TryParse(((Label)gvr.FindControl("lblBuiltPcs")).Text, out pcs))
                        {
                            pcs = 0;
                        }
                        if (!float.TryParse(((Label)gvr.FindControl("lblBuiltWt")).Text, out wt))
                        {
                            wt = 0;
                        }

                        string AWBno = "", AWBPrefix = "";
                        AWBno = ((Label)gvr.FindControl("lblAWBNo")).Text;
                        AWBPrefix = AWBno.Substring(0, AWBno.IndexOf("-"));
                        AWBno = AWBno.Substring(AWBno.Length - 8);
                        string AWBLocation, AWBLoadingPriority, AWBBuilderName, ParentNumber, ULDDest;
                        AWBLocation = ((TextBox)gvr.FindControl("txtAWBLocation")).Text;
                        AWBLoadingPriority = ((TextBox)gvr.FindControl("txtAWBLoadingPriority")).Text;
                        AWBBuilderName = ((TextBox)gvr.FindControl("txtAWBBuilderName")).Text;
                        ULDDest = Session["FlightPlanning_Dest"].ToString();
                        double ScaleWt = 0;
                        string DollyWt = "0";
                        bool isULD = false;
                        if (((TextBox)gvr.FindControl("lblULDNo")).Text.Trim() == "")
                        {
                            isULD = false;
                            ParentNumber = ((TextBox)gvr.FindControl("lblCartNo")).Text.ToUpper();
                            if (ParentNumber != ((Label)gvr.FindControl("lblOldCartNo")).Text.ToUpper())
                            {

                                #region Check if Cart is associated with flight which is not departed
                                string ValidCart = objBuildULD.checkCartValidForSave(ParentNumber,
                                    txtFlightCode.Text.ToUpper() + txtFlightID.Text.ToUpper(), TextBoxdate.Text,
                                    Session["Station"].ToString());
                                if (ValidCart == "INVALID")
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Invalid Cart";
                                    ((TextBox)gvr.FindControl("lblCartNo")).Focus();
                                    return;
                                }
                                else if (ValidCart == "NOT DEPARTED")
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Cart associated with non departed flight";
                                    ((TextBox)gvr.FindControl("lblCartNo")).Focus();
                                    return;
                                }
                                else if (ValidCart == "DIFFERENT FLIGHT")
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Cart associated with another flight";
                                    ((TextBox)gvr.FindControl("lblCartNo")).Focus();
                                    return;
                                }
                                #endregion Check if Cart is associated with flight which is not departed

                                DeleteAssignedAWB(rowIndex);
                            }
                        }
                        else
                        {
                            isULD = true;
                            ParentNumber = ((TextBox)gvr.FindControl("lblULDNo")).Text.ToUpper();
                            if (ParentNumber != ((Label)gvr.FindControl("lblOldULDNo")).Text.ToUpper())
                            {
                                //Validate current ULD.
                                if (objBuildULD.CheckULDValidity(ParentNumber.ToUpper()) == false)
                                {
                                    lblStatus.Text = "ULD number format not valid";
                                    lblStatus.ForeColor = Color.Red;
                                    ((TextBox)gvr.FindControl("lblULDNo")).Focus();
                                    return;
                                }
                                #region Check if ULD valid or not for save
                                string ValidULD = objBuildULD.checkValidULDForSave(ParentNumber.ToUpper(),
                                    txtFlightCode.Text.ToUpper() + txtFlightID.Text.ToUpper(), FlightDate,
                                    Session["Station"].ToString().ToUpper(), ULDDest.ToUpper(),
                                    Session["Station"].ToString().ToUpper(), Convert.ToDecimal(0));

                                if (ValidULD.ToUpper() == "LOCATION INVALID")
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "ULD not in current location";
                                    ((TextBox)gvr.FindControl("lblULDNo")).Focus();
                                    return;
                                }
                                else if (ValidULD.ToUpper() == "ULD NOT EMPTY")
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "ULD getting used for another flight";
                                    ((TextBox)gvr.FindControl("lblULDNo")).Focus();
                                    return;
                                }
                                #endregion Check if ULD valid or not for save

                                #region Check ULD destination valid or not
                                DataSet dsValidDest = new DataSet("FltPlan_SaveAssignAWBs_dsValidDest");
                                dsValidDest = objBuildULD.CheckValidDestination(ParentNumber.ToUpper(),
                                txtFlightCode.Text.ToUpper() + txtFlightID.Text.ToUpper(), FlightDate,
                                Session["Station"].ToString().ToUpper(), ULDDest.ToUpper(), ref result);
                                if (dsValidDest != null)
                                {
                                    if (dsValidDest.Tables != null)
                                    {
                                        if (dsValidDest.Tables.Count > 0)
                                        {
                                            if (dsValidDest.Tables[0].Rows[0][0].ToString().ToUpper() == "INVALID")
                                            {
                                                lblStatus.ForeColor = Color.Red;
                                                lblStatus.Text = "ULD destination is invalid";
                                                return;
                                            }
                                        }
                                    }
                                }
                                #endregion Check ULD destination valid or not

                                DeleteAssignedAWB(rowIndex);
                            }
                        }

                        if (!isULD)
                        {
                            DataSet dsRes = new DataSet("FltPlan_SaveAssignAWBs_dsRes");
                            dsRes = objBuildULD.AddBulkAssignedAWBDetailsFromCart(AWBPrefix, AWBno, pcs,
                            wt, txtFlightCode.Text.ToUpper() + txtFlightID.Text.ToUpper(), FlightDate,
                            Session["Station"].ToString(), ULDDest, Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss"),
                            Convert.ToString(Session["Username"]), AWBLocation, AWBLoadingPriority, AWBBuilderName,
                            ref result, ParentNumber, AWBLoadingPriority, AWBBuilderName, ScaleWt, "", "");
                            blnRes = false;
                            if (dsRes != null)
                            {
                                if (dsRes.Tables != null)
                                {
                                    if (dsRes.Tables.Count > 0)
                                    {
                                        if (dsRes.Tables[0].Rows[0][0].ToString() == "Bulk AWBs saved successfully")
                                        {
                                            blnRes = true;
                                        }
                                    }
                                }
                            }
                            if (dsRes != null)
                            {
                                dsRes.Dispose();
                            }
                        }
                        else
                        {
                            DataSet dsRes = new DataSet("FltPlan_SaveAssignAWBs_dsRes");
                            dsRes = objBuildULD.AddULDBulkAWBDetails(ParentNumber, AWBPrefix, AWBno, pcs, wt,
                            txtFlightCode.Text.ToUpper() + txtFlightID.Text.ToUpper(), FlightDate, "", "", Session["Station"].ToString(),
                            ULDDest.ToUpper(), Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss"),
                            Session["UserName"].ToString(), AWBLocation, AWBLoadingPriority, AWBLocation, AWBLoadingPriority,
                            ScaleWt, "", AWBBuilderName, AWBBuilderName, "N", ref result, DollyWt, "", "");

                            blnRes = false;
                            if (dsRes != null)
                            {
                                if (dsRes.Tables != null)
                                {
                                    if (dsRes.Tables.Count > 0)
                                    {
                                        if (dsRes.Tables[0].Rows[0][0].ToString() == "ULD saved successfully")
                                        {
                                            blnRes = true;
                                        }
                                    }
                                }
                            }
                            if (dsRes != null)
                            {
                                dsRes.Dispose();
                            }
                        }
                    }
                }
                if (blnRes == true)
                {
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "AWBs saved successfully.";
                }
                else
                {
                    if (!rowSelected)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please select AWBs to save";
                        return;
                    }
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "All AWBs could not be saved. Please try again.";
                    return;
                }

            }
            catch (Exception)
            {
            }
        }
        #endregion Unassign Cargo

        #region Delete Assigned AWB
        private bool DeleteAssignedAWB(int intIndex)
        {
            try
            {
                string result = "";
                bool blnRes = false;
                int pcs = 0;
                float wt = 0;
                string FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                GridViewRow gvr = grdBulkAssignedAWB.Rows[intIndex];

                if (!int.TryParse(((Label)gvr.FindControl("lblBuiltPcs")).Text, out pcs))
                {
                    pcs = 0;
                }
                if (!float.TryParse(((Label)gvr.FindControl("lblBuiltWt")).Text, out wt))
                {
                    wt = 0;
                }

                if (((TextBox)gvr.FindControl("lblULDNo")).Text == "")
                {
                    DataSet dsRes = new DataSet("FltPlan_DeleteAssignAWBs_dsRes");
                    dsRes = objBuildULD.DeleteBulkAssignedAWBDetailsFromCart(((Label)gvr.FindControl("lblAWBNo")).Text,
                        pcs, wt, txtFlightCode.Text + txtFlightID.Text, FlightDate, Session["Station"].ToString(),
                        Session["FlightPlanning_Dest"].ToString(), "D", ((Label)gvr.FindControl("lblOldCartNo")).Text, ref result);
                    blnRes = false;
                    if (dsRes != null)
                    {
                        if (dsRes.Tables != null)
                        {
                            if (dsRes.Tables.Count > 0)
                            {
                                if (dsRes.Tables[0].Rows[0][0].ToString() == "Bulk unassigned")
                                {
                                    blnRes = true;
                                }
                            }
                        }
                    }
                    if (dsRes != null)
                    {
                        dsRes.Dispose();
                    }
                }
                else
                {
                    DataSet dsRes = new DataSet("FltPlan_DeleteAssignAWBs_dsRes");
                    dsRes = objBuildULD.DeleteULDBulkAWBDetails(((Label)gvr.FindControl("lblOldULDNo")).Text,
                        ((Label)gvr.FindControl("lblAWBNo")).Text, pcs, wt, txtFlightCode.Text + txtFlightID.Text,
                        FlightDate, Session["Station"].ToString(), Session["FlightPlanning_Dest"].ToString(), "D", ref result);
                    blnRes = false;
                    if (dsRes != null)
                    {
                        if (dsRes.Tables != null)
                        {
                            if (dsRes.Tables.Count > 0)
                            {
                                if (dsRes.Tables[0].Rows[0][0].ToString() == "Unassigned successfull")
                                {
                                    blnRes = true;
                                }
                            }
                        }
                    }
                    if (dsRes != null)
                    {
                        dsRes.Dispose();
                    }
                }
                return (blnRes);
            }
            catch (Exception)
            {
            }
            return (false);
        }
        #endregion Unassign Cargo

        #region Delete Cart
        private bool DeleteCart(int intIndex)
        {
            try
            {
                string result = "";
                bool blnRes = false;
                int pcs = 0;
                float wt = 0;

                string FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                GridViewRow gvr = grdBulkAssignedAWB.Rows[intIndex];
                if (!int.TryParse(((Label)gvr.FindControl("lblBuiltPcs")).Text, out pcs))
                {
                    pcs = 0;
                }
                if (!float.TryParse(((Label)gvr.FindControl("lblBuiltWt")).Text, out wt))
                {
                    wt = 0;
                }

                if (((TextBox)gvr.FindControl("lblULDNo")).Text == "")
                {
                    DataSet dsRes = new DataSet("FltPlan_DeleteCart_dsRes");
                    dsRes = objBuildULD.DeleteBulkAssignedAWBDetailsAndCart(((Label)gvr.FindControl("lblAWBNo")).Text,
                        pcs, wt, txtFlightCode.Text + txtFlightID.Text, FlightDate, Session["Station"].ToString(),
                        Session["FlightPlanning_Dest"].ToString(), "D", ((Label)gvr.FindControl("lblOldCartNo")).Text, ref result);
                    blnRes = false;
                    if (dsRes != null)
                    {
                        if (dsRes.Tables != null)
                        {
                            if (dsRes.Tables.Count > 0)
                            {
                                if (dsRes.Tables[0].Rows[0][0].ToString() == "Bulk unassigned and Cart Deleted")
                                {
                                    blnRes = true;
                                }
                            }
                        }
                    }
                    if (dsRes != null)
                    {
                        dsRes.Dispose();
                    }
                }
                else
                {
                    DataSet dsRes = new DataSet("FltPlan_DeleteCart_dsRes");
                    dsRes = objBuildULD.DeleteULDAndAWBDetails(((Label)gvr.FindControl("lblOldULDNo")).Text,
                        ((Label)gvr.FindControl("lblAWBNo")).Text, pcs, wt, txtFlightCode.Text + txtFlightID.Text,
                        FlightDate, Session["Station"].ToString(), Session["FlightPlanning_Dest"].ToString(), "D", ref result);
                    blnRes = false;
                    if (dsRes != null)
                    {
                        if (dsRes.Tables != null)
                        {
                            if (dsRes.Tables.Count > 0)
                            {
                                if (dsRes.Tables[0].Rows[0][0].ToString().ToUpper() == "ULD DELETED SUCCESSFULLY")
                                {
                                    blnRes = true;
                                }
                            }
                        }
                    }
                    if (dsRes != null)
                    {
                        dsRes.Dispose();
                    }
                }
                return (blnRes);
            }
            catch (Exception)
            {
            }
            return (false);
        }
        #endregion Unassign Cargo

        #region Delete Cart By Number
        private bool DeleteCart(string ParentNumber, bool IsULD)
        {
            try
            {
                string result = "";
                bool blnRes = false;
                int pcs = 0;
                float wt = 0;
                string FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");


                if (IsULD == false)
                {
                    DataSet dsRes = new DataSet("FltPlan_DeleteCart1_dsRes");
                    dsRes =  objBuildULD.DeleteBulkAssignedAWBDetailsAndCart("", pcs, wt,
                        txtFlightCode.Text + txtFlightID.Text, FlightDate, Session["Station"].ToString(),
                        Session["FlightPlanning_Dest"].ToString(), "D", ParentNumber, ref result);
                    blnRes = false;
                    if (dsRes != null)
                    {
                        if (dsRes.Tables != null)
                        {
                            if (dsRes.Tables.Count > 0)
                            {
                                if (dsRes.Tables[0].Rows[0][0].ToString() == "Bulk unassigned and Cart Deleted")
                                {
                                    blnRes = true;
                                }
                            }
                        }
                    }
                    if (dsRes != null)
                    {
                        dsRes.Dispose();
                    }
                }
                else
                {
                    DataSet dsRes = new DataSet("FltPlan_DeleteCart1_dsRes");
                    dsRes = objBuildULD.DeleteULDAndAWBDetails(ParentNumber, "", pcs, wt,
                        txtFlightCode.Text + txtFlightID.Text, FlightDate, Session["Station"].ToString(),
                        Session["FlightPlanning_Dest"].ToString(), "D", ref result);
                    blnRes = false;
                    if (dsRes != null)
                    {
                        if (dsRes.Tables != null)
                        {
                            if (dsRes.Tables.Count > 0)
                            {
                                if (dsRes.Tables[0].Rows[0][0].ToString().ToUpper() == "ULD DELETED SUCCESSFULLY")
                                {
                                    blnRes = true;
                                }
                            }
                        }
                    }
                    if (dsRes != null)
                    {
                        dsRes.Dispose();
                    }
                }
                return (blnRes);
            }
            catch (Exception)
            {
            }
            return (false);
        }
        #endregion Unassign Cargo

        #region btnFinalizeULD_Click
        protected void btnFinalizeULD_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GetFlightPlanningStatus())
                    return;
                string ULDNumber = "", FLTno = "", ULDOrigin = "", ULDDest = "", Updatedon = "", Updatedby = "";
                bool dsResult = false;
                string isFinalized = "F";
                string strResult = "";
                if (txtFlightID.Text.Trim() != "")
                    FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                else
                    FLTno = "";
                string FltDate;
                if (TextBoxdate.Text.Trim() == "")
                    FltDate = "";
                else
                    FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                for (int i = 0; i < grdULDList.Rows.Count; i++)
                {
                    if (((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
                    {
                        ULDNumber = ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text;
                        ULDOrigin = ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOL")).Text;
                        ULDDest = ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOU")).Text;

                        //ULDOrigin = tabMenifestDetails.Rows[0]["Origin"].ToString();
                        //ULDDest = tabMenifestDetails.Rows[0]["Dest"].ToString();
                        Updatedon = Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss");
                        Updatedby = Convert.ToString(Session["Username"]);

                        dsResult = objBuildULD.SetFinalizeULD(ULDNumber, FLTno, FltDate, ULDOrigin, ULDDest, Updatedon, Updatedby, isFinalized, ref strResult);
                        //BindAWBLoadPlanULD(0);
                    }
                }
                if (dsResult)
                {
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "ULD finalized successfully";
                }
                else
                {
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "ULD could not be finalized. Please try again.";
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "ULD not Finalized. Please try again.";
                return;
            }
        }
        #endregion btnFinalizeULD_Click

        #region btnReOpenULD_Click
        protected void btnReOpenULD_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GetFlightPlanningStatus())
                    return;

                string ULDNumber = "", FLTno = "", ULDOrigin = "", ULDDest = "", Updatedon = "", Updatedby = "";
                bool dsResult = false;
                string isFinalized = "R";
                string strResult = "";
                if (txtFlightID.Text.Trim() != "")
                    FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                else
                    FLTno = "";
                string FltDate;
                if (TextBoxdate.Text.Trim() == "")
                    FltDate = "";
                else
                    FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                for (int i = 0; i < grdULDList.Rows.Count; i++)
                {
                    if (((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
                    {
                        ULDNumber = ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text;
                        ULDOrigin = ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOL")).Text;
                        ULDDest = ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOU")).Text;

                        //ULDOrigin = tabMenifestDetails.Rows[0]["Origin"].ToString();
                        //ULDDest = tabMenifestDetails.Rows[0]["Dest"].ToString();
                        Updatedon = Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss");
                        Updatedby = Convert.ToString(Session["Username"]);

                        dsResult = objBuildULD.SetFinalizeULD(ULDNumber, FLTno, FltDate, ULDOrigin, ULDDest, Updatedon, Updatedby, isFinalized, ref strResult);

                    }
                }
                if (dsResult)
                {
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "ULD(s) reopened successfully";
                }
                else
                {
                    BtnList_Click(null, null);
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "ULD(s) could not be reopened. Please try again.";
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "ULD(s) could not be reopened. Please try again.";
                return;
            }
        }
        #endregion btnReOpenULD_Click

        private void readCargoInfo(ref paramCargoInfo objCargoParam)
        {
            try
            {
                //objCargoParam.AirlineIdentifier = txtFlightCode.Text.Trim();
                //objCargoParam.FlightNumber = txtFlightID.Text.Trim();
                objCargoParam.Departure = lblDepAirport.Text.Trim();
                //objCargoParam.Arrival = "";
                getCargoFromAWB(grdBulkAssignedAWB, ref objCargoParam.objCargoList);

                getCargoFromULD(grdULDList, ref objCargoParam.objCargoList);
            }
            catch (Exception objEx)
            {
            }
        }
        int uldTareWt = 0;
        private void getCargoFromULD(GridView grdULDList, ref ArrayList objCargoList)
        {
            try
            {
                for (int i = 0; i < grdULDList.Rows.Count; i++)
                {
                    try
                    {
                        string strUldNo = ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text.Trim();

                        ReadCargoFromDB(ref objCargoList, "", "", strUldNo, txtFlightCode.Text.Trim() + txtFlightID.Text.Trim());

                    }
                    catch (Exception objEx)
                    {
                    }
                }
            }
            catch (Exception objex)
            {
            }
        }

        private void getCargoFromAWB(GridView grdBulkAssignedAWB, ref ArrayList objCargoList)
        {
            try
            {
                for (int i = 0; i < grdCartList.Rows.Count; i++)
                {
                    try
                    {
                        string strCartNo = ((Label)grdCartList.Rows[i].FindControl("lblOldCartNo")).Text.Trim();
                        DataSet ds = new DataSet("FltPlan_getCargoFromAWB_ds");
                        ds = objBuildULD.GetBulkAssignedAWBDetailsFromCart(lblDepAirport.Text, txtFlightCode.Text.Trim() + txtFlightID.Text.Trim(), TextBoxdate.Text.Trim(), "", "", strCartNo);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            try
                            {
                                String strAWBNo = dr["AWBNo"].ToString().Trim();
                                if (!String.IsNullOrEmpty(strAWBNo) && strAWBNo.Contains('-'))
                                {
                                    ReadCargoFromDB(ref objCargoList, strAWBNo.Substring(0, 3), strAWBNo.Substring(4), "", "");
                                }
                            }
                            catch (Exception objEx)
                            {
                            }
                        }

                    }
                    catch (Exception objEx)
                    {
                    }
                }
            }
            catch (Exception objEx)
            {
            }
        }

        private void ReadCargoFromDB(ref ArrayList objCargoList, string awbPrefix, string awbNo, String ULDNo, String FlightNo)
        {
            try
            {
                int LooseCount = 0, ContCount = 0;
                DataSet dsData = new DataSet("FltPlan_ReadCargoFromDB_dsData");
                dsData = objBuildULD.GetHAWBInfo(awbPrefix, awbNo, ULDNo, FlightNo);
                foreach (DataRow dr in dsData.Tables[0].Rows)
                {
                    try
                    {
                        Cargo objCargo = new Cargo();

                        try
                        {
                            objCargo.Origin = dr["Origin"].ToString();
                            objCargo.Destination = dr["Destination"].ToString();
                            objCargo.SpecialInfoCode = dr["SHCCode"].ToString();

                            if (Convert.ToString(dr["ULDNo"]).Trim() != "")
                            {
                                ContCount = ContCount + 1;
                                objCargo.TypeCode = Convert.ToString(dr["ULDNo"]).Substring(0, 3);
                                objCargo.ULDNumber = dr["ULDNo"].ToString().Replace(objCargo.TypeCode, "");
                                objCargo.LoadInfoCode = "C";
                                objCargo.Identifier = "Cont" + ContCount.ToString();
                                objCargo.Remarks = "Cont" + ContCount.ToString();
                                objCargo.CargoType = CargoType.Container;
                                objCargo.TareWeight = uldTareWt;
                            }
                            else
                            {
                                LooseCount = LooseCount + 1;
                                objCargo.LoadInfoCode = "B";
                                objCargo.Identifier = "loose" + LooseCount.ToString();
                                objCargo.Remarks = "loose" + LooseCount.ToString();
                                objCargo.CargoType = CargoType.LooseUnit;
                                objCargo.TareWeight = 0;
                            }
                        }
                        catch (Exception objEx)
                        {
                        }

                        try
                        {
                            String strTemp = dr["AcceptedWgt"].ToString();
                            if (!string.IsNullOrEmpty(dr["AcceptedWgt"].ToString()))
                                objCargo.CheckWeight = (int)Convert.ToDouble(strTemp);

                            if (!String.IsNullOrEmpty(dr["AcceptedPcs"].ToString()))
                                objCargo.NrUnits = Convert.ToInt32(dr["AcceptedPcs"].ToString());
                        }
                        catch (Exception objEx)
                        {
                        }
                        objCargoList.Add(objCargo);
                    }
                    catch (Exception objEx)
                    {
                    }
                }

            }
            catch (Exception objEx)
            {
            }
        }

        protected void CreateAndSendPDF(string toEmailId)
        {
            System.IO.MemoryStream Logo = null;
            DataSet ds2 = new DataSet("FltPlan_CreateAndSendPDF_ds2");
            ds2 = (DataSet)Session["FlightPlanning_dsLoad"];
            DataTable dtTable1 = new DataTable("FltPlan_CreateAndSendPDF_dtTable1");
            dtTable1 = (DataTable)Session["FlightPlanning_ULDNEW"];
            try
            {
                try
                {
                    Logo = CommonUtility.GetImageStream(Page.Server);
                }
                catch (Exception ex)
                {
                    Logo = new System.IO.MemoryStream();
                }

                string[] flt = Session["FlightPlanning_FltDet"].ToString().Split('-');
                string FltNo = flt[0].ToUpper();

                if (!dtTable4.Columns.Contains("Station"))
                    dtTable4.Columns.Add("Station", typeof(string));
                if (!dtTable4.Columns.Contains("FltNo"))
                    dtTable4.Columns.Add("FltNo", typeof(string));
                if (!dtTable4.Columns.Contains("ExecutionDate"))
                    dtTable4.Columns.Add("ExecutionDate", typeof(string));
                DataRow dr = dtTable4.NewRow();
                dr["Station"] = Session["Station"].ToString();
                dr["FltNo"] = FltNo;
                // dr["ExecutionDate"] = Session["IT"].ToString();
                dr["ExecutionDate"] = TextBoxdate.Text;
                dtTable4.Rows.Add(dr);

                if (dtTable1.Columns.Contains("Logo") == false)
                {
                    DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                    col1.DefaultValue = Logo.ToArray();
                    dtTable1.Columns.Add(col1);
                }

                #region Report
                dtTable2 = getAWBsFromAllCarts();

                if (dtTable2.Columns.Contains("Remark") == false)
                {
                    DataColumn col1 = new DataColumn("Remark", typeof(string));
                    col1.DefaultValue = "";
                    dtTable2.Columns.Add(col1);
                }

                if (!dtTable3.Columns.Contains("MAILID"))
                    dtTable3.Columns.Add("MAILID");
                if (!dtTable3.Columns.Contains("MAILWt"))
                    dtTable3.Columns.Add("MAILWt");
                if (!dtTable3.Columns.Contains("MAILOrigin"))
                    dtTable3.Columns.Add("MAILOrigin");
                if (!dtTable3.Columns.Contains("MAILDest"))
                    dtTable3.Columns.Add("MAILDest");
                if (!dtTable3.Columns.Contains("Dest"))
                    dtTable3.Columns.Add("Dest");
                if (!dtTable3.Columns.Contains("MAILLoadingPriority"))
                    dtTable3.Columns.Add("MAILLoadingPriority");
                if (!dtTable3.Columns.Contains("MAILPcs"))
                    dtTable3.Columns.Add("MAILPcs");
                if (!dtTable3.Columns.Contains("AccPcs"))
                    dtTable3.Columns.Add("AccPcs");
                if (!dtTable3.Columns.Contains("AccWt"))
                    dtTable3.Columns.Add("AccWt");
                DataRow dtrow1 = dtTable3.NewRow();
                DataRow dtrow2 = dtTable3.NewRow();
                DataRow dtrow3 = dtTable3.NewRow();
                DataRow dtrow4 = dtTable3.NewRow();
                DataRow dtrow5 = dtTable3.NewRow();
                dtTable3.Rows.Add(dtrow1);
                dtTable3.Rows.Add(dtrow2);
                dtTable3.Rows.Add(dtrow3);
                dtTable3.Rows.Add(dtrow4);
                dtTable3.Rows.Add(dtrow5);

                //dtTable3 = getAWBsFromAllCarts("CN38");

                //if (dtTable3.Columns.Contains("Remark") == false)
                //{
                //    DataColumn col1 = new DataColumn("Remark", typeof(string));
                //    col1.DefaultValue = "";
                //    dtTable3.Columns.Add(col1);
                //}

                ReportViewer rptLoadPlanReport = new ReportViewer();
                ReportDataSource rds1 = new ReportDataSource();
                rptLoadPlanReport.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = rptLoadPlanReport.LocalReport;

                rep1.ReportPath = Server.MapPath("/Reports/rptCargoManifest.rdlc");

                rds1.Name = "dsCargoManifest_DataTable1";

                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);

                rptLoadPlanReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                #region Render to PDF
                try
                {
                    string reportType = "PDF";
                    int OutboxSerial = 0;
                    //string mimeType;
                    //string encoding;
                    string fileNameExtension;
                    string deviceInfo = "<DeviceInfo><PageHeight>40cm</PageHeight><PageWidth>38cm</PageWidth></DeviceInfo>";
                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;
                    byte[] bytes = rptLoadPlanReport.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    Response.Buffer = true;
                    Response.Clear();
                    BalInterface objIBal = new BalInterface();
                    String strFltNoAndDate = "";
                    string strBody = "";
                    try
                    {
                        strFltNoAndDate = txtFlightCode.Text + txtFlightID.Text + "_" + TextBoxdate.Text.Replace('/', '-');
                        strBody = "Flight No :- " + txtFlightCode.Text + txtFlightID.Text + Environment.NewLine +
                                    "Flight Date:- " + TextBoxdate.Text;
                    }
                    catch (Exception objex)
                    {
                    }
                    string[] Extension = { ".pdf" };
                    string[] DocumentName = { "LoadPlan-" + strFltNoAndDate };
                    MemoryStream[] Document = { new MemoryStream(bytes) };

                    #region "Send by Email"

                    string strSubject = "Load Control - " + strFltNoAndDate;

                    //objIBal.sendMail("cebu@smartkargomsgs.com", toEmailId, "SCMMsgCebu1", strSubject, strBody, false, 25, "", Document, DocumentName, Extension);

                    #endregion "Send by Email"

                    #region "Upload on Bolb"

                    byte[] raw = bytes;
                    Stream decoded = new MemoryStream(raw);

                    string FileName = "LoadControl-" + strFltNoAndDate + ".pdf";
                    bool Uploaded = CommonUtility.UploadBlob(decoded, FileName);
                    OutboxSerial = cls_BL.DumpInterfaceInformation(strSubject, FileName, Convert.ToDateTime(Session["IT"]), "LoadControl", "", true,
                        "cebu@smartkargomsgs.com", toEmailId, Document[0], Extension[0]);

                    #endregion "Upload on Bolb"

                }
                catch (Exception ex)
                {

                }
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Logo = null;
                if (ds2 != null)
                    ds2.Dispose();
                if (dtTable1 != null)
                    dtTable1.Dispose();
            }
        }

        private void assignCargoDatax(ref ArrayList objCargoList, DataSet dsData)
        {
            try
            {
                foreach (DataRow dr in dsData.Tables[0].Rows)
                {
                    try
                    {
                        Cargo objCargo = new Cargo();
                        objCargo.CargoType = CargoType.LooseUnit;
                        objCargo.TareWeight = 0;
                        objCargo.LoadInfoCode = "";
                        objCargo.Identifier = "";
                        objCargo.SpecialInfoCode = "";

                        objCargo.Origin = dr["Origin"].ToString();
                        objCargo.Destination = dr["Destination"].ToString();
                        objCargo.Remarks = dr["ArrivalRemarks"].ToString();
                        objCargo.TypeCode = dr["CommodityCode"].ToString();
                        objCargo.ULDNumber = dr["ULDNo"].ToString();
                        if (String.IsNullOrEmpty(dr["ULDNo"].ToString()))
                        {
                            objCargo.TareWeight = uldTareWt;
                            objCargo.CargoType = CargoType.Pallet;
                        }
                        try
                        {
                            String strTemp = dr["AcceptedWgt"].ToString();
                            if (!string.IsNullOrEmpty(dr["AcceptedWgt"].ToString()))
                                objCargo.CheckWeight = (int)Convert.ToDouble(strTemp);

                            if (!String.IsNullOrEmpty(dr["AcceptedPcs"].ToString()))
                                objCargo.NrUnits = Convert.ToInt32(dr["AcceptedPcs"].ToString());
                        }
                        catch (Exception objEx)
                        {
                        }
                        objCargoList.Add(objCargo);
                    }
                    catch (Exception objEx)
                    {
                    }
                }
            }
            catch (Exception objEx)
            {
            }
        }

        protected void btnExportToManifest_Click(object sender, EventArgs e)
        {
            if (!GetFlightPlanningStatus())
                return;

            LoginBL lBal = new LoginBL();
            try
            {
                if (!ValidateExportToManiOnSave())
                    return;
            }
            catch (Exception ex)
            { }
            try
            {
                string strRes = "", FLTno = "", FltDate = "", Updatedon = "", Updatedby = "", POL = "";
                if (txtFlightID.Text.Trim() != "")
                    FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                else
                    FLTno = "";

                Updatedby = Convert.ToString(Session["Username"]);

                POL = Session["Station"].ToString();

                Updatedon = Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss");

                if (TextBoxdate.Text.Trim() == "")
                    FltDate = "";
                else
                    FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                DataTable dtULDList = new DataTable("FltPlan_btnExportToManifest_dtULDList");
                dtULDList = (DataTable)Session["FlightPlanning_ULDList"];

                //Code to Update Planning status of ULD and AWB under that ULD
                if (dtULDList.Rows.Count > 0)
                {
                    for (int row = 0; row < dtULDList.Rows.Count; row++)
                    {
                        strRes = objBuildULD.UpdateULDPlanningStatus(dtULDList.Rows[row]["Uldno"].ToString(), POL, FLTno, FltDate, Updatedon, Updatedby);
                        if (strRes != "ULD UPDATED")
                        {
                            //lblStatus.ForeColor = Color.Red;
                            //lblStatus.Text = "Error while exporting to manifest";
                            //return;
                        }
                    }
                }


                DataTable dtCartList = new DataTable("FltPlan_btnExportToManifest_dtCartList");
                dtCartList = (DataTable)Session["FlightPlanning_CartList"];

                //Code to Update Planning status of ULD and AWB under that ULD
                if (dtCartList.Rows.Count > 0)
                {
                    for (int row = 0; row < dtCartList.Rows.Count; row++)
                    {
                        strRes = objBuildULD.UpdateCartPlanningStatus(dtCartList.Rows[row]["Cartno"].ToString(), POL, FLTno, FltDate, Updatedon, Updatedby);
                        if (strRes != "CART UPDATED")
                        {
                            //lblStatus.ForeColor = Color.Red;
                            //lblStatus.Text = "Error while exporting to manifest";
                            //return;
                        }
                    }
                }


                //commented by Vijay - Added below functionality in objBuildULD.UpdateCartPlanningStatus
                //DataTable dtBulkAWBList = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];


                //Code to Update Planning status of AWB added to bulk
                //if (dtBulkAWBList.Rows.Count > 0)
                //{
                //    for (int row1 = 0; row1 < dtBulkAWBList.Rows.Count; row1++)
                //    {
                //        strRes = objBuildULD.UpdateBulkAWBPlanningStatus(dtBulkAWBList.Rows[row1]["AWBno"].ToString(), POL, FLTno, FltDate, Updatedon, Updatedby);
                //        if (strRes != "BULK AWB UPDATED")
                //        {
                //            //lblStatus.ForeColor = Color.Red;
                //            //lblStatus.Text = "Error while exporting to manifest";
                //            //return;
                //        }
                //    }
                //}


                DataTable dtAWBList = new DataTable("FltPlan_btnExportToManifest_dtAWBList");
                dtAWBList = (DataTable)Session["FlightPlanning_AWBdata"];

                ////Code to Update FlightNo = '' of AWB in Load Plan
                //if (dtAWBList.Rows.Count > 0)
                //{
                //    for (int row2 = 0; row2 < dtAWBList.Rows.Count; row2++)
                //    {
                //        strRes = objBuildULD.UpdateAWBFlightToBlank(dtAWBList.Rows[row2]["AWBNumber"].ToString(), POL, FLTno, FltDate, Updatedon, Updatedby);
                //        if (strRes != "AWB UPDATED")
                //        {
                //            //lblStatus.ForeColor = Color.Red;
                //            //lblStatus.Text = "Error while exporting to manifest";
                //            //return;
                //        }
                //    }
                //}

                BtnList_Click(sender, e);
                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = "Export to manifest successful";
            }
            catch (Exception ex)
            {

            }

            try
            {

                BalInterface objBalInterface = new BalInterface();
                CargoResponseSimple objResponse = null;

                object[] objQueryVal = { txtFlightCode.Text.Trim() + txtFlightID.Text.Trim(), DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null), lblDepAirport.Text.Trim() };
                DataSet ds = new DataSet("FltPlan_btnExportToManifest_ds");
                ds = objBalInterface.fetchFlywareBodyType(objQueryVal);
                if (ds != null && ds.Tables[0].Rows[0]["WideOrNarrow"].ToString().ToUpper() == "WIDE")
                {
                    paramCargoInfo objCargoParam = new paramCargoInfo();
                    objCargoParam.Arrival = ds.Tables[0].Rows[0]["Destination"].ToString().ToUpper();
                    objCargoParam.STD = Convert.ToDateTime(ds.Tables[0].Rows[0]["DepDateTime"].ToString());
                    objCargoParam.Departure = lblDepAirport.Text.Trim();
                    objCargoParam.AirlineIdentifier = txtFlightCode.Text.Trim();
                    objCargoParam.FlightNumber = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();

                    readCargoInfo(ref objCargoParam);
                    objResponse = objBalInterface.ExecuteCargoRequest(objCargoParam, Convert.ToDateTime(Session["IT"]));
                }
                else
                {
                    bool blnLoadControl = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "SendToLoadControl"));
                    if (blnLoadControl)
                    {
                        string EmailId = new BalInterface().FetchEmailForLoadControl(lblDepAirport.Text.Trim());
                        if (EmailId.Trim() != "")
                        {
                            // CreateAndSendPDF(EmailId.Trim());
                            btnPrintLoadPlan_Click(null, null);
                        }
                    }
                }
                objResponse = null;
            }
            catch (Exception objEx)
            {
            }


        }

        public static string TruncateString(string longString, int maxLength)
        {
            if (string.IsNullOrEmpty(longString) || longString.Length <= maxLength)
            {
                return longString;
            }
            else
            {
                return longString.Substring(0, maxLength);
            }
        }

        protected void btnPrintWtStmt_Click(object sender, EventArgs e)
        {
            System.IO.MemoryStream Logo = null;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = new DataSet("FltPlan_btnPrintWt_ds");
            ReportViewer rptUCRReport = new ReportViewer();
            ReportDataSource rds1 = new ReportDataSource();
            LocalReport rep1 = null;

            try
            {
                for (int i = 0; i < grdULDList.Rows.Count; i++)
                {
                    if (((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
                    {
                        //Logo

                        try
                        {
                            Logo = CommonUtility.GetImageStream(Page.Server);
                            //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        }
                        catch (Exception ex)
                        {
                            Logo = new System.IO.MemoryStream();
                        }

                        string[] fltdet = Session["FlightPlanning_FltDet"].ToString().Split('-');
                        string uldno = ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text;
                        string fltno = fltdet[0];
                        string fltdt = fltdet[1];
                        string org = ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOL")).Text;
                        string dest = ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOU")).Text;

                        object[] param = { uldno, fltdt, fltno, org, dest };
                        string[] pname = { "uldNo", "fltdt", "fltno", "org", "dest" };
                        SqlDbType[] QueryTypes = new SqlDbType[5];
                        QueryTypes[0] = SqlDbType.VarChar;
                        QueryTypes[1] = SqlDbType.VarChar;
                        QueryTypes[2] = SqlDbType.VarChar;
                        QueryTypes[3] = SqlDbType.VarChar;
                        QueryTypes[4] = SqlDbType.VarChar;

                        ds = da.SelectRecords("sp_getULDWtStatement", pname, param, QueryTypes);
                        if (ds != null)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataTable dt = new DataTable("FltPlan_btnPrintWt_dt");
                                dt = ds.Tables[0].Copy();
                                if (dt.Columns.Contains("Logo") == false)
                                {
                                    DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                    col1.DefaultValue = Logo.ToArray();
                                    dt.Columns.Add(col1);
                                }
                                if (dt.Columns.Contains("TIME") == false)
                                {
                                    DataColumn TIME = new DataColumn("TIME", System.Type.GetType("System.String"));
                                    TIME.DefaultValue = Session["IT"].ToString();
                                    dt.Columns.Add(TIME);
                                }
                                rptUCRReport.ProcessingMode = ProcessingMode.Local;

                                rep1 = rptUCRReport.LocalReport;

                                rep1.ReportPath = Server.MapPath("/Reports/ULDWtStatement_New.rdlc");

                                rds1.Name = "dsWtStatement_DataTable1";
                                rds1.Value = dt;
                                rep1.DataSources.Add(rds1);

                                //rptUCRReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                                #region Render to PDF
                                try
                                {
                                    string reportType = "PDF";
                                    //string mimeType;
                                    //string encoding;
                                    string fileNameExtension;
                                    string deviceInfo = "<DeviceInfo><PageHeight>40cm</PageHeight><PageWidth>70cm</PageWidth></DeviceInfo>";

                                    //"<DeviceInfo>" +

                                    //"  <OutputFormat>PDF</OutputFormat>" +

                                    //"</DeviceInfo>";

                                    Warning[] warnings;
                                    string[] streamIds;
                                    string mimeType = string.Empty;
                                    string encoding = string.Empty;
                                    string extension = string.Empty;

                                    //Render the report
                                    // send it to the client to download
                                    byte[] bytes = rptUCRReport.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                                    Response.Buffer = true;
                                    Response.Clear();
                                    Response.ContentType = mimeType;
                                    Response.AddHeader("content-disposition", "attachment; filename=" + "ULDWtStatement" + "." + ".pdf");
                                    Response.BinaryWrite(bytes); // create the file
                                    Response.Flush();


                                    //Response.End();
                                }
                                catch (Exception ex)
                                {

                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            catch { }
            finally
            {
                Logo = null;
                da = null;
                if (ds != null)
                    ds.Dispose();
                rptUCRReport = null;
                rds1 = null;
                rep1 = null;
            }
        }

        protected void btnPrnULDPlan_Click(object sender, EventArgs e)
        {
            System.IO.MemoryStream Logo = null;
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = new DataSet("FltPlan_btnPrintULDPlan_ds");
            ReportViewer rptULDPlan = new ReportViewer();
            ReportDataSource rds1 = new ReportDataSource();
            LocalReport rep1 = null;
            DataTable dtAWBDetails = new DataTable("FltPlan_btnPrintULDPlan_dtAWBDetails");
            dtAWBDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];
            for (int i = 0; i < grdULDList.Rows.Count; i++)
            {
                if (((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
                {
                    //Logo
                    try
                    {
                        Logo = CommonUtility.GetImageStream(Page.Server);
                        //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                    }
                    catch (Exception ex)
                    {
                        Logo = new System.IO.MemoryStream();
                    }

                    string[] fltdet = Session["FlightPlanning_FltDet"].ToString().Split('-');
                    string uldno = ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text;
                    string fltno = fltdet[0];
                    string fltdt = fltdet[1];
                    string org = ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOL")).Text;
                    string dest = ((TextBox)grdULDList.Rows[i].FindControl("txtULDPOU")).Text;

                    object[] param = { uldno, fltdt, fltno, org, dest };
                    string[] pname = { "uldNo", "fltdt", "fltno", "org", "dest" };
                    SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                    ds = da.SelectRecords("sp_getULDWtStatement", pname, param, QueryTypes);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = new DataTable("FltPlan_btnPrnLoadPlan_dt");
                            dt = ds.Tables[0].Copy();
                            if (dt.Columns.Contains("Logo") == false)
                            {
                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                col1.DefaultValue = Logo.ToArray();
                                dt.Columns.Add(col1);
                            }
                            if (dt.Columns.Contains("ExecutionDate") == false)
                            {
                                DataColumn col1 = new DataColumn("ExecutionDate", typeof(string));
                                col1.DefaultValue = Session["IT"].ToString();
                                dt.Columns.Add(col1);
                            }
                            //if (dt.Columns.Contains("GrWtWithDolly") == false)
                            //    dt.Columns.Add("GrWtWithDolly");

                            //for (int j = 0; j < dt.Rows.Count; j++)
                            //{
                            //    decimal GrWt = 0, DollyWt = 0;
                            //    if (dt.Rows[j]["GrWt"].ToString() != "")
                            //        GrWt = decimal.Parse(dt.Rows[j]["GrWt"].ToString());
                            //    if (dt.Rows[j]["DollyWt"].ToString() != "")
                            //        DollyWt = decimal.Parse(dt.Rows[j]["DollyWt"].ToString());
                            //    dt.Rows[j]["GrWtWithDolly"] = (GrWt + DollyWt).ToString();
                            //}

                            rptULDPlan.ProcessingMode = ProcessingMode.Local;

                            rep1 = rptULDPlan.LocalReport;

                            rep1.ReportPath = Server.MapPath("/Reports/rptULDDetails.rdlc");

                            rds1.Name = "dsULDDetails_DataTable1";
                            rds1.Value = dt;
                            rep1.DataSources.Add(rds1);

                            rptULDPlan.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandlerForULDPLan);
                            #region Render to PDF
                            try
                            {
                                string reportType = "PDF";
                                //string mimeType;
                                //string encoding;
                                string fileNameExtension;
                                string deviceInfo = "<DeviceInfo><PageHeight>20cm</PageHeight><PageWidth>27cm</PageWidth></DeviceInfo>";

                                //"<DeviceInfo>" +

                                //"  <OutputFormat>PDF</OutputFormat>" +

                                //"</DeviceInfo>";

                                Warning[] warnings;
                                string[] streamIds;
                                string mimeType = string.Empty;
                                string encoding = string.Empty;
                                string extension = string.Empty;

                                //Render the report
                                // send it to the client to download
                                byte[] bytes = rptULDPlan.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                                Response.Buffer = true;
                                Response.Clear();
                                Response.ContentType = mimeType;
                                Response.AddHeader("content-disposition", "attachment; filename=" + "ULDDetails" + "." + ".pdf");
                                Response.BinaryWrite(bytes); // create the file
                                Response.Flush();


                                //Response.End();
                            }
                            catch (Exception ex)
                            {

                            }
                            #endregion
                        }
                    }
                }
            }
        }

        public void ItemsSubreportProcessingEventHandlerForULDPLan(object sender, SubreportProcessingEventArgs e)
        {
            DataTable dtAWBDetails = new DataTable("FltPlan_EventHandlerULDPlan_dtAWBDetails");
            dtAWBDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];
            if (dtAWBDetails == null || dtAWBDetails.Rows.Count <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    DataRow dr = dtAWBDetails.NewRow();
                    dtAWBDetails.Rows.Add(dr);
                }
            }
            else
            {
                if (dtAWBDetails.Columns.Contains("User") == false)
                {
                    DataColumn col1 = new DataColumn("User", typeof(string));
                    col1.DefaultValue = Session["UserName"].ToString();
                    dtAWBDetails.Columns.Add(col1);
                }
            }
            e.DataSources.Add(new ReportDataSource("dsULDDetails_DataTable2", dtAWBDetails));

        }

        protected void btnPrintLoadPlan_Click(object sender, EventArgs e)
        {
            //Logo
            System.IO.MemoryStream Logo = null;
            DataSet ds2 = new DataSet("FltPlan_btnPrintLoadPlan_ds2");
            ds2 = (DataSet)Session["FlightPlanning_dsLoad"];
            DataTable dtTable1 = new DataTable("FltPlan_btnPrintLoadPlan_dtTable1");
            dtTable1 = (DataTable)Session["FlightPlanning_ULDNEW"];
           
            try
            {
                try
                {
                    Logo = CommonUtility.GetImageStream(Page.Server);
                }
                catch (Exception ex)
                {
                    Logo = new System.IO.MemoryStream();
                }

                string[] flt = Session["FlightPlanning_FltDet"].ToString().Split('-');
                string FltNo = flt[0].ToUpper();

                if (!dtTable4.Columns.Contains("Station"))
                    dtTable4.Columns.Add("Station", typeof(string));
                if (!dtTable4.Columns.Contains("FltNo"))
                    dtTable4.Columns.Add("FltNo", typeof(string));
                if (!dtTable4.Columns.Contains("ExecutionDate"))
                    dtTable4.Columns.Add("ExecutionDate", typeof(string));
                DataRow dr = dtTable4.NewRow();
                dr["Station"] = Session["Station"].ToString();
                dr["FltNo"] = FltNo;
                // dr["ExecutionDate"] = Session["IT"].ToString();
                dr["ExecutionDate"] = TextBoxdate.Text;

                dtTable4.Rows.Add(dr);

                if (dtTable1.Columns.Contains("Logo") == false)
                {
                    DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                    col1.DefaultValue = Logo.ToArray();
                    dtTable1.Columns.Add(col1);
                }

                #region Report

                dtTable2 = getAWBsFromAllCarts();

                if (dtTable2.Columns.Contains("Remark") == false)
                {
                    DataColumn col1 = new DataColumn("Remark", typeof(string));
                    col1.DefaultValue = "";
                    dtTable2.Columns.Add(col1);
                }

                #region Cart Summary

                dtCartSummary.Columns.Add("CartNumber", typeof(string));
                dtCartSummary.Columns.Add("BuiltPcs", typeof(string));
                dtCartSummary.Columns.Add("BuiltWt", typeof(string));
                dtCartSummary.Columns.Add("ScaleWeight", typeof(string));
                dtCartSummary.Columns.Add("LoadingPriority", typeof(string));

                ArrayList al = new ArrayList();
                for (int i = 0; i < dtTable2.Rows.Count; i++)
                {
                    if (dtTable2.Rows[i]["CartNumber"].ToString() != "")
                    {
                        if (al.Count <= 0)
                            al.Add(dtTable2.Rows[i]["CartNumber"].ToString());
                        else if (!al.Contains(dtTable2.Rows[i]["CartNumber"].ToString()))
                            al.Add(dtTable2.Rows[i]["CartNumber"].ToString());
                    }
                }

                for (int i = 0; i < al.Count; i++)
                {
                    DataTable dt = new DataTable("FltPlan_btnPrintLoadPlan_dt");
                    dt = dtTable2.Select("CartNumber='" + al[i].ToString() + "'").CopyToDataTable();

                    decimal Pcs = 0, Wt = 0;
                    string ScaleWeight = string.Empty;
                    string LoadingPriority = string.Empty;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        Pcs += Convert.ToDecimal(dt.Rows[j]["BuiltPcs"].ToString());
                        Wt += Convert.ToDecimal(dt.Rows[j]["BuiltWt"].ToString());
                        ScaleWeight = dt.Rows[j]["ScaleWeight"].ToString();
                        LoadingPriority = dt.Rows[j]["AWBLoadingPriority"].ToString();
                    }
                    DataRow drCart = dtCartSummary.NewRow();
                    drCart["CartNumber"] = al[i].ToString();
                    drCart["BuiltPcs"] = Pcs;
                    drCart["BuiltWt"] = Wt;
                    drCart["ScaleWeight"] = ScaleWeight;
                    drCart["LoadingPriority"] = LoadingPriority;

                    dtCartSummary.Rows.Add(drCart);
                }

                #endregion

                #region  PO Mail - added by Amit.

                try
                {
                    dtTable3 = dtTable2.Clone();
                    for (int i = 0; i < dtTable2.Rows.Count; i++)
                    {
                        if (dtTable2.Rows[i]["DocumentType"].ToString() == "CN38")
                        {
                            DataRow dr1 = dtTable3.NewRow();
                            for (int j = 0; j < dr1.Table.Columns.Count; j++)
                            {
                                dr1[j] = dtTable2.Rows[i][j].ToString();
                            }
                            dtTable3.Rows.Add(dr1);
                            dtTable2.Rows[i].Delete();
                            i--;
                        }
                        else
                        {   
                            if (dtTable2.Rows[i]["CartNumber"] == null 
                                || dtTable2.Rows[i]["CartNumber"].ToString() == "")
                            {   //Delete AWBs from Bulk Details which are part of ULD.
                                dtTable2.Rows[i].Delete();
                                i--;
                            }
                        }
                    }

                }
                catch (Exception)
                {
                   
                }

                if (dtTable3.Columns.Contains("Remark") == false)
                {
                    DataColumn col1 = new DataColumn("Remark", typeof(string));
                    col1.DefaultValue = "";
                    dtTable3.Columns.Add(col1);
                }

                #endregion


                ReportViewer rptLoadPlanReport = new ReportViewer();
                ReportDataSource rds1 = new ReportDataSource();
                rptLoadPlanReport.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = rptLoadPlanReport.LocalReport;

                rep1.ReportPath = Server.MapPath("/Reports/rptCargoManifest.rdlc");

                rds1.Name = "dsCargoManifest_DataTable1";

                rds1.Value = dtTable1; //ULD Section Table

                rep1.DataSources.Add(rds1);

                rptLoadPlanReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                #region Render to PDF
                try
                {
                    string deviceInfo = "<DeviceInfo></DeviceInfo>";

                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;

                    //Render the report
                    // send it to the client to download
                    byte[] bytes = rptLoadPlanReport.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    Response.Buffer = true;
                    Response.Clear();

                    if (sender == null && e == null)
                    {
                        try
                        {
                            BalInterface objIBal = new BalInterface();
                            String strFltNoAndDate = "";
                            string strBody = "";
                            try
                            {
                                strFltNoAndDate = txtFlightCode.Text + txtFlightID.Text + "_" + TextBoxdate.Text.Replace('/', '-');
                                strBody = "Flight No   :- " + txtFlightCode.Text + txtFlightID.Text + Environment.NewLine +
                                          "Flight Date :- " + TextBoxdate.Text;
                            }
                            catch (Exception)
                            {
                            }
                            string[] Extension = { ".pdf" };
                            string[] DocumentName = { "LoadPlan-" + strFltNoAndDate };
                            MemoryStream[] Document = { new MemoryStream(bytes) };

                            #region "Send by Email"

                            string strSubject = "Load Control - " + strFltNoAndDate;

                            //string toEmailId = new LoginBL().GetMasterConfiguration("ToEmailIdForLoadControl");
                            string toEmailId = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ToEmailIdForLoadControl");
                            //objIBal.sendMail("cebu@smartkargomsgs.com", toEmailId, "SCMMsgCebu1", strSubject, strBody, false, 25, "", Document, DocumentName, Extension);

                            #endregion "Send by Email"

                            #region "Upload on Bolb"

                            byte[] raw = bytes;
                            Stream decoded = new MemoryStream(raw);

                            string FileName = "LoadControl-" + strFltNoAndDate + ".pdf";
                            
                            CommonUtility.UploadBlob(decoded, FileName);
                            
                            cls_BL.DumpInterfaceInformation(strSubject, FileName, Convert.ToDateTime(Session["IT"]), "LoadControl", "", true,
                                "cebu@smartkargomsgs.com", new BalInterface().FetchEmailForLoadControl(lblDepAirport.Text.Trim()), Document[0], Extension[0]);

                            raw = null;
                            if (decoded != null)
                                decoded.Dispose();
                                
                            #endregion "Upload on Bolb"


                        }
                        catch (Exception)
                        {
                        }
                        return;
                    }

                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + "ULDLoadPlan" + "." + ".pdf");
                    Response.BinaryWrite(bytes); // create the file
                    Response.Flush();

                    bytes = null;
                }
                catch (Exception)
                {

                }
                #endregion
                #endregion
            }
            catch (Exception)
            {
            }
            finally
            {
                Logo = null;
                if (ds2 != null)
                    ds2.Dispose();
                if (dtTable1 != null)
                    dtTable1.Dispose();
            }
        }

        #region Button NoToc Print Click
        protected void btnNoToc_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;
                showNOTOCData();
            }
            catch (Exception)
            { }
        }
        #endregion

        #region Function to Show NOTC
        private void showNOTOCData()
        {
            System.IO.MemoryStream Logo = null;
            DataTable DTNOTCDetails = new DataTable("FltPlan_showNOTOCData_DTNOTCDetails");
            DataSet DsNationality = new DataSet("FltPlan_showNOTOCData_DsNationality");
            DataTable DTNOTOCAWBs = new DataTable("FltPlan_showNOTOCData_DTNOTOCAWBs");
            DataTable DTNOTOCOtherLoad = new DataTable("FltPlan_showNOTOCData_DTNOTOCOtherLoad");
            DataSet ds = new DataSet("FltPlan_showNOTOCData_ds");
            DataTable DTDGR = new DataTable("FltPlan_showNOTOCData_DTDGR");
            DataTable DTSPC = new DataTable("FltPlan_showNOTOCData_DTSPC");
            ReportDataSource rds1_FltPlanning = new ReportDataSource();
            DataTable dtTable1 = new DataTable("FltPlan_showNOTOCData_dtTable1");
            try
            {
                Session["DTNOTOC_FltPln"] = null;
                Session["DTNOTCAWBs_FltPln"] = null;
                //img for report
                try
                {
                    Logo = CommonUtility.GetImageStream(Page.Server);
                }
                catch (Exception ex)
                {
                    Logo = new System.IO.MemoryStream();
                }

                DTNOTCDetails.Columns.Add("NotocID");
                DTNOTCDetails.Columns.Add("LoadingStation");
                DTNOTCDetails.Columns.Add("FlightNumber");
                DTNOTCDetails.Columns.Add("Date");
                DTNOTCDetails.Columns.Add("Registration");
                DTNOTCDetails.Columns.Add("PreparedBy");
                DTNOTCDetails.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));

                DateTime dt = new DateTime();
                string strNationality = "";
                try
                {

                    dt = DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null);
                    DsNationality = objExpMani.GetPOUAirlineSchedule(txtFlightCode.Text + txtFlightID.Text, 
                        Session["Station"].ToString(), dt);
                    strNationality = txtTailNo.Text;
                }
                catch (Exception)
                {
                }

                if (grdBulkAssignedAWB.Rows.Count >= 0)
                {
                    DataRow drNotocDetails = DTNOTCDetails.NewRow();
                    drNotocDetails["NotocID"] = "";
                    drNotocDetails["LoadingStation"] = Session["Station"].ToString();
                    drNotocDetails["FlightNumber"] = txtFlightCode.Text + txtFlightID.Text;
                    drNotocDetails["Date"] = lblDate.Text.TrimStart('|');
                    drNotocDetails["Registration"] = strNationality;
                    drNotocDetails["PreparedBy"] = Session["Username"].ToString();
                    drNotocDetails["Logo"] = Logo.ToArray();
                    DTNOTCDetails.Rows.Add(drNotocDetails);
                }

                //string strFlightNo = txtFlightCode.Text + txtFlightID.Text + "        " + TextBoxdate.Text;
                //string strPreparedBy = Session["Username"].ToString();
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
                DTNOTOCOtherLoad.Columns.Add("ERGCode");
                if (grdBulkAssignedAWB.Rows.Count > 0)
                {
                    for (int i = 0; i < grdBulkAssignedAWB.Rows.Count; i++)
                    {
                        DataRow[] DtRow = new DataRow[0];

                        string AWBNo = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAWBNo")).Text;
                       // string Uldno = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblULDNo")).Text;
                        string LoadingComp = "-";
                        //For DGR Cargo
                        try
                        {
                            ds = objExpMani.GetDGRCargoDetails(AWBNo.Substring(AWBNo.Length - 8));
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                
                                for (int ii = 0; ii < ds.Tables[0].Rows.Count; ii++)
                                {

                                    Array.Resize(ref DtRow, DtRow.Length + 1);

                                    DtRow[DtRow.Length - 1] = DTNOTOCAWBs.NewRow();

                                    if (ds.Tables[0].Rows[ii]["AWBNumber"].ToString() == AWBNo.Substring(AWBNo.Length - 8))
                                    {
                                        DtRow[DtRow.Length - 1]["AWBNo"] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblAWBNo")).Text;
                                        DtRow[DtRow.Length - 1]["POU"] = Session["FlightPlanning_Dest"] != null ? Session["FlightPlanning_Dest"].ToString() : string.Empty;
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
                                        if (((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("lblULDno")).Text != "")
                                        {
                                            DtRow[DtRow.Length - 1]["ULDNo"] = ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("lblULDno")).Text;
                                        }
                                        else

                                            DtRow[DtRow.Length - 1]["ULDNo"] = ds.Tables[0].Rows[ii]["ULDNo"].ToString();
                                       // DtRow[DtRow.Length - 1]["ULDNo"] = ds.Tables[0].Rows[ii]["ULDNo"].ToString();
                                        DtRow[DtRow.Length - 1]["Description"] = ds.Tables[0].Rows[ii]["Description"].ToString();
                                        DtRow[DtRow.Length - 1]["SHCDescription"] = ds.Tables[0].Rows[ii]["SHCDescription"].ToString();
                                        DtRow[DtRow.Length - 1]["ERGCode"] = ds.Tables[0].Rows[ii]["ERGCode"].ToString();
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
                                OtherAWBNo = ds.Tables[1].Rows[0]["AWBNumber"].ToString();
                                CommDesc = ds.Tables[1].Rows[0]["Description"].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        DataRow DtRowOther = DTNOTOCOtherLoad.NewRow();
                        if (OtherAWBNo == AWBNo.Substring(AWBNo.Length - 8))
                        {
                            DtRowOther["POU"] = Session["FlightPlanning_Dest"] != null ? Session["FlightPlanning_Dest"].ToString() : string.Empty;
                            DtRowOther["AirWayBillNumber"] = OtherAWBNo;
                            DtRowOther["Description"] = CommDesc;
                            DtRowOther["NoOfPackages"] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltPcs")).Text;
                            DtRowOther["Quantity"] = ((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblBuiltWt")).Text;
                            DtRowOther["Information"] = "";
                            DtRowOther["Code"] = "";
                            DtRowOther["LoadingCompartment"] = "";
                            DtRowOther["LaodedAsShown"] = "";
                            DtRowOther["OtherInformation"] = "";

                            DTNOTOCOtherLoad.Rows.Add(DtRowOther);
                        }
                        DtRow = null;
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

                try
                {

                    dtTable1 = DTNOTCDetails;

                    dtTable2_NoToc = DTDGR;
                    dtTable3 = DTNOTOCOtherLoad;
                    dtTable4_NoToc = DTSPC;
                    ReportViewer ReportViewer1 = new ReportViewer();
                    ReportViewer1.Visible = true;



                    ReportViewer1.Reset();

                    ReportViewer1.ProcessingMode = ProcessingMode.Local;

                    LocalReport rep1 = ReportViewer1.LocalReport;

                    rep1.ReportPath = Server.MapPath("/Reports/NOTOC_New.rdlc");
                    //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "NOTOC.rdlc";
                    rds1_FltPlanning.Name = "dsNOTOC_New_dtNOTOC_New";// "dsNotoc_dtNotoc'";
                    rds1_FltPlanning.Value = dtTable1;
                    rep1.DataSources.Add(rds1_FltPlanning);


                    ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler_NoToc);
                    //ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);


                    //this.ReportViewer1.LocalReport.Refresh(); 

                    #region Render to PDF
                    try
                    {
                        string reportType = "PDF";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;
                        string deviceInfo = "<DeviceInfo></DeviceInfo>";

                        //"<DeviceInfo>" +

                        //"  <OutputFormat>PDF</OutputFormat>" +

                        //"</DeviceInfo>";

                        Warning[] warnings;
                        string[] streams;
                        byte[] renderedBytes;

                        //Render the report

                        renderedBytes = rep1.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                        Response.Clear();

                        Response.ContentType = mimeType;

                        Response.AddHeader("content-disposition", "attachment; filename=NOTOC Doc" + "." + fileNameExtension);

                        Response.BinaryWrite(renderedBytes);

                        warnings = null;
                        streams = null;
                        renderedBytes = null;
                        if (rep1 != null)
                            rep1.Dispose();

                    }
                    catch (Exception)
                    {
                    }
                    #endregion

                }
                catch (Exception)
                {
                }
                finally
                {
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Logo = null;
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
                if (dtTable1 != null)
                    dtTable1.Dispose();
                if (DTDGR != null)
                    DTDGR.Dispose();
                if (DTSPC != null)
                    DTSPC.Dispose();
                rds1_FltPlanning = null;
            }
        }
        #endregion


        #region ItemsSubreportProcessingEventHandler_NoToc
        public void ItemsSubreportProcessingEventHandler_NoToc(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsNOTOC_New_dtSubRpt_New", dtTable2_NoToc));
            e.DataSources.Add(new ReportDataSource("dsNOTOC_New_dtSubRpt_SPC1", dtTable4_NoToc));
        }
        #endregion ItemsSubreportProcessingEventHandler_NoToc

        protected DataTable getAWBsFromAllCarts()
        {
            // type can be AWB Or POMail
            string FLTno = "", FltDate = "", POL = "", CartNumber = "";
            if (txtFlightID.Text.Trim() != "")
                FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
            else
                FLTno = "";

            POL = Session["Station"].ToString();

            FltDate = TextBoxdate.Text;

            tabAssignedBulkAWBDetails = (DataTable)Session["FlightPlanning_PrintLoadPlan"];
            tabAssignedBulkAWBDetails.Rows.Clear();

            DataTable tabBulkLoadCart = new DataTable("FltPlan_getAWBsFromAllCarts_tabBulkLoadCart");
            tabBulkLoadCart = tabAssignedBulkAWBDetails.Copy();
            tabBulkLoadCart.Columns.Add("ScaleWeight");

            DataSet dsbulkawbData = new DataSet("FltPlan_getAWBsFromAllCarts_dsbulkawbData");
            dsbulkawbData = objBuildULD.GetBulkAssignedAWBDetailsFromCart(lblDepAirport.Text, FLTno, FltDate, "", "", CartNumber);

            if (dsbulkawbData.Tables.Count > 0)
            {
                if (dsbulkawbData.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsbulkawbData.Tables[0].Rows.Count; i++)
                    {
                        DataRow l_Datarow = tabBulkLoadCart.NewRow();

                        l_Datarow["AWBno"] = dsbulkawbData.Tables[0].Rows[i]["AWBNo"].ToString();
                        l_Datarow["ULDno"] = dsbulkawbData.Tables[0].Rows[i]["Uldno"].ToString();
                        l_Datarow["TotalPcs"] = dsbulkawbData.Tables[0].Rows[i]["TotalPcs"].ToString();
                        l_Datarow["TotalWt"] = dsbulkawbData.Tables[0].Rows[i]["TotalWt"].ToString();
                        l_Datarow["AcceptedPcs"] = dsbulkawbData.Tables[0].Rows[i]["AcceptedPcs"].ToString();
                        l_Datarow["AcceptedWt"] = dsbulkawbData.Tables[0].Rows[i]["AcceptedWt"].ToString();
                        l_Datarow["BuiltPcs"] = dsbulkawbData.Tables[0].Rows[i]["pieces"].ToString();
                        l_Datarow["BuiltWt"] = dsbulkawbData.Tables[0].Rows[i]["GWeight"].ToString();
                        l_Datarow["Origin"] = dsbulkawbData.Tables[0].Rows[i]["ULDOrigin"].ToString(); ;
                        l_Datarow["Dest"] = dsbulkawbData.Tables[0].Rows[i]["ULDDest"].ToString();
                        l_Datarow["Location"] = dsbulkawbData.Tables[0].Rows[i]["Location"].ToString();
                        l_Datarow["AWBLoadingPriority"] = dsbulkawbData.Tables[0].Rows[i]["LoadingPriority"].ToString();
                        l_Datarow["AWBBuilderName"] = dsbulkawbData.Tables[0].Rows[i]["AWBBuilderName"].ToString();
                        l_Datarow["FlightExists"] = dsbulkawbData.Tables[0].Rows[i]["FlightExists"].ToString();
                        l_Datarow["CartNumber"] = dsbulkawbData.Tables[0].Rows[i]["CartNo"].ToString();
                        l_Datarow["FltNo"] = dsbulkawbData.Tables[0].Rows[i]["FltNo"].ToString();
                        l_Datarow["FltDate"] = dsbulkawbData.Tables[0].Rows[i]["FltDate"].ToString();
                        l_Datarow["ProductType"] = dsbulkawbData.Tables[0].Rows[i]["ProductType"].ToString();
                        l_Datarow["Description"] = dsbulkawbData.Tables[0].Rows[i]["Description"].ToString();
                        l_Datarow["CommodityCode"] = dsbulkawbData.Tables[0].Rows[i]["CommodityCode"].ToString();
                        l_Datarow["SHC"] = dsbulkawbData.Tables[0].Rows[i]["SHC"].ToString();
                        l_Datarow["ScaleWeight"] = dsbulkawbData.Tables[0].Rows[i]["ScaleWeight"].ToString();
                        l_Datarow["DocumentType"] = dsbulkawbData.Tables[0].Rows[i]["DocumentType"].ToString().Trim();

                        tabBulkLoadCart.Rows.Add(l_Datarow);
                    }
                }
            }
            if (dsbulkawbData != null)
            {
                dsbulkawbData.Dispose();
            }
            return tabBulkLoadCart;
        }

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            if (dtTable2 == null || dtTable2.Rows.Count <= 0)//dtTable2-AWB Section
            {
                for (int i = 0; i < 4; i++)
                {
                    DataRow dr = dtTable2.NewRow();
                    dtTable2.Rows.Add(dr);
                }
            }
            else if (dtTable2.Rows.Count < 5 && dtTable2.Rows.Count > 0)
            {
                int row = 5 - dtTable2.Rows.Count;
                for (int i = 0; i < row; i++)
                {
                    DataRow dr = dtTable2.NewRow();
                    dtTable2.Rows.Add(dr);
                }
            }
            e.DataSources.Add(new ReportDataSource("dsCargoManifest_DataTable5", dtTable2));

            //if (dtTable3!= null)//dtTable3-MAIL Section

            if (dtTable3 == null || dtTable3.Rows.Count <= 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    DataRow dr = dtTable3.NewRow();
                    dtTable3.Rows.Add(dr);
                }
            }
            else if (dtTable3.Rows.Count < 5 && dtTable3.Rows.Count > 0)
            {
                int row = 5 - dtTable3.Rows.Count;
                for (int i = 0; i < row; i++)
                {
                    DataRow dr = dtTable3.NewRow();
                    dtTable3.Rows.Add(dr);
                }
            }
            e.DataSources.Add(new ReportDataSource("dsCargoManifest_DataTable3", dtTable3));


            if (dtTable4 != null)//dtTable4-Header Section
                e.DataSources.Add(new ReportDataSource("dsCargoManifest_DataTable4", dtTable4));

            e.DataSources.Add(new ReportDataSource("dsCargoManifest_dtCartSumm", dtCartSummary));
        }

        protected void btnFltPln_Click(object sender, EventArgs e)
        {
            if (txtFlightID.Text == "")
            {
                lblStatus.Text = "Please enter FlightID";
                lblStatus.ForeColor = Color.Red;
                return;
            }
            else
            {
                lblStatus.Text = "";
                lblULDStatus.Text = "";

                if (Convert.ToInt16(Session["FlightPlanning_ShowDest"]) == 1)
                {
                    lblStatus.Text = "Please select Flight No tab above to view Flight Build Plan";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                Session["GHA_AssignULD_HAWBAddToULD"] = null;
                Session["GHA_AssignULD_HAWBOriginal"] = null;
                Session["GHA_AssignULD_HAWBAddToBulk"] = null;
                gdvULDLoadPlanAWB.DataSource = null;
                gdvULDLoadPlanAWB.DataBind();
                BindAWBLoadPlanULDS();
                //btnNewULD.Visible = true;
                btnPrintWtStmt.Enabled = true;
                // BindBulkAssignedAWBS();
                // CheckFlightStatus();
            }
        }

        public void BindAWBLoadPlanULDS()
        {
            lblStatus.Text = "";
            lblULDStatus.Text = "";
            DataSet dsawb = new DataSet("FltPlan_BindAWBLoadPlanULDS_dsawb");
            DataTable MemDetails1 = new DataTable("FltPlan_BindAWBLoadPlanULDS_MemDetails1");
            Session["FlightPlannning_dsLoad"] = null;
            Session["FlightPlannning_ULDNEW"] = null;

            try
            {

                string flightID;
                if (txtFlightID.Text.Trim() != "")
                    flightID = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                else
                    flightID = "";

                string FlightDate = TextBoxdate.Text;
                Session["FlightPlannning_FltDet"] = flightID + "-" + FlightDate; //Shubh
                dsawb = objBuildULD.GetAWBLoadPlanULDDetails(lblDepAirport.Text, flightID, FlightDate, 
                    txtAWBPrefix.Text.Trim(), txtAWBNo.Text.Trim(), "");

                Session["FlightPlannning_dsLoad"] = dsawb;
                Session["FlightPlannning_ULDNEW"] = dsawb.Tables[2];
                MemDetails1.Columns.Add("AWBNumber");
                MemDetails1.Columns.Add("ShipperName");
                MemDetails1.Columns.Add("CommodityCode");
                MemDetails1.Columns.Add("Priority");
                MemDetails1.Columns.Add("TotalPieces");
                MemDetails1.Columns.Add("TotalWeight");
                MemDetails1.Columns.Add("Description");
                MemDetails1.Columns.Add("ProductType");
                if (dsawb != null)
                {
                    for (int j = 0; j < dsawb.Tables[0].Rows.Count; j++)
                    {
                        DataRow l_Datarow = MemDetails1.NewRow();
                        MemDetails1.Rows.Add(l_Datarow);
                        MemDetails1.Rows[j]["AWBNumber"] = dsawb.Tables[0].Rows[j]["AWBNumber"];
                        MemDetails1.Rows[j]["ShipperName"] = dsawb.Tables[0].Rows[j]["ShipperName"];
                        MemDetails1.Rows[j]["CommodityCode"] = dsawb.Tables[0].Rows[j]["CommodityCode"];
                        MemDetails1.Rows[j]["TotalPieces"] = dsawb.Tables[0].Rows[j]["TotalPieces"];
                        MemDetails1.Rows[j]["TotalWeight"] = dsawb.Tables[0].Rows[j]["TotalWeight"];
                        MemDetails1.Rows[j]["Description"] = dsawb.Tables[0].Rows[j]["description"];
                        MemDetails1.Rows[j]["ProductType"] = dsawb.Tables[0].Rows[j]["ProductType"];

                    }
                }

                Session["FlightPlanning_fltpln"] = MemDetails1;

                try
                {
                    #region Report

                    if (MemDetails1.Columns.Contains("Remark") == false)
                    {
                        DataColumn col1 = new DataColumn("Remark", typeof(string));
                        col1.DefaultValue = "";
                        MemDetails1.Columns.Add(col1);
                    }

                    if (MemDetails1.Rows.Count < 1)
                    {
                        lblStatus.Text = "No data found";
                    }
                    else
                    {
                        ReportViewer rptLoadPlanReport = new ReportViewer();
                        ReportDataSource rds1 = new ReportDataSource();
                        rptLoadPlanReport.ProcessingMode = ProcessingMode.Local;

                        LocalReport rep1 = rptLoadPlanReport.LocalReport;

                        rep1.ReportPath = Server.MapPath("/Reports/rptFltPln.rdlc");

                        rds1.Name = "dsfltpln_dtfltpln";
                        // rds1.Value = dtTable1; //ULD Section Table
                        rds1.Value = MemDetails1;
                        rep1.DataSources.Add(rds1);

                        rptLoadPlanReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                        #region Render to PDF
                        try
                        {
                            
                            string deviceInfo = "<DeviceInfo><PageHeight>40cm</PageHeight><PageWidth>50cm</PageWidth></DeviceInfo>";

                            Warning[] warnings;
                            string[] streamIds;
                            string mimeType; //= string.Empty;
                            string encoding;//= string.Empty;
                            string extension;//= string.Empty;

                            //Render the report
                            // send it to the client to download
                            byte[] bytes = rptLoadPlanReport.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                            Response.Buffer = true;
                            Response.Clear();
                            Response.ContentType = mimeType;
                            Response.AddHeader("content-disposition", "attachment; filename=" + "PrintFlightPlan" + "." + ".pdf");
                            Response.BinaryWrite(bytes); // create the file
                            Response.Flush();

                            warnings = null;
                            streamIds = null;
                            bytes = null;
                        }
                        catch (Exception)
                        {
                        }
                        #endregion
                    }
                    #endregion
                }
                catch (Exception)
                { }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dsawb != null)
                    dsawb.Dispose();
                if (MemDetails1 != null)
                    MemDetails1.Dispose();
            }
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~\\GHA_FlightPlanning.aspx",false);
        }

        #region Button Delete ULD
        protected void btnDeleteULD_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GetFlightPlanningStatus())
                    return;

                bool isMatchingAWBFound = false;
                bool isSelected = true;
                bool res = false;
                foreach (GridViewRow gvr in grdULDList.Rows)
                {
                    if (((CheckBox)gvr.FindControl("rdULDList")).Checked)
                    {
                        isMatchingAWBFound = false;
                        isSelected = true;
                        for (int i = 0; i < grdBulkAssignedAWB.Rows.Count; i++)
                        {
                            if (((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblOldULDNo")).Text ==
                                ((Label)gvr.FindControl("lblOldULDNo")).Text && ((Label)gvr.FindControl("lblOldULDNo")).Text != "")
                            {
                                isMatchingAWBFound = true;
                                res = DeleteCart(i);

                                #region Saving HAWBDetails After Unassign
                                //try
                                //{
                                //    if (Session["GHA_FlightPlanning_HAWBAddToULD"] != null)
                                //    {
                                //        foreach (GridViewRow grow in ((GridView)grdBulkAssignedAWB.Rows[i].FindControl("GVSubHAWB")).Rows)
                                //        {
                                //            if (dtULDDetails.Rows[i]["AWBno"].ToString() == ((Label)grow.FindControl("lblAWBNo")).Text)
                                //            {
                                //                objBuildULD.DeleteULDAndAWBDetails(dtULDDetails.Rows[i]["ULDno"].ToString(), ((Label)grow.FindControl("lblHAWBNo")).Text, int.Parse(((Label)grow.FindControl("lblBuiltPcs")).Text), float.Parse(((Label)grow.FindControl("lblBuiltWt")).Text), FLTno, FlightDate, dtULDDetails.Rows[i]["Origin"].ToString(),
                                //                    dtULDDetails.Rows[i]["Dest"].ToString(), "D", ref strResult);
                                //            }
                                //        }
                                //    }
                                //}
                                //catch (Exception ex)
                                //{ }
                                #endregion
                            }
                        }
                        if (!isMatchingAWBFound)
                        {
                            res = DeleteCart(((Label)gvr.FindControl("lblOldULDNo")).Text, true);
                        }
                    }
                }

                if (res == true)
                {
                    BtnList_Click(null, null);
                    lblStatus.Text = "ULD deleted and AWBs unassigned successfully.";
                    lblStatus.ForeColor = Color.Green;
                    return;
                }
                else
                {
                    if (!isSelected)
                    {
                        lblStatus.Text = "Please select row(s) to delete.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        BtnList_Click(null, null);
                        lblStatus.Text = "ULD could not be deleted. Please try again.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = ex.Message;
                return;
            }
        }
        #endregion

        protected void rdCartList_CheckedChanged(Object sender, System.EventArgs e)
        {
            hfULDNumber.Value = "";
            lblStatus.Text = "";
            lblULDStatus.Text = "";
            int index = -1;
            try
            {   //If row index is received as sender.
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                index = gvRow.RowIndex;
            }
            catch (Exception)
            {
            }

            if (index < 0)
            {   //Function called from btnSaveCart_Click.
                index = (int)sender;
            }

            for (int i = 0; i < grdCartList.Rows.Count; i++)
            {
                if (i != index)
                {
                    ((CheckBox)grdCartList.Rows[i].FindControl("rdCartList")).Checked = false;
                }
            }

            string CartNumber = "";
            CartNumber = ((Label)grdCartList.Rows[index].FindControl("lblOldCartNo")).Text;

            string flightID;
            if (txtFlightID.Text.Trim() != "")
                flightID = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
            else
                flightID = "";

            string FltDate = TextBoxdate.Text;

            HideULDAWBDetails();

            for (int i = 0; i < grdULDList.Rows.Count; i++)
            {
                ((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked = false;
            }

            BindAWBManifestCart(CartNumber, lblDepAirport.Text, flightID, FltDate);

            ShowULDAWBSummary("");

            CheckFlightStatus(Convert.ToString(Session["FlightPlan_FlightStatus"]));
            lblStatus.Text = "";
            lblULDStatus.Text = "";
        }

        protected void HideULDAWBDetails()
        {
            pnlBulkAssignedAWB.Visible = true;
            Pnlgrd.Visible = false;
            btnSave.Visible = false;
            btnUnassign.Visible = false;
            btnSaveBulkAWB.Visible = true;
            btnUnAssignBulkAWB.Visible = true;
            lbluld1.Text = "Assigned AWBs";
        }

        protected void HideCartAWBDetails()
        {
        }

        #region Bind Bulk Assigned AWB from cart
        protected void BindAWBManifestCart(string CartNumber, string POL, string FLTNo, string FlightDate)
        {
            DataSet dsbulkawbData = new DataSet("FltPlan_BindAWBManifestCart_dsbulkawbData");
            try
            {
                dsbulkawbData = objBuildULD.GetBulkAssignedAWBDetailsFromCart(lblDepAirport.Text, FLTNo, FlightDate, "", "", CartNumber);

                if (dsbulkawbData.Tables.Count > 0)
                {
                    if (dsbulkawbData.Tables[0].Rows.Count > 0)
                    {
                        tabAssignedBulkAWBDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];
                        tabAssignedBulkAWBDetails.Rows.Clear();

                        for (int i = 0; i < dsbulkawbData.Tables[0].Rows.Count; i++)
                        {
                            DataRow l_Datarow = tabAssignedBulkAWBDetails.NewRow();

                            l_Datarow["AWBno"] = dsbulkawbData.Tables[0].Rows[i]["AWBNo"].ToString();
                            l_Datarow["ULDno"] = dsbulkawbData.Tables[0].Rows[i]["Uldno"].ToString();
                            l_Datarow["TotalPcs"] = dsbulkawbData.Tables[0].Rows[i]["TotalPcs"].ToString();
                            l_Datarow["TotalWt"] = dsbulkawbData.Tables[0].Rows[i]["TotalWt"].ToString();
                            l_Datarow["AcceptedPcs"] = dsbulkawbData.Tables[0].Rows[i]["AcceptedPcs"].ToString();
                            l_Datarow["AcceptedWt"] = dsbulkawbData.Tables[0].Rows[i]["AcceptedWt"].ToString();
                            l_Datarow["BuiltPcs"] = dsbulkawbData.Tables[0].Rows[i]["pieces"].ToString();
                            l_Datarow["BuiltWt"] = dsbulkawbData.Tables[0].Rows[i]["GWeight"].ToString();
                            l_Datarow["Origin"] = dsbulkawbData.Tables[0].Rows[i]["ULDOrigin"].ToString(); ;
                            l_Datarow["Dest"] = dsbulkawbData.Tables[0].Rows[i]["ULDDest"].ToString();
                            l_Datarow["Location"] = dsbulkawbData.Tables[0].Rows[i]["Location"].ToString();
                            l_Datarow["AWBLoadingPriority"] = dsbulkawbData.Tables[0].Rows[i]["LoadingPriority"].ToString();
                            l_Datarow["AWBBuilderName"] = dsbulkawbData.Tables[0].Rows[i]["AWBBuilderName"].ToString();
                            l_Datarow["FlightExists"] = dsbulkawbData.Tables[0].Rows[i]["FlightExists"].ToString();
                            l_Datarow["CartNumber"] = dsbulkawbData.Tables[0].Rows[i]["CartNo"].ToString();
                            l_Datarow["FltNo"] = dsbulkawbData.Tables[0].Rows[i]["FltNo"].ToString();
                            l_Datarow["FltDate"] = dsbulkawbData.Tables[0].Rows[i]["FltDate"].ToString();
                            l_Datarow["ProductType"] = dsbulkawbData.Tables[0].Rows[i]["ProductType"].ToString();
                            l_Datarow["Description"] = dsbulkawbData.Tables[0].Rows[i]["Description"].ToString();
                            l_Datarow["CommodityCode"] = dsbulkawbData.Tables[0].Rows[i]["CommodityCode"].ToString();
                            l_Datarow["SHC"] = dsbulkawbData.Tables[0].Rows[i]["SHC"].ToString();

                            tabAssignedBulkAWBDetails.Rows.Add(l_Datarow);

                            if (tabAssignedBulkAWBDetails != null && tabAssignedBulkAWBDetails.Rows.Count > 0)
                            {
                                Session["FlightPlanning_ManifestBulkAWBData"] = tabAssignedBulkAWBDetails;
                            }

                            grdBulkAssignedAWB.Visible = true;
                            grdBulkAssignedAWB.DataSource = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];
                            grdBulkAssignedAWB.DataBind();

                            Session["FlightPlanning_GDVBULKAWBDetails"] = tabAssignedBulkAWBDetails;
                        }

                    }
                    else
                    {
                        grdBulkAssignedAWB.DataSource = null;
                        grdBulkAssignedAWB.DataBind();

                        tabAssignedBulkAWBDetails = (DataTable)Session["FlightPlanning_ManifestBulkAWBData"];
                        tabAssignedBulkAWBDetails.Rows.Clear();
                    }

                    if (dsbulkawbData.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsbulkawbData.Tables[0].Rows.Count; i++)
                        {
                            ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("txtAWBLocation")).Text = dsbulkawbData.Tables[0].Rows[i]["Location"].ToString();
                            ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("txtAWBLoadingPriority")).Text = dsbulkawbData.Tables[0].Rows[i]["LoadingPriority"].ToString();
                            ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("txtAWBBuilderName")).Text = dsbulkawbData.Tables[0].Rows[i]["AWBBuilderName"].ToString();
                            if (((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("lblULDNo")).Text == "")
                            {
                                ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("lblULDNo")).Enabled = false;
                            }
                            else
                            {
                                ((TextBox)grdBulkAssignedAWB.Rows[i].FindControl("lblCartNo")).Enabled = false;
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (dsbulkawbData != null)
                    dsbulkawbData.Dispose();
                if (tabMenifestDetails != null)
                    tabMenifestDetails.Dispose();
            }
        }
        #endregion Bind Bulk Assigned AWB from cart

        protected void btnNewCart_Click(object sender, EventArgs e)
        {
            if (!GetFlightPlanningStatus())
                return;

            //remove previous data from session of AWBs assigned to Cart
            ((DataTable)Session["FlightPlanning_ManifestBulkAWBData"]).Rows.Clear();

            DataTable dtNewCartList = new DataTable("FltPlan_btnNewCart_dtNewCartList");
            dtNewCartList = (DataTable)Session["FlightPlanning_CartList"];
            DataSet dsNewCart = new DataSet("FltPlan_btnNewCart_dsNewCart");

            try
            {
                lblStatus.Text = "";
                lblULDStatus.Text = "";
                string flightID;
                if (txtFlightID.Text.Trim() != "")
                    flightID = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                else
                    flightID = "";

                dsNewCart = objBuildULD.GetAWBLoadPlanULDDetails(lblDepAirport.Text, flightID, TextBoxdate.Text,
                    txtAWBPrefix.Text.Trim(), txtAWBNo.Text.Trim(), "");

                DataRow l_Datarow = dtNewCartList.NewRow();
                l_Datarow["Cartno"] = "";
                l_Datarow["LoadingPriority"] = "";
                l_Datarow["CartStatus"] = "Empty";
                l_Datarow["CartBuilderName"] = "";
                l_Datarow["ScaleWeight"] = "0";
                l_Datarow["BulkWeight"] = "0";

                dtNewCartList.Rows.Add(l_Datarow);

                grdCartList.DataSource = dtNewCartList;
                grdCartList.DataBind();
                Session["FlightPlanning_CartList"] = dtNewCartList;

                if (dtNewCartList != null)
                {
                    if (dtNewCartList.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtNewCartList.Rows.Count; i++)
                        {
                            if (dtNewCartList.Rows[i][0].ToString() != "")
                            {
                                ((Label)grdCartList.Rows[i].FindControl("lblOldCartNo")).Text = dtNewCartList.Rows[i]["Cartno"].ToString();
                                //((TextBox)grdCartList.Rows[i].FindControl("txtCartNo")).Enabled = false;
                                ((TextBox)grdCartList.Rows[i].FindControl("txtCartLoadingPriority")).Text = dtNewCartList.Rows[i]["LoadingPriority"].ToString();
                                ((TextBox)grdCartList.Rows[i].FindControl("txtCartBuilderName")).Text = dtNewCartList.Rows[i]["CartBuilderName"].ToString();
                                ((TextBox)grdCartList.Rows[i].FindControl("txtScaleWeight")).Text = dtNewCartList.Rows[i]["ScaleWeight"].ToString();
                            }
                            else
                            {
                                ((Label)grdCartList.Rows[i].FindControl("lblOldCartNo")).Text = dtNewCartList.Rows[i]["Cartno"].ToString();
                                //((TextBox)grdCartList.Rows[i].FindControl("txtCartNo")).Enabled = true;
                                ((TextBox)grdCartList.Rows[i].FindControl("txtCartLoadingPriority")).Text = dtNewCartList.Rows[i]["LoadingPriority"].ToString();
                                ((TextBox)grdCartList.Rows[i].FindControl("txtCartBuilderName")).Text = dtNewCartList.Rows[i]["CartBuilderName"].ToString();
                                ((TextBox)grdCartList.Rows[i].FindControl("txtScaleWeight")).Text = dtNewCartList.Rows[i]["ScaleWeight"].ToString();
                            }
                        }
                        ((CheckBox)grdCartList.Rows[dsNewCart.Tables[3].Rows.Count].FindControl("rdCartList")).Checked = true;

                        //uncheck all radio buttons of ULD list when new cart created
                        for (int k = 0; k < grdULDList.Rows.Count; k++)
                        {
                            ((CheckBox)grdULDList.Rows[k].FindControl("rdULDList")).Checked = false;
                        }
                    }
                }

                //To show blank manifest grid when new ULD row is added and checked.
                HideULDAWBDetails();

                ShowULDAWBSummary("");

            }
            catch { }
            finally
            {
                if (dtNewCartList != null)
                    dtNewCartList.Dispose();
                if (dsNewCart != null)
                    dsNewCart.Dispose();
            }
        }

        protected void btnSaveCart_Click(object sender, EventArgs e)
        {
            if (!GetFlightPlanningStatus())
                return;

            try
            {
                Session["FltPlan_SelectCartNo"] = null;
                bool res = false;
                DataSet dsBlankCartResult = new DataSet("FltPlan_btnSaveCart_dsBlankCartResult");
                string CartNumber = "", CartLoadingPriority = "", CartBuilderName = "", NewCartNo = "";
                double ScaleWeight = 0.0;
                string FLTno = "", Updatedby, POL = "", FltDate;
                string Updatedon = "";
                for (int i = 0; i < grdCartList.Rows.Count; i++)
                {
                    if (((CheckBox)grdCartList.Rows[i].FindControl("rdCartList")).Checked == true)
                    {
                        CartNumber = ((Label)grdCartList.Rows[i].FindControl("lblOldCartNo")).Text;
                        CartLoadingPriority = ((TextBox)grdCartList.Rows[i].FindControl("txtCartLoadingPriority")).Text;
                        CartBuilderName = ((TextBox)grdCartList.Rows[i].FindControl("txtCartBuilderName")).Text;
                        ScaleWeight = Convert.ToDouble(((TextBox)grdCartList.Rows[i].FindControl("txtScaleWeight")).Text);
                        Session["FltPlan_SelectCartNo"] = CartNumber;
                        NewCartNo = ((TextBox)grdCartList.Rows[i].FindControl("txtCartNo")).Text;

                        if (NewCartNo == "")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter Cart number";
                            return;
                        }

                        if (txtFlightID.Text.Trim() != "")
                            FLTno = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                        else
                            FLTno = "";

                        if (TextBoxdate.Text.Trim() == "")
                            FltDate = "";
                        else
                            FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

                        POL = Session["Station"].ToString();
                        Updatedby = Convert.ToString(Session["Username"]);
                        Updatedon = Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd HH:mm:ss");

                        #region Check if Cart is associated with flight which is not departed
                        string ValidCart = objBuildULD.checkCartValidForSave(CartNumber, FLTno, FltDate, POL);
                        if (ValidCart == "INVALID")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Invalid Cart";
                            return;
                        }
                        else if (ValidCart == "NOT DEPARTED")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Cart associated with non departed flight";
                            return;
                        }
                        else if (ValidCart == "DIFFERENT FLIGHT")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Cart associated with another flight";
                            return;
                        }
                        #endregion Check if Cart is associated with flight which is not departed

                        string strResult = string.Empty;

                        dsBlankCartResult = objBuildULD.AddCartDetails(CartNumber, FLTno, FltDate, POL, Updatedon, Updatedby,
                            CartLoadingPriority, CartBuilderName, ScaleWeight, ref strResult, NewCartNo);

                        if (dsBlankCartResult != null)
                        {
                            if (dsBlankCartResult.Tables != null)
                            {
                                if (dsBlankCartResult.Tables.Count > 0)
                                {
                                    if (dsBlankCartResult.Tables[0].Rows[0][0].ToString() == "Cart saved successfully")
                                    {
                                        res = true;
                                    }
                                    else
                                    {
                                        res = false;
                                        lblStatus.ForeColor = Color.Red;
                                        lblStatus.Text = "Cart not saved";
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                if (res == true)
                {
                    BtnList_Click(sender, e);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "Cart Saved Successfully";
                    return;
                }
            }
            catch (Exception)
            {
            }
        }

        protected void btnDeleteCart_Click(object sender, EventArgs e)
        {
            if (!GetFlightPlanningStatus())
                return;

            DataTable DTAWBDetails = new DataTable("FltPlan_btnDeleteCart_DTAWBDetails");
            DataTable dtULDDetails = new DataTable("FltPlan_btnDeleteCart_dtULDDetails");
            try
            {
                bool res = false;
                bool isSelected = false;
                bool isMatchingAWBFound = false;
                foreach (GridViewRow gvr in grdCartList.Rows)
                {
                    if (((CheckBox)gvr.FindControl("rdCartList")).Checked)
                    {
                        isSelected = true;
                        isMatchingAWBFound = false;
                        for (int i = 0; i < grdBulkAssignedAWB.Rows.Count; i++)
                        {
                            if (((Label)grdBulkAssignedAWB.Rows[i].FindControl("lblOldCartNo")).Text ==
                                ((Label)gvr.FindControl("lblOldCartNo")).Text && ((Label)gvr.FindControl("lblOldCartNo")).Text != "")
                            {
                                isMatchingAWBFound = true;
                                res = DeleteCart(i);
                            }
                        }
                        if (!isMatchingAWBFound)
                        {
                            res = DeleteCart(((Label)gvr.FindControl("lblOldCartNo")).Text, false);
                        }
                    }
                }

                if (res == true)
                {
                    BtnList_Click(null, null);
                    lblStatus.Text = "Cart deleted and AWBs unassigned successfully.";
                    lblStatus.ForeColor = Color.Green;
                    return;
                }
                else
                {
                    if (!isSelected)
                    {
                        lblStatus.Text = "Please select row(s) to delete.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        BtnList_Click(null, null);
                        lblStatus.Text = "Cart could not be deleted. Please try again.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                return;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (DTAWBDetails != null)
                    DTAWBDetails.Dispose();
                if (dtULDDetails != null)
                    dtULDDetails.Dispose();
            }
        }

        protected void btnReAssignULD_Click(object sender, EventArgs e)
        {
            if (!GetFlightPlanningStatus())
                return;

            DataTable dt = new DataTable("FltPlan_btnReassignULD_dt");
            DataTable DTAWBDetails = new DataTable("FltPlan_btnReassignULD_DTAWBDetails");
            DataTable dt1 = new DataTable("FltPlan_btnReassignULD_dt1");
            DataTable dtCurrentTable = new DataTable("FltPlan_btnReassignULD_dtCurrentTable");
            Button btn = (Button)sender;
            try
            {
                lblStatus.Text = "";
                lblULDStatus.Text = "";
                ddlReason.Visible = true;

                int AWBcheck = 0, ULDCheck = 0, CARTCheck = 0;

                if (btn.CommandArgument.ToString() == "ULD")
                {
                    for (int i = 0; i < grdULDList.Rows.Count; i++)
                    {
                        if (((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
                        {
                            hfULDNumber.Value = ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text;
                            hfCartNumber.Value = "";
                            ULDCheck = ULDCheck + 1;
                        }
                    }

                    if (ULDCheck == 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please select ULD for Re-Assign";
                        return;
                    }
                }
                else if (btn.CommandArgument.ToString() == "CART")
                {
                    for (int i = 0; i < grdCartList.Rows.Count; i++)
                    {
                        if (((CheckBox)grdCartList.Rows[i].FindControl("rdCartList")).Checked == true)
                        {
                            hfCartNumber.Value = ((Label)grdCartList.Rows[i].FindControl("lblOldCartNo")).Text;
                            hfULDNumber.Value = "";
                            CARTCheck = CARTCheck + 1;
                        }
                    }

                    if (CARTCheck == 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please select Cart for Re-Assign";
                        return;
                    }
                }

                LoadAirlineCode("");
                grdRouting.Rows[0].Cells[7].Enabled = false;
                grdRouting.Rows[0].Cells[8].Enabled = false;
                //Added by Poorna
                Session["FlightPlanning_Split"] = "R";
                btnAddManifest.Text = "Re-Assign";

                lblReason.Visible = true;
                
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

                #region AWB reassign
                if (AWBcheck > 0)
                {
                    for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                    {
                        if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
                        {
                            DataRow dr;
                            string POL = lblDepAirport.Text;
                            string POU = "";
                            if (Session["FlightPlanning_dsLoad"] != null)
                            {
                                DataSet dsLoad = (DataSet)Session["FlightPlanning_dsLoad"];
                                if (dsLoad.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsLoad.Tables[0].Rows.Count; k++)
                                    {
                                        string awbno = dsLoad.Tables[0].Rows[k]["AWBNumber"].ToString();
                                        if (awbno == ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text.Trim())
                                        {
                                            POU = dsLoad.Tables[0].Rows[k]["DestinationCode"].ToString();
                                            break;
                                        }
                                    }
                                }
                                if (dsLoad != null)
                                {
                                    dsLoad.Dispose();
                                }
                            }

                            // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                            dr = DTAWBDetails.NewRow();
                            dr[0] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text;
                            dr[1] = ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text;
                            dr[2] = ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text;
                            dr[3] = ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingPcs")).Text;
                            dr[4] = ((TextBox)gdvULDLoadPlanAWB.Rows[j].FindControl("txtRemainingWt")).Text;
                            dr[5] = POL;
                            dr[6] = POU;
                            dr[7] = "";
                            dr[8] = "";
                            DTAWBDetails.Rows.Add(dr);
                            break;
                        }
                    }
                }

                #endregion AWB reassign

                #region ULD reassign
                else if (ULDCheck > 0)
                {
                    for (int i = 0; i < grdULDList.Rows.Count; i++)
                    {
                        if (((CheckBox)grdULDList.Rows[i].FindControl("rdULDList")).Checked == true)
                        {
                            DataRow dr;
                            string POL = lblDepAirport.Text;
                            string POU = Session["FlightPlanning_Dest"].ToString(); ;

                            for (int j = 0; j < grdBulkAssignedAWB.Rows.Count; j++)
                            {
                                if (((Label)grdBulkAssignedAWB.Rows[j].FindControl("lblOldULDNo")).Text
                                        == ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text)
                                {
                                    // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                                    dr = DTAWBDetails.NewRow();
                                    dr[0] = ((Label)grdBulkAssignedAWB.Rows[j].FindControl("lblAWBno")).Text;
                                    dr[1] = ((Label)grdBulkAssignedAWB.Rows[j].FindControl("lblBuiltPcs")).Text;
                                    dr[2] = ((Label)grdBulkAssignedAWB.Rows[j].FindControl("lblBuiltWt")).Text;
                                    dr[3] = ((Label)grdBulkAssignedAWB.Rows[j].FindControl("lblBuiltPcs")).Text;
                                    dr[4] = ((Label)grdBulkAssignedAWB.Rows[j].FindControl("lblBuiltWt")).Text;
                                    dr[5] = POL;
                                    dr[6] = POU;
                                    dr[7] = ((Label)grdULDList.Rows[i].FindControl("lblOldULDNo")).Text;
                                    dr[8] = "";

                                    DTAWBDetails.Rows.Add(dr);

                                }
                            }
                        }
                    }

                }
                #endregion ULD reassign

                #region CART reassign
                else if (CARTCheck > 0)
                {
                    for (int i = 0; i < grdCartList.Rows.Count; i++)
                    {
                        if (((CheckBox)grdCartList.Rows[i].FindControl("rdCartList")).Checked == true)
                        {
                            DataRow dr;
                            for (int j = 0; j < grdBulkAssignedAWB.Rows.Count; j++)
                            {
                                string POL = lblDepAirport.Text;
                                string POU = Session["FlightPlanning_Dest"].ToString();
                                if (((Label)grdBulkAssignedAWB.Rows[j].FindControl("lblOldCartNo")).Text
                                    == ((Label)grdCartList.Rows[i].FindControl("lblOldCartNo")).Text)
                                {
                                    // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                                    dr = DTAWBDetails.NewRow();
                                    dr[0] = ((Label)grdBulkAssignedAWB.Rows[j].FindControl("lblAWBno")).Text;
                                    dr[1] = ((Label)grdBulkAssignedAWB.Rows[j].FindControl("lblBuiltPcs")).Text;
                                    dr[2] = ((Label)grdBulkAssignedAWB.Rows[j].FindControl("lblBuiltWt")).Text;
                                    dr[3] = ((Label)grdBulkAssignedAWB.Rows[j].FindControl("lblBuiltPcs")).Text;
                                    dr[4] = ((Label)grdBulkAssignedAWB.Rows[j].FindControl("lblBuiltWt")).Text;
                                    dr[5] = POL;
                                    dr[6] = POU;
                                    dr[7] = "";
                                    dr[8] = ((Label)grdCartList.Rows[i].FindControl("lblOldCartNo")).Text.Trim();

                                    DTAWBDetails.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
                #endregion CART reassign

                Session["AWBdata"] = DTAWBDetails;
                txtReason.Text = "";

                if ((DataTable)Session["AWBdata"] != null && ((DataTable)Session["AWBdata"]).Rows.Count > 0)
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
                pnlGrid.Style["TOP"] = "260px";

                try
                {
                    LoadAWBGrid();
                    dt1 = DTAWBDetails;
                    dtCurrentTable = (DataTable)Session["FlightPlanning_AWBdata"];
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

                            ViewState["CurrentTable1"] = dtCurrentTable;
                        }
                    }

                    # region Code to get Offload reasons

                    DataSet ds = new DataSet("FltPlan_btnReassignULD_ds");
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
                            ddlReason.DataTextField = "Reason";
                            ds.Tables[0].Rows.Add("Others");
                            ddlReason.DataSource = ds.Tables[0];
                            ddlReason.DataBind();
                            ddlReason.SelectedIndex = 0;

                        }
                        catch (Exception) 
                        { 
                        }
                    }
                    if (ds != null)
                        ds.Dispose();
                    #endregion

                    //Clear Route
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text = string.Empty;
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedIndex = 0;
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Clear();
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text = Convert.ToDateTime(Session["IT"]).ToString("dd/MM/yyyy");
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Clear();
                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text = string.Empty;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text = string.Empty;
                        ((TextBox)grdRouting.Rows[i].FindControl("txtLocation")).Text = string.Empty;
                    }
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

        #region AddRouteDetails
        protected void btnAddRouteDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string prevdest = "", strDate = "";

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

                DataSet dsRoutDetails = new DataSet("FltPlan_btnAddRouteDetails_dsRoutDetails");
                dsRoutDetails = ((DataSet)Session["FlightPlanning_dsRoutDetails"]).Copy();
                DataRow row = dsRoutDetails.Tables[0].NewRow();

                row["FltOrigin"] = prevdest;
                row["FltDate"] = strDate;  //dtCurrentDate.ToString("dd/MM/yyyy");
                row["Airline"] = txtFlightCode.Text.ToString();

                dsRoutDetails.Tables[0].Rows.Add(row);

                Session["FlightPlanning_dsRoutDetails"] = dsRoutDetails.Copy();
                grdRouting.DataSource = null;
                grdRouting.DataSource = dsRoutDetails.Copy();
                grdRouting.DataBind();
                
                if (dsRoutDetails != null)
                    dsRoutDetails.Dispose();

                //Validation by Vijay
                ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).ReadOnly = true;

                LoadDropDownAndCheckBoxRouteDetails();
                LoadAirlineCode("");
                
            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        }
        #endregion AddRouteDetails

        #region SaveRouteDetails
        public void SaveRouteDetails()
        {
            DataSet dsRoutDetails = new DataSet("FltPlan_SaveRouteDetails_dsRoutDetails");
            dsRoutDetails = ((DataSet)Session["FlightPlanning_dsRoutDetails"]).Clone();

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
                row["Location"] = ((TextBox)grdRouting.Rows[i].FindControl("txtLocation")).Text;
                dsRoutDetails.Tables[0].Rows.Add(row);
            }

            Session["FlightPlanning_dsRoutDetails"] = dsRoutDetails.Copy();
            if (dsRoutDetails != null)
            {
                dsRoutDetails.Dispose();
            }
        }
        #endregion SaveRouteDetails

        #region LoadDropdown
        public void LoadDropDownAndCheckBoxRouteDetails()
        {
            try
            {
                DataSet dsRouteDetails = new DataSet("FltPlan_LoadDropDownAndCheckBoxRouteDetails_dsRoutDetails");
                dsRouteDetails = (DataSet)Session["FlightPlanning_dsRoutDetails"];
                for (int i = 0; i < dsRouteDetails.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["PartnerType"].ToString().Trim()));
                        DropDownList routedroplist = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType"));
                        routedroplist.Text = dsRouteDetails.Tables[0].Rows[i]["PartnerType"].ToString().Trim();
                    }
                    catch (Exception) 
                    { 
                    }

                    try
                    {
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["Airline"].ToString().Trim()));
                        DropDownList routedroplist = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner"));
                        routedroplist.Text = dsRouteDetails.Tables[0].Rows[i]["Airline"].ToString().Trim();

                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Clear();
                        ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["FltNumber"].ToString().Trim(), dsRouteDetails.Tables[0].Rows[i]["FltTime"].ToString().Trim()));

                    }
                    catch (Exception) 
                    { 
                    }
                }
                if (dsRouteDetails != null)
                    dsRouteDetails.Dispose();

            }
            catch (Exception ex)
            {
                LBLRouteStatus.Text = "" + ex.Message;
            }

        }
        #endregion LoadDropdown

        #region DeleteRow
        protected void btnDeleteRoute_Click(object sender, EventArgs e)
        {
            try
            {
                SaveRouteDetails();

                DataSet dsRouteDetailsTemp = new DataSet("FltPlan_btnDeleteRoute_dsRouteDetailsTemp");
                dsRouteDetailsTemp = ((DataSet)Session["FlightPlanning_dsRoutDetails"]).Clone();

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
                        row["Location"] = ((TextBox)grdRouting.Rows[i].FindControl("txtLocation")).Text;
                        dsRouteDetailsTemp.Tables[0].Rows.Add(row);
                    }
                }

                Session["FlightPlanning_dsRoutDetails"] = dsRouteDetailsTemp.Copy();
                grdRouting.DataSource = null;
                grdRouting.DataSource = dsRouteDetailsTemp.Copy();
                grdRouting.DataBind();

                LoadDropDownAndCheckBoxRouteDetails();

                Session["FlightPlanning_Mod"] = "1";

                if (dsRouteDetailsTemp != null)
                    dsRouteDetailsTemp.Dispose();

            }
            catch (Exception ex)
            {
                LBLRouteStatus.Text = "" + ex.Message;
            }
        }
        #endregion DeleteRow

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                pnlGrid.Visible = false;
                AllButtonStatus(true);
            }
            catch (Exception)
            {
            }
        }

        protected void btnAddManifest_Click(object sender, EventArgs e)
        {
            string[] PName = new string[13];
            object[] PValue = new object[13];
            SqlDbType[] PType = new SqlDbType[13];
            string[] PName1 = new string[10];
            object[] PValue1 = new object[10];
            SqlDbType[] PType1 = new SqlDbType[10];
            try
            {
                Label1.Text = "";

                string strMode = Convert.ToString(Session["FlightPlanning_Split"]);
                string FltDate = ((TextBox)grdRouting.Rows[0].FindControl("txtFdate")).Text.Trim();

                if (FltDate == "")
                    FltDate = "01/01/1900";

                //AWB Reassign
                if (((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text != "")
                {
                    for (int j = 0; j < grdBulkAssignedAWB.Rows.Count; j++)
                    {
                        if (((CheckBox)grdBulkAssignedAWB.Rows[j].FindControl("Check0")).Checked == true)
                        {
                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtTotalPCS")).Text))
                            {
                                Label1.Text = "Please enter valid Pcs";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txttotalweight")).Text))
                            {
                                Label1.Text = "Please enter valid Weight";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                            break;
                        }
                    }

                    if (((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text.Trim() == "")
                    {
                        Label1.Text = "Please enter valid Destination";
                        Label1.ForeColor = Color.Red;
                        ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Focus();
                        return;
                    }

                    if (((TextBox)grdRouting.Rows[0].FindControl("txtPcs")).Text.Trim() == "")
                    {
                        Label1.Text = "Please enter valid Pcs";
                        Label1.ForeColor = Color.Red;
                        ((TextBox)grdRouting.Rows[0].FindControl("txtPcs")).Focus();
                        return;
                    }
                    if (((TextBox)grdRouting.Rows[0].FindControl("txtWt")).Text.Trim() == "")
                    {
                        Label1.Text = "Please enter valid Weight";
                        Label1.ForeColor = Color.Red;
                        ((TextBox)grdRouting.Rows[0].FindControl("txtWt")).Focus();
                        return;
                    }
                    try
                    {
                        if (Convert.ToInt32(((TextBox)grdRouting.Rows[0].FindControl("txtPcs")).Text.Trim()) > Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPcs")).Text.Trim()))
                        {
                            Label1.Text = "Please enter valid Pcs";
                            Label1.ForeColor = Color.Red;
                            ((TextBox)grdRouting.Rows[0].FindControl("txtPcs")).Focus();
                            return;
                        }
                        if (Convert.ToDecimal(((TextBox)grdRouting.Rows[0].FindControl("txtWt")).Text.Trim()) > Convert.ToDecimal(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text.Trim()))
                        {
                            Label1.Text = "Please enter valid Weight";
                            Label1.ForeColor = Color.Red;
                            ((TextBox)grdRouting.Rows[0].FindControl("txtWt")).Focus();
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        Label1.Text = "Please enter valid Pcs and Weight";
                        Label1.ForeColor = Color.Red;
                        return;
                    }
                    if (((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).Text.Trim() == "" &&
                        ((TextBox)grdRouting.Rows[0].FindControl("txtLocation")).Text.Trim() == "")
                    {
                        Label1.Text = "Please enter valid Location";
                        Label1.ForeColor = Color.Red;
                        ((TextBox)grdRouting.Rows[0].FindControl("txtLocation")).Focus();
                        return;
                    }

                    if (txtFlightCode.Text.Trim() + txtFlightID.Text.Trim() == ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text &&
                        TextBoxdate.Text.Trim() == FltDate &&
                        ((TextBox)grdAWBs.Rows[0].FindControl("txtOrigin")).Text.Trim() == ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim() &&
                        ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim() == ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text.Trim())
                    {
                        Label1.Text = "Can not reassign to the same flight.";
                        Label1.ForeColor = Color.Red;
                        return;
                    }

                    string str1 = ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim();
                    string[] str = str1.Split('-');
                    string AWBPrefix = str[0];
                    string AWBNummber = str[1];

                    #region Parameter for AWB
                    PName[0] = "OldFlt";
                    PName[1] = "OldFltDt";
                    PName[2] = "OldOrg";
                    PName[3] = "OldDest";
                    PName[4] = "AWBNo";
                    PName[5] = "AWBPrefix";
                    PName[6] = "NewFlt";
                    PName[7] = "NewFltDt";
                    PName[8] = "NewOrg";
                    PName[9] = "NewDest";
                    PName[10] = "PCS";
                    PName[11] = "WT";
                    PName[12] = "Location";

                    PValue[0] = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                    PValue[1] = TextBoxdate.Text.Trim();

                    PValue[2] = ((TextBox)grdAWBs.Rows[0].FindControl("txtOrigin")).Text.Trim();
                    PValue[3] = ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim();
                    PValue[4] = AWBNummber;
                    PValue[5] = AWBPrefix;
                    PValue[6] = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text;
                    PValue[7] = FltDate;
                    PValue[8] = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                    PValue[9] = ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text.Trim();
                    PValue[10] = ((TextBox)grdRouting.Rows[0].FindControl("txtPcs")).Text.Trim();
                    PValue[11] = ((TextBox)grdRouting.Rows[0].FindControl("txtWt")).Text.Trim();
                    PValue[12] = ((TextBox)grdRouting.Rows[0].FindControl("txtLocation")).Text.Trim();

                    PType[0] = SqlDbType.VarChar;
                    PType[1] = SqlDbType.VarChar;
                    PType[2] = SqlDbType.VarChar;
                    PType[3] = SqlDbType.VarChar;
                    PType[4] = SqlDbType.VarChar;
                    PType[5] = SqlDbType.VarChar;
                    PType[6] = SqlDbType.VarChar;
                    PType[7] = SqlDbType.VarChar;
                    PType[8] = SqlDbType.VarChar;
                    PType[9] = SqlDbType.VarChar;
                    PType[10] = SqlDbType.Int;
                    PType[11] = SqlDbType.Decimal;
                    PType[12] = SqlDbType.VarChar;
                    #endregion

                    DataSet dsin = new DataSet("FltPlan_btnAddManifest_dsin");
                    dsin = da.SelectRecords("SPAddReassign", PName, PValue, PType);

                    if (dsin != null && dsin.Tables.Count > 0 && dsin.Tables[0].Rows.Count > 0)
                    {
                        if (dsin.Tables[0].Rows[0][0].ToString() == "TRUE")
                        {
                            BtnList_Click(null, null);
                            pnlGrid.Visible = false;

                            lblStatus.Text = "AWB Reassigned successfully";
                            lblStatus.ForeColor = Color.Green;

                            AllButtonStatus(true);
                            return;
                        }
                        else if (dsin.Tables[0].Rows[0][0].ToString() == "EXPORTED TO MANIFEST")
                        {
                            Label1.Text = "Can not reassign to flight which is already exported to manifest";
                            Label1.ForeColor = Color.Red;
                            return;
                        }
                    }
                    if (dsin != null)
                    {
                        dsin.Dispose();
                    }
                }

                //ULD unassign 
                else if (hfULDNumber.Value != "")
                {
                    if (!GetFlightPlanningStatus())
                        return;

                    string struld = ((TextBox)grdAWBs.Rows[0].FindControl("lblULDNO")).Text.Trim();

                    if (((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text.Trim() == "" &&
                        ((TextBox)grdRouting.Rows[0].FindControl("txtLocation")).Text.Trim() == "")
                    {
                        Label1.Text = "Please enter valid Location";
                        Label1.ForeColor = Color.Red;
                        return;
                    }

                    #region Parameter for ULD
                    PName1[0] = "OldFlt";
                    PName1[1] = "OldFltDt";
                    PName1[2] = "OldOrg";
                    PName1[3] = "OldDest";
                    PName1[4] = "ULDNo";
                    //PName[5] = "AWBPrefix";
                    PName1[5] = "NewFlt";
                    PName1[6] = "NewFltDt";
                    PName1[7] = "NewOrg";
                    PName1[8] = "NewDest";
                    PName1[9] = "Location";
                    //PName1[10] = "WT";

                    PValue1[0] = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                    PValue1[1] = TextBoxdate.Text.Trim();

                    PValue1[2] = ((TextBox)grdAWBs.Rows[0].FindControl("txtOrigin")).Text.Trim();
                    PValue1[3] = ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim();
                    PValue1[4] = struld;
                    //PValue[5] = AWBPrefix;
                    PValue1[5] = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text;
                    PValue1[6] = FltDate;
                    PValue1[7] = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                    PValue1[8] = ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text.Trim();
                    PValue1[9] = ((TextBox)grdRouting.Rows[0].FindControl("txtLocation")).Text.Trim();
                    //PValue1[10] = ((TextBox)grdRouting.Rows[0].FindControl("txtWt")).Text.Trim();

                    PType1[0] = SqlDbType.VarChar;
                    PType1[1] = SqlDbType.VarChar;
                    PType1[2] = SqlDbType.VarChar;
                    PType1[3] = SqlDbType.VarChar;
                    PType1[4] = SqlDbType.VarChar;
                    //PType[5] = SqlDbType.VarChar;
                    PType1[5] = SqlDbType.VarChar;
                    PType1[6] = SqlDbType.VarChar;
                    PType1[7] = SqlDbType.VarChar;
                    PType1[8] = SqlDbType.VarChar;
                    PType1[9] = SqlDbType.VarChar;
                    //PType1[10] = SqlDbType.Decimal;
                    #endregion

                    DataSet dsin = new DataSet("FltPlan_btnAddManifest_dsin");
                    dsin = da.SelectRecords("SPAddRemoveULD", PName1, PValue1, PType1);
                    if (dsin != null && dsin.Tables.Count > 0 && dsin.Tables[0].Rows.Count > 0)
                    {
                        if (dsin.Tables[0].Rows[0][0].ToString() == "TRUE")
                        {
                            BtnList_Click(null, null);
                            pnlGrid.Visible = false;

                            lblStatus.Text = "ULD Reassigned successfully";
                            lblStatus.ForeColor = Color.Green;

                            AllButtonStatus(true);
                            return;
                        }
                    }
                    if (dsin != null)
                    {
                        dsin.Dispose();
                    }
                }

                //CART unassign    
                else if (hfCartNumber.Value != "")
                {
                    if (!GetFlightPlanningStatus())
                        return;

                    string strcart = ((Label)grdAWBs.Rows[0].FindControl("lblULDNO")).Text.Trim();

                    if (((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text.Trim() == "" &&
                        ((TextBox)grdRouting.Rows[0].FindControl("txtLocation")).Text.Trim() == "")
                    {
                        Label1.Text = "Please enter valid Location";
                        Label1.ForeColor = Color.Red;
                        return;
                    }

                    #region Parameter for CART
                    PName1[0] = "OldFlt";
                    PName1[1] = "OldFltDt";
                    PName1[2] = "OldOrg";
                    PName1[3] = "OldDest";
                    PName1[4] = "CartNo";
                    //PName[5] = "AWBPrefix";
                    PName1[5] = "NewFlt";
                    PName1[6] = "NewFltDt";
                    PName1[7] = "NewOrg";
                    PName1[8] = "NewDest";
                    PName1[9] = "Location";
                    //PName1[10] = "WT";

                    PValue1[0] = txtFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                    PValue1[1] = TextBoxdate.Text.Trim();

                    PValue1[2] = ((TextBox)grdAWBs.Rows[0].FindControl("txtOrigin")).Text.Trim();
                    PValue1[3] = ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim();
                    PValue1[4] = strcart;
                    //PValue[5] = AWBPrefix;
                    PValue1[5] = ((DropDownList)grdRouting.Rows[0].FindControl("ddlFltNum")).SelectedItem.Text;
                    PValue1[6] = FltDate;
                    PValue1[7] = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                    PValue1[8] = ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text.Trim();
                    PValue1[9] = ((TextBox)grdRouting.Rows[0].FindControl("txtLocation")).Text.Trim();
                    //PValue1[10] = ((TextBox)grdRouting.Rows[0].FindControl("txtWt")).Text.Trim();

                    PType1[0] = SqlDbType.VarChar;
                    PType1[1] = SqlDbType.VarChar;
                    PType1[2] = SqlDbType.VarChar;
                    PType1[3] = SqlDbType.VarChar;
                    PType1[4] = SqlDbType.VarChar;
                    //PType[5] = SqlDbType.VarChar;
                    PType1[5] = SqlDbType.VarChar;
                    PType1[6] = SqlDbType.VarChar;
                    PType1[7] = SqlDbType.VarChar;
                    PType1[8] = SqlDbType.VarChar;
                    PType1[9] = SqlDbType.VarChar;
                    //PType1[10] = SqlDbType.Decimal;
                    #endregion

                    DataSet dsin = new DataSet("FltPlan_btnAddManifest_dsin");
                    dsin = da.SelectRecords("SPAddRemoveCart", PName1, PValue1, PType1);

                    if (dsin != null && dsin.Tables.Count > 0 && dsin.Tables[0].Rows.Count > 0)
                    {
                        if (dsin.Tables[0].Rows[0][0].ToString() == "TRUE")
                        {
                            BtnList_Click(null, null);
                            pnlGrid.Visible = false;

                            lblStatus.Text = "Cart Reassigned successfully";
                            lblStatus.ForeColor = Color.Green;

                            AllButtonStatus(true);
                            return;
                        }
                    }
                    if (dsin != null)
                    {
                        dsin.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                PName = null;
                PValue = null;
                PType = null;
                PName1 = null;
                PValue1 = null;
                PType1 = null;
            }
        }

        protected bool ValidateExportToManiOnSave()
        {
            for (int i = 0; i < grdCartList.Rows.Count; i++)
            {
                if (Convert.ToDouble(((TextBox)grdCartList.Rows[i].FindControl("txtScaleWeight")).Text) == 00.0)
                {
                    lblStatus.Text = "Please enter Cart weight.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return false;
                }
            }
            for (int i = 0; i < grdULDList.Rows.Count; i++)
            {
                if (Convert.ToDouble(((TextBox)grdULDList.Rows[i].FindControl("txtScaleWeight")).Text) == 00.0)
                {
                    lblStatus.Text = "Please enter ULD weight.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return false;
                }
            }
            return true;
        }

        #region Validate HAWB child Grid on row creation
        protected void gdvULDLoadPlanAWB_RowCreated(object sender, GridViewRowEventArgs e)
        {
            #region Validate HAWB Child Grid icon
            try
            {
                string ValidateHAWBGrid = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ValidateChildHAWBGridIcon");
                bool Validate = ValidateHAWBGrid != string.Empty ? Convert.ToBoolean(ValidateHAWBGrid) : true;
                e.Row.Cells[0].Visible = Validate;
            }
            catch (Exception)
            { }
            #endregion
        }

        protected void grdBulkAssignedAWB_RowCreated(object sender, GridViewRowEventArgs e)
        {
            #region Validate HAWB Child Grid icon
            try
            {
                string ValidateHAWBGrid = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ValidateChildHAWBGridIcon");
                bool Validate = ValidateHAWBGrid != string.Empty ? Convert.ToBoolean(ValidateHAWBGrid) : true;
                e.Row.Cells[0].Visible = Validate;
            }
            catch (Exception ex)
            { }
            #endregion
        }

        protected void gdvULDDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {
            #region Validate HAWB Child Grid icon
            try
            {
                string ValidateHAWBGrid = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ValidateChildHAWBGridIcon");
                bool Validate = ValidateHAWBGrid != string.Empty ? Convert.ToBoolean(ValidateHAWBGrid) : true;
                e.Row.Cells[0].Visible = Validate;
            }
            catch (Exception ex)
            { }
            #endregion
        }
        #endregion

        #region btnFltRoute_Click
        protected void btnFltRoute_Click(object sender, EventArgs e)
        {
            Session["FlightPlanning_ShowDest"] = 1;
            BtnList_Click(this, new EventArgs());
            btnReAssign.Enabled = false;
        }
        #endregion btnFltRoute_Click

        protected void btnFlightNumber_Click(object sender, EventArgs e)
        {
            Session["FlightPlanning_ShowDest"] = 0;
            BtnList_Click(this, new EventArgs());
            btnReAssign.Enabled = true;
        }

        protected void btnFetchAWB_Click(object sender, EventArgs e)
        {
            DataSet dsFetchAWB = new DataSet("FltPlan_btnFetchAWB_dsFetchAWB");
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
                if (!DateTime.TryParseExact(TextBoxdate.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None,
                    out dtFltDate))
                {
                    lblFetchStatus.Text = "Please select or enter flight date in DD/MM/YYYY format.";
                    lblFetchStatus.ForeColor = Color.Blue;
                    TextBoxdate.Focus();
                    return;
                }

                dsFetchAWB = objBuildULD.GetAWBForReassign(txtFetchAWBPrefix.Text, txtFetchAWBNumber.Text,
                    txtFlightCode.Text + "" + txtFlightID.Text, TextBoxdate.Text, Session["Station"].ToString());

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

        protected void btnAssignFetchedAWB_Click(object sender, EventArgs e)
        {
            if (!GetFlightPlanningStatus())
                return;
            lblFetchStatus.Text = "";
            try
            {
                int intNewPcs = 0;
                if (!int.TryParse(txtFetchPcs.Text, out intNewPcs))
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
                if ((txtCartNumber.Text == "" && txtULDNumber.Text == "") ||
                    (txtCartNumber.Text != "" && txtULDNumber.Text != ""))
                {
                    lblFetchStatus.Text = "Please enter either Cart Number or ULD Number to which AWB is to be assigned.";
                    lblFetchStatus.ForeColor = Color.Red;
                    return;
                }

                DataSet dsReassigned = new DataSet("FltPlan_btnAssignFetchedAWB_dsReassigned");
                dsReassigned = objBuildULD.ReassignFetchedAWB(txtFetchAWBPrefix.Text, txtFetchAWBNumber.Text,
                    lblFetchFlt.Text, lblFetchFltDate.Text, txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
                    intOldPcs, decOldWt, intNewPcs, decNewWt, Session["Station"].ToString(),
                    Convert.ToDateTime(Session["IT"]), Session["UserName"].ToString(), "", txtCartNumber.Text,
                    txtULDNumber.Text, Session["FlightPlanning_Dest"].ToString());
                if (dsReassigned != null)
                {
                    if (dsReassigned.Tables != null && dsReassigned.Tables.Count > 0)
                    {
                        if (dsReassigned.Tables[0].Rows.Count > 0)
                        {
                            btnFetchClear_Click(null, null);
                            if (dsReassigned.Tables[0].Rows[0][0].ToString() == "")
                            {
                                BtnList_Click(null, null);
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
                if (dsReassigned != null)
                {
                    dsReassigned.Dispose();
                }
            }
            catch (Exception)
            {
            }
            lblFetchStatus.Text = "AWB could not be assigned. Please try again.";
            lblFetchStatus.ForeColor = Color.Red;
        }

       void getNavitireInfo()
        {
            try
            {
                String UserName = " ";
                if (Session["UserName"]!=null)
                {
                    UserName = Session["UserName"].ToString();
                }
                Navitire obj = new Navitire(UserName);
                NavitireParamInfo objParam = new NavitireParamInfo();

                objParam.CarrierCode      = txtFlightCode.Text.Trim();
                objParam.FlightNumber     = txtFlightID.Text.Trim();
                objParam.DepartureDate    = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
                objParam.DepartureStation = lblDepAirport.Text.Trim();
                if(!String.IsNullOrEmpty(lblRoute.Text) && lblRoute.Text.Contains('-'))
                    objParam.ArrivalStation = lblRoute.Text.Substring(lblRoute.Text.LastIndexOf('-')+1);
                GetPaxManifest x = new GetPaxManifest();
                x.PaxManifestHeaderValue = new PaxManifestHeader();
                x.PaxManifestHeaderValue.AuthUser = "paxmanifestUser";
                x.PaxManifestHeaderValue.SecurityKey = "A@th";
                //String paxXML = x.GetManifest("7/22/2014", "mnl", "ceb", "561");              
                String paxXML = x.GetManifest(DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy"), objParam.DepartureStation.ToLower(), objParam.ArrivalStation.ToLower(), objParam.FlightNumber);
                NavitireOutData objOut= obj.UpdateData(objParam, paxXML);
                lblRoute.Text = lblRoute.Text + "  AvlCap: " + objOut.UpdCarCap;
            }
            catch (Exception)
            {
            }
        }

       #region btnGetPaxCap_Click
       protected void btnGetPaxCap_Click(object sender, EventArgs e)
       {
           try
           {
               BtnList_Click(this, new EventArgs());
               if (f_FETCHNAVITIRE)
                   getNavitireInfo();
           }
           catch (Exception)
           {
           }
       }
       #endregion btnGetPaxCap_Click

        #region Get Flight Planning Status
        /// <summary>
        /// Gets result of flight availability for Planning and shows appropriate status message.
        /// </summary>
        /// <returns>True if flight is available for planning.</returns>
        private bool GetFlightPlanningStatus()
        {
           string fltStatus = "N";
           fltStatus = objBuildULD.GetFlightPlanningStatus(txtFlightCode.Text + txtFlightID.Text, TextBoxdate.Text,
               lblDepAirport.Text);
           if (fltStatus != null && fltStatus != "" && fltStatus != "N")
           {
               BtnList_Click(this, new EventArgs());
               lblStatus.ForeColor = Color.Red;
               switch (fltStatus)
               {
                   case "D":
                       lblStatus.Text = "Flight is already departed.";
                       break;
                   case "E":
                       lblStatus.Text = "Flight is already exported to manifest.";
                       break;
                   default:
                       break;
               }
               return (false);
           }
           else
               return (true);
        }
        #endregion Get Flight Planning Status

    }
}
