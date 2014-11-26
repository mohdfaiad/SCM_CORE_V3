using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BAL;
using System.Collections;
 
//25-7-2012
namespace ProjectSmartCargoManager
{
    public partial class AircraftEquipment : System.Web.UI.Page
    {
        AircraftBAL objBAL = new AircraftBAL();
        int count;

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadDataGrid();


            }

        }
        #endregion PageLoad
        #region Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            object[] AcInfo = new object[16];
            int i = 0;
            // int valid = 1;
            string valid;
            try
            {
                AcInfo.SetValue(txtManuf.Text, i);
                i++;
                AcInfo.SetValue(txtAcType.Text, i);
                i++;
                AcInfo.SetValue(txtAcVer.Text, i);
                i++;
                AcInfo.SetValue(txtCount.Text, i);
                i++;
                AcInfo.SetValue(txtPCap.Text, i);
                i++;
                AcInfo.SetValue(txtLWt.Text, i);
                i++;
                AcInfo.SetValue(txtCarCap.Text, i);
                i++;
                AcInfo.SetValue(txtMTOW.Text, i);
                i++;
                AcInfo.SetValue(txtRwt.Text, i);
                i++;
                //Swapnil 25-7-12
                try
                {
                    double inch;
                    double base1;
                    double height;
                    double vollen;
                    //double volbre;
                    //double volhei;

                    if (ddldimension.SelectedItem.Text == "Inch")
                    {
                        inch = (Convert.ToDouble(txtRl.Text) * (2.54));
                        base1 = (Convert.ToDouble(txtRb.Text) * (2.54));
                        height = (Convert.ToDouble(txtRh.Text) * (2.54));
                        AcInfo.SetValue(inch, i);
                        i++;
                        AcInfo.SetValue(base1, i);
                        i++;
                        AcInfo.SetValue(height, i);
                        i++;
                    }
                    else
                    {

                        AcInfo.SetValue(txtRl.Text, i);
                        i++;
                        AcInfo.SetValue(txtRb.Text, i);
                        i++;
                        AcInfo.SetValue(txtRh.Text, i);
                        i++;
                    }
                    if (ddlVolMeasure.SelectedItem.Text == "Inch")
                    {
                        vollen = (Convert.ToDouble(txtVol.Text) / (0.061024));
                        decimal d = Convert.ToDecimal(vollen);
                        decimal rounded = Decimal.Round(d, 2);
                        //string vol = Convert.ToString(rounded);
                        //volbre = (Convert.ToDouble(txtVolBreadth.Text) * (2.54));
                        //volhei = (Convert.ToDouble(txtVolHeight.Text) * (2.54));
                        //i++;
                        AcInfo.SetValue(rounded, i);
                        i++;
                        //i++;
                        //AcInfo.SetValue(volbre, i);
                        //i++;
                        //AcInfo.SetValue(volhei, i);
                    }
                    else
                    {

                        AcInfo.SetValue(txtVol.Text, i);
                        i++;
                        //i++;
                        //AcInfo.SetValue(txtVolBreadth.Text, i);
                        //i++;
                        //AcInfo.SetValue(txtVolHeight.Text, i);
                    }
                    AcInfo.SetValue(ddldimension.SelectedItem.Text, i);
                    i++;
                    AcInfo.SetValue(ddlVolMeasure.SelectedItem.Text, i);
                    i++;
                }
                catch (Exception ex)
                { }
                //
                AcInfo.SetValue(ddlAircraftIdentity.SelectedItem.Text, i);
                i++;
                SaveEquipment();

                valid = "";
                valid = objBAL.SaveAircraftEquipment(AcInfo);
                if (valid == "fail")
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = "Aircraft Equipment insertion failed-Aircraft already exist ";
                }

                if (valid == "inserted")
                {
                    ClearFunction();
                    LoadDataGrid();
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Text = "Aircraft Equipment inserted successfuly";
                }
                if (valid == "updated")
                {
                    ClearFunction();
                    LoadDataGrid();
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Text = "Aircraft Equipment updated successfuly";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion Save
        #region retrieve
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {

            DataSet ds = new DataSet();
            try
            {
                object[] AcInfo = new object[3];
                int i = 0;

                AcInfo.SetValue(txtManuf.Text, i);
                i++;
                AcInfo.SetValue(txtAcType.Text, i);
                i++;
                AcInfo.SetValue(txtAcVer.Text, i);


                ds = objBAL.GetAircraftEquipment(AcInfo);

                dgAcEquip.DataSource = ds.Tables[0];
                dgAcEquip.DataBind();

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion retrieve
        # region LoadDataGrid
        public void LoadDataGrid()//Create Equipment on Count
        {
            lblStatus.Text = "";
            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "TailNumber";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Status";
            myDataTable.Columns.Add(myDataColumn);
            DataRow dr;



            for (int i = 0; i < count; i++)
            {

                dr = myDataTable.NewRow();
                dr["TailNumber"] = "";
                dr["Status"] = "";
                myDataTable.Rows.Add(dr);
            }


            ViewState["CurrentTable"] = myDataTable;
            //Bind the DataTable to the Grid

            dgAcEquip.DataSource = null;
            dgAcEquip.DataSource = myDataTable;
            dgAcEquip.DataBind();


        }
        # endregion LoadDataGrid
        //protected void Button1_Click(object sender, EventArgs e)
        //{ }
        #region SaveEquipment
        protected void SaveEquipment()
        {
            string valid = "";
            try
            {
                object[] EqInfo = new object[4];
                int i;

                for (int cnt = 0; cnt < dgAcEquip.Rows.Count; cnt++)
                {
                    i = 0;
                    EqInfo.SetValue(txtAcType.Text, i);
                    i++;
                    EqInfo.SetValue(txtAcVer.Text, i);
                    i++;
                    EqInfo.SetValue(((TextBox)dgAcEquip.Rows[cnt].FindControl("txtTailNumber")).Text, i);
                    i++;
                    EqInfo.SetValue(((DropDownList)dgAcEquip.Rows[cnt].FindControl("ddlStatus")).Text, i);
                    i++;
                    string res = objBAL.SaveEquipment(EqInfo);

                }

            }
            catch (Exception ex)
            {
            }
        }
        #endregion SaveEquipment
        #region AddEquip
        protected void btnAddEquip_Click(object sender, EventArgs e)
        {
            // count= int.Parse(txtCount.Text);
            try
            {
                count = Convert.ToInt32(txtCount.Text);
            }
            catch (Exception)
            {


            }


            LoadDataGrid();

        }
        #endregion AddEquip
        #region Count
        protected void txtCount_TextChanged(object sender, EventArgs e)
        {
            count = Convert.ToInt32(txtCount.Text);
            LoadDataGrid();
        }
        #endregion Count
        #region Clear Function
        protected void ClearFunction()
        {
            txtAcType.Text = txtAcVer.Text = txtCarCap.Text = txtCount.Text = string.Empty;
            txtLWt.Text = txtManuf.Text = txtMTOW.Text = txtPCap.Text = string.Empty;
            txtRh.Text = txtRl.Text = txtRwt.Text = txtRb.Text = string.Empty;
            //txtVolLen.Text = txtVolBreadth.Text = txtVolHeight.Text = string.Empty;
            txtVol.Text = string.Empty;
            ddldimension.SelectedIndex = 0;
            ddlVolMeasure.SelectedIndex = 0;
            lblStatus.Text = string.Empty;
        }
        #endregion Clear Function
    }
}



