<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MailBookingSummary.aspx.cs" Inherits="ProjectSmartCargoManager.MailBookingSummary" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
        document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

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
        width: 3px;
    }
    </style>
 
 <asp:UpdatePanel ID="UP1" runat="server">
 <ContentTemplate>
 
<div id="contentarea">
<div class="msg">
    <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
<h1>Mail Booking Summary</h1>
<div class="botline">
<table>
    <tr>
        <td>Consignment ID:</td>
        <td colspan="2">
            <asp:TextBox ID="txtConID" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>Date:*</td>
        <td>
             <asp:TextBox ID="txtFromDate" runat="server" Width="74px"></asp:TextBox>
            <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                 Enabled="True" TargetControlID="txtFromDate" PopupButtonID="imgFromDate">
            </asp:CalendarExtender>
            <asp:ImageButton ID="imgFromDate" runat ="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
           <asp:TextBoxWatermarkExtender ID="txtFromDate_Watermark" runat="server" TargetControlID="txtFromDate" WatermarkText="From">
           </asp:TextBoxWatermarkExtender>
        </td>
        <td>
            <asp:TextBox ID="txtToDate" runat="server" Width="74px"></asp:TextBox>
           <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                 Enabled="True" TargetControlID="txtToDate" PopupButtonID="imgToDate">
            </asp:CalendarExtender>
            <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
            <asp:TextBoxWatermarkExtender ID="txtToDate_Watermark" runat="server" TargetControlID="txtToDate" WatermarkText="To">
           </asp:TextBoxWatermarkExtender>
        </td>
    </tr>
    <tr>
        <td>Flight:</td>
        <td>
            <asp:TextBox ID="txtFltDt" runat="server" Width="74px"></asp:TextBox>
            <asp:CalendarExtender ID="txtFltDt_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                 Enabled="True" TargetControlID="txtFltDt" PopupButtonID="imgFltDt">
            </asp:CalendarExtender>
            <asp:ImageButton ID="imgFltDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
            <asp:TextBoxWatermarkExtender ID="txtFltDt_WaterMark" runat="server" TargetControlID="txtFltDt" WatermarkText="Date">
           </asp:TextBoxWatermarkExtender>
        </td>
        <td>
            <asp:TextBox ID="txtFltPrefix" runat="server" MaxLength="2" Width="40px"></asp:TextBox>
            <asp:TextBoxWatermarkExtender ID="txtFltPrefix_Watermark" runat="server" TargetControlID="txtFltPrefix" WatermarkText="Prefix">
           </asp:TextBoxWatermarkExtender>
           
            <asp:TextBox ID="txtFltNo" runat="server" MaxLength="4" Width="60px"></asp:TextBox>
            <asp:TextBoxWatermarkExtender ID="txtFltNo_Watermark" runat="server" TargetControlID="txtFltNo" WatermarkText="No">
           </asp:TextBoxWatermarkExtender>
        </td>
    </tr>
    <tr>
        <td>Post Admin Origin:</td>
        <td>
            <asp:DropDownList ID="ddlPAdmOrg" runat="server" Width="80px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>Post Office Origin:</td>
        <td>
            <asp:DropDownList ID="ddlPOffOrg" runat="server" Width="80px">
            </asp:DropDownList>
        </td>
        <td>Post Office Destination:</td>
        <td>
            <asp:DropDownList ID="ddlPOffDest" runat="server" Width="80px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" onclick="btnList_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" /> 
        </td>
    </tr>
</table>
</div>
</div>

<div class="ltfloat">

    <asp:GridView ID="grdMailSummary" runat="server" AlternatingRowStyle-CssClass="AltRowStyle" 
     CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle" PagerStyle-CssClass="PagerStyle" 
     RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle" PageSize="10"
     AutoGenerateColumns="false" onpageindexchanging="grdMailSummary_PageIndexChanging" 
     onrowcommand="grdMailSummary_RowCommand" Width="100%" AllowPaging="true">
    
    <Columns>
        <asp:TemplateField HeaderText="Consignment ID">
            <ItemTemplate>
                <asp:HyperLink ID="hlnkConID" runat="server" Text='<%# Eval("ConsignmentID") %>' NavigateUrl='<%# "MailBookingNew.aspx?command=Edit&ConID=" + Eval("ConsignmentID")%>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Post Admin Origin" HeaderStyle-Wrap="true">
            <ItemTemplate>
                <asp:Label ID="lblPAdmOrg" runat="server" Text='<%#Eval("PostalAdminOrg")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Post Office Origin" HeaderStyle-Wrap="true">
            <ItemTemplate>
                <asp:Label ID="lblPOffOrg" runat="server" Text='<%#Eval("PostOfficeOrg")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Post Office Dest" HeaderStyle-Wrap="true">
            <ItemTemplate>
                <asp:Label ID="lblPOffDest" runat="server" Text='<%#Eval("PostOfficeDest")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Flight Date">
            <ItemTemplate>
                <asp:Label ID="lblFltDt" runat="server" Text='<%#Eval("FltDt")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Flight No">
            <ItemTemplate>
                <asp:Label ID="lblFltNo" runat="server" Text='<%#Eval("FltNo")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Flight Origin">
            <ItemTemplate>
                <asp:Label ID="lblFltOrg" runat="server" Text='<%#Eval("FltOrg")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:TemplateField HeaderText="Flight Dest">
            <ItemTemplate>
                <asp:Label ID="lblFltDest" runat="server" Text='<%#Eval("FltDest")%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    
    </Columns>
    
    </asp:GridView>

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
<div id="msgfade" class="black_overlaymsg"></div>
 
</ContentTemplate>
</asp:UpdatePanel>
 
</asp:Content>