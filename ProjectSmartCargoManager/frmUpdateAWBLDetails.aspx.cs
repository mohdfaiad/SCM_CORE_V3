using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class frmUpdateAWBLDetails : System.Web.UI.Page
    {
        BALAWBTracLDetails BATD = new BALAWBTracLDetails();
        bool result;


        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LabelStatus.Text = "";
                LoadGrid();
                if (Session["awbPrefix"] != null)
                {
                    txtPrefix.Text = Session["awbPrefix"].ToString();
                }
            }
        }
        #endregion

        #region LoadGrid
        private void LoadGrid()
        {
            DataTable myDataTable = new DataTable();
            DataColumn myDataColumn;
            DataSet Ds = new DataSet();


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Message";
            myDataTable.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "MessageDate";
            myDataTable.Columns.Add(myDataColumn);


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "MessageTime";
            myDataTable.Columns.Add(myDataColumn);


            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "Details";
            myDataTable.Columns.Add(myDataColumn);


            DataRow dr;
            dr = myDataTable.NewRow();
            dr["Message"] = Session["Station"].ToString();
            dr["MessageDate"] = "";
            dr["MessageTime"] = "";
            dr["Details"] = "";

            myDataTable.Rows.Add(dr);

            GridViewUpdateAWBLDetails.DataSource = null;
            GridViewUpdateAWBLDetails.DataSource = myDataTable;
            GridViewUpdateAWBLDetails.DataBind();
        }

        #endregion


        #region Update AWB Details
        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string messageCode, message, MessageDate, Messagetime, Details;
                //for (int i = 0; i < GridViewUpdateAWBLDetails.Rows.Count; i++)
                //{
                string AWBPrefix, AWBNumber;
                AWBPrefix = txtPrefix.Text;
                AWBNumber = TextBoxAWBno.Text;

                messageCode = ((DropDownList)GridViewUpdateAWBLDetails.Rows[0].FindControl("ddlMessage")).SelectedItem.Text;   
                message = ((DropDownList)GridViewUpdateAWBLDetails.Rows[0].FindControl("ddlMessage")).SelectedItem.Value;
                try
                {
                    MessageDate = DateTime.Parse(((TextBox)GridViewUpdateAWBLDetails.Rows[0].FindControl("txtDate")).Text).ToString("dd/MM/yyyy");
                }
                catch (Exception)
                {
                    LabelStatus.Text = "Please enter Valid Date (DD/MM/YYYY).";
                    LabelStatus.ForeColor = Color.Red;
                    return;
                }
                
                Messagetime = ((TextBox)GridViewUpdateAWBLDetails.Rows[0].FindControl("txtTime")).Text;
                Details = ((TextBox)GridViewUpdateAWBLDetails.Rows[0].FindControl("txtDetails")).Text;

                if (messageCode == "" || messageCode.ToUpper() == "SELECT")
                {
                    LabelStatus.Text = "Please select Message";
                    LabelStatus.ForeColor = Color.Red;
                    return;
                }
                if (MessageDate == "")
                {
                    LabelStatus.Text = "Please select Message Date";
                    LabelStatus.ForeColor = Color.Red;
                    return;
                }
                if (Messagetime == "")
                {
                    LabelStatus.Text = "Please enter Message Time";
                    LabelStatus.ForeColor = Color.Red;
                    return;
                }
                if (Details == "")
                {
                    LabelStatus.Text = "Please enter Message Details starting with Station Code.";
                    LabelStatus.ForeColor = Color.Red;
                    return;
                }
                //CreadtedBy = Session["UserName"].ToString();
                result = BATD.savedata(message, messageCode, MessageDate, Messagetime, Details,AWBPrefix, AWBNumber);

                //}
                    if (result == true)
                {
                    LabelStatus.Text = "Record Updated Successfully";
                    LabelStatus.ForeColor = Color.Green;
                    BtnClear_Click(sender,e);
                    return;

                }
                else
                {
                    LabelStatus.Text = "Record not Updated.Please try again";
                    LabelStatus.ForeColor = Color.Red;
                    return;
                }
            }
            catch (Exception ex)
            { }
        }

        #endregion

        #region Clear
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            TextBoxAWBno.Text = "";
            for (int i = 0; i < GridViewUpdateAWBLDetails.Rows.Count; i++)
            {
                ((DropDownList)GridViewUpdateAWBLDetails.Rows[0].FindControl("ddlMessage")).SelectedIndex=0;
                ((TextBox)GridViewUpdateAWBLDetails.Rows[0].FindControl("txtDate")).Text = "";
                ((TextBox)GridViewUpdateAWBLDetails.Rows[0].FindControl("txtTime")).Text = "";
                ((TextBox)GridViewUpdateAWBLDetails.Rows[0].FindControl("txtDetails")).Text = "";
                           
            }

        }

        #endregion

    }
}
