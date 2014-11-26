<%@ Page Title="Upload PAX Load" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="UploadPaxLoad.aspx.cs" Inherits="ProjectSmartCargoManager.UploadPaxLoad" %>
  <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    
   
    <style type="text/css">
        .style1
        {
            height: 39px;
        }
        .style4
        {
            height: 16px;
        }
        .style6
        {
            height: 30px;
        }
    </style>
   
    
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       

     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
      <div id="contentarea">
      
      <table width="100%" >
<tr>
<td colspan="3">
    <asp:Label ID="lblError" runat="server"  CssClass="pageerror" ></asp:Label>
    </td>

</tr>
<tr>
<td colspan="3">&nbsp;</td>

</tr>

<tr>
<td class="style1">&nbsp;</td>
<td class="style1" colspan="2">
<table width="100%">
<tr>
<td width="30%" class="style6">
    <asp:FileUpload ID="FileExcelUpload" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="btnUpload" runat="server" Text="Upload" 
            onclick="btnUpload_Click" />
    </td>
   
    <td width="10%" class="style6">
    </td>
   
    <td width="10%" class="style6">
        <asp:Button ID="btnDownload" runat="server" Text="Download Log File" 
            onclick="btnDownload_Click"  />
    </td>
 <td class="style6" ></td>
    
    </tr>
    </table>
    </td>

</tr>

<tr>
<td class="style4"></td>
    <td class="style4">
       
        From Date
    <asp:TextBox ID="txtFlightFromdate" runat="server" Width="100px"
            ToolTip="Please enter date format: dd/MM/yyyy" style="margin-top: 2px"></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightFromdate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightFromdate">
              </asp:CalendarExtender>
    
    &nbsp;To Date<asp:TextBox ID="txtToDate" runat="server" ToolTip="Please enter date format: dd/MM/yyyy" Width="100px"></asp:TextBox>
       <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtToDate">
              </asp:CalendarExtender>
        <asp:Button ID="btnList" runat="server" Text="List" onclick="btnList_Click" 
            />
       
    </td>

<td class="style4">  &nbsp;</td>

<td class="style4">
    </td>

</tr>

<tr>
<td colspan="3">
    <asp:GridView ID="grdPaxLoad" runat="server"  Width="100%"
         >
          <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
 <EditRowStyle CssClass="grdrowfont" ></EditRowStyle>
                <FooterStyle CssClass="grdrowfont"></FooterStyle>
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
    </asp:GridView>
    </td>

</tr>

</table>
      </div> 
      
</asp:Content>
