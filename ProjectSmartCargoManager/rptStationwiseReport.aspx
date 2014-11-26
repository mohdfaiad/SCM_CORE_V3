<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="rptStationwiseReport.aspx.cs" Inherits="ProjectSmartCargoManager.rptStationwiseReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>
     <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <style type="text/css">
        .style3
        {
            width: 170px;
        }
        .style4
        {
            width: 343px;
        }
        .style5
        {
            width: 140px;
        }
    </style>
 <script language="javascript" type="text/javascript">

     function SelectAllStations(chkObj) {
         var multi = document.getElementById("<%= chkListShipmentType.ClientID %>");
         var chkBoxCount = multi.getElementsByTagName("input");
         if (chkObj.checked)
             for (i = 0; i < chkBoxCount.length; i++)
             chkBoxCount[i].checked = true;
         else
             for (i = 0; i < chkBoxCount.length; i++)
             chkBoxCount[i].checked = false;
     }

     function GetAgentCode() {
         var level = 'AgentCode';
         var TxtOriginClientObject = '<%=txtAgentCode.ClientID %>';
         window.open('ListDataView.aspx?Parent=AgentCode&level=' + level + '&TargetTXT=' + TxtOriginClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
         return false;
     }

    </script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--<script type="text/javascript">

    function Validation() {
        var checkAll = document.getElementById("<%= chkAll.ClientID %>");
        var checkDomestic = document.getElementById("<%= chkDomestic.ClientID %>");
        var checkInternational = document.getElementById("<%= chkInternational.ClientID %>");
        var checkPOMail = document.getElementById("<%= chkPOMail.ClientID %>");
        if (checkAll.checked = true) {
            checkDomestic.checked = false;
            checkInternational.checked = false;
            checkPOMail.checked = false;
        }
        else{

            checkAll.checked = false;
        }


    }


    function ValidationInternational() {
        var checkAll = document.getElementById("<%= chkAll.ClientID %>");
        var checkDomestic = document.getElementById("<%= chkDomestic.ClientID %>");
        var checkInternational = document.getElementById("<%= chkInternational.ClientID %>");
        var checkPOMail = document.getElementById("<%= chkPOMail.ClientID %>");
        if (checkInternational.checked = true) {
            checkAll.checked = false;
        }

    }

    function ValidationDomestic() {
        var checkAll = document.getElementById("<%= chkAll.ClientID %>");
        var checkDomestic = document.getElementById("<%= chkDomestic.ClientID %>");
        var checkInternational = document.getElementById("<%= chkInternational.ClientID %>");
        var checkPOMail = document.getElementById("<%= chkPOMail.ClientID %>");
        if (checkDomestic.checked = true) {
            checkAll.checked = false;
        }

    }

    function ValidationMail() {
        var checkAll = document.getElementById("<%= chkAll.ClientID %>");
        var checkDomestic = document.getElementById("<%= chkDomestic.ClientID %>");
        var checkInternational = document.getElementById("<%= chkInternational.ClientID %>");
        var checkPOMail = document.getElementById("<%= chkPOMail.ClientID %>");
        if (checkPOMail.checked = true) {
            checkAll.checked = false;
        }

    }
</script>--%>
<%--
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            //document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }
     </script>
    --%>
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

    <div id="contentarea">
    <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
       <%-- <h1>
            <img alt="Revenue" src="Images/stationwisereport.png" /> 
            
            </h1>--%>
       <h1>
           Station Wise Tonnage
       </h1>

     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="botline">
    
        <table width="80%" cellpadding="2" cellspacing="2" >
            <tr>
                <td>
                    AgentCode</td>
                <td>
                    <asp:TextBox ID="txtAgentCode" runat="server" Width="112px"></asp:TextBox>
                    <asp:ImageButton ID="IBOrigin" runat="server" ImageUrl="~/Images/list_bullets.png"
                         ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />
                </td>
                <td>
                    Payment Type </td>
                <td>
                    <asp:DropDownList ID="ddlPaymentType" runat="server">
                     <asp:ListItem>All(Without FOC)</asp:ListItem>
                    <asp:ListItem>PP</asp:ListItem>
                        <asp:ListItem>CC</asp:ListItem>
                         <asp:ListItem>FOC</asp:ListItem>
                       
                     
                    </asp:DropDownList>
                </td>
                <td rowspan"5" rowspan="5">
                 Select Station<br />
                <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" OnClick="javascript:SelectAllStations(this);" AutoPostBack="true"/>
                    <div style="OVERFLOW-Y:scroll; WIDTH:179px; HEIGHT:70px">
                    <asp:CheckBoxList ID="chkListShipmentType" runat="server"  
                        meta:resourcekey="chkListStationResource1">
                        </asp:CheckBoxList> 
                     </div>
                   </td>
                <td>
                    
                </td>
            </tr>
            <tr>
                <td>
                    Controlling Location Code</td>
                <td>
                   
                       <asp:DropDownList ID="ddlControlingLocator" runat="server" >   
                       <asp:ListItem>All</asp:ListItem>
                       <asp:ListItem>Yes</asp:ListItem>
                        <asp:ListItem>No</asp:ListItem>
                        
                     
                    </asp:DropDownList>
                       
                </td>
                <td>
                    Level</td>
                <td >
                   
                    <asp:DropDownList ID="ddlType" runat="server" 
                        onselectedindexchanged="ddlType_SelectedIndexChanged" AutoPostBack="True">
                           <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem>Airport</asp:ListItem>
                       <%-- <asp:ListItem>City</asp:ListItem>--%>
                        <asp:ListItem>Region</asp:ListItem>
                        <asp:ListItem>Country</asp:ListItem>
                      
                    </asp:DropDownList>
                       
                    <asp:DropDownList ID="ddlCode" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    From Date</td>
                <td>
                    <asp:TextBox ID="txtFromdate" runat="server" Width="112px"></asp:TextBox>
                    <asp:ImageButton ID="imgAWBFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                    <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" PopupButtonID="imgAWBFromDt" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFromdate">
                    </asp:CalendarExtender>
                </td>
                <td>
                    To Date</td>
                <td>
                    <asp:TextBox ID="txtTodate" runat="server" Width="112px"></asp:TextBox>
                     <asp:ImageButton ID="imgAWBToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                    <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server" PopupButtonID="imgAWBToDt" Format="dd/MM/yyyy"  
                        Enabled="True" TargetControlID="txtTodate">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>
                    Agent Reference Code</td>
                <td>
                    <asp:TextBox ID="txtAgentReferenceCode" runat="server" Width="112px"></asp:TextBox></td>
                <td>
                    &nbsp;</td>
                <td>
                   
                       <asp:TextBox ID="txtControllingCode" visible="false"  runat="server" Width="112px"></asp:TextBox>
                       
                </td>
            </tr>
            <tr>
                <td>
    
        <asp:Button ID="btnList" runat="server" CssClass="button" onclick="btnList_Click" Text="List" />
                        
    &nbsp;
                        
    <asp:Button ID="btnCLear" runat="server" CssClass="button" Text="Clear" 
                        onclick="btnCLear_Click" />
                        
      &nbsp;
                        
      <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" onclick="btnExport_Click"/>
    
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <table>
            <tr>
                <td class="style3">
                    <%--<asp:CheckBox ID="chkDomestic" runat="server" Text="Domestic" onchange="javascript:ValidationDomestic()"  />
                    <asp:CheckBox ID="chkInternational" runat="server" Text="International" onchange="javascript:ValidationInternational()" />
                </td>
                <td>
                    <asp:CheckBox ID="chkPOMail" runat="server" Text="POMail" onchange="javascript:ValidationMail()" />--%>
                    <%--<asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" OnClick="javascript:SelectAllStations(this);" AutoPostBack="true"/>
                    <div style="OVERFLOW-Y:scroll; WIDTH:180px; HEIGHT:70px">
                    <asp:CheckBoxList ID="chkListShipmentType" runat="server" 
                        meta:resourcekey="chkListStationResource1">
                        </asp:CheckBoxList> 
                     </div>--%>
                </td>
                
                <td>
                    &nbsp;</td>
                <td>
    
                    <asp:DropDownList ID="ddlAWBStatus" runat="server" Visible="False">
                           <asp:ListItem>All</asp:ListItem>
                       <asp:ListItem>Booked</asp:ListItem>
                        <asp:ListItem>Executed</asp:ListItem>
                        <asp:ListItem>Reopened</asp:ListItem>
                       
                        <asp:ListItem>Void</asp:ListItem>
                      
                    </asp:DropDownList>
                </td>
                <td class="style4">
    
            
                    </td>
                <td class="style5">
            
                     </td>
            </tr>
            </table>
            <tr>
            <td colspan="3">
            
           <%-- <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
           ForeColor="Red"></asp:Label>--%>
            </td>
            </tr>
            
        </table>
    
    </div>
    <%--<div class="rout" visible="false">
    <img src="Images/report.png"  />
        Note-Filter Criterion will be displayed here</div>--%>
        <div style="border: thin solid #000000; float:left">
  <rsweb:ReportViewer ID="RptStnwiseViewer" runat="server" Width="1022px" 
                >
            </rsweb:ReportViewer>           
  
    </div>
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
    </div>
    
    </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
