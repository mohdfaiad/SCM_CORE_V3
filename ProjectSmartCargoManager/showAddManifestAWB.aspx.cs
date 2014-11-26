using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

namespace ProjectSmartCargoManager
{
    public partial class showAddManifestAWB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                try
                {
                    LoadGridSchedule();
                    DataTable dt = (DataTable)Session["AWBdata"];
                    DataTable dtCurrentTable = (DataTable)Session["AWBdata"];
                    grdAWBs.DataSource = dtCurrentTable;
                     grdAWBs.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        DataRow drCurrentRow = null;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            drCurrentRow = dtCurrentTable.NewRow();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text = dt.Rows[i][0].ToString();

                            ((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text = dt.Rows[i][1].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = dt.Rows[i][2].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = dt.Rows[i][3].ToString();
                            ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = dt.Rows[i][4].ToString();

                            ViewState["CurrentTable"] = dtCurrentTable;

                            
                        }
                       // grdAWBs.DataSource = dtCurrentTable;
                       // grdAWBs.DataBind();
                        
                    }
                   // grdAWBs.DataSource = dt;
                  //  grdAWBs.DataBind();


                   // myButton.Attributes.Add("onClick", "close();");


                }
                catch (Exception ex)
                {

                }

            }
        }


        #region Loadgrid Intial Row
        private void LoadGridSchedule()
        {

            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AWBNo";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Pieces";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Weight";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AvlPCS";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "AvlWgt";
            myDataTable.Columns.Add(myDataColumn);

            
            DataRow dr;
            dr = myDataTable.NewRow();
            //dr["RowNumber"] = 1;
            dr["AWBNo"] = "";//"5";
            dr["Pieces"] = "";// "5";
            dr["Weight"] = "";
            dr["AvlPCS"] = "";
            dr["AvlWgt"] = "";
            
            myDataTable.Rows.Add(dr);
            ViewState["CurrentTable"] = myDataTable;
            //Bind the DataTable to the Grid

            grdAWBs.DataSource = null;
            grdAWBs.DataSource = myDataTable;
            grdAWBs.DataBind();
        }
        #endregion 

        public bool IsInputValid()
        {
            try
            {
                int  pcscount = 0, Weight = 0;

                for (int i = 0; i < grdAWBs.Rows.Count; i++)
                {
                    try
                    {
                        pcscount = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtPCS")).Text);

                    }
                    catch
                    {

                        LBLStatus.Text = "Enter valid PCS row:" + (i+1);
                        LBLStatus.ForeColor = Color.Red;
                        ((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text = "";
                        return false;
                    }

                    try
                    {
                        Weight = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text);

                    }
                    catch
                    {
                        LBLStatus.Text = "Enter valid Weight in row:" + (i+1);
                        LBLStatus.ForeColor = Color.Red;
                        ((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text = "";
                        return false;
                    }

                    try
                    {
                        if (((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text.Trim() == "")
                        {
                            LBLStatus.Text = "Fill PCS count for row:" + (i+1);
                            LBLStatus.ForeColor = Color.Red;
                            return false;
                        }
                        pcscount = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text);

                    }
                    catch
                    {
                        LBLStatus.Text = "Enter valid pcscount row:" + i;
                        LBLStatus.ForeColor = Color.Red;
                        ((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text = "";
                        return false;
                    }

                    DataTable dtAWB=(DataTable)Session["AWBdata"];
                     pcscount = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text);

                     try
                     {
                         for (int j = 0; j < dtAWB.Rows.Count; j++)
                         {
                             if (((TextBox)grdAWBs.Rows[i].FindControl("txtAWBno")).Text == dtAWB.Rows[j][0].ToString())
                             {
                                 if (int.Parse(dtAWB.Rows[j][1].ToString()) < pcscount)
                                 {
                                     LBLStatus.Text = "Enter valid pcscount row:" + (i+1);
                                     LBLStatus.ForeColor = Color.Red;
                                     ((TextBox)grdAWBs.Rows[i].FindControl("txtPcs")).Text = dtAWB.Rows[j][1].ToString();
                                     return false;

                                 }
                                 else
                                 {

                                     int AvlPCS = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text);
                                     AvlPCS = int.Parse(dtAWB.Rows[j][1].ToString()) - pcscount;
                                     ((TextBox)grdAWBs.Rows[i].FindControl("txtAvlPCS")).Text = AvlPCS.ToString();

                                     int AvlWeight = (int.Parse(dtAWB.Rows[j][2].ToString())) / (int.Parse(dtAWB.Rows[j][1].ToString()));
                                     AvlWeight=AvlWeight*AvlPCS;
                                     ((TextBox)grdAWBs.Rows[i].FindControl("txtAwlWeight")).Text = AvlWeight.ToString();
                                     int ActWeight = int.Parse(((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text);
                                     ActWeight = ActWeight - AvlWeight;
                                     ((TextBox)grdAWBs.Rows[i].FindControl("txtweight")).Text = ActWeight.ToString();
                                 }

                             }
                         }
                         //grdAWBs.r
                         //grdAWBs.DataBind();
                     }
                     catch (Exception ex)
                     {

                     }
                   
                    //if (pcscount > int.Parse(LBLPcsCount.Text))
                    //{
                    //    LBLStatus.Text = "Total pcs count should be smaller than " + LBLPcsCount.Text;
                    //    LBLStatus.ForeColor = Color.Red;
                    //    return false;
                    //}

                }

                LBLStatus.Text = "";
                return true;
            }
            catch (Exception ex)
            {
                LBLStatus.Text = "Error : while checking input validation.";
                LBLStatus.ForeColor = Color.Red;
                return false;
            }
        }

        protected void txtPcs_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (!IsInputValid())
                {
                  //  TXTVolume.Text = "";
                   // TXTTotal.Text = "";
                    return;
                }


                int PCS = 0;
                decimal Weight = 0;

                //TextBox txt=(TextBox)grdAWBs.Rows[](FindControl("").
               
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "Error: " + ex.Message + "", true);
            }
        }




        protected void btnAddManifest_Click(object sender, EventArgs e)
        {

            Session["AWBSplitDalta"] = grdAWBs.DataSource;


            DataTable dt = new DataTable();
            DataTable DTAWBDetails = new DataTable();

            DTAWBDetails.Columns.Add("AWBNo");
            DTAWBDetails.Columns.Add("Pieces");
            DTAWBDetails.Columns.Add("Weight");
            DTAWBDetails.Columns.Add("AvlPCS");
            DTAWBDetails.Columns.Add("AvlWgt");


            for (int j = 0; j < grdAWBs.Rows.Count; j++)
            {

                DataRow dr;

                //GridViewRow row = gdvULDLoadPlanAWB.Rows[j];
                //dr = dt.NewRow();

                // DTAWBDetails.Rows.Add(dtData.Rows[j][0], dtData.Rows[j][1], dtData.Rows[j][2], dtData.Rows[j][3], dtData.Rows[j][4]);
                dr = DTAWBDetails.NewRow();
                dr[0] = ((TextBox)grdAWBs.Rows[j].FindControl("txtAWBno")).Text;
                dr[1] = ((TextBox)grdAWBs.Rows[j].FindControl("txtPcs")).Text;
                dr[2] = ((TextBox)grdAWBs.Rows[j].FindControl("txtweight")).Text;
                dr[3] = ((TextBox)grdAWBs.Rows[j].FindControl("txtAvlPCS")).Text;
                dr[4] = ((TextBox)grdAWBs.Rows[j].FindControl("txtAwlWeight")).Text;
                DTAWBDetails.Rows.Add(dr);




            }
            Session["AWBSplitData"] = DTAWBDetails;
            Session["Username"] = "QID";

            //  Response.Redirect("frmExportManifest.aspx", false);
            //Response.Write("window.open('frmExportManifest.aspx','_blank')");

            //Implement Your logic here.....  
            //..............................  
            //now refresh parent page and close this window  
            ////string script = "this.window.opener.location=this.window.opener.location;this.window.close();";
            ////if (!ClientScript.IsClientScriptBlockRegistered("REFRESH_PARENT"))
            ////    ClientScript.RegisterClientScriptBlock(typeof(string), "REFRESH_PARENT", script, true);



            //frmExportManifest tempDialog = new frmExportManifest();
            

            
            //tempDialog.ShowSplitAWBGrid();

          //  ((frmExportManifest)Page.Parent).ShowSplitAWBGrid();

          //  GridView gdv = this.Parent.FindControl("gdvULDLoadPlanAWB") as GridView;
          //int k= gdv.Rows.Count; //u can set ur value here 



           // Page.RegisterStartupScript("as", "<script language='javascript'>window.close();</script>");



       //     ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "onAddNew", "window.opener.location.href = 'localhost:64813/frmExportManifest.aspx?fromChild=1';self.close();", false);
         //   ClientScript.RegisterStartupScript(Page.GetType, "Javascript", "PassValues();", true);


           

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
           // Response.Redirect("frmExportManifest.aspx", false);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void myButton_Click(object sender, EventArgs e)
        {

        }

        protected void btnShowEAWB_Click(object sender, EventArgs e)
        {

        }
    }
}
