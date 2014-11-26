<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmListDemCharges.aspx.cs" MasterPageFile ="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmListDemCharges" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
      <div id="contentarea">
      <h1>List Demurrage Charges</h1>
  <div class="botline">
    <table width = "70%">
    <tr>
    <td colspan="4">
        <asp:Label ID="lblError" runat="server" ForeColor="#FF0066" Text="" Font-Bold="true"></asp:Label>
    </td>
    </tr>
    <tr>
    <td>
    Charge Code
    </td>
    <td>
        <asp:TextBox ID="txtChargeCode" runat="server" MaxLength="20">
        </asp:TextBox>
    </td>
    <td>
    Location Level</td>
    <td>
        <asp:DropDownList ID="ddllevel" runat="server" AutoPostBack="True" 
           onselectedindexchanged="ddllevel_SelectedIndexChanged">
           <asp:ListItem Selected="True" Text="Country" Value="Country"></asp:ListItem>
           <asp:ListItem Text="Station" Value="Station"></asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>
    Location
    </td>
    <td>
        <asp:DropDownList ID="ddlStation" runat="server">
        </asp:DropDownList>
    </td>
    </tr>
    <tr>
    <td colspan="2">
        <asp:Button ID="btnSearch" runat="server" CssClass = "button" Text="List" 
            onclick="btnSearch_Click" />
            &nbsp;
        <asp:Button ID="btnClear" runat="server" CssClass = "button" Text="Clear" 
            onclick="btnClear_Click"/>
    </td>
    </tr>
    </table>
    </div>
    <div style="float:left;width:800px">
        <asp:Label ID="lblEmpty" runat="server" ForeColor="#FF0066" Text="" Font-Bold="true"></asp:Label>
    
        <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" 
              Width="100%"
            AutoGenerateEditButton="True" style="margin-top: 0px"
            HeaderStyle-CssClass="HeaderStyle" 
            RowStyle-CssClass="RowStyle"              
            AlternatingRowStyle-CssClass="AltRowStyle" AllowPaging="True" 
            PagerStyle-CssClass="PagerStyle" onrowediting="gvResult_RowEditing" 
              onpageindexchanging="gvResult_PageIndexChanging">
                     <Columns>
                     <asp:TemplateField HeaderText="Charge Code">    
                       <ItemTemplate>    
                        <asp:Label ID="lblChargeCode" runat="server" Text = '<%# Eval("ChargeCode") %>'></asp:Label>
                       </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Location Level">    
                       <ItemTemplate>    
                        <asp:Label ID="lblLocationLevel" runat="server" Text = '<%# Eval("LocationLevel") %>'></asp:Label>
                       </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Location">    
                       <ItemTemplate>    
                        <asp:Label ID="lblLocation" runat="server" Text = '<%# Eval("Location") %>'></asp:Label>
                       </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Grace Period">    
                       <ItemTemplate>    
                        <asp:Label ID="lblGracePeriod" runat="server" Text = '<%# Eval("GracePrd") %>'></asp:Label>
                       </ItemTemplate>
                     </asp:TemplateField>
                     </Columns>
            <AlternatingRowStyle CssClass="AltRowStyle"></AlternatingRowStyle>
</asp:GridView>
    </div>
</div>
</asp:Content>
