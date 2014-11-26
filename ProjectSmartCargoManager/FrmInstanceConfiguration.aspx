<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmInstanceConfiguration.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.FrmInstanceConfiguration" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 </asp:Content>
 
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
 <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  
 <%-- <style type="text/css">
        .black_overlay
        {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 1000px;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: 0.8;
            filter: alpha(opacity=80);
        }
        .white_content
        {
            margin: 0 auto;
            display: none;
            position: absolute;
            top: 30%;
            left: 35%;
            width: 30%;
            height: 45%;
            padding: 16px;
            border: 16px solid #ccdce3;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
        .black_overlaymsg
        {
            display: none;
            position: fixed;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: White;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }
        .white_contentmsg
        {
            display: none;
            position: fixed;
            top: 45%;
            left: 45%;
            width: 5%;
            height: 5%;
            padding: 16px;
            background-color: Transparent;
            z-index: 1002;
        }
    </style>--%>
   
<div id="contentarea">
   
   <div class="msg">
   <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
   </div>
                
    <h1> Instance Configurations</h1>
   
   
   
   <div class="botline">
   <asp:Panel ID="pnlNew" runat="server">
   
   <table width="80%" border="0" cellpadding="3" cellspacing="3">
   <tr>
    <td>
        <asp:Label ID="LblCntId" runat="server" Text="Client Id"></asp:Label>
    </td>
    
    <td>
        <asp:TextBox ID="TxtCntId" runat="server"></asp:TextBox>
    </td>
   </tr>
   
    <tr>
      <td><asp:Label ID="LblClntNm" runat="server" Text="Client Name"></asp:Label></td>
      <td><asp:TextBox ID="TxtClntNm" runat="server"></asp:TextBox></td>
      <td><asp:Label ID="LblAddr" runat="server" Text="Client Address"></asp:Label></td>
      <td><asp:TextBox ID="TxtAddr" runat="server" TextMode="MultiLine"></asp:TextBox></td>
    </tr>
    
    <tr>
      <td><asp:Label ID="LblEmail" runat="server" Text="Email ID"></asp:Label></td>
      <td><asp:TextBox ID="TxtEmail" runat="server"></asp:TextBox></td>
      <td><asp:Label ID="LblPhone" runat="server" Text="Phone No"></asp:Label></td>
      <td><asp:TextBox ID="TxtPhNo" runat="server"></asp:TextBox></td>
    </tr>
    
    
    <tr>
      <td><asp:Label ID="LblMblNo" runat="server" Text="Mobile No"></asp:Label></td>
      <td><asp:TextBox ID="TxtMblNo" runat="server"></asp:TextBox></td>
      <td><asp:Label ID="LblFax" runat="server" Text="Fax No"></asp:Label></td>
      <td><asp:TextBox ID="TxtFax" runat="server"></asp:TextBox></td>
    </tr>
    
    
    <tr>
      <td><asp:Label ID="LblRegOffAddr" runat="server" Text="Reg Office Address"></asp:Label></td>
      <td><asp:TextBox ID="TxtRegOffAddr" runat="server"></asp:TextBox></td>
      <td><asp:Label ID="LblRegoffPh" runat="server" Text="Reg Office Phone"></asp:Label></td>
      <td><asp:TextBox ID="TxtRegoffPh" runat="server"></asp:TextBox></td>
    </tr>
    
    <tr>
      <td><asp:Label ID="LblCntLogo" runat="server" Text="Client Logo"></asp:Label></td>
      <td><asp:FileUpload ID="FileUploadCntLogo" runat="server" Width="170px" /> &nbsp;
          <asp:Button ID="BtnCntLogo" runat="server" Text="Upload" CssClass="button"/></td>
          
          <td><asp:Label ID="LblURL" runat="server" Text="Client URL"></asp:Label></td>
      <td><asp:TextBox ID="TxtURL" runat="server"></asp:TextBox></td>
      
      
    </tr>
    
    <tr>
      <td><asp:Label ID="LblRptLogo" runat="server" Text="Client Report Logo"></asp:Label></td>
      <td><asp:FileUpload ID="FileUploadRptLogo" runat="server" Width="170px" />&nbsp;
          <asp:Button ID="BtnRptLogo" runat="server" Text="Upload" CssClass="button"/>
      </td>
      
      <td><asp:Label ID="LblSupportEmail" runat="server" Text="Support Email"></asp:Label></td>
      <td><asp:TextBox ID="TxtSuprtEmail" runat="server"></asp:TextBox></td>
      
    </tr>
    
    <tr>
    <td><asp:Label ID="LblHeaderLogo" runat="server" Text="Client Header Logo"></asp:Label></td>
      <td><asp:FileUpload ID="FileUploadHeaderLogo" runat="server"  Width="170px"/>&nbsp;
          <asp:Button ID="BtnHeaderLogo" runat="server" Text="Upload" CssClass="button"/>
      </td>
      
      <td><asp:Label ID="LblSupportPhone" runat="server" Text="Support Phone"></asp:Label></td>
      <td><asp:TextBox ID="TxtSuprtPh" runat="server"></asp:TextBox></td>
    </tr>
    
       <tr>
       <td>
           <asp:Button ID="BtnSave" runat="server" Text="Save"  CssClass="button" 
               onclick="BtnSave_Click"/>  &nbsp;
               
           <asp:Button ID="BtnClear" runat="server" Text="Clear" CssClass="button" 
               onclick="BtnClear_Click" />
       </td>
       </tr>
     
   </table>
   
   
   </asp:Panel>
   </div>
   
   </div>
 
 </asp:Content>