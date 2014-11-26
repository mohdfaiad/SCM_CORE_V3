<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAgentQuoteListNew.aspx.cs" Inherits="ProjectSmartCargoManager.frmAgentQuoteListNew" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript"  type="text/javascript">

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

    function PadLeft(origStr, padChar, Len) {
        if (origStr.length >= Len) {
            return origStr;
         }
         else {
             while (origStr.length < Len) {
                 origStr = padChar + origStr;
             }
             return (origStr);
         }
       }
    function SingleSelection(rbID, rbName) {

        var grid = document.getElementById("<%= gdvQuoteList.ClientID %>");
        //alert(rbID.checked);
        var selrb = document.getElementById(rbID.id);
                //alert(selrb.id);
        if (grid.rows.length > 0) {
            for (i = 2; i <= grid.rows.length - 1; i++) {

                var radioButton;
                var rowIndex = i.toString();
                                //alert(rowIndex);
                if (grid.rows.length > 99) {
                    rowIndex = PadLeft(rowIndex, "0", 3);
                                        //alert(rowIndex);
                }
                else {
                    rowIndex = PadLeft(rowIndex, "0", 2);
                                        //alert(rowIndex);
                }

                radioButton = document.getElementById(grid.id + '_ctl' + rowIndex + '_' + rbName);
                //alert(radioButton.id + "; " + selrb.id);
                document.getElementById(radioButton.id).checked = false;
            }
        }
        //alert(selrb.id)
        document.getElementById(selrb.id).checked = true;
    }

    
    
    function btnfinalize_Click() {
        var grid = document.getElementById("<%= gdvQuoteList.ClientID %>");
        if (grid.rows.length > 0) {
            for (i = 2; i <= grid.rows.length - 1; i++) {
                var radioButton;
                var rowIndex = i.toString();
                if (grid.rows.length > 99) {
                    rowIndex = PadLeft(rowIndex, "0", 3);
                }
                else {
                    rowIndex = PadLeft(rowIndex, "0", 2);
                }

                radioButton = document.getElementById(grid.id + '_ctl' + rowIndex + '_' + rbName);
                //alert(radioButton.id + "; " + selrb.id);
                if (document.getElementById(radioButton.id).checked == true) {
                    return true;
                }
            }
        }
        document.getElementById("<%= lblStatus.ClientID %>").innerHTML = "Please select row to Finalize.";
        document.getElementById("<%= lblStatus.ClientID %>").style.color = "red";
        return (false);
    }

    function GetAgentCode() {
        var level = 'AgentCode';
        var TxtOriginClientObject = '<%=TXTAgentCode.ClientID %>';
        window.open('ListDataView.aspx?Parent=AgentCode&level=' + level + '&TargetTXT=' + TxtOriginClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        return false;
    }

</script>

    <style type="text/css">
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
        <h1>Agent Quotes</h1>
        
            &nbsp;
            <asp:UpdatePanel ID="UPFourth" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Large"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <div class="botline">
                <asp:UpdatePanel ID="UPFirst" runat="server">
                    <ContentTemplate>
                        <table border="0" style="width:91%; height: 100%">
                            <tr>
                                <td>
                                    Origin</td>
                                <td>
                                    <asp:DropDownList ID="ddlSource" runat="server" AppendDataBoundItems="True"> </asp:DropDownList>
                                </td>
                                <td>
                                    Agent Code</td>
                                <td>
                                  <asp:TextBox ID="TXTAgentCode" runat="server" AutoPostBack="true" Width="110px"  TabIndex="4" CssClass="styleUpper"></asp:TextBox>
                                   <asp:AutoCompleteExtender ID="ACEAgentCode" runat="server" ServiceMethod="GetAgentCode" CompletionSetCount="10" TargetControlID="TXTAgentCode"
                                    MinimumPrefixLength="1"></asp:AutoCompleteExtender>&nbsp;
                                    <asp:ImageButton ID="IBOrigin" runat="server" ImageUrl="~/Images/list_bullets.png"
                            ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />
                                    </td>
                                    <td>
                                    <asp:Button ID="btnList" runat="server" CssClass="button" OnClick="btnList_Click" OnClientClick="callShow();" Text="List" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" />
                                </td>
                                <td>
                                    &nbsp;</td><td>
                                    &nbsp;</td><td>
                                    &nbsp;</td></tr><tr>
                                <td>
                                    &nbsp;</td><td>
                                    <asp:TextBox ID="txtFromDate" runat="server" Width="115px" Visible="false"></asp:TextBox><asp:ImageButton ID="btnFromDate" runat="server" ImageAlign="AbsMiddle" 
                                        ImageUrl="~/Images/calendar_2.png" Visible="false"/>
                                    <asp:CalendarExtender ID="CEFromDate" runat="server" Format="yyyy-MM-dd" 
                                        PopupButtonID="btnFromDate" PopupPosition="BottomLeft" 
                                        TargetControlID="txtFromDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    &nbsp;</td><td>
                                    <asp:TextBox ID="txtToDate" runat="server" Width="115px" Visible="false"></asp:TextBox><asp:ImageButton ID="btnToDate" runat="server" ImageAlign="AbsMiddle" 
                                        ImageUrl="~/Images/calendar_2.png" Visible="false"/>
                                    <asp:CalendarExtender ID="CEToDate" runat="server" Format="yyyy-MM-dd" 
                                        PopupButtonID="btnToDate" PopupPosition="BottomLeft" 
                                        TargetControlID="txtToDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    &nbsp;</td><td>
                                </td>
                                <td colspan="2">
                                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</td><td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    &nbsp;</td><td colspan="2">
                                    &nbsp;</td><td>
                                    &nbsp;</td></tr></table></ContentTemplate><Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnList" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <br />
            <h2>
                Quote Details</h2><asp:UpdatePanel ID="UPThird" runat="server">
                <ContentTemplate>
                    <asp:Label ID="LBLNoOfRecords" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblTotal" runat="server"></asp:Label></ContentTemplate></asp:UpdatePanel><div class="divback">
                <asp:UpdatePanel ID="UPSecond" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gdvQuoteList" runat="server" AllowPaging="true" 
                            AutoGenerateColumns="False" Height="82px" PageSize="15" 
                            Width="100%" OnRowCommand="gdvQuoteList_RowCommand" OnPageIndexChanging="gdvQuoteList_PageIndexChanging">
                            <Columns>
                  <asp:TemplateField>
                                            <ItemTemplate>  
                                                <%--<asp:RadioButton ID="rbSelect" runat="server" onclick="javascript:SingleSelection(this, 'rbSelect');return true;" />--%>
                                                <asp:CheckBox ID="chkQ" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Quote #"  Visible="false">
    <ItemTemplate>
    <asp:Label ID="lblQuoteID" runat="server"  Text='<%# Eval("SrNo") %>' Width="100px" ></asp:Label></ItemTemplate></asp:TemplateField>
    <asp:TemplateField HeaderText="Quote #" >
    <ItemTemplate>
    <asp:Label ID="lblAgentQuoteID" runat="server"  Text='<%# Eval("AgentQuoteID") %>' Width="100px"></asp:Label></ItemTemplate></asp:TemplateField>
    <asp:TemplateField HeaderText="Agent Code" >
    <ItemTemplate>
    <asp:Label ID="lblAgenCode" runat="server"  Text='<%# Eval("AgentCode") %>' Width="100px"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Agent Name">
    <ItemTemplate>
    <asp:Label ID="lblAgenName" runat="server"  Text='<%# Eval("AgentName") %>' Width="200px"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Origin">
    <ItemTemplate>
    <asp:Label ID="lblOrigin" runat="server"  Text='<%# Eval("Origin") %>' Width="50px"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Destination">
    <ItemTemplate>
    <asp:Label ID="lblDest" runat="server"  Text='<%# Eval("Dest") %>' Width="50px"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Flight #">
    <ItemTemplate>
    <asp:Label ID="lblFlight" runat="server"  Text='<%# Eval("FlightNo") %>' Width="70px"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Flight Date">
    <ItemTemplate>
    <asp:Label ID="lblFlightDt" runat="server"  Text='<%# Eval("FlightDate") %>' Width="150px"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Freight Weight">
    <ItemTemplate>
    <asp:Label ID="lblFreightweight" runat="server"  Text='<%# Eval("FreightWeight") %>' Width="70px"></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Rate per Kg">
    <ItemTemplate>
    <asp:Label ID="lblFreightRate" runat="server"  Text='<%# Eval("FreightRate") %>' Width="70px"></asp:Label></ItemTemplate></asp:TemplateField>
   
                            <asp:ButtonField CommandName="Edit" Text="Edit" Visible="true">
                                        <ItemStyle Width="50px" />
                                    </asp:ButtonField>
                              </Columns>
                            <FooterStyle Wrap="True" />
                            <HeaderStyle CssClass="titlecolr" />
                            <RowStyle HorizontalAlign="Center" />
                            <AlternatingRowStyle HorizontalAlign="Center" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div align="left">
                <asp:Button ID="btnBooking" runat="server" CssClass="button" 
                    Text="Book" onclick="btnBooking_Click" OnClientClick="btnfinalize_Click" Visible="false" />
                    <asp:Button ID="btnApprove" runat="server" CssClass="button" 
                    Text="Approve" onclick="btnApprove_Click" Visible="true" />
                    
                <%--&nbsp;<asp:Button ID="btnPrint" runat="server" CssClass="button" Text="Print" />
                &nbsp;<asp:Button ID="btnClose" runat="server" CssClass="button" 
                    PostBackUrl="~/Home.aspx" Text="Close" />--%>
            </div>
            
        
    </div>
    
     
    <div id="msglight" class="white_contentmsg" >
<table>

<tr>

<td width="5%" align="center">
    <br />
    <img src="Images/loading.gif" />
    <br />
    <asp:Label ID="msgshow" runat="server" ></asp:Label></td></tr></table></div><div id="msgfade" class="black_overlaymsg"></div>
		</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
