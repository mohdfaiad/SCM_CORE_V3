using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Data;
using QID.DataAccess;

using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using Microsoft.Reporting.WebForms;


namespace ProjectSmartCargoManager
{
    public partial class frmULDMovementHistory : System.Web.UI.Page
    {
        
        #region Variable
        private DataSet Dataset1 = new DataSet("ULDMH_DS1");
        private DataSet Dataset2 = new DataSet("ULDMH_DS2");
        clsFillCombo clsCombo = new clsFillCombo();
        SQLServer db = new SQLServer(Global.GetConnectionString());
        #endregion

        #region On page load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    txtFromDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                    if (Session["ULDMovementList"] != null)
                    {
                       // RptViewerULDHistory.Visible = false;
                        string ULDNumber = Session["ULDMovementList"].ToString();
                        DataSet ds = new DataSet("ULDMH_DS3");
                        ds = GetMovementHistory("", "", "", "", "", "", ULDNumber);
                        Session["ULDMovementList"] = null;
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    FileInfo info;
                                    ////ReportRuntime runtime = null;
                                    ////ReportDefinition definition = null;
                                    
                                    DataSet res_Revenue = new DataSet("ULDMH_DS4");
                                    res_Revenue = ds;
                                    if (res_Revenue != null)
                                    {
                                        if (res_Revenue.Tables[0].Rows.Count > 0)
                                        {
                                            #region RDLC

                                            //Logo
                                            System.IO.MemoryStream Logo = null;
                                            try
                                            {
                                                Logo = CommonUtility.GetImageStream(Page.Server);
                                                //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                                            }
                                            catch (Exception ex)
                                            {
                                                Logo = new System.IO.MemoryStream();
                                            }

                                            if (res_Revenue.Tables[0].Columns.Contains("Logo") == false)
                                            {
                                                DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                                                col1.DefaultValue = Logo.ToArray();
                                                res_Revenue.Tables[0].Columns.Add(col1);
                                            }

                                            RptULDMovementHistory.Visible = true;
                                            RptULDMovementHistory.ProcessingMode = ProcessingMode.Local;
                                            RptULDMovementHistory.LocalReport.ReportPath = Server.MapPath("~/Reports/rptfromULDMovementHistory.rdlc");
                                            //Customers dsCustomers = GetData("select top 20 * from customers");
                                            ReportDataSource datasource = new ReportDataSource("dsfrmULDMovementHistory_dtfrmULDMovementHistory", res_Revenue.Tables[0]);
                                            RptULDMovementHistory.LocalReport.DataSources.Clear();
                                            RptULDMovementHistory.LocalReport.DataSources.Add(datasource);
                                            // RptULDMovementHistory.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                                            //SaveUserActivityLog("");
                                            Session["ds_ULDMOVHistory"] = res_Revenue;
                                            #endregion

                                        }
                                        else
                                        {
                                          //  RptViewerULDHistory.Visible = false;
                                            lblStatus.ForeColor = Color.Red;
                                            lblStatus.Text = "Data not available for given search criteria";
                                            txtFromDate.Focus();
                                        }
                                        res_Revenue.Dispose();
                                    }
                                }
                                else
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "No Movement available for selected ULD's!";
                                }
                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "No Movement available for selected ULD's!";
                            }
                            ds.Dispose();
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "No Movement available for selected ULD's!";
                        }
                    }
                    clsCombo.FillAllComboBoxes("tblWarehouseMaster", "SELECT", ddlOrigin);
                    clsCombo.FillAllComboBoxes("tblWarehouseMaster", "SELECT", ddlDestination);
                }
            }
            catch (Exception)
            {
                lblStatus.Text = "Error in Page Loading..";
                lblStatus.Visible = true;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region To show subreport
        //private void WARCustWise_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{
        //    string dname = e.DataSetName; 
        //    if (dname == "DataSet1")
        //    {
        //        e.Data = Dataset1;
        //    }
        //    else
        //    {
        //        e.Data = Dataset2;
        //    }
        //}
        #endregion

        #region ListClick
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                if (ddlOrigin.SelectedItem.Text == "SELECT" && txtULDNo.Text.Trim() == "" && txtFltNo.Text.Trim() == "" && txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() == "" && ddlMovementType.SelectedItem.Text.Trim() == "SELECT")
                {
                    lblStatus.Text = "Atleast one parameter is required";
                    return;
                }
                string Origin = "", Dest = "", FlightNo = "", FromDt = "", ToDt = "", MType = "", ULDNo = "";
                if (ddlOrigin.SelectedItem.Text.Trim() == "SELECT") Origin = ""; else Origin = ddlOrigin.SelectedItem.Text.Trim();
                if (ddlDestination.SelectedItem.Text.Trim() == "SELECT") Dest = ""; else Dest = ddlDestination.SelectedItem.Text.Trim();
                if (ddlMovementType.SelectedItem.Text.Trim() == "SELECT") MType = ""; else MType = ddlMovementType.SelectedItem.Text.Trim();
                if (txtFltNo.Text.Trim() == "") FlightNo = ""; else FlightNo = txtFltNo.Text.Trim();
                if (txtFromDate.Text.Trim() == "") FromDt = ""; else FromDt = txtFromDate.Text.Trim();
                if (txtToDate.Text.Trim() == "") ToDt = ""; else ToDt = txtToDate.Text.Trim();
                if (txtULDNo.Text.Trim() == "") ULDNo = ""; else ULDNo = txtULDNo.Text.Trim();

                DataSet res_Revenue = new DataSet("ULDMH_DS5");
                res_Revenue = GetMovementHistory(Origin, Dest, FlightNo, FromDt, ToDt, MType, ULDNo);
                FileInfo info;
                ////ReportRuntime runtime = null;
                ////ReportDefinition definition = null;
                if (res_Revenue != null)
                {
                    if (res_Revenue.Tables[0].Rows.Count > 0)
                    {
                        //Dataset1 = res_Revenue;
                        //RptViewerULDHistory.Visible = false;
                        //info = new FileInfo(Server.MapPath("/Reports/ULDMovementHistory.rdlx"));
                        //definition = new ReportDefinition(info);
                        //runtime = new ReportRuntime(definition);
                        //runtime.LocateDataSource += WARCustWise_LocateDataSource;
                        //RptViewerULDHistory.SetReport(runtime);

                        //DataTable myDataTable = new DataTable();
                        //DataColumn myDataColumn;
                        //DataSet Ds = new DataSet();

                        //myDataColumn = new DataColumn();
                        //myDataColumn.DataType = Type.GetType("System.String");
                        //myDataColumn.ColumnName = "ULDNumber";
                        //myDataTable.Columns.Add(myDataColumn);


                        //myDataColumn = new DataColumn();
                        //myDataColumn.DataType = Type.GetType("System.String");
                        //myDataColumn.ColumnName = "Flight";
                        //myDataTable.Columns.Add(myDataColumn);

                        //myDataColumn = new DataColumn();
                        //myDataColumn.DataType = Type.GetType("System.String");
                        //myDataColumn.ColumnName = "Origin";
                        //myDataTable.Columns.Add(myDataColumn);

                        //myDataColumn = new DataColumn();
                        //myDataColumn.DataType = Type.GetType("System.String");
                        //myDataColumn.ColumnName = "Destination";
                        //myDataTable.Columns.Add(myDataColumn);

                       
                        //myDataColumn = new DataColumn();
                        //myDataColumn.DataType = Type.GetType("System.String");
                        //myDataColumn.ColumnName = "MovedOn";
                        //myDataTable.Columns.Add(myDataColumn);

                        //myDataColumn = new DataColumn();
                        //myDataColumn.DataType = Type.GetType("System.String");
                        //myDataColumn.ColumnName = "Type";
                        //myDataTable.Columns.Add(myDataColumn);

                        //myDataColumn = new DataColumn();
                        //myDataColumn.DataType = Type.GetType("System.String");
                        //myDataColumn.ColumnName = "UpdatedBy";
                        //myDataTable.Columns.Add(myDataColumn);

                        //myDataColumn = new DataColumn();
                        //myDataColumn.DataType = Type.GetType("System.String");
                        //myDataColumn.ColumnName = "Status";
                        //myDataTable.Columns.Add(myDataColumn);

                      

                        //DataRow dr;
                        //dr = myDataTable.NewRow();
                        //dr["ULDNumber"] = txtULDNo.Text;
                        //dr["Flight"] = txtFltNo.Text;
                        //dr["Origin"] = ddlOrigin.SelectedItem.Text;
                        //dr["Destination"] = ddlDestination.SelectedItem.Text;
                      

                        //dr["MovedOn"] = Flightfromdate;
                        //dr["Type"] = FlightToDate;// "9";
                        //dr["UpdatedBy"] = ddlAirCraftType.SelectedValue;// "9";
                        //dr["Status"] = ddlStatus.SelectedValue;// "9";
                        
                        //myDataTable.Rows.Add(dr);

                        //  Ds.Tables.Add(myDataTable);

                        //DataSet dschk = new DataSet();
                        //dschk.Tables.Add(myDataTable);
                        //Dataset2 = dschk.Copy();

                        //Dataset1 = ds.Copy();

                        #region RDLC

                        //Logo
                        System.IO.MemoryStream Logo = null;
                        try
                        {
                            Logo = CommonUtility.GetImageStream(Page.Server);
                            //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        }
                        catch (Exception ex)
                        {
                            Logo = new System.IO.MemoryStream();
                        }

                        if (res_Revenue.Tables[0].Columns.Contains("Logo") == false)
                        {
                            DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                            col1.DefaultValue = Logo.ToArray();
                            res_Revenue.Tables[0].Columns.Add(col1);
                        }

                        RptULDMovementHistory.Visible = true;
                        RptULDMovementHistory.ProcessingMode = ProcessingMode.Local;
                        RptULDMovementHistory.LocalReport.ReportPath = Server.MapPath("~/Reports/rptfromULDMovementHistory.rdlc");
                        //Customers dsCustomers = GetData("select top 20 * from customers");
                        ReportDataSource datasource = new ReportDataSource("dsfrmULDMovementHistory_dtfrmULDMovementHistory", res_Revenue.Tables[0]);
                        RptULDMovementHistory.LocalReport.DataSources.Clear();
                        RptULDMovementHistory.LocalReport.DataSources.Add(datasource);
                       // RptULDMovementHistory.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);
                        //SaveUserActivityLog("");
                        Session["ds_ULDMOVHistory"] = res_Revenue;
                        #endregion

                    }
                    else
                    {
                        RptULDMovementHistory.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        txtFromDate.Focus();
                        Session["ds_ULDMOVHistory"] = null;
                    }
                    res_Revenue.Dispose();
                }
                else
                {
                    //RptViewerULDHistory.Visible = false;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Data not available for given search criteria";
                    txtFromDate.Focus();
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Error in Getting Data: " + ex.Message;
            }
        }
        #endregion

        #region GetMovement DataSet
        public DataSet GetMovementHistory(string Origin, string Destination, string FlightNo, string FromDt, string ToDt, string MoveType, string ULDNo)
        {
            DataSet ds = new DataSet("ULDMH_DS6");
            string[] QueryNames = new string[7];
            object[] QueryValues = new object[7];
            SqlDbType[] QueryTypes = new SqlDbType[7];
            try
            {
                lblStatus.Text = "";
                Session["grdULDMoveHistory"] = null;

                QueryNames[0] = "Origin";
                QueryNames[1] = "Destination";
                QueryNames[2] = "FlightNo";
                QueryNames[3] = "FromDt";
                QueryNames[4] = "ToDt";
                QueryNames[5] = "MovementType";
                QueryNames[6] = "ULDno";

                QueryTypes[0] = SqlDbType.VarChar;
                QueryTypes[1] = SqlDbType.VarChar;
                QueryTypes[2] = SqlDbType.VarChar;
                QueryTypes[3] = SqlDbType.VarChar;
                QueryTypes[4] = SqlDbType.VarChar;
                QueryTypes[5] = SqlDbType.VarChar;
                QueryTypes[6] = SqlDbType.VarChar;

                QueryValues[0] = Origin;
                QueryValues[1] = Destination;
                QueryValues[2] = FlightNo;
                QueryValues[3] = FromDt;
                QueryValues[4] = ToDt;
                QueryValues[5] = MoveType;
                QueryValues[6] = ULDNo;

                ds = db.SelectRecords("spGETULDMovementHistory", QueryNames, QueryValues, QueryTypes);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ds != null)
                {
                    ds.Dispose();
                }
                lblStatus.Text = ex.Message;
            }
            finally
            {
                QueryNames = null;
                QueryValues = null;
                QueryTypes = null;
            }
            return null;
        }
        #endregion

        #region get data to export

        public void GetDataToExport()
        {
            try
            {
                lblStatus.Text = "";
                if (ddlOrigin.SelectedItem.Text == "SELECT" && txtULDNo.Text.Trim() == "" && txtFltNo.Text.Trim() == "" && txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() == "" && ddlMovementType.SelectedItem.Text.Trim() == "SELECT")
                {
                    lblStatus.Text = "Atleast one parameter is required";
                    return;
                }
                string Origin = "", Dest = "", FlightNo = "", FromDt = "", ToDt = "", MType = "", ULDNo = "";
                if (ddlOrigin.SelectedItem.Text.Trim() == "SELECT") Origin = ""; else Origin = ddlOrigin.SelectedItem.Text.Trim();
                if (ddlDestination.SelectedItem.Text.Trim() == "SELECT") Dest = ""; else Dest = ddlDestination.SelectedItem.Text.Trim();
                if (ddlMovementType.SelectedItem.Text.Trim() == "SELECT") MType = ""; else MType = ddlMovementType.SelectedItem.Text.Trim();
                if (txtFltNo.Text.Trim() == "") FlightNo = ""; else FlightNo = txtFltNo.Text.Trim();
                if (txtFromDate.Text.Trim() == "") FromDt = ""; else FromDt = txtFromDate.Text.Trim();
                if (txtToDate.Text.Trim() == "") ToDt = ""; else ToDt = txtToDate.Text.Trim();
                if (txtULDNo.Text.Trim() == "") ULDNo = ""; else ULDNo = txtULDNo.Text.Trim();

                DataSet res_Revenue = new DataSet("ULDMH_DS7");
                res_Revenue = GetMovementHistory(Origin, Dest, FlightNo, FromDt, ToDt, MType, ULDNo);
              


                if (res_Revenue != null)
                {
                    if (res_Revenue.Tables[0].Rows.Count > 0)
                    {
                        //RptViewerULDHistory.Visible = false;

                        Session["ds_ULDMOVHistory"] = res_Revenue;

                      

                        //Logo
                        System.IO.MemoryStream Logo = null;
                        try
                        {
                            Logo = CommonUtility.GetImageStream(Page.Server);
                            //System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                        }
                        catch (Exception ex)
                        {
                            Logo = new System.IO.MemoryStream();
                        }

                        if (res_Revenue.Tables[0].Columns.Contains("Logo") == false)
                        {
                            DataColumn col1 = new DataColumn("Logo", System.Type.GetType("System.Byte[]"));
                            col1.DefaultValue = Logo.ToArray();
                            res_Revenue.Tables[0].Columns.Add(col1);
                        }

                    }
                    else
                    {
                        //RptViewerULDHistory.Visible = false;
                        RptULDMovementHistory.Visible = false;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Data not available for given search criteria";
                        Session["ds_ULDMOVHistory"] = null;
                        txtFromDate.Focus();
                    }
                    res_Revenue.Dispose();
                }
                else
                {
                    //RptViewerULDHistory.Visible = false;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Data not available for given search criteria";
                    txtFromDate.Focus();
                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Error in Getting Data: " + ex.Message;
            }
        }

        #endregion

        #region Validate Controls
        private bool Validate()
        {
            try
            {
                try
                {
                    if (txtFromDate.Text.Trim() != "" ||txtToDate.Text.Trim() != "")
                    {
                       // DateTime dt1 = DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", null); //
                        DateTime dt1= DateTime.Parse(txtFromDate.Text);
                      //  DateTime dt2 = DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", null); //
                        DateTime dt2= DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                          txtFromDate.Focus();
                            return false;
                        }


                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                 txtFromDate.Focus();
                    return false;
                }



                ReportBAL objBal = new ReportBAL();
                string strResult = string.Empty;

                try
                {
                    strResult = objBal.GetReportInterval(DateTime.Parse(txtFromDate.Text), DateTime.Parse(txtToDate.Text));
                }
                catch
                {
                    strResult = "";
                }
                finally
                {
                    objBal = null;
                }
                if (strResult != "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = strResult;
                  txtFromDate.Focus();
                    return false;
                }



            }
            catch (Exception ex)
            {


            }
            return true;

        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet("ULDMH_DS8");
            DataTable dt = new DataTable("ULDMH_DT1");
           
            try
            {
                 if (Validate() == false)
                {
                    Session["ds_ULDMOVHistory"] = null;
                    RptULDMovementHistory.Visible = false;
                    return;
                }
                GetDataToExport();

                lblStatus.Text = string.Empty;
                
                ds = (DataSet)Session["ds_ULDMOVHistory"];
              
                if (Session["ds_ULDMOVHistory"] == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    //SaveUserActivityLog(lblStatus.Text);

                    return;
                } 
                dt = (DataTable)ds.Tables[0];
                //dt = city.GetAllCity();//your datatable 
                if (dt.Columns.Contains("Logo"))
                { dt.Columns.Remove("Logo"); }
                string attachment = "attachment; filename=ULDMovementHistory.xls";
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
                ds = null;
                dt = null;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/frmULDMovementHistory.aspx",false);
        }
    }   
}
