<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptSLAPerformance.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.rptSLAPerformance" %>


<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
    <style type="text/css">
        .style1
        {
        }
        .style2
        {
            width: 144px;
        }
        .style3
        {
            height: 33px;
        }
        .style5
        {
            width: 92px;
        }
        .style6
        {
            width: 245px;
        }
        .style7
        {
            width: 73px;
        }
        .style8
        {
            width: 117px;
        }
        .style9
        {
        }
        .style10
        {
            width: 109px;
        }
        .style11
        {
            width: 26px;
        }
    </style>
 
</asp:Content>

 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
     
     <%--<script type="text/javascript">

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
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>--%>
     
     <div id="contentarea">
   
    
    <h1>
            <%--<img alt="" src="Images/flightshlist.png"  style="vertical-align:5"/> </h1>--%>
       <h1>
           SLA Performance Report</h1>
          <div class="botline">
            <table width="100%">
                <tr>
                    <td class="style7">
                        Station</td>
                    <td class="style2">
                        <asp:DropDownList ID="ddlStation" runat="server">
              </asp:DropDownList>
                    </td>
                    <td class="style8" >
                        Flight#</td>
                    <td class="style10" >
                      
              <asp:TextBox ID="txtFlightNo" runat="server" Width="100px"></asp:TextBox>
                    </td>
                    
                    <td class="style9" colspan="2" >
                      
              <asp:TextBox ID="txtFlightDate" runat="server" Width="100px" 
                          ToolTip="Please enter valid date format: dd/MM/yyyy" 
                 ></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightDate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightDate">
              </asp:CalendarExtender>
                    </td>
                    
                    <td class="style6">
                        &nbsp;
                        </td>
                    <td class="style1">
                        &nbsp;</td>
                    <td>
                          &nbsp;</td>
                </tr>
                <tr>
                    <td class="style7">
                        Date From</td>
                    <td class="style2">
              <asp:TextBox ID="txtFlightFromdate" runat="server"  Width="100px" 
                            ToolTip="Please enter date format: dd/MM/yyyy"></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightFromdate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightFromdate">
              </asp:CalendarExtender>
                    </td>
                    <td class="style8" >
                        Date To</td>
                    <td class="style10" >
                      
              <asp:TextBox ID="txtFlightToDate" runat="server" Width="100px" 
                          ToolTip="Please enter valid date format: dd/MM/yyyy" 
                 ></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightToDate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightToDate">
              </asp:CalendarExtender>
                    </td>
                    
                    <td class="style11" >
                      
                        &nbsp;</td>
                    
                    <td class="style5">
                        AWB Number</td>
                    <td class="style6">
                    <asp:TextBox ID="txtAWBPrefix" runat="server" Width="30px" MaxLength=3></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtAWBNo" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    <td class="style1">
                        &nbsp;</td>
                    <td>
                          &nbsp;</td>
                </tr>
                <tr>
                    <td class="style3" colspan="9">
                    <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnExport" runat="server" Text="Export" Visible="false"
                            CssClass="button" onclick="btnExport_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnclear" runat="server" Text="Clear" 
                            CssClass="button" onclick="btnclear_Click" />    
                        </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
             ForeColor="Red"></asp:Label></td>
                    <td>
                        &nbsp;</td>
                    <td>
                        
                                            &nbsp;</td>
                </tr>
           </table>
    
   
    
    </div>
    
    <table width="100%">
        <tr>
        <td style="border: thin solid #000000; float: left;overflow:auto">
            
            <br />
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="500px" 
                Width="1000px" >
            </rsweb:ReportViewer>
            
                    </td>  
            
            
</tr>
   </table>
         
         </div>
      
     <%--<div id="msglight" class="white_contentmsg">
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
    </asp:UpdatePanel>--%>
  
    </asp:Content>
