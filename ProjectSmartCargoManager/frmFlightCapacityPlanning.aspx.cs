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
    public partial class frmFlightCapacityPlanning : System.Web.UI.Page
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
        BALFlightCapacity BFC = new BALFlightCapacity();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                if (!IsPostBack)
                {
                    dispgrid.Visible = false;
                    Session["ExtraInf"] = null;
                    LoadStationDropDowns();
                    //GetAgentCode();
                    ddlSource.SelectedIndex = ddlSource.Items.IndexOf(ddlSource.Items.FindByText(Session["Station"].ToString()));
                   
                    string strQueryString = Convert.ToString(Request.QueryString["Flag"]);
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    if (strQueryString == "H")
                    {
                        txtFromDate.Text = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy");
                        txtToDate.Text = ((DateTime)Session["IT"]).ToString("dd/MM/yyyy");
                        btnList_Click(null, null);
                    }
                    #region page configuration
                    GridView1.AllowPaging = true;
                    try
                    {
                        

                        string AllowPaging = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "PagingInCapacityPlanning");
                        if (AllowPaging != null &&  AllowPaging != "")
                        {
                            GridView1.AllowPaging = Convert.ToBoolean(AllowPaging);
                        }
                        
                    }
                    catch(Exception){}
                    #endregion

                }
                //if (Session["awbPrefix"] != null)
                //    TXTAWBPrefix.Text = Session["awbPrefix"].ToString();
                //else
                //{
                //    MasterBAL objBal = new MasterBAL();
                //    Session["awbPrefix"] = objBal.awbPrefix();
                //    TXTAWBPrefix.Text = Session["awbPrefix"].ToString();
                //}
                if (Request.QueryString["command"].Equals("Capacity", StringComparison.OrdinalIgnoreCase))
                {
                    lblHeader.Text = "Capacity Planning";
                }
                if (Request.QueryString["command"].Equals("ConfirmBooking", StringComparison.OrdinalIgnoreCase))
                {
                    lblHeader.Text = "Confirm Booking";
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
                    dtFrom = DateTime.ParseExact(txtFromDate.Text.Trim(),"dd/MM/yyyy",null);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "From date is invalid";
                    return false;
                }

                try
                {
                    dtTo = DateTime.ParseExact(txtToDate.Text.Trim(),"dd/MM/yyyy",null);

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
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            try
            {
                GridViewRow row = e.Row;
                string strSortfl = string.Empty;
                string strSortfldt = string.Empty;


                // Make sure we aren't in header/footer rows
                if (row.DataItem == null)
                {
                    return;
                }


                GridView gv = new GridView();
                gv = (GridView)row.FindControl("GridView2");
                Table tb = new Table();
                tb = (Table)row.FindControl("tblLYP");
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
                string agent = txtAgentCode.Text.Trim() == "All" ? "" : txtAgentCode.Text.Trim();

                //string day = strSortfldt.Substring(0, 2);   by mohan use when publishing
                //string mon = strSortfldt.Substring(3, 2);
                //string yr = strSortfldt.Substring(6, 4);
                //strSortfldt = yr + "-" + mon + "-" + day;

                //DateTime myorgDateTime = Convert.ToDateTime(strSortfldt);

                // DateTime myorgDateTime = Convert.ToDateTime(strSortfldt);
                DateTime myorgDateTime = DateTime.ParseExact(strSortfldt, "dd/MM/yyyy", null);

                DateTime myDateTime = DateTime.ParseExact(strSortfldt, "dd/MM/yyyy", null);

                //        DateTime myDateTime = Convert.ToDateTime(strSortfldt);
                DateTime myDateTime360 = myDateTime.AddYears(-1);
                string alldates360 = "'" + myDateTime.ToString("yyyy-MM-dd") + "'";
                int a360 = 1;
                while (myDateTime.AddDays(-7) >= myDateTime360)
                {
                    if (myDateTime.AddDays(-7) >= myDateTime360)
                    {
                        a360 = a360 + 1;
                        alldates360 = alldates360 + "'" + myDateTime.AddDays(-7).ToString("yyyy-MM-dd") + "'";
                    }
                    myDateTime = myDateTime.AddDays(-7);
                }
                alldates360 = alldates360.Replace("''", "','");

                BFC.GetAWBLevelDataRev(strSortfl, strSortfldt, myorgDateTime.ToString("yyyy-MM-dd").Trim(), alldates360.Trim(), strSortfl.Trim(), ori, dest, agent, ref ds, ref errormessage, "", "");
                if (ds != null || ds.Tables.Count > 0)
                {
                    //if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            Session["ExtraInf"] = ds.Tables[2];
                        }
                        else
                        {
                            Session["ExtraInf"] = ds.Tables[3];
                        }

                        gv.DataSource = ds.Tables[0];
                        gv.DataBind();
                        //       tb.Rows[3].Cells[0].Text = ds.Tables[1].Rows[0][0].ToString();

                        ((Label)tb.FindControl("lbl1dL")).Text = ds.Tables[1].Rows[0][0].ToString();
                        ((Label)tb.FindControl("lbl1dYa")).Text = ds.Tables[1].Rows[0][1].ToString();
                        ((Label)tb.FindControl("lbl52dL")).Text = ds.Tables[1].Rows[0][2].ToString();
                        ((Label)tb.FindControl("lbl52dY")).Text = ds.Tables[1].Rows[0][3].ToString();
                        ((Label)tb.FindControl("lbl30dL")).Text = ds.Tables[1].Rows[0][4].ToString();
                        ((Label)tb.FindControl("lbl30dY")).Text = ds.Tables[1].Rows[0][5].ToString();
                        ((Label)tb.FindControl("lbl365dL")).Text = ds.Tables[1].Rows[0][6].ToString();
                        ((Label)tb.FindControl("lbl365dY")).Text = ds.Tables[1].Rows[0][7].ToString();
                        ((Label)tb.FindControl("lbl1dP")).Text = ds.Tables[1].Rows[0][8].ToString();
                        ((Label)tb.FindControl("lbl52dP")).Text = ds.Tables[1].Rows[0][9].ToString();
                        ((Label)tb.FindControl("lbl30dP")).Text = ds.Tables[1].Rows[0][10].ToString();
                        ((Label)tb.FindControl("lbl365dP")).Text = ds.Tables[1].Rows[0][11].ToString();

                    }
                }

                //sc = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);
                //sc.Open();
                //scmd = new SqlCommand("spgetInternalDataofCapacity", sc); //receipt come from xpress bag details
                //scmd.CommandType = CommandType.StoredProcedure;
                //scmd.Parameters.AddWithValue("@FlightNo", strSortfl);
                //scmd.Parameters.AddWithValue("@FlightDt", strSortfldt);

                //sdr = scmd.ExecuteReader();
                //dt1.Load(sdr);
                //gv.DataSource = dt1;
                //gv.DataBind();
                //sc.Close();
            }
            catch (Exception ex)
            {
                ds = null;
                dt1 = null;
                lblStatus.Text = ex.Message;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
                if (dt1 != null)
                    dt1.Dispose();
            }
        }
        #endregion

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            btnList_Click(sender, e);
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                //if (e.Row.Cells[10].Text.Equals("Confirmed", StringComparison.OrdinalIgnoreCase))
                if (((Label)e.Row.Cells[10].FindControl("lblStatus")).Text.Equals("Confirmed", StringComparison.OrdinalIgnoreCase))
                {
                    ((Button)e.Row.FindControl("btnConfirm")).Visible = false;

                }
            }
            catch (Exception ex) { }
            

            try 
            {
                if (!Request.QueryString["command"].Equals("Revenue", StringComparison.OrdinalIgnoreCase))
                {
                    e.Row.Cells[11].Visible = false;
                    e.Row.Cells[12].Visible = false;
                    e.Row.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                    e.Row.Cells[15].Visible = false;
                    e.Row.Cells[17].Visible = false;
                
                }
            }
            catch (Exception ex) { }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {   
                DataRowView dr = (DataRowView)e.Row.DataItem;
                int menuId = Convert.ToInt32(dr["IdentRow"]);
                GridView secondGrid = (GridView)e.Row.FindControl("GridView3");
              // string a =e.Row.Parent.Controls.ToString();
              // GridViewRow Row = ((GridViewRow)((Control)sender).Parent.Parent);
              

               //if( ((DataTable)Session["ExtraInf"]).Rows.Count==0)
               // {
               //     ((Button)e.Row.Parent.FindControl("btnviability")).Visible = false;
               //}
                DataRow[] result;
                result = ((DataTable)Session["ExtraInf"]).Select("IdentRow = '" + menuId + "'");
                DataTable dtForSecondGrid = new DataTable(); //here you populate this table with values by passing the key 'menuid'
                dtForSecondGrid = ((DataTable)Session["ExtraInf"]).Clone();
                foreach (DataRow row in result)
                {
                    dtForSecondGrid.Rows.Add(row.ItemArray);
                }
                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ToString());
                //string str = "Select * from MenuDetails where MenuId=" + menuId;
               // SqlDataAdapter da = new SqlDataAdapter(str, con);
                //da.Fill(dtForSecondGrid);
                dtForSecondGrid.Columns.RemoveAt(3);
                secondGrid.DataSource = dtForSecondGrid;
                secondGrid.DataBind();

                if (dtForSecondGrid != null)
                    dtForSecondGrid.Dispose();
            }
        }


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
               // Response.Redirect("~//ConBooking.aspx?command=Edit&AWBNumber=" + awb,false);
                string query = "'GHA_ConBooking.aspx?command=Edit&AWBNumber=" +awb + "'";
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
        private void getdata(string source, string dest, string flight, string status, string fromdate, string todate, 
            string awbnumber, string StrAgentCode,string prefix, bool ShowNilFlights)
        {
            DataSet ds = new DataSet();
            try
            {
                BFC.GetAllAWBs(source, dest, flight, fromdate, todate, prefix, status, awbnumber, ref ds, ref errormessage,
                    StrAgentCode, ShowNilFlights);
                if (ds != null || ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dispgrid.Visible = true;
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
                        dispgrid.Visible = false;
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
                ds = null;
                lblStatus.Text = ex.Message;
            }
            finally 
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        protected void ddlSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetFlights();
        }

        protected void ddlDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetFlights();
        }

        #region UserDefined

        public void LoadStationDropDowns()
        {
            DataSet dsResult = new DataSet();
            try
            {
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

                    ddlFlight.Items.Clear();
                    ddlFlight.Items.Add(new ListItem("All", "All"));

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
                dsResult = null;
                lblStatus.Text = ex.Message;
            }
            finally 
            {
                if (dsResult != null)
                    dsResult.Dispose();
            }
        }

        public void GetFlights()
        {

            DataSet dsResult = new DataSet();

            try
            {
                string source, dest, date;
                source = dest = date = "";

                if (ddlSource.SelectedItem.Text.Trim() == "All" || ddlDest.SelectedItem.Text.Trim() == "All")
                {
                    ddlFlight.Items.Clear();
                    ddlFlight.Items.Add(new ListItem("All", "All"));
                    ddlFlight.SelectedIndex = 0;
                }
                else
                {
                    source = ddlSource.SelectedItem.Text;
                    dest = ddlDest.SelectedItem.Text;
                    date = "";// txtDate.Text;


                    if (objBAL.GetAllFlights(source, dest, date, ref dsResult, ref errormessage))
                    {

                        DataRow row = dsResult.Tables[0].NewRow();
                        row["FltNumber"] = "All";
                        dsResult.Tables[0].Rows.Add(row);

                        ddlFlight.DataSource = dsResult.Tables[0].Copy();
                        ddlFlight.DataTextField = "FltNumber";
                        ddlFlight.DataValueField = "FltNumber";
                        ddlFlight.DataBind();
                        ddlFlight.SelectedItem.Text = "All";
                    }
                    else
                    {
                        lblStatus.Text = "" + errormessage;
                    }
                }

            }
            catch (Exception ex)
            {
                dsResult = null;
                lblStatus.Text = "" + ex.Message;
            }
            finally 
            {
                if (dsResult != null)
                    dsResult.Dispose();
            }
        }

        #endregion
        public void GetAgentCode()
        {
            DataSet dsResult = new DataSet();
            try
            {
                dsResult = da.SelectRecords("SP_GetAgentCode");
                //dsResult.Tables[0].Rows.Add("All");

                //DropDownList ddl = ((DropDownList)GrdEmbargoDetails.Rows[i].FindControl("ddlApplicable"));
                //ddlAgentCode.Items.Add("All");
                //ddlAgentCode.DataSource = dsResult;
                //ddlAgentCode.DataMember = dsResult.Tables[0].TableName;
                //ddlAgentCode.DataValueField = dsResult.Tables[0].Columns[1].ColumnName;
                //ddlAgentCode.DataTextField = dsResult.Tables[0].Columns[1].ColumnName;
                //ddlAgentCode.DataBind();
                //ddlAgentCode.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                dsResult = null;
            }
            finally 
            {
                if (dsResult != null)
                    dsResult.Dispose();
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (!IsValide())
                    return;

                string source, dest, flight, fromdate, awbnumber, awbprefix, todate, status, StrAgentCode;
                source = dest = flight = fromdate = todate = awbnumber = StrAgentCode = "";

                source = ddlSource.SelectedItem.Text.Trim() == "All" ? "" : ddlSource.SelectedItem.Text.Trim();
                dest = ddlDest.SelectedItem.Text.Trim() == "All" ? "" : ddlDest.SelectedItem.Text.Trim();
                flight = ddlFlight.SelectedItem.Text.Trim() == "All" ? "" : ddlFlight.SelectedItem.Text.Trim();
                awbnumber = TXTAWBNumber.Text.Trim();

                fromdate = txtFromDate.Text;
                todate = txtToDate.Text;
                status = DDLStatus.SelectedItem.Text.Trim().Equals("All",StringComparison.OrdinalIgnoreCase) ? "" : DDLStatus.SelectedItem.Text.Trim();
                awbprefix = TXTAWBPrefix.Text.Trim();
                StrAgentCode = txtAgentCode.Text.Trim() == "All" ? "" : txtAgentCode.Text.Trim();

                getdata(source, dest, flight, status, fromdate, todate, awbnumber, StrAgentCode,awbprefix,chkShowNilFlt.Checked);

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error :" + ex.Message;

            }
        }

        protected void ConfirmShipment(object sender, EventArgs e)
        {
            try
            {
                BALFlightCapacity objBal = new BALFlightCapacity();
                int rowindex = 0;
                Button TextBox = (Button)sender;
                GridViewRow grRow = (GridViewRow)TextBox.NamingContainer;
                rowindex = grRow.RowIndex;
                string FlightNo = hdnFliNo.Value;
                string FlightDate = hdnFliDt.Value;

                string AWBNumber = ((Label)grRow.FindControl("lblAWBno")).Text.Trim();

                if (AWBNumber.Length > 8)
                    AWBNumber = AWBNumber.Substring(AWBNumber.Length - 8, 8);
                
                //flight No & date Need to pass
                objBal.ConfirmAWBNumber(AWBNumber, FlightNo, FlightDate, Convert.ToString(Session["UserName"]), Convert.ToDateTime(Session["IT"]).ToString("MM/dd/yyyy HH:mm"));
                btnList_Click(null, null);
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (lblHeader.Text == "Capacity Planning")
            {
                Response.Redirect("~/frmFlightCapacityPlanning.aspx?command=Capacity", false);
                return;
            }
            if (lblHeader.Text == "Revenue")
            {
                Response.Redirect("~/frmFlightCapacityPlanning.aspx?command=Revenue", false);
                return;

            }
            if (lblHeader.Text == "Confirm Booking")
            {
                Response.Redirect("~/frmFlightCapacityPlanning.aspx?command=ConfirmBooking", false);
                return;

            }

        }
    }
}
