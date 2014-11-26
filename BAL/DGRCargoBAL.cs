using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QID.DataAccess;
namespace BAL
{
    

   public class DGRCargoBAL
    {
        SQLServer db = new SQLServer("");
        string constr = "";

        public DGRCargoBAL()
        {
            constr = Global.GetConnectionString();
        }
        #region SaveDGRCargo
        public bool SaveDGRCargo(string AWBNumber, string UNID, int Pieces,string Weight,string ERGCode,string UpdatedBy,string UpdatedOn,string AWBPrefix, string PG,string Desc)
        {
            DataSet res=new DataSet() ;
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = new DataSet();

                string[] Pname = new string[10];
                object[] Pvalue = new object[10];
                SqlDbType[] Ptype = new SqlDbType[10];

                Pname[0] = "AWBNumber";
                Pname[1] = "UNID";
                Pname[2] = "Pieces";
                Pname[3] = "Weight";
                Pname[4] = "ERGCode";
                Pname[5] = "UpdatedBy";
                Pname[6] = "UpdatedOn";
                Pname[7] = "AWBPrefix";
                Pname[8] = "PG";
                Pname[9] = "Desc";

                Pvalue[0] = AWBNumber;
                Pvalue[1] = UNID;
                Pvalue[2] = Pieces;
                Pvalue[3] = Weight;
                Pvalue[4] = ERGCode;
                Pvalue[5] = UpdatedBy;
                Pvalue[6]=UpdatedOn;
                Pvalue[7] = AWBPrefix;
                Pvalue[8] = PG;
                Pvalue[9] = Desc;


                Ptype[0] = SqlDbType.VarChar;
                Ptype[1] = SqlDbType.VarChar;
                Ptype[2] = SqlDbType.Int;
                Ptype[3] = SqlDbType.VarChar;
                Ptype[4] = SqlDbType.VarChar;
                Ptype[5] = SqlDbType.VarChar;
                Ptype[6] = SqlDbType.VarChar;
                Ptype[7] = SqlDbType.VarChar;
                Ptype[8] = SqlDbType.VarChar;
                Ptype[9] = SqlDbType.VarChar;

                SQLServer db = new SQLServer(Global.GetConnectionString());

                return db.ExecuteProcedure("SPInsertDGRCargo", Pname, Ptype, Pvalue);
            }
               
            catch (Exception ex)
            {
                return false  ; 
            }
            
        }
        #endregion SaveDGRCargo

    }
    
}
