using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectSmartCargoManager
{
    public partial class SearchPopup : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString != null)
                    {
                        lblHeader.Text = Request.QueryString.Get("type").ToString();                        
                        txtCode.Text = Request.QueryString.Get("val1").ToString();
                        if (txtCode.Text == "null")
                        {
                            txtCode.Text = "";
                            txtCode.Visible = false;
                            lblCode.Visible = false;
                        }
                        txtName.Text = Request.QueryString.Get("val2").ToString();
                        if (txtName.Text == "null")
                        {                                                        
                            txtName.Text = "";
                            txtName.Visible = false;
                            lblName.Visible = false;
                        }
                    }
                }
                catch (Exception)
                {
                }
                if (lblHeader.Text == "Origin")
	            {
                    btnList.Attributes.Add("onclick", "javascript:setValues('txtCode','ctl00_ContentPlaceHolder1_txtOrg','null','null');return false;");
	            }
                
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {

        }
    }
}
