<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmChatTest.aspx.cs" Inherits="ProjectSmartCargoManager.frmChatTest" %>

<%@ Register Assembly="DG.Square.ASP.NET.Ajax.ChatControl" Namespace="DG.Square.ASP.NET.Ajax.ChatControl"
    TagPrefix="ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CHATROOM</title>
</head>
<body background="Images/chat.png" >
   <div  style="background:Images/chat.png scroll bottom center;" >
    <form id="form1" runat="server">
    
    <div>
     
    
     <ajax:ChatBox ID="ChatBox1" runat="server" />
     <asp:ImageButton ID="btnLogout" ImageUrl="Images/logout.png" ImageAlign="AbsMiddle"
                                runat="server" OnClick="Logout" OnClientClick="return confirm('Do you want to Logout ?')"
                                Text="Sure to Logout." />
    </div>
    </form></div>
</body>
</html>
