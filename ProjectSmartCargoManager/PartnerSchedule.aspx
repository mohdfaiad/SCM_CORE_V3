<%@ Page Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="PartnerSchedule.aspx.cs" Inherits="ProjectSmartCargoManager.PartnerSchedule" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

.imgHover .hover { 
    display: none;
    position:absolute;
    z-index: 2;

}


 p.MsoNormal
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:10.0pt;
	margin-left:0in;
	line-height:115%;
	font-size:11.0pt;
	font-family:"Calibri","sans-serif";
	}
    </style>
    
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM0" runat="server">  </asp:ToolkitScriptManager>

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
            height: 36px;
        }
    </style>
    
      <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
     <div  class="imgHover" style="text-align:center;">
  
                  

                    </div>

     <div id="contentarea">
        <h1>
            <%--<img alt="" src="Images/flightshnew.png"  style="vertical-align:5"/>--%> 
            New Flight Schedule</h1>
           <%--    <asp:updateprogress id="UpdateProgress1" runat="server" associatedupdatepanelid="UpdatePanel1" dynamiclayout="true">
                        <progresstemplate>

                           <img src="Images/loading.gif" alt="loading" height="32px" width="32px"/>

                        </progresstemplate>
                    </asp:updateprogress>--%>
            
           
            <div>
                <table width="70%">
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
                                Font-Size="Large" ForeColor="Red"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            Partner Type</td>
                        <td>
                            <asp:DropDownList ID="drpPartnerType" runat="server" Width="80px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Partner Code</td>
                        <td>
                            <asp:TextBox ID="txtPartnerCode" runat="server" Width="100px"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="txtPartnerCode_AutoCompleteExtender" 
                                runat="server" EnableCaching="true" Enabled="True" MinimumPrefixLength="1" 
                                ServiceMethod="GetStation" ServicePath="~/Home.aspx" 
                                TargetControlID="txtPartnerCode">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            Flight #&nbsp; *</td>
                        <td>
                            <asp:TextBox ID="ddlFlight" runat="server" MaxLength="7" Width="114px"></asp:TextBox>
                        </td>
                        <td>
                            Aircraft Type&nbsp;&nbsp; *</td>
                        <td>
                            <asp:DropDownList ID="ddlAirCraftType" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="ddlAirCraftType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Free Sale&nbsp; (kg)&nbsp; *</td>
                        <td>
                            <asp:TextBox ID="txtCargoCapacity" runat="server" MaxLength="6" Width="108px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Origin&nbsp;&nbsp; *</td>
                        <td>
                            <div>
                                <asp:DropDownList ID="ddlOrigin" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="ddlOrigin_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            Destination&nbsp; *</td>
                        <td>
                            <asp:DropDownList ID="ddlDestination" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="ddlDestination_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            From date *</td>
                        <td>
                            <asp:TextBox ID="txtFromdate" runat="server" AutoPostBack="True" 
                                ontextchanged="txtFromdate_TextChanged" Width="114px"></asp:TextBox>
                            <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" 
                                Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFromdate" PopupButtonID="imgFromDate">
                            </asp:CalendarExtender>
                             <asp:ImageButton ID="imgFromDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                        </td>
                        <td>
                            To Date&nbsp;&nbsp; *</td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" AutoPostBack="True" 
                                ontextchanged="txtToDate_TextChanged" Width="114px"></asp:TextBox>
                            <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" 
                                Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtToDate" PopupButtonID="imgToDate">
                            </asp:CalendarExtender>
                            <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbtDoeastic" runat="server" Checked="True" GroupName="A" 
                                Text="Domestic" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbtInternational" runat="server" GroupName="A" 
                                Text="International" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <h2>
                Route Details</h2>
            <div>
                <%--<div>
                    <table width="100%">
                        <tr>
                            <td width="40%">
                            </td>
                            <td width="40%">
                            </td>
                            <td align="right" width="10%">
                                <asp:Button ID="btnAddNewRow" runat="server" CssClass="button" 
                                    onclick="btnAddNewRow_Click" Text="Add New" />
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnDelete" runat="server" CssClass="button" 
                                    onclick="btnDelete_Click" Text="Delete" />
                            </td>
                        </tr>
                    </table>
                </div>--%>
               
                <asp:Panel ID="gridpanel" runat="server" ScrollBars="Horizontal">
                  <asp:UpdatePanel runat="server" ID="UpdPan1">
                  <ContentTemplate>
                      <h1>
                      </h1>
                    <asp:GridView ID="grdScheduleinfo" runat="server" AutoGenerateColumns="False" 
                        ShowFooter="True" Width="80%">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CHK" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="From  *">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlFromOrigin" runat="server" Width="45px">
                                    </asp:DropDownList>
                                    <%--<asp:TextBox  ID="txtSource" runat="server" Width="70px" ></asp:TextBox>--%>
                                    <%--<asp:TextBox ID="txtbankname" runat="server"></asp:TextBox>--%>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True" />
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="To  *">
                                <ItemTemplate>
                                    <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                                    <asp:DropDownList ID="ddlToDest" runat="server" Width="45px">
                                    </asp:DropDownList>
                                    <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True" />
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Dept Time  *" 
                                ItemStyle-Width="10%">
                                <ItemTemplate>
                                    Day
                                    <asp:TextBox ID="txtDeptDay" runat="server" DataTextField="" Width="8px"></asp:TextBox>
                                    <asp:NumericUpDownExtender ID="NumericUpDownExtender_DeptDay" runat="server" 
                                        Maximum="2" Minimum="1" RefValues="" ServiceDownMethod="" ServiceUpMethod="" 
                                        TargetButtonDownID="" TargetButtonUpID="" TargetControlID="txtDeptDay" 
                                        Width="40" />
                                    Hr
                                    <asp:TextBox ID="txtDeptTimeHr" runat="server" DataTextField="" Width="8px"></asp:TextBox>
                                    <asp:NumericUpDownExtender ID="txtDeptTimeHr_NumericUpDownExtender" 
                                        runat="server" Maximum="23" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtDeptTimeHr" Width="40" />
                                    : Min
                                    <asp:TextBox ID="txtDeptTimeMin" runat="server" DataTextField="" Width="8px"></asp:TextBox>
                                    <asp:NumericUpDownExtender ID="txtDeptTimeMin_NumericUpDownExtender1" 
                                        runat="server" Maximum="60" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtDeptTimeMin" Width="40" />
                                </ItemTemplate>
                                <HeaderStyle Wrap="False" />
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Arrival Time  *" 
                                ItemStyle-Width="20%">
                                <ItemTemplate>
                                    Day
                                    <asp:TextBox ID="txtArrivalDay" runat="server" DataTextField="" Width="8px"></asp:TextBox>
                                    <asp:NumericUpDownExtender ID="NumericUpDownExtender_ArrivalDay" runat="server" 
                                        Maximum="2" Minimum="1" RefValues="" ServiceDownMethod="" ServiceUpMethod="" 
                                        TargetButtonDownID="" TargetButtonUpID="" TargetControlID="txtArrivalDay" 
                                        Width="40" />
                                    Hr<asp:TextBox ID="txtArrivaltimeHr" runat="server" Width="8px"></asp:TextBox>
                                    <asp:NumericUpDownExtender ID="txtArrivaltimeHr_NumericUpDownExtender" 
                                        runat="server" Maximum="23" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtArrivaltimeHr" Width="40" />
                                    : Min<asp:TextBox ID="txtArrivalTimeMin" runat="server" Width="8px"></asp:TextBox>
                                    <asp:NumericUpDownExtender ID="txtArrivalTimeMin_NumericUpDownExtender1" 
                                        runat="server" Maximum="60" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtArrivalTimeMin" Width="40" />
                                    <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtbankguranteeamt"  ErrorMessage="Please Enter Numeric Value"  ValidationExpression="^\d+$" ValidationGroup="check">
                        </asp:RegularExpressionValidator>--%>
                                </ItemTemplate>
                                <HeaderStyle Wrap="True" />
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Frequency  *" 
                                ItemStyle-Width="40%">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkMon" runat="server" Checked="false" Text="Mo" />
                                    <asp:CheckBox ID="chkTues" runat="server" Checked="false" Text="Tu" />
                                    <asp:CheckBox ID="chkwed" runat="server" Checked="false" Text="We" />
                                    <asp:CheckBox ID="chkThur" runat="server" Checked="false" Text="Th" />
                                    <asp:CheckBox ID="chkFri" runat="server" Checked="false" Text="Fr" />
                                    <asp:CheckBox ID="chkSat" runat="server" Checked="false" Text="Sa" />
                                    <asp:CheckBox ID="chkSun" runat="server" Checked="false" Text="Su" />
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <HeaderStyle Wrap="True" />
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="AirCraft Type*" 
                                ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlAirCraft" runat="server" AutoPostBack="True" 
                                        OnSelectedIndexChanged="showCapacityInGrid">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <HeaderStyle Wrap="True" />
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Tail No" ItemStyle-Width="30%">
                               <ItemTemplate>
                                   <asp:DropDownList ID="ddlTailNo" runat="server">
                                   <asp:ListItem Selected Value="Select">
                                   </asp:ListItem>
                                   </asp:DropDownList>
                               </ItemTemplate>
                               <FooterStyle HorizontalAlign="Right" />
                               <HeaderStyle Wrap="true" />
                               <ItemStyle Wrap="false" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Capacity(Kg)*" 
                                ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCapacity" runat="server" MaxLength="6" Text="" Width="50px"></asp:TextBox>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <HeaderStyle Wrap="True" />
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Status  *">
                                <FooterTemplate>
                                    <asp:Button ID="btnAdd" runat="server" CssClass="button" OnClick="Addrow" 
                                        Text="Add New" visible="false" />
                                </FooterTemplate>
                                <ItemTemplate>
                                    <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                                    <asp:DropDownList ID="ddlStatus" runat="server" Height="18px" Width="60px">
                                        <asp:ListItem Value="ACTIVE"></asp:ListItem>
                                        <asp:ListItem Value="CANCELLED"></asp:ListItem>
                                        <asp:ListItem Value="DRAFT"></asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" ></asp:TextBox>--%>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                                <HeaderStyle Wrap="True" />
                                <ItemStyle Wrap="False" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="titlecolr" />
                        <RowStyle HorizontalAlign="Center" />
                        <AlternatingRowStyle HorizontalAlign="Center" />
                    </asp:GridView>
                    </ContentTemplate>
                   <Triggers>
                   <asp:AsyncPostBackTrigger ControlID="btnAddNewRow"  EventName="click" />
                   <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="click" />
                   </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>
            <div ID="fotbut">
                <asp:Button ID="btnSave" runat="server" CssClass="button" 
                    onclick="btnSave_Click" Text="Save" />
                       <asp:Button ID="btnAddNewRow" runat="server" CssClass="button" 
                                    onclick="btnAddNewRow_Click" Text="Add" />
                                     <asp:Button ID="btnDelete" runat="server" CssClass="button" 
                                    onclick="btnDelete_Click" Text="Delete" />
                    &nbsp;
                <asp:FileUpload ID="FileUpload_SSIM" runat="server" />
                <asp:Button ID="btnSSIM" runat="server" CssClass="button" 
                    onclick="btnSSIM_Click" Text="SSIM Import" />
                    <asp:Button ID="BtnCLose" runat="server" CssClass="button" 
                    Text="Close" onclick="BtnCLose_Click" Visible="false" />
                   
                             
                            
                               
                            
            </div>
            <h1>
            </h1>
            <h1>
            </h1>
            <h1>
            </h1>
            <h1>
            </h1>
             </h1>
             
            
            </h1>
             
             </h1>
             
     
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
    <asp:PostBackTrigger ControlID="btnSSIM" />
    </Triggers>
    </asp:UpdatePanel>

</asp:Content>
