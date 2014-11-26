using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using System.Drawing;
using BAL;
using SCM.Common.Struct;
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{

    public partial class GHA_Imp_Delivery : System.Web.UI.Page
    {

        #region Variables
        SQLServer da = new SQLServer(Global.GetConnectionString());
        LoginBL objBL = new LoginBL();
        string str = string.Empty;
        string Commodity = string.Empty;
        DateTime dtCurrentDate = DateTime.Now;
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        ReportDataSource rds3 = new ReportDataSource();
        DataTable dtTable1 = new DataTable("GHA_Imp_Delivery_dtTable1");
        DataTable dtTable2 = new DataTable("GHA_Imp_Delivery_dtTable2");
        DataTable dtTable3 = new DataTable("GHA_Imp_Delivery_dtTable3");
        DataTable dtTable4 = new DataTable("GHA_Imp_Delivery_dtTable4");
        #endregion Variables

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dtCurrentDate = (DateTime)Session["IT"];

                if (!IsPostBack)
                {
                    Session["Delivery_ShipperName"] = null;
                    ViewState["DOReopen"] = "N";
                    ViewState["ReopenedDoNo"] = "";

                    if (Session["RoleName"] != null)
                    {
                        if (Session["RoleName"].ToString() == "Super User")
                        {
                            btnReOpenDO.Visible = true;
                        }
                        else
                        {
                            btnReOpenDO.Visible = false;
                        }

                    }
                    #region Operation Time Popup Config Check
                    try
                    {
                        //LoginBL objConfig = new LoginBL();
                        btnOpsTime.Visible = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "enableActualOpsTime"));
                        BtnRecipt.Visible = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "enableDeliveryReciept"));
                        // objConfig = null;
                    }
                    catch (Exception ex)
                    { }
                    #endregion

                    Loaddropdown();
                    LoadTokenDropDown();
                    //LoadOperationTimeConfig();

                    if (Session["awbPrefix"] != null)
                    {
                        
                            txtprefix.Text = Session["awbPrefix"].ToString();
                            txtFlightPrefix.Text = Session["AirlinePrefix"].ToString();
                        
                    }
                    else
                    {
                        MasterBAL objBal = new MasterBAL();
                        Session["awbPrefix"] = objBal.awbPrefix();
                        txtprefix.Text = Session["awbPrefix"].ToString();
                        txtFlightPrefix.Text = Session["AirlinePrefix"].ToString();
                    }
                    if (Request.QueryString["DoNumber"] != null)
                    {

                        string DoNo = Request.QueryString["DoNumber"].ToString();
                        txtDoNumber.Text = DoNo;
                        DataSet DSet = new DataSet("Dlv_PageLoad_DSet");
                        DSet = da.SelectRecords("sp_GetDOHeaderDetails", "DoNumber", txtDoNumber.Text.Trim(), SqlDbType.VarChar);

                        txtprefix.Text = "";
                        hdRePrint.Value = "1";
                        btnList_Click(sender, e);
                        if (DSet != null)
                        {
                            if (DSet.Tables.Count > 0)
                            {
                                if (DSet.Tables[0].Rows.Count > 0)
                                {
                                    txtissuedate.Text = DSet.Tables[0].Rows[0]["IssueDate"].ToString();
                                    txtissuedto.Text = DSet.Tables[0].Rows[0]["IssuedTo"].ToString();
                                    txtissuename.Text = DSet.Tables[0].Rows[0]["IssueName"].ToString();
                                    txtreciversname.Text = DSet.Tables[0].Rows[0]["ReciversName"].ToString();
                                    txtconsignee.Text = DSet.Tables[0].Rows[0]["Consignee"].ToString();
                                    txtDORemarks.Text = DSet.Tables[0].Rows[0]["DORemarks"].ToString();
                                    txtDODate.Text = DSet.Tables[0].Rows[0]["DODate"].ToString();
                                }
                            }
                        }
                        btnprint.Visible = true;
                        if (DSet != null)
                        {
                            DSet.Dispose();
                        }
                    }
                    else
                    {
                        txtflightdate.Text = dtCurrentDate.ToString("dd/MM/yyyy");
                        LoadGrid();
                        LoadULDGrid();

                        txtissuedate.Text = dtCurrentDate.ToString("dd/MM/yyyy");

                        for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                        {
                            if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("check")).Checked == true)
                            {
                                HidFlag.Value = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtremainingpieces")).Text;
                            }
                        }
                    }

                    try
                    {
                        if (Session["ULDACT"].ToString().ToUpper() == "FALSE")
                        {
                            lblULDNo.Visible = false;
                            txtULDNo.Visible = false;
                            UDLdiv.Visible = false;
                            lblULD.Visible = false;
                        }

                        string strOutput = "";
                        if (Session["Man_DEMO_System"] == null)
                        {
                            //strOutput = objBL.GetMasterConfiguration("DemoInstance");
                            strOutput = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "DemoInstance");
                            if (strOutput != "")
                                Session["Man_DEMO_System"] = Convert.ToBoolean(strOutput);
                            else
                                Session["Man_DEMO_System"] = true;
                        }
                        if (Convert.ToBoolean(Session["Man_DEMO_System"]) == false)
                        {
                            lblGatePassNo.Visible = false;
                            ddlTokenList.Visible = false;
                            txtTokenDt.Visible = false;
                            ImageButton2.Visible = false;
                        }

                    }
                    catch (Exception ex)
                    {
                    }

                }

            }
            catch (Exception)
            { }

        }
        #endregion Load

        #region Load Operation Time Config
        /// <summary>
        /// Loads configuration of whether to show popup by default on "Save" or not.
        /// </summary>
        //private void LoadOperationTimeConfig()
        //{
        //    try
        //    {
        //        //Load configuration for operation time popup.
        //        if (Session["ShowOperationTimeOnSave"] == null)
        //        {
        //            Session["ShowOperationTimeOnSave"] = false;
        //            LoginBL objLogin = new LoginBL();
        //            string config = objLogin.GetMasterConfiguration("OperationTimePopup");
        //            if (config != null)
        //            {
        //                bool result = false;
        //                if (Boolean.TryParse(config, out result))
        //                    Session["ShowOperationTimeOnSave"] = result;
        //                else
        //                    Session["ShowOperationTimeOnSave"] = false;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        #endregion Load Operation Time Config

        #region LoadDropdown
        public void Loaddropdown()
        {
            DataSet Agent = new DataSet("Dlv_LoadDropdown_Agent");
            try
            {
                StockAllocationBAL objBAL = new StockAllocationBAL();
                Agent = objBAL.GetAgentCode();
                ddlAgentName.DataSource = Agent.Tables[0];
                ddlAgentName.DataTextField = "AgentName";
                ddlAgentName.DataValueField = "AgentCode";
                ddlAgentName.DataBind();
                ddlAgentName.Items.Insert(0, "Select");
                ddlAgentName.SelectedIndex = 0;
                objBAL = null;
            }
            catch (Exception)
            { }
            finally
            {
                if (Agent != null)
                {
                    Agent.Dispose();
                }
            }
        }
        #endregion Loaddropdown

        #region Check Function
        //public void check(object sender, EventArgs e)
        //{

        //    //code for getting data through sp and taking from table and showing in textbox
        //    if (txtissuedto.Text == null)
        //    {
        //        DataSet ds = null;
        //        try
        //        {
        //            for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
        //            {

        //                if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("chk")).Checked == true)
        //                {
        //                    string paramname = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;

        //                    string AWBNumber = string.Empty;

        //                    object paramvalue = AWBNumber;

        //                    SqlDbType paramtype = SqlDbType.NVarChar;

        //                    ds = da.SelectRecords("SpGetAgentCode", paramname, paramvalue, paramtype);
        //                    if (ds != null)
        //                    {
        //                        if (ds.Tables.Count > 0)
        //                        {
        //                            if (ds.Tables[0].Rows.Count > 0)
        //                            {
        //                                txtissuedto.Text = ds.Tables[0].Rows[0][0].ToString();
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //        }
        //        finally
        //        {
        //            if (ds != null)
        //                ds.Dispose();
        //        }
        //    }

        //}
        #endregion Check Function

        //Clear function to clear text boxes
        #region Clear
        public void clear()
        {
            txtAWBNo.Text = "";

            txtFlightPrefix.Text = "";
            txtFlightNo.Text = "";

            txthawb.Text = "";
            txtissuedate.Text = dtCurrentDate.ToString("dd/MM/yyyy");
            txtissuedto.Text = "";
            txtissuename.Text = "";
            txtreciversname.Text = "";
            txtconsignee.Text = "";
            grdMaterialDetails.DataSource = null;
            grdMaterialDetails.DataBind();
            lblStatus.ForeColor = Color.Red;
            lblStatus.Text = "";

            ddlAgentName.SelectedItem.Text = "";
            txtflightdate.Text = "";
        }
        #endregion Clear

        //Onclick of List Button
        #region List
        protected void btnList_Click(object sender, EventArgs e)
        {
            Session["Delivery_ShipperName"] = null;
            try
            {

                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "";
                ClearDeliveryInfo();
                grdMaterialDetails.DataSource = null;
                grdMaterialDetails.DataBind();
                grvDOChargesList.DataSource = null;
                grvDOChargesList.DataBind();
                LoadGrid();
                btnprint.Visible = false;
                DataSet ds = new DataSet("Dlv_btnList_ds");
                try
                {
                    string[] paramname = new string[14];
                    object[] paramvalue = new object[14];
                    SqlDbType[] paramtype = new SqlDbType[14];
                    /*************************** Set Param Name ********************************/
                    paramname[0] = "DONo";
                    paramname[1] = "AWBNo";
                    paramname[2] = "FlightNo";
                    paramname[3] = "StationCode";
                    paramname[4] = "Flightdate";
                    paramname[5] = "AWBPrefix";
                    paramname[6] = "DoNumber";
                    paramname[7] = "Status";
                    paramname[8] = "TokenNo";
                    paramname[9] = "TokenDate";
                    paramname[10] = "ULDNo";
                    paramname[11] = "IsPrint";
                    paramname[12] = "ConSearch";//Add byPawan 
                    paramname[13] = "AgentCode";

                    string dono = string.Empty;
                    string awbno = string.Empty;
                    string flightno = string.Empty;
                    string StationCode = string.Empty;
                    string Flightdate = string.Empty;
                    string TokenNo = string.Empty;
                    string TokenDate = string.Empty;
                    string strfromdate = string.Empty;
                    string strtodate = string.Empty;

                    DateTime dt;

                    try
                    {
                        if (txtTokenDt.Text.Trim() != "")
                        {
                            //dt = Convert.ToDateTime(txtInvoiceFrom.Text);
                            //Change 03082012
                            string day = txtTokenDt.Text.Substring(0, 2);
                            string mon = txtTokenDt.Text.Substring(3, 2);
                            string yr = txtTokenDt.Text.Substring(6, 4);
                            strfromdate = yr + "/" + mon + "/" + day;
                            dt = Convert.ToDateTime(strfromdate);
                        }
                    }
                    catch
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Selected Token Date format invalid!";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    DateTime dtto;

                    try
                    {
                        if (txtflightdate.Text.Trim() != "")
                        {
                            //dtto = Convert.ToDateTime(txtInvoiceTo.Text);
                            //Change 03082012
                            string day = txtflightdate.Text.Substring(0, 2);
                            string mon = txtflightdate.Text.Substring(3, 2);
                            string yr = txtflightdate.Text.Substring(6, 4);
                            strtodate = yr + "-" + mon + "-" + day;
                            dtto = Convert.ToDateTime(strtodate);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Selected Flight Date format invalid!";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }

                    dono = ddlAgentName.SelectedItem.Text;
                    if (dono == "Select")
                    {
                        dono = "";
                    }
                    awbno = txtAWBNo.Text;
                    if (awbno == "")
                    {
                        awbno = "";
                    }
                    if (ddlTokenList.SelectedIndex != 0)
                    {
                        TokenNo = ddlTokenList.SelectedItem.Text.Trim();
                    }
                    else
                    { TokenNo = ""; }
                    TokenDate = txtTokenDt.Text.Trim();
                    flightno = txtFlightPrefix.Text.Trim() + txtFlightNo.Text.Trim();
                    StationCode = Session["Station"].ToString();

                    if (txtflightdate.Text == "")
                    {
                        Flightdate = "";
                    }
                    else
                    {
                        Flightdate = txtflightdate.Text.Trim();
                    }
                    /*************************** Set Param Values ********************************/
                    paramvalue[0] = dono;
                    paramvalue[1] = awbno;
                    paramvalue[2] = flightno;
                    paramvalue[3] = StationCode;
                    paramvalue[4] = Flightdate;
                    paramvalue[5] = txtprefix.Text.Trim();
                    paramvalue[6] = txtDoNumber.Text.Trim();
                    paramvalue[7] = ddlStatus.Text.Trim();
                    paramvalue[8] = TokenNo;
                    paramvalue[9] = TokenDate;
                    paramvalue[10] = txtULDNo.Text.Trim();
                    paramvalue[11] = hdRePrint.Value.Trim() != string.Empty ? hdRePrint.Value.Trim() : "0";
                    paramvalue[12] = txtConSearch.Text.Trim();//Add by Pawan 
                    paramvalue[13] = ddlAgentName.SelectedIndex > 0 ? ddlAgentName.SelectedValue.Trim() : "";//Add by Pawan 

                    /*************************** Set Param Type ********************************/
                    paramtype[0] = SqlDbType.NVarChar;
                    paramtype[1] = SqlDbType.NVarChar;
                    paramtype[2] = SqlDbType.NVarChar;
                    paramtype[3] = SqlDbType.NVarChar;
                    paramtype[4] = SqlDbType.VarChar;
                    paramtype[5] = SqlDbType.VarChar;
                    paramtype[6] = SqlDbType.VarChar;
                    paramtype[7] = SqlDbType.VarChar;
                    paramtype[8] = SqlDbType.VarChar;
                    paramtype[9] = SqlDbType.VarChar;
                    paramtype[10] = SqlDbType.VarChar;
                    paramtype[11] = SqlDbType.VarChar;
                    paramtype[12] = SqlDbType.VarChar;//Add by Pawan 
                    paramtype[13] = SqlDbType.VarChar;//Add by Pawan 

                    Session["Agent"] = ddlAgentName.SelectedItem.Text.Trim();
                    Session["FligtNo"] = txtFlightPrefix.Text.Trim() + txtFlightNo.Text.Trim(); //DdlFlightno.SelectedItem.Text.Trim();
                    Session["AWBNO"] = txtAWBNo.Text;
                    Session["Flightdate"] = txtflightdate.Text;

                    ds = da.SelectRecords("SpGetDeliveryNew_GHA", paramname, paramvalue, paramtype);
                    if (ds != null && ds.Tables.Count > 1 && ds.Tables[0].Rows.Count < 1 && ds.Tables[1].Rows.Count < 1)
                    {
                        lblStatus.Text = "No Data Found Please Try Again";
                        lblStatus.ForeColor = Color.Red;

                        grdMaterialDetails.DataSource = null;
                        grdMaterialDetails.DataBind();

                        grdULDDelivery.DataSource = null;
                        grdULDDelivery.DataBind();

                        LoadGrid();
                        LoadULDGrid();


                        return;
                    }
                    paramname = null;
                    paramvalue = null;
                    paramtype = null;
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grdMaterialDetails.DataSource = ds.Tables[0];
                                grdMaterialDetails.DataBind();
                                hdRePrint.Value = string.Empty;
                                BLArrival arr = new BLArrival();
                                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                {
                                    string status = ds.Tables[0].Rows[k][14].ToString();
                                    string AWBNumber = ds.Tables[0].Rows[k][0].ToString();
                                    string location = Session["Station"].ToString();
                                    string Complete = "";
                                    string station = Session["Station"].ToString();

                                    string statusIsclosed = "";
                                    string PayMode = ds.Tables[0].Rows[k][17].ToString();
                                  Session["Delivery_ShipperName"] = ds.Tables[0].Rows[k]["ShipperName"].ToString();
                               
                                    DataSet ds2 = new DataSet("Dlv_btnList_ds2");
                                    ds2 = arr.GetIsClosedtatus(AWBNumber, station);
                                    if (ds2 != null)
                                    {
                                        if (ds2.Tables[0].Rows.Count > 0)
                                        {
                                            statusIsclosed = ds2.Tables[0].Rows[0]["IsClosed"].ToString();
                                        }
                                        ds2.Dispose();
                                    }
                                    DataSet ds3 = new DataSet("Dlv_btnList_ds3");
                                    ds3 = arr.GetCCAmount(AWBNumber);
                                    if (ds3 != null)
                                    {
                                        if (ds3.Tables[0].Rows.Count > 0)
                                        {
                                            Session["CCAmount"] = ds3.Tables[0].Rows[0]["Total"].ToString();
                                        }
                                        ds3.Dispose();
                                    }

                                    if (Complete == "Complete")
                                    {

                                        for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                                        {
                                            string awb = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;
                                            if (awb == AWBNumber)
                                            {
                                                for (int j = 0; j < grdMaterialDetails.Columns.Count; j++)
                                                {
                                                    grdMaterialDetails.Rows[i].Cells[j].Enabled = false;
                                                }
                                            }
                                        }
                                    }
                                    if (statusIsclosed == "Closed")
                                    {

                                        for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                                        {
                                            string awb = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;
                                            if (awb == AWBNumber)
                                            {
                                                for (int j = 0; j < grdMaterialDetails.Columns.Count; j++)
                                                {
                                                    grdMaterialDetails.Rows[i].Cells[j].Enabled = false;
                                                }
                                            }
                                        }
                                        lblStatus.Text = "Flight Is closed in Arrival..Please Reopen the flight in Arrival";
                                        lblStatus.ForeColor = Color.Red;
                                        return;
                                    }
                                    if (status == "partial")
                                    {

                                        for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                                        {
                                            string awb = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;
                                            if (awb == AWBNumber)
                                            {
                                                for (int j = 1; j < grdMaterialDetails.Columns.Count; j++)
                                                {
                                                    grdMaterialDetails.Rows[i].Cells[j].Enabled = true;
                                                }
                                            }
                                        }
                                        lblStatus.Text = "Some AWB are Partially Delivered";
                                        lblStatus.ForeColor = Color.Green;
                                    }
                                    else if (status == "")
                                    {
                                        lblStatus.Text = "New Flight is Arrived";
                                        lblStatus.ForeColor = Color.Green; ;
                                    }
                                    if (PayMode == "CC")
                                    {
                                        for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                                        {
                                            string awb = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;
                                            if (awb == AWBNumber)
                                            {
                                                lblStatus.Text = "Please Collect cash for AWB No:" + awb;
                                                lblStatus.ForeColor = Color.Red;
                                            }
                                        }
                                    }
                                }
                                arr = null;
                                if (!chkReprint.Checked)
                                {
                                    foreach (GridViewRow gvr in grdMaterialDetails.Rows)
                                    {   //No Reprinting. Keep grid editable.
                                        //((TextBox)gvr.FindControl("txtactualpieces")).Text = "0";
                                        ((TextBox)gvr.FindControl("txtactualpieces")).Enabled = true;
                                        //((TextBox)gvr.FindControl("txtactualwt")).Text = "0";
                                        ((TextBox)gvr.FindControl("txtactualwt")).Enabled = true;
                                        ((TextBox)gvr.FindControl("txtflightno")).Enabled = true;
                                    }
                                    txtissuedate.Text = dtCurrentDate.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    foreach (GridViewRow gvr in grdMaterialDetails.Rows)
                                    {   //No Reprinting. Keep grid editable.                                    
                                        ((TextBox)gvr.FindControl("txtactualpieces")).Enabled = false;
                                        ((TextBox)gvr.FindControl("txtactualwt")).Enabled = false;
                                        ((TextBox)gvr.FindControl("txtflightno")).Enabled = false;
                                    }
                                }
                                DataRow dr = ds.Tables[0].Rows[0];
                                if (dr != null)
                                {
                                    chkReprint.Enabled = false;
                                }
                                EnableDeliveryPcsandWt(false, false);
                            }

                            if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                            {
                                grdULDDelivery.DataSource = ds.Tables[1];
                                grdULDDelivery.DataBind();
                                foreach (GridViewRow row in grdULDDelivery.Rows)
                                {
                                    //Set colour coding for received ULDs.
                                    if (((HiddenField)row.FindControl("IsReceived")).Value == "Y")
                                    {
                                        row.BackColor = CommonUtility.ColorHighlightedGrid;

                                    }

                                }
                            }
                            else
                            {
                                grdULDDelivery.DataSource = null;
                                grdULDDelivery.DataBind();
                                LoadULDGrid();
                            }
                        }
                    }
                    else
                    {
                        lblStatus.Text = "No Data Found Please Try Again";
                        lblStatus.ForeColor = Color.Red;
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (ds != null)
                    {
                        ds.Dispose();
                    }
                //    if (Session["Delivery_ShipperName"] != null)
                //    {
                //        Session["Delivery_ShipperName"] = null;
                //}
                }
            }
            catch (Exception)
            { }
        }
        #endregion List

        #region CalculatePieces
        //protected void calculateactualpiece(object sender, EventArgs e)
        //{
        //    TextBox btn = (TextBox)sender;

        //    //Get the row that contains this button
        //    GridViewRow gvr = (GridViewRow)btn.NamingContainer;

        //    //Get rowindex
        //    int rowindex = gvr.RowIndex;
        //    string grwt = ((Label)grdMaterialDetails.Rows[rowindex].FindControl("txtgrwt")).Text;
        //    string expectdpcs = ((Label)grdMaterialDetails.Rows[rowindex].FindControl("txtexpectedpieces")).Text;
        //    string actualpcs = ((TextBox)grdMaterialDetails.Rows[rowindex].FindControl("txtactualpieces")).Text;
        //    float scct = Convert.ToInt32(grwt) / Convert.ToInt32(expectdpcs);
        //    float total = scct * Convert.ToInt32(actualpcs);
        //    ((TextBox)grdMaterialDetails.Rows[rowindex].FindControl("txtactualwt")).Text = total.ToString();
        //    int exp = Convert.ToInt32(expectdpcs);
        //    int act = Convert.ToInt32(actualpcs);
        //    if (exp >= act)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "cann();", true);
        //        ((TextBox)grdMaterialDetails.Rows[rowindex].FindControl("txtactualpieces")).Text = "";
        //    }
        //}
        #endregion CalculatePices

        #region btnClear click event
        //protected void btnClear_Click(object sender, EventArgs e)
        //{
        //    clear();
        //    chkReprint.Checked = false;
        //    chkReprint.Enabled = true;
        //    btnPrintDO.Enabled = false;
        //    //grdMaterialDetails.DataSource = null;
        //    //grdMaterialDetails.DataBind();  
        //    LoadGrid();
        //}
        #endregion btnClear click

        #region btnsave click event
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string DelInfo = "";
                lblStatus.Text = "";
                btnprint.Visible = false;
                string strDONumber = string.Empty;
                lblStatus.ForeColor = Color.Red;
                bool IsSelected = false;
                if (grdMaterialDetails.Rows == null || grdMaterialDetails.Rows.Count <= 0)
                {
                    lblStatus.Text = "Please select AWB to save DO details.";
                    return;
                }

                if (Session["DeliveryInfoMandatory"] == null || Convert.ToString(Session["DeliveryInfoMandatory"]).ToLower() == "true")
                {
                    if (Session["DeliveryInfoMandatory"] == null)
                        //DelInfo = objBL.GetMasterConfiguration("IsDeliveryInfoMandatory");
                        DelInfo = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "IsDeliveryInfoMandatory");
                    else
                        DelInfo = Convert.ToString(Session["DeliveryInfoMandatory"]);

                    if (DelInfo != "")
                    {
                        Session["DeliveryInfoMandatory"] = Convert.ToBoolean(DelInfo);
                        if (Convert.ToBoolean(DelInfo))
                        {
                            if (!DeliveryInfoValidation())
                                return;
                        }
                    }
                    else
                    {
                        Session["DeliveryInfoMandatory"] = false;
                    }
                }

                //if (txtconsignee.Text == "")//jayant
                //{
                //    lblStatus.Text = "Please Enter Cosignee Name.";
                //}
                //if (txtreciversname.Text == "")
                //{
                //    lblStatus.Text = "Please Enter Reciever Name.";
                //}//end

                int intReopenRemPcs = 0; decimal dcReopenRemWt = 0;
                if (ViewState["DOReopen"].ToString() == "Y")
                {
                    for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                    {
                        HyperLink hypLinkDONo = ((HyperLink)grdMaterialDetails.Rows[i].FindControl("hlnkDONumber"));
                        string NextDO = "";
                        if (grdMaterialDetails.Rows.Count != i + 1)
                        {
                            HyperLink NexthypLinkDONo = ((HyperLink)grdMaterialDetails.Rows[i + 1].FindControl("hlnkDONumber"));
                            NextDO = NexthypLinkDONo.Text;
                        }

                        if (hypLinkDONo.Text == "")
                        {
                            if (NextDO == "" || NextDO == ViewState["ReopenedDoNo"].ToString())
                            {
                                int IntRemainingPcs = Convert.ToInt32(ViewState["IntRemainingPcs"].ToString());
                                decimal IntRemainingWt = Convert.ToDecimal(ViewState["IntRemainingWt"].ToString());
                                dcReopenRemWt = Convert.ToDecimal(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualwt")).Text.Trim());
                                intReopenRemPcs = Convert.ToInt32(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualpieces")).Text.Trim());
                                break;
                            }
                        }
                    }
                }
                for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                {
                    if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("check")).Checked == true)
                    {
                        int IntRemainingPcs = 0;
                        decimal dcRemainingWt = 0;
                        if (ViewState["DOReopen"].ToString() == "Y")
                        {
                            IntRemainingPcs = Convert.ToInt32(ViewState["IntRemainingPcs"].ToString());
                            dcRemainingWt = Convert.ToDecimal(ViewState["IntRemainingWt"].ToString());
                        }
                        else
                        {
                            IntRemainingPcs = Convert.ToInt32(((Label)grdMaterialDetails.Rows[i].FindControl("txtexpectedpieces")).Text.Trim());
                            dcRemainingWt = Convert.ToDecimal(((Label)grdMaterialDetails.Rows[i].FindControl("txtgrwt")).Text.Trim());
                        }
                        int IntDeliverPcs = Convert.ToInt32(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualpieces")).Text.Trim());
                        decimal IdcDeliverWt = Convert.ToDecimal(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualwt")).Text.Trim());
                        int IntArrivedPcs = Convert.ToInt32(((Label)grdMaterialDetails.Rows[i].FindControl("txtoriginalpieces")).Text.Trim());
                        decimal IdcArrivedWt = Convert.ToDecimal(((Label)grdMaterialDetails.Rows[i].FindControl("txtoriginalgrwt")).Text.Trim());
                        int IntAcceptedPcs = Convert.ToInt32(((Label)grdMaterialDetails.Rows[i].FindControl("lblAccPcs")).Text.Trim());
                        decimal IdcAcceptedWt = Convert.ToDecimal(((Label)grdMaterialDetails.Rows[i].FindControl("lblAccWt")).Text.Trim());

                        //txtDelieveredpieces
                        if (ViewState["DOReopen"].ToString() == "Y")
                        {
                            HyperLink hypLinkDONo = ((HyperLink)grdMaterialDetails.Rows[i].FindControl("hlnkDONumber"));
                            if (hypLinkDONo.Text != "")
                            {
                                if (IntRemainingPcs + intReopenRemPcs < IntDeliverPcs)
                                {
                                    lblStatus.Text = "Deliver pieces can not be more than Remaining pieces.";
                                    return;
                                }

                                if (dcRemainingWt + dcReopenRemWt < IdcDeliverWt)
                                {
                                    lblStatus.Text = "Deliver weight can not be more than Remaining weight.";
                                    return;
                                }

                                if (IntRemainingPcs + intReopenRemPcs == IntDeliverPcs && dcRemainingWt + dcReopenRemWt != IdcDeliverWt)
                                {
                                    lblStatus.Text = "Please enter valid Weight.";
                                    ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualwt")).Focus();
                                    return;
                                }

                                if (dcRemainingWt + dcReopenRemWt == IdcDeliverWt && IntRemainingPcs + intReopenRemPcs != IntDeliverPcs)
                                {
                                    lblStatus.Text = "Please enter valid Pieces.";
                                    ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualpieces")).Focus();
                                    return;
                                }
                                #region Code Added By Deepak for Delivery OverRide

                                ((Label)grdMaterialDetails.Rows[i].FindControl("txtexpectedpieces")).Text = (IntArrivedPcs - IntDeliverPcs).ToString();
                                ((Label)grdMaterialDetails.Rows[i].FindControl("txtgrwt")).Text = (IdcArrivedWt - IdcDeliverWt).ToString();
                                ((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredpieces")).Text = (IntDeliverPcs).ToString();
                                ((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredwt")).Text = (IdcDeliverWt).ToString();
                                #endregion
                            }
                        }
                        else
                        {
                            // Will enter here when Not Reopened
                            #region Code Commented By Deepak (14/04/2014)
                            if (IntRemainingPcs < IntDeliverPcs)
                            {
                                lblStatus.Text = "Deliver pieces can not be more than Remaining pieces.";
                                return;
                            }

                            if (dcRemainingWt < IdcDeliverWt)
                            {
                                lblStatus.Text = "Deliver weight can not be more than Remaining weight.";
                                return;
                            }

                            if (IntRemainingPcs == IntDeliverPcs && dcRemainingWt != IdcDeliverWt)
                            {
                                lblStatus.Text = "Please enter valid Weight.";
                                ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualwt")).Focus();
                                return;
                            }

                            if (dcRemainingWt == IdcDeliverWt && IntRemainingPcs != IntDeliverPcs)
                            {
                                lblStatus.Text = "Please enter valid Pieces.";
                                ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualpieces")).Focus();
                                return;
                            }
                            #endregion

                            #region Code Added By Deepak for Delivery OverRide
                            //if (IntArrivedPcs < IntDeliverPcs)
                            //{
                            //    lblStatus.Text = "Deliver pieces can not be more than Arrived pieces.";
                            //    return;
                            //}

                            //if (IdcArrivedWt < IdcDeliverWt)
                            //{
                            //    lblStatus.Text = "Deliver weight can not be more than Arrived weight.";
                            //    return;
                            //}

                            //if (IntArrivedPcs < IntRemainingPcs)
                            //{
                            //    lblStatus.Text = "Deliver pieces can not be more than Arrived pieces.";
                            //    return;
                            //}

                            //if (IdcArrivedWt < IntRemainingWt)
                            //{
                            //    lblStatus.Text = "Deliver weight can not be more than Arrived weight.";
                            //    return;
                            //}

                            //Validate if delivered pcs are more than accepted pcs.
                            if (IntDeliverPcs > IntAcceptedPcs)
                            {
                                lblStatus.Text = "AWB: " + ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text + " can't deliver more than total accepted !";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                            //Validate if delivered wt is more than accepted wt.
                            if (IdcDeliverWt > IdcAcceptedWt)
                            {
                                lblStatus.Text = "AWB: " + ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text + " can't deliver more than total accepted !";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }

                            ((Label)grdMaterialDetails.Rows[i].FindControl("txtexpectedpieces")).Text = (IntArrivedPcs - IntDeliverPcs).ToString();
                            ((Label)grdMaterialDetails.Rows[i].FindControl("txtgrwt")).Text = (IdcArrivedWt - IdcDeliverWt).ToString();
                            ((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredpieces")).Text = (IntDeliverPcs).ToString();
                            ((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredwt")).Text = (IdcDeliverWt).ToString();
                            #endregion
                        }
                        #region Validate if CC AWB's life cycle completed in collection
                        if (((HyperLink)grdMaterialDetails.Rows[i].FindControl("hlnkCollection")).Text.Trim() == "Collect")
                        {
                            lblStatus.Text = "AWB: " + ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text + " can't be delivered! Please collect the pending invoice amount!!";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        #endregion
                    }
                }
                btnPrintDO.Enabled = false;

                lblStatus.Text = "";

                #region Check AWB Validity Cutsoms Deepak(15/04/2014)

                for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                {

                    if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("check")).Checked == true)
                    {

                        try
                        {
                            CustomsImportBAL objCustoms = new CustomsImportBAL();
                            if (!objCustoms.ValidateAWBDelivery(((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text, ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtflightno")).Text, ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text))
                            {
                                lblStatus.Text = "AWBNo: " + ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text + " cannot be delivered as its on hold.";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                            objCustoms = null;
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                #endregion

                #region "Save AWB details for DO"
                string paramname1 = null;
                object paramvalue1 = null;
                SqlDbType paramtype1;
                string[] paramname = null;
                object[] paramvalue = null;
                SqlDbType[] paramtype = null;
                DataSet dscommodity = new DataSet("Dlv_btnSave_dscommodity");
                string OrgDONumber = string.Empty;
                string Currency = "";

                for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                {

                    if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("check")).Checked == true)
                    {
                        if (((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualpieces")).Enabled && ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualwt")).Enabled)
                        {
                            OrgDONumber = ((HyperLink)grdMaterialDetails.Rows[i].FindControl("hlnkDONumber")).Text.Trim();
                        }
                        else
                        {
                            lblStatus.Text = "AWB: " + ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text + " already Delivered!";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                }

                //Get Import Charges SP Name
                string SP = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ImportChargesCalculation");
                if (SP == "" && SP.Length < 1)
                    SP = "sp_CalculateImportCharges_V1";
                strDONumber = GetDONumber(Session["UserName"].ToString().ToUpper(), Session["Station"].ToString());

                for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                {

                    if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("check")).Checked == true)
                    {
                        if (((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualpieces")).Enabled && ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualwt")).Enabled)
                        {

                            #region Calculate Import Charges
                            if (((HyperLink)grdMaterialDetails.Rows[i].FindControl("hlnkDONumber")).Text.Length < 1)
                            {
                                string AWBNumber = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text.Trim();
                                string AWBPrefix = AWBNumber.Substring(0, AWBNumber.IndexOf('-'));
                                AWBNumber = AWBNumber.Substring(AWBNumber.IndexOf('-') + 1, AWBNumber.Length - AWBNumber.IndexOf('-') - 1);
                                int DLVPcs = Convert.ToInt32(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualpieces")).Text);
                                decimal DLVWt = Convert.ToDecimal(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualwt")).Text);

                                object[] PVal = { AWBPrefix, AWBNumber, Session["IT"], DLVPcs, DLVWt, Session["Station"].ToString() };
                                string[] PName = { "AWBPrefix", "AWBNumber", "DLVDate", "DLVPcs", "DLVWt", "DLVStation" };
                                SqlDbType[] PType = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.Int, SqlDbType.Decimal,SqlDbType.VarChar };
                                DataSet dsImportCharges = new DataSet("Dlv_btnSave_dsImportCharges");
                                da = new SQLServer(Global.GetConnectionString());
                                dsImportCharges = da.SelectRecords(SP, PName, PVal, PType);

                                if (dsImportCharges != null && dsImportCharges.Tables.Count > 0 && dsImportCharges.Tables[0].Rows.Count > 0)
                                {

                                    ((Label)grdMaterialDetails.Rows[i].FindControl("lblCharges")).Text = dsImportCharges.Tables[0].Rows[0]["Charge"].ToString();
                                    ((Label)grdMaterialDetails.Rows[i].FindControl("lblTax")).Text = dsImportCharges.Tables[0].Rows[0]["Tax"].ToString();
                                    ((Label)grdMaterialDetails.Rows[i].FindControl("lblTotal")).Text = dsImportCharges.Tables[0].Rows[0]["Total"].ToString();
                                    ((Label)grdMaterialDetails.Rows[i].FindControl("lblAmtDue")).Text = dsImportCharges.Tables[0].Rows[0]["Total"].ToString();
                                    Currency = dsImportCharges.Tables[0].Rows[0]["Currency"].ToString();
                                }
                            }
                            #endregion

                            //strDONumber = GetDONumber(Session["UserName"].ToString().ToUpper(), Session["Station"].ToString());

                            string status = string.Empty;
                            string operationType = string.Empty;
                            paramname1 = "AWBNumber";

                            paramvalue1 = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;

                            paramtype1 = SqlDbType.NVarChar;

                            da = new SQLServer(Global.GetConnectionString());

                            dscommodity = da.SelectRecords("Sp_GetCommodity", paramname1, paramvalue1, paramtype1);
                            Commodity = "";
                            if (dscommodity != null)
                            {
                                if (dscommodity.Tables[0].Rows.Count > 0)
                                {
                                    Commodity = dscommodity.Tables[0].Rows[0]["CommodityCode"].ToString();
                                }
                                dscommodity.Dispose();
                            }

                            IsSelected = true;
                            int chkconvert = Convert.ToInt32(((Label)grdMaterialDetails.Rows[i].FindControl("txtoriginalpieces")).Text.Trim());//Convert.ToInt32(((Label)grdMaterialDetails.Rows[i].FindControl("txtexpectedpieces")).Text);
                            int chkcon = Convert.ToInt32(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualpieces")).Text);
                            if (chkcon <= 0)
                            {
                                lblStatus.Text = "Delivered pieces can not be 0";
                                ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualpieces")).Focus();
                                return;
                            }
                            if (chkconvert > chkcon)
                            {
                                status = "partial";
                                operationType = "P";
                            }
                            if (chkconvert == chkcon)
                            {
                                status = "Complete";
                                operationType = "C";
                            }

                            paramname = new string[31];
                            paramname[0] = "AgentName";
                            paramname[1] = "AWBNumber";
                            paramname[2] = "FlightNumber";
                            paramname[3] = "ExpectedPcs";
                            paramname[4] = "ActualPcs";
                            paramname[5] = "ReciverName";
                            paramname[6] = "DONumber";
                            paramname[7] = "ActualWeight";
                            paramname[8] = "IssuedTo";
                            paramname[9] = "IssueName";
                            paramname[10] = "IssueDate";
                            paramname[11] = "HAWBNo";
                            paramname[12] = "Consignee";
                            paramname[13] = "status";
                            paramname[14] = "operationType ";
                            paramname[15] = "ExpectedWeight";
                            paramname[16] = "FltDate";
                            paramname[17] = "UpdatedBy";
                            paramname[18] = "UpdatedOn";
                            paramname[19] = "HAWBNumber";
                            paramname[20] = "Origin";
                            paramname[21] = "Destination";
                            paramname[22] = "Location";
                            paramname[23] = "DORemarks";
                            paramname[24] = "Charges";
                            paramname[25] = "Tax";
                            paramname[26] = "Total";
                            paramname[27] = "AmtDue";
                            paramname[28] = "OrgDONumber";
                            paramname[29] = "ReopenedDoNo";
                            paramname[30] = "Currency";
                            //ViewState["ReopenedDoNo"] 

                            paramvalue = new object[31];
                            paramvalue[0] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtagentname")).Text;
                            paramvalue[1] = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;
                            paramvalue[2] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtflightno")).Text;
                            paramvalue[3] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtexpectedpieces")).Text;
                            paramvalue[4] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualpieces")).Text;
                            paramvalue[5] = txtreciversname.Text.Trim();
                            paramvalue[6] = strDONumber;
                            paramvalue[7] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtactualwt")).Text;
                            paramvalue[8] = txtissuedto.Text.Trim();
                            paramvalue[9] = txtissuename.Text.Trim();
                            paramvalue[10] = txtissuedate.Text.Trim();
                            paramvalue[11] = ((Label)grdMaterialDetails.Rows[i].FindControl("txthawbs")).Text.Trim();
                            paramvalue[12] = txtconsignee.Text.Trim();
                            paramvalue[13] = status;
                            paramvalue[14] = operationType;
                            paramvalue[15] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtgrwt")).Text;
                            paramvalue[16] = DateTime.ParseExact(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text, "dd/MM/yyyy", null);
                            paramvalue[17] = Session["UserName"].ToString();
                            paramvalue[18] = dtCurrentDate;
                            paramvalue[19] = ((Label)grdMaterialDetails.Rows[i].FindControl("txthawbs")).Text.Trim();
                            paramvalue[20] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtOrigin")).Text;
                            paramvalue[21] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtDest")).Text;
                            paramvalue[22] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtLocationAWB")).Text;
                            paramvalue[23] = txtDORemarks.Text.Trim();
                            paramvalue[24] = ((Label)grdMaterialDetails.Rows[i].FindControl("lblCharges")).Text;
                            paramvalue[25] = ((Label)grdMaterialDetails.Rows[i].FindControl("lblTax")).Text;
                            paramvalue[26] = ((Label)grdMaterialDetails.Rows[i].FindControl("lblTotal")).Text;
                            paramvalue[27] = ((Label)grdMaterialDetails.Rows[i].FindControl("lblAmtDue")).Text;
                            paramvalue[28] = OrgDONumber;
                            paramvalue[29] = ViewState["ReopenedDoNo"].ToString();
                            paramvalue[30] = Currency;

                            txtDODate.Text = dtCurrentDate.ToString("dd/MM/yyyy HH:mm");

                            paramtype = new SqlDbType[31];
                            paramtype[0] = SqlDbType.NVarChar;
                            paramtype[1] = SqlDbType.NVarChar;
                            paramtype[2] = SqlDbType.NVarChar;
                            paramtype[3] = SqlDbType.NVarChar;
                            paramtype[4] = SqlDbType.NVarChar;
                            paramtype[5] = SqlDbType.NVarChar;
                            paramtype[6] = SqlDbType.NVarChar;
                            paramtype[7] = SqlDbType.Float;
                            paramtype[8] = SqlDbType.NVarChar;
                            paramtype[9] = SqlDbType.NVarChar;
                            paramtype[10] = SqlDbType.NVarChar;
                            paramtype[11] = SqlDbType.NVarChar;
                            paramtype[12] = SqlDbType.NVarChar;
                            paramtype[13] = SqlDbType.NVarChar;
                            paramtype[14] = SqlDbType.NVarChar;
                            paramtype[15] = SqlDbType.Float;
                            paramtype[16] = SqlDbType.DateTime;
                            paramtype[17] = SqlDbType.VarChar;
                            paramtype[18] = SqlDbType.DateTime;
                            paramtype[19] = SqlDbType.VarChar;
                            paramtype[20] = SqlDbType.VarChar;
                            paramtype[21] = SqlDbType.VarChar;
                            paramtype[22] = SqlDbType.VarChar;
                            paramtype[23] = SqlDbType.NVarChar;
                            paramtype[24] = SqlDbType.VarChar;
                            paramtype[25] = SqlDbType.VarChar;
                            paramtype[26] = SqlDbType.VarChar;
                            paramtype[27] = SqlDbType.VarChar;
                            paramtype[28] = SqlDbType.VarChar;
                            paramtype[29] = SqlDbType.VarChar; 
                            paramtype[30] = SqlDbType.VarChar;

                            if (!chkReprint.Checked)
                            {
                                bool ds = da.InsertData("SpCreateDO_New", paramname, paramtype, paramvalue);
                                if (ds == true)
                                {
                                    lblStatus.Text = "Do Created";
                                    lblStatus.ForeColor = Color.Green;
                                    ViewState["DOReopen"] = "N";
                                    ViewState["ReopenedDoNo"] = "";
                                    cls_BL.addMsgToOutBox("SCM", "AWB Number: " + ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text.Trim() + " Delivered.", "", "");
                                }
                            }
                            else
                            {
                                lblStatus.Text = "Insertion Failed Please Try Again";
                                lblStatus.ForeColor = Color.Red;
                            }
                            paramname = null;
                            paramtype = null;
                            paramvalue = null;
                        }
                    }
                }
                if (dscommodity != null)
                {
                    dscommodity.Dispose();
                }
                #endregion

                #region "Save ULD details for DO"
                for (int i = 0; i < grdULDDelivery.Rows.Count; i++)
                {
                    if (((CheckBox)grdULDDelivery.Rows[i].FindControl("ULDcheck")).Checked == true)
                    {
                        string ULDNumber = ((Label)grdULDDelivery.Rows[i].FindControl("txtuldsno")).Text.Trim();
                        string status = string.Empty;
                        string operationType = string.Empty;
                        if (((HiddenField)grdULDDelivery.Rows[i].FindControl("IsReceived")).Value != "Y")
                        {
                            if (((Label)grdULDDelivery.Rows[i].FindControl("txtexpectedpieces")).Text.Trim() == "0")
                            {
                                lblStatus.Text = ULDNumber + " - ULD Already delivered.";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                        }
                        else
                        {
                            if (((HyperLink)grdULDDelivery.Rows[i].FindControl("hlnkDONumber")).Text.Trim() != "")
                            {

                                lblStatus.Text = ULDNumber + " - ULD Already delivered.";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                        }

                        #region Validate if CC AWB's life cycle completed in collection
                        if (((HyperLink)grdULDDelivery.Rows[i].FindControl("hlnkCollection")).Text.Trim() == "Collect")
                        {
                            lblStatus.Text = "AWB: " + ((LinkButton)grdULDDelivery.Rows[i].FindControl("txtawbno")).Text + " can't be delivered! Please collect the pending invoice amount!!";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        #endregion

                        IsSelected = true;

                        paramname = new string[15];
                        paramname[0] = "ULDNumber";
                        paramname[1] = "FltNo";
                        paramname[2] = "FltDt";
                        paramname[3] = "Origin";
                        paramname[4] = "Destination";
                        paramname[5] = "UserName";
                        paramname[6] = "TimeStamp";
                        paramname[7] = "ReceiverName";
                        paramname[8] = "ConsigneeName";
                        paramname[9] = "IssuedTo";
                        paramname[10] = "IssuedName";
                        paramname[11] = "IssueDate";
                        paramname[12] = "DockNumber";
                        paramname[13] = "DoNumber";
                        paramname[14] = "Location";

                        paramvalue = new object[15];
                        paramvalue[0] = ULDNumber;
                        paramvalue[1] = ((TextBox)grdULDDelivery.Rows[i].FindControl("txtflightno")).Text.Trim();
                        paramvalue[2] = DateTime.ParseExact(((TextBox)grdULDDelivery.Rows[i].FindControl("txtbookingdate")).Text.Trim(), "dd/MM/yyyy", null);
                        paramvalue[3] = ((Label)grdULDDelivery.Rows[i].FindControl("txtOrigin")).Text.Trim();
                        paramvalue[4] = ((Label)grdULDDelivery.Rows[i].FindControl("txtDest")).Text.Trim();
                        paramvalue[5] = Convert.ToString(Session["UserName"]);
                        paramvalue[6] = Convert.ToDateTime(Session["IT"]);
                        paramvalue[7] = txtreciversname.Text.Trim();
                        paramvalue[8] = txtconsignee.Text.Trim();
                        paramvalue[9] = txtissuedto.Text.Trim();
                        paramvalue[10] = txtissuename.Text.Trim();
                        paramvalue[11] = txtissuedate.Text.Trim();
                        paramvalue[12] = txtDockNumber.Text.Trim();
                        paramvalue[13] = strDONumber;
                        paramvalue[14] = ((TextBox)grdULDDelivery.Rows[i].FindControl("txtLocationUld")).Text.Trim();

                        paramtype = new SqlDbType[15];
                        paramtype[0] = SqlDbType.NVarChar;
                        paramtype[1] = SqlDbType.NVarChar;
                        paramtype[2] = SqlDbType.DateTime;
                        paramtype[3] = SqlDbType.NVarChar;
                        paramtype[4] = SqlDbType.NVarChar;
                        paramtype[5] = SqlDbType.NVarChar;
                        paramtype[6] = SqlDbType.DateTime;
                        paramtype[7] = SqlDbType.NVarChar;
                        paramtype[8] = SqlDbType.NVarChar;
                        paramtype[9] = SqlDbType.NVarChar;
                        paramtype[10] = SqlDbType.NVarChar;
                        paramtype[11] = SqlDbType.NVarChar;
                        paramtype[12] = SqlDbType.NVarChar;
                        paramtype[13] = SqlDbType.NVarChar;
                        paramtype[14] = SqlDbType.NVarChar;

                        if (!chkReprint.Checked)
                        {
                            if (da == null)
                                da = new SQLServer(Global.GetConnectionString());
                            bool ds = da.InsertData("sp_SaveULDDeliveryDetails", paramname, paramtype, paramvalue);
                            if (ds == true)
                            {
                                lblStatus.Text = "Do Created";
                                lblStatus.ForeColor = Color.Green;
                                cls_BL.addMsgToOutBox("SCM", "ULD Number: " + ULDNumber + " Delivered.", "", "");
                            }
                        }
                        else
                        {
                            lblStatus.Text = "Insertion Failed Please Try Again";
                            lblStatus.ForeColor = Color.Red;
                        }
                    }
                }
                paramname = null;
                paramtype = null;
                paramvalue = null;
                #endregion

                if (!IsSelected)
                {
                    lblStatus.Text = "Please select AWB Number(s) for generating DO.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                Session["CurrentAgent"] = "";

                if (!chkReprint.Checked)
                {
                    //txtDoNumber.Text = strDONumber;

                    //Show popup to save actual operation time.
                    if (CommonUtility.ShowOperationTimeOnSave == true)
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
                    ddlStatus.SelectedIndex = 0;
                    btnList_Click(sender, e);
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "DO Generation Successful!";
                    //btnprint.Visible = true;
                }
            }
            catch (Exception)
            { }
        }
        #endregion btnsave click

        protected bool DeliveryInfoValidation()
        {
            if (txtreciversname.Text == "")
            {
                lblStatus.Text = "Please Enter Receiver's Name.";
                return false;
            }
            if (txtconsignee.Text == "")
            {
                lblStatus.Text = "Please Enter Consignee Name.";
                return false;
            }
            if (txtissuedto.Text == "")
            {
                lblStatus.Text = "Please Enter Issued To.";
                return false;
            }
            if (txtissuename.Text == "")
            {
                lblStatus.Text = "Please Enter Issue Name.";
                return false;
            }

            return true;
        }

        private void SaveOperationTime(bool ShowOperationsPopuup)
        {
            try
            {

                //Set dataset for AWBs in AWB grid.                    
                List<SCM.Common.Struct.clsOperationTimeStamp> objListOpsTime = new List<SCM.Common.Struct.clsOperationTimeStamp>();

                SCM.Common.Struct.clsOperationTimeStamp objOpsTimeStamp;
                new SCM.Common.Struct.clsOperationTimeStamp();

                //Set data of AWB for updating Arrival time stamp.
                foreach (GridViewRow gvr in grdMaterialDetails.Rows)
                {
                    if (((CheckBox)gvr.FindControl("check")).Checked)
                    {
                        objOpsTimeStamp = new SCM.Common.Struct.clsOperationTimeStamp();
                        if (((LinkButton)gvr.FindControl("txtawbno")).Text != "" &&
                            (((LinkButton)gvr.FindControl("txtawbno")).Text.Contains("-")) &&
                            (((LinkButton)gvr.FindControl("txtawbno")).Text.Length > ((LinkButton)gvr.FindControl("txtawbno")).Text.IndexOf('-')))
                        {
                            objOpsTimeStamp.AWBPrefix = ((LinkButton)gvr.FindControl("txtawbno")).Text.Substring(0,
                                ((LinkButton)gvr.FindControl("txtawbno")).Text.IndexOf('-'));
                            objOpsTimeStamp.AWBNumber = ((LinkButton)gvr.FindControl("txtawbno")).Text.Substring(
                                ((LinkButton)gvr.FindControl("txtawbno")).Text.IndexOf('-') + 1);
                        }
                        else
                        {
                            objOpsTimeStamp.AWBPrefix = "";
                            objOpsTimeStamp.AWBNumber = ((LinkButton)gvr.FindControl("txtawbno")).Text;
                        }
                        objOpsTimeStamp.Description = "";
                        DateTime dt = System.DateTime.Now;
                        if (DateTime.TryParseExact(((TextBox)gvr.FindControl("txtbookingdate")).Text, "dd/MM/yyyy", null,
                            System.Globalization.DateTimeStyles.None, out dt))
                        {
                            objOpsTimeStamp.FlightDt = dt;
                        }
                        else
                        {
                            objOpsTimeStamp.FlightDt = DateTime.Now;
                        }
                        objOpsTimeStamp.FlightNo = ((TextBox)gvr.FindControl("txtflightno")).Text;
                        objOpsTimeStamp.OperationalStatus = "DLV";
                        objOpsTimeStamp.OperationalType = "DLV";
                        objOpsTimeStamp.OperationDate = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy"); //DateTime.Now.ToString("dd/MM/yyyy");
                        objOpsTimeStamp.OperationTime = ((DateTime)Session["IT"]).ToString("HH:mm"); //DateTime.Now.ToString("HH:mm");
                        int pieceCount = 0;
                        if (int.TryParse(((TextBox)gvr.FindControl("txtactualpieces")).Text, out pieceCount))
                        {
                            objOpsTimeStamp.Pieces = pieceCount;
                        }
                        else
                        {
                            objOpsTimeStamp.Pieces = 0;
                        }
                        decimal weight = 0;
                        if (decimal.TryParse(((TextBox)gvr.FindControl("txtactualwt")).Text, out weight))
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

                    }
                }

                //Set data of ULD for updating Arrival time stamp.
                foreach (GridViewRow gvr in grdULDDelivery.Rows)
                {
                    if (((CheckBox)gvr.FindControl("ULDcheck")).Checked)
                    {
                        objOpsTimeStamp = new SCM.Common.Struct.clsOperationTimeStamp();
                        objOpsTimeStamp.ULDNumber = ((Label)gvr.FindControl("txtuldsno")).Text;
                        objOpsTimeStamp.AWBPrefix = "";
                        objOpsTimeStamp.AWBNumber = "";
                        objOpsTimeStamp.Description = "";
                        DateTime dt = System.DateTime.Now;
                        if (DateTime.TryParseExact(((TextBox)gvr.FindControl("txtbookingdate")).Text, "dd/MM/yyyy", null,
                            System.Globalization.DateTimeStyles.None, out dt))
                        {
                            objOpsTimeStamp.FlightDt = dt;
                        }
                        else
                        {
                            objOpsTimeStamp.FlightDt = DateTime.Now;
                        }
                        objOpsTimeStamp.FlightNo = ((TextBox)gvr.FindControl("txtflightno")).Text;
                        objOpsTimeStamp.OperationalStatus = "DLV";
                        objOpsTimeStamp.OperationalType = "DLV";
                        objOpsTimeStamp.OperationDate = DateTime.Now.ToString("dd/MM/yyyy");
                        objOpsTimeStamp.OperationTime = DateTime.Now.ToString("HH:mm");
                        int pieceCount = 0;
                        if (int.TryParse(((TextBox)gvr.FindControl("txtactualpieces")).Text, out pieceCount))
                        {
                            objOpsTimeStamp.Pieces = pieceCount;
                        }
                        else
                        {
                            objOpsTimeStamp.Pieces = 0;
                        }
                        decimal weight = 0;
                        if (decimal.TryParse(((TextBox)gvr.FindControl("txtactualwt")).Text, out weight))
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

        #region gridview selectedinddexchange
        //protected void grdMaterialDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        //{
        //    string str = grdMaterialDetails.SelectedRow.RowIndex.ToString();
        //}
        #endregion gridview selectedinddexchange

        #region gridview Rowcommand
        protected void grdMaterialDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Sangram 2014-10-30
                try
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    GridViewRow selectedRow = grdMaterialDetails.Rows[index];

                    string agentname = ((TextBox)grdMaterialDetails.Rows[index].FindControl("txtagentname")).Text;
                    string awbno = ((TextBox)grdMaterialDetails.Rows[index].FindControl("txtawbno")).Text;
                    string ExpectedPieces = ((TextBox)grdMaterialDetails.Rows[index].FindControl("txtexpectedpieces")).Text;
                    string flightno = ((TextBox)grdMaterialDetails.Rows[index].FindControl("txtflightno")).Text;
                    ViewState["AgentName"] = agentname;
                    ViewState["AWBNo"] = awbno;
                    ViewState["ExpPieces"] = ExpectedPieces;
                    ViewState["fltno"] = flightno;
                }
                catch(Exception)
                {
                }
            }
            if (e.CommandName == "ShowDODeatils")
            {
                DataSet dsDO = new DataSet("Dlv_grdMaterialDetails_Row_dsDO");
                try
                {
                    grvDOChargesList.DataSource = null;
                    grvDOChargesList.DataBind();
                    string AWBNo = e.CommandArgument.ToString();
                    string param = "AWBNo";
                    SqlDbType dbtypes = SqlDbType.VarChar;
                    object values = AWBNo;
                    dsDO = da.SelectRecords("SP_GetDODetails1", param, values, dbtypes);
                    if (dsDO != null)
                    {
                        if (dsDO.Tables.Count > 0)
                        {
                            if (dsDO.Tables[0].Rows.Count > 0)
                            {
                                txtconsignee.Text = dsDO.Tables[0].Rows[0]["Consignee"].ToString();
                                txtissuedto.Text = dsDO.Tables[0].Rows[0]["IssuedTo"].ToString();
                                txtissuedate.Text = dsDO.Tables[0].Rows[0]["IssueDate"].ToString();
                                txthawb.Text = dsDO.Tables[0].Rows[0]["HAWBNumber"].ToString();
                                txtissuename.Text = dsDO.Tables[0].Rows[0]["IssueName"].ToString();
                                txtreciversname.Text = dsDO.Tables[0].Rows[0]["ReciversName"].ToString();
                            }
                        }
                    }

                    dsDO = da.SelectRecords("SP_GetDOCharges", param, values, dbtypes);
                    values = null;
                    if (dsDO != null)
                    {
                        if (dsDO.Tables != null)
                        {
                            if (dsDO.Tables.Count > 0)
                            {
                                if (dsDO.Tables[0].Rows.Count > 0)
                                {
                                    grvDOChargesList.PageIndex = 0;
                                    grvDOChargesList.DataSource = dsDO;
                                    grvDOChargesList.DataMember = dsDO.Tables[0].TableName;
                                    grvDOChargesList.DataBind();
                                    grvDOChargesList.Visible = true;
                                    dsDO.Clear();
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
                    if (dsDO != null)
                    {
                        dsDO.Dispose();
                    }
                }
            }
        }
        #endregion gridview Rowcommand

        #region printdo buttonclick
        protected void btnPrintDO_Click1(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "prndo", "PrintDO();", true);
        }
        #endregion printdo buttonclick

        #region execute buttonclick
        protected void btnExecute_Click1(object sender, EventArgs e)
        {
            clear();
        }
        #endregion xecute buttonclick

        #region LoadgridInfo
        public void LoadGrid()
        {
            DataTable myDataTable = new DataTable("Dlv_LoadGrid_myDataTable");
            DataColumn myDataColumn = null;
            try
            {

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "DoNumber";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AgentName";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AWBNumber";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Origin";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Destination";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "PiecesCount";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "GrossWeight";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RCVPieces";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ExpectedWeight";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Expectedcount";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltNo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ExpectedWt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Discription";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "CommCode";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AWBDate";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RemainingPieces";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RCVWeight";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ConsigneeName";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "TotalPieceCount";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "PayMode";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "InvoiceNo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "IsCollection";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "DORecPcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "DORecWt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "HAWBNo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Location";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Charges";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Tax";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Total";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AmountDue";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "InvoiceURL";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "CollectionURL";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AccPcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AccWt";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "UpdatedOn";
                myDataTable.Columns.Add(myDataColumn);

                DataRow dr;
                dr = myDataTable.NewRow();
                dr["AgentName"] = "";
                dr["AWBNumber"] = "";//"5";
                dr["Origin"] = "";// "5";
                dr["Destination"] = "";// "9";
                dr["PiecesCount"] = "0";
                dr["GrossWeight"] = "0";
                dr["RCVPieces"] = "0";
                dr["ExpectedWeight"] = "0";
                dr["Expectedcount"] = "0";
                dr["FltNo"] = "";
                dr["ExpectedWt"] = "0";
                dr["Discription"] = "";
                dr["CommCode"] = "";
                dr["AWBDate"] = "";
                dr["RemainingPieces"] = "0";
                dr["RCVWeight"] = "0";
                dr["ConsigneeName"] = "";
                dr["TotalPieceCount"] = "0";
                dr["PayMode"] = "";
                dr["DORecPcs"] = "0";
                dr["DORecWt"] = "0";
                dr["HAWBNo"] = "";
                dr["InvoiceNo"] = string.Empty;
                dr["IsCollection"] = string.Empty;
                dr["Charges"] = "0";
                dr["Tax"] = "0";
                dr["Total"] = "0";
                dr["AmountDue"] = "0";
                dr["InvoiceURL"] = string.Empty;
                dr["CollectionURL"] = string.Empty;
                dr["AccPcs"] = "0";
                dr["AccWt"] = "0";
                dr["UpdatedOn"] = string.Empty;

                myDataTable.Rows.Add(dr);

                //Bind the DataTable to the Grid
                grdMaterialDetails.DataSource = null;
                grdMaterialDetails.DataSource = myDataTable;
                grdMaterialDetails.DataBind();
            }
            catch (Exception)
            { }
            finally
            {
                if (myDataColumn != null)
                {
                    myDataColumn.Dispose();
                }
                if (myDataTable != null)
                {
                    myDataTable.Dispose();
                }
            }
        }
        #endregion LoadgridInfo

        protected void BtnRecipt_Click(object sender, EventArgs e)
        {
            Response.Redirect("PaymentReceipt.aspx", true);
        }

        protected void btnprint_Click(object sender, EventArgs e)
        {
            if (txtDoNumber.Text == "")
            {
                lblStatus.Text = "Please select DO Number to print";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }
            PrintDeliveryOrderPDF();
        }

        #region LoadgridInfo
        public void LoadULDGrid()
        {
            DataTable myDataTable = new DataTable("Dlv_LoadULDGrid_myDataTable");
            DataColumn myDataColumn = null;
            try
            {

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "DoNumber";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ULDNo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AWBCount";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Origin";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Destination";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "PiecesCount";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "GrossWeight";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RCVPieces";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ExpectedWeight";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Expectedcount";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltNo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ExpectedWt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Discription";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AWBDate";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RemainingPieces";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RCVWeight";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ConsigneeName";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "PayMode";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "DORecPcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "DORecWt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "HAWBNo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Location";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "IsReceived";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "IsCollection";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();//Pawan
                myDataColumn.DataType = Type.GetType("System.String");//Pawan
                myDataColumn.ColumnName = "CollectionURL";//Pawan
                myDataTable.Columns.Add(myDataColumn);//Pawan

                myDataColumn = new DataColumn();//pawan
                myDataColumn.DataType = Type.GetType("System.String");//pawan
                myDataColumn.ColumnName = "InvoiceNo";//pawan
                myDataTable.Columns.Add(myDataColumn);//pawan

                DataRow dr;
                dr = myDataTable.NewRow();
                dr["ULDNo"] = "";
                dr["AWBCount"] = "";//"5";
                dr["Origin"] = "";// "5";
                dr["Destination"] = "";// "9";
                dr["PiecesCount"] = "0";
                dr["GrossWeight"] = "0";
                dr["RCVPieces"] = "0";
                dr["ExpectedWeight"] = "0";
                dr["Expectedcount"] = "0";
                dr["FltNo"] = "";
                dr["ExpectedWt"] = "0";
                dr["Discription"] = "";

                dr["AWBDate"] = "";
                dr["RemainingPieces"] = "0";
                dr["RCVWeight"] = "0";
                dr["ConsigneeName"] = "";
                dr["PayMode"] = "";
                dr["DORecPcs"] = "0";
                dr["DORecWt"] = "0";
                dr["HAWBNo"] = "";
                dr["IsReceived"] = "N";
                dr["CollectionURL"] = "";//Pawan
                dr["InvoiceNo"] = "";//Pawan

                myDataTable.Rows.Add(dr);

                //Bind the DataTable to the Grid
                grdULDDelivery.DataSource = null;
                grdULDDelivery.DataSource = myDataTable;
                grdULDDelivery.DataBind();
            }
            catch (Exception)
            { }
            finally
            {
                if (myDataTable != null)
                    myDataTable.Dispose();
                if (myDataColumn != null)
                    myDataColumn.Dispose();
            }
        }
        #endregion LoadgridInfo

        private string GetDONumber(string ULDNumber, string Station)
        {
            string returnVal="";
            // Sangram 2014-10-30
            try
            {
                string[] ColumnNames = new string[2];
                SqlDbType[] DataType = new SqlDbType[2];
                Object[] Values = new object[2];
                int i = 0;

                i = 0;
                //0
                ColumnNames.SetValue("AWBNumber", i);
                DataType.SetValue(SqlDbType.NVarChar, i);
                Values.SetValue(ULDNumber, i);

                i++;

                ColumnNames.SetValue("Station", i);
                DataType.SetValue(SqlDbType.NVarChar, i);
                Values.SetValue(Station, i);

                 returnVal = da.GetStringByProcedure("SpGetDONumber", ColumnNames, Values, DataType);
                da = null;
                if (returnVal == null)
                {
                    returnVal = string.Empty;
                }
            }
            catch (Exception)
            {
            }
                return returnVal;
          
        }


        protected void PrintDeliveryOrderPDF()
        {
            try
            {
                lblStatus.Text = "";
                Session["MultipleAWB"] = null;
                Session["MultipleULD"] = null;
                DataTable dtHeader = new DataTable("Dlv_PrintDeliveryOrderPDF_dtHeader");
                DataTable dtULDDetails = new DataTable("Dlv_PrintDeliveryOrderPDF_dtULDDetails");
                DataTable dtSurface = new DataTable("Dlv_PrintDeliveryOrderPDF_dtSurface");
                try
                {
                    #region Added New Code Deepak
                    string CustomerSupport = string.Empty;
                    string CustomerRules = string.Empty;
                    double ST = 0;

                    dtHeader.Columns.Add("DoNumber", typeof(string));
                    dtHeader.Columns.Add("IssuedTo", typeof(string));
                    dtHeader.Columns.Add("IssueName", typeof(string));
                    dtHeader.Columns.Add("ReciverName", typeof(string));
                    dtHeader.Columns.Add("IssueDate", typeof(string));
                    dtHeader.Columns.Add("StaffId", typeof(string));
                    dtHeader.Columns.Add("Customer", typeof(string));
                    dtHeader.Columns.Add("Station", typeof(string));
                    dtHeader.Columns.Add("ServiceTax", typeof(double));
                    dtHeader.Columns.Add("Total", typeof(int));
                    dtHeader.Columns.Add("ChargeHead", typeof(string));
                    dtHeader.Columns.Add("Charge", typeof(string));
                    dtHeader.Columns.Add("Consignee", typeof(string));
                    dtHeader.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                    dtHeader.Columns.Add("DODate", typeof(string));
                    dtHeader.Columns.Add("Remarks", typeof(string));
                    dtHeader.Columns.Add("AgentName", typeof(string));
                    dtHeader.Columns.Add("PayMode", typeof(string));
                    dtHeader.Columns.Add("CommCode", typeof(string));
                    dtHeader.Columns.Add("Rules", typeof(string));
                    dtHeader.Columns.Add("AwbOrg", typeof(string));
                    dtHeader.Columns.Add("AwbDest", typeof(string));
                    dtHeader.Columns.Add("ShipperName", typeof(string));


                    dtULDDetails.Columns.Add("ULDNumber", typeof(string));
                    dtULDDetails.Columns.Add("AWBCount", typeof(string));
                    dtULDDetails.Columns.Add("FlightNumber", typeof(string));
                    dtULDDetails.Columns.Add("ActualPieces", typeof(string));
                    dtULDDetails.Columns.Add("ActualWeight", typeof(string));
                    dtULDDetails.Columns.Add("IssueDate", typeof(string));

                    dtSurface.Columns.Add("DoNumber", typeof(string));
                    dtSurface.Columns.Add("AWBNumber", typeof(string));
                    dtSurface.Columns.Add("HAWBNo", typeof(string));
                    dtSurface.Columns.Add("FlightNumber", typeof(string));
                    dtSurface.Columns.Add("IssuedTo", typeof(string));
                    dtSurface.Columns.Add("IssueName", typeof(string));
                    dtSurface.Columns.Add("ReciverName", typeof(string));
                    dtSurface.Columns.Add("ActualPieces", typeof(string));
                    dtSurface.Columns.Add("ActualWeight", typeof(string));
                    dtSurface.Columns.Add("IssueDate", typeof(string));
                    dtSurface.Columns.Add("AgentName", typeof(string));
                    dtSurface.Columns.Add("Discription", typeof(string));
                    dtSurface.Columns.Add("ConsigneeName", typeof(string));
                    dtSurface.Columns.Add("CCAmount", typeof(float));
                    dtSurface.Columns.Add("PayMode", typeof(string));
                    dtSurface.Columns.Add("CommCode", typeof(string));
                    dtSurface.Columns.Add("DODate", typeof(string));
                    dtSurface.Columns.Add("StaffId", typeof(string));
                    dtSurface.Columns.Add("Station", typeof(string));
                    dtSurface.Columns.Add("CustomerSupport", typeof(string));
                    dtSurface.Columns.Add("ServiceTax", typeof(double));
                    dtSurface.Columns.Add("Remarks", typeof(string));
                    dtSurface.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                    dtSurface.Columns.Add("Total", typeof(int));
                    dtSurface.Columns.Add("AwbOrg", typeof(string));
                    dtSurface.Columns.Add("AwbDest", typeof(string));
                    dtSurface.Columns.Add("ShipperName", typeof(string));

                    string Shippername = Session["Delivery_ShipperName"].ToString();

                    int Total = 0;
                    int Count = 0;
                    int ULDCount = 0;
                    #region Getting AWB Info for DO Print

                    DataRow drSurface = null;
                    string MultipleAWBs = string.Empty;
                    for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                    {
                        if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("check")).Checked == true)
                        {
                            if (Convert.ToDecimal(((Label)grdMaterialDetails.Rows[i].FindControl("lblAmtDue")).Text) > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please Pay amount due!');</SCRIPT>", false);
                                return;
                            }
                            Count++;
                            if (((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text != "")
                            {
                                string AWBNumber = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;
                                DataSet dscomm = new DataSet("Dlv_PrintDeliveryOrderPDF_dscomm");
                                dscomm = da.SelectRecords("Sp_GetCommodity_V1", "AWBNumber", AWBNumber, SqlDbType.VarChar);
                                drSurface = dtSurface.NewRow();
                                try
                                {
                                    if (dscomm != null)
                                    {
                                        if (dscomm.Tables.Count > 0)
                                        {
                                            if (dscomm.Tables[0].Rows.Count > 0)
                                            {
                                                drSurface["CommCode"] = dscomm.Tables[0].Rows[0]["CommodityCode"].ToString();
                                            }
                                            if (dscomm.Tables[1].Rows.Count > 0)
                                            {
                                                drSurface["CCAmount"] = dscomm.Tables[1].Rows[0]["Total"].ToString();
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                { }
                                finally
                                {
                                    if (dscomm != null)
                                    {
                                        dscomm.Dispose();
                                    }
                                }
                                //drSurface["DoNumber"] = str;
                                drSurface["DoNumber"] = ((HyperLink)grdMaterialDetails.Rows[i].FindControl("hlnkDONumber")).Text;
                                drSurface["AWBNumber"] = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;
                                drSurface["HAWBNo"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txthawbs")).Text.Trim();
                                drSurface["FlightNumber"] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtflightno")).Text;
                                drSurface["IssuedTo"] = txtissuedto.Text.Trim();
                                drSurface["IssueName"] = txtissuename.Text.Trim();
                                drSurface["ReciverName"] = txtreciversname.Text.Trim();
                                Total += Convert.ToInt32(((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredpieces")).Text.Trim());
                                drSurface["ActualPieces"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredpieces")).Text + "/" + ((Label)grdMaterialDetails.Rows[i].FindControl("txtTotalpieces")).Text;
                                drSurface["ActualWeight"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredwt")).Text;
                                DateTime dtdate = DateTime.ParseExact(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text, "dd/MM/yyyy", null);
                                string sdate = dtdate.ToString("dd/MM/yyyy");
                                drSurface["IssueDate"] = sdate;// ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text;
                                drSurface["AgentName"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtagentname")).Text;
                                drSurface["Discription"] = ((Label)grdMaterialDetails.Rows[i].FindControl("lblDiscription")).Text;
                                drSurface["ConsigneeName"] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtConsigneeName")).Text;

                                drSurface["PayMode"] = ((Label)grdMaterialDetails.Rows[i].FindControl("lblpaymenttype")).Text;
                                drSurface["AwbOrg"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtOrigin")).Text;
                                drSurface["AwbDest"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtDest")).Text;
                                drSurface["ShipperName"] = Shippername;

                                //Added extra by Vijay
                                drSurface["StaffId"] = Session["UserName"].ToString();
                                drSurface["Station"] = Session["Station"].ToString();
                                if (Session["ST"] != null)
                                    ST = Convert.ToDouble(Session["ST"].ToString());
                                else
                                {
                                    MasterBAL objBal = new MasterBAL();
                                    Session["ST"] = objBal.getServiceTax();
                                    ST = Convert.ToDouble(Session["ST"].ToString());
                                }

                                drSurface["ServiceTax"] = ST;
                                DataSet dsCustomer = new DataSet("Dlv_PrintDeliveryOrderPDF_dsCustomer");
                                try
                                {
                                    dsCustomer = da.SelectRecords("Sp_GetCustomerSupportInfo");
                                    if (dsCustomer.Tables[0].Rows.Count > 0)
                                    {
                                        CustomerSupport = dsCustomer.Tables[0].Rows[0]["CustomerSupport"].ToString();
                                    }
                                    else
                                    {
                                        CustomerSupport = "";
                                    }
                                }
                                catch
                                {
                                    CustomerSupport = "";
                                }
                                finally
                                {
                                    if (dsCustomer != null)
                                    {
                                        dsCustomer.Dispose();
                                    }
                                }
                                drSurface["CustomerSupport"] = CustomerSupport;
                                drSurface["Remarks"] = txtDORemarks.Text.Trim();
                                drSurface["DODate"] = txtDODate.Text.Trim() == string.Empty ? ((DateTime)Session["IT"]).ToString("dd/MM/yyyy HH:mm") : txtDODate.Text.Trim();


                                System.IO.MemoryStream Logo2 = null;
                                try
                                {
                                    Logo2 = CommonUtility.GetImageStream(Page.Server);
                                }
                                catch (Exception)
                                {
                                    Logo2 = new System.IO.MemoryStream();
                                }

                                drSurface["Logo"] = Logo2.ToArray();

                                if (Logo2 != null)
                                {
                                    Logo2.Dispose();
                                }
                                //Added extra by Vijay


                                dtSurface.Rows.Add(drSurface);
                                MultipleAWBs += "," + drSurface["AWBNumber"];
                            }
                        }
                    }
                    string strHeadOrg = "", strHeadDest = "", strDONumber = "", strIssueTo = "", strIssueName = "";
                    if (Count == 0)
                    {
                        for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                        {
                            if (((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text != "")
                            {
                                if (Convert.ToDecimal(((Label)grdMaterialDetails.Rows[i].FindControl("lblAmtDue")).Text) > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please pay amount due!');</SCRIPT>", false);
                                    return;
                                }
                                string AWBNumber = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;
                                DataSet dscomm = new DataSet("Dlv_PrintDeliveryOrderPDF_dscomm");
                                dscomm = da.SelectRecords("Sp_GetCommodity_V1", "AWBNumber", AWBNumber, SqlDbType.VarChar);
                                drSurface = dtSurface.NewRow();
                                try
                                {
                                    if (dscomm != null)
                                    {
                                        if (dscomm.Tables.Count > 0)
                                        {
                                            if (dscomm.Tables[0].Rows.Count > 0)
                                            {
                                                drSurface["CommCode"] = dscomm.Tables[0].Rows[0]["CommodityCode"].ToString();
                                            }
                                            if (dscomm.Tables[1].Rows.Count > 0)
                                            {
                                                drSurface["CCAmount"] = dscomm.Tables[1].Rows[0]["Total"].ToString();
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                { }
                                finally
                                {
                                    if (dscomm != null)
                                    {
                                        dscomm.Dispose();
                                    }
                                }
                                //drSurface["DoNumber"] = str;
                                drSurface["DoNumber"] = strDONumber = ((HyperLink)grdMaterialDetails.Rows[i].FindControl("hlnkDONumber")).Text;
                                drSurface["AWBNumber"] = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;
                                drSurface["HAWBNo"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txthawbs")).Text.Trim();
                                drSurface["FlightNumber"] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtflightno")).Text;
                                drSurface["IssuedTo"] = strIssueTo = txtissuedto.Text.Trim();
                                drSurface["IssueName"] = strIssueName = txtissuename.Text.Trim();
                                drSurface["ReciverName"] = txtreciversname.Text.Trim();
                                Total += Convert.ToInt32(((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredpieces")).Text.Trim());
                                drSurface["ActualPieces"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredpieces")).Text + "/" + ((Label)grdMaterialDetails.Rows[i].FindControl("txtoriginalpieces")).Text;
                                drSurface["ActualWeight"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredwt")).Text;
                                DateTime dtdate = DateTime.ParseExact(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text, "dd/MM/yyyy", null);
                                string sdate = dtdate.ToString("dd/MM/yyyy");
                                drSurface["IssueDate"] = sdate;// ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text;
                                drSurface["AgentName"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtagentname")).Text;
                                drSurface["Discription"] = ((Label)grdMaterialDetails.Rows[i].FindControl("lblDiscription")).Text;
                                drSurface["ConsigneeName"] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtConsigneeName")).Text;

                                drSurface["PayMode"] = ((Label)grdMaterialDetails.Rows[i].FindControl("lblpaymenttype")).Text;
                                drSurface["AwbOrg"] = strHeadOrg = ((Label)grdMaterialDetails.Rows[i].FindControl("txtOrigin")).Text;
                                drSurface["AwbDest"] = strHeadDest = ((Label)grdMaterialDetails.Rows[i].FindControl("txtDest")).Text;
                                drSurface["ShipperName"] = Shippername;

                                //Added extra by Vijay
                                drSurface["StaffId"] = Session["UserName"].ToString();
                                drSurface["Station"] = Session["Station"].ToString();
                                if (Session["ST"] != null)
                                    ST = Convert.ToDouble(Session["ST"].ToString());
                                else
                                {
                                    MasterBAL objBal = new MasterBAL();
                                    Session["ST"] = objBal.getServiceTax();
                                    ST = Convert.ToDouble(Session["ST"].ToString());
                                }

                                drSurface["ServiceTax"] = ST;
                                DataSet dsCustomer = new DataSet("Dlv_PrintDeliveryOrderPDF_dsCustomer");
                                try
                                {
                                    dsCustomer = da.SelectRecords("Sp_GetCustomerSupportInfo");
                                    if (dsCustomer.Tables[0].Rows.Count > 0)
                                    {
                                        CustomerSupport = dsCustomer.Tables[0].Rows[0]["CustomerSupport"].ToString();
                                    }
                                    else
                                    {
                                        CustomerSupport = "";
                                    }
                                }
                                catch
                                {
                                    CustomerSupport = "";
                                }
                                finally
                                {
                                    if (dsCustomer != null)
                                    {
                                        dsCustomer.Dispose();
                                    }
                                }
                                drSurface["CustomerSupport"] = CustomerSupport;
                                drSurface["Remarks"] = txtDORemarks.Text.Trim();
                                drSurface["DODate"] = txtDODate.Text.Trim() == string.Empty ? dtCurrentDate.ToString("dd/MM/yyyy HH:mm") : txtDODate.Text.Trim();

                                System.IO.MemoryStream Logo2 = null;
                                try
                                {
                                    Logo2 = CommonUtility.GetImageStream(Page.Server);
                                }
                                catch (Exception)
                                {
                                    Logo2 = new System.IO.MemoryStream();
                                }

                                drSurface["Logo"] = Logo2.ToArray();

                                if (Logo2 != null)
                                {
                                    Logo2.Dispose();
                                }

                                //Added extra by Vijay

                                dtSurface.Rows.Add(drSurface);
                                MultipleAWBs += "," + drSurface["AWBNumber"];
                            }
                        }
                    }
                    for (int f = 0; f < dtSurface.Rows.Count; f++)
                    {
                        dtSurface.Rows[f]["Total"] = Total;
                    }
                    Session["MultipleAWB"] = MultipleAWBs;

                    drSurface = null;
                    #endregion

                    #region Getting ULD Info for DO Print
                    string MultipleULDs = string.Empty;
                    //Sangram 2014-10-30
                    try
                    {
                        for (int i = 0; i < grdULDDelivery.Rows.Count; i++)
                        {
                            if (((CheckBox)grdULDDelivery.Rows[i].FindControl("ULDcheck")).Checked == true)
                            {
                                ULDCount++;
                                if (((Label)grdULDDelivery.Rows[i].FindControl("txtuldsno")).Text.Trim() != "")
                                {
                                    DataRow drULDDetails = dtULDDetails.NewRow();
                                    drULDDetails["ULDNumber"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtuldsno")).Text.Trim();
                                    drULDDetails["FlightNumber"] = ((TextBox)grdULDDelivery.Rows[i].FindControl("txtflightno")).Text;
                                    Total += Convert.ToInt32(((Label)grdULDDelivery.Rows[i].FindControl("txtDelieveredpieces")).Text.Trim());
                                    drULDDetails["ActualPieces"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtDelieveredpieces")).Text;
                                    drULDDetails["ActualWeight"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtDelieveredwt")).Text;
                                    DateTime dtdateU = DateTime.ParseExact(((TextBox)grdULDDelivery.Rows[i].FindControl("txtbookingdate")).Text, "dd/MM/yyyy", null);
                                    string sdateU = dtdateU.ToString("dd-MM-yyyy");
                                    drULDDetails["IssueDate"] = sdateU;// ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text;
                                    drULDDetails["AWBCount"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtAWBs")).Text;
                                    dtULDDetails.Rows.Add(drULDDetails);
                                    MultipleULDs += "," + drULDDetails["ULDNumber"];
                                }
                            }
                        }
                        if (ULDCount == 0)
                        {
                            for (int i = 0; i < grdULDDelivery.Rows.Count; i++)
                            {
                                if (((Label)grdULDDelivery.Rows[i].FindControl("txtuldsno")).Text.Trim() != "")
                                {
                                    DataRow drULDDetails = dtULDDetails.NewRow();
                                    drULDDetails["ULDNumber"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtuldsno")).Text.Trim();
                                    drULDDetails["FlightNumber"] = ((TextBox)grdULDDelivery.Rows[i].FindControl("txtflightno")).Text;
                                    Total += Convert.ToInt32(((Label)grdULDDelivery.Rows[i].FindControl("txtDelieveredpieces")).Text.Trim());
                                    drULDDetails["ActualPieces"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtDelieveredpieces")).Text;
                                    drULDDetails["ActualWeight"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtDelieveredwt")).Text;
                                    DateTime dtdateU = DateTime.ParseExact(((TextBox)grdULDDelivery.Rows[i].FindControl("txtbookingdate")).Text, "dd/MM/yyyy", null);
                                    string sdateU = dtdateU.ToString("dd-MM-yyyy");
                                    drULDDetails["IssueDate"] = sdateU;// ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text;
                                    drULDDetails["AWBCount"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtAWBs")).Text;
                                    dtULDDetails.Rows.Add(drULDDetails);
                                    MultipleULDs += "," + drULDDetails["ULDNumber"];
                                }

                            }
                        }
                        Session["MultipleULD"] = MultipleULDs;
                    }
                    catch (Exception)
                    {
                    }
                    #endregion

                    #region Gathering Header Information for DO Print
                    //Sangram 2014-10-30
                    try
                    {
                        string agentName = string.Empty, paymode = string.Empty, commcode = string.Empty;

                        int chkCountAWB = 0, chkdValue = 0;
                        for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                        {
                            if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("check")).Checked == true)
                            {
                                chkCountAWB++;
                            }
                        }

                        for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                        {
                            #region count 0
                            if (chkCountAWB == 0 || chkCountAWB == grdMaterialDetails.Rows.Count)
                            {

                                if (((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text != "")
                                {
                                    if (Convert.ToDecimal(((Label)grdMaterialDetails.Rows[i].FindControl("lblAmtDue")).Text) > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please pay amount due!');</SCRIPT>", false);
                                        return;
                                    }
                                    if (i == 0)
                                    {
                                        agentName = ((Label)grdMaterialDetails.Rows[i].FindControl("txtagentname")).Text;
                                        paymode = ((Label)grdMaterialDetails.Rows[i].FindControl("lblpaymenttype")).Text;
                                        commcode = ((Label)grdMaterialDetails.Rows[i].FindControl("lblCommCode")).Text;
                                    }
                                    if (i > 0)
                                    {
                                        if (agentName != ((Label)grdMaterialDetails.Rows[i].FindControl("txtagentname")).Text)
                                        {
                                            agentName = "";

                                        }
                                        if (paymode != ((Label)grdMaterialDetails.Rows[i].FindControl("lblpaymenttype")).Text)
                                        {
                                            paymode = "";

                                        }
                                        if (commcode != ((Label)grdMaterialDetails.Rows[i].FindControl("lblCommCode")).Text)
                                        {
                                            commcode = "";

                                        }
                                    }
                                }
                            }
                            #endregion

                            #region count > 0
                            else
                            {
                                if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("check")).Checked == true)
                                {
                                    chkdValue++;
                                    if (((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text != "")
                                    {
                                        if (Convert.ToDecimal(((Label)grdMaterialDetails.Rows[i].FindControl("lblAmtDue")).Text) > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please pay amount due!');</SCRIPT>", false);
                                            return;
                                        }
                                        if (chkdValue == 1)
                                        {
                                            agentName = ((Label)grdMaterialDetails.Rows[i].FindControl("txtagentname")).Text;
                                            paymode = ((Label)grdMaterialDetails.Rows[i].FindControl("lblpaymenttype")).Text;
                                            commcode = ((Label)grdMaterialDetails.Rows[i].FindControl("lblCommCode")).Text;
                                        }
                                        if (chkdValue > 1)
                                        {
                                            if (agentName != ((Label)grdMaterialDetails.Rows[i].FindControl("txtagentname")).Text)
                                            {
                                                agentName = "";
                                            }
                                            if (paymode != ((Label)grdMaterialDetails.Rows[i].FindControl("lblpaymenttype")).Text)
                                            {
                                                paymode = "";
                                            }
                                            if (commcode != ((Label)grdMaterialDetails.Rows[i].FindControl("lblCommCode")).Text)
                                            {
                                                commcode = "";

                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        DataRow drHeader = dtHeader.NewRow();
                        drHeader["DoNumber"] = txtDoNumber.Text;
                        drHeader["IssuedTo"] = txtissuedto.Text.Trim();
                        drHeader["IssueName"] = txtissuename.Text.Trim();
                        drHeader["ReciverName"] = txtreciversname.Text.Trim();
                        drHeader["IssueDate"] = txtissuedate.Text.Trim();
                        drHeader["StaffId"] = Session["UserName"].ToString();
                        drHeader["Station"] = Session["Station"].ToString();
                        drHeader["Consignee"] = txtconsignee.Text.Trim();
                        drHeader["ShipperName"] = Shippername;
                        if (Session["ST"] != null)
                            ST = Convert.ToDouble(Session["ST"].ToString());
                        else
                        {
                            MasterBAL objBal = new MasterBAL();
                            Session["ST"] = objBal.getServiceTax();
                            ST = Convert.ToDouble(Session["ST"].ToString());
                        }

                        drHeader["ServiceTax"] = ST;
                        drHeader["Consignee"] = txtconsignee.Text;
                        drHeader["Total"] = Total;
                        DataSet dsCustomerSupport = new DataSet("Dlv_PrintDeliveryOrderPDF_dsCustomerSupport");
                        try
                        {
                            dsCustomerSupport = da.SelectRecords("Sp_GetCustomerSupportInfo");
                            if (dsCustomerSupport.Tables[0].Rows.Count > 0)
                            {
                                CustomerSupport = dsCustomerSupport.Tables[0].Rows[0]["CustomerSupport"].ToString();
                                CustomerRules = dsCustomerSupport.Tables[0].Rows[0]["CustomerRules"].ToString();
                            }
                            else
                            {
                                CustomerSupport = "";
                            }
                        }
                        catch
                        {
                            CustomerSupport = "";
                        }
                        finally
                        {
                            if (dsCustomerSupport != null)
                            {
                                dsCustomerSupport.Dispose();
                            }
                        }
                        drHeader["Customer"] = CustomerSupport;
                        drHeader["Rules"] = CustomerRules;

                        //img for report
                        System.IO.MemoryStream Logo1 = null;
                        try
                        {
                            Logo1 = CommonUtility.GetImageStream(Page.Server);
                        }
                        catch (Exception)
                        {
                            Logo1 = new System.IO.MemoryStream();
                        }

                        drHeader["Logo"] = Logo1.ToArray();
                        drHeader["DODate"] = txtDODate.Text.Trim() == string.Empty ? ((DateTime)Session["IT"]).ToString("dd/MM/yyyy HH:mm") : txtDODate.Text.Trim();
                        drHeader["Remarks"] = txtDORemarks.Text.Trim();
                        drHeader["AgentName"] = agentName;
                        drHeader["PayMode"] = paymode;
                        drHeader["CommCode"] = commcode;
                        drHeader["AwbOrg"] = strHeadOrg;
                        drHeader["AwbDest"] = strHeadDest;
                        dtHeader.Rows.Add(drHeader);
                        if (Logo1 != null)
                        {
                            Logo1.Dispose();
                        }
                    }
                    catch (Exception)
                    {
                    }
                    #endregion

                    DataSet dset = new DataSet("GHA_Imp_Delivery_Print_dset");
                    dset.Tables.Add(dtHeader.Copy());
                    dset.Tables.Add(dtSurface.Copy());
                    dset.Tables.Add(dtULDDetails.Copy());

                    #endregion
                    if (dset != null)
                    {
                        //Session["abc"] = dset;
                        if (dset.Tables.Count > 0)
                        {
                            if (dset.Tables[0].Rows.Count > 0 && dset.Tables[1].Rows.Count > 0 || dset.Tables[0].Rows.Count > 0 && dset.Tables[2].Rows.Count > 0)
                            {
                                //dset = new DataSet("Table1");
                                //dset.Tables.Add(((DataTable)Session["abc"]).Copy());
                                //dset = (DataSet)Session["abc"];
                                if (dset != null)
                                {
                                    if (dset.Tables.Count > 0)
                                    {
                                        if (dset.Tables[0].Rows.Count > 0)
                                        {
                                            //string MultipleAWBs = string.Empty;
                                            //string MultipleULDs = string.Empty;
                                            string[] AWBNo;

                                            //if (Session["MultipleAWB"] != null)
                                            //{
                                                //MultipleAWBs = Session["MultipleAWB"].ToString();
                                                //char[] charSeparator = new char[] { ',' };
                                                //AWBNo = MultipleAWBs.Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);


                                            //}
                                            //if (Session["MultipleULD"] != null)
                                            //{
                                                //MultipleULDs = Session["MultipleULD"].ToString();

                                            //}
                                            string[] QueryNames = { "AWBNumber", "ULDNo" };
                                            SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar };
                                            object[] QueryValues = { MultipleAWBs, MultipleULDs };

                                            SQLServer db = new SQLServer(Global.GetConnectionString());
                                            DataSet dCharge = new DataSet("Dlv_PrintDeliveryOrderPDF_dCharge");
                                            dCharge = db.SelectRecords("sp_GetChargeHeadForDelivery", QueryNames, QueryValues, QueryTypes);
                                            if (dCharge != null)
                                            {
                                                dtTable4 = dCharge.Tables[0];
                                            }
                                            db = null;

                                            ReportViewer rptUCRReport = new ReportViewer();

                                            dtTable1 = dset.Tables[0];//dtHeader
                                            dtTable2 = dset.Tables[1];//dtSurface
                                            dtTable3 = dset.Tables[2];//dtULD
                                            //dtTable4 = dset.Tables[3];

                                            rptUCRReport.ProcessingMode = ProcessingMode.Local;

                                            LocalReport rep1 = rptUCRReport.LocalReport;

                                            rep1.ReportPath = Server.MapPath("/Reports/DeliveryOrder.rdlc");

                                            //rds1.Name = "dsDeliveryOrder_DataTable1";
                                            //rds1.Value = dtTable1;
                                            //rep1.DataSources.Add(rds2);
                                            //rds1.Name = "dsDeliveryOrder_DataTable2";
                                            //rds1.Value = dtTable2;
                                            //rep1.DataSources.Add(rds3);
                                            rds1.Name = "dsDeliveryOrder_DataTable3";
                                            rds1.Value = dtTable1;// dtTable2;
                                            rep1.DataSources.Add(rds1);
                                            rptUCRReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                                            #region Render to PDF
                                            try
                                            {
                                                string reportType = "PDF";
                                                string fileNameExtension;

                                                string deviceInfo = "<DeviceInfo><PageWidth>13.04in</PageWidth><PageHeight>10in</PageHeight></DeviceInfo>";


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
                                                string filename = Session["UserName"].ToString() + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".pdf";
                                                string FullName = filename.Replace(" ", "");
                                                Response.AddHeader("content-disposition", "attachment; filename=" + FullName);
                                                //Response.AddHeader("content-disposition", "attachment; filename=" + "DO" + "." + ".pdf");
                                                Response.BinaryWrite(bytes); // create the file
                                                Response.Flush();
                                                Response.Close();


                                                //Response.End();
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            #endregion

                                        }
                                    }
                                }
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Download();</SCRIPT>", false);
                            }
                            else
                            {
                                lblStatus.Text = "No AWB's selected to print DO";
                                lblStatus.ForeColor = Color.Red;
                                dset.Dispose();
                                return;
                            }
                        }
                        string[] paramname = new string[7];
                        object[] paramvalue = new object[7];

                        try
                        {
                            SqlDbType[] paramtype = new SqlDbType[7];
                            paramtype[0] = SqlDbType.VarChar;
                            paramtype[1] = SqlDbType.VarChar;
                            paramtype[2] = SqlDbType.VarChar;
                            paramtype[3] = SqlDbType.VarChar;
                            paramtype[4] = SqlDbType.VarChar;
                            paramtype[5] = SqlDbType.DateTime;
                            paramtype[6] = SqlDbType.VarChar;

                            /*************************** Set Param Name ********************************/
                            paramname[0] = "DoNumber";
                            paramname[1] = "IssuedTo";
                            paramname[2] = "IssueName";
                            paramname[3] = "ReciverName";
                            paramname[4] = "Remarks";
                            paramname[5] = "UpdateOn";//Add byPawan 
                            paramname[6] = "UpdateBy";


                            paramvalue[0] = txtDoNumber.Text.Trim();// "DoNumber";
                            paramvalue[1] = strIssueTo;// "IssuedTo";
                            paramvalue[2] = strIssueName;// "IssueName";
                            paramvalue[3] = txtreciversname.Text.Trim();// "ReciverName";
                            paramvalue[4] = txtDORemarks.Text.Trim();
                            paramvalue[5] = Convert.ToDateTime(Session["IT"]).ToString("MM/dd/yyyy HH:mm:ss");
                            paramvalue[6] = Convert.ToString(Session["UserName"]);
                            if (txtDoNumber.Text.Trim() != "")
                                da.InsertData("SP_UpdateDOPrint", paramname, paramtype, paramvalue);
                        }
                        catch { }
                        if (dset != null)
                            dset.Dispose();
                    }
                }
                catch (Exception e)
                {
                    lblStatus.Text = "Error while printing DO: " + e.Message + " Please try again";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                finally
                {
                    if (dtHeader != null)
                    {
                        dtHeader.Dispose();
                    }
                    if (dtSurface != null)
                    {
                        dtSurface.Dispose();
                    }
                    if (dtULDDetails != null)
                    {
                        dtULDDetails.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }

        protected void PrintDeliveryOrderPDFX()
        {
            try
            {
                lblStatus.Text = "";
                Session["MultipleAWB"] = null;
                Session["MultipleULD"] = null;
                DataTable dtHeader = new DataTable("Dlv_PrintdeliveryOrderPDFX_dtHeader");
                DataTable dtULDDetails = new DataTable("Dlv_PrintdeliveryOrderPDFX_dtULDDetails");
                DataTable dtSurface = new DataTable("Dlv_PrintdeliveryOrderPDFX_dtSurface");
                try
                {
                    #region Added New Code Deepak
                    string CustomerSupport = string.Empty;
                    string CustomerRules = string.Empty;
                    double ST = 0;

                    dtHeader.Columns.Add("DoNumber", typeof(string));
                    dtHeader.Columns.Add("IssuedTo", typeof(string));
                    dtHeader.Columns.Add("IssueName", typeof(string));
                    dtHeader.Columns.Add("ReciverName", typeof(string));
                    dtHeader.Columns.Add("IssueDate", typeof(string));
                    dtHeader.Columns.Add("StaffId", typeof(string));
                    dtHeader.Columns.Add("Customer", typeof(string));
                    dtHeader.Columns.Add("Station", typeof(string));
                    dtHeader.Columns.Add("ServiceTax", typeof(double));
                    dtHeader.Columns.Add("Total", typeof(int));
                    dtHeader.Columns.Add("ChargeHead", typeof(string));
                    dtHeader.Columns.Add("Charge", typeof(string));
                    dtHeader.Columns.Add("Consignee", typeof(string));
                    dtHeader.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                    dtHeader.Columns.Add("DODate", typeof(string));
                    dtHeader.Columns.Add("Remarks", typeof(string));
                    dtHeader.Columns.Add("AgentName", typeof(string));
                    dtHeader.Columns.Add("PayMode", typeof(string));
                    dtHeader.Columns.Add("CommCode", typeof(string));
                    dtHeader.Columns.Add("Rules", typeof(string));
                    dtHeader.Columns.Add("ShipperName", typeof(string));

                    dtULDDetails.Columns.Add("ULDNumber", typeof(string));
                    dtULDDetails.Columns.Add("AWBCount", typeof(string));
                    dtULDDetails.Columns.Add("FlightNumber", typeof(string));
                    dtULDDetails.Columns.Add("ActualPieces", typeof(string));
                    dtULDDetails.Columns.Add("ActualWeight", typeof(string));
                    dtULDDetails.Columns.Add("IssueDate", typeof(string));

                    dtSurface.Columns.Add("DoNumber", typeof(string));
                    dtSurface.Columns.Add("AWBNumber", typeof(string));
                    dtSurface.Columns.Add("HAWBNo", typeof(string));
                    dtSurface.Columns.Add("FlightNumber", typeof(string));
                    dtSurface.Columns.Add("IssuedTo", typeof(string));
                    dtSurface.Columns.Add("IssueName", typeof(string));
                    dtSurface.Columns.Add("ReciverName", typeof(string));
                    dtSurface.Columns.Add("ActualPieces", typeof(string));
                    dtSurface.Columns.Add("ActualWeight", typeof(string));
                    dtSurface.Columns.Add("IssueDate", typeof(string));
                    dtSurface.Columns.Add("AgentName", typeof(string));
                    dtSurface.Columns.Add("Discription", typeof(string));
                    dtSurface.Columns.Add("ConsigneeName", typeof(string));
                    dtSurface.Columns.Add("CCAmount", typeof(float));
                    dtSurface.Columns.Add("PayMode", typeof(string));
                    dtSurface.Columns.Add("CommCode", typeof(string));
                    dtSurface.Columns.Add("DODate", typeof(string));
                    dtSurface.Columns.Add("StaffId", typeof(string));
                    dtSurface.Columns.Add("Station", typeof(string));
                    dtSurface.Columns.Add("CustomerSupport", typeof(string));
                    dtSurface.Columns.Add("ServiceTax", typeof(double));
                    dtSurface.Columns.Add("Remarks", typeof(string));
                    dtSurface.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                    dtSurface.Columns.Add("Total", typeof(int));
                    dtSurface.Columns.Add("ShipperName", typeof(string));

                    int Total = 0;
                    int Count = 0;
                    int ULDCount = 0;
                    #region Getting AWB Info for DO Print

                    DataRow drSurface = null;
                    string MultipleAWBs = string.Empty;
                    for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                    {
                        if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("check")).Checked == true)
                        {
                            if (Convert.ToDecimal(((Label)grdMaterialDetails.Rows[i].FindControl("lblAmtDue")).Text) > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please Pay amount due!');</SCRIPT>", false);
                                return;
                            }
                            Count++;
                            if (((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text != "")
                            {
                                string AWBNumber = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;
                                DataSet dscomm = new DataSet("Dlv_PrintDeliveryOrderPDFX_dscomm");
                                dscomm = da.SelectRecords("Sp_GetCommodity_V1", "AWBNumber", AWBNumber, SqlDbType.VarChar);
                                drSurface = dtSurface.NewRow();
                                try
                                {
                                    if (dscomm != null)
                                    {
                                        if (dscomm.Tables.Count > 0)
                                        {
                                            if (dscomm.Tables[0].Rows.Count > 0)
                                            {
                                                drSurface["CommCode"] = dscomm.Tables[0].Rows[0]["CommodityCode"].ToString();
                                            }
                                            if (dscomm.Tables[1].Rows.Count > 0)
                                            {
                                                drSurface["CCAmount"] = dscomm.Tables[1].Rows[0]["Total"].ToString();
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                { }
                                finally
                                {
                                    if (dscomm != null)
                                    {
                                        dscomm.Dispose();
                                    }
                                }
                                //drSurface["DoNumber"] = str;
                                drSurface["DoNumber"] = ((HyperLink)grdMaterialDetails.Rows[i].FindControl("hlnkDONumber")).Text;
                                drSurface["AWBNumber"] = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;
                                drSurface["HAWBNo"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txthawbs")).Text.Trim();
                                drSurface["FlightNumber"] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtflightno")).Text;
                                drSurface["IssuedTo"] = txtissuedto.Text.Trim();
                                drSurface["IssueName"] = txtissuename.Text.Trim();
                                drSurface["ReciverName"] = txtreciversname.Text.Trim();
                                Total += Convert.ToInt32(((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredpieces")).Text.Trim());
                                drSurface["ActualPieces"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredpieces")).Text + "/" + ((Label)grdMaterialDetails.Rows[i].FindControl("txtoriginalpieces")).Text;
                                drSurface["ActualWeight"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredwt")).Text;
                                DateTime dtdate = DateTime.ParseExact(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text, "dd/MM/yyyy", null);
                                string sdate = dtdate.ToString("dd/MM/yyyy");
                                drSurface["IssueDate"] = sdate;// ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text;
                                drSurface["AgentName"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtagentname")).Text;
                                drSurface["Discription"] = ((Label)grdMaterialDetails.Rows[i].FindControl("lblDiscription")).Text;
                                drSurface["ConsigneeName"] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtConsigneeName")).Text;

                                drSurface["PayMode"] = ((Label)grdMaterialDetails.Rows[i].FindControl("lblpaymenttype")).Text;

                                //Added extra by Vijay
                                drSurface["StaffId"] = Session["UserName"].ToString();
                                drSurface["Station"] = Session["Station"].ToString();
                                if (Session["ST"] != null)
                                    ST = Convert.ToDouble(Session["ST"].ToString());
                                else
                                {
                                    MasterBAL objBal = new MasterBAL();
                                    Session["ST"] = objBal.getServiceTax();
                                    ST = Convert.ToDouble(Session["ST"].ToString());
                                }

                                drSurface["ServiceTax"] = ST;
                                DataSet dsCustomer = new DataSet("Dlv_PrintDeliveryOrderPDFX_dsCustomer");
                                try
                                {
                                    dsCustomer = da.SelectRecords("Sp_GetCustomerSupportInfo");
                                    if (dsCustomer.Tables[0].Rows.Count > 0)
                                    {
                                        CustomerSupport = dsCustomer.Tables[0].Rows[0]["CustomerSupport"].ToString();
                                    }
                                    else
                                    {
                                        CustomerSupport = "";
                                    }
                                }
                                catch
                                {
                                    CustomerSupport = "";
                                }
                                finally
                                {
                                    if (dsCustomer != null)
                                    {
                                        dsCustomer.Dispose();
                                    }
                                }
                                drSurface["CustomerSupport"] = CustomerSupport;
                                drSurface["Remarks"] = txtDORemarks.Text.Trim();
                                drSurface["DODate"] = txtDODate.Text.Trim() == string.Empty ? ((DateTime)Session["IT"]).ToString("dd/MM/yyyy HH:mm") : txtDODate.Text.Trim();


                                System.IO.MemoryStream Logo2 = null;
                                try
                                {
                                    Logo2 = CommonUtility.GetImageStream(Page.Server);
                                }
                                catch (Exception)
                                {
                                    Logo2 = new System.IO.MemoryStream();
                                }

                                drSurface["Logo"] = Logo2.ToArray();

                                if (Logo2 != null)
                                {
                                    Logo2.Dispose();
                                }
                                //Added extra by Vijay


                                dtSurface.Rows.Add(drSurface);
                                MultipleAWBs += "," + drSurface["AWBNumber"];
                            }
                        }
                    }

                    if (Count == 0)
                    {
                        for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                        {
                            if (((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text != "")
                            {
                                if (Convert.ToDecimal(((Label)grdMaterialDetails.Rows[i].FindControl("lblAmtDue")).Text) > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please pay amount due!');</SCRIPT>", false);
                                    return;
                                }
                                string AWBNumber = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;
                                DataSet dscomm = new DataSet("Dlv_PrintDeliveryOrderPDFX_dscomm");
                                dscomm = da.SelectRecords("Sp_GetCommodity_V1", "AWBNumber", AWBNumber, SqlDbType.VarChar);
                                drSurface = dtSurface.NewRow();
                                try
                                {
                                    if (dscomm != null)
                                    {
                                        if (dscomm.Tables.Count > 0)
                                        {
                                            if (dscomm.Tables[0].Rows.Count > 0)
                                            {
                                                drSurface["CommCode"] = dscomm.Tables[0].Rows[0]["CommodityCode"].ToString();
                                            }
                                            if (dscomm.Tables[1].Rows.Count > 0)
                                            {
                                                drSurface["CCAmount"] = dscomm.Tables[1].Rows[0]["Total"].ToString();
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                { }
                                finally
                                {
                                    if (dscomm != null)
                                    {
                                        dscomm.Dispose();
                                    }
                                }
                                //drSurface["DoNumber"] = str;
                                drSurface["DoNumber"] = ((HyperLink)grdMaterialDetails.Rows[i].FindControl("hlnkDONumber")).Text;
                                drSurface["AWBNumber"] = ((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text;
                                drSurface["HAWBNo"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txthawbs")).Text.Trim();
                                drSurface["FlightNumber"] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtflightno")).Text;
                                drSurface["IssuedTo"] = txtissuedto.Text.Trim();
                                drSurface["IssueName"] = txtissuename.Text.Trim();
                                drSurface["ReciverName"] = txtreciversname.Text.Trim();
                                Total += Convert.ToInt32(((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredpieces")).Text.Trim());
                                drSurface["ActualPieces"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredpieces")).Text + "/" + ((Label)grdMaterialDetails.Rows[i].FindControl("txtoriginalpieces")).Text;
                                drSurface["ActualWeight"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredwt")).Text;
                                DateTime dtdate = DateTime.ParseExact(((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text, "dd/MM/yyyy", null);
                                string sdate = dtdate.ToString("dd/MM/yyyy");
                                drSurface["IssueDate"] = sdate;// ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text;
                                drSurface["AgentName"] = ((Label)grdMaterialDetails.Rows[i].FindControl("txtagentname")).Text;
                                drSurface["Discription"] = ((Label)grdMaterialDetails.Rows[i].FindControl("lblDiscription")).Text;
                                drSurface["ConsigneeName"] = ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtConsigneeName")).Text;

                                drSurface["PayMode"] = ((Label)grdMaterialDetails.Rows[i].FindControl("lblpaymenttype")).Text;

                                //Added extra by Vijay
                                drSurface["StaffId"] = Session["UserName"].ToString();
                                drSurface["Station"] = Session["Station"].ToString();
                                if (Session["ST"] != null)
                                    ST = Convert.ToDouble(Session["ST"].ToString());
                                else
                                {
                                    MasterBAL objBal = new MasterBAL();
                                    Session["ST"] = objBal.getServiceTax();
                                    ST = Convert.ToDouble(Session["ST"].ToString());
                                }

                                drSurface["ServiceTax"] = ST;
                                DataSet dsCustomer = new DataSet("Dlv_PrintDeliveryOrderPDFX_dsCustomer");
                                try
                                {
                                    dsCustomer = da.SelectRecords("Sp_GetCustomerSupportInfo");
                                    if (dsCustomer.Tables[0].Rows.Count > 0)
                                    {
                                        CustomerSupport = dsCustomer.Tables[0].Rows[0]["CustomerSupport"].ToString();
                                    }
                                    else
                                    {
                                        CustomerSupport = "";
                                    }
                                }
                                catch
                                {
                                    CustomerSupport = "";
                                }
                                finally
                                {
                                    if (dsCustomer != null)
                                    {
                                        dsCustomer.Dispose();
                                    }
                                }
                                drSurface["CustomerSupport"] = CustomerSupport;
                                drSurface["Remarks"] = txtDORemarks.Text.Trim();
                                drSurface["DODate"] = txtDODate.Text.Trim() == string.Empty ? dtCurrentDate.ToString("dd/MM/yyyy HH:mm") : txtDODate.Text.Trim();

                                System.IO.MemoryStream Logo2 = null;
                                try
                                {
                                    Logo2 = CommonUtility.GetImageStream(Page.Server);
                                }
                                catch (Exception)
                                {
                                    Logo2 = new System.IO.MemoryStream();
                                }

                                drSurface["Logo"] = Logo2.ToArray();

                                if (Logo2 != null)
                                {
                                    Logo2.Dispose();
                                }

                                //Added extra by Vijay

                                dtSurface.Rows.Add(drSurface);
                                MultipleAWBs += "," + drSurface["AWBNumber"];
                            }
                        }
                    }
                    for (int f = 0; f < dtSurface.Rows.Count; f++)
                    {
                        dtSurface.Rows[f]["Total"] = Total;
                    }
                    Session["MultipleAWB"] = MultipleAWBs;

                    drSurface = null;
                    #endregion

                    #region Getting ULD Info for DO Print
                    string MultipleULDs = string.Empty;
                    //Sangram 2014-10-30
                    try
                    {
                        for (int i = 0; i < grdULDDelivery.Rows.Count; i++)
                        {
                            if (((CheckBox)grdULDDelivery.Rows[i].FindControl("ULDcheck")).Checked == true)
                            {
                                ULDCount++;
                                if (((Label)grdULDDelivery.Rows[i].FindControl("txtuldsno")).Text.Trim() != "")
                                {
                                    DataRow drULDDetails = dtULDDetails.NewRow();
                                    drULDDetails["ULDNumber"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtuldsno")).Text.Trim();
                                    drULDDetails["FlightNumber"] = ((TextBox)grdULDDelivery.Rows[i].FindControl("txtflightno")).Text;
                                    Total += Convert.ToInt32(((Label)grdULDDelivery.Rows[i].FindControl("txtDelieveredpieces")).Text.Trim());
                                    drULDDetails["ActualPieces"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtDelieveredpieces")).Text;
                                    drULDDetails["ActualWeight"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtDelieveredwt")).Text;
                                    DateTime dtdateU = DateTime.ParseExact(((TextBox)grdULDDelivery.Rows[i].FindControl("txtbookingdate")).Text, "dd/MM/yyyy", null);
                                    string sdateU = dtdateU.ToString("dd-MM-yyyy");
                                    drULDDetails["IssueDate"] = sdateU;// ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text;
                                    drULDDetails["AWBCount"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtAWBs")).Text;
                                    dtULDDetails.Rows.Add(drULDDetails);
                                    MultipleULDs += "," + drULDDetails["ULDNumber"];
                                }
                            }
                        }
                        if (ULDCount == 0)
                        {
                            for (int i = 0; i < grdULDDelivery.Rows.Count; i++)
                            {
                                if (((Label)grdULDDelivery.Rows[i].FindControl("txtuldsno")).Text.Trim() != "")
                                {
                                    DataRow drULDDetails = dtULDDetails.NewRow();
                                    drULDDetails["ULDNumber"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtuldsno")).Text.Trim();
                                    drULDDetails["FlightNumber"] = ((TextBox)grdULDDelivery.Rows[i].FindControl("txtflightno")).Text;
                                    Total += Convert.ToInt32(((Label)grdULDDelivery.Rows[i].FindControl("txtDelieveredpieces")).Text.Trim());
                                    drULDDetails["ActualPieces"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtDelieveredpieces")).Text;
                                    drULDDetails["ActualWeight"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtDelieveredwt")).Text;
                                    DateTime dtdateU = DateTime.ParseExact(((TextBox)grdULDDelivery.Rows[i].FindControl("txtbookingdate")).Text, "dd/MM/yyyy", null);
                                    string sdateU = dtdateU.ToString("dd-MM-yyyy");
                                    drULDDetails["IssueDate"] = sdateU;// ((TextBox)grdMaterialDetails.Rows[i].FindControl("txtbookingdate")).Text;
                                    drULDDetails["AWBCount"] = ((Label)grdULDDelivery.Rows[i].FindControl("txtAWBs")).Text;
                                    dtULDDetails.Rows.Add(drULDDetails);
                                    MultipleULDs += "," + drULDDetails["ULDNumber"];
                                }

                            }
                        }
                        Session["MultipleULD"] = MultipleULDs;
                    }
                    catch (Exception)
                    {
                    }
                    #endregion

                    #region Gathering Header Information for DO Print

                    try
                    {
                        string agentName = string.Empty, paymode = string.Empty, commcode = string.Empty;

                        int chkCountAWB = 0, chkdValue = 0;
                        for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                        {
                            if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("check")).Checked == true)
                            {
                                chkCountAWB++;
                            }
                        }

                        for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                        {
                            #region count 0
                            if (chkCountAWB == 0 || chkCountAWB == grdMaterialDetails.Rows.Count)
                            {

                                if (((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text != "")
                                {
                                    if (Convert.ToDecimal(((Label)grdMaterialDetails.Rows[i].FindControl("lblAmtDue")).Text) > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please pay amount due!');</SCRIPT>", false);
                                        return;
                                    }
                                    if (i == 0)
                                    {
                                        agentName = ((Label)grdMaterialDetails.Rows[i].FindControl("txtagentname")).Text;
                                        paymode = ((Label)grdMaterialDetails.Rows[i].FindControl("lblpaymenttype")).Text;
                                        commcode = ((Label)grdMaterialDetails.Rows[i].FindControl("lblCommCode")).Text;
                                    }
                                    if (i > 0)
                                    {
                                        if (agentName != ((Label)grdMaterialDetails.Rows[i].FindControl("txtagentname")).Text)
                                        {
                                            agentName = "";

                                        }
                                        if (paymode != ((Label)grdMaterialDetails.Rows[i].FindControl("lblpaymenttype")).Text)
                                        {
                                            paymode = "";

                                        }
                                        if (commcode != ((Label)grdMaterialDetails.Rows[i].FindControl("lblCommCode")).Text)
                                        {
                                            commcode = "";

                                        }
                                    }
                                }
                            }
                            #endregion

                            #region count > 0
                            else
                            {
                                if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("check")).Checked == true)
                                {
                                    chkdValue++;
                                    if (((LinkButton)grdMaterialDetails.Rows[i].FindControl("txtawbno")).Text != "")
                                    {
                                        if (Convert.ToDecimal(((Label)grdMaterialDetails.Rows[i].FindControl("lblAmtDue")).Text) > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please pay amount due!');</SCRIPT>", false);
                                            return;
                                        }
                                        if (chkdValue == 1)
                                        {
                                            agentName = ((Label)grdMaterialDetails.Rows[i].FindControl("txtagentname")).Text;
                                            paymode = ((Label)grdMaterialDetails.Rows[i].FindControl("lblpaymenttype")).Text;
                                            commcode = ((Label)grdMaterialDetails.Rows[i].FindControl("lblCommCode")).Text;
                                        }
                                        if (chkdValue > 1)
                                        {
                                            if (agentName != ((Label)grdMaterialDetails.Rows[i].FindControl("txtagentname")).Text)
                                            {
                                                agentName = "";
                                            }
                                            if (paymode != ((Label)grdMaterialDetails.Rows[i].FindControl("lblpaymenttype")).Text)
                                            {
                                                paymode = "";
                                            }
                                            if (commcode != ((Label)grdMaterialDetails.Rows[i].FindControl("lblCommCode")).Text)
                                            {
                                                commcode = "";

                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        DataRow drHeader = dtHeader.NewRow();
                        drHeader["DoNumber"] = txtDoNumber.Text;
                        drHeader["IssuedTo"] = txtissuedto.Text.Trim();
                        drHeader["IssueName"] = txtissuename.Text.Trim();
                        drHeader["ReciverName"] = txtreciversname.Text.Trim();
                        drHeader["IssueDate"] = txtissuedate.Text.Trim();
                        drHeader["StaffId"] = Session["UserName"].ToString();
                        drHeader["Station"] = Session["Station"].ToString();
                        drHeader["Consignee"] = txtconsignee.Text.Trim();

                        if (Session["ST"] != null)
                            ST = Convert.ToDouble(Session["ST"].ToString());
                        else
                        {
                            MasterBAL objBal = new MasterBAL();
                            Session["ST"] = objBal.getServiceTax();
                            ST = Convert.ToDouble(Session["ST"].ToString());
                        }

                        drHeader["ServiceTax"] = ST;
                        drHeader["Consignee"] = txtconsignee.Text;
                        drHeader["Total"] = Total;
                        DataSet dsCustomerSupport = new DataSet("Dlv_PrintDeliveryOrderPDFX_dsCustomerSupport");
                        try
                        {
                            dsCustomerSupport = da.SelectRecords("Sp_GetCustomerSupportInfo");
                            if (dsCustomerSupport.Tables[0].Rows.Count > 0)
                            {
                                CustomerSupport = dsCustomerSupport.Tables[0].Rows[0]["CustomerSupport"].ToString();
                                CustomerRules = dsCustomerSupport.Tables[0].Rows[0]["CustomerRules"].ToString();
                            }
                            else
                            {
                                CustomerSupport = "";
                            }
                        }
                        catch
                        {
                            CustomerSupport = "";
                        }
                        finally
                        {
                            if (dsCustomerSupport != null)
                            {
                                dsCustomerSupport.Dispose();
                            }
                        }
                        drHeader["Customer"] = CustomerSupport;
                        drHeader["Rules"] = CustomerRules;

                        //img for report
                        System.IO.MemoryStream Logo1 = null;
                        try
                        {
                            Logo1 = CommonUtility.GetImageStream(Page.Server);
                        }
                        catch (Exception)
                        {
                            Logo1 = new System.IO.MemoryStream();
                        }

                        drHeader["Logo"] = Logo1.ToArray();

                        drHeader["DODate"] = txtDODate.Text.Trim() == string.Empty ? ((DateTime)Session["IT"]).ToString("dd/MM/yyyy HH:mm") : txtDODate.Text.Trim();
                        drHeader["Remarks"] = txtDORemarks.Text.Trim();
                        drHeader["AgentName"] = agentName;
                        drHeader["PayMode"] = paymode;
                        drHeader["CommCode"] = commcode;

                        dtHeader.Rows.Add(drHeader);
                        if (Logo1 != null)
                        {
                            Logo1.Dispose();
                        }
                 
                    }
                    catch (Exception)
                    {
                    }
                   #endregion
                
                    DataSet dset = new DataSet("Dlv_PrintDeliveryOrderPDFX_dset");
                    dset.Tables.Add(dtHeader.Copy());
                    dset.Tables.Add(dtSurface.Copy());
                    dset.Tables.Add(dtULDDetails.Copy());

                    #endregion
                    if (dset != null)
                    {
                        Session["abc"] = dset;
                        if (dset.Tables.Count > 0)
                        {
                            if (dset.Tables[0].Rows.Count > 0 && dset.Tables[1].Rows.Count > 0 || dset.Tables[0].Rows.Count > 0 && dset.Tables[2].Rows.Count > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Download();</SCRIPT>", false);
                            }
                            else
                            {
                                lblStatus.Text = "No AWB's selected to print DO";
                                lblStatus.ForeColor = Color.Red;
                                dset.Dispose();
                                return;
                            }
                        }
                        dset.Dispose();
                    }
                }
                catch (Exception e)
                {
                    lblStatus.Text = "Error while printing DO: " + e.Message + " Please try again";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                finally
                {
                    if (dtHeader != null)
                    {
                        dtHeader.Dispose();
                    }
                    if (dtSurface != null)
                    {
                        dtSurface.Dispose();
                    }
                    if (dtULDDetails != null)
                    {
                        dtULDDetails.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }

        #region Load Token DropDown

        public void LoadTokenDropDown()
        {
            try
            {
                lblStatus.Text = "";

                SQLServer dbase = new SQLServer(Global.GetConnectionString());
                DataSet ds = new DataSet("Dlv_LoadTokenDropDown_ds");
                ds = dbase.SelectRecords("sp_GetGatePassNoDelivery");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlTokenList.DataSource = ds;
                            ddlTokenList.DataTextField = "TNKNumber";
                            ddlTokenList.DataValueField = "TNKNumber";
                            ddlTokenList.DataBind();
                            ddlTokenList.Items.Insert(0, "Select");
                        }
                        else
                        { ddlTokenList.Items.Insert(0, "Select"); }
                    }
                    else
                    { ddlTokenList.Items.Insert(0, "Select"); }
                }
                else
                { ddlTokenList.Items.Insert(0, "Select"); }
                ds.Dispose();
                dbase = null;
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region btnOpsTime_Click
        protected void btnOpsTime_Click(object sender, ImageClickEventArgs e)
        {
            SaveOperationTime(true);
        }
        #endregion btnOpsTime_Click

        protected void btnEPouch_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                int rowChk = 0;
                string awbno;
                foreach (GridViewRow row in grdMaterialDetails.Rows)
                {
                    if (((CheckBox)row.FindControl("check")).Checked)
                    {
                        awbno = ((LinkButton)row.FindControl("txtawbno")).Text.Replace("-", "");
                        Session["ePouchAWBNo"] = awbno.Trim();
                        rowChk++;
                    }
                }
                if (rowChk == 1)
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('ePouchNew.aspx','_blank')", true);
                if (rowChk <= 0)
                {
                    lblStatus.Text = "Select atleast one row";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (rowChk > 1)
                {
                    lblStatus.Text = "Select only one row";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex)
            { }

        }

        #region AWB Modify
        protected void LnkModify_Click(object sender, EventArgs e)
        {
            DataSet res = new DataSet("Dlv_LnkModify_res");
            try
            {
                int rowid = -1;
                for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                {   //Find out selected row for editing.
                    if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("check")).Checked)
                    {
                        rowid = i;
                        break;
                    }
                }
                if (rowid == -1)
                {
                    lblStatus.Text = "Please select row which you want to modify.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                GridViewRow row = grdMaterialDetails.Rows[rowid];
                string AWBNo = ((LinkButton)row.FindControl("txtawbno")).Text;
                string FltNo = ((TextBox)row.FindControl("txtflightno")).Text;
                string DONumber = ((HyperLink)row.FindControl("hlnkDONumber")).Text;

                ////editgrid = AWBNo;
                ////Session["editgrid"] = AWBNo;


                ((TextBox)grdMaterialDetails.Rows[rowid].FindControl("txtactualpieces")).Enabled = true;
                ((TextBox)grdMaterialDetails.Rows[rowid].FindControl("txtactualwt")).Enabled = true;
                //((TextBox)grdMaterialDetails.Rows[rowid].FindControl("txtDelieveredpieces")).Enabled = true;
                //((TextBox)grdMaterialDetails.Rows[rowid].FindControl("txtDelieveredwt")).Enabled = true;
                //((TextBox)grdMaterialDetails.Rows[rowid].FindControl("txtexpectedpieces")).Enabled = true;
                //((TextBox)grdMaterialDetails.Rows[rowid].FindControl("txtgrwt")).Enabled = true;
                //((DropDownList)grdMaterialDetails.Rows[rowid].FindControl("ddlDiscrepancy")).Enabled = true;

            }
            catch (Exception)
            {

            }
        }
        #endregion AWB Modify

        #region Button ReOpen DO
        protected void btnReOpenDO_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;

                int checkedCount = 0;
                int MakeDisable = -1;
                string ReopenedDoNo = "";
                for (int i = 0; i < grdMaterialDetails.Rows.Count; i++)
                {
                    if (((CheckBox)grdMaterialDetails.Rows[i].FindControl("check")).Checked == true
                        && ((HyperLink)grdMaterialDetails.Rows[i].FindControl("hlnkDONumber")).Text != ""
                        )
                    {
                        checkedCount++;
                        ReopenedDoNo = ((HyperLink)grdMaterialDetails.Rows[i].FindControl("hlnkDONumber")).Text;
                        ViewState["IntRemainingPcs"] = Convert.ToInt32(((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredpieces")).Text.Trim());
                        ViewState["IntRemainingWt"] = Convert.ToDecimal(((Label)grdMaterialDetails.Rows[i].FindControl("txtDelieveredwt")).Text.Trim());
                    }
                    if (((HyperLink)grdMaterialDetails.Rows[i].FindControl("hlnkDONumber")).Text == "")
                    {
                        MakeDisable = i;
                    }
                }
                if (checkedCount == 0)
                {
                    lblStatus.Text = "Please select DO Number to ReOpen.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (checkedCount > 1)
                {
                    lblStatus.Text = "Please check only single DO Number.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (MakeDisable > -1)
                {
                    ((TextBox)grdMaterialDetails.Rows[MakeDisable].FindControl("txtactualwt")).Enabled = false;
                    ((TextBox)grdMaterialDetails.Rows[MakeDisable].FindControl("txtactualpieces")).Enabled = false;
                }

                EnableDeliveryPcsandWt(true, true);
                ViewState["DOReopen"] = "Y";
                ViewState["ReopenedDoNo"] = ReopenedDoNo;
                lblStatus.Text = "DO Reopened :" + ReopenedDoNo;
                lblStatus.ForeColor = Color.Green;
            }
            catch
            { }
        }
        #endregion

        #region Enable Disable Delivery Pcs & Wt

        public int EnableDeliveryPcsandWt(bool Enable, bool CheckBoxCheck)
        {
            try
            {
                lblStatus.Text = string.Empty;
                int count = 0;
                foreach (GridViewRow row in grdMaterialDetails.Rows)
                {
                    if (CheckBoxCheck)
                    {
                        if (((CheckBox)row.FindControl("check")).Checked)
                        {
                            if (((HyperLink)row.FindControl("hlnkDONumber")).Text != "")
                            {
                                count++;
                                ((TextBox)row.FindControl("txtactualpieces")).Enabled = Enable;
                                ((TextBox)row.FindControl("txtactualwt")).Enabled = Enable;
                            }
                        }
                    }
                    else
                    {
                        if (((HyperLink)row.FindControl("hlnkDONumber")).Text != "")
                        {

                            count++;
                            ((TextBox)row.FindControl("txtactualpieces")).Enabled = Enable;
                            ((TextBox)row.FindControl("txtactualwt")).Enabled = Enable;

                        }
                    }
                }
                return count;

            }
            catch (Exception ex)
            { return 0; }
        }
        #endregion

        #region Button Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/GHA_Imp_Delivery.aspx",false);
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region AWBNo_TextChanged
        protected void txtAWBNo_TextChanged(object sender, EventArgs e)
        {
            btnList_Click(null, null);
        }

        #endregion

        #region grdMaterialDetails RowDataBound
        protected void grdMaterialDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HyperLink hl = (HyperLink)e.Row.FindControl("hlnkCollection");

                    hl.NavigateUrl = "javascript:void window.open('" + hl.NavigateUrl + 
                        "','','height=440,width=1150,left=150,top=150,screenX=0,screenY=100,"+
                        "scrollbars=yes,resizable=yes');";
                }

                //string AWBNo = ((LinkButton)e.Row.FindControl("txtawbno")).Text.Trim();
                //Repeater rptInvoice = ((Repeater)e.Row.FindControl("rptInvoiceNumber"));
                //Repeater rptCollection = ((Repeater)e.Row.FindControl("rptCollect"));
                //DataSet ds = new DataSet();
                //ds = da.SelectRecords("spGetRepeaterDetailsDelivery", "AWBNo", AWBNo, SqlDbType.VarChar);
                //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    rptInvoice.DataSource = ds;
                //    rptInvoice.DataBind();
                //    rptCollection.DataSource = ds;
                //    rptCollection.DataBind();

                //}

                //if (ViewState["DOReopen"].ToString() == "Y")
                //{
                //    ((TextBox)e.Row.FindControl("txtactualpieces")).Enabled = false; ;
                //    ((TextBox)e.Row.FindControl("txtactualwt")).Enabled=false;;
                //}
            }
            catch (Exception ex)
            { }

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

        #region Clear Delivery Info
        public void ClearDeliveryInfo()
        {
            txtissuedate.Text = Session["IT"] != null ? ((DateTime)Session["IT"]).ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy");
            txtreciversname.Text = string.Empty;
            txtconsignee.Text = string.Empty;
            txtissuedto.Text = string.Empty;
            txtissuename.Text = string.Empty;
            txtDORemarks.Text = string.Empty;
        }
        #endregion

        protected void grdULDDelivery_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdMaterialDetails_RowDataBound1(object sender, GridViewRowEventArgs e)
        {

        }
        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsDeliveryOrder_DataTable4", dtTable4));
            e.DataSources.Add(new ReportDataSource("dsDeliveryOrder_DataTable2", dtTable3));
            e.DataSources.Add(new ReportDataSource("dsDeliveryOrder_DataTable1", dtTable2)); //Amount,PayMode Removed From AWB DO Subreport
        }
    }
}
 