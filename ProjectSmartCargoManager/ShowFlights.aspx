
<%--

 2012-06-12  vinayak
 2012-06-25  vinayak
 2012-07-24  vinayak
 2012-07-25  vinayak
 2012-07-30  vinayak
 2012-08-03  vinayak

--%>


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowFlights.aspx.cs" Inherits="ProjectSmartCargoManager.ShowFlights" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style/style.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">

        function DoneClick(button) {

            opener.document.getElementById('<%= Request["Hid"] %>').value = "1";                
            window.close();            
            window.opener.CalledFromShowFlights();
            
        }
    </script>
    
</head>
<body style="background: url()">
    <form id="form1" runat="server">
    <div style="overflow:scroll;  height:300px" >
        <br />
        <asp:Label ID="LBLStatus" runat="server" ForeColor="Red"></asp:Label>
        <br />
        <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" CssClass="grdrowfont"
            Width="399px" ID="GRDShowFlights">
            <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:CheckBox ID="CHKSelect" runat="server" AutoPostBack="True" TabIndex="1"
                            oncheckedchanged="CHKSelect_CheckedChanged" />
                        <asp:HiddenField ID="HidScheduleID" runat="server" Value='<%# Eval("ScheduleID") %>' />
                        <asp:HiddenField ID="HidSerial" runat="server" Value='<%# Eval("Serial") %>' />
                    </ItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flt #">
                    <ItemTemplate>
                        <asp:Label ID="LBLFltNum" runat="server" Text='<%# Eval("FltNumber") %>' Width="100px" />
                    </ItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Origin">
                    <ItemTemplate>
                        <asp:Label ID="LBLFltOrg" runat="server" Text='<%# Eval("FltOrigin") %>' Width="100px" />
                    </ItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Destination">
                    <ItemTemplate>
                        <asp:Label ID="LBLFltDest" runat="server" Text='<%# Eval("FltDestination") %>' Width="100px" />
                    </ItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rate Per Kg">
                    <ItemTemplate>
                        <asp:Label ID="LBLRatePerKg" runat="server" Text='<%# Eval("RatePerKg") %>' Width="100px" />
                    </ItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Departure Time">
                    <ItemTemplate>
                        <asp:Label ID="LBLDeptTime" runat="server" Text='<%# Eval("DeptTime") %>' Width="100px" />
                    </ItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Arrival Time">
                    <ItemTemplate>
                        <asp:Label ID="LBLArrTime" runat="server" Text='<%# Eval("ArrTime") %>' Width="100px" />
                    </ItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flt Date">
                    <ItemTemplate>
                        <asp:Label ID="LBLFltDate" runat="server" Text='<%# Eval("FltDate") %>' Width="100px" />
                    </ItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Cargo Capacity">
                    <ItemTemplate>
                        <asp:Label ID="LBLCapacity" runat="server" Text='<%# Eval("CargoCapacity") %>' Width="100px" />
                    </ItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Equipment">
                    <ItemTemplate>
                        <asp:Label ID="LBLEquipment" runat="server" Text='<%# Eval("EquipmentNo") %>' Width="100px" />
                    </ItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="Carrier">
                    <ItemTemplate>
                        <asp:Label ID="LBLCarrier" runat="server" Text='<%# Eval("Partner") %>' Width="100px" />
                    </ItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="ColorFlag" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="LBLColorFlag" runat="server" Text='<%# Eval("ColorFlag") %>' Width="50px" />
                    </ItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:TemplateField>             
            </Columns>
            <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
            <FooterStyle CssClass="grdrowfont"></FooterStyle>
            <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
            <RowStyle CssClass="grdrowfont"></RowStyle>
        </asp:GridView>
        <br />
        &nbsp;&nbsp;
        <asp:Button ID="btnOk" runat="server" CssClass="button" Text="Done" TabIndex="2" 
            onclick="btnOk_Click"   OnClientClick="DoneClick(this);"/>
        
    </div>
    </form>
</body>
</html>
