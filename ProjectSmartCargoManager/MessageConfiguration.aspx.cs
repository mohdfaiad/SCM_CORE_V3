using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data;
using BAL;
using System.Text.RegularExpressions;

namespace ProjectSmartCargoManager
{
    public partial class MessageConfiguration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClearAll();

            }
          //  List<string> Str1 = new List<string>();
          // Str1= ProcessMail();
        }

        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            object[] MsgInfo = new object[6];
            int i = 0;
            if (Page.IsValid)
            {
                try
                {

                    MsgInfo.SetValue(txtOrigin.Text.ToUpper(), i);
                    i++;
                    MsgInfo.SetValue(txtDestination.Text.ToUpper(), i);
                    i++;
                    MsgInfo.SetValue(txtFltNo.Text.ToUpper() == string.Empty ? "ALL" : txtFltNo.Text.ToUpper(), i);
                    i++;
                    MsgInfo.SetValue(txtEmailFFM.Text.TrimEnd(','), i);
                    i++;
                    MsgInfo.SetValue(txtEmailFTX.Text.TrimEnd(','), i);
                    i++;
                    MsgInfo.SetValue(txtEmailFSU.Text.TrimEnd(','), i);
                    MessageConfigurationBAL Conf = new MessageConfigurationBAL();
                    string Result = Conf.SaveConfiguratin(MsgInfo);

                    if (Result == "pass" || Result == "updated")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        lblStatus.Text = "Data saved successfully";
                        ClearAll();
                    }

                    else
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        lblStatus.Text = "Please select different data to enter";

                    }
                }
                catch (Exception)
                { }
            }
            
               
 
        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        protected void TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            ClearAll();
            lblStatus.Text = "";
        }


        private void ClearAll()
        {
            txtOrigin.ReadOnly = false;
            txtDestination.ReadOnly = false;
            txtFltNo.ReadOnly = false; 
            txtOrigin.Text = txtDestination.Text = txtFltNo.Text = txtEmailFFM.Text = txtEmailFTX.Text = txtEmailFSU.Text = "";

        }

        protected void btnList_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "";
               
                DataSet ds = new DataSet();
                object[] MsgInfo = new object[3];
                int i = 0;
                MsgInfo.SetValue(txtOrigin.Text, i);
                i++;
                MsgInfo.SetValue(txtDestination.Text, i);
                i++;
                MsgInfo.SetValue(txtFltNo.Text.ToUpper() == string.Empty ? "ALL" : txtFltNo.Text.ToUpper(), i);
               
              //  MsgInfo.SetValue(txtFltNo.Text, i);
                MessageConfigurationBAL Conf = new MessageConfigurationBAL();
                ds = Conf.ListConfiguration(MsgInfo);
                if (ds != null)
                {
                    txtEmailFFM.Text = ds.Tables[0].Rows[0][3].ToString();
                    txtEmailFTX.Text = ds.Tables[0].Rows[0][4].ToString();
                    txtEmailFSU.Text = ds.Tables[0].Rows[0][5].ToString();
                    txtOrigin.ReadOnly = true;
                    txtDestination.ReadOnly = true;
                    txtFltNo.ReadOnly = true;
    


                }

            }
            catch (Exception)
            {
                
               
            }


        }
        #region Test
        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    List<string> Str1 = new List<string>();
        //    Regex regExFlight = new Regex(@"(?<Sno>\d{0,})[/](?<Flt>\w{0,})[/](?<FltDate>\w{0,})[/](?<Ori>\w{3})(?:([/](?<RegNo>[\w\d-]{0,}))|)");
        //               string Mail ="FFM/4\r\n1/IT352/29OCT/DEL/VTKFD\r\nIXJ\r\n090-12796302DELIXJ/T1K1.0/COMPANY CARGO/GEN\r\n090-31716241DELIXJ/T30K211.0/MOBILE/GEN\r\n090-12894243DELIXJ/T1K6.0/PRINTED MTRL/GEN\r\n090-12796276DELIXJ/T1K5.0/COMPANY CARGO/GEN\r\n090-31716392DELIXJ/T20K78.0/MOBILE/GEN\r\nLAST";

        //               regExFlight.Match(Mail);
        //    // Str1= ProcessMail();
        //}

        //protected List<string> ProcessMail()
        //{
        //    string POL, POU = "", FltNo, manDate, Stemp = "";
        //    List<string> Str = new List<string>();
        //    //string Mail ="FFM/4\r\n1/IT352/29OCT/DEL/VTKFD\r\nIXJ\r\n090-12796302DELIXJ/T1K1.0/COMPANY CARGO/GEN\r\n090-31716241DELIXJ/T30K211.0/MOBILE/GEN\r\n090-12894243DELIXJ/T1K6.0/PRINTED MTRL/GEN\r\n090-12796276DELIXJ/T1K5.0/COMPANY CARGO/GEN\r\n090-31716392DELIXJ/T20K78.0/MOBILE/GEN\r\nLAST";
        //    string Mail = "FFM/4\r\n1/IT062/31OCT/CMB\r\nMAA/NIL\r\nCJB/NIL\r\nLAST\r\nSENT";
        //    //string Mail = "FFM/8\r\n1/SG152/10JUL/BOM\r\nSTV\r\nSG-50231252BOMDEL/T20K200CC5/   /GEN\r\nDEL\r\nSG-50231296BOMDEL/T30K300CC5/   /GEN\r\nSG-50231285BOMDEL/T20K200CC5/   /GEN\r\nLAST";
        //    #region Conversion of FFM single line string to standard Format


        //    int cnt;
        //    cnt = 0;

        //    Mail = Mail.Replace("\r\n", "$");
        //    Mail = Mail.Replace("-", "/");
        //    while (cnt < Mail.Length)//
        //    {
        //        if (Mail[cnt] != '$')
        //        {
        //            Stemp += Mail[cnt];
        //        }
        //        if (Mail[cnt] == '$')
        //        {

        //            Str.Add(Stemp + '/');
        //            Stemp = "";
        //        }

        //        cnt++;
        //    }
        //    #endregion
        //    #region Processing 2'nd line of FFM Message
        //    int j = 0;
        //    string Summary = "";
        //    string FlightSummary = Str[1].Trim();
        //    List<string> msgFlightSummary = new List<string>();
        //    while (j < FlightSummary.Length)//Processing 2'nd line of FFM Message 
        //    {
        //        if (FlightSummary[j] != '/')
        //        {
        //            Summary += FlightSummary[j];
        //        }
        //        if (FlightSummary[j] == '/')
        //        {
        //            msgFlightSummary.Add(Summary);
        //            Summary = "";
        //        }
        //        j++;
        //    }
        //    msgFlightSummary.RemoveAt(0);
        //    #endregion
        //    #region Processing rest of FFM message in Simple Row Format
        //    List<string> msgStr = new List<string>();
        //    int i = 2;//Skip 2 Rows already Processed 
        //    while (i < Str.Count)
        //    {
        //        if (Str[i].Trim().Length == 4)
        //        {
        //            POU = Str[i].ToString();
        //            // i++;
        //        }
        //        if (Str[i].Trim().Length == 8)//NIL FFM's
        //        {
        //            msgStr.Add(Str[i].ToString());
        //            //  msgStr.Add(POU.Substring(0, 3) + "/" + Str[i]);
        //            //i++;

        //        }
        //        if (Str[i].Trim().Length > 8)// (>8)To satisfy with Nil else (>4)Ok
        //        {
        //            msgStr.Add(POU.Substring(0, 3) + "/" + Str[i]);
        //            //i++;

        //        }
        //        i++;
        //    }

        //    #endregion

        //    #region Parsing Fields
        //    string msgStrTemp = "";
        //    Summary = "";
        //    string[,] fields = new string[msgStr.Count, 6];
        //    int l;
        //    for (int k = 0; k < msgStr.Count; k++)
        //    {
        //        msgStrTemp = msgStr[k];
        //        l = 0;
        //        cnt = 0;
        //        while (l < msgStrTemp.Length)
        //        {
        //            if (msgStrTemp[l] != '/')
        //            {
        //                Summary += msgStrTemp[l];
        //            }
        //            if (msgStrTemp[l] == '/')
        //            {
        //                fields[k, cnt] = Summary;
        //                cnt++;
        //                Summary = "";
        //            }
        //            l++;
        //        }
        //    }
        //    #endregion

        //    #region Adding To Manifest
        //    int c = 0;
        //    string val = "", AWB = "", ORG = "", DEST = "";
        //    for (int x = 0; x < fields.Length / 6; x++)
        //        for (int y = 0; y < 6; y++)
        //            if (fields[x, y] == null)
        //                fields[x, y] = "";

        //    while (c < fields.Length / 6)
        //    {
        //        if (fields[c, 2] != "")
        //        {
        //            AWB = fields[c, 2].Substring(0, 8);
        //            ORG = fields[c, 2].Substring(8, 3);
        //            DEST = fields[c, 2].Substring(11, 3);
        //        }
        //        else
        //            AWB = ORG = DEST = "";




        //        val = msgFlightSummary[0].ToString() + msgFlightSummary[1].ToString() + msgFlightSummary[2].ToString();
        //        val += fields[c, 0].ToString() + fields[c, 1].ToString() + AWB + ORG + DEST + fields[c, 3].ToString() + fields[c, 4].ToString() + fields[c, 5].ToString();
        //        c++;
        //    }
        //    #endregion
        //   return msgStr;//Str //msgStr   msgFlightSummary;   

        //}
#endregion
    }
}
