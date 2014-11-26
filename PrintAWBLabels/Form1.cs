using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrintAWBLabels
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cls_DetamaxPrint objPrint = new cls_DetamaxPrint();
            objPrint.PrintXrayLabel("58980003162", "BOM", "Shipper", "Consignee", "100013|100014|100015|100016");
        }
    }
}
