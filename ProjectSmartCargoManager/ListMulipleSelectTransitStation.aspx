<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListMulipleSelectTransitStation.aspx.cs" Inherits="ProjectSmartCargoManager.ListMulipleSelectTransitStation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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

        function CloseWindow(selectedval, id) {

            opener.document.getElementById('<%= Request["TargetTXT"] %>').value = '' + selectedval;            
            window.close();
        }  
   </script>
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <div style="font-size: medium">
        Select Value
    </div>
    <br />
    <div class="divgrd" style="width: 350px; height: 165px">
        <asp:GridView ID="GRD" runat="server" AutoGenerateColumns="False" EnableViewState="true"
            ShowFooter="True" Width="95%" Height="100%">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" />
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Value" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="LBLValue" runat="server" EnableViewState="true" Text='<%# Eval("Value") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="true" />
                </asp:TemplateField>
               <%--<asp:TemplateField HeaderText="Detail" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="LBLDetail" runat="server" Width="40px" EnableViewState="true" Text='<%# Eval("Detail") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>--%>
            </Columns>
            <HeaderStyle CssClass="titlecolr" />
            <RowStyle HorizontalAlign="Center" />
            <AlternatingRowStyle HorizontalAlign="Center" />
        </asp:GridView>
    </div>
    <br />
    &nbsp;&nbsp;
    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click">
    </asp:Button>
    &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click">
    </asp:Button>
    </form>
</body>
</html>
