<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmListPOs.aspx.cs" MasterPageFile ="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmListPOs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
      <div id="contentarea">
      <h1>List Purchase Orders</h1>
  <div class="botline">
    <table width = "70%">
    <tr>
    <td colspan="4">
        <asp:Label ID="lblError" runat="server" ForeColor="#FF0066" Text="Label" Font-Size="Large" Font-Bold="true"></asp:Label>

    </td>
    </tr>
    <tr>
    <td>
    From Date
        *</td>
    <td>
        <asp:TextBox ID="txtFrmDt" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
        <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" PopupButtonID="imgFromDt"
                Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFrmDt">
        </asp:CalendarExtender>
    </td>
    <td>
    To Date
        *
    </td>
    <td>
        <asp:TextBox ID="txtToDt" runat="server"></asp:TextBox>
        <asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
        <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" PopupButtonID="imgToDt"
                Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtToDt">
        </asp:CalendarExtender>
    </td>
    <td>
    Worldwide Region
    </td>
    <td>
        <asp:DropDownList ID="drpWWR" runat="server">
        </asp:DropDownList>
    </td>
    </tr>
    <tr>
    <td colspan="2">
        <asp:Button ID="btnSearch" runat="server" CssClass = "button" Text="List" 
            onclick="btnSearch_Click" />
            &nbsp;
        <asp:Button ID="btnClear" runat="server" CssClass = "button" Text="Clear" onclick="btnClear_Click" 
            />
    </td>
    </tr>
    </table>
    </div>
    <div>
        <asp:Label ID="lblEmpty" runat="server" ForeColor="#FF0066" Text="Label" Font-Bold="true"></asp:Label>
    </div>
    <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" 
              Width="100%"
            AutoGenerateEditButton="True" style="margin-top: 0px"
            HeaderStyle-CssClass="HeaderStyle" 
            RowStyle-CssClass="RowStyle"              
            AlternatingRowStyle-CssClass="AltRowStyle" AllowPaging="True" 
        PagerStyle-CssClass="PagerStyle" onrowediting="gvResult_RowEditing"
                            >
                                 <Columns>
                                 <asp:TemplateField HeaderText="PO#">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblPONo" runat="server" Text = '<%# Eval("PONumber") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="PO Date">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblPODt" runat="server" Text = '<%# Eval("PODate") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Manufacturer">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblManufacturer" runat="server" Text = '<%# Eval("Name") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Order Region">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblOrderReg" runat="server" Text = '<%# Eval("WWRegionCode") %>'></asp:Label>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 </Columns>

<AlternatingRowStyle CssClass="AltRowStyle"></AlternatingRowStyle>
</asp:GridView>
</div>
</asp:Content>
