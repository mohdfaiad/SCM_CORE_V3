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
using System.Text.RegularExpressions;
using BAL;
using QID.DataAccess;
//using DG.Square.ASP.NET.Ajax.ChatControl;

namespace ProjectSmartCargoManager
{
    public partial class Login : System.Web.UI.Page
    {

        #region Variables
        HomeBL objBLL1 = new HomeBL();
        BALAWBtracking trackBl = new BALAWBtracking();
        MasterBAL objBal = new MasterBAL();
        LoginBL objBLL = new LoginBL();
        SMS  objsms = new SMS();
        EMAILOUT objEmail = new EMAILOUT(); 
        SQLServer da = new SQLServer(Global.GetConnectionString());
        //ChatBox cb = new ChatBox();
        Instance ObjInstance = new Instance();
        User UserAccess = new User();

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string ssid = "";
                    string rawURl = "";
                    if(Session["SSOID"] != null)
                        ssid = Session["SSOID"].ToString();
                    
                    if (Session["RequestedRedirectionURL"] != null)
                        rawURl = Session["RequestedRedirectionURL"].ToString();

                    Session.Clear();

                    if (ssid != "")
                        Session["SSOID"] = ssid;
                    if (rawURl != "")
                        Session["RequestedRedirectionURL"] = rawURl;

                    Session["Station"] = "";
                    Session["UserName"] = "";
                    Session["AgentCode"] = "";
                    Session["awbPrefix"] = null;
                    Session["ST"] = null;
                    Session["IpAddress"] = null;
                    Session["DelExecAWB"] = null;
                    Session["ObjInstance"] = null;
                    fillAwbPrefix();
                    ClientName();
                    ClientAddress();
                    ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>noBack();</script>");
                    ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>disableBackButton();</script>");

                    // Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    txtUserName.Focus();

                    GetUserList();
                    
                }
                if (Session["awbPrefix"] == null)
                    fillAwbPrefix();

                if (Session["ST"] == null)
                    getServiceTax();

                if (Session["AirlinePrefix"] == null)
                    fillAirlinePrefix();
                if (Session["IpAddress"] == null)
                    GetIPAddress();
                if (Session["DelExecAWB"] == null)
                    GetDelExecAWB();
                // txtTrack.Attributes.Add("onclick", "clearText()"); 
                if (Session["awbPrefix"] != null)
                    txtPrefix.ToolTip = "Enter AWB Prefix Ex: " + Session["awbPrefix"].ToString();

                if (!IsPostBack)
                {
                    //Fetch AcceptsPartnerAWB flag to enable/ disable AWB Prefix text box.
                    LoginBL objBal = new LoginBL();
                    string prefixEnable = "true";
                    prefixEnable = objBal.GetMasterConfiguration("AcceptPartnerAWB");
                    if (prefixEnable == null && prefixEnable == "")
                    {
                        prefixEnable = "true";
                    }
                    txtPrefix.Text = Session["awbPrefix"].ToString();
                    txtPrefix.Enabled = Convert.ToBoolean(prefixEnable);

                    //Redirection from other module url.
                    if (Session["SSOID"] != null && Session["SSOID"].ToString() != "")
                    {   //Validate Session ID.
                        if (GetUserBySessionID())
                        {
                            btnLogin_Click(this, new EventArgs());
                        }
                        else
                        {
                            Session["SSOID"] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
             
            }
        }
        #endregion

        #region Get User By SessionID
        private bool GetUserBySessionID()
        {
            bool res = false;
            DataSet ds = new DataSet("Login_GetUserBySessionID_ds");
            try
            {
                object[] LoginInfo = new object[2];
                LoginInfo[0] = Session["SSOID"].ToString();
                LoginInfo[1] = Session.Timeout;
                ds = objBLL.GetUserDetailsBySessionID(LoginInfo);
                if (ds != null && ds.Tables != null)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        txtUserName.Text = ds.Tables[0].Rows[0]["LoginName"].ToString();
                        txtPwd.Text = ds.Tables[0].Rows[0]["Password"].ToString();
                        txtStation.Text = ds.Tables[0].Rows[0]["LastAccessStation"].ToString();
                        res = true;
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            return (res);
        }
        #endregion Get User By SessionID

        #region fillAwbPrefix
        protected void fillAwbPrefix()
        {
            Session["awbPrefix"] = Session["awbPrefix"] == null ? objBal.awbPrefix() : Session["awbPrefix"];

            #region Instance
            try
            {
                if (Session["ObjInstance"] != null)
                {
                    ObjInstance = (Instance)Session["ObjInstance"];
                    ObjInstance.AWBPrefix = Session["awbPrefix"] != null ? Session["awbPrefix"].ToString() : objBal.awbPrefix();
                    Session["ObjInstance"] = ObjInstance;
                }
                else
                {
                    ObjInstance.AWBPrefix = Session["awbPrefix"] != null ? Session["awbPrefix"].ToString() : objBal.awbPrefix();
                    Session["ObjInstance"] = ObjInstance;
                }
            }
            catch (Exception ex)
            { }
            finally
            {
 
            }
            #endregion

        }
        #endregion
        
        #region GetDelExecAWB
        protected void GetDelExecAWB()
        {
            Session["DelExecAWB"] = objBal.CheckConfiguration("DelExecAWBBooking", "DelExecAWB");
            if (Session["DelExecAWB"] == null)
                Session["DelExecAWB"] = false;

        }
        #endregion

        #region getServiceTax
        protected void getServiceTax()
        {
            Session["ST"] = objBal.getServiceTax();
        }
        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        #region GetIPAddress
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
        #endregion

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            bool blnFlag = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SecureLogin"]);
           
            bool encryptPwd = false;
            string password = "";
            string strmsg = txtPwd.Text.Trim();
            
            if (txtUserName.Text.Trim() != "" && txtPwd.Text.Trim() != "" && txtStation.Text.Trim() !="" )
            {
                try
                {

                    encryptPwd = Session["ConfigXML"] != null ? Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "EncryptPassword")) : Convert.ToBoolean(objBLL.GetMasterConfiguration("EncryptPassword"));
                  if (encryptPwd)
                  {
                      #region Encrypt Pwd
                      password = txtPwd.Text.Trim();
                      strmsg = string.Empty;
                      byte[] encode = new byte[password.Length];
                      encode = Encoding.UTF8.GetBytes(password);
                      strmsg = Convert.ToBase64String(encode);
                      #endregion
                  }
                    object[] UserParams = new string[4];
                    int i = 0;
                    //0
                    UserParams.SetValue(txtUserName.Text.Trim(), i);
                    i++;

                    //1
                    UserParams.SetValue(strmsg, i);
                    i++;

                    //2
                    UserParams.SetValue(txtStation.Text.Trim(), i);

                    DataSet ds = new DataSet("Login_btnLogin_ds");
                    ds = objBLL.GetUserDetails(UserParams);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = ds.Tables[0].Rows[0];
                                Session["TZ"] = ds.Tables[1].Rows[0]["TimeZones"].ToString();
                                Session["RoleName"] = ds.Tables[3].Rows[0]["RoleName"].ToString();
                                Session["DateFormat"] = dr["DateFormat"].ToString().Trim();
                                
                                Session["RoleType"] = ds.Tables[3].Rows[0]["RoleType"].ToString();
                                if (ds.Tables[4].Rows.Count > 0)
                                    Session["ULDACT"] = ds.Tables[4].Rows[0]["Value"].ToString().ToUpper();
                                else
                                    Session["ULDACT"] = "true";

                                if (ds.Tables[5].Rows.Count > 0)
                                {
                                    DataSet objDS = new DataSet("Login_btnLogin_objDS");
                                    objDS.Tables.Add(ds.Tables[5].Copy());
                                    Session["ConfigXML"] = objDS.GetXml();
                                    objDS = null;
                                }

                                #region "Set Partner Type Master & Partner Master"

                                BookingBAL objBookingBal = new BookingBAL();

                                if (CommonUtility.PartnerTypeMaster == null)
                                    CommonUtility.PartnerTypeMaster = objBookingBal.GetPartnerType(true);

                                if (CommonUtility.PartnerMaster == null)
                                    CommonUtility.PartnerMaster = objBookingBal.GetAvailabePartners();

                                if (CommonUtility.AWBPrefixMaster == null)
                                    CommonUtility.AWBPrefixMaster = objBookingBal.GetAvailabeAWBPrefixs();

                                objBookingBal = null;

                                #endregion

                                try
                                {
                                    DataSet status = new DataSet("Login_btnLogin_status");
                                    status =da.SelectRecords("SP_GetAcceptPartnerAWB ");
                                    if (status.Tables[0].Rows.Count > 0)
                                    {
                                        Session["AcceptPartnerAWB"] = status.Tables[0].Rows[0][0].ToString().ToUpper();

                                    }

                                    else
                                    {
                                        Session["AcceptPartnerAWB"] = "false";
                                        
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                if (!CurrentIndiaTimings(true))
                                {
                                    txtStation.Text = "";
                                    txtPwd.Focus();
                                    return;
                                }
                                //If User is with multiple stations, dr["StationCode"] will contain Multiple station codes
                                //if User is with Single station, dr["StationCode"] will contain single station code
                                Session["Station"] = txtStation.Text.ToUpper().Trim();

                                Session["UserName"] = dr["LoginName"].ToString().Trim();
                                Session["NameofUser"] = dr["UserName"].ToString().Trim();
                                try
                                {
                                    if (dr["PwdUpdatedOn"].ToString() != "" && dr["PwdExpiresIn"].ToString() != "")
                                    {
                                        DateTime pwdUpdate = Convert.ToDateTime(dr["PwdUpdatedOn"].ToString());
                                        if (dr["PwdExpiresIn"].ToString() != "")
                                            pwdUpdate = pwdUpdate.AddDays(Convert.ToDouble(dr["PwdExpiresIn"].ToString()));
                                        Session["PwdExp"] = pwdUpdate;
                                    }
                                }
                                catch (Exception ex)
                                { }
                                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                                {
                                    Session["AgentCode"] = ds.Tables[2].Rows[0]["AgentCode"].ToString().Trim();
                                    Session["CustCode"] = ds.Tables[2].Rows[0]["CustomerCode"].ToString().Trim();
                                    Session["AgentName"] = ds.Tables[2].Rows[0]["AgentName"].ToString().Trim();
                                }
                                else
                                {
                                    Session["AgentCode"] = "";
                                    Session["CustCode"] = null;
                                    Session["AgentName"] = null;
                                }

                                Session["CheckRFIDEnable"] = ds.Tables[1].Rows[0]["isRFIDEnable"].ToString();
                                //Session["TZ"] = ds.Tables[1].Rows[0]["TimeZones"].ToString();//ds.Tables[0].Rows["TimeZone"].ToString();
                                //cb.startSession(dr["LoginName"].ToString().Trim(), dr["LoginName"].ToString().Trim());
                                //cb.Themes = "cupertino";

                                if (Session["SSOID"] == null)
                                {   //Generate Session ID.
                                    Guid objGuid = Guid.NewGuid();
                                    Session["SSOID"] = objGuid.ToString();
                                    objBLL.UpdateAccessStationSession(Session["UserName"].ToString(), Session["Station"].ToString(), Session["SSOID"].ToString());

                                    //Cookies for validation of ssid for Single Sign On.
                                    if(Session["ConfigXML"] != null)
                                    {
                                        try 
	                                    {
                                    	    if (CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "UseCookiesForValidation") != null)
                                            {
                                                CommonUtility.UseCookiesForValidation = Convert.ToBoolean(CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "UseCookiesForValidation"));
                                            }	
	                                    }
	                                    catch (Exception)
	                                    {
                                            CommonUtility.UseCookiesForValidation = false;
	                                    }
                                        
                                    }
                                    if (CommonUtility.UseCookiesForValidation)
                                    {
                                        HttpCookie cookie = new HttpCookie("SSID", Session["SSOID"].ToString());
                                        cookie.Domain = "cloudapp.net";
                                        Context.Response.SetCookie(cookie);
                                    }
                                }

                                LoadPartnerMaster();
                                getUserRoll();
                                LoadSystemParameters();

                                #region UserAccess
                                try
                                {
                                    if (Session["UserAccess"] != null)
                                    {
                                        UserAccess = (User)Session["UserAccess"];
                                        UserAccess.RoleName = ds.Tables[3].Rows[0]["RoleName"].ToString();
                                        UserAccess.RoleType = ds.Tables[3].Rows[0]["RoleType"].ToString();
                                        UserAccess.UserName = dr["LoginName"].ToString().Trim();
                                        UserAccess.LoginStation = txtStation.Text.ToUpper().Trim();
                                        Session["UserAccess"] = UserAccess;
                                    }
                                    else
                                    {
                                        UserAccess.RoleName = ds.Tables[3].Rows[0]["RoleName"].ToString();
                                        UserAccess.RoleType = ds.Tables[3].Rows[0]["RoleType"].ToString();
                                        UserAccess.UserName = dr["LoginName"].ToString().Trim();
                                        UserAccess.LoginStation = txtStation.Text.ToUpper().Trim();
                                        Session["UserAccess"] = UserAccess;
                                    }
                                    UserAccess = null;
                                }
                                catch (Exception ex)
                                { }
                                #endregion
                                if (Session["awbPrefix"] == null)
                                {
                                    MasterBAL objBal = new MasterBAL();
                                    Session["awbPrefix"] = objBal.awbPrefix();
                                    objBal = null;
                                }
                                
                                if (blnFlag)
                                {
                                    string Station = txtStation.Text;
                                    string LoginName = txtUserName.Text;
                                    string str = objBLL.GetAuthenticationCode(LoginName, Station);
                                    if (str != null)
                                    {
                                        string MobileNo = objBLL.GetMobileNo(LoginName, Station);
                                        if (MobileNo != null)
                                        {
                                            bool sms = objsms.sendSMS(MobileNo, str);
                                            DataSet res = new DataSet("Login_btnLogin_res");
                                            res = objBLL.GetEmail(LoginName, Station);
                                            if (res.Tables[0].Rows.Count > 0)
                                            {
                                                string Email = res.Tables[0].Rows[0]["UserEmail"].ToString();
                                                bool blnemail = objEmail.sendMail("swapnil.kulkarni@qidtech.com", Email, "qidtech#1", "OTP Password", str, true);
                                            }

                                        }
                                    }
                                    Response.Redirect("FrmAuthenticateUser.aspx", false);
                                    return;
                                }
                                else
                                {
                                    if (Session["RequestedRedirectionURL"] != null &&
                                        Session["RequestedRedirectionURL"].ToString() != "")
                                    {
                                        Response.Redirect(Session["RequestedRedirectionURL"].ToString(), false);
                                        Session["RequestedRedirectionURL"] = null;
                                    }
                                    else
                                    {
                                        Response.Redirect("~/Home.aspx", false);
                                    }
                                    CurrentIndiaTimings(false);
                                    objBLL.SaveUserLoginDetails(Session["UserName"].ToString(), Session["Station"].ToString(), (DateTime)Session["IT"], Session["IpAddress"].ToString(), true);
                                                             
                                   return;
                                }
                                
                            }
                            
                        }
                    }

                }
                catch (Exception ex)
                {
                    //Response.Redirect("http://72.167.41.153:8006/Home.aspx",false);
                    bool blnemail = objEmail.sendMail("criticalsupport@smartkargo.com", "criticalsupport@smartkargo.com", 
                        "qidtech#1", "Login Failed: " + HttpContext.Current.Request.Url.AbsoluteUri, 
                        "Exception for: " + txtUserName.Text + "\r\nStation: " + txtStation.Text + "\r\nException: " + ex, true);

                    //bool sendsms = objEmail.sendSMS("9552543992", "Spicejet System Down" + ex); 
                

                }
            }

            ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert ('Invalid User Name / Password / Station')</script>");

        }

        public static string Encrypt(string phrase)
        {

            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            string Password = "CSC";
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(phrase);
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
            //Creates a symmetric encryptor object. 
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream();
            //Defines a stream that links data streams to cryptographic transformations
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(PlainText, 0, PlainText.Length);
            //Writes the final state and clears the buffer
            cryptoStream.FlushFinalBlock();
            byte[] CipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string EncryptedData = Convert.ToBase64String(CipherBytes);

            return EncryptedData;
            //string decodedPhrase;
            //byte[] Buff;

            //cryptDES3.Key = cryptMD5Hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(phrase));
            //cryptDES3.Mode = CipherMode.ECB;
            //ICryptoTransform desdencrypt = cryptDES3.CreateDecryptor();
            //Buff = ASCIIEncoding.ASCII.GetBytes(phrase);
            //decodedPhrase = Convert.ToBase64String(desdencrypt.TransformFinalBlock(Buff, 0, Buff.Length));

            //return decodedPhrase;
        }

        protected void btnTrack_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = "";
                if (txtPrefix.Text == "")
                {
                    lblMsg.Text = "Kindly Enter prefix for Track AWBs";
                    return;
                }
                if (txtTrack.Text.Trim() == null || txtTrack.Text.Trim() == "")
                {
                    lblMsg.Text = "Kindly enter AWB's for tracking ";
                    return;

                }

                string str = txtTrack.Text;

                string[] Arr = txtTrack.Text.Trim().Replace(" ", "").Replace("\r", "").Replace("\n", "").Split(',');
                

                for (int i = 0; i < Arr.Length; i++)
                {
                    if (Arr[i].Length < 8 || Arr[i].Length > 8)
                    {
                        lblMsg.Text = Arr[i] + " Is Not Real AWB Number";
                        return;
                    }
                }

                string awbno = txtTrack.Text;


                //if (txtTrack.Text != "")
                //{
                //    FinalAwbList = TextBoxAWBno.Text.Trim();
                //}
                DataSet ds = new DataSet("Login_btnTrack_ds");
                ds = trackBl.GetAWBTrackingData(txtPrefix.Text.Trim(), txtTrack.Text.Trim().Replace(" ", "").Replace("\r", "").Replace("\n", ""));

                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                           LoginBL objBal = new LoginBL();
                            string c2kenable = "false";
                            c2kenable = objBal.GetMasterConfiguration("C2KTrackAWBFromLogin");
                        if (c2kenable != null && c2kenable != "" && c2kenable =="true")
                        {
                        //code to redirect to c2k page
                            Session["TrackAWB"] = txtPrefix.Text + "|" + txtTrack.Text;
                            ClientScript.RegisterStartupScript(Page.GetType(), "openPopUp1", "javascript:openPopUp1();", true);
                            lblMsg.Text = "AWB track details are shown in popup window.Please make sure popup is not blocked";
                            lblMsg.ForeColor = System.Drawing.Color.Blue;
                        }
                        else
                        {
                            //code to redirect to track awb page
                            Session["TrackAWB"] = txtPrefix.Text + "|" + txtTrack.Text;

                            ClientScript.RegisterStartupScript(Page.GetType(), "openPopUp", "javascript:openPopUp();", true);
                            lblMsg.Text = "AWB track details are shown in popup window.Please make sure popup is not blocked";
                            lblMsg.ForeColor = System.Drawing.Color.Blue;
                        }
                    }
                        else
                        {
                            lblMsg.Text = "AWB details not available";
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                }
                else
                {
                    lblMsg.Text = "AWB Details not available";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                ds = null;
            }
            catch (Exception ex)
            {

            }
        }

        protected void txtTrack_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnTrack_DataBinding(object sender, EventArgs e)
        {

        }

        protected void btnTrack_Click1(object sender, EventArgs e)
        {

        }

        protected void txtTrack_TextChanged1(object sender, EventArgs e)
        {

        }

        protected void txtStation_TextChanged(object sender, EventArgs e)
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());
            string str = txtStation.Text.Trim();

            string[] paramname = new string[1];
            paramname[0] = "Station";

            object[] paramvalue = new object[1];
            paramvalue[0] = txtStation.Text.Trim();

            SqlDbType[] paramtype = new SqlDbType[1];
            paramtype[0] = SqlDbType.VarChar;

            bool ds = da.InsertData("SP_ValidateStation", paramname, paramtype, paramvalue);


            if (ds == true)
            {

            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert ('Station Code Does not Exists')</script>");
                return;
            }
        }

        #region CurrentIndiaTimings
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
                    ClientScript.RegisterStartupScript(GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert ('Timezone configured in system for Login Station you entered is not valid. Please contact system administrator or login with different station code.')</script>");
                else
                    Session["IT"] = DateTime.Now;
            }
            return (false);
        }
        #endregion

        #region fillAirlinePrefix
        protected void fillAirlinePrefix()
        {
            Session["AirlinePrefix"] = objBal.AirlinePrefix();
            #region Instance
            try
            {
                if (Session["ObjInstance"] != null)
                {
                    ObjInstance = (Instance)Session["ObjInstance"];
                    ObjInstance.AirlinePreifx = objBal.AirlinePrefix();
                    Session["ObjInstance"] = ObjInstance;
                }
                else
                {
                    ObjInstance.AirlinePreifx = objBal.AirlinePrefix();
                    Session["ObjInstance"] = ObjInstance;
                }
            }
            catch (Exception ex)
            { }
            #endregion
        }
        #endregion

        #region LoadPartnerMaster
        protected void LoadPartnerMaster()
        {
            SQLServer db = new SQLServer(Global.GetConnectionString());

            DataSet ds = new DataSet("Login_LoadPartnerMaster_ds");
            ds = da.GetDataset("Select Prefix3Letter from AWBPrefixMaster Union Select Prefix from AWBPrefixMaster Union Select PartnerPrefix from tblPartnerMaster; Select Status, StatusCode from AWBStatusMaster order by [Order];");

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count>0)
            {
                Session["PartnerCode"] = ds;
            }
            else
            {
                Session["PartnerCode"] = null;
            }

            if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                Session["StatusMaster"] = ds.Tables[1];
            }
            else
            {
                Session["StatusMaster"] = null;
            }

            ds = null;
            db = null;
            
            
        }
        #endregion

        #region GetUserRoll
        protected void getUserRoll()
        {
            try
            {
                object[] UserParams = new string[1];
                int i = 0;
                string MenuColor = System.Configuration.ConfigurationManager.AppSettings["MenuColor"];

                UserParams.SetValue(Session["UserName"].ToString(), i);

                DataSet ds = new DataSet("Login_getUserRoll_ds");
                ds = objBLL1.GetUserRollDetails(UserParams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];
                            Session["RoleID"] = dr["RoleID"].ToString();

                            //Code to restrict access to pages depending on User role
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                if (ds.Tables[1].Rows[0]["AgentCode"].ToString().Trim() != "")
                                    Session["ACode"] = ds.Tables[1].Rows[0]["AgentCode"].ToString();
                                else
                                    Session["ACode"] = null;

                                //foreach (DataRow drLink in ds.Tables[1].Rows)
                                //{
                                //    foreach (Control C in form1.Controls)
                                //    {
                                //        if (C.ID == drLink["HyperlinkID"].ToString())
                                //        {
                                //            (C as HyperLink).Visible = true;
                                //            (C as HyperLink).BackColor = Color.FromName(MenuColor);
                                //        }
                                //    }

                                //}
                            }
                            #region UserAccess
                            try
                            {
                                if (Session["UserAccess"] != null)
                                {
                                    UserAccess = (User)Session["UserAccess"];
                                    UserAccess.RoleID = dr["RoleID"].ToString();
                                    Session["UserAccess"] = UserAccess;
                                }
                                else
                                {
                                    UserAccess.RoleID = dr["RoleID"].ToString();
                                    Session["UserAccess"] = UserAccess;
                                }
                            }
                            catch (Exception ex)
                            { }
                            #endregion

                        }
                    }
                }
                ds = null;
                

            }
            catch (Exception ex)
            {
                //Response.Redirect("http://72.167.41.153:8006/Home.aspx",false);

            }
        }
        #endregion GetUserRoll  
      
        #region ClientName
        protected void ClientName()
        {
            Session["awbPrefix"] = objBal.awbPrefix();

            #region Instance
            try
            {
                if (Session["ObjInstance"] != null)
                {
                    ObjInstance = (Instance)Session["ObjInstance"];
                    ObjInstance.ClientName = objBal.clientName();
                    Session["ObjInstance"] = ObjInstance;
                }
                else
                {
                    ObjInstance.ClientName = objBal.clientName();
                    Session["ObjInstance"] = ObjInstance;
                }
            }
            catch (Exception ex)
            { }
            #endregion

        }
        #endregion

        #region ClientAddress
        protected void ClientAddress()
        {
            //Session["awbPrefix"] = objBal.clientAddress();

            #region Instance
            try
            {
                if (Session["ObjInstance"] != null)
                {
                    ObjInstance = (Instance)Session["ObjInstance"];
                    ObjInstance.ClientAddress = objBal.clientAddress();
                    Session["ObjInstance"] = ObjInstance;
                }
                else
                {
                    ObjInstance.ClientAddress = objBal.clientAddress();
                    Session["ObjInstance"] = ObjInstance;
                }
            }
            catch (Exception ex)
            { }
            #endregion

        }
        #endregion

        protected void lnkFrgtPwd_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("Login_lnkFrgtPwd_ds");
            string email = "";
            string Subject = "Request for Password Change";
            string Msg = "Please click on the following link to change the password.\n";
            Msg += Request.Url.GetLeftPart(UriPartial.Authority) + "/SetPassword.aspx?ID=" + txtUserName.Text.Trim();
            bool res=false;
            try
            {
                ds = (DataSet)ViewState["dsUserList"];

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["LoginID"].ToString().ToUpper() == txtUserName.Text.Trim().ToUpper())
                        email = ds.Tables[0].Rows[i]["EmailID"].ToString();
                }

               res= cls_BL.addMsgToOutBox(Subject, Msg, "", email,true);
               if (res)
               {
                   ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Link to change password has been sent to registered Email ID');</SCRIPT>", false);
               }
            }
            catch (Exception ex)
            { }
            finally { ds = null;  }
        }

        protected void GetUserList()
        {
            DataSet ds = new DataSet("Login_GetUserList_ds");
            UserCreationBAL UserBal = new UserCreationBAL();
            try
            {
                ds = UserBal.GetUserListData("", 0, "");
                string UserList = "";
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["dsUserList"] = ds;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (UserList == "")
                            UserList = ds.Tables[0].Rows[i]["LoginName"].ToString();
                        else
                            UserList = UserList + "," + ds.Tables[0].Rows[i]["LoginName"].ToString();
                    }
                }
                hdnUserList.Value = UserList;
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void LoadSystemParameters()
        {
            string FlightValidation = string.Empty;
            if (CommonUtility.SmartKargoInstance == "" || CommonUtility.SmartKargoInstance == null)
            {
                // To find out the system is for Airline or GHA
                CommonUtility.SmartKargoInstance = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "SmartKargoInstance");
                if (CommonUtility.SmartKargoInstance == "")
                    CommonUtility.SmartKargoInstance = "AR";

                // To find out the Flight number is mandatory for operations
                FlightValidation = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "FlightValidation");
                if (FlightValidation != "")
                    CommonUtility.FlightValidation = Convert.ToBoolean(FlightValidation); // FlightRouteValidation
                else
                    CommonUtility.FlightValidation = true;
                FlightValidation = string.Empty;

                // To find out the shipper and consignee is mandatory at what stage (BK - Booking, EX - Execution, AC - Acceptance)
                CommonUtility.ShipperMandatoryDuring = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ValidateShipperDuring");
                CommonUtility.ConsigneeMandatoryDuring = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ValidateConsigneeDuring");

                // To Find out the CCSF & IAC functionality is enabled.
                string strClientFlag = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "ValidateCCSF&IAC");
                if (strClientFlag == "")
                    CommonUtility.ValidateCCSFandIAC = "0";
                else
                {
                    if (Convert.ToBoolean(strClientFlag))
                        CommonUtility.ValidateCCSFandIAC = "1";
                    else
                        CommonUtility.ValidateCCSFandIAC = "0";
                }
                strClientFlag = string.Empty;

                // To find whether to show operations popup or not
                string config = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "OperationTimePopup");
                if (config != null)
                {
                    bool result = false;
                    if (Boolean.TryParse(config, out result))
                        CommonUtility.ShowOperationTimeOnSave = result;
                    else
                        CommonUtility.ShowOperationTimeOnSave = false;
                }
                config = string.Empty;

                // To find whether to show Combination of Save and Execute
                config = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "CombineSave&Execute");
                if (config != null)
                {
                    bool result = false;
                    if (Boolean.TryParse(config, out result))
                        CommonUtility.CombineSaveandExecute = result;
                    else
                        CommonUtility.CombineSaveandExecute = false;
                }
                config = string.Empty;

                CommonUtility.HideRatesAcceptance = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "HideRatesAcceptance");
                CommonUtility.EmbargoFunctionality = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "EmbargoFunctionality");
            }
        }
    }
}

