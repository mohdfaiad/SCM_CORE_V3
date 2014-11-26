using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using BAL;
using QID.DataAccess;
namespace ProjectSmartCargoManager
{
    public partial class FrmAuthenticateUser : System.Web.UI.Page
    {
        SMS objsms = new SMS();
        LoginBL objLogin = new LoginBL(); 
        SQLServer da = new SQLServer(Global.GetConnectionString());
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                   
                }
            }
            catch (Exception ex)
            { }


        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            try
            {
                string verification = txtauthenicationcode.Text;
                string Station = Session["Station"].ToString();
                string LoginName = Session["UserName"].ToString();

                bool res = objLogin.Validate(verification, LoginName, Station);

                if (res == true)
                {
                    Response.Redirect("Home.aspx",false);
                }
                else
                {
                    Response.Redirect("Login.aspx");  
                }
            }
            catch (Exception ex)
            { 
             
            }
        }

      
    }
}
