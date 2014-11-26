<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmInvoiceListing.aspx.cs"  MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.FrmInvoiceListing" %>

<%--<%@ Register Assembly="C1.Web.Wijmo.Controls.3" Namespace="C1.Web.Wijmo.Controls.C1Menu"
    TagPrefix="wijmo" %>--%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="jquery-ui.custom.js" type="text/javascript"></script>
<script src="jquery-1.10.1.js" type="text/javascript"></script>
<link rel="stylesheet" href="/resources/demos/style.css" />
    <style type="text/css">
     
    </style>
    <script language="javascript" type="text/javascript">
        function GetInvoiceType(val) {
            
            //in val u get dropdown list selected value
            if (val == "1" || val == "4") {
                document.getElementById("ctl00_ContentPlaceHolder1_txtAWBNumber").value = "";
                document.getElementById("ctl00_ContentPlaceHolder1_txtAWBNumber").disabled = true;
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_txtAWBNumber").disabled = false;
            }
        }
        function GenerateRegularInvoices() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Regular' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Regular' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateProformaInvoices() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + "&Type=Proforma", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Proforma' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");

            }
        }

        function GenerateWalkInInvoices() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=WalkIn' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowWalkInAgentInvoice.aspx?INVNO=" + invArr[i] + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=WalkIn' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateDestInvoices() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Dest' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowDestAgentInvoice.aspx?INVNO=" + invArr[i] + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Dest' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateRegularInvoicesPDF() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Regular' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Regular' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Regular' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateProformaInvoicesPDF() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + "&Type=Proforma", "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Proforma' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Proforma' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");

            }
        }

        function GenerateWalkInInvoicesPDF() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=WalkIn' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowWalkInAgentInvoice.aspx?INVNO=" + invArr[i] + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=WalkIn' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateDestInvoicesPDF() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Dest' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowDestAgentInvoice.aspx?INVNO=" + invArr[i] + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Dest' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
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
    
    <%--<script language="javascript" type="text/javascript">
        function GenerateRegularInvoices() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Regular' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateProformaInvoices() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + "&Type=Proforma", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Proforma' + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
                
            }
        }

        function GenerateWalkInInvoices() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                window.open("ShowWalkInAgentInvoice.aspx?INVNO=" + invArr[i] + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateDestInvoices() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                window.open("ShowDestAgentInvoice.aspx?INVNO=" + invArr[i] + '&Format=Excel', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateRegularInvoicesPDF() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowAirCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Regular' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateProformaInvoicesPDF() {
            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i], invArr[i], "status=0,toolbar=0, menubar=0, width=490, height=650");
                //window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + "&Type=Proforma", "status=0,toolbar=0, menubar=0, width=490, height=650");
                window.open("ShowCargoInvoice.aspx?INVNO=" + invArr[i] + '&Type=Proforma' + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");

            }
        }

        function GenerateWalkInInvoicesPDF() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                window.open("ShowWalkInAgentInvoice.aspx?INVNO=" + invArr[i] + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
            }
        }

        function GenerateDestInvoicesPDF() {

            var hfInvNos = document.getElementById("<%= hfInvoiceNos.ClientID %>");
            var InvList = hfInvNos.value;
            var invArr = InvList.split(',');
            for (var i = 0; i < invArr.length; i++) {
                window.open("ShowDestAgentInvoice.aspx?INVNO=" + invArr[i] + '&Format=PDF', "", "status=0,toolbar=0, menubar=0, width=490, height=650");
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

    </script>--%>
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
  <script type="text/javascript">
      function Download() {
          window.open('Download.aspx?Mode=ERP' , 'Download', 'menubar=0, toolbar=0, location=0, status=0, resizable=0, width=100, height=50');
      }
  </script>
 </asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <br />
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
  <div id="contentarea">
  
    <h1>       
            Invoice Listing
    </h1> 
       
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ></asp:Label>
 
  <div class="botline"> 
  
  <table width="100%" cellpadding="5" cellspacing="3">
  
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
        <asp:DropDownList ID="ddlInvoiceType" runat="server" Width="80px" onchange="GetInvoiceType(this.options[this.selectedIndex].value);" >
          <asp:ListItem Text="Agent" Value="1"></asp:ListItem>
          <asp:ListItem Text="Walk-In" Value="2"></asp:ListItem>
          <asp:ListItem Text="Destination" Value="3"></asp:ListItem>
          <asp:ListItem Text="Proforma" Value="4"></asp:ListItem>
        </asp:DropDownList>
    </td>
    <td>Invoice Status</td>
     <td>
        <asp:DropDownList ID="ddlInvoiceStatus" runat="server" Width="80px" >
          <asp:ListItem Text="All" Value=""></asp:ListItem>
          <asp:ListItem Text="Open" Value="O"></asp:ListItem>
          <asp:ListItem Text="Closed" Value="C"></asp:ListItem>
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
     AWB Number
    </td>
    <td>
        <asp:TextBox ID="txtAWBPrefix" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" 
                runat="server" TargetControlID="txtAWBPrefix" WatermarkText="Prefix">
            </asp:TextBoxWatermarkExtender> 
        &nbsp;
        <asp:TextBox ID="txtAWBNumber" runat="server" Width="90px" MaxLength="10"></asp:TextBox>
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
          <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
        </asp:DropDownList>
    </td>
    </tr>
    <tr>      
    <td colspan="2">
        <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
            onclick="btnList_Click" />
            &nbsp;
        <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" onclick="btnClear_Click" 
            />
    </td>
     <td></td>

   </tr>
  </table>  
  
  </div> 
  <br />
 
<div id="divPrint" style="overflow-x:auto;width:100%">
<asp:GridView ID="grdInvoiceList" Width="80%" runat="server"
             AutoGenerateColumns="False" AllowPaging="True" PageSize="10" ShowFooter="true"
        onpageindexchanging="grdInvoiceList_PageIndexChanging" 
        onrowdatabound="grdInvoiceList_RowDataBound">
 <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
            <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" >
                   <HeaderTemplate>
                       <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="javascript:SelectheaderCheckboxes(this);"/>
                   </HeaderTemplate>
                   <ItemTemplate>
                        <asp:CheckBox ID="ChkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Origin" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblOrigin" runat="server" Text='<%# Eval("Origin") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Agent Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentName" runat="server" Text='<%# Eval("AgentName") %>' ></asp:Label >
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
                
                <asp:TemplateField HeaderText="Invoice Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("InvoiceDt") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Invoice Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" Visible="true" >
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# Eval("InvoiceAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                       <asp:Label ID="lblGTotalInvoiceAmount" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                    <FooterStyle HorizontalAlign="Center" Font-Bold="true" />
                </asp:TemplateField>
                
                
                <asp:TemplateField HeaderText="Collected Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" visible="true" >
                    <ItemTemplate>
                       <asp:Label ID="lblCollectionAmount" runat="server" Text='<%# Eval("CollectedAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <FooterTemplate>
                       <asp:Label ID="lblGTotalCollectionAmount" runat="server" />
                    </FooterTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                    <FooterStyle HorizontalAlign="Center" Font-Bold="true" />
                </asp:TemplateField>
                
                
                <asp:TemplateField HeaderText="Invoice Balance" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" Visible="true">
                <ItemTemplate>
                    <asp:Label ID="lblInvoiceBalance" runat="server" Text='<%# Eval("PendingAmount") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label ID="lblGTotalInvoiceBalance" runat="server" />
                </FooterTemplate>
                <HeaderStyle Wrap="True"></HeaderStyle>
                   <ItemStyle Wrap="False" />
                   <FooterStyle HorizontalAlign="Center" Font-Bold="true" />
               </asp:TemplateField>
                
               <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceStatus" runat="server" Text='<%# Eval("InvoiceStatus") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>  
                   
                
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
                            OnClientClick="javascript:CallPrint('divPrint')" onclick="btnPrint_Click" />
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
                    </td>
                    <td>
                        <asp:Button ID="btnCloseInvoice" Visible="false" runat="server" 
                            Text="Close Invoice" CssClass="button" onclick="btnCloseInvoice_Click" />
                        <asp:ConfirmButtonExtender ID="CBECloseInvoice" runat="server" 
                        ConfirmText="Are you sure you want to close selected invoices?" Enabled="True" 
                        TargetControlID="btnCloseInvoice" ConfirmOnFormSubmit="false" >
                        </asp:ConfirmButtonExtender>
                    </td>
                    <td>
                            <asp:Button ID="btnExportERP" runat="server" CssClass="button"
                             Text="Export To ERP" onclick="btnExportERP_Click"  Visible="false"/> 
                        <asp:Button ID="btnCASS" Visible="false" runat="server" 
                            Text="CASS- Billing Participant Export" CssClass="button"  
                             onclick="btnCASS_Click" Width="175px" />
                        <asp:Button ID="btnCASSHandOffFile" Visible="false" runat="server" 
                            Text="CASS- Hand Off Export" CssClass="button" Width="150px" onclick="btnCASSHandOffFile_Click" 
                              />
                        <asp:Button ID="btnCASSStandardDocs"  runat="server" 
                            Text="CASS- Std Documents Export" CssClass="button" Width="185px" onclick="btnCASSStandardDocs_Click" Visible="false" 
                              />
                              
                        <asp:Button ID="btnSSIM"  runat="server" 
                            Text="SSIM" CssClass="button"  
                            Width="119px" onclick="btnSSIM_Click" Visible="false" />
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
