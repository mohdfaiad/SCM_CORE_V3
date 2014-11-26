using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using QID.DataAccess;

namespace BAL
{
    public class AircraftBAL
    {
       string constr = "";
        public AircraftBAL()
        {
            constr = Global.GetConnectionString();
       
        }
        
        #region SaveAircraftEquipment
        public string SaveAircraftEquipment(object[] AcEq)
        {
            try 
            {
                string[] ParamNames = new string[] { "Manufacturer", "AircraftType", "Version", "Count", "PassengerCapacity", "LandingWeight", "CargoCapacity", "MTOW", "Rwt","Rl","Rb","Rh","Vl","RUnit","VUnit","AIdentity"};
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                string res = da.GetStringByProcedure("spSaveAircraft", ParamNames, AcEq, ParamTypes);
                
                    return res;
              
            }
            catch (Exception)
            {
                return "Error";
            }
            
        }
        #endregion SaveAircraftEquipment

        # region GetAircraftEquipment
        public DataSet GetAircraftEquipment(object[] AcEq)
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                string[] ParamNames = new string[] {"AircraftType", "Version" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar };
                DataSet ds = da.SelectRecords("spGetAircraftEquipment", ParamNames, AcEq, ParamTypes);
                //DataSet ds = da.SelectRecords("spGetAircraftEquipment", "Manufacturer", AcInfo, SqlDbType.VarChar);
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            return (ds);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return (null);


        }
        #endregion GetAircraftEquipment

        #region SaveEquipment
        public string SaveEquipment(object[] AcEq)
        {
            try
            {
                string[] ParamNames = new string[] { "AircraftType", "Version","TailNo", "Status" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                string res = da.GetStringByProcedure("spSaveEquipment", ParamNames, AcEq, ParamTypes);
               
                    return res;
               
            }
            catch (Exception)
            {
                return "Error";
            }
            
        }
        #endregion SaveEquipment

        #region EditEquipment
        public int EditEquipment(object[] AcEq)
        {
            try
            {
                string[] ParamNames = new string[] { "Manufacturer", "AircraftType", "Version", "PassengerCapacity", "LandingWeight", "CargoCapacity", "MTOW", "TailNo", "Status", "Rl", "Rb", "Rh", "Vl","RUnit","VUnit","Srno","AIdentity"};
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar,
                SqlDbType.Int,SqlDbType.VarChar};
                SQLServer da = new SQLServer(constr);
                bool res = da.UpdateData("spEditEquipment", ParamNames, ParamTypes, AcEq);
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            return (-1);
        }
        #endregion EditEquipment

        #region DeleteEquipment
        public int DeleteEquipment(object[] AcEq)
        {
            try
            {
                string[] ParamNames = new string[] { "AircraftType", "Version", "TailNo" };
                SqlDbType[] ParamTypes = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar };
                SQLServer da = new SQLServer(constr);
                bool res = da.UpdateData("spDeleteAircraftEquipment", ParamNames, ParamTypes, AcEq);
                if (res)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
            }
            return (-1);
        }
        #endregion DeleteEquipment

        # region GetAircraftList
        public DataSet GetAircraftList()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetAircraftList");
               if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            return (ds);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return (null);


        }
        #endregion GetAircraftList

        # region GetTailNo
        public DataSet GetTailNo()
        {
            try
            {
                SQLServer da = new SQLServer(constr);
                DataSet ds = da.SelectRecords("spGetAircraftTailNo");
                if (ds != null)
                {
                    if (ds.Tables != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            return (ds);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return (null);


        }
        #endregion GetTailNo
    }
}
