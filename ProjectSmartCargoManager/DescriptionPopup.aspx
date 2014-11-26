<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DescriptionPopup.aspx.cs" Inherits="ProjectSmartCargoManager.DescriptionPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="style/style.css" rel="stylesheet" type="text/css" />

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function SaveDescription() {
            var txtDesc = document.getElementById("<%= txtDescription.ClientID %>");
            opener.document.getElementById('<%= Request["DescriptionID"] %>').value = txtDesc.value;
            window.close();
        }
    </script>
</head>
<body style="background:#FFFFFF;">
    <form id="form1" runat="server">
    <div style="margin:6px;">
    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" MaxLength="1000" Width="378px" Height="278px" BorderColor="Gray" BorderWidth="2px" BorderStyle="Solid"></asp:TextBox>
    
    </div>
    <div style="margin:6px;">
    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClientClick="javascript:SaveDescription();" />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="javascript:window.close();" CssClass="button" />
    </div>
    </form>
</body>
</html>
