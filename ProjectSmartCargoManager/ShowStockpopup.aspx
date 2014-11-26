<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowStockpopup.aspx.cs" Inherits="ProjectSmartCargoManager.ShowStockpopup" %>

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
       <asp:GridView ID="grdStockAllocation" runat="server" AutoGenerateColumns="false"  Width="100%" CellSpacing="2" CellPadding="2" PageSize="10" >
    <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle >
    
    <Columns>
  
               
                <asp:BoundField DataField="AFrom" HeaderText="From" />
                <asp:BoundField DataField="ATo" HeaderText="To" />
                <asp:BoundField DataField="Available" HeaderText="Available AWB" />
                 <asp:BoundField DataField="Last" HeaderText="Last Allocated" />
               
                </Columns>
            <HeaderStyle CssClass="titlecolr" />
            <RowStyle HorizontalAlign="Center" />
    </asp:GridView>
        <br />
        &nbsp;&nbsp;
        <asp:Button ID="btnOk" runat="server" CssClass="button" Text="Done" 
             OnClientClick="DoneClick(this);"/>
        
    </div>
    </form>
</body>
</html>
