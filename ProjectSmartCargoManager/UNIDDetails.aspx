<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UNIDDetails.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.UNIDDetails" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

    </style>

    <script type="text/javascript">
        function GetUNIDCode(obj) {
            var UNIDNo = document.getElementById('<%= txtUnidNo.ClientID%>');
            var splitval = UNIDNo.value.split("-");
            UNIDNo.value = splitval[0];
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
  <div class="msg">
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
  </div>
    <h1>UNID Details</h1> 
       
  <div class="botline"> 
  <table width="60%">
   <tr>
        <td>
        UNID Number *
        </td>
        <td>
         <asp:TextBox ID="txtUnidNo" runat="server" Width="100px" onchange="GetUNIDCode(this)"></asp:TextBox>
         <asp:AutoCompleteExtender ID="ACEUNIDCode" BehaviorID="ACEUNIDCode" runat="server"
         ServiceMethod="GetUNIDCodeWithDesc" CompletionInterval="0" EnableCaching="false"
         CompletionSetCount="10" TargetControlID="txtUnidNo" MinimumPrefixLength="1">
         </asp:AutoCompleteExtender>
            &nbsp;
            <asp:Button ID="btnList" runat="server" 
                Text="List" CssClass="button" Enabled="True" onclick="btnList_Click" />
        </td>
        
        <td>
        RadioActive &nbsp;
            <asp:CheckBox ID="chkRadioactive" runat="server"/>
        </td>
        <td>
        RMC
        </td>
        <td>
            <asp:TextBox ID="txtRmc" runat="server" Width="80px" MaxLength="20"></asp:TextBox>
        </td>
        <td>
         Active 
        </td>
        <td>
        <asp:CheckBox ID="ChkActive" checked="true" runat="server"/>
        </td>
    </tr>
  </table>  
  </div> 

<div class="ltfloat" style="width:100%;">
    <table width="100%">
    <tr>
        <td>
        Proper Shipping Name *
        </td>
        <td>
            <asp:TextBox ID="txtShippingName" runat="server" Width="120px" MaxLength="30"></asp:TextBox>
        </td>
        <td>
        Class/Division *
        </td>
        <td>
            <asp:TextBox ID="txtClassDiv" runat="server" Width="100px"></asp:TextBox>
        </td>
        <td>
        IMP Code
        </td>
        <td>
            <asp:TextBox ID="txtImpCode" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
        </td>
        <td>Technical</td>
        <td>
            <asp:CheckBox ID="chkTechnical" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
        Sub Risk
        </td>
        <td>
            <asp:TextBox ID="txtSubRisk" runat="server" Width="100px" MaxLength="30"></asp:TextBox>
        </td>
        <td>
        Hazard Label(s)
        </td>
        <td>
            <asp:TextBox ID="txtHazardLabels" runat="server" Width="100px" MaxLength="50"></asp:TextBox>
        </td>
        <td>
        Description
        </td>
        <td>
            <asp:TextBox ID="txtDescription" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
        </td>
        <td>
           
        </td>
        </tr>
        <tr>
        <td>
        PG
        </td>
        <td>
            <asp:TextBox ID="txtPg" runat="server" Width="100px" MaxLength="20"></asp:TextBox>
        </td>
        <td>
        SP
        </td>
        <td>
            <asp:TextBox ID="txtSp" runat="server" Width="100px" MaxLength="20"></asp:TextBox>
        </td>
        <td>
        ERG Code
        </td>
        <td>
            <asp:TextBox ID="txtErgCode" runat="server" Width="100px" MaxLength="20"></asp:TextBox>
        </td>
        <td>
          
        </td>
        </tr>
    </table>
    
<table style=" width:80%; margin-top:30px;" cellpadding="2" cellspacing="3">

<tr>
<td valign="top">
<div>
<asp:Label Text="PASSENGER AND CARGO AIRCRAFT" ID="lblPCA" runat="server" 
         Font-Bold="True" Font-Size="Medium"></asp:Label>
<table cellpadding="3" cellspacing="3">
<tr>
<td>Forbidden &nbsp;<asp:CheckBox ID="chkForbiddenPCA" runat="server"/>
</td>
<td>
    
</td>
</tr>
<tr>
<td>
<asp:Label Text="LIMITED QTY" ID="lblLimitedQty" runat="server" 
         Font-Bold="True" Font-Size="Medium"></asp:Label>
</td>
<td>
<asp:Label Text="UNLIMITED QTY" ID="lblUnlimitedQty" runat="server" 
         Font-Bold="True" Font-Size="Medium"></asp:Label>
</td>
</tr>
<tr>
<td>
<table>
<tr>
<td>No Limit &nbsp;
    <asp:CheckBox ID="chkNoLimitLQ" runat="server" />
</td>
<td>
</td>
</tr>
<tr>
<td>PI
</td>
<td>
    <asp:TextBox ID="txtPILQ" runat="server" Width="60px"></asp:TextBox>
</td>
</tr>
<tr>
<td>Max Net Qty per Kg
</td>
<td>
    <asp:TextBox ID="txtMaxQtyLQ" runat="server" Width="100px"></asp:TextBox>
    </td>
</tr>
</table>
</td>
<td>

<table><tr>
<td>
No Limit &nbsp;
    <asp:CheckBox ID="chkNoLimitULQ" runat="server"/>
</td>
<td>
</td>
</tr>
<tr>
<td>PI
</td>
<td>
    <asp:TextBox ID="txtPIULQ" runat="server" Width="60px"></asp:TextBox>
</td>
</tr>
<tr>
<td>Max Net Qty per Kg
</td>
<td>
    <asp:TextBox ID="txtMaxQtyULQ" runat="server" Width="100px"></asp:TextBox>
    </td>
</tr></table>
</td>
</tr>
</table></div>
</td>
<td valign="top">
<div>
<asp:Label Text="CARGO AIRCRAFT ONLY" ID="lblCA" runat="server" 
         Font-Bold="True" Font-Size="Medium"></asp:Label>
<table cellpadding="3" cellspacing="3">
<tr>
<td>Forbidden &nbsp;
    <asp:CheckBox ID="chkForbiddenCA" runat="server" /></td>
    <td></td>
</tr>
<tr>
<td>
    &nbsp;</td>
<td>&nbsp;</td>
</tr>
<tr>
<td>
No Limit &nbsp;
<asp:CheckBox ID="chkNoLimitCA" runat="server" /></td>
<td></td>
</tr>
<tr><td>PI</td><td>
    <asp:TextBox ID="txtPICA" runat="server" Width="60px"></asp:TextBox></td>
</tr>
<tr><td>Max Net Qty per Kg</td><td>
    <asp:TextBox ID="txtMaxQtyCA" runat="server" Width="100px"></asp:TextBox></td>
</tr>
</table></div>
</td>
</tr>
</table>    
</div>  

  <div id="fotbut">
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnOK" runat="server" 
                            Text="Save" CssClass="button" onclick="btnOK_Click" Enabled="True" />
                    </td>&nbsp;
                    <td>
                        <asp:Button ID="btnCancel" runat="server" 
                            Text="Clear" CssClass="button" onclick="btnCancel_Click"/>
                    </td>
                    <td> <asp:Button ID="btnDel" runat="server" 
                            Text="Delete" CssClass="button" onclick="btnDel_Click"/>
                    </td>
                </tr>
            </table>
            </div>

 </div>
</asp:Content> 