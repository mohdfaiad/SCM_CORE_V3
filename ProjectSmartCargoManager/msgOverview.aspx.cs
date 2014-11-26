using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BAL;
using QID.DataAccess;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class msgOverview1 : System.Web.UI.Page
    {
        static string AgentCode = "";
        MessagingConsole_BAL OBJds = new MessagingConsole_BAL();
        SQLServer db = new SQLServer(Global.GetConnectionString());

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    AgentCode = Convert.ToString(Session["ACode"]);

                    int RoleId = Convert.ToInt32(Session["RoleID"]);

                    if (RoleId == 1 && AgentCode != "")
                    {
                        // txtAgentCode.Text = AgentCode;
                        //txtAgentCode.Enabled = false;
                    }
                    LoadingDropdownlist(ddlMsgType, ddlPartnerType);
                    multiview_msgcenter.ActiveViewIndex = -1;
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region AutoCompleteMethod
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetPartnerType(string prefixText, int count, string contextKey)
        {
            string con = Global.GetConnectionString();

            SqlDataAdapter dad = new SqlDataAdapter("SELECT PartnerCode from tblPartnerMaster where PartnerCode like '" + prefixText + "%' and PartnerType='" + contextKey + "'", con);
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

        #region Loading Dropdownlist

        public void LoadingDropdownlist(DropDownList ddmsgtype, DropDownList ddptype)
        {
            DataSet ds = new DataSet();
            SQLServer db = new SQLServer(Global.GetConnectionString());

            try
            {
                string query = "Select distinct MessageName from tblMessageMaster; select distinct PartnerType from tblPartnerMaster";
                ds = db.GetDataset(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                        {
                            ddmsgtype.DataSource = ds.Tables[0];
                            ddmsgtype.DataTextField = "MessageName";
                            ddmsgtype.DataValueField = "MessageName";
                            ddmsgtype.DataBind();
                            ddmsgtype.Items.Insert(0, "Select");


                            ddptype.DataSource = ds.Tables[1];
                            ddptype.DataTextField = "PartnerType";
                            ddptype.DataValueField = "PartnerType";
                            ddptype.DataBind();
                            ddptype.Items.Insert(0, "Select");

                        }
                    }

                }



            }
            catch (Exception ex)
            { }




        }
        #endregion

        #region Server Side Validations
        protected void ddlMsgType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                txtAWBNumber.Text = "";
                txtFlightFromdate0.Text = "";
                txtFlightNo.Text = "";
                txtmailId1.Text = "";
                txtPartnerCode.Text = "";
                ddlPartnerType.SelectedIndex = 0;
                ddlCommType.SelectedIndex = 0;
                multiview_msgcenter.ActiveViewIndex = -1;
                if (ddlMsgType.SelectedItem.Text == "FFR" || ddlMsgType.SelectedItem.Text == "FFA" || ddlMsgType.SelectedItem.Text == "FWB")
                {

                    lblawbnum.Visible = true;
                    txtAWBNumber.Visible = true;
                    btnList.Visible = true;
                    btnclear.Visible = true;
                    lblFlgtdate.Visible = false;
                    lblFlightNo.Visible = false;
                    txtFlightNo.Visible = false;
                    txtFlightFromdate0.Visible = false;
                    chkHAWBnew.Visible = false;
                }
                if (ddlMsgType.SelectedItem.Text == "FHL")
                {
                    lblawbnum.Visible = true;
                    txtAWBNumber.Visible = true;
                    lblFlgtdate.Visible = false;
                    lblFlightNo.Visible = false;
                    txtFlightNo.Visible = false;
                    txtFlightFromdate0.Visible = false;
                    chkHAWBnew.Visible = true;
                    btnList.Visible = true;
                    btnclear.Visible = true;
                }
                if (ddlMsgType.SelectedItem.Text == "FBL" || ddlMsgType.SelectedItem.Text == "FFM")
                {
                    lblawbnum.Visible = false;
                    txtAWBNumber.Visible = false;
                    chkHAWBnew.Visible = false;
                    lblFlgtdate.Visible = true;
                    lblFlightNo.Visible = true;
                    txtFlightNo.Visible = true;
                    txtFlightFromdate0.Visible = true;
                    btnList.Visible = true;
                    btnclear.Visible = true;

                }
                if (ddlMsgType.SelectedItem.Text == "Select")
                {
                    lblawbnum.Visible = false;
                    txtAWBNumber.Visible = false;
                    btnList.Visible = false;
                    btnclear.Visible = false;
                    lblFlgtdate.Visible = false;
                    lblFlightNo.Visible = false;
                    txtFlightNo.Visible = false;
                    txtFlightFromdate0.Visible = false;
                    chkHAWBnew.Visible = false;

                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Button List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                //btnclear_Click(sender, e);
                lblStatus.Text = "";

                DataSet ds = new DataSet();

                //string FlightDt = FlightDate.ToString();

                string POL = Session["Station"].ToString();

                #region FBL

                if (ddlMsgType.SelectedItem.Text == "FBL")
                {
                    if (txtFlightNo.Text != "" && txtFlightFromdate0.Text != "")
                    {
                        string FlightNumber = txtFlightNo.Text.Trim();
                        DateTime FlightDate = DateTime.ParseExact(txtFlightFromdate0.Text, "dd/MM/yyyy", null);

                        ds = OBJds.GetAWBDetails(POL, FlightNumber, FlightDate);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    gdvULDLoadPlanAWB.DataSource = ds.Tables[1];
                                    gdvULDLoadPlanAWB.DataBind();
                                    multiview_msgcenter.ActiveViewIndex = 0;
                                    btnSendMsg.Visible = true;
                                }
                                else
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "No Records Found";
                                    multiview_msgcenter.ActiveViewIndex = -1;
                                }
                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "No Records Found";
                                multiview_msgcenter.ActiveViewIndex = -1;
                            }
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "No Records Found";
                            multiview_msgcenter.ActiveViewIndex = -1;
                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Enter both FlightNo and FlightDate";

                    }

                }
                #endregion

                #region FFR & FFA
                else if (ddlMsgType.SelectedItem.Text == "FFR" || ddlMsgType.SelectedItem.Text == "FFA")
                {

                    if (txtAWBNumber.Text != "")
                    {
                        string AWBNumber = txtAWBNumber.Text.Trim();
                        if (ddlMsgType.SelectedItem.Text == "FFR")
                        {
                            ds = OBJds.GetALLAWBDetails(AWBNumber);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        grdMaterialDetails.DataSource = ds.Tables[1];
                                        grdMaterialDetails.DataBind();
                                        GRDRates.DataSource = ds.Tables[7];
                                        GRDRates.DataBind();
                                        grdRouting.DataSource = ds.Tables[3];
                                        grdRouting.DataBind();
                                        ((DropDownList)(grdRouting.Rows[0].FindControl("ddlFltNum"))).Items.Add(ds.Tables[3].Rows[0]["FltNumber"].ToString());
                                        if (ds.Tables[3].Rows[0]["Status"].ToString() == "C")
                                        {
                                            ((DropDownList)(grdRouting.Rows[0].FindControl("ddlStatus"))).Items.Add("Confirmed");

                                        }
                                        else
                                        {
                                            ((DropDownList)(grdRouting.Rows[0].FindControl("ddlStatus"))).Items.Add("Queued");

                                        }
                                        //((DropDownList)(grdRouting.Rows[0].FindControl("ddlStatus"))).Items.Add(ds.Tables[3].Rows[0]["Status"].ToString());
                                        ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode"))).Items.Add(ds.Tables[1].Rows[0]["CommodityCode"].ToString());
                                        ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).Items.Add(ds.Tables[1].Rows[0]["PaymentMode"].ToString());
                                        ((DropDownList)(GRDRates.Rows[0].FindControl("ddlPayMode"))).Items.Add(ds.Tables[7].Rows[0]["PayMode"].ToString());
                                        txtAgentName.Text = ds.Tables[0].Rows[0]["AgentName"].ToString();
                                        TXTConAddress.Text = ds.Tables[6].Rows[0]["ConsigneeAddress"].ToString();
                                        TXTConsignee.Text = ds.Tables[6].Rows[0]["ConsigneeName"].ToString();
                                        TXTConTelephone.Text = ds.Tables[6].Rows[0]["ConsigneeTelephone"].ToString();
                                        TXTCustomerCode.Text = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                                        TXTDvForCarriage.Text = ds.Tables[0].Rows[0]["DVCarriage"].ToString();
                                        TXTAgentCode.Text = ds.Tables[0].Rows[0]["AgentCode"].ToString();
                                        TXTDvForCustoms.Text = ds.Tables[0].Rows[0]["DVCustom"].ToString();
                                        txtExecutedAt.Text = ds.Tables[0].Rows[0]["ExecutedAt"].ToString();
                                        txtExecutedBy.Text = ds.Tables[0].Rows[0]["ExecutedBy"].ToString();
                                        txtHandling.Text = ds.Tables[0].Rows[0]["HandlingInfo"].ToString();
                                        txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                                        TXTShipAddress.Text = ds.Tables[6].Rows[0]["ShipperAddress"].ToString();
                                        TXTShipper.Text = ds.Tables[6].Rows[0]["ShipperName"].ToString();
                                        TXTShipTelephone.Text = ds.Tables[6].Rows[0]["ShipperTelephone"].ToString();
                                        txtExecutionDate.Text = ds.Tables[0].Rows[0]["ExecutionDate"].ToString();
                                        ddlOrg.Items.Add(ds.Tables[0].Rows[0]["OriginCode"].ToString());
                                        ddlDest.Items.Add(ds.Tables[0].Rows[0]["DestinationCode"].ToString());
                                        ddlServiceclass.Items.Add(ds.Tables[0].Rows[0]["ServiceCargoClassId"].ToString());
                                        CHKConsole.Checked = bool.Parse(ds.Tables[0].Rows[0]["Console"].ToString());
                                        CHKBonded.Checked = bool.Parse(ds.Tables[0].Rows[0]["Bonded"].ToString());
                                        CHKAsAggred.Checked = bool.Parse(ds.Tables[0].Rows[0]["Agreed"].ToString());
                                        CHKExportShipment.Checked = bool.Parse(ds.Tables[0].Rows[0]["Export"].ToString());


                                        btnSendMsg.Visible = true;
                                        multiview_msgcenter.ActiveViewIndex = 1;

                                    }
                                    else
                                    {
                                        lblStatus.ForeColor = Color.Red;
                                        lblStatus.Text = "No Records Found";
                                        multiview_msgcenter.ActiveViewIndex = -1;
                                    }
                                }
                                else
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "No Records Found";
                                    multiview_msgcenter.ActiveViewIndex = -1;
                                }

                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "No Records Found";
                                multiview_msgcenter.ActiveViewIndex = -1;
                            }
                        }
                        if (ddlMsgType.SelectedItem.Text == "FFA")
                        {
                            ds = OBJds.GetALLAWBDetails_FFA(AWBNumber);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {

                                        grdMaterialDetails.DataSource = ds.Tables[1];
                                        grdMaterialDetails.DataBind();
                                        GRDRates.DataSource = ds.Tables[7];
                                        GRDRates.DataBind();
                                        grdRouting.DataSource = ds.Tables[3];
                                        grdRouting.DataBind();
                                        ((DropDownList)(grdRouting.Rows[0].FindControl("ddlFltNum"))).Items.Add(ds.Tables[3].Rows[0]["FltNumber"].ToString());
                                        if (ds.Tables[3].Rows[0]["Status"].ToString() == "C")
                                        {
                                            ((DropDownList)(grdRouting.Rows[0].FindControl("ddlStatus"))).Items.Add("Confirmed");

                                        }
                                        else
                                        {
                                            ((DropDownList)(grdRouting.Rows[0].FindControl("ddlStatus"))).Items.Add("Queued");

                                        }
                                        //((DropDownList)(grdRouting.Rows[0].FindControl("ddlStatus"))).Items.Add(ds.Tables[3].Rows[0]["Status"].ToString());
                                        ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlMaterialCommCode"))).Items.Add(ds.Tables[1].Rows[0]["CommodityCode"].ToString());
                                        ((DropDownList)(grdMaterialDetails.Rows[0].FindControl("ddlPaymentMode"))).Items.Add(ds.Tables[1].Rows[0]["PaymentMode"].ToString());
                                        ((DropDownList)(GRDRates.Rows[0].FindControl("ddlPayMode"))).Items.Add(ds.Tables[7].Rows[0]["PayMode"].ToString());
                                        txtAgentName.Text = ds.Tables[0].Rows[0]["AgentName"].ToString();
                                        TXTConAddress.Text = ds.Tables[6].Rows[0]["ConsigneeAddress"].ToString();
                                        TXTConsignee.Text = ds.Tables[6].Rows[0]["ConsigneeName"].ToString();
                                        TXTConTelephone.Text = ds.Tables[6].Rows[0]["ConsigneeTelephone"].ToString();
                                        TXTCustomerCode.Text = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                                        TXTDvForCarriage.Text = ds.Tables[0].Rows[0]["DVCarriage"].ToString();
                                        TXTAgentCode.Text = ds.Tables[0].Rows[0]["AgentCode"].ToString();
                                        TXTDvForCustoms.Text = ds.Tables[0].Rows[0]["DVCustom"].ToString();
                                        txtExecutedAt.Text = ds.Tables[0].Rows[0]["ExecutedAt"].ToString();
                                        txtExecutedBy.Text = ds.Tables[0].Rows[0]["ExecutedBy"].ToString();
                                        txtHandling.Text = ds.Tables[0].Rows[0]["HandlingInfo"].ToString();
                                        txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                                        TXTShipAddress.Text = ds.Tables[6].Rows[0]["ShipperAddress"].ToString();
                                        TXTShipper.Text = ds.Tables[6].Rows[0]["ShipperName"].ToString();
                                        TXTShipTelephone.Text = ds.Tables[6].Rows[0]["ShipperTelephone"].ToString();
                                        txtExecutionDate.Text = ds.Tables[0].Rows[0]["ExecutionDate"].ToString();
                                        ddlOrg.Items.Add(ds.Tables[0].Rows[0]["OriginCode"].ToString());
                                        ddlDest.Items.Add(ds.Tables[0].Rows[0]["DestinationCode"].ToString());
                                        ddlServiceclass.Items.Add(ds.Tables[0].Rows[0]["ServiceCargoClassId"].ToString());
                                        CHKConsole.Checked = bool.Parse(ds.Tables[0].Rows[0]["Console"].ToString());
                                        CHKBonded.Checked = bool.Parse(ds.Tables[0].Rows[0]["Bonded"].ToString());
                                        CHKAsAggred.Checked = bool.Parse(ds.Tables[0].Rows[0]["Agreed"].ToString());
                                        CHKExportShipment.Checked = bool.Parse(ds.Tables[0].Rows[0]["Export"].ToString());


                                        btnSendMsg.Visible = true;
                                        multiview_msgcenter.ActiveViewIndex = 1;

                                    }
                                    else
                                    {
                                        lblStatus.ForeColor = Color.Red;
                                        lblStatus.Text = "No Records Found";
                                        multiview_msgcenter.ActiveViewIndex = -1;
                                    }
                                }
                                else
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "No Records Found";
                                    multiview_msgcenter.ActiveViewIndex = -1;

                                }

                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "No Records Found";
                                multiview_msgcenter.ActiveViewIndex = -1;
                            }
                        }



                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Enter AWB Number";
                    }




                }
                #endregion

                #region FFM
                else if (ddlMsgType.SelectedItem.Text == "FFM")
                {
                    if (txtFlightNo.Text != "" && txtFlightFromdate0.Text != "")
                    {
                        string FlightNumber = txtFlightNo.Text.Trim();
                        DateTime FlightDate = DateTime.ParseExact(txtFlightFromdate0.Text, "dd/MM/yyyy", null);

                        ds = OBJds.GetManifestDetails(POL, FlightNumber, FlightDate);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    gdvULDDetails.DataSource = ds;
                                    gdvULDDetails.DataBind();
                                    multiview_msgcenter.ActiveViewIndex = 2;
                                    btnSendMsg.Visible = true;
                                }
                                else
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "No Records Found";
                                    multiview_msgcenter.ActiveViewIndex = -1;
                                }
                            }
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "No Records Found";
                            multiview_msgcenter.ActiveViewIndex = -1;
                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Enter both FlightNo and FlightDate";

                    }

                }
                #endregion
                else
                {

                }

            }
            catch (Exception ex)
            { }

        }
        #endregion

        #region SendMessage_Click
        protected void btnSendMsg_Click(object sender, EventArgs e)
        {

            try
            {
                string UserLoc = Session["Station"].ToString();
                bool flag = false;
                lblStatus.Text = "";
                string flightdate = "";
                string fromemailid = ConfigurationManager.AppSettings["FromEmailID"].ToString();
                DataSet ds = new DataSet();
                DataSet dset = new DataSet();
                string[] queryname = new string[1];
                string[] qname = new string[1];
                object[] queryvalue = new object[1];
                SqlDbType[] querytype = new SqlDbType[1];
                queryname[0] = "AWBNumber";
                qname[0] = "AWBNo";
                queryvalue[0] = txtAWBNumber.Text;
                querytype[0] = SqlDbType.VarChar;
                WebReference.MessagingService objref = new ProjectSmartCargoManager.WebReference.MessagingService();
                if (ddlMsgType.SelectedItem.Text == "FFR")
                {
                    ds = db.SelectRecords("SpGetBookingFFR", queryname, queryvalue, querytype);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                flag = objref.EncodeFFRforSend(ds, fromemailid.Trim(), txtmailId1.Text.Trim());
                                if (flag == true)
                                {
                                    lblStatus.ForeColor = Color.Green;
                                    lblStatus.Text = "FFR Sent Successfully";
                                    multiview_msgcenter.ActiveViewIndex = -1;
                                    btnSendMsg.Visible = false;
                                    ddlCommType.SelectedIndex = 0;
                                    ddlMsgType.SelectedIndex = 0;
                                    ddlPartnerType.SelectedIndex = 0;
                                    txtmailId1.Text = "";
                                    txtPartnerCode.Text = "";
                                    txtFlightFromdate0.Text = "";
                                    txtFlightNo.Text = "";
                                    txtAWBNumber.Text = "";
                                    txtAWBNumber.Visible = false;
                                    lblawbnum.Visible = false;
                                    btnclear.Visible = false;
                                    btnList.Visible = false;
                                }
                                else
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Message Sending Failed";
                                }
                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "No Records Found";

                            }
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "No Records Found";

                        }

                    }
                }
                else if (ddlMsgType.SelectedItem.Text == "FFM")
                {
                    DateTime dt = DateTime.ParseExact(txtFlightFromdate0.Text, "dd/MM/yyyy", null);
                    flightdate = dt.ToString("MM/dd/yyyy");
                    flag = objref.EncodeFFMForSend(UserLoc.Trim(), txtFlightNo.Text.Trim(), flightdate.Trim(), fromemailid.Trim(), txtmailId1.Text.Trim());

                    if (flag == true)
                    {
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Text = "FFM Sent Successfully";
                        multiview_msgcenter.ActiveViewIndex = -1;
                        btnSendMsg.Visible = false;
                        ddlCommType.SelectedIndex = 0;
                        ddlMsgType.SelectedIndex = 0;
                        ddlPartnerType.SelectedIndex = 0;
                        txtmailId1.Text = "";
                        txtPartnerCode.Text = "";
                        txtFlightFromdate0.Text = "";
                        txtFlightNo.Text = "";
                        lblFlgtdate.Visible = false;
                        txtFlightFromdate0.Visible = false;
                        txtFlightNo.Visible = false;
                        lblFlightNo.Visible = false;
                        btnclear.Visible = false;
                        btnList.Visible = false;
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Message Sending Failed";
                    }

                }
                else if (ddlMsgType.SelectedItem.Text == "FBL")
                {
                    DateTime dt = DateTime.ParseExact(txtFlightFromdate0.Text, "dd/MM/yyyy", null);
                    flightdate = dt.ToString("MM/dd/yyyy");

                    flag = objref.EncodeFBLForSend(UserLoc.Trim(), txtFlightNo.Text.Trim(), flightdate.Trim(), fromemailid.Trim(), txtmailId1.Text.Trim());

                    if (flag == true)
                    {
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Text = "FBL Sent Successfully";
                        multiview_msgcenter.ActiveViewIndex = -1;
                        btnSendMsg.Visible = false;
                        ddlCommType.SelectedIndex = 0;
                        ddlMsgType.SelectedIndex = 0;
                        ddlPartnerType.SelectedIndex = 0;
                        txtmailId1.Text = "";
                        txtPartnerCode.Text = "";
                        txtFlightFromdate0.Text = "";
                        txtFlightNo.Text = "";
                        lblFlgtdate.Visible = false;
                        txtFlightFromdate0.Visible = false;
                        txtFlightNo.Visible = false;
                        lblFlightNo.Visible = false;
                        btnclear.Visible = false;
                        btnList.Visible = false;
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Message Sending Failed";
                    }

                }
                else if (ddlMsgType.SelectedItem.Text == "FFA")
                {
                    ds = db.SelectRecords("spGetFFADataAsPerAWBNumber", qname, queryvalue, querytype);


                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                flag = objref.encodeFFAForSend(ds, fromemailid.Trim(), txtmailId1.Text.Trim());
                                if (flag == true)
                                {
                                    lblStatus.ForeColor = Color.Green;
                                    lblStatus.Text = "FFA Sent Successfully";
                                    multiview_msgcenter.ActiveViewIndex = -1;
                                    btnSendMsg.Visible = false;
                                    ddlCommType.SelectedIndex = 0;
                                    ddlMsgType.SelectedIndex = 0;
                                    ddlPartnerType.SelectedIndex = 0;
                                    txtmailId1.Text = "";
                                    txtPartnerCode.Text = "";
                                    txtFlightFromdate0.Text = "";
                                    txtFlightNo.Text = "";
                                    txtAWBNumber.Text = "";
                                    txtAWBNumber.Visible = false;
                                    lblawbnum.Visible = false;
                                    btnclear.Visible = false;
                                    btnList.Visible = false;
                                }
                                else
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Message Sending Failed";
                                }
                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "No Records Found";

                            }
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "No Records Found";

                        }

                    }

                }


            }
            catch (Exception ex)

            { }
        }
        #endregion

        #region Button Clear

        protected void btnclear_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            txtAWBNumber.Text = "";
            txtFlightFromdate0.Text = "";
            txtFlightNo.Text = "";
            txtPartnerCode.Text = "";
            txtmailId1.Text = "";
            lblFlgtdate.Visible = false;
            lblFlightNo.Visible = false;
            txtFlightNo.Visible = false;
            txtFlightFromdate0.Visible = false;
            lblawbnum.Visible = false;
            chkHAWBnew.Visible = false;
            txtAWBNumber.Visible = false;
            btnList.Visible = false;
            btnclear.Visible = false;
            gdvULDLoadPlanAWB.DataSource = null;
            gdvULDLoadPlanAWB.DataBind();
            multiview_msgcenter.ActiveViewIndex = -1;
            ddlPartnerType.SelectedIndex = 0;
            ddlMsgType.SelectedIndex = 0;
            ddlCommType.SelectedIndex = 0;
        }

        #endregion

        #region PartnerCode Validation
        protected void txtPartnerCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                DataSet ds = new DataSet();
                ds = db.GetDataset("Select Distinct PartnerEmailiD from tblMessageConfiguration2 where PartnerType='" + ddlPartnerType.SelectedItem.Text.Trim() + "' and MessageType='" + ddlMsgType.SelectedItem.Text.Trim() + "' and PartnerCode='" + txtPartnerCode.Text.Trim() + "' and MsgCommType='" + ddlCommType.SelectedItem.Text.Trim() + "'");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtmailId1.Text = ds.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
                            btnSendMsg.Enabled = true;
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Invalid Selection";
                            txtPartnerCode.Text = "";
                            txtmailId1.Text = "";
                            btnSendMsg.Enabled = false;
                        }
                    }
                }


            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Retrieving value for autocomplete method
        protected void ddlPartnerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPartnerCode_AutoCompleteExtender.ContextKey = ddlPartnerType.SelectedItem.Text;

        }
        #endregion
    }
}
