using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using System.Configuration;
using System.Data.Sql;
using System.Collections;
using System.Drawing;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class frmCCAProcessing : System.Web.UI.Page
    {
        CCABal objBAL = new CCABal();
        MasterBAL objBal = new MasterBAL();
        string AWBPrefix = string.Empty;
        static Double ST = 0.0;
        static float Rate = 0;
        static float CommissionPercent = 0;
        static double ServiceTax, commissionamt, STComm, OrgServiceTax, OrgTDSOnCommissionAmt, OrgSTComm, RevSTComm;
        static double TDSOnComm; static double NormalComm;
        static double FinalCT = 0; static double FinalRT = 0;
        SQLServer db = new SQLServer(Global.GetConnectionString());
        static string PayMode = "";
        static double TDSOnCommissionAmt = 0;
        static double OrgTotal = 0.0;
        static double RevTotal = 0.0;
        string strflightdate;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                try
                {
                    Session["CCA_AWBNo"] = "";
                    Session["CCA_AWBPre"] = "";
                    Session["CCA_Mode"] = "";

                    lblStatus.Text = "";
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
                    CCADatatable();
                    if (Request.QueryString["CCANo"] != null && Request.QueryString["AWBNo"] != null)
                    {
                        string CCANo = Request.QueryString["CCANo"];
                        string AWBNo = Request.QueryString["AWBNo"];
                        Session["CCA_CCANo"] = CCANo;
                        Session["CCA_AWBNo"] = AWBNo;//AWBNo=Pre + AWBNo
                        Session["CCA_AWBPre"] = Request.QueryString["AWBPre"] == null ? "" :Request.QueryString["AWBPre"];
                        Session["CCA_Mode"] = Request.QueryString["Mode"] == null ? "" : Request.QueryString["Mode"];

                        // string FlightNo = Request.QueryString["FlightNo"];
                        //DateTime FlightDate = DateTime.Parse(Request.QueryString["FlightDate"]);
                        //DateTime dt = DateTime.ParseExact(FlightDate, "dd/MM/yyyy",null);
                        if (CCANo != null && AWBNo != null)
                        {
                            DataSet ds = objBAL.GetCCADetails(CCANo, AWBNo);
                            //DataSet ds = objBAL.GetCCADetails(CCANo, AWBNo,FlightNo,FlightDate);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        txtAWBNo.Enabled = false;
                                        txtAWBPrefix.Enabled = false;
                                        txtFlightDate.Enabled = false;
                                        txtFlightNo.Enabled = false;
                                        txtFlightPrefix.Enabled = false;
                                        txtAWBNo.Text = ds.Tables[0].Rows[0]["AWBNumber"].ToString();
                                        txtAWBPrefix.Text = ds.Tables[0].Rows[0]["AWBPrefix"].ToString();
                                        //txtFlightNo.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString().Substring(2, ds.Tables[0].Rows[0]["FlightNo"].ToString().Trim().Length - 2);
                                        //txtFlightPrefix.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString().Substring(0, 2);
                                        //txtFlightNo.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString();
                                        //txtFlightDate.Text = ds.Tables[0].Rows[0]["FlightDate"].ToString();
                                        txtStatus.Text = ds.Tables[0].Rows[0]["Status"].ToString();
                                        txtNumber.Text = ds.Tables[0].Rows[0]["CCANumber"].ToString();
                                        string PMode = ds.Tables[0].Rows[0]["PayMode"].ToString();
                                        PayMode = PMode;
                                        txtAgentCode.Text = ds.Tables[0].Rows[0]["AgentCode"].ToString();
                                        txtAirlineCode.Text = ds.Tables[0].Rows[0]["AirlineCode"].ToString();
                                        txtCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                                        txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNumber"].ToString();
                                        txtInvoiceDate.Text = ds.Tables[0].Rows[0]["InvoiceDate"].ToString();
                                        txtCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                                        txtDestination.Text = ds.Tables[0].Rows[0]["Destination"].ToString();
                                        txtOrigin.Text = ds.Tables[0].Rows[0]["Origin"].ToString();
                                        txtOriginalWeight.Text = ds.Tables[0].Rows[0]["ChargableWeight"].ToString();
                                        txtOrgNetAmt.Text = ds.Tables[0].Rows[0]["NetAmount"].ToString();
                                        txtRemarksCorrection.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                                        txtCommodityCodeOrg.Text = ds.Tables[0].Rows[0]["CommodityCodeOrg"].ToString();
                                        txtOrgCommission.Text = ds.Tables[0].Rows[0]["Commission"].ToString();
                                        txtOrgIncentive.Text = ds.Tables[0].Rows[0]["Discount"].ToString();
                                        txtDateOfIssue.Text = ds.Tables[0].Rows[0]["AWBDate"].ToString();
                                        string WeightIndicator = ds.Tables[0].Rows[0]["WeightIndicator"].ToString();
                                        if (WeightIndicator == "K")
                                        {
                                            rdbKilo.Checked = true;
                                        }
                                        else
                                        {
                                            rdbPound.Checked = true;
                                        }
                                        if (PMode == "PP")
                                        {
                                            txtOrgWgtChrgsPP.Text = ds.Tables[0].Rows[0]["FreightRate"].ToString();
                                            txtOrgWgtChrgsCC.Text = "0";
                                            txtOrgValChargesCC.Text = "0";
                                            txtOrgValChargesPP.Text = ds.Tables[0].Rows[0]["ValCharges"].ToString();
                                            txtOrgOCDAPP.Text = ds.Tables[0].Rows[0]["OCDA"].ToString();
                                            txtOrgOCDCPP.Text = ds.Tables[0].Rows[0]["OCDC"].ToString();
                                            txtOrgOCDACC.Text = "0";
                                            txtOrgOCDCCC.Text = "0";
                                        }
                                        else
                                        {
                                            txtOrgWgtChrgsCC.Text = ds.Tables[0].Rows[0]["FreightRate"].ToString();
                                            txtOrgValChargesCC.Text = ds.Tables[0].Rows[0]["ValCharges"].ToString();
                                            txtOrgWgtChrgsPP.Text = "0";
                                            txtOrgValChargesPP.Text = "0";
                                            txtOrgOCDAPP.Text = "0";
                                            txtOrgOCDCPP.Text = "0";
                                            txtOrgOCDACC.Text = ds.Tables[0].Rows[0]["OCDA"].ToString();
                                            txtOrgOCDCCC.Text = ds.Tables[0].Rows[0]["OCDC"].ToString();
                                        }
                                        if (PMode == "PP")
                                        {

                                            txtOrgPayableAirline.Text = ds.Tables[0].Rows[0]["OrgNetPayableAirline"].ToString();//(float.Parse(ds.Tables[0].Rows[0]["NetAmount"].ToString()) + float.Parse(txtOrgOCDCPP.Text) - float.Parse(txtOrgOCDAPP.Text)).ToString();

                                        }
                                        else
                                        {
                                            txtOrgPayableAirline.Text = ds.Tables[0].Rows[0]["OrgNetPayableAirline"].ToString();//(float.Parse(ds.Tables[0].Rows[0]["NetAmount"].ToString()) + float.Parse(txtOrgOCDCCC.Text) - float.Parse(txtOrgOCDACC.Text)).ToString();

                                        }
                                        txtRevisedWeight.Text = ds.Tables[0].Rows[0]["RevisedChargableWeight"].ToString();
                                        if (PMode == "PP")
                                        {
                                            txtRevWeightChrgsPP.Text = ds.Tables[0].Rows[0]["RevisedFreightRate"].ToString();
                                            txtRevWeightChrgsCC.Text = "0";
                                            txtRevValChargesCC.Text = "0";
                                            txtRevWeightChrgsCC.Enabled = false;
                                            txtRevValChargesCC.Enabled = false;
                                            txtRevOCDACC.Enabled = false;
                                            txtRevOCDCCC.Enabled = false;
                                            //txtRevWeightChrgsPP.Enabled = true;
                                            //txtRevValChargesPP.Enabled = true;
                                            //txtRevOCDAPP.Enabled = true;
                                            //txtRevOCDCPP.Enabled = true;
                                            //txtRevCommission.Enabled = true;
                                            //txtRevisedIncentive.Enabled = true;
                                            txtRevValChargesPP.Text = ds.Tables[0].Rows[0]["RevisedValCharges"].ToString();
                                            txtRevCommission.Text = ds.Tables[0].Rows[0]["RevisedCommission"].ToString();
                                            txtRevisedIncentive.Text = txtOrgIncentive.Text;
                                            txtRevNetAmt.Text = (float.Parse(txtRevWeightChrgsPP.Text == "" ? "0" : txtRevWeightChrgsPP.Text) -
                                                float.Parse(txtRevCommission.Text == "" ? "0" : txtRevCommission.Text) -
                                                float.Parse(txtRevisedIncentive.Text == "" ? "0" : txtRevisedIncentive.Text)).ToString();
                                            txtRevOCDACC.Text = "0";
                                            txtRevOCDAPP.Text = ds.Tables[0].Rows[0]["RevisedOCDA"].ToString();
                                            txtRevOCDCCC.Text = "0";
                                            txtRevOCDCPP.Text = ds.Tables[0].Rows[0]["RevisedOCDC"].ToString();
                                            txtRevPayableAirline.Text = (float.Parse(txtRevNetAmt.Text == "" ? "0" : txtRevNetAmt.Text) +
                                                float.Parse(txtRevOCDCPP.Text == "" ? "0" : txtRevOCDCPP.Text) -
                                                float.Parse(txtRevOCDAPP.Text == "" ? "0" : txtRevOCDAPP.Text)).ToString();

                                        }
                                        else
                                        {
                                            //txtRevWeightChrgsCC.Enabled = true;
                                            //txtRevValChargesCC.Enabled = true;
                                            //txtRevOCDACC.Enabled = true;
                                            //txtRevOCDCCC.Enabled = true;
                                            txtRevWeightChrgsPP.Enabled = false;
                                            txtRevValChargesPP.Enabled = false;
                                            txtRevOCDAPP.Enabled = false;
                                            txtRevOCDCPP.Enabled = false;
                                            //txtRevCommission.Enabled = true;
                                            //txtRevisedIncentive.Enabled = true;
                                            txtRevWeightChrgsCC.Text = ds.Tables[0].Rows[0]["RevisedFreightRate"].ToString();
                                            txtRevWeightChrgsPP.Text = "0";
                                            txtRevValChargesPP.Text = "0";
                                            txtRevValChargesCC.Text = ds.Tables[0].Rows[0]["RevisedValCharges"].ToString();
                                            txtRevCommission.Text = ds.Tables[0].Rows[0]["RevisedCommission"].ToString();
                                            txtRevisedIncentive.Text = txtOrgIncentive.Text;
                                            txtRevNetAmt.Text = (float.Parse(txtRevWeightChrgsCC.Text) - float.Parse(txtRevCommission.Text) - float.Parse(txtRevisedIncentive.Text)).ToString();
                                            txtRevOCDACC.Text = ds.Tables[0].Rows[0]["RevisedOCDA"].ToString();
                                            txtRevOCDAPP.Text = "0";
                                            txtRevOCDCCC.Text = ds.Tables[0].Rows[0]["RevisedOCDC"].ToString();
                                            txtRevOCDCPP.Text = "0";
                                            txtRevPayableAirline.Text = (float.Parse(txtRevNetAmt.Text) + float.Parse(txtRevOCDCCC.Text) - float.Parse(txtRevOCDACC.Text)).ToString();


                                        }
                                        txtRevSTComm.Text = ds.Tables[0].Rows[0]["RevisedSTCommission"].ToString();
                                        txtOrgSTComm.Text = ds.Tables[0].Rows[0]["STCommission"].ToString();
                                        txtOrgServiceTax.Text = ds.Tables[0].Rows[0]["ServiceTax"].ToString();
                                        txtRevServiceTax.Text = ds.Tables[0].Rows[0]["RevisedServiceTax"].ToString();
                                        txtOrgTDSComm.Text = ds.Tables[0].Rows[0]["RevisedTDSCommission"].ToString();
                                        txtRevTDSComm.Text = ds.Tables[0].Rows[0]["TDSCommission"].ToString();
                                        txtOrgTotal.Text = ds.Tables[0].Rows[0]["CurrentTotal"].ToString();
                                        txtRevTotal.Text = ds.Tables[0].Rows[0]["RevisedTotal"].ToString();
                                        txtCommodityCodeRev.Text = ds.Tables[0].Rows[0]["CommodityCodeRev"].ToString();
                                        //txtCommodityCodeRev.Enabled = true;
                                        ST = Convert.ToDouble(ds.Tables[0].Rows[0]["ST"].ToString());
                                        CommissionPercent = float.Parse(ds.Tables[0].Rows[0]["CommissionPercent"].ToString());
                                        TDSOnComm = Convert.ToDouble(ds.Tables[0].Rows[0]["TDSOnCommissionPercent"].ToString());
                                        Rate = float.Parse(ds.Tables[0].Rows[0]["Rate"].ToString());
                                       // string strMode = Session["CCA_Mode"].ToString();
                                        if (Session["CCA_Mode"] != null && Session["CCA_Mode"].ToString() == "Edit")
                                        {
                                            btnAccept.Visible = false;
                                            btnReject.Visible = false;
                                            btnSave.Visible = true;
                                            btnCalculate.Visible = true;
                                            txtCommodityCodeRev.Enabled = true;
                                            txtRevisedWeight.Enabled = true;
                                            txtRevCommission.Enabled = true;
                                            txtRevisedIncentive.Enabled = true;
                                            txtRevNetAmt.Enabled = true;
                                            txtRevTDSComm.Enabled = true;
                                            txtRevSTComm.Enabled = true;
                                            txtRevPayableAirline.Enabled = true;
                                            txtRevServiceTax.Enabled = true;
                                            if (PMode == "PP")
                                            {
                                                txtRevOCDAPP.Enabled = true;
                                                txtRevOCDCPP.Enabled = true;
                                                txtRevOCDCCC.Enabled = false;
                                                txtRevOCDACC.Enabled = false;
                                            }
                                            else 
                                            {
                                                txtRevOCDAPP.Enabled = false;
                                                txtRevOCDCPP.Enabled = false;
                                                txtRevOCDCCC.Enabled = true;
                                                txtRevOCDACC.Enabled = true; 
                                            }
                                            //else if (Session["CCA_Mode"] != null && Session["CCA_Mode"].ToString() == "")
                                            //{
                                            //    txtRevOCDAPP.Enabled = false;
                                            //    txtRevOCDCPP.Enabled = false;
                                            //    txtRevOCDCCC.Enabled = true;
                                            //    txtRevOCDACC.Enabled = true;
                                            //}
                                        }
                                        else
                                        {
                                            if (Session["CCA_Mode"] != null && Session["CCA_Mode"].ToString() == "View")
                                            {
                                                btnAccept.Visible = false;
                                                btnReject.Visible = false;
                                                txtRemarksCorrection.Enabled = false;
                                            }
                                            else
                                            {
                                                btnAccept.Visible = true;
                                                btnReject.Visible = true;
                                            }
                                            btnCalculate.Visible = false;
                                            
                                            btnSave.Visible = false;
                                        }
                                        //txtRevisedWeight.Enabled = true;
                                        hdSelection.Value = "WorkFlow";
                                        btnList.Visible = false;
                                        //btnCalculate.Visible = false;

                                    }
                                }
                            }
                            //if (Session["CCA_Mode"] != null && Session["CCA_Mode"].ToString() == "")
                            //{
                            //    btnCalculate.Visible = false;
                            //    btnAccept.Visible = true;
                            //}

                        }
                        else
                        {
                            btnSave.Visible = true;
                            btnCalculate.Enabled = false;
                        }

                    }
                }
                catch (Exception ex)
                { }
            }
            //if (!IsPostBack)
            //{
            //    lblStatus.Text = "";
            //    txtDCMNumber.Text = "";
            //    CCADatatable();

            //    //Code to calculate taxes on text change of Freight, OCDC, OCDA
            //    string handlerPanelCalc = ClientScript.GetPostBackEventReference(this.btnTaxCalc, "");
            //    txtRevisedFreight.Attributes.Add("onblur", handlerPanelCalc);
            //    txtRevsedOCDC.Attributes.Add("onblur", handlerPanelCalc);
            //    txtRevisedOCDA.Attributes.Add("onblur", handlerPanelCalc);
            //    txtRevisedServiceTax.Attributes.Add("onblur", handlerPanelCalc);
            //    txtRevisedComm.Attributes.Add("onblur", handlerPanelCalc);

            //    string handlerCommChangeCalc = ClientScript.GetPostBackEventReference(this.btnCommChange, "");
            //    txtRevisedComm.Attributes.Add("onblur", handlerCommChangeCalc);
            //}
        }
        /// <summary>
        /// This function genrates Unique DCM Number which is used as a DCM Number
        /// </summary>
        /// <returns></returns>
        #region GetCCANumber
        protected string GetCCANumber()
        {
            try
            {
                DataSet Ds = new DataSet();
                Ds = objBAL.GetCCANumber();
                return Ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception)
            {

                return null;
            }

        }
        # endregion GetCCANumber
        /// <summary>
        /// This function gets data from billing and assign it to the rspective textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Fill Current & Revised Data
        //protected void btnList_Click(object sender, EventArgs e)
        //{
        //    if (!ValidateList())
        //        return;
        //    //this is to get the values from agent master 
        //    DataSet dsAgent = objBAL.GetDataFromAgent(txtAWB.Text, txtInvoiceNo.Text);
        //    if (dsAgent.Tables[0].Rows.Count > 0)
        //    {
        //        TDSOnComm = Convert.ToDouble(dsAgent.Tables[0].Rows[0]["TDSOnCommision"].ToString());
        //        NormalComm = Convert.ToDouble(dsAgent.Tables[0].Rows[0]["NorrmalComm"].ToString());
        //    }

        //    //ClearFields();
        //    string AWBNo, InvoiceNo;
        //    lblStatus.Text = "";
        //    txtDCMNumber.Text = "";
        //    DataSet ds = new DataSet();
        //    DataTable dt;
        //    try
        //    {
        //        AWBNo = txtAWB.Text;
        //        InvoiceNo = txtInvoiceNo.Text;
        //        ds = objBAL.FillCurrentCCA(AWBNo, InvoiceNo);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            dt = ds.Tables[0];
        //            txtCGrossWt.Text = dt.Rows[0][0].ToString();
        //            txtCCharWt.Text = dt.Rows[0][1].ToString();
        //            txtCFreight.Text = dt.Rows[0][2].ToString();
        //            txtCOCDC.Text = dt.Rows[0][3].ToString();
        //            txtCOCDA.Text = dt.Rows[0][4].ToString();
        //            txtCServiceTax.Text = dt.Rows[0][5].ToString();
        //            txtCCommission.Text = dt.Rows[0][6].ToString();
        //            txtCTDSOnComm.Text = dt.Rows[0][7].ToString();
        //            txtSTOnComm.Text = dt.Rows[0][8].ToString();
        //            txtCTot.Text = dt.Rows[0][9].ToString();
        //        }




        //    }
        //    catch (Exception)
        //    {

        //        //  throw;
        //    }
        //}
        #endregion Fill Current & Revised Data

        #region validation for list button
        //protected bool ValidateList()
        //{
        //    if (txtAWB.Text.Trim() == "" || txtInvoiceNo.Text.Trim() == "")
        //    {
        //        lblStatus.Text = "Please enter AWB number and Invoice number";
        //        lblStatus.ForeColor = Color.Blue;
        //        return false;

        //    }
        //    return true;
        //}
        #endregion validation for list button

        /// <summary>
        /// This is a datatable used to save to save data when user hits on a Save Button
        /// </summary>
        #region CCADatatable
        public void CCADatatable()
        {
            try
            {
                DataTable dtCCA = new DataTable();
                dtCCA.Columns.Add("AWBPrefix", typeof(string));
                dtCCA.Columns.Add("AWBNumber", typeof(string));
                dtCCA.Columns.Add("InvoiceNumber", typeof(string));
                dtCCA.Columns.Add("grossWt", typeof(double));
                dtCCA.Columns.Add("ChargbleWt", typeof(double));
                dtCCA.Columns.Add("FreightRate", typeof(double));
                dtCCA.Columns.Add("OCDC", typeof(double));
                dtCCA.Columns.Add("OCDA", typeof(double));
                dtCCA.Columns.Add("ServiceTax", typeof(double));
                dtCCA.Columns.Add("Commission", typeof(double));
                dtCCA.Columns.Add("TDSComm", typeof(double));
                dtCCA.Columns.Add("STComm", typeof(double));
                dtCCA.Columns.Add("Total", typeof(double));
                dtCCA.Columns.Add("RevisedgrossWt", typeof(double));
                dtCCA.Columns.Add("RevisedChargbleWt", typeof(double));
                dtCCA.Columns.Add("RevisedFreightRate", typeof(double));
                dtCCA.Columns.Add("RevisedOCDC", typeof(double));
                dtCCA.Columns.Add("RevisedOCDA", typeof(double));
                dtCCA.Columns.Add("RevisedServiceTax", typeof(double));
                dtCCA.Columns.Add("RevisedCommission", typeof(double));
                dtCCA.Columns.Add("RevisedTDSComm", typeof(double));
                dtCCA.Columns.Add("RevisedSTComm", typeof(double));
                dtCCA.Columns.Add("RevisedTotal", typeof(double));
                dtCCA.Columns.Add("Remarks", typeof(string));
                dtCCA.Columns.Add("Discount", typeof(double));
                dtCCA.Columns.Add("RevisedDiscount", typeof(double));
                dtCCA.Columns.Add("ValCharges", typeof(double));
                dtCCA.Columns.Add("RevisedValCharges", typeof(double));
                dtCCA.Columns.Add("Status", typeof(string));
                dtCCA.Columns.Add("CCANumber", typeof(string));
                dtCCA.Columns.Add("FlightNo", typeof(string));
                dtCCA.Columns.Add("FlightDate", typeof(string));
                dtCCA.Columns.Add("CommodityCodeOrg", typeof(string));
                dtCCA.Columns.Add("CommodityCodeRev", typeof(string));

                Session["CCA"] = dtCCA;
            }
            catch (Exception ex)
            { }

        }
        #endregion CCADatatable
        /// <summary>
        /// This method saves data in a table named debit credit processing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Save CCA Processing
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        double grossWt, ChargbleWt, FreightRate, OCDC, OCDA, ServiceTax, Commission, TDSComm, STComm, Total;
        //        double RevisedgrossWt, RevisedChargbleWt, RevisedFreightRate, RevisedOCDC, RevisedOCDA, RevisedServiceTax, RevisedCommission, RevisedTDSComm, RevisedSTComm, RevisedTotal;
        //        string AWBnumber, InvoiceNumber, remarks;


        //        AWBnumber = txtAWB.Text;
        //        InvoiceNumber = txtInvoiceNo.Text;
        //        grossWt = Convert.ToDouble(txtCGrossWt.Text);
        //        ChargbleWt = Convert.ToDouble(txtCCharWt.Text);
        //        FreightRate = Convert.ToDouble(txtCFreight.Text);
        //        OCDC = Convert.ToDouble(txtCOCDC.Text);
        //        OCDA = Convert.ToDouble(txtCOCDA.Text);
        //        ServiceTax = Convert.ToDouble(txtCServiceTax.Text);
        //        Commission = Convert.ToDouble(txtCCommission.Text);
        //        TDSComm = Convert.ToDouble(txtCTDSOnComm.Text);
        //        STComm = Convert.ToDouble(txtSTOnComm.Text);
        //        Total = Convert.ToDouble(txtCTot.Text);
        //        RevisedgrossWt = Convert.ToDouble(txtRevisedGrosswt.Text);
        //        RevisedChargbleWt = Convert.ToDouble(txtRevisedChargableWt.Text);
        //        RevisedFreightRate = Convert.ToDouble(txtRevisedFreight.Text);
        //        RevisedOCDC = Convert.ToDouble(txtRevsedOCDC.Text);
        //        RevisedOCDA = Convert.ToDouble(txtRevisedOCDA.Text);
        //        RevisedServiceTax = Convert.ToDouble(txtRevisedServiceTax.Text);
        //        RevisedCommission = Convert.ToDouble(txtRevisedComm.Text);
        //        RevisedTDSComm = Convert.ToDouble(txtRevisedTDSOnComm.Text);
        //        RevisedSTComm = Convert.ToDouble(txtRevisedSTOnComm.Text);
        //        RevisedTotal = Convert.ToDouble(txtRevisedTotal.Text);
        //        remarks = txtRemarks.Text.Trim();

        //        FinalCT = FinalCT + Total;
        //        FinalRT = FinalRT + RevisedTotal;

        //        DataTable dtCCA = (DataTable)Session["CCA"];
        //        DataRow dr = dtCCA.Rows.Add(AWBnumber, InvoiceNumber, grossWt, ChargbleWt, FreightRate, OCDC, OCDA, ServiceTax, Commission, TDSComm, STComm, Total, RevisedgrossWt, RevisedChargbleWt, RevisedFreightRate, RevisedOCDC, RevisedOCDA, RevisedServiceTax, RevisedCommission, RevisedTDSComm, RevisedSTComm, RevisedTotal, remarks);
        //        Session["CCA"] = dtCCA;

        //        if (dtCCA.Rows.Count > 0)
        //        {
        //            clear();
        //            lblStatus.Text = "AWB charges saved successfully";
        //            lblStatus.ForeColor = Color.Green;
        //            return;
        //        }
        //        else
        //        {
        //            lblStatus.Text = "AWB charges not saved please try again ";
        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }
        //        //     


        //    }
        //    catch (Exception Ex)
        //    {
        //        lblStatus.Text = "AWB charges not saved please try again ";
        //        lblStatus.ForeColor = Color.Red;
        //        return;
        //    }

        //}
        #endregion Save DCMProcessing
        /// <summary>
        /// When we hit on a generate dcm at that time data present in a Datatable that is saved in a table  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region GenrateCCA
        //protected void btnGenerateCCA_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        object[] objCCA = new object[24];

        //        //To check if AWB is saved before generating DCM.
        //        DataTable dtCCA = (DataTable)Session["CCA"];
        //        if (dtCCA.Rows.Count == 0)
        //        {
        //            lblStatus.Text = "No AWB's are saved to Generate CCA";

        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }
        //        else
        //        {
        //            for (int i = 0; i < dtCCA.Rows.Count; i++)
        //            {
        //                int j = 0;
        //                objCCA.SetValue(dtCCA.Rows[i]["AWBNumber"].ToString(), j);//1
        //                objCCA.SetValue(dtCCA.Rows[i]["InvoiceNumber"].ToString(), ++j);//2
        //                objCCA.SetValue(dtCCA.Rows[i]["grossWt"].ToString(), ++j);//3
        //                objCCA.SetValue(dtCCA.Rows[i]["ChargbleWt"].ToString(), ++j);//4
        //                objCCA.SetValue(dtCCA.Rows[i]["FreightRate"].ToString(), ++j);//5
        //                objCCA.SetValue(dtCCA.Rows[i]["OCDC"].ToString(), ++j);//6
        //                objCCA.SetValue(dtCCA.Rows[i]["OCDA"].ToString(), ++j);//7
        //                objCCA.SetValue(dtCCA.Rows[i]["ServiceTax"].ToString(), ++j);//8
        //                objCCA.SetValue(dtCCA.Rows[i]["Commission"].ToString(), ++j);//9
        //                objCCA.SetValue(dtCCA.Rows[i]["TDSComm"].ToString(), ++j);//10
        //                objCCA.SetValue(dtCCA.Rows[i]["STComm"].ToString(), ++j);//11
        //                objCCA.SetValue(dtCCA.Rows[i]["Total"].ToString(), ++j);//12
        //                objCCA.SetValue(dtCCA.Rows[i]["RevisedgrossWt"].ToString(), ++j);//13
        //                objCCA.SetValue(dtCCA.Rows[i]["RevisedChargbleWt"].ToString(), ++j);//14
        //                objCCA.SetValue(dtCCA.Rows[i]["RevisedFreightRate"].ToString(), ++j);//15
        //                objCCA.SetValue(dtCCA.Rows[i]["RevisedOCDC"].ToString(), ++j);//16
        //                objCCA.SetValue(dtCCA.Rows[i]["RevisedOCDA"].ToString(), ++j);//17
        //                objCCA.SetValue(dtCCA.Rows[i]["RevisedServiceTax"].ToString(), ++j);//18
        //                objCCA.SetValue(dtCCA.Rows[i]["RevisedCommission"].ToString(), ++j);//19
        //                objCCA.SetValue(dtCCA.Rows[i]["RevisedTDSComm"].ToString(), ++j);//20
        //                objCCA.SetValue(dtCCA.Rows[i]["RevisedSTComm"].ToString(), ++j);//21
        //                objCCA.SetValue(dtCCA.Rows[i]["RevisedTotal"].ToString(), ++j);//22
        //                objCCA.SetValue(dtCCA.Rows[i]["Remarks"].ToString(), ++j);//23
        //                string UserName = Session["UserName"].ToString();//24
        //                objCCA.SetValue(UserName, ++j);
        //                string res = "";
        //                res = objBAL.SaveCCAProcessing(objCCA);

        //                if (res != "error")
        //                {
        //                    txtDCMNumber.Text = res;
        //                    lblStatus.Text = "CCA generated successfully";
        //                    lblStatus.ForeColor = Color.Green;

        //                    //To show DCM amount and DCM type (Credit/Debit)
        //                    //double RevisedTotal = Convert.ToDouble(txtRevisedTotal.Text);
        //                    //double CurrentTotal = Convert.ToDouble(txtCTot.Text);

        //                    double RevisedTotal = FinalRT;
        //                    double CurrentTotal = FinalCT;
        //                    if (RevisedTotal > CurrentTotal)
        //                    {
        //                        txtCCAAmount.Text = Math.Round((RevisedTotal - CurrentTotal), 2).ToString();
        //                        txtCCAType.Text = "Debit";
        //                    }
        //                    else if (RevisedTotal < CurrentTotal)
        //                    {
        //                        txtCCAAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
        //                        txtCCAType.Text = "Credit";
        //                    }
        //                    else
        //                    {
        //                        txtCCAAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
        //                    }
        //                }
        //                else
        //                {
        //                    lblStatus.Text = res;
        //                    lblStatus.ForeColor = Color.Red;
        //                }

        //            }

        //            Session["CCA"] = null;

        //        }

        //        FinalCT = 0; FinalRT = 0;

        //    }
        //    catch (Exception ex)
        //    { }
        //}
        #endregion GenrateCCA
        #region clear
        //public void clear()
        //{
        //    try
        //    {
        //        txtCGrossWt.Text = "";
        //        txtAWB.Text = "";
        //        txtInvoiceNo.Text = "";
        //        txtCCharWt.Text = "";
        //        txtCFreight.Text = "";
        //        txtCOCDA.Text = "";
        //        txtCOCDC.Text = "";
        //        txtCServiceTax.Text = "";
        //        txtCCommission.Text = "";
        //        txtCTDSOnComm.Text = "";
        //        txtSTOnComm.Text = "";
        //        txtCTot.Text = "";
        //        txtRevisedGrosswt.Text = "";
        //        txtRevisedChargableWt.Text = "";
        //        txtRevisedFreight.Text = "";
        //        txtRevisedComm.Text = "";
        //        txtRevisedOCDA.Text = "";
        //        txtRevsedOCDC.Text = "";
        //        txtRevisedServiceTax.Text = "";
        //        txtRevisedSTOnComm.Text = "";
        //        txtRevisedTDSOnComm.Text = "";
        //        txtRevisedTotal.Text = "";
        //        txtDCMNumber.Text = "";
        //        lblStatus.Text = "";
        //        txtCCAAmount.Text = "";
        //        txtCCAType.Text = "";
        //        txtRemarks.Text = "";

        //    }
        //    catch (Exception ex)
        //    { }
        //}
        #endregion clear

        /// <summary>
        /// In case of getting  data from agent master mapping with agentCode from agentMaster and controlling locator code from billingawbinvoicematching
        /// all calculatin part reatedly is done in this block
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Calculation
        //protected void txtRevisedOCDA_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        if (txtAWB.Text == "" && txtInvoiceNo.Text == "")
        //        {
        //            lblStatus.Text = "Please provide awbnumner and invoicenumber";
        //            lblStatus.ForeColor = Color.Red;
        //            return;

        //        }
        //        string AWBno = txtAWB.Text;
        //        string InvoiceNo = txtInvoiceNo.Text;
        //        //this is to get the values from agent master 
        //        DataSet ds = objBAL.GetDataFromAgent(AWBno, InvoiceNo);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            TDSOnComm = Convert.ToDouble(ds.Tables[0].Rows[0]["TDSOnCommision"].ToString());
        //            NormalComm = Convert.ToDouble(ds.Tables[0].Rows[0]["NorrmalComm"].ToString());
        //        }
        //        //Formulae to calculate all fields
        //        ServiceTax = Math.Round(((Convert.ToDouble(txtRevisedFreight.Text) + Convert.ToDouble(txtRevsedOCDC.Text) + Convert.ToDouble(txtRevisedOCDA.Text)) * ST) / 100);
        //        commissionamt = Math.Round(((Convert.ToDouble(txtRevisedFreight.Text)) * NormalComm) / 100);
        //        STComm = Math.Round((commissionamt * ST) / 100);
        //        TDSOnComm = Math.Round(((commissionamt + STComm) * TDSOnComm) / 100);

        //        txtRevisedServiceTax.Text = Convert.ToString(ServiceTax);
        //        txtRevisedComm.Text = Convert.ToString(commissionamt);
        //        txtRevisedSTOnComm.Text = Convert.ToString(STComm);
        //        txtRevisedTDSOnComm.Text = Convert.ToString(TDSOnComm);


        //        //Total
        //        double RevisedTotal;
        //        RevisedTotal = Math.Round(Convert.ToDouble(txtRevisedFreight.Text) + Convert.ToDouble(txtRevsedOCDC.Text) + Convert.ToDouble(txtRevisedServiceTax.Text) + Convert.ToDouble(txtRevisedTDSOnComm.Text)) - (Convert.ToDouble(txtRevisedComm.Text) + ((Convert.ToDouble(txtRevisedComm.Text) * ST) / 100));
        //        //RevisedTotal = (Convert.ToDouble(txtRevisedChargableWt.Text) + Convert.ToDouble(txtRevisedComm.Text) +Convert.ToDouble( txtRevisedFreight.Text) + Convert.ToDouble(txtRevisedGrosswt.Text) + Convert.ToDouble(txtRevisedOCDA.Text) + Convert.ToDouble(txtRevisedServiceTax.Text )+ Convert.ToDouble(txtRevisedSTOnComm.Text) + Convert.ToDouble(txtRevisedTDSOnComm.Text) + Convert.ToDouble(txtRevsedOCDC.Text) + Convert.ToDouble(txtSTOnComm.Text));
        //        txtRevisedTotal.Text = Convert.ToString(RevisedTotal);

        //    }
        //    catch (Exception ex)
        //    { }
        //}
        #endregion Calculation
        /// <summary>
        /// Called a clear function which clears the textboxes and allow user to create new one
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ClearButton
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                //clear();
            }
            catch (Exception ex) { }
        }
        #endregion ClearButton

        protected void btnTaxCalc_Click(object sender, EventArgs e)
        {
            //CalcTaxOnTextExit();
        }

        //protected void CalcTaxOnTextExit()
        //{
        //    //Code to put zero if nothing is entered in textbox
        //    if (txtRevisedFreight.Text == "")
        //        txtRevisedFreight.Text = "0";
        //    if (txtRevisedOCDA.Text == "")
        //        txtRevisedOCDA.Text = "0";
        //    if (txtRevsedOCDC.Text == "")
        //        txtRevsedOCDC.Text = "0";
        //    if (txtRevisedServiceTax.Text == "")
        //        txtRevisedServiceTax.Text = "0";
        //    if (txtRevisedComm.Text == "")
        //        txtRevisedComm.Text = "0";

        //    try
        //    {
        //        if (Convert.ToDouble(txtRevisedFreight.Text.Trim()) < 0)
        //        {
        //            lblStatus.Text = "Enter valid revised Freight";
        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }
        //        else if (Convert.ToDouble(txtRevsedOCDC.Text.Trim()) < 0)
        //        {
        //            lblStatus.Text = "Enter valid revised OCDC";
        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }
        //        else if (Convert.ToDouble(txtRevisedOCDA.Text.Trim()) < 0)
        //        {
        //            lblStatus.Text = "Enter valid revised OCDA";
        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }
        //        else if (Convert.ToDouble(txtRevisedServiceTax.Text.Trim()) < 0)
        //        {
        //            lblStatus.Text = "Enter valid revised Service Tax";
        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }
        //        else if (Convert.ToDouble(txtRevisedComm.Text.Trim()) < 0)
        //        {
        //            lblStatus.Text = "Enter valid revised Commission";
        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }
        //        else
        //        {
        //            //Code to get calculate taxes on change of Freight, OCDC and OCDA
        //            txtRevisedServiceTax.Text = Math.Round((Convert.ToDouble(txtRevisedFreight.Text) + Convert.ToDouble(txtRevisedOCDA.Text) + Convert.ToDouble(txtRevsedOCDC.Text)) * ST / 100, 2).ToString();
        //            txtRevisedComm.Text = Math.Round((Convert.ToDouble(txtRevisedFreight.Text) * NormalComm) / 100, 2).ToString();
        //            txtRevisedSTOnComm.Text = Math.Round(Convert.ToDouble(txtRevisedComm.Text) * ST / 100, 2).ToString();
        //            txtRevisedTDSOnComm.Text = Math.Round((Convert.ToDouble(txtRevisedComm.Text) + Convert.ToDouble(txtRevisedSTOnComm.Text)) * TDSOnComm / 100, 2).ToString();
        //            txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txtRevisedFreight.Text) + Convert.ToDouble(txtRevsedOCDC.Text) + Convert.ToDouble(txtRevisedServiceTax.Text) + Convert.ToDouble(txtRevisedTDSOnComm.Text) - (Convert.ToDouble(txtRevisedComm.Text) + Convert.ToDouble(txtRevisedSTOnComm.Text)), 2).ToString();
        //        }

        //        double CurrentTotal = FinalCT + Convert.ToDouble(txtCTot.Text.Trim());
        //        double RevisedTotal = FinalRT + Convert.ToDouble(txtRevisedTotal.Text.Trim());

        //        if (RevisedTotal > CurrentTotal)
        //        {
        //            txtCCAAmount.Text = Math.Round((RevisedTotal - CurrentTotal), 2).ToString();
        //            txtCCAType.Text = "Debit";
        //        }
        //        else if (RevisedTotal < CurrentTotal)
        //        {
        //            txtCCAAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
        //            txtCCAType.Text = "Credit";
        //        }
        //        else
        //        {
        //            txtCCAAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
        //        }
        //    }
        //    catch
        //    {

        //        throw;
        //    }
        //}

        //protected void CalcTaxOnCommChange()
        //{
        //    //Code to put zero if nothing is entered in textbox
        //    if (txtRevisedFreight.Text == "")
        //        txtRevisedFreight.Text = "0";
        //    if (txtRevisedOCDA.Text == "")
        //        txtRevisedOCDA.Text = "0";
        //    if (txtRevsedOCDC.Text == "")
        //        txtRevsedOCDC.Text = "0";
        //    if (txtRevisedServiceTax.Text == "")
        //        txtRevisedServiceTax.Text = "0";
        //    if (txtRevisedComm.Text == "")
        //        txtRevisedComm.Text = "0";

        //    try
        //    {
        //        if (Convert.ToDouble(txtRevisedFreight.Text.Trim()) < 0)
        //        {
        //            lblStatus.Text = "Enter valid revised Freight";
        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }
        //        else if (Convert.ToDouble(txtRevsedOCDC.Text.Trim()) < 0)
        //        {
        //            lblStatus.Text = "Enter valid revised OCDC";
        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }
        //        else if (Convert.ToDouble(txtRevisedOCDA.Text.Trim()) < 0)
        //        {
        //            lblStatus.Text = "Enter valid revised OCDA";
        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }
        //        else if (Convert.ToDouble(txtRevisedServiceTax.Text.Trim()) < 0)
        //        {
        //            lblStatus.Text = "Enter valid revised Service Tax";
        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }
        //        else if (Convert.ToDouble(txtRevisedComm.Text.Trim()) < 0)
        //        {
        //            lblStatus.Text = "Enter valid revised Commission";
        //            lblStatus.ForeColor = Color.Red;
        //            return;
        //        }
        //        else
        //        {
        //            //Code to get calculate taxes on change of Freight, OCDC and OCDA
        //            txtRevisedSTOnComm.Text = Math.Round(Convert.ToDouble(txtRevisedComm.Text) * ST / 100, 2).ToString();
        //            txtRevisedTDSOnComm.Text = Math.Round((Convert.ToDouble(txtRevisedComm.Text) + Convert.ToDouble(txtRevisedSTOnComm.Text)) * TDSOnComm / 100, 2).ToString();
        //            txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txtRevisedFreight.Text) + Convert.ToDouble(txtRevsedOCDC.Text) + Convert.ToDouble(txtRevisedServiceTax.Text) + Convert.ToDouble(txtRevisedTDSOnComm.Text) - (Convert.ToDouble(txtRevisedComm.Text) + Convert.ToDouble(txtRevisedSTOnComm.Text)), 2).ToString();
        //        }

        //        double CurrentTotal = FinalCT + Convert.ToDouble(txtCTot.Text.Trim());
        //        double RevisedTotal = FinalRT + Convert.ToDouble(txtRevisedTotal.Text.Trim());

        //        if (RevisedTotal > CurrentTotal)
        //        {
        //            txtCCAAmount.Text = Math.Round((RevisedTotal - CurrentTotal), 2).ToString();
        //            txtCCAType.Text = "Debit";
        //        }
        //        else if (RevisedTotal < CurrentTotal)
        //        {
        //            txtCCAAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
        //            txtCCAType.Text = "Credit";
        //        }
        //        else
        //        {
        //            txtCCAAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
        //        }
        //    }
        //    catch
        //    {

        //        throw;
        //    }
        //}

        protected void btnCommChange_Click(object sender, EventArgs e)
        {
            //CalcTaxOnCommChange();
        }

        protected void txtAWB_TextChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            { }
        }

        protected void txtAWBNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";

                //DateTime dt;

                //try
                //{
                //    //dt = Convert.ToDateTime(txtInvoiceFrom.Text);
                //    //Change 03082012
                //    string day = txtFlightDate.Text.Substring(0, 2);
                //    string mon = txtFlightDate.Text.Substring(3, 2);
                //    string yr = txtFlightDate.Text.Substring(6, 4);
                //    strflightdate = yr + "-" + mon + "-" + day;
                //    dt = Convert.ToDateTime(strflightdate);
                //}
                //catch (Exception ex)
                //{
                //    lblStatus.Visible = true;
                //    lblStatus.Text = "Selected Date format invalid";
                //    lblStatus.ForeColor = Color.Red;
                //    return;
                //}

                #region Validate AWB
                string Result = isAWBInvoiced(txtAWBPrefix.Text.Trim() + "-" + txtAWBNo.Text.Trim());
                if (Result != "true")
                {
                    lblStatus.Text = Result;
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                #endregion

                string[] QueryNames = new string[1];
                object[] QueryValues = new object[1];
                SqlDbType[] QueryTypes = new SqlDbType[1];

                QueryNames[0] = "AWBNo";
                //QueryNames[1] = "FlightNo";
                //QueryNames[2] = "FlightDate";

                QueryValues[0] = txtAWBPrefix.Text.Trim() + txtAWBNo.Text.Trim();
                //QueryValues[1] = txtFlightPrefix.Text.Trim()+ txtFlightNo.Text.Trim();
                //QueryValues[2] = dt;

                QueryTypes[0] = SqlDbType.VarChar;
                //QueryTypes[1] = SqlDbType.VarChar;
                //QueryTypes[2] = SqlDbType.DateTime;

                DataSet ds = db.SelectRecords("spPopulateCCA", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtAWBNo.Text = ds.Tables[0].Rows[0]["AWBNumber"].ToString();
                            txtAWBPrefix.Text = ds.Tables[0].Rows[0]["AWBPrefix"].ToString();
                            //txtFlightDate.Text = ds.Tables[0].Rows[0]["FlightDate"].ToString();
                            //txtFlightNo.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString().Substring(2,ds.Tables[0].Rows[0]["FlightNo"].ToString().Trim().Length-2);
                            //txtFlightPrefix.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString().Substring(0,2);
                            txtStatus.Text = ds.Tables[0].Rows[0]["Status"].ToString();
                            txtAgentCode.Text = ds.Tables[0].Rows[0]["AgentCode"].ToString();
                            PayMode = ds.Tables[0].Rows[0]["PayMode"].ToString();
                            txtAirlineCode.Text = ds.Tables[0].Rows[0]["AirlineCode"].ToString();
                            txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNumber"].ToString();
                            txtInvoiceDate.Text = ds.Tables[0].Rows[0]["InvoiceDate"].ToString();
                            txtCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                            txtDestination.Text = ds.Tables[0].Rows[0]["Destination"].ToString();
                            txtOrigin.Text = ds.Tables[0].Rows[0]["Origin"].ToString();
                            txtOriginalWeight.Text = ds.Tables[0].Rows[0]["ChargableWt"].ToString();
                            txtOrgSTComm.Text = ds.Tables[0].Rows[0]["STCommissionAmt"].ToString();
                            txtRevSTComm.Text = "0";
                            txtOrgTDSComm.Text = ds.Tables[0].Rows[0]["TDSOnCommision"].ToString();
                            txtRevTDSComm.Text = "0";
                            txtOrgServiceTax.Text = ds.Tables[0].Rows[0]["ServiceTax"].ToString();
                            txtRevServiceTax.Text = "0";
                            txtOrgOCDACC.Text = ds.Tables[0].Rows[0]["OCDACC"].ToString();
                            txtOrgOCDAPP.Text = ds.Tables[0].Rows[0]["OCDAPP"].ToString();
                            txtOrgOCDCCC.Text = ds.Tables[0].Rows[0]["OCDCCC"].ToString();
                            txtOrgOCDCPP.Text = ds.Tables[0].Rows[0]["OCDCPP"].ToString();
                            txtOrgNetAmt.Text = ds.Tables[0].Rows[0]["NetAmount"].ToString();
                            txtOrgTotal.Text = ds.Tables[0].Rows[0]["Total"].ToString();
                            txtRevTotal.Text = "0";
                            txtCommodityCodeOrg.Text = ds.Tables[0].Rows[0]["CommodityCode"].ToString();
                            if (PayMode == "PP")
                            {

                                txtOrgPayableAirline.Text = ds.Tables[0].Rows[0]["OrgNetPayableAirline"].ToString();//(float.Parse(ds.Tables[0].Rows[0]["NetAmount"].ToString()) + float.Parse(txtOrgOCDCPP.Text) - float.Parse(txtOrgOCDAPP.Text)).ToString();

                            }
                            else
                            {
                                txtOrgPayableAirline.Text = ds.Tables[0].Rows[0]["OrgNetPayableAirline"].ToString();//(float.Parse(ds.Tables[0].Rows[0]["NetAmount"].ToString()) + float.Parse(txtOrgOCDCCC.Text) - float.Parse(txtOrgOCDACC.Text)).ToString();

                            }
                            txtOrgCommission.Text = ds.Tables[0].Rows[0]["Commission"].ToString();
                            txtOrgIncentive.Text = ds.Tables[0].Rows[0]["Discount"].ToString();
                            txtDateOfIssue.Text = ds.Tables[0].Rows[0]["AWBDate"].ToString();
                            string WeightIndicator = ds.Tables[0].Rows[0]["WeightIndicator"].ToString();
                            if (WeightIndicator == "K")
                            {
                                rdbKilo.Checked = true;
                            }
                            else
                            {
                                rdbPound.Checked = true;
                            }
                            txtOrgWgtChrgsPP.Text = ds.Tables[0].Rows[0]["WeightChargesPP"].ToString();
                            txtOrgWgtChrgsCC.Text = ds.Tables[0].Rows[0]["WeightChargesCC"].ToString();
                            txtOrgValChargesCC.Text = ds.Tables[0].Rows[0]["ValChgsCC"].ToString();
                            txtOrgValChargesPP.Text = ds.Tables[0].Rows[0]["ValChgsPP"].ToString();
                            Rate = float.Parse(ds.Tables[0].Rows[0]["Rate"].ToString());
                            CommissionPercent = float.Parse(ds.Tables[0].Rows[0]["CommissionPercent"].ToString());
                            TDSOnComm = Convert.ToDouble(ds.Tables[0].Rows[0]["TDSOnCommision"].ToString());
                            OrgSTComm = Convert.ToDouble(ds.Tables[0].Rows[0]["STCommissionAmt"].ToString());
                            ST = Convert.ToDouble(ds.Tables[0].Rows[0]["ST"].ToString());
                            OrgTotal = Convert.ToDouble(ds.Tables[0].Rows[0]["Total"].ToString());
                            OrgServiceTax = Convert.ToDouble(ds.Tables[0].Rows[0]["ServiceTax"].ToString());
                            OrgTDSOnCommissionAmt = Convert.ToDouble(ds.Tables[0].Rows[0]["TDSOnCommAmt"].ToString());
                            txtRevisedWeight.Enabled = true;
                            txtRevisedWeight.Text = "0";
                            if (PayMode == "PP")
                            {
                                txtRevWeightChrgsCC.Enabled = false;
                                txtRevValChargesCC.Enabled = false;
                                txtRevOCDACC.Enabled = false;
                                txtRevOCDCCC.Enabled = false;
                            }
                            else
                            {
                                txtRevWeightChrgsPP.Enabled = false;
                                txtRevValChargesPP.Enabled = false;
                                txtRevOCDAPP.Enabled = false;
                                txtRevOCDCPP.Enabled = false;
                            }

                            if (PayMode == "PP" || PayMode == "PX")
                            {
                                txtRevWeightChrgsPP.Text = "0";//(float.Parse(txtRevisedWeight.Text) * Rate).ToString();
                                txtRevWeightChrgsCC.Text = "0";
                                txtRevValChargesCC.Text = "0";
                                txtRevValChargesPP.Text = "0";//txtOrgValChargesPP.Text;
                                txtRevCommission.Text = "0";//((float.Parse(txtRevWeightChrgsPP.Text) * CommissionPercent) / 100).ToString();
                                txtRevisedIncentive.Text = "0";//txtOrgIncentive.Text;
                                txtRevNetAmt.Text = "0";//(float.Parse(txtRevWeightChrgsPP.Text) - float.Parse(txtRevCommission.Text) - float.Parse(txtRevisedIncentive.Text)).ToString();
                                txtRevOCDACC.Text = "0";//txtOrgOCDACC.Text;
                                txtRevOCDAPP.Text = "0";//txtOrgOCDAPP.Text;
                                txtRevOCDCCC.Text = "0";//txtOrgOCDCCC.Text;
                                txtRevOCDCPP.Text = "0";//txtOrgOCDCPP.Text;
                                txtRevPayableAirline.Text = "0";//(float.Parse(txtRevNetAmt.Text) + float.Parse(txtRevOCDCPP.Text) - float.Parse(txtRevOCDAPP.Text)).ToString();
                                txtRevCommission.Enabled = true;
                                txtRevisedIncentive.Enabled = true;
                                txtRevOCDAPP.Enabled = true;
                                txtRevOCDCPP.Enabled = true;
                                txtRevValChargesPP.Enabled = true;
                                txtRevWeightChrgsPP.Enabled = true;
                                //txtRevPayableAirline.Enabled = true;

                            }
                            else
                            {
                                txtRevWeightChrgsCC.Text = "0"; //(float.Parse(txtRevisedWeight.Text) * Rate).ToString();
                                txtRevWeightChrgsPP.Text = "0";
                                txtRevValChargesPP.Text = "0";
                                txtRevValChargesCC.Text = "0"; //txtOrgValChargesCC.Text;
                                txtRevCommission.Text = "0"; //((float.Parse(txtRevWeightChrgsCC.Text) * CommissionPercent) / 100).ToString();
                                txtRevisedIncentive.Text = "0"; //txtOrgIncentive.Text;
                                txtRevNetAmt.Text = "0"; //(float.Parse(txtRevWeightChrgsCC.Text) - float.Parse(txtRevCommission.Text) - float.Parse(txtRevisedIncentive.Text)).ToString();
                                txtRevOCDACC.Text = "0"; //txtOrgOCDACC.Text;
                                txtRevOCDAPP.Text = "0"; //txtOrgOCDAPP.Text;
                                txtRevOCDCCC.Text = "0"; //txtOrgOCDCCC.Text;
                                txtRevOCDCPP.Text = "0"; //txtOrgOCDCPP.Text;
                                txtRevPayableAirline.Text = "0"; //(float.Parse(txtRevNetAmt.Text) + float.Parse(txtRevOCDCCC.Text) - float.Parse(txtRevOCDACC.Text)).ToString();
                                txtRevCommission.Enabled = true;
                                txtRevisedIncentive.Enabled = true;
                                txtRevOCDACC.Enabled = true;
                                txtRevOCDCCC.Enabled = true;
                                txtRevValChargesCC.Enabled = true;
                                txtRevWeightChrgsCC.Enabled = true;
                                //txtRevPayableAirline.Enabled = true;

                            }
                            //txtRevNetAmt.Enabled = true;
                            btnCalculate.Visible = true;
                            btnSave.Visible = true;
                            //btnCalculate_Click(sender, e);
                            txtCommodityCodeRev.Enabled = true;
                            txtRevServiceTax.Enabled = true;
                            txtRevSTComm.Enabled = true;
                            txtRevTDSComm.Enabled = true;
                            txtRevTotal.Enabled = true;
                            txtRevPayableAirline.Enabled = true;
                            txtRevNetAmt.Enabled = true;


                        }
                        else
                        {
                            lblStatus.Text = "No Records found for the given search criteria!";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        #region Revised Weight text Changed
        protected void txtRevisedWeight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                float result;
                if (float.TryParse(txtRevisedWeight.Text, out result))
                {
                    if (PayMode == "PP" || PayMode == "PX")
                    {
                        txtRevWeightChrgsPP.Text = "0";//(float.Parse(txtRevisedWeight.Text) * Rate).ToString();
                        txtRevWeightChrgsCC.Text = "0";
                        txtRevValChargesCC.Text = "0";
                        txtRevValChargesPP.Text = "0";//txtOrgValChargesPP.Text;
                        txtRevCommission.Text = "0";//((float.Parse(txtRevWeightChrgsPP.Text) * CommissionPercent) / 100).ToString();
                        txtRevisedIncentive.Text = "0";//txtOrgIncentive.Text;
                        txtRevNetAmt.Text = "0";//(float.Parse(txtRevWeightChrgsPP.Text) - float.Parse(txtRevCommission.Text) - float.Parse(txtRevisedIncentive.Text)).ToString();
                        txtRevOCDACC.Text = "0";//txtOrgOCDACC.Text;
                        txtRevOCDAPP.Text = "0";//txtOrgOCDAPP.Text;
                        txtRevOCDCCC.Text = "0";//txtOrgOCDCCC.Text;
                        txtRevOCDCPP.Text = "0";//txtOrgOCDCPP.Text;
                        txtRevPayableAirline.Text = "0";//(float.Parse(txtRevNetAmt.Text) + float.Parse(txtRevOCDCPP.Text) - float.Parse(txtRevOCDAPP.Text)).ToString();
                        txtRevCommission.Enabled = true;
                        txtRevisedIncentive.Enabled = true;
                        txtRevOCDAPP.Enabled = true;
                        txtRevOCDCPP.Enabled = true;
                        txtRevValChargesPP.Enabled = true;
                        txtRevWeightChrgsPP.Enabled = true;
                        //txtRevPayableAirline.Enabled = true;

                    }
                    else
                    {
                        txtRevWeightChrgsCC.Text = "0"; //(float.Parse(txtRevisedWeight.Text) * Rate).ToString();
                        txtRevWeightChrgsPP.Text = "0";
                        txtRevValChargesPP.Text = "0";
                        txtRevValChargesCC.Text = "0"; //txtOrgValChargesCC.Text;
                        txtRevCommission.Text = "0"; //((float.Parse(txtRevWeightChrgsCC.Text) * CommissionPercent) / 100).ToString();
                        txtRevisedIncentive.Text = "0"; //txtOrgIncentive.Text;
                        txtRevNetAmt.Text = "0"; //(float.Parse(txtRevWeightChrgsCC.Text) - float.Parse(txtRevCommission.Text) - float.Parse(txtRevisedIncentive.Text)).ToString();
                        txtRevOCDACC.Text = "0"; //txtOrgOCDACC.Text;
                        txtRevOCDAPP.Text = "0"; //txtOrgOCDAPP.Text;
                        txtRevOCDCCC.Text = "0"; //txtOrgOCDCCC.Text;
                        txtRevOCDCPP.Text = "0"; //txtOrgOCDCPP.Text;
                        txtRevPayableAirline.Text = "0"; //(float.Parse(txtRevNetAmt.Text) + float.Parse(txtRevOCDCCC.Text) - float.Parse(txtRevOCDACC.Text)).ToString();
                        txtRevCommission.Enabled = true;
                        txtRevisedIncentive.Enabled = true;
                        txtRevOCDACC.Enabled = true;
                        txtRevOCDCCC.Enabled = true;
                        txtRevValChargesCC.Enabled = true;
                        txtRevWeightChrgsCC.Enabled = true;
                        txtRevTotal.Enabled = true;
                        txtRevSTComm.Enabled = true;
                        txtRevTDSComm.Enabled = true;
                        txtRevServiceTax.Enabled = true;
                        //txtRevPayableAirline.Enabled = true;

                    }
                    btnCalculate_Click(sender, e);
                    txtCommodityCodeRev.Enabled = true;
                }
                else
                {
                    lblStatus.Text = "Revised Weight not in proper numeric format !";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
                return;
            }

        }
        #endregion

        #region Button Calculate
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                //if (CheckBox1.Checked)
                //{


                TDSOnCommissionAmt = Convert.ToDouble(txtRevTDSComm.Text);//Math.Round(((float.Parse(txtRevCommission.Text) * float.Parse(TDSOnComm.ToString())) / 100),2);
                //if (PayMode == "PP")
                //{
                //}
                //else
                //{

                //}
                RevSTComm = Convert.ToDouble(txtRevSTComm.Text);//Math.Round(((float.Parse(txtRevCommission.Text) * ST) / 100),2);
                if (PayMode == "PP")
                {
                    ServiceTax = Convert.ToDouble(txtRevServiceTax.Text);//Math.Round((((float.Parse(txtRevWeightChrgsPP.Text) + float.Parse(txtRevOCDAPP.Text) + float.Parse(txtRevOCDCPP.Text)) * ST) / 100), 2);

                    txtRevNetAmt.Text = (Math.Round((float.Parse(txtRevWeightChrgsPP.Text) + float.Parse(txtRevValChargesPP.Text) - float.Parse(txtRevCommission.Text) - float.Parse(txtRevisedIncentive.Text)), 2)).ToString();

                    txtRevPayableAirline.Text = (Math.Round(float.Parse(txtRevNetAmt.Text) + float.Parse(txtRevOCDCPP.Text) - float.Parse(txtRevOCDAPP.Text), 2)).ToString();

                    RevTotal = Math.Round((float.Parse(txtRevWeightChrgsPP.Text) + float.Parse(txtRevOCDCPP.Text) + ServiceTax + TDSOnCommissionAmt) - (float.Parse(txtRevCommission.Text) + float.Parse(txtRevSTComm.Text)), 2);//((float.Parse(txtRevCommission.Text) * ST) / 100)), 2);
                    txtRevTotal.Text = RevTotal.ToString();

                }
                else
                {
                    ServiceTax = Convert.ToDouble(txtRevServiceTax.Text);//Math.Round((((float.Parse(txtRevWeightChrgsCC.Text) + float.Parse(txtRevOCDACC.Text) + float.Parse(txtRevOCDCCC.Text)) * ST) / 100), 2);

                    txtRevNetAmt.Text = (Math.Round((float.Parse(txtRevWeightChrgsCC.Text) + float.Parse(txtRevValChargesCC.Text) - float.Parse(txtRevCommission.Text) - float.Parse(txtRevisedIncentive.Text)), 2)).ToString();

                    txtRevPayableAirline.Text = (Math.Round(float.Parse(txtRevNetAmt.Text) + float.Parse(txtRevOCDCCC.Text) - float.Parse(txtRevOCDACC.Text), 2)).ToString();

                    RevTotal = Math.Round((float.Parse(txtRevWeightChrgsCC.Text) + float.Parse(txtRevOCDCCC.Text) + ServiceTax + TDSOnCommissionAmt) - (float.Parse(txtRevCommission.Text) + float.Parse(txtRevSTComm.Text)), 2);//((float.Parse(txtRevCommission.Text) * ST) / 100)), 2);
                    txtRevTotal.Text = RevTotal.ToString();
                }

                if (hdSelection.Value == "")
                {

                    btnSave.Visible = true;
                }
                else
                {
                    btnAccept.Visible = true;
                    btnReject.Visible = true;
                }
                //}
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Button Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                try
                {
                    lblStatus.Text = "";

                    double grossWt, ChargbleWt, FreightRate, OCDC, OCDA, OriginalServiceTax, Commission, TDSComm, STComm, Total;
                    double RevisedgrossWt, RevisedChargbleWt, RevisedFreightRate, RevisedOCDC, RevisedOCDA, RevisedServiceTax, RevisedCommission, RevisedTDSComm, RevisedSTComm, RevisedTotal, Discount, RevDiscount, ValCharges, RevisedValCharges;
                    string AWBnumber, InvoiceNumber, remarks, Status, CCANumber, FlightNo, AWBPrefix, OrgCommCode, RevCommCode;
                    //DateTime FltDate;
                    //try
                    //{
                    //    string day = txtFlightDate.Text.Substring(0, 2);
                    //    string mon = txtFlightDate.Text.Substring(3, 2);
                    //    string yr = txtFlightDate.Text.Substring(6, 4);
                    //    strflightdate = yr + "/" + mon + "/" + day;
                    //    FltDate = Convert.ToDateTime(strflightdate);
                    //}
                    //catch (Exception ex)
                    //{
                    //    lblStatus.Visible = true;
                    //    lblStatus.Text = "Selected Date format invalid";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return;
                    //}
                    AWBPrefix = txtAWBPrefix.Text.Trim();
                    AWBnumber = txtAWBNo.Text.Trim();
                    InvoiceNumber = txtInvoiceNo.Text.Trim();
                    FlightNo = txtFlightPrefix.Text.Trim() + txtFlightNo.Text.Trim();
                    grossWt = Convert.ToDouble(txtOriginalWeight.Text == "" ? "0" : txtOriginalWeight.Text);
                    ChargbleWt = Convert.ToDouble(txtOriginalWeight.Text == "" ? "0" : txtOriginalWeight.Text);
                    Status = "New";
                    CCANumber = "";

                    //btnCalculate_Click(sender, e);
                    if (PayMode == "PP")
                    {
                        FreightRate = Convert.ToDouble(txtOrgWgtChrgsPP.Text == "" ? "0" : txtOrgWgtChrgsPP.Text);
                    }
                    else
                    { FreightRate = Convert.ToDouble(txtOrgWgtChrgsCC.Text == "" ? "0" : txtOrgWgtChrgsCC.Text); }
                    if (PayMode == "PP")
                    {
                        OCDC = Convert.ToDouble(txtOrgOCDCPP.Text == "" ? "0" : txtOrgOCDCPP.Text);
                    }
                    else
                    {
                        OCDC = Convert.ToDouble(txtOrgOCDCCC.Text == "" ? "0" : txtOrgOCDCCC.Text);
                    }
                    if (PayMode == "PP")
                    {
                        OCDA = Convert.ToDouble(txtOrgOCDAPP.Text == "" ? "0" : txtOrgOCDAPP.Text);
                    }
                    else
                    {
                        OCDA = Convert.ToDouble(txtOrgOCDACC.Text == "" ? "0" : txtOrgOCDACC.Text);
                    }
                    //ServiceTax = Convert.ToDouble(txtCServiceTax.Text);
                    //Commission = Convert.ToDouble(txtCCommission.Text);
                    OriginalServiceTax = Convert.ToDouble(txtOrgServiceTax.Text == "" ? "0" : txtOrgServiceTax.Text);//OrgServiceTax;
                    Commission = Convert.ToDouble(txtOrgCommission.Text == "" ? "0" : txtOrgCommission.Text);
                    TDSComm = Convert.ToDouble(txtOrgTDSComm.Text == "" ? "0" : txtOrgTDSComm.Text);//OrgTDSOnCommissionAmt;
                    STComm = Convert.ToDouble(txtOrgSTComm.Text == "" ? "0" : txtOrgSTComm.Text);//OrgSTComm;
                    //TDSComm = Convert.ToDouble(txtCTDSOnComm.Text);
                    //STComm = Convert.ToDouble(txtSTOnComm.Text);
                    Total = Convert.ToDouble(txtOrgTotal.Text == "" ? "0" : txtOrgTotal.Text);//Convert.ToDouble(OrgTotal);
                    RevisedgrossWt = Convert.ToDouble(txtRevisedWeight.Text == "" ? "0" : txtRevisedWeight.Text);
                    RevisedChargbleWt = Convert.ToDouble(txtRevisedWeight.Text == "" ? "0" : txtRevisedWeight.Text);
                    if (PayMode == "PP")
                    {
                        RevisedFreightRate = Convert.ToDouble(txtRevWeightChrgsPP.Text == "" ? "0" : txtRevWeightChrgsPP.Text);
                    }
                    else
                    { RevisedFreightRate = Convert.ToDouble(txtRevWeightChrgsCC.Text == "" ? "0" : txtRevWeightChrgsCC.Text); }
                    //RevisedFreightRate = Convert.ToDouble(txtRevisedFreight.Text);

                    if (PayMode == "PP")
                    {
                        RevisedOCDC = Convert.ToDouble(txtRevOCDCPP.Text == "" ? "0" : txtRevOCDCPP.Text);
                    }
                    else
                    {
                        RevisedOCDC = Convert.ToDouble(txtRevOCDCCC.Text == "" ? "0" : txtRevOCDCCC.Text);
                    }
                    if (PayMode == "PP")
                    {
                        RevisedOCDA = Convert.ToDouble(txtRevOCDAPP.Text == "" ? "0" : txtRevOCDAPP.Text);
                    }
                    else
                    {
                        RevisedOCDA = Convert.ToDouble(txtRevOCDACC.Text == "" ? "0" : txtRevOCDACC.Text);
                    }
                    //RevisedOCDC = Convert.ToDouble(txtRevsedOCDC.Text);
                    //RevisedOCDA = Convert.ToDouble(txtRevisedOCDA.Text);
                    RevisedServiceTax = Convert.ToDouble(txtRevServiceTax.Text == "" ? "0" : txtRevServiceTax.Text);//Convert.ToDouble(ServiceTax);
                    RevisedCommission = Convert.ToDouble(txtRevCommission.Text == "" ? "0" : txtRevCommission.Text);
                    RevisedTDSComm = Convert.ToDouble(txtRevTDSComm.Text == "" ? "0" : txtRevTDSComm.Text);//Convert.ToDouble(TDSOnCommissionAmt);
                    RevisedSTComm = Convert.ToDouble(txtRevSTComm.Text == "" ? "0" : txtRevSTComm.Text);//Convert.ToDouble(RevSTComm);
                    RevisedTotal = Convert.ToDouble(txtRevTotal.Text == "" ? "0" : txtRevTotal.Text);//Convert.ToDouble(RevTotal);
                    remarks = txtRemarksCorrection.Text.Trim();
                    Discount = Convert.ToDouble(txtOrgIncentive.Text);
                    RevDiscount = Convert.ToDouble(txtRevisedIncentive.Text);
                    if (PayMode == "PP")
                    {
                        ValCharges = Convert.ToDouble(txtOrgValChargesPP.Text == "" ? "0" : txtOrgValChargesPP.Text);
                        RevisedValCharges = Convert.ToDouble(txtRevValChargesPP.Text == "" ? "0" : txtRevValChargesPP.Text);
                    }
                    else
                    {

                        ValCharges = Convert.ToDouble(txtOrgValChargesCC.Text == "" ? "0" : txtOrgValChargesCC.Text);
                        RevisedValCharges = Convert.ToDouble(txtRevValChargesCC.Text == "" ? "0" : txtRevValChargesCC.Text);
                    }


                    FinalCT = FinalCT + Total;
                    FinalRT = FinalRT + RevisedTotal;

                    OrgCommCode = txtCommodityCodeOrg.Text.Trim();
                    RevCommCode = txtCommodityCodeRev.Text.Trim();
                    DataTable dtCCA = (DataTable)Session["CCA"];
                    DataRow dr = dtCCA.Rows.Add(AWBPrefix, AWBnumber, InvoiceNumber, grossWt, ChargbleWt, FreightRate, OCDC, OCDA, OriginalServiceTax, Commission, TDSComm, STComm, Total, RevisedgrossWt, RevisedChargbleWt, RevisedFreightRate, RevisedOCDC, RevisedOCDA, RevisedServiceTax, RevisedCommission, RevisedTDSComm, RevisedSTComm, RevisedTotal, remarks, Discount, RevDiscount, ValCharges, RevisedValCharges, Status, CCANumber, FlightNo, "", OrgCommCode, RevCommCode); ;
                    Session["CCA"] = dtCCA;


                    object[] objCCA = new object[36];

                    //To check if AWB is saved before generating DCM.
                    //        DataTable dtCCA = (DataTable)Session["CCA"];

                    for (int i = 0; i < dtCCA.Rows.Count; i++)
                    {
                        int j = 0;
                        objCCA.SetValue(dtCCA.Rows[i]["AWBPrefix"].ToString(), j);
                        objCCA.SetValue(dtCCA.Rows[i]["AWBNumber"].ToString(), ++j);//1
                        objCCA.SetValue(dtCCA.Rows[i]["InvoiceNumber"].ToString(), ++j);//2
                        objCCA.SetValue(dtCCA.Rows[i]["grossWt"].ToString(), ++j);//3
                        objCCA.SetValue(dtCCA.Rows[i]["ChargbleWt"].ToString(), ++j);//4
                        objCCA.SetValue(dtCCA.Rows[i]["FreightRate"].ToString(), ++j);//5
                        objCCA.SetValue(dtCCA.Rows[i]["OCDC"].ToString(), ++j);//6
                        objCCA.SetValue(dtCCA.Rows[i]["OCDA"].ToString(), ++j);//7
                        objCCA.SetValue(dtCCA.Rows[i]["ServiceTax"].ToString(), ++j);//8
                        objCCA.SetValue(dtCCA.Rows[i]["Commission"].ToString(), ++j);//9
                        objCCA.SetValue(dtCCA.Rows[i]["TDSComm"].ToString(), ++j);//10
                        objCCA.SetValue(dtCCA.Rows[i]["STComm"].ToString(), ++j);//11
                        objCCA.SetValue(dtCCA.Rows[i]["Total"].ToString(), ++j);//12
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedgrossWt"].ToString(), ++j);//13
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedChargbleWt"].ToString(), ++j);//14
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedFreightRate"].ToString(), ++j);//15
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedOCDC"].ToString(), ++j);//16
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedOCDA"].ToString(), ++j);//17
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedServiceTax"].ToString(), ++j);//18
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedCommission"].ToString(), ++j);//19
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedTDSComm"].ToString(), ++j);//20
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedSTComm"].ToString(), ++j);//21
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedTotal"].ToString(), ++j);//22
                        objCCA.SetValue(dtCCA.Rows[i]["Remarks"].ToString(), ++j);//23
                        string UserName = Session["UserName"].ToString();//24
                        objCCA.SetValue(UserName, ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["Discount"].ToString(), ++j);//24
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedDiscount"].ToString(), ++j);//25
                        objCCA.SetValue(dtCCA.Rows[i]["ValCharges"].ToString(), ++j);//24
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedValCharges"].ToString(), ++j);//25
                        objCCA.SetValue(dtCCA.Rows[i]["Status"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["CCANumber"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["FlightNo"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["FlightDate"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["CommodityCodeOrg"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["CommodityCodeRev"].ToString(), ++j);
                        //Added  by Vijay
                        objCCA.SetValue(Convert.ToDateTime(Session["IT"]), ++j);
                        string res = "";
                        res = objBAL.SaveCCAProcessing(objCCA);

                        if (res != "error")
                        {
                            //txtNumber.Text = res;
                            DataSet ds = objBAL.GetCCADetails(res, txtAWBPrefix.Text.Trim() + txtAWBNo.Text.Trim());
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        txtAWBNo.Text = ds.Tables[0].Rows[0]["AWBNumber"].ToString();
                                        txtAWBPrefix.Text = ds.Tables[0].Rows[0]["AWBPrefix"].ToString();
                                        //txtFlightNo.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString().Substring(2, ds.Tables[0].Rows[0]["FlightNo"].ToString().Trim().Length - 2);
                                        //txtFlightPrefix.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString().Substring(0, 2);
                                        //txtFlightNo.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString();
                                        //txtFlightDate.Text = ds.Tables[0].Rows[0]["FlightDate"].ToString();
                                        txtStatus.Text = ds.Tables[0].Rows[0]["Status"].ToString();
                                        txtNumber.Text = ds.Tables[0].Rows[0]["CCANumber"].ToString();
                                        txtRemarksCorrection.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                                        string PMode = ds.Tables[0].Rows[0]["PayMode"].ToString();
                                        PayMode = PMode;
                                        txtAgentCode.Text = ds.Tables[0].Rows[0]["AgentCode"].ToString();
                                        txtAirlineCode.Text = ds.Tables[0].Rows[0]["AirlineCode"].ToString();
                                        txtCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                                        txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNumber"].ToString();
                                        txtInvoiceDate.Text = ds.Tables[0].Rows[0]["InvoiceDate"].ToString();
                                        txtCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                                        txtDestination.Text = ds.Tables[0].Rows[0]["Destination"].ToString();
                                        txtOrigin.Text = ds.Tables[0].Rows[0]["Origin"].ToString();
                                        txtOriginalWeight.Text = ds.Tables[0].Rows[0]["ChargableWeight"].ToString();
                                        txtOrgNetAmt.Text = ds.Tables[0].Rows[0]["NetAmount"].ToString();

                                        txtOrgCommission.Text = ds.Tables[0].Rows[0]["Commission"].ToString();
                                        txtOrgIncentive.Text = ds.Tables[0].Rows[0]["Discount"].ToString();
                                        txtDateOfIssue.Text = ds.Tables[0].Rows[0]["AWBDate"].ToString();
                                        string WeightIndicator = ds.Tables[0].Rows[0]["WeightIndicator"].ToString();
                                        if (WeightIndicator == "K")
                                        {
                                            rdbKilo.Checked = true;
                                        }
                                        else
                                        {
                                            rdbPound.Checked = true;
                                        }
                                        if (PMode == "PP")
                                        {
                                            txtOrgWgtChrgsPP.Text = ds.Tables[0].Rows[0]["FreightRate"].ToString();
                                            txtOrgWgtChrgsCC.Text = "0";
                                            txtOrgValChargesCC.Text = "0";
                                            txtOrgValChargesPP.Text = ds.Tables[0].Rows[0]["ValCharges"].ToString();
                                            txtOrgOCDAPP.Text = ds.Tables[0].Rows[0]["OCDA"].ToString();
                                            txtOrgOCDCPP.Text = ds.Tables[0].Rows[0]["OCDC"].ToString();
                                            txtOrgOCDACC.Text = "0";
                                            txtOrgOCDCCC.Text = "0";
                                        }
                                        else
                                        {
                                            txtOrgWgtChrgsCC.Text = ds.Tables[0].Rows[0]["FreightRate"].ToString();
                                            txtOrgValChargesCC.Text = ds.Tables[0].Rows[0]["ValCharges"].ToString();
                                            txtOrgWgtChrgsPP.Text = "0";
                                            txtOrgValChargesPP.Text = "0";
                                            txtOrgOCDAPP.Text = "0";
                                            txtOrgOCDCPP.Text = "0";
                                            txtOrgOCDACC.Text = ds.Tables[0].Rows[0]["OCDA"].ToString();
                                            txtOrgOCDCCC.Text = ds.Tables[0].Rows[0]["OCDC"].ToString();
                                        }
                                        if (PMode == "PP")
                                        {

                                            txtOrgPayableAirline.Text = ds.Tables[0].Rows[0]["OrgNetPayableAirline"].ToString();//(float.Parse(ds.Tables[0].Rows[0]["NetAmount"].ToString()) + float.Parse(txtOrgOCDCPP.Text) - float.Parse(txtOrgOCDAPP.Text)).ToString();

                                        }
                                        else
                                        {
                                            txtOrgPayableAirline.Text = ds.Tables[0].Rows[0]["OrgNetPayableAirline"].ToString();//(float.Parse(ds.Tables[0].Rows[0]["NetAmount"].ToString()) + float.Parse(txtOrgOCDCCC.Text) - float.Parse(txtOrgOCDACC.Text)).ToString();

                                        }
                                        txtRevisedWeight.Text = ds.Tables[0].Rows[0]["RevisedChargableWeight"].ToString();
                                        if (PMode == "PP")
                                        {
                                            txtRevWeightChrgsPP.Text = ds.Tables[0].Rows[0]["RevisedFreightRate"].ToString();
                                            txtRevWeightChrgsCC.Text = "0";
                                            txtRevValChargesCC.Text = "0";
                                            txtRevValChargesPP.Text = ds.Tables[0].Rows[0]["RevisedValCharges"].ToString();
                                            txtRevCommission.Text = ds.Tables[0].Rows[0]["RevisedCommission"].ToString();
                                            txtRevisedIncentive.Text = txtOrgIncentive.Text;
                                            txtRevNetAmt.Text = (float.Parse(txtRevWeightChrgsPP.Text == "" ? "0" : txtRevWeightChrgsPP.Text) -
                                                float.Parse(txtRevCommission.Text == "" ? "0" : txtRevCommission.Text) -
                                                float.Parse(txtRevisedIncentive.Text == "" ? "0" : txtRevisedIncentive.Text)).ToString();
                                            txtRevOCDACC.Text = "0";
                                            txtRevOCDAPP.Text = ds.Tables[0].Rows[0]["RevisedOCDA"].ToString();
                                            txtRevOCDCCC.Text = "0";
                                            txtRevOCDCPP.Text = ds.Tables[0].Rows[0]["RevisedOCDC"].ToString();
                                            txtRevPayableAirline.Text = (float.Parse(txtRevNetAmt.Text == "" ? "0" : txtRevNetAmt.Text) +
                                                float.Parse(txtRevOCDCPP.Text == "" ? "0" : txtRevOCDCPP.Text) -
                                                float.Parse(txtRevOCDAPP.Text == "" ? "0" : txtRevOCDAPP.Text)).ToString();

                                        }
                                        else
                                        {
                                            txtRevWeightChrgsCC.Text = ds.Tables[0].Rows[0]["RevisedFreightRate"].ToString();
                                            txtRevWeightChrgsPP.Text = "0";
                                            txtRevValChargesPP.Text = "0";
                                            txtRevValChargesCC.Text = ds.Tables[0].Rows[0]["RevisedValCharges"].ToString();
                                            txtRevCommission.Text = ds.Tables[0].Rows[0]["RevisedCommission"].ToString();
                                            txtRevisedIncentive.Text = txtOrgIncentive.Text;
                                            txtRevNetAmt.Text = (float.Parse(txtRevWeightChrgsCC.Text) - float.Parse(txtRevCommission.Text) - float.Parse(txtRevisedIncentive.Text)).ToString();
                                            txtRevOCDACC.Text = ds.Tables[0].Rows[0]["RevisedOCDA"].ToString();
                                            txtRevOCDAPP.Text = "0";
                                            txtRevOCDCCC.Text = ds.Tables[0].Rows[0]["RevisedOCDC"].ToString();
                                            txtRevOCDCPP.Text = "0";
                                            txtRevPayableAirline.Text = (float.Parse(txtRevNetAmt.Text) + float.Parse(txtRevOCDCCC.Text) - float.Parse(txtRevOCDACC.Text)).ToString();


                                        }

                                        txtRevSTComm.Text = ds.Tables[0].Rows[0]["RevisedSTCommission"].ToString();
                                        txtOrgSTComm.Text = ds.Tables[0].Rows[0]["STCommission"].ToString();
                                        txtOrgServiceTax.Text = ds.Tables[0].Rows[0]["ServiceTax"].ToString();
                                        txtRevServiceTax.Text = ds.Tables[0].Rows[0]["RevisedServiceTax"].ToString();
                                        txtOrgTDSComm.Text = ds.Tables[0].Rows[0]["RevisedTDSCommission"].ToString();
                                        txtRevTDSComm.Text = ds.Tables[0].Rows[0]["TDSCommission"].ToString();
                                        txtOrgTotal.Text = ds.Tables[0].Rows[0]["CurrentTotal"].ToString();
                                        txtRevTotal.Text = ds.Tables[0].Rows[0]["RevisedTotal"].ToString();
                                        txtCommodityCodeOrg.Text = ds.Tables[0].Rows[0]["CommodityCodeOrg"].ToString();
                                        txtCommodityCodeRev.Text = ds.Tables[0].Rows[0]["CommodityCodeRev"].ToString();
                                        ST = Convert.ToDouble(ds.Tables[0].Rows[0]["ST"].ToString());
                                        CommissionPercent = float.Parse(ds.Tables[0].Rows[0]["CommissionPercent"].ToString());
                                        TDSOnComm = Convert.ToDouble(ds.Tables[0].Rows[0]["TDSOnCommissionPercent"].ToString());

                                    }
                                }
                            }

                            //ClearControls();
                            lblStatus.Text = "Correction saved successfully";
                            lblStatus.ForeColor = Color.Green;
                        }
                        else
                        {
                            lblStatus.Text = "Correction could not be saved!! Please try again ";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }


                        //if (dtCCA.Rows.Count > 0)
                        //{
                        //    //clear();
                        //    lblStatus.Text = "AWB charges saved successfully";
                        //    lblStatus.ForeColor = Color.Green;
                        //    return;
                        //}
                        //else
                        //{
                        //    lblStatus.Text = "AWB charges not saved please try again ";
                        //    lblStatus.ForeColor = Color.Red;
                        //    return;
                        //}
                        //     



                    }
                }
                catch (Exception Ex)
                {
                    lblStatus.Text = "Correction could not be updated!! Please try again ";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }


            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region Button Accept
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                //object[] QueryValues = new object[4];
                //QueryValues[0] = txtNumber.Text.Trim();
                //QueryValues[1] = txtAWBNo.Text.Trim();
                //QueryValues[2] = "Accepted";
                //QueryValues[3] = txtInvoiceNo.Text.Trim();


                try
                {
                    lblStatus.Text = "";

                    double grossWt, ChargbleWt, FreightRate, OCDC, OCDA, OriginalServiceTax, Commission, TDSComm, STComm, Total;
                    double RevisedgrossWt, RevisedChargbleWt, RevisedFreightRate, RevisedOCDC, RevisedOCDA, RevisedServiceTax, RevisedCommission, RevisedTDSComm, RevisedSTComm, RevisedTotal, Discount, RevDiscount, ValCharges, RevisedValCharges;
                    string AWBnumber, InvoiceNumber, remarks, Status, CCANumber, FlightNo, AWBPrefix, OrgCommCode, RevCommCode;
                    //DateTime FltDate;
                    //try
                    //{
                    //    string day = txtFlightDate.Text.Substring(0, 2);
                    //    string mon = txtFlightDate.Text.Substring(3, 2);
                    //    string yr = txtFlightDate.Text.Substring(6, 4);
                    //    strflightdate = yr + "/" + mon + "/" + day;
                    //    FltDate = Convert.ToDateTime(strflightdate);
                    //}
                    //catch (Exception ex)
                    //{
                    //    lblStatus.Visible = true;
                    //    lblStatus.Text = "Selected Date format invalid";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return;
                    //}

                    //btnCalculate_Click(sender, e);
                    AWBPrefix = txtAWBPrefix.Text.Trim();
                    AWBnumber = txtAWBNo.Text.Trim();
                    FlightNo = txtFlightPrefix.Text.Trim() + txtFlightNo.Text.Trim();
                    InvoiceNumber = txtInvoiceNo.Text.Trim();
                    grossWt = Convert.ToDouble(txtOriginalWeight.Text);
                    ChargbleWt = Convert.ToDouble(txtOriginalWeight.Text);
                    Status = "Accepted";
                    CCANumber = txtNumber.Text.Trim();
                    if (PayMode == "PP")
                    {
                        FreightRate = Convert.ToDouble(txtOrgWgtChrgsPP.Text);
                    }
                    else
                    { FreightRate = Convert.ToDouble(txtOrgWgtChrgsCC.Text); }
                    if (PayMode == "PP")
                    {
                        OCDC = Convert.ToDouble(txtOrgOCDCPP.Text);
                    }
                    else
                    {
                        OCDC = Convert.ToDouble(txtOrgOCDCCC.Text);
                    }
                    if (PayMode == "PP")
                    {
                        OCDA = Convert.ToDouble(txtOrgOCDAPP.Text);
                    }
                    else
                    {
                        OCDA = Convert.ToDouble(txtOrgOCDACC.Text);
                    }
                    //ServiceTax = Convert.ToDouble(txtCServiceTax.Text);
                    //Commission = Convert.ToDouble(txtCCommission.Text);
                    OriginalServiceTax = Convert.ToDouble(txtOrgServiceTax.Text);//OrgServiceTax;
                    Commission = Convert.ToDouble(txtOrgCommission.Text);
                    TDSComm = Convert.ToDouble(txtOrgTDSComm.Text);//OrgTDSOnCommissionAmt;
                    STComm = Convert.ToDouble(txtOrgSTComm.Text);//OrgSTComm;
                    //TDSComm = Convert.ToDouble(txtCTDSOnComm.Text);
                    //STComm = Convert.ToDouble(txtSTOnComm.Text);
                    Total = Convert.ToDouble(txtOrgTotal.Text);//Convert.ToDouble(OrgTotal);
                    RevisedgrossWt = Convert.ToDouble(txtRevisedWeight.Text);
                    RevisedChargbleWt = Convert.ToDouble(txtRevisedWeight.Text);
                    if (PayMode == "PP")
                    {
                        RevisedFreightRate = Convert.ToDouble(txtRevWeightChrgsPP.Text);
                    }
                    else
                    { RevisedFreightRate = Convert.ToDouble(txtRevWeightChrgsCC.Text); }
                    //RevisedFreightRate = Convert.ToDouble(txtRevisedFreight.Text);

                    if (PayMode == "PP")
                    {
                        RevisedOCDC = Convert.ToDouble(txtRevOCDCPP.Text);
                    }
                    else
                    {
                        RevisedOCDC = Convert.ToDouble(txtRevOCDCCC.Text);
                    }
                    if (PayMode == "PP")
                    {
                        RevisedOCDA = Convert.ToDouble(txtRevOCDAPP.Text);
                    }
                    else
                    {
                        RevisedOCDA = Convert.ToDouble(txtRevOCDACC.Text);
                    }
                    //RevisedOCDC = Convert.ToDouble(txtRevsedOCDC.Text);
                    //RevisedOCDA = Convert.ToDouble(txtRevisedOCDA.Text);
                    RevisedServiceTax = Convert.ToDouble(txtRevServiceTax.Text);//Convert.ToDouble(ServiceTax);
                    RevisedCommission = Convert.ToDouble(txtRevCommission.Text);
                    RevisedTDSComm = Convert.ToDouble(txtRevTDSComm.Text);//Convert.ToDouble(TDSOnCommissionAmt);
                    RevisedSTComm = Convert.ToDouble(txtRevSTComm.Text);//Convert.ToDouble(RevSTComm);
                    RevisedTotal = Convert.ToDouble(txtRevTotal.Text);//Convert.ToDouble(RevTotal);
                    remarks = txtRemarksCorrection.Text.Trim();
                    Discount = Convert.ToDouble(txtOrgIncentive.Text);
                    RevDiscount = Convert.ToDouble(txtRevisedIncentive.Text);
                    if (PayMode == "PP")
                    {
                        ValCharges = Convert.ToDouble(txtOrgValChargesPP.Text);
                        RevisedValCharges = Convert.ToDouble(txtRevValChargesPP.Text);
                    }
                    else
                    {

                        ValCharges = Convert.ToDouble(txtOrgValChargesCC.Text);
                        RevisedValCharges = Convert.ToDouble(txtRevValChargesCC.Text);
                    }


                    FinalCT = FinalCT + Total;
                    FinalRT = FinalRT + RevisedTotal;
                    OrgCommCode = txtCommodityCodeOrg.Text.Trim();
                    RevCommCode = txtCommodityCodeRev.Text.Trim();
                    DataTable dtCCA = (DataTable)Session["CCA"];
                    DataRow dr = dtCCA.Rows.Add(AWBPrefix, AWBnumber, InvoiceNumber, grossWt, ChargbleWt, FreightRate, OCDC, OCDA, OriginalServiceTax, Commission, TDSComm, STComm, Total, RevisedgrossWt, RevisedChargbleWt, RevisedFreightRate, RevisedOCDC, RevisedOCDA, RevisedServiceTax, RevisedCommission, RevisedTDSComm, RevisedSTComm, RevisedTotal, remarks, Discount, RevDiscount, ValCharges, RevisedValCharges, Status, CCANumber, FlightNo, string.Empty, OrgCommCode, RevCommCode);
                    Session["CCA"] = dtCCA;


                    object[] objCCA = new object[36];

                    //To check if AWB is saved before generating DCM.
                    //        DataTable dtCCA = (DataTable)Session["CCA"];

                    for (int i = 0; i < dtCCA.Rows.Count; i++)
                    {
                        int j = 0;
                        objCCA.SetValue(dtCCA.Rows[i]["AWBPrefix"].ToString(), j);//1
                        objCCA.SetValue(dtCCA.Rows[i]["AWBNumber"].ToString(), ++j);//1
                        objCCA.SetValue(dtCCA.Rows[i]["InvoiceNumber"].ToString(), ++j);//2
                        objCCA.SetValue(dtCCA.Rows[i]["grossWt"].ToString(), ++j);//3
                        objCCA.SetValue(dtCCA.Rows[i]["ChargbleWt"].ToString(), ++j);//4
                        objCCA.SetValue(dtCCA.Rows[i]["FreightRate"].ToString(), ++j);//5
                        objCCA.SetValue(dtCCA.Rows[i]["OCDC"].ToString(), ++j);//6
                        objCCA.SetValue(dtCCA.Rows[i]["OCDA"].ToString(), ++j);//7
                        objCCA.SetValue(dtCCA.Rows[i]["ServiceTax"].ToString(), ++j);//8
                        objCCA.SetValue(dtCCA.Rows[i]["Commission"].ToString(), ++j);//9
                        objCCA.SetValue(dtCCA.Rows[i]["TDSComm"].ToString(), ++j);//10
                        objCCA.SetValue(dtCCA.Rows[i]["STComm"].ToString(), ++j);//11
                        objCCA.SetValue(dtCCA.Rows[i]["Total"].ToString(), ++j);//12
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedgrossWt"].ToString(), ++j);//13
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedChargbleWt"].ToString(), ++j);//14
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedFreightRate"].ToString(), ++j);//15
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedOCDC"].ToString(), ++j);//16
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedOCDA"].ToString(), ++j);//17
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedServiceTax"].ToString(), ++j);//18
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedCommission"].ToString(), ++j);//19
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedTDSComm"].ToString(), ++j);//20
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedSTComm"].ToString(), ++j);//21
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedTotal"].ToString(), ++j);//22
                        objCCA.SetValue(dtCCA.Rows[i]["Remarks"].ToString(), ++j);//23
                        string UserName = Session["UserName"].ToString();//24
                        objCCA.SetValue(UserName, ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["Discount"].ToString(), ++j);//24
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedDiscount"].ToString(), ++j);//25
                        objCCA.SetValue(dtCCA.Rows[i]["ValCharges"].ToString(), ++j);//24
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedValCharges"].ToString(), ++j);//25
                        objCCA.SetValue(dtCCA.Rows[i]["Status"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["CCANumber"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["FlightNo"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["FlightDate"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["CommodityCodeOrg"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["CommodityCodeRev"].ToString(), ++j);

                        //Added  by Vijay
                        objCCA.SetValue(Convert.ToDateTime(Session["IT"]), ++j);

                        string res = "";
                        res = objBAL.SaveCCAProcessing(objCCA);

                        if (res != "error")
                        {
                            // txtNumber.Text = res;
                            DataSet ds = objBAL.GetCCADetails(txtNumber.Text.Trim(), txtAWBPrefix.Text.Trim() + txtAWBNo.Text.Trim());
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        txtAWBNo.Enabled = false;
                                        txtFlightNo.Enabled = false;
                                        txtFlightDate.Enabled = false;
                                        txtAWBNo.Text = ds.Tables[0].Rows[0]["AWBNumber"].ToString();
                                        txtAWBPrefix.Text = ds.Tables[0].Rows[0]["AWBPrefix"].ToString();
                                        //txtFlightDate.Text = ds.Tables[0].Rows[0]["FlightDate"].ToString();
                                        //txtFlightNo.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString().Substring(2, ds.Tables[0].Rows[0]["FlightNo"].ToString().Trim().Length - 2);
                                        //txtFlightPrefix.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString().Substring(0, 2);
                                        //txtFlightNo.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString();
                                        txtStatus.Text = ds.Tables[0].Rows[0]["Status"].ToString();
                                        txtNumber.Text = ds.Tables[0].Rows[0]["CCANumber"].ToString();
                                        txtRemarksCorrection.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                                        string PMode = ds.Tables[0].Rows[0]["PayMode"].ToString();
                                        PayMode = PMode;
                                        txtAgentCode.Text = ds.Tables[0].Rows[0]["AgentCode"].ToString();
                                        txtAirlineCode.Text = ds.Tables[0].Rows[0]["AirlineCode"].ToString();
                                        txtCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                                        txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNumber"].ToString();
                                        txtInvoiceDate.Text = ds.Tables[0].Rows[0]["InvoiceDate"].ToString();
                                        txtCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                                        txtDestination.Text = ds.Tables[0].Rows[0]["Destination"].ToString();
                                        txtOrigin.Text = ds.Tables[0].Rows[0]["Origin"].ToString();
                                        txtOriginalWeight.Text = ds.Tables[0].Rows[0]["ChargableWeight"].ToString();
                                        txtOrgNetAmt.Text = ds.Tables[0].Rows[0]["NetAmount"].ToString();

                                        txtOrgCommission.Text = ds.Tables[0].Rows[0]["Commission"].ToString();
                                        txtOrgIncentive.Text = ds.Tables[0].Rows[0]["Discount"].ToString();
                                        txtDateOfIssue.Text = ds.Tables[0].Rows[0]["AWBDate"].ToString();
                                        string WeightIndicator = ds.Tables[0].Rows[0]["WeightIndicator"].ToString();
                                        if (WeightIndicator == "K")
                                        {
                                            rdbKilo.Checked = true;
                                        }
                                        else
                                        {
                                            rdbPound.Checked = true;
                                        }
                                        if (PMode == "PP")
                                        {
                                            txtOrgWgtChrgsPP.Text = ds.Tables[0].Rows[0]["FreightRate"].ToString();
                                            txtOrgWgtChrgsCC.Text = "0";
                                            txtOrgValChargesCC.Text = "0";
                                            txtOrgValChargesPP.Text = ds.Tables[0].Rows[0]["ValCharges"].ToString();
                                            txtOrgOCDAPP.Text = ds.Tables[0].Rows[0]["OCDA"].ToString();
                                            txtOrgOCDCPP.Text = ds.Tables[0].Rows[0]["OCDC"].ToString();
                                            txtOrgOCDACC.Text = "0";
                                            txtOrgOCDCCC.Text = "0";
                                        }
                                        else
                                        {
                                            txtOrgWgtChrgsCC.Text = ds.Tables[0].Rows[0]["FreightRate"].ToString();
                                            txtOrgValChargesCC.Text = ds.Tables[0].Rows[0]["ValCharges"].ToString();
                                            txtOrgWgtChrgsPP.Text = "0";
                                            txtOrgValChargesPP.Text = "0";
                                            txtOrgOCDAPP.Text = "0";
                                            txtOrgOCDCPP.Text = "0";
                                            txtOrgOCDACC.Text = ds.Tables[0].Rows[0]["OCDA"].ToString();
                                            txtOrgOCDCCC.Text = ds.Tables[0].Rows[0]["OCDC"].ToString();
                                        }
                                        if (PMode == "PP")
                                        {

                                            txtOrgPayableAirline.Text = ds.Tables[0].Rows[0]["OrgNetPayableAirline"].ToString();//(float.Parse(ds.Tables[0].Rows[0]["NetAmount"].ToString()) + float.Parse(txtOrgOCDCPP.Text) - float.Parse(txtOrgOCDAPP.Text)).ToString();

                                        }
                                        else
                                        {
                                            txtOrgPayableAirline.Text = ds.Tables[0].Rows[0]["OrgNetPayableAirline"].ToString();//(float.Parse(ds.Tables[0].Rows[0]["NetAmount"].ToString()) + float.Parse(txtOrgOCDCCC.Text) - float.Parse(txtOrgOCDACC.Text)).ToString();

                                        }
                                        txtRevisedWeight.Text = ds.Tables[0].Rows[0]["RevisedChargableWeight"].ToString();
                                        if (PMode == "PP")
                                        {
                                            txtRevWeightChrgsPP.Text = ds.Tables[0].Rows[0]["RevisedFreightRate"].ToString();
                                            txtRevWeightChrgsCC.Text = "0";
                                            txtRevValChargesCC.Text = "0";
                                            txtRevValChargesPP.Text = ds.Tables[0].Rows[0]["RevisedValCharges"].ToString();
                                            txtRevCommission.Text = ds.Tables[0].Rows[0]["RevisedCommission"].ToString();
                                            txtRevisedIncentive.Text = txtOrgIncentive.Text;
                                            txtRevNetAmt.Text = (float.Parse(txtRevWeightChrgsPP.Text) - float.Parse(txtRevCommission.Text) - float.Parse(txtRevisedIncentive.Text)).ToString();
                                            txtRevOCDACC.Text = "0";
                                            txtRevOCDAPP.Text = ds.Tables[0].Rows[0]["RevisedOCDA"].ToString();
                                            txtRevOCDCCC.Text = "0";
                                            txtRevOCDCPP.Text = ds.Tables[0].Rows[0]["RevisedOCDC"].ToString();
                                            txtRevPayableAirline.Text = (float.Parse(txtRevNetAmt.Text) + float.Parse(txtRevOCDCPP.Text) - float.Parse(txtRevOCDAPP.Text)).ToString();
                                            txtRevWeightChrgsCC.Enabled = false;
                                            txtRevValChargesCC.Enabled = false;
                                            txtRevOCDACC.Enabled = false;
                                            txtRevOCDCCC.Enabled = false;

                                        }
                                        else
                                        {
                                            txtRevWeightChrgsCC.Text = ds.Tables[0].Rows[0]["RevisedFreightRate"].ToString();
                                            txtRevWeightChrgsPP.Text = "0";
                                            txtRevValChargesPP.Text = "0";
                                            txtRevValChargesCC.Text = ds.Tables[0].Rows[0]["RevisedValCharges"].ToString();
                                            txtRevCommission.Text = ds.Tables[0].Rows[0]["RevisedCommission"].ToString();
                                            txtRevisedIncentive.Text = txtOrgIncentive.Text;
                                            txtRevNetAmt.Text = (float.Parse(txtRevWeightChrgsCC.Text) - float.Parse(txtRevCommission.Text) - float.Parse(txtRevisedIncentive.Text)).ToString();
                                            txtRevOCDACC.Text = ds.Tables[0].Rows[0]["RevisedOCDA"].ToString();
                                            txtRevOCDAPP.Text = "0";
                                            txtRevOCDCCC.Text = ds.Tables[0].Rows[0]["RevisedOCDC"].ToString();
                                            txtRevOCDCPP.Text = "0";
                                            txtRevPayableAirline.Text = (float.Parse(txtRevNetAmt.Text) + float.Parse(txtRevOCDCCC.Text) - float.Parse(txtRevOCDACC.Text)).ToString();
                                            txtRevWeightChrgsPP.Enabled = false;
                                            txtRevValChargesPP.Enabled = false;
                                            txtRevOCDAPP.Enabled = false;
                                            txtRevOCDCPP.Enabled = false;

                                        }
                                        txtRevSTComm.Text = ds.Tables[0].Rows[0]["RevisedSTCommission"].ToString();
                                        txtOrgSTComm.Text = ds.Tables[0].Rows[0]["STCommission"].ToString();
                                        txtOrgServiceTax.Text = ds.Tables[0].Rows[0]["ServiceTax"].ToString();
                                        txtRevServiceTax.Text = ds.Tables[0].Rows[0]["RevisedServiceTax"].ToString();
                                        txtOrgTDSComm.Text = ds.Tables[0].Rows[0]["RevisedTDSCommission"].ToString();
                                        txtRevTDSComm.Text = ds.Tables[0].Rows[0]["TDSCommission"].ToString();
                                        txtOrgTotal.Text = ds.Tables[0].Rows[0]["CurrentTotal"].ToString();
                                        txtRevTotal.Text = ds.Tables[0].Rows[0]["RevisedTotal"].ToString();
                                        txtCommodityCodeOrg.Text = ds.Tables[0].Rows[0]["CommodityCodeOrg"].ToString();
                                        txtCommodityCodeRev.Text = ds.Tables[0].Rows[0]["CommodityCodeRev"].ToString();
                                        ST = Convert.ToDouble(ds.Tables[0].Rows[0]["ST"].ToString());
                                        CommissionPercent = float.Parse(ds.Tables[0].Rows[0]["CommissionPercent"].ToString());
                                        TDSOnComm = Convert.ToDouble(ds.Tables[0].Rows[0]["TDSOnCommissionPercent"].ToString());
                                        //txtRevNetAmt.Enabled = true;
                                        //txtRevPayableAirline.Enabled = true;

                                    }
                                }
                            }
                            lblStatus.Text = "Correction accepted successfully";
                            lblStatus.ForeColor = Color.Green;
                        }
                        else
                        {
                            lblStatus.Text = "Correction could not be accepted!! Please try again ";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        //if (dtCCA.Rows.Count > 0)
                        //{
                        //    //clear();
                        //    lblStatus.Text = "AWB charges saved successfully";
                        //    lblStatus.ForeColor = Color.Green;
                        //    return;
                        //}
                        //else
                        //{
                        //    lblStatus.Text = "AWB charges not saved please try again ";
                        //    lblStatus.ForeColor = Color.Red;
                        //    return;
                        //}
                        //     



                    }
                }
                catch (Exception Ex)
                {
                    lblStatus.Text = "Correction could not be accepted!! Please try again ";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //if (objBAL.UpdateStatus(QueryValues))
                //{
                //    txtAWB_TextChanged(sender, e);
                //    lblStatus.Text = "Correction accepted successfully!";
                //    lblStatus.ForeColor = Color.Green;
                //}
                //else
                //{
                //    lblStatus.Text = "Correction acception failed! Please try again...";
                //    lblStatus.ForeColor = Color.Red;
                //}
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region Button Reject
        protected void btnReject_Click(object sender, EventArgs e)
        {
            //try
            //{

            //    object[] QueryValues = new object[4];
            //    QueryValues[0] = txtNumber.Text.Trim();
            //    QueryValues[1] = txtAWBNo.Text.Trim();
            //    QueryValues[2] = "Rejected";
            //    QueryValues[3] = txtInvoiceNo.Text.Trim();

            //    if (objBAL.UpdateStatus(QueryValues))
            //    {
            //        txtAWB_TextChanged(sender, e);
            //        lblStatus.Text = "Correction rejected successfully!";
            //        lblStatus.ForeColor = Color.Green;
            //    }
            //    else
            //    {
            //        lblStatus.Text = "Correction rejection failed! Please try again...";
            //        lblStatus.ForeColor = Color.Red;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    lblStatus.Text = ex.Message;
            //    lblStatus.ForeColor = Color.Red;
            //}


            try
            {
                lblStatus.Text = "";
                //object[] QueryValues = new object[4];
                //QueryValues[0] = txtNumber.Text.Trim();
                //QueryValues[1] = txtAWBNo.Text.Trim();
                //QueryValues[2] = "Accepted";
                //QueryValues[3] = txtInvoiceNo.Text.Trim();


                try
                {

                    double grossWt, ChargbleWt, FreightRate, OCDC, OCDA, OriginalServiceTax, Commission, TDSComm, STComm, Total;
                    double RevisedgrossWt, RevisedChargbleWt, RevisedFreightRate, RevisedOCDC, RevisedOCDA, RevisedServiceTax, RevisedCommission, RevisedTDSComm, RevisedSTComm, RevisedTotal, Discount, RevDiscount, ValCharges, RevisedValCharges;
                    string AWBnumber, InvoiceNumber, remarks, Status, CCANumber, FlightNo, OrgCommCode, RevCommCode;
                    //DateTime FltDate;
                    //try
                    //{
                    //    string day = txtFlightDate.Text.Substring(0, 2);
                    //    string mon = txtFlightDate.Text.Substring(3, 2);
                    //    string yr = txtFlightDate.Text.Substring(6, 4);
                    //    strflightdate = yr + "/" + mon + "/" + day;
                    //    FltDate = Convert.ToDateTime(strflightdate);
                    //}
                    //catch (Exception ex)
                    //{
                    //    lblStatus.Visible = true;
                    //    lblStatus.Text = "Selected Date format invalid";
                    //    lblStatus.ForeColor = Color.Red;
                    //    return;
                    //}


                    //btnCalculate_Click(sender, e);
                    AWBPrefix = txtAWBPrefix.Text.Trim();
                    AWBnumber = txtAWBNo.Text.Trim();
                    FlightNo = txtFlightPrefix.Text.Trim() + txtFlightNo.Text.Trim();
                    InvoiceNumber = txtInvoiceNo.Text;
                    grossWt = Convert.ToDouble(txtOriginalWeight.Text);
                    ChargbleWt = Convert.ToDouble(txtOriginalWeight.Text);
                    Status = "Rejected";
                    CCANumber = txtNumber.Text.Trim();
                    if (PayMode == "PP")
                    {
                        FreightRate = Convert.ToDouble(txtOrgWgtChrgsPP.Text);
                    }
                    else
                    { FreightRate = Convert.ToDouble(txtOrgWgtChrgsCC.Text); }
                    if (PayMode == "PP")
                    {
                        OCDC = Convert.ToDouble(txtOrgOCDCPP.Text);
                    }
                    else
                    {
                        OCDC = Convert.ToDouble(txtOrgOCDCCC.Text);
                    }
                    if (PayMode == "PP")
                    {
                        OCDA = Convert.ToDouble(txtOrgOCDAPP.Text);
                    }
                    else
                    {
                        OCDA = Convert.ToDouble(txtOrgOCDACC.Text);
                    }
                    //ServiceTax = Convert.ToDouble(txtCServiceTax.Text);
                    //Commission = Convert.ToDouble(txtCCommission.Text);
                    OriginalServiceTax = Convert.ToDouble(txtOrgServiceTax.Text);//OrgServiceTax;
                    Commission = Convert.ToDouble(txtOrgCommission.Text);
                    TDSComm = Convert.ToDouble(txtOrgTDSComm.Text);//OrgTDSOnCommissionAmt;
                    STComm = Convert.ToDouble(txtOrgSTComm.Text);//OrgSTComm;
                    //TDSComm = Convert.ToDouble(txtCTDSOnComm.Text);
                    //STComm = Convert.ToDouble(txtSTOnComm.Text);
                    Total = Convert.ToDouble(txtOrgTotal.Text);//Convert.ToDouble(OrgTotal);
                    RevisedgrossWt = Convert.ToDouble(txtRevisedWeight.Text);
                    RevisedChargbleWt = Convert.ToDouble(txtRevisedWeight.Text);
                    if (PayMode == "PP")
                    {
                        RevisedFreightRate = Convert.ToDouble(txtRevWeightChrgsPP.Text);
                    }
                    else
                    { RevisedFreightRate = Convert.ToDouble(txtRevWeightChrgsCC.Text); }
                    //RevisedFreightRate = Convert.ToDouble(txtRevisedFreight.Text);

                    if (PayMode == "PP")
                    {
                        RevisedOCDC = Convert.ToDouble(txtRevOCDCPP.Text);
                    }
                    else
                    {
                        RevisedOCDC = Convert.ToDouble(txtRevOCDCCC.Text);
                    }
                    if (PayMode == "PP")
                    {
                        RevisedOCDA = Convert.ToDouble(txtRevOCDAPP.Text);
                    }
                    else
                    {
                        RevisedOCDA = Convert.ToDouble(txtRevOCDACC.Text);
                    }
                    //RevisedOCDC = Convert.ToDouble(txtRevsedOCDC.Text);
                    //RevisedOCDA = Convert.ToDouble(txtRevisedOCDA.Text);
                    RevisedServiceTax = Convert.ToDouble(txtRevServiceTax.Text);//Convert.ToDouble(ServiceTax);
                    RevisedCommission = Convert.ToDouble(txtRevCommission.Text);
                    RevisedTDSComm = Convert.ToDouble(txtRevTDSComm.Text);//Convert.ToDouble(TDSOnCommissionAmt);
                    RevisedSTComm = Convert.ToDouble(txtRevSTComm.Text);//Convert.ToDouble(RevSTComm);
                    RevisedTotal = Convert.ToDouble(txtRevTotal.Text);//Convert.ToDouble(RevTotal);
                    remarks = txtRemarksCorrection.Text.Trim();
                    Discount = Convert.ToDouble(txtOrgIncentive.Text);
                    RevDiscount = Convert.ToDouble(txtRevisedIncentive.Text);
                    if (PayMode == "PP")
                    {
                        ValCharges = Convert.ToDouble(txtOrgValChargesPP.Text);
                        RevisedValCharges = Convert.ToDouble(txtRevValChargesPP.Text);
                    }
                    else
                    {

                        ValCharges = Convert.ToDouble(txtOrgValChargesCC.Text);
                        RevisedValCharges = Convert.ToDouble(txtRevValChargesCC.Text);
                    }
                    OrgCommCode = txtCommodityCodeOrg.Text.Trim();
                    RevCommCode = txtCommodityCodeRev.Text.Trim();

                    FinalCT = FinalCT + Total;
                    FinalRT = FinalRT + RevisedTotal;
                    DataTable dtCCA = (DataTable)Session["CCA"];
                    DataRow dr = dtCCA.Rows.Add(AWBPrefix, AWBnumber, InvoiceNumber, grossWt, ChargbleWt, FreightRate, OCDC, OCDA, OriginalServiceTax, Commission, TDSComm, STComm, Total, RevisedgrossWt, RevisedChargbleWt, RevisedFreightRate, RevisedOCDC, RevisedOCDA, RevisedServiceTax, RevisedCommission, RevisedTDSComm, RevisedSTComm, RevisedTotal, remarks, Discount, RevDiscount, ValCharges, RevisedValCharges, Status, CCANumber, FlightNo, string.Empty, OrgCommCode, RevCommCode);
                    Session["CCA"] = dtCCA;


                    object[] objCCA = new object[36];

                    //To check if AWB is saved before generating DCM.
                    //        DataTable dtCCA = (DataTable)Session["CCA"];

                    for (int i = 0; i < dtCCA.Rows.Count; i++)
                    {
                        int j = 0;
                        objCCA.SetValue(dtCCA.Rows[i]["AWBPrefix"].ToString(), j);
                        objCCA.SetValue(dtCCA.Rows[i]["AWBNumber"].ToString(), ++j);//1
                        objCCA.SetValue(dtCCA.Rows[i]["InvoiceNumber"].ToString(), ++j);//2
                        objCCA.SetValue(dtCCA.Rows[i]["grossWt"].ToString(), ++j);//3
                        objCCA.SetValue(dtCCA.Rows[i]["ChargbleWt"].ToString(), ++j);//4
                        objCCA.SetValue(dtCCA.Rows[i]["FreightRate"].ToString(), ++j);//5
                        objCCA.SetValue(dtCCA.Rows[i]["OCDC"].ToString(), ++j);//6
                        objCCA.SetValue(dtCCA.Rows[i]["OCDA"].ToString(), ++j);//7
                        objCCA.SetValue(dtCCA.Rows[i]["ServiceTax"].ToString(), ++j);//8
                        objCCA.SetValue(dtCCA.Rows[i]["Commission"].ToString(), ++j);//9
                        objCCA.SetValue(dtCCA.Rows[i]["TDSComm"].ToString(), ++j);//10
                        objCCA.SetValue(dtCCA.Rows[i]["STComm"].ToString(), ++j);//11
                        objCCA.SetValue(dtCCA.Rows[i]["Total"].ToString(), ++j);//12
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedgrossWt"].ToString(), ++j);//13
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedChargbleWt"].ToString(), ++j);//14
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedFreightRate"].ToString(), ++j);//15
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedOCDC"].ToString(), ++j);//16
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedOCDA"].ToString(), ++j);//17
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedServiceTax"].ToString(), ++j);//18
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedCommission"].ToString(), ++j);//19
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedTDSComm"].ToString(), ++j);//20
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedSTComm"].ToString(), ++j);//21
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedTotal"].ToString(), ++j);//22
                        objCCA.SetValue(dtCCA.Rows[i]["Remarks"].ToString(), ++j);//23
                        string UserName = Session["UserName"].ToString();//24
                        objCCA.SetValue(UserName, ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["Discount"].ToString(), ++j);//24
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedDiscount"].ToString(), ++j);//25
                        objCCA.SetValue(dtCCA.Rows[i]["ValCharges"].ToString(), ++j);//24
                        objCCA.SetValue(dtCCA.Rows[i]["RevisedValCharges"].ToString(), ++j);//25
                        objCCA.SetValue(dtCCA.Rows[i]["Status"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["CCANumber"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["FlightNo"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["FlightDate"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["CommodityCodeOrg"].ToString(), ++j);
                        objCCA.SetValue(dtCCA.Rows[i]["CommodityCodeRev"].ToString(), ++j);

                        //Added  by Vijay
                        objCCA.SetValue(Convert.ToDateTime(Session["IT"]), ++j);

                        string res = "";
                        res = objBAL.SaveCCAProcessing(objCCA);

                        if (res != "error")
                        {

                            //txtNumber.Text = res;
                            DataSet ds = objBAL.GetCCADetails(txtNumber.Text.Trim(), txtAWBPrefix.Text.Trim() + txtAWBNo.Text.Trim());
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        txtAWBNo.Enabled = false;
                                        txtFlightDate.Enabled = false;
                                        txtFlightNo.Enabled = false;
                                        txtAWBNo.Text = ds.Tables[0].Rows[0]["AWBNumber"].ToString();
                                        txtAWBPrefix.Text = ds.Tables[0].Rows[0]["AWBPrefix"].ToString();
                                        //txtFlightNo.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString().Substring(2, ds.Tables[0].Rows[0]["FlightNo"].ToString().Trim().Length - 2);
                                        //txtFlightPrefix.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString().Substring(0, 2);
                                        ////txtFlightNo.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString();
                                        //txtFlightDate.Text = ds.Tables[0].Rows[0]["FlightDate"].ToString();
                                        txtStatus.Text = ds.Tables[0].Rows[0]["Status"].ToString();
                                        txtNumber.Text = ds.Tables[0].Rows[0]["CCANumber"].ToString();
                                        txtRemarksCorrection.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                                        string PMode = ds.Tables[0].Rows[0]["PayMode"].ToString();
                                        PayMode = PMode;
                                        txtAgentCode.Text = ds.Tables[0].Rows[0]["AgentCode"].ToString();
                                        txtAirlineCode.Text = ds.Tables[0].Rows[0]["AirlineCode"].ToString();
                                        txtCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                                        txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNumber"].ToString();
                                        txtInvoiceDate.Text = ds.Tables[0].Rows[0]["InvoiceDate"].ToString();
                                        txtCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                                        txtDestination.Text = ds.Tables[0].Rows[0]["Destination"].ToString();
                                        txtOrigin.Text = ds.Tables[0].Rows[0]["Origin"].ToString();
                                        txtOriginalWeight.Text = ds.Tables[0].Rows[0]["ChargableWeight"].ToString();
                                        txtOrgNetAmt.Text = ds.Tables[0].Rows[0]["NetAmount"].ToString();

                                        txtOrgCommission.Text = ds.Tables[0].Rows[0]["Commission"].ToString();
                                        txtOrgIncentive.Text = ds.Tables[0].Rows[0]["Discount"].ToString();
                                        txtDateOfIssue.Text = ds.Tables[0].Rows[0]["AWBDate"].ToString();
                                        string WeightIndicator = ds.Tables[0].Rows[0]["WeightIndicator"].ToString();
                                        if (WeightIndicator == "K")
                                        {
                                            rdbKilo.Checked = true;
                                        }
                                        else
                                        {
                                            rdbPound.Checked = true;
                                        }
                                        if (PMode == "PP")
                                        {
                                            txtOrgWgtChrgsPP.Text = ds.Tables[0].Rows[0]["FreightRate"].ToString();
                                            txtOrgWgtChrgsCC.Text = "0";
                                            txtOrgValChargesCC.Text = "0";
                                            txtOrgValChargesPP.Text = ds.Tables[0].Rows[0]["ValCharges"].ToString();
                                            txtOrgOCDAPP.Text = ds.Tables[0].Rows[0]["OCDA"].ToString();
                                            txtOrgOCDCPP.Text = ds.Tables[0].Rows[0]["OCDC"].ToString();
                                            txtOrgOCDACC.Text = "0";
                                            txtOrgOCDCCC.Text = "0";
                                            txtRevWeightChrgsCC.Enabled = false;
                                            txtRevValChargesCC.Enabled = false;
                                            txtRevOCDACC.Enabled = false;
                                            txtRevOCDCCC.Enabled = false;
                                        }
                                        else
                                        {
                                            txtOrgWgtChrgsCC.Text = ds.Tables[0].Rows[0]["FreightRate"].ToString();
                                            txtOrgValChargesCC.Text = ds.Tables[0].Rows[0]["ValCharges"].ToString();
                                            txtOrgWgtChrgsPP.Text = "0";
                                            txtOrgValChargesPP.Text = "0";
                                            txtOrgOCDAPP.Text = "0";
                                            txtOrgOCDCPP.Text = "0";
                                            txtOrgOCDACC.Text = ds.Tables[0].Rows[0]["OCDA"].ToString();
                                            txtOrgOCDCCC.Text = ds.Tables[0].Rows[0]["OCDC"].ToString();
                                            txtRevWeightChrgsPP.Enabled = false;
                                            txtRevValChargesPP.Enabled = false;
                                            txtRevOCDAPP.Enabled = false;
                                            txtRevOCDCPP.Enabled = false;
                                        }
                                        if (PMode == "PP")
                                        {

                                            txtOrgPayableAirline.Text = ds.Tables[0].Rows[0]["OrgNetPayableAirline"].ToString();//(float.Parse(ds.Tables[0].Rows[0]["NetAmount"].ToString()) + float.Parse(txtOrgOCDCPP.Text) - float.Parse(txtOrgOCDAPP.Text)).ToString();

                                        }
                                        else
                                        {
                                            txtOrgPayableAirline.Text = ds.Tables[0].Rows[0]["OrgNetPayableAirline"].ToString();//(float.Parse(ds.Tables[0].Rows[0]["NetAmount"].ToString()) + float.Parse(txtOrgOCDCCC.Text) - float.Parse(txtOrgOCDACC.Text)).ToString();

                                        }
                                        txtRevisedWeight.Text = ds.Tables[0].Rows[0]["RevisedChargableWeight"].ToString();
                                        if (PMode == "PP")
                                        {
                                            txtRevWeightChrgsPP.Text = ds.Tables[0].Rows[0]["RevisedFreightRate"].ToString();
                                            txtRevWeightChrgsCC.Text = "0";
                                            txtRevValChargesCC.Text = "0";
                                            txtRevValChargesPP.Text = ds.Tables[0].Rows[0]["RevisedValCharges"].ToString();
                                            txtRevCommission.Text = ds.Tables[0].Rows[0]["RevisedCommission"].ToString();
                                            txtRevisedIncentive.Text = txtOrgIncentive.Text;
                                            txtRevNetAmt.Text = (float.Parse(txtRevWeightChrgsPP.Text) - float.Parse(txtRevCommission.Text) - float.Parse(txtRevisedIncentive.Text)).ToString();
                                            txtRevOCDACC.Text = "0";
                                            txtRevOCDAPP.Text = ds.Tables[0].Rows[0]["RevisedOCDA"].ToString();
                                            txtRevOCDCCC.Text = "0";
                                            txtRevOCDCPP.Text = ds.Tables[0].Rows[0]["RevisedOCDC"].ToString();
                                            txtRevPayableAirline.Text = (float.Parse(txtRevNetAmt.Text) + float.Parse(txtRevOCDCPP.Text) - float.Parse(txtRevOCDAPP.Text)).ToString();

                                        }
                                        else
                                        {
                                            txtRevWeightChrgsCC.Text = ds.Tables[0].Rows[0]["RevisedFreightRate"].ToString();
                                            txtRevWeightChrgsPP.Text = "0";
                                            txtRevValChargesPP.Text = "0";
                                            txtRevValChargesCC.Text = ds.Tables[0].Rows[0]["RevisedValCharges"].ToString();
                                            txtRevCommission.Text = ds.Tables[0].Rows[0]["RevisedCommission"].ToString();
                                            txtRevisedIncentive.Text = txtOrgIncentive.Text;
                                            txtRevNetAmt.Text = (float.Parse(txtRevWeightChrgsCC.Text) - float.Parse(txtRevCommission.Text) - float.Parse(txtRevisedIncentive.Text)).ToString();
                                            txtRevOCDACC.Text = ds.Tables[0].Rows[0]["RevisedOCDA"].ToString();
                                            txtRevOCDAPP.Text = "0";
                                            txtRevOCDCCC.Text = ds.Tables[0].Rows[0]["RevisedOCDC"].ToString();
                                            txtRevOCDCPP.Text = "0";
                                            txtRevPayableAirline.Text = (float.Parse(txtRevNetAmt.Text) + float.Parse(txtRevOCDCCC.Text) - float.Parse(txtRevOCDACC.Text)).ToString();


                                        }
                                        txtRevSTComm.Text = ds.Tables[0].Rows[0]["RevisedSTCommission"].ToString();
                                        txtOrgSTComm.Text = ds.Tables[0].Rows[0]["STCommission"].ToString();
                                        txtOrgServiceTax.Text = ds.Tables[0].Rows[0]["ServiceTax"].ToString();
                                        txtRevServiceTax.Text = ds.Tables[0].Rows[0]["RevisedServiceTax"].ToString();
                                        txtOrgTDSComm.Text = ds.Tables[0].Rows[0]["RevisedTDSCommission"].ToString();
                                        txtRevTDSComm.Text = ds.Tables[0].Rows[0]["TDSCommission"].ToString();
                                        txtOrgTotal.Text = ds.Tables[0].Rows[0]["CurrentTotal"].ToString();
                                        txtRevTotal.Text = ds.Tables[0].Rows[0]["RevisedTotal"].ToString();
                                        txtCommodityCodeOrg.Text = ds.Tables[0].Rows[0]["CommodityCodeOrg"].ToString();
                                        txtCommodityCodeRev.Text = ds.Tables[0].Rows[0]["CommodityCodeRev"].ToString();
                                        ST = Convert.ToDouble(ds.Tables[0].Rows[0]["ST"].ToString());
                                        CommissionPercent = float.Parse(ds.Tables[0].Rows[0]["CommissionPercent"].ToString());
                                        TDSOnComm = Convert.ToDouble(ds.Tables[0].Rows[0]["TDSOnCommissionPercent"].ToString());
                                        //txtRevNetAmt.Enabled = true;
                                        //txtRevPayableAirline.Enabled = true;
                                    }
                                }
                            }
                            lblStatus.Text = "Correction rejected successfully";
                            lblStatus.ForeColor = Color.Green;
                        }
                        else
                        {
                            lblStatus.Text = "Correction could not be rejected!! Please try again ";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        //if (dtCCA.Rows.Count > 0)
                        //{
                        //    //clear();
                        //    lblStatus.Text = "AWB charges saved successfully";
                        //    lblStatus.ForeColor = Color.Green;
                        //    return;
                        //}
                        //else
                        //{
                        //    lblStatus.Text = "AWB charges not saved please try again ";
                        //    lblStatus.ForeColor = Color.Red;
                        //    return;
                        //}
                        //     



                    }
                }
                catch (Exception Ex)
                {
                    lblStatus.Text = "Correction could not be rejected!! Please try again ";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                //if (objBAL.UpdateStatus(QueryValues))
                //{
                //    txtAWB_TextChanged(sender, e);
                //    lblStatus.Text = "Correction accepted successfully!";
                //    lblStatus.ForeColor = Color.Green;
                //}
                //else
                //{
                //    lblStatus.Text = "Correction acception failed! Please try again...";
                //    lblStatus.ForeColor = Color.Red;
                //}
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region Button List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                if (txtAWBPrefix.Text.Trim() == "")
                {
                    lblStatus.Text = "Please enter the AWB No !";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                if (txtAWBNo.Text.Trim() == "")
                {
                    lblStatus.Text = "Please enter the AWB No !";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                //if (txtFlightPrefix.Text.Trim() == "")
                //{
                //    lblStatus.Text = "Please enter the Flight Prefix !";
                //    lblStatus.ForeColor = Color.Red;
                //    return;
                //}
                //if (txtFlightNo.Text.Trim() == "")
                //{
                //    lblStatus.Text = "Please enter the Flight No !";
                //    lblStatus.ForeColor = Color.Red;
                //    return;
                //}
                //if (txtFlightDate.Text.Trim() == "")
                //{
                //    lblStatus.Text = "Please enter the Flight Date !";
                //    lblStatus.ForeColor = Color.Red;
                //    return;
                //}
                txtAWBNo_TextChanged(sender, e);
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region Clear Function
        private void ClearControls()
        {
            foreach (Control c in this.Form.Controls)
            {
                foreach (Control ctrl in c.Controls)
                {
                    if (ctrl is TextBox)
                    {
                        ((TextBox)ctrl).Text = string.Empty;
                    }
                    if (ctrl is DropDownList)
                    {
                        ((DropDownList)ctrl).SelectedIndex = 0;
                    }
                }
            }
            btnAccept.Visible = false;
            btnReject.Visible = false;
            btnSave.Visible = false;
            btnCalculate.Visible = false;
        }
        #endregion

        #region btnClear_Click
        protected void btnClear_Click1(object sender, EventArgs e)
        {
            try
            {
                Server.Transfer("~\\frmCCAProcessing.aspx");
            }
            catch (Exception)
            {
            }
        }
        #endregion btnClear_Click

        #region Validate if AWB is Invoiced
        public string isAWBInvoiced(string AWBNumber)
        {
            try
            {
                DataSet ds = db.SelectRecords("spIsAWBInvoiced", "AWBNumber", AWBNumber, SqlDbType.VarChar);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        return "true";
                    }
                    else
                        return "Please close the invoice to generate CCA on AWB.";
                }
                else
                    return "AWB not found!";
            }
            catch (Exception ex)
            { return ex.Message; }
        }
        #endregion

    }
}
