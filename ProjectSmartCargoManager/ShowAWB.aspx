<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowAWB.aspx.cs" Inherits="ProjectSmartCargoManager.ShowAWB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link href="style/style.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">

        function DoneClick(button) {

                        
            window.close();            
          
            
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
            Width="100px" ID="GRDShowFlights">
            <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
            <Columns>
           
           
                
                <asp:TemplateField HeaderText="AWBNumber">
                    <ItemTemplate>
                        <asp:Label ID="LBLAWBNumber" runat="server" Text='<%# Eval("AWBNumber") %>' Width="399px" />
                    </ItemTemplate>
                    <ItemStyle Wrap="True"></ItemStyle>
                </asp:TemplateField>
             
            </Columns>
            <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
            <FooterStyle CssClass="grdrowfont"></FooterStyle>
            <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
            <RowStyle CssClass="grdrowfont"></RowStyle>
        </asp:GridView>
        <br />
        &nbsp;&nbsp;
        <asp:Button ID="btnOk" runat="server" CssClass="button" Text="Done" 
             OnClientClick="DoneClick(this);"/>
        
    </div>
    </form>
</body>
</html>
