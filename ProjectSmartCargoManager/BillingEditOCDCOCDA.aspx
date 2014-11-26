<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillingEditOCDCOCDA.aspx.cs" Inherits="ProjectSmartCargoManager.BillingEditOCDCOCDA" %>

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

        function CloseWindowDC(total) {
            opener.document.getElementById('<%= Request["TxtChargeID"] %>').value = '' + total;
            
            window.close();
            window.opener.Changed();
        }

        function CloseWindowDA(total) {

            opener.document.getElementById('<%= Request["TxtChargeID"] %>').value = '' + total;
            
            window.close();
            window.opener.Changed();
        }  
        
        
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <br />
    <div class="divgrd" style="width: 500px; height: 400px">
        <br />
        <asp:Label ID="LBLStatus" runat="server" ForeColor="Red"></asp:Label>
        <table width="500px">
            <tr>
                <td width="500px" align="right">
                    <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add" 
                        onclick="btnAdd_Click" />
                    &nbsp;
                    <asp:Button ID="btnDelete" runat="server" CssClass="button"
                        Text="Delete" onclick="btnDelete_Click" />
                    &nbsp;
                </td>
            </tr>
        </table>
        <asp:GridView ID="GRD" runat="server" AutoGenerateColumns="False" EnableViewState="true"
            ShowFooter="True">
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:CheckBox ID="CHKSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Charge Head Name">
                    <ItemTemplate>
                        <asp:Label ID="LBLChargehead" runat="server" Width="100px" EnableViewState="true"
                            Text='<%# Eval("ChargeName") %>'></asp:Label>
                        <br />
                        <asp:DropDownList ID="DDLChargeHead" runat="server" Visible="false" OnSelectedIndexChanged="DDLChargeHead_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Charge Head Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="LBLCode" runat="server" Width="100px" EnableViewState="true" Text='<%# Eval("Charge Head Code") %>'></asp:Label>
                        <br />
                        <asp:Label ID="LBLChargeHeadCode" runat="server" Visible="false"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Charge" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="TXTCharge" runat="server" Text='<%# Eval("Charge") %>' Visible="false"
                            AutoPostBack="True" OnTextChanged="TXTCharge_TextChanged" Width="100px"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tax" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="TXTTax" runat="server" Text='<%# Eval("Tax") %>' Width="100px" Visible="false"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
            </Columns>
           
            <HeaderStyle CssClass="titlecolr" />
            <RowStyle HorizontalAlign="Center" />
            <AlternatingRowStyle HorizontalAlign="Center" />
        </asp:GridView>
        <br />
        &nbsp;&nbsp;
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" Width="60px"
            OnClick="btnSave_Click"></asp:Button>
    </div>
    </form>
</body>
</html>
