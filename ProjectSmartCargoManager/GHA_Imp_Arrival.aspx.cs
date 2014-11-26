using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using System.Drawing;
using System.Collections;
using BAL;
using System.Text;
using SCM.Common.Struct;
using Microsoft.Reporting.WebForms;
namespace ProjectSmartCargoManager
{
    //28-7-2012
    //1-10-2012
    public partial class GHA_Imp_Arrival : System.Web.UI.Page
    {

        #region Variable Declaration
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BLArrival BlArl = new BLArrival();
        BLExpManifest objExpMani = new BLExpManifest();
        bool ds;
        ArrayList ULDDestpt = new ArrayList();
        DateTime dtCurrentDate = DateTime.Now;
        MasterBAL objMst = new MasterBAL();
        static int awbrowno;
        string AWBNo;
        int flag = 1;
        string editgrid;
        string MAWBNo;
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable("GHA_Imp_Arrival_dtTable1");
        DataTable dtTable2 = new DataTable("GHA_Imp_Arrival_dtTable2");
        DataTable dtTable3 = new DataTable("GHA_Imp_Arrival_dtTable3");

        struct ShipperConsignee
        {
            public string AccountCode;
            public string Name;
            public string Telephone;
            public string Address1;
            public string Address2;
            public string City;
            public string State;
            public string Country;
            public string Pincode;
            public string Email;
            public string KnownShipper;
        }

        #endregion Variable Declaration

        //Load
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dtCurrentDate = (DateTime)Session["IT"];
                lblDate.Text = dtCurrentDate.ToString();

                if (!IsPostBack)
                {
                    lnkCreate.Enabled = false;
                    LnkModify.Enabled = false;

                    #region Operation Time Popup Config Check
                    try
                    {
                        LoginBL objConfig = new LoginBL();
                        //btnOpsTime.Visible = Convert.ToBoolean(objConfig.GetMasterConfiguration("enableActualOpsTime"));
                        btnOpsTime.Visible = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "enableActualOpsTime"));
                        objConfig = null;
                    }
                    catch
                    { }
                    #endregion
                    HandleVehicleInfo();
                    txtFlightDate.Text = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy");
                    if (Session["AirlinePrefix"] != null)
                    {
                        txtFlightPrefix.Text = Session["AirlinePrefix"].ToString();
                    }
                    LoadSystemParameters();
                    LoadULDGrid();
                    LoadGridArrival();
                    //LoadOperationTimeConfig();
                    LoadCountryCodes();

                    #region Block grid

                    for (int i = 0; i < GVArrDet.Rows.Count; i++)
                    {
                        ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Enabled = false;

                        ((TextBox)GVArrDet.Rows[i].FindControl("Owner")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Enabled = false;

                        ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Enabled = false;

                        ((TextBox)GVArrDet.Rows[i].FindControl("BkdPcs")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("BkdWt")).Enabled = false;

                        ((TextBox)GVArrDet.Rows[i].FindControl("CustomStatusCode")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Enabled = false;

                        ((TextBox)GVArrDet.Rows[0].FindControl("AWB")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[0].FindControl("FlightNo")).Enabled = false;

                    }
                    #endregion BlockGrid

                    //Load the parameters which are coming in Querystring.
                    string FlightNo = string.Empty;
                    string FlightDt = string.Empty;

                    if (Request.QueryString["FltNo"] != null)
                        FlightNo = Request.QueryString["FltNo"];

                    if (Request.QueryString["FltDt"] != null)
                        FlightDt = Request.QueryString["FltDt"];

                    if (FlightNo != "")
                    {
                        txtFlightPrefix.Text = FlightNo.Substring(0, 2);
                        txtFlightNo.Text = FlightNo.Replace(txtFlightPrefix.Text, "");

                        txtFlightDate.Text = FlightDt;
                        btnList_Click(null, null);
                    }

                    //End
                    
                    try
                    {
                        if (Session["ULDACT"].ToString().ToUpper() == "FALSE")
                        {
                            UDLdiv.Visible = false;
                            lblULD.Visible = false;
                            btnprintUCR.Visible = false;
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

        #region Load Country Codes
        private void LoadCountryCodes()
        {
            try
            {
                BookingBAL objBooking = new BookingBAL();

                DataSet objDS = new DataSet("Arrival_LdCnt_ds");
                objDS = objBooking.GetAllCountry();
                if (objDS != null)
                {
                    //Loading Country Master codes
                    if (objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                    {
                        objDS.Tables[0].Rows.Add("Select", "Select");
                        ddlShipCountry.DataSource = objDS;
                        ddlShipCountry.DataMember = objDS.Tables[0].TableName;
                        ddlShipCountry.DataTextField = "CountryName";
                        ddlShipCountry.DataValueField = "CountryCode";
                        ddlShipCountry.DataBind();
                        ddlShipCountry.SelectedValue = "";

                        ddlConCountry.DataSource = objDS;
                        ddlConCountry.DataMember = objDS.Tables[0].TableName;
                        ddlConCountry.DataTextField = "CountryName";
                        ddlConCountry.DataValueField = "CountryCode";
                        ddlConCountry.DataBind();
                        ddlConCountry.SelectedValue = "";
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion Load Country Codes

        #region Clear
        /// <summary>
        /// Clear Fuction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblHAWBStatus.Text = "";
            lblStatus.Text = "";
            CleanChildAWBSessions();
            txtFlightNo.Text = "";
            txtFlightDate.Text = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy");
            LoadGridArrival();
            LoadULDGrid();
            ((TextBox)GVArrDet.Rows[0].FindControl("AWB")).Enabled = false;
            ((TextBox)GVArrDet.Rows[0].FindControl("FlightNo")).Enabled = false;
            lnkCreate.Enabled = false;
            LnkModify.Enabled = false;
            txtFlightPrefix.Enabled = true; txtFlightNo.Enabled = true;
            txtFlightDate.Enabled = true;

        }
        #endregion Clear

        #region List
        /// <summary>
        /// List Data according to criteria 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnList_Click(object sender, EventArgs e)
        {
            if (txtFlightPrefix.Text == "")
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please Enter Flight Prefix.";
                return;
            }
            if (txtFlightNo.Text == "")
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please Enter Flight No.";
                return;
            }

            if ((txtFlightDate.Text.Trim()).Length != 10)
            {
                lblStatus.ForeColor = Color.Red;
                if ((txtFlightDate.Text.Trim()).Length == 0)
                    lblStatus.Text = "Please Enter Date";
                else
                    lblStatus.Text = "Please Enter Correct Date";
                return;
            }
            lblHAWBStatus.Text = "";
            lblStatus.Text = "";
            try
            {
                CleanChildAWBSessions();
                #region GetList
                lblStatus.Text = "";
            }
            catch (Exception)
            { }
            bool isDataFound = false;
            string[] paramname = new string[4];
            object[] paramvalue = new object[4];
            SqlDbType[] paramtype = new SqlDbType[4];

            DataSet ds = new DataSet("Arrival_BtnList_ds");
            //DataSet ds = null;
            try
            {
                paramname[0] = "Fltno";
                paramname[1] = "FltDate";
                paramname[2] = "StationCode";
                paramname[3] = "ArrivalStatus";

                string Fltno = string.Empty;
                string FltDate = string.Empty;
                string StationCode = string.Empty;
                string discrepancy = "";

                if (txtFlightNo.Text.Trim() == "")
                {
                    paramvalue[0] = "";
                }
                else
                {
                    paramvalue[0] = txtFlightPrefix.Text.Trim() + txtFlightNo.Text.Trim();
                }
                if (txtFlightDate.Text.Trim() == "")
                {
                    paramvalue[1] = "";
                }
                else
                {
                    paramvalue[1] = txtFlightDate.Text;
                    Session["ActlFlightDate"] = txtFlightDate.Text;
                }
                if (Convert.ToString(Session["Station"]) != "")
                {
                    StationCode = Session["Station"].ToString();
                }

                paramvalue[2] = StationCode;
                paramvalue[3] = ddlArrivalStatus.SelectedValue;
                //4/3/2012
                paramtype[0] = SqlDbType.NVarChar;
                paramtype[1] = SqlDbType.NVarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                #endregion GetList

                #region Get Flight Details
                string strdestination = string.Empty;
                string Fltnumber = txtFlightPrefix.Text.Trim() + txtFlightNo.Text.Trim();
                string DepartureAirport = Session["Station"].ToString();
                #endregion Get Flight Details

                ds = da.SelectRecords("Sp_GetArrival_GHA", paramname, paramvalue, paramtype);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Session["FltOrigin"] = ds.Tables[0].Rows[0]["Org"];
                }
                else if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    Session["FltOrigin"] = ds.Tables[1].Rows[0]["POL"];
                }

                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    grdULDDetails.DataSource = ds.Tables[1];
                    grdULDDetails.DataBind();
                    int i = 0;
                    for (i = 0; i < grdULDDetails.Rows.Count; i++)
                    {
                        if (ds.Tables[1].Rows[i]["isarrived"].ToString() == "True")
                        {
                            ((CheckBox)grdULDDetails.Rows[i].FindControl("isarrived")).Checked = true;
                            ((CheckBox)grdULDDetails.Rows[i].FindControl("checkULD")).Enabled = false;
                        }
                        if (ds.Tables[1].Rows[i]["ArrivalCustomCheck"].ToString() == "True")
                        {
                            ((CheckBox)grdULDDetails.Rows[i].FindControl("CustomCheck")).Checked = true;
                        }
                        if (ds.Tables[1].Rows[i]["ArrivalSecurityCheck"].ToString() == "True")
                        {
                            ((CheckBox)grdULDDetails.Rows[i].FindControl("SecurityCheck")).Checked = true;
                        }
                        //Set colour coding for received ULDs.
                        if (ds.Tables[1].Rows[i]["IsReceived"].ToString() == "Y")
                        {
                            grdULDDetails.Rows[i].BackColor = CommonUtility.ColorHighlightedGrid;
                            ((CheckBox)grdULDDetails.Rows[i].FindControl("BUP")).Checked = true;
                            //Disable child AWB popup for Received ULDs.
                            ((ImageButton)grdULDDetails.Rows[i].FindControl("btnULDChlidPopup")).Enabled = false;
                        }

                        //Disable controls..
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Enabled = false;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightNo")).Enabled = false;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightDate")).Enabled = false;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDOrg")).Enabled = false;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDPOL")).Enabled = false;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDDestn")).Enabled = false;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDULDWt")).Enabled = false;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDScaleWt")).Enabled = false;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBCount")).Enabled = false;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBPcs")).Enabled = false;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBWt")).Enabled = false;
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("SecurityCheck")).Enabled = true;
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("CustomCheck")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("CustomStatusCode")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("Remark")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDLocation")).Enabled = true;
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("isarrived")).Enabled = false;
                        isDataFound = true;
                    }
                    Session["dsULDData"] = ds.Tables[1];
                }
                else
                {
                    LoadULDGrid();
                }

                if (ds != null && ds.Tables.Count > 1)
                {
                    Session["dsULDData"] = ds.Tables[1];
                }
                // Upper Grid
                if (ds != null)
                {
                    Session["dsData"] = ds.Tables[0];
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            GVArrDet.DataSource = ds.Tables[0];
                            GVArrDet.DataBind();
                            //Set data in session for adding new ULD row.
                            Session["dtCreditInfo"] = ds.Tables[0];
                            for (int i = 0; i < GVArrDet.Rows.Count; i++)
                            {


                                DataRow dr = ds.Tables[0].Rows[i];
                                //Set values for custom check & security check box.
                                ((CheckBox)GVArrDet.Rows[i].FindControl("SecurityCheck")).Checked = Convert.ToBoolean(dr["SecurityCheck"]);
                                ((CheckBox)GVArrDet.Rows[i].FindControl("CustomCheck")).Checked = Convert.ToBoolean(dr["CustomCheck"]);

                                ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("Owner")).Enabled = false; ;
                                ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Enabled = false; ;
                                ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Enabled = false;

                                ((TextBox)GVArrDet.Rows[i].FindControl("BkdPcs")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("BkdWt")).Enabled = false;

                                ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Enabled = false;

                                ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Enabled = false;

                                ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("Shipper")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[i].FindControl("Consignee")).Enabled = false;
                                ((HiddenField)GVArrDet.Rows[i].FindControl("hdnManualAWB")).Value = "N";

                                ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Enabled = false;

                                isDataFound = true;

                                #region US Customs Check Added by DEEPAK 24APR14
                                if (Convert.ToBoolean(((HiddenField)GVArrDet.Rows[i].FindControl("hdIsUSCustom")).Value))
                                {
                                    ((CheckBox)GVArrDet.Rows[i].FindControl("CustomCheck")).Checked = true;
                                    ((CheckBox)GVArrDet.Rows[i].FindControl("CustomCheck")).Enabled = false;

                                }
                                #endregion
                            }

                            #region Validate

                            DataSet ddlds1 = new DataSet("Arr_BtnLst_ddlds1");
                            ddlds1 = da.SelectRecords("Sp_GetDiscrepancy");

                            for (int i = 0; i < GVArrDet.Rows.Count; i++)
                            {

                                DropDownList ddl = ((DropDownList)GVArrDet.Rows[i].FindControl("ddlDiscrepancy"));
                                ddl.DataSource = ddlds1;
                                ddl.DataMember = ddlds1.Tables[0].TableName;
                                ddl.DataTextField = ddlds1.Tables[0].Columns[0].ColumnName;
                                ddl.DataValueField = ddlds1.Tables[0].Columns[0].ColumnName;
                                ddl.DataBind();
                                ddl.Dispose();
                            }

                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            {

                                string reassign = ds.Tables[0].Rows[k][16].ToString();
                                string offloadreassign = ds.Tables[0].Rows[k][18].ToString();
                                string isclosed = ds.Tables[0].Rows[k][17].ToString();
                                string AWBNumber = ds.Tables[0].Rows[k][3].ToString();
                                string OffloadLoc = ds.Tables[0].Rows[k][19].ToString();
                                string location = Session["Station"].ToString();
                                string statusIsclosed = "";
                                string station = Session["Station"].ToString();
                                string FltNo = ds.Tables[0].Rows[k]["FltNo"].ToString();
                                discrepancy = ds.Tables[0].Rows[k]["Discrepancy"].ToString();
                                (((DropDownList)GVArrDet.Rows[k].FindControl("ddlDiscrepancy")).Text) = discrepancy;
                                DateTime fltdate = Convert.ToDateTime(ds.Tables[0].Rows[k]["Date"]); //DateTime.ParseExact(txtFlightDate.Text, "dd-MM-yyyy", null);
                                string Complete = "";

                                //24-8-2012 new requirement
                                DataSet ds2 = new DataSet("Arrival_Btnl_dsClose");
                                ds2 = BlArl.GetIsClosedtatus(AWBNumber, station);

                                DataSet ds1 = new DataSet("Arrival_Btnl_dsDoPcs");
                                ds1 = BlArl.GetDoPieces(AWBNumber, FltNo, fltdate);

                                if (ds1 != null)
                                {
                                    if (ds1.Tables[0].Rows.Count > 0)
                                    {
                                        Complete = ds1.Tables[0].Rows[0]["status"].ToString();
                                    }
                                    ds1.Dispose();
                                }
                                if (ds2 != null)
                                {
                                    if (ds2.Tables[0].Rows.Count > 0)
                                    {
                                        statusIsclosed = ds2.Tables[0].Rows[0]["IsClosed"].ToString();
                                    }
                                    ds2.Dispose();
                                }

                                if (Complete == "Complete")
                                {

                                    for (int i = 0; i < GVArrDet.Rows.Count; i++)
                                    {
                                        string awb = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text;
                                        if (awb == AWBNumber)
                                        {
                                            ////********* Commented by Vishal on 17 JUN based on G8 request
                                            //for (int j = 0; j < GVArrDet.Columns.Count; j++)
                                            //{
                                            //    GVArrDet.Rows[i].Cells[j].Enabled = false;
                                            //}
                                        }
                                    }
                                    lblStatus.Text = "AWB from this flight are Delivered..";
                                    lblStatus.ForeColor = Color.Green;

                                }
                                if ((offloadreassign == "Reassign") && (OffloadLoc == location))
                                {
                                    for (int i = 0; i < GVArrDet.Rows.Count; i++)
                                    {

                                        string awb = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text;
                                        if (awb == AWBNumber)
                                        {
                                            for (int j = 0; j < GVArrDet.Columns.Count; j++)
                                            {
                                                GVArrDet.Rows[i].Cells[j].Enabled = false;
                                            }
                                        }
                                    }
                                    lblStatus.Text = "Some AWB from this flight are Reassigned..";
                                    lblStatus.ForeColor = Color.Green;
                                }
                                if (statusIsclosed == "Closed")
                                {
                                    for (int i = 0; i < GVArrDet.Rows.Count; i++)
                                    {
                                        for (int j = 0; j < GVArrDet.Columns.Count; j++)
                                        {
                                            GVArrDet.Rows[i].Cells[j].Enabled = false;
                                        }
                                    }
                                    lblStatus.Text = "Flight is closed..";
                                    lblStatus.ForeColor = Color.Red;
                                }

                                if (statusIsclosed == "Opened")
                                {
                                    for (int i = 0; i < GVArrDet.Rows.Count; i++)
                                    {
                                        for (int j = 0; j < GVArrDet.Columns.Count; j++)
                                        {
                                            GVArrDet.Rows[i].Cells[j].Enabled = true;
                                        }
                                    }
                                    lblStatus.Text = "Flight is Re-Opened..";
                                    lblStatus.ForeColor = Color.Green;
                                }
                            #endregion Validate

                            }

                            foreach (GridViewRow gvr in GVArrDet.Rows)
                            {   //No Reprinting. Keep grid editable.
                                if (((TextBox)gvr.FindControl("Expectedpcs")).Text == "0")
                                {   //Disable controls if all pcs are already arrived.
                                    ((TextBox)gvr.FindControl("RcvPcs")).Enabled = false;
                                    ((TextBox)gvr.FindControl("RcvWt")).Enabled = false;
                                    ((DropDownList)gvr.FindControl("ddlDiscrepancy")).Enabled = false;
                                    ((TextBox)gvr.FindControl("Remark")).Enabled = false;
                                }
                                else
                                {
                                    ((TextBox)gvr.FindControl("RcvPcs")).Enabled = true;
                                    ((TextBox)gvr.FindControl("RcvWt")).Enabled = true;
                                    ((DropDownList)gvr.FindControl("ddlDiscrepancy")).Enabled = true;
                                    ((TextBox)gvr.FindControl("Remark")).Enabled = true;
                                }
                            }
                            txtFlightDate.Enabled = false;
                            txtFlightNo.Enabled = false;
                            txtFlightPrefix.Enabled = false;
                            lnkCreate.Enabled = true;
                            LnkModify.Enabled = true;
                        }
                        else
                        {
                            LoadGridArrival();

                            string[] paramname1 = new string[2];
                            object[] paramvalue1 = new object[2];
                            SqlDbType[] paramtype1 = new SqlDbType[2];

                            DataSet ds1 = new DataSet("Arr_SP_Flt");
                            paramname1[0] = "FltPreFltNumber";
                            paramname1[1] = "Date";

                            paramvalue1[0] = txtFlightPrefix.Text.Trim() + txtFlightNo.Text.Trim();
                            DateTime date = DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            paramvalue1[1] = date.ToString("yyyy/MM/dd");

                            paramtype1[0] = SqlDbType.VarChar;
                            paramtype1[1] = SqlDbType.DateTime;
                            ds1 = da.SelectRecords("SP_FltExistOrNot", paramname1, paramvalue1, paramtype1);
                            if (ds1.Tables.Count != 0)
                            {
                                if (ds1.Tables[0].Rows.Count > 0 || ds1.Tables[1].Rows.Count > 0)
                                {
                                    ((TextBox)GVArrDet.Rows[0].FindControl("AWB")).Enabled = true;
                                    ((TextBox)GVArrDet.Rows[0].FindControl("FlightNo")).Enabled = true;
                                    lnkCreate.Enabled = true;
                                    LnkModify.Enabled = true;
                                    txtFlightDate.Enabled = false;
                                    txtFlightNo.Enabled = false;
                                    txtFlightPrefix.Enabled = false;
                                }
                                else
                                {
                                    txtFlightDate.Enabled = true;
                                    txtFlightNo.Enabled = true;
                                    txtFlightPrefix.Enabled = true;
                                    lnkCreate.Enabled = false;
                                    LnkModify.Enabled = false;
                                    ((TextBox)GVArrDet.Rows[0].FindControl("AWB")).Enabled = false;
                                    ((TextBox)GVArrDet.Rows[0].FindControl("FlightNo")).Enabled = false;
                                }
                            }
                            if (ds1 != null)
                                ds1.Dispose();
                        }
                    }
                    else
                    {
                        lblStatus.Text = "Sorry, No AWBs found..";
                        lblStatus.ForeColor = Color.Red;
                    }
                }
                //Show message if data not found.
                if (!isDataFound)
                {
                    lblStatus.Text = "No data found. Please verify current station and filter criteria.";
                    lblStatus.ForeColor = Color.Red;
                }


                #region visibility shownotes
                //new functionality to  show or hide shownotes imges
                if (isDataFound)
                {

                    bool flag = false;
                    flag = CommonUtility.ShowNotes("", "", Fltnumber, txtFlightDate.Text.Trim());
                    imgNotebtn.Visible = flag;

                }

                #endregion Shownotes
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                paramname = null;
                paramvalue = null;
                paramtype = null;
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }
        #endregion List

        #region Clean Child AWB Sessions
        private void CleanChildAWBSessions()
        {
            //Clean previous CHILD AWB Sessions (if any)..
            for (int intULDRow = 0; intULDRow < grdULDDetails.Rows.Count; intULDRow++)
            {
                if (((TextBox)grdULDDetails.Rows[intULDRow].FindControl("GrdULDNo")).Text != "")
                {
                    Session["ChildAWB_" + ((TextBox)grdULDDetails.Rows[intULDRow].FindControl("GrdULDNo")).Text] = null;
                }
            }
            //Clean previous HAWB Sessions (if any)..
            for (int intAWBRow = 0; intAWBRow < GVArrDet.Rows.Count; intAWBRow++)
            {
                if (((TextBox)GVArrDet.Rows[intAWBRow].FindControl("AWB")).Text != "")
                {
                    Session["ARR_HAWBDetails" + ((TextBox)GVArrDet.Rows[intAWBRow].FindControl("AWB")).Text] = null;
                    Session["Arrival_Shipper_" + ((TextBox)GVArrDet.Rows[intAWBRow].FindControl("AWB")).Text] = null;
                    Session["Arrival_Consignee_" + ((TextBox)GVArrDet.Rows[intAWBRow].FindControl("AWB")).Text] = null;
                }
            }
        }
        #endregion Clean Child AWB Sessions

        /// <summary>
        /// Auto Arrive data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region AutoArrive
        protected void btnAutoArrive_Click(object sender, EventArgs e)
        {
            lblHAWBStatus.Text = "";
            lblStatus.Text = "";
            bool IsSelected = false;
            try
            {
                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {
                    if (((CheckBox)GVArrDet.Rows[i].FindControl("check")).Checked == true)
                    {
                        IsSelected = true;
                    }
                }
                if (IsSelected)
                {
                    for (int i = 0; i < GVArrDet.Rows.Count; i++)
                    {
                        string mftpcs = ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text;
                        string mftwt = ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text;
                        ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text = mftpcs;
                        ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Text = mftwt;
                    }
                    //Call save to save the data of selected rows.
                    btnSave_Click(this, new EventArgs());
                }
                else
                {
                    lblStatus.Text = "Please Select  row to Auto arrival..";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception)
            { }

        }
        #endregion AutoArrive

        #region List Button Filter
        protected void btnListFilter_Click(object sender, EventArgs e)
        {
        }
        #endregion List Button Filter

        #region Save_Click grid
        /// <summary>
        /// Save Data into arrival summary table usnig stored procedure sp_insertArrival
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            string[] paramname;
            object[] paramvalue;
            SqlDbType[] paramtype;
            try
            {

                bool IsSelected = false;
                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {
                    ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Enabled = false;

                    if (((CheckBox)GVArrDet.Rows[i].FindControl("check")).Checked == true)
                    {
                        IsSelected = true;
                        break;
                    }
                }

                for (int i = 0; i < grdULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)grdULDDetails.Rows[i].FindControl("checkULD")).Checked == true)
                    {
                        IsSelected = true;
                        break;
                    }
                }

                #region Isselected
                if (IsSelected)
                {

                    #region Saving AWB Details
                    for (int i = 0; i < GVArrDet.Rows.Count; i++)
                    {
                        //string ArrAWBNumber = "";
                        if (((CheckBox)GVArrDet.Rows[i].FindControl("check")).Checked == true)
                        {
                            //Validate if AWB Number is not blank.
                            if (((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text == "" ||
                                ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text.Length < 8)
                            {
                                lblStatus.Text = "Please enter valid AWB Number (Prefix-Number)";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Enabled = true;
                                ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Focus();
                                return;
                            }
                            //Validate if Flight # is entered.
                            if (((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text.Length < 2)
                            {
                                lblStatus.Text = "Please enter valid Flight Number (Prefix+Number)";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Enabled = true;
                                ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Focus();
                                return;
                            }
                            //Validate if Flight Date is entered.
                            if (((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Text.Length < 8)
                            {
                                lblStatus.Text = "Please enter valid Flight Date (dd/MM/yyyy)";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Enabled = true;
                                ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Focus();
                                return;
                            }
                            //Validate if Origin is entered.
                            if (((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Text.Length < 3)
                            {
                                lblStatus.Text = "Please enter valid Origin Station";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Enabled = true;
                                ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Focus();
                                return;
                            }
                            //Validate if Destination is entered.
                            if (((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Text.Length < 3)
                            {
                                lblStatus.Text = "Please enter valid Destination Station";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Enabled = true;
                                ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Focus();
                                return;
                            }
                            //Validate if POL is entered.
                            if (((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text.Length < 3)
                            {
                                lblStatus.Text = "Please enter valid POL";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Enabled = true;
                                ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Focus();
                                return;
                            }
                            //Validate if Rec Pcs is entered.
                            if (((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text.Length == 0)
                            {
                                lblStatus.Text = "Please enter valid Received Pcs";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Enabled = true;
                                ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Focus();
                                return;
                            }
                            //Validate if Rec Wt is entered.
                            if (((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Text.Length == 0)
                            {
                                lblStatus.Text = "Please enter valid Received Wt";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Enabled = true;
                                ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Focus();
                                return;
                            }

                            string status = string.Empty;
                            string operationType = string.Empty;
                            IsSelected = true;
                            int chkconvert = Convert.ToInt32(((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text);
                            int chkcon = Convert.ToInt32(((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text);
                            string origin = ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text;
                            string station = Session["Station"].ToString();
                            if (origin == station)
                            {
                                lblStatus.Text = "Sorry Please select different destination for arrival..";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }

                            decimal dcRemainingWt = Convert.ToDecimal(((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text);
                            decimal dcReceivedWt = Convert.ToDecimal(((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Text);

                            if (chkcon < 1)
                            {
                                //btnList_Click(null, null);
                                lblStatus.Text = "Received pieces can not be 0";
                                ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Focus();
                                return;
                            }

                            #region Code to Include Converted Weight Less than 1 (Deepak 29thMay14)
                            if (dcReceivedWt <= 0)
                            {

                                //btnList_Click(null, null);
                                lblStatus.Text = "Received weight can not be 0";
                                ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Focus();
                                return;

                            }
                            #endregion
                            //For CS-296 by Vishal
                            if (Session["editgrid"] == null ||
                                Session["editgrid"].ToString() != ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text)
                            {
                                if (chkcon > chkconvert)
                                {
                                    //btnList_Click(null, null);
                                    lblStatus.Text = "Received pieces can not be more than remaining pieces.";
                                    ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Focus();
                                    return;
                                }
                                if (dcReceivedWt > dcRemainingWt)
                                {
                                    //btnList_Click(null, null);
                                    lblStatus.Text = "Received weight can not be more than remaining weight.";
                                    ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Focus();
                                    return;
                                }
                            }
                            else
                            {   //If AWB is being edited.
                                int accpcs = 0;
                                if (((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Text != "")
                                    accpcs = int.Parse(((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Text);

                                decimal accwt = 0;
                                if (((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Text != "")
                                    accwt = decimal.Parse(((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Text);

                                if (chkcon > accpcs)
                                {
                                    lblStatus.Text = "Received pieces can not be more than accepted pieces.";
                                    ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Focus();
                                    return;
                                }
                                if (dcReceivedWt > accwt)
                                {
                                    lblStatus.Text = "Received weight can not be more than accepted weight.";
                                    ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Focus();
                                    return;
                                }
                            }

                            if (chkcon == chkconvert && dcReceivedWt != dcRemainingWt)
                            {
                                //btnList_Click(null, null);
                                lblStatus.Text = "Please enter valid Weight.";
                                ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Focus();
                                return;
                            }

                            if (dcReceivedWt == dcRemainingWt && chkcon != chkconvert)
                            {
                                //btnList_Click(null, null);
                                lblStatus.Text = "Please enter valid Pieces.";
                                ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Focus();
                                return;
                            }

                            //Validate RECEIVED HAWB COUNT AGAINST HAWB PCS.
                            if (((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Text
                                != ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text)
                            {   //Check if HAWB Count is matching HAWB Received Count.
                                if ((((TextBox)GVArrDet.Rows[i].FindControl("HAWBCt")).Text
                                    == ((TextBox)GVArrDet.Rows[i].FindControl("RCVHAWBCt")).Text) &&
                                    (((TextBox)GVArrDet.Rows[i].FindControl("HAWBCt")).Text != ""
                                    && ((TextBox)GVArrDet.Rows[i].FindControl("HAWBCt")).Text != "0"))
                                {
                                    lblStatus.Text = "Please select appropriate HAWB for Partial Arrival of AWB.";
                                    lblStatus.ForeColor = Color.Red;
                                    return;
                                }
                            }

                            if (ViewState["AWBLocMandatoryInArr"] != null && ViewState["AWBLocMandatoryInArr"].ToString() != ""
                                && (bool)ViewState["AWBLocMandatoryInArr"] == true)
                            {
                                //Validate LOCATION.
                                if (((TextBox)GVArrDet.Rows[i].FindControl("txtLocation")).Text == "")
                                {
                                    lblStatus.Text = "Please enter Location for AWB.";
                                    lblStatus.ForeColor = Color.Red;
                                    ((TextBox)GVArrDet.Rows[i].FindControl("txtLocation")).Focus();
                                    return;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < GVArrDet.Rows.Count; i++)
                    {
                        string ArrAWBNumber = "";

                        #region check
                        if (((CheckBox)GVArrDet.Rows[i].FindControl("check")).Checked == true)
                        {

                            //If Manual AWB entered for arrival.
                            if (((HiddenField)GVArrDet.Rows[i].FindControl("hdnManualAWB")).Value == "Y")
                            {   //FFM Messaging logic to update database for Booking, ULD & FFM information.
                                validateAndInsertFFMData(i);

                                //Save AWB Shipper Consignee details (if any).
                                ClearShipperConsigneeValues();

                                string awbnum = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text;
                                string awbprefix = "";
                                if (awbnum.Contains("-"))
                                {
                                    awbprefix = awbnum.Substring(0, awbnum.IndexOf("-"));
                                }
                                string awbnumwoprefix = "";
                                if (awbnum.Contains("-"))
                                {
                                    awbnumwoprefix = awbnum.Substring(awbnum.IndexOf("-") + 1);
                                }
                                lblShipperAWB.Text = awbnum;
                                lblConsigneeAWB.Text = awbnum;

                                if (Session["Arrival_Shipper_" + lblShipperAWB.Text] != null)
                                {   //If shipper details already available in session.
                                    txtShipperAccountCode.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).AccountCode;
                                    TXTShipper.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Name;
                                    TXTShipAddress.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Address1;
                                    TXTShipperAdd2.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Address2;
                                    TXTShipperCity.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).City;
                                    TXTShipperEmail.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Email;
                                    TXTShipperState.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).State;
                                    TXTShipPinCode.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Pincode;
                                    TXTShipTelephone.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Telephone;
                                    ddlShipCountry.SelectedValue = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Country;
                                }
                                if (Session["Arrival_Consignee_" + lblConsigneeAWB.Text] != null)
                                {   //If shipper details already available in session.
                                    txtConsigneeAccountCode.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).AccountCode;
                                    TXTConsignee.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Name;
                                    TXTConAddress.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Address1;
                                    TXTConsigAdd2.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Address2;
                                    TXTConsigCity.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).City;
                                    TXTConsigEmail.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Email;
                                    TXTConsigState.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).State;
                                    TXTConsigPinCode.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Pincode;
                                    TXTConTelephone.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Telephone;
                                    ddlConCountry.SelectedValue = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Country;
                                }

                                BookingBAL objBLL = new BookingBAL();
                                if (!objBLL.SaveAWBShipperConsignee(new object[] {awbnumwoprefix,TXTShipper.Text.Trim(),
                                        TXTShipAddress.Text.Trim(),ddlShipCountry.Text,TXTShipTelephone.Text.Trim(),
                                        TXTConsignee.Text.Trim(),TXTConAddress.Text.Trim(),ddlConCountry.Text,
                                        TXTConTelephone.Text.Trim(),TXTShipperAdd2.Text.Trim(),TXTShipperCity.Text.Trim(),
                                        TXTShipperState.Text.Trim(),TXTShipPinCode.Text.Trim(),//Shipper extra data
                                        TXTConsigAdd2.Text.Trim(),TXTConsigCity.Text.Trim(),TXTConsigState.Text.Trim(),
                                        TXTConsigPinCode.Text.Trim(),txtShipperAccountCode.Text.Trim(),
                                        txtConsigneeAccountCode.Text.Trim(),TXTShipperEmail.Text.Trim(),
                                        TXTConsigEmail.Text.Trim(),awbprefix
                                       }))
                                {
                                    lblStatus.Text = "Shipper/Consignee save failed";
                                    lblStatus.ForeColor = Color.Red;
                                }
                                else
                                {
                                    Session["Arrival_Shipper_" + lblShipperAWB.Text] = null;
                                    Session["Arrival_Consignee_" + lblConsigneeAWB.Text] = null;
                                }

                            }
                            //Update AWB for arrival.
                            #region PartialArrival Logic
                            string status = string.Empty;
                            string operationType = string.Empty;

                            IsSelected = true;
                            int chkconvert = Convert.ToInt32(((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text);
                            int chkcon = Convert.ToInt32(((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text);
                            string origin = ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text;
                            string station = Session["Station"].ToString();

                            decimal dcRemainingWt = Convert.ToDecimal(((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text);
                            decimal dcReceivedWt = Convert.ToDecimal(((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Text);

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

                            #endregion PartialArrival Logic
                            paramname = new string[32];
                            paramvalue = new object[32];
                            paramtype = new SqlDbType[32];
                            #region Paramname
                            paramname[0] = "ULD";
                            paramname[1] = "POL";
                            paramname[2] = "ULDDestination";
                            paramname[3] = "AWBNumber";
                            paramname[4] = "Owner";
                            paramname[5] = "MFTPieces";
                            paramname[6] = "MFTWeight";
                            paramname[7] = "RCVPieces";
                            paramname[8] = "RCVWeight";
                            paramname[9] = "Origin";
                            paramname[10] = "Destination";
                            paramname[11] = "StatedPieces";
                            paramname[12] = "StatedWeight";
                            paramname[13] = "Discrepancy";
                            paramname[14] = "SecurityCheck";
                            paramname[15] = "CustomCheck";
                            paramname[16] = "CustomStatusCode";
                            paramname[17] = "Remark";
                            paramname[18] = "FltNo";
                            paramname[19] = "status";
                            paramname[20] = "operationType";
                            paramname[21] = "Discription";
                            paramname[22] = "ExpectedPcs";
                            paramname[23] = "ExpectedWeight";
                            paramname[24] = "FltDate";
                            paramname[25] = "UpdatedBy";
                            paramname[26] = "UpdatedOn";
                            paramname[27] = "Location";
                            paramname[28] = "CommodityCode";
                            paramname[29] = "FltOrigin";
                            paramname[30] = "IsEdit";
                            paramname[31] = "IsManual";
                            #endregion Paramname

                            #region Paramvalue

                            paramvalue[0] = ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Text;
                            paramvalue[1] = ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text;
                            paramvalue[2] = Session["Station"].ToString();
                            paramvalue[3] = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text;
                            ArrAWBNumber = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text;
                            flag = 0;
                            if (Session["editgrid"] != null)
                            {
                                string GridAWB = Session["editgrid"].ToString();
                                if (GridAWB == ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text)
                                {
                                    flag = 1;
                                }
                            }
                            paramvalue[4] = ((TextBox)GVArrDet.Rows[i].FindControl("Owner")).Text;
                            paramvalue[5] = ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Text;
                            paramvalue[6] = ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Text;
                            paramvalue[7] = ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text;
                            paramvalue[8] = ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Text;
                            string MftPcs = ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Text;
                            string rcvpcs = ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text;
                            string Arrivedpcs = ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Text;
                            int Validate = (Convert.ToInt32(MftPcs) - Convert.ToInt32(Arrivedpcs));
                            if ((Session["editgrid"] == null ||
                                Session["editgrid"].ToString() != ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text) &&
                                Convert.ToInt32(rcvpcs) > Validate)
                            {
                                lblStatus.Text = "Recived pieces cannot be greater than remaining pieces";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }

                            paramvalue[9] = ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Text;
                            paramvalue[10] = ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Text;
                            paramvalue[11] = ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Text;
                            paramvalue[12] = ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Text;
                            paramvalue[13] = ((DropDownList)GVArrDet.Rows[i].FindControl("ddlDiscrepancy")).Text;
                            paramvalue[14] = ((CheckBox)GVArrDet.Rows[i].FindControl("SecurityCheck")).Checked;
                            paramvalue[15] = ((CheckBox)GVArrDet.Rows[i].FindControl("CustomCheck")).Checked;
                            paramvalue[16] = ((TextBox)GVArrDet.Rows[i].FindControl("CustomStatusCode")).Text;
                            paramvalue[17] = ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Text;
                            paramvalue[18] = ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text;
                            paramvalue[19] = status;
                            paramvalue[20] = operationType;
                            paramvalue[21] = ((Label)GVArrDet.Rows[i].FindControl("CommDesc")).Text;
                            paramvalue[22] = ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text;
                            paramvalue[23] = ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text;
                            paramvalue[24] = DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", null);//.ToString("dd/MM/yyyy");
                            paramvalue[25] = Session["UserName"].ToString();
                            paramvalue[26] = dtCurrentDate;
                            paramvalue[27] = ((TextBox)GVArrDet.Rows[i].FindControl("txtLocation")).Text;
                            paramvalue[28] = ((TextBox)GVArrDet.Rows[i].FindControl("CommCode")).Text;
                            paramvalue[29] = ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text;
                            //paramvalue[30] = ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Enabled;
                            paramvalue[30] = flag;
                            paramvalue[31] = ((HiddenField)GVArrDet.Rows[i].FindControl("hdnManualAWB")).Value == "Y" ? true : false;
                            #endregion Paramvalue

                            #region Paramtype
                            paramtype[0] = SqlDbType.NVarChar;
                            paramtype[1] = SqlDbType.NVarChar;
                            paramtype[2] = SqlDbType.VarChar;
                            paramtype[3] = SqlDbType.VarChar;
                            paramtype[4] = SqlDbType.VarChar;
                            paramtype[5] = SqlDbType.Int;
                            paramtype[6] = SqlDbType.Float;
                            paramtype[7] = SqlDbType.Int;
                            paramtype[8] = SqlDbType.Float;
                            paramtype[9] = SqlDbType.VarChar;
                            paramtype[10] = SqlDbType.VarChar;
                            paramtype[11] = SqlDbType.Int;
                            paramtype[12] = SqlDbType.Float;
                            paramtype[13] = SqlDbType.VarChar;
                            paramtype[14] = SqlDbType.Bit;
                            paramtype[15] = SqlDbType.Bit;
                            paramtype[16] = SqlDbType.VarChar;
                            paramtype[17] = SqlDbType.VarChar;
                            paramtype[18] = SqlDbType.VarChar;
                            paramtype[19] = SqlDbType.VarChar;
                            paramtype[20] = SqlDbType.VarChar;
                            paramtype[21] = SqlDbType.VarChar;
                            paramtype[22] = SqlDbType.Int;
                            paramtype[23] = SqlDbType.Float;
                            paramtype[24] = SqlDbType.DateTime;
                            paramtype[25] = SqlDbType.VarChar;
                            paramtype[26] = SqlDbType.DateTime;
                            paramtype[27] = SqlDbType.VarChar;
                            paramtype[28] = SqlDbType.VarChar;
                            paramtype[29] = SqlDbType.VarChar;
                            paramtype[30] = SqlDbType.Bit;
                            paramtype[31] = SqlDbType.Bit;
                            #endregion Paramtype

                            string ULDDest = ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text;
                            ds = da.InsertData("Sp_InsertArrival", paramname, paramtype, paramvalue);
                            if (ds)
                            {
                                //Reset Session["editgrid"] if current AWB was set for Modification.
                                if (flag == 1)
                                {
                                    Session["editgrid"] = null;
                                }

                                try
                                {
                                    //Insert/ Update HAWB.
                                    SaveHAWBs(((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text,
                                       ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text,
                                       ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Text,
                                        txtFlightPrefix.Text + txtFlightNo.Text,
                                        DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy"));

                                    //string Message = "AWB Number :" + ArrAWBNumber + "(Pieces" + Arrivedpcs + ") arrived at Location" + paramvalue[3].ToString();
                                    //cls_BL.addMsgToOutBox("SCM", Message, "", "");

                                    // Deepak- Code for Customs Auto Messaging
                                    try
                                    {
                                        object[] QueryValues = new object[3];
                                        QueryValues[0] = ArrAWBNumber;
                                        QueryValues[1] = paramvalue[18];
                                        QueryValues[2] = txtFlightDate.Text;
                                        string[] AWBDetails = ArrAWBNumber.Split('-');

                                        CustomsImportBAL objCustoms = new CustomsImportBAL();
                                        if (Convert.ToBoolean(((HiddenField)GVArrDet.Rows[i].FindControl("hdIsUSCustom")).Value))
                                        {
                                            if (Convert.ToBoolean(((HiddenField)GVArrDet.Rows[i].FindControl("hdAutoCustom")).Value))
                                            {

                                                #region Send FRI MESSAGE CUSTOMS
                                                try
                                                {
                                                    if (((HiddenField)GVArrDet.Rows[i].FindControl("hdnManualAWB")).Value == "Y")
                                                    {

                                                        object[] QueryVals = { ArrAWBNumber, paramvalue[18].ToString(), txtFlightDate.Text };


                                                        StringBuilder sbFRI = objCustoms.EncodingFRIMessage(QueryVals);
                                                        object[] QueryValsFRI = { ArrAWBNumber, 1, paramvalue[18].ToString(), txtFlightDate.Text, sbFRI.ToString().ToUpper() };

                                                        objCustoms.UpdateFRIMessage(QueryValsFRI);

                                                        if (sbFRI != null)
                                                        {
                                                            if (sbFRI.ToString() != "")
                                                            {
                                                                object[] QueryValMail = { "FRI", paramvalue[18].ToString(), txtFlightDate.Text };
                                                                
                                                                //Getting MailID for FRI Message
                                                                DataSet dMail = new DataSet("Arr_MailID_dMail1");
                                                                dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                                                
                                                                string MailID = string.Empty;
                                                                if (dMail != null)
                                                                {
                                                                    MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                                }
                                                                cls_BL.addMsgToOutBox("FRI", sbFRI.ToString().ToUpper(), "", MailID);
                                                            }


                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                { }
                                                #endregion
                                                StringBuilder sb = objCustoms.EncodingFSNMessage(QueryValues);
                                                object[] QueryVal = { QueryValues[0], QueryValues[1], QueryValues[2], sb.ToString().ToUpper() };
                                                objCustoms.UpdateFSNMessage(QueryVal);

                                                if (sb != null)
                                                {
                                                    if (sb.ToString() != "")
                                                    {
                                                        object[] QueryValMail = { "FSN", paramvalue[18], txtFlightDate.Text };
                                                        
                                                        //Getting MailID for FSN Message
                                                        DataSet dMail = new DataSet("Arrival_GetCust_ds");
                                                        dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                                        
                                                        string MailID = string.Empty;
                                                        if (dMail != null)
                                                        {
                                                            MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                        }
                                                        cls_BL.addMsgToOutBox("FSN", sb.ToString().ToUpper(), "", MailID);
                                                    }
                                                }
                                            }
                                            #region ACAS PSN Messaging Deepak (18/04/2014)
                                            LoginBL objLogin = new LoginBL();
                                            if (Convert.ToBoolean(((HiddenField)GVArrDet.Rows[i].FindControl("hdAutoACAS")).Value))
                                            {
                                                try
                                                {
                                                    ACASBAL objACAS = new ACASBAL();
                                                    StringBuilder sbPSN = objACAS.EncodingPSNMessage(QueryValues);
                                                    object[] QueryValPSN = { QueryValues[0], QueryValues[1], QueryValues[2], sbPSN.ToString().ToUpper() };
                                                    objACAS.UpdatePSNMessage(QueryValPSN);

                                                    if (sbPSN != null)
                                                    {
                                                        if (sbPSN.ToString() != "")
                                                        {
                                                            object[] QueryValMail = { "PSN", paramvalue[18], txtFlightDate.Text };
                                                            
                                                            //Getting MailID for PSN Message
                                                            DataSet dMail = new DataSet("Arrival_SaveClick_dMail");
                                                            dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                                           
                                                            string MailID = string.Empty;
                                                            if (dMail != null)
                                                            {
                                                                MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                            }
                                                            cls_BL.addMsgToOutBox("PSN", sbPSN.ToString().ToUpper(), "", MailID);
                                                        }
                                                    }
                                                }
                                                catch (Exception Ex)
                                                {
                                                    #region Sending PER Error Message on System Error
                                                    try
                                                    {
                                                        ACASBAL objACAS = new ACASBAL();
                                                        StringBuilder sbPER = objACAS.EncodingPERMessage(QueryValues);
                                                        object[] QueryValPER = { QueryValues[0], QueryValues[1], QueryValues[2], sbPER.ToString().ToUpper() };
                                                        objACAS.UpdatePERMessage(QueryValPER);

                                                        if (sbPER != null)
                                                        {
                                                            if (sbPER.ToString() != "")
                                                            {
                                                                object[] QueryValMail = { "PER", paramvalue[18], txtFlightDate.Text };
                                                                //Getting MailID for PER Message

                                                                DataSet dMail = new DataSet("Arr_BtnS_dMail");
                                                                dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);

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
                                            }
                                            #endregion

                                        }

                                        objCustoms = null;
                                        QueryValues = null;
                                        AWBDetails = null;



                                    }
                                    catch (Exception ex)
                                    { }
                                }
                                catch (Exception)
                                { }
                            }
                        }
                        else
                        {
                            lblStatus.Text = "Record not inserted Please try again..";
                            lblStatus.ForeColor = Color.Red;
                        }
                        #endregion check

                    }
                    if (ds == true)
                    {
                        //passing parameter to add record in AWBMilestone
                        try
                        {
                            lblStatus.Text = "Arrival record saved successfully !";
                            lblStatus.ForeColor = Color.Green;
                            Session["editgrid"] = null;

                            //Vijay - Code to Create Dest agent invoice.
                            GenerateDestAgentInvoiceNumber();

                        }
                        catch (Exception)
                        { }
                    }
                    else
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "NotInserted()", true);
                        lblStatus.Text = "Please Select Row to add";
                        lblStatus.ForeColor = Color.Red;
                    }
                    #endregion

                    #region Saving ULD Details

                    for (int i = 0; i < grdULDDetails.Rows.Count; i++)
                    {
                        if (((CheckBox)grdULDDetails.Rows[i].FindControl("checkULD")).Checked == true)
                        {
                            //Validate if AWB Number is not blank.
                            if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Text == "" ||
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Text.Length < 6)
                            {
                                lblStatus.Text = "Please enter valid ULD Number";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Enabled = true;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Focus();
                                return;
                            }
                            //Validate if Flight # is entered.
                            if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightNo")).Text.Length < 2)
                            {
                                lblStatus.Text = "Please enter valid Flight Number (Prefix+Number)";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightNo")).Enabled = true;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightNo")).Focus();
                                return;
                            }
                            //Validate if Flight Date is entered.
                            if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightDate")).Text.Length < 8)
                            {
                                lblStatus.Text = "Please enter valid Flight Date (dd/MM/yyyy)";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightDate")).Enabled = true;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightDate")).Focus();
                                return;
                            }
                            //Validate if Origin is entered.
                            if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDOrg")).Text.Length < 3)
                            {
                                lblStatus.Text = "Please enter valid Origin Station";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDOrg")).Enabled = true;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDOrg")).Focus();
                                return;
                            }
                            //Validate if Destination is entered.
                            if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDDestn")).Text.Length < 3)
                            {
                                lblStatus.Text = "Please enter valid Destination Station";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDDestn")).Enabled = true;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDDestn")).Focus();
                                return;
                            }
                            //Validate if POL is entered.
                            if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDPOL")).Text.Length < 3)
                            {
                                lblStatus.Text = "Please enter valid POL";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDPOL")).Enabled = true;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDPOL")).Focus();
                                return;
                            }
                            //Validate if ULD Wt is entered.
                            if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDULDWt")).Text.Length == 0)
                            {
                                lblStatus.Text = "ULD Wt can not be Blank";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDULDWt")).Enabled = true;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDULDWt")).Focus();
                                return;
                            }
                            //Validate if Scale Wt is entered.
                            if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDScaleWt")).Text.Length == 0)
                            {
                                lblStatus.Text = "Scale Wt can not be Blank";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDScaleWt")).Enabled = true;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDScaleWt")).Focus();
                                return;
                            }
                            //Validate if AWB Count is entered.
                            if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBCount")).Text.Length == 0)
                            {
                                lblStatus.Text = "AWB Count can not be Blank";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBCount")).Enabled = true;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBCount")).Focus();
                                return;
                            }
                            //Validate if AWB Pcs is entered.
                            if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBPcs")).Text.Length == 0)
                            {
                                lblStatus.Text = "AWB Pieces can not be Blank";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBPcs")).Enabled = true;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBPcs")).Focus();
                                return;
                            }
                            //Validate if AWB Pcs is entered.
                            if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBWt")).Text.Length == 0)
                            {
                                lblStatus.Text = "AWB Weight can not be Blank";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBWt")).Enabled = true;
                                ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBWt")).Focus();
                                return;
                            }
                            if (ViewState["ULDLocMandatoryInArr"] != null && ViewState["ULDLocMandatoryInArr"].ToString() != ""
                                && (bool)ViewState["ULDLocMandatoryInArr"] == true)
                            {
                                //Validate if ULD Location is entered.
                                if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDLocation")).Text == "")
                                {
                                    lblStatus.Text = "Please enter Location for ULD.";
                                    lblStatus.ForeColor = Color.Red;
                                    ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDLocation")).Focus();
                                    return;
                                }
                            }

                            string ULDNumber = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Text.Trim();
                            paramname = new string[18];
                            paramname[0] = "UDLNo";
                            paramname[1] = "Fltno";
                            paramname[2] = "FltDate";
                            paramname[3] = "StationCode";
                            paramname[4] = "UserName";
                            paramname[5] = "TimeStamp";
                            paramname[6] = "ULDWt";
                            paramname[7] = "ScaleWt";
                            paramname[8] = "AWBCount";
                            paramname[9] = "AWBPcs";
                            paramname[10] = "AWBWt";
                            paramname[11] = "SecurityCheck";
                            paramname[12] = "CustomeCheck";
                            paramname[13] = "CustomeCode";
                            paramname[14] = "Remarks";
                            paramname[15] = "Location";
                            paramname[16] = "Origin";
                            paramname[17] = "BUP";
                            bool SecurityCheck = false;
                            SecurityCheck = ((CheckBox)grdULDDetails.Rows[i].FindControl("SecurityCheck")).Checked;

                            bool CustomCheck = false;
                            CustomCheck = ((CheckBox)grdULDDetails.Rows[i].FindControl("CustomCheck")).Checked;

                            bool IsBUP = false;
                            IsBUP = ((CheckBox)grdULDDetails.Rows[i].FindControl("BUP")).Checked;

                            decimal ScaleWt = 0;
                            int AWBCount = 0;

                            try
                            {
                                if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDScaleWt")).Text.Trim() != "")
                                    ScaleWt = Convert.ToDecimal(((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDScaleWt")).Text.Trim());

                                if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBCount")).Text.Trim() != "")
                                    AWBCount = Convert.ToInt32(((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBCount")).Text.Trim());
                            }
                            catch
                            {
                            }

                            paramvalue = new object[18];
                            paramvalue[0] = ULDNumber;
                            paramvalue[1] = txtFlightPrefix.Text.Trim() + txtFlightNo.Text.Trim();
                            paramvalue[2] = txtFlightDate.Text.Trim();    //DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", null);//.ToString("dd/MM/yyyy");
                            paramvalue[3] = Convert.ToString(Session["Station"]);
                            paramvalue[4] = Convert.ToString(Session["UserName"]);
                            paramvalue[5] = Convert.ToDateTime(Session["IT"]);
                            paramvalue[6] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDULDWt")).Text;
                            paramvalue[7] = ScaleWt;
                            paramvalue[8] = AWBCount;
                            paramvalue[9] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBPcs")).Text;
                            paramvalue[10] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBWt")).Text;
                            paramvalue[11] = SecurityCheck;//((CheckBox)grdULDDetails.Rows[i].FindControl("SecurityCheck")).Checked;
                            paramvalue[12] = CustomCheck;//((CheckBox)grdULDDetails.Rows[i].FindControl("CustomCheck")).Checked;
                            paramvalue[13] = ((TextBox)grdULDDetails.Rows[i].FindControl("CustomStatusCode")).Text;
                            paramvalue[14] = ((TextBox)grdULDDetails.Rows[i].FindControl("Remark")).Text;
                            paramvalue[15] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDLocation")).Text;
                            paramvalue[16] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDOrg")).Text;
                            paramvalue[17] = IsBUP;

                            paramtype = new SqlDbType[18];
                            paramtype[0] = SqlDbType.VarChar;
                            paramtype[1] = SqlDbType.VarChar;
                            paramtype[2] = SqlDbType.VarChar;
                            paramtype[3] = SqlDbType.VarChar;
                            paramtype[4] = SqlDbType.VarChar;
                            paramtype[5] = SqlDbType.DateTime;
                            paramtype[6] = SqlDbType.VarChar;
                            paramtype[7] = SqlDbType.VarChar;
                            paramtype[8] = SqlDbType.Int;
                            paramtype[9] = SqlDbType.Int;
                            paramtype[10] = SqlDbType.VarChar;
                            paramtype[11] = SqlDbType.Bit;
                            paramtype[12] = SqlDbType.Bit;
                            paramtype[13] = SqlDbType.VarChar;
                            paramtype[14] = SqlDbType.VarChar;
                            paramtype[15] = SqlDbType.VarChar;
                            paramtype[16] = SqlDbType.VarChar;
                            paramtype[17] = SqlDbType.Bit;
                            ds = da.InsertData("sp_ImpArriveULDDetails", paramname, paramtype, paramvalue);
                            if (ds)
                            {
                                //Save child AWBs as Arrived if any into database.
                                SaveChildAWBs(ULDNumber);
                                if (Session["ChildAWB_" + ULDNumber] != null)
                                    Session["ChildAWB_" + ULDNumber] = null;
                            }
                        }
                        Session["editgrid"] = null;
                    }
                    if (ds == true)
                    {
                        //passing parameter to add record in AWBMilestone
                        try
                        {

                            lblStatus.Text = "Arrival record saved successfully !";
                            lblStatus.ForeColor = Color.Green;
                            Session["editgrid"] = null;

                            //Vijay - Code to Create Dest agent invoice.
                            GenerateDestAgentInvoiceNumber();

                        }
                        catch (Exception)
                        { }
                    }
                    else
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "NotInserted()", true);
                        lblStatus.Text = "Please Select Row to add";
                        lblStatus.ForeColor = Color.Red;
                    }
                    #endregion
                }
                #endregion isselected
                else
                {
                    lblStatus.Text = "Please select atleast one row to Save arrival Pieces";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //Show popup to save actual operation time.
                if (CommonUtility.ShowOperationTimeOnSave != null && CommonUtility.ShowOperationTimeOnSave == true)
                    SaveOperationTime(true);
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
                lblStatus.Text = "Arrival record saved successfully !";
                lblStatus.ForeColor = Color.Green;

            }
            catch (Exception)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "Error()", true);
                lblStatus.Text = "Please check the Data ";
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                paramname = null;
                paramvalue = null;
                paramtype = null;
            }
        }

        #endregion Button save

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
                foreach (GridViewRow gvr in GVArrDet.Rows)
                {
                    if (((CheckBox)gvr.FindControl("check")).Checked)
                    {
                        objOpsTimeStamp = new SCM.Common.Struct.clsOperationTimeStamp();
                        if (((TextBox)gvr.FindControl("AWB")).Text != "" &&
                            (((TextBox)gvr.FindControl("AWB")).Text.Contains("-")) &&
                            (((TextBox)gvr.FindControl("AWB")).Text.Length > ((TextBox)gvr.FindControl("AWB")).Text.IndexOf('-')))
                        {
                            objOpsTimeStamp.AWBPrefix = ((TextBox)gvr.FindControl("AWB")).Text.Substring(0,
                                ((TextBox)gvr.FindControl("AWB")).Text.IndexOf('-'));
                            objOpsTimeStamp.AWBNumber = ((TextBox)gvr.FindControl("AWB")).Text.Substring(
                                ((TextBox)gvr.FindControl("AWB")).Text.IndexOf('-') + 1);
                        }
                        else
                        {
                            objOpsTimeStamp.AWBPrefix = "";
                            objOpsTimeStamp.AWBNumber = ((TextBox)gvr.FindControl("AWB")).Text;
                        }
                        objOpsTimeStamp.Description = "";
                        DateTime dt = System.DateTime.Now;
                        if (DateTime.TryParseExact(((TextBox)gvr.FindControl("FltDate")).Text, "dd/MM/yyyy", null,
                            System.Globalization.DateTimeStyles.None, out dt))
                        {
                            objOpsTimeStamp.FlightDt = dt;
                        }
                        else
                        {
                            objOpsTimeStamp.FlightDt = DateTime.Now;
                        }
                        objOpsTimeStamp.FlightNo = ((TextBox)gvr.FindControl("FlightNo")).Text;
                        objOpsTimeStamp.OperationalStatus = "ARR";
                        objOpsTimeStamp.OperationalType = "ARR";
                        objOpsTimeStamp.OperationDate = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy"); //DateTime.Now.ToString("dd/MM/yyyy");
                        objOpsTimeStamp.OperationTime = ((DateTime)Session["IT"]).ToString("HH:mm"); //DateTime.Now.ToString("HH:mm");
                        int pieceCount = 0;
                        if (int.TryParse(((TextBox)gvr.FindControl("RcvPcs")).Text, out pieceCount))
                        {
                            objOpsTimeStamp.Pieces = pieceCount;
                        }
                        else
                        {
                            objOpsTimeStamp.Pieces = 0;
                        }
                        decimal weight = 0;
                        if (decimal.TryParse(((TextBox)gvr.FindControl("RcvWt")).Text, out weight))
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
                foreach (GridViewRow gvr in grdULDDetails.Rows)
                {
                    if (((CheckBox)gvr.FindControl("checkULD")).Checked)
                    {
                        objOpsTimeStamp = new SCM.Common.Struct.clsOperationTimeStamp();
                        objOpsTimeStamp.ULDNumber = ((TextBox)gvr.FindControl("GrdULDNo")).Text;
                        objOpsTimeStamp.AWBPrefix = "";
                        objOpsTimeStamp.AWBNumber = "";
                        objOpsTimeStamp.Description = "";
                        DateTime dt = System.DateTime.Now;
                        if (DateTime.TryParseExact(((TextBox)gvr.FindControl("GrdULDFlightDate")).Text, "dd/MM/yyyy", null,
                            System.Globalization.DateTimeStyles.None, out dt))
                        {
                            objOpsTimeStamp.FlightDt = dt;
                        }
                        else
                        {
                            objOpsTimeStamp.FlightDt = DateTime.Now;
                        }
                        objOpsTimeStamp.FlightNo = ((TextBox)gvr.FindControl("GrdULDFlightNo")).Text;
                        objOpsTimeStamp.OperationalStatus = "ARR";
                        objOpsTimeStamp.OperationalType = "ARR";
                        objOpsTimeStamp.OperationDate = DateTime.Now.ToString("dd/MM/yyyy");
                        objOpsTimeStamp.OperationTime = DateTime.Now.ToString("HH:mm");
                        int pieceCount = 0;
                        if (int.TryParse(((TextBox)gvr.FindControl("GrdULDAWBPcs")).Text, out pieceCount))
                        {
                            objOpsTimeStamp.Pieces = pieceCount;
                        }
                        else
                        {
                            objOpsTimeStamp.Pieces = 0;
                        }
                        decimal weight = 0;
                        if (decimal.TryParse(((TextBox)gvr.FindControl("GrdULDAWBWt")).Text, out weight))
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
                lblStatus.ForeColor = Color.Red;
            }
        }


        #region Save Child AWBs
        private bool SaveChildAWBs(string strULDNo)
        {
           // DataTable dtChildAWB = null;
            DataTable dtChildAWB = new DataTable("Arrival_ChildAWBs_dt");

            try
            {
                //Check if session of Child AWBs for given ULD exists.
                if (Session["ChildAWB_" + strULDNo] != null)
                {
                    dtChildAWB = (DataTable)Session["ChildAWB_" + strULDNo];
                    if (dtChildAWB != null)
                    {   //Insert each manually added child awb in database.
                        foreach (DataRow drChild in dtChildAWB.Rows)
                        {
                            if (drChild["ManualAWB"].ToString() == "Y")
                            {
                                int intRecPcs = 0;
                                int.TryParse(drChild["RcvPcs"].ToString(), out intRecPcs);
                                float fltRecWt = 0;
                                float.TryParse(drChild["RcvWt"].ToString(), out fltRecWt);
                                //Insert data only if received pcs & weight is higher..
                                if (intRecPcs > 0 && fltRecWt > 0)
                                {
                                    string[] paramname = new string[30];
                                    object[] paramvalue = new object[30];
                                    SqlDbType[] paramtype = new SqlDbType[30];

                                    #region Paramname
                                    paramname[0] = "ULD";
                                    paramname[1] = "POL";
                                    paramname[2] = "ULDDestination";
                                    paramname[3] = "AWBNumber";
                                    paramname[4] = "Owner";
                                    paramname[5] = "MFTPieces";
                                    paramname[6] = "MFTWeight";
                                    paramname[7] = "RCVPieces";
                                    paramname[8] = "RCVWeight";
                                    paramname[9] = "Origin";
                                    paramname[10] = "Destination";
                                    paramname[11] = "StatedPieces";
                                    paramname[12] = "StatedWeight";
                                    paramname[13] = "Discrepancy";
                                    paramname[14] = "SecurityCheck";
                                    paramname[15] = "CustomCheck";
                                    paramname[16] = "CustomStatusCode";
                                    paramname[17] = "Remark";
                                    paramname[18] = "FltNo";
                                    paramname[19] = "status";
                                    paramname[20] = "operationType";
                                    paramname[21] = "Discription";
                                    paramname[22] = "ExpectedPcs";
                                    paramname[23] = "ExpectedWeight";
                                    paramname[24] = "FltDate";
                                    paramname[25] = "UpdatedBy";
                                    paramname[26] = "UpdatedOn";
                                    paramname[27] = "Location";
                                    paramname[28] = "CommodityCode";
                                    paramname[29] = "FltOrigin";
                                    #endregion Paramname

                                    #region Paramvalue

                                    paramvalue[0] = strULDNo;
                                    paramvalue[1] = drChild["POL"].ToString();
                                    paramvalue[2] = drChild["ULDdest"].ToString();
                                    if (paramvalue[2] == null || paramvalue[2].ToString() == "")
                                    {
                                        paramvalue[2] = Session["Station"].ToString();
                                    }
                                    paramvalue[3] = drChild["AWBno"].ToString();
                                    if (Session["editgrid"] != null)
                                    {
                                        string GridAWB = Session["editgrid"].ToString();
                                        if (GridAWB == drChild["AWBno"].ToString())
                                        {
                                            flag = 0;
                                        }
                                    }
                                    else
                                    {
                                        flag = 1;
                                    }
                                    paramvalue[4] = "";
                                    paramvalue[5] = drChild["PCS"].ToString();
                                    paramvalue[6] = drChild["GrossWgt"].ToString();
                                    paramvalue[7] = drChild["RcvPcs"].ToString();
                                    paramvalue[8] = drChild["RcvWt"].ToString();
                                    paramvalue[9] = drChild["Org"].ToString();
                                    paramvalue[10] = drChild["Dest"].ToString();
                                    paramvalue[11] = drChild["StatedPCS"].ToString();
                                    paramvalue[12] = drChild["StatedWgt"].ToString();
                                    paramvalue[13] = drChild["Discrepancy"].ToString();
                                    paramvalue[14] = drChild["SecurityCheck"].ToString();
                                    paramvalue[15] = drChild["CustomCheck"].ToString();
                                    paramvalue[16] = drChild["CustomStatusCode"].ToString();
                                    paramvalue[17] = drChild["ArrivalRemarks"].ToString();
                                    paramvalue[18] = drChild["FltNo"].ToString();
                                    paramvalue[19] = "C";
                                    paramvalue[20] = "";
                                    paramvalue[21] = drChild["Desc"].ToString();
                                    paramvalue[22] = drChild["ExpectedPcs"].ToString();
                                    paramvalue[23] = drChild["ExpectedWeight"].ToString();
                                    paramvalue[24] = DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", null);//.ToString("dd/MM/yyyy");
                                    paramvalue[25] = Session["UserName"].ToString();
                                    paramvalue[26] = dtCurrentDate;
                                    paramvalue[27] = drChild["Location"].ToString();
                                    paramvalue[28] = drChild["SCC"].ToString();
                                    paramvalue[29] = drChild["POL"].ToString();
                                    #endregion Paramvalue

                                    #region Paramtype
                                    paramtype[0] = SqlDbType.NVarChar;
                                    paramtype[1] = SqlDbType.NVarChar;
                                    paramtype[2] = SqlDbType.VarChar;
                                    paramtype[3] = SqlDbType.VarChar;
                                    paramtype[4] = SqlDbType.VarChar;
                                    paramtype[5] = SqlDbType.Int;
                                    paramtype[6] = SqlDbType.Float;
                                    paramtype[7] = SqlDbType.Int;
                                    paramtype[8] = SqlDbType.Float;
                                    paramtype[9] = SqlDbType.VarChar;
                                    paramtype[10] = SqlDbType.VarChar;
                                    paramtype[11] = SqlDbType.Int;
                                    paramtype[12] = SqlDbType.Float;
                                    paramtype[13] = SqlDbType.VarChar;
                                    paramtype[14] = SqlDbType.Bit;
                                    paramtype[15] = SqlDbType.Bit;
                                    paramtype[16] = SqlDbType.VarChar;
                                    paramtype[17] = SqlDbType.VarChar;
                                    paramtype[18] = SqlDbType.VarChar;
                                    paramtype[19] = SqlDbType.VarChar;
                                    paramtype[20] = SqlDbType.VarChar;
                                    paramtype[21] = SqlDbType.VarChar;
                                    paramtype[22] = SqlDbType.Int;
                                    paramtype[23] = SqlDbType.Float;
                                    paramtype[24] = SqlDbType.DateTime;
                                    paramtype[25] = SqlDbType.VarChar;
                                    paramtype[26] = SqlDbType.DateTime;
                                    paramtype[27] = SqlDbType.VarChar;
                                    paramtype[28] = SqlDbType.VarChar;
                                    paramtype[29] = SqlDbType.VarChar;
                                    #endregion Paramtype

                                    da.InsertData("Sp_InsertChildAWBArrival", paramname, paramtype, paramvalue);
                                    paramname = null;
                                    paramtype = null;
                                    paramvalue = null;
                                }
                            }
                        }
                        if (dtChildAWB != null)
                            dtChildAWB.Dispose();
                    }
                    Session["ChildAWB_" + strULDNo] = null;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error in saving Child AWBs: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
                return (false);
            }
            return (true);
        }
        #endregion Save Child AWBs

        #region Save HAWBs
        private bool SaveHAWBs(string AWBNo, string FltOrigin, string FltDest, string FltNo, string FltDt)
        {
            //DataTable dtHAWB = null;
            DataTable dtHAWB = new DataTable("Arr_SaveHAWBs_dtHAWB");

            try
            {
                string AWBPrefix = "";
                string AWBNumber = "";
                if (AWBNo.Contains("-"))
                {
                    AWBPrefix = AWBNo.Substring(0, AWBNo.IndexOf('-'));
                }
                if (AWBNo.Length > 8)
                {
                    AWBNumber = AWBNo.Substring(AWBNo.Length - 8);
                }
                //Check if session of HAWBs for given AWB exists.
                if (Session["ARR_HAWBDetails" + AWBNo] != null)
                {
                    dtHAWB = (DataTable)Session["ARR_HAWBDetails" + AWBNo];
                    if (dtHAWB != null)
                    {   //Insert each manually added child awb in database.
                        foreach (DataRow drChild in dtHAWB.Rows)
                        {
                            BAL.BALHAWBDetails HAWB = new BALHAWBDetails();
                            if (drChild["ManualHAWB"].ToString() == "Y")
                            {   //Check if not dummy HAWB.    
                                if (drChild["HAWBNo"].ToString().ToUpper() != "DUMMY")
                                {
                                    //Insert data only if received pcs & weight is higher..
                                    HAWB.PutHAWBDetails(AWBNumber, drChild["HAWBNo"].ToString(), int.Parse(drChild["HAWBPcs"].ToString()),
                                            float.Parse(drChild["HAWBWt"].ToString()), drChild["Description"].ToString(),
                                            drChild["CustID"].ToString(), drChild["CustName"].ToString(),
                                            drChild["CustAddress"].ToString(), drChild["CustCity"].ToString(),
                                            drChild["Zipcode"].ToString(), drChild["Origin"].ToString(),
                                            drChild["Destination"].ToString(), drChild["SHC"].ToString(),
                                            drChild["HAWBPrefix"].ToString(), AWBPrefix, FltOrigin, FltDest, "C",
                                            FltNo, FltDt);
                                }
                            }
                            else
                            {   //Update Arrival Status.
                                if (Convert.ToBoolean(drChild["ArrivalStatus"]) == true)
                                {
                                    HAWB.UpdateHAWBArrival(drChild["HAWBPrefix"].ToString(), drChild["HAWBNo"].ToString(),
                                    AWBNo, "C", Session["UserName"].ToString(), Session["IT"].ToString());
                                }
                            }
                        }
                        if (dtHAWB != null)
                            dtHAWB.Dispose();
                    }
                    Session["ARR_HAWBDetails" + AWBNo] = null;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error in saving HAWBs: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
                return (false);
            }
            return (true);
        }
        #endregion Save Child AWBs

        #region validateAndInsertFFMData
        /*
         *Updated on 5/8/2013
         *Data insert into ExportManifestTable
         */
        private bool validateAndInsertFFMData(int AWBRowIndex)
        {
            try
            {
                string flightnum = ((TextBox)GVArrDet.Rows[AWBRowIndex].FindControl("FlightNo")).Text;
                string date = ((TextBox)GVArrDet.Rows[AWBRowIndex].FindControl("FltDate")).Text;

                string source = "", dest = "";
                string[] PName = new string[] { "flightnum", "date" };
                SqlDbType[] PType = new SqlDbType[] { SqlDbType.NVarChar, SqlDbType.VarChar };
                object[] PValue = new object[] { flightnum, date };

                DataSet ds = new DataSet("Arr_Validate_ds1");
                ds = da.SelectRecords("spGetDestCodeForFFM", PName, PValue, PType);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            source = ds.Tables[0].Rows[0]["source"].ToString();
                            dest = ds.Tables[0].Rows[0]["Dest"].ToString();
                        }
                    }
                }

                source = ((TextBox)GVArrDet.Rows[AWBRowIndex].FindControl("POL")).Text;

                string origin = ((TextBox)GVArrDet.Rows[AWBRowIndex].FindControl("Origin")).Text;
                string awbdest = ((TextBox)GVArrDet.Rows[AWBRowIndex].FindControl("Destn")).Text;
                string awbnum = ((TextBox)GVArrDet.Rows[AWBRowIndex].FindControl("AWB")).Text;
                string unloadingairport = Session["Station"].ToString();
                string mftpcscnt = ((TextBox)GVArrDet.Rows[AWBRowIndex].FindControl("MftPcs")).Text;
                string mftweight = ((TextBox)GVArrDet.Rows[AWBRowIndex].FindControl("MftWt")).Text;
                string SCC = ((TextBox)GVArrDet.Rows[AWBRowIndex].FindControl("CommCode")).Text;
                string awbprefix = "";
                if (awbnum.Contains("-"))
                {
                    awbprefix = awbnum.Substring(0, awbnum.IndexOf("-"));
                }
                string awbnumwoprefix = "";
                if (awbnum.Contains("-"))
                {
                    awbnumwoprefix = awbnum.Substring(awbnum.IndexOf("-") + 1);
                }
                try
                {
                    int ManifestID = ExportManifestSummary(flightnum, source, unloadingairport, 
                        DateTime.ParseExact(date, "dd/MM/yyyy", null),true);
                    if (ManifestID > 0)
                    {

                        #region Check Availabe ULD data if Present insert into ExportManifestULDAWB Association
                        string ULDNo = "Bulk";
                        try
                        {
                            if (ExportManifestULDAWBAssociation(ULDNo, source, unloadingairport, origin, awbdest, awbnumwoprefix,
                                SCC, "0", mftpcscnt, mftweight, "GEN", date, ManifestID, awbprefix, flightnum))
                            {
                                if (!ULDawbAssociation(flightnum, origin, unloadingairport, awbnumwoprefix,
                                    mftpcscnt, mftweight, DateTime.ParseExact(date, "dd/MM/yyyy", null), ULDNo))
                                {
                                    lblStatus.Text = "Error [1] in FFM ULDawbAssociation Data:" + ULDNo;
                                    lblStatus.ForeColor = Color.Red;
                                    return (false);
                                }
                            }
                            else
                            {
                                lblStatus.Text = "Error [2] in generating Manifest ULD details: " + awbnum;
                                lblStatus.ForeColor = Color.Red;
                                return (false);
                            }
                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = "Error [3] in generating Manifest Details & Summary for ULD: " + ex.Message;
                            lblStatus.ForeColor = Color.Red;
                            return (false);
                        }
                        #endregion

                        #region Add Consigment Details
                        try
                        {

                            #region Store in Manifest Tables
                            try
                            {
                                if (!ExportManifestDetails(ULDNo, source, unloadingairport, origin, awbdest,
                                    awbnumwoprefix, SCC, "0", mftpcscnt, mftweight, "GEN", DateTime.ParseExact(date, "dd/MM/yyyy", null),
                                    ManifestID, flightnum, awbprefix))
                                {
                                    lblStatus.Text = "Error [4] in generating Manifest Details for AWB:" + awbnum;
                                    lblStatus.ForeColor = Color.Red;
                                    return (false);
                                }
                            }
                            catch (Exception ex)
                            {
                                lblStatus.Text = "Error [5] in generating Manifest Details: " + ex.Message;
                                lblStatus.ForeColor = Color.Red;
                                return (false);
                            }
                            #endregion

                            #region Status Message in Table
                            string[] PVName = new string[] { "AWBPrefix", "AWBNumber", "MType", "desc", "date", "time", "refno" };
                            object[] PValues = new object[] { awbprefix, awbnumwoprefix, "M ARR", origin + "-" + flightnum + "-" + date, "", "", 0 };
                            SqlDbType[] sqlType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.NVarChar, SqlDbType.NVarChar, SqlDbType.Int };
                            da.InsertData("spInsertAWBMessageStatus", PVName, sqlType, PValues);

                            #endregion

                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = "Error in creating Manifest [6]: " + ex.Message;
                            lblStatus.ForeColor = Color.Red;
                            return (false);
                        }
                        #endregion

                        #region Add Consigment Details
                        try
                        {

                            #region Store in booking table

                            bool isAWBPresent = false;

                            #region Check AWB Present or Not
                            DataSet dsCheck = new DataSet("Arr_Validate_dsCheck");

                            string[] pname = new string[] { "AWBNumber", "AWBPrefix" };
                            object[] values = new object[] { awbnumwoprefix, awbprefix };
                            SqlDbType[] ptype = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                            dsCheck = da.SelectRecords("sp_getawbdetails", pname, values, ptype);

                            if (dsCheck != null)
                            {
                                if (dsCheck.Tables.Count > 0)
                                {
                                    if (dsCheck.Tables[0].Rows.Count > 0)
                                    {
                                        if (dsCheck.Tables[0].Rows[0]["AWBNumber"].ToString().Equals(awbnumwoprefix, StringComparison.OrdinalIgnoreCase))
                                        {
                                            isAWBPresent = true;
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Add AWB details
                            if (!isAWBPresent)
                            {
                                string[] paramname = new string[] { "AirlinePrefix", "AWBNum", "Origin", "Dest", "PcsCount", 
                                    "Weight", "Volume","ComodityCode", "ComodityDesc", "CarrierCode", "FlightNum", 
                                    "FlightDate", "FlightOrigin", "FlightDest","ShipperName", "ShipperAddr", "ShipperPlace", 
                                    "ShipperState", "ShipperCountryCode", "ShipperContactNo", "ConsName", "ConsAddr", 
                                    "ConsPlace","ConsState", "ConsCountryCode", "ConsContactNo", "CustAccNo", 
                                    "IATACargoAgentCode", "CustName", "SystemDate", "MeasureUnit", "Length", "Breadth", 
                                    "Height", "PartnerStatus","REFNo"};

                                object[] paramvalue = new object[] { awbprefix,awbnumwoprefix,origin,awbdest,mftpcscnt,
                                    mftweight,"",SCC,"", flightnum.Substring(0,2), flightnum, date, 
                                    source,dest,"","","","","","", "","", "","","","","", "", "", 
                                    Convert.ToDateTime(Session["IT"]).ToString("yyyy-MM-dd"),"", "", "", "", "" ,0};

                                SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,
                                              SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,
                                              SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,SqlDbType.Int };

                                string procedure = "spInsertBookingDataFromFFR";
                                if (!da.InsertData(procedure, paramname, paramtype, paramvalue))
                                {
                                    lblStatus.Text = "Error [7] in generating AWB record for: " + awbnum;
                                    lblStatus.ForeColor = Color.Red;
                                }
                            }
                            #endregion

                            #endregion

                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = "Error [8] in generating booking record: " + ex.Message;
                            lblStatus.ForeColor = Color.Red;
                            return (false);
                        }
                        #endregion
                    }


                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error [9] in StoreImportFFM_Summary Data: " + ex.Message;
                    lblStatus.ForeColor = Color.Red;
                    return (false);
                }
                return (true);
            }
            catch (Exception ex)
            {
                lblStatus.Text = ("Error [10] in generating manifest record in ValidateAndInsertFFM: " + ex.Message);
                lblStatus.ForeColor = Color.Red;
                return (false);
            }
        }

        public bool ULDawbAssociation(string FltNo, string POL, string POU, string AWBno, string PCS, string WGT, DateTime FltDate, string ULDNo)
        {
            bool res;
            try
            {
                string[] param = { "ULDtripid", "ULDNo", "AWBNumber", "POL", "POU", "FltNo", "Pcs", "Wgt", "AvlPcs", 
                                     "AvlWgt", "Updatedon", "Updatedby", "Status", "Manifested", "FltDate" };

                int _pcs = int.Parse(PCS);
                float _wgt = float.Parse(WGT);
                SqlDbType[] sqldbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar
                                             , SqlDbType.Int, SqlDbType.Float, SqlDbType.Int, SqlDbType.Float,SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.Bit,SqlDbType.Bit, SqlDbType.DateTime };
                object[] values = { "", ULDNo, AWBno, POL, POU, FltNo, 0, 0, _pcs, _wgt, DateTime.Now, "FFM", false, false, FltDate };


                //res = db.InsertData("SPImpManiSaveUldAwbAssociation", param, sqldbtypes, values);
                if (da.InsertData("SPImpManiSaveUldAwbAssociation", param, sqldbtypes, values))
                {
                    res = true;
                }
                else
                {
                    clsLog.WriteLog("Failes ULDAWBAssociation Save:" + AWBno + " Error: " + da.LastErrorDescription);
                    res = false;
                }

            }
            catch (Exception ex)
            {
                clsLog.WriteLog("Error in FFM ULDAWBAssociation:" + ex.Message);
                res = false;
            }
            return res;
        }

        #region FFM data to ExportManifest

        #region Export Manifest Summary Data Insert
        public int ExportManifestSummary(string FlightNo, string POL, string POU, DateTime FltDate, bool ManualArrival)
        {
            int ID = 0;
            try
            {
                SQLServer db = new SQLServer(Global.GetConnectionString());
                string[] param = { "FLTno", "POL", "POU", "FLTDate", "ManualArrival" };
                SqlDbType[] sqldbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, 
                                             SqlDbType.DateTime, SqlDbType.Bit };
                object[] values = { FlightNo, POL, POU, FltDate, ManualArrival };

                ID = db.GetIntegerByProcedure("spExpManifestSummaryFFM", param, values, sqldbtypes);
                if (ID < 1)
                {
                    clsLog.WriteLog("Error saving ExportFFM:" + db.LastErrorDescription);
                }
                //res = db.InsertData("SPImpManiSaveManifest", param, sqldbtypes, values);


            }
            catch (Exception ex)
            {
                clsLog.WriteLog("Error saving ExpFFM:" + ex.Message);
                ID = 0;

            }
            return ID;
        }
        #endregion

        #region ExportManifestDetails
        public bool ExportManifestDetails(string ULDNo, string POL, string POU, string ORG, string DES, string AWBno,
            string SCC, string VOL, string PCS, string WGT, string Desc, DateTime FltDate, int ManifestID,
            string FlightNo, string AWBPrefix)
        {
            bool res;
            try
            {
                string[] param = { "POL", "POU", "ORG", "DES", "AWBno", "SCC", "VOL", "PCS", "WGT", "Desc", "FLTDate", 
                                     "ManifestID", "FlightNo", "AWBPrefix", "Source" };
                SqlDbType[] sqldbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, 
                                             SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, 
                                             SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.Int, 
                                             SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { POL, POU, ORG, DES, AWBno, SCC, VOL, PCS, WGT, Desc, FltDate, ManifestID, FlightNo, 
                                      AWBPrefix, "A" };
                //res = db.InsertData("spExpManifestDetailsFFM", param, sqldbtypes, values);
                if (da.InsertData("spExpManifestDetailsFFM", param, sqldbtypes, values))
                {
                    res = true;
                }
                else
                {
                    clsLog.WriteLog("Failes ManifDetails Save:" + AWBno + " Error: " + da.LastErrorDescription);
                    res = false;
                }
            }
            catch (Exception ex)
            {
                clsLog.WriteLog("Error saving ImportFFM [" + DateTime.Now + "]");
                res = false;
            }
            return res;
        }
        #endregion

        #region ExportManifestULDAWBAssociation
        public bool ExportManifestULDAWBAssociation(string ULDNo, string POL, string POU, string ORG, string DES, string AWBno, string SCC, string VOL, string PCS, string WGT, string Desc, string FltDate, int ManifestID, string awbprefix, string flightno)
        {
            bool res;
            try
            {
                string[] param = { "ULDNo", "POL", "POU", "ORG", 
                                     "DES", "AWBno", "SCC", "VOL", 
                                     "PCS", "WGT", "Desc", "FLTDate", 
                                     "ManifestID", "AWBPrefix", "FlightNo", "Source"};
                SqlDbType[] sqldbtypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, 
                                             SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, 
                                             SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, 
                                             SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                object[] values = { ULDNo, POL, POU, ORG, 
                                     DES, AWBno, SCC, VOL, 
                                     PCS, WGT, Desc, FltDate, 
                                     ManifestID, awbprefix, flightno,"A" };
                //res = db.InsertData("spExpManifestULDAWBFFM", param, sqldbtypes, values);
                if (da.InsertData("spExpManifestULDAWBFFM", param, sqldbtypes, values))
                {
                    res = true;
                }
                else
                {
                    clsLog.WriteLog("Failes ManifDetails Save:" + AWBno + " Error: " + da.LastErrorDescription);
                    res = false;
                }
            }
            catch (Exception ex)
            {
                clsLog.WriteLog("Error saving EXPULDAWB:" + ex.Message);
                res = false;
            }
            return res;
        }
        #endregion

        #endregion

        #endregion validateAndInsertFFMData

        //Vijay Work
        #region Generate Dest Agent Invoice Number
        protected void GenerateDestAgentInvoiceNumber()
        {
            string strFltdate;
            object[] AwbRateInfo = null;
            //DataSet dsAWBData = null;

            DataSet dsAWBData = new DataSet("Arr_InvNo_ds1");

            try
            {
                #region Prepare Parameters
                AwbRateInfo = new object[2];
                int i = 0;

                AwbRateInfo.SetValue(txtFlightPrefix.Text.Trim() + txtFlightNo.Text.Trim(), i);
                i++;

                //Validation for Flight date
                if (txtFlightDate.Text == "")
                {
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dt;
                try
                {
                    //Change 03082012
                    string day = txtFlightDate.Text.Substring(0, 2);
                    string mon = txtFlightDate.Text.Substring(3, 2);
                    string yr = txtFlightDate.Text.Substring(6, 4);
                    strFltdate = yr + "-" + mon + "-" + day;
                    dt = Convert.ToDateTime(strFltdate);

                }
                catch (Exception)
                {
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                AwbRateInfo.SetValue(Convert.ToDateTime(strFltdate).ToString("yyyy-MM-dd HH:mm:ss"), i);

                #endregion Prepare Parameters

                dsAWBData = BlArl.GetArrivedCCAWBData(AwbRateInfo);
                if (dsAWBData != null)
                {
                    if (dsAWBData.Tables != null)
                    {
                        if (dsAWBData.Tables.Count > 0)
                        {
                            if (dsAWBData.Tables[0].Rows.Count > 0)
                            {
                                try
                                {
                                    string res = "";
                                    string InvoiceNumList = "";
                                    for (int cnt = 0; cnt < dsAWBData.Tables[0].Rows.Count; cnt++)
                                    {
                                        //Generate Invoice Number (WALKIN Agent) for each AWB
                                        #region Prepare Parameters
                                        object[] AwbInfo = new object[5];
                                        int j = 0;

                                        AwbInfo.SetValue(dsAWBData.Tables[0].Rows[cnt]["AWBNumber"].ToString() + "-" + dsAWBData.Tables[0].Rows[cnt]["AWBPrefix"].ToString(), j);
                                        j++;
                                        AwbInfo.SetValue(Session["UserName"].ToString(), j);
                                        j++;
                                        AwbInfo.SetValue(Convert.ToDateTime(Session["IT"].ToString()), j);
                                        j++;
                                        AwbInfo.SetValue("", j);
                                        j++;
                                        AwbInfo.SetValue(Session["Station"].ToString(), j);
                                        
                                        #endregion Prepare Parameters

                                        res = BlArl.InsertArrivedCCAWBData(AwbInfo);

                                        //Generate Invoice Number (WALKIN Agent) for each AWB
                                        #region Prepare Parameters
                                        object[] AwbInvoiceInfo = new object[2];
                                        int k = 0;

                                        string AWBNumber = dsAWBData.Tables[0].Rows[cnt]["AWBPrefix"].ToString().Trim() + dsAWBData.Tables[0].Rows[cnt]["AWBNumber"].ToString().Trim();
                                        AwbInvoiceInfo.SetValue(AWBNumber, k);
                                        k++;

                                        //Set UpdatedOn
                                        AwbInvoiceInfo.SetValue(Convert.ToDateTime(Session["IT"].ToString()), k);

                                        #endregion Prepare Parameters

                                        if (InvoiceNumList == "")
                                        {
                                            InvoiceNumList = InvoiceNumList + BlArl.GenerateBunchInvoiceNumDestAgent(AwbInvoiceInfo);
                                        }
                                        else
                                        {
                                            InvoiceNumList = InvoiceNumList + "," + BlArl.GenerateBunchInvoiceNumDestAgent(AwbInvoiceInfo);
                                        }
                                        AwbInvoiceInfo = null;
                                        AwbInfo = null;
                                    }

                                    //Code to Generate Invoice (WALKIN Agent)
                                    hfInvoiceNos.Value = InvoiceNumList;

                                    #region Show/Hide CC Invoice Popup to print
                                    try
                                    {

                                        if (Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ShowCCInvoiceArrival")))
                                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "GenerateInvoices();", true);
                                    }
                                    catch (Exception ex)
                                    { }
                                    #endregion

                                }
                                catch
                                {
                                }
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
                if (dsAWBData != null)
                {
                    dsAWBData.Dispose();
                }
                AwbRateInfo = null;
            }
        }
        #endregion Generate Dest Agent Invoice Number

        /// <summary>
        /// Print Arrival Manifest
        /// </summary>
        #region Function to Show manifest Data
        private void showManifestData()
        {
            try
            {
                DateTime dtFlightDate = DateTime.ParseExact(txtFlightDate.Text.Trim(), "dd/MM/yyyy", null);

                Session["DTExport"] = "";
                Session["DTAWBDetails"] = "";

                DataTable DTExport = new DataTable("Arr_FunMnf_dtExp");

                DTExport.Columns.Add("Owner");
                DTExport.Columns.Add("Nationality");
                DTExport.Columns.Add("FlightNo");
                DTExport.Columns.Add("LoadingPt");
                DTExport.Columns.Add("UnloadingPt");
                DTExport.Columns.Add("TotalWeight");
                DTExport.Columns.Add("PreparedBy");
                DTExport.Columns.Add("Total");
                DTExport.Columns.Add("UnloadAt");


                DataTable DTAWBDetails = new DataTable("Arr_FunMnf_dtAWB");

                DTAWBDetails.Columns.Add("AWBNo");
                DTAWBDetails.Columns.Add("NoOfPcs");
                DTAWBDetails.Columns.Add("Description");
                DTAWBDetails.Columns.Add("Weight");
                DTAWBDetails.Columns.Add("Org");
                DTAWBDetails.Columns.Add("Dest");
                DTAWBDetails.Columns.Add("NextFlight");
                DTAWBDetails.Columns.Add("SCC");
                DTAWBDetails.Columns.Add("Remark");
                DTAWBDetails.Columns.Add("TotalPCS");
                DTAWBDetails.Columns.Add("TotalWeight");
                DTAWBDetails.Columns.Add("BookedPCS");
                DTAWBDetails.Columns.Add("BookedWt");
                DTAWBDetails.Columns.Add("BookedTotalPcs");
                DTAWBDetails.Columns.Add("BookedTotalWt");
                DTAWBDetails.Columns.Add("Consignee");
                DTAWBDetails.Columns.Add("Location"); //New Column

                DataTable DTULDDetails = new DataTable("Arr_FunMnf_dtULD");

                DTULDDetails.Columns.Add("ULDNo");
                DTULDDetails.Columns.Add("NoOfPcs");
                DTULDDetails.Columns.Add("Weight");
                DTULDDetails.Columns.Add("BookedPCS");
                DTULDDetails.Columns.Add("BookedWt");
                DTULDDetails.Columns.Add("Description");
                DTULDDetails.Columns.Add("Org");
                DTULDDetails.Columns.Add("Dest");
                DTULDDetails.Columns.Add("NextFlight");
                DTULDDetails.Columns.Add("SCC");
                DTULDDetails.Columns.Add("Remark");
                DTULDDetails.Columns.Add("Consignee");
                DTULDDetails.Columns.Add("TotalPCS");
                DTULDDetails.Columns.Add("TotalWt");
                DTULDDetails.Columns.Add("Location"); //New Column

                string strOwner = "";
                //DataSet dsAirlineDetails = null; 
                DataSet dsAirlineDetails = new DataSet("Arr_FunMnf_dtAirLn");

                try
                {
                    dsAirlineDetails = objMst.GetAirlineDetails("", "");
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
                catch (Exception)
                {
                    strOwner = "NIL";
                }
                finally
                {
                    if (dsAirlineDetails != null)
                    {
                        dsAirlineDetails.Dispose();
                    }
                }
                string strNationality = Convert.ToString(Session["FltRegNo"]);
                if (strNationality == "" || strNationality == null)
                {
                    strNationality = "NIL";
                }
                string strFlightNo = txtFlightPrefix.Text.ToUpper() + txtFlightNo.Text.ToUpper() + " " + " " + txtFlightDate.Text.Trim();
                string strLoadingPt = Convert.ToString(Session["FltOrigin"]);
                if (strLoadingPt == "" || strLoadingPt == null)
                {
                    strLoadingPt = "NIL";
                }
                string strUnloadPt = Convert.ToString(Session["Station"]);

                //need to change
                float fltTotalWeight = 0;
                float fltTotalPCS = 0;
                float BookedTotalPcs = 0;
                decimal BookedTotalWt = 0;
                string strPreparedBy = Session["Username"].ToString();
                float strTotal = 0;
                string UnloadAt = "";
                if (UnloadAt == "" || UnloadAt == null)
                {
                    UnloadAt = "NIL";
                }
                //DataSet DsNationality = null; 
                DataSet DsNationality = new DataSet("Arr_shwMnf_Nlty");
                try
                {
                    DsNationality = objExpMani.GetPOUAirlineSchedule(txtFlightPrefix.Text + txtFlightNo.Text, strLoadingPt, dtFlightDate);

                    strNationality = DsNationality.Tables[1].Rows[0][0].ToString();
                }
                catch (Exception)
                {
                    strNationality = "NIL";
                }
                finally
                {
                    if (DsNationality != null)
                    {
                        DsNationality.Dispose();
                    }
                }

                string FlightNo = txtFlightPrefix.Text + txtFlightNo.Text;
                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {

                    string strDest = "";

                    strDest = ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text.ToUpper();
                    if (strDest == "" || strDest == null)
                    {
                        strDest = "NIL";
                    }

                    if (!ULDDestpt.Contains(((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Text))
                    {
                        UnloadAt = ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text;
                        ULDDestpt.Add(GVArrDet.Rows[i].Cells[11].Text);
                    }

                    string strAWBNo = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text;
                    if (strAWBNo == "" || strAWBNo == null)
                    {
                        strAWBNo = "NIL";
                    }
                    string strOrg = ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Text.ToUpper();
                    if (strOrg == "" || strOrg == null)
                    {
                        strOrg = "NIL";
                    }
                    string strSCC = "";
                    string Consignee = "";
                    strSCC = ((TextBox)GVArrDet.Rows[i].FindControl("CommCode")).Text;
                    //Get consignee code for AWB.
                    Consignee = BlArl.GetConsignee(((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text);

                    if (strSCC == "" || strSCC == null)
                    {
                        strSCC = "NIL";
                    }
                    if (Consignee == "" || Consignee == null)
                    {
                        Consignee = "NIL";
                    }
                    string strNoOfPCS = ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Text;
                    try
                    {
                        fltTotalPCS = fltTotalPCS + float.Parse(strNoOfPCS);
                    }
                    catch (Exception)
                    {
                        fltTotalPCS = 0;
                    }
                    string strWeight = ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Text;
                    try
                    {
                        fltTotalWeight = fltTotalWeight + float.Parse(strWeight);
                    }
                    catch (Exception)
                    {
                        fltTotalWeight = 0;
                    }

                    int BookedPcs;
                    try
                    {
                        BookedPcs = Convert.ToInt32(((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Text);
                    }
                    catch (Exception)
                    {
                        BookedPcs = 0;
                    }
                    try
                    {
                        BookedTotalPcs = BookedTotalPcs + BookedPcs;
                    }
                    catch (Exception)
                    {
                        BookedTotalPcs = 0;
                    }
                    decimal BookedWt;
                    try
                    {
                        BookedWt = Convert.ToDecimal(((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Text);
                    }
                    catch (Exception)
                    {
                        BookedWt = 0;
                    }
                    try
                    {
                        BookedTotalWt = BookedTotalWt + (BookedWt);
                    }
                    catch (Exception)
                    {
                        BookedTotalWt = 0;
                    }

                    string strDescription = "";
                    strDescription = ((Label)GVArrDet.Rows[i].FindControl("CommDesc")).Text;
                    if (strDescription == "" || strDescription == null)
                    {
                        strDescription = "NIL";
                    }
                    string strRemark = "";
                    strRemark = ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Text;
                    if (strRemark == "" || strRemark == null)
                    {
                        strRemark = "NIL";
                    }
                    string strNextflight = ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Text;
                    if (strNextflight == "" || strNextflight == null)
                    {
                        strNextflight = "NIL";
                    }
                    string location = ((TextBox)GVArrDet.Rows[i].FindControl("txtLocation")).Text.ToUpper(); //New

                    DTAWBDetails.Rows.Add(strAWBNo, strNoOfPCS, strDescription, strWeight, strOrg, strDest, strNextflight,
                        strSCC, strRemark, fltTotalPCS, fltTotalWeight, BookedPcs, BookedWt, BookedTotalPcs, BookedTotalWt,
                        Consignee, location);

                    //Session["DTAWBDetails"] = DTAWBDetails;
                    dtTable2 = DTAWBDetails;
                }

                #region ULDDetails
                int totalPCS = 0;
                decimal totalWt = 0;
                for (int i = 0; i < grdULDDetails.Rows.Count; i++)
                {
                    string strDest = "";
                    strDest = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDDestn")).Text.ToUpper();
                    if (strDest == "" || strDest == null)
                    {
                        strDest = "NIL";
                    }

                    if (!ULDDestpt.Contains(((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDDestn")).Text))
                    {
                        UnloadAt = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDLocation")).Text;
                        ULDDestpt.Add(grdULDDetails.Rows[i].Cells[11].Text);
                    }

                    string strULDNo = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Text;
                    if (strULDNo == "" || strULDNo == null)
                    {
                        strULDNo = "NIL";
                    }
                    string strOrg = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDPOL")).Text.ToUpper();
                    if (strOrg == "" || strOrg == null)
                    {
                        strOrg = "NIL";
                    }

                    string strSCC = "";
                    string Consignee = "";
                    string ULDNo = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Text;

                    strSCC = "";
                    if (strSCC == "" || strSCC == null)
                    {
                        strSCC = "NIL";
                    }
                    int BookedPcs;
                    try
                    {
                        BookedPcs = Convert.ToInt32(((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBPcs")).Text);
                        totalPCS = totalPCS + BookedPcs;
                    }
                    catch (Exception)
                    {
                        BookedPcs = 0;
                    }
                    decimal BookedWt;
                    try
                    {
                        BookedWt = Convert.ToDecimal(((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBWt")).Text);
                        totalWt = totalWt + BookedWt;
                    }
                    catch (Exception)
                    {
                        BookedWt = 0;
                    }
                    string location = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDLocation")).Text.ToUpper(); //New

                    DTULDDetails.Rows.Add(strULDNo, BookedPcs, BookedWt, BookedPcs, BookedWt, "", strOrg,
                        strDest, "", strSCC, ((TextBox)grdULDDetails.Rows[i].FindControl("Remark")).Text, Consignee,
                        totalPCS, totalWt, location);

                    //Session["DTULDDetails"] = DTULDDetails;
                    dtTable3 = DTULDDetails;

                }
                #endregion

                UnloadAt = UnloadAt.Remove(UnloadAt.Length, 0);
                DTExport.Rows.Add(strOwner, strNationality, strFlightNo, strLoadingPt,
                    strUnloadPt, fltTotalWeight, strPreparedBy, strTotal, UnloadAt);
                //Session["DTExport"] = DTExport;
                dtTable1 = DTExport;

                if (DTExport != null)
                    DTExport.Dispose();
                if (DTAWBDetails != null)
                    DTAWBDetails.Dispose();
                if (DTULDDetails != null)
                    DTULDDetails.Dispose();

            }
            catch (Exception)
            {
                lblStatus.Text = "Error";
            }
        }
        #endregion

        #region Load ULD Grid
        public void LoadULDGrid()
        {
            try
            {
                DataTable myDataTable = new DataTable("Arr_Lduld_dt1");
                DataColumn myDataColumn;

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ULDNo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltNo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltDate";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Org";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Dest";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "POL";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ULDWt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ScaleWt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AWBCount";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AWBPcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AWBWt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrivalSecurityCheck";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrivalCustomCheck";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrivalCustomStatusCode";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrivalRemark";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Location";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "IsArrived";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "IsReceived";
                myDataTable.Columns.Add(myDataColumn);

                DataRow dr;
                dr = myDataTable.NewRow();
                dr["ULDNo"] = "";
                dr["FltNo"] = "";// "5";
                dr["FltDate"] = "";
                dr["Org"] = "";
                dr["Dest"] = "";
                dr["POL"] = "";//"5";
                dr["ULDWt"] = "";
                dr["ScaleWt"] = "";
                dr["AWBCount"] = "";
                dr["AWBPcs"] = "";
                dr["AWBWt"] = "";
                dr["ArrivalSecurityCheck"] = "";
                dr["ArrivalCustomCheck"] = "";
                dr["ArrivalCustomStatusCode"] = "";
                dr["ArrivalRemark"] = "";
                dr["Location"] = "";
                dr["IsArrived"] = "";
                dr["IsReceived"] = "";

                myDataTable.Rows.Add(dr);

                //Bind the DataTable to the Grid
                grdULDDetails.DataSource = null;
                grdULDDetails.DataSource = myDataTable;
                grdULDDetails.DataBind();
                Session["dsULDData"] = myDataTable.Copy();

                //Enable controls if user wants to enter new ULD.
                ((TextBox)grdULDDetails.Rows[0].FindControl("GrdULDNo")).Enabled = true;
                ((TextBox)grdULDDetails.Rows[0].FindControl("GrdULDFlightNo")).Enabled = true;
                ((TextBox)grdULDDetails.Rows[0].FindControl("GrdULDFlightDate")).Enabled = true;
                ((TextBox)grdULDDetails.Rows[0].FindControl("GrdULDOrg")).Enabled = true;
                ((TextBox)grdULDDetails.Rows[0].FindControl("GrdULDPOL")).Enabled = true;
                ((TextBox)grdULDDetails.Rows[0].FindControl("GrdULDDestn")).Enabled = true;
                ((TextBox)grdULDDetails.Rows[0].FindControl("GrdULDULDWt")).Enabled = true;
                ((TextBox)grdULDDetails.Rows[0].FindControl("GrdULDScaleWt")).Enabled = true;
                ((TextBox)grdULDDetails.Rows[0].FindControl("GrdULDAWBCount")).Enabled = true;
                ((TextBox)grdULDDetails.Rows[0].FindControl("GrdULDAWBPcs")).Enabled = true;
                ((TextBox)grdULDDetails.Rows[0].FindControl("GrdULDAWBWt")).Enabled = true;
                ((CheckBox)grdULDDetails.Rows[0].FindControl("SecurityCheck")).Enabled = true;
                ((CheckBox)grdULDDetails.Rows[0].FindControl("CustomCheck")).Enabled = true;
                ((TextBox)grdULDDetails.Rows[0].FindControl("CustomStatusCode")).Enabled = true;
                ((TextBox)grdULDDetails.Rows[0].FindControl("Remark")).Enabled = true;
                ((TextBox)grdULDDetails.Rows[0].FindControl("GrdULDLocation")).Enabled = true;
                ((CheckBox)grdULDDetails.Rows[0].FindControl("BUP")).Enabled = true;

                //Dispose unused objects.
                myDataColumn.Dispose();
                myDataTable.Dispose();

            }
            catch (Exception)
            {
            }
        }
        #endregion LoadULDGrid

        /// <summary>
        /// Load initial grid data
        /// </summary>
        #region Load Arrival Grid
        public void LoadGridArrival()
        {
            try
            {

                DataTable myDataTable = new DataTable("Arr_LdArr_dt");
                DataColumn myDataColumn;

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ULDno";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "POL";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltNo";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ULDdest";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AWBno";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Owner";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "StatedPCS";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "StatedWgt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ExpectedPcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ExpectedWeight";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "PCS";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Org";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Dest";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Discrepancy";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Desc";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.Boolean");
                myDataColumn.ColumnName = "SecurityCheck";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.Boolean");
                myDataColumn.ColumnName = "CustomCheck";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "CustomStatusCode";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RemainingPcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "status";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "statusreassign";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrivedPieces";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrivedWeight";
                myDataTable.Columns.Add(myDataColumn);
                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "GrossWgt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FltDate";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "BookedPcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "BookedWt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "SCC";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "DESC";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Location";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ArrivalRemarks";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ManualAWB";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RcvPcs";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RcvWt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "HAWBCt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "RCVHAWBCt";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Shipper";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Consignee";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.Boolean");
                myDataColumn.ColumnName = "IsManualArrival";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.Boolean");
                myDataColumn.ColumnName = "IsUSCustom";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.Boolean");
                myDataColumn.ColumnName = "AutoCustom";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.Boolean");
                myDataColumn.ColumnName = "AutoACAS";
                myDataTable.Columns.Add(myDataColumn);

                DataRow dr;
                dr = myDataTable.NewRow();
                dr["ULDno"] = "";
                dr["POL"] = "";//"5";
                dr["FltNo"] = "";// "5";
                dr["ULDdest"] = "";// "9";
                dr["AWBno"] = "";
                dr["Owner"] = "";
                dr["StatedPCS"] = "";
                dr["StatedWgt"] = "";
                dr["ExpectedPcs"] = "";
                dr["ExpectedWeight"] = "";
                dr["PCS"] = "";
                dr["Org"] = "";
                dr["Dest"] = "";
                dr["Discrepancy"] = "0";
                dr["Desc"] = "";
                dr["SecurityCheck"] = false;
                dr["CustomCheck"] = false;
                dr["CustomStatusCode"] = "";
                dr["RemainingPcs"] = "";
                dr["status"] = "";
                dr["statusreassign"] = "";
                dr["ArrivedPieces"] = "";
                dr["ArrivedWeight"] = "";
                dr["GrossWgt"] = "";
                dr["FltDate"] = "";
                dr["BookedPCS"] = "";
                dr["BookedWt"] = "";
                dr["SCC"] = "";
                dr["DESC"] = "";
                dr["Location"] = "";
                dr["ArrivalRemarks"] = "";
                dr["ManualAWB"] = "Y";
                dr["RcvPcs"] = "0";
                dr["RcvWt"] = "0";
                dr["HAWBCt"] = "0";
                dr["RCVHAWBCt"] = "0";
                dr["Shipper"] = "";
                dr["Consignee"] = "";
                dr["IsManualArrival"] = "true";
                dr["IsUSCustom"] = false;
                dr["AutoCustom"] = false;
                dr["AutoACAS"] = false;

                myDataTable.Rows.Add(dr);

                //Bind the DataTable to the Grid
                GVArrDet.DataSource = null;
                GVArrDet.DataSource = myDataTable;
                GVArrDet.DataBind();
                Session["dtCreditInfo"] = myDataTable.Copy();

                //Add values in Discrepancy Grid.
                if (GVArrDet.Rows.Count >= 1)
                {
                    DataSet ddlds = new DataSet("Arrival_Ldgrd_Disc");
                    ddlds = da.SelectRecords("Sp_GetDiscrepancy");

                    if (ddlds != null && ddlds.Tables.Count > 0)
                    {
                        DropDownList ddl = ((DropDownList)GVArrDet.Rows[0].FindControl("ddlDiscrepancy"));
                        if (ddl != null)
                        {
                            ddl.DataSource = ddlds.Tables[0];
                            ddl.DataTextField = ddlds.Tables[0].Columns[0].ColumnName;
                            ddl.DataValueField = ddlds.Tables[0].Columns[0].ColumnName;
                            ddl.DataBind();
                            ddl.Dispose();
                        }
                        ddlds.Dispose();
                    }
                }

                ((TextBox)GVArrDet.Rows[0].FindControl("Expectedpcs")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("ExpectedWeight")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("Owner")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("MftPcs")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("MftWt")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("txtArrivedWt")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("txtArrivedPcs")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("StdPcs")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("StdWt")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("BkdPcs")).Enabled = false;
                ((TextBox)GVArrDet.Rows[0].FindControl("BkdWt")).Enabled = false;

                //Enable fields for all rows except newly added last row.
                ((TextBox)GVArrDet.Rows[0].FindControl("POL")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("ULDDestn")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("AWB")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("Origin")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("Destn")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("CustomStatusCode")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("Remark")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("FlightNo")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("FltDate")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("Shipper")).Enabled = true;
                ((TextBox)GVArrDet.Rows[0].FindControl("Consignee")).Enabled = true;

                myDataColumn.Dispose();
                myDataTable.Dispose();

            }
            catch (Exception)
            {
            }
        }
        #endregion Load Arrival Grid

        /// <summary>
        /// This function belongs to add new row to grid 
        /// </summary>
        //30-9-2012
        #region SaveGridData
        public void SaveRouteDetails()
        {
            //DataTable dtCreditInfo = null;
            DataTable dtCreditInfo = new DataTable("Arrival_GridData_dtSave2");

            try
            {
                dtCreditInfo = ((DataTable)Session["dtCreditInfo"]).Clone();
                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {
                    DataRow row = dtCreditInfo.NewRow();
                    row["ULDno"] = ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Text;
                    row["POL"] = ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text;
                    row["FltNo"] = ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text;
                    row["FltDate"] = ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Text;
                    row["ULDdest"] = ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text;
                    row["AWBno"] = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text;
                    //row["Owner"] = ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text;
                    if (((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Text == "")
                        row["StatedPCS"] = "0";
                    else
                        row["StatedPCS"] = ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Text == "")
                        row["StatedWgt"] = "0";
                    else
                        row["StatedWgt"] = ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text == "")
                        row["ExpectedPcs"] = "0";
                    else
                        row["ExpectedPcs"] = ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text == "")
                        row["ExpectedWeight"] = "0";
                    else
                        row["ExpectedWeight"] = ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Text == "")
                        row["PCS"] = "0";
                    else
                        row["PCS"] = ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Text;

                    row["Org"] = ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Text;
                    row["Dest"] = ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Text;
                    row["Discrepancy"] = ((DropDownList)GVArrDet.Rows[i].FindControl("ddlDiscrepancy")).Text;
                    row["Desc"] = ((Label)GVArrDet.Rows[i].FindControl("CommDesc")).Text;
                    row["SecurityCheck"] = ((CheckBox)GVArrDet.Rows[i].FindControl("SecurityCheck")).Checked;
                    row["CustomCheck"] = ((CheckBox)GVArrDet.Rows[i].FindControl("CustomCheck")).Checked;
                    row["CustomStatusCode"] = ((TextBox)GVArrDet.Rows[i].FindControl("CustomStatusCode")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("Remainingpcs")).Text != "")
                        row["RemainingPcs"] = ((TextBox)GVArrDet.Rows[i].FindControl("Remainingpcs")).Text;
                    else
                        row["RemainingPcs"] = "0";

                    row["statusreassign"] = ((TextBox)GVArrDet.Rows[i].FindControl("statusreassign")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Text == "")
                        row["ArrivedPieces"] = "0";
                    else
                        row["ArrivedPieces"] = ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Text == "")
                        row["ArrivedWeight"] = "0.00";
                    else
                        row["ArrivedWeight"] = ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Text == "")
                        row["GrossWgt"] = "0";
                    else
                        row["GrossWgt"] = ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("BkdPcs")).Text == "")
                        row["BookedPCS"] = "0";
                    else
                        row["BookedPCS"] = ((TextBox)GVArrDet.Rows[i].FindControl("BkdPcs")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("BkdWt")).Text == "")
                        row["BookedWt"] = "0";
                    else
                        row["BookedWt"] = ((TextBox)GVArrDet.Rows[i].FindControl("BkdWt")).Text;

                    row["Location"] = ((TextBox)GVArrDet.Rows[i].FindControl("txtLocation")).Text;
                    row["ArrivalRemarks"] = ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Text;
                    row["SCC"] = ((TextBox)GVArrDet.Rows[i].FindControl("CommCode")).Text;
                    row["ManualAWB"] = ((HiddenField)GVArrDet.Rows[i].FindControl("hdnManualAWB")).Value;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text == "")
                        row["RcvPcs"] = "0";
                    else
                        row["RcvPcs"] = ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Text;

                    if (((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Text == "")
                        row["RcvWt"] = "0";
                    else
                        row["RcvWt"] = ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Text;
                    // HAWB Count
                    if (((TextBox)GVArrDet.Rows[i].FindControl("HAWBCt")).Text == "")
                        row["HAWBCt"] = "0";
                    else
                        row["HAWBCt"] = ((TextBox)GVArrDet.Rows[i].FindControl("HAWBCt")).Text;

                    // RECEIVED HAWB Count
                    if (((TextBox)GVArrDet.Rows[i].FindControl("RCVHAWBCt")).Text == "")
                        row["RCVHAWBCt"] = "0";
                    else
                        row["RCVHAWBCt"] = ((TextBox)GVArrDet.Rows[i].FindControl("RCVHAWBCt")).Text;
                    row["Shipper"] = ((TextBox)GVArrDet.Rows[i].FindControl("Shipper")).Text;
                    row["Consignee"] = ((TextBox)GVArrDet.Rows[i].FindControl("Consignee")).Text;

                    dtCreditInfo.Rows.Add(row);

                }

                Session["dtCreditInfo"] = dtCreditInfo.Copy();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error [SaveRouteDetails]: " + ex.Message;
            }
            finally
            {
                if (dtCreditInfo != null)
                {
                    dtCreditInfo.Dispose();
                }
            }
        }
        #endregion SaveGridData

        #region SaveULDGridData
        public void SaveULDGridDetails()
        {
            //DataTable dtCreditInfo = null;
            DataTable dtCreditInfo = new DataTable("Arrival_SaveULD_dtCredit");
            try
            {
                dtCreditInfo = ((DataTable)Session["dsULDData"]).Clone();
                for (int i = 0; i < grdULDDetails.Rows.Count; i++)
                {
                    DataRow row = dtCreditInfo.NewRow();
                    row["ULDNo"] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Text;
                    row["FltNo"] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightNo")).Text;
                    row["FltDate"] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightDate")).Text;
                    row["Org"] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDOrg")).Text;
                    row["Dest"] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDDestn")).Text;
                    row["POL"] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDPOL")).Text;
                    row["ULDWt"] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDULDWt")).Text;

                    if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDScaleWt")).Text == null ||
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDScaleWt")).Text == "")
                        row["ScaleWt"] = "0.00";
                    else
                        row["ScaleWt"] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDScaleWt")).Text;

                    row["AWBCount"] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBCount")).Text;
                    row["AWBPcs"] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBPcs")).Text;
                    row["AWBWt"] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBWt")).Text;
                    row["ArrivalSecurityCheck"] = ((CheckBox)grdULDDetails.Rows[i].FindControl("SecurityCheck")).Checked.ToString();
                    row["ArrivalCustomCheck"] = ((CheckBox)grdULDDetails.Rows[i].FindControl("CustomCheck")).Checked.ToString();
                    row["ArrivalCustomStatusCode"] = ((TextBox)grdULDDetails.Rows[i].FindControl("CustomStatusCode")).Text;
                    row["ArrivalRemark"] = ((TextBox)grdULDDetails.Rows[i].FindControl("Remark")).Text;
                    row["IsArrived"] = ((CheckBox)grdULDDetails.Rows[i].FindControl("isarrived")).Checked.ToString();
                    row["Location"] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDLocation")).Text;
                    row["IsReceived"] = ((HiddenField)grdULDDetails.Rows[i].FindControl("IsReceived")).Value;

                    dtCreditInfo.Rows.Add(row);
                }

                Session["dsULDData"] = dtCreditInfo.Copy();
            }
            catch (Exception)
            { }
            finally
            {
                if (dtCreditInfo != null)
                {
                    dtCreditInfo.Dispose();
                }
            }
        }
        #endregion SaveGridData

        /// <summary>
        /// Add new to grid dynamically
        /// </summary>
        /// 
        #region AddnewRow To Grid
        private void AddNewRowToGrid()
        {
            //DataTable dtCreditInfo = null;
            DataTable dtCreditInfo = new DataTable("Arrival_NewRow_Dt2");
            //DataSet ddlds = null;
            DataSet ddlds = new DataSet("Arrival_NewRow_Ds2");

            try
            {
                dtCreditInfo = (DataTable)Session["dtCreditInfo"];
                DataRow rw = dtCreditInfo.NewRow();

                dtCreditInfo.Rows.Add(rw);

                GVArrDet.DataSource = dtCreditInfo.Copy();
                GVArrDet.DataBind();

                //ddlds = new DataSet();
                ddlds = da.SelectRecords("Sp_GetDiscrepancy");
                DropDownList ddl;
                for (int i = 0; i < dtCreditInfo.Rows.Count; i++)
                {
                    DataRow row = dtCreditInfo.Rows[i];
                    if (GVArrDet.Rows.Count > 1)
                    {
                        ddl = ((DropDownList)GVArrDet.Rows[i].FindControl("ddlDiscrepancy"));
                        ddl.DataSource = ddlds.Tables[0];
                        ddl.DataTextField = ddlds.Tables[0].Columns[0].ColumnName;
                        ddl.DataValueField = ddlds.Tables[0].Columns[0].ColumnName;
                        ddl.DataBind();
                        ddl.Dispose();
                    }
                    ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Text = row["ULDno"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Text = row["POL"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text = row["FltNo"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Text = row["FltDate"].ToString();

                    if (row["ULDdest"] != null && row["ULDdest"].ToString() != "")
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text = row["ULDdest"].ToString();
                    else
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text = Session["Station"].ToString();

                    ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text = row["AWBno"].ToString();
                    //((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Text = row["Owner"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Text = row["StatedPCS"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Text = row["StatedWgt"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text = row["ExpectedPcs"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text = row["ExpectedWeight"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Text = row["PCS"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Text = row["Org"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Text = row["Dest"].ToString();
                    ((DropDownList)GVArrDet.Rows[i].FindControl("ddlDiscrepancy")).Text = row["Discrepancy"].ToString();
                    ((Label)GVArrDet.Rows[i].FindControl("CommDesc")).Text = row["Desc"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("CustomStatusCode")).Text = row["CustomStatusCode"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Remainingpcs")).Text = row["RemainingPcs"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("statusreassign")).Text = row["statusreassign"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Text = row["ArrivedPieces"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Text = row["ArrivedWeight"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Text = row["GrossWgt"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Text = row["ArrivalRemarks"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("txtLocation")).Text = row["Location"].ToString();
                    ((HiddenField)GVArrDet.Rows[i].FindControl("hdnManualAWB")).Value = row["ManualAWB"].ToString();
                    if (row["HAWBCt"].ToString() == "")
                    {
                        ((TextBox)GVArrDet.Rows[i].FindControl("HAWBCt")).Text = "0";
                    }
                    else
                    {
                        ((TextBox)GVArrDet.Rows[i].FindControl("HAWBCt")).Text = row["HAWBCt"].ToString();
                    }
                    if (row["RCVHAWBCt"].ToString() == "")
                    {
                        ((TextBox)GVArrDet.Rows[i].FindControl("RCVHAWBCt")).Text = "0";
                    }
                    else
                    {
                        ((TextBox)GVArrDet.Rows[i].FindControl("RCVHAWBCt")).Text = row["RCVHAWBCt"].ToString();
                    }

                    if (row["SecurityCheck"] != null && row["SecurityCheck"].ToString() != "")
                    {
                        ((CheckBox)GVArrDet.Rows[i].FindControl("SecurityCheck")).Checked =
                            Convert.ToBoolean(row["SecurityCheck"]);
                    }
                    if (row["CustomCheck"] != null && row["CustomCheck"].ToString() != "")
                    {
                        ((CheckBox)GVArrDet.Rows[i].FindControl("CustomCheck")).Checked =
                            Convert.ToBoolean(row["CustomCheck"]);
                    }
                    ((TextBox)GVArrDet.Rows[i].FindControl("Shipper")).Text = row["Shipper"].ToString();
                    ((TextBox)GVArrDet.Rows[i].FindControl("Consignee")).Text = row["Consignee"].ToString();

                    ((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("ULD")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("Owner")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedWt")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("StdPcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("StdWt")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("BkdPcs")).Enabled = false;
                    ((TextBox)GVArrDet.Rows[i].FindControl("BkdWt")).Enabled = false;

                    //Disable fields for all rows except newly added last row.
                    if (((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text != "")
                    {
                        ((TextBox)GVArrDet.Rows[i].FindControl("POL")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("ULDDestn")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("CustomStatusCode")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Remark")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Shipper")).Enabled = false;
                        ((TextBox)GVArrDet.Rows[i].FindControl("Consignee")).Enabled = false;

                        //Disable received pcs & weight text boxes if all pieces are already arrived.
                        if (((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text == "0")
                        {
                            ((TextBox)GVArrDet.Rows[i].FindControl("RcvPcs")).Enabled = false;
                            ((TextBox)GVArrDet.Rows[i].FindControl("RcvWt")).Enabled = false;
                        }
                        ((DropDownList)GVArrDet.Rows[i].FindControl("ddlDiscrepancy")).Enabled = false;

                    }
                    else
                    {
                        ((HiddenField)GVArrDet.Rows[i].FindControl("hdnManualAWB")).Value = "Y";
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
                if (ddlds != null)
                    ddlds.Dispose();

                if (dtCreditInfo != null)
                    dtCreditInfo.Dispose();
            }
        }

        private void AddNewRowToULDGrid()
        {
            //DataTable dtCreditInfo = null;
            DataTable dtCreditInfo = new DataTable("Arrival_NRowuld_dtCre");
            try
            {
                dtCreditInfo = (DataTable)Session["dsULDData"];
                DataRow rw = dtCreditInfo.NewRow();
                dtCreditInfo.Rows.Add(rw);
                grdULDDetails.DataSource = dtCreditInfo.Copy();
                grdULDDetails.DataBind();

                for (int i = 0; i < grdULDDetails.Rows.Count; i++)
                {
                    DataRow row = dtCreditInfo.Rows[i];

                    if (dtCreditInfo.Rows[i]["isarrived"].ToString() == "True")
                    {
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("isarrived")).Checked = true;
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("checkULD")).Enabled = false;
                    }
                    if (dtCreditInfo.Rows[i]["ArrivalCustomCheck"].ToString() == "True")
                    {
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("CustomCheck")).Checked = true;
                    }
                    if (dtCreditInfo.Rows[i]["ArrivalSecurityCheck"].ToString() == "True")
                    {
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("SecurityCheck")).Checked = true;
                    }
                    ((CheckBox)grdULDDetails.Rows[i].FindControl("isarrived")).Enabled = false;

                    //Set back color.
                    if (((HiddenField)grdULDDetails.Rows[i].FindControl("IsReceived")).Value == "Y")
                    {
                        grdULDDetails.Rows[i].BackColor = CommonUtility.ColorHighlightedGrid;
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("BUP")).Checked = true;
                    }
                    else
                    {
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("BUP")).Checked = false;
                    }
                    ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Text = row["ULDNo"].ToString();
                    ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightNo")).Text = row["FltNo"].ToString();
                    ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightDate")).Text = row["FltDate"].ToString();
                    ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDOrg")).Text = row["Org"].ToString();
                    ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDDestn")).Text = row["Dest"].ToString();
                    ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDPOL")).Text = row["POL"].ToString();
                    ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDULDWt")).Text = row["ULDWt"].ToString();
                    ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDScaleWt")).Text = row["ScaleWt"].ToString();
                    ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBCount")).Text = row["AWBCount"].ToString();
                    ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBPcs")).Text = row["AWBPcs"].ToString();
                    ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBWt")).Text = row["AWBWt"].ToString();
                    if (row["ArrivalSecurityCheck"] != null && row["ArrivalSecurityCheck"].ToString() != "")
                    {
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("SecurityCheck")).Checked = Convert.ToBoolean(row["ArrivalSecurityCheck"]);
                    }
                    if (row["ArrivalCustomCheck"] != null && row["ArrivalCustomCheck"].ToString() != "")
                    {
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("CustomCheck")).Checked = Convert.ToBoolean(row["ArrivalCustomCheck"]);
                    }
                    ((TextBox)grdULDDetails.Rows[i].FindControl("CustomStatusCode")).Text = row["ArrivalCustomStatusCode"].ToString();
                    ((TextBox)grdULDDetails.Rows[i].FindControl("Remark")).Text = row["ArrivalRemark"].ToString();
                    ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDLocation")).Text = row["Location"].ToString();

                    //Enable controls for newly added rows.
                    if (((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Text == "")
                    {
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightNo")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightDate")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDOrg")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDPOL")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDDestn")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDULDWt")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDScaleWt")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBCount")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBPcs")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBWt")).Enabled = true;
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("SecurityCheck")).Enabled = true;
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("CustomCheck")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("CustomStatusCode")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("Remark")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDLocation")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDOrg")).Enabled = true;
                        ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDPOL")).Enabled = false;
                        ((ImageButton)grdULDDetails.Rows[i].FindControl("btnULDChlidPopup")).Visible = true;
                        ((CheckBox)grdULDDetails.Rows[i].FindControl("BUP")).Enabled = true;
                    }
                }

                dtCreditInfo.Dispose();

            }
            catch (Exception)
            {
            }
        }
        #endregion AddnewRow To Grid

        /// <summary>
        /// Close Flight Functanility make one flag in a exportmanifest table as IsClosed and check accordigly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Closeflt
        protected void btnCloseFlt_Click(object sender, EventArgs e)
        {
            string[] paramname = null;
            object[] parvalue = null;
            SqlDbType[] paramtype = null;
            try
            {

                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {
                    paramname = new string[3];
                    paramname[0] = "IsClosed";
                    paramname[1] = "AWBno";
                    paramname[2] = "Flag";

                    parvalue = new object[3];
                    parvalue[0] = "Closed";
                    parvalue[1] = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text;
                    parvalue[2] = "1";

                    paramtype = new SqlDbType[3];
                    paramtype[0] = SqlDbType.VarChar;
                    paramtype[1] = SqlDbType.VarChar;
                    paramtype[2] = SqlDbType.VarChar;

                    bool ds = da.UpdateData("Sp_UpdateIsclosedstatus", paramname, paramtype, parvalue);
                    if (ds == true)
                    {
                        for (int j = 1; j < GVArrDet.Columns.Count; j++)
                        {
                            GVArrDet.Rows[i].Cells[j].Enabled = false;
                        }
                    }
                }
                lblStatus.Text = "Flight is closed";
                lblStatus.ForeColor = Color.Red;
            }
            catch (Exception)
            { }
            finally
            {
                paramname = null;
                parvalue = null;
                paramtype = null;
            }

            string[] parasmname = null;
            object[] parasvalue = null;
            SqlDbType[] paramstype = null;
            try
            {

                //Code to Generate Arrival Discrepency Alert to Origin and Destination Managers
                parasmname = new string[3];
                parasmname[0] = "FlightNo";
                parasmname[1] = "FlightDate";
                parasmname[2] = "Destination";

                parasvalue = new object[3];
                parasvalue[0] = txtFlightPrefix.Text + txtFlightNo.Text;
                parasvalue[1] = txtFlightDate.Text;
                parasvalue[2] = Session["Station"].ToString();

                paramstype = new SqlDbType[3];
                paramstype[0] = SqlDbType.VarChar;
                paramstype[1] = SqlDbType.VarChar;
                paramstype[2] = SqlDbType.VarChar;

                da.UpdateData("SP_ArrivalDiscrepancy", parasmname, paramstype, parasvalue);


                lblStatus.Text = "Flight is closed";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception)
            { }
            finally
            {
                parasmname = null;
                parasvalue = null;
                paramstype = null;
            }




        }
        #endregion Closeft

        /// <summary>
        /// Manifest Printing is done on this button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ManifestButton
        protected void btnManifest_Click(object sender, EventArgs e)
        {
            //Sumit 2014-10-30
            try
            {
                if (txtFlightDate.Text == "" && txtFlightNo.Text == "")
                {
                    lblStatus.Text = "Please Enter Flight Date and Flight Number";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                showManifestData();

                #region Manifest Print on the Same Page

                ReportViewer ReportViewer1 = new ReportViewer();
                ReportViewer1.Visible = true;



                ReportViewer1.Reset();

                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;
                rep1.ReportPath = Server.MapPath("/Reports/EXP_ULDArrival.rdlc");
                //rep1.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "EXP_ULDArrival.rdlc";
                rds1.Name = "dsArrival_dtManifest";
                rds1.Value = dtTable1;
                rep1.DataSources.Add(rds1);


                ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                //ItemsSubreportProcessingEventHandler(Object, e);
                #region "Print as PDF"

                string reportType = "PDF";

                string mimeType;

                string encoding;

                string fileNameExtension = "pdf";



                //The DeviceInfo settings should be changed based on the reportType

                //http://msdn2.microsoft.com/en-us/library/ms155397.aspx

                string deviceInfo = "<DeviceInfo><PageHeight>30cm</PageHeight><PageWidth>30cm</PageWidth></DeviceInfo>";

                //"<DeviceInfo>" +

                //"  <OutputFormat>PDF</OutputFormat>" +

                ////"  <PageWidth>8.5in</PageWidth>" +

                ////"  <PageHeight>11in</PageHeight>" +

                ////"  <MarginTop>0.01in</MarginTop>" +

                ////"  <MarginLeft>0.01in</MarginLeft>" +

                ////"  <MarginRight>0.01in</MarginRight>" +

                ////"  <MarginBottom>0.01in</MarginBottom>" +

                //"</DeviceInfo>";



                Warning[] warnings;

                string[] streams;

                byte[] renderedBytes;



                //Render the report

                renderedBytes = rep1.Render(

                    reportType,

                    deviceInfo,

                    out mimeType,

                    out encoding,

                    out fileNameExtension,

                    out streams,

                    out warnings);



                //Clear the response stream and write the bytes to the outputstream

                //Set content-disposition to "attachment" so that user is prompted to take an action

                //on the file (open or save)

                Response.Clear();

                Response.ContentType = mimeType;

                Response.AddHeader("content-disposition", "attachment; filename=" + "Arrival_Manifest" + "." + fileNameExtension);

                Response.BinaryWrite(renderedBytes);
                //Response.Flush();
                //Response.Close();

                #endregion

                #endregion


                #region Commented Previous Code
                //string query = "'ShowEMAWBULD1.aspx'";

                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open(" + query + ");", true);
                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "newwindow.focus();", true);
                #endregion
            }
            catch (Exception ex)
            { }
        }

        #endregion ManifestButton

        /// <summary>
        /// Paging is done for the grid GVArrival 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Paging
        protected void GVArrDet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVArrDet.PageIndex = e.NewPageIndex;
                GVArrDet.DataSource = (DataTable)Session["dsData"];
                GVArrDet.DataBind();
            }
            catch (Exception)
            { }
        }
        #endregion Paging

        /// <summary>
        /// Reassign opens pop up on the screen and saves data in expmanifstoffload table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Reassign
        protected void btnReassign_Click(object sender, EventArgs e)
        {
            //DataTable dtSurface = null;
            DataTable dtSurface = new DataTable("Arrival_btnRe_dtSur2");
            try
            {
                //dtSurface = new DataTable();
                dtSurface.Columns.Add("AWBNumber", typeof(string));
                dtSurface.Columns.Add("FlightNumber", typeof(string));
                dtSurface.Columns.Add("Pieces", typeof(string));
                dtSurface.Columns.Add("Weight", typeof(string));
                dtSurface.Columns.Add("Origin", typeof(string));
                dtSurface.Columns.Add("Destination", typeof(string));

                bool Isselected = false;
                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {
                    if (((CheckBox)GVArrDet.Rows[i].FindControl("check")).Checked == true)
                    {
                        Isselected = true;
                    }
                }
                if (Isselected)
                {
                    lblStatus.Text = "";
                    for (int i = 0; i < GVArrDet.Rows.Count; i++)
                    {
                        if (((CheckBox)GVArrDet.Rows[i].FindControl("check")).Checked == true)
                        {
                            string reassign = ((TextBox)GVArrDet.Rows[i].FindControl("Status")).Text;

                            //31-7-2012
                            if (reassign == "Reassigned")
                            {
                                lblStatus.Text = "Cannot Reassign as the AWB is already reassigned..";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }

                            DataRow drSurface = dtSurface.NewRow();
                            string AWBno = (((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text);
                            drSurface["AWBNumber"] = AWBno;
                            string[] paramName = new string[] { "AWBNumber", "Station" };
                            object[] paramValue = new object[] { AWBno, Session["Station"].ToString() };
                            SqlDbType[] paramType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };

                            DataSet ds = new DataSet("Arrival_btnRe_dsSur");
                            ds = da.SelectRecords("SpGetArrivedPiecesFromOps", paramName, paramValue, paramType);
                            if (ds != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    if (ds.Tables[0].Rows[0]["RCVPieces"].ToString() != "0" &&
                                        ds.Tables[0].Rows[0]["RCVPieces"].ToString() != "")
                                    {
                                        drSurface["Pieces"] = ds.Tables[0].Rows[0]["RCVPieces"].ToString();
                                        drSurface["Weight"] = ds.Tables[0].Rows[0]["RCVWeight"].ToString();
                                    }
                                    else
                                    {
                                        lblStatus.Text = "No Pieces are Arrived. Please update Arrived pieces";
                                        lblStatus.ForeColor = Color.Red;
                                        return;
                                    }
                                }
                                else
                                {
                                    lblStatus.Text = "No Pieces are Arrived. Please update Arrived pieces";
                                    lblStatus.ForeColor = Color.Red;
                                    return;
                                }
                                ds.Dispose();
                            }
                            drSurface["FlightNumber"] = ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text;
                            //int expectedpcs = (Convert.ToInt32(((TextBox)GVArrDet.Rows[i].FindControl("Expectedpcs")).Text));
                            //int manifestedpcs = (Convert.ToInt32(((TextBox)GVArrDet.Rows[i].FindControl("MftPcs")).Text));
                            //int manifestedwt = (Convert.ToInt32(((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Text));
                            //int expectedwt = (Convert.ToInt32(((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text));
                            //double manifestedwt = (Convert.ToDouble(((TextBox)GVArrDet.Rows[i].FindControl("MftWt")).Text));
                            //double expectedwt = (Convert.ToDouble(((TextBox)GVArrDet.Rows[i].FindControl("ExpectedWeight")).Text));

                            drSurface["Origin"] = ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Text;
                            drSurface["Destination"] = ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Text;
                            dtSurface.Rows.Add(drSurface);

                        }
                    }
                    Session["Arrival_Reassign"] = dtSurface;
                    //ClientScript.RegisterStartupScript(this.GetType(), "", "callexport();", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callexport();</SCRIPT>", false);

                }
                else
                {
                    lblStatus.Text = "Please Select atleast one row from grid to Reassign or it is already reassigned.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dtSurface != null)
                {
                    dtSurface.Dispose();
                }
            }
        }
        #endregion Reassign

        /// <summary>
        /// Reopen is done same as a close flight functnality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Reopen
        protected void btnReopen_Click(object sender, EventArgs e)
        {
            string[] paramname = null;
            object[] parvalue = null;
            SqlDbType[] paramtype = null;
            try
            {
                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {
                    paramname = new string[3];
                    paramname[0] = "IsClosed";
                    paramname[1] = "AWBno";
                    paramname[2] = "Flag";


                    parvalue = new object[3];
                    parvalue[0] = "Opened";
                    parvalue[1] = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text;
                    parvalue[2] = "0";

                    paramtype = new SqlDbType[3];
                    paramtype[0] = SqlDbType.VarChar;
                    paramtype[1] = SqlDbType.VarChar;
                    paramtype[2] = SqlDbType.VarChar;

                    bool ds = da.UpdateData("Sp_UpdateIsclosedstatus", paramname, paramtype, parvalue);
                    if (ds == true)
                    {
                        for (int j = 1; j < GVArrDet.Columns.Count; j++)
                        {
                            GVArrDet.Rows[i].Cells[j].Enabled = true;
                        }
                    }
                }

                lblStatus.Text = "Flight is Re-Opened";
                lblStatus.ForeColor = Color.Green;

            }
            catch (Exception)
            { }
            finally
            {
                paramname = null;
                paramtype = null;
                parvalue = null;
            }
        }
        #endregion Reopen

        /// <summary>
        /// Edit Functnality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Edit

        protected void GVArrDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //DataTable dsDataRow = null;
            DataTable dsDataRow = new DataTable("Arr_GVArrDet_dt");
            //DataSet res = null;
            DataSet res = new DataSet("Arr_GVArrDet_dsRes");
            try
            {

                awbrowno = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "Edit")
                {

                    int rowid = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = GVArrDet.Rows[rowid];
                    dsDataRow = (DataTable)Session["dsData"];
                    AWBNo = dsDataRow.Rows[rowid][4].ToString();
                    string FltNo = txtFlightPrefix.Text + txtFlightNo.Text;

                    editgrid = AWBNo;
                    Session["editgrid"] = AWBNo;

                    string fltdate1 = DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", null).ToString("dd/MM/yyyy");
                    res = BlArl.DeliveredAWB(AWBNo, FltNo, fltdate1);
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        lblStatus.Text = "Sorry AWB is Delivered";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        ((TextBox)GVArrDet.Rows[rowid].FindControl("RcvPcs")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[rowid].FindControl("RcvWt")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[rowid].FindControl("txtArrivedPcs")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[rowid].FindControl("txtArrivedWt")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[rowid].FindControl("Expectedpcs")).Enabled = true;
                        ((TextBox)GVArrDet.Rows[rowid].FindControl("ExpectedWeight")).Enabled = true;
                        ((DropDownList)GVArrDet.Rows[rowid].FindControl("ddlDiscrepancy")).Enabled = true;
                    }
                }
                //SHOW HAWB POPUP
                if (e.CommandName == "HAWB")
                {

                    MAWBNo = ((TextBox)GVArrDet.Rows[awbrowno].FindControl("AWB")).Text;
                    lblMAWB.Text = MAWBNo;

                    lblMAWBTotPcs.Text = ((TextBox)GVArrDet.Rows[awbrowno].FindControl("MftPcs")).Text.Trim();
                    lblMAWBTotWt.Text = ((TextBox)GVArrDet.Rows[awbrowno].FindControl("MftWt")).Text.Trim();

                    int MAWBTotPcs;
                    decimal MAWBTotWt;

                    if (!int.TryParse(lblMAWBTotPcs.Text, out MAWBTotPcs))
                    {
                        lblStatus.Text = "Please enter Rcv Pieces";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
                        return;
                    }

                    if (!decimal.TryParse(lblMAWBTotWt.Text, out MAWBTotWt))
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        lblStatus.Text = "Please enter Rcv Wt";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
                        return;
                    }
                    if (((TextBox)GVArrDet.Rows[awbrowno].FindControl("Origin")).Text == "")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        lblStatus.Text = "Origin can not be Blank";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
                        return;
                    }
                    lblHAWBOrigin.Text = ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Origin")).Text;
                    if (((TextBox)GVArrDet.Rows[awbrowno].FindControl("Destn")).Text == "")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        lblStatus.Text = "Destination can not be Blank";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
                        return;
                    }
                    if (((TextBox)GVArrDet.Rows[awbrowno].FindControl("POL")).Text == "")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        lblStatus.Text = "POL can not be Blank";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
                        return;
                    }
                    lblHAWBDest.Text = ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Destn")).Text;
                    lblMAWBTotPcs.Text = MAWBTotPcs.ToString();
                    lblMAWBTotWt.Text = MAWBTotWt.ToString();

                    Session["MAWBNo"] = MAWBNo;
                    string awbNumber = "";

                    if (MAWBNo.Length > 8)
                    {
                        awbNumber = MAWBNo.Substring(MAWBNo.Length - 8);
                    }
                    else
                    {
                        lblStatus.Text = "Please enter valid AWB Number";
                        return;
                    }
                    string awbPrefix = "";
                    if (MAWBNo.Contains("-"))
                    {
                        awbPrefix = MAWBNo.Substring(0, MAWBNo.IndexOf('-'));
                    }
                    else
                    {
                        lblStatus.Text = "Please enter valid AWB Prefix";
                        return;
                    }

                    BAL.BALHAWBDetails HAWB = new BALHAWBDetails();
                    DataSet ds = new DataSet("Arr_GVRwcmd_ds");

                    if (Session["ARR_HAWBDetails" + MAWBNo] == null)
                        ds = HAWB.GetHAWBDetailsArrival(awbNumber, awbPrefix, txtFlightPrefix.Text + txtFlightNo.Text,
                            DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", null).ToString("MM/dd/yyyy"),
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("POL")).Text,
                            ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Destn")).Text
                            );
                    else
                    {
                        //ds = new DataSet();
                        ds.Tables.Add(((DataTable)Session["ARR_HAWBDetails" + MAWBNo]).Copy());
                    }

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count >= 0)
                    {
                        Session["ARR_HAWBDetails" + MAWBNo] = ds.Tables[0];
                        Refresh_gvH();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
                    }
                    else
                    {
                        Refresh_gvH();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
                    }
                    //Disable "ADD HAWB" button if AWB is not Manual AWB.
                    if (((HiddenField)GVArrDet.Rows[awbrowno].FindControl("hdnManualAWB")).Value == "N")
                    {
                        ((Button)gvHAWBDetails.FooterRow.FindControl("btnADDHAWB")).Enabled = false;
                        //btnSaveHAWB.Enabled = false;
                        btnDeleteAWB.Enabled = false;
                    }
                    else
                    {
                        ((Button)gvHAWBDetails.FooterRow.FindControl("btnADDHAWB")).Enabled = true;
                        btnSaveHAWB.Enabled = true;
                        btnDeleteAWB.Enabled = true;
                    }
                }

                if (e.CommandName == "SHIPPER")
                {
                    ShowShipperConsigneePopup(awbrowno, true);
                }
                if (e.CommandName == "CONSIGNEE")
                {
                    ShowShipperConsigneePopup(awbrowno, false);
                }
            }
            catch (Exception)
            { }
            finally
            {
                if (res != null)
                {
                    res.Dispose();
                }
                if (dsDataRow != null)
                {
                    dsDataRow.Dispose();
                }
            }
        }

        #region AWB Modify
        protected void LnkModify_Click(object sender, EventArgs e)
        {
            DataSet res = new DataSet("Arr_LnkModify_res");
            try
            {
                int rowid = -1;
                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {   //Find out selected row for editing.
                    if (((CheckBox)GVArrDet.Rows[i].FindControl("check")).Checked)
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

                GridViewRow row = GVArrDet.Rows[rowid];
                string AWBNo = ((TextBox)row.FindControl("AWB")).Text;
                string FltNo = txtFlightPrefix.Text + txtFlightNo.Text;

                editgrid = AWBNo;
                Session["editgrid"] = AWBNo;

                string fltdate1 = DateTime.ParseExact(txtFlightDate.Text, "dd/MM/yyyy", null).ToString("dd/MM/yyyy");
                res = BlArl.DeliveredAWB(AWBNo, FltNo, fltdate1);
                if (res.Tables[0].Rows.Count > 0)
                {
                    lblStatus.Text = "This AWB is Delivered";
                    lblStatus.ForeColor = Color.Red;
                    ////************ Commented by Vishal on 17 JUN 2014 for G8.
                    //return;
                }
                //else
                //{
                ((TextBox)GVArrDet.Rows[rowid].FindControl("RcvPcs")).Enabled = true;
                ((TextBox)GVArrDet.Rows[rowid].FindControl("RcvWt")).Enabled = true;
                ((TextBox)GVArrDet.Rows[rowid].FindControl("txtArrivedPcs")).Enabled = true;
                ((TextBox)GVArrDet.Rows[rowid].FindControl("txtArrivedWt")).Enabled = true;
                ((TextBox)GVArrDet.Rows[rowid].FindControl("Expectedpcs")).Enabled = true;
                ((TextBox)GVArrDet.Rows[rowid].FindControl("ExpectedWeight")).Enabled = true;
                ((DropDownList)GVArrDet.Rows[rowid].FindControl("ddlDiscrepancy")).Enabled = true;
                ((CheckBox)GVArrDet.Rows[rowid].FindControl("CustomCheck")).Enabled = true;
                ((TextBox)GVArrDet.Rows[rowid].FindControl("Remark")).Enabled = true;

                //Change Pcs And Wt values
                decimal arrivedpcs = Convert.ToDecimal(((TextBox)GVArrDet.Rows[rowid].FindControl("txtArrivedPcs")).Text);
                decimal arrivedwt = Convert.ToDecimal(((TextBox)GVArrDet.Rows[rowid].FindControl("txtArrivedWt")).Text);
                decimal ManfPcs = Convert.ToDecimal(((TextBox)GVArrDet.Rows[rowid].FindControl("MftPcs")).Text);
                decimal ManigWt = Convert.ToDecimal(((TextBox)GVArrDet.Rows[rowid].FindControl("MftWt")).Text);

                ((TextBox)GVArrDet.Rows[rowid].FindControl("txtArrivedPcs")).Text = "0";
                ((TextBox)GVArrDet.Rows[rowid].FindControl("txtArrivedWt")).Text = "0.00";

                ((TextBox)GVArrDet.Rows[rowid].FindControl("Expectedpcs")).Text = ManfPcs.ToString();
                ((TextBox)GVArrDet.Rows[rowid].FindControl("ExpectedWeight")).Text = ManigWt.ToString();

                ((TextBox)GVArrDet.Rows[rowid].FindControl("RcvPcs")).Text =
                    ((TextBox)GVArrDet.Rows[rowid].FindControl("Expectedpcs")).Text;

                ((TextBox)GVArrDet.Rows[rowid].FindControl("RcvWt")).Text =
                    ((TextBox)GVArrDet.Rows[rowid].FindControl("ExpectedWeight")).Text;

                ((TextBox)GVArrDet.Rows[rowid].FindControl("txtArrivedPcs")).Enabled = false;
                ((TextBox)GVArrDet.Rows[rowid].FindControl("txtArrivedWt")).Enabled = false;

                ((TextBox)GVArrDet.Rows[rowid].FindControl("Expectedpcs")).Enabled = false;
                ((TextBox)GVArrDet.Rows[rowid].FindControl("ExpectedWeight")).Enabled = false;
                ////if (arrivedpcs > 0 && arrivedpcs < ManfPcs)
                ////{
                ////((TextBox)GVArrDet.Rows[rowid].FindControl("Expectedpcs")).Text = (ManfPcs - arrivedpcs).ToString();
                ////((TextBox)GVArrDet.Rows[rowid].FindControl("ExpectedWeight")).Text = (ManigWt - arrivedwt).ToString();
                ////}
                ////else
                ////{
                ////((TextBox)GVArrDet.Rows[rowid].FindControl("Expectedpcs")).Text = ManfPcs.ToString();
                ////((TextBox)GVArrDet.Rows[rowid].FindControl("ExpectedWeight")).Text = ManigWt.ToString();
                ////}
                //}
            }
            catch (Exception)
            {

            }
        }
        #endregion AWB Modify

        #endregion Edit

        #region Show Shipper Consignee Popup
        private void ShowShipperConsigneePopup(int rowIndex, bool showShipper)
        {
            try
            {

                string awbNumber = ((TextBox)GVArrDet.Rows[awbrowno].FindControl("AWB")).Text;
                if (awbNumber == "")
                {
                    lblStatus.Text = "Please enter AWB Number to edit Shipper/ Consignee details.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {

                    lblShipperAWB.Text = awbNumber;
                    lblConsigneeAWB.Text = awbNumber;
                    if (showShipper)
                    {   //If popup for shipper is to be shown
                        if (Session["Arrival_Shipper_" + lblShipperAWB.Text] == null)
                        {   //If value not present in session then fetch from database.
                            string shipperCode = ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Shipper")).Text;

                            //DataSet ds;
                            DataSet ds = new DataSet("Arr_ShwPopup_ds1");
                            ds = BlArl.GetShipperConsigneeByAWB(awbNumber, Convert.ToDateTime(Session["IT"].ToString()));

                            if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                            {
                                ShipperConsignee objShipper = new ShipperConsignee();
                                objShipper.AccountCode = ds.Tables[1].Rows[0]["ShipperAccCode"].ToString();
                                objShipper.Address1 = ds.Tables[1].Rows[0]["ShipperAddress"].ToString();
                                objShipper.Address2 = ds.Tables[1].Rows[0]["ShipperAdd2"].ToString();
                                objShipper.City = ds.Tables[1].Rows[0]["ShipperCity"].ToString();
                                objShipper.Country = ds.Tables[1].Rows[0]["ShipperCountry"].ToString();
                                objShipper.Email = ds.Tables[1].Rows[0]["ShipperEmailId"].ToString();
                                objShipper.KnownShipper = ds.Tables[1].Rows[0]["ShipperAccCode"].ToString();
                                objShipper.Name = ds.Tables[1].Rows[0]["ShipperName"].ToString();
                                objShipper.Pincode = ds.Tables[1].Rows[0]["ShipperPincode"].ToString();
                                objShipper.State = ds.Tables[1].Rows[0]["ShipperState"].ToString();
                                objShipper.Telephone = ds.Tables[1].Rows[0]["ShipperTelephone"].ToString();
                                //Set values in session.
                                Session["Arrival_Shipper_" + lblShipperAWB.Text] = objShipper;
                            }
                            if (ds != null)
                            {
                                ds.Dispose();
                            }
                        }
                        if (Session["Arrival_Shipper_" + lblShipperAWB.Text] != null)
                        {   //If shipper details already available in session.
                            txtShipperAccountCode.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).AccountCode;
                            TXTShipper.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Name;
                            TXTShipAddress.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Address1;
                            TXTShipperAdd2.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Address2;
                            TXTShipperCity.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).City;
                            TXTShipperEmail.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Email;
                            TXTShipperState.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).State;
                            TXTShipPinCode.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Pincode;
                            TXTShipTelephone.Text = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Telephone;
                            ddlShipCountry.SelectedValue = ((ShipperConsignee)Session["Arrival_Shipper_" + lblShipperAWB.Text]).Country;
                        }
                        else
                        {
                            ClearShipperConsigneeValues();
                        }
                        //Disable shipper consignee fields for non manual AWB.
                        if (((HiddenField)GVArrDet.Rows[awbrowno].FindControl("hdnManualAWB")).Value == "Y")
                            SetShipperConsigneeControlState(true);
                        else
                            SetShipperConsigneeControlState(false);

                        //Show shipper details popup.
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowShipper",
                            "<SCRIPT LANGUAGE='javascript'>ViewPanel_shipperPopUp();</script>", false);
                        txtShipperAccountCode.Focus();
                    }
                    else
                    {
                        //If popup for consignee is to be shown
                        if (Session["Arrival_Consignee_" + lblConsigneeAWB.Text] == null)
                        {   //If value not present in session then fetch from database.
                            string consigneeCode = ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Consignee")).Text;

                            //DataSet ds;
                            DataSet ds = new DataSet("Arr_ShwPopup_dsAwb");
                            ds = BlArl.GetShipperConsigneeByAWB(awbNumber, Convert.ToDateTime(Session["IT"].ToString()));

                            if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                            {
                                ShipperConsignee objConsignee = new ShipperConsignee();
                                objConsignee.AccountCode = ds.Tables[1].Rows[0]["ConsigAccCode"].ToString();
                                objConsignee.Address1 = ds.Tables[1].Rows[0]["ConsigneeAddress"].ToString();
                                objConsignee.Address2 = ds.Tables[1].Rows[0]["ConsigneeAddress2"].ToString();
                                objConsignee.City = ds.Tables[1].Rows[0]["ConsigneeCity"].ToString();
                                objConsignee.Country = ds.Tables[1].Rows[0]["ConsigneeCountry"].ToString();
                                objConsignee.Email = ds.Tables[1].Rows[0]["ConsigEmailId"].ToString();
                                objConsignee.KnownShipper = ds.Tables[1].Rows[0]["ConsigAccCode"].ToString();
                                objConsignee.Name = ds.Tables[1].Rows[0]["ConsigneeName"].ToString();
                                objConsignee.Pincode = ds.Tables[1].Rows[0]["ConsigneePincode"].ToString();
                                objConsignee.State = ds.Tables[1].Rows[0]["ConsigneeState"].ToString();
                                objConsignee.Telephone = ds.Tables[1].Rows[0]["ConsigneeTelephone"].ToString();
                                //Set values in session.
                                Session["Arrival_Consignee_" + lblConsigneeAWB.Text] = objConsignee;
                            }
                            if (ds != null)
                            {
                                ds.Dispose();
                            }
                        }
                        if (Session["Arrival_Consignee_" + lblConsigneeAWB.Text] != null)
                        {   //If shipper details already available in session.
                            txtConsigneeAccountCode.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).AccountCode;
                            TXTConsignee.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Name;
                            TXTConAddress.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Address1;
                            TXTConsigAdd2.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Address2;
                            TXTConsigCity.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).City;
                            TXTConsigEmail.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Email;
                            TXTConsigState.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).State;
                            TXTConsigPinCode.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Pincode;
                            TXTConTelephone.Text = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Telephone;
                            ddlConCountry.SelectedValue = ((ShipperConsignee)Session["Arrival_Consignee_" + lblConsigneeAWB.Text]).Country;
                        }
                        else
                        {
                            ClearShipperConsigneeValues();
                        }

                        //Disable shipper consignee fields for non manual AWB.
                        if (((HiddenField)GVArrDet.Rows[awbrowno].FindControl("hdnManualAWB")).Value == "Y")
                            SetShipperConsigneeControlState(true);
                        else
                            SetShipperConsigneeControlState(false);

                        //Show consignee details popup.
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowConsignee",
                            "<SCRIPT LANGUAGE='javascript'>ViewPanel_ConsigneePopUp();</script>", false);
                        txtConsigneeAccountCode.Focus();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion Show Shipper Consignee Popup

        #region Set Shipper Consignee Control State
        private void SetShipperConsigneeControlState(bool IsEnable)
        {
            //Enabled/ disable Shipper controls.
            TXTShipper.Enabled = IsEnable;
            TXTShipAddress.Enabled = IsEnable;
            txtShipperAccountCode.Enabled = IsEnable;
            TXTShipperAdd2.Enabled = IsEnable;
            TXTShipperCity.Enabled = IsEnable;
            TXTShipperEmail.Enabled = IsEnable;
            TXTShipperState.Enabled = IsEnable;
            TXTShipPinCode.Enabled = IsEnable;
            TXTShipTelephone.Enabled = IsEnable;
            ddlShipCountry.Enabled = IsEnable;

            //Enable/ disable Consignee controls.
            TXTConAddress.Enabled = IsEnable;
            TXTConsigAdd2.Enabled = IsEnable;
            TXTConsigCity.Enabled = IsEnable;
            TXTConsigEmail.Enabled = IsEnable;
            TXTConsignee.Enabled = IsEnable;
            txtConsigneeAccountCode.Enabled = IsEnable;
            TXTConsigPinCode.Enabled = IsEnable;
            TXTConsigState.Enabled = IsEnable;
            TXTConTelephone.Enabled = IsEnable;
            ddlConCountry.Enabled = IsEnable;

        }
        #endregion Set Shipper Consignee Control State

        #region Button Add HAWB click
        protected void btnADDHAWB_Click(object sender, EventArgs e)
        {
            lblHAWBStatus.Text = "";
            lblStatus.Text = "";
            try
            {
                int SumPcs = 0;
                float SumWt = 0;

                // For tracking the Sum
                TextBox textHAWBPrefix = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtHAWBPrefix"));
                TextBox textHAWBNo = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtHAWBNo"));
                TextBox textHAWBPcs = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtHAWBPcs"));
                TextBox textHAWBWt = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtHAWBWt"));
                TextBox textDescription = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtDescription"));
                TextBox textCustID = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtCustID"));
                TextBox textCustName = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtCustName"));
                TextBox textCustAddress = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtCustAddress"));
                TextBox textCity = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtCity"));
                TextBox textZipcode = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtZipcode"));
                TextBox textOrigin = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtOrigin"));
                TextBox textDestination = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtDestination"));
                TextBox textSHC = (TextBox)(gvHAWBDetails.FooterRow.FindControl("txtSHC"));

                int HAWBPcs = 0;
                float HAWBWt = 0;
                if (!int.TryParse(textHAWBPcs.Text, out HAWBPcs))
                {
                    lblHAWBStatus.Text = "Please enter valid HAWB Pcs";
                    lblHAWBStatus.ForeColor = Color.Red;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                    return;
                }
                HAWBWt = 0;
                if (!float.TryParse(textHAWBWt.Text, out HAWBWt))
                {
                    lblHAWBStatus.Text = "Please enter valid HAWB Wt";
                    lblHAWBStatus.ForeColor = Color.Red;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                    return;
                }

                for (int i = 0; i < gvHAWBDetails.Rows.Count; i++)
                {
                    HAWBPcs = 0;
                    if (!int.TryParse(((Label)gvHAWBDetails.Rows[i].FindControl("lblHAWBPcs")).Text, out HAWBPcs))
                    {
                        lblHAWBStatus.Text = "Invalid Pcs in one of the HAWBs";
                        lblHAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                        return;
                    }
                    HAWBWt = 0;
                    if (!float.TryParse(((Label)gvHAWBDetails.Rows[i].FindControl("lblHAWBWt")).Text, out HAWBWt))
                    {
                        lblHAWBStatus.Text = "Invalid Wt in one of the HAWBs";
                        lblHAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                        return;
                    }
                    SumPcs = SumPcs + HAWBPcs;
                    SumWt = SumWt + HAWBWt;
                    //Validate if same HAWBPrefix & HAWBNumber exists in already added HAWBs.
                    if (((Label)gvHAWBDetails.Rows[i].FindControl("lblHAWBPrefix")).Text.ToUpper() +
                        ((Label)gvHAWBDetails.Rows[i].FindControl("lblHAWBNo")).Text.ToUpper() ==
                        textHAWBPrefix.Text.ToUpper() + textHAWBNo.Text.ToUpper())
                    {
                        lblHAWBStatus.Text = "HAWB Prefix & Number you entered already exists for MAWB.";
                        lblHAWBStatus.ForeColor = Color.Red;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                        return;
                    }
                }

                if (!int.TryParse(textHAWBPcs.Text, out HAWBPcs))
                {
                    HAWBPcs = 0;
                }
                if (!float.TryParse(textHAWBWt.Text, out HAWBWt))
                {
                    HAWBWt = 0;
                }
                if (SumPcs + HAWBPcs > int.Parse(lblMAWBTotPcs.Text))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidateHAWBPcs();</script>", false);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message1", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                    return;
                }
                if (SumWt + HAWBWt > float.Parse(lblMAWBTotWt.Text))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ValidateHAWBWt();</script>", false);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message2", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
                    return;
                }

                ((DataTable)Session["ARR_HAWBDetails" + lblMAWB.Text]).Rows.Add(lblMAWB.Text, textHAWBNo.Text,
                    textHAWBPcs.Text, textHAWBWt.Text, textDescription.Text, textCustID.Text, textCustName.Text,
                    textCustAddress.Text, textCity.Text, textZipcode.Text, textOrigin.Text, textDestination.Text,
                    textSHC.Text, textHAWBPrefix.Text,
                    ((CheckBox)gvHAWBDetails.FooterRow.FindControl("chkArrivalStatusFooter")).Checked, 'Y');
                Refresh_gvH();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = ex.Message;
            }
        }
        #endregion

        #region SaveHAWBButton click
        protected void btnSaveHAWB_Click(object sender, EventArgs e)
        {
            lblHAWBStatus.Text = "";
            lblStatus.Text = "";
            //DataTable dt = null;
            DataTable dt = new DataTable("Arr_BtHAWB_dtHAWB");

            try
            {
                dt = (DataTable)Session["ARR_HAWBDetails" + lblMAWB.Text];

                if (dt != null)
                    dt.Rows.Clear();
                else
                {
                    Refresh_gvH();
                    dt = (DataTable)Session["ARR_HAWBDetails" + lblMAWB.Text];
                    dt.Rows.Clear();
                }
                int ArrivedHAWBCt = 0;
                //BAL.BALHAWBDetails HAWB = new BALHAWBDetails();
                for (int i = 0; i < gvHAWBDetails.Rows.Count; i++)
                {
                    string HAWBPrefix = ((Label)gvHAWBDetails.Rows[i].FindControl("lblHAWBPrefix")).Text;
                    string HAWBNo = ((Label)gvHAWBDetails.Rows[i].FindControl("lblHAWBNo")).Text;
                    int HAWBPcs = int.Parse(((Label)gvHAWBDetails.Rows[i].FindControl("lblHAWBPcs")).Text);
                    decimal HAWBWt = decimal.Parse(((Label)gvHAWBDetails.Rows[i].FindControl("lblHAWBWt")).Text);
                    string Description = ((Label)gvHAWBDetails.Rows[i].FindControl("lblDescription")).Text;
                    string CustID = ((Label)gvHAWBDetails.Rows[i].FindControl("lblCustID")).Text;
                    string CustName = ((Label)gvHAWBDetails.Rows[i].FindControl("lblCustName")).Text;
                    string CustAddress = ((Label)gvHAWBDetails.Rows[i].FindControl("lblCustAddress")).Text;
                    string City = ((Label)gvHAWBDetails.Rows[i].FindControl("lblCity")).Text;
                    string Zipcode = ((Label)gvHAWBDetails.Rows[i].FindControl("lblZipcode")).Text;
                    string Origin = ((Label)gvHAWBDetails.Rows[i].FindControl("lblOrigin")).Text;
                    string Destination = ((Label)gvHAWBDetails.Rows[i].FindControl("lblDestination")).Text;
                    string SHC = ((Label)gvHAWBDetails.Rows[i].FindControl("lblSHC")).Text;
                    // Calculate Arrived HAWB COUNT.
                    if (((CheckBox)gvHAWBDetails.Rows[i].FindControl("chkArrivalStatusRow")).Checked)
                    {
                        ArrivedHAWBCt++;
                    }

                    if (HAWBNo != "DUMMY" && HAWBNo != "Dummy")
                        dt.Rows.Add(lblMAWB.Text, HAWBNo, HAWBPcs, HAWBWt, Description, CustID, CustName, CustAddress,
                            City, Zipcode, Origin, Destination, SHC, HAWBPrefix,
                            ((CheckBox)gvHAWBDetails.Rows[i].FindControl("chkArrivalStatusRow")).Checked,
                            ((HiddenField)gvHAWBDetails.Rows[i].FindControl("hdnManualHAWB")).Value);

                }

                Session["ARR_HAWBDetails" + lblMAWB.Text] = dt;
                //SET HAWB COUNT & ARRIVED HAWB COUNT IN AWBDETAILS GRID.
                foreach (GridViewRow gvr in GVArrDet.Rows)
                {   //Find out matching AWB Row.
                    if (((TextBox)gvr.FindControl("AWB")).Text == lblMAWB.Text)
                    {
                        ((TextBox)gvr.FindControl("HAWBCt")).Text = dt.Rows.Count.ToString();
                        ((TextBox)gvr.FindControl("RCVHAWBCt")).Text = ArrivedHAWBCt.ToString();
                        break;
                    }
                }
            }
            catch (Exception Ex)
            {
                dt = null;
                lblStatus.Text = Ex.Message;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
            }
        }
        #endregion

        #region btnDeleteHAWB click
        protected void btnDeleteAWB_Click(object sender, EventArgs e)
        {
            lblHAWBStatus.Text = "";
            lblStatus.Text = "";
            //DataTable dt = null;
            DataTable dt = new DataTable("Arr_DelAwb_dtDel");
            try
            {
                string HAWBNoTest = ((Label)gvHAWBDetails.Rows[0].FindControl("lblHAWBNo")).Text;
                if (HAWBNoTest == "DUMMY" && gvHAWBDetails.Rows.Count == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr22", "javascript:callclose1();", true);
                    return;
                }
                dt = (DataTable)(Session["ARR_HAWBDetails" + lblMAWB.Text]);
                for (int i = gvHAWBDetails.Rows.Count - 1; i >= 0; i--)
                {
                    if (((CheckBox)(gvHAWBDetails.Rows[i].FindControl("check"))).Checked)
                    {   //Validate if HAWB being deleted is Manually Added.
                        if (((HiddenField)(gvHAWBDetails.Rows[i].FindControl("hdnManualHAWB"))).Value == "Y")
                        {
                            DataRow dr = dt.Rows[i];
                            dt.Rows.Remove(dr);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:alert('You can not delete HAWB which is not added Manually');", true);
                            break;
                        }
                    }
                }
                (Session["ARR_HAWBDetails" + lblMAWB.Text]) = dt;
                Refresh_gvH();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "scr28", "javascript:callclose1();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callShow1();</script>", false);
            }
            catch (Exception Ex)
            {
                dt = null;
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = Ex.Message;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }
        #endregion

        #region Refresh Gridview
        protected void Refresh_gvH()
        {
            //DataTable dt = null;
            DataTable dt = new DataTable("Arrival_Refresh_gvH");
            try
            {
                dt = (DataTable)(Session["ARR_HAWBDetails" + lblMAWB.Text]);
                if (dt.Rows.Count == 0)
                {
                    dt.Rows.Add("DUMMY", "DUMMY", 0, 0, "", "", "", "", "", "", "", "", "", "", false, 'Y');
                    gvHAWBDetails.DataSource = dt;
                    gvHAWBDetails.DataBind();
                    gvHAWBDetails.Rows[0].Visible = false;
                }
                else if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["MAWBNo"].ToString() == "DUMMY" && dt.Rows.Count > 1)
                    {
                        DataRow drDummy = dt.Rows[0];
                        dt.Rows.Remove(drDummy);
                    }
                    gvHAWBDetails.DataSource = dt;
                    gvHAWBDetails.DataBind();
                    if (gvHAWBDetails.Rows.Count > 0)
                    {
                        gvHAWBDetails.Rows[0].Visible = true;
                        int rowIndex = 0;
                        foreach (GridViewRow gvr in gvHAWBDetails.Rows)
                        {   //Set arrival status.
                            ((CheckBox)gvr.FindControl("chkArrivalStatusRow")).Checked =
                                Convert.ToBoolean(dt.Rows[rowIndex]["ArrivalStatus"]);
                            //If HAWB is Not Manual & if it is Arrived then disable Arrival Checkbox.
                            if (((HiddenField)gvr.FindControl("hdnManualHAWB")).Value == "N" &&
                                Convert.ToBoolean(dt.Rows[rowIndex]["ArrivalStatus"]) == true)
                            {
                                ((CheckBox)gvr.FindControl("chkArrivalStatusRow")).Enabled = false;
                            }
                            else
                            {
                                ((CheckBox)gvr.FindControl("chkArrivalStatusRow")).Enabled = true;
                            }
                            rowIndex++;
                        }
                    }
                }
                ((TextBox)gvHAWBDetails.FooterRow.FindControl("txtOrigin")).Text = lblHAWBOrigin.Text;
                ((TextBox)gvHAWBDetails.FooterRow.FindControl("txtOrigin")).Enabled = false;
                ((TextBox)gvHAWBDetails.FooterRow.FindControl("txtDestination")).Text = lblHAWBDest.Text;
                ((TextBox)gvHAWBDetails.FooterRow.FindControl("txtDestination")).Enabled = false;
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// Create new row in a grid dynamically
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Create new row in a grid
        protected void lnkCreate_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                SaveRouteDetails();

                AddNewRowToGrid();

            }
            catch (Exception) { }

        }
        #endregion  Create new row in a grid

        #region Create new row in ULD grid
        protected void lnkULDCreate_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                SaveULDGridDetails();

                AddNewRowToULDGrid();

            }
            catch (Exception)
            { }

        }
        #endregion  Create new row in ULD grid

        #region GetData for the dynaicaly added row
        public void Getdata(object sender, EventArgs e)
        {
            try
            {
                //int rowno=GVArrDet.Rows.Count;
                TextBox txt = sender as TextBox;
                GridViewRow row = txt.NamingContainer as GridViewRow;
                int awbrowno = row.RowIndex;

                string awbno = ((TextBox)GVArrDet.Rows[awbrowno].FindControl("AWB")).Text.Trim();
                if (awbno.Contains(" "))
                {
                    awbno = awbno.Replace(" ", "");
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("AWB")).Text = awbno;
                }

                if (awbno.Length > 9)
                {
                    //For each row in grid, validate if same AWB # is already present.
                    for (int rowIndex = 0; rowIndex < GVArrDet.Rows.Count; rowIndex++)
                    {
                        if (rowIndex != awbrowno)
                        {
                            if (((TextBox)GVArrDet.Rows[rowIndex].FindControl("AWB")).Text.Trim() == awbno)
                            {
                                //AWB # is already available in the grid.
                                lblStatus.Text = "AWB # is already present in arrival.";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("AWB")).Text = "";
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("AWB")).Focus();
                                return;
                            }
                        }
                    }

                    lblStatus.Text = "";
                    //Clear existing data..
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("StdPcs"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("MftPcs"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("Expectedpcs"))).Text = "";

                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("ExpectedWeight"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("StdWt"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("MftWt"))).Text = "";

                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("BkdPcs"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("BkdWt"))).Text = "";

                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("RcvPcs"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("RcvWt"))).Text = "";

                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("POL"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("Origin"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("Destn"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("ULDDestn"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("CommCode"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("Remark"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("ULD"))).Text = "";

                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("txtArrivedPcs"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("txtArrivedWt"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("FlightNo"))).Text = "";
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("FltDate"))).Text = "";

                    //Enable fields for data entry.
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("POL")).Enabled = true;
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("ULDDestn")).Enabled = true;
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("AWB")).Enabled = true;
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Origin")).Enabled = true;
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Destn")).Enabled = true;
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("CustomStatusCode")).Enabled = true;
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Remark")).Enabled = true;
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("FlightNo")).Enabled = true;
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("FltDate")).Enabled = true;
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Shipper")).Enabled = true;
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Consignee")).Enabled = true;

                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("RcvPcs")).Enabled = true;
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("RcvWt")).Enabled = true;

                    ((DropDownList)GVArrDet.Rows[awbrowno].FindControl("ddlDiscrepancy")).Enabled = true;

                    DataSet ds = new DataSet("Arr_GetData1_dsRow");
                    ds = BlArl.Getdata(awbno);

                    #region Bind Data
                    if (ds != null)
                    {
                        //Check if AWB is already delivered in the system.
                        if (ds.Tables.Count > 1)
                        {
                            if (ds.Tables[1].Rows.Count > 0)
                            {   //AWB is Delivered.
                                lblStatus.Text = "AWB entered is either already arrived or delivered.";
                                lblStatus.ForeColor = Color.Red;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("AWB")).Text = "";
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("AWB")).Focus();
                                return;
                            }
                        }
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                int PiecesCount = Convert.ToInt32(ds.Tables[0].Rows[0]["PiecesCount"].ToString());
                                double GrossWeight = Convert.ToDouble(ds.Tables[0].Rows[0]["GrossWeight"].ToString());
                                string Origin = ds.Tables[0].Rows[0]["OriginCode"].ToString();
                                string Destination = ds.Tables[0].Rows[0]["DestinationCode"].ToString();
                                string Commodity = ds.Tables[0].Rows[0]["CommodityCode"].ToString();
                                string FltNumber = ds.Tables[0].Rows[0]["FltNumber"].ToString();

                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("StdPcs"))).Text = PiecesCount.ToString();
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("MftPcs"))).Text = PiecesCount.ToString();
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("Expectedpcs"))).Text = PiecesCount.ToString();

                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("ExpectedWeight"))).Text = GrossWeight.ToString();
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("StdWt"))).Text = GrossWeight.ToString();
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("MftWt"))).Text = GrossWeight.ToString();

                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("BkdPcs"))).Text = PiecesCount.ToString();
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("BkdWt"))).Text = GrossWeight.ToString();

                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("RcvPcs"))).Text = PiecesCount.ToString();
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("RcvWt"))).Text = GrossWeight.ToString();

                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("POL"))).Text = Origin.ToString();
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("Origin"))).Text = Origin.ToString();
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("Destn"))).Text = Destination.ToString();
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("ULDDestn"))).Text = Destination.ToString();
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("CommCode"))).Text = Commodity.ToString();
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("Remark"))).Text = FltNumber.ToString();
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("ULD"))).Text = "Bulk";

                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("txtArrivedPcs"))).Text = "0";
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("txtArrivedWt"))).Text = "0";
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("FlightNo"))).Text = txtFlightPrefix.Text + txtFlightNo.Text;
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("FltDate"))).Text = txtFlightDate.Text;

                                //Disable read only fields to stop user from editing.
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Expectedpcs")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("ExpectedWeight")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("ULD")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("POL")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("ULDDestn")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Owner")).Enabled = false; ;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("MftPcs")).Enabled = false; ;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("MftWt")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("txtArrivedWt")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("txtArrivedPcs")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Origin")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Destn")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("StdPcs")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("StdWt")).Enabled = false;
                                ((CheckBox)GVArrDet.Rows[awbrowno].FindControl("SecurityCheck")).Enabled = false;
                                ((CheckBox)GVArrDet.Rows[awbrowno].FindControl("CustomCheck")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("CustomStatusCode")).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("FlightNo")).Enabled = false;
                                ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("FltDate"))).Enabled = false;
                                ((TextBox)GVArrDet.Rows[awbrowno].FindControl("CommCode")).Enabled = false;

                                ds.Dispose();
                                return;
                            }
                        }

                    }
                    //Set Flt # & Flt Date from top of page by default.
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("FlightNo"))).Text =
                        txtFlightPrefix.Text + txtFlightNo.Text;
                    ((TextBox)(GVArrDet.Rows[awbrowno].FindControl("FltDate"))).Text = txtFlightDate.Text;
                    ////Data not found.
                    //lblStatus.Text = "AWB is not manifested..";
                    //lblStatus.ForeColor = Color.Red;
                    #endregion Bind Data
                    if (ds != null)
                        ds.Dispose();
                }
                else
                {

                    lblStatus.Text = "Please enter valid AWB # in Prefix-Number format.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception)
            { }
        }

        public void GetdataULD(object sender, EventArgs e)
        {
            try
            {
                //int rowno=GVArrDet.Rows.Count;
                TextBox txt = sender as TextBox;
                GridViewRow row = txt.NamingContainer as GridViewRow;
                int awbrowno = row.RowIndex;

                string awbno = ((TextBox)grdULDDetails.Rows[awbrowno].FindControl("GrdULDNo")).Text.Trim();

                lblStatus.Text = "";

                DataSet ds = new DataSet("Arrival_dynRow_dsUld");
                ds = BlArl.GetdataULd(awbno);

                #region Bind Data
                if (ds != null)
                {
                    //Bind data extracted from operation uld table.
                    if (ds.Tables.Count > 0)
                    {
                        grdULDDetails.DataSource = ds.Tables[0];
                        grdULDDetails.DataBind();
                    }
                    ds.Dispose();
                }
                else
                {
                    //Set Flt # & Flt Date from top of page by default.
                    ((TextBox)(grdULDDetails.Rows[awbrowno].FindControl("GrdULDFlightNo"))).Text =
                        txtFlightPrefix.Text + txtFlightNo.Text;
                    ((TextBox)(grdULDDetails.Rows[awbrowno].FindControl("GrdULDFlightDate"))).Text = txtFlightDate.Text;
                    ((TextBox)(grdULDDetails.Rows[awbrowno].FindControl("GrdULDDestn"))).Text =
                        Session["Station"].ToString();

                    //lblStatus.Text = "ULD is not manifested / not exists..";
                    //lblStatus.ForeColor = Color.Red;
                    //return;
                }
                #endregion Bind Data
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #endregion GetData for the dynaicaly added row

        private int mostRecentRowIndex = -1;

        protected void GVArrDet_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                mostRecentRowIndex = e.Row.RowIndex;
            }
            catch (Exception)
            { }
        }

        protected void GVArrDet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                awbrowno = 0;
            }
            catch (Exception)
            { }
        }

        //shashikant....Messaging Code
        # region btnSendAAR_Click
        protected void btnSendAAR_Click(object sender, EventArgs e)
        {
            try
            {

                if (grdULDDetails.Rows.Count > 0)
                {
                    for (int i = 0; i < grdULDDetails.Rows.Count; i++)
                    {
                        if (((CheckBox)grdULDDetails.Rows[i].FindControl("checkULD")).Checked == true)
                        {
                            string uldno, fltno, fltdt, org, dest;
                            uldno = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Text;
                            if (uldno == "")
                                return;
                            fltno = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightNo")).Text;
                            fltdt = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightDate")).Text;
                            org = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDOrg")).Text;
                            dest = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDDestn")).Text;
                            object[] param = { uldno, fltno, fltdt, org, dest };
                            string[] pname = { "uldno", "fltno", "fltdt", "org", "dest" };
                            SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                            da.ExecuteProcedure("sp_SendFSAAAR_ULD", pname, QueryTypes, param);
                        }
                    }
                }
                if (GVArrDet.Rows.Count > 0)
                {
                    for (int i = 0; i < GVArrDet.Rows.Count; i++)
                    {
                        if (((CheckBox)GVArrDet.Rows[i].FindControl("check")).Checked == true)
                        {
                            string awbno, fltno, fltdt, org, dest;
                            awbno = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text;
                            if (awbno == "")
                                return;
                            fltno = ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text;
                            fltdt = ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Text;
                            org = ((TextBox)GVArrDet.Rows[i].FindControl("Origin")).Text;
                            dest = ((TextBox)GVArrDet.Rows[i].FindControl("Destn")).Text;
                            object[] param = { awbno, fltno, fltdt, org, dest };
                            string[] pname = { "awbno", "fltno", "fltdt", "org", "dest" };
                            SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                            da.ExecuteProcedure("sp_SendFSAAAR_AWB", pname, QueryTypes, param);
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
        }
        # endregion btnSendAAR_Click

        //shashikant .....Messaging Code
        # region btnOK_Click
        protected void btnOK_Click(object sender, EventArgs e)
        {
            //Sumit 2014-10-30
            try
            {
                string AirlinePrefix = txtFlightPrefix.Text.Trim();
                string AWBNo = "";
                string CarrierCode = txtFlightPrefix.Text.Trim();
                string FlightNo = txtFlightNo.Text.Trim(); ;
                string OperType = "ARR";
                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {
                    if (((CheckBox)GVArrDet.Rows[i].FindControl("check")).Checked == true)
                    {
                        AWBNo = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text.Substring(2);
                        if (AWBNo.Length > 0)
                        {
                            bool flag = cls_BL.EncodeFSAForSend(AirlinePrefix, AWBNo, CarrierCode, FlightNo, OperType, "swapnil@qidtech.com", txtEmailID.Text.Trim());

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);
                            lblStatus.Text = "FSA/ARR Sent Successfully";
                            lblStatus.ForeColor = Color.Green;
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);
            }
            catch (Exception ex) { }
        }
        # endregion btnOK_Click

        public void LoadSystemParameters()
        {
            //Sumit 2014-10-30
            try
            {
                LoginBL objBL = new LoginBL();
                string SmartKargoInstance = string.Empty;
                string ManifestPartner = string.Empty;

                //if (Session["SKI"] == null || Convert.ToString(Session["SKI"]) == "")
                //{
                //    SmartKargoInstance = objBL.GetMasterConfiguration("SmartKargoInstance");

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
                //Get configuration to set AWB Location Mandatory/ Optional.
                if (ViewState["AWBLocMandatoryInArr"] == null || ViewState["AWBLocMandatoryInArr"].ToString() == "")
                {
                    ViewState["AWBLocMandatoryInArr"] = false;
                    SmartKargoInstance = "";
                    //SmartKargoInstance = objBL.GetMasterConfiguration("AWBLocMandatoryInArr");
                    SmartKargoInstance = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "AWBLocMandatoryInArr");
                    if (SmartKargoInstance != null && SmartKargoInstance != "")
                    {
                        ViewState["AWBLocMandatoryInArr"] = Convert.ToBoolean(SmartKargoInstance);
                    }
                }
                //Get configuration to set ULD Location Mandatory/ Optional.
                if (ViewState["ULDLocMandatoryInArr"] == null || ViewState["ULDLocMandatoryInArr"].ToString() == "")
                {
                    ViewState["ULDLocMandatoryInArr"] = false;
                    SmartKargoInstance = "";
                    //SmartKargoInstance = objBL.GetMasterConfiguration("ULDLocMandatoryInArr");
                    SmartKargoInstance = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ULDLocMandatoryInArr");
                    if (SmartKargoInstance != null && SmartKargoInstance != "")
                    {
                        ViewState["ULDLocMandatoryInArr"] = Convert.ToBoolean(SmartKargoInstance);
                    }
                }
                try
                {
                    //if (!Convert.ToBoolean(objBL.GetMasterConfiguration("handleCIMPMsg")))
                    if (!Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "handleCIMPMsg")))
                    {
                        btnSendAAR.Visible = false;
                        btnSendFSN.Visible = false;
                        btnSendACAS.Visible = false;
                    }
                }
                catch (Exception ex) { }
                objBL = null;
            }
            catch (Exception ex) { }
        }

        public bool ValidateFlightPrtefix()
        {
            //DataSet ds = null;
            DataSet ds = new DataSet("Arr_FltPrex_dsVal");
            LoginBL Bal = new LoginBL();
            bool IsMatched = false;
            try
            {
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
                            if (txtFlightPrefix.Text.Trim().ToUpper() == ds.Tables[0].Rows[intCount]["DesignatorCode"].ToString().ToUpper())
                            {
                                IsMatched = true;
                                break;
                            }
                        }
                        ds.Dispose();
                    }
                }
            }
            catch (Exception)
            {
            }
            return IsMatched;
        }

        #region btnOpsTime_Click
        protected void btnOpsTime_Click(object sender, ImageClickEventArgs e)
        {
            SaveOperationTime(true);
        }
        #endregion btnOpsTime_Click


        #region Button Send AMS Messages
        protected void btnSendFSN_Click(object sender, EventArgs e)
        {
            try
            {
                int countFSN = 0;
                int count = 0;
                lblStatus.Text = "";
                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {
                    string ArrAWBNumber = string.Empty;
                    if (((CheckBox)GVArrDet.Rows[i].FindControl("check")).Checked == true)
                    {
                        count++;

                        //Validate if AWB Number is not blank.
                        if (((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text == "" ||
                            ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text.Length < 8)
                        {
                            return;
                        }
                        //Validate if Flight # is entered.
                        if (((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text.Length < 2)
                        {
                            return;
                        }
                        //Validate if Flight Date is entered.
                        if (((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Text.Length < 8)
                        {
                            return;
                        }
                        ArrAWBNumber = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text;
                        string FlightNo = ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text;
                        string FlightDate = ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Text;
                        string ArrivedPcs = ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Text;
                        int ArrPcs = 0;
                        try
                        { ArrPcs = Convert.ToInt32(ArrivedPcs); }
                        catch (Exception ex)
                        { return; }
                        if (ArrPcs > 0)
                        {
                            // Deepak- Code for Customs Auto Messaging
                            try
                            {
                                object[] QueryValues = new object[3];
                                QueryValues[0] = ArrAWBNumber;
                                QueryValues[1] = FlightNo;
                                QueryValues[2] = FlightDate;
                                string[] AWBDetails = ArrAWBNumber.Split('-');
                                CustomsImportBAL objCustoms = new CustomsImportBAL();

                                DataSet dCust = new DataSet("Arr_Fsn_dCust");
                                dCust = objCustoms.CheckCustomsAWBAvailability(QueryValues);

                                if (dCust != null)
                                {
                                    if (dCust.Tables[1].Rows[0]["Validate"].ToString() == "True")
                                    {
                                        if (Convert.ToBoolean(((HiddenField)GVArrDet.Rows[i].FindControl("hdIsManual")).Value))
                                        {
                                            #region Send FRI MESSAGE CUSTOMS
                                            try
                                            {
                                                object[] QueryVals = { ArrAWBNumber, FlightNo, FlightDate };
                                                StringBuilder sbFRI = objCustoms.EncodingFRIMessage(QueryVals);
                                                object[] QueryValsFRI = { ArrAWBNumber, 1, FlightNo, FlightDate, sbFRI.ToString().ToUpper() };

                                                if (objCustoms.UpdateFRIMessage(QueryValsFRI))
                                                {

                                                    if (sbFRI != null)
                                                    {
                                                        if (sbFRI.ToString() != "")
                                                        {
                                                            object[] QueryValMail = { "FRI", FlightNo, FlightDate };
                                                            //Getting MailID for FRI Message

                                                            DataSet dMail = new DataSet("Arr_Fsn_DsFRI");
                                                            dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                                            string MailID = string.Empty;
                                                            if (dMail != null)
                                                            {
                                                                MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                            }
                                                            cls_BL.addMsgToOutBox("FRI", sbFRI.ToString().ToUpper(), "", MailID);
                                                        }


                                                    }
                                                    countFSN++;
                                                    lblStatus.Text = "AMS Message(s) Sent Successfully!";
                                                    lblStatus.ForeColor = Color.Green;
                                                }

                                            }
                                            catch (Exception ex)
                                            { }
                                            #endregion
                                        }
                                        StringBuilder sb = objCustoms.EncodingFSNMessage(QueryValues);
                                        object[] QueryVal = { QueryValues[0], QueryValues[1], QueryValues[2], sb.ToString().ToUpper() };
                                        if (objCustoms.UpdateFSNMessage(QueryVal))
                                        {
                                            if (sb != null)
                                            {
                                                if (sb.ToString() != "")
                                                {
                                                    object[] QueryValMail = { "FSN", FlightNo, FlightDate };
                                                    //Getting MailID for FSN Message
                                                    DataSet dMail = new DataSet("Arr_Fsn_DFsn");
                                                    dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);

                                                    string MailID = string.Empty;
                                                    if (dMail != null)
                                                    {
                                                        MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                    }
                                                    cls_BL.addMsgToOutBox("FSN", sb.ToString().ToUpper(), "", MailID);
                                                }
                                            }
                                            lblStatus.Text = "AMS Message(s) Sent Successfully!";
                                            lblStatus.ForeColor = Color.Green;
                                        }
                                    }
                                }
                                objCustoms = null;
                                dCust.Dispose();
                                QueryValues = null;
                                AWBDetails = null;
                            }
                            catch (Exception ex)
                            {
                                #region Sending PER Error Message on System Error
                                try
                                {
                                    CustomsImportBAL objACAS = new CustomsImportBAL();
                                    object[] QueryValues = new object[3];
                                    QueryValues[0] = ArrAWBNumber;
                                    QueryValues[1] = FlightNo;
                                    QueryValues[2] = FlightDate;

                                    StringBuilder sbPER = objACAS.EncodingFERMessage(QueryValues);
                                    object[] QueryValPER = { QueryValues[0], QueryValues[1], QueryValues[2], sbPER.ToString().ToUpper() };
                                    objACAS.UpdateFERMessage(QueryValPER);

                                    if (sbPER != null)
                                    {
                                        if (sbPER.ToString() != "")
                                        {
                                            object[] QueryValMail = { "FER", FlightNo, FlightDate };
                                            //Getting MailID for PER Message
                                            DataSet dMail = new DataSet("Arr_Fsn_PerMsg");
                                            dMail = objACAS.GetCustomMessagesMailID(QueryValMail);

                                            string MailID = string.Empty;
                                            if (dMail != null)
                                            {
                                                MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                            }
                                            cls_BL.addMsgToOutBox("FER", sbPER.ToString().ToUpper(), "", MailID);
                                        }
                                    }
                                }
                                catch (Exception Ex)
                                { }
                                #endregion

                                lblStatus.Text = countFSN > 0 ? ArrAWBNumber + ": FRI Sent Successfully! FSN Couldn't be sent as Shipper & Consignee data is missing!" : string.Empty;
                                lblStatus.ForeColor = Color.Blue;

                            }
                        }



                    }
                }
                if (count == 0)
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

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetCustomStatusCodes(string prefixText, int count)
        {
            //Sumit 2014-10-30
            try
            {
                string con = Global.GetConnectionString();
                // SqlConnection con = new SqlConnection("connection string"); 
                SqlDataAdapter dad = new SqlDataAdapter("SELECT CONVERT(VARCHAR,StatusCode) + '(' + Description + ')' from tblStatusCodes where (Description like '%" + prefixText + "%' or StatusCode like '%" + prefixText + "%')", con);
                DataSet ds = new DataSet("Arrival_WebServiceDS");
                dad.Fill(ds);
                List<string> list = new List<string>(ds.Tables[0].Rows.Count);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(dr[0].ToString());

                }
                dad = null;
                if (ds != null)
                    ds.Dispose();

                return list.ToArray();
            }
            catch (Exception ex) { return null; }
        }

        private void HandleVehicleInfo()
        {
            LoginBL objBL = new LoginBL();

            try
            {
                if (Session["HandleVehicleInfo"] == null)
                    //Session["HandleVehicleInfo"] = Convert.ToBoolean(objBL.GetMasterConfiguration("HandleVehicleInfo"));
                    Session["HandleVehicleInfo"] = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "HandleVehicleInfo"));

                if (!Convert.ToBoolean(Session["HandleVehicleInfo"]))
                {
                    TpMargin.Visible = false;
                }
            }
            catch (Exception ex) { Session["HandleVehicleInfo"] = false; }
            finally { objBL = null; }
        }

        #region btnSaveShipper_Click
        protected void btnSaveShipper_Click(object sender, EventArgs e)
        {
            try
            {
                ShipperConsignee objShipper = new ShipperConsignee();
                objShipper.AccountCode = txtShipperAccountCode.Text;
                objShipper.Address1 = TXTShipAddress.Text;
                objShipper.Address2 = TXTShipperAdd2.Text;
                objShipper.City = TXTShipperCity.Text;
                objShipper.Country = ddlShipCountry.SelectedValue;
                objShipper.Email = TXTShipperEmail.Text;
                objShipper.Name = TXTShipper.Text;
                objShipper.Pincode = TXTShipPinCode.Text;
                objShipper.State = TXTShipperState.Text;
                objShipper.Telephone = TXTShipTelephone.Text;
                //Set values in session.
                Session["Arrival_Shipper_" + lblShipperAWB.Text] = objShipper;

                //Hide shipper details popup.
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideShipper",
                    "<SCRIPT LANGUAGE='javascript'>HidePanel_shipperPopUp();</script>", false);

                //Set shipper as Y in grid.
                if (TXTShipper.Text != "")
                {
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Shipper")).Text = "Y";
                }
                else
                {
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Shipper")).Text = "N";
                }

            }
            catch (Exception)
            {
            }
        }
        #endregion btnSaveShipper_Click

        #region Clear Shipper Consignee Values
        private void ClearShipperConsigneeValues()
        {
            try
            {
                //Clear shipper data.
                txtShipperAccountCode.Text = "";
                TXTShipper.Text = "";
                TXTShipAddress.Text = "";
                TXTShipperAdd2.Text = "";
                TXTShipperCity.Text = "";
                TXTShipperEmail.Text = "";
                TXTShipperState.Text = "";
                TXTShipPinCode.Text = "";
                TXTShipTelephone.Text = "";

                //Clear consignee data.
                TXTConAddress.Text = "";
                TXTConsigAdd2.Text = "";
                TXTConsigCity.Text = "";
                TXTConsigCity.Text = "";
                TXTConsigEmail.Text = "";
                TXTConsignee.Text = "";
                txtConsigneeAccountCode.Text = "";
                TXTConsigPinCode.Text = "";
                TXTConsigState.Text = "";
                TXTConTelephone.Text = "";

                ddlShipCountry.SelectedValue = "Select";
                ddlConCountry.SelectedValue = "Select";
            }
            catch (Exception)
            {
            }
        }
        #endregion Clear Shipper Consignee Values

        #region btnCancelShipper_Click
        protected void btnCancelShipper_Click(object sender, EventArgs e)
        {
            //Hide shipper details popup.
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideShipper",
                "<SCRIPT LANGUAGE='javascript'>HidePanel_shipperPopUp();</script>", false);
        }
        #endregion btnCancelShipper_Click

        #region btnCancelConsignee_Click
        protected void btnCancelConsignee_Click(object sender, EventArgs e)
        {
            //Hide consignee details popup.
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideConsignee",
                "<SCRIPT LANGUAGE='javascript'>HidePanel_ConsigneePopUp();</script>", false);
        }
        #endregion btnCancelConsignee_Click

        #region btnSaveConsignee_Click
        protected void btnSaveConsignee_Click(object sender, EventArgs e)
        {
            try
            {
                ShipperConsignee objConsignee = new ShipperConsignee();
                objConsignee.AccountCode = txtConsigneeAccountCode.Text;
                objConsignee.Address1 = TXTConAddress.Text;
                objConsignee.Address2 = TXTConsigAdd2.Text;
                objConsignee.City = TXTConsigCity.Text;
                objConsignee.Country = ddlConCountry.SelectedValue;
                objConsignee.Email = TXTConsigEmail.Text;
                objConsignee.KnownShipper = "";
                objConsignee.Name = TXTConsignee.Text;
                objConsignee.Pincode = TXTConsigPinCode.Text;
                objConsignee.State = TXTConsigState.Text;
                objConsignee.Telephone = TXTConTelephone.Text;
                //Set values in session.
                Session["Arrival_Consignee_" + lblConsigneeAWB.Text] = objConsignee;

                //Hide shipper details popup.
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideConsignee",
                    "<SCRIPT LANGUAGE='javascript'>HidePanel_ConsigneePopUp();</script>", false);

                //Set consignee as Y in grid.
                if (TXTConsignee.Text != "")
                {
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Consignee")).Text = "Y";
                }
                else
                {
                    ((TextBox)GVArrDet.Rows[awbrowno].FindControl("Consignee")).Text = "N";
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion btnSaveConsignee_Click

        #region Button Send ACAS Messages
        protected void btnSendACAS_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                lblStatus.Text = "";
                for (int i = 0; i < GVArrDet.Rows.Count; i++)
                {
                    string ArrAWBNumber = string.Empty;
                    if (((CheckBox)GVArrDet.Rows[i].FindControl("check")).Checked == true)
                    {
                        count++;

                        //Validate if AWB Number is not blank.
                        if (((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text == "" ||
                            ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text.Length < 8)
                        {
                            return;
                        }
                        //Validate if Flight # is entered.
                        if (((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text.Length < 2)
                        {
                            return;
                        }
                        //Validate if Flight Date is entered.
                        if (((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Text.Length < 8)
                        {
                            return;
                        }
                        ArrAWBNumber = ((TextBox)GVArrDet.Rows[i].FindControl("AWB")).Text;
                        string FlightNo = ((TextBox)GVArrDet.Rows[i].FindControl("FlightNo")).Text;
                        string FlightDate = ((TextBox)GVArrDet.Rows[i].FindControl("FltDate")).Text;
                        string ArrivedPcs = ((TextBox)GVArrDet.Rows[i].FindControl("txtArrivedPcs")).Text;
                        int ArrPcs = 0;
                        try
                        { ArrPcs = Convert.ToInt32(ArrivedPcs); }
                        catch (Exception ex)
                        { return; }
                        if (ArrPcs > 0)
                        {
                            // Deepak- Code for Customs Auto Messaging
                            try
                            {
                                object[] QueryValues = new object[3];
                                QueryValues[0] = ArrAWBNumber;
                                QueryValues[1] = FlightNo;
                                QueryValues[2] = FlightDate;
                                string[] AWBDetails = ArrAWBNumber.Split('-');
                                ACASBAL objCustoms = new ACASBAL();

                                DataSet dCust = new DataSet("Arr_BtnACAS_dCust");
                                dCust = objCustoms.CheckACASAWBAvailability(QueryValues);
                                if (dCust != null)
                                {
                                    if (dCust.Tables[1].Rows[0]["Validate"].ToString() == "True")
                                    {
                                        if (Convert.ToBoolean(((HiddenField)GVArrDet.Rows[i].FindControl("hdIsManual")).Value))
                                        {
                                            #region Send PRI MESSAGE CUSTOMS
                                            try
                                            {
                                                object[] QueryVals = { ArrAWBNumber, FlightNo, FlightDate };
                                                StringBuilder sbFRI = objCustoms.EncodingPRIMessage(QueryVals);
                                                object[] QueryValsFRI = { ArrAWBNumber, 1, FlightNo, FlightDate, sbFRI.ToString().ToUpper() };

                                                if (objCustoms.UpdatePRIMessage(QueryValsFRI))
                                                {

                                                    if (sbFRI != null)
                                                    {
                                                        if (sbFRI.ToString() != "")
                                                        {
                                                            object[] QueryValMail = { "PRI", FlightNo, FlightDate };
                                                            //Getting MailID for FRI Message
                                                            DataSet dMail = new DataSet("Arrl_FRI_ACAS1");
                                                            dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                                            string MailID = string.Empty;
                                                            if (dMail != null)
                                                            {
                                                                MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                            }
                                                            cls_BL.addMsgToOutBox("PRI", sbFRI.ToString().ToUpper(), "", MailID);
                                                        }


                                                    }
                                                    lblStatus.Text = "ACAS Message(s) Sent Successfully!";
                                                    lblStatus.ForeColor = Color.Green;
                                                }

                                            }
                                            catch (Exception ex)
                                            { }
                                            #endregion
                                        }
                                        StringBuilder sb = objCustoms.EncodingPSNMessage(QueryValues);
                                        object[] QueryVal = { QueryValues[0], QueryValues[1], QueryValues[2], sb.ToString().ToUpper() };
                                        if (objCustoms.UpdatePSNMessage(QueryVal))
                                        {
                                            if (sb != null)
                                            {
                                                if (sb.ToString() != "")
                                                {
                                                    object[] QueryValMail = { "PSN", FlightNo, FlightDate };
                                                    //Getting MailID for FSN Message
                                                    DataSet dMail = new DataSet("Arrl_PSN_ACAS2");
                                                    dMail = objCustoms.GetCustomMessagesMailID(QueryValMail);
                                                    string MailID = string.Empty;
                                                    if (dMail != null)
                                                    {
                                                        MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                                    }
                                                    cls_BL.addMsgToOutBox("PSN", sb.ToString().ToUpper(), "", MailID);
                                                }
                                            }
                                            lblStatus.Text = "ACAS Message(s) Sent Successfully!";
                                            lblStatus.ForeColor = Color.Green;
                                        }
                                    }
                                }
                                objCustoms = null;
                                dCust.Dispose();
                                QueryValues = null;
                                AWBDetails = null;
                            }
                            catch (Exception ex)
                            {

                                #region Sending PER Error Message on System Error
                                try
                                {
                                    ACASBAL objACAS = new ACASBAL(); object[] QueryValues = new object[3];
                                    QueryValues[0] = ArrAWBNumber;
                                    QueryValues[1] = FlightNo;
                                    QueryValues[2] = FlightDate;

                                    StringBuilder sbPER = objACAS.EncodingPERMessage(QueryValues);
                                    object[] QueryValPER = { QueryValues[0], QueryValues[1], QueryValues[2], sbPER.ToString().ToUpper() };
                                    objACAS.UpdatePERMessage(QueryValPER);

                                    if (sbPER != null)
                                    {
                                        if (sbPER.ToString() != "")
                                        {
                                            object[] QueryValMail = { "PER", FlightNo, FlightDate };
                                            //Getting MailID for PER Message
                                            DataSet dMail = new DataSet("Arrl_PER_ACAS");
                                            dMail = objACAS.GetCustomMessagesMailID(QueryValMail);
                                            string MailID = string.Empty;
                                            if (dMail != null)
                                            {
                                                MailID = dMail.Tables[0].Rows[0]["EmailID"].ToString();
                                            }
                                            cls_BL.addMsgToOutBox("PER", sbPER.ToString().ToUpper(), "", MailID);
                                        }
                                    }
                                }
                                catch (Exception Ex)
                                { }
                                #endregion
                            }
                        }



                    }
                }
                if (count == 0)
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

        #region grdULDDetails_RowDataBound
        /// <summary>
        /// This event binds 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdULDDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow row = e.Row;
                if (row.DataItem == null)
                {
                    return;
                }
                //DataTable dt = null;
                DataTable dt = new DataTable("grdULD_Rwbnd_dt");
                object[] paramvalue = new object[4];

                paramvalue[0] = ((DataRowView)e.Row.DataItem)["FltNo"].ToString().Trim();

                paramvalue[1] = ((DataRowView)e.Row.DataItem)["FltDate"].ToString().Trim();

                paramvalue[2] = Session["Station"].ToString();
                paramvalue[3] = ((DataRowView)e.Row.DataItem)["ULDNo"].ToString().Trim();

                dt = BlArl.GetULDChildAWB(paramvalue);

                if (dt != null && dt.Rows.Count > 0)
                {

                    GridView gv = new GridView();
                    gv = (GridView)row.FindControl("GVSubArrDet");

                    //Bind the DataTable to the Grid 
                    gv.DataSource = null;
                    gv.DataSource = dt;
                    gv.DataBind();

                    //Set value of Custom Check checkbox.
                    for (int i = 0; i < gv.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["CustomCheck"] != null && dt.Rows[i]["CustomCheck"].ToString() != "")
                        {
                            ((CheckBox)gv.Rows[i].FindControl("CustomCheck")).Checked =
                                Convert.ToBoolean(dt.Rows[i]["CustomCheck"]);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion grdULDDetails_RowDataBound

        #region ShipperCodeChanges Event
        protected void ShipperCodeDetailsChanged(object sender, EventArgs e)
        {
            ShipperBAL objSHBal = null;
            //DataSet ds = null;
            DataSet ds = new DataSet("Arr_Evntchg_Ds");

            try
            {
                if (sender.Equals(txtShipperAccountCode))
                {
                    objSHBal = null;
                    ds = null;

                    if (txtShipperAccountCode.Text != "")
                    {
                        objSHBal = new ShipperBAL();
                        ds = objSHBal.GetShipperAccountInfo(txtShipperAccountCode.Text.Trim(), Convert.ToDateTime(Session["IT"]).Date);

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            TXTShipper.Text = ds.Tables[0].Rows[0]["AccountName"].ToString();
                            TXTShipTelephone.Text = ds.Tables[0].Rows[0]["PhoneNumber"].ToString();
                            TXTShipAddress.Text = ds.Tables[0].Rows[0]["Adress1"].ToString();
                            TXTShipperAdd2.Text = ds.Tables[0].Rows[0]["Adress2"].ToString();
                            TXTShipperCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                            TXTShipperState.Text = ds.Tables[0].Rows[0]["State"].ToString();
                            ddlShipCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                            TXTShipPinCode.Text = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                            TXTShipperEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                        }
                    }
                    else if (txtShipperAccountCode.Text == "")
                    {
                        return;
                    }
                }
                else if (sender.Equals(txtConsigneeAccountCode))
                {
                    objSHBal = null;
                    ds = null;

                    if (txtConsigneeAccountCode.Text == "")
                    {
                        return;
                    }

                    objSHBal = new ShipperBAL();
                    ds = objSHBal.GetShipperAccountInfo(txtConsigneeAccountCode.Text.Trim(), Convert.ToDateTime(Session["IT"]).Date);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        TXTConsignee.Text = ds.Tables[0].Rows[0]["AccountName"].ToString();
                        TXTConTelephone.Text = ds.Tables[0].Rows[0]["PhoneNumber"].ToString();
                        TXTConAddress.Text = ds.Tables[0].Rows[0]["Adress1"].ToString();
                        TXTConsigAdd2.Text = ds.Tables[0].Rows[0]["Adress2"].ToString();
                        TXTConsigCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                        TXTConsigState.Text = ds.Tables[0].Rows[0]["State"].ToString();
                        ddlConCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                        TXTConsigPinCode.Text = ds.Tables[0].Rows[0]["ZipCode"].ToString();
                        TXTConsigEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                objSHBal = null;
                ds = null;
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Error in ShipperCode";
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                objSHBal = null;
                if (sender.Equals(txtConsigneeAccountCode))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanel_ConsigneePopUp();</script>", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanel_shipperPopUp();</script>", false);
                }
            }
        }
        #endregion

        #region GetShipperCode_Autopopulate
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetShipperCode(string prefixText, int count)
        {
            //Sumit 2014-10-30
            try
            {
                string con = Global.GetConnectionString();
                // SqlConnection con = new SqlConnection("connection string");
                SqlDataAdapter dad = new SqlDataAdapter("SELECT AccountCode + '(' +AccountName+ ')' from AccountMaster where (AccountName like '" + prefixText + "%' or AccountCode like '" + prefixText + "%') and ParticipationType in ('Both','Shipper')", con);
                DataSet ds = new DataSet("Arr_WebMthd_dsCode");
                dad.Fill(ds);
                List<string> list = new List<string>(ds.Tables[0].Rows.Count);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(dr[0].ToString());
                }

                if (ds != null)
                    ds.Dispose();
                dad = null;
                return list.ToArray();
            }
            catch (Exception ex) { return null; }
        }
        #endregion GetShipperCode

        #region GetConsigneeCode_AutoPopulate
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetConsigneeCode(string prefixText, int count)
        {
            //Sumit 2014-10-30
            try
            {
                string con = Global.GetConnectionString();
                // SqlConnection con = new SqlConnection("connection string");
                SqlDataAdapter dad = new SqlDataAdapter("SELECT AccountCode + '(' +AccountName+ ')' from AccountMaster where (AccountName like '" + prefixText + "%' or AccountCode like '" + prefixText + "%') and ParticipationType in ('Both','Consignee')", con);
                DataSet ds = new DataSet("GetCon_dsWeb");
                dad.Fill(ds);
                List<string> list = new List<string>(ds.Tables[0].Rows.Count);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(dr[0].ToString());
                }

                if (ds != null)
                    ds.Dispose();
                dad = null;
                return list.ToArray();
            }
            catch (Exception ex) { return null; }
        }
        #endregion GetConsigneeCode_AutoPopulate

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

        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("dsArrival_dtAWBDetails", dtTable2));
            e.DataSources.Add(new ReportDataSource("dsArrival_dtULDDetails", dtTable3));

        }
    }
}
