<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmULDAuditTrail.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.FrmULDAuditTrail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<style>
    .grdrowfont
 {
    font-family:Calibri, Arial, Helvetica, sans-serif;
    font-size:13px;
 }
</style>
    <asp:ToolkitScriptManager ID="TookScriptManager" runat="server"></asp:ToolkitScriptManager>
    </br></br></br>
    <div id="WorkPage">
    <div id="singlecol">
<div class="pagetitle">ULD Audit Trail</div>
<br />
<asp:Label ID="lblStatus" runat="server" ForeColor="Red" 
            style="font-weight: 700; font-size: medium"></asp:Label>
<table cellpadding="3" cellspacing="3">

<tr>

<td>ULD # :</td>
<td>
<asp:TextBox ID="txtULDNo" runat="server"></asp:TextBox>
    </td>
<td>From Date :</td>

<td>
<asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
<asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFromDate">
              </asp:CalendarExtender>
    </td>

<td>To Date :</td>

<td>
<asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                 <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" 
                     Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtToDate">
                 </asp:CalendarExtender>
    </td>

<td>
<asp:Button ID="btnSearch" runat="server" Font-Names="Verdana" CssClass="button"  
        Text="SEARCH"  onclick="btnSearch_Click"/> <%--onclick="btnSearch_Click--%>
    </td>

</tr>

</table>
<br />
<asp:Label ID="lblUCRHistory" runat="server" Text="UCR History" Font-Size="Larger" Visible="false"></asp:Label>
<br />
   <asp:GridView ID="grdULDMovement" runat="server" AutoGenerateColumns="False" 
             ShowFooter="false"    HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
            AlternatingRowStyle-CssClass="trcolor" 
             PagerStyle-CssClass="PagerStyle" CssClass="grdrowfont" >
    <Columns>
   
    <asp:TemplateField HeaderText="ULD Number" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtULDNumber" runat="server" Text='<%# Eval("ULDNo") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="UCR Number" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtUCRNumber" runat="server" Text='<%# Eval("UCRNo") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtLocation" runat="server" Text='<%# Eval("Location") %>' Width="100px" ReadOnly="true" ></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="SubLocation" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtSubLocation" runat="server" Text='<%# Eval("SubLocation") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Released To" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtReleasedTo" runat="server" Text='<%# Eval("ReleasedTo") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Released On" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtReleasedOn" runat="server" Text='<%# Eval("ReleasedOn") %>' WWidth="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Returned On" ItemStyle-HorizontalAlign="Center" >
    <ItemTemplate>
    <asp:TextBox ID="txtReturnedOn" runat="server" Text='<%# Eval("ReturnedOn") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    
    
    
    </Columns>
     <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                    <FooterStyle CssClass="grdrowfont"></FooterStyle>
                    <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
                    <RowStyle CssClass="grdrowfont"></RowStyle>
    </asp:GridView>
    
    <br />
    <br />
    <asp:Label ID="lblFlightHistory" runat="server" Text="Flight Movement History" Visible="false" Font-Size="Larger"></asp:Label>
    <br />
    <asp:GridView ID="grdFlightHistory" runat="server" AutoGenerateColumns="False" 
             ShowFooter="false"    HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
            AlternatingRowStyle-CssClass="trcolor" 
             PagerStyle-CssClass="PagerStyle" CssClass="grdrowfont" >
    <Columns>
   
    <asp:TemplateField HeaderText="ULD Number" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtULDNum" runat="server" Text='<%# Eval("ULDNo") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Flight Number" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtFlightNumber" runat="server" Text='<%# Eval("FlightNo") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Flight Date" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtFlightDate" runat="server" Text='<%# Eval("FltDate") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Origin" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtOrigin" runat="server" Text='<%# Eval("Origin") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Destination" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtDestination" runat="server" Text='<%# Eval("Destination") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    </Columns>
     <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                    <FooterStyle CssClass="grdrowfont"></FooterStyle>
                    <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
                    <RowStyle CssClass="grdrowfont"></RowStyle>
    </asp:GridView>
     <br />
    <br />
    <asp:Label ID="lblMovementHistory" runat="server" Text="Movement History" Visible="false" Font-Size="Larger"></asp:Label>
    <br />
    <asp:GridView ID="grdMovementHistory" runat="server" AutoGenerateColumns="False" 
             ShowFooter="false"    HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
            AlternatingRowStyle-CssClass="trcolor" 
             PagerStyle-CssClass="PagerStyle" CssClass="grdrowfont" >
    <Columns>
   
    <asp:TemplateField HeaderText="ULD Number" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtULDNumb" runat="server" Text='<%# Eval("ULDNo") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtLoc" runat="server" Text='<%# Eval("Location") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Located On" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtLocatedOn" runat="server" Text='<%# Eval("LocatedOn") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    <asp:TemplateField HeaderText="Location Source" ItemStyle-HorizontalAlign="Center">
    <ItemTemplate>
    <asp:TextBox ID="txtLocSource" runat="server" Text='<%# Eval("LocationSource") %>' Width="100px" ReadOnly="true"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField >
    </Columns>
     <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                    <FooterStyle CssClass="grdrowfont"></FooterStyle>
                    <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
                    <RowStyle CssClass="grdrowfont"></RowStyle>
    </asp:GridView>

    
    
    
    
</div>

</div>
</asp:Content>

