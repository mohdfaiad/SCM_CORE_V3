<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmTracerAddFoundAwb.aspx.cs" Inherits="MyKFCargoNewProj.FrmTracerAddFoundAwb" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ADD Found Lost Tracer</title>
    

    <script language="javascript" type="text/javascript">
        function GetGridRowValue() 
        {
            window.opener.document.getElementById("ctl00_ContentPlaceHolder1_btnView").click();
            window.close();      
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
        .style1
        {
            width: 351px;
        }
        .style3
        {
            width: 152px;
        }
    </style>
    
    <link type="text/css" href="SControls.css" rel="stylesheet" />
    
</head>
<body>
    <form id="form1" method="post" runat="server" >
    <div>
        <asp:Panel ID = "PnlMain" runat="server" >
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <table style="width:800px">
                <tr class="H2">
                    <td style="text-align: center">
                        
                        <asp:Label ID="lblTitle" runat="server" CssClass="H2" 
                            Text="EDIT Tracer Found Pcs"></asp:Label>
                    </td>
                    
                </tr>
             </table>
             
             <asp:Panel ID = "PnlFilter" runat="server" Width="800px" BackColor="#F1F1E5"  >
             <table style="width:800px">
                <tr class="tr1">
                    <td >
                        
                        Short Pcs:</td>
                    
                    <td >
                        
                        <asp:TextBox ID="txtMsdPcs" runat="server" BackColor="#FFFFBF" Enabled="False"></asp:TextBox>
                        <input id="hdnMsdPcs" type="hidden" runat="server" />
                        
                    </td>
                    
                    <td >
                        
                        Type<input id="hndFndType" type="hidden" runat="server"/>&nbsp;:</td>
                    
                    <td>
                    
                        <asp:DropDownList ID="ddlFndType" runat="server" BackColor="#FFFFBF">
                        <asp:ListItem>Found</asp:ListItem>
                        </asp:DropDownList>
                        <input id="hdnFndType" type="hidden" runat="server" />
                        
                    
                        <input id="hdnFndTypeTxt" type="hidden" runat="server" />
                        
                    
                    </td>
                </tr>
                <tr class="tr1">
                    <td >Found Pcs:</td>
                    
                    <td>
                        
                        <asp:TextBox ID="txtFndPcs" runat="server" BackColor="#FFFFBF"
                            ValidationGroup="ValChk"></asp:TextBox>
                        <input id="hdnFndPcs" type="hidden" runat="server" />                        
                        
                        <%--<asp:Label ID="lblFoundPcs" runat="server" Width="50px" Visible="False"></asp:Label>--%>
                        
                    </td>
                    
                    <td class="style1">
                        
                        <asp:Label ID="lblLoc" runat="server" Text="Found Location"></asp:Label>
                        :</td>
                    
                    <td>
                    
                        <asp:DropDownList ID="ddlFndLoc" runat="server" BackColor="#FFFFBF" 
                            AutoPostBack="True" 
                            onselectedindexchanged="ddlFndLoc_SelectedIndexChanged" 
                            ValidationGroup="ValChk">
                        </asp:DropDownList>
                        <input id="hdnFndLoc" type="hidden" runat="server" />                        
                        <input id="hdnFndLocTxt" type="hidden" runat="server" />
                    </td>
                </tr>
                <tr class="tr1">
                    <td >
                        
                        Remarks:</td>
                    
                    <td class="style3" colspan="3">
                        
                        <asp:TextBox ID="txtRemarks" runat="server" BackColor="#FFFFBF" MaxLength="200" 
                            Width="300px"></asp:TextBox>
                        
                    </td>
                    
                    </tr>
                    
                <tr>
                    <td> Attach File: </td>
                    <td colspan="2">
                        <asp:FileUpload ID="MyFile" runat="server" BackColor="#FFFFBF" />
                        <%--<input id="MyFile" type="file" runat="server"/>--%>
                        <asp:Button ID="btnUpload" runat="server" 
                            onclick="btnUpload_Click" Text="Upload" />
                        (Max 4MB)</td>
                    
                    <td>
                   <%-- <asp:UpdatePanel ID="UpdPnl" runat="server">
                    <ContentTemplate >--%>
                        <asp:ImageButton ID="btnEdit" runat="server" onclick="btnEdit_Click" 
                            Height="35px" ImageUrl="~/images/Add-icon3.png" ValidationGroup="ValChk" 
                            Width="50px" />
<input id="hdnAWBno" type="hidden" runat="server" />
<%--</ContentTemplate>
 </asp:UpdatePanel>--%>
                        <input id="hdnTracerNo" type="hidden" runat="server" />
                   </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center">
                        <asp:Label ID="lblErrormsg" runat="server" CssClass="errMsgOn"
                            Visible="False" ></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="errMsgOn"
                            ControlToValidate="ddlFndLoc" ErrorMessage="Please Select Found Location" 
                            InitialValue="SELECT" SetFocusOnError="True" ValidationGroup="ValChk"></asp:RequiredFieldValidator>
                        
                        
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="errMsgOn"
                            ControlToValidate="txtFndPcs" ErrorMessage="Found Pcs Mandatory." 
                            ValidationGroup="ValChk"></asp:RequiredFieldValidator>
                        
                        
                    </td>
                </tr>
                
            </table>
            </asp:Panel>
            <asp:RoundedCornersExtender ID="PnlFilter_RoundedCornersExtender" 
                runat="server" Enabled="True" TargetControlID="PnlFilter" Radius="6" Corners="All" BorderColor="DarkGray">
            </asp:RoundedCornersExtender>
            <br />
            <asp:Panel ID = "grdPnl" runat="server" Width="100%" >
            <table width= "100%">
                <tr class="tr1">
                    <td style="width:50% ;text-align: center">
                        Archived Files
                    </td>
                    <td style="width:50% ;text-align: center">
                        Current Uploaded Files
                    </td>
                </tr>
                <tr>
                    <td style="width:50%">
                        <asp:GridView ID="grdArchived" runat="server" BackColor="White" 
                             BorderColor="#990000" BorderStyle="Solid" BorderWidth="1px" 
          CellPadding="3" CellSpacing="2"
                            ShowFooter="True" AutoGenerateColumns="False" 
                            DataKeyNames="ArchivedFileId" onrowcreated="grdArchived_RowCreated" 
                            CssClass="ctrlGrid" onrowdatabound="grdArchived_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" 
                                        ImageUrl="~/images/1288766577_button_cancel.png" 
                                        CausesValidation="false"
                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                                        CommandName="Delete"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BinaryData">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBinaryData" runat="server" Text='<%# Bind("BinaryData") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="MimeType">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMimeType" runat="server" Text='<%# Bind("MimeType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AWBNo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAWBNo" runat="server" Text='<%# Bind("AWBNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TracerNo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTracerNo" runat="server" Text='<%# Bind("TracerNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                            </Columns>
                                <RowStyle BackColor="White" ForeColor="Blue"  HorizontalAlign="Center" 
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
                                    HorizontalAlign="Center" Font-Names="Verdana" Font-Size="Small" />
      </asp:GridView>
                    </td>
                    <td style="width:50%">
                        <asp:GridView ID="grdCurrArchived" runat="server" BackColor="White" 
                             BorderColor="#990000" BorderStyle="Solid" BorderWidth="1px" 
          CellPadding="3" CellSpacing="2" ShowFooter="True" AutoGenerateColumns="False" 
                            onrowcommand="grdCurrArchived_RowCommand" 
                            onrowdeleting="grdCurrArchived_RowDeleting" CssClass="ctrlGrid" 
                            onrowdatabound="grdCurrArchived_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("FileName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" 
                                        ImageUrl="~/images/1288766577_button_cancel.png" 
                                        CausesValidation="false"
                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                                        CommandName="Delete"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                            </Columns>
                                <RowStyle BackColor="White" ForeColor="Blue"  HorizontalAlign="Center" 
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
                                    HorizontalAlign="Center" Font-Names="Verdana" Font-Size="Small" />
                    </asp:GridView>
                    </td>
                </tr>
            </table>
                
            </asp:Panel>
        </asp:Panel>
    </div>
    </form>
</body>
</html>