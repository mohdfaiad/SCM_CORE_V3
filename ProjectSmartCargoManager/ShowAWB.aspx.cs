using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;


namespace ProjectSmartCargoManager
{
    public partial class ShowAWB : System.Web.UI.Page
    {
        StockAllocationBAL objBAL = new StockAllocationBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LBLStatus.Text = "";

                FillGrid(int.Parse(Request.QueryString[0]), int.Parse(Request.QueryString[1]));
                
            }
        }

        private void FillGrid(int from, int to)
        {
            try
            {
                
                object[] AWB = new object[]
                    {from,to};
                DataSet ds = objBAL.GetUnusedAWB(AWB);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    GRDShowFlights.DataSource = ds.Tables[0];
                    GRDShowFlights.DataBind();
                }
            }
            catch (Exception ex)
            {

                LBLStatus.Text = "Error :" + ex.Message;
            }
            
        }
    }
}
