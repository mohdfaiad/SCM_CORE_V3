using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using BAL;

using System.IO;
using System.Drawing;
using System.Data.SqlClient;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class FlightBudget : System.Web.UI.Page
    {
        string gvUniqueID = String.Empty;
        int gvNewPageIndex = 0;
        int gvEditIndex = -1;
        BALException objBAL = new BALException();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataSet dsbudget = new DataSet();
        BookingBAL objBLL = new BookingBAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    lblStatus.Text = "";
                    GetCountry();
                    GetOrigin();
                    GetDestination();
                    GetPOS();
                    ////rptViewerFlightBudget.Visible = false;
                    FillCurrencyCodes();
                    LoadRegion();
                 LoadFlightPrefix();
                 GetFlights();
                }
                catch (Exception)
                {
                }
            }
        }
        # region Get Origin List

        private void GetOrigin()
        {
            DataSet ds = null;
            try
            {
                ds = objBAL.GetOriginCodeList(ddlOrigin.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = "AirPortCode";//ds.Tables[2].Columns["Code"].ColumnName;

                            ddlOrigin.DataTextField = "AirPort"; //ds.Tables[2].Columns["Code"].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        # endregion GetOriginCode List
        //private void LoadCurrencyType()
        //{
        //    DataSet ds = null;
        //    try
        //    {
        //        ds = objBAL.GetCurrencyCodeList(ddlCurrency.SelectedValue);
        //        if (ds != null)
        //        {
        //            if (ds.Tables != null)
        //            {
        //                if (ds.Tables.Count > 0)
        //                {
        //                    ddlCurrency.DataSource = ds;

        //                    //ddlOrigin.DataSource = ds;
        //                    ddlCurrency.DataMember = ds.Tables[0].TableName;
        //                    ddlCurrency.DataValueField = ds.Tables[2].Columns["CurrencyCode"].ColumnName;

        //                    ddlCurrency.DataTextField = ds.Tables[2].Columns["CurrencyCode"].ColumnName;
        //                    ddlCurrency.DataBind();
        //                    ddlCurrency.Items.Insert(0, "Select");

        //                }

        //            }

        //        }


        //    }
        //    catch (Exception ex)
        //    { }
        //    finally
        //    {
        //        if (ds != null)
        //            ds.Dispose();
        //    }


        //}

        private void LoadFlightPrefix()
        {
            try
            {
                DataSet ds = objBLL.GetFlightPrefixList();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlFlightPrefix.Items.Clear();
                            //ddlComodityCd.Items.Add("Select");
                            ddlFlightPrefix.DataSource = ds.Tables[0];
                            //ddlComodityCd.DataValueField = ds.Tables[0].Columns[0].ColumnName;
                            //ddlComodityCd.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                            ddlFlightPrefix.DataTextField = "PartnerCode";
                            ddlFlightPrefix.DataValueField = "PartnerCode";
                            ddlFlightPrefix.DataBind();
                            //ddlComodityCd.SelectedIndex = 0;
                            ddlFlightPrefix.Items.Insert(0, "SELECT");
                            // ddlComodityCd.SelectedValue = "GEN";
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        
        }
        private void  GetCountry()
        {
            DataSet ds = null;
            try
            {
                ds = objBAL.GetCountryCodeList(ddlCountry.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            //ds.Tables[0].Rows.Add("", "");
                            ddlCountry.DataSource = ds;
                            ddlCountry.DataMember = ds.Tables[0].TableName;
                            ddlCountry.DataValueField = "CountryCode";
                            ddlCountry.DataTextField = "Country";
                            ddlCountry.DataBind();
                            ddlCountry.Items.Insert(0, "Select");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }




        }

        # region Get POS List

        private void GetPOS()
        {
            DataSet ds = null;
            try
            {
                ds = objBAL.GetOriginCodeList(ddlPOS.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlPOS.DataSource = ds;
                            ddlPOS.DataMember = ds.Tables[0].TableName;
                            ddlPOS.DataValueField = "AirPortCode";//ds.Tables[2].Columns["Code"].ColumnName;

                            ddlPOS.DataTextField = "AirPort";// ds.Tables[2].Columns["Code"].ColumnName;
                            ddlPOS.DataBind();
                            ddlPOS.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        # endregion GetPOS List

        # region Get Destination List

        private void GetDestination()
        {
            DataSet ds = null;
            try
            {
                ds = objBAL.GetOriginCodeList(ddlDestination.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlDestination.DataSource = ds;
                            ddlDestination.DataMember = ds.Tables[0].TableName;
                            ddlDestination.DataValueField = "AirPortCode"; //ds.Tables[2].Columns["Code"].ColumnName;

                            ddlDestination.DataTextField = "AirPort";//ds.Tables[2].Columns["Code"].ColumnName;
                            ddlDestination.DataBind();
                            ddlDestination.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
        }

        # endregion GetDestinationCode List

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                string  AgentCode = "", Prefix = "", Status = "All", AWBNumber = "",  Year;
               // string FlightNo = txtFlightNo.Text.Trim();
        
                //Origin = ddlOrigin.SelectedItem.Text.Trim() == "All" ? "" : ddlOrigin.SelectedItem.Text.Trim();
                //Dest = ddlDestination.SelectedItem.Text.Trim() == "All" ? "" : ddlDestination.SelectedItem.Text.Trim();
                string Origin = "", Dest = "";
                if (ddlOrigin.SelectedItem.Text.Trim() != "All")
                {
                   Origin = ddlOrigin.SelectedValue.ToString().Trim();
                }
                if (ddlDestination.SelectedItem.Text.Trim() != "All")
                {
                    Dest = ddlDestination.SelectedValue.ToString().Trim();
                }

                string country = "";
                if (ddlCountry.SelectedItem.Text.Trim() != "Select")
                {
                    country = ddlCountry.SelectedValue.ToString().Trim();
                }


                string Region = "";
                if (ddlRegion.SelectedItem.Text.Trim() != "Select")
                {
                    //Region = ddlRegion.SelectedValue.ToString();
                    Region = ddlRegion.SelectedValue.ToString().Trim();
                }

                string POS = "";
                if (ddlPOS.SelectedItem.Text.Trim() != "All")
                {

                    POS = ddlPOS.SelectedValue.ToString().Trim();
                }
                string currency = "";
                if (ddlCurrency.SelectedItem.Text.Trim() != "")
                {
                    currency = ddlCurrency.SelectedValue.ToString().Trim();
                
                }
                string FlightNumber = "";
                    
                if (ddlFlightNumber.SelectedItem.Text.Trim() != "select")
                {
                    FlightNumber = ddlFlightNumber.SelectedValue.ToString().Trim();
                }
                string Month = "";
                if (ddlMonth.SelectedItem.Text.Trim() != "Select")
                {
                    Month = ddlMonth.SelectedValue.ToString().Trim();

                }
               // Month = ddlMonth.SelectedItem.Text.Trim() == "Select" ? "" : ddlMonth.SelectedItem.Text.Trim();
                Year = ddlYear.SelectedItem.Text.Trim() == "Select" ? "" : ddlYear.SelectedItem.Text.Trim();
               // Region = ddlRegion.SelectedItem.Text.Trim() == "Select" ? "" : ddlRegion.SelectedItem.Text.Trim();
               // FlightNumber=ddlFlightNumber.SelectedItem.Text.Trim()=="All" ? "" :ddlFlightNumber.SelectedItem.Text.Trim();
               // POS = ddlPOS.SelectedItem.Text.Trim() == "All" ? "" : ddlPOS.SelectedItem.Text.Trim();
                DateTime fromdate = Convert.ToDateTime("2000-01-01 00:00:00.000");
                DateTime todate = Convert.ToDateTime("2020-01-01 00:00:00.000");

                #region Parametes
                string[] PName = new string[14];
                PName[0] = "Source";
                PName[1] = "Dest";
                PName[2] = "Flight";
                PName[3] = "fromdate";
                PName[4] = "todate";
                PName[5] = "Prefix";
                PName[6] = "Status";
                PName[7] = "AWBNumber";
                PName[8] = "AgentCode";
                PName[9] = "Region";
                PName[10] = "Month";
                PName[11] = "Year";
                PName[12] = "POS";
                PName[13] = "Currency";




                object[] paramvalue = new object[14];
                //object[] ProRateListInfo = new object[2];

                paramvalue[0] = Origin;
                paramvalue[1] = Dest;
                paramvalue[2] = FlightNumber;//txtFlightNo.Text.Trim();
                paramvalue[3] = fromdate;
                paramvalue[4] = todate;
                paramvalue[5] = Prefix;
                paramvalue[6] = Status;
                paramvalue[7] = AWBNumber;
                paramvalue[8] = AgentCode;
                paramvalue[9] = Region;
                paramvalue[10] = Month;//txtFromDate.Text.Trim();
                paramvalue[11] = Year;//txtToDate.Text.Trim();
                paramvalue[12] = POS;
                paramvalue[13] = currency;



                //SqlDbType[] paramtype = new SqlDbType[5];
                SqlDbType[] paramtype = new SqlDbType[14];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.DateTime;
                paramtype[4] = SqlDbType.DateTime;
                paramtype[5] = SqlDbType.VarChar;
                paramtype[6] = SqlDbType.VarChar;
                paramtype[7] = SqlDbType.VarChar;
                paramtype[8] = SqlDbType.VarChar;
                paramtype[9] = SqlDbType.VarChar;
                paramtype[10] = SqlDbType.VarChar;
                paramtype[11] = SqlDbType.VarChar;
                paramtype[12] = SqlDbType.VarChar;
                paramtype[13] = SqlDbType.VarChar;

                #endregion
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;
                //dsbudget = da.SelectRecords("spgetAllMainDataforCapacityPlanningforSummary", PName, paramvalue, paramtype);
                if (Year != "" && Month!="")
                {
                    dsbudget = da.SelectRecords("[spgetAllMainDataforCapacityPlanningforSummary_NewTest1]", PName, paramvalue, paramtype);
                    Session["dsExp"] = dsbudget;
                    if (dsbudget != null || dsbudget.Tables.Count > 0)
                    {
                        GridView1.DataSource = dsbudget.Tables[0];
                        GridView1.DataBind();
                        btnExport.Visible = true;
                    }
                }
                else
                {
                    lblStatus.Text = "Select Appropriate Field";
                    //lblStatus.Text = "Select Year .";

                }

                //dsbudget = da.SelectRecords("[SP_GetBudget]", PName, paramvalue, paramtype);

                //rptViewerFlightBudget.Visible = true;
                //  System.Configuration.ConfigurationManager.AppSettings["ReportPath"] + "AWB.rdlc";
                //info = new FileInfo(Server.MapPath("/Reports/rptbudget1 (2).rdlx"));
                //// info = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["ReportPath"]+ "rptABMovement.rdlx");
                //definition = new ReportDefinition(info);
                //runtime = new ReportRuntime(definition);


                ////  Dataset2 = dschk;

                //runtime.LocateDataSource += WRAXBDetails_LocateDataSource;
                //rptViewerFlightBudget.SetReport(runtime);



            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dsbudget != null)
                    dsbudget.Dispose();
            }

        }

        //private void WRAXBDetails_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{

        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")
        //    e.Data = dsbudget.Tables[0];
        //    //if (dname == "dsbudget")
        //    //{
        //    //    //e.Data = dsbudget;

        //    //}
        //    //else
        //    //{
        //    //    e.Data = dsbudget;
        //    //}
        //}

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;

            try
            {
                lblStatus.Text = "";
                if ((DataSet)Session["dsExp"] == null)
                    //if(ds == null)
                    return;

                dsExp = (DataSet)Session["dsExp"];
                //dsExp = ds;
                dt = (DataTable)dsExp.Tables[0];
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

        protected void btnclear_Click(object sender, EventArgs e)
        {
            ////rptViewerFlightBudget.Visible = false;
            //txtFromDate.Text = ((DateTime)Session["IT"]).ToString("yyyy-MM-dd");
            //txtToDate.Text = ((DateTime)Session["IT"]).ToString("yyyy-MM-dd");
            ddlRegion.SelectedIndex = 0;
            ddlOrigin.SelectedIndex = 0;
            ddlDestination.SelectedIndex = 0;
            ddlMonth.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            ddlPOS.SelectedIndex = 0;
            //txtFlightNo.Text = "";
            btnExport.Visible = false;
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
                    ClientScript.RegisterStartupScript(GetType(), "Expand", "<SCRIPT LANGUAGE='javascript'>expandcollapse('div" + ((DataRowView)e.Row.DataItem)["Region"].ToString() + "," + ((DataRowView)e.Row.DataItem)["Origin"].ToString() + "," + ((DataRowView)e.Row.DataItem)["Destination"].ToString() + "," + ((DataRowView)e.Row.DataItem)["FlightNo"].ToString() + "," + ((DataRowView)e.Row.DataItem)["Month"].ToString() + "," + ((DataRowView)e.Row.DataItem)["Year"].ToString() + "," + ((DataRowView)e.Row.DataItem)["POS"].ToString() + "','one');</script>");
                }
                //strSortfl = ((DataRowView)e.Row.DataItem)["FlightNo"].ToString();
                //strSortfldt = ((DataRowView)e.Row.DataItem)["FlightDate"].ToString();
                //string ori = ((DataRowView)e.Row.DataItem)["Origin"].ToString();
                //string dest = ((DataRowView)e.Row.DataItem)["Destination"].ToString();
                string Origin, Dest, AgentCode = "", Prefix = "", Status = "All", AWBNumber = "", Month, Year, Region, POS;
                Origin = ((DataRowView)e.Row.DataItem)["Origin"].ToString();//ddlOrigin.SelectedItem.Text.Trim() == "All" ? "" : ddlOrigin.SelectedItem.Text.Trim();
                Dest = ((DataRowView)e.Row.DataItem)["Destination"].ToString();//ddlDestination.SelectedItem.Text.Trim() == "All" ? "" : ddlDestination.SelectedItem.Text.Trim();
                Month = ((DataRowView)e.Row.DataItem)["Month"].ToString();//ddlMonth.SelectedItem.Text.Trim() == "Select" ? "" : ddlMonth.SelectedItem.Text.Trim();
                Year = ((DataRowView)e.Row.DataItem)["Year"].ToString();//ddlYear.SelectedItem.Text.Trim() == "Select" ? "" : ddlYear.SelectedItem.Text.Trim();
                Region = ((DataRowView)e.Row.DataItem)["Region"].ToString();//ddlRegion.SelectedItem.Text.Trim() == "Select" ? "" : ddlRegion.SelectedItem.Text.Trim();
                POS = ((DataRowView)e.Row.DataItem)["POS"].ToString();//ddlPOS.SelectedItem.Text.Trim() == "All" ? "" : ddlPOS.SelectedItem.Text.Trim();
                DateTime fromdate = Convert.ToDateTime("2000-01-01 00:00:00.000");
                DateTime todate = Convert.ToDateTime("2020-01-01 00:00:00.000");

                #region Parametes
                string[] PName = new string[14];
                PName[0] = "Source";
                PName[1] = "Dest";
                PName[2] = "Flight";
                PName[3] = "fromdate";
                PName[4] = "todate";
                PName[5] = "Prefix";
                PName[6] = "Status";
                PName[7] = "AWBNumber";
                PName[8] = "AgentCode";
                PName[9] = "Region";
                PName[10] = "Month";
                PName[11] = "Year";
                PName[12] = "POS";
                PName[13] = "Currency";




                object[] paramvalue = new object[14];
                //object[] ProRateListInfo = new object[2];

                paramvalue[0] = Origin;
                paramvalue[1] = Dest;
                paramvalue[2] = ((DataRowView)e.Row.DataItem)["FlightNo"].ToString();//txtFlightNo.Text.Trim();
                paramvalue[3] = fromdate;
                paramvalue[4] = todate;
                paramvalue[5] = Prefix;
                paramvalue[6] = Status;
                paramvalue[7] = AWBNumber;
                paramvalue[8] = AgentCode;
                paramvalue[9] = Region;
                paramvalue[10] = Month;//txtFromDate.Text.Trim();
                paramvalue[11] = Year;//txtToDate.Text.Trim();
                paramvalue[12] = POS;
                paramvalue[13] = ddlCurrency.SelectedItem.Text.Trim();



                //SqlDbType[] paramtype = new SqlDbType[5];
                SqlDbType[] paramtype = new SqlDbType[14];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.DateTime;
                paramtype[4] = SqlDbType.DateTime;
                paramtype[5] = SqlDbType.VarChar;
                paramtype[6] = SqlDbType.VarChar;
                paramtype[7] = SqlDbType.VarChar;
                paramtype[8] = SqlDbType.VarChar;
                paramtype[9] = SqlDbType.VarChar;
                paramtype[10] = SqlDbType.VarChar;
                paramtype[11] = SqlDbType.VarChar;
                paramtype[12] = SqlDbType.VarChar;
                paramtype[13] = SqlDbType.VarChar;

                #endregion

                ////string day = strSortfldt.Substring(0, 2);   by mohan use when publishing
                ////string mon = strSortfldt.Substring(3, 2);
                ////string yr = strSortfldt.Substring(6, 4);
                ////strSortfldt = yr + "-" + mon + "-" + day;

                ////DateTime myorgDateTime = Convert.ToDateTime(strSortfldt);

                //// DateTime myorgDateTime = Convert.ToDateTime(strSortfldt);
                //DateTime myorgDateTime = DateTime.ParseExact(strSortfldt, "dd/MM/yyyy", null);

                //DateTime myDateTime = DateTime.ParseExact(strSortfldt, "dd/MM/yyyy", null);

                ////        DateTime myDateTime = Convert.ToDateTime(strSortfldt);
                //DateTime myDateTime360 = myDateTime.AddYears(-1);
                //string alldates360 = "'" + myDateTime.ToString("yyyy-MM-dd") + "'";
                //int a360 = 1;
                //while (myDateTime.AddDays(-7) >= myDateTime360)
                //{
                //    if (myDateTime.AddDays(-7) >= myDateTime360)
                //    {
                //        a360 = a360 + 1;
                //        alldates360 = alldates360 + "'" + myDateTime.AddDays(-7).ToString("yyyy-MM-dd") + "'";
                //    }
                //    myDateTime = myDateTime.AddDays(-7);
                //}
                //alldates360 = alldates360.Replace("''", "','");
                
                ds = da.SelectRecords("spgetAllMainDataforCapacityPlanningforSummary_NewTest1", PName, paramvalue, paramtype);
                //BFC.GetAWBLevelData(strSortfl, strSortfldt, myorgDateTime.ToString("yyyy-MM-dd").Trim(), alldates360.Trim(), strSortfl.Trim(), ref ds, ref errormessage);
                if (ds != null || ds.Tables.Count > 0)
                {

                    {
                        //Session["ExtraInf"] = ds.Tables[2];

                        gv.DataSource = ds.Tables[1];
                        gv.DataBind();


                        //((Label)tb.FindControl("lbl1dL")).Text = ds.Tables[1].Rows[0][0].ToString();
                        //((Label)tb.FindControl("lbl1dYa")).Text = ds.Tables[1].Rows[0][1].ToString();
                        //((Label)tb.FindControl("lbl52dL")).Text = ds.Tables[1].Rows[0][2].ToString();
                        //((Label)tb.FindControl("lbl52dY")).Text = ds.Tables[1].Rows[0][3].ToString();
                        //((Label)tb.FindControl("lbl30dL")).Text = ds.Tables[1].Rows[0][4].ToString();
                        //((Label)tb.FindControl("lbl30dY")).Text = ds.Tables[1].Rows[0][5].ToString();
                        //((Label)tb.FindControl("lbl365dL")).Text = ds.Tables[1].Rows[0][6].ToString();
                        //((Label)tb.FindControl("lbl365dY")).Text = ds.Tables[1].Rows[0][7].ToString();
                        //((Label)tb.FindControl("lbl1dP")).Text = ds.Tables[1].Rows[0][8].ToString();
                        //((Label)tb.FindControl("lbl52dP")).Text = ds.Tables[1].Rows[0][9].ToString();
                        //((Label)tb.FindControl("lbl30dP")).Text = ds.Tables[1].Rows[0][10].ToString();
                        //((Label)tb.FindControl("lbl365dP")).Text = ds.Tables[1].Rows[0][11].ToString();

                    }
                }


            }
            catch (Exception ex)
            {
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

        #region Fill Currency
        private void FillCurrencyCodes()
        {
            DataSet dsCur = null;
            try
            {
                BALCurrency BalCur = new BALCurrency();
                dsCur = BalCur.GetCurrencyCodeList("");
                if (dsCur != null && dsCur.Tables.Count > 0)
                {
                    if (dsCur.Tables[0].Rows.Count > 0)
                    {
                        ddlCurrency.Items.Clear();

                        ddlCurrency.DataSource = dsCur;
                        ddlCurrency.DataMember=dsCur.Tables[0].TableName;
                        ddlCurrency.DataTextField = "Code";
                        ddlCurrency.DataValueField = "ID";
                        ddlCurrency.DataBind();
                        ddlCurrency.SelectedIndex = -1;
                    }
                    else
                    {
                        ddlCurrency.Items.Clear();
                        ddlCurrency.SelectedIndex = 0;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (dsCur != null)
                    dsCur.Dispose();
            }
        }
        #endregion

        protected void LoadRegion()
        {
            StockAllocationBAL objBAL = new StockAllocationBAL();
            DataSet Region = null;
            try
            {
                Region = objBAL.GetRegionCode();

                ddlRegion.Items.Clear();
                ddlRegion.Items.Add(new ListItem("Select"));

                for (int intCount = 0; intCount < Region.Tables[0].Rows.Count; intCount++)
                {
                    ddlRegion.Items.Add(new ListItem(Region.Tables[0].Rows[intCount][0].ToString()));
                }
                ddlRegion.SelectedIndex = 0;
            }
            catch (Exception ex)
            { }
            finally
            {
                if (Region != null)
                    Region.Dispose();
            }
        }

        protected void ddlFlightPrefix_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FlightPrefixCode = ddlFlightPrefix.SelectedItem.Value.ToString();
            GetFlight(FlightPrefixCode);
        }
        public void GetFlight(string FlightPrefixCode)
        {
            DataSet dsResult = new DataSet();
            try
            {
                if (ddlFlightPrefix.SelectedItem.Value.ToString() == "Select")
                {
                    ddlFlightNumber.DataSource = "";
                    ddlFlightNumber.DataBind();
                }


                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                dsResult = objSQL.SelectRecords("spGetAllFlightListPrefixWise", "FlightPrefix", FlightPrefixCode, SqlDbType.VarChar);
                if (dsResult != null)
                {
                    if (dsResult.Tables.Count > 0)
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            ddlFlightNumber.Items.Clear();
                            ddlFlightNumber.DataSource = dsResult.Tables[0];
                            ddlFlightNumber.DataTextField = "FlightID";
                            ddlFlightNumber.DataValueField = "FlightID";
                            ddlFlightNumber.DataBind();
                            ddlFlightNumber.Items.Insert(0, new ListItem("Select", ""));
                            ddlFlightNumber.SelectedIndex = -1;
                        }

                        else
                        {
                            ddlFlightNumber.Items.Clear();
                            lblStatus.Text = "No Flight for this Partner";
                            lblStatus.ForeColor = Color.Red;
                            ddlFlightNumber.DataSource = null;
                            ddlFlightNumber.DataBind();
                            ddlFlightNumber.Items.Insert(0, new ListItem("Select", null));
                        }
                    }
                }


            }
            catch (Exception ex)
            { }

        }

        public void GetFlights()
        {
            try
            {
                SQLServer objSQL = new SQLServer(Global.GetConnectionString());
                DataSet dsInstance = new DataSet();
                //string FlightPrefix;
                dsInstance = objSQL.SelectRecords("GetCurrentInstance");
                string current = dsInstance.Tables[0].Rows[0][0].ToString();
                //  FlightPrefix = ddlFlightPrefix.SelectedValue.ToString().Trim();
                {
                    DataSet dsResult = new DataSet();
                    string errormessage = "";
                    //if (objBAL.GetAllFlightsNew(source, dest, date, ref dsResult, ref errormessage))
                    string procedure = "sp_GetFlightPrefix";
                    dsResult = objSQL.SelectRecords(procedure);
                    if (dsResult != null)
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                                ddlFlightPrefix.Items.Clear();
                                ddlFlightPrefix.DataSource = dsResult.Tables[0];
                                ddlFlightPrefix.DataTextField = "PartnerCode";
                                ddlFlightPrefix.DataValueField = "PartnerCode";
                                ddlFlightPrefix.DataBind();

                                ddlFlightPrefix.SelectedValue = current;
                                GetFlight(current);

                            }
                        }
                    }
                    else
                    {
                        lblStatus.Text = "" + errormessage;
                        lblStatus.ForeColor = Color.Red;
                    }
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "" + ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
    }
}
