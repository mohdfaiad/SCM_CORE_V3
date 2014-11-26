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
    public partial class Generaldeclarationoutwardinward : System.Web.UI.Page
    {
        #region Variable
        SQLServer db = new SQLServer(Global.GetConnectionString());
        BALGenDeclaration objBAL = new BALGenDeclaration();

        private DataSet Dataset1 = new DataSet();
        private DataSet Dataset2 = new DataSet();
        private DataSet Dataset3 = new DataSet();
        #endregion variable

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAirportCode();
                
                DataTable dt = new DataTable();
                dt.Columns.Add("PLACE", typeof(string));
                dt.Columns.Add("TOTALCREW", typeof(string));
                DataRow dr1 = dt.NewRow();
                dr1["PLACE"] = string.Empty;
                dr1["TOTALCREW"] = string.Empty;
                dt.Rows.Add(dr1);

                grdRouting.DataSource = dt;
                grdRouting.DataBind();
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
                            ddlArrivalAt.DataSource = ds;
                            ddlArrivalAt.DataMember = ds.Tables[0].TableName;
                            ddlArrivalAt.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlArrivalAt.DataTextField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlArrivalAt.DataBind();
                            ddlArrivalAt.Items.Insert(0, new ListItem("Select", "Select"));

                            ddlDepFrm.DataSource = ds;
                            ddlDepFrm.DataMember = ds.Tables[0].TableName;
                            ddlDepFrm.DataValueField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlDepFrm.DataTextField = ds.Tables[0].Columns["AirportCode"].ColumnName;
                            ddlDepFrm.DataBind();
                            ddlDepFrm.Items.Insert(0, new ListItem("Select", "Select"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        # endregion GetCurrencyCode List

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PLACE", typeof(string));
            dt.Columns.Add("TOTALCREW", typeof(string));
            
            for (int i = 0; i < grdRouting.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["PLACE"] = ((TextBox)grdRouting.Rows[i].Cells[1].FindControl("txtPlace")).Text;
                dr["TOTALCREW"] = ((TextBox)grdRouting.Rows[i].Cells[1].FindControl("txtCrewNo")).Text;
                dt.Rows.Add(dr);
            }
            DataRow dr1 = dt.NewRow();
            dr1["PLACE"] = string.Empty;
            dr1["TOTALCREW"] = string.Empty;
            dt.Rows.Add(dr1);

            grdRouting.DataSource = dt;
            grdRouting.DataBind();
        }
        
        protected void btnDel_Click(object sender, EventArgs e)
        {
            if (grdRouting.Rows.Count == 1)
                return;
            
            DataTable dt=new DataTable();
            dt.Columns.Add("PLACE", typeof(string));
            dt.Columns.Add("TOTALCREW", typeof(string));
            
            for (int i = 0; i < grdRouting.Rows.Count-1; i++)
            {
                DataRow dr = dt.NewRow();
                dr["PLACE"] = ((TextBox)grdRouting.Rows[i].Cells[1].FindControl("txtPlace")).Text;
                dr["TOTALCREW"] = ((TextBox)grdRouting.Rows[i].Cells[1].FindControl("txtCrewNo")).Text;
                dt.Rows.Add(dr);
            }
            grdRouting.DataSource = dt;
            grdRouting.DataBind();
                
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int srno=0;
            #region Parameters For Gen Declartion
            object[] Params = new object[17];
            int i = 0;

            //1
            Params.SetValue(txtOperator.Text, i);
            i++;

            //2
            Params.SetValue(txtNationality.Text, i);
            i++;

            //3
            Params.SetValue(txtFlightNo.Text, i);
            i++;

            //4
            Params.SetValue(txtFltDt.Text, i);
            i++;

            //5
            Params.SetValue(ddlDepFrm.SelectedItem.Text, i);
            i++;

            //6
            Params.SetValue(ddlArrivalAt.SelectedItem.Text, i);
            i++;

            //7
            Params.SetValue(txtEmbarking.Text, i);
            i++;

            //8
            Params.SetValue(txtDepThrough.Text, i);
            i++;

            //9
            Params.SetValue(txtDisembarking.Text, i);
            i++;

            //10
            Params.SetValue(txtArrThrough.Text, i);
            i++;

            //11
            Params.SetValue(txtSED.Text, i);
            i++;

            //12
            Params.SetValue(txtAWBs.Text, i);
            i++;

            //13
            Params.SetValue(txtHealthDeclaration.Text, i);
            i++;

            //14
            Params.SetValue(txtOtherCondition.Text, i);
            i++;

            //15
            Params.SetValue(txtTreatment.Text, i);
            i++;

            //16
            Params.SetValue(DateTime.Now, i);
            i++;

            //17
            Params.SetValue(Session["UserName"].ToString(), i);
            i++;

            #endregion Parameters  For Gen Declartion
            
            DataSet ds= objBAL.SaveCustomsDeclaration(Params);
            if (ds.Tables[0].Rows.Count>0)
            {
                lblStatus.Text = "Record Added Successfully";
                lblStatus.ForeColor = Color.Green;
                srno = int.Parse((ds.Tables[0].Rows[0][0].ToString()));
            }
            else
            {
                lblStatus.Text = "Record Insertion Failed";
                lblStatus.ForeColor = Color.Red;
                return;
            }

            for (int k = 0; k < grdRouting.Rows.Count; k++)
            {
                object[] ParamsForPlace = new object[3];
                int j = 0;

                //1
                ParamsForPlace.SetValue(srno, j);
                j++;

                //2
                string place = ((TextBox)(grdRouting.Rows[k].FindControl("txtPlace"))).Text.ToString();
                ParamsForPlace.SetValue(place, j);
                j++;

                //3
                string crewno = ((TextBox)(grdRouting.Rows[k].FindControl("txtCrewNo"))).Text.ToString();
                ParamsForPlace.SetValue(crewno, j);
                j++;
                DataSet ds1 = objBAL.SaveCustomsPlaceAndCrew(ParamsForPlace);
                
            }           
        }

        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            object[] Params = new object[3];
            int i = 0;

            //1
            Params.SetValue(txtFlightNo.Text, i);
            i++;

            //2
            Params.SetValue(txtFltDt.Text, i);
            i++;

           
            DataSet ds1 = objBAL.GetGenDeclaration(Params);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                string DepFrom = ds1.Tables[0].Rows[0]["DepartureFrom"].ToString();
                string ArrAt = ds1.Tables[0].Rows[0]["ArrivalAt"].ToString();
                ddlDepFrm.SelectedIndex = ddlDepFrm.Items.IndexOf(((ListItem)ddlDepFrm.Items.FindByText(DepFrom)));
                ddlArrivalAt.SelectedIndex = ddlArrivalAt.Items.IndexOf(((ListItem)ddlArrivalAt.Items.FindByText(ArrAt)));

                txtOperator.Text = ds1.Tables[0].Rows[0]["Operator"].ToString();

                txtNationality.Text = ds1.Tables[0].Rows[0]["NationalityMarks"].ToString();

                txtEmbarking.Text = ds1.Tables[0].Rows[0]["DepartureEmbarking"].ToString();
                txtDepThrough.Text = ds1.Tables[0].Rows[0]["DepartureThrough"].ToString();

                txtDisembarking.Text = ds1.Tables[0].Rows[0]["ArrivalDisembarking"].ToString();
                txtArrThrough.Text = ds1.Tables[0].Rows[0]["ArrivalThrough"].ToString();

                txtSED.Text = ds1.Tables[0].Rows[0]["SEDs"].ToString();
                txtAWBs.Text = ds1.Tables[0].Rows[0]["AWBs"].ToString();

                txtHealthDeclaration.Text = ds1.Tables[0].Rows[0]["DeclarationHealth"].ToString();
                txtOtherCondition.Text = ds1.Tables[0].Rows[0]["OtherCondition"].ToString();
                txtTreatment.Text = ds1.Tables[0].Rows[0]["DetailsOfTreatment"].ToString();
            }
            if (ds1.Tables[1].Rows.Count > 0)
            {
                grdRouting.DataSource = ds1.Tables[1];
                grdRouting.DataBind();
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
                ////_reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/CBPForm7507.rdlx")));

                //DataTable dtable = dset.Tables[dset.Tables.Count - 4];
                //DataTable Dt = dset.Tables[dset.Tables.Count - 1];
                //Dataset1.Tables.Add(Dt.Copy());
                //Dataset2.Tables.Add(dtable.Copy());
                ////DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef);
                // _reportRuntime.LocateDataSource += WRAXBDetails_LocateDataSource;
                string exportFile = System.IO.Path.GetTempFileName() + ".pdf";
                System.IO.FileInfo myFile = new System.IO.FileInfo(exportFile);
                System.Collections.Specialized.NameValueCollection settings = new System.Collections.Specialized.NameValueCollection();
                settings.Add("hideToolbar", "True");
                settings.Add("hideMenubar", "True");
                settings.Add("hideWindowUI", "True");
                settings.Add("MarginLeft", "0.1in");
                settings.Add("MarginRight", "0.1in");
                settings.Add("MarginTop", "0.1in");
                settings.Add("MarginBottom", "0.1in");
                settings.Add("PageWidth", "9in");
                settings.Add("PageHeight", "13in");
                settings.Add("FitWindow", "True");

                ////PdfRenderingExtension _renderingExtension = new DataDynamics.Reports.Rendering.Pdf.PdfRenderingExtension();
                ////FileStreamProvider _provider = new DataDynamics.Reports.Rendering.IO.FileStreamProvider(myFile.Directory, Path.GetFileNameWithoutExtension(myFile.Name));
                ////_reportRuntime.Render(_renderingExtension, _provider, settings);
                Response.Clear();
                Response.ContentType = "application/pdf";
                string filename = Session["UserName"].ToString() + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".pdf";
                string FullName = filename.Replace(" ", "");
                Response.AddHeader("content-disposition", "attachment; filename=" + FullName);
                Response.BinaryWrite(File.ReadAllBytes(exportFile));
                myFile.Delete();




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
