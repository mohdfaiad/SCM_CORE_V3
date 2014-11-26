<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmUploadFlightData.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmUploadFlightData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" style="margin-top:80px;">
<tr>
<td colspan="5">
    <asp:Label ID="lblError" runat="server"  CssClass="pageerror" ></asp:Label>
    </td>

</tr>
<tr>
<td colspan="5">&nbsp;</td>

</tr>

<tr>
<td></td>
<td colspan="4">
<table width="100%">
<tr>
<td width="30%">
    <asp:FileUpload ID="FileExcelUpload" runat="server"  />
    </td>
    <td width="10%">
        <asp:Button ID="btnUpload" runat="server" Text="Upload" 
            onclick="btnUpload_Click" />
    </td>
   
    <td width="10%">
        <asp:Button ID="btnDownload" runat="server" Text="Download Log File" 
            onclick="btnDownload_Click"  />
    </td>
 <td >&nbsp;</td>
    
    </tr>
    </table>
    </td>

</tr>

</table>
</asp:Content>