using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace PrintAWBLabels
{
    public class cls_DetamaxPrint
    {

        #region Variables

        int ItemCount = 0;
        string AXBNo = "", Source = "", SourceLoc = "", Destination = "", destLoc = "", destPin = "", Via = "", UBI = "", lblType = "", FromUBI = "", ToUBI="", FromPCS="", ToPCS="";
        SerialPort btSSP = new SerialPort();
        private const string FileName = "Label.prn";
        private const string batFileName = "Label.bat";
        int cnt = 0;
        string strFile;// = FilePath + "\\" + FileName;
        //private string PrinterPort = ConfigurationManager.AppSettings["ZebPrnPort"].ToString();
        //private static string APP_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
        private static string APP_PATH = ConfigurationManager.AppSettings["PrinterFile"].ToString();
        //private static string APP_PATH = "D:\\DesktopPrinter";
        private string FilePath = APP_PATH;
        //private string FilePath = "D:\\DigitalDiaryLabelStore\\Default\\"; //Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);

        #region Prepare Label Data
        private string str1 = "n";
        private string str2 = "M0500";
        private string str3 = "V0";
        private string str4 = "Kf0075";
        private string str5 = "SM";
        private string str6 = "d";
        private string str7 = "L";
        private string str8 = "D11";
        private string str9 = "PM";
        private string str10 = "pG";
        private string str11 = "SM";
        private string str12 = "ySU8";
        private string str13 = "FB+";
        private string str14 = "A2";
        private string str15 = "1911S0100990031P053P051DEL"; // DEST
        private string str16 = "1911S0100760048P010P012BOM"; // SOURCE
        private string str17 = "1911S0101500170P012P012VIA - ";
        private string str18 = "1911S0100760138P010P014HYD"; // VIA
        private string str19 = "1911S0101500040P012P012ORG-";
        private string str20 = "FB-";
        private string str21 = "1911S0100580048P012P012999/999"; // TOT PICS AND CTR
        private string str22 = "1911S0100590138P012P01212345"; // WT
        private string str23 = "FB+";
        private string str24 = "1911S0101300040P012P012PCS-";
        private string str25 = "1911S0101300175P012P012WT-";
        private string str26 = "1a3103100230039090-02768076"; // BARCODE 1
        private string str27 = "1911S0100090049P010P010090-02768076"; // BARCODE 2
        private string str28 = "1X1100002580080L160015";
        private string str29 = "A1";
        private string str30 = "1911S0102600080P010P010 KINGFISHER CARGO ";
        private string str31 = "A2";
        private string str32 = "1X1100002550080B160021001001";
        private string str33 = "Q0001";
        private string str34 = "E";
        private string strBox = "";
        private string strLine1 = "";
        private string strLine2 = "";
        private string str38 = "";
        private string str39 = "";
        private string str40 = "";
        private string str41 = "";
        private string str42 = "";
        private string str43 = "";
        private string str44 = "";
        private string str45 = "";
        private string strAXB = "";
        private string strAXBData = "";
        private string strQty = "";
        private string strQtyData = "";
        private string strOrg = "";
        private string strOrgData = "";
        private string strOrgLoc = "";
        private string strDest = "";
        private string strDestData = "";
        private string strDestLoc = "";
        private string strDestPincode = "";
        private string strContactNo = "";
        private string strFooter = "Powered By QID";
        #endregion

        #endregion

        #region Constructor
        //default constructor
        public cls_DetamaxPrint()
        {
            btSSP.BaudRate = 19200;
            btSSP.DataBits = 8;
            btSSP.PortName = "COM3";
        }
        
        //parameterise constructor
        public cls_DetamaxPrint(string Port, int BaudeRate)
        {
            //InitializeComponent();

            btSSP.BaudRate = BaudeRate;
            btSSP.DataBits = 8;
            btSSP.PortName = Port;

            //txtAWBNo.Text = AWBNo;
            //txtSource.Text = Source;
            //txtDestination.Text = Destination;
            //txtVia.Text = Via;
            //txtTotalWt.Text = TotalWt;
            //txtNoOfPieces.Text = "" + ItemCount;
        }
        #endregion

        #region PrintLabel
        public bool PrintLabel(string _Dest, string _Source, string _DestPin, int _ItemCount, string _AXBNo, string _FromPCS, string _ToPCS)
        {
            bool flag = false;
            try
            {
                Destination = _Dest;
                //destLoc = _DestLoc;
                Source = _Source;
                //SourceLoc = _sourceLoc;
                destPin = _DestPin;
                ItemCount = _ItemCount;
                AXBNo = _AXBNo;
                //UBI = _UBI;
                //cnt = _Cnt;
                //lblType = _lblType;
                //FromUBI = _FromUBI;
                //ToUBI = _ToUBI;
                FromPCS = _FromPCS;
                ToPCS = _ToPCS;
                
                
                //Via = _Via;
                //TotalWt = _TotalWt;

                this.CallPrinter();
            }
            catch(Exception ex)
            {
            flag = false;
            }
            return flag;
        }
        #endregion

        #region ConfigPort
        void ConfigPort(string Port)
        {
            btSSP.BaudRate = 19200;
            btSSP.DataBits = 8;
            btSSP.PortName = Port;
            //btSSP.PortName = ConfigurationManager.AppSettings["BTPort"].ToString();

        }
        #endregion

        #region CallPrinter
        private void CallPrinter()
        {
            try
            {
                strFile = FilePath + "\\" + FileName;
                //this.ClearLogs();
                //int TotBags = 10; // int.Parse(txtNoOfPieces.Text);
                File.Delete(APP_PATH + "\\" + FileName);
                
                //for (int i = 1; i <= ItemCount; i++)
                //{
                //    Delay(5);
                    //this.printLabel_DM(AWBNo, Source, Via, Destination, ItemCount.ToString(), TotalWt, i.ToString());
                //UBI = FromUBI;
                for (int i = int.Parse(FromPCS); i <= int.Parse(ToPCS); i++)
                {

                    //if (lblType == "AIR")
                    //{
                    //    this.prepareAirLabel(AXBNo, Source, SourceLoc, Destination, destLoc, destPin, ItemCount.ToString(), i.ToString(), UBI);
                    //    UBI = (double.Parse(UBI) + 1).ToString();
                    //}
                    //else if (lblType == "SFC")
                    //{
                        this.prepareSFCLabel(AXBNo, Source, Destination, destPin, ItemCount.ToString(), i.ToString());
                    //}
                    Delay(5);
                    
                }
                //}

                Delay(15);
                //File.Copy(strFile, APP_PATH +"\\"+ FileName, true);

                System.Diagnostics.Process Process1 = new System.Diagnostics.Process();
                //Process Process1 = new Process();
                Process1.EnableRaisingEvents = false;
                string cmd = APP_PATH + "\\"+ batFileName;
                System.Diagnostics.ProcessStartInfo info = new ProcessStartInfo();



                info.UseShellExecute = true;

                info.FileName = batFileName;
                //D:\Products\SCM\DesktopPrinter
                //info.WorkingDirectory = "D:\\DesktopPrinter\\";//APP_PATH +"\\";
                info.WorkingDirectory = APP_PATH+"\\";

                //info.Verb = "runas";

                System.Diagnostics.Process.Start(info);


                Process1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        #endregion

        #region PrepareSFCLabel
        private void prepareSFCLabel(string RXBNo, string Origin, string Dest, string destPin, string totPics, string Ctr)
        {
            try
            {
                // First Create File
                //  this.CreateFile();
                // write content to file
                Delay(2);
                WriteToFile(str1);// = "n";
                WriteToFile(str2);// ="M0500";
                WriteToFile(str3);// ="V0";
                WriteToFile(str4);// ="Kf0075";
                WriteToFile(str5);// ="SM";
                WriteToFile(str6);// ="d";
                WriteToFile(str7);// ="L";
                WriteToFile(str8);// ="D11";
                WriteToFile(str9);// ="PM";
                WriteToFile(str10);// ="pG";
                WriteToFile(str11);// ="SM";
                WriteToFile(str12);// ="ySU8";
                WriteToFile(str13);// ="FB+";
                WriteToFile(str14);// ="A2";

                strBox = "1X1100001150010B275135002002";
                //strBox = "1X1100001150010B275135002002";
                WriteToFile(strBox);

                strLine1 = "1X1100001800010L275002";
                WriteToFile(strLine1);

                strLine2 = "1X1100001150150L002135";
                WriteToFile(strLine2);

                strAXB = "1911S0102300020P010P010AWBNo";
                WriteToFile(strAXB);
                RXBNo = RXBNo.PadLeft(10, '0');

                strAXBData = "1911S0101950015P013P012" + RXBNo.Substring(0, 6);
                WriteToFile(strAXBData);

                strAXBData = "1911S0101950080P025P020" + RXBNo.Substring(6, 4);
                WriteToFile(strAXBData);
                //strAXBData = "1911S0101950013P018P018" + RXBNo;
                //WriteToFile(strAXBData);

                strQty = "1911S0102300160P010P010No Of Pcs";
                WriteToFile(strQty);

                strQtyData = "1911S0101950155P018P018" + Ctr.ToString() + "/" + totPics;
                WriteToFile(strQtyData);

                strOrg = "1911S0101650020P010P010ORIGIN";
                WriteToFile(strOrg);

                strOrgData = "1911S0101250020P025P025" + Source;
                WriteToFile(strOrgData);

                //if (SourceLoc.Substring(SourceLoc.Length - 3, 3) == "CBC")
                //    strOrgLoc = "1911S0101250110P010P010CBC";
                //else
                //    strOrgLoc = "1911S0101250110P010P010" + SourceLoc;
                //WriteToFile(strOrgLoc);

                strDest = "1911S0101650160P010P010DESTINATION";
                WriteToFile(strDest);

                strDestData = "1911S0101250160P025P025" + Destination;
                WriteToFile(strDestData);

                //if (destLoc.Trim() != "")
                //{
                //    if (destLoc.Substring(destLoc.Length - 3, 3) == "CBC")
                //        strDestLoc = "1911S0101350235P010P010CBC";
                //    else
                //        strDestLoc = "1911S0101350235P010P010" + destLoc;

                //    WriteToFile(strDestLoc);
                //}

                strDestPincode = "1911S0101200165P010P010" + destPin;
                WriteToFile(strDestPincode);

                // barcode 1
                str26 = "1a0000000700025" + RXBNo + Ctr.ToString().PadLeft(4, '0');
                //str26 = "1a8104100820049090-" + UBI;
                //str26 = "1a3103100500039090-" + UBI;
                WriteToFile(str26);// ="1a3103100230039090-02768076"; // BARCODE 1
                // barcode 2
                str27 = "1911S0100450065P020P020" + RXBNo;
                //str27 = "1911S0100380035P040P027" + UBI;
                //str27 = "1911S0100100021P039P017" + UBI;
                WriteToFile(str27);// ="1911S0100090049P010P010090-02768076"; // BARCODE 2

                //WriteToFile("1911S0100100050P010P010Date - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                WriteToFile("1911S0100250050P010P010Date - " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                //WriteToFile("1911S0100100080P014P014" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                //WriteToFile(str28);// ="1X1100001600041L122015";1X1100001600041L122015
                WriteToFile(str29);// ="A1";
                //str30 = "1911S0102650025P018P018Jet Airways CARGO";
                str30 = "1911S0102650035P018P018Air Costa CARGO";
                //String strLableHeading = "Air Costa CARGO";
                //String strHedingCmd = "1911S0102650035P018P018";
                String lableHeading = "GoAir CARGO";
                String hedingCmd = "1911S0102650055P018P018";

                lableHeading = getValueFromConf("lableHeading");
                hedingCmd = getValueFromConf("hedingCmd");


                if(String.IsNullOrEmpty(hedingCmd ))
                    hedingCmd = "1911S0102650055P018P018";
                if(String.IsNullOrEmpty(lableHeading))
                     lableHeading = "GoAir CARGO";
                str30 = hedingCmd+lableHeading;

                WriteToFile(str30);// ="1911S0101630042P008P008 Delex Cargo (AIR)";
               // strContactNo = "1911S0102520040P010P010Ph: 1800222111/09223222111";
                strContactNo = "1911S0102520040P010P010" + getValueFromConf("clientContact"); ;
                //WriteToFile(strContactNo);
                strFooter = "1911S0100100055P007P007Powered By QID  Ph: 020-40039501";
                WriteToFile(strFooter);
                
                WriteToFile(str31);// ="A2";
                //WriteToFile(str32);// ="1X1100001560039B126021001001";
                WriteToFile(str33);// ="Q0001";
                WriteToFile(str34);// ="E";
            }
            catch (Exception ex)
            {

            }
        }

        private string getValueFromConf(String Key )
        {
            try
            {
                return ConfigurationManager.AppSettings[Key].ToString();
            }
            catch(Exception objEx)
            {
            }
            return "";
        }
        #endregion

        #region Delay
        private void Delay(int seconds)
        {
            try
            {
                int time = seconds * 1000;
                for (int i = 0; i < time; i++)
                {
                    // do nothing
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region WritetoFile
        private void WriteToFile(string Message)
        {
            try
            {
                StreamWriter sw = new StreamWriter(strFile, true);
                sw.WriteLine(Message);
                sw.Close();
                Delay(100);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        #endregion

        public void prepareSecurityLabel(string AWBNo, string SecurityCode, string Location, string Shipper, string Consignee)
        {
            try
            {
                // First Create File
                //  this.CreateFile();
                // write content to file
                Delay(2);
                WriteToFile(str1);// = "n";
                WriteToFile(str2);// ="M0500";
                WriteToFile(str3);// ="V0";
                WriteToFile(str4);// ="Kf0075";
                WriteToFile(str5);// ="SM";
                WriteToFile(str6);// ="d";
                WriteToFile(str7);// ="L";
                WriteToFile(str8);// ="D11";
                WriteToFile(str9);// ="PM";
                WriteToFile(str10);// ="pG";
                WriteToFile(str11);// ="SM";
                WriteToFile(str12);// ="ySU8";
                WriteToFile(str13);// ="FB+";
                WriteToFile(str14);// ="A2";

                str30 = "1911S0102650025P018P018Jet Airways CARGO";
                WriteToFile(str30);

                str31 = "1911S0102350060P018P018Security Check";
                WriteToFile(str31);

                string strSecurity = "1911S0102050060P018P018" + SecurityCode;
                WriteToFile(strSecurity);

                string strBarcode = "1W1c77000008000902000000000" + AWBNo + "," + SecurityCode + "," + Shipper + "," + Consignee;
                WriteToFile(strBarcode);

                string strAWB = "1911S0100500025P018P018AWB: " + AWBNo;
                WriteToFile(strAWB);

                //WriteToFile("1911S0100100050P010P010Date - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                WriteToFile("1911S0100250050P010P010" + Location.Trim().ToUpper() + ": " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                //WriteToFile("1911S0100100080P014P014" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                //WriteToFile(str28);// ="1X1100001600041L122015";1X1100001600041L122015
                WriteToFile(str29);// ="A1";

                strContactNo = "1911S0102520040P010P010Ph: 1800222111/09223222111";
                //WriteToFile(strContactNo);
                strFooter = "1911S0100100055P007P007Powered By QID  Ph: 020-40039501";
                WriteToFile(strFooter);
                str31 = "A2";
                WriteToFile(str31);// ="A2";
                //WriteToFile(str32);// ="1X1100001560039B126021001001";
                WriteToFile(str33);// ="Q0001";
                WriteToFile(str34);// ="E";
            }
            catch (Exception ex)
            {

            }
        }

        #region CallPrinter for X-Ray Labels
        private void CallPrinterXray(string AWBNumber, string Location, string Shipper, string Consignee, string PieceInfo)
        {
            try
            {
                string[] strPieceIds = PieceInfo.Split('|');

                strFile = FilePath + "\\" + FileName;
                //this.ClearLogs();
                //int TotBags = 10; // int.Parse(txtNoOfPieces.Text);
                File.Delete(APP_PATH + "\\" + FileName);

                for (int i = 0; i < strPieceIds.Length; i++)
                {
                    this.prepareSecurityLabel(AWBNumber, strPieceIds[i], Location, Shipper, Consignee);
                    Delay(5);
                }

                Delay(15);
                //File.Copy(strFile, APP_PATH +"\\"+ FileName, true);

                System.Diagnostics.Process Process1 = new System.Diagnostics.Process();
                //Process Process1 = new Process();
                Process1.EnableRaisingEvents = false;
                string cmd = APP_PATH + "\\" + batFileName;
                System.Diagnostics.ProcessStartInfo info = new ProcessStartInfo();
                
                info.UseShellExecute = true;

                info.FileName = batFileName;
                //D:\Products\SCM\DesktopPrinter
                //info.WorkingDirectory = "D:\\DesktopPrinter\\";//APP_PATH +"\\";
                info.WorkingDirectory = APP_PATH + "\\";

                //info.Verb = "runas";

                System.Diagnostics.Process.Start(info);


                Process1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        #endregion

        #region PrintLabel Xray
        public bool PrintXrayLabel(string AWBNumber, string Location, string Shipper, string Consignee, string PieceInfo)
        {
            bool flag = false;
            try
            {
                this.CallPrinterXray(AWBNumber, Location, Shipper, Consignee, PieceInfo);
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
        #endregion
    }
}
