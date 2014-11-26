using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace SMSGatewayApp
{
    public partial class _Default : System.Web.UI.Page
    {

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            string Message = "";
            string MobileNumber = "";
            DateTime ReceivedOn = System.DateTime.Now;

            if (!IsPostBack)
            {
                try
                {
                    Message = Request.QueryString["message"];
                    MobileNumber = Request.QueryString["mobilenumber"].ToString();
                    ReceivedOn = DateTime.Parse(Request.QueryString["receivedon"].ToString());
                }
                catch (Exception ex) { }

                try
                {
                    lblMsg.Text = lblMsg.Text + " " + Message;
                    lblMobNum.Text = lblMobNum.Text + " " + MobileNumber;
                    lblRcvon.Text = lblRcvon.Text + " " + ReceivedOn.ToString();
                }
                catch (Exception ex) { }

                if (Request.QueryString["mobilenumber"] != null)
                {

                    try
                    {
                        if (MobileNumber.Trim() == "")
                        {
                            lblStatus.Text = "Invalid Mobile Number";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        Message = Message.Trim().ToUpper();
                        Message = Message.Trim().Replace("JETCARGO", "");
                        Message = Message.Trim().Replace("jetcargo", "");
                        
                        if (Message.Trim().Substring(0,3)=="FFR")
                        {
                            
                            {
                                lblStatus.Text = "";
                            
                                try
                                {
                                    if (ReceivedOn == null)
                                    {
                                        ReceivedOn = System.DateTime.Now;
                                    }
                                    string Procedure = "spInsertSMSinInbox";
                                    QID.DataAccess.SQLServer objSQL = new QID.DataAccess.SQLServer(BAL.Global.GetConnectionString());
                                    bool flag = false;
                                    string[] paramname = new string[] { "MobileNo", "Subject", "Message", "ReceivedOn" };
                                    object[] paramvalue = new object[] { MobileNumber.Trim(), "FFR", Message.Trim(), ReceivedOn };
                                    SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.DateTime };
                                    flag = objSQL.InsertData(Procedure, paramname, paramtype, paramvalue);
                                    if (flag == true)
                                    {
                                        lblStatus.Text = "Message Processed Successfully";
                                        lblStatus.ForeColor = System.Drawing.Color.Green;
                                    }
                                    else
                                    {
                                        lblStatus.Text = "Error while Processing Message";
                                        lblStatus.ForeColor = System.Drawing.Color.Red;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    lblStatus.Text = ex.Message;
                                }

                            }
                        }
                        else
                        {
                            lblStatus.Text = "Invalid Message";
                            lblStatus.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = ex.Message;
                    }
                }
            }
        }
        #endregion

        #region btnTest_Click
        protected void btnTest_Click(object sender, EventArgs e)
        {
            string Number = "+919960616110";
            string Message = "FFR Test Message";
            Response.Redirect("~/SMS.aspx?mobilenumber=" + Number + "&message= " + Message.Trim());// + "$receivedon="+ DateTime.Now.ToString());
        }
        #endregion

    }
}
