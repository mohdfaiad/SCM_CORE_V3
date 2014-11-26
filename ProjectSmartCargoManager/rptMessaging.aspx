<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptMessaging.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.rptMessaging" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>

 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <style>
.black_overlaynew
		{
			display: none;
			position: absolute;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 1000px;
			background-color: black;
			z-index:1001;
			-moz-opacity:0.8;
			opacity:0.4;
			filter:alpha(opacity=80);
		}
	.white_contentnew 
		{
		    margin:0 auto;
			display: none;
			position:fixed;
			top: 15%;
			left: 30%;
			height: 70%;
			padding: 16px;
			border: 16px solid #ccdce3;
			background-color: white;
			z-index:1002;
		
            
		}
		.black_overlaymsg{
			display: none;
			position: fixed;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 100%;
			background-color: White;
			z-index:1001;
			-moz-opacity: 0.8;
			opacity:.80;
			filter: alpha(opacity=80);
		}
		.white_contentmsg {
			display: none;
			position: fixed;
			top: 45%;
			left: 45%;
			width: 5%;
			height: 5%;
			padding: 16px;
			background-color: Transparent;
			z-index:1002;
			
		}
		
         .style5
         {
             width: 96px;
         }
         .style6
         {
             width: 145px;
         }
         .style7
         {
             width: 80px;
         }
         .style8
         {
             width: 146px;
         }
         .style9
         {
             width: 88px;
         }
		
     </style>
<script type ="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);
    
    function callShow() {
        document.getElementById('msglight').style.display = 'block';
        document.getElementById('msgfade').style.display = 'block';
        //document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

    }
    function callclose() {
        document.getElementById('msglight').style.display = 'none';
        document.getElementById('msgfade').style.display = 'none';
    }


function ViewPanelSplit() {
    document.getElementById('Lightsplit').style.display = 'block';
    document.getElementById('fadesplit').style.display = 'block';
}
function HidePanelSplit() {
    document.getElementById('Lightsplit').style.display = 'none';
    document.getElementById('fadesplit').style.display = 'none';
}

function ShowMessage(msgtype, hlfSrNo) {

    //    window.open("MessagePopUp.aspx?Msgtype=" + msgtype + "&hlfSrNo=" + hlfSrNo, "", "left=" + (screen.availWidth / 2 - 100) + ",top=" + (screen.availHeight / 2 - 100) + ",width=400,height=600,toolbar=0,resizable=0");
    window.open("MessagePopUp.aspx?Msgtype=" + msgtype + "&hlfSrNo=" + hlfSrNo, "", "left=450,top=90,width=400,height=500,scrollbars=0,toolbar=0,resizabel=0")
}

</script>
     <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
    <%--<asp:UpdatePanel ID="updatepnl" runat="server">
                    <ContentTemplate>--%>
     <div id="contentarea">
   
    
    <h1>Message Console</h1>
    
     <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True"  
             ForeColor="Red"></asp:Label>
            <%--<img alt="" src="Images/flightshlist.png"  style="vertical-align:5"/> </h1>--%>
                <div class="botline">
           
                    <table cellpadding="3" cellspacing="3"><tr><td>
                    <asp:Image ID="img1" runat="server" ImageUrl="~/Images/email_inbox.jpg" />
                        <asp:RadioButton ID="Incoming" runat="server" Text="Incoming" Font-Bold="true" Font-Size="Medium"
                             GroupName="Message"/>
                    
                    
                    </td>
                        <td></td><td></td><td>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/email_outbox.jpg" />
                        <asp:RadioButton ID="Outgoing" runat="server" Text="Outgoing"  Font-Bold="True" Font-Size="Medium"
                             GroupName="Message"  />
                        
                    </td></tr>
                    
                    </table>
                    
                   
                    </div><br/><br/>  <br />
                   <div class=divback>
                 <table width="100%" cellpadding= "6" cellspacing="3">
                <tr>
                    <td class="style9">
                        Date From :</td>
                    <td class="style8">
              <asp:TextBox ID="txtFlightFromdate" runat="server"  Width="100px" 
                            ToolTip="Please enter date format: dd/MM/yyyy"></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightFromdate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightFromdate" PopupButtonID="imgFromDate">
              </asp:CalendarExtender>
              <asp:ImageButton ID="imgFromDate" runat ="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                    </td>
                    <td class="style7">
                        Date To :</td>
                    <td >
                      
              <asp:TextBox ID="txtFlightToDate" runat="server" Width="100px" 
                          ToolTip="Please enter valid date format: dd/MM/yyyy" 
                 ></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightToDate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightToDate" PopupButtonID="imgToDate">
              </asp:CalendarExtender>
              <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                    </td>
                    
                    <td class="style5">
                        Message Type :</td>
                    <td>
                        <asp:DropDownList ID="ddlmsgtype" runat="server">
                       <%--    <asp:ListItem>ALL</asp:ListItem>
                    <asp:ListItem>FFR</asp:ListItem>
                    <asp:ListItem>FFA</asp:ListItem>
                    <asp:ListItem>FFM</asp:ListItem>--%>
                        </asp:DropDownList>
                    </td>
                    
                    <td class="style6">Communication Type :
                        </td>
                    <td>
                    <asp:DropDownList ID="ddlcommMsgtype" runat="server">
                    <asp:ListItem>ALL</asp:ListItem>
                    <asp:ListItem>SITA</asp:ListItem>
                    <asp:ListItem>EMAIL</asp:ListItem>
                    </asp:DropDownList>
                    
                        </td>
             
                </tr>
                <tr>
                    <td class="style9">
                        Criteria :</td>
                    <td class="style8">
                        <asp:TextBox ID = "txtCriteria" runat = "server" Width="100px"></asp:TextBox>
                        </td>
                    <td class="style7">
                      
                        <asp:CheckBox ID="ChkProcessed" runat="server" Text="Processed" />
                    </td>
                    <td >
                      
                                            <asp:CheckBox ID="ChkFailed" runat="server" Text="Failed" />
                        
                    </td>
                    
                    <td class="style5">
                        <%--<asp:Button ID="btnReprocess1" runat="server" CssClass="button" 
                            onclick="btnReprocess_Click" Text="ReprocessMsg" Visible="false" />--%>
                    </td>
           
                </tr>
               
                
           </table>
           
             <table>
               <tr>
                  <td >
                        <asp:Button ID="btnList1" runat="server" CssClass="button" 
                            OnClick="btnList_Click" Text="List" /> &nbsp;&nbsp;
                    
                      <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" 
                            onclick="btnExport_Click"/>  &nbsp;&nbsp;
                    
                        <asp:Button ID="btnclear1" runat="server" CssClass="button" 
                            onclick="btnclear_Click" Text="Clear" />
                            
                      
                            
                    </td>
                </tr>
            </table>
            
            </div>
    
   
    
   
    
    <table width="100%">
        <tr>
        <td>
            
            <div style="margin-top:25px">
                                         <asp:Panel ID="Pnlgrd" runat="server" ScrollBars="Auto" Height="200px" 
                                             style="margin-top:20px"
                                             BorderStyle="Solid" BorderWidth="1px" Width="1000px">
                                             
                                             <asp:GridView ID="gdvMsg" runat="server" AutoGenerateColumns="False" 
                                                 CellPadding="3" CellSpacing="3" onrowcommand="gdvMsg_RowCommand"  >
                                             <Columns>
                                             
                                             
                                              <asp:TemplateField AccessibleHeaderText=" " HeaderText=" ">
                                             <ItemTemplate>
                                             <asp:Button ID="btnShow" runat="server" Text="Show" CommandName="ShowMessage" OnClick="ShowMsg_Click" visible="false" AutoPostBack="true"></asp:Button>
                                             </ItemTemplate>
                                             </asp:TemplateField>
                                              <asp:TemplateField AccessibleHeaderText=" " HeaderText=" " Visible="false">
                                              <ItemTemplate>
                                             <asp:Button ID="btnDelete" runat="server" Text="Show" OnClick="ShowMsg_Click" visible="false"></asp:Button>
                                             </ItemTemplate>
                                             </asp:TemplateField>
                                 
                                             <asp:TemplateField AccessibleHeaderText="Message Type" HeaderText="Message Type">
                                             <ItemTemplate>
                                             <asp:Label ID="lbltype" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
                                             </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField AccessibleHeaderText="Message" HeaderText="Message">
                                             <ItemTemplate>
                                             <asp:LinkButton ID="lnkMsg" runat="server" Text='<%#Eval("body").ToString().Length>30? Eval("body").ToString().Substring(0,30):Eval("body").ToString() %>' CommandArgument='<%# Eval("body")%>' CommandName="Message"  OnClick="lnkMsg_Click" AutoPostBack="true"></asp:LinkButton>
                                             </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField AccessibleHeaderText="Sender" HeaderText="MailID">
                                             <ItemTemplate>
                                            <asp:Label ID="lblfrm" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                             </ItemTemplate>
                                             </asp:TemplateField>
                                           <%--  <asp:TemplateField AccessibleHeaderText="Reciever" HeaderText="Reciever">
                                             <ItemTemplate>
                                             <asp:Label ID="lblto"  runat="server" Enabled="false" Text='<%# Eval("ToiD") %>'></asp:Label>
                                             </ItemTemplate>
                                             </asp:TemplateField>--%>
                                             <asp:TemplateField AccessibleHeaderText="Recieved Date" HeaderText="Date">
                                             <ItemTemplate>
                                             <asp:Label ID="lblrecieved" runat="server" Text='<%# Eval("Date") %>'></asp:Label>
                                             </ItemTemplate>
                                             </asp:TemplateField>
                                            <%-- <asp:TemplateField AccessibleHeaderText="Sent Date" HeaderText="Sent Date">
                                             <ItemTemplate>
                                             <asp:Label ID="lblsent" runat="server" Text='<%# Eval("SendOn") %>'></asp:Label>
                                             </ItemTemplate>
                                             </asp:TemplateField>--%>
                                             <asp:TemplateField AccessibleHeaderText="Processed" HeaderText="Processed">
                                             <ItemTemplate>
                                             <asp:Label ID="lblprocess" runat="server" Text='<%# Eval("isProcessed") %>'></asp:Label>
                                             </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField AccessibleHeaderText="Status" HeaderText="Status">
                                             <ItemTemplate>
                                             <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("STATUS") %>'></asp:Label>
                                             </ItemTemplate>
                                             </asp:TemplateField>
                                             
                                             <asp:TemplateField AccessibleHeaderText="Created Date" HeaderText="Created Date">
                                             <ItemTemplate>
                                             <asp:Label ID="lblrcreated" runat="server" Text='<%# Eval("CreatedOn") %>'></asp:Label>
                                             </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:TemplateField AccessibleHeaderText="Updated Date" HeaderText="Updated Date">
                                             <ItemTemplate>
                                             <asp:Label ID="lblupdated" runat="server" Text='<%# Eval("UpdatedOn") %>'></asp:Label>
                                             </ItemTemplate>
                                             </asp:TemplateField>
                                              <asp:TemplateField AccessibleHeaderText="Process Message" HeaderText="Error/Warning Msg">
                                             <ItemTemplate>
                                             <asp:Label ID="lblsatmsg" runat="server" Text='<%# Eval("Error") %>'></asp:Label>
                                             </ItemTemplate>
                                             </asp:TemplateField>
                                             
                                             <asp:TemplateField AccessibleHeaderText="IsBlog" HeaderText="IsBlog" Visible="false">
                                             <ItemTemplate>
                                             <asp:Label ID="lblIsBlog" runat="server" Text='<%# Eval("IsBlog") %>'></asp:Label>
                                             </ItemTemplate>
                                             </asp:TemplateField>
                                 
                                                        
                                             </Columns><AlternatingRowStyle   Wrap="false"/>
                                                    <EditRowStyle CssClass="grdrowfont" />
                                                    <FooterStyle CssClass="grdrowfont" />
                                                    <HeaderStyle CssClass="titlecolr" Wrap="False" />
                                                    <RowStyle CssClass="grdrowfont"  HorizontalAlign="Center" Wrap="False" />
                                             </asp:GridView>
                                             </asp:Panel>
                                             
                                         
                    
                                     
                                     
                                     </div>&nbsp;</td>  
            
            
</tr>
   </table>
         
         </div>
         <div id="msglight" class="white_contentmsg" >
<table>

<tr>

<td width="5%" align="center">
    <br />
    <img src="Images/loading.gif" />
    <br />
    <asp:Label ID="msgshow" runat="server" ></asp:Label>
</td>
</tr>
</table>
		</div>
		<div id="msgfade" class="black_overlaymsg"></div>
         <div id="Lightsplit"  class="white_contentnew">
<table width="100%">
<tr>
<td>

</td>
</tr>

<tr>
<td>

</td>
<td>
<div style="overflow: auto; height: 400px; width: 500px;" align="center">
<table>
<tr>
<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label ID=lblmsg runat="server" Text="Message Content" Font-Size="Medium" Font-Bold="true">
</asp:Label>  </td></tr>
<tr>
<td>
<asp:TextBox ID="txtMessageBody" runat="server" TextMode="MultiLine" ReadOnly="true"  
         Height="500px" Width="600px" style="OVERFLOW:auto"  ></asp:TextBox>
         </td>
         </tr>
          
         </table>

         </div>
         <br />
         <div align="center">
         
    <table width = "30%">
    <tr>
    <td>
       <asp:Button ID="btnMsgDelete" runat="server" class="button" Text="Delete" 
            OnClick="btnDelete_Click" CausesValidation="False"/>                

    </td>
    <td>
        <asp:Button ID="btnProcess" runat="server" class="button" Text="Re-Process" 
            Visible="false" OnClick="btnProcess_Click" CausesValidation="False"/>

    </td>
    <td>
       <asp:Button ID="btnEdit" runat="server" class="button" Text="Edit" 
            onclick="btnEdit_Click" CausesValidation="False"/>
    </td>
     <td>
        <asp:Button ID="btnSitaUpload" CssClass="button" runat="server" Text="Send via SITA"
                            OnClick="btnSitaUpload_Click" />
    </td>
    <td>
       <asp:Button ID="btnClose" runat="server" class="button" Text="Close" 
            onclick="btnClose_Click" CausesValidation="False"/>

    </td>
    
    </tr>
    </table>
<%--<input type="button" id="btnSplitCancel" class="button" value="Close"  onclick="HidePanelSplit();" size="150%"/>--%>

<asp:HiddenField ID="hlfSender" runat="server" />
<asp:HiddenField ID="hlfreceiver" runat="server" />

<asp:HiddenField ID="hlfSubject" runat="server" />
<asp:HiddenField ID="hlfSrNo" runat="server" />

</div>
    
    </td>
</tr>
<tr>
<td>
</td>
</tr>



</table>
		</div>
		<div id="fadesplit" class="black_overlaynew"></div>
		
<%--</ContentTemplate>
<Triggers>
                            <asp:PostBackTrigger ControlID="gdvMsg" />
                        </Triggers>
</asp:UpdatePanel>--%>
		 
</asp:Content>

