<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetPassword.aspx.cs" Inherits="ProjectSmartCargoManager.SetPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="style/style.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
        function CheckPwd() {
            var regEx = /(?=.*?[A-Za-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*]).{6,25}$/
            var pwd = document.getElementById("<%= txtNewPwd.ClientID%>").value;
            var Conpwd = document.getElementById("<%= txtNewConformPwd.ClientID%>").value;
            var HdnPwds = document.getElementById("<%= hdnPwds.ClientID%>").value;
            var HdnCurrPwd = document.getElementById("<%= hdnCurrPassword.ClientID%>").value;
            var PwdList = HdnPwds.split(',');
            var arraycontains = PwdList.indexOf(pwd);
            if (arraycontains > -1) {
                alert('Please select different Password');
                return false;
            }
//            else if (!(regEx.test(pwd))) {
//            var error = 'Password must be minimum 6 letters and must contain 1 alphabet,1 Number and One of Special Characters "#?!@$%^&*"';
//                alert(error);
//                return false;
//            }
            else if (Conpwd != pwd) {
                alert('Confirm Password does not match');
                return false;
            }
//            else if (Oldpwd != HdnCurrPwd) {
//                alert('Old Password Not Correct');
//                return false;
//            }
            return true;
        }

        function ShowConfirmMsg() {
            alert('New Password has been set successfully');
            return true;
        }
    </script>
</head>
<body style="background:#fff;">
    <form id="form1" runat="server">
    <div class="divback" style="width:300px; margin:10px auto;" >
    
   
    
        <h2>Update Password</h2>
         <asp:Label ID="lblStatus" runat="server" Font-Size="Large" Font-Bold="true"></asp:Label>
        <div>
           <table cellpadding="3" cellspacing="3">
            <tr>
                <td>UserName:</td>
                <td>
                    <asp:Label ID="lblUserName" runat="server" Font-Bold="true"></asp:Label>
                </td>
            </tr>
           <%--<tr>
                <td>Old Password:</td>
                <td>
                    <asp:TextBox ID="txtOldPwd" runat="server" Visible="true"></asp:TextBox>
                </td>
            </tr>--%>
            <tr>    
                <td>New Password:</td>
                <td>
                    <asp:TextBox ID="txtNewPwd" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>    
                <td>Confirm Password:</td>
                <td>
                    <asp:TextBox ID="txtNewConformPwd" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSet" runat="server" Text="Set" CssClass="button" 
                        onclick="btnSet_Click" OnClientClick="javascript:return CheckPwd();" />
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" 
                        onclick="btnClose_Click"/>
                </td>
            </tr>
           </table> 
        </div>
    </div>
    <asp:HiddenField ID="hdnPwds" runat="server" />
    <asp:HiddenField ID="hdnCurrPassword" runat="server" />
    </form>
</body>
</html>
