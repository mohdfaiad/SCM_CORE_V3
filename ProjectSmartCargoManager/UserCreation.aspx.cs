using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using QID.DataAccess;
using System.Linq;
using BAL;
using System.Text;
using System.Text.RegularExpressions;


namespace ProjectSmartCargoManager
{
    public partial class UserCreation : System.Web.UI.Page
    {
        UserCreationBAL objUserBAL = new UserCreationBAL();
        LoginBL lbal = new LoginBL();
        //EMAILOUT objEmail = new EMAILOUT();

        string strCheckedStations;
        string strCheckedRoles;

        protected void Page_Load(object sender, EventArgs e)
        {
            btnBack.Attributes.Add("onClick", "javascript:history.back(); return false;");
            if (!IsPostBack)
            {
                getAllRoles();
                getAllStations("Station");
                chkIsActive.Checked = true;

                if (Request.QueryString["cmd"] == "Edit" || Request.QueryString["cmd"] == "View")
                {
                    btnList.Visible = false;
                    getUserDetails();
                }
                if (Request.QueryString["cmd"] == "Edit")
                {
                    btnUpdate.Visible = true;
                    btnSave.Visible = false;
                    //btnBack.Visible = true;
                }
                else if (Request.QueryString["cmd"] == "View")
                {
                    btnUpdate.Visible = false;
                    btnSave.Visible = false;
                    btnBack.Visible = true;

                    disableForView();
                }
                else
                {
                    btnSave.Visible = true;
                    btnBack.Visible = false;
                    btnUpdate.Visible = false;

                }

            }
        }

        #region disable controls for view
        protected void disableForView()
        {
            txtLoginID.Enabled = false;
            txtPassword.Enabled = false;
            txtUserName.Enabled = false;
            txtEmail.Enabled = false;
            //ddlRole.Enabled = false;
            chkListRole.Enabled = false;
            txtAgentCode.Enabled = false;
            chkListStation.Enabled = false;
            chkSelectAll.Enabled = false;
            chkIsActive.Enabled = false;
        }
        #endregion disable controls for view

        //old
        /*protected string getCheckedStations()
        {
            if (chkSelectAll.Checked == true)
            {
                for (int i = 0; i < chkListStation.Items.Count; i++)
                {
                    if (strCheckedStations == "")
                        strCheckedStations = chkListStation.Items[i].Value;
                    else
                        strCheckedStations = strCheckedStations + "," + chkListStation.Items[i].Value;
                }
            }
            else
            {
                for (int i = 0; i < chkListStation.Items.Count; i++)
                {
                    if (chkListStation.Items[i].Selected == true)
                    {
                        if (strCheckedStations == "")
                            strCheckedStations = chkListStation.Items[i].Value;
                        else
                            strCheckedStations = strCheckedStations + "," + chkListStation.Items[i].Value;
                    }
                }
            }


            return strCheckedStations;
        }*/

        protected string getCheckedStations()
        {
            #region Station
            if (drLevel.SelectedItem.Text == "Station")
            {
                if (chkSelectAll.Checked == true)
                {
                    for (int i = 0; i < chkListStation.Items.Count; i++)
                    {
                        if (strCheckedStations == "")
                            strCheckedStations = chkListStation.Items[i].Value;
                        else
                            strCheckedStations = strCheckedStations + "," + chkListStation.Items[i].Value;
                    }
                }
                else
                {
                    for (int i = 0; i < chkListStation.Items.Count; i++)
                    {
                        if (chkListStation.Items[i].Selected == true)
                        {
                            if (strCheckedStations == "")
                                strCheckedStations = chkListStation.Items[i].Value;
                            else
                                strCheckedStations = strCheckedStations + "," + chkListStation.Items[i].Value;
                        }
                    }
                }
            }
            #endregion Station

            #region Country
            if (drLevel.SelectedItem.Text == "Country")
            {
                if (chkSelectAll.Checked == true)
                {
                    for (int i = 0; i < chkListStation.Items.Count; i++)
                    {
                        string cntrycode = chkListStation.Items[i].Value.ToString();
                        DataSet dscountryStn = objUserBAL.GetAllStationsForCountry(cntrycode);
                        if (dscountryStn != null)
                        {
                            if (dscountryStn.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dscountryStn.Tables[0].Rows.Count; j++)
                                {
                                    if (strCheckedStations == "")
                                        strCheckedStations = dscountryStn.Tables[0].Rows[j][0].ToString();
                                    else
                                        strCheckedStations = strCheckedStations + "," + dscountryStn.Tables[0].Rows[j][0].ToString();
                                }
                            }
                            dscountryStn.Dispose();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < chkListStation.Items.Count; i++)
                    {
                        if (chkListStation.Items[i].Selected == true)
                        {
                            string cntrycode = chkListStation.Items[i].Value.ToString();
                            DataSet dscountryStn = objUserBAL.GetAllStationsForCountry(cntrycode);
                            if (dscountryStn != null)
                            {
                                for (int j = 0; j < dscountryStn.Tables[0].Rows.Count; j++)
                                {
                                    if (strCheckedStations == "")
                                        strCheckedStations = dscountryStn.Tables[0].Rows[j][0].ToString();
                                    else
                                        strCheckedStations = strCheckedStations + "," + dscountryStn.Tables[0].Rows[j][0].ToString();
                                }
                                dscountryStn.Dispose();
                            }
                        }
                    }
                }
            }
            #endregion Country
            return strCheckedStations;
        }

        protected void getAllRoles()
        {
            DataSet ds = null;
            try
            {
                ds = objUserBAL.GetAllRoles();
                if (ds != null)
                {
                    /*
                    ddlRole.DataSource = ds;
                    ddlRole.DataMember = ds.Tables[0].TableName;
                    ddlRole.DataTextField = "RoleName";
                    ddlRole.DataValueField = "RoleID";
                    ddlRole.DataBind();
                    ddlRole.SelectedIndex = -1;
                    */
                    chkListRole.DataSource = ds;
                    chkListRole.DataTextField = "RoleName";
                    chkListRole.DataValueField = "RoleID";
                    chkListRole.DataBind();
                    ds.Dispose();
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
        }

        //Old
        /*protected void getAllStations()
        {
            DataSet ds = objUserBAL.GetAllStations();
            if (ds != null)
            {
                chkListStation.DataSource = ds;
                chkListStation.DataMember = ds.Tables[0].TableName;
                chkListStation.DataTextField = "AirportName";
                chkListStation.DataValueField = "AirportCode";
                chkListStation.DataBind();
                chkListStation.SelectedIndex = -1;
            }
        }*/

        protected void getAllStations(string level)
        {
            DataSet ds = null;
            try
            {
                ds = objUserBAL.GetAllStations(level);
                if (ds != null)
                {
                    string col0 = ds.Tables[0].Columns[0].ToString();
                    string col1 = ds.Tables[0].Columns[1].ToString();
                    chkListStation.DataSource = ds.Tables[0];
                    //chkListStation.DataMember = ds.Tables[0].TableName;
                    chkListStation.DataTextField = col1;
                    chkListStation.DataValueField = col0;
                    chkListStation.DataBind();
                    chkListStation.SelectedIndex = -1;
                    ds.Dispose();
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
        }

        //Function to get all Agent code on autopopulate while creating user.
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static string[] GetAllAgentCode(string prefixText, int count)
        {
            List<string> list = null;
            SqlDataAdapter dad = null;
            DataSet ds = null;
            try
            {
                string con = Global.GetConnectionString();
                // SqlConnection con = new SqlConnection("connection string");
                //SqlDataAdapter dad = new SqlDataAdapter("SELECT AgentCode from dbo.AgentMaster where (AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%') and Station='" + orgdest[0] + "'", con);
                dad = new SqlDataAdapter("SELECT AgentCode from dbo.AgentMaster where AgentName like '" + prefixText + "%' or AgentCode like '" + prefixText + "%'", con);
                ds = new DataSet();
                dad.Fill(ds);
                list = new List<string>(ds.Tables[0].Rows.Count);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(dr[0].ToString());
                }
                return list.ToArray();
            }
            catch (Exception)
            {
                list = null;
            }
            finally
            {
                if (dad != null)
                {
                    dad.Dispose();
                }
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            return (null);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            AddUpdateUser("I");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            AddUpdateUser("U");
        }

        protected void AddUpdateUser(string Flag)
        {
            object[] UserInfo;
            bool IsEncrypt = false;
            try
            {
                lblStatus.Visible = false;
                strCheckedStations = "";
                strCheckedStations = getCheckedStations();
                strCheckedRoles = string.Empty;
                strCheckedRoles = ConcatenateCheckBoxListItems(chkListRole);
                

                #region Check Password Strength

                string regexStrong = "(?=.*?[A-Za-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*]).{6,25}$";
                string regexMedium = "(?=.*?[A-Za-z])(?=.*?[0-9]).{6,25}$";
                string regexLow = @"^[\S]{6,}$";

                bool pwdchk = false;
                string PwdStrength = lbal.GetMasterConfiguration("PasswordStrength");

                if (PwdStrength == "Strong")
                {
                    pwdchk = Regex.IsMatch(txtPassword.Text.Trim(), regexStrong);
                    if (pwdchk == false)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Password must be minimum 6 letters and must contain 1 alphabet,1 Number and One of Special Characters #?!@$%^&*')</script>", false);
                        return;
                    }
                }
                if (PwdStrength == "Medium")
                {
                    pwdchk = Regex.IsMatch(txtPassword.Text.Trim(), regexMedium);
                    if (pwdchk == false)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Password must be minimum 6 letters and alphanumeric')</script>", false);
                        return;
                    }
                }
                if (PwdStrength == "Low")
                {
                    pwdchk = Regex.IsMatch(txtPassword.Text.Trim(), regexLow);
                    if (pwdchk == false)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Password must be minimum 6 letters')</script>", false);
                        return;
                    }
                }

                #endregion

                if (txtLoginID.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Enter Login ID";
                    return;
                }
                if (txtUserName.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Enter User Name";
                    return;
                }
                if (txtPassword.Text == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please Enter Password";
                    return;
                }
                if (strCheckedStations == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select atleast one Station.";
                    return;
                }
                //if (ddlRole.SelectedValue == "1" && txtAgentCode.Text.Trim() == "")
                //{
                //    lblStatus.Visible = true;
                //    lblStatus.ForeColor = Color.Red;
                //    lblStatus.Text = "Please enter Agent Code fot this user.";
                //    return;
                //}
                if (strCheckedRoles == string.Empty && txtAgentCode.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter Agent Code fot this user.";
                    return;
                }

                #region Prepare Parameters

                string Pwd = txtPassword.Text.Trim();

                IsEncrypt = Convert.ToBoolean(lbal.GetMasterConfiguration("EncryptPassword"));
                if (IsEncrypt)
                {
                    Pwd = EncryptPwd(txtPassword.Text.Trim());
                }

                UserInfo = new object[15];
                int irow = 0;

                //0
                UserInfo.SetValue(txtUserName.Text.Trim(), irow);
                irow++;

                //1
                UserInfo.SetValue(txtLoginID.Text.Trim(), irow);
                irow++;

                //2
                UserInfo.SetValue(Pwd, irow);
                irow++;

                //3
                UserInfo.SetValue(txtEmail.Text.Trim(), irow);
                irow++;

                //4
                //UserInfo.SetValue(ddlRole.SelectedValue, irow);
                UserInfo.SetValue(strCheckedRoles, irow);
                irow++;

                //5
                UserInfo.SetValue(txtAgentCode.Text.Trim(), irow);
                irow++;

                //6
                UserInfo.SetValue(strCheckedStations, irow);
                irow++;

                //7
                UserInfo.SetValue(Session["UserName"].ToString(), irow);
                irow++;

                //8
                UserInfo.SetValue(chkIsActive.Checked, irow);
                irow++;

                //9
                UserInfo.SetValue(Flag, irow);
                irow++;

                //10
                UserInfo.SetValue("", irow);
                irow++;

                //11
                UserInfo.SetValue(txtMobileNo.Text, irow);
                irow++;

                //12
                if (txtPwdExpiry.Text != "")
                    UserInfo.SetValue(Convert.ToInt32(txtPwdExpiry.Text.Trim()), irow);
                else
                    UserInfo.SetValue("0", irow);
                irow++;

                //13
                DateTime dt = Convert.ToDateTime(Session["IT"].ToString());
                UserInfo.SetValue(dt, irow);
                irow++;

                //14
                UserInfo.SetValue(Session["UserName"].ToString(), irow);
                irow++;
                #endregion Prepare Parameters

                string res = "";
                res = objUserBAL.AddModifyUserDetails(UserInfo);

                if (res != "error")
                {
                    getUserDetails();

                    //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('" + res + "');</SCRIPT>");
                    lblStatus.Visible = true;
                    lblStatus.Text = res;
                    lblStatus.ForeColor = Color.Green;
                    //Sending mail after creating username and password --Swapnil
                    string Email = txtEmail.Text;
                    string UserName = txtUserName.Text;
                    string Password = txtPassword.Text;
                    string message = "User";
                    //string FromMail = "spicejetcargo@qidtech.com";
                    string body = "Hello " + txtUserName.Text + "@Your LoginID is: " + txtLoginID.Text + "@ and password is: " + txtPassword.Text + "@ Thank you" + "@@ From QID Technologies";
                    body = body.Replace("@", "" + Environment.NewLine);
                    //string Pass = "qidtech#1";
                    //send Email after User Creation
                    //bool checkemail = objEmail.sendMail(FromMail, Email, Pass, message, body, false);
                    //swapnil...........................code of sending mail.

                    cls_BL.addMsgToOutBox(message, body, "", Email);

                    #region for Master Audit Log
                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    object[] Params = new object[7];
                    int i = 0;

                    //1

                    Params.SetValue("UserCreation", i);
                    i++;

                    //2
                    Params.SetValue(txtUserName.Text, i);
                    i++;

                    //3

                    Params.SetValue("ADD", i);
                    i++;

                    //4

                    Params.SetValue(message, i);
                    i++;


                    //5

                    Params.SetValue(res, i);
                    i++;

                    //6

                    Params.SetValue(Session["UserName"], i);
                    i++;

                    //7
                    Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), i);
                    i++;


                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Params);
                    #endregion
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Saving User details failed";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                UserInfo = null;
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            getUserDetails();
        }

        protected void getUserDetails()
        {
            DataSet DSUserData = null;
            try
            {
                lblStatus.Visible = false;
                lblStatus.Text = "";
                string LoginID;
                LoginID = "";

                //Validation for From date
                if (Request.QueryString["cmd"] == "Edit" || Request.QueryString["cmd"] == "View")
                {
                    LoginID = Request.QueryString["LoginID"].ToString();
                }
                else if (txtLoginID.Text.Trim() == "")
                {
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please enter Login ID";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }
                else
                {
                    LoginID = txtLoginID.Text.Trim();
                }

                DSUserData = objUserBAL.GetUserDetails(LoginID);

                if (DSUserData != null && DSUserData.Tables.Count > 0 && DSUserData.Tables[0].Rows.Count > 0)
                {
                    Session["DSUserData"] = DSUserData;
                    lblStatus.Visible = false;
                    fillUserDetails();
                }
                else
                {
                    lblStatus.Focus();
                    lblStatus.Visible = true;
                    lblStatus.Text = "User details does not exist";
                    lblStatus.ForeColor = Color.Blue;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (DSUserData != null)
                {
                    DSUserData.Dispose();
                }
            }
        }

        protected void fillUserDetails()
        {
            DataSet dsUser = null;
            Array arrStation;
            Array arrRole;
            bool isEncrypt = false;
            isEncrypt = Convert.ToBoolean(lbal.GetMasterConfiguration("EncryptPassword"));

            try
            {
                dsUser = (DataSet)Session["DSUserData"];
                txtLoginID.Text = dsUser.Tables[0].Rows[0]["LoginName"].ToString();
                if (isEncrypt)
                {
                    string pwd = DecryptPwd(dsUser.Tables[0].Rows[0]["Password"].ToString());
                    txtPassword.Attributes.Add("value", pwd);
                }
                else
                    txtPassword.Attributes.Add("value", dsUser.Tables[0].Rows[0]["Password"].ToString());
                txtUserName.Text = dsUser.Tables[0].Rows[0]["UserName"].ToString();
                txtEmail.Text = dsUser.Tables[0].Rows[0]["UserEmail"].ToString();
                txtAgentCode.Text = dsUser.Tables[0].Rows[0]["AgentCode"].ToString();
                //ddlRole.SelectedValue = dsUser.Tables[0].Rows[0]["RoleID"].ToString();
                #region Populating Roles
                arrRole = dsUser.Tables[0].Rows[0]["RoleID"].ToString().Split(',');
                for (int i = 0; i < chkListRole.Items.Count; i++)
                {
                    foreach (string stn in arrRole)
                    {
                        if (stn == chkListRole.Items[i].Value)
                        {
                            chkListRole.Items[i].Selected = true;
                        }
                    }
                }
                #endregion
                if (dsUser.Tables[0].Rows[0]["IsActive"].ToString() == "True")
                {
                    chkIsActive.Checked = true;
                }
                else
                {
                    chkIsActive.Checked = false;
                }
                if (dsUser.Tables[0].Rows[0]["IsAllStn"].ToString() == "True")
                {
                    chkSelectAll.Checked = true;
                }
                else
                {
                    chkSelectAll.Checked = false;
                }
                //Set Mobile Number
                if (dsUser.Tables[0].Rows[0]["MobileNumber"] != null)
                {
                    txtMobileNo.Text = dsUser.Tables[0].Rows[0]["MobileNumber"].ToString();
                }

                arrStation = dsUser.Tables[0].Rows[0]["StationCode"].ToString().Split(',');
                for (int i = 0; i < chkListStation.Items.Count; i++)
                {
                    foreach (string stn in arrStation)
                    {
                        if (stn == chkListStation.Items[i].Value)
                        {
                            chkListStation.Items[i].Selected = true;
                        }
                    }
                }
                txtPwdExpiry.Text = dsUser.Tables[0].Rows[0]["PwdExpiresIn"].ToString();
                lblPwdUpdatedOn.Text = "Password Updated On-" + dsUser.Tables[0].Rows[0]["PwdUpdatedOn"].ToString();
            }
            catch (Exception)
            {
            }
            finally
            {
                arrStation = null;
                if (dsUser != null)
                {
                    dsUser.Dispose();
                }
            }
        }

        protected void drLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getAllStations(drLevel.SelectedItem.Text);
            }
            catch (Exception) { }
        }

        protected string EncryptPwd(string password)
        {
            try
            {
                string strmsg = string.Empty;
                byte[] encode = new byte[password.Length];
                encode = Encoding.UTF8.GetBytes(password);
                strmsg = Convert.ToBase64String(encode);
                return strmsg;
            }
            catch (Exception ex)
            { }
            return "";
        }

        protected string DecryptPwd(string encryptpwd)
        {
            try
            {
                string decryptpwd = string.Empty;
                UTF8Encoding encodepwd = new UTF8Encoding();
                Decoder Decode = encodepwd.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
                int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                decryptpwd = new String(decoded_char);
                return decryptpwd;
            }
            catch (Exception ex)
            { }
            return "";
        }

        #region Getting Concatenated String From CheckBoxList
        private string ConcatenateCheckBoxListItems(CheckBoxList chkList)
        {
            var selectedValues =
                (from item in chkList.Items.Cast<ListItem>() where item.Selected select item.Value).ToArray();
            var selectedValuesJoined = string.Join(",", selectedValues);
            return selectedValuesJoined;
        }
        #endregion
    }
}