<%@ Page  Title="Route Wise Tonnage Report" Language="C#" AutoEventWireup="true" CodeBehind="rptLegWiseReport.aspx.cs" Inherits="ProjectSmartCargoManager.rptLegWiseReport" MasterPageFile="~/SmartCargoMaster.Master"%>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
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
       <h1>
           Route Wise Tonnage
       </h1>
 

     <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="botline">
    
        <table width="70%" cellpadding="2" cellspacing="2" >
            <%--<tr>
            
                <td>Origin</td>
                <td>
                 <asp:DropDownList ID="ddlOrigin" runat="server" >   
                 </asp:DropDownList>
                </td>
                
                <td>Destination</td>
                <td>
                    <asp:DropDownList ID="ddlDest" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>--%>
            
            <tr>
                <td>From Date</td>
                <td>
                    <asp:TextBox ID="txtFromdate" runat="server" Width="112px" ></asp:TextBox>
                    <asp:ImageButton ID="imgFromDate" runat="server" ImageAlign="AbsMiddle" 
                     ImageUrl="~/Images/calendar_2.png" />
                    <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFromdate" PopupButtonID="imgFromDate">
                    </asp:CalendarExtender>
                </td>
                
                <td>To Date</td>
                <td>
                    <asp:TextBox ID="txtTodate" runat="server" Width="112px"></asp:TextBox>
                    <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" 
                     ImageUrl="~/Images/calendar_2.png" />
                    <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"  
                        Enabled="True" TargetControlID="txtTodate" PopupButtonID="imgToDate">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                <asp:Button ID="btnList" runat="server" CssClass="button" onclick="btnList_Click" Text="List" />
                &nbsp;
                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" onclick="btnExport_Click" />
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
            <td colspan="4">
            
           <%-- <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
           ForeColor="Red" Font-Size="Large"></asp:Label>--%>
            </td>
            </tr>
            
        </table>
    
    </div>
    <%--<div class="rout" visible="false">
    <img src="Images/report.png"  />
        Note-Filter Criterion will be displayed here</div>--%>
     <div style=" border: thin solid #000000; float:left;">
     
    <rsweb:ReportViewer ID="RptLegWiseViewer" runat="server" Width="1022px">
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
    


</asp:Content>
