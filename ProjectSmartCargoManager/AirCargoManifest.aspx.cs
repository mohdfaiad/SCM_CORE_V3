using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using QID.DataAccess;
using System.Data;
using System.Drawing;
//using DataDynamics.Reports.Rendering.Pdf;
//using DataDynamics.Reports.Rendering.IO;
using System.IO;

namespace ProjectSmartCargoManager
{
    public partial class AirCargoManifest : System.Web.UI.Page
    {
        #region Variable
        SQLServer db = new SQLServer(Global.GetConnectionString());
        BALAirCargoManifest objBAL = new BALAirCargoManifest();

        private DataSet Dataset1 = new DataSet();
        private DataSet Dataset2 = new DataSet();
        private DataSet Dataset3 = new DataSet();
        #endregion variable

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblDepAirport.Text = Convert.ToString(Session["Station"]);
                GetAirportCode();
            }
        }

        # region Get Airport Code List
        private void GetAirportCode()
        {
            try
            {
                DataSet ds = objBAL.GetAirportCodes();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ddlUnlading.DataSource = ds;
                            ddlUnlading.DataMember = ds.Tables[0].TableName;
                            ddlUnlading.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlUnlading.DataTextField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlUnlading.DataBind();
                            ddlUnlading.Items.Insert(0, new ListItem("Select", "Select"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GetCurrencyCode List

        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region Parameters 
            object[] Params = new object[8];
            int i = 0;

            //1
            Params.SetValue(txtFlightNo.Text, i);
            i++;

            //2
            DateTime date = DateTime.ParseExact(txtFltDt.Text, "dd/MM/yyyy", null); 
            Params.SetValue(date, i);
            i++;

            //3
            Params.SetValue(txtOperator.Text, i);
            i++;

            //4
            Params.SetValue(txtNationality.Text, i);
            i++;

            //5
            Params.SetValue(lblDepAirport.Text, i);
            i++;

            //6
            Params.SetValue(ddlUnlading.SelectedItem.Text, i);
            i++;

            //7
            Params.SetValue(txtConsolidator.Text, i);
            i++;

            //8
            Params.SetValue(txtDeconsolidator.Text, i);
            i++;

           #endregion Parameters  For Gen Declartion

           bool result = objBAL.SaveAirCargoManifest(Params);
            if (result==true)
            {
                lblStatus.Text = "Record Added Successfully";
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                lblStatus.Text = "Record Insertion Failed";
                lblStatus.ForeColor = Color.Red;
                return;
            }
                       
        }

        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            object[] Params = new object[2];
            int i = 0;

            //1
            Params.SetValue(txtFlightNo.Text, i);
            i++;

            //2
            DateTime dt = DateTime.ParseExact(txtFltDt.Text, "dd/MM/yyyy", null);
            Params.SetValue(dt, i);
            i++;


            DataSet ds1 = objBAL.GetAirCargoManifest(Params);
            if (ds1.Tables[0].Rows.Count > 0)
            {

                string unLading = ds1.Tables[0].Rows[0]["UnladingPort"].ToString();
                ddlUnlading.SelectedIndex = ddlUnlading.Items.IndexOf(((ListItem)ddlUnlading.Items.FindByText(unLading)));

                txtOperator.Text = ds1.Tables[0].Rows[0]["Operator"].ToString();
                txtNationality.Text = ds1.Tables[0].Rows[0]["Nationality"].ToString();

                txtConsolidator.Text = ds1.Tables[0].Rows[0]["Consolidator"].ToString();
                txtDeconsolidator.Text = ds1.Tables[0].Rows[0]["DeConslidator"].ToString();
                Session["AirCargoManifest"] = ds1;
            }
            else
            {
                lblStatus.Text = "No Records Found..";
                lblStatus.ForeColor = Color.Red;
            }

        }
        
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Session["CustAWBInfoData"] != null)
                //{
                //    DataSet dset = (DataSet)Session["CustAWBInfoData"];
                //    if (dset != null)
                //    {
                //        if (dset.Tables.Count > 0)
                //        {
                //            string Kick = dset.Tables[dset.Tables.Count - 4].Rows[1]["Description"].ToString();
                //            if (Kick != "")
                //            {
                //                Session["DealData"] = dset;
                //string Flat = dset.Tables[dset.Tables.Count - 2].Rows[1]["Description"].ToString();

                ////DataDynamics.Reports.ReportDefinition _reportDef = new ReportDefinition();
                ////_reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/CBPForm7509.rdlx")));

                //DataTable dtable = dset.Tables[dset.Tables.Count - 4];
                //DataTable Dt = dset.Tables[dset.Tables.Count - 1];
                //Dataset1.Tables.Add(Dt.Copy());
                //Dataset2.Tables.Add(dtable.Copy());
                ////DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef);
                // _reportRuntime.LocateDataSource += WRAXBDetails_LocateDataSource;
                ////string exportFile = System.IO.Path.GetTempFileName() + ".pdf";
                ////System.IO.FileInfo myFile = new System.IO.FileInfo(exportFile);
                ////System.Collections.Specialized.NameValueCollection settings = new System.Collections.Specialized.NameValueCollection();
                ////settings.Add("hideToolbar", "True");
                ////settings.Add("hideMenubar", "True");
                ////settings.Add("hideWindowUI", "True");
                ////settings.Add("MarginLeft", "0.1in");
                ////settings.Add("MarginRight", "0.1in");
                ////settings.Add("MarginTop", "0.1in");
                ////settings.Add("MarginBottom", "0.1in");
                ////settings.Add("PageWidth", "9in");
                ////settings.Add("PageHeight", "13in");
                ////settings.Add("FitWindow", "True");

                ////PdfRenderingExtension _renderingExtension = new DataDynamics.Reports.Rendering.Pdf.PdfRenderingExtension();
                ////FileStreamProvider _provider = new DataDynamics.Reports.Rendering.IO.FileStreamProvider(myFile.Directory, Path.GetFileNameWithoutExtension(myFile.Name));
                ////_reportRuntime.Render(_renderingExtension, _provider, settings);
                ////Response.Clear();
                ////Response.ContentType = "application/pdf";
                ////string filename = Session["UserName"].ToString() + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".pdf";
                ////string FullName = filename.Replace(" ", "");
                ////Response.AddHeader("content-disposition", "attachment; filename=" + FullName);
                ////Response.BinaryWrite(File.ReadAllBytes(exportFile));
                ////myFile.Delete();

                //    }
                //    else
                //    {
                //        lblStatus.Text = "No AWB's Excluded!";
                //        lblStatus.ForeColor = Color.Red;
                //    }
                //}
                //else
                //{
                //    lblStatus.Text = "No AWB's present for the report to process!";
                //    lblStatus.ForeColor = Color.Red;
                //}
                //}
                //}
            }
            catch
            {
                //lblStatus.Text = "Error while printing deal report; Please try again";
                //lblStatus.ForeColor = Color.Red;
                //return;
            }

        }

        //private void WRAXBDetails_LocateDataSource(object sender, LocateDataSourceEventArgs e)
        //{

        //    string dname = e.DataSetName;
        //    //  if (dname.ToLower() == "DataSetValue")    
        //    if (dname == "DataSet1")
        //    {
        //        e.Data = Dataset1;

        //    }
        //    else
        //        if (dname == "DataSet3")
        //        {
        //            e.Data = Dataset3;
        //        }
        //        else
        //        {
        //            e.Data = Dataset2;
        //        }


        //}
    }
}
