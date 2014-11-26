<%@ Page Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="frmUploadData.aspx.cs" Inherits="ProjectSmartCargoManager.frmUploadData" %>
  <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    
   
    <style type="text/css">
        .style1
        {
            height: 39px;
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
    <h1>Upload Data</h1></td>

</tr>
<tr>
<td colspan="3">&nbsp;</td>

</tr>

<tr>
<td class="style1">&nbsp;</td>
<td class="style1" colspan="2">
<table width="100%">
    <tr>
<td width="10%">AWB&#39;s</td>
<td width="25%" class="style6">
    <asp:FileUpload ID="AWBFileUpload" runat="server" />
    </td>
    <td width="10%" class="style6">
        <asp:Button ID="btnAWBUpload" runat="server" Text="Upload" onclick="btnAWBUpload_Click" />
    </td>
   
    <td width="10%" class="style6">
    <asp:HyperLink ID="hlnkAWBTemplate" Text="Download Template" NavigateUrl="~/Templates/AWBDataTemplate.xlsx" runat="server" />
    </td>
       
 <td class="style6" >Uploaded<asp:CheckBox ID="chkAWB" runat="server" /></td>
    
    </tr>
  
    </table>
    </td>

</tr>

</table>
      </div> 
      
</asp:Content>
