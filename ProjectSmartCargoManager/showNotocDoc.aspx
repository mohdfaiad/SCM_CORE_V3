<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="showNotocDoc.aspx.cs" Inherits="ProjectSmartCargoManager.showNotocDoc" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" Height="437px" Width="922px">
            <LocalReport ReportPath="Reports\NOTOC_New.rdlc">
                <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="dsNOTOC_New_dtNOTOC_New" />
                   <%-- <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="dsNotoc_dtNotoc" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="dsNotoc_dtNotocAWBs" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="dsNotoc_dtOtherSpecialLoad" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="dsNotoc_DataTable1" />--%>
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            SelectMethod="GetData" 
            TypeName="ProjectSmartCargoManager.dsNOTC_NewTableAdapters.">
        </asp:ObjectDataSource>
    
    </div>
    </form>
</body>
</html>
