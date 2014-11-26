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
using System.Drawing;
using BAL;
using System.Collections;

namespace ProjectSmartCargoManager
{
    public partial class SmartCargoMaster : System.Web.UI.MasterPage
    {
        HomeBL objBLL = new HomeBL();
        SqlDataAdapter dad1 = new SqlDataAdapter();
        User UserAccess = new User();

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

            txtSearchLink_AutoCompleteExtender.DelimiterCharacters = "\n";

            #region Session Timeout
            //CS-692 System wide - Session timeout now set to 15mins (to reduce the unused connections) 
            if (Session["UserName"] == null && Request.QueryString["SSID"] == null && Session["SSOID"] == null)
            {
                Response.Redirect("~/SessionTimeOut.aspx", false);
                Session["UserName"] = "";
                Session["RoleName"] = "";
                Session["Station"] = "";
                Session["IpAddress"] = "";
                return;
            }
            else
            {
                if (CommonUtility.UseCookiesForValidation)
                {
                    bool cookieFound = false;
                    if (Context.Request.Cookies != null && Context.Request.Cookies.Count > 0)
                    {
                        for (int i = 0; i < Context.Request.Cookies.Count; i++)
                        {
                            if (Context.Request.Cookies[i].Name == "SSID" && Context.Request.Cookies[i].Value != "")
                            {
                                cookieFound = true;
                                break;
                            }
                        }
                    }
                    if (!cookieFound)
                    {
                        Session["SSOID"] = "";
                        Response.Redirect("~/Login.aspx", false);
                        Session["UserName"] = "";
                        Session["RoleName"] = "";
                        Session["Station"] = "";
                        Session["IpAddress"] = "";
                        return;
                    }
                }
            }
             
            #endregion

            if (Session["UserName"] != null && Session["UserName"].ToString() != "")
            {
                //getUserRoll();
            }
            else
            {
                //Store requested page name for SSO.
                if (Request.QueryString["SSID"] != null && Request.QueryString["SSID"] != "")
                {
                    Session["RequestedRedirectionURL"] = Request.RawUrl;
                    Session["SSOID"] = Request.QueryString["SSID"].ToString();
                }
                Response.Redirect("~/Login.aspx", false);
                Session["RequestedRedirectionURL"] = Request.RawUrl;
                Session["UserName"] = "";
                Session["RoleName"] = "";
                Session["Station"] = "";
                Session["IpAddress"] = "";
                return;
            }

            ApplyPermissions();

            try
            {

                if (Session["RoleID"] != null)
                {

                    //HideRoleID.Value = Session["RoleID"].ToString();
                    int roleid = Convert.ToInt32(Session["RoleID"].ToString());
                    txtSearchLink_AutoCompleteExtender.ContextKey = roleid.ToString();


                }
                if (!IsPostBack)
                {
                    #region Set Booking URL Flag
                    hdBookingPageFlag.Value = (CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "BookingURL") == null || CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "BookingURL") == string.Empty) ? "GHA_ConBooking.aspx" : CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "BookingURL");
                    #endregion

                    getStation();
                    if (Session["UserName"] != null && Session["UserName"].ToString() != "")
                    {
                        getUserRoll();
                    }
                    else
                    {
                        //Store requested page name for SSO.
                        if (Request.QueryString["SSID"] != null && Request.QueryString["SSID"] != "")
                        {
                            Session["RequestedRedirectionURL"] = Request.RawUrl;
                            Session["SSOID"] = Request.QueryString["SSID"].ToString();
                        }
                        Response.Redirect("~/Login.aspx", false);
                        Session["RequestedRedirectionURL"] = Request.RawUrl;

                        Session["UserName"] = "";
                        Session["RoleName"] = "";
                        Session["Station"] = "";
                        Session["IpAddress"] = "";
                        return;
                    }
                    txtprefix.Text = Convert.ToString(Session["awbPrefix"]);
                }
                //}
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Page_Load

        #region Page_Unload
        protected void Page_Unload(object sender, EventArgs e)
        {
            try
            {
                if (Session["PreviousURL"] != null && Session["PreviousURL"].ToString() != "")
                {
                    CleanUpSessions();
                }
                Session["PreviousURL"] = HttpContext.Current.Request.Url.AbsoluteUri;
            }
            catch (Exception)
            {
            }
        }
        #endregion Page_Unload

        #region Clean Up Sessions
        private void CleanUpSessions()
        {
            try
            {
                // Redirection from List Booking.
                if (Session["PreviousURL"].ToString().Contains("GHA_ListBooking.aspx")
                    && !HttpContext.Current.Request.Url.AbsoluteUri.Contains("GHA_ListBooking.aspx"))
                {   // Set related sessions to NULL.
                    Session["dsListBooking"] = null;
                    return;
                }

                //Redirection from 


            }
            catch (Exception)
            {
               
            }
        }
        #endregion Clean Up Sessions

        #region GetUserRoll
        protected void getUserRoll()
        {
            try
            {
                object[] UserParams = new string[1];
                int i = 0;
                int count = 0;
                string MenuColor = System.Configuration.ConfigurationManager.AppSettings["MenuColor"];

                UserParams.SetValue(Session["UserName"].ToString(), i);

                DataSet ds = new DataSet("MasterPage_getUserRoll_ds");
                ds = Session["SCM_Master_Hyperlinks"] != null ? (DataSet)Session["SCM_Master_Hyperlinks"] : objBLL.GetUserRollDetails(UserParams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Session["SCM_Master_Hyperlinks"] = ds;

                            DataRow dr = ds.Tables[0].Rows[0];
                            Session["RoleID"] = dr["RoleID"].ToString();

                            //Code to restrict access to pages depending on User role
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                if (ds.Tables[1].Rows[0]["AgentCode"].ToString().Trim() != "")
                                    Session["ACode"] = ds.Tables[1].Rows[0]["AgentCode"].ToString();
                                else
                                    Session["ACode"] = null;

                                //Load OPS, CONFIG & BI urls.
                                string strOpsURL = "~";
                                string strConfigURL = "~";
                                string strBIURL = "~";
                                if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
                                {
                                    strOpsURL = ds.Tables[3].Rows[0]["OPSURL"].ToString();
                                    strConfigURL = ds.Tables[3].Rows[0]["CONFIGURL"].ToString();
                                    strBIURL = ds.Tables[3].Rows[0]["BIURL"].ToString();
                                }
                                
                                foreach (DataRow drLink in ds.Tables[1].Rows)
                                {
                                    foreach (Control C in form1.Controls)
                                    {
                                        
                                        if (C.ID == drLink["HyperlinkID"].ToString())
                                        {
                                            
                                            (C as HyperLink).Visible = true;
                                            (C as HyperLink).BackColor = Color.FromName(MenuColor);
                                            if ((C as HyperLink).NavigateUrl != null && (C as HyperLink).NavigateUrl != "")
                                            {
                                                string strQueryChar = "?";
                                                //Set OPSURL
                                                if ((C as HyperLink).NavigateUrl.ToUpper().Contains("OPSURL/"))
                                                {
                                                    if ((C as HyperLink).NavigateUrl.Contains("?"))
                                                    {
                                                        strQueryChar = "&";
                                                    }
                                                    (C as HyperLink).NavigateUrl =
                                                    (C as HyperLink).NavigateUrl.Replace("OPSURL/", strOpsURL + "/")
                                                        + strQueryChar + "SSID=" + Session["SSOID"].ToString();
                                                    continue;
                                                }
                                                //Set CONFIGURL
                                                if ((C as HyperLink).NavigateUrl.ToUpper().Contains("CONFIGURL/"))
                                                {
                                                    strQueryChar = "?";
                                                    if ((C as HyperLink).NavigateUrl.Contains("?"))
                                                    {
                                                        strQueryChar = "&";
                                                    }
                                                    (C as HyperLink).NavigateUrl =
                                                        (C as HyperLink).NavigateUrl.Replace("CONFIGURL/", strConfigURL + "/")
                                                            + strQueryChar + "SSID=" + Session["SSOID"].ToString();
                                                    continue;
                                                }
                                                //Set BIURL
                                                if ((C as HyperLink).NavigateUrl.ToUpper().Contains("BIURL/"))
                                                {
                                                    strQueryChar = "?";
                                                    if ((C as HyperLink).NavigateUrl.Contains("?"))
                                                    {
                                                        strQueryChar = "&";
                                                    }
                                                    (C as HyperLink).NavigateUrl =
                                                            (C as HyperLink).NavigateUrl.Replace("BIURL/", strBIURL + "/")
                                                            + strQueryChar + "SSID=" + Session["SSOID"].ToString();
                                                    continue;
                                                }
                                            }
                                        }

                                        
                                    }

                                }

                                #region Check if the entered URL is accessible
                                foreach (Control C in form1.Controls)
                                {
                                    if (C is HyperLink)
                                    {
                                        //string MenuURL = (C as HyperLink).NavigateUrl.ToUpper();
                                        string CompareURL = HttpContext.Current.Request.Url.AbsoluteUri.ToUpper();
                                        //string[] CompareURL = HttpContext.Current.Request.Url.AbsoluteUri.Split('/');
                                        string[] MenuURL = (C as HyperLink).NavigateUrl.Split('/');
                                        //string[] CompareURLSub = CompareURL[CompareURL.Length - 1].ToUpper().Split('?');
                                        string[] MenuURLSub = MenuURL[MenuURL.Length - 1].ToUpper().Split('?');
                                        //if (((MenuURLSub[0].ToUpper().Contains(CompareURLSub[0].ToUpper()) || HttpContext.Current.Request.Url.AbsoluteUri.Contains("Home.aspx") || HttpContext.Current.Request.Url.AbsoluteUri.Contains("UnauthorizedAccess.aspx")) && (C as HyperLink).Visible == true))
                                        //{
                                        //    count++;
                                        //}
                                        if (CompareURL.Contains(MenuURLSub[0].ToUpper()) && (C as HyperLink).Visible == true && MenuURLSub[0] != string.Empty)
                                        {
                                            count++;
                                        }
                                    }
                                }

                                #endregion

                               
                            }


                        }
                        else
                        {
                            Session.Clear();
                            Response.Redirect("~/Login.aspx", false);
                        }
                    }

                }
                ds = null;
                if ((HttpContext.Current.Request.Url.AbsoluteUri.ToUpper().Contains("Login.aspx".ToUpper())) || (HttpContext.Current.Request.Url.AbsoluteUri.ToUpper().Contains("Home.aspx".ToUpper()) || HttpContext.Current.Request.Url.AbsoluteUri.ToUpper().Contains("ChangePassword.aspx".ToUpper()) || HttpContext.Current.Request.Url.AbsoluteUri.ToUpper().Contains("UnauthorizedAccess.aspx".ToUpper())))
                {
                    count++;
                }
                if (count == 0)
                {
                    Response.Redirect("UnauthorizedAccess.aspx", false);
                }

            }
            catch (Exception ex)
            {
                //Response.Redirect("http://72.167.41.153:8006/Home.aspx",false);

            }
        }
        #endregion GetUserRoll
        //Roles
        #region AddControls
        private void AddControls(ControlCollection page, ArrayList controlList, DataSet dsControls)
        {
            foreach (Control c in page)
            {
                if (c.ID != null)
                {
                    controlList.Add(c.ID);

                    for (int i = 0; i < dsControls.Tables[0].Rows.Count; i++)
                    {
                        if (c.ID == dsControls.Tables[0].Rows[i]["ControlId"].ToString())
                        {
                            if (c is DropDownList)
                                ((DropDownList)(c)).Enabled = false;
                            else if (c is Button)
                                ((Button)(c)).Enabled = false;
                            else if (c is TextBox)
                                ((TextBox)(c)).Enabled = false;
                            else if (c is CheckBox)
                                ((CheckBox)(c)).Enabled = false;
                            else if (c is LinkButton)
                                ((LinkButton)(c)).Enabled = false;
                            else if (c is GridView)
                                ((GridView)(c)).Enabled = false;
                            else if (c is ImageButton)
                                ((ImageButton)(c)).Enabled = false;
                            else if (c is Panel)
                                ((Panel)(c)).Enabled = false;
                            break;
                        }
                    }

                }

                if (c.HasControls())
                {
                    AddControls(c.Controls, controlList, dsControls);
                }
            }
        }
        #endregion AddControls
        #region ApplyPermissions
        private void ApplyPermissions()
        {
            try
            {
                ArrayList controlList = new ArrayList();
                string AgentCode = string.Empty;
                AgentCode = Convert.ToString(Session["ACode"]);

                HomeBL objHome = new HomeBL();
                int RoleId = Convert.ToInt32(Session["RoleID"]);
                DataSet objDS = new DataSet("MasterPage_ApplyPermissions_objDS");
                objDS = objHome.GetUserPermissions(((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath, RoleId);
                //DataSet objDS = Session["SCM_Master_Permissions"] != null ? (DataSet)Session["SCM_Master_Permissions"] : objHome.GetUserPermissions(((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath, RoleId);
                //Session["SCM_Master_Permissions"] = objDS;
                //Page.Title=
                if (!IsPostBack)
                {
                    string Pagename = ((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath;
                    Pagename = Pagename.Replace("~/", "");
                    string con = Global.GetConnectionString();
                    //FlightWise Productivity Report   [BI4]
                    string Query = "select distinct M.Menu+'   '+'-'+'['+M.Code+']' as Menudetails,M.Code,M.Link from tblrolemaster M inner join UserRoleMenuAccess U on u.HyperlinkID=M.HyperlinkID  where M.link like'" + Pagename + "'";
                    SqlDataAdapter dad = new SqlDataAdapter(Query, con);
                    DataSet ds = new DataSet("MasterPage_ApplyPermissions_ds");
                    dad.Fill(ds);
                    #region UserAccess
                    try
                    {
                        if (Session["UserAccess"] != null)
                        {
                            UserAccess = (User)Session["UserAccess"];
                            UserAccess.UserRoleMenuAccessDS = ds;
                            Session["UserAccess"] = UserAccess;
                        }
                        else
                        {
                            UserAccess.UserRoleMenuAccessDS = ds;
                            Session["UserAccess"] = UserAccess;
                        }
                    }
                    catch (Exception ex)
                    { }
                    #endregion
                    for (int link = 0; link < ds.Tables[0].Rows.Count; link++)
                    {
                        if (Pagename == ds.Tables[0].Rows[link][2].ToString())
                        {
                            Page.Title = ds.Tables[0].Rows[link][0].ToString();

                        }
                    }

                    ds = null;
                }


                AddControls(Page.Controls, controlList, objDS);
                #region UserAccess
                try
                {
                    if (Session["UserAccess"] != null)
                    {
                        UserAccess = (User)Session["UserAccess"];
                        UserAccess.UserPermissionsDS = objDS;
                        Session["UserAccess"] = UserAccess;
                    }
                    else
                    {
                        UserAccess.UserPermissionsDS = objDS;
                        Session["UserAccess"] = UserAccess;
                    }
                }
                catch (Exception ex)
                { }
                #endregion
                controlList = null;
                CurrentIndiaTimings(false);

                objDS = null;
            }
            catch (Exception ex)
            { }
        }
        #endregion ApplyPermissions
        //end
        #region SearchLink
        protected void txtSearchLink_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string Search;

                SqlDataAdapter dad;
                string Search1 = txtSearchLink.Text;
                Search = Search1.Substring(0, 4);
                //Search = Search.Length > 3 ? Search.Substring(Search.IndexOf('[') + 1, Search.IndexOf(']') - Search.IndexOf('[') - 1) : Search;

                string RoleId = (Session["Roleid"].ToString());
                //if (Search.Length > 3)
                //{
                //    //Search = Search.Substring(0, Search.IndexOf('[') - 1).Trim();
                //    Search = Search.Substring(Search.IndexOf('[') + 1, Search.IndexOf(']') - Search.IndexOf('[') - 1);
                //}

                string con = Global.GetConnectionString();

                dad1 = new SqlDataAdapter("select distinct u.roleid,M.Code,M.PageURL,M.PageName from tblPageMaster M inner join UserRoleMenuAccess U on u.HyperlinkID=M.PageLink  where M.PageURL!='null'and M.Code='" + Search + "'", con);

                //if (Search.Length > 4)
                //{
                //    dad1 = new SqlDataAdapter("select distinct u.roleid,M.PageName,M.Code,M.PageURL from tblPageMaster M inner join UserRoleMenuAccess U on u.HyperlinkID=M.PageLink where M.Code='" + Search +"'", con);
                //  // dad1 = new SqlDataAdapter("select distinct u.roleid,M.Menu,M.Code,M.Link from tblrolemaster M inner join UserRoleMenuAccess U on u.HyperlinkID=M.HyperlinkID where M.Menu ='" + Search + "'", con);
                //}
                //else
                //{
                //    dad1 = new SqlDataAdapter("select distinct u.roleid,M.PageName,M.Code,M.PageURL  from tblPageMaster M inner join UserRoleMenuAccess U on u.HyperlinkID=M.PageLink where M.Code='" + Search + "'", con);
                //   //dad1 = new SqlDataAdapter("select distinct u.roleid,M.Menu,M.Code,M.Link from tblrolemaster M inner join UserRoleMenuAccess U on u.HyperlinkID=M.HyperlinkID where M.Code ='" + Search + "'", con);
                //}

                DataSet ds = new DataSet("MasterPage_txtSearchLink_ds");
                dad1.Fill(ds);

                if (txtSearchLink.Text.Contains("\\n"))
                {
                    string url = ds.Tables[0].Rows[0][2].ToString();
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "', '_blank');", true);
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message12", "<SCRIPT LANGUAGE='javascript'>myFunction();</script>", false);

                }
                else
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {


                        Response.Redirect(ds.Tables[0].Rows[0][2].ToString(), false);
                        txtSearchLink.Text = "";
                        return;
                    }

                    if (Search != "")
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (Search.Length > 4)
                            {
                                if (Search == ds.Tables[0].Rows[i][1].ToString())
                                {
                                    //if (Session["Roleid"].ToString() == ds.Tables[0].Rows[i][2].ToString())
                                    //{

                                    txtSearchLink_AutoCompleteExtender.DelimiterCharacters = "\r\n";
                                    Response.Redirect(ds.Tables[0].Rows[i][2].ToString(), false);

                                    txtSearchLink.Text = "";
                                    return;
                                    //}
                                }
                            }
                            else
                            {
                                Search = Search.ToUpper();
                                if (Search == ds.Tables[0].Rows[i][2].ToString())
                                {
                                    //if (Session["Roleid"].ToString() == ds.Tables[0].Rows[i][2].ToString())
                                    //{
                                    txtSearchLink_AutoCompleteExtender.DelimiterCharacters = "\r\n";

                                    Response.Redirect(ds.Tables[0].Rows[i][3].ToString(), false);
                                    txtSearchLink.Text = "";
                                    return;
                                    //}
                                }
                            }
                        }
                    }

                }

                ds = null;
            }
            catch (Exception ex)
            {
            }
        }
        
        #endregion SearchLink
        //Searching

        #region Logout
        protected void Logout(object sender, EventArgs e)
        {
            try
            {
                LoginBL objBLL = new LoginBL();

                if (Session["IpAddress"] == null)
                    GetIPAddress();
                CurrentIndiaTimings(false);
                objBLL.SaveUserLoginDetails(Session["UserName"].ToString(), Session["Station"].ToString(), (DateTime)Session["IT"], Session["IpAddress"].ToString(), false);
                Session.Clear();

                GC.Collect();

                if (CommonUtility.UseCookiesForValidation)
                {
                    //Cookies for validation of ssid for Single Sign On.
                    if (Response.Cookies != null)
                    {
                        if (Response.Cookies["SSID"] != null)
                        {
                            Response.Cookies["SSID"].Value = "";
                        }
                    }
                }
                Response.Redirect("~/Login.aspx",false);
                Session["UserName"] = "";
                Session["RoleName"] = "";
                Session["Station"] = "";
                Session["IpAddress"] = "";
                return;
            }
            catch (Exception)
            {
            }
        }
        #endregion SearchLink

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
                }
            }
            Session["IpAddress"] = context.Request.ServerVariables["REMOTE_ADDR"];
        }

        private bool CurrentIndiaTimings(bool ShowMessage)
        {
            try
            {

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById(Convert.ToString(Session["TZ"]));
                DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

                Session["IT"] = dtIndianTime;
                return (true);
            }
            catch (Exception)
            {
                if (ShowMessage)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert ('Timezone configured in system for Station you selected is not valid. Please contact system administrator or try to set different station.')</script>", false);
                }
                else
                {
                    Session["IT"] = DateTime.Now;
                }
            }
            return (false);
        }
        #endregion

        protected void btnCahngeStation_Click(object sender, EventArgs e)
        {
            try
            {
                string strOld = Session["Station"].ToString();  //To reset station to original value if error occurs.
                Session["Station"] = ddlStation.SelectedItem.Text;
                string str = Session["Station"].ToString();
                string strPage = Page.AppRelativeVirtualPath.Substring(2);

                LoginBL lb = new LoginBL();
                Session["TZ"] = lb.GetTimeZone(str);

                if (!CurrentIndiaTimings(true))
                {
                    ddlStation.SelectedValue = strOld;
                    Session["Station"] = strOld;
                    return;
                }

                //Update Last Access Station for user.
                lb.UpdateAccessStationSession(Session["UserName"].ToString(), Session["Station"].ToString(), Session["SSOID"].ToString());

                Server.Transfer(strPage, false);

                Page_Load(sender, e);
            }
            catch (Exception)
            {
            }
        }


        private void getStation()
        {
            try
            {
                object[] UserParams = new string[1];
                int i = 0;
                UserParams.SetValue(Session["UserName"].ToString(), i);

                DataSet ds = new DataSet("MasterPage_getStation_ds");
                ds = objBLL.GetUserRollDetails(UserParams);
                if (ddlStation.DataSource == null || ddlStation.DataSource == "")
                {
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        ddlStation.DataSource = null;
                        ddlStation.DataSource = ds;
                        ddlStation.DataMember = ds.Tables[2].TableName;
                        ddlStation.DataValueField = ds.Tables[2].Columns[0].ColumnName;
                        ddlStation.DataTextField = ds.Tables[2].Columns[0].ColumnName;
                        ddlStation.DataBind();
                        ddlStation.Text = Session["Station"].ToString();

                    }
                }
                ds = null;
            }
            catch (Exception)
            {
            }
        }
        private static string _strUpdatedBy;
        public static string UpdatedBy
        {
            set
            {

                _strUpdatedBy = value;
            }
            get
            {
                return _strUpdatedBy;
            }
        }

        #region btnOpsSave_Click
        public void btnOpsSave_Click(object sender, EventArgs e)
        {

        }
        #endregion btnOpsSave_Click

        public void ShowTime()
        {
            try
            {

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ViewPanelSplitTWO();", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplitTWO();</script>", false);

                //if (!Page.ClientScript.IsStartupScriptRegistered("alert"))
                //{
                //    Page.ClientScript.RegisterStartupScript
                //        (this.GetType(), "alert", "ViewPanelSplitTWO();", true);
                //}
            }
            catch (Exception ex)
            {

            }

        }

        #region btnOpsCancel_Click
        public void btnOpsCancel_Click(object sender, EventArgs e)
        {

        }
        #endregion btnOpsCancel_Click

        public bool Contains(string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
