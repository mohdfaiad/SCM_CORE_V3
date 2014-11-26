using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using BAL;
using System.IO;
using System.Drawing;
using QID.DataAccess;
//using clsDataLib;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace ProjectSmartCargoManager
{
    public partial class rptMessaging : System.Web.UI.Page
    {


        #region Variables
        ReportBAL OBJasb=new ReportBAL();
        static string AgentCode = "";
        public static string CurrTime = "";
        static string message = "";
        static int gridIndex = 0;
        int rowindex = 0;
        string subject = "";
        string body = "";
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    AgentCode = Convert.ToString(Session["ACode"]);

                    int RoleId = Convert.ToInt32(Session["RoleID"]);
                    Session["Message"] = null;

                    if (RoleId == 1 && AgentCode != "")
                    {
                    }
                    
                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");

                    txtFlightFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFlightToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    LoadingMsgDropdownDetails(ddlcommMsgtype, ddlmsgtype);

                    ddlcommMsgtype.Items.FindByText(ddlcommMsgtype.SelectedItem.Text);
                    ddlmsgtype.Items.FindByText(ddlmsgtype.SelectedItem.Text);
                }
                catch (Exception ex)
                { }
            }
        }
        #endregion

        #region List Button
        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            try
            {

                lblStatus.Text = "";
                txtMessageBody.Text = "";
                Session["Message"] = null;
                gdvMsg.DataSource = null;
                gdvMsg.DataBind();

                if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                {
                    //lblStatus.ForeColor = Color.Red;
                    //lblStatus.Text = "Please enter From date and To date for datewise Schedule List";
                    //txtFlightFromdate.Focus();
                    //return;
                }
                if (txtFlightFromdate.Text.Trim() != "" || txtFlightToDate.Text.Trim() != "")
                {
                    DateTime dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    DateTime dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                    int chk = DateTime.Compare(dt1, dt2);
                    if (chk > 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid To date";
                        //txtFromdate.Focus();
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                //txtFromdate.Focus();
                return;
            }
            try
            {

                string fromdate = "", ToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {
                    if (txtFlightFromdate.Text != "")
                    {
                        dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);

                        fromdate = dt1.ToString("MM/dd/yyyy");
                    }
                    if (txtFlightToDate.Text != "")
                    {
                        dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);
                        ToDate = dt2.ToString("MM/dd/yyyy");
                    }
                }
                catch (Exception ex)
                { }
                string MsgType = "";

                MsgType = ddlmsgtype.SelectedItem.Text;
                string CommMsgType = "";
                CommMsgType = ddlcommMsgtype.SelectedItem.Text;
                string Process = "";
                string Status = "ALL";
                if (ChkProcessed.Checked == true)
                {
                    Process = "1";
                }
                if (ChkFailed.Checked == true)
                {
                    Process = "0";
                }

                if (Incoming.Checked == true)
                {
                    ds = OBJasb.GetMessagingDetails_Inbox(fromdate, ToDate, Status, Process, MsgType, CommMsgType, txtCriteria.Text.Trim());

                    if (ds != null)
                    {
                        if (ds.Tables != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {

                                    gdvMsg.DataSource = ds;
                                    gdvMsg.DataBind();
                                    Session["Message"] = ds;

                                    //shashikant..........
                                    for (int i = 0; i < gdvMsg.Rows.Count; i++)
                                    {
                                        if (((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text.ToString() == "True")
                                        {
                                            ((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text = "Success";
                                        }
                                        //string a = ((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text.ToString();
                                        else if (((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text.ToString() == "False")
                                        {
                                            ((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text = "Failed";
                                        }
                                    }


                                }
                                else
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "No Records Found";
                                }

                            }
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "No Records Found";
                        }
                    }

                }
                else
                    if (Outgoing.Checked == true)
                    {
                        ds = OBJasb.GetMessagingDetails_Outbox(txtFlightFromdate.Text.Trim(), txtFlightToDate.Text.Trim(), Status, Process, MsgType, CommMsgType, txtCriteria.Text.Trim());

                        if (ds != null)
                        {
                            if (ds.Tables != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        gdvMsg.DataSource = ds;
                                        gdvMsg.DataBind();
                                        Session["Message"] = ds;

                                        //shashikant............
                                        for (int i = 0; i < gdvMsg.Rows.Count; i++)
                                        {
                                            if (((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text.ToString() == "True")
                                            {
                                                ((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text = "Success";
                                            }
                                            //string a = ((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text.ToString();
                                            else if (((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text.ToString() == "False")
                                            {
                                                ((Label)(gdvMsg.Rows[i].FindControl("lblprocess"))).Text = "Failed";
                                            }
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
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "No Records Found";
                        }


                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please Select Incoming or Outgoing option.";
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
        #endregion

        #region Button Clear
        protected void btnclear_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            txtFlightFromdate.Text = "";
            txtFlightToDate.Text = "";
            gdvMsg.DataSource = null;
            gdvMsg.DataBind();
            txtMessageBody.Text = "";
            ddlcommMsgtype.SelectedIndex = 0;
            ddlmsgtype.SelectedIndex = 0;
            Outgoing.Checked = false;
            Incoming.Checked = false;

        }
        #endregion

        #region Loading Dropdownlist

        public void LoadingMsgDropdownDetails(DropDownList ddcommtype,DropDownList ddmsgtype)
        {
            QID.DataAccess.SQLServer db = new QID.DataAccess.SQLServer(Global.GetConnectionString());
            DataSet ds = new DataSet();
            try
            {
                string Query = "Select MessageIATACode from dbo.tblMessageMaster";
                ds = db.GetDataset(Query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddmsgtype.DataSource = ds;
                            ddmsgtype.DataTextField = "MessageIATACode";
                            ddmsgtype.DataValueField = "MessageIATACode";
                            ddmsgtype.DataBind();
                            ddmsgtype.Items.Insert(0, "ALL");

                        }

                    }
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
                db = null;
            }
        }
        #endregion

        #region Message Details
        protected void gdvMsg_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                string strBlog = string.Empty;
                string filename = string.Empty;
                rowindex = gvRow.RowIndex;

                if (rowindex < 0)
                    return;

                gridIndex = 0;
                gridIndex = rowindex;

                strBlog = ((Label)gdvMsg.Rows[rowindex].FindControl("lblIsBlog")).Text.Trim();

                if (e.CommandName == "Message")
                {
                    if (strBlog == "0")
                    {
                        txtMessageBody.Text = Convert.ToString(e.CommandArgument.ToString());

                        string mailbox = "";

                        if (Incoming.Checked)
                        {
                            mailbox = "inbox";
                        }
                        if (Outgoing.Checked)
                        {
                            mailbox = "outbox";
                        }

                        // Session["Msg_Subject"] = ((Label)gdvMsg.Rows[rowindex].FindControl("lbltype")).Text;
                        //Session["Msg_Body"] = ((LinkButton)gdvMsg.Rows[rowindex].FindControl("lnkMsg")).Text;

                        //Session["Msg_Sender"] = ((Label)gdvMsg.Rows[rowindex].FindControl("lblfrm")).Text;

                        // Get All Email Id (ToId's)
                        Session["Msg_ToEmailId"] = ((Label)gdvMsg.Rows[rowindex].FindControl("lblfrm")).Text;
                        // Get Message Body
                        Session["Msg_Body"] = txtMessageBody.Text;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowMessage('" + mailbox + "','" + hlfSrNo.Value.ToString() + "');</script>", false);


                    }
                    else
                    {
                        filename = Convert.ToString(e.CommandArgument.ToString());

                        //string containerName = "blobstorage"; //container must be lowercase, no special characters
                        ////byte[] downloadStream = null;
                        //StorageCredentialsAccountAndKey cred = new StorageCredentialsAccountAndKey("qidstorage", "NUro8/C7+kMqtwOwLbe6agUvA83s+8xSTBqrkMwSjPP6MAxVkdtsLDGjyfyEqQIPv6JHEEf5F5s4a+DFPsSQfg==");
                        //CloudStorageAccount storageAccount = new CloudStorageAccount(cred, false);
                        //CloudBlobClient blobClient = new CloudBlobClient(storageAccount.BlobEndpoint.AbsoluteUri, cred);

                        ////get a reference to the blob
                        //CloudBlob blob = blobClient.GetBlobReference(string.Format("{0}/{1}", containerName, filename));
                        ////blob = container.GetBlobReference(fileName);
                        //MemoryStream memStream = new MemoryStream();
                        //blob.DownloadToStream(memStream);
                        //Response.ContentType = blob.Properties.ContentType;
                        //Response.AddHeader("Content-Disposition", "Attachment; filename=" + filename.ToString());
                        //Response.AddHeader("Content-Length", blob.Properties.Length.ToString());
                        //Response.BinaryWrite(memStream.ToArray()); 


                        //get a reference to the blob
                        CloudBlob blob = CommonUtility.BlodProperties(filename);
                        //blob = container.GetBlobReference(fileName);
                        MemoryStream memStream = new MemoryStream();
                        blob.DownloadToStream(memStream);
                        Response.ContentType = blob.Properties.ContentType;
                        Response.AddHeader("Content-Disposition", "Attachment; filename=" + filename.ToString());
                        Response.AddHeader("Content-Length", blob.Properties.Length.ToString());
                        Response.BinaryWrite(memStream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }
            
        }
        #endregion

        # region btnEdit_Click
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    txtMessageBody.ReadOnly = false;
            //    btnProcess.Visible = true;
            //    btnEdit.Visible = false;
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);
            //}
            //catch (Exception ex) { }
        }
        # endregion btnEdit_Click

        #region btnSitaUpload_Click
        protected void btnSitaUpload_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (txtMessageBody.Text.Length > 0 )
            //    {
            //        FTP.SaveSITAMsg(txtMessageBody.Text.ToString(),  "File" + System.DateTime.Now.ToString("hhmmss"));
            //    }
            //}
            //catch (Exception ex) { }
        }
        #endregion

        #region btnDelete_Click
        protected void btnDeleteNew_Click(object sender, EventArgs e) 
        {
            try 
            {
                txtMessageBody.ReadOnly = true;
                btnProcess.Visible = false;
                btnEdit.Visible = true;
                string mailbox="";
                if (Incoming.Checked) 
                {
                    mailbox = "inbox";
                }
                if (Outgoing.Checked) 
                {
                    mailbox = "outbox";
                }
                if (hlfSrNo.Value.Length > 0) 
                {
                    if(OBJasb.DeleteMessage(hlfSrNo.Value.ToString(),mailbox))
                    {
                        lblStatus.Text = "Message Deleted Successfully";
                        lblStatus.ForeColor = Color.Green;
                        //Reload List View
                        btnList_Click(null, null);
                    }
                }
                
            }
            catch (Exception ex) { }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);

        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    txtMessageBody.ReadOnly = true;
            //    btnProcess.Visible = false;
            //    btnEdit.Visible = true;
            //    string mailbox = "";
            //    if (Incoming.Checked)
            //    {
            //        mailbox = "inbox";
            //    }
            //    if (Outgoing.Checked)
            //    {
            //        mailbox = "outbox";
            //    }
            //    if (hlfSrNo.Value.Length > 0)
            //    {
            //        if (OBJasb.DeleteMessage(hlfSrNo.Value.ToString(), mailbox))
            //        {
            //            lblStatus.Text = "Message Deleted Successfully";
            //            lblStatus.ForeColor = Color.Green;
            //            //Reload List View
            //            btnList_Click(null, null);
            //        }
            //    }

            //}
            //catch (Exception ex) { }
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);

        }
        #endregion

        #region btnProcess_Click
        protected void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                //#region Outbox Message Process
                //if (Outgoing.Checked == true)
                //{
                //    body = txtMessageBody.Text;
                //    #region Prepare Parameters
                //    object[] MailInfo = new object[11];
                //    int i = 0;
                //    //0
                //    MailInfo.SetValue(hlfSubject.Value, i);
                //    i++;

                //    //1
                //    MailInfo.SetValue(body, i);
                //    i++;

                //    //2
                //    MailInfo.SetValue(hlfSender.Value, i);
                //    i++;

                //    //3
                //    MailInfo.SetValue(hlfreceiver.Value, i);
                //    i++;

                //    //4
                //    MailInfo.SetValue(System.DateTime.Now, i);
                //    i++;

                //    //5
                //    MailInfo.SetValue(System.DateTime.Now, i);
                //    i++;

                //    //6
                //    MailInfo.SetValue(false, i);
                //    i++;

                //    //7
                //    MailInfo.SetValue("Active", i);
                //    i++;

                //    //8
                //    MailInfo.SetValue(hlfSubject.Value, i);
                //    i++;

                //    //9
                //    MailInfo.SetValue(System.DateTime.Now, i);
                //    i++;

                //    //10
                //    MailInfo.SetValue(System.DateTime.Now, i);

                //    #endregion Prepare Parameters
                //    int ID = 0;
                //    ID = OBJasb.AddMailinOutbox(MailInfo);
                //    MailInfo = null;
                //    if (ID >= 0)
                //    {
                //        lblStatus.Text = "Message Process Successfully";
                //        lblStatus.ForeColor = Color.Green;
                //        btnProcess.Visible = false;
                //        btnEdit.Visible = true;
                //        //Reload List View
                //        btnList_Click(null, null);

                //    }
                //    else
                //    {
                //        lblStatus.Text = "Failed To Process Message";
                //        lblStatus.ForeColor = Color.Red;
                //        btnProcess.Visible = false;
                //        btnEdit.Visible = true;
                //    }

                //}
                //#endregion
                
                //#region Inbox Message Process
                //if (Incoming.Checked == true)
                //{
                  
                //    if (OBJasb.UpdateMessageStatus(hlfSrNo.Value.ToString(),txtMessageBody.Text.Trim(), "Active"))
                //    {
                //        lblStatus.Text = "Message Successfully Submitted for Processing ";
                //        lblStatus.ForeColor = Color.Green;
                //        btnProcess.Visible = false;
                //        btnEdit.Visible = true;
                //        btnList_Click(null, null);
                //    }
                //    else
                //    {
                //        lblStatus.Text = "Failed To Process Message";
                //        lblStatus.ForeColor = Color.Red;
                //        btnProcess.Visible = false;
                //        btnEdit.Visible = true;
                //    }
                //}
                //#endregion
               // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);

            }
            catch (Exception ex)
            {
            }

        }

        #endregion btnProcess_Click

        # region lnkMsg_Click
        protected void lnkMsg_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            try
            {
                LinkButton btn = (LinkButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;

                if (gvr.RowIndex < 0)
                    return;

                rowindex = gvr.RowIndex;

                gridIndex = 0;
                gridIndex = rowindex;

                hlfSubject.Value = ((Label)gdvMsg.Rows[rowindex].FindControl("lbltype")).Text;
                if (Outgoing.Checked)
                {
                    hlfreceiver.Value = "";
                    hlfSender.Value = ((Label)gdvMsg.Rows[rowindex].FindControl("lblfrm")).Text;
                }
                if (Incoming.Checked)
                {
                    hlfSender.Value = "";
                    hlfreceiver.Value = ((Label)gdvMsg.Rows[rowindex].FindControl("lblfrm")).Text;
                }
                ds = (DataSet)Session["Message"];
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        hlfSrNo.Value = ds.Tables[0].Rows[rowindex]["Srno"].ToString();
                    }

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
        # endregion lnkMsg_Click

        #region btnClose_Click
        protected void btnClose_Click(object sender, EventArgs e)
        {
            //try 
            //{
            //    txtMessageBody.ReadOnly = true;
            //    btnProcess.Visible = false;
            //    btnEdit.Visible = true;
            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);

            //}
            //catch (Exception ex) { }
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HidePanelSplit();</script>", false);

        }
        #endregion
       
        #region Show Message
        protected void ShowMsg_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            try
            {

                Button btn = (Button)sender;

                GridViewRow gvr = (GridViewRow)btn.NamingContainer;

                if (gvr.RowIndex < 0)
                    return;

                rowindex = gvr.RowIndex;

                gridIndex = 0;
                gridIndex = rowindex;

                hlfSubject.Value = ((Label)gdvMsg.Rows[rowindex].FindControl("lbltype")).Text;
                if (Outgoing.Checked)
                {
                    hlfreceiver.Value = "";
                    hlfSender.Value = ((Label)gdvMsg.Rows[rowindex].FindControl("lblfrm")).Text;
                }
                if (Incoming.Checked)
                {
                    hlfSender.Value = "";
                    hlfreceiver.Value = ((Label)gdvMsg.Rows[rowindex].FindControl("lblfrm")).Text;
                }
                //hlfSender.Value = ((Label)gdvMsg.Rows[rowindex].FindControl("lblfrm")).Text;
                //hlfreceiver.Value = ((Label)gdvMsg.Rows[rowindex].FindControl("lblto")).Text;
                ds = (DataSet)Session["Message"];
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        hlfSrNo.Value = ds.Tables[0].Rows[rowindex]["Srno"].ToString();
                        //btnDelete_Click(null,null);
                        txtMessageBody.Text = Server.HtmlEncode(ds.Tables[0].Rows[rowindex]["body"].ToString());
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);

                    }

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
        # endregion lnkMsg_Click

        #region Button Export
        protected void btnExport_Click(object sender, EventArgs e)
        {
            // Session["Message"]

            lblStatus.Text = "";
            DataSet dsExp = new DataSet("Msg_dsExp");
            DataTable dt = new DataTable("Msg_dt");

            try
            {
                if ((DataSet)Session["Message"] == null)
                    return;
                dsExp = (DataSet)Session["Message"];
                dt = (DataTable)dsExp.Tables[0];

                if (Session["Message"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                string attachment = "attachment;filename=MessagingList.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";

                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }

                Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                //Response.Redirect("ListSpotRateApproval.aspx", false);

            }
            catch (Exception ex)
            {
                //lblStatus.Text = "Error Message :" + ex.Message;
                //lblStatus.ForeColor = Color.Red;

            }
            finally
            {
                dsExp = null;
                dt = null;
            }

        }
        #endregion

    }
}
