<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListMultipleSelect.aspx.cs"
    Inherits="ProjectSmartCargoManager.ListMultipleSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
        function reSizeWindow() {
            window.resizeTo(600, 350)
        }
        window.onload = reSizeWindow();

        function SelectAllULD(headerchk) {
            var gvcheck = document.getElementById("<%=GRD.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    if (inputs[0].disabled == false)
                        inputs[0].checked = true;
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
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
    <div class="divgrd" style="width: 500px; height: 170px">
        <asp:GridView ID="GRD" runat="server" AutoGenerateColumns="False" EnableViewState="true"
            ShowFooter="True" Width="100%" Height="100%">
            <Columns>
                <asp:TemplateField HeaderStyle-Width="25px">
                <HeaderTemplate>
                     <asp:CheckBox ID="chkSelectAllStn" runat="server" onclick="javascript:SelectAllULD(this);" />
                </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" />
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Value" HeaderStyle-Wrap="true" HeaderStyle-Width="90px">
                    <ItemTemplate>
                        <asp:Label ID="LBLValue" runat="server" EnableViewState="true" Text='<%# Eval("Value") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="true" />
                </asp:TemplateField>
             <asp:TemplateField HeaderText="Name" HeaderStyle-Wrap="true" HeaderStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="LBLName" runat="server" EnableViewState="true" Text='<%# Eval("Name") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="True" />
                </asp:TemplateField>
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
