<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmRepairOrderList.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.FrmRepairOrderList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
 
 <div id="contentarea">
 
 <div class="msg">
    <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
</div>
    
  <h1>List Repair Orders</h1>
    <%--<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>--%>
  
  <div class="botline">
  
  <table width="80%">
    
    <tr>
    <td>Order #</td>
    <td>
        <asp:TextBox ID="TxtOrder" runat="server" Width="100px"></asp:TextBox>
    </td>
    
  <td>Order Date</td> 
   <td>
        <asp:TextBox ID="TxtOrderDate" runat="server" Width="100px"></asp:TextBox>
        <asp:CalendarExtender ID="TxtOrderDate_CalendarExtender" runat="server" 
            Enabled="True" TargetControlID="TxtOrderDate" Format="dd/MM/yyyy">
        </asp:CalendarExtender>
    </td>
    
    <td>Station</td>
    <td>
        <asp:DropDownList ID="ddlStation" runat="server" Width="130px">
        </asp:DropDownList>
    </td>
    
   <td>Expected Delivery Date</td>
    <td><asp:TextBox ID="TxtExpDelDate" runat="server" Width="100px"></asp:TextBox>
    <asp:CalendarExtender ID="TxtExpDelDate_CalendarExtender1" runat="server" 
            Enabled="True" TargetControlID="TxtExpDelDate" Format="dd/MM/yyyy">
        </asp:CalendarExtender>
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
  

  <div style="float:left;width:100%">
  <asp:gridview ID="grdROrderList" runat="server" Width="100%" 
            AutoGenerateColumns="False" HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
            AlternatingRowStyle-CssClass="AltRowStyle" 
             PagerStyle-CssClass="PagerStyle" 
          onpageindexchanging="grdROrderList_PageIndexChanging">
<Columns>
       <%-- <asp:TemplateField >
        <ItemTemplate>
        <asp:CheckBox ID="grdChk" runat="server" ></asp:CheckBox>
        </ItemTemplate>
        </asp:TemplateField>--%>

        <asp:TemplateField HeaderText="Order No">
        <ItemTemplate>
        <asp:Label ID="grdRONo" runat="server" Text='<%# Eval("RONo") %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Order Date">
        <ItemTemplate>
        <asp:Label ID="grdRODate" runat="server" Text='<%# Eval("RoDate") %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Station">
        <ItemTemplate>
        <asp:Label ID="grdStation" runat="server" Text='<%# Eval("Station") %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Expected Delivery Date">
        <ItemTemplate>
        <asp:Label ID="grdExpDeliveryDate" runat="server" Text='<%# Eval("ExpDeliveryDate") %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>

   </Columns>
  </asp:gridview>
  </div>
  
  </div>
</asp:Content>