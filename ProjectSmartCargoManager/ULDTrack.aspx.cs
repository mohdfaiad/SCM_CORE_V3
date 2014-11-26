using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QID.DataAccess;
using System.Data;

namespace ProjectSmartCargoManager
{
    public partial class ULDTrack : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            SQLServer da = new SQLServer(Global.GetConnectionString());
            lblStatus.Text = "";
            try
            {
                if (txtULDNumber.Text.Length < 5)
                {
                    lblStatus.Text = "Please Enter Valid ULD Number";
                    return;
                }
                ds = da.SelectRecords("spTrackULDDetials", "ULDNo", txtULDNumber.Text.Trim(), SqlDbType.VarChar);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            GrdULDSummary.DataSource = ds.Tables[0];
                            GrdULDSummary.DataBind();
                        }
                        else
                        {
                            GrdULDSummary.DataSource = null;
                            GrdULDSummary.DataBind();
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                ds = null;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                da = null;
            }
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            try 
            {
                lblStatus.Text="";
                txtULDNumber.Text="";
                GrdULDSummary.DataSource = null;
                GrdULDSummary.DataBind();
            }
            catch (Exception ex) { }
        }
    }
}
