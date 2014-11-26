<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillingEditFlightDetails.aspx.cs" Inherits="ProjectSmartCargoManager.BillingEditFlightDetails" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

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
            width: 900px;
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
            width: 900px;
            padding-top: 6px;
        }
    </style>
    
    

</head>

<body>
    <script type="text/javascript">

    function CloseWindow(total) {
        opener.document.getElementById('<%= Request["TxtChargeID"] %>').value = '' + total;

        window.close();
        window.opener.Changed();
    }

    </script>
    <form id="form1" runat="server">
    <br />
    <div class="divgrd" style="width: 1100px; height: 400px">
        <br />
        <asp:Label ID="LBLStatus" runat="server" ForeColor="Red"></asp:Label>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <table width="500px">
            <tr style="visibility:hidden">
                <td width="500px" align="right">
                    <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add" 
                        onclick="btnAdd_Click" />
                    &nbsp;
                    <asp:Button ID="btnDelete" runat="server" CssClass="button" Text="Delete" 
                        onclick="btnDelete_Click" />
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
                <asp:TemplateField HeaderText="Org">
                    <ItemTemplate>
                        <asp:Label ID="LBLOrigin" runat="server" Width="60px" EnableViewState="true"
                            Text='<%# Eval("Origin") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dest">
                    <ItemTemplate>
                        <asp:Label ID="LBLDest" runat="server" Width="60px" EnableViewState="true"
                            Text='<%# Eval("Destination") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flight #">
                    <ItemTemplate>
                        <asp:Label ID="LBLFlightNo" runat="server" Width="100px" EnableViewState="true"
                            Text='<%# Eval("FlightNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="New Flight #">
                    <ItemTemplate>
                        <asp:TextBox ID="TXTFlightNo" runat="server" Width="100px"  Visible="false" Enabled="false"
                            Text='<%# Eval("NewFlightNumber") %>'></asp:TextBox>
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flt Date" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="TXTFlightDate" runat="server" Visible="false" Enabled="false" Width="100px" Text='<%# Eval("FlightDate") %>'></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" Format="dd/MM/yyyy" TargetControlID="TXTFlightDate" runat="server">
                        </asp:CalendarExtender>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="PCS" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="TXTPcs" runat="server" Text='<%# Eval("Pieces") %>' Visible="true" Enabled="false" Width="60px" ></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Charged Wt" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="TXTChWeight" runat="server" Text='<%# Eval("ChargedWeight") %>' Visible="false" Enabled="false" Width="60px"
                        AutoPostBack="true" OnTextChanged="TXTChWeight_TextChanged" ></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rate per Kg" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="TXTRate" runat="server" Text='<%# Eval("RatePerKg") %>' Visible="false" Width="60px"
                        AutoPostBack="true" OnTextChanged="TXTRate_TextChanged" ></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rate Type">
                    <ItemTemplate>
                        <asp:Label ID="LBLRateType" runat="server" Width="60px" EnableViewState="true"
                            Text='<%# Eval("RateType") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Freight" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="TXTFreight" runat="server" Text='<%# Eval("Freight") %>' Width="60px" Visible="false" Enabled ="false"></asp:TextBox>
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
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
            Width="60px" onclick="btnSave_Click"
            ></asp:Button>
    </div>
    </form>
</body>
</html>