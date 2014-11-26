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

namespace ProjectSmartCargoManager
{
    public partial class GHA_Imp_BreakULD : System.Web.UI.Page
    {
        
        #region Variables
        BookingBAL objBLL = new BookingBAL();
        BLExpManifest objExpMani = new BLExpManifest();
        BLBuildULD objBuildULD = new BLBuildULD();
        BALBreakULD ObjCustoms = new BALBreakULD();

        DataTable MemDetails = new DataTable("Brk_Var_dtMem");
        DataTable tabMenifestDetails = new DataTable("Brk_Var_dtMnfDetails");
        DataSet dsAWBData = new DataSet("Brk_Var_dsAWBData");
       
        bool chkduplicate = false;
        string SelectedULDnos, Destination;

        int TotalULD = 0, totalULDAWB = 0, totalULDPCS = 0, totalAWB = 0, totalAWBPCS = 0;
        float totalAWBWt = 0, totalAWBVol = 0, totalULDVol = 0, totalULDWt = 0;
        ArrayList ULDDestpt = new ArrayList();
        MasterBAL objMst = new MasterBAL();
        DateTime dtCurrentDate = DateTime.Now;
        private string POLairport = "";
        private string FltNumber = "";
        private string Fltdate = "";
        private string FrommailId = "";
        string fblmsg;
        string ffmmsg;
        bool ChkSuperUserOffload = false;
        #endregion

        #region Form Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dtCurrentDate = (DateTime)Session["IT"];
                if (!IsPostBack)
                {
                    try
                    {
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
                        if (Session["AirlinePrefix"] != null)
                            txtFlightPrefix.Text = Convert.ToString(Session["AirlinePrefix"]);

                        ClearSession();
                        hdnManifestFlag.Value = "";

                        string UserName = Session["Username"].ToString();
                        string StationCode = Session["Station"].ToString();

                        lblDepAirport.Text = Convert.ToString(Session["Station"]);
                        
                        TextBoxdate.Text = dtCurrentDate.ToString("dd/MM/yyyy");

                        ShowULDAWBSummary();

                        #region Temp table for AWB Tab
                        DataTable MemDetails = new DataTable("Bruld_PgLd_memDt");

                        MemDetails.Columns.Add("ULDno");
                        MemDetails.Columns.Add("POU");
                        MemDetails.Columns.Add("TareWeight");

                        Session["Break_DataTableTemp"] = (DataTable)MemDetails;
                        MemDetails.Dispose();
                        #endregion Temp table for AWB Tab


                        #region Temp table for Manifest grid
                        DataTable tabMenifestDetails = new DataTable("Bruld_Pgld_dtManif");

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

                        Session["Break_ManifestGridData"] = (DataTable)tabMenifestDetails;
                        
                        tabMenifestDetails.Dispose();
                        #endregion Temp table for Manifest grid
                        LoadGridRoutingDetail();
                        FillIrregularityCode();
                        AWBDesignators();
                        FillDDLULDNumber();

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>return disable();</script>", false);

                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion Form Load

        #region Flight Clear Button  Click
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            txtFlightID.Text = "";
            TextBoxdate.Text = dtCurrentDate.ToString("dd/MM/yyyy");// System.DateTime.Now.ToString("dd/MM/yyyy");
            txtFlightID.Focus();

            //Need to add the logic to clear the screen.
            Response.Redirect("~/GHA_Imp_BreakULD.aspx",false);
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
            catch (Exception)
            {

            }
        }
        #endregion Load Plan List Button Click

        #region Load Plan AWB Grid Fill

        public void FillAWBGrid()
        {
            //DataSet dsawb = null;
            DataSet dsawb = new DataSet("Bruld_dsLoadPlan");

            try
            {
                string flightID = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                DateTime FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);
                
                dsawb = objExpMani.GetAwbTabdetails(lblDepAirport.Text, flightID, FlightDate);
                if (dsawb != null)
                {
                    gdvULDLoadPlanAWB.DataSource = dsawb;
                    gdvULDLoadPlanAWB.DataBind();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dsawb != null)
                {
                    dsawb.Dispose();
                }
            }
        }
        #endregion Load Plan AWB Grid Fill

        public void FillAWBGridMain()
        {
            //DataSet dsawb = null;
            DataSet dsawb = new DataSet("Brkuld_ds_FillAWB");

            try
            {
                string flightID = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                //dsawb = new DataSet();

                DateTime FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);
                //11 may addded
                dsawb = objExpMani.GetAwbTabdetails(lblDepAirport.Text, flightID, FlightDate);

                Session["Break_dsLoadBreak"] = dsawb;

                DataTable MemDetails = new DataTable("Brkuld_FillAWB_dtMem");

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
                
                if (MemDetails != null)
                    MemDetails.Dispose();

                try
                {
                    if (dsawb != null)
                    {
                        if (dsawb.Tables[3].Rows.Count > 0)
                        {
                            gdvULDLoadPlan.DataSource = dsawb.Tables[3];
                            Session["Break_ULDData"] = dsawb.Tables[3];
                            Session["Break_ULDAWBData"] = dsawb.Tables[4];
                            gdvULDLoadPlan.DataBind();
                        }
                    }
                }
                catch (Exception)
                {
                }
                if (dsawb != null)
                {
                    for (int i = 0; i < dsawb.Tables[0].Rows.Count; i++)
                    {
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAWBno")).Text = dsawb.Tables[0].Rows[i][0].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblPieces")).Text = dsawb.Tables[0].Rows[i][1].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblWeight")).Text = dsawb.Tables[0].Rows[i][2].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlPCS")).Text = dsawb.Tables[0].Rows[i][3].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlWgt")).Text = dsawb.Tables[0].Rows[i][4].ToString();

                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotPCS")).Text = dsawb.Tables[0].Rows[i]["AWBPcs"].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotWgt")).Text = dsawb.Tables[0].Rows[i]["AWBGwt"].ToString();
                    }
                    Session["Break_AWBdata"] = dsawb.Tables[0];

                    Session["Break_AddToManifestAWBdata"] = dsawb.Tables[0];
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dsawb != null)
                {
                    dsawb.Dispose();
                }
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
                Bulk = gdvULDDetails.Rows[intCount].Cells[1].Text.Trim();

                if (Bulk.ToUpper() == "BULK")
                {
                    BulkCount = BulkCount + 1;
                    BulkPcs = BulkPcs + Convert.ToInt32(gdvULDDetails.Rows[intCount].Cells[8].Text.Trim());
                    BulkWt = BulkWt + Convert.ToDecimal(gdvULDDetails.Rows[intCount].Cells[9].Text.Trim());
                    BulkVol = BulkVol + Convert.ToDecimal(gdvULDDetails.Rows[intCount].Cells[10].Text.Trim());
                }
                else
                {
                    ULDCount = ULDCount + 1;
                    ULDPcs = ULDPcs + Convert.ToInt32(gdvULDDetails.Rows[intCount].Cells[8].Text.Trim());
                    ULDWt = ULDWt + Convert.ToDecimal(gdvULDDetails.Rows[intCount].Cells[9].Text.Trim());
                    ULDVol = ULDVol + Convert.ToDecimal(gdvULDDetails.Rows[intCount].Cells[10].Text.Trim());
                }
            }
        }
        #endregion

        #region Select ULD DDl Fill
        public void FillSelectULDDDL()
        {
            //DataSet dsULD = null;
            DataSet dsULD = new DataSet("Brkuld_dsULD");

            try
            {
                //ddlSelectULD.Items.Clear();
                string flightID = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                //dsULD = new DataSet();

                if (dsULD != null)
                {
                    Session["Break_ULDData"] = dsULD.Tables[0];
                }
                if (dsULD != null)
                {
                    //ddlSelectULD.Items.Add("Select");
                    // ddlSelectULD.Items.Add("Bulk");

                    //ddlSelectULD.DataSource = dsULD;
                    //ddlSelectULD.DataTextField = "ULDno";
                    //ddlSelectULD.DataValueField = "ULDno";
                    //ddlSelectULD.DataBind();

                }
                else
                {
                    //ddlSelectULD.Items.Add("Select ULD");
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

        #region Clear Gridview ULD and all textbox
        protected void ClearGrid()
        {
            gdvULDDetails.DataSource = null;
            gdvULDDetails.DataBind();
        }
        #endregion

        #region createPO
        protected void createPO()
        {
        }
        #endregion createPO

       #region Add Checked AWB Data rows to Manifest Grid

        public void AddAWBDatarowsToManifestGrid()
        {
            //DataTable dsawb1 = null;
            DataTable dsawb1 = new DataTable("Brkuld_dt1_AddAwb");
            //DataSet dsawbData = null;
            DataSet dsawbData = new DataSet("Brkuld_ds1_AddAwb");

            try
            {
                string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "",
                    Updatedby = "", AWBDest = "", CommoCode = "", AWBOrigin = "";
                int PCS = 0, AVLPCS = 0, AWBPcs = 0;
                double WGT = 0.0, AVLWGT = 0.0, AWBGwt = 0;

                string desc = "", Vol = "";
                bool IsManifest = false;
                string IsBonded = string.Empty, AWBType = string.Empty;

                FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                POL = Convert.ToString(Session["Station"]);

                POU = null;
                Updatedby = Convert.ToString(Session["Username"]);
                ULDno = "Bulk";
                DateTime FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);
                //dsawb1 = new DataTable();
                //dsawbData = new DataSet();

                dsawbData = objExpMani.GetAwbTabdetails(lblDepAirport.Text, FLTno, FlightDate);

                dsawb1 = (DataTable)Session["Break_AddToManifestAWBdata"];

                if (Session["Break_AWBdata"] != null)
                {
                    dsawb1 = (DataTable)Session["Break_AWBdata"];
                }

                IsManifest = false;
                for (int i = 0; i < dsawb1.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked == true)
                    {
                        ((CheckBox)gdvULDLoadPlanAWB.Rows[i].FindControl("Check2")).Checked = false;

                        //Appended 25apr
                        AWBno = ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAWBno")).Text; //gdvULDLoadPlanAWB.Rows[i].Cells[1].Text;
                        PCS = Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblPieces")).Text);
                        WGT = Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblWeight")).Text);
                        AVLPCS = Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlPCS")).Text);
                        AVLWGT = Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlWgt")).Text);

                        AWBPcs = Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotPCS")).Text);
                        AWBGwt = Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotWgt")).Text);

                        try
                        {
                            if (dsawb1.Rows.Count > 0)
                            {
                                for (int t = 0; t < dsawb1.Rows.Count; t++)
                                {
                                    if (dsawb1.Rows[t][0].ToString() == AWBno)
                                    {
                                        AWBDest = dsawbData.Tables[1].Rows[t][5].ToString();
                                        desc = dsawbData.Tables[1].Rows[t][6].ToString();
                                        Vol = dsawbData.Tables[1].Rows[t][7].ToString();
                                        AWBOrigin = dsawbData.Tables[1].Rows[t]["OrginCode"].ToString();

                                        CommoCode = dsawbData.Tables[1].Rows[t]["CommodityCode"].ToString();
                                        IsBonded = dsawbData.Tables[1].Rows[t]["IsBonded"].ToString();
                                        AWBType = dsawbData.Tables[1].Rows[t]["Type"].ToString();
                                        //Added on 27 sept
                                        dsawb1.Rows[t].Delete();
                                        dsawbData.Tables[1].Rows[t].Delete();
                                    }
                                }
                                dsawb1.AcceptChanges();
                                dsawbData.Tables[1].AcceptChanges();
                                //adding collected data to gridview rows one by one
                            }
                        }
                        catch (Exception)
                        {
                        }

                        tabMenifestDetails = (DataTable)Session["Break_ManifestGridData"];
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
                        catch (Exception)
                        {
                        }
                        if (IsAvialble == false)
                        {
                            //DataSet dsDestDetails = null;
                            DataSet dsDestDetails = new DataSet("Brkuld_dsDest");
                            try
                            {
                                string ActAWB = AWBno.Substring(AWBno.Length - 8);

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
                            catch (Exception)
                            {
                            }
                            finally
                            {
                                if (dsDestDetails != null)
                                {
                                    dsDestDetails.Dispose();
                                }
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
                            l_Datarow["LoadingPriority"] = "";
                            l_Datarow["Remark"] = "";
                            l_Datarow["Bonded"] = IsBonded;

                            tabMenifestDetails.Rows.Add(l_Datarow);
                        }

                        if (tabMenifestDetails != null && tabMenifestDetails.Rows.Count > 0)
                        {
                            Session["Break_ManifestGridData"] = tabMenifestDetails;
                        }

                        gdvULDDetails.DataSource = "";

                        gdvULDDetails.DataSource = (DataTable)Session["Break_ManifestGridData"];
                        gdvULDDetails.DataBind();

                        Session["Break_GDVULDDetails"] = tabMenifestDetails;
                    }
                }

                gdvULDLoadPlanAWB.DataSource = null;
                gdvULDLoadPlanAWB.DataBind();

                gdvULDLoadPlanAWB.DataSource = dsawb1;
                gdvULDLoadPlanAWB.DataBind();

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
                    }
                }
                Session["Break_AWBdata"] = dsawb1;
                Session["Break_AddToManifestAWBdata"] = dsawb1;

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
                ShowULDAWBSummary();
            }
            catch (Exception)
            { }
            finally
            {
                if (dsawb1 != null)
                    dsawb1.Dispose();
                if (dsawbData != null)
                    dsawbData.Dispose();
            }
        }
        #endregion Add Checked AWB Data rows to Manifest Grid

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
            try
            {
                string strMode = "";
                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby, counter = "", SCC = "", Desc = "";
                    int PCS = 0, AVLPCS = 0;
                    double WGT = 0.0, AVLWGT = 0.0, ULDwgt = 0.0, VOL = 0;
                    string Updatedon = "";
                    string LoadingPriority = "", Remark = "", ULDDest = "", AWBOrigin = "", IsBonded = "";

                    FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                    Updatedby = Convert.ToString(Session["Username"]);
                    ULDno = gdvULDDetails.Rows[i].Cells[1].Text.Trim();
                    POU = gdvULDDetails.Rows[i].Cells[2].Text.Trim();
                    POL = gdvULDDetails.Rows[i].Cells[3].Text.Trim();// Session["Station"].ToString(); 

                    ULDDest = gdvULDDetails.Rows[i].Cells[4].Text.Trim();
                    if (gdvULDDetails.Rows[i].Cells[5].Text.Trim() == "&nbsp;")
                    {
                        counter = "0";
                    }
                    else
                    {
                        counter = gdvULDDetails.Rows[i].Cells[5].Text.Trim();
                    }
                    AWBno = gdvULDDetails.Rows[i].Cells[6].Text.Trim();
                    AWBno = AWBno.Substring(AWBno.Length - 8);
                    if (gdvULDDetails.Rows[i].Cells[7].Text == "&nbsp;")
                    {
                        SCC = "";
                    }
                    else
                    {
                        SCC = gdvULDDetails.Rows[i].Cells[7].Text;
                    }

                    PCS = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[8].Text);
                    WGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[9].Text);
                    if (gdvULDDetails.Rows[i].Cells[10].Text == "&nbsp;")
                    {
                        VOL = 0;
                    }
                    else
                    {
                        VOL = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[10].Text);
                    }
                    AVLPCS = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[13].Text);
                    AVLWGT = Convert.ToDouble(gdvULDDetails.Rows[i].Cells[14].Text);
                    if (gdvULDDetails.Rows[i].Cells[15].Text == "&nbsp;")
                    {
                        Desc = "";
                    }
                    else
                    {
                        Desc = gdvULDDetails.Rows[i].Cells[15].Text;
                        Desc = Desc.Replace("&amp;", "&");
                    }

                    if (gdvULDDetails.Rows[i].Cells[19].Text == "&nbsp;")
                    {
                        LoadingPriority = "";
                    }
                    else
                    {
                        LoadingPriority = ((DropDownList)gdvULDDetails.Rows[i].FindControl("ddlLoadingPriority")).SelectedItem.Text.ToString();
                    }

                    if (gdvULDDetails.Rows[i].Cells[20].Text == "&nbsp;")
                    {
                        Remark = "";
                    }
                    else
                    {
                        Remark = ((TextBox)gdvULDDetails.Rows[i].FindControl("txtRemark")).Text.ToString();
                    }

                    if (gdvULDDetails.Rows[i].Cells[21].Text == "&nbsp;")
                    {
                        IsBonded = "";
                    }
                    else
                    {
                        IsBonded = gdvULDDetails.Rows[i].Cells[21].Text.Trim();
                    }

                    string CurrLoc = gdvULDDetails.Rows[i].Cells[3].Text; ;// Session["Station"].ToString();

                    Updatedon = dtCurrentDate.ToString("yyyy/MM/dd"); //DateTime.Now.ToString("yyyy/MM/dd");
                    strMode = Convert.ToString(Session["Break_Split"]);
                    bool IsManifest = false;
                    IsManifest = bool.Parse(gdvULDDetails.Rows[i].Cells[18].Text);
                    DateTime FltDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);
                    AWBOrigin = gdvULDDetails.Rows[i].Cells[16].Text.Trim();
                    string tailNo = string.Empty;

                    if (strMode != "O")
                    {
                        //If condition added for save POL AWB's match with this location  
                        if (Session["Station"].ToString() == POL)
                        {
                            string strResult = string.Empty;
                            string strIdentifier = Convert.ToString(Session["Break_Identifier"]);
                            //appended 25apr -add awb to manifest completed so they dont appear in awb again
                            bool result = objExpMani.AddULDAWBassocition(FLTno, POL, POU, 
                                ULDno, ULDwgt, AWBno, PCS, WGT, AVLPCS, AVLWGT, Updatedby, 
                                dtCurrentDate, FltDate, strIdentifier, ref strResult,"","","");                    //

                            if (strResult.Length > 0)
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = strResult;
                                return;
                            }
                        }
                    }
                }
                if (strMode == "O")
                {
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "Manifest Version Saved Successfully";
                    return;
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion Save Manifest Grid data To DB

        protected void btnSplitUnassign_Click(object sender, EventArgs e)
        {
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
                    string txt = gdvULDDetails.Rows[k].Cells[18].Text;
                    if (((CheckBox)gdvULDDetails.Rows[k].FindControl("Check0")).Checked == true)
                    {
                        if (bool.Parse(gdvULDDetails.Rows[k].Cells[18].Text))
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "This AWB is Manifested so we can't Split and UnAssign this AWB";
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
                Session["Break_Split"] = "U";
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

                DataTable DTAWBDetails = new DataTable("Brkuld_dt_btnSplite");

                DTAWBDetails.Columns.Add("AWBNo");
                DTAWBDetails.Columns.Add("Pieces");
                DTAWBDetails.Columns.Add("Weight");
                DTAWBDetails.Columns.Add("AvlPCS");
                DTAWBDetails.Columns.Add("AvlWgt");
                for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[j].FindControl("Check0")).Checked == true)
                    {
                        DataRow dr;
                        //Changes  on 2 oct
                        dr = DTAWBDetails.NewRow();
                        dr[0] = gdvULDDetails.Rows[j].Cells[6].Text;
                        dr[1] = gdvULDDetails.Rows[j].Cells[8].Text;
                        dr[2] = gdvULDDetails.Rows[j].Cells[9].Text;
                        dr[3] = gdvULDDetails.Rows[j].Cells[8].Text;
                        dr[4] = gdvULDDetails.Rows[j].Cells[9].Text;
                        DTAWBDetails.Rows.Add(dr);
                    }
                }

                Session["Break_AWBdata1"] = DTAWBDetails;
                pnlGrid.Visible = true;
                pnlGrid.Style["Z-INDEX"] = "150";
                pnlGrid.Style["LEFT"] = "342px";
                pnlGrid.Style["POSITION"] = "absolute";
                pnlGrid.Style["TOP"] = "320px";
                try
                {
                    LoadGridSchedule();

                    DataTable dt1 = new DataTable("Brkuld_Spt_Dt");
                    dt1 = DTAWBDetails;

                    DataTable dtCurrentTable = new DataTable("Brkuld_BtnSpt_Dtcurr");
                    dtCurrentTable = (DataTable)Session["Break_AWBdata1"];

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

                            ViewState["CurrentTable1"] = dtCurrentTable;
                        }
                    }
                    AllButtonStatus(false);
                    if (dt1 != null)
                    {
                        dt1.Dispose();
                    }
                    if (dtCurrentTable != null)
                    {
                        dtCurrentTable.Dispose();
                    }
                }
                catch (Exception)
                {
                }
                if (DTAWBDetails != null)
                {
                    DTAWBDetails.Dispose();
                }
            }
            catch (Exception)
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

                Session["Break_Split"] = "A";
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

                DataTable DTAWBDetails = new DataTable("Brkuld_BtnAss_dt2");

                DTAWBDetails.Columns.Add("AWBNo");
                DTAWBDetails.Columns.Add("Pieces");
                DTAWBDetails.Columns.Add("Weight");
                DTAWBDetails.Columns.Add("AvlPCS");
                DTAWBDetails.Columns.Add("AvlWgt");

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

                        DTAWBDetails.Rows.Add(dr);

                    }
                }

                Session["Break_AWBdata1"] = DTAWBDetails;

                pnlGrid.Visible = true;
                pnlGrid.Style["Z-INDEX"] = "150";
                pnlGrid.Style["LEFT"] = "342px";
                pnlGrid.Style["POSITION"] = "absolute";
                pnlGrid.Style["TOP"] = "260px";
                try
                {
                    LoadGridSchedule();

                    DataTable dt1 = new DataTable("Brkuld_btnAss_dtAwb2");
                    dt1 = DTAWBDetails;

                    DataTable dtCurrentTable = new DataTable("Brkuld_btnsAss_dtCrnt");
                    dtCurrentTable = (DataTable)Session["Break_AWBdata1"];

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

                            ViewState["CurrentTable1"] = dtCurrentTable;

                        }
                    }

                    AllButtonStatus(false);
                    if (dt1 != null)
                    {
                        dt1.Dispose();
                    }
                    if (dtCurrentTable != null)
                    {
                        dtCurrentTable.Dispose();
                    }
                }
                catch (Exception)
                {
                }
                if (DTAWBDetails != null)
                {
                    DTAWBDetails.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }

        public void ShowSplitAWBGrid()
        {
            //DataTable dtSplitData = null;
            DataTable dtSplitData = new DataTable("Brkuld_ShwSpt_dt");

            try
            {
                //dtSplitData = new DataTable();

                dtSplitData = (DataTable)Session["Break_AWBSplitData"];
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
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dtSplitData != null)
                {
                    dtSplitData.Dispose();
                }
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
                    if (((CheckBox)gdvULDDetails.Rows[j].FindControl("Check0")).Checked == true)
                    {
                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(gdvULDDetails.Rows[j].Cells[8].Text))
                        {
                            Label1.Text = "Please enter valid Pcs";
                            Label1.ForeColor = Color.Red;
                            return;
                        }
                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToDouble(gdvULDDetails.Rows[j].Cells[9].Text))
                        {
                            Label1.Text = "Please enter valid Weight";
                            Label1.ForeColor = Color.Red;
                            return;
                        }
                        if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(gdvULDDetails.Rows[j].Cells[8].Text))
                        {
                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) < Convert.ToDouble(gdvULDDetails.Rows[j].Cells[9].Text))
                            {
                                Label1.Text = "Please enter valid Pcs";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                        }

                        if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) == Convert.ToDouble(gdvULDDetails.Rows[j].Cells[9].Text))
                        {
                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(gdvULDDetails.Rows[j].Cells[8].Text))
                            {
                                Label1.Text = "Please enter valid Weight";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
                        }
                        break;
                    }
                }
                string strMode = Convert.ToString(Session["Break_Split"]);
                double OffloadWGT = 0.0;

                //Code starts for Return to Shipper
                if (strMode == "RSI")
                {
                    BLExpManifest objExpManifest = new BLExpManifest();
                    string ActualFltNo = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                    DateTime ActualFltDate = DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null);
                    string OffloadLoc = lblDepAirport.Text.Trim();
                    string strAWBno = ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim().Substring(((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim().Length - 8, 8);
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
                    string POLoading = ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).Text;
                    string POUnloading = ((TextBox)grdRouting.Rows[0].FindControl("txtFltDest")).Text;

                    if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAvlPCS")).Text))
                    {
                        Label1.Text = "Please enter valid Pcs";
                        Label1.ForeColor = Color.Red;
                        return;
                    }
                    if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtAwlWeight")).Text))
                    {
                        Label1.Text = "Please enter valid Weight";
                        Label1.ForeColor = Color.Red;
                        return;
                    }
                    string PartnerCode = ((DropDownList)grdRouting.Rows[0].FindControl("ddlPartner")).SelectedValue;
                    string Partnertype = ((DropDownList)grdRouting.Rows[0].FindControl("ddlPartnerType")).SelectedValue;

                    bool blnResult = objExpManifest.OffLoadShipmentinManifest(ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim(), "", OffloadLoc, strAWBno, APCS, AWGT, OffloadPCS, OffloadWGT,
                       UserName, POLoading, POUnloading, string.Empty, Convert.ToDateTime(Session["IT"]), "Return To Shipper", strMode, DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null), Convert.ToDateTime(Session["IT"]), PartnerCode, Partnertype);

                    objExpManifest = null;
                    if (blnResult)
                    {
                        pnlGrid.Visible = false;
                        return;
                    }
                }

                //End of Return to shipper
                if (strMode == "U")
                {
                    //code to do the Split and Assign functionality. //Added by poorna.

                    //code to do the Split and Assign functionality. //Added by poorna.

                    for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                    {
                        if (((CheckBox)gdvULDDetails.Rows[j].FindControl("Check0")).Checked == true)
                        {
                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(gdvULDDetails.Rows[j].Cells[8].Text))
                            {
                                Label1.Text = "Please enter valid Pcs";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToDouble(gdvULDDetails.Rows[j].Cells[9].Text))
                            {
                                Label1.Text = "Please enter valid Weight";
                                Label1.ForeColor = Color.Red;
                                return;
                            }

                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) == Convert.ToInt32(gdvULDDetails.Rows[j].Cells[8].Text))
                            {
                                if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) < Convert.ToDouble(gdvULDDetails.Rows[j].Cells[9].Text))
                                {
                                    Label1.Text = "Please enter valid Weight";
                                    Label1.ForeColor = Color.Red;
                                    return;
                                }
                            }

                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) == Convert.ToDouble(gdvULDDetails.Rows[j].Cells[9].Text))
                            {
                                if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) < Convert.ToInt32(gdvULDDetails.Rows[j].Cells[8].Text))
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
                    return;
                }

                if (strMode == "O")
                {
                    //code to do the Split and Assign functionality. //Added by poorna.
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

                            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text != "" && ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
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
                    //Verify the entered flight number
                    if (txtNextFlight.Text.ToUpper().Trim() != "")
                    {
                        string Origin = Convert.ToString(Session["Station"]);
                        string FltNo = txtNextFlight.Text.ToUpper().Trim();
                        string Dest = string.Empty;
                        DateTime dt = DateTime.ParseExact(txtNFltDate.Text.Trim(), "dd/MM/yyyy", null);

                        int diff = (dt - dtCurrentDate.Date).Days;

                        for (int ICount = 0; ICount < gdvULDDetails.Rows.Count; ICount++)
                        {
                            if (((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim() == gdvULDDetails.Rows[ICount].Cells[6].Text.Trim())
                            {
                                Dest = gdvULDDetails.Rows[ICount].Cells[4].Text.Trim();
                                break;
                            }
                        }

                        ShowFlightsBAL objBAL = new ShowFlightsBAL();
                        //DataSet objDS = null;
                        DataSet objDS = new DataSet("Brkuld_BtnAddMnf_ds");

                        string errormessage = string.Empty;
                        string flightID = ddlFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();
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

                        if (objDS != null)
                            objDS.Dispose();

                        if (blnExists == false)
                        {
                            Label1.ForeColor = Color.Red;
                            Label1.Text = "Please enter valid flight number.";
                            return;
                        }
                    }
                    
                    SaveOffLoadDetails();
                    AllButtonStatus(true);
                    return;
                }

                if (strMode == "R")
                {
                    if (UpdatePanelRouteDetails.Visible == true)
                    {
                        int chkPCS = 0;
                        double chkWGT = 0.0;
                        for (int i = 0; i < grdRouting.Rows.Count; i++)
                        {
                            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text == "Select")
                            {
                                Label1.Text = "Please enter valid flight";
                                Label1.ForeColor = Color.Red;
                                return;
                            }
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


                            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text != "" && ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
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
                    //Verify the entered flight number
                    if (txtNextFlight.Text.ToUpper().Trim() != "")
                    {
                        string Origin = Convert.ToString(Session["Station"]);
                        string FltNo = txtNextFlight.Text.ToUpper().Trim();
                        string Dest = Convert.ToString(Session["Break_Dest"]);/// ddlMainPOU.SelectedItem.Value.ToString();
                        DateTime dt = DateTime.ParseExact(txtNFltDate.Text.Trim(), "dd/MM/yyyy", null);

                        int diff = (dt - dtCurrentDate.Date).Days;

                        ShowFlightsBAL objBAL = new ShowFlightsBAL();
                        //DataSet objDS = null;
                        DataSet objDS = new DataSet("Brkuld_btnAddMnf_dsFltLst");

                        string errormessage = string.Empty;
                        string flightID = ddlFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();

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
                        if (objDS != null)
                        {
                            objDS.Dispose();
                        }
                        if (blnExists == false)
                        {
                            Label1.ForeColor = Color.Red;
                            Label1.Text = "Please enter valid flight number.";
                            return;
                        }
                    }
                    SaveOffLoadDetails();
                    AllButtonStatus(true);
                    return;
                }
                if (strMode == "RS")
                {
                    for (int j = 0; j < gdvULDLoadPlanAWB.Rows.Count; j++)
                    {
                        //  if (((CheckBox)gdvULDLoadPlanAWB.Rows[j].FindControl("Check2")).Checked == true)
                        if (((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblAWBno")).Text == ((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text)
                        {
                            if (Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text) > Convert.ToInt32(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblPieces")).Text))
                            {
                                Label1.Text = "Please enter valid PCS";
                                Label1.ForeColor = Color.Red;
                                return;


                            }
                            if (Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text) > Convert.ToDouble(((Label)gdvULDLoadPlanAWB.Rows[j].FindControl("lblWeight")).Text))
                            {
                                Label1.Text = "Please enter valid PCS ";
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
                                    Label1.Text = "Please enter valid PCS";
                                    Label1.ForeColor = Color.Red;
                                    return;
                                }
                            }
                            break;
                        }
                    }
                    SaveAddToShipper();
                    AllButtonStatus(true);
                    return;
                }


                string ULDno = "", AWBno = "", POU = "", POL = "", FLTno = "", Updatedby = "", AWBDest = "", CommCode = "";
                int AVLPCS = 0, AWBPcs = 0;
                double WGT = 0.0, AVLWGT = 0.0, AWBGwt = 0;
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


                FLTno = ddlFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();
                POL = Convert.ToString(Session["Station"]);
                //POU = ddlMainPOU.SelectedItem.Value.ToString();
                POU = string.Empty;
                Updatedby = Convert.ToString(Session["Username"]);
                ULDno = "Bulk";
                string IsBonded = string.Empty, AWBType = string.Empty;

                DateTime FlightDate = DateTime.ParseExact(TextBoxdate.Text, "dd/MM/yyyy", null);

                DataSet dsawbData = new DataSet("Brkuld_addMnft_dsAwb");
                dsawbData = objExpMani.GetAwbTabdetails(lblDepAirport.Text, FLTno, FlightDate);

                DataTable dsawb1 = new DataTable("Brkuld_addMnft_dt1Awb");

                if (Session["Break_AWBdata"] != null)
                {
                    dsawb1 = (DataTable)Session["Break_AWBdata"];
                }
                else
                {

                    dsawb1 = dsawbData.Tables[0];
                }


                for (int i = 0; i < grdAWBs.Rows.Count; i++)
                {

                    //Appended 25apr
                    AWBno = ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text;

                    PCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text);
                    WGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text);
                    
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
                                        break;
                                    }
                                }

                                //adding collected data to gridview rows one by one
                            }
                            catch (Exception)
                            {
                            }


                            if (int.Parse(((Label)gdvULDLoadPlanAWB.Rows[k].FindControl("lblPieces")).Text) == 0)
                            {
                                //Added on 27 sept
                                //dsawb1.Tables[1].Rows.
                                dsawb1.Rows[k].Delete();
                                dsawbData.Tables[1].Rows[k].Delete();

                                dsawb1.AcceptChanges();
                                dsawbData.Tables[1].AcceptChanges();
                            }
                        }
                    }

                    tabMenifestDetails = (DataTable)Session["Break_ManifestGridData"];

                    int duplicnt = 0;
                    bool ismatch = false;
                    for (int chk = 0; chk < tabMenifestDetails.Rows.Count; chk++)
                    {
                        if (tabMenifestDetails.Rows[chk]["AWBno"].ToString() == AWBno)
                        {
                            tabMenifestDetails.Rows[chk]["PCS"] = (int.Parse(tabMenifestDetails.Rows[chk]["PCS"].ToString()) + PCS).ToString();
                            tabMenifestDetails.Rows[chk]["GrossWgt"] = (double.Parse(tabMenifestDetails.Rows[chk]["GrossWgt"].ToString()) + WGT).ToString();
                            duplicnt = duplicnt + 1;
                            ismatch = true;
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

                        tabMenifestDetails.Rows.Add(l_Datarow);
                    }

                    if (tabMenifestDetails != null && tabMenifestDetails.Rows.Count > 0)
                    {
                        Session["Break_ManifestGridData"] = tabMenifestDetails;
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

                        }
                    }
                    Session["Break_AWBdata"] = dsawb1;// dsawbData.Tables[1];

                    gdvULDDetails.DataSource = "";

                    gdvULDDetails.DataSource = (DataTable)Session["Break_ManifestGridData"];
                    gdvULDDetails.DataBind();
                }

                try
                {
                    for (int k = 0; k < gdvULDDetails.Rows.Count; k++)
                    {
                        if (gdvULDDetails.Rows[k].Cells[6].Text.Trim() == tabMenifestDetails.Rows[k]["AWBNo"].ToString())
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
                                string AWB = gdvULDDetails.Rows[m].Cells[6].Text.Trim();
                                if (dsawbData.Tables[2].Rows[r]["AWBNo"].ToString() == AWB.Substring(AWB.Length - 8))//AWBno.SubString(AWB.Length-8)) 
                                {
                                    Remark = dsawbData.Tables[2].Rows[r]["Remarks"].ToString();
                                    ((TextBox)gdvULDDetails.Rows[m].FindControl("txtRemark")).Text = Remark;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }

                ShowULDAWBSummary();
                hdnManifestFlag.Value = "1";
                if (dsawbData != null)
                {
                    dsawbData.Dispose();
                }
                if (dsawb1 != null)
                {
                    dsawb1.Dispose();
                }
            }
            catch (Exception)
            { }

            pnlGrid.Visible = false;

            AllButtonStatus(true);
        }

        #region Loadgrid Intial Row
        private void LoadGridSchedule()
        {

            DataTable myDataTable = new DataTable("Brkuld_Ldgrd_dt3");
            DataColumn myDataColumn;
            DataSet Ds = new DataSet("Brkuld_Ldgrd_ds3");

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


            DataRow dr;
            dr = myDataTable.NewRow();
            //dr["RowNumber"] = 1;
            dr["AWBNo"] = "";//"5";
            dr["Pieces"] = "";// "5";
            dr["Weight"] = "";
            dr["AvlPCS"] = "";
            dr["AvlWgt"] = "";

            myDataTable.Rows.Add(dr);
            ViewState["CurrentTable11"] = myDataTable;
            //Bind the DataTable to the Grid

            grdAWBs.DataSource = null;
            grdAWBs.DataSource = myDataTable;
            grdAWBs.DataBind();

            if (myDataTable != null)
            {
                myDataTable.Dispose();
            }
            if (myDataColumn != null)
            {
                myDataColumn.Dispose();
            }
        }
        #endregion

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
            catch (Exception)
            { }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);

        }
        #endregion

        #region Send FFM
        protected void btnSendFFM_Click(object sender, EventArgs e)
        {
            try
            {
                string FlightNumber = ddlFlightCode.Text.ToString() + txtFlightID.Text.ToString();
                //DataSet ds = null;
                DataSet ds = new DataSet("Brkuld_SendFFM_ds");

                BALEmailID ObjEmail = new BALEmailID();

                try
                {
                    ds = ObjEmail.GetEmail(lblDepAirport.Text, Session["Break_Dest"].ToString(), "FFM", FlightNumber, ddlFlightCode.Text.ToString());
                    txtEmailID.Text = ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
                    lblMsgCommType.Text = ds.Tables[0].Rows[0]["MsgCommType"].ToString();
                    if (lblMsgCommType.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) || lblMsgCommType.Text.Equals("SITA", StringComparison.OrdinalIgnoreCase))
                    {
                        GenerateSITAHeader(ds.Tables[0].Rows[0]["PartnerSITAiD"].ToString());
                    }
                }
                catch (Exception)
                { }
                if (ds != null)
                {
                    ds.Dispose();
                }
                ffmmsg = "ffm";
                lblMsg.Text = "FFM";
                txtMessageBody.Text = cls_BL.EncodeFFM(Session["Break_POLairport"].ToString(), 
                    Session["Break_FltNumber"].ToString(), Session["Break_Fltdate"].ToString());
                Session["Break_ffmmsg"] = "ffm";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", 
                    "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                ObjEmail = null;
            }
            catch (Exception)
            { }
        }
        #endregion

        private void LoadAWBDataGrid(DataTable pi_objDataTable)
        {
            try
            {
                DataTable MemDetails = new DataTable("Brkuld_ldMem_dt"); // (DataTable)Session["Break_AWBdata"];
                
                MemDetails.Columns.Add("lblAWBno");
                MemDetails.Columns.Add("lblPieces");
                MemDetails.Columns.Add("lblWeight");
                MemDetails.Columns.Add("lblAvlPCS");

                MemDetails.Columns.Add("lblAvlWgt");
                MemDetails.Columns.Add("lblTotPCS");
                MemDetails.Columns.Add("lblTotWgt");

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
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlPCS")).Text = pi_objDataTable.Rows[i][3].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblAvlWgt")).Text = pi_objDataTable.Rows[i][4].ToString();

                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotPCS")).Text = pi_objDataTable.Rows[i][6].ToString();
                        ((Label)gdvULDLoadPlanAWB.Rows[i].FindControl("lblTotWgt")).Text = pi_objDataTable.Rows[i][7].ToString();

                    }
                    Session["Break_AWBdata"] = pi_objDataTable;
                }
                if (MemDetails != null)
                {
                    MemDetails.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }

        private void SplitAndUnAssignfunction()
        {
            try
            {
                string AWBno = "", POU = "", POL = "", FLTno = "", 
                    Updatedby = "";
                int PCS = 0;
                double WGT = 0.0;

                DataTable dtULDDetails = new DataTable("Brkuld_Fun_dtUld3");
                dtULDDetails = (DataTable)Session["Break_GDVULDDetails"];

                DataTable ManifestData = new DataTable("Brkuld_Fun_dtManifest3");
                ManifestData = (DataTable)Session["Break_ManifestGridData"];

                FLTno = ddlFlightCode.Text.Trim() + txtFlightID.Text.Trim();
                POL = Convert.ToString(Session["Station"]);
                
                POU = string.Empty;
                Updatedby = Convert.ToString(Session["Username"]);

                PCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[0].FindControl("txtPCS")).Text);
                WGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[0].FindControl("txtweight")).Text);

                DataTable DTAWBDetails = new DataTable("Brkuld_Fun_dtAwb3");
                DTAWBDetails = (DataTable)Session["Break_AWBdata"];

                if (DTAWBDetails == null)
                {
                    //DTAWBDetails = new DataTable();

                    DTAWBDetails.Columns.Add("AWBNo");
                    DTAWBDetails.Columns.Add("Pieces");
                    DTAWBDetails.Columns.Add("Weight");
                    DTAWBDetails.Columns.Add("AvlPCS");
                    DTAWBDetails.Columns.Add("AvlWgt");

                    DTAWBDetails.Columns.Add("BookedPCS");
                    DTAWBDetails.Columns.Add("BookedWgt");
                }

                DataRow drNew = null;
                for (int i = 0; i < grdAWBs.Rows.Count; i++)
                {                    
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

                        }
                    }
                    for (int t = 0; t < gdvULDDetails.Rows.Count; t++)
                    {

                        if (gdvULDDetails.Rows[t].Cells[6].Text == ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text)
                        {
                            gdvULDDetails.Rows[t].Cells[8].Text = (Convert.ToInt32(gdvULDDetails.Rows[t].Cells[8].Text) - PCS).ToString();
                            gdvULDDetails.Rows[t].Cells[9].Text = (double.Parse(gdvULDDetails.Rows[t].Cells[9].Text) - WGT).ToString();
                            
                            ManifestData.Rows[t][7] = gdvULDDetails.Rows[t].Cells[8].Text;
                            ManifestData.Rows[t][8] = gdvULDDetails.Rows[t].Cells[9].Text;
                            actPCS = Convert.ToInt32(gdvULDDetails.Rows[t].Cells[13].Text);
                            Actwgt = double.Parse(gdvULDDetails.Rows[t].Cells[14].Text);

                            AWBPcs = Convert.ToInt32(gdvULDDetails.Rows[i].Cells[11].Text.Trim());
                            AWBWT = double.Parse(gdvULDDetails.Rows[i].Cells[12].Text.Trim());
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
                    }

                    LoadAWBDataGrid(DTAWBDetails);

                    //for (int l = 0; l < gdvULDDetails.Rows.Count; l++)
                    //{
                    for (int l = 0; l < ManifestData.Rows.Count; l++)
                    {
                        if (((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text == gdvULDDetails.Rows[l].Cells[6].Text)
                        {
                            int UnassignPCS = Convert.ToInt32(gdvULDDetails.Rows[l].Cells[8].Text);
                            //(Convert.ToInt32(gdvULDDetails.Rows[l].Cells[8].Text) - Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text));
                            //Addded on 26 Sept Delete particular row from grid
                            if (UnassignPCS <= 0)
                            {
                                ManifestData.Rows[l].Delete();
                            }
                        }

                    }

                    ManifestData.AcceptChanges();

                    Session["Break_ManifestGridData"] = ManifestData;

                    gdvULDDetails.DataSource = ManifestData;
                    gdvULDDetails.DataBind();

                }
                //Remove PCS from Manifested grid
                ShowULDAWBSummary();

                if (DTAWBDetails != null)
                {
                    DTAWBDetails.Dispose();
                }
                if (dtULDDetails != null)
                {
                    dtULDDetails.Dispose();
                }
                if (ManifestData != null)
                {
                    ManifestData.Dispose();
                }
            }
            catch (Exception)
            { }

            pnlGrid.Visible = false;
        }

        protected void btnOffload_Click(object sender, EventArgs e)
        {
            try
            {
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
                    string txt = gdvULDDetails.Rows[k].Cells[18].Text;
                    if (((CheckBox)gdvULDDetails.Rows[k].FindControl("Check0")).Checked == true)
                    {
                        if (!bool.Parse(gdvULDDetails.Rows[k].Cells[18].Text))
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
                if (check > 1)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Select only One ULD from Manifest TAB for Offload";
                    return;
                }

                //Added by Poorna
                Session["Break_Split"] = "O";
                btnAddManifest.Text = "Save";
               
                LoadAirlineCode("");
                UpdatePartnerCode(1);

                # region Code to get Offload reasons

                DataSet ds = new DataSet("Brkuld_Offload_dsRes");
                ds = objExpMani.GetOffloadReasons();
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
                catch (Exception) { }
                finally
                {
                    if (ds != null)
                    {
                        ds.Dispose();
                    }
                }
                #endregion


                lblReason.Visible = true;
                //   txtReason.Visible = true;
                grdAWBs.Columns[2].Visible = false;

                grdAWBs.Columns[3].Visible = false;
                grdAWBs.Columns[5].Visible = true;

                grdAWBs.Columns[6].Visible = true;

                DataTable DTAWBDetails = new DataTable("Brkuld_brnOff_awbDt");

                DTAWBDetails.Columns.Add("AWBNo");
                DTAWBDetails.Columns.Add("Pieces");
                DTAWBDetails.Columns.Add("Weight");
                DTAWBDetails.Columns.Add("AvlPCS");
                DTAWBDetails.Columns.Add("AvlWgt");
                DTAWBDetails.Columns.Add("Origin");
                DTAWBDetails.Columns.Add("Destination");

                for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[j].FindControl("Check0")).Checked == true)
                    {
                        DataRow dr;

                        //GridViewRow row = gdvULDLoadPlanAWB.Rows[j];
                        //dr = dt.NewRow();

                        // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                        dr = DTAWBDetails.NewRow();
                        string POL = gdvULDDetails.Rows[j].Cells[16].Text;
                        string POU = gdvULDDetails.Rows[j].Cells[17].Text;
                        string AWBNo = gdvULDDetails.Rows[j].Cells[6].Text;

                        dr[0] = gdvULDDetails.Rows[j].Cells[6].Text;
                        dr[1] = gdvULDDetails.Rows[j].Cells[8].Text;
                        dr[2] = gdvULDDetails.Rows[j].Cells[9].Text;
                        dr[3] = gdvULDDetails.Rows[j].Cells[8].Text;
                        dr[4] = gdvULDDetails.Rows[j].Cells[9].Text;
                        if (Session["Break_dsMani"] != null)
                        {
                            DataSet dsManifest = new DataSet("Brkuld_BtnOff_ManiDs");
                            dsManifest = (DataSet)Session["Break_dsMani"];

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
                        break;
                    }
                }

                Session["Break_AWBdata"] = DTAWBDetails;

                txtReason.Text = "";

                pnlGrid.Visible = true;
                UpdatePanelRouteDetails.Visible = true;

                if (UpdatePanelRouteDetails.Visible == true)
                {
                    for (int i = 0; i < grdRouting.Rows.Count; i++)
                    {
                        ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text = "";

                        ((TextBox)grdRouting.Rows[i].FindControl("txtPcs")).Text = "";

                        ((TextBox)grdRouting.Rows[i].FindControl("txtWt")).Text = "";

                    }
                }
                // pnlGrid.Attributes.item("style") = "Z-INDEX: 176; LEFT: 584px; POSITION: absolute; TOP: 176px";
                pnlGrid.Style["Z-INDEX"] = "150";
                pnlGrid.Style["LEFT"] = "342px";
                pnlGrid.Style["POSITION"] = "absolute";
                pnlGrid.Style["TOP"] = "260px";
                pnlGrid.Style["Width"] = "700px";

                //DataTable dt1 = null;
                DataTable dt1 = new DataTable("Brkuld_grdAWBs_Dt1");
                //DataTable dtCurrentTable = null;
                DataTable dtCurrentTable = new DataTable("Brkuld_grdAWBs_CrtDt");

                try
                {
                    LoadGridSchedule();
                    dt1 = DTAWBDetails;
                    dtCurrentTable = (DataTable)Session["Break_AWBdata"];
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

                            ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Enabled = false;

                            ViewState["CurrentTable1"] = dtCurrentTable;
                        }
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    if(dt1 != null)
                    {
                        dt1.Dispose();
                    }
                    if (dtCurrentTable != null)
                    {
                        dtCurrentTable.Dispose();
                    }
                }
                AllButtonStatus(false);
                if (DTAWBDetails != null)
                {
                    DTAWBDetails.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }

        private void SaveAddToShipper()
        {
            try
            {
                string AWBno = "", POU = "", POL = "", ActFLTno = "", Updatedby = "", 
                    OffLoadFltNo = "", OffloadLoc = "";
                int OffloadPCS = 0, AVLPCS = 0;
                double OffloadWGT = 0.0, AVLWGT = 0.0;
                DateTime dtOffLoadDate;
                string strMode = string.Empty;

                ActFLTno = ddlFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();
                POL = Convert.ToString(Session["Station"]);
                
                POU = string.Empty;
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
                    if (((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim() == gdvULDDetails.Rows[j].Cells[6].Text.Trim())
                    {
                        AVLPCS = Convert.ToInt32(gdvULDDetails.Rows[j].Cells[13].Text);
                        AVLWGT = Convert.ToDouble(gdvULDDetails.Rows[j].Cells[14].Text);
                    }
                }

                BLExpManifest objExpManifest = new BLExpManifest();

                bool blnResult = objExpManifest.ReturnToShipper(ActFLTno, ActFLTno, OffloadLoc, AWBno, AVLPCS, AVLWGT, OffloadPCS, OffloadWGT,
                    Updatedby, POL, POU, string.Empty, dtOffLoadDate, txtReason.Text.Trim(), strMode);

                if (blnResult)
                {
                    pnlGrid.Visible = false;
                }
            }
            catch (Exception)
            {
            }
        }

        private void SaveOffLoadDetails()
        {
            try
            {
                string AWBno = "", POU = "", POL = "", ActFLTno = "", Updatedby = "", 
                    OffLoadFltNo = "", OffloadLoc = "";
                int OffloadPCS = 0, AVLPCS = 0;
                double OffloadWGT = 0.0, AVLWGT = 0.0;
                DateTime dtOffLoadDate;
                string strMode = string.Empty;

                ActFLTno = ddlFlightCode.Text.ToUpper().Trim() + txtFlightID.Text.ToUpper().Trim();
                
                strMode = (string)Session["Break_Split"];

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
                            if (((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).SelectedItem.Text != "" && ((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text.Trim() == "")
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
                }
                //For Arrival Date 
                for (int i = 0; i < grdAWBs.Rows.Count; i++)
                {
                    AWBno = ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text;
                    AWBno = AWBno.Substring(AWBno.Length - 8);
                    AVLPCS = Convert.ToInt32(((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text);
                    AVLWGT = Convert.ToDouble(((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text);
                }

                //Changing Manifest pcs and manifest wt
                for (int j = 0; j < gdvULDDetails.Rows.Count; j++)
                {
                    if (((TextBox)grdAWBs.Rows[0].FindControl("txtAWBno")).Text.Trim() == gdvULDDetails.Rows[j].Cells[6].Text.Trim())
                    {
                        AVLPCS = Convert.ToInt32(gdvULDDetails.Rows[j].Cells[13].Text);
                        AVLWGT = Convert.ToDouble(gdvULDDetails.Rows[j].Cells[14].Text);
                    }
                }

                DateTime dtActualDate = DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null);
                
                BLExpManifest objExpManifest = new BLExpManifest();
                bool blnResult = false;
                for (int i = 0; i < grdRouting.Rows.Count; i++)
                {
                    POL = ((TextBox)grdRouting.Rows[i].FindControl("txtFltOrig")).Text;
                    //POU = ddlMainPOU.SelectedItem.Value.ToString();
                    POU = ((TextBox)grdRouting.Rows[i].FindControl("txtFltDest")).Text; ;
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

                    DataSet objDS = new DataSet("Brkuld_Save_objDS");
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
                    if (objDS != null)
                        objDS.Dispose();

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
                    if (strDate == "")
                        dtOffLoadDate = dtCurrentDate;//DateTime.Now;
                    else
                        dtOffLoadDate = DateTime.ParseExact(((TextBox)grdRouting.Rows[i].FindControl("txtFdate")).Text, "dd/MM/yyyy", null);

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
                        Reason = ddlReason.SelectedItem.Text.Trim();
                    }

                    string PartnerCode = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartner")).SelectedValue;
                    string Partnertype = ((DropDownList)grdRouting.Rows[i].FindControl("ddlPartnerType")).SelectedValue;

                    if (PartnerCode == "Other")
                        OffLoadFltNo = ((TextBox)grdRouting.Rows[i].FindControl("txtFlightID")).Text;

                    blnResult = objExpManifest.OffLoadShipmentinManifest(ActFLTno, OffLoadFltNo, OffloadLoc, AWBno, AVLPCS, AVLWGT, OffloadPCS, OffloadWGT,
                       Updatedby, POL, POU, string.Empty, dtOffLoadDate, Reason, strMode, dtActualDate, dtCurrentDate, PartnerCode, Partnertype);

                }
                if (blnResult)
                {
                    pnlGrid.Visible = false;
                }
            }
            catch (Exception)
            {
            }
        }

        protected void btnReopenFit_Click(object sender, EventArgs e)
        {
        }

        #region btnNOTOC_Click
        protected void btnNOTOC_Click(object sender, EventArgs e)
        {
        }
        #endregion btnNOTOC_Click

        private void AllButtonStatus(bool res)
        {

            BtnAddtoManifest.Enabled = res;

            BtnClear.Enabled = res;

            //btnCloseCTM.Enabled = res;
            BtnList.Enabled = res;
            //BtnListDetails.Enabled = res;
            btnShowEAWB.Enabled = res;
            btnSplitAssign.Enabled = res;
            btnSplitUnassign.Enabled = res;
            btnUnassign.Enabled = res;

        }

        protected void btnSendFBL_Click(object sender, EventArgs e)
        {
            try
            {
                string FlightNumber = ddlFlightCode.Text.ToString() + txtFlightID.Text.ToString();
                //DataSet ds = null;
                DataSet ds = new DataSet("Brkuld_BtnFBL_ds");

                try
                {
                    BALEmailID ObjEmail = new BALEmailID();
                    ds = ObjEmail.GetEmail(lblDepAirport.Text, Session["Break_Dest"].ToString(), "FBL", FlightNumber, ddlFlightCode.Text.ToString());
                    txtEmailID.Text = ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
                    lblMsgCommType.Text = ds.Tables[0].Rows[0]["MsgCommType"].ToString();
                    if (lblMsgCommType.Text.Equals("ALL", StringComparison.OrdinalIgnoreCase) || lblMsgCommType.Text.Equals("SITA", StringComparison.OrdinalIgnoreCase))
                    {
                        GenerateSITAHeader(ds.Tables[0].Rows[0]["PartnerSITAiD"].ToString());
                    }
                }
                catch (Exception) { }
                if (ds != null)
                {
                    ds.Dispose();
                }

                ffmmsg = "";
                fblmsg = "fbl";
                Session["Break_fbl"] = fblmsg;
                string Date = Session["Break_Fltdate"].ToString().Trim();
                string Origin = Session["Break_POLairport"].ToString().Trim();
                string FltNumber = Session["Break_FltNumber"].ToString().Trim();

                txtMessageBody.Text = cls_BL.EncodeFBL(Origin, FltNumber, Date);

                lblMsg.Text = "FBL";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);

            }
            catch (Exception)
            { }
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

                DataSet dsRoutDetails = new DataSet("Brkuld_BtnAddRt_dsRout");
                dsRoutDetails = ((DataSet)Session["Break_dsRoutDetails"]).Copy();
                DataRow row = dsRoutDetails.Tables[0].NewRow();

                row["FltOrigin"] = prevdest;
                row["FltDate"] = strDate;  //dtCurrentDate.ToString("dd/MM/yyyy");

                dsRoutDetails.Tables[0].Rows.Add(row);

                Session["Break_dsRoutDetails"] = dsRoutDetails.Copy();
                grdRouting.DataSource = null;
                grdRouting.DataSource = dsRoutDetails.Copy();
                grdRouting.DataBind();

                //Validation by Vijay
                ((TextBox)grdRouting.Rows[0].FindControl("txtFltOrig")).ReadOnly = true;

                LoadDropDownAndCheckBoxRouteDetails();

                if (dsRoutDetails != null)
                {
                    dsRoutDetails.Dispose();
                }
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

                DataSet dsRouteDetailsTemp = new DataSet("Brkuld_BtnDel_TempDs");
                dsRouteDetailsTemp = ((DataSet)Session["Break_dsRoutDetails"]).Clone();

                DataSet dsRouteDetails = new DataSet("Brkuld_BtnDel_RoutDs");
                dsRouteDetails = (DataSet)Session["Break_dsRoutDetails"];

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

                Session["Break_dsRoutDetails"] = dsRouteDetailsTemp.Copy();
                grdRouting.DataSource = null;
                grdRouting.DataSource = dsRouteDetailsTemp.Copy();
                grdRouting.DataBind();

                LoadDropDownAndCheckBoxRouteDetails();

                Session["Break_Mod"] = "1";
                if (dsRouteDetails != null)
                    dsRouteDetails.Dispose();
                if (dsRouteDetailsTemp != null)
                    dsRouteDetailsTemp.Dispose();

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
            try
            {
                DataSet dsRoutDetails = new DataSet("Brkuld_SaveRoute_dsR");
                dsRoutDetails = ((DataSet)Session["Break_dsRoutDetails"]).Clone();

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

                Session["Break_dsRoutDetails"] = dsRoutDetails.Copy();

                if (dsRoutDetails != null)
                    dsRoutDetails.Dispose();

            }
            catch (Exception)
            {
            }
        }
        #endregion SaveRouteDetails

        #region LoadDropdown
        public void LoadDropDownAndCheckBoxRouteDetails()
        {
            try
            {
                DataSet dsRouteDetails = new DataSet("Brkuld_Drpdwn_dsD");
                dsRouteDetails = (DataSet)Session["Break_dsRoutDetails"];

                for (int i = 0; i < dsRouteDetails.Tables[0].Rows.Count; i++)
                {
                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Clear();
                    ((DropDownList)grdRouting.Rows[i].FindControl("ddlFltNum")).Items.Add(new ListItem(dsRouteDetails.Tables[0].Rows[i]["FltNumber"].ToString().Trim(), dsRouteDetails.Tables[0].Rows[i]["FltTime"].ToString().Trim()));
                }
                if (dsRouteDetails != null)
                {
                    dsRouteDetails.Dispose();
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

                UpdatePartnerCode(rowindex);
                GetFlightRouteData(rowindex);

                Session["Break_Mod"] = "1";

            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }

        #region GetFlightList
        public DataSet GetFlightList(string Origin, string Dest, string strdate, int hr, int min, int AllowedHr, ref string errormessage)
        {
            DataSet dsResult = new DataSet("Brkuld_GetFlt_dsResult");
            try
            {

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
            catch (Exception)
            {
                if (dsResult != null)
                {
                    dsResult.Dispose();
                }
            }
            return (null);
        }
        #endregion GetFlightList

        #region  FormatRecords
        public void FormatRecords(string org, string dest, ref DataSet dsResult, int PrevHr, int PrevMin, int AllowedHr)
        {
            int i = 0;
            string ScheduleID = "";

            DataSet dsNewResult = new DataSet("Brkuld_Format_dsNew");
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

            dsResult = new DataSet("Brk_FrmRecds_dsRes0");
            dsResult.Tables.Add(dv.ToTable().Copy());

            DataTable dt = new DataTable("Brkuld_Format_Dt2");
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
            dsResult = new DataSet("Brk_FrmRecd_ResTb");
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

            DataSet dsresult = new DataSet("brkuld_txtFdate_ds");
            //DataSet dsresult = null;
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
                dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, hr, min, AllowedHr, ref errormessage);

                if (dsresult != null && dsresult.Tables.Count != 0)
                {
                    Session["Break_Flt"] = dsresult.Copy();
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
            catch (Exception)
            {
            }
            finally
            {
                if (dsresult != null)
                    dsresult.Dispose();
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

            DataSet ds = new DataSet("Brkuld_txtFlt_Ds");
            ds = (DataSet)Session["Break_Flt"];

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

            }
            if (ds != null)
            {
                ds.Dispose();
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        }
        #endregion FltNumber

        #region loadgridroutingdetails
        public void LoadGridRoutingDetail()
        {
            DataTable myDataTable = new DataTable("Break_Routing_Dt");
            DataColumn myDataColumn;
            DataSet Ds = new DataSet("Break_Routing_Ds");

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

            DataSet dsRoutDetails = new DataSet("Break_Routing_dsRout");
            dsRoutDetails.Tables.Add(myDataTable);
            Session["Break_dsRoutDetails"] = dsRoutDetails.Copy();

            if (dsRoutDetails != null)
            {
                dsRoutDetails.Dispose();
            }
            if (myDataColumn != null)
            {
                myDataColumn.Dispose();
            }
            if (myDataTable != null)
            {
                myDataTable.Dispose();
            }
        }
        #endregion loadgridroutingdetails

        #region Fill Irregularity Code
        private void FillIrregularityCode()
        {
            LoginBL Bal = new LoginBL();
            //DataTable dsIrr = null;
            DataTable dsIrr = new DataTable("Break_Irre_dsIrr");

            try
            {
                dsIrr = Bal.LoadSystemMasterDataNew("IC").Tables[0];
                if (dsIrr != null && dsIrr.Rows.Count > 0)
                {
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                Bal = null;
                if (dsIrr != null)
                    dsIrr.Dispose();
            }
        }
        #endregion

        private void AWBDesignators()
        {
            LoginBL Bal = new LoginBL();
            ddlFlightCode.Items.Clear();

            DataTable dt = new DataTable("Break_Design_dt");
            dt = Bal.LoadSystemMasterDataNew("DC").Tables[0];

            for (int intCount = 0; intCount < dt.Rows.Count; intCount++)
            {
                ddlFlightCode.Items.Add(dt.Rows[intCount]["DesignatorCode"].ToString());
            }
            Bal = null;
            if (dt != null)
            {
                dt.Dispose();
            }
        }

        #region Load ULD Dropdown
        public void FillDDLULDNumber()
        {
            string POL = "";
            POL = Convert.ToString(Session["Station"]);

            DataSet ds = new DataSet("Break_DDLULD_ds");
            ds = objBuildULD.GetULDNumbers(POL);

            if (ds != null)
            {
                ddlULDNo.DataSource = ds;
                ddlULDNo.DataMember = ds.Tables[0].TableName;
                ddlULDNo.DataTextField = "ULDNO";
                ddlULDNo.DataValueField = "ULDNO";
                ddlULDNo.DataBind();
                ddlULDNo.Items.Insert(0, new ListItem("All", ""));
                ddlULDNo.SelectedIndex = -1;
                ds.Dispose();
            }
        }
        #endregion LoadAgentDropdown

        private void GenerateSITAHeader(string receiverSITAID)
        {
            try
            {
                BookingBAL objBLL = new BookingBAL();
                receiverSITAID = receiverSITAID.Replace(',', ' ');
                string SenderSITAID = objBLL.getSITAID(ddlFlightCode.SelectedItem.Text.ToString());
                txtSITAHeader.Text = "QP " + receiverSITAID;
                txtSITAHeader.Text = txtSITAHeader.Text + "\r\n" + "." + SenderSITAID + " " + System.DateTime.Now.ToString("dd") + System.DateTime.Now.ToUniversalTime().ToString("hhMM") + " P25";
                txtSITAHeader.Visible = true;
            }
            catch (Exception) { }
        }

        protected void btnSitaUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMessageBody.Text.Length > 0 || txtSITAHeader.Text.Length > 0)
                {
                    FTP.SaveSITAMsg(txtSITAHeader.Text.ToString() + "\r\n" + txtMessageBody.Text.ToString(), lblMsg.Text.ToString() + "File" + System.DateTime.Now.ToString("hhmmss"));
                }
            }
            catch (Exception) { }
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
            catch (Exception) { }
        }

        protected void ClearSession()
        {
            Session["Break_DataTableTemp"] = null;
            Session["Break_ManifestGridData"] = null;
            Session["Break_ULDData"] = null;
            Session["Break_IsDeparted"] = null;
            Session["Break_IsPreManifested"] = null;
            Session["Break_IsManifested"] = null;
            Session["Break_MD"] = null;
            Session["Break_AT"] = null;
            Session["Break_dsLoadBreak"] = null;
            Session["Break_ULDAWBData"] = null;
            Session["Break_AWBdata"] = null;
            Session["Break_AddToManifestAWBdata"] = null;
            Session["Break_MsgConf"] = null;
            Session["Break_FFM"] = null;
        }

        #region UpdatePartnerCode
        private void UpdatePartnerCode(int rowindex)
        {
            DataSet dsResult = new DataSet("Break1_Partner_ds");
            try
            {
                string errormessage = "";
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
                                    catch (Exception) { }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception)
            { }
            finally
            {
                if (dsResult != null)
                    dsResult.Dispose();
            }
        }
        #endregion

        #region GetFlightRouteData
        private void GetFlightRouteData(int rowindex)
        {
            //DataSet dsresult = null;
            DataSet dsresult = new DataSet("Break1_FltRData_ds");

            try
            {
                string strPartnerCode = ((DropDownList)grdRouting.Rows[rowindex].FindControl("ddlPartner")).Text.Trim();

                string errormessage = "";
                // DataSet dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text, ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, hr, min, AllowedHr, ref errormessage);
                dsresult = GetFlightList(((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltOrig")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFltDest")).Text.ToUpper(), ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text, 0, 0, 0, ref errormessage, strPartnerCode);

                if (dsresult != null && dsresult.Tables.Count != 0)
                {
                    DataSet ds = new DataSet("Break_result_ds1");
                    ds = (DataSet)Session["Break_Flt"];

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
                                        DataTable dt = new DataTable("Brk_If_dt");

                                        dt = dsresult.Tables[0].Copy();
                                        dt.TableName = name;
                                        ds.Tables.Add(dt);
                                        ds.AcceptChanges();
                                        Session["Break_Flt"] = ds.Copy();
                                    }
                                }
                                catch (Exception) { }

                            }
                            else if (ds.Tables.Count == 1)
                            {
                                Session["Break_Flt"] = dsresult.Copy();
                            }
                        }
                        catch (Exception)
                        { }
                        ds.Dispose();
                    }
                    else
                    {
                        Session["Break_Flt"] = dsresult.Copy();
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
            catch (Exception)
            { }
            finally
            {
                if (dsresult != null)
                {
                    dsresult.Dispose();
                }
            }

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
            catch (Exception)
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
                        txtbox.Dispose();
                        drop.Dispose();
                        ddl.Dispose();
                        break;
                    }
                }
                GetFlightRouteData(rowindex);
                ((TextBox)grdRouting.Rows[rowindex].FindControl("txtFdate")).Text = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy");

            }
            catch (Exception) { }
        }
        #endregion

        #region Load Airline code
        public void LoadAirlineCode(string filter)
        {
            //DataSet ds = null;
            DataSet ds = new DataSet("Break_Air_ds");

            try
            {
                //DataSet ds = objBLL.GetPartnerType(chkInterline.Checked);
                ds = objBLL.GetPartnerType(true);

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
                ddl.Dispose();
                txtdest.Dispose();
            }
            catch (Exception)
            { }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }
        #endregion

        public DataSet GetFlightList(string Origin, string Dest, string strdate, int hr, int min, int AllowedHr, ref string errormessage, string PartnerCode)
        {
            DataSet dsResult = new DataSet("Brk_ListFlt2_dsRes");
            try
            {
                if (strdate.Trim() == "")
                {
                    if (PartnerCode == Convert.ToString(Session["Break_AirlinePrefix"]))
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

                    if (PartnerCode == Convert.ToString(Session["Break_AirlinePrefix"]))
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
            catch (Exception)
            {
                if (dsResult != null)
                {
                    dsResult.Dispose();
                }
            }
            return (null);
        }

        protected void BtnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            Session["Break_QueryValues"] = null;
            gdvULDDetails.DataSource = null;
            gdvULDDetails.DataBind();
            gdvULDLoadPlan.DataSource = null;
            gdvULDLoadPlan.DataBind();
            BindAWBLoadPlanULD();
        }

        protected void BindAWBLoadPlanULD()
        {
            //DataSet dsawb = null;
            DataSet dsawb = new DataSet("Brk2_PlanULD_dsawb");

            try
            {
                Session["Break_dBreak"] = null;
                Session["Break_ULDBreak"] = null;
                Session["Break_LoadPlan"] = null;
                string flightID = txtFlightPrefix.Text.Trim() + txtFlightID.Text.Trim();
                string FlightDate = TextBoxdate.Text.Trim();
                string AWBPrefix = txtAWBPrefix.Text.Trim();
                string AWBNumber = txtAWBNo.Text.Trim();
                string FlightDestination = Session["Station"].ToString();

                #region Deepak Modifications
                if (FlightDate != "")
                {
                    if (FlightDate != "")
                    {
                        try
                        {

                            string day = FlightDate.Substring(0, 2);
                            string mon = FlightDate.Substring(3, 2);
                            string yr = FlightDate.Substring(6, 4);
                            string strtodate = yr + "-" + mon + "-" + day;
                            DateTime dt = Convert.ToDateTime(strtodate);

                        }
                        catch (Exception)
                        {
                            lblStatus.Visible = true;
                            lblStatus.Text = "Selected Flight Date format invalid!";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                }
                object[] QueryValues = new object[5]; ;
                QueryValues[0] = txtAWBPrefix.Text.Trim() + txtAWBNo.Text.Trim();
                QueryValues[1] = flightID;
                QueryValues[2] = FlightDate;
                QueryValues[3] = txtULDNo.Text.Trim();
                QueryValues[4] = FlightDestination;
                Session["Break_QueryValues"] = QueryValues;

                dsawb = ObjCustoms.GetULDList(QueryValues);
                //  dsawb = objExpMani.GetAwbTabdetails(Destination.ToString(), flightID);//further research to be done
                if (dsawb != null)
                {
                    if (dsawb.Tables.Count > 0)
                    {
                        if (dsawb.Tables[2].Rows.Count > 0)
                        {
                            Session["Break_dBreak"] = dsawb;
                            Session["Break_ULDBreak"] = dsawb.Tables[0];
                            gdvULDLoadPlan.DataSource = dsawb.Tables[2];
                            gdvULDLoadPlan.DataBind();
                            Session["Break_LoadPlan"] = dsawb.Tables[2];
                        }
                    }
                }
                else
                {
                    lblStatus.Text = "No Records Found!";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                #endregion

            }
            catch (Exception)
            { }
            finally
            {
                if (dsawb != null)
                {
                    dsawb.Dispose();
                }
            }
        }

        #region Button Save
        protected void btnSave_Click1(object sender, EventArgs e)
        {
            //DataTable dtUnassign = null;
            DataTable dtUnassign = new DataTable("Brk4_btnSave4_Unassign");

            //DataTable dtULDLoadPlan = null;
            DataTable dtULDLoadPlan = new DataTable("Brk4_btnSave4_ULDLdPlan");

            int[] SerialNumber;
            int[] UnsavedSerialNumber;
            object[] SummaryValues;
            DataRow[] dModRow;
            try
            {
                lblStatus.Text = "";
                int count = 0;
                string MessageAWB = string.Empty;
                string AWBCount = string.Empty;
                string ULDNumber = string.Empty;
                object PcsCount = 0;
                object GrossWeight = 0;
                SummaryValues = new object[11];
                SerialNumber = new int[0];
                UnsavedSerialNumber = new int[0];
                dModRow = new DataRow[0];
                dtUnassign = ((DataSet)Session["Break_dBreak"]).Tables[0].Clone();
                dtULDLoadPlan = ((DataSet)Session["Break_dBreak"]).Tables[2].Clone();
                if (gdvULDDetails.Rows.Count > 0)
                {
                    for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                    {
                        if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked)
                        {
                            count++;
                            Array.Resize(ref SerialNumber, SerialNumber.Length + 1);
                            SerialNumber[SerialNumber.Length - 1] = Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblSerialNumber")).Text);

                        }
                        else
                        {
                            Array.Resize(ref UnsavedSerialNumber, UnsavedSerialNumber.Length + 1);
                            UnsavedSerialNumber[UnsavedSerialNumber.Length - 1] = Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblSerialNumber")).Text);
                        }
                    }

                    DataTable dt = new DataTable("Brk5_BtnSave5_dtBrk");
                    dt = ((DataTable)Session["Break_ULDBreak"]).Copy();

                    for (int i = 0; i < SerialNumber.Length; i++)
                    {

                        DataRow[] FilteredRow = dt.Select("SerialNumber =" + SerialNumber[i] + "");
                        if (FilteredRow.Length > 0)
                        {
                            Array.Resize(ref dModRow, dModRow.Length + 1);
                            dModRow[dModRow.Length - 1] = FilteredRow[0];
                        }
                        FilteredRow = null;
                    }
                    if (dModRow.Length > 0)
                    {
                        foreach (DataRow row in dModRow)
                        {
                            dtUnassign.ImportRow(row);
                        }
                    }
                    if (dtUnassign.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtUnassign.Rows.Count; i++)
                        {
                            for (int dk = 0; dk < dtUnassign.Rows.Count; dk++)
                            {
                                for (int kk = 0; kk < gdvULDDetails.Rows.Count; kk++)
                                {
                                    if (dtUnassign.Rows[dk]["SerialNumber"].ToString() == ((Label)gdvULDDetails.Rows[kk].FindControl("lblSerialNumber")).Text)
                                    {
                                        dtUnassign.Rows[dk]["Location"] = ((TextBox)gdvULDDetails.Rows[kk].FindControl("txtLocation")).Text;
                                    }
                                }
                            }

                            MessageAWB += "," + dtUnassign.Rows[i]["AWBNo"].ToString();
                            AWBCount = dtUnassign.Rows[i]["AWBNo"].ToString();
                            PcsCount = Convert.ToInt32(dtUnassign.Rows[i]["PCS"].ToString());
                            GrossWeight = Convert.ToDecimal(dtUnassign.Rows[i]["GrossWgt"].ToString());
                            SummaryValues[0] = dtUnassign.Rows[i]["ULDno"].ToString();
                            SummaryValues[1] = AWBCount;
                            SummaryValues[2] = PcsCount;
                            SummaryValues[3] = GrossWeight;
                            SummaryValues[4] = 1;
                            SummaryValues[5] = Convert.ToInt32(dtUnassign.Rows[i]["SerialNumber"].ToString());
                            SummaryValues[6] = DateTime.Now.ToString();
                            SummaryValues[7] = Session["UserName"].ToString();
                            SummaryValues[8] = dtUnassign.Rows[i]["Location"].ToString();
                            SummaryValues[9] = dtUnassign.Rows[i]["FltNo"].ToString();
                            SummaryValues[10] = dtUnassign.Rows[i]["FlightDate"].ToString();
                            ULDNumber = SummaryValues[0].ToString();
                            if (ObjCustoms.UpdateULDBreakupDetails(SummaryValues))
                            {
                                Session["Break_ULDBreak"] = null;
                                Session["Break_dBreak"] = null;
                                Session["Break_LoadPlan"] = null;
                                gdvULDDetails.DataSource = null;
                                gdvULDDetails.DataBind();
                                gdvULDLoadPlan.DataSource = null;
                                gdvULDLoadPlan.DataBind();
                                //ddlSelectULD.Items.Clear();
                                //ddlSelectULD.Items.Insert(0, "Select ULD");
                                lblStatus.Text = "Records Saved Successfully !";
                                lblStatus.ForeColor = Color.Green;
                            }
                            else
                            {
                                return;
                            }
                        }


                    }
                    if (count == 0)
                    {
                        lblStatus.Text = "Please Select Records to save...";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    object[] QueryValues = new object[5];
                    if (Session["Break_QueryValues"] != null)
                    {
                        QueryValues = (object[])Session["Break_QueryValues"];
                    }
                    //DataSet dsawb = null;

                    DataSet dsawb = new DataSet("Brk6_BtnS_dsawb");
                    dsawb = ObjCustoms.GetULDList(QueryValues);

                    if (dsawb != null)
                    {
                        if (dsawb.Tables.Count > 0)
                        {
                            Session["Break_dBreak"] = dsawb;
                            Session["Break_ULDBreak"] = dsawb.Tables[0];

                        }

                        #region Added COde after Deployment

                        DataTable dtAfterSaveULDLoadPlan = new DataTable("Brk7_AfterSave3_dt3");
                        DataTable dtULDLoadPlanSummary = new DataTable("Brk7_Summary_DsSummary");

                        if (UnsavedSerialNumber.Length > 0)
                        {

                            for (int h = 0; h < UnsavedSerialNumber.Length; h++)
                            {
                                for (int d = 0; d < ((DataTable)Session["Break_ULDBreak"]).Rows.Count; d++)
                                {
                                    if (Convert.ToString(UnsavedSerialNumber[h]) == ((DataTable)Session["Break_ULDBreak"]).Rows[d]["SerialNumber"].ToString())
                                    {
                                        ((DataTable)Session["Break_ULDBreak"]).Rows[d]["IsBreak"] = 1;
                                    }
                                }
                            }
                        }
                        dtAfterSaveULDLoadPlan = ((DataTable)Session["Break_ULDBreak"]).Clone();
                        dtULDLoadPlanSummary = ((DataSet)Session["Break_dBreak"]).Tables[2].Clone();

                        #region Code Commented & Added for Showing ULD on left handside even after save

                        DataRow[] drUnassign = ((DataTable)Session["Break_ULDBreak"]).Select("IsBreak=0 and ULDno='" + ULDNumber + "'");
                        foreach (DataRow row in drUnassign)
                        {
                            dtAfterSaveULDLoadPlan.ImportRow(row);
                        }

                        //if (dtAfterSaveULDLoadPlan.Rows.Count > 0)
                        //{
                        object[] ULDSummary = new object[7];
                        DataRow[] drw = ((DataSet)Session["Break_dBreak"]).Tables[2].Select("ULDNo='" + ULDNumber + "'");
                         //Added recently
                        if (drw.Length > 0)
                        {
                            int Count = dtAfterSaveULDLoadPlan.Rows.Count;
                            PcsCount = dtAfterSaveULDLoadPlan.Compute("SUM(PCS)", "");
                            GrossWeight = dtAfterSaveULDLoadPlan.Compute("SUM(GrossWgt)", "");
                            ULDSummary[0] = ULDNumber;
                            foreach (DataRow row in drw)
                            {
                                ULDSummary[1] = row["Location"].ToString();
                                ULDSummary[6] = row["ULDType"].ToString();
                                ULDSummary[5] = row["ULDLocation"].ToString();
                            }
                            ULDSummary[2] = Count;
                            ULDSummary[3] = PcsCount;
                            ULDSummary[4] = GrossWeight;
                            dtULDLoadPlanSummary.Rows.Add(ULDSummary);
                        }
                        DataRow[] drUnEx = ((DataSet)Session["Break_dBreak"]).Tables[2].Select("ULDNo<>'" + ULDNumber + "'");
                        foreach (DataRow row in drUnEx)
                        {
                            dtULDLoadPlanSummary.ImportRow(row);
                        }
                        //    //End


                        //}

                        gdvULDLoadPlan.DataSource = dtULDLoadPlanSummary;
                        gdvULDLoadPlan.DataBind();


                        for (int i = 0; i < gdvULDLoadPlan.Rows.Count; i++)
                        {
                            if (gdvULDLoadPlan.Rows[i].Cells[1].Text == ULDNumber)
                            {
                                ((RadioButton)(gdvULDLoadPlan.Rows[i].FindControl("Check1"))).Checked = true;
                            }
                        }
                        #endregion
                        

                        
                        Session["Break_LoadPlan"] = dtULDLoadPlanSummary;

                        if (dsawb.Tables.Count > 0)
                        {
                            if (dsawb.Tables[0].Rows.Count > 0)
                            {
                                DataTable dTable = new DataTable("Brk8_Sve_dTable");
                                dTable = dsawb.Tables[0].Clone();

                                DataRow[] dRow = dsawb.Tables[0].Select("ULDNo='" + ULDNumber + "' and isBreak=1");
                                foreach (DataRow row in dRow)
                                {
                                    dTable.ImportRow(row);
                                }
                                //Session["Break_dBreak"] = dsawb;
                                //Session["Break_ULDBreak"] = dsawb.Tables[0];


                                gdvULDDetails.DataSource = dTable;
                                gdvULDDetails.DataBind();
                                dTable.Dispose();
                            }
                        }

                        #endregion

                        //Swapnil Message
                        cls_BL.addMsgToOutBox("SCM", "AWBNumber:" + MessageAWB.Substring(1, MessageAWB.Length - 1) + " is broken from ULD:" + ULDNumber, "", "");
                        if (dt != null)
                        {
                            dt.Dispose();
                        }
                        if (dsawb != null)
                        {
                            dsawb.Dispose();
                        }
                        QueryValues = null;
                    }
                }
            }
            catch (Exception)
            { }
            finally
            {
                
                if (dtUnassign != null)
                    dtUnassign.Dispose();
                if (dtULDLoadPlan != null)
                    dtULDLoadPlan.Dispose();

                SerialNumber = null;
                SummaryValues = null;
                dModRow = null;

            }
        }
        #endregion

        protected void btnUnassign_Click(object sender, EventArgs e)
        {
            object[] SummaryValues = new object[7];
            int[] SerialNumber = new int[0];
            DataRow[] dModRow = new DataRow[0];

            //DataTable dtUnassign = null;
            DataTable dtUnassign = new DataTable("Brk7_btnUn_dtUn");

            //DataTable dtULDLoadPlan = null;
            DataTable dtULDLoadPlan = new DataTable("Brk7_btnUn_dtLdp");

            try
            {

                lblStatus.Text = "";
                int ULDCheckCount = 0;
                #region Code Added to check if records are selected(23/01/2014)
                try
                {
                    if (gdvULDDetails.Rows.Count > 0)
                    {
                        for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                        {
                            if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked)
                            {
                                ULDCheckCount++;
                            }
                        }
                    }

                    if (ULDCheckCount == 0)
                    {
                        lblStatus.Text = "Please select Records to Unassign...";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                catch (Exception ex)
                { }
                #endregion

                int count = 0;
                int AWBCount = 0;
                object PcsCount = 0;
                object GrossWeight = 0;
                dtUnassign = ((DataSet)Session["Break_Break_dBreak"]).Tables[0].Clone();
                dtULDLoadPlan = ((DataSet)Session["Break_Break_dBreak"]).Tables[2].Clone();
                string ULDNo = "";
                if (gdvULDDetails.Rows.Count > 0)
                {
                    for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                    {
                        if (((CheckBox)gdvULDDetails.Rows[i].FindControl("Check0")).Checked)
                        {
                            ULDNo = ((Label)gdvULDDetails.Rows[i].FindControl("lblULDNo")).Text;
                            count++;
                            Array.Resize(ref SerialNumber, SerialNumber.Length + 1);
                            SerialNumber[SerialNumber.Length - 1] = Convert.ToInt32(((Label)gdvULDDetails.Rows[i].FindControl("lblSerialNumber")).Text);
                        }
                    }

                    DataTable dt = new DataTable("Brk7_btnUn_ULDBrk");
                    dt = ((DataTable)Session["Break_Break_ULDBreak"]).Copy();

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        for (int i = 0; i < SerialNumber.Length; i++)
                        {
                            if (Convert.ToInt32(dt.Rows[j]["SerialNumber"].ToString()) == SerialNumber[i])
                            {
                                dt.Rows[j]["IsBreak"] = 0;
                            }
                        }
                    }

                    Session["Break_Break_ULDBreak"] = dt;
                    DataRow[] drUnassign = dt.Select("IsBreak=0 and ULDno='" + ULDNo + "'");
                    foreach (DataRow row in drUnassign)
                    {
                        dtUnassign.ImportRow(row);
                    }

                    if (dtUnassign.Rows.Count > 0)
                    {
                        
                        DataRow[] drw = ((DataSet)Session["Break_Break_dBreak"]).Tables[2].Select("ULDNo='" + ULDNo + "'");
                        AWBCount = dtUnassign.Rows.Count;
                        PcsCount = dtUnassign.Compute("SUM(PCS)", "");
                        GrossWeight = dtUnassign.Compute("SUM(GrossWgt)", "");
                        SummaryValues[0] = ULDNo;
                        foreach (DataRow row in drw)
                        {
                            SummaryValues[1] = row["Location"].ToString();
                            SummaryValues[6] = row["ULDType"].ToString();
                            SummaryValues[5] = row["ULDLocation"].ToString();
                        }
                        SummaryValues[2] = AWBCount;
                        SummaryValues[3] = PcsCount;
                        SummaryValues[4] = GrossWeight;
                        dtULDLoadPlan.Rows.Add(SummaryValues);
                        DataRow[] drUnEx = ((DataSet)Session["Break_Break_dBreak"]).Tables[2].Select("ULDNo<>'" + ULDNo + "'");
                        foreach (DataRow row in drUnEx)
                        {
                            dtULDLoadPlan.ImportRow(row);
                        }
                        gdvULDLoadPlan.DataSource = dtULDLoadPlan;
                        gdvULDLoadPlan.DataBind();

                        DataRow[] drEx = dt.Select("IsBreak=1 and ULDno='" + ULDNo + "'");
                        DataTable dtExcludedAWB = new DataTable("Brk9_UnClick_dtExcl");

                        dtExcludedAWB = ((DataTable)Session["Break_Break_ULDBreak"]).Clone();
                        foreach (DataRow row in drEx)
                        {
                            dtExcludedAWB.ImportRow(row);
                        }
                       
                        gdvULDDetails.DataSource = dtExcludedAWB;
                        gdvULDDetails.DataBind();

                        #region Code Added to show ULD on Left side
                        for (int i = 0; i < gdvULDLoadPlan.Rows.Count; i++)
                        {
                            if (gdvULDLoadPlan.Rows[i].Cells[1].Text == ULDNo)
                            {
                                ((RadioButton)(gdvULDLoadPlan.Rows[i].FindControl("Check1"))).Checked = true;
                            }
                        }
                        #endregion

                        drw = null;
                        drEx = null;
                        drUnEx = null;
                        if (dtExcludedAWB != null)
                        {
                            dtExcludedAWB.Dispose();
                        }
                    }
                    if (count == 0)
                    {

                        lblStatus.Text = "Please select Records to Unassign...";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    if (dt != null)
                    {
                        dt.Dispose();
                    }

                    drUnassign = null;

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                SummaryValues = null;
                SerialNumber = null;
                dModRow = null;
                if (dtUnassign != null)
                    dtUnassign.Dispose();
                if (dtULDLoadPlan != null)
                    dtULDLoadPlan.Dispose();
            }
        }

        #region ADD TO Manifest

        protected void BtnAddtoManifest_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                SelectedULDnos = "";
                int CntULDChk = 0;
                for (int i = 0; i < gdvULDLoadPlan.Rows.Count; i++)
                {
                    if (((RadioButton)gdvULDLoadPlan.Rows[i].FindControl("Check1")).Checked == true)
                    {
                        CntULDChk += 1;
                        SelectedULDnos += "'" + (gdvULDLoadPlan.Rows[i].Cells[1].Text) + "',";
                    }
                }
                //Added on 26 sept
                if (gdvULDLoadPlan.Rows.Count > 0)
                {
                    if (CntULDChk == 0)//if none of AWB ror ULD is selectrd n  addto manifest is clicked
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "alertSelectAWBULD();", true);
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Select ULD from Tab to Break !";
                        return;
                    }
                }

                if (CntULDChk != 0)//to fetch data for checked ULDs and add data to grid 
                {
                    AddULDDatarowsToManifestGrid();
                    for (int i = 0; i < gdvULDLoadPlan.Rows.Count; i++)
                    {
                        if (((RadioButton)gdvULDLoadPlan.Rows[i].FindControl("Check1")).Checked == true)
                        {
                            string ULDNumber = GetSelectedULD();
                            //gdvULDLoadPlan.DataSource = null;
                            //gdvULDLoadPlan.DataBind();

                            DataTable DTLoadPlan = new DataTable("Brk_AddtoMnf_Ldplan");
                            DTLoadPlan = (DataTable)Session["Break_Break_LoadPlan"];

                            DataTable DtULDPlan = new DataTable("Brk_AddtoMnf_DtULD");
                            DtULDPlan = DTLoadPlan.Clone();

                            if (DTLoadPlan != null)
                            {

                                #region Code Commented & Added for Showing ULD details even after selecting it
                                //DataRow[] drow = DTLoadPlan.Select("ULDNo<>'" + ULDNumber + "'");
                                //foreach (DataRow row in drow)
                                //{
                                //    DtULDPlan.ImportRow(row);
                                //}
                                //gdvULDLoadPlan.DataSource = DtULDPlan;
                                //gdvULDLoadPlan.DataSource = DTLoadPlan;
                                //gdvULDLoadPlan.DataBind();

                                DataTable dtAfterSaveULDLoadPlan = new DataTable("Brk9_Add2Mnf_AfterSave2");
                                DataTable dtULDLoadPlanSummary = new DataTable("Brk9_Add2Mnf_Summary2");

                                object PcsCount = 0;
                                object GrossWeight = 0;
                                dtAfterSaveULDLoadPlan = ((DataTable)Session["Break_Break_ULDBreak"]).Clone();
                                dtULDLoadPlanSummary = ((DataSet)Session["Break_Break_dBreak"]).Tables[2].Clone();
                                DataRow[] drUnassign = ((DataTable)Session["Break_Break_ULDBreak"]).Select("IsBreak=0 and ULDno='" + ULDNumber + "'");
                                foreach (DataRow row in drUnassign)
                                {
                                    dtAfterSaveULDLoadPlan.ImportRow(row);
                                }

                                //if (dtAfterSaveULDLoadPlan.Rows.Count > 0)
                                //{
                                object[] ULDSummary = new object[7];
                                DataRow[] drw = ((DataSet)Session["Break_Break_dBreak"]).Tables[2].Select("ULDNo='" + ULDNumber + "'");
                                int Count = dtAfterSaveULDLoadPlan.Rows.Count;
                                PcsCount = dtAfterSaveULDLoadPlan.Compute("SUM(PCS)", "");
                                GrossWeight = dtAfterSaveULDLoadPlan.Compute("SUM(GrossWgt)", "");
                                ULDSummary[0] = ULDNumber;
                                foreach (DataRow row in drw)
                                {
                                    ULDSummary[1] = row["Location"].ToString();
                                    ULDSummary[6] = row["ULDType"].ToString();
                                    ULDSummary[5] = row["ULDLocation"].ToString();
                                }
                                ULDSummary[2] = Count;
                                ULDSummary[3] = PcsCount;
                                ULDSummary[4] = GrossWeight;
                                dtULDLoadPlanSummary.Rows.Add(ULDSummary);
                                DataRow[] drUnEx = ((DataSet)Session["Break_Break_dBreak"]).Tables[2].Select("ULDNo<>'" + ULDNumber + "'");
                                foreach (DataRow row in drUnEx)
                                {
                                    dtULDLoadPlanSummary.ImportRow(row);
                                }

                                gdvULDLoadPlan.DataSource = dtULDLoadPlanSummary;
                                gdvULDLoadPlan.DataBind();

                                for (int j = 0; j < gdvULDLoadPlan.Rows.Count; j++)
                                {
                                    if (gdvULDLoadPlan.Rows[j].Cells[1].Text == ULDNumber)
                                    {
                                        ((RadioButton)(gdvULDLoadPlan.Rows[j].FindControl("Check1"))).Checked = true;
                                    }
                                }
                                #endregion

                                Session["Break_Break_LoadPlan"] = DTLoadPlan;
                                DTLoadPlan.Dispose();
                                DtULDPlan.Dispose();
                            }
                        }
                    }
                }

                hdnManifestFlag.Value = "1";
            }
            catch (Exception)
            { }
        }

        #endregion ADD TO Manifest

        #region Add Checked ULD Data rows to Manifest Grid

        public void AddULDDatarowsToManifestGrid()
        {
            //DataTable dtULDManifest = null;
            DataTable dtULDManifest = new DataTable("Brk10_FunAddULD_dt");

            try
            {
                lblStatus.Text = "";
                dtULDManifest = ((DataTable)Session["Break_ULDBreak"]).Clone();
                int count = 0;
                int jk = 0;
                string ULDNo = GetSelectedULD();
                DataRow[] drow = ((DataTable)Session["Break_ULDBreak"]).Select("ULDNo ='" + ULDNo + "' and IsBreak=0");
                foreach (DataRow finalrow in drow)
                {
                    dtULDManifest.ImportRow(finalrow);
                    dtULDManifest.Rows[jk++]["IsBreak"] = 1;
                }

                #region Added Code by deepak(23/01/2014)
                DataRow[] drowAssigned = ((DataTable)Session["Break_ULDBreak"]).Select("ULDNo ='" + ULDNo + "' and IsBreak=1");
                foreach (DataRow finalrow in drowAssigned)
                {
                    dtULDManifest.ImportRow(finalrow);
                }
                #endregion

                if (dtULDManifest.Rows.Count > 0)
                {
                    for (int i = 0; i < dtULDManifest.Rows.Count; i++)
                    {
                        for (int j = 0; j < ((DataTable)Session["Break_ULDBreak"]).Rows.Count; j++)
                        {
                            if (dtULDManifest.Rows[i]["SerialNumber"].ToString() == ((DataTable)Session["Break_ULDBreak"]).Rows[j]["SerialNumber"].ToString() && dtULDManifest.Rows[i]["IsBreak"].ToString() == "1")
                            {
                                ((DataTable)Session["Break_ULDBreak"]).Rows[j]["IsBreak"] = 1;
                            }
                        }
                    }
                    gdvULDDetails.DataSource = dtULDManifest;
                    gdvULDDetails.DataBind();
                }
                if (count == 0)
                {
                    dtULDManifest = null;
                    dtULDManifest = ((DataTable)Session["Break_ULDBreak"]).Clone();
                    foreach (GridViewRow row in gdvULDLoadPlan.Rows)
                    {
                        DataRow[] drow1 = ((DataTable)Session["Break_ULDBreak"]).Select("ULDNo='" + ULDNo + "' and IsBreak=0");
                        int ii = 0;
                        foreach (DataRow finalrow in drow1)
                        {
                            dtULDManifest.ImportRow(finalrow);
                            dtULDManifest.Rows[ii++]["IsBreak"] = 1;
                        }
                    }
                    if (dtULDManifest.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtULDManifest.Rows.Count; i++)
                        {
                            for (int j = 0; j < ((DataTable)Session["Break_ULDBreak"]).Rows.Count; j++)
                            {
                                if (dtULDManifest.Rows[i]["SerialNumber"].ToString() == ((DataTable)Session["Break_ULDBreak"]).Rows[j]["SerialNumber"].ToString() && dtULDManifest.Rows[i]["IsBreak"].ToString() == "1")
                                {
                                    ((DataTable)Session["Break_ULDBreak"]).Rows[j]["IsBreak"] = 1;
                                }
                            }
                        }
                        gdvULDDetails.DataSource = dtULDManifest;
                        gdvULDDetails.DataBind();
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dtULDManifest != null)
                {
                    dtULDManifest.Dispose();
                }
            }
        }

        #endregion

        #region Getting Selected ULDNumber
        public string GetSelectedULD()
        {
            try
            {
                int count = 0;
                string ULDNumber = "";
                if (gdvULDLoadPlan.Rows.Count > 0)
                {
                    for (int i = 0; i < gdvULDLoadPlan.Rows.Count; i++)
                    {
                        if (((RadioButton)(gdvULDLoadPlan.Rows[i].FindControl("Check1"))).Checked)
                        {
                            count++;
                            ULDNumber = gdvULDLoadPlan.Rows[i].Cells[1].Text;
                            return ULDNumber;
                        }
                    }
                    if (count > 1)
                    { return "Please select a single ULD to break!"; }
                    if (count == 0)
                    { return "Please select a  ULD to break!"; }
                    return "Error";
                }

                return "";
            }
            catch (Exception)
            { return ""; }

        }
        #endregion

        #region Getting Left Hand Grid 

        public void SetULDLoadPlanAfterFilter()
        {
            //DataTable dtULDManifest = null;
            DataTable dtULDManifest = new DataTable("Brk1_FunFilter_dtMnf");

            object[] SummaryValues = new object[4];
            try
            {
                lblStatus.Text = "";
                int AWBCount = 0;
                object PcsCount = 0;
                object GrossWeight = 0;
                dtULDManifest = ((DataTable)Session["Break_ULDBreak"]).Clone();
                string ULDNo = GetSelectedULD();
                foreach (GridViewRow row in gdvULDLoadPlan.Rows)
                {
                    DataRow[] drow1 = ((DataTable)Session["Break_ULDBreak"]).Select("ULDNo='" + ULDNo + "' and IsBreak=0");
                    foreach (DataRow finalrow in drow1)
                    {
                        dtULDManifest.ImportRow(finalrow);
                        //dtULDManifest.Rows[ii++]["IsBreak"] = 1;

                    }
                }
                if (dtULDManifest.Rows.Count > 0)
                {
                    AWBCount = dtULDManifest.Rows.Count;
                    PcsCount = dtULDManifest.Compute("SUM(PCS)", "");
                    GrossWeight = dtULDManifest.Compute("SUM(GrossWgt)", "");
                    SummaryValues[0] = ULDNo;
                    SummaryValues[1] = AWBCount;
                    SummaryValues[2] = PcsCount;
                    SummaryValues[3] = GrossWeight;
                    dtULDManifest.Rows.Add(SummaryValues);
                    gdvULDLoadPlan.DataSource = dtULDManifest;
                    gdvULDLoadPlan.DataBind();

                }
            }
            catch (Exception)
            { }
            finally
            {
                dtULDManifest = null;
                SummaryValues = null;
            }
        }
        #endregion


    }



}
