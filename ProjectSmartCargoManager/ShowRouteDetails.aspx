<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowRouteDetails.aspx.cs" Inherits="ProjectSmartCargoManager.ShowRouteDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .divback
        {
            background: url(images/divback.png) repeat-x scroll left bottom;
            border: 1px solid #d2cfca;
            border-radius: 6px;
            padding: 10px;
            margin: 0px;
            width: 546px;
            height: 76px;
        }
        .divgrd
        {
            overflow: scroll;
        }
        .titlecolr
        {
            background:#c62534; color:#ffffff;
            line-height: 20px;
            font-family: Calibri, Arial, Helvetica, sans-serif;
            font-size: 14px;
        }
        .button
        {
            background: url(images/buton.png) repeat-x;
            border-radius: 6px;
            color: #FFFFFF;
            font-weight: bold;
            font-family: Calibri, Arial, Helvetica, sans-serif;
            font-size: 13px;
        }
        .button:hover
        {
            background: url(images/butin.png) repeat-x;
            border-radius: 6px;
            color: #FFFFFF;
            font-weight: bold;
            font-family: Calibri, Arial, Helvetica, sans-serif;
            font-size: 13px;
        }
        .buttonSearch
        {
            background: none;
        }
        .botline
        {
            border-bottom: 1px solid #a9acb0;
            padding-bottom: 6px;
            float: left;
            width: 718px;
            padding-top: 6px;
        }
    </style>
    <script type="text/javascript">

        function CloseWindow() {
            window.close();
        }  
    
    </script>

</head>
<body>
<form id="form1" runat="server">
    <br />
    <div style="font-size: Largel; font-weight:bold ">
        Route Details
    </div>
    <br />
    <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ></asp:Label>
        </p>
    <div class="divback">
        <asp:GridView ID="grdRouteDetails" runat="server" AutoGenerateColumns="False" EnableViewState="true"
            ShowFooter="True" Width="100%">
             <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblOrigin" runat="server" Width="40px" EnableViewState="true" Text='<%# Eval("FltOrigin") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Destination" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblDestination" runat="server" Width="40px" EnableViewState="true" Text='<%# Eval("FltDestination") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flight #" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblFlightNo" runat="server" Width="40px" EnableViewState="true" Text='<%# Eval("FltNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flight Date" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblFlightDt" runat="server" Width="40px" EnableViewState="true" Text='<%# Eval("FltDate") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pieces" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblPieces" runat="server" Width="40px" EnableViewState="true" Text='<%# Eval("Pcs") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Weight" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblWeight" runat="server" Width="40px" EnableViewState="true" Text='<%# Eval("Wt") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="titlecolr"></HeaderStyle>
            <RowStyle HorizontalAlign="Center" />
            <AlternatingRowStyle HorizontalAlign="Center" />
        </asp:GridView>
    </div>
    <br />
    &nbsp;&nbsp;
    &nbsp;<asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" OnClientClick="CloseWindow();">
    </asp:Button>
    </form>
</body>
</html>
