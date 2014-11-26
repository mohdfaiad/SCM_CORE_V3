#region IMPORT CLASSES

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Configuration;
using System.Net.Mail;
using System.Data.Common;
using System.Net;
using System.Web.Security;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Data.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
//using clsDataLib;
using BAL;
using System.Drawing;
using QID.DataAccess;

#endregion IMPORT CLASSES

namespace MyKFCargoNewProj
{
    public partial class FrmTracerAddFoundAwbPopup : System.Web.UI.Page
    {

        #region Variables
        MasterBAL objbal = new MasterBAL();
        #endregion

        #region PAGE LOAD EVENT

        protected void Page_Load(object sender, EventArgs e)
        {
            //ImageButton1.Attributes.Add("onclick", "javascript:GetGridRowValue()");   

            if (!IsPostBack)
            {
                if (Request.QueryString["AWBNo"].ToString() != null || Request.QueryString["AWBNo"].ToString() != string.Empty)
                {
                    Session["FileByte"] = null;
                    Cache["attchFile"] = string.Empty;
                    Cache["attchInputStream"] = string.Empty;
                    Cache["attchMimeType"] = string.Empty;
                    Cache["attchBinaryData"] = string.Empty;
                    Cache["Dt"] = string.Empty;
                    Cache["attchFileSize"] = string.Empty;
                    hdnAWBno.Value = Request.QueryString["AWBNo"].ToString();
                    hdnTracerNo.Value = Request.QueryString["TracerNo"].ToString();
                    txtMsdPcs.Text = Request.QueryString["MsdPcs"].ToString();
                    HdnBtn.Value = Request.QueryString["btnVis"].ToString();
                    HdnUID.Value = Request.QueryString["UID"].ToString();
                    //lblFoundPcs.Text = Request.QueryString["FndPcs"].ToString();
                    txtFndPcs.Text = "0";

                    

                    ddlFndType.SelectedValue = Request.QueryString["FndType"].ToString();

                    LoadALLStationCode();

                    ddlFndLoc.SelectedValue = Request.QueryString["FndLoc"].ToString();
                    ReloadUpdatedData();
                    LoadGridView();
                }
                else
                {
                    lblErrormsg.Visible = true;
                    lblErrormsg.Text = "Error While Populating Records. Please Close Window And Try Again..";
                }
            }
        }

        #endregion PAGE LOAD EVENT


        //#region ON ADD IMAGE BUTTON CLICK EVENT
        #region ON Save BUTTON CLICK EVENT
        /// <summary>
        /// To INSERT FOUND PCS AND ATTACHEMENT IF ANY TO DATABASE THIS EVENT IS CLICKED
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        //protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        //{

        protected void BtnEdit1_Click(object sender, EventArgs e)
        {
            //btnEdit.Attributes.Add("onclick", "javascript:GetGridRowValue()");
            hdnFndTypeTxt.Value = ddlFndType.SelectedValue;
            hdnFndLocTxt.Value = ddlFndLoc.SelectedValue;
            int iMsdPcs = 0;
            int iFndPcs = 0;
            //ReloadUpdatedData();
            
            if (txtMsdPcs.Text == string.Empty)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Short Pcs Cannot Be Blank.Please ReGenarate Tracer.');</SCRIPT>");
                return;
            }

            if (txtMsdPcs.Text == "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('For 0 Short Pcs Cannot sent Found Mail.Please ReGenarate Tracer.');</SCRIPT>");
                return;
            }

            if (txtMsdPcs.Text != string.Empty)
            {
                iMsdPcs = Convert.ToInt32(txtMsdPcs.Text);
            }

            if (txtFndPcs.Text != string.Empty)
            {
                iFndPcs = Convert.ToInt32(txtFndPcs.Text);
            }

            if (ddlFndLoc.SelectedIndex == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please select the Found/Lost Location');</SCRIPT>");
                return;
            }

            try
            {

                ArrayList AttachmentFiles = new ArrayList();
                ArrayList AttachInputStream = new ArrayList();
                ArrayList AttachMimeType = new ArrayList();
                ArrayList AttachBinaryData = new ArrayList();
                ArrayList AttachFileSize = new ArrayList();



                string[] DParam = new string[8];
                Object[] DValues = new object[8];
                SqlDbType[] DTypes = new SqlDbType[8];

                int i = 0;

                string[] DParamU = new string[8];
                SqlDbType[] DTypesU = new SqlDbType[8];
                object[] DValuesU = new object[8];

                DParamU.SetValue("AWBNo", i);
                DTypesU.SetValue(SqlDbType.VarChar, i);
                DValuesU.SetValue(hdnAWBno.Value, i);
                i++;

                DParamU.SetValue("LastUpdatedStatus", i);
                DTypesU.SetValue(SqlDbType.VarChar, i);
                DValuesU.SetValue(ddlFndType.SelectedValue, i);
                i++;

                DParamU.SetValue("TracerNo", i);
                DTypesU.SetValue(SqlDbType.BigInt, i);
                DValuesU.SetValue(hdnTracerNo.Value, i);
                i++;

                DParamU.SetValue("MissPcs", i);
                DTypesU.SetValue(SqlDbType.Int, i);
                DValuesU.SetValue(iMsdPcs, i);
                i++;

                DParamU.SetValue("FoundPcs", i);
                DTypesU.SetValue(SqlDbType.Int, i);
                DValuesU.SetValue(iFndPcs, i);
                i++;

                DParamU.SetValue("FoundLoc", i);
                DTypesU.SetValue(SqlDbType.VarChar, i);
                DValuesU.SetValue(ddlFndLoc.SelectedValue, i);
                i++;

                DParamU.SetValue("Remarks", i);
                DTypesU.SetValue(SqlDbType.VarChar, i);
                DValuesU.SetValue(txtRemarks.Text, i);
                i++;

                DParamU.SetValue("InsDate", i);
                DTypesU.SetValue(SqlDbType.DateTime, i);
                DValuesU.SetValue(DateTime.Now, i);
                i++;
                i = 0;
                Int64 FoundID = 0;

                Boolean Ins = UpdateData("SpUpdatePendAWBTracer", DParamU, DTypesU, DValuesU, ref FoundID); 
  
                ReloadUpdatedData();

                //#region SpADDTracerArchivedFileLive

                //if (Ins == true)
                //{
                //    if (iFndPcs != 0)
                //    {
                //        if (ddlFndType.SelectedValue == "Found")
                //        {
                //            if (Cache["attchFile"].ToString() != string.Empty)
                //            {
                //                AttachmentFiles = (ArrayList)Cache["attchFile"];
                //                AttachInputStream = (ArrayList)Cache["attchInputStream"];
                //                AttachMimeType = (ArrayList)Cache["attchMimeType"];
                //                AttachBinaryData = (ArrayList)Cache["attchBinaryData"];
                //                AttachFileSize = (ArrayList)Cache["attchFileSize"];
                //                for (int iCol = 0; iCol < AttachmentFiles.Count; iCol++)
                //                {
                //                    Stream IStream = (Stream)AttachInputStream[i];

                //                    DParam.SetValue("AWBNo", i);
                //                    DValues.SetValue(hdnAWBno.Value, i);
                //                    DTypes.SetValue(SqlDbType.VarChar, i);
                //                    i++;

                //                    DParam.SetValue("TracerNo", i);
                //                    DValues.SetValue(hdnTracerNo.Value, i);
                //                    DTypes.SetValue(SqlDbType.BigInt, i);
                //                    i++;

                //                    DParam.SetValue("BinaryData", i);
                //                    DValues.SetValue(AttachBinaryData[iCol], i);
                //                    DTypes.SetValue(SqlDbType.VarBinary, i);
                //                    i++;

                //                    DParam.SetValue("Name", i);
                //                    DValues.SetValue(AttachmentFiles[iCol], i);
                //                    DTypes.SetValue(SqlDbType.VarChar, i);
                //                    i++;

                //                    DParam.SetValue("MimeType", i);
                //                    DValues.SetValue(AttachMimeType[iCol], i);
                //                    DTypes.SetValue(SqlDbType.VarChar, i);
                //                    i++;

                //                    DParam.SetValue("FileSize", i);
                //                    DValues.SetValue(AttachFileSize[iCol], i);
                //                    DTypes.SetValue(SqlDbType.Int, i);
                //                    i++;

                //                    DParam.SetValue("UploadDate", i);
                //                    DValues.SetValue(DateTime.Now, i);
                //                    DTypes.SetValue(SqlDbType.DateTime, i);
                //                    i++;

                //                    DParam.SetValue("TracerTranID", i);
                //                    DValues.SetValue(FoundID, i);
                //                    DTypes.SetValue(SqlDbType.BigInt, i);
                //                    i++;

                //                    i = 0;

                //                    bool chkFlag = InsertData("SpADDTracerArchivedFileLive", DParam, DTypes, DValues);
                                    
                //                }
                //            }


                //            int RetVal = SendEmail(hdnAWBno.Value, ddlFndType.SelectedValue, ddlFndLoc.SelectedValue, iFndPcs);

                //            if (RetVal == 1)
                //            {
                //                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Email about Found Pcs Sent Sucessfully.');</SCRIPT>");
                //            }
                //            else
                //            {
                //                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Sending Email about Found Pcs Failed.');</SCRIPT>");
                //            }

                //        }
                //    }

                //}
                //else
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error While Inserting Records. Please Try Again');</SCRIPT>");
                //}


                //#endregion 


                int RetVal = SendEmail(hdnAWBno.Value, ddlFndType.SelectedValue, ddlFndLoc.SelectedValue, iFndPcs);

                if (RetVal == 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Email about Found Pcs Sent Sucessfully.');</SCRIPT>");
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Sending Email about Found Pcs Failed.');</SCRIPT>");
                }

            }
            catch (Exception ex)
            {

                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error While Inserting Records " + ex.Message + ". Please Try Again');</SCRIPT>");
            }

            string btnN = string.Empty;
            btnN = "ctl00_ContentPlaceHolder1_btnRefresh";

            ClientScript.RegisterStartupScript(this.GetType(), "ClosePopup1", "GetGridRowValue1('" + btnN + "');", true);

        }

        #endregion ON Save BUTTON CLICK EVENT


        #region UPDATING DATA


        /// Updates data in database through specified stored procedure.
        
      public bool UpdateData(string Procedure, string[] ColumnNames, SqlDbType[] DataType, object[] Values, ref Int64 FoundID)
        {
            SqlConnection sqCon = new SqlConnection();

            try
            {
                //string retQuery = "Select @@Identity";
                string StrCon = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
                sqCon.ConnectionString = StrCon;
                if (sqCon.State != System.Data.ConnectionState.Open)
                    sqCon.Open();
                SqlCommand sqCmd = new SqlCommand();
                sqCmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < ColumnNames.Length; i++)
                {
                    sqCmd.Parameters.Add("@" + ColumnNames.GetValue(i).ToString(), (SqlDbType)DataType.GetValue(i));
                    sqCmd.Parameters[i].Value = Values.GetValue(i);
                }
                sqCmd.Parameters.Add("@TrnID", SqlDbType.BigInt, 0, "TrnID");
                sqCmd.Parameters["@TrnID"].Direction = ParameterDirection.Output;
                sqCmd.Connection = sqCon;
                sqCmd.CommandText = Procedure;
                sqCmd.CommandTimeout = 0;
                if (sqCmd.ExecuteNonQuery() > 0)
                {
                    //sqCmd.CommandText = retQuery;

                    FoundID = (Int64)sqCmd.Parameters["@TrnID"].Value;
                    sqCon.Close();
                    return (true);
                }
                else
                {
                    FoundID = 0;
                    sqCon.Close();
                    return (false);
                }
            }
            catch (Exception)
            {
                FoundID = 0;
                sqCon.Close();
                return (false);
            }
        }

        #endregion UPDATING DATA


        # region SendEmail

        /// <summary>
        /// SENDING TRACER MAIL ON GENERATE TRACER BUTTON CLICK
        /// </summary>
        /// <param name="dsAwbNo"></param>
        /// <param name="mailType"></param>
        /// <param name="FoundLoc"></param>
        /// <param name="FoundCount"></param>
        /// <returns>INTEGER IN AS 1 SENT AND 0 NOT SENT</returns>

        private int SendEmail(string dsAwbNo, string mailType, string FoundLoc, Int32 FoundCount)
        {
            string SavedFileName = string.Empty;
            string strFileName = string.Empty;
            int RetVal = 1;

            try
            {
                string sQry = string.Empty;
                string ToEmailID = string.Empty;
                int cnt = 1;

                #region LIVE EMAIL ID's
                DataSet ds4 = new DataSet();
                ds4 = GetRecords(dsAwbNo, "Email");
                try
                {
                    if (ds4 != null)
                    {
                        if (ds4.Tables[0].Rows.Count >= 1)
                        {
                            cnt = 1;
                            for (int i = 0; i < ds4.Tables[0].Rows.Count; i++)
                            {
                                ToEmailID = ToEmailID + ds4.Tables[0].Rows[i][0].ToString() + ",";
                            }
                        }
                        else
                        {
                            cnt = 0;
                        }

                    }
                    else
                    {
                        cnt = 0;
                    }
                }
                catch (Exception)
                {
                    cnt = 0;
                }
                #endregion LIVE EMAIL ID's

                string FromEmailID = ConfigurationManager.AppSettings["FromEmailID"].ToString();
                string EmailPwd = ConfigurationManager.AppSettings["Pass"].ToString();

                string client = objbal.clientName();

                if (cnt == 1)
                {

                    #region Commented Below as Tracer is in Developement(This Id's used for Testing Team)
                    /// ToEmailID = "sanjeev@qidtech.com, ramakanth@qidtech.com,bhagyalaxmi@qidtech.com, afrin@qidtech.com";
                    #endregion Commented Below as Tracer is in Developement

                    MailMessage Mail = new MailMessage();
                    Mail.From = new MailAddress(FromEmailID);

                    //ToEmailID = ToEmailID.Replace(",,", ",");
                    //Mail.To.Add(ToEmailID.Trim(','));

                    #region Replace commas from Emil id

                    string[] ArrToEmailID = ToEmailID.Split(',');

                    if (ArrToEmailID.Length > 0)
                    {
                        ToEmailID = "";
                        for (int i = 0; i < ArrToEmailID.Length; i++)
                        {
                            if (ArrToEmailID[i].Trim() != "")
                            {
                                ToEmailID += ArrToEmailID[i].Trim() + ",";
                            }
                        }
                        if (ToEmailID.Trim() != "")
                        {
                            if (ToEmailID.Substring(ToEmailID.Length - 1, 1) == ",")
                            {
                                ToEmailID = ToEmailID.Substring(0, ToEmailID.Length - 1);
                            }
                        }

                    }

                    Mail.To.Add(ToEmailID.Trim(','));

                    #endregion

                    string body = string.Empty;
                    string subject = string.Empty;

                    
                    //subject = client + " " + "TRACER:"+" "+hdnTracerNo.Value + ": PCs Found AWB No is " + dsAwbNo + " QTY Found: " + FoundCount + " At: " + FoundLoc;
                    // updated on 22 Sept 2014 for [JIRA] (AC-67) 
                    subject = "TRACER:" + " " + hdnTracerNo.Value + ": PCs Found AWB No is " + dsAwbNo + " QTY Found: " + FoundCount + " At: " + FoundLoc;

                    Mail.Subject = subject;
                    Mail.IsBodyHtml = true;

                  #region OLD HTML Format Message
                    //body = "Dear ALL,<br/><br/> ";
                    //body += "Please Note PCS Found Reported At .<br/><br/> ";
                    //body += "Details are as follows.<br/><br/>";
                    //body += "<table border = 1>";

                    //body += "<tr><td>AIRWAY BILL NO</td><td>Found Pcs</td><td>Found Location</td><td>Remark</td><td>On Dated</td><td>Updated By</td></tr>";
                    ////body += "<tr><td> " + dsAwbNo + "</td><td> " + FoundCount + "</td><td> " + FoundLoc + "</td><td> " + txtRemarks.Text + "</td><td> " + Convert.ToString(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")) + "</td><td> " + HdnUID.Value.ToUpper() + "</td></tr>";

                    //body += "<tr><td> " + dsAwbNo + "</td><td> " + FoundCount + "</td><td> " + FoundLoc + "</td><td> " + txtRemarks.Text + "</td><td> " + Convert.ToString(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")) + "</td><td> " + Session["UserName"].ToString().ToUpper() + "</td></tr>";

                    //body += "</table><br/><br/>";
                    //body += "Thanks,<br/>QID Cargo Team<br/><br/>";
                    //body += "Note: This is an autogenerated E-mail. Please do not reply.";

                  #endregion

                    // Plain text Message

                    body = "Dear ALL," + "\r\n";
                    body += "Please Note PCS Found Reported As follows" + "\r\n";
                    //body += "Details are as follows" + "\r\n";
                    //body += "<table border = 1>";

                    body += "Airway Bill No:-"+ dsAwbNo +"\r\n";
                    body += "Found Pcs:-" + FoundCount +"\r\n";
                    body += "Found Location:-"+ FoundLoc +"\r\n";
                    body += "Remark:-" + txtRemarks.Text + "\r\n";
                    //body += "On Dated:-" + Convert.ToString(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"))+ "\r\n";
                    body += "On Dated:-" + Convert.ToString((Session["IT"].ToString())) + "\r\n";
                    body += "Updated By:-" + Session["UserName"].ToString().ToUpper() + "\r\n";
                    

                    //body += "<tr><td> " + dsAwbNo + "</td><td> " + FoundCount + "</td><td> " + FoundLoc + "</td><td> " + txtRemarks.Text + "</td><td> " + Convert.ToString(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")) + "</td><td> " + Session["UserName"].ToString().ToUpper() + "</td></tr>";

                    //body += "</table><br/><br/>";
                    //body += "Thanks,"+"\r\n";
                    //body += "QID Cargo Team" + "\r\n";
                    body += "Note: This is an autogenerated E-mail. Please do not reply.";

                    Mail.Body = body.ToString();

                    ArrayList AttachmentFiles = new ArrayList();
                    ArrayList AttachInputStream = new ArrayList();

                    string[] Document = new string[0];
                    MemoryStream[] Attachments = new MemoryStream[0];

                    string[] Extension = new string[0];

                    if (Cache["attchFile"] != null)
                    {
                        if (Cache["attchFile"].ToString() != string.Empty)
                        {
                            AttachmentFiles = (ArrayList)Cache["attchFile"];
                            AttachInputStream = (ArrayList)Cache["attchInputStream"];

                            List<byte[]> FDoc = new List<byte[]>();
                            FDoc = (List<byte[]>)Session["FileByte"];


                            for (int i = 0; i < AttachmentFiles.Count; i++)
                            {
                                MemoryStream IStream = new MemoryStream(FDoc[i]);
                                //Stream IStream = (Stream)AttachInputStream[i];
                                Mail.Attachments.Add(new Attachment(IStream, AttachmentFiles[i].ToString()));

                                Array.Resize(ref Document, Document.Length + 1);
                                Document[Document.Length - 1] = AttachmentFiles[i].ToString().Substring(0, AttachmentFiles[i].ToString().Length - 4);

                                Array.Resize(ref Attachments, Attachments.Length + 1);
                                Attachments[Attachments.Length - 1] = IStream;

                                Array.Resize(ref Extension, Extension.Length + 1);
                                Extension[Extension.Length - 1] = AttachmentFiles[i].ToString().Substring(AttachmentFiles[i].ToString().Length - 4);
                            }  
                        }
                        else
                        {
                            if (MyFile.PostedFile.FileName != string.Empty)
                            {
                                Mail.Attachments.Add(new Attachment(MyFile.PostedFile.InputStream, MyFile.PostedFile.FileName));

                            }

                        }
                    }


                    // Function addMsgToOutBox to Send Emails & store the uploaded files into Database.(sp_AddAttachmentMessage)
                    bool result = false;
                    //result = cls_BL.addMsgToOutBox(subject, body.ToString(),FromEmailID, ToEmailID, Attachments, Document, Extension);
                    result = cls_BL.addMsgToOutBox(subject, body.ToString(), "", ToEmailID, Attachments, Document, Extension, Convert.ToDateTime(Session["IT"].ToString()));



                    //SmtpClient smtp = new SmtpClient("smtpout.secureserver.net", 25);
                    ////smtp.Credentials = new NetworkCredential("kfatracker@qidtech.com", "qidtech#1");
                    //smtp.Credentials = new NetworkCredential(FromEmailID, EmailPwd);
                    //Mail.Priority = MailPriority.High;

                    //smtp.Send(Mail);
                    //if (Mail.Attachments.Count >= 1)
                    //{
                    //    Mail.Attachments.Clear();
                    //    Mail.Attachments.Dispose();
                    //}

                    //MyFile.PostedFile.InputStream.Dispose();

                }
                else
                {
                    RetVal = 0;
                }
            }

            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        # endregion SendEmail


        #region ON UPLOAD(ATTACHED FILE) BUTTON CLICK EVENT

        /// <summary>
        /// UPLOAD ATTACHED FILE WITH FOUND PCS TO SENT VIA EMAIL AND INSERT INTO DATABASE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                #region Old Code
                //if (MyFile.PostedFile.FileName != string.Empty)
                //{
                //    ArrayList AttachmentFiles = new ArrayList();
                //    ArrayList AttachInputStream = new ArrayList();

                //    ArrayList AttachMimeType = new ArrayList();
                //    ArrayList AttachBinaryData = new ArrayList();

                //    ArrayList AttachDT = new ArrayList();
                //    ArrayList AttachFileSize = new ArrayList();

                //    DataTable myDataTable = new DataTable();
                //    DataColumn myDataColumn;

                //    myDataColumn = new DataColumn();
                //    myDataColumn.DataType = Type.GetType("System.String");
                //    myDataColumn.ColumnName = "FileName";
                //    myDataTable.Columns.Add(myDataColumn);

                //    DataRow dr;
                //    dr = myDataTable.NewRow();

                //    if (Cache["attchFile"].ToString() != string.Empty)
                //    {

                //        AttachmentFiles = (ArrayList)Cache["attchFile"];
                //        AttachInputStream = (ArrayList)Cache["attchInputStream"];

                //        AttachMimeType = (ArrayList)Cache["attchMimeType"];
                //        AttachBinaryData = (ArrayList)Cache["attchBinaryData"];

                //        AttachDT = (ArrayList)Cache["Dt"];
                //        AttachFileSize = (ArrayList)Cache["attchFileSize"];

                //        if (Cache["Dt"].ToString() != string.Empty)
                //        {
                //            for (int i = 0; i < AttachDT.Count; i++)
                //            {
                //                dr = myDataTable.NewRow();
                //                dr["FileName"] = AttachDT[i];
                //                myDataTable.Rows.Add(dr);
                //            }
                //        }
                //    }

                //    AttachmentFiles.Add(MyFile.PostedFile.FileName);
                //    AttachInputStream.Add(MyFile.PostedFile.InputStream);

                //    AttachMimeType.Add(MyFile.PostedFile.ContentType);
                //    AttachDT.Add(MyFile.PostedFile.FileName);
                //    AttachFileSize.Add(MyFile.PostedFile.ContentLength);

                //    Cache["attchFile"] = AttachmentFiles;
                //    Cache["attchInputStream"] = AttachInputStream;

                //    Cache["attchMimeType"] = AttachMimeType;
                //    Cache["attchFileSize"] = AttachFileSize;

                //    Cache["Dt"] = AttachDT;
                //    string contentType = MyFile.PostedFile.ContentType;
                //    string fileName = MyFile.PostedFile.FileName;

                //    byte[] byteArray = MyFile.FileBytes;
                //    Binary LinqBinary = new Binary(byteArray);
                //    AttachBinaryData.Add(LinqBinary.ToArray());
                //    Cache["attchBinaryData"] = AttachBinaryData;


                //    dr = myDataTable.NewRow();
                //    dr["FileName"] = fileName;

                //    myDataTable.Rows.Add(dr);
                //    grdCurrArchived.DataSource = null;
                //    grdCurrArchived.DataSource = myDataTable;
                //    grdCurrArchived.DataBind();

                //}
                #endregion 

                if (MyFile.PostedFile.FileName != string.Empty)
                {
                    ArrayList AttachmentFiles = new ArrayList();
                    ArrayList AttachInputStream = new ArrayList();
                    ArrayList AttachDT = new ArrayList();

                    List<byte[]> FDoc = new List<byte[]>();
                    DataTable myDataTable = new DataTable();
                    DataColumn myDataColumn;

                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = Type.GetType("System.String");
                    myDataColumn.ColumnName = "FileName";
                    myDataTable.Columns.Add(myDataColumn);

                    DataRow dr;
                    dr = myDataTable.NewRow();

                    if (Cache["attchFile"].ToString() != string.Empty)
                    {
                        AttachmentFiles = (ArrayList)Cache["attchFile"];
                        AttachInputStream = (ArrayList)Cache["attchInputStream"];
                        AttachDT = (ArrayList)Cache["Dt"];

                        if (Cache["Dt"].ToString() != string.Empty)
                        {
                            for (int i = 0; i < AttachDT.Count; i++)
                            {
                                dr = myDataTable.NewRow();
                                dr["FileName"] = AttachDT[i];
                                myDataTable.Rows.Add(dr);
                            }
                        }
                    }

                    Stream fs = MyFile.PostedFile.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    if (Session["FileByte"] != null)
                    {
                        FDoc = (List<byte[]>)Session["FileByte"];
                        FDoc.Add(bytes);
                    }
                    else
                    {
                        FDoc.Add(bytes);
                    }
                    Session["FileByte"] = FDoc;

                    AttachmentFiles.Add(MyFile.PostedFile.FileName);
                    AttachInputStream.Add(MyFile.PostedFile.InputStream);
                    AttachDT.Add(MyFile.PostedFile.FileName);

                    Cache["attchFile"] = AttachmentFiles;
                    Cache["attchInputStream"] = AttachInputStream;
                    Cache["Dt"] = AttachDT;

                    dr = myDataTable.NewRow();
                    dr["FileName"] = MyFile.PostedFile.FileName;

                    myDataTable.Rows.Add(dr);
                    grdCurrArchived.DataSource = null;
                    grdCurrArchived.DataSource = myDataTable;
                    grdCurrArchived.DataBind();
                    grdCurrArchived.Visible = true;

                }

                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please Add Attachment To Upload...');</script>");
                    MyFile.Focus();
                }

            }
            catch (Exception)
            {

            }
        }

        #endregion ON UPLOAD(ATTACHED FILE) BUTTON CLICK EVENT


        #region BIND DATASET WITH GRIDVIEW FUNCTION

        /// <summary>
        /// BINDING GRIDVIEW WITH RETURNED DATASET 
        /// </summary>

        public void LoadGridView()
        {
            DataSet oDs = new DataSet();
            grdArchived.Visible = false;

            string[] DParam = new string[1];
            object[] DValues = new object[1];
            SqlDbType[] DTypes = new SqlDbType[1];

            int i = 0;

            //DParam.SetValue("AWBNo", i);
            //DValues.SetValue(hdnAWBno.Value, i);
            //DTypes.SetValue(SqlDbType.VarChar, i);
            //i++;

            DParam.SetValue("TracerNo", i);
            DValues.SetValue(hdnTracerNo.Value, i);
            DTypes.SetValue(SqlDbType.BigInt, i);
            i++;

            //string TracerNo = Request.QueryString["TracerNo"].ToString();

            string ErrMsg = string.Empty;

            oDs = SelectRecords("SpGetTracerArchivedFileLive", DParam, DTypes, DValues, ref ErrMsg);

            if (oDs != null)
            {
                if (oDs.Tables[0].Rows.Count >= 1)
                {
                    grdArchived.DataSource = null;
                    grdArchived.DataSource = oDs;
                    grdArchived.DataBind();
                    Session["Attachment"] = oDs;
                    grdArchived.Visible = true;
                }
                else
                {
                    grdArchived.DataSource = null;
                    grdArchived.Visible = false;
                }
            }
            else
            {
                grdArchived.DataSource = null;
                grdArchived.Visible = false;
            }

        }

        #endregion BIND DATASET WITH GRIDVIEW FUNCTION


        #region INSERTING DATA INTO TABLE

        /// <summary>
        /// Inserts data in database through stored procedure.
        /// </summary>
        /// <param name="Procedure">Name of stored procedure to be executed.</param>
        /// <param name="ColumnNames">Columns in which data is to be inserted.</param>
        /// <param name="DataType">Data type of respective columns.</param>
        /// <param name="Values">Value to be inserted in each column.</param>
        /// <returns>True if insert successful.</returns>

        public bool InsertData(string Procedure, string[] ColumnNames, SqlDbType[] DataType, object[] Values)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                string StrCon = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
                con.ConnectionString = StrCon;
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < ColumnNames.Length; i++)
                {
                    cmd.Parameters.Add("@" + ColumnNames.GetValue(i).ToString(), (SqlDbType)DataType.GetValue(i));
                    cmd.Parameters[i].Value = Values.GetValue(i);
                }
                cmd.Connection = con;
                cmd.CommandText = Procedure;
                if (cmd.ExecuteNonQuery() > 0)
                    return (true);
                else
                {
                    return (false);
                }
            }
            catch (Exception)
            {
                return (false);
            }
        }

        #endregion INSERTING DATA INTO TABLE


        #region SelectStoreProcedureRecords

        /// <summary>
        /// Fetch records using Stored Procedure without parameters.
        /// </summary>
        /// <param name="selectProcedure">Name of Stored Procedure.</param>
        /// <returns>Dataset containing records returned by executing stored procedure.</returns>

        public DataSet SelectRecords(string selectProcedure, string[] DParam, SqlDbType[] DTypes, Object[] DValues, ref string ErrorMsg)
        {
            SqlConnection sqCon = new SqlConnection();
            DataSet oDs = new DataSet();
            try
            {
                string StrCon = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
                sqCon.ConnectionString = StrCon;
                if (sqCon.State != System.Data.ConnectionState.Open)
                    sqCon.Open();
                SqlDataAdapter Adapter = new SqlDataAdapter();
                SqlCommand sqCmd = new SqlCommand();
                sqCmd.CommandType = CommandType.StoredProcedure;
                sqCmd.CommandText = selectProcedure;
                sqCmd.CommandType = CommandType.StoredProcedure;
                sqCmd.Connection = sqCon;
                Adapter.SelectCommand = sqCmd;
                for (int i = 0; i < DParam.Length; i++)
                {
                    sqCmd.Parameters.Add("@" + DParam[i], DTypes[i]).Value = DValues[i];
                }
                Adapter.Fill(oDs);
                if (sqCon.State == System.Data.ConnectionState.Open)
                    sqCon.Close();
            }
            catch (SqlException SqlEx)
            {
                sqCon.Close();
                oDs = null;
                ErrorMsg = SqlEx.Message;
            }
            catch (Exception ex)
            {
                sqCon.Close();
                ErrorMsg = ex.Message;
                oDs = null;
            }

            return oDs;
        }

        /// <summary>
        /// Fetch records using Stored Procedure without parameters.
        /// </summary>
        /// <param name="selectProcedure">Name of Stored Procedure.</param>
        /// <returns>Dataset containing records returned by executing stored procedure.</returns>

        public DataSet SelectRecords(string selectProcedure)
        {
            SqlConnection sqCon = new SqlConnection();
            DataSet dataSet = new DataSet();
            try
            {
               // string StrCon = ConfigurationManager.ConnectionStrings["ConStr"].ToString();

                string StrCon = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
                sqCon.ConnectionString = StrCon;
                if (sqCon.State != System.Data.ConnectionState.Open)
                    sqCon.Open();
                SqlDataAdapter Adapter = new SqlDataAdapter();
                SqlCommand sqCmd = new SqlCommand();
                sqCmd.CommandType = CommandType.StoredProcedure;
                sqCmd.CommandText = selectProcedure;
                sqCmd.CommandType = CommandType.StoredProcedure;
                sqCmd.Connection = sqCon;
                Adapter.SelectCommand = sqCmd;

                Adapter.Fill(dataSet);

            }
            catch (SqlException)
            {
                dataSet = null;
            }
            catch (Exception)
            {
                dataSet = null;
            }

            return dataSet;
        }

        #endregion SelectStoreProcedureRecords


        #region BIND DATASET WITH FOUND AIRPORT DROPDOWNLIST FUNCTION

        /// <summary>
        /// BIND FOUND AIRPORT DROP DOWN LIST WITH RETURNED DATASET FROM FUNCTION
        /// </summary>

        public void LoadALLStationCode()
        {
            DataSet oDs = new DataSet();
            oDs = SelectRecords("SpGetAllStationForTracer");
            if (oDs != null)
            {
                if (oDs.Tables[0].Rows.Count >= 1)
                {
                    ddlFndLoc.DataSource = oDs;
                    ddlFndLoc.DataTextField = "AirportCode";
                    ddlFndLoc.DataValueField = "AirportCode";
                    ddlFndLoc.DataBind();
                    ddlFndLoc.Items.Insert(0, "SELECT");
                }
            }
        }

        #endregion BIND DATASET WITH FOUND AIRPORT DROPDOWNLIST FUNCTION


        #region GRIDVIEW ON ROW CREATED EVENT

        /// <summary>
        /// ON GRIDVIEW ROW CREATION HIDE FEW ROWS WHICH REQUIRED FOR BACK PROCESS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdArchived_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].CssClass = "hiddencol1";
                e.Row.Cells[2].CssClass = "hiddencol1";
                e.Row.Cells[3].CssClass = "hiddencol1";
                //e.Row.Cells[4].CssClass = "hiddencol1";
                //e.Row.Cells[5].CssClass = "hiddencol1";

            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].CssClass = "hiddencol1";
                e.Row.Cells[2].CssClass = "hiddencol1";
                e.Row.Cells[3].CssClass = "hiddencol1";
                //e.Row.Cells[5].CssClass = "hiddencol1";
                //e.Row.Cells[4].CssClass = "hiddencol1";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].CssClass = "hiddencol1";
                e.Row.Cells[2].CssClass = "hiddencol1";
                e.Row.Cells[3].CssClass = "hiddencol1";
                //e.Row.Cells[5].CssClass = "hiddencol1";
                //e.Row.Cells[4].CssClass = "hiddencol1";
            }
        }

        #endregion GRIDVIEW ON ROW CREATED EVENT


        #region COMMENTED GRIDVIEW EVENT FOR TESTING PURPOSE

        //protected void grdArchived_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    #region COMMENTED BELOW CODE SHOULD BE APPLICABLE FOR VIEWING FOUND HISTORY
        //    //if (e.CommandName == "Download")
        //    //{

        //    //    int index = Convert.ToInt32(e.CommandArgument);

        //    //    GridViewRow row = grdArchived.Rows[index];
        //    //    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

        //    //    string Name = ((Label)grdArchived.Rows[index].FindControl("lblName")).Text;
        //    //    string StrBinaryData = ((Label)grdArchived.Rows[index].FindControl("lblBinaryData")).Text;
        //    //    byte[] BinaryData = encoding.GetBytes(StrBinaryData);
        //    //    string MimeType = ((Label)grdArchived.Rows[index].FindControl("lblMimeType")).Text;
        //    //    string AWBNo = ((Label)grdArchived.Rows[index].FindControl("lblAWBNo")).Text;
        //    //    string TracerNo = ((Label)grdArchived.Rows[index].FindControl("lblTracerNo")).Text;
        //    //    //byte[] ArrBinaryData = new byte(BinaryData);
        //    //    Response.Clear();
        //    //    Response.ContentType = MimeType;
        //    //    Response.AddHeader("content-disposition", string.Format("attachment; {0}",Name));
        //    //    Response.BinaryWrite(BinaryData);
        //    //    Response.End();
        //    //}
        //    #endregion

        //    if (e.CommandName == "Delete")
        //    {

        //        //int index = Convert.ToInt32(e.CommandArgument);

        //        //ArrayList AttachmentFiles = new ArrayList();
        //        //ArrayList AttachInputStream = new ArrayList();
        //        ////HttpFileCollection Files = HttpContext.Current.Request.Files;

        //        //if (Cache["attchFile"].ToString() != string.Empty)
        //        //{
        //        //    AttachmentFiles = (ArrayList)Cache["attchFile"];
        //        //    AttachInputStream = (ArrayList)Cache["attchInputStream"];
        //        //    //AttachmentFiles.RemoveAt(index);
        //        //    //AttachInputStream.RemoveAt(index);
        //        //    //Cache["attchFile"] = AttachmentFiles;
        //        //    //Cache["attchInputStream"] = AttachInputStream;
        //        //}

        //        //GridViewRow row = grdArchived.Rows[index];
        //        //System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

        //        //Int64 ArchivedID = Convert.ToInt64(grdArchived.DataKeys[index].Value.ToString());
        //        //string AWBNo = ((Label)grdArchived.Rows[index].FindControl("lblAWBNo")).Text;
        //        //string TracerNo = ((Label)grdArchived.Rows[index].FindControl("lblTracerNo")).Text;

        //        //string[] DParam = new string[3];
        //        //object[] DValues = new object[3];
        //        //SqlDbType[] DTypes = new SqlDbType[3];
        //        //int i = 0;

        //        //DParam.SetValue("AWBNo", i);
        //        //DValues.SetValue(AWBNo, i);
        //        //DTypes.SetValue(SqlDbType.VarChar, i);
        //        //i++;

        //        //DParam.SetValue("TracerNo", i);
        //        //DValues.SetValue(TracerNo, i);
        //        //DTypes.SetValue(SqlDbType.BigInt, i);
        //        //i++;

        //        //DParam.SetValue("ArchivedFileId", i);
        //        //DValues.SetValue(ArchivedID, i);
        //        //DTypes.SetValue(SqlDbType.BigInt, i);
        //        //i++;

        //        /////BELOW BL CLASS IS CALLED TO DELETE ROW FROM GRIDVIEW
        //        //bool chkFlag = InsertData("SpDeleteTracerArchivedFile", DParam, DTypes, DValues);
        //        // if (chkFlag == true)
        //        // {
        //        //     //grdArchived.DeleteRow(index);
        //        //     //LoadGridView();
        //        // }
        //    }
        //}

        //protected void grdArchived_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    //LoadGridView();
        //}

        #endregion COMMENTED GRIDVIEW EVENT FOR TESTING PURPOSE


        #region ON CURRENTLY UPLOADED GRIDVIEW DELETE IMAGE BUTTON CLICK EVENT

        /// <summary>
        /// TO REMOVE AURRENTLY UPLOAD FILE ATTACHED TO LIST
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdCurrArchived_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {

                int index = Convert.ToInt32(e.CommandArgument);

                ArrayList AttachmentFiles = new ArrayList();
                ArrayList AttachInputStream = new ArrayList();
                ArrayList AttachDT = new ArrayList();

                if (Cache["attchFile"].ToString() != string.Empty)
                {
                    AttachmentFiles = (ArrayList)Cache["attchFile"];
                    AttachInputStream = (ArrayList)Cache["attchInputStream"];
                    AttachDT = (ArrayList)Cache["Dt"];
                    AttachmentFiles.RemoveAt(index);
                    AttachInputStream.RemoveAt(index);
                    AttachDT.RemoveAt(index);
                    Cache["attchFile"] = AttachmentFiles;
                    Cache["attchInputStream"] = AttachInputStream;
                    Cache["Dt"] = AttachDT;
                    grdCurrArchived.DeleteRow(index);
                }
            }
        }

        #endregion ON CURRENTLY UPLOADED GRIDVIEW DELETE IMAGE BUTTON CLICK EVENT


        #region AFTER CURRENTLY UPLOADED GRIDVIEW ROW DELETED EVENT

        /// <summary>
        /// AFTER ROW DELETED FROM CURRENTLY UPLOADED FILE GRIDVIEWTHIS EVENT IS FIRED TO RELOAD GRIDVIEW AND UNBOUND DATA FROM ARRAY LIST
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdCurrArchived_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ArrayList AttachDT = new ArrayList();
            AttachDT = (ArrayList)Cache["Dt"];
            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FileName";
            myDataTable.Columns.Add(myDataColumn);



            DataRow dr;
            dr = myDataTable.NewRow();
            if (Cache["Dt"].ToString() != string.Empty)
            {
                for (int i = 0; i < AttachDT.Count; i++)
                {
                    dr = myDataTable.NewRow();
                    dr["FileName"] = AttachDT[i];
                    myDataTable.Rows.Add(dr);
                }
            }
            grdCurrArchived.DataSource = null;
            grdCurrArchived.DataSource = myDataTable;
            grdCurrArchived.DataBind();
        }

        #endregion AFTER CURRENTLY UPLOADED GRIDVIEW ROW DELETED EVENT


        protected void ddlFndLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFndLoc.SelectedIndex == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please Select Found Location Mandatory');</SCRIPT>");
            }
        }

        #region RELOADING UPDATED RECORDS TO AVOID ERRORS

        public void ReloadUpdatedData()
        {
            DataSet oDs = new DataSet();
            string errMsg = string.Empty;
            try
            {
                string[] DParam = new string[2];
                object[] DValues = new object[2];
                SqlDbType[] DTypes = new SqlDbType[2];

                DParam.SetValue("AWBNo", 0);
                DValues.SetValue(hdnAWBno.Value, 0);
                DTypes.SetValue(SqlDbType.VarChar, 0);

                DParam.SetValue("TracerNo", 1);
                DValues.SetValue(hdnTracerNo.Value, 1);
                DTypes.SetValue(SqlDbType.BigInt, 1);

                oDs = SelectRecords("SpGetTracerMissedPcsLive", DParam, DTypes, DValues, ref errMsg);
                if (oDs != null)
                {
                    if (oDs.Tables[0].Rows.Count >= 1)
                    {
                        txtMsdPcs.Text = oDs.Tables[0].Rows[0][0].ToString();
                    }
                    else
                    {
                        txtMsdPcs.Text = "0";
                    }
                    txtFndPcs.Text = "0";
                }
                else
                {
                    txtFndPcs.Text = "0";
                    txtMsdPcs.Text = "0";
                }
            }
            catch (Exception)
            {
                txtFndPcs.Text = "0";
                txtMsdPcs.Text = "0";
            }
        }
        #endregion

        protected void grdArchived_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {

                foreach (TableCell tc in e.Row.Cells)
                {
                    tc.Attributes["style"] = "border-color:#990000";
                }
            }
        }

        protected void grdCurrArchived_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {

                foreach (TableCell tc in e.Row.Cells)
                {
                    tc.Attributes["style"] = "border-color:#990000";
                }
            }
        }


        #region RETRIVING DATASET FOR AGENTNAME/EMAILID FUNCTION

        /// <summary>
        /// RETRIEVING DATA FOR EITHER AGENT NAME IN TRACER GENERATION OR LIST OF EMAIL ID's
        /// </summary>
        /// <param name="AwbNo"></param>
        /// <param name="GetType"></param>
        /// <returns></returns>

        public DataSet GetRecords(string AwbNo, string GetType)
        {
            DataSet oDs = new DataSet();
            try
            {
                string spName = string.Empty;

                SQLServer ObjData = new SQLServer(Global.GetConnectionString());
                string[] DParam = new string[1];
                object[] DValues = new object[1];
                SqlDbType[] DTypes = new SqlDbType[1];

                DParam.SetValue("AWBNo", 0);
                DValues.SetValue(AwbNo, 0);
                DTypes.SetValue(SqlDbType.VarChar, 0);

                if (GetType == "Email")
                {
                    spName = "spGetTracerEmailID";
                }
                else
                {
                    spName = "spTracerGetAgent";
                }
                oDs = ObjData.SelectRecords(spName, DParam, DValues, DTypes);

                if (oDs != null)
                {
                    if (oDs.Tables[0].Rows.Count <= 0)
                    {
                        oDs = null;
                    }
                }
                else
                {
                    oDs = null;
                }


            }
            catch (Exception)
            {
                oDs = null;

            }
            return oDs;
        }

        #endregion RETRIVING DATASET FOR AGENTNAME/EMAILID FUNCTION


        #region Button Display
        protected void btnDisplay_Click(object sender, EventArgs e)
        {
            try
            {
                //int SrNo=0;
                //Session["DocumentsData"] = null;
                InvoiceImage.Visible = false;
                //btnNext.Visible = false;
                //btnPrev.Visible = false;
                //lblPageNo.Text = "";
                int count = 0;

                for (int i = 0; i < grdArchived.Rows.Count; i++)
                {
                    if (((RadioButton)(grdArchived.Rows[i].FindControl("rdbePouch"))).Checked)
                    {
                        count++;
                    }
                }

                if (count == 1)
                {
                    for (int j = 0; j < grdArchived.Rows.Count; j++)
                    {
                        if (((RadioButton)(grdArchived.Rows[j].FindControl("rdbePouch"))).Checked)
                        {
                            string SrNo = ((Label)grdArchived.Rows[j].FindControl("lblSerialNumber")).Text;
                            string FName = ((Label)grdArchived.Rows[j].FindControl("lblName")).Text;
                            string MIMEType = ((Label)grdArchived.Rows[j].FindControl("lblMimeType")).Text;
                            string Attachment = ((Label)grdArchived.Rows[j].FindControl("lblAttachment")).Text;

                            #region Commented
                            //DataSet ds = new DataSet();
                            ////grdArchived.Visible = false;
                            //string[] DParam = new string[2];
                            //object[] DValues = new object[2];
                            //SqlDbType[] DTypes = new SqlDbType[2];

                            //int i = 0;

                            //DParam.SetValue("FName", i);
                            //DValues.SetValue(FName, i);
                            //DTypes.SetValue(SqlDbType.VarChar, i);
                            //i++;

                            //DParam.SetValue("MIMEType", i);
                            //DValues.SetValue(MIMEType, i);
                            //DTypes.SetValue(SqlDbType.NVarChar, i);
                            //i++;

                            //string ErrMsg = string.Empty;
                            //ds = SelectRecords("SpGetTracerArchivedFiles", DParam, DTypes, DValues, ref ErrMsg);
                            #endregion

                            DataSet ds = new DataSet();

                            ds = (DataSet)Session["Attachment"];
                            

                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        int SrNum = ds.Tables[0].Rows.Count;
                                        //Session["DocumentsData"] = ds;
                                        //if (ds.Tables[0].Rows[0]["DocumentExtension"].ToString() != ".pdf")
                                        //{
                                        InvoiceImage.ImageUrl = "~/ShowTracerImages.ashx?SrNo=" + SrNo;// ds.Tables[0].Rows[0]["SrNo"].ToString();
                                            InvoiceImage.Visible = true;
                                            // btnNext.Visible = true;
                                            //btnPrev.Visible = true;
                                            //lblPageNo.Text = "Page : " + imagecount.ToString();
                                        //}
                                        //else
                                        //{
                                            //InvoiceImage.Visible = false;
                                            // btnNext.Visible = true;
                                            //btnPrev.Visible = true;
                                            //lblPageNo.Text = "Page : " + imagecount.ToString();

                                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Download(" + imagecount + ");</SCRIPT>", false);
                                        //}
                                    }
                                }
                            }
                             


                        }
                            else
                            {
                                lblErrormsg.Text = "No documents uploaded to preview!";
                                lblErrormsg.ForeColor = Color.Red;
                            }


                        }
                    
                    }
                    

                else
                    if (count > 1)
                    {
                        lblErrormsg.Text = "Please select a single document for preview!";
                        lblErrormsg.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblErrormsg.Text = "Please select a document for preview!";
                        lblErrormsg.ForeColor = Color.Red;

                    }


            }
            catch (Exception ex)
            {
              
            }
        }

        #endregion



    }
}
