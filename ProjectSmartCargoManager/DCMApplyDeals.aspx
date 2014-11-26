<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DCMApplyDeals.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.DCMApplyDeals" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
    function RadioCheck(rb) {
        var gv = document.getElementById("<%=GrdDealDetails.ClientID%>");
        var rbs = gv.getElementsByTagName("input");

        var row = rb.parentNode.parentNode;
        for (var i = 0; i < rbs.length; i++) {
            if (rbs[i].type == "radio") {
                if (rbs[i].checked && rbs[i] != rb) {
                    rbs[i].checked = false;
                    break;
                }
            }
        }
    }

    function ViewPanelSplit() {
        document.getElementById('Lightsplit').style.display = 'block';
        document.getElementById('fadesplit').style.display = 'block';
    }
    function HidePanelSplit() {
        document.getElementById('Lightsplit').style.display = 'none';
        document.getElementById('fadesplit').style.display = 'none';
    }

    function ViewPanelSplitTonnage() {
        document.getElementById('LightTonnageSlab').style.display = 'block';
        document.getElementById('FadeTonnageSlab').style.display = 'block';
    }
    function HidePanelSplitTonnage() {
        document.getElementById('LightTonnageSlab').style.display = 'none';
        document.getElementById('FadeTonnageSlab').style.display = 'none';
    }
    function ViewPanelSplitPreview() {
        document.getElementById('LightPreview').style.display = 'block';
        document.getElementById('FadePreview').style.display = 'block';
    }
    function HidePanelSplitPreview() {
        document.getElementById('LightPreview').style.display = 'none';
        document.getElementById('FadePreview').style.display = 'none';
    }
</script> 

<style>
.black_overlaynew
		{
			display: none;
			position:absolute;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 1000px;
			background-color: black;
			z-index:1001;
			-moz-opacity:0.8;
			opacity:0.4;
			filter:alpha(opacity=80);
			float:left;
		}
	.white_contentnew 
		{
		    margin:0 auto;
			display: none;
			position:fixed;
			top: 5%;
			left: 40%;
			padding: 16px;
			border: 16px solid #ccdce3;
			background-color: white;
			z-index:1002;
		
            
		}
		
		.white_content 
		{
		    margin:0 auto;
			display: none;
			position:fixed;
			top: 5%;
			left: 10%;
			height: 81%;
			padding: 24px;
			border: 16px solid #ccdce3;
			background-color: white;
			z-index:1002;
		
            
		}
		
		
</style> 

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
    
    <h1><img src="Images/txt_agent.png" />
    <br />
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
             ForeColor="Red"></asp:Label>
     </h1>
     <div class="botline">
    <table width="60%">
     <tr>
      <td>
       Agent Code
      </td>
      <td>
          <asp:DropDownList ID="ddlAgentCode" runat="server" Width="95px" >
          </asp:DropDownList>
      </td>
      
      <td>
          From
      </td>
      <td>
          
          &nbsp;<asp:TextBox ID="txtDealFrom" runat="server" Width="70px"></asp:TextBox>
                          <asp:ImageButton ID="btnFromDate" runat="server" ImageAlign="AbsMiddle" 
                              ImageUrl="~/Images/calendar_2.png" />
           <asp:CalendarExtender ID="txtDealFrom_CalendarExtender" runat="server" 
          Enabled="True" TargetControlID="txtDealFrom" Format="dd-MM-yyyy" PopupButtonID="btnFromDate" PopupPosition="BottomLeft">
          </asp:CalendarExtender>
         
      </td>
      
       <td>
           To
      </td>
      <td>
          
          <asp:TextBox ID="txtDealTo" runat="server" Width="70px"></asp:TextBox>
                          <asp:ImageButton ID="btnFromDate0" runat="server" ImageAlign="AbsMiddle" 
                              ImageUrl="~/Images/calendar_2.png" />
          <asp:CalendarExtender ID="txtDealTo_CalendarExtender" runat="server" 
          Enabled="True" TargetControlID="txtDealTo" Format="dd-MM-yyyy" PopupButtonID="btnFromDate0" PopupPosition="BottomLeft">
          </asp:CalendarExtender>
      </td>
    
      <td>
          
           <asp:Button ID="btnList" runat="server" CssClass="button" 
                Text="List" onclick="btnList_Click" />
      </td>
    
      <td>
          
           <asp:Button ID="btnClear" runat="server" CssClass="button" 
                Text="Clear" OnClick="btnClear_Click" />
         </td>
    
     </tr>
     </table> 
    
    </div>
    <div style="float:left">
        <asp:GridView ID="GrdDealDetails" runat="server" AutoGenerateColumns="False" 
            ShowFooter="True"    AllowPaging="False" 
             PageSize="20" onrowcommand="GrdDealDetails_RowCommand" 
             >
            <Columns>
                
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:radiobutton ID="ChkSelect" runat="server" onclick="javascript:RadioCheck(this)" />
                    </ItemTemplate>
                </asp:TemplateField>
                             
                <asp:TemplateField HeaderText="Deal ID" HeaderStyle-Wrap="true" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblDealID" runat="server" Text='<%# Eval("DealID") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Deal ID" HeaderStyle-Wrap="true" >
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDealID" runat="server" CommandArgument='<%# Eval("DealID") %>' CommandName="DealDetails" Text='<%# Eval("DealID") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Deal Type" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblDealType" runat="server" Text='<%# Eval("DealType") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Tonnage Slab" HeaderStyle-Wrap="false" Visible="false">
                    <ItemTemplate>
                    <asp:LinkButton ID="lnkTonnage" Text="Tonnage" runat="server" CommandArgument='<%# Eval("DealID") %>' CommandName="Tonnage"></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Exclusions" HeaderStyle-Wrap="false" Visible="false">
                    <ItemTemplate>
                    <asp:LinkButton ID="lnkExclusions" Text="Exclusion" runat="server" CommandArgument='<%# Eval("DealID") %>' CommandName="Exclusion"></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="SpotRate Tonnage" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblSpotRate" runat="server" Text='<%# Eval("SpotRateTonnageIsInclude") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Incentive Amount(Tonnage)" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblKickAmt" runat="server" Text='<%# Eval("KickbackAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                    <asp:TemplateField HeaderText="Incentive Amount(AWB)" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblFlatAmt" runat="server" Text='<%# Eval("FlatAmount") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Treshold" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblTreshold" runat="server" Text='<%# Eval("Threshold") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Applicable From" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblFrom" runat="server" Text='<%# Eval("ApplicableFrom") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Applicable To" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblTo" runat="server" Text='<%# Eval("ApplicableTo") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Deal Amount" HeaderStyle-Wrap="true" >
                    <ItemTemplate>
                        <asp:Label ID="lblDealAmount" runat="server"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Preview Deal" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Button  ID="btnPreviewDeal" runat="server" CommandArgument='<%# Eval("DealID") %>' CommandName="Preview" CssClass="button" Text="Preview" />
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                 
           </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>
    </div>
          <asp:Button ID="btnApplyDeals" runat="server" CssClass="button" 
                Text="Apply Deal" onclick="btnApplyDeals_Click" Width="200px" Visible="false"/>
                 &nbsp;<asp:Button ID="btnPrintApplicableTonnageDeal" Visible="false" 
            runat="server" CssClass="button" Width="200px" Text="Print Tonnage Deal Report" 
            onclick="btnPrintApplicableTonnageDeal_Click"  />
                  <asp:Button ID="btnPrintAWBSummary" Visible="false" 
            runat="server" CssClass="button" Width="200px" 
            Text="Print Tonnage Deal AWB Summary" onclick="btnPrintAWBSummary_Click" 
              />
                  &nbsp;<asp:Button ID="btnPrintKickBackDeal" Width="200px" runat="server" 
            Visible="false" CssClass="button" Text="Print Incentive(Tonnage) Report" 
            onclick="btnPrintKickBackDeal_Click"  />
                   &nbsp;<asp:Button Width="200px" ID="btnPrintFlatAWBRate" runat="server" 
            Visible="false" CssClass="button" Text="Print Incentive(AWB) Report" 
            onclick="btnPrintFlatAWBRate_Click"  />
            &nbsp;<asp:Button ID="btnClose" Visible="false" runat="server" CssClass="button" Text="Close" PostBackUrl="~/Home.aspx" />
     </div> 
     
     <div id="Lightsplit"  class="white_contentnew">
    <asp:GridView ID="grdExclusion" runat="server" AutoGenerateColumns="false" HeaderStyle-Wrap="false">
    <Columns>
    <asp:TemplateField HeaderText="Exclusion Type" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblExclusionType" runat="server" Text='<%# Eval("ParamName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
               <asp:TemplateField HeaderText="Exclude" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblToExclude" runat="server" Text='<%# Eval("ParamValue") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField  HeaderStyle-Wrap="false">
               <ItemTemplate>
                        <asp:Label ID="lblSource" runat="server" Text='<%# Eval("IsExclude") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
    </Columns>
    <AlternatingRowStyle   Wrap="false"/>
                                                    <EditRowStyle CssClass="grdrowfont" />
                                                    <FooterStyle CssClass="grdrowfont" />
                                                    <HeaderStyle CssClass="titlecolr" Wrap="False" />
                                                    <RowStyle CssClass="grdrowfont"  HorizontalAlign="Center" Wrap="False" />
    </asp:GridView>
    
    <asp:GridView ID="grdTonnageSlab" runat="server" AutoGenerateColumns="false" HeaderStyle-Wrap="false">
		
    <Columns>
    <asp:TemplateField HeaderText="Tonnage" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblTonnageSlab" runat="server" Text='<%# Eval("Tonnage") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
               <asp:TemplateField HeaderText="Rate" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblRate" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                
    </Columns>
    <AlternatingRowStyle   Wrap="false"/>
                                                    <EditRowStyle CssClass="grdrowfont" />
                                                    <FooterStyle CssClass="grdrowfont" />
                                                    <HeaderStyle CssClass="titlecolr" Wrap="False" />
                                                    <RowStyle CssClass="grdrowfont"  HorizontalAlign="Center" Wrap="False" />
    </asp:GridView>
    
        <asp:Button ID="btnCloseExclusion" runat="server" OnClientClick="javascript:HidePanelSplit()" CssClass="button" Text="Close" />

    

		</div>
		<div id="fadesplit" class="black_overlaynew"></div>
		
		<div id="LightTonnageSlab" class="white_content">
		
       <asp:Button ID="btnCloseTonnage" runat="server" OnClientClick="javascript:HidePanelSplitTonnage()"  CssClass="button" Text="Close" />

    </div>
   <div id="FadeTonnageSlab" class="black_overlaynew"></div>
   <div id="LightPreview" class="white_content">
   <dd:WebReportViewer ID="rptTonnageDeal" runat="server" Height="500px" Width="1000px" />
   <br />
   <div style="float:left; margin-top:auto">
          <asp:Button ID="btnClosePreview" runat="server" OnClientClick="javascript:HidePanelSplitPreview()"  CssClass="button" Text="Close" />
          </div>

   </div>
   <div id="FadePreview" class="black_overlaynew"></div>
   
   
 </asp:Content>
