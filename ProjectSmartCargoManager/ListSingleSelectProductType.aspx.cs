using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class ListSingleSelectProductType : System.Web.UI.Page
    {
        BALProductType objBAL = new BALProductType();
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ddlds1 = new DataSet("ListSingleSelectProductType_1");
            try
            {
                if (!IsPostBack)
                {
                    //Find out parameters from query string.
                    string origin = "";
                    string destination = "";
                    string fltnum = "";
                    string fltdt = "";
                    string commcode = "";
                    decimal weight = 0;
                    string shipdate = "";
                    if (Request.QueryString != null)
                    {
                        if (Request.QueryString["origin"] != null)
                            origin = Request.QueryString["origin"].ToString();
                        if (Request.QueryString["destination"] != null)
                            destination = Request.QueryString["destination"].ToString();
                        if (Request.QueryString["fltnum"] != null)
                            fltnum = Request.QueryString["fltnum"].ToString();
                        if (Request.QueryString["fltdt"] != null)
                            fltdt = Request.QueryString["fltdt"].ToString();
                        if (Request.QueryString["commcode"] != null)
                            commcode = Request.QueryString["commcode"].ToString();
                        if (Request.QueryString["weight"] != null)
                            decimal.TryParse(Request.QueryString["weight"].ToString(), out weight);
                        if (Request.QueryString["shipDate"] != null)
                            shipdate = Request.QueryString["shipDate"].ToString();
                    }

                    ddlds1 = objBAL.GetMatchingProductType(origin, destination, fltnum, fltdt, "", commcode, weight, shipdate);

                    if (ddlds1.Tables.Count > 0)
                    {
                        if (ddlds1.Tables[0].Rows.Count > 0)
                        {
                            GRD.DataSource = null;
                            GRD.DataSource = ddlds1.Tables[0];
                            GRD.DataBind();
                            //Disable radio buttons if Capacity is 'NOT AVAILABLE' for a particular product type.
                            foreach (GridViewRow gvr in GRD.Rows)
                            {
                                if (((Label)gvr.FindControl("lblAvailableCapacity")).Text == "NOT AVAILABLE")
                                {
                                    ((RadioButton)gvr.FindControl("rbSelect")).Enabled = false;
                                    ((RadioButton)gvr.FindControl("rbSelect")).Checked = false;
                                }
                            }
                        }
                        else
                        {   //Get values from table 3.
                            if (ddlds1.Tables.Count > 3)
                            {
                                if (ddlds1.Tables[3].Rows.Count > 0)
                                {
                                    GRD.DataSource = null;
                                    GRD.DataSource = ddlds1.Tables[3];
                                    GRD.DataBind();
                                }
                            }
                        }
                    }
                    else
                    {
                        lblStatus.Text = "No product types found";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "alert('Error :'" + ex.Message + ")", true);
            }
            finally
            {
                if (ddlds1 != null)
                {
                    ddlds1.Dispose();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "window.close();", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strSelected = "0";
            bool isSelected = false;
            try
            {
                
                for (int i = 0; i < GRD.Rows.Count; i++)
                {
                    if (((CheckBox)GRD.Rows[i].FindControl("rbSelect")).Checked)
                    {
                        strSelected = ((Label)GRD.Rows[i].FindControl("lblSrNo")).Text;
                        isSelected = true;
                        break;
                    }
                }
                
            }
            catch (Exception)
            {
                
            }
            if (!isSelected)
            {
                lblStatus.Text = "Please select a Product Type.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "javascript:CloseWindow('" + strSelected + "');", true);
            }
        }

    }
}
