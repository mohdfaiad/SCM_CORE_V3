<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnauthorizedAccess.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.UnauthorizedAccess" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
      
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
    

    
   <div style="width:450px; margin:50px auto; border:3px double #0B8FD7; box-shadow: 10px 10px 5px #999; padding:20px;">
  
       
<table><tr><td valign="top"><img src="Images/!.png" style="vertical-align:middle; height:25px;" /></td><td> <div>
    You are not authorized to access this functionality. <br /><br /><asp:Button ID="btnClose" 
        runat="server" Text="Close" CssClass="button" onclick="btnClose_Click" /></div></td></tr>
    <tr>
    <td> </td>
    </tr>
    </table></div>
 </asp:Content>
