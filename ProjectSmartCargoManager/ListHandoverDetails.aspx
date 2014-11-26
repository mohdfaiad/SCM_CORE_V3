<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListHandoverDetails.aspx.cs" Inherits="ProjectSmartCargoManager.ListHandoverDetails" MasterPageFile = "~/SmartCargoMaster.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
 </asp:Content> 

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</asp:ToolkitScriptManager>
    
    <style type="text/css">
     
    </style>
    
    <script language="javascript" type="text/javascript">

        function DoPostBackWithRowIndex(rowIndex) {
            if (document.getElementById('<%=HdnSelectedRowIndex.ClientID%>') != null)
            {
                document.getElementById('<%=HdnSelectedRowIndex.ClientID%>').value = rowIndex;
            }
            return true;
        }

        function DisableTextBox() 
        {

            var sel = document.getElementById("ddlPaymentType");
            if (sel.options[sel.selectedIndex].value == "Cash")
            {

            }
        } 

    </script>
    
    <script type="text/javascript">

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

        function callShow1() {
            document.getElementById('blackening').style.display = 'block';
            document.getElementById('whitening').style.display = 'block';
        }

        function callclose1() {
            document.getElementById('blackening').style.display = 'none';
            document.getElementById('whitening').style.display = 'none';
        }
     </script>
    
    <style type="text/css">
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
    </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
    
    <div id="contentarea">
  
    <h1>       
            Collection Handover List
    </h1> 
        
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ></asp:Label>
        
        
  <div class="botline"> 
  <table width="100%" cellpadding="3">
    <tr>
        <td>
            Payment Dt *
        </td>
        <td>
            <asp:TextBox ID="txtInvoiceFrom" runat="server" Width="80px"></asp:TextBox>
            <asp:ImageButton ID="btnInvoiceFrom" runat="server" ImageUrl="~/Images/calendar_2.png"
               ImageAlign="AbsMiddle" />
            <asp:CalendarExtender ID="CEInvoiceFrom" Format="dd/MM/yyyy" TargetControlID="txtInvoiceFrom"
               PopupButtonID="btnInvoiceFrom" runat="server" PopupPosition="BottomLeft">
            </asp:CalendarExtender>
            &nbsp;
             <asp:TextBox ID="txtInvoiceTo" runat="server" Width="80px"></asp:TextBox>
             <asp:ImageButton ID="btnInvoiceTo" runat="server" ImageUrl="~/Images/calendar_2.png"
                ImageAlign="AbsMiddle" />
             <asp:CalendarExtender ID="CEInvoiceTo" Format="dd/MM/yyyy" TargetControlID="txtInvoiceTo"
                PopupButtonID="btnInvoiceTo" runat="server" PopupPosition="BottomLeft">
             </asp:CalendarExtender>
        </td>
         <td>
             User ID
        </td>
        <td>
            <asp:TextBox ID="txtUserID" runat="server" Width="90px"></asp:TextBox>
        </td>
        <td>
            Location</td>
        <td>
            <asp:TextBox ID="txtLocation" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
    <td>
        <asp:Button ID="btnSearch" runat="server" CssClass="button" Text="List" 
            onclick="btnSearch_Click"/>
        &nbsp;
        <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" 
            onclick="btnClear_Click" />
            
    </td>
   </tr>
  </table>  
</div>
<br/>
<%--onrowdatabound="grdInvoiceList_RowDataBound" onpageindexchanging="grdInvoiceList_PageIndexChanging"--%>
<%--<asp:GridView ID="grdInvoiceList" Width="90%" runat="server"
         AutoGenerateColumns="False" AllowPaging="True" PageSize="20" 
         onrowdatabound="grdInvoiceList_RowDataBound" 
        onpageindexchanging="grdInvoiceList_PageIndexChanging" ShowFooter="true" Height="150px">
 <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
            <Columns>
               
                <asp:TemplateField HeaderText="Agent Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" Visible=false>
                    <ItemTemplate>
                       <asp:Label ID="lblSrNo" runat="server" Text='<%# Eval("SrNo") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                            
                 <asp:TemplateField HeaderText="Agent Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentName" runat="server" Text='<%# Eval("AgentName") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblGTotalText" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Central Agent" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblCentralAgent" runat="server" Text='<%# Eval("CentralAgent") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Local Agent" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblLocalAgent" runat="server" Text='<%# Eval("LocalAgent") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Invoice Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# Eval("InvoiceNumber") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Invoice Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# Eval("InvoiceAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblGTotalInvoiceAmt" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Collected Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblCollectedAmount" runat="server" Text='<%# Eval("CollectedAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblGTotalCollectedAmt" runat="server" CssClass = "alignrgt"/>
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="TDS" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblTDSAmount" runat="server" Text='<%# Eval("TDSAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Payment Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblPaymentType" runat="server" Text='<%# Eval("PaymentType") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="DCM Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblDCMAmount" runat="server" Text='<%# Eval("DCMAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="DCM Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblDCMType" runat="server" Text='<%# Eval("DCMType") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Cheque#/DD#/ RTGS#" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblChequeDdNo" runat="server" Text='<%# Eval("ChequeDdNo") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Cheque Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblChequeDate" runat="server" Text='<%# Eval("ChequeDate") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Bank Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblBankName" runat="server" Text='<%# Eval("BankName") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Payment Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblPaymentDt" runat="server" Text='<%# Eval("PaymentDate") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Pending Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblPendingAmount" runat="server" Text='<%# Eval("PendingAmount") %>' ></asp:Label >
                    </ItemTemplate>
                     <FooterTemplate>
                        <asp:Label ID="lblGTotalPendingAmt" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
        </Columns> 
                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <FooterStyle BackColor="#336699" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
</asp:GridView>--%>
<%--<div id="whitening" class="white_content" style="width:600px; left:27%; height:200px;"  >
<div id="divbackNew" class="divback">
<table width = "80%">
    <tr>
        <td align ="center" >
        Handover Amount 
        </td>
        <td align ="center">
        Handed Over to
        </td>
        <td align ="center">
        Remarks
        </td>
        <td></td>
    </tr>
    <tr>
        <td align ="center">
            <asp:TextBox ID="txtHandOverAmt" runat="server"></asp:TextBox>
        </td>
        <td align ="center">
            <asp:DropDownList ID="drpUsers" runat="server"></asp:DropDownList>
        </td>
        <td align ="center">
            <asp:TextBox ID="txtRemarks" runat="server"></asp:TextBox>
        </td>
        <td align = "center">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button" 
                onclick="btnSubmit_Click"/>
        </td>
        <td>
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" 
                OnClientClick="javascript:callclose1();"/>
        </td>
    </tr>
</table>
</div>
</div>
--%>
<div>
<asp:GridView ID="grdHandoverAmount" Width="100%" runat="server" 
AutoGenerateColumns="False" AllowPaging="True" PageSize="10" ShowFooter="true" 
Height="150px" onpageindexchanging="grdHandoverAmount_PageIndexChanging" >
 <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
            <Columns>
               
                <asp:TemplateField HeaderText="Handover Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblPayment" runat="server" Text='<%# Eval("HandoverDate") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                            
                 <asp:TemplateField HeaderText="Handover By" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("HandoverBy") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                 </asp:TemplateField>
                 
                 <asp:TemplateField HeaderText="Station" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("HandoverStation") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                 </asp:TemplateField>
                 
                 <asp:TemplateField HeaderText="Handed Over Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblHandedOverAmount" runat="server" Text='<%# Eval("HandoverAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                 </asp:TemplateField>
                 
                 <asp:TemplateField HeaderText="Handed Over To" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblHandedTo" runat="server" Text='<%# Eval("HandoverTo") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                 </asp:TemplateField>
                 
                 <asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                 </asp:TemplateField>
             </Columns> 
                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <FooterStyle BackColor="#336699" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
          </asp:GridView>
</div>

<div id="blackening" class="black_overlay"></div>

<div id="fadesplit" class="black_overlay">
    </div>

  <div id="fotbut">
            <table>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td><asp:HiddenField ID="HdnSelectedRowIndex" runat="server" Value="" /></td>
                </tr>
            </table>
            </div>
            <br />
 </div>
 
    <div id="msglight" class="white_contentmsg">
        <table>
            <tr>
                <td width="5%" align="center">
                    <br />
                    <img src="Images/loading.gif" />
                    <br />
                    <asp:Label ID="msgshow" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    
    <div id="msgfade" class="black_overlaymsg">
    </div>
    
    </ContentTemplate>
    </asp:UpdatePanel>
    
  </asp:Content> 
