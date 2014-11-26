using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using System.Drawing;
using Microsoft.Reporting.WebForms;

namespace ProjectSmartCargoManager
{
    public partial class CTMList : System.Web.UI.Page
    {

        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtULD = new DataTable("CTMList_dtULD");
        DataTable dtAWB = new DataTable("CTMList_dtAWB");
    
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblAirport.Text = Session["Station"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region List Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            DataSet dsVal = new DataSet("CTMList_dsVal");
            string[] paramname = new string[12];
            object[] paramvalue = new object[12];
            SqlDbType[] paramtype = new SqlDbType[12];
            try
            {

                gvCTM.DataSource = null;
                gvCTM.DataBind();

                
                paramname[0] = "FltNofrmPrefix";
                paramname[1] = "FromFltNo";
                paramname[2] = "FromFltDate";
                paramname[3] = "FltNotoPrefix";
                paramname[4] = "ToFltNo";
                paramname[5] = "ToFltDate";
                paramname[6] = "Station";
                paramname[7] = "AWBPrefix";
                paramname[8] = "AWBNo";
                paramname[9] = "ULDNo";
                paramname[10] = "CTMType";
                paramname[11] = "CTMRef";

                
                paramvalue[0] = txtFromPre.Text.Trim();// +txtFromFlight.Text.Trim();
                paramvalue[1] = txtFromPre.Text.Trim() + txtFromFlight.Text.Trim();//DateTime.Parse(txtFDate.Text.Trim());
                //if (ddlCTM.SelectedIndex == 0)
                //{
                //    paramvalue[2] = "NA";
                //}
                //else
                //{
                //if (txtFDate.Text != "")
                //{
                //    paramvalue[2] = DateTime.ParseExact(txtFDate.Text.Trim(), "dd/MM/yyyy", null);
                //}
                //else
                {
                    paramvalue[2] = txtFDate.Text.Trim();

                }
                    //}

                paramvalue[3] = txtToPre.Text.Trim();//lblAirport.Text.Trim();
                //if (txtAWBNo.Text.Trim() == "")
                //{
                //    paramvalue[4] = "NA";
                //}
                //else
                //{
                paramvalue[4] = txtToPre.Text.Trim() + txtToFlight.Text.Trim();//txtAWBPre.Text.Trim() + txtAWBNo.Text.Trim();
                //}
                //if (txtCTMRef.Text.Trim() == "")
                //{
                //    paramvalue[5] = "NA";
                //}
                //else
                //{
                //if (txtTDate.Text != "")
                //{
                //    paramvalue[5] = DateTime.ParseExact(txtTDate.Text.Trim(), "dd/MM/yyyy", null);//txtCTMRef.Text.Trim();

                //}
                //else
                {
                    paramvalue[5] = txtTDate.Text.Trim();//txtCTMRef.Text.Trim();

                }//}
                paramvalue[6] = Session["Station"];
                paramvalue[7] = txtAWBPre.Text.Trim();
                paramvalue[8] = txtAWBPre.Text.Trim() + txtAWBNo.Text.Trim();
                paramvalue[9] = txtULDNo.Text.Trim();

                string CTMType = "";

                if (ddlCTM.SelectedItem.Text == "Select")
                {
                    CTMType = "";
                }
                else
                {
                    CTMType = ddlCTM.SelectedItem.Text.Trim();
                }
                paramvalue[10] = CTMType;
                paramvalue[11] = txtCTMRef.Text.Trim();


                
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.VarChar;
                paramtype[6] = SqlDbType.VarChar;
                paramtype[7] = SqlDbType.VarChar;
                paramtype[8] = SqlDbType.VarChar;
                paramtype[9] = SqlDbType.VarChar;
                paramtype[10] = SqlDbType.VarChar;
                paramtype[11] = SqlDbType.VarChar;

                SQLServer sq = new SQLServer(Global.GetConnectionString());
                Session["CTM"] = null;
                dsVal = sq.SelectRecords("SPGetCTMList", paramname, paramvalue, paramtype);
                //DataSet dsVal1 = sq.SelectRecords("spgetlistforCTMULD", paramname, paramvalue, paramtype);
                Session["CTM"] = dsVal;
                //Session["CTMULD"] = dsVal1;
                if (dsVal != null && dsVal.Tables.Count > 0)
                {
                    if (dsVal.Tables[0].Rows.Count > 0 || dsVal.Tables[1].Rows.Count > 0)
                    {
                        gvCTM.DataSource = dsVal.Tables[0];
                        gvCTM.DataBind();
                        lblMsg.Text = "";
                        if (txtCTMRef.Text.Trim() == "")
                        {
                            //btnSave.Visible = true;
                        }
                   
                        grdULDDetails.DataSource = dsVal.Tables[1];
                        grdULDDetails.DataBind();
                        lblMsg.Text = "";
                        if (txtCTMRef.Text.Trim() == "")
                        {
                            //btnSave.Visible = true;
                        }
                    }
                    else
                    {
                        lblMsg.Text = "No Record Available";
                        lblMsg.ForeColor = Color.Red;
                        grdULDDetails.DataSource = null;
                        grdULDDetails.DataBind();
                        gvCTM.DataSource = null;
                        gvCTM.DataBind();
                    }
                }
                //if (dsVal1 != null && dsVal1.Tables.Count > 0)
                //{
                //    if (dsVal.Tables[1].Rows.Count > 0)
                //    {
                //        grdULDDetails.DataSource = dsVal;
                //        grdULDDetails.DataBind();
                //        if (txtCTMRef.Text.Trim() == "")
                //        {
                //            //btnSave.Visible = true;
                //        }
                //    }
                //    else
                //    {
                //        lblMsg.Text = "No Record Available for ULDs";
                //        lblMsg.ForeColor = Color.Red;
                //    }
                //}
                //else
                //{
                //    lblMsg.Text = "Error in retrieving data!";
                //    lblMsg.ForeColor = Color.Red;
                //}
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error in Listing!";
                lblMsg.ForeColor = Color.Red;
            }
            finally
            {
                if (dsVal != null)
                {
                    dsVal.Dispose();
                }
                paramname = null;
                paramtype = null;
                paramvalue = null;
            }
        }
        #endregion

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CTMList.aspx",false);
        }
        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            if (dtULD != null)
                e.DataSources.Add(new ReportDataSource("CTM_DataTableULD", dtULD));
            if (dtAWB != null)
                e.DataSources.Add(new ReportDataSource("CTM_dtAWB", dtAWB));

        }

        private void RenderReport(DataTable dt)
        {

            try
            {
                //A method that returns a collection for our report


                ReportViewer1.ProcessingMode = ProcessingMode.Local;

                LocalReport rep1 = ReportViewer1.LocalReport;

                rep1.ReportPath = Server.MapPath("/Reports/CTM.rdlc");


                rds1.Name = "CTM_DataTable1";
                rds1.Value = dt;
                rep1.DataSources.Add(rds1);


                ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemsSubreportProcessingEventHandler);

                string reportType = "PDF";

                string mimeType;

                string encoding;

                string fileNameExtension;



                string deviceInfo =

               "<DeviceInfo>" +

               "  <OutputFormat>PDF</OutputFormat>" +

               "  <PageWidth>10.76in</PageWidth>" +

               "  <PageHeight>11in</PageHeight>" +

               //"  <MarginTop>0.01in</MarginTop>" +

               //"  <MarginLeft>0.01in</MarginLeft>" +

               //"  <MarginRight>0.01in</MarginRight>" +

               //"  <MarginBottom>0.01in</MarginBottom>" +

               "</DeviceInfo>";



                Warning[] warnings;

                string[] streams;

                byte[] renderedBytes;



                //Render the report

                renderedBytes = rep1.Render(

                    reportType,

                    deviceInfo,

                    out mimeType,

                    out encoding,

                    out fileNameExtension,

                    out streams,

                    out warnings);



                //Clear the response stream and write the bytes to the outputstream

                //Set content-disposition to "attachment" so that user is prompted to take an action

                //on the file (open or save)

                Response.Clear();

                Response.ContentType = mimeType;

                Response.AddHeader("content-disposition", "attachment; filename=CTM." + fileNameExtension);

                Response.BinaryWrite(renderedBytes);


                //Response.End();
            }
            catch (Exception ex)
            {

            }


        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtCTM = new DataTable("CTMList_btnPrint_dtCTM");
            DataTable dtAWBnew = new DataTable("CTMList_btnPrint_dtAWBnew");
            DataTable dtULDnew = new DataTable("CTMList_btnPrint_dtULDnew");
            DataSet dsVal = new DataSet("CTMList_btnPrint_dsVal");
            DataTable expdt = new DataTable("CTMList_btnPrint_expdtULD");
            DataTable tempdt = new DataTable("CTMList_btnPrint_tempdtULD");
            try
            {

                if (Session["CTM"] == null)
                    return;

                dsVal = (DataSet)Session["CTM"];// Session["CTM"];
                //img for report
                System.IO.MemoryStream Logo = null;
                try
                {

                    Logo = CommonUtility.GetImageStream(Page.Server);
                }
                catch (Exception ex)
                {
                    Logo = new System.IO.MemoryStream();
                }
                dtAWB = dsVal.Tables[0];
                dtULD = dsVal.Tables[1];

                dtAWBnew = dtAWB.Clone();
                dtULDnew = dtULD.Clone();

                dtCTM.Columns.Add("CTMNo");
                dtCTM.Columns.Add("Airport");
                dtCTM.Columns.Add("Date");
                dtCTM.Columns.Add("TransferredTo");
                dtCTM.Columns.Add("Transferredby");
                dtCTM.Columns.Add("ReceivedBy");
                dtCTM.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));

                for (int i = 0; i < gvCTM.Rows.Count; i++)
                {
                    CheckBox chkBox = (CheckBox)gvCTM.Rows[i].FindControl("chkgvCTM");

                    if (chkBox.Checked)
                    {

                        dtCTM.Rows.Add(dtAWB.Rows[i]["CTMRefNo"].ToString(), lblAirport.Text, Session["IT"].ToString(), dtAWB.Rows[i]["ToCarrier"].ToString().ToUpper(), dtAWB.Rows[i]["FromCarrier"].ToString().ToUpper(), dtAWB.Rows[i]["ToCarrier"].ToString().ToUpper(), Logo.ToArray());

                        string AWBNo = ((Label)gvCTM.Rows[i].FindControl("lblAWB")).Text;

                        expdt = dsVal.Tables[0].Select("AWBNo='" + AWBNo + "'").CopyToDataTable();
                        tempdt = expdt.Copy();
                        foreach (DataRow dr in tempdt.Rows)
                        {
                            // dtAWB.Rows.Add(dr);  
                            dtAWBnew.ImportRow(dr);

                        }
                        dtAWB = dtAWBnew.Copy();
                    }

                }


                for (int i = 0; i < grdULDDetails.Rows.Count; i++)
                {
                    CheckBox chkBox = (CheckBox)grdULDDetails.Rows[i].FindControl("checkULD");

                    if (chkBox.Checked)
                    {

                        dtCTM.Rows.Add(dtAWB.Rows[i]["CTMRefNo"].ToString(), lblAirport.Text, Session["IT"].ToString(), dtAWB.Rows[i]["ToCarrier"].ToString().ToUpper(), dtAWB.Rows[i]["FromCarrier"].ToString().ToUpper(), dtAWB.Rows[i]["ToCarrier"].ToString().ToUpper(), Logo.ToArray());

                        string ULDNo = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Text;

                        expdt = dsVal.Tables[1].Select("ULDNo='" + ULDNo + "'").CopyToDataTable();
                        tempdt = expdt.Copy();
                        foreach (DataRow dr in tempdt.Rows)
                        {
                            // dtAWB.Rows.Add(dr);
                            dtULDnew.ImportRow(dr);

                        }
                        dtULD = dtULDnew.Copy();
                    }

                }

                RenderReport(dtCTM);

            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (dtCTM != null)
                {
                    dtCTM.Dispose();
                }
                if (dtAWBnew != null)
                {
                    dtAWBnew.Dispose();
                }
                if (dtULDnew != null)
                {
                    dtULDnew.Dispose();
                }
                if (expdt != null)
                {
                    expdt.Dispose();
                }
                if (tempdt != null)
                {
                    tempdt.Dispose();
                }
                if (dsVal != null)
                {
                    dsVal.Dispose();
                }
                
            }

        }
        protected void gvCTM_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void gvCTM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dss1 = new DataSet("CTMList_gvCTMPageIndex_dss1");
            DataTable ds = new DataTable("CTMList_gvCTMPageIndex_ds");
            try
            {
                dss1 = (DataSet)Session["CTM"];
                ds = dss1.Tables[0];
                gvCTM.PageIndex = e.NewPageIndex;
                gvCTM.DataSource = ds;
                gvCTM.DataBind();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (ds!= null)
                {
                    ds.Dispose();
                }
                if (dss1 != null)
                {
                    dss1.Dispose();
                }
            }
        }

        protected void grdULDDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataSet dss = new DataSet("CTMList_grdULDPageIndex_dss");
            DataTable ds = new DataTable("CTMList_grdULDPageIndex_ds2");
            try
            {
                dss = (DataSet)Session["CTM"];
                ds = dss.Tables[1];
                grdULDDetails.PageIndex = e.NewPageIndex;
                grdULDDetails.DataSource = ds;
                grdULDDetails.DataBind();



            }
            catch (Exception ex)
            {

            }
            finally 
            {
                if (dss != null)
                { dss.Dispose(); }
                if (ds != null)
                {
                    ds.Dispose();
                }
            }
        }

        protected void grdULDDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        }

     
    }


