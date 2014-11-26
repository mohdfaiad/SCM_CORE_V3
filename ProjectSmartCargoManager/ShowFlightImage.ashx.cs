using System;
using System.Configuration;
using System.Web;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

namespace ProjectSmartCargoManager
{
   
    public class ShowFlightImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Int32 imageid;
            if (context.Request.QueryString["id"] != null)
            {
                imageid = Convert.ToInt32(context.Request.QueryString["id"]);
            }
            else
                throw new ArgumentException("No parameter specified");

            context.Response.ContentType = "image/png";
            Stream strm = ShowEmpImage(imageid);
            byte[] buffer = new byte[9000];
            int byteSeq = strm.Read(buffer, 0, 9000);
            while (byteSeq > 0)
            {
                context.Response.OutputStream.Write(buffer, 0, byteSeq);
                byteSeq = strm.Read(buffer, 0, 9000);
            }
            //context.Response.BinaryWrite(buffer);
        }

        public Stream ShowEmpImage(int empno)
        {
            string conn = Global.GetConnectionString();
            SqlConnection connection = new SqlConnection(conn);
            string sql = "SELECT  Document FROM tblFlightUploadedDocuments WHERE SerialNumber = @ID ";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", empno);
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
