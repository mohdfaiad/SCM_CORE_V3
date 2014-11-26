using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Web.UI.WebControls.WebParts;
using System.Configuration;
using System.Data.SqlClient;
using BAL;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class RateCardMaster : System.Web.UI.Page
    {
        
        RateCardMasterBAL objBAL = new RateCardMasterBAL();
        string errormessage = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RBExAC.Checked = true;
                RBExCC.Checked = true;
                RBExAD.Checked = true;
                RBExFC.Checked = true;
                RBExFN.Checked = true;
                RBExHC.Checked = true;
                RBExSC.Checked = true;

                Session["FlightNumber"] = "";
                Session["AirlineCode"] = "";
                Session["CommCode"] = "";
                Session["AgentCode"] = "";
                Session["Shipper"] = "";
                Session["mode"] = "";

                if (Request.QueryString["cmd"] == "Edit" || Request.QueryString["cmd"] == "View")
                {
                    getRateCardDetails();
                }
            }

            if (Request.QueryString["cmd"] == "Edit")
            {
                
                btnBack.Visible = true;
                btnSave.Text = "Update";
                btnCancel.Visible = false;
                Session["mode"] = "Edit";

            }
            else if (Request.QueryString["cmd"] == "View")
            {
                
                btnBack.Visible = true;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                Session["mode"] = "View";
                disableForView();
            }
            else
            {
                btnSave.Text = "Save";
                btnBack.Visible = false;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                Session["mode"] = "";
            }
            
        }

        # region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRateCardName.Text == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please Enter Rate Card Name ');</SCRIPT>");
                    lblStatus.Text = "Please enter Rate Card name";
                    lblStatus.ForeColor = Color.Blue;
                   // MessageBox.Show("Please Enter FlightID's which is not Operated");
                    txtRateCardName.Focus();
                    return;
                }

                if (txtvalidfrom.Text == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select valid from date');</SCRIPT>");
                    lblStatus.Text = "Please select valid from date";
                    lblStatus.ForeColor = Color.Blue;
                    // MessageBox.Show("Please Enter FlightID's which is not Operated");
                    
                    return;
                }
                if (txtvalidtill.Text == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select Valid till date');</SCRIPT>");
                    lblStatus.Text = "Please select valid till date";
                    lblStatus.ForeColor = Color.Blue;
                    // MessageBox.Show("Please Enter FlightID's which is not Operated");

                    return;
                }

                DateTime dtfrom, dtto;

                try
                {
                    dtfrom = Convert.ToDateTime(txtvalidfrom.Text);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Valid from date format invalid');</SCRIPT>");                   
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                try
                {
                    dtto = Convert.ToDateTime(txtvalidtill.Text);
                }
                catch (Exception ex)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Valid till date format invalid');</SCRIPT>");
                    lblStatus.Text = "Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (dtto < dtfrom)
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Valid to date should be greater than Valid from date');</SCRIPT>");
                    lblStatus.Text = "To date should be greater than From date";
                    lblStatus.ForeColor = Color.Red;
                    // MessageBox.Show("Please Enter FlightID's which is not Operated");

                    return;
                }


                if(RBIncFN.Checked == true && TXTFlightNumber.Text.Trim() == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select Flight number');</SCRIPT>");
                    lblStatus.Text = "Please select Flight number";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                if (RBIncFC.Checked == true && TXTFlightCarrier.Text.Trim() == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Flight carrier');</SCRIPT>");
                    lblStatus.Text = "Please enter Flight carrier";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                if (RBIncHC.Checked == true && TXTHandlingCode.Text.Trim() == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please enter Handling code');</SCRIPT>");
                    lblStatus.Text = "Please enter Handling code";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                if (RBIncAC.Checked == true && TXTAirLineCode.Text.Trim() == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select Airline code');</SCRIPT>");
                    lblStatus.Text = "Please select Airline code";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                if (RBIncCC.Checked == true && TXTIATAComCode.Text.Trim() == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select IATA comm code');</SCRIPT>");
                    lblStatus.Text = "Please select IATA comm code";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                if (RBIncAD.Checked == true && TXTAgentCode.Text.Trim() == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select Agent code');</SCRIPT>");
                    lblStatus.Text = "Please select Agent code";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                if (RBIncSC.Checked == true && TXTShipperCode.Text.Trim() == "")
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select Shipper code');</SCRIPT>");
                    lblStatus.Text = "Please select Shipper code";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
               
                #region Prepare Parameters
                object[] RateCardInfo = new object[20];
                int i = 0;

                //0
                RateCardInfo.SetValue(txtRateCardName.Text, i);
                i++;

                //1
                if (ddlRateCardType.SelectedValue == "Market")
                   
                    RateCardInfo.SetValue("MKT", i);
                else
                    RateCardInfo.SetValue("IATA", i);
                 i++;

                //2
                RateCardInfo.SetValue(Convert.ToDateTime(txtvalidfrom.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                i++;

                //3
                RateCardInfo.SetValue(Convert.ToDateTime(txtvalidtill.Text).ToString("yyyy-MM-dd HH:mm:ss"), i);
                i++;

                //4
                if (ddlStatus.SelectedValue == "Active")
                    RateCardInfo.SetValue("ACT", i);
                else
                    RateCardInfo.SetValue("DFT", i);
                i++;


                RateCardInfo.SetValue(TXTFlightNumber.Text, i);
                i++;

                RateCardInfo.SetValue(TXTFlightCarrier.Text, i);
                i++;

                RateCardInfo.SetValue(TXTHandlingCode.Text, i);
                i++;

                RateCardInfo.SetValue(TXTAirLineCode.Text, i);
                i++;

                RateCardInfo.SetValue(TXTIATAComCode.Text, i);
                i++;

                RateCardInfo.SetValue(TXTAgentCode.Text, i);
                i++;

                RateCardInfo.SetValue(TXTShipperCode.Text, i);
                i++;


                RateCardInfo.SetValue(RBIncFN.Checked, i);
                i++;


                RateCardInfo.SetValue(RBIncFC.Checked, i);
                i++;


                RateCardInfo.SetValue(RBIncHC.Checked, i);
                i++;

                RateCardInfo.SetValue(RBIncAC.Checked, i);
                i++;

                RateCardInfo.SetValue(RBIncCC.Checked, i);
                i++;

                RateCardInfo.SetValue(RBIncAD.Checked, i);
                i++;

                RateCardInfo.SetValue(RBIncSC.Checked, i);
                i++;

                int rateCardSrNo;
                //to Update Rate Card
                if (HidSrNo.Value != "")
                    rateCardSrNo = Convert.ToInt32(HidSrNo.Value);
                //to Save Rate Card
                else
                    rateCardSrNo = 0;

                RateCardInfo.SetValue(rateCardSrNo, i);
                

                #endregion Prepare Parameters
                

                
                int RateCardID = 0;
                string res = "";
                res = objBAL.AddRateCard(RateCardInfo);
                //RateCardID=objBAL.AddRateCard(RateCardInfo);

                if (res != "error" && res != "Rate card name already exists" && res != "Rate card with same details already exists")
                {
                    // ClientScript.RegisterStartupScript(this.GetType(),"Submit", "javascript:alert('Prospect Registered Sucessfully');"); 
                    #region for Master Audit Log
                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    object[] Paramsss = new object[7];
                    int k = 0;

                    //1
                    Paramsss.SetValue("Masters-Rates", k);
                    k++;

                    //2
                    string Value = "Rate Card";
                    Paramsss.SetValue(Value, k);
                    k++;

                    //3
                    Paramsss.SetValue("ADD", k);
                    k++;

                    //4
                    string Msg = "Rate Card Added";
                    Paramsss.SetValue(Msg, k);
                    k++;

                    //5
                    string Desc = "Rate Card Name:" + txtRateCardName.Text;
                    Paramsss.SetValue(Desc, k);
                    k++;

                    //6

                    Paramsss.SetValue(Session["UserName"], k);
                    k++;

                    //7
                    Paramsss.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                    k++;

                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Paramsss);
                    #endregion
                    txtRateCardName.Text = "";
                   ddlRateCardType.SelectedIndex = 0;
                    txtvalidfrom.Text = "";
                    txtvalidtill.Text = "";
                   ddlStatus.SelectedIndex = 0;
                   TXTFlightNumber.Text = "";
                   TXTFlightCarrier.Text = "";
                   TXTHandlingCode.Text = "";
                   TXTAirLineCode.Text = "";
                   TXTIATAComCode.Text = "";
                   TXTAgentCode.Text = "";
                   TXTShipperCode.Text = "";
                   RBExFN.Checked = false;
                   RBIncFN.Checked = false;
                   RBExFC.Checked = false;
                   RBIncFC.Checked = false;
                   RBExHC.Checked = false;
                   RBIncHC.Checked = false;
                   RBExAC.Checked = false;
                   RBIncAC.Checked = false;
                   RBExCC.Checked = false;
                   RBIncCC.Checked = false;
                   RBExAD.Checked = false;
                   RBIncAD.Checked = false;
                   RBExSC.Checked = false;
                   RBIncSC.Checked = false;
                   HidSrNo.Value = "";


                    //  if (!ClientScript.IsOnSubmitStatementRegistered("confirm"))
                    //if(Request.QueryString["cmd"] == "Edit")
                    //    ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('"+ res +"');</SCRIPT>");
                    //else
                   //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                   lblStatus.Text = res;
                   lblStatus.ForeColor = Color.Green;



                    //ClientScript.RegisterOnSubmitStatement(this.GetType(), "Submit", "alert('Prospect Registered Sucessfully')"); 
                }
                else
                {
                    
                    //if (Request.QueryString["cmd"] == "Edit")
                    //    ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Failed To Update RateCard');</SCRIPT>");
                    //else
                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Red;
                }


                //btnSubmit.Attributes.Add("onClick", "javascript:alert('Prospect Registerd Sucessfully');"); 

            }
            catch (Exception ex)
            {

            }

        }
        # endregion btnSave_Click

        #region getRateCardDetails
        public void getRateCardDetails()
        {
            string rateCardName = Request.QueryString["RCName"].ToString();
            DataSet dsRateCard = new DataSet();
            dsRateCard = objBAL.GetRateCard(rateCardName);
            fillRateCardDetails(dsRateCard);
        }
        #endregion getRateCardDetails

        #region fillRateCardDetails
        public void fillRateCardDetails(DataSet dsRateDetails)
        {
            //Code to fill details of rate Card from dataset
            HidSrNo.Value = dsRateDetails.Tables[1].Rows[0]["RateCardSrNo"].ToString();
            txtRateCardName.Text = dsRateDetails.Tables[0].Rows[0]["RateCardName"].ToString();
            if (dsRateDetails.Tables[0].Rows[0]["RateCardType"].ToString() == "MKT")
                ddlRateCardType.SelectedIndex = 1;
            else
                ddlRateCardType.SelectedIndex = 0;
            if (dsRateDetails.Tables[0].Rows[0]["Status"].ToString() == "ACT")
                ddlStatus.SelectedIndex = 0;
            else
                ddlStatus.SelectedIndex = 1;
            txtvalidfrom.Text = dsRateDetails.Tables[0].Rows[0]["StartDate"].ToString();
            txtvalidtill.Text = dsRateDetails.Tables[0].Rows[0]["EndDate"].ToString();

            TXTFlightNumber.Text = dsRateDetails.Tables[1].Rows[0]["ParamValue"].ToString();
            Session["FlightNumber"] = dsRateDetails.Tables[1].Rows[0]["ParamValue"].ToString();
            TXTFlightCarrier.Text = dsRateDetails.Tables[1].Rows[1]["ParamValue"].ToString();
            TXTHandlingCode.Text = dsRateDetails.Tables[1].Rows[2]["ParamValue"].ToString();
            TXTAirLineCode.Text = dsRateDetails.Tables[1].Rows[3]["ParamValue"].ToString();
            Session["AirlineCode"] = dsRateDetails.Tables[1].Rows[3]["ParamValue"].ToString();
            TXTIATAComCode.Text = dsRateDetails.Tables[1].Rows[4]["ParamValue"].ToString();
            Session["CommCode"] = dsRateDetails.Tables[1].Rows[4]["ParamValue"].ToString();
            TXTAgentCode.Text = dsRateDetails.Tables[1].Rows[5]["ParamValue"].ToString();
            Session["AgentCode"] = dsRateDetails.Tables[1].Rows[5]["ParamValue"].ToString();
            TXTShipperCode.Text = dsRateDetails.Tables[1].Rows[6]["ParamValue"].ToString();
            Session["Shipper"] = dsRateDetails.Tables[1].Rows[6]["ParamValue"].ToString();

            if (dsRateDetails.Tables[1].Rows[0]["IsInclude"].ToString() == "False")
                RBExFN.Checked = true;
            else
                RBIncFN.Checked = true;
            if (dsRateDetails.Tables[1].Rows[1]["IsInclude"].ToString() == "False")
                RBExFC.Checked = true;
            else
                RBIncFC.Checked = true;
            if (dsRateDetails.Tables[1].Rows[2]["IsInclude"].ToString() == "False")
                RBExHC.Checked = true;
            else
                RBIncHC.Checked = true;
            if (dsRateDetails.Tables[1].Rows[3]["IsInclude"].ToString() == "False")
                RBExAC.Checked = true;
            else
                RBIncAC.Checked = true;
            if (dsRateDetails.Tables[1].Rows[4]["IsInclude"].ToString() == "False")
                RBExCC.Checked = true;
            else
                RBIncCC.Checked = true;
            if (dsRateDetails.Tables[1].Rows[5]["IsInclude"].ToString() == "False")
                RBExAD.Checked = true;
            else
                RBIncAD.Checked = true;
            if (dsRateDetails.Tables[1].Rows[6]["IsInclude"].ToString() == "False")
                RBExSC.Checked = true;
            else
                RBIncSC.Checked = true;



        }
        #endregion fillRateCardDetails

        # region btnCancel_Click
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtRateCardName.Text = "";
            ddlRateCardType.SelectedIndex = 0;
            txtvalidfrom.Text = "";
            txtvalidtill.Text = "";
            ddlStatus.SelectedIndex = 0;
            TXTFlightNumber.Text = "";
            TXTFlightCarrier.Text = "";
            TXTHandlingCode.Text = "";
            TXTAirLineCode.Text = "";
            TXTIATAComCode.Text = "";
            TXTAgentCode.Text = "";
            TXTShipperCode.Text = "";
            RBExFN.Checked = false;
            RBIncFN.Checked = false;
            RBExFC.Checked = false;
            RBIncFC.Checked = false;
            RBExHC.Checked = false;
            RBIncHC.Checked = false;
            RBExAC.Checked = false;
            RBIncAC.Checked = false;
            RBExCC.Checked = false;
            RBIncCC.Checked = false;
            RBExAD.Checked = false;
            RBIncAD.Checked = false;
            RBExSC.Checked = false;
            RBIncSC.Checked = false;

        }

        # endregion btnCancel_Click

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListRateCard.aspx");
        }

        #region disable controls for view
        protected void disableForView()
        {
            txtRateCardName.Enabled = false;
            ddlRateCardType.Enabled = false;
            ddlStatus.Enabled = false;
            txtvalidfrom.Enabled = false;
            btnValidFrom.Enabled = false;
            txtvalidtill.Enabled = false;
            imgBtnValidTill.Enabled = false;
            TXTFlightNumber.Enabled = false;
            
            TXTFlightCarrier.Enabled = false;
            //ImageButton4.Enabled = false;
            TXTHandlingCode.Enabled = false;
            TXTAirLineCode.Enabled = false;
            //ImageButton8.Enabled = false;
            TXTIATAComCode.Enabled = false;
            //ImageButton6.Enabled = false;
            TXTAgentCode.Enabled = false;
            //ImageButton7.Enabled = false;
            TXTShipperCode.Enabled = false;
            //ImageButton10.Enabled = false;

            RBExAC.Enabled = false;
            RBExAD.Enabled = false;
            RBExCC.Enabled = false;
            RBExFC.Enabled = false;
            RBExFN.Enabled = false;
            RBExHC.Enabled = false;
            RBExSC.Enabled = false;
            RBIncAC.Enabled = false;
            RBIncAD.Enabled = false;
            RBIncCC.Enabled = false;
            RBIncFC.Enabled = false;
            RBIncFN.Enabled = false;
            RBIncHC.Enabled = false;
            RBIncSC.Enabled = false;


        }
        #endregion disable controls for view
    }
}
