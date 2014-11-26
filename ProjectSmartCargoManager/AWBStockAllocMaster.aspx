<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AWBStockAllocMaster.aspx.cs"  MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.AWBStockAllocMaster" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">

        .style63
        {
            color: #FF0000;
        }
    </style>
    <script language="javascript" type="text/javascript">

    </script>
 </asp:Content> 
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
  <div id="contentarea">
  
    <h1>   
    AWB Stock Allocation    
           <%-- <img src="Images/txt_billing.png" />--%>
 </h1> 
   <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
   </p>
  <div  class="botline"> 
  <table width=" 80%">
   <tr>
    <td>
     Region
    </td>
    <td>
        <asp:DropDownList ID="ddlRegion" runat="server" Width="80px" 
            AutoPostBack="True" onselectedindexchanged="ddlRegion_SelectedIndexChanged" >
        </asp:DropDownList>
    </td>
    <td>
     City
    </td>
    <td>
        <asp:DropDownList ID="ddlCity" runat="server" Width="80px" >
        </asp:DropDownList>
    </td>
    <td>
     From
    </td>
    <td>
        <asp:TextBox ID="txtAWBFrom" runat="server" Width="90px"></asp:TextBox>
    </td>
    <td>
     To
    </td>
    <td>
     <asp:TextBox ID="txtAWBTo" runat="server" Width="90px"></asp:TextBox>
    </td>
    <td>
        <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" onclick="btnSave_Click" 
           />&nbsp
        <asp:Button ID="btnFilter" runat="server" CssClass="button" Text="Search" onclick="btnFilter_Click" 
           />
    </td>
   </tr>
  </table>  
  </div> 
  <br /><br /><br />
<div class="divback">
<asp:GridView ID="grdAWBStock" runat="server"  Width="80%"
         AutoGenerateColumns="False" >
 <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
            <Columns>
                <asp:BoundField DataField="Region" ItemStyle-HorizontalAlign ="Center" HeaderText="Region" />
                <asp:BoundField DataField="City" ItemStyle-HorizontalAlign ="Center" HeaderText="City" />
                <asp:BoundField DataField="AWBFrom" ItemStyle-HorizontalAlign ="Center" HeaderText="From" />
                <asp:BoundField DataField="AWBTo" ItemStyle-HorizontalAlign ="Center" HeaderText="To" />
                
</Columns> 
                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <FooterStyle CssClass="grdrowfont"></FooterStyle>
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
</asp:GridView>
</div>  
  </asp:Content> 