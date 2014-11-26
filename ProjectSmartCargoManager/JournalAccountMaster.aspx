<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="JournalAccountMaster.aspx.cs" Inherits="ProjectSmartCargoManager.JournalAccountMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
function ViewPanelSplit() {
            document.getElementById('Lightsplit').style.display = 'block';
            document.getElementById('fadesplit').style.display = 'block';
        }
        function HidePanelSplit() {
            document.getElementById('Lightsplit').style.display = 'none';
            document.getElementById('fadesplit').style.display = 'none';
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
        </script>
        <style>
.black_overlaynew
		{
			display: none;
			position: absolute;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 1000px;
			background-color: black;
			z-index:1001;
			-moz-opacity:0.8;
			opacity:0.4;
			filter:alpha(opacity=80);
		}
	.white_contentnew 
		{
		    margin:0 auto;
			display: none;
			position:fixed;
			top: 15%;
			left: 30%;
			/*height: 70%;*/
			padding: 6px;
			border: 16px solid #ccdce3;
			background-color: white;
			z-index:1002;
		
            
		}
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
<asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
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
    
    
    
    <div id="contentarea">
   <h1> 
            Journal Accounting Entries
         </h1>
         
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
           
 
<asp:Panel ID="pnlNew" runat="server">
<div class="botline">
<table width="100%" border="0">
<tr>
<td>From Date</td>
<td><asp:TextBox ID="txtfrmdt" runat="server" Width="85px"></asp:TextBox>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="ImageButton1"
                                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtfrmdt">
                                        </asp:CalendarExtender></td>
<td>
   To Date 
</td>
<td>   
       <asp:TextBox ID="txttodt" runat="server" Width="85px"></asp:TextBox>
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="ImageButton2"
                                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txttodt">
                                        </asp:CalendarExtender>    
   </td>

      <td>AWBPrefix
             </td>
            <td>
                
                <asp:TextBox ID="txtAWBPrefix" runat="server" Width="45px" MaxLength="3"></asp:TextBox>&nbsp;
                                        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtAWBPrefix"
                                            WatermarkText="Prefix" />
                                            <asp:TextBox ID="txtAWBNo" runat="server" Width="80px" MaxLength="8"></asp:TextBox>                                        
                                        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="txtAWBNo"
                                            WatermarkText="AWB Number" />
                </td>
            
            <%--<td>
               AWBNumber
            </td>--%>
            
           
                <td>
         
        
        Flight
    </td>
    <td>
    <asp:TextBox ID="txtFlightCode" runat="server" Width="45px" MaxLength="2" Visible="true"></asp:TextBox>&nbsp;
                                        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtFlightCode"
                                            WatermarkText="Prefix"  />
                                        <asp:TextBox ID="txtFlightID" runat="server" Width="80px" MaxLength="4" Visible="true"></asp:TextBox>
                                        <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtFlightID"
                                            WatermarkText="Flight ID" />
    </td></tr><tr>
        <td>
        Flight Date
        </td>
        <td>
        <asp:TextBox ID="txtFltFromDt" runat="server" Width="85px" Visible="true"></asp:TextBox>
                                        <asp:ImageButton ID="imgFltFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" Visible="false" />
                                        <asp:CalendarExtender ID="TextBoxdate_CalendarExtender" runat="server" PopupButtonID="imgFltFromDt"
                                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFltFromDt">
                                        </asp:CalendarExtender>
        </td>
        <%--<td>
        Entity Code
        </td>
        <td>
       <asp:TextBox ID="txtEntityCode" runat="server" Width="100px"></asp:TextBox>
        </td>--%>
        <%--<td>
        DbAccountID
        </td>
        <td>
        <asp:DropDownList ID="ddlDbAccID" runat="server" ></asp:DropDownList>
        
        </td>--%>
        <td>
        SCMAcctField
        </td>
        <td>
        <asp:DropDownList ID="ddlSCMAcctField" runat="server"></asp:DropDownList>
        </td>
        <td>
        DbCrAccountType
        </td>
        <td>
        
        <asp:DropDownList ID="ddlAccountType" runat="server" >
        <asp:ListItem>Select</asp:ListItem>
        <asp:ListItem>Debit</asp:ListItem>
        <asp:ListItem>Credit</asp:ListItem>
        </asp:DropDownList>
        </td>
        
        <td>
        DbCrAccountID
        </td>
        <td>
        <asp:TextBox ID="txtDbCrAccountID" runat="server" Width="100px"></asp:TextBox>
        
        </td>
        </tr>
        <tr>
        <%--<td>
        CrSCMAcctField
        </td>
        <td>
        <asp:DropDownList ID="ddlCrSCMAcctField" runat="server" ></asp:DropDownList>
       
        </td>--%>
        <td>
        Entity Type
        </td>
        <td>
        <asp:DropDownList ID="ddlEntity" runat="server" ></asp:DropDownList>
        </td>
        <td>
        Entity ID
        </td>
        <td>
         <asp:TextBox ID="txtEntityID" runat="server" Width="100px"></asp:TextBox>
        </td>
        <td>
        Chart of Account ID
        
        </td>
        <td>
        <asp:DropDownList ID="ddlChartAccID" runat="server" ></asp:DropDownList>
        </td>
             <td colspan="2">
             <asp:Button ID="btnList" runat="server" CssClass="button" 
             Text="List" CausesValidation="false" OnClick="btnList_Click"  />
        
        <asp:Button ID="btnClear" runat="server" CssClass="button" 
             Text="Clear" CausesValidation="false" onclick="btnClear_Click"/>
             <asp:Button ID="btnExport" runat="server" CssClass="button" 
             Text="Export" CausesValidation="false" onclick="btnExport_Click"/>
             </td>
        </tr>
        </table>
 
   </div>
</asp:Panel>
<asp:Panel ID="pnlGrid"  runat="server">
<div class="ltfloat" style="width:100%; overflow:auto;">
    <asp:GridView ID="grdJournalAccount" runat="server" ShowFooter="false" Width="100%" 
 AutoGenerateColumns="False" CellPadding="2" 
 CellSpacing="3" PageSize="10" AllowPaging="True" 
 AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
        SelectedRowStyle-CssClass="SelectedRowStyle" 
        onpageindexchanging="grdJournalAccount_PageIndexChanging" OnRowCommand="grdJournalAccount_RowCommand" >
           
            <Columns>
            <asp:ButtonField CommandName="View" Text="View" Visible="true">
                                        <ItemStyle Width="50px" />
                                    </asp:ButtonField>
             <asp:TemplateField HeaderText="" Visible="false">
             <ItemTemplate>
             <asp:Label ID="lblSerialNumber" runat="server" Text = '<%# Eval("SerialNumber") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
            <%--<asp:TemplateField HeaderText="EntityID" Visible="false">
             <ItemTemplate>
             <asp:Label ID="lblEntityID" runat="server" Text = '<%# Eval("EntityID") %>'/>
             </ItemTemplate>
             </asp:TemplateField>--%>
             <%--<asp:TemplateField HeaderText="AWBPrefix" Visible="false">
             <ItemTemplate>
             <asp:LinkButton ID="lblAWBPrefix" runat="server" Text = '<%# Eval("AWBPrefix") %>' CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="View"/>
             </ItemTemplate>
             </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="AWBNumber" Visible="true">
             <ItemTemplate>
             <asp:LinkButton ID="lblAWBNumber" runat="server" Text = '<%# Eval("AWBNumber") %>' CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="AWBNumber"/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <%--<asp:TemplateField HeaderText="FltDate" Visible="true">
             <ItemTemplate>
             <asp:Label ID="lblFltDate" runat="server" Text = '<%# Eval("FltDate") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="FltNumber" Visible="true">
             <ItemTemplate>
             <asp:LinkButton ID="lblFltNumber" runat="server" Text = '<%# Eval("FltNumber") %>' CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"/>
             </ItemTemplate>
             </asp:TemplateField>--%>
             
             <%--<asp:TemplateField HeaderText="EntityCode" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:LinkButton ID="lblEntityCode" runat="server" Text = '<%# Eval("EntityCode") %>' CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"/>
             </ItemTemplate>
            </asp:TemplateField>--%>
            
            <%--<asp:TemplateField HeaderText="EntityType" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblEntityType" runat="server" Text = '<%# Eval("EntityType") %>'/>
             </ItemTemplate>
            </asp:TemplateField>--%>
            
            <%--<asp:TemplateField HeaderText="DbAccountID" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblDbAccountID" runat="server" Text = '<%# Eval("DbAccountID") %>'/>
             </ItemTemplate>
            </asp:TemplateField>--%>
            
            <asp:TemplateField HeaderText="DbSCMAcctField" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblDbSCMAcctField" runat="server" Text = '<%# Eval("DbSCMAcctField") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="DbSCMAcctValue" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblDbSCMAcctValue" runat="server" Text = '<%# Eval("DbSCMAcctValue") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Reference Entity Type" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblCrAccountID" runat="server" Text = '<%# Eval("RefEntityType") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Reference Entity ID" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblCrSCMAcctField" runat="server" Text = '<%# Eval("RefEntityID") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Db/Cr Acc Type" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblCrSCMAcctValue" runat="server" Text = '<%# Eval("DbCrAccountType") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Chart of Acc ID" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblChartAccountID" runat="server" Text = '<%# Eval("ChartofAccountID") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            <%--<asp:TemplateField HeaderText="BlSCMAccount" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblBlSCMAccount" runat="server" Text = '<%# Eval("BlSCMAccount") %>'/>
             </ItemTemplate>
            </asp:TemplateField>--%>
            
            <%--<asp:TemplateField HeaderText="BlAccountValue" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblBlAccountValue" runat="server" Text = '<%# Eval("BlAccountValue") %>'/>
             </ItemTemplate>
            </asp:TemplateField>--%>
             
             <asp:TemplateField HeaderText="Updated By" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblUpdatedBy" runat="server" Text = '<%# Eval("UpdatedBy") %>'/>
             </ItemTemplate>
             </asp:TemplateField>   
             
             <asp:TemplateField HeaderText="Updated On" ItemStyle-HorizontalAlign="Center" Visible="true" HeaderStyle-Width="150px">
             <ItemTemplate>
             <asp:Label ID="lblUpdatedOn" runat="server" Text = '<%# Eval("UpdatedOn") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <%--<asp:TemplateField HeaderText="Active" Visible="true" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
            <asp:Label ID="lblIsActive" runat="server" Text = '<%# Eval("IsActive") %>'/>
            </ItemTemplate>
            </asp:TemplateField>--%>
            
            </Columns>
            
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
                    <asp:Label ID="msgshow" runat="server"></asp:Label></td></tr></table></div>
     <div id="msgfade" class="black_overlaymsg">
    </div>
    
    <div id="Lightsplit" class="white_contentnew" style="width: 455px;">
         <div style="margin-bottom:15px; overflow: auto;  height:350px;">
             
             <fieldset>
                 <legend>Details</legend>
                 <div>
                         <div id="dbDIv" runat="server" class="ltfloat" style="border-right-style: solid; border-right-color: #000000; border-right-width: thin; width:40%; margin:20px; ">
                             <span style="font-size:21px; font-weight:bold;">Debit</span>
                     <asp:GridView ID="grdDebit" runat="server" BorderStyle="None" BorderColor="White" GridLines="None" AutoGenerateColumns="false">
                     <Columns>
                     <asp:TemplateField ControlStyle-BorderColor="White" ControlStyle-BorderStyle="None">
                     <ItemTemplate>
                     <asp:Label ID="lblgrdAccount" runat="server" Text = '<%# "<b>Account:</b>" + Eval("ChartofAccountID") %>'/>
                     <br />
                     <asp:Label ID="lblFor" runat="server" Text = '<%# "<b>For:</b>" + Eval("RefEntityType")+" - "+ Eval("RefEntityID") %>'/>
                     <br />
                     <asp:Label ID="lblSCMField" runat="server" Text = '<%# "<b>SCMField:</b>" + Eval("DbSCMAcctField") %>'/>
                     <br />
                     <asp:Label ID="lblValue" runat="server" Text = '<%# "<b>Value:</b>" + Eval("DbSCMAcctValue") %>'/>
                     <br />
                     <asp:Label ID="lblGLAccount" runat="server" Text = '<%# "<b>GLAccount:</b>" + Eval("GLAccountCode") +" - "+ Eval("GLAccountName") %>'/>
                     <br />
                     <asp:Label ID="lblCostCenter" runat="server" Text = '<%# "<b>CostCenter:</b>" + Eval("CostCenterID") +" - "+ Eval("CostCenterName") %>'/>
                     <br />
                    <hr style="border:1px dotted #ccc;" />
                     </ItemTemplate>
                     </asp:TemplateField>
                     </Columns>
                     
                     </asp:GridView>
                            </div>
                        
<div id="crDIv" runat="server" class="ltfloat" style="width:40%; margin:20px; ">
                             <span style="font-size:21px; font-weight:bold;">Credit</span>
                     <asp:GridView ID="grdCredit" runat="server" BorderStyle="None" BorderColor="White" GridLines="None" AutoGenerateColumns="false">
                     <Columns>
                     <asp:TemplateField ControlStyle-BorderColor="White" ControlStyle-BorderStyle="None">
                     <ItemTemplate>
                     
                     <asp:Label ID="lblgrdAccount" runat="server" Text = '<%# "<b>Account:</b>" + Eval("ChartofAccountID") %>'/>
                     <br />
                     <asp:Label ID="lblFor" runat="server" Text = '<%# "<b>For:</b>" + Eval("RefEntityType")+" - "+ Eval("RefEntityID") %>'/>
                     <br />
                     <asp:Label ID="lblSCMField" runat="server" Text = '<%# "<b>SCMField:</b>" + Eval("DbSCMAcctField") %>'/>
                     <br />
                     <asp:Label ID="lblValue" runat="server" Text = '<%# "<b>Value:</b>" + Eval("DbSCMAcctValue") %>'/>
                      <br />
                     <asp:Label ID="lblGLAccount" runat="server" Text = '<%# "<b>GLAccount:</b>" + Eval("GLAccountCode") +" - "+ Eval("GLAccountName") %>'/>
                     <br />
                     <asp:Label ID="lblCostCenter" runat="server" Text = '<%# "<b>CostCenter:</b>" + Eval("CostCenterID") +" - "+ Eval("CostCenterName") %>'/>
                     <br />
                    <hr style="border:1px dotted #ccc;"  />
                     </ItemTemplate>
                     </asp:TemplateField>
                     </Columns>
                     
                     </asp:GridView>
                     </div>
 
                        
                    </div>
             </fieldset>
            
             
         </div>
         <asp:Button ID="btnExportPopup" runat="server" CssClass="button" Text="Export" OnClick="btnExportPopup_Click" />
         <asp:Button ID="btnOk" runat="server" CssClass="button" Text="Close" />
     </div>
     
     <div id="fadesplit" class="black_overlaynew"></div>
</asp:Content>
