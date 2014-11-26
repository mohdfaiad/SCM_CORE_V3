using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Drawing; 
//using System.;

namespace BAL
{
    public class ClsImageConversion
    {
        private List<Point> m_listPoints = new List<Point>();
        private Graphics m_gfxCanvas;
        static private Pen m_penLine = new Pen(Color.Black);

        public byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                // MemoryStream ms = new MemoryStream();
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();

                ///data compression

            }
            catch (Exception ex)
            {
                return ms.ToArray();
                //MessageBox.Show(ex.Message);
            }

        }

            public Image  byteArrayToImage(byte[] b)
            {
                MemoryStream ms = new MemoryStream(b);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
                //Image img = new Bitmap(ms);
                // DOSignImg = APP_PATH + "\\DOSignImage.bmp";
                //img.Save(APP_PATH + "\\ByteToImage.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                //return img;

            }
            

    }


}

