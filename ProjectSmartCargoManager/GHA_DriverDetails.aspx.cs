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

using System.IO;
//using DataDynamics.Reports.Rendering.Pdf;
//using DataDynamics.Reports.Rendering.IO;
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class GHA_DriverDetails : System.Web.UI.Page
    {
        #region Variables
        SQLServer da = new SQLServer(Global.GetConnectionString());
        //string Flight = "";
        ShowFlightsBAL objBAL = new ShowFlightsBAL();
        BookingBAL objBLL = new BookingBAL();
        BLExpManifest objExpMani = new BLExpManifest();
        DataTable MemDetails, tabMenifestDetails;
        DataSet dsAWBData;
        bool chkduplicate = false;
        string SelectedULDnos;

        int TotalULD = 0, totalULDAWB = 0, totalULDPCS = 0, totalAWB = 0, totalAWBPCS = 0;
        float totalAWBWt = 0, totalAWBVol = 0, totalULDVol = 0, totalULDWt = 0;
        ArrayList ULDDestpt = new ArrayList();
        EMAILOUT Em = new EMAILOUT();
        MasterBAL objMst = new MasterBAL();
        DateTime dtCurrentDate = DateTime.Now;
        //private string POLairport = "";
        private string FltNumber = "";
        //private string Fltdate = "";
        //private string FrommailId = "";
        string fblmsg;
        string ffmmsg = string.Empty;
        bool ChkSuperUserOffload = false;
        DataTable dtAWB = new DataTable();
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
                    try
                    {
                        if (Session["AirlinePrefix"] != null)
                            txtFlightPrefix.Text = Convert.ToString(Session["AirlinePrefix"]);
                        if (Session["awbPrefix"] != null)
                        {
                            txtAWBPrefix.Text = Session["awbPrefix"].ToString();

                        }
                        else
                        {
                            MasterBAL objBal = new MasterBAL();
                            Session["awbPrefix"] = objBal.awbPrefix();
                            txtAWBPrefix.Text = Session["awbPrefix"].ToString();
                        }
                        ////bool fbl = false;
                        ////bool ffm = false;
                        ClearSession();
                        hdnManifestFlag.Value = "";

                        //Session["Username"] = "bomdemo";
                        //Session["StationCode"] = "BOM";
                        
                        string UserName = Session["Username"].ToString();
                        string StationCode = Session["Station"].ToString();

                        lblDepAirport.Text = Convert.ToString(Session["Station"]);
                        /* Clear Session on form load*/

                        //Session["ULDData"] = "";
                        //Session["DataTableTemp"] = "";
                        //Session["ManifestGridData"] = "";


                        //TextBox17.Text = Convert  .ToString ( System.DateTime.Now ,"dd/MMM/yyyy HH:mm:ss");
                        //txtfltdt .Text = System.DateTime.Now.ToString ( "yyyy/MM/dd HH:mm:ss");
                        txtfltdt.Text = dtCurrentDate.ToString("dd/MM/yyyy");// System.DateTime.Now.ToString("dd/MM/yyyy");

                        //ShowULDAWBSummary();

                        #region Temp table for AWB Tab
                        DataTable MemDetails = new DataTable();

                        MemDetails.Columns.Add("ULDno");
                        MemDetails.Columns.Add("POU");
                        MemDetails.Columns.Add("TareWeight");

                        Session["DataTableTemp"] = (DataTable)MemDetails;
                        #endregion Temp table for AWB Tab


                        #region Temp table for Manifest grid
                        DataTable tabMenifestDetails = new DataTable();

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

                        DataTable tabMenifestDetailsGHA = new DataTable();
                        tabMenifestDetailsGHA.Columns.Add("AWBNumber");
                        tabMenifestDetailsGHA.Columns.Add("PCS");
                        tabMenifestDetailsGHA.Columns.Add("WT");
                        tabMenifestDetailsGHA.Columns.Add("FlightNo");
                        tabMenifestDetailsGHA.Columns.Add("FlightDt");
                        tabMenifestDetailsGHA.Columns.Add("DriverName");
                        tabMenifestDetailsGHA.Columns.Add("DLNumber");
                        tabMenifestDetailsGHA.Columns.Add("Phone");
                        tabMenifestDetailsGHA.Columns.Add("VehicleNo");
                        tabMenifestDetailsGHA.Columns.Add("TokenNumber");
                        tabMenifestDetailsGHA.Columns.Add("TokenDate");
                        tabMenifestDetailsGHA.Columns.Add("ULDNo");
                        //tabMenifestDetailsGHA = (DataTable)Session["grdulddetails"];
                        Session["grdulddetails"] = (DataTable)tabMenifestDetailsGHA;

                        Session["ManifestGridData"] = (DataTable)tabMenifestDetails;
                        #endregion Temp table for Manifest grid
                        LoadGridRoutingDetail();
                        //                        FillIrregularityCode();
                        AWBDesignators();

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>return disable();</script>", false);

                        MemDetails = new DataTable();

                        MemDetails.Columns.Add("lblAWBno");
                        MemDetails.Columns.Add("lblPieces");
                        MemDetails.Columns.Add("lblWeight");
                        MemDetails.Columns.Add("lblAvlPCS");
                        MemDetails.Columns.Add("lblAvlWgt");

                        MemDetails.Columns.Add("lblTotPCS");
                        MemDetails.Columns.Add("lblTotWgt");

                        DataRow dr = MemDetails.NewRow();
                        dr["lblAWBno"] = "58980003022";
                        dr["lblPieces"] = "10";
                        dr["lblWeight"] = "100";

                        dr["lblAvlPCS"] = "10";
                        dr["lblAvlWgt"] = "100";
                        dr["lblTotPCS"] = "10";
                        dr["lblTotWgt"] = "100";

                        MemDetails.Rows.Add(dr);
                        grdAWBs.Visible = true;

                        if (Request.QueryString["del"] == "1")
                        {
                            chkdelivery.Checked = true;
                            lblTitle.Text = "Cargo Pickup";
                            TabPanelULD.Visible = true;
                            try
                            {
                                if (Session["ULDACT"].ToString().ToUpper() == "FALSE")
                                    TabPanelULD.Visible = false;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                            lblTitle.Text = "Cargo Handover";
                            TabPanelULD.Visible = false;
                        }
                        if (chkdelivery.Checked == true)
                        {
                            btnCloseCTM.Text = "Print GatePass";
                        }

                        LoadToken();
                        //gdvULDLoadPlanAWB.DataSource = MemDetails;
                        //gdvULDLoadPlanAWB.DataBind();

                        //  txtReason.Visible = false;
                        // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowHideTextBox();</script>", false);
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Form Load


        #region Flight Clear Button  Click
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            txtFlightID.Text = "";
            txtfltdt.Text = dtCurrentDate.ToString("dd/MM/yyyy");// System.DateTime.Now.ToString("dd/MM/yyyy");
            txtFlightID.Focus();
            gdvULDLoadPlanAWB.DataSource = null;
            gdvULDLoadPlanAWB.Visible = false;
            gdvULDDetails.DataSource = null;
            gdvULDDetails.Visible = false;
            txtAWBNo.Text = "";
            txtAWBPrefix.Text = "";
            txtDLNumber.Text = "";
            txtDockNo.Text = "";
            txtDriverName.Text = "";
            txtphone.Text = "";
            ddltkn.SelectedIndex = 0;
            //Need to add the logic to clear the screen.
            //Response.Redirect("~/frmExportManifest.aspx");
            //End
        }
        #endregion Flight Clear Button  Click

        #region Load Plan List Button Click
        protected void BtnLoadPlanRefList_Click(object sender, EventArgs e)
        {
            try
            {
                FillSelectULDDDL();

                FillAWBGrid();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion Load Plan List Button Click

        #region Load Plan AWB Grid Fill

        public void FillAWBGrid()
        {
            try
            {
                string flightID = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                DateTime FlightDate = DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null);
                DataSet dsawb = new DataSet();

                //  dsawb = objExpMani.GetAwbTabdetails(ddlMainPOU.SelectedItem.Value.ToString(), flightID);//further research to be done

                dsawb = objExpMani.GetAwbTabdetails(lblDepAirport.Text, flightID, FlightDate);


                // dsawb1 = dsawb.Copy();
                if (dsawb != null)
                {
                    //gdvULDLoadPlanAWB.DataSource = dsawb;
                    //gdvULDLoadPlanAWB.DataBind();
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

        }


        #endregion Load Plan AWB Grid Fill


        public void FillAWBGridMain()
        {
            try
            {
                string flightID = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                DataSet dsawb = new DataSet();
                //DateTime dt=DateTime.Parse(txtfltdt.Text.Trim();
                DateTime FlightDate = DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null);
                //11 may addded
                dsawb = objExpMani.GetAwbTabdetails(lblDepAirport.Text, flightID, FlightDate);
                //  dsawb = objExpMani.GetAwbTabdetails(Destination.ToString(), flightID);//further research to be done
                Session["dsLoad"] = dsawb;

                DataTable MemDetails = new DataTable();

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
                //gdvULDLoadPlanAWB.DataSource = (DataTable)MemDetails;
                //gdvULDLoadPlanAWB.DataBind();
                try
                {
                    ChangeButtonStatus();

                    if (dsawb != null)
                    {
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

        }

        #region Select ULD DDl Fill
        public void FillSelectULDDDL()
        {
            try
            {
                ddlSelectULD.Items.Clear();
                string flightID = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                DataSet dsULD = new DataSet();

                dsULD = null;
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
        }

        #endregion  Select ULD DDl Fill

        #region Load Plan Clear Button Click
        protected void BtnLoadPlanRefClear_Click(object sender, EventArgs e)
        {

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



        }
        #endregion


        #region ULD Grid add rows
        protected bool addgrid(string memid, string memname)
        {
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
                    DataTable DSULDadddata = (DataTable)Session["ULDData"];

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
            return false;
        }
        #endregion

        protected void createPO()
        {
            MemDetails = new DataTable();

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


        #region Assign To ULD
        //protected void btnAssigntoULD_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        int CntAWBChk = 0;
        //        string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby;
        //        int PCS = 0, AVLPCS = 0;
        //        double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0;
        //        string Updatedon = "";


        //        //CheckBox cb = ((CheckBox)gdvULDLoadPlanAWB.Rows[0].FindControl("Check2"));

        //        for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
        //        {
        //            if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
        //            {
        //                CntAWBChk += 1;
        //            }
        //        }

        //        int CntULDChk = 0;

        //        if (ddlSelectULD.SelectedIndex > 0)
        //        {
        //            {
        //                CntULDChk += 1;
        //                if (CntAWBChk > 0)//ULD to be added to ULD tab only on association with awb.
        //                {
        //                    chkduplicate = addgrid(ddlSelectULD.SelectedValue.ToString(), ddlSelectULD.SelectedItem.Text.ToString());
        //                }
        //            }
        //        }

        //        if (CntAWBChk != 0 && CntULDChk != 0)
        //        {
        //            FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
        //            POL = Convert.ToString(Session["Station"]);
        //            //  POU = ddlMainPOU.SelectedItem.Value.ToString();
        //            POU = null;// ddlMainPOU.Items[0].Value.ToString();
        //            Updatedby = Convert.ToString(Session["Username"]);

        //            //for (int i = 0; i < gdvULDLoadPlan.Rows.Count; i++)
        //            //{
        //            //    if (((CheckBox)gdvULDLoadPlan.Rows[i].FindControl("Check1")).Checked == true)
        //            //    {
        //            //        ULDno = (gdvULDLoadPlan.Rows[i].Cells[1].Text);
        //            //        ULDwgt = Convert.ToDouble(gdvULDLoadPlan.Rows[i].Cells[3].Text);
        //            //        break;
        //            //    }
        //            //}

        //            if (Session["ULDData"] != null)
        //            {

        //                DataTable DSULDadddata = (DataTable)Session["ULDData"];

        //                for (int i = 0; i < DSULDadddata.Rows.Count; i++)
        //                {
        //                    if (DSULDadddata.Rows[i][0].ToString() == ddlSelectULD.SelectedItem.Text)
        //                    {
        //                        ULDno = (DSULDadddata.Rows[i][0].ToString());

        //                        double tareWeight = 0.0;
        //                        if (DSULDadddata.Rows[i][2].ToString() != "")
        //                            tareWeight = Convert.ToDouble(DSULDadddata.Rows[i][2].ToString());
        //                        ULDwgt = tareWeight;
        //                        break;
        //                    }
        //                }
        //            }

        //            string strResult = string.Empty;
        //            string strIdentifier = Convert.ToString(Session["Identifier"]);

        //            for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
        //            {
        //                if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
        //                {
        //                    AWBno = gdvULDLoadPlanAWB.Rows[i].Cells[1].Text;
        //                    PCS = Convert.ToInt32(gdvULDLoadPlanAWB.Rows[i].Cells[2].Text);
        //                    WGT = Convert.ToDouble(gdvULDLoadPlanAWB.Rows[i].Cells[3].Text);
        //                    AVLPCS = Convert.ToInt32(gdvULDLoadPlanAWB.Rows[i].Cells[4].Text);
        //                    AVLWGT = Convert.ToDouble(gdvULDLoadPlanAWB.Rows[i].Cells[5].Text);
        //                    Updatedon = dtCurrentDate.ToString("MM/dd/yyyy HH:mm:ss");// DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

        //                    //avoid awb-added to uld no from displaying in awb grid
        //                    bool result = objExpMani.AddULDAWBassocition(FLTno, POL, POU, ULDno, ULDwgt, AWBno, PCS, WGT, AVLPCS, AVLWGT, Updatedby, dtCurrentDate, dtCurrentDate, strIdentifier, ref strResult,"");

        //                }
        //            }

        //            // ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "ULDAWBAssocitionSuccessfull();", true);

        //            lblStatus.ForeColor = Color.Green;
        //            lblStatus.Text = "AWB Assigned to ULD Successfully";

        //            return;
        //        }
        //        else
        //        {

        //            // ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "display_alertAWBULD();", true);

        //            lblStatus.ForeColor = Color.Red;
        //            lblStatus.Text = "Please Select ULD from DDL and AWB from TAB";

        //            return;

        //        }


        //        FillAWBGrid();//used as  refresh 

        //    }
        //    catch (Exception ex)
        //    { }

        //}
        #endregion Assign To ULD


        protected void btnULDSummary1_Click(object sender, EventArgs e)
        {

        }

        #region ADD TO Manifest

        protected void BtnAddtoManifest_Click(object sender, EventArgs e)
        {
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


                if (CntULDChk != 0 && CntAWBChk == 0)//to fetch data for checked ULDs and add data to grid 
                {
                    AddULDDatarowsToManifestGrid();
                    //for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
                    //{
                    //    if (((CheckBox)gdvULDLoadPlan.Rows[i].FindControl("Check1")).Checked == true)
                    //    {

                    //        gdvULDLoadPlan.Rows[i].Visible = false;
                    //    }
                    //}

                }


                if (CntAWBChk != 0 && CntULDChk == 0)//to add AWB rows to manifest
                {
                    Session["Split"] = "A";
                    //btnSave.Enabled = true;
                    //btnDepartFit.Enabled = true;
                    btnSave.Enabled = true;
                    AddAWBDatarowsToManifestGrid();



                    //to remove the awbnos and ulds from current grid once added to manifest details grid. 
                    for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
                    {
                        if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                        {

                            gdvULDLoadPlanAWB.Rows[i].Visible = false;

                        }
                    }
                    for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
                    {
                        //if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check1")).Checked == true)
                        //{

                        //    gdvULDLoadPlanAWB.Rows[i].Visible = false;
                        //}
                    }

                }

                if (CntAWBChk != 0 && CntULDChk != 0)//to add AWB and ULD rows to manifest if both are selected
                {
                    //AddULDnAWBDatarowsToManifestGrid();
                    //for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
                    //{
                    //    if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                    //    {

                    //        gdvULDLoadPlanAWB.Rows[i].Visible = false;

                    //    }
                    //}
                    //for (int i = 0; i < gdvULDLoadPlan.Rows.Count; i++)
                    //{
                    //    if (((CheckBox)gdvULDLoadPlan.Rows[i].FindControl("Check1")).Checked == true)
                    //    {

                    //        gdvULDLoadPlan.Rows[i].Visible = false;
                    //    }
                    //}

                }

                hdnManifestFlag.Value = "1";
            }
            catch (Exception ex)
            { }


        }

        #endregion ADD TO Manifest

        #region Add Checked AWB Data rows to Manifest Grid

        public void AddAWBDatarowsToManifestGrid()
        {
            try
            {
                string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby = "", AWBDest = "", CommoCode = "", AWBOrigin = "", DriverName = "";
                int PCS = 0, AVLPCS = 0, AWBPcs = 0;
                double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, AWBGwt = 0;
                //string Updatedon = "";
                string LoadingPriority = "", Remark = "", desc = "", Vol = "";
                bool IsManifest = false;
                string IsBonded = string.Empty, AWBType = string.Empty;

                FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                POL = Convert.ToString(Session["Station"]);
                // POU = ddlMainPOU.SelectedItem.Value.ToString();
                POU = null;// ddlMainPOU.Items[0].Value.ToString();
                Updatedby = Convert.ToString(Session["Username"]);
                ULDno = "Bulk";
                ULDwgt = 0;
                DateTime FlightDate = System.DateTime.Now;
                try
                {
                    if (txtfltdt.Text.Length > 5)
                    {
                        FlightDate = DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null);
                    }
                }
                catch (Exception ex) { }
                DataTable dsawb1 = new DataTable();
                DataSet dsgha = new DataSet();
                DataTable dtghaawb = new DataTable();
                DataTable dtgrdawb = new DataTable();

                DataSet dsawbData = new DataSet();
                //if (Session["AWBdata"] == null || Session["AWBdata"].ToString() == "")
                //{
                dsawbData = objExpMani.GetAwbTabdetails(lblDepAirport.Text, FLTno, FlightDate);

                try
                {
                    //dsgha.Tables.Add((DataTable)Session["gdvULDLoadPlanAWB"]);
                    //dsgha.AcceptChanges();
                    dsgha = (DataSet)Session["gdvULDLoadPlanAWB"];
                }
                catch (Exception ex) { }

                try
                {
                    //dtghaawb = (DataTable)Session["gdvULDLoadPlanAWB"];
                    dtghaawb = (DataTable)dsgha.Tables[0];
                }
                catch (Exception ex) { }
                // dsawb1 = dsawbData.Tables[0];
                //}
                //else
                //{
                try
                {
                    dsawb1 = (DataTable)Session["ManifestGridDatagha"];
                }
                catch (Exception ex)
                { }// Session["AWBdata"];
                // }
                if (Session["ManifestGridDatagha"] != null)
                {
                    dtgrdawb = (DataTable)Session["ManifestGridDatagha"];

                }
                else
                {
                    dtgrdawb.Columns.Add("AWBNumber", typeof(string));
                    dtgrdawb.Columns.Add("PiecesCount", typeof(string));
                    dtgrdawb.Columns.Add("GrossWeight", typeof(string));
                    dtgrdawb.Columns.Add("FlightNo", typeof(string));
                    dtgrdawb.Columns.Add("FlightDt", typeof(string));
                    dtgrdawb.Columns.Add("DriverName", typeof(string));
                    dtgrdawb.Columns.Add("DLNumber", typeof(string));
                    dtgrdawb.Columns.Add("Phone", typeof(string));
                    dtgrdawb.Columns.Add("VehicleNo", typeof(string));
                    dtgrdawb.Columns.Add("TokenNumber", typeof(string));
                    dtgrdawb.Columns.Add("TokenDate", typeof(string));
                    dtgrdawb.Columns.Add("IsScreeningReq", typeof(string));
                    dtgrdawb.Columns.Add("ULDNo", typeof(string));
                }

                IsManifest = false;

                DataTable dtNew = dtghaawb.Copy();

                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                    {
                        ((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked = false;
                        //   gdvULDLoadPlanAWB.Rows[i].Visible = false;
                        //Appended 25apr
                        AWBno = ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAWBno")).Text; //gdvULDLoadPlanAWB.Rows[i].Cells[1].Text;
                        PCS = Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblPCS")).Text);
                        WGT = Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblWt")).Text);
                        DriverName = ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblDriverName")).Text;
                        FltNumber = ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblFltNumber")).Text;
                        string FltDt = ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblFltDt")).Text;
                        string isscreen = ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblisscreen")).Text;
                        DataRow drgrd = dtgrdawb.NewRow();
                        drgrd["AWBNumber"] = AWBno.ToString();
                        drgrd["PiecesCount"] = PCS;
                        drgrd["GrossWeight"] = WGT;
                        drgrd["DriverName"] = DriverName;
                        drgrd["FlightNo"] = FltNumber;
                        drgrd["FlightDt"] = FltDt;
                        drgrd["IsScreeningReq"] = isscreen;


                        dtgrdawb.Rows.Add(drgrd);


                        //AVLPCS = Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlPCS")).Text);
                        //AVLWGT = Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlWgt")).Text);

                        //AWBPcs = Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotPCS")).Text);
                        //AWBGwt = Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotWgt")).Text);

                        // Updatedon = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                        try
                        {
                            if (dtghaawb.Rows.Count > 0)
                            {
                                for (int t = 0; t < dtghaawb.Rows.Count; t++)
                                //for (int t = dtghaawb.Rows.Count - 1; t >= 0; t--)
                                {
                                    if (dtghaawb.Rows[t][0].ToString() == AWBno)
                                    {
                                        ////AWBDest = dsawbData.Tables[1].Rows[t][5].ToString();
                                        ////desc = dsawbData.Tables[1].Rows[t][6].ToString();
                                        ////Vol = dsawbData.Tables[1].Rows[t][7].ToString();
                                        ////AWBOrigin = dsawbData.Tables[1].Rows[t]["OrginCode"].ToString();

                                        ////CommoCode = dsawbData.Tables[1].Rows[t]["CommodityCode"].ToString();
                                        ////IsBonded = dsawbData.Tables[1].Rows[t]["IsBonded"].ToString();
                                        ////AWBType = dsawbData.Tables[1].Rows[t]["Type"].ToString();
                                        //Added on 27 sept
                                        //dsawb1.Tables[1].Rows.
                                        dtghaawb.Rows[t].Delete();
                                        dsgha.Tables[0].Rows[t].Delete();
                                    }
                                }
                                dtghaawb.AcceptChanges();
                                dsgha.Tables[0].AcceptChanges();
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
                                    if (AWBno == tabMenifestDetails.Rows[n][5].ToString())
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
                        //if (IsAvialble == false)
                        //{
                        //    try
                        //    {
                        //        //if (desc == "" && AWBDest == "")
                        //        //{
                        //        //    
                        //        string ActAWB = AWBno.Substring(AWBno.Length - 8);

                        //        DataSet dsDestDetails = objExpMani.GetULDAWBData(ActAWB);
                        //        if (dsDestDetails.Tables[0].Rows.Count > 0)
                        //        {
                        //            AWBDest = dsDestDetails.Tables[0].Rows[0][0].ToString();
                        //            desc = dsDestDetails.Tables[0].Rows[0][1].ToString();
                        //            Vol = dsDestDetails.Tables[0].Rows[0][2].ToString();
                        //            CommoCode = dsDestDetails.Tables[0].Rows[0]["CommodityCode"].ToString();
                        //            AWBOrigin = dsDestDetails.Tables[0].Rows[0]["OriginCode"].ToString();

                        //        }


                        //        //}
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //    }

                        //    DataRow l_Datarow = tabMenifestDetails.NewRow();

                        //    l_Datarow["ULDno"] = ULDno;
                        //    l_Datarow["POU"] = POU;
                        //    l_Datarow["POL"] = POL;
                        //    l_Datarow["AWBDest"] = AWBDest;
                        //    l_Datarow["Counter"] = AWBType;
                        //    l_Datarow["AWBno"] = AWBno;
                        //    l_Datarow["SCC"] = CommoCode;
                        //    l_Datarow["PCS"] = PCS;
                        //    l_Datarow["GrossWgt"] = WGT;
                        //    l_Datarow["VOL"] = Vol;
                        //    l_Datarow["StatedPCS"] = AVLPCS;
                        //    l_Datarow["StatedWgt"] = AVLWGT;
                        //    l_Datarow["BookedPCS"] = AWBPcs;
                        //    l_Datarow["BookedWgt"] = AWBGwt;
                        //    l_Datarow["Desc"] = desc;
                        //    l_Datarow["Orign"] = AWBOrigin;
                        //    l_Datarow["Dest"] = AWBDest;
                        //    l_Datarow["Manifested"] = IsManifest;
                        //    l_Datarow["LoadingPriority"] = "";
                        //    l_Datarow["Remark"] = "";
                        //    l_Datarow["Bonded"] = IsBonded;

                        //    tabMenifestDetails.Rows.Add(l_Datarow);
                        //}

                        //if (tabMenifestDetails != null && tabMenifestDetails.Rows.Count > 0)
                        //{
                        //    Session["ManifestGridData"] = tabMenifestDetails;
                        //}

                        //gdvULDDetails.DataSource = "";

                        //gdvULDDetails.DataSource = (DataTable)Session["ManifestGridData"];
                        //gdvULDDetails.DataBind();
                        //try
                        //{
                        //    ChangeButtonStatus();
                        //}
                        //catch (Exception)
                        //{
                        //}

                        //Session["GDVULDDetails"] = tabMenifestDetails;
                    }
                }

                gdvULDLoadPlanAWB.DataSource = null;
                gdvULDLoadPlanAWB.DataBind();
                DataSet testds = new DataSet();
                testds.Tables.Add(dtghaawb.Copy());
                gdvULDLoadPlanAWB.DataSource = dtghaawb;
                gdvULDLoadPlanAWB.DataBind();
                Session["gdvULDLoadPlanAWB"] = testds;
                gdvULDDetails.DataSource = (DataTable)dtgrdawb;
                gdvULDDetails.DataBind();
                Session["grdulddetails"] = dtghaawb;
                Session["ManifestGridDatagha"] = dtgrdawb;

                try
                {
                    ChangeButtonStatus();
                }
                catch (Exception)
                {


                }
                //Added on 27 Sept

                if (dtgrdawb.Rows.Count > 0)
                {
                    for (int k = 0; k < dtghaawb.Rows.Count; k++)
                    {


                        //  tabMenifestDetails.Rows.Add(l_Datarow);
                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAWBno")).Text = dtghaawb.Rows[k][0].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPCS")).Text = dtghaawb.Rows[k][1].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWt")).Text = dtghaawb.Rows[k][2].ToString();
                        //((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlPCS")).Text = dsawb1.Rows[k][3].ToString();
                        //((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlWgt")).Text = dsawb1.Rows[k][4].ToString();

                        //((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblTotPCS")).Text = dsawb1.Rows[k]["AWBPcs"].ToString();
                        //((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblTotWgt")).Text = dsawb1.Rows[k]["AWBGwt"].ToString();

                        //MemDetails.Rows.Add(l_Datarow);

                        //gdvULDDetails.DataSource = null;
                        //gdvULDDetails.DataBind();

                        gdvULDDetails.DataSource = (DataTable)dtgrdawb;
                        gdvULDDetails.DataBind();

                        //Session["grdulddetails"] = dtghaawb;
                        //Session["ManifestGridDatagha"] = dtgrdawb;

                    }
                }
                Session["AWBdata"] = dtghaawb;// dsawbData.Tables[1];
                Session["AddToManifestAWBdata"] = dtghaawb;


                for (int j = 0; j < tabMenifestDetails.Rows.Count; j++)
                {
                    if (tabMenifestDetails.Rows[j][0].ToString() == "Bulk")
                    {

                        totalAWB = 1 + totalAWB;
                        totalAWBPCS = totalAWBPCS + int.Parse(tabMenifestDetails.Rows[j][7].ToString());
                        totalAWBWt = totalAWBWt + float.Parse(tabMenifestDetails.Rows[j][8].ToString());

                        totalAWBVol = totalAWBVol + int.Parse(tabMenifestDetails.Rows[j][9].ToString());

                    }

                }
                //ShowULDAWBSummary();
                //Session[""] = (DataTable);
            }
            catch (Exception ex)
            { }

        }

        #endregion Add Checked AWB Data rows to Manifest Grid

        #region Add Checked ULD Data rows to Manifest Grid

        public void AddULDDatarowsToManifestGrid()
        {
            try
            {
                string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby, AWBDest = "",FltDt="";
                int PCS = 0, AVLPCS = 0, AWBPcs = 0;
                double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, AWBGwt = 0;
                //string Updatedon = "";
                string LoadingPriority = "", Remark = "", vol = "", Desc = "";
                //DataSet dsULD = (DataSet)Session["ManifestGridDataghaULD"];
                DataTable dtULD = (DataTable)Session["ManifestGridDataghaULD"];

                FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                POL = Convert.ToString(Session["Station"]);
                //POU = ddlMainPOU.SelectedItem.Value.ToString();
                POU = null; // ddlMainPOU.Items[0].Value.ToString();
                Updatedby = Convert.ToString(Session["Username"]);

                SelectedULDnos = SelectedULDnos.Remove(SelectedULDnos.Length - 1, 1);
                DateTime FlightDate = DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null);

                //DataSet dsawb1 = objExpMani.GetAwbTabdetails(lblDepAirport.Text, FLTno, FlightDate);



                if (Request.QueryString["del"] == "1")
                {
                #region IMPORT SIDE
                

                    for (int j = 0; j < gdvULDLoadPlan.Rows.Count; j++)
                    {
                        if (((CheckBox)gdvULDLoadPlan.Rows[j].FindControl("Check1")).Checked == true)
                        {
                            //string SelectedULD = gdvULDLoadPlan.Rows[j].Cells[2].Text;lblULDNo
                            string SelectedULD = ((Label)gdvULDLoadPlan.Rows[j].FindControl("lblULDNo")).Text;

                            //DataSet dsGetULDAWBassdata = new DataSet();


                            //dsGetULDAWBassdata = Session["ManifestGridDatagha"]//objExpMani.GetULDDetails(SelectedULD);

                            //if (dsGetULDAWBassdata != null && dsGetULDAWBassdata.Tables[0].Rows.Count > 0)
                            //{
                            //for (int i = 0; i < dsGetULDAWBassdata.Tables[0].Rows.Count; i++)
                            //{
                            ULDno = ((Label)gdvULDLoadPlan.Rows[j].FindControl("lblULDNo")).Text;//dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(0).ToString();

                            //AWBno = dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(3).ToString();
                            PCS = Convert.ToInt32(((Label)gdvULDLoadPlan.Rows[j].FindControl("lblULDPCS")).Text);//Convert.ToInt32(dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(4).ToString());
                            WGT = Convert.ToDouble(((Label)gdvULDLoadPlan.Rows[j].FindControl("lblULDWt")).Text);//Convert.ToDouble(dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(5).ToString());
                            //AVLPCS = Convert.ToInt32(dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(6).ToString());
                            //AVLWGT = Convert.ToDouble(dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(7).ToString());
                            // Updatedon = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                            FLTno = ((Label)gdvULDLoadPlan.Rows[j].FindControl("lblFltNo")).Text;
                            FltDt = ((Label)gdvULDLoadPlan.Rows[j].FindControl("lblFltDt")).Text;

                            try
                            {
                                if (dtULD.Rows.Count > 0)
                                {
                                    for (int t = 0; t < dtULD.Rows.Count; t++)
                                    //for (int t = dtghaawb.Rows.Count - 1; t >= 0; t--)
                                    {
                                        if (dtULD.Rows[t][0].ToString() == ULDno)
                                        {
                                            dtULD.Rows[t].Delete();
                                            //dsULD.Tables[0].Rows[t].Delete();
                                        }
                                    }
                                    dtULD.AcceptChanges();
                                    //dsULD.Tables[0].AcceptChanges();
                                    //adding collected data to gridview rows one by one
                                }
                            }
                            catch (Exception ex)
                            {
                            }

                            //adding collected data to gridview rows one by one
                            DataTable tabMenifestDetails = new DataTable();
                            //if(Session["ManifestGridDatagha"] != null)
                            tabMenifestDetails = (DataTable)Session["ManifestGridDatagha"];

                            DataRow l_Datarow = tabMenifestDetails.NewRow();

                            l_Datarow["AWBNumber"] = "";
                            l_Datarow["PiecesCount"] = PCS;
                            l_Datarow["GrossWeight"] = WGT;
                            //l_Datarow["AWBDest"] = AWBDest;
                            l_Datarow["FlightNo"] = FLTno;
                            l_Datarow["FlightDt"] = FltDt;
                            //l_Datarow["SCC"] = CommCode;
                            l_Datarow["DriverName"] = "";
                            l_Datarow["DLNumber"] = "";
                            l_Datarow["Phone"] = "";
                            l_Datarow["VehicleNo"] = "";
                            l_Datarow["TokenNumber"] = "";
                            l_Datarow["TokenDate"] = "";
                            l_Datarow["IsScreeningReq"] = "";
                            l_Datarow["ULDNo"] = ULDno;
                            //l_Datarow["Manifested"] = IsManifest;
                            //l_Datarow["LoadingPriority"] = "";
                            //l_Datarow["Remark"] = "";
                            //l_Datarow["Bonded"] = "";


                            tabMenifestDetails.Rows.Add(l_Datarow);
                            if (tabMenifestDetails != null && tabMenifestDetails.Rows.Count > 0)
                            {
                                Session["ManifestGridDatagha"] = tabMenifestDetails;
                            }

                            gdvULDDetails.DataSource = (DataTable)Session["ManifestGridDatagha"];
                            gdvULDDetails.DataBind();
                            Session["ManifestGridDataghaULD"] = dtULD;
                            //gdvULDLoadPlan.DataSource = dtULD;
                            gdvULDLoadPlan.DataSource = (DataTable)Session["ManifestGridDataghaULD"];
                            gdvULDLoadPlan.DataBind();
                            try
                            {
                                ChangeButtonStatus();
                            }
                            catch (Exception)
                            {


                            }
                        }
                        //}
                        //}
                    }

                #endregion
                }
                else
                {
                    #region EXPORT SIDE
                    string[] Pname = new string[5];
                    object[] Pvalue = new object[5];
                    SqlDbType[] Ptype = new SqlDbType[5];




                    Pname[0] = "FltNo";
                    Ptype[0] = SqlDbType.NVarChar;
                    Pvalue[0] = FLTno;

                    Pname[1] = "FltDate";
                    Ptype[1] = SqlDbType.DateTime;
                    Pvalue[1] = FlightDate;

                    Pname[2] = "Station";
                    Ptype[2] = SqlDbType.NVarChar;
                    Pvalue[2] = POL;

                    Pname[3] = "AWBno";
                    Ptype[3] = SqlDbType.NVarChar;
                    Pvalue[3] = "";

                    Pname[4] = "TKNNO";
                    Ptype[4] = SqlDbType.NVarChar;
                    Pvalue[4] = "";

                    DataSet dsawb1 = da.SelectRecords("SP_GHAIncommingBooking", Pname, Pvalue, Ptype);



                    bool IsManifest = false;
                    string CommCode = string.Empty;

                    for (int j = 0; j < gdvULDLoadPlan.Rows.Count; j++)
                    {
                        if (((CheckBox)gdvULDLoadPlan.Rows[j].FindControl("Check1")).Checked == true)
                        {
                            //string SelectedULD = gdvULDLoadPlan.Rows[j].Cells[2].Text;lblULDNo
                            string SelectedULD = ((Label)gdvULDLoadPlan.Rows[j].FindControl("lblULDNo")).Text;

                            DataSet dsGetULDAWBassdata = new DataSet();


                            dsGetULDAWBassdata = objExpMani.GetULDDetails(SelectedULD);

                            if (dsGetULDAWBassdata != null && dsGetULDAWBassdata.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsGetULDAWBassdata.Tables[0].Rows.Count; i++)
                                {
                                    ULDno = dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(0).ToString();

                                    AWBno = dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(3).ToString();
                                    PCS = Convert.ToInt32(dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(4).ToString());
                                    WGT = Convert.ToDouble(dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(5).ToString());
                                    //AVLPCS = Convert.ToInt32(dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(6).ToString());
                                    //AVLWGT = Convert.ToDouble(dsGetULDAWBassdata.Tables[0].Rows[i].ItemArray.GetValue(7).ToString());
                                    // Updatedon = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

                                    try
                                    {
                                        if (dtULD.Rows.Count > 0)
                                        {
                                            for (int t = 0; t < dtULD.Rows.Count; t++)
                                            //for (int t = dtghaawb.Rows.Count - 1; t >= 0; t--)
                                            {
                                                if (dtULD.Rows[t][0].ToString() == ULDno)
                                                {
                                                    dtULD.Rows[t].Delete();
                                                    //dsULD.Tables[0].Rows[t].Delete();
                                                }
                                            }
                                            dtULD.AcceptChanges();
                                            //dsULD.Tables[0].AcceptChanges();
                                            //adding collected data to gridview rows one by one
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }

                                    //adding collected data to gridview rows one by one
                                    DataTable tabMenifestDetails = new DataTable();
                                    //if(Session["ManifestGridDatagha"] != null)
                                    tabMenifestDetails = (DataTable)Session["ManifestGridDatagha"];

                                    DataRow l_Datarow = tabMenifestDetails.NewRow();

                                    l_Datarow["AWBNumber"] = AWBno;
                                    l_Datarow["PiecesCount"] = PCS;
                                    l_Datarow["GrossWeight"] = WGT;
                                    //l_Datarow["AWBDest"] = AWBDest;
                                    l_Datarow["FlightNo"] = "";
                                    l_Datarow["FlightDt"] = "";
                                    //l_Datarow["SCC"] = CommCode;
                                    l_Datarow["DriverName"] = "";
                                    l_Datarow["DLNumber"] = "";
                                    l_Datarow["Phone"] = "";
                                    l_Datarow["VehicleNo"] = "";
                                    l_Datarow["TokenNumber"] = "";
                                    l_Datarow["TokenDate"] = "";
                                    l_Datarow["IsScreeningReq"] = "";
                                    l_Datarow["ULDNo"] = ULDno;
                                    //l_Datarow["Manifested"] = IsManifest;
                                    //l_Datarow["LoadingPriority"] = "";
                                    //l_Datarow["Remark"] = "";
                                    //l_Datarow["Bonded"] = "";


                                    tabMenifestDetails.Rows.Add(l_Datarow);
                                    if (tabMenifestDetails != null && tabMenifestDetails.Rows.Count > 0)
                                    {
                                        Session["ManifestGridDatagha"] = tabMenifestDetails;
                                    }
                                    //if (tabMenifestDetails.Rows[i][0].ToString() == "Bulk")
                                    //{
                                    //    totalAWB = 1 + totalAWB;
                                    //    totalAWBPCS = totalAWBPCS + int.Parse(tabMenifestDetails.Rows[i][7].ToString());
                                    //    totalAWBWt = totalAWBWt + int.Parse(tabMenifestDetails.Rows[i][8].ToString());
                                    //    totalAWBVol = totalAWBVol + int.Parse(tabMenifestDetails.Rows[i][9].ToString());
                                    //}
                                    //else
                                    //{
                                    //    TotalULD = 1 + TotalULD;
                                    //    totalULDAWB = 1 + totalULDAWB;
                                    //    totalULDPCS = totalULDPCS + int.Parse(tabMenifestDetails.Rows[i][7].ToString());
                                    //    totalULDWt = totalULDWt + int.Parse(tabMenifestDetails.Rows[i][8].ToString());
                                    //    totalULDVol = totalULDVol + int.Parse(tabMenifestDetails.Rows[i][9].ToString());

                                    //}

                                    gdvULDDetails.DataSource = (DataTable)Session["ManifestGridDatagha"];
                                    gdvULDDetails.DataBind();
                                    Session["ManifestGridDataghaULD"] = dtULD;
                                    //gdvULDLoadPlan.DataSource = dtULD;
                                    gdvULDLoadPlan.DataSource = (DataTable)Session["ManifestGridDataghaULD"];
                                    gdvULDLoadPlan.DataBind();
                                    try
                                    {
                                        ChangeButtonStatus();
                                    }
                                    catch (Exception)
                                    {


                                    }

                                    //Added on 27 Sept

                                    //if (dsawb1.Tables[1].Rows.Count > 0)
                                    //{
                                    //    for (int k = 0; k < dsawb1.Tables[1].Rows.Count; i++)
                                    //    {

                                    //        //  tabMenifestDetails.Rows.Add(l_Datarow);
                                    //        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAWBno")).Text = dsawb1.Tables[1].Rows[i][0].ToString();
                                    //        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblPieces")).Text = dsawb1.Tables[1].Rows[i][1].ToString();
                                    //        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblWeight")).Text = dsawb1.Tables[1].Rows[i][2].ToString();
                                    //        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlPCS")).Text = dsawb1.Tables[1].Rows[i][3].ToString();
                                    //        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlWgt")).Text = dsawb1.Tables[1].Rows[i][4].ToString();

                                    //        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotPCS")).Text = dsawb1.Tables[1].Rows[i]["AWBPcs"].ToString();
                                    //        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotWgt")).Text = dsawb1.Tables[1].Rows[i]["AWBGwt"].ToString();

                                    //        //MemDetails.Rows.Add(l_Datarow);

                                    //        //gdvULDLoadPlanAWB.DataSource = (DataTable)MemDetails;
                                    //        //gdvULDLoadPlanAWB.DataBind();

                                    //    }
                                    //}
                                    //Session["AWBdata"] = dsawb1.Tables[1];






                                }
                            }
                        }
                    }
                    #endregion
                }
                //ShowULDAWBSummary();

                //Session[""] = (DataTable);
            }
            catch (Exception ex)
            {
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
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    bool res = false;
        //    string strMode = "";
        //    for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
        //    {
        //        //if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
        //        //{

        //        string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby, counter = "", SCC = "", Desc = "";
        //        int PCS = 0, AVLPCS = 0;
        //        double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, VOL = 0;
        //        string Updatedon = "", IMENo="";
        //        string LoadingPriority = "", Remark = "", ULDDest = "", AWBOrigin = "", IsBonded = "";

        //        FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
        //        Updatedby = Convert.ToString(Session["Username"]);

        //        IMENo = "";

        //        ULDno = gdvULDDetails.Rows[i].Cells[1].Text.Trim();
        //        POU = gdvULDDetails.Rows[i].Cells[2].Text.Trim();
        //        POL = gdvULDDetails.Rows[i].Cells[3].Text.Trim();// Session["Station"].ToString(); 

        //        //gdvULDDetails.Rows[i].Cells[3].Text;
        //        ULDDest = gdvULDDetails.Rows[i].Cells[4].Text.Trim();
        //        if (gdvULDDetails.Rows[i].Cells[5].Text.Trim() == "&nbsp;")
        //        {
        //            counter = "0";
        //        }
        //        else
        //        {
        //            counter = gdvULDDetails.Rows[i].Cells[5].Text.Trim();
        //        }
        //        AWBno = gdvULDDetails.Rows[i].Cells[6].Text.Trim();
        //        AWBno = AWBno.Substring(AWBno.Length - 8);

        //        if (gdvULDDetails.Rows[i].Cells[7].Text == "&nbsp;")
        //        {
        //            SCC = "";
        //        }
        //        else
        //        {
        //            SCC = gdvULDDetails.Rows[i].Cells[7].Text;
        //        }


        //        PCS = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[8].Text);
        //        WGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[9].Text);
        //        if (gdvULDDetails.Rows[i].Cells[10].Text == "&nbsp;")
        //        {
        //            VOL = 0;
        //        }
        //        else
        //        {
        //            VOL = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[10].Text);
        //        }
        //        AVLPCS = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[13].Text);
        //        AVLWGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[14].Text);
        //        if (gdvULDDetails.Rows[i].Cells[15].Text == "&nbsp;")
        //        {
        //            Desc = "";
        //        }
        //        else
        //        {
        //            Desc = gdvULDDetails.Rows[i].Cells[15].Text;
        //            Desc = Desc.Replace("&amp;", "&");
        //            //Desc for commodity code
        //            // Desc = Desc.Substring(3, Desc.Length);
        //        }

        //        if (gdvULDDetails.Rows[i].Cells[19].Text == "&nbsp;")
        //        {
        //            LoadingPriority = "";
        //        }
        //        else
        //        {
        //            LoadingPriority = ((DropDownList)gdvULDDetails.Rows[i].FindControl("ddlLoadingPriority")).SelectedItem.Text.ToString();
        //        }

        //        if (gdvULDDetails.Rows[i].Cells[20].Text == "&nbsp;")
        //        {
        //            Remark = "";
        //        }
        //        else
        //        {
        //            Remark = ((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text.ToString();
        //        }

        //        if (gdvULDDetails.Rows[i].Cells[21].Text == "&nbsp;")
        //        {
        //            IsBonded = "";
        //        }
        //        else
        //        {
        //            IsBonded = gdvULDDetails.Rows[i].Cells[21].Text.Trim();
        //        }                

        //        string CurrLoc = gdvULDDetails.Rows[i].Cells[3].Text; ;// Session["Station"].ToString();

        //        Updatedon = dtCurrentDate.ToString("yyyy/MM/dd"); //DateTime.Now.ToString("yyyy/MM/dd");
        //        strMode = Convert.ToString(Session["Split"]);
        //        bool IsManifest = false;
        //        IsManifest = bool.Parse(gdvULDDetails.Rows[i].Cells[18].Text);
        //        DateTime FltDate = DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null);
        //        AWBOrigin = gdvULDDetails.Rows[i].Cells[16].Text.Trim();
        //        string tailNo = string.Empty;

        //        if (strMode != "O")
        //        {
        //            //If condition added for save POL AWB's match with this location  
        //            if (Session["Station"].ToString() == POL)
        //            {
        //                string strResult = string.Empty;
        //                string strIdentifier = Convert.ToString(Session["Identifier"]);
        //                //appended 25apr -add awb to manifest completed so they dont appear in awb again
        //                bool result = objExpMani.AddULDAWBassocition(FLTno, POL, POU, ULDno, ULDwgt, AWBno, PCS, WGT, AVLPCS, AVLWGT, Updatedby, dtCurrentDate, FltDate, strIdentifier, ref strResult,"");                    //

        //                if (strResult.Length > 0)
        //                {
        //                    lblStatus.ForeColor = Color.Red;
        //                    lblStatus.Text = strResult;
        //                    return;
        //                }

        //                //res = objExpMani.SaveManifestdata(FLTno, ULDno, POU, POL, ULDDest, counter, AWBno, SCC, VOL, PCS, WGT, AVLPCS, AVLWGT,
        //                //    Desc, LoadingPriority, Remark, Updatedby, Updatedon, IsManifest, FltDate, CurrLoc, AWBOrigin,
        //                //    1, IsBonded,tailNo,IMENo,"");
        //            }
        //        }


        //        // }

        //    }
        //    if (res == true || strMode == "O")
        //    {
        //        ////BtnList_Click(null, null);
        //        lblStatus.ForeColor = Color.Green;
        //        lblStatus.Text = "Manifest Version Saved Successfully";

        //        return;
        //    }

        //}
        #endregion Save Manifest Grid data To DB


        #region Commit Manifest
        //All Code is same as on SAVE manifest button only thing is here ISACTIVE is set to false....that means manifest version final/closed 
        //protected void btnCommManifest_Click(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        bool res = false;

        //        for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
        //        {
        //            //if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
        //            //{
        //            string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby, counter = "", SCC = "", Desc = "";
        //            int PCS = 0, AVLPCS = 0;
        //            double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, VOL = 0;
        //            string Updatedon = "";
        //            string LoadingPriority = "", Remark = "", ULDDest = "";

        //            FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
        //            Updatedby = Convert.ToString(Session["Username"]);

        //            ULDno = gdvULDDetails.Rows[i].Cells[1].Text.Trim();
        //            POU = gdvULDDetails.Rows[i].Cells[2].Text.Trim();
        //            POL = gdvULDDetails.Rows[i].Cells[3].Text.Trim();
        //            ULDDest = gdvULDDetails.Rows[i].Cells[4].Text.Trim();
        //            if (gdvULDDetails.Rows[i].Cells[5].Text == "&nbsp;")
        //            {
        //                counter = "0";
        //            }
        //            else
        //            {
        //                counter = gdvULDDetails.Rows[i].Cells[5].Text;
        //            }
        //            AWBno = gdvULDDetails.Rows[i].Cells[6].Text.Trim();
        //            AWBno = AWBno.Substring(AWBno.Length - 8);
        //            if (gdvULDDetails.Rows[i].Cells[7].Text == "&nbsp;")
        //            {
        //                SCC = "0";
        //            }
        //            else
        //            {
        //                SCC = gdvULDDetails.Rows[i].Cells[7].Text;
        //            }


        //            PCS = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[8].Text);
        //            WGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[9].Text);
        //            if (gdvULDDetails.Rows[i].Cells[10].Text == "&nbsp;")
        //            {
        //                VOL = 0;
        //            }
        //            else
        //            {
        //                VOL = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[10].Text);
        //            }
        //            AVLPCS = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[13].Text);
        //            AVLWGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[14].Text);
        //            if (gdvULDDetails.Rows[i].Cells[15].Text == "&nbsp;")
        //            {
        //                Desc = "";
        //            }
        //            else
        //            {
        //                Desc = gdvULDDetails.Rows[i].Cells[15].Text;
        //                Desc = Desc.Replace("&nbsp;", "&");
        //            }

        //            bool IsManifest = bool.Parse(gdvULDDetails.Rows[i].Cells[18].Text);
        //            if (gdvULDDetails.Rows[i].Cells[19].Text == "&nbsp;")
        //            {
        //                LoadingPriority = "";
        //            }
        //            else
        //            {
        //                LoadingPriority = ((DropDownList)gdvULDDetails.Rows[i].FindControl("ddlLoadingPriority")).SelectedItem.Text.ToString();
        //            }

        //            if (gdvULDDetails.Rows[i].Cells[20].Text == "&nbsp;")
        //            {
        //                Remark = "";
        //            }
        //            else
        //            {
        //                Remark = ((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text.ToString();
        //            }


        //            // Updatedon = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        //            Updatedon = dtCurrentDate.ToString("yyyy/MM/dd"); //DateTime.Now.ToString("yyyy/MM/dd");
        //            DateTime dtflightdate = DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null);
        //            //res = objExpMani.CommitManifestdata(FLTno, ULDno, POU, POL, ULDDest, counter, AWBno, SCC, VOL, PCS, WGT, AVLPCS, AVLWGT, Desc, LoadingPriority, Remark, Updatedby, Updatedon, dtflightdate);

        //            //Add Records for Tracking purpose

        //            Object[] AWBDetails = new object[6];
        //            int intI = 0;

        //            AWBDetails.SetValue(AWBno, intI);
        //            intI++;

        //            AWBDetails.SetValue(PCS, intI);
        //            intI++;

        //            AWBDetails.SetValue(FLTno, intI);
        //            intI++;

        //            AWBDetails.SetValue("Manifested", intI);
        //            intI++;

        //            AWBDetails.SetValue(WGT, intI);
        //            intI++;

        //            AWBDetails.SetValue("", intI);
        //            intI++;

        //            AWBMilestonesBAL objMileStone = new AWBMilestonesBAL();
        //            //if (IsManifest == false && res == true)
        //            //{

        //            string strResult = objMileStone.AddAWBMaifestMilestone(AWBDetails);
        //            // }
        //            //if ((string)Session["IsDeparted"] == "R")
        //            //{
        //            //    string strResult = objMileStone.AddAWBMilestone(AWBDetails);
        //            //}
        //            objMileStone = null;
        //            AWBDetails = null;

        //            //End of recording tracking for AWB.
        //            //}

        //        }

        //        if (res == true)
        //        {
        //            Session["IsCommit"] = "True";
        //            // ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Successfull();", true);
        //            lblStatus.ForeColor = Color.Green;
        //            lblStatus.Text = "Manifest Version Commited Successfully";
        //            // return;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //    }



        //    /* Old Code 
        //     return;

        //     for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
        //     {
        //         if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
        //         {
        //             string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby, counter = "", SCC = "", Desc = "";
        //             int PCS = 0, AVLPCS = 0, VOL = 0;
        //             double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0;
        //             string Updatedon = "";
        //             string LoadingPriority = "", Remark = "", ULDDest = "";

        //             FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
        //             Updatedby = Convert.ToString(Session["Username"]);

        //             ULDno = gdvULDDetails.Rows[i].Cells[1].Text;
        //             POU = gdvULDDetails.Rows[i].Cells[2].Text;
        //             POL = gdvULDDetails.Rows[i].Cells[3].Text;
        //             ULDDest = gdvULDDetails.Rows[i].Cells[4].Text;
        //             if (gdvULDDetails.Rows[i].Cells[5].Text == "&nbsp;")
        //             {
        //                 counter = "0";
        //             }
        //             else
        //             {
        //                 counter = gdvULDDetails.Rows[i].Cells[5].Text;
        //             }
        //             AWBno = gdvULDDetails.Rows[i].Cells[6].Text;
        //             if (gdvULDDetails.Rows[i].Cells[7].Text == "&nbsp;")
        //             {
        //                 SCC = "0";
        //             }
        //             else
        //             {
        //                 SCC = gdvULDDetails.Rows[i].Cells[7].Text;
        //             }


        //             PCS = Convert.ToInt16(gdvULDDetails.Rows[i].Cells[8].Text);
        //             WGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[9].Text);
        //             if (gdvULDDetails.Rows[i].Cells[10].Text == "&nbsp;")
        //             {
        //                 VOL = 0;
        //             }
        //             else
        //             {
        //                 VOL = Convert.ToInt16(gdvULDDetails.Rows[i].Cells[10].Text);
        //             }
        //             AVLPCS = Convert.ToInt16(gdvULDDetails.Rows[i].Cells[11].Text);
        //             AVLWGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[12].Text);
        //             if (gdvULDDetails.Rows[i].Cells[13].Text == "&nbsp;")
        //             {
        //                 Desc = "";
        //             }
        //             else
        //             {
        //                 Desc = gdvULDDetails.Rows[i].Cells[13].Text;
        //             }

        //             if (gdvULDDetails.Rows[i].Cells[16].Text == "&nbsp;")
        //             {
        //                 LoadingPriority = "";
        //             }
        //             else
        //             {
        //                 LoadingPriority = ((DropDownList)gdvULDDetails.Rows[i].FindControl("ddlLoadingPriority")).SelectedItem.Text.ToString();
        //             }

        //             if (gdvULDDetails.Rows[i].Cells[17].Text == "&nbsp;")
        //             {
        //                 Remark = "";
        //             }
        //             else
        //             {
        //                 Remark = ((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text.ToString();
        //             }


        //             Updatedon = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        //             bool res = objExpMani.CommitManifestdata(FLTno, ULDno, POU, POL, ULDDest, counter, AWBno, SCC, VOL, PCS, WGT, AVLPCS, AVLWGT, Desc, LoadingPriority, Remark, Updatedby, Updatedon);

        //             if (res == true)
        //             {
        //                 // ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Successfull();", true);
        //                 lblStatus.ForeColor = Color.Green;
        //                 lblStatus.Text = "Manifest Version Commited Successfully";
        //                 return;
        //             }
        //         }

        //     }
        //    */
        //}
        #endregion Commit Manifest

        protected void BtnListDetails_Click(object sender, EventArgs e)
        {


        }

        #region Button list Details Click
        //protected void BtnListDetails_Click1(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        Session["AWBdata"] = null;
        //        gdvULDLoadPlanAWB.DataSource = null;
        //        gdvULDLoadPlanAWB.DataBind();
        //        try
        //        {
        //            ChangeButtonStatus();
        //        }
        //        catch (Exception)
        //        {


        //        }

        //        string FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();


        //        string ManifestDate = "", ManifestdateFrom = "", ManifestdateTo = "";

        //        ManifestDate = txtfltdt.Text.ToString();
        //        if (ManifestDate.Length > 10)
        //        {
        //            ManifestdateFrom = ManifestDate.Substring(0, 10) + " 00:00:00";
        //            ManifestdateTo = ManifestDate.Substring(0, 10) + " 23:59:59";
        //        }
        //        else
        //        {
        //            ManifestdateFrom = ManifestDate;// +" 00:00:00";
        //            ManifestdateTo = ManifestDate + " 23:59:59";
        //        }

        //        string version = string.Empty;
        //        string UldNo = string.Empty;
        //        string ValPOL = string.Empty;
        //        string ValPOU = string.Empty;
        //        string DepartureAirport = lblDepAirport.Text;



        //        DataSet ds = objExpMani.GetManifestDetailsRevised(FLTno, ManifestdateFrom, ManifestdateTo, DepartureAirport, version, UldNo, ValPOL, ValPOU);//return manifest data
        //        DateTime FlightDate = DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null);
        //        DataSet dsawb3 = objExpMani.GetAwbTabdetails(lblDepAirport.Text, FLTno, FlightDate);

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            string ULDno = "", AWBno = "", POU = "", POL = "", AWBDest = "", CommCode = "";
        //            int PCS = 0, AVLPCS = 0, AWBPcs = 0;
        //            double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, VolWt = 0.0, AWBGwt = 0;
        //            string Updatedon = "", Desc = "";
        //            bool blnIsCommited = false;
        //            //All Old code by Poorna
        //            /*
        //            blnIsCommited = Convert.ToBoolean(ds.Tables[0].Rows[0]["Isactive"]);
        //            if (blnIsCommited)
        //            {
        //                btnOffload.Enabled = false;
        //                btnUnassign.Enabled = true;
        //                btnSplitUnassign.Enabled = true;
        //            }
        //            else
        //            {
        //                btnOffload.Enabled = true;
        //                btnUnassign.Enabled = false ;
        //                btnSplitUnassign.Enabled = false;

        //            }*/

        //            //btnOffload.Enabled = false;
        //            btnUnassign.Enabled = false;
        //            btnSplitUnassign.Enabled = false;
        //            btnSplitUnassign.Enabled = false;
        //            BtnAddtoManifest.Enabled = false;

        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                ULDno = ds.Tables[0].Rows[i].ItemArray.GetValue(0).ToString();

        //                AWBno = ds.Tables[0].Rows[i]["AWBno"].ToString();
        //                PCS = Convert.ToInt32(ds.Tables[0].Rows[i]["PCS"]);
        //                WGT = Convert.ToDouble(ds.Tables[0].Rows[i]["GrossWgt"]);
        //                AVLPCS = Convert.ToInt32(ds.Tables[0].Rows[i]["StatedPCS"]);
        //                AVLWGT = Convert.ToDouble(ds.Tables[0].Rows[i]["StatedWgt"]);

        //                AWBPcs = Convert.ToInt32(ds.Tables[0].Rows[i]["AWBPcs"]);
        //                AWBGwt = Convert.ToDouble(ds.Tables[0].Rows[i]["AWBGwt"]);

        //                // Updatedon = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        //                VolWt = Convert.ToDouble(ds.Tables[0].Rows[i]["Vol"]);
        //                Desc = ds.Tables[0].Rows[i]["Desc"].ToString();
        //                bool IsManifest = bool.Parse(ds.Tables[0].Rows[i]["Manifested"].ToString());
        //                try
        //                {
        //                    if (dsawb3.Tables[1].Rows.Count > 0)
        //                    {
        //                        if (dsawb3.Tables[1].Rows[i][0].ToString() == AWBno)
        //                        {
        //                            AWBDest = dsawb3.Tables[1].Rows[i]["Dest"].ToString();
        //                            CommCode = dsawb3.Tables[1].Rows[i]["CommodityCode"].ToString();
        //                        }
        //                    }
        //                    //adding collected data to gridview rows one by one
        //                }
        //                catch (Exception ex)
        //                {
        //                }
        //                //adding collected data to gridview rows one by one

        //                tabMenifestDetails = (DataTable)Session["ManifestGridData"];

        //                if (tabMenifestDetails == null)
        //                    tabMenifestDetails = new DataTable();

        //                DataRow l_Datarow = tabMenifestDetails.NewRow();

        //                l_Datarow["ULDno"] = ULDno;
        //                l_Datarow["POU"] = POU;
        //                l_Datarow["POL"] = POL;
        //                l_Datarow["AWBDest"] = AWBDest;
        //                l_Datarow["Counter"] = "";
        //                l_Datarow["AWBno"] = AWBno;
        //                l_Datarow["SCC"] = CommCode;
        //                l_Datarow["PCS"] = PCS;
        //                l_Datarow["GrossWgt"] = WGT;
        //                l_Datarow["VOL"] = VolWt;
        //                l_Datarow["StatedPCS"] = AVLPCS;
        //                l_Datarow["StatedWgt"] = AVLWGT;

        //                l_Datarow["BookedPCS"] = AWBPcs;
        //                l_Datarow["BookedWgt"] = AWBGwt;

        //                l_Datarow["Desc"] = Desc;
        //                l_Datarow["Orign"] = POL;
        //                l_Datarow["Dest"] = AWBDest;
        //                l_Datarow["Manifested"] = IsManifest;
        //                l_Datarow["LoadingPriority"] = "";
        //                l_Datarow["Remark"] = "";
        //                l_Datarow["Bonded"] = "";


        //                tabMenifestDetails.Rows.Add(l_Datarow);

        //                if (tabMenifestDetails.Rows[i][0].ToString() == "Bulk")
        //                {
        //                    totalAWB = 1 + totalAWB;
        //                    totalAWBPCS = totalAWBPCS + int.Parse(tabMenifestDetails.Rows[i]["PCS"].ToString());
        //                    totalAWBWt = totalAWBWt + int.Parse(tabMenifestDetails.Rows[i]["GrossWgt"].ToString());
        //                    totalAWBVol = totalAWBVol + float.Parse(tabMenifestDetails.Rows[i]["Vol"].ToString());
        //                }
        //                else
        //                {
        //                    TotalULD = 1 + TotalULD;
        //                    totalULDAWB = 1 + totalULDAWB;
        //                    totalULDPCS = totalULDPCS + int.Parse(tabMenifestDetails.Rows[i]["PCS"].ToString());
        //                    totalULDWt = totalULDWt + int.Parse(tabMenifestDetails.Rows[i]["GrossWgt"].ToString());
        //                    totalULDVol = totalULDVol + float.Parse(tabMenifestDetails.Rows[i]["Vol"].ToString());

        //                }


        //                gdvULDDetails.DataSource = ds;
        //                gdvULDDetails.DataBind();
        //                try
        //                {
        //                    ChangeButtonStatus();
        //                }
        //                catch (Exception)
        //                {


        //                }

        //                Session["GDVULDDetails"] = ds.Tables[0];
        //            }
        //            ShowULDAWBSummary();
        //            //btnSplitUnassign.Enabled = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        #endregion

        #region On Print Manifest button Click
        //protected void btnPrintMFT_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int intAWBTypes = 0;
        //        string strAWBType = string.Empty;

        //        if (Session["AT"] != null)
        //        {
        //            intAWBTypes = ((DataTable)Session["AT"]).Rows.Count;
        //        }
        //        string hidPrintMFTURLs = "";
        //        if (intAWBTypes > 0)
        //        {
        //            for (int m = 0; m < intAWBTypes; m++)
        //            {
        //                strAWBType = ((DataTable)Session["AT"]).Rows[m][0].ToString();
        //                hidPrintMFTURLs = hidPrintMFTURLs + showManifestData(strAWBType, m) + "|";
        //                //System.Threading.Thread.Sleep(2000);
        //            }
        //            HidPrintMFT.Value = hidPrintMFTURLs.Substring(0, hidPrintMFTURLs.Length - 1);
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>LoopPrintMFT();</script>", false);
        //        }
        //        else
        //            showManifestData("", 0);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return;
        //}
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
                    string strOrg1 = gdvULDDetails.Rows[j].Cells[3].Text.Trim();

                    if (!arPOU.Contains(gdvULDDetails.Rows[j].Cells[2].Text))
                    {
                        arPOU.Add(gdvULDDetails.Rows[j].Cells[2].Text);
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

                    Logo = CommonUtility.GetImageStream(Page.Server);
                    //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                }
                catch (Exception ex)
                {
                    Logo = new System.IO.MemoryStream();
                }

                //end

                DataTable DTAWBDetails = new DataTable();

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

                    DataSet dsAirlineDetails = objMst.GetAirlineDetails(strLoadingPt, strUnloadPt);
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
                string strFlightNo = ddlFlightCode.Text + txtFlightID.Text + "        " + txtfltdt.Text;

                //ddlMainPOU.Items[m].Text;// ddlMainPOU.Text;//need to change
                float strTotalWeight = 0;
                string strPreparedBy = Session["Username"].ToString();
                int strTotal = 0;
                string UnloadAt = "";
                string TailNo = string.Empty;
                string EGMNo = string.Empty;

                DateTime dt = new DateTime();

                try
                {
                    //DateTime dt1 = new DateTime();
                    dt = DateTime.ParseExact(txtfltdt.Text.Trim(), "dd/MM/yyyy", null);


                    DataSet DsNationality = objExpMani.GetPOUAirlineSchedule(ddlFlightCode.Text + txtFlightID.Text, strLoadingPt, dt);


                    strNationality = DsNationality.Tables[1].Rows[0][0].ToString();
                }
                catch (Exception ex)
                {

                }



                DataTable DTExport = new DataTable();

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


                DataTable DTTransitExport = new DataTable();

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
                    SaveNilManifest(ddlFlightCode.Text + txtFlightID.Text, "Bulk", strLoadingPt, strUnloadPt, strPreparedBy, dt, dtCurrentDate);
                    if (gdvULDDetails.Rows.Count < 1)
                    {
                        DataRow DtRow = DTExport.NewRow();
                        DtRow["Owner"] = strOwner;
                        DtRow["Nationality"] = strNationality;
                        DtRow["FlightNo"] = strFlightNo;
                        DtRow["LoadingPt"] = strLoadingPt;
                        DtRow["UnloadPt"] = strUnloadPt;
                        DtRow["TotalWeight"] = "Nil";
                        DtRow["PreparedBy"] = strPreparedBy;
                        DtRow["Total"] = "Nil";
                        DtRow["UnloadAt"] = strUnloadPt;
                        DtRow["TailNo"] = string.Empty;
                        DtRow["EGMNo"] = string.Empty;
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

                //if (!ULDDestpt.Contains(gdvULDDetails.Rows[i].Cells[2].Text))
                //{
                //     UnloadAt = ddlMainPOU.Text;
                //}
                string FlightNo = ddlFlightCode.Text + txtFlightID.Text;

                DTAWBDetails.Rows.Clear();
                DTExport.Rows.Clear();
                DTTransitExport.Rows.Clear();

                for (int l = 0; l < gdvULDDetails.Rows.Count; l++)
                {
                    //strUnloadPt = ddlMainPOU.Items[m].Text;// ddlMainPOU.Text;//need to change
                    //if (ddlMainPOU.Items[m].Text == gdvULDDetails.Rows[l].Cells[2].Text)
                    //{

                    if (((DataTable)Session["MD"]).Rows[l]["DocumentType"].ToString() == strAWBType)
                    {

                        string pou = arPOU[0].ToString();
                        if (pou == gdvULDDetails.Rows[l].Cells[2].Text.Trim())
                        {

                            //DataSet dsawb2 = objExpMani.GetAwbTabdetails(lblDepAirport.Text, FlightNo);
                            // for(int j=0;j<ddlMainPOU.Items.Count;j++)
                            // {
                            //if(!ULDDestpt.Contains(ddlMainPOU.Items[j].Text))
                            // {
                            //     ULDDestpt.Add(ddlMainPOU.Items[j].Text);
                            int Totalpcs = 0;
                            int localPCS = 0;
                            int transitPCS = 0;
                            float localWeight = 0;
                            float transitWeight = 0;
                            float AcctepedWt = 0;

                            for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                            {
                                if (((DataTable)Session["MD"]).Rows[i]["DocumentType"].ToString() == strAWBType && gdvULDDetails.Rows[i].Cells[6].Text.Trim() != "")
                                {

                                    //if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
                                    //{
                                    string stPOU = Convert.ToString(Session["Dest"]);// arPOU[m].ToString();
                                    //if (stPOU == gdvULDDetails.Rows[i].Cells[2].Text)
                                    //{
                                    string strOrg = gdvULDDetails.Rows[i].Cells[16].Text.Trim();

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
                                    strDest = gdvULDDetails.Rows[i].Cells[4].Text.Trim();
                                    // }

                                    if (!ULDDestpt.Contains(gdvULDDetails.Rows[i].Cells[2].Text.Trim()))
                                    {
                                        //UnloadAt = gdvULDDetails.Rows[i].Cells[2].Text + "," + UnloadAt;
                                        ULDDestpt.Add(gdvULDDetails.Rows[i].Cells[2].Text);
                                        UnloadAt = (string)Session["FtlPOU"];
                                        TailNo = string.Empty;
                                        EGMNo = string.Empty;
                                    }
                                    string ULDNo = "BULK";
                                    ULDNo = gdvULDDetails.Rows[i].Cells[1].Text.Trim();
                                    string strAWBNo = gdvULDDetails.Rows[i].Cells[6].Text.Trim();

                                    // ULDDest = gdvULDDetails.Rows[i].Cells[4].Text;
                                    string strSCC = "";

                                    if (gdvULDDetails.Rows[i].Cells[7].Text == "&nbsp;")
                                    {
                                        strSCC = "0";
                                    }
                                    else
                                    {
                                        strSCC = gdvULDDetails.Rows[i].Cells[7].Text;
                                    }


                                    int strNoOfPCS = int.Parse(gdvULDDetails.Rows[i].Cells[8].Text);
                                    int actPcs = int.Parse(gdvULDDetails.Rows[i].Cells[13].Text);
                                    string strWeight = gdvULDDetails.Rows[i].Cells[9].Text;
                                    strTotalWeight = strTotalWeight + float.Parse(strWeight);
                                    strTotal = strTotal + strNoOfPCS;
                                    string AcceptPCS = gdvULDDetails.Rows[i].Cells[13].Text;
                                    string AcceptWGT = gdvULDDetails.Rows[i].Cells[14].Text;

                                    string BookedPcs = gdvULDDetails.Rows[i].Cells[11].Text;
                                    string BookedGwt = gdvULDDetails.Rows[i].Cells[12].Text;

                                    //Calculat Total Volume
                                    //if (gdvULDDetails.Rows[i].Cells[10].Text == "&nbsp;")
                                    //{
                                    //    VOL = 0;
                                    //}
                                    //else
                                    //{
                                    //    VOL = Convert.ToInt16(gdvULDDetails.Rows[i].Cells[10].Text);
                                    //}
                                    //AVLPCS = Convert.ToInt16(gdvULDDetails.Rows[i].Cells[11].Text);
                                    //AVLWGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[12].Text);
                                    string strDescription = "";
                                    if (gdvULDDetails.Rows[i].Cells[15].Text == "&nbsp;")
                                    {
                                        strDescription = "";
                                    }
                                    else
                                    {
                                        strDescription = gdvULDDetails.Rows[i].Cells[15].Text;
                                        strDescription = strDescription.Replace("&amp;", "&");
                                    }


                                    string strRemark = "";

                                    if (gdvULDDetails.Rows[i].Cells[20].Text == "&nbsp;")
                                    {
                                        strRemark = "";
                                    }
                                    else
                                    {
                                        strRemark = ((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text.ToString();

                                    }

                                    string IsBonded = gdvULDDetails.Rows[i].Cells[21].Text.Trim();

                                    if (strOrg != strLoadingPt)
                                    {
                                        DTTransitExport.Rows.Add(strAWBNo, strNoOfPCS, BookedPcs, strDescription, strWeight, strOrg, strDest, strNextflight, strSCC, strRemark, transitPCS, transitWeight, BookedGwt, ULDNo, IsBonded);

                                        transitPCS = transitPCS + strNoOfPCS;
                                        transitWeight = transitWeight + float.Parse(strWeight);
                                    }
                                    else
                                    {

                                        DTAWBDetails.Rows.Add(strAWBNo, strNoOfPCS, BookedPcs, strDescription, strWeight, strOrg, strDest, strNextflight, strSCC, strRemark, localPCS, localWeight, BookedGwt, ULDNo, IsBonded);
                                        localPCS = localPCS + strNoOfPCS;
                                        localWeight = localWeight + float.Parse(strWeight);
                                    }
                                    // }

                                }
                                // }
                            }


                            if (UnloadAt != "")
                            {
                                //UnloadAt = UnloadAt.Remove(UnloadAt.Length - 1, 1);
                                UnloadAt = (string)Session["FtlPOU"];
                                TailNo = string.Empty;
                                EGMNo = string.Empty;

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

                            DTExport.Rows.Add(strOwner, strNationality, strFlightNo, strLoadingPt, strUnloadPt, strTotalWeight, strPreparedBy, strTotal, UnloadAt, TailNo, EGMNo, Logo.ToArray());
                            Session["DTExport" + Iteration] = DTExport.Copy();
                            //  dsDTExport.Tables.Add(DTExport);

                            if (UnloadAt == "")
                            {

                                DTAWBDetails.Rows.Add("", "", "", "", "", "", "", "", "");


                                Session["DTAWBDetails" + Iteration] = DTAWBDetails.Copy();
                                // dsDTAWBDetails.Tables.Add(DTAWBDetails);

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
                //Response.Write("<script>");
                //Response.Write("window.open(" + query + ",'_blank')");
                //Response.Write("</script>");


                //ScriptManager.RegisterStartupScript(this, GetType(), "OpenWindow", "window.open(" + query1 + ");", true); IMP
                //ScriptManager.RegisterStartupScript(this, GetType(), "OpenWindow", "newwindow.focus();", true);   IMP

                return query1;

                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "newwindow.focus();", true);

                //Response.Redirect("ShowEMAWBULD.aspx",false);
                //Server.Transfer("ShowEMAWBULD.aspx", true);

                // Session["DTAWBDetails"] = "";
                // Session["DTExport"] = "";
                //}



            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Function to Show NOTC
        //private void showNOTOCData()
        //{
        //    try
        //    {

        //        // DataSet dsDTAWBDetails = new DataSet();
        //        //DataSet dsDTExport = new DataSet();
        //        ArrayList arPOU = new ArrayList();



        //        Session["DTNOTOC"] = null;
        //        Session["DTNOTCAWBs"] = null;
        //        //img for report
        //        System.IO.MemoryStream Logo = null;
        //        try
        //        {

        //            Logo = CommonUtility.GetImageStream(Page.Server);
        //        }
        //        catch (Exception ex)
        //        {
        //            Logo = new System.IO.MemoryStream();
        //        }
        //        //end
        //        DataTable DTNOTCDetails = new DataTable();

        //        DTNOTCDetails.Columns.Add("NotocID");
        //        DTNOTCDetails.Columns.Add("LoadingStation");
        //        DTNOTCDetails.Columns.Add("FlightNumber");
        //        DTNOTCDetails.Columns.Add("Date");
        //        DTNOTCDetails.Columns.Add("Registration");
        //        DTNOTCDetails.Columns.Add("PreparedBy");
        //        DTNOTCDetails.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));

        //        DateTime dt = new DateTime();
        //        string strNationality = "";
        //        try
        //        {
        //            //DateTime dt1 = new DateTime();
        //            dt = DateTime.ParseExact(txtfltdt.Text.Trim(), "dd/MM/yyyy", null);
        //            string strLoadingPt = Session["Station"].ToString();

        //            DataSet DsNationality = objExpMani.GetPOUAirlineSchedule(ddlFlightCode.Text + txtFlightID.Text, strLoadingPt, dt);


        //            strNationality = DsNationality.Tables[1].Rows[0][0].ToString();
        //        }
        //        catch (Exception ex)
        //        {

        //        }

        //        if (gdvULDDetails.Rows.Count >= 0)
        //        {
        //            DataRow drNotocDetails = DTNOTCDetails.NewRow();

        //            drNotocDetails["NotocID"] = "";
        //            drNotocDetails["LoadingStation"] = Session["Station"].ToString();
        //            drNotocDetails["FlightNumber"] = ddlFlightCode.Text + txtFlightID.Text;
        //            //drNotocDetails["Date"] = lblDate.Text;
        //            drNotocDetails["Registration"] = strNationality;
        //            drNotocDetails["PreparedBy"] = Session["Username"].ToString();
        //            drNotocDetails["Logo"] = Logo.ToArray();


        //            DTNOTCDetails.Rows.Add(drNotocDetails);
        //        }


        //        string strFlightNo = ddlFlightCode.Text + txtFlightID.Text + "        " + txtfltdt.Text;


        //        string strPreparedBy = Session["Username"].ToString();



        //        DataTable DTNOTOCAWBs = new DataTable();

        //        DTNOTOCAWBs.Columns.Add("POU");
        //        DTNOTOCAWBs.Columns.Add("AWBNo");
        //        DTNOTOCAWBs.Columns.Add("ShipperName");
        //        DTNOTOCAWBs.Columns.Add("DivisionClass1");
        //        DTNOTOCAWBs.Columns.Add("UNID");
        //        DTNOTOCAWBs.Columns.Add("SubRisk");
        //        DTNOTOCAWBs.Columns.Add("NoOfPkgs");
        //        DTNOTOCAWBs.Columns.Add("Quantity");
        //        DTNOTOCAWBs.Columns.Add("RadioActiveCategory");
        //        DTNOTOCAWBs.Columns.Add("PkgGroup");
        //        DTNOTOCAWBs.Columns.Add("CodeReverse");
        //        DTNOTOCAWBs.Columns.Add("CAO");
        //        DTNOTOCAWBs.Columns.Add("LoadedComp");



        //        DataTable DTNOTOCOtherLoad = new DataTable();

        //        DTNOTOCOtherLoad.Columns.Add("POU");
        //        DTNOTOCOtherLoad.Columns.Add("AirWayBillNumber");
        //        DTNOTOCOtherLoad.Columns.Add("Description");
        //        DTNOTOCOtherLoad.Columns.Add("NoOfPackages");
        //        DTNOTOCOtherLoad.Columns.Add("Quantity");
        //        DTNOTOCOtherLoad.Columns.Add("Information");
        //        DTNOTOCOtherLoad.Columns.Add("Code");
        //        DTNOTOCOtherLoad.Columns.Add("LoadingCompartment");
        //        DTNOTOCOtherLoad.Columns.Add("LaodedAsShown");
        //        DTNOTOCOtherLoad.Columns.Add("OtherInformation");

        //        if (gdvULDDetails.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
        //            {

        //                DataRow DtRow = DTNOTOCAWBs.NewRow();
        //                DtRow["POU"] = gdvULDDetails.Rows[i].Cells[2].Text;
        //                DtRow["AWBNo"] = gdvULDDetails.Rows[i].Cells[6].Text;
        //                string AWBNo = gdvULDDetails.Rows[i].Cells[6].Text;
        //                string DGRAWB = "";

        //                string UNID = "", DivisionClass = "", ShippingName = "", Subrisk = "", NoOfPkgs = "", Quantity = "", RadioCategory = "", PkgGroup = "", CodeReverse = "", CAO = "-", LoadingComp = "-";
        //                //For DGR Cargo
        //                try
        //                {
        //                    DataSet ds = objExpMani.GetDGRCargoDetails(AWBNo.Substring(AWBNo.Length - 8));
        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        DGRAWB = ds.Tables[0].Rows[0][0].ToString();
        //                        UNID = ds.Tables[0].Rows[0][1].ToString();
        //                        NoOfPkgs = ds.Tables[0].Rows[0][2].ToString();
        //                        ShippingName = ds.Tables[0].Rows[0][8].ToString();
        //                        Subrisk = ds.Tables[0].Rows[0][9].ToString();
        //                        RadioCategory = ds.Tables[0].Rows[0][5].ToString();
        //                        DivisionClass = ds.Tables[0].Rows[0][6].ToString();
        //                        PkgGroup = ds.Tables[0].Rows[0][7].ToString();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                }
        //                if (DGRAWB == AWBNo.Substring(AWBNo.Length - 8))
        //                {
        //                    DtRow["ShipperName"] = ShippingName;
        //                    DtRow["DivisionClass1"] = DivisionClass;
        //                    DtRow["UNID"] = UNID;
        //                    DtRow["SubRisk"] = Subrisk;
        //                    DtRow["NoOfPkgs"] = NoOfPkgs;
        //                    DtRow["Quantity"] = Quantity;
        //                    DtRow["RadioActiveCategory"] = RadioCategory;
        //                    DtRow["PkgGroup"] = PkgGroup;
        //                    DtRow["CodeReverse"] = CodeReverse;
        //                    DtRow["CAO"] = CAO;
        //                    DtRow["LoadedComp"] = LoadingComp;
        //                    DTNOTOCAWBs.Rows.Add(DtRow);
        //                }

        //                //Other Load
        //                string OtherAWBNo = "";
        //                string CommDesc = "";
        //                try
        //                {

        //                    DataSet ds = objExpMani.GetDGRCargoDetails(AWBNo.Substring(AWBNo.Length - 8));
        //                    if (ds.Tables[1].Rows.Count > 0)
        //                    {
        //                        OtherAWBNo = ds.Tables[1].Rows[0][0].ToString();
        //                        CommDesc = ds.Tables[1].Rows[0][2].ToString();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                }
        //                DataRow DtRowOther = DTNOTOCOtherLoad.NewRow();
        //                if (OtherAWBNo == AWBNo.Substring(AWBNo.Length - 8))
        //                {
        //                    DtRowOther["POU"] = gdvULDDetails.Rows[i].Cells[2].Text;
        //                    DtRowOther["AirWayBillNumber"] = OtherAWBNo;
        //                    DtRowOther["Description"] = CommDesc;
        //                    DtRowOther["NoOfPackages"] = gdvULDDetails.Rows[i].Cells[8].Text;
        //                    DtRowOther["Quantity"] = gdvULDDetails.Rows[i].Cells[9].Text;
        //                    DtRowOther["Information"] = "";
        //                    DtRowOther["Code"] = "";
        //                    DtRowOther["LoadingCompartment"] = "";
        //                    DtRowOther["LaodedAsShown"] = "";
        //                    DtRowOther["OtherInformation"] = "";

        //                    DTNOTOCOtherLoad.Rows.Add(DtRowOther);
        //                }


        //            }
        //        }






        //        Session["DTNOTOC"] = DTNOTCDetails;
        //        Session["DTNOTCAWBs"] = DTNOTOCAWBs;
        //        Session["DTNOtherCode"] = DTNOTOCOtherLoad;
        //        // SaveNilManifest(ddlFlightCode.Text + txtFlightID.Text, "Bulk", strLoadingPt, strUnloadPt, strPreparedBy, dt, DateTime.Now);


        //        string query = "'showNotocDoc.aspx'";
        //        //Response.Write("<script>");

        //        //Response.Write("window.open(" + query + ",'_blank')");
        //        //Response.Write("</script>");

        //        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open(" + query + ");", true);
        //        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "newwindow.focus();", true);



        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        #endregion

        protected void btnSplitUnassign_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                int check = 0;
                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check1")).Checked == true)
                    {
                        check = check + 1;
                    }

                }

                //for (int k = 0; k < gdvULDDetails.Rows.Count; k++)
                //{
                //    string txt = gdvULDDetails.Rows[k].Cells[1].Text;
                //    if (((CheckBox)gdvULDDetails.Rows[k].FindControl("Check1")).Checked == true)
                //    {
                //        if (bool.Parse(gdvULDDetails.Rows[k].Cells[18].Text))
                //        {
                //            lblStatus.ForeColor = Color.Red;
                //            lblStatus.Text = "This AWB is Manifested so we can't Split and UnAssign this AWB";
                //            return;
                //        }
                //    }

                //}

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
                //grdAWBs.Columns[5].Visible = false;

                //grdAWBs.Columns[6].Visible = false;
                UpdatePanelRouteDetails.Visible = false;

                DataTable dt = new DataTable();
                DataTable DTAWBDetails = new DataTable();

                DTAWBDetails.Columns.Add("AWBno");
                DTAWBDetails.Columns.Add("PCS");
                DTAWBDetails.Columns.Add("WT");
                //DTAWBDetails.Columns.Add("AvlPCS");
                //DTAWBDetails.Columns.Add("AvlWgt");


                for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[j].FindControl("Check1")).Checked == true)
                    {
                        DataRow dr;

                        //GridViewRow row = gdvULDLoadPlanAWB.Rows[j];
                        //dr = dt.NewRow();

                        // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                        //Changes  on 2 oct (   gdvULDDetails.Rows[j].Cells[11].Text to gdvULDDetails.Rows[j].Cells[8].Text)
                        dr = DTAWBDetails.NewRow();
                        dr[0] = ((Label)gdvULDDetails.Rows[j].FindControl("lblAWBNo")).Text;// gdvULDLoadPlanAWB.Rows[j].Cells[1].Text;
                        dr[1] = ((Label)gdvULDDetails.Rows[j].FindControl("lblPCS")).Text;//gdvULDLoadPlanAWB.Rows[j].Cells[2].Text;
                        dr[2] = ((Label)gdvULDDetails.Rows[j].FindControl("lblWt")).Text;
                        //dr[3] = gdvULDDetails.Rows[j].Cells[8].Text;
                        //dr[4] = gdvULDDetails.Rows[j].Cells[9].Text;

                        DTAWBDetails.Rows.Add(dr);
                    }
                }

                Session["AWBdata1"] = DTAWBDetails;

                //  Response.Write("<script>");
                //  Response.Write("window.open('ShowAddManifestAWB.aspx','_blank','left=0,top=0,width=600,height=450,toolbar=0,resizable=0')");
                //   Response.Write("</script>");
                //SqlBulkCopy sbc = new SqlBulkCopy(targetConnStr);
                //sbc.DestinationTableName = "yourDestinationTable";
                //sbc.WriteToServer(dt);
                //sbc.Close(); 




                pnlGrid.Visible = true;
                // pnlGrid.Attributes.item("style") = "Z-INDEX: 176; LEFT: 584px; POSITION: absolute; TOP: 176px";
                pnlGrid.Style["Z-INDEX"] = "150";
                pnlGrid.Style["LEFT"] = "342px";
                pnlGrid.Style["POSITION"] = "absolute";
                pnlGrid.Style["TOP"] = "320px";



                try
                {
                    LoadGridSchedule();
                    DataTable dt1 = DTAWBDetails;
                    DataTable dtCurrentTable = (DataTable)Session["AWBdata1"];
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
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtWT")).Text = dt1.Rows[i][2].ToString();
                            //((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = dt1.Rows[i][3].ToString();
                            //((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = dt1.Rows[i][4].ToString();

                            ViewState["CurrentTable1"] = dtCurrentTable;


                        }
                        // grdAWBs.DataSource = dtCurrentTable;
                        // grdAWBs.DataBind();

                    }
                    // grdAWBs.DataSource = dt;
                    //  grdAWBs.DataBind();


                    // myButton.Attributes.Add("onClick", "close();");
                    AllButtonStatus(false);


                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {
            }

        }

        protected void btnUnassign_Click(object sender, EventArgs e)
        {
            try
            {

                lblStatus.Text = "";
                int check = 0;
                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check1")).Checked == true)
                    {
                        check = check + 1;
                        if (((Label)gdvULDDetails.Rows[i].FindControl("lblTokenNumber")).Text.Trim() != "")
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "AWB can't be Un-Assigned after token generation.";                            
                            return;
                        }
                    }
                }
                
                if (check == 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select only One from Manifest TAB for UnAssign";
                    return;
                }
                if (check > 1)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select only One from Manifest TAB for UnAssign";
                    return;
                }
                
                bool blnFlag = false;
                DataTable DTAWBDetails = null;

                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {

                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check1")).Checked == true)
                    {
                        for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                        {
                            if (((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBNo")).Text == ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBNo")).Text)
                            {
                                if (gdvULDLoadPlanAWB.Rows[j].Visible == true)
                                {
                                    ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPCS")).Text = (Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPCS")).Text) + Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblPCS")).Text)).ToString();

                                    ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWT")).Text = (Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWT")).Text) + Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblWT")).Text)).ToString();
                                    //((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlPCS")).Text = (Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlPCS")).Text) + Convert.ToInt32(gdvULDDetails.Rows[i].Cells[11].Text)).ToString();
                                    //((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlWgt")).Text = (Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlWgt")).Text) + Convert.ToDouble(gdvULDDetails.Rows[i].Cells[12].Text)).ToString();
                                    blnFlag = true;
                                }
                                else
                                {
                                    ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPCS")).Text = ((Label)gdvULDDetails.Rows[j].FindControl("lblPCS")).Text;
                                    ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWT")).Text = ((Label)gdvULDDetails.Rows[j].FindControl("lblWT")).Text;
                                    //((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlPCS")).Text = gdvULDDetails.Rows[i].Cells[8].Text;
                                    //((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlWgt")).Text = gdvULDDetails.Rows[i].Cells[9].Text;

                                    //((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblTotWgt")).Text = gdvULDDetails.Rows[i].Cells[11].Text;
                                    //((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblTotWgt")).Text = gdvULDDetails.Rows[i].Cells[12].Text;

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
                                DTAWBDetails = new DataTable();

                                DTAWBDetails.Columns.Add("AWBNo");
                                DTAWBDetails.Columns.Add("PCS");
                                DTAWBDetails.Columns.Add("WT");
                                //DTAWBDetails.Columns.Add("AvlPCS");
                                //DTAWBDetails.Columns.Add("AvlWgt");
                                //DTAWBDetails.Columns.Add("BookedPCS");
                                //DTAWBDetails.Columns.Add("BookedWgt");
                            }


                            //for (int x = 0; x < gdvULDDetails.Rows.Count; x++)
                            //{
                            if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check1")).Checked == true)
                            {
                                DataRow dr;

                                //GridViewRow row = gdvULDLoadPlanAWB.Rows[j];
                                //dr = dt.NewRow();

                                // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                                dr = DTAWBDetails.NewRow();
                                dr[0] = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBNo")).Text; ;
                                dr[1] = Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblPCS")).Text);
                                dr[2] = Convert.ToDecimal(((Label)gdvULDDetails.Rows[i].FindControl("lblWT")).Text);


                                string AWBBNo = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBNo")).Text;
                                int pc = Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblPCS")).Text);
                                decimal wtt = Convert.ToDecimal(((Label)gdvULDDetails.Rows[i].FindControl("lblWT")).Text);
                                string tkn = ((Label)gdvULDDetails.Rows[i].FindControl("lblTokenNumber")).Text;

                                #region Parameter
                                string[] PName = new string[2];
                                PName[0] = "AWBBNO";
                                //PName[1] = "pc";
                                //PName[2] = "wtt";
                                PName[1] = "tkn";


                                object[] PValue = new object[2];

                                PValue[0] = AWBBNo;
                                //PValue[1] = pc;
                                //PValue[2] = wtt;
                                PValue[1] = tkn;



                                SqlDbType[] PType = new SqlDbType[2];
                                PType[0] = SqlDbType.VarChar;
                                //PType[1] = SqlDbType.Int;
                                //PType[2] = SqlDbType.Decimal;
                                PType[1] = SqlDbType.VarChar;


                                #endregion
                                DataSet ds = null;
                                try
                                {
                                    ds = da.SelectRecords("SPUpdateTKNGHA", PName, PValue, PType);
                                }
                                catch
                                {
                                    ds = null;
                                }
                                finally
                                {
                                    PName = null; PValue = null; PType = null;
                                }


                                //dr[3] = gdvULDDetails.Rows[i].Cells[13].Text;
                                //dr[4] = gdvULDDetails.Rows[i].Cells[14].Text;

                                //dr[6] = gdvULDDetails.Rows[i].Cells[11].Text;
                                //dr[7] = gdvULDDetails.Rows[i].Cells[12].Text;

                                DTAWBDetails.Rows.Add(dr);
                            }
                            //}

                            //}
                        }
                        //gdvULDDetails.Rows[i].Visible = false;

                        Session["AWBdata"] = DTAWBDetails;
                        if (DTAWBDetails != null)
                            LoadAWBDataGrid(DTAWBDetails);




                        DataTable dtULDDetails = (DataTable)Session["ManifestGridDatagha"];// (DataTable)Session["GDVULDDetails"];

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
                        DataSet testds = new DataSet();
                        testds.Tables.Add(DTAWBDetails.Copy());
                        Session["gdvULDLoadPlanAWB"] = testds;
                        //  DataTable dtManifest=(DataTable)Session["ManifestGridData"] ;

                        // AllButtonStatus(false);
                        //ShowULDAWBSummary();
                        hdnManifestFlag.Value = "1";
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSplitAssign_Click(object sender, EventArgs e)
        {
            try
            {
                int check = 0;
                for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                    {
                        check = check + 1;
                    }

                }
                if (check == 0)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select One ULD from AWB TAB for Split and Assign";
                    return;
                }
                if (check > 1)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select only One ULD from AWB TAB for Split and Assign";
                    return;
                }



                Session["Split"] = "A";
                btnAddManifest.Text = "Add to Manifest";
                lblNextFlight.Visible = false;
                txtNextFlight.Visible = false;

                grdAWBs.Columns[2].Visible = true;

                grdAWBs.Columns[3].Visible = true;
                //grdAWBs.Columns[5].Visible = false;

                //grdAWBs.Columns[6].Visible = false;



                UpdatePanelRouteDetails.Visible = false;
                lblNFltDate.Visible = false;
                txtNFltDate.Visible = false;

                lblReason.Visible = false;
                txtReason.Visible = false;

                DataTable dt = new DataTable();
                DataTable DTAWBDetails = new DataTable();

                DTAWBDetails.Columns.Add("AWBno");
                DTAWBDetails.Columns.Add("PCS");
                DTAWBDetails.Columns.Add("WT");
                //DTAWBDetails.Columns.Add("AvlPCS");
                //DTAWBDetails.Columns.Add("AvlWgt");


                for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
                    {
                        DataRow dr;

                        //GridViewRow row = gdvULDLoadPlanAWB.Rows[j];
                        //dr = dt.NewRow();

                        // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                        dr = DTAWBDetails.NewRow();
                        dr[0] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBNo")).Text;// gdvULDLoadPlanAWB.Rows[j].Cells[1].Text;
                        dr[1] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPCS")).Text;//gdvULDLoadPlanAWB.Rows[j].Cells[2].Text;
                        dr[2] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWt")).Text;
                        //dr[3] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlPCS")).Text;//((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlPCS")).Text;
                        //dr[4] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlWgt")).Text;//((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlWgt")).Text;

                        DTAWBDetails.Rows.Add(dr);



                    }
                }

                Session["AWBdata1"] = DTAWBDetails;

                //  Response.Write("<script>");
                //  Response.Write("window.open('ShowAddManifestAWB.aspx','_blank','left=0,top=0,width=600,height=450,toolbar=0,resizable=0')");
                //   Response.Write("</script>");
                //SqlBulkCopy sbc = new SqlBulkCopy(targetConnStr);
                //sbc.DestinationTableName = "yourDestinationTable";
                //sbc.WriteToServer(dt);
                //sbc.Close(); 




                pnlGrid.Visible = true;
                // pnlGrid.Attributes.item("style") = "Z-INDEX: 176; LEFT: 584px; POSITION: absolute; TOP: 176px";
                pnlGrid.Style["Z-INDEX"] = "150";
                pnlGrid.Style["LEFT"] = "342px";
                pnlGrid.Style["POSITION"] = "absolute";
                pnlGrid.Style["TOP"] = "260px";



                try
                {
                    LoadGridSchedule();
                    DataTable dt1 = DTAWBDetails;
                    DataTable dtCurrentTable = (DataTable)Session["AWBdata1"];
                    grdAWBs.DataSource = dtCurrentTable;
                    grdAWBs.DataBind();
                    if (dt1.Rows.Count > 0)
                    {
                        DataRow drCurrentRow = null;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            drCurrentRow = dtCurrentTable.NewRow();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBNo")).Text = dt1.Rows[i][0].ToString();

                            ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text = dt1.Rows[i][1].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtWT")).Text = dt1.Rows[i][2].ToString();
                            //((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = dt1.Rows[i][3].ToString();
                            //((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = dt1.Rows[i][4].ToString();

                            ViewState["CurrentTable1"] = dtCurrentTable;


                        }
                        // grdAWBs.DataSource = dtCurrentTable;
                        // grdAWBs.DataBind();

                    }

                    AllButtonStatus(false);
                    // grdAWBs.DataSource = dt;
                    //  grdAWBs.DataBind();


                    // myButton.Attributes.Add("onClick", "close();");


                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {
            }

        }

        public void ShowSplitAWBGrid()
        {
            try
            {
                DataTable dtSplitData = new DataTable();
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
        }

        protected void btnAddManifest_Click(object sender, EventArgs e)
        {
            try
            {
                Label1.Text = "";
                int PCS = 0;

                //Added on 28 sept 

                for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[j].FindControl("Check1")).Checked == true)
                    {
                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblPCS")).Text))
                        {
                            Label1.Text = "Please enter valid Pcs";
                            Label1.ForeColor = Color.Red;
                            return;


                        }
                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtWT")).Text) > Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblWT")).Text))
                        {
                            Label1.Text = "Please enter valid Weight";
                            Label1.ForeColor = Color.Red;
                            return;
                        }


                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblPCS")).Text))
                        {
                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtWT")).Text) < Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblWT")).Text))
                            {
                                Label1.Text = "Please enter valid Pcs";
                                Label1.ForeColor = Color.Red;
                                return;
                            }


                        }

                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtWT")).Text) == Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblWT")).Text))
                        {
                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblPCS")).Text))
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

                //Code starts for Return to Shipper

                //if (strMode == "RSI")
                //{
                //    BLExpManifest objExpManifest = new BLExpManifest();
                //    string ActualFltNo = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                //    DateTime ActualFltDate = DateTime.ParseExact(txtfltdt.Text.Trim(), "dd/MM/yyyy", null);
                //    string OffloadLoc = lblDepAirport.Text.Trim();
                //    string strAWBno = ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim().Substring(((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim().Length - 8, 8);
                //    int APCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text.Trim());
                //    double AWGT = double.Parse(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text.Trim());


                //    if (((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text.Trim() == "")
                //    {
                //        Label1.Text = "Please enter valid Pcs";
                //        Label1.ForeColor = Color.Red;
                //        return;
                //    }

                //    if (((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text.Trim() == "")
                //    {
                //        Label1.Text = "Please enter valid Weight";
                //        Label1.ForeColor = Color.Red;
                //        return;
                //    }


                //    int OffloadPCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text.Trim());
                //    OffloadWGT = double.Parse(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text.Trim());
                //    string UserName = Session["UserName"].ToString();

                //    string POLoading = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text;
                //    string POUnloading = ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text;


                //    if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text))
                //    {
                //        Label1.Text = "Please enter valid Pcs";
                //        Label1.ForeColor = Color.Red;
                //        return;
                //    }

                //    if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text))
                //    {
                //        Label1.Text = "Please enter valid Weight";
                //        Label1.ForeColor = Color.Red;
                //        return;
                //    }
                //    string PartnerCode = ((DropDownList)grdRouting.Rows[0].FindControl("ddlPartner")).SelectedValue;
                //    string Partnertype = ((DropDownList)grdRouting.Rows[0].FindControl("ddlPartnerType")).SelectedValue;




                //    bool blnResult = objExpManifest.OffLoadShipmentinManifest(ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim(), "", OffloadLoc, strAWBno, APCS, AWGT, OffloadPCS, OffloadWGT,
                //       UserName, POLoading, POUnloading, string.Empty, Convert.ToDateTime(Session["IT"]), "Return To Shipper", strMode, DateTime.ParseExact(txtfltdt.Text.Trim(), "dd/MM/yyyy", null), Convert.ToDateTime(Session["IT"]),PartnerCode,Partnertype);

                //    objExpManifest = null;

                //    if (blnResult)
                //    {
                //        pnlGrid.Visible = false;
                //        ////BtnList_Click(null, null);
                //        return;
                //    }
                //}

                //End of Return to shipper

                if (strMode == "U")
                {
                    //code to do the Split and Assign functionality. //Added by poorna.

                    //code to do the Split and Assign functionality. //Added by poorna.

                    //for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                    //{
                    //    if (((CheckBox)gdvULDDetails.Rows[j].FindControl("Check0")).Checked == true)
                    //    {
                    //        //if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblPCS")).Text))
                    //        //{
                    //        //    Label1.Text = "Please enter valid Pcs";
                    //        //    Label1.ForeColor = Color.Red;
                    //        //    return;
                    //        //}

                    //        //if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblWT")).Text))
                    //        //{
                    //        //    Label1.Text = "Please enter valid Weight";
                    //        //    Label1.ForeColor = Color.Red;
                    //        //    return;
                    //        //}

                    //        //if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblPCS")).Text))
                    //        //{
                    //        //    if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) < Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblWT")).Text))
                    //        //    {
                    //        //        Label1.Text = "Please enter valid Weight";
                    //        //        Label1.ForeColor = Color.Red;
                    //        //        return;
                    //        //    }
                    //        //}

                    //        //if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) == Convert.ToDouble(((Label)gdvULDDetails.Rows[j].FindControl("lblWT")).Text))
                    //        //{
                    //        //    if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(((Label)gdvULDDetails.Rows[j].FindControl("lblPCS")).Text))
                    //        //    {
                    //        //        Label1.Text = "Please enter valid PCS";
                    //        //        Label1.ForeColor = Color.Red;
                    //        //        return;
                    //        //    }
                    //        //}

                    //        break;
                    //    }
                    //}
                    SplitAndUnAssignfunction();
                    AllButtonStatus(true);
                    return;
                }

                //if (strMode == "O")
                //{
                //    //code to do the Split and Assign functionality. //Added by poorna.

                //    if (UpdatePanelRouteDetails.Visible == true)
                //    {
                //        int chkPCS = 0;
                //        double chkWGT = 0.0;
                //        for (int i = 0; i < grdRouting.Rows.Count; i++)
                //        {
                //            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == "")
                //            {
                //                Label1.Text = "Please enter valid Origin";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }

                //            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim() == "")
                //            {
                //                Label1.Text = "Please enter valid Destination";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }

                //            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim())
                //            {
                //                Label1.Text = "Please enter valid Origin & Destination";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }

                //            string Org = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                //            if (Org != lblDepAirport.Text)
                //            {
                //                Label1.Text = "Please enter valid Origin";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }

                //            if (((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim().ToUpper() != ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim().ToUpper())
                //            {
                //                Label1.Text = "Please enter valid Destination";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }

                //            if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == null)
                //            {
                //                Label1.Text = "Please enter valid PCS";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }

                //            if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == null)
                //            {
                //                Label1.Text = "Please enter valid PCS";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }

                //            if (((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == null)
                //            {
                //                Label1.Text = "Please enter valid Weight ";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }

                //            if (lblDepAirport.Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim())
                //            {
                //                chkPCS = chkPCS + Convert.ToInt32(((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim());
                //                chkWGT = chkWGT + Convert.ToDouble(((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim());
                //            }
                //            if (chkPCS > Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text))
                //            {
                //                Label1.Text = "Please enter valid PCS";
                //                Label1.ForeColor = Color.Red;
                //                return;

                //            }
                //            if (chkWGT > Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text))
                //            {
                //                Label1.Text = "Please enter valid Weight";
                //                Label1.ForeColor = Color.Red;
                //                return;

                //            }

                //            if (chkPCS == 0)
                //            {
                //                Label1.Text = "Please enter valid PCS ";
                //                Label1.ForeColor = Color.Red;
                //                return;

                //            }
                //            if (chkWGT == 0)
                //            {
                //                Label1.Text = "Please enter valid Weight ";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }

                //            if (chkPCS == Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text.Trim()) &&
                //                chkWGT != Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text.Trim()))
                //            {
                //                Label1.Text = "Please enter valid Weight ";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }

                //            if (
                //                chkWGT == Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text.Trim()) &&
                //                chkPCS != Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text.Trim()))
                //            {
                //                Label1.Text = "Please enter valid Pieces";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }

                //            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text != "" && ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
                //            {
                //                Label1.ForeColor = Color.Red;
                //                Label1.Text = "Please enter Next Flight Details.";
                //                return;
                //            }

                //            if (ddlReason.SelectedValue == "Others" && txtReason.Text == "")
                //            {
                //                Label1.Text = "Please enter Offload reason";
                //                Label1.ForeColor = Color.Red;
                //                return;


                //            }

                //        }
                //    }



                //Verify the entered flight number



                //    if (txtNextFlight.Text.ToUpper().Trim() != "")
                //    {
                //        string Origin = Convert.ToString(Session["Station"]);
                //        string FltNo = txtNextFlight.Text.ToUpper().Trim();
                //        string Dest = string.Empty;
                //        DateTime dt = DateTime.ParseExact(txtNFltDate.Text.Trim(), "dd/MM/yyyy", null);

                //        int diff = (dt - dtCurrentDate.Date).Days;

                //        for (int ICount = 0; ICount < gdvULDDetails.Rows.Count; ICount++)
                //        {
                //            if (((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim() == gdvULDDetails.Rows[ICount].Cells[6].Text.Trim())
                //            {
                //                Dest = gdvULDDetails.Rows[ICount].Cells[4].Text.Trim();
                //                break;
                //            }
                //        }

                //        ShowFlightsBAL objBAL = new ShowFlightsBAL();
                //        DataSet objDS = null;
                //        string errormessage = string.Empty;
                //        string flightID = ddlFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();
                //        string AWBNo = ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim();
                //        AWBNo = AWBNo.Substring(AWBNo.Length - 8);
                //        objBAL.GetFlightListForManifest(Origin, Dest, diff, ref objDS, ref errormessage, dtCurrentDate.Date, AWBNo, flightID, DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null));
                //        bool blnExists = false;

                //        if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                //        {
                //            for (int ICount = 0; ICount < objDS.Tables[0].Rows.Count; ICount++)
                //            {
                //                if (FltNo == objDS.Tables[0].Rows[ICount]["FltNumber"].ToString().Trim())
                //                {
                //                    blnExists = true;
                //                    break;
                //                }
                //            }
                //        }

                //        objBAL = null;
                //        objDS = null;

                //        if (blnExists == false)
                //        {
                //            Label1.ForeColor = Color.Red;
                //            Label1.Text = "Please enter valid flight number.";
                //            return;
                //        }
                //    }
                //    //

                //    SaveOffLoadDetails();
                //    AllButtonStatus(true);
                //    return;
                //}

                //if (strMode == "R")
                //{
                //    //if (txtNextFlight.Text.ToUpper().Trim() == "" || txtNFltDate.Text.Trim() == "")
                //    //{
                //    //    Label1.ForeColor = Color.Red;
                //    //    Label1.Text = "Please enter Next Flight Details.";

                //    //    return;
                //    //}

                //    if (UpdatePanelRouteDetails.Visible == true)
                //    {
                //        int chkPCS = 0;
                //        double chkWGT = 0.0;
                //        for (int i = 0; i < grdRouting.Rows.Count; i++)
                //        {
                //            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text == "Select")
                //            {
                //                Label1.Text = "Please enter valid flight";
                //                Label1.ForeColor = Color.Red;
                //                return;


                //            }
                //            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == "")
                //            {
                //                Label1.Text = "Please enter valid Origin";
                //                Label1.ForeColor = Color.Red;
                //                return;


                //            }
                //            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim() == "")
                //            {
                //                Label1.Text = "Please enter valid Destination";
                //                Label1.ForeColor = Color.Red;
                //                return;


                //            }
                //            if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim())
                //            {
                //                Label1.Text = "Please enter valid Origin & Destination";
                //                Label1.ForeColor = Color.Red;
                //                return;


                //            }
                //            string Org = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
                //            if (Org != lblDepAirport.Text)
                //            {
                //                Label1.Text = "Please enter valid Origin";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }
                //            if (((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim() != ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim())
                //            {
                //                Label1.Text = "Please enter valid Destination";
                //                Label1.ForeColor = Color.Red;
                //                return;


                //            }



                //            if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == null)
                //            {
                //                Label1.Text = "Please enter valid PCS";
                //                Label1.ForeColor = Color.Red;
                //                return;


                //            }


                //            if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == null)
                //            {
                //                Label1.Text = "Please enter valid PCS";
                //                Label1.ForeColor = Color.Red;
                //                return;


                //            }
                //            if (((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == null)
                //            {
                //                Label1.Text = "Please enter valid Weight ";
                //                Label1.ForeColor = Color.Red;
                //                return;


                //            }

                //            if (lblDepAirport.Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim())
                //            {
                //                chkPCS = chkPCS + Convert.ToInt32(((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim());
                //                chkWGT = chkWGT + Convert.ToDouble(((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim());
                //            }
                //            if (chkPCS > Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text))
                //            {
                //                Label1.Text = "Please enter valid PCS";
                //                Label1.ForeColor = Color.Red;
                //                return;

                //            }
                //            if (chkWGT > Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text))
                //            {
                //                Label1.Text = "Please enter valid Weight";
                //                Label1.ForeColor = Color.Red;
                //                return;

                //            }

                //            if (chkPCS == 0)
                //            {
                //                Label1.Text = "Please enter valid PCS ";
                //                Label1.ForeColor = Color.Red;
                //                return;

                //            }
                //            if (chkWGT == 0)
                //            {
                //                Label1.Text = "Please enter valid Weight ";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }


                //            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text != "" && ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
                //            {
                //                Label1.ForeColor = Color.Red;
                //                Label1.Text = "Please enter Next Flight Details.";
                //                return;
                //            }
                //            if (ddlReason.SelectedValue == "Others" && txtReason.Text == "")
                //            {
                //                Label1.Text = "Please enter Offload reason";
                //                Label1.ForeColor = Color.Red;
                //                return;


                //            }

                //        }
                //    }



                //Verify the entered flight number



                //    if (txtNextFlight.Text.ToUpper().Trim() != "")
                //    {
                //        string Origin = Convert.ToString(Session["Station"]);
                //        string FltNo = txtNextFlight.Text.ToUpper().Trim();
                //        string Dest = Convert.ToString(Session["Dest"]);/// ddlMainPOU.SelectedItem.Value.ToString();
                //        DateTime dt = DateTime.ParseExact(txtNFltDate.Text.Trim(), "dd/MM/yyyy", null);

                //        int diff = (dt - dtCurrentDate.Date).Days;

                //        ShowFlightsBAL objBAL = new ShowFlightsBAL();
                //        DataSet objDS = null;
                //        string errormessage = string.Empty;
                //        string flightID = ddlFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();

                //        string AWBNo = ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim();
                //        AWBNo = AWBNo.Substring(AWBNo.Length - 8);

                //        objBAL.GetFlightListForManifest(Origin, Dest, diff, ref objDS, ref errormessage, dtCurrentDate.Date, AWBNo, flightID, DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null));
                //        bool blnExists = false;

                //        if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                //        {
                //            for (int ICount = 0; ICount < objDS.Tables[0].Rows.Count; ICount++)
                //            {
                //                if (FltNo == objDS.Tables[0].Rows[ICount]["FltNumber"].ToString())
                //                {
                //                    blnExists = true;
                //                    break;
                //                }
                //            }
                //        }

                //        if (blnExists == false)
                //        {
                //            Label1.ForeColor = Color.Red;
                //            Label1.Text = "Please enter valid flight number.";
                //            return;
                //        }
                //    }
                //    //

                //    SaveOffLoadDetails();
                //    AllButtonStatus(true);
                //    return;
                //}





                //if (strMode == "RS")
                //{


                //    for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                //    {
                //        //  if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
                //        if (((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text == ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text)
                //        {
                //            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                //            {
                //                Label1.Text = "Please enter valid PCS";
                //                Label1.ForeColor = Color.Red;
                //                return;


                //            }
                //            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                //            {
                //                Label1.Text = "Please enter valid PCS ";
                //                Label1.ForeColor = Color.Red;
                //                return;
                //            }

                //            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                //            {
                //                if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) < Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                //                {
                //                    Label1.Text = "Please enter valid Weight";
                //                    Label1.ForeColor = Color.Red;
                //                    return;
                //                }


                //            }

                //            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) == Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                //            {
                //                if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                //                {
                //                    Label1.Text = "Please enter valid PCS";
                //                    Label1.ForeColor = Color.Red;
                //                    return;


                //                }
                //            }


                //            break;
                //        }
                //    }


                //    SaveAddToShipper();
                //    AllButtonStatus(true);
                //    return;
                //}


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
                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPCS")).Text))
                        {
                            Label1.Text = "Please enter valid Pcs";
                            Label1.ForeColor = Color.Red;
                            return;


                        }
                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtWT")).Text) > Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWT")).Text))
                        {
                            Label1.Text = "Please enter valid Pcs";
                            Label1.ForeColor = Color.Red;
                            return;
                        }

                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPCS")).Text))
                        {
                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtWT")).Text) < Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWT")).Text))
                            {
                                Label1.Text = "Please enter valid Weight";
                                Label1.ForeColor = Color.Red;
                                return;
                            }


                        }

                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtWT")).Text) == Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWT")).Text))
                        {
                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPCS")).Text))
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


                FLTno = ddlFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();
                POL = Convert.ToString(Session["Station"]);
                //POU = ddlMainPOU.SelectedItem.Value.ToString();
                POU = string.Empty;
                Updatedby = Convert.ToString(Session["Username"]);
                ULDno = "Bulk";
                ULDwgt = 0;
                string IsBonded = string.Empty, AWBType = string.Empty;

                DateTime FlightDate = DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null);

                DataSet dsawbData = objExpMani.GetAwbTabdetails(lblDepAirport.Text, FLTno, FlightDate);
                DataTable dsawb1 = new DataTable();
                DataSet dssess = null;
                if (Session["gdvULDLoadPlanAWB"] != null)
                {
                    dssess = (DataSet)Session["gdvULDLoadPlanAWB"];
                    dsawb1 = (DataTable)dssess.Tables[0];
                    //dsawb1 = (DataTable)Session["gdvULDLoadPlanAWB"];
                }
                else
                {

                    dsawb1 = dsawbData.Tables[0];
                }


                for (int i = 0; i < grdAWBs.Rows.Count; i++)
                {
                    //if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                    //{
                    //    ((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked = false;
                    //   gdvULDLoadPlanAWB.Rows[i].Visible = false;
                    //Appended 25apr
                    AWBno = ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBNo")).Text;

                    PCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text);
                    WGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtWT")).Text);
                    // AVLPCS = Convert.ToInt16(((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text);
                    //AVLWGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text);

                    for (int k = 0; k < dsawb1.Rows.Count; k++)
                    {
                        if (((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAWBNo")).Text == AWBno)
                        {
                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPCS")).Text))
                            {
                                if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtWT")).Text) < Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWT")).Text))
                                {
                                    Label1.Text = "Please enter valid Weight";
                                    Label1.ForeColor = Color.Red;
                                    return;
                                }
                            }

                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtWT")).Text) == Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWT")).Text))
                            {
                                if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPCS")).Text))
                                {
                                    Label1.Text = "Please enter valid Pcs";
                                    Label1.ForeColor = Color.Red;
                                    return;


                                }
                            }

                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPCS")).Text = (int.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPCS")).Text) - PCS).ToString();
                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWT")).Text = (double.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWT")).Text) - WGT).ToString();
                            //AVLPCS = Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlPCS")).Text);
                            //AVLWGT = Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlWgt")).Text);

                            AWBPcs = Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPCS")).Text);
                            AWBGwt = Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWT")).Text);

                            dsawb1.Rows[k][1] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPCS")).Text;
                            dsawb1.Rows[k][2] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWT")).Text;
                            //dsawbData.Tables[1].Rows[k][1] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPCS")).Text;
                            //dsawbData.Tables[1].Rows[k][2] = ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWT")).Text;
                            // Updatedon = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                            try
                            {
                                //for (int m = 0; m < dsawb1.Rows.Count; m++)
                                //{
                                //    //if (dsawb1.Rows[m][0].ToString() == AWBno)
                                //    if (dsawbData.Tables[1].Rows[m][0].ToString() == AWBno)
                                //    {
                                //        AWBDest = dsawbData.Tables[1].Rows[m][5].ToString();
                                //        Desc = dsawbData.Tables[1].Rows[m][6].ToString();
                                //        Vol = dsawbData.Tables[1].Rows[m][7].ToString();
                                //        SCC = dsawbData.Tables[1].Rows[m][6].ToString();
                                //        CommCode = dsawbData.Tables[1].Rows[m]["CommodityCode"].ToString();
                                //        IsBonded = dsawbData.Tables[1].Rows[m]["IsBonded"].ToString();
                                //        AWBType = dsawbData.Tables[1].Rows[m]["Type"].ToString();
                                //        break;
                                //    }
                                //}

                                //adding collected data to gridview rows one by one
                            }
                            catch (Exception ex)
                            {
                            }


                            if (int.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPCS")).Text) == 0)
                            {
                                //((CheckBox)gdvULDLoadPlanAWB.Rows[k].FindControl("Check2")).Checked = false;
                                //gdvULDLoadPlanAWB.Rows[k].Visible = false;

                                //Added on 27 sept
                                //dsawb1.Tables[1].Rows.
                                dsawb1.Rows[k].Delete();
                                //dsawbData.Tables[1].Rows[k].Delete();

                                dsawb1.AcceptChanges();
                                //dsawbData.Tables[1].AcceptChanges();                                
                            }
                        }
                    }

                    tabMenifestDetails = (DataTable)Session["ManifestGridData"];

                    DataTable tabMenifestDetailsGHAadd = new DataTable();
                    int duplicnt = 0;
                    bool ismatch = false;
                    if (Session["grdulddetails"] != null)
                    {
                        tabMenifestDetailsGHAadd = (DataTable)Session["grdulddetails"];


                        for (int chk = 0; chk < tabMenifestDetailsGHAadd.Rows.Count; chk++)
                        {
                            if (tabMenifestDetailsGHAadd.Rows[chk]["AWBNumber"].ToString() == AWBno)
                            {
                                tabMenifestDetailsGHAadd.Rows[chk]["PCS"] = (int.Parse(tabMenifestDetailsGHAadd.Rows[chk]["PCS"].ToString()) + PCS).ToString();
                                tabMenifestDetailsGHAadd.Rows[chk]["GrossWgt"] = (double.Parse(tabMenifestDetailsGHAadd.Rows[chk]["WT"].ToString()) + WGT).ToString();
                                duplicnt = duplicnt + 1;
                                ismatch = true;
                            }
                        }
                    }
                    if (ismatch == false)
                    {

                        DataRow l_Datarow = tabMenifestDetailsGHAadd.NewRow();
                        bool IsManifest = false;

                        //l_Datarow["ULDno"] = ULDno;
                        //l_Datarow["POU"] = POU;
                        //l_Datarow["POL"] = POL;
                        //l_Datarow["AWBDest"] = AWBDest;
                        //l_Datarow["Counter"] = AWBType;
                        l_Datarow["AWBNumber"] = AWBno;
                        //l_Datarow["SCC"] = CommCode;
                        l_Datarow["PCS"] = PCS;
                        l_Datarow["WT"] = WGT;
                        l_Datarow["FlightNo"] = "";
                        l_Datarow["FlightDt"] = "";
                        l_Datarow["DriverName"] = "";

                        l_Datarow["DLNumber"] = "";
                        l_Datarow["Phone"] = "";

                        l_Datarow["VehicleNo"] = "";
                        l_Datarow["TokenNumber"] = "";
                        l_Datarow["TokenDate"] = "";
                        //l_Datarow["Manifested"] = IsManifest;
                        //l_Datarow["LoadingPriority"] = LoadingPriority;

                        //l_Datarow["Remark"] = Remark;
                        //l_Datarow["Bonded"] = IsBonded;

                        tabMenifestDetailsGHAadd.Rows.Add(l_Datarow);
                    }

                    if (tabMenifestDetailsGHAadd != null && tabMenifestDetailsGHAadd.Rows.Count > 0)
                    {
                        Session["ManifestGridDatagha"] = tabMenifestDetailsGHAadd;
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
                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPCS")).Text = dsawb1.Rows[k][1].ToString();
                            ((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWT")).Text = dsawb1.Rows[k][2].ToString();
                            //((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlPCS")).Text = dsawb1.Rows[k][3].ToString();
                            //((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlWgt")).Text = dsawb1.Rows[k][4].ToString();

                            //((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblTotPCS")).Text = dsawb1.Rows[k]["AWBPcs"].ToString();
                            //((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblTotWgt")).Text = dsawb1.Rows[k]["AWBGwt"].ToString();

                            //MemDetails.Rows.Add(l_Datarow);

                            //gdvULDLoadPlanAWB.DataSource = (DataTable)MemDetails;
                            //gdvULDLoadPlanAWB.DataBind();

                        }
                    }
                    Session["gdvULDLoadPlanAWB"] = dsawb1;// dsawbData.Tables[1];

                    //if (tabMenifestDetails.Rows[i][0].ToString() == "Bulk")
                    //{
                    //    totalAWB = 1 + totalAWB;
                    //    totalAWBPCS = totalAWBPCS + int.Parse(tabMenifestDetails.Rows[i][7].ToString());
                    //    totalAWBWt = totalAWBWt + int.Parse(tabMenifestDetails.Rows[i][8].ToString());
                    //    totalAWBVol = totalAWBVol + int.Parse(tabMenifestDetails.Rows[i][9].ToString());
                    //}
                    //else
                    //{
                    //    TotalULD = 1 + TotalULD;
                    //    totalULDAWB = 1 + totalULDAWB;
                    //    totalULDPCS = totalULDPCS + int.Parse(tabMenifestDetails.Rows[i][7].ToString());
                    //    totalULDWt = totalULDWt + int.Parse(tabMenifestDetails.Rows[i][8].ToString());
                    //    totalULDVol = totalULDVol + int.Parse(tabMenifestDetails.Rows[i][9].ToString());

                    //}

                    gdvULDDetails.DataSource = "";

                    gdvULDDetails.DataSource = (DataTable)Session["ManifestGridDatagha"];
                    gdvULDDetails.DataBind();


                    // }
                }



                try
                {
                    //for (int k = 0; k < gdvULDDetails.Rows.Count; k++)
                    //{
                    //    if (gdvULDDetails.Rows[k].Cells[6].Text.Trim() == tabMenifestDetails.Rows[k]["AWBNo"].ToString())
                    //    {
                    //        if (((TextBox)gdvULDDetails.Rows[k].FindControl("txtRemark")).Text == "")
                    //        {
                    //            ((TextBox)gdvULDDetails.Rows[k].FindControl("txtRemark")).Text = tabMenifestDetails.Rows[k]["Remark"].ToString();

                    //            ((DropDownList)gdvULDDetails.Rows[k].FindControl("ddlLoadingPriority")).SelectedValue = tabMenifestDetails.Rows[k]["LoadingPriority"].ToString();
                    //        }
                    //    }



                    //}

                    //for (int m = 0; m < gdvULDDetails.Rows.Count; m++)
                    //{
                    //    for (int r = 0; r < dsawbData.Tables[2].Rows.Count; r++)
                    //    {
                    //        if (dsawbData.Tables[2].Rows.Count > 0)
                    //        {
                    //            string AWB = gdvULDDetails.Rows[m].Cells[6].Text.Trim();
                    //            if (dsawbData.Tables[2].Rows[r]["AWBNo"].ToString() == AWB.Substring(AWB.Length - 8))//AWBno.SubString(AWB.Length-8)) 
                    //            {
                    //                Remark = dsawbData.Tables[2].Rows[r]["Remarks"].ToString();
                    //                ((TextBox)gdvULDDetails.Rows[m].FindControl("txtRemark")).Text = Remark;
                    //            }


                    //        }
                    //    }

                    //}
                }
                catch (Exception ex)
                {
                }




                //for (int j = 0; j < tabMenifestDetails.Rows.Count; j++)
                //{
                //    if (tabMenifestDetails.Rows[j][0].ToString() == "Bulk")
                //    {

                //        totalAWB = 1 + totalAWB;
                //        totalAWBPCS = totalAWBPCS + int.Parse(tabMenifestDetails.Rows[j][7].ToString());
                //        totalAWBWt = totalAWBWt + float.Parse(tabMenifestDetails.Rows[j][8].ToString());

                //        totalAWBVol = totalAWBVol + int.Parse(tabMenifestDetails.Rows[j][9].ToString());

                //    }

                //}

                //ShowULDAWBSummary();
                hdnManifestFlag.Value = "1";
                //Session[""] = (DataTable);
            }
            catch (Exception ex)
            { }

            pnlGrid.Visible = false;

            AllButtonStatus(true);
        }

        #region Loadgrid Intial Row
        private void LoadGridSchedule()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AWBno";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "PCS";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "WT";
            myDataTable.Columns.Add(myDataColumn);

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "AvlPCS";
            //myDataTable.Columns.Add(myDataColumn);

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "AvlWgt";
            //myDataTable.Columns.Add(myDataColumn);


            DataRow dr;
            dr = myDataTable.NewRow();
            //dr["RowNumber"] = 1;
            dr["AWBno"] = "";//"5";
            dr["PCS"] = "";// "5";
            dr["WT"] = "";
            //dr["AvlPCS"] = "";
            //dr["AvlWgt"] = "";

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
        }
        #endregion

        public bool IsInputValid()
        {
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

                        DataTable tabMenifestDetails = (DataTable)Session["ManifestGridData"];


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


                        DataTable dtAWB = (DataTable)Session["AWBdata"];

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

                            //grdAWBs.r
                            //grdAWBs.DataBind();
                        }
                        catch (Exception ex)
                        {

                        }



                        //DataTable dtAWB =;// (DataTable)Session["AWBdata1"];

                        //pcscount = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text);

                        //try
                        //{
                        //    for (int j = 0; j < dtAWB.Rows.Count; j++)
                        //    {
                        //        if (((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text == dtAWB.Rows[j][0].ToString())
                        //        {
                        //            if (int.Parse(dtAWB.Rows[j][1].ToString()) < pcscount)
                        //            {
                        //                Label1.Text = "Enter valid pcscount row:" + (i + 1);
                        //                Label1.ForeColor = Color.Red;
                        //                ((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text = dtAWB.Rows[j][1].ToString();
                        //                return false;

                        //            }
                        //            else
                        //            {

                        //                int AvlPCS = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text);
                        //                AvlPCS = int.Parse(dtAWB.Rows[j][1].ToString()) - pcscount;
                        //                ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = AvlPCS.ToString();

                        //                int AvlWeight = (int.Parse(dtAWB.Rows[j][2].ToString())) / (int.Parse(dtAWB.Rows[j][1].ToString()));
                        //                AvlWeight = AvlWeight * AvlPCS;
                        //                ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = AvlWeight.ToString();
                        //                int ActWeight = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text);
                        //                ActWeight = ActWeight - AvlWeight;
                        //                ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = ActWeight.ToString();
                        //            }

                        //        }
                        //    }
                        //    //grdAWBs.r
                        //    //grdAWBs.DataBind();
                        //}
                        //catch (Exception ex)
                        //{

                        //}
                    }
                    //if (pcscount > int.Parse(LBLPcsCount.Text))
                    //{
                    //    LBLStatus.Text = "Total pcs count should be smaller than " + LBLPcsCount.Text;
                    //    LBLStatus.ForeColor = Color.Red;
                    //    return false;
                    //}

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
        }

        //protected void txtPcs_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        //if (!IsInputValid())
        //        //{
        //        //    //  TXTVolume.Text = "";
        //        //    // TXTTotal.Text = "";
        //        //    return;
        //        //}


        //        int PCS = 0;
        //        decimal Weight = 0;

        //        //TextBox txt=(TextBox)grdAWBs.Rows[](FindControl("").

        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
        //    }
        //}

        protected void btnShowEAWB_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                pnlGrid.Visible = false;
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
        //protected void btnSendFFM_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string FlightNumber = ddlFlightCode.Text.ToString() + txtFlightID.Text.ToString();
        //        DataSet ds = new DataSet();
        //        BALEmailID ObjEmail = new BALEmailID();
        //        try
        //        {
        //            ds = ObjEmail.GetEmail(lblDepAirport.Text, Session["Dest"].ToString(), "FFM", FlightNumber, ddlFlightCode.Text.ToString());
        //            txtEmailID.Text = ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
        //            lblMsgCommType.Text = ds.Tables[0].Rows[0]["MsgCommType"].ToString();
        //            if (lblMsgCommType.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) || lblMsgCommType.Text.Equals("SITA", StringComparison.OrdinalIgnoreCase))
        //            {
        //                GenerateSITAHeader(ds.Tables[0].Rows[0]["PartnerSITAiD"].ToString());
        //            }
        //        }
        //        catch (Exception ex) { }
        //        ffmmsg = "ffm";
        //        lblMsg.Text = "FFM";
        //        txtMessageBody.Text = cls_BL.EncodeFFM(Session["POLairport"].ToString(), Session["FltNumber"].ToString(), Session["Fltdate"].ToString());
        //        Session["ffmmsg"] = "ffm";
        //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);

        //    }
        //    catch (Exception)
        //    { }
        //}
        #endregion

        private void LoadAWBDataGrid(DataTable pi_objDataTable)
        {
            try
            {
                DataTable MemDetails = new DataTable(); // (DataTable)Session["AWBdata"];
                if (MemDetails == null)
                    MemDetails = new DataTable();

                //MemDetails.Columns.Add("AWBNumber");
                //MemDetails.Columns.Add("PiecesCount");
                //MemDetails.Columns.Add("GrossWeight");
                ////MemDetails.Columns.Add("lblAvlPCS");
                //////MemDetails.Columns.Add("lblCommodity");
                ////MemDetails.Columns.Add("lblAvlWgt");
                ////MemDetails.Columns.Add("lblTotPCS");
                ////MemDetails.Columns.Add("lblTotWgt");

                ////MemDetails.Rows.Add(MemDetails.NewRow());
                //for (int j = 0; j < pi_objDataTable.Rows.Count; j++)
                //{
                //    DataRow l_Datarow = MemDetails.NewRow();

                //    MemDetails.Rows.Add(l_Datarow);
                //}
                gdvULDLoadPlanAWB.DataSource = (DataTable)pi_objDataTable;
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
                    // gdvULDLoadPlanAWB.DataSource = dsawb;


                    for (int i = 0; i < pi_objDataTable.Rows.Count; i++)
                    {

                        //  tabMenifestDetails.Rows.Add(l_Datarow);
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAWBNo")).Text = pi_objDataTable.Rows[i][0].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblPCS")).Text = pi_objDataTable.Rows[i][1].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblWt")).Text = pi_objDataTable.Rows[i][2].ToString();
                        //((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlPCS")).Text = pi_objDataTable.Rows[i][3].ToString();
                        //((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlWgt")).Text = pi_objDataTable.Rows[i][4].ToString();

                        //((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotPCS")).Text = pi_objDataTable.Rows[i][6].ToString();
                        //((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotWgt")).Text = pi_objDataTable.Rows[i][7].ToString();

                        //MemDetails.Rows.Add(l_Datarow);

                        //gdvULDLoadPlanAWB.DataSource = (DataTable)MemDetails;
                        //gdvULDLoadPlanAWB.DataBind();

                    }
                    Session["AWBdata"] = pi_objDataTable;
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void SplitAndUnAssignfunction()
        {
            try
            {
                string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby = "", AWBDest = "";
                int PCS = 0, AVLPCS = 0;
                double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0;
                //string Updatedon = "";
                string LoadingPriority = "", Remark = "";

                DataSet dtULD = (DataSet)Session["GDVULDDetails"];
                DataTable dtULDDetails = new DataTable();
                if (dtULD != null)
                    dtULDDetails = dtULD.Tables[0];
                //DataTable ManifestData = (DataTable)Session["ManifestGridDatagha"];
                DataTable ManifestData = new DataTable();
                if (Session["ManifestGridDatagha"] != null)
                {
                    ManifestData = (DataTable)Session["ManifestGridDatagha"];
                    //if (ManiData != null)
                    //    ManifestData = ManiData.Tables[0];
                }
                FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                POL = Convert.ToString(Session["Station"]);
                //POU = ddlMainPOU.SelectedItem.Value.ToString();
                POU = string.Empty;
                Updatedby = Convert.ToString(Session["Username"]);
                ULDno = "Bulk";
                ULDwgt = 0;


                PCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text);
                WGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtWT")).Text);
                //DataSet dsawb1 = objExpMani.GetAwbTabdetails(lblDepAirport.Text, FLTno);

                DataTable DTAWBDetails = (DataTable)Session["AWBdata"];

                if (DTAWBDetails == null)
                {
                    DTAWBDetails = new DataTable();

                    DTAWBDetails.Columns.Add("AWBNumber");
                    DTAWBDetails.Columns.Add("PiecesCount");
                    DTAWBDetails.Columns.Add("GrossWeight");
                    //DTAWBDetails.Columns.Add("AvlPCS");
                    //DTAWBDetails.Columns.Add("AvlWgt");

                    //DTAWBDetails.Columns.Add("BookedPCS");
                    //DTAWBDetails.Columns.Add("BookedWgt");
                }

                DataRow drNew = null;
                for (int i = 0; i < grdAWBs.Rows.Count; i++)
                {
                    /* Comment for Split and unassign match AWB 
                    drNew = DTAWBDetails.NewRow();
                    DTAWBDetails.Rows.Add(drNew);
                    */

                    //if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                    //{
                    //    ((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked = false;
                    //   gdvULDLoadPlanAWB.Rows[i].Visible = false;
                    //Appended 25apr
                    bool ismacth = false;
                    int actPCS = 0;
                    double Actwgt = 0;
                    int AWBPcs = 0;
                    double AWBWT = 0;


                    for (int k = 0; k < DTAWBDetails.Rows.Count; k++)
                    {
                        ///if (DTAWBDetails.Rows[k][6] == AWBno)
                        if (Convert.ToString(DTAWBDetails.Rows[k][0]) == ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text)
                        {
                            string PCScountshow = (Convert.ToInt32(DTAWBDetails.Rows[k][1]) + PCS).ToString();
                            string WGTCntShow = (double.Parse(DTAWBDetails.Rows[k][2].ToString()) + WGT).ToString();

                            DTAWBDetails.Rows[k][1] = PCScountshow;
                            DTAWBDetails.Rows[k][2] = WGTCntShow;

                            ismacth = true;

                            //if (int.Parse(gdvULDDetails.Rows[t].Cells[8].Text) == 0)
                            //{
                            //    ((CheckBox)gdvULDDetails.Rows[t].FindControl("Check0")).Checked = false;
                            //    gdvULDDetails.Rows[t].Visible = false;
                            //}
                        }
                    }
                    for (int t = 0; t < gdvULDDetails.Rows.Count; t++)
                    {

                        if (((Label)gdvULDDetails.Rows[0].FindControl("lblAWBNo")).Text == ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text)
                        {
                            ((Label)gdvULDDetails.Rows[0].FindControl("lblPCS")).Text = (Convert.ToInt32(((Label)gdvULDDetails.Rows[0].FindControl("lblPCS")).Text) - PCS).ToString();
                            ((Label)gdvULDDetails.Rows[0].FindControl("lblWT")).Text = (double.Parse(((Label)gdvULDDetails.Rows[0].FindControl("lblWT")).Text) - WGT).ToString();

                            DataRow dr1;

                            //////GridViewRow row = gdvULDLoadPlanAWB.Rows[j];
                            //////dr = dt.NewRow();

                            ////// DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                            //////Changes  on 2 oct (   gdvULDDetails.Rows[j].Cells[11].Text to gdvULDDetails.Rows[j].Cells[8].Text)
                            //dr1 = ManifestData.NewRow();
                            ////dr1[0] = "";//((Label)gdvULDDetails.Rows[j].FindControl("lblAWBNo")).Text;// gdvULDLoadPlanAWB.Rows[j].Cells[1].Text;
                            //dr1["PiecesCount"] = 0;//((Label)gdvULDDetails.Rows[0].FindControl("lblPCS")).Text;//gdvULDLoadPlanAWB.Rows[j].Cells[2].Text;
                            //dr1["GrossWeight"] = 0;//((Label)gdvULDDetails.Rows[0].FindControl("lblWT")).Text;

                            //ManifestData.Rows.Add(dr1);
                            try
                            {
                                ManifestData.Rows[t][1] = ((Label)gdvULDDetails.Rows[0].FindControl("lblPCS")).Text;
                                ManifestData.Rows[t][2] = ((Label)gdvULDDetails.Rows[0].FindControl("lblWT")).Text;
                                actPCS = Convert.ToInt32(((Label)gdvULDDetails.Rows[0].FindControl("lblPCS")).Text);
                                Actwgt = double.Parse(((Label)gdvULDDetails.Rows[0].FindControl("lblWT")).Text);

                            }
                            catch (Exception ex)
                            {
                            }
                            //AWBPcs = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[11].Text.Trim());
                            //AWBWT = double.Parse(gdvULDDetails.Rows[i].Cells[12].Text.Trim());
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
                        WGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtWT")).Text);

                        DTAWBDetails.Rows[count - 1][0] = AWBno;
                        DTAWBDetails.Rows[count - 1][1] = PCS;
                        DTAWBDetails.Rows[count - 1][2] = WGT;
                        //DTAWBDetails.Rows[count - 1][3] = actPCS;

                        //DTAWBDetails.Rows[count - 1][4] = Actwgt;
                        //DTAWBDetails.Rows[count - 1][6] = AWBPcs;
                        //DTAWBDetails.Rows[count - 1][7] = AWBWT;
                    }

                    LoadAWBDataGrid(DTAWBDetails);

                    //for (int l = 0; l < gdvULDDetails.Rows.Count; l++)
                    //{
                    for (int l = 0; l < ManifestData.Rows.Count; l++)
                    {
                        if (((TextBox)grdAWBs.Rows[l].FindControl("txtAWBno")).Text == ((Label)gdvULDDetails.Rows[l].FindControl("lblAWBNo")).Text)
                        {
                            /* comment on 2 oct for adding row in grid
                            drNew[0] = ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text;
                            drNew[1] = Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text);
                            drNew[2] = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text);
                            drNew[3] = Convert.ToInt32(gdvULDDetails.Rows[l].Cells[11].Text);
                            drNew[4] = Convert.ToDouble(gdvULDDetails.Rows[l].Cells[12].Text);
                            */






                            int UnassignPCS = Convert.ToInt32(((Label)gdvULDDetails.Rows[l].FindControl("lblPCS")).Text);
                            //(Convert.ToInt32(gdvULDDetails.Rows[l].Cells[8].Text) - Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text));
                            //Addded on 26 Sept Delete particular row from grid
                            if (UnassignPCS <= 0)
                            {
                                ManifestData.Rows[l].Delete();


                            }
                            else
                            {

                                ////string PCScunt = (Convert.ToInt32(gdvULDDetails.Rows[l].Cells[8].Text) - Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text)).ToString();

                                ////ManifestData.Rows[l][7] = PCScunt;// (Convert.ToInt32(gdvULDDetails.Rows[l].Cells[8].Text) - Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text)).ToString();

                                ////string WEIGGTCNT = (Convert.ToDouble(gdvULDDetails.Rows[l].Cells[12].Text) - Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text)).ToString();


                                ////ManifestData.Rows[l][8] = WEIGGTCNT;
                                //gdvULDDetails.Rows[l].Cells[8].Text = (Convert.ToInt32(gdvULDDetails.Rows[l].Cells[8].Text) - Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text)).ToString();
                                //gdvULDDetails.Rows[l].Cells[9].Text = (Convert.ToDouble(gdvULDDetails.Rows[l].Cells[12].Text) - Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text)).ToString();


                            }


                        }

                    }

                    ManifestData.AcceptChanges();

                    //Session["ManifestGridDatagha"] = ManifestData;
                    Session["ManifestGridDatagha"] = ManifestData;

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

            }
            catch (Exception ex)
            { }

            pnlGrid.Visible = false;
        }

        //protected void btnOffload_Click(object sender, EventArgs e)
        //{
        //    try
        //    {                
        //        int check = 0;
        //        for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
        //        {
        //            if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
        //            {
        //                check = check + 1;
        //            }

        //        }
        //        for (int k = 0; k < gdvULDDetails.Rows.Count; k++)
        //        {
        //            string txt = gdvULDDetails.Rows[k].Cells[18].Text;
        //            if (((CheckBox)gdvULDDetails.Rows[k].FindControl("Check0")).Checked == true)
        //            {
        //                if (!bool.Parse(gdvULDDetails.Rows[k].Cells[18].Text))
        //                {
        //                    lblStatus.ForeColor = Color.Red;
        //                    lblStatus.Text = "This AWB is not Manifested so we can't Offload this AWB (PCS)";
        //                    return;
        //                }
        //            }

        //        }

        //        if (check == 0)
        //        {
        //            lblStatus.ForeColor = Color.Red;
        //            lblStatus.Text = "Please Select One ULD from Manifested TAB for Offload";
        //            return;
        //        }
        //        if (check > 1)
        //        {
        //            lblStatus.ForeColor = Color.Red;
        //            lblStatus.Text = "Please Select only One ULD from Manifest TAB for Offload";
        //            return;
        //        }

        //        //Added by Poorna
        //        Session["Split"] = "O";
        //        btnAddManifest.Text = "Save";
        //        //lblNextFlight.Visible = true;
        //        //txtNextFlight.Visible = true;

        //        //lblNFltDate.Visible = true;
        //        //txtNFltDate.Visible = true;

        //        LoadAirlineCode("");
        //        UpdatePartnerCode(1);
        //        # region Code to get Offload reasons


        //        DataSet ds = objExpMani.GetOffloadReasons();
        //        {
        //            try
        //            {
        //                if (ds.Tables.Count > 0)
        //                {
        //                    if (ds.Tables[0].Rows.Count > 0)
        //                    {
        //                        txtReason.Enabled = false;
        //                    }
        //                    else
        //                    {
        //                        txtReason.Enabled = true;
        //                    }
        //                }
        //                else
        //                {
        //                    txtReason.Enabled = true;
        //                }
        //                ddlReason.DataTextField = "Reason";
        //                ds.Tables[0].Rows.Add("Others");
        //                ddlReason.DataSource = ds.Tables[0];
        //                ddlReason.DataBind();
        //                ddlReason.SelectedIndex = 0;

        //            }
        //            catch (Exception ex) { }
        //        }
        //        #endregion


        //        lblReason.Visible = true;
        //     //   txtReason.Visible = true;
        //        grdAWBs.Columns[2].Visible = false;

        //        grdAWBs.Columns[3].Visible = false;
        //        grdAWBs.Columns[5].Visible = true;

        //        grdAWBs.Columns[6].Visible = true;


        //        DataTable dt = new DataTable();
        //        DataTable DTAWBDetails = new DataTable();

        //        DTAWBDetails.Columns.Add("AWBNo");
        //        DTAWBDetails.Columns.Add("Pieces");
        //        DTAWBDetails.Columns.Add("Weight");
        //        DTAWBDetails.Columns.Add("AvlPCS");
        //        DTAWBDetails.Columns.Add("AvlWgt");
        //        DTAWBDetails.Columns.Add("Origin");
        //        DTAWBDetails.Columns.Add("Destination");


        //        for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
        //        {
        //            if (((CheckBox)gdvULDDetails.Rows[j].FindControl("Check0")).Checked == true)
        //            {
        //                DataRow dr;

        //                //GridViewRow row = gdvULDLoadPlanAWB.Rows[j];
        //                //dr = dt.NewRow();

        //                // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
        //                dr = DTAWBDetails.NewRow();
        //                string POL = gdvULDDetails.Rows[j].Cells[16].Text;
        //                string POU = gdvULDDetails.Rows[j].Cells[17].Text;
        //                string AWBNo = gdvULDDetails.Rows[j].Cells[6].Text;

        //                dr[0] = gdvULDDetails.Rows[j].Cells[6].Text;
        //                dr[1] = gdvULDDetails.Rows[j].Cells[8].Text;
        //                dr[2] = gdvULDDetails.Rows[j].Cells[9].Text;
        //                dr[3] = gdvULDDetails.Rows[j].Cells[8].Text;
        //                dr[4] = gdvULDDetails.Rows[j].Cells[9].Text;
        //                if (Session["dsMani"] != null)
        //                {
        //                    DataSet dsManifest = (DataSet)Session["dsMani"];
        //                    if (dsManifest.Tables[2].Rows.Count > 0)
        //                    {
        //                        for (int k = 0; k < dsManifest.Tables[2].Rows.Count; k++)
        //                        {
        //                            if (dsManifest.Tables[2].Rows[k]["AWBNumber"].ToString() == AWBNo)
        //                            {
        //                                POL = dsManifest.Tables[2].Rows[k]["POL"].ToString();
        //                                POU = dsManifest.Tables[2].Rows[k]["POU"].ToString();
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //                dr[5] = POL;
        //                dr[6] = POU;
        //                DTAWBDetails.Rows.Add(dr);
        //                break;
        //            }
        //        }

        //        Session["AWBdata"] = DTAWBDetails;

        //        txtReason.Text = "";

        //        pnlGrid.Visible = true;
        //        UpdatePanelRouteDetails.Visible = true;

        //        if (UpdatePanelRouteDetails.Visible == true)
        //        {
        //            for (int i = 0; i < grdRouting.Rows.Count; i++)
        //            {
        //                ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text = "";

        //                ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text = "";

        //                ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text = "";

        //            }
        //        }
        //        // pnlGrid.Attributes.item("style") = "Z-INDEX: 176; LEFT: 584px; POSITION: absolute; TOP: 176px";
        //        pnlGrid.Style["Z-INDEX"] = "150";
        //        pnlGrid.Style["LEFT"] = "342px";
        //        pnlGrid.Style["POSITION"] = "absolute";
        //        pnlGrid.Style["TOP"] = "260px";
        //        pnlGrid.Style["Width"] = "700px";


        //        try
        //        {

        //            //DTAWBDetails.Columns.Add("AWBNo");
        //            //DTAWBDetails.Columns.Add("Pieces");
        //            //DTAWBDetails.Columns.Add("Weight");


        //            LoadGridSchedule();
        //            DataTable dt1 = DTAWBDetails;
        //            DataTable dtCurrentTable = (DataTable)Session["AWBdata"];
        //            grdAWBs.DataSource = dtCurrentTable;
        //            grdAWBs.DataSource = dtCurrentTable;
        //            grdAWBs.DataBind();



        //            if (dt1.Rows.Count > 0)
        //            {
        //                DataRow drCurrentRow = null;
        //                for (int i = 0; i < dt1.Rows.Count; i++)
        //                {
        //                    drCurrentRow = dtCurrentTable.NewRow();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text = dt1.Rows[i][0].ToString();

        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text = dt1.Rows[i][1].ToString();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = dt1.Rows[i][2].ToString();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = dt1.Rows[i][3].ToString();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = dt1.Rows[i][4].ToString();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtOrigin")).Text = dt1.Rows[i][5].ToString();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtDestination")).Text = dt1.Rows[i][6].ToString();

        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Enabled = false;

        //                    ViewState["CurrentTable1"] = dtCurrentTable;
        //                }
        //                // grdAWBs.DataSource = dtCurrentTable;
        //                // grdAWBs.DataBind();

        //            }
        //            // grdAWBs.DataSource = dt;
        //            //  grdAWBs.DataBind();

        //            //btnOffload.Attributes.Add("onClick", "showDiv('toggle');");
        //         //   ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>return select();</script>", false);

        //            // myButton.Attributes.Add("onClick", "close();");


        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        ////BtnList_Click(null, null);
        //        AllButtonStatus(false);

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void SaveAddToShipper()
        //{
        //    try
        //    {
        //        string AWBno = "", POU = "", POL = "", ActFLTno = "", Updatedby = "", OffLoadFltNo = "", OffloadLoc = "";
        //        int OffloadPCS = 0, AVLPCS = 0;
        //        double OffloadWGT = 0.0, AVLWGT = 0.0;
        //        DateTime dtOffLoadDate;
        //        string strMode = string.Empty;


        //        ActFLTno = ddlFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();
        //        POL = Convert.ToString(Session["Station"]);
        //        // POU = ddlMainPOU.SelectedItem.Value.ToString();
        //        POU = string.Empty;
        //        Updatedby = Convert.ToString(Session["Username"]);
        //        OffLoadFltNo = txtNextFlight.Text.ToUpper().Trim();
        //        OffloadLoc = Convert.ToString(Session["Station"]);

        //        //strMode = (string)Session["Split"];


        //        dtOffLoadDate = dtCurrentDate;

        //        //For Arrival Date 
        //        for (int i = 0; i < grdAWBs.Rows.Count; i++)
        //        {
        //            AWBno = ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text;


        //            OffloadPCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text);
        //            OffloadWGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text);
        //            AVLPCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text);
        //            AVLWGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text);
        //        }

        //        for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
        //        {
        //            //  if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
        //            if (((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text == AWBno)
        //            {
        //                if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
        //                {
        //                    Label1.Text = "Please enter valid Pcs to return to Shipper";
        //                    Label1.ForeColor = Color.Red;
        //                    return;


        //                }
        //                if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
        //                {
        //                    Label1.Text = "Please enter valid Pcs to return to Shipper";
        //                    Label1.ForeColor = Color.Red;
        //                    return;
        //                }

        //                if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
        //                {
        //                    if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) < Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
        //                    {
        //                        Label1.Text = "Please enter valid Weight";
        //                        Label1.ForeColor = Color.Red;
        //                        return;
        //                    }


        //                }

        //                if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) == Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
        //                {
        //                    if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
        //                    {
        //                        Label1.Text = "Please enter valid Pcs to return to Shipper";
        //                        Label1.ForeColor = Color.Red;
        //                        return;


        //                    }
        //                }


        //                break;
        //            }
        //        }

        //        AWBno = AWBno.Substring(AWBno.Length - 8);

        //        //Changing Manifest pcs and manifest wt
        //        for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
        //        {
        //            if (((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim() == gdvULDDetails.Rows[j].Cells[6].Text.Trim())
        //            {

        //                AVLPCS = Convert.ToInt32(gdvULDDetails.Rows[j].Cells[13].Text);
        //                AVLWGT = Convert.ToDouble(gdvULDDetails.Rows[j].Cells[14].Text);

        //            }

        //        }

        //        BLExpManifest objExpManifest = new BLExpManifest();

        //        bool blnResult = objExpManifest.ReturnToShipper(ActFLTno, ActFLTno, OffloadLoc, AWBno, AVLPCS, AVLWGT, OffloadPCS, OffloadWGT,
        //            Updatedby, POL, POU, string.Empty, dtOffLoadDate, txtReason.Text.Trim(), strMode);

        //        if (blnResult)
        //        {
        //            pnlGrid.Visible = false;
        //            //BtnList_Click(null, null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}


        //private void SaveOffLoadDetails()
        //{
        //    try
        //    {
        //        string AWBno = "", POU = "", POL = "", ActFLTno = "", Updatedby = "", OffLoadFltNo = "", OffloadLoc = "";
        //        int OffloadPCS = 0, AVLPCS = 0;
        //        double OffloadWGT = 0.0, AVLWGT = 0.0;
        //        DateTime dtOffLoadDate;
        //        string strMode = string.Empty;


        //        ActFLTno = ddlFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();
        //        //POL = Convert.ToString(Session["Station"]);
        //        ////POU = ddlMainPOU.SelectedItem.Value.ToString();
        //        //POU = ddlMainPOU.Items[0].Value.ToString();
        //        //Updatedby = Convert.ToString(Session["Username"]);
        //        //OffLoadFltNo = txtNextFlight.Text.ToUpper().Trim();
        //        //OffloadLoc = Convert.ToString(Session["Station"]);
        //        strMode = (string)Session["Split"];

        //        if (strMode != "R")
        //        {
        //            if (UpdatePanelRouteDetails.Visible == true)
        //            {
        //                int chkPCS = 0;
        //                double chkWGT = 0.0;
        //                for (int i = 0; i < grdRouting.Rows.Count; i++)
        //                {
        //                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == "")
        //                    {
        //                        Label1.Text = "Please enter valid Origin";
        //                        Label1.ForeColor = Color.Red;
        //                        return;


        //                    }
        //                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim() == "")
        //                    {
        //                        Label1.Text = "Please enter valid Destination";
        //                        Label1.ForeColor = Color.Red;
        //                        return;


        //                    }

        //                    if (((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text.Trim())
        //                    {
        //                        Label1.Text = "Please enter valid Origin & Destination";
        //                        Label1.ForeColor = Color.Red;
        //                        return;


        //                    }
        //                    string Org = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text.Trim();
        //                    if (Org != lblDepAirport.Text)
        //                    {
        //                        Label1.Text = "Please enter valid Origin";
        //                        Label1.ForeColor = Color.Red;
        //                        return;
        //                    }
        //                    if (((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltDest")).Text.Trim().ToUpper() != ((TextBox)grdAWBs.Rows[0].FindControl("txtDestination")).Text.Trim().ToUpper())
        //                    {
        //                        Label1.Text = "Please enter valid Destination";
        //                        Label1.ForeColor = Color.Red;
        //                        return;


        //                    }



        //                    if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == null)
        //                    {
        //                        Label1.Text = "Please enter valid PCS";
        //                        Label1.ForeColor = Color.Red;
        //                        return;


        //                    }


        //                    if (((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim() == null)
        //                    {
        //                        Label1.Text = "Please enter valid PCS";
        //                        Label1.ForeColor = Color.Red;
        //                        return;


        //                    }
        //                    if (((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == "" || ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim() == null)
        //                    {
        //                        Label1.Text = "Please enter valid Weight ";
        //                        Label1.ForeColor = Color.Red;
        //                        return;


        //                    }

        //                    if (lblDepAirport.Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim())
        //                    {
        //                        chkPCS = chkPCS + Convert.ToInt32(((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim());
        //                        chkWGT = chkWGT + Convert.ToDouble(((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim());
        //                    }
        //                    if (chkPCS > Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text))
        //                    {
        //                        Label1.Text = "Please enter valid PCS";
        //                        Label1.ForeColor = Color.Red;
        //                        return;

        //                    }
        //                    if (chkWGT > Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text))
        //                    {
        //                        Label1.Text = "Please enter valid Weight";
        //                        Label1.ForeColor = Color.Red;
        //                        return;

        //                    }

        //                    if (chkPCS == 0)
        //                    {
        //                        Label1.Text = "Please enter valid PCS ";
        //                        Label1.ForeColor = Color.Red;
        //                        return;

        //                    }
        //                    if (chkWGT == 0)
        //                    {
        //                        Label1.Text = "Please enter valid Weight ";
        //                        Label1.ForeColor = Color.Red;
        //                        return;
        //                    }


        //                    if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text != "" && ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
        //                    {
        //                        Label1.ForeColor = Color.Red;
        //                        Label1.Text = "Please enter Next Flight Details.";
        //                        return;
        //                    }

        //                    if (ddlReason.SelectedValue == "Others" && txtReason.Text=="")
        //                    {
        //                        Label1.Text = "Please enter Offload reason";
        //                        Label1.ForeColor = Color.Red;
        //                        return;


        //                    }

        //                }
        //            }


        //        }






        //        //For Arrival Date 
        //        for (int i = 0; i < grdAWBs.Rows.Count; i++)
        //        {
        //            AWBno = ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text;
        //            AWBno = AWBno.Substring(AWBno.Length - 8);

        //            //OffloadPCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text);
        //            //OffloadWGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text);
        //            AVLPCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text);
        //            AVLWGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text);
        //        }




        //        //Changing Manifest pcs and manifest wt
        //        for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
        //        {
        //            if (((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim() == gdvULDDetails.Rows[j].Cells[6].Text.Trim())
        //            {

        //                AVLPCS = Convert.ToInt32(gdvULDDetails.Rows[j].Cells[13].Text);
        //                AVLWGT = Convert.ToDouble(gdvULDDetails.Rows[j].Cells[14].Text);

        //            }

        //        }

        //        DateTime dtActualDate = DateTime.ParseExact(txtfltdt.Text.Trim(), "dd/MM/yyyy", null);


        //        BLExpManifest objExpManifest = new BLExpManifest();
        //        bool blnResult = false;


        //        for (int i = 0; i < grdRouting.Rows.Count; i++)
        //        {


        //            POL = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
        //            //POU = ddlMainPOU.SelectedItem.Value.ToString();
        //            POU = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text; ;
        //            Updatedby = Convert.ToString(Session["Username"]);

        //            OffLoadFltNo = ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text;
        //            if (OffLoadFltNo == "Select")
        //                OffLoadFltNo = "";




        //            OffloadLoc = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
        //            string strDate = ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text;
        //            //Checks Nextflightdate is blank or not
        //            //Swapnil--------------------------------------------------------
        //            HomeBL objHome = new HomeBL();
        //            int RoleId = Convert.ToInt32(Session["RoleID"]);
        //            DataSet objDS = objHome.GetUserPermissions(((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath, RoleId);
        //            objHome = null;

        //            if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
        //            {
        //                for (int j = 0; j < objDS.Tables[0].Rows.Count; j++)
        //                {
        //                    if (objDS.Tables[0].Rows[j]["ControlId"].ToString() == "OffloadForSuperUser")
        //                    {
        //                        ChkSuperUserOffload = true;
        //                    }
        //                }
        //            }
        //            objDS = null;


        //            //----------------------------------------------------------------

        //            if (ChkSuperUserOffload == false)
        //            {
        //                if (strDate != "")
        //                {
        //                    if (DateTime.ParseExact(((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text, "dd/MM/yyyy", null) < dtCurrentDate.Date.AddDays(-2))
        //                    {
        //                        Label1.ForeColor = Color.Red;
        //                        Label1.Text = "We can't assign Offloaded PCS to previous day flight";
        //                        return;
        //                    }
        //                }
        //            }


        //            if (strDate == "")
        //                dtOffLoadDate = dtCurrentDate;//DateTime.Now;
        //            else
        //                dtOffLoadDate = DateTime.ParseExact(((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text, "dd/MM/yyyy", null);



        //            if (lblDepAirport.Text.Trim() == ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text.Trim())
        //            {
        //                OffloadPCS = OffloadPCS + Convert.ToInt32(((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text.Trim());
        //                OffloadWGT = OffloadWGT + Convert.ToDouble(((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text.Trim());
        //            }

        //            string Reason = "";
        //            if (ddlReason.SelectedItem.Text.Trim() == "Select")
        //            {
        //                lblStatus.Text = "Please select Reason";
        //                lblStatus.ForeColor = Color.Red;
        //                return;
        //            }
        //            if (txtReason.Text.Trim() != "")
        //            {
        //                Reason = txtReason.Text.Trim();
        //            }
        //            else
        //            {
        //                Reason = ddlReason.SelectedItem.Text.Trim();
        //            }

        //            string PartnerCode = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedValue;
        //            string Partnertype = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedValue;

        //            if (PartnerCode=="Other")
        //                OffLoadFltNo = ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text; 

        //            blnResult = objExpManifest.OffLoadShipmentinManifest(ActFLTno, OffLoadFltNo, OffloadLoc, AWBno, AVLPCS, AVLWGT, OffloadPCS, OffloadWGT,
        //               Updatedby, POL, POU, string.Empty, dtOffLoadDate, Reason, strMode, dtActualDate, dtCurrentDate,PartnerCode,Partnertype);

        //        }
        //        if (blnResult)
        //        {
        //            pnlGrid.Visible = false;
        //            //BtnList_Click(null, null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //protected void btnDepartFit_Click(object sender, EventArgs e)
        //{
        //    if (Session["IsManifested"] != null && (bool)Session["IsManifested"] == false)
        //    {


        //        UpdateDepartedFlight("D");

        //        BtnAddtoManifest.Enabled = false;
        //        btnSplitAssign.Enabled = false;

        //        //Add Records for Tracking purpose

        //        bool res = false;

        //        for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
        //        {
        //            //if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
        //            //{
        //            string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby, counter = "", SCC = "", Desc = "";
        //            int PCS = 0, AVLPCS = 0;
        //            double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, VOL = 0;
        //            string Updatedon = "";
        //            string LoadingPriority = "", Remark = "", ULDDest = "";

        //            FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
        //            Updatedby = Convert.ToString(Session["Username"]);

        //            ULDno = gdvULDDetails.Rows[i].Cells[1].Text;
        //            POU = gdvULDDetails.Rows[i].Cells[2].Text;
        //            POL = gdvULDDetails.Rows[i].Cells[3].Text;
        //            ULDDest = gdvULDDetails.Rows[i].Cells[4].Text;
        //            if (gdvULDDetails.Rows[i].Cells[5].Text == "&nbsp;")
        //            {
        //                counter = "0";
        //            }
        //            else
        //            {
        //                counter = gdvULDDetails.Rows[i].Cells[5].Text;
        //            }
        //            AWBno = gdvULDDetails.Rows[i].Cells[6].Text;
        //            if (gdvULDDetails.Rows[i].Cells[7].Text == "&nbsp;")
        //            {
        //                SCC = "0";
        //            }
        //            else
        //            {
        //                SCC = gdvULDDetails.Rows[i].Cells[7].Text;
        //            }


        //            PCS = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[8].Text);
        //            WGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[9].Text);
        //            if (gdvULDDetails.Rows[i].Cells[10].Text == "&nbsp;")
        //            {
        //                VOL = 0;
        //            }
        //            else
        //            {
        //                VOL = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[10].Text);
        //            }
        //            AVLPCS = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[13].Text);
        //            AVLWGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[14].Text);
        //            if (gdvULDDetails.Rows[i].Cells[15].Text == "&nbsp;")
        //            {
        //                Desc = "";
        //            }
        //            else
        //            {
        //                Desc = gdvULDDetails.Rows[i].Cells[15].Text;
        //            }

        //            if (gdvULDDetails.Rows[i].Cells[19].Text == "&nbsp;")
        //            {
        //                LoadingPriority = "";
        //            }
        //            else
        //            {
        //                LoadingPriority = ((DropDownList)gdvULDDetails.Rows[i].FindControl("ddlLoadingPriority")).SelectedItem.Text.ToString();
        //            }
        //            bool IsManifest = false;
        //            if ((string)Session["IsDeparted"] == "D")
        //            {
        //                IsManifest = true;
        //            }
        //            if (gdvULDDetails.Rows[i].Cells[20].Text == "&nbsp;")
        //            {
        //                Remark = "";
        //            }
        //            else
        //            {
        //                Remark = ((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text.ToString();
        //            }


        //            // Updatedon = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        //            Updatedon = dtCurrentDate.ToString("yyyy/MM/dd");//DateTime.Now.ToString("yyyy/MM/dd");
        //            DateTime dtflightdate = DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null);
        //            //res = objExpMani.CommitManifestdata(FLTno, ULDno, POU, POL, ULDDest, counter, AWBno, SCC, VOL, PCS, WGT, AVLPCS, AVLWGT, Desc, LoadingPriority, Remark, Updatedby, Updatedon, dtflightdate);

        //            //Add Records for Tracking purpose

        //            Object[] AWBDetails = new object[6];
        //            int intI = 0;
        //            int intDep = 0;

        //            AWBDetails.SetValue(AWBno, intI);
        //            intI++;

        //            AWBDetails.SetValue(PCS, intI);
        //            intI++;

        //            AWBDetails.SetValue(FLTno, intI);
        //            intI++;

        //            intDep = intI;
        //            AWBDetails.SetValue("Departed", intI);
        //            intI++;

        //            AWBDetails.SetValue(WGT, intI);
        //            intI++;

        //            AWBDetails.SetValue("", intI);
        //            intI++;

        //            AWBMilestonesBAL objMileStone = new AWBMilestonesBAL();
        //            //if (IsManifest == false && res == true)
        //            // {

        //            string strResult = objMileStone.AddAWBMaifestMilestone(AWBDetails);
        //            // }
        //            //if ((string)Session["IsDeparted"] == "R")
        //            //{
        //            //    string strResult = objMileStone.AddAWBMilestone(AWBDetails);
        //            //}
        //            //Departed
        //            //AWBDetails.SetValue("Departed", intDep);
        //            //strResult = objMileStone.AddAWBMilestone(AWBDetails);

        //            objMileStone = null;
        //            AWBDetails = null;

        //        }


        //       // btnSendFFM_Click(null, null);
        //        //Vijay - Code to generate Walk-in customer invoice
        //        //GenerateWalkinAgentInvoiceNumber();
        //    }
        //    else
        //    {
        //        lblStatus.ForeColor = Color.Red;
        //        lblStatus.Text = "Please Manifest Flight";
        //    }
        //}

        //protected void GenerateWalkinAgentInvoiceNumber()
        //{
        //    string strFltdate;
        //    #region Prepare Parameters
        //    object[] AwbRateInfo = new object[2];
        //    int i = 0;

        //    AwbRateInfo.SetValue(ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim(), i);
        //    i++;

        //    //Validation for Flight date
        //    if (txtfltdt.Text == "")
        //    {
        //        lblStatus.Text = "Please select Valid date";
        //        lblStatus.ForeColor = Color.Blue;
        //        return;
        //    }

        //    DateTime dt;

        //    try
        //    {
        //        //dt = Convert.ToDateTime(txtbillingfrom.Text);
        //        //Change 03082012
        //        string day = txtfltdt.Text.Substring(0, 2);
        //        string mon = txtfltdt.Text.Substring(3, 2);
        //        string yr = txtfltdt.Text.Substring(6, 4);
        //        strFltdate = yr + "-" + mon + "-" + day;
        //        dt = Convert.ToDateTime(strFltdate);

        //    }
        //    catch (Exception ex)
        //    {
        //        lblStatus.Text = "Selected Date format invalid";
        //        lblStatus.ForeColor = Color.Red;
        //        return;
        //    }

        //    AwbRateInfo.SetValue(Convert.ToDateTime(strFltdate).ToString("yyyy-MM-dd HH:mm:ss"), i);

        //    #endregion Prepare Parameters

        //    dsAWBData = objExpMani.GetDepartedPPAWBData(AwbRateInfo);

        //    if (dsAWBData != null)
        //    {
        //        if (dsAWBData.Tables != null)
        //        {
        //            if (dsAWBData.Tables.Count > 0)
        //            {
        //                if (dsAWBData.Tables[0].Rows.Count > 0)
        //                {
        //                    try
        //                    {
        //                        string res = "";
        //                        string InvoiceNumList = "";
        //                        for (int cnt = 0; cnt < dsAWBData.Tables[0].Rows.Count; cnt++)
        //                        {
        //                            //Generate Invoice Number (WALKIN Agent) for each AWB
        //                            #region Prepare Parameters
        //                            object[] AwbInfo = new object[14];
        //                            int j = 0;

        //                            AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["AWBNumber"].ToString(), j);
        //                            j++;
        //                            AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["AgentCode"].ToString(), j);
        //                            j++;
        //                            AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["OriginCode"].ToString(), j);
        //                            j++;
        //                            AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["DestinationCode"].ToString(), j);
        //                            j++;
        //                            AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["Pieces"].ToString(), j);
        //                            j++;
        //                            AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["GrossWeight"].ToString(), j);
        //                            j++;
        //                            AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["ChargedWeight"].ToString(), j);
        //                            j++;
        //                            AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["FrtIATA"].ToString(), j);
        //                            j++;
        //                            AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["FrtMKT"].ToString(), j);
        //                            j++;
        //                            AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["OCDueCar"].ToString(), j);
        //                            j++;
        //                            AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["OCDueAgent"].ToString(), j);
        //                            j++;
        //                            AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["ServTax"].ToString(), j);
        //                            j++;
        //                            AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["AWBDate"].ToString(), j);


        //                            #endregion Prepare Parameters

        //                            res = objExpMani.InsertDepartedPPAWBData(AwbInfo);

        //                            //Generate Invoice Number (WALKIN Agent) for each AWB
        //                            #region Prepare Parameters
        //                            object[] AwbInvoiceInfo = new object[1];
        //                            int k = 0;

        //                            AwbInvoiceInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["AWBNumber"].ToString(), k);

        //                            #endregion Prepare Parameters

        //                            if (InvoiceNumList == "")
        //                            {
        //                                InvoiceNumList = InvoiceNumList + objExpMani.GenerateBunchInvoiceNumWalkInAgent(AwbInvoiceInfo);
        //                            }
        //                            else
        //                            {
        //                                InvoiceNumList = InvoiceNumList + "," + objExpMani.GenerateBunchInvoiceNumWalkInAgent(AwbInvoiceInfo);
        //                            }

        //                        }

        //                        //Code to Generate Invoice (WALKIN Agent)
        //                        hfInvoiceNos.Value = InvoiceNumList;

        //                        Page.ClientScript.RegisterStartupScript(this.GetType(), "client", "GenerateInvoices();", true);

        //                    }
        //                    catch
        //                    {

        //                    }
        //                }
        //            }
        //        }
        //    }


        //}


        //protected void btnReopenFit_Click(object sender, EventArgs e)
        //{
        //    UpdateDepartedFlight("R");
        //    ////DeleteMileStone("Manifested");
        //    ////DeleteMileStone("Departed");

        //    BtnAddtoManifest.Enabled = true;
        //    btnSplitAssign.Enabled = true;
        //}


        //public void DeleteMileStone(string milestone)
        //{

        //    for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
        //    {
        //        //if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
        //        //{
        //        string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby, counter = "", SCC = "", Desc = "";
        //        int PCS = 0, AVLPCS = 0;
        //        double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, VOL = 0;
        //        string Updatedon = "";
        //        string LoadingPriority = "", Remark = "", ULDDest = "";


        //        ULDno = gdvULDDetails.Rows[i].Cells[1].Text;
        //        POU = gdvULDDetails.Rows[i].Cells[2].Text;
        //        POL = gdvULDDetails.Rows[i].Cells[3].Text;
        //        ULDDest = gdvULDDetails.Rows[i].Cells[4].Text;
        //        if (gdvULDDetails.Rows[i].Cells[5].Text == "&nbsp;")
        //        {
        //            counter = "0";
        //        }
        //        else
        //        {
        //            counter = gdvULDDetails.Rows[i].Cells[5].Text;
        //        }
        //        AWBno = gdvULDDetails.Rows[i].Cells[6].Text;
        //        if (gdvULDDetails.Rows[i].Cells[7].Text == "&nbsp;")
        //        {
        //            SCC = "0";
        //        }
        //        else
        //        {
        //            SCC = gdvULDDetails.Rows[i].Cells[7].Text;
        //        }


        //        PCS = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[8].Text);
        //        WGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[9].Text);
        //        if (gdvULDDetails.Rows[i].Cells[10].Text == "&nbsp;")
        //        {
        //            VOL = 0;
        //        }
        //        else
        //        {
        //            VOL = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[10].Text);
        //        }
        //        AVLPCS = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[13].Text);
        //        AVLWGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[14].Text);
        //        if (gdvULDDetails.Rows[i].Cells[15].Text == "&nbsp;")
        //        {
        //            Desc = "";
        //        }
        //        else
        //        {
        //            Desc = gdvULDDetails.Rows[i].Cells[15].Text;
        //        }

        //        if (gdvULDDetails.Rows[i].Cells[19].Text == "&nbsp;")
        //        {
        //            LoadingPriority = "";
        //        }
        //        else
        //        {
        //            LoadingPriority = ((DropDownList)gdvULDDetails.Rows[i].FindControl("ddlLoadingPriority")).SelectedItem.Text.ToString();
        //        }

        //        Object[] AWBDetails = new object[6];
        //        int intI = 0;
        //        int intDep = 0;

        //        AWBDetails.SetValue(AWBno, intI);
        //        intI++;

        //        AWBDetails.SetValue(PCS, intI);
        //        intI++;

        //        AWBDetails.SetValue(FLTno, intI);
        //        intI++;

        //        intDep = intI;
        //        AWBDetails.SetValue(milestone, intI);
        //        intI++;

        //        AWBDetails.SetValue(WGT, intI);
        //        intI++;

        //        AWBDetails.SetValue("", intI);
        //        intI++;

        //        AWBMilestonesBAL objMileStone = new AWBMilestonesBAL();
        //        string strResult = objMileStone.DeleteAWBMilestone(AWBDetails);
        //    }

        //}



        //private string  UpdateDepartedFlight(string FlightStatus)
        //{

        //    string ULDno = "", POU = "", POL = "", FLTno = "", Updatedby, Version = "", strfltdate = "";
        //    DateTime dtFlightdate;
        //    BLExpManifest objManifest = new BLExpManifest();

        //    FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
        //    Updatedby = Convert.ToString(Session["Username"]);

        //    //Added by Vijay
        //    string day = txtfltdt.Text.Substring(0, 2);
        //    string mon = txtfltdt.Text.Substring(3, 2);
        //    string yr = txtfltdt.Text.Substring(6, 4);
        //    strfltdate = mon + "-" + day + "-" + yr;
        //    dtFlightdate = Convert.ToDateTime(strfltdate);
        //    //dtFlightdate = DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null);

        //    ULDno = string.Empty;
        //    POU = string.Empty;
        //    POL = lblDepAirport.Text.Trim();//ddlPOLDetails.Text;
        //    Version = string.Empty;

        //    string str = objManifest.ReOpenManifestdata(FLTno, ULDno, POU, POL, Updatedby, dtCurrentDate, FlightStatus, dtFlightdate);

        //        lblStatus.ForeColor = Color.Green;
        //    if(str.Trim()=="" || str.Trim()==null)
        //    {
        //        if (FlightStatus == "D")
        //            lblStatus.Text = "Flight Departed Successfully.";
        //        else
        //            lblStatus.Text = "Flight Re-Opened Successfully.";

        //        return str;
        //    }
        //    else
        //    {
        //        lblStatus.ForeColor = Color.Red;
        //        lblStatus.Text=str.Trim();
        //        return str;
        //    }

        //    //}
        //    //else
        //    //{
        //    //    lblStatus.ForeColor = Color.Red;
        //    //    lblStatus.Text = "Error in while updating. Try again.";
        //    //    return false;
        //    //}

        //}

        private bool SaveNilManifest(string FlightNo, string ULDNo, string POL, string POU, string UserName, DateTime FlightDate, DateTime SysDate)
        {
            BLExpManifest objManifest = new BLExpManifest();

            bool blnResult = objManifest.SaveNilManifest(FlightNo, ULDNo, POL, POU, UserName, FlightDate, SysDate, "");

            return blnResult;
        }

        protected void gdvULDDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //protected void btnNOTOC_Click(object sender, EventArgs e)
        //{
        //    if (Session["IsManifested"] != null && (bool)Session["IsManifested"] == false)
        //    {
        //        showNOTOCData();
        //    }
        //}

        //protected void btnReAssign_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int check = 0;
        //        for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
        //        {
        //            if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
        //            {
        //                check = check + 1;
        //            }

        //        }

        //        if (check == 0)
        //        {
        //            lblStatus.ForeColor = Color.Red;
        //            lblStatus.Text = "Please Select atleast One AWB to Re-Assign.";
        //            return;
        //        }
        //        if (check > 1)
        //        {
        //            lblStatus.ForeColor = Color.Red;
        //            lblStatus.Text = "Please Select only One AWB to Re-Assign.";
        //            return;
        //        }

        //        LoadAirlineCode("");

        //        //Added by Poorna
        //        Session["Split"] = "R";
        //        btnAddManifest.Text = "Re-Assign";
        //        //lblNextFlight.Visible = true;
        //        //txtNextFlight.Visible = true;

        //        //lblNFltDate.Visible = true;
        //        //txtNFltDate.Visible = true;


        //        lblReason.Visible = true;
        //        //txtReason.Visible = true;

        //        grdAWBs.Columns[2].Visible = false;

        //        grdAWBs.Columns[3].Visible = false;
        //        grdAWBs.Columns[5].Visible = true;

        //        grdAWBs.Columns[6].Visible = true;
        //        UpdatePanelRouteDetails.Visible = true;

        //        DataTable dt = new DataTable();
        //        DataTable DTAWBDetails = new DataTable();

        //        DTAWBDetails.Columns.Add("AWBNo");
        //        DTAWBDetails.Columns.Add("Pieces");
        //        DTAWBDetails.Columns.Add("Weight");
        //        DTAWBDetails.Columns.Add("AvlPCS");
        //        DTAWBDetails.Columns.Add("AvlWgt");
        //        DTAWBDetails.Columns.Add("Origin");
        //        DTAWBDetails.Columns.Add("Destination");

        //        for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
        //        {
        //            if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
        //            {
        //                DataRow dr;

        //                //GridViewRow row = gdvULDLoadPlanAWB.Rows[j];
        //                //dr = dt.NewRow();
        //                string POL = lblDepAirport.Text;
        //                string POU = "";
        //                if (Session["dsLoad"] != null)
        //                {
        //                    DataSet dsLoad = (DataSet)Session["dsLoad"];
        //                    if (dsLoad.Tables[0].Rows.Count > 0)
        //                    {
        //                        for (int k = 0; k < dsLoad.Tables[0].Rows.Count; k++)
        //                        {
        //                            string awbno = dsLoad.Tables[0].Rows[k]["AWBNumber"].ToString();
        //                            if (awbno == ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text.Trim())
        //                            {
        //                                POU = dsLoad.Tables[0].Rows[k]["POU"].ToString();

        //                                break;
        //                            }

        //                        }

        //                    }
        //                }

        //                // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
        //                dr = DTAWBDetails.NewRow();
        //                dr[0] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text;
        //                dr[1] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text;
        //                dr[2] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text;
        //                dr[3] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text;
        //                dr[4] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text;
        //                dr[5] = POL;
        //                dr[6] = POU;


        //                DTAWBDetails.Rows.Add(dr);
        //                break;
        //            }
        //        }

        //        Session["AWBdata"] = DTAWBDetails;
        //        txtReason.Text = "";
        //        //  Response.Write("<script>");
        //        //  Response.Write("window.open('ShowAddManifestAWB.aspx','_blank','left=0,top=0,width=600,height=450,toolbar=0,resizable=0')");
        //        //   Response.Write("</script>");
        //        //SqlBulkCopy sbc = new SqlBulkCopy(targetConnStr);
        //        //sbc.DestinationTableName = "yourDestinationTable";
        //        //sbc.WriteToServer(dt);
        //        //sbc.Close(); 




        //        pnlGrid.Visible = true;
        //        // pnlGrid.Attributes.item("style") = "Z-INDEX: 176; LEFT: 584px; POSITION: absolute; TOP: 176px";
        //        pnlGrid.Style["Z-INDEX"] = "150";
        //        pnlGrid.Style["LEFT"] = "342px";
        //        pnlGrid.Style["POSITION"] = "absolute";
        //        pnlGrid.Style["TOP"] = "260px";

        //        try
        //        {
        //            LoadGridSchedule();
        //            DataTable dt1 = DTAWBDetails;
        //            DataTable dtCurrentTable = (DataTable)Session["AWBdata"];
        //            grdAWBs.DataSource = dtCurrentTable;
        //            grdAWBs.DataBind();
        //            if (dt1.Rows.Count > 0)
        //            {
        //                DataRow drCurrentRow = null;
        //                for (int i = 0; i < dt1.Rows.Count; i++)
        //                {
        //                    drCurrentRow = dtCurrentTable.NewRow();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text = dt1.Rows[i][0].ToString();

        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text = dt1.Rows[i][1].ToString();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = dt1.Rows[i][2].ToString();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = dt1.Rows[i][3].ToString();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = dt1.Rows[i][4].ToString();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtOrigin")).Text = dt1.Rows[i][5].ToString();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtDestination")).Text = dt1.Rows[i][6].ToString();

        //                    ViewState["CurrentTable1"] = dtCurrentTable;


        //                }
        //                // grdAWBs.DataSource = dtCurrentTable;
        //                // grdAWBs.DataBind();

        //            }
        //            // grdAWBs.DataSource = dt;
        //            //  grdAWBs.DataBind();


        //            // myButton.Attributes.Add ("onClick", "close();");


        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        ////BtnList_Click(null, null);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        private void AllButtonStatus(bool res)
        {
            // btnAddManifest.Enabled = res;
            BtnAddtoManifest.Enabled = res;
            // btnAssigntoULD.Enabled = res;


            BtnClear.Enabled = res;

            btnCloseCTM.Enabled = res;
            BtnList.Enabled = res;
            //BtnListDetails.Enabled = res;
            btnShowEAWB.Enabled = res;
            btnSplitAssign.Enabled = res;
            btnSplitUnassign.Enabled = res;
            btnUnassign.Enabled = res;

        }

        //protected void btnReturnToShipper_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int check = 0;
        //        for (int i = 0; i < gdvULDLoadPlanAWB.Rows.Count; i++)
        //        {
        //            if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
        //            {
        //                check = check + 1;
        //            }

        //        }
        //        if (check == 0)
        //        {
        //            lblStatus.ForeColor = Color.Red;
        //            lblStatus.Text = "Please Select One ULD from AWB TAB for Split and Assign";
        //            return;
        //        }
        //        if (check > 1)
        //        {
        //            lblStatus.ForeColor = Color.Red;
        //            lblStatus.Text = "Please Select only One ULD from AWB TAB for Split and Assign";
        //            return;
        //        }

        //        Session["Split"] = "RSI";
        //        btnAddManifest.Text = "Return To Shipper";
        //        lblNextFlight.Visible = false;
        //        txtNextFlight.Visible = false;

        //        grdAWBs.Columns[2].Visible = true;

        //        grdAWBs.Columns[3].Visible = true;
        //        grdAWBs.Columns[5].Visible = false;

        //        grdAWBs.Columns[6].Visible = false;

        //        UpdatePanelRouteDetails.Visible = false;
        //        lblNFltDate.Visible = false;
        //        txtNFltDate.Visible = false;

        //        lblReason.Visible = false;
        //        txtReason.Visible = false;

        //        DataTable dt = new DataTable();
        //        DataTable DTAWBDetails = new DataTable();

        //        DTAWBDetails.Columns.Add("AWBNo");
        //        DTAWBDetails.Columns.Add("Pieces");
        //        DTAWBDetails.Columns.Add("Weight");
        //        DTAWBDetails.Columns.Add("AvlPCS");
        //        DTAWBDetails.Columns.Add("AvlWgt");


        //        for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
        //        {
        //            if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
        //            {
        //                DataRow dr;

        //                // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
        //                dr = DTAWBDetails.NewRow();
        //                dr[0] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text;// gdvULDLoadPlanAWB.Rows[j].Cells[1].Text;
        //                dr[1] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text;//gdvULDLoadPlanAWB.Rows[j].Cells[2].Text;
        //                dr[2] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text;
        //                dr[3] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlPCS")).Text;//((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlPCS")).Text;
        //                dr[4] = ((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlWgt")).Text;//((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAvlWgt")).Text;

        //                DTAWBDetails.Rows.Add(dr);
        //            }
        //        }

        //        Session["AWBdata1"] = DTAWBDetails;

        //        pnlGrid.Visible = true;
        //        // pnlGrid.Attributes.item("style") = "Z-INDEX: 176; LEFT: 584px; POSITION: absolute; TOP: 176px";
        //        pnlGrid.Style["Z-INDEX"] = "150";
        //        pnlGrid.Style["LEFT"] = "342px";
        //        pnlGrid.Style["POSITION"] = "absolute";
        //        pnlGrid.Style["TOP"] = "260px";

        //        try
        //        {
        //            LoadGridSchedule();
        //            DataTable dt1 = DTAWBDetails;
        //            DataTable dtCurrentTable = (DataTable)Session["AWBdata1"];
        //            grdAWBs.DataSource = dtCurrentTable;
        //            grdAWBs.DataBind();
        //            if (dt1.Rows.Count > 0)
        //            {
        //                DataRow drCurrentRow = null;
        //                for (int i = 0; i < dt1.Rows.Count; i++)
        //                {
        //                    drCurrentRow = dtCurrentTable.NewRow();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text = dt1.Rows[i][0].ToString();

        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text = dt1.Rows[i][1].ToString();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = dt1.Rows[i][2].ToString();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = dt1.Rows[i][3].ToString();
        //                    ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = dt1.Rows[i][4].ToString();

        //                    ViewState["CurrentTable1"] = dtCurrentTable;
        //                }
        //            }
        //            AllButtonStatus(false);                
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        private void ChangeButtonStatus()
        {
            try
            {
            }
            catch (Exception ex)
            {


            }
        }

        protected void btnAddULDToManifest_Click(object sender, EventArgs e)
        {
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

                            FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                            POL = Convert.ToString(Session["Station"]);
                            // POU = ddlMainPOU.SelectedItem.Value.ToString();
                            POU = string.Empty;
                            Updatedby = Convert.ToString(Session["Username"]);
                            ULDno = "Bulk";
                            ULDwgt = 0;
                            DateTime FlightDate = DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null);
                            // dsawbData = objExpMani.GetAwbTabdetails(lblDepAirport.Text, FLTno, FlightDate);
                            DataTable dsULDData = new DataTable();
                            DataTable dsULDAWBData = new DataTable();
                            //  dsawb1 = (DataTable)Session["AddToManifestAWBdata"];// Session["AWBdata"];
                            // }
                            if (Session["ULDData"] != null)
                            {
                                dsULDData = (DataTable)Session["ULDData"];
                                dsULDAWBData = (DataTable)Session["ULDAWBData"];
                            }

                            IsManifest = false;
                            for (int j = 0; j < gdvULDLoadPlan.Rows.Count; j++)
                            {
                                if (((CheckBox)gdvULDLoadPlan.Rows[j].FindControl("Check1")).Checked == true)
                                {
                                    for (int i = 0; i < dsULDAWBData.Rows.Count; i++)
                                    {

                                        // ((CheckBox)gdvULDLoadPlan.Rows[j].FindControl("Check1")).Checked = false;
                                        //   gdvULDLoadPlanAWB.Rows[i].Visible = false;
                                        //Appended 25apr
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
                                                    //if (desc == "" && AWBDest == "")
                                                    //{
                                                    //    
                                                    string ActAWB = AWBno.Substring(AWBno.Length - 8);

                                                    DataSet dsDestDetails = objExpMani.GetULDAWBData(ActAWB);
                                                    if (dsDestDetails.Tables[0].Rows.Count > 0)
                                                    {
                                                        AWBDest = dsDestDetails.Tables[0].Rows[0][0].ToString();
                                                        desc = dsDestDetails.Tables[0].Rows[0][1].ToString();
                                                        Vol = dsDestDetails.Tables[0].Rows[0][2].ToString();
                                                        CommoCode = dsDestDetails.Tables[0].Rows[0]["CommodityCode"].ToString();
                                                        AWBOrigin = dsDestDetails.Tables[0].Rows[0]["OriginCode"].ToString();

                                                    }


                                                    //}
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

                            if (dsULDAWBData.Rows.Count > 0)
                            {
                                for (int k = 0; k < dsULDAWBData.Rows.Count; k++)
                                {



                                    //  tabMenifestDetails.Rows.Add(l_Datarow);
                                    //////((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAWBno")).Text = dsULDAWBData.Rows[k][0].ToString();
                                    //////((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text = dsULDAWBData.Rows[k][1].ToString();
                                    //////((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblWeight")).Text = dsULDAWBData.Rows[k][2].ToString();
                                    //////((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlPCS")).Text = dsULDAWBData.Rows[k][3].ToString();
                                    //////((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblAvlWgt")).Text = dsULDAWBData.Rows[k][4].ToString();

                                    //////((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblTotPCS")).Text = dsULDAWBData.Rows[k]["AWBPcs"].ToString();
                                    //////((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblTotWgt")).Text = dsULDAWBData.Rows[k]["AWBGwt"].ToString();

                                    //MemDetails.Rows.Add(l_Datarow);

                                    //gdvULDLoadPlanAWB.DataSource = (DataTable)MemDetails;
                                    //gdvULDLoadPlanAWB.DataBind();

                                }
                            }
                            Session["ULDAWBdata"] = dsULDAWBData;// dsawbData.Tables[1];
                            Session["AddToManifestAWBdata"] = dsULDAWBData;


                            for (int j = 0; j < tabMenifestDetails.Rows.Count; j++)
                            {
                                if (tabMenifestDetails.Rows[j][0].ToString() == "Bulk")
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

        }

        //protected void btnSendFBL_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //GetEmailConfig("FBL");
        //        //txtEmailID.Text = Session["ToID"].ToString();

        //        string FlightNumber = ddlFlightCode.Text.ToString() + txtFlightID.Text.ToString();
        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            BALEmailID ObjEmail = new BALEmailID();
        //            ds = ObjEmail.GetEmail(lblDepAirport.Text, Session["Dest"].ToString(), "FBL", FlightNumber, ddlFlightCode.Text.ToString());
        //            txtEmailID.Text = ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
        //            lblMsgCommType.Text = ds.Tables[0].Rows[0]["MsgCommType"].ToString();
        //            if (lblMsgCommType.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) || lblMsgCommType.Text.Equals("SITA", StringComparison.OrdinalIgnoreCase))
        //            {
        //                GenerateSITAHeader(ds.Tables[0].Rows[0]["PartnerSITAiD"].ToString());
        //            }
        //        }
        //        catch (Exception ex) { }

        //            ffmmsg = "";
        //            fblmsg = "fbl";
        //            Session["fbl"] = fblmsg;
        //            string Date = Session["Fltdate"].ToString().Trim();
        //            string Origin = Session["POLairport"].ToString().Trim();
        //            string FltNumber = Session["FltNumber"].ToString().Trim();
        //            //string ToId = Session["ToID"].ToString().Trim();
        //            txtMessageBody.Text = cls_BL.EncodeFBL(Origin, FltNumber, Date);

        //            lblMsg.Text = "FBL";
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);



        //    }
        //    catch (Exception ex)
        //    { }

        //}

        //protected void btnAssign_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string  Origin, Destination, FlightNumber;
        //        DateTime FlightDate;
        //        int Totpcs, PcsToAssign;
        //        Decimal TotWgt, WgtToAssign;


        //        if (Session["AWBdata"] == null)
        //            return;
        //        DataTable dtAWB = (DataTable)Session["AWBdata"];
        //        DataTable dtCreditInfoA = new DataTable();
        //        dtCreditInfoA.Columns.Add("AWB");
        //        dtCreditInfoA.Columns.Add("TotPcs");
        //        dtCreditInfoA.Columns.Add("TotWgt");
        //        dtCreditInfoA.Columns.Add("PcsToAssign");
        //        dtCreditInfoA.Columns.Add("WgttoAssign");
        //        dtCreditInfoA.Columns.Add("Origin");
        //        dtCreditInfoA.Columns.Add("Destination");
        //        dtCreditInfoA.Columns.Add("FlightNumber");
        //        dtCreditInfoA.Columns.Add("FliightDate");

        //             FlightNumber = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
        //            // string  fltDate = Convert.ToDateTime 
        //                   FlightDate = DateTime.ParseExact(txtfltdt.Text.Trim(), "dd/MM/yyyy", null);


        //        if (dtAWB != null && dtAWB.Rows.Count > 0)
        //        {
        //            try
        //            {
        //            for (int i = 0; i < dtAWB.Rows.Count; i++)
        //            {
        //                DataRow rw = dtCreditInfoA.NewRow();
        //                string AWBNo=dtAWB.Rows[i]["AWBNumber"].ToString();
        //                if (AWBNo.Length > 8)
        //                {
        //                    AWBNo = AWBNo.Substring(AWBNo.Length - 8);
        //                }
        //                rw["AWB"] = AWBNo;
        //                rw["TotPcs"] = dtAWB.Rows[i]["PiecesCount"];
        //                rw["TotWgt"] = dtAWB.Rows[i]["ChargedWeight"];
        //                rw["PcsToAssign"] = dtAWB.Rows[i]["PiecesCount"];
        //                rw["WgttoAssign"] = dtAWB.Rows[i]["ChargedWeight"];
        //                rw["Origin"] = dtAWB.Rows[i]["FltOrigin"];
        //                rw["Destination"] = dtAWB.Rows[i]["FltDestination"];
        //                rw["FlightNumber"] = FlightNumber;
        //                rw["FliightDate"] = FlightDate;

        //                dtCreditInfoA.Rows.Add(rw);


        //            }



        //        }
        //        catch (Exception ex)
        //        {
        //        }

        //          Session["AWBForULDAssoc"] = dtCreditInfoA;
        //          if (Session["AWBForULDAssoc"] != null)
        //          {
        //              //Response.Redirect("frmULDToAWBAssoc.aspx",false);
        //              ClientScript.RegisterStartupScript(this.GetType(), "", "callexportULD();", true);
        //          }
        //        }
        //        //  allocFirstLine(AWBNo, Totpcs, TotWgt, PcsToAssign, WgtToAssign, Origin, Destination, FlightNumber, FlightDate);


        //    }
        //    catch (Exception ex)
        //    { }
        //}


        #region allocFirstLine
        protected void allocFirstLine(string AWBNo, int Totpcs, decimal TotWgt, int PcsToAssign, decimal WgtToAssign, string Origin, string Destination, string FlightNumber, DateTime FlightDate)
        {
            try
            {
                DataTable dtCreditInfoA = new DataTable();
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

                DataSet dsRoutDetails = ((DataSet)Session["dsRoutDetails"]).Copy();
                DataRow row = dsRoutDetails.Tables[0].NewRow();

                row["FltOrigin"] = prevdest;
                row["FltDate"] = strDate;  //dtCurrentDate.ToString("dd/MM/yyyy");

                dsRoutDetails.Tables[0].Rows.Add(row);

                Session["dsRoutDetails"] = dsRoutDetails.Copy();
                grdRouting.DataSource = null;
                grdRouting.DataSource = dsRoutDetails.Copy();
                grdRouting.DataBind();

                //Validation by Vijay
                ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).ReadOnly = true;
                // ((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltOrig")).ReadOnly = true;

                LoadDropDownAndCheckBoxRouteDetails();

                // Session["Mod"] = "1";

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
            try
            {
                SaveRouteDetails();

                DataSet dsRouteDetailsTemp = ((DataSet)Session["dsRoutDetails"]).Clone();
                DataSet dsRouteDetails = (DataSet)Session["dsRoutDetails"];

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

                        dsRouteDetailsTemp.Tables[0].Rows.Add(row);
                    }
                }

                Session["dsRoutDetails"] = dsRouteDetailsTemp.Copy();
                grdRouting.DataSource = null;
                grdRouting.DataSource = dsRouteDetailsTemp.Copy();
                grdRouting.DataBind();

                LoadDropDownAndCheckBoxRouteDetails();

                Session["Mod"] = "1";

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
            DataSet dsRoutDetails = ((DataSet)Session["dsRoutDetails"]).Clone();

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
                DataSet dsRouteDetails = (DataSet)Session["dsRoutDetails"];

                for (int i = 0; i < dsRouteDetails.Tables[0].Rows.Count; i++)
                {
                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Clear();
                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["FltNumber"].ToString().Trim(), dsRouteDetails.Tables[0].Rows[i]["FltTime"].ToString().Trim()));

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
                //if (ddlDest.SelectedItem.Text == "Select")
                //{
                //    LBLRouteStatus.Text = "kindly select Destination in Consignment Details.";
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                //    ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text = "";
                //    ddlDest.Focus();
                //    return;
                //}

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

                //int hr = 0, min = 0, AllowedHr = 0;
                //if (((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.Trim() == BookingBAL.OrgStation.Trim())
                //{
                //    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                //    DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

                //    hr = dtIndianTime.Hour;
                //    min = dtIndianTime.Minute;
                //    AllowedHr = 2;
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
                //        AllowedHr = 1;
                //    }
                //    else
                //    {
                //        hr = int.Parse(time.Substring(0, time.IndexOf(":")));
                //        min = int.Parse(time.Substring(time.IndexOf(":") + 1));

                //        AllowedHr = 1;
                //    }
                //}

                string errormessage = "";
                UpdatePartnerCode(rowindex);
                GetFlightRouteData(rowindex);

                //DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, 0, 0, 0, ref errormessage);

                //Session["Flt"] = dsresult.Copy();

                //if (dsresult != null && dsresult.Tables.Count != 0)
                //{
                //    DataRow row = dsresult.Tables[0].NewRow();
                //    row["FltNumber"] = "Select";
                //    row["ArrTime"] = "Select";

                //    dsresult.Tables[0].Rows.Add(row);

                //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataTextField = "FltNumber";
                //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataValueField = "ArrTime";
                //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataSource = dsresult.Tables[0].Copy();
                //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataBind();

                //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).SelectedIndex = dsresult.Tables[0].Rows.Count - 1;
                //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).Focus();
                //}
                //else
                //{
                //    LBLRouteStatus.Text = "no record found";
                //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).Items.Clear();
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                //    return;
                //}

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
            DataSet dsResult = new DataSet();
            if (strdate.Trim() == "")
            {
                if (new ShowFlightsBAL().GetFlightList(Origin, Dest,0, ref dsResult, ref errormessage, dtCurrentDate))
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
            DataSet dsNewResult = dsResult.Clone();
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

            dsResult = new DataSet();
            dsResult.Tables.Add(dv.ToTable().Copy());


            //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);


            DataTable dt = dsResult.Tables[0].Clone();
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

            dsResult = new DataSet();
            dsResult.Tables.Add(dt);
        }
        #endregion FormatRecords

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
                DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, hr, min, AllowedHr, ref errormessage);

                if (dsresult != null && dsresult.Tables.Count != 0)
                {
                    Session["Flt"] = dsresult.Copy();
                    DataRow row = dsresult.Tables[0].NewRow();
                    row["FltNumber"] = "Select";
                    row["ArrTime"] = "Select";

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

            DataSet ds = (DataSet)Session["Flt"];

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
            DataTable myDataTable = new DataTable();
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

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "Status";
            //myDataTable.Columns.Add(myDataColumn);


            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "Accepted";
            //myDataTable.Columns.Add(myDataColumn);

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "AcceptedPcs";
            //myDataTable.Columns.Add(myDataColumn);


            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "AcceptedWt";
            //myDataTable.Columns.Add(myDataColumn);


            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "ScheduleID";
            //myDataTable.Columns.Add(myDataColumn);

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "ChrgWt";
            //myDataTable.Columns.Add(myDataColumn);


            DataRow dr;
            dr = myDataTable.NewRow();
            dr["FltOrigin"] = Session["Station"].ToString();
            dr["FltDestination"] = "";// "DEL";
            dr["FltNumber"] = "";// "IT101";
            dr["FltDate"] = dtCurrentDate.ToString("dd/MM/yyyy");// "14/Jan/2012";
            dr["Pcs"] = "";// 
            dr["Wt"] = "";// 
            //dr["ChrgWt"] = "";// 
            //dr["Status"] = "C";// 
            //dr["Accepted"] = "N";// 
            //dr["AcceptedPcs"] = "";// 
            //dr["AcceptedWt"] = "";


            myDataTable.Rows.Add(dr);

            grdRouting.DataSource = null;
            grdRouting.DataSource = myDataTable;
            grdRouting.DataBind();

            //((TextBox)grdRouting.Rows[grdRouting.Rows.Count - 1].FindControl("txtFltOrig")).ReadOnly = true;

            DataSet dsRoutDetails = new DataSet();
            dsRoutDetails.Tables.Add(myDataTable);
            Session["dsRoutDetails"] = dsRoutDetails.Copy();

            // SetRouteGridValues(myDataTable);
        }
        #endregion loadgridroutingdetails

        //protected void btnFinalize_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        if (Session["IsDeparted"] != null && Convert.ToString(Session["IsDeparted"]) == "D")
        //        {
        //            return;
        //        }

        //        btnSave_Click(null, null);

        //        if (Session["IsCommit"] != null)
        //        {
        //            string str = Convert.ToString(Session["IsCommit"]);
        //            if (str == "True")
        //            {
        //                return;
        //            }
        //        }


        //        bool res = false;

        //        for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
        //        {
        //            //if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked == true)
        //            //{
        //            string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby, counter = "", SCC = "", Desc = "";
        //            int PCS = 0, AVLPCS = 0;
        //            double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, VOL = 0;
        //            string Updatedon = "";
        //            string LoadingPriority = "", Remark = "", ULDDest = "";
        //            //bool IsManifest = false;
        //            FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
        //            Updatedby = Convert.ToString(Session["Username"]);

        //            ULDno = gdvULDDetails.Rows[i].Cells[1].Text.Trim();
        //            POU = gdvULDDetails.Rows[i].Cells[2].Text.Trim();
        //            POL = gdvULDDetails.Rows[i].Cells[3].Text.Trim();
        //            ULDDest = gdvULDDetails.Rows[i].Cells[4].Text.Trim();
        //            if (gdvULDDetails.Rows[i].Cells[5].Text == "&nbsp;")
        //            {
        //                counter = "0";
        //            }
        //            else
        //            {
        //                counter = gdvULDDetails.Rows[i].Cells[5].Text;
        //            }
        //            AWBno = gdvULDDetails.Rows[i].Cells[6].Text.Trim();
        //            AWBno = AWBno.Substring(AWBno.Length - 8);
        //            if (gdvULDDetails.Rows[i].Cells[7].Text == "&nbsp;")
        //            {
        //                SCC = "0";
        //            }
        //            else
        //            {
        //                SCC = gdvULDDetails.Rows[i].Cells[7].Text;
        //            }


        //            PCS = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[8].Text);
        //            WGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[9].Text);
        //            if (gdvULDDetails.Rows[i].Cells[10].Text == "&nbsp;")
        //            {
        //                VOL = 0;
        //            }
        //            else
        //            {
        //                VOL = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[10].Text);
        //            }
        //            AVLPCS = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[13].Text);
        //            AVLWGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[14].Text);
        //            if (gdvULDDetails.Rows[i].Cells[15].Text == "&nbsp;")
        //            {
        //                Desc = "";
        //            }
        //            else
        //            {
        //                Desc = gdvULDDetails.Rows[i].Cells[15].Text;
        //                Desc = Desc.Replace("&amp;", "&");
        //            }
        //            bool IsManifest = bool.Parse(gdvULDDetails.Rows[i].Cells[18].Text.Trim());


        //            if (gdvULDDetails.Rows[i].Cells[19].Text == "&nbsp;")
        //            {
        //                LoadingPriority = "";
        //            }
        //            else
        //            {
        //                LoadingPriority = ((DropDownList)gdvULDDetails.Rows[i].FindControl("ddlLoadingPriority")).SelectedItem.Text.ToString();
        //            }

        //            if (gdvULDDetails.Rows[i].Cells[20].Text == "&nbsp;")
        //            {
        //                Remark = "";
        //            }
        //            else
        //            {
        //                Remark = ((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text.ToString();
        //            }

        //            // Updatedon = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        //            Updatedon = dtCurrentDate.ToString("yyyy/MM/dd");// DateTime.Now.ToString("yyyy/MM/dd");
        //            DateTime dtflightdate = DateTime.ParseExact(txtfltdt.Text, "dd/MM/yyyy", null);
        //            //Added code to commit only AWB's Loaded from POL
        //            if (Session["Station"].ToString() == POL)
        //            {
        //                //res = objExpMani.CommitManifestdata(FLTno, ULDno, POU, POL, ULDDest, counter, AWBno, SCC, VOL, PCS, WGT, AVLPCS, AVLWGT, Desc, LoadingPriority, Remark, Updatedby, Updatedon, dtflightdate);
        //            }
        //        }

        //        if (res == true)
        //        {
        //            // ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Successfull();", true);
        //            lblStatus.ForeColor = Color.Green;
        //            lblStatus.Text = "Manifest Version Commited Successfully";
        //            // return;

        //        }

        //        hdnManifestFlag.Value = "";

        //        //BtnList_Click(null, null);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return;

        //}

        #region btnTesting Event
        protected void btnTesting_Click(object sender, EventArgs e)
        {

        }
        #endregion


        private void AWBDesignators()
        {
            LoginBL Bal = new LoginBL();
            ddlFlightCode.Items.Clear();

            DataTable dt = Bal.LoadSystemMasterDataNew("DC").Tables[0];
            for (int intCount = 0; intCount < dt.Rows.Count; intCount++)
            {
                ddlFlightCode.Items.Add(dt.Rows[intCount]["DesignatorCode"].ToString());
            }
            Bal = null;
            dt = null;
        }

        //private void GenerateSITAHeader(string receiverSITAID)
        //{
        //    try
        //    {
        //        BookingBAL objBLL = new BookingBAL();
        //        receiverSITAID = receiverSITAID.Replace(',', ' ');
        //        string SenderSITAID = objBLL.getSITAID(ddlFlightCode.SelectedItem.Text.ToString());
        //        txtSITAHeader.Text = "QP " + receiverSITAID;
        //        txtSITAHeader.Text = txtSITAHeader.Text + "\r\n" + "." + SenderSITAID + " " + System.DateTime.Now.ToString("dd") + System.DateTime.Now.ToUniversalTime().ToString("hhMM") + " P25";
        //        txtSITAHeader.Visible = true;
        //    }
        //    catch (Exception ex) { }
        //}

        protected void btnSitaUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMessageBody.Text.Length > 0 || txtSITAHeader.Text.Length > 0)
                {
                    FTP.SaveSITAMsg(txtSITAHeader.Text.ToString() + "\r\n" + txtMessageBody.Text.ToString(), lblMsg.Text.ToString() + "File" + System.DateTime.Now.ToString("hhmmss"));
                }
            }
            catch (Exception ex) { }
        }

        protected void btnFTPUpload_Click(object sender, EventArgs e)
        {
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
            Session["ManifestGridDatagha"] = null;
        }

        //protected void ddlReason_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //if (ddlReason.SelectedItem.Text.Trim() == "Others")
        //        //{
        //        //    txtReason.Visible = true;
        //        //}
        //        //else
        //        //{
        //        //    txtReason.Visible = false;
        //        //}


        //    }
        //    catch (Exception ex)
        //    { }
        //}

        #region UpdatePartnerCode
        private void UpdatePartnerCode(int rowindex)
        {
            try
            {
                string errormessage = "";
                DataSet dsResult = new DataSet();
                if (objBLL.GetAvailabePartners(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), dtCurrentDate, ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartnerType")).Text.ToUpper(), ref dsResult, ref errormessage))
                {
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
                                    dsResult.Dispose();
                                }
                            }

                        }
                    }
                }

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
                    DataSet ds = (DataSet)Session["Flt"];
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
                                        DataTable dt = new DataTable();
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
                    row["FltNumber"] = "Select";
                    row["ArrTime"] = "Select";

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

            //code flt dest changed
            //BookingBAL.OrgStation = BookingBAL.OrgStation.Trim();

            //((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper().Trim();
            //((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text = ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper().Trim();
            //string strPartnerCode = ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartner")).Text.Trim();

            //string errormessage = "";
            ////DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, 0, 0, 0, ref errormessage);
            //DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, 0, 0, 0, ref errormessage, strPartnerCode);

            //Session["Flt"] = dsresult.Copy();

            //if (dsresult != null && dsresult.Tables.Count != 0)
            //{
            //    DataRow row = dsresult.Tables[0].NewRow();
            //    row["FltNumber"] = "Select";
            //    row["ArrTime"] = "Select";

            //    dsresult.Tables[0].Rows.Add(row);

            //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataTextField = "FltNumber";
            //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataValueField = "ArrTime";
            //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataSource = dsresult.Tables[0].Copy();
            //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).DataBind();

            //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).SelectedIndex = dsresult.Tables[0].Rows.Count - 1;
            //}
            //else
            //{
            //    LBLRouteStatus.Text = "no record found";
            //    ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlFltNum")).Items.Clear();
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            //    return;
            //}

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
                DataSet ds = objBLL.GetPartnerType(true);

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
                                //ddl.DataSource = ds;
                                //ddl.DataMember = ds.Tables[0].TableName;
                                //ddl.DataTextField = "PartnerType";
                                //ddl.DataValueField = "PartnerType";
                                //ddl.DataBind();
                                ddl.Items.Clear();
                                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                {
                                    ddl.Items.Add(ds.Tables[0].Rows[k][0].ToString());
                                }
                            }
                            ds.Dispose();
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
            DataSet dsResult = new DataSet();
            if (strdate.Trim() == "")
            {
                if (PartnerCode == Convert.ToString(Session["AirlinePrefix"]))
                {
                    if (new ShowFlightsBAL().GetFlightListforDay(Origin, Dest, ref dsResult, ref errormessage, dtCurrentDate,""))
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

                if (PartnerCode == Convert.ToString(Session["AirlinePrefix"]))
                {
                    if (new ShowFlightsBAL().GetFlightListforDay(Origin, Dest, ref dsResult, ref errormessage, dt,""))
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

        protected void BtnList_Click(object sender, EventArgs e)
        {
            try
            {
                string Flight = "";
                string fltdt = "";
                if (txtFlightID.Text.Length > 0)
                {
                    Flight = txtFlightPrefix.Text.Trim() + txtFlightID.Text.Trim();//ddlFlightCode.SelectedItem.Text.Trim() + txtFlightID.Text.Trim();
                }
                else
                {
                    Flight = "";
                }

                if (txtfltdt.Text.Length > 0)
                {
                    fltdt = txtfltdt.Text.ToString();
                }
                else
                {
                    fltdt = "";
                }
                DataTable tabMenifestDetailsGHA = new DataTable();
                tabMenifestDetailsGHA.Columns.Add("AWBNumber");
                tabMenifestDetailsGHA.Columns.Add("PiecesCount");
                tabMenifestDetailsGHA.Columns.Add("GrossWeight");
                tabMenifestDetailsGHA.Columns.Add("FlightNo");
                tabMenifestDetailsGHA.Columns.Add("FlightDt");
                tabMenifestDetailsGHA.Columns.Add("DriverName");
                tabMenifestDetailsGHA.Columns.Add("DLNumber");
                tabMenifestDetailsGHA.Columns.Add("Phone");
                tabMenifestDetailsGHA.Columns.Add("VehicleNo");
                tabMenifestDetailsGHA.Columns.Add("TokenNumber");
                tabMenifestDetailsGHA.Columns.Add("TokenDate");
                tabMenifestDetailsGHA.Columns.Add("IsScreeningReq");
                tabMenifestDetailsGHA.Columns.Add("ULDNo");
                //tabMenifestDetailsGHA = (DataTable)Session["grdulddetails"];
                Session["grdulddetails"] = (DataTable)tabMenifestDetailsGHA;
                Session["ManifestGridDatagha"] = (DataTable)tabMenifestDetailsGHA;

                #region Parameter
                string[] PName = new string[5];
                PName[0] = "FltNo";
                PName[1] = "FltDate";
                PName[2] = "Station";
                PName[3] = "AWBno";
                PName[4] = "TKNNO";

                object[] PValue = new object[5];
                PValue[0] = Flight;
                PValue[1] = fltdt;
                PValue[2] = Session["Station"].ToString();
                if (txtAWBNo.Text.Length > 0)
                    PValue[3] = txtAWBPrefix.Text.Trim() + txtAWBNo.Text.Trim();
                else
                    PValue[3] = "";
                PValue[4] = ddltkn.SelectedItem.Text.Trim();

                SqlDbType[] PType = new SqlDbType[5];
                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.VarChar;
                PType[2] = SqlDbType.VarChar;
                PType[3] = SqlDbType.VarChar;
                PType[4] = SqlDbType.VarChar;

                #endregion
                DataSet Ds = new DataSet();
                if (chkdelivery.Checked == true)
                {
                    Ds = da.SelectRecords("SP_GHADelivery", PName, PValue, PType);

                    try
                    {
                        if (Ds != null)
                        {

                            if (Ds.Tables != null)
                            {
                                if (Ds.Tables.Count > 0)
                                {
                                    if (Ds.Tables[0].Rows.Count > 0)
                                    {

                                        gdvULDLoadPlanAWB.PageIndex = 0;
                                        gdvULDLoadPlanAWB.DataSource = Ds;
                                        gdvULDLoadPlanAWB.DataMember = Ds.Tables[0].TableName;
                                        gdvULDLoadPlanAWB.DataBind();
                                        gdvULDLoadPlanAWB.Visible = true;
                                        Session["gdvULDLoadPlanAWB"] = Ds;
                                        lblStatus.Text = "";
                                        if (chkdelivery.Checked == true)
                                        {
                                            gdvULDLoadPlanAWB.HeaderRow.Cells[2].Text = "Arr Pcs";
                                            gdvULDLoadPlanAWB.HeaderRow.Cells[3].Text = "Arr Wt";
                                            gdvULDLoadPlanAWB.HeaderRow.Cells[4].Text = "Dlr Pcs";
                                            gdvULDLoadPlanAWB.HeaderRow.Cells[5].Text = "Dlr Wt";
                                            gdvULDLoadPlanAWB.HeaderRow.Cells[6].Text = "Rem Pcs";
                                            gdvULDLoadPlanAWB.HeaderRow.Cells[7].Text = "Rem Wt";
                                        }

                                    }
                                    if (Ds.Tables[1].Rows.Count > 0)
                                    {

                                        gdvULDDetails.PageIndex = 0;
                                        gdvULDDetails.DataSource = Ds;
                                        gdvULDDetails.DataMember = Ds.Tables[1].TableName;
                                        gdvULDDetails.DataBind();
                                        gdvULDDetails.Visible = true;
                                        Session["ManifestGridDatagha"] = Ds.Tables[1];
                                        lblStatus.Text = "";


                                    }
                                    if (Ds.Tables[2].Rows.Count > 0)
                                    {

                                        gdvULDLoadPlan.PageIndex = 0;
                                        gdvULDLoadPlan.DataSource = Ds;
                                        gdvULDLoadPlan.DataMember = Ds.Tables[2].TableName;
                                        gdvULDLoadPlan.DataBind();
                                        gdvULDLoadPlan.Visible = true;
                                        Session["ManifestGridDataghaULD"] = Ds.Tables[2];
                                        lblStatus.Text = "";


                                    }

                                }
                                else
                                {
                                    lblStatus.Text = "No Records are Found.";
                                }
                            }
                            else
                            {
                                lblStatus.Text = "No Records are Found.";
                            }
                            
                        }
                    }

                    catch (Exception ex)
                    { }
                    finally
                    {
                        PValue = null; PType = null; PName = null;
                    }
                }
                else
                {
                    Ds = da.SelectRecords("SP_GHAIncommingBooking", PName, PValue, PType);

                    try
                    {
                        if (Ds != null)
                        {
                            if (Ds.Tables != null)
                            {
                                if (Ds.Tables.Count > 0)
                                {
                                    if (Ds.Tables[0].Rows.Count > 0)
                                    {

                                        gdvULDLoadPlanAWB.PageIndex = 0;
                                        gdvULDLoadPlanAWB.DataSource = Ds;
                                        gdvULDLoadPlanAWB.DataMember = Ds.Tables[0].TableName;
                                        gdvULDLoadPlanAWB.DataBind();
                                        gdvULDLoadPlanAWB.Visible = true;
                                        Session["gdvULDLoadPlanAWB"] = Ds;
                                        lblStatus.Text = "";


                                    }
                                    //else
                                    //{
                                    //    lblStatus.Text = "No Records are Found.";
                                    //}

                                    if (Ds.Tables[1].Rows.Count > 0)
                                    {

                                        gdvULDDetails.PageIndex = 0;
                                        gdvULDDetails.DataSource = Ds;
                                        gdvULDDetails.DataMember = Ds.Tables[1].TableName;
                                        gdvULDDetails.DataBind();
                                        gdvULDDetails.Visible = true;
                                        Session["ManifestGridDatagha"] = Ds.Tables[1];
                                        lblStatus.Text = "";


                                    }

                                    if (Ds.Tables[2].Rows.Count > 0)
                                    {

                                        gdvULDLoadPlan.PageIndex = 0;
                                        gdvULDLoadPlan.DataSource = Ds;
                                        gdvULDLoadPlan.DataMember = Ds.Tables[2].TableName;
                                        gdvULDLoadPlan.DataBind();
                                        gdvULDLoadPlan.Visible = true;
                                        Session["ManifestGridDataghaULD"] = Ds.Tables[2];
                                        lblStatus.Text = "";


                                    }
                                    //else
                                    //{
                                    //    lblStatus.Text = "No Records are Found.";
                                    //}

                                }
                                else
                                {
                                    lblStatus.Text = "No Records are Found.";
                                }
                            }
                            else
                            {
                                lblStatus.Text = "No Records are Found.";
                            }
                        }

                    }
                    catch (Exception ex)
                    { }
                    finally
                    {
                        PValue = null; PType = null; PName = null;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnprintTKN_Click(object sender, EventArgs e)
        {
            string[] PName = new string[4];
            object[] PValue = new object[4];
            SqlDbType[] PType = new SqlDbType[4];
            try
            {
                //if (!Validation())
                //    return;
                string fltno = ddlFlightCode.SelectedItem.Text.Trim() + txtFlightID.Text.Trim();
                #region Parameter

                PName[0] = "strRotationId";
                PName[1] = "intYear";
                PName[2] = "strUserName";
                PName[3] = "strOutput";

                if (chkdelivery.Checked == true)
                {
                    PValue[0] = "GTP";
                }
                else
                {
                    PValue[0] = "TKN";
                }
                PValue[1] = 0;//txtFrmDate.Text;
                PValue[2] = Session["UserName"].ToString();//txtToDate.Text;
                PValue[3] = "";

                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.SmallInt;
                PType[2] = SqlDbType.VarChar;
                PType[3] = SqlDbType.VarChar;

                #endregion
                DataSet ds = da.SelectRecords("spGenerateRotationNoNEW", PName, PValue, PType);

                string TKN = ds.Tables[0].Rows[0]["RotationId"].ToString();

                Session["TKN"] = TKN.ToString();

                #region parameter for insert
                StringBuilder awbnos = new StringBuilder();
                DataSet dsinsert1 = new DataSet();
                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    if (((Label)gdvULDDetails.Rows[i].FindControl("lblTokenNumber")).Text == "")
                    {
                        #region save

                        string drivername = "", fltdt = "";
                        string AWBno = ((Label)gdvULDDetails.Rows[i].FindControl("lblAWBno")).Text; //gdvULDLoadPlanAWB.Rows[i].Cells[1].Text;
                        string PCS = ((Label)gdvULDDetails.Rows[i].FindControl("lblPCS")).Text;
                        string WGT = ((Label)gdvULDDetails.Rows[i].FindControl("lblWt")).Text;
                        string ULDNo = ((Label)gdvULDDetails.Rows[i].FindControl("lblULDNo")).Text;
                        //if (((Label)gdvULDDetails.Rows[i].FindControl("lblDriverName")).Text == "" || ((Label)gdvULDDetails.Rows[i].FindControl("lblDriverName")).Text == null)
                        //{
                        drivername = txtDriverName.Text.Trim();
                        //}
                        //else
                        //{
                        //    drivername = ((Label)gdvULDDetails.Rows[i].FindControl("lblDriverName")).Text;
                        //}
                        //if (((Label)gdvULDDetails.Rows[i].FindControl("lblTokenNumber")).Text != "")
                        //{
                        //    TKN = ((Label)gdvULDDetails.Rows[i].FindControl("lblTokenNumber")).Text;
                        //}
                        if (ddltkn.SelectedIndex > 0)
                        {
                            TKN = ddltkn.SelectedItem.Text.Trim();
                        }
                        else
                        {
                            TKN = Session["TKN"].ToString();
                        }
                        if (((Label)gdvULDDetails.Rows[i].FindControl("lblFlightNo")).Text != "")
                        {
                            fltno = ((Label)gdvULDDetails.Rows[i].FindControl("lblFlightNo")).Text;
                        }
                        if (((Label)gdvULDDetails.Rows[i].FindControl("lblFlightDt")).Text != "")
                        {
                            fltdt = ((Label)gdvULDDetails.Rows[i].FindControl("lblFlightDt")).Text;
                        }
                        else
                        {
                            fltdt = txtfltdt.Text.Trim();
                        }

                        string DL = txtDLNumber.Text.Trim();
                        string phone = txtphone.Text.Trim();
                        string vehicleno = txtVehicleNo.Text.Trim();
                        bool isscreen;
                        if (((Label)gdvULDDetails.Rows[i].FindControl("lblisscreen")).Text == "Y")
                        {
                            isscreen = true;
                        }
                        else
                        {
                            isscreen = false;
                        }
                        awbnos.Append(AWBno + ",");

                        #region Parameter
                        string[] PName1 = new string[16];
                        PName1[0] = "AWBno";
                        PName1[1] = "PCS";
                        PName1[2] = "WGT";
                        PName1[3] = "drivername";
                        PName1[4] = "DL";
                        PName1[5] = "phone";
                        PName1[6] = "vehicleno";
                        PName1[7] = "FlightNo";
                        PName1[8] = "FlghtDt";
                        PName1[9] = "TokenNumber";
                        PName1[10] = "TokenDate";
                        PName1[11] = "UpdatedOn";
                        PName1[12] = "UpdatedBy";
                        PName1[13] = "DOC";
                        PName1[14] = "Isscreen";
                        PName1[15] = "ULDNo";



                        object[] PValue1 = new object[16];
                        PValue1[0] = AWBno;
                        PValue1[1] = PCS;
                        PValue1[2] = WGT;
                        PValue1[3] = drivername;
                        PValue1[4] = DL;
                        PValue1[5] = phone;
                        PValue1[6] = vehicleno;
                        PValue1[7] = fltno;
                        PValue1[8] = fltdt;
                        PValue1[9] = TKN;
                        PValue1[10] = Session["IT"].ToString();
                        PValue1[11] = Session["IT"].ToString();
                        PValue1[12] = Session["UserName"].ToString();
                        PValue1[13] = txtDockNo.Text.Trim();
                        PValue1[14] = isscreen;
                        PValue1[15] = ULDNo;

                        SqlDbType[] PType1 = new SqlDbType[16];
                        PType1[0] = SqlDbType.VarChar;
                        PType1[1] = SqlDbType.VarChar;
                        PType1[2] = SqlDbType.VarChar;
                        PType1[3] = SqlDbType.VarChar;
                        PType1[4] = SqlDbType.VarChar;
                        PType1[5] = SqlDbType.VarChar;
                        PType1[6] = SqlDbType.VarChar;
                        PType1[7] = SqlDbType.VarChar;
                        PType1[8] = SqlDbType.VarChar;
                        PType1[9] = SqlDbType.VarChar;
                        PType1[10] = SqlDbType.DateTime;
                        PType1[11] = SqlDbType.DateTime;
                        PType1[12] = SqlDbType.VarChar;
                        PType1[13] = SqlDbType.VarChar;
                        PType1[14] = SqlDbType.Bit;
                        PType1[15] = SqlDbType.VarChar;

                        #endregion

                        try
                        {

                            DataSet dsinsert = da.SelectRecords("sp_SaveGHAIncomingBooking", PName1, PValue1, PType1);
                            dsinsert1 = dsinsert;
                            LoadToken();
                            ddltkn.SelectedValue = TKN.ToString();
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            PName1 = null;
                            PValue1 = null;
                            PType1 = null;
                        }
                        #endregion
                    }

                }

                try
                {
                    if (dsinsert1 != null)
                    {
                        if (dsinsert1.Tables != null)
                        {
                            if (dsinsert1.Tables.Count > 0)
                            {
                                if (dsinsert1.Tables[0].Rows.Count > 0)
                                {
                                    gdvULDDetails.PageIndex = 0;
                                    gdvULDDetails.DataSource = dsinsert1;
                                    gdvULDDetails.DataMember = dsinsert1.Tables[0].TableName;
                                    gdvULDDetails.DataBind();


                                    //Session["ForToken"] = dsinsert1.Tables[1];
                                    //DataTable dsinsert3 = new DataSet();
                                    DataTable dsinsert3 = dsinsert1.Tables[1];
                                    Session["ForToken"] = dsinsert3;
                                    DataTable dsinsert2 = dsinsert1.Tables[0];
                                    Session["ManifestGridDatagha"] = dsinsert2;

                                    if (chkdelivery.Checked == true)
                                    {
                                        lblStatus.Text = TKN + " Gate Pass Number Successfully Created";
                                        lblStatus.ForeColor = System.Drawing.Color.Green;
                                    }
                                    else
                                    {
                                        lblStatus.Text = TKN + " Token Number Successfully Created";
                                        lblStatus.ForeColor = System.Drawing.Color.Green;
                                    }

                                    btnSave.Enabled = false;
                                }
                                else
                                {
                                    lblStatus.Text = "No Records are Found.";
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
                #endregion
                try
                {
                    string msg = "";
                    if (chkdelivery.Checked == true)
                    {
                        msg = "Gate Pass Number" + TKN.ToString() + " is Generated for AWB Numbers:" + awbnos.ToString();
                    }
                    else
                    {
                        msg = "Token Number" + TKN.ToString() + " is Generated for AWB Numbers:" + awbnos.ToString();
                    }
                    cls_BL.addMsgToOutBox("SCM", msg, "", "");
                }
                catch (Exception ex)
                { }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                PName = null;
                PValue = null;
                PType = null;
            }

        }

        protected void btngenTKN_Click(object sender, EventArgs e)
        {
            #region Old Print
            //try
            //{
            //    if (Session["ForToken"] != null)
            //    {
            //        DataTable dset = (DataTable)Session["ForToken"];
            //        if (dset != null)
            //        {
            //            //if (dset.Count > 0)
            //            //{
            //                if (dset.Rows.Count > 0)
            //                {
            //                    Session["ForToken"] = dset;
            //                    DataDynamics.Reports.ReportDefinition _reportDef = new ReportDefinition();
            //                    //DataDynamics.Reports.ReportDefinition _reportDef1 = new ReportDefinition();
            //                    if (chkdelivery.Checked == true)
            //                    {
            //                        _reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/PrintGatePass.rdlx")));
            //                    }
            //                    else
            //                    {
            //                        _reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/PrintToken.rdlx")));
            //                    }
            //                    // _reportDef1 = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/ExcludedDetails.rdlx")));

            //                    //DataTable dtable = dset.Tables[dset.Tables.Count - 5];
            //                    //DataTable Dt = dset.Tables[dset.Tables.Count - 1];
            //                    //Dataset1.Tables.Add(Dt.Copy());
            //                    //Dataset2.Tables.Add(dtable.Copy());
            //                    //DataDynamics.Reports.ReportRuntime _reportRuntime1 = new DataDynamics.Reports.ReportRuntime(_reportDef1);

            //                    DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef);
            //                    _reportRuntime.LocateDataSource += WRAXBDetails_LocateDataSource;
            //                    // _reportRuntime1.LocateDataSource += WRAXBDetails_LocateDataSource;
            //                    string exportFile = System.IO.Path.GetTempFileName() + ".pdf";
            //                    System.IO.FileInfo myFile = new System.IO.FileInfo(exportFile);
            //                    System.Collections.Specialized.NameValueCollection settings = new System.Collections.Specialized.NameValueCollection();
            //                    settings.Add("hideToolbar", "True");
            //                    settings.Add("hideMenubar", "True");
            //                    settings.Add("hideWindowUI", "True");
            //                    settings.Add("MarginLeft", "0.1in");
            //                    settings.Add("MarginRight", "0.1in");
            //                    settings.Add("MarginTop", "0.1in");
            //                    settings.Add("MarginBottom", "0.1in");
            //                    settings.Add("PageWidth", "9in");
            //                    settings.Add("PageHeight", "13in");
            //                    settings.Add("FitWindow", "True");

            //                    PdfRenderingExtension _renderingExtension = new DataDynamics.Reports.Rendering.Pdf.PdfRenderingExtension();
            //                    FileStreamProvider _provider = new DataDynamics.Reports.Rendering.IO.FileStreamProvider(myFile.Directory, Path.GetFileNameWithoutExtension(myFile.Name));
            //                    _reportRuntime.Render(_renderingExtension, _provider, settings);
            //                    Response.Clear();
            //                    Response.ContentType = "application/pdf";
            //                    string filename = Session["UserName"].ToString() + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".pdf";
            //                    string FullName = filename.Replace(" ", "");
            //                    Response.AddHeader("content-disposition", "attachment; filename=" + FullName);
            //                    Response.BinaryWrite(File.ReadAllBytes(exportFile));
            //                    myFile.Delete();
            //                }
            //                else
            //                {
            //                    lblStatus.Text = "No AWB's found for the agent to apply deals!!";
            //                    lblStatus.ForeColor = Color.Red;
            //                    return;
            //                }

            //            }
            //        //}
            //    }
            //}
            //catch
            //{
            //    lblStatus.Text = "Error while printing deal report; Please try again";
            //    lblStatus.ForeColor = Color.Red;
            //    return;
            //}
            #endregion

            DataTable dt = (DataTable)Session["ForToken"];

            if (gdvULDDetails.Rows.Count == 0 || dt == null)
            {
                lblStatus.Text = "Token details not available.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            //Logo
            System.IO.MemoryStream Logo = null;
            try
            {
                Logo = CommonUtility.GetImageStream(Page.Server);
                //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
            }
            catch (Exception ex)
            {
                Logo = new System.IO.MemoryStream();
            }
            
            DataTable dtMain = new DataTable();
            dtMain.Columns.Add("TokenNo", typeof(string));
            dtMain.Columns.Add("TokenDt", typeof(string));
            dtMain.Columns.Add("DriverName", typeof(string));
            dtMain.Columns.Add("LicenseNo", typeof(string));
            dtMain.Columns.Add("PhoneNo", typeof(string));
            dtMain.Columns.Add("VehicleNo", typeof(string));
            dtMain.Columns.Add("DockNo", typeof(string));
            dtMain.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));


            dtAWB.Columns.Add("AWBPrefix", typeof(string));
            dtAWB.Columns.Add("AWBNo", typeof(string));
            dtAWB.Columns.Add("TotPcs", typeof(string));
            dtAWB.Columns.Add("GrWt", typeof(string));
            dtAWB.Columns.Add("CommCode", typeof(string));
            dtAWB.Columns.Add("CommDesc", typeof(string));
            dtAWB.Columns.Add("ULDNo", typeof(string));

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dtMain.Rows.Count < 1)
                {
                    DataRow drMainRow = dtMain.NewRow();
                    drMainRow["TokenNo"] = dt.Rows[0]["TokenNumber"].ToString();
                    drMainRow["TokenDt"] = dt.Rows[0]["TokenDate"].ToString();
                    drMainRow["DriverName"] = dt.Rows[0]["DriverName"].ToString();
                    drMainRow["LicenseNo"] = dt.Rows[0]["DLNumber"].ToString();
                    drMainRow["PhoneNo"] = dt.Rows[0]["Phone"].ToString();
                    drMainRow["VehicleNo"] = dt.Rows[0]["VehicleNo"].ToString();
                    drMainRow["DockNo"] = dt.Rows[0]["DockNo"].ToString();
                    drMainRow["Logo"] = Logo.ToArray();
                    dtMain.Rows.Add(drMainRow);
                }
                DataRow drAWBRow = dtAWB.NewRow();
                drAWBRow["AWBPrefix"] = dt.Rows[i]["AWBPrefix"].ToString();
                drAWBRow["AWBNo"] = dt.Rows[i]["AWBPrefix"].ToString() + '-' + dt.Rows[i]["AWBNumber"].ToString();
                drAWBRow["TotPcs"] = dt.Rows[i]["PiecesCount"].ToString();
                drAWBRow["GrWt"] = dt.Rows[i]["GrossWeight"].ToString();
                drAWBRow["CommCode"] = dt.Rows[i]["CommodityCode"].ToString();
                drAWBRow["CommDesc"] = dt.Rows[i]["CommodityDesc"].ToString();
                if (dt.Columns.Contains("ULDNo") == true)
                    drAWBRow["ULDNo"] = dt.Rows[i]["ULDNo"].ToString();
                else
                    drAWBRow["ULDNo"] = "";
                dtAWB.Rows.Add(drAWBRow);
            }
            ReportViewer rptPrintToken = new ReportViewer();
            ReportDataSource rds1 = new ReportDataSource();
            LocalReport rep1 = null;
            rptPrintToken.ProcessingMode = ProcessingMode.Local;

            rep1 = rptPrintToken.LocalReport;

            if (chkdelivery.Checked == true)
                rep1.ReportPath = Server.MapPath("/Reports/rptPrintGatePass.rdlc");
            else
                rep1.ReportPath = Server.MapPath("/Reports/rptPrintToken.rdlc");
            rds1.Name = "dsPrintToken_DataTable1";
            rds1.Value = dtMain;
            rep1.DataSources.Add(rds1);

            rptPrintToken.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandlerForToken);
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
                byte[] bytes = rptPrintToken.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                if (chkdelivery.Checked == true)
                    Response.AddHeader("content-disposition", "attachment; filename=" + "GatePass" + "." + ".pdf");
                else
                    Response.AddHeader("content-disposition", "attachment; filename=" + "TokenReceipt" + "." + ".pdf");
                Response.BinaryWrite(bytes); // create the file
                Response.Flush();


                //Response.End();
            }
            catch (Exception ex)
            {

            }
            #endregion
        }
        
        public void ItemsSubreportProcessingEventHandlerForToken(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsPrintToken_DataTable2", dtAWB));
        }

        ////private void WRAXBDetails_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        ////{

        ////    //string dname = e.DataSetName;
        ////    ////  if (dname.ToLower() == "DataSetValue")    
        ////    //if (dname == "DataSet1")
        ////    //{
        ////    //    e.Data = Dataset1;

        ////    //}
        ////    //else
        ////    //    if (dname == "DataSet3")
        ////    //    {
        ////    //        e.Data = Dataset3;
        ////    //    }
        ////    //    else
        ////    //    {
        ////    //        e.Data = Dataset2;
        ////    //    }
        ////    DataTable ds = (DataTable)Session["ForToken"];
        ////    if (ds.Rows.Count > 0)
        ////    {
        ////        e.Data = ds;
        ////        //Session["Export"] = ds.Tables[0];
        ////    }

        ////    //else if (ds.Tables[0].Rows.Count <= 0)
        ////    //{

        ////    //}


        ////}

        protected bool Validation()
        {
            try
            {
                //if (txtFlightID.Text.Trim() == "")
                //{
                //    lblStatus.Text = "Please Enter Flight Number.";
                //    lblStatus.ForeColor = System.Drawing.Color.Green;
                //    return false;
                //}
                if (txtDriverName.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Enter Driver Name.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    return false;
                }
                if (txtDLNumber.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Enter DL Number.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    return false;
                }
                if (txtVehicleNo.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Enter Vehicle Number.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void LoadToken()
        {
            string[] PName = new string[1];
            object[] PValue = new object[1];
            SqlDbType[] PType = new SqlDbType[1];
            DataSet ds = null;

            try
            {
                #region Parameter

                PName[0] = "TKNTYPE";

                if (chkdelivery.Checked == true)
                {
                    PValue[0] = "GTP";
                }
                else
                {
                    PValue[0] = "TKN";
                }

                PType[0] = SqlDbType.VarChar;

                #endregion
                ds = da.SelectRecords("GetToken", PName, PValue, PType);

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddltkn.DataSource = ds;
                            ddltkn.DataMember = ds.Tables[0].TableName;
                            ddltkn.DataValueField = ds.Tables[0].Columns["TokenNumber"].ColumnName;

                            ddltkn.DataTextField = ds.Tables[0].Columns["TokenNumber"].ColumnName;
                            ddltkn.DataBind();
                            ddltkn.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                PName = null;
                PValue = null;
                PType = null;
                ds = null;
            }
        }
    }
}