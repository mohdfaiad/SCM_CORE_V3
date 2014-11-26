using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ProjectSmartCargoManager
{
    public partial class ShowStockpopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            if (Session["stockPopup"] != null)
            {

                DataTable dvTab = ((DataView)Session["stockPopup"]).ToTable();
                grdStockAllocation.DataSource = dvTab;
                grdStockAllocation.DataBind();
                LBLStatus.Text = "Please select stock from the range given below";
                Session["stockPopup"] = null;
            
            }
        }
    }
}
