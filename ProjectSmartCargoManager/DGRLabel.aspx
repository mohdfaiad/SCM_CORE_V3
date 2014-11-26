<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DGRLabel.aspx.cs" Inherits="ProjectSmartCargoManager.DGRLabel" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style>
    table
    {
    	border:1px solid #ccc;
    	}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
          <%--<asp:GridView ID="GrdDGRLbl" runat="server" AutoGenerateColumns="false">
          <Columns>
          <asp:TemplateField HeaderText="DGR Labels">
          <ItemTemplate>
            <asp:Image ID="imgDGRLbl" runat="server" Width="200px" Height="100px" ImageUrl='<%#Eval("DGRImg")%>'/>
          </ItemTemplate>
          </asp:TemplateField>
          </Columns>
          </asp:GridView>--%>
          <%--<table runat="server" id="table1" width="90%" cellpadding="6" cellspacing="3">
          <tr>
          <td>
          <table width="90%" cellpadding="6" cellspacing="3">
          <tr>
          <td colspan="2">title</td>
          </tr>
          <tr>
          <td>part1</td><td>part2</td>
          </tr>
          <tr>
          <td>text1</td><td>text2</td>
          </tr>
          </table>
          </td>
          <td valign="top">image</td>
          </tr>
          </table>--%>
       
    </div>
    <asp:Repeater ID="Repeater1" runat="server">
    <ItemTemplate>
        <table runat="server" id="table1" width="32%" cellpadding="2" cellspacing="1" style="border:1px solid #ccc;">
          <tr>
          <td>
          <table width="90%" cellpadding="4" border="1" >
          <tr>
          <td colspan="2" style="text-align:center;font-size:larger;">
              <asp:Label ID="lblHeading" runat="server" Text='<%#Eval("Client")%>' Font-Bold="true"></asp:Label></td>
          </tr>
          <tr>
          <td><strong>AWB No:</strong></td>
          <td><strong>Pcs:</strong></td>
          </tr>
          <tr>
          <td><asp:Label ID="lblAWB" runat="server" Text='<%#Eval("AWB")%>'></asp:Label></td>
          <td><asp:Label ID="lblPcs" runat="server" Text='<%#Eval("Pcs")%>'></asp:Label></td>
          </tr>
          <tr>
          <tr></tr>
          <tr>
          <td><strong>Origin:</strong></td>
          <td><strong>Destination:</strong></td>
          </tr>
          <tr>
          <td><asp:Label ID="lblOrg" runat="server" Text='<%#Eval("Org")%>'></asp:Label></td>
          <td><asp:Label ID="lblDest" runat="server" Text='<%#Eval("Dest")%>'></asp:Label></td>
          </tr>
          <tr>
          <td colspan="2"><strong>Date:</strong><asp:Label ID="lblDt" runat="server" Text='<%#Eval("Time")%>'></asp:Label></td>
          </tr>
          </table>
          </td>
          <td valign="middle">
              <asp:Image ID="imgDGR" runat="server" ImageUrl='<%#Eval("DGRImg")%>' Width="200px" Height="180px"/></td>
          </tr>
          
          </table>
    </ItemTemplate>
    </asp:Repeater>
     <asp:Button ID="btnPrintLbl" runat="server" Text="Print" Enabled="false" onclick="btnPrintLbl_Click"/>
    </form>
</body>
</html>
