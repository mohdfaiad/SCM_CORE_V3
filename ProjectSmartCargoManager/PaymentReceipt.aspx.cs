using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QID.DataAccess;
using System.Drawing;
using BAL;

namespace ProjectSmartCargoManager
{
    public partial class PaymentReceipt : System.Web.UI.Page
    {
        SQLServer da = new SQLServer(Global.GetConnectionString());
        BALPaymentReceipt objRecipt = new BALPaymentReceipt(); 

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadDliveyDetails();
                    LoadPaymentGrid();
                }
            }
            catch (Exception ex) { } 
        }
        #endregion Load
        #region LoadDliveyDetails

        public void LoadDliveyDetails()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("AttributeName");
            dt.Columns.Add("Value");
           
            DataRow row;
            row = dt.NewRow();
            row["AttributeName"] = "";// "TV";
            row["Value"] = "";
          
            dt.Rows.Add(row);


            grdDeliveryDetails.DataSource = null;
            grdDeliveryDetails.DataSource = dt.Copy();
            grdDeliveryDetails.DataBind();

            Session["dtRates"] = dt.Copy();
        }


        
        #endregion LoadDliveyDetails
        #region PaymentRecipt
        public void LoadPaymentGrid()
        {
            try
            {
               DataTable myDataTable = new DataTable();
                DataColumn myDataColumn;
                DataSet Ds = new DataSet();

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ServiceName";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "ChargeName";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Amount";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Waive";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "GST";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "NetAmount";
                myDataTable.Columns.Add(myDataColumn);


                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Currency";
                myDataTable.Columns.Add(myDataColumn);

                myDataColumn = new DataColumn();
                myDataColumn.DataType = Type.GetType("System.String");
                myDataColumn.ColumnName = "Remark";
                myDataTable.Columns.Add(myDataColumn);


                DataRow dr;
                dr = myDataTable.NewRow();
                dr["ServiceName"] = "";
                dr["ChargeName"] = "";//"5";
                dr["Amount"] = "";
                dr["Waive"] = "";
                dr["GST"] = "";
                dr["NetAmount"] = "";
                dr["Currency"] = "";
                dr["Remark"] = "";


                myDataTable.Rows.Add(dr);
                //ViewState["CurrentTable"] = myDataTable;
                //Bind the DataTable to the Grid

                GrdpaaymentReceipt.DataSource = null;
                GrdpaaymentReceipt.DataSource = myDataTable;
                GrdpaaymentReceipt.DataBind();
            }
            catch (Exception ex) { }
        }

        #endregion PaymentRecipt
        #region BtnClear
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception ex) { }
        }
        #endregion BtnClear
        #region Clear
        public void clear()
        {
            txtpaymentadvice.Text = "";
            txtServiceCode.Text = "";
            //LoadDliveyDetails();
            LoadPaymentGrid();
            txttotal.Text = "";
            txtGSTTotal.Text = "";
            txttds.Text = "";
            txtgrandtotal.Text = "";
            txtcustomername.Text = "";
            txtCustomerCode.Text = "";
            txtrondedamt.Text = "";
            txtfinalAmt.Text = "";
            txtbalancegiven.Text = "";
            txtremark.Text = "";
            txtpaymentdeetail.Text = "";
        }
        #endregion Clear
        #region BtnClose
        protected void btnclose_Click(object sender, EventArgs e)
        {
            Response.Redirect("delivery.aspx", false);
        }
        #endregion Btclose
        #region save
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (txtCustomerCode.Text == "" && txtServiceCode.Text == "")
            {
                lblStatus.Text = "Please Enter Custmer Code and Service Code";
                return; 
            }
            try
            {
                #region Paramname
                string[] paramname = new string[24];
                paramname[0] = "PaymentAdviceNumber";
                paramname[1] = "ServiceCode";
                paramname[2] = "AttributeName";
                paramname[3] = "AttributeValue";
                paramname[4] = "ServiceName";
                paramname[5] = "ChargeName";
                paramname[6] = "Amount";
                paramname[7] = "Waive";
                paramname[8] = "GST";
                paramname[9] = "NetAmount";
                paramname[10] = "Currency";
                paramname[11] = "Remark";
                paramname[12] = "Total";
                paramname[13] = "FinalAmount";
                paramname[14] = "GSTOnTotal";
                paramname[15] = "BalanceGiven";
                paramname[16] = "TDS";
                paramname[17] = "CheckWaive";
                paramname[18] = "GrandTotal";
                paramname[19] = "RoundOffAmount";
                paramname[20] = "CustommerCode";
                paramname[21] = "CustomerName"; 
                paramname[22] = "PaymntDetails";
                paramname[23] = "PaymentRemark";
            #endregion Paramname 
                #region Paramvalue
                object[] paramvalue = new object[24];
                paramvalue[0] = txtpaymentadvice.Text;
                paramvalue[1] = txtServiceCode.Text;
                //Delivery Details Grid
                for (int j = 0; j < grdDeliveryDetails.Rows.Count; j++)
                {
                    if (((CheckBox)grdDeliveryDetails.Rows[j].FindControl("chk")).Checked == true)
                    {
                            paramvalue[2] = ((Label)grdDeliveryDetails.Rows[j].FindControl("lblattributename")).Text;
                            paramvalue[3] = ((Label)grdDeliveryDetails.Rows[j].FindControl("lblvalue")).Text;
                    }
                }
                //Delivery Details Grid

                //GridPayment Receipt
                 for (int cnt = 0; cnt < grdDeliveryDetails.Rows.Count; cnt++)
                {
                    if (((CheckBox)grdDeliveryDetails.Rows[cnt].FindControl("check")).Checked == true)
                    {
                        paramvalue[4] = ((Label)GrdpaaymentReceipt.Rows[cnt].FindControl("lblServiceName")).Text;
                        paramvalue[5] = ((Label)GrdpaaymentReceipt.Rows[cnt].FindControl("lblchargename")).Text;
                        paramvalue[6] = ((Label)GrdpaaymentReceipt.Rows[0].FindControl("lblamount")).Text;
                        paramvalue[7] = ((Label)GrdpaaymentReceipt.Rows[0].FindControl("lblwaive")).Text;
                        paramvalue[8] = ((Label)GrdpaaymentReceipt.Rows[0].FindControl("lblgst")).Text;
                        paramvalue[9] = ((Label)GrdpaaymentReceipt.Rows[0].FindControl("lblnetamount")).Text;
                        paramvalue[10] = ((Label)GrdpaaymentReceipt.Rows[0].FindControl("lblcurrency")).Text;
                        paramvalue[11] = ((TextBox)GrdpaaymentReceipt.Rows[0].FindControl("txtremark")).Text;
                    }
               }
               //GridPayment Receipt
                paramvalue[12]=txttotal.Text; 
                paramvalue[13]=txtfinalAmt.Text; 
                paramvalue[14]=txtGSTTotal.Text; 
                paramvalue[15]=txtbalancegiven.Text; 
                paramvalue[16]=txttds.Text; 
                paramvalue[17]=chkwaive.Checked; 
                paramvalue[18]=txtgrandtotal.Text;
                paramvalue[19]=txtrondedamt.Text; 
                paramvalue[20]=txtCustomerCode.Text;
                paramvalue[21]= txtcustomername.Text;
                paramvalue[22]=txtpaymentdeetail.Text;
                paramvalue[23]=txtremark .Text;
                #endregion Paramvalue

                #region ParamType
                SqlDbType[] paramtype = new SqlDbType[24];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;
                paramtype[3] = SqlDbType.Float ;
                paramtype[4] = SqlDbType.VarChar;
                paramtype[5] = SqlDbType.VarChar;
                paramtype[6] = SqlDbType.Float;
                paramtype[7] = SqlDbType.VarChar;
                paramtype[8] = SqlDbType.VarChar;
                paramtype[9] = SqlDbType.Float;
                paramtype[10] = SqlDbType.VarChar;
                paramtype[11] = SqlDbType.VarChar;
                paramtype[12] = SqlDbType.Float;
                paramtype[13] = SqlDbType.Float;
                paramtype[14] = SqlDbType.Float;
                paramtype[15] = SqlDbType.Float;
                paramtype[16] = SqlDbType.VarChar;
                paramtype[17] = SqlDbType.Bit;
                paramtype[18] = SqlDbType.Float;
                paramtype[19] = SqlDbType.Float;
                paramtype[20] = SqlDbType.VarChar;
                paramtype[21] = SqlDbType.VarChar;
                paramtype[22] = SqlDbType.VarChar;
                paramtype[23] = SqlDbType.VarChar;
                #endregion ParamType

 //PaymentAdviceNumber,ServiceCode,AttributeName,AttributeValue,ServiceName,ChargeName,
//Amount,Waive,GST,NetAmount,Currency,Remark,Total,FinalAmount,GSTOnTotal,BalanceGiven,
//TDS,CheckWaive,GrandTotal,RoundOffAmount,CustommerCode,CustomerName,PaymntDetails,
//PaymentRemark

                bool ds = da.InsertData("Sp_InsertPaymentDetails", paramname, paramtype, paramvalue);

                if (ds == false)
                {
                    lblStatus.Text = "Error in Saving Data";
                    lblStatus.ForeColor = Color.Red;   
                
                }
                else if (ds == true)
                {
                    lblStatus.Text = "Receipt Saved Successfully..";
                    lblStatus.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error..Please Try again..";
                lblStatus.ForeColor = Color.Red;    
            
            }

        }
        #endregion Save
    }
  
}
