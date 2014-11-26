<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="ImportSummary.aspx.cs" Inherits="ProjectSmartCargoManager.ImportSummary" %>
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
   <h1> 
            Import Summary</h1>
         <p>
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
            </p>
 
<asp:Panel ID="pnlNew" runat="server"><table border="0">
<tr>
<td align = "right" >
        Location :
    </td>
    <td align = "left">
        <asp:DropDownList ID="ddlLocation" runat="server" AppendDataBoundItems="True" 
            Width="140px">
        </asp:DropDownList>
    </td>
    <td align = "right">
        From Date :
    </td>
    <td align = "left">
        <asp:TextBox ID="txtFrmDate" runat="server" Width="100px"></asp:TextBox>
         <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
          Enabled="True" TargetControlID="txtFrmDate" Format="dd/MM/yyyy" PopupButtonID="imgFromDate" >
         </asp:CalendarExtender>
        <asp:ImageButton ID="imgFromDate" runat="server" 
            ImageUrl="~/Images/calendar_2.png" />
    </td>
    <td align = "right">
        To Date :
    </td>
    <td align = "left">
        <asp:TextBox ID="txtToDate" runat="server" Width="100px"></asp:TextBox>
        <asp:ImageButton ID="imgToDate" runat="server" 
            ImageUrl="~/Images/calendar_2.png" />
        <asp:CalendarExtender ID="CalendarExtender4" runat="server" 
          Enabled="True" TargetControlID="txtToDate" Format="dd/MM/yyyy"  PopupButtonID="imgToDate">
        </asp:CalendarExtender>
    </td>

    

</tr>
    
    <caption>
        <br />
        
        <tr>
            <td align="right">
                &nbsp;</td>
           <td align="left">
                <%--<asp:DropDownList ID="ddlAgentCode" runat="server">
                </asp:DropDownList>--%>
            </td>
            <td align="right">
                &nbsp;</td>
            <td align="left">
                &nbsp;</td>
            <td align="right">
                &nbsp;</td>
            <td align="left">
                
                <asp:Button ID="btnList" runat="server" CssClass="button" 
                    onclick="btnList_Click" Text="<%$ Resources:LabelNames, LBL_BTN_LIST %>" />
                <asp:Button ID="btnExport" runat="server" CssClass="button" 
                    Text="<%$ Resources:LabelNames, LBL_BTN_EXPORT %>" Visible="false" />
                    <asp:Button ID="btnClear" runat="server" CssClass="button" 
                    onclick="btnClear_Click" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>" />
            </td>
            
        </tr>
        
        
        
    </caption>

    

</table>
</asp:Panel>  
<asp:Panel ID="pnlGrid"  runat="server">


<div>
    <asp:GridView ID="grdImportList" runat="server" ShowFooter="false" Width="80%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" 
         OnPageIndexChanging="grdImportList_PageIndexChanging">
           
            <Columns>
             <asp:TemplateField HeaderText="FltNo" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblFlightNo" runat="server" Text = '<%# Eval("FlightNo") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblOrigin" runat="server" Text = '<%# Eval("Origin") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
                         
             <asp:TemplateField HeaderText="Location" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDestination" runat="server" Text = '<%# Eval("Destination") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="FltDate" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblFltDate" runat="server" Text = '<%# Eval("CreatedOn") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="DepartedPcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDeparted" runat="server" Text = '<%# Eval("Departed") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="DepartedWt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDepartedWt" runat="server" Text = '<%# Eval("PcDWeight") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="OffloadedPcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblOffloadPcs" runat="server" Text = '<%# Eval("OffloadPcs") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             
             <asp:TemplateField HeaderText="OffloadedWt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblOffloadWt" runat="server" Text = '<%# Eval("OffloadWt") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="ArrivedPcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblArrived" runat="server" Text = '<%# Eval("Arrival") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="ArrivedWt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblArrivedWt" runat="server" Text = '<%# Eval("PcArWeight") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="DeliveredPcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDelivered" runat="server" Text = '<%# Eval("Delivered") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="DeliveredWt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDeliveredWt" runat="server" Text = '<%# Eval("PcDlWeight") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="FltStatus" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblFltStatus" runat="server" Text = '<%# Eval("FltStatus") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
                      
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
