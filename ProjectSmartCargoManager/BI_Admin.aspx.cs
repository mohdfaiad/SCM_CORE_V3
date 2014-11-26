using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectSmartCargoManager
{
    public partial class BI_Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Designer1.ResourcePath = "http://scmcebureports.cloudapp.net/TemporaryResource.axd"; //SessionStorage.ResourcePath;
            Designer1.SecurityToken = "A841A3D7A4589C797107BCF8C3D16F0AA7F5481AA001A7A43FA7AF42485D447AB58B23D85E30F5175F9630C69F83C8BD492260D018220BF673CB2793C1C2D5F8660FF88B8B43D25495183B7B9F24768B3316F052C20D94F925B4CC85F7425F59B6FF89BD0B71A1DFFFFEB36E529D7DC69D459DA91FBDA50C264E0C425C78989A"; //SessionStorage.SecurityToken;
        }
    }
}
