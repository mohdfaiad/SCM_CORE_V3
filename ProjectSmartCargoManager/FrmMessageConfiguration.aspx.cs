using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using QID.DataAccess;
using System.Drawing;
using System.Collections;
using BAL;
using System.Text.RegularExpressions;
using System.Data;


namespace ProjectSmartCargoManager
{
    public partial class FrmMessageConfiguration : System.Web.UI.Page
    {
        MessageConfigurationBAL Conf = new MessageConfigurationBAL();
        string Result;
        string strCheckedStations;
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    getMessage();

                    if (Request.QueryString["cmd"] == "Edit" || Request.QueryString["cmd"] == "View")
                    {
                        getRateLineDetails();
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>hideDiv('<%= ckhULDRate.ClientID %>');</script>", false);
                    }
                    if (Request.QueryString["cmd"] == "Edit")
                    {
                        btnSave.Text = "Update";
                        //Session["mode"] = "Edit";

                    }
                    else if (Request.QueryString["cmd"] == "View")
                    {
                        btnSave.Visible = false;
                        //Session["mode"] = "View";
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion Load

        #region GetMessages
        public void getMessage()
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;
            try
            {
                ds = da.SelectRecords("Sp_GetMessages");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    //chkListStation.DataSource = ds;
                    //chkListStation.DataMember = ds.Tables[0].TableName;
                    //chkListStation.DataTextField = "MessageName";
                    //chkListStation.DataValueField = "MessageName";
                    //chkListStation.DataBind();
                    //chkListStation.SelectedIndex = -1;

                    ddlPartnerType.DataTextField = "PartnerType";
                    ddlPartnerType.DataSource = ds.Tables[1];
                    ddlPartnerType.DataBind();

                    //ddlMessageType.Items.Insert(0, new ListItem("Select", ""));
                    //ddlMessageType.SelectedIndex = -1;

                    ddlPartnerType.Items.Insert(0, new ListItem("Select", ""));
                    ddlPartnerType.SelectedIndex = -1;
                }
            }
            catch (Exception e)
            {
                ds = null;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }
        }
        #endregion GetMessages
        
        #region function
        //SELECT FlightID from dbo.AirlineSchedule where FlightID like '"+ prefixText +"%' or FlightID like '"+ prefixText +"%'


        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static string[] GetPartner(string prefixText, int count)
        {

            string con = Global.GetConnectionString();

            // SqlConnection con = new SqlConnection("connection string");
            //SELECT FlightID from dbo.AirlineSchedule where FlightID like '"+ prefixText +"%' or FlightID like '"+ prefixText +"%'
            //int i=int.Parse(Session["RoleID"].ToString());
            string query = "select PartnerName from tblPartnerMaster where PartnerName like '%" + prefixText;
            SqlDataAdapter dad = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            dad.Fill(ds);

            List<string> list = new List<string>(ds.Tables[0].Rows.Count);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(dr[0].ToString());
                //+Environment.NewLine
                //list.Add("\n"); 
            }
            dad = null;
            if (ds != null)
                ds.Dispose();

            return list.ToArray();

        }

        #endregion function
        
        #region Clear
        public void Clear()
        {
            try
            {
                txtOrigin.Text = "";
                txtDestination.Text = "";
                txtTransitDest.Text = "";
                txtFltNo.Text = "";
                txtSitaID.Text = "";
                txtEmailFFM.Text = "";
                ddlPartnerType.SelectedIndex = -1;
                ddlPartnerType_SelectedIndexChanged(null, null);
                //ddlPartnerCode.SelectedItem.Text = "Select";
                //ddlMessageType.SelectedIndex = -1;
                ddlMsgCommType.SelectedIndex = -1;
                txtFTPID.Text = txtFTPPassword.Text = txtFTPUserName.Text = string.Empty;
                //foreach (ListItem li in chkListStation.Items)
                //{
                //    li.Selected = false;
                //    li.Enabled = true;
                //}
                txtMessageType.Text = string.Empty;
            }
            catch (Exception e)
            { }
        }
        #endregion Clear
        
        #region Clear
        protected void btClear_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            { }
        }
        #endregion Clear
        
        #region Save or Update
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string PartnerCode, MessageType, Origin, Destination, TransitDestination, FlightNumber, SitaID, MessageCommType, EmailId,PartnerType,createdBy,FTPID,FTPUserNm,FTPPwd;
                //strCheckedStations = getCheckedStations();
                if (Page.IsValid)
                {
                    if (btnSave.Text == "Save")
                    {
                        #region Save
                        #region CheckList
                        if (txtMessageType.Text.Trim() != string.Empty)
                        {
                            string[] MessageTypes = txtMessageType.Text.Trim().Split(',');
                            for (int i = 0; i < MessageTypes.Length; i++)
                            {
                                #region CheckList

                                if (ddlPartnerCode.SelectedItem.Text.Trim() == "Select")
                                {
                                    lblStatus.Text = "Please select Partner Code";
                                    lblStatus.ForeColor = Color.Red;
                                    return;
                                }
                                else
                                {
                                    PartnerCode = ddlPartnerCode.SelectedItem.Text.Trim();
                                }
                                //if (ddlMessageType.SelectedItem.Text == "Select")
                                //{
                                //    lblStatus.Text = "Please select Message Type";
                                //    lblStatus.ForeColor = Color.Red;
                                //    return;
                                //}
                                //else
                                //{
                                MessageType = MessageTypes[i];
                                //MessageType = MessageType.Substring(1, MessageType.Length - 1);
                                //}
                                if (txtOrigin.Text.Trim() == "")
                                {
                                    Origin = "";
                                }
                                else
                                {
                                    Origin = txtOrigin.Text.TrimEnd(',');
                                }
                                if (txtDestination.Text.Trim() == "")
                                {
                                    Destination = "";
                                }
                                else
                                {
                                    Destination = txtDestination.Text.TrimEnd(',');
                                }
                                if (txtTransitDest.Text.Trim() == "")
                                {
                                    TransitDestination = txtTransitDest.Text = "";
                                }
                                else
                                {
                                    TransitDestination = txtTransitDest.Text.TrimEnd(',');
                                }
                                if (txtSitaID.Text.Trim() == "")
                                {
                                    SitaID = "";
                                }
                                else
                                {
                                    SitaID = txtSitaID.Text.TrimEnd(',');
                                }
                                if (txtFltNo.Text == "")
                                {
                                    FlightNumber = "";
                                }
                                else
                                {
                                    FlightNumber = txtFltNo.Text.TrimEnd(',');
                                }
                                if (txtEmailFFM.Text == "")
                                {
                                    EmailId = "";
                                }
                                else
                                {
                                    EmailId = txtEmailFFM.Text.TrimEnd(',');
                                }
                                if (txtFTPID.Text == "")
                                {
                                    FTPID = "";
                                }
                                else
                                {
                                    FTPID = txtFTPID.Text.TrimEnd(',');
                                }
                                if (txtFTPUserName.Text == "")
                                {
                                    FTPUserNm = "";
                                }
                                else
                                {
                                    FTPUserNm = txtFTPUserName.Text.TrimEnd(',');
                                }
                                if (txtFTPPassword.Text == "")
                                {
                                    FTPPwd = "";
                                }
                                else
                                {
                                    FTPPwd = txtFTPPassword.Text.TrimEnd(',');
                                }
                                MessageCommType = ddlMsgCommType.SelectedItem.Text.Trim();
                                if (ddlPartnerType.SelectedItem.Text.Trim() == "Select")
                                {
                                    lblStatus.Text = "Select Partner Type.";
                                    lblStatus.ForeColor = Color.Red;
                                    return;
                                }
                                else
                                {
                                    PartnerType = ddlPartnerType.SelectedItem.Text.Trim();
                                }
                                createdBy = Session["UserName"].ToString();
                                Result = Conf.SaveConfiguratinMessage(PartnerCode, MessageType, Origin, Destination, TransitDestination, FlightNumber, SitaID, MessageCommType, EmailId, PartnerType, createdBy, FTPID, FTPUserNm, FTPPwd, chkAutoGenerate.Checked, txtMsgStart.Text, txtMsgEnd.Text);

                                #endregion CheckList
                            }

                        }
                        else
                        {
                            lblStatus.Text = "Please enter Message Type!";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                        #endregion CheckList
                        if (Result == "pass" || Result == "updated")
                        {
                            lblStatus.ForeColor = System.Drawing.Color.Green;
                            lblStatus.Text = "Message Configuration saved successfully";
                            Clear();
                            return;
                        }

                        else
                        {
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            lblStatus.Text = "Please select different data to enter";
                            return;

                        }
                        #endregion Save}
                    }
                    else if (btnSave.Text == "Update")
                    {
                        #region Update
                        if (txtMessageType.Text.Trim() != string.Empty)
                        {
                            #region Prepare Parameters

                            //Message Type
                            string[] MessageTypes = txtMessageType.Text.Trim().Split(',');
                            //string msgtype = null;
                            //foreach (ListItem li in chkListStation.Items)
                            //{
                            //    if (li.Selected == true)
                            //        msgtype = li.Text;
                            //}

                            string msgtype = null;
                            foreach (string li in MessageTypes)
                            {
                                msgtype = li;
                                break;
                            }

                            MessageType = msgtype;

                            //Partner Code
                            if (ddlPartnerCode.SelectedItem.Text.Trim() == "Select")
                            {
                                lblStatus.Text = "Please select Partner Code";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                            else
                            {
                                PartnerCode = ddlPartnerCode.SelectedItem.Text.Trim();
                            }

                            //Origin
                            if (txtOrigin.Text.Trim() == "")
                            {
                                Origin = "";
                            }
                            else
                            {
                                Origin = txtOrigin.Text.TrimEnd(',');
                            }

                            //Destination
                            if (txtDestination.Text.Trim() == "")
                            {
                                Destination = "";
                            }
                            else
                            {
                                Destination = txtDestination.Text.TrimEnd(',');
                            }

                            //Transit Destination
                            if (txtTransitDest.Text.Trim() == "")
                            {
                                TransitDestination = txtTransitDest.Text = "";
                            }
                            else
                            {
                                TransitDestination = txtTransitDest.Text.TrimEnd(',');
                            }
                            //SITA ID
                            if (txtSitaID.Text.Trim() == "")
                            {
                                SitaID = "";
                            }
                            else
                            {
                                SitaID = txtSitaID.Text.TrimEnd(',');
                            }

                            //Flight No
                            if (txtFltNo.Text == "")
                            {
                                FlightNumber = "";

                            }
                            else
                            {
                                FlightNumber = txtFltNo.Text.TrimEnd(',');
                            }

                            //Email Id
                            if (txtEmailFFM.Text == "")
                            {
                                EmailId = "";
                            }
                            else
                            {
                                EmailId = txtEmailFFM.Text.TrimEnd(',');
                            }

                            //FTP Id
                            if (txtFTPID.Text == "")
                            {
                                FTPID = "";
                            }
                            else
                            {
                                FTPID = txtFTPID.Text.TrimEnd(',');
                            }

                            //FTP User Name
                            if (txtFTPUserName.Text == "")
                            {
                                FTPUserNm = "";
                            }
                            else
                            {
                                FTPUserNm = txtFTPUserName.Text.TrimEnd(',');
                            }

                            //FTP Pwd
                            if (txtFTPPassword.Text == "")
                            {
                                FTPPwd = "";
                            }
                            else
                            {
                                FTPPwd = txtFTPPassword.Text.TrimEnd(',');
                            }

                            //Msg Comm Type
                            MessageCommType = ddlMsgCommType.SelectedItem.Text.Trim();

                            //Partner Type
                            if (ddlPartnerType.SelectedItem.Text.Trim() == "Select")
                            {
                                lblStatus.Text = "Select Partner Type.";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }
                            else
                            {
                                PartnerType = ddlPartnerType.SelectedItem.Text.Trim();
                            }

                            createdBy = Session["UserName"].ToString();
                            string srno = Session["MsgId"].ToString();

                            #endregion Prepare Parameters

                            Result = Conf.UpdateConfiguratinMessage(srno, PartnerCode, MessageType, Origin, Destination, TransitDestination, FlightNumber, SitaID, MessageCommType, EmailId, PartnerType, createdBy, FTPID, FTPUserNm, FTPPwd, chkAutoGenerate.Checked, txtMsgStart.Text, txtMsgEnd.Text);


                            if (Result == "pass" || Result == "updated")
                            {
                                lblStatus.ForeColor = System.Drawing.Color.Green;
                                lblStatus.Text = "Message Configuration updated successfully";
                                txtOrigin.Enabled = txtDestination.Enabled = txtFltNo.Enabled = txtTransitDest.Enabled = true;
                                Clear();
                                //Server.Transfer("~/FrmMessageConfiguration.aspx");
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>chngurl();</script>", false);
                                return;
                            }

                            else
                            {
                                lblStatus.ForeColor = System.Drawing.Color.Red;
                                lblStatus.Text = "Message Configuration could not be updated";
                                return;

                            }
                        }
                        else
                        {
                            lblStatus.Text = "Please enter Message Type!";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                        #endregion Update
                    }
                }
            }
            catch (Exception ex)
            {
                string bhfyt = ex.Message;
            }

        }
        #endregion Save or Update
        
        #region PartnerType
        protected void ddlPartnerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SQLServer da = new SQLServer(Global.GetConnectionString());
            DataSet ds = null;
            try
            {
                if (ddlPartnerType.SelectedIndex > -1 && ddlPartnerType.SelectedItem.Text.Length > 0)
                {
                    ds = da.SelectRecords("spGetPartnerCode", "Type", ddlPartnerType.SelectedItem.Text.Trim(), SqlDbType.VarChar);
                    ddlPartnerCode.DataTextField = "PartnerCode";
                    ddlPartnerCode.DataSource = ds.Tables[0];
                    ddlPartnerCode.DataBind();
                }

            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
            }

        }
        #endregion PartnerType

//        #region GetString
//        protected string getCheckedStations()
//        {
//            //if (chkSelectAll.Checked == true)
//            //{
//                for (int i = 0; i < chkListStation.Items.Count; i++)
//                {
//                    if (strCheckedStations == "")
//                        strCheckedStations = chkListStation.Items[i].Value;
//                    else
//                        strCheckedStations = strCheckedStations + "," + chkListStation.Items[i].Value;
//                }
//           // }
//            //else
//            //{
//            //    for (int i = 0; i < chkListStation.Items.Count; i++)
//            //    {
//            //        if (chkListStation.Items[i].Selected == true)
//            //        {
//            //            if (strCheckedStations == "")
//            //                strCheckedStations = chkListStation.Items[i].Value;
//            //            else
//            //                strCheckedStations = strCheckedStations + "," + chkListStation.Items[i].Value;
//            //        }
//            //    }
//            //}


//            return strCheckedStations;
//        }
//#endregion Getstring

        protected void ddlMsgCommType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        #region Get Msg Details
        public void getRateLineDetails()
        {
            string Id = Request.QueryString["SrNo"].ToString();
            Session["MsgId"] = Id;
            DataSet dsRateCard = new DataSet();
            dsRateCard = Conf.GetMsgList(Id);
            fillMsgDetails(dsRateCard);
        }
        #endregion Get Msg Details

        #region fillMsgDetails
        public void fillMsgDetails(DataSet dsMsgDetails)
        {
            //Code to fill details of msg from dataset

            txtOrigin.Text = dsMsgDetails.Tables[0].Rows[0]["OrgAirportCode"].ToString();
            txtDestination.Text = dsMsgDetails.Tables[0].Rows[0]["DestAirportCode"].ToString();
            txtTransitDest.Text = dsMsgDetails.Tables[0].Rows[0]["TransitAirportCode"].ToString();
            txtFltNo.Text = dsMsgDetails.Tables[0].Rows[0]["FlightNumber"].ToString();
            txtEmailFFM.Text = dsMsgDetails.Tables[0].Rows[0]["PartnerEmailiD"].ToString();
            txtSitaID.Text = dsMsgDetails.Tables[0].Rows[0]["PartnerSITAiD"].ToString();
            txtFTPID.Text = dsMsgDetails.Tables[0].Rows[0]["FTPID"].ToString();
            txtFTPPassword.Text = dsMsgDetails.Tables[0].Rows[0]["FTPPassword"].ToString();
            txtFTPUserName.Text = dsMsgDetails.Tables[0].Rows[0]["FTPUserName"].ToString();
            
            txtOrigin.Enabled = txtDestination.Enabled = txtTransitDest.Enabled = txtFltNo.Enabled = false;

            ddlPartnerType.SelectedValue = dsMsgDetails.Tables[0].Rows[0]["PartnerType"].ToString();
            ddlPartnerType_SelectedIndexChanged(null, null);
            ddlPartnerCode.SelectedValue = dsMsgDetails.Tables[0].Rows[0]["PartnerCode"].ToString();

            try 
            {
                chkAutoGenerate.Checked = Convert.ToBoolean(dsMsgDetails.Tables[0].Rows[0]["AutoGenerate"].ToString());
                txtMsgStart.Text = dsMsgDetails.Tables[0].Rows[0]["MsgStartWith"].ToString();
                txtMsgEnd.Text = dsMsgDetails.Tables[0].Rows[0]["MsgEndWith"].ToString();
            }
            catch (Exception ex) { }

            string msgtype = dsMsgDetails.Tables[0].Rows[0]["Messagetype"].ToString();
            txtMessageType.Text = msgtype;

            //foreach (ListItem li in chkListStation.Items)
            //{
            //    li.Enabled = false;

            //    if (li.Text == msgtype)
            //    {
            //        li.Selected = true;
            //        //li.Enabled = true;
            //    }
            //}
        }
        #endregion fillMsgDetails

       
    }
}