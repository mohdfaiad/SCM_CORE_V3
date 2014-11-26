using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using BAL;
namespace ProjectSmartCargoManager
{
    public partial class SpotRateMaster : System.Web.UI.Page
    {
        BALSpotRate BlSpotRate = new BALSpotRate();
        AgentBAL objBLL = new AgentBAL();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    GetCommodity();
                    txtAuthorisedBy.Text = Session["UserName"].ToString();
                    LoadAgentDropdown(); 
                    txtStation.Text = Session["station"].ToString();

                    if (Request.QueryString["cmd"] == "AWBNo")
                        FillSpotRateDetails("");                    
                }

                if (Session["awbPrefix"] != null)
                {
                   txtPrefix.Text = Session["awbPrefix"].ToString();
                }
                else
                {
                    MasterBAL objBal = new MasterBAL();
                    Session["awbPrefix"] = objBal.awbPrefix();
                    txtPrefix.Text = Session["awbPrefix"].ToString();
                }

                if (Request.QueryString["cmd"] == "AWBNo")
                {
                    BtnList.Visible = false;
                    btnClear.Visible = false;
                    btnSave.Text = "Update";
                }
                else
                {
                    BtnList.Visible = true;
                    btnClear.Visible = true;
                    btnSave.Text = "Save";
                }
                if (Request.QueryString["AppStat"] == "A")
                {
                    btnSave.Visible = false;
                }
                
            }
            catch (Exception ex)
            { }
        }
        #endregion Load
        //To show commodity in a dropdown
        #region GetCommodity
        public void GetCommodity()
        {
            try
            {
                DataSet dsResult = new DataSet();
                //string errormessage = "";

                //string level = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text);
                dsResult = da.SelectRecords("SpGetCommodity");
                //dsResult.Tables[0].Rows.Add("Select");


                // DropDownList ddl = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable"));

                ddlCommodity.DataSource = dsResult;
                ddlCommodity.DataMember = dsResult.Tables[0].TableName;
                ddlCommodity.DataValueField = dsResult.Tables[0].Columns[0].ColumnName;
                ddlCommodity.DataTextField = dsResult.Tables[0].Columns[0].ColumnName;
                ddlCommodity.DataBind();
            }
            catch (Exception e) { }

        }
        #endregion GetCommodity
        //Save data in a tblspotrate table
        #region Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string AWBNumber = "",Origin="",Destination="",AgentName="",currency="",AgentCode="",FlightNumber="",Commodity="",SpotCategory="",
            station="",FWDName="",Remarks="",Reason="",SpotRateType="";
            string IssuedBy = "", AuthorisedBy = "",specialapproval="",UpdatedBy="";
            double Weight, volume,thresholdLimit,spotRate;
            DateTime FltDate, ReqDate, IssueDate, AuthorisedDate,validfrom,validto;
            int commissionable = 0, Noncommissionable = 0 ;

            try
            {
                if (txtawbnumber.Text == "")
                {
                    lblStatus.Text = "Please Enter AWB Number";
                    lblStatus.ForeColor = Color.Red;
                    return; 
                }
                else
                {
                    AWBNumber = txtPrefix.Text.Trim() + txtawbnumber.Text.Trim();
                }
                
                Origin = txtOrigin.Text;
                Destination = txtDestination.Text;
                AgentName=ddlagentname.SelectedItem.Text;
                AgentCode = ddlAgentCode.SelectedItem.Text;
                spotRate =Convert.ToDouble(txtSpotRate.Text);
                currency = txtCurrency.Text;
                if (rbCommisionable.Checked)
                {
                    commissionable = 1;
                }
                else
                {
                commissionable = 0;
                }
                if (rbNonCommissionable.Checked)
                {
                    Noncommissionable = 1;
                }
                else
                {
                    Noncommissionable = 0; 
                }
                FlightNumber = txtFlightNumber.Text;
                if (ddlCommodity.SelectedItem.Text.Trim() == "Select")
                {
                    lblStatus.Text = "Please Select Commodity Code";
                    lblStatus.ForeColor=Color.Red;
                    return; 
                }
                Commodity = ddlCommodity.SelectedItem.Text;
                if (ddlSpotCategory.SelectedItem.Text == "Select")
                {
                    lblStatus.Text = "Please select Spot Category";
                    lblStatus.ForeColor = Color.Red;
                    return; 
                }
                if (ddlSpotCategory.SelectedItem.Text != "Flat Charge" && ddlWtCategory.SelectedIndex==0)
                {
                    lblStatus.Text = "Please select Weight Category";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                SpotCategory = ddlSpotCategory.SelectedItem.Text;
                station = txtStation.Text;
                FWDName = txtFWDName.Text;
                Remarks = txtremarks.Text;
                Reason = txtReason.Text;
                IssuedBy = txtIssuedBy.Text;
                AuthorisedBy=txtAuthorisedBy.Text;
                
                Weight = Convert.ToDouble(txtWeight.Text.Trim());
                volume = Convert.ToDouble(txtVolume.Text.Trim() == "" ? "0" : txtVolume.Text.Trim());
                thresholdLimit=Convert.ToDouble(txtthreshold.Text);
                if (txtFlightDate.Text == "")
                {
                    lblStatus.Text = "Please Enter Flight Date";
                    lblStatus.ForeColor = Color.Red;
                    return;
 
                }
                //SpotCategory = ddlSpotCategory.SelectedItem.Text;    
                FltDate = DateTime.ParseExact(txtFlightDate.Text,"dd-MM-yyyy",null);
                if (txtreqdate.Text == "")
                {
                    lblStatus.Text = "Please Enter Request Date";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    ReqDate = DateTime.ParseExact(txtreqdate.Text, "dd-MM-yyyy", null);
                }
                if (txtIssueDate.Text == "")
                {
                    IssueDate = DateTime.Parse("01-01-2000");
                }
                else
                {
                    IssueDate = DateTime.ParseExact(txtIssueDate.Text, "dd-MM-yyyy", null);
                }
                if (txtAuthorisedDate.Text == "")
                {
                    AuthorisedDate = DateTime.Parse("01-01-2000");
                }
                else
                {
                    AuthorisedDate = DateTime.ParseExact(txtAuthorisedDate.Text, "dd-MM-yyyy", null);
                }
                if (txtValidFrom.Text == "")
                {
                    validfrom = DateTime.Parse("01-01-2000");
                }
                else
                {
                    validfrom = DateTime.ParseExact(txtValidFrom.Text, "dd-MM-yyyy", null);
                }
                if (txtValidTo.Text == "")
                {
                    validto = DateTime.Parse("01-01-2000") ;
                }
                else
                {
                    validto = DateTime.ParseExact(txtValidTo.Text, "dd-MM-yyyy", null);
                }
                if (chkSpecialArrival.Checked)
                {
                    specialapproval = "Yes";
                }
                else
                {
                    specialapproval = "No"; 
                }
                UpdatedBy = Session["UserName"].ToString();

                string WtCategory = "";
                if (ddlWtCategory.SelectedIndex != 0)
                    WtCategory = ddlWtCategory.SelectedValue;

                DataSet blnRes = BlSpotRate.SaveSpotRate(AWBNumber,Origin,Destination, AgentName, currency, AgentCode, 
                    FlightNumber, Commodity, SpotCategory, station, FWDName, Remarks, Reason, commissionable,IssuedBy, 
                    AuthorisedBy, specialapproval, Weight, volume, thresholdLimit, spotRate, FltDate, ReqDate, IssueDate, 
                    AuthorisedDate, validfrom, validto,Noncommissionable,UpdatedBy,
                    Convert.ToDateTime(Session["IT"].ToString()),WtCategory);

                
                if (blnRes.Tables[0].Rows.Count > 0)
                {
                    if (blnRes.Tables[0].Rows[0]["Column1"].ToString() == "Approved")
                    {
                        lblStatus.Text = "Spot Rate already Approved for this AWB.";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                    else
                    {
                        txtSpotrateid.Text = blnRes.Tables[0].Rows[0]["Column1"].ToString();
                        btnSave.Visible = false;
                        lblStatus.Text = "Spot Rate Saved Successfully";
                        lblStatus.ForeColor = Color.Green;

                        #region For Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        object[] Params = new object[7];
                        int i = 0;

                        //1
                        Params.SetValue("Spot Rate", i);
                        i++;

                        //2
                        Params.SetValue("AWB Number:"+AWBNumber, i);
                        i++;

                        //3

                        Params.SetValue("ADD", i);
                        i++;

                        //4

                        Params.SetValue("", i);
                        i++;


                        //5

                        Params.SetValue("", i);
                        i++;

                        //6

                        Params.SetValue(Session["UserName"], i);
                        i++;

                        //7
                        Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), i);
                        i++;


                        #endregion Prepare Parameters
                        ObjMAL.AddMasterAuditLog(Params);
                        #endregion

                        #region Send links to approve or reject Spot Rate

                        
                        
                        LoginBL lbal=new LoginBL();
                        MasterBAL objBal = new MasterBAL();
                        string Subject = "Spot Rate Approval/Rejection", ToEmail = "", Msg = "", Approve = "", Reject = "";
                        string AWBPrefix = Session["awbPrefix"] != null ? Session["awbPrefix"].ToString() : objBal.awbPrefix();
                        string SpotRateID = cls_Encode_Decode.StringCipher.Encrypt(txtSpotrateid.Text, AWBPrefix);
                        Approve = "/SpotRateApprovalStatus.aspx?Approval=Approved&spotrateid=" + SpotRateID;
                        Reject = "/SpotRateApprovalStatus.aspx?Approval=Rejected&spotrateid=" + SpotRateID;

                        Msg += "Dear Approver," + "\n\n A Spot Rate approval is requested for following AWB: \n\n";
                        Msg +="AWB #:"+txtPrefix.Text+"-"+txtawbnumber.Text.ToString()+"\r\n";
                        Msg+="Agent Code:"+ddlAgentCode.SelectedItem.Text.ToString()+"\r\n";
                        Msg+="Agent Name:"+ddlagentname.SelectedItem.Text.ToString()+"\r\n";
                        Msg+="Origin: "+txtOrigin.Text+"\r\n";
                        Msg+="Destination:"+txtDestination.Text+"\r\n";
                        Msg+="Chargeable Wt:"+txtWeight.Text+"\r\n";
                        Msg+="Commodity Code:"+ddlCommodity.SelectedItem.Text.ToString()+"\r\n";
                        Msg+="Applied Rate: "+txtAppliedRate.Text+"\r\n";
                        Msg+="Requested Spot Rate:"+txtSpotRate.Text.ToString()+"\r\n";
                        Msg += "Spot Category:"+ddlSpotCategory.Text+"\r\n";
                        Msg += "Spot Rate Id : " + txtSpotrateid.Text+"\n\n";                        
                        Msg += "To Approve Spot Rate Click On Following Link.\n";
                        Msg += Request.Url.GetLeftPart(UriPartial.Authority) + Approve;
                        Msg += "\n\nTo Reject Spot Rate Click On Following Link.\n";
                        Msg+=Request.Url.GetLeftPart(UriPartial.Authority) + Reject+"\n";
                        Msg += "\r\n Note: This is a system generated email. Please do not reply. If you were an unintended recipient kindly delete this email,";
                        Msg +="\r\nThanks"+"\r\n"+txtIssuedBy.Text.ToString();
                        try
                        {
                            ToEmail = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "SpotRateLink");//lbal.GetMasterConfiguration("SpotRateLink");
                        }
                        catch (Exception ex) 
                        { }
                        
                        BALEmailID ObjEmail = new BALEmailID();
                        DataSet ds = new DataSet("dsEmail");

                        try
                        {
                            ds = ObjEmail.GetEmail(txtOrigin.Text, txtDestination.Text, "SPOTRATE", "", "");
                            if (ds != null)
                            {
                                ToEmail = ToEmail + "," + ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();

                            }

                        }
                        catch (Exception ex)
                        { }
                        finally
                        {
                            ObjEmail = null;
                            ds = null;
                        }
                        bool res = cls_BL.addMsgToOutBox(Subject, Msg, "", ToEmail.Trim(','));
                        objBal = null;
                        
                        #endregion
                        
                        return;
                    }
                }
                else
                {
                    lblStatus.Text = "Spot Rate Not Saved Please try again.";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Please Try Again.. Record not Saved.";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }
        #endregion Save
        //autopopulate agent code
        #region AutopopulateAgentCode
        protected void txtAgenntCode_TextChanged(object sender, EventArgs e)
        {
            try
            {

                //string[] orgdest = new ConBooking().GetOrgDest();

                //string con = Global.GetConnectionString();
                //// SqlConnection con = new SqlConnection("connection string");
                //SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentName from dbo.AgentMaster where AgentCode ='" + txtAgenntCode.Text.Trim() + "'", con);
                //DataSet ds = new DataSet();
                //dad.Fill(ds);

                //if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                //{
                //    txtAgenntCode.Text = ds.Tables[0].Rows[0][0].ToString();
                //}
                //else
                //{
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Agent code invalid.";
                //    return;
                //}


            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :" + ex.Message;
            }

        }
        #endregion AutopopulateAgentCode
        //autopopulate agent name
        #region GetAgent
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAgentCode(string prefixText, int count)
        {

            string[] orgdest = new ConBooking_GHA().GetOrgDest();

            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%') and Station='" + orgdest[0] + "'", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
        }
        #endregion GetAgent
        //autopopulate station code
        #region newMethod
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetStation(string prefixText, int count)
        {
            string con = Global.GetConnectionString();
            // SqlConnection con = new SqlConnection("connection string");
            //SELECT FlightID from dbo.AirlineSchedule where FlightID like '"+ prefixText +"%' or FlightID like '"+ prefixText +"%'

            SqlDataAdapter dad = new SqlDataAdapter("SELECT AirportCode from AirportMaster where AirportName like '" + prefixText + "%' or AirportCode like '" + prefixText + "%'", con);
            DataSet ds = new DataSet();
            dad.Fill(ds);
            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());

            }

            return list.ToArray();
        }

        #endregion
        //List data according to parameter
        #region List
        protected void BtnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = ""; 
                string AWBNumber = txtawbnumber.Text.Trim();
                DataSet ds = BlSpotRate.GetAWBDetails(AWBNumber, txtPrefix.Text.Trim());
                
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables.Count > 2)
                    {
                        if (ds.Tables[2].Rows.Count > 0) // Invoice Generated..
                        {
                            lblStatus.Text = "AWB already invoiced...Spot Rate cannot be generated / modified !!!";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                    }
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0) //Spot rate already generated, Populate existing details...
                        {
                            FillSpotRateDetails(txtPrefix.Text.Trim() + "-" + txtawbnumber.Text.Trim());
                            return;
                        }
                    }
                    
                    if (ds.Tables[0].Rows.Count > 0) // AWB Details from AWBSummaryMaster
                    {
                        txtOrigin.Text = ds.Tables[0].Rows[0]["origincode"].ToString();
                        txtDestination.Text = ds.Tables[0].Rows[0]["DestinationCode"].ToString();
                        ddlAgentCode.SelectedItem.Text = ds.Tables[0].Rows[0]["AgentCode"].ToString();
                        ddlagentname.SelectedItem.Text = ds.Tables[0].Rows[0]["AgentName"].ToString();
                        txtFlightNumber.Text = ds.Tables[0].Rows[0]["FltNumber"].ToString();
                        ddlCommodity.SelectedItem.Text = ds.Tables[0].Rows[0]["CommodityCode"].ToString();      
                        DateTime datedt = (DateTime.ParseExact(ds.Tables[0].Rows[0]["FltDate"].ToString(), "dd/MM/yyyy", null));
                        txtFlightDate.Text = datedt.ToString("dd-MM-yyyy");
                        txtCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                        txtStation.Text = ds.Tables[0].Rows[0]["origincode"].ToString();
                        txtWeight.Text = ds.Tables[0].Rows[0]["GrossWeight"].ToString();
                        txtVolume.Text = ds.Tables[0].Rows[0]["VolumetricWeight"].ToString();
                        txtAppliedRate.Text = ds.Tables[0].Rows[0]["RatePerKg"].ToString(); 
                        //Initaiting Default Values..
                        if (Session["DefaultCurrency"] == null)
                            LoadCurrencyType();
                        txtAppliedRate.Text = ds.Tables[0].Rows[0]["RatePerKg"].ToString(); 
                        txtCurrency.Text = Convert.ToString(Session["DefaultCurrency"]);
                        txtreqdate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd-MM-yyyy");
                        txtIssueDate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd-MM-yyyy");
                        txtAuthorisedDate.Text = Convert.ToDateTime(Session["IT"]).ToString("dd-MM-yyyy");
                        txtValidFrom.Text = Convert.ToDateTime(Session["IT"]).ToString("dd-MM-yyyy");
                        txtValidTo.Text = Convert.ToDateTime(Session["IT"]).ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        lblStatus.Text = "AWB is not Booked Please book for AWBNumber";
                        lblStatus.ForeColor = Color.Red;
                        return;
                    }
                }

            }
            catch (Exception ex)
            { 
            }

        }
        #endregion List
        //clear button funtion
        #region ClearButton
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                //clear(); 
                Response.Redirect("~/SpotRateMaster.aspx");
            }
            catch (Exception ex)
            { }
        }
        #endregion ClearButton
        //cleaar funtion
        #region clear
        public void clear()
        {
            try
            {
                txtFlightDate.Text = "";
                //txtAgenntCode.Text = "";
                //txtAgentName.Text = "";
                txtIssuedBy.Text = "";
                txtIssueDate.Text = "";
                txtFWDName.Text = "";
                txtFlightNumber.Text = "";
                txtOrigin.Text = "";
                //txtPrefix.Text = "";
                txtReason.Text = "";
                txtremarks.Text = "";
                txtSpotrateid.Text = "";
                txtValidFrom.Text = "";
                txtValidTo.Text = "";
                txtVolume.Text = "";
                txtWeight.Text = "";
                lblStatus.Text = "";
                txtDestination.Text = "";
                txtawbnumber.Text = "";
                txtSpotRate.Text = "";
                txtthreshold.Text = "";
                txtreqdate.Text = "";
                txtFWDName.Text = "";
                txtreqdate.Text = "";
                btnSave.Visible = true;
                ddlAgentCode.SelectedIndex = -1;
                ddlagentname.SelectedIndex = -1;
                ddlCommodity.SelectedIndex = -1;
                txtCurrency.Text = "";
                txtStation.Text = "";
  
   
  
            }
            catch (Exception ex)
            { }
        }
        #endregion Clear
        //spot rate categories
        #region SpotCategory
        protected void ddlSpotCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSpotCategory.SelectedItem.Text.Trim() == "Flat Charge")
            {
                rbNonCommissionable.Checked = true;
                rbCommisionable.Checked = false;   
            }
            else if (ddlSpotCategory.SelectedItem.Text.Trim() == "Per KG")
            {
                rbCommisionable.Checked = true;
                rbNonCommissionable.Checked = false;
            }
        }
        #endregion SpotCategory
        //add agent to dropdown
        #region Load Agent Dropdown
        public void LoadAgentDropdown()
        {
            try
            {
                DataSet ds = objBLL.GetAgentList(Convert.ToString(Session["AgentCode"]));
                if (ds != null)
                {
                    ddlAgentCode.DataSource = ds;
                    ddlAgentCode.DataMember = ds.Tables[0].TableName;
                    ddlAgentCode.DataTextField = "AgentCode";
                    ddlAgentCode.DataValueField = "AgentCode";
                    ddlAgentCode.DataBind();


                    ddlagentname.DataSource = ds;
                    ddlagentname.DataMember = ds.Tables[0].TableName;
                    ddlagentname.DataTextField = "AgentName";
                    ddlagentname.DataValueField = "AgentName";
                    ddlagentname.DataBind();
                    ddlagentname.Items.Add("select");   
                    // ddlAgtCode.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion Load Grid Material Detail

        protected void btnRoute_Click(object sender, EventArgs e)
        {

        }

        protected void FillSpotRateDetails(string AWBNo)
        {
            try
            {
                lblStatus.Text = "";
                string AWBNumber = string.Empty,spotID="";

                if (AWBNo == "")
                { 
                    AWBNumber = Request.QueryString["AWBNumber"]; 
                    spotID = Request.QueryString["SpotID"].ToString();
                }
                else
                    AWBNumber = AWBNo;

                DataSet ds = BlSpotRate.GetSpotRateDetails(AWBNumber,spotID);

                if (ds != null)
                {
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            lblStatus.Text = "AWB already invoiced...Spot Rate cannot be updated!";
                            lblStatus.ForeColor = Color.Red;
                            btnSave.Visible = false;
                            //return;
                        }
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtawbnumber.Text = ds.Tables[0].Rows[0]["AWBNumber"].ToString();
                        txtSpotrateid.Text = ds.Tables[0].Rows[0]["SpotRateId"].ToString();
                        txtOrigin.Text = ds.Tables[0].Rows[0]["origin"].ToString();
                        txtDestination.Text = ds.Tables[0].Rows[0]["Destination"].ToString();
                        ddlAgentCode.SelectedItem.Text = ds.Tables[0].Rows[0]["AgentCode"].ToString();
                        ddlagentname.SelectedItem.Text = ds.Tables[0].Rows[0]["AgentName"].ToString();
                        txtWeight.Text = ds.Tables[0].Rows[0]["Weight"].ToString();
                        txtVolume.Text = ds.Tables[0].Rows[0]["Volume"].ToString();
                        txtFlightNumber.Text = ds.Tables[0].Rows[0]["FlightNumber"].ToString();
                        
                        txtFlightDate.Text = ds.Tables[0].Rows[0]["FlightDate"].ToString();

                        ddlCommodity.SelectedItem.Text = ds.Tables[0].Rows[0]["Commodity"].ToString();
                        ddlSpotCategory.SelectedItem.Text = ds.Tables[0].Rows[0]["SpotRateCategory"].ToString();
                        txtSpotRate.Text = ds.Tables[0].Rows[0]["SpotRate"].ToString();
                        if (ds.Tables[0].Rows[0]["Commissionable"].ToString() == "True")
                            rbCommisionable.Checked = true;
                        if (ds.Tables[0].Rows[0]["NonCommissionable"].ToString() == "True")
                            rbNonCommissionable.Checked = true;
                        txtthreshold.Text = ds.Tables[0].Rows[0]["ThresholdLimit"].ToString();
                        txtCurrency.Text = ds.Tables[0].Rows[0]["Currency"].ToString();
                        txtStation.Text = ds.Tables[0].Rows[0]["Station"].ToString();
                        
                        txtreqdate.Text = ds.Tables[0].Rows[0]["RequestDate"].ToString();
                        txtFWDName.Text = ds.Tables[0].Rows[0]["FwdName"].ToString();
                        txtremarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                        if (ds.Tables[0].Rows[0]["SpecialApproval"].ToString() == "Yes")
                            chkSpecialArrival.Checked = true;
                        txtReason.Text = ds.Tables[0].Rows[0]["Reason"].ToString();
                        txtremarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                        txtIssuedBy.Text = ds.Tables[0].Rows[0]["IssuedBy"].ToString();
                        
                        txtIssueDate.Text = ds.Tables[0].Rows[0]["IssueDate"].ToString();
                        txtAuthorisedBy.Text = ds.Tables[0].Rows[0]["AuthorisedBy"].ToString();
                        
                        txtAuthorisedDate.Text = ds.Tables[0].Rows[0]["AuthorisedDate"].ToString();
           
                        txtValidFrom.Text = ds.Tables[0].Rows[0]["ValidFrom"].ToString();
                       
                        txtValidTo.Text = ds.Tables[0].Rows[0]["ValidTo"].ToString();

                        ddlWtCategory.Text = ds.Tables[0].Rows[0]["WtCategory"].ToString();

                        if (!ds.Tables[0].Rows[0]["Aproval"].ToString().Equals("New", StringComparison.OrdinalIgnoreCase))
                        {
                            lblStatus.Text = "Spot Rate Already " + ds.Tables[0].Rows[0]["Aproval"].ToString();
                            lblStatus.ForeColor = Color.Green;
                            btnSave.Visible = false;
                        }
                    }
                    else
                    {
                       
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void LoadCurrencyType()
        {
            DataSet ds = null;
            BookingBAL objBLL = new BookingBAL();
            string DefaultCurrency = string.Empty;

            try
            {
                if (objBLL.GetAirpotCurrency(Convert.ToString(Session["Station"]), ref ds))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DefaultCurrency = ds.Tables[0].Rows[0]["BookingCurrrency"].ToString().Length > 1 ? ds.Tables[0].Rows[0]["BookingCurrrency"].ToString() : "INR";
                        Session["DefaultCurrency"] = DefaultCurrency;
                    }
                }
            }
            catch (Exception ex) { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                objBLL = null;
            }
        }
    }
}
