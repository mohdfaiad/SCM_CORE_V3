using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;

namespace ProjectSmartCargoManager
{
    public partial class frmLineJQ : System.Web.UI.Page
    {
        StockAllocationBAL objBAL = new StockAllocationBAL();
        Jquery objJquery = new Jquery();
        DataSet ds = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>HideDiv();</script>", false);
            if (!IsPostBack)
            {
                if (rbRegLoc.SelectedItem.Value.ToString() == "Location")
                {
                    DataSet City = objBAL.GetCityCode();
                    ddlRegionLocation.DataSource = City.Tables[0];
                    ddlRegionLocation.DataTextField = "CityCode";
                    ddlRegionLocation.DataValueField = "CityCode";
                    ddlRegionLocation.DataBind();
                    ddlRegionLocation.Items.Insert(0, "All");
                    ddlRegionLocation.SelectedIndex = 0;
                }

                if (rbRegLoc.SelectedItem.Value.ToString() == "Region")
                {
                    DataSet Region = objBAL.GetRegionCode();
                    ddlRegionLocation.DataSource = Region.Tables[0];
                    ddlRegionLocation.DataTextField = "RegionCode";
                    ddlRegionLocation.DataValueField = "RegionCode";
                    ddlRegionLocation.DataBind();
                    ddlRegionLocation.Items.Insert(0, "All");
                    ddlRegionLocation.SelectedIndex = 0;
                }

                ddlYear1.SelectedIndex = 1;
                ddlYear2.SelectedIndex = 1;
            }
        }


        #region Selection Idexchanged
        protected void rbRegLoc_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (rbRegLoc.SelectedIndex == 0)
            {
                DataSet City = objBAL.GetCityCode();
                ddlRegionLocation.DataSource = City.Tables[0];
                ddlRegionLocation.DataTextField = "CityCode";
                ddlRegionLocation.DataValueField = "CityCode";
                ddlRegionLocation.DataBind();
                ddlRegionLocation.Items.Insert(0, "All");
                ddlRegionLocation.SelectedIndex = 0;
            }
            else
            {
                DataSet Region = objBAL.GetRegionCode();
                ddlRegionLocation.DataSource = Region.Tables[0];
                ddlRegionLocation.DataTextField = "RegionCode";
                ddlRegionLocation.DataValueField = "RegionCode";
                ddlRegionLocation.DataBind();
                ddlRegionLocation.Items.Insert(0, "All");
                ddlRegionLocation.SelectedIndex = 0;
            }
        }
        #endregion

        #region Populate Click
        protected void btnPopulate_Click(object sender, EventArgs e)
        {
            CallButtonGetData();
        }
        #endregion

        #region Call Button Get Data
        private void CallButtonGetData()
        {
            try
            {
                string Month1 = ddlMonth1.SelectedItem.Value, Year1 = ddlYear1.SelectedItem.Text, FortNight1 = ddlFortnight1.SelectedItem.Text;
                string Month2 = ddlMonth2.SelectedItem.Value, Year2 = ddlYear2.SelectedItem.Text, FortNight2 = ddlFortnight2.SelectedItem.Text;
                bool isRegion = (rbRegLoc.SelectedItem.Value.ToString() == "Region");
                string RegLocName = ddlRegionLocation.SelectedItem.Text;//rbRegLoc.SelectedItem.Value.ToString();
                ds = objJquery.GetProduction(Month1,Year1,FortNight1,Month2,Year2,FortNight2,isRegion,RegLocName);
                if (ds.Tables.Count > 1)
                {
                    lblMonth1.Text = ds.Tables[0].Rows[0]["Months"].ToString();
                    lblTicks1.Text = ds.Tables[0].Rows[0]["Dates"].ToString();
                    lblMonth2.Text = ds.Tables[1].Rows[0]["Months"].ToString();
                    lblTicks2.Text = ds.Tables[1].Rows[0]["Dates"].ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>SplitArray();</script>", false);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
