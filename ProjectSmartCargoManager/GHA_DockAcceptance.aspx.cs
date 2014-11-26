using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using BAL;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class DockAcceptance_GHA : System.Web.UI.Page
    {
        #region Variable
        SQLServer db = new SQLServer(Global.GetConnectionString());
        BalGHADockAccp objBAL = new BalGHADockAccp();
        #endregion variable

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
            catch (Exception ex) { }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;

            object[] Params = new object[7];
            int i = 0;

            //0
            try
            {
                DateTime dt = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", null);
                Params.SetValue(dt.ToString("dd/MM/yyyy"), i);
                
            }
            catch (Exception ex)
            {
               //DateTime dt= DateTime.Today.Date;  
            Params.SetValue(null,i);
            
            }
            i++;         

            //1
            Params.SetValue(txtFltNo.Text, i);
            i++;

            //2 txtShipper.Text
            Params.SetValue("", i);
            i++;

            //3 txtIAC.Text
            Params.SetValue("", i);
            i++;

            //4 txtCSSF.Text
            Params.SetValue("", i);
            i++;

            //5
            Params.SetValue(txtDockNo.Text, i);
            i++;

            //6
            Params.SetValue(txtTknNo.Text, i);
            i++;


            DataSet ds = new DataSet();
            ds = objBAL.GetDockAccpList(Params);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grdDockAcceptance.DataSource = ds;
                    grdDockAcceptance.DataBind();
                }
                else
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    grdDockAcceptance.DataSource = null;
                    grdDockAcceptance.DataBind();
                }
            }
            ViewState["ds"] = ds;
            Params = null;
            ds.Dispose();
        }

        protected void grdDockAcceptance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)ViewState["ds"];
            grdDockAcceptance.PageIndex = e.NewPageIndex;
            grdDockAcceptance.DataSource = ds;
            grdDockAcceptance.DataBind();
            ds.Dispose();
        }

        protected void grdDockAcceptance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Start")
            {
                int RowIndex = Convert.ToInt32(e.CommandArgument);
                string DockNo = ((TextBox)grdDockAcceptance.Rows[RowIndex].FindControl("txtDockNo")).Text;

                Session["DockNoFromDockAccp"] = null;
               
                if(DockNo!="")
                Session["DockNoFromDockAccp"]=DockNo;

                else if (DockNo == "")
                {
                    lblStatus.Text = "Enter Dock No";
                    lblStatus.ForeColor = Color.Red;
                }

                string TokenNo = ((Label)grdDockAcceptance.Rows[RowIndex].FindControl("lblTokenNo")).Text;
                string TokenDt = ((Label)grdDockAcceptance.Rows[RowIndex].FindControl("lblTokenDt")).Text;
                Response.Redirect("GHA_Acceptance.aspx?" + "No=" + TokenNo + "&Dt=" + TokenDt); 
            }
        }
    }
}
