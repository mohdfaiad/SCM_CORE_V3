using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using BAL;
using System.IO;
using System.Data.OleDb;

namespace ProjectSmartCargoManager
{
    public partial class ProRateMaster : System.Web.UI.Page
    {
        ProRateBAL objBAL = new ProRateBAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetOrigin();
                GetDestination();
                btnExport.Visible = false;
            }
        }

        #region Get Pro Rate List
        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
                string Origin, dest;
                Origin = ddlOrigin.SelectedItem.Text.Trim() == "All" ? "" : ddlOrigin.SelectedItem.Text.Trim();
                dest = ddlDestination.SelectedItem.Text.Trim() == "All" ? "" : ddlDestination.SelectedItem.Text.Trim();
                #region Prepare Parameters
                object[] ProRateListInfo = new object[2];
                int i = 0;

                //0
                ProRateListInfo.SetValue(Origin, i);
                i++;

                //1
                ProRateListInfo.SetValue(dest, i);
                i++;
                #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.spGetProRateCodeList(ProRateListInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grdProRateList.PageIndex = 0;
                                grdProRateList.DataSource = ds;
                                grdProRateList.DataMember = ds.Tables[0].TableName;
                                grdProRateList.DataBind();
                                grdProRateList.Visible = true;
                                Session["ds"]=ds;
                                //ds.Clear();
                                btnClear_Click(null, null);
                                btnExport.Visible = true;
                            }
                            else if (ds.Tables[0].Rows.Count <= 0)
                            {
                                btnClear_Click(null,null); 
                                grdProRateList.DataSource = null;
                                grdProRateList.DataBind();
                                btnExport.Visible = false;
                                lblStatus.Text = "Record does not exist";
                                lblStatus.ForeColor = Color.Red;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }

        }
        #endregion Get Pro Rate List

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
                            ddlOrigin.DataValueField = ds.Tables[0].Columns["Code"].ColumnName;

                            ddlOrigin.DataTextField = ds.Tables[0].Columns["Code"].ColumnName;
                            ddlOrigin.DataBind();
                            ddlOrigin.Items.Insert(0, "All");
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
                DataSet ds = objBAL.GetDestinationCodeList(ddlDestination.SelectedValue);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            ddlDestination.DataSource = ds;
                            ddlDestination.DataMember = ds.Tables[0].TableName;
                            ddlDestination.DataValueField = ds.Tables[0].Columns["Code"].ColumnName;

                            ddlDestination.DataTextField = ds.Tables[0].Columns["Code"].ColumnName;
                            ddlDestination.DataBind();
                            ddlDestination.Items.Insert(0, "All");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        # endregion GetDestinationCode List 

        #region clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtValidTo.Text = string.Empty;
            txtValidFrm.Text = string.Empty;
            txtProFact.Text = string.Empty;
            txtConstr.Text = string.Empty;
            chkAct.Checked = false;
            ddlOrigin.SelectedIndex = 0;
            ddlDestination.SelectedIndex = 0;
            btnSave.Text = "Save";
            lblStatus.Text = string.Empty;
            
        }
        #endregion clear

        # region grdProRateList_RowCommand
        protected void grdProRateList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region Edit
            try
            {
                if (e.CommandName == "Edit")
                {
                    btnSave.Text = "Update";
                    lblStatus.Text = "";
                    int RowIndex = Convert.ToInt32(e.CommandArgument);

                    Session["id"] = ((Label)(grdProRateList.Rows[RowIndex].FindControl("lblId"))).Text.ToString();
                    string OrgCode = ((Label)grdProRateList.Rows[RowIndex].FindControl("lblOriginCode")).Text;
                    string DestCode = ((Label)grdProRateList.Rows[RowIndex].FindControl("lblDestinationCode")).Text;
                    Label lblProRate = (Label)grdProRateList.Rows[RowIndex].FindControl("lblProRateFactor");
                    Label lblConstrRate = (Label)grdProRateList.Rows[RowIndex].FindControl("lblConstrFactor");
                   Label lblfrm = (Label)grdProRateList.Rows[RowIndex].FindControl("lblValidFrm");
                    Label lblto = (Label)grdProRateList.Rows[RowIndex].FindControl("lblValidTo");
                    Label lblstatus = (Label)grdProRateList.Rows[RowIndex].FindControl("lblact");
                    
                    ddlOrigin.SelectedIndex = ddlOrigin.Items.IndexOf((ListItem)ddlOrigin.Items.FindByText(OrgCode));
                    ddlDestination.SelectedIndex = ddlDestination.Items.IndexOf((ListItem)ddlDestination.Items.FindByText(DestCode));
                    txtProFact.Text = lblProRate.Text;
                    txtConstr.Text = lblConstrRate.Text;
                    if (lblstatus.Text == "True")
                    {
                        chkAct.Checked = true;
                    }

                    if (lblstatus.Text == "False")
                    {
                        chkAct.Checked = false;
                    }
                    DateTime dtfrm = DateTime.Parse(lblfrm.Text);
                    DateTime dtto = DateTime.Parse(lblto.Text);
                    txtValidFrm.Text = dtfrm.ToShortDateString();
                    txtValidTo.Text = dtto.ToShortDateString();


                }
            }
            catch (Exception ex) { }
           #endregion Edit

            #region Delete
            if (e.CommandName == "DeleteRecord")
            {
                int RowIndex = Convert.ToInt32(e.CommandArgument);

                Label lblid=((Label)(grdProRateList.Rows[RowIndex].FindControl("lblId")));
                int id = int.Parse(lblid.Text);
                string org = ((Label)(grdProRateList.Rows[RowIndex].FindControl("lblOriginCode"))).Text;
                string dest = ((Label)(grdProRateList.Rows[RowIndex].FindControl("lblDestinationCode"))).Text;
                string prorate = ((Label)(grdProRateList.Rows[RowIndex].FindControl("lblProRateFactor"))).Text;

                try
                {
                    #region Prepare Parameters
                    object[] Params = new object[1];
                    int i = 0;

                    //1
                    Params.SetValue(id, i);
                    i++;


                    #endregion Prepare Parameters

                    int ID = 0;
                    ID = objBAL.DeleteProRate(Params);
                    if (ID >= 0)
                    {
                        #region for Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        object[] Paramsss = new object[7];
                        int k = 0;

                        //1
                        Paramsss.SetValue("Airline Proration", k);
                        k++;

                        //2
                        string MstValue = org+"-"+dest;
                        Paramsss.SetValue(MstValue, k);
                        k++;

                        //3
                        Paramsss.SetValue("DELETED", k);
                        k++;

                        //4
                        string Msg = "Org:" + org + "/Dest:" + dest;
                        Paramsss.SetValue(Msg, k);
                        k++;

                        //5
                        string Desc = "ProRate:" + prorate;
                        Paramsss.SetValue(Desc, k);
                        k++;

                        //6

                        Paramsss.SetValue(Session["UserName"], k);
                        k++;

                        //7
                        Paramsss.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                        k++;

                        #endregion Prepare Parameters
                        ObjMAL.AddMasterAuditLog(Paramsss);
                        #endregion

                        btnClear_Click(null, null);
                        btnList_Click(null, null);
                        lblStatus.Text = "Record Deleted Successfully";
                        lblStatus.ForeColor = Color.Green;

                    }
                    else
                    {
                        btnClear_Click(null, null);
                        btnList_Click(null, null);
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Record Deletion Failed..";
                        
                    }
                }
                catch (Exception ex)
                {

                }
            }
           #endregion Delete
        }
        # endregion grdProRateList_RowCommand

        # region grdProRateList_RowEditing
        protected void grdProRateList_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        # endregion grdProRateList_RowEditing

        # region grdProRateList_PageIndexChanging
        protected void grdProRateList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                /*string Origin, dest;
                Origin = ddlOrigin.SelectedItem.Text.Trim() == "All" ? "" : ddlOrigin.SelectedItem.Text.Trim();
                dest = ddlDestination.SelectedItem.Text.Trim() == "All" ? "" : ddlDestination.SelectedItem.Text.Trim();
                #region Prepare Parameters
                object[] ProRateListInfo = new object[2];
                int i = 0;

                //0
                ProRateListInfo.SetValue(Origin, i);
                i++;

                //1
                ProRateListInfo.SetValue(dest, i);
                i++;
             #endregion Prepare Parameters

                DataSet ds = new DataSet();
                ds = objBAL.spGetProRateCodeList(ProRateListInfo);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                grdProRateList.PageIndex = 0;
                                grdProRateList.DataSource = ds;
                                grdProRateList.DataMember = ds.Tables[0].TableName;
                                grdProRateList.DataBind();
                                grdProRateList.Visible = true;
                                //ds.Clear();

                            }
                        }
                    }
                }*/

                DataSet dstemp = (DataSet)Session["ds"];
                grdProRateList.PageIndex = e.NewPageIndex;
                grdProRateList.DataSource = dstemp;// ds.Copy();
                grdProRateList.DataMember = dstemp.Tables[0].TableName;
                grdProRateList.DataBind();


                //for (int j = 0; j < grdProRateList.Rows.Count; j++)
                //{
                //    if (((Label)(grdProRateList.Rows[j].FindControl("lblAct"))).Text.ToString() == "True")
                //    {
                //        ((Label)(grdProRateList.Rows[j].FindControl("lblAct"))).Text = "Active";
                //    }

                //    else if (((Label)(grdProRateList.Rows[j].FindControl("lblAct"))).Text.ToString() == "False")
                //    {
                //        ((Label)(grdProRateList.Rows[j].FindControl("lblAct"))).Text = "InActive";
                //    }
                //}
            }
            catch (Exception ex)
            {
            }
        }

        # endregion grdProRateList_PageIndexChanging

        #region Add Pro Rate
        protected void btnSave_Click(object sender, EventArgs e)
        {
                try
                {
                    #region Save
                    if (btnSave.Text == "Save")
                    {
                        #region Prepare Parameters
                        object[] Params = new object[7];
                        int i = 0;
                        if (txtValidFrm.Text != "" && txtValidTo.Text != "")
                        {
                            if (ddlDestination.SelectedIndex != 0 && ddlOrigin.SelectedIndex != 0)
                            {
                                //1

                                Params.SetValue(ddlOrigin.SelectedItem.Text, i);
                                i++;

                                //2
                                Params.SetValue(ddlDestination.SelectedItem.Text, i);
                                i++;

                                //3
                                int prorate = int.Parse(txtProFact.Text);
                                Params.SetValue(prorate, i);
                                i++;

                                //4
                                int constrrate;
                                if (txtConstr.Text == "")
                                {
                                    Params.SetValue(null, i);
                                    i++;
                                }
                                else
                                {
                                    constrrate = int.Parse(txtConstr.Text);
                                    Params.SetValue(constrrate, i);
                                    i++;
                                }

                                //5
                                DateTime frm = DateTime.Parse(txtValidFrm.Text);
                                Params.SetValue(frm, i);
                                i++;

                                //6
                                DateTime to = DateTime.Parse(txtValidTo.Text);
                                Params.SetValue(to, i);
                                i++;

                                //7
                                if (chkAct.Checked == true)
                                {
                                    Params.SetValue(true, i);
                                    i++;
                                }
                                else if (chkAct.Checked == false)
                                {
                                    Params.SetValue(false, i);
                                    i++;
                                }

                        #endregion Prepare Parameters

                                int ID = 0;
                                ID = objBAL.AddProRate(Params);
                                if (ID >= 0)
                                {

                                    #region for Master Audit Log
                                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                                    #region Prepare Parameters
                                    object[] Paramsss = new object[7];
                                    int k = 0;

                                    //1
                                    Paramsss.SetValue("Airline Proration", k);
                                    k++;

                                    //2
                                    string MstValue = ddlOrigin.SelectedItem.Text+"-"+ddlDestination.SelectedItem.Text;
                                    Paramsss.SetValue(MstValue, k);
                                    k++;

                                    //3
                                    Paramsss.SetValue("ADD", k);
                                    k++;

                                    //4
                                    string Msg = "Org:" + ddlOrigin.SelectedItem.Text + "/Dest:" + ddlDestination.SelectedItem.Text;
                                    Paramsss.SetValue(Msg, k);
                                    k++;

                                    //5
                                    string Desc = "ProRate:" + prorate.ToString();
                                    Paramsss.SetValue(Desc, k);
                                    k++;

                                    //6

                                    Paramsss.SetValue(Session["UserName"], k);
                                    k++;

                                    //7
                                    Paramsss.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                                    k++;

                                    #endregion Prepare Parameters
                                    ObjMAL.AddMasterAuditLog(Paramsss);
                                    #endregion

                                    btnClear_Click(null, null);
                                    btnList_Click(null, null);
                                    lblStatus.Text = "Record Added Successfully";
                                    lblStatus.ForeColor = Color.Green;
                                    btnSave.Text = "Save";


                                }
                                else
                                {
                                    lblStatus.ForeColor = Color.Red;
                                    lblStatus.Text = "Record Addition Failed..";
                                    btnList_Click(null, null);
                                }
                            }
                            else
                            {
                                lblStatus.Text = "Select Origin and Destination";
                                lblStatus.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            lblStatus.Text = "Select Dates";
                            lblStatus.ForeColor = Color.Red;
                        }
                    }
                 #endregion Save

                    #region Update
                    if (btnSave.Text == "Update")
                    {
                        try
                        {
                            #region Prepare Parameters
                            object[] Params = new object[8];
                            int i = 0;
                            int id = int.Parse(Session["id"].ToString());

                            if (txtValidFrm.Text != "" && txtValidTo.Text != "" && txtProFact.Text != "")
                            {
                            if (ddlDestination.SelectedIndex != 0 && ddlOrigin.SelectedIndex != 0)
                            {
                            //0
                            Params.SetValue(id, i);
                            i++;

                            //1
                            Params.SetValue(ddlOrigin.SelectedItem.Text, i);
                            i++;

                            //2
                            Params.SetValue(ddlDestination.SelectedItem.Text, i);
                            i++;

                            //3
                            int prorate = int.Parse(txtProFact.Text);
                            Params.SetValue(prorate, i);
                            i++;

                            //4
                            int constrrate;
                            if (txtConstr.Text == "")
                            {
                                Params.SetValue(null, i);
                                i++;
                            }
                            else
                            {
                                constrrate = int.Parse(txtConstr.Text);
                                Params.SetValue(constrrate, i);
                                i++;
                            }

                            //5
                            DateTime frm = DateTime.Parse(txtValidFrm.Text);
                            Params.SetValue(frm, i);
                            i++;

                            //6
                            DateTime to = DateTime.Parse(txtValidTo.Text);
                            Params.SetValue(to, i);
                            i++;

                            //7
                            if (chkAct.Checked == true)
                            {
                                Params.SetValue(true, i);
                                i++;
                            }
                            else if (chkAct.Checked == false)
                            {
                                Params.SetValue(false, i);
                                i++;
                            }

                            #endregion Prepare Parameters

                            int ID = 0;
                            ID = objBAL.UpdateProRate(Params);
                            if (ID >= 0)
                            {
                                #region for Master Audit Log
                                MasterAuditBAL ObjMAL = new MasterAuditBAL();
                                #region Prepare Parameters
                                object[] Paramsss = new object[7];
                                int k = 0;

                                //1
                                Paramsss.SetValue("Airline Proration", k);
                                k++;

                                //2
                                string MstValue = ddlOrigin.SelectedItem.Text + "-" + ddlDestination.SelectedItem.Text;
                                Paramsss.SetValue(MstValue, k);
                                k++;

                                //3
                                Paramsss.SetValue("UPDATE", k);
                                k++;

                                //4
                                string Msg = "Org:" + ddlOrigin.SelectedItem.Text + "/Dest:" + ddlDestination.SelectedItem.Text;
                                Paramsss.SetValue(Msg, k);
                                k++;

                                //5
                                string Desc = "ProRate:" + prorate.ToString();
                                Paramsss.SetValue(Desc, k);
                                k++;

                                //6

                                Paramsss.SetValue(Session["UserName"], k);
                                k++;

                                //7
                                Paramsss.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString("dd/MM/yyyy"), k);
                                k++;

                                #endregion Prepare Parameters
                                ObjMAL.AddMasterAuditLog(Paramsss);
                                #endregion

                                btnClear_Click(null, null);
                                btnList_Click(null, null);
                                lblStatus.Text = "Record Updated Successfully";
                                lblStatus.ForeColor = Color.Green;

                            }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Record Updation Failed..";
                                btnList_Click(null, null);
                            }
                        }
                            else
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Select Origin and Destination.";
                                //btnList_Click(null, null);
                            }
                        }
                            else
                            {
                                lblStatus.Text = "Select Dates and ProRate Factor";
                                lblStatus.ForeColor = Color.Red;
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    #endregion Update
                }
                catch (Exception ex)
                {

                }
            
           
        }
        #endregion Add Pro Rate

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["ds"];
            DataTable dt = new DataTable();
           
            try
            {
                if (ds == null)
                {
                    lblStatus.Text = "Select Records to export";
                    return;
                }   

                dt = ds.Tables[0];
                dt.Columns.Remove("ID");
                dt.Columns.Remove("OrgName");
                dt.Columns.Remove("OrgCountry");
                dt.Columns.Remove("DestName");
                dt.Columns.Remove("DestCountry");
                
                string attachment = "attachment; filename=ProRate.xls";
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
            { lblStatus.Text = ex.Message; }
            finally
            {
                ds = null;
                dt = null;
            }
        }

        /*protected void btnImoprt_Click(object sender, EventArgs e)
        {
            string file = @"C:\Users\TOSHIBA\Downloads\Screnning Report.xls";
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            Microsoft.Office.Interop.Excel.Range range;

            xlApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
            xlWorkBook = xlApp.Workbooks.Open("" + file, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;
            string errormessage = "";

            for (int i = 2; i <= range.Rows.Count; i++)
            {
                string source = (string)(range.Cells[i, 1] as Microsoft.Office.Interop.Excel.Range).Value2;

                //if (source == null)
                //{
                //    break;
                //}
                //if (source == "")
                //{
                //    break;
                //}

                string dest = (string)(range.Cells[i, 3] as Microsoft.Office.Interop.Excel.Range).Value2;
                string flight = (string)(range.Cells[i, 5] as Microsoft.Office.Interop.Excel.Range).Value2;
                string commcode = (string)(range.Cells[i, 8] as Microsoft.Office.Interop.Excel.Range).Value2;
                decimal min = decimal.Parse("" + (range.Cells[i, 9] as Microsoft.Office.Interop.Excel.Range).Value2);
                decimal normal = decimal.Parse("" + (range.Cells[i, 10] as Microsoft.Office.Interop.Excel.Range).Value2);

                decimal fourtyfive = 0;
                decimal hundred = 0;

                if ((range.Cells[i, 11] as Microsoft.Office.Interop.Excel.Range).Value2 != null)
                    fourtyfive = decimal.Parse("" + (range.Cells[i, 11] as Microsoft.Office.Interop.Excel.Range).Value2);

                if ((range.Cells[i, 12] as Microsoft.Office.Interop.Excel.Range).Value2 != null)
                    hundred = decimal.Parse("" + (range.Cells[i, 12] as Microsoft.Office.Interop.Excel.Range).Value2);

                if (flight == null)
                    flight = "";

                //flight = "SG" + flight.Substring(3);
                //flight = "";
                //commcode = "";

                Log.WriteLog("Line No(" + i + ") :" + source + "," + dest + "," + flight + "," + min + "," + normal + "," + fourtyfive + "," + hundred);

                //if (!InsertRateLine(source, dest, flight, commcode, min, normal, fourtyfive, hundred, ref errormessage))
                //{
                //    Log.WriteLog("Error : Line No(" + i + ")" + errormessage);
                //}

                Log.WriteLog("Inserted Successfully. Line No(" + i + ")");

            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            //releaseObject(xlWorkSheet);
            //releaseObject(xlWorkBook);
            //releaseObject(xlApp);

        }
        */
    }
}
