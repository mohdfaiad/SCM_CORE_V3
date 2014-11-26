<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAgentCapacityNew.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmAgentCapacityNew" %>

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
		
        .style7
        {
            width: 64px;
        }
        		
        .style8
        {
            width: 142px;
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
        
        function checkval() {

            var t = 1.00;
            //            document.getElementById("clock").value = 3.00;
            //alert(document.getElementById("clock").innerHTML);
            //alert(document.getElementById("<%= clock.ClientID %>").value);
        }
        var int = self.setInterval(function() { clock(document.getElementById("<%= clock.ClientID %>").value) }, 1000);
        function clock(t) {

            //alert(t);
            var s = parseFloat(t) - parseFloat(0.01);
            s = parseFloat(s).toFixed(2);
            //alert(s);
            if (s.substr(2, 2) == "99") {

                s = s.replace("99", "59");
            }
            document.getElementById("<%= clock.ClientID %>").value = s;
            if (s == "0.00") {
                window.clearInterval(int);
                document.location.reload(true);
            }
        }

        function ChkHAWBCount(val) {
            alert(val.id);
            alert(val.checked);
            var ctrlname1 = val.id.replace('chkSelect', 'txtWeight');
            var ctrlname2 = val.id.replace('chkSelect', 'txtRate');
            var ctrlname3 = val.id.replace('chkSelect', 'txtCurrency');
            if (val.id.checked == true) {
                alert(ctrlname1);
                alert(ctrlname2);
                alert(ctrlname3);
                
                document.getElementById(ctrlname1).disabled = false;
                document.getElementById(ctrlname2).disabled = false;
                document.getElementById(ctrlname3).disabled = false;
            }
            else {
                document.getElementById(ctrlname1).disabled = true;
                document.getElementById(ctrlname2).disabled = true;
                document.getElementById(ctrlname3).disabled = true;
            }
//            var grid = document.getElementById("<%= grdWeekDay.ClientID %>");
//            alert(grid.id);
            //            var radioButton = document.getElementById(grid.id + '_ctl' + rowIndex + '_check');

        }
        function putvalue(ddl) {
            var strUser = ddl.value;
            alert(strUser);
            document.getElementById("<%= txtDeptTime.ClientID %>").value = strUser;
        }

        function GetAgentCode() {
            var level = 'AgentCode';
            var TxtOriginClientObject = '<%=TXTAgentCode.ClientID %>';
            var TxtAgentName = '<%=txtAgentName.ClientID %>'
            window.open('ListDataView.aspx?Parent=AgentCode&level=' + level + '&TargetTXT=' + TxtOriginClientObject + '&AName=' + TxtAgentName, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
            return false;
        }
        
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate >
    
    <div id="contentarea">  <%--style="height:480px">--%>
        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
               ForeColor="Red"></asp:Label>
               <div class="botline">
    <table>
    <tr>
    <td style="width: 482px">
    
     <div id="divFlightDetails">
        <h1>Allocate Agent Capacity</h1>
        <%--<asp:Label ID="Label1" runat="server" Text="Flight Details"></asp:Label>--%>
        <table>
        <tr>
        <td >
            <asp:Label ID="lblAgentCode" runat="server" Text="Agent Code"></asp:Label>
        </td>
        <td >
        <asp:TextBox ID="TXTAgentCode" runat="server" AutoPostBack="true" Width="110px" 
                OnTextChanged="TXTAgentCode_TextChanged" TabIndex="4" CssClass="styleUpper" 
                onChange="callShow();"></asp:TextBox>
        <asp:AutoCompleteExtender ID="ACEAgentCode" runat="server" ServiceMethod="GetAgentCode" ServicePath="~/frmAgentCapacityNew.aspx"
        CompletionInterval="0" EnableCaching="false" CompletionSetCount="10" TargetControlID="TXTAgentCode"
        MinimumPrefixLength="1">
        </asp:AutoCompleteExtender>
        <asp:ImageButton ID="IBOrigin" runat="server" ImageUrl="~/Images/list_bullets.png"
                            ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />
        </td>
        <td>
            <asp:Label ID="lblAgentName" runat="server" Text="Agent Name"></asp:Label>
        </td>
        <td>
        <asp:TextBox ID="txtAgentName" runat="server" Width="110px" ReadOnly="True" TabIndex="5"></asp:TextBox>
        </td>
        
        <td>
            <asp:Label ID="lblOrigin" runat="server" Text="Origin"></asp:Label>
            </td>
            <td >
            <asp:DropDownList ID="ddlSource" runat="server" AppendDataBoundItems="True">
            <asp:ListItem Text="Select"></asp:ListItem>
            </asp:DropDownList>
            <%--<asp:TextBoxWatermarkExtender ID="txtSource_TextBoxWatermarkExtender" WatermarkText="Origin Code" runat="server" TargetControlID="txtSource">
                                   </asp:TextBoxWatermarkExtender>
                                   <asp:AutoCompleteExtender ID="txtSource_AutoCompleteExtender" runat="server" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" TargetControlID="txtSource">
                                   </asp:AutoCompleteExtender>--%>

        </td>
        <td>
            <asp:Label ID="lblDest" runat="server" Text="Dest"></asp:Label>
            </td>
            <td>
            <asp:DropDownList ID="ddlDest" runat="server" 
                onselectedindexchanged="ddlDest_SelectedIndexChanged" 
                AppendDataBoundItems="True" AutoPostBack="True">
            <asp:ListItem Text="Select"></asp:ListItem>
            </asp:DropDownList>
            <%--<asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1_txtDest" WatermarkText="Destination Code" runat="server" TargetControlID="txtDest">
                                   </asp:TextBoxWatermarkExtender>--%>
                                   <%--<asp:AutoCompleteExtender ID="AutoCompleteExtender1_txtSource" runat="server" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True"  ServicePath="~/Home.aspx" TargetControlID="txtDest">
                                   </asp:AutoCompleteExtender>
           <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" TargetControlID="txtDest">
           </asp:AutoCompleteExtender>--%>
        </td>
        </tr>
        <tr>
        <%--<td style="width: 204px">--%>
        <td  >
            <asp:Label ID="lblFlight" runat="server" Text="FlightID"></asp:Label>
            </td>
            <td>
            <asp:DropDownList ID="ddlFlight" runat="server" AutoPostBack="True" 
                onchange ="putvalue(this)" 
                    onselectedindexchanged="ddlFlight_SelectedIndexChanged">
            <asp:ListItem Text="Select"></asp:ListItem>
            </asp:DropDownList>
                <asp:DropDownList ID="ddlFlightNumber" runat="server">
                </asp:DropDownList>
        </td>
        <td  >
            <asp:Label ID="lblDeprtTime" runat="server" Text="Dept Time"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtDeptTime" runat="server" ReadOnly = "true" Width="50px"></asp:TextBox>
        </td>
       
        <td>
            <asp:Label ID="lblFlightDate" runat="server" Text="From Date"></asp:Label>
        </td>
        <td >
            <asp:TextBox ID="txtFromDate" runat="server" Width="110px"></asp:TextBox>
            <asp:CalendarExtender ID="txtFlightFromdate_CalendarExtender" runat="server"  Format="MM/dd/yyyy"
                  Enabled="True" TargetControlID="txtFromDate" PopupButtonID="imgAWBFromDt">
              </asp:CalendarExtender>
              <asp:ImageButton ID="imgAWBFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
            
        </td>
        <td >
            <asp:Label ID="Label1" runat="server" Text="To Date"></asp:Label>
        </td>
        <td >
            <asp:TextBox ID="txtToDate" runat="server" Width="110px"></asp:TextBox>
            <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Format="MM/dd/yyyy"
                Enabled="True" TargetControlID="txtToDate" PopupButtonID="imgAWBToDt">
            </asp:CalendarExtender>
            <asp:ImageButton ID="imgAWBToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
            
        </td>
        </tr>
            <tr>
                <td colspan="12" >
                <asp:Label ID="Label2" runat="server" Text="Comodity Code"></asp:Label>
                    <asp:DropDownList ID="ddlComodityCd" runat="server" AppendDataBoundItems="True" >
                        
            <asp:ListItem Text="Select"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
    </table>
    <hr />
    <h2>Blocked Capacity</h2>
         <asp:Label ID="lblDay" runat="server" Text="Day Of Week"></asp:Label>
         
         <asp:CheckBoxList ID="chkWeekDays" runat="server" RepeatDirection="Horizontal" Visible="false">
         <asp:ListItem>Sun</asp:ListItem>
         <asp:ListItem>Mon</asp:ListItem>
         <asp:ListItem>Tue</asp:ListItem>
         <asp:ListItem>Wed</asp:ListItem>
         <asp:ListItem>Thi</asp:ListItem>
         <asp:ListItem>Fri</asp:ListItem>
         <asp:ListItem>Sat</asp:ListItem>
         </asp:CheckBoxList>
         
    <asp:GridView ID="grdWeekDay" runat="server" AutoGenerateColumns="False" Height="82px" Width="100%">
    <Columns>
 
    <asp:TemplateField>
     <ItemTemplate>  
     <%--<asp:RadioButton ID="rbSelect" runat="server" onclick="javascript:SingleSelection(this, 'rbSelect');return true;" />--%>
     <asp:CheckBox ID="chkSelect" runat="server" OnCheckedChanged="chkSelect_CheckChanged" AutoPostBack="true"/>  <%--onClick="ChkHAWBCount(this);"--%>
     </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Day Of Week" >
    <ItemTemplate>
    <asp:Label ID="lblDayOfWeek" runat="server"  Text='<%# Eval("DayOfWeek") %>' Width="50px"></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Weight">
    <ItemTemplate>
    <%--<asp:Label ID="lblAgenCode" runat="server"  Text='<%# Eval("AgentCode") %>' Width="200px"></asp:Label>--%>
    <asp:TextBox ID="txtWeight" runat="server" Width="100px" Enabled="false"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="No Show Rate/Kg">
    <ItemTemplate>
    <%--<asp:Label ID="lblFlight" runat="server"  Text='<%# Eval("FlightNo") %>' Width="100px"></asp:Label>--%>
    <asp:TextBox ID="txtRate" runat="server" Width="100px" Enabled="false"></asp:TextBox>
    </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Currency" Visible="false">
    <ItemTemplate>
    <%--<asp:Label ID="lblFromDt" runat="server"  Text='<%# Eval("FromDate") %>' Width="100px"></asp:Label>--%>
    <asp:TextBox ID="txtCurrency" runat="server" Width="100px" Text="NOK" Enabled="false"></asp:TextBox>
    
    </ItemTemplate>
    </asp:TemplateField>
    
    </Columns>
    <HeaderStyle CssClass="titlecolr" />
    <RowStyle HorizontalAlign="Center" />
    <AlternatingRowStyle HorizontalAlign="Center" />
   </asp:GridView>
    
    </div>
    
     <div id="divBookingWeight">
    
        <asp:Panel ID="pnlBookingWeight" runat="server">
        
            <%--<asp:Label ID="Label2" runat="server" Text="Shipment Summary"></asp:Label>--%>
        <table>
        <tr>
        <td>
            <%--<asp:Label ID="lblComodity" runat="server" Text="Comodity Code"></asp:Label>
            <asp:DropDownList ID="ddlComodityCd" runat="server" AppendDataBoundItems="True">
            <asp:ListItem Text="Select"></asp:ListItem>
            </asp:DropDownList>--%>
            </td>
        <td>
            <asp:Label ID="lblBookingWgt" runat="server" Text="Weight to Book (Kg)" Visible="false"></asp:Label>
            <asp:TextBox ID="txtBookingWgt" runat="server" Width="70px" Visible="false"></asp:TextBox>
            <asp:Button ID="btnCheck" CssClass="button" runat="server" Text="Check" 
                onclick="btnCheck_Click" Visible="False" />
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
        <asp:Panel ID="pnlBidRate" runat="server" Visible="false">
            <h2>Agent Rate</h2>
            <%--<asp:Label ID="Label3" runat="server" Text="Bid Value"></asp:Label>--%>
     <table>
        <tr>
        <td>
            <asp:Label ID="lblBidPrice" runat="server" Text="Rate Per Kg"></asp:Label>
            <asp:TextBox ID="txtBidPrice" runat="server" Width="70px"></asp:TextBox>
        </td>
        <td>
            <asp:Button ID="btnBidRate" CssClass="button" runat="server" Text="Quote" 
                onclick="btnBidRate_Click" OnClientClick="checkval();" Visible="False"/>
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
    
     <div id="divBookTimer">
     
     <asp:Panel ID="pnlRelease" runat="server" Visible="True">
         Release Capacity
         &nbsp;&nbsp;
         <asp:DropDownList ID="ddlTime" runat="server" ToolTip="If Release Duration is 00:00 and Agent does not provide commited cargo then Agent will be charged for Complete Blocked Space." >
         <asp:ListItem>00:00</asp:ListItem>
         <asp:ListItem>00:30</asp:ListItem>
         <asp:ListItem>01:00</asp:ListItem>
         <asp:ListItem>01:30</asp:ListItem>
         <asp:ListItem>02:00</asp:ListItem>
         <asp:ListItem>02:30</asp:ListItem>
         <asp:ListItem>03:00</asp:ListItem>
         <asp:ListItem>03:30</asp:ListItem>
         <asp:ListItem>04:00</asp:ListItem>
         <asp:ListItem>04:30</asp:ListItem>
         <asp:ListItem>05:00</asp:ListItem>
         <asp:ListItem>05:30</asp:ListItem>
         <asp:ListItem>06:00</asp:ListItem>
         <asp:ListItem>06:30</asp:ListItem>
         <asp:ListItem>07:00</asp:ListItem>
         <asp:ListItem>07:30</asp:ListItem>
         <asp:ListItem>08:00</asp:ListItem>
         <asp:ListItem>08:30</asp:ListItem>
         <asp:ListItem>09:00</asp:ListItem>
         <asp:ListItem>09:30</asp:ListItem>
         <asp:ListItem>10:00</asp:ListItem>
         <asp:ListItem>10:30</asp:ListItem>
         <asp:ListItem>11:00</asp:ListItem>
         <asp:ListItem>11:30</asp:ListItem>
         <asp:ListItem>12:00</asp:ListItem>
         </asp:DropDownList>
         &nbsp;&nbsp;(HH:mm) before Departure <br />
         <%--(If Release Duration is 00:00, Automated Billing for Blocked Space will be triggered if Agent does not provide commited cargo.)--%></asp:Panel>
        <asp:Panel ID="pnlBookTimer" runat="server" Visible="False">
        <h2></h2>
     <table width="100">
        <tr>
        <td>
            <asp:Label ID="lblRateValid" runat="server" Text="Rate Valid For Next "></asp:Label>
            <asp:TextBox ID="clock" runat="server" Text="03:00" ReadOnly="true" Width="50px"></asp:TextBox>
            <asp:Label ID="lblMin" runat="server" Text="Min"></asp:Label>
        </td>
        <td>
            <asp:Button ID="btnBookNow" CssClass="button" runat="server" Text="Book Now" />
        </td>
        </tr>
     </table>
     <hr />
     </asp:Panel>
     </div>
    
     </td>
    </tr>    
    </table>
    </div>
    
    </div>
    
    <div id="divButtons">
    
    <asp:Button ID="btnSave" CssClass="button" runat="server" Text="Save" onclick="btnSave_Click" Visible="True" />
    <asp:Button ID="btnClear" CssClass="button" runat="server" Text="Clear" onclick="btnClear_Click" />
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
	
    <div id="msgfade" class="black_overlaymsg">
    </div>
	
    </ContentTemplate>
    </asp:UpdatePanel>
        
    </asp:Content>