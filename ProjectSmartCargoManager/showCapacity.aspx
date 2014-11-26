<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="showCapacity.aspx.cs" Inherits="ProjectSmartCargoManager.showCapacity" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.tabpan
{
    float:left;
    
    }
    
    .tableRight
{
   
    margin-left:20px;
    padding-left:150px;
    vertical-align:top;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />
<br />
<br />

     <br />
        <asp:Label ID="LBLStatus" runat="server" ForeColor="Red"></asp:Label>
        <br />
        <div style="text-align:center;">
     <b>&nbsp;&nbsp;&nbsp;&nbsp; Booked</b>&nbsp;&nbsp;&nbsp; &nbsp;
     <img alt="Booked" src="Images/sBook.jpg" /> <b>&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; Accepted</b>
&nbsp;&nbsp;&nbsp; &nbsp;
     <img alt="Accepted" src="Images/Check.jpg" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; <b> Empty
</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
     <img alt="Empty" src="Images/sEpty.jpg" />
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
     </div>
     <br />
    <div  id="divrun"  style="float:left;width:100%;">
       
      
        
       
      
    </div>
     <asp:TabContainer ID="tbMaincon" runat="server" ActiveTabIndex="0" 
                        BorderColor="#F0F0F0" BorderStyle="None" BorderWidth="1px" Height="400px" 
                        style="width:100%;" Width="610px" ScrollBars="Auto" >
        </asp:TabContainer>
</asp:Content>
    

