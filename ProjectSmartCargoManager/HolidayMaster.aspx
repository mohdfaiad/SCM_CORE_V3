<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HolidayMaster.aspx.cs" Title="HolidayMaster" Inherits="ProjectSmartCargoManager.HolidayMaster" MasterPageFile="~/SmartCargoMaster.Master"  %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript">
	    function select() {
	        
	        var hldyVal = document.getElementById("<%= ddlHolidayType.ClientID%>").value;
	        var stnId=document.getElementById("<%= ddlStn.ClientID%>");
	        var cntryId=document.getElementById("<%= ddlCountry.ClientID%>");
	        var dayId=document.getElementById("<%= ddlDay.ClientID%>");
	        var dateId=document.getElementById("<%= txtDate.ClientID%>");
	        var datefrmId=document.getElementById("<%= txtDayFrm.ClientID%>");
	        var datetoId = document.getElementById("<%= txtDayTo.ClientID%>");
	        var validfrmId = document.getElementById("<%= txtValidFrm.ClientID%>");
	        var validtoId = document.getElementById("<%= txtValidTo.ClientID%>");
	        
	        if (hldyVal == "Select") {
	            stnId.disabled = true; cntryId.disabled = true;
	            dayId.disabled = true;
	            validfrmId.disabled = true; validtoId.disabled = true;
               datefrmId.disabled = true; datetoId.disabled = true;
	            stnId.selectedIndex = 0;
	            cntryId.selectedIndex = 0;
	            dayId.selectedIndex = 0;
	            datefrmId.value="";
	            datetoId.value = "";
	            validfrmId.value = "";
	            validtoId.value = "";
	            
	          }
	        if(hldyVal=="WO") {
	            stnId.disabled = false; cntryId.disabled = false;
	            dayId.disabled = false;
	            datefrmId.disabled = true; datetoId.disabled = true;
	            validfrmId.disabled = false; validtoId.disabled = false;
	            stnId.selectedIndex = 0;
	            cntryId.selectedIndex = 0;
	            dayId.selectedIndex = 0;
	            datefrmId.value = "";
	            datetoId.value = "";
	            validfrmId.value = "";
	            validtoId.value = "";
	        }
	        if (hldyVal == "PH") {
	            stnId.disabled = true; cntryId.disabled = false;
	            dayId.disabled = true;
	            datefrmId.disabled = false; datetoId.disabled = false;
	            validfrmId.disabled = true; validtoId.disabled = true;
	            stnId.selectedIndex = 0;
	            cntryId.selectedIndex = 0;
	            dayId.selectedIndex = 0;
	            datefrmId.value = "";
	            datetoId.value = "";
	            validfrmId.value = "";
	            validtoId.value = "";
	        }
	        if (hldyVal == "CH") {
	            stnId.disabled = true; cntryId.disabled = false;
	            dayId.disabled = true; 
	            datefrmId.disabled = false; datetoId.disabled = false;
	            validfrmId.disabled = true; validtoId.disabled = true;
	            stnId.selectedIndex = 0;
	            cntryId.selectedIndex = 0;
	            dayId.selectedIndex = 0;
	            datefrmId.value = "";
	            datetoId.value = "";
	            validfrmId.value = "";
	            validtoId.value = "";
	        }
	        if (hldyVal == "SH") {
	            stnId.disabled = false;cntryId.disabled = false;
	            dayId.disabled = true; 
	            datefrmId.disabled = false; datetoId.disabled = false;
	            validfrmId.disabled = true; validtoId.disabled = true;
	            stnId.selectedIndex = 0;
	            cntryId.selectedIndex = 0;
	            dayId.selectedIndex = 0;
	            datefrmId.value = "";
	            datetoId.value = "";
	            validfrmId.value = "";
	            validtoId.value = "";
	        }
	        return true;
	    }
//	    function datetxt() {
//	        var dateId = document.getElementById("<%= txtDate.ClientID%>");
//	        var datefrmId = document.getElementById("<%= txtDayFrm.ClientID%>");
//	        var datetoId = document.getElementById("<%= txtDayTo.ClientID%>");
//	        if (dateId.value == "") {
//	            datefrmId.disabled = false;
//	            datetoId.disabled = false;
//	        }
//	        else {
//	            datefrmId.disabled = true;
//	            datetoId.disabled = true;
//	        }
//	        return true;
//	    }
//	    function dayFrmTo() {
//	        var hldyVal = document.getElementById("<%= ddlHolidayType.ClientID%>").value;
//	        var dateId = document.getElementById("<%= txtDate.ClientID%>");
//	        var datefrmId = document.getElementById("<%= txtDayFrm.ClientID%>").value;
//	        var datetoId = document.getElementById("<%= txtDayTo.ClientID%>").value;
//	        if (datefrmId == "" && datetoId == "" && hldyVal!="WO")
//	        { dateId.disabled = false; }
//	        else
//	            dateId.disabled = true;
//	    }
	    function funchk() {
	        var hldyVal = document.getElementById("<%= ddlHolidayType.ClientID%>").value;
	        var stnId = document.getElementById("<%= ddlStn.ClientID%>");
	        var cntryId = document.getElementById("<%= ddlCountry.ClientID%>");
	        var dayId = document.getElementById("<%= ddlDay.ClientID%>");
	        var datefrmId = document.getElementById("<%= txtDayFrm.ClientID%>");
	        var datetoId = document.getElementById("<%= txtDayTo.ClientID%>");
	        var validfrmId = document.getElementById("<%= txtValidFrm.ClientID%>");
	        var validtoId = document.getElementById("<%= txtValidTo.ClientID%>");
	        if (hldyVal == "Select") {
	            alert('Select Holiday Type');
	            return false;
	        }
	        if (hldyVal == "WO") {
	            if (stnId.value == "Select" || cntryId.value == "Select" || dayId.value == "Select")
	        {
	            alert('Select Country/Station/Day');
	            return false;
	        }
	        if (validfrmId == "" || validtoId.value == "") {
	            alert('Select date range');
	            return false;
	        } 

	    }
	        if (hldyVal == "PH") {
	            if (cntryId.value == "Select") {
	                alert('Select Country');
	                return false;
	            }
	            if (datefrmId.value == "" || datetoId.value == "") {
	                alert('Select date range');
	                return false;
	            } 
	           
	        }
	        if (hldyVal == "CH") {
	            if (cntryId.value == "Select") {
	                alert('Select Country');
	                return false;
	            }
	            if (datefrmId.value == "" || datetoId.value == "") {
	                alert('Select date range');
	                return false;
	            } 
	        }
	       
	        if (hldyVal == "SH") {
	            if (stnId.value == "Select" || cntryId.value == "Select") {
	                alert('Select Country/Day');
	                return false;
	            }
	            if (datefrmId.value == "" || datetoId.value == "") {
	                alert('Select date range');
	                return false;
	            } 
	        }
	      
	    }
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:ToolkitScriptManager ID="scriptmgr1" runat="server">
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
    .style1
    {
        height: 32px;
    }
    </style>
    
<asp:UpdatePanel ID="UP1" runat="server">
<ContentTemplate>
<div id="contentarea">
<h1>Holiday Master</h1>

<asp:Label ID="StatusLbl" runat="server" Font-Bold="true"  Font-Size="Large" ForeColor="Red"></asp:Label>
<div class="botline">
<table width="100%">
<tr>
<td>Holiday Type</td>
<td>
    <asp:DropDownList ID="ddlHolidayType" runat="server" Width="95px"
     AutoPostBack="false" onchange="javascript:return select();">
    <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
    <asp:ListItem Text="Weekly Off" Value="WO"></asp:ListItem>
    <asp:ListItem Text="Public Holiday" Value="PH"></asp:ListItem>
    <asp:ListItem Text="Station Holiday" Value="SH"></asp:ListItem>
    <asp:ListItem Text="Company Holiday" Value="CH"></asp:ListItem>
    </asp:DropDownList>
</td>

<td>Country</td>
<td>
<asp:DropDownList ID="ddlCountry" runat="server" Width="95px">
</asp:DropDownList></td>

<td>Station</td>
<td>
<asp:DropDownList ID="ddlStn" runat="server"  Width="95px"></asp:DropDownList>
</td>
</tr>
<tr>
<td class="style1">Day</td>
<td class="style1">
<asp:DropDownList ID="ddlDay" runat="server" Width="95px">
<asp:ListItem Text="Select" Value="Select"></asp:ListItem>
<asp:ListItem Text="Sunday" Value="Sun"></asp:ListItem>
<asp:ListItem Text="Monday" Value="Mon"></asp:ListItem>
<asp:ListItem Text="Tuesday" Value="Tue"></asp:ListItem>
<asp:ListItem Text="Wednesday" Value="Wed"></asp:ListItem>
<asp:ListItem Text="Thursday" Value="Thu"></asp:ListItem>
<asp:ListItem Text="Friday" Value="Fri"></asp:ListItem>
<asp:ListItem Text="Saturday" Value="Sat"></asp:ListItem>
</asp:DropDownList>
</td>
<%--<td>Date</td>--%>
<td class="style1"><asp:TextBox ID="txtDate" runat="server" Width="70px" Visible="false"></asp:TextBox>
<asp:CalendarExtender ID="Ext_txtDate" runat="server" TargetControlID="txtDate"></asp:CalendarExtender>
</td>
<td class="style1">Valid From</td>
<td class="style1"><asp:TextBox ID="txtValidFrm" runat="server" Width="70px"></asp:TextBox>
<asp:CalendarExtender ID="Ext_txtValidFrm" runat="server" TargetControlID="txtValidFrm" PopupButtonID="btnValidFrom"></asp:CalendarExtender>
    <asp:ImageButton ID="btnValidFrom" runat="server" ImageAlign="AbsMiddle" 
        ImageUrl="~/Images/calendar_2.png" />
    </td>
<td class="style1">
    Valid To</td>
<td class="style1"><asp:TextBox ID="txtValidTo" runat="server" Width="70px"></asp:TextBox>
<asp:CalendarExtender ID="Ext_txtValidTo" runat="server" TargetControlID="txtValidTo" PopupButtonID="imgValidTo"></asp:CalendarExtender>
    <asp:ImageButton ID="imgValidTo" runat="server" ImageAlign="AbsMiddle" 
        ImageUrl="~/Images/calendar_2.png" />
    </td>

<td class="style1">Date From</td>
<td class="style1"><asp:TextBox ID="txtDayFrm" runat="server" Width="70px"></asp:TextBox>
<asp:CalendarExtender ID="Ext_txtDayFrm" runat="server" TargetControlID="txtDayFrm" PopupButtonID="imgDayFrom"></asp:CalendarExtender>
    <asp:ImageButton ID="imgDayFrom" runat="server" ImageAlign="AbsMiddle" 
        ImageUrl="~/Images/calendar_2.png" />
</td>
<td class="style1">Date To</td>
<td class="style1"><asp:TextBox ID="txtDayTo" runat="server" Width="70px"></asp:TextBox>
<asp:CalendarExtender ID="Ext_txtDayTo" runat="server" TargetControlID="txtDayTo" PopupButtonID="imgDayTo"></asp:CalendarExtender>
    <asp:ImageButton ID="imgDayTo" runat="server" ImageAlign="AbsMiddle" 
        ImageUrl="~/Images/calendar_2.png" />
</td>

<td class="style1">
</td>
</tr>
    <tr>
        <td colspan="12">
            <asp:Button ID="btnSave" runat="server" CssClass="button" 
                onclick="btnSave_Click" OnClientClick="javascript:return funchk();" 
                Text="Save" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnList" runat="server" CssClass="button" 
                onclick="btnList_Click" Text="List" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnClear" runat="server" CssClass="button" 
                Text="Clear" onclick="btnClear_Click" />
        </td>
    </tr>
</table></div>
<div class="ltfloat">
<asp:GridView ID="GrdHldy" runat="server" AutoGenerateColumns="false" 
onrowcommand="GrdHldy_RowCommand" onrowediting="GrdHldy_RowEditing" BorderColor="#BFBEBE"
onpageindexchanging="GrdHldy_PageIndexChanging" AllowPaging="true" PageSize="10"  
        HeaderStyle-CssClass="titlecolr" Height="82px" Width="1000px">
<Columns>

<asp:TemplateField HeaderText="Sr No" Visible="false" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblSrNo" runat="server" Text='<%#Eval("SerialNumber")%>'></asp:Label>
</ItemTemplate>
    <ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>

<asp:TemplateField HeaderText="Station" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblStn" runat="server" Text='<%#Eval("Station")%>'></asp:Label>
</ItemTemplate>
    <ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>

<asp:TemplateField HeaderText="Country" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblCountry" runat="server" Text='<%#Eval("Country")%>'></asp:Label>
</ItemTemplate>
    <ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>

<asp:TemplateField HeaderText="Day" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblDay" runat="server" Text='<%#Eval("Day")%>'></asp:Label>
</ItemTemplate>
    <ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>

<asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="Center" Visible="false">
<ItemTemplate>
<asp:Label ID="lblDate" runat="server" Text='<%#Eval("Date")%>'></asp:Label>
</ItemTemplate>
    <ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>

<asp:TemplateField HeaderText="Holiday Type" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblHolidayType" runat="server" Text='<%#Eval("HolidayType")%>'></asp:Label>
</ItemTemplate>
    <ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>

<asp:TemplateField HeaderText="Date From" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblDateFrom" runat="server" Text='<%#Eval("DateFrom","{0:dd/MM/yyyy}")%>'></asp:Label>
</ItemTemplate>
    <ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>

<asp:TemplateField HeaderText="Date To" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblDateTo" runat="server" Text='<%#Eval("DateTo","{0:dd/MM/yyyy}")%>'></asp:Label>
</ItemTemplate>
    <ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>

<%--<asp:ButtonField CommandName="Edit" Text="Edit">
</asp:ButtonField>--%>
<asp:ButtonField CommandName="DeleteRecord" Text="Delete">
</asp:ButtonField>
</Columns>
    <HeaderStyle CssClass="titlecolr" />
</asp:GridView>
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
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>