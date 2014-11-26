<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptEndOfShift.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.rptEndOfShift" %>
<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>
     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
     
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
     
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <style type="text/css">
        .style1
        {
            width: 253px;
        }
        
       .hrtime div{ display: inline-table; padding-right:5px !important;}
    </style>
    <script type="text/javascript">
        function DisableButton() {
            document.getElementById("<%=btnList.ClientID %>").disabled = true;
            document.getElementById("<%=btnClear.ClientID %>").disabled = true;
            document.getElementById("<%=btnExport.ClientID %>").disabled = true;
        }
        window.onbeforeunload = DisableButton;
    </script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contentarea">
       <%-- <h1>
            <img alt="Revenue" src="Images/stationwisereport.png" /> 
            
            </h1>--%>
            <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
       <h1>
           End Of Shift Report
       </h1>

     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="botline">
    
        <table width="90%" cellpadding="2" cellspacing="2" >
            <tr>
                <td>
                    From Date</td>
                <td>
                    <asp:TextBox ID="txtFromdate" runat="server" Width="80px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFromdate" PopupButtonID="imgFromDt">
                    </asp:CalendarExtender>
                   
                    <asp:ImageButton ID="imgFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                
                 </td>
                 <td>
                 <div class="hrtime">
                     <asp:TextBox ID="txtFromTimeHr" runat="server" DataTextField="" Width="50px"></asp:TextBox>
                
                      <asp:TextBox ID="txtFromTimeMin" runat="server" DataTextField=""></asp:TextBox>(HR:MI)
                                         
                    <asp:NumericUpDownExtender ID="txtFromTimeHr_NumericUpDownExtender" 
                                        runat="server" Maximum="23" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtFromTimeHr" Width="40"  />
                                        
                    <asp:NumericUpDownExtender ID="txtFromTimeMin_NumericUpDownExtender1" 
                                        runat="server" Maximum="59" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtFromTimeMin" Width="40" /></div>
                </td>
                <td>
                    To Date</td>
                <td>
                    <asp:TextBox ID="txtTodate" runat="server" Width="80px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"  
                        Enabled="True" TargetControlID="txtTodate" PopupButtonID="imgToDt">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                </td>
                <td>
                <div class="hrtime">
                     <asp:TextBox ID="txtToTimeHr" runat="server" DataTextField="" Width="50px" CssClass=""></asp:TextBox>
                     <asp:TextBox ID="txtToTimeMin" runat="server" DataTextField="" Width="50px" CssClass="try"></asp:TextBox>(HR:MI)  
                     <asp:NumericUpDownExtender ID="txtToTimeHr_NumericUpDownExtender" 
                                        runat="server" Maximum="23" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtToTimeHr" Width="40" />
                                        
                     <asp:NumericUpDownExtender ID="txtToTimeMin_NumericUpDownExtender" 
                                        runat="server" Maximum="59" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtToTimeMin" Width="40" /></div>
                 </td>
                 <td>
                     Employee
                 </td>
                 <td>
                     <asp:TextBox ID="txtEmployee" runat="server" Width="112px"></asp:TextBox>
                 </td>
            </tr>
            <tr>
            <td colspan="2">
                <asp:Button ID="btnList" runat="server" CssClass="button" onclick="btnList_Click" Text="List" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
                    onclick="btnClear_Click" />
                <asp:Button ID="btnExport" runat="server" CssClass="button" Text="Export" onclick="btnExport_Click" Visible="false" />
            </td>
            </tr>
            <tr>
            <td colspan="6">
            
          <%--  <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
           ForeColor="Red"></asp:Label>--%>
            </td>
            </tr>
            
        </table>
    
    </div>
    <%--<div class="rout" visible="false">
    <img src="Images/report.png"  />
        Note-Filter Criterion will be displayed here</div>--%>
        <div style=" border: thin solid #000000; float: left;">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server"  
                Width="1024px">
            </rsweb:ReportViewer>
      
    <%--<dd:webreportviewer ID="RptViewerRevenue_Station" runat="server" Height="450px" 
                Width="1024px" xmlns:dd="datadynamics.reports.web" />--%>
   
    </div>
    </div>

</asp:Content>