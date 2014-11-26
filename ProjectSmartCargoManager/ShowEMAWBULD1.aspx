<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowEMAWBULD1.aspx.cs" Inherits="ProjectSmartCargoManager.ShowEMAWBULD1" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    </head>
<body>
    <form id="form1" runat="server">
    <div style="width:1024px">
     
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
     <asp:Panel ID="Search" runat="server" Visible ="false" >
        <br />
        <table style="width:100%;">
            <tr>
                <td colspan="3">
                    Select Manifest Print/Screen</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    Flight #</td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" Width="37px">SG</asp:TextBox>
                    <asp:TextBox ID="txtFlightNo" runat="server" Width="122px"></asp:TextBox>
                </td>
                <td>
                    Flight Date</td>
                <td>
                    <asp:TextBox ID="txtFlightDate" runat="server"></asp:TextBox> 
                      <asp:CalendarExtender ID="txtFlightDate_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="txtFlightDate">
                    </asp:CalendarExtender>
                    </td>
                <td>
                    Airport </td>
                <td>
                    <asp:TextBox ID="txtAirportCode" runat="server" Width="99px"></asp:TextBox></td>
            </tr>
            <tr>
                <td >
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnList" runat="server" Height="22px" Text="List" 
                        onclick="btnList_Click" />
                </td>
                <td>
                    <%--<asp:Button ID="btn" runat="server" Height="22px" Text="List" />--%>
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    </div>
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
        Font-Size="8pt" Height="400px" Width="999px">
        <LocalReport ReportPath="Reports\EXP_ULDArrival.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" 
                    Name="dsArrival_dtManifest" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
        SelectMethod="GetData" 
        TypeName="ProjectSmartCargoManager.dsArrivalTableAdapters.">
    </asp:ObjectDataSource>
    </form>
</body>
</html>
