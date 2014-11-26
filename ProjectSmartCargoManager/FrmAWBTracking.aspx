<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAWBTracking.aspx.cs"  Inherits="ProjectSmartCargoManager.FrmAWBTracking" %>


 
 
 <%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">--%>
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
 
<form id="FrmAWBTracking" runat="server" style="width:1024px;height:200px" >
 
 <%-- <div id="contentarea">
--%> 
  
  
    <script language="javascript" type="text/javascript">
        function NoRecFound() 
        {
            alert("No Records found ");
        }
        
   </script>

<div id="contentarea">
    <h1>
        <img alt="" src="Images/txttrackAWB.png"  style="vertical-align:5"/> 
    </h1>
    
    <div class="botline">
    <asp:Label ID="LabelStatus"  runat="server" Font-Bold="true" Font-Size="Medium" ></asp:Label>
               <table style="width:47%;">
                    <tr>
                        <td valign="top">
                Prefix</td>
                        <td valign="top">
                            <asp:TextBox ID="txtPrefix"  runat="server" CssClass="inputbg60" MaxLength="4" 
                    Width="50px" ToolTip="Enter AWB Prefix" Height="22px"></asp:TextBox>
                        </td>
                        <td></td><td></td>
                        <td valign="top">
            AWB No's
            </td>
                        <td valign="top">
                        <asp:TextBox ID="TextBoxAWBno" runat="server" 
                            ontextchanged="TextBoxAWBno_TextChanged" Height="45px" 
                            ToolTip="Enter AWB no(S). If multiple AWBs, enter comma separated. eg:98989898,98989899" 
                            Width="250px" TextMode="MultiLine" MaxLength="179" ></asp:TextBox>
                        </td>
                        <td valign="top">
                        <asp:Button ID="ButtonGO" runat="server" Text="Track" Width ="60px" CssClass="button"
                            onclick="ButtonGO_Click" />
                        </td>
                    </tr>
                    
                </table>
                
        </div>
        <div style="float:left; margin-top:5px;width:100%;" >
        
    
        <asp:Panel ID="pnlShowData" runat="server" Visible="false" Width="100%">
       
       <table width="100%">  
       <tr>
       <td>
       <asp:Label id="lblAWBNo" runat="server" Text="" Font-Bold="true" ForeColor="Brown" Font-Size="Larger" >
       </asp:Label>
       </td>
       </tr>    
         <tr>
           <td>
              <asp:Image ID="ImageBooking" runat="server" Height="103px" Width="140px"  />
              <asp:Image ID="ImageAcceptance" runat="server" Height="103px" Width="140px"  />
               <asp:Image ID="ImageManifested" runat="server" Height="103px" Width="140px"  />              
              <asp:Image ID="ImageDeparted" runat="server" Height="103px" Width="140px"  />   
              <asp:Image ID="ImageArrival" runat="server" Height="103px" Width="140px"  />               
              <asp:Image ID="ImagePartialDelivery" runat="server" Height="103px" Width="140px"  />
              <asp:Image ID="ImageDelivered" runat="server" Height="103px" Width="140px" />
             
           </td>
         </tr>
       </table>
      <br />
      <div>
         <%--CssClass="style1" --%>
            <asp:Label ID="lblSelectRows" runat="server" Text="Please click on AWB row to view its status." Font-Size="Medium" ForeColor="Blue">
            </asp:Label>
              <asp:GridView ID="GridViewAwbTracking" runat="server"  Width="100%" PageSize="300"
                AutoGenerateColumns="False"   AllowPaging="False" AllowSorting="False"  
                DataKeyNames ="AWBnumber"  onpageindexchanging="GridViewAwbTracking_PageIndexChanging" 
                  onselectedindexchanged="GridViewAwbTracking_SelectedIndexChanged" 
                  onselectedindexchanging="GridViewAwbTracking_SelectedIndexChanging" 
                   >
              <Columns>
                  
                 <asp:BoundField HeaderText="AWB No." DataField="AWBNumber" ItemStyle-ForeColor="Blue" 
                    ItemStyle-BorderColor="Black" />
                  <asp:BoundField DataField="milestone" HeaderText="Milestone" >
                  <ItemStyle HorizontalAlign="Center" />
                  </asp:BoundField>
                  <asp:BoundField DataField="pieces" HeaderText="Pieces" />
                  <asp:BoundField DataField="Piecesweight" HeaderText="Weight" />
                  <asp:BoundField DataField="flightno" HeaderText="Flight No" >
                  <ItemStyle HorizontalAlign="Center" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Createdon" HeaderText="Flight Date" >
                  <ItemStyle HorizontalAlign="Center" />
                  </asp:BoundField>

                  <asp:BoundField DataField="Origin" HeaderText="Origin" >
                  <ItemStyle HorizontalAlign="Center" />
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="Destination" HeaderText="Destination" >
                  <ItemStyle HorizontalAlign="Center" />
                  </asp:BoundField>                  
                 
              </Columns>
              
               <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <FooterStyle CssClass="grdrowfont"></FooterStyle>
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
                <RowStyle HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="trcolor">
        
                 </AlternatingRowStyle>
               </asp:GridView> 

          <asp:Button ID="BtnPrinDo" runat="server" Text="PrintDO" Width ="60px" 
               CssClass="button" onclick="BtnPrinDo_Click" Visible="false"/>
      
              <asp:Label ID="lblPrintDO" Text="Print DO: " Font-Bold="true" runat="server"></asp:Label>
               <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDONumber" runat="server" Text='<%# Eval("DONumber") %>' CommandArgument='<%# Eval("DONumber") %>' CommandName="Print"/>
                            </ItemTemplate>
                            <SeparatorTemplate>,</SeparatorTemplate>
                        </asp:Repeater>
       </div>
       </asp:Panel>
       </div>
    </div>
    
      <div style=" margin-top:5px;">
        <%--<asp:Panel ID="pnlShowData" runat="server" >
       --%>
      <br />
       <table width="100%" >
         
       <tr>        
          <td> 
              <asp:GridView ID="GrdFSATracking" runat="server"   Width="100%"
                AutoGenerateColumns="False"   AllowPaging="false" AllowSorting="True" Visible="true" >
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
              <asp:TemplateField HeaderText="System Date">
              <ItemTemplate>
              <asp:Label ID="lblSysDt" runat="server" Text='<%# Eval("SystemDate")%>'></asp:Label>
              </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="System Time">
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
<%--</div> 
--%>
</form>
</body>
</html>


  <%--</asp:Content> --%>