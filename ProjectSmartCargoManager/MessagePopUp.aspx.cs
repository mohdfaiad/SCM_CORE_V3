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
    public partial class MessagePopUp : System.Web.UI.Page
    {

        #region Variables
        ReportBAL OBJasb = new ReportBAL();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Session["Msg_Body"] != null && Session["Msg_ToEmailId"]!=null)
                    {
                        txtMessageBody.Text = Session["Msg_Body"].ToString();
                        txtEmailId.Text = Session["Msg_ToEmailId"].ToString();

                    }
                }
                catch (Exception ex)
                {

                }

                finally
                {
                    Session["Msg_Body"] = null;
                    Session["Msg_ToEmailId"] = null;
                }
            }
        }
        #endregion 

        #region Delete Button
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                txtMessageBody.ReadOnly = true;
                btnProcess.Visible = false;
                btnEdit.Visible = true;

                string mailbox = Request.QueryString["Msgtype"].ToString();
                int hlfSrNo = Convert.ToInt32(Request.QueryString["hlfSrNo"].ToString());

                if (hlfSrNo > 0)
                {
                    if (OBJasb.DeleteMessage(hlfSrNo.ToString(), mailbox))
                    {
                        //lblStatus.Text = "Message Deleted Successfully";
                        //lblStatus.ForeColor = Color.Green;
                        //Reload List View
                        //btnList_Click(null, null);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Message Deleted Successfully!!!');</script>", false);
                    }
                }

            }
            catch (Exception ex) { }
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Window.Close();</script>", false);
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            btnClose_Click(null, null);


        }
        #endregion

        #region Button Process
        protected void btnProcess_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                int hlfSrNo = Convert.ToInt32(Request.QueryString["hlfSrNo"].ToString());
                string mailbox = Request.QueryString["Msgtype"].ToString();

                #region Prepare Parameters

                object[] MailInfo = new object[4];
                int i = 0;

                //0
                MailInfo.SetValue(hlfSrNo.ToString(), i);
                i++;

                //1
                MailInfo.SetValue(mailbox, i);
                i++;

                //2
                MailInfo.SetValue(txtMessageBody.Text, i);
                i++;

                //3
                MailInfo.SetValue(txtEmailId.Text, i);

                #endregion

                #region Message Process

                    int ID = 0;
                    ID = OBJasb.EditMessage(MailInfo);

                    MailInfo = null;

                    if (ID >= 0)
                    {
                        lblStatus.Text = "Message reprocessed successfully";
                        lblStatus.ForeColor = Color.Green;
                        btnProcess.Visible = false;
                        txtMessageBody.ReadOnly = true;
                        txtEmailId.ReadOnly = true;
                        btnEdit.Visible = true;
                    }
                    else
                    {
                        lblStatus.Text = "Failed to reprocess the message. Please try again";
                        lblStatus.ForeColor = Color.Red;
                        btnProcess.Visible = true;
                        txtMessageBody.ReadOnly = false;
                        btnEdit.Visible = false;
                    }

                #endregion
                
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
               // btnClose_Click(null,null);

            }
            catch (Exception ex)
            {

            }

            finally
            {
                //Session["Msg_Subject"] = null;
                //Session["Msg_Body"] = null;
                //Session["Msg_Sender"] = null;
                //Session["Msg_Receiver"] = null;
            }
        }
        #endregion

        #region Button Edit
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                txtMessageBody.ReadOnly = false;
                txtEmailId.ReadOnly = false;
                btnProcess.Visible = true;
                btnEdit.Visible = false;
            }
            catch (Exception ex)
            {
               
            }
        }
        #endregion

        #region SitaUpload
        protected void btnSitaUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMessageBody.Text.Length > 0)
                {
                    FTP.SaveSITAMsg(txtMessageBody.Text.ToString(), "File" + System.DateTime.Now.ToString("hhmmss"));
                }
            }
            catch (Exception ex) { }
        }
        #endregion

        #region Button Close
        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                txtMessageBody.ReadOnly = true;
                btnProcess.Visible = false;
                btnEdit.Visible = true;
                
                Session["Msg_Body"] = null;
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "<script language=javascript>window.close();</script>");
            }
            catch (Exception ex)
            {
               
            }
        }
        #endregion
    }
}
