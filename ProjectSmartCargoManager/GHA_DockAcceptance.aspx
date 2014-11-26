<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="GHA_DockAcceptance.aspx.cs" Inherits="ProjectSmartCargoManager.DockAcceptance_GHA" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ContentPlaceHolderID="head" ID="Content1" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content2" runat="server">
    <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
<div id="contentarea">
   <h1>Dock Management </h1>

<asp:Label ID="lblStatus" runat="server" Font-Size="Large" Font-Bold="true"></asp:Label>
<div class="botline">                         
<table>
<tr>
<td>Token Date</td>
<td>
    <asp:TextBox ID="txtDate" runat="server" Width="67px" MaxLength="10"></asp:TextBox>
    <asp:CalendarExtender ID="txtDate_Ext" runat="server" TargetControlID="txtDate" Format="dd/MM/yyyy" PopupButtonID="imgDate">
    </asp:CalendarExtender>
    <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
</td>
<td>Flight#</td>
<td>
    <asp:TextBox ID="txtFltNo" runat="server" Width="60px" MaxLength="10"></asp:TextBox>
</td>
<td>Dock#</td>
<td>
    <asp:TextBox ID="txtDockNo" runat="server" Width="60px"></asp:TextBox>
</td>
<td>Token#</td>
<td>
    <asp:TextBox ID="txtTknNo" runat="server" Width="60px"></asp:TextBox>
</td>
<%--<td>Known Shipper</td>
<td>
    <asp:TextBox ID="txtShipper" runat="server" Width="55px"></asp:TextBox>
</td>
<td>IAC Code</td>
<td>
    <asp:TextBox ID="txtIAC" runat="server" Width="55px"></asp:TextBox>
</td>
<td>CCSF#</td>
<td>
    <asp:TextBox ID="txtCSSF" runat="server" Width="55px"></asp:TextBox>
</td>--%>
<td>
    <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
        onclick="btnList_Click" />
</td>
</tr>
</table>
<br />
<div class="ltfloat">
    <asp:GridView ID="grdDockAcceptance" runat="server" 
        AlternatingRowStyle-CssClass="AltRowStyle" AutoGenerateColumns="false" 
     CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
        PagerStyle-CssClass="PagerStyle" Width="100%" PageSize="10"
     RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle"
     AllowPaging="true" onpageindexchanging="grdDockAcceptance_PageIndexChanging" 
        onrowcommand="grdDockAcceptance_RowCommand">
     <Columns>
     
     <asp:TemplateField HeaderText="Token#">
     <ItemTemplate>
         <asp:Label ID="lblTokenNo" runat="server" Text='<%#Eval("TokenNumber")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Token Date">
     <ItemTemplate>
         <asp:Label ID="lblTokenDt" runat="server" Text='<%#Eval("TokenDate")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Driver Name">
     <ItemTemplate>
       <asp:Label ID="lblDriverName" runat="server" Text='<%#Eval("DriverName")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
    <%-- <asp:TemplateField HeaderText="Commodity Code">
     <ItemTemplate>
         <asp:Label ID="lblCommodityCode" runat="server" Text='<%#Eval("CommodityCode")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>--%>
     
     <asp:TemplateField HeaderText="Flight Details">
     <ItemTemplate>
        <asp:Label ID="lblFltDet" runat="server" Text='<%#Eval("FlightDetails")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <%--<asp:TemplateField HeaderText="Flight#">
     <ItemTemplate>
        <asp:Label ID="lblFltNo" runat="server" Text='<%#Eval("FlightNo")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Dep Date and Time">
     <ItemTemplate>
        <asp:Label ID="lblDtTime" runat="server" Text='<%#Eval("FlightDt")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>--%>
     
      <asp:TemplateField HeaderText="Booked Pieces">
     <ItemTemplate>
        <asp:Label ID="lblBookedPcs" runat="server" Text='<%#Eval("PCS")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Booked Weight">
     <ItemTemplate>
         <asp:Label ID="lblBookedWt" runat="server" Text='<%#Eval("WT")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>

     <asp:TemplateField HeaderText="Dock#">
     <ItemTemplate>
         <asp:TextBox ID="txtDockNo" runat="server" Text='<%#Eval("DockNo")%>'></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
          
      <asp:TemplateField>
     <ItemTemplate>
         <asp:Button ID="btnStart" runat="server" Text="Start Acceptance" CssClass="button" CommandName="Start" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
     </ItemTemplate>
     </asp:TemplateField>
     
     </Columns>
    </asp:GridView>
</div>
<br />
</div>
</div>
</asp:Content>
