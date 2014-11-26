<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRepairOrder.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmRepairOrder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    
    
    <div id="contentarea">

    
     <h1>Repair Order</h1>
     <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>

     <div class="botline">

     <table width="100%">
    
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
    </table>  <%--<asp:Button ID="eer" CssClass="button" Text="List" runat="server" />--%>
    
    </div>
    <div id="rightcol">
    
  <!--  <h2>ADD ULD repair station order</h2>
    <div class="box"> <table width="100%">
    <tr>
    <td>ULD Type</td>
    <td>ULD Part #</td>
    <td>Repair Station</td>
    <td>Delivery Date</td>
    </tr>
        <tr>
    <td>
        <asp:DropDownList ID="DropDownList3" runat="server">
        </asp:DropDownList>
            </td>
            <td>
            <asp:DropDownList ID="DropDownList4" runat="server">
        </asp:DropDownList>
            </td>
    <td>
        <asp:DropDownList ID="DropDownList5" runat="server">
        </asp:DropDownList>
            </td>
    <td>
        <asp:TextBox ID="TextBox3" runat="server" CssClass="inputbgmed"></asp:TextBox>
            </td>
    </tr>
    </table></div>-->
   
   
   
    <div>
    <strong>Repair Order Details</strong> 
    
 <asp:gridview ID="grdRepairOrder" runat="server" Width="100%" 
            AutoGenerateColumns="False" HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
            AlternatingRowStyle-CssClass="AltRowStyle" 
             PagerStyle-CssClass="PagerStyle">
<Columns>
       <%-- <asp:TemplateField >
        <ItemTemplate>
        <asp:CheckBox ID="grdChk" runat="server" ></asp:CheckBox>
        </ItemTemplate>
        </asp:TemplateField>--%>

        <asp:TemplateField HeaderText="ULD No.">
        <ItemTemplate>
        <asp:Label ID="grdULDNo" runat="server" Text='<%# Eval("ULD") %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="ULD Part #">
        <ItemTemplate>
        <asp:Label ID="grdULDPartNo" runat="server" Text='<%# Eval("ULDPart") %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="ULDPartSerialNo">
        <ItemTemplate>
        <asp:Label ID="grdULDPartSerialNo" runat="server" Text='<%# Eval("SerialNo") %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Nature of Damage">
        <ItemTemplate>
        <asp:Label ID="grdNatureofDamage" runat="server" Text='<%# Eval("NatureOfDmg") %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>

   </Columns>
</asp:gridview>



<asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="button" 
            onclick="BtnSave_Click" />&nbsp;
<asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="button" 
            onclick="BtnCancel_Click" />&nbsp;
<asp:Button ID="btnDispatch" runat="server" CssClass="button"  Text="Dispatch" />
  
</div>

    </div>
    <div id="leftcol">
    <h2>ADD ULD(s)</h2>
    
     <%--<table width="100%"><tr>
     <td>
    <a href="#" class="uldranges"><span style="height:50px; margin-left:25px;">ULD</span></a></td>
    <td style="width:50%">
    <a href="#" class="uldrangem">
    <span style="height:50px; margin-left:25px;">Part</span></a></td></tr>
    </table>
    --%>
    
    <table width="60%">
      <tr>
            <td colspan="2">ULD #</td>
    </tr>
    <tr>
        <td colspan="2"> <asp:TextBox ID="txtULDNo" runat="server" CssClass="inputbgmed" > </asp:TextBox> </td>
    </tr>
    
    <tr>
        <td> ULD Part #</td>
    </tr>
    
    <tr>
    <td> <asp:TextBox ID="TxtPartNo" runat="server" CssClass="inputbgmed" ></asp:TextBox> </td>
    </tr>
    
    <tr>
    <td colspan="2"> ULD Part Serial #   </td> 
    </tr>
    
    <tr>
    <td colspan="2"><asp:DropDownList ID="ddlPartSerialNo" runat="server" ></asp:DropDownList>  </td>
    </tr>
    
    <tr>
    <td colspan="2"> Nature of Damage  </td>
    </tr>
    
    <tr>
    <td colspan="2"><asp:TextBox ID="TxtNatureofDamage" runat="server" CssClass="inputbgmed"  ></asp:TextBox> </td>
    </tr>
    
    <tr>
    <td colspan="2"> <asp:Button ID="btnAdd" runat="server" Text="Add" 
            CssClass="button" onclick="btnAdd_Click" /></td>
    </tr>
    
    </table>
    
    </div>
    
</div>
    
    <br />
    <br />
</asp:Content>