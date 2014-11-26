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

namespace ProjectSmartCargoManager
{
    public partial class DCMGenerate : System.Web.UI.Page
    {
        DCMGenerateBAL objBAL = new DCMGenerateBAL();
        MasterBAL objBal = new MasterBAL();
        string AWBPrefix = string.Empty;
        Double ST = 0.0;
        string flightDate, currentFlightDate, revisedFlightDate;
        double ServiceTax, commissionamt, STComm;
        static double TDSOnComm; static double NormalComm;
        static double FinalCT = 0; static double FinalRT = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["ST"] != null)
            //    ST = Convert.ToDouble(Session["ST"].ToString());
            //else
            //{
            //    Session["ST"] = objBal.getServiceTax();
            //    ST = Convert.ToDouble(Session["ST"].ToString());
            //}

            if (!IsPostBack)
            {
                if (Session["awbPrefix"] != null)
                    AWBPrefix = Session["awbPrefix"].ToString();
                else
                {
                    Session["awbPrefix"] = objBal.awbPrefix();
                    AWBPrefix = Session["awbPrefix"].ToString();
                }
                txtPreAWB.Text = AWBPrefix;

                //Set default flight prefix if any.
                if (Session["AirlinePrefix"] != null)
                    txtFlightPrefix.Text = Session["AirlinePrefix"].ToString();

                lblStatus.Text = "";
                txtDCMNumber.Text = "";
                DCMDatatable();

                //Code to calculate taxes on text change of Freight, OCDC, OCDA
                string handlerPanelCalc = ClientScript.GetPostBackEventReference(this.btnTaxCalc, "");
                txtRevisedFreight.Attributes.Add("onblur", handlerPanelCalc);
                txtRevsedOCDC.Attributes.Add("onblur", handlerPanelCalc);
                txtRevisedOCDA.Attributes.Add("onblur", handlerPanelCalc);
                //txtRevisedServiceTax.Attributes.Add("onblur", handlerPanelCalc);
                txtRevisedComm.Attributes.Add("onblur", handlerPanelCalc);

                string handlerSTChangeCalc = ClientScript.GetPostBackEventReference(this.btnSTChange, "");
                txtRevisedServiceTax.Attributes.Add("onblur", handlerSTChangeCalc);

                string handlerCommChangeCalc = ClientScript.GetPostBackEventReference(this.btnCommChange, "");
                txtRevisedComm.Attributes.Add("onblur", handlerCommChangeCalc);

                string handlerSTOnCommChangeCalc = ClientScript.GetPostBackEventReference(this.btnSTOnCommChange, "");
                txtRevisedSTOnComm.Attributes.Add("onblur", handlerSTOnCommChangeCalc);

                string handlerTDSOnCommChangeCalc = ClientScript.GetPostBackEventReference(this.btnTDSOnCommChange, "");
                txtRevisedTDSOnComm.Attributes.Add("onblur", handlerTDSOnCommChangeCalc);
                Session["Mode"] = "";
                Session["DCMNumber"] = "";
                string handlerAWBNumberChange = ClientScript.GetPostBackEventReference(this.btnAWBNumberChange, "");
                txtAWB.Attributes.Add("onblur", handlerAWBNumberChange);
                if (Request.QueryString["Mode"] != null && Request.QueryString["DCMNumber"] != null &&
                    Request.QueryString["Mode"].ToString().Length > 0)//line indicates it come from another page for edit or View
                {
                    //btnList_Click(sender,e);
                    Session["Mode"] = Request.QueryString["Mode"].ToString();
                    Session["DCMNumber"] = Request.QueryString["DCMNumber"].ToString();
                    Session["AWBPre"] = Request.QueryString["AWBPre"].ToString();
                    Session["AWBNumber"] = Request.QueryString["AWBNumber"].ToString();

                    DataSet ds = objBAL.FillDCMDetails(Session["DCMNumber"].ToString(), Session["AWBPre"].ToString(), Session["AWBNumber"].ToString());
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["DCMType"].ToString() == "A")
                        {
                            txtDCMNumber.Text = ds.Tables[0].Rows[0]["DCMNumber"].ToString();
                            txtDCMType.Text = ds.Tables[0].Rows[0]["DCMType"].ToString();

                            txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNumber"].ToString();
                            txtPreAWB.Text = ds.Tables[0].Rows[0]["AWBPrefix"].ToString();
                            txtAWB.Text = ds.Tables[0].Rows[0]["AWBNumber"].ToString();
                            txtFlightPrefix.Text = ds.Tables[0].Rows[0]["FlightPre"].ToString();
                            txtFlightNo.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString();
                            txtFlightDate.Text = ds.Tables[0].Rows[0]["FlightDate"].ToString();

                            txtCurrentFlightDate.Text = ds.Tables[0].Rows[0]["FlightDate"].ToString();
                            txtCurrentFltNo.Text = ds.Tables[0].Rows[0]["CFlightNo"].ToString();
                            txtCGrossWt.Text = ds.Tables[0].Rows[0]["GrossWeight"].ToString();
                            txtCCharWt.Text = ds.Tables[0].Rows[0]["ChargableWeight"].ToString();
                            txtCFreight.Text = ds.Tables[0].Rows[0]["FreightRate"].ToString();
                            txtCOCDC.Text = ds.Tables[0].Rows[0]["OCDC"].ToString();
                            txtCOCDA.Text = ds.Tables[0].Rows[0]["OCDA"].ToString();
                            txtCServiceTax.Text = ds.Tables[0].Rows[0]["ServiceTax"].ToString();
                            txtCCommission.Text = ds.Tables[0].Rows[0]["Commission"].ToString();
                            txtSTOnComm.Text = ds.Tables[0].Rows[0]["STCommission"].ToString();
                            txtCTDSOnComm.Text = ds.Tables[0].Rows[0]["TDSCommission"].ToString();
                            txtCTot.Text = ds.Tables[0].Rows[0]["CurrentTotal"].ToString();
                            
                            txtRevisedFlightDate.Text = ds.Tables[0].Rows[0]["RevisedFlightDate"].ToString();
                            txtRevisedFltNo.Text = ds.Tables[0].Rows[0]["RevisedFlightNo"].ToString();
                            txtRevisedGrosswt.Text = ds.Tables[0].Rows[0]["RevisedGrossWeight"].ToString();
                            txtRevisedChargableWt.Text = ds.Tables[0].Rows[0]["RevisedChargableWeight"].ToString();
                            txtRevisedFreight.Text = ds.Tables[0].Rows[0]["RevisedFreightRate"].ToString();
                            txtRevsedOCDC.Text = ds.Tables[0].Rows[0]["RevisedOCDC"].ToString();
                            txtRevisedOCDA.Text = ds.Tables[0].Rows[0]["RevisedOCDA"].ToString();
                            txtRevisedServiceTax.Text = ds.Tables[0].Rows[0]["RevisedServiceTax"].ToString();
                            txtRevisedComm.Text = ds.Tables[0].Rows[0]["RevisedCommission"].ToString();
                            txtRevisedSTOnComm.Text = ds.Tables[0].Rows[0]["RevisedSTCommission"].ToString();
                            txtRevisedTDSOnComm.Text = ds.Tables[0].Rows[0]["RevisedTDSCommission"].ToString();
                            txtRevisedTotal.Text = ds.Tables[0].Rows[0]["RevisedTotal"].ToString();
                            
                            txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();

                            rbDCMAWB.Checked = true;
                            rbDCMDeals.Checked = false;
                            double CurrentTotal = FinalCT + Convert.ToDouble(txtCTot.Text.Trim() == "" ? "0" : txtCTot.Text.Trim());
                            double RevisedTotal = FinalRT + Convert.ToDouble(txtRevisedTotal.Text.Trim() == "" ? "0" : txtRevisedTotal.Text.Trim());

                            if (RevisedTotal > CurrentTotal)
                            {
                                txtDCMAmount.Text = Math.Round((RevisedTotal - CurrentTotal), 2).ToString();
                                txtDCMType.Text = "Debit";
                            }
                            else if (RevisedTotal < CurrentTotal)
                            {
                                txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                                txtDCMType.Text = "Credit";
                            }
                            else
                            {
                                txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                            }
                        }
                        else
                        {
                            rbDCMAWB.Checked = false;
                            rbDCMDeals.Checked = true;
                            txtRevisedFlightDate.Enabled = false;
                            txtRevisedFltNo.Enabled = false;
                        }
                        if (Session["Mode"] != null && Session["Mode"].ToString() == "View")//line
                        {
                            MakeTxtEnabled(false);
                            btnSave.Enabled = false;

                        }
                        else
                        {
                            btnSave.Enabled = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This function genrates Unique DCM Number which is used as a DCM Number
        /// </summary>
        /// <returns></returns>
        #region GetDCMNumber
        protected string GetDCMNumber()
        {
            try
            {
                DataSet Ds = new DataSet();
                Ds = objBAL.GetDCMNumber();
                return Ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception)
            {

                return null;
            }

        }
        # endregion GetDCMNumber

        /// <summary>
        /// This function gets data from billing and assign it to the rspective textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Fill Current & Revised Data
        protected void btnList_Click(object sender, EventArgs e)
        {
            Session["Mode"] = "";
            Session["DCMNumber"] = "";
            if (!ValidateList())
                return;
            //this is to get the values from agent master 
            DataSet dsAgent = objBAL.GetDataFromAgent(txtAWB.Text, txtInvoiceNo.Text);
            if (dsAgent != null && dsAgent.Tables.Count != 0 && dsAgent.Tables[0].Rows.Count > 0)
            {
                TDSOnComm = Convert.ToDouble(dsAgent.Tables[0].Rows[0]["TDSOnCommision"].ToString() == "" ? "0" : dsAgent.Tables[0].Rows[0]["TDSOnCommision"].ToString());
                NormalComm = Convert.ToDouble(dsAgent.Tables[0].Rows[0]["NorrmalComm"].ToString() == "" ? "0" : dsAgent.Tables[0].Rows[0]["NorrmalComm"].ToString());
            }
            //ClearFields();
            string AWBNo, InvoiceNo, FlightNo, FlightDate;
            lblStatus.Text = "";
            txtDCMNumber.Text = "";
            DataSet ds = new DataSet();
            DataTable dt;
            DataTable dtRev;
            try
            {
                AWBNo = txtPreAWB.Text.Trim() + txtAWB.Text.Trim();
                InvoiceNo = txtInvoiceNo.Text.Trim();
                FlightNo = txtFlightPrefix.Text.Trim() + txtFlightNo.Text.Trim();
                FlightDate = txtFlightDate.Text.Trim();
                //For DCM against AWB
                if (rbDCMAWB.Checked)
                {
                    ds = objBAL.FillCurrentDCM(AWBNo, InvoiceNo, FlightNo, FlightDate);
                }
                else //For DCM against Deals
                {
                    ds = objBAL.FillCurrentDCMDeals(InvoiceNo);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    Session["STDCM"] = dt.Rows[0]["ST"].ToString();
                    txtCGrossWt.Text = dt.Rows[0][0].ToString();
                    txtCCharWt.Text = dt.Rows[0][1].ToString();
                    txtCFreight.Text = dt.Rows[0][2].ToString();
                    txtCOCDC.Text = dt.Rows[0][3].ToString();
                    txtCOCDA.Text = dt.Rows[0][4].ToString();
                    txtCServiceTax.Text = dt.Rows[0][5].ToString();
                    txtCCommission.Text = dt.Rows[0][6].ToString();
                    txtCTDSOnComm.Text = dt.Rows[0][7].ToString();
                    txtSTOnComm.Text = dt.Rows[0][8].ToString();
                    txtCTot.Text = dt.Rows[0][9].ToString();

                    if (rbDCMAWB.Checked)
                    {
                        txtRevisedFltNo.Enabled = true;
                        txtRevisedFlightDate.Enabled = true;
                        txtCurrentFlightDate.Text = dt.Rows[0][11].ToString();
                        txtRevisedFlightDate.Text = dt.Rows[0][11].ToString();
                        txtCurrentFltNo.Text = dt.Rows[0][10].ToString();
                        txtRevisedFltNo.Text = dt.Rows[0][10].ToString();
                    }
                    else
                    {
                        txtRevisedFltNo.Enabled = false;
                        txtRevisedFlightDate.Enabled = false;
                        txtCurrentFltNo.Text = dt.Rows[0][10].ToString();
                        txtRevisedFltNo.Text = dt.Rows[0][10].ToString();
                        txtCurrentFlightDate.Text = dt.Rows[0][11].ToString();
                        txtRevisedFlightDate.Text = dt.Rows[0][11].ToString();

                        dtRev = ds.Tables[1];
                        txtRevisedGrosswt.Text = dtRev.Rows[0][0].ToString();
                        txtRevisedChargableWt.Text = dtRev.Rows[0][1].ToString();
                        txtRevisedFreight.Text = dtRev.Rows[0][2].ToString();
                        txtRevsedOCDC.Text = dtRev.Rows[0][3].ToString();
                        txtRevisedOCDA.Text = dtRev.Rows[0][4].ToString();
                        txtRevisedServiceTax.Text = dtRev.Rows[0][5].ToString();
                        txtRevisedComm.Text = dtRev.Rows[0][6].ToString();
                        txtRevisedTDSOnComm.Text = dtRev.Rows[0][7].ToString();
                        txtRevisedSTOnComm.Text = dtRev.Rows[0][8].ToString();
                        txtRevisedTotal.Text = dtRev.Rows[0][9].ToString();
                    }
                }
            }
            catch (Exception)
            {
                //  throw;
            }
        }

        private void MakeTxtEnabled(bool b)
        {
            txtRevisedChargableWt.Enabled = b; txtRevisedServiceTax.Enabled = false; txtRevsedOCDC.Enabled = false; txtRemarks.Enabled = false;
            txtRevisedComm.Enabled = b; txtRevisedFlightDate.Enabled = b; txtRevisedFltNo.Enabled = b;
            txtRevisedFreight.Enabled = b; txtRevisedGrosswt.Enabled = b; txtRevisedOCDA.Enabled = b;
            txtRevisedSTOnComm.Enabled = b; txtRevisedTDSOnComm.Enabled = b; txtRevisedTotal.Enabled = b;
        }
        #endregion Fill Current & Revised Data

        #region validation for list button
        protected bool ValidateList()
        {
            if (rbDCMAWB.Checked) //for DCM against AWB
            {
                
                if (txtPreAWB.Text.Trim() == "" || txtAWB.Text.Trim() == "" || txtInvoiceNo.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Enter AWBPrefix, AWB number and Invoice Number";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
                if (txtFlightNo.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Enter Flight number";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }

                if (txtFlightDate.Text.Trim() != "")
                {
                    DateTime dt;

                    try
                    {
                        string day = txtFlightDate.Text.Substring(0, 2);
                        string mon = txtFlightDate.Text.Substring(3, 2);
                        string yr = txtFlightDate.Text.Substring(6, 4);
                        flightDate = yr + "/" + mon + "/" + day;
                        dt = Convert.ToDateTime(flightDate);
                    }
                    catch
                    {
                        lblStatus.Visible = true;
                        lblStatus.Text = "Selected Date format invalid";
                        lblStatus.ForeColor = Color.Red;
                        return false;
                    }
                }
            }
            else //for DCM against deals
            {
                if (txtInvoiceNo.Text.Trim() == "")
                {
                    lblStatus.Text = "Please Enter Invoice Number";
                    lblStatus.ForeColor = Color.Red;
                    return false;
                }
            }

            return true;
        }
        #endregion validation for list button

        #region get invoice number from AWB number
        protected string getInvoiceNumber(string AWBNumber)
        {
            string InvoiceNumber = "";
            try
            {
                InvoiceNumber = objBAL.GetInvoiceNumber(AWBNumber);
                lblStatus.Text = "";
                return InvoiceNumber;
            }
            catch (Exception)
            {
                lblStatus.Text = InvoiceNumber;
                lblStatus.ForeColor = Color.Red;
                return "";
            }


        }
        #endregion get invoice number from AWB number

        /// <summary>
        /// This is a datatable used to save to save data when user hits on a Save Button
        /// </summary>
        #region DCMDatatable
        public void DCMDatatable()
        {
            try
            {
                DataTable dtDCM = new DataTable("dt_DCM");
                dtDCM.Columns.Add("AWBPrefix", typeof(string));
                dtDCM.Columns.Add("AWBNumber", typeof(string));
                dtDCM.Columns.Add("InvoiceNumber", typeof(string));
                dtDCM.Columns.Add("FlightNo", typeof(string));
                dtDCM.Columns.Add("grossWt", typeof(double));
                dtDCM.Columns.Add("ChargbleWt", typeof(double));
                dtDCM.Columns.Add("FreightRate", typeof(double));
                dtDCM.Columns.Add("OCDC", typeof(double));
                dtDCM.Columns.Add("OCDA", typeof(double));
                dtDCM.Columns.Add("ServiceTax", typeof(double));
                dtDCM.Columns.Add("Commission", typeof(double));
                dtDCM.Columns.Add("TDSComm", typeof(double));
                dtDCM.Columns.Add("STComm", typeof(double));
                dtDCM.Columns.Add("Total", typeof(double));
                dtDCM.Columns.Add("RevisedFlightNo", typeof(string));
                dtDCM.Columns.Add("RevisedgrossWt", typeof(double));
                dtDCM.Columns.Add("RevisedChargbleWt", typeof(double));
                dtDCM.Columns.Add("RevisedFreightRate", typeof(double));
                dtDCM.Columns.Add("RevisedOCDC", typeof(double));
                dtDCM.Columns.Add("RevisedOCDA", typeof(double));
                dtDCM.Columns.Add("RevisedServiceTax", typeof(double));
                dtDCM.Columns.Add("RevisedCommission", typeof(double));
                dtDCM.Columns.Add("RevisedTDSComm", typeof(double));
                dtDCM.Columns.Add("RevisedSTComm", typeof(double));
                dtDCM.Columns.Add("RevisedTotal", typeof(double));
                dtDCM.Columns.Add("CreatedBy", typeof(string));
                dtDCM.Columns.Add("Remarks", typeof(string));
                dtDCM.Columns.Add("FlightDate", typeof(string));
                dtDCM.Columns.Add("RevisedFlightDate", typeof(string));
                dtDCM.Columns.Add("DCMNumber", typeof(string));


                Session["DCM"] = dtDCM;
            }
            catch (Exception ex)
            { }

        }
        #endregion DCMDatatable

        /// <summary>
        /// This method saves data in a table named debit credit processing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Save DCM Processing
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbDCMAWB.Checked == true)
                {
                    if (string.IsNullOrEmpty(txtPreAWB.Text))
                    {
                        lblStatus.Text = "Please Enter AWB Prefix.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    if (string.IsNullOrEmpty(txtAWB.Text))
                    {
                        lblStatus.Text = "Please Enter AWB No.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    if (string.IsNullOrEmpty(txtInvoiceNo.Text))
                    {
                        lblStatus.Text = "Please Enter Invoice No.";
                        lblStatus.ForeColor = Color.Red; 
                        return;
                    }
                    if (string.IsNullOrEmpty(txtFlightNo.Text))
                    {
                        lblStatus.Text = "Please Enter Flight No.!";
                        lblStatus.ForeColor = Color.Red; 
                        return;
                    }

                    if (string.IsNullOrEmpty(txtFlightDate.Text))
                    {
                        lblStatus.Text = "Please Enter Flight Date.!";
                        lblStatus.ForeColor = Color.Red; 
                        return;
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(txtInvoiceNo.Text))
                    {
                        lblStatus.Text = "Please Enter Invoice No.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                try
                {
                    if (txtCurrentFlightDate.Text.Trim() != "")
                    {
                        DateTime dt;
                        string day = txtCurrentFlightDate.Text.Substring(0, 2);
                        string mon = txtCurrentFlightDate.Text.Substring(3, 2);
                        string yr = txtCurrentFlightDate.Text.Substring(6, 4);
                        currentFlightDate = yr + "/" + mon + "/" + day;
                        dt = Convert.ToDateTime(currentFlightDate);
                    }
                    if (txtRevisedFlightDate.Text.Trim() != "")
                    {
                        DateTime dt;
                        string day = txtRevisedFlightDate.Text.Substring(0, 2);
                        string mon = txtRevisedFlightDate.Text.Substring(3, 2);
                        string yr = txtRevisedFlightDate.Text.Substring(6, 4);
                        revisedFlightDate = yr + "/" + mon + "/" + day;
                        dt = Convert.ToDateTime(revisedFlightDate);
                    }
                }
                catch
                {
                    lblStatus.Text = "Revised/Current Flight Date not in correct format!";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                DCMDatatable();

                double grossWt, ChargbleWt, FreightRate, OCDC, OCDA, ServiceTax, Commission, TDSComm, STComm, Total;
                double RevisedgrossWt, RevisedChargbleWt, RevisedFreightRate, RevisedOCDC, RevisedOCDA, RevisedServiceTax, RevisedCommission, RevisedTDSComm, RevisedSTComm, RevisedTotal;
                string AWBPrefix, AWBnumber, InvoiceNumber, remarks, FlightNo, RevisedFlightNo, FlightDate, RevisedFlightDate;

                AWBPrefix = txtPreAWB.Text.Trim();
                AWBnumber = txtAWB.Text.Trim();
                InvoiceNumber = txtInvoiceNo.Text;
                grossWt = Convert.ToDouble(txtCGrossWt.Text == "" ? "0" : txtCGrossWt.Text);
                ChargbleWt = Convert.ToDouble(txtCCharWt.Text == "" ? "0" : txtCCharWt.Text);
                FreightRate = Convert.ToDouble(txtCFreight.Text == "" ? "0" : txtCFreight.Text);
                OCDC = Convert.ToDouble(txtCOCDC.Text == "" ? "0" : txtCOCDC.Text);
                OCDA = Convert.ToDouble(txtCOCDA.Text == "" ? "0" : txtCOCDA.Text);
                ServiceTax = Convert.ToDouble(txtCServiceTax.Text == "" ? "0" : txtCServiceTax.Text);
                Commission = Convert.ToDouble(txtCCommission.Text == "" ? "0" : txtCCommission.Text);
                TDSComm = Convert.ToDouble(txtCTDSOnComm.Text == "" ? "0" : txtCTDSOnComm.Text);
                STComm = Convert.ToDouble(txtSTOnComm.Text == "" ? "0" : txtSTOnComm.Text);
                Total = Convert.ToDouble(txtCTot.Text == "" ? "0" : txtCTot.Text);
                FlightNo = txtCurrentFltNo.Text.Trim();
                RevisedgrossWt = Convert.ToDouble(txtRevisedGrosswt.Text == "" ? "0" : txtRevisedGrosswt.Text);
                RevisedChargbleWt = Convert.ToDouble(txtRevisedChargableWt.Text == "" ? "0" : txtRevisedChargableWt.Text);
                RevisedFreightRate = Convert.ToDouble(txtRevisedFreight.Text == "" ? "0" : txtRevisedFreight.Text);
                RevisedOCDC = Convert.ToDouble(txtRevsedOCDC.Text == "" ? "0" : txtRevsedOCDC.Text);
                RevisedOCDA = Convert.ToDouble(txtRevisedOCDA.Text == "" ? "0" : txtRevisedOCDA.Text);
                RevisedServiceTax = Convert.ToDouble(txtRevisedServiceTax.Text == "" ? "0" : txtRevisedServiceTax.Text);
                RevisedCommission = Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text);
                RevisedTDSComm = Convert.ToDouble(txtRevisedTDSOnComm.Text == "" ? "0" : txtRevisedTDSOnComm.Text);
                RevisedSTComm = Convert.ToDouble(txtRevisedSTOnComm.Text == "" ? "0" : txtRevisedSTOnComm.Text);
                RevisedTotal = Convert.ToDouble(txtRevisedTotal.Text == "" ? "0" : txtRevisedTotal.Text);
                RevisedFlightNo = txtRevisedFltNo.Text.Trim();
                remarks = txtRemarks.Text.Trim();
                FlightDate = currentFlightDate;
                RevisedFlightDate = revisedFlightDate;
                if (currentFlightDate == null || revisedFlightDate == null)
                {
                    currentFlightDate = "";
                    revisedFlightDate = "";
                }


                FinalCT = FinalCT + Total;
                FinalRT = FinalRT + RevisedTotal;

                DataTable dtDcm = new DataTable("dt_DCM");
                dtDcm = (DataTable)Session["DCM"];
                //DataRow dr = 
                dtDcm.Rows.Add(AWBPrefix, AWBnumber, InvoiceNumber, FlightNo, grossWt,
                                        ChargbleWt, FreightRate, OCDC, OCDA, ServiceTax, Commission, TDSComm, STComm, Total,
                                        RevisedFlightNo, RevisedgrossWt, RevisedChargbleWt, RevisedFreightRate, RevisedOCDC,
                                        RevisedOCDA, RevisedServiceTax, RevisedCommission, RevisedTDSComm, RevisedSTComm,
                                        RevisedTotal, Session["UserName"].ToString(), remarks, FlightDate, RevisedFlightDate, Session["DCMNumber"].ToString());
                Session["DCM"] = dtDcm;

                if (dtDcm.Rows.Count > 0)
                {
                    clear();
                    lblStatus.Text = "AWB charges saved successfully";
                    //Session["Mode"] = "";
                    //Session["DCMNumber"] = "";
                    lblStatus.ForeColor = Color.Green;
                    //if (Session["DCMNumber"].ToString() != "")
                    //{
                    //    Response.Redirect("ListDCMAWBDeals.aspx", false);
                    //}
                    return;
                }
                else
                {
                    lblStatus.Text = "AWB charges not saved please try again ";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                //     


            }
            catch
            {
                lblStatus.Text = "AWB charges not saved please try again ";
                lblStatus.ForeColor = Color.Red;
                return;
            }

        }
        #endregion Save DCMProcessing

        /// <summary>
        /// When we hit on a generate dcm at that time data present in a Datatable that is saved in a table  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region GenrateDCM
        protected void btnGenerateDCM_Click(object sender, EventArgs e)
        {
            try
            {
                object[] objDCM = new object[32];
                //to save DCM type A - DCM against AWB and D - DCM against deals
                string dcmType = string.Empty;
                //To check if AWB is saved before generating DCM.
                DataTable dtDCM = (DataTable)Session["DCM"];
                if (dtDCM.Rows.Count == 0)
                {
                    lblStatus.Text = "Save Current and Revised values to Generate DCM";

                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    for (int i = 0; i < dtDCM.Rows.Count; i++)
                    {
                        int j = 0;
                        if (rbDCMAWB.Checked)
                            dcmType = "A";
                        else
                            dcmType = "D";

                        objDCM.SetValue(dcmType, j);//0
                        objDCM.SetValue(dtDCM.Rows[i]["AWBPrefix"].ToString(), ++j);//1
                        objDCM.SetValue(dtDCM.Rows[i]["AWBNumber"].ToString(), ++j);//2
                        objDCM.SetValue(dtDCM.Rows[i]["InvoiceNumber"].ToString(), ++j);//3
                        objDCM.SetValue(dtDCM.Rows[i]["FlightNo"].ToString(), ++j);//4
                        objDCM.SetValue(dtDCM.Rows[i]["grossWt"].ToString(), ++j);//5
                        objDCM.SetValue(dtDCM.Rows[i]["ChargbleWt"].ToString(), ++j);//6
                        objDCM.SetValue(dtDCM.Rows[i]["FreightRate"].ToString(), ++j);//7
                        objDCM.SetValue(dtDCM.Rows[i]["OCDC"].ToString(), ++j);//8
                        objDCM.SetValue(dtDCM.Rows[i]["OCDA"].ToString(), ++j);//9
                        objDCM.SetValue(dtDCM.Rows[i]["ServiceTax"].ToString(), ++j);//10
                        objDCM.SetValue(dtDCM.Rows[i]["Commission"].ToString(), ++j);//11
                        objDCM.SetValue(dtDCM.Rows[i]["TDSComm"].ToString(), ++j);//12
                        objDCM.SetValue(dtDCM.Rows[i]["STComm"].ToString(), ++j);//13
                        objDCM.SetValue(dtDCM.Rows[i]["Total"].ToString(), ++j);//14
                        objDCM.SetValue(dtDCM.Rows[i]["RevisedFlightNo"].ToString(), ++j);//15
                        objDCM.SetValue(dtDCM.Rows[i]["RevisedgrossWt"].ToString(), ++j);//16
                        objDCM.SetValue(dtDCM.Rows[i]["RevisedChargbleWt"].ToString(), ++j);//17
                        objDCM.SetValue(dtDCM.Rows[i]["RevisedFreightRate"].ToString(), ++j);//18
                        objDCM.SetValue(dtDCM.Rows[i]["RevisedOCDC"].ToString(), ++j);//19
                        objDCM.SetValue(dtDCM.Rows[i]["RevisedOCDA"].ToString(), ++j);//20
                        objDCM.SetValue(dtDCM.Rows[i]["RevisedServiceTax"].ToString(), ++j);//21
                        objDCM.SetValue(dtDCM.Rows[i]["RevisedCommission"].ToString(), ++j);//22
                        objDCM.SetValue(dtDCM.Rows[i]["RevisedTDSComm"].ToString(), ++j);//23
                        objDCM.SetValue(dtDCM.Rows[i]["RevisedSTComm"].ToString(), ++j);//24
                        objDCM.SetValue(dtDCM.Rows[i]["RevisedTotal"].ToString(), ++j);//25
                        objDCM.SetValue(dtDCM.Rows[i]["CreatedBy"].ToString(), ++j);//26
                        objDCM.SetValue(dtDCM.Rows[i]["Remarks"].ToString(), ++j);//27
                        objDCM.SetValue(dtDCM.Rows[i]["FlightDate"].ToString(), ++j);//28
                        objDCM.SetValue(dtDCM.Rows[i]["RevisedFlightDate"].ToString(), ++j);//29
                        objDCM.SetValue(Convert.ToDateTime(Session["IT"].ToString()), ++j);//30
                        objDCM.SetValue(Session["DCMNumber"], ++j);//31

                        string res = "";
                        res = objBAL.SaveDCMProcessing(objDCM);

                        if (res != "error")
                        {
                            txtDCMNumber.Text = res;
                            lblStatus.Text = "DCM generated successfully";
                            lblStatus.ForeColor = Color.Green;

                            //To show DCM amount and DCM type (Credit/Debit)
                            //double RevisedTotal = Convert.ToDouble(txtRevisedTotal.Text);
                            //double CurrentTotal = Convert.ToDouble(txtCTot.Text);

                            double RevisedTotal = FinalRT;
                            double CurrentTotal = FinalCT;
                            if (RevisedTotal > CurrentTotal)
                            {
                                txtDCMAmount.Text = Math.Round((RevisedTotal - CurrentTotal), 2).ToString();
                                txtDCMType.Text = "Debit";
                            }
                            else if (RevisedTotal < CurrentTotal)
                            {
                                txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                                txtDCMType.Text = "Credit";
                            }
                            else
                            {
                                txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                            }
                        }
                        else
                        {
                            lblStatus.Text = res;
                            lblStatus.ForeColor = Color.Red;
                        }

                    }

                    Session["DCM"] = null;

                }

                FinalCT = 0; FinalRT = 0;
                clear();
            }
            catch
            { }
        }
        #endregion GenrateDCM

        #region clear
        public void clear()
        {
            try
            {
                txtCGrossWt.Text = "";
                txtAWB.Text = "";
                txtFlightNo.Text = "";
                txtInvoiceNo.Text = "";
                txtCurrentFltNo.Text = "";
                txtCCharWt.Text = "";
                txtCFreight.Text = "";
                txtCOCDA.Text = "";
                txtCOCDC.Text = "";
                txtCServiceTax.Text = "";
                txtCCommission.Text = "";
                txtCTDSOnComm.Text = "";
                txtSTOnComm.Text = "";
                txtCTot.Text = "";
                txtRevisedFltNo.Text = "";
                txtRevisedGrosswt.Text = "";
                txtRevisedChargableWt.Text = "";
                txtRevisedFreight.Text = "";
                txtRevisedComm.Text = "";
                txtRevisedOCDA.Text = "";
                txtRevsedOCDC.Text = "";
                txtRevisedServiceTax.Text = "";
                txtRevisedSTOnComm.Text = "";
                txtRevisedTDSOnComm.Text = "";
                txtRevisedTotal.Text = "";
                txtFlightDate.Text = "";
                txtCurrentFlightDate.Text = "";
                txtRevisedFlightDate.Text = "";
                txtRemarks.Text = "";

            }
            catch (Exception ex)
            { }
        }
        #endregion clear

        /// <summary>
        /// In case of getting  data from agent master mapping with agentCode from agentMaster and controlling locator code from billingawbinvoicematching
        /// all calculatin part reatedly is done in this block
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Calculation
        protected void txtRevisedOCDA_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (txtAWB.Text == "" && txtInvoiceNo.Text == "")
                {
                    lblStatus.Text = "Please provide awbnumner and invoicenumber";
                    lblStatus.ForeColor = Color.Red;
                    return;

                }
                string AWBno = txtAWB.Text;
                string InvoiceNo = txtInvoiceNo.Text;
                //this is to get the values from agent master 
                DataSet ds = objBAL.GetDataFromAgent(AWBno, InvoiceNo);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    TDSOnComm = Convert.ToDouble(ds.Tables[0].Rows[0]["TDSOnCommision"].ToString() == "" ? "0" :
                                                 ds.Tables[0].Rows[0]["TDSOnCommision"].ToString());
                    NormalComm = Convert.ToDouble(ds.Tables[0].Rows[0]["NorrmalComm"].ToString() == "" ? "0" :
                                                  ds.Tables[0].Rows[0]["NorrmalComm"].ToString());
                }
                //Formulae to calculate all fields
                ServiceTax = Math.Round(((Convert.ToDouble(txtRevisedFreight.Text == "" ? "0" : txtRevisedFreight.Text) +
                    Convert.ToDouble(txtRevsedOCDC.Text) + Convert.ToDouble(txtRevisedOCDA.Text == "" ? "0" : txtRevisedOCDA.Text)) * ST) / 100);
                commissionamt = Math.Round(((Convert.ToDouble(txtRevisedFreight.Text == "" ? "0" : txtRevisedFreight.Text)) * NormalComm) / 100);

                STComm = Math.Round((commissionamt * ST) / 100);
                TDSOnComm = Math.Round(((commissionamt + STComm) * TDSOnComm) / 100);

                txtRevisedServiceTax.Text = Convert.ToString(ServiceTax);
                txtRevisedComm.Text = Convert.ToString(commissionamt);
                txtRevisedSTOnComm.Text = Convert.ToString(STComm);
                txtRevisedTDSOnComm.Text = Convert.ToString(TDSOnComm);


                //Total
                double RevisedTotal;
                RevisedTotal = Math.Round(Convert.ToDouble(txtRevisedFreight.Text == "" ? "0" : txtRevisedFreight.Text) +
                    Convert.ToDouble(txtRevsedOCDC.Text == "" ? "0" : txtRevsedOCDC.Text) +
                    Convert.ToDouble(txtRevisedServiceTax.Text == "" ? "0" : txtRevisedServiceTax.Text) +
                    Convert.ToDouble(txtRevisedTDSOnComm.Text == "" ? "0" : txtRevisedTDSOnComm.Text)) -
                    (Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text) +
                    ((Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text) * ST) / 100));
                //RevisedTotal = (Convert.ToDouble(txtRevisedChargableWt.Text) + Convert.ToDouble(txtRevisedComm.Text) +Convert.ToDouble( txtRevisedFreight.Text) + Convert.ToDouble(txtRevisedGrosswt.Text) + Convert.ToDouble(txtRevisedOCDA.Text) + Convert.ToDouble(txtRevisedServiceTax.Text )+ Convert.ToDouble(txtRevisedSTOnComm.Text) + Convert.ToDouble(txtRevisedTDSOnComm.Text) + Convert.ToDouble(txtRevsedOCDC.Text) + Convert.ToDouble(txtSTOnComm.Text));
                txtRevisedTotal.Text = Convert.ToString(RevisedTotal);

            }
            catch
            { }
        }
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
                clear();
                lblStatus.Text = "";
                Session["Mode"] = "";
                Session["DCMNumber"] = "";
                MakeTxtEnabled(true);
                if (rbDCMAWB.Checked == true)
                {
                    txtRevisedFltNo.Enabled = true;
                    txtRevisedFlightDate.Enabled = true;
                }
                else
                {
                    txtRevisedFltNo.Enabled = false;
                    txtRevisedFlightDate.Enabled = false;
                }
            }
            catch { }
        }
        #endregion ClearButton

        protected void btnTaxCalc_Click(object sender, EventArgs e)
        {
            CalcTaxOnTextExit();
        }

        protected void CalcTaxOnTextExit()
        {
            //Code to put zero if nothing is entered in textbox
            if (txtRevisedFreight.Text == "")
                txtRevisedFreight.Text = "0";
            if (txtRevisedOCDA.Text == "")
                txtRevisedOCDA.Text = "0";
            if (txtRevsedOCDC.Text == "")
                txtRevsedOCDC.Text = "0";
            if (txtRevisedServiceTax.Text == "")
                txtRevisedServiceTax.Text = "0";
            if (txtRevisedComm.Text == "")
                txtRevisedComm.Text = "0";

            try
            {
                if (Convert.ToDouble(txtRevisedFreight.Text.Trim() == "" ? "0" : txtRevisedFreight.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised Freight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevsedOCDC.Text.Trim() == "" ? "0" : txtRevsedOCDC.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised OCDC";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedOCDA.Text.Trim() == "" ? "0" : txtRevisedOCDA.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised OCDA";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedServiceTax.Text.Trim() == "" ? "0" : txtRevisedServiceTax.Text) < 0)
                {
                    lblStatus.Text = "Enter valid revised Service Tax";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedComm.Text.Trim() == "" ? "0" : txtRevisedComm.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised Commission";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    ST = Session["STDCM"] != null ? Convert.ToDouble(Session["STDCM"]) : 0;
                    //Code to get calculate taxes on change of Freight, OCDC and OCDA
                    ST = float.Parse(txtCServiceTax.Text == "" ? "0" : txtCServiceTax.Text) == 0 ? 0 : (Session["STDCM"] != null ? Convert.ToDouble(Session["STDCM"]) : 0);
                    txtRevisedServiceTax.Text = Math.Round((Convert.ToDouble(txtRevisedFreight.Text == "" ? "0" : txtRevisedFreight.Text) +
                        Convert.ToDouble(txtRevisedOCDA.Text == "" ? "0" : txtRevisedOCDA.Text) +
                        Convert.ToDouble(txtRevsedOCDC.Text == "" ? "0" : txtRevsedOCDC.Text)) * ST / 100, 2).ToString();
                    txtRevisedComm.Text = Math.Round((Convert.ToDouble(txtRevisedFreight.Text == "" ? "0" : txtRevisedFreight.Text) * NormalComm) / 100, 2).ToString();
                    ST = float.Parse(txtSTOnComm.Text == "" ? "0" : txtSTOnComm.Text) == 0 ? 0 : (Session["STDCM"] != null ? Convert.ToDouble(Session["STDCM"]) : 0);
                    txtRevisedSTOnComm.Text = Math.Round(Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text) * ST / 100, 2).ToString();
                    //txtRevisedTDSOnComm.Text = Math.Round((Convert.ToDouble(txtRevisedComm.Text) + Convert.ToDouble(txtRevisedSTOnComm.Text)) * TDSOnComm / 100, 2).ToString();
                    txtRevisedTDSOnComm.Text = Math.Round((Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text) + 0) * TDSOnComm / 100, 2).ToString();
                    txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txtRevisedFreight.Text == "" ? "0" : txtRevisedFreight.Text) +
                        Convert.ToDouble(txtRevsedOCDC.Text == "" ? "0" : txtRevsedOCDC.Text) + Convert.ToDouble(txtRevisedServiceTax.Text == "" ? "0" : txtRevisedServiceTax.Text) +
                        Convert.ToDouble(txtRevisedTDSOnComm.Text == "" ? "0" : txtRevisedTDSOnComm.Text) -
                        (Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text) +
                        Convert.ToDouble(txtRevisedSTOnComm.Text == "" ? "0" : txtRevisedSTOnComm.Text)), 2).ToString();
                }

                double CurrentTotal = FinalCT + Convert.ToDouble(txtCTot.Text.Trim() == "" ? "0" : txtCTot.Text.Trim());
                double RevisedTotal = FinalRT + Convert.ToDouble(txtRevisedTotal.Text.Trim() == "" ? "0" : txtRevisedTotal.Text.Trim());

                if (RevisedTotal > CurrentTotal)
                {
                    txtDCMAmount.Text = Math.Round((RevisedTotal - CurrentTotal), 2).ToString();
                    txtDCMType.Text = "Debit";
                }
                else if (RevisedTotal < CurrentTotal)
                {
                    txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                    txtDCMType.Text = "Credit";
                }
                else
                {
                    txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                }
            }
            catch
            {

                throw;
            }
        }

        protected void CalcTaxOnCommChange()
        {
            //Code to put zero if nothing is entered in textbox
            if (txtRevisedFreight.Text == "")
                txtRevisedFreight.Text = "0";
            if (txtRevisedOCDA.Text == "")
                txtRevisedOCDA.Text = "0";
            if (txtRevsedOCDC.Text == "")
                txtRevsedOCDC.Text = "0";
            if (txtRevisedServiceTax.Text == "")
                txtRevisedServiceTax.Text = "0";
            if (txtRevisedComm.Text == "")
                txtRevisedComm.Text = "0";

            try
            {
                if (Convert.ToDouble(txtRevisedFreight.Text.Trim() == "" ? "0" : txtRevisedFreight.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised Freight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevsedOCDC.Text.Trim() == "" ? "0" : txtRevsedOCDC.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised OCDC";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedOCDA.Text.Trim() == "" ? "0" : txtRevisedOCDA.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised OCDA";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedServiceTax.Text.Trim() == "" ? "0" : txtRevisedServiceTax.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised Service Tax";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedComm.Text.Trim() == "" ? "0" : txtRevisedComm.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised Commission";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    ST = float.Parse(txtSTOnComm.Text == "" ? "0" : txtSTOnComm.Text) == 0 ? 0 : (Session["STDCM"] != null ? Convert.ToDouble(Session["STDCM"]) : 0);
                    //Code to get calculate taxes on change of Freight, OCDC and OCDA
                    txtRevisedSTOnComm.Text = Math.Round(Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text) * ST / 100, 2).ToString();
                    //txtRevisedTDSOnComm.Text = Math.Round((Convert.ToDouble(txtRevisedComm.Text) + Convert.ToDouble(txtRevisedSTOnComm.Text)) * TDSOnComm / 100, 2).ToString();
                    txtRevisedTDSOnComm.Text = Math.Round((Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text) + 0) * TDSOnComm / 100, 2).ToString();
                    txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txtRevisedFreight.Text == "" ? "0" : txtRevisedFreight.Text) + Convert.ToDouble(txtRevsedOCDC.Text == "" ? "0" : txtRevsedOCDC.Text) +
                        Convert.ToDouble(txtRevisedServiceTax.Text == "" ? "0" : txtRevisedServiceTax.Text) + Convert.ToDouble(txtRevisedTDSOnComm.Text == "" ? "0" : txtRevisedTDSOnComm.Text) -
                        (Convert.ToDouble(txtRevisedComm.Text) + Convert.ToDouble(txtRevisedSTOnComm.Text == "" ? "0" : txtRevisedSTOnComm.Text)), 2).ToString();
                }

                double CurrentTotal = FinalCT + Convert.ToDouble(txtCTot.Text.Trim() == "" ? "0" : txtCTot.Text.Trim());
                double RevisedTotal = FinalRT + Convert.ToDouble(txtRevisedTotal.Text.Trim());

                if (RevisedTotal > CurrentTotal)
                {
                    txtDCMAmount.Text = Math.Round((RevisedTotal - CurrentTotal), 2).ToString();
                    txtDCMType.Text = "Debit";
                }
                else if (RevisedTotal < CurrentTotal)
                {
                    txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                    txtDCMType.Text = "Credit";
                }
                else
                {
                    txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                }
            }
            catch
            {

                throw;
            }
        }

        protected void btnCommChange_Click(object sender, EventArgs e)
        {
            CalcTaxOnCommChange();
        }

        protected void btnAWBNumberChange_Click(object sender, EventArgs e)
        {
            string InvNum = getInvoiceNumber(txtPreAWB.Text.Trim() + txtAWB.Text.Trim());
            if (InvNum != "")
            {
                txtInvoiceNo.Text = InvNum;
            }
            else
            {
                lblStatus.Text = "Please close the Invoice to generate DCM for AWB.";
                lblStatus.ForeColor = Color.Red;
                clear();
            }
        }

        protected void rbDCMAWB_CheckedChanged(object sender, EventArgs e)
        {
            txtAWB.Text = "";
            txtFlightNo.Text = "";
            txtAWB.Enabled = true;
            txtFlightNo.Enabled = true;
            txtFlightDate.Enabled = true;
            txtRevisedFltNo.Enabled = true;
            txtRevisedFlightDate.Enabled = true;
        }

        protected void rbDCMDeals_CheckedChanged(object sender, EventArgs e)
        {
            txtAWB.Text = "";
            txtFlightNo.Text = "";
            txtAWB.Enabled = false;
            txtFlightNo.Enabled = false;
            txtFlightDate.Enabled = false;
            txtRevisedFltNo.Enabled = false;
            txtRevisedFlightDate.Enabled = false;
        }

        protected void btnSTChange_Click(object sender, EventArgs e)
        {
            CalcTaxOnSTChange();
        }

        protected void CalcTaxOnSTChange()
        {
            //Code to put zero if nothing is entered in textbox
            if (txtRevisedFreight.Text == "")
                txtRevisedFreight.Text = "0";
            if (txtRevisedOCDA.Text == "")
                txtRevisedOCDA.Text = "0";
            if (txtRevsedOCDC.Text == "")
                txtRevsedOCDC.Text = "0";
            if (txtRevisedServiceTax.Text == "")
                txtRevisedServiceTax.Text = "0";
            if (txtRevisedComm.Text == "")
                txtRevisedComm.Text = "0";

            try
            {
                if (Convert.ToDouble(txtRevisedFreight.Text.Trim() == "" ? "0" : txtRevisedFreight.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised Freight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevsedOCDC.Text.Trim() == "" ? "0" : txtRevsedOCDC.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised OCDC";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedOCDA.Text.Trim() == "" ? "0" : txtRevisedOCDA.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised OCDA";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedServiceTax.Text.Trim() == "" ? "0" : txtRevisedServiceTax.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised Service Tax";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedComm.Text.Trim() == "" ? "0" : txtRevisedComm.Text) < 0)
                {
                    lblStatus.Text = "Enter valid revised Commission";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    //ST = Session["STDCM"] != null ? Convert.ToDouble(Session["STDCM"]) : 0;
                    ST = float.Parse(txtSTOnComm.Text == "" ? "0" : txtSTOnComm.Text) == 0 ? 0 : (Session["STDCM"] != null ? Convert.ToDouble(Session["STDCM"]) : 0);
                    //Code to get calculate taxes on change of Freight, OCDC and OCDA
                    txtRevisedComm.Text = Math.Round((Convert.ToDouble(txtRevisedFreight.Text == "" ? "0" : txtRevisedFreight.Text) * NormalComm) / 100, 2).ToString();
                    txtRevisedSTOnComm.Text = Math.Round(Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text) * ST / 100, 2).ToString();
                    //txtRevisedTDSOnComm.Text = Math.Round((Convert.ToDouble(txtRevisedComm.Text) + Convert.ToDouble(txtRevisedSTOnComm.Text)) * TDSOnComm / 100, 2).ToString();
                    txtRevisedTDSOnComm.Text = Math.Round((Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text) + 0) * TDSOnComm / 100, 2).ToString();
                    txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txtRevisedFreight.Text == "" ? "0" : txtRevisedFreight.Text) +
                        Convert.ToDouble(txtRevsedOCDC.Text) + Convert.ToDouble(txtRevisedServiceTax.Text == "" ? "0" : txtRevisedServiceTax.Text) +
                        Convert.ToDouble(txtRevisedTDSOnComm.Text == "" ? "0" : txtRevisedTDSOnComm.Text) - (Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text) +
                        Convert.ToDouble(txtRevisedSTOnComm.Text == "" ? "0" : txtRevisedSTOnComm.Text)), 2).ToString();
                }

                double CurrentTotal = FinalCT + Convert.ToDouble(txtCTot.Text.Trim() == "" ? "0" : txtCTot.Text.Trim());
                double RevisedTotal = FinalRT + Convert.ToDouble(txtRevisedTotal.Text.Trim() == "" ? "0" : txtRevisedTotal.Text.Trim());

                if (RevisedTotal > CurrentTotal)
                {
                    txtDCMAmount.Text = Math.Round((RevisedTotal - CurrentTotal), 2).ToString();
                    txtDCMType.Text = "Debit";
                }
                else if (RevisedTotal < CurrentTotal)
                {
                    txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                    txtDCMType.Text = "Credit";
                }
                else
                {
                    txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                }
            }
            catch
            {
                throw;
            }
        }

        protected void btnSTOnCommChange_Click(object sender, EventArgs e)
        {
            CalcTaxOnSTOnCommChange();
        }

        protected void CalcTaxOnSTOnCommChange()
        {
            //Code to put zero if nothing is entered in textbox
            if (txtRevisedFreight.Text == "")
                txtRevisedFreight.Text = "0";
            if (txtRevisedOCDA.Text == "")
                txtRevisedOCDA.Text = "0";
            if (txtRevsedOCDC.Text == "")
                txtRevsedOCDC.Text = "0";
            if (txtRevisedServiceTax.Text == "")
                txtRevisedServiceTax.Text = "0";
            if (txtRevisedComm.Text == "")
                txtRevisedComm.Text = "0";

            try
            {
                if (Convert.ToDouble(txtRevisedFreight.Text.Trim() == "" ? "0" : txtRevisedFreight.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised Freight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevsedOCDC.Text.Trim() == "" ? "0" : txtRevsedOCDC.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised OCDC";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedOCDA.Text.Trim() == "" ? "0" : txtRevisedOCDA.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised OCDA";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedServiceTax.Text.Trim() == "" ? "0" : txtRevisedServiceTax.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised Service Tax";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedComm.Text.Trim() == "" ? "0" : txtRevisedComm.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised Commission";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    ST = Session["STDCM"] != null ? Convert.ToDouble(Session["STDCM"]) : 0;
                    //Code to get calculate taxes on change of Freight, OCDC and OCDA
                    //txtRevisedTDSOnComm.Text = Math.Round((Convert.ToDouble(txtRevisedComm.Text) + Convert.ToDouble(txtRevisedSTOnComm.Text)) * TDSOnComm / 100, 2).ToString();
                    txtRevisedTDSOnComm.Text = Math.Round((Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text) + 0) * TDSOnComm / 100, 2).ToString();
                    txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txtRevisedFreight.Text == "" ? "0" : txtRevisedFreight.Text) +
                        Convert.ToDouble(txtRevsedOCDC.Text == "" ? "0" : txtRevsedOCDC.Text) +
                        Convert.ToDouble(txtRevisedServiceTax.Text == "" ? "0" : txtRevisedServiceTax.Text) +
                        Convert.ToDouble(txtRevisedTDSOnComm.Text == "" ? "0" : txtRevisedTDSOnComm.Text) -
                        (Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text) +
                         Convert.ToDouble(txtRevisedSTOnComm.Text == "" ? "0" : txtRevisedSTOnComm.Text)), 2).ToString();
                }

                double CurrentTotal = FinalCT + Convert.ToDouble(txtCTot.Text.Trim() == "" ? "0" : txtCTot.Text.Trim());
                double RevisedTotal = FinalRT + Convert.ToDouble(txtRevisedTotal.Text.Trim() == "" ? "0" : txtRevisedTotal.Text.Trim());

                if (RevisedTotal > CurrentTotal)
                {
                    txtDCMAmount.Text = Math.Round((RevisedTotal - CurrentTotal), 2).ToString();
                    txtDCMType.Text = "Debit";
                }
                else if (RevisedTotal < CurrentTotal)
                {
                    txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                    txtDCMType.Text = "Credit";
                }
                else
                {
                    txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                }
            }
            catch
            {
                throw;
            }
        }

        protected void btnTDSOnCommChange_Click(object sender, EventArgs e)
        {
            CalcTaxOnTDSOnCommChange();
        }

        protected void CalcTaxOnTDSOnCommChange()
        {
            //Code to put zero if nothing is entered in textbox
            if (txtRevisedFreight.Text == "")
                txtRevisedFreight.Text = "0";
            if (txtRevisedOCDA.Text == "")
                txtRevisedOCDA.Text = "0";
            if (txtRevsedOCDC.Text == "")
                txtRevsedOCDC.Text = "0";
            if (txtRevisedServiceTax.Text == "")
                txtRevisedServiceTax.Text = "0";
            if (txtRevisedComm.Text == "")
                txtRevisedComm.Text = "0";

            try
            {
                if (Convert.ToDouble(txtRevisedFreight.Text.Trim() == "" ? "0" : txtRevisedFreight.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised Freight";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevsedOCDC.Text.Trim() == "" ? "0" : txtRevsedOCDC.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised OCDC";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedOCDA.Text.Trim() == "" ? "0" : txtRevisedOCDA.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised OCDA";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedServiceTax.Text.Trim() == "" ? "0" : txtRevisedServiceTax.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised Service Tax";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else if (Convert.ToDouble(txtRevisedComm.Text.Trim() == "" ? "0" : txtRevisedComm.Text.Trim()) < 0)
                {
                    lblStatus.Text = "Enter valid revised Commission";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    // ST = Session["STDCM"] != null ? Convert.ToDouble(Session["STDCM"]) : 0;
                    //Code to get calculate taxes on change of Freight, OCDC and OCDA
                    txtRevisedTotal.Text = Math.Round(Convert.ToDouble(txtRevisedFreight.Text == "" ? "0" : txtRevisedFreight.Text) +
                        Convert.ToDouble(txtRevsedOCDC.Text == "" ? "0" : txtRevsedOCDC.Text) +
                        Convert.ToDouble(txtRevisedServiceTax.Text == "" ? "0" : txtRevisedServiceTax.Text) +
                        Convert.ToDouble(txtRevisedTDSOnComm.Text == "" ? "0" : txtRevisedTDSOnComm.Text) -
                        (Convert.ToDouble(txtRevisedComm.Text == "" ? "0" : txtRevisedComm.Text) +
                        Convert.ToDouble(txtRevisedSTOnComm.Text == "" ? "0" : txtRevisedSTOnComm.Text)), 2).ToString();
                }

                double CurrentTotal = FinalCT + Convert.ToDouble(txtCTot.Text.Trim() == "" ? "0" : txtCTot.Text.Trim());
                double RevisedTotal = FinalRT + Convert.ToDouble(txtRevisedTotal.Text.Trim() == "" ? "0" : txtRevisedTotal.Text.Trim());

                if (RevisedTotal > CurrentTotal)
                {
                    txtDCMAmount.Text = Math.Round((RevisedTotal - CurrentTotal), 2).ToString();
                    txtDCMType.Text = "Debit";
                }
                else if (RevisedTotal < CurrentTotal)
                {
                    txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                    txtDCMType.Text = "Credit";
                }
                else
                {
                    txtDCMAmount.Text = Math.Round((CurrentTotal - RevisedTotal), 2).ToString();
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
