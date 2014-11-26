using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using QID.DataAccess;
using System.Drawing;
using System.Text;
using BAL;
using System.Text.RegularExpressions;

namespace ProjectSmartCargoManager
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        #region Variables
        SQLServer da = new SQLServer(Global.GetConnectionString());
        LoginBL lBal = new LoginBL();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUserName.Text = Convert.ToString(Session["UserName"]);
                LoadPwd();
                try
                {
                    if (Request.QueryString != null)
                    {
                        if (Request.QueryString["Exp"] != null)
                        {
                            lblMsg.Text = "Password has Expired.Please Change the Password";
                            lblMsg.ForeColor = Color.Red;
                        }

                    }
                }
                catch (Exception ex)
                { }
            }
        }

        #region bnShow_Click
        protected void bnShow_Click(object sender, EventArgs e)
        {
            string OldPwdEncrypt = txtOldPwd.Text.Trim(), EncryptedPwd = txtNewPwd.Text.Trim();
            bool IsEncryptPwd = false;
            lblMsg.ForeColor = Color.Red;
            lblMsg.Text = string.Empty;
            DataSet ds = null;
            object[] paramvalue = new object[6];
            string[] paramname = new string[6];
            SqlDbType[] paramtype = new SqlDbType[6];
            try
            {
                #region Check Password Strength
                LoginBL lbal = new LoginBL();
                string regexStrong = "(?=.*?[A-Za-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*]).{6,25}$";
                string regexMedium = "(?=.*?[A-Za-z])(?=.*?[0-9]).{6,25}$";
                string regexLow = @"^[\S]{6,}$";
                bool pwdchk = false;
                string PwdStrength = lbal.GetMasterConfiguration("PasswordStrength");

                if (PwdStrength == "Strong")
                {
                    pwdchk = Regex.IsMatch(txtNewPwd.Text.Trim(), regexStrong);
                    if (pwdchk == false)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Password must be minimum 6 letters and must contain 1 alphabet,1 Number and One of Special Characters #?!@$%^&*')</script>", false);
                        return;
                    }
                }
                if (PwdStrength == "Medium")
                {
                    pwdchk = Regex.IsMatch(txtNewPwd.Text.Trim(), regexMedium);
                    if (pwdchk == false)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Password must be minimum 6 letters and alphanumeric')</script>", false);
                        return;
                    }
                }
                if (PwdStrength == "Low")
                {
                    pwdchk = Regex.IsMatch(txtNewPwd.Text.Trim(), regexLow);
                    if (pwdchk == false)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('Password must be minimum 6 letters')</script>", false);
                        return;
                    }
                }

                #endregion

                IsEncryptPwd = Convert.ToBoolean(lBal.GetMasterConfiguration("EncryptPassword"));
                if (IsEncryptPwd)
                {
                    OldPwdEncrypt = EncryptPwd(txtOldPwd.Text.Trim());
                    EncryptedPwd = EncryptPwd(txtNewPwd.Text.Trim());
                }
                if (txtUserName.Text != "" && txtOldPwd.Text != "" && txtNewPwd.Text != "" && txtNewConformPwd.Text != "")
                {
                    if (txtNewConformPwd.Text != txtNewPwd.Text)
                    {
                        lblMsg.Text = "New and Confirm Password must be same.";
                        return;
                    }
                    DateTime dt = Convert.ToDateTime(Session["IT"].ToString());

                    string PwdExpiryDays = "";
                   PwdExpiryDays= lbal.GetMasterConfiguration("PwdExpiry");

                    paramvalue[0] = txtUserName.Text.Trim();
                    paramvalue[1] = OldPwdEncrypt;
                    paramvalue[2] = EncryptedPwd;
                    paramvalue[3] = dt;
                    paramvalue[4] = Session["UserName"].ToString();
                    paramvalue[5] = PwdExpiryDays;

                    paramname[0] = "LoginName";
                    paramname[1] = "OldPassword";
                    paramname[2] = "Password";
                    paramname[3] = "UpdatedOn";
                    paramname[4] = "UpdatedBy";
                    paramname[5] = "PwdExpiry";

                    paramtype[0] = SqlDbType.NVarChar;
                    paramtype[1] = SqlDbType.NVarChar;
                    paramtype[2] = SqlDbType.NVarChar;
                    paramtype[3] = SqlDbType.DateTime;
                    paramtype[4] = SqlDbType.NVarChar;
                    paramtype[5] = SqlDbType.VarChar;

                    ds = da.SelectRecords("SpChangePassword", paramname, paramvalue, paramtype);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                                {
                                    LoadPwd();
                                    lblMsg.ForeColor = Color.Green;
                                    lblMsg.Text = "Password Change Successfully done.";
                                    ds.Dispose();
                                    paramvalue = null;
                                    paramname = null;
                                    paramtype = null;
                                    Session["UserName"] = null;
                                    Session["SSOID"] = null;
                                    Response.Redirect("Home.aspx");
                                }
                                else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                                {
                                    lblMsg.Text = "Error. Try again";
                                }
                                else
                                {
                                    lblMsg.Text = "Invalid User Name/Password.";
                                }
                            }
                        }
                        ds.Dispose();
                    }
                }
                else
                {
                    lblMsg.Text = "Please Fill the details.";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Oops! there might be some problem: " + ex.Message;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
                paramvalue = null;
                paramname = null;
                paramtype = null;
            }
        }
        #endregion bnShow_Click

        protected void LoadPwd()
        {
            bool IsDecrypt = false;
            IsDecrypt = Convert.ToBoolean(lBal.GetMasterConfiguration("EncryptPassword"));
            DataSet ds = new DataSet();
            string pwds = string.Empty;
            try
            {
                ds = da.SelectRecords("sp_GetPwds", "UserName", txtUserName.Text.Trim(), SqlDbType.VarChar);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (IsDecrypt)
                        {
                            string decrypt = DecryptPwd(ds.Tables[0].Rows[i][0].ToString());
                            if (pwds == "")
                                pwds = decrypt.ToLower();
                            else
                                pwds = pwds + "," + decrypt.ToLower();
                        }
                        else
                        {
                            if (pwds == "")
                                pwds = ds.Tables[0].Rows[i][0].ToString().ToLower();
                            else
                                pwds = pwds + "," + ds.Tables[0].Rows[i][0].ToString().ToLower();
                        }
                    }
                    hdnPwds.Value = pwds;
                }
                else
                {
                    if (IsDecrypt)
                    {
                        string decrypt = DecryptPwd(ds.Tables[1].Rows[0][0].ToString());
                        if (pwds == "")
                            pwds = decrypt.ToLower();
                        else
                            pwds = pwds + "," + decrypt.ToLower();
                    }
                    else
                    {
                        if (pwds == "")
                            pwds = ds.Tables[1].Rows[0][0].ToString().ToLower();
                        else
                            pwds = pwds + "," + ds.Tables[1].Rows[0][0].ToString().ToLower();
                    }
                    hdnPwds.Value = pwds;
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
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
    }
}