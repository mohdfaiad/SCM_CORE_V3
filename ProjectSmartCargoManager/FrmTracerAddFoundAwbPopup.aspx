<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTracerAddFoundAwbPopup.aspx.cs" Inherits="MyKFCargoNewProj.FrmTracerAddFoundAwbPopup" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ADD Found Lost Tracer</title>
    <link href="style/style.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function GetGridRowValue1(btn) {
            try {
                window.opener.document.getElementById(btn).click();
                window.close();
            }
            catch (e) {
                CloseWin();
            }
        }

        

    </script>
    
     <script type="text/javascript">
         function Gethref() {
             var href2 = document.getElementById("lboxhref");
             var source = document.getElementById('<%= InvoiceImage.ClientID%>');
             href2.href = source.src;

         }
     </script>
    
    <script type="text/javascript">
        function RadioCheck(rb) {
            var gv = document.getElementById("<%=grdArchived.ClientID%>");
            var rbs = gv.getElementsByTagName("input");

            var row = rb.parentNode.parentNode;
            for (var i = 0; i < rbs.length; i++) {
                if (rbs[i].type == "radio") {
                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;
                        break;
                    }
                }
            }
        }

        function Download(count) {
            window.open('Download.aspx?FileName=' + count, 'Download', 'menubar=0, toolbar=0, location=0, status=0, resizable=0, width=100, height=50');
        }   
</script>
    
    <style type="text/css">
       
    .hiddencol1
    {
        display:none;
    }
    .viscol1
    {
        display:block;
    }
        </style>
    
    <link type="text/css" href="SControls.css" rel="stylesheet" />
    
</head>
<body style="background:#ffffff !important;">

    <form id="form1" method="post" runat="server" >
    <div style="width:850px; margin:0px auto; border:solid 3px #cccccc; padding:10px; ">
        <asp:Panel ID = "PnlMain" runat="server" >
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            
            <div class="msg">
            <asp:Label ID="lblErrormsg" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" Visible="False" ></asp:Label>
            
            </div>
                       <h1 style="width:840px;"> 
                           <%--<img src="Images/txt_edittracer.png" />--%> Edit Tracer Found / Lost Pcs
                       </h1>
                            <input id="HdnUID" type="hidden" runat="server"/>                    
                
             <asp:Panel ID = "PnlFilter" runat="server">
             <table cellpadding="6" cellspacing="6" width="96%">
             <tr><td valign="top">
             <table cellpadding="3" cellspacing="3">
             
                 <tr>
                     <td>
                         Short Pcs</td>
                     <td>
                         <asp:TextBox ID="txtMsdPcs" runat="server" Enabled="False" Width="100px"></asp:TextBox>
                         <input id="hdnMsdPcs" runat="server" type="hidden" />
                     </td>
                     <td>
                         Type<input id="hndFndType" runat="server" type="hidden" /></td>
                     <td>
                         <asp:DropDownList ID="ddlFndType" runat="server">
                             <asp:ListItem>Found</asp:ListItem>
                         </asp:DropDownList>
                         <input id="hdnFndType" runat="server" type="hidden" />
                         <input id="hdnFndTypeTxt" runat="server" type="hidden" />
                     </td>
                 </tr>
                <tr>
                    <td >Found/Lost Pcs</td>
                    
                    <td>
                        
                        <asp:TextBox ID="txtFndPcs" runat="server"  Width ="100px" ValidationGroup="ValChk"></asp:TextBox>
                        <input id="hdnFndPcs" type="hidden" runat="server" />                        
                        
                        <%--<asp:Label ID="lblFoundPcs" runat="server" Width="50px" Visible="False"></asp:Label>--%>
                        
                    </td>
                    
                    <td>
                        
                        Found/Lost Location</td>
                    
                    <td>
                    
                        <asp:DropDownList ID="ddlFndLoc" runat="server"  
                            onselectedindexchanged="ddlFndLoc_SelectedIndexChanged" 
                            ValidationGroup="ValChk">
                        </asp:DropDownList>
                        <input id="hdnFndLoc" type="hidden" runat="server" />                        
                        <input id="hdnFndLocTxt" type="hidden" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        
                        Remarks</td>
                    
                    <td colspan="3">
                        
                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="200" 
                            Width="100px"></asp:TextBox>
                        
                    </td>
                    
                    
                    
                </tr>
                <tr>
                    <td>Attach File
                    </td>
                    <td colspan="3">
                        <asp:FileUpload ID="MyFile" runat="server"/>
                        <%--<input id="MyFile" type="file" runat="server"/>--%>
                        <asp:Button ID="btnUpload" runat="server" 
                            onclick="btnUpload_Click" Text="Upload" CssClass="button" />
                        (Max 4MB)<%-- <asp:UpdatePanel ID="UpdPnl" runat="server">
                    <ContentTemplate >--%>
                        <%--<asp:ImageButton ID="btnEdit" runat="server" onclick="btnEdit_Click" 
                             ImageUrl="~/images/Add-icon3.png" ValidationGroup="ValChk" 
                             ToolTip="click Here To Save Edited Detail." />--%>
                        <input id="hdnAWBno" runat="server" type="hidden" />
                        <%--</ContentTemplate>
 </asp:UpdatePanel>--%>
                        <input id="hdnTracerNo" runat="server" type="hidden" />
                    </td>
                    
                </tr>
                
                <tr>
                   <td>
                    <asp:Button ID="BtnEdit1" runat="server" Text="Save" onclick="BtnEdit1_Click" CssClass="button"/>
                   </td>
                    <td colspan="3" style="text-align: center">
                        
                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="errMsgOn"
                            ControlToValidate="ddlFndLoc" ErrorMessage="Please Select Found Location" 
                            InitialValue="SELECT" SetFocusOnError="True" ValidationGroup="ValChk"></asp:RequiredFieldValidator>
                        
                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="errMsgOn"
                            ControlToValidate="txtFndPcs" ErrorMessage="Found Pcs Mandatory." 
                            ValidationGroup="ValChk"></asp:RequiredFieldValidator>
                        <input id="HdnBtn" type="hidden" runat="server" />
                        
                    </td>
                </tr>
                
            </table></td>
             <td style="width:40%" valign="top" >
             
             <div class="divback" style="width:100%;"><strong>Current Uploaded Files</strong>
             
                        <asp:GridView ID="grdCurrArchived" runat="server"  
                            CellPadding="3" CellSpacing="2" ShowFooter="True" AutoGenerateColumns="False" 
                            onrowcommand="grdCurrArchived_RowCommand" 
                            onrowdeleting="grdCurrArchived_RowDeleting"  
                            onrowdatabound="grdCurrArchived_RowDataBound" Width="100%" >
                            <Columns>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("FileName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" 
                                        ImageUrl="~/images/cross.jpg" 
                                        CausesValidation="false"
                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                                        CommandName="Delete"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                            </Columns>
                        <%--<RowStyle BackColor="White" ForeColor="Blue"  HorizontalAlign="Center" 
                            Font-Names="Verdana" Font-Size="Small"/>
                        <FooterStyle BackColor="#990000" ForeColor="White"  HorizontalAlign="Center" 
                            Font-Names="Verdana" Font-Size="Small" />
                        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" 
                            Font-Names="Verdana" Font-Size="Small" />
                        <SelectedRowStyle BackColor="White" ForeColor="#990000" 
                            HorizontalAlign="Center" Font-Names="Verdana" Font-Size="Small" />
                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" 
                            HorizontalAlign="Center" Font-Names="Verdana" Font-Size="Small" />
                        <AlternatingRowStyle BackColor="White" ForeColor="Blue" 
                            HorizontalAlign="Center" Font-Names="Verdana" Font-Size="Small" />--%>
                            
                             <HeaderStyle CssClass="titlecolr" />
                        <RowStyle HorizontalAlign="Center" />
                        <AlternatingRowStyle HorizontalAlign="Center" />
                    </asp:GridView> </div></td></tr></table>
             
            </asp:Panel>
           <%-- <asp:RoundedCornersExtender ID="PnlFilter_RoundedCornersExtender" 
                runat="server" Enabled="True" TargetControlID="PnlFilter" Radius="6" Corners="All" BorderColor="DarkGray">
            </asp:RoundedCornersExtender>--%>
            
            
            
            
            
            
            
            <br />
            <fieldset style="border:1px solid #000;"><legend style="border:1px solid #ccc; font-weight:bold; padding:3px;" >Archived Files:</legend>
            <asp:Panel ID = "grdPnl" runat="server" Width="100%" >
            <table width= "100%" cellpadding="3" cellspacing="3">
                <tr>
                    <td></td>
                    <td>
                        
                        <%--   <asp:ImageButton ID="btnPrev" runat="server"  ImageUrl="~/Images/prev.png" Height="25px" Width="25px"  Visible="false"/></td>
            
            
            <asp:Label ID="lblPageNo" runat="server"></asp:Label></td>
            
                  <asp:ImageButton ID="btnNext" runat="server" ImageUrl="~/Images/next.png" Height="25px" Width="25px" Visible="false"/>
     --%>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                    
                        <asp:GridView ID="grdArchived" runat="server" BackColor="White" CellPadding="3" CellSpacing="2" 
                            ShowFooter="True" AutoGenerateColumns="False" > <%--DataKeyNames="ArchivedFileId"--%>
                           
                            <Columns>
                            
                            <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Select">
                                <ItemTemplate>
                                   <asp:RadioButton ID="rdbePouch" runat="server"  onclick="javascript:RadioCheck(this)"  />
                                </ItemTemplate>
                                <HeaderStyle Wrap="True" />
                                <ItemStyle Wrap="False" />
                           </asp:TemplateField>
                           
                           <asp:TemplateField HeaderText="SerialNumber" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Eval("SerialNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                      
                                <asp:TemplateField HeaderText="FileName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="MimeType">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMimeType" runat="server" Text='<%# Eval("MimeType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                
                                <asp:TemplateField HeaderText="Attachment" Visible="false">
                                    <ItemTemplate>
                                    <%--<asp:Image ID="Image" runat="server"--%>
                                        <asp:Label ID="lblAttachment" runat="server" Text='<%# Eval("Attachment") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <%--<asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" 
                                        ImageUrl="~/images/cross.jpg" 
                                        CausesValidation="false"
                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                                        CommandName="Delete"/>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="Attachment">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAttachment" runat="server" Text='<%# Bind("Attachment") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                
                                
                               <%-- <asp:TemplateField HeaderText="AWBNo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAWBNo" runat="server" Text='<%# Bind("AWBNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TracerNo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTracerNo" runat="server" Text='<%# Bind("TracerNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                
                            </Columns>
                            
                             <HeaderStyle CssClass="titlecolr" />
                        <RowStyle HorizontalAlign="Center" />
                        <AlternatingRowStyle HorizontalAlign="Center" />
                    </asp:GridView>
                    <asp:Button ID="btnDisplay" Text="Display" CssClass="button" runat="server" 
                    UseSubmitBehavior="true" onclick="btnDisplay_Click"/>
                    </td>
                    
                    <td>
                        
                           <asp:Panel ID="pnlImage" runat="server" >
        <a id="lboxhref" rel="lightbox" title="Image"  >
            
            <asp:ImageButton ID="InvoiceImage" ImageAlign="AbsMiddle"  runat="server" BorderColor="Black" 
                BorderStyle="Solid" Width="100%" Height="100%" BorderWidth="1" BackColor="#000000" OnClientClick="javascript:Gethref()" value="" Visible="false"   />
                </a>
            </asp:Panel></td>
                </tr>
            </table>
                
               
                
            </asp:Panel>
            </fieldset>
            
            
        </asp:Panel>
    </div>
    </form>
</body>
</html>