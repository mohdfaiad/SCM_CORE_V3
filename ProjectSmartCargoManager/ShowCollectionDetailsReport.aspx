<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowCollectionDetailsReport.aspx.cs" Inherits="ProjectSmartCargoManager.ShowCollectionDetailsReport" %>

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
            Font-Size="8pt" Height="600px" Width="1060px">
            <LocalReport ReportPath="Reports\CollectionDetails.rdlc">
                <DataSources>
                    
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="CollectionDetails_DataTable2" />

                    
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            SelectMethod="GetData" 
            TypeName="ProjectSmartCargoManager.CollectionDetailsTableAdapters.">
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
