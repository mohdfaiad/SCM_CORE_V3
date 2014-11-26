using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using nsBalInterface;

namespace IAMCEBInterface.asmx
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {

        [WebMethod]
        public DataSet GetFlightDetails(String strFlightDate)
        {
            try
            {
                DateTime dtTemp=Convert.ToDateTime(strFlightDate );

                //DateTime dtTemp = new DateTime(2013, 12, 09);
                return GetFlightDetails(dtTemp, "" );
            }
            catch (Exception objEx)
            {
                // Err=" Error while getting flight information due to :- "+objEx.Message;
            }
            return null;
        }

        public DataSet GetFlightDetails(DateTime  dtFlightDate)
        {
            try
            {                
                return GetFlightDetails(dtFlightDate, "");
            }
            catch (Exception objEx)
            {
                // Err=" Error while getting flight information due to :- "+objEx.Message;
            }
            return null;
        }


        public DataSet  GetFlightDetails(DateTime objDate, String strFlightNo)
        {
            try
            {
                BalInterface objBal = new BalInterface();
                return objBal.GetFlightDetails(objDate, strFlightNo);
            }
            catch(Exception objEx)
            {
                // Err=" Error while getting flight information due to :- "+objEx.Message;
            }
            return null;
        }
    }
}
