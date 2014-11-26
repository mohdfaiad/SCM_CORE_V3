<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpotRateApprovalStatus.aspx.cs" Inherits="ProjectSmartCargoManager.SpotRateSavetmp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Image ID="ImgAprov" runat="server" ImageUrl="~/Images/Client_logo.png" />
        <br />
    <asp:Label ID="lblsprotsaverate" runat="server"></asp:Label>
        <asp:Label ID="lblFailed" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
