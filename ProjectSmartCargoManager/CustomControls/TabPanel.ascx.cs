using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class TabPanel : System.Web.UI.UserControl
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                LoadDestination();
                GetData_Click(sender, e);
            }

        }

        #region OnActiveTab_Changed
        protected void OnActiveTab_Changed(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>CallPopulateClick();</script>", false);
            }
            catch (Exception ex)
            { }
        }
        #endregion

        # region grdImportList_PageIndexChanging
        protected void grdImportList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //DateTime fromDate;
            //DateTime ToDate;
            DateTime fromDate = DateTime.Now, ToDate = DateTime.Now;
            //txtFrmDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            //txtToDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            fromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
            ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
            string Location;
            if (ddlLocation.SelectedIndex > 0)
            {
                Location = ddlLocation.SelectedItem.Text.Trim();
            }
            else
                //import and Export 
                Location = Session["Station"].ToString();
            //string Source = ddlLocation.SelectedItem.Text.Trim() == "All" ? "" : ddlLocation.SelectedItem.Text.Trim();
            //string Source = ddlLocation.SelectedItem.Text.Trim();
            #region Parameter
            string[] PName = new string[3];
            PName[0] = "Source";
            PName[1] = "frmDate";
            PName[2] = "ToDate";

            object[] PValue = new object[3];
            PValue[0] = Location;
            PValue[1] = fromDate;//txtFrmDate.Text;
            PValue[2] = ToDate;//txtToDate.Text;

            SqlDbType[] PType = new SqlDbType[3];
            PType[0] = SqlDbType.VarChar;
            PType[1] = SqlDbType.Date;
            PType[2] = SqlDbType.Date;

            #endregion
            DataSet dsimpexp = da.SelectRecords("SP_GetImport", PName, PValue, PType);


            try
            {
                if (dsimpexp != null)
                {
                    if (dsimpexp.Tables != null)
                    {
                        if (dsimpexp.Tables.Count > 0)
                        {
                            if (dsimpexp.Tables[0].Rows.Count > 0)
                            {
                                grdImportList.PageIndex = 0;
                                grdImportList.DataSource = dsimpexp;
                                grdImportList.DataMember = dsimpexp.Tables[0].TableName;
                                grdImportList.DataBind();
                                grdImportList.Visible = true;
                                //dsFLAB.Clear();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            grdImportList.PageIndex = e.NewPageIndex;
            grdImportList.DataSource = dsimpexp.Copy();
            grdImportList.DataBind();



        }
        # endregion grvNFLABList_PageIndexChanging

        # region grdexportList_PageIndexChanging
        protected void grdexportList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //DateTime fromDate;
            //DateTime ToDate;
            DateTime fromDate = DateTime.Now, ToDate = DateTime.Now;
            //txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            fromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
            ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
            string Location;
            if (ddlLocation.SelectedIndex > 0)
            {
                Location = ddlLocation.SelectedItem.Text.Trim();
            }
            else
                //import and Export 
                Location = Session["Station"].ToString();
            //string Source = ddlLocation.SelectedItem.Text.Trim() == "All" ? "" : ddlLocation.SelectedItem.Text.Trim();
            //string Source = ddlLocation.SelectedItem.Text.Trim();
            #region Parameter
            string[] PName = new string[3];
            PName[0] = "Source";
            PName[1] = "frmDate";
            PName[2] = "ToDate";

            object[] PValue = new object[3];
            PValue[0] = Location;
            PValue[1] = fromDate;//txtFrmDate.Text;
            PValue[2] = ToDate;//txtToDate.Text;

            SqlDbType[] PType = new SqlDbType[3];
            PType[0] = SqlDbType.VarChar;
            PType[1] = SqlDbType.Date;
            PType[2] = SqlDbType.Date;

            #endregion
            DataSet dsimpexp = da.SelectRecords("SP_GetExport", PName, PValue, PType);


            try
            {
                if (dsimpexp != null)
                {
                    if (dsimpexp.Tables != null)
                    {
                        if (dsimpexp.Tables.Count > 0)
                        {
                            if (dsimpexp.Tables[0].Rows.Count > 0)
                            {
                                grdexportList.PageIndex = 0;
                                grdexportList.DataSource = dsimpexp;
                                grdexportList.DataMember = dsimpexp.Tables[0].TableName;
                                grdexportList.DataBind();
                                grdexportList.Visible = true;
                                //dsFLAB.Clear();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            grdexportList.PageIndex = e.NewPageIndex;
            grdexportList.DataSource = dsimpexp.Copy();
            grdexportList.DataBind();



        }
        # endregion grvNFLABList_PageIndexChanging

        #region validateUser
        private bool validateUser()
        {
            bool flag = false;
            try
            {
                HomeBL objHome = new HomeBL();
                int RoleId = Convert.ToInt32(Session["RoleID"]);
                DataSet objDS = objHome.GetUserPermissions(((System.Web.UI.TemplateControl)(Page)).AppRelativeVirtualPath, RoleId);
                objHome = null;

                if (objDS != null && objDS.Tables.Count > 0 && objDS.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < objDS.Tables[0].Rows.Count; j++)
                    {
                        if (objDS.Tables[0].Rows[j]["ControlId"].ToString() == "FlightDashboard")
                        {
                            //string userLoc = "";
                            //userLoc = Session["Station"].ToString();
                            //ddlLocation.Text = userLoc;
                            //ddlLocation.Enabled = false;
                            flag = true;
                            break;
                        }
                    }
                }
                objDS = null;

            }
            catch (Exception ex)
            { }
            return flag;
        }
        #endregion

        #region Load Location Dropdown
        /// <summary>
        /// Lodas location list in Origin and Destination dropdowns.
        /// </summary>
        public void LoadDestination()
        {
            try
            {
                BookingBAL objBLL = new BookingBAL();
                DataSet ds = objBLL.GetDestinationsForSource("");
                if (ds != null)
                {
                    ddlLocation.Items.Clear();
                    DataRow row = ds.Tables[0].NewRow();

                    ddlLocation.Items.Add("All");
                    ddlLocation.DataSource = ds;
                    ddlLocation.DataMember = ds.Tables[0].TableName;
                    ddlLocation.DataTextField = "AirportCode";
                    ddlLocation.DataValueField = "AirportCode";
                    ddlLocation.DataBind();

                    ddlLocation.SelectedIndex = 0;


                    // ----------- For ddlLocationAg------------

                    //ddlLocationAg.Items.Add("All");
                    //ddlLocationAg.DataSource = ds;
                    //ddlLocationAg.DataMember = ds.Tables[0].TableName;
                    //ddlLocationAg.DataTextField = "AirportCode";
                    //ddlLocationAg.DataValueField = "AirportCode";
                    //ddlLocationAg.DataBind();

                    //ddlLocationAg.SelectedIndex = 0;

                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Load Location Dropdown

        #region btnGetDataFlights_Click
        protected void GetData_Click(object sender, EventArgs e)
        {
            try
            {

                DataSet dsData = new DataSet();
                BAL.Jquery objJQ = new Jquery();
                string Location = "";
                DateTime FromDate = DateTime.Now, ToDate = DateTime.Now;
                //if (TabContainer1.ActiveTabIndex == 0)  // If Flights Tab is open
                {
                    if (ddlLocation.SelectedIndex > 0)
                    {
                        Location = ddlLocation.SelectedItem.Text.Trim();
                    }
                    else
                    {
                        Location = Session["Station"] != null ? Session["Station"].ToString() : string.Empty;
                    }
                    FromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
                    ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
                }
                //else if (TabContainer1.ActiveTabIndex == 1)
                //{
                //    if (ddlLocationAg.SelectedIndex > 0)
                //    {
                //        Location = ddlLocation.SelectedItem.Text.Trim();
                //    }

                //   FromDate = DateTime.ParseExact(txtFrmDtAg.Text.Trim(), "dd/MM/yyyy", null);
                //   ToDate = DateTime.ParseExact(txtToDtAg.Text.Trim(), "dd/MM/yyyy", null);
                //}

                dsData = objJQ.getDataForFlightDashboard(Location, FromDate, ToDate);
                #region import export
                DateTime fromDate;
                //DateTime ToDate;
                //txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                fromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
                ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
                //import and Export 
                //string Source = Session["Station"].ToString();
                //string Source = ddlLocation.SelectedItem.Text.Trim() == "All" ? "" : ddlLocation.SelectedItem.Text.Trim();
                //string Source = ddlLocation.SelectedItem.Text.Trim();
                #region Parameter
                string[] PName = new string[3];
                PName[0] = "Source";
                PName[1] = "frmDate";
                PName[2] = "ToDate";

                object[] PValue = new object[3];
                PValue[0] = Location;
                PValue[1] = fromDate;//txtFrmDate.Text;
                PValue[2] = ToDate;//txtToDate.Text;

                SqlDbType[] PType = new SqlDbType[3];
                PType[0] = SqlDbType.VarChar;
                PType[1] = SqlDbType.Date;
                PType[2] = SqlDbType.Date;

                #endregion
                DataSet dsimpexp = da.SelectRecords("SP_GetImport", PName, PValue, PType);
                try
                {
                    if (dsimpexp != null)
                    {
                        if (dsimpexp.Tables != null)
                        {
                            if (dsimpexp.Tables.Count > 0)
                            {
                                if (dsimpexp.Tables[0].Rows.Count > 0)
                                {
                                    grdImportList.PageIndex = 0;
                                    grdImportList.DataSource = dsimpexp;
                                    grdImportList.DataMember = dsimpexp.Tables[0].TableName;
                                    grdImportList.DataBind();
                                    grdImportList.Visible = true;
                                    //dsFLAB.Clear();

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
                //DateTime fromDate;
                //DateTime ToDate;
                //txtFrmDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //fromDate = DateTime.ParseExact(txtFrmDate.Text.Trim(), "dd/MM/yyyy", null);
                //ToDate = DateTime.ParseExact(txtToDate.Text.Trim(), "dd/MM/yyyy", null);
                //import and Export 
                //string Location = Session["Station"].ToString();
                //string Source = ddlLocation.SelectedItem.Text.Trim() == "All" ? "" : ddlLocation.SelectedItem.Text.Trim();
                //string Source = ddlLocation.SelectedItem.Text.Trim();
                #region Parameter
                string[] PName1 = new string[3];
                PName1[0] = "Source";
                PName1[1] = "frmDate";
                PName1[2] = "ToDate";

                object[] PValue1 = new object[3];
                PValue1[0] = Location;
                PValue1[1] = fromDate;//txtFrmDate.Text;
                PValue1[2] = ToDate;//txtToDate.Text;

                SqlDbType[] PType1 = new SqlDbType[3];
                PType1[0] = SqlDbType.VarChar;
                PType1[1] = SqlDbType.Date;
                PType1[2] = SqlDbType.Date;

                #endregion
                DataSet dsimp = da.SelectRecords("SP_GetExport", PName1, PValue1, PType1);
                try
                {
                    if (dsimp != null)
                    {
                        if (dsimp.Tables != null)
                        {
                            if (dsimp.Tables.Count > 0)
                            {
                                if (dsimp.Tables[0].Rows.Count > 0)
                                {
                                    grdexportList.PageIndex = 0;
                                    grdexportList.DataSource = dsimp;
                                    grdexportList.DataMember = dsimp.Tables[0].TableName;
                                    grdexportList.DataBind();
                                    grdexportList.Visible = true;
                                    //dsFLAB.Clear();

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                { }
                #endregion


                //--- Set All Labels Some Default Value
                lblG1Flt1.Text = "";
                lblG1Flt2.Text = "";
                lblG1Flt3.Text = "";
                lblG1Flt4.Text = "";
                lblG1Flt5.Text = "";
                lblG1Wgt1.Text = "0";
                lblG1Wgt2.Text = "0";
                lblG1Wgt3.Text = "0";
                lblG1Wgt4.Text = "0";
                lblG1Wgt5.Text = "0";
                lblG2Flt1.Text = "";
                lblG2Flt2.Text = "";
                lblG2Flt3.Text = "";
                lblG2Flt4.Text = "";
                lblG2Flt5.Text = "";
                lblG2Wgt1.Text = "0";
                lblG2Wgt2.Text = "0";
                lblG2Wgt3.Text = "0";
                lblG2Wgt4.Text = "0";
                lblG2Wgt5.Text = "0";
                lblAgent1G1.Text = "";
                lblAgent2G1.Text = "";
                lblAgent3G1.Text = "";
                lblAgent4G1.Text = "";
                lblAgent5G1.Text = "";
                lblTopAgTon1.Text = "0";
                lblTopAgTon2.Text = "0";
                lblTopAgTon3.Text = "0";
                lblTopAgTon4.Text = "0";
                lblTopAgTon5.Text = "0";
                lblAgent1G2.Text = "";
                lblAgent2G2.Text = "";
                lblAgent3G2.Text = "";
                lblAgent4G2.Text = "";
                lblAgent5G2.Text = "";
                lblBotAgTon1.Text = "0";
                lblBotAgTon2.Text = "0";
                lblBotAgTon3.Text = "0";
                lblBotAgTon4.Text = "0";
                lblBotAgTon5.Text = "0";
                if (dsData != null)
                {
                    if (dsData.Tables.Count > 1)
                    {
                        if (dsData.Tables[0].Rows.Count > 0)
                        {

                            try
                            {
                                if (dsData.Tables[0].Rows.Count > 0)
                                {
                                    lblG1Flt1.Text = dsData.Tables[0].Rows[0][0].ToString();
                                    lblG1Wgt1.Text = dsData.Tables[0].Rows[0][1].ToString();
                                }
                                if (dsData.Tables[0].Rows.Count > 1)
                                {
                                    lblG1Flt2.Text = dsData.Tables[0].Rows[1][0].ToString();
                                    lblG1Wgt2.Text = dsData.Tables[0].Rows[1][1].ToString();
                                }
                                if (dsData.Tables[0].Rows.Count > 2)
                                {
                                    lblG1Flt3.Text = dsData.Tables[0].Rows[2][0].ToString();
                                    lblG1Wgt3.Text = dsData.Tables[0].Rows[2][1].ToString();
                                }
                                if (dsData.Tables[0].Rows.Count > 3)
                                {
                                    lblG1Flt4.Text = dsData.Tables[0].Rows[3][0].ToString();
                                    lblG1Wgt4.Text = dsData.Tables[0].Rows[3][1].ToString();
                                }
                                if (dsData.Tables[0].Rows.Count > 4)
                                {
                                    lblG1Flt5.Text = dsData.Tables[0].Rows[4][0].ToString();
                                    lblG1Wgt5.Text = dsData.Tables[0].Rows[4][1].ToString();
                                }
                            }
                            catch (Exception ex)
                            { }
                        }
                        if (dsData.Tables[1].Rows.Count > 0)
                        {

                            try
                            {
                                if (dsData.Tables[1].Rows.Count > 0)
                                {
                                    lblG2Flt1.Text = dsData.Tables[1].Rows[0][0].ToString();
                                    lblG2Wgt1.Text = dsData.Tables[1].Rows[0][1].ToString();
                                }
                                if (dsData.Tables[1].Rows.Count > 1)
                                {
                                    lblG2Flt2.Text = dsData.Tables[1].Rows[1][0].ToString();
                                    lblG2Wgt2.Text = dsData.Tables[1].Rows[1][1].ToString();
                                }
                                if (dsData.Tables[1].Rows.Count > 2)
                                {
                                    lblG2Flt3.Text = dsData.Tables[1].Rows[2][0].ToString();
                                    lblG2Wgt3.Text = dsData.Tables[1].Rows[2][1].ToString();
                                }
                                if (dsData.Tables[1].Rows.Count > 3)
                                {
                                    lblG2Flt4.Text = dsData.Tables[1].Rows[3][0].ToString();
                                    lblG2Wgt4.Text = dsData.Tables[1].Rows[3][1].ToString();
                                }
                                if (dsData.Tables[1].Rows.Count > 4)
                                {
                                    lblG2Flt5.Text = dsData.Tables[1].Rows[4][0].ToString();
                                    lblG2Wgt5.Text = dsData.Tables[1].Rows[4][1].ToString();
                                }
                            }
                            catch (Exception ex)
                            { }
                        }

                        if (dsData.Tables[2].Rows.Count > 0)
                        {

                            try
                            {
                                if (dsData.Tables[2].Rows.Count > 0)
                                {
                                    lblAgent1G1.Text = dsData.Tables[2].Rows[0][0].ToString();
                                    lblTopAgTon1.Text = dsData.Tables[2].Rows[0][1].ToString();
                                }
                                if (dsData.Tables[2].Rows.Count > 1)
                                {
                                    lblAgent2G1.Text = dsData.Tables[2].Rows[1][0].ToString();
                                    lblTopAgTon2.Text = dsData.Tables[2].Rows[1][1].ToString();
                                }
                                if (dsData.Tables[2].Rows.Count > 2)
                                {
                                    lblAgent3G1.Text = dsData.Tables[2].Rows[2][0].ToString();
                                    lblTopAgTon3.Text = dsData.Tables[2].Rows[2][1].ToString();
                                }
                                if (dsData.Tables[2].Rows.Count > 3)
                                {
                                    lblAgent4G1.Text = dsData.Tables[2].Rows[3][0].ToString();
                                    lblTopAgTon4.Text = dsData.Tables[2].Rows[3][1].ToString();
                                }
                                if (dsData.Tables[1].Rows.Count > 4)
                                {
                                    lblAgent5G1.Text = dsData.Tables[1].Rows[4][0].ToString();
                                    lblTopAgTon5.Text = dsData.Tables[1].Rows[4][1].ToString();
                                }
                            }
                            catch (Exception ex)
                            { }
                        }

                        if (dsData.Tables[3].Rows.Count > 0)
                        {

                            try
                            {
                                if (dsData.Tables[3].Rows.Count > 0)
                                {
                                    lblAgent1G2.Text = dsData.Tables[3].Rows[0][0].ToString();
                                    lblBotAgTon1.Text = dsData.Tables[3].Rows[0][1].ToString();
                                }
                                if (dsData.Tables[3].Rows.Count > 1)
                                {
                                    lblAgent2G2.Text = dsData.Tables[3].Rows[1][0].ToString();
                                    lblBotAgTon2.Text = dsData.Tables[3].Rows[1][1].ToString();
                                }
                                if (dsData.Tables[3].Rows.Count > 2)
                                {
                                    lblAgent3G2.Text = dsData.Tables[3].Rows[2][0].ToString();
                                    lblBotAgTon3.Text = dsData.Tables[3].Rows[2][1].ToString();
                                }
                                if (dsData.Tables[3].Rows.Count > 3)
                                {
                                    lblAgent4G2.Text = dsData.Tables[3].Rows[3][0].ToString();
                                    lblBotAgTon4.Text = dsData.Tables[3].Rows[3][1].ToString();
                                }
                                if (dsData.Tables[3].Rows.Count > 4)
                                {
                                    lblAgent5G2.Text = dsData.Tables[3].Rows[4][0].ToString();
                                    lblBotAgTon5.Text = dsData.Tables[3].Rows[4][1].ToString();
                                }
                            }
                            catch (Exception ex)
                            { }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "<SCRIPT LANGUAGE='javascript'>CallPopulateClick();</script>", false);
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        
    }
}