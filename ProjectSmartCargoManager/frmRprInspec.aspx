<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRprInspec.aspx.cs" MasterPageFile ="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmRprInspec" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

<div id="contentarea">
<h1>ULD Repair Status</h1>
<table>
<tr>
<td>
    <asp:Label ID="Label1" runat="server" Text="Label">In Repair From</asp:Label>
</td>
<td>
    <asp:Label ID="Label2" runat="server" Text="Label">In Repair Till</asp:Label>
</td>
<td>
    <asp:Label ID="Label3" runat="server" Text="Label">ULD Type</asp:Label>
</td>
<td>
    <asp:Label ID="Label4" runat="server" Text="Label">ULD No</asp:Label>
</td>
<td>
    <asp:Label ID="Label5" runat="server" Text="Label">ULD Owner</asp:Label>
</td>
<td>
    <asp:Label ID="Label6" runat="server" Text="Label">Repair Station</asp:Label>
</td>
<td>
    <asp:Label ID="Label7" runat="server" Text="Label">Repair Status</asp:Label>
</td>
<td>
    <asp:Label ID="Label8" runat="server" Text=""></asp:Label>
</td>
</tr>
<tr>
<td>
    <asp:TextBox ID="txtFrmDate" runat="server" CssClass="inputbgmed" Width="100"></asp:TextBox>
    <asp:CalendarExtender ID="txtFrmDate_CalendarExtender" runat="server" 
        Enabled="True" TargetControlID="txtFrmDate">
    </asp:CalendarExtender>
</td>
<td>
    <asp:TextBox ID="txtTillDate" runat="server" CssClass="inputbgmed"></asp:TextBox>
    <asp:CalendarExtender ID="txtTillDate_CalendarExtender" runat="server" 
        Enabled="True" TargetControlID="txtTillDate">
    </asp:CalendarExtender>
</td>

<td>
    <asp:DropDownList ID="drpUldType" runat="server">
    </asp:DropDownList>
</td>
<td>
    <asp:TextBox ID="txtUldNo" runat="server" CssClass="inputbgmed" Width="100px"></asp:TextBox>
</td>
<td>
    <asp:DropDownList ID="drpUldOwner" runat="server">
    </asp:DropDownList>
</td>
<td>
    <asp:DropDownList ID="drpRepairStn" runat="server">
    </asp:DropDownList>
</td>
<td>
    <asp:DropDownList ID="drpRepairSts" runat="server">
    </asp:DropDownList>
</td>
<td align = "right">
   
        <asp:Button ID="btnSearch" runat="server" CssClass = "button"
        Text="List" />
        &nbsp;
        <asp:Button ID="btnClear" runat="server" CssClass = "button"
        Text="Clear" />
</td>
</tr>
</table>

<div>
<h2>Main Repair Station Details</h2>
<asp:GridView ID="gvRprInspec" runat="server" AutoGenerateColumns="False" 
              Width="100%" ShowFooter="false" CellSpacing=3 CellPadding=3 AutoGenerateEditButton="True" 
                HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle"              AlternatingRowStyle-CssClass="AltRowStyle" AllowPaging="True" PagerStyle-CssClass="PagerStyle"   
                            >
                                 <Columns>
                                 <asp:TemplateField HeaderText="Repair Station">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblRprStn" runat="server" Text = '<%# Eval("RprStn") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="ULD No.">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblULD" runat="server" Text = '<%# Eval("UldNo") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Recieved Date">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblRecDt" runat="server" Text = '<%# Eval("RcvDt") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Repair Order#">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblRprOrdno" runat="server" Text = '<%# Eval("RprOrdNo") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Repair Status">    
                                   <ItemTemplate>    
                                   <asp:DropDownList ID="drpStsSnc" runat="server" ></asp:DropDownList>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Status Since">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblStsSnc" runat="server" Text = '<%# Eval("StsSnc") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="OutDt">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblOutDt" runat="server" Text = '<%# Eval("OutDt") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Go to">    
                                   <ItemTemplate>    
                                   
                                   <asp:Button ID="btnInspect" runat="server" CssClass = "button"
                                                    Text="Inspect" />
                                   
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Action">    
                                   <ItemTemplate>    
                                    
                                   <asp:Button ID="btnUpdateStatus" runat="server" CssClass = "button"  Text="Update" />
                                    
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 </Columns>
</asp:GridView>
</div></div>
</asp:Content>

