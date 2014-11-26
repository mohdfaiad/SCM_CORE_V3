<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SCM_Dynamic.aspx.cs"  Inherits="ProjectSmartCargoManager.SCM_Dynamic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
     <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>SmartKargo-Index</title>
<link href="style/visiblec.css" rel="stylesheet" type="text/css" media="screen" />
<link type="text/css" href="style/menu.css" rel="stylesheet" />

  
    
     <style type="text/css">
         #form1
         {
             margin-bottom: 19px;
         }
     </style>
     
     <script type="text/javascript">

     function openNewTab(url) {
        window.open(url, 'Contact', '');
     }
     
     </script>
</head>
<body>


<table width="83%">

<tr>
<td style="width:200px"> &nbsp;</td>
<td><img src="Images/Client_logo.png" alt="Smart Cargo" title="Smart Kargo" border="0" class="marginTop30" align="left" /> <a href="#"><img src="Images/Client_logo1.png" alt="Smart Cargo" title="Smart cargo" border="0" class="marginTop30" align="right" /></a></td>
</tr>
</table>
<br />

<div id="menu">
    <ul class="menu">
      <li class="selected"><a href="Home.aspx"><img src="images/home.png" alt="Home" title="Home" border="0" />Home</a></li>
     <%-- <li><a href="http://www.qidtech.com/?page_id=719"><img src="images/about.png" alt="About VC" title="About VC" border="0" />About 
          VC</a></li>--%>
      <li>
          <asp:HyperLink ID="hlnkContactUs" Visible="true" runat="server" NavigateUrl="http://www.qidtech.com/?page_id=84" 
           OnClick="javascript:openNewTab(this.href);return(false);" Width="70px" >
           <img src="images/contacts.png" alt="Contacts" title="Contacts" border="0" /> Contact 
           </asp:HyperLink>
      </li>
    </ul>
</div>
<div class="clearBoth"></div>


    <form id="form1" runat="server">

      <div class="container">

    <%--<div class="grayBox" style="margin:30px auto; float:none;">--%>
    <%--<div class="grayBoxMiddle" style="height:182px"  >--%>
      
        <table border="0">
          <tr>
            <td colspan="2"><h4 style="color:#000000; font-weight:bold;">Dynamic Diagnostics</h4></td>
          </tr>
          
          <tr>
            <td valign="top">
                <a href="#" style=" background:url('images/buton.png') repeat-x; border-radius:6px; color:#FFFFFF; font-weight:bold;font-family:Calibri, Arial, Helvetica, sans-serif;
                font-size:13px;  padding:7px;">
                <asp:Button ID="btnTest" runat="server" Text="Test" Font-Names="Verdana" Font-Size="Small" OnClick="btnTest_Click" ForeColor="#ffffff" /></a>
                <br />
                <%--</td><td valign="top">
                    <asp:TextBox ID="txtResult"  runat="server" MaxLength="200" ReadOnly="true" TextMode="MultiLine" BorderStyle="None" Height="60px" Width="180px"></asp:TextBox>
              </td>--%>
          </tr>
          <tr>
              <td>
                  <asp:TextBox ID="txtResult"  runat="server" MaxLength="200" ReadOnly="true" TextMode="MultiLine" BorderStyle="None" Height="60px" Width="180px"></asp:TextBox>
              </td>
          </tr>          
         
          
      </table>
      
    <%--</div>--%>
    <%--<div class="grayBoxBot"></div>
    

    <div class="clearBoth">
    </div>--%>
    <%--</div>--%>
    </div>

    </form>
    <div class="footer">Rights reserved @ Quantum ID Technologies 2014.</div>
    
</body>
</html>





    
    
    
    

 


 

    
