<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRepairInspection.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmRepairInspection" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            height: 24px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:ToolkitScriptManager ID="TSM" runat="server">
    </asp:ToolkitScriptManager>
  
    <div id="contentarea">
    
    <h1>
    Repair Order Inspection
    </h1>
<div id="rightcol" style="width:60%;">

<table width="100%">
<tr>
<td style="width:20%;"><strong>ULD Number</strong></td>
<td >
    <asp:Label ID="lblULDNo" runat="server" Text="ABE12345671"></asp:Label></td>
</tr>
<tr>
<td><strong>Manufacturer</strong></td>
<td>
    <asp:Label ID="lblManufacturer" runat="server" Text="NAP NO"></asp:Label></td>
</tr>
<tr>
<td ><strong>Date of Order</strong></td>
<td >
    <asp:Label ID="lblDOO" runat="server" Text="2012-11-06"></asp:Label></td>
</tr>
<tr>
<td ><strong>Date of Arrival</strong></td>
<td >
    <asp:Label ID="lblDOA" runat="server" Text="2012-11-12"></asp:Label></td>
</tr>

<tr>
<td><strong>Purchase Price</strong></td>
<td>
    <asp:Label ID="lblPurchasePrice" runat="server" Text="4000.00 EUR"></asp:Label></td>
</tr>


</table>
    <h2>Applicable Parts</h2>
    
    
    
    
    
    <asp:GridView ID="gvApplicableParts" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
            AlternatingRowStyle-CssClass="AltRowStyle" 
             PagerStyle-CssClass="PagerStyle">
    <Columns>
    <asp:TemplateField >
    <ItemTemplate>
    <asp:CheckBox ID="chkApp" runat="server"></asp:CheckBox>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Part Number">
    <ItemTemplate>
    <asp:Label ID="lblPartNo" runat="server"  Text='<%# Eval("PartNo") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Description">
    <ItemTemplate>
    <asp:Label ID="lblDescription" runat="server"  Text='<%# Eval("Desc") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Price">
    <ItemTemplate>
    <asp:Label ID="lblPrice" runat="server"  Text='<%# Eval("Price") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    </Columns>
    </asp:GridView>
    
    <h2>Assigned Items</h2>
    
    <asp:GridView ID="gvAssiItems" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
            AlternatingRowStyle-CssClass="AltRowStyle" 
             PagerStyle-CssClass="PagerStyle">
    <Columns>
    <asp:TemplateField >
    <ItemTemplate>
    <asp:CheckBox ID="chkAssiItm" runat="server"></asp:CheckBox>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Ref">
    <ItemTemplate>
    <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Description">
    <ItemTemplate>
    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Desc") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Count">
    <ItemTemplate>
    <asp:Label ID="lblCount" runat="server" Text='<%# Eval("Count") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="Price">
    <ItemTemplate>
    <asp:Label ID="lblAssiPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="RR">
    <ItemTemplate>
    <asp:CheckBox ID="chkAssiRR" runat="server"></asp:CheckBox>
    </ItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="W min">
    <ItemTemplate>
    <asp:Label ID="lblAssiWmin" runat="server" Text='<%# Eval("Wmin") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
     <asp:TemplateField HeaderText="L min">
    <ItemTemplate>
    <asp:Label ID="lblAssiLMin" runat="server" Text='<%# Eval("Lmin") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    </Columns>
    </asp:GridView>
</div>
<div id="leftcol" style="width:28%;">
<h2 class="rt">Repair History</h2>
<asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="False" HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
            AlternatingRowStyle-CssClass="AltRowStyle" 
             PagerStyle-CssClass="PagerStyle">
    <Columns>
    <asp:TemplateField HeaderText="Work Order #">
    <ItemTemplate>
    <asp:HyperLink ID="lblWO" runat="server" Text='<%# Eval("WorkOrder") %>' NavigateUrl="#"></asp:HyperLink>
    </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Start Date">
    <ItemTemplate>
    <asp:Label ID="lblSD" runat="server" Text='<%# Eval("Start") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
      <asp:TemplateField HeaderText="Finish Date">
    <ItemTemplate>
    <asp:Label ID="lblFD" runat="server" Text='<%# Eval("Finish") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    </Columns>
    </asp:GridView>
 </div>
</div>
</asp:Content>