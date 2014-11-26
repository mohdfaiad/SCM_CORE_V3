using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectSmartCargoManager
{
    public partial class Discrepancy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((DropDownList)Grdiscrepancy.Rows[0].FindControl("ddlDiscrerpancy")).Items.Add("Lost");
            ((DropDownList)Grdiscrepancy.Rows[0].FindControl("ddlDiscrerpancy")).Items.Add("Damage");
            ((DropDownList)Grdiscrepancy.Rows[0].FindControl("ddlDiscrerpancy")).Items.Add("Wet");
            ((DropDownList)Grdiscrepancy.Rows[0].FindControl("ddlDiscrerpancy")).Items.Add("Found");


        }
    }
}
