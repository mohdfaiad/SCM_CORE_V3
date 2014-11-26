<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendFFR.aspx.cs" Inherits="ProjectSmartCargoManager.SendFFR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">

        function Close() {
            window.close();
        }
    
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table style="width:100%;">
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="Large" 
                        Text="FFR"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width:150px" valign="top">
                    Message:</td>
                <td>
                    <asp:TextBox ID="txtMessage" runat="server" Rows="15" TextMode="MultiLine" 
                        Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width:150px">
                    SITA Recipients:</td>
                <td>
                    <asp:TextBox ID="txtSITARecipients" runat="server" Rows="3" TextMode="MultiLine" 
                        Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="width:150px">
                    E-Mail Recipients:</td>
                <td>
                    <asp:TextBox ID="txtEmailRecipients" runat="server" Rows="3" TextMode="MultiLine" 
                        Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td  style="width:150px">
                    SMS Recipients:</td>
                <td>
                    <asp:TextBox ID="txtSMSRecipients" runat="server" Rows="3" TextMode="MultiLine" 
                        Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnSend" runat="server" Text="Send" onclick="btnSend_Click" />
&nbsp;
                    <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="javascript:Close()"/>
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
