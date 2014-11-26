using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ProjectSmartCargoManager
{
    public partial class showCapacity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["fltCapacity"] != null)
                    {
                        DataTable dtdata = (DataTable)Session["fltCapacity"];
                        DataSet dtdataNew = (DataSet)Session["fltVarDetails"];



                        for (int i = 0; i < dtdata.Rows.Count; i++)
                        {
                            Table tbMerageAlldata = new Table();
                            TableRow trRowMerageAlldata = new TableRow();
                            TableCell tcCellMerageAlldataA = new TableCell();
                            TableCell tcCellMerageAlldataB = new TableCell();
                            tcCellMerageAlldataB.CssClass = "tableRight";

                            GridView gvNew = new GridView();
                            gvNew.ID = "gv" + i;
                            gvNew.DataSource = dtdataNew.Tables[i];
                            gvNew.DataBind();
                            tcCellMerageAlldataB.Controls.Add(gvNew);  // new
                            gvNew.CellPadding = 8;
                            gvNew.CellSpacing = 8;
                            gvNew.HeaderStyle.CssClass="titlecolr";
                            gvNew.AlternatingRowStyle.CssClass="trcolor";
                            gvNew.EditRowStyle.CssClass="grdrowfont";
                            gvNew.RowStyle.CssClass="grdrowfont";
                            gvNew.RowStyle.CssClass="grdrowfont";
                            gvNew.FooterStyle.CssClass="grdrowfont";



                            Table tbMain = new Table();
                            TableRow trRow = new TableRow();
                            AjaxControlToolkit.TabPanel tbp = new AjaxControlToolkit.TabPanel();
                            tbp.ID = "tbp" + dtdata.Rows[i][0].ToString();
                            tbp.HeaderText = "Flight : " + dtdata.Rows[i][0].ToString();
                            tbp.CssClass = "tabpan";

                            TableCell tcCell = new TableCell();
                            tcCell.CssClass = "mar";
                            decimal oneblo = Convert.ToDecimal(dtdata.Rows[i][3].ToString()) / 100;
                            decimal bkd = Math.Round(Convert.ToDecimal(dtdata.Rows[i][1].ToString()) / oneblo);
                            decimal Acp = Math.Round(Convert.ToDecimal(dtdata.Rows[i][2].ToString()) / oneblo);
                            Label lblfulDetails = new Label();
                            lblfulDetails.ID = "lblfulDetails" + i;
                            lblfulDetails.Text = "Total Capacity : " + dtdata.Rows[i][3].ToString() + "    Booked : " + dtdata.Rows[i][1].ToString() + "     Accepted : " + dtdata.Rows[i][2].ToString() + "<br /><br />";
                            lblfulDetails.Attributes.Add("Style", "font: 12px/20px Verdana,sans-serif;");
                            Label lbl = new Label();
                            lbl.ID = "lbl" + i;
                            lbl.Text = "1 Block =" + oneblo.ToString() + "<br />";
                            lbl.Attributes.Add("Style", "font: 13px/20px Verdana,sans-serif;font-weight: bold;");
                            tcCell.Controls.Add(lbl);
                            tcCell.Controls.Add(lblfulDetails);
                            trRow.Cells.Add(tcCell);
                            tbMain.Rows.Add(trRow);
                            int cntA = 0;
                            int cntB = 0;
                            Table tbldynamic = new Table();
                            for (int p = 0; p < 10; p++)
                            {
                                TableRow tr = new TableRow();
                                for (int q = 0; q < 10; q++)
                                {
                                    TableCell tc = new TableCell();
                                    Image _im = new Image();
                                    _im.ID = "_im" + p + q;
                                    if (cntA < Convert.ToInt32(Acp))
                                    {
                                        _im.ImageUrl = "~/Images/Check.jpg";
                                        _im.ToolTip = "Accepted : " + dtdata.Rows[i][2].ToString();
                                        tc.Controls.Add(_im);
                                        cntA = cntA + 1;
                                    }
                                    else if (cntB < Convert.ToInt32(bkd))
                                    {
                                        _im.ImageUrl = "~/Images/sBook.jpg";
                                        _im.ToolTip = "Booked : " + dtdata.Rows[i][1].ToString();
                                        tc.Controls.Add(_im);
                                        cntB = cntB + 1;
                                    }
                                    else
                                    {
                                        _im.ImageUrl = "~/Images/sEpty.jpg";
                                        _im.ToolTip = "Remaining : " + (Convert.ToDecimal(dtdata.Rows[i][3].ToString()) - (Convert.ToDecimal(dtdata.Rows[i][1].ToString()) + Convert.ToDecimal(dtdata.Rows[i][2].ToString()))).ToString();
                                        tc.Controls.Add(_im);
                                    }
                                    tr.Cells.Add(tc);

                                }
                                tbldynamic.Rows.Add(tr);
                            }
                            
                            tbp.Controls.Add(tbMain);
                            tcCellMerageAlldataA.Controls.Add(tbldynamic); //new
                            //tbp.Controls.Add(tbldynamic);  new 
                            //tbp.Controls.Add(gvNew); new
                            //tbMaincon.Tabs.Add(tbp); new
                            trRowMerageAlldata.Cells.Add(tcCellMerageAlldataA); // new
                            trRowMerageAlldata.Cells.Add(tcCellMerageAlldataB); // new
                            tbMerageAlldata.Rows.Add(trRowMerageAlldata); // new
                            tbp.Controls.Add(tbMerageAlldata);
                            tbMaincon.Tabs.Add(tbp);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LBLStatus.Text = ex.Message;
            }
        }
    }
}
