<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true"
    CodeBehind="GHA_ListBooking.aspx.cs" Inherits="ProjectSmartCargoManager.ListBooking_GHA" %>

<%-- 

  2012-07-04 vinayak
  2012-07-06  vinayak Edit/View
  
--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">

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

        function GetAgentCode(obj) {
           
            var destination = obj;
            
            if (destination.value.indexOf("(") > 0) {
                //                if (destination.value.length > 4) {
                var start = destination.value.indexOf("(");
                var end = destination.value.lastIndexOf(")");
                var str = destination.value;
                var CustStart = destination.value.indexOf("$");

                destination.value = str.substring(0, start);
                
            }
        }

        function GetShiperCode(obj) {
            var destination = obj;

            if (destination.value.indexOf("(") > 0) {
                //                if (destination.value.length > 4) {
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
        function onListPopulatedshiper() {
            
            var completionList = $find("ShipperCode").get_completionList();
            completionList.style.width = 'auto';
        }

        function onUserListPopulated() {

            var completionList = $find("ACEUserName").get_completionList();
            completionList.style.width = 'auto';
        }

        
    </script>
    
    
    <script language="javascript" type="text/javascript" src="scripts/jquery.min.js"></script>
    
    <script type="text/javascript">
    
      // Function for get Keypress Event & Call List Button 
        $(function() {
        
        // used to specific Text Box
        //$("#<%=txtAWBNo.ClientID %>").keydown(function(e) {

        // used to $(":text"): Selects (ALL TextBoxes) only text elements on page (input[type=text])
        $(":text").keydown(function(e) {
                   if (e.keyCode == 13) { //if this is enter key
                   // Code to call Button
                    $("#<%= btnList.ClientID %>").trigger('click');
                   //alert("Amit");
                    return true;
                }
            });
        });


        // Function for when Enter Key Pressed in Textbox, Disable Postback on Pressing Entery Key in Textbox
        $(function() {
            $(':text').bind('keydown', function(e) {
                //on keydown for all textboxes
            if (e.target.className != "ddlSource")
                //excludes specific textbox like SearchTextBox
                {
                    if (e.keyCode == 13) { //if this is enter key
                        e.preventDefault();
                        return false;
                    }
                    else
                        return true;
                }
                else
                    return true;
            });
        });
      
    </script>

    <style type="text/css">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="uppnl" runat="server">
        <ContentTemplate>
            <div id="contentarea">
                <h1>
                    List Bookings
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
                                        AWB
                                    </td>
                                <td colspan="3">
                                        <%--<asp:DropDownList ID="ddlFlight" runat="server">
                                        </asp:DropDownList>--%>
                                        <asp:TextBox ID="txtAWBPrefix" runat="server" Width="45px" MaxLength="3"></asp:TextBox>&nbsp;
                                        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtAWBPrefix"
                                            WatermarkText="Prefix" />
                                        <asp:TextBox ID="txtAWBNo" runat="server" Width="80px" MaxLength="8" ></asp:TextBox>
                                        <%--<asp:TextBox ID="txtAWBNo" runat="server" Width="80px" MaxLength="8" AutoPostBack="true"></asp:TextBox>--%>
                                           
                                        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="txtAWBNo"
                                            WatermarkText="AWB Number" />&nbsp;
                                        <asp:TextBox ID="txtAWBFromDt" runat="server" Width="85px" ></asp:TextBox>
                                        <asp:ImageButton ID="imgAWBFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgAWBFromDt"
                                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtAWBFromDt">
                                        </asp:CalendarExtender>&nbsp;
                                        <asp:TextBox ID="txtAWBToDt" runat="server" Width="85px" ></asp:TextBox>
                                        <asp:ImageButton ID="imgAWBToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgAWBToDt"
                                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtAWBToDt">
                                        </asp:CalendarExtender>
                                    </td>
                                    
                                    <td>
                                        Comm Code
                                    </td>
                                    <td>
                                        <%--<asp:DropDownList ID="ddlCommCode" runat="server" Width="150px">
                                        </asp:DropDownList>--%>
                                        <asp:TextBox ID="txtCommodityCode" TabIndex="35" runat="server" Width="100px"
                                            CssClass="styleUpper" onchange="return GetCommodityCode(this);" AutoPostBack="true">
                                        </asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACECommCode" BehaviorID="ACECommCode" runat="server"
                                            ServiceMethod="GetCommodityCodesWithName" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="txtCommodityCode" MinimumPrefixLength="1"
                                            OnClientPopulated="onCommListPopulated">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="LBLAWBStatus" runat="server" Text="Status"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDLStatus" runat="server"  Width="150px">
                                            <asp:ListItem Text="Booked" Value="B"></asp:ListItem>
                                            <asp:ListItem Text="Executed" Value="E"></asp:ListItem>
                                            <asp:ListItem Text="Reopen" Value="R"></asp:ListItem>
                                            <asp:ListItem Text="Void" Value="V"></asp:ListItem>
                                            <%--<asp:ListItem Text="Queued" Value="Q"></asp:ListItem>--%>
                                            <asp:ListItem Text="ALL" Value="A" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    
                                </tr>
                                
                                <tr>
                                <td>
                                        Flight
                                    </td>
                                <td colspan="3">
                                        <%--<asp:DropDownList ID="ddlFlight" runat="server">
                                        </asp:DropDownList>--%>
                                        <asp:TextBox ID="txtFlightCode" runat="server" Width="45px" MaxLength="2" AutoPostBack="true"></asp:TextBox>&nbsp;
                                        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtFlightCode"
                                            WatermarkText="Prefix" />
                                        <asp:TextBox ID="txtFlightID" runat="server" Width="80px" MaxLength="4" AutoPostBack="true"></asp:TextBox>
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
                                     <%--<asp:DropDownList ID="drpUsers" runat="server" Width="150px">
                                        </asp:DropDownList>--%>
                                        <asp:TextBox ID="txtUsers" runat="server" Width="135px" AutoPostBack="true">
                                        </asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACEUserName" BehaviorID="ACEUserName" runat="server"
                                            ServiceMethod="GetUserName" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="txtUsers" MinimumPrefixLength="1"
                                            OnClientPopulated="onUserListPopulated">
                                        </asp:AutoCompleteExtender>
                                        <asp:CheckBox ID="CHKinFFR" runat="server" AutoPostBack="True" OnCheckedChanged="CHKinFFR_CheckedChanged"
                                            Text="InComing FFR" />&nbsp;&nbsp;
                                    <asp:CheckBox ID="CHKfailed" runat="server" Text="Process Failed" />
                                    <asp:CheckBox ID="chkViaTemplate" runat="server" Text="ViaTemplate" />
                                    
                                    </td>
                                    <td>
                                        Agent Code
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTAgentCode" runat="server" Width="100px" TabIndex="55" CssClass="styleUpper"
                                            onchange="GetAgentCode(this);" AutoPostBack="true"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACEAgentCode" BehaviorID="ACEAgentCode" runat="server"
                                            ServiceMethod="GetAgentCodeWithName" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="TXTAgentCode" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated">
                                        </asp:AutoCompleteExtender>                                        
                                    </td>
                                    
                                    <td>Shipper code
                                    </td>
                                    
                                    <td>
                                    <asp:TextBox ID="txtshiper" runat="server" Width="100px" TabIndex="55" CssClass="styleUpper"
                                            onchange="GetShiperCode(this);" AutoPostBack="true"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ShipperCode" BehaviorID="ShipperCode" runat="server"
                                            ServiceMethod="GetShipperCodeWithName" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="txtshiper" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulatedshiper">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    
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
                    Booking Details
                </h2>
                <asp:UpdatePanel ID="UPThird" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="LBLNoOfRecords" runat="server"></asp:Label>
                        &nbsp;<asp:Label ID="lblTotal" runat="server"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div style="padding-top: 10px;">
                    <asp:UpdatePanel ID="UPSecond" runat="server">
                        <ContentTemplate>

                            <asp:GridView ID="GRDBooking" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                                Height="82px" OnPageIndexChanging="GRDBooking_PageIndexChanging" OnRowCommand="GRDBooking_RowCommand"
                                PageSize="15" ShowFooter="True" Width="100%" HeaderStyle-CssClass="HeaderStyle"
                                CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" AlternatingRowStyle-CssClass="AltRowStyle"
                                PagerStyle-CssClass="PagerStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
                                <Columns>
                                    <%--<asp:BoundField DataField="AWBPrefix" HeaderText="AWB Prefix">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>--%>
                                    <%--<asp:BoundField DataField="AWBNumber" HeaderText="AWB Number">
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>--%>
                                    <asp:TemplateField HeaderText="AWB Number">
                                    <ItemTemplate>
                                    <asp:HyperLink  ID="lnkAWBNumber" Width="100px" runat="server" Text='<%# Eval("AWBNumber") %>' NavigateUrl='<%# Convert.ToString(Session["BookingURL"]) + "?command=View&AWBNumber=" + Eval("AWBNumber")%>'></asp:HyperLink>
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
                                    <asp:BoundField DataField="AWBDate" HeaderText="AWB Date">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FltNumber" HeaderText="Flight No">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FltDate" HeaderText="Flight Date">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PiecesCount" HeaderText="Pieces Count">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="GrossWeight" HeaderText="Gross Weight"
                                        DataFormatString="{0:00.00}">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ChargedWeight" HeaderText="Charged Weight"
                                        DataFormatString="{0:00.00}">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProductType" HeaderText="Product Type">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ExecutedBy" HeaderText="ExecutedBy">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AWBStatus" HeaderText="AWBStatus">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Shipper" HeaderText="Shipper">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:ButtonField CommandName="Edit" Text="Edit">
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
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnPrint" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div align="left">
                    <asp:Button ID="btnNewBooking" runat="server" CssClass="button" PostBackUrl="~/GHA_ConBooking.aspx"
                        Text="New Booking" />
                    &nbsp;<asp:Button ID="btnPrint" runat="server" CssClass="button" Text="Export" OnClick="btnPrint_Click" />
                    &nbsp;<asp:Button ID="btnClose" runat="server" CssClass="button" PostBackUrl="~/Home.aspx"
                        Text="Close" Visible="false" />
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
