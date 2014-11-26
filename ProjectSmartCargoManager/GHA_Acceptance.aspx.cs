using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using BAL;
using QID.DataAccess;
using System.Web.Services;
using Microsoft.Reporting.WebForms;
using System.Text;
using SCM.Common.Struct;

namespace ProjectSmartCargoManager
{
    public partial class Acceptance_GHA : System.Web.UI.Page
    {
        BalGHADockAccp objBAL = new BalGHADockAccp();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        object AWBSender;
        EventArgs AWBEvent;
        BLBuildULD objBuild = new BLBuildULD();
        string m = "";
        BALUCR objUCR = new BALUCR();
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        ReportDataSource rds3 = new ReportDataSource();
        BookingBAL objBLL = new BookingBAL();
        ConBooking_GHA objBook = new ConBooking_GHA();

        DataTable dtTable1 = new DataTable();
        DataTable dtTable2 = new DataTable();
        DataTable dtTable3 = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    #region Operation Time Popup Config Check
                    try
                    {
                        LoginBL objConfig = new LoginBL();
                        btnOpsTime.Visible = Convert.ToBoolean(objConfig.GetMasterConfiguration("enableActualOpsTime"));
                        objConfig = null;
                    }
                    catch (Exception ex)
                    { }
                    #endregion

                    
                    Session["dsDimesionAllAcceptance"] = null;
                    //Newly Added
                    Session["ULDLoc"] = null;
                    //txtTokenDt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFlightDate.Text = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy");//DateTime.Now.ToString("dd/MM/yyyy");
                    lblStatus.Text = "";
                    if (Session["AirlinePrefix"] != null)
                        txtFlightID.Text = Convert.ToString(Session["AirlinePrefix"]);
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
                    //ddlStatus.Text = "P";
                    //botlineGrid.Attributes.Add("style", "display:none");
                    AccpGridDIV.Attributes.Add("style", "display:none");
                    AccpTaskDiv.Attributes.Add("style", "display:none");

                    DataSet dsTokenList = da.SelectRecords("Sp_GETTokenListAcceptance");
                    if (dsTokenList != null)
                    {
                        if (dsTokenList.Tables[0].Rows.Count > 0)
                        {
                            ddlTokenList.DataSource = dsTokenList.Tables[0];
                            ddlTokenList.DataTextField = "TokenNumber";
                            ddlTokenList.DataBind();
                            ddlTokenList.Items.Insert(0, new ListItem("Select"));
                            dsTokenList.Dispose();
                        }
                    }
                    if (Request.QueryString["No"] != null)
                    {
                        //AWBSender = sender;
                        //AWBEvent = e;
                        GetAWBDetails();
                        string tokenno = Request.QueryString["No"].ToString();
                        ddlTokenList.SelectedIndex = ddlTokenList.Items.IndexOf(((ListItem)ddlTokenList.Items.FindByText(tokenno)));
                        //ddlTokenList.Enabled = false;
                    }
                    LoadDropDown();

                    LoadOperationTimeConfig();

                    if (Request.QueryString["FltNo"] != null)
                    {
                        string strFlightNo = Convert.ToString(Request.QueryString["FltNo"]);
                        txtFlightID.Text = strFlightNo.Substring(0, 2);
                        txtFlightNo.Text = strFlightNo.Replace(txtFlightID.Text,"");

                        txtFlightDate.Text = Convert.ToString(Request.QueryString["FltDt"]);

                        txtAWBPrefix.Text = Convert.ToString(Request.QueryString["AWBNo"]).Substring(0, 3);
                        txtAWBNumber.Text = Convert.ToString(Request.QueryString["AWBNo"]).Replace(txtAWBPrefix.Text, "").Replace("-", "");
                        btnList_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        #region Load Operation Time Config
        /// <summary>
        /// Loads configuration of whether to show popup by default on "Save" or not.
        /// </summary>
        private void LoadOperationTimeConfig()
        {
            try
            {
                //Load configuration for operation time popup.
                if (Session["ShowOperationTimeOnSave"] == null)
                {
                    Session["ShowOperationTimeOnSave"] = false;
                    LoginBL objLogin = new LoginBL();
                    string config = objLogin.GetMasterConfiguration("OperationTimePopup");
                    if (config != null)
                    {
                        bool result = false;
                        if (Boolean.TryParse(config, out result))
                            Session["ShowOperationTimeOnSave"] = result;
                        else
                            Session["ShowOperationTimeOnSave"] = false;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion Load Operation Time Config

        private void LoadDropDown()
        {
            DataSet ds = null;
            try
            {
                ds = objBAL.GetDropDownData();
                if (ds.Tables.Count > 0)
                {
                    ViewState["dt0"] = ds.Tables[0];
                    ViewState["dt1"] = ds.Tables[1];

                    foreach (GridViewRow row in grdAWBDetails.Rows)
                    {
                        ((DropDownList)row.FindControl("ddlCommCode")).DataSource = ds.Tables[0];
                        ((DropDownList)row.FindControl("ddlCommCode")).DataTextField = "CommodityDesc";
                        ((DropDownList)row.FindControl("ddlCommCode")).DataValueField = "CommodityCode";
                        ((DropDownList)row.FindControl("ddlCommCode")).DataBind();
                        //((DropDownList)row.FindControl("ddlSHC")).DataSource = ds.Tables[1];
                        //((DropDownList)row.FindControl("ddlSHC")).DataTextField = "SHC";
                        //((DropDownList)row.FindControl("ddlSHC")).DataValueField = "SHC";
                        //((DropDownList)row.FindControl("ddlSHC")).DataBind();
                    }

                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        private void GetAWBDetails()
        {
            try
            {
                Session["dsListGHA_Acceptance"] = null;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                string Id = Request.QueryString["No"].ToString();
                string dt = Request.QueryString["Dt"];
                hdnTokenNo.Value = Id;
                hdnTokenDt.Value = dt;
                DateTime TokenDt = DateTime.ParseExact(dt, "dd/MM/yyyy", null);
                object[] Params = { Id, TokenDt.ToString("dd/MM/yyyy"),"","","","","","" };
                DataSet ds = objBAL.GetAWBList(Params);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["dsListGHA_Acceptance"] = ds;
                        grdAWBDetails.DataSource = ds;
                        grdAWBDetails.DataBind();
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        AccpTaskDiv.Attributes.Add("style", "display:block");
                        if (grdAWBDetails.Rows.Count > 0)
                        {
                            ((RadioButton)grdAWBDetails.Rows[0].FindControl("radSelectAWB")).Checked = true;
                            //RadioButton1_CheckedChanged( AWBSender,AWBEvent);
                        }
                        LoadDropDown();

                        for (int i = 0; i < grdAWBDetails.Rows.Count; i++)
                        {
                            string shc, comcode;

                            shc = ds.Tables[0].Rows[i]["SHCCodes"].ToString();
                            comcode = ds.Tables[0].Rows[i]["CommodityCode"].ToString();

                            if (shc != "")
                                ((TextBox)grdAWBDetails.Rows[i].FindControl("txtSpecialHandlingCode")).Text = shc;
                                //((DropDownList)grdAWBDetails.Rows[i].FindControl("ddlSHC")).SelectedValue = shc;
                            if (comcode != "")
                                ((DropDownList)grdAWBDetails.Rows[i].FindControl("ddlCommCode")).SelectedValue = comcode;
                        }
                        //botlineGrid.Attributes.Add("style", "display:block");

                        if (ds != null)
                            ds.Dispose();
                    }
                    else
                    {
                        lblStatus.Text = "No Records Found...";
                        lblStatus.ForeColor = Color.Red;
                    }
                }

                //botlineGrid.Attributes.Add("style", "display:block");
            }
            catch (Exception ex)
            { }
        }

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                Session["dsDimesionAllAcceptance"] = null;
                //Newly Added
                Session["ULDLoc"] = null;
                foreach (GridViewRow row in grdAWBDetails.Rows)
                {
                    if (((RadioButton)row.FindControl("radSelectAWB")).Checked == true)
                    {
                        #region Commented Code
                        //hdStatus.Value = "Summary";
                        ////hdnPcsCount.Value = ((Label)row.FindControl("lblPcs")).Text;
                        ////hdnWt.Value = ((Label)row.FindControl("lblWt")).Text;
                        //grdAcceptance.DataSource = null;
                        //grdAcceptance.DataBind();
                        //string awbnumber = ((Label)row.FindControl("lblAWB")).Text;
                        //Session["awbnumber"] = awbnumber;

                        //#region Populating Pieces in Grid

                        //string[] PName = new string[1];
                        //PName[0] = "AWBNo";

                        //object[] PValue = new object[1];
                        //PValue[0] = awbnumber;

                        //SqlDbType[] PType = new SqlDbType[1];
                        //PType[0] = SqlDbType.VarChar;

                        //DataSet Ds = da.SelectRecords("sp_getAWBDimensionForCargoAccp", PName, PValue, PType);
                        //if (Ds != null)
                        //{
                        //    if (Ds.Tables.Count > 0)
                        //    {
                        //        if (Ds.Tables[0].Rows.Count > 0)
                        //        {

                        //            grdAcceptance.DataSource = Ds;
                        //            grdAcceptance.DataBind();
                        //            AccpTaskDiv.Attributes.Add("style", "display:block");
                                    
                        //            //AccpGridDIV.Attributes.Add("style", "display:block");

                        //            for (int i = 0; i < grdAcceptance.Rows.Count; i++)
                        //            {
                        //                if (Ds.Tables[0].Rows[0]["IsFlag"].ToString() == "Booking")
                        //                {
                        //                    for (int m = 0; m < Convert.ToInt32(((Label)row.FindControl("lblRcvdPcs")).Text); m++)
                        //                    {
                        //                        ((CheckBox)grdAcceptance.Rows[m].FindControl("chkAccept")).Checked = true;
                        //                    }
                                            
                                            
                        //                }
                        //                else
                        //                {

                        //                    if (((Label)grdAcceptance.Rows[i].FindControl("lblIsAccp")).Text == "True")
                        //                        ((CheckBox)grdAcceptance.Rows[i].FindControl("chkAccept")).Checked = true;
                        //                    else
                        //                        ((CheckBox)grdAcceptance.Rows[i].FindControl("chkAccept")).Checked = false;
                        //                }
                        //            }

                        //            if (Ds.Tables[0].Rows[0]["isTamper"].ToString() == "True")
                        //                chkTamper.Checked = true;

                        //            if (Ds.Tables[0].Rows[0]["isPackaging"].ToString() == "True")
                        //                chkPackaging.Checked = true;

                        //            if (Ds.Tables[0].Rows[0]["isVisual"].ToString() == "True")
                        //                chkVisual.Checked = true;

                        //            if (Ds.Tables[0].Rows[0]["isSmell"].ToString() == "True")
                        //                chkSmell.Checked = true;

                        //            if (Ds.Tables[0].Rows[0]["isDGR"].ToString() == "True")
                        //                chkDGR.Checked = true;

                        //            if (Ds.Tables[0].Rows[0]["isLiveAnimal"].ToString() == "True")
                        //                chkLiveAnimal.Checked = true;

                        //            #region Calculate Total Vol,Weight and Scale Weight

                        //            decimal FinalTotal = 0, FinalVolume = 0, FinalScaleWt = 0, Volume = 0, Weight = 0, ScaleWt = 0;
                        //            for (int i = 0; i < grdAcceptance.Rows.Count; i++)
                        //            {
                        //                Volume = 0; Weight = 0;

                        //                if (((TextBox)grdAcceptance.Rows[i].FindControl("txtVol")).Text.Trim() != "")
                        //                    Volume = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtVol")).Text.Trim());

                        //                if (((TextBox)grdAcceptance.Rows[i].FindControl("txtWt")).Text.Trim() != "")
                        //                    Weight = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtWt")).Text.Trim());

                        //                if (((TextBox)grdAcceptance.Rows[i].FindControl("txtScaleWt")).Text.Trim() != "")
                        //                    ScaleWt = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtScaleWt")).Text.Trim());

                        //                FinalVolume = FinalVolume + Volume;
                        //                FinalTotal = FinalTotal + Weight;
                        //                FinalScaleWt = FinalScaleWt + ScaleWt;

                        //            }

                        //            txtTotVolWt.Text = FinalTotal.ToString("0.00");
                        //            txtTotVolCms.Text = FinalVolume.ToString("0.00");
                        //            txtTotScaleWt.Text = FinalScaleWt.ToString("0.00");

                        //            decimal TotalVolume = 0;

                        //            if (txtTotVolCms.Text.Trim() != "")
                        //                TotalVolume = Convert.ToDecimal(txtTotVolCms.Text.Trim());

                        //            if (TotalVolume != 0)
                        //            {
                        //                txtTotVolMtr.Text = (TotalVolume / 10000).ToString("0.00");

                        //            #endregion Calculate Total Vol,Weight and Scale Weight

                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion


                        #endregion
                        chkDGR.Checked = Convert.ToBoolean(((Label)row.FindControl("lblDGR")).Text);
                        chkLiveAnimal.Checked = Convert.ToBoolean(((Label)row.FindControl("lblAnimal")).Text);
                        chkPackaging.Checked = Convert.ToBoolean(((Label)row.FindControl("lblPackage")).Text);
                        chkSmell.Checked = Convert.ToBoolean(((Label)row.FindControl("lblSmell")).Text);
                        chkTamper.Checked = Convert.ToBoolean(((Label)row.FindControl("lblTamper")).Text);
                        chkVisual.Checked = Convert.ToBoolean(((Label)row.FindControl("lblVisual")).Text);
                        hdnAWBNo.Value = ((Label)row.FindControl("lblAWB")).Text;
                        hdUCRNo.Value = ((Label)row.FindControl("lblUCRNo")).Text;
                        if (((Label)row.FindControl("lblUCRNo")).Text != "")
                        {
                            btnPrint.Enabled = true;
                        }
                        else
                            btnPrint.Enabled = false;

                        if (((Label)row.FindControl("lblULDNo")).Text != "")
                        {
                            btnPrint.Enabled = true;
                        }
                        else
                        {
                            btnPrint.Enabled = false;
                        }

                        //if (Convert.ToInt32(((Label)row.FindControl("lblIsPieces")).Text) > 0)
                        //{
                        //    ((ImageButton)row.FindControl("btnDimensionsPopup")).Visible = true;
                        //}
                        //else
                        //{
                        //    ((ImageButton)row.FindControl("btnDimensionsPopup")).Visible = false;
                        //}



                    }
                }
            }
            catch (Exception ex)
            { }
        }

        protected void CalculateVolume(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                TextBox TextBox = (TextBox)sender;
                GridViewRow grRow = (GridViewRow)TextBox.NamingContainer;
                rowindex = grRow.RowIndex;

                int Length = 0, Breadth = 0, Height = 0;
                decimal Volume = 0, Weight = 0;

                //string txtlength;
                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text.Trim() != "")
                {
                    //txtlength = ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text;
                    Length = int.Parse(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text.Trim());
                }


                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim() != "")
                    Breadth = int.Parse(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim() != "")
                    Height = int.Parse(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim());

                Volume = Length * Breadth * Height;

                //if (ddlUnit.Text.Trim().ToUpper() == "CMS")
                //{
                Weight = Volume / 6000;

                //}
                //else if (ddlUnit.Text.Trim().ToUpper() == "INCHES")
                //{
                //    Weight = Volume / 366;
                //}
                //else if (ddlUnit.Text.Trim().ToUpper() == "METERS")
                //{
                //    Weight = Volume / 0.006m;
                //}

                ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtVol")).Text = Volume.ToString("0.00");
                ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtWt")).Text = Weight.ToString("0.00");

                CalculateTotal();

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")) == ((TextBox)sender))
                    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Focus();
                else if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")) == ((TextBox)sender))
                    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Focus();
                else if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")) == ((TextBox)sender))
                    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtWt")).Focus();

            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }

        private void CalculateTotal()
        {
            decimal FinalTotal = 0, FinalVolume = 0, Volume = 0, Weight = 0;

            for (int i = 0; i < grdAcceptance.Rows.Count; i++)
            {
                Volume = 0; Weight = 0;

                if (((TextBox)grdAcceptance.Rows[i].FindControl("txtVol")).Text.Trim() != "")
                    Volume = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtVol")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[i].FindControl("txtWt")).Text.Trim() != "")
                    Weight = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtWt")).Text.Trim());

                FinalVolume = FinalVolume + Volume;
                FinalTotal = FinalTotal + Weight;
            }

            txtTotVolWt.Text = FinalTotal.ToString("0.00");
            txtTotVolCms.Text = FinalVolume.ToString("0.00");

            decimal TotalVolume = 0;

            if (txtTotVolCms.Text.Trim() != "")
                TotalVolume = Convert.ToDecimal(txtTotVolCms.Text.Trim());

            if (TotalVolume != 0)
            {
                txtTotVolMtr.Text = (TotalVolume / 10000).ToString("0.00");
                //else if (ddlUnit.Text.Trim().ToUpper() == "INCHES")
                //{
                //    txtMeterVolume.Text = (TotalVolume / 610.24m).ToString("0.00");
                //}
                //else if (ddlUnit.Text.Trim().ToUpper() == "METERS")
                //{
                //    txtMeterVolume.Text = TXTVolume.Text;
                //}
            }
            else
                txtTotVolMtr.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                int count = 0;
                //string commcode = null, shc = null;
                //int SummaryTotal = 0;
                //float SummaryWeight = 0;

                if (Session["dsDimesionAllAcceptance"] != null)
                {
                    DataSet PieceDetails = (DataSet)Session["dsDimesionAllAcceptance"];
                    for (int k = 0; k < grdAWBDetails.Rows.Count; k++)
                    {
                        if (((RadioButton)grdAWBDetails.Rows[k].FindControl("radSelectAWB")).Checked == true)
                        {
                            count++;
                            if (PieceDetails!=null && PieceDetails.Tables.Count > 0)
                            {
                                if (PieceDetails.Tables[0].Rows.Count > 0)
                                {
                                    if (((Label)grdAWBDetails.Rows[k].FindControl("lblAWB")).Text == PieceDetails.Tables[0].Rows[0]["AWBNumber"].ToString() && ((Label)grdAWBDetails.Rows[k].FindControl("lblFlightNo")).Text.Trim() == PieceDetails.Tables[0].Rows[0]["FlightNo"].ToString() && ((Label)grdAWBDetails.Rows[k].FindControl("lblFlightDate")).Text == PieceDetails.Tables[0].Rows[0]["FlightDate"].ToString())
                                    {
                                        #region Checking AWB Manifest Status
                                        if (((Label)grdAWBDetails.Rows[k].FindControl("lblManifestStatus")).Text != "")
                                        {
                                            lblStatus.Text = "AWBNo: " + ((Label)grdAWBDetails.Rows[k].FindControl("lblAWB")).Text + " already manifested!";
                                            lblStatus.ForeColor = Color.Blue;
                                            return;
                                        }
                                        #endregion

                                        #region New Code added to Accept ULD Against AWB(20/01/2014)
                                        if (Session["dsDimesionAllAcceptance"] != null)
                                        {
                                            string AWBNum = ((Label)grdAWBDetails.Rows[k].FindControl("lblAWB")).Text;
                                            string[] AWBDet = AWBNum.Split('-');
                                            string FlightNum = ((Label)grdAWBDetails.Rows[k].FindControl("lblFlightNo")).Text;
                                            string FltDt = ((Label)grdAWBDetails.Rows[k].FindControl("lblFlightDate")).Text;
                                            if (AWBDet.Length > 0)
                                            {
                                                GenerateAWBDimensions(AWBDet[1], int.Parse(((TextBox)grdAWBDetails.Rows[k].FindControl("txtAcceptedPcs")).Text), (DataSet)Session["dsDimesionAllAcceptance"],
                                                    Convert.ToDecimal(((TextBox)grdAWBDetails.Rows[k].FindControl("txtAcceptedWt")).Text), true, AWBDet[0], FlightNum, FltDt);
                                            }

                                        }

                                        #endregion
                                        //((TextBox)grdAWBDetails.Rows[k].FindControl("txtAcceptedPcs")).Text = Session["AccpPcs"].ToString();
                                        //((TextBox)grdAWBDetails.Rows[k].FindControl("txtAcceptedWt")).Text = Session["AccpWt"].ToString();
                                        //((TextBox)grdAWBDetails.Rows[k].FindControl("txtRcvPcs")).Text = Session["RemPcs"].ToString();
                                        //((TextBox)grdAWBDetails.Rows[k].FindControl("txtRcvWt")).Text = Session["RemWt"].ToString();
                                        hdStatus.Value = "Detail";
                                        for (int i = 0; i < PieceDetails.Tables[0].Rows.Count; i++)
                                        {

                                            //PieceDetails[i][13] = chkTamper.Checked;
                                            //PieceDetails[i][14] = chkPackaging.Checked;
                                            //PieceDetails[i][15] = chkVisual.Checked;
                                            //PieceDetails[i][16] = chkSmell.Checked;
                                            //PieceDetails[i][17] = chkDGR.Checked;
                                            //PieceDetails[i][18] = chkLiveAnimal.Checked;

                                            //Saving Piece Details From Popup
                                           // bool result = objBAL.SaveAccepanceData();
                                            bool result = objBAL.SaveAccepanceData(PieceDetails.Tables[0].Rows[i]["AWBNumber"].ToString(),
                                                PieceDetails.Tables[0].Rows[i]["PieceId"].ToString(), PieceDetails.Tables[0].Rows[i]["Units"].ToString(), float.Parse(PieceDetails.Tables[0].Rows[i]["Length"].ToString()), float.Parse(PieceDetails.Tables[0].Rows[i]["Breadth"].ToString()),
                                                float.Parse(PieceDetails.Tables[0].Rows[i]["Height"].ToString()), Convert.ToDecimal(PieceDetails.Tables[0].Rows[i]["Volume"]), Convert.ToDecimal(PieceDetails.Tables[0].Rows[i]["Weight"]), PieceDetails.Tables[0].Rows[i]["ScaleWeight"].ToString() != "" ? Convert.ToDecimal(PieceDetails.Tables[0].Rows[i]["ScaleWeight"]) : 0,
                                                PieceDetails.Tables[0].Rows[i]["ULDNo"].ToString(), PieceDetails.Tables[0].Rows[i]["PieceType"].ToString(), PieceDetails.Tables[0].Rows[i]["BagNo"].ToString(),
                                                PieceDetails.Tables[0].Rows[i]["Location"].ToString(), Convert.ToBoolean(PieceDetails.Tables[0].Rows[i]["isTamper"]), Convert.ToBoolean(PieceDetails.Tables[0].Rows[i]["isPackaging"]), Convert.ToBoolean(PieceDetails.Tables[0].Rows[i]["isVisual"]),
                                                Convert.ToBoolean(PieceDetails.Tables[0].Rows[i]["isSmell"]), Convert.ToBoolean(PieceDetails.Tables[0].Rows[i]["isDGR"]), Convert.ToBoolean(PieceDetails.Tables[0].Rows[i]["isLiveAnimal"]), (DateTime)Session["IT"], Session["UserName"].ToString(),
                                                1, PieceDetails.Tables[0].Rows[i]["DockNo"].ToString(), PieceDetails.Tables[0].Rows[i]["SHC"].ToString(), PieceDetails.Tables[0].Rows[i]["CommodityCode"].ToString(), Convert.ToInt64(PieceDetails.Tables[0].Rows[i]["SrNo"]), true, PieceDetails.Tables[0].Rows[i]["FlightNo"].ToString(),
                                               DateTime.ParseExact(PieceDetails.Tables[0].Rows[i]["FlightDate"].ToString(), "dd/MM/yyyy", null).ToString());
                                            if (result == true)
                                            {
                                                if (PieceDetails.Tables[0].Rows[i]["PieceType"].ToString() == "ULD" && PieceDetails.Tables[0].Rows[i]["ULDNo"].ToString() != "")
                                                {
                                                    SaveULDNoinMaster(PieceDetails.Tables[0].Rows[i]["ULDNo"].ToString());
                                                }
                                                lblStatus.Text = "Record Added Successfully...";
                                                lblStatus.ForeColor = Color.Green;
                                                //ddlTokenList.Enabled = true;                                                
                                            }
                                            else { return; }
                                        }
                                        btnSaveNew_Click(sender, e);
                                        Session["PieceDetails"] = null;
                                        Session["dsDimesionAllAcceptance"] = null;
                                        //Newly Added
                                        Session["ULDLoc"] = null;
                                    }
                                    else
                                    {
                                        Session["PieceDetails"] = null;
                                        Session["dsDimesionAllAcceptance"] = null;
                                        //Newly Added
                                        Session["ULDLoc"] = null;
                                        btnSaveNew_Click(sender, e);
                                    }
                                    
                                }
                            }
                        }
                    }
                    //btnSaveNew_Click(sender, e);
                    //Session["PieceDetails"] = null;


                }
                else
                {
                    hdStatus.Value = "Summary";
                    for (int k = 0; k < grdAWBDetails.Rows.Count; k++)
                    {
                        if (((RadioButton)grdAWBDetails.Rows[k].FindControl("radSelectAWB")).Checked == true)
                        {
                            count++;
                            #region Checking AWB Manifest Status
                            if (((Label)grdAWBDetails.Rows[k].FindControl("lblManifestStatus")).Text != "")
                            {
                                lblStatus.Text = "AWBNo: " + ((Label)grdAWBDetails.Rows[k].FindControl("lblAWB")).Text + " already manifested!";
                                lblStatus.ForeColor = Color.Blue;
                                return;
                            }
                            #endregion
                            btnSaveNew_Click(sender, e);

                        }
                    }
                }
                if (count == 0)
                {
                    lblStatus.Text = "Please select an AWB to Accept!";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }

            catch (Exception ex)
            { }
        }

        protected void CalculateScaleWeight(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                TextBox TextBox = (TextBox)sender;
                GridViewRow grRow = (GridViewRow)TextBox.NamingContainer;
                rowindex = grRow.RowIndex;

                decimal ScaleWt = 0, FinalScaleWt = 0;

                for (int i = 0; i < grdAcceptance.Rows.Count; i++)
                {
                    if (((TextBox)grdAcceptance.Rows[i].FindControl("txtScaleWt")).Text.Trim() != "")
                        ScaleWt = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtScaleWt")).Text.Trim());

                    FinalScaleWt = FinalScaleWt + ScaleWt;
                }
                txtTotScaleWt.Text = FinalScaleWt.ToString("0.00");
            }
            catch (Exception ex)
            { }
        }

        protected void CopyDimensions(object sender, EventArgs e)
        {
            try
            {
                int rowindex = 0;
                Button button = (Button)sender;
                GridViewRow grRow = (GridViewRow)button.NamingContainer;
                rowindex = grRow.RowIndex;

                int Length = 0, Breadth = 0, Height = 0, RowCount = 0;
                decimal Volume = 0, Weight = 0;

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text.Trim() != "")
                    Length = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLgth")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim() != "")
                    Breadth = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim() != "")
                    Height = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtVol")).Text.Trim() != "")
                    Volume = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtVol")).Text.Trim());

                if (((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtWt")).Text.Trim() != "")
                    Weight = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtWt")).Text.Trim());

                RowCount = Convert.ToInt32(((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtCopy")).Text.Trim());

                for (int intRow = rowindex + 1; intRow <= RowCount + rowindex; intRow++)
                {
                    if (intRow < grdAcceptance.Rows.Count)
                    {
                        ((TextBox)grdAcceptance.Rows[intRow].FindControl("txtLgth")).Text = Length.ToString();
                        ((TextBox)grdAcceptance.Rows[intRow].FindControl("txtBreadth")).Text = Breadth.ToString();
                        ((TextBox)grdAcceptance.Rows[intRow].FindControl("txtHeight")).Text = Height.ToString();
                        ((TextBox)grdAcceptance.Rows[intRow].FindControl("txtVol")).Text = Volume.ToString();
                        ((TextBox)grdAcceptance.Rows[intRow].FindControl("txtWt")).Text = Weight.ToString();
                    }
                }

                //while (RowCount < grdAcceptance.Rows.Count)
                //{
                //    rowindex = rowindex + 1;

                //    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtLength")).Text = Length.ToString();
                //    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtBreadth")).Text = Breadth.ToString();
                //    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtHeight")).Text = Height.ToString();
                //    ((Label)grdAcceptance.Rows[rowindex].FindControl("lblVolume")).Text = Volume.ToString();
                //    ((TextBox)grdAcceptance.Rows[rowindex].FindControl("txtWeight")).Text = Weight.ToString();
                //}

                CalculateTotal();
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                Session["dsListGHA_Acceptance"] = null;
                Session["dsDimesionAllAcceptance"] = null;
                //Newly Added
                Session["ULDLoc"] = null;
                AccpTaskDiv.Attributes.Add("style", "display:none");
                lblStatus.Text = "";
                btnSave.Visible = false;
                btnCancel.Visible = false;
                btnPrint.Enabled = false;//false
                grdAcceptance.DataSource = null;
                grdAcceptance.DataBind();
                grdAWBDetails.DataSource = null;
                grdAWBDetails.DataBind();
                chkDGR.Checked = false;
                chkLiveAnimal.Checked = false;
                chkPackaging.Checked = false;
                chkSmell.Checked = false;
                chkTamper.Checked = false;
                chkVisual.Checked = false;
                string tokenNo = "";
                try
                {
                    //if ((ddlFlPre.Text.Length <= -1 || txtFlightID.Text.Length <= -1 || TextBoxdate.Text.Length <= -1 || ddlListPre.Text.Length <= -1 || txtAWBSno.Text.Length <= -1 || txtSDest.Text.Length <= -1 || ddlDuartion.SelectedIndex < 0 || ddlStatus.SelectedIndex < 0) == false)
                    if (txtFlightID.Text.Trim().Length <= 0 && txtFlightNo.Text.Trim().Length <= 0 && txtFlightDate.Text.Trim().Length <= 0 && txtAWBPrefix.Text.Length <= 0 && txtAWBNumber.Text.Trim().Length <= 0 && txtTokenDt.Text.Trim().Length <= 0 && txtDockNo.Text.Trim().Length <= 0 && ddlTokenList.SelectedIndex <= 0 && ddlStatus.SelectedIndex <= 0)
                    {
                        lblStatus.Text = "Please Enter/select atleast one filter.";
                        return;
                    }

                }
                catch (Exception ex)
                { }
                if (ddlTokenList.SelectedIndex <= 0)
                {
                    //    lblStatus.Text = "Select Token Number...";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return;
                    tokenNo = "";
                }
                else
                {
                    tokenNo = ddlTokenList.SelectedItem.Text;
                }

                string dt = txtTokenDt.Text;
                //DateTime date=DateTime.Now;
                if (txtTokenDt.Text != "")
                {
                    try
                    {
                        DateTime date = DateTime.ParseExact(dt, "dd/MM/yyyy", null);
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Selected Token Date format invalid";
                        lblStatus.ForeColor = Color.Red;
                    }
                }
                string fltDate = txtFlightDate.Text.Trim();
                string flightDateValidated = "";
                if (fltDate != "")
                {
                    DateTime dtime;

                    try
                    {
                        //dt = Convert.ToDateTime(txtInvoiceFrom.Text);
                        //Change 03082012
                        string day = fltDate.Substring(0, 2);
                        string mon = fltDate.Substring(3, 2);
                        string yr = fltDate.Substring(6, 4);
                        flightDateValidated = yr + "/" + mon + "/" + day;
                        dtime = Convert.ToDateTime(flightDateValidated);
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Selected Flight Date format invalid";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                object[] Params = { tokenNo,dt,txtAWBPrefix.Text.Trim()+txtAWBNumber.Text.Trim(),txtFlightID.Text.Trim()+txtFlightNo.Text.Trim(),fltDate,Session["Station"].ToString(),txtDockNo.Text,ddlStatus.Text.Trim()};

                DataSet ds = objBAL.GetAWBList(Params);
                Params = null;
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //AccpTaskDiv.Attributes.Add("style", "display:block");
                        Session["dsListGHA_Acceptance"] = ds;
                        grdAWBDetails.DataSource = ds;
                        grdAWBDetails.DataBind();
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        LoadDropDown();
                        AccpTaskDiv.Attributes.Add("style", "display:block");

                        for (int i = 0; i < grdAWBDetails.Rows.Count; i++)
                        {
                            #region Commented Code (19/01/2014)
                            //if (Convert.ToInt32(((Label)grdAWBDetails.Rows[i].FindControl("lblIsPieces")).Text) > 0)
                            //{
                            //    ((ImageButton)grdAWBDetails.Rows[i].FindControl("btnDimensionsPopup")).Visible = true;
                            //}
                            //else
                            //{
                            //    ((ImageButton)grdAWBDetails.Rows[i].FindControl("btnDimensionsPopup")).Visible = false;
                            //}
                            #endregion

                            string shc, comcode;
                            if (grdAWBDetails.Rows.Count > 0)
                            {
                                ((RadioButton)grdAWBDetails.Rows[0].FindControl("radSelectAWB")).Checked = true;
                                chkTamper.Checked = (bool)ds.Tables[0].Rows[0]["isTamper"];
                                chkPackaging.Checked = (bool)ds.Tables[0].Rows[0]["isPackaging"];
                                chkVisual.Checked = (bool)ds.Tables[0].Rows[0]["isVisual"];
                                chkSmell.Checked = (bool)ds.Tables[0].Rows[0]["isSmell"];
                                chkDGR.Checked = (bool)ds.Tables[0].Rows[0]["isDGR"];
                                chkLiveAnimal.Checked = (bool)ds.Tables[0].Rows[0]["isLiveAnimal"];
                                hdnAWBNo.Value = ds.Tables[0].Rows[0]["AWBNumber"].ToString();
                                hdUCRNo.Value = ds.Tables[0].Rows[0]["UCRNo"].ToString();
                                if (ds.Tables[0].Rows[0]["UCRNo"].ToString() != "")
                                {
                                    btnPrint.Enabled = true;
                                }
                                else
                                    btnPrint.Enabled = false;

                                if (ds.Tables[0].Rows[0]["ULDNo"].ToString() != "")
                                { 
                                    btnPrint.Enabled = true; 
                                }
                                else
                                {
                                    btnPrint.Enabled = false;
                                }
                                //if (Convert.ToInt32(ds.Tables[0].Rows[0]["IsPieces"].ToString()) > 0)
                                //{
                                //    ((ImageButton)grdAWBDetails.Rows[0].FindControl("btnDimensionsPopup")).Visible = true;
                                //}
                                //else
                                //{
                                //    ((ImageButton)grdAWBDetails.Rows[0].FindControl("btnDimensionsPopup")).Visible = false;
                                //}
                                
                            }
                            shc = ds.Tables[0].Rows[i]["SHCCodes"].ToString();
                            comcode = ds.Tables[0].Rows[i]["CommodityCode"].ToString();

                            if (shc != "")
                                ((TextBox)grdAWBDetails.Rows[i].FindControl("txtSpecialHandlingCode")).Text = shc;
                            if (comcode != "")
                                ((DropDownList)grdAWBDetails.Rows[i].FindControl("ddlCommCode")).SelectedValue = comcode;

                        }
                        //RadioButton1_CheckedChanged(sender, e);
                        //botlineGrid.Attributes.Add("style", "display:block");
                        if (ds != null)
                            ds.Dispose();
                    }
                    else
                    {
                        lblStatus.Text = "No Records Found...";
                        lblStatus.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        protected void btnShowFlights_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = -1;
                for (int i = 0; i < grdAWBDetails.Rows.Count; i++)
                {
                    if (((ImageButton)grdAWBDetails.Rows[i].FindControl("btnDimensionsPopup")) == ((ImageButton)sender))
                    {
                        rowIndex = i;
                        break;
                    }
                }

                //if (((TextBox)grdAWBDetails.Rows[rowIndex].FindControl("txtFltOrig")).Text == "" ||
                //    ((TextBox)grdAWBDetails.Rows[rowIndex].FindControl("txtFltDest")).Text == "")
                //{
                //    return;
                //}

                string awbno = ((Label)grdAWBDetails.Rows[rowIndex].FindControl("lblAWB")).Text;
                Session["SHC"] = ((DropDownList)grdAWBDetails.Rows[rowIndex].FindControl("ddlSHC")).SelectedItem.Text;
                Session["commCode"] = ((DropDownList)grdAWBDetails.Rows[rowIndex].FindControl("ddlCommCode")).SelectedItem.Text;
                hdnAWBNo.Value = awbno;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>showDimension();</script>", false);

            }
            catch (Exception)
            {
            }
        }

        protected void btnSaveNew_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                string AWBNumber = string.Empty;
                string FlightNo = string.Empty;
                string FlightDate = string.Empty;
                btnPrint.Enabled = false;//false
                string PartnerCode = string.Empty;
                int count = 0;
                for (int i = 0; i < grdAWBDetails.Rows.Count; i++)
                {
                    if (((RadioButton)grdAWBDetails.Rows[i].FindControl("radSelectAWB")).Checked == true)
                    {
                        count++;
                        int TotalPcs = int.Parse(((Label)grdAWBDetails.Rows[i].FindControl("lblPcs")).Text);
                        float TotalWeight = float.Parse(((Label)grdAWBDetails.Rows[i].FindControl("lblWt")).Text);
                        //int totPcs = int.Parse(((Label)grdAWBDetails.Rows[i].FindControl("lblRemainingPcs")).Text);
                        //float totWt = float.Parse(((Label)grdAWBDetails.Rows[i].FindControl("lblRemainingWt")).Text);
                        //int AcceptedPcs = int.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("lblRcvPcs")).Text);
                        //float AcceptedWt = float.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("lblRcvWt")).Text);
                        //float accpWt = float.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).Text);
                        //float RemainingWt = float.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).Text);
                        //int RemainingPcs = int.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).Text);
                        //int RecievedPcs = int.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("lblRcvPcs")).Text);
                        //float RecievedWt = float.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("lblRcvWt")).Text);
                        int AcceptedPcs = int.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("txtAcceptedPcs")).Text);
                        float AcceptedWt = float.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("txtAcceptedWt")).Text);
                        float RemainingWt = float.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).Text);
                        int RemainingPcs = int.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).Text);
                        ///Configuration based Acceptance
                        LoginBL objLBAL = new LoginBL();
                        bool validatePcsWt = true;
                        try
                        {
                            validatePcsWt = Convert.ToBoolean(objLBAL.GetMasterConfiguration("AcceptAdditionalWt"));
                        }
                        catch (Exception ex)
                        {
                            validatePcsWt = true;
                        }
                        if (!validatePcsWt)
                        {
                            if (TotalPcs < AcceptedPcs)
                            {
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Confirm();</SCRIPT>", false);
                                lblStatus.Text = "Accepted Pcs Cannot Be Greater Than Total Pcs.";
                                lblStatus.ForeColor = Color.Red;
                                
                                return;
                            }

                            if (TotalWeight < AcceptedWt)
                            {
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Confirm();</SCRIPT>", false);
                                lblStatus.Text = "Accepted weight Cannot Be Greater Than Total weight.";
                                lblStatus.ForeColor = Color.Red;
                               
                                return;
                            }
                        }
                        ////


                        object[] Params = new object[17];

                        int j = 0;
                        PartnerCode = ((Label)grdAWBDetails.Rows[i].FindControl("lblPartner")).Text;
                        FlightNo = ((Label)grdAWBDetails.Rows[i].FindControl("lblFlightNo")).Text;
                        FlightDate = ((Label)grdAWBDetails.Rows[i].FindControl("lblFlightDate")).Text;
                      
                        //0
                        string awb = ((Label)grdAWBDetails.Rows[i].FindControl("lblAWB")).Text;
                        AWBNumber = awb;
                        hdnAWBNo.Value = AWBNumber;
                        Params.SetValue(awb, j);
                        j++;

                        //1
                        //int accpPcs = int.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).Text);
                      
                        //if (Session["PieceDetails"]==null)
                        //{
                            if (AcceptedPcs > TotalPcs)
                            {
                               // hdConfirm.Value = "true";
                                //hdConfirm.Value = "false";
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Confirm();</SCRIPT>", false);
                                //lblStatus.Text = "Accepted Pcs Cannot Be Greater Than Total Pcs...";
                                //lblStatus.ForeColor = Color.Red;
                                ////((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).BorderColor = Color.Red;
                                //return;

                                //if (hdConfirm.Value == "true")
                                //{
                                    Params.SetValue(AcceptedPcs, j);
                                    ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).Text = (TotalPcs - AcceptedPcs).ToString();//(float.Parse(((Label)grdAWBDetails.Rows[i].FindControl("lblPcs")).Text) - accpPcs).ToString();
                                    ((TextBox)grdAWBDetails.Rows[i].FindControl("txtAcceptedPcs")).Text = "0";
                                    j++;
                                    hdnPcsCount.Value = (AcceptedPcs).ToString();
                                //}
                                //else
                                //{ return; }
                            }
                            else if (AcceptedPcs <= 0)
                            {
                                lblStatus.Text = "Accepted Pcs Cannot Be 0...";
                                lblStatus.ForeColor = Color.Red;
                                //((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).BorderColor = Color.Red;
                                return;
                            }
                            else
                            {
                                //((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).BorderColor = Color.Empty;
                                //((TextBox)grdAWBDetails.Rows[i].FindControl("lblRcvPcs")).Text = ( AcceptedPcs).ToString();//(Convert.ToInt32(((Label)grdAWBDetails.Rows[i].FindControl("lblRcvPcs")).Text) + accpPcs).ToString();
                                Params.SetValue(AcceptedPcs, j);
                                ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).Text = (TotalPcs - AcceptedPcs).ToString();//(float.Parse(((Label)grdAWBDetails.Rows[i].FindControl("lblPcs")).Text) - accpPcs).ToString();
                                //((TextBox)grdAWBDetails.Rows[i].FindControl("txtAcceptedPcs")).Text = "0";
                                j++;
                                hdnPcsCount.Value = (AcceptedPcs).ToString();
                            }

                            if (AcceptedWt > TotalWeight)
                            {
                                ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).Text = (TotalWeight - AcceptedWt).ToString();
                                //((TextBox)grdAWBDetails.Rows[i].FindControl("txtAcceptedWt")).Text = "0";

                                Params.SetValue(AcceptedWt, j);
                                j++;
                                hdnWt.Value = (AcceptedWt).ToString();
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>return confirm('Accepted Wt greater than Gross Wt. Do you want to continue?');</SCRIPT>", false);
                                //lblStatus.Text = "Accepted Wt Cannot Be Greater Than Gross Wt...";
                                ////lblStatus.ForeColor = Color.Red;
                                //((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).BorderColor = Color.Red;
                                //return;
                            }
                            else if (AcceptedWt <= 0)
                            {
                                lblStatus.Text = "Accepted Wt Cannot Be 0...";
                                lblStatus.ForeColor = Color.Red;
                                //((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).BorderColor = Color.Red;
                                return;
                            }
                            else
                            {
                                //((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).BorderColor = Color.Empty;
                                //((TextBox)grdAWBDetails.Rows[i].FindControl("lblRcvWt")).Text = (AcceptedWt).ToString();
                                ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).Text = (TotalWeight - AcceptedWt).ToString();
                                //((TextBox)grdAWBDetails.Rows[i].FindControl("txtAcceptedWt")).Text = "0";

                                Params.SetValue(AcceptedWt, j);
                                j++;
                                hdnWt.Value = (AcceptedWt).ToString();
                            }
                        
                            #region AcceptPartial and Tolerance for partner
                            DataSet PData = objBAL.GetPartnerDetails(PartnerCode);
                            if (PData != null)
                            {
                                string AcceptPartial = PData.Tables[0].Rows[0]["AcceptPartial"].ToString();
                                decimal AcceptedTreshold = Convert.ToDecimal(PData.Tables[0].Rows[0]["AcceptedTolerance"].ToString());
                                if (AcceptPartial == "0")
                                {
                                    if (AcceptedPcs != TotalPcs)
                                    {
                                        lblStatus.Text = "Partial Acceptance not allowed for this Partner:" + PartnerCode;
                                        lblStatus.ForeColor = Color.Red;
                                        return;
                                    }
                                    decimal TresholdValue = 0;
                                    if (AcceptedTreshold != 0)
                                    {
                                        TresholdValue = (Convert.ToDecimal(TotalWeight) * AcceptedTreshold) / 100;
                                        if (Convert.ToDecimal(AcceptedWt) < (Convert.ToDecimal(TotalWeight) - TresholdValue) || Convert.ToDecimal(AcceptedWt) > (Convert.ToDecimal(TotalWeight) + TresholdValue))
                                        {

                                            lblStatus.Text = "Accepted Weight exceeds Treshold Value for Partner:" + PartnerCode;
                                            lblStatus.ForeColor = Color.Red;
                                            return;
                                        }
                                    }
                                }

                                //else
                                //{
                                //if (AcceptedPcs != TotalPcs)
                                //{
                                //    lblStatus.Text = "Partial Acceptance not allowed for this Partner:" + PartnerCode;
                                //    lblStatus.ForeColor = Color.Red;
                                //    return;
                                //}
                                //}
                            }
                            #endregion
                            //}
                            //Commented for Piece level incompatibility
                        //else
                        //{
                        //    int RemPcs=Convert.ToInt32(((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).Text);
                        //     float RemWt = float.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).Text);
                        //    int SummPcs=Convert.ToInt32(((TextBox)grdAWBDetails.Rows[i].FindControl("lblRcvPcs")).Text);

                        //    float SumWt = float.Parse(((TextBox)grdAWBDetails.Rows[i].FindControl("lblRcvWt")).Text);
                        //    if (SummPcs > TotalPcs)
                        //    {
                        //        lblStatus.Text = "Accepted Pcs Cannot Be Greater Than Total Pcs...";
                        //        lblStatus.ForeColor = Color.Red;
                        //        //((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).BorderColor = Color.Red;
                        //        return;
                        //    }
                        //    else if ( SummPcs <= 0)
                        //    {
                        //        lblStatus.Text = "Accepted Pcs Cannot Be 0...";
                        //        lblStatus.ForeColor = Color.Red;
                        //        //((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).BorderColor = Color.Red;
                        //        return;
                        //    }
                        //    else
                        //    {
                        //        ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).BorderColor = Color.Empty;
                        //        //((TextBox)grdAWBDetails.Rows[i].FindControl("lblRcvPcs")).Text = (SummPcs).ToString();//(Convert.ToInt32(((Label)grdAWBDetails.Rows[i].FindControl("lblRcvPcs")).Text) + accpPcs).ToString();
                        //        Params.SetValue(SummPcs, j);
                        //       // ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvPcs")).Text = (TotalPcs-SummPcs).ToString();//(float.Parse(((Label)grdAWBDetails.Rows[i].FindControl("lblPcs")).Text) - accpPcs).ToString();
                        //        j++;
                        //        hdnPcsCount.Value = (SummPcs).ToString();
                        //    }

                        //    if ( SumWt > TotalWeight)
                        //    {
                        //        lblStatus.Text = "Accepted Wt Cannot Be Greater Than Gross Wt...";
                        //        lblStatus.ForeColor = Color.Red;
                        //        //((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).BorderColor = Color.Red;
                        //        return;
                        //    }
                        //    else if (SumWt <= 0)
                        //    {
                        //        lblStatus.Text = "Accepted Wt Cannot Be 0...";
                        //        lblStatus.ForeColor = Color.Red;
                        //        //((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).BorderColor = Color.Red;
                        //        return;
                        //    }
                        //    else
                        //    {
                        //        ((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).BorderColor = Color.Empty;
                        //        //((TextBox)grdAWBDetails.Rows[i].FindControl("lblRcvWt")).Text = (SumWt).ToString();
                        //        //((TextBox)grdAWBDetails.Rows[i].FindControl("txtRcvWt")).Text = (TotalWeight - SumWt).ToString();

                        //        Params.SetValue(SumWt, j);
                        //        j++;
                        //        hdnWt.Value = (SumWt).ToString();
                        //    }
                        //}


                        


                        //2
                        

                        //3

                            DateTime date = Session["IT"] != null ? (DateTime)Session["IT"] : DateTime.Now;
                        Params.SetValue(date, j);
                        j++;

                        //4
                        Params.SetValue(Session["UserName"].ToString(), j);
                        j++;
                        Params.SetValue(((TextBox)grdAWBDetails.Rows[i].FindControl("txtSpecialHandlingCode")).Text, j);
                        j++;
                        Params.SetValue(((DropDownList)grdAWBDetails.Rows[i].FindControl("ddlCommCode")).Text, j);
                        j++;
                        if (hdStatus.Value == "Detail")
                        {
                            Params.SetValue("false", j);
                            j++;
                        }
                        else
                        {

                            Params.SetValue("true", j);
                            j++;
                        }
                        Params.SetValue(FlightNo, j);
                        j++;

                        Params.SetValue(FlightDate, j);
                        j++;

                        if (((TextBox)grdAWBDetails.Rows[i].FindControl("txtLocation")).Text != "")
                        {
                            Params.SetValue(((TextBox)grdAWBDetails.Rows[i].FindControl("txtLocation")).Text, j);
                            j++;
                        }
                        else
                        {
                            Params.SetValue("Non Bonded Area", j);
                            j++;
                        }

                        Params.SetValue(chkTamper.Checked, j);
                        j++;

                        Params.SetValue(chkPackaging.Checked, j);
                        j++;

                        Params.SetValue(chkVisual.Checked, j);
                        j++;

                        Params.SetValue(chkSmell.Checked, j);
                        j++;

                        Params.SetValue(chkDGR.Checked, j);
                        j++;

                        Params.SetValue(chkLiveAnimal.Checked, j);
                        j++;


                        bool result = objBAL.SaveGHAAcceptanceData_V2(Params);
                        Params = null;
                        if (result == true)
                        {
                            string ULDNumber = string.Empty;
                            string Location = string.Empty;
                            
                            if (Session["ULDLoc"] != null)
                            {
                                try
                                {
                                    if (((string[])Session["ULDLoc"])[0].ToString() != "")
                                    {
                                        ((Label)grdAWBDetails.Rows[i].FindControl("lblULDNo")).Text = ((string[])Session["ULDLoc"])[0].Replace("$", ",");
                                    }
                                }
                                catch (Exception ex)
                                { }
                                //string[] ULDDet = (string[])Session["ULDLoc"];
                                //ULDNumber = ULDDet[0];
                                //hdULDNo.Value = ULDNumber;
                                //Location = ULDDet[1];
                                //hdLocation.Value = Location;
                                //hdnAWBNo.Value = AWBNumber;
                                //hdULDorigin.Value=ULDDet[2];
                                //hdULDDestination.Value = ULDDet[3];
                                //hdFlightNumber.Value = FlightNo;
                                //hdFlightDate.Value = FlightDate;
                                
                            }

                            ULDNumber = ((Label)grdAWBDetails.Rows[i].FindControl("lblULDNo")).Text;
                            hdULDNo.Value = ULDNumber;
                            hdFlightNumber.Value = FlightNo;
                            hdFlightDate.Value = FlightDate;
                            hdULDDestination.Value = ((Label)grdAWBDetails.Rows[i].FindControl("lblDestination")).Text;
                           
                            //Integration of ULD Acceptance module(19/01/2014)

                            //char[] charSeparator = new char[] { ',' };
                            //foreach (string ULD in ULDNumber.Split(charSeparator, StringSplitOptions.RemoveEmptyEntries))
                            //{
                            //    if (SaveULDNoinMaster(ULD))
                            //    {
                            //        //btnPrint.Enabled = true;
                            //        //divPrintUCR.Attributes.Add("style", "display:block");

                            //    }
                            //    string Result = string.Empty;
                            //    string FltDate = DateTime.ParseExact(FlightDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
                            //    //objBuild.AddReceivedULDDetails(ULD, awb.Trim().Substring(0, 3).ToString(), awb, FlightNo, FltDate, "true", Session["IT"].ToString(), Session["UserName"].ToString(), ref Result);
                            //}
                            
                            //Relisting after Save
                            ddlStatus.Text = "";
                            //btnList_Click(sender, e);
                           
                            for (int d = 0; d < grdAWBDetails.Rows.Count; d++)
                            {
                                if (((Label)grdAWBDetails.Rows[d].FindControl("lblAWB")).Text == awb && ((Label)grdAWBDetails.Rows[d].FindControl("lblFlightNo")).Text == hdFlightNumber.Value && ((Label)grdAWBDetails.Rows[d].FindControl("lblFlightDate")).Text == hdFlightDate.Value)
                                {
                                    ((RadioButton)grdAWBDetails.Rows[d].FindControl("radSelectAWB")).Checked = true;
                                    chkDGR.Checked = Convert.ToBoolean(((Label)grdAWBDetails.Rows[d].FindControl("lblDGR")).Text);
                                    chkLiveAnimal.Checked = Convert.ToBoolean(((Label)grdAWBDetails.Rows[d].FindControl("lblAnimal")).Text);
                                    chkPackaging.Checked = Convert.ToBoolean(((Label)grdAWBDetails.Rows[d].FindControl("lblPackage")).Text);
                                    chkSmell.Checked = Convert.ToBoolean(((Label)grdAWBDetails.Rows[d].FindControl("lblSmell")).Text);
                                    chkTamper.Checked = Convert.ToBoolean(((Label)grdAWBDetails.Rows[d].FindControl("lblTamper")).Text);
                                    chkVisual.Checked = Convert.ToBoolean(((Label)grdAWBDetails.Rows[d].FindControl("lblVisual")).Text);
                                }
                                else
                                {
                                    ((RadioButton)grdAWBDetails.Rows[d].FindControl("radSelectAWB")).Checked = false;

                                }
                            }
                    
                            if (ULDNumber != "")
                            {
                                ddlStatus.Text = "";
                                txtAWBPrefix.Text = awb.Trim().Substring(0, 3);
                                txtAWBNumber.Text = awb.Trim().Substring(4, 8);
                                try
                                {
                                    if (FlightNo.Trim() != "")
                                    {
                                        txtFlightID.Text = FlightNo.Trim().Substring(0, 2);
                                        txtFlightNo.Text = FlightNo.Trim().Substring(2, FlightNo.Length - 2);
                                    }
                                }
                                catch (Exception ex)
                                { }
                                txtFlightDate.Text = FlightDate;
                                txtDockNo.Text = "";
                                txtTokenDt.Text = "";
                                ddlTokenList.SelectedIndex = 0;
                                //Show popup to save actual operation time.
                                if (Session["ShowOperationTimeOnSave"] != null && (bool)Session["ShowOperationTimeOnSave"] == true)
                                    SaveOperationTime(true);
                                else
                                {
                                    //Save Operational date and By Default
                                    SaveOperationTime(false);
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
                                btnList_Click(sender, e);
                                btnPrint.Enabled = true;
                            }
                            else
                            {
                                //Show popup to save actual operation time.
                                ddlStatus.Text = "P";//Added to show only AWB's which are partially accepted or not accepted
                                if (Session["ShowOperationTimeOnSave"] != null && (bool)Session["ShowOperationTimeOnSave"] == true)
                                    SaveOperationTime(true);
                                else
                                {
                                    //Save Operational date and By Default
                                    SaveOperationTime(false);
                                    BALCommon objBALCommon = new BALCommon();
                                    System.Collections.Generic.List<SCM.Common.Struct.clsOperationTimeStamp> LstOperation = null;
                                    LstOperation = (System.Collections.Generic.List<SCM.Common.Struct.clsOperationTimeStamp>)Session["listOperationTime"];

                                    if (LstOperation != null && LstOperation.Count>0)
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

                                btnList_Click(sender, e);
                            }

                            lblStatus.Text = "Shipment accepted for " + awb + " on " + ((DateTime)Session["IT"]).ToString("dd/MM/yyyy hh:mm:ss tt");// "Records Added Successfully...";
                            lblStatus.ForeColor = Color.Green;

                            string msg = "Cargo Accepted for AWB" + awb;
                            msg += "\nPCS:" + hdnPcsCount.Value.ToString() + "\nWT:" + hdnWt.Value.ToString() + "\nUnder Token No:";
                            msg += hdnTokenNo.Value.ToString() + "\nDated:" + hdnTokenDt.Value.ToString();
                            cls_BL.addMsgToOutBox("SCM", msg, "", "");

                            try
                            {
                                //Vijay - Code to check BillingEvent flag (EX- execution, AC- Acceptance, DP- Departure)
                                string BillingEvent = "";
                                BillingEvent = objBLL.getConfiguredBillingEvent(awb);

                                //If BillingEvent is EX, record should be inserted in Billing tables
                                if (BillingEvent.ToUpper() == "AC")
                                {
                                    objBook.AddBillingDetails(awb, BillingEvent.ToUpper());
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            #region ACAS PRI Message Automation Deepak(15/04/2014)
                            try
                            {
                                  LoginBL objLogin = new LoginBL();
                                  if (Convert.ToBoolean(objLogin.GetMasterConfiguration("ACASAutomation")))
                                  {
                                      ACASBAL objACAS = new ACASBAL();

                                      if (objACAS.ACASFRITriggerPointValidation() == "AC")
                                      {
                                          object[] QueryValues = { AWBNumber, FlightNo, FlightDate };
                                          DataSet dsACAS = objACAS.CheckACASAWBAvailability(QueryValues);
                                          if (dsACAS != null)
                                          {
                                              if (dsACAS.Tables[1].Rows[0]["Validate"].ToString() == "True")
                                              {
                                                  StringBuilder sbPRI = objACAS.EncodingPRIMessage(QueryValues);

                                                  object[] QueryVal = { AWBNumber, 1, FlightNo, FlightDate, sbPRI.ToString().ToUpper() };

                                                  if (objACAS.UpdatePRIMessage(QueryVal))
                                                  {
                                                      if (sbPRI != null)
                                                      {
                                                          if (sbPRI.ToString() != "")
                                                          {
                                                              object[] QueryValMail = { "PRI", FlightNo, FlightDate };
                                                              //Getting MailID for PRI Message
                                                              DataSet dMail = objACAS.GetCustomMessagesMailID(QueryValMail);
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
                                  }
                            }
                            catch (Exception Ex)
                            {

                                #region Sending PER Error Message on System Error
                                try
                                {
                                    ACASBAL objACAS = new ACASBAL();
                                    object[] QueryValues = { AWBNumber, FlightNo, FlightDate };
                                    StringBuilder sbPER = objACAS.EncodingPERMessage(QueryValues);
                                    object[] QueryValPER = { QueryValues[0], QueryValues[1], QueryValues[2], sbPER.ToString().ToUpper() };
                                    objACAS.UpdatePERMessage(QueryValPER);

                                    if (sbPER != null)
                                    {
                                        if (sbPER.ToString() != "")
                                        {
                                            object[] QueryValMail = { "PER", FlightNo, FlightDate };
                                            //Getting MailID for PER Message
                                            DataSet dMail = objACAS.GetCustomMessagesMailID(QueryValMail);
                                            string MailID = string.Empty;
                                            if (dMail != null)
                                            {
                                                MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                            }
                                            cls_BL.addMsgToOutBox("PER", sbPER.ToString().ToUpper(), "", MailID);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                { }
                                #endregion
                            }

                            #endregion

                        }
                        else
                        {
                            lblStatus.Text = "Error While Adding Record...";
                            lblStatus.ForeColor = Color.Red;
                        }
                    }
                }
                if (count == 0)
                {
                    lblStatus.Text = "Select atleast one row...";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex)
            { }

        }

        /// <summary>
        /// Sets data for showing Operation Time Popup.
        /// </summary>
        private void SaveOperationTime(bool ShowOperationsPopuup)
        {
            try
            {

                //Set dataset for AWBs in AWB grid.                    
                List<SCM.Common.Struct.clsOperationTimeStamp> objListOpsTime = new List<SCM.Common.Struct.clsOperationTimeStamp>();

                SCM.Common.Struct.clsOperationTimeStamp objOpsTimeStamp;
                new SCM.Common.Struct.clsOperationTimeStamp();

                //Set data of AWB for updating Arrival time stamp.
                foreach (GridViewRow gvr in grdAWBDetails.Rows)
                {
                    if (((RadioButton)gvr.FindControl("radSelectAWB")).Checked)
                    {
                        objOpsTimeStamp = new SCM.Common.Struct.clsOperationTimeStamp();
                        if (((Label)gvr.FindControl("lblAWB")).Text != "" &&
                            (((Label)gvr.FindControl("lblAWB")).Text.Contains("-")) &&
                            (((Label)gvr.FindControl("lblAWB")).Text.Length > ((Label)gvr.FindControl("lblAWB")).Text.IndexOf('-')))
                        {
                            objOpsTimeStamp.AWBPrefix = ((Label)gvr.FindControl("lblAWB")).Text.Substring(0,
                                ((Label)gvr.FindControl("lblAWB")).Text.IndexOf('-'));
                            objOpsTimeStamp.AWBNumber = ((Label)gvr.FindControl("lblAWB")).Text.Substring(
                                ((Label)gvr.FindControl("lblAWB")).Text.IndexOf('-') + 1);
                        }
                        else
                        {
                            objOpsTimeStamp.AWBPrefix = "";
                            objOpsTimeStamp.AWBNumber = ((Label)gvr.FindControl("lblAWB")).Text;
                        }
                        objOpsTimeStamp.Description = "";
                        DateTime dt = System.DateTime.Now;
                        if (DateTime.TryParseExact(((Label)gvr.FindControl("lblFlightDate")).Text, "dd/MM/yyyy", null,
                            System.Globalization.DateTimeStyles.None, out dt))
                        {
                            objOpsTimeStamp.FlightDt = dt;
                        }
                        else
                        {
                            objOpsTimeStamp.FlightDt = DateTime.Now;
                        }
                        objOpsTimeStamp.FlightNo = ((Label)gvr.FindControl("lblFlightNo")).Text;
                        objOpsTimeStamp.OperationalStatus = "RCS";
                        objOpsTimeStamp.OperationalType = "RCS";
                        objOpsTimeStamp.OperationDate = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy"); //DateTime.Now.ToString("dd/MM/yyyy");
                        objOpsTimeStamp.OperationTime = ((DateTime)Session["IT"]).ToString("HH:mm"); //DateTime.Now.ToString("HH:mm");
                        int pieceCount = 0;
                        if (int.TryParse(((TextBox)gvr.FindControl("txtAcceptedPcs")).Text, out pieceCount))
                        {
                            objOpsTimeStamp.Pieces = pieceCount;
                        }
                        else
                        {
                            objOpsTimeStamp.Pieces = 0;
                        }
                        decimal weight = 0;
                        if (decimal.TryParse(((TextBox)gvr.FindControl("txtAcceptedWt")).Text, out weight))
                        {
                            objOpsTimeStamp.Weight = weight;
                        }
                        else
                        {
                            objOpsTimeStamp.Weight = 0;
                        }
                        objOpsTimeStamp.ULDNumber = "";
                        objOpsTimeStamp.Station = Session["Station"].ToString();

                        //Add individual item to list.
                        objListOpsTime.Add(objOpsTimeStamp);

                        objOpsTimeStamp = null;
                        //Only single record will be selected at a time, so break from the loop.
                        break;

                    }
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
                lblStatus.ForeColor = Color.Red;
            }
        }

        protected void grdAWBDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                lblStatus.Text = "";
                if (e.CommandName == "Dimension")
                {
                    string AWBNumber = e.CommandArgument.ToString();
                    Session["awbnumber"] = AWBNumber;

                    for (int i = 0; i < grdAWBDetails.Rows.Count; i++)
                    {
                        ((RadioButton)grdAWBDetails.Rows[i].FindControl("radSelectAWB")).Checked = false;
                        if (((Label)grdAWBDetails.Rows[i].FindControl("lblAWB")).Text == AWBNumber)
                        {
                            ((RadioButton)grdAWBDetails.Rows[i].FindControl("radSelectAWB")).Checked = true;
                        }
                    }
                   

                    string[] PName = new string[1];
                    PName[0] = "AWBNo";

                    object[] PValue = new object[1];
                    PValue[0] = AWBNumber;

                    SqlDbType[] PType = new SqlDbType[1];
                    PType[0] = SqlDbType.VarChar;
                    grdAcceptance.DataSource = null;
                    grdAcceptance.DataBind();

                    DataSet Ds = null;

                    try
                    {
                        Ds = da.SelectRecords("sp_getAWBDimensionForCargoAccp", PName, PValue, PType);
                    }
                    catch { }
                    finally
                    { PName = null; PValue = null; PType = null; }

                    if (Ds != null)
                    {
                        if (Ds.Tables.Count > 0)
                        {
                            if (Ds.Tables[0].Rows.Count > 0)
                            {
                                hdStatus.Value = "Detail";
                                grdAcceptance.DataSource = Ds;
                                grdAcceptance.DataBind();
                                AccpTaskDiv.Attributes.Add("style", "display:block");
                                AccpGridDIV.Attributes.Add("style", "display:block");

                                for (int i = 0; i < grdAcceptance.Rows.Count; i++)
                                {
                                    if (Ds.Tables[0].Rows[0]["IsFlag"].ToString() == "Booking")
                                    {
                                        for (int h = 0; h < grdAWBDetails.Rows.Count; h++)
                                        {
                                            if (((RadioButton)grdAWBDetails.Rows[h].FindControl("radSelectAWB")).Checked)
                                            {
                                                for (int m = 0; m < Convert.ToInt32(((Label)grdAWBDetails.Rows[h].FindControl("lblRcvdPcs")).Text); m++)
                                                {
                                                    ((CheckBox)grdAcceptance.Rows[m].FindControl("chkAccept")).Checked = true;
                                                }
                                            }
                                        }


                                    }
                                    else
                                    {

                                        if (((Label)grdAcceptance.Rows[i].FindControl("lblIsAccp")).Text == "True")
                                            ((CheckBox)grdAcceptance.Rows[i].FindControl("chkAccept")).Checked = true;
                                        else
                                            ((CheckBox)grdAcceptance.Rows[i].FindControl("chkAccept")).Checked = false;
                                    }
                                    if (((Label)grdAcceptance.Rows[i].FindControl("lblIsAccp")).Text == "True")
                                        ((CheckBox)grdAcceptance.Rows[i].FindControl("chkAccept")).Checked = true;
                                    else
                                        ((CheckBox)grdAcceptance.Rows[i].FindControl("chkAccept")).Checked = false;
                                }

                                if (Ds.Tables[0].Rows[0]["isTamper"].ToString() == "True")
                                    chkTamper.Checked = true;

                                if (Ds.Tables[0].Rows[0]["isPackaging"].ToString() == "True")
                                    chkPackaging.Checked = true;

                                if (Ds.Tables[0].Rows[0]["isVisual"].ToString() == "True")
                                    chkVisual.Checked = true;

                                if (Ds.Tables[0].Rows[0]["isSmell"].ToString() == "True")
                                    chkSmell.Checked = true;

                                if (Ds.Tables[0].Rows[0]["isDGR"].ToString() == "True")
                                    chkDGR.Checked = true;

                                if (Ds.Tables[0].Rows[0]["isLiveAnimal"].ToString() == "True")
                                    chkLiveAnimal.Checked = true;

                                #region Calculate Total Vol,Weight and Scale Weight

                                decimal FinalTotal = 0, FinalVolume = 0, FinalScaleWt = 0, Volume = 0, Weight = 0, ScaleWt = 0;
                                for (int i = 0; i < grdAcceptance.Rows.Count; i++)
                                {
                                    Volume = 0; Weight = 0;

                                    if (((TextBox)grdAcceptance.Rows[i].FindControl("txtVol")).Text.Trim() != "")
                                        Volume = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtVol")).Text.Trim());

                                    if (((TextBox)grdAcceptance.Rows[i].FindControl("txtWt")).Text.Trim() != "")
                                        Weight = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtWt")).Text.Trim());

                                    if (((TextBox)grdAcceptance.Rows[i].FindControl("txtScaleWt")).Text.Trim() != "")
                                        ScaleWt = Convert.ToDecimal(((TextBox)grdAcceptance.Rows[i].FindControl("txtScaleWt")).Text.Trim());

                                    FinalVolume = FinalVolume + Volume;
                                    FinalTotal = FinalTotal + Weight;
                                    FinalScaleWt = FinalScaleWt + ScaleWt;

                                }

                                txtTotVolWt.Text = FinalTotal.ToString("0.00");
                                txtTotVolCms.Text = FinalVolume.ToString("0.00");
                                txtTotScaleWt.Text = FinalScaleWt.ToString("0.00");

                                decimal TotalVolume = 0;

                                if (txtTotVolCms.Text.Trim() != "")
                                    TotalVolume = Convert.ToDecimal(txtTotVolCms.Text.Trim());

                                if (TotalVolume != 0)
                                {
                                    txtTotVolMtr.Text = (TotalVolume / 10000).ToString("0.00");

                                #endregion Calculate Total Vol,Weight and Scale Weight

                                }
                            }
                            Ds.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/GHA_Acceptance.aspx");
            }
            catch (Exception ex)
            { }
        }

        protected void btnReceipt_Click(object sender, EventArgs e)
        {

        }

        #region grdAWBDetails_PageIndexChanging
        protected void grdAWBDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //Newly Added
                Session["dsDimesionAllAcceptance"] = null;
                Session["ULDLoc"] = null;
                //Newly Added End



                DataSet dsResult = (DataSet)Session["dsListGHA_Acceptance"];

                grdAWBDetails.PageIndex = e.NewPageIndex;
                grdAWBDetails.DataSource = dsResult.Copy();
                grdAWBDetails.DataBind();
                btnSave.Visible = true;
                btnCancel.Visible = true;
                LoadDropDown();
                AccpTaskDiv.Attributes.Add("style", "display:block");

                for (int i = 0; i < grdAWBDetails.Rows.Count; i++)
                {
                    #region Commented Code (19/01/2014)
                    //if (Convert.ToInt32(((Label)grdAWBDetails.Rows[i].FindControl("lblIsPieces")).Text) > 0)
                    //{
                    //    ((ImageButton)grdAWBDetails.Rows[i].FindControl("btnDimensionsPopup")).Visible = true;
                    //}
                    //else
                    //{
                    //    ((ImageButton)grdAWBDetails.Rows[i].FindControl("btnDimensionsPopup")).Visible = false;
                    //}
                    #endregion

                    string shc, comcode;
                    if (grdAWBDetails.Rows.Count > 0)
                    {
                        ((RadioButton)grdAWBDetails.Rows[0].FindControl("radSelectAWB")).Checked = true;
                        chkDGR.Checked = Convert.ToBoolean(((Label)grdAWBDetails.Rows[0].FindControl("lblDGR")).Text);
                        chkLiveAnimal.Checked = Convert.ToBoolean(((Label)grdAWBDetails.Rows[0].FindControl("lblAnimal")).Text);
                        chkPackaging.Checked = Convert.ToBoolean(((Label)grdAWBDetails.Rows[0].FindControl("lblPackage")).Text);
                        chkSmell.Checked = Convert.ToBoolean(((Label)grdAWBDetails.Rows[0].FindControl("lblSmell")).Text);
                        chkTamper.Checked = Convert.ToBoolean(((Label)grdAWBDetails.Rows[0].FindControl("lblTamper")).Text);
                        chkVisual.Checked = Convert.ToBoolean(((Label)grdAWBDetails.Rows[0].FindControl("lblVisual")).Text);
                        hdnAWBNo.Value = ((Label)grdAWBDetails.Rows[0].FindControl("lblAWB")).Text;
                        hdUCRNo.Value = ((Label)grdAWBDetails.Rows[0].FindControl("lblUCRNo")).Text;
                        if (((Label)grdAWBDetails.Rows[0].FindControl("lblUCRNo")).Text != "")
                        {
                            btnPrint.Enabled = true;
                        }
                        else
                            btnPrint.Enabled = false;

                        //if (Convert.ToInt32(((Label)grdAWBDetails.Rows[0].FindControl("lblIsPieces")).Text) > 0)
                        //{
                        //    ((ImageButton)grdAWBDetails.Rows[0].FindControl("btnDimensionsPopup")).Visible = true;
                        //}
                        //else
                        //{
                        //    ((ImageButton)grdAWBDetails.Rows[0].FindControl("btnDimensionsPopup")).Visible = false;
                        //}

                    }
                    shc = dsResult.Tables[0].Rows[i]["SHCCodes"].ToString();
                    comcode = dsResult.Tables[0].Rows[i]["CommodityCode"].ToString();

                    if (shc != "")
                        ((DropDownList)grdAWBDetails.Rows[i].FindControl("ddlSHC")).SelectedValue = shc;
                    if (comcode != "")
                        ((DropDownList)grdAWBDetails.Rows[i].FindControl("ddlCommCode")).SelectedValue = comcode;

                }

            }
            catch (Exception ex)
            { }
        }
        #endregion

        private bool SaveULDNoinMaster(string UDLNumber)
        {
            if (UDLNumber.Trim() == "")
                return false;

            string strULDPrefix = UDLNumber.Trim().Substring(0, 3);
            string strULDSuffix = UDLNumber.Trim().Substring(UDLNumber.Trim().Length - 2, 2);
            string strULDSerial = UDLNumber.Trim().Replace(strULDPrefix, "").Replace(strULDSuffix, "");

            BALULDMaster blULD = new BALULDMaster();

            blULD.SelectRecords(UDLNumber, strULDSuffix, 0, "0", "0", 0, 0, 0, "0", "", "", "", false, "", strULDSerial, Convert.ToString(Session["Station"]), Convert.ToString(Session["UserName"]), Convert.ToDateTime(Session["IT"]),
                "", 0, "0", "0", Convert.ToDateTime(Session["IT"]), "", "", "", false, "","Y","0");

            blULD = null;

            return true;
        }

        #region Button Print
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                if (grdAWBDetails.Rows.Count > 0)
                {
                    
                    string ULDNumber = hdULDNo.Value;
                    //string Location = hdLocation.Value;
                    string AWBNumber = hdnAWBNo.Value;
                    //string ULDOrigin = hdULDorigin.Value;
                    string ULDDestination = hdULDDestination.Value;
                    string FlightNumber=hdFlightNumber.Value;
                    string FlightDate=hdFlightDate.Value;
                    //char[] charSeparator = new char[] { ',' };
                    //foreach (string ULD in ULDNumber.Split('$'))

                    //{
                    //for (int i = 0; i < grdAWBDetails.Rows.Count; i++)
                    //{
                    //    if (((RadioButton)grdAWBDetails.Rows[i].FindControl("radSelectAWB")).Checked == true)
                    //    {
                    //        if (((Label)grdAWBDetails.Rows[i].FindControl("lblAWB")).Text == AWBNumber &&((Label)grdAWBDetails.Rows[i].FindControl("lblFlightNo")).Text == FlightNumber && ((Label)grdAWBDetails.Rows[i].FindControl("lblFlightDate")).Text == FlightDate)
                    //        {
                                string UCR = objUCR.generateUCR(AWBNumber, ULDNumber, "true", ULDDestination, "", DateTime.Now,
                                    Session["UserName"].ToString(), Convert.ToDateTime(Session["IT"].ToString()));
                                if (UCR != "")
                                {
                                    RenderUCRReport(UCR);
                                }
                                //btnPrint.Enabled = false;
                                //divPrintUCR.Attributes.Add("style", "display:none");
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Getting UCR Report Details

        #region FillUCRReport Session
        private DataTable FillUCRReport(string UCR)
        {
            try
            {

                DataSet dsgetUCRRpt = objUCR.GetUCRReportNew(UCR);
                if (dsgetUCRRpt != null && dsgetUCRRpt.Tables.Count > 0)
                {
                    return dsgetUCRRpt.Tables[0];
                }
                return null;
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Fill ULDIATA Codes
        private DataTable GetULDIATACodes(string UCR)
        {
            try
            {

                DataSet dsgetIATARpt = objUCR.GetIATACodesReport(UCR); //dbN.SelectRecords("spSubRptIATACodes", pname, pvalue, ptype);
                if (dsgetIATARpt != null && dsgetIATARpt.Tables.Count > 0)
                {
                    return dsgetIATARpt.Tables[0];
                }
                return null;
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion

        #region Get UCRULDReport Details
        private DataTable GetUCRULDRpt(string UCR)
        {
            try
            {
                DataSet dsgetUCRULDRpt = objUCR.GetUCRULDRpt(UCR);
                if (dsgetUCRULDRpt != null && dsgetUCRULDRpt.Tables.Count > 0)
                {
                    return dsgetUCRULDRpt.Tables[0];
                }
                return null;
            }
            catch (Exception ex)
            { return null; }
        }
        #endregion
        #endregion

        #region Render Report
        public void RenderUCRReport(string UCRNo)
        {
            try
            {
                DataTable dtRpt = FillUCRReport(UCRNo);
                DataTable dtIATARpt = GetULDIATACodes(UCRNo);
                DataTable dtUCRULDRpt = GetUCRULDRpt(UCRNo);
                ReportViewer rptUCRReport=new ReportViewer();

                string AWNNo = "";



                //A method that returns a collection for our report

                //Note: A report can have multiple data sources

                dtTable1 = new DataTable();
                dtTable1 = dtRpt;

                dtTable2 = new DataTable();
                dtTable2 = dtIATARpt;

                dtTable3 = new DataTable();
                dtTable3 = dtUCRULDRpt;

                //List<Employee> employeeCollection = GetData();
                if (dtTable1 != null)
                {
                    if (dtTable1.Rows.Count > 0)
                    {
                        AWNNo = dtTable1.Rows[0]["AWBNo"].ToString();
                    }
                }
                System.IO.MemoryStream Logo = null;
                try
                {

                    Logo = CommonUtility.GetImageStream(Page.Server);
                }
                catch (Exception ex)
                {
                    Logo = new System.IO.MemoryStream();
                }
                try
                {
                    dtTable1.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                }
                catch (Exception ex)
                { }
                //dtTable1.Rows.Add(Logo.ToArray());
                dtTable1.Rows[0]["Logo"] = Logo.ToArray();
                //LocalReport localReport = new LocalReport();
                //localReport.ReportPath = Server.MapPath("~/rptUCR.rdlc");

                //ReportDataSource reportDataSource = new ReportDataSource("dsShowUCR_DataTable1", dtTable1);
                //localReport.DataSources.Add(reportDataSource);





                //rptUCRReport.Reset();

                rptUCRReport.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = rptUCRReport.LocalReport;

                rep1.ReportPath = Server.MapPath("/rptUCR.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "Billing1.rdlc";
                rds1.Name = "dsUCR_DataTable1";
                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);

                rptUCRReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                #region Render to PDF
                try
                {
                    string reportType = "PDF";
                    //string mimeType;
                    //string encoding;
                    string fileNameExtension;
                    string deviceInfo = "<DeviceInfo><PageHeight>35cm</PageHeight><PageWidth>48cm</PageWidth></DeviceInfo>";

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
                    Response.AddHeader("content-disposition", "attachment; filename=" + "UCR" + "." + ".pdf");
                    Response.BinaryWrite(bytes); // create the file
                    Response.Flush();


                    //Response.End();
                }
                catch (Exception ex)
                {

                }
                #endregion
            }
            catch (Exception ex)
            { }
        }
        #endregion

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsUCR_DataTable2", dtTable2));
            e.DataSources.Add(new ReportDataSource("dsUCR_DataTable3", dtTable3));
        }

        #region Button Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/GHA_Acceptance.aspx");
            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region GenerateAWBDimensions
        private DataSet GenerateAWBDimensions(string AWBNumber, int AWBPieces, DataSet Dimensions, decimal AWBWt, bool IsCreate, string AWBPrefix,string FlightNo,string FlightDate)
        {
            DataSet ds = null;
            BookingBAL BAL = new BookingBAL();
            int Rowcount = 0;
           // string FlightNo = string.Empty, FlightDt = string.Empty;

            int intDimRowCount = 0;
            int FltPieces = 0;

            try
            {
                //if (Dimensions == null || Dimensions.Tables.Count < 1 || Dimensions.Tables[0].Rows.Count < 1)
                //{
                //    Dimensions = BAL.GenerateAWBDimensions(AWBNumber, AWBPieces, Dimensions, AWBWt, Convert.ToString(Session["UserName"]),
                //Convert.ToDateTime(Session["IT"]), false);
                //}

                if (Dimensions != null && Dimensions.Tables.Count > 0 && Dimensions.Tables[0].Rows.Count > 0)
                {

                    ds = BAL.GenerateAWBDimensionsAcceptance(AWBNumber, AWBPieces, Dimensions, AWBWt, Convert.ToString(Session["UserName"]),
               Convert.ToDateTime(Session["IT"]), IsCreate, AWBPrefix,"1",FlightNo,FlightDate);

                }
            }
            catch { }


            BAL = null;
            Dimensions = null;
            return ds;
        }
        #endregion

        #region btnOpsTime_Click
        protected void btnOpsTime_Click(object sender, ImageClickEventArgs e)
        {
            SaveOperationTime(true);
        }
        #endregion btnOpsTime_Click

        #region Button Send PRI
        protected void btnPRI_Click(object sender, EventArgs e)
        {
            ACASBAL objACAS = new ACASBAL();
            try
            {
                foreach (GridViewRow row in grdAWBDetails.Rows)
                {
                    if (((RadioButton)row.FindControl("radSelectAWB")).Checked)
                    {
                        string AWBNumber = ((Label)row.FindControl("lblAWB")).Text;
                        string FlightNo = ((Label)row.FindControl("lblFlightNo")).Text;
                        string FlightDate = ((Label)row.FindControl("lblFlightDate")).Text;

                        object[] QueryValues = { AWBNumber, FlightNo, FlightDate };
                        DataSet dsACAS = objACAS.CheckACASAWBAvailability(QueryValues);
                        if (dsACAS != null)
                        {
                            if (dsACAS.Tables[1].Rows[0]["Validate"].ToString() == "True")
                            {
                                StringBuilder sbPRI = objACAS.EncodingPRIMessage(QueryValues);

                                object[] QueryVal = { AWBNumber, 1, FlightNo, FlightDate, sbPRI.ToString().ToUpper() };

                                if (objACAS.UpdatePRIMessage(QueryVal))
                                {
                                    if (sbPRI != null)
                                    {
                                        if (sbPRI.ToString() != "")
                                        {
                                            object[] QueryValMail = { "PRI", FlightNo, FlightDate };
                                            //Getting MailID for PRI Message
                                            DataSet dMail = objACAS.GetCustomMessagesMailID(QueryValMail);
                                            string MailID = string.Empty;
                                            if (dMail != null)
                                            {
                                                MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                            }
                                            cls_BL.addMsgToOutBox("PRI", sbPRI.ToString().ToUpper(), "", MailID);
                                            lblStatus.Text = "PRI Message Sent Successfully!";
                                            lblStatus.ForeColor = Color.Green;
                                        }
                                    }
                                }

                            }
                        }
                        
                    }
                }

            }
            catch (Exception Ex)
            {
 
            }
            finally
            {
                objACAS = null;
            }
                
            
        }
        #endregion

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