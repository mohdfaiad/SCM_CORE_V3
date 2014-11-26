using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ProjectSmartCargoManager
{
    public partial class frmRepairInspection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dtCreditInfo = new DataTable();
                dtCreditInfo.Columns.Add("WorkOrder");
                dtCreditInfo.Columns.Add("Start");
                dtCreditInfo.Columns.Add("Finish");
                dtCreditInfo.Rows.Add("WRO43", "2013-11-06", "2013-11-09");
                dtCreditInfo.Rows.Add("WRO44", "2013-11-12", "2013-11-20");
                dtCreditInfo.Rows.Add("WRO45", "2013-11-25", "-");
                gvHistory.DataSource = dtCreditInfo;
                gvHistory.DataBind();

                DataTable dtCredit = new DataTable();
                dtCredit.Columns.Add("PartNo");
                dtCredit.Columns.Add("Desc");
                dtCredit.Columns.Add("Price");
                dtCredit.Rows.Add("HY7756 1", "auto generated 1", "500.01 USD");
                dtCredit.Rows.Add("HY7756 2", "auto generated 2", "1000 USD");
                gvApplicableParts.DataSource = dtCredit;
                gvApplicableParts.DataBind();

                DataTable dtCreditAssi = new DataTable();
                dtCreditAssi.Columns.Add("Ref");
                dtCreditAssi.Columns.Add("Desc");
                dtCreditAssi.Columns.Add("Count");
                dtCreditAssi.Columns.Add("Price");
                dtCreditAssi.Columns.Add("Wmin");
                dtCreditAssi.Columns.Add("Lmin");

                dtCreditAssi.Rows.Add("HY7756 1", "auto generated 1", "10", "0.00 USD", "0", "0.00");
                gvAssiItems.DataSource = dtCreditAssi;
                gvAssiItems.DataBind();
            }
            catch (Exception)
            {
            }
        }
    }
}
