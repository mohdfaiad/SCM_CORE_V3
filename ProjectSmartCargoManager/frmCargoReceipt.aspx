<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCargoReceipt.aspx.cs" Inherits="ProjectSmartCargoManager.frmCargoReceipt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
 <link href="style/style.css" rel="stylesheet" type="text/css" />
    <title></title>
    <script language="javascript">
        function callclose() {
            self.close();
        }
    </script>
    </head>
<body>
    <form id="form1" runat="server">
   
    <div id="divmail" runat="server" style="margin-top:120px;">
    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
    <asp:HiddenField ID="hdn" runat="server" /> 
    <table width = "100%" style="border: 1px solid #000000;"  id="mailmain" runat="server">
    <tr style="border: 1px solid #000000;">
    <td style="border-style: solid; border-width: 1px;" >
    Shipper Name
    </td>
    <td style="border-style: solid; border-width: 1px;">
    Shipment Identification<br />
&nbsp;<asp:Label ID="txtAWB" runat="server" ></asp:Label>
    </td>
    <td style="border-style: solid; border-width: 1px;" rowspan="2" >
     <br>
        <h2> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cargo Receipt</h2>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Issued By&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="txtIssuedBy" runat="server" style="font-weight: 700" ></asp:Label>
        </td>
    </tr>
  
    <tr style="height:100px; border: 1px solid black;">
    <td style="border-style: solid; border-width: 1px;" colspan="2">
        <asp:Label ID="txtShipperName" runat="server" ></asp:Label><br />
        <asp:Label ID="txtShipperAddress" runat="server" ></asp:Label>
        
        </td>
    </tr>
    
    <tr style=" border: 1px solid black;">
    <td style="border-style: solid; border-width: 1px;">
        Day/Month/Time<br />
        (Of Shipment Acceptance)
        </td>
    <td style="border-style: solid; border-width: 1px;">
        Airport/City Code<br />
        (Of Shipment Acceptance)</td>
    <td style="border-style: solid; border-width: 1px;" rowspan="2" >
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <span class="style1">&nbsp;Carriage is Subject to Carrier&#39;s Conditions
        </span>
        <br class="style1" />
        <span class="style1">&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;of Contract Previously made available to Shipper</span></td>
    </tr>
    
    <tr style=" border: 1px solid black;">
    <td style="border-style: solid; border-width: 1px;height:20px;">
        <asp:Label ID="txtDMT" runat="server"></asp:Label>
        </td>
    <td style="border-style: solid; border-width: 1px;">
        <asp:Label ID="txtAirCode" runat="server"></asp:Label>
        </td>
    </tr>
    
    <tr style=" border: 1px solid black;">
    <td style="vertical-align:top;" colspan="3">
       <table width="100%">
       <tr>
       <td style="border-style: solid; border-width: 1px">No. of&nbsp; Pieces</td>
       <td style="border-style: solid; border-width: 1px">Gross Weight</td>
       <td style="border-style: solid; border-width: 1px">K/L</td>
       <td style="border-style: solid; border-width: 1px">Volume</td>
       <td style="border-style: solid; border-width: 1px">Airport/City Code (of Origin)</td>
       <td style="border-style: solid; border-width: 1px">Airport/City Code (of Destination)</td>
       <td style="border-style: solid; border-width: 1px">Airport/City Code (of Routing)</td>
       
       </tr>
       <tr>
       <td style="border-style: solid; border-width: 1px">
           <asp:Label ID="txtNoOFPiece" runat="server"></asp:Label>
           </td>
       <td style="border-style: solid; border-width: 1px">
           <asp:Label ID="txtGW" runat="server"></asp:Label>
           </td>
       <td style="border-style: solid; border-width: 1px;text-align:center;">K</td>
       <td style="border-style: solid; border-width: 1px">
           <asp:Label ID="txtVol" runat="server"></asp:Label>
           </td>
       <td style="border-style: solid; border-width: 1px">
           <asp:Label ID="txtAirCodeO" runat="server"></asp:Label>
           </td>
       <td style="border-style: solid; border-width: 1px">
           <asp:Label ID="txtAirCodeD" runat="server"></asp:Label>
           </td>
       <td style="border-style: solid; border-width: 1px">
           <asp:Label ID="txtAirCodeR" runat="server"></asp:Label>
           </td>
       
       </tr>
       </table> </td>
    </tr>
    
    </table>
    
    </div>
    <br />
    <div style="text-align:center;">
    <asp:TextBox ID="txtSendID" runat="server" TextMode="MultiLine" Width="250px" 
            Height="60px" ToolTip="Email must be Comma (,) Seperated"></asp:TextBox>
    <asp:Button runat="server" ID="btnsendmail" Text="Send Mail"  
             onclick="btnsendmail_Click" CssClass="button"/>
    </div>
    </form>
</body>
</html>
