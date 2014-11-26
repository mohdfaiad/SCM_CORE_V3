<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="ProjectSmartCargoManager.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html> 
<head> 
<script language="javascript">
    function printdiv(printpage) {        
        var headstr = "<html><head><title></title></head><body>";
        var footstr = "</body>";
        var newstr = document.all.item(printpage).innerHTML;
        var oldstr = document.body.innerHTML;
        document.body.innerHTML = newstr;  //headstr + newstr + footstr;
        window.print();
        document.body.innerHTML = oldstr;
        return false;
    } 
</script> 
<title>div print</title> 
</head> 
 
<body> 
    <form id="form1" runat="server">
//HTML Page 
//Other content you wouldn't like to print 
<input name="b_print" type="button" class="ipt"   onClick="printdiv('div_print');" value=" Print "> 
 
<div id="div_print"> 
 
<h1 style="Color:Red">The Div content which you want to print</h1> 
 
</div> 
//Other content you wouldn't like to print 
//Other content you wouldn't like to print<br />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Date" />
    <br />
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </form>
</body> 
 
</html>
