<%@ Page Title="ULD Details" Language="C#" AutoEventWireup="true" CodeBehind="FrmULDMaster.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.FrmULDMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript">
        function Validate() {
            var isValid = false;
            isValid = Page_ClientValidate('Search');
            if (isValid) {
                isValid = Page_ClientValidate();
            }
            
            return isValid;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div id="contentarea">
    
    <h1>ULD Details</h1>
<table width="100%">
<tr>
<td colspan="2">
    <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
</td>
    
</tr>

<tr>
<td>ULD #</td>
<td>
<asp:DropDownList ID="ddlULDType" runat="server" AutoPostBack="True" 
        onselectedindexchanged="ddlULDType_SelectedIndexChanged"></asp:DropDownList>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
        ControlToValidate="ddlULDType" ErrorMessage="*" InitialValue="0" 
        SetFocusOnError="True" ValidationGroup="Search"></asp:RequiredFieldValidator>
    <asp:TextBox ID="txtULDNo" runat="server" CssClass="inputbgmed"></asp:TextBox>
    &nbsp;
    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"  ValidationGroup="Save"
        ControlToValidate="txtULDNo" ErrorMessage="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
    <%--<asp:Label ID="lblOwner" runat="server"></asp:Label>--%>
    
&nbsp;&nbsp;<asp:DropDownList ID="drpOwner" runat="server" ValidationGroup="Save"></asp:DropDownList>
    &nbsp;<asp:RequiredFieldValidator  runat="server" ID="RequiredFieldValidator10" ControlToValidate="drpOwner" 
        ErrorMessage="*" SetFocusOnError="True" 
        InitialValue="0" ValidationGroup="Save"></asp:RequiredFieldValidator>
        
    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" text="<%$ Resources:LabelNames, LBL_BTN_LIST %>" CssClass="button" ValidationGroup="Search" /> 
    <%--<asp:ImageButton ID="btnSearch" runat="server" Height="20px" 
        ImageUrl="~/images/search1.png" Width="20px" onclick="btnSearch_Click" ValidationGroup="Search" />--%>
        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkIsThirdParty" runat="server" Text="IsThirdParty" />
    
    </td>
    
</tr>


<tr>
<td>ULD Manufacturer</td>
<td>
 <asp:TextBox ID="txtULDManu" runat="server" CssClass="inputbgmed"></asp:TextBox>
   
   </td>
</tr>

<tr>
<td>ULD Purchase Cost</td>
<td>
    <asp:TextBox ID="txtULDPurCost" runat="server" CssClass="inputbgmed"></asp:TextBox>
     <asp:FilteredTextBoxExtender ID="txtULDPurCost_FilteredTextBoxExtender1" 
        runat="server" Enabled="True" TargetControlID="txtULDPurCost" FilterMode="ValidChars" FilterType="Custom" ValidChars=".0123456789" >
    </asp:FilteredTextBoxExtender>
    <asp:DropDownList ID="ddluldcurr" runat="server"> </asp:DropDownList></td>
        <%--<asp:ListItem Selected="True">USD</asp:ListItem>--%>
            
</tr>

<tr>
<td>ULD Location</td>
<td>
    <asp:DropDownList ID="ddlWH" runat="server" AutoPostBack="True" 
        onselectedindexchanged="ddlWH_SelectedIndexChanged">
        <asp:ListItem Selected="True">Station</asp:ListItem>
        <asp:ListItem>SubLocation</asp:ListItem>
    </asp:DropDownList>
    &nbsp;<asp:DropDownList ID="ddlSubWH" runat="server">
    </asp:DropDownList>
    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
        ControlToValidate="ddlSubWH" ErrorMessage="*" InitialValue="0" 
        SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
    </td>
</tr>

<tr>
<td>Updated On</td>
<td>
    <asp:TextBox ID="txtULDLocDate" runat="server" CssClass="inputbgmed"></asp:TextBox>
    <asp:CalendarExtender ID="txtULDLocDate_CalendarExtender" runat="server" 
        Enabled="True" TargetControlID="txtULDLocDate">
    </asp:CalendarExtender>
    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
        ControlToValidate="txtULDLocDate" ErrorMessage="*" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
    </td>
</tr>
<tr>
<td>ULD Location Source</td>
<td>
<asp:DropDownList ID="ddlDimSource" runat="server">
    <asp:ListItem Selected="True">Manual</asp:ListItem>
    <asp:ListItem>UCR</asp:ListItem>
    <asp:ListItem>LDM</asp:ListItem>
    <asp:ListItem>Stock</asp:ListItem>
    </asp:DropDownList>
    </td>
</tr>
<tr>
<td>Dimension(WxLxH)</td>
<td>
    <asp:TextBox ID="txtULDW" runat="server" CssClass="inputbgmed"></asp:TextBox>
      <asp:FilteredTextBoxExtender ID="txtULDW_FilteredTextBoxExtender1" 
        runat="server" Enabled="True" TargetControlID="txtULDW" FilterMode="ValidChars" FilterType="Custom" ValidChars=".0123456789" >
    </asp:FilteredTextBoxExtender>
    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
        ControlToValidate="txtULDW" ErrorMessage="*" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
    <asp:TextBox ID="txtULDL" runat="server" CssClass="inputbgmed"></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="txtULDL_FilteredTextBoxExtender1" 
        runat="server" Enabled="True" TargetControlID="txtULDL" FilterMode="ValidChars" FilterType="Custom" ValidChars=".0123456789" >
    </asp:FilteredTextBoxExtender>
    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
        ControlToValidate="txtULDL" ErrorMessage="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
    --%>
    <asp:TextBox ID="txtULDH" runat="server" CssClass="inputbgmed"></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="txtULDH_FilteredTextBoxExtender1" 
        runat="server" Enabled="True" TargetControlID="txtULDH" FilterMode="ValidChars" FilterType="Custom" ValidChars=".0123456789" >
    </asp:FilteredTextBoxExtender>
    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
        ControlToValidate="txtULDH" ErrorMessage="*" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
   <asp:DropDownList ID="ddlULDWei" runat="server">
       <asp:ListItem Selected="True">Cubic cm</asp:ListItem>
    </asp:DropDownList>
   </td>
</tr>
<%--<tr>
<td>Volume</td>
<td colspan="2">
    <asp:TextBox ID="txtVol" runat="server" CssClass="inputbgmed"></asp:TextBox>
    <asp:DropDownList ID="ddlVol" runat="server"></asp:DropDownList></td>
</tr>
--%>
<tr>
<td>Tare Weight</td>
<td>
    <asp:TextBox ID="txtULDTare" runat="server" CssClass="inputbgmed"></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="txtULDTare_FilteredTextBoxExtender" 
        runat="server" Enabled="True" TargetControlID="txtULDTare" FilterMode="ValidChars" FilterType="Custom" ValidChars=".0123456789" >
    </asp:FilteredTextBoxExtender>
    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ControlToValidate="txtULDTare" ErrorMessage="*" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
&nbsp;<asp:DropDownList ID="ddlTareWei" runat="server">
       <asp:ListItem Selected="True">Kg</asp:ListItem>
    </asp:DropDownList></td>
</tr>

<tr>
<td>Dolly Weight</td>
<td>
    <asp:TextBox ID="txtDollyWt" runat="server" CssClass="inputbgmed"></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
        runat="server" Enabled="True" TargetControlID="txtDollyWt" FilterMode="ValidChars" FilterType="Custom" ValidChars=".0123456789" >
    </asp:FilteredTextBoxExtender>
    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ControlToValidate="txtULDTare" ErrorMessage="*" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>       
</td>
</tr>

<tr>
<td>Max Gross Weight</td>
<td>
    <asp:TextBox ID="txtMaxGrossWt" runat="server" CssClass="inputbgmed"></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
        runat="server" Enabled="True" TargetControlID="txtMaxGrossWt" FilterMode="ValidChars" FilterType="Custom" ValidChars=".0123456789" >
    </asp:FilteredTextBoxExtender>
    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
        ControlToValidate="txtULDTare" ErrorMessage="*" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>       
</td>
</tr>

<tr>
<td>ULD Status</td>
<td>
<asp:DropDownList ID="ddlULDStatus" runat="server"></asp:DropDownList>
   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
        ControlToValidate="ddlULDStatus" ErrorMessage="*" InitialValue="0" 
        SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
    </td>
</tr>



<tr>
<td>ULD Use Status</td>
<td>
<asp:DropDownList ID="ddlULDUseStatus" runat="server"></asp:DropDownList>
</td>
</tr>

<tr>
<td>IsReceived</td>
<td>
    <asp:CheckBox ID="chkIsReceived" runat="server" />
</td>
</tr>


<tr>
<td>ULD Economical Repair Point</td>
<td>
    <asp:TextBox ID="txtULDEcoRprPoint" runat="server" CssClass="inputbgmed"></asp:TextBox></td>
</tr>

<tr>
<td>Certification</td>
<td>
    <asp:TextBox ID="txtCertification" runat="server" CssClass="inputbgmed"></asp:TextBox></td>
</tr>

<tr>
<td>Remarks</td>
<td>
    <asp:TextBox ID="txtComment" runat="server" CssClass="inputbgmed"></asp:TextBox></td>
</tr>
<tr>
<td colspan="2">
<asp:Button ID="btnCreate" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SAVE %>" CssClass="button" OnClientClick="return Validate()" ValidationGroup="Save"
        onclick="btnCreate_Click" />
<asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_DELETE %>" CssClass="button" 
        Visible="False" />
<asp:Button ID="btnClear" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>" CssClass="button" 
        onclick="btnClear_Click"  ValidationGroup="Clear"/>
 
</td>

</tr>

</table>
</div>
</asp:Content>