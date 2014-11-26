using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectSmartCargoManager
{
    public partial class BI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Designer1.ResourcePath = "http://scmcebureports.cloudapp.net/TemporaryResource.axd"; //SessionStorage.ResourcePath;
            Designer1.SecurityToken = "2A90783BA9D56E73B06DBE720C7819AF3C9B71857D39527B74E7C2596CDC01002322CFBD1751C404BA9778C445CA89D9D2AD373AA4D27077759ACD2E549288CACE356A194A384439F66377AB50D0BB82C2E0B0F9439F56F911F7F32DEF3909E731FA49CE4C0FE8312F3D552DB3D0E0EA28010859FB164DE83A1040F17E18BD58"; //SessionStorage.SecurityToken;
 
        }
    }
}
