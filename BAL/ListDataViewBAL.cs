using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QID.DataAccess;

namespace BAL
{
    public class ListDataViewBAL
    {
        SQLServer db = new SQLServer(Global.GetConnectionString());

        public bool FillDdl(string DDL, ref DataSet dsResult, ref string errormessage, string[] parameters)
        {
            try
            {
                errormessage = "";
                switch (DDL)
                {
                    case "Origin":

                        string[] parama = { "level"};
                        SqlDbType[] dbtypes = { SqlDbType.VarChar};
                        object[] values = { parameters[0] };

                        dsResult = db.SelectRecords("SP_GetOrigin", parama, values, dbtypes);
                        if (dsResult != null)
                        {

                            if (dsResult.Tables.Count >0)
                            {
                                return true;
                            }
                            else
                            {
                                errormessage = "Error : (SP_GetOrigin) table count is zero";
                                return false;
                            }

                        }
                        else
                        {
                            errormessage = "Error : (SP_GetOrigin) ds is null";
                            return false;
                        }

                        break;



                    case "Currency":

                        dsResult = db.SelectRecords("SP_GetCurrency");
                        if (dsResult != null)
                        {

                            if (dsResult.Tables.Count > 0)
                            {
                                return true;
                            }
                            else
                            {
                                errormessage = "Error : (SP_GetCurrency) table count is zero";
                                return false;
                            }

                        }
                        else
                        {
                            errormessage = "Error : (SP_GetCurrency) ds is null";
                            return false;
                        }
                        break;


                    case "Param":

                        parama = new string[] { "ParamName" };
                        dbtypes = new SqlDbType[] { SqlDbType.VarChar};
                        values = new object[] { parameters[0] };

                        dsResult = db.SelectRecords("SP_GetSpecParameterData",parama,values,dbtypes);
                        if (dsResult != null)
                        {

                            if (dsResult.Tables.Count > 0)
                            {
                                return true;
                            }
                            else
                            {
                                errormessage = "Error : (SP_GetSpecParameterData) table count is zero";
                                return false;
                            }

                        }
                        else
                        {
                            errormessage = "Error : (SP_GetCurrency) ds is null";
                            return false;
                        }
                        break;


                    case "Code":

                        dsResult = db.SelectRecords("SP_GetOtherChargesCode");
                        if (dsResult != null)
                        {

                            if (dsResult.Tables.Count > 0)
                            {
                                return true;
                            }
                            else
                            {
                                errormessage = "Error : (SP_GetOtherChargesCode) table count is zero";
                                return false;
                            }

                        }
                        else
                        {
                            errormessage = "Error : (SP_GetOtherChargesCode) ds is null";
                            return false;
                        }
                        break;

                    case "MsgType":

                        dsResult = db.SelectRecords("SP_GetMessageType");
                        if (dsResult != null)
                        {

                            if (dsResult.Tables.Count > 0)
                            {
                                return true;
                            }
                            else
                            {
                                errormessage = "Error : (SP_GetMessageType) table count is zero";
                                return false;
                            }

                        }
                        else
                        {
                            errormessage = "Error : (SP_GetMessageType) ds is null";
                            return false;
                        }
                        break;

                }
                
                return true;
            }
            catch (Exception ex)
            {
                errormessage = "" + ex.Message;
                return false;
            }
        }


    }
}
