<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAWBTrackFromLogin.aspx.cs" Inherits="ProjectSmartCargoManager.FrmAWBTrackFromLogin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

    <style type="text/css">

     
        .style1
        {
            width: 30%;
        }

     
        </style>
    <script language="javascript" type="text/javascript">

        function Download(count) {
            window.open('Download.aspx?Mode=D', 'Download', 'left=100,top=100,width=800,height=420,toolbar=0,resizable=1');
        }

    </script>
 <%--</asp:Content> --%>

 
<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
--%> 
<html xmlns="http://www.w3.org/1999/xhtml" >

 <head id="Head1" runat="server">
    <title></title>
      
    <script type="text/javascript">

        function DoneClick(button) {

           
            window.close();
            

        }
    </script>
</head>
 <link href="style/style.css" rel="stylesheet" type="text/css" />

<body style="background:#fff;">
 
<form id="FrmAWBTrackFromLogin" runat="server" style="width:1024px;height:200px" >
<div id="contentarea">
     <h1>
            C2K Tracking</h1>
    
    <div class="botline">
       <table width="100%" >
       
          <tr>
          <td>
        <h3> Enter AWB Details (Prefix and AWB No)</h3> 
          
          </td>
          </tr> 
       
       <tr >
       <td valign="top" >
               <table style="width:40%;">
                    <tr>
                        <td valign="top">
                Prefix</td>
                        <td valign="top">
                            <asp:TextBox ID="txtPrefix"  runat="server" CssClass="inputbg60" MaxLength="4"  
                    Width="50px" Height="22px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="txtPrefix" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td valign="top">
            AWB No:</td>
                        <td valign="top">
                            <asp:TextBox ID="TextBoxAWBno" runat="server" 
                 Height="22px" 
                    
                    Width="140px"  MaxLength="12" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ControlToValidate="TextBoxAWBno" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
            <asp:Button ID="ButtonGO" runat="server" Text="Track" Width ="60px" CssClass="button"
                onclick="ButtonGO_Click" /> 
                        </td>
                    </tr>
                </table>
              </td> 
       
       </tr>     
           
       
       <tr >
        <td  >
            <asp:Label ID="LabelStatus"  runat="server" ForeColor="#FF3300"   Font-Bold="True" 
            Font-Size="Large" ></asp:Label>
           </td>        
       </tr>     
           
       
       </table>
        </div>
        <div style=" margin-top:5px;">
        <%--<asp:Panel ID="pnlShowData" runat="server" >
       --%>
      <br />
       <table width="100%" >
         
       <tr>        
          <td> 
              <asp:GridView ID="GridViewAwbTracking" runat="server"   Width="100%"
         AutoGenerateColumns="False"   AllowPaging="false" AllowSorting="True"  >
              <Columns>
              <asp:TemplateField HeaderText="Message">
              <ItemTemplate>
              <asp:Label ID="lblMessage" runat="server" Text='<%# Eval("Message") %>'></asp:Label>
              </ItemTemplate>
              </asp:TemplateField>
                   <asp:TemplateField HeaderText="Message Date (dd/MM/yyyy)">
              <ItemTemplate>
              <asp:Label ID="lblMessageDate" runat="server" Text='<%# Eval("MessageDate") %>'></asp:Label>
              </ItemTemplate>
              </asp:TemplateField>
                <asp:TemplateField HeaderText="Message Time">
              <ItemTemplate>
              <asp:Label ID="lblMessageTime" runat="server" Text='<%# Eval("MessageTime") %>'></asp:Label>
              </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="System Date" Visible="false">
              <ItemTemplate>
              <asp:Label ID="lblSysDt" runat="server" Text='<%# Eval("SystemDate")%>'></asp:Label>
              </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="System Time" Visible="false">
              <ItemTemplate>
              <asp:Label ID="lblSysTime" runat="server" Text='<%# Eval("SystemTime") %>'></asp:Label>
              </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="Station Code">
              <ItemTemplate>
              <asp:Label ID="lblStnCode" runat="server" Text='<%# Eval("StnCode") %>'></asp:Label>
              </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="Pieces">
              <ItemTemplate>
              <asp:Label ID="lblPcs" runat="server" Text='<%# Eval("Pcs") %>'></asp:Label>
              </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="Weight">
              <ItemTemplate>
              <asp:Label ID="lblWgt" runat="server" Text='<%# Eval("Wght") %>'></asp:Label>
              </ItemTemplate>
              </asp:TemplateField>
                <asp:TemplateField HeaderText="Details">
              <ItemTemplate>
              <asp:Label ID="lblDetails" runat="server" Text='<%# Eval("Details") %>'></asp:Label>
              </ItemTemplate>
              </asp:TemplateField>
              
              </Columns>
              
               <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <FooterStyle CssClass="grdrowfont"></FooterStyle>
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
                <RowStyle HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="trcolor">
        
                 </AlternatingRowStyle>
               </asp:GridView> 
           </td>                
       </tr>
      
       </table>
      <%-- </asp:Panel>
--%>
       </div>
      
   
    </div>
</form>
</asp:Content>

