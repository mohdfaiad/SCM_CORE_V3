<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmInterlineInvoiceListing.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmInterlineInvoiceListing" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="jquery-ui.custom.js" type="text/javascript"></script>
<script src="jquery-1.10.1.js" type="text/javascript"></script>
<link rel="stylesheet" href="/resources/demos/style.css" />
    <style type="text/css">
     
    </style>
    <script language="javascript" type="text/javascript">
        function GenerateRegularInvoices() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowInterlineAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=RegularInvoices' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateRegularCreditNotes() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + "&Type=Proforma", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowInterlineAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=RegularCreditNotes' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateProformaInvoices() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowInterlineAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=ProformaInvoices' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateProformaCreditNotes() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + "&Type=Proforma", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowInterlineAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=ProformaCreditNotes' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateRegularInvoicesPDF() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowInterlineAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=RegularInvoices' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateRegularCreditNotesPDF() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + "&Type=Proforma", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowInterlineAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=RegularCreditNotes' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateProformaInvoicesPDF() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowInterlineAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=ProformaInvoices' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateProformaCreditNotesPDF() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + "&Type=Proforma", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowInterlineAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=ProformaCreditNotes' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }
    
        function CallPrint(strid) {
            var prtContent = document.getElementById(strid);
         
            var WinPrint = window.open('', '', 'letf=0,top=0,width=*,height=*,toolbar=0,scrollbars=1,status=0');
            WinPrint.document.write("<br>");
            WinPrint.document.write("<CENTER><h2> Invoice Report </h2>");
            WinPrint.document.write("<br>");
            WinPrint.document.write("<b>Agent Name:</b>&nbsp");
            WinPrint.document.write(document.getElementById("<%= hf1.ClientID %>").value);
            WinPrint.document.write("&nbsp&nbsp<b>Bill Type:</b>&nbsp");
            WinPrint.document.write(document.getElementById("<%= hf2.ClientID %>").value);
            WinPrint.document.write("&nbsp&nbsp<b>From Date:</b>&nbsp");
            WinPrint.document.write(document.getElementById("<%= hf3.ClientID %>").value);
            WinPrint.document.write("&nbsp&nbsp<b>To Date:</b>&nbsp");
            WinPrint.document.write(document.getElementById("<%= hf4.ClientID %>").value);
            WinPrint.document.write("&nbsp&nbsp<b>Origin:</b>&nbsp");
            WinPrint.document.write(document.getElementById("<%= hf5.ClientID %>").value);
            WinPrint.document.write("</CENTER><hr>");
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.write("<br><br>");
            WinPrint.document.write();
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
        }

        function SelectheaderCheckboxes(headerchk) {
            var gvcheck = document.getElementById("<%=grdInvoiceList.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
            return (false);
        }
        
    </script>
      <style>
    .ui-menu { position: absolute; width: 100px; }
  </style>
  <script>
      $(function() {
          $("#rerun")
      .button()
      .click(function() {
          alert("Running the last action");
      })
      .next()
        .button({
            text: false,
            icons: {
                primary: "ui-icon-triangle-1-s"
            }
        })
        .click(function() {
            var menu = $(this).parent().next().show().position({
                my: "left top",
                at: "left bottom",
                of: this
            });
            $(document).one("click", function() {
                menu.hide();
            });
            return false;
        })
        .parent()
          .buttonset()
          .next()
            .hide()
            .menu();
      });
  </script>
 </asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
  <div id="contentarea">
  
    <h1>       
            Interline Invoice Listing
    </h1> 
        
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ></asp:Label>
        
  <div class="botline"> 
  
  <table width="100%" cellpadding="3">
   <tr>
    <td>
     Invoice Dt *
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
     Invoice #
    </td>
    <td>
        <asp:TextBox ID="txtInvoiceNumber" runat="server" Width="90px"></asp:TextBox>
    </td>
    <td>Invoice Type</td>
    <td>
        <asp:DropDownList ID="ddlInvoiceType" runat="server" Width="110px" >
          <asp:ListItem Text="Regular Invoice" Value="1"></asp:ListItem>
          <asp:ListItem Text="Regular Credit Note" Value="2"></asp:ListItem>
          <asp:ListItem Text="Proforma Invoice" Value="3"></asp:ListItem>
          <asp:ListItem Text="Proforma Credit Note" Value="4"></asp:ListItem>
        </asp:DropDownList>
    </td>
    
    
    </tr>
    <tr>
    <td>
     Agent
    </td>
    <td>
        <asp:DropDownList ID="ddlAgentCode" runat="server" Width="80px" 
            AutoPostBack="True" onselectedindexchanged="ddlAgentCode_SelectedIndexChanged" >
        </asp:DropDownList>
        &nbsp;
        <asp:DropDownList ID="ddlAgentName" runat="server" Width="170px" 
            AutoPostBack="True" onselectedindexchanged="ddlAgentName_SelectedIndexChanged" >
        </asp:DropDownList>
    </td>
     <td>
     Origin
    </td>
    <td>
        <asp:TextBox ID="txtOrigin" runat="server" Width="90px"></asp:TextBox>
        <asp:AutoCompleteExtender ID="txtOrigin_AutoCompleteExtender" runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
                 TargetControlID="txtOrigin">
               </asp:AutoCompleteExtender>
    </td>
    <td>
     Bill Type
    </td>
    <td>
        <asp:DropDownList ID="ddlBillType" runat="server" Width="120px" >
         <asp:ListItem Text="All" Value=""></asp:ListItem>
          <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
          <asp:ListItem Text="Fortnightly" Value="Fortnightly"></asp:ListItem>
          <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
        </asp:DropDownList>
    </td>      
   </tr>
   <tr>
       <td>
     AWB #
    </td>
    <td>
        <asp:TextBox ID="txtAWBPrefix" runat="server" Width="40px"></asp:TextBox>
        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" 
                runat="server" TargetControlID="txtAWBPrefix" WatermarkText="Prefix">
            </asp:TextBoxWatermarkExtender> 
        &nbsp;
        <asp:TextBox ID="txtAWBNumber" runat="server" Width="90px"></asp:TextBox>
    </td>
    <td>
    Carrier
    </td>
    <td>
        <asp:DropDownList ID="ddlCarrier" runat="server" Width="60px" >
        </asp:DropDownList>
    </td>
     </tr>
     <tr>
       <td>
            <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
                onclick="btnList_Click" />
            &nbsp;
            <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" 
                onclick="btnClear_Click" />
        </td>
     
   </tr>
  </table>  
  
  </div> 
  <br />
 
<div id="divPrint" class="ltfloat" style="width:100%;">
<asp:GridView ID="grdInvoiceList" Width="80%" runat="server"
             AutoGenerateColumns="False" AllowPaging="True" PageSize="10" 
        onpageindexchanging="grdInvoiceList_PageIndexChanging" 
        onrowcommand="grdInvoiceList_RowCommand">
 <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                       <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="javascript:SelectheaderCheckboxes(this);"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Agent Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" Visible="false">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentName" runat="server" Text='<%# Eval("AgentName") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Partner" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblPartnerCode" runat="server" Text='<%# Eval("PartnerCode") %>' ></asp:Label >
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
                
                <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" Visible="false">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# Eval("InvoiceAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
              <%--  <asp:TemplateField HeaderText="IDEC" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Button CssClass="button" CommandName="IDEC" ID="btnIDEC" runat="server" Text="IDEC Input File"  CommandArgument='<%# Eval("InvoiceAmount") %>' />
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>--%>
                
        </Columns> 
                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
</asp:GridView>

</div>   
  <div id="fotbut">
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnPrint" Visible="false" runat="server" 
                            Text="Print Listing" CssClass="button" 
                            OnClientClick="javascript:CallPrint('divPrint')" />
                    </td>&nbsp;
                    <td>
                        <asp:Button ID="btnPrintInvoice" Visible="false" runat="server" 
                            Text="Print Invoice (Excel)" CssClass="button" onclick="btnPrintInvoice_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnPrintInvoicePDF" Visible="false" runat="server" 
                            Text="Print Invoice (PDF)" CssClass="button" 
                            onclick="btnPrintInvoicePDF_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnInvoiceSummary" Visible="false" runat="server" 
                            Text="Invoice Summary" CssClass="button" onclick="btnInvoiceSummary_Click" 
                            Width="119px" />
                        <asp:Button ID="btnIDEC" Visible="false" runat="server" 
                            Text="IDEC Input File" CssClass="button"
                            Width="119px" onclick="btnIDEC_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnCASS" Visible="false" runat="server" 
                            Text="CASS- Billing Participant Export" CssClass="button" Width="175px" />
                        <asp:Button ID="btnCASSHandOffFile" Visible="false" runat="server" 
                            Text="CASS- Hand Off Export" CssClass="button" Width="150px" 
                              />
                        <asp:Button ID="btnCASSStandardDocs"  runat="server" 
                            Text="CASS- Std Documents Export" CssClass="button" Width="185px" Visible="false" 
                              />
                              
                        <asp:Button ID="btnSSIM"  runat="server" 
                            Text="SSIM" CssClass="button"  
                            Width="119px" Visible="false" />
                    </td>
                    <td><asp:HiddenField ID="hf1" runat="server" Value="" /></td>
                    <td><asp:HiddenField ID="hf2" runat="server" Value="" /></td>
                    <td><asp:HiddenField ID="hf3" runat="server" Value="" /></td>
                    <td><asp:HiddenField ID="hf4" runat="server" Value="" /></td>
                    <td><asp:HiddenField ID="hf5" runat="server" Value="" /></td>
                    <td><asp:HiddenField ID="hfInvoiceNos" runat="server" Value="" /></td>
                </tr>
            </table>
            </div>
            <br />
 </div>
    </asp:Content> 
