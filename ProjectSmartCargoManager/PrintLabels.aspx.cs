using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectSmartCargoManager
{
    public partial class PrintLabels : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LBLAWBPartI.Text = Session["AWBPartI"].ToString();
            LBLAWBPartII.Text = Session["AWBPartII"].ToString();
            LBLDestination.Text = Session["Destination"].ToString();
            LBLOrg.Text = Session["Origin"].ToString();
            HidPcs.Value = Session["Pcs"].ToString();
            LBLVia.Text = Session["Via"].ToString();
            LBLDate.Text = "Date : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
