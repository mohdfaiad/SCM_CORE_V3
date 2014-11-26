using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ProjectSmartCargoManager
{
    public partial class frmDemCost : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DemType");
            dt.Columns.Add("lblCostUnit");
            dt.Columns.Add("lblCost");
            dt.Columns.Add("lblULDOwner");
            dt.Columns.Add("lblULDType");
            dt.Rows.Add("INTERNAL", "$", "200","QID","Cubic");
            dt.Rows.Add("INTERNAL", "Rs", "2000","KFA","Cubic");
            dt.Rows.Add("EXTERNAL", "Din", "1200","QID","Cubic");
            gvDemCost.DataSource = dt;
            gvDemCost.DataBind();

        }

    }
}
