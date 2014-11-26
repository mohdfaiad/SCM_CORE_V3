using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using BAL;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace ProjectSmartCargoManager
{
    public partial class SetPassword : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        LoginBL lBal = new LoginBL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string UserName = Request.QueryString["ID"].ToString();
                lblUserName.Text = UserName;
                LoadPwd(UserName);
            }
        }

        protected void LoadPwd(string UserName)
        {
            bool IsDecrypt = false;
            IsDecrypt = Convert.ToBoolean(lBal.GetMasterConfiguration("EncryptPassword"));
            DataSet ds = new DataSet();
            string pwds = string.Empty;
            try
            {
                ds = da.SelectRecords("sp_GetPwds", "UserName", UserName, SqlDbType.VarChar);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (IsDecrypt)
                        {
                            string decrypt = DecryptPwd(ds.Tables[0].Rows[i][0].ToString());
                            if (pwds == "")
                                pwds = decrypt;
                            else
                                pwds = pwds + "," + decrypt;
                        }
                        else
                        {
                            if (pwds == "")
                                pwds = ds.Tables[0].Rows[i][0].ToString();
                            else
                                pwds = pwds + "," + ds.Tables[0].Rows[i][0].ToString();
                        }
                    }
                    hdnPwds.Value = pwds;
                }
                hdnCurrPassword.Value = ds.Tables[1].Rows[0][0].ToString();
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

        protected void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;

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

                bool IsEncrypt = false;
                IsEncrypt = Convert.ToBoolean(lBal.GetMasterConfiguration("EncryptPassword"));
                string NewPwd = txtNewPwd.Text.Trim();
                if (IsEncrypt)
                {
                    NewPwd = EncryptPwd(NewPwd);
                }

                #region parameters
                object[] PValue = new object[4];
                string[] Pname = new string[4];
                SqlDbType[] PType = new SqlDbType[4];

                string PwdExpiryDays = "";
                PwdExpiryDays = lbal.GetMasterConfiguration("PwdExpiry");

                int i = 0;

                PValue.SetValue(lblUserName.Text, i);
                Pname.SetValue("UserName", i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                PValue.SetValue(NewPwd, i);
                Pname.SetValue("Pwd", i);
                PType.SetValue(SqlDbType.VarChar, i);
                i++;

                PValue.SetValue(DateTime.Now, i);
                Pname.SetValue("UpdatedOn", i);
                PType.SetValue(SqlDbType.DateTime, i);
                i++;

                PValue.SetValue(PwdExpiryDays, i);
                Pname.SetValue("PwdExpiry", i);
                PType.SetValue(SqlDbType.VarChar, i);
                #endregion

                bool res = false;
                res = da.ExecuteProcedure("Sp_SetNewPwd", Pname, PType, PValue);
                if (res)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ShowConfirmMsg();</SCRIPT>", false);
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    lblStatus.Text = "Password could not be set";
                    lblStatus.ForeColor = Color.Red;
                }

            }
            catch (Exception ex)
            { }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Login.aspx");
            }
            catch (Exception ex)
            { }
        }
    }
}
    