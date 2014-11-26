using System;
using System.Configuration;
using System.Web;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

namespace ProjectSmartCargoManager
{
   
    public class ShowTracerImages : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Int32 SrNum;
            if (context.Request.QueryString["SrNo"] != null)
            {
                SrNum = Convert.ToInt32(context.Request.QueryString["SrNo"]);
            }
            else
                throw new ArgumentException("No parameter specified");

            context.Response.ContentType = "image/png";
            Stream strm1 = ShowTracerImage(SrNum);
            byte[] buffer1 = new byte[9000];
            int byteSeq1 = strm1.Read(buffer1, 0, 9000);
            while (byteSeq1 > 0)
            {
                context.Response.OutputStream.Write(buffer1, 0, byteSeq1);
                byteSeq1 = strm1.Read(buffer1, 0, 9000);
            }

        }

        #region Dispay Tracer Image
        public Stream ShowTracerImage(int Srno)
        {
            string conn = Global.GetConnectionString();
            SqlConnection connection = new SqlConnection(conn);

            string sql = "SELECT  Attachment FROM tblMessageAttachements WHERE SerialNumber = @ID ";

            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", Srno);
            connection.Open();
            object img = cmd.ExecuteScalar();
            try
            {
                return new MemoryStream((byte[])img);
            }
            catch
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
