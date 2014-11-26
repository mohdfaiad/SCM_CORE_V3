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

namespace ProjectSmartCargoManager
{
    public partial class rptOverviewAWB : System.Web.UI.Page
    {
        ArrayList arFlight = new ArrayList();
        ReportBAL OBJasb = new ReportBAL();
        private DataSet Dataset1;
        private DataSet Dataset2;
        DataSet ds;
        static string AgentCode = "";
        public static string CurrTime = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    AgentCode = Convert.ToString(Session["ACode"]);

                    int RoleId = Convert.ToInt32(Session["RoleID"]);

                    if (RoleId == 1 && AgentCode != "")
                    {
                    }

                    DateTime dtCurr = Convert.ToDateTime(Session["IT"].ToString());
                    CurrTime = dtCurr.ToString("dd/MM/yyyy hh:mm:ss tt");

                    txtFlightFromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFlightToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    ////rptViewerAWBMovement.Visible = false;
                }
            }
            catch (Exception)
            {
            }
        }

        protected void btnList_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtFlightFromdate.Text == "" || txtFlightToDate.Text == "")
                {
                }
                if (txtFlightFromdate.Text.Trim() != "" || txtFlightToDate.Text.Trim() != "")
                {
                    DateTime dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                    DateTime dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                    int chk = DateTime.Compare(dt1, dt2);
                    if (chk > 0)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Please enter valid To date";
                        return;
                    }


                }
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                return;
            }



            try
            {
                int rowIndex = 0;

                string Flightfromdate = "", FlightToDate = "";
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                try
                {

                    if (txtFlightFromdate.Text != "")
                    {
                        dt1 = DateTime.ParseExact(txtFlightFromdate.Text, "dd/MM/yyyy", null);

                        Flightfromdate = dt1.ToString("MM/dd/yyyy");
                    }
                    if (txtFlightToDate.Text != "")
                    {
                        dt2 = DateTime.ParseExact(txtFlightToDate.Text, "dd/MM/yyyy", null);
                        FlightToDate = dt2.ToString("MM/dd/yyyy");


                    }
                }
                catch (Exception ex)
                {

                }


                string FlightNo = "";
                if (txtFlightNo.Text == "")
                    FlightNo = "All";
                else
                    FlightNo = txtFlightNo.Text;

                string strdomestic = "";
                strdomestic = "All";

                string Source = "All", Dest = "All";
                if (txtAutoSource.Text.Trim() != "")
                {
                    Source = txtAutoSource.Text;
                }
                if (txtAutoDest.Text.Trim() != "")
                {
                    Dest = txtAutoDest.Text.Trim();
                }
                string country = "All";
                if (txtCountry.Text.Trim() != "")
                {
                    country = txtCountry.Text;
                }


                string Region = "All";
                if (txtRegion.Text.Trim() != "")
                {
                    Region = txtRegion.Text;
                }

                string AWBNumber = "";
                if (txtAWBNo.Text.Trim() != "")
                {
                    AWBNumber = txtAWBNo.Text.Trim();
                }
                string awbprefix = "";
                if (txtAWBPrefix.Text.Trim() != "")
                {
                    AWBNumber = txtAWBPrefix.Text.Trim();
                }
                string Aircraft = "All";
                FileInfo info;
                ////ReportRuntime runtime;
                ////ReportDefinition definition;
                DataSet ds = new DataSet();

                if (ddldateselection.SelectedItem.Text == "Execution Date")
                { ds = OBJasb.GetOverviewAWB(country, Region, Source, Dest, FlightNo, Aircraft, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic, AWBNumber, AgentCode, awbprefix); }
                else
                {
                    ds = OBJasb.GetOverviewAWBFlightDate(country, Region, Source, Dest, FlightNo, Aircraft, Flightfromdate, FlightToDate, ddlStatus.SelectedValue, strdomestic, AWBNumber, AgentCode, awbprefix);
                }


                Session["dsExp"] = ds;
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {


                            DataTable myDataTable = new DataTable();
                            DataColumn myDataColumn;
                            DataSet Ds = new DataSet();

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Country";
                            myDataTable.Columns.Add(myDataColumn);


                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Region";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Origin";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "Destination";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "FlightID";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "FromDt";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "ToDt";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "AWBNo";
                            myDataTable.Columns.Add(myDataColumn);

                            myDataColumn = new DataColumn();
                            myDataColumn.DataType = Type.GetType("System.String");
                            myDataColumn.ColumnName = "ReportDate";
                            myDataTable.Columns.Add(myDataColumn);

                            DataRow dr;
                            dr = myDataTable.NewRow();
                            dr["Country"] = txtCountry.Text.Trim();
                            dr["Region"] = txtRegion.Text.Trim(); //"5";
                            dr["Origin"] = txtAutoSource.Text.Trim() + ":";// "5";
                            dr["Destination"] = txtAutoDest.Text.Trim();
                            dr["FlightID"] = txtFlightNo.Text.Trim();

                            dr["FromDt"] = txtFlightFromdate.Text.Trim();
                            dr["ToDt"] = txtFlightToDate.Text;// "9";
                            dr["AWBNo"] = txtAWBNo.Text.Trim();
                            dr["ReportDate"] = CurrTime;
                            myDataTable.Rows.Add(dr);

                            //  Ds.Tables.Add(myDataTable);

                            DataSet dschk = new DataSet();
                            dschk.Tables.Add(myDataTable);
                            Dataset2 = dschk.Copy();


                            Dataset1 = ds.Copy();

                            ////rptViewerAWBMovement.Visible = true;
                            info = new FileInfo(Server.MapPath("/Reports/rptOverviewAwb.rdlx"));
                            ////definition = new ReportDefinition(info);
                            ////runtime = new ReportRuntime(definition);

                            ////runtime.LocateDataSource += WRAXBDetails_LocateDataSource; //+ "//images//Add-icon3.png";
                            ////rptViewerAWBMovement.SetReport(runtime);
                            //rptViewerAWBMovement.Lo(runtime);

                            btnExport.Visible = true;
                        }
                    }
                }
            }


            catch (Exception ex)
            {
            }



        }

        //private void WRAXBDetails_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{

        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")
        //    if (dname == "dsShowAWBMovement")
        //    {
        //        e.Data = Dataset1;
        //    }
        //    else
        //    {
        //        e.Data = Dataset2;
        //    }
        //}

        protected void btnclear_Click(object sender, EventArgs e)
        {
            ////rptViewerAWBMovement.Visible = false;
            txtAutoDest.Text = "";
            txtAutoSource.Text = "";
            txtCountry.Text = "";
            txtFlightNo.Text = "";
            txtFlightToDate.Text = "";
            txtRegion.Text = "";
            txtFlightFromdate.Text = "";
            lblStatus.Text = "";
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet dsExp = null;
            DataTable dt = null;

            try
            {
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
    }
}
