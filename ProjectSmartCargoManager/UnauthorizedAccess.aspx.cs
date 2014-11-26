using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectSmartCargoManager
{
    public partial class UnauthorizedAccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Button Close
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home.aspx");
        }
        #endregion
    }
}
