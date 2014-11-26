<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true"
    CodeBehind="GHA_ListTemplate.aspx.cs" Inherits="ProjectSmartCargoManager.ListTemplate_GHA" %>

<%-- 

  2012-07-04 vinayak
  2012-07-06  vinayak Edit/View
  
--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function checkCheckBoxes(theForm) {
            if (
    theForm.CHKinFFR.checked == true) {
                alert("test");
                theForm.CHKfailed.visible = true;
                //alert('You didn\'t choose any of the checkboxes!');
                return true;
            } else {
                return false;
                theForm.CHKinFFR.checked == false;
            }
        }

        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }
        function ViewPanelSplit() {
            document.getElementById('Lightsplit').style.display = 'block';
            document.getElementById('fadesplit').style.display = 'block';
        }
        function HidePanelSplit() {
            document.getElementById('Lightsplit').style.display = 'none';
            document.getElementById('fadesplit').style.display = 'none';
        }

        function onCommListPopulated() {

            var completionList = $find("ACECommCode").get_completionList();
            completionList.style.width = 'auto';
        }

        function GetCommodityCode(obj) {
            var destination = obj;
            
            if (destination.value.indexOf("(") > 0) {            
                var str = destination.value;
                var start = destination.value.indexOf("(");
                obj.value = str.substring(0, start);            
            }            
            return false;
        }
        function checkForm(sender, args) {
            
            var date1 = document.getElementById('<%=txtFromDate.ClientID%>').value;
            
        var date2 = document.getElementById('<%=TextBox6.ClientID%>').value;
            
            var daysBetween = (new Date(date2).getTime() - new Date(date1).getTime()) / 86400000;

            var today = new Date();

            var convertedDate = today.getMonth() + 1 + "/" + today.getDate() + "/" + today.getFullYear();

            var todayDifference = new Date(convertedDate).getTime() - new Date(date2).getTime();

            if (daysBetween > 30) {
                args.IsValid = false;
                confirm('The date range should not exceed 30 days.');
                
                document.getElementById("error").innerHTML = "The date range should not exceed 14 days.";
                return false;

            }


            args.IsValid = true;
            document.getElementById("error").style.display = "none";
            return true;
        }

        function GetAgentCode(obj) {
            
            var destination = obj;
            
            if (destination.value.indexOf("(") > 0) {
                
                var start = destination.value.indexOf("(");
                var end = destination.value.lastIndexOf(")");
                var str = destination.value;
                var CustStart = destination.value.indexOf("$");

                destination.value = str.substring(0, start);

            }
        }
        function onListPopulated() {
            
            var completionList = $find("ACEAgentCode").get_completionList();
            completionList.style.width = 'auto';
        }
        
    </script>

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
			/*height: 70%;*/
			padding: 6px;
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
		
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="uppnl" runat="server">
        <ContentTemplate>
            <div id="contentarea">
                <h1>
                    List Booking Templates
                    <%--<img alt="" src="images/txt_listbooking.png" />--%></h1>
                <asp:UpdatePanel ID="UPFourth" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Large"></asp:Label>
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="botline">
                    <asp:UpdatePanel ID="UPFirst" runat="server">
                        <ContentTemplate>
                            <table border="0" cellpadding="3" cellspacing="3" width="95%">
                                <tr>
                                  <td>
                                        Flight
                                    </td>
                                <td colspan="3">
                                        <%--<asp:DropDownList ID="ddlFlight" runat="server">
                                        </asp:DropDownList>--%>
                                        <asp:TextBox ID="txtFlightCode" runat="server" Width="45px" MaxLength="2"></asp:TextBox>&nbsp;
                                        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtFlightCode"
                                            WatermarkText="Prefix" />
                                        <asp:TextBox ID="txtFlightID" runat="server" Width="80px" MaxLength="4"></asp:TextBox>
                                        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtFlightID"
                                            WatermarkText="Flight ID" />&nbsp;
                                        <asp:TextBox ID="txtFltFromDt" runat="server" Width="85px"></asp:TextBox>
                                        <asp:ImageButton ID="imgFltFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                        <asp:CalendarExtender ID="TextBoxdate_CalendarExtender" runat="server" PopupButtonID="imgFltFromDt"
                                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFltFromDt">
                                        </asp:CalendarExtender>&nbsp;
                                        <asp:TextBox ID="txtFltToDt" runat="server" Width="85px"></asp:TextBox>
                                        <asp:ImageButton ID="imgFltToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgFltToDt"
                                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFltToDt">
                                        </asp:CalendarExtender>
                                    </td>
                                 
                                    
                                    <td>
                                        Comm Code
                                    </td>
                                    <td>
                                        <%--<asp:DropDownList ID="ddlCommCode" runat="server" Width="150px">
                                        </asp:DropDownList>--%>
                                        <asp:TextBox ID="txtCommodityCode" TabIndex="35" runat="server" Width="100px"
                                            CssClass="styleUpper" onchange="return GetCommodityCode(this);">
                                        </asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACECommCode" BehaviorID="ACECommCode" runat="server"
                                            ServiceMethod="GetCommodityCodesWithName" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="txtCommodityCode" MinimumPrefixLength="1"
                                            OnClientPopulated="onCommListPopulated">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        Origin
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSource" runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Destination
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDest" runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        User
                                    </td>
                                    <td colspan="3">
                                     <asp:DropDownList ID="drpUsers" runat="server" Width="150px">
                                        </asp:DropDownList>&nbsp;
                                       
                                    </td>
                                    <td>
                                        Agent Code
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTAgentCode" runat="server" Width="100px" TabIndex="55" CssClass="styleUpper"
                                            onchange="GetAgentCode(this);"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACEAgentCode" BehaviorID="ACEAgentCode" runat="server"
                                            ServiceMethod="GetAgentCodeWithName" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="TXTAgentCode" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated">
                                        </asp:AutoCompleteExtender>                                        
                                    </td>
                                    <td colspan="4"></td>
                                    <%--<td>
                                        Comm Code
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCommCode" runat="server">
                                        </asp:DropDownList>
                                    </td>--%>
                                    <%--<td style="display: none">
                                        Flight Date
                                    </td>
                                    <td style="display: none">
                                        >
                                        <asp:TextBox ID="txtFltDate" runat="server" Width="90px"></asp:TextBox>
                                        <asp:ImageButton ID="btnFltDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd" PopupButtonID="btnFltDate"
                                            PopupPosition="BottomLeft" TargetControlID="txtFltDate">
                                        </asp:CalendarExtender>
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnList" runat="server" CssClass="button" OnClick="btnList_Click"
                                            OnClientClick="callShow();" Text="List" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClear" runat="server" CssClass="button"
                                            Text="Clear" onclick="btnClear_Click" />
                                    </td>
                                    
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnList" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <br />
                <h2>
                    Template Details
                </h2>
                <asp:UpdatePanel ID="UPThird" runat="server">
                </asp:UpdatePanel>
                <div style="padding-top: 10px;">
                    <asp:UpdatePanel ID="UPSecond" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GRDBooking" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                Height="82px" OnPageIndexChanging="GRDBooking_PageIndexChanging" 
                                PageSize="10" ShowFooter="True" Width="100%" HeaderStyle-CssClass="HeaderStyle"
                                CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" AlternatingRowStyle-CssClass="AltRowStyle"
                                PagerStyle-CssClass="PagerStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
                                <Columns>
                                    <%--<asp:BoundField DataField="AWBPrefix" HeaderText="AWB Prefix">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>--%>
                                    <%--<asp:BoundField DataField="AWBNumber" HeaderText="AWB Number">
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>--%>
                                  <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CHKSelect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TemplateID">
                                    <ItemTemplate>
                                    <asp:TextBox ID="txtTemplateID" runat="server" AutoPostBack="false" Text='<%# Eval("TemplateID") %>' ></asp:TextBox> 
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="OriginCode" HeaderText="Org">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DestinationCode" HeaderText="Dest">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AgentCode" HeaderText="Agent Code">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                     <%--   <asp:BoundField DataField="AgentName" HeaderText="Agent Name">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>--%>
                                    <asp:TemplateField HeaderText="AgentName">
                                    <ItemTemplate>
                                    <asp:Label ID="lblAgentName" runat="server" Width="200px" Text='<%# Eval("AgentName").ToString().PadRight(15).Substring(0,15)+"..."%> ' ToolTip='<%# Eval("AgentName")%>'></asp:Label> 
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:BoundField DataField="ProductType" HeaderText="Product Type">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="CommodityCode" HeaderText="Commodity Code">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                  <asp:TemplateField HeaderText="Commodity Desc">
                                    <ItemTemplate>
                                    <asp:Label ID="lblCommodityDesc" Width="200px" runat="server" Text='<%# Eval("CommodityDesc").ToString().PadRight(15).Substring(0,15)+"..."%> ' ToolTip='<%# Eval("CommodityDesc")%>'></asp:Label>
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                   <%-- <asp:BoundField DataField="CommodityDesc" HeaderText="Commodity Desc" ItemStyle-Width="80px" ItemStyle-Wrap="true">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>--%>
                                    <asp:BoundField DataField="PiecesCount" HeaderText="Pieces Count" ItemStyle-HorizontalAlign="Right">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="GrossWeight" HeaderText="Gross Weight" ItemStyle-HorizontalAlign="Right"
                                        DataFormatString="{0:00.00}">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                   <%-- <asp:BoundField DataField="ChargedWeight" HeaderText="Charged Weight" ItemStyle-HorizontalAlign="Right"
                                        DataFormatString="{0:00.00}">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>--%>
                                    <asp:TemplateField HeaderText="SerialNumber" Visible="false">
                                    <ItemTemplate>
                                    <asp:Label ID="lblSrno" runat="server" Text='<%# Eval("SerialNumber") %>' ></asp:Label> 
                                    </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField HeaderText="HandlingInfo" Visible="false">
                                    <ItemTemplate>
                                    <asp:Label ID="lblHandlingInfo" runat="server" Text='<%# Eval("HandlingInfo") %>' ></asp:Label> 
                                    </ItemTemplate>
                                     </asp:TemplateField>
                                    <asp:ButtonField CommandName="Edit" Text="Book" Visible="false">
                                        <ItemStyle Width="50px" />
                                    </asp:ButtonField>
                                    <asp:ButtonField CommandName="View" Text="View" Visible="false">
                                        <ItemStyle Width="50px" />
                                    </asp:ButtonField>
                                </Columns>
                                <%-- <HeaderStyle CssClass="titlecolr" />--%>
                                <RowStyle HorizontalAlign="Center" />
                                <AlternatingRowStyle HorizontalAlign="Center" />
                            </asp:GridView>
                        </ContentTemplate>
                        
                    </asp:UpdatePanel>
                </div>
                <div align="left">
                    <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" OnClick="btnSave_Click" />&nbsp;
                    <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add" />&nbsp;
                    <asp:Button ID="btnCreateBooking" runat="server" CssClass="button" Text="Create Booking" OnClick="pop_Click" />&nbsp;
                   <%-- <asp:Button ID="Button2" runat="server" CssClass="button" Text="pop" OnClick="pop_Click" />&nbsp;--%>
                </div>
                <p>
                </p>
                <p>
                </p>
                <p>
                </p>
                <p>
                
                </p>
                <p>
                </p>
                <p>
                </p>
                <p>
                </p>
                <p>
                </p>
                <p>
                </p>
            </div>
            
            <div id="Lightsplit" class="white_contentnew" style="overflow: auto;  width: 500px;">
         <div>
             <fieldset>
                 <legend>AWB Details</legend>
                 <table>
                 <tr><td style="width:100px;" colspan="3"><asp:Label ID="lblstatuspop" runat="server"></asp:Label></td></tr>
                 <tr>
                 <td>Origin
                 </td>
                 <td><asp:TextBox ID="txtorigin" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                 </td>
                 <td>Destination
                 </td>
                 <td><asp:TextBox ID="txtdest" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                 </td>
                 
                 </tr>
                 <tr>
                 <td>Partner Type
                 </td>
                 <td><asp:TextBox ID="txtPartnerType" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                 </td>
                 <td>
                 Partner Code
                 </td>
                 <td><asp:TextBox ID="txtPartnercode" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                 </td>
                 </tr>
                 
                 <tr>
                 <td>
                 Flight Date
                 </td>
                 <td><asp:TextBox ID="txtfltdt" runat="server" AutoPostBack="true" Width="85px" OnTextChanged="txtfltdt_TextChanged"></asp:TextBox>*
                             <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" 
                                 ImageUrl="~/Images/calendar_2.png" />
                             <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
                                 Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgDate" 
                                 TargetControlID="txtfltdt">
                             </asp:CalendarExtender>
                 </td>
                 <td>
                 Flight
                 </td>
                 <td colspan="3">
                             
                                 <asp:DropDownList ID="fltID" runat="server">
                                 <%--<asp:ListItem Text="Select"></asp:ListItem>--%>
                                 </asp:DropDownList>*
                             
                         </td>
                 </tr>
                 <tr>
                 <td>Product Type
                 </td>
                 <td><%--<asp:TextBox ID="txtProductType" runat="server" Width="80px"></asp:TextBox>--%>
                 <asp:DropDownList ID="ddlProductType" runat="server" CssClass="alignrgt" 
                                         Width="120px" Enabled="false">
                                    </asp:DropDownList>
                 </td>
                 </tr>
                     
                     <tr>
                         <td>
                             Commdity</td>
                         <td>
                             <asp:TextBox ID="TextBox3" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                         </td>
                         <td>Description
                         </td>
                         <td>
                             <asp:TextBox ID="TextBox4" runat="server" Width="100px" Enabled="false"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             Pices</td>
                         <td>
                             <asp:TextBox ID="TextBox5" runat="server" Width="50px"></asp:TextBox>
                         </td>
                         <td>
                             Weight</td>
                         <td>
                             <asp:TextBox ID="txtweight" runat="server" Width="50px"></asp:TextBox>
                         </td>
                         
                     </tr>
                     <tr>
                     <td>
                             Handling Info</td>
                         <td colspan="3">
                             <asp:TextBox ID="txthndlnginfo" runat="server" Width="200px" Height="23px" TextMode="MultiLine"></asp:TextBox>
                         </td>
                     </tr>
                 </table>
             </fieldset>
             <fieldset>
                 <legend>Recurrence pattern</legend>
                 <table width="100%">
                 <tr> <td>&nbsp;&nbsp;&nbsp;Flight<asp:DropDownList ID="ddlFltID" runat="server" OnSelectedIndexChanged="ddlFltID_SelectedIndexChanged" AutoPostBack="true">
                                 </asp:DropDownList>
                     </td></tr>
                     <tr>
                    
                         <td>
                         <div style="border-right:1px solid #ccc; margin-left:5px; padding-right:15px;">
                             <asp:RadioButton ID="rbdnDaily" runat="server" Text="Daily"  GroupName="rd" AutoPostBack="true" OnCheckedChanged="rbdnDaily_CheckedChanged" />
                             <br />
                             <asp:RadioButton ID="rbdnWeekly" runat="server" Text="Weekly"  GroupName="rd" OnCheckedChanged="rbdnWeekly_CheckedChanged" AutoPostBack="true" />
                             <br />
                             <asp:RadioButton ID="rbdnMonthly" runat="server" Text="Monthly"  GroupName="rd" OnCheckedChanged="rbdnMonthly_CheckedChanged" AutoPostBack="true" />
                             <%--<br />
                             <asp:RadioButton ID="rbdnyearrly" runat="server" Text="yearly" Enabled="false" />--%></div>
                         </td>
                         <td>&nbsp;
                         </td>
                         <td valign="top">
                             <%--Recur every
                             <asp:TextBox ID="txtrecur" runat="server" Width="30" Enabled="false"></asp:TextBox>
                             week(s)on:
                             <br />--%>
                             <asp:CheckBox ID="chksunday" runat="server" Text="Sunday"  />
                             <asp:CheckBox ID="chkmonday" runat="server" Text="Monday"  />
                             <asp:CheckBox ID="ChkTuesday" runat="server" Text="Tuesday"  />
                             <asp:CheckBox ID="ChkWednesday" runat="server" Text="Wednesday"  /><br />
                             <asp:CheckBox ID="ChkThursday" runat="server" Text="Thursday" />
                             <asp:CheckBox ID="ChkFriday" runat="server" Text="Friday" />
                             <asp:CheckBox ID="ChkSaturday" runat="server" Text="Saturday" />
                             
                             <br />
                             <asp:RadioButton ID="rbdsun" runat="server" Text="Sunday" GroupName="rd3"  />
                             <asp:RadioButton ID="rbdmon" runat="server" Text="Monday"  GroupName="rd3"/>
                             <asp:RadioButton ID="rbdtue" runat="server" Text="Tuesday"  GroupName="rd3"/>
                             <asp:RadioButton ID="rbdwed" runat="server" Text="Wednesday"  GroupName="rd3"/><br />
                             <asp:RadioButton ID="rbdthu" runat="server" Text="Thursday" GroupName="rd3"/>
                             <asp:RadioButton ID="rbdfri" runat="server" Text="Friday" GroupName="rd3"/>
                             <asp:RadioButton ID="rbdsat" runat="server" Text="Saturday" GroupName="rd3"/>
                         </td>
                     </tr>
                 </table>
             </fieldset>
             <fieldset>
                 <legend>Range of recurrence</legend>
                 <table>
                     <tr>
                         <td valign="top">
                             Start
                             <asp:TextBox ID="txtFromDate" runat="server" Width="115px"></asp:TextBox>
                             <asp:ImageButton ID="btnFromDate" runat="server" ImageAlign="AbsMiddle" 
                                 ImageUrl="~/Images/calendar_2.png" />
                             <asp:CalendarExtender ID="CEFromDate" runat="server" Format="yyyy-MM-dd" 
                                 PopupButtonID="btnFromDate" PopupPosition="TopLeft" 
                                 TargetControlID="txtFromDate">
                             </asp:CalendarExtender>
                             <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Back date not allowed." ControlToValidate="txtFromDate" Operator="GreaterThanEqual" Type="String"></asp:CompareValidator>
                         </td>
                         <td>
                             <%--<asp:RadioButton ID="RadioButton1" runat="server" Text="No end date" />
                             <br />--%>
                             <asp:RadioButton ID="RadioButton2" runat="server" Text="end after:" GroupName="rd1" />
                             <asp:TextBox ID="Textbox31" runat="server" Width="30"></asp:TextBox>
                             occurrences<br />
                             <asp:RadioButton ID="RadioButton3" runat="server" Text="end by:" GroupName="rd1" />
                             <asp:TextBox ID="TextBox6" runat="server" Width="115px"></asp:TextBox>
                             <asp:ImageButton ID="ImageButton11" runat="server" ImageAlign="AbsMiddle" 
                                 ImageUrl="~/Images/calendar_2.png" />
                             <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="yyyy-MM-dd" 
                                 PopupButtonID="ImageButton11" PopupPosition="TopRight" 
                                 TargetControlID="TextBox6">
                             </asp:CalendarExtender>
                             <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Back date not allowed." ControlToValidate="TextBox6" Operator="GreaterThanEqual" Type="String" ></asp:CompareValidator>
                             <asp:CustomValidator ID="cusvalidator" runat="server" ClientValidationFunction="checkForm"
ErrorMessage="The Date range should not exceed more than 30 days" EnableClientScript="true"
ValidationGroup="Group1" Display="None" ControlToValidate="TextBox6"></asp:CustomValidator>
                            
                         </td>
                     </tr>
                 </table>
             </fieldset>
             <asp:Button ID="Button1" runat="server" CssClass="button" Text="Save" OnClientClick="checkForm();" OnClick="Button1_Click" />
             <asp:Button ID="Button12" runat="server" CssClass="button" Text="Cancel" />
         </div>
     </div>
     <div id="fadesplit" class="black_overlaynew"></div>
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
            <div id="msgfade" class="black_overlaymsg"></div>
            
        </ContentTemplate>
       <%-- <Triggers>--%>
        <%--<asp:PostBackTrigger ControlID="txtfltdt" />--%>
        <%--<asp:AsyncPostBackTrigger ControlID="txtfltdt" EventName="txtfltdt_TextChanged" />
        
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
