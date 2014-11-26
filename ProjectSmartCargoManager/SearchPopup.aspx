<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchPopup.aspx.cs" Inherits="ProjectSmartCargoManager.SearchPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Search</title>
    <style type="text/css">
        .style1
        {
        }
        .style2
        {
            width: 176px;
        }
    </style>
    
    <script type="text/javascript">

        //Function to return value to parent window...
        function setValues(ctrl1, parctrl1, ctrl2, parctrl2) {
            
            var val1, val2;
            if (ctrl1 != 'null') {
                val1 = document.getElementById(ctrl1).value;
                opener.document.getElementById(parctrl1).value = val1;
            }
            if (ctrl2 != 'null') {
                val2 = document.getElementById(ctrl2).value;
                opener.document.getElementById(parctrl2).value = val2;
            }
//            if (code.length < 1) {
//                alert("Please enter Code correctly!");
//                return false;
//            }
//            opener.document.getElementById("ctl00_ContentPlaceHolder1_txtOrg").value = code;
            self.close();
            return false;
        }
         
</script>
</head>

<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 263px">
            <tr>
                <td align="center" class="style1" colspan="2">
                    <asp:Label ID="lblHeader" runat="server" Font-Size="Medium" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" class="style1">
                    <asp:Label ID="lblCode" runat="server" Text="Code"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" class="style1">
                    <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    &nbsp;</td>
                <td class="style2">
                    <asp:Button ID="btnList" runat="server" Text="List" onclick="btnList_Click" />
                </td>
            </tr>
            </table>
    
    </div>
    </form>
</body>
</html>
