using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BAL;
using System.Drawing;
using QID.DataAccess;
using System.IO;


namespace ProjectSmartCargoManager
{
    public partial class ExceptionMonitoring : System.Web.UI.Page
    {

        BALException objBAL = new BALException();
        ReportBAL objReport = new ReportBAL();
        SQLServer da = new SQLServer(Global.GetConnectionString());
        string strfromdate = "", strtodate = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetOrigin();
                GetDestination();
                txtvalidfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtvalidto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                btnExport.Visible = true;
                //GetAgentCode();
            }
        }

        # region Get Origin List

        private void GetOrigin()
        {
            try
            {
                DataSet ds = objBAL.GetOriginCodeList(ddlOrigin.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlOrigin.DataSource = ds;
                            ddlOrigin.DataMember = ds.Tables[0].TableName;
                            ddlOrigin.DataValueField = "AirPortCode";//ds.Tables[0].Columns["Code"].ColumnName;

                            ddlOrigin.DataTextField = "AirPort";//ds.Tables[0].Columns["Code"].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, "ALL");
                            ddlOrigin.Items[0].Value = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion GetOriginCode List

        # region Get destination List

        private void GetDestination()
        {
            try
            {
                DataSet ds = objBAL.GetOriginCodeList(ddlDest.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlDest.DataSource = ds;
                            ddlDest.DataMember = ds.Tables[0].TableName;
                            ddlDest.DataValueField = "AirPortCode";//ds.Tables[0].Columns["Code"].ColumnName;

                            ddlDest.DataTextField = "AirPort";// ds.Tables[0].Columns["Code"].ColumnName;
                            ddlDest.DataBind();
                            ddlDest.Items.Insert(0, "ALL");
                            ddlDest.Items[0].Value = "";

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion GetDestinationCode List

        //# region Get AgentCode List

        //private void GetAgentCode()
        //{
        //    try
        //    {
        //        DataSet ds = objBAL.GetAgentCodeList(ddlAgentCode.SelectedValue);
        //        if (ds != null)
        //        {
        //            if (ds.Tables != null)
        //            {
        //                if (ds.Tables.Count > 0)
        //                {

        //                    ddlAgentCode.DataSource = ds;
        //                    ddlAgentCode.DataMember = ds.Tables[1].TableName;
        //                    ddlAgentCode.DataValueField = ds.Tables[1].Columns["AgentCode"].ColumnName;

        //                    ddlAgentCode.DataTextField = ds.Tables[1].Columns["AgentCode"].ColumnName;
        //                    ddlAgentCode.DataBind();
        //                    ddlAgentCode.Items.Insert(0, "All");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //# endregion GetDestinationCode List 



        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtvalidfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtvalidto.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFLAB.Text = "";
            txtFLABper.Text = "";
            txtNotFLAB.Text = "";
            txtTotalAcceptedAWBs.Text = "";
            //txtTotalBookedAWBs.Text = "";
            
            //ddlAgentCode.SelectedIndex = 0;
            ddlDest.SelectedIndex = 0;
            ddlOrigin.SelectedIndex = 0;
            grvNFLABList.Visible = false;
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                Session["dsExp"] = null;
                grvNFLABList.Visible = false;
                lblStatus.Text = string.Empty;

                if (Validate() == false)
                {
                    Session["dsExp"] = null;
                    grvNFLABList.Visible = false;
                    //return;
                }
                string AgentCode;

                int i = 0;
                //Origin = ddlOrigin.SelectedItem.Text.Trim() == "All" ? "" : ddlOrigin.SelectedItem.Text.Trim();
                //Dest = ddlDest.SelectedItem.Text.Trim() == "All" ? "" : ddlDest.SelectedItem.Text.Trim();

                string Origin = "";
                if (ddlOrigin.SelectedItem.Text.Trim() != "ALL")
                {
                    Origin = ddlOrigin.SelectedValue.ToString().Trim();
                
                }
                string Dest = "";
                if (ddlDest.SelectedItem.Text.Trim() != "ALL")
                {
                    Dest = ddlDest.SelectedValue.ToString().Trim();
                }
                #region Parametes
                string[] PName = new string[4];
                PName[0] = "Origin";
                PName[1] = "Dest";
                PName[2] = "ValidFrom";
                PName[3] = "ValidTo";
                //PName[4] = "AgentCode";

                object[] paramvalue = new object[4];
                //object[] ProRateListInfo = new object[2];

                paramvalue[0] = Origin;
                paramvalue[1] = Dest;
                if (txtvalidfrom.Text != "")
                {
                    strfromdate = txtvalidfrom.Text.Trim();
                    paramvalue[2] = strfromdate;//Convert.ToDateTime(strfromdate).ToString("dd/MM/yyyy");
                    //= strfromdate;
                }

                if (txtvalidto.Text != "")
                {
                    strtodate = txtvalidto.Text.Trim();
                    paramvalue[3] = strtodate;//Convert.ToDateTime(strtodate).ToString("dd/MM/yyyy");
                    //paramvalue[3] = strtodate;
                }
                //AgentCode = ddlAgentCode.SelectedItem.Text.Trim() == "All" ? "" : ddlAgentCode.SelectedItem.Text.Trim();
                //paramvalue[4] = AgentCode;

                //SqlDbType[] paramtype = new SqlDbType[5];
                SqlDbType[] paramtype = new SqlDbType[4];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                //paramtype[4] = SqlDbType.VarChar;
                #endregion



                DataSet ds = da.SelectRecords("SP_GETFLABDETAILS", PName, paramvalue, paramtype);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            txtTotalAcceptedAWBs.Text = ds.Tables[3].Rows[0]["Accepted"].ToString();
                        }
                        //if (ds.Tables[1].Rows.Count > 0)
                        //{
                        //    txtTotalBookedAWBs.Text = ds.Tables[1].Rows[0]["Booked"].ToString();
                        //}
                        //if (ds.Tables[2].Rows.Count > 0)
                        //{
                        //    txtFLAB.Text = ds.Tables[2].Rows[0]["FLAB"].ToString();
                        //}
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            txtNotFLAB.Text = ds.Tables[2].Rows[0]["NotFLAB"].ToString();
                        }

                    }

                }



                //float bookedpcs = Convert.ToInt32(txtTotalBookedAWBs.Text);
                float acceptedcnt = Convert.ToInt32(txtTotalAcceptedAWBs.Text);
                float nFLAB = Convert.ToInt32(txtNotFLAB.Text);
                float Flab = acceptedcnt - nFLAB;
                float result;


                txtFLAB.Text = Flab.ToString();
                result = (Flab * 100) / acceptedcnt;

                txtFLABper.Text = result.ToString("0.00");


                DataSet dsFLAB = da.SelectRecords("SP_GETFLABDETAILS", PName, paramvalue, paramtype);
                //DataSet dsFLAB = da.SelectRecords("GetOffloadAWBNo",PName,paramvalue,paramtype);
                //try
                //{
                //    for (int k; k < dsFLAB.Tables[0].Rows.Count; k++)
                //    {
                //        string AWBNo = dsFLAB.Tables[0].Rows[0]["AWBNumber"].ToString();

                //        object[] paramvalueoff = new object[1];
                //        paramvalueoff[0] = AWBNo;
                //        string[] PNameoff = new string[1];
                //        PNameoff[0] = "AWBNo";
                //        SqlDbType[] paramtypeoff = new SqlDbType[1];
                //        paramtypeoff[0] = SqlDbType.VarChar;

                //        DataSet dsOffload = da.SelectRecords("Sp_GetOffloadDetails", PNameoff, paramvalueoff, paramtypeoff);
                //        if (dsOffload != null)
                //        {
                //            if (dsOffload.Tables != null)
                //            {
                //                if (dsOffload.Tables.Count > 0)
                //                {
                //                    if (dsOffload.Tables[0].Rows.Count > 0)
                //                    {
                //                        grvNFLABList.PageIndex = 0;
                //                        grvNFLABList.DataSource = dsOffload;
                //                        grvNFLABList.DataMember = dsOffload.Tables[0].TableName;
                //                        grvNFLABList.DataBind();
                //                        grvNFLABList.Visible = true;
                //                        //dsFLAB.Clear();
                //                        //btnExport.Visible = true;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{ }
                try
                {
                    Session["dsExp"] = dsFLAB;
                    if (dsFLAB != null)
                    {
                        if (dsFLAB.Tables != null)
                        {
                            if (dsFLAB.Tables.Count > 0)
                            {
                                if (dsFLAB.Tables[0].Rows.Count > 0)
                                {
                                    grvNFLABList.PageIndex = 0;
                                    grvNFLABList.DataSource = dsFLAB;
                                    grvNFLABList.DataMember = dsFLAB.Tables[0].TableName;
                                    grvNFLABList.DataBind();
                                    grvNFLABList.Visible = true;
                                    //dsFLAB.Clear();
                                    btnExport.Visible = true;

                                    //for (int j = 0; j < grvNFLABList.Rows.Count; j++)
                                    //{
                                    //    if (((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text.ToString() == "True")
                                    //    {
                                    //        ((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text = "Loaded";
                                    //    }

                                    //    else if (((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text.ToString() == "False")
                                    //    {
                                    //        ((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text = "Not Loaded";
                                    //    }
                                    //}
                                }
                                else
                                {
                                    lblStatus.Text = "No Data Found";
                                    txtFLAB.Text = "";
                                    txtFLABper.Text = "";
                                    txtNotFLAB.Text = "";
                                    txtTotalAcceptedAWBs.Text = "";
                                    lblStatus.ForeColor = Color.Red;
                                    grvNFLABList.Visible = false;
                                    
                                }
                            }
                        }
                    }
                }



            //}
                catch (Exception ex)
                { }
            }
            catch (Exception ex)
            { }
        }
        # region grvNFLABList_RowCommand
        protected void grvNFLABList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Manage")
                {
                    lblStatus.Text = "";
                    //int RowIndex = Convert.ToInt32(e.CommandArgument);
                    //Label lblAWBNumber = (Label)grvNFLABList.Rows[RowIndex].FindControl("lblAWBNumber");
                    //Label lblOrigin = (Label)grvNFLABList.Rows[RowIndex].FindControl("lblOrigin");
                    //Label lblDestination = (Label)grvNFLABList.Rows[RowIndex].FindControl("lblDestination");
                    ////Label lblFromDate = (Label)grvNFLABList.Rows[RowIndex].FindControl("lblFromDate");
                    ////Label lblToDate = (Label)grvNFLABList.Rows[RowIndex].FindControl("lblToDate");
                    //Label lblAgentCode = (Label)grvNFLABList.Rows[RowIndex].FindControl("lblAgentCode");



                    //btnSave.Text = "Update";

                }
            }
            catch (Exception ex)
            {

            }

        }
        # endregion grvNFLABList_RowCommand

        # region grvNFLABList_RowEditing
        protected void grvNFLABList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grvNFLABList_RowEditing

        # region grvNFLABList_PageIndexChanging
        protected void grvNFLABList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string  AgentCode;

            int i = 0;

            string Origin = "";
            if (ddlOrigin.SelectedItem.Text.Trim() != "")
            {
                Origin = ddlOrigin.SelectedValue.ToString().Trim();

            }
            string Dest = "";
            if (ddlDest.SelectedItem.Text.Trim() != "")
            {

                Dest = ddlDest.SelectedValue.ToString().Trim();
            }
            //Origin = ddlOrigin.SelectedItem.Text.Trim() == "All" ? "" : ddlOrigin.SelectedItem.Text.Trim();
            //Dest = ddlDest.SelectedItem.Text.Trim() == "All" ? "" : ddlDest.SelectedItem.Text.Trim();
            #region Parametes
            string[] PName = new string[4];
            PName[0] = "Origin";
            PName[1] = "Dest";
            PName[2] = "ValidFrom";
            PName[3] = "ValidTo";
            //PName[4] = "AgentCode";

            object[] paramvalue = new object[4];
            //object[] ProRateListInfo = new object[2];

            paramvalue[0] = Origin;
            paramvalue[1] = Dest;
            if (txtvalidfrom.Text != "")
            {
                strfromdate = txtvalidfrom.Text.Trim();
               // paramvalue[2] = Convert.ToDateTime(strfromdate).ToString("dd/MM/yyyy");
                //= strfromdate;
                paramvalue[2] = strfromdate;//.ToString("dd/MM/yyyy 00:00:00.000");
                }

            if (txtvalidto.Text != "")
            {
                strtodate = txtvalidto.Text.Trim();
                paramvalue[3] =strtodate;//.ToString("dd/MM/yyyy");
                //paramvalue[3] = strtodate;
            }
            //AgentCode = ddlAgentCode.SelectedItem.Text.Trim() == "All" ? "" : ddlAgentCode.SelectedItem.Text.Trim();
            //paramvalue[4] = AgentCode;

            SqlDbType[] paramtype = new SqlDbType[4];
            paramtype[0] = SqlDbType.VarChar;
            paramtype[1] = SqlDbType.VarChar;
            paramtype[2] = SqlDbType.VarChar;
            paramtype[3] = SqlDbType.VarChar;
            //paramtype[4] = SqlDbType.VarChar;
            #endregion


            DataSet dsFLAB = da.SelectRecords("SP_GETFLABDETAILS", PName, paramvalue, paramtype);
            //DataSet dsFLAB = da.SelectRecords("GetOffloadAWBNo", PName, paramvalue, paramtype);

            if (dsFLAB != null)
            {
                if (dsFLAB.Tables != null)
                {
                    if (dsFLAB.Tables.Count > 0)
                    {
                        if (dsFLAB.Tables[0].Rows.Count > 0)
                        {
                            grvNFLABList.PageIndex = 0;
                            grvNFLABList.DataSource = dsFLAB;
                            grvNFLABList.DataMember = dsFLAB.Tables[0].TableName;
                            grvNFLABList.DataBind();
                            grvNFLABList.Visible = true;
                            //dsFLAB.Clear();

                        }
                    }
                }
            }


            grvNFLABList.PageIndex = e.NewPageIndex;
            grvNFLABList.DataSource = dsFLAB.Copy();
            grvNFLABList.DataBind();


            //for (int j = 0; j < grvNFLABList.Rows.Count; j++)
            //{
            //    if (((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text.ToString() == "True")
            //    {
            //        ((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text = "Loaded";
            //    }

            //    else if (((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text.ToString() == "False")
            //    {
            //        ((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text = "Not Loaded";
            //    }
            //}






        }
        # endregion grvNFLABList_PageIndexChanging

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //ExportGridToExcel(grvNFLABList, "StudentMarks.xls");
            DataSet dsExp = null;
            DataTable dt = null;
            lblStatus.Text = "";
            grvNFLABList.Visible = false;
            Session["dsExp"] = null;
            if (Validate() == false)
            {
                grvNFLABList.Visible = false;
                Session["dsExp"] = null;
                return;
            }
            try
            {
                Session["dsExp"] = null;
              
                if ((DataSet)Session["dsExp"] == null)
                    //if(ds == null)
                    GetData();

                dsExp = (DataSet)Session["dsExp"];
                if(dsExp!=null)
                dt = (DataTable)dsExp.Tables[0];

                if (Session["dsExp"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    //SaveUserActivityLog(lblStatus.Text);
                    grvNFLABList.Visible = false;
                    return;
                }
                //dt = city.GetAllCity();//your datatable 
                string attachment = "attachment; filename=Booked vs Flown Report.xls";
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
                //dsExp = null;
                //dt = null;
            }
        }
        //public void ExportGridToExcel(GridView grdGridView, string fileName)
        //{
        //    Response.Clear();
        //    Response.AddHeader("content-disposition",string.Format("attachment;filename={0}.xls", fileName));
        //    Response.Charset = "";
        //    Response.ContentType = "application/vnd.xls";

        //    StringWriter stringWrite = new StringWriter();
        //    HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        //    grdGridView.RenderControl(htmlWrite);
        //    Response.Write(stringWrite.ToString());
        //    Response.End();
        //}

        //protected void btnEx_Click(object sender, EventArgs e)
        //{
        //    ExportGridToExcel(grvNFLABList, "StudentMarks.xls");
        //}

        //public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        //{
        //}
        //private void PrepareGridViewForExport(System.Web.UI.Control gv)
        //{
        //    LinkButton lb = new LinkButton();
        //    Literal l = new Literal();
        //    string name = String.Empty;
        //    for (int i = 0; i < gv.Controls.Count; i++)
        //    {
        //        PrepareGridViewForExport(grvNFLABList.Controls);
        //    }
        //}

        #region Validate Controls
        private bool Validate()
        {
            try
            {
                try
                {
                    if (txtvalidfrom.Text.Trim() != "" || txtvalidto.Text.Trim() != "")
                    {
                        DateTime dt1 = DateTime.ParseExact(txtvalidfrom.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtFromdate.Text);
                        DateTime dt2 = DateTime.ParseExact(txtvalidto.Text, "dd/MM/yyyy", null); //DateTime.Parse(txtToDate.Text);

                        int chk = DateTime.Compare(dt1, dt2);
                        if (chk > 0)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Please enter valid To date";
                            txtvalidfrom.Focus();
                            return false;
                        }


                    }
                }
                catch (Exception ex)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please enter valid date format ex:dd/MM/yyyy";
                    txtvalidfrom.Focus();
                    return false;
                }


                if (ddlOrigin.SelectedItem.Value.ToString() != "All" && ddlDest.SelectedItem.Value.ToString() == "All")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Please select level Type";
                    txtvalidfrom.Focus();
                    return false;
                }
                string strResult = string.Empty;
                try
                {
                    strResult = objReport.GetReportInterval(DateTime.ParseExact(txtvalidfrom.Text.Trim(), "dd/MM/yyyy", null), DateTime.ParseExact(txtvalidto.Text.Trim(), "dd/MM/yyyy", null));
                }
                catch
                {
                    strResult = "";
                }
                if (strResult != "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = strResult;
                    txtvalidfrom.Focus();
                    return false;
                }

            }
            catch (Exception ex)
            {


            }
            return true;
        }
        #endregion
        protected void GetData()
        {
             try
            {
                if (Validate() == false)
                {
                    grvNFLABList.Visible = false;
                    Session["dsExp"] = null;
                    return;
                }
                //string Origin, Dest;

                int i = 0;
                //Origin = ddlOrigin.SelectedItem.Text.Trim() != "All" ? "" : ddlOrigin.SelectedItem.Text.Trim();
                //Dest = ddlDest.SelectedItem.Text.Trim() != "All" ? "" : ddlDest.SelectedItem.Text.Trim();

                string Origin = "";
                if (ddlOrigin.SelectedItem.Text.Trim() != "ALL")
                {
                    Origin = ddlOrigin.SelectedValue.ToString().Trim();

                }
                string Dest = "";
                if (ddlDest.SelectedItem.Text.Trim() != "ALL")
                {
                    Dest = ddlDest.SelectedValue.ToString().Trim();
                }
                 
                 #region Parametes
                string[] PName = new string[4];
                PName[0] = "Origin";
                PName[1] = "Dest";
                PName[2] = "ValidFrom";
                PName[3] = "ValidTo";
                //PName[4] = "AgentCode";

                object[] paramvalue = new object[4];
                //object[] ProRateListInfo = new object[2];

                paramvalue[0] = Origin;
                paramvalue[1] = Dest;
                if (txtvalidfrom.Text != "")
                {
                    strfromdate = txtvalidfrom.Text.Trim();
                    paramvalue[2] = strfromdate;//Convert.ToDateTime(strfromdate).ToString("dd/MM/yyyy");
                    //= strfromdate;
                }

                if (txtvalidto.Text != "")
                {
                    strtodate = txtvalidto.Text.Trim();
                    paramvalue[3] = strtodate;//Convert.ToDateTime(strtodate).ToString("dd/MM/yyyy");
                    //paramvalue[3] = strtodate;
                }
                //AgentCode = ddlAgentCode.SelectedItem.Text.Trim() == "All" ? "" : ddlAgentCode.SelectedItem.Text.Trim();
                //paramvalue[4] = AgentCode;

                //SqlDbType[] paramtype = new SqlDbType[5];
                SqlDbType[] paramtype = new SqlDbType[4];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.VarChar;
                //paramtype[4] = SqlDbType.VarChar;
                #endregion



                DataSet ds = da.SelectRecords("SP_GETFLABDETAILS", PName, paramvalue, paramtype);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            txtTotalAcceptedAWBs.Text = ds.Tables[3].Rows[0]["Accepted"].ToString();
                        }
                        //if (ds.Tables[1].Rows.Count > 0)
                        //{
                        //    txtTotalBookedAWBs.Text = ds.Tables[1].Rows[0]["Booked"].ToString();
                        //}
                        //if (ds.Tables[2].Rows.Count > 0)
                        //{
                        //    txtFLAB.Text = ds.Tables[2].Rows[0]["FLAB"].ToString();
                        //}
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            txtNotFLAB.Text = ds.Tables[2].Rows[0]["NotFLAB"].ToString();
                        }

                    }

                }



                //float bookedpcs = Convert.ToInt32(txtTotalBookedAWBs.Text);
                float acceptedcnt = Convert.ToInt32(txtTotalAcceptedAWBs.Text);
                float nFLAB = Convert.ToInt32(txtNotFLAB.Text);
                float Flab = acceptedcnt - nFLAB;
                float result;


                txtFLAB.Text = Flab.ToString();
                result = (Flab * 100) / acceptedcnt;

                txtFLABper.Text = result.ToString("0.00");


                DataSet dsFLAB = da.SelectRecords("SP_GETFLABDETAILS", PName, paramvalue, paramtype);
              
                try
                {
                    Session["dsExp"] = dsFLAB;
                    if (dsFLAB != null)
                    {
                        if (dsFLAB.Tables != null)
                        {
                            if (dsFLAB.Tables.Count > 0)
                            {
                                if (dsFLAB.Tables[0].Rows.Count > 0)
                                {
                                    grvNFLABList.PageIndex = 0;
                                    grvNFLABList.DataSource = dsFLAB;
                                    grvNFLABList.DataMember = dsFLAB.Tables[0].TableName;
                                    grvNFLABList.DataBind();
                                    grvNFLABList.Visible = true;
                                    //dsFLAB.Clear();
                                    btnExport.Visible = true;

                                    //for (int j = 0; j < grvNFLABList.Rows.Count; j++)
                                    //{
                                    //    if (((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text.ToString() == "True")
                                    //    {
                                    //        ((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text = "Loaded";
                                    //    }

                                    //    else if (((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text.ToString() == "False")
                                    //    {
                                    //        ((Label)(grvNFLABList.Rows[j].FindControl("lblLoadedStatus"))).Text = "Not Loaded";
                                    //    }
                                    //}
                                }
                                else
                                {
                                    lblStatus.Text = "No Data Found";
                                    lblStatus.ForeColor = Color.Red;
                                    grvNFLABList.Visible = false;

                                }
                            }
                        }
                    }
                }



            //}
                catch (Exception ex)
                { }
            }
            catch (Exception ex)
            { }
        
          

        }
        
          public string GetReportInterval(DateTime FromDate, DateTime ToDate)
        {
            double DaysConfigured = 0;
            double intReportInterval = (ToDate - FromDate).TotalDays + 1;
            LoginBL objBL = new LoginBL();

            try
            {
                Session["dsExp"] = null;
                string strOutput = objBL.GetMasterConfiguration("ReportInterval");

                if (strOutput != "")
                    DaysConfigured = Convert.ToDouble(strOutput);
                else
                    DaysConfigured = 0;
            }
            catch
            {
                DaysConfigured = 0;
            }
            finally
            {
                objBL = null;
            }

            if (DaysConfigured > 0 && intReportInterval > DaysConfigured)
            {
               // grvNFLABList.DataBind();
                grvNFLABList.Visible = false;
                return "Report can be generated only for " + DaysConfigured.ToString() + " days.";
            }
            else
                return "";
        }
    }
    }

