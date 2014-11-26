<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowUCR.aspx.cs" Inherits="ProjectSmartCargoManager.ShowUCR" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 623px; width: 1050px">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" Height="600px" Width="1000px">
            <LocalReport ReportPath="rptUCR.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="dsUCR_DataTable1" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="dsUCR_DataTable2" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="dsUCR_DataTable3" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            SelectMethod="GetData" TypeName="UVMWeb.dsUCRTableAdapters.">
        </asp:ObjectDataSource> 
    </div>
    <%--<asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Back" />--%>
    </form>
</body>
</html>