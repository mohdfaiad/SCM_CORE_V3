<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmConBooking.aspx.cs" Inherits="ProjectSmartCargoManager.FrmConBooking" %>

 2012-04-05  vinayak
 2012-05-05  vinayak
 2012-06-25  vinayak

--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewInfoInGrid.aspx.cs"
    Inherits="ProjectSmartCargoManager.ViewInfoInGrid" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="style/style.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
       
        .divgrd
        {
            overflow: scroll;
            padding:5px;
            width:600px;
            margin:5px;
            
        }
        
      
    </style>

    <script type="text/javascript">

       
        function CloseWindow(charge, tax, total,subtotal) {

            //TxtChargeID TxtTaxID TxtTotalID
            opener.document.getElementById('<%= Request["TxtSubTotalID"] %>').value = '' + subtotal;          
            opener.document.getElementById('<%= Request["TxtChargeID"] %>').value = '' + charge;
            opener.document.getElementById('<%= Request["TxtTaxID"] %>').value = '' + tax;
            opener.document.getElementById('<%= Request["TxtTotalID"] %>').value = '' + total;
            
            window.close();
        } 
        
         
        
        
    
    </script>

</head>
<body style="background:#ffffff !important;">
    <form id="form1" runat="server">
    <div class="divgrd" style="height:300px; border:solid 5px #cccccc;">
    <h1>Oc Due
        <asp:Label ID="lbltype" runat="server"></asp:Label>
        </h1>
        <h2 style="color:#0e2f8e;"> Currency:
        <asp:Label ID="lblCurrency" runat="server"></asp:Label></h2>
        
        <asp:Label ID="LBLStatus" runat="server" ForeColor="Red"></asp:Label>
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnAdd" runat="server" CssClass="button" OnClick="btnAdd_Click" Text="Add" />
                    &nbsp;
                    <asp:Button ID="btnDelete" runat="server" CssClass="button" OnClick="btnDelete_Click"
                        Text="Delete" />
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
                        <asp:Label ID="LBLCharge" runat="server" Width="10px" EnableViewState="true" Visible="false" Text='<%# Eval("Charge") %>'></asp:Label>
                        <%--<br />--%>
                        <asp:TextBox ID="TXTCharge" runat="server" Text='<%# Eval("Charge") %>' Visible="true" Enabled="false"
                            AutoPostBack="True" OnTextChanged="TXTCharge_TextChanged" Width="100px"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tax" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="LBLTax" runat="server" Width="10px" EnableViewState="true" Text='<%# Eval("Tax") %>' Visible="false"></asp:Label>
                        <%--<br />--%>
                        <asp:TextBox ID="TXTTax" runat="server" Text='<%# Eval("Tax") %>' Width="100px" Visible="true" Enabled="false"></asp:TextBox>
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
