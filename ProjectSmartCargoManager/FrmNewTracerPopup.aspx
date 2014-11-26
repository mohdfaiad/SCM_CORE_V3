<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNewTracerPopup.aspx.cs" Inherits="MyKfCargo.FrmNewTracerPopup" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="style/style.css" rel="stylesheet" type="text/css" />
    
    <script language="javascript" type="text/javascript">
        function GetGridRowValue(txtAwb,btn,txtAWBPrefix) 
        {
           try{
               window.opener.document.getElementById("ctl00_ContentPlaceHolder1_txtAwbno").value = txtAwb;
               window.opener.document.getElementById("ctl00_ContentPlaceHolder1_txtAWBPrefix").value = txtAWBPrefix;
               window.opener.document.getElementById("ctl00_ContentPlaceHolder1_BtnSearch2").click();
               
                window.close();
                }
                catch(e){
                CloseWin();
                }

            }
            function ConfirmAWB() {
                var hdConf = document.getElementById("hdConfirm");
                var result = confirm("AWB Does not Exists.. Do you want to continue?");
                if (result == true) {
                    hdConf.value = "true";
                }
                else {
                    hdConf.value = "false";
                }
            }
        function CloseWin() 
        {
            window.close();
        }

        function checkAWBNo()
         {
             var AWB = document.getElementById('txtAwbNoTr').value;
                        
             if (AWB.length!= 8) 
             {
                 alert("Please Enter 8 Digit AWB No with Prefix");
             }
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
    <form id="form1" runat="server">
    <div style="width:850px; margin:10px auto; border:solid 3px #cccccc; padding:10px; ">
    <%--<asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
        <asp:Panel ID="Panel2" runat="server" 
            CssClass="PnlMain"  ><%--Style="display: none;"--%>
                    <div>
                        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                        </asp:ToolkitScriptManager>
                        
                        <div class="msg">
                           <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
                        </div>
                        
                        <h1 style="width:840px;">
                        
                            <img src="Images/txt_createnewtracer.png" />
                        </h1>
                        <table  style="width:90%;">
                        <tr>
                        <td style="width:70%;">
                        <table>
                        <tr>
                        <td style="text-align: center">
                            <input id="hdnFltVal" type="hidden" runat="server"/>
                            <input id="HdnPanel" type="hidden" runat="server"/>
                            <input id="HdnUID" type="hidden" runat="server"/>
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                 Flight No</td>
                            <td >
                                <asp:TextBox ID="txtFlightNo" runat="server" AutoPostBack="True" Width="100px"
                                    ontextchanged="txtFlightNo_TextChanged" CssClass="ctrltxt" MaxLength="7" ></asp:TextBox>
                              
                                <asp:AutoCompleteExtender ID="txtFlightNo_AutoCompleteExtender" runat="server" 
                                    ServiceMethod="GetFlightList" MinimumPrefixLength="3" CompletionInterval="10" 
                                    CompletionSetCount="10" DelimiterCharacters="" Enabled="True" ServicePath="" 
                                    TargetControlID="txtFlightNo" UseContextKey="True">
                                </asp:AutoCompleteExtender>
                                
                                <%-- <asp:DropDownList ID="ddlFlightID" runat="server" Width="100px">
                                </asp:DropDownList>--%><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                    ControlToValidate="txtFlightNo" ErrorMessage="Mandatory" 
                                    SetFocusOnError="True" CssClass="errMsgOn" ValidationGroup="ValChkGenTr"></asp:RequiredFieldValidator>
                            </td>
                            
                        </tr>
                        
                        <tr>
                                <td >
                                      Orgin</td>
                                <td>
                                    <asp:DropDownList ID="ddlOrg" runat="server" CssClass="tr1" Width="100px">
                                    </asp:DropDownList>
                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" 
                                        ControlToValidate="ddlOrg" CssClass="errMsgOn" ErrorMessage="Mandatory" 
                                        InitialValue="SELECT" SetFocusOnError="True" ValidationGroup="ValChkGenTr"></asp:RequiredFieldValidator>
                                    
                            </td>
                            
                        </tr>
                        <tr>
                            <td>Dest</td>
                            <td>
                                <asp:DropDownList ID="ddlDest" runat="server" CssClass="tr1" Width="100px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" 
                                 ControlToValidate="ddlDest" CssClass="errMsgOn" ErrorMessage="Mandatory" 
                                 InitialValue="SELECT" SetFocusOnError="True" ValidationGroup="ValChkGenTr"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                           <td>
                                  Flight Date</td>
                           <td >
                           <asp:TextBox ID="txtDate" runat="server" CssClass="ctrltxt" AutoPostBack="True" Width="100px"
                                   ontextchanged="txtDate_TextChanged" ></asp:TextBox>
                               <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" 
                                   Enabled="True" TargetControlID="txtDate" Format="dd/MM/yyyy" PopupButtonID="imgfltdate" >
                               </asp:CalendarExtender>
                               
                               <asp:ImageButton ID="imgfltdate" runat="server" ImageAlign="AbsMiddle" 
                        ImageUrl="~/Images/calendar_2.png" />
                        
                               <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                   ControlToValidate="txtDate" ErrorMessage="Mandatory" 
                                   SetFocusOnError="True" CssClass="errMsgOn" ValidationGroup="ValChkGenTr"></asp:RequiredFieldValidator>
                           </td>
                        </tr>
                        <tr>
                            <td>
                                 Airwaybill No
                            </td>
                            <td >
                                <asp:TextBox ID="txtAWBPrefix" runat="server" MaxLength="3" CssClass="ctrltxt" 
                                    AutoPostBack="True" Width="40px"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                    ControlToValidate="txtAWBPrefix" ErrorMessage="*" 
                                    SetFocusOnError="True" CssClass="errMsgOn" ValidationGroup="ValChkGenTr"></asp:RequiredFieldValidator>
                                    
                                <asp:TextBox ID="txtAwbNoTr" runat="server" AutoPostBack="True" Width="100px"
                                    CssClass="ctrltxt" MaxLength="8" ontextchanged="txtAwbNoTr_TextChanged" onchange="javascript:return checkAWBNo()"></asp:TextBox>
                                    
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                    ControlToValidate="txtAwbNoTr" ErrorMessage="Mandatory" 
                                    SetFocusOnError="True" CssClass="errMsgOn" ValidationGroup="ValChkGenTr"></asp:RequiredFieldValidator>
                                    
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ControlToValidate="txtAwbNoTr" CssClass="errMsgOn" ErrorMessage="Only Digits" 
                                    ValidationExpression="^\d+$" ValidationGroup="ValChkGenTr"></asp:RegularExpressionValidator>
                                </td>
                        </tr>
                        <tr>
                                <td>
                                
                                    Total Sent PCS
                                </td>
                                <td>
                                <asp:TextBox ID="txtTotalPcs" runat="server" MaxLength="6" Width="50px"
                                         CssClass="ctrltxt"></asp:TextBox>
                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                        ControlToValidate="txtTotalPcs" ErrorMessage="Mandatory" 
                                        SetFocusOnError="True" CssClass="errMsgOn" ValidationGroup="ValChkGenTr"></asp:RequiredFieldValidator>
                                   <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                        ControlToValidate="txtTotalPcs" CssClass="errMsgOn" ErrorMessage="Only Digits" 
                                        SetFocusOnError="True" ValidationExpression="^\d+$" 
                                        ValidationGroup="ValChkGenTr"></asp:RegularExpressionValidator>
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" 
                                        ControlToValidate="txtTotalPcs" CssClass="errMsgOn" ErrorMessage="Invalid" 
                                        MaximumValue="9999" MinimumValue="1" SetFocusOnError="True" 
                                        ValidationGroup="ValChkGenTr"></asp:RangeValidator>
                                </td>
                                </tr>
                        
                        <tr>
                            <td>Total Wt(in kgs)
                            </td>
                            <td>
                            <asp:TextBox ID="txtWgt" runat="server"  CssClass="ctrltxt" Width="50px"
                                        MaxLength="8"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                        ControlToValidate="txtWgt" CssClass="errMsgOn" ErrorMessage="Only Digits" 
                                        SetFocusOnError="True" ValidationExpression="^\d*\.?\d*$" 
                                        ValidationGroup="ValChkGenTr"></asp:RegularExpressionValidator>
                                    
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" 
                                        ControlToValidate="txtWgt" ErrorMessage="Mandatory" SetFocusOnError="True" 
                                        CssClass="errMsgOn" ValidationGroup="ValChkGenTr"></asp:RequiredFieldValidator>
                                    
                            </td>
                        </tr>
                        <tr>
                            <td >
                                Short PCS
                            </td>
                            <td  >
                                <asp:TextBox ID="txtShortage" runat="server" MaxLength="6" CssClass="ctrltxt" Width="50px"
                                    ValidationGroup="ValChkGenTr"></asp:TextBox>
                                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="txtShortage" ErrorMessage="Mandatory" 
                                    ValidationGroup="ValChkGenTr" CssClass="errMsgOn"></asp:RequiredFieldValidator>
                                <input id="hdnSentPcs" type="hidden" runat="server"/>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                                    ControlToValidate="txtShortage" CssClass="errMsgOn" ErrorMessage="Only Digits" 
                                    SetFocusOnError="True" ValidationExpression="^\d+$" 
                                    ValidationGroup="ValChkGenTr"></asp:RegularExpressionValidator>
                                </td>
                        </tr>
                        <tr>
                            <td>AWB Status</td>
                            <td>
                                <asp:DropDownList ID="ddlAWBStatus" runat="server" 
                                ToolTip="SELECT THE DO STATUS CRITERIA IF ANY" CssClass="tr1" Width="100px">
                                <asp:ListItem Value="UnDelivered">UnDelivered</asp:ListItem>
                                <asp:ListItem Value="Partially">Partially Delivered</asp:ListItem>
                                <asp:ListItem Value="Completed">Completed</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                                
                        <tr>
                            <td >
                                Packaging
                            </td>
                            <td>
                                <asp:TextBox ID="txtPkng" runat="server"  CssClass="ctrltxt" Width="100px"></asp:TextBox>
                            </td>
                            </tr>
                            
                            <tr>
                            <td>
                                Contents
                            </td>
                            <td>
                                <asp:TextBox ID="txtContents" runat="server" CssClass="ctrltxt" Width="100px" ></asp:TextBox>
                            </td>
                            </tr>
                            <tr class="tr1">
                            <td>
                                 Consignor
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrgAgent" runat="server"  CssClass="ctrltxt" ></asp:TextBox>
                           </td>
                            </tr>
                            
                            <tr>
                            <td>
                                 Consignee
                            </td>
                            <td>
                                <asp:TextBox ID="txtDestAgent" runat="server"  CssClass="ctrltxt" ></asp:TextBox>
                           </td>
                           </tr>
                           
                            <tr>
                            <td >
                                Remarks
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server"  CssClass="ctrltxt" ></asp:TextBox>
                            </td>
                            </tr>
                        
                        <tr>
                            <td>
                                 Attachements 
                            </td>
                            <td>
                                 <input id="MyFile" type="File" runat="Server" 
                                     title="CLICK TO ATTACHED FILE IF ANY" />
                                <asp:Button ID="btnUpd" runat="server" Text="Upload" onclick="btnUpd_Click" 
                                     ToolTip="CLICK HERE TO UPLOAD ATTACHED FILE" Height="21px" Width="80px" class="button" />
                                 
                                 (Max 4Mb)</td>
                                 
                        </tr>
                        <tr>
                            <td></td>
                            <td style="text-align: left">
                                
                                <asp:Button ID="btnSubmit" runat="server" Text="Generate Tracer" 
                                    onclick="btnSubmit_Click" 
                                    ToolTip="CLICK HERE TO GENERATE TRACER AND SEND EMAIL " 
                                    ValidationGroup="ValChkGenTr" CssClass="button"
                                    
                                     /> <%--onclick="btnSubmit_Click"--%>
                                <input id="hdnWeight" type="hidden" runat="server"/>
                                <input id="hdnEdit" type="hidden" runat="server" />
                                <input id="hdnTracerNo" type="hidden"  runat="server"/>
                            </td>
                                 
                        </tr>
                    </table> </td>
                        
                        <td valign="top">
                        <h3>Uploaded Files</h3>
                                <%--<asp:ImageButton ID="ImgCancelBtn" runat="server" 
                                    ImageUrl="~/images/1288766577_button_cancel.png" onclick="ImgCancelBtn_Click" />--%>
                        <asp:UpdatePanel ID="UpdPnlgrdCurrArchived" runat="server"> 
                             <ContentTemplate>
                                <asp:GridView ID="grdCurrArchived" runat="server" AutoGenerateColumns="False" 
                                    CellPadding="0" onrowcommand="grdCurrArchived_RowCommand" 
                                    onrowdeleting="grdCurrArchived_RowDeleting" ShowFooter="True" Width="300px">
                                    <RowStyle BorderColor="#333333" BorderStyle="Solid" ForeColor="#000066" CssClass="ctrlGrid" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="File Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("FileName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" 
                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                                                    CommandName="Delete" ImageUrl="~/images/cross.jpg" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <%--<FooterStyle BackColor="White" ForeColor="#000066" CssClass="ctrlGrid" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" CssClass="ctrlGrid" />
                                    <SelectedRowStyle BackColor="#669999" ForeColor="White" CssClass="ctrlGrid" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" CssClass="ctrlGrid" />
                                    <AlternatingRowStyle BorderColor="#333333" BorderStyle="Solid" CssClass="ctrlGrid" />--%>
                                    
                                                                <HeaderStyle CssClass="titlecolr" />
                        <RowStyle HorizontalAlign="Center" />
                        <AlternatingRowStyle HorizontalAlign="Center" />
                                    
                                    
                                </asp:GridView>
                              </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:HiddenField ID="hdConfirm" runat="server" />
                            </td></tr></table>
                    
                  </div>
                </asp:Panel>
       
        <%--<FooterStyle BackColor="White" ForeColor="#000066" CssClass="ctrlGrid" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" CssClass="ctrlGrid" />
                                    <SelectedRowStyle BackColor="#669999" ForeColor="White" CssClass="ctrlGrid" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" CssClass="ctrlGrid" />
                                    <AlternatingRowStyle BorderColor="#333333" BorderStyle="Solid" CssClass="ctrlGrid" />--%>
      
    </div>
    </form>
</body>
</html>
