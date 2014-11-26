#region IMPORT CLASSES

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Configuration;
using System.Net.Mail;
using System.Data.Common;
using System.Net;
using System.Web.Security;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
//using clsDataLib;
using QID.DataAccess;
using ProjectSmartCargoManager;

#endregion IMPORT CLASSES

namespace MyKFCargoNewProj
{
    public partial class FrmTracerHistoryPopup : System.Web.UI.Page
    {


        #region PAGE LOAD EVENT

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["AWBNo"].ToString() != null || Request.QueryString["AWBNo"].ToString() != string.Empty)
                {
                    string strAWBNo = Request.QueryString["AWBNo"].ToString();
                    string strTracerNo = Request.QueryString["TracerNo"].ToString();
                    hdnAwbNo.Value = strAWBNo;
                    hdnTracerNo.Value = strTracerNo;
                    LoadGridview(strAWBNo, strTracerNo);
                }
                else
                {
                    txtErrorMsg.Visible = true;
                }
            }
        }

        #endregion PAGE LOAD EVENT


        #region BIND DATASET WITH GRIDVIEW FUNCTION

        /// <summary>
        /// BINDING GRIDVIEW WITH RETURNED DATASET 
        /// </summary>

        private void LoadGridview(string AWBNo, string TracerNo)
        {
            DataSet oDs = new DataSet();
            SQLServer ObjData = new SQLServer(Global.GetConnectionString());

            try
            {
                string[] QueryPname = new string[2];
                object[] QueryValue = new object[2];
                SqlDbType[] QueryType = new SqlDbType[2];
                int i = 0;
                QueryPname.SetValue("AWBNo", i);
                QueryValue.SetValue(AWBNo, i);
                QueryType.SetValue(SqlDbType.VarChar, i);
                i++;

                QueryPname.SetValue("TracerNo", i);
                QueryValue.SetValue(TracerNo, i);
                QueryType.SetValue(SqlDbType.BigInt, i);
                i++;

                oDs = ObjData.SelectRecords("SpGetTracerHistoryLive", QueryPname, QueryValue, QueryType);

                if (oDs != null)
                {
                    if (oDs.Tables[0].Rows.Count >= 1)
                    {
                        grdHistory.DataSource = oDs.Tables[0];
                        grdHistory.DataBind();
                    }
                    else
                    {
                        txtErrorMsg.Text = "No Tracer History Found";

                    }
                }
                else
                {
                    txtErrorMsg.Text = "No Tracer History Found";
                }

            }
            catch (Exception ex)
            {

                txtErrorMsg.Text = "Error Populating Tracer History" + ex.Message;
            }
        }

        #endregion BIND DATASET WITH GRIDVIEW FUNCTION


        #region ON GRIDVIEW DOWNLOAD IMAGE BUTTON CLICK EVENT

        /// <summary>
        /// TO DOWLOAD ATTACH FILE BY CLICKING ON DOWNLOAD IMAGE BUTTON CLICK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                SQLServer ObjData = new SQLServer(Global.GetConnectionString());

                GridViewRow Row = grdHistory.Rows[index];
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

                string Name = ((Label)grdHistory.Rows[index].FindControl("lblName")).Text;
                string MimeType = ((Label)grdHistory.Rows[index].FindControl("lblMimeType")).Text;

                DataSet oDs = new DataSet();
                string[] QueryPname = new string[4];
                object[] QueryValue = new object[4];
                SqlDbType[] QueryType = new SqlDbType[4];
                int i = 0;
                QueryPname.SetValue("AWBNo", i);
                QueryValue.SetValue(hdnAwbNo.Value, i);
                QueryType.SetValue(SqlDbType.VarChar, i);
                i++;

                QueryPname.SetValue("TracerNo", i);
                QueryValue.SetValue(hdnTracerNo.Value, i);
                QueryType.SetValue(SqlDbType.BigInt, i);
                i++;

                QueryPname.SetValue("Name", i);
                QueryValue.SetValue(Name, i);
                QueryType.SetValue(SqlDbType.VarChar, i);
                i++;

                QueryPname.SetValue("MimeType", i);
                QueryValue.SetValue(MimeType, i);
                QueryType.SetValue(SqlDbType.VarChar, i);
                i++;
                oDs = ObjData.SelectRecords("SpGetTracerHistoryUploadedfile", QueryPname, QueryValue, QueryType);

                using (Stream st = new MemoryStream((byte[])oDs.Tables[0].Rows[0][0]))
                {
                    long dataLengthToRead = st.Length;
                    Response.ContentType = MimeType;
                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Name + "\"");
                    Response.BinaryWrite((byte[])oDs.Tables[0].Rows[0][0]);
                    Response.Flush();

                }
                Response.End();

            }
        }

        #endregion ON GRIDVIEW DOWNLOAD IMAGE BUTTON CLICK EVENT


        #region GRIDVIEW ON ROW CREATED EVENT

        /// <summary>
        /// ON GRIDVIEW ROW CREATION HIDE FEW ROWS WHICH REQUIRED FOR BACK PROCESS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdHistory_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].CssClass = "hiddencol1";
                e.Row.Cells[5].CssClass = "hiddencol1";

            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[4].CssClass = "hiddencol1";
                e.Row.Cells[5].CssClass = "hiddencol1";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[4].CssClass = "hiddencol1";
                e.Row.Cells[5].CssClass = "hiddencol1";
            }
        }

        #endregion GRIDVIEW ON ROW CREATED EVENT


        #region GRIDVIEW ON ROW DATA BOUND EVENT

        /// <summary>
        /// ON GRIDVIEW ROW DATABOUND FOR REPLICATING DOWNLOAD IMAGE BUTTON TO EMPTY ROW IF NO ATTACHEMENT FOUND 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    tc.Attributes["style"] = "border-color:#990000";
                }

                Label lblName = (e.Row.FindControl("lblName") as Label);

                if (lblName.Text.ToUpper() == "N/A")
                {
                    LinkButton btnDownload = (e.Row.FindControl("btnDownload") as LinkButton);
                    btnDownload.Text = string.Empty;
                    btnDownload.Enabled = false;
                }
            }
        }

        #endregion GRIDVIEW ON ROW DATA BOUND EVENT

        protected void grdHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdHistory.PageIndex = e.NewPageIndex;
                LoadGridview(hdnAwbNo.Value,hdnTracerNo.Value);
            }
            catch (Exception)
            {
                LoadGridview(hdnAwbNo.Value, hdnTracerNo.Value);
            }
        }


    }
}
