using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using BAL;



namespace ProjectSmartCargoManager
{
    public partial class SpotRateSavetmp : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            string strquerystring = Request.QueryString["Approval"].ToString();
            string strspotrateid = Request.QueryString["spotrateid"].ToString();
            MasterBAL objBal = new MasterBAL();
            string AWBPrefix = objBal.awbPrefix();
            strspotrateid = cls_Encode_Decode.StringCipher.Decrypt(strspotrateid.Replace(" ", "+"), AWBPrefix);
            if (strspotrateid != "Err")
            {
                try
                {

                    //string strquerystring = Request.QueryString["Approval"].ToString();
                    //string strspotrateid = Request.QueryString["spotrateid"].ToString();

                    if (strquerystring != null && strspotrateid != null)
                    {
                        string[] PName = new string[] { "SpotID", "Status" };
                        object[] PValue = new object[] { strspotrateid, strquerystring };
                        SqlDbType[] PType = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                        //bool dsapprove = da.GetBoolean("update tblSpotRateMaster set Aproval= '" + strquerystring + "' where SpotRateID='" + strspotrateid + "' ");
                        if (da.UpdateData("spSpotRateStatusUpdate", PName, PType, PValue))
                        {
                            lblsprotsaverate.ForeColor = System.Drawing.Color.Green;
                            lblsprotsaverate.Text = "Thank You. The Spot Rate is Successfully " + strquerystring;
                        }
                       /* if (strquerystring == "Approved")
                        {
                            bool dsapprove = da.GetBoolean("update tblSpotRateMaster set Aproval= '" + strquerystring + "' where SpotRateID='" + strspotrateid + "' ");
                            lblsprotsaverate.ForeColor = System.Drawing.Color.Green;
                            lblsprotsaverate.Text = "Thank You. The Spot Rate is Successfully Approved.";
                        }
                        else
                        {
                            bool dsapprove = da.GetBoolean("update tblSpotRateMaster set Aproval= '" + strquerystring + "' where SpotRateID='" + strspotrateid + "' ");
                            lblsprotsaverate.ForeColor = System.Drawing.Color.Green;
                            lblsprotsaverate.Text = "Thank You. The Spot Rate is Successfully Rejected.";
                        }*/
                    }
                }
                catch (Exception ex)
                {
                    if (strquerystring == "Approved")
                    {
                        lblsprotsaverate.ForeColor = System.Drawing.Color.Red;
                        lblFailed.Text = "Sorry, the Spot Rate Approval failed...";
                        lblFailed.ForeColor = System.Drawing.Color.Red;
                    }
                    if (strquerystring == "Rejected")
                    {
                        lblsprotsaverate.ForeColor = System.Drawing.Color.Red;
                        lblFailed.Text = "Sorry, the Spot Rate Rejection failed...";
                        lblFailed.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        lblsprotsaverate.ForeColor = System.Drawing.Color.Red;
                        lblFailed.Text = "Sorry, the Spot Rate Operation failed...";
                        lblFailed.ForeColor = System.Drawing.Color.Red;
                    }
                }
                objBal = null;
            }
            else
            {
                lblsprotsaverate.ForeColor = System.Drawing.Color.Red;
                lblFailed.Text = "Invalid URL! Please copy & paste the complete URL...";
                lblFailed.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
