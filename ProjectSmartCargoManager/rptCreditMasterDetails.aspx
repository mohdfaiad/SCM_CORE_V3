<%@ Page  Title="Bank Gaurantee Report" Language="C#" AutoEventWireup="true" CodeBehind="rptCreditMasterDetails.aspx.cs"  MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.rptCreditMasterDetails"  %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
   <script type="text/javascript">

       function GetAgentCode() {
           var level = 'AgentCode';
           var TxtOriginClientObject = '<%=txtAgentCode.ClientID %>';
           window.open('ListDataView.aspx?Parent=AgentCode&level=' + level + '&TargetTXT=' + TxtOriginClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
           return false;
       }
        
    </script>
   
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contentarea">
<div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
       <h1>
           Bank Gaurantee Report
       </h1>
    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="botline">
    <%--<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
           ForeColor="Red"></asp:Label>--%>
        <table width="85%" cellpadding="2" cellspacing="2" >
            <tr>
                <td>
                    AgentCode
                </td>
                <td>
                    <asp:TextBox ID="txtAgentCode" runat="server" Width="112px"></asp:TextBox>
                    <asp:ImageButton ID="IBOrigin" runat="server" ImageUrl="~/Images/list_bullets.png"
                         ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />
                </td>
                <td>
                    Valid From :
                </td>
                <td>
                    <asp:TextBox ID="txtFromDt" runat="server" Width="112px"></asp:TextBox>
                    
                    <asp:ImageButton ID="imgFrmDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                    
                    <asp:CalendarExtender ID="txtFromDt_CalendarExtender" runat="server" Format="dd/MM/yyyy" Enabled="True" TargetControlID="txtFromDt" PopupButtonID="imgFrmDt"> 
                    </asp:CalendarExtender>
                </td>
                <td>
                    Valid To :
                </td>
                <td>
                    <asp:TextBox ID="txtToDt" runat="server" Width="112px"></asp:TextBox>
                    
                    <asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                    <asp:CalendarExtender ID="txtToDt_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtToDt" PopupButtonID="imgToDt">
                    </asp:CalendarExtender>
                </td>
                <td>
                    
    
                </td>
            </tr>
            <tr>
            <td colspan="2">
            <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
                        onclick="btnList_Click" />&nbsp;
                <asp:Button ID="BtnClear" runat="server" Text="Clear" CssClass="button" 
                    onclick="BtnClear_Click"/>
                        
            &nbsp;<asp:Button ID="btnExport" runat="server" Text="Export" 
                            CssClass="button" onclick="btnExport_Click" />
            </td>
            </tr>
        </table>
</div>
 <div style=" border: thin solid #000000; float: left;">
    <%--<dd:webreportviewer ID="WebReportViewer1" runat="server" Height="450px" 
         Width="1024px" xmlns:dd="datadynamics.reports.web" />--%>
        
         <rsweb:ReportViewer ID="ReportViewer1" runat="server"  
         Width="1024px">
    </rsweb:ReportViewer>
          
 </div>
</div>
    
</asp:Content>