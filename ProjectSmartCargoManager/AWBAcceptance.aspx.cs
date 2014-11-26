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
    public partial class AWBAcceptance : System.Web.UI.Page
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
            if (!IsPostBack)
            {
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

                            ddlSCHOrg.DataSource = ds;
                            ddlSCHOrg.DataMember = ds.Tables[0].TableName;
                            ddlSCHOrg.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlSCHOrg.DataTextField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlSCHOrg.DataBind();
                            ddlSCHOrg.Items.Insert(0, new ListItem("Select", "Select"));

                            ddlSCHDest.DataSource = ds;
                            ddlSCHDest.DataMember = ds.Tables[0].TableName;
                            ddlSCHDest.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlSCHDest.DataTextField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlSCHDest.DataBind();
                            ddlSCHDest.Items.Insert(0, new ListItem("Select", "Select"));

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GetCurrencyCode List

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
            Params.SetValue("CustomsAWBAcceptance", i);
            #endregion Parameters

            DataSet ds = new DataSet();
            ds = objBAL.GetCustomRecords(Params);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtTransFrm.Text = ds.Tables[0].Rows[0]["FromCarrier"].ToString();
                txtTransTo.Text = ds.Tables[0].Rows[0]["ToCarrier"].ToString();
                
                string org = ds.Tables[0].Rows[0]["SurfaceOrigin"].ToString();
                ddlOrg.SelectedIndex = ddlOrg.Items.IndexOf(((ListItem)ddlOrg.Items.FindByText(org)));

                string dest = ds.Tables[0].Rows[0]["SurfaceDest"].ToString();
                ddlDest.SelectedIndex = ddlDest.Items.IndexOf(((ListItem)ddlDest.Items.FindByText(dest)));

                txtDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();

                txtProduct.Text = ds.Tables[0].Rows[0]["Product"].ToString();

                txtPriority.Text = ds.Tables[0].Rows[0]["Priority"].ToString();

                txtStatus.Text = ds.Tables[0].Rows[0]["Status"].ToString();

                txtPad.Text = ds.Tables[0].Rows[0]["Pad"].ToString();

                txtCustoms.Text = ds.Tables[0].Rows[0]["EUCustoms"].ToString();

                txtCustStn.Text = ds.Tables[0].Rows[0]["CustomsStation"].ToString();

                txtAccPcs.Text = ds.Tables[0].Rows[0]["AccpPieces"].ToString();
                txtAccWt.Text = ds.Tables[0].Rows[0]["AccpWeight"].ToString();
                txtVol.Text = ds.Tables[0].Rows[0]["AccpVolume"].ToString();
                //txtTotPcs.Text = ds.Tables[0].Rows[0]["Customs"].ToString();
                //txtTotWt.Text = ds.Tables[0].Rows[0]["Customs"].ToString();
                //txtTotVol.Text = ds.Tables[0].Rows[0]["Customs"].ToString();

                string sluld = ds.Tables[0].Rows[0]["SLULD"].ToString();
                string sluldVer = ds.Tables[0].Rows[0]["SLULDVerified"].ToString();

                if (sluld == "true")
                    chkSluld.Checked = true;
                else chkSluld.Checked = false;

                if (sluldVer == "true")
                    chkSluldWt.Checked = true;
                else chkSluldWt.Checked = false;

                txtCrossRef.Text = ds.Tables[0].Rows[0]["CrossReference"].ToString();
                //txtSCHTransTo.Text = ds.Tables[0].Rows[0][""].ToString(); ;
                //SHC org & dest not available

                txtWareHouseLoc.Text = ds.Tables[0].Rows[0]["WHLocation"].ToString();

                txtContainerId.Text = ds.Tables[0].Rows[0]["ContainerId"].ToString();

                txtFlightNo.Text = ds.Tables[0].Rows[0]["FlightNo"].ToString();

                txtOffload.Text = ds.Tables[0].Rows[0]["Offload"].ToString();

                txtRemarks.Text = ds.Tables[0].Rows[0]["HandlingRemarks"].ToString();

                txtLabels.Text = ds.Tables[0].Rows[0]["Labels"].ToString();

                DateTime dt = (DateTime)ds.Tables[0].Rows[0]["DropOffDate"];
                txtDropOff.Text = dt.ToShortDateString();

                txtDropBy.Text = ds.Tables[0].Rows[0]["DroppedBy"].ToString();

                txtArrPort.Text = ds.Tables[0].Rows[0]["AirrivalPort"].ToString();
                txtDestStn.Text = ds.Tables[0].Rows[0]["DestStation"].ToString(); ;
                
                txtImportFlt.Text = ds.Tables[0].Rows[0]["ImportFlight"].ToString();

                string isBonded = ds.Tables[0].Rows[0]["IsBonded"].ToString();
                string partarr = ds.Tables[0].Rows[0]["IsPartArrival"].ToString();

                if (isBonded == "true")
                    chkBonded.Checked = true;
                else chkBonded.Checked = false;

                if (partarr == "true")
                    chkPartArr.Checked = true;
                else chkPartArr.Checked = false;
            }
            grdAcceptanceList.DataSource = ds;
            grdAcceptanceList.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region Parameters
            object[] Params = new object[47];
            int i = 0;

            //0
            string awbnumber = txtAwbPrefix.Text + txtAWBNo.Text;
            Params.SetValue(awbnumber, i);
            i++;

            //1
            Params.SetValue(ddlSCHOrg.SelectedItem.Text, i);
            i++;

            //2
            Params.SetValue(ddlSCHDest.SelectedItem.Text, i);
            i++;

            //3
            Params.SetValue(txtTransFrm.Text, i);
            i++;

            //4
            Params.SetValue(txtTransTo.Text, i);
            i++;

            //5
            Params.SetValue(ddlOrg.SelectedItem.Text, i);
            i++;

            //6
            Params.SetValue(ddlDest.SelectedItem.Text, i);
            i++;

            //7
            Params.SetValue(txtDescription.Text, i);
            i++;

            //8
            Params.SetValue(txtProduct.Text, i);
            i++;

            //9
            Params.SetValue(txtPriority.Text, i);
            i++;

            //10
            Params.SetValue(txtStatus.Text, i);
            i++;

            //11
            Params.SetValue(txtPad.Text, i);
            i++;

            //12
            Params.SetValue(txtCustoms.Text, i);
            i++;

            //13
            Params.SetValue(txtCustStn.Text, i);
            i++;

            //14
            Params.SetValue(txtAccPcs.Text, i);
            i++;

            //15
            Params.SetValue(txtAccWt.Text, i);
            i++;

            //16
            Params.SetValue("Kg", i);
            i++;

            //17
            Params.SetValue(txtVol.Text, i);
            i++;

            //18
            if(chkSluld.Checked==true)
            Params.SetValue("true", i);
            else Params.SetValue("false", i);
            i++;

            //19 
            if(radbtn.SelectedIndex==0)
            Params.SetValue("true", i);
            else Params.SetValue("false", i);
            i++;

            //20
            if(radbtn.SelectedIndex==0)
            Params.SetValue(txtCountourVal.Text, i);
            else Params.SetValue("", i);
            i++;


            //21
            if(radbtn.SelectedItem.Text=="Pallets")
            Params.SetValue("true", i);
            else Params.SetValue("false", i);
            i++;

            //22
            if (radbtn.SelectedItem.Text=="LD2")
                Params.SetValue("true", i);
            else Params.SetValue("false", i);
            i++;

            //23
            if(chkSluldWt.Checked==true)
            Params.SetValue("true", i);
            else Params.SetValue("false", i);
            i++;

            //24 Spcl Handling Code
            Params.SetValue("", i);
            i++;

            //25 
            Params.SetValue(txtCrossRef.Text, i);
            i++;

            //26 Manifest Grp
            Params.SetValue("", i);
            i++;

            //27 Acceptance Date
            Params.SetValue(DateTime.Now, i);
            i++;

            //28
            Params.SetValue(txtWareHouseLoc.Text, i);
            i++;

            //29
            Params.SetValue(txtContainerId.Text, i);
            i++;


            //30
            Params.SetValue(txtFlightNo.Text, i);
            i++;

            //31 Flt Dt
            Params.SetValue(DateTime.Now, i);
            i++;

            //32
            Params.SetValue(txtOffload.Text, i);
            i++;

            //33
            Params.SetValue(txtRemarks.Text, i);
            i++;

            //34
            Params.SetValue(txtLabels.Text, i);
            i++;

            //35
            if(chkCarrHold.Checked==true)
            Params.SetValue("true", i);
            else Params.SetValue("false", i);
            i++;

            //36
            if(chkDocRec.Checked==true)
            Params.SetValue("true", i);
            else Params.SetValue("false", i);
            i++;

            //37 txtDropOff.Text
            Params.SetValue(DateTime.Now, i);
            i++;
            
            //38
            Params.SetValue(txtDropBy.Text, i);
            i++;

            //39
            if(chkBonded.Checked==true)
            Params.SetValue("true", i);
            else Params.SetValue("false", i);
            i++;

            //40
            Params.SetValue(txtArrPort.Text, i);
            i++;

            //41
            Params.SetValue(txtDestStn.Text, i);
            i++;

            //42
            Params.SetValue(txtImportFlt.Text, i);
            i++;

            //43
            Params.SetValue(DateTime.Now, i);
            i++;

            //44
            if(chkPartArr.Checked==true)
            Params.SetValue("true", i);
            else Params.SetValue("false", i);
            i++;

            //45
            Params.SetValue(Session["UserName"].ToString(), i);
            i++;

            //46
            Params.SetValue(DateTime.Now, i);
            i++;



            #endregion Parameters
            bool result = objBAL.SaveCustomsAWBAcceptance(Params);
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
