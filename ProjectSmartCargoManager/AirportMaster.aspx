<%@ Page Title="AirportMaster" Language="C#" AutoEventWireup="true"  MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="AirportMaster.aspx.cs" Inherits="ProjectSmartCargoManager.AirportMaster" %>

<%@ Register assembly="TimePicker" namespace="MKB.TimePicker" tagprefix="MKB" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
    function GetCode() {
        var origin = document.getElementById('<%= txtAirportCode.ClientID%>');
        if (origin.value.length > 4) {
            origin.value = origin.value.substring(origin.value.length - 4);
            origin.value = origin.value.replace(')', '');

        }
    }
</script>
 <script language="javascript" type="text/javascript" src="scripts/jquery.min.js"></script>

    
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
       
        .style1
        {
            width: 782px;
        }
       
        .style2
        {
            height: 27px;
        }
        .style3
        {
            width: 91px;
        }
        .style4
        {
            height: 27px;
            width: 91px;
        }
        .style5
        {
            height: 27px;
            width: 105px;
        }
        .style6
        {
            width: 105px;
        }
        .style7
        {
            height: 27px;
            width: 111px;
        }
        .style8
        {
            width: 111px;
        }
       
        </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
  
    <div id="contentarea">
    <div class="msg">
    <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
    </div>
   <h1>Airport Master</h1>
  <asp:Label ID="Label1" runat="server" Visible="False"></asp:Label>

<asp:Panel ID="pnlGrid"  runat="server">
<div style="overflow:auto;">
<asp:GridView ID="grvAirportList" runat="server" Width="100%" AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" AllowPaging="True" 
onrowcommand="grvAirportList_RowCommand" onrowediting="grvAirportList_RowEditing" onpageindexchanging="grvAirportList_PageIndexChanging" 
HeaderStyle-CssClass="HeaderStyle"  CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" AlternatingRowStyle-CssClass="AltRowStyle" 
PagerStyle-CssClass="PagerStyle" SelectedRowStyle-CssClass="SelectedRowStyle" >
           
            <Columns>
             <asp:TemplateField HeaderText="Country Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblCountryCode" runat="server" Text = '<%# Eval("CountryCode") %>'/>
                    </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Region" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblRegion" runat="server" Text = '<%# Eval("RegionCode") %>'/>
                    </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Airport Name" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblAirportName" runat="server" Text = '<%# Eval("AirportName") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Airport Code" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblAirportCode" runat="server" Text = '<%# Eval("AirportCode") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="City" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblCity" runat="server" Text = '<%# Eval("CityCode") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
              <asp:TemplateField HeaderText="GL Account" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblGLAccount" runat="server" Text = '<%# Eval("GLAccountCode") %>'/>
                    </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Station Email ID" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblStation" runat="server" Text = '<%# Eval("StationMailId") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Manager Name" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblManager" runat="server" Text = '<%# Eval("ManagerName") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Manager Email ID" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblManagerEmailId" runat="server" Text = '<%# Eval("ManagerEmailId") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Shift Mobile No" HeaderStyle-Wrap="true" ItemStyle-Width="70px">
             <ItemTemplate>
             <asp:Label ID="lblShiftMobNo" runat="server" Text = '<%# Eval("ShiftMobNo") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Landline No" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblLandlineNo" runat="server" Text = '<%# Eval("LandlineNo") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Manager mobile No" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblManagerMobNo" runat="server" Text = '<%# Eval("ManagerMobNo") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblStatus" runat="server" Text = '<%# Eval("IsActive") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Counter" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblCounter" runat="server" Text = '<%# Eval("counter") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Transit Time" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblTransitTime" runat="server" Text = '<%# Eval("TransitTime") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="CutOff Time" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblCutOffTime" runat="server" Text = '<%# Eval("CutOffTime") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="City Type" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblCityType" runat="server" Text = '<%# Eval("CityType") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
            <asp:ButtonField CommandName="Edit" Text="Edit">
             <ItemStyle Width="50px" />
             </asp:ButtonField>
            </Columns>
    </asp:GridView>
    <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" OnClick="btnExport_Click"
   Visible="false" />
    </div>
</asp:Panel>
<asp:Panel ID="pnlNew" runat="server"><div class="botline">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
<tr><td class="style1">
<table cellpadding="0" cellspacing="3">
    <tr>
        <td class="style3">
            <tr>
                <td class="style4">
                    Airport Code*
                </td>
                <td class="style2">
                    <asp:TextBox ID="txtAirportCode" runat="server"  MaxLength="3" Onchange="javascript:GetCode();">
   </asp:TextBox>
                    <asp:AutoCompleteExtender ID="AirportCode" runat="server" 
                        BehaviorID="AirportCode" CompletionInterval="0" CompletionSetCount="10" 
                        EnableCaching="false" MinimumPrefixLength="1" ServiceMethod="GetStation" 
                        TargetControlID="txtAirportCode">
                    </asp:AutoCompleteExtender>
                </td>
                <td class="style5">
                    Airport Name *</td>
                <td class="style2">
                    <asp:TextBox ID="txtAirportName" runat="server" MaxLength="30">
   </asp:TextBox>
                </td>
                <td class="style7">
                    City*
                </td>
                <td class="style2">
                    <asp:TextBox ID="txtCity" runat="server" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    Country Code*
                </td>
                <td>
                    <asp:DropDownList ID="ddlCountryCode" runat="server" AutoPostBack="True" Width="100px"
                        onselectedindexchanged="ddlCountryCode_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td class="style5">
                    Region*</td>
                <td>
                    <asp:DropDownList ID="ddlRegion" runat="server">
                        <asp:ListItem>Select</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="style7">
                    Active
                </td>
                <td>
                    <asp:CheckBox ID="chkActive" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="style4">
                    Manager Name</td>
                <td class="style2">
                    <asp:TextBox ID="txtManagerName" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                <td class="style5">
                    Manager Email ID</td>
                <td class="style2">
                    <asp:TextBox ID="txtManagerEmail" runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td class="style7">
                    Station Email ID</td>
                <td class="style2">
                    <asp:TextBox ID="txtStationEmail" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    Shift Mobile No</td>
                <td class="style2">
                    <asp:TextBox ID="txtShiftMobNo" runat="server" MaxLength="15"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                        ControlToValidate="txtShiftMobNo" ErrorMessage="*" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                </td>
                <td class="style5">
                    Landline No.</td>
                <td class="style2">
                    <asp:TextBox ID="txtLandlineNo" runat="server" MaxLength="15"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                        ControlToValidate="txtLandlineNo" ErrorMessage="*" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                </td>
                <td class="style7">
                    Manager Mob.No </td>
                <td class="style2">
                    <asp:TextBox ID="txtManagerMobNo" runat="server" MaxLength="15"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" 
                        ControlToValidate="txtManagerMobNo" ErrorMessage="*" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    <span>Counter/Office Time</span></td>
                <td>
                    <asp:TextBox ID="txtCounter" runat="server"></asp:TextBox>
                </td>
                <td class="style6">
                    Service Tax Exempted</td>
                <td>
                    <asp:CheckBox ID="chkExempted" runat="server" />
                </td>
                <td class="style8">
                    GL Account Code</td>
                <td>
                    <asp:DropDownList ID="ddlGLAccountCode" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
            <td class="style4">Latitude</td>
            <td class="style2">
                <asp:TextBox ID="txtLatitude" runat="server"></asp:TextBox>
            </td>
            <td class="style5">Longitude</td>
            <td class="style2">
                <asp:TextBox ID="txtLongitude" runat="server"></asp:TextBox>
            </td>
            <td class="style7">ULD Enabled</td>
            <td class="style2">
                <asp:CheckBox ID="chkIsULDEnabled" runat="server" />
            </td>
            </tr>
        </td>
    </tr>
    
    </table></td>
<td><div class="divback" style="font-weight:bold;">
<table  cellspacing="1">
<tr>
        <td>
            Transit Time</td>
        <td>
           <asp:TextBox ID="txtTransitTimeHr" runat="server" Width="30px" ToolTip="Cost" MaxLength="2"></asp:TextBox> 
         <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"  
            TargetControlID="txtTransitTimeHr"  
            WatermarkText="HH"  
            WatermarkCssClass="watermarked" />
           
         <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                Enabled="True" FilterType="Numbers" TargetControlID="txtTransitTimeHr" 
                ValidChars="0123456789">
                   
            </asp:FilteredTextBoxExtender>
           
            :
           <asp:TextBox ID="txtTransitTimeMin" runat="server" Width="30px" MaxLength="2"></asp:TextBox>
           <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server"  
            TargetControlID="txtTransitTimeMin"  
            WatermarkText="MM"  
            WatermarkCssClass="watermarked" /> 
           <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                Enabled="True" FilterType="Numbers" TargetControlID="txtTransitTimeMin" 
                ValidChars="0123456789">
            </asp:FilteredTextBoxExtender>
           <%-- <MKB:TimeSelector ID="CtlTransitTime" runat="server" DisplaySeconds="False" 
                SelectedTimeFormat="TwentyFour">
            </MKB:TimeSelector>--%>
        </td></tr>
        <tr>
        <td>
            CutOff Time</td>
        <td>
            <asp:TextBox ID="txtCutOffTimeHr" runat="server" Width="30px" MaxLength="2"></asp:TextBox> 
              <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server"  
            TargetControlID="txtCutOffTimeHr"  
            WatermarkText="HH"  
            WatermarkCssClass="watermarked" /> 
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" 
                Enabled="True" FilterType="Numbers" TargetControlID="txtCutOffTimeHr" 
                ValidChars="0123456789">
            </asp:FilteredTextBoxExtender>
            :
            <asp:TextBox ID="txtCutOffTimeMin" runat="server" Width="30px" MaxLength="2"></asp:TextBox>
              <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server"  
            TargetControlID="txtCutOffTimeMin"  
            WatermarkText="MM"  
            WatermarkCssClass="watermarked" /> 
             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" 
                Enabled="True" FilterType="Numbers" TargetControlID="txtCutOffTimeMin" 
                ValidChars="0123456789">
            </asp:FilteredTextBoxExtender>
            <%--<MKB:TimeSelector ID="CtlCutOffTime" runat="server" DisplaySeconds="False" 
                SelectedTimeFormat="TwentyFour">
            </MKB:TimeSelector>--%>
        </td></tr>
    <tr>
     <td>Metro</td>
        <td>
            <asp:CheckBox ID="isMetro" runat="server" />
        </td>
        </tr>
        <tr>
        <td>Time Zone*</td>
        <td>
            <asp:DropDownList ID="ddlTimeZone" runat="server" AutoPostBack="True" Width="90px">
            </asp:DropDownList>
            </td>
        
        </tr>
        </table>
</div>
    <asp:RangeValidator ID="RangeValidator3" runat="server" 
        ControlToValidate="txtTransitTimeHr" 
        ErrorMessage="Transit Hours should be between 00-23" 
        MaximumValue="23" MinimumValue="00"></asp:RangeValidator>
    <br />
    <asp:RangeValidator ID="RangeValidator4" runat="server" 
        ControlToValidate="txtTransitTimeMin" 
        ErrorMessage="Transit Minutes should be between 00-59" MaximumValue="59" 
        MinimumValue="00"></asp:RangeValidator>
    <br />
    <asp:RangeValidator ID="RangeValidator6" runat="server" 
        ControlToValidate="txtCutOffTimeHr" 
        ErrorMessage="CutOff Hours should be between 00-23" MaximumValue="23" 
        MinimumValue="00"></asp:RangeValidator>
    <br />
    <asp:RangeValidator ID="RangeValidator5" runat="server" 
        ControlToValidate="txtCutOffTimeMin" 
        ErrorMessage="CutOff Minutes should be between 00-59" 
        MaximumValue="59" MinimumValue="00"></asp:RangeValidator>
    <br />
    </td>
        </tr>
<tr><td colspan="2" align="right">
<asp:Button ID="btnList" runat="server" CssClass="button" 
        onclick="btnList_Click" Text="List"  />  <asp:Button ID="btnClear" runat="server" CssClass="button" 
                onclick="btnClear_Click" Text="Clear" /></td></tr>
</table>

    
        </div>
<asp:TabContainer ID="AirportMasterTab" runat="server" ActiveTabIndex="1" 
        Width="1022px"  Font-Names="Calibri,Arial,Helvetica,sans-serif" 
        Font-Size="14">
<asp:TabPanel ID="TPContact" runat="server" HeaderText="TPContact" Width="620px"
                Height="27px">
                <HeaderTemplate>
                    Other Contacts
                </HeaderTemplate>
                <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                       <h3>
                            GHA
                        </h3>
                        <table width="80%" style=" font-family:Calibri,Arial,Helvetica,sans-serif; Font-Size:14;">
                            <tr runat="server">
        <td runat="server">
            Name</td>
        <td runat="server">
            <asp:TextBox ID="txtGHAName" runat="server" MaxLength="50"></asp:TextBox>
        </td>
        <td runat="server">
            Address</td>
        <td runat="server">
            <asp:TextBox ID="txtGHAAddress" runat="server" MaxLength="200"></asp:TextBox>
        </td>
        <td runat="server">
            Email ID</td>
        <td runat="server">
            <asp:TextBox ID="txtGHAEmailID" runat="server" MaxLength="100" ToolTip="Separated by comma (,)"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            Phone No.</td>
        <td>
            <asp:TextBox ID="txtGHAPhone" runat="server" MaxLength="15"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" 
                ControlToValidate="txtGHAPhone" ErrorMessage="*" ValidationExpression="\d+"></asp:RegularExpressionValidator>
        </td>
        <td>
            Mobile No.</td>
        <td>
            <asp:TextBox ID="txtGHAMobNo" runat="server" MaxLength="15"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" 
                ControlToValidate="txtGHAMobNo" ErrorMessage="*" ValidationExpression="\d+"></asp:RegularExpressionValidator>
        </td>
        <td>
            FAX No.</td>
        <td>
            <asp:TextBox ID="txtGHAFaxNo" runat="server" MaxLength="15"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" 
                ControlToValidate="txtGHAFaxNo" ErrorMessage="*" ValidationExpression="\d+"></asp:RegularExpressionValidator>
        </td>
    </tr>
    </table>
    <asp:Panel ID="Panel2" runat="server">
                        <h3>
                            <br />
                            GSA
                        </h3></asp:Panel>
                        <table width="80%" style=" font-family:Calibri,Arial,Helvetica,sans-serif; Font-Size:14;">
                        
                            <tr>
        <td>
            Name</td>
        <td>
            <asp:TextBox ID="txtGSAName" runat="server" MaxLength="50"></asp:TextBox>
        </td>
        <td>
            Address</td>
        <td>
            <asp:TextBox ID="txtGSAAddress" runat="server" MaxLength="200"></asp:TextBox>
        </td>
        
          <td>
            Email ID</td>
        <td>
            <asp:TextBox ID="txtGSAEmailID" runat="server" MaxLength="100" ToolTip="Separated by comma (,)"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            Phone No.</td>
        <td>
            <asp:TextBox ID="txtGSAPhone" runat="server"  MaxLength="15"
                ontextchanged="txtGSAPhone_TextChanged"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" 
                ControlToValidate="txtGSAPhone" ErrorMessage="*" ValidationExpression="\d+"></asp:RegularExpressionValidator>
        </td>
        <td>
            Mobile No.</td>
        <td>
            <asp:TextBox ID="txtGSAMobNo" runat="server" MaxLength="15"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator10" 
                runat="server" ControlToValidate="txtGSAMobNo" ErrorMessage="*" 
                ValidationExpression="\d+"></asp:RegularExpressionValidator>
        </td>
          <td>
            FAX No.</td>
        <td>
            <asp:TextBox ID="txtGSAFaxNo" runat="server" MaxLength="15"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" 
                runat="server" ControlToValidate="txtGSAFaxNo" ErrorMessage="*" 
                ValidationExpression="\d+"></asp:RegularExpressionValidator>
        </td>
    </tr>
    </table><asp:Panel ID="Panel3" runat="server">
                        <h3>
                            <br />
                            APM
                       </h3></asp:Panel>
                 <table width="80%" style=" font-family:Calibri,Arial,Helvetica,sans-serif; Font-Size:14;">           
                 <tr>
        <td>
            Name</td>
        <td>
            <asp:TextBox ID="txtAPMName" runat="server" MaxLength="50"></asp:TextBox>
        </td>
        <td>
            Address</td>
        <td>
            <asp:TextBox ID="txtAPMAddress" runat="server" MaxLength="200"></asp:TextBox>
        </td>
        <td>
            Email ID</td>
        <td>
            <asp:TextBox ID="txtAPMEmailID" runat="server" MaxLength="100" ToolTip="Separated by comma (,)"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            Phone No.</td>
        <td>
            <asp:TextBox ID="txtAPMPhone" runat="server" MaxLength="15"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" 
                runat="server" ControlToValidate="txtAPMPhone" ErrorMessage="*" 
                ValidationExpression="\d+"></asp:RegularExpressionValidator>
        </td>
        <td>
            Mobile No.</td>
        <td>
            <asp:TextBox ID="txtAPMMobNo" runat="server" MaxLength="15"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" 
                runat="server" ControlToValidate="txtAPMMobNo" ErrorMessage="*" 
                ValidationExpression="\d+"></asp:RegularExpressionValidator>
        </td>
         <td>
            FAX No.</td>
        <td>
            <asp:TextBox ID="txtAPMFaxNo" runat="server" MaxLength="15"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator14" 
                runat="server" ControlToValidate="txtAPMFaxNo" ErrorMessage="*" 
                ValidationExpression="\d+"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td>
            Additional
            <br />
            Information</td>
        <td colspan="5">
            <asp:TextBox ID="txtAdditionalInfo" runat="server" TextMode="MultiLine" Width="350px" MaxLength="500"></asp:TextBox>
        </td>
    </tr>
                            
                        </table>
                    </asp:Panel>
                </ContentTemplate>
                    
                </asp:TabPanel>
    <asp:TabPanel ID="TPCurrency" runat="server" HeaderText="TPCurrency">
    <HeaderTemplate>Currency</HeaderTemplate>
    <ContentTemplate>
    <asp:Panel ID="CurrencyPanle" runat="server">
    <h3> Booking Currency</h3>
    <table cellpadding="3" cellspacing="3"  width="60%">
    <tr>
    <td>
    Booking Currency
    </td>
    <td>
    <asp:DropDownList ID="ddlBookingCurrency" runat="server" Width="90px" >
        <asp:ListItem>All</asp:ListItem>
    </asp:DropDownList>
    </td>
    
    <td>
    Booking Currency Type
    </td>
    <td>
    <asp:DropDownList ID="ddlBookingCurrencyType" runat="server" Width="90px">
        <asp:ListItem>All</asp:ListItem>
        </asp:DropDownList>
    </td>
    </tr>
    </table><br />
    <h3> Invoice Currency</h3>
    <table cellpadding="3" cellspacing="3" width="60%" >
    <tr>
    <td>
    Invoice Currency
    </td>
    <td>
    <asp:DropDownList ID="ddlInvoiceCurrency" runat="server" Width="90px"></asp:DropDownList>
    </td>
   
    <td>
    Invoice Currency Type
    </td>
    <td>
    <asp:DropDownList ID="ddlInvoiceCurrencyType" runat="server" Width="90px">
        <asp:ListItem>All</asp:ListItem>
        </asp:DropDownList>
    </td>
    </tr>
    </table>
    </asp:Panel>
    </ContentTemplate>
    
    </asp:TabPanel>
</asp:TabContainer>
<br />
            <asp:Button ID="btnSave" runat="server" CssClass="button" 
                onclick="btnSave_Click" Text="Save" />
           
      
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