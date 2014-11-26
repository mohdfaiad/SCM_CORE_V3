#region
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Reflection;
using System.Runtime;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
#endregion

namespace PrintAWBLabels
{
    class clsOneilOC3
    {
        SerialPort srPort = new SerialPort();
        
        private string setEasyMode = Convert.ToChar(27) + "EZ";
        //private string Label = "";
        //string query = Convert.ToChar(27) + "{Sn?}";

        #region Constructor
        public clsOneilOC3()
        {
            srPort.PortName = System.Configuration.ConfigurationManager.AppSettings["PortName"];
            srPort.BaudRate = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BaudRate"]);
        }

        public clsOneilOC3(string COMPort, int baudrate)
        {
            srPort.PortName = COMPort;
            srPort.BaudRate = baudrate;
        }
        #endregion;

        #region Print Label
        public int PrintLabel(string dest, string source, string via, string item, string awb, int no)
        {
            try
            {
                //int option = 0;
                //awb.PadLeft(11, '0');
                string lblHeader = "";
                try
                {
                    lblHeader = getValueFromConf("lableHeading");
                }
                catch (Exception ex)
                {
                    lblHeader = "GoAir CARGO";
                }
                
                string ClientPhone = "";
                try
                {
                    ClientPhone = getValueFromConf("clientContact");
                }
                catch (Exception ex)
                {
                    ClientPhone = "";
                }
                
                string mhead = "{PRINT:";
                //string name = "@0,50:MF107, HM2, VM2|Jet Airways CARGO|";
                string name = "@0,50:MF107, HM2, VM2|"+lblHeader.Trim()+"|";
                //string lPhone = "@40,170:MF226, HM1, VM1|Ph: 1800222111/09223222111|";
                string lPhone = "@40,170:MF226, HM1, VM1|"+ ClientPhone.Trim() +"|";
                string AXB = "@80,50:MF107|AWBNo|";
                
                string AXBValue1 ="";
                string AXBValue2 = "";
                if (awb.Trim().Length == 10)
                {
                    AXBValue1 = "@135,50:MF226, HM2, VMULT2|" + awb.Substring(0, 6) + "|";
                    AXBValue2 = "@115,180:MF226, HM3, VMULT3|" + awb.Substring(6, 4) + "|";
                }
                else if (awb.Trim().Length == 11)
                {
                    if (awb.Trim().ToUpper().Substring(0, 3) == "SAC")
                    {
                        AXBValue1 = "@135,50:MF226, HM2, VMULT2|" + awb.Substring(0, 3) + "|";
                        AXBValue1 += "@135,110:MF226, HM2, VMULT2|" + awb.Substring(3, 4) + "|";
                        AXBValue2 = "@115,185:MF226, HM3, VMULT3|" + awb.Substring(7, 4) + "|";
                    }
                    else
                    {
                        AXBValue1 = "@135,50:MF226, HM2, VMULT2|" + awb.Substring(0, 7) + "|";
                        AXBValue2 = "@115,180:MF226, HM3, VMULT3|" + awb.Substring(7, 4) + "|";
                    }
                }
                //string mtype = "@35,140:MF204, HM3, V6|" + type + "|";

                string mitm = "@80,310:MF107|No Of PCS|";
                string mitemValue = "@135,310:MF226, HM3, VMULT2|" + no.ToString() + "/" + item + " |";

                string mdest = "@220,310:MF107|DEST|";
                //string mdestValue = "@255,310:MF226, HM8, VMULT3|" + dest + "|";
                string mdestValue = "@255,310:MF107, HM4, VMULT3|" + dest + "|";
                string destpin = "@300,310:MF107| |";
                string msource = "@220,50:MF107|ORIGIN|";
                //string msourceValue = "@255,50:MF226, HM8, VMULT3|" + source + "|";
                string msourceValue = "@255,50:MF107, HM4, VMULT3|" + source + "|";
                string mvia = "@222,350:MF107, HM3, VM2|" + via + "|";

                //string mwt = "@275,350:MF075, HM3, VM2|WT- " + weight + "|";
                //string mBaxb = "@335,50:BC39N,HIGH 25,WIDE 4|" + axb + "|";
                string mBaxb = "@338,100:BC39N,HIGH 15,WIDE 2|" + awb.Trim() + no.ToString().Trim().PadLeft(4,'0') +"|";
                string mLaxb = "@420,150:MF226, HM3, VMULT2|" + awb.Trim() + "|";
                string tDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                string mtDate = "@470,60:MF107|Date:" + tDate + "|";
                string lFooter = "@508,150:MF226|Powered By QID Ph: 020-40039501|";
                //string mtDate = "@528,60:MF107|Date:" + tDate + "|";
                //string hLine1 = "@0,40:HLINE, length 520, thick 5|";  //
                string hLine2 = "@60,40:HLINE, length 520, thick 5|";   //
                string vLine1 = "@60,40:VLINE, length 270, thick 5|";   //
                string vLine3 = "@60,300:VLINE, length 270, thick 5|";  //
                string vLine2 = "@60,555:VLINE, length 270, thick 5|";  //
                string hLine4 = "@325,40:HLINE, length 520, thick 5|";  //
                string hLine3 = "@200,40:HLINE, length 520, thick 5|";  //

                string mFooter = "}";
                //string changeMode = "{LP}";
                string back = "{BACK:15}";
                string ahead = "{AHEAD:114}";
                byte[] mByte = new byte[1];
                mByte[0] = 14;
                byte[] mmByte = new byte[3];
                mmByte[0] = 27;
                mmByte[1] = 67;
                mmByte[2] = 14;
                byte[] query = new byte[6];
                query[0] = 27;
                query[1] = 123;
                query[2] = 83;
                query[3] = 84;
                query[4] = 63;
                query[5] = 125;
                string config = Convert.ToChar(27) + "{CL!M:1}";

                string LabelData = mhead + name + lPhone + vLine1 + vLine2 + vLine3 + hLine2 + hLine3 + hLine4 + AXB + AXBValue1 + AXBValue2 + mdest + mdestValue + destpin + msource + msourceValue + mvia + mitm + mitemValue + mBaxb + mLaxb + mtDate + lFooter + mFooter;

                if (openport())                 //open serial port
                {
                    try
                    {
                        //srPort.WriteLine(config);
                        srPort.DiscardInBuffer();
                        srPort.DiscardOutBuffer();
                        srPort.Write(query, 0, 6);    //send query to printer
                        Delay(5000);
                        //char[] buffer = new char[srPort.BytesToRead];
                        //srPort.Read(buffer, 0, srPort.BytesToRead);

                        //if(buffer[18] != 'P')
                        //{
                        //    DialogResult result = new DialogResult();
                        //    result = MessageBox.Show("Please Check Printer Paper and Continue","Print",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1 );
                        //    if (result == DialogResult.Yes)
                        //    {
                        //        return 2;
                        //    }
                        //    else
                        //    {
                        //        return 0;
                        //    }
                        //}
                        //  srPort.WriteTimeout = 200;
                        int btw = srPort.BytesToWrite;
                        //srPort.Write(mmByte, 0, 3); //set form size to 14

                        srPort.Write(setEasyMode);//set printer to easy mode
                        //srPort.Write(back);
                        srPort.Write(LabelData);    //print label
                        srPort.Write(ahead);
                        Delay(1000);
                        while (srPort.BytesToWrite > 0)
                        {
                            Application.DoEvents();
                        }
                        //srPort.Write(changeMode);   //set printer to line mode by using {LP}
                        //srPort.Write(mByte, 0, 1);  //set form feed
                        srPort.Close();             //close the serial port

                    }
                    catch (Exception ex)
                    {
                        srPort.Close();             //close the serial port
                        return (0);
                    }
                    finally
                    {
                        srPort.Close();             //close the serial port
                    }
                    return (1);
                }
                else
                {
                    return (0);
                }
            }
            catch (Exception)
            {
                return (0);
            }

        }
        #endregion

        #region DELAY
        private void Delay(int mSec)
        {
            int i, j;
            int hmSec = mSec / 10;
            try
            {
                for (i = 0; i < mSec; i++)
                {
                    for (j = 0; j < hmSec; j++)
                    {
                        //
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion

        #region OpenPort
        private bool openport()
        {
            bool flag=false ;
            try
            {
                srPort.Open();
                if (srPort.IsOpen)
                    flag = true;
                return flag;
            }
            catch (Exception ex)
            {
                return flag;
            }
        }
        #endregion

        #region SendCommand
        private void SendCommand(int CMD_HEADER, int CMD_VALUE, int CMD_PARAM)
        {
            try
            {
                if (!srPort.IsOpen)
                    return;
                byte[] arrByte = new byte[3];
                arrByte[0] = Convert.ToByte(CMD_HEADER);
                arrByte[1] = Convert.ToByte(CMD_VALUE);
                arrByte[2] = Convert.ToByte(CMD_PARAM);
                srPort.Write(arrByte, 0, arrByte.Length);
            }
            catch (Exception)
            {
            }
        }

        private void SendCommand(int CMD_HEADER, int CMD_VALUE)
        {
            try
            {
                if (!srPort.IsOpen)
                    return;
                byte[] arrByte = new byte[2];
                arrByte[0] = Convert.ToByte(CMD_HEADER);
                arrByte[1] = Convert.ToByte(CMD_VALUE);
                srPort.Write(arrByte, 0, arrByte.Length);
            }
            catch (Exception)
            {
            }
        }

        private void SendCommand(int CMD_HEADER, int CMD_VALUE, int CMD_PARAM, int CMD_LENGTH, string CMD_DATA)
        {
            try
            {
                if (!srPort.IsOpen)
                    return;
                byte[] arrByte = new byte[4 + CMD_DATA.Length];
                arrByte[0] = Convert.ToByte(CMD_HEADER);
                arrByte[1] = Convert.ToByte(CMD_VALUE);
                arrByte[2] = Convert.ToByte(CMD_PARAM);
                arrByte[3] = Convert.ToByte(CMD_LENGTH);
                byte[] arayByte = Encoding.ASCII.GetBytes(CMD_DATA);
                for (int i = 0; i < arayByte.Length; i++)
                {
                    try
                    {
                        arrByte[i + 4] = arayByte[i];
                    }
                    catch (Exception)
                    {
                        arrByte[i + 4] = 48;
                    }
                }
                srPort.Write(arrByte, 0, arrByte.Length);
            }
            catch (Exception)
            {
            }
        }

        private void SendCommand(string PRINT_DATA)
        {
            try
            {
                if (srPort.IsOpen)
                {
                    srPort.WriteLine(PRINT_DATA);
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion SendCommand

        #region getValueFromConf
        private string getValueFromConf(String Key)
        {
            try
            {
                return ConfigurationManager.AppSettings[Key].ToString();
            }
            catch (Exception objEx)
            {
            }
            return "";
        }
        #endregion

    }
}
