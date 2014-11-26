using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using Microsoft.Reporting.WebForms;
using System.Drawing;
using System.Data.SqlClient;
using QID.DataAccess;



namespace ProjectSmartCargoManager
{
    public partial class StockAllocation : System.Web.UI.Page
    {
        #region declare Variables
        StockAllocationBAL objBAL = new StockAllocationBAL();
        int RoleId;
        string Station;
        string ParentName = "", ParentType = "";
        string flag = "";
        DataSet dsflag = new DataSet();
        //  int HideGrd = 1;//1=Hide,0=Show
        #endregion

        #region Fill Ip Address & Update Time


        internal void CurrentIndiaTimings()
        {
            //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime dtIndianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);

            Session["IT"] = dtIndianTime;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string agentCode = Convert.ToString(Session["ACode"]);

                ScriptManager1.RegisterAsyncPostBackControl(ddlAWBtype);
                //mod 7 Jun 2013
                //ScriptManager1.RegisterAsyncPostBackControl(cbAWBTypeInt);
                //ScriptManager1.RegisterAsyncPostBackControl(cbAWBTypeDom);
                if (!IsPostBack)
                {
                    bool flagUser = false;
                    //ViewState["HideGrd"] = 1;
                    ClearSession();
                    FillSession();
                    lblAWBStatus.Text = string.Empty;
                    fillCnoteType();
                    fillStockAWBType();
                    //
                    if (RoleId == 8)
                    {
                        ddlTypeAllocation.Items[1].Enabled = false;
                        ddlTypeAllocation.Items[2].Enabled = false;
                        ddlTypeAllocation.Items[3].Enabled = false;
                    }
                    else
                    {
                        ddlTypeAllocation.Items[1].Enabled = true;
                        ddlTypeAllocation.Items[2].Enabled = true;
                        ddlTypeAllocation.Items[3].Enabled = true;
                    }

                    //
                    //mod 7 Jun 2013
                    // cbAWBTypeDom.Checked = true;
                    cbAWBTypeCheckedChanged(sender, e);
                    LoadGridAllocation();
                    if (agentCode != "")
                    {
                        FillGridagentDetails(agentCode);
                       // btnList_Click(sender, e);
                    }
                    //#region Block grid

                    //for (int i = 0; i < grdStockAllocation.Rows.Count; i++)
                    //{
                    //    ((TextBox)grdStockAllocation.Rows[i].FindControl("LEVEL")).Enabled = false;
                    //    ((TextBox)grdStockAllocation.Rows[i].FindControl("FROM")).Enabled = false;
                    //    ((TextBox)grdStockAllocation.Rows[i].FindControl("TO")).Enabled = false;
                    //    ((TextBox)grdStockAllocation.Rows[i].FindControl("STATUS")).Enabled = false;
                    //    ((TextBox)grdStockAllocation.Rows[i].FindControl("ALLOCATION TIME")).Enabled = false;

                    //    ((TextBox)grdStockAllocation.Rows[i].FindControl("ALLOCATED BY")).Enabled = false;
                    //    ((TextBox)grdStockAllocation.Rows[i].FindControl("AVAILABLE AWB")).Enabled = false;
                    //    ((TextBox)grdStockAllocation.Rows[i].FindControl("LAST ALLOCATED")).Enabled = false;

                    //    ((TextBox)grdStockAllocation.Rows[i].FindControl("CNOTE TYPE")).Enabled = false;
                    //    ((TextBox)grdStockAllocation.Rows[i].FindControl("AWB TYPE")).Enabled = false;
                       
                    //}
                    //#endregion BlockGrid

                    //---------------------------Sumit-----------------------------//
                    //flagUser = validateUser();
                    //if (flagUser == false)
                    //{
                    //    try
                    //    {
                    //        //string station = Session["Station"].ToString();
                    //        //ddlTypeAllocation.SelectedValue = "City";
                    //        //ddlCodeAllocation.Items.Add(station);
                    //        //ddlCodeAllocation.Text = station;

                    //    }
                    //    catch (Exception ex)
                    //    { }
                    //}
                    //---------------------------Sumit-----------------------------//
                }

                lblStatus.Text = "";
                #region Define PageSize for grid as per configuration
                try
                {
                    LoginBL objConfig = new LoginBL();
                    grdStockAllocation.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    grdStockAllocationChild.PageSize = Convert.ToInt32(objConfig.GetMasterConfiguration("StdListPageSize"));
                    objConfig = null;
                }
                catch (Exception ex)
                { }
                #endregion
            }
            catch (Exception)
            {
            }
        }

        private void fillCnoteType()
        {
            try
            {
                DataSet ds=objBAL.GetCnoteCode();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlCNType.DataSource = ds.Tables[0];
                    ddlCNType.DataValueField = "AWBCnoteMaster";
                    ddlCNType.DataTextField = "AWBCnoteMaster";
                    ddlCNType.DataBind();
                    ddlCNType.SelectedIndex = 0;

                    ddlCNTypeAllocate.DataSource = ds.Tables[0];
                    ddlCNTypeAllocate.DataValueField = "AWBCnoteMaster";
                    ddlCNTypeAllocate.DataTextField = "AWBCnoteMaster";
                    ddlCNTypeAllocate.DataBind();
                    ddlCNTypeAllocate.SelectedIndex = 0;

                }
            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void fillStockAWBType()
        {
            try
            {
                DataSet ds = objBAL.GetAWBtype();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlStockAWBtype.DataSource = ds.Tables[0];
                    ddlStockAWBtype.DataValueField = "awbcode";
                    ddlStockAWBtype.DataTextField = "awbtype";
                    ddlStockAWBtype.DataBind();
                    ddlStockAWBtype.Items.Insert(0, "Select");
                    ddlStockAWBtype.SelectedIndex = 0;


                    ddlStockAWBtypeAllocation.DataSource = ds.Tables[0];
                    ddlStockAWBtypeAllocation.DataValueField = "awbcode";
                    ddlStockAWBtypeAllocation.DataTextField = "awbtype";
                    ddlStockAWBtypeAllocation.DataBind();
                    //ddlStockAWBtypeAllocation.Items.Insert(0, "Select");
                    ddlStockAWBtypeAllocation.SelectedIndex = 0;



                }
            }
            catch (Exception ex)
            {


            }
        }

        private void FillGridagentDetails( string agentCode)
        {
            try
            {

            
            Clear();
            object[] AWBStock = new object[4];
            int i = 0;

            AWBStock.SetValue("Agent",i);
            i++;
            AWBStock.SetValue(agentCode, i);
            i++;

                //mod 7 Jun 2013
            //Check For Domestic or International
            //string allChecked = string.Empty;
            //if (cbAWBTypeInt.Checked == true)
            //    allChecked = "B";//mod previous "I"
            //if (cbAWBTypeDom.Checked == true)
            //    allChecked = "D";
            //if (cbAWBTypeInt.Checked == true && cbAWBTypeDom.Checked == true)
            //    allChecked = "I";//mod previous "B"
            //


            AWBStock.SetValue(ddlStockAWBtype.SelectedValue.ToString(), i);
            i++;
            AWBStock.SetValue("",i);
            DataSet AWB = objBAL.GetAWBStock(AWBStock);
            if(AWB.Tables[0].Rows.Count>=0 )
            {
                ddlType.Text = "Agent";
                ddlCode.SelectedItem.Text = agentCode;
                
                ddlTypeAllocation.Text = "Agent";
                ddlCodeAllocation.SelectedItem.Text = agentCode;

                ddlTypeHistory.Text = "Agent";
                ddlCodeHistory.SelectedItem.Text = agentCode;

                ddlAWBtype.Enabled = true;
                
                DataView objDv = new DataView(AWB.Tables[0]);
            objDv.Sort = "Afrom";
            grdStockAllocation.DataSource = objDv;
            grdStockAllocation.DataBind();

           Session["AllocationGridSort"] = AWB;
            }
            }
            catch (Exception ex)
            {

               
            }
            }

        private void ClearSession()
        {
            Session["HistoryAWB"] = "";
            Session["HistoryAWBsub"] = "";
        }
        
        #region Fill Session For DDL
        private void FillSession()
        {
            try
            {
               
                DataSet Region = objBAL.GetRegionCode();
                Session["Region"] = Region.Tables[0];
                  Station= Session["Station"].ToString();
                 RoleId =Convert.ToInt32(Session["RoleID"].ToString());
                if (RoleId ==7)
                {
                    RoleId = 1;
                }
                else if (RoleId != 8)
                {
                  RoleId=0;
                }
                DataSet City = objBAL.GetCityCodeStock(Station,RoleId);
               Session["City"]  = City.Tables[0];
                DataSet Agent = objBAL.GetAgentCode();
                Session["Agent"] = Agent.Tables[0];
                //mod
                 dsflag = objBAL.GetSubAgent();
                flag = dsflag.Tables[0].Rows[0]["Value"].ToString();
                if (flag == "true" || flag == "True" || flag == "TRUE")
                {
                    string CL = "";
                    if (Convert.ToString(ViewState["UserType"]) == "CL")
                    {
                        CL = Session["AgentCode"].ToString();
                    }
                    DataSet SubAgent = objBAL.GetFilteredAgentCode(CL);
                    Session["SubAgent"] = SubAgent.Tables[0];
                }
                else
                {
                    ddlCode.Items.Remove(ddlCode.Items.FindByText("SubAgent"));
                    
                }
                
                //Mod Vikas 29Apr2013
                if (RoleId == 8)
                {

                    //Filtering Agents According to Station
                    DataTable dView = (DataTable)Session["Agent"];
                    EnumerableRowCollection<DataRow> query = from  Cityi in dView.AsEnumerable()
                                                             where Cityi.Field<string>("City").Equals(Station)
                                                             select Cityi;
                                        
                   DataView view = query.AsDataView();
                   Session["Agent"]= view.ToTable();

                    //Filtering City According to Station
                   dView = (DataTable)Session["City"];
                   query = from Cityi in dView.AsEnumerable()
                           where Cityi.Field<string>("CityCode").Equals(Station)
                           select Cityi;
                   view = query.AsDataView();
                   Session["City"] = view.ToTable();









                }
            }
            catch (Exception)
            {
                
               
            }
        }
        #endregion

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session["Region"] == null || Session["City"] == null || Session["Agent"] == null)
                {
                    FillSession();
                }
                Clear();

              
                //clear grid view
                //grdStockAllocation.DataSource = "";
                //grdStockAllocation.DataBind();
                //grdStockAllocationChild.DataSource = "";
            
                //grdStockAllocationChild.DataBind();

                if (ddlType.SelectedItem.Value.ToString() == "Select")
                {
                    ddlCode.DataSource = "";
                    ddlCode.DataBind();
                    ddlCode.Items.Insert(0, "Select");
                }

                if (ddlType.SelectedItem.Value.ToString() == "HO")
                {
                    ddlCode.DataSource = "";
                    ddlCode.DataBind();
                    ddlCode.Items.Add("HO");
                    //ddlTypeAllocation.DataSource = "";
                    //ddlTypeAllocation.DataBind();
                    //ddlTypeAllocation.Items.Add("HO");
                    //ddlTypeHistory.DataSource = "";
                    //ddlTypeHistory.DataBind();
                    //ddlTypeHistory.Items.Add("HO");
                    ParentName = "";
                    ParentType = "";
                }


                if (ddlType.SelectedItem.Value.ToString() == "Region")
                {
                   // DataSet Region = objBAL.GetRegionCode();
                    if ((DataTable)Session["Region"] != null)
                    {
                        ddlCode.DataSource = (DataTable)Session["Region"];
                        ddlCode.DataTextField = "RegionCode";
                        ddlCode.DataValueField = "RegionCode";
                        ddlCode.DataBind();
                        ddlCode.Items.Insert(0, "Select");

                        //ddlTypeAllocation.DataSource = (DataTable)Session["Region"];
                        //ddlTypeAllocation.DataTextField = "RegionCode";
                        //ddlTypeAllocation.DataValueField = "RegionCode";
                        //ddlTypeAllocation.DataBind();
                        //ddlTypeAllocation.Items.Insert(0, "Select");
                        //ddlTypeHistory.DataSource = (DataTable)Session["Region"];
                        //ddlTypeHistory.DataTextField = "RegionCode";
                        //ddlTypeHistory.DataValueField = "RegionCode";
                        //ddlTypeHistory.DataBind();
                        //ddlTypeHistory.Items.Insert(0, "Select");
                        //ddlTypeHistory.SelectedIndex = 0;
                    }

                    //ddlTypeAllocation.DataSource = "";
                    //ddlTypeAllocation.DataBind();
                    //ddlTypeAllocation.Items.Add("Region");
                    //ddlTypeHistory.DataSource = "";
                    //ddlTypeHistory.DataBind();
                    //ddlTypeHistory.Items.Add("Region");

                   // ddlTypeAllocation.DataSource = "";

                }
                if (ddlType.SelectedItem.Value.ToString() == "City")
                {
                    //code modified by manjusha
                    HomeBL objBLL = new HomeBL();
                    string restStation = "false";

                    restStation = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "RestrictStationAccess");
                    //restStation = objBal.GetMasterConfiguration("RestrictStationAccess");
                    try
                    {
                        if (restStation == "true" && restStation != null && restStation != "")
                        {
                            object[] UserParams = new string[1];
                            int i = 0;
                            UserParams.SetValue(Session["UserName"].ToString(), i);

                            DataSet ds = new DataSet("MasterPage_getStationSA_ds");
                            ds = objBLL.GetUserRollDetails(UserParams);
                            if (ddlCode.DataSource == null || ddlCode.DataSource == "")
                            {
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    ddlCode.DataSource = null;
                                    ddlCode.DataSource = ds;
                                    ddlCode.DataMember = ds.Tables[2].TableName;
                                    ddlCode.DataValueField = ds.Tables[2].Columns[0].ColumnName;
                                    ddlCode.DataTextField = ds.Tables[2].Columns[0].ColumnName;
                                    ddlCode.DataBind();
                                    ddlCode.Items.Insert(0, "Select");
                                    ddlCode.SelectedIndex = 0;
                                    //ddlSource.Text = Session["Station"].ToString();

                                }
                            }
                        }
                        else
                        {

                            //DataSet City = objBAL.GetCityCodeStock();
                            if ((DataTable)Session["City"] != null)
                            {
                                ddlCode.DataSource = (DataTable)Session["City"];
                                ddlCode.DataTextField = "City";
                                ddlCode.DataValueField = "CityCode";
                                ddlCode.DataBind();
                                ddlCode.Items.Insert(0, "Select");
                                ddlCode.SelectedIndex = 0;

                            }
                            //ddlTypeAllocation.DataSource = "";
                            //ddlTypeAllocation.DataBind();
                            //ddlTypeAllocation.Items.Add("City");
                            //ddlTypeHistory.DataSource = "";
                            //ddlTypeHistory.DataBind();
                            //ddlTypeHistory.Items.Add("City");

                        }
                    }
                    catch (Exception ex)
                    { }
                }
                if (ddlType.SelectedItem.Value.ToString() == "Agent")
                {

                    //DataSet Agent = objBAL.GetAgentCode();
                    if ((DataTable)Session["Agent"] != null)
                    {
                        ddlCode.DataSource = (DataTable)Session["Agent"];
                        ddlCode.DataTextField = "AgentName";
                        ddlCode.DataValueField = "AgentCode";

                        ddlCode.DataBind();
                        ddlCode.Items.Insert(0, "Select");
                        ddlCode.SelectedIndex = 0;
                    }
                   
                }
                //mod 08Oct2013
                dsflag = objBAL.GetSubAgent();
                flag = dsflag.Tables[0].Rows[0]["Value"].ToString();
                if (flag == "true" || flag == "True" || flag == "TRUE")
                {
                    if (ddlType.SelectedItem.Value.ToString() == "SubAgent")
                    {
                        if ((DataTable)Session["SubAgent"] != null)
                        {
                            ddlCode.DataSource = (DataTable)Session["SubAgent"];
                            ddlCode.DataTextField = "AgentName";
                            ddlCode.DataValueField = "AgentCode";

                            ddlCode.DataBind();
                            ddlCode.Items.Insert(0, "Select");
                            ddlCode.SelectedIndex = 0;
                        }
                        //ddlTypeAllocation.DataSource = "";
                        //ddlTypeAllocation.DataBind();
                        //ddlTypeAllocation.Items.Add("SubAgent");
                        //ddlTypeHistory.DataSource = "";
                        //ddlTypeHistory.DataBind();
                        //ddlTypeHistory.Items.Add("SubAgent");
                    }
                }
                
               


            }
            catch (Exception)
            {


            }

        }

        protected void ddlType_SelectedIndexChangedOrg(object sender, EventArgs e)
        {
            try
            {

            
            Clear();
            //clear grid view
            grdStockAllocation.DataSource = "";

           grdStockAllocationChild.DataSource = "";
                grdStockAllocation.DataBind();
               grdStockAllocationChild.DataBind();

            if (ddlType.SelectedItem.Value.ToString() == "Select")
            {
                ddlCode.DataSource = "";
                ddlCode.DataBind();
            }

            if (ddlType.SelectedItem.Value.ToString() == "HO")
            {
                ddlCode.DataSource = "";
                ddlCode.DataBind();
                ddlCode.Items.Add("HO");
                ParentName = "";
                ParentType = "";
            }

       
            if (ddlType.SelectedItem.Value.ToString() == "Region")
            {
                DataSet Region = objBAL.GetRegionCode();
                ddlCode.DataSource = Region.Tables[0];
                ddlCode.DataTextField = "RegionCode";
                ddlCode.DataValueField = "RegionCode";
                ddlCode.DataBind();
                ddlCode.Items.Insert(0, "Select");
                ddlCode.SelectedIndex = 0;
              
               
            }
            if (ddlType.SelectedItem.Value.ToString() == "City")
            {
                string Station= Session["Station"].ToString();
                 RoleId =Convert.ToInt32(Session["RoleID"].ToString());
                if (RoleId == 7)
                {
                    RoleId = 1;
                }
                else if (RoleId != 8)
                {

                    RoleId = 0;
                }

                DataSet City = objBAL.GetCityCodeStock(Station,RoleId);
                ddlCode.DataSource = City.Tables[0];
                ddlCode.DataTextField = "City";
                ddlCode.DataValueField = "CityCode";
                ddlCode.DataBind();
                ddlCode.Items.Insert(0, "Select");
                ddlCode.SelectedIndex = 0;
            }
            if (ddlType.SelectedItem.Value.ToString() == "Agent")
            {
               
                DataSet Agent = objBAL.GetAgentCode();
                ddlCode.DataSource = Agent.Tables[0];
                ddlCode.DataTextField = "AgentName";
                ddlCode.DataValueField = "AgentCode";
               
                ddlCode.DataBind();
                ddlCode.Items.Insert(0, "Select");
                ddlCode.SelectedIndex = 0;
               
            }

            }
            catch (Exception)
            {

               
            }

        }

        void Clear()
        {
            txtAWBTo.Text = txtAWBFrom.Text = txtCount.Text =lblStatus.Text= "";
            lblChildLevel.Text = string.Empty;
            lblParentLevel.Text = string.Empty;
        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {

                Clear();
                //if (txtAWBNo.Text != string.Empty)
                //{
                //    if (txtAWBNo.Text.Length == 8)
                //    {

                //        if ((Convert.ToInt32(txtAWBNo.Text.Substring(0, 7)) % 7).ToString() != txtAWBNo.Text.Substring(7, 1))
                //        {
                //            lblStatus.Text = "Please enter valid AWB!";
                //            lblStatus.ForeColor = Color.Blue;
                //            return;
                //        }

                //    }
                //    else
                //    {
                //        lblStatus.Text = "Please enter valid AWB!";
                //        lblStatus.ForeColor = Color.Blue;
                //        return;
                //    }
                //}
              

                if (txtAWBNo.Text == string.Empty && (ddlType.SelectedItem.Text == "Select" || ddlCode.SelectedItem.Text == "Select"))
                {
                    lblStatus.Text = "Please enter mandatory fields marked with (*)!";
                    lblStatus.ForeColor = Color.Blue;
                    lblAWBStatus.Visible = false;
                    return;
                }

                if (txtAWBNo.Text != string.Empty)
                {

                    string awbstatus = objBAL.GetAWBStatus(txtAWBNo.Text);

                    if (awbstatus != null && awbstatus != "")
                    {
                        lblAWBStatus.Text = awbstatus;
                        lblAWBStatus.ForeColor = Color.Green;
                        lblAWBStatus.Visible = true;
                    }

                }
                else
                {
                    lblAWBStatus.Text = string.Empty;
                }

                grdStockAllocation.DataSource = null;
                grdStockAllocation.DataBind();

                grdStockAllocationChild.DataSource = null;
                grdStockAllocationChild.DataBind();

                object[] AWBStock = new object[4];
                int i = 0;

                AWBStock.SetValue(ddlType.SelectedItem.Value.ToString(), i);
                i++;
                AWBStock.SetValue(ddlCode.SelectedItem.Value.ToString(), i);
                i++;
                //mod 7 Jan 2013
                //Check For Domestic or International
                //string allChecked = string.Empty;
                //if (cbAWBTypeInt.Checked == true)
                //    allChecked = "B";//mod previous "I"
                //if (cbAWBTypeDom.Checked == true)
                //    allChecked = "D";
                //if (cbAWBTypeInt.Checked == true && cbAWBTypeDom.Checked == true)
                //    allChecked = "I";//mod previous "B"
                //
                AWBStock.SetValue(ddlStockAWBtype.SelectedValue.ToString(), i);
                i++;
                AWBStock.SetValue(txtAWBNo.Text.Trim(), i);

                DataSet AWB = objBAL.GetAWBStock(AWBStock);
                //
                DataView objDv = new DataView(AWB.Tables[0]);
                # region
                // if (ddlCNType.SelectedItem.Text.ToString() != "ALL")
                 //   objDv.RowFilter = "cntype = '" + ddlCNType.SelectedItem.Text.ToString() + "'";

                //if (ddlStockAWBtype.SelectedItem.Text.ToString() != "Select")
                //    objDv.RowFilter = "awbtype = '" + ddlAWBTypeList.SelectedValue.ToString() + "'AND (AWBstockType = '" + ddlStockAWBtype.SelectedItem.Text.ToString() + "')" + "' AND (cntype = '" + ddlCNType.SelectedItem.Text.ToString() + "')";
                // if (ddlAWBTypeList.SelectedItem.Text.ToString() != "All")
                //   objDv.RowFilter = "awbtype = '" + ddlAWBTypeList.SelectedValue.ToString() + "'";
                //objDv.RowFilter = "AWBstockType = '" + ddlStockAWBtype.SelectedItem.Text.ToString() + "' ";
                #endregion

                objDv.RowFilter = "cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";

                if (ddlStockAWBtype.SelectedItem.Text.ToString() != "Select" && ddlAWBTypeList.SelectedValue.ToString()!="All")
                {
                    objDv.RowFilter = "awbtype='" + ddlAWBTypeList.SelectedValue.ToString() + "' and AWBStockType='" + ddlStockAWBtype.SelectedItem.Text.ToString() + "' and cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";
                }
                if (ddlStockAWBtype.SelectedItem.Text.ToString() == "Select" && ddlAWBTypeList.SelectedValue.ToString() != "All")
                {
                    objDv.RowFilter = "awbtype='" + ddlAWBTypeList.SelectedValue.ToString() + "' and cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";
                }
                if (ddlStockAWBtype.SelectedItem.Text.ToString() != "Select" && ddlAWBTypeList.SelectedValue.ToString() == "All")
                {
                    objDv.RowFilter = "AWBStockType='" + ddlStockAWBtype.SelectedItem.Text.ToString() + "' and cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";
                }


                DataView objDv1 = new DataView();
                if (txtAWBNo.Text != string.Empty)
                {
                    string AWBNo = txtAWBNo.Text.Length > 7 ? txtAWBNo.Text.Trim().Substring(0, 7) : txtAWBNo.Text.Trim();
                    objDv.RowFilter = "Afrom<=" + AWBNo + " and Ato>=" + AWBNo;

                }
                if (objDv.Count == 0)
                {
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblAWBStatus.Visible = false;

                }

                objDv.Sort = "Afrom";

                grdStockAllocation.DataSource = objDv;
                grdStockAllocation.DataBind();
                lblParentLevel.Text = ddlType.SelectedItem.Text != "Select" ? ddlType.SelectedItem.Text.Trim() : string.Empty;

                Session["StockAllocationGrd"] = objDv;
                if (AWB.Tables.Count > 1)
                {
                    objDv = new DataView(AWB.Tables[1]);
                    
                    #region
                    //if (ddlCNType.SelectedItem.Text.ToString() != "ALL")
                    //    objDv.RowFilter = "cntype = '" + ddlCNType.SelectedItem.Text.ToString() + "'";
                    //if (ddlStockAWBtype.SelectedItem.Text.ToString() != "Select")
                    //    // objDv.RowFilter = "AWBstockType = '" + ddlStockAWBtype.SelectedItem.Text.ToString() + "'";
                    //    objDv.RowFilter = "AWBstockType = '" + ddlStockAWBtype.SelectedItem.Text.ToString() + "' AND (cntype = '" + ddlCNType.SelectedItem.Text.ToString() + "')";

                    //if (ddlAWBTypeList.SelectedItem.Text.ToString() != "All")
                    //    objDv.RowFilter = "awbtype = '" + ddlAWBTypeList.SelectedValue.ToString() + "'";
#endregion
                    
                    objDv.RowFilter = "cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";

                    if (ddlStockAWBtype.SelectedItem.Text.ToString() != "Select" && ddlAWBTypeList.SelectedValue.ToString() != "All")
                    {
                        objDv.RowFilter = "awbtype='" + ddlAWBTypeList.SelectedValue.ToString() + "' and AWBStockType='" + ddlStockAWBtype.SelectedItem.Text.ToString() + "' and cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";
                    }
                    if (ddlStockAWBtype.SelectedItem.Text.ToString() == "Select" && ddlAWBTypeList.SelectedValue.ToString() != "All")
                    {
                        objDv.RowFilter = "awbtype='" + ddlAWBTypeList.SelectedValue.ToString() + "' and cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";
                    }
                    if (ddlStockAWBtype.SelectedItem.Text.ToString() != "Select" && ddlAWBTypeList.SelectedValue.ToString() == "All")
                    {
                        objDv.RowFilter = "AWBStockType='" + ddlStockAWBtype.SelectedItem.Text.ToString() + "' and cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";
                    }



                    if (txtAWBNo.Text != string.Empty)
                    {
                        string AWBNo = txtAWBNo.Text.Length > 7 ? txtAWBNo.Text.Trim().Substring(0, 7) : txtAWBNo.Text.Trim();
                        objDv.RowFilter = "Afrom<=" + AWBNo + " and Ato>=" + AWBNo;

                    }

                    objDv.Sort = "Afrom";
                    grdStockAllocationChild.DataSource = objDv;
                    grdStockAllocationChild.DataBind();
                    if (objDv.Count > 0)
                    {
                        if (lblParentLevel.Text == "HO")
                        {
                            lblChildLevel.Text = "Region";
                        }
                        else
                            if (lblParentLevel.Text == "Region")
                            {
                                lblChildLevel.Text = "City";
                            }
                            else
                                if (lblParentLevel.Text == "City")
                                {
                                    lblChildLevel.Text = "Agent";
                                }
                               
                                  
                    }
                   
                            

                    Session["StockAllocationGrdChild"] = objDv;
                }


                //

                //grdStockAllocation.DataSource = AWB.Tables[0];
                //grdStockAllocation.DataBind();

                //grdStockAllocationChild.DataSource = AWB.Tables[1];
                //grdStockAllocationChild.DataBind();

                Session["AllocationGridSort"] = AWB;
                // grdStockAllocationChild_Show(sender,e); Show/Hide Grid Call

            }
            catch (Exception ex)
            {
            }
           
        }

        protected void btnAllocateAWB_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;

                //if (string.Equals(ddlTypeAllocation.SelectedItem.Text.ToString(), "Agent"))
                //    getNextAWB(); 

                if (int.Parse(txtAWBFrom.Text) > int.Parse(txtAWBTo.Text))
                {
                    lblStatus.Text = "Please Select Proper AWB Range";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                DataSet Parent;

                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "HO")
                {
                    ParentName = "";
                  ParentType = "";
                }
                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "Region")
                {
                    ParentName = "HO";
                    ParentType = "HO";
                }
                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "City")
                {
                    Station = Session["Station"].ToString();
                    RoleId = Convert.ToInt32(Session["RoleID"].ToString());
                    if (RoleId == 7)
                    {
                        RoleId = 1;
                    }
                    else if (RoleId != 8)
                    {
                        RoleId = 0;
                    }
                    Parent = objBAL.GetCityCodeStock(Station,RoleId);
                    ParentName = Parent.Tables[0].Rows[ddlCodeAllocation.SelectedIndex - 1]["Region"].ToString();
                    ParentType = "Region";

                }
                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "Agent")
                {
                    Parent = objBAL.GetAgentCode();
                    ParentName = Parent.Tables[0].Rows[ddlCodeAllocation.SelectedIndex - 1]["City"].ToString();
                   ParentType = "City";
                }
                
                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "SubAgent")
                {
                    Parent = objBAL.GetFilteredAgentCode("");
                    ParentName = Parent.Tables[0].Rows[ddlCodeAllocation.SelectedIndex - 1]["Agent"].ToString();
                    ParentType = "Agent";
                }
                //if (validateDdlSelection("allocate") == false)
                //{
                //    return;
                //}

                //Check for BG Account

                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "Agent")
                {
                    //string TranAccount = string.Empty, Remarks = string.Empty, TranType = string.Empty;
                    //decimal BGAmount = 0, BankGAmt = 0, ThrValue = 0;
                    //BookingBAL objBLL = new BookingBAL();
                    //bool ValidateBG = false;

                    //objBLL.GetAccountDetails(ddlCodeAllocation.SelectedItem.Value.ToString(), DateTime.Now.Date, ref TranAccount,
                    //    ref BGAmount, ref BankGAmt, ref ThrValue, ref ValidateBG);

                    //if (BGAmount < 1 && ValidateBG == true)
                    //{
                    //    lblStatus.ForeColor = System.Drawing.Color.Red;
                    //    lblStatus.Text = "Agent is not having sufficient balance.";
                    //    return;
                    //}
                }

                //End

                //Validate Consecutive Rane of AWB for region
                //if (ddlTypeAllocation.SelectedItem.Text.ToString() == "Region")
                //{
                //    string ConscAWB = string.Empty;
                //    ConscAWB = objBAL.GetConsecutiveStockAWB(int.Parse(txtAWBFrom.Text));

                //    if (ConscAWB != "valid" && ConscAWB !=null )
                //    {
                //        if (ConscAWB == "invalid")
                //        {
                //            lblStatus.ForeColor = System.Drawing.Color.Red;
                //            lblStatus.Text = "Stock Already allocated to Region";
                //            return;
                //        }
                //        if (ConscAWB != txtAWBFrom.Text)
                //        {
                //            lblStatus.ForeColor = System.Drawing.Color.Red;
                //            lblStatus.Text = "Please select AWB from " + ConscAWB;
                //            return;
                //        }
                //    }
                //}
                
                //End


                object[] AWB = new object[11];
                int i = 0;
                string exec = "";
                AWB.SetValue(ddlCodeAllocation.SelectedItem.Value.ToString(), i);
                i++;
                AWB.SetValue(txtAWBFrom.Text, i);
                i++;
                AWB.SetValue(txtAWBTo.Text, i);
                i++;
                AWB.SetValue(ParentName, i);
                i++;
                AWB.SetValue(ParentType, i);
                i++;
                AWB.SetValue((string)Session["UserName"], i);
                i++;
                CurrentIndiaTimings();
                AWB.SetValue((DateTime)Session["IT"], i);
                i++;
                AWB.SetValue(txtAWBFrType.Text.ToUpper(), i);
                i++;
               // AWB.SetValue(ddlAWBtype.Text.ToString(), i);
                //i++;
                if (ddlAWBtype.SelectedValue == "Electronic")
                {

                    AWB.SetValue("EAWB", i);
                    i++;
                }
                else if (ddlAWBtype.SelectedValue == "Physical")
                {

                    AWB.SetValue("PAWB", i);
                    i++;

                }
                AWB.SetValue(ddlCNTypeAllocate.Text.ToString(), i);
                i++;
                AWB.SetValue(ddlStockAWBtypeAllocation.SelectedItem.Text.ToString(), i);



                exec = objBAL.AllocateAWBStock(AWB);
                if (exec != "True")
                {
                    //Allocation fail
                    lblStatus.Text = exec;
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    

                }
                else
                {
                    
                    //AC-78 added by Sangram
                    #region for Master Audit Log
                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    int k=0;
                    object[] Params = new object[7];


                    //1

                    Params.SetValue("Stock Allocation", k);
                    k++;

                    //2
                    Params.SetValue(txtAWBFrom.Text+'-'+txtAWBTo.Text, k);
                    k++;

                    //3

                    Params.SetValue("Allocation", k);
                    k++;

                    //4
                    string StockHolderType = ddlTypeAllocation.SelectedItem.Value.ToString();
                    string StockHolderCode = ddlCodeAllocation.SelectedItem.Value.ToString();
                    Params.SetValue("Stock allocated to " + StockHolderType + '-' + StockHolderCode, k);
                    k++;


                    //5

                    Params.SetValue("", k);
                    k++;

                    //6

                    Params.SetValue(Session["UserName"], k);
                    k++;

                    //7
                    Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString(), k);
                    k++;


                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Params);
                    #endregion

                    ddlTypeAllocation.SelectedIndex = 0;

                    ddlCodeAllocation.DataSource = "";
                    ddlCodeAllocation.DataBind();
                    resetDdlIndex();

                    btnList_Click(sender,e);
                    lblStatus.Text ="Stock Allocated Successfully" ;
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    txtAWBTo.Text = txtAWBFrom.Text = txtCount.Text = ""; 
                    
                }
            }
            catch (Exception)
            {


            }
            
        }

        private void resetDdlIndex()
        {
            try
            {
                ddlCNTypeAllocate.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
              
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/StockAllocation.aspx", false);
            //try
            //{
            //    Clear();
            //    ClearSession(); 
            //    if (ddlType.SelectedItem.Value.ToString() == "HO")
            //    {
            //        ddlCode.DataSource = "";
            //        ddlCode.DataBind();
            //        ddlCode.Items.Add("Select");
            //        // ParentName = "";
            //        //ParentType = "";
            //    }
            //    if (ddlTypeAllocation.SelectedItem.Value.ToString() == "HO")
            //    {
            //        ddlCodeAllocation.DataSource = "";
            //        ddlCodeAllocation.DataBind();
            //        ddlCodeAllocation.Items.Add("Select");
            //       // ParentName = "";
            //        //ParentType = "";
            //    }
            //    ddlStockAWBtype.SelectedIndex = 0;
            //    ddlStockAWBtypeAllocation.SelectedIndex = 0;
            //    //cbAWBTypeDom.Checked = true;
            //    //cbAWBTypeInt.Checked = false;
            //    //txtAWBFrType.Text = txtAWBToType.Text = "";

            //    //3 Oct ddlAWBtype change
            //    ddlAWBtype.SelectedIndex = 0;
            //    ddlCNTypeAllocate.SelectedIndex = 0;

            //    ddlCNType.SelectedIndex = 0;
            //    ddlCode.SelectedIndex = 0;
            //    ddlType.SelectedIndex = 0;
            //    ddlCodeAllocation.SelectedIndex = 0;
            //    ddlTypeAllocation.SelectedIndex = 0;
            //    grdStockAllocation.DataSource = "";
            //    grdStockAllocationChild.DataSource = "";
            //    grdStockAllocation.DataBind();
            //    grdStockAllocationChild.DataBind();
            //    //mod 7 Jun 2013
               
            //    //
            //}
            //catch (Exception ex)
            //{
            //}

        }

        protected void btnBlackListAWB_Click(object sender, EventArgs e)
        {

         
            try
            {
                if (int.Parse(txtAWBFrom.Text) > int.Parse(txtAWBTo.Text))
                {
                    lblStatus.Text = "Please Select Proper AWB Range";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                DataSet Parent;

                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "HO")
                {
                    ParentName = "";
                  ParentType = "";
                }
                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "Region")
                {
                    ParentName = "HO";
                    ParentType = "HO";
                }
                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "City")
                {
                    Station = Session["Station"].ToString();
                    RoleId = Convert.ToInt32(Session["RoleID"].ToString());
                    if (RoleId == 7)
                    {
                        RoleId = 1;
                    }
                    else if (RoleId != 8)
                    {
                        RoleId = 0;
                    }
                    Parent = objBAL.GetCityCodeStock(Station,RoleId);
                    ParentName = Parent.Tables[0].Rows[ddlCodeAllocation.SelectedIndex - 1]["Region"].ToString();
                    ParentType = "Region";

                }
                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "Agent")
                {
                    Parent = objBAL.GetAgentCode();
                    ParentName = Parent.Tables[0].Rows[ddlCodeAllocation.SelectedIndex - 1]["City"].ToString();
                   ParentType = "City";
                }
                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "SubAgent")
                {
                    Parent = objBAL.GetFilteredAgentCode("");
                    ParentName = Parent.Tables[0].Rows[ddlCodeAllocation.SelectedIndex - 1]["Agent"].ToString();
                    ParentType = "Agent";
                }

                object[] AWB = new object[8];
                int i = 0;
                string exec = "";
                AWB.SetValue(ddlCodeAllocation.SelectedItem.Value.ToString(), i);
                i++;
                AWB.SetValue(txtAWBFrom.Text, i);
                i++;
                AWB.SetValue(txtAWBTo.Text, i);
                i++;
                AWB.SetValue(ParentName, i);
                i++;
                AWB.SetValue(ParentType, i);
                i++;
                //(string)Session["AgentCode"]
                AWB.SetValue((string)Session["UserName"], i);
                i++;
                AWB.SetValue(txtAWBFrType.Text.ToUpper(), i);
                i++;
                AWB.SetValue(ddlCNTypeAllocate.SelectedItem.Value.ToString(), i);
               
                exec = objBAL.BlacklistAWBStock(AWB);
                if (exec != "True")
                {
                    //Allocation fail
                    lblStatus.Text = exec;
                    lblStatus.ForeColor = System.Drawing.Color.Red;

                }
                else
                {

                    //AC-78 added by Sangram
                    #region for Master Audit Log
                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    int k = 0;
                    object[] Params = new object[7];


                    //1

                    Params.SetValue("Stock Allocation", k);
                    k++;

                    //2
                    Params.SetValue(txtAWBFrom.Text + '-' + txtAWBTo.Text, k);
                    k++;

                    //3

                    Params.SetValue("Black list", k);
                    k++;

                    //4
                    string StockHolderType = ddlTypeAllocation.SelectedItem.Value.ToString();
                    string StockHolderCode = ddlCodeAllocation.SelectedItem.Value.ToString();
                    Params.SetValue("Stock BlackListed " + StockHolderType + '-' + StockHolderCode, k);
                    k++;


                    //5

                    Params.SetValue("", k);
                    k++;

                    //6

                    Params.SetValue(Session["UserName"], k);
                    k++;

                    //7
                    Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString(), k);
                    k++;


                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Params);
                    #endregion
                    
                    ddlTypeAllocation.SelectedIndex = 0;

                    ddlCodeAllocation.DataSource = "";
                    ddlCodeAllocation.DataBind();
                    resetDdlIndex();

                    
                    btnList_Click(sender,e);
                    lblStatus.Text ="Stock Blacklisted Successfully" ;
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    txtAWBTo.Text = txtAWBFrom.Text = txtCount.Text = ""; 
                    
                }
            }
            catch (Exception)
            {


            }
            
       
        }

        protected void ddlTypeAllocation_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                ddlStockAWBtypeAllocation.Enabled = true;
            
            Clear();
            //clear grid view


            if (ddlTypeAllocation.SelectedItem.Value.ToString() == "Select")
            {
                ddlCodeAllocation.DataSource = "";
                ddlCodeAllocation.DataBind();
                return;
            }

            if (ddlTypeAllocation.SelectedItem.Value.ToString() == "HO")
            {
                ddlCodeAllocation.DataSource = "";
                ddlCodeAllocation.DataBind();
                ddlCodeAllocation.Items.Add("HO");
                ParentName = "";
                ParentType = "";

                //3 Oct EnableDisable ddlAWBtype
                ddlAWBtype.Enabled = true;
                //
                return;
            }


            if (ddlTypeAllocation.SelectedItem.Value.ToString() == "Region")
            {
                DataSet Region = objBAL.GetRegionCode();
                ddlCodeAllocation.DataSource = Region.Tables[0];
                ddlCodeAllocation.DataTextField = "RegionCode";
                ddlCodeAllocation.DataValueField = "RegionCode";
                ddlCodeAllocation.DataBind();
                ddlCodeAllocation.Items.Insert(0, "Select");
                ddlCodeAllocation.SelectedIndex = 0;
                //3 Oct EnableDisable ddlAWBtype
                ddlAWBtype.Enabled = false;
                //
                return;

            }
            if (ddlTypeAllocation.SelectedItem.Value.ToString() == "City")
            {
                //code modified by manjusha
                HomeBL objBLL = new HomeBL();
                string restStation = "false";

                restStation = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "RestrictStationAccess");
                //restStation = objBal.GetMasterConfiguration("RestrictStationAccess");
                try
                {
                    if (restStation == "true" && restStation != null && restStation != "")
                    {
                        object[] UserParams = new string[1];
                        int i = 0;
                        UserParams.SetValue(Session["UserName"].ToString(), i);

                        DataSet ds = new DataSet("MasterPage_getStationSAA_ds");
                        ds = objBLL.GetUserRollDetails(UserParams);
                        if (ddlCodeAllocation.DataSource == null || ddlCodeAllocation.DataSource == "")
                        {
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                ddlCodeAllocation.DataSource = null;
                                ddlCodeAllocation.DataSource = ds;
                                ddlCodeAllocation.DataMember = ds.Tables[2].TableName;
                                ddlCodeAllocation.DataValueField = ds.Tables[2].Columns[0].ColumnName;
                                ddlCodeAllocation.DataTextField = ds.Tables[2].Columns[0].ColumnName;
                                ddlCodeAllocation.DataBind();
                                ddlCodeAllocation.Items.Insert(0, "Select");
                                ddlCodeAllocation.SelectedIndex = 0;
                                //ddlSource.Text = Session["Station"].ToString();
                                //3 Oct EnableDisable ddlAWBtype
                                ddlAWBtype.Enabled = false;
                                //
                                return;
                            }
                        }
                    }

                    else
                    {
                        Station = Session["Station"].ToString();
                        RoleId = Convert.ToInt32(Session["RoleID"].ToString());
                        if (RoleId == 7)
                        {
                            RoleId = 1;
                        }
                        else if (RoleId != 8)
                        {
                            RoleId = 0;
                        }
                        DataSet City = objBAL.GetCityCodeStock(Station, RoleId);
                        ddlCodeAllocation.DataSource = City.Tables[0];
                        ddlCodeAllocation.DataTextField = "City";
                        ddlCodeAllocation.DataValueField = "CityCode";
                        ddlCodeAllocation.DataBind();
                        ddlCodeAllocation.Items.Insert(0, "Select");
                        ddlCodeAllocation.SelectedIndex = 0;

                        //3 Oct EnableDisable ddlAWBtype
                        ddlAWBtype.Enabled = false;
                        //
                        return;
                    }
                }
                catch (Exception ex)
                { }
            }
            if (ddlTypeAllocation.SelectedItem.Value.ToString() == "Agent")
            {

                //DataSet Agent = objBAL.GetAgentCode();
                //ddlCodeAllocation.DataSource = Agent.Tables[0];
                ddlCodeAllocation.DataSource = (DataTable)Session["Agent"];
                ddlCodeAllocation.DataTextField = "AgentName";
                ddlCodeAllocation.DataValueField = "AgentCode";

                ddlCodeAllocation.DataBind();
                ddlCodeAllocation.Items.Insert(0, "Select");
                ddlCodeAllocation.SelectedIndex = 0;

                //3 Oct EnableDisable ddlAWBtype
                ddlAWBtype.Enabled = false;
                //
                return;
            }

            //mod 30May
            if (ddlTypeAllocation.SelectedItem.Value.ToString() == "SubAgent")
            {
                //mod 08Oct2013
                dsflag = objBAL.GetSubAgent();
                flag = dsflag.Tables[0].Rows[0]["Value"].ToString();
                if (flag == "true" || flag == "True" || flag == "TRUE")
                {
                    string CL = "";
                    if (Convert.ToString(ViewState["UserType"]) == "CL")
                    {
                        CL = Session["AgentCode"].ToString();
                    }

                    DataSet Agent = objBAL.GetFilteredAgentCode(CL);
                    ddlCodeAllocation.DataSource = Agent.Tables[0];
                    ddlCodeAllocation.DataTextField = "AgentName";
                    ddlCodeAllocation.DataValueField = "AgentCode";

                    ddlCodeAllocation.DataBind();
                    ddlCodeAllocation.Items.Insert(0, "Select");
                    ddlCodeAllocation.SelectedIndex = 0;

                    //3 Oct EnableDisable ddlAWBtype
                    ddlAWBtype.Enabled = false;
                    //
                    return;
                }
            }

            }
            catch (Exception)
            {

                
            }

        }

        #region Function List AWB
        protected void btnListHistory_Click(object sender, EventArgs e)
        {
            try
            {
              //add clear
                object[] AWBStock = new object[7];
                int i = 0;

                AWBStock.SetValue(ddlTypeHistory.SelectedItem.Value.ToString(), i);
                i++;
                AWBStock.SetValue(ddlCodeHistory.SelectedItem.Value.ToString(), i);
                i++;
                 string sFrom="";
                if (txtDateFrom.Text != "")
                {
                    DateTime dtFrom = DateTime.ParseExact(txtDateFrom.Text.Trim(), "dd/MM/yyyy", null);
                    sFrom  = dtFrom.ToShortDateString();
                }
                else { sFrom = ""; }
                AWBStock.SetValue(sFrom, i);
                i++;
            string sTo="";
            if (txtDateTo.Text != "")
            {
        DateTime dtTo = DateTime.ParseExact(txtDateTo.Text.Trim(), "dd/MM/yyyy", null);
            sTo = dtTo.ToShortDateString();
 }
    else { sTo = ""; }
                    AWBStock.SetValue(sTo, i);
                  string AWBNo=txtAWBNumber.Text.Trim(); 
                
                //if(AWBNo!="")
                //{
                    if (AWBNo.Length > 7)
                    {
                        AWBStock.SetValue(AWBNo.Substring(0,7), ++i);
                    }
                    else
                    {
                        AWBStock.SetValue("", ++i);
                        //lblStatus.Text = "Please enter Valid 8 digit AWB Number";
                        //lblStatus.ForeColor = Color.Red;
                        //return;
                    }
                //}
                string AWBFrom = txtFromAWB.Text.Trim();
                string AWBTo = txtToAWB.Text.Trim();

                if (AWBFrom.Length >= 7 && AWBTo.Length >= 7)
                {
                    AWBStock.SetValue(AWBFrom, ++i);
                    AWBStock.SetValue(AWBTo, ++i);

                }
                else
                {
                    AWBStock.SetValue("", ++i);
                    AWBStock.SetValue("", ++i);
                }

                DataSet AWB = objBAL.GetAWBStockHistory(AWBStock);

                GridViewHistory.DataSource = AWB.Tables[0];
                GridViewHistory.DataBind();

                AddtoHiddenField(AWBStock);//
                Session["HistoryAWB"] = AWB.Tables[0];

                //Table With Header Row for report
                DataTable dtSubReport = new DataTable();
                dtSubReport.Columns.Add("AType");
                dtSubReport.Columns.Add("ACode");
                dtSubReport.Columns.Add("ADateFrom");
                dtSubReport.Columns.Add("ADateTo");
                dtSubReport.Rows.Add(ddlTypeHistory.SelectedItem.Value,ddlCodeHistory.SelectedItem.Value,txtDateFrom.Text,txtDateTo.Text);
                Session["HistoryAWBsub"] =dtSubReport;
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Function Formatting Print(JScript)
        void AddtoHiddenField(object[] AWB)
        {
            for (int i = 0; i < AWB.Length; i++)
            {
                HtmlInputHidden hiddenField = new HtmlInputHidden();
                hiddenField.ID = "hf"+i;
                hiddenField.Value = AWB[i].ToString();

                this.Controls.Add(hiddenField);
            }
        }
        #endregion

        #region Function OnIndexChange StockAllocation Type
        protected void ddlTypeHistory_SelectedIndexChanged(object sender, EventArgs e)
        {


            try
            {
                GridViewHistory.DataSource = "";
                GridViewHistory.DataBind();
                // clear
            }
            catch (Exception ex)
            {
                
                
            }
            
            
            if (ddlTypeHistory.SelectedItem.Value.ToString() == "Select")
            {
                ddlCodeHistory.DataSource = "";
                ddlCodeHistory.DataBind();
            }

            if (ddlTypeHistory.SelectedItem.Value.ToString() == "HO")
            {
                ddlCodeHistory.DataSource = "";
                ddlCodeHistory.DataBind();
                ddlCodeHistory.Items.Add("HO");
                ParentName = "";
                ParentType = "";
            }


            if (ddlTypeHistory.SelectedItem.Value.ToString() == "Region")
            {
                DataSet Region = objBAL.GetRegionCode();
                ddlCodeHistory.DataSource = Region.Tables[0];
                ddlCodeHistory.DataTextField = "RegionCode";
                ddlCodeHistory.DataValueField = "RegionCode";
                ddlCodeHistory.DataBind();
                ddlCodeHistory.Items.Insert(0, "Select");
                ddlCodeHistory.SelectedIndex = 0;


            }
            if (ddlTypeHistory.SelectedItem.Value.ToString() == "City")
            {
                //code modified by manjusha
                HomeBL objBLL = new HomeBL();
                string restStation = "false";

                restStation = CommonUtility.GetConfigurationValue(Convert.ToString(Session["ConfigXML"]), "RestrictStationAccess");
                //restStation = objBal.GetMasterConfiguration("RestrictStationAccess");
                try
                {
                    if (restStation == "true" && restStation != null && restStation != "")
                    {
                        object[] UserParams = new string[1];
                        int i = 0;
                        UserParams.SetValue(Session["UserName"].ToString(), i);

                        DataSet ds = new DataSet("MasterPage_getStationSAH_ds");
                        ds = objBLL.GetUserRollDetails(UserParams);
                        if (ddlCodeHistory.DataSource == null || ddlCodeHistory.DataSource == "")
                        {
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                ddlCodeHistory.DataSource = null;
                                ddlCodeHistory.DataSource = ds;
                                ddlCodeHistory.DataMember = ds.Tables[2].TableName;
                                ddlCodeHistory.DataValueField = ds.Tables[2].Columns[0].ColumnName;
                                ddlCodeHistory.DataTextField = ds.Tables[2].Columns[0].ColumnName;
                                ddlCodeHistory.DataBind();
                                ddlCodeHistory.Items.Insert(0, "Select");
                                ddlCodeHistory.SelectedIndex = 0;
                                //ddlSource.Text = Session["Station"].ToString();

                            }
                        }
                    }

                    else
                    {
                        Station = Session["Station"].ToString();
                        RoleId = Convert.ToInt32(Session["RoleID"].ToString());
                        if (RoleId == 7)
                        {
                            RoleId = 1;
                        }
                        else if (RoleId != 8)
                        {
                            RoleId = 0;
                        }
                        DataSet City = objBAL.GetCityCodeStock(Station, RoleId);
                        ddlCodeHistory.DataSource = City.Tables[0];
                        ddlCodeHistory.DataTextField = "City";
                        ddlCodeHistory.DataValueField = "CityCode";
                        ddlCodeHistory.DataBind();
                        ddlCodeHistory.Items.Insert(0, "Select");
                        ddlCodeHistory.SelectedIndex = 0;
                    }
                }
             catch(Exception ex)
            {}
            }
           
       
            if (ddlTypeHistory.SelectedItem.Value.ToString() == "Agent")
            {

                DataSet Agent = objBAL.GetAgentCode();
                ddlCodeHistory.DataSource = Agent.Tables[0];
                ddlCodeHistory.DataTextField = "AgentName";
                ddlCodeHistory.DataValueField = "AgentCode";

                ddlCodeHistory.DataBind();
                ddlCodeHistory.Items.Insert(0, "Select");
                ddlCodeHistory.SelectedIndex = 0;

            }
            //mod 08Oct2013
            dsflag = objBAL.GetSubAgent();
            flag = dsflag.Tables[0].Rows[0]["Value"].ToString();
            if (flag == "true" || flag == "True" || flag == "TRUE")
            {
                if (ddlTypeHistory.SelectedItem.Value.ToString() == "SubAgent")
                {
                    if ((DataTable)Session["SubAgent"] != null)
                    {
                        ddlCodeHistory.DataSource = (DataTable)Session["SubAgent"];
                        ddlCodeHistory.DataTextField = "AgentName";
                        ddlCodeHistory.DataValueField = "AgentCode";

                        ddlCodeHistory.DataBind();
                        ddlCodeHistory.Items.Insert(0, "Select");
                        ddlCodeHistory.SelectedIndex = 0;
                    }
                }
            }



        }
        #endregion

        protected void btnClearHistory_Click(object sender, EventArgs e)
        {
            try
            {
                //Clear();

                if (ddlTypeHistory.SelectedItem.Value.ToString() == "HO")
                {
                    ddlCodeHistory.DataSource = "";
                    ddlCodeHistory.DataBind();
                    ddlCodeHistory.Items.Add("Select");
                    // ParentName = "";
                    //ParentType = "";
                }
                ddlCodeHistory.SelectedIndex = 0;
                ddlTypeHistory.SelectedIndex = 0;
                GridViewHistory.DataSource = "";
                GridViewHistory.DataBind();
                txtDateTo.Text = txtDateFrom.Text = "";
                ClearSession();
            }
            catch (Exception ex)
            {
            }
        }
       
        #region Function Return AWB
        protected void btnReturnAWB_Click(object sender, EventArgs e)
        {

            try
            {
                if (int.Parse(txtAWBFrom.Text) > int.Parse(txtAWBTo.Text))
                {
                    lblStatus.Text = "Please Select Proper AWB Range";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                DataSet Parent;

                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "HO")
                {
                    lblStatus.Text = "Cannot return from HO ";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
           else
                {
                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "Region")
                {
                    ParentName = "HO";
                    ParentType = "HO";
                }
                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "City")
                {
                    Station = Session["Station"].ToString();
                    RoleId = Convert.ToInt32(Session["RoleID"].ToString());
                    if (RoleId == 7)
                    {
                        RoleId = 1;
                    }
                    else if (RoleId != 8)
                    {
                        RoleId = 0;
                    }

                    Parent = objBAL.GetCityCodeStock(Station,RoleId);
                    ParentName = Parent.Tables[0].Rows[ddlCodeAllocation.SelectedIndex - 1]["Region"].ToString();
                    ParentType = "Region";

                }
                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "Agent")
                {
                    Parent = objBAL.GetAgentCode();
                    ParentName = Parent.Tables[0].Rows[ddlCodeAllocation.SelectedIndex - 1]["City"].ToString();
                    ParentType = "City";
                }

                object[] AWB = new object[7];
                int i = 0;
                string exec = "";
                AWB.SetValue(ddlCodeAllocation.SelectedItem.Value.ToString(), i);
                i++;
                AWB.SetValue(txtAWBFrom.Text, i);
                i++;
                AWB.SetValue(txtAWBTo.Text, i);
                i++;
                AWB.SetValue(ParentName, i);
                i++;
                AWB.SetValue(ParentType, i);
                i++;
                //(string)Session["AgentCode"]
                AWB.SetValue((string)Session["UserName"], i);
                i++;
                AWB.SetValue(txtAWBFrType.Text.ToUpper(), i);
               
                    exec = objBAL.ReturnAWBStock(AWB);
                if (exec != "True")
                {
                    //Allocation fail
                    lblStatus.Text = exec;
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
                else
                {

                    //AC-78 added by Sangram
                    #region for Master Audit Log
                    MasterAuditBAL ObjMAL = new MasterAuditBAL();
                    #region Prepare Parameters
                    int k = 0;
                    object[] Params = new object[7];


                    //1

                    Params.SetValue("Stock Allocation", k);
                    k++;

                    //2
                    Params.SetValue(txtAWBFrom.Text + '-' + txtAWBTo.Text, k);
                    k++;

                    //3

                    Params.SetValue("Stock Return", k);
                    k++;

                    //4
                    string StockHolderType = ddlTypeAllocation.SelectedItem.Value.ToString();
                    string StockHolderCode = ddlCodeAllocation.SelectedItem.Value.ToString();
                    Params.SetValue("Stock Returned from " + StockHolderType + '-' + StockHolderCode, k);
                    k++;


                    //5

                    Params.SetValue("", k);
                    k++;

                    //6

                    Params.SetValue(Session["UserName"], k);
                    k++;

                    //7
                    Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString(), k);
                    k++;


                    #endregion Prepare Parameters
                    ObjMAL.AddMasterAuditLog(Params);
                    #endregion

                    ddlTypeAllocation.SelectedIndex = 0;

                    ddlCodeAllocation.DataSource = "";
                    ddlCodeAllocation.DataBind();
                    resetDdlIndex();


                    btnList_Click(sender, e);
                    lblStatus.Text = "Stock Returned Successfully";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    txtAWBTo.Text = txtAWBFrom.Text = txtCount.Text = ""; 
                }
               }
            }
            catch (Exception)
            {


            }

        }
#endregion

        #region Function Revoke
        protected void btnRevokeAWB_Click(object sender, EventArgs e)
        {

            try
            {
                if (int.Parse(txtAWBFrom.Text) > int.Parse(txtAWBTo.Text))
                {
                    lblStatus.Text = "Please Select Proper AWB Range";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                DataSet Parent;

                if (ddlTypeAllocation.SelectedItem.Text.ToString() == "HO")
                {
                    lblStatus.Text = "Cannot return from HO ";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
                 if (ddlTypeAllocation.SelectedItem.Text.ToString() == "Region")
                    {
                        ParentName = "HO";
                        ParentType = "HO";
                    }
                    if (ddlTypeAllocation.SelectedItem.Text.ToString() == "City")
                    {
                        Station = Session["Station"].ToString();
                        RoleId = Convert.ToInt32(Session["RoleID"].ToString());
                        if (RoleId == 7)
                        {
                            RoleId = 1;
                        }
                        else  if (RoleId != 8)
                        {
                            RoleId = 0;
                        }
                        Parent = objBAL.GetCityCodeStock(Station,RoleId);
                        ParentName = Parent.Tables[0].Rows[ddlCodeAllocation.SelectedIndex - 1]["Region"].ToString();
                        ParentType = "Region";

                    }
                    if (ddlTypeAllocation.SelectedItem.Text.ToString() == "Agent")
                    {
                        Parent = objBAL.GetAgentCode();
                        ParentName = Parent.Tables[0].Rows[ddlCodeAllocation.SelectedIndex - 1]["City"].ToString();
                        ParentType = "City";
                    }
                    if (ddlTypeAllocation.SelectedItem.Text.ToString() == "SubAgent")
                    {
                        Parent = objBAL.GetFilteredAgentCode("");
                        ParentName = Parent.Tables[0].Rows[ddlCodeAllocation.SelectedIndex - 1]["Agent"].ToString();
                        ParentType = "Agent";
                    }


                    object[] AWB = new object[9];
                    int i = 0;
                    string exec = "";
                    AWB.SetValue(ddlCodeAllocation.SelectedItem.Value.ToString(), i);
                    i++;
                    AWB.SetValue(txtAWBFrom.Text, i);
                    i++;
                    AWB.SetValue(txtAWBTo.Text, i);
                    i++;
                    AWB.SetValue(ParentName, i);
                    i++;
                    AWB.SetValue(ParentType, i);
                    i++;
                    //(string)Session["AgentCode"]
                    AWB.SetValue((string)Session["UserName"], i);
                    i++;
                    AWB.SetValue(txtAWBFrType.Text.ToUpper(), i);
                    i++;
                    AWB.SetValue(ddlStockAWBtypeAllocation.Text.Trim(), i);
                    i++;
                    AWB.SetValue(ddlCNTypeAllocate.SelectedItem.Value.ToString(), i);
               
                    exec = objBAL.RevokeAWBStock(AWB);
                    if (exec != "True")
                    {
                        //Allocation fail
                        lblStatus.Text = exec;
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {

                        //AC-78 added by Sangram
                        #region for Master Audit Log
                        MasterAuditBAL ObjMAL = new MasterAuditBAL();
                        #region Prepare Parameters
                        int k = 0;
                        object[] Params = new object[7];


                        //1

                        Params.SetValue("Stock Allocation", k);
                        k++;

                        //2
                        Params.SetValue(txtAWBFrom.Text + '-' + txtAWBTo.Text, k);
                        k++;

                        //3

                        Params.SetValue("Revoke", k);
                        k++;

                        //4
                        string StockHolderType = ddlTypeAllocation.SelectedItem.Value.ToString();
                        string StockHolderCode = ddlCodeAllocation.SelectedItem.Value.ToString();
                        Params.SetValue("Stock revoked " + StockHolderType + '-' + StockHolderCode, k);
                        k++;


                        //5

                        Params.SetValue("", k);
                        k++;

                        //6

                        Params.SetValue(Session["UserName"], k);
                        k++;

                        //7
                        Params.SetValue(DateTime.Parse(Session["IT"].ToString()).ToString(), k);
                        k++;


                        #endregion Prepare Parameters
                        ObjMAL.AddMasterAuditLog(Params);
                        #endregion
                        
                        ddlTypeAllocation.SelectedIndex = 0;

                        ddlCodeAllocation.DataSource = "";
                        ddlCodeAllocation.DataBind();
                        resetDdlIndex();


                        btnList_Click(sender, e);
                        lblStatus.Text = "Stock Revoked Successfully";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        txtAWBTo.Text = txtAWBFrom.Text = txtCount.Text = ""; 
                    }
                }
          
            catch (Exception)
            {


            }

        }
        #endregion

        # region StockAllocationChild DrillDown
        protected void grdStockAllocationChild_Show(object sender, EventArgs e)
        {
            if ((int)ViewState["HideGrd"] == 1)//Hide
            {
                if (grdStockAllocationChild.Rows.Count > 0)
                    for (int i = 0; i < grdStockAllocationChild.Rows.Count; i++)
                    {
                        grdStockAllocationChild.Rows[i].Visible = false;
                    }
             string url = "~/Images/plus.gif";

                ((ImageButton)grdStockAllocationChild.HeaderRow.FindControl("grdStockAllocationChildShowHide")).ImageUrl = url;
             
                ViewState["HideGrd"] = 0;
                return;    
            }
            if ((int)ViewState["HideGrd"] == 0)
            {
                if (grdStockAllocationChild.Rows.Count > 0)
                    for (int i = 0; i < grdStockAllocationChild.Rows.Count; i++)
                    {
                        grdStockAllocationChild.Rows[i].Visible = true;
                    }
                string url = "~/Images/minus.gif";
               ((ImageButton)grdStockAllocationChild.HeaderRow.FindControl("grdStockAllocationChildShowHide")).ImageUrl = url;
                
                ViewState["HideGrd"] = 1;
                return; 
            }   
       
           
        }
#endregion

        protected void GenerateHistoryReport(object sender, EventArgs e)
        {
            var HistoryAWB =Session["HistoryAWB"]as DataTable;
            var HistoryAWBsub = Session["HistoryAWBsub"] as DataTable;
            if (HistoryAWB!=null && HistoryAWBsub!=null)
           {
            if (HistoryAWB.Rows.Count > 0 && HistoryAWBsub.Rows.Count > 0)
            {
               lblStatus.Text="";
              // Response.Redirect("showAWBHistory.aspx");
               //string Url = "showAWB.aspx?fromAWB=" + fromAWB.Substring(fromAWB.Length - 7, 7) + "&toAWB=" + toAWB.Substring(toAWB.Length - 7, 7);
               ScriptManager.RegisterClientScriptBlock(this, GetType(), "newpage", "customOpen('showAWBHistory.aspx');", true);

               Response.Write("<script>");
               Response.Write("window.open('showAWBHistory.aspx','_blank', 'left=0,top=0,width=*,height=*,toolbar=0,scrollbars=1,status=0')");
               Response.Write("</script>");

            }
           }
        }

        protected void cbAWBTypeCheckedChanged(object sender, EventArgs e)
        {
            string preAWB = string.Empty;
            if (Session["awbPrefix"] != null)
            {
                preAWB = Session["awbPrefix"].ToString();

            }
            else
            {
                MasterBAL objBal = new MasterBAL();
                Session["awbPrefix"] = objBal.awbPrefix();
                preAWB = Session["awbPrefix"].ToString();
            }
           
            //mod 7 Jun 2013

            //if(ddlStockAWBtype.SelectedValue.ToString()=="D") --decides AWB prefix in textbox
                txtAWBFrType.Text = txtAWBToType.Text = preAWB;

            //if (cbAWBTypeDom.Checked == true && cbAWBTypeInt.Checked==false)
            //    txtAWBFrType.Text = txtAWBToType.Text = preAWB;
            //if (cbAWBTypeInt.Checked == true &&cbAWBTypeDom.Checked ==false )
            //    txtAWBFrType.Text = txtAWBToType.Text = "775";
            // if (cbAWBTypeInt.Checked == false &&cbAWBTypeDom.Checked ==false )
            //     txtAWBFrType.Text = txtAWBToType.Text = "";
        }

        protected bool validateDdlSelection(string value)
        {

            if (ddlType.Text == "HO")
            {
                if (ddlTypeAllocation.Text == "City" || ddlTypeAllocation.Text == "Agent")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Cannot " + value + " " + ddlTypeAllocation.Text + " directly";
                    return false;
                }
                
            }
             if (ddlType.Text == "Region")
            {
                if (ddlTypeAllocation.Text == "HO" || ddlTypeAllocation.Text == "Agent")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Cannot " + value + " " + ddlTypeAllocation.Text + " directly";
                    return false;
                }
                
            }
             if (ddlType.Text == "City")
             {
                 if (ddlTypeAllocation.Text == "HO" || ddlTypeAllocation.Text == "Region")
                 {
                     lblStatus.ForeColor = Color.Red;
                     lblStatus.Text = "Cannot " + value + " " + ddlTypeAllocation.Text + " directly";
                     return false;
                 }

             }

             if (ddlType.Text == "Agent")
             {
                 if (ddlTypeAllocation.Text !="Agent" )
                 {
                     lblStatus.ForeColor = Color.Red;
                     lblStatus.Text = "Please select Agent to " + value ;
                     return false;
                 }

             }
             return true;
           
                        
                       
        }

        protected void ReviseCount()
    {

        if (txtAWBTo.Text != "" && txtAWBFrom.Text != "")
        {
            txtCount.Text = "";
            if (int.Parse(txtAWBTo.Text) >= int.Parse(txtAWBFrom.Text))
            {
                txtCount.Text = ((int.Parse(txtAWBTo.Text) - int.Parse(txtAWBFrom.Text)) + 1).ToString();
            }
        }
        if (txtAWBTo.Text == "" || txtAWBFrom.Text == "")
        {
            txtCount.Text = "";
        }

    }

        protected void txtAWBFrom_TextChanged(object sender, EventArgs e)
        {
            //ReviseCount();
            //**** VKT 08102013 Fetch AWB Type (P/ E) from database based on AWB Number entered.
            if (txtAWBFrom.Text.Length == 7 && txtAWBFrType.Text != "" && ddlTypeAllocation.SelectedValue != "HO")
            {
                try
                {
                    
                    //Fetch AWB Type (E or P)   
                    string awbtype = objBAL.GetAWBStockTypeWithPrefix(txtAWBFrType.Text, txtAWBFrom.Text);
                    string awbstocktype = objBAL.GetAWBStockTypeNew(txtAWBFrom.Text);
                    if (awbtype != null && awbtype != "")
                    {
                       // ddlAWBtype.SelectedValue = awbtype;
                        if (awbtype == "EAWB")
                        {
                            ddlAWBtype.SelectedValue = "Electronic";
                        }
                        if (awbtype == "PAWB")
                        {
                            ddlAWBtype.SelectedValue = "Physical";

                        }
                    }
                    if (awbstocktype != null && awbstocktype != "")
                    {
                    ddlStockAWBtypeAllocation.SelectedValue =awbstocktype;
                    ddlStockAWBtypeAllocation.Enabled = false;
                    }


                    #region CNType
                    DataSet cntype=null;
                    try
                    {
                        SQLServer da = new SQLServer(Global.GetConnectionString());
                        string[] paramname = new string[2];
                        paramname[0] = "AWBFrom";
                        paramname[1] = "AWBPrefix";

                        object[] paramvalue = new object[2];
                        paramvalue[0] = txtAWBFrom.Text;

                        paramvalue[1] = txtAWBFrType.Text;

                        SqlDbType[] paramtype = new SqlDbType[2];
                        paramtype[0] = SqlDbType.Int;
                        paramtype[1] = SqlDbType.VarChar;
                        cntype = da.SelectRecords("GetAWBStockCNType", paramname, paramvalue, paramtype);

                        if (cntype != null)
                        {
                            if (cntype.Tables[0].Rows.Count > 0)
                            {
                                ddlCNTypeAllocate.SelectedValue = cntype.Tables[0].Rows[0]["CNType"].ToString();
                                ddlCNTypeAllocate.Enabled = false;
                            }
                            else
                            {
                                ddlCNTypeAllocate.SelectedIndex = 0;
                                ddlCNTypeAllocate.Enabled = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                    finally
                    {
                        if (cntype != null)
                        {
                            cntype.Dispose();
                        }
                    }
                    #endregion
                }
                catch (Exception)
                {
                }
            }
        }

        protected void txtAWBTo_TextChanged(object sender, EventArgs e)
        {
            //ReviseCount();
        }

        protected void txtCount_TextChanged(object sender, EventArgs e)
        {
            // getNextAWB();

            //    if (txtAWBFrom.Text != "" && txtCount.Text!="")
            //        txtAWBTo.Text = ((int.Parse(txtCount.Text) + int.Parse(txtAWBFrom.Text)) - 1).ToString();
        }
        protected void grdStockAllocation_Sorting(object sender, GridViewSortEventArgs e)
        {
        //    DataSet AWB = (DataSet)Session["AllocationGridSort"];
            
        //    grdStockAllocation.DataSource = AWB.Tables[0];
        //    GridViewSortExpression = e.SortExpression;

        //    //Gets the Pageindex of the GridView.
        //    int iPageIndex = grdStockAllocation.PageIndex;
        //    grdStockAllocation.DataSource = SortDataTable(myDataTable, false);
        //    grdStockAllocation.DataBind();
        //    grdStockAllocation.PageIndex = iPageIndex;

            

        //    //grdStockAllocationChild.DataSource = AWB.Tables[1];
        //    //grdStockAllocationChild.DataBind();

            
        }
        
        protected void grdStockAllocation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdStockAllocation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "List")
                {
                    string fromAWB = string.Empty, toAWB = string.Empty;
                    int val = Convert.ToInt32(e.CommandArgument);
                    fromAWB = grdStockAllocation.Rows[val].Cells[2].Text.ToString().PadLeft(7,'0');
                    toAWB = grdStockAllocation.Rows[val].Cells[3].Text.ToString().PadLeft(7, '0');
                    if (fromAWB != string.Empty && toAWB != string.Empty)
                    {
                        // string Url = "showAWB.aspx?fromAWB=" + fromAWB.Substring(fromAWB.Length - 7, 7) + "&toAWB=" + toAWB.Substring(toAWB.Length - 7, 7) + "','_blank', 'left=200,top=200,width=850,height=300,toolbar=0,resizable=0'";
                        //string Url = "showAWB.aspx?fromAWB=" + fromAWB.Substring(fromAWB.Length - 7, 7) + "&toAWB=" + toAWB.Substring(toAWB.Length - 7, 7);
                        string Url = "showAWB.aspx?fromAWB=" + fromAWB + "&toAWB=" + toAWB;
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "newpage", "customOpen('" + Url + "');", true);
                    }
                }
            }
            catch (Exception)
            { }

        }
        
        protected void grdStockAllocationChild_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "List")
                {
                    string fromAWB = string.Empty, toAWB = string.Empty;
                    int val = Convert.ToInt32(e.CommandArgument);
                    fromAWB = grdStockAllocationChild.Rows[val].Cells[3].Text.ToString().PadLeft(7,'0');
                    toAWB = grdStockAllocationChild.Rows[val].Cells[4].Text.ToString().PadLeft(7, '0');
                    if (fromAWB != string.Empty && toAWB != string.Empty)
                    {
                        string Url = "showAWB.aspx?fromAWB=" + fromAWB.Substring(fromAWB.Length - 7, 7) + "&toAWB=" + toAWB.Substring(toAWB.Length - 7, 7);
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "newpage", "customOpen('" + Url + "');", true);
                    }
                }
            }
            catch (Exception)
            { }
        }

        protected void grdStockAllocation_SelectedIndexChanged1(object sender, EventArgs e)
        {

        }
        //protected void grdStockAllocationChild_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdStockAllocationChild.RowCommand -= new GridViewCommandEventHandler(grdStockAllocation_RowCommand);

        //    DataSet AWB = (DataSet)Session["AllocationGridSort"];

        //    grdStockAllocationChild.DataSource = AWB.Tables[0];


        //    grdStockAllocationChild.PageIndex = e.NewPageIndex;
        //    // getNextAWB();
        //    // grdStockAllocation.DataSource = "";
        //    grdStockAllocationChild.DataBind();
        //    // grdStockAllocation.RowCommand += new GridViewCommandEventHandler(grdStockAllocation_RowCommand);
        
        //}

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
                        if (objDS.Tables[0].Rows[j]["ControlId"].ToString() == "StockAllocation")
                        {
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

        protected void ddlCodeAllocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtAWBFrom.Text = txtAWBTo.Text = "";

                string aType = ddlTypeAllocation.Text;
                string aCode = ddlCodeAllocation.Text;
                if(string.Equals(ddlTypeAllocation.SelectedItem.Text.ToString(),"Agent"))
                getNextAWB(); 
            }
            catch (Exception ex)
            {
                
                
            }

        }
        #region Next AWB

        protected void  getNextAWB( )
    {
        try
        {
            DataSet ds= new DataSet();
            string Acode=string.Empty;
            string Atype = string.Empty;
            string Partype = string.Empty;
            string Parname = string.Empty;
            bool allocated=false;
            string CountAWB;

            object[] tmp;

            //Case when Afrom & Ato needs to be filled based on Count
            //if (txtCount.Text.Length == 0)
            //    return;
            //else
            // CountAWB = txtCount.Text.TrimEnd(); 

            //Case when Afrom needs to be filled based on DDL
                CountAWB = "1";
            //EndCase

            if (ddlCodeAllocation.Text != "Select" && ddlCodeAllocation.Text != "" && ddlCodeAllocation.Text != "HO")
            {
                Acode = ddlCodeAllocation.Text;
                Atype = ddlTypeAllocation.Text;
                Partype = Atype;
                Parname = Acode;
            if (Acode.Length == 0)
            {
                return;
            }
                //Mod
            do
            {
                tmp = new object[] { Parname, Partype };
                ds = objBAL.GetParentDetails(tmp);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //Mod 23Jul2013
                    Partype = ds.Tables[0].Rows[0][0].ToString();
                    Parname = ds.Tables[0].Rows[0][1].ToString();

                    tmp = new object[] { Partype, Parname, "D" };
                    //D- temperory adjustment

                    ds = objBAL.GetAWBStock(tmp);
                    # region Code
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataView dv = new DataView(ds.Tables[0]);

                        //ds.Tables[0].Columns.Remove("Alevel");
                        //ds.Tables[0].Columns.Remove("Status");
                        //ds.Tables[0].Columns.Remove("Atime");
                        //ds.Tables[0].Columns.Remove("Auser");
                        //ds.Tables[0].Columns.Remove("cntype");
                        dv.Sort = " AFrom";
                        
                        dv.RowFilter = "AWBType='" + ddlAWBtype.Text + "' and CNType = '" + ddlCNTypeAllocate.Text + "' and  AWBstockType = '" + ddlStockAWBtypeAllocation.SelectedItem.Text + "' and available >0";
                         
                       // DataView dv = new DataView(ds.Tables[0]);
                        dv.Table.Columns.Remove("Alevel");
                        dv.Table.Columns.Remove("Status");
                        dv.Table.Columns.Remove("Atime");
                        dv.Table.Columns.Remove("Auser");
                        dv.Table.Columns.Remove("cntype");

                        //dv.RowFilter = "available >0";
                        //if(dv.Count>0)
                        //dv.Sort = " AFrom";
                        

                        string Afrom = string.Empty;
                        // bool allocated=false;
                        int totalAvailable = 0;
                        int count = dv.Count;
                        string[,] stockRange = new string[2, count];
                        # region If continuous Range of stock is available
                        for (int i = 0; i < count; i++)
                        {

                            if (Convert.ToInt32(dv[i][2].ToString()) >= Convert.ToInt32(CountAWB))
                            {
                                if (Convert.ToString(dv[i][3]).Length == 0)
                                {
                                    Afrom = dv[i][0].ToString();
                                }
                                else
                                {
                                    Afrom = dv[i][3].ToString();

                                }
                                Afrom = Afrom.Substring(Afrom.Length - 7, 7);

                                // +1 As Next AWB range for allocation
                                int newStartRange = int.Parse(Afrom);
                                newStartRange += 1;
                                txtAWBFrom.Text = newStartRange.ToString();
                                allocated = true;

                                break;
                            }
                            totalAvailable += Convert.ToInt32(dv[i][2].ToString());
                        }
                        # endregion

                        # region If no continuous range is available
                        //                if (allocated==false)
                        //                {
                        //                if(totalAvailable>=Convert.ToInt32(txtCount.Text.Trim()))
                        //                {
                        //                    Session["stockPopup"] = dv;
                        //                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "newpage", "customOpen('ShowStockpopup.aspx');", true);


                        //                    //Response.Write("<script type='text/javascript'>window.open('ShowStockpopup.aspx','_blank');</script>");

                        //                    //ClientScript.RegisterStartupScript(this.GetType(), "ShowStockpopup", "ShowStockpopup();", true);
                        //                    //Response.Write("<script>");

                        //                    //Response.Write("window.open('ShowStockpopup.aspx','_blank', 'left=200,top=200,width=850,height=300,toolbar=0,resizable=0')");
                        //                    //Response.Write("window.open('ShowStockpopup.aspx','_blank','width=560,height=340,toolbar=0,menubar=0,location=0')"); 
                        //                    //Response.Write("</script>");




                        //                    //for (int i = 0; i < count; i++)
                        //                    //{
                        //                    //    for (int j = 0; j < 2; j++)
                        //                    //    {
                        //                    //        stockRange[i, j] = dv[i][j].ToString();
                        //                    //        stockRange[i, j].Substring(stockRange[i, j].Length - 7, 7);

                        //                    //    }
                        //                    //}


                        //                }
                        //                }

                        # endregion

                    }
                    # endregion

                }
            } while (allocated == false && Parname !="HO");
                }
            else
            {
                if(ddlTypeAllocation.Text!="HO")
            {
                lblStatus.Text = "Please select Stock Holder Code value for allocation";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }   return;
            }
        }
        catch (Exception)
        {
            
            throw;
        }
    }
        #endregion

        protected void ddlCNTypeAllocate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtAWBFrom.Text = txtAWBTo.Text = "";
            //if (string.Equals(ddlTypeAllocation.SelectedItem.Text.ToString(), "Agent"))
            //    getNextAWB(); 
        }

        protected void ddlAWBtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtAWBFrom.Text = txtAWBTo.Text = "";

            //if (string.Equals(ddlTypeAllocation.SelectedItem.Text.ToString(), "Agent"))
            //    getNextAWB(); 
        }

        protected void ddlStockAWBtypeAllocation_SelectedIndexChanged(object sender, EventArgs e)
        {
           // txtAWBFrom.Text = txtAWBTo.Text = "";

            //if (string.Equals(ddlTypeAllocation.SelectedItem.Text.ToString(), "Agent"))
            //    getNextAWB(); 
        }

      #region Load Arrival Grid

        public void LoadGridAllocation()
        { 
            try
            {
                DataTable myDataTable = new DataTable();
                DataColumn myDataColumn;

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "LEVEL";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "FROM";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "TO";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "STATUS";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ALLOCATION TIME";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ALLOCATED BY";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AVAILABLE AWB";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "LAST ALLOCATED AWB";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "CNOTE TYPE";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "AWB TYPE";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "APrefix";
                myDataTable.Columns.Add(myDataColumn);

                DataRow dr;
                dr = myDataTable.NewRow();
                dr["Level"] = "";
                dr["From"] = "";//"5";
                dr["To"] = "";// "5";
                dr["Status"] = "";// "9";
                dr["APrefix"] = "";// "9";
               //// dr["AllocationTime"] = "";
               // dr["AllocatedBy"] = "";
               // dr["AvailableAwb"] = "";
               // dr["LastAllocated"] = "";
               // dr["CnoteType"] = "";
               // dr["AwbType"] = "";
              
                myDataTable.Rows.Add(dr);

                //Bind the DataTable to the Grid
                grdStockAllocation.DataSource = null;
                grdStockAllocation.DataSource = myDataTable;
                grdStockAllocation.DataBind();
                Session["AllocationGridSort"] = myDataTable.Copy();

                ////Add values in Discrepancy Grid.
                //if (GVArrDet.Rows.Count >= 1)
                //{
                //    DataSet ddlds = da.SelectRecords("Sp_GetDiscrepancy");
                //    if (ddlds != null && ddlds.Tables.Count > 0)
                //    {
                //        DropDownList ddl = ((DropDownList)GVArrDet.Rows[0].FindControl("ddlDiscrepancy"));
                //        if (ddl != null)
                //        {
                //            ddl.DataSource = ddlds.Tables[0];
                //            ddl.DataTextField = ddlds.Tables[0].Columns[0].ColumnName;
                //            ddl.DataValueField = ddlds.Tables[0].Columns[0].ColumnName;
                //            ddl.DataBind();
                //            ddl.Dispose();
                //        }
                //        ddlds.Dispose();
                //    }
                //}
                
              

                myDataColumn.Dispose();
                myDataTable.Dispose();

            }
            catch (Exception)
            {
            }
        }
        #endregion Load Arrival Grid

        protected void btnPrintHistory_Click(object sender, EventArgs e)
        {

        }

        protected void grdStockAllocation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataView dsResult = (DataView)Session["StockAllocationGrd"];

            grdStockAllocation.PageIndex = e.NewPageIndex;
            grdStockAllocation.DataSource = dsResult;
            grdStockAllocation.DataBind();

        }
        protected void grdStockAllocationChild_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataView dsResult = (DataView)Session["StockAllocationGrdChild"];

            grdStockAllocationChild.PageIndex = e.NewPageIndex;
            grdStockAllocationChild.DataSource = dsResult;
            grdStockAllocationChild.DataBind();

        }

        protected void GridViewHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dsResult = (DataTable)Session["HistoryAWB"];

            GridViewHistory.PageIndex = e.NewPageIndex;
            GridViewHistory.DataSource = dsResult;
            GridViewHistory.DataBind();

        }
        
        public void GetData()
        {
            try
            {

                Clear();
         
                if (txtAWBNo.Text == string.Empty && (ddlType.SelectedItem.Text == "Select" || ddlCode.SelectedItem.Text == "Select"))
                {
                    lblStatus.Text = "Please enter mandatory fields marked with (*)!";
                    lblStatus.ForeColor = Color.Blue;
                    lblAWBStatus.Visible = false;
                    return;
                }

                if (txtAWBNo.Text != string.Empty)
                {

                    string awbstatus = objBAL.GetAWBStatus(txtAWBNo.Text);

                    if (awbstatus != null && awbstatus != "")
                    {
                        lblAWBStatus.Text = awbstatus;
                        lblAWBStatus.ForeColor = Color.Green;
                        lblAWBStatus.Visible = true;
                    }

                }
                else
                {
                    lblAWBStatus.Text = string.Empty;
                }

                grdStockAllocation.DataSource = null;
                grdStockAllocation.DataBind();

                grdStockAllocationChild.DataSource = null;
                grdStockAllocationChild.DataBind();

                object[] AWBStock = new object[4];
                int i = 0;

                AWBStock.SetValue(ddlType.SelectedItem.Value.ToString(), i);
                i++;
                AWBStock.SetValue(ddlCode.SelectedItem.Value.ToString(), i);
                i++;
                AWBStock.SetValue(ddlStockAWBtype.SelectedValue.ToString(), i);
                i++;
                AWBStock.SetValue(txtAWBNo.Text.Trim(), i);

                DataSet AWB = objBAL.GetAWBStock(AWBStock);

                DataView objDv = new DataView(AWB.Tables[0]);
         
                objDv.RowFilter = "cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";

                if (ddlStockAWBtype.SelectedItem.Text.ToString() != "Select" && ddlAWBTypeList.SelectedValue.ToString() != "All")
                {
                    objDv.RowFilter = "awbtype='" + ddlAWBTypeList.SelectedValue.ToString() + "' and AWBStockType='" + ddlStockAWBtype.SelectedItem.Text.ToString() + "' and cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";
                }
                if (ddlStockAWBtype.SelectedItem.Text.ToString() == "Select" && ddlAWBTypeList.SelectedValue.ToString() != "All")
                {
                    objDv.RowFilter = "awbtype='" + ddlAWBTypeList.SelectedValue.ToString() + "' and cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";
                }
                if (ddlStockAWBtype.SelectedItem.Text.ToString() != "Select" && ddlAWBTypeList.SelectedValue.ToString() == "All")
                {
                    objDv.RowFilter = "AWBStockType='" + ddlStockAWBtype.SelectedItem.Text.ToString() + "' and cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";
                }

               // Session["StockAllocationExportParent"] = objDv;
                
                DataView objDv1 = new DataView();
                if (txtAWBNo.Text != string.Empty)
                {
                    string AWBNo = txtAWBNo.Text.Length > 7 ? txtAWBNo.Text.Trim().Substring(0, 7) : txtAWBNo.Text.Trim();
                    objDv.RowFilter = "Afrom<=" + AWBNo + " and Ato>=" + AWBNo;

                }
                if (objDv.Count == 0)
                {
                    lblStatus.Text = "No Records Found";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblAWBStatus.Visible = false;

                }
                lblParentLevel.Text = ddlType.SelectedItem.Text != "Select" ? ddlType.SelectedItem.Text.Trim() : string.Empty;

                objDv.Sort = "Afrom";
                if (AWB.Tables.Count > 1)
                {
                    objDv1 = new DataView(AWB.Tables[1]);

                    objDv1.RowFilter = "cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";

                    if (ddlStockAWBtype.SelectedItem.Text.ToString() != "Select" && ddlAWBTypeList.SelectedValue.ToString() != "All")
                    {
                        objDv1.RowFilter = "awbtype='" + ddlAWBTypeList.SelectedValue.ToString() + "' and AWBStockType='" + ddlStockAWBtype.SelectedItem.Text.ToString() + "' and cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";
                    }
                    if (ddlStockAWBtype.SelectedItem.Text.ToString() == "Select" && ddlAWBTypeList.SelectedValue.ToString() != "All")
                    {
                        objDv1.RowFilter = "awbtype='" + ddlAWBTypeList.SelectedValue.ToString() + "' and cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";
                    }
                    if (ddlStockAWBtype.SelectedItem.Text.ToString() != "Select" && ddlAWBTypeList.SelectedValue.ToString() == "All")
                    {
                        objDv1.RowFilter = "AWBStockType='" + ddlStockAWBtype.SelectedItem.Text.ToString() + "' and cntype= '" + ddlCNType.SelectedItem.Text.ToString() + "'";
                    }



                    if (txtAWBNo.Text != string.Empty)
                    {
                        string AWBNo = txtAWBNo.Text.Length > 7 ? txtAWBNo.Text.Trim().Substring(0, 7) : txtAWBNo.Text.Trim();
                        objDv1.RowFilter = "Afrom<=" + AWBNo + " and Ato>=" + AWBNo;

                    }

                    objDv1.Sort = "Afrom";
                 }
               
                if (objDv.Count > 0)
                {
                    if (lblParentLevel.Text == "HO")
                    {
                        lblChildLevel.Text = "Region";
                    }
                    else
                        if (lblParentLevel.Text == "Region")
                        {
                            lblChildLevel.Text = "City";
                        }
                        else
                            if (lblParentLevel.Text == "City")
                            {
                                lblChildLevel.Text = "Agent";
                            }


                }

                DataSet ExportData = new DataSet();
                if (lblParentLevel.Text == "Agent")
                {
                    ExportData.Tables.Add(objDv.ToTable(lblParentLevel.Text));
                    Session["StockAllocationExport"] = ExportData;
               
                }
                else
                {
                    ExportData.Tables.Add(objDv.ToTable(lblParentLevel.Text));
                    ExportData.Tables.Add(objDv1.ToTable(lblChildLevel.Text));
                    Session["StockAllocationExport"] = ExportData;
                }
                   


                


            }
            catch (Exception ex)
            {
            }

        }

        protected void btnExportStock_Click(object sender, EventArgs e)
        {

            DataSet dt = null;
            Session["StockAllocationExport"] = null;
            lblStatus.Text = "";
            try
            {
                if ((DataSet)Session["StockAllocationExport"] == null)
                    GetData();

                dt = (DataSet)Session["StockAllocationExport"];
                if (lblParentLevel.Text == "Agent")
                {
                    dt.Tables[0].TableName = lblParentLevel.Text;
                }
                else
                {
                    dt.Tables[0].TableName = lblParentLevel.Text;
                    dt.Tables[1].TableName = lblChildLevel.Text;
                }


                if (Session["StockAllocationExport"] == null && dt == null)
                {
                    lblStatus.Text = "No records found";
                    lblStatus.ForeColor = Color.Red;
                    //SaveUserActivityLog(lblStatus.Text);
                    
                    return;
                }
                ExcelHelper.ToExcel(dt, "StockAllocation" + ".xls", Response);

                Response.End();
                Response.Flush();
                Response.Close();
            }
            catch (Exception ex)
            { }
            finally
            {

                dt = null;
            }
        }
        
        }        
    }

