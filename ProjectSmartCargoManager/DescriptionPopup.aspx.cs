using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectSmartCargoManager
{
    public partial class DescriptionPopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    txtDescription.Text = Request.QueryString["Text"] != null ? Request.QueryString["Text"].ToString() : string.Empty;
                }
                catch (Exception ex)
                { }
            }
        }
    }
}
