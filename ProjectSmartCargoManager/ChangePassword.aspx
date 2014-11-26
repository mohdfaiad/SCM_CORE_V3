<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master"  Inherits="ProjectSmartCargoManager.ChangePassword" Title="Change Password"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 60%;
        }
    </style>
    
    <script type="text/javascript">
        function CheckPwd() {
            var regEx = /(?=.*?[A-Za-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*]).{6,25}$/
            var pwd = document.getElementById("<%= txtNewPwd.ClientID%>").value;
            var Conpwd = document.getElementById("<%= txtNewConformPwd.ClientID%>").value;
            var oldPwd = document.getElementById("<%= txtOldPwd.ClientID%>").value;

            var HdnPwds = document.getElementById("<%= hdnPwds.ClientID%>").value;
            var PwdList = HdnPwds.split(',');
            var arraycontains = PwdList.indexOf(pwd.toLowerCase());

            if (arraycontains > -1) {
                alert('Please select different Password');
                return false;
            }
//            else if (!(regEx.test(pwd))) {
//            var error = 'Password must be minimum 6 letters and must contain 1 alphabet,1 Number and One of Special Characters "#?!@$%^&*"';
//                alert(error);
//                return false;
//            }
            else {
                return true;
            }
        }

        function Validate() {
            var regEx = /(?=.*?[A-Za-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*]).{6,25}$/
            var pwd = document.getElementById("<%= txtNewPwd.ClientID%>").value;
            var Conpwd = document.getElementById("<%= txtNewConformPwd.ClientID%>").value;
            var oldPwd = document.getElementById("<%= txtOldPwd.ClientID%>").value;

            var HdnPwds = document.getElementById("<%= hdnPwds.ClientID%>").value;
            var PwdList = HdnPwds.split(',');

            var arraycontains = PwdList.indexOf(pwd.toLowerCase());
            
            if (oldPwd == "") {
                alert('Please Enter Old Password');
                return false;
            }
            else if (pwd == "") {
                alert('Please Enter New Password');
                return false;
            }
            else if (Conpwd == "") {
                alert('Please Confirm New Password');
                return false;
            }
            else if (Conpwd != pwd) {
                alert('New and Confirm Passwords do not match');
                return false;
            }
            else if (arraycontains > -1) {
                alert('Please select different Password');
                return false;
            }
//            else if (!(regEx.test(pwd))) {
//            var error = 'Password must be minimum 6 letters and must contain 1 alphabet,1 Number and One of Special Characters "#?!@$%^&*"';
//                alert(error);
//                return false;
//            }
            return true;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<div id="contentarea">
<div class="msg">
<asp:Label ID="lblMsg" runat="server" ForeColor="#FF3300" Font-Bold="true" Font-Size="Large"></asp:Label>
</div>
    <h1>
    Change Password
    </h1>
    <table class="style1" cellpadding="3" cellspacing="3">
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">                

            </td>            
        </tr>
        <tr>
            <td align="left" style="width:13%">
                User Name:
                <%--<asp:Label ID="lblUserName" runat="server" Text="User Name:"></asp:Label>--%>
            </td>
            <td style="width:25%" align="left">
                <asp:TextBox ID="txtUserName" runat="server" CssClass="inputbg" Enabled="false"></asp:TextBox>
            </td>
            <td style="width:25%">
                &nbsp;</td>
        </tr>
        <tr >
            <td style="width:13%" align="left">
                Old Password:
                <%--<asp:Label ID="lblOldPwd" runat="server" Text="Old Password:"></asp:Label>--%>
            </td>
            <td style="width:25%">
                <asp:TextBox ID="txtOldPwd" runat="server" CssClass="inputbg" TextMode="Password"></asp:TextBox>
            </td>
            <td style="width:25%">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width:13%" align="left">
                <%--<asp:Label ID="lblNewPwd" runat="server" Text="New Password:"></asp:Label>--%>
                New Password:</td>
            <td style="width:25%">
                <asp:TextBox ID="txtNewPwd" runat="server" CssClass="inputbg"
                ToolTip="1 alphabet,1 Number and One of Special Characters '#?!@$%^&*'" TextMode="Password" onchange="javascript:return CheckPwd();"></asp:TextBox>
            </td>
            <td style="width:25%">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width:13%" align="left">
                <%--<asp:Label ID="lblNewConformPwd" runat="server" Text="Confirm Password:"></asp:Label>--%>
                Confirm Password:</td>
            <td style="width:25%">
                <asp:TextBox ID="txtNewConformPwd" runat="server" CssClass="inputbg" TextMode="Password"></asp:TextBox>
                <%--<asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToCompare="txtNewPwd" ControlToValidate="txtNewConformPwd" 
                    ErrorMessage="CompareValidator"></asp:CompareValidator>--%>
            </td>
            <td style="width:25%">
                &nbsp;</td>
        </tr>
        <tr>
                 <td align="center">
                 &nbsp;</td>
            <td align="left">
              <a href="#" class="redbutton red"><span>
              <asp:Button ID="bnShow" runat="server"  Text="Change Password" CssClass="button" 
              onclick="bnShow_Click" OnClientClick="javascript:return Validate();" Height="23px"/></span></a>
                   
            </td>
           
            <td style="width:25%">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                &nbsp;</td>
            <td style="width:25%">
                &nbsp;</td>
        </tr>
    </table>
</div>
    <asp:HiddenField ID="hdnPwds" runat="server" />
</asp:Content>

