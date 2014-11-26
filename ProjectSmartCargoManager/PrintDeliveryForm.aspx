<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintDeliveryForm.aspx.cs" Inherits="ProjectSmartCargoManager.PrintDeliveryForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Print DO</title>
    
    </head>
     <script language="javascript" type="text/javascript">
         function printpage() {
            document.getElementById("<%=btnClear.ClientID %>").style.display = 'none';
            if (document.getElementById("<%=btnnext.ClientID %>"))
             {
                document.getElementById("<%=btnnext.ClientID %>").style.display = 'none';
             }
            document.getElementById("<%=btnprint.ClientID %>").style.display = 'none';
            var lbl = document.getElementById("<%=lblCopy.ClientID %>");
            //Print Agent Copy.
            lbl.innerHTML = "(SpiceJet Copy)";
            window.print();

            //Print JET Copy.
            lbl.innerHTML = "(Agent Copy)";            
            window.print();

            window.close();

         }

         function closePage() {
             window.close();
         }
     
</script>
     <body style="font-family:Tahoma">
     <form id="form1" runat="server">
    <div style="width:900px">
    <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
    </p>
     
    <table width="100%">
    <tr>
    <td style="width:60px">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Client_logo.png" 
            Height="60px" Width="220px" />
    </td>
    <td align="center" style="font-size:xx-large">
    Delivery Order
    </td>
    <td>
    <table width="100%">
   
    <tr>
    <td style="width:60px">
    Date:</td>
    <td>
        <asp:Label ID="lbldate" runat="server" ForeColor="#3366FF"></asp:Label>
    </td>
    </tr>
    <tr>
    <td>
     Staff Id:
    </td>
    <td>
        <asp:Label ID="lblstaffid" runat="server" ForeColor="#3366FF"></asp:Label>
    </td>
    </tr>
    <tr>
    <td>
     Station:</td>
    <td>
        <asp:Label ID="lblstation" runat="server" ForeColor="#3366FF"></asp:Label>
    </td>
    </tr>
    </table> 
    </td> 
    </tr>
    </table> 
    <hr />
    <table width="100%">
    <tr>
    <%--<td width="100px" >
     Awb Number
    </td>
    <td>
        <asp:Label  ID="lblawbid" runat="server" Width="80px" ></asp:Label>
    </td>--%>
    <%--<td width="110px">
     Flight Number
    </td>
    <td width="110px" >
        <asp:Label ID="lblflightid" runat="server" Width="100px" ></asp:Label>
    </td>--%>
    <td width="110px">
     Agent Name
    </td>
    <td >
        <asp:Label ID="lblagentname" runat="server" Width="500px" ></asp:Label>
    </td>
    <%--<td >
    Flight Date 
    </td>
    <td>
        <asp:Label ID="lblflightdatetime" runat="server" Width="100px" style="margin-left: 0px" ></asp:Label>
    </td>--%>
    </tr>
    </table>
    <div>
    </div>
    <div>
    Delivery Details
    </div>
    <div>
    </div> 
    <table cellspacing="6px" cellpadding="1px">
    <tr>
    <td style="border-collapse: collapse; border-spacing: 0px;">
    DO No:</td>
    <td style="border-width: 1px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
        <asp:Label ID="lbldnno" runat="server" Font-Underline="False" 
            ForeColor="#3366FF"></asp:Label>
    </td>
    <%--<td align="left" >
        AWB No:</td>
    <td style="border-width: 1px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
        <asp:Label ID="lblawbno" runat="server" ForeColor="#3366FF"></asp:Label>
    </td>--%>
    <%--<td style="width:60px">
    DN Date
    </td>
    <td style="width:60px">
        <asp:Label ID="lbldndate" runat="server"></asp:Label>
    </td>--%>
    <td style="width:60px"  >
       IGM No:
      </td>
     <td style="border-width: 1px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
         <asp:Label ID="lbligmno" runat="server" ForeColor="#3366FF"></asp:Label>
     </td>
    
    <td>
    HAWB No:    
    </td>
    <td style="border-width: 1px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
        <asp:Label ID="lblhawbno" runat="server" ForeColor="#3366FF"></asp:Label>
    </td>
    </tr>
    
    <tr>
    <td>
    No Of HAWBs:
    </td>
    <td style="border-width: 1px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
        <asp:Label ID="lblnohawb" runat="server" ForeColor="#3366FF"></asp:Label>
    </td>
    <td>
    CC/PP:
    </td>
    <td style="border-width: 1px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
        <asp:Label ID="lblcc" runat="server" ForeColor="#3366FF"></asp:Label>
    </td>
    
   
    <%--</table>
    <table width="100%">--%>
    
    <td>
    Payment Mode:</td>
    <td style="border-width: 1px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
        <asp:Label ID="lblpaymentmode" runat="server" ForeColor="#3366FF"></asp:Label>
    </td>
     </tr>
    <tr>
    <td>
    Handling Code:</td>
    <td style="border-width: 1px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
        <asp:Label ID="lblhandling" runat="server" ForeColor="#3366FF"></asp:Label>
    </td>
    <td>
    Addl. Handling Code:</td>
    <td style="border-width: 1px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
        <asp:Label ID="lbladdlhandling" runat="server" ForeColor="#3366FF"></asp:Label>
    </td>
    </tr>
    
   <%-- <tr>
    <td style="width:110px">
      DO Pieces:</td>
     <td style="border-width: 1px; width:110px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;" >
         <asp:Label ID="lbldnpieces" runat="server" ForeColor="#3366FF"></asp:Label>
     </td>
     <td style="width:110px">
     DO Weight:</td>
     <td style="border-width: 1px; width:110px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
         <asp:Label ID="lbldnweight" runat="server" ForeColor="#3366FF"></asp:Label>
     </td>
     
    </tr>--%>
    </table>  
   <%-- <table width="100%">
    <tr>
     <td style="width:13%">
      DN Pieces
     </td>
     <td style="width:20%" >
         <asp:Label ID="lbldnpieces" runat="server"></asp:Label>
     </td>
     <td style="width:16%">
     DN Weight
     </td>
     <td style="width:16%">
         <asp:Label ID="lbldnweight" runat="server"></asp:Label>
     </td>
     <td style="width:16%">
       IGM No
     </td>
     <td style="width:60px">
         <asp:Label ID="lbligmno" runat="server"></asp:Label>
     </td>
    </tr>
    </table> --%>
    <table>
    <tr>
    <%--<td>
     Flight No:</td>
    <td style="border-width: 1px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
        <asp:Label ID="lblflightno" runat="server" ForeColor="#3366FF"></asp:Label>
    </td>
    <td>
    Flight Date
    </td>
    <td style="border-width: 1px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
        <asp:Label ID="lblflightdate" runat="server" ForeColor="#3366FF"></asp:Label>
    </td>--%>
    <td>
     Service Tax
    </td>
    <td style="border-width: 1px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
        <asp:TextBox ID="txtservicetax" runat="server" Width="110px" 
            ForeColor="#3366FF"></asp:TextBox>
    </td>
    <td style="width:40px" rowspan="2" colspan="4">
    <td>
    <table style="padding: inherit; margin: auto; border: thin solid #000000; width:200px; table-layout: auto; border-collapse: collapse; border-spacing: 0px;">
    <tr>
    <td style="border: thin solid #000000; width:110px">
    Charge Head
    </td>
    <td style="border: thin solid #000000; width:90px">
        Charge
    </td>
    </tr>
    <tr>
    <td style="border: thin solid #000000; width:110px">
        <asp:TextBox ID="txtchargehead" runat="server" Width="100px" 
            ForeColor="#3366FF" ></asp:TextBox>
    </td>
    <td style="border: thin solid #000000; width:90px">
        <asp:TextBox ID="txtcharge" runat="server" Width="70px" ForeColor="#3366FF"></asp:TextBox>
   
    </td>
    </tr>
    </table> 
    </td>
    </tr>
    
    <tr>
    <td style="width:110px">
        &nbsp;</td>
    <td style="width:110px">
        &nbsp;</td>
    <td style="width:110px">
        &nbsp;</td>
    <td style="width:110px">
        &nbsp;</td>
    <td style="width:110px">
        &nbsp;</td>
    <td style="width:110px">
        &nbsp;</td>
    </tr>
    
    </table>
    <div >
  <table width ="100%">
    <tr>
    <td>
    
     <asp:GridView ID="grdeliveryOrder" runat="server" AutoGenerateColumns="False" 
            ShowFooter="True"   Width="100%" Height="4px">
            <Columns>
                                            
                <asp:TemplateField HeaderText="AWB Number" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblconsigneeNo" runat="server" Width="40px" Text='<%# Eval("AWBNumber") %>' ></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="No Of Pkgs." HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblnopkgs" runat="server" Text='<%# Eval("ActualPieces") %>' Width="40px"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="G Weight" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblweight" runat="server" Width="40px" Text='<%# Eval("ActualWeight") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Flight Number" HeaderStyle-Wrap="true" >
                    <ItemTemplate>
                        <asp:Label ID="lblfltno" runat="server" Width="40px" Text='<%# Eval("FlightNumber") %>'  ></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Flight Date" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblfltdate" runat="server" Width="70px" Text='<%# Eval("IssueDate") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Discription" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lbldiscription" runat="server" Width="70px" Text='<%# Eval("Discription") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                </Columns> 
                </asp:GridView>

    </td>
    </tr> 
    </table> 
    </div>  
    <table>
  
     <tr>
      <td style="width:110px">
       Issued To:</td>
      <td style="border-width: 1px; width:110px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
      <asp:Label ID="lblissuedto" runat="server" ForeColor="#3366FF"></asp:Label>
      </td>
      <td style="width:110px">
      Issue Name:</td>
      <td style="border-width: 1px; width:110px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
          <asp:Label ID="lblissuename" runat="server" ForeColor="#3366FF"></asp:Label>
      </td>
      <td style="width:110px">
      TDS:</td>
      <td style="border-width: 1px; width:110px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
          <asp:Label ID="txttds" runat="server" ForeColor="#3366FF"></asp:Label>
      </td>
      
     </tr>
     <tr>
     <td style="width:110px">
     Reciver's Name:</td>
     <td style="border-width: 1px; width:110px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
         <asp:Label ID="lblreciversname" runat="server" ForeColor="#3366FF"></asp:Label>
     </td>
     <td style="width:110px">
     ADHOC Cust:
     </td>
     <td style="border-width: 1px; width:110px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
         <asp:Label ID="lbladhoc" runat="server" ForeColor="#3366FF"></asp:Label>
     </td>
     <td style="width:110px">
     &nbsp;Total Pieces:</td>
     <td style="border-width: 1px; width:110px; border-bottom-style: solid; border-collapse: collapse; border-spacing: 0px;">
         <asp:Label ID="lblgrandtotal" runat="server" ForeColor="#3366FF"></asp:Label>
     </td>
     </tr>      
     </table>
     <table width="100%">
     <tr>
     <td style="width:13%"  >
      Shipment Description:</td>
     <td style="width:60px">
         <asp:TextBox ID="TextBox1" runat="server" Width="376px" ForeColor="#3366FF"></asp:TextBox>
     </td>
     </tr>
     </table> 
     <table width="100%">
     <tr>
     <td style="width:13%"> 
     Remarks:</td>
     <td style="width:70px">
         <asp:TextBox ID="txtremarks" runat="server" Width="760px" 
             ForeColor="#3366FF"></asp:TextBox>
     </td>
     
     </tr>
     <tr>
     <td style="width:13%">
       Signature:</td>
     <td style="width:70px">    
         <asp:Image ID="Image2" runat="server" Height="92px" Width="248px" />
     </td>
     </tr>
     </table> 
    <table width="100%">
   <tr>
   <td style="width:auto" align="center" valign="top">
    <asp:Button ID="btnprint" runat="server" Text="Print" CssClass="button" 
           onclick="btnprint_Click" style="height: 26px" />
       <span lang="en-us">&nbsp;</span><asp:Button ID="btnnext" runat="server" Text="Next" CssClass="button" 
           onclick="btnnext_Click" style="height: 26px" />      
       <span lang="en-us">&nbsp;</span>
   <asp:Button ID="btnClear" runat="server" Text="Close" CssClass="button" 
           OnClientClick="javascript:closePage()" />
                           
   
       <asp:Label ID="lblCopy" runat="server"></asp:Label>
                           
   
   </td>
   </tr>
   </table>
     </div>  
    </form>
</body>
</html>
