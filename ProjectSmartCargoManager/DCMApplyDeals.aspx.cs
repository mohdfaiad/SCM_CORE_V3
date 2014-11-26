using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using BAL;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
using System.IO;
//
//using DataDynamics.Reports.Rendering.Pdf;
//using DataDynamics.Reports.Rendering.IO;
//using DataDynamics.Reports.Rendering.Excel;
  
namespace ProjectSmartCargoManager
{
    public partial class DCMApplyDeals : System.Web.UI.Page
    {
        AgentBAL objBLL = new AgentBAL();
        BALAgentDeal objBAL = new BALAgentDeal();
        DataSet ds;
        SQLServer db = new SQLServer(Global.GetConnectionString());
        private DataSet Dataset1=new DataSet();
        private DataSet Dataset2=new DataSet();
        private DataSet Dataset3 = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Session["DealData"] = null;
                    LoadAgentName();
                }
            }
            catch(Exception ex)
            {

            }
        }

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

        #region Apply Deals
        protected void btnApplyDeals_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                Session["DealData"] = null;
                ApplyDeals();
                GrdDealDetails.DataSource = null;
                GrdDealDetails.DataBind();
                
            }
            catch (Exception ex)
            { }
        }
        #endregion Apply Deals

        protected void ApplyDeals()
        {
            #region Commented Code
            //try
            //{
            //    //To check if deals are available or not
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
            //    if (txtDealFrom.Text == "")
            //    {
            //        lblStatus.Text = "Please select Valid date";
            //        lblStatus.ForeColor = Color.Blue;
            //        return;
            //    }

            //    DateTime dt;

            //    try
            //    {
            //        string day = txtDealFrom.Text.Substring(0, 2);
            //        string mon = txtDealFrom.Text.Substring(3, 2);
            //        string yr = txtDealFrom.Text.Substring(6, 4);
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
            //    if (txtDealTo.Text == "")
            //    {
            //        lblStatus.Text = "Please select Valid date";
            //        lblStatus.ForeColor = Color.Blue;
            //        return;
            //    }

            //    DateTime dtto;

            //    try
            //    {
            //        string day = txtDealTo.Text.Substring(0, 2);
            //        string mon = txtDealTo.Text.Substring(3, 2);
            //        string yr = txtDealTo.Text.Substring(6, 4);
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

            //    ds = objBAL.GetAvailableDealList(AWBInfo);

            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        //If Deals are available
            //        string res;
            //        #region Commented Code
            //        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            //        {
            //            object[] DealInfo = new object[5];
            //            int d = 0;

            //            DealInfo.SetValue(ds.Tables[0].Rows[j][0].ToString(), d);
            //            d++;

            //            DealInfo.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), d);
            //            d++;

            //            DealInfo.SetValue(Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), d);
            //            d++;

            //            string UserName = Session["UserName"].ToString();
            //            DealInfo.SetValue(UserName, d);
            //            d++;

            //            DealInfo.SetValue(System.DateTime.Now, d);

            //            try
            //            {
            //               DataSet dset = objBAL.ApplyAgentDeals(DealInfo);

            //                //if (res == "Deal applied")
            //                //{
            //                //    lblStatus.Text = "Deals applied successfully";
            //                //    lblStatus.ForeColor = Color.Green;
            //                //    return;
            //                //}
            //                //else//No awb applicable for deal
            //                //{
            //                //    lblStatus.Text = "No AWBs available for deals";
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
            //                lblStatus.Text = "Error while applying deals; Please try again";
            //                lblStatus.ForeColor = Color.Red;
            //                return;
            //            }
            //        }
            //        #endregion


            //    }
            //    else
            //    {
            //        lblStatus.Text = "No deals available";
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
                //To check if deals are available or not
                string strfromdate, strtodate;
                string DealID = "";

                for (int j = 0; j < GrdDealDetails.Rows.Count; j++)
                {
                    if (((CheckBox)GrdDealDetails.Rows[j].FindControl("ChkSelect")).Checked)
                    {
                        DealID = ((Label)GrdDealDetails.Rows[j].FindControl("lblDealID")).Text.Trim();
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
                if (txtDealFrom.Text == "")
                {
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dt;

                try
                {
                    string day = txtDealFrom.Text.Substring(0, 2);
                    string mon = txtDealFrom.Text.Substring(3, 2);
                    string yr = txtDealFrom.Text.Substring(6, 4);
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
                if (txtDealTo.Text == "")
                {
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dtto;

                try
                {
                    string day = txtDealTo.Text.Substring(0, 2);
                    string mon = txtDealTo.Text.Substring(3, 2);
                    string yr = txtDealTo.Text.Substring(6, 4);
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

                object[] DealInfo = new object[6];
                int d = 0;

                DealInfo.SetValue(DealID, d);
                d++;

                DealInfo.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), d);
                d++;

                DealInfo.SetValue(Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), d);
                d++;

                string UserName = Session["UserName"].ToString();
                DealInfo.SetValue(UserName, d);
                d++;

                DealInfo.SetValue(System.DateTime.Now, d);
                d++;

                DealInfo.SetValue("Confirm", d);


                try
                {
                    DataSet dset = objBAL.ApplyAgentDeals(DealInfo);

                    if (dset != null)
                    {
                        if (dset.Tables.Count > 0)
                        {
                            if (dset.Tables[2].Rows.Count > 0)
                            {
                                Session["DealData"] = dset;
                                btnPrintKickBackDeal.Visible = true;
                                btnPrintApplicableTonnageDeal.Visible = true;
                                btnPrintFlatAWBRate.Visible = true;
                                btnApplyDeals.Visible = false;
                                btnPrintAWBSummary.Visible = true;
                                lblStatus.Text = "Deal Applied Successfully";
                                lblStatus.ForeColor = Color.Green;
                            }
                            else
                            {
                                lblStatus.Text = "No AWB's found for the agent to apply deals!!";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }

                            //string Kick=dset.Tables[dset.Tables.Count-3].Rows[1]["Description"].ToString();
                            //string Flat = dset.Tables[dset.Tables.Count - 2].Rows[1]["Description"].ToString();

                            //DataDynamics.Reports.ReportDefinition _reportDef1 = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/TonnageDeal.rdlx")));

                            //DataTable dtable = dset.Tables[dset.Tables.Count - 4];
                            //DataTable Dt = dset.Tables[dset.Tables.Count - 1];
                            //Dataset1.Tables.Add(Dt.Copy());
                            //Dataset2.Tables.Add(dtable.Copy());
                            //DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef1);
                            //_reportRuntime.LocateDataSource += WRAXBDetails_LocateDataSource;
                            //string exportFile = System.IO.Path.GetTempFileName() + ".pdf";
                            //System.IO.FileInfo myFile = new System.IO.FileInfo(exportFile);
                            //System.Collections.Specialized.NameValueCollection settings = new System.Collections.Specialized.NameValueCollection();
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

                            //PdfRenderingExtension _renderingExtension = new DataDynamics.Reports.Rendering.Pdf.PdfRenderingExtension();
                            //FileStreamProvider _provider = new DataDynamics.Reports.Rendering.IO.FileStreamProvider(myFile.Directory, Path.GetFileNameWithoutExtension(myFile.Name));
                            //_reportRuntime.Render(_renderingExtension, _provider, settings);
                            //Response.Clear();
                            //Response.ContentType = "application/pdf";
                            //string filename = Session["UserName"].ToString() + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".pdf";
                            //string FullName = filename.Replace(" ", "");
                            //Response.AddHeader("content-disposition", "attachment; filename=" + FullName);
                            //Response.BinaryWrite(File.ReadAllBytes(exportFile));
                            //myFile.Delete();

                            //if(Kick!="")
                            //{
                            //    DataDynamics.Reports.ReportDefinition _reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/KickDeal.rdlx")));

                            //    DataTable dtable1 = dset.Tables[dset.Tables.Count - 3];
                            //    //DataTable Dt = dset.Tables[dset.Tables.Count - 1];
                            //    //Dataset1.Tables.Add(Dt.Copy());
                            //    Dataset2 = new DataSet();
                            //    Dataset2.Tables.Add(dtable1.Copy());
                            //    DataDynamics.Reports.ReportRuntime _reportRuntime1 = new DataDynamics.Reports.ReportRuntime(_reportDef);
                            //    _reportRuntime1.LocateDataSource += WRAXBDetails_LocateDataSource;
                            //    string exportFile1 = System.IO.Path.GetTempFileName() + ".pdf";
                            //    System.IO.FileInfo myFile1 = new System.IO.FileInfo(exportFile1);
                            //    System.Collections.Specialized.NameValueCollection settings1 = new System.Collections.Specialized.NameValueCollection();
                            //    settings1.Add("hideToolbar", "True");
                            //    settings1.Add("hideMenubar", "True");
                            //    settings1.Add("hideWindowUI", "True");
                            //    settings1.Add("MarginLeft", "0.1in");
                            //    settings1.Add("MarginRight", "0.1in");
                            //    settings1.Add("MarginTop", "0.1in");
                            //    settings1.Add("MarginBottom", "0.1in");
                            //    settings1.Add("PageWidth", "9in");
                            //    settings1.Add("PageHeight", "13in");
                            //    settings1.Add("FitWindow", "True");

                            //    PdfRenderingExtension _renderingExtension1 = new DataDynamics.Reports.Rendering.Pdf.PdfRenderingExtension();
                            //    FileStreamProvider _provider1 = new DataDynamics.Reports.Rendering.IO.FileStreamProvider(myFile1.Directory, Path.GetFileNameWithoutExtension(myFile1.Name));
                            //    _reportRuntime1.Render(_renderingExtension1, _provider1, settings1);
                            //    Response.Clear();
                            //    Response.ContentType = "application/pdf";
                            //    string filename1 = Session["UserName"].ToString()  + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".pdf";
                            //    string FullName1 = filename1.Replace(" ", "");
                            //    Response.AddHeader("content-disposition", "attachment; filename=" + FullName1);
                            //    Response.BinaryWrite(File.ReadAllBytes(exportFile1));
                            //    myFile1.Delete();
                            //}

                            //if (Flat != "")
                            //{
                            //    DataDynamics.Reports.ReportDefinition _reportDef2 = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/KickDeal.rdlx")));

                            //    DataTable dtable1 = dset.Tables[dset.Tables.Count - 2];
                            //    //DataTable Dt = dset.Tables[dset.Tables.Count - 1];
                            //    //Dataset1.Tables.Add(Dt.Copy());
                            //    Dataset2 = new DataSet();
                            //    Dataset2.Tables.Add(dtable1.Copy());
                            //    DataDynamics.Reports.ReportRuntime _reportRuntime2 = new DataDynamics.Reports.ReportRuntime(_reportDef2);
                            //    _reportRuntime2.LocateDataSource += WRAXBDetails_LocateDataSource;
                            //    string exportFile2 = System.IO.Path.GetTempFileName() + ".pdf";
                            //    System.IO.FileInfo myFile2 = new System.IO.FileInfo(exportFile2);
                            //    System.Collections.Specialized.NameValueCollection settings2 = new System.Collections.Specialized.NameValueCollection();
                            //    settings2.Add("hideToolbar", "True");
                            //    settings2.Add("hideMenubar", "True");
                            //    settings2.Add("hideWindowUI", "True");
                            //    settings2.Add("MarginLeft", "0.1in");
                            //    settings2.Add("MarginRight", "0.1in");
                            //    settings2.Add("MarginTop", "0.1in");
                            //    settings2.Add("MarginBottom", "0.1in");
                            //    settings2.Add("PageWidth", "9in");
                            //    settings2.Add("PageHeight", "13in");
                            //    settings2.Add("FitWindow", "True");

                            //    PdfRenderingExtension _renderingExtension2 = new DataDynamics.Reports.Rendering.Pdf.PdfRenderingExtension();
                            //    FileStreamProvider _provider2 = new DataDynamics.Reports.Rendering.IO.FileStreamProvider(myFile2.Directory, Path.GetFileNameWithoutExtension(myFile2.Name));
                            //    _reportRuntime2.Render(_renderingExtension2, _provider2, settings2);
                            //    Response.Clear();
                            //    Response.ContentType = "application/pdf";
                            //    string filename2 = Session["UserName"].ToString() + DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".pdf";
                            //    string FullName2 = filename2.Replace(" ", "");
                            //    Response.AddHeader("content-disposition", "attachment; filename=" + FullName2);
                            //    Response.BinaryWrite(File.ReadAllBytes(exportFile2));
                            //    myFile2.Delete();
                            //}
                            
                            //DataDynamics.Reports.ReportDefinition[] _reportDef=new ReportDefinition[3];
                            //_reportDef[0]= new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/TonnageDeal.rdlx")));
                            //_reportDef[1] = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/KickDeal.rdlx")));
                            //_reportDef[2] = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/FlatDeal.rdlx")));
                            //for (int count = 0; count < _reportDef.Length; count++)
                            //{
                            //    //Dataset1 = null;
                            //    //Dataset2 = null;
                            //    //Dataset1 = new DataSet();
                            //    //Dataset2 = new DataSet();
                            //    DataTable dtable = dset.Tables[dset.Tables.Count - (4-(count))];
                            //    DataTable Dt = dset.Tables[dset.Tables.Count - 1];
                            //    Dataset1.Tables.Add(Dt.Copy());
                            //    Dataset2.Tables.Add(dtable.Copy());
                            //    DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef[count]);
                            //    _reportRuntime.LocateDataSource += WRAXBDetails_LocateDataSource;
                            //    string exportFile = System.IO.Path.GetTempFileName() + ".pdf";
                            //    System.IO.FileInfo myFile = new System.IO.FileInfo(exportFile);
                            //    System.Collections.Specialized.NameValueCollection settings = new System.Collections.Specialized.NameValueCollection();
                            //    settings.Add("hideToolbar", "True");
                            //    settings.Add("hideMenubar", "True");
                            //    settings.Add("hideWindowUI", "True");
                            //    settings.Add("MarginLeft", "0.1in");
                            //    settings.Add("MarginRight", "0.1in");
                            //    settings.Add("MarginTop", "0.1in");
                            //    settings.Add("MarginBottom", "0.1in");
                            //    settings.Add("PageWidth", "9in");
                            //    settings.Add("PageHeight", "13in");
                            //    settings.Add("FitWindow", "True");

                            //    PdfRenderingExtension _renderingExtension = new DataDynamics.Reports.Rendering.Pdf.PdfRenderingExtension();
                            //    FileStreamProvider _provider = new DataDynamics.Reports.Rendering.IO.FileStreamProvider(myFile.Directory, Path.GetFileNameWithoutExtension(myFile.Name));
                            //    _reportRuntime.Render(_renderingExtension, _provider, settings);
                            //    Response.Clear();
                            //    Response.ContentType = "application/pdf";
                            //    string filename = Session["UserName"].ToString() +(count+1).ToString()+ DateTime.Now.ToString("mmddyyy hh:MM:ss") + ".pdf";
                            //    string FullName = filename.Replace(" ", "");
                            //    Response.AddHeader("content-disposition", "attachment; filename=" + FullName);
                            //    Response.BinaryWrite(File.ReadAllBytes(exportFile));
                            //    myFile.Delete();
                            //}
                                

                            

                        }
                    }
                }
                catch
                {
                    lblStatus.Text = "Error while applying deals; Please try again";
                    lblStatus.ForeColor = Color.Red;
                    return;
                }


            }
            catch (Exception ex)
            {


            }

        }

        #region Button Preview 
        protected void btnPreviewDeal_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";




            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                lblStatus.ForeColor = Color.Red;
 
            }

        }
        #endregion

        #region GrdDealDetails RowCommand
        protected void GrdDealDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                if (e.CommandName == "Tonnage")
                {
                    string DealId = e.CommandArgument.ToString();

                    DataSet ds = db.SelectRecords("SpGetTonnageDetailsSlab", "DealId", DealId, SqlDbType.VarChar);

                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grdTonnageSlab.DataSource = ds;
                                grdTonnageSlab.DataBind();
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplitTonnage();</script>", false); 

 
                            }
                        }
                    }
                    
                }
                else
                    if (e.CommandName == "Exclusion")
                    {
                        string DealId = e.CommandArgument.ToString();

                        DataSet ds = db.SelectRecords("SpGetExclusionDetails", "DealId", DealId, SqlDbType.VarChar);

                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    grdExclusion.DataSource = ds;
                                    grdExclusion.DataBind();
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplit();</script>", false); 


                                }
                            }
                        }

                    }
                    else
                        if (e.CommandName == "DealDetails")
                        {
                            string DealId = e.CommandArgument.ToString();

                            DataSet ds = db.SelectRecords("SpGetExclusionDetails", "DealId", DealId, SqlDbType.VarChar);

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
                if (e.CommandName == "Preview")
                {


                    try
                    {
                        //To check if deals are available or not
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
                        if (txtDealFrom.Text == "")
                        {
                            lblStatus.Text = "Please select Valid date";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                        DateTime dt;

                        try
                        {
                            string day = txtDealFrom.Text.Substring(0, 2);
                            string mon = txtDealFrom.Text.Substring(3, 2);
                            string yr = txtDealFrom.Text.Substring(6, 4);
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
                        if (txtDealTo.Text == "")
                        {
                            lblStatus.Text = "Please select Valid date";
                            lblStatus.ForeColor = Color.Blue;
                            return;
                        }

                        DateTime dtto;

                        try
                        {
                            string day = txtDealTo.Text.Substring(0, 2);
                            string mon = txtDealTo.Text.Substring(3, 2);
                            string yr = txtDealTo.Text.Substring(6, 4);
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

                        object[] DealInfo = new object[6];
                        int d = 0;

                        DealInfo.SetValue(e.CommandArgument.ToString(), d);
                        d++;

                        DealInfo.SetValue(Convert.ToDateTime(strfromdate).ToString("yyyy-MM-dd HH:mm:ss"), d);
                        d++;

                        DealInfo.SetValue(Convert.ToDateTime(strtodate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), d);
                        d++;

                        string UserName = Session["UserName"].ToString();
                        DealInfo.SetValue(UserName, d);
                        d++;

                        DealInfo.SetValue(System.DateTime.Now, d);
                        d++;

                        DealInfo.SetValue("", d);

                        try
                        {
                            DataSet dset = objBAL.ApplyAgentDeals(DealInfo);

                            if (dset != null)
                            {
                                if (dset.Tables.Count > 0)
                                {
                                    if (dset.Tables[2].Rows.Count > 0)
                                    {
                                        


                                        //rptTonnageDeal.Visible = true;
                                        info = new FileInfo(Server.MapPath("/Reports/TonnageDeal.rdlx"));
                                        ////definition = new ReportDefinition(info);
                                        ////runtime = new ReportRuntime(definition);
                                        DataTable dtable = dset.Tables[dset.Tables.Count - 5];
                                        DataTable Dt = dset.Tables[dset.Tables.Count - 1];
                                        //DataTable dtAWB = dset.Tables[dset.Tables.Count - 2];
                                        //Dataset1.Tables.Add(Dt.Copy());
                                        //Dataset2.Tables.Add(dtable.Copy());
                                        //Dataset3.Tables.Add(dtAWB.Copy());
                                        Dataset1.Tables.Add(Dt.Copy());
                                        Dataset2.Tables.Add(dtable.Copy());
                                        ////runtime.LocateDataSource += WRAXBDetails_LocateDataSource;
                                        ////rptTonnageDeal.SetReport(runtime);


                                        //btnExport.Visible = true;
                                        //grdPreview.DataSource = ds.Tables[15];
                                        //grdPreview.DataBind();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>ViewPanelSplitPreview();</script>", false);


                                    }
                                    else
                                    {
                                        lblStatus.Text = "No Data found for the agent to apply deals!!";
                                        lblStatus.ForeColor = Color.Red;
                                        return;
                                    }
                                }
                            }
                          
                        }
                        catch
                        {
                            lblStatus.Text = "Error while applying deals; Please try again";
                            lblStatus.ForeColor = Color.Red;
                            return;
                        }
                      

                    }
                    catch (Exception ex)
                    {


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

        #region Button List
        protected void btnList_Click(object sender, EventArgs e)
        {

            try
            {
                lblStatus.Text = "";
                btnApplyDeals.Visible = false;
                btnPrintApplicableTonnageDeal.Visible = false;
                btnPrintFlatAWBRate.Visible = false;
                btnPrintKickBackDeal.Visible = false;
                btnPrintAWBSummary.Visible = false;
                //To check if deals are available or not
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
                if (txtDealFrom.Text == "")
                {
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dt;

                try
                {
                    string day = txtDealFrom.Text.Substring(0, 2);
                    string mon = txtDealFrom.Text.Substring(3, 2);
                    string yr = txtDealFrom.Text.Substring(6, 4);
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
                if (txtDealTo.Text == "")
                {
                    lblStatus.Text = "Please select Valid date";
                    lblStatus.ForeColor = Color.Blue;
                    return;
                }

                DateTime dtto;

                try
                {
                    string day = txtDealTo.Text.Substring(0, 2);
                    string mon = txtDealTo.Text.Substring(3, 2);
                    string yr = txtDealTo.Text.Substring(6, 4);
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

                ds = objBAL.GetAvailableDealList(AWBInfo);
                string[] QueryN=new string[4];
                object[] QueryV=new object[4];
                SqlDbType[] QueryT=new SqlDbType[4];

                QueryN[0]="DealId";
                QueryN[1]="FromDate";
                QueryN[2]="ToDate";
                QueryN[3]="Selection";
                QueryT[0]=SqlDbType.VarChar;
                QueryT[1]=SqlDbType.VarChar;
                QueryT[2]=SqlDbType.VarChar;
                QueryT[3]=SqlDbType.VarChar;
                QueryV[1]=AWBInfo[1];
                QueryV[2]=AWBInfo[2];
                QueryV[3]="";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //If Deals are available
                    //string res;
                    string[] DealAmount = new string[0];
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        QueryV[0]=ds.Tables[0].Rows[j]["DealID"].ToString();
                        DataSet Amount = db.SelectRecords("SP_GettingDealValue", QueryN, QueryV, QueryT);
                        Array.Resize(ref DealAmount, DealAmount.Length + 1);
                        DealAmount[DealAmount.Length - 1] = Amount.Tables[0].Rows[0][0].ToString();
                    }
                    
                    GrdDealDetails.DataSource = ds;
                    GrdDealDetails.DataBind();
                    btnApplyDeals.Visible = true;
                    btnClose.Visible = true;
                    for (int j = 0; j < GrdDealDetails.Rows.Count; j++)
                    {
                        if (DealAmount[j] == "")
                        {
                            ((Label)GrdDealDetails.Rows[j].FindControl("lblDealAmount")).Text = "N/A";
                        }
                        else
                        {
                            ((Label)GrdDealDetails.Rows[j].FindControl("lblDealAmount")).Text = DealAmount[j];
                        }
                    }

                }
                else
                {
                    lblStatus.Text = "No deals available";
                    lblStatus.ForeColor = Color.Blue;
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

        #region Button Print Applicable Tonnage Report
        protected void btnPrintApplicableTonnageDeal_Click(object sender, EventArgs e)
        {

            try
            {
                if (Session["DealData"] != null)
                {
                    DataSet dset = (DataSet)Session["DealData"];
                    if (dset != null)
                    {
                        if (dset.Tables.Count > 0)
                        {
                            if (dset.Tables[2].Rows.Count > 0)
                            {
                                Session["DealData"] = dset;
                                ////DataDynamics.Reports.ReportDefinition _reportDef = new ReportDefinition();
                                //DataDynamics.Reports.ReportDefinition _reportDef1 = new ReportDefinition();
                                ////_reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/TonnageDeal.rdlx")));
                               // _reportDef1 = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/ExcludedDetails.rdlx")));

                                DataTable dtable = dset.Tables[dset.Tables.Count - 5];
                                DataTable Dt = dset.Tables[dset.Tables.Count - 1];
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

                                ///PdfRenderingExtension _renderingExtension = new DataDynamics.Reports.Rendering.Pdf.PdfRenderingExtension();
                                ///FileStreamProvider _provider = new DataDynamics.Reports.Rendering.IO.FileStreamProvider(myFile.Directory, Path.GetFileNameWithoutExtension(myFile.Name));
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
                                lblStatus.Text = "No AWB's found for the agent to apply deals!!";
                                lblStatus.ForeColor = Color.Red;
                                return;
                            }

                        }
                    }
                }
            }
            catch
            {
                lblStatus.Text = "Error while printing deal report; Please try again";
                lblStatus.ForeColor = Color.Red;
                return;
            }

        }
        #endregion

        #region Button Print Kick Back Deal Report
        protected void btnPrintKickBackDeal_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["DealData"] != null)
                {
                    DataSet dset = (DataSet)Session["DealData"];
                    if (dset != null)
                    {
                        if (dset.Tables.Count > 0)
                        {
                            string Kick = dset.Tables[dset.Tables.Count - 4].Rows[1]["Description"].ToString();
                            if (Kick != "")
                            {
                                Session["DealData"] = dset;
                                //string Flat = dset.Tables[dset.Tables.Count - 2].Rows[1]["Description"].ToString();

                                ////DataDynamics.Reports.ReportDefinition _reportDef = new ReportDefinition();
                                ////_reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/KickDeal.rdlx")));

                                DataTable dtable = dset.Tables[dset.Tables.Count - 4];
                                DataTable Dt = dset.Tables[dset.Tables.Count - 1];
                                Dataset1.Tables.Add(Dt.Copy());
                                Dataset2.Tables.Add(dtable.Copy());
                                ////DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef);
                                ////_reportRuntime.LocateDataSource += WRAXBDetails_LocateDataSource;
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
                                lblStatus.Text = "No AWB's Excluded!";
                                lblStatus.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            lblStatus.Text = "No AWB's present for the report to process!";
                            lblStatus.ForeColor = Color.Red;
                        }
                    }
                }
            }
            catch
            {
                lblStatus.Text = "Error while printing deal report; Please try again";
                lblStatus.ForeColor = Color.Red;
                return;
            }

        }
        #endregion

        #region Button Print Flat AWB Rate Report
        protected void btnPrintFlatAWBRate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["DealData"] != null)
                {
                    DataSet dset = (DataSet)Session["DealData"];
                    if (dset != null)
                    {
                        if (dset.Tables.Count > 0)
                        {
                            string Flat = dset.Tables[dset.Tables.Count - 3].Rows[1]["Description"].ToString();
                            if (Flat != "")
                            {
                                Session["DealData"] = dset;

                                ////DataDynamics.Reports.ReportDefinition _reportDef = new ReportDefinition();
                                ////_reportDef = new DataDynamics.Reports.ReportDefinition(new FileInfo(Server.MapPath("/Reports/FlatDeal.rdlx")));

                                DataTable dtable = dset.Tables[dset.Tables.Count - 3];
                                DataTable Dt = dset.Tables[dset.Tables.Count - 1];
                                Dataset1.Tables.Add(Dt.Copy());
                                Dataset2.Tables.Add(dtable.Copy());
                                ////DataDynamics.Reports.ReportRuntime _reportRuntime = new DataDynamics.Reports.ReportRuntime(_reportDef);
                                ////_reportRuntime.LocateDataSource += WRAXBDetails_LocateDataSource;
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
                                lblStatus.Text = "No AWB's Excluded!";
                                lblStatus.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            lblStatus.Text = "No AWB's present for the report to process!";
                            lblStatus.ForeColor = Color.Red;
                        }
                    }
                }
            }
            catch
            {
                lblStatus.Text = "Error while printing deal report; Please try again";
                lblStatus.ForeColor = Color.Red;
                return;
            }
        }
        #endregion

        #region Button Print AWB Summary Tonnage Deal
        protected void btnPrintAWBSummary_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["DealData"] != null)
                {
                    DataSet dset = (DataSet)Session["DealData"];
                    if (dset != null)
                    {
                        if (dset.Tables.Count > 0)
                        {
                            if (dset.Tables[dset.Tables.Count - 2].Rows.Count > 0)
                            {
                                
                                    Session["DealData"] = dset;

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
                GrdDealDetails.DataSource = null;
                GrdDealDetails.DataBind();
                ddlAgentCode.SelectedIndex = 0;
                txtDealFrom.Text = "";
                txtDealTo.Text = "";
                btnPrintApplicableTonnageDeal.Visible = false;
                btnPrintAWBSummary.Visible = false;
                btnPrintFlatAWBRate.Visible = false;
                btnPrintKickBackDeal.Visible = false;
                btnApplyDeals.Visible = false;
                btnClose.Visible = false;
                Session["DealData"] = null;
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
