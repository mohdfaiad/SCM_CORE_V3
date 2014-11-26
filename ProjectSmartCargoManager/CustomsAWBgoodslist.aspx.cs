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
    public partial class CustomsAWBgoodslist : System.Web.UI.Page
    {
        #region Variable
        SQLServer db = new SQLServer(Global.GetConnectionString());
        BALCustomsAWBInfo objBAL = new BALCustomsAWBInfo();

        private DataSet Dataset1 = new DataSet();
        private DataSet Dataset2 = new DataSet();
        private DataSet Dataset3 = new DataSet();
        #endregion variable

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    GetAirportCode();
                    GetCountryCode();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
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
                            ddlOrg.DataSource = ds;
                            ddlOrg.DataMember = ds.Tables[0].TableName;
                            ddlOrg.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlOrg.DataTextField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlOrg.DataBind();
                            ddlOrg.Items.Insert(0, new ListItem("Select", "Select"));

                            ddlDest.DataSource = ds;
                            ddlDest.DataMember = ds.Tables[0].TableName;
                            ddlDest.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlDest.DataTextField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlDest.DataBind();
                            ddlDest.Items.Insert(0, new ListItem("Select", "Select"));

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GetCurrencyCode List

        #region Get Country Code
        private void GetCountryCode()
        {
            try
            {
                DataSet ds = objBAL.GetCountryCodes();
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            countrycode.DataSource = ds;
                            countrycode.DataMember = ds.Tables[0].TableName;
                            countrycode.DataValueField = ds.Tables[0].Columns["CountryCode"].ColumnName;

                            countrycode.DataTextField = ds.Tables[0].Columns["CountryCode"].ColumnName;
                            countrycode.DataBind();
                            countrycode.Items.Insert(0, new ListItem("Select", "Select"));

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        protected void btnListAgentStock_Click(object sender, EventArgs e)
        {
            if (txtAwbPrefix.Text == "" || txtAWBNo.Text == "")
            {
                lblStatus.Text = "Enter AWB Number";
                lblStatus.ForeColor = Color.Red;
                return;
            }


            #region Parameters
            object[] Params = new object[2];
            int i = 0;

            //1
            string awbnumber = txtAwbPrefix.Text + txtAWBNo.Text;
            Params.SetValue(awbnumber, i);
            i++;

            //2
            Params.SetValue("CustomsGoodsList", i);
            #endregion Parameters

            DataSet ds = new DataSet();
            ds = objBAL.GetCustomRecords(Params);
            //Session["GoodsList"] = ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtHouse.Text = ds.Tables[0].Rows[0]["HouseAWBNo"].ToString();

                string ctrycode = ds.Tables[0].Rows[0]["CountryCode"].ToString();
                countrycode.SelectedIndex = countrycode.Items.IndexOf(((ListItem)countrycode.Items.FindByText(ctrycode)));

                txtFlightNo.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString();

                DateTime dt = (DateTime)(ds.Tables[0].Rows[0]["FlightDate"]);
                txtFlightFromdate.Text = dt.ToShortDateString();

                string org = ds.Tables[0].Rows[0]["Origin"].ToString();
                ddlOrg.SelectedIndex = ddlOrg.Items.IndexOf(((ListItem)ddlOrg.Items.FindByText(org)));

                string dest = ds.Tables[0].Rows[0]["Destination"].ToString();
                ddlDest.SelectedIndex = ddlDest.Items.IndexOf(((ListItem)ddlDest.Items.FindByText(dest)));

                txtCustoms.Text = ds.Tables[0].Rows[0]["Customs"].ToString();

                string arrStatus = ds.Tables[0].Rows[0]["Arrival"].ToString();
                ddlArrState.SelectedIndex = ddlArrState.Items.IndexOf(((ListItem)ddlArrState.Items.FindByText(arrStatus)));

                txtOffload.Text = ds.Tables[0].Rows[0]["Offload"].ToString(); ;

                txtPart.Text = ds.Tables[0].Rows[0]["Part"].ToString();

                txtDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();

                txtConsole.Text = ds.Tables[0].Rows[0]["Consol"].ToString();

                txtQueryStat.Text = ds.Tables[0].Rows[0]["QueryStatus"].ToString();

                txtShipper.Text = ds.Tables[0].Rows[0]["Shipper"].ToString();

                txtConsignee.Text = ds.Tables[0].Rows[0]["Consignee"].ToString();

                txtCustVal.Text = ds.Tables[0].Rows[0]["CustomsValue"].ToString();

                txtCurr.Text = ds.Tables[0].Rows[0]["Currency"].ToString();

                txtFDA.Text = ds.Tables[0].Rows[0]["FDA"].ToString();

                txtPcs.Text = ds.Tables[0].Rows[0]["Pieces"].ToString();
                txtWt.Text = ds.Tables[0].Rows[0]["Weight"].ToString(); ;

                txtShed.Text = ds.Tables[0].Rows[0]["Shed"].ToString();
                txtAgent.Text = ds.Tables[0].Rows[0]["Agent"].ToString();

                string OnwardCarr = ds.Tables[0].Rows[0]["OnwardCarrier"].ToString();
                //ddlOnwardCarr.SelectedIndex = ddlOnwardCarr.Items.IndexOf(((ListItem)ddlOnwardCarr.Items.FindByText(OnwardCarr)));
                txtOnwardCarr.Text = OnwardCarr;

                string TransitCtrl = ds.Tables[0].Rows[0]["TransitControl"].ToString();
                //ddlTransCtrl.SelectedIndex = ddlTransCtrl.Items.IndexOf(((ListItem)ddlTransCtrl.Items.FindByText(TransitCtrl)));
                txtTransCtrl.Text = TransitCtrl;

                txtBond.Text = ds.Tables[0].Rows[0]["Bond"].ToString();
            }
            grdGoodsList.DataSource = ds;
            grdGoodsList.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region Parameters
            object[] Params = new object[31];
            int i = 0;

            //0
            string awbnumber = txtAwbPrefix.Text + txtAWBNo.Text;
            Params.SetValue(awbnumber, i);
            i++;

            //1
            Params.SetValue(txtHouse.Text, i);
            i++;

            //2
            Params.SetValue(countrycode.SelectedItem.Text, i);
            i++;

            //3
            Params.SetValue(ddlOrg.SelectedItem.Text, i);
            i++;

            //4
            Params.SetValue(ddlDest.SelectedItem.Text, i);
            i++;

            //5
            Params.SetValue(txtCustoms.Text, i);
            i++;

            //6
            Params.SetValue(txtPcs.Text, i);
            i++;

            //7
            Params.SetValue(txtWt.Text, i);
            i++;

            //8
            Params.SetValue(txtDescription.Text, i);
            i++;

            //9
            Params.SetValue(txtConsole.Text, i);
            i++;

            //10
            Params.SetValue(txtQueryStat.Text, i);
            i++;

            //11
            Params.SetValue(txtShipper.Text, i);
            i++;

            //12
            Params.SetValue(txtConsignee.Text, i);
            i++;

            //13
            Params.SetValue(txtCustVal.Text, i);
            i++;

            //14
            Params.SetValue(txtCurr.Text, i);
            i++;

            //15
            Params.SetValue(txtFDA.Text, i);
            i++;

            //16
            Params.SetValue(txtFlightNo.Text, i);
            i++;

            //17
            Params.SetValue(txtFlightFromdate.Text, i);
            i++;

            //18
            Params.SetValue(txtPart.Text, i);
            i++;

            //19 Flt Pcs
            Params.SetValue(txtPcs.Text, i);
            i++;

            //20 Flt wt
            Params.SetValue(txtWt.Text, i);
            i++;

            //21
            Params.SetValue(ddlArrState.SelectedItem.Text, i);
            i++;

            //22
            Params.SetValue(txtOffload.Text, i);
            i++;

            //23
            Params.SetValue(txtShed.Text, i);
            i++;

            //24
            Params.SetValue(txtAgent.Text, i);
            i++;

            //25 ddlTransCtrl.SelectedItem.Text
            Params.SetValue(txtTransCtrl.Text, i);
            i++;

            //26 ddlOnwardCarr.SelectedItem.Text
            Params.SetValue(txtOnwardCarr.Text, i);
            i++;

            //27
            Params.SetValue(txtBond.Text, i);
            i++;

            //28
            if (chkDel.Checked == true)
            Params.SetValue("true", i);
            else Params.SetValue("false", i);
            i++;

            //29 ammended reason
            Params.SetValue("ammended reason", i);
            i++;

           
            //30
            Params.SetValue(Session["UserName"].ToString(), i);
            i++;

            #endregion Parameters
            bool result = objBAL.SaveCustomsGoodsList(Params);
            if (result == true)
            {
                lblStatus.Text = "Record Added Successfully";
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                lblStatus.Text = "Record Insertion Failed";
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
                ////_reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/CBPForm7512.rdlx")));

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
