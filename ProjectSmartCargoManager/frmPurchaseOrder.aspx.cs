using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//using clsDataLib;
using System.Configuration;
using System.Drawing;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public partial class frmPurchaseOrder : System.Web.UI.Page
    {
        DataTable SupInDetails = null;
        DataTable SupInDetailsinner = null;
        DataSet innergrid = new DataSet();
        DataSet ds = null;
      
        string gvUniqueID = String.Empty;
        int gvNewPageIndex = 0;
        int gvEditIndex = -1;
        string gvSortExpr = String.Empty;
        clsFillCombo cfc = new clsFillCombo();
     
        protected void fillManuandRegdrop()
        {
            try
            {
                cfc.FillAllComboBoxes("tblManufacturer", "Select", ddlManu);
                cfc.FillAllComboBoxes("tblWWRegionMaster", "Select", ddlRegion);
                cfc.FillAllComboBoxes("tblWarehouseMaster", "Select", ddlWH);
                cfc.FillAllComboBoxes("tblULDTypeMaster", "Select", ddlAddUldType);
                cfc.FillAllComboBoxes("tblManufacturerPartNo", "Select", ddlULDPart);
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void clearAllSession()
        {
            Session["chekAppend"] = null;// this will chk whether user perform some functionality before save
            Session["PageMode"] = null;
            Session["findcnt"] = 0;
            Session["NEWPO"] = null;
            Session["innerdata"] = null;
            Session["idofEdit"] = null;
            Session["PageMode"] = "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtPODate.Attributes.Add("readonly", "readonly"); 
            txtAddExpDelDate.Attributes.Add("readonly", "readonly");
            if (!IsPostBack)
            {
                try
                {
                    clearAllSession();
                    txtAddExpDelDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                    txtPODate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                    fillManuandRegdrop();
                    rblType_SelectedIndexChanged(sender, e);
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    if (Request.QueryString[0] != null)
                    {
                        Session["PageMode"] = Request.QueryString[0].ToString();
                    }
                    if (Request.QueryString[0] == "Create")
                    {
                        txtPONo.Enabled = false;
                        btnSearch.Visible = false;
                        GVOrdDetails.Columns[6].Visible = false;
                        GVOrdDetails.Columns[8].Visible = false;
                        ddlManu.Focus();
                    }
                    else if (Request.QueryString[0] == "Search")
                    {
                        txtPONo.Enabled = true;
                        btnSearch.Visible = true;
                        GVOrdDetails.Columns[6].Visible = false;
                        GVOrdDetails.Columns[8].Visible = true;
                        txtPONo.Focus();
                        #region for getting PO from PO list Page
                        try
                        {
                            string POForSearch = Request.QueryString[1].ToString();
                            txtPONo.Text = POForSearch.Trim();
                            search();
                        }
                        catch (Exception)
                        {
                        } 
                        #endregion
                    }
                    else
                    {
                        return;
                    }
                    
                   
                }
                catch (Exception ex)
                {
                }
            }
        }

        protected void loadgrid()
        {
            if (Session["NEWPO"] != null)
            {
                GVOrdDetails.DataSource = (DataTable)Session["NEWPO"];
            }
            else
            {
                GVOrdDetails.DataSource = null;
            }
            GVOrdDetails.DataBind();
        }

        protected void createPO()
        {
            SupInDetails = new DataTable();

            SupInDetails.Columns.Add("ULDType");
            SupInDetails.Columns.Add("ULDPart");
            SupInDetails.Columns.Add("Qty");
            SupInDetails.Columns.Add("Delivery");
            SupInDetails.Columns.Add("WareHouse");
            SupInDetails.Columns.Add("ID");
            //SupInDetails.Columns.Add("ULD");
        }
      
        protected void addgrid(string ULDType, string ULDPart, string Qty, string Delivery,string Warehouse)
        {
            #region outer grid
            if (Session["NEWPO"] != null)
            {
                SupInDetails = (DataTable)Session["NEWPO"];
            }
            else
            {
                createPO();
            }
            DataRow l_Datarow = SupInDetails.NewRow();
            l_Datarow["ULDType"] = ULDType;
            l_Datarow["ULDPart"] = ULDPart;
            l_Datarow["Qty"] = Qty;
            l_Datarow["Delivery"] = Delivery;
            l_Datarow["WareHouse"] = Warehouse;
            l_Datarow["ID"] = "0";


            //l_Datarow["ULD"] = ULD;

            SupInDetails.Rows.Add(l_Datarow);

            if (SupInDetails != null && SupInDetails.Rows.Count > 0)
            {
                Session["NEWPO"] = SupInDetails;
            } 
            #endregion

        

            loadgrid();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                RequiredFieldValidator6.ErrorMessage = "";
                lblerror.Text = "";
                #region check existance in grid
                try
                {
                    if (GVOrdDetails.Rows.Count > 0)
                    {
                        if (rblType.SelectedIndex == 1)
                        {
                            for (int i = 0; i < GVOrdDetails.Rows.Count; i++)
                            {
                                if (GVOrdDetails.Rows[i].Cells[0].Text.Trim() == "")
                                {
                                    if (GVOrdDetails.Rows[i].Cells[0].Text.Trim() == ddlULDPart.SelectedItem.Text.ToString())
                                    {
                                        lblerror.Text = "ULD Part information already exist in data list";
                                        lblerror.ForeColor = Color.Red;
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            double noofuldDG = 0;
                            if (rblSM.SelectedIndex == 0)
                            {
                                noofuldDG = 1;

                            }
                            else
                            {
                                noofuldDG = (Convert.ToDouble(txtTo.Text.Trim()) - Convert.ToDouble(txtFrom.Text.Trim())) + 1;
                            }
                            DataSet gv = (DataSet)Session["innerdata"];

                            if (gv.Tables.Count > 0)
                            {
                                for (int h = 0; h < gv.Tables.Count; h++)
                                {
                                    if (gv.Tables[h].Rows.Count > 0)
                                    {
                                        for (int l = 0; l < gv.Tables[h].Rows.Count; l++)
                                        {
                                            double val = 0;
                                            val = Convert.ToDouble(txtFrom.Text.Trim());

                                            for (int k = 0; k < noofuldDG; k++)
                                            {
                                                string uldno = gv.Tables[h].Rows[l][0].ToString();
                                                if (uldno.Trim() != "")
                                                {
                                                    if (uldno == ddlAddUldType.SelectedItem.Text.ToString() + val.ToString().PadLeft(5, '0') + Session["AirlinePrefix"].ToString())
                                                    {
                                                        lblerror.Text = "ULD No: " + uldno + " is in the range of available ULDs in data list";
                                                        lblerror.ForeColor = Color.Red;
                                                        return;
                                                    }
                                                }
                                                val = val + 1;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    lblerror.Text = "Error : " + ex.Message;
                    lblerror.ForeColor = Color.Red;
                    return;
                } 
                #endregion

                if (rblType.SelectedIndex == 0)
                {
                    #region checking in database for range
                    try
                    {
                        string[] paramnameInn = new string[5];
                        paramnameInn[0] = "from";
                        paramnameInn[1] = "to";
                        paramnameInn[2] = "ULDType";
                        paramnameInn[3] = "CompN";
                        paramnameInn[4] = "ID";

                        object[] paramvalueInn = new object[5];
                        paramvalueInn[0] = Convert.ToDecimal(txtFrom.Text.ToString());
                        if (rblSM.SelectedIndex == 1)
                        {
                            paramvalueInn[1] = Convert.ToDecimal(txtTo.Text.ToString());
                        }
                        else
                        {
                            paramvalueInn[1] = Convert.ToDecimal(txtFrom.Text.ToString());
                        }
                        paramvalueInn[2] = ddlAddUldType.SelectedItem.Text.ToString();
                        paramvalueInn[3] = Session["AirlinePrefix"].ToString();
                        if(Session["idofEdit"]==null)
                        {
                            paramvalueInn[4] = "0";
                        
                        }else
                        {
                        paramvalueInn[4] =Session["idofEdit"].ToString();
                        }

                        SqlDbType[] paramtypeInn = new SqlDbType[5];
                        paramtypeInn[0] = SqlDbType.Decimal;
                        paramtypeInn[1] = SqlDbType.Decimal;
                        paramtypeInn[2] = SqlDbType.NVarChar;
                        paramtypeInn[3] = SqlDbType.NVarChar;
                        paramtypeInn[4] = SqlDbType.BigInt;



                        SQLServer dbchk = new SQLServer(Global.GetConnectionString());
                        DataSet dsChk = dbchk.SelectRecords("spchkExistanceInRange", paramnameInn, paramvalueInn, paramtypeInn);
                        if (dsChk != null && dsChk.Tables.Count > 0)
                        {
                            if (dsChk.Tables[0].Rows.Count > 0)
                            {
                                if (dsChk.Tables[0].Rows[0][0].ToString() != "0")
                                {
                                    lblerror.Text = "In this range, ULD: " + dsChk.Tables[0].Rows[0][0].ToString() + " is already allocated";
                                    lblerror.ForeColor = Color.Red;
                                    return;
                                }
                            }
                            else
                            {
                                lblerror.Text = "Error while checking ULD range";
                                lblerror.ForeColor = Color.Red;
                                return;
                            }
                        }
                        else
                        {
                            lblerror.Text = "Error while checking ULD range";
                            lblerror.ForeColor = Color.Red;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblerror.Text = "Error : " + ex.Message;
                        lblerror.ForeColor = Color.Red;
                        return;
                    }
                    #endregion
                }
                
                Session["findcnt"] = 0;  // for maintain datatable no for inner grid
                
                #region inner grid
               
                 SupInDetailsinner = new DataTable();
                 SupInDetailsinner.Columns.Add("ULDRan");
                 double noofuld=0;
                 if (rblType.SelectedIndex == 0 && rblSM.SelectedIndex == 1)
                 {
                     noofuld = (Convert.ToDouble(txtTo.Text.Trim()) - Convert.ToDouble(txtFrom.Text.Trim())) + 1;
                     double val=0;
                     val = Convert.ToDouble(txtFrom.Text.Trim());

                     for (int i = 1; i <= noofuld; i++)
                     {
                         DataRow l_Datarowinner = SupInDetailsinner.NewRow();
                         l_Datarowinner["ULDRan"] = ddlAddUldType.SelectedItem.Text.ToString() + val.ToString().PadLeft(5,'0') + Session["AirlinePrefix"].ToString();
                         SupInDetailsinner.Rows.Add(l_Datarowinner);
                         val = val + 1;
                     }
                 }
                 else if (rblType.SelectedIndex == 0 && rblSM.SelectedIndex == 0)
                 {
                     DataRow l_Datarowinner = SupInDetailsinner.NewRow();
                     l_Datarowinner["ULDRan"] = ddlAddUldType.SelectedItem.Text.ToString() + txtFrom.Text.Trim().PadLeft(5,'0')+ Session["AirlinePrefix"].ToString();
                     SupInDetailsinner.Rows.Add(l_Datarowinner);
                 }
                 else
                 {
                     DataRow l_Datarowinner = SupInDetailsinner.NewRow();
                     l_Datarowinner["ULDRan"] = "";
                     SupInDetailsinner.Rows.Add(l_Datarowinner);
                 }
                 if (Session["innerdata"] != null)
                 {
                     innergrid = (DataSet)Session["innerdata"];
                     innergrid.Tables.Add(SupInDetailsinner);
                 }
                 else
                 {
                     innergrid.Tables.Add(SupInDetailsinner);
                     Session["innerdata"] = innergrid;
                 }
                #endregion
               
                if (rblType.SelectedIndex == 0)
                {
                    if (rblSM.SelectedIndex == 0)
                    {
                        addgrid(ddlAddUldType.SelectedItem.Text.ToString(), "", "1", txtAddExpDelDate.Text.Trim(),ddlWH.SelectedItem.Text.ToString());
                    }
                    else
                    {
                        addgrid(ddlAddUldType.SelectedItem.Text.ToString(), "", noofuld.ToString(), txtAddExpDelDate.Text.Trim(), ddlWH.SelectedItem.Text.ToString());
                    }
                }
                else
                {
                    addgrid("", ddlULDPart.SelectedItem.Text.ToString(), txtCount.Text.Trim(), txtAddExpDelDate.Text.Trim(), ddlWH.SelectedItem.Text.ToString());
                }
                Session["chekAppend"] = "0";
                RequiredFieldValidator6.ErrorMessage = "*";
                btnSave.Visible = true;
                btnSave.Focus();
                txtTo.Text = "";
                txtFrom.Text = "";
                txtCount.Text = "";
                Session["idofEdit"] = null;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
                lblerror.ForeColor = Color.Red;
            }
        }

        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblType.SelectedIndex == 0)
            {
                uldPart.Visible = false;
                uldtype.Visible = true;
                rblSM.SelectedIndex = 1;
                rblSM_SelectedIndexChanged(sender,e);
            }
            else
            {
                uldPart.Visible = true;
                uldtype.Visible = false;
            }
            ddlAddUldType.SelectedIndex = 0;
            txtLastAlloc.Text = "0";
            //ddlWH.SelectedIndex = 0;
            txtCount.Text = "";
            ddlULDPart.SelectedIndex = 0;
        }

        protected void rblSM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblSM.SelectedIndex == 0)
            {
                txtTo.Visible = false;
                lblTo.Visible = false;
                lblFrom.Text = "Number";
            }
            else {
                txtTo.Visible = true;
                lblTo.Visible = true;
                lblFrom.Text = "From";
            }
           
            txtFrom.Text = "";
            txtTo.Text = "";
        }

        protected void GVOrdDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow row = e.Row;
                string strSort = string.Empty;

                if (row.DataItem == null)
                {
                    return;
                }

                GridView gv1 = new GridView();

                gv1 = (GridView)row.FindControl("grdChildGridNormal");
                if (gv1.UniqueID == gvUniqueID)
                {
                    gv1.PageIndex = gvNewPageIndex;
                    gv1.EditIndex = gvEditIndex;
                }
               
                innergrid = (DataSet)Session["innerdata"];
                gv1.DataSource = innergrid.Tables[Convert.ToInt32(Session["findcnt"].ToString())];
                gv1.DataBind();
                Session["findcnt"] = Convert.ToInt32(Session["findcnt"]) + 1;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
                lblerror.ForeColor = Color.Red;
            }
        }

        protected bool inserOrderDet(string uldtype, string uldPart, string qty, string delivery, string warehouse,int rowNo,string pono,string username,string datetime,string mode)
        {
            bool resD = false;
            #region insert PO details parameter
            try
            {
                string[] paramnameInn = new string[9];
                paramnameInn[0] = "uldtype";
                paramnameInn[1] = "uldpart";
                paramnameInn[2] = "delivery";
                paramnameInn[3] = "createdBy";
                paramnameInn[4] = "createdOn";
                paramnameInn[5] = "PONO";
                paramnameInn[6] = "WareHouse";
                paramnameInn[7] = "rowno";
                paramnameInn[8] = "mode";

                object[] paramvalueInn = new object[9];
                paramvalueInn[0] = uldtype.ToString();
                paramvalueInn[1] = uldPart.ToString();
                paramvalueInn[2] = Convert.ToDateTime(delivery);
                paramvalueInn[3] = username;
                paramvalueInn[4] = Convert.ToDateTime(datetime);
                paramvalueInn[5] = pono;
                paramvalueInn[6] = warehouse;
                paramvalueInn[7] = rowNo;
                paramvalueInn[8] = mode;

                SqlDbType[] paramtypeInn = new SqlDbType[9];
                paramtypeInn[0] = SqlDbType.NVarChar;
                paramtypeInn[1] = SqlDbType.NVarChar;
                paramtypeInn[2] = SqlDbType.DateTime;
                paramtypeInn[3] = SqlDbType.NVarChar;
                paramtypeInn[4] = SqlDbType.DateTime;
                paramtypeInn[5] = SqlDbType.NVarChar;
                paramtypeInn[6] = SqlDbType.NVarChar;
                paramtypeInn[7] = SqlDbType.Int;
                paramtypeInn[8] = SqlDbType.NVarChar;

                SQLServer dbSavePO = new SQLServer(Global.GetConnectionString());
                DataSet dsRes = dbSavePO.SelectRecords("spSavePODetails", paramnameInn, paramvalueInn, paramtypeInn);
                if (dsRes != null && dsRes.Tables.Count > 0)
                {
                    if (dsRes.Tables[0].Rows.Count > 0)
                    {
                        string detailsID = dsRes.Tables[0].Rows[0][0].ToString();
                        #region insert uld numbers
                        try
                        {
                                       DataSet gv = (DataSet)Session["innerdata"];
                                       if (gv.Tables.Count > 0)
                                       {

                                           if (gv.Tables[rowNo].Rows.Count > 0)
                                           {
                                               for (int j = 0; j < gv.Tables[rowNo].Rows.Count; j++)
                                               {
                                                   string uldno = gv.Tables[rowNo].Rows[j][0].ToString();
                                                   bool res = insertPOULD(detailsID, uldno, delivery, warehouse, username, datetime, mode, j, pono);
                                                   if (res == false)
                                                   {
                                                       return resD = false;
                                                   }
                                               }
                                               resD = true;
                                           }

                                       }


                            #region old working but only grid which is in front is store, pageindex item not store

                            //GridView gv = ((GridView)GVOrdDetails.Rows[rowNo].FindControl("grdChildGridNormal"));
                            //if (gv.Rows.Count > 0)
                            //{
                            //    for (int j = 0; j < gv.Rows.Count; j++)
                            //    {
                            //        string uldno = ((Label)gv.Rows[j].FindControl("chldgrdDelivery")).Text;
                            //        bool res = insertPOULD(detailsID, uldno, delivery, warehouse, username, datetime,mode,j,pono);
                            //        if (res == false)
                            //        {
                            //         return  resD = false;
                            //        }
                            //    }
                            //} 
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            resD = false;
                            lblerror.Text = ex.Message;
                            lblerror.ForeColor = Color.Red;
                        }
                        #endregion
                    }
                    else
                    {
                        resD = false;
                    }
                }
                else
                {
                    resD = false;
                }
            }
            catch (Exception ex)
            {
                resD = false;
                lblerror.Text = ex.Message;
                lblerror.ForeColor = Color.Red;
            }
            #endregion
            return resD;
        }

        protected bool insertPOULD(string detailsID,string uldno,string expDeldate,string warehouse,string username,string datetime,string mode,int rowno,string PONO)
        {
            bool resB = false;
            try
            {
                string[] paramnameInnULD = new string[9];
                paramnameInnULD[0] = "PODetailsID";
                paramnameInnULD[1] = "ULDNumber";
                paramnameInnULD[2] = "EXPDeliveryDate";
                paramnameInnULD[3] = "EXPDeliveryWHCode";
                paramnameInnULD[4] = "createdBy";
                paramnameInnULD[5] = "createdOn";
                paramnameInnULD[6] = "mode";
                paramnameInnULD[7] = "rowno";
                paramnameInnULD[8] = "pono";

                object[] paramvalueInnULD = new object[9];
                paramvalueInnULD[0] = detailsID;
                paramvalueInnULD[1] = uldno;
                paramvalueInnULD[2] = Convert.ToDateTime(expDeldate);
                paramvalueInnULD[3] = warehouse;
                paramvalueInnULD[4] = username;
                paramvalueInnULD[5] = Convert.ToDateTime(datetime);
                paramvalueInnULD[6] = mode;
                paramvalueInnULD[7] = rowno;
                paramvalueInnULD[8] = PONO;

                SqlDbType[] paramtypeInnULD = new SqlDbType[9];
                paramtypeInnULD[0] = SqlDbType.BigInt;
                paramtypeInnULD[1] = SqlDbType.NVarChar;
                paramtypeInnULD[2] = SqlDbType.DateTime;
                paramtypeInnULD[3] = SqlDbType.NVarChar;
                paramtypeInnULD[4] = SqlDbType.NVarChar;
                paramtypeInnULD[5] = SqlDbType.DateTime;
                paramtypeInnULD[6] = SqlDbType.NVarChar;
                paramtypeInnULD[7] = SqlDbType.Int;
                paramtypeInnULD[8] = SqlDbType.NVarChar;

                SQLServer dbPOULD = new SQLServer(Global.GetConnectionString());
                bool resULD = dbPOULD.InsertData("spInsertPOULDs", paramnameInnULD, paramtypeInnULD, paramvalueInnULD);
                if (resULD == true)
                {
                    resB = true;
                }
            }
            catch (Exception ex)
            {
                resB = false;
                lblerror.Text = ex.Message;
                lblerror.ForeColor = Color.Red;
            }
            return resB;
        }
     
        protected void grdChildGridEdit_PageIndexChanging(object sender,GridViewPageEventArgs e)
        {
            try
            {
                GridView btn = (GridView)sender;

                //Get the row that contains this button
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;

                //Get rowindex
                int rowindex = gvr.RowIndex;
                //GridView gv1 = new GridView();
                //gv1 = (GridView)row.FindControl("grdChildGridNormal");
                btn.PageIndex = e.NewPageIndex;
                // innergrid = (DataSet)Session["innerdata"];
                // gv1.DataSource = innergrid.Tables[Convert.ToInt32(Session["findcnt"].ToString())];
                btn.DataSource = ((DataSet)Session["innerdata"]).Tables[rowindex];

                btn.DataBind();
            }
            catch (Exception ex)
            {
                
                lblerror.Text = ex.Message;
                lblerror.ForeColor = Color.Red;
            }
        }

        protected bool checkDelivered(string pono)
        {
            bool res = false;
            try
            {
                string[] paramname = new string[1];
                paramname[0] = "pono";

                object[] paramvalue = new object[1];
                paramvalue[0] = pono;

                SqlDbType[] paramtype = new SqlDbType[1];
                paramtype[0] = SqlDbType.NVarChar;

                SQLServer dbPONoDel = new SQLServer(Global.GetConnectionString());
                DataSet dsPOdel = dbPONoDel.SelectRecords("spchkIsDelivered", paramname, paramvalue, paramtype);
                if (dsPOdel != null && dsPOdel.Tables.Count > 0)
                {
                    if (dsPOdel.Tables[0].Rows.Count > 0)
                    {
                        if (dsPOdel.Tables[0].Rows[0][0].ToString() == "0")
                        {
                            res = true;
                        }
                        else
                        {
                            res = false;
                        }
                    }
                    else
                        res = false;
                }
                else
                    res = false;
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string PONo = string.Empty;
            lblerror.Text = "";
            if (Session["chekAppend"] == null)
            {
                lblerror.Text = "You did not modify anything, so cannot perform Save operation.";
                lblerror.ForeColor = Color.Red;
                return;
            }
            try
            {
                if (Session["PageMode"].ToString() == "Create")
                {
                    #region Create Save Block
                    try
                    {
                        if (GVOrdDetails.Rows.Count > 0)
                        {
                            if (txtPONo.Text.Trim() == "")
                            {
                                #region getPOMax and Create PO no for next
                                string[] paramname = new string[5];
                                paramname[0] = "podate";
                                paramname[1] = "manuID";
                                paramname[2] = "regionID";
                                paramname[3] = "createdBy";
                                paramname[4] = "createdOn";

                                object[] paramvalue = new object[5];
                                paramvalue[0] = Convert.ToDateTime(txtPODate.Text.Trim());
                                paramvalue[1] = ddlManu.SelectedValue;
                                paramvalue[2] = ddlRegion.SelectedValue;
                                paramvalue[3] = Session["UserName"].ToString();
                                paramvalue[4] = Convert.ToDateTime(System.DateTime.Now.ToString());

                                SqlDbType[] paramtype = new SqlDbType[5];
                                paramtype[0] = SqlDbType.DateTime;
                                paramtype[1] = SqlDbType.BigInt;
                                paramtype[2] = SqlDbType.BigInt;
                                paramtype[3] = SqlDbType.NVarChar;
                                paramtype[4] = SqlDbType.DateTime;

                                SQLServer dbPONo = new SQLServer(Global.GetConnectionString());
                                DataSet dsPO = dbPONo.SelectRecords("spgetMaxPOandcreatePONo", paramname, paramvalue, paramtype);
                                if (dsPO != null && dsPO.Tables.Count > 0)
                                {
                                    if (dsPO.Tables[0].Rows.Count > 0)
                                    {
                                        PONo = dsPO.Tables[0].Rows[0][0].ToString();
                                        txtPONo.Text = PONo.Trim();
                                    }
                                    else
                                    {
                                        txtPONo.Text = "";
                                        return;
                                    }
                                }
                                else
                                {
                                    txtPONo.Text = "";
                                    return;
                                }
                                #endregion
                            }
                            if (txtPONo.Text.Trim() != "")
                            {
                                PONo = txtPONo.Text.Trim();
                                bool isdel = checkDelivered(PONo);
                                if (isdel == false)
                                {
                                    lblerror.Text = "PO can't be updated as 1 or more items are delivered";
                                    lblerror.ForeColor = Color.Red;
                                    return;
                                }

                                #region insert PO details
                                try
                                {
                                    for (int i = 0; i < GVOrdDetails.Rows.Count; i++)
                                    {
                                        string uldtype = ((Label)GVOrdDetails.Rows[i].FindControl("grdULDType")).Text;
                                        string uldpart = ((Label)GVOrdDetails.Rows[i].FindControl("grdULDPartNo")).Text;
                                        string qty = ((Label)GVOrdDetails.Rows[i].FindControl("grdQuantity")).Text;
                                        string delivery = ((Label)GVOrdDetails.Rows[i].FindControl("grdDelivery")).Text;
                                        string warehouse = ((Label)GVOrdDetails.Rows[i].FindControl("grdWareHouse")).Text;

                                        bool result = inserOrderDet(uldtype, uldpart, qty, delivery, warehouse, i, PONo, Session["UserName"].ToString(), System.DateTime.Now.ToString(), "create");
                                        if (result == false)
                                        {
                                            txtPONo.Text = "";
                                            rollbackPO(PONo);
                                            return;
                                        }
                                    }
                                    lblerror.Text = "PO Created Successfully";
                                    lblerror.ForeColor = Color.Green;
                                    btnCancel.Visible = true;
                                }
                                catch (Exception ex)
                                {
                                    txtPONo.Text = "";
                                    lblerror.Text = ex.Message;
                                    lblerror.ForeColor = Color.Red;
                                    rollbackPO(PONo);

                                    return;
                                }
                                #endregion
                            }
                            else
                            {
                                //  Cannot create po some problem occur   
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        txtPONo.Text = "";
                        lblerror.Text = ex.Message;
                        lblerror.ForeColor = Color.Red;
                        return;
                    }
                    finally
                    {
                        //   if (error == 1)
                        //     rollbackPO(PONo);  // why error=1, bcoz v need to delete when there is any entry in one of the table but if there is error during first insert so why to execute rollback query
                    }
                    #endregion
                }
                else if (Session["PageMode"].ToString() == "Search")
                {
                    #region Update Block
                    try
                    {
                        if (GVOrdDetails.Rows.Count > 0)
                        {
                            PONo = txtPONo.Text.Trim();
                            if (PONo.Trim() == "")
                            {
                                // po cant not be empty
                                lblerror.Text = "PO Number required.";
                                lblerror.ForeColor = Color.Red;
                                return;
                            }
                            #region check PO existance
                            try
                            {
                                string[] paramnameRB = new string[1];
                                paramnameRB[0] = "pono";

                                object[] paramvalueRB = new object[1];
                                paramvalueRB[0] = PONo;

                                SqlDbType[] paramtypeRB = new SqlDbType[1];
                                paramtypeRB[0] = SqlDbType.NVarChar;

                                SQLServer dbRBPO = new SQLServer(Global.GetConnectionString());
                                DataSet res = dbRBPO.SelectRecords("spupdateStatusPO", paramnameRB, paramvalueRB, paramtypeRB);
                                if (res != null && res.Tables.Count > 0)
                                {
                                    if (res.Tables[0].Rows.Count > 0)
                                    {
                                        if (res.Tables[0].Rows[0][0].ToString() == "0")
                                        {
                                            lblerror.Text = "Error during update";
                                            lblerror.ForeColor = Color.Red;
                                        }
                                        else if (res.Tables[0].Rows[0][0].ToString() == "2")
                                        {
                                            lblerror.Text = "PO does not exist";
                                            lblerror.ForeColor = Color.Red;
                                        }
                                        else
                                        {
                                            bool isdel = checkDelivered(PONo);
                                            if (isdel == false)
                                            {
                                                lblerror.Text = "PO can't be updated, as 1 or more items are delivered";
                                                lblerror.ForeColor = Color.Red;
                                                return;
                                            }
                                            #region remove old and insert new data in tables of PO
                                            if (GVOrdDetails.Rows.Count > 0)
                                            {
                                                #region insert PO details
                                                try
                                                {
                                                    for (int i = 0; i < GVOrdDetails.Rows.Count; i++)
                                                    {
                                                        string uldtype = ((Label)GVOrdDetails.Rows[i].FindControl("grdULDType")).Text;
                                                        string uldpart = ((Label)GVOrdDetails.Rows[i].FindControl("grdULDPartNo")).Text;
                                                        string qty = ((Label)GVOrdDetails.Rows[i].FindControl("grdQuantity")).Text;
                                                        string delivery = ((Label)GVOrdDetails.Rows[i].FindControl("grdDelivery")).Text;
                                                        string warehouse = ((Label)GVOrdDetails.Rows[i].FindControl("grdWareHouse")).Text;

                                                        bool result = inserOrderDet(uldtype, uldpart, qty, delivery, warehouse, i, PONo,
                                                            Session["UserName"].ToString(), System.DateTime.Now.ToString(),
                                                            "search");
                                                        if (result == false)
                                                        {
                                                            return;
                                                        }
                                                    }
                                                    lblerror.Text = "PO Updated Successfully";
                                                    lblerror.ForeColor = Color.Green;

                                                }
                                                catch (Exception ex)
                                                {
                                                    lblerror.Text = "Error : " + ex.Message;
                                                    return;
                                                }
                                                #endregion
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                lblerror.Text = "Error : " + ex.Message;
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        lblerror.Text = "Error : " + ex.Message;
                    }
                    finally
                    {
                    }
                    #endregion
                }
                Session["chekAppend"] = null;
                cfc.FillAllComboBoxes("tblULDTypeMaster", "Select", ddlAddUldType);
                search();

            }
            catch (Exception)
            {
            }
        }
      
        #region roll back whole po transaction in case of any error
        protected bool rollbackPO(string pono)
        {
            bool rechk = false;
            try
            {
                string[] paramnameRB = new string[1];
                paramnameRB[0] = "pono";

                object[] paramvalueRB = new object[1];
                paramvalueRB[0] = pono;

                SqlDbType[] paramtypeRB = new SqlDbType[1];
                paramtypeRB[0] = SqlDbType.NVarChar;

                SQLServer dbRBPO = new SQLServer(Global.GetConnectionString());
                bool res = dbRBPO.UpdateData("sprollbackPO", paramnameRB, paramtypeRB, paramvalueRB);
                if (res != true)
                {
                    // maintain error into txt file
                    rechk = false;
                    string strError = "Error during Rollback, PONo : " + pono + " on " + System.DateTime.Now.ToString();
                    string filepath = Server.MapPath("~/Errorlog.txt");
                    clsLog.WriteLog( strError, filepath);
                }
                else
                {
                    rechk = true;
                }
            }
            catch (Exception ex)
            {
                // maintain error into txt file
                rechk = false;
                string strError = "Error during Rollback, PONo : " + pono + " on " + System.DateTime.Now.ToString();
                string filepath = Server.MapPath("~/Errorlog.txt");
                clsLog.WriteLog("Error during Rollback, PONo : "+pono+" on "+System.DateTime.Now.ToString());
            }
            return rechk;
        }
        
        #endregion

        protected void GVOrdDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                lblerror.Text = "";
                DataTable dt;
                dt = (DataTable)Session["NEWPO"];
                if (Session["PageMode"].ToString() == "Search")
                {
                    if (dt.Rows.Count == 1)
                    {
                        lblerror.Text = "cannot allow to delete full po one by one, if u want then cancel po";
                        return;
                    }
                }

                Session["findcnt"] = 0;
                DataSet ds;
                ds = (DataSet)Session["innerdata"];
                ds.Tables.RemoveAt(e.RowIndex);
                ds.AcceptChanges();
                Session["innerdata"] = ds;
                //DataTable dt;
                //dt = (DataTable)Session["NEWPO"];
                dt.Rows[e.RowIndex].Delete();
                dt.AcceptChanges();
                GVOrdDetails.DataSource = dt;
                GVOrdDetails.DataBind();
                Session["NEWPO"] = dt;
                if (Session["PageMode"].ToString() == "Create")
                {
                    if (dt.Rows.Count == 0)
                    {
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                    }
                }
                Session["chekAppend"] = "0";
                btnSave.Focus();
            }
            catch (Exception ex)
            {
                lblerror.Text = "Error : " + ex.Message; 
            }
        }

        #region clear page section
        protected void btnClear_Click(object sender, EventArgs e)
        {
            clearpage();
        }

        protected void clearpage()
        {
            try
            {
                Session["findcnt"] = 0;
                Session["NEWPO"] = null;
                Session["innerdata"] = null;
                Session["idofEdit"] = null;
                lblerror.Text = "";
                txtAddExpDelDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                txtPODate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                ddlAddUldType.SelectedIndex = 0;
                ddlManu.SelectedIndex = 0;
                ddlRegion.SelectedIndex = 0;
                ddlULDPart.SelectedIndex = 0;
                ddlWH.SelectedIndex = 0;
                txtCount.Text = "";
                txtFrom.Text = "";
                txtLastAlloc.Text = "0";
                txtTo.Text = "";
                txtPONo.Text = "";
                btnCancel.Visible = false;
                btnSave.Visible = false;
                if (Session["PageMode"].ToString() == "Create")
                {
                    txtPONo.Text = "";
                    txtPONo.Enabled = false;
                    btnSearch.Visible = false;
                    GVOrdDetails.Columns[6].Visible = false;
                    GVOrdDetails.Columns[8].Visible = false;

                    ddlManu.Focus();
                }
                else if (Session["PageMode"].ToString() == "Search")
                {
                    txtPONo.Enabled = true;
                    btnSearch.Visible = true;
                    GVOrdDetails.Columns[6].Visible = false;
                    GVOrdDetails.Columns[8].Visible = true;

                    txtPONo.Focus();
                }
                GVOrdDetails.DataSource = null;
                GVOrdDetails.DataBind();
                Session["chekAppend"] = null;
            }
            catch (Exception ex)
            {
            }
        } 
        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPONo.Text.Trim() != "")
                {
                    bool res = rollbackPO(txtPONo.Text.Trim());
                    if (Session["PageMode"].ToString() == "Create")
                    {
                        if (res == true)
                            btnCancel.Visible = false;
                        else
                            btnCancel.Visible = true;
                    }
                    if (res == true)
                        clearpage();
                    else
                    {
                        lblerror.Text = "Error during delete PO";
                        btnCancel.Focus();
                    }
                }
                else
                {
                    lblerror.Text = "PONo Required";
                    txtPONo.Focus();
                }
                Session["chekAppend"] = null;
                cfc.FillAllComboBoxes("tblULDTypeMaster", "Select", ddlAddUldType);
            }
            catch (Exception ex)
            {
                lblerror.Text = "Error : " + ex.Message;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            search();
        }
        
        public void search()
        {
            lblerror.Text = "";
            if (txtPONo.Text.Trim() != "")
            {
                Session["idofEdit"] = null;
                Session["NEWPO"] = null;
                Session["innerdata"] = null;
                Session["findcnt"] = 0;
                Session["chekAppend"] = null;
                try
                {
                    string[] paramnameRB = new string[1];
                    paramnameRB[0] = "pono";

                    object[] paramvalueRB = new object[1];
                    paramvalueRB[0] = txtPONo.Text.Trim();

                    SqlDbType[] paramtypeRB = new SqlDbType[1];
                    paramtypeRB[0] = SqlDbType.NVarChar;

                    SQLServer dbRBPO = new SQLServer(Global.GetConnectionString());
                    DataSet res = dbRBPO.SelectRecords("spgetPODetails", paramnameRB, paramvalueRB, paramtypeRB);
                    if (res != null && res.Tables.Count > 0)
                    {
                        if (res.Tables[0].Rows.Count > 0)
                        {
                            btnCancel.Visible = false;
                            btnSave.Visible = false;
                            
                            if (res.Tables[0].Rows[0][0].ToString() == "1")
                            {
                                Session["NEWPO"] = (DataTable)res.Tables[1];
                                DataSet ds = new DataSet();

                                for (int i = 0; i < res.Tables[2].Rows.Count; i++)
                                {
                                    DataTable dt = new DataTable();
                                    dt.Columns.Add("ULDRan");

                                    DataRow[] result = res.Tables[3].Select("PODetailsID = " + res.Tables[2].Rows[i][0].ToString());
                                    foreach (DataRow row in result)
                                    {
                                        dt.Rows.Add(row[0]);
                                    }
                                    ds.Tables.Add(dt);
                                }
                                Session["innerdata"] = ds;
                                GVOrdDetails.DataSource = res.Tables[1];
                                GVOrdDetails.DataBind();
                                btnCancel.Visible = true;
                                btnSave.Visible = true;
                                btnSave.Focus();
                            }
                            else if (res.Tables[0].Rows[0][0].ToString() == "2")
                            {
                                lblerror.Text = "PONo not Exist";
                                txtPONo.Focus();
                                return;
                            }
                            else
                            {
                                lblerror.Text = "Error during data fetching, Try again";
                                btnSearch.Focus();
                                return;

                            }
                            if (res.Tables[4].Rows.Count > 0)
                            {
                                txtPODate.Text = res.Tables[4].Rows[0][0].ToString();
                                ddlManu.SelectedValue = res.Tables[4].Rows[0][2].ToString();
                                ddlRegion.SelectedValue = res.Tables[4].Rows[0][1].ToString();

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblerror.Text = "Error : " + ex.Message;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
            }
            else
            {
                lblerror.Text = "PONo Required";
            }
        }

        protected void ddlAddUldType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string getval = ddlAddUldType.SelectedValue.ToString();
                if (getval.Contains('-'))
                {
                    string[] spl = getval.Split('-');
                    txtLastAlloc.Text = spl[1].ToString();
                }
                else
                {
                    txtLastAlloc.Text = "0";
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }

        protected void GVOrdDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                Session["idofEdit"] = null;
                string oldULDType = ((Label)GVOrdDetails.Rows[e.NewEditIndex].FindControl("grdULDType")).Text;
                if (oldULDType.Trim() != "")
                {
                    rblType.SelectedIndex = 0;
                    rblType_SelectedIndexChanged(sender, e);
                    for (int i = 0; i < ddlAddUldType.Items.Count; i++)
                    {
                        if (ddlAddUldType.Items[i].Text == oldULDType.Trim())  // for temp.
                        {
                            ddlAddUldType.SelectedIndex = i;
                            break;
                        }
                    }
                    GridView gv = ((GridView)GVOrdDetails.Rows[e.NewEditIndex].FindControl("grdChildGridNormal"));
                    if (gv.Rows.Count > 1)
                    {
                        rblSM.SelectedIndex = 1;
                        rblSM_SelectedIndexChanged(sender, e);
                       string uldval =((Label)(gv.Rows[0].FindControl("chldgrdDelivery"))).Text;

                       txtTo.Text = (Convert.ToDouble(((Label)(gv.Rows[0].FindControl("chldgrdDelivery"))).Text.ToString().Substring(3, 5)) + (Convert.ToDouble(((Label)GVOrdDetails.Rows[e.NewEditIndex].FindControl("grdQuantity")).Text.Trim())-1)).ToString();
                       // txtTo.Text = (Convert.ToDouble(gv.Rows[0].Cells[0].Text.ToString().Substring(3, 5)) + (Convert.ToDouble(gv.Rows[0].Cells[2].Text.ToString()) - 1)).ToString();
                    }
                    else
                    {
                        rblSM.SelectedIndex = 0;
                        rblSM_SelectedIndexChanged(sender, e);
                    }
                   // txtFrom.Text = Convert.ToDouble(gv.Rows[0].Cells[0].Text.ToString().Substring(3, 5)).ToString();
                    txtFrom.Text = Convert.ToDouble(((Label)(gv.Rows[0].FindControl("chldgrdDelivery"))).Text.ToString().Substring(3, 5)).ToString();

                }
                else
                {
                    rblType.SelectedIndex = 1;
                    rblType_SelectedIndexChanged(sender, e);
                    txtCount.Text = ((Label)GVOrdDetails.Rows[e.NewEditIndex].FindControl("grdQuantity")).Text.Trim();
                    for (int i = 0; i < ddlULDPart.Items.Count; i++)
                    {
                        if (ddlULDPart.Items[i].Text == ((Label)GVOrdDetails.Rows[e.NewEditIndex].FindControl("grdULDPartNo")).Text.Trim())  // for temp.
                        {
                            ddlULDPart.SelectedIndex = i;
                            break;
                        }
                    }
                
                }
               
                txtAddExpDelDate.Text = ((Label)GVOrdDetails.Rows[e.NewEditIndex].FindControl("grdDelivery")).Text.Trim();
                for (int i = 0; i < ddlWH.Items.Count; i++)
                {
                    if (ddlWH.Items[i].Text == ((Label)GVOrdDetails.Rows[e.NewEditIndex].FindControl("grdWareHouse")).Text.Trim())  // for temp.
                    {
                        ddlWH.SelectedIndex = i;
                        break;
                    }
                }

                Session["idofEdit"] = ((Label)GVOrdDetails.Rows[e.NewEditIndex].FindControl("grdisRec")).Text.Trim();

                Session["findcnt"] = 0;
                DataTable dt;
                dt = (DataTable)Session["NEWPO"];
                DataSet ds;
                ds = (DataSet)Session["innerdata"];
                ds.Tables.RemoveAt(e.NewEditIndex);
                ds.AcceptChanges();
                Session["innerdata"] = ds;
                dt.Rows[e.NewEditIndex].Delete();
                dt.AcceptChanges();
                Session["NEWPO"] = dt;
                GVOrdDetails.DataSource = dt;
                GVOrdDetails.DataBind();
                if (dt.Rows.Count == 0)
                {
                    btnSave.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
                lblerror.ForeColor = Color.Red;
                 Session["idofEdit"]=null;
            }
        }

    }
}
