<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="rptEmission.aspx.cs" Inherits="ProjectSmartCargoManager.rptEmission" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  
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
<asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
  
    <div id="contentarea">
   <h1> 
            Emission Report</h1>
         <%--<p>--%>
         <table>
         <tr>
         <td>
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
         </td>
         </tr>
         </table>
            <%--</p>--%>
 <div class="botline">

<table>
<tr>
<td>
    Origin</td>
    
<td>
    <asp:DropDownList ID="ddlOrigin" runat="server">
    </asp:DropDownList>
</td><td>
        Destination</td>
<td>
    <asp:DropDownList ID="ddlDest" runat="server">
    </asp:DropDownList>
    </td></tr>
    
    
        <caption>
            <br />
            <tr>
                <td>
                    From Date*</td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="100px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" 
                        Format="dd/MM/yyyy" PopupButtonID="ImageButton1" TargetControlID="txtFromDate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsMiddle" 
                        ImageUrl="~/Images/calendar_2.png" />
                </td>
                <td>
                    To Date*</td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" Width="100px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" 
                        Format="dd/MM/yyyy" PopupButtonID="imgDate" TargetControlID="txtToDate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" 
                        ImageUrl="~/Images/calendar_2.png" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnList" runat="server" CssClass="button" 
                        onclick="btnList_Click"  Text="List" />
                    <asp:Button ID="btnExport" runat="server" CssClass="button" 
                        onclick="btnExport_Click" Text="Export" />
                        <asp:Button ID="btnClear" runat="server" CssClass="button" 
                         Text="Clear"  onclick="btnClear_Click"/>
                    
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
    </caption>


</table>
</div>

<asp:Panel ID="pnlReport"  runat="server">
<div style="float:left;overflow:auto;">
 
                    <rsweb:ReportViewer ID="rptEmissionReport" runat="server" Width="1024px" Height="450px" Visible="false">
                    </rsweb:ReportViewer>
   <br />
            
    </div>
</asp:Panel>
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
    <Triggers>
    <asp:PostBackTrigger ControlID="btnExport" />
    </Triggers>
    </asp:UpdatePanel>
</asp:Content>

