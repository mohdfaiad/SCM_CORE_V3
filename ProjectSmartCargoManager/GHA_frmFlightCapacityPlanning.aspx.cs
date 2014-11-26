using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using System.Configuration;
using System.Data.SqlClient;
using BAL;
using QID.DataAccess;
using System.Drawing;



namespace ProjectSmartCargoManager
{
    public partial class GHA_frmFlightCapacityPlanning : System.Web.UI.Page
    {
        string gvUniqueID = String.Empty;
        SQLServer da = new SQLServer(Global.GetConnectionString());
        int gvNewPageIndex = 0;
        int gvEditIndex = -1;
        ListBookingBAL objBAL = new ListBookingBAL();
        string errormessage = "";
        string gvSortExpr = String.Empty;
        SqlConnection sc = null;
        SqlCommand scmd = null;
        SqlDataReader sdr = null;
        SqlConnection sc1 = null;
        SqlCommand scmd1 = null;
        GHA_BALFlightCapacity BFC = new GHA_BALFlightCapacity();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                if (!IsPostBack)
                {
                    LoadStationDropDowns();
                    GetAgentCode();

                    txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                    ddlSource.SelectedIndex = ddlSource.Items.IndexOf(ddlSource.Items.FindByText(Session["Station"].ToString()));

                }
                if (Session["awbPrefix"] != null)
                    TXTAWBPrefix.Text = Session["awbPrefix"].ToString();
                else
                {
                    MasterBAL objBal = new MasterBAL();
                    Session["awbPrefix"] = objBal.awbPrefix();
                    TXTAWBPrefix.Text = Session["awbPrefix"].ToString();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;   
            }
        }
        public bool IsValide()
        {
            lblStatus.Text = "";

            try
            {
                int.Parse(TXTAWBNumber.Text.Trim() == "" ? "0" : TXTAWBNumber.Text.Trim());

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: AWBNumber not valid.";
                return false;
            }

            if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() != "")
            {
                lblStatus.Text = "Select To Date";
                return false;
            }
            if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() == "")
            {
                lblStatus.Text = "Select From Date";
                return false;
            }

            if (txtFromDate.Text.Trim() != "" && txtToDate.Text.Trim() != "")
            {
                DateTime dtFrom, dtTo;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                try
                {
                    dtFrom = Convert.ToDateTime(txtFromDate.Text.Trim());

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "From date is invalid";
                    return false;
                }

                try
                {
                    dtTo = Convert.ToDateTime(txtToDate.Text.Trim());

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "To date is invalid";
                    return false;
                }

                if (dtFrom > dtTo)
                {
                    lblStatus.Text = "From date should be smaller than to date.";
                    return false;
                }
            }

            return true;
        }

        #region GridView1 Event Handlers
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow row = e.Row;
                string strSortfl = string.Empty;
                string strSortfldt = string.Empty;

                if (row.DataItem == null)
                {
                    return;
                }

                DataTable dt1 = new DataTable();
                GridView gv = new GridView();
                gv = (GridView)row.FindControl("GridView2");

                if (gv.UniqueID == gvUniqueID)
                {
                    gv.PageIndex = gvNewPageIndex;
                    gv.EditIndex = gvEditIndex;
                    ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["FlightNo"].ToString() + "," + ((DataRowView)e.Row.DataItem)["FlightDate"].ToString() + "," + ((DataRowView)e.Row.DataItem)["DeptTime"].ToString() + "," + ((DataRowView)e.Row.DataItem)["ArrTime"].ToString() + "','one');</script>");
                }
                strSortfl = ((DataRowView)e.Row.DataItem)["FlightNo"].ToString();
                strSortfldt = ((DataRowView)e.Row.DataItem)["FlightDate"].ToString();
                string ori = ((DataRowView)e.Row.DataItem)["Origin"].ToString();
                string dest = ((DataRowView)e.Row.DataItem)["Destination"].ToString();

                sc = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);
                sc.Open();
                scmd = new SqlCommand("spgetInternalDataofCapacity", sc); 
                scmd.CommandType = CommandType.StoredProcedure;
                scmd.Parameters.AddWithValue("@FlightNo", strSortfl);
                scmd.Parameters.AddWithValue("@FlightDt", strSortfldt);
                //scmd.Parameters.AddWithValue("@Ori", ori);
                //scmd.Parameters.AddWithValue("@dest", dest);
                scmd.Parameters.AddWithValue("@prefix", TXTAWBPrefix.Text.Trim());
                scmd.Parameters.AddWithValue("@AWB", TXTAWBNumber.Text.Trim());
                scmd.Parameters.AddWithValue("@Agent", ddlAgentCode.SelectedItem.Text);

                sdr = scmd.ExecuteReader();
                dt1.Load(sdr);
                gv.DataSource = dt1;
                gv.DataBind();
                sc.Close();
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }
        #endregion
        public void editMode(object sender, EventArgs e)
        {

            try
            {
                string awb = hdn.Value;
                //if (awb.Length > 8)
                //{
                //    awb = awb.Substring(awb.Length - 8);
                //}
                //string preAWB = string.Empty;
                //if (Session["awbPrefix"] != null)
                //{
                //    preAWB = Session["awbPrefix"].ToString();

                //}
                //else
                //{
                //    MasterBAL objBal = new MasterBAL();
                //    Session["awbPrefix"] = objBal.awbPrefix();
                //    preAWB = Session["awbPrefix"].ToString();
                //}
                
                //Response.Redirect("~//ConBooking.aspx?command=Edit&AWBNumber=" + awb,false);
              // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'><a href='ConBooking.aspx?command=Edit&AWBNumber='" + awb+"' target='blank' re_target='blank'></a></script>", false);
                string query = "'GHA_ConBooking.aspx?command=Edit&AWBNumber=" + awb + "'";
                //Response.Write("<script>");
                //Response.Write("window.open(" + query + ",'_blank')");
                //Response.Write("</script>");

               // ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open(" + query + ", target='blank', 'toolbar=0,location=0,menubar=0');", true);
              //  ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "javascript:newTab(" + query + ")", true);

                //HttpContext.Current.Response.Write("<script>window.open("+query+");</script>");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open(" + query + ",'_blank');", true);
                
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "newwindow.focus();", true);
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }
        private void getdata(string source, string dest, string flight, string status, string fromdate, string todate, string awbnumber, string StrAgentCode,string prefix)
        {
            try
            {
                DataSet ds=new DataSet();
                BFC.GetAllAWBs(source, dest, flight, fromdate, todate, prefix, status, awbnumber, ref ds, ref errormessage, StrAgentCode);
                if (ds != null || ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GridView1.DataSource = ds.Tables[0];
                        GridView1.DataBind();
                        Session["dsExp"] = ds.Tables[0];
                        btnExport.Visible = true;
                    }
                    else
                    {
                        lblStatus.Text = "No Records Found";
                        lblStatus.ForeColor = Color.Blue;
                        GridView1.DataSource = null;
                        GridView1.DataBind();
                        //return;

                    }
                }

                //DataTable dt1 = new DataTable();
                //sc = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);

                //sc.Open();
                //scmd = new SqlCommand("spgetAllMainDataforCapacityPlanning", sc); //receipt come from xpress bag details
                //scmd.CommandType = CommandType.StoredProcedure;
                //scmd.Parameters.AddWithValue("@fromdate", "2013-05-21");
                //scmd.Parameters.AddWithValue("@todate", "2013-6-22");

                //scmd.Parameters.AddWithValue("@fromdate", fromdate);
                //scmd.Parameters.AddWithValue("@todate", todate);
                //scmd.Parameters.AddWithValue("@source", source);
                //scmd.Parameters.AddWithValue("@dest", dest);
                //scmd.Parameters.AddWithValue("@flight", flight);
                //scmd.Parameters.AddWithValue("@status", status);
                //scmd.Parameters.AddWithValue("@awbno", awbnumber);
                //scmd.Parameters.AddWithValue("@agentCode", StrAgentCode);

                //sdr = scmd.ExecuteReader();
                //dt1.Load(sdr);
                //GridView1.DataSource = dt1;
                //GridView1.DataBind();
                //sc.Close();
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }

        protected void ddlSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetFlights();
        }

        protected void ddlDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetFlights();
        }

        #region UserDefined

        public void LoadStationDropDowns()
        {
            try
            {
                DataSet dsResult = new DataSet();

                if (objBAL.GetAllStaions(ref dsResult, ref errormessage))
                {

                    DataRow row = dsResult.Tables[0].NewRow();
                    row["Code"] = "All";
                    dsResult.Tables[0].Rows.Add(row);

                    ddlSource.DataSource = dsResult.Tables[0].Copy();
                    ddlSource.DataTextField = "Code";
                    ddlSource.DataValueField = "Code";
                    ddlSource.DataBind();

                    ddlDest.DataSource = dsResult.Tables[0].Copy();
                    ddlDest.DataTextField = "Code";
                    ddlDest.DataValueField = "Code";
                    ddlDest.DataBind();

                    //ddlFlight.Items.Clear();
                    //ddlFlight.Items.Add(new ListItem("All", "All"));

                    ddlSource.SelectedValue = "All";
                    ddlDest.SelectedValue = "All";

                }
                else
                {
                    lblStatus.Text = "" + errormessage;
                }

            }
            catch (Exception ex)
            {
                
                lblStatus.Text = ex.Message;
            }
        }

        //public void GetFlights()
        //{
        //    try
        //    {
        //        string source, dest, date;
        //        source = dest = date = "";

        //        if (ddlSource.SelectedItem.Text.Trim() == "All" || ddlDest.SelectedItem.Text.Trim() == "All")
        //        {
        //            ddlFlight.Items.Clear();
        //            ddlFlight.Items.Add(new ListItem("All", "All"));
        //            ddlFlight.SelectedIndex = 0;
        //        }
        //        else
        //        {
        //            source = ddlSource.SelectedItem.Text;
        //            dest = ddlDest.SelectedItem.Text;
        //            date = "";// txtDate.Text;


        //            DataSet dsResult = new DataSet();

        //            if (objBAL.GetAllFlights(source, dest, date, ref dsResult, ref errormessage))
        //            {

        //                DataRow row = dsResult.Tables[0].NewRow();
        //                row["FltNumber"] = "All";
        //                dsResult.Tables[0].Rows.Add(row);

        //                ddlFlight.DataSource = dsResult.Tables[0].Copy();
        //                ddlFlight.DataTextField = "FltNumber";
        //                ddlFlight.DataValueField = "FltNumber";
        //                ddlFlight.DataBind();
        //            }
        //            else
        //            {
        //                lblStatus.Text = "" + errormessage;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblStatus.Text = "" + ex.Message;
        //    }
        //}

        #endregion
        public void GetAgentCode()
        {
            try
            {
                DataSet dsResult = new DataSet();
                //string errormessage = "";

                //string level = (((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlParameter")).Text);
                dsResult = da.SelectRecords("SP_GetAgentCode");
               // dsResult.Tables[0].Rows.Add("Select");

                ddlAgentCode.Items.Add("All");
                //DropDownList ddl = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable"));

                ddlAgentCode.DataSource = dsResult;
                ddlAgentCode.DataMember = dsResult.Tables[0].TableName;
                ddlAgentCode.DataValueField = dsResult.Tables[0].Columns[1].ColumnName;
                ddlAgentCode.DataTextField = dsResult.Tables[0].Columns[1].ColumnName;
                ddlAgentCode.DataBind();
                ddlAgentCode.SelectedIndex = 0;
            }
            catch (Exception ex)
            { }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValide())
                    return;

                DataSet dsResult = new DataSet();

                string source, dest, flight, fromdate, awbnumber, awbprefix, todate, status, StrAgentCode;
                source = dest = flight = fromdate = todate = awbnumber = StrAgentCode = "";

                source = ddlSource.SelectedItem.Text.Trim() == "All" ? "" : ddlSource.SelectedItem.Text.Trim();
                dest = ddlDest.SelectedItem.Text.Trim() == "All" ? "" : ddlDest.SelectedItem.Text.Trim();
                flight = ddlFlight.Text.Trim() == "DY" ||  ddlFlight.Text.Trim() == ""? "" : ddlFlight.Text.Trim();
                awbnumber = TXTAWBNumber.Text.Trim();
                StrAgentCode = ddlAgentCode.SelectedItem.Text.Trim() == "All" ? "" : ddlAgentCode.SelectedItem.Text.Trim();
                fromdate = txtFromDate.Text;
                todate = txtToDate.Text;
                status = DDLStatus.SelectedItem.Text.Trim();
                awbprefix = TXTAWBPrefix.Text.Trim();



                getdata(source, dest, flight, status, fromdate, todate, awbnumber, StrAgentCode, awbprefix);

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :" + ex.Message;

            }
            finally {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dsExp = null;
            DataTable dt = null;

            try
            {
                if ((DataTable)Session["dsExp"] == null)
                    //if(ds == null)
                    return;

                dsExp = (DataTable)Session["dsExp"];
                //dsExp = ds;
                //dt = (DataTable)dsExp.Tables[0];
                dt = dsExp.Copy();
                //dt = city.GetAllCity();//your datatable 
                string attachment = "attachment; filename=Report.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
            }
            catch (Exception ex)
            { }
            finally
            {
                dsExp = null;
                dt = null;
            }
        }
       
    }
}
