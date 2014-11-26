using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using GenCode128;

public partial class GetBarcodeImage : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (int.Parse(Session["Pcs"].ToString()) < int.Parse(Session["Pcs"].ToString()))
            return;

        string BarcodeValue = Session["AWBPartI"].ToString() + "" + Session["AWBPartII"].ToString() + "" + Session["PcsCount"].ToString();

        System.Drawing.Image myimg = Code128Rendering.MakeBarcodeImage(BarcodeValue, 3, true);
        
        Bitmap objbitmap = new Bitmap(myimg);
        Graphics objGraphics = Graphics.FromImage(objbitmap);
                
        objbitmap.Save(Response.OutputStream, ImageFormat.Jpeg);

        objbitmap.Dispose();
        objGraphics.Dispose();
        Response.End();

        Session["PcsCount"] = int.Parse(Session["PcsCount"].ToString()) + 1;
    }

    
}
