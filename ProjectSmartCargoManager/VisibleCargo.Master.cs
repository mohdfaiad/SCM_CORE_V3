using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;

namespace Visible_Cargo
{
    public partial class VisibleCargo : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    MasterBAL objBal = new MasterBAL();
                    string contactURL = objBal.ContactURL();
                    if (contactURL != null && contactURL != "")
                    {
                        hlnkContactUs.NavigateUrl = contactURL;
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
