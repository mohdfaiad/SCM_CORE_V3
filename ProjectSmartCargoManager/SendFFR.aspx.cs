using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectSmartCargoManager
{
    public partial class SendFFR : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["FFR"] != null && Session["FFR"].ToString() != "")
                {
                    txtMessage.Text = Session["FFR"].ToString();
                }
            }

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {

        }
    }
}
