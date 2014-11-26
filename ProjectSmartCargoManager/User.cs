using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using QID.DataAccess;

namespace ProjectSmartCargoManager
{
    public class User
    {

       public  string UserName="", RoleName = "", RoleType = "", LoginStation = "";
       public  string RoleID="";

       public DataSet UserPermissionsDS = new DataSet();
       public DataSet UserRoleMenuAccessDS = new DataSet();
        
        
        
    }
     
}
