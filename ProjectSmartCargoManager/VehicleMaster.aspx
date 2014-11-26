<%@ Page Title="VehicleMaster" Language="C#" AutoEventWireup="true" CodeBehind="VehicleMaster.aspx.cs" Inherits="ProjectSmartCargoManager.VehicleMaster" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
        function CheckAllEmp(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=grdVehicleDetails.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[3].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }

        function SelectAllULD(headerchk) {
            var gvcheck = document.getElementById("<%=grdVehicleDetails.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    if (inputs[0].disabled == false)
                        inputs[0].checked = true;
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
</script>

</asp:Content>
 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
<asp:ToolkitScriptManager ID="TSM" runat="server">
</asp:ToolkitScriptManager>
  
<script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }
     </script>

<style type="text/css">
        .black_overlay
        {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 1000px;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: 0.8;
            filter: alpha(opacity=80);
        }
        .white_content
        {
            margin: 0 auto;
            display: none;
            position: absolute;
            top: 30%;
            left: 35%;
            width: 30%;
            height: 45%;
            padding: 16px;
            border: 16px solid #ccdce3;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
        .black_overlaymsg
        {
            display: none;
            position: fixed;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: White;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }
        .white_contentmsg
        {
            display: none;
            position: fixed;
            top: 45%;
            left: 45%;
            width: 5%;
            height: 5%;
            padding: 16px;
            background-color: Transparent;
            z-index: 1002;
        }
    </style>
   
<asp:UpdatePanel runat="server" ID="updtPnl">
<ContentTemplate>
  
<div id="contentarea">
<div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
<h1>Vehicle Master</h1>

<br />
<table>
<tr>
    <td>Station:</td>
    <td>
        <asp:DropDownList ID="ddlStn" runat="server" Width="70px">
        </asp:DropDownList>
    </td>
    <td>Vehicle No:</td>
    <td>
        <asp:TextBox ID="txtVehicleNo" runat="server"></asp:TextBox>
    </td>
    <td>Vehicle Type:</td>
    <td>
        <asp:DropDownList ID="ddlVehicleType" runat="server"  Width="90px">
        </asp:DropDownList>
    </td>
     <td>Driver Code:</td>
    <td>
        <asp:TextBox ID="txtDriverCode" runat="server"></asp:TextBox>
    </td>
     <td>Partner Code:</td>
    <td>
        <asp:DropDownList ID="ddlPartnerCode" runat="server"  Width="80px">
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <td>
        <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" onclick="btnList_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" />
        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" 
            onclick="btnExport_Click" />
    </td>
</tr>
</table>
</div>
<hr />
<div class="ltfloat">

<asp:GridView ID="grdVehicleDetails" runat="server" AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" 
HeaderStyle-CssClass="HeaderStyle" PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
SelectedRowStyle-CssClass="SelectedRowStyle" AutoGenerateColumns="false" AllowPaging="true" PageSize="10">

<Columns>

    <asp:TemplateField>
    <HeaderTemplate>
        <asp:CheckBox ID="chkAll" runat="server" onclick="javascript:SelectAllULD(this);" />
    </HeaderTemplate>
    <ItemTemplate>
        <asp:CheckBox ID="chkRecord" runat="server" />
    </ItemTemplate>
    </asp:TemplateField>
        
    <asp:TemplateField HeaderText="SrNo" Visible="false">
    <ItemTemplate>
        <asp:Label ID="lblSrNo" runat="server" Width="50px" Text='<%#Eval("SrNo")%>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Vehicle#">
    <ItemTemplate>
        <asp:TextBox ID="txtVehicleNo" runat="server" Width="70px" Text='<%#Eval("VehNo")%>'></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Driver Code">
    <ItemTemplate>
        <asp:TextBox ID="txtDriverCode" runat="server" Width="50px" Text='<%#Eval("DriverCode")%>'></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Vehicle Type">
    <ItemTemplate>
        <asp:DropDownList ID="ddlGVehicleType" runat="server"  Width="90px"></asp:DropDownList>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Station">
    <ItemTemplate>
        <asp:DropDownList ID="ddlGStnCode" runat="server"  Width="70px"></asp:DropDownList>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Partner Code">
    <ItemTemplate>
        <asp:DropDownList ID="ddlGPartnerCode" runat="server"  Width="80px"></asp:DropDownList>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Chassis No">
    <ItemTemplate>
        <asp:TextBox ID="txtChassisNo" runat="server" Width="70px" Text='<%#Eval("ChassisNum")%>'></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Manufacturer" Visible="true">
    <ItemTemplate>
        <asp:TextBox ID="txtManufacturer" runat="server" Width="70px" Text='<%#Eval("Manufacturer")%>'></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField>
    
     <asp:TemplateField HeaderText="Vehicle Capacity" Visible="true">
    <ItemTemplate>
        <asp:TextBox ID="txtVehCapacity" runat="server" Width="75px" Text='<%#Eval("VehicleCapacity")%>'></asp:TextBox>
        <%--<asp:RegularExpressionValidator ID="valid_txtVehCapacity" runat="server" ErrorMessage="Only Digits"
             ControlToValidate="txtVehCapacity" ValidationExpression="^\d+\.?\d*$"></asp:RegularExpressionValidator>--%>
    </ItemTemplate>
    </asp:TemplateField>
    
     <asp:TemplateField HeaderText="Latitude" Visible="true">
    <ItemTemplate>
        <asp:TextBox ID="txtLatitude" runat="server" Width="75px" Text='<%#Eval("Latitude")%>'></asp:TextBox>
        <%--<asp:RegularExpressionValidator ID="valid_txtLatitude" runat="server" ErrorMessage="Only Digits"
             ControlToValidate="txtLatitude" ValidationExpression="^\d+\.?\d*$"></asp:RegularExpressionValidator>--%>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Longitude" Visible="true">
    <ItemTemplate>
        <asp:TextBox ID="txtLongitude" runat="server" Width="75px" Text='<%#Eval("Longitude")%>'></asp:TextBox>
        <%--<asp:RegularExpressionValidator ID="valid_txtLongitude" runat="server" ErrorMessage="Only Digits"
             ControlToValidate="txtLongitude" ValidationExpression="^\d+\.?\d*$"></asp:RegularExpressionValidator>--%>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Is Active" Visible="true">
    <ItemTemplate>
        <asp:CheckBox ID="chkIsAct" runat="server" />
    </ItemTemplate>
    </asp:TemplateField>
    
    </Columns>

</asp:GridView>

</div>

<div class="ltfloat">
<table>
<tr>
    <td>
        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" 
            onclick="btnAdd_Click" />
        <asp:Button ID="btnDel" runat="server" Text="Delete" CssClass="button" 
            onclick="btnDel_Click" />
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
            onclick="btnSave_Click" />
    </td>
</tr>
</table>
</div>

  
<div id="msglight" class="white_contentmsg">
        <table>
            <tr>
                <td width="5%" align="center">
                    <br />
                    <img src="Images/loading.gif" />
                    <br />
                    <asp:Label ID="msgshow" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
<div id="msgfade" class="black_overlaymsg">
    
</ContentTemplate>
</asp:UpdatePanel>
  
</asp:Content>