using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using BAL;
using QID.DataAccess;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class frmAWBTrackingMaster : System.Web.UI.Page
    {

        #region Variables
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BALAWBtracking BALobj = new BALAWBtracking();
        BLArrival objArr = new BLArrival();
        BALAWBTracLDetails BATD = new BALAWBTracLDetails();
        string CurrAWBno = "";
        static string FinalAwbList = "";
        #endregion Variables

        #region Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            LabelStatus.Visible = true;
            if (!IsPostBack)
            {
                string trackAWB = null;
                try
                {
                    HideAllImages();

                    //Fetch AcceptsPartnerAWB flag to enable/ disable AWB Prefix text box.
                    LoginBL objBal = new LoginBL();
                    string prefixEnable = "true";
                    prefixEnable = objBal.GetMasterConfiguration("AcceptPartnerAWB");
                    if (prefixEnable == null || prefixEnable == "")
                    {
                        prefixEnable = "true";
                    }
                    txtPrefix.Enabled = Convert.ToBoolean(prefixEnable);

                    string AWBPrefix = Session["awbPrefix"].ToString();
                    string AirlinePrefix = Session["AirlinePrefix"].ToString();
                    txtPrefix.ToolTip = "Enter AWB Prefix ex: " + AWBPrefix;
                    //Check if querystring is present then fetch AWB Status from query string.
                    if (Request.QueryString != null && Request.QueryString["AWBNo"] != null
                        && Request.QueryString["AWBNo"] != "" && Request.QueryString["AWBPrefix"] != null
                        && Request.QueryString["AWBPrefix"] != "")
                    {   //Extract AWB Numbers from
                        trackAWB = Request.QueryString["AWBPrefix"] + "|" + Request.QueryString["AWBNo"];
                    }
                    else
                    {
                        if (Session["TrackAWB"] != null)
                        {
                            trackAWB = Session["TrackAWB"].ToString();
                        }
                    }
                    if (trackAWB != null && trackAWB != "")
                    {
                        string[] spltPrefix = trackAWB.Split('|');
                        txtPrefix.Text = spltPrefix[0].Trim();
                        string[] AWBNos = spltPrefix[1].Trim().Split(',');
                        string prefix = spltPrefix[0].ToString();
                        FinalAwbList = "";
                        for (int i = 0; i < AWBNos.Length; i++)
                        {
                            FinalAwbList += AWBNos[i].ToString() + ",";
                        }
                        TextBoxAWBno.Text = FinalAwbList;
                        FinalAwbList = FinalAwbList.Remove(FinalAwbList.Length - 1, 1);
                        GridBindMethod(FinalAwbList);
                        GridViewAwbTracking.Visible = true;
                        spltPrefix = null;
                        AWBNos = null;
                    }
                    else
                    {
                        txtPrefix.Text = AWBPrefix;
                        TextBoxAWBno.Text = "";
                        pnlShowData.Visible = false;
                    }

                    //If FSA Trackig grid is configured to be shown then only fetch data.
                    ViewState["ShowFSA"] = false;
                    LoginBL objMast = new LoginBL();
                    string showFSA = objMast.GetMasterConfiguration("ShowFSAInAWBTracking");
                    if (showFSA != null && showFSA != "")
                    {
                        ViewState["ShowFSA"] = Convert.ToBoolean(showFSA);
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion Page load

        #region Grid Bind Method
        public void GridBindMethod(string FinalAWBs)
        {
            DataSet DSawbmilestonedata = null;
            try
            {
                pnlShowData.Visible = true;
                CurrAWBno = "";
                DSawbmilestonedata = BALobj.GetAWBTrackingData(txtPrefix.Text.Trim(), FinalAWBs);
                if (DSawbmilestonedata != null && DSawbmilestonedata.Tables.Count > 0 && DSawbmilestonedata.Tables[0].Rows.Count > 0)
                {
                    CurrAWBno = DSawbmilestonedata.Tables[0].Rows[0][0].ToString();
                    DataTable DTnew = null;
                    DTnew = DSawbmilestonedata.Tables[0].Clone();
                    foreach (DataRow dr in DSawbmilestonedata.Tables[0].Rows)
                    {
                        if (CurrAWBno != dr[0].ToString())
                        {
                            DataRow dr1 = DTnew.NewRow();
                            DTnew.Rows.Add(dr1);
                            CurrAWBno = dr[0].ToString();
                        }
                        DTnew.ImportRow(dr);
                    }
                    GridViewAwbTracking.Visible = true;
                    GridViewAwbTracking.DataSource = DTnew;
                    GridViewAwbTracking.DataBind();
                    if (DTnew != null)
                        DTnew.Dispose();
                    try
                    {
                        HideAllImages();
                        lblSelectRows.Visible = true;
                        if (FinalAWBs.Length == 8)
                        {
                            lblSelectRows.Visible = false;
                            for (int i = 0; i < GridViewAwbTracking.Rows.Count; i++)
                            {
                                string AWBnumberCHK = GridViewAwbTracking.DataKeys[i].Value.ToString();
                                lblAWBNo.Text = txtPrefix.Text + "-" + FinalAWBs;
                                GridViewAwbTracking.Rows[i].Cells[0].ForeColor = Color.Brown;
                                if ((txtPrefix.Text.ToString() + "-" + FinalAWBs) == AWBnumberCHK)
                                {
                                    if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Booked")
                                    {
                                        ImageBooking.Visible = true;
                                        ImageBooking.ImageUrl = "~/Images/booking_gr.png";
                                        continue;
                                    }
                                    if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Accepted")
                                    {
                                        ImageAcceptance.Visible = true;
                                        ImageAcceptance.ImageUrl = "~/Images/accepted_gr.png";
                                        continue;
                                    }
                                    if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Manifested")
                                    {
                                        ImageManifested.Visible = true;
                                        ImageManifested.ImageUrl = "~/Images/manifested_gr.png";
                                        continue;
                                    }
                                    if (GridViewAwbTracking.Rows[i].Cells[1].Text.Trim().Contains("Departed"))
                                    {
                                        ImageDeparted.Visible = true;
                                        ImageDeparted.ImageUrl = "~/Images/departed_gr.png";
                                        continue;
                                    }
                                    if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Arrival")
                                    {
                                        ImageArrival.Visible = true;
                                        ImageArrival.ImageUrl = "~/Images/arrival_gr.png";
                                        continue;
                                    }
                                    if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Partial Delivery")
                                    {
                                        ImagePartialDelivery.Visible = true;
                                        ImagePartialDelivery.ImageUrl = "~/Images/pardelivery_gr.png";
                                        continue;
                                    }
                                    if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Delivery")
                                    {
                                        ImageDelivered.Visible = true;
                                        ImageDelivered.ImageUrl = "~/Images/delivered_gr.png";
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    // message to display in label
                    LabelStatus.Visible = true;
                    LabelStatus.Focus();
                    LabelStatus.Text = "No Records Found";
                    LabelStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (DSawbmilestonedata != null)
                {
                    DSawbmilestonedata.Dispose();
                }
            }
        }
        #endregion Grid Bind Method

        #region Button GO Click
        protected void ButtonGO_Click(object sender, EventArgs e)
        {
            try
            {
                pnlShowData.Visible = false;
                LabelStatus.Text = "";
                lblAWBNo.Text = "";
                if (txtPrefix.Text == "")
                {
                    LabelStatus.Text = "Kindly Enter prefix for Track AWBs";
                    LabelStatus.ForeColor = Color.Red;
                    return;
                }
                if (TextBoxAWBno.Text.Trim() == null || TextBoxAWBno.Text.Trim() == "")
                {
                    LabelStatus.Text = "Kindly enter AWB's for tracking ";
                    LabelStatus.ForeColor = Color.Red;
                    return;
                }

                string[] Arr = TextBoxAWBno.Text.Trim().Replace(" ", "").Replace("\r", "").Replace("\n", "").Split(',');
                for (int i = 0; i < Arr.Length; i++)
                {
                    if (Arr[i].Length < 8 || Arr[i].Length > 8)
                    {
                        LabelStatus.Text = Arr[i] + " Is Not Real AWB Number";
                        LabelStatus.ForeColor = Color.Red;
                        return;
                    }
                }
                Arr = null;
                string awbno = TextBoxAWBno.Text;
                if (TextBoxAWBno.Text != "")
                {
                    FinalAwbList = TextBoxAWBno.Text.Trim().Replace(" ", "").Replace("\r", "").Replace("\n", "");
                }

                DataSet ds = BALobj.GetAWBTrackingData(txtPrefix.Text.Trim(), TextBoxAWBno.Text.Trim().Replace(" ", "").Replace("\r", "").Replace("\n", ""));
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            pnlShowData.Visible = true;
                            GridBindMethod(FinalAwbList);
                            ds.Dispose();
                        }
                        else
                        {
                            LabelStatus.Text = "AWB Details not available";
                            LabelStatus.ForeColor = Color.Red;
                            ds.Dispose();
                            return;
                        }
                    }
                }

                #region
                if ((bool)ViewState["ShowFSA"] == true)
                {
                    string message = "";
                    ds = BATD.dsgetAWBRecord(txtPrefix.Text.Trim(), TextBoxAWBno.Text.Trim(), ref message);

                    if (ds != null)
                    {
                        GrdFSATracking.DataSource = ds;
                        GrdFSATracking.DataBind();
                        ds.Dispose();
                    }
                }
                #endregion
                #region Populate DoNumbers if present Only for single AWB Not for multiple AWB
                lblIsCollect.Text = "";
                if (!TextBoxAWBno.Text.Trim().Contains(","))
                {
                    DataSet dsDo1 = BALobj.IsCollect(txtPrefix.Text.Trim(), TextBoxAWBno.Text.Trim().Replace(" ", "").Replace("\r", "").Replace("\n", ""));
                    if (dsDo1 != null && dsDo1.Tables.Count > 0 && dsDo1.Tables[0].Rows.Count > 0)
                    {
                        string strIsCollect = dsDo1.Tables[0].Rows[0][0].ToString();
                        if (strIsCollect != "")
                        {
                            Repeater1.DataSource = null;
                            Repeater1.DataBind();
                            lblIsCollect.Text = "Amount is due for collection. Please pay the due amount in order to Print DO";
                            lblIsCollect.ForeColor = Color.Red;
                            return;
                        }
                    }
                    try
                    {
                        Repeater1.DataSource = null;
                        Repeater1.DataBind();
                        DataSet dsDo = BALobj.GetDoNumbersAWBTracking(txtPrefix.Text.Trim(), TextBoxAWBno.Text.Trim().Replace(" ", "").Replace("\r", "").Replace("\n", ""));
                        if (dsDo != null)
                        {
                            Repeater1.DataSource = dsDo;
                            Repeater1.DataBind();
                        }

                    }
                    catch (Exception ex)
                    { }
                }
                #endregion
            }
            catch (Exception)
            {
            }
        }
        #endregion Button GO Click

        #region Render
        protected override void Render(HtmlTextWriter writer)
        {
            if (TextBoxAWBno.Text.Length > 16)
            {
                foreach (GridViewRow gvr in GridViewAwbTracking.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        gvr.Cells[0].Attributes["onmouseover"] = "this.style.cursor='pointer';";
                        gvr.Cells[0].Attributes["onmouseout"] = "this.style.textDecoration='none';";
                        ////underline and show hand icon on hover
                        //gvr.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                        //gvr.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                        //true parameter is is register for event validation
                        gvr.Cells[0].Attributes["onclick"] = ClientScript.GetPostBackEventReference(GridViewAwbTracking, "Select$" + gvr.DataItemIndex, true);
                    }
                }
            }
            base.Render(writer);
        }
        #endregion Render

        #region Page Change
        protected void GridViewAwbTracking_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                lblAWBNo.Text = "";
                GridViewAwbTracking.PageIndex = e.NewPageIndex;
                GridBindMethod(FinalAwbList);
            }
            catch (Exception)
            {
            }
        }
        #endregion Page Change

        #region Grid Row Data Bound To add image
        //protected void GridViewAwbTracking_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {

        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            #region Code to add/Change image  control dynamically
        //            // set reference to current data record 
        //            // use IDataRecord if you bind to a DataReader, and DataRowView if you bind to a DataSet
        //            //DataRowView dataRow = (DataRowView)e.Row.DataItem;
        //            //string myflag = dataRow["Milestone"].ToString();
        //            //if (myflag == "Booking")
        //            //{
        //            //    e.Row .Cells [0].Controls .Clear() ;
        //            //    ImageButton   img = new ImageButton();
        //            //    //img.BackColor = Color.Transparent;
        //            //    img.ImageUrl = "~/Images/booking.png";
        //            //    e.Row.Cells [0].Controls .Add (img );
        //            //    img.Enabled = false;
        //            //    //img.Style.Add("background-color","Transparent");
        //            //    //img.Style.Add("BackColor", "Transparent");
        //            //    img.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Transparent");
        //            //}
        //            //if (myflag == "Accepted")
        //            //{
        //            //    e.Row.Cells[0].Controls.Clear();
        //            //    ImageButton img = new ImageButton();
        //            //    img.ImageUrl = "~/Images/delivery1.png";
        //            //    e.Row.Cells[0].Controls.Add(img);
        //            //    img.Enabled = false;
        //            //    img.Style.Add("background-color", "Transparent");
        //            //    img.Style.Add("BackColor", "Transparent");
        //            //    img.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Transparent");
        //            //}                 

        //            //if (myflag == "Arrival")
        //            //{
        //            //    e.Row.Cells[0].Controls.Clear();
        //            //    ImageButton img = new ImageButton();
        //            //    img.ImageUrl = "~/Images/plus.gif";
        //            //    e.Row.Cells[0].Controls.Add(img);
        //            //    img.Enabled = false;
        //            //}
        //            //if (myflag == "Partial Delivery")
        //            //{
        //            //    e.Row.Cells[0].Controls.Clear();
        //            //    ImageButton img = new ImageButton();
        //            //    img.ImageUrl = "~/Images/plus.gif";
        //            //    e.Row.Cells[0].Controls.Add(img);
        //            //    img.Enabled = false;
        //            //}
        //            //if (myflag == "Delivery")
        //            //{
        //            //    e.Row.Cells[0].Controls.Clear();
        //            //    ImageButton img = new ImageButton();
        //            //    img.ImageUrl = "~/Images/delivery1.png";
        //            //    e.Row.Cells[0].Controls.Add(img);
        //            //    img.Enabled = false;
        //            //}
        //            //if (myflag == "Manifested")
        //            //{
        //            //    e.Row.Cells[0].Controls.Clear();
        //            //    ImageButton img = new ImageButton();                       
        //            //    img.ImageUrl = "~/Images/Manifested.png";
        //            //    e.Row.Cells[0].Controls.Add(img);
        //            //    img.Enabled = false;
        //            //}
        //            #endregion Code to add/Change image  control dynamically
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        #endregion Grid Row Data Bound To add image

        #region Grid Row Index Changing
        protected void GridViewAwbTracking_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            ImageBooking.ImageUrl = "~/Images/booking_red.png";
            ImageAcceptance.ImageUrl = "~/Images/accepted_red.png";
            ImageManifested.ImageUrl = "~/Images/manifested_red.png";
            ImageDeparted.ImageUrl = "~/Images/departed_red.png";
            ImageArrival.ImageUrl = "~/Images/arrival_red.png";
            ImagePartialDelivery.ImageUrl = "~/Images/pardelivery_red.png";
            ImageDelivered.ImageUrl = "~/Images/delivered_red.png";
        }
        #endregion Grid Row Index Changing

        #region Grid Row Index Changed
        protected void GridViewAwbTracking_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HideAllImages();
                string AWBnumberSelected = "";
                if (GridViewAwbTracking.SelectedIndex > -1)
                {

                    if (GridViewAwbTracking.SelectedIndex > (GridViewAwbTracking.PageSize - 1))//to solve the issue of index out ofbounds for page no greater than index 0.
                    {
                        int selectedgridrowindex = ((GridViewAwbTracking.SelectedIndex) % (GridViewAwbTracking.PageSize));
                        AWBnumberSelected = GridViewAwbTracking.DataKeys[selectedgridrowindex].Value.ToString();
                    }
                    else
                    {
                        AWBnumberSelected = GridViewAwbTracking.DataKeys[GridViewAwbTracking.SelectedIndex].Value.ToString();
                    }
                    lblAWBNo.Text = AWBnumberSelected;

                    for (int i = 0; i < GridViewAwbTracking.Rows.Count; i++)
                    {
                        string AWBnumberCHK = GridViewAwbTracking.DataKeys[i].Value.ToString();
                        GridViewAwbTracking.Rows[i].Cells[0].ForeColor = Color.Blue;
                        if (AWBnumberSelected == AWBnumberCHK)
                        {
                            GridViewAwbTracking.Rows[i].Cells[0].ForeColor = Color.Brown;
                            if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Booked")
                            {
                                ImageBooking.ImageUrl = "~/Images/booking_gr.png";
                                ImageBooking.Visible = true;
                                continue;
                            }
                            if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Accepted")
                            {
                                ImageAcceptance.ImageUrl = "~/Images/accepted_gr.png";
                                ImageAcceptance.Visible = true;
                                continue;
                            }
                            if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Manifested")
                            {
                                ImageManifested.ImageUrl = "~/Images/manifested_gr.png";
                                ImageManifested.Visible = true;
                                continue;
                            }
                            if (GridViewAwbTracking.Rows[i].Cells[1].Text.Trim().Contains("Departed"))
                            {
                                ImageDeparted.ImageUrl = "~/Images/departed_gr.png";
                                ImageDeparted.Visible = true;
                                continue;
                            }
                            if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Arrival")
                            {
                                ImageArrival.ImageUrl = "~/Images/arrival_gr.png";
                                ImageArrival.Visible = true;
                                continue;
                            }
                            if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Partial Delivery")
                            {
                                ImagePartialDelivery.Visible = true;
                                ImagePartialDelivery.ImageUrl = "~/Images/pardelivery_gr.png";
                                continue;
                            }
                            if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Delivery")
                            {
                                ImagePartialDelivery.Visible = false;
                                ImageDelivered.ImageUrl = "~/Images/delivered_gr.png";
                                ImageDelivered.Visible = true;
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion Grid Row Index Changed

        #region Hide All Images
        private void HideAllImages()
        {
            ImageBooking.Visible = false;
            ImageAcceptance.Visible = false;
            ImageManifested.Visible = false;
            ImageDeparted.Visible = false;
            ImageArrival.Visible = false;
            ImagePartialDelivery.Visible = false;
            ImageDelivered.Visible = false;
        }
        #endregion Hide All Images

        #region Text box AWB no Text Change
        protected void TextBoxAWBno_TextChanged(object sender, EventArgs e)
        {
            if (LabelStatus.Visible == true)
            {
                LabelStatus.Text = "";
            }
        }
        #endregion Text box AWB no Text Change

        //Swapnil 31-10-2012
        #region PrintDo
        protected void BtnPrinDo_Click(object sender, EventArgs e)
        {
            #region commented by Vijay
            //try
            //{
            //    for (int i = 0; i < GridViewAwbTracking.Rows.Count; i++)
            //    {
            //        if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Partial Delivery")
            //        {
            //            #region ReprintDo 
            //                if (TextBoxAWBno.Text == "")
            //                {
            //                    LabelStatus.Text = "Please enter awb number to Print Do";
            //                    LabelStatus.ForeColor = Color.Red;
            //                    return;
            //                }
            //                else
            //                {
            //                    string AWBNumber = TextBoxAWBno.Text.Trim();
            //                    string Station = Session["Station"].ToString();

            //                    DataSet ds = objArr.GetdataTracking(AWBNumber, Station);
            //                    if (ds != null)
            //                    {
            //                        if (ds.Tables[0].Rows.Count > 0)
            //                        {
            //                            Session["Tracking"] = ds;
            //                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            //                            {
            //                                DataTable dtSurface = new DataTable();
            //                                dtSurface.Columns.Add("AgentName", typeof(string));
            //                                dtSurface.Columns.Add("AWBNumber", typeof(string));
            //                                dtSurface.Columns.Add("FlightNumber", typeof(string));
            //                                dtSurface.Columns.Add("TotalPieces", typeof(string));
            //                                dtSurface.Columns.Add("ActualWeight", typeof(string));
            //                                dtSurface.Columns.Add("IssueName", typeof(string));
            //                                dtSurface.Columns.Add("IssuedTo", typeof(string));
            //                                dtSurface.Columns.Add("IssueDate", typeof(DateTime));
            //                                dtSurface.Columns.Add("HAWBNumber", typeof(string));
            //                                dtSurface.Columns.Add("Consignee", typeof(String));
            //                                dtSurface.Columns.Add("ReciversName", typeof(String));
            //                                dtSurface.Columns.Add("FltDate", typeof(DateTime));
            //                                dtSurface.Columns.Add("Desc", typeof(String));
            //                                dtSurface.Columns.Add("DoNumber", typeof(String));
            //                                dtSurface.Columns.Add("SCC", typeof(string));
            //                                dtSurface.Columns.Add("CCAmount", typeof(string));
            //                                dtSurface.Columns.Add("PType", typeof(string));

            //                                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            //                                {
            //                                    DataRow drSurface = dtSurface.NewRow();
            //                                    drSurface["AgentName"] = ds.Tables[0].Rows[k]["AgentName"].ToString();
            //                                    drSurface["AWBNumber"] = ds.Tables[0].Rows[k]["AWBNumber"].ToString();

            //                                    DataSet ds3 = objArr.GetCCAmount(AWBNumber);
            //                                    drSurface["FlightNumber"] = ds.Tables[0].Rows[k]["FlightNumber"].ToString();
            //                                    drSurface["TotalPieces"] = ds.Tables[0].Rows[k]["TotalPieces"].ToString();
            //                                    drSurface["ActualWeight"] = ds.Tables[0].Rows[k]["ActualWeight"].ToString();
            //                                    drSurface["IssueName"] = ds.Tables[0].Rows[k]["IssueName"].ToString();
            //                                    drSurface["IssuedTo"] = ds.Tables[0].Rows[k]["IssuedTo"].ToString();
            //                                    drSurface["IssueDate"] = ds.Tables[0].Rows[k]["FltDate"].ToString();
            //                                    drSurface["HAWBNumber"] = ds.Tables[0].Rows[k]["HAWBNumber"].ToString();
            //                                    drSurface["Consignee"] = ds.Tables[0].Rows[k]["Consignee"].ToString();
            //                                    drSurface["ReciversName"] = ds.Tables[0].Rows[k]["ReciversName"].ToString();
            //                                    drSurface["FltDate"] = ds.Tables[0].Rows[k]["FltDate"].ToString();
            //                                    drSurface["DoNumber"] = ds.Tables[0].Rows[k]["DONumber"].ToString();
            //                                    drSurface["Desc"] = ds.Tables[0].Rows[k]["Desc"].ToString();
            //                                    drSurface["SCC"] = ds.Tables[0].Rows[k]["SCC"].ToString();
            //                                    drSurface["PType"] = "PP";
            //                                    if (ds3 != null)
            //                                    {
            //                                        if (ds3.Tables[0].Rows.Count > 0)
            //                                        {
            //                                            drSurface["CCAmount"] = ds3.Tables[0].Rows[0]["Total"].ToString();
            //                                            drSurface["PType"] = "CC"; //Overwrite PType set above.
            //                                        }
            //                                        ds3.Dispose();
            //                                    }
            //                                    dtSurface.Rows.Add(drSurface);
            //                                }
            //                                Session["DoPrintTracking"] = dtSurface;
            //                                if (dtSurface != null)
            //                                {
            //                                    dtSurface.Dispose();
            //                                }
            //                                Response.Redirect("PrintDeliveryForm.aspx", false);
            //                            }
            //                        }
            //                        ds.Dispose();
            //                    }
            //                }
            //                #endregion ReprintDo
            //        }
            //        else if (GridViewAwbTracking.Rows[i].Cells[1].Text == "Delivery")
            //        {
            //            #region ReprintDo
            //            if (TextBoxAWBno.Text == "")
            //            {
            //                LabelStatus.Text = "Please enter awb number to Print Do";
            //                LabelStatus.ForeColor = Color.Red;
            //                return; 
            //            }
            //            else
            //            {
            //                string AWBNumber = TextBoxAWBno.Text.Trim() ;
            //                string Station = Session["Station"].ToString();

            //                DataSet ds = objArr.GetdataTracking(AWBNumber, Station);
            //                if (ds != null)
            //                {
            //                    if (ds.Tables[0].Rows.Count > 0)
            //                    {
            //                        Session["Tracking"] = ds;
            //                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            //                        {
            //                            DataTable dtSurface = new DataTable();
            //                            dtSurface.Columns.Add("AgentName", typeof(string));
            //                            dtSurface.Columns.Add("AWBNumber", typeof(string));
            //                            dtSurface.Columns.Add("FlightNumber", typeof(string));
            //                            dtSurface.Columns.Add("TotalPieces", typeof(string));
            //                            dtSurface.Columns.Add("ActualWeight", typeof(string));
            //                            dtSurface.Columns.Add("IssueName", typeof(string));
            //                            dtSurface.Columns.Add("IssuedTo", typeof(string));
            //                            dtSurface.Columns.Add("IssueDate", typeof(DateTime));
            //                            dtSurface.Columns.Add("HAWBNumber", typeof(string));
            //                            dtSurface.Columns.Add("Consignee", typeof(String));
            //                            dtSurface.Columns.Add("ReciversName", typeof(String));
            //                            dtSurface.Columns.Add("FltDate", typeof(DateTime));
            //                            dtSurface.Columns.Add("Desc", typeof(String));
            //                            dtSurface.Columns.Add("DoNumber", typeof(String));
            //                            dtSurface.Columns.Add("SCC", typeof(string));
            //                            dtSurface.Columns.Add("CCAmount", typeof(string));
            //                            dtSurface.Columns.Add("PType", typeof(string));

            //                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            //                            {
            //                                //DataSet ds = (DataSet)Session["DsDoDetails"];

            //                                DataRow drSurface = dtSurface.NewRow();
            //                                drSurface["AgentName"] = ds.Tables[0].Rows[k]["AgentName"].ToString();
            //                                //string AWBNumber = ((Label)grdDeliveryDetails.Rows[i].FindControl("txtawbno")).Text;
            //                                drSurface["AWBNumber"] = ds.Tables[0].Rows[k]["AWBNumber"].ToString();
            //                                DataSet ds3 = objArr.GetCCAmount(AWBNumber);
            //                                //drSurface["AWBNumber"] = ((Label)grdDeliveryDetails.Rows[i].FindControl("txtawbno")).Text;
            //                                drSurface["FlightNumber"] = ds.Tables[0].Rows[k]["FlightNumber"].ToString();
            //                                drSurface["TotalPieces"] = ds.Tables[0].Rows[k]["TotalPieces"].ToString();
            //                                drSurface["ActualWeight"] = ds.Tables[0].Rows[k]["ActualWeight"].ToString();
            //                                drSurface["IssueName"] = ds.Tables[0].Rows[k]["IssueName"].ToString();
            //                                drSurface["IssuedTo"] = ds.Tables[0].Rows[k]["IssuedTo"].ToString();
            //                                drSurface["IssueDate"] = ds.Tables[0].Rows[k]["FltDate"].ToString();
            //                                drSurface["HAWBNumber"] = ds.Tables[0].Rows[k]["HAWBNumber"].ToString();
            //                                drSurface["Consignee"] = ds.Tables[0].Rows[k]["Consignee"].ToString();
            //                                drSurface["ReciversName"] = ds.Tables[0].Rows[k]["ReciversName"].ToString();
            //                                drSurface["FltDate"] = ds.Tables[0].Rows[k]["FltDate"].ToString();
            //                                drSurface["DoNumber"] = ds.Tables[0].Rows[k]["DONumber"].ToString();
            //                                drSurface["Desc"] = ds.Tables[0].Rows[k]["Desc"].ToString();
            //                                drSurface["SCC"] = ds.Tables[0].Rows[k]["SCC"].ToString();
            //                                drSurface["PType"] = "PP"; 
            //                                if (ds3 != null)
            //                                {
            //                                    if (ds3.Tables[0].Rows.Count > 0)
            //                                    {
            //                                        drSurface["CCAmount"] = ds3.Tables[0].Rows[0]["Total"].ToString();
            //                                        drSurface["PType"] = "CC";
            //                                    }
            //                                    ds3.Dispose();
            //                                }
            //                                dtSurface.Rows.Add(drSurface);
            //                            }
            //                            Session["DoPrintTracking"] = dtSurface;
            //                            if (dtSurface != null)
            //                            {
            //                                dtSurface.Dispose();
            //                            }
            //                            Response.Redirect("PrintDeliveryForm.aspx", false);
            //                        }
            //                    }
            //                    ds.Dispose();
            //                }
            //           }
            //            #endregion ReprintDo 
            //        }        
            //    }
            //}
            //catch (Exception)
            //{ }
            #endregion

            try
            {
                double ST = 0.0;
                string CustomerSupport = "";
                #region ReprintDo
                if (TextBoxAWBno.Text == "")
                {
                    LabelStatus.Text = "Please enter awb number to Print Do";
                    LabelStatus.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    string AWBNumber = txtPrefix.Text.Trim() + '-' + TextBoxAWBno.Text.Trim();
                    string Station = Session["Station"].ToString();

                    DataSet ds = objArr.GetdataTracking(AWBNumber, Station, "");
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            DataTable dtHeader = new DataTable();
                            dtHeader = ds.Tables[0];

                            dtHeader.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));

                            DataRow drHeader = dtHeader.NewRow();
                            drHeader["DoNumber"] = dtHeader.Rows[0]["DoNumber"].ToString();
                            drHeader["IssuedTo"] = dtHeader.Rows[0]["IssuedTo"].ToString();
                            drHeader["IssueName"] = dtHeader.Rows[0]["IssueName"].ToString();
                            drHeader["ReciverName"] = dtHeader.Rows[0]["ReciverName"].ToString();
                            drHeader["IssueDate"] = dtHeader.Rows[0]["IssueDate"].ToString();
                            drHeader["StaffId"] = dtHeader.Rows[0]["StaffId"].ToString();
                            drHeader["Station"] = dtHeader.Rows[0]["Station"].ToString();
                            if (Session["ST"] != null)
                                ST = Convert.ToDouble(Session["ST"].ToString());
                            else
                            {
                                MasterBAL objBal = new MasterBAL();
                                Session["ST"] = objBal.getServiceTax();
                                ST = Convert.ToDouble(Session["ST"].ToString());
                            }

                            drHeader["ServiceTax"] = ST;
                            drHeader["Consignee"] = dtHeader.Rows[0]["Consignee"].ToString();
                            drHeader["Total"] = dtHeader.Rows[0]["Total"].ToString();
                            DataSet dsCustomerSupport = null;
                            try
                            {
                                dsCustomerSupport = da.SelectRecords("Sp_GetCustomerSupportInfo");
                                if (dsCustomerSupport.Tables[0].Rows.Count > 0)
                                {
                                    CustomerSupport = dsCustomerSupport.Tables[0].Rows[0]["CustomerSupport"].ToString();
                                }
                                else
                                {
                                    CustomerSupport = "";
                                }
                            }
                            catch
                            {
                                CustomerSupport = "";
                            }
                            finally
                            {
                                if (dsCustomerSupport != null)
                                {
                                    dsCustomerSupport.Dispose();
                                }
                            }
                            drHeader["Customer"] = CustomerSupport;

                            //img for report
                            System.IO.MemoryStream Logo1 = null;
                            try
                            {
                                Logo1 = CommonUtility.GetImageStream(Page.Server);
                            }
                            catch (Exception)
                            {
                                Logo1 = new System.IO.MemoryStream();
                            }

                            drHeader["Logo"] = Logo1.ToArray();

                            dtHeader.Rows.Clear();
                            dtHeader.Rows.Add(drHeader);

                            if (Logo1 != null)
                            {
                                Logo1.Dispose();
                            }

                            #region create dtSurface and dtUlDDetails table
                            DataTable dtULDDetails = new DataTable();
                            DataTable dtSurface = new DataTable();
                            dtULDDetails.Columns.Add("ULDNumber", typeof(string));
                            dtULDDetails.Columns.Add("AWBCount", typeof(string));
                            dtULDDetails.Columns.Add("FlightNumber", typeof(string));
                            dtULDDetails.Columns.Add("ActualPieces", typeof(string));
                            dtULDDetails.Columns.Add("ActualWeight", typeof(string));
                            dtULDDetails.Columns.Add("IssueDate", typeof(string));


                            dtSurface = ds.Tables[1];

                            ////////////////////////////////

                            dtSurface.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));

                            DataRow drSurface = dtSurface.NewRow();
                            drSurface["DoNumber"] = dtSurface.Rows[0]["DoNumber"].ToString();
                            drSurface["AWBNumber"] = dtSurface.Rows[0]["AWBNumber"].ToString();
                            drSurface["HAWBNo"] = dtSurface.Rows[0]["HAWBNo"].ToString();
                            drSurface["FlightNumber"] = dtSurface.Rows[0]["FlightNumber"].ToString();
                            drSurface["IssuedTo"] = dtSurface.Rows[0]["IssuedTo"].ToString();
                            drSurface["IssueName"] = dtSurface.Rows[0]["IssueName"].ToString();
                            drSurface["ReciverName"] = dtSurface.Rows[0]["ReciverName"].ToString();
                            drSurface["ActualPieces"] = dtSurface.Rows[0]["ActualPieces"].ToString();
                            drSurface["ActualWeight"] = dtSurface.Rows[0]["ActualWeight"].ToString();
                            drSurface["IssueDate"] = dtSurface.Rows[0]["IssueDate"].ToString();
                            drSurface["AgentName"] = dtSurface.Rows[0]["AgentName"].ToString();
                            drSurface["Discription"] = dtSurface.Rows[0]["Discription"].ToString();
                            drSurface["ConsigneeName"] = dtSurface.Rows[0]["ConsigneeName"].ToString();
                            drSurface["CCAmount"] = dtSurface.Rows[0]["CCAmount"].ToString();
                            drSurface["PayMode"] = dtSurface.Rows[0]["PayMode"].ToString();
                            drSurface["CommCode"] = dtSurface.Rows[0]["CommCode"].ToString();
                            drSurface["Station"] = dtSurface.Rows[0]["Station"].ToString();
                            drSurface["StaffId"] = dtSurface.Rows[0]["StaffId"].ToString();
                            drSurface["CommCode"] = dtSurface.Rows[0]["CommCode"].ToString();

                            if (Session["ST"] != null)
                                ST = Convert.ToDouble(Session["ST"].ToString());
                            else
                            {
                                MasterBAL objBal = new MasterBAL();
                                Session["ST"] = objBal.getServiceTax();
                                ST = Convert.ToDouble(Session["ST"].ToString());
                            }

                            drSurface["ServiceTax"] = ST;
                            DataSet dsCustomer = null;
                            try
                            {
                                dsCustomer = da.SelectRecords("Sp_GetCustomerSupportInfo");
                                if (dsCustomer.Tables[0].Rows.Count > 0)
                                {
                                    CustomerSupport = dsCustomer.Tables[0].Rows[0]["CustomerSupport"].ToString();
                                }
                                else
                                {
                                    CustomerSupport = "";
                                }
                            }
                            catch
                            {
                                CustomerSupport = "";
                            }
                            finally
                            {
                                if (dsCustomer != null)
                                {
                                    dsCustomer.Dispose();
                                }
                            }
                            drSurface["CustomerSupport"] = CustomerSupport;
                            drSurface["Remarks"] = dtSurface.Rows[0]["Remarks"].ToString();
                            drSurface["DODate"] = dtSurface.Rows[0]["DODate"].ToString();

                            //img for report
                            System.IO.MemoryStream Logo2 = null;
                            try
                            {
                                Logo2 = CommonUtility.GetImageStream(Page.Server);
                            }
                            catch (Exception)
                            {
                                Logo2 = new System.IO.MemoryStream();
                            }

                            drSurface["Logo"] = Logo2.ToArray();

                            dtSurface.Rows.Clear();
                            dtSurface.Rows.Add(drSurface);

                            if (Logo2 != null)
                            {
                                Logo2.Dispose();
                            }

                            /////////////////////////////////

                            #endregion

                            DataSet dset = new DataSet();
                            dset.Tables.Add(dtHeader.Copy());
                            dset.Tables.Add(dtSurface.Copy());
                            dset.Tables.Add(dtULDDetails.Copy());

                            if (dset != null)
                            {
                                Session["abc"] = dset;
                                if (dset.Tables.Count > 0)
                                {
                                    if (dset.Tables[0].Rows.Count > 0 && dset.Tables[1].Rows.Count > 0 || dset.Tables[0].Rows.Count > 0 && dset.Tables[2].Rows.Count > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Download();</SCRIPT>", false);
                                    }
                                    else
                                    {
                                    }
                                }
                                dset.Dispose();
                            }

                        }
                        ds.Dispose();
                    }
                }
                #endregion ReprintDo

            }
            catch (Exception ex)
            { }


        }
        #endregion printDo

        #region DO Number Grid ItemCommand
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            try
            {
                if (e.CommandName == "Print")
                {
                    try
                    {
                        double ST = 0.0;
                        string CustomerSupport = "";
                        #region ReprintDo
                        if (TextBoxAWBno.Text == "")
                        {
                            LabelStatus.Text = "Please enter awb number to Print Do";
                            LabelStatus.ForeColor = Color.Red;
                            return;
                        }
                        else
                        {
                            string AWBNumber = txtPrefix.Text.Trim() + '-' + TextBoxAWBno.Text.Trim();
                            string Station = Session["Station"].ToString();
                            string strHeadOrg="",strHeadDest="";
                            DataSet ds = objArr.GetdataTracking(AWBNumber, Station, e.CommandArgument.ToString());
                            if (ds != null)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {

                                    #region Header

                                    DataTable dtHeader = new DataTable();
                                    dtHeader = ds.Tables[0];

                                    dtHeader.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                                    dtHeader.Columns.Add("DODate", typeof(string));
                                    dtHeader.Columns.Add("PayMode", typeof(string));
                                    dtHeader.Columns.Add("AgentName", typeof(string));
                                    dtHeader.Columns.Add("Remarks", typeof(string));
                                    dtHeader.Columns.Add("CommCode", typeof(string));
                                    dtHeader.Columns.Add("Rules", typeof(string));
                                    DataRow drHeader = dtHeader.NewRow();
                                    drHeader["DoNumber"] = dtHeader.Rows[0]["DoNumber"].ToString();
                                    drHeader["IssuedTo"] = dtHeader.Rows[0]["IssuedTo"].ToString();
                                    drHeader["IssueName"] = dtHeader.Rows[0]["IssueName"].ToString();
                                    drHeader["ReciverName"] = dtHeader.Rows[0]["ReciverName"].ToString();
                                    drHeader["IssueDate"] = dtHeader.Rows[0]["IssueDate"].ToString();
                                    drHeader["StaffId"] = dtHeader.Rows[0]["StaffId"].ToString();
                                    drHeader["Station"] = dtHeader.Rows[0]["Station"].ToString();
                                    if (Session["ST"] != null)
                                        ST = Convert.ToDouble(Session["ST"].ToString());
                                    else
                                    {
                                        MasterBAL objBal = new MasterBAL();
                                        Session["ST"] = objBal.getServiceTax();
                                        ST = Convert.ToDouble(Session["ST"].ToString());
                                    }
                                    drHeader["AwbOrg"] = dtHeader.Rows[0]["AwbOrg"].ToString(); ;
                                    drHeader["AwbDest"] = dtHeader.Rows[0]["AwbDest"].ToString(); ;
                                    drHeader["ServiceTax"] = ST;
                                    drHeader["Consignee"] = dtHeader.Rows[0]["Consignee"].ToString();
                                    drHeader["Total"] = dtHeader.Rows[0]["Total"].ToString();
                                    strHeadOrg = dtHeader.Rows[0]["AwbOrg"].ToString();
                                    strHeadDest = dtHeader.Rows[0]["AwbDest"].ToString(); 
                                    DataSet dsCustomerSupport = null;
                                    string CustomerRules = "";
                                    try
                                    {
                                        dsCustomerSupport = da.SelectRecords("Sp_GetCustomerSupportInfo");
                                        if (dsCustomerSupport.Tables[0].Rows.Count > 0)
                                        {
                                            CustomerSupport = dsCustomerSupport.Tables[0].Rows[0]["CustomerSupport"].ToString();
                                            CustomerRules = dsCustomerSupport.Tables[0].Rows[0]["CustomerRules"].ToString();

                                        }
                                    }
                                    catch
                                    {
                                    }
                                    finally
                                    {
                                        if (dsCustomerSupport != null)
                                        {
                                            dsCustomerSupport.Dispose();
                                        }
                                    }
                                    drHeader["Customer"] = CustomerSupport;
                                    drHeader["Rules"] = CustomerRules;

                                    //img for report
                                    System.IO.MemoryStream Logo1 = null;
                                    try
                                    {
                                        Logo1 = CommonUtility.GetImageStream(Page.Server);
                                    }
                                    catch (Exception)
                                    {
                                        Logo1 = new System.IO.MemoryStream();
                                    }

                                    drHeader["Logo"] = Logo1.ToArray();
                                    drHeader["DODate"] = ds.Tables[1].Rows[0]["DODate"].ToString();
                                    drHeader["PayMode"] = ds.Tables[1].Rows[0]["PayMode"].ToString();
                                    drHeader["AgentName"] = ds.Tables[1].Rows[0]["AgentName"].ToString();
                                    drHeader["Remarks"] = ds.Tables[1].Rows[0]["Remarks"].ToString();
                                    drHeader["CommCode"] = ds.Tables[1].Rows[0]["CommCode"].ToString();

                                    dtHeader.Rows.Clear();
                                    dtHeader.Rows.Add(drHeader);

                                    if (Logo1 != null)
                                    {
                                        Logo1.Dispose();
                                    }
                                    #endregion

                                    #region create dtSurface and dtUlDDetails table

                                    DataTable dtULDDetails = new DataTable();
                                    DataTable dtSurface = new DataTable();
                                    dtULDDetails.Columns.Add("ULDNumber", typeof(string));
                                    dtULDDetails.Columns.Add("AWBCount", typeof(string));
                                    dtULDDetails.Columns.Add("FlightNumber", typeof(string));
                                    dtULDDetails.Columns.Add("ActualPieces", typeof(string));
                                    dtULDDetails.Columns.Add("ActualWeight", typeof(string));
                                    dtULDDetails.Columns.Add("IssueDate", typeof(string));


                                    dtSurface.Columns.Add("DoNumber", typeof(string));
                                    dtSurface.Columns.Add("AWBNumber", typeof(string));
                                    dtSurface.Columns.Add("HAWBNo", typeof(string));
                                    dtSurface.Columns.Add("FlightNumber", typeof(string));
                                    dtSurface.Columns.Add("IssuedTo", typeof(string));
                                    dtSurface.Columns.Add("IssueName", typeof(string));
                                    dtSurface.Columns.Add("ReciverName", typeof(string));
                                    dtSurface.Columns.Add("ActualPieces", typeof(string));
                                    dtSurface.Columns.Add("ActualWeight", typeof(string));
                                    dtSurface.Columns.Add("IssueDate", typeof(string));
                                    dtSurface.Columns.Add("AgentName", typeof(string));
                                    dtSurface.Columns.Add("Discription", typeof(string));
                                    dtSurface.Columns.Add("ConsigneeName", typeof(string));
                                    dtSurface.Columns.Add("CCAmount", typeof(string));
                                    dtSurface.Columns.Add("PayMode", typeof(string));
                                    dtSurface.Columns.Add("CommCode", typeof(string));
                                    dtSurface.Columns.Add("Station", typeof(string));
                                    dtSurface.Columns.Add("StaffId", typeof(string));
                                    dtSurface.Columns.Add("ServiceTax", typeof(string));
                                    dtSurface.Columns.Add("CustomerSupport", typeof(string));
                                    dtSurface.Columns.Add("Remarks", typeof(string));
                                    dtSurface.Columns.Add("DODate", typeof(string));
                                    dtSurface.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
                                    dtSurface.Columns.Add("AwbDest", typeof(string));
                                    dtSurface.Columns.Add("AwbOrg", typeof(string));

                                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                    {
                                        DataRow drSurface = dtSurface.NewRow();
                                        drSurface["DoNumber"] = ds.Tables[1].Rows[i]["DoNumber"].ToString();
                                        drSurface["AWBNumber"] = ds.Tables[1].Rows[i]["AWBNumber"].ToString();
                                        drSurface["HAWBNo"] = ds.Tables[1].Rows[i]["HAWBNo"].ToString();
                                        drSurface["FlightNumber"] = ds.Tables[1].Rows[i]["FlightNumber"].ToString();
                                        drSurface["IssuedTo"] = ds.Tables[1].Rows[i]["IssuedTo"].ToString();
                                        drSurface["IssueName"] = ds.Tables[1].Rows[i]["IssueName"].ToString();
                                        drSurface["ReciverName"] = ds.Tables[1].Rows[i]["ReciverName"].ToString();
                                        drSurface["ActualPieces"] = ds.Tables[1].Rows[i]["ActualPieces"].ToString();
                                        drSurface["ActualWeight"] = ds.Tables[1].Rows[i]["ActualWeight"].ToString();
                                        drSurface["IssueDate"] = ds.Tables[1].Rows[i]["IssueDate"].ToString();
                                        drSurface["AgentName"] = ds.Tables[1].Rows[i]["AgentName"].ToString();
                                        drSurface["Discription"] = ds.Tables[1].Rows[i]["Discription"].ToString();
                                        drSurface["ConsigneeName"] = ds.Tables[1].Rows[i]["ConsigneeName"].ToString();
                                        drSurface["CCAmount"] = ds.Tables[1].Rows[i]["CCAmount"].ToString();
                                        drSurface["PayMode"] = ds.Tables[1].Rows[i]["PayMode"].ToString();
                                        drSurface["CommCode"] = ds.Tables[1].Rows[i]["CommCode"].ToString();
                                        drSurface["Station"] = ds.Tables[1].Rows[i]["Station"].ToString();
                                        drSurface["StaffId"] = ds.Tables[1].Rows[i]["StaffId"].ToString();
                                        drSurface["AwbOrg"] =strHeadOrg;
                                        drSurface["AwbDest"] =strHeadDest;
                                       
                                        if (Session["ST"] != null)
                                            ST = Convert.ToDouble(Session["ST"].ToString());
                                        else
                                        {
                                            MasterBAL objBal = new MasterBAL();
                                            Session["ST"] = objBal.getServiceTax();
                                            ST = Convert.ToDouble(Session["ST"].ToString());
                                        }

                                        drSurface["ServiceTax"] = ST;
                                        DataSet dsCustomer = null;
                                        try
                                        {
                                            dsCustomer = da.SelectRecords("Sp_GetCustomerSupportInfo");
                                            if (dsCustomer.Tables[0].Rows.Count > 0)
                                            {
                                                CustomerSupport = dsCustomer.Tables[0].Rows[0]["CustomerSupport"].ToString();
                                            }
                                            else
                                            {
                                                CustomerSupport = "";
                                            }
                                        }
                                        catch
                                        {
                                            CustomerSupport = "";
                                        }
                                        finally
                                        {
                                            if (dsCustomer != null)
                                            {
                                                dsCustomer.Dispose();
                                            }
                                        }
                                        drSurface["CustomerSupport"] = CustomerSupport;
                                        drSurface["Remarks"] = ds.Tables[1].Rows[0]["Remarks"].ToString();
                                        drSurface["DODate"] = ds.Tables[1].Rows[0]["DODate"].ToString();

                                        dtSurface.Rows.Add(drSurface);
                                    }

                                    //img for report
                                    System.IO.MemoryStream Logo2 = null;
                                    try
                                    {
                                        Logo2 = CommonUtility.GetImageStream(Page.Server);
                                    }
                                    catch (Exception)
                                    {
                                        Logo2 = new System.IO.MemoryStream();
                                    }

                                    if (dtSurface.Columns.Contains("Logo"))
                                        dtSurface.Columns["Logo"].DefaultValue = Logo2.ToArray();

                                    if (Logo2 != null)
                                    {
                                        Logo2.Dispose();
                                    }

                                    /////////////////////////////////

                                    #endregion

                                    DataSet dset = new DataSet();
                                    dset.Tables.Add(dtHeader.Copy());
                                    dset.Tables.Add(dtSurface.Copy());
                                    dset.Tables.Add(dtULDDetails.Copy());

                                    if (dset != null)
                                    {
                                        Session["abc"] = dset;
                                        if (dset.Tables.Count > 0)
                                        {
                                            if (dset.Tables[0].Rows.Count > 0 && dset.Tables[1].Rows.Count > 0 || dset.Tables[0].Rows.Count > 0 && dset.Tables[2].Rows.Count > 0)
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>Download();</SCRIPT>", false);
                                            }
                                            else
                                            {
                                            }
                                        }
                                        dset.Dispose();
                                    }

                                }
                                ds.Dispose();
                            }
                        }
                        #endregion ReprintDo

                    }
                    catch (Exception ex)
                    { }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion
    }
}
