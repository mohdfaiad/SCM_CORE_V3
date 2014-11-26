<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionTimeOut.aspx.cs" Inherits="ProjectSmartCargoManager.SessionTimeOut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style>
    body {
margin: 0px;
padding: 0px;
background-color: #FFFFFF;
font-family: Calibri, Arial, Helvetica, sans-serif;
font-size: 18px;
color: #4e4e4e;
 }
    
    </style>
</head>
<body>
    <form id="form1" runat="server">
   <div style="width:450px; margin:50px auto; border:3px double #0B8FD7; box-shadow: 10px 10px 5px #999; padding:20px;">
  
       
<table><tr><td valign="top"><img src="Images/!.png" style="vertical-align:middle; height:25px;" /></td><td> <div>Your session has expired. <br />To access your account, Please
        <asp:HyperLink ID="hlnLogin" runat="server" NavigateUrl="~/Login.aspx">Login</asp:HyperLink>
        &nbsp;again.</div></td></tr></table></div>
    </form>
</body>
</html>
