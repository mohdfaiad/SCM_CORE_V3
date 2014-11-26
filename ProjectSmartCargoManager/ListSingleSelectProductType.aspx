<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListSingleSelectProductType.aspx.cs" Inherits="ProjectSmartCargoManager.ListSingleSelectProductType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="style/style.css" rel="stylesheet" type="text/css" />
<link href="style/jetGridView.css" rel="stylesheet" type="text/css" />

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

        function CloseWindow(selectedval) {
            opener.document.getElementById('<%= Request["TxtClientObject"] %>').value = '' + selectedval;            
            window.close();
        }

        function RadioCheck(rb) {
            var gv = document.getElementById("<%=GRD.ClientID%>");
            var rbs = gv.getElementsByTagName("input");

            var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "radio") {
                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;
                        break;
                    }
                }
            }
        }
        
   </script>
</head>
<body style="background:#FFFFFF;">
    <form id="form1" runat="server">
    <div style="width:700px;margin:10px auto;overflow:scroll;">
        <h1 style="width:90%;" >
        Available Product Types
        </h1> 
        <asp:Label ID="lblStatus" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
        <asp:GridView ID="GRD" runat="server" AutoGenerateColumns="False" EnableViewState="true"
            ShowFooter="True" Width="90%" Height="100%">
            <AlternatingRowStyle CssClass="trcolor" HorizontalAlign="Center"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:RadioButton ID="rbSelect" runat="server" onclick="javascript:RadioCheck(this);"/>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sr No" HeaderStyle-Wrap="true" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblSrNo" runat="server" Width="40px" EnableViewState="true" 
                        Text='<%# Eval("SerialNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Product Type" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblProductType" runat="server" Width="140px" EnableViewState="true" 
                        Text='<%# Eval("ProductType") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Product Description" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblProductDescription" runat="server" Width="140px" EnableViewState="true" 
                        Text='<%# Eval("ProductDescription") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Available Capacity" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAvailableCapacity" runat="server" Width="140px" EnableViewState="true" 
                        Text='<%# Eval("AvailableCapacity") + " " +Eval("BalanceCapacity") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rate/Kg" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblRatePerKg" runat="server" Width="140px" EnableViewState="true" 
                        Text='<%# Eval("RatePerKg") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
            </Columns>
            <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
            <FooterStyle CssClass="grdrowfont"></FooterStyle>
            <HeaderStyle CssClass="HeaderStyle"></HeaderStyle>
            <RowStyle CssClass="grdrowfont" HorizontalAlign="Center"></RowStyle>
        </asp:GridView>
        <br />
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click">
        </asp:Button>
        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click">
    </asp:Button>
    </div>
    
    </form>
</body>
</html>
