using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class ShowRouteDetails : System.Web.UI.Page
    {
        #region Variables

        BillingAWBFileInvoiceBAL objBAL = new BillingAWBFileInvoiceBAL();
        string AWBNumber;
        DataSet ds;
       
        #endregion Variables
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                AWBNumber = Request.QueryString["AWBNumber"];
                ShowAWBRouteDetails(AWBNumber);
            }
        }

        protected void ShowAWBRouteDetails(string AWBNo)
        {
            #region Prepare Parameters
            object[] AwbInfo = new object[1];
            int i = 0;

            //0
            AwbInfo.SetValue(AWBNo, i);

            #endregion Prepare Parameters


            ds = objBAL.GetAWBRouteDetails(AwbInfo);

            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                lblStatus.Text = "";
                                grdRouteDetails.DataSource = ds.Tables[0];
                                grdRouteDetails.DataBind();
                                grdRouteDetails.Visible = true;

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>alert('No records found.');</SCRIPT>");
                            lblStatus.Text = "No records found";
                            lblStatus.ForeColor = Color.Blue;
                            grdRouteDetails.Visible = false;
                            
                            return;
                        }

                    }
                }
            }

        }
    }
}
