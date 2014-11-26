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

using System.IO;
//using DataDynamics.Reports.Rendering.Pdf;
//using DataDynamics.Reports.Rendering.IO;
using Microsoft.Reporting.WebForms;
using System.Text;
//using PrintAWBLabels;
//using SampleDesktopTestPrint;

namespace ProjectSmartCargoManager
{
    public partial class frmScreeningDashboerd : System.Web.UI.Page
    {
        ScreeningDashboard sb = new ScreeningDashboard();
        private DataSet Dataset1 = new DataSet();
        private DataSet Dataset2 = new DataSet();
        //cls_DetamaxPrint objPrint = new cls_DetamaxPrint();
        
        #region Form Load
        protected void Page_Load(object sender, EventArgs e)
        {
            
            lblParaStatus.Text = "";
            lblStatus.Text = "";
            PanelError.Text = "";
            lblStatus.ForeColor = Color.Red;
            
            //Page.ClientScript.RegisterStartupScript(typeof(Page), ((DateTime)Session["IT"]).Ticks.ToString(), "scrollTo('" + autoUScroll.ClientID + "');", true);
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>scrollTo('" + autoUScroll.ClientID + "');</script>", false);
            try
            {
                this.TextBoxdate.Attributes.Add("onkeypress", "onclick()");
                this.txtFlightID.Attributes.Add("onkeypress", "onclick()");
                
                if (!IsPostBack)
                {
                    try
                    {
                        
                        TextBoxdate.Text = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy");
                        Session["UnscreenGrid"] = null;
                        Session["ScreenGrid"] = null;
                        Session["OldUnscreenGrid"] = null;
                        Session["OldScreenGrid"] = null;
                        Session["CallType"] = null;
                        Session["XrayData"] = null;
                        Session["K9Data"] = null;
                        Session["ETDData"] = null;
                        Session["PhyData"] = null;
                        Session["CHECKEDU_ITEMS"] = null;
                        Session["CHECKEDS_ITEMS"] = null;
                        Session["CHECKEDW_ITEMS"] = null;
                        string UserName = Session["Username"].ToString();
                        string StationCode = Session["Station"].ToString();
                        lblDepAirport.Text = Convert.ToString(Session["Station"]);
                        btnCallPnl.Visible = true;
                        btnReject.Visible = true;
                        btnUnassign.Visible = true;
                        btnUpdateScreen.Visible = true;
                        btnScreenNotReq.Visible = true;
                        showWar.Visible = false;
                        btnSendFSU.Visible = true;
                        btnPrintMFT.Visible = true;
                        btnReturn.Visible = true;
                        btnPrintLbl.Visible = true;
                        ddlFlPre.Text = Session["AirlinePrefix"] != null ? Session["AirlinePrefix"].ToString() : string.Empty;
                        ddlListPre.Text = Session["awbPrefix"] != null ? Session["awbPrefix"].ToString() : string.Empty;
                        TextBoxdate.Text = Session["IT"] != null ? ((DateTime)Session["IT"]).ToString("dd/MM/yyyy") : string.Empty;

                        //unsOk.Enabled = false;
                        btnPrintAWBDet.Visible = true;
                        fillSystemDropDown();
                        if (Session["CheckRFIDEnable"].ToString() == "True")
                        {
                            btnScreenNotReq.Visible = true;
                            gvUnScreening.Columns[2].Visible = false;

                            gdvULDDetails.Columns[2].Visible = false;
                            gdvULDDetails.Columns[6].Visible = false;
                            gdvULDDetails.Columns[8].Visible = false;
                            gdvULDDetails.Columns[11].Visible = false;
                            gdvULDDetails.Columns[14].Visible = false;
                            gdvULDDetails.Columns[17].Visible = false;

                            GVPopUp.Columns[2].Visible=false;
                            GVPopUp.Columns[6].Visible = false;
                            GVPopUp.Columns[9].Visible = false;
                            GVPopUp.Columns[12].Visible = false;
                            GVPopUp.Columns[15].Visible = false;
                            GVPopUp.Columns[18].Visible = false;

                        }
                        else {
                            btnScreenNotReq.Visible = false;
                            gvUnScreening.Columns[3].Visible = false;
                            gvUnScreening.Columns[5].Visible = false;
                            gvUnScreening.Columns[6].Visible = false;
                            gvUnScreening.Columns[7].Visible = false;

                            gdvULDDetails.Columns[3].Visible = false;
                            gdvULDDetails.Columns[5].Visible = false;
                            gdvULDDetails.Columns[9].Visible = false;
                            gdvULDDetails.Columns[12].Visible = false;
                            gdvULDDetails.Columns[15].Visible = false;
                            gdvULDDetails.Columns[18].Visible = false;
                            gdvULDDetails.Columns[19].Visible = false;
                            gdvULDDetails.Columns[20].Visible = false;

                            GVPopUp.Columns[3].Visible = false;
                            GVPopUp.Columns[5].Visible = false;
                            GVPopUp.Columns[7].Visible = false;
                            GVPopUp.Columns[10].Visible = false;
                            GVPopUp.Columns[13].Visible = false;
                            GVPopUp.Columns[16].Visible = false;
                            GVPopUp.Columns[19].Visible = false;
                            GVPopUp.Columns[20].Visible = false;
                        }
                        ddlFlPre.Focus();

                    }
                    catch (Exception ex)
                    {
                    }
                    LoginBL objBL = new LoginBL();
                    try
                    {
                        if (!Convert.ToBoolean(objBL.GetMasterConfiguration("handleCIMPMsg")))
                        {
                            btnSendFSU.Visible = false;                            
                        }
                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Form Load

        protected void fillSystemDropDown()
        {
            DataTable dtXray = new DataTable();
            DataTable dtK9 = new DataTable();
            DataTable dtETD = new DataTable();
            DataTable dtPhy = new DataTable();
            DataTable dtremark = new DataTable();
            DataTable dtScrException =new DataTable();

            dtXray.Columns.Add("Xray");
            dtXray.Rows.Add("Select");
            dtK9.Columns.Add("K9");
            dtK9.Rows.Add("Select");
            dtETD.Columns.Add("ETD");
            dtETD.Rows.Add("Select");

            dtPhy.Columns.Add("Physical");
            dtPhy.Rows.Add("Select");
            ddlxra.Items.Clear();
            ddlxra.Items.Add("ALL");
            
            dtremark.Columns.Add("Remarks");
            dtremark.Rows.Add("Select Remark");

            dtScrException.Columns.Add("ExceptionCode");
            dtScrException.Rows.Add("Select");
            
            try
            {
                DataSet dsSAUs = sb.getDataforDropDownInGrid(lblDepAirport.Text.Trim());
                #region Commented to Add New Remarks

                //for (int i = 0; i < dsSAUs.Tables[0].Rows.Count; i++)
                //{
                //    string val = dsSAUs.Tables[0].Rows[i]["Remark"].ToString();
                //    if (val != "")
                //    {
                //        dtremark.Rows.Add(val);
                //    }
                //}
                #endregion

                if (dsSAUs.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < dsSAUs.Tables[3].Rows.Count; i++)
                    {
                        string val = dsSAUs.Tables[3].Rows[i]["ExceptionCode"].ToString();
                        if (val != "")
                            dtScrException.Rows.Add(val);
                    }
                }
                for (int i = 0; i < dsSAUs.Tables[2].Rows.Count; i++)
                {
                    string val = dsSAUs.Tables[2].Rows[i]["Remark"].ToString();
                    if (val != "")
                    {
                        dtremark.Rows.Add(val);
                    }
                }
                if (dsSAUs != null && dsSAUs.Tables.Count > 0)
                {
                    if (dsSAUs.Tables[0].Rows.Count > 0)
                    {
                     
                        DataRow[] result = dsSAUs.Tables[0].Select("MachineType = 'Xray'");
                        foreach (DataRow row in result)
                        {
                            dtXray.Rows.Add(row[1]);
                            ddlxra.Items.Add(row[1].ToString());
                        }

                        DataRow[] resultK = dsSAUs.Tables[0].Select("MachineType = 'K9'");
                        foreach (DataRow row in resultK)
                        {
                          dtK9.Rows.Add(row[1]);
                        }

                        DataRow[] resultETD = dsSAUs.Tables[0].Select("MachineType = 'ETD'");
                        foreach (DataRow row in resultETD)
                        {
                            dtETD.Rows.Add(row[1]);
                        }
                     
                    }
                    else
                    {
                    }
                    if (dsSAUs.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsSAUs.Tables[1].Rows.Count; i++)
                        {
                            dtPhy.Rows.Add(dsSAUs.Tables[1].Rows[i][0].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                PanelError.Text = ex.Message;
                dtXray = null;
            }
            finally
            {
                Session["XrayData"] = dtXray;
                Session["K9Data"] = dtK9;
                Session["ETDData"] = dtETD;
                Session["PhyData"] = dtPhy;
                Session["RemData"] = dtremark;
                Session["ScrExceptionGrd"] = dtScrException;
            }
          
        }

        protected void BtnList_Click(object sender, EventArgs e)
        {
            try
            {
                tmrRef.Enabled = false;
                btnCallPnl.Visible = false;
                btnReject.Visible = false;
                btnUnassign.Visible = false;
                btnUpdateScreen.Visible = false;
                btnScreenNotReq.Visible = false;
                btnSendFSU.Visible = false;
                btnPrintMFT.Visible = false;
                try
                {
                    //if ((ddlFlPre.Text.Length <= -1 || txtFlightID.Text.Length <= -1 || TextBoxdate.Text.Length <= -1 || ddlListPre.Text.Length <= -1 || txtAWBSno.Text.Length <= -1 || txtSDest.Text.Length <= -1 || ddlDuartion.SelectedIndex < 0 || ddlStatus.SelectedIndex < 0) == false)
                    if (ddlFlPre.Text.Trim().Length <= 0 && txtFlightID.Text.Trim().Length <= 0 && TextBoxdate.Text.Trim().Length <= 0 && ddlListPre.Text.Length <= 0 && txtAWBSno.Text.Trim().Length <= 0 && txtSDest.Text.Trim().Length <= 0 && ddlDuartion.SelectedIndex <= 0 && ddlStatus.SelectedIndex <= 0)

                    {
                        lblStatus.Text = "Please Enter/select atleast one filter.";
                        return;   
                    }
                    
                }
                catch (Exception ex)
                { }
                try
                {
                    if (TextBoxdate.Text.Trim() != "")
                    {
                        DateTime dt = DateTime.ParseExact(TextBoxdate.Text.Trim(), "dd/MM/yyyy", null);
                    }
                }
                catch (Exception ex)
                {
                    lblParaStatus.Text = "Flight Date in not in Correct Format";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message12", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                    return;
                }

                try
                {
                    string duration = string.Empty;
                    string dest = string.Empty;
                    string flightdate = string.Empty;
                    string AWBNo = string.Empty;
                    string flightNo = string.Empty;

                    dest = txtSDest.Text.Trim();
                    flightdate = TextBoxdate.Text.Trim();
                    AWBNo = ddlListPre.Text.ToString() + "-" + txtAWBSno.Text.Trim();
                    flightNo = ddlFlPre.Text.ToString() + "" + txtFlightID.Text.Trim();

                    if (ddlDuartion.SelectedIndex == 0)
                    {
                        duration = "NA";
                    }
                    else
                    {
                        duration = ddlDuartion.SelectedItem.Text.ToString();
                        TimeSpan ts = TimeSpan.Parse(duration);
                        DateTime dt = Convert.ToDateTime(((DateTime)Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss")).Add(-(ts));
                        duration = dt.ToString();
                    }
                    if (txtSDest.Text.Trim() == "")
                    {
                        dest = "NA";
                    }
                    if (TextBoxdate.Text.Trim() == "")
                    {
                        flightdate = "NA";
                    }
                    if (txtAWBSno.Text.Trim() == "")
                    {
                        AWBNo = "NA";
                    }
                    if (txtFlightID.Text.Trim() == "")
                    {
                        flightNo = "NA";
                    }


                    DataSet dsSAUs = sb.getScreenAndUnScreen(flightNo, flightdate, AWBNo, dest, duration, lblDepAirport.Text.Trim(), Session["CheckRFIDEnable"].ToString(), ddlxra.SelectedItem.Text.ToString(),ddlStatus.Text.Trim());
                    if (dsSAUs != null && dsSAUs.Tables.Count > 0)
                    {
                        if (dsSAUs.Tables[0].Rows.Count > 0)
                        {
                            gvUnScreening.DataSource = dsSAUs.Tables[0];
                            gvUnScreening.DataBind();
                            btnCallPnl.Visible = true;
                            btnReject.Visible = true;
                            if (Session["CheckRFIDEnable"].ToString() == "True")
                            {
                                btnScreenNotReq.Visible = true;
                                lblCountUnT.Text = "Total Un-Screened Tag(s) : " + dsSAUs.Tables[0].Rows.Count;
                            }
                            else
                            {
                                int chkcnt = 0;
                                for (int i = 0; i < gvUnScreening.Rows.Count; i++)
                                {
                                    chkcnt = chkcnt + Convert.ToInt32(dsSAUs.Tables[0].Rows[i][6].ToString());
                                }
                                lblCountUnT.Text = "Total Un-Screened Pieces : " + chkcnt;
                            }
                        }
                        else
                        {
                            gvUnScreening.DataSource = null;
                            gvUnScreening.DataBind();
                            if (Session["CheckRFIDEnable"].ToString() == "True")
                                lblCountUnT.Text = "Total Un-Screened Tag(s) : 0";
                            else
                                lblCountUnT.Text = "Total Un-Screened Pieces : 0";
                        }
                        if (dsSAUs.Tables[1].Rows.Count > 0)
                        {
                            gdvULDDetails.DataSource = dsSAUs.Tables[1];
                            gdvULDDetails.DataBind();
                            btnUnassign.Visible = true;
                            btnUpdateScreen.Visible = true;
                            btnSendFSU.Visible = true;
                            btnPrintMFT.Visible = true;
                            btnReturn.Visible = true;
                            btnPrintLbl.Visible = true;
                            btnPrintAWBDet.Visible = true;
                            if (Session["CheckRFIDEnable"].ToString() == "True")
                                lblSCntTag.Text = "Total Screened Tag(s) : " + dsSAUs.Tables[1].Rows.Count;
                            else
                            {
                                int chkcnt = 0;
                                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                                {
                                    chkcnt = chkcnt + Convert.ToInt32(dsSAUs.Tables[1].Rows[i][20].ToString());
                                }
                                lblSCntTag.Text = "Total Screened Pieces : " + chkcnt;
                            }
                        }
                        else
                        {
                            gdvULDDetails.DataSource = null;
                            gdvULDDetails.DataBind();
                            if (Session["CheckRFIDEnable"].ToString() == "True")
                                lblSCntTag.Text = "Total Screened Tag(s) : 0";
                            else
                                lblSCntTag.Text = "Total Screened Pieces : 0";
                        }
                        if (dsSAUs.Tables[2].Rows.Count > 0)
                        {
                            if (dsSAUs.Tables[2].Rows.Count > 1)
                            {
                                showWar.Visible = true;
                                chkWarAWB.DataSource = dsSAUs.Tables[2];
                                chkWarAWB.DataTextField = "AWBNumber";
                                chkWarAWB.DataValueField = "AWBNumber";
                                chkWarAWB.DataBind();
                            }
                            else
                            {
                                showWar.Visible = false;
                            }

                        }
                        else
                        {
                            showWar.Visible = false;
                        }
                        Session["UnscreenGrid"] = dsSAUs.Tables[0];
                        Session["ScreenGrid"] = dsSAUs.Tables[1];
                        if (dsSAUs.Tables[0].Rows.Count == 0 && dsSAUs.Tables[1].Rows.Count == 0)
                        {
                            lblStatus.Text = "No Record Available in both Screening and Un-Screening";
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Problem occured during fetching data, Try Again";
                    }
                    Session["Rptdataset"] = dsSAUs;
                    //Code Cleanup
                    dsSAUs.Dispose();
                }
                catch (Exception ex)
                {
                    //lblStatus.Text = ex.Message;
                }
                finally
                {
                    tmrRef.Enabled = true;
                    
                    //  ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>StartTimer();</script>", false);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message14", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                
                }
            }
            catch (Exception ex)
            { }
            
        }
       
        protected DataTable fillDropinGridETD()
        {
            DataTable dtETD = new DataTable();

            try
            {
                dtETD = (DataTable)Session["ETDData"];
            }
            catch (Exception ex)
            {
                dtETD.Columns.Add("ETD");
                dtETD.Rows.Add("Select");
                PanelError.Text = ex.Message;
            }
            return dtETD;

        }
        protected DataTable fillDropinGridK9()
        {

            DataTable dtK9 = new DataTable();
            try
            {
                dtK9 = (DataTable)Session["K9Data"];
            }
            catch (Exception ex)
            {
                
            dtK9.Columns.Add("K9");
            dtK9.Rows.Add("Select");
            PanelError.Text = ex.Message;
            }

            return dtK9;
        }
        protected DataTable fillDropinGridXray()
        {
            DataTable dtXray = new DataTable();
            try
            {
                dtXray = (DataTable)Session["XrayData"];
            }
            catch (Exception ex)
            {
                dtXray.Columns.Add("Xray");
                dtXray.Rows.Add("Select");
                PanelError.Text = ex.Message;
            }
            return dtXray;
        }
        protected DataTable fillDropinGridPhysical()
        {

            DataTable dtPhy = new DataTable();
            try
            {
                dtPhy = (DataTable)Session["PhyData"];

            }
            catch (Exception ex)
            {
            dtPhy.Columns.Add("Physical");
            dtPhy.Rows.Add("Select");
            PanelError.Text = ex.Message;
            }
            return dtPhy;
        }
        protected DataTable fillDropinGridRmrk()
        {

            DataTable dtremark = new DataTable();
           
            try
            {
                dtremark = (DataTable)Session["RemData"];
            }
            catch (Exception ex)
            {

            }
            return dtremark;
        }
        protected DataTable fillScrExceptionDropDown()
        {

            DataTable dtremark = new DataTable();

            try
            {
                dtremark = (DataTable)Session["ScrExceptionGrd"];
            }
            catch (Exception ex)
            {

            }
            return dtremark;
        }
       
        #region old filling security type drop down (working)
        //protected DataTable fillDropinGridETD()
        //{


        //    DataTable dtETD = new DataTable();

        //    dtETD.Columns.Add("ETD");
        //    dtETD.Rows.Add("Select");

        //    try
        //    {

        //        DataSet dsSAUs = sb.getDataforDropDownInGrid(lblDepAirport.Text.Trim());
        //        if (dsSAUs != null && dsSAUs.Tables.Count > 0)
        //        {
        //            if (dsSAUs.Tables[0].Rows.Count > 0)
        //            {
        //                DataRow[] resultETD = dsSAUs.Tables[0].Select("MachineType = 'ETD'");
        //                foreach (DataRow row in resultETD)
        //                {
        //                    dtETD.Rows.Add(row[1]);
        //                }

        //            }
        //            else
        //            {

        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        PanelError.Text = ex.Message;
        //        dtETD = null;
        //    }
        //    finally
        //    {

        //    }
        //    return dtETD;

        //}
        //protected DataTable fillDropinGridK9()
        //{

        //    DataTable dtK9 = new DataTable();

        //    dtK9.Columns.Add("K9");
        //    dtK9.Rows.Add("Select");

        //    try
        //    {

        //        DataSet dsSAUs = sb.getDataforDropDownInGrid(lblDepAirport.Text.Trim());
        //        if (dsSAUs != null && dsSAUs.Tables.Count > 0)
        //        {
        //            if (dsSAUs.Tables[0].Rows.Count > 0)
        //            {

        //                DataRow[] resultK = dsSAUs.Tables[0].Select("MachineType = 'K9'");
        //                foreach (DataRow row in resultK)
        //                {
        //                    dtK9.Rows.Add(row[1]);
        //                }

        //            }
        //            else
        //            {

        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        PanelError.Text = ex.Message;
        //        dtK9 = null;
        //    }
        //    finally
        //    {
        //    }
        //    return dtK9;
        //}
        //protected DataTable fillDropinGridXray()
        //{
        //  //  DataSet dscop = new DataSet();

        //    DataTable dtXray = new DataTable();
        //    //DataTable dtK9 = new DataTable();
        //    //DataTable dtETD = new DataTable();
        //    //DataTable dtPhy = new DataTable();

        //    dtXray.Columns.Add("Xray");
        //    dtXray.Rows.Add("Select");
        //    //dtK9.Columns.Add("K9");
        //    //dtK9.Rows.Add("Select");
        //    //dtETD.Columns.Add("ETD");
        //    //dtETD.Rows.Add("Select");
        //    //dtPhy.Columns.Add("Xray");
        //    //dtPhy.Rows.Add("Select");

        //    try
        //    {

        //        DataSet dsSAUs = sb.getDataforDropDownInGrid(lblDepAirport.Text.Trim());
        //        if (dsSAUs != null && dsSAUs.Tables.Count > 0)
        //        {
        //            if (dsSAUs.Tables[0].Rows.Count > 0)
        //            {

        //                DataRow[] result = dsSAUs.Tables[0].Select("MachineType = 'Xray'");
        //                foreach (DataRow row in result)
        //                {
        //                    dtXray.Rows.Add(row[1]);
        //                }


        //                //DataRow[] resultK = dsSAUs.Tables[0].Select("MachineType = K9");
        //                //foreach (DataRow row in resultK)
        //                //{
        //                //    dtK9.Rows.Add(row[0]);
        //                //}


        //                //DataRow[] resultETD = dsSAUs.Tables[0].Select("MachineType = ETD");
        //                //foreach (DataRow row in resultETD)
        //                //{
        //                //    dtETD.Rows.Add(row[0]);
        //                //}

        //            }
        //            else
        //            {

        //            }
        //            //if (dsSAUs.Tables[1].Rows.Count > 0)
        //            //{
        //            //    dtPhy = dsSAUs.Tables[1];
        //            //}
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        PanelError.Text = ex.Message;
        //        dtXray = null;
        //    }
        //    finally {


        //    }
        //    //return dtdir;
        //    return dtXray;
        //    //return dscop;


        //}

        //protected DataTable fillDropinGridPhysical()
        //{

        //    DataTable dtPhy = new DataTable();

        //    dtPhy.Columns.Add("Physical");
        //    dtPhy.Rows.Add("Select");

        //    try
        //    {

        //        DataSet dsSAUs = sb.getDataforDropDownInGrid(lblDepAirport.Text.Trim());
        //        if (dsSAUs != null && dsSAUs.Tables.Count > 0)
        //        {

        //            if (dsSAUs.Tables[1].Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dsSAUs.Tables[1].Rows.Count; i++)
        //                { 
        //                dtPhy.Rows.Add(dsSAUs.Tables[1].Rows[i][0].ToString());
        //                }
        //                //dtPhy = dsSAUs.Tables[1];


        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        PanelError.Text = ex.Message;
        //        dtPhy = null;
        //    }
        //    finally
        //    {

        //    }
        //    //return dtdir;
        //    return dtPhy;

        //} 
        #endregion

        protected void tmrRef_Tick(object sender, EventArgs e)
        {
            if (tmrRef.Enabled)
            {
                tmrRef.Enabled = false;
            }
            else
                return;
            try
            {
               // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr", "javascript:checkvalid();", true);
                SaveCheckedValues();
                BtnList_Click(sender, e);
                PopulateCheckedValues();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr21", "javascript:putdivval();ChkAllSOrNot('true','U');ChkAllSOrNot('true','S');", true);

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
            finally {
                tmrRef.Enabled = true;
            }
        }

        private void PopulateCheckedValues()
        {
            #region for unscreen
            try
            {
                ArrayList userdetails = (ArrayList)Session["CHECKEDU_ITEMS"];
                if (userdetails != null && userdetails.Count > 0)
                {
                    foreach (GridViewRow gvrow in gvUnScreening.Rows)
                    {
                        string fultag=string.Empty;
                        if (Session["CheckRFIDEnable"].ToString() == "True")
                        {
                            fultag = ((Label)gvrow.FindControl("lblfulTagID")).Text.Trim();
                        }
                        else {
                            fultag = ((Label)gvrow.FindControl("lblAWBno")).Text.Trim();
                        }
                        bool result = ((CheckBox)gvrow.FindControl("chkUnScreen")).Checked;
                        if (userdetails.Contains(fultag))
                        {
                            CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkUnScreen");
                            myCheckBox.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            #endregion

            #region for screen
            try
            {
                ArrayList userdetails = (ArrayList)Session["CHECKEDS_ITEMS"];
                if (userdetails != null && userdetails.Count > 0)
                {
                    foreach (GridViewRow gvrow in gdvULDDetails.Rows)
                    {
                        string fultag = string.Empty;
                        if (Session["CheckRFIDEnable"].ToString() == "True")
                        {
                            fultag = ((Label)gvrow.FindControl("lblSFullTagID")).Text.Trim();
                        }
                        else {
                            fultag = ((Label)gvrow.FindControl("lblSAWBno")).Text.Trim();
                        }
                        bool result = ((CheckBox)gvrow.FindControl("chkScreen")).Checked;
                        if (userdetails.Contains(fultag))
                        {
                            CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkScreen");
                            myCheckBox.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            #endregion

            #region for Warning
            try
            {
                ArrayList userdetails = (ArrayList)Session["CHECKEDW_ITEMS"];
                if (userdetails != null && userdetails.Count > 0)
                {
                    for (int i = 0; i < chkWarAWB.Items.Count; i++)
                    {
                        string fulAWB = chkWarAWB.Items[i].Text.Trim();
                        bool result = chkWarAWB.Items[i].Selected;
                        if (userdetails.Contains(fulAWB))
                        {
                            chkWarAWB.Items[i].Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
        }
       
        private void SaveCheckedValues()
        {
            #region for UnScreen
            try
            {
                ArrayList UnScreendetails = new ArrayList();
                string Tagf = string.Empty;
                foreach (GridViewRow gvrow in gvUnScreening.Rows)
                {
                    string fultag = string.Empty;
                    if (Session["CheckRFIDEnable"].ToString() == "True")
                    {
                        fultag = ((Label)gvrow.FindControl("lblfulTagID")).Text.Trim();
                    }
                    else
                    {
                        fultag = ((Label)gvrow.FindControl("lblAWBno")).Text.Trim();
                    }
                    bool result = ((CheckBox)gvrow.FindControl("chkUnScreen")).Checked;

                    // Check in the Session
                    if (Session["CHECKEDU_ITEMS"] != null)
                        UnScreendetails = (ArrayList)Session["CHECKEDU_ITEMS"];
                    if (result)
                    {
                        if (!UnScreendetails.Contains(fultag))
                            UnScreendetails.Add(fultag);
                    }
                    else
                        UnScreendetails.Remove(fultag);
                }
                if (UnScreendetails != null && UnScreendetails.Count > 0)
                    Session["CHECKEDU_ITEMS"] = UnScreendetails;
            }
            catch (Exception ex)
            {
            }
            #endregion

            #region for Screen
            try
            {
                ArrayList Screendetails = new ArrayList();
                string Tagf = string.Empty;
                foreach (GridViewRow gvrow in gdvULDDetails.Rows)
                {
                    string fultag = string.Empty;
                    if (Session["CheckRFIDEnable"].ToString() == "True")
                    {
                        fultag = ((Label)gvrow.FindControl("lblSFullTagID")).Text.Trim();
                    }
                    else
                    {
                        fultag = ((Label)gvrow.FindControl("lblSAWBno")).Text.Trim();
                    }
                    bool result = ((CheckBox)gvrow.FindControl("chkScreen")).Checked;

                    // Check in the Session
                    if (Session["CHECKEDS_ITEMS"] != null)
                        Screendetails = (ArrayList)Session["CHECKEDS_ITEMS"];
                    if (result)
                    {
                        if (!Screendetails.Contains(fultag))
                            Screendetails.Add(fultag);
                    }
                    else
                        Screendetails.Remove(fultag);
                }
                if (Screendetails != null && Screendetails.Count > 0)
                    Session["CHECKEDS_ITEMS"] = Screendetails;
            }
            catch (Exception ex)
            {
            }
            #endregion

            #region for Warning
            try
            {
                ArrayList Wardetails = new ArrayList();
                string AWBf = string.Empty;
                for (int i = 0; i < chkWarAWB.Items.Count; i++)
                {
                    string fulAWB = chkWarAWB.Items[i].Text.Trim();
                    bool result = chkWarAWB.Items[i].Selected;
                    if (Session["CHECKEDW_ITEMS"] != null)
                        Wardetails = (ArrayList)Session["CHECKEDW_ITEMS"];

                    if (result)
                    {
                        if (!Wardetails.Contains(fulAWB))
                            Wardetails.Add(fulAWB);
                    }
                    else
                        Wardetails.Remove(fulAWB);
                }
                if (Wardetails != null && Wardetails.Count > 0)
                    Session["CHECKEDW_ITEMS"] = Wardetails;
            }
            catch (Exception ex)
            {
            }
            #endregion
        }

        protected void btnPopOK_Click(object sender, EventArgs e)
        {
            try
            {
                PanelError.Text = "";
                if (GVPopUp.Rows.Count > 0)
                {
                    DataTable dtLeft = (DataTable)Session["UnscreenGrid"];
                    DataTable dtRight = (DataTable)Session["ScreenGrid"];
                    Session["OldUnscreenGrid"] = Session["UnscreenGrid"];
                    Session["OldScreenGrid"] = Session["ScreenGrid"];
                    if (Session["CallType"].ToString().Trim() != "Reject")
                    {
                        if (Session["CheckRFIDEnable"].ToString() != "True")
                        {

                            for (int i = 0; i < GVPopUp.Rows.Count; i++)
                            {
                                int AcceptedPcs = 0;
                                if (((Label)GVPopUp.Rows[i].FindControl("lblAcceptedPcs")).Text.Trim() != "")
                                {
                                    AcceptedPcs = Convert.ToInt32(((Label)GVPopUp.Rows[i].FindControl("lblAcceptedPcs")).Text.Trim());
                                }
                                else
                                { 
                                    ((Label)GVPopUp.Rows[i].FindControl("lblAcceptedPcs")).Text = AcceptedPcs.ToString(); 
                                }
                                int totCnt = 0;
                                if (((Label)GVPopUp.Rows[i].FindControl("lblPopcntAWBno")).Text.Trim() != "")
                                {
                                    totCnt = Convert.ToInt32(((Label)GVPopUp.Rows[i].FindControl("lblPopcntAWBno")).Text.Trim());
                                }
                                else
                                { 
                                    ((Label)GVPopUp.Rows[i].FindControl("lblPopcntAWBno")).Text = totCnt.ToString(); 
                                }
                                int totScan = 0;
                                if (((TextBox)GVPopUp.Rows[i].FindControl("lblPopTotScan")).Text.Trim() != "")
                                {
                                    totScan = Convert.ToInt32((((TextBox)GVPopUp.Rows[i].FindControl("lblPopTotScan")).Text.Trim()));
                                }
                                else
                                {
                                    ((TextBox)GVPopUp.Rows[i].FindControl("lblPopTotScan")).Text = totScan.ToString();
                                }
                                int xrayCnt = 0;
                                if (((TextBox)GVPopUp.Rows[i].FindControl("lblPopXrayCount")).Text.Trim() != "")
                                {
                                    xrayCnt = Convert.ToInt32(((TextBox)GVPopUp.Rows[i].FindControl("lblPopXrayCount")).Text.Trim());
                                }
                                else
                                {
                                    ((TextBox)GVPopUp.Rows[i].FindControl("lblPopXrayCount")).Text = xrayCnt.ToString();
                                }
                                int k9Cnt = 0;
                                if (((TextBox)GVPopUp.Rows[i].FindControl("lblPopK9Count")).Text.Trim() != "")
                                {
                                    k9Cnt = Convert.ToInt32((((TextBox)GVPopUp.Rows[i].FindControl("lblPopK9Count")).Text.Trim()));
                                }
                                else
                                {
                                    ((TextBox)GVPopUp.Rows[i].FindControl("lblPopK9Count")).Text = k9Cnt.ToString();
                                }
                                int etdCnt = 0;
                                if (((TextBox)GVPopUp.Rows[i].FindControl("lblPopETDCount")).Text.Trim() != "")
                                {
                                    etdCnt = Convert.ToInt32(((TextBox)GVPopUp.Rows[i].FindControl("lblPopETDCount")).Text.Trim());
                                }
                                else
                                {
                                    ((TextBox)GVPopUp.Rows[i].FindControl("lblPopETDCount")).Text = etdCnt.ToString();
                                }
                                int phyCnt = 0;
                                if (((TextBox)GVPopUp.Rows[i].FindControl("lblPopPhysicalCount")).Text.Trim() != "")
                                {
                                    phyCnt = Convert.ToInt32((((TextBox)GVPopUp.Rows[i].FindControl("lblPopPhysicalCount")).Text.Trim()));
                                }
                                else
                                {
                                    ((TextBox)GVPopUp.Rows[i].FindControl("lblPopPhysicalCount")).Text = phyCnt.ToString();
                                }
                                string IsScreenReq = ((Label)GVPopUp.Rows[i].FindControl("lblscrrq")).Text.Trim();
                                if (IsScreenReq == "Y")
                                {
                                    if (totScan > AcceptedPcs)
                                    {
                                        PanelError.Text = "Total Scanned Pcs cannot be greater than Accepted Pcs.., RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (totScan != AcceptedPcs && totScan < AcceptedPcs)
                                    {
                                        PanelError.Text = "Partial Screening not allowed..Please try again!, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                }
                                if (IsScreenReq == "Y")
                                {
                                    //If Exception Selected check Xray,K9,ETD and Physical count.
                                    if (((DropDownList)GVPopUp.Rows[i].FindControl("ddlScrExceptions")).SelectedIndex > 0 && (xrayCnt != 0 || k9Cnt != 0 || etdCnt != 0 || phyCnt != 0))
                                    {
                                        PanelError.Text = "XrayCount,K9 Count,ETD Count and Physical Count must be 0, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    //If Exception Selected check Xray Type Dropdown
                                    if (((DropDownList)GVPopUp.Rows[i].FindControl("ddlScrExceptions")).SelectedIndex > 0 && ((DropDownList)GVPopUp.Rows[i].FindControl("lblPopXRayID")).SelectedIndex != 0)
                                    {
                                        PanelError.Text = "Do not select Xray type, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    //If Exception Selected check ETD Drop down
                                    if (((DropDownList)GVPopUp.Rows[i].FindControl("ddlScrExceptions")).SelectedIndex > 0 && ((DropDownList)GVPopUp.Rows[i].FindControl("lblPopETDID")).SelectedIndex != 0)
                                    {
                                        PanelError.Text = "Do not select ETD type, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    //If Exception Selected check K9 Drop down
                                    if (((DropDownList)GVPopUp.Rows[i].FindControl("ddlScrExceptions")).SelectedIndex > 0 && ((DropDownList)GVPopUp.Rows[i].FindControl("lblPopCanineID")).SelectedIndex != 0)
                                    {
                                        PanelError.Text = "Do not select ETD type, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }

                                    if (totScan > (xrayCnt + k9Cnt + etdCnt + phyCnt) && ((DropDownList)GVPopUp.Rows[i].FindControl("ddlScrExceptions")).SelectedIndex <=0)
                                    {
                                        PanelError.Text = "Total Scan Count Cannot greater than Sum of all Scan Type, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (totCnt < totScan || totCnt < xrayCnt || totCnt < k9Cnt || totCnt < etdCnt || totCnt < phyCnt)
                                    {
                                        PanelError.Text = "Total Count cannot be less , RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (totScan < xrayCnt || totScan < k9Cnt || totScan < etdCnt || totScan < phyCnt)
                                    {
                                        PanelError.Text = "Scan Count can not be less than any Checking Type Count , RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopXRayID")).SelectedIndex != 0 && ((TextBox)GVPopUp.Rows[i].FindControl("lblPopXrayCount")).Text.Trim() == "0")
                                    {
                                        PanelError.Text = "X-ray Count Must be greater than 0, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopXRayID")).SelectedIndex == 0 && Convert.ToInt32(((TextBox)GVPopUp.Rows[i].FindControl("lblPopXrayCount")).Text.Trim()) > 0)
                                    {
                                        PanelError.Text = "Select X-ray Type, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopCanineID")).SelectedIndex != 0 && ((TextBox)GVPopUp.Rows[i].FindControl("lblPopK9Count")).Text.Trim() == "0")
                                    {
                                        PanelError.Text = "K-9 Count Must be greater than 0, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopCanineID")).SelectedIndex == 0 && Convert.ToInt32(((TextBox)GVPopUp.Rows[i].FindControl("lblPopK9Count")).Text.Trim()) > 0)
                                    {
                                        PanelError.Text = "Select K-9 Type, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopETDID")).SelectedIndex != 0 && ((TextBox)GVPopUp.Rows[i].FindControl("lblPopETDCount")).Text.Trim() == "0")
                                    {
                                        PanelError.Text = "ETD Count Must be greater than 0, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopETDID")).SelectedIndex == 0 && Convert.ToInt32(((TextBox)GVPopUp.Rows[i].FindControl("lblPopETDCount")).Text.Trim()) > 0)
                                    {
                                        PanelError.Text = "Select ETD Type, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    //if (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopPhysical")).SelectedIndex != 0 && ((TextBox)GVPopUp.Rows[i].FindControl("lblPopPhysicalCount")).Text.Trim() == "0")
                                    //{
                                    //    PanelError.Text = "Physical Count Must be greater than 0, RowNo : " + (i + 1).ToString();
                                    //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                    //    return;
                                    //}
                                    if (((TextBox)GVPopUp.Rows[i].FindControl("lblPopPhysical")).Text != "" && ((TextBox)GVPopUp.Rows[i].FindControl("lblPopPhysicalCount")).Text.Trim() == "0")
                                    {
                                        PanelError.Text = "Physical Count Must be greater than 0, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (((TextBox)GVPopUp.Rows[i].FindControl("lblPopPhysical")).Text == "" && Convert.ToInt32(((TextBox)GVPopUp.Rows[i].FindControl("lblPopPhysicalCount")).Text.Trim()) > 0)
                                    {
                                        PanelError.Text = "Select Physical Type, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                   
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < GVPopUp.Rows.Count; i++)
                            {
                                string AWBNo = ((Label)GVPopUp.Rows[i].FindControl("lblPopAWBno")).Text.Trim();
                                string ccsf = (((Label)GVPopUp.Rows[i].FindControl("lblPopCCSF")).Text.Trim());
                                if (ccsf.ToUpper() == Convert.ToString("false").ToUpper())
                                {
                                    if ((((DropDownList)GVPopUp.Rows[i].FindControl("lblPopXRayID")).SelectedIndex == 0 && ((DropDownList)GVPopUp.Rows[i].FindControl("lblPopCanineID")).SelectedIndex == 0 && ((DropDownList)GVPopUp.Rows[i].FindControl("lblPopETDID")).SelectedIndex == 0 && ((TextBox)GVPopUp.Rows[i].FindControl("lblPopPhysical")).Text == ""))
                                    {
                                        PanelError.Text = "For non CCSF shipments, then atleast one Screening type needs to be selected, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopXRayID")).SelectedIndex != 0 && ((TextBox)GVPopUp.Rows[i].FindControl("txtXrayTime")).Text.Trim() == "")
                                    {
                                        PanelError.Text = "X-ray Time is Required, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopCanineID")).SelectedIndex != 0 && ((TextBox)GVPopUp.Rows[i].FindControl("txtK9Time")).Text.Trim() == "")
                                    {
                                        PanelError.Text = "K-9 Time is Required, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopETDID")).SelectedIndex != 0 && ((TextBox)GVPopUp.Rows[i].FindControl("txtETDTime")).Text.Trim() == "")
                                    {
                                        PanelError.Text = "ETD Time is Required, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                    if (((TextBox)GVPopUp.Rows[i].FindControl("lblPopPhysical")).Text != ""  && ((TextBox)GVPopUp.Rows[i].FindControl("txtPhysicalTime")).Text.Trim() == "")
                                    {
                                        PanelError.Text = "Physical Time is Required, RowNo : " + (i + 1).ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                        return;
                                    }
                                }

                                try
                                {
                                    DateTime dt = DateTime.Parse(((TextBox)GVPopUp.Rows[i].FindControl("txtXrayTime")).Text.Trim());
                                }
                                catch (Exception ex)
                                {
                                    PanelError.Text = "Xray Time is Not in Correct Format, RowNo : " + (i + 1).ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                    return;
                                }
                                try
                                {
                                    DateTime dt = DateTime.Parse(((TextBox)GVPopUp.Rows[i].FindControl("txtK9Time")).Text.Trim());
                                }
                                catch (Exception ex)
                                {
                                    PanelError.Text = "K9 Time is Not in Correct Format, RowNo : " + (i + 1).ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                    return;
                                }
                                try
                                {
                                    DateTime dt = DateTime.Parse(((TextBox)GVPopUp.Rows[i].FindControl("txtETDTime")).Text.Trim());
                                }
                                catch (Exception ex)
                                {
                                    PanelError.Text = "ETD Time is Not in Correct Format, RowNo : " + (i + 1).ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                    return;
                                }
                                try
                                {
                                    DateTime dt = DateTime.Parse(((TextBox)GVPopUp.Rows[i].FindControl("txtPhysicalTime")).Text.Trim());
                                }
                                catch (Exception ex)
                                {
                                    PanelError.Text = "Physical Time is Not in Correct Format, RowNo : " + (i + 1).ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                    return;
                                }
                            }
                        }
                    }
                    else {
                        for (int i = 0; i < GVPopUp.Rows.Count; i++)
                        {
                            string AWBNo = ((Label)GVPopUp.Rows[i].FindControl("lblPopAWBno")).Text.Trim();
                            string rejectReason = (((TextBox)GVPopUp.Rows[i].FindControl("lblPopReject")).Text.Trim());
                            int totCnt = Convert.ToInt32(((Label)GVPopUp.Rows[i].FindControl("lblPopcntAWBno")).Text.Trim());
                            int totScan = Convert.ToInt32((((TextBox)GVPopUp.Rows[i].FindControl("lblPopTotScan")).Text.Trim()));
                            if (totScan > totCnt)
                            {
                                PanelError.Text = "Reject Piece(s) Cannot greater than Total Pieces, RowNo : " + (i + 1).ToString();
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                return;
                            }
                            if (totScan ==0)
                            {
                                PanelError.Text = "Reject Piece(s) Cannotbe Zero, RowNo : " + (i + 1).ToString();
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                return;
                            }
                            if (rejectReason.Trim() == "")
                            {
                                PanelError.Text = "Reason Of Rejection Required, RowNo : " + (i + 1).ToString();
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                                return;
                            }
                        }
                    }
                        try
                        {
                            using (SqlConnection cnReject = new SqlConnection(Global.GetConnectionString()))
                            {
                                cnReject.Open();
                                SqlTransaction sqlTran = cnReject.BeginTransaction();
                                SqlCommand command = cnReject.CreateCommand();
                                command.Transaction = sqlTran;

                                try
                                {
                                    for (int i = 0; i < GVPopUp.Rows.Count; i++)
                                    {
                                        string totalCount = (((TextBox)GVPopUp.Rows[i].FindControl("lblPopTotScan")).Text.Trim());
                                        if (totalCount == "")
                                            totalCount = "0";
                                        string XrayCount = (((TextBox)GVPopUp.Rows[i].FindControl("lblPopXrayCount")).Text.Trim());
                                        if (XrayCount == "")
                                            XrayCount = "0";
                                        string k9Count = (((TextBox)GVPopUp.Rows[i].FindControl("lblPopK9Count")).Text.Trim());
                                        if (k9Count == "")
                                            k9Count = "0";
                                        string ETDCount = (((TextBox)GVPopUp.Rows[i].FindControl("lblPopETDCount")).Text.Trim());
                                        if (ETDCount == "")
                                            ETDCount = "0";
                                        string PhysicalCount = (((TextBox)GVPopUp.Rows[i].FindControl("lblPopPhysicalCount")).Text.Trim());
                                        if (PhysicalCount == "")
                                            PhysicalCount = "0";
                                        string totAWBCnt = (((Label)GVPopUp.Rows[i].FindControl("lblPopcntAWBno")).Text.Trim());

                                        string AWBNo = ((Label)GVPopUp.Rows[i].FindControl("lblPopAWBno")).Text.Trim();
                                        string tagID = (((Label)GVPopUp.Rows[i].FindControl("lblPopTagID")).Text.Trim());
                                        string Dest = (((Label)GVPopUp.Rows[i].FindControl("lblPopDest")).Text.Trim());
                                        string ccsf = (((Label)GVPopUp.Rows[i].FindControl("lblPopCCSF")).Text.Trim());
                                        string xray = (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopXRayID")).SelectedItem.Text.Trim());
                                        if (xray == "Select")
                                        {
                                            xray = ""; XrayCount = "0";
                                        }
                                        string xrayTime = (((TextBox)GVPopUp.Rows[i].FindControl("txtXrayTime")).Text.Trim());
                                        if (xrayTime.Trim() == "")
                                        {
                                            xrayTime = "1900/01/01";
                                        }
                                        string k9 = (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopCanineID")).SelectedItem.Text.Trim());
                                        if (k9 == "Select")
                                        {
                                            k9 = ""; k9Count = "0";
                                        }
                                        string k9Time = (((TextBox)GVPopUp.Rows[i].FindControl("txtK9Time")).Text.Trim());
                                        if (k9Time.Trim() == "")
                                        {
                                            k9Time = "1900/01/01";
                                        }
                                        string etd = (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopETDID")).SelectedItem.Text.Trim());
                                        if (etd == "Select")
                                        {
                                            etd = ""; ETDCount = "0";
                                        }
                                        string etdTime = (((TextBox)GVPopUp.Rows[i].FindControl("txtETDTime")).Text.Trim());
                                        if (etdTime.Trim() == "")
                                        {
                                            etdTime = "1900/01/01";
                                        }
                                        //string physical = (((DropDownList)GVPopUp.Rows[i].FindControl("lblPopPhysical")).SelectedItem.Text.Trim());
                                        //if (physical == "Select")
                                        //{
                                        //    physical = ""; PhysicalCount = "0";
                                        //}

                                        string physical = (((TextBox)GVPopUp.Rows[i].FindControl("lblPopPhysical")).Text.Trim());
                                        if (physical == "")
                                        {
                                            physical = ""; PhysicalCount = "0";
                                        }
                                        string physicalTime = (((TextBox)GVPopUp.Rows[i].FindControl("txtPhysicalTime")).Text.Trim());
                                        if (physicalTime.Trim() == "")
                                        {
                                            physicalTime = "1900/01/01";
                                        }
                                        string isSubTag = (((Label)GVPopUp.Rows[i].FindControl("lblPopSubTag")).Text.Trim());
                                        string location = lblDepAirport.Text.Trim();
                                        string fulltag = (((Label)GVPopUp.Rows[i].FindControl("lblPopfulTagID")).Text.Trim());
                                        string rejectReason = (((TextBox)GVPopUp.Rows[i].FindControl("lblPopReject")).Text.Trim());
                                        string remark=null;
                                        if ((((DropDownList)GVPopUp.Rows[i].FindControl("ddlRemark")).SelectedIndex > 0))
                                            remark = (((DropDownList)GVPopUp.Rows[i].FindControl("ddlRemark")).SelectedItem.Text);
                                        else remark = "No Remark";
                                        string isScreened = (((Label)GVPopUp.Rows[i].FindControl("lblscrrq")).Text.Trim());
                                        string addremark = (((TextBox)GVPopUp.Rows[i].FindControl("txtAddRemrk")).Text.Trim());
                                        if (isScreened != "Y")
                                        {
                                            if (addremark == "")
                                            {
                                                addremark = "Pre-Screened";
                                            }
                                        }
                                        string FlightNo = (((Label)GVPopUp.Rows[i].FindControl("lblFlightNo")).Text.Trim());
                                        string FlightDate = (((Label)GVPopUp.Rows[i].FindControl("lblFlightDate")).Text.Trim());
                                        string Location = (((TextBox)GVPopUp.Rows[i].FindControl("txtLocation")).Text.Trim());
                                        string ScrException = "";
                                        if (((DropDownList)GVPopUp.Rows[i].FindControl("ddlScrExceptions")).SelectedIndex == 0)
                                            ScrException = "";
                                        else
                                            ScrException = ((DropDownList)GVPopUp.Rows[i].FindControl("ddlScrExceptions")).SelectedItem.Text;
                                        #region save
                                        bool res = false; 
                                        if (Session["CallType"].ToString().Trim() == "UnScreen")
                                        {
                                            res = sb.SaveTag(location, AWBNo, fulltag, Dest, ccsf, xray, xrayTime, k9, k9Time, etd, etdTime, physical, physicalTime, isSubTag, Session["Username"].ToString(), ((DateTime)Session["IT"]).ToString(), totalCount, XrayCount, k9Count, ETDCount, PhysicalCount, Session["CheckRFIDEnable"].ToString(),remark,addremark,FlightNo,FlightDate,Location,ScrException);
                                            int totscan = int.Parse(totalCount);

                                            #region ACAS PRI Message Automation Deepak(15/04/2014)
                                            try
                                            {
                                                ACASBAL objACAS = new ACASBAL();
                                                  LoginBL objLogin = new LoginBL();
                                                  if (Convert.ToBoolean(objLogin.GetMasterConfiguration("ACASAutomation")))
                                                  {
                                                      if (objACAS.ACASFRITriggerPointValidation() == "SC")
                                                      {
                                                          object[] QueryValues = { AWBNo, FlightNo, FlightDate };
                                                          DataSet dsACAS = objACAS.CheckACASAWBAvailability(QueryValues);
                                                          if (dsACAS != null)
                                                          {
                                                              if (dsACAS.Tables[1].Rows[0]["Validate"].ToString() == "True")
                                                              {
                                                                  StringBuilder sbPRI = objACAS.EncodingPRIMessage(QueryValues);

                                                                  object[] QueryVal = { AWBNo, 1, FlightNo, FlightDate, sbPRI.ToString().ToUpper() };

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
                                                    object[] QueryValues = { AWBNo, FlightNo, FlightDate };
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
                                            //ViewState["totscn"] = totscan;
                                            //ViewState["awbnum"] = AWBNo;
                                            //res = sb.UpdateLbl(location, totscan);
                                            //lblDetails();
                                        }
                                        else if (Session["CallType"].ToString().Trim() == "Screen")
                                        {
                                            res = sb.UpdateScreenTag(location, AWBNo, fulltag, Dest, ccsf, xray, xrayTime, k9, k9Time, etd, etdTime, physical, physicalTime, isSubTag, Session["Username"].ToString(), ((DateTime)Session["IT"]).ToString(), totalCount, XrayCount, k9Count, ETDCount, PhysicalCount, Session["CheckRFIDEnable"].ToString(), remark, addremark, FlightNo, FlightDate,Location,ScrException);
                                        }
                                        else if (Session["CallType"].ToString().Trim() == "Reject")
                                        {
                                            res = sb.Reject(location, AWBNo, fulltag, Dest, ccsf, xray, xrayTime, k9, k9Time, etd, etdTime, physical, physicalTime, isSubTag, Session["Username"].ToString(), ((DateTime)Session["IT"]).ToString(), totalCount, XrayCount, k9Count, ETDCount, PhysicalCount, Session["CheckRFIDEnable"].ToString(), rejectReason, FlightNo, FlightDate,Location);
                                        }
                                        if (res == false)
                                        {
                                            Session["UnscreenGrid"] = Session["OldUnscreenGrid"];
                                            Session["ScreenGrid"] =  Session["OldScreenGrid"];
                                            gvUnScreening.DataSource = (DataTable)Session["OldUnscreenGrid"];
                                            gvUnScreening.DataBind();
                                            gdvULDDetails.DataSource = (DataTable)Session["OldScreenGrid"];
                                            gdvULDDetails.DataBind();
                                            sqlTran.Rollback();
                                            lblStatus.Text = PanelError.Text = "Error during Action, AWBNo : " + AWBNo;
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr5", "javascript:putdivval();", true);

                                            return;
                                        }
                                        #endregion

                                        if (Session["CallType"].ToString().Trim() == "UnScreen")
                                        {
                                            dtRight.Rows.Add(AWBNo, Dest, ccsf, xray, xrayTime, k9, k9Time, etd, etdTime, physical, physicalTime, tagID, isSubTag, fulltag,((DateTime)Session["IT"]).ToString(),totAWBCnt,XrayCount,k9Count,ETDCount,PhysicalCount,totalCount);
                                            for (int k = 0; k < dtLeft.Rows.Count; k++)
                                            {
                                                try
                                                {
                                                    if (dtLeft.Rows[k][0].ToString() == AWBNo && dtLeft.Rows[k][3].ToString() == tagID && dtLeft.Rows[k][5].ToString() == fulltag)
                                                    {
                                                        dtLeft.Rows[k].Delete();
                                                        dtLeft.AcceptChanges();
                                                        break;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                            }
                                        }
                                    }
                                    sqlTran.Commit();
                                    //BtnList_Click(sender, e);
                                    //if (Session["CallType"].ToString().Trim() == "UnScreen")
                                    //{
                                    //    lblStatus.Text = PanelError.Text = "Mark Screened Successfully";
                                    //}
                                    //else if (Session["CallType"].ToString().Trim() == "Screen")
                                    //{
                                    //    lblStatus.Text = PanelError.Text = "Update Screened Successfully";
                                    //}
                                    //else {
                                    //    lblStatus.Text = PanelError.Text = "Reject Successfully done";
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    lblStatus.Text = ex.Message;
                                    gvUnScreening.DataSource = (DataTable)Session["OldUnscreenGrid"];
                                    gvUnScreening.DataBind();
                                    gdvULDDetails.DataSource = (DataTable)Session["OldScreenGrid"];
                                    gdvULDDetails.DataBind();
                                    Session["UnscreenGrid"] = Session["OldUnscreenGrid"];
                                    Session["ScreenGrid"] = Session["OldScreenGrid"];
                                    lblStatus.Text = PanelError.Text = ex.Message;
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr5", "javascript:putdivval();", true);

                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = ex.Message;
                            gvUnScreening.DataSource = (DataTable)Session["OldUnscreenGrid"];
                            gvUnScreening.DataBind();
                            gdvULDDetails.DataSource = (DataTable)Session["OldScreenGrid"];
                            gdvULDDetails.DataBind();
                            Session["UnscreenGrid"] = Session["OldUnscreenGrid"];
                            Session["ScreenGrid"] = Session["OldScreenGrid"];
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr5", "javascript:putdivval();", true);

                            return;
                        }
                        if (Session["CallType"].ToString().Trim() != "Reject")
                        {
                            dtRight.AcceptChanges();
                            dtLeft.AcceptChanges();
                            gvUnScreening.DataSource = dtLeft;
                            gvUnScreening.DataBind();
                            gdvULDDetails.DataSource = dtRight;
                            gdvULDDetails.DataBind();
                            Session["UnscreenGrid"] = dtLeft;
                            Session["ScreenGrid"] = dtRight;
                            
                        }
                    //Added line after Addition of Acceptance status filter criteria
                        //ddlStatus.Text = "P";
                    //
                        BtnList_Click(sender, e);
                        if (Session["CheckRFIDEnable"].ToString() == "True")
                        {
                            if (Session["CallType"].ToString().Trim() == "UnScreen")
                            {
                                lblStatus.Text = PanelError.Text = "Tag(s) successfully marked as Screened";
                            }
                            else if (Session["CallType"].ToString().Trim() == "Screen")
                            {
                                lblStatus.Text = PanelError.Text = "Screened Tag(s) successfully Updated";
                            }
                            else
                            {
                                lblStatus.Text = PanelError.Text = "Tag(s) Successfully Rejected";
                            }
                        }
                        else {
                            if (Session["CallType"].ToString().Trim() == "UnScreen")
                            {
                                lblStatus.Text = PanelError.Text = "Piece(s) successfully marked as Screened";
                            }
                            else if (Session["CallType"].ToString().Trim() == "Screen")
                            {
                                lblStatus.Text = PanelError.Text = "Screened Piece(s) successfully Updated";
                            }
                            else
                            {
                                lblStatus.Text = PanelError.Text = "Piece(s) Successfully Rejected";
                            }
                        }
                        lblStatus.ForeColor = Color.Green;

                        

                    //Code Cleanup
                        dtLeft.Dispose();
                        dtRight.Dispose();

                }
                else {
                    PanelError.Text = "No AWBNo available for Transfer";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr3", "javascript:putdivval();", true);

                    return;
                }
            }
            catch (Exception ex)
            {
                PanelError.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                return;
            }

          //   tmrRef.Enabled = true;
          //  ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>StartTimer();</script>", false);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr2", "javascript:putdivval();", true);

        }

        protected void btnCallPnl_Click(object sender, EventArgs e)
        {
            
            try
            {
                tmrRef.Enabled = false;
                Session["CallType"] = "UnScreen";
                hdnType.Value = "UnScreen";
                lblStatus.Text = "";
                int count = 0,ScrNtReqCnt=0;
                PanelError.Text = "";
                if (gvUnScreening.Rows.Count > 0)
                {
                    DataTable dtb = new DataTable();
                    dtb.Columns.Add("AWBNumber");
                    dtb.Columns.Add("TagID");
                    dtb.Columns.Add("DestinationCode");
                    dtb.Columns.Add("CCSF");
                    dtb.Columns.Add("fulTagID");
                    dtb.Columns.Add("XRayID");
                    dtb.Columns.Add("XrayTime");
                    dtb.Columns.Add("K9ID");
                    dtb.Columns.Add("K9Time");
                    dtb.Columns.Add("ETDID");
                    dtb.Columns.Add("ETDTime");
                    dtb.Columns.Add("PhysicalID");
                    dtb.Columns.Add("PhysicalTime");
                    dtb.Columns.Add("IsSubTag");
                    dtb.Columns.Add("CountTag");
                    dtb.Columns.Add("XrayCount");
                    dtb.Columns.Add("K9Count");
                    dtb.Columns.Add("ETDCount");
                    dtb.Columns.Add("PhysicalCount");
                    dtb.Columns.Add("TotalCount");
                    dtb.Columns.Add("IsScrReq");
                    dtb.Columns.Add("FltNumber");
                    dtb.Columns.Add("FltDate");
                    dtb.Columns.Add("AcceptedPcs");
                    dtb.Columns.Add("Location");

                    for (int i = 0; i < gvUnScreening.Rows.Count; i++)
                    {
                        if (((CheckBox)gvUnScreening.Rows[i].FindControl("chkUnScreen")).Checked == true)
                        {
                            //if (((Label)gvUnScreening.Rows[i].FindControl("lblReqScr")).Text != "N")
                            //{
                                DataRow dr = dtb.NewRow();
                                dr[0] = ((Label)(gvUnScreening.Rows[i].FindControl("lblAWBno"))).Text.ToString();
                                dr[1] = ((Label)(gvUnScreening.Rows[i].FindControl("lblTagID"))).Text.ToString();
                                dr[2] = ((Label)(gvUnScreening.Rows[i].FindControl("lblDest"))).Text.ToString();
                                dr[3] = ((Label)(gvUnScreening.Rows[i].FindControl("lblCCSF"))).Text.ToString();
                                dr[4] = ((Label)(gvUnScreening.Rows[i].FindControl("lblfulTagID"))).Text.ToString();
                                if (ddlxra.SelectedIndex == 0)
                                { dr[5] = "Select"; }
                                else
                                {
                                    dr[5] = ddlxra.SelectedItem.Text.ToString(); //"Select";
                                }
                                dr[6] = ((DateTime)Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss");
                                dr[7] = "Select";
                                dr[8] = ((DateTime)Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss");
                                dr[9] = "Select";
                                dr[10] = ((DateTime)Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss");
                                dr[11] = "Select";
                                dr[12] = ((DateTime)Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss");
                                dr[13] = ((Label)(gvUnScreening.Rows[i].FindControl("lblSubTag"))).Text.ToString();
                                dr[14] = ((Label)(gvUnScreening.Rows[i].FindControl("lblcntAWBno"))).Text.ToString();
                                dr[15] = "0";
                                dr[16] = "0";
                                dr[17] = "0";
                                dr[18] = "0";
                                dr[19] = "0";
                                dr[20] = ((Label)(gvUnScreening.Rows[i].FindControl("lblReqScr"))).Text.ToString();
                                dr[21] = ((Label)(gvUnScreening.Rows[i].FindControl("lblFlightNo"))).Text.ToString();
                                dr[22] = ((Label)(gvUnScreening.Rows[i].FindControl("lblFlightDate"))).Text.ToString();
                                dr[23] = ((Label)(gvUnScreening.Rows[i].FindControl("lblAcceptedPcs"))).Text.ToString();
                                dr[24] = ((Label)(gvUnScreening.Rows[i].FindControl("txtLocation"))).Text.ToString();
                                dtb.Rows.Add(dr);
                                count++;
                            //}
                            
                        }
                    }
                    //if (ScrNtReqCnt > 0 && count == 0)
                    //{
                    //    lblStatus.Text = "Screening Not Required For Selected AWB(s)";
                    //    return;
                    //}
                    if (count > 0)
                    {
                        GVPopUp.DataSource = dtb;
                        GVPopUp.DataBind();
                        GVPopUp.Columns[21].Visible = false;
                        for (int i = 0; i < GVPopUp.Rows.Count; i++)
                        {
                            //if (((Label)GVPopUp.Rows[i].FindControl("lblscrrq")).Text == "N")
                            //{
                               // string totpcs=((Label)GVPopUp.Rows[i].FindControl("lblPopcntAWBno")).Text;
                               // //GVPopUp.Rows[i].Enabled = false;
                               //((TextBox)GVPopUp.Rows[i].FindControl("lblPopTotScan")).Text=totpcs;
                               //((TextBox)GVPopUp.Rows[i].FindControl("lblPopXrayCount")).Text = totpcs;
                               ((DropDownList)GVPopUp.Rows[i].FindControl("lblPopXRayID")).SelectedIndex = 1;
                            //}
                        }
                        if (Session["CheckRFIDEnable"].ToString() == "True")
                        {
                            GVPopUp.Columns[8].Visible = true;
                            GVPopUp.Columns[10].Visible = true;
                            GVPopUp.Columns[11].Visible = true;
                            GVPopUp.Columns[13].Visible = true;
                            GVPopUp.Columns[14].Visible = true;
                            GVPopUp.Columns[16].Visible = true;
                            GVPopUp.Columns[17].Visible = true;
                            GVPopUp.Columns[19].Visible = true;
                        }
                        else
                        {
                            GVPopUp.Columns[6].Visible = true;
                            GVPopUp.Columns[8].Visible = true;
                            GVPopUp.Columns[9].Visible = true;
                            GVPopUp.Columns[11].Visible = true;
                            GVPopUp.Columns[12].Visible = true;
                            GVPopUp.Columns[14].Visible = true;
                            GVPopUp.Columns[15].Visible = true;
                            GVPopUp.Columns[17].Visible = true;
                            GVPopUp.Columns[18].Visible = true;

                        }
                        GVPopUp.Columns[5].HeaderText = "Total Scaned";

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                    }
                    else
                    {
                        if (Session["CheckRFIDEnable"].ToString() == "True")
                        {
                            lblStatus.Text = "Select Tag(s) to be marked as Screened";
                        }
                        else {
                            lblStatus.Text = "Select Piece(s) to be marked as Screened";
                        }
                       // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>StartTimer();</script>", false);
                    }

                    //Code for Cleanup
                    dtb.Dispose();
                }
                else
                {
                    if (Session["CheckRFIDEnable"].ToString() == "True")
                    {
                        lblStatus.Text = "No Un-Screened Tag available to be marked as Screened";
                    }
                    else {
                        lblStatus.Text = "No Un-Screened Piece available to be marked as Screened";
                    }
                    tmrRef.Enabled = true;
                   // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>StartTimer();</script>", false);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
            finally {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr6", "javascript:putdivval();", true);
            }
        }

        //protected void btnReject_Click(object sender, EventArgs e)
        //{
        //    if (gvUnScreening.Rows.Count > 0)
        //    {
        //        Session["CallType"] = "Reject";
        //        try
        //        {
        //           // tmrRef.Enabled = false;

        //            DataTable dtLeft = (DataTable)Session["UnscreenGrid"];
        //            Session["OldUnscreenGrid"] = Session["UnscreenGrid"];
        //            int chk = 0;
        //            using (SqlConnection cnReject = new SqlConnection(Global.GetConnectionString()))
        //            {
        //                cnReject.Open();
        //                SqlTransaction sqlTran = cnReject.BeginTransaction();
        //                SqlCommand command = cnReject.CreateCommand();
        //                command.Transaction = sqlTran;
        //                try
        //                {
        //                    if (gvUnScreening.Rows.Count > 0)
        //                    {
        //                        for (int i = 0; i < gvUnScreening.Rows.Count; i++)
        //                        {
        //                            if (((CheckBox)gvUnScreening.Rows[i].FindControl("chkUnScreen")).Checked == true)
        //                            {
        //                                chk = 1;
        //                                string awbno = ((Label)(gvUnScreening.Rows[i].FindControl("lblAWBno"))).Text.ToString();
        //                                string tagid = ((Label)(gvUnScreening.Rows[i].FindControl("lblfulTagID"))).Text.ToString();
        //                                string dest = ((Label)(gvUnScreening.Rows[i].FindControl("lblDest"))).Text.ToString();
        //                                string ccsf = ((Label)(gvUnScreening.Rows[i].FindControl("lblCCSF"))).Text.ToString();
        //                                string isSubTag = ((Label)(gvUnScreening.Rows[i].FindControl("lblSubTag"))).Text.ToString();

        //                                bool res = sb.RejectTag(lblDepAirport.Text.Trim(), awbno, tagid, Session["Username"].ToString(), ((DateTime)Session["IT"]).ToString(), ccsf, isSubTag);
        //                                if (res == false)
        //                                {
        //                                    Session["UnscreenGrid"] = Session["OldUnscreenGrid"];
        //                                    gvUnScreening.DataSource = (DataTable)Session["OldUnscreenGrid"];
        //                                    gvUnScreening.DataBind();
        //                                    sqlTran.Rollback();
        //                                    lblStatus.Text = "Error during Reject, AWBNo: " + awbno;
        //                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
        //                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr7", "javascript:putdivval();", true);

        //                                    return;
        //                                }
        //                                else
        //                                {
        //                                    dtLeft.Rows[i].Delete();
        //                                }
        //                            }
        //                        }
        //                        if (chk == 1)
        //                        {
        //                            dtLeft.AcceptChanges();
        //                            Session["UnscreenGrid"] = dtLeft;
        //                            gvUnScreening.DataSource = dtLeft;
        //                            gvUnScreening.DataBind();
        //                            sqlTran.Commit();
        //                           // BtnList_Click(sender, e);
        //                            lblStatus.Text = "Tag Reject Successfully";
        //                           // tmrRef.Enabled = true;
        //                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr8", "javascript:putdivval();", true);

        //                            return;
        //                        }
        //                        else {
        //                            lblStatus.Text = "Select Tags for Reject";
        //                        }

        //                    }
        //                    else
        //                    {
        //                        lblStatus.Text = "No TagID available for Reject";
        //                    }
        //                  //  ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        //                }
        //                catch (Exception ex)
        //                {
        //                    lblStatus.Text = ex.Message;
        //                    Session["UnscreenGrid"] = Session["OldUnscreenGrid"];
        //                    gvUnScreening.DataSource = (DataTable)Session["OldUnscreenGrid"];
        //                    gvUnScreening.DataBind();
        //                    sqlTran.Rollback();
        //                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            lblStatus.Text = ex.Message;
        //        }
        //        finally
        //        {
        //          //  tmrRef.Enabled = true;
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);

        //           // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>StartTimer();</script>", false);
        //        }
        //    }
        //    else {
        //        lblStatus.Text = "No Tags Available for reject";
        //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

        //    }
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr9", "javascript:putdivval();", true);

        //}

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvUnScreening.Rows.Count > 0)
                {
                    Session["CallType"] = "Reject";
                    hdnType.Value = "Reject";

                    lblStatus.Text = "";
                    int count = 0;
                    PanelError.Text = "";
                    try
                    {
                        DataTable dtb = new DataTable();
                        dtb.Columns.Add("AWBNumber");
                        dtb.Columns.Add("TagID");
                        dtb.Columns.Add("DestinationCode");
                        dtb.Columns.Add("CCSF");
                        dtb.Columns.Add("fulTagID");
                        dtb.Columns.Add("XRayID");
                        dtb.Columns.Add("XrayTime");
                        dtb.Columns.Add("K9ID");
                        dtb.Columns.Add("K9Time");
                        dtb.Columns.Add("ETDID");
                        dtb.Columns.Add("ETDTime");
                        dtb.Columns.Add("PhysicalID");
                        dtb.Columns.Add("PhysicalTime");
                        dtb.Columns.Add("IsSubTag");
                        dtb.Columns.Add("CountTag");
                        dtb.Columns.Add("XrayCount");
                        dtb.Columns.Add("K9Count");
                        dtb.Columns.Add("ETDCount");
                        dtb.Columns.Add("PhysicalCount");
                        dtb.Columns.Add("TotalCount");
                        dtb.Columns.Add("IsScrReq");
                        dtb.Columns.Add("FltNumber");
                        dtb.Columns.Add("FltDate");
                        dtb.Columns.Add("AcceptedPcs");
                        dtb.Columns.Add("Location");

                        for (int i = 0; i < gvUnScreening.Rows.Count; i++)
                        {
                            if (((CheckBox)gvUnScreening.Rows[i].FindControl("chkUnScreen")).Checked == true)
                            {
                                DataRow dr = dtb.NewRow();
                                dr[0] = ((Label)(gvUnScreening.Rows[i].FindControl("lblAWBno"))).Text.ToString();
                                dr[1] = ((Label)(gvUnScreening.Rows[i].FindControl("lblTagID"))).Text.ToString();
                                dr[2] = ((Label)(gvUnScreening.Rows[i].FindControl("lblDest"))).Text.ToString();
                                dr[3] = ((Label)(gvUnScreening.Rows[i].FindControl("lblCCSF"))).Text.ToString();
                                dr[4] = ((Label)(gvUnScreening.Rows[i].FindControl("lblfulTagID"))).Text.ToString();
                                dr[5] = "Select";
                                dr[6] = ((DateTime)Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss");
                                dr[7] = "Select";
                                dr[8] = ((DateTime)Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss");
                                dr[9] = "Select";
                                dr[10] = ((DateTime)Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss");
                                dr[11] = "Select";
                                dr[12] = ((DateTime)Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss");
                                dr[13] = ((Label)(gvUnScreening.Rows[i].FindControl("lblSubTag"))).Text.ToString();
                                dr[14] = ((Label)(gvUnScreening.Rows[i].FindControl("lblcntAWBno"))).Text.ToString();
                                dr[15] = "0";
                                dr[16] = "0";
                                dr[17] = "0";
                                dr[18] = "0";
                                dr[19] = "0";
                                dr[20] = ((Label)(gvUnScreening.Rows[i].FindControl("lblReqScr"))).Text.ToString();
                                dr[21] = ((Label)(gvUnScreening.Rows[i].FindControl("lblFlightNo"))).Text.ToString();
                                dr[22] = ((Label)(gvUnScreening.Rows[i].FindControl("lblFlightDate"))).Text.ToString();
                                dr[23] = ((Label)(gvUnScreening.Rows[i].FindControl("lblAcceptedPcs"))).Text.ToString();
                                dr[24] = ((Label)(gvUnScreening.Rows[i].FindControl("txtLocation"))).Text.ToString();
                                dtb.Rows.Add(dr);
                                count++;
                            }
                        }
                        if (count > 0)
                        {
                            GVPopUp.DataSource = dtb;
                            GVPopUp.DataBind();
                            GVPopUp.Columns[21].Visible = true;
                            //if (Session["CheckRFIDEnable"].ToString() == "True")
                            //{
                            //    GVPopUp.Columns[7].Visible = false;
                            //    GVPopUp.Columns[9].Visible = false;
                            //    GVPopUp.Columns[10].Visible = false;
                            //    GVPopUp.Columns[12].Visible = false;
                            //    GVPopUp.Columns[13].Visible = false;
                            //    GVPopUp.Columns[15].Visible = false;
                            //    GVPopUp.Columns[16].Visible = false;
                            //    GVPopUp.Columns[18].Visible = false;
                            //}
                            //else
                            //{
                            //    //  GVPopUp.Columns[5].Visible = false;
                            //    GVPopUp.Columns[7].Visible = false;
                            //    GVPopUp.Columns[8].Visible = false;
                            //    GVPopUp.Columns[10].Visible = false;
                            //    GVPopUp.Columns[11].Visible = false;
                            //    GVPopUp.Columns[13].Visible = false;
                            //    GVPopUp.Columns[14].Visible = false;
                            //    GVPopUp.Columns[16].Visible = false;
                            //    GVPopUp.Columns[17].Visible = false;

                            //}
                            if (Session["CheckRFIDEnable"].ToString() == "True")
                            {
                                GVPopUp.Columns[8].Visible = true;
                                GVPopUp.Columns[10].Visible = true;
                                GVPopUp.Columns[11].Visible = true;
                                GVPopUp.Columns[13].Visible = true;
                                GVPopUp.Columns[14].Visible = true;
                                GVPopUp.Columns[16].Visible = true;
                                GVPopUp.Columns[17].Visible = true;
                                GVPopUp.Columns[19].Visible = true;
                            }
                            else
                            {
                                GVPopUp.Columns[6].Visible = true;
                                GVPopUp.Columns[8].Visible = true;
                                GVPopUp.Columns[9].Visible = true;
                                GVPopUp.Columns[11].Visible = true;
                                GVPopUp.Columns[12].Visible = true;
                                GVPopUp.Columns[14].Visible = true;
                                GVPopUp.Columns[15].Visible = true;
                                GVPopUp.Columns[17].Visible = true;
                                GVPopUp.Columns[18].Visible = true;

                            }
                            GVPopUp.Columns[5].HeaderText = "Reject Pieces";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                        }
                        else
                        {
                            if (Session["CheckRFIDEnable"].ToString() == "True")
                            {
                                lblStatus.Text = "Select Tag(s) to be marked as Rejected";
                            }
                            else
                            {
                                lblStatus.Text = "Select Piece(s) to be marked as Rejected";
                            }
                            tmrRef.Enabled = true;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>StartTimer();</script>", false);
                        }

                        //Code for Cleanup
                        dtb.Dispose();
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = ex.Message;
                    }
                    finally
                    {
                    }
                }
                else
                {
                    if (Session["CheckRFIDEnable"].ToString() == "True")
                    {
                        lblStatus.Text = "No Tag available for Rejection";
                    }
                    else
                    {
                        lblStatus.Text = "No Piece available for Rejection";
                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr9", "javascript:putdivval();", true);
            }
            catch (Exception ex)
            { }
        }

        protected void btnScreenNotReq_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["CheckRFIDEnable"].ToString() == "True")
                {
                    if (gvUnScreening.Rows.Count > 0)
                    {
                        int chk = 0;
                        DataTable dtLeft = (DataTable)Session["UnscreenGrid"];
                        DataTable dtRight = (DataTable)Session["ScreenGrid"];
                        Session["OldUnscreenGrid"] = Session["UnscreenGrid"];
                        Session["OldScreenGrid"] = Session["ScreenGrid"];
                        for (int i = 0; i < gvUnScreening.Rows.Count; i++)
                        {
                            string AWBNo = ((Label)gvUnScreening.Rows[i].FindControl("lblAWBno")).Text.Trim();
                            string ccsf = (((Label)gvUnScreening.Rows[i].FindControl("lblCCSF")).Text.Trim());
                            if (((CheckBox)gvUnScreening.Rows[i].FindControl("chkUnScreen")).Checked == true)
                            {
                                if (ccsf.ToUpper() == Convert.ToString("false").ToUpper())
                                {
                                    lblStatus.Text = "Not Allowed for Non CCSF shipments, RowNo : " + (i + 1).ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr10", "javascript:putdivval();", true);
                                    return;
                                }
                            }
                        }

                        try
                        {
                            using (SqlConnection cnReject = new SqlConnection(Global.GetConnectionString()))
                            {
                                cnReject.Open();
                                SqlTransaction sqlTran = cnReject.BeginTransaction();
                                SqlCommand command = cnReject.CreateCommand();
                                command.Transaction = sqlTran;

                                try
                                {
                                    for (int i = 0; i < gvUnScreening.Rows.Count; i++)
                                    {
                                        if (((CheckBox)gvUnScreening.Rows[i].FindControl("chkUnScreen")).Checked == true)
                                        {
                                            chk = 1;
                                            string AWBNo = ((Label)gvUnScreening.Rows[i].FindControl("lblAWBno")).Text.Trim();
                                            string tagID = (((Label)gvUnScreening.Rows[i].FindControl("lblTagID")).Text.Trim());
                                            string Dest = (((Label)gvUnScreening.Rows[i].FindControl("lblDest")).Text.Trim());
                                            string ccsf = (((Label)gvUnScreening.Rows[i].FindControl("lblCCSF")).Text.Trim());
                                            string FlightNo = (((Label)gvUnScreening.Rows[i].FindControl("lblFlightNo")).Text.Trim());
                                            string FlightDate = (((Label)gvUnScreening.Rows[i].FindControl("lblFlightDate")).Text.Trim());
                                            string Location = (((Label)gvUnScreening.Rows[i].FindControl("txtLocation")).Text.Trim());
                                            string xray = string.Empty;
                                            if (xray == "Select")
                                            {
                                                xray = "";
                                            }
                                            string xrayTime = "1900/01/01";
                                            string k9 = string.Empty;
                                            if (k9 == "Select")
                                            {
                                                k9 = "";
                                            }
                                            string k9Time = "1900/01/01";
                                            string etd = string.Empty;
                                            if (etd == "Select")
                                            {
                                                etd = "";
                                            }
                                            string etdTime = "1900/01/01";
                                            string physical = string.Empty;
                                            if (physical == "Select")
                                            {
                                                physical = "";
                                            }
                                            string physicalTime = "1900/01/01";
                                            string isSubTag = (((Label)gvUnScreening.Rows[i].FindControl("lblSubTag")).Text.Trim());
                                            string location = lblDepAirport.Text.Trim();
                                            string fulltag = (((Label)gvUnScreening.Rows[i].FindControl("lblfulTagID")).Text.Trim());
                                            string rem = (((Label)gvUnScreening.Rows[i].FindControl("lblremark")).Text.Trim());
                                            string addrem = (((Label)gvUnScreening.Rows[i].FindControl("lbladdremark")).Text.Trim());
                                            string exception = (((Label)gvUnScreening.Rows[i].FindControl("lblScrExcep")).Text.Trim());
                                            bool res = sb.SaveTag(location, AWBNo, fulltag, Dest, ccsf, xray, xrayTime, k9, k9Time, etd, etdTime, physical, physicalTime, isSubTag, Session["Username"].ToString(), ((DateTime)Session["IT"]).ToString(), "0", "0", "0", "0", "0", Session["CheckRFIDEnable"].ToString(), rem, addrem, FlightNo, FlightDate,Location,exception);
                                            if (res == false)
                                            {
                                                Session["ScreenGrid"] = Session["OldScreenGrid"];
                                                Session["UnscreenGrid"] = Session["OldUnscreenGrid"];
                                                gdvULDDetails.DataSource = (DataTable)Session["OldScreenGrid"];
                                                gdvULDDetails.DataBind();
                                                gvUnScreening.DataSource = (DataTable)Session["OldUnscreenGrid"];
                                                gvUnScreening.DataBind();
                                                sqlTran.Rollback();
                                                lblStatus.Text = "Error during marking tag(s) as Screened, TagID: " + tagID;
                                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr10", "javascript:putdivval();", true);
                                                return;
                                            }
                                            else
                                            {
                                                //        dtLeft.Rows[i].Delete();
                                            }
                                        }
                                    }
                                    if (chk == 1)
                                    {
                                        dtRight.AcceptChanges();
                                        dtLeft.AcceptChanges();
                                        gvUnScreening.DataSource = dtLeft;
                                        gvUnScreening.DataBind();
                                        gdvULDDetails.DataSource = dtRight;
                                        gdvULDDetails.DataBind();
                                        Session["UnscreenGrid"] = dtLeft;
                                        Session["ScreenGrid"] = dtRight;
                                        sqlTran.Commit();
                                        //Code for Cleanup
                                        dtLeft.Dispose();
                                        dtRight.Dispose();

                                        BtnList_Click(sender, e);
                                        lblStatus.Text = "Tag(s) Successfully marked as Screened";
                                        lblStatus.ForeColor = Color.Green;
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr10", "javascript:putdivval();", true);
                                        return;
                                    }
                                    else
                                    {
                                        lblStatus.Text = "Select Tag(s) to be marked as Screened";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Session["ScreenGrid"] = Session["OldScreenGrid"];
                                    Session["UnscreenGrid"] = Session["OldUnscreenGrid"];
                                    gdvULDDetails.DataSource = (DataTable)Session["OldScreenGrid"];
                                    gdvULDDetails.DataBind();
                                    gvUnScreening.DataSource = (DataTable)Session["OldUnscreenGrid"];
                                    gvUnScreening.DataBind();
                                    sqlTran.Rollback();
                                    lblStatus.Text = ex.Message;
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr10", "javascript:putdivval();", true);
                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = ex.Message;
                        }
                        finally
                        {
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>StartTimer();</script>", false);
                        }
                    }
                    else
                    {
                        lblStatus.Text = "No Tag available for marking as Screened";
                        // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                    }
                }
                else
                {
                    lblStatus.Text = "This Process is not Valid";
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr10", "javascript:putdivval();", true);
            }
            catch (Exception ex)
            { }
        }

        protected void btnUnassign_Click(object sender, EventArgs e)
        {
            try
            {
                if (gdvULDDetails.Rows.Count > 0)
                {
                    try
                    {
                        lblStatus.Text = "";
                        int count = 0;
                        PanelError.Text = "";
                        if (gdvULDDetails.Rows.Count > 0)
                        {
                            int chk = 0;
                            DataTable dtLeft = (DataTable)Session["UnscreenGrid"];
                            DataTable dtRight = (DataTable)Session["ScreenGrid"];
                            Session["OldUnscreenGrid"] = Session["UnscreenGrid"];
                            Session["OldScreenGrid"] = Session["ScreenGrid"];

                            try
                            {
                                using (SqlConnection cnReject = new SqlConnection(Global.GetConnectionString()))
                                {
                                    cnReject.Open();
                                    SqlTransaction sqlTran = cnReject.BeginTransaction();
                                    SqlCommand command = cnReject.CreateCommand();
                                    command.Transaction = sqlTran;
                                    try
                                    {
                                        for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                                        {
                                            string awbno = ((Label)gdvULDDetails.Rows[i].FindControl("lblSAWBno")).Text.Trim();
                                            string tag = (((Label)gdvULDDetails.Rows[i].FindControl("lblSTagID")).Text.Trim());
                                            string dest = (((Label)gdvULDDetails.Rows[i].FindControl("lblSDest")).Text.Trim());
                                            string ccsf = (((Label)gdvULDDetails.Rows[i].FindControl("lblSCCSF")).Text.Trim());
                                            string fultag = (((Label)gdvULDDetails.Rows[i].FindControl("lblSFullTagID")).Text.Trim());
                                            string subtag = (((Label)gdvULDDetails.Rows[i].FindControl("lblSSubTag")).Text.Trim());
                                            string FlightNo = (((Label)gdvULDDetails.Rows[i].FindControl("lblSfltno")).Text.Trim());
                                            string FlightDate = (((Label)gdvULDDetails.Rows[i].FindControl("lblSfltdt")).Text.Trim());
                                           
                                            if (((CheckBox)gdvULDDetails.Rows[i].FindControl("chkScreen")).Checked == true)
                                            {
                                                chk = 1;

                                                #region save
                                                bool res = sb.MarkUnscreenOld(((Label)gdvULDDetails.Rows[i].FindControl("lblSAWBno")).Text.Trim(), (((Label)gdvULDDetails.Rows[i].FindControl("lblSFullTagID")).Text.Trim()), (((Label)gdvULDDetails.Rows[i].FindControl("lblSDest")).Text.Trim()), (((Label)gdvULDDetails.Rows[i].FindControl("lblSCCSF")).Text.Trim()), (((Label)gdvULDDetails.Rows[i].FindControl("lblSSubTag")).Text.Trim()), lblDepAirport.Text, Session["CheckRFIDEnable"].ToString(), FlightNo, FlightDate);
                                                if (res == false)
                                                {
                                                    Session["UnscreenGrid"] = Session["OldUnscreenGrid"];
                                                    Session["ScreenGrid"] = Session["OldScreenGrid"];
                                                    gvUnScreening.DataSource = (DataTable)Session["OldUnscreenGrid"];
                                                    gvUnScreening.DataBind();
                                                    gdvULDDetails.DataSource = (DataTable)Session["OldScreenGrid"];
                                                    gdvULDDetails.DataBind();
                                                    sqlTran.Rollback();
                                                    lblStatus.Text = "Error during Mark as UnScreened, AWBNo : " + awbno;
                                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr10", "javascript:putdivval();", true);

                                                    return;
                                                }
                                                #endregion

                                                dtLeft.Rows.Add(awbno, dest, ccsf, tag, subtag, fultag);
                                                for (int k = 0; k < dtRight.Rows.Count; k++)
                                                {
                                                    try
                                                    {
                                                        if (dtRight.Rows[k][0].ToString() == awbno && dtRight.Rows[k][11].ToString() == tag && dtRight.Rows[k][13].ToString() == fultag)
                                                        {
                                                            dtRight.Rows[k].Delete();
                                                            dtRight.AcceptChanges();
                                                            break;
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                            }
                                        }
                                        if (chk == 1)
                                        {
                                            sqlTran.Commit();
                                            dtRight.AcceptChanges();
                                            dtLeft.AcceptChanges();
                                            gvUnScreening.DataSource = dtLeft;
                                            gvUnScreening.DataBind();
                                            gdvULDDetails.DataSource = dtRight;
                                            gdvULDDetails.DataBind();
                                            Session["UnscreenGrid"] = dtLeft;
                                            Session["ScreenGrid"] = dtRight;

                                            //Code for Cleanup
                                            dtLeft.Dispose();
                                            dtRight.Dispose();

                                            BtnList_Click(sender, e);
                                            if (Session["CheckRFIDEnable"].ToString() == "True")
                                            {
                                                lblStatus.Text = "Tag(s) Successfully marked as Un-Screened";
                                            }
                                            else
                                            {
                                                lblStatus.Text = "Piece(s) Successfully marked as Un-Screened";
                                            }
                                            lblStatus.ForeColor = Color.Green;
                                        }
                                        else
                                        {
                                            if (Session["CheckRFIDEnable"].ToString() == "True")
                                            {
                                                lblStatus.Text = "Select Tag(s) for marking as Un-Screened";
                                            }
                                            else
                                            {
                                                lblStatus.Text = "Select Piece(s) for marking as Un-Screened";
                                            }
                                        }
                                        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);

                                    }
                                    catch (Exception ex)
                                    {
                                        Session["UnscreenGrid"] = Session["OldUnscreenGrid"];
                                        Session["ScreenGrid"] = Session["OldScreenGrid"];
                                        gvUnScreening.DataSource = (DataTable)Session["OldUnscreenGrid"];
                                        gvUnScreening.DataBind();
                                        gdvULDDetails.DataSource = (DataTable)Session["OldScreenGrid"];
                                        gdvULDDetails.DataBind();
                                        sqlTran.Rollback();
                                        lblStatus.Text = ex.Message;
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);

                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr11", "javascript:putdivval();", true);


                                        return;
                                    }
                                }
                                //dtRight.AcceptChanges();
                                //dtLeft.AcceptChanges();
                                //gvUnScreening.DataSource = dtLeft;
                                //gvUnScreening.DataBind();
                                //gdvULDDetails.DataSource = dtRight;
                                //gdvULDDetails.DataBind();
                                //Session["UnscreenGrid"] = dtLeft;
                                //Session["ScreenGrid"] = dtRight;
                            }
                            catch (Exception ex)
                            {
                                lblStatus.Text = ex.Message;
                                // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = ex.Message;
                    }
                    finally
                    {
                        //tmrRef.Enabled = true;
                        // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>StartTimer();</script>", false);
                    }
                }
                else
                {
                    if (Session["CheckRFIDEnable"].ToString() == "True")
                    {
                        lblStatus.Text = "No Tag available for marking as Un-Screened";
                    }
                    else
                    {
                        lblStatus.Text = "No Piece available for marking as Un-Screened";
                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr12", "javascript:putdivval();", true);
            }
            catch (Exception ex)
            { }
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                tmrRef.Enabled = false;
                Session["UnscreenGrid"] = null;
                Session["ScreenGrid"] = null;
                Session["OldUnscreenGrid"] = null;
                Session["OldScreenGrid"] = null;
                Session["CallType"] = null;
                Session["CHECKEDU_ITEMS"] = null;
                Session["CHECKEDS_ITEMS"] = null;
                Session["CHECKEDW_ITEMS"] = null;
                gvUnScreening.DataSource = null; gvUnScreening.DataBind();
                GVPopUp.DataSource = null; GVPopUp.DataBind();
                gdvULDDetails.DataSource = null; gdvULDDetails.DataBind();
                ddlDuartion.SelectedIndex = 0;
                if (Session["AirlinePrefix"] == null)
                    ddlFlPre.Text = Session["AirlinePrefix"].ToString();
                if (Session["awbPrefix"] != null)
                    ddlListPre.Text = Session["awbPrefix"].ToString();
                txtAWBSno.Text = "";
                txtFlightID.Text = "";
                txtSDest.Text = "";
                TextBoxdate.Text = "";
                ddlxra.SelectedIndex = 0;
                lblCountUnT.Text = "";
                lblSCntTag.Text = "";
                btnCallPnl.Visible = false;
                btnReject.Visible = false;
                btnUnassign.Visible = false;
                btnUpdateScreen.Visible = false;
                btnScreenNotReq.Visible = false;
                btnSendFSU.Visible = false;
                btnPrintMFT.Visible = false;
                btnPrintLbl.Visible =btnReturn.Visible= false;
                //Response.Redirect("~/"
                
            }
            catch (Exception ex)
            {
                lblParaStatus.Text = ex.Message;
            }
            finally {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            
            }
        }

        protected void btnUpdateScreen_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                tmrRef.Enabled = false;
                Session["CallType"] = "Screen";
                hdnType.Value = "Screen";

                lblStatus.Text = "";
                int count = 0;
                PanelError.Text = "";
                if (gdvULDDetails.Rows.Count > 0)
                {
                    DataTable dtb = new DataTable();
                    dtb.Columns.Add("AWBNumber");
                    dtb.Columns.Add("TagID");
                    dtb.Columns.Add("DestinationCode");
                    dtb.Columns.Add("CCSF");
                    dtb.Columns.Add("fulTagID");
                    dtb.Columns.Add("XRayID");
                    dtb.Columns.Add("XrayTime");
                    dtb.Columns.Add("K9ID");
                    dtb.Columns.Add("K9Time");
                    dtb.Columns.Add("ETDID");
                    dtb.Columns.Add("ETDTime");
                    dtb.Columns.Add("PhysicalID");
                    dtb.Columns.Add("PhysicalTime");
                    dtb.Columns.Add("IsSubTag");
                    dtb.Columns.Add("CountTag");
                    dtb.Columns.Add("XrayCount");
                    dtb.Columns.Add("K9Count");
                    dtb.Columns.Add("ETDCount");
                    dtb.Columns.Add("PhysicalCount");
                    dtb.Columns.Add("TotalCount");
                    dtb.Columns.Add("Remark"); //New Row
                    dtb.Columns.Add("AddRemark"); //New Row
                    dtb.Columns.Add("IsScrReq");
                    dtb.Columns.Add("FltNumber");
                    dtb.Columns.Add("FltDate");
                    dtb.Columns.Add("AcceptedPcs");
                    dtb.Columns.Add("Location");
                    dtb.Columns.Add("ScrException");
                    for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                    {
                        if (((CheckBox)gdvULDDetails.Rows[i].FindControl("chkScreen")).Checked == true)
                        {
                            DataRow dr = dtb.NewRow();
                            dr[0] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSAWBno"))).Text.ToString();
                            dr[1] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSTagID"))).Text.ToString();
                            dr[2] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSDest"))).Text.ToString();
                            dr[3] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSCCSF"))).Text.ToString();
                            dr[4] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSFullTagID"))).Text.ToString();
                            if (((Label)(gdvULDDetails.Rows[i].FindControl("lblSXRayID"))).Text.ToString().Trim() == "")
                            {
                                dr[5] = "Select";
                                dr[6] = ((DateTime)Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss");
                            }
                            else
                            {
                                dr[5] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSXRayID"))).Text.ToString();
                                dr[6] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSXRayTime"))).Text.ToString();
                            }
                            if (((Label)(gdvULDDetails.Rows[i].FindControl("lblSCanineID"))).Text.ToString().Trim() == "")
                            {
                                dr[7] = "Select";
                                dr[8] = ((DateTime)Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss");
                            }
                            else
                            {
                                dr[7] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSCanineID"))).Text.ToString();
                                dr[8] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSK9Time"))).Text.ToString();
                            }
                            if (((Label)(gdvULDDetails.Rows[i].FindControl("lblSETDID"))).Text.ToString().Trim() == "")
                            {
                                dr[9] = "Select";
                                dr[10] = ((DateTime)Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss");
                            }
                            else
                            {
                                dr[9] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSETDID"))).Text.ToString();
                                dr[10] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSETDTime"))).Text.ToString();
                            }
                            if (((Label)(gdvULDDetails.Rows[i].FindControl("lblSPhysical"))).Text.ToString().Trim() == "")
                            {
                                dr[11] = "Select";
                                dr[12] = ((DateTime)Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss");
                            }
                            else
                            {
                                dr[11] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSPhysical"))).Text.ToString();
                                dr[12] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSPhysicalTime"))).Text.ToString();
                            }
                            dr[13] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSSubTag"))).Text.ToString();
                            dr[14] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblScntAWBno"))).Text.ToString();
                            dr[15] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSXrayCount"))).Text.ToString();
                            dr[16] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSK9Count"))).Text.ToString();
                            dr[17] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSETDCount"))).Text.ToString();
                            dr[18] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSPhysicalCount"))).Text.ToString();
                            dr[19] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSTotScan"))).Text.ToString();
                            dr[20] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSRemark"))).Text.ToString();
                            dr[21] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSaddremark"))).Text.ToString();
                            dr[22] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSScrReq"))).Text.ToString();
                            dr[23] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSfltno"))).Text.ToString();
                            dr[24] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSfltdt"))).Text.ToString();
                            dr[25] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblAcceptedPcs"))).Text.ToString();
                            dr[26] = ((Label)(gdvULDDetails.Rows[i].FindControl("txtLocation"))).Text.ToString();
                            dr[27] = ((Label)(gdvULDDetails.Rows[i].FindControl("lblScrExcep"))).Text.ToString();
                            dtb.Rows.Add(dr);
                            count++;
                            
                        }
                    }
                    if (count > 0)
                    {
                        GVPopUp.DataSource = dtb;
                        GVPopUp.DataBind();
                        GVPopUp.Columns[21].Visible = false;
                        

                        //if (Session["CheckRFIDEnable"].ToString() == "True")
                        //{
                        //    GVPopUp.Columns[7].Visible = true;
                        //    GVPopUp.Columns[9].Visible = true;
                        //    GVPopUp.Columns[10].Visible = true;
                        //    GVPopUp.Columns[12].Visible = true;
                        //    GVPopUp.Columns[13].Visible = true;
                        //    GVPopUp.Columns[15].Visible = true;
                        //    GVPopUp.Columns[16].Visible = true;
                        //    GVPopUp.Columns[18].Visible = true;
                        //    //GVPopUp.Columns[].Visible = true;
                        //    //GVPopUp.Columns[].Visible = true;
                        //}
                        //else
                        //{
                        //    GVPopUp.Columns[5].Visible = true;
                        //    GVPopUp.Columns[7].Visible = true;
                        //    GVPopUp.Columns[8].Visible = true;
                        //    GVPopUp.Columns[10].Visible = true;
                        //    GVPopUp.Columns[11].Visible = true;
                        //    GVPopUp.Columns[13].Visible = true;
                        //    GVPopUp.Columns[14].Visible = true;
                        //    GVPopUp.Columns[16].Visible = true;
                        //    GVPopUp.Columns[17].Visible = true;

                        //}

                        if (Session["CheckRFIDEnable"].ToString() == "True")
                        {
                            GVPopUp.Columns[8].Visible = true;
                            GVPopUp.Columns[10].Visible = true;
                            GVPopUp.Columns[11].Visible = true;
                            GVPopUp.Columns[13].Visible = true;
                            GVPopUp.Columns[14].Visible = true;
                            GVPopUp.Columns[16].Visible = true;
                            GVPopUp.Columns[17].Visible = true;
                            GVPopUp.Columns[19].Visible = true;
                        }
                        else
                        {
                            GVPopUp.Columns[6].Visible = true;
                            GVPopUp.Columns[8].Visible = true;
                            GVPopUp.Columns[9].Visible = true;
                            GVPopUp.Columns[11].Visible = true;
                            GVPopUp.Columns[12].Visible = true;
                            GVPopUp.Columns[14].Visible = true;
                            GVPopUp.Columns[15].Visible = true;
                            GVPopUp.Columns[17].Visible = true;
                            GVPopUp.Columns[18].Visible = true;

                        }
                        GVPopUp.Columns[5].HeaderText = "Total Scaned";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
                       
                        //if (GVPopUp.Rows.Count > 0)
                        //{
                        //    for (int i = 0; i < GVPopUp.Rows.Count; i++)
                        //    {
                        //        ((TextBox)(GVPopUp.Rows[i].FindControl("lblPopTotScan"))).Enabled = false;
                        //        ((TextBox)(GVPopUp.Rows[i].FindControl("lblPopXrayCount"))).Enabled = false;
                        //    }
                        //}
                    }
                    else
                    {
                        if (Session["CheckRFIDEnable"].ToString() == "True")
                        {
                            lblStatus.Text = "Select Tag(s) to Update";
                        }
                        else
                        {
                            lblStatus.Text = "Select Piece(s) to Update";
                        }
                        tmrRef.Enabled = true;
                        // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>StartTimer();</script>", false);
                    }
                }
                else
                {
                    if (Session["CheckRFIDEnable"].ToString() == "True")
                    {
                        lblStatus.Text = "No Tag available for Update";
                    }
                    else
                    {
                        lblStatus.Text = "No Piece available for Update";
                    }
                    tmrRef.Enabled = true;
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>StartTimer();</script>", false);
                }
            }
            catch (Exception ex)
            {
            }
            
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr13", "javascript:putdivval();", true);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (gdvULDDetails.Rows.Count > 0)
                {
                    DataTable dtRight = (DataTable)Session["ScreenGrid"];
                    Session["OldScreenGrid"] = Session["ScreenGrid"];
                    int chk = 0;

                    try
                    {
                        using (SqlConnection cnReject = new SqlConnection(Global.GetConnectionString()))
                        {
                            cnReject.Open();
                            SqlTransaction sqlTran = cnReject.BeginTransaction();
                            SqlCommand command = cnReject.CreateCommand();
                            command.Transaction = sqlTran;
                            try
                            {
                                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                                {

                                    string awbno = ((Label)gdvULDDetails.Rows[i].FindControl("lblSAWBno")).Text.Trim();
                                    string tag = (((Label)gdvULDDetails.Rows[i].FindControl("lblSTagID")).Text.Trim());
                                    string dest = (((Label)gdvULDDetails.Rows[i].FindControl("lblSDest")).Text.Trim());
                                    string ccsf = (((Label)gdvULDDetails.Rows[i].FindControl("lblSCCSF")).Text.Trim());
                                    string fultag = (((Label)gdvULDDetails.Rows[i].FindControl("lblSFullTagID")).Text.Trim());
                                    string subtag = (((Label)gdvULDDetails.Rows[i].FindControl("lblSSubTag")).Text.Trim());
                                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("chkScreen")).Checked == true)
                                    {
                                        chk = 1;

                                        bool res = sb.MarkClosed(((Label)gdvULDDetails.Rows[i].FindControl("lblSAWBno")).Text.Trim(), (((Label)gdvULDDetails.Rows[i].FindControl("lblSFullTagID")).Text.Trim()), (((Label)gdvULDDetails.Rows[i].FindControl("lblSDest")).Text.Trim()), (((Label)gdvULDDetails.Rows[i].FindControl("lblSCCSF")).Text.Trim()), (((Label)gdvULDDetails.Rows[i].FindControl("lblSSubTag")).Text.Trim()), lblDepAirport.Text, Session["CheckRFIDEnable"].ToString());
                                        if (res == false)
                                        {
                                            Session["ScreenGrid"] = Session["OldScreenGrid"];
                                            gdvULDDetails.DataSource = (DataTable)Session["OldScreenGrid"];
                                            gdvULDDetails.DataBind();
                                            sqlTran.Rollback();
                                            lblStatus.Text = "Error during Closing, RowNo : " + (i+1);
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr10", "javascript:putdivval();", true);

                                            return;
                                        }

                                    }
                                }
                                if (chk == 1)
                                {
                                    sqlTran.Commit();
                                    dtRight.AcceptChanges();
                                    gdvULDDetails.DataSource = dtRight;
                                    gdvULDDetails.DataBind();
                                    Session["ScreenGrid"] = dtRight;
                                    BtnList_Click(sender, e);
                                    lblStatus.Text = "Closing Done Successfully";
                                    lblStatus.ForeColor = Color.Green;
                                }
                                else
                                {
                                    if (Session["CheckRFIDEnable"].ToString() == "True")
                                    {
                                        lblStatus.Text = "Select Tag(s) for Closing";
                                    }
                                    else {
                                        lblStatus.Text = "Select AWB(s) for Closing";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Session["ScreenGrid"] = Session["OldScreenGrid"];
                                gdvULDDetails.DataSource = (DataTable)Session["OldScreenGrid"];
                                gdvULDDetails.DataBind();
                                sqlTran.Rollback();
                                lblStatus.Text = ex.Message;
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr11", "javascript:putdivval();", true);
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = ex.Message;
                    }
                }
                else
                {
                    if (Session["CheckRFIDEnable"].ToString() == "True")
                    {
                        lblStatus.Text = "No Tag available for Closing";
                    }
                    else {
                        lblStatus.Text = "No AWB available for Closing";
                    
                    }
                   // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message15", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                }
            }
            catch (Exception ex)
            {
                //                Response.Redirect("Home.aspx", false);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message15", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr12", "javascript:putdivval();", true);
            }
        }

        protected void Ignore_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                if (chkWarAWB.Items.Count > 0)
                {
                    for (int i = 0; i < chkWarAWB.Items.Count; i++)
                    {
                        if (chkWarAWB.Items[i].Selected == true)
                        {
                            bool res = sb.IgnoreAWB(lblDepAirport.Text.Trim(), chkWarAWB.Items[i].Text.Trim(), ((DateTime)Session["IT"]).ToString(), Session["CheckRFIDEnable"].ToString());
                            if (res == false)
                            {
                                lblStatus.Text = "Problem occured while ignoring Sequence Broken AWB(s)";
                                return;
                            }
                        }
                    }
                    lblStatus.Text = "AWB(s) Successfully marked as Ignored";
                    lblStatus.ForeColor = Color.Green;
                    BtnList_Click(sender, e);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message19", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr10", "javascript:putdivval();", true);
                    return;
                }
                else
                {
                    lblStatus.Text = "No AWB Available for marking as Ignored";
                }
            
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
            finally {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message19", "<SCRIPT LANGUAGE='javascript'>callclose();StartTimer();</script>", false);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scr10", "javascript:putdivval();", true);
            }
        }
        
        #region Screening Report
        protected void btnPrintMFT_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;
                if (gdvULDDetails.Rows.Count <= 0)
                {
                    lblStatus.Text = "No records available...";
                    lblStatus.ForeColor = Color.Red;
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
                DataTable dt = new DataTable();
                dt.Columns.Add("AWBNumber", typeof(string));
                dt.Columns.Add("CountTag", typeof(string));
                dt.Columns.Add("ScreenStation", typeof(string));
                dt.Columns.Add("FltOrigin", typeof(string));
                dt.Columns.Add("DestinationCode", typeof(string));
                dt.Columns.Add("FltNumber", typeof(string));
                dt.Columns.Add("FltDate", typeof(string));
                dt.Columns.Add("Carrier", typeof(string));
                dt.Columns.Add("AgentCode", typeof(string));
                dt.Columns.Add("GrossWeight", typeof(string));
                //dt.Columns.Add("Volume", typeof(string));
                dt.Columns.Add("UpdatedOn", typeof(string));
                dt.Columns.Add("XrayCount", typeof(string));
                dt.Columns.Add("ETDCount", typeof(string));
                dt.Columns.Add("PhysicalCount", typeof(string));
                dt.Columns.Add("Remark", typeof(string));
                dt.Columns.Add("AdditionalRemark", typeof(string));
                dt.Columns.Add("ReturnedPcs", typeof(string));
                dt.Columns.Add("IsScrReq", typeof(string));
                dt.Columns.Add("AcceptedPcs", typeof(string));
                dt.Columns.Add("Location", typeof(string));
                dt.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                dt.Columns.Add("K9", typeof(string));
                dt.Columns.Add("ScrMethod", typeof(string));
                dt.Columns.Add("CCSFCount", typeof(string));
                dt.Columns.Add("ScrException", typeof(string));
                dt.Columns.Add("Screener", typeof(string));
                dt.Columns.Add("Airport", typeof(string));
                dt.Columns.Add("Date", typeof(string));

                //string strEmail = string.Empty;
                foreach (GridViewRow rw in gdvULDDetails.Rows)
                {
                    CheckBox chkBx = (CheckBox)rw.FindControl("chkScreen");
                    if (chkBx != null && chkBx.Checked)
                    {
                        DataRow dr = dt.NewRow();
                        dr["AWBNumber"] = ((Label)rw.FindControl("lblSAWBno")).Text.ToString();


                        int xray, k9, etd, physical;
                        xray = Convert.ToInt32(((Label)rw.FindControl("lblSXrayCount")).Text);
                        k9 = Convert.ToInt32(((Label)rw.FindControl("lblSK9Count")).Text);
                        physical = Convert.ToInt32(((Label)rw.FindControl("lblSPhysicalCount")).Text);
                        etd = Convert.ToInt32(((Label)rw.FindControl("lblSETDCount")).Text);

                        string ScrMethod = "";
                        string[] method = new string[] { };
                        ArrayList methodList = new ArrayList();
                        methodList.Clear();
                        if (xray > 0)
                            methodList.Add("X-Ray");
                        if (k9 > 0)
                            methodList.Add("Canine");
                        if (etd > 0)
                           methodList.Add("ETD");
                        if (physical > 0)
                            methodList.Add("Physical");

                        if (methodList.Count > 0)
                            ScrMethod = string.Join(",", (string[])methodList.ToArray(Type.GetType("System.String")));
                            


                        if (((Label)rw.FindControl("lblSScrReq")).Text == "N")
                            dr["CountTag"] = "";
                        else
                            dr["CountTag"] = ((Label)rw.FindControl("lblScntAWBno")).Text.ToString();

                        dr["ScreenStation"] = ((Label)rw.FindControl("lblSscrstn")).Text.ToString();
                        dr["FltOrigin"] = ((Label)rw.FindControl("lblSfltorg")).Text.ToString();
                        dr["DestinationCode"] = ((Label)rw.FindControl("lblSDest")).Text.ToString();
                        dr["FltNumber"] = ((Label)rw.FindControl("lblSfltno")).Text.ToString();
                        dr["FltDate"] = ((Label)rw.FindControl("lblSfltdt")).Text.ToString();
                        dr["Carrier"] = ((Label)rw.FindControl("lblScarrier")).Text.ToString();
                        dr["AgentCode"] = ((Label)rw.FindControl("lblSagcode")).Text.ToString();
                        dr["GrossWeight"] = ((Label)rw.FindControl("lblSgrwt")).Text.ToString();
                        //dr["Volume"] = ((Label)rw.FindControl("lblSAWBno")).Text.ToString();
                        dr["UpdatedOn"] = ((Label)rw.FindControl("lblSupdton")).Text.ToString();
                        dr["XrayCount"] = ((Label)rw.FindControl("lblSXrayCount")).Text.ToString();
                        dr["ETDCount"] = ((Label)rw.FindControl("lblSETDCount")).Text.ToString();
                        dr["PhysicalCount"] = ((Label)rw.FindControl("lblSPhysicalCount")).Text.ToString();
                        dr["Remark"] = ((Label)rw.FindControl("lblSRemark")).Text.ToString();
                        dr["AdditionalRemark"] = ((Label)rw.FindControl("lblSaddremark")).Text.ToString();
                        dr["ReturnedPcs"] = ((Label)rw.FindControl("lblSretpcs")).Text.ToString();
                        if (((Label)rw.FindControl("lblSScrReq")).Text == "N")
                            dr["IsScrReq"] = "No";
                        if (((Label)rw.FindControl("lblSScrReq")).Text == "Y")
                            dr["IsScrReq"] = "Yes";
                        dr["AcceptedPcs"] = ((Label)rw.FindControl("lblAcceptedPcs")).Text.ToString();
                        dr["Location"] = ((Label)rw.FindControl("txtLocation")).Text.ToString();
                        dr["Logo"] = Logo.ToArray();
                        dr["K9"] = ((Label)rw.FindControl("lblSK9Count")).Text.ToString();

                        dr["ScrMethod"] = ScrMethod;

                        if (((Label)rw.FindControl("lblSScrReq")).Text == "N")
                            dr["CCSFCount"] = ((Label)rw.FindControl("lblScntAWBno")).Text.ToString();
                        else
                            dr["CCSFCount"] = "0";

                        dr["ScrException"] = ((Label)rw.FindControl("lblScrExcep")).Text.ToString(); ;
                        dr["Screener"] = Session["NameofUser"].ToString();
                        dr["Airport"] = Session["Station"].ToString();
                        dr["Date"] = Session["IT"].ToString();
                        dt.Rows.Add(dr);
                    }
                }
                //DataSet dsnw = new DataSet();
                //dsnw.Tables.Add(dt);
                //Session["dsnew"] = dsnw;
                //Response.Redirect("rptScreening.aspx");
                if (dt.Rows.Count > 0)
                {
                    //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('rptScreening.aspx','', 'left=0,top=0,width=800,height=450,toolbar=0,resizable=0');", true);
                    ReportViewer rptUCRReport = new ReportViewer();
                    ReportDataSource rds1 = new ReportDataSource();
                    LocalReport rep1 = null;

                    rptUCRReport.ProcessingMode = ProcessingMode.Local;

                    rep1 = rptUCRReport.LocalReport;

                    rep1.ReportPath = Server.MapPath("/Reports/rptScreening.rdlc");

                    rds1.Name = "dsScreeningRpt_DataTable1";
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
                        string deviceInfo = "<DeviceInfo><PageHeight>11in</PageHeight><PageWidth>11in</PageWidth></DeviceInfo>";

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
                        byte[] bytes = rptUCRReport.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamIds, out warnings);
                        Response.Buffer = true;
                        Response.Clear();
                        Response.ContentType = mimeType;
                        Response.AddHeader("content-disposition", "attachment; filename=" + "ScreeningRpt" + "." + ".pdf");
                        Response.BinaryWrite(bytes); // create the file
                        Response.Flush();


                        //Response.End();
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion

                    //Code for Cleanup
                    dt.Dispose();
                }
                else
                {
                    //lblStatus.Text = "Kindly select records to Print and try again...";
                    //lblStatus.ForeColor = Color.Red;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message19", "<SCRIPT LANGUAGE='javascript'>alert('Kindly select records to print!');</script>", false);
                    return;
                }
            }
            catch (Exception ex)
            {
                ////lblStatus.Text = "Error while printing report";
                ////lblStatus.ForeColor = Color.Red;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message19", "<SCRIPT LANGUAGE='javascript'>alert('Error while printing report!');</script>", false);
                return;
            }

        }
        #endregion Screening Report

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                DataSet dslbl = new DataSet();
                ArrayList alnew = new ArrayList();
                ArrayList alreason = new ArrayList();
                DataSet dspcid = new DataSet();
                string totalscanpcs = null;

                if (gdvULDDetails.Rows.Count > 0)
                {
                    int count = 0;
                    DataTable dtb = new DataTable();
                    dtb.Columns.Add("Labels", typeof(int));


                    for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                    {
                        if (((CheckBox)gdvULDDetails.Rows[i].FindControl("chkScreen")).Checked == true)
                        {
                            count++;
                            string awbnumber = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSAWBno"))).Text.ToString();

                            if (count == 1)
                            {
                                int RetPieces = 0;
                                int pcscount = int.Parse(((Label)(gdvULDDetails.Rows[i].FindControl("lblScntAWBno"))).Text.ToString());
                                int scancount = int.Parse(((Label)(gdvULDDetails.Rows[i].FindControl("lblSTotScan"))).Text.ToString());
                                try
                                {
                                    RetPieces = int.Parse(((Label)(gdvULDDetails.Rows[i].FindControl("lblSretpcs"))).Text.ToString());
                                }
                                catch (Exception ex) { }
                                lblAWBDisplay.Text = awbnumber;
                                lblAWBPcsDisplay.Text = (scancount - RetPieces).ToString();
                            }
                            if (count > 1)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Select only one row at a time');</script>", false);
                                break;
                            }

                        }

                    }
                    if (count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Select one row');</script>", false);
                    }

                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanellSplit();</script>", false);
            }
            catch (Exception ex)
            { }
        }

        protected void unsOk_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                string FlightNo = string.Empty;
                string FlightDate = string.Empty;
                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("chkScreen")).Checked == true)
                    {
                        //string awbnumber = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSAWBno"))).Text.ToString();
                        FlightNo = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSfltno"))).Text.ToString();
                        FlightDate = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSfltdt"))).Text.ToString();

                    }
                }
                string AWBNumber = lblAWBDisplay.Text.Trim().Substring(4, 8);
                //string AWBPrefix = lblAWBDisplay.Text.Trim().Replace(AWBNumber, "");
                string AWBPrefix = lblAWBDisplay.Text.Trim().Substring(0, 3);

                int AWBPieces = Convert.ToInt32(lblAWBPcsDisplay.Text.Trim());

                string Station = Convert.ToString(Session["Station"]);
                bool IsScreened = true;
                string UserName = Convert.ToString(Session["UserName"]);
                DateTime dtTimeStamp = Convert.ToDateTime(Session["IT"]);

                if (txtReturnPcs.Text.Trim() == "" || txtReturnPcs.Text.Trim() == "0")
                {
                    lblStatus.Text = "Please enter valid Pieces";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (txtReturnWt.Text.Trim() == "" || txtReturnWt.Text.Trim() == "0")
                {
                    lblStatus.Text = "Please enter valid Weight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                int Pieces = Convert.ToInt32(txtReturnPcs.Text.Trim());
                decimal Weight = Convert.ToDecimal(txtReturnWt.Text.Trim());

                if (Pieces > AWBPieces)
                {
                    lblStatus.Text = "Please enter valid Weight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                ScreeningDashboard objBAL = new ScreeningDashboard();
                bool blnResult = objBAL.SaveReturnToShipperDetails(AWBPrefix, AWBNumber, Pieces, Weight, Station, IsScreened, UserName, dtTimeStamp,FlightNo,FlightDate);

                if (blnResult)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanellSplit();</script>", false);
                    BtnList_Click(null, null);
                    lblStatus.Text = "Pieces returned to Shipper Successfully.";
                    lblStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblStatus.Text = "Error while returning to Shipper.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                //Code for Cleanup
                objBAL = null;
            }
            catch (Exception ex)
            { }
        }

        #region Print Labels
        protected void btnPrintLbl_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
            //{
            //    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("chkScreen")).Checked == true)
            //    {
            //        string awbnumb = (((Label)gdvULDDetails.Rows[i].FindControl("lblSAWBno"))).Text.ToString();
            //        int scancount = int.Parse(((Label)(gdvULDDetails.Rows[i].FindControl("lblScntAWBno"))).Text.ToString());
            //        int retpcs = int.Parse(((Label)(gdvULDDetails.Rows[i].FindControl("lblSretpcs"))).Text.ToString());
            //        int printcnt = scancount - retpcs;
            //        string location = Session["Station"].ToString();
            //        bool res = sb.UpdateLbl(awbnumb, location, printcnt);
            //    }
            //}

            //DataSet dslbl = new DataSet();
            ArrayList alreject = new ArrayList();
            int lblfrm, lblto, scanpcs, retpcs, printcnt;
       ArrayList finalal=new ArrayList();
       lblStatus.Text = "";
            string awbno,scrid,FlightNo,FlightDate;

            try
            {
                if (gdvULDDetails.Rows.Count > 0)
                {
                    int count = 0;

                    for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                    {
                        if (((CheckBox)gdvULDDetails.Rows[i].FindControl("chkScreen")).Checked == true)
                        {
                            count++;
                            FlightNo = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSfltno"))).Text.ToString();
                            FlightDate = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSfltdt"))).Text.ToString();
                            scrid = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSId"))).Text.ToString();
                            awbno = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSAWBno"))).Text.ToString();
                            scanpcs = int.Parse(((Label)(gdvULDDetails.Rows[i].FindControl("lblSTotScan"))).Text);
                            if ((((Label)(gdvULDDetails.Rows[i].FindControl("lblSretpcs"))).Text) == "")
                                retpcs = 0;
                            else
                             retpcs = int.Parse(((Label)(gdvULDDetails.Rows[i].FindControl("lblSretpcs"))).Text);
                            printcnt = scanpcs - retpcs;
                            
                            //update label count
                            bool res = sb.UpdateLbl(scrid,awbno, Session["Station"].ToString(), printcnt,FlightNo,FlightDate);

                            if (count == 1)
                            {
                                int id = int.Parse(((Label)(gdvULDDetails.Rows[i].FindControl("lblSId"))).Text.ToString());

                                DataSet dslbls = sb.GetLbls(awbno, id, FlightNo, FlightDate);
                                lblfrm = int.Parse(dslbls.Tables[0].Rows[0]["XrayLblFrm"].ToString());
                                lblto = int.Parse(dslbls.Tables[0].Rows[0]["XrayLblTo"].ToString());
                               
                                //dslbl = sb.getRejectLbl(Session["Station"].ToString(), lblfrm, lblto);
                                //if (dslbl.Tables[0].Rows.Count > 0)
                                //{
                                //    for (int j = 0; j < dslbl.Tables[0].Rows.Count; j++)
                                //    {
                                //        alreject.Add(dslbl.Tables[0].Rows[j]["LblNo"].ToString());
                                //    }
                                //}
                                //for (int k = lblfrm; k <= lblto; k++)
                                //{
                                //    if (alreject.Contains(k.ToString()) == false)
                                //    {
                                //        finalal.Add(k);
                                //    }
                                //}
                                string strPieceIds = string.Empty;
                                
                                int lblvalue = lblfrm;
                                for (int l = 1; l <= printcnt; l++)
                                {   
                                    strPieceIds = strPieceIds + lblvalue + "|";
                                    lblvalue++;
                                }
                                
                                //for (int m = 0; m < finalal.Count; m++)
                                //{
                                //    //objPrint.PrintSecurityLabel(awbno,finalal[m].ToString(),pcscnt,Session["Station"].ToString(), "EDSBOM", "BHIWAI");
                                //    strPieceIds = strPieceIds + finalal[m] + "|";
                                //}

                                if (strPieceIds.Length > 0)
                                    strPieceIds = strPieceIds.Substring(0, strPieceIds.Length - 1);

                                string strURL = "window.open('LabelPrinting.htm?param=0;0;0;" + awbno + ";0;0;XR;" + strPieceIds + ";" + Session["Station"].ToString() + ";Ship;Consig;', 'Print','left=0,top=0,width=260,height=220,toolbar=0,resizable=0');";

                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>" + strURL + "</script>", false);
                            }
                            if (count > 1)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Select only one row at a time');</script>", false);
                                break;
                            }
                        }
                    }
                    if (count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Select one row');</script>", false);

                    }
                }
             }
            catch (Exception ex)
            {
                lblStatus.Text = "There was an error while printing the labels.";
                lblStatus.ForeColor = Color.Red;
            }
            
        }
        #endregion Print Labels

        protected void btnPrintAWBDet_Click(object sender, EventArgs e)
        {
            try
            {
                if (gdvULDDetails.Rows.Count <= 0)
                {
                    lblStatus.Text = "No records available...";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                lblStatus.Text = "";
                SQLServer da = new SQLServer(Global.GetConnectionString());
                int count = 0;
                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("chkScreen")).Checked == true)
                    {
                        count++;

                        string[] s = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSAWBno"))).Text.ToString().Split('-');
                        string awbnumber = s[0] + s[1];
                        string FlightNo = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSfltno"))).Text.Trim();
                        string FlightDate = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSfltdt"))).Text.Trim();
                        string isscrreq = ((Label)(gdvULDDetails.Rows[i].FindControl("lblSScrReq"))).Text;
                        if (isscrreq == "N")
                            isscrreq = "No";
                        if (isscrreq == "Y")
                            isscrreq = "Yes";
                        string[] QueryNames = new string[3];
                        SqlDbType[] QueryTypes = new SqlDbType[3];
                        object[] QueryValues = new object[3];

                        QueryNames[0] = "awbno";
                        QueryNames[1] = "FlightNo";
                        QueryNames[2] = "FlightDate";

                        QueryTypes[0] = SqlDbType.VarChar;
                        QueryTypes[1] = SqlDbType.VarChar;
                        QueryTypes[2] = SqlDbType.VarChar;

                        QueryValues[0] = awbnumber;
                        QueryValues[1] = FlightNo;
                        QueryValues[2] = FlightDate;
                        DataSet dset = da.SelectRecords("sp_getAWBDetaillsForScreeningRpt", QueryNames, QueryValues, QueryTypes);
                        DataTable dt = dset.Tables[1].Copy();
                        DataColumn col1 = new DataColumn("IsScrReq", typeof(string));
                        col1.DefaultValue = isscrreq;
                        dt.Columns.Add(col1);
                        DataSet ds = new DataSet();
                        ds.Tables.Add(dt);

                        Session["awbdetailsforxray"] = dset;
                        Session["dt"] = dt;
                        ds.Dispose();
                        dt.Dispose();
                        dset.Dispose();
                    }
                }
                if (count == 1)
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('ShowAWBDetails.aspx','', 'left=0,top=0,width=800,height=450,toolbar=0,resizable=0');", true);
                }
                if (count > 1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Select only one row at a time');</script>", false);
                
                }
                if (count == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Select atleast one row');</script>", false);
                }
            }
            catch (Exception ex)
            { }
        }
        
        #region Button Send FSU
        protected void btnSendFSU_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                for (int i = 0; i < gdvULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)gdvULDDetails.Rows[i].FindControl("chkScreen")).Checked)
                    {
                        string AWBNumber = ((Label)gdvULDDetails.Rows[i].FindControl("lblSAWBno")).Text;
                        if (sb.SendFSUScreening(AWBNumber))
                        {
                            lblStatus.Text = "FSU Sent Successfully!";
                            lblStatus.ForeColor = Color.Green;
                        }
                        else
                        {

                            lblStatus.Text = "FSU Sending Failed!";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button Send PRI
        protected void btnSendPRI_Click(object sender, EventArgs e)
        {
            ACASBAL objACAS = new ACASBAL();
            string AWBNumber = string.Empty; string FlightNo = string.Empty; string FlightDate = string.Empty;
            try
            {
                foreach (GridViewRow row in gdvULDDetails.Rows)
                {
                    if (((CheckBox)row.FindControl("chkScreen")).Checked)
                    {
                        AWBNumber = ((Label)row.FindControl("lblSAWBno")).Text;
                        FlightNo = ((Label)row.FindControl("lblSfltno")).Text;
                        FlightDate = ((Label)row.FindControl("lblSfltdt")).Text;

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
                #region Sending PER Error Message on System Error
                try
                {
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
            finally
            {
                objACAS = null;
            }
        }
        #endregion

    }


}