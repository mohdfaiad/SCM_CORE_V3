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
using System.Data;


namespace ProjectSmartCargoManager
{
    public partial class frmCTM : System.Web.UI.Page
    {
        ReportDataSource rds1 = new ReportDataSource();
        ReportDataSource rds2 = new ReportDataSource();
        DataTable dtTable1 = new DataTable();
        DataTable dtULD = new DataTable();
  

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (!IsPostBack)
            {
                try
                {
                    txtFDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    txtCreateDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    lblAirport.Text = Session["Station"].ToString().Trim();
                    btnSave.Visible = false;

                }
                catch (Exception ex)
                {

                }


            }
        }

        #region List Click
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                gvCTM.DataSource = null;
                gvCTM.DataBind();
                btnSave.Visible = false;
                string[] paramname = new string[6];
                paramname[0] = "From";
                paramname[1] = "FromDate";
                paramname[2] = "BoundType";
                paramname[3] = "Airportcode";
                paramname[4] = "AWBNo";
                paramname[5] = "CTMRefNo";

                object[] paramvalue = new object[6];
                paramvalue[0] = txtFromPre.Text.Trim() + txtFromFlight.Text.Trim();
                paramvalue[1] = DateTime.Parse(txtFDate.Text.Trim());
                if (ddlCTM.SelectedIndex == 0)
                {
                    paramvalue[2] = "NA";
                }
                else
                {
                    paramvalue[2] = ddlCTM.SelectedItem.Text.ToString();
                }
                paramvalue[3] = lblAirport.Text.Trim();
                if (txtAWBNo.Text.Trim() == "")
                {
                    paramvalue[4] = "NA";
                }
                else
                {
                    paramvalue[4] = txtAWBPre.Text.Trim() + txtAWBNo.Text.Trim();
                }
                if (txtCTMRef.Text.Trim() == "")
                {
                    paramvalue[5] = "NA";
                }
                else
                {
                    paramvalue[5] = txtCTMRef.Text.Trim();
                }

                SqlDbType[] paramtype = new SqlDbType[6];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.DateTime;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.VarChar;

                SQLServer sq = new SQLServer(Global.GetConnectionString());
                Session["CTM"] = null;
                DataSet dsVal = sq.SelectRecords("spgetlistforCTM", paramname, paramvalue, paramtype);
                DataSet dsVal1 = sq.SelectRecords("spgetlistforCTMULD", paramname, paramvalue, paramtype);
                Session["CTM"] = dsVal;
                Session["CTMULD"]= dsVal1;
                if (dsVal != null && dsVal.Tables.Count > 0)
                {
                    if (dsVal.Tables[0].Rows.Count > 0)
                    {
                        gvCTM.DataSource = dsVal;
                        gvCTM.DataBind();
                        if (txtCTMRef.Text.Trim() == "")
                        {
                            btnSave.Visible = true;
                        }
                    }
                    else
                    {
                        lblMsg.Text = "No Record Available for AWBs";
                        lblMsg.ForeColor = Color.Red;
                    }
                }
                if (dsVal1 != null && dsVal1.Tables.Count > 0)
                {
                    if (dsVal1.Tables[0].Rows.Count > 0)
                    {
                        grdULDDetails.DataSource = dsVal1;
                        grdULDDetails.DataBind();
                        if (txtCTMRef.Text.Trim() == "")
                        {
                            btnSave.Visible = true;
                        }
                    }
                    else
                    {
                        lblMsg.Text = "No Record Available for ULDs";
                        lblMsg.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblMsg.Text = "Error in retrieving data!";
                    lblMsg.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error in Listing!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Save Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int chkavail = 0;
                for (int i = 0; i < gvCTM.Rows.Count; i++)
                {
                    if (((CheckBox)gvCTM.Rows[i].FindControl("chkgvCTM")).Checked == true)
                    {
                        chkavail = 1;
                    }
                    
                }
                for (int i = 0; i < grdULDDetails.Rows.Count; i++)
                {
                    if (((CheckBox)grdULDDetails.Rows[i].FindControl("checkULD")).Checked == true)
                    {
                        chkavail = 1;
                    }
                }
                if (chkavail == 0)
                {
                    lblMsg.Text = "Select Row for Save";
                    lblMsg.ForeColor = Color.Red;
                    return;
                }
                if (ddlCTM.SelectedIndex == 0)
                {
                    lblMsg.Text = "Please Select CTM Type";
                    lblMsg.ForeColor = Color.Red;
                    return;
                }
                string[] paramname = new string[11];
                paramname[0] = "CTMRefNo";
                paramname[1] = "CTM";
                paramname[2] = "AirportCode";
                paramname[3] = "FromCarrier";
                paramname[4] = "FromFlDate";
                paramname[5] = "ToCarrier";
                paramname[6] = "ToFlDate";
                paramname[7] = "CreatedOn";
                paramname[8] = "CreatedBy";
                paramname[9] = "UpdatedOn";
                paramname[10] = "UpdatedBy";

                
                object[] paramvalue = new object[11];
                paramvalue[0] = txtCTMRef.Text.Trim();

                paramvalue[1] = ddlCTM.SelectedItem.Text;

                paramvalue[2] = lblAirport.Text;
                paramvalue[3] = txtFromPre.Text.Trim() + txtFromFlight.Text.Trim();
                //paramvalue[4] = DateTime.ParseExact(txtFDate.Text.Trim(),"MM/dd/yyyy",null);
                paramvalue[4] = DateTime.ParseExact(txtFDate.Text.Trim(),"M/d/yyyy",null);
                paramvalue[5] = txtToPre.Text.Trim() + txtToFlight.Text.Trim();
                paramvalue[6] = DateTime.ParseExact(txtTDate.Text.Trim(),"M/d/yyyy",null);
                paramvalue[7] = Session["IT"].ToString();
                paramvalue[8] = Session["UserName"].ToString();
                paramvalue[9] = Session["IT"].ToString();
                paramvalue[10] = Session["UserName"].ToString();

                SqlDbType[] paramtype = new SqlDbType[11];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                paramtype[4] = SqlDbType.DateTime;
                paramtype[5] = SqlDbType.VarChar;
                paramtype[6] = SqlDbType.DateTime;
                paramtype[7] = SqlDbType.DateTime;
                paramtype[8] = SqlDbType.VarChar;
                paramtype[9] = SqlDbType.DateTime;
                paramtype[10] = SqlDbType.VarChar;

                SQLServer sq = new SQLServer(Global.GetConnectionString());
                string srno = "";
                 
                DataSet ds = sq.SelectRecords("SpAddCTMMasterRecords", paramname, paramvalue, paramtype);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            srno = ds.Tables[0].Rows[0][0].ToString();
                            #region AWB Save
                            for (int i = 0; i < gvCTM.Rows.Count; i++)
                            {
                                if (((CheckBox)gvCTM.Rows[i].FindControl("chkgvCTM")).Checked == true)
                                {
                                    string[] paramnameC = new string[22];
                                    paramnameC[0] = "CTMRefNo";
                                    paramnameC[1] = "AWBNo";
                                    paramnameC[2] = "OwnerCode";
                                    paramnameC[3] = "StdPcs";
                                    paramnameC[4] = "StdWt";
                                    paramnameC[5] = "StdWtUnit";
                                    paramnameC[6] = "StdVol";
                                    paramnameC[7] = "StdVolUnit";
                                    paramnameC[8] = "ActPcs";
                                    paramnameC[9] = "ActWt";
                                    paramnameC[10] = "ActWtUnit";
                                    paramnameC[11] = "ActVol";
                                    paramnameC[12] = "ActVolUnit";
                                    paramnameC[13] = "Origin";
                                    paramnameC[14] = "Dest";
                                    paramnameC[15] = "SCC";
                                    paramnameC[16] = "ShipDetails";
                                    paramnameC[17] = "Remarks";
                                    paramnameC[18] = "CreatedOn";
                                    paramnameC[19] = "CreatedBy";
                                    //paramnameC[20] = "UpdatedOn";
                                    //paramnameC[21] = "UpdatedBy";
                                    paramnameC[20] = "CTMMasSrNo";
                                    paramnameC[21] = "ULDNo";

                                    object[] paramvalueC = new object[22];
                                    paramvalueC[0] = txtCTMRef.Text.Trim();
                                    paramvalueC[1] = ((Label)gvCTM.Rows[i].FindControl("lblAWB")).Text;
                                    paramvalueC[2] = ((Label)gvCTM.Rows[i].FindControl("lblOwnerCode")).Text;
                                    paramvalueC[3] = ((Label)gvCTM.Rows[i].FindControl("lblStdPcs")).Text;
                                    paramvalueC[4] = ((Label)gvCTM.Rows[i].FindControl("lblStdWgt")).Text;
                                    paramvalueC[5] = ((Label)gvCTM.Rows[i].FindControl("lblWgtUnit")).Text;
                                    paramvalueC[6] = ((Label)gvCTM.Rows[i].FindControl("lblStdVol")).Text;
                                    paramvalueC[7] = ((Label)gvCTM.Rows[i].FindControl("lblVolUnit")).Text;
                                    paramvalueC[8] = ((Label)gvCTM.Rows[i].FindControl("lblActPcs")).Text;
                                    paramvalueC[9] = ((Label)gvCTM.Rows[i].FindControl("lblActWgt")).Text;
                                    paramvalueC[10] = ((Label)gvCTM.Rows[i].FindControl("lblWtUnit")).Text;
                                    paramvalueC[11] = ((Label)gvCTM.Rows[i].FindControl("lblActVol")).Text;
                                    paramvalueC[12] = ((Label)gvCTM.Rows[i].FindControl("lblActVolUnit")).Text;
                                    paramvalueC[13] = ((Label)gvCTM.Rows[i].FindControl("lblOrigin")).Text;
                                    paramvalueC[14] = ((Label)gvCTM.Rows[i].FindControl("lblDest")).Text;
                                    paramvalueC[15] = ((Label)gvCTM.Rows[i].FindControl("lblSCC")).Text;
                                    paramvalueC[16] = ((Label)gvCTM.Rows[i].FindControl("lblShipDetails")).Text;
                                    paramvalueC[17] = ((Label)gvCTM.Rows[i].FindControl("lblRemarks")).Text;
                                    paramvalueC[18] = Session["IT"].ToString();
                                    paramvalueC[19] = Session["UserName"].ToString();
                                    paramvalueC[20] = srno;
                                    paramvalueC[21] = ((Label)gvCTM.Rows[i].FindControl("lblULD")).Text;

                                    SqlDbType[] paramtypeC = new SqlDbType[22];
                                    paramtypeC[0] = SqlDbType.VarChar;
                                    paramtypeC[1] = SqlDbType.VarChar;
                                    paramtypeC[2] = SqlDbType.VarChar;
                                    paramtypeC[3] = SqlDbType.Int;
                                    paramtypeC[4] = SqlDbType.Decimal;
                                    paramtypeC[5] = SqlDbType.VarChar;
                                    paramtypeC[6] = SqlDbType.Decimal;
                                    paramtypeC[7] = SqlDbType.VarChar;
                                    paramtypeC[8] = SqlDbType.Int;
                                    paramtypeC[9] = SqlDbType.Decimal;
                                    paramtypeC[10] = SqlDbType.VarChar;
                                    paramtypeC[11] = SqlDbType.Decimal;
                                    paramtypeC[12] = SqlDbType.VarChar;
                                    paramtypeC[13] = SqlDbType.VarChar;
                                    paramtypeC[14] = SqlDbType.VarChar;
                                    paramtypeC[15] = SqlDbType.VarChar;
                                    paramtypeC[16] = SqlDbType.VarChar;
                                    paramtypeC[17] = SqlDbType.VarChar;
                                    paramtypeC[18] = SqlDbType.DateTime;
                                    paramtypeC[19] = SqlDbType.VarChar;
                                    paramtypeC[20] = SqlDbType.BigInt;
                                    paramtypeC[21] = SqlDbType.VarChar;
                                    SQLServer da = new SQLServer(Global.GetConnectionString());
                                    bool res = da.InsertData("SpAddCTMDetailsRecords", paramnameC, paramtypeC, paramvalueC);
                                    if (res == true)
                                    {
                                        lblMsg.Text = "Record Saved Successfully!";
                                        lblMsg.ForeColor = Color.Green;
                                        txtCTMRef.Text = srno;
                                        btnSave.Visible = false;
                                    }
                                    else
                                    {
                                        txtCTMRef.Text = "";
                                        lblMsg.Text = "Saving Record Failed!";
                                        lblMsg.ForeColor = Color.Red;
                                        btnSave.Visible = true;

                                    }
                                }
                            }
                            #endregion AWB Save
                            #region ULD Save
                            for (int i = 0; i < grdULDDetails.Rows.Count; i++)
                            {
                                if (((CheckBox)grdULDDetails.Rows[i].FindControl("checkULD")).Checked == true)
                                {
                                    string[] paramnameC1 = new string[14];
                                    paramnameC1[0] = "CTMRefNo";
                                    paramnameC1[1] = "ULDNo";
                                    paramnameC1[2] = "AWBNo";
                                    paramnameC1[3] = "FltNo";
                                    paramnameC1[4] = "FltDt";
                                    paramnameC1[5] = "Origin";
                                    paramnameC1[6] = "Dest";
                                    paramnameC1[7] = "ULDWt";
                                    paramnameC1[8] = "ScaleWt";
                                    paramnameC1[9] = "AWBCt";
                                    paramnameC1[10] = "AWBPcs";
                                    paramnameC1[11] = "AWBWt";
                                    paramnameC1[12] = "Location";
                                    //paramnameC1[13] = "CreatedOn";
                                    //paramnameC1[14] = "CreatedBy";
                                    //paramnameC[20] = "UpdatedOn";
                                    //paramnameC[21] = "UpdatedBy";
                                    paramnameC1[13] = "CTMMasSrNo";
                                    //paramnameC1[14] = "POL";
                                    

                                    object[] paramvalueC1 = new object[14];
                                    paramvalueC1[0] = txtCTMRef.Text.Trim();
                                    paramvalueC1[1] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDNo")).Text;
                                    paramvalueC1[2] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdAWBNo")).Text;
                                    paramvalueC1[3] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightNo")).Text;
                                    paramvalueC1[4] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDFlightDate")).Text;
                                    paramvalueC1[5] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDOrg")).Text;
                                    paramvalueC1[6] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDDestn")).Text;
                                    paramvalueC1[7] = Convert.ToDecimal(((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDULDWt")).Text);
                                    try
                                    {
                                        paramvalueC1[8] = Convert.ToDecimal(((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDScaleWt")).Text);
                                    }
                                    catch (Exception ex)
                                    {
                                        paramvalueC1[8] = Convert.ToDecimal("0");
                                    }
                                    paramvalueC1[9] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBCount")).Text;
                                    paramvalueC1[10] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBPcs")).Text;
                                    paramvalueC1[11] = Convert.ToDecimal(((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDAWBWt")).Text);
                                    paramvalueC1[12] = ((TextBox)grdULDDetails.Rows[i].FindControl("GrdULDLocation")).Text;
                                    //paramvalueC1[13] = Session["IT"].ToString();
                                    //paramvalueC1[14] = Session["UserName"].ToString();
                                    paramvalueC1[13] = srno;
                                    //paramvalueC1[14] = ((TextBox)gvCTM.Rows[i].FindControl("GrdULDPOL")).Text;
                                    
                                    

                                    SqlDbType[] paramtypeC1 = new SqlDbType[14];
                                    paramtypeC1[0] = SqlDbType.VarChar;
                                    paramtypeC1[1] = SqlDbType.VarChar;
                                    paramtypeC1[2] = SqlDbType.VarChar;
                                    paramtypeC1[3] = SqlDbType.VarChar;
                                    paramtypeC1[4] = SqlDbType.DateTime;
                                    paramtypeC1[5] = SqlDbType.VarChar;
                                    paramtypeC1[6] = SqlDbType.VarChar;
                                    paramtypeC1[7] = SqlDbType.Decimal;
                                    paramtypeC1[8] = SqlDbType.Decimal;
                                    paramtypeC1[9] = SqlDbType.Int;
                                    paramtypeC1[10] = SqlDbType.Int;
                                    paramtypeC1[11] = SqlDbType.Decimal;
                                    paramtypeC1[12] = SqlDbType.VarChar;
                                    paramtypeC1[13] = SqlDbType.VarChar;
                                    //paramtypeC1[14] = SqlDbType.VarChar;
                                    
                                    SQLServer da = new SQLServer(Global.GetConnectionString());
                                    bool res = da.InsertData("SpAddCTMULDDetailsRecords", paramnameC1, paramtypeC1, paramvalueC1);
                                    if (res == true)
                                    {
                                        lblMsg.Text = "Record Saved Successfully!";
                                        lblMsg.ForeColor = Color.Green;
                                        txtCTMRef.Text = srno;
                                        btnSave.Visible = false;
                                    }
                                    else
                                    {
                                        txtCTMRef.Text = "";
                                        lblMsg.Text = "Saving Record Failed!";
                                        lblMsg.ForeColor = Color.Red;
                                        btnSave.Visible = true;

                                    }
                                }
                            }
                            #endregion ULD Save
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        protected void btnClear_Click(object sender, EventArgs e)
        {
            gvCTM.DataSource = null;
            gvCTM.DataBind();
            txtCTMRef.Text = "";
            ddlCTM.SelectedIndex = 0;
            txtCreateDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            txtAWBPre.Text = "";
            txtAWBNo.Text = "";
            txtFromPre.Text = "";
            txtFromFlight.Text = "";
            txtFDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            txtToPre.Text = "";
            txtToFlight.Text = "";
            txtTDate.Text = "";

        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if ((DataSet)Session["CTM"] == null)
                {
                    return;
                }

                DataSet dsVal = (DataSet)Session["CTM"];
                if (dsVal == null)
                    return;

                if ((DataSet)Session["CTMULD"] == null)
                {
                    return;
                }

                DataSet dsValULD = (DataSet)Session["CTMULD"];
                if (dsValULD == null)
                    return;

                dtULD = dsValULD.Tables[0];
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
                //end
                DataTable dtCTM = new DataTable();

                dtCTM.Columns.Add("CTMNo");
                dtCTM.Columns.Add("Airport");
                dtCTM.Columns.Add("Date");
                dtCTM.Columns.Add("TransferredTo");
                dtCTM.Columns.Add("AWBPrefix");
                dtCTM.Columns.Add("AWBNo");
                dtCTM.Columns.Add("AWBDest");
                dtCTM.Columns.Add("PCS");
                dtCTM.Columns.Add("Wgt");
                dtCTM.Columns.Add("Remarks");
                dtCTM.Columns.Add("Transferredby");
                dtCTM.Columns.Add("ReceivedBy");
                dtCTM.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));

                if (dsVal != null)
                {
                    if (dsVal.Tables != null)
                    {
                        if (dsVal.Tables.Count > 0)
                        {
                            for (int i = 0; i < dsVal.Tables[0].Rows.Count; i++)
                            {
                                string AWBNumber = dsVal.Tables[0].Rows[i]["AWBNo"].ToString();
                                string AWBPrefix = dsVal.Tables[0].Rows[i]["AWBNo"].ToString().Substring(0, AWBNumber.Length - 8);
                                AWBNumber = AWBNumber.Replace(AWBPrefix, "");
                                dtCTM.Rows.Add(txtCTMRef.Text.Trim(), lblAirport.Text, txtFDate.Text.Trim(), txtToPre.Text.Trim(), AWBPrefix, AWBNumber, dsVal.Tables[0].Rows[i]["DestinationCode"].ToString(), dsVal.Tables[0].Rows[i]["ActPcs"].ToString(), dsVal.Tables[0].Rows[i]["ActWtdetails"].ToString(), dsVal.Tables[0].Rows[i]["Remarks"].ToString(), txtFromPre.Text.Trim(), txtToPre.Text.Trim(),Logo.ToArray());
                            }

                            RenderReport(dtCTM);
                        }
                    }
                }



            }
            catch (Exception ex)
            {
            }
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

                //"  <PageWidth>8.5in</PageWidth>" +

                //"  <PageHeight>11in</PageHeight>" +

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


        public void ItemsSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("CTM_DataTableULD", dtULD));

        }

    }
}
