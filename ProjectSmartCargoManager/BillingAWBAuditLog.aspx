<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillingAWBAuditLog.aspx.cs" Inherits="ProjectSmartCargoManager.BillingAWBAuditLog" MasterPageFile="~/SmartCargoMaster.Master"%>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:ToolkitScriptManager ID="TSM" runat="server">
</asp:ToolkitScriptManager>

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
        .style2
    {
        width: 36px;
    }
    </style>
 
<div id="contentarea">
<div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
<h1>Billing Audit Log</h1>



<div class="botline">

<table>
<tr>
    <td class="style2">AWB#</td>
   <td>
        <asp:TextBox ID="txtAWBPrefix" runat="server" Width="40px"></asp:TextBox>
        <asp:TextBoxWatermarkExtender ID="txtAWBPrefix_WaterMark" runat="server" TargetControlID="txtAWBPrefix" WatermarkText="Prefix">
        </asp:TextBoxWatermarkExtender>
        
        <asp:TextBox ID="txtAWBNo" runat="server" Width="80px"></asp:TextBox>
        <asp:TextBoxWatermarkExtender ID="txtAWBNo_WaterMark" runat="server" TargetControlID="txtAWBNo" WatermarkText="Number">
        </asp:TextBoxWatermarkExtender> 
    </td>
    
    
</tr>
<tr>
<td class="style2">
        <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" onclick="btnList_Click" /></td>
        <td>
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" />
        &nbsp;<asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" 
            onclick="btnExport_Click" />
    </td>
</tr>
</table>

</div>

<div class="ltfloat">

    <asp:GridView ID="grdBillingLog" runat="server" 
        AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" 
     HeaderStyle-CssClass="titlecolr"  PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
     SelectedRowStyle-CssClass="SelectedRowStyle" AllowPaging="true" PageSize="10" 
        AutoGenerateColumns="false" 
        onpageindexchanging="grdBillingLog_PageIndexChanging">
     
     <Columns>
        <asp:TemplateField HeaderText="AWB No">
            <ItemTemplate>
                <asp:Label ID="lblAWBNo" runat="server" Text='<%#Eval("AWBNumber")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Origin">
            <ItemTemplate>
                <asp:Label ID="lblOrg" runat="server" Text='<%#Eval("Origin")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Destination">
            <ItemTemplate>
                <asp:Label ID="lblDest" runat="server" Text='<%#Eval("Destination")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Flt #">
            <ItemTemplate>
                <asp:Label ID="lblFlightNo" runat="server" Text='<%#Eval("FlightNo")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Flt Date">
            <ItemTemplate>
                <asp:Label ID="lblFlightDate" runat="server" Text='<%#Eval("FlightDate")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Agent">
            <ItemTemplate>
                <asp:Label ID="lblAgent" runat="server" Text='<%#Eval("AgentCode")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Status">
            <ItemTemplate>
                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Ref Number">
            <ItemTemplate>
                <asp:Label ID="lblRefNumber" runat="server" Text='<%#Eval("RefNumber")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Freight">
            <ItemTemplate>
                <asp:Label ID="lblFrt" runat="server" Text='<%#Eval("FreightMKT")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="OCDC">
            <ItemTemplate>
                <asp:Label ID="lblOCDC" runat="server" Text='<%#Eval("OCDueCar")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="OCDA">
            <ItemTemplate>
                <asp:Label ID="lblOCDA" runat="server" Text='<%#Eval("OCDueAgent")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Service Tax">
            <ItemTemplate>
                <asp:Label ID="lblServTax" runat="server" Text='<%#Eval("ServiceTax")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Total">
            <ItemTemplate>
                <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("FinalAmt")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Date">
            <ItemTemplate>
                <asp:Label ID="lblUpdatedOn" runat="server" Text='<%#Eval("UpdatedOn")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Operator">
            <ItemTemplate>
                <asp:Label ID="lblUpdatedBy" runat="server" Text='<%#Eval("UpdatedBy")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
     </Columns>
    </asp:GridView>

    <br />
<div style="overflow-x: auto;width: 1000px;">
    <asp:GridView ID="grdCCALog" runat="server" 
        AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" 
     HeaderStyle-CssClass="titlecolr"  PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
     SelectedRowStyle-CssClass="SelectedRowStyle" AllowPaging="true" PageSize="10" 
        AutoGenerateColumns="false" 
        onpageindexchanging="grdCCALog_PageIndexChanging">
     
<RowStyle CssClass="RowStyle"></RowStyle>
     
     <Columns>
        <asp:TemplateField HeaderText="InvoiceNo">
            <ItemTemplate>
                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Eval("InvoiceNumber")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="AWB No">
            <ItemTemplate>
                <asp:Label ID="lblAWBNo" runat="server" Text='<%#Eval("AWBNumber")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
         <asp:TemplateField HeaderText="Agent">
            <ItemTemplate>
                <asp:Label ID="lblAgent" runat="server" Text='<%#Eval("AgentCode")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="FlightNo">
            <ItemTemplate>
                <asp:Label ID="lblFlightNo" runat="server" Text='<%#Eval("FlightNo")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
              
        <asp:TemplateField HeaderText="FlightDate">
            <ItemTemplate>
                <asp:Label ID="lblFlightDate" runat="server" Text='<%#Eval("FlightDate")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
      
        <asp:TemplateField HeaderText="ChargableWeight">
            <ItemTemplate>
                <asp:Label ID="lblChargableWeight" runat="server" 
                    Text='<%#Eval("ChargableWeight")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="RevisedCh.Weight">
            <ItemTemplate>
                <asp:Label ID="lblRevisedChargableWeight" runat="server" 
                    Text='<%#Eval("RevisedChargableWeight")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="FreightRate">
            <ItemTemplate>
                <asp:Label ID="lblFreightRate" runat="server" Text='<%#Eval("FreightRate")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="RevisedFr.Rate">
            <ItemTemplate>
                <asp:Label ID="lblRevisedFreightRate" runat="server" 
                    Text='<%#Eval("RevisedFreightRate")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="OCDC">
            <ItemTemplate>
                <asp:Label ID="lblOCDC" runat="server" Text='<%#Eval("OCDC")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="RevisedOCDC">
            <ItemTemplate>
                <asp:Label ID="lblRevisedOCDC" runat="server" Text='<%#Eval("RevisedOCDC")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="OCDA">
            <ItemTemplate>
                <asp:Label ID="lblOCDA" runat="server" Text='<%#Eval("OCDA")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="RevisedOCDA">
            <ItemTemplate>
                <asp:Label ID="lblRevisedOCDA" runat="server" Text='<%#Eval("RevisedOCDA")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="ServiceTax">
            <ItemTemplate>
                <asp:Label ID="lblServiceTax" runat="server" Text='<%#Eval("ServiceTax")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="RevisedSr.Tax">
            <ItemTemplate>
                <asp:Label ID="lblRevisedServiceTax" runat="server" 
                    Text='<%#Eval("RevisedServiceTax")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="CurrentTotal">
            <ItemTemplate>
                <asp:Label ID="lblCurrentTotal" runat="server" 
                    Text='<%#Eval("CurrentTotal")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
          <asp:TemplateField HeaderText="RevisedTotal">
            <ItemTemplate>
                <asp:Label ID="lblRevisedTotal" runat="server" 
                    Text='<%#Eval("RevisedTotal")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
     </Columns>

<PagerStyle CssClass="PagerStyle"></PagerStyle>

<SelectedRowStyle CssClass="SelectedRowStyle"></SelectedRowStyle>

<HeaderStyle CssClass="titlecolr"></HeaderStyle>

<AlternatingRowStyle CssClass="AltRowStyle"></AlternatingRowStyle>
    </asp:GridView>
</div>
    <br />

    <br />

</div>

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

</asp:Content>