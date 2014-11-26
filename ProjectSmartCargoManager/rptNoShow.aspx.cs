using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using BAL;
using System.Globalization;
using QID.DataAccess;

using System.IO;

namespace ProjectSmartCargoManager
{
    public partial class rptNoShow : System.Web.UI.Page
    {
        ListBookingBAL objBAL = new ListBookingBAL();
        ReportBAL objrpt = new ReportBAL();
        string errormessage = "";
        SQLServer da = new SQLServer(Global.GetConnectionString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStationDropDowns();
            }
        }
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

                    
                    if (Session["RoleName"].ToString() == "Super User")
                    {
                        ddlSource.SelectedValue = "All";
                    }
                    else
                    {
                        ddlSource.SelectedValue = Session["Station"].ToString();
                        
                    }
                    ddlDest.SelectedValue = "All";

                }
                else
                {
                    //lblStatus.Text = "" + errormessage;
                }
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
                //lblStatus.Text = string.Empty;
                string source, dest;
                source = ddlSource.SelectedItem.Text.Trim() == "All" ? "" : ddlSource.SelectedItem.Text.Trim();
                dest = ddlDest.SelectedItem.Text.Trim() == "All" ? "" : ddlDest.SelectedItem.Text.Trim();
                object[] param = { source, dest, txtAWBFromDt.Text, txtAWBToDt.Text, txtCommodityCode.Text };
                string[] pname = { "Source", "Dest", "AWBFromDate", "AWBToDate", "CommodityCode" };
                SqlDbType[] QueryTypes = { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                DataSet ds = da.SelectRecords("SP_GetrptNoShow", pname, param, QueryTypes);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ////rptNoShowAWB.Visible = true;
                        Session["ds"] = ds.Tables[0];
                        FileInfo info;
                        ////ReportRuntime runtime;
                        ////ReportDefinition definition;

                        info = new FileInfo(Server.MapPath("/Reports/rptNoShhow.rdlx"));
                        ////definition = new ReportDefinition(info);
                        ////runtime = new ReportRuntime(definition);
                        ////runtime.LocateDataSource += dsNoShow_LocateDataSource;
                        ////rptNoShowAWB.SetReport(runtime);
                    }
                    else
                    {
                        //lblStatus.Text = "No records found.";
                        //lblStatus.ForeColor = System.Drawing.Color.Red;
                        ////rptNoShowAWB.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            { }
        }
        //private void dsNoShow_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{

        //    try
        //    {
        //        string dname = e.DataSetName;
        //        DataTable dsDwellTime = (DataTable)Session["ds"];

        //        if (dname == "DataSet1")
        //        {
        //            e.Data = dsDwellTime;
        //            //Session["Export"] = dsLegWise.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    { }

        //}
    }
}
