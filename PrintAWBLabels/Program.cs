using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PrintAWBLabels
{
    static class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            string OC3 = string.Empty;
            string Origin = args[0];
            string Destination = args[1];
            int PiecesCount = Convert.ToInt32(args[2]);
            string AWBNo = args[3];
            string FromPcs = args[4];
            string ToPcs = args[5];

            string PrintingType = "";
            try
            {
                PrintingType = args[6]; // For X-Ray Label Printing or AWB Label Printing. (BK - Booking, XR - X-ray)            
            }
            catch (Exception ex)
            {
                PrintingType = "BK";
            }
            
            string Location = "";
            try
            {
                Location = args[7];
            }
            catch (Exception ex)
            {
                Location = "";
            }

            string Shipper = "";
            try
            {
                Shipper = args[8];
            }
            catch (Exception ex)
            {
                Shipper = "";
            }

            string Consignee = "";
            try
            {
                Consignee = args[9];
            }
            catch (Exception ex)
            {
                Consignee = "";
            }

            string PieceInfo = "";
            try
            {
                PieceInfo = args[10];
            }
            catch (Exception ex)
            {
                PieceInfo = "";
            }

            OC3 = System.Configuration.ConfigurationManager.AppSettings["OC3"];

            if (OC3 != "1")
            {
                cls_DetamaxPrint objPrint = new cls_DetamaxPrint();

                if (PrintingType == "BK")
                {
                    objPrint.PrintLabel(Destination, Origin, "", PiecesCount, AWBNo, FromPcs, ToPcs);
                }
                else
                {
                    objPrint.PrintXrayLabel(AWBNo, Location, Shipper, Consignee, PieceInfo);
                }
            }
            else
            {
                clsOneilOC3 objPrint = new clsOneilOC3();

                for (int ICount = 0; ICount < PiecesCount; ICount++)
                {
                    objPrint.PrintLabel(Destination, Origin, "", PiecesCount.ToString(), AWBNo, PiecesCount - ICount);
                    System.Threading.Thread.Sleep(2000);
                }
            }
        }

        //[STAThread]
        //static void Main()
        //{
        //    System.Windows.Forms.Application.Run(new Form1());
        //}
    }
}
