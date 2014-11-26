using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectSmartCargoManager
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string str = "1,0,0,0,1,1,0";
            string[] chkWeekdays = str.ToString().Split(',');
           string date="13/08/2012";
        

           string dateday= DateTime.Parse("08/15/2012").DayOfWeek.ToString();
          
            int dateok=0;
            if (dateday == "Sunday")
                dateok = 1;
            if (dateday == "Monday")
                dateok = 2;
            if (dateday == "Tuesday")
                dateok = 3;
            if (dateday == "Wednesday")
                dateok = 4;
            if (dateday == "Thursday")
                dateok = 5;
            if (dateday == "Friday")
                dateok = 6;
            if (dateday == "Saturday")
                dateok = 7;

            if(str[dateok].ToString()=="1")
                Label1.Text = "08/13/2012";
            else
                Label1.Text = "Old Date";

            string AWBno = "SG50231996";

            AWBno = AWBno.Substring(AWBno.Length-8);
            Label1.Text = AWBno; 
           // if(

          


                                           
                                            
        }
    }
}
