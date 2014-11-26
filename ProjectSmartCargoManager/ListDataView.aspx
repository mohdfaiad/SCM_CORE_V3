<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListDataView.aspx.cs" Inherits="ProjectSmartCargoManager.ListDataView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%-- <link href="style.css" rel="stylesheet" type="text/css" />--%>
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
            background: #36a3f8;
            color: #ffffff;
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

        function CloseWindow(selectedval,id) {

            opener.document.getElementById('<%= Request["TargetTXT"] %>').value = '' + selectedval;
            opener.document.getElementById('<%= Request["Hid"] %>').value = '' + id;
            window.close();
        }

        function CloseAgentWindow(selectedval, id) {

            opener.document.getElementById('<%= Request["TargetTXT"] %>').value = '' + selectedval;
            window.close();
        }
        function CloseAgentNameWindow(selectedval, id) {

            opener.document.getElementById('<%= Request["TargetTXT"] %>').value = '' + selectedval;
            opener.document.getElementById('<%= Request["AName"] %>').value = '' + id;
            window.close();
        }
        function reSizeWindow() {
            window.resizeTo(570, 350)
                    }
        window.onload = reSizeWindow();
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <br />
    <div style="font-size: medium">
        Select Value
    </div>
    <div class="divgrd" style="width: 500px; height: 200px">
        <asp:GridView ID="GRD" runat="server" AutoGenerateColumns="False" EnableViewState="true"
            ShowFooter="True" Width="100%" Height="100%">
            <Columns>
                <asp:TemplateField HeaderStyle-Width="25px">
                    <ItemTemplate>
                        <asp:RadioButton ID="RBT" runat="server" OnCheckedChanged="select_OnCheckedChanged"
                            AutoPostBack="false" />
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Value" HeaderStyle-Wrap="true" HeaderStyle-Width="90px">
                    <ItemTemplate>
                        <asp:Label ID="LBLValue" runat="server" EnableViewState="true" Text='<%# Eval("Value") %>'></asp:Label>
                        <asp:HiddenField ID="Hid" runat="server" Value='<%# Eval("ID") %>' />
                    </ItemTemplate>
                    <HeaderStyle Wrap="True" HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle Wrap="true"/>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="Name" HeaderStyle-Wrap="true" HeaderStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="LBLName" runat="server" EnableViewState="true" Text='<%# Eval("Name") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True" HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle Wrap="true"/>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="titlecolr" />
            <RowStyle HorizontalAlign="Left" />
            <AlternatingRowStyle HorizontalAlign="Left" />
        </asp:GridView>
    </div>
    <asp:Button ID="btnSelect" runat="server" Text="Select" CssClass="button" 
        onclick="btnSelect_Click" />
    </form>
</body>
</html>
