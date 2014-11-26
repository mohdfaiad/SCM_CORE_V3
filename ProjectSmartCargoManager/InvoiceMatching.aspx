<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="InvoiceMatching.aspx.cs" Inherits="ProjectSmartCargoManager.InvoiceMatching" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />
<br />
         <br />   <br />
<h1>
            <img alt="" src="Images/aircarftedit.png"  style="vertical-align:5"/> </h1>
            <p>
            
        </p>


<div>

    <asp:Label ID="lblError" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Blue"></asp:Label>
    <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Blue"></asp:Label>
<br />
    <asp:DropDownList ID="ddlSheets" runat="server" Height="16px">
    </asp:DropDownList>
    
    <table class="style1">
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Upload File"></asp:Label>
            </td>
            <td>
    <asp:FileUpload ID="FileExcelUpload" runat="server"  />
            </td>
            <td>
    <asp:Button ID="btnUpload" runat="server" Text="Upload"  CssClass="button" 
        onclick="btnUpload_Click"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Invoice Number"></asp:Label>
            </td>
            <td>
    <asp:TextBox ID="txtInvoiceNumber" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnCompare" runat="server" Text="Compare" CssClass="button" 
                    onclick="btnCompare_Click" />
            </td>
        </tr>
    </table>
</div>
<div>
    <asp:GridView ID="dGrd" runat="server" Visible="false">
    </asp:GridView>
    <asp:GridView ID="GrdXl" runat="server" Visible="false">
    </asp:GridView>
</div>

</asp:Content>
