using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QID.DataAccess;
using System.Data;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class frmAgentQuoteListNew : System.Web.UI.Page
    {

        BAL.BookingBAL objBkgBL = new BAL.BookingBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDestination();
                LoadGridUserList();
                
                if (Convert.ToString(Session["AgentCode"]) != "")
                {
                    TXTAgentCode.Text = Convert.ToString(Session["AgentCode"]);
                    TXTAgentCode.ReadOnly = true;
                    IBOrigin.Visible = false;
                    btnApprove.Visible = false;
                }

            }
        }

        #region loadList
        private bool loadList()
        {
            bool flag = false;
            try
            {
                SQLServer objSQL = new SQLServer(BAL.Global.GetConnectionString());
                string procedure = "spGetAgentQuoteList";

                string[] paramname = new string[] {"AgentCode",
                                                   "Origin",
                                                   "FromDate",
                                                   "ToDate" };

                object[] paramvalue = new object[] {TXTAgentCode.Text.Trim(),
                                                    ddlSource.SelectedValue.ToString().Trim(),
                                                    System.DateTime.Now,
                                                    System.DateTime.Now};

                SqlDbType[] paramtype = new SqlDbType[] { SqlDbType.VarChar,
                                                          SqlDbType.VarChar,
                                                          SqlDbType.DateTime,
                                                          SqlDbType.DateTime };

                DataSet dsData = new DataSet();
                dsData = objSQL.SelectRecords(procedure, paramname, paramvalue, paramtype);
                if (dsData != null)
                {
                    if (dsData.Tables.Count > 0)
                    {
                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            gdvQuoteList.DataSource = null;
                            gdvQuoteList.DataSource = dsData.Tables[0];
                            gdvQuoteList.DataBind();
                            gdvQuoteList.Columns[11].Visible = true;
                            Session["dsdata"] = dsData.Tables[0];
                            lblStatus.Text = string.Empty;
                            flag = true;
                        }
                        else
                        {
                            lblStatus.Text = "No data found for given criteria";
                            lblStatus.ForeColor = Color.Red;
                            ddlSource.Focus();
                            Session["dsdata"] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return flag;
        }
        #endregion

        #region Load Destination
        /// <summary>
        /// Lodas location list in Origin and Destination dropdowns.
        /// </summary>
        private void LoadDestination()
        {
            try
            {
                SQLServer da = new SQLServer(Global.GetConnectionString());
                DataSet ds = da.SelectRecords("spGetAirportCodes");
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddlSource.DataSource = ds;
                            ddlSource.DataMember = ds.Tables[0].TableName;
                            ddlSource.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlSource.DataTextField = ds.Tables[0].Columns["Airport"].ColumnName;
                            ddlSource.DataBind();
                            ddlSource.Items.Insert(0, new ListItem("Select", "Select"));


                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Load Location Dropdown

        #region btnList_Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                //if (TXTAgentCode.Text.Trim() == "")
                //{
                //    lblStatus.Text = "Please enter Agent Code";
                //    lblStatus.ForeColor = Color.Red;
                //}
                //else if (txtFromDate.Text.Trim() == "")
                //{
                //    lblStatus.Text = "Please select From Date";
                //    lblStatus.ForeColor = Color.Red;
                //}
                //else if (txtToDate.Text.Trim() == "")
                //{
                //    lblStatus.Text = "Please select From Date";
                //    lblStatus.ForeColor = Color.Red;
                //}
                //else if (ddlSource.SelectedIndex <= 0)
                //{
                //    lblStatus.Text = "Please Select Source";
                //    lblStatus.ForeColor = Color.Red;
                //}
                //else
                //{
                    loadList();
                //}
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region btnBooking_Click
        protected void btnBooking_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            try
            {
                foreach (GridViewRow grv in gdvQuoteList.Rows)
                {
                    //Find selected row.
                    if (((RadioButton)grv.FindControl("rbSelect")).Checked)
                    {
                        string quoteOrg = ((Label)grv.FindControl("lblOrigin")).Text.Trim();
                        if (Session["Station"].ToString() != quoteOrg)
                        {
                            lblStatus.Text = "Booking for Origin other than your Station is not Allowd";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                        //Redirect to Booking with agent code, pcs & weight information.
                        string QuoteID = ((Label)grv.FindControl("lblQuoteID")).Text;
                        string link = "~/GHA_ConBooking.aspx?command=Quote&ID="+ QuoteID ;
                        //Server.Transfer("~/ConBooking.aspx?command=Quote&ID=" + ((Label)grv.FindControl("lblQuoteID")).Text);
                        //Server.Transfer(link, true);
                        Response.Redirect(link, false);
                        return;
                    }
                }
                lblStatus.Text = "Please select Quote to Book";
                lblStatus.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Add New Row to Grid
        public void LoadGridUserList()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AgentCode";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AgentName";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FlightNo";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Origin";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FlightDate";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Dest";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FreightWeight";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "FreightRate";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "SrNo";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AgentQuoteID";
            myDataTable.Columns.Add(myDataColumn);

            //myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.String");
            //myDataColumn.ColumnName = "IsAllStn";
            //myDataTable.Columns.Add(myDataColumn);


            DataRow dr;
            dr = myDataTable.NewRow();
            dr["SrNo"] = "";
            dr["AgentCode"] = "";
            dr["AgentName"] = "";
            dr["FlightNo"] = "";
            dr["FlightDate"] = ""; 
            dr["Origin"] = ""; 
            dr["Dest"] = "";           
            dr["FreightWeight"] = "";
            dr["FreightRate"] = "";
            dr["AgentQuoteID"] = "";


            myDataTable.Rows.Add(dr);

            gdvQuoteList.DataSource = null;
            gdvQuoteList.DataSource = myDataTable;
            gdvQuoteList.DataBind();
            gdvQuoteList.Columns[11].Visible = false;
            Session["dsdata"] = myDataTable.Copy();

        }
        #endregion


        protected void btnClear_Click(object sender, EventArgs e)
        {
           
                ddlSource.SelectedIndex = 0;
                TXTAgentCode.Text = string.Empty;
                gdvQuoteList.DataSource = null;
                gdvQuoteList.DataBind();
                LoadGridUserList();
            
        }


        protected void btnApprove_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            try
            {
                //int cnt = 0;

                //for (int i = 0; i < gdvQuoteList.Rows.Count; i++)
                //{
                //    if (((CheckBox)gdvQuoteList.Rows[i].FindControl("chkQ")).Checked == true)
                //    {
                //        cnt = cnt + 1;
                //    }
                //}
                //if (cnt > 1 || cnt == 0)
                //{
                //    lblStatus.Text = "Please select only one record.";
                //    lblStatus.ForeColor = System.Drawing.Color.Red;
                //    return;
                //}
                string srno = "";
                for (int i = 0; i < gdvQuoteList.Rows.Count; i++)
                {
                    if (((CheckBox)gdvQuoteList.Rows[i].FindControl("chkQ")).Checked == true)
                    {
                        srno = ((Label)gdvQuoteList.Rows[i].FindControl("lblQuoteID")).Text;
                    
                
                
                string[] paramname = new string[1];
                paramname[0] = "SerialNumber";
               
                object[] paramvalue = new object[1];
                paramvalue[0] = srno;

                
                SqlDbType[] paramtype = new SqlDbType[1];
                paramtype[0] = SqlDbType.VarChar;
                

                SQLServer sq = new SQLServer(Global.GetConnectionString());
                ds = sq.SelectRecords("SP_ApproveAgentQuote", paramname, paramvalue, paramtype);

                   
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0][0].ToString() == "TRUE")
                            {
                               // btnList_Click(null, null);
                                lblStatus.Text = "Agent Quote Approved.";
                                lblStatus.ForeColor = System.Drawing.Color.Green;
                            }
                            else
                            {
                                lblStatus.Text = "Agent Quote not Approved.";
                                lblStatus.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }
                }
                    }
                }
                    
                

            }
            catch (Exception ex)
            { }
        }
        protected void gdvQuoteList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string AgentQID = ((Label)gdvQuoteList.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("lblQuoteID")).Text;
            if (e.CommandName == "Edit")
            {
               
                Response.Redirect("~//frmReverseBidding.aspx?command=" + e.CommandName + "&AgentQuoteID=" +AgentQID);

            }

        }
        protected void gdvQuoteList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dsResult = (DataTable)Session["dsdata"];

            gdvQuoteList.PageIndex = e.NewPageIndex;
            gdvQuoteList.DataSource = dsResult.Copy();
            gdvQuoteList.DataBind();

        }

    }
}

