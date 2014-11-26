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
    public partial class FlightCapacityPlanning : System.Web.UI.Page
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
                    LoadAWBStatusDropdown();
                    LoadStationDropDowns();
                    //if (Session["RoleName"].ToString() != "Super User")
                    //{
                    //    ddlSource.Enabled = false;
                    //    ddlSource.SelectedIndex = ddlSource.Items.IndexOf(ddlSource.Items.FindByText(Session["Station"].ToString()));
                    //}
                    //else
                    //{
                    //    ddlSource.Enabled = true;
                    //}
                    if (Session["awbPrefix"] != null)
                        TXTAWBPrefix.Text = Session["awbPrefix"].ToString();
                    else
                    {
                        MasterBAL objBal = new MasterBAL();
                        Session["awbPrefix"] = objBal.awbPrefix();
                        TXTAWBPrefix.Text = Session["awbPrefix"].ToString();
                    }
                    GetAgentCode();
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");// ("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");// ("yyyy-MM-dd");
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
                    //dtFrom = Convert.ToDateTime(txtFromDate.Text.Trim());
                    dtFrom = DateTime.ParseExact(txtFromDate.Text.Trim(), "dd/MM/yyyy", null);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "From date is invalid";
                    return false;
                }

                try
                {
                    dtTo = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);

                    //dtTo = Convert.ToDateTime(txtToDate.Text.Trim());

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


        #region Grid View 2 Event
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow row = e.Row;
                string Status = string.Empty;
                // Make sure we aren't in header/footer rows
                if (row.DataItem == null)
                {
                    return;
                }
                Status = ((DataRowView)e.Row.DataItem)["Status"].ToString();
                if (Status.Equals("Confirmed", StringComparison.OrdinalIgnoreCase))
                {
                    ((Button)row.FindControl("btnConfirm")).Enabled = false;
                }
            }
            catch (Exception ex) { }
                
        }
        #endregion


        #region GridView1 Event Handlers
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region Modified Code
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
                string agent = ddlAgentCode.SelectedItem.Text.Trim() == "All" ? "" : ddlAgentCode.SelectedItem.Text.Trim();

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


                BFC.GetAWBLevelDataRev(strSortfl, strSortfldt, myorgDateTime.ToString("yyyy-MM-dd").Trim(), alldates360.Trim(), strSortfl.Trim(), ori, dest, agent, ref ds, ref errormessage, TXTAWBPrefix.Text.Trim(), TXTAWBNumber.Text.Trim());
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

            }
            catch (Exception ex)
            {
                dt1 = null;
                ds = null;
                lblStatus.Text = ex.Message;
            }
            finally 
            {
                if (dt1 != null) 
                {
                    dt1.Dispose();
                }
                if (ds != null)
                    ds.Dispose();
            }
            #endregion
            #region Original Code
            //try
            //{
            //    GridViewRow row = e.Row;
            //    string strSortfl = string.Empty;
            //    string strSortfldt = string.Empty;

            //    if (row.DataItem == null)
            //    {
            //        return;
            //    }

            //    DataTable dt1 = new DataTable();
            //    GridView gv = new GridView();
            //    gv = (GridView)row.FindControl("GridView2");

            //    if (gv.UniqueID == gvUniqueID)
            //    {
            //        gv.PageIndex = gvNewPageIndex;
            //        gv.EditIndex = gvEditIndex;
            //        ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["FlightNo"].ToString() + "," + ((DataRowView)e.Row.DataItem)["FlightDate"].ToString() + "," + ((DataRowView)e.Row.DataItem)["DeptTime"].ToString() + "," + ((DataRowView)e.Row.DataItem)["ArrTime"].ToString() + "','one');</script>");
            //    }
            //    strSortfl = ((DataRowView)e.Row.DataItem)["FlightNo"].ToString();
            //    strSortfldt = ((DataRowView)e.Row.DataItem)["FlightDate"].ToString();
            //    string ori = ((DataRowView)e.Row.DataItem)["Origin"].ToString();
            //    string dest = ((DataRowView)e.Row.DataItem)["Destination"].ToString();

            //    sc = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString);
            //    sc.Open();
            //    scmd = new SqlCommand("spgetInternalDataofCapacityV1", sc); 
            //    scmd.CommandType = CommandType.StoredProcedure;
            //    scmd.Parameters.AddWithValue("@FlightNo", strSortfl);
            //    scmd.Parameters.AddWithValue("@FlightDt", strSortfldt);
            //    scmd.Parameters.AddWithValue("@Origin", ori);
            //    scmd.Parameters.AddWithValue("@Destination", dest);
            //    if(ddlAgentCode.Text.Equals("All",StringComparison.OrdinalIgnoreCase))
            //    scmd.Parameters.AddWithValue("@AgentCode","" );
            //   else
            //    scmd.Parameters.AddWithValue("@AgentCode",ddlAgentCode.Text );

            //    // scmd.Parameters.AddWithValue("@prefix", TXTAWBPrefix.Text.Trim());
            //   // scmd.Parameters.AddWithValue("@AWB", TXTAWBNumber.Text.Trim());


            //    sdr = scmd.ExecuteReader();
            //    dt1.Load(sdr);
            //    gv.DataSource = dt1;
            //    gv.DataBind();
            //    sc.Close();
            //}
            //catch (Exception ex)
            //{
            //    lblStatus.Text = ex.Message;
            //}
            #endregion
        }
        #endregion
        public void editMode(object sender, EventArgs e)
        {

            try
            {
                string preAWB = string.Empty;
                string awbno = string.Empty;

                string awb = hdn.Value;
                if (awb.Length > 8 && awb.Contains("-"))
                {
                    string[] split = awb.Split('-');
                    if (split.Length > 1) 
                    {
                        preAWB = split[0];
                        awbno = split[1];
                    }
                }
               
                string query = "'GHA_ConBooking.aspx?command=Edit&AWBNumber=" + awbno + "&AWBPrefix=" + preAWB + "'";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open(" + query + ",'_blank');", true);
                
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "newwindow.focus();", true);
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }
        private void getdata(string source, string dest, string flight, string status, string fromdate, string todate, 
            string awbnumber, string StrAgentCode,string prefix,bool ShowNilFlights)
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
                        GridView1.DataSource = ds.Tables[0];
                        GridView1.DataBind();
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
            //GetFlights();
        }

        protected void ddlDest_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetFlights();
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
                dsResult = null;
                lblStatus.Text = ex.Message;
            }
            finally 
            {
                if (dsResult != null)
                    dsResult.Dispose();
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
            DataSet dsResult = new DataSet();
            try
            {   //string errormessage = "";

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
                flight = txtFlightCode.Text + txtFlightNo.Text;                
                awbnumber = TXTAWBNumber.Text.Trim();
                StrAgentCode = ddlAgentCode.SelectedItem.Text.Trim() == "All" ? "" : ddlAgentCode.SelectedItem.Text.Trim();
                fromdate = txtFromDate.Text;
                todate = txtToDate.Text;
                status = DDLStatus.SelectedValue.ToString();
                awbprefix = TXTAWBPrefix.Text.Trim();
                if (status.Equals("ALL", StringComparison.OrdinalIgnoreCase) || status.Equals("A", StringComparison.OrdinalIgnoreCase)) 
                {
                    status = "";
                }


                getdata(source, dest, flight, status, fromdate, todate, awbnumber, StrAgentCode, awbprefix,false);

            }
            catch (Exception ex)
            {
               
                lblStatus.Text = "Error :" + ex.Message;

            }
            finally {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>callclose();</script>", false);
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

                string AWBNumber="",AWBPrefix="";
                 string str   = ((Label)grRow.FindControl("lblAWBno")).Text.Trim();

                 if (str.Length > 8 && str.Contains("-")) 
                 {
                     string[] split = str.Split('-');
                     if (split.Length > 0) 
                     {
                         AWBPrefix = split[0];
                         AWBNumber = split[1];
                     }
                 }
                    
                //flight No & date Need to pass
                objBal.ConfirmAWBNumber(AWBPrefix,AWBNumber, FlightNo, FlightDate, Convert.ToString(Session["UserName"]), Convert.ToDateTime(Session["IT"]).ToString("MM/dd/yyyy HH:mm"));
                btnList_Click(null, null);
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e) 
        {
            try 
            {
                Response.Redirect("FlightCapacityPlanning.aspx",false);
            }
            catch (Exception ex) { }
        }

        #region LoadAWBStatusDropdown
        public void LoadAWBStatusDropdown()
        {
            DataTable dt = null;
            try
            {
                dt = (DataTable)Session["StatusMaster"];

                if (dt != null)
                {
                    DDLStatus.DataSource = dt;
                    DDLStatus.DataMember = dt.TableName;
                    DDLStatus.DataTextField = "Status";
                    DDLStatus.DataValueField = "StatusCode";
                    DDLStatus.DataBind();
                }
                DDLStatus.Items.Add(new ListItem("ALL", "A"));
                DDLStatus.Text = "ALL";
                DDLStatus.Text = "A";
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally 
            {
                if (dt != null)
                    dt.Dispose();
            }
        }
        #endregion
    }
}
