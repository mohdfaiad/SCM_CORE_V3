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
//using DAL;
using ProjectSmartCargoManager;
//using System.Collections.Generic;

      
namespace ProjectSmartCargoManager
{
    public partial class Dashboard : System.Web.UI.Page
    {
        
        //DataAccess da = new DataAccess();  
        SQLServer da = new SQLServer(Global.GetConnectionString());
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["AWBNumber"] = ((HyperLink)grdDashboard.Rows[0].Cells[0].Controls[0]).Text;
            //Session["AWBNumber"]=((HyperLink)grdDashboard.FindControl("AWBNumber")).Text;
             
            txtfromdate.Text = DateTime.Today.ToString();
            //DataSet ds = da.SelectRecords("sp_dashboard");
            
            string[] paramname = new string[1];
            paramname[0] = "FltDate";
            string FltDate ="NA";
             //"1990-04-09 00:00:00.000"
            object[] paramvalue = new object[1];
            if (FltDate == "NA")
            {

                FltDate = "NA"; // NA=not available 
                paramvalue[0] = "NA";
            }
                
            else
            {
                
                paramvalue[0] = DateTime.Parse(FltDate).ToString("yyyy-MM-dd");
            }

            SqlDbType[] paramtype = new SqlDbType[1];
            paramtype[0] = SqlDbType.VarChar;

            

            DataSet ds = da.SelectRecords("sp_dashboard", paramname, paramvalue, paramtype);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        grdDashboard.DataSource = ds.Tables[0];
                        grdDashboard.DataBind();


                        //code for checking grid is emply or not


                    }
                }
            }
        
        }

        protected void btnList_Click(object sender, EventArgs e)
        {

            string[] paramname = new string[1];
            paramname[0] = "fltdate";
            string  FltDate=string.Empty;

            //if (FltDate == "")
            //{

            //    FltDate = "NA"; // NA=not available 
            //}


            object[] paramvalue = new object[1];
            paramvalue[0] = DateTime.Parse(txttodate.Text.Trim()).ToString("yyyy-MM-dd");

            SqlDbType[] paramtype = new SqlDbType[1];
            paramtype[0] = SqlDbType.VarChar;

            DataSet ds = da.SelectRecords("sp_dashboard", paramname, paramvalue, paramtype);
             if (ds != null)
             {
                 if (ds.Tables.Count > 0)
                 {
                     if (ds.Tables[0].Rows.Count > 0)
                     {
                         grdDashboard.DataSource = ds.Tables[0];
                         grdDashboard.DataBind();
                     }
                     else
                     {
                         grdDashboard.Visible = false;  
                         ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "NoData()", true);
                     }
                 }
                 //else
                 //{
                 //    ClientScript.RegisterStartupScript(this.GetType(), "ShowPnl2", "NoData()", true);
                 //}
             }
        }
        
        //protected void grdDashboard_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Session["AWBNumber"] = ((HyperLink)grdDashboard.FindControl("AWBNumber")).Text;
        //}

        public void AWBURL(object sender,EventArgs e)
        {
            //Response.Redirect("FrmConBooking.aspx?awbno="+);
            LinkButton btn = (LinkButton)sender;
             GridViewRow newgrdDashboard  = (GridViewRow)btn.NamingContainer;
             int rowindex = newgrdDashboard.RowIndex;
                        
             string str= ((LinkButton)grdDashboard.Rows[rowindex].FindControl("Lnkawburl")).Text;
             Response.Redirect("~//GHA_ConBooking.aspx?command=View&AWBNumber=" + str.Trim(), false);             
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txttodate.Text = "";
            Page_Load(sender, e);

            grdDashboard.Visible = true;  
        }
    }
}
