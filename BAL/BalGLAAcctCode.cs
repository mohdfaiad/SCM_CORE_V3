using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration; 
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class BalGLAAcctCode
    {
        #region Variables

        string constr = ConfigurationManager.ConnectionStrings["ConStr"].ToString();

        #endregion Variables

        #region Save GLA
        public bool SaveGLADetials(string selection, string GLADesc, string AcctCode)
        {
            try
            {
                SQLServer db = new SQLServer(constr);
                bool flag = false;

                string[] paramname = new string[3];
                paramname[0] = "selection";
                paramname[1] = "GLADesc";
                paramname[2] = "AcctCode";

                object[] paramvalue = new object[3];
                paramvalue[0] = selection;
                paramvalue[1] = GLADesc;
                paramvalue[2] = AcctCode;

                SqlDbType[] paramtype = new SqlDbType[3];
                paramtype[0] = SqlDbType.VarChar;
                paramtype[1] = SqlDbType.VarChar;
                paramtype[2] = SqlDbType.VarChar;

                flag = db.UpdateData("spAddGLcodes", paramname, paramtype, paramvalue);
                return flag;

            }
            catch (Exception ex)
            {
                
            }
            return false;
        }
        #endregion

    }
}
