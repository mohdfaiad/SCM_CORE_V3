<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptProofOfReceipt.aspx.cs" Inherits="ProjectSmartCargoManager.RptProofOfReceipt" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>
     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
     

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:ToolkitScriptManager ID="ScriptManager1" runat="server"></asp:ToolkitScriptManager>


<%--<asp:UpdatePanel runat="server" ID="updtPnl">
<ContentTemplate>--%>

<div id="contentarea">
 
 <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>

<h1>Proof of Receipt Report </h1>


<div class="botline">
  <table width="85%" cellpadding="2" cellspacing="2">
     <tr>
         <td>
             <asp:Label ID="LblAgent" runat="server" Text="Agent Name"></asp:Label>
         </td>
         <td>
             <asp:TextBox ID="TxtAgentNm" runat="server"></asp:TextBox>
         </td>
         
         <td>
             <asp:Label ID="LblStation" runat="server" Text="Station"></asp:Label>
         </td>
         <td>
             <asp:DropDownList ID="ddlStation" runat="server"></asp:DropDownList>
         </td>
         
         <td>
             <asp:Label ID="LblPaymode" runat="server" Text="PayMode"></asp:Label>
         </td>
         <td>
             <asp:DropDownList ID="ddlPaymode" runat="server">
             <asp:ListItem>ALL(Without FOC)</asp:ListItem>
             <asp:ListItem>PP</asp:ListItem>
             <asp:ListItem>CC</asp:ListItem>
             <asp:ListItem>FOC</asp:ListItem>
             </asp:DropDownList>
         </td>
     </tr>
     
     <tr>
       <td>
           <asp:Label ID="LblFrmDt" runat="server" Text="From Date"></asp:Label></td>
       <td> 
           <asp:TextBox ID="txtFrmDate" runat="server"></asp:TextBox>
           <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFrmDate" PopupButtonID="imgFromDate">
                    </asp:CalendarExtender>
            <asp:ImageButton ID="imgFromDate" runat="server" ImageAlign="AbsMiddle" 
                     ImageUrl="~/Images/calendar_2.png" />
           </td>
       
       <td>
           <asp:Label ID="LblToDt" runat="server" Text="To Date"></asp:Label></td>
       <td>
           <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
           <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"  
                        Enabled="True" TargetControlID="txtTodate" PopupButtonID="imgToDate">
                    </asp:CalendarExtender>
           <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" 
                     ImageUrl="~/Images/calendar_2.png" />
           </td>
       
     </tr>
     
     <tr>
         <td>
            <asp:Button ID="BtnList" runat="server" CssClass="button" Text="List" 
                 onclick="BtnList_Click" /> 
             <asp:Button ID="BtnExport" runat="server" CssClass="button" Text="Export" 
                 onclick="BtnExport_Click" />
            <asp:Button ID="BtnClear" runat="server" CssClass="button" Text="Clear" 
                 onclick="BtnClear_Click" />
         </td>
     </tr> 
     
  </table>
</div>

<div style=" border: thin solid #000000; float:left;">
           <rsweb:ReportViewer ID="RPTViewer" runat="server" Width="1022px">
           </rsweb:ReportViewer>
</div>

</div>

<%--</ContentTemplate>

<Triggers>
    <asp:PostBackTrigger ControlID="BtnExport" />
    
</Triggers>
</asp:UpdatePanel>--%>

</asp:Content>