<%@ Page Title="Bank Guarantee Collection Log"  Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="rptBGCollection.aspx.cs" Inherits="ProjectSmartCargoManager.rptBGCollection" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
   <script type="text/javascript">
        
        function GetAgentCode() {
            var level = 'AgentCode';
            var TxtOriginClientObject = '<%=txtAgentCode.ClientID %>';
            window.open('ListDataView.aspx?Parent=AgentCode&level=' + level + '&TargetTXT=' + TxtOriginClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
            return false;
        }
        
    </script>
   
    <style type="text/css">
        .style1
        {
            width: 10%;
        }
        .style2
        {
            width: 15%;
        }
        .style3
        {
            width: 7%;
        }
        .style4
        {
            width: 5%;
        }
        .style5
        {
            width: 8%;
        }
        .style6
        {
            width: 18%;
        }
    </style>
   
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contentarea">
 <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
</div>
       <h1>
           Bank Guarantee Collection Log
       </h1>
      
    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="botline">
        <table width="100%" cellpadding="2" cellspacing="2" >
            <tr>
                <td class="style1">
                    AgentCode
                </td>
                <td class="style2">
                    <asp:TextBox ID="txtAgentCode" runat="server" Width="112px"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="txtAgentCode_Extender" runat="server" ServiceMethod="GetAgentCode"
                                            CompletionInterval="0" EnableCaching="false" CompletionSetCount="10" TargetControlID="txtAgentCode"
                                            MinimumPrefixLength="1">
                    </asp:AutoCompleteExtender>
                    <asp:ImageButton ID="IBOrigin" runat="server" ImageUrl="~/Images/list_bullets.png"
                         ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />
                </td>
                <td class="style3">
                    Valid From* 
                </td>
                <td class="style2">
                    <asp:TextBox ID="txtFromDt" runat="server" Width="102px" 
                        ></asp:TextBox>
                        <asp:ImageButton ID="imgFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                    <asp:CalendarExtender ID="txtFromDt_CalendarExtender" runat="server" PopupButtonID="imgFromDt" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFromDt">
                    </asp:CalendarExtender>
                </td>
                <td class="style4">
                    Valid To* 
                </td>
                <td class="style2">
                    <asp:TextBox ID="txtToDt" runat="server" Width="102px"></asp:TextBox>
                    <asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                    <asp:CalendarExtender ID="txtToDt_CalendarExtender" runat="server" PopupButtonID="imgToDt" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtToDt">
                    </asp:CalendarExtender>
                </td>
                <td class="style5">
                    AWB Number 
                </td>
                <td class="style6">
                <asp:TextBox ID="txtAwbPrefix" runat="server" Width="45"></asp:TextBox>
                                    <asp:TextBox ID="txtAWBNo" runat="server" Width="102px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style1">
                Transaction Type
                </td>
                <td class="style2">
                    <asp:DropDownList ID="ddlTransactionType" runat="server">
                        <asp:ListItem>ALL</asp:ListItem>
                        <asp:ListItem>Credit</asp:ListItem>
                        <asp:ListItem>Debit</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td >
                
                </td>
                <td class="style2">
                </td>
                <td class="style4">
                </td>
                <td class="style2">
                </td>
                <td class="style5">
                </td>
                <td class="style6">
                     
                </td>
                
            </tr>
            <tr>
            <td colspan="8">
            <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
                         Width = "50px" onclick="btnList_Click"/>
                         <asp:Button ID="btnClear" runat="server" CssClass="button" 
                    Text="Clear" Width="50" onclick="btnClear_Click" />
            <asp:Button ID="btnExport" runat="server" Text="Export" Visible="false"
                            CssClass="button" onclick="btnExport_Click" />
                <br />
                <br />
    <%--<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
           ForeColor="Red"></asp:Label>--%>
            </td>
            </tr>
        </table>
    </div>
    <div style=" border: thin solid #000000; float: left;">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1020px" >
        </rsweb:ReportViewer>
        <dd:WebReportViewer ID="WebReportViewer1" runat="server" Height="450px" Width="1024px" />
       
    </div>
</div>
</asp:Content>