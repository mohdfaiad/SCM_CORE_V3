using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class ListArrival : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                getdata();
            }
            catch (Exception ex)
            { }
        }
        #endregion Load
        #region GetData
        public void getdata()
        {
            try
            {

                string[] paramname = new string[2];
                paramname[0] = "AWBNumber";
                paramname[1] = "FlightNumber";

                string AgentCode = string.Empty;
                string City = string.Empty;


                object[] paramvalue = new object[2];


                if (txtawbnumber.Text.Trim() == "")
                {
                    paramvalue[0] = "";
                }
                else
                {
                    paramvalue[0] = txtawbnumber.Text.Trim() ;
                }

                if (txtflightNumber.Text.Trim() == "")
                {
                    paramvalue[1] = "";
                }
                else
                {
                    paramvalue[1] = txtflightNumber.Text.Trim() ;

                }


                SqlDbType[] paramtype = new SqlDbType[2];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;

                DataSet ds = da.SelectRecords("Sp_ListArrival", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            grdArrivalDetails.DataSource = ds.Tables[0];
                            grdArrivalDetails.DataBind();
                        }
                        else
                        {
                            grdArrivalDetails.Visible = false;
                            lblStatus.Text = "No Data Found";
                            //ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "NoData()", true);
                            //LoadGridCreditDetails();
                        }
                    }
                }
            }
            catch (Exception ex)
            { }


        }
        #endregion Getdata
        #region Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtawbnumber.Text = "";
            txtflightNumber.Text = "";
        }
        #endregion Clear
        #region List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                getdata();
            }
            catch (Exception ex)
            { }
        }
        #endregion List
    }
}
