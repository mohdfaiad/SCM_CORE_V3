#region IMPORT CLASSES

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//using Quantumid.Act.mykfcargo.Business;
using System.Collections.Generic;
using System.Data.Common;
using System.Net;
using System.IO;
using System.Globalization;
//using clsDataLib;
using QID.DataAccess;
using BAL;
using System.Diagnostics;
using System.Drawing;

#endregion IMPORT CLASSES

namespace MyKfCargo
{
    public partial class FrmNewTracerPopup : System.Web.UI.Page
    {
        #region Variables
        public static string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
        SQLServer db = new SQLServer(Global.GetConnectionString());

        cls_BL objBAL = new cls_BL();
        MasterBAL objbal = new MasterBAL();

        #endregion Variables
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    Session["FileByte"] = null;

                    Cache["attchFile"] = string.Empty;
                    txtAWBPrefix.Text = Convert.ToString(Session["awbPrefix"]);
                    txtAWBPrefix.Enabled = false;

                    txtDate.Attributes.Add("readonly", "readonly");
                    LoadAirportCode();//pnlname
                    if (Request.QueryString["pnlName"].ToString() != null || Request.QueryString["pnlName"].ToString() != string.Empty || Request.QueryString["TracerNo"].ToString()!=null)
                    {
                        HdnPanel.Value = Request.QueryString["pnlname"].ToString();
                        HdnUID.Value = Request.QueryString["UID"].ToString();
                        string TracerNo = Request.QueryString["TracerNo"].ToString();

                        if (TracerNo != null && TracerNo != "" && TracerNo !="undefined")
                        {

                            try
                            {
                                //string TracerNo = Request.QueryString["TracerNo"].ToString();

                                DataSet ds = new DataSet();

                                string[] QueryPname = new string[1];
                                QueryPname[0] = "TracerNo";

                                object[] QueryValue = new object[1];
                                QueryValue.SetValue(Convert.ToInt64(TracerNo), 0);

                                SqlDbType[] QueryType = new SqlDbType[1];
                                QueryType[0] = SqlDbType.BigInt;

                                SQLServer ObjData = new SQLServer(Global.GetConnectionString());

                                ds = ObjData.SelectRecords("SP_GetTracerIsGeneratedDetails", QueryPname, QueryValue, QueryType);

                                if (ds != null && ds.Tables[0].Rows.Count > 0)
                                {
                                    txtFlightNo.Text = ds.Tables[0].Rows[0]["FltNo"].ToString();
                                    txtFlightNo.Enabled = false;
                                    ddlOrg.SelectedValue = ds.Tables[0].Rows[0]["Origin"].ToString();
                                    ddlOrg.Enabled = false;
                                    ddlDest.SelectedValue = ds.Tables[0].Rows[0]["Dest"].ToString();
                                    ddlDest.Enabled = false;
                                    txtDate.Text = ds.Tables[0].Rows[0]["AWBDate"].ToString();
                                    txtDate.Enabled = false;
                                    txtAWBPrefix.Text = ds.Tables[0].Rows[0]["AWBPrex"].ToString();
                                    txtAWBPrefix.Enabled = false;
                                    txtAwbNoTr.Text = ds.Tables[0].Rows[0]["AWBNo"].ToString();
                                    txtAwbNoTr.Enabled = false;
                                    txtTotalPcs.Text = ds.Tables[0].Rows[0]["SentPcs"].ToString();
                                    txtTotalPcs.Enabled = false;
                                    txtWgt.Text = ds.Tables[0].Rows[0]["ChargebleWgt"].ToString();
                                    txtWgt.Enabled = false;
                                    txtShortage.Text = ds.Tables[0].Rows[0]["MissedPcs"].ToString();

                                    txtContents.Text = ds.Tables[0].Rows[0]["ContentType"].ToString();
                                    txtPkng.Text = ds.Tables[0].Rows[0]["Packaging"].ToString();
                                    txtOrgAgent.Text = ds.Tables[0].Rows[0]["Consignor"].ToString();
                                    txtDestAgent.Text = ds.Tables[0].Rows[0]["Consignee"].ToString();
                                    txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();

                                }

                            }
                            catch (Exception ex)
                            {

                            }
                        }

                    }
                    
                }
            }
            catch (Exception ex)
            { }
        }


        #region ON GENERATE TRACER UPLOAD(ATTACHED FILE) BUTTON CLICK EVENT

        /// <summary>
        /// UPLOAD ATTACHED FILE WITH TRACER TO SENT VIA EMAIL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void btnUpd_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (MyFile.Value != string.Empty)
                {
                    ArrayList AttachmentFiles = new ArrayList();
                    ArrayList AttachInputStream = new ArrayList();
                    ArrayList AttachDT = new ArrayList();

                    List<byte []> FDoc=new List<byte []>();
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
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Please Add Attachment To Upload..." + ex.Message + "');</script>");
                MyFile.Focus();
            }

        }

        #endregion ON GENERATE TRACER UPLOAD(ATTACHED FILE) BUTTON CLICK EVENT


        #region Button GENERATE TRACER
        
        /// ON CLICK OF GENERATE TRACER BUTTON IN GENERATE AND SEND TRACER FRAME 

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            string FDate = txtDate.Text;
            //FDate = FDate.Substring(3, 3) + FDate.Substring(0, 3) + FDate.Substring(6, 4);
            FDate = FDate.Substring(0, 3) + FDate.Substring(3, 3) + FDate.Substring(6, 4);

            DateTime vDate = DateTime.ParseExact(FDate,"dd/MM/yyyy",null);
            if (vDate > Convert.ToDateTime(Session["IT"].ToString()))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('AWB Date cannot be more than current date');</SCRIPT>");
                txtDate.Text = string.Empty;
                txtDate.Focus();
                return;
            }
            if (txtWgt.Text == "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Total Weight can be greater than 0');</SCRIPT>");
                txtWgt.Text = string.Empty;
                txtWgt.Focus();
                return;
            }

            if (Convert.ToInt32(txtShortage.Text) > Convert.ToInt32(txtTotalPcs.Text))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Shortage cannot be more then Total Pcs');</SCRIPT>");
                txtShortage.Text = string.Empty;
                txtShortage.Focus();
                return;
            }

            try
            {
                errMsg = string.Empty;

                //if (hdnFltVal.Value != "Passed")
                //{
                //    int retVal = CheckFiltersNoExists("AWB", txtAWBPrefix.Text+txtAwbNoTr.Text, ref errMsg);
                //    if (retVal >= 1)
                //    {
                //        ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + errMsg + "');</script>");
                //        txtFlightNo.Focus();
                //        hdnFltVal.Value = string.Empty;
                //        return;
                //    }
                //}
                

                string[] QueryPname = new string[19];
                object[] QueryValue = new object[19];
                SqlDbType[] QueryType = new SqlDbType[19];

                QueryPname[0] = "AWBNo";
                QueryPname[1] = "FlightNo";
                QueryPname[2] = "AWBDate";
                QueryPname[3] = "Org";
                QueryPname[4] = "Dest";
                QueryPname[5] = "SentPcs";
                QueryPname[6] = "TotalWgt";
                QueryPname[7] = "ShortPcs";
                QueryPname[8] = "AwbStatus";
                QueryPname[9] = "InsDt";
                QueryPname[10] = "Content";
                QueryPname[11] = "Packaging";
                QueryPname[12] = "Consignor";
                QueryPname[13] = "Consignee";
                QueryPname[14] = "Remarks";
                QueryPname[15] = "CreatedOn";
                QueryPname[16] = "CreatedBy";
                QueryPname[17] = "UpdatedOn";
                QueryPname[18] = "UpdatedBy";


                QueryType[0] = SqlDbType.VarChar;
                QueryType[1] = SqlDbType.VarChar;
                QueryType[2] = SqlDbType.DateTime;
                QueryType[3] = SqlDbType.VarChar;
                QueryType[4] = SqlDbType.VarChar;
                QueryType[5] = SqlDbType.Int;
                QueryType[6] = SqlDbType.Decimal;
                QueryType[7] = SqlDbType.Int;
                QueryType[8] = SqlDbType.VarChar;
                QueryType[9] = SqlDbType.DateTime;
                QueryType[10] = SqlDbType.VarChar;
                QueryType[11] = SqlDbType.VarChar;
                QueryType[12] = SqlDbType.VarChar;
                QueryType[13] = SqlDbType.VarChar;
                QueryType[14] = SqlDbType.VarChar;
                QueryType[15] = SqlDbType.DateTime;
                QueryType[16] = SqlDbType.VarChar;
                QueryType[17] = SqlDbType.DateTime;
                QueryType[18] = SqlDbType.VarChar;

                //            @AwbNo varchar(10) ,@FlightNo varchar(10),@AwbDt Datetime,@Org varchar(10),@Dest varchar(10),
                //@SentPcs int, @TotalWgt int, @ShortPcs int,@AwbStatus varchar(10),@InsDt Datetime
	
                string StrAwbDate = txtDate.Text;
                //string StrFinalAwbDate = StrAwbDate.Substring(3, 3) + StrAwbDate.Substring(0, 3) + StrAwbDate.Substring(6, 4);
                string StrFinalAwbDate = StrAwbDate.Substring(0, 3) + StrAwbDate.Substring(3, 3) + StrAwbDate.Substring(6, 4);
              //  DateTime dt = DateTime.Parse(StrFinalAwbDate);
                DateTime dt = DateTime.ParseExact(StrFinalAwbDate, "dd/MM/yyyy", null);

                
                QueryValue.SetValue(txtAWBPrefix.Text+"-"+txtAwbNoTr.Text, 0);
                QueryValue.SetValue(txtFlightNo.Text, 1);
                QueryValue.SetValue(dt, 2);
                QueryValue.SetValue(ddlOrg.SelectedValue, 3);
                QueryValue.SetValue(ddlDest.SelectedValue, 4);
                QueryValue.SetValue(Convert.ToInt32(txtTotalPcs.Text), 5);
                QueryValue.SetValue(Convert.ToDecimal(txtWgt.Text), 6);
                QueryValue.SetValue(Convert.ToInt32(txtShortage.Text), 7);
                QueryValue.SetValue(ddlAWBStatus.SelectedValue, 8);
                QueryValue.SetValue(Convert.ToDateTime(Session["IT"].ToString()), 9);
                QueryValue.SetValue(txtContents.Text, 10);
                QueryValue.SetValue(txtPkng.Text, 11);
                QueryValue.SetValue(txtOrgAgent.Text, 12);
                QueryValue.SetValue(txtDestAgent.Text, 13);
                QueryValue.SetValue(txtRemarks.Text, 14);

                DateTime CreatedOn = DateTime.Parse(Session["IT"].ToString());
                QueryValue.SetValue(CreatedOn, 15);
                string CreatedBy = Session["UserName"].ToString();
                QueryValue.SetValue(CreatedBy, 16);
                DateTime UpdatedOn = DateTime.Parse(Session["IT"].ToString());
                QueryValue.SetValue(UpdatedOn, 17);
                string UpdatedBy = Session["UserName"].ToString();
                QueryValue.SetValue(UpdatedBy, 18);


                SQLServer ObjData = new SQLServer(Global.GetConnectionString());

                //bool bFlag = ObjData.InsertData("SpADDNewTracerDetail", QueryPname, QueryValue, QueryType);
                Int64 FoundID = 0;
                Boolean Ins = InsertData("SpADDNewTracerDetailLive", QueryPname, QueryType, QueryValue, ref FoundID);
                hdnTracerNo.Value = Convert.ToString(FoundID);
                if (Ins != false)
                {
                    #region SEND TRACER EMAIL IF RECORDS UPDATED SUCCESSFULLY

                    int RetVal = SendEmail(txtAwbNoTr.Text, "Short", "", 0);
                    if (RetVal == 1)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Tracer sent Sucessfully');</SCRIPT>");
                        
                    }
                    else
                    {

                        ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Tracer Created.Mail Sending Failed');</SCRIPT>");
                        SendEmail(txtAwbNoTr.Text, "Short", "", 0);
                    }

                    #endregion SEND TRACER EMAIL IF RECORDS UPDATED SUCCESSFULLY
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Tracer Not Created.Please Try Again');</SCRIPT>");
                }


            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('In Connection While Inserting Records" + ex.Message + "');</SCRIPT>");
            }


            try
            {
                string TDate = txtDate.Text;
                //string StrDate = TDate.Substring(3, 3) + TDate.Substring(0, 3) + TDate.Substring(6, 4);
                string StrDate = TDate.Substring(0, 3) + TDate.Substring(3, 3) + TDate.Substring(6, 4);

//                DateTime TDt = Convert.ToDateTime(StrDate);
                DateTime TDt = DateTime.ParseExact(StrDate, "dd/MM/yyyy", null);

                TDt = TDt.AddDays(7);
                StrDate = TDate.Substring(0, 3) + TDate.Substring(3, 3) + TDate.Substring(6, 4);
            }
            catch (Exception ex)
            {

            }
            
            string strAwb = txtAwbNoTr.Text.Trim();
            string strAWBPrefix = txtAWBPrefix.Text.Trim();
            string btnN = string.Empty;
            
            if (HdnPanel.Value == "Panel3")
            {
                btnN = "ctl00_ContentPlaceHolder1_btnView1";
            }
            else
            {
                btnN = "ctl00_ContentPlaceHolder1_btnView";
            }
            ClientScript.RegisterStartupScript(this.GetType(), "ClosePopup", "GetGridRowValue('" + strAwb + "','" + btnN + "','" + strAWBPrefix + "');", true);
            
            //else if (HdnPanel.Value == "Panel1")
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "ClosePopup", "CloseWin();", true);
            //}

        }

        #endregion Button GENERATE TRACER


        #region CHECK AWBNO EXISTS IN DATABASE

        /// <summary>
        /// TO CHECK FOR EXISTING FLIGHT,AWBNO AND TRACER NO FOR VALIDATION PURPOSE IF NOT EXIST THROWS ERROR
        /// </summary>
        /// <param name="FlightNo"></param>
        /// <returns></returns>

        public int CheckFiltersNoExists(string CheckType, string ValidateNo, ref string ErrorMsg)
        {
            int retVal = 0;
            DataSet oDs = new DataSet();
            ErrorMsg = string.Empty;
            try
            {

                /// BELOW FILLING DATASET WITH VALUE AS 0 OR 1 NOT EXIST OR EXIST
                string[] DParam = new string[2];
                object[] DValues = new object[2];
                SqlDbType[] DTypes = new SqlDbType[2];

                DParam.SetValue("CheckType", 0);
                DValues.SetValue(CheckType, 0);
                DTypes.SetValue(SqlDbType.VarChar, 0);

                DParam.SetValue("ValidateNo", 1);
                DValues.SetValue(ValidateNo, 1);
                DTypes.SetValue(SqlDbType.VarChar, 1);
                SQLServer ObjData = new SQLServer(Global.GetConnectionString());

                oDs = ObjData.SelectRecords("SpCheckExistsTracerFiltersLive", DParam, DValues, DTypes);

                if (oDs != null)
                {
                    if (oDs.Tables.Count > 1)
                    {
                        if (oDs.Tables[1].Rows.Count > 0)
                        {
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ConfirmAWB();</script>", false);
                            if (hdConfirm.Value == "false")
                            {
                                Response.Redirect("FrmNewTracerPopup.aspx");

                            }
                        }
                    }

                    if (oDs.Tables[0].Rows.Count >= 1)
                    {
                        /// TO ASSIGN DATASET VALUE TO INTEGER VARIABLE
                        retVal = Convert.ToInt16(oDs.Tables[0].Rows[0][0].ToString());
                        if (retVal >= 1)
                        {
                            ErrorMsg = "Provided AWBNO Already Exists. Please Check Tracer List";
                        }
                        else
                        {
                            retVal = 0;
                        }
                    }
                    else
                    {
                        retVal = 3;
                        ErrorMsg = "Error Validating" + CheckType;
                    }
                }
                else
                {
                    retVal = 3;
                    ErrorMsg = "Error Validating" + CheckType;
                }
            }
            catch (Exception)
            {

                retVal = 2;
            }
            return retVal;
        }

        #endregion CHECK AWBNO EXISTS IN DATABASE


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
            bool flag = false;

            SmtpClient Smtp = new SmtpClient("smtpout.secureserver.net", 80);
            int RetVal = 1;

            try
            {

                string sQry = string.Empty;
                string ToEmailID = string.Empty;
                int cnt = 1;

                #region RETRIEVING LIVE ID'S

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


                if (cnt == 1)
                {
                    //ToEmailID = "afrin@qidtech.com";

                    MailMessage Mail = new MailMessage();
                    Mail.From = new MailAddress(FromEmailID);

                    //ToEmailID = ToEmailID.Replace(",,",",");

                    #region Replace commas from Emil id

                    string[] ArrToEmailID = ToEmailID.Split(',');

                    if (ArrToEmailID.Length > 0)
                    {
                        ToEmailID="";
                        for (int i = 0; i < ArrToEmailID.Length; i++)
                        {
                            if (ArrToEmailID[i].Trim() != "")
                            {
                                ToEmailID += ArrToEmailID[i].Trim()+",";
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
                    string client = objbal.clientName();

                    if (mailType == "Short")
                    {
                        
                        //subject = "KFA TRACER " + hdnTracerNo.Value + ": Shortage Detected on " + txtFlightNo.Text + " Source:" + ddlOrg.SelectedValue + "  AWB No is " + txtAwbNoTr.Text + " and short pcs   " + txtShortage.Text;
                       // subject = client+ " " + "TRACER:"+ " " + hdnTracerNo.Value + ": Shortage Detected on " + txtFlightNo.Text + " Source:" + ddlOrg.SelectedValue + "  AWB No is " + txtAwbNoTr.Text + " and short pcs   " + txtShortage.Text;
                        
                        // updated on 22 Sept 2014 for [JIRA] (AC-67) 
                        subject = "TRACER:" + " " + hdnTracerNo.Value + ": Shortage Detected on " + txtFlightNo.Text + " Source:" + ddlOrg.SelectedValue + "  AWB No is " + txtAwbNoTr.Text + " and short pcs   " + txtShortage.Text;

                        Mail.Subject = subject;



                        Mail.IsBodyHtml = true;

                        //body = "Dear All,<br/><br/> ";
                        //body += "This is to inform you that there is Shortage Detected.<br/><br/> ";
                        //body += "Details are as follows.<br/><br/>";

                        //body += "<table border = 1>";
                        //body += "<tr><td>FLIGHT NO</td> <td> " + txtFlightNo.Text + "</td></tr>";
                        //body += "<tr><td>FLIGHT DATE(AWB DATE)</td> <td> " + txtDate.Text + "</td></tr>";
                        //body += "<tr><td>ORGIN-DEST</td> <td> " + ddlOrg.SelectedValue + " - " + ddlDest.SelectedValue + "  </td></tr>";
                        //body += "<tr><td>AIRWAY BILL NO</td> <td> " + dsAwbNo + "</td></tr>";
                        //body += "<tr><td>PCS/WT</td> <td> " + txtTotalPcs.Text + "pcs/" + txtWgt.Text + "kgs" + "</td></tr>";
                        //body += "<tr><td>SHORTAGE</td> <td> " + txtShortage.Text + "</td></tr>";
                        //body += "<tr><td>PACKAGING</td> <td> " + txtPkng.Text + "</td></tr>";
                        //body += "<tr><td>CONTENT</td> <td> " + txtContents.Text + "</td></tr>";
                        //body += "<tr><td>CONSIGNOR</td> <td> " + txtOrgAgent.Text + "</td></tr>";
                        //body += "<tr><td>CONSIGNEE</td> <td> " + txtDestAgent.Text + "</td></tr>";
                        //body += "<tr><td>REMARKS</td> <td> " + txtRemarks.Text + "</td></tr>";
                        ////body += "<tr><td>Updated By</td> <td> " + HdnUID.Value.ToUpper() + "</td></tr>";
                        //body += "<tr><td>Updated By</td> <td> " + Session["UserName"].ToString().ToUpper() + "</td></tr>";

                        body = "Dear All," + "\r\n";
                        body += "This is to inform you that there is Shortage Detected,Details are as follows " + "\r\n";
                        //body += "Details are as follows." + "\r\n";

                        body += "FLIGHT NO:" + txtFlightNo.Text + "\r\n";
                        body += "FLIGHT DATE:" + txtDate.Text + "\r\n";
                        body += "ORGIN-DEST: " + ddlOrg.SelectedValue + " - " + ddlDest.SelectedValue + "\r\n";
                        body += "AIRWAY BILL NO:" + dsAwbNo + "\r\n";
                        body += "PCS/WT:" + txtTotalPcs.Text + "pcs/" + txtWgt.Text + "kgs" + "\r\n";
                        body += "SHORTAGE:" + txtShortage.Text + "\r\n";
                        body += "PACKAGING: " + txtPkng.Text + "\r\n";
                        body += "CONTENT:" + txtContents.Text + "\r\n";
                        body += "CONSIGNOR:" + txtOrgAgent.Text + "\r\n";
                        body += "CONSIGNEE:" + txtDestAgent.Text + "\r\n";
                        body += "REMARKS:" + txtRemarks.Text + "\r\n";
                        body += "Updated By:" + Session["UserName"].ToString().ToUpper() + "\r\n";

                    }

                    
                    //body += "</table><br/><br/>";

                    // Temperary commented on 23rd April 2014

                    //if (mailType == "Short")
                    //{
                    //    body += "<a href=" + "http://mykfcargo.cloudapp.net/FrmTracerAddFoundAwb.aspx?TracerNo=" +
                    //hdnTracerNo.Value + "&AWBNo=" + dsAwbNo + "&MsdPcs=" + txtShortage.Text +
                    //"&FndPcs=0&FndType=NF&FndLoc=QID&chek=new;>Not Found</a>";
                    //    body += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    //    body += "<a href=" + "http://mykfcargo.cloudapp.net/FrmTracerAddFoundAwb.aspx?TracerNo=" +
                    //    hdnTracerNo.Value + "&AWBNo=" + dsAwbNo + "&MsdPcs=" + txtShortage.Text +
                    //    "&FndPcs=0&FndType=F&FndLoc=QID&chek=new;>Found</a>";

                    //}

                    //body += "Thanks,"+"\r\n";
                    //body+= "QID Cargo Team"+"\r\n";
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
                            FDoc =(List<byte[]>) Session["FileByte"];

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
                            if (MyFile.Value != string.Empty)
                            {
                                Mail.Attachments.Add(new Attachment(MyFile.PostedFile.InputStream, MyFile.PostedFile.FileName));

                            }

                        }
                    }

                    // Function addMsgToOutBox to Send Emails & store the uploaded files into Database.(sp_AddAttachmentMessage)
                    bool result=false;
                    //result = cls_BL.addMsgToOutBox(subject, body.ToString(),FromEmailID, ToEmailID, Attachments, Document, Extension);
                    result = cls_BL.addMsgToOutBox(subject, body.ToString(), "", ToEmailID, Attachments, Document, Extension, Convert.ToDateTime(Session["IT"].ToString()));


                    //SmtpClient Smtp = new SmtpClient("smtpout.secureserver.net", 80);
                    //Smtp.Credentials = new NetworkCredential(FromEmailID, EmailPwd);
                    //Mail.Priority = MailPriority.High;

                    //Smtp.Send(Mail);
                    //Mail.Attachments.Clear();
                    //Mail.Attachments.Dispose();


                }
                else
                {
                    RetVal = 0;
                }
            }

            catch (Exception ex)
            {
                MailMessage Mail = new MailMessage();
                Mail = (MailMessage)Cache["Email"];

                lblStatus.Text = "Error:" + ex.Message;
                lblStatus.ForeColor = Color.Red;

                #region Try Catch

                try
                {
                    Smtp.Send(Mail);

                    Mail.Attachments.Clear();
                    Mail.Attachments.Dispose();
                    RetVal = 1;
                    //DispError("Tracer Mail Sent Successfully.", true);
                    lblStatus.Text = "Tracer Mail Sent Successfully.";
                }
                catch (Exception)
                {

                    Mail = (MailMessage)Cache["Email"];
                    try
                    {
                        Smtp.Send(Mail);

                        Mail.Attachments.Clear();
                        Mail.Attachments.Dispose();
                        RetVal = 1;
                        //DispError("Tracer Mail Sent Successfully.", true);
                        lblStatus.Text = "Tracer Mail Sent Successfully.";
                    }
                    catch (Exception)
                    {
                        lblStatus.Text = "Tracer Mail Sending Failed.";
                        //DispError("Tracer Mail Sent Failed.", true);
                        RetVal = 0;
                    }
                }

                #endregion
            }
            return RetVal;
        }

        # endregion SendEmail

        protected void txtFlightNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string errMsg = string.Empty;

                int retVal = CheckFiltersNoExists("FLIGHT", txtFlightNo.Text, ref errMsg);
                if (retVal != 1)
                {
                    ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Flight No Does Not Exists');</script>");
                    txtFlightNo.Focus();
                    hdnFltVal.Value = string.Empty;
                }
                else
                {
                    hdnFltVal.Value = "Passed";
                    DataSet oDs = new DataSet();
                    SQLServer ObjData = new SQLServer(Global.GetConnectionString());
                    
                    string[] QueryPname = new string[1];
                    object[] QueryValue = new object[1];
                    SqlDbType[] QueryType = new SqlDbType[1];

                    QueryPname.SetValue("FLIGHTNO", 0);
                    QueryValue.SetValue(txtFlightNo.Text,0);
                    QueryType.SetValue(SqlDbType.VarChar,0);
                    oDs = ObjData.SelectRecords("SpGetOrgDestByFlightNo", QueryPname, QueryValue, QueryType);
                    if (oDs != null)
                    {
                        if (oDs.Tables[0].Rows.Count >= 1)
                        {
                            ddlOrg.SelectedValue = oDs.Tables[0].Rows[0][0].ToString();
                            ddlDest.SelectedValue = oDs.Tables[0].Rows[0][1].ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
               hdnFltVal.Value = string.Empty;
            }
            

        }


        #region ON GENERATE TRACER CURRENT ATTACHED FILES GRIDVIEW DELETE BUTTON CLICK EVENT

        /// <summary>
        /// DELETE BUTTON CLICKED FOR REMOVING ATTACHED FILE FROM CURRENTLY UPLOADED FILE DISPLAYED INSIDE GRIDVIEW INSIDE TRACER GENERATION FRAME
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

        #endregion ON GENERATE TRACER CURRENT ATTACHED FILES GRIDVIEW DELETE BUTTON CLICK EVENT


        #region ON GENERATE TRACER CURRENT ATTACHED FILES GRIDVIEW ROW DELETING EVENT

        /// <summary>
        /// AFTER ROW DELETED FROM CURRENTLY UPLOADED FILE GRIDVIEW INSIDE TRACER GENERATION FRAME THIS EVENT IS FIRED TO RELOAD GRIDVIEW AND UNBOUND DATA FROM ARRAY LIST
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

        #endregion ON GENERATE TRACER CURRENT ATTACHED FILES GRIDVIEW ROW DELETING EVENT

        #region LOAD DATASET TO FILL SOURCE AND DESTINATION CHECKBOX LIST FUNCTION

        /// <summary>
        /// RETRIEVING DATA FROM CENTRAL DB TABLE TO FILL RETURNING DATASET FOR SOURCE AND DESTINATION
        /// </summary>


        public DataSet LoadALLStationCode()
        {
            DataSet oDs = new DataSet();
            SQLServer ObjData = new SQLServer(Global.GetConnectionString());
            try
            {

                oDs = ObjData.SelectRecords("SpGetOrgDestForTracer");

            }
            catch (Exception)
            {

                oDs = null;
            }
            return oDs;
        }

        #endregion LOAD DATASET TO FILL SURCE AND DESTINATION CHECKBOX LIST FUNC

        #region BIND DATASET WITH SOURCE AND DESTINATION CHECKBOX LIST FUNC

        /// <summary>
        /// BIND SOURCE AND DESTINATION CHECKBOX LIST WITH RETURNED DATASET FROM FUNCTION
        /// </summary>

        public void LoadAirportCode()
        {
            DataSet oDs = new DataSet();

            try
            {
                oDs = LoadALLStationCode();
                if (oDs != null)
                {
                    if (oDs.Tables[0].Rows.Count >= 1)
                    {
                        /// BINDING SORCE AIRPORT
                        ddlOrg.DataSource = oDs;
                        ddlOrg.DataTextField = "AirportCode";
                        ddlOrg.DataValueField = "AirportCode";
                        ddlOrg.DataBind();
                        ddlOrg.Items.Insert(0, "SELECT");
                        ddlOrg.Items[0].Selected = true;

                        /// BINDING DESTINATION AIRPORT

                        ddlDest.DataSource = oDs;
                        ddlDest.DataTextField = "AirportCode";
                        ddlDest.DataValueField = "AirportCode";
                        ddlDest.DataBind();
                        ddlDest.Items.Insert(0, "SELECT");
                        ddlDest.Items[0].Selected = true;
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error Loading Source And Destination Codes...');</script>");
                        ddlOrg.Focus();
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error Loading Source And Destination Codes...');</script>");
                    ddlOrg.Focus();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Error Loading Source And Destination Codes..." + ex.Message + "');</script>");
                ddlOrg.Focus();
            }

        }

        #endregion BIND DATASET WITH SOURCE AND DESTINATION CHECKBOX LIST FUNC


        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetFlightList(string prefixText, int count, string contextKey)
        {
            List<string> CompletionSet = new List<string>();
            //SqlDataReader oDr = getFlightList(
            //string custItem = string.Empty;
            try
            {
                String[] DParam = { "PrefixText" };
                SqlDbType[] DTypes = { SqlDbType.VarChar };
                Object[] DValues = { prefixText };
                //Database ObjData = new Database();
                SQLServer ObjData = new SQLServer(constr);
               
                DataSet oDr = ObjData.SelectRecords("SpGetTracerFlights", DParam, DValues, DTypes);

                if(oDr !=null && oDr.Tables.Count>0 && oDr.Tables[0].Rows.Count>0)
                {
                    for(int intCount=0;intCount<oDr.Tables[0].Rows.Count;intCount++)
                    {
                        CompletionSet.Add(oDr.Tables[0].Rows[intCount][0].ToString());
                    }
                }

                //while (oDr.Read())
                //{
                //    CompletionSet.Add(oDr[0].ToString());
                //}
            }
            catch (Exception)
            {

                CompletionSet.Add("No Records Found");
            }

            return CompletionSet.ToArray();
        }

        #region INSERTING DATA

        /// <summary>
        /// Insert data in database through specified stored procedure.
        /// </summary>
        /// <param name="Procedure">Name of stored procedure to be executed.</param>
        /// <param name="ColumnNames">Parameter names.</param>
        /// <param name="DataType">Data type of parameters.</param>
        /// <param name="Values">Values of parameters.</param>
        /// <returns>True if update successful.</returns>

        public bool InsertData(string Procedure, string[] ColumnNames, SqlDbType[] DataType, object[] Values, ref Int64 FoundID)
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
            catch (Exception ex)
            {
                //e.StackTrace
                FoundID = 0;
                sqCon.Close();
                return (false);
            }
        }

        #endregion INSERTING DATA

        protected void txtAwbNoTr_TextChanged(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            int retVal = CheckFiltersNoExists("AWB", txtAWBPrefix.Text+txtAwbNoTr.Text, ref errMsg);
            if (retVal >= 1)
            {
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + errMsg + "');</script>");
                txtFlightNo.Focus();
                hdnFltVal.Value = string.Empty;
                return;
            }
            else
            {
                hdnFltVal.Value = "Passed";
            }
                
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            string FDate = txtDate.Text;

            try
            {
                //FDate = FDate.Substring(3, 3) + FDate.Substring(0, 3) + FDate.Substring(6, 4);
                FDate = FDate.Substring(0, 3) + FDate.Substring(3, 3) + FDate.Substring(6, 4);
                DateTime vDate = DateTime.ParseExact(FDate, "dd/MM/yyyy", null);

                if (vDate > Convert.ToDateTime(Session["IT"].ToString()))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Tracer Cannot Be Generated For Future Date');</SCRIPT>");
                    txtDate.Text = string.Empty;
                    txtDate.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                //lblStatus.Text = "Error:" + ex.Message;
                //lblStatus.ForeColor = Color.Red;
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


        # region Updating Document Details to DB
        public bool UpdateDocumentDetails(string AWBNo, string DocumentName, string UploadedBy, string DocumentNo, string Extension, byte[] Document, string DocumentFileName)
        {

            try
            {
                bool flag = false;
                string[] QueryNames = new string[7];
                object[] QueryValues = new object[7];
                SqlDbType[] QueryTypes = new SqlDbType[7];

                QueryNames[0] = "AWBNo";
                QueryNames[1] = "DocumentName";
                QueryNames[2] = "UploadedBy";
                QueryNames[3] = "DocumentNo";
                QueryNames[4] = "Extension";
                QueryNames[5] = "Document";
                QueryNames[6] = "FileName";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
                QueryTypes[5] = SqlDbType.VarBinary;
                QueryTypes[6] = SqlDbType.VarChar;


                QueryValues[0] = AWBNo;
                QueryValues[1] = DocumentName;
                QueryValues[2] = UploadedBy;
                QueryValues[3] = DocumentNo;
                QueryValues[4] = Extension;
                QueryValues[5] = Document;
                QueryValues[6] = DocumentFileName;

                flag = db.InsertData("SP_InsertUploadedDocuments", QueryNames, QueryTypes, QueryValues);
                return flag;

            }
            catch (Exception ex)
            { }
            return false;
        }
        #endregion


    }
}
