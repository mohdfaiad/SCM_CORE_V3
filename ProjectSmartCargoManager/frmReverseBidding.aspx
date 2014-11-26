<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReverseBidding.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmReverseBidding" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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
		
        .style1
        {
            width: 78px;
        }
        .style4
        {
            width: 625px;
        }
        .style6
        {
            width: 78px;
            height: 32px;
        }
        .style7
        {
            height: 32px;
        }
        		
    </style>

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }
     
     </script>
     
    <script type="text/javascript">
        

        function GetAgentCode() {
            var level = 'AgentCode';
            var TxtOriginClientObject = '<%=TXTAgentCode.ClientID %>';
            var TxtAgentName = '<%=txtAgentName.ClientID %>'
            window.open('ListDataView.aspx?Parent=AgentCode&level=' + level + '&TargetTXT=' + TxtOriginClientObject + '&AName=' + TxtAgentName, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
            return false;
        }
        function checkValue() {

            document.getElementById('<%=btnBookNow.ClientID %>').disabled = true;
//            document.getElementById('<%=btnSave.ClientID %>').disabled = false;
            
        }
        
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</asp:ToolkitScriptManager>
    
    
    
    <div id="contentarea" style="height:410px">
        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
    <table>
    <tr>
    <td class="style4">
    
     <div id="divFlightDetails">
        <h1>New Agent Quote</h1>
        <%--<asp:Label ID="Label1" runat="server" Text="Flight Details"></asp:Label>--%>
        <table>
        <tr>
        <td>Agent QuoteID
        </td>
        <td>
        <asp:TextBox ID="txtAgentQID" runat="server" Width="100px"></asp:TextBox>&nbsp;&nbsp;&nbsp; 
            <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
                onclick="btnList_Click" />&nbsp;&nbsp;&nbsp; 
                <asp:Button ID="btnClear" CssClass="button" runat="server" Text="Clear" onclick="btnClear_Click" />
        </td>
        
        </tr>
        <tr>
        <td class="style6">
            <asp:Label ID="lblAgentCode" runat="server" Text="Agent Code"></asp:Label>
        </td>
        <td class="style7">
        <asp:TextBox ID="TXTAgentCode" runat="server" AutoPostBack="true" Width="110px" OnTextChanged="TXTAgentCode_TextChanged" TabIndex="4" CssClass="styleUpper" 
        onChange="callShow();"></asp:TextBox>
        <asp:AutoCompleteExtender ID="ACEAgentCode" runat="server" ServiceMethod="GetAgentCode"
        CompletionInterval="0" EnableCaching="false" CompletionSetCount="10" TargetControlID="TXTAgentCode" MinimumPrefixLength="1">
        </asp:AutoCompleteExtender>
         <asp:ImageButton ID="IBOrigin" runat="server" ImageUrl="~/Images/list_bullets.png"
              ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />
        </td>
        <td class="style7">
            <asp:Label ID="lblAgentName" runat="server" Text="Agent Name"></asp:Label>
        </td>
        <td class="style7">
        <asp:TextBox ID="txtAgentName" runat="server" Width="110px" ReadOnly="True" TabIndex="5"></asp:TextBox>
        </td>
        </tr>
        
        
            <tr>
        <td class="style1">
            <asp:Label ID="lblSource" runat="server" Text="Origin"></asp:Label>
                </td><td>
                    <asp:DropDownList ID="ddlSource" runat="server" AppendDataBoundItems="True" 
                        AutoPostBack="True" onselectedindexchanged="ddlSource_SelectedIndexChanged">
                        <asp:ListItem Text="Select"></asp:ListItem>
                    </asp:DropDownList>
                    <%--<asp:TextBoxWatermarkExtender ID="txtSource_TextBoxWatermarkExtender" WatermarkText="Origin Code" runat="server" TargetControlID="txtSource">
                                   </asp:TextBoxWatermarkExtender>
                                   <asp:AutoCompleteExtender ID="txtSource_AutoCompleteExtender" runat="server" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" TargetControlID="txtSource">
                                   </asp:AutoCompleteExtender>--%>&nbsp;</td><td>
                <asp:Label ID="lblDest" runat="server" Text="Dest"></asp:Label></td><td>
                    <asp:DropDownList ID="ddlDest" runat="server" AppendDataBoundItems="True" 
                        AutoPostBack="True" onselectedindexchanged="ddlDest_SelectedIndexChanged">
                        <asp:ListItem Text="Select"></asp:ListItem>
                    </asp:DropDownList>
                    <%--<asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1_txtDest" WatermarkText="Destination Code" runat="server" TargetControlID="txtDest">
                                   </asp:TextBoxWatermarkExtender>--%>
                    <%--<asp:AutoCompleteExtender ID="AutoCompleteExtender1_txtSource" runat="server" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True"  ServicePath="~/Home.aspx" TargetControlID="txtDest">
                                   </asp:AutoCompleteExtender>
           <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" TargetControlID="txtDest">
           </asp:AutoCompleteExtender>--%> </td></tr>
           <tr>
        <%--<td style="width: 204px">--%>
        <td class="style1">
            <asp:Label ID="lblFlightDate" runat="server" Text="Flight Date"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtFlightDate" runat="server" Width="110px" OnTextChanged="txtFlightDate_Changed" AutoPostBack="true"  
                    ></asp:TextBox>
                <asp:CalendarExtender ID="txtFlightDate_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="txtFlightDate" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
        </td><td >
            <asp:Label ID="lblFlight" runat="server" Text="FlightID"></asp:Label></td><td>
            <%--<asp:DropDownList ID="ddlFlightPrefix" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="ddlFlightPrefix_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                   <asp:DropDownList ID="ddlFlightPrefix" runat="server" 
                       onselectedindexchanged="ddlFlightPrefix_SelectedIndexChanged">
                   </asp:DropDownList>
&nbsp;
                <asp:DropDownList ID="ddlFlight" runat="server" AutoPostBack="false" 
                    onselectedindexchanged="ddlFlight_SelectedIndexChanged">
                    <asp:ListItem Text="Select"></asp:ListItem></asp:DropDownList></td></tr>
           <tr>
        <td class="style1">
            &nbsp;</td><td>
            &nbsp;</td><td>
                <asp:Label ID="lblDeprtTime" runat="server" Text="Dept Time" Visible="false"></asp:Label></td><td>
                <asp:TextBox ID="txtDeptTime" runat="server" ReadOnly="true" Width="50px" 
                        Visible="false"></asp:TextBox></td></tr></table><hr />
    </div>
    
     <div id="divBookingWeight">
    
        <asp:Panel ID="pnlBookingWeight" runat="server" >
        <h2>Shipment Summary</h2><%--<asp:Label ID="Label2" runat="server" Text="Shipment Summary"></asp:Label>--%>
        <table>
        <tr>
        <td>
            <asp:Label ID="lblComodity" runat="server" Text="Comodity Code"></asp:Label>
            &nbsp;
            <asp:DropDownList ID="ddlComodityCd" runat="server" AppendDataBoundItems="True" Width="85px" >
            </asp:DropDownList>
            </td><td>
                &nbsp;&nbsp;
            <asp:Label ID="lblBookingWgt" runat="server" Text="Weight to Book"></asp:Label>&nbsp; <asp:TextBox ID="txtBookingWgt" runat="server" Width="70px" onchange="checkValue(this);"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;Pieces to Book&nbsp;<asp:TextBox ID="txtPCS" runat="server" Width="70px" onchange="checkValue(this);"></asp:TextBox> &nbsp;&nbsp;<asp:Button ID="btnCheck" CssClass="button" runat="server" Text="Check" 
                onclick="btnCheck_Click" Visible="false" />
        </td>
        <td>
            <asp:Image ID="imgValidBkgWgt" runat="server" ImageUrl="~/Images/Check.jpg" Visible="false"/>
            <asp:Image ID="imgInvalidBkgWgt" runat="server" ImageUrl="~/Images/Cross.jpg" Visible="false"/>
        </td>
        </tr>
        </table>
        <hr />
        </asp:Panel>
     </div>
    
     <div id="divBidRate">
        <asp:Panel ID="pnlBidRate" runat="server" >
            <h2>Quote Value</h2><%--<asp:Label ID="Label3" runat="server" Text="Bid Value"></asp:Label>--%>
     <table>
        <tr>
        <td>
            <asp:Label ID="lblBidPrice" runat="server" Text="Quote Price"></asp:Label>&nbsp; <asp:TextBox ID="txtBidPrice" runat="server" Width="70px" onchange="checkValue(this);"></asp:TextBox></td><td>
                &nbsp;&nbsp;
            <asp:Button ID="btnBidRate" CssClass="button" runat="server" Text="Quote" 
                onclick="btnBidRate_Click" Visible="false" />
        </td>
        <td>
            <asp:Image ID="imgBidAproved" runat="server" ImageUrl="~/Images/Check.jpg" Visible="false"/>
            <asp:Image ID="imgBidRejected" runat="server" ImageUrl="~/Images/Cross.jpg" Visible="false"/>
        </td>
        </tr>
     </table>
     <hr />
        </asp:Panel>
     </div>
    <div id="divButtons">
    <asp:Button ID="btnSave" CssClass="button" runat="server" Text="Save" onclick="btnSave_Click" />
    
    <asp:Button ID="btnBookNow" CssClass="button" runat="server" Text="Book Now" 
            Visible="true" onclick="btnBookNow_Click" />
    
    
    </div>
     
    
     </td>
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
    <asp:Label ID="msgshow" runat="server" ></asp:Label></td></tr></table></div>
    <div id="msgfade" class="black_overlaymsg"></div>
	
    
        
    </asp:Content>
