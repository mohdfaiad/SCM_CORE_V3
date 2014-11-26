using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using QID.DataAccess;
using System.Data;
using BAL;

//using DataDynamics.Reports.Rendering.Pdf;
//using DataDynamics.Reports.Rendering.IO;
//using DataDynamics.Reports.Rendering.Excel;
using System.IO;

namespace ProjectSmartCargoManager
{
    public partial class frmApplyPLI : System.Web.UI.Page
    {
        #region Variables
        DataSet ds;
        SQLServer db = new SQLServer(Global.GetConnectionString());
        BALAgentPLI objBAL = new BALAgentPLI();
        private DataSet Dataset1 = new DataSet();
        private DataSet Dataset2 = new DataSet();
        private DataSet Dataset3 = new DataSet();
        AgentBAL objBLL = new AgentBAL();
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadAgentName();
                }


            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
          

        }
        #endregion

        #region Button List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {

                lblStatus.Text = "";
                btnApplyPLIs.Visible = false;
                btnPrintApplicableTonnagePLI.Visible = false;
                btnPrintFlatAWBRate.Visible = false;
                btnPrintKickBackPLI.Visible = false;
                btnPrintAWBSummary.Visible = false;
                //To check if PLIs are available or not
                string strfromdate, strtodate;

                #region Prepare Parameters
                object[] AWBInfo = new object[3];
                int i = 0;

                if (ddlAgentCode.SelectedItem.Text == "Select")
                {
                    lblStatus.Text = "Please Select Agent Code";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                AWBInfo.SetValue(ddlAgentCode.SelectedItem.Value, i);
                i++;

                //Validation for From date
                if (txtPLIFrom.Text == "")
                {
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dt;

                try
                {
                    string day = txtPLIFrom.Text.Substring(0, 2);
                    string mon = txtPLIFrom.Text.Substring(3, 2);
                    string yr = txtPLIFrom.Text.Substring(6, 4);
                    strfromdate = yr + "-" + mon + "-" + day;
                    dt = Convert.ToDateTime(strfromdate);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                AWBInfo.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), i);
                i++;

                //Validation for To date
                if (txtPLITo.Text == "")
                {
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dtto;

                try
                {
                    string day = txtPLITo.Text.Substring(0, 2);
                    string mon = txtPLITo.Text.Substring(3, 2);
                    string yr = txtPLITo.Text.Substring(6, 4);
                    strtodate = yr + "-" + mon + "-" + day;
                    dtto = Convert.ToDateTime(strtodate);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (dtto < dt)
                {
                    lblStatus.Text = "To date should be greater than From date";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                AWBInfo.SetValue(Convert.ToDateTime(strtodate).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"), i);

                #endregion Prepare Parameters

                ds = objBAL.GetAvailablePLIList(AWBInfo);
                string[] QueryN = new string[4];
                object[] QueryV = new object[4];
                SqlDbType[] QueryT = new SqlDbType[4];

                QueryN[0] = "PLIId";
                QueryN[1] = "FromDate";
                QueryN[2] = "ToDate";
                QueryN[3] = "Selection";
                QueryT[0] = SqlDbType.VarChar;
                QueryT[1] = SqlDbType.VarChar;
                QueryT[2] = SqlDbType.VarChar;
                QueryT[3] = SqlDbType.VarChar;
                QueryV[1] = AWBInfo[1];
                QueryV[2] = AWBInfo[2];
                QueryV[3] = "";

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //If PLIs are available
                        //string res;
                        string[] PLIAmount = new string[0];
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            QueryV[0] = ds.Tables[0].Rows[j]["PLIID"].ToString();
                            DataSet Amount = db.SelectRecords("SP_GettingPLIValue", QueryN, QueryV, QueryT);
                            Array.Resize(ref PLIAmount, PLIAmount.Length + 1);
                            PLIAmount[PLIAmount.Length - 1] = Amount.Tables[0].Rows[0][0].ToString();
                        }

                        GrdPLIDetails.DataSource = ds;
                        GrdPLIDetails.DataBind();
                        btnApplyPLIs.Visible = true;
                        btnClose.Visible = true;
                        for (int j = 0; j < GrdPLIDetails.Rows.Count; j++)
                        {
                            if (PLIAmount[j] == "")
                            {
                                ((Label)GrdPLIDetails.Rows[j].FindControl("lblPLIAmount")).Text = "N/A";
                            }
                            else
                            {
                                ((Label)GrdPLIDetails.Rows[j].FindControl("lblPLIAmount")).Text = PLIAmount[j];
                            }
                        }

                    }
                    else
                    {
                        lblStatus.Text = "No PLIs available";
                        lblStatus.ForeColor = Color.Blue;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        #region GrdPLIDetails RowCommand
        protected void GrdPLIDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Preview")
                {
                    try
                    {
                        //To check if PLIs are available or not
                        string strfromdate, strtodate;
                        FileInfo info;
                        ////ReportRuntime runtime;
                        ////ReportDefinition definition;

                        #region Prepare Parameters
                        object[] AWBInfo = new object[3];
                        int i = 0;

                        if (ddlAgentCode.SelectedItem.Text == "Select")
                        {
                            lblStatus.Text = "Please Select Agent Code";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        AWBInfo.SetValue(ddlAgentCode.SelectedItem.Value, i);
                        i++;

                        //Validation for From date
                        if (txtPLIFrom.Text == "")
                        {
                            lblStatus.Text = "Please select Valid date";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                        DateTime dt;

                        try
                        {
                            string day = txtPLIFrom.Text.Substring(0, 2);
                            string mon = txtPLIFrom.Text.Substring(3, 2);
                            string yr = txtPLIFrom.Text.Substring(6, 4);
                            strfromdate = yr + "-" + mon + "-" + day;
                            dt = Convert.ToDateTime(strfromdate);

                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = "Selected Date format invalid";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        AWBInfo.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), i);
                        i++;

                        //Validation for To date
                        if (txtPLITo.Text == "")
                        {
                            lblStatus.Text = "Please select Valid date";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                        DateTime dtto;

                        try
                        {
                            string day = txtPLITo.Text.Substring(0, 2);
                            string mon = txtPLITo.Text.Substring(3, 2);
                            string yr = txtPLITo.Text.Substring(6, 4);
                            strtodate = yr + "-" + mon + "-" + day;
                            dtto = Convert.ToDateTime(strtodate);
                        }
                        catch (Exception ex)
                        {
                            lblStatus.Text = "Selected Date format invalid";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        if (dtto < dt)
                        {
                            lblStatus.Text = "To date should be greater than From date";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }

                        AWBInfo.SetValue(Convert.ToDateTime(strtodate).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"), i);

                        #endregion Prepare Parameters

                        object[] PLIInfo = new object[6];
                        int d = 0;

                        PLIInfo.SetValue(e.CommandArgument.ToString(), d);
                        d++;

                        PLIInfo.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), d);
                        d++;

                        PLIInfo.SetValue(Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), d);
                        d++;

                        string UserName = Session["UserName"].ToString();
                        PLIInfo.SetValue(UserName, d);
                        d++;

                        PLIInfo.SetValue(System.DateTime.Now, d);
                        d++;

                        PLIInfo.SetValue("", d);

                        try
                        {
                            DataSet dset = objBAL.ApplyAgentPLIs(PLIInfo);

                            if (dset != null)
                            {
                                if (dset.Tables.Count > 0)
                                {
                                    if (dset.Tables[2].Rows.Count > 0)
                                    {


                                        ////rptTonnagePLI.Visible = true;
                                        info = new FileInfo(Server.MapPath("/Reports/PLI.rdlx"));
                                        ////definition = new ReportDefinition(info);
                                        ////runtime = new ReportRuntime(definition);
                                        DataTable dtable = dset.Tables[4];
                                        DataTable Dt = dset.Tables[6];
                                        //DataTable dtAWB = dset.Tables[dset.Tables.Count - 2];
                                        //Dataset1.Tables.Add(Dt.Copy());
                                        //Dataset2.Tables.Add(dtable.Copy());
                                        //Dataset3.Tables.Add(dtAWB.Copy());
                                        Dataset1.Tables.Add(Dt.Copy());
                                        Dataset2.Tables.Add(dtable.Copy());
                                        ////runtime.LocateDataSource += WRAXBDetails_LocateDataSource;
                                        ////rptTonnagePLI.SetReport(runtime);


                                        //btnExport.Visible = true;
                                        //grdPreview.DataSource = ds.Tables[15];
                                        //grdPreview.DataBind();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplitPreview();</script>", false);


                                    }
                                    else
                                    {
                                        lblStatus.Text = "No Data found for the agent to apply PLI!!";
                                        lblStatus.ForeColor = Color.Red;
                                        return;
                                    }
                                }
                            }

                        }
                        catch
                        {
                            lblStatus.Text = "Error while applying PLI; Please try again";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }


                    }
                    catch (Exception ex)
                    {


                    }
                }
                else
                    if (e.CommandName == "PLIDetails")
                    {


                        string PLIId = e.CommandArgument.ToString();

                        DataSet ds = db.SelectRecords("SpGetExclusionPLIDetails", "PLIId", PLIId, SqlDbType.VarChar);

                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    grdExclusion.DataSource = ds.Tables[0];
                                    grdExclusion.DataBind();
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        grdTonnageSlab.DataSource = ds.Tables[1];
                                        grdTonnageSlab.DataBind();

                                    }
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false);


                                }
                            }
                        }
                    }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion

        #region Button Apply PLI
        protected void btnApplyPLIs_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                Session["PLIData"] = null;
                ApplyPLIs();
                GrdPLIDetails.DataSource = null;
                GrdPLIDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }


        }
        #endregion

        protected void ApplyPLIs()
        {
            #region Commented Code
            //try
            //{
            //    //To check if PLIs are available or not
            //    string strfromdate, strtodate;

            //    #region Prepare Parameters
            //    object[] AWBInfo = new object[3];
            //    int i = 0;

            //    if (ddlAgentCode.SelectedItem.Text == "Select")
            //    {
            //        lblStatus.Text = "Please Select Agent Code";
            //        lblStatus.ForeColor = Color.Red;
            //        return;
            //    }

            //    AWBInfo.SetValue(ddlAgentCode.SelectedItem.Text, i);
            //    i++;

            //    //Validation for From date
            //    if (txtPLIFrom.Text == "")
            //    {
            //        lblStatus.Text = "Please select Valid date";
            //        lblStatus.ForeColor = Color.Blue;
            //        return;
            //    }

            //    DateTime dt;

            //    try
            //    {
            //        string day = txtPLIFrom.Text.Substring(0, 2);
            //        string mon = txtPLIFrom.Text.Substring(3, 2);
            //        string yr = txtPLIFrom.Text.Substring(6, 4);
            //        strfromdate = yr + "-" + mon + "-" + day;
            //        dt = Convert.ToDateTime(strfromdate);

            //    }
            //    catch (Exception ex)
            //    {
            //        lblStatus.Text = "Selected Date format invalid";
            //        lblStatus.ForeColor = Color.Red;
            //        return;
            //    }

            //    AWBInfo.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), i);
            //    i++;

            //    //Validation for To date
            //    if (txtPLITo.Text == "")
            //    {
            //        lblStatus.Text = "Please select Valid date";
            //        lblStatus.ForeColor = Color.Blue;
            //        return;
            //    }

            //    DateTime dtto;

            //    try
            //    {
            //        string day = txtPLITo.Text.Substring(0, 2);
            //        string mon = txtPLITo.Text.Substring(3, 2);
            //        string yr = txtPLITo.Text.Substring(6, 4);
            //        strtodate = yr + "-" + mon + "-" + day;
            //        dtto = Convert.ToDateTime(strtodate);
            //    }
            //    catch (Exception ex)
            //    {
            //        lblStatus.Text = "Selected Date format invalid";
            //        lblStatus.ForeColor = Color.Red;
            //        return;
            //    }

            //    if (dtto < dt)
            //    {
            //        lblStatus.Text = "To date should be greater than From date";
            //        lblStatus.ForeColor = Color.Red;
            //        return;
            //    }

            //    AWBInfo.SetValue(Convert.ToDateTime(strtodate).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"), i);

            //    #endregion Prepare Parameters

            //    ds = objBAL.GetAvailablePLIList(AWBInfo);

            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        //If PLIs are available
            //        string res;
            //        #region Commented Code
            //        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            //        {
            //            object[] PLIInfo = new object[5];
            //            int d = 0;

            //            PLIInfo.SetValue(ds.Tables[0].Rows[j][0].ToString(), d);
            //            d++;

            //            PLIInfo.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), d);
            //            d++;

            //            PLIInfo.SetValue(Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), d);
            //            d++;

            //            string UserName = Session["UserName"].ToString();
            //            PLIInfo.SetValue(UserName, d);
            //            d++;

            //            PLIInfo.SetValue(System.DateTime.Now, d);

            //            try
            //            {
            //               DataSet dset = objBAL.ApplyAgentPLIs(PLIInfo);

            //                //if (res == "PLI applied")
            //                //{
            //                //    lblStatus.Text = "PLIs applied successfully";
            //                //    lblStatus.ForeColor = Color.Green;
            //                //    return;
            //                //}
            //                //else//No awb applicable for PLI
            //                //{
            //                //    lblStatus.Text = "No AWBs available for PLIs";
            //                //    lblStatus.ForeColor = Color.Blue;
            //                //    return;
            //                //}
            //               if (dset != null)
            //               {
            //                   if (dset.Tables.Count > 0)
            //                   {

            //                   }
            //               }
            //            }
            //            catch
            //            {
            //                lblStatus.Text = "Error while applying PLIs; Please try again";
            //                lblStatus.ForeColor = Color.Red;
            //                return;
            //            }
            //        }
            //        #endregion


            //    }
            //    else
            //    {
            //        lblStatus.Text = "No PLIs available";
            //        lblStatus.ForeColor = Color.Blue;
            //        return;
            //    }

            //}
            //catch(Exception ex)
            //{


            //}
            #endregion

            try
            {
                //To check if PLIs are available or not
                string strfromdate, strtodate;
                string PLIID = "";

                for (int j = 0; j < GrdPLIDetails.Rows.Count; j++)
                {
                    if (((CheckBox)GrdPLIDetails.Rows[j].FindControl("ChkSelect")).Checked)
                    {
                        PLIID = ((Label)GrdPLIDetails.Rows[j].FindControl("lblPLIID")).Text.Trim();
                    }
                }

                #region Prepare Parameters
                object[] AWBInfo = new object[3];
                int i = 0;

                if (ddlAgentCode.SelectedIndex == 0)
                {
                    lblStatus.Text = "Please Select Agent Code";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                AWBInfo.SetValue(ddlAgentCode.SelectedItem.Value, i);
                i++;

                //Validation for From date
                if (txtPLIFrom.Text == "")
                {
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dt;

                try
                {
                    string day = txtPLIFrom.Text.Substring(0, 2);
                    string mon = txtPLIFrom.Text.Substring(3, 2);
                    string yr = txtPLIFrom.Text.Substring(6, 4);
                    strfromdate = yr + "-" + mon + "-" + day;
                    dt = Convert.ToDateTime(strfromdate);

                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                AWBInfo.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), i);
                i++;

                //Validation for To date
                if (txtPLITo.Text == "")
                {
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dtto;

                try
                {
                    string day = txtPLITo.Text.Substring(0, 2);
                    string mon = txtPLITo.Text.Substring(3, 2);
                    string yr = txtPLITo.Text.Substring(6, 4);
                    strtodate = yr + "-" + mon + "-" + day;
                    dtto = Convert.ToDateTime(strtodate);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Selected Date format invalid";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                if (dtto < dt)
                {
                    lblStatus.Text = "To date should be greater than From date";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }

                AWBInfo.SetValue(Convert.ToDateTime(strtodate).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"), i);

                #endregion Prepare Parameters

                object[] PLIInfo = new object[6];
                int d = 0;

                PLIInfo.SetValue(PLIID, d);
                d++;

                PLIInfo.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), d);
                d++;

                PLIInfo.SetValue(Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), d);
                d++;

                string UserName = Session["UserName"].ToString();
                PLIInfo.SetValue(UserName, d);
                d++;

                PLIInfo.SetValue(System.DateTime.Now, d);
                d++;

                PLIInfo.SetValue("Confirm", d);


                try
                {
                    DataSet dset = objBAL.ApplyAgentPLIs(PLIInfo);

                    if (dset != null)
                    {
                        if (dset.Tables.Count > 0)
                        {
                            if (dset.Tables[2].Rows.Count > 0)
                            {
                                Session["PLIData"] = dset;
                                btnPrintApplicableTonnagePLI.Visible = true;
                                btnApplyPLIs.Visible = false;
                                btnPrintAWBSummary.Visible = true;
                                lblStatus.Text = "PLI Applied Successfully";
                                lblStatus.ForeColor = Color.Green;
                            }
                            else
                            {
                                lblStatus.Text = "No AWB's found for the agent to apply PLI!!";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }

                        }
                    }
                }
                catch
                {
                    lblStatus.Text = "Error while applying PLI; Please try again";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }


            }
            catch (Exception ex)
            {


            }

        }

        #region Button Print AWB Summary
        protected void btnPrintAWBSummary_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["PLIData"] != null)
                {
                    DataSet dset = (DataSet)Session["PLIData"];
                    if (dset != null)
                    {
                        if (dset.Tables.Count > 0)
                        {
                            if (dset.Tables[dset.Tables.Count - 2].Rows.Count > 0)
                            {

                                Session["PLIData"] = dset;

                                ////DataDynamics.Reports.ReportDefinition _reportDef = new ReportDefinition();
                                ////_reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/ExcludedDetails.rdlx")));

                                //DataTable dtable = dset.Tables[dset.Tables.Count - 3];
                                //DataTable Dt = dset.Tables[dset.Tables.Count - 1];
                                DataTable dtAWB = dset.Tables[dset.Tables.Count - 2];
                                //Dataset1.Tables.Add(Dt.Copy());
                                //Dataset2.Tables.Add(dtable.Copy());
                                Dataset3.Tables.Add(dtAWB.Copy());
                                ////DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef);
                                ////_reportRuntime.LocateDataSource += WRAXBDetails_LocateDataSource;
                                string exportFile = System.IO.Path.GetTempFileName() + ".pdf";
                                System.IO.FileInfo myFile = new System.IO.FileInfo(exportFile);
                                System.Collections.Specialized.NameValueCollection settings = new System.Collections.Specialized.NameValueCollection();
                                //settings.Add("hideToolbar", "True");
                                //settings.Add("hideMenubar", "True");
                                //settings.Add("hideWindowUI", "True");
                                //settings.Add("MarginLeft", "0.1in");
                                //settings.Add("MarginRight", "0.1in");
                                //settings.Add("MarginTop", "0.1in");
                                //settings.Add("MarginBottom", "0.1in");
                                //settings.Add("PageWidth", "9in");
                                //settings.Add("PageHeight", "13in");
                                //settings.Add("FitWindow", "True");
                                //ExcelTransformationDevice _renderingExtension = new ExcelTransformationDevice();
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





                            }
                            else
                            {
                                lblStatus.Text = "No AWB's present for the report to process!";
                                lblStatus.ForeColor = Color.Red;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }
        #endregion 

        #region Event for Loading Dataset into Report
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
        #endregion

        #region Button Applicable PLI 
        protected void btnPrintApplicableTonnagePLI_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (Session["PLIData"] != null)
                    {
                        DataSet dset = (DataSet)Session["PLIData"];
                        if (dset != null)
                        {
                            if (dset.Tables.Count > 0)
                            {
                                if (dset.Tables[2].Rows.Count > 0)
                                {
                                    Session["PLIData"] = dset;
                                    ////DataDynamics.Reports.ReportDefinition _reportDef = new ReportDefinition();
                                    //DataDynamics.Reports.ReportDefinition _reportDef1 = new ReportDefinition();
                                    ////_reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/PLI.rdlx")));
                                    // _reportDef1 = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/ExcludedDetails.rdlx")));

                                    DataTable dtable = dset.Tables[4];
                                    DataTable Dt = dset.Tables[6];
                                    Dataset1.Tables.Add(Dt.Copy());
                                    Dataset2.Tables.Add(dtable.Copy());
                                    //DataDynamics.Reports.ReportRuntime _reportRuntime1 = new DataDynamics.Reports.ReportRuntime(_reportDef1);

                                    ////DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef);
                                    ////_reportRuntime.LocateDataSource += WRAXBDetails_LocateDataSource;
                                    // _reportRuntime1.LocateDataSource += WRAXBDetails_LocateDataSource;
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
                                }
                                else
                                {
                                    lblStatus.Text = "No AWB's found for the agent to apply PLI!!";
                                    lblStatus.ForeColor = Color.Red;
                                    return;
                                }

                            }
                        }
                    }
                }
                catch
                {
                    lblStatus.Text = "Error while printing PLI report; Please try again";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion

        //Load agent name in a dopdown
        #region Load AgentName
        public void LoadAgentName()
        {
            try
            {
                if (Session["AgentCode"] == null)
                    Session["AgentCode"] = "";
                DataSet ds = objBLL.GetAgentList(Session["AgentCode"].ToString());
                if (ds != null)
                {
                    ddlAgentCode.DataSource = ds;
                    ddlAgentCode.DataMember = ds.Tables[0].TableName;
                    ddlAgentCode.DataTextField = "AgentName";
                    ddlAgentCode.DataValueField = "AgentCode";
                    ddlAgentCode.DataBind();
                    //ddlAgentCode.Items.Insert(0, "Select");
                    ddlAgentCode.SelectedIndex = 0;

                }
            }
            catch (Exception ex)
            { }
        }
        #endregion LoadAgentName

        #region Button Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                grdExclusion.DataSource = null;
                grdExclusion.DataBind();
                grdTonnageSlab.DataSource = null;
                grdTonnageSlab.DataBind();
                GrdPLIDetails.DataSource = null;
                GrdPLIDetails.DataBind();
                ddlAgentCode.SelectedIndex = 0;
                txtPLIFrom.Text = "";
                txtPLITo.Text = "";
                btnPrintApplicableTonnagePLI.Visible = false;
                btnPrintAWBSummary.Visible = false;
                btnPrintFlatAWBRate.Visible = false;
                btnPrintKickBackPLI.Visible = false;
                btnApplyPLIs.Visible = false;
                btnClose.Visible = false;
                Session["PLIData"] = null;
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
            }

        }
        #endregion




    }
}
