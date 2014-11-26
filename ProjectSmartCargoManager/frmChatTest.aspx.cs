using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class frmChatTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        #region Fill Ip Address & Update Time
        protected void GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    Session["IpAddress"] = addresses[0];
                    //return addresses[0];
                }
            }
            Session["IpAddress"] = context.Request.ServerVariables["REMOTE_ADDR"];
            //return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        internal void CurrentIndiaTimings()
        {
            //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

            Session["IT"] = dtIndianTime;
        }
        #endregion

        #region Logout
        protected void Logout(object sender, ImageClickEventArgs e)
        {
            try
            {
                LoginBL objBLL = new LoginBL();
                if (Session["IpAddress"] == null)
                    GetIPAddress();
                CurrentIndiaTimings();
                objBLL.SaveUserLoginDetails(Session["UserName"].ToString(), Session["Station"].ToString(), (DateTime)Session["IT"], Session["IpAddress"].ToString(), false);
                Session.Clear();

                try
                {
                    ////ChatBox1.endSession(); // call end chat session.
                }
                catch
                {
                }
                Session.Abandon();

                Server.Transfer("Login.aspx");
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}
