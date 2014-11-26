using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectSmartCargoManager
{
    public partial class SCM_Dynamic : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtResult.Text = "";
            }
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            doTest();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home.aspx", false);
        }

        private void doTest()
        {
            try
            {
                BAL.BALDiagnostics objBAL = new BAL.BALDiagnostics();
                string result = "";
                //result = txtResult.Text;

                #region Testing BAL
                result += "Testing BAL . . . ";
                txtResult.Text = result;
                bool flag = objBAL.testBAL();
                if (flag == true)
                {
                    result += "OK";
                }
                else
                {
                    result += "Error";
                }
                txtResult.Text = result;
                #endregion

                #region Testing DAL
                result += "\r\nTesting DAL . . . ";
                txtResult.Text = result;
                flag = false;
                flag = objBAL.testDAL();
                if (flag == true)
                {
                    result += "OK";
                }
                else
                {
                    result += "Error";
                }
                txtResult.Text = result;
                #endregion

            }
            catch (Exception ex)
            { }
        }

    }
}
