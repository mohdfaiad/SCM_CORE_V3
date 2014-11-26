<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="rptCargoLoadFactor.aspx.cs" Inherits="ProjectSmartCargoManager.rptCargoLoadFactor" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <style type="text/css">
        .style1
        {
            width: 139px;
        }
        .style2
        {
            width: 149px;
        }
        .style3
        {
            width: 62px;
        }
        .style5
        {
            width: 135px;
        }
        .style6
        {
            width: 78px;
        }
        .style7
        {
            width: 51px;
        }
    </style>
    
        <script type="text/javascript">
        function DisableButton() {
            document.getElementById("<%=btnList.ClientID %>").disabled = true;
            document.getElementById("<%=btnClear.ClientID %>").disabled = true;
            document.getElementById("<%=btnExportReport.ClientID %>").disabled = true;
        }
        window.onbeforeunload = DisableButton;
    </script>
   
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contentarea">
        <%--<h1>
            <img alt="Revenue" src="Images/stationwisereport.png" /> </h1>--%>
            <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
            <h1>
            Cargo Load Factor Report
            </h1>
    <div class="botline">
    
        <table width="100%" cellpadding="2" cellspacing="2" >
            
            <tr>
                <td>
                    From Date</td>
                <td>
                    <asp:TextBox ID="txtFromdate" runat="server" Width="112px" 
                        ></asp:TextBox>
                    <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFromdate" PopupButtonID="imgFromDate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgFromDate" runat="server" ImageAlign="AbsMiddle" 
                     ImageUrl="~/Images/calendar_2.png" />
                </td>
                <td>
                    To Date</td>
                <td>
                    <asp:TextBox ID="txtTodate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"  
                        Enabled="True" TargetControlID="txtTodate" PopupButtonID="imgToDate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" 
                     ImageUrl="~/Images/calendar_2.png" />
                </td>
                <td>
                    Flights</td>
                <td>
                    <asp:DropDownList ID="ddlStationType" runat="server">
                        <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem Value="DOM">Domestic</asp:ListItem>
                        <asp:ListItem Value="INT">International</asp:ListItem>
                    </asp:DropDownList>
                </td>

        <td __designer:mapid="10" class="style7">
            Station</td>
        <td __designer:mapid="11">
            <asp:DropDownList ID="ddlAirport" runat="server">
            </asp:DropDownList>
        </td>
                                 <td>
                    Carrier</td>
                <td>
                    <asp:DropDownList ID="ddlCarrier" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
            <td colspan="3">
            
          <%--  <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
           ForeColor="Red"></asp:Label>--%>
            <asp:Button ID="btnList" runat="server" CssClass="button" onclick="btnList_Click" 
                        Text="List" />
        
                <asp:Button ID="btnClear" runat="server" CssClass="button" 
                    onclick="btnClear_Click" Text="Clear" />
        
        <asp:Button ID="btnExportReport" runat="server" CssClass="button"  
                        Text="Export"  onclick="btnExportReport_Click" />
            </td>
            </tr>
            
        </table>
    
    </div>
  
        <div style=" border: thin solid #000000; float: left;">
         
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1024px">
            </rsweb:ReportViewer>
            
            </div>
  
  </div>
    

</asp:Content>
