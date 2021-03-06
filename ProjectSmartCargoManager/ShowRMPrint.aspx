﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowRMPrint.aspx.cs" Inherits="ProjectSmartCargoManager.ShowRMPrint" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" Height="600px" Width="1060px">
            <LocalReport ReportPath="Reports\RMPrint.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="dsRMPrint_DataTable1" />
                    
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            SelectMethod="GetData" >
        </asp:ObjectDataSource>

    </div>
    </form>
</body>
</html>

