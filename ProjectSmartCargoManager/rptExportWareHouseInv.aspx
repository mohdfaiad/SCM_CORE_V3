<%@ Page Title="WareHouse Inventory Export" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="rptExportWareHouseInv.aspx.cs" Inherits="ProjectSmartCargoManager.rptExportWareHouseInv" %>
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
    </style>
<asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
  
    <div id="contentarea">
    <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
   <h1> 
           WareHouse Inventory Export</h1>
         <%--<p>--%>
         <table>
         <tr>
         <td>
               <%-- <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>--%>
                <br />
            </td>
            </tr>
            </table>
            <%--</p>--%>
 
<asp:Panel ID="pnlNew" runat="server">
<table width="75%" border="0">

<%--<tr>
<td>
    Location</td>
    
<td>
    <asp:DropDownList ID="ddlAirport" runat="server">
    </asp:DropDownList>
</td>

<td>
        &nbsp;</td>
<td>
    &nbsp;</td></tr>--%>
    
    <tr>
        <td>
            Station</td>
        <td>
            <asp:DropDownList ID="ddlAirport" runat="server">
            </asp:DropDownList>
        </td>
        <td>
            From Date*</td>
        <td>
            <asp:TextBox ID="txtvalidfrom" runat="server" Width="100px"></asp:TextBox>
            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" 
                Format="dd/MM/yyyy" PopupButtonID="imgFromDate" TargetControlID="txtvalidfrom">
            </asp:CalendarExtender>
            <asp:ImageButton ID="imgFromDate" runat="server" ImageAlign="AbsMiddle" 
                ImageUrl="~/Images/calendar_2.png" />
        </td>
        <td>
            To Date*</td>
        <td>
            <asp:TextBox ID="txtvalidto" runat="server" Width="100px"></asp:TextBox>
            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" 
                Format="dd/MM/yyyy" PopupButtonID="imgToDate" TargetControlID="txtvalidto">
            </asp:CalendarExtender>
            <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" 
                ImageUrl="~/Images/calendar_2.png" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Button ID="btnList" runat="server" CssClass="button" 
                onclick="btnList_Click" Text="<%$ Resources:LabelNames, LBL_BTN_LIST %>" />
            &nbsp;<asp:Button ID="btnClear" runat="server" CssClass="button" 
                onclick="btnClear_Click" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>" />
            &nbsp;<asp:Button ID="btnExport" runat="server" CssClass="button" 
                onclick="btnExport_Click" Text="<%$ Resources:LabelNames, LBL_BTN_EXPORT %>" Visible="false" />
        </td>
        <%--<td>
                &nbsp;</td>--%>
        <td>
            &nbsp;</td>
        <td>
        </td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;&nbsp; &nbsp;&nbsp;
        </td>
        <td>
        </td>
        <td>
        </td>
        <td>
        </td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>

</table>
</asp:Panel>  
<asp:Panel ID="pnlGrid"  runat="server">


<div>
    <asp:GridView ID="grdexpList" runat="server" ShowFooter="false" Width="80%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" 
         OnRowCommand="grdexpList_RowCommand"
        onrowediting="grdexpList_RowEditing" onpageindexchanging="grdexpList_PageIndexChanging">
           
            <Columns>
            
            <asp:TemplateField HeaderText="AWB Prefix" HeaderStyle-Wrap="true" >
                    <ItemTemplate>
                        <asp:Label ID="lblAWBPrefix" runat="server" Text = '<%# Eval("AWBPrefix") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
            
             <asp:TemplateField HeaderText="AWB #" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAWBNumber" runat="server" Text = '<%# Eval("AWBNumber") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Origin Date" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblOriginDate" runat="server" Text = '<%# Bind("OriginDate","{0:dd/MM/yyyy}") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
                         
             <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblOrigin" runat="server" Text = '<%# Eval("Origin") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Destination" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDestination" runat="server" Text = '<%# Eval("Destination") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Booked Flt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblBookedFlt" runat="server" Text = '<%# Eval("BookedFlt") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Booked FltDate" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblBookedFltDate" runat="server" Text = '<%# Bind("BookedFltDate","{0:dd/MM/yyyy}") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Total Pieces" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblTotalPieces" runat="server" Text = '<%# Eval("TotalPieces") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Total Weight" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblTotalWeight" runat="server" Text = '<%# Eval("TotalWeight") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Description" HeaderStyle-Wrap="true" >
             <ItemTemplate>
             <asp:Label ID="lblDescription" runat="server" Width="270px" Text = '<%# Eval("Description") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Shipper" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblShipper" runat="server" Text = '<%# Eval("Shipper") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Consignee" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblConsignee" runat="server" Text = '<%# Eval("Consignee") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Pieces Onhand" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblPiecesOnhand" runat="server" Text = '<%# Eval("PiecesOnhand") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Date Pieces were Onhand" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblDatePieceswereOnhand" runat="server" Text = '<%# Bind("DatePieceswereOnhand","{0:dd/MM/yyyy}") %>'/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             
             <%--<asp:TemplateField HeaderText="Offloadweight" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblOffloadweight" runat="server" Text = '<%# Eval("Offloadweight") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Remarks" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblRemarks" runat="server" Text = '<%# Eval("Remarks") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             
             <asp:TemplateField HeaderText="LoadedStatus" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblLoadedStatus" runat="server" Text = '<%# Eval("IsLoaded") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>--%>
             
            <%--<asp:ButtonField CommandName="Manage" Text="Manage">
                                    <ItemStyle Width="50px" />
                                </asp:ButtonField>--%>
             
             <%--<asp:ButtonField CommandName="Process" Text="ReProcess">
                                    <ItemStyle Width="50px" />
                                </asp:ButtonField>--%>
            </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
    </div>
    
</asp:Panel>
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
    <Triggers>
    <asp:PostBackTrigger ControlID="btnExport" />
    </Triggers>
    </asp:UpdatePanel>

</asp:Content>
