<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="rptDailySalesReport.aspx.cs" Inherits="ProjectSmartCargoManager.rptDailySalesReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM0" runat="server">
 
 </asp:ToolkitScriptManager>
     
 <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            //document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function DisableButton() {
            document.getElementById("<%=btnList.ClientID %>").disabled = true;
            document.getElementById("<%=btnClear.ClientID %>").disabled = true;
            document.getElementById("<%=btnExport.ClientID %>").disabled = true;
        }
        window.onbeforeunload = DisableButton;
        
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }

        function GetShipperCode(obj) {
            var destination = obj.value;

            var objString = destination.split("|");
            var AccountCode = objString[0];
            if (objString.length > 1) {

                obj.value = AccountCode;

            }
        }
        function onCommListPopulated() {

            var completionList = $find("ACECommCode").get_completionList();
            completionList.style.width = 'auto';
        }

        function onShipperListPopulated() {

            var completionList = $find("ACESHPCode").get_completionList();
            completionList.style.width = 'auto';
        }
        function onSHCListPopulated() {

            var completionList = $find("ACESHCCode").get_completionList();
            completionList.style.width = 'auto';
        }
        function onAgentListPopulated() {

            var completionList = $find("ACEAgentCode").get_completionList();
            completionList.style.width = 'auto';
        }
        
        function GetCommodityCode(obj) {
            var destination = obj;

            if (destination.value.indexOf("(") > 0) {
                //                if (destination.value.length > 4) {
                var str = destination.value;
                var start = destination.value.indexOf("(");
                obj.value = str.substring(0, start);
            }
        }
        function GetSHCCode(obj) {
            var destination = obj;

            if (destination.value.indexOf("(") > 0) {
                //                if (destination.value.length > 4) {
                var str = destination.value;
                var start = destination.value.indexOf("(");
                obj.value = str.substring(0, start);
            }
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
          
      .hrtime div{ display: inline-table; padding-right:5px !important;}
    </style>
    
     
 <div id="contentarea">
 <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
 <h1>Daily Sales Report</h1>

<div class="botline">
<table width="100%" cellpadding="3" cellspacing="3"><tr><td>
<table width="80%">
<tr>

<td>From Date</td>
<td>

    <asp:TextBox ID="txtFromdate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFromdate" PopupButtonID="imgfromdate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgfromdate" runat="server" ImageAlign="AbsMiddle" 
                        ImageUrl="~/Images/calendar_2.png" />
</td>
                        
<td>
                         <div class="hrtime">
                                 <asp:TextBox ID="txtFromTimeHr" runat="server" DataTextField="" Width="50px"></asp:TextBox>
               
                      <asp:TextBox ID="txtFromTimeMin" runat="server" DataTextField="" Width="50px"></asp:TextBox>(HR:MI)
                                         
                    <asp:NumericUpDownExtender ID="txtFromTimeHr_NumericUpDownExtender" 
                                        runat="server" Maximum="23" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtFromTimeHr" Width="40"  />
                                        
                    <asp:NumericUpDownExtender ID="txtFromTimeMin_NumericUpDownExtender1" 
                                        runat="server" Maximum="59" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtFromTimeMin" Width="40" />
                                        </div>
</td>
<td>To Date</td>
<td><asp:TextBox ID="txtTodate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"  
                        Enabled="True" TargetControlID="txtTodate" PopupButtonID="imgtodate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgtodate" runat="server" ImageAlign="AbsMiddle" 
                        ImageUrl="~/Images/calendar_2.png" /> </td>
<td>
                        <div class="hrtime">
                     <asp:TextBox ID="txtToTimeHr" runat="server" DataTextField="" Width="50px"></asp:TextBox>
                     <asp:TextBox ID="txtToTimeMin" runat="server" DataTextField="" Width="50px"></asp:TextBox>(HR:MI)
                      
                     <asp:NumericUpDownExtender ID="txtToTimeHr_NumericUpDownExtender" 
                                        runat="server" Maximum="23" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtToTimeHr" Width="40" />
                                        
                     <asp:NumericUpDownExtender ID="txtToTimeMin_NumericUpDownExtender" 
                                        runat="server" Maximum="59" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtToTimeMin" Width="40" /></div>
                                        </td>

                
                </tr>
 </table>
 </td></tr>
 <tr><td>
 
<table width="100%" cellpadding="3" cellspacing="3">           
<tr>
<td>
    Station</td>
<td>
    <asp:DropDownList ID="ddlStation" runat="server">
    </asp:DropDownList>
</td>

<td>Carrier </td>
<td><asp:DropDownList ID="ddlCarrier" runat="server">
 </asp:DropDownList> </td>

<td>
    Shipper</td>
    <td><asp:TextBox ID="txtShipperCode" runat="server" MaxLength="50" TabIndex="200" Width="77px" onchange="GetShipperCode(this);" 
                                         AutoPostBack="false"> </asp:TextBox>
                                         
   <asp:AutoCompleteExtender ID="ACESHPCode" runat="server" BehaviorID="ACESHPCode"
   CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
   MinimumPrefixLength="1" ServiceMethod="GetShipperCode" 
TargetControlID="txtShipperCode" OnClientPopulated="onShipperListPopulated" FirstRowSelected="true">
</asp:AutoCompleteExtender>
                                         </td>
<td>Commodity</td>
<td>
    <asp:TextBox ID="txtCommodityCode" runat="server" Width="110px" CssClass="styleUpper"
                                            onchange="GetCommodityCode(this);" TabIndex="120"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="ACECommCode" BehaviorID="ACECommCode" runat="server"
                                                        ServiceMethod="GetCommodityCodesWithName" CompletionInterval="0" EnableCaching="false"
                                                        CompletionSetCount="10" TargetControlID="txtCommodityCode" MinimumPrefixLength="1"
                                                        OnClientPopulated="onCommListPopulated" FirstRowSelected="true">
                                                    </asp:AutoCompleteExtender>
    </td>
</tr>
<tr>
<td>
                   Flights
                </td>
<td><asp:DropDownList ID="ddlStationType" runat="server">
                        <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem Value="DOM">Domestic</asp:ListItem>
                        <asp:ListItem Value="INT">International</asp:ListItem>
                    </asp:DropDownList>
    </td>
    
    <td>
    DSR Type
    </td>
    
    <td><asp:DropDownList ID="ddlDSRType" runat="server">
    <asp:ListItem>Accepted</asp:ListItem>
    <asp:ListItem>Invoiced</asp:ListItem>
    <asp:ListItem>Not Departed</asp:ListItem>
 </asp:DropDownList>
        
                                         </td>
    <td>SHC</td>
   
    <td><asp:TextBox ID="txtSHCCode" runat="server" MaxLength="50" 
            TabIndex="200" Width="77px" onchange="GetSHCCode(this);" 
                                         AutoPostBack="false"> </asp:TextBox>
                                         
   <asp:AutoCompleteExtender ID="txtSHCCode_AutoCompleteExtender" runat="server" BehaviorID="ACESHCCode"
   CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
   MinimumPrefixLength="1" ServiceMethod="GetSHCCode" 
TargetControlID="txtSHCCode" OnClientPopulated="onSHCListPopulated" 
            FirstRowSelected="true">
</asp:AutoCompleteExtender></td> 
<td>Agent</td>
    <td>
    <asp:TextBox ID="txtAgentCode" runat="server" Width="110px" CssClass="styleUpper"
                                            onchange="GetCommodityCode(this);" 
            TabIndex="120"></asp:TextBox>
              <asp:AutoCompleteExtender ID="txtAgentCode_AutoCompleteExtender" 
           runat="server" BehaviorID="ACEAgentCode"
                                                        
            ServiceMethod="GetAgentCodesWithName" CompletionInterval="0" EnableCaching="false"
                                                        CompletionSetCount="10" 
            TargetControlID="txtAgentCode" MinimumPrefixLength="1"
                                                        
            OnClientPopulated="onAgentListPopulated" FirstRowSelected="true">
                                                    </asp:AutoCompleteExtender>
    </td>
    <td>&nbsp;</td>
    </tr>
</table></td></tr>
<tr><td><asp:Button ID="btnList" runat="server" Text="List" CssClass="button" onclick="btnList_Click" />
  <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" 
        />
      
        <asp:Button ID="btnExport" runat="server" CssClass="button"  
                        Text="Export" onclick="btnExport_Click"   />
</td></tr>
</table>

</div>
 

<div style="border: thin solid #000000; float:left">
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="1024px">
    </rsweb:ReportViewer>
 
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
