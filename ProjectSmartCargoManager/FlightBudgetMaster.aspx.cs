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
using System.Configuration;
using Excel;

namespace ProjectSmartCargoManager
{
    public partial class FlightBudgetMaster : System.Web.UI.Page
    {
        string gvUniqueID = String.Empty;
        int gvNewPageIndex = 0;
        int gvEditIndex = -1;
        BALException objBAL = new BALException();
       
        SQLServer da = new SQLServer(Global.GetConnectionString());
        DataSet dsbudget = new DataSet();
        BookingBAL objBLL = new BookingBAL();

        string logPath = string.Empty;
        string logfilename = string.Empty;
        DateTime dtCurrentDate = new DateTime();
        string strUserName = "";
        excelUpload obj = new excelUpload();
        string path = ConfigurationManager.AppSettings["PaxLoadPath"].ToString();
        String FileExtension = string.Empty;
        String filePath = string.Empty;
        DataSet dsupload = new DataSet();
        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
      //  SQLServer da = new SQLServer(Global.GetConnectionString().ToString());
        bool flag;
        string uploadsheet = "";
        UploadMastersBAL objUpload = new UploadMastersBAL();
        string strflag = "";
        MasterAuditBAL ObjMAL = new MasterAuditBAL();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    lblStatus.Text = "";
                  //  GetCountry();
                    GetOrigin();
                    GetDestination();
                   // GetPOS();
                    Session["dsBudgetMaster"] = null;
                  //  FillCurrencyCodes();
                    LoadRegion();
                 LoadFlightPrefix();
                 GetFlights();
                 CreateEmptyRow();

                }
                catch (Exception)
                {
                }
            }
        }

        #region Create Empty Row
        public void CreateEmptyRow()
        {

            DataSet DsAddNewRow = null;

            try
            {
                
                #region Add New row to grid
                if (Session["dsBudgetMaster"] != null)
                {
                    DsAddNewRow = ((DataSet)Session["dsBudgetMaster"]).Copy();
                    DataRow row = DsAddNewRow.Tables[0].NewRow();
                    row["Region"] = "";
                    row["Market"] = "";
                    row["Currency"] = "";
                    row["FlightNo"] = "";
                    row["Origin"] = "";
                    row["Destination"] = "";
                    row["Year"] = "";
                    row["Month"] = "";
                    row["AC_Type"] = "";
                    row["FLT_INDIC"] = "";
                    row["IsActive"] = false;
                    row["POS"] = "";
                    row["No_Of_Flights"] = "";
                    row["UoM"] = "";
                    row["Cargo_per_Flight"] = "";
                    row["PO_Mail_per_Flight"] = "";
                    row["Courier_per_Flight"] = "";
                    row["Cargo_Rate"] = "";
                    row["PO_Mail_rate"] = "";
                    row["Courier_Rate"] = "";
                    row["ForeCastBudget"] = "";
                    row["TargetBudget"] = "";
                    row["ID"] = "";

                    DsAddNewRow.Tables[0].Rows.Add(row);

                    Session["dsBudgetMaster"] = DsAddNewRow.Copy();

                   grvBudgetMaster.DataSource = DsAddNewRow.Copy();
                   grvBudgetMaster.DataBind();


                    DsAddNewRow.Dispose();
                }
                else
                {

                    DataTable dt = new DataTable();

                    dt.Columns.Add("Region");
                    dt.Columns.Add("Market");
                    dt.Columns.Add("Currency");
                    dt.Columns.Add("FlightNo");
                    dt.Columns.Add("Origin");
                    dt.Columns.Add("Destination");
                    dt.Columns.Add("Year");
                    dt.Columns.Add("Month");
                    dt.Columns.Add("AC_Type");
                    dt.Columns.Add("FLT_INDIC");
                    dt.Columns.Add("IsActive");
                    dt.Columns.Add("POS");
                    dt.Columns.Add("No_Of_Flights");
                    dt.Columns.Add("UoM");
                    dt.Columns.Add("Cargo_per_Flight");
                    dt.Columns.Add("PO_Mail_per_Flight");
                    dt.Columns.Add("Courier_per_Flight");
                    dt.Columns.Add("Cargo_Rate");
                    dt.Columns.Add("PO_Mail_rate");
                    dt.Columns.Add("Courier_Rate");
                    dt.Columns.Add("ForeCastBudget");
                    dt.Columns.Add("TargetBudget");
                    dt.Columns.Add("ID");

                    DataRow dr = dt.NewRow();
                    dr["Region"] = "";
                    dr["Market"] = "";
                    dr["Currency"] = "";
                    dr["FlightNo"] = "";
                    dr["Origin"] = "";
                    dr["Destination"] = "";
                    dr["Year"] = "";
                    dr["Month"] = "";
                    dr["AC_Type"] = "";
                    dr["FLT_INDIC"] = "";
                    dr["IsActive"] = false;
                    dr["POS"] = "";
                    dr["No_Of_Flights"] = "";
                    dr["UoM"] = "";
                    dr["Cargo_per_Flight"] = "";
                    dr["PO_Mail_per_Flight"] = "";
                    dr["Courier_per_Flight"] = "";
                    dr["Cargo_Rate"] = "";
                    dr["PO_Mail_rate"] = "";
                    dr["Courier_Rate"] = "";
                    dr["ForeCastBudget"] = "";
                    dr["TargetBudget"] = "";
                    dr["ID"] = "";

                    dt.Rows.Add(dr);
                    dt.AcceptChanges();

                    grvBudgetMaster.DataSource = dt;
                    grvBudgetMaster.DataBind();
                    dt.Dispose();


                }



                #endregion

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }

        #endregion


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
                            ddlOrigin.Items.Insert(0, "Select");
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
      /*  private void  GetCountry()
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
                            ddlCountry.Items.Insert(0, "All");
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
        */
     /*   # region Get POS List

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
*/
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
                            ddlDestination.Items.Insert(0, "Select");
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
                lblStatus.Text = string.Empty;

                string Origin = ""; 
                string Dest = "";
                string FlightNo = "";
                string Year="";
                string Month="";
                string Region = "";
                if (ddlOrigin.SelectedItem.Text.Trim() == "Select")
                {
                    lblStatus.Text = "Please Select Origin";
                    lblStatus.ForeColor = Color.Red;

                }        
                else
                      {
                          Origin = ddlOrigin.SelectedValue.ToString().Trim();
                      } 
                if (ddlDestination.SelectedItem.Text.Trim() == "Select")
                {
                    lblStatus.Text = "Please Select Destination";
                    lblStatus.ForeColor = Color.Red;

                }        
                else
                      {
                          Dest = ddlDestination.SelectedValue.ToString().Trim();
                      } 
                if (ddlRegion.SelectedItem.Text.Trim() != "All")
                {
                    Region =ddlRegion.SelectedValue.ToString().Trim();
                }
                  
                if (ddlFlightNumber.SelectedItem.Text.Trim() != "All")
                {
                    FlightNo = ddlFlightNumber.SelectedValue.ToString().Trim();
                }
                if (ddlYear.SelectedItem.Text.Trim() == "Select")
                      {
                            lblStatus.Text = "Please Select Year";
                            lblStatus.ForeColor = Color.Red;
                      }
                else
                      {
                      Year = ddlYear.SelectedItem.Text.Trim();
                      } 
                
                if (ddlMonth.SelectedItem.Text.Trim() == "Select")
                    {
                        lblStatus.Text = "Please Select Year";
                        lblStatus.ForeColor = Color.Red;

                    }
                else
                    {
                        Month =ddlMonth.SelectedItem.Text.Trim();
                    }
              

         
                string[] paramname = new string[6];
                paramname[0] = "Origin";
                paramname[1] = "Dest";
           
                paramname[2] = "FlightNo";
                paramname[3] = "Year";
                paramname[4] = "Month";
                paramname[5] = "Region";

               
                object[] paramvalue = new object[6];
               
                paramvalue[0] = Origin;
                paramvalue[1] = Dest;
               
              
                paramvalue[2] = FlightNo;
                paramvalue[3] = Year;
                paramvalue[4] = Month;
                paramvalue[5] = Region;



                
                SqlDbType[] paramtype = new SqlDbType[6];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
               
                paramtype[3] = SqlDbType.Int;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.VarChar;
              
      
               
                    dsbudget = da.SelectRecords("sp_GetBudgetMaster", paramname, paramvalue, paramtype);
                    Session["dsBudgetMaster"] = dsbudget;
                    //if (dsbudget != null || dsbudget.Tables.Count > 1)
                    //{
                    // grvBudgetMaster.DataSource = dsbudget.Tables[0];
                    // grvBudgetMaster.DataBind();
                    //    btnExport.Visible = true;
                    //}
                    //else
                    //{
                    //    lblStatus.Text = "No Data Found...!";
                    //    lblStatus.ForeColor = Color.Red;
                    //}
                    if (dsbudget != null)
                    {
                        if (dsbudget.Tables != null)
                        {
                            if (dsbudget.Tables.Count > 0)
                            {
                                if (dsbudget.Tables[0].Rows.Count > 0)
                                {
                                    grvBudgetMaster.PageIndex = 0;
                                    grvBudgetMaster.DataSource = dsbudget;
                                    grvBudgetMaster.DataMember = dsbudget.Tables[0].TableName;
                                    grvBudgetMaster.DataBind();
                                    grvBudgetMaster.Visible = true;
                                 Session["dsBudgetMaster"] = dsbudget.Tables[0];


                                }
                                else
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Visible = true;
                                    lblStatus.Text = "No records found...";
                                    grvBudgetMaster.PageIndex = 0;
                                    grvBudgetMaster.Visible = false;
                                }
                            }
                        }
                    }
      
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

        public void GetData()
        {
            try
            {
                lblStatus.Text = string.Empty;

                string Origin = "";
                string Dest = "";
                string FlightNo = "";
                string Year = "";
                string Month = "";
                string Region = "";
                if (ddlOrigin.SelectedItem.Text.Trim() == "Select")
                {
                    lblStatus.Text = "Please Select Origin";
                    lblStatus.ForeColor = Color.Red;

                }
                else
                {
                    Origin = ddlOrigin.SelectedValue.ToString().Trim();
                }
                if (ddlDestination.SelectedItem.Text.Trim() == "Select")
                {
                    lblStatus.Text = "Please Select Destination";
                    lblStatus.ForeColor = Color.Red;

                }
                else
                {
                    Dest = ddlDestination.SelectedValue.ToString().Trim();
                }
                if (ddlRegion.SelectedItem.Text.Trim() != "All")
                {
                    Region = ddlRegion.SelectedValue.ToString().Trim();
                }

                if (ddlFlightNumber.SelectedItem.Text.Trim() != "All")
                {
                    FlightNo = ddlFlightNumber.SelectedValue.ToString().Trim();
                }
                if (ddlYear.SelectedItem.Text.Trim() == "Select")
                {
                    lblStatus.Text = "Please Select Year";
                    lblStatus.ForeColor = Color.Red;
                }
                else
                {
                    Year = ddlYear.SelectedItem.Text.Trim();
                }

                if (ddlMonth.SelectedItem.Text.Trim() == "Select")
                {
                    lblStatus.Text = "Please Select Year";
                    lblStatus.ForeColor = Color.Red;

                }
                else
                {
                    Month = ddlMonth.SelectedItem.Text.Trim();
                }



                string[] paramname = new string[6];
                paramname[0] = "Origin";
                paramname[1] = "Dest";

                paramname[2] = "FlightNo";
                paramname[3] = "Year";
                paramname[4] = "Month";
                paramname[5] = "Region";


                object[] paramvalue = new object[6];

                paramvalue[0] = Origin;
                paramvalue[1] = Dest;


                paramvalue[2] = FlightNo;
                paramvalue[3] = Year;
                paramvalue[4] = Month;
                paramvalue[5] = Region;




                SqlDbType[] paramtype = new SqlDbType[6];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;

                paramtype[3] = SqlDbType.Int;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.VarChar;



                dsbudget = da.SelectRecords("sp_GetBudgetMaster", paramname, paramvalue, paramtype);
               
                if (dsbudget != null)
                {
                    if (dsbudget.Tables != null)
                    {
                        if (dsbudget.Tables.Count > 0)
                        {
                            if (dsbudget.Tables[0].Rows.Count > 0)
                            {
                                //grvBudgetMaster.PageIndex = 0;
                                //grvBudgetMaster.DataSource = dsbudget;
                                //grvBudgetMaster.DataMember = dsbudget.Tables[0].TableName;
                                //grvBudgetMaster.DataBind();
                                //grvBudgetMaster.Visible = true;
                                Session["dsBudgetMaster"] = dsbudget;
                               

                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Visible = true;
                                lblStatus.Text = "No records found...";
                                grvBudgetMaster.PageIndex = 0;
                            }
                        }
                    }
                }

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
            lblStatus.Text = "";
            try
            {
                GetData();
               
                if ((DataSet)Session["dsBudgetMaster"] == null)
                    return;
               

                dsExp = (DataSet)Session["dsBudgetMaster"];
           
                dt = (DataTable)dsExp.Tables[0];
             
                string attachment = "attachment; filename=FlightBudget.xls";
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
          
            //txtFromDate.Text = ((DateTime)Session["IT"]).ToString("yyyy-MM-dd");
            //txtToDate.Text = ((DateTime)Session["IT"]).ToString("yyyy-MM-dd");
            ddlRegion.SelectedIndex = 0;
            ddlOrigin.SelectedIndex = 0;
            ddlDestination.SelectedIndex = 0;
            ddlMonth.SelectedIndex = 0;
            ddlYear.SelectedIndex = 0;
            //ddlPOS.SelectedIndex = 0;
            //txtFlightNo.Text = "";
         //   btnExport.Visible = false;
        
            Response.Redirect("~/FlightBudgetMaster.aspx");
        }

     

        protected void grvBudgetMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
            
                DataTable dt = null;
          
                dt = (DataTable)Session["dsBudgetMaster"];
              //  dt = (DataTable)dsExp.Tables[0]; 
                
                grvBudgetMaster.PageIndex = e.NewPageIndex;
                grvBudgetMaster.DataSource = dt;
                grvBudgetMaster.DataBind();



            }
            catch (Exception ex)
            {

            }
        }

    /*    #region Fill Currency
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
        */
        protected void LoadRegion()
        {
            StockAllocationBAL objBAL = new StockAllocationBAL();
            DataSet Region = null;
            try
            {
                Region = objBAL.GetRegionCode();

                ddlRegion.Items.Clear();
                ddlRegion.Items.Add(new ListItem("All"));

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
                if (ddlFlightPrefix.SelectedItem.Value.ToString() == "All")
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
                            ddlFlightNumber.Items.Insert(0, new ListItem("All", ""));
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


        #region Add Click
        private void ADD()
        {
            DataTable dtNewList = new DataTable();
            if (Session["dsBudgetMaster"] == null)
            {
                dtNewList = null;
            }
            else
            {
                dtNewList = (DataTable)Session["dsBudgetMaster"];
            }
            if (dtNewList == null)
            {
                dtNewList = new DataTable();

                dtNewList.Columns.Add("Region");
                dtNewList.Columns.Add("Market");
                dtNewList.Columns.Add("Currency");
                dtNewList.Columns.Add("FlightNo");
                dtNewList.Columns.Add("Origin");
                dtNewList.Columns.Add("Destination");
                dtNewList.Columns.Add("Year");
                dtNewList.Columns.Add("Month");
                dtNewList.Columns.Add("AC_Type");
                dtNewList.Columns.Add("FLT_INDIC");
               // dtNewList.Columns.Add("IsActive", typeof(bool));
                dtNewList.Columns.Add("POS");
                dtNewList.Columns.Add("No_Of_Flights");
                dtNewList.Columns.Add("UoM");
                dtNewList.Columns.Add("Cargo_per_Flight");
                dtNewList.Columns.Add("PO_Mail_per_Flight");
                dtNewList.Columns.Add("Courier_per_Flight");
                dtNewList.Columns.Add("Cargo_Rate");
                dtNewList.Columns.Add("PO_Mail_rate");
                dtNewList.Columns.Add("Courier_Rate");
                dtNewList.Columns.Add("ForeCastBudget");
                dtNewList.Columns.Add("TargetBudget");
                dtNewList.Columns.Add("ID");

            }
            //DataSet dtNewList = null;
            lblStatus.Text = "";
            try
            {
               

                DataRow l_Datarow = dtNewList.NewRow();

                l_Datarow["Region"] = "";
                l_Datarow["Market"] = "";
                l_Datarow["Currency"] = "";
                l_Datarow["FlightNo"] = "";
                l_Datarow["Origin"] = "";
                l_Datarow["Destination"] = "";
                l_Datarow["Year"] = 0;
                l_Datarow["Month"] = "";
                l_Datarow["AC_Type"] = "";
                l_Datarow["FLT_INDIC"] = "";
               // l_Datarow["IsActive"] = false;
                l_Datarow["POS"] = "";
                l_Datarow["No_Of_Flights"] = 0;
                l_Datarow["UoM"] = "";
                l_Datarow["Cargo_per_Flight"] = 0;
                l_Datarow["PO_Mail_per_Flight"] = 0;
                l_Datarow["Courier_per_Flight"] = 0;
                l_Datarow["Cargo_Rate"] = 0.00;
                l_Datarow["PO_Mail_rate"] = 0;
                l_Datarow["Courier_Rate"] = 0.00;
                l_Datarow["ForeCastBudget"] = 0.00;
                l_Datarow["TargetBudget"] = 0.00;
                l_Datarow["ID"] =0;

                dtNewList.Rows.Add(l_Datarow);

                grvBudgetMaster.DataSource = dtNewList;
                grvBudgetMaster.DataBind();
                Session["dsBudgetMaster"] = dtNewList;



            }

            catch (Exception ex)
            {
            }
            finally
            {
                if (dtNewList != null)
                    dtNewList.Dispose();
            }
        }
        #endregion

        protected void grvBudgetMaster_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;

            ADD();
          
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool CheckForSelect = false;
            lblStatus.Text = string.Empty;
            int parmcount = 0;
            for (int i = 0; i < grvBudgetMaster.Rows.Count; i++)
            {
                CheckBox chkBox = (CheckBox)grvBudgetMaster.Rows[i].FindControl("chkSelect");
                if (chkBox.Checked)
                {
                    CheckForSelect = true;

                    parmcount++;
                    TextBox txtRegion = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtRegion");
                    TextBox txtMarket = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtMarket");
                    TextBox txtCurrency = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtCurrency");
                    TextBox txtFlightNo = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtFlightNo");
                    TextBox txtOrigin = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtOrigin");
                    TextBox txtDestination = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtDestination");
                    TextBox txtYear = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtYear");
                    TextBox txtMonth = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtMonth");
                    TextBox txtAC_Type = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtAC_Type");
                    TextBox txtFLT_INDIC = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtFLT_INDIC");
                    TextBox txtPOS = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtPOS");
                    TextBox txtNo_Of_Flights = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtNo_Of_Flights");
                    TextBox txtUoM = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtUoM");
                    TextBox txtCargo_per_Flight = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtCargo_per_Flight");
                    TextBox txtPO_Mail_per_Flight = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtPO_Mail_per_Flight");
                    TextBox txtCourier_per_Flight = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtCourier_per_Flight");
                    TextBox txtCargo_Rate = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtCargo_Rate");
                    TextBox txtPO_Mail_rate = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtPO_Mail_rate");
                    TextBox txtCourier_Rate = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtCourier_Rate");
                    TextBox txtForeCastBudget = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtForeCastBudget");
                    TextBox txtTargetBudget = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtTargetBudget");
                    TextBox txtID = (TextBox)grvBudgetMaster.Rows[i].FindControl("txtID");
                 
                    string[] paramname = new string[22];
                    paramname[0] = "Region";
                    paramname[1] = "Market";
                    paramname[2] = "Currency";
                    paramname[3] = "Flight";
                    paramname[4] = "Origin";
                    paramname[5] = "Dest";
                    paramname[6] = "Year";
                    paramname[7] = "Month";
                    paramname[8] = "AcType";
                    paramname[9] = "FLT";
                    paramname[10] = "POS";
                    paramname[11] = "NoofFlight";
                    paramname[12] = "UoM";
                    paramname[13] = "Cargo";
                    paramname[14] = "POMail";
                    paramname[15] = "Courier";
                    paramname[16] = "CargoRate";
                    paramname[17] = "POMailRate";
                    paramname[18] = "CourierRate";
                    paramname[19] = "ForeCast";
                    paramname[20] = "Target";
                    paramname[21] = "ID";


                    object[] paramvalue = new object[22];

                    paramvalue[0] = txtRegion.Text;
                    paramvalue[1] = txtMarket.Text;
                    paramvalue[2] = txtCurrency.Text;

                    //if(txtFlightNo.Text!="")
                    //paramvalue[3] = Convert.ToInt32(txtFlightNo.Text);
                    //else
                    paramvalue[3] =txtFlightNo.Text;

                    paramvalue[4] = txtOrigin.Text;
                    paramvalue[5] = txtDestination.Text;

                    if (txtYear.Text != "")
                        paramvalue[6] = Convert.ToInt32(txtYear.Text);
                    else
                        paramvalue[6] = 0;

                    paramvalue[7] = txtMonth.Text;
                    paramvalue[8] =txtAC_Type.Text;

                    //if (txtFLT_INDIC.Text != "")
                    //    paramvalue[9] = Convert.ToInt32(txtFLT_INDIC.Text);
                    //else
                        paramvalue[9] = txtFLT_INDIC.Text;

                    paramvalue[10] =txtPOS.Text;

                    if (txtNo_Of_Flights.Text != "")
                        paramvalue[11] = Convert.ToInt32(txtNo_Of_Flights.Text);
                    else
                        paramvalue[11] = 0;

                    paramvalue[12] = txtUoM.Text;
                    
                    if(txtCargo_per_Flight.Text!="")
                        paramvalue[13] =Convert.ToInt32(txtCargo_per_Flight.Text);
                   else
                        paramvalue[13] = 0;

                    if(txtPO_Mail_per_Flight.Text!="")
                         paramvalue[14] =Convert.ToInt32(txtPO_Mail_per_Flight.Text);
                    else
                        paramvalue[14] =0;

                    if(txtCourier_per_Flight.Text!="")
                    paramvalue[15] = Convert.ToInt32(txtCourier_per_Flight.Text);
                    else
                        paramvalue[15] = 0;

                    if(txtCargo_Rate.Text!="")
                    paramvalue[16] =Convert.ToDecimal(txtCargo_Rate.Text);
                    else
                        paramvalue[16] = 0.00;

                    if(txtPO_Mail_rate.Text!="")
                    paramvalue[17] =Convert.ToInt32(txtPO_Mail_rate.Text);
                    else
                        paramvalue[17] = 0;
                   if(txtCourier_Rate.Text!="")
                    paramvalue[18] =Convert.ToDecimal(txtCourier_Rate.Text);
                    else
                       paramvalue[18] =0.00;

                   if (txtForeCastBudget.Text != "")
                       paramvalue[19] = Convert.ToDecimal(txtForeCastBudget.Text);
                   else
                       paramvalue[19] = 0.00;

                   if (txtTargetBudget.Text != "")
                       paramvalue[20] = Convert.ToDecimal(txtTargetBudget.Text);
                   else
                       paramvalue[20] = 0.00;
                    if (txtID.Text != "")
                        paramvalue[21] = txtID.Text;
                    else
                    paramvalue[21] = 0;

                    SqlDbType[] paramtype = new SqlDbType[22];
                    paramtype[0] = SqlDbType.VarChar;
                    paramtype[1] = SqlDbType.VarChar;
                    paramtype[2] = SqlDbType.VarChar;
                    paramtype[3] = SqlDbType.VarChar;
                    paramtype[4] = SqlDbType.VarChar;
                    paramtype[5] = SqlDbType.VarChar;
                    paramtype[6] = SqlDbType.Int;
                    paramtype[7] = SqlDbType.VarChar;
                    paramtype[8] = SqlDbType.VarChar;
                    paramtype[9] = SqlDbType.VarChar;
                    paramtype[10] = SqlDbType.VarChar;
                    paramtype[11] = SqlDbType.Int;
                    paramtype[12] = SqlDbType.VarChar;
                    paramtype[13] = SqlDbType.Int;
                    paramtype[14] = SqlDbType.Int;
                    paramtype[15] = SqlDbType.Int;
                    paramtype[16] = SqlDbType.Decimal;
                    paramtype[17] = SqlDbType.Int;
                    paramtype[18] = SqlDbType.Decimal;
                    paramtype[19] = SqlDbType.Decimal;
                    paramtype[20] = SqlDbType.Decimal;
                    paramtype[21] = SqlDbType.Int;



                    DataSet dsCategory = da.SelectRecords("sp_SaveFlightBudget", paramname, paramvalue, paramtype);



                    if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
                    {
                        if (dsCategory.Tables[0].Rows[0][0].ToString() == "INSERTED")
                        {


                            

                            #region for Master Audit Log
                            #region Prepare Parameters
                            object[] Paramsmaster = new object[7];
                            int count = 0;

                            //1

                            Paramsmaster.SetValue("Flight Budget Master", count);
                            count++;

                            //2
                            Paramsmaster.SetValue(txtFlightNo, count);
                            count++;

                            //3

                            Paramsmaster.SetValue("SAVE", count);
                            count++;

                            //4

                            Paramsmaster.SetValue("", count);
                            count++;


                            //5

                            Paramsmaster.SetValue("", count);
                            count++;

                            //6

                            Paramsmaster.SetValue(Session["UserName"], count);
                            count++;

                            //7
                            Paramsmaster.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), count);
                            count++;


                            #endregion Prepare Parameters
                            ObjMAL.AddMasterAuditLog(Paramsmaster);
                            #endregion

                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Visible = true;
                            lblStatus.Text = "Flight Budget Inserted Successfully !!";

                        }
                        else if (dsCategory.Tables[0].Rows[0][0].ToString() == "UPDATED")
                        {


                        
                            #region for Master Audit Log
                            #region Prepare Parameters
                            object[] Paramsmaster = new object[7];
                            int count = 0;

                            //1

                            Paramsmaster.SetValue("Flight Budget Master", count);
                            count++;

                            //2
                            Paramsmaster.SetValue(txtFlightNo, count);
                            count++;

                            //3

                            Paramsmaster.SetValue("UPDATE", count);
                            count++;

                            //4

                            Paramsmaster.SetValue("", count);
                            count++;


                            //5

                            Paramsmaster.SetValue("", count);
                            count++;

                            //6

                            Paramsmaster.SetValue(Session["UserName"], count);
                            count++;

                            //7
                            Paramsmaster.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), count);
                            count++;


                            #endregion Prepare Parameters
                            ObjMAL.AddMasterAuditLog(Paramsmaster);
                            #endregion

                            btnList_Click(null, null);
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Visible = true;
                            lblStatus.Text = "Flight Budget Updated Successfully !!";

                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Visible = true;
                        lblStatus.Text = "Error In Inserting Data !!";
                    }
                }

            }
            if (parmcount == 0)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Visible = true;
                lblStatus.Text = "Please select a checkbox !!";
                return;
            }
            btnSave.Enabled = true;
            btnAdd.Enabled = true;

        }
        private bool LoadBudgetExcelData(string ext, string filepath)
        {
            string logPath = string.Empty;
            string logfilename = string.Empty;
            DateTime dtCurrentDate = new DateTime();
            string strUserName = "";
            //excelUpload obj = new excelUpload();


            FileInfo fics = new FileInfo(Server.MapPath(path + BudgetFileUpload.FileName + "_Log" + ".txt"));
            bool fileExists = fics.Exists;

            if (fileExists == true)
            {
                fics.Delete();
            }


            logfilename = BudgetFileUpload.FileName + "_Log" + ".txt";
            logPath = Server.MapPath(path + logfilename);
            Session["logpath"] = logPath;
            Session["logfilename"] = logfilename;
            string connString = string.Empty;

            FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = null;
            if (ext == ".xls")
            {
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                //...
            }
            else if (ext == ".xlsx")
            {
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //...
            }
            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            DataSet result = new DataSet();
            result = excelReader.AsDataSet();

            //...
            //4. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result1 = excelReader.AsDataSet();

            //6. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();

            bool IsInsert = false;

            try
            {

                DataSet dsPO = new DataSet();
                //oleda.Fill(dsPO, "Order");
                dsPO = result1.Copy();
                if (dsPO != null && dsPO.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsPO.Tables[0].Rows.Count; i++)
                    {
                        string Region = (string)(dsPO.Tables[0].Rows[i][0].ToString());
                        string Market = (string)(dsPO.Tables[0].Rows[i][1].ToString());
                        string FlightNo = (string)(dsPO.Tables[0].Rows[i][2].ToString());
                        string Origin = (string)(dsPO.Tables[0].Rows[i][3].ToString());
                        string Destination = (string)(dsPO.Tables[0].Rows[i][4].ToString());
                        string Year = (string)(dsPO.Tables[0].Rows[i][5].ToString());
                        string Month = (string)(dsPO.Tables[0].Rows[i][6].ToString());
                        string AC_Type = (string)(dsPO.Tables[0].Rows[i][7].ToString());
                        string FLT_INDIC = (string)(dsPO.Tables[0].Rows[i][8].ToString());
                        string POS = (string)(dsPO.Tables[0].Rows[i][9].ToString());
                        int NO_OF_FLIGHT = Convert.ToInt32(dsPO.Tables[0].Rows[i][10].ToString());
                        string UoM = (string)(dsPO.Tables[0].Rows[i][11].ToString());
                        string Currency = (string)(dsPO.Tables[0].Rows[i][12].ToString());
                        int Cargo_per_Flight = Convert.ToInt32((dsPO.Tables[0].Rows[i][13].ToString()));
                        int PO_Mail_per_Flight = Convert.ToInt32((dsPO.Tables[0].Rows[i][14].ToString()));
                        int Courier_per_Flight = Convert.ToInt32((dsPO.Tables[0].Rows[i][15].ToString()));
                        decimal Cargo_Rate = Convert.ToDecimal((dsPO.Tables[0].Rows[i][16].ToString()));
                        int PO_Mail_rate = Convert.ToInt32((dsPO.Tables[0].Rows[i][17].ToString()));
                        decimal Courier_Rate = Convert.ToDecimal((dsPO.Tables[0].Rows[i][18].ToString()));


                        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
                        SQLServer da = new SQLServer(constr);

                        try
                        {
                            IsInsert = da.InsertData("insert into FlightBudget(Region,Market,FlightNo,Origin,Destination,Year,Month,AC_Type,FLT_INDIC,POS,No_Of_Flights,UoM,Currency,Cargo_per_Flight,PO_Mail_per_Flight,Courier_per_Flight,Cargo_Rate,PO_Mail_rate,Courier_Rate) values('" + Region + "','" + Market + "','" + FlightNo + "','" + Origin + "','" + Destination + "'," + Year + ",'" + Month + "','" + AC_Type + "','" + FLT_INDIC + "','" + POS + "'," + NO_OF_FLIGHT + ",'" + UoM + "','" + Currency + "'," + Cargo_per_Flight + "," + PO_Mail_per_Flight + "," + Courier_per_Flight + "," + Cargo_Rate + "," + PO_Mail_rate + "," + Courier_Rate + ")");
                            //da.InsertData("insert into tblProrateMaster(OriginCode,DestCode,ProrateFactor,isActive) values('" + Origin + "','" + Dest + "'," + Prorate + ",'true')");
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                if (IsInsert == true)
                {

                    flag = true;
                    uploadsheet = "Budget";
                    string uploadflg = objUpload.FlagUpload(flag, uploadsheet);
                }
                strflag = "true";
                return IsInsert;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected void btnUploadBudget_Click(object sender, EventArgs e)
        {
            #region Checking file extension
            try
            {
                Boolean FileOK = false;
                String FileExtension = string.Empty;
                String filePath = string.Empty;


                if (BudgetFileUpload.HasFile)
                {
                    Session["WorkingFile"] = BudgetFileUpload.FileName;
                    FileExtension = Path.GetExtension(Session["WorkingFile"].ToString()).ToLower();
                    if (BudgetFileUpload.FileName.Contains("-"))
                    {
                        lblStatus.Text = "Please remove - sign from file name";
                        return;
                    }
                    String[] allowedExtensions = { ".xls", ".xlsx" };
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (FileExtension == allowedExtensions[i])
                        {
                            FileOK = true;
                        }
                    }
                }
                if (FileOK)
                {
                    #region new add, checking existance of file
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(Server.MapPath(path));
                    }

                    FileInfo fi = new FileInfo(Server.MapPath(path + BudgetFileUpload.FileName));

                    bool fileExists = fi.Exists;
                    if (fileExists == true)
                    {
                        lblStatus.Text = "File already Uploaded";
                        return;
                    }
                    #endregion




                    string filename = Path.Combine(path, BudgetFileUpload.FileName);
                    BudgetFileUpload.SaveAs(Server.MapPath(filename));


                    //FileExcelUpload.SaveAs(Server.MapPath("/FlightData/" + FileExcelUpload.FileName));
                    //  FileExcelUpload.SaveAs(Server.MapPath(filename));
                    filePath = Server.MapPath(path + BudgetFileUpload.FileName);
                    if (!LoadBudgetExcelData(FileExtension, filePath))
                    {
                        if (strflag == "true")
                        {
                            lblStatus.Text = "File Uploaded Successfully.";
                            lblStatus.ForeColor = System.Drawing.Color.Green;
                            fi.Delete();
                        }
                        else
                        {
                            lblStatus.Text = "Please choose proper format";
                            lblStatus.ForeColor = System.Drawing.Color.Green;
                            fi.Delete();
                        }
                    }
                    else
                    {
                        lblStatus.Text = "File Processed, Check log file for details.";
                        fi = new FileInfo(Server.MapPath(path + BudgetFileUpload.FileName));
                        fileExists = fi.Exists;
                        if (fileExists == true)
                        {
                            fi.Delete();
                        }
                    }
                    //btnUpload.Visible = false;
                    //btnDownload.Visible = true;
                    //FileExcelUpload.Enabled = false;
                }
                else
                {
                    lblStatus.Text = "Cannot accept files of this type.";
                    //btnUpload.Visible = true;
                    //btnDownload.Visible = false;
                    //FileExcelUpload.Enabled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
        }
    }
}
