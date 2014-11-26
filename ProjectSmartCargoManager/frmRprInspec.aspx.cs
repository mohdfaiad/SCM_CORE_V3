using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ProjectSmartCargoManager
{
    public partial class frmRprInspec : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("RprStn");
                dt.Columns.Add("UldNo");
                dt.Columns.Add("RcvDt");
                dt.Columns.Add("RprOrdNo");
                dt.Columns.Add("RprSts");
                dt.Columns.Add("StsSnc");
                dt.Columns.Add("OutDt");

                dt.Rows.Add("DGS BRU", "AKE00038AA", DateTime.Now.ToString(), "WO-30-20040420", "INSPECTED", DateTime.Now.ToString(), "");
                dt.Rows.Add("DGS BRU", "AKE00038AA", DateTime.Now.ToString(), "WO-30-20040420", "AWAITING SPARE PARTS", DateTime.Now.ToString(), "");
                dt.Rows.Add("DGS BRU", "AKE00038AA", DateTime.Now.ToString(), "WO-30-20040420", "RECEIVED", DateTime.Now.ToString(), "");
                gvRprInspec.DataSource = dt;
                gvRprInspec.DataBind();
            }
            catch (Exception)
            {
            }
        }
    }
}
