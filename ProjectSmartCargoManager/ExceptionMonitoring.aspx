<%@ Page Title="Booked Vs Flown Report" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="ExceptionMonitoring.aspx.cs" Inherits="ProjectSmartCargoManager.ExceptionMonitoring" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">



<script type="text/javascript">
    function DivFLAB() {
        document.getElementById("FLABDive").style.display = 'none';

    }

    function percentcal() {
        var bookedpc = document.getElementById('ctl00_ContentPlaceHolder1_txtTotalBookedAWBs').value;
        var flab = document.getElementById('ctl00_ContentPlaceHolder1_txtFLAB').value;
        var result = bookedpc / flab * 100;
        alert(result);
        document.getElementById('ctl00_ContentPlaceHolder1_txtFLABper').innerHTML = result;
    }
</script>
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
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
</div>
   <h1> 
            Booked Vs Flown</h1>
         <%--<p>--%>
         <table>
         <tr>
         <td>
               <%-- <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>--%>
         </td>
         </tr>
         </table>
            <%--</p>--%>
 
<asp:Panel ID="pnlNew" runat="server"><table width="55%" border="0">
<tr>
<td>
    Origin</td>
    
<td>
    <asp:DropDownList ID="ddlOrigin" runat="server">
    </asp:DropDownList>
</td><td>
        &nbsp;Destination</td>
<td>
    <asp:DropDownList ID="ddlDest" runat="server">
    </asp:DropDownList>
    </td></tr>
    
    <caption>
        <br />
        <tr>
            <td>
                From Date*</td>
            <td>
                <asp:TextBox ID="txtvalidfrom" runat="server" Width="100px"></asp:TextBox>
                 <asp:ImageButton ID="imgAWBFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                <asp:CalendarExtender ID="txtvalidfrom_CalendarExtender3" runat="server" Enabled="True"  PopupButtonID="imgAWBFromDt" TargetControlID="txtvalidfrom"  Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ErrorMessage="*" ControlToValidate="txtvalidfrom"></asp:RequiredFieldValidator>
                                    
            </td>
            <td>
                To Date*</td>
            <td>
                <asp:TextBox ID="txtvalidto" runat="server" Width="100px"></asp:TextBox>
                 <asp:ImageButton ID="imgAWBToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
               <asp:CalendarExtender ID="txtvalidto_CalendarExtender1" runat="server" Enabled="True"  PopupButtonID="imgAWBToDt" TargetControlID="txtvalidto"  Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ErrorMessage="*" ControlToValidate="txtvalidto"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                
                <asp:Button ID="btnList" runat="server" CssClass="button" 
                    onclick="btnList_Click" OnClientClick="DivFLAB()" Text="List"  />
                &nbsp;&nbsp;
                
                <asp:Button ID="btnClear" runat="server" CssClass="button" 
                    onclick="btnClear_Click" Text="Clear" />
                    &nbsp;
                    <asp:Button ID="btnExport" runat="server" Text="Export" Visible="false"
                            CssClass="button" onclick="btnExport_Click" />
                </td>
<%--           <td>
                <asp:DropDownList ID="ddlAgentCode" runat="server">
                </asp:DropDownList>
            </td>--%>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </caption>

</table>
</asp:Panel>  
<br />
<asp:Panel ID="pnlGrid"  runat="server">
<div id="FLABDiv"> 
<table>
<%--<tr>
<td>
Total Booked AWBs
</td>
<td>
<asp:TextBox runat="server" ID="txtTotalBookedAWBs" ReadOnly="true" Width="100px"></asp:TextBox>
</td>
</tr>--%>
<tr>
<td>
Total Accepted AWBs
</td>
<td>
<asp:TextBox ID="txtTotalAcceptedAWBs" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
</td>
</tr>
<tr>
<td>
    Flown as Booked</td>
<td>
<asp:TextBox ID="txtFLAB" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
</td>
</tr>
<tr>
<td>
FLAB%
</td>
<td>
<asp:TextBox ID="txtFLABper" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
</td>
</tr>
<tr>
<td>Not Flown as Booked</td>
<td>
<asp:TextBox ID="txtNotFLAB" runat="server" OnChange='javascript:percentcal();' ReadOnly="true" Width="100px"></asp:TextBox>
</td>

</tr>
</table>
</div>

<div>
    <asp:GridView ID="grvNFLABList" runat="server" ShowFooter="false" Width="80%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" 
         OnRowCommand="grvNFLABList_RowCommand"
        onrowediting="grvNFLABList_RowEditing" onpageindexchanging="grvNFLABList_PageIndexChanging">
           
            <Columns>
            
             <asp:TemplateField HeaderText="AWB Prefix" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAWBPrefix" runat="server" Text = '<%# Eval("AWBPrefix") %>'/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="AWBNumber" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAWBNumber" runat="server" Text = '<%# Eval("AWBno") %>'/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Org" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblOrg" runat="server" Text = '<%# Eval("Origin") %>'/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
                         
             <asp:TemplateField HeaderText="Dest" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDestinationCode" runat="server" Text = '<%# Eval("Dest") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <%--<asp:TemplateField HeaderText="AgentCode" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblAgentCode" runat="server" Text = '<%# Eval("AgentCode") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>--%>
             
             <asp:TemplateField HeaderText="Bkd.Flt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblBkdflt" runat="server" Text = '<%# Eval("BkdFlt") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Bkd.Pcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblBkdPcs" runat="server" Text = '<%# Eval("BkdPcs") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Bkd.Wt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblBkdWt" runat="server" Text = '<%# Eval("BkdWt") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Bkd.Dt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblBkdDt" runat="server" Text = '<%# Eval("BkdDt") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Flown.Flt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblFlownFlt" runat="server" Text = '<%# Eval("FlownFlt") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Flown.Loc" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblflownLoc" runat="server" Text = '<%# Eval("FlownLoc") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <%--<asp:TemplateField HeaderText="FromDate" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblFromDate" runat="server" Text = '<%# Eval("ValidFrom") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="ToDate" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblToDate" runat="server" Text = '<%# Eval("ValidTo") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             --%>
             
             
             <asp:TemplateField HeaderText="Flown.Pcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblFlownPcs" runat="server" Text = '<%# Eval("FlownPcs") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Flown.Wt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblFlownWt" runat="server" Text = '<%# Eval("FlownWt") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Flown.Dt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblFlownDt" runat="server" Text = '<%# Eval("FlownDt") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             
            <%-- <asp:TemplateField HeaderText="OffloadPcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblOffloadPcs" runat="server" Text = '<%# Eval("OffloadPcs") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
              <asp:TemplateField HeaderText="ActWeight" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblActWeight" runat="server" Text = '<%# Eval("ActWeight") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Offloadweight" HeaderStyle-Wrap="true">
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
             
            <%-- <asp:ButtonField CommandName="Process" Text="ReProcess">
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
