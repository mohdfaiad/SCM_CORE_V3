<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAgentCapacityList.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmAgentCapacityList" %>

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
                //alert(grid.rows.length);
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
	    .style4
        {
            width: 199px;
        }
        .style5
        {
            width: 151px;
        }
        .style6
        {
            width: 71px;
        }
        .style7
        {
            width: 76px;
        }
	    .style8
        {
            width: 222px;
        }
	    .style9
        {
            width: 72px;
        }
	    .style10
        {
            height: 32px;
        }
        .style11
        {
            width: 72px;
            height: 32px;
        }
        .style12
        {
            width: 71px;
            height: 32px;
        }
        .style13
        {
            width: 151px;
            height: 32px;
        }
        .style14
        {
            height: 32px;
            width: 222px;
        }
	</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    <asp:UpdatePanel ID="uppnl" runat="server">
    <ContentTemplate>
    
    <div id="contentarea">
        <h1>Agent Capacity List</h1>
        
            <asp:UpdatePanel ID="UPFourth" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Large"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <div class="botline">
                <asp:UpdatePanel ID="UPFirst" runat="server">
                    <ContentTemplate>
                        <table border="0" style="width:80%">
                            <tr>
                            <td class="style10">
                            Origin
                            </td> 
                            <td class="style14" >
                                <asp:DropDownList ID="ddlOrg" runat="server">
                                </asp:DropDownList>
                                </td> 
                            <td class="style11">
                            Destination
                            </td>  
                            <td class="style10" >
                             <asp:DropDownList ID="ddlDest" runat="server" AutoPostBack="true" 
                                    onselectedindexchanged="ddlDest_SelectedIndexChanged">
                                </asp:DropDownList>
                               
                            </td>
                                <td class="style10">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="style12">
                                    </td>
                                <td class="style13">
        
                                    </td>
                                <td class="style10">
                                    </td>
                            </tr>
                            
                            <tr>
                                <td class="style7">
                                    Flight #</td>
                                <td class="style8">
                                    <asp:DropDownList ID="ddlFlightPrefix" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="ddlFlightPrefix_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddlFlight" runat="server" AppendDataBoundItems="True">
                                    </asp:DropDownList>
                                </td>
                                <td class="style9">
                                    Agent Code</td>
                                <td class="style4">
                                    <asp:TextBox ID="TXTAgentCode" runat="server" AutoPostBack="true" 
                                        CssClass="styleUpper" onChange="callShow();" TabIndex="4" Width="110px"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="ACEAgentCode" runat="server" 
                                        CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
                                        MinimumPrefixLength="1" ServiceMethod="GetAgentCode" 
                                        TargetControlID="TXTAgentCode">
                                    </asp:AutoCompleteExtender>
                                    &nbsp;
                                    <asp:ImageButton ID="IBOrigin" runat="server" ImageAlign="AbsMiddle" 
                                        ImageUrl="~/Images/list_bullets.png" 
                                        OnClientClick="javascript:GetAgentCode();return false;" />
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td class="style6">
                                    &nbsp;</td>
                                <td class="style5">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            
                            <tr>
                                <td colspan="8">
                                    <asp:Button ID="btnList" runat="server" CssClass="button" 
                                        OnClick="btnList_Click" OnClientClick="callShow();" Text="List" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnClear" runat="server" CssClass="button" 
                                        onclick="btnClear_Click" Text="Clear" />
                                </td>
                            </tr>
                            
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnList" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            
            <h2>List Details</h2>
            <asp:UpdatePanel ID="UPThird" runat="server">
                <ContentTemplate>
                    <asp:Label ID="LBLNoOfRecords" runat="server"></asp:Label>
              <asp:Label ID="lblTotal" runat="server"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                <asp:UpdatePanel ID="UPSecond" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gdvQuoteList" runat="server" AllowPaging="true" 
                            AutoGenerateColumns="False" Height="82px" PageSize="15" Width="100%" 
                            OnPageIndexChanging="gdvQuoteList_PageIndexChanging">
                            <Columns>
                            
    <asp:TemplateField Visible="false">
    <ItemTemplate>  
    <asp:RadioButton ID="rbSelect" runat="server" onclick="javascript:SingleSelection(this, 'rbSelect');return true;" />
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Agent Code" >
    <ItemTemplate>
    <asp:Label ID="lblSrNo" runat="server"  Text='<%# Eval("AgentCode") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Agent Name">
    <ItemTemplate>
    <asp:Label ID="lblAgenCode" runat="server"  Text='<%# Eval("AgentName") %>' Width="200px"></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Flight #">
    <ItemTemplate>
    <asp:Label ID="lblFlight" runat="server"  Text='<%# Eval("FlightNo") %>' Width="100px"></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="From Date">
    <ItemTemplate>
    <asp:Label ID="lblFromDt" runat="server"  Text='<%# Eval("FromDate") %>' Width="100px"></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="To Date">
    <ItemTemplate>
    <asp:Label ID="lblToDate" runat="server"  Text='<%# Eval("ToDate") %>' Width="100px"></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Day Of Week">
    <ItemTemplate>
    <asp:Label ID="lblDayOfWeek" runat="server"  Text='<%# Eval("DayOfWeek") %>' Width="70px"></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Blocked Capacity">
    <ItemTemplate>
    <asp:Label ID="lblBlockedCapacity" runat="server"  Text='<%# Eval("FreightWeight") %>' Width="70px"></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Rate per Kg">
    <ItemTemplate>
    <asp:Label ID="lblFreightRate" runat="server"  Text='<%# Eval("FreightRate") %>' Width="70px"></asp:Label>
    </ItemTemplate>
    </asp:TemplateField>
    
                            </Columns>
                            <HeaderStyle CssClass="titlecolr" />
                            <RowStyle HorizontalAlign="Center" />
                            <AlternatingRowStyle HorizontalAlign="Center" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div >
                <asp:Button ID="btnBooking" runat="server" CssClass="button" 
                    Text="Book" onclick="btnBooking_Click" OnClientClick="btnfinalize_Click" Visible="false"/>
                <%--&nbsp;<asp:Button ID="btnPrint" runat="server" CssClass="button" Text="Print" />--%>
                &nbsp;
            </div>
            
        
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
		
		<div id="msgfade" class="black_overlaymsg"></div>
    
    </ContentTemplate>
    </asp:UpdatePanel>    

</asp:Content>