<%@ Page Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="FrmTracerGeneration.aspx.cs" Inherits="MyKFCargoNewProj.FrmTracerGeneration" Title="Tracer" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
       
    .hiddencol1
    {
        display:none;
    }
    .viscol1
    {
        display:block;
    }
    
    
.modalPopup {
	background-color:#ffffdd;
	border-width:3px;
	border-style:solid;
	border-color:Gray;
	padding:3px;
	width:250px;
	font-family:Calibri;
	font-size:15px;
	font-weight:bold;
}


        .style9
        {
            width: 135px;
        }
        .style10
        {
            width: 401px;
        }
        

        .style14
        {
            height: 25px;
        }
        .style15
        {
            width: 401px;
            height: 25px;
        }
        .style16
        {
            width: 135px;
            height: 22px;
        }
        .style17
        {
            width: 401px;
            height: 22px;
        }
        

        .style18
        {
            height: 22px;
        }
        

        #MyFile
        {
            width: 220px;
        }
        

        .style19
        {
            font-family: Verdana;
            font-size: 14px;
            font-weight: bold;
            height: 19px;
        }
        .style20
        {
            height: 19px;
        }
        

        </style>
   
<script language="JavaScript" type="text/javascript">

    function OpenTracerHistoryPopup(txtAwbNo, txtTracer) 
    {

        window.open("FrmTracerHistoryPopup.aspx?TracerNo=" + txtTracer + "&AWBNo=" + txtAwbNo,
        "List", "scrollbars=no,resizable=no,width=500,height=250");
        return false;
    }

    function OpenAWBTrackingPopup(txtAwbNo) {

//        window.open("frmGPSTracking.aspx?AWBNo=" + txtAwbNo,
//        "List", "scrollbars=no,resizable=no,width=800,height=800");
//        return false;
    }

    function OpenCargoManifestPopup(txtFlightNo, FDate, TDate) {

        window.open("FrmCargoManifestPopup.aspx?FlightNo=" + txtFlightNo + "&FDate=" + FDate + "&TDate=" + TDate,
        "List", "scrollbars=no,resizable=no,width=800,height=800");
        return false;
    }
    
    function OpenTracerFoundADD(txtAwbNo, txtTracer,
    MsdPcs, FndPcs, FndType, FndLoc,btn,UID) {

        window.open("FrmTracerAddFoundAwbPopup.aspx?TracerNo=" + txtTracer + "&AWBNo=" + txtAwbNo +
        "&MsdPcs=" + MsdPcs + "&FndPcs=" + FndPcs +
        "&FndType=" + FndType + "&FndLoc=" + FndLoc + "&btnVis=" + btn + "&UID=" + UID,
        "List", "scrollbars=no,resizable=no,width=900,height=600,top=10px, left=200px");
        return false;
    }
   
// open new window to Generate Tracer
    function OpenCreateTracer(pnlname, UID, TracerNo) 
    {

        var pnlname;
//       alert(TracerNo);

        window.open("FrmNewTracerPopup.aspx?pnlName=" + pnlname + "&UID=" + UID +"&TracerNo=" + TracerNo, "List", "scrollbars=no,resizable=no,width=950,height=560");
        return false;
    }

    function AdvSearchButtonClick() {
        document.getElementById(btn).click();
    }
    
    function CallButtonClick(btn) {
        document.getElementById(btn).click();
    }

    function myprint() 
    {
        window.print();
    }

    function MyFile_onclick() 
    {

    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
            <asp:ToolkitScriptManager runat="server">
            </asp:ToolkitScriptManager>
    <div id="contentarea"></div>
    
    <asp:UpdatePanel ID="updHead" runat="server">
       <ContentTemplate>
        <div class="msg">
        <asp:Label ID="lblError" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>
    
    <h1>
        <%--<img src="Images/txt_tracer.png" />--%> Tracer Generation
    </h1>
                    
            
            <div style="display: none;">
            <asp:ImageButton ID="btnRefresh" runat="server" onclick="BtnSearch_Click" 
                Height="5px" ImageUrl="~/images/NoImage.png" />
            </div>
            <input id="HdnCTime" type="hidden" runat="server" />
        
   
    <asp:UpdatePanel ID="updMainPnl" runat="server" UpdateMode="Conditional">
    
    <ContentTemplate>
    
            
                
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtAwbno" ErrorMessage="AWBNO is Mandatory Field." 
                    SetFocusOnError="True" ValidationGroup="chkVal"></asp:RequiredFieldValidator>
                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="txtAWBPrefix" ErrorMessage="AWBPrefix is Mandatory Field." 
                    SetFocusOnError="True" ValidationGroup="chkVal"></asp:RequiredFieldValidator>
                    
                    <asp:Timer ID="Timer1" runat="server" Enabled="False" Interval="10" 
                    ontick="Timer1_Tick">
                    
                </asp:Timer><input id="HdnPanel" type="hidden" runat="server"/>
                
         
    <asp:Panel ID="pnlfilter" runat="server">
    <asp:Panel ID="Panel3" runat="server"  Visible="false">
        <div class="botline" visible="false">
    <table style="width:80%;">
        <tr class="tr1">
            
            <td>
               
                    
                <asp:Button ID="BtnSearch" runat="server" Text="Search" 
                    onclick="BtnSearch_Click" CssClass="button" Visible="false"/>     &nbsp;
			    
                <asp:Button ID="BtnExcel1" runat="server" Text="Export"
                    onclick="BtnExcel1_Click" CssClass="button" Visible="false"/>  &nbsp;
                
                <input id="HdnIsPageLoad" type="hidden" runat="server"/>
            <%--</td>
            <td>--%>
                <asp:LinkButton ID="btnlnkAdv" runat="server" onclick="btnlnkAdv_Click" CssClass="button" Visible="false">Advanced Search</asp:LinkButton> &nbsp;
               <asp:LinkButton ID="btnTracer" runat="server" CssClass="button" Visible="false"
                    
                    ToolTip="CLICK HERE TO  ADD NEW AWB DETAIL NOT PRESET IN TRACER LIST.NEW AWB DETAIL NOT PRESET IN TRACER LIST.">Create New Tracer</asp:LinkButton>
      
                
            </td>
            
                 </tr>
        
        
    </table></div>
    </asp:Panel>
    <%--<asp:RoundedCornersExtender ID="Panel3_RoundedCornersExtender" runat="server" 
        Enabled="True" TargetControlID="Panel3" Radius="6" Corners="All" BorderColor="DarkGray">
    </asp:RoundedCornersExtender>--%>
    <div class="botline">
    <asp:Panel ID="Panel1" runat="server"  >
        <%--<div class="divback">--%>
        
        <span style="float:right;">
        
        <asp:ImageButton ID="btnCloseAdv" runat="server" Visible="false" 
                    ImageUrl="~/images/Cross.jpg"  
                    onclick="btnCloseAdv_Click" ToolTip="Click to Close Advanced Search."/>
                    
         </span>
         <%--<h3>
             <img src="Images/txt_advancesearch.png" visible="false"/>
       </h3>--%>
    <table width="100%">
    <tr>
        <td>AWBNo:</td>
            <td align="left" valign="top">                
             <asp:TextBox ID="txtAWBPrefix" runat="server" MaxLength="3" Width="40px"></asp:TextBox>
             <asp:TextBox ID="txtAwbno" runat="server" MaxLength="8" Width="100px"></asp:TextBox>                   
            </td>
            
            <td align="left" valign="top">From Date:</td>
			    <td align="left" valign="top">
                <asp:TextBox ID="txtFromDt" runat="server"  CssClass="ctrltxt" Width="100px"></asp:TextBox>
			    <asp:CalendarExtender ID="txtFromDt_CalendarExtender" runat="server" 
			    Enabled="True" TargetControlID="txtFromDt" Format="dd/MM/yyyy" PopupButtonID="imgfromdate">
                </asp:CalendarExtender>
                <asp:ImageButton ID="imgfromdate" runat="server" ImageAlign="AbsMiddle" 
                        ImageUrl="~/Images/calendar_2.png" />
			    </td>
			    
			    <td align="left" valign="top" >To Date:</td>
			    <td align="left" valign="top" >
                <asp:TextBox ID="txtToDt" runat="server"  CssClass="ctrltxt" Width="100px"></asp:TextBox>
                <asp:CalendarExtender ID="txtToDt_CalendarExtender" runat="server" Enabled="True" 
                TargetControlID="txtToDt" Format="dd/MM/yyyy" PopupButtonID="imgToDate">
                </asp:CalendarExtender>
                <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" 
                        ImageUrl="~/Images/calendar_2.png" />
            </td>
    </tr>
		<%--<tr>
			
			
            <td align="left" valign="top">
                &nbsp;</td>
            
            
            <td align="left" valign="top">
                &nbsp;</td>
        </tr>--%>
        
        <%--<tr>
            <td ><strong>Filter By</strong></td>
            <td >
                &nbsp;</td>
            <td>&nbsp;</span></td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td colspan="2">
               <strong>Search By Status</strong> </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>--%>
        <tr>
            <td>
                Origin:<%--<span class="MandatoryField">*</span>--%>
            </td>
            <td >
                <asp:Panel ID="PnlSrc" runat="server"  ScrollBars="Auto">
                    <asp:UpdatePanel ID="UpdPnlChkSrc" runat="server">
                        <ContentTemplate>
                            <%--<asp:CheckBoxList ID="ChkSrcList" runat="server" BackColor="#FFFFDD"  EnableViewState="true"
                        CellPadding="1" CellSpacing="1" RepeatColumns="4" RepeatDirection="Horizontal" 
                        TextAlign="Left" AutoPostBack="True" 
                        onselectedindexchanged="ChkSrcList_SelectedIndexChanged" 
                        ToolTip="CHECK TO VIEW MULTIPLE SOURCE AIRPORT TRACER" CssClass="tr1" Visible="false">
                    </asp:CheckBoxList>--%>
                            <asp:DropDownList ID="DdlOriginList" runat="server" AppendDataBoundItems="True" 
                                onselectedindexchanged="DdlOriginList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
            <td >
                Destination:<%--<span class="MandatoryField">*</span>--%>
            </td>
            <td>
                <asp:Panel ID="PnlDest" runat="server" ScrollBars="Auto">
                    <asp:UpdatePanel ID="UpdPnlChkDest" runat="server">
                        <ContentTemplate>
                            <%--<asp:CheckBoxList ID="ChkDestList" runat="server" BackColor="#FFFFDD"  EnableViewState="true"
                        CellPadding="1" CellSpacing="2" RepeatColumns="4" RepeatDirection="Horizontal" 
                        TextAlign="Left" 
                        ToolTip="CHECK TO VIEW MULTIPLE DESTINATION AIRPORT TRACER" 
                        AutoPostBack="True" onselectedindexchanged="ChkDestList_SelectedIndexChanged" 
                              CssClass="tr1" Visible="false">
                    </asp:CheckBoxList>--%>
                            <asp:DropDownList ID="DdlDestinationList" runat="server" 
                                AppendDataBoundItems="True" 
                                onselectedindexchanged="DdlDestinationList_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
            <%--<td>
                </td>--%>
            <td>
                Tracer Status</td>
            <td>
                <asp:DropDownList ID="ddlTracerStatus" runat="server" CssClass="ctrlDDl" 
                    ToolTip="SELECT THE TRACER STATUS CRITERIA IF ANY">
                    <asp:ListItem Selected="True">All</asp:ListItem>
                    <asp:ListItem value="Add">Add</asp:ListItem>
                    <asp:ListItem Value="Open">Open</asp:ListItem>
                    <asp:ListItem Value="TNG">Close</asp:ListItem>
                </asp:DropDownList></td>
            <td>
                Delivery Status</td>
            <td>
                <asp:DropDownList ID="ddlAWBStatus" runat="server" CssClass="ctrlDDl" 
                    ToolTip="SELECT THE DELIVERY STATUS CRITERIA IF ANY" Width="120px">
                    <%--<asp:ListItem Value="Undelivered">Undelivered</asp:ListItem>--%>
                    <asp:ListItem Selected="True">All</asp:ListItem>
                    <asp:ListItem Value="Undelivered">Undelivered</asp:ListItem>
                    <asp:ListItem Value="Partially">Partially Delivered</asp:ListItem>
                    <asp:ListItem Value="Delivered">Delivered</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        
        <tr>
			<td valign="top" style="text-align: left" colspan="9">
                <asp:RadioButtonList ID="RdbOtherFilters" runat="server" 
                    RepeatDirection="Horizontal" AutoPostBack="True" 
                    onselectedindexchanged="RdbOtherFilters_SelectedIndexChanged" 
                    ToolTip="CLICK HERE TO VIEW BY TRACER NO OR AWBNO OR FLIGHT NO" Visible="false">
                    <asp:ListItem Selected="True">None</asp:ListItem>
                    <asp:ListItem>Tracer#</asp:ListItem>
                    <asp:ListItem>Flight#</asp:ListItem>
                </asp:RadioButtonList>
              <%--</td>
              <td>--%>  
                <asp:TextBox ID="txtOtherFilter" runat="server" AutoPostBack="True" Width="100px"
                    CssClass="ctrltxt" MaxLength="8" 
                    ontextchanged="txtOtherFilter_TextChanged" 
                    ToolTip="ENTER TRACERNO IF FILTER BY TRACERNO" ValidationGroup="CheckVal" 
                    Visible="False"></asp:TextBox>
                    
                <asp:TextBox ID="txtFltNo" runat="server" Width="100px"
                    CssClass="ctrltxt" MaxLength="6" ontextchanged="txtFltNo_TextChanged" 
                    ToolTip="ENTER FLIGHT NO IF FILTER BY FLIGHT NO" Visible="False"></asp:TextBox>
                    
                <asp:AutoCompleteExtender ID="txtFltNo_AutoCompleteExtender" runat="server" 
                    CompletionInterval="10" CompletionSetCount="10" DelimiterCharacters="" 
                    Enabled="True" MinimumPrefixLength="3" ServiceMethod="GetFlightList" 
                    ServicePath="" TargetControlID="txtFltNo" UseContextKey="True">
                </asp:AutoCompleteExtender>
                <input id="hdnAwbNoVal" runat="server" type="hidden" />
            </td>
            
            
        </tr>
       <tr>
            <td align="left" valign="top" colspan="4" style="text-align: center" >
                
			</td>
            <td align="left" style="text-align: center" valign="top">
                &nbsp;</td>
            <td align="left" style="text-align: center" valign="top">
                &nbsp;</td>
            <td align="left" style="text-align: center" valign="top">
                &nbsp;</td>
            <td align="left" style="text-align: center" valign="top">
                &nbsp;
            </td>
            <td align="left" style="text-align: center" valign="top">
                
                     
                <%--<asp:Button ID="BtnClear" runat="server" Text="Close"  CssClass="button" 
                    onclick="BtnClear_Click"/>--%>
           </td>
       </tr>
		
	</table>
	
	 <asp:Button ID="BtnSearch2" runat="server" Text="List" onclick="BtnSearch_Click" CssClass="button" /> &nbsp;
	 
	 <asp:Button ID="BtnClear" runat="server" Text="Clear" CssClass="button" onclick="BtnClear_Click" />&nbsp;
			    
     <asp:Button ID="BtnExcel2" runat="server" Text="Excel" onclick="BtnExcel1_Click" CssClass="button"  /> &nbsp;
                    
    <asp:LinkButton ID="btnNewTracer" runat="server" ToolTip="CLICK HERE TO  ADD NEW AWB DETAIL NOT PRESET IN TRACER LIST.NEW AWB DETAIL NOT PRESET IN TRACER LIST." CssClass="button" >Create New Tracer</asp:LinkButton> &nbsp;
    
    
              
	</div>
	
	
    </asp:Panel>
    
	<%--<asp:RoundedCornersExtender ID="Panel1_RoundedCornersExtender" runat="server" 
        Enabled="True" TargetControlID="Panel1" Radius="6" Corners="All" BorderColor="DarkGray">
    </asp:RoundedCornersExtender>--%></asp:Panel>
    
  
  <asp:Timer ID="Timer2" runat="server" Enabled="False" Interval="1000" 
                    ontick="Timer2_Tick">
                </asp:Timer>
 
  <div style="text-align: center">
  
        <%--<asp:Label ID="lblLoadStat" runat="server" CssClass="tr1" 
            Text="Loading Data Please Wait..."></asp:Label>
        <asp:Image ID="ImgLoading" runat="server" ImageUrl="images/loader_spinner.gif" />--%>
  
  </div>
  
  <br />

<div id="divgen" runat="server" visible="true" style="float:left;">
                    
                    <asp:GridView ID="grdViewTracer" runat="server" AllowPaging="True" 
                        AllowSorting="True"    AutoGenerateColumns="false"
                         DataKeyNames="TracerNo" EmptyDataText="NO RECORDS FOUND" 
                        EnableViewState="true" HorizontalAlign="Center" 
                        onpageindexchanging="grdViewTracer_PageIndexChanging" 
                        onrowcommand="grdViewTracer_RowCommand" onrowcreated="grdViewTracer_RowCreated" 
                        onrowdatabound="grdViewTracer_RowDataBound" onsorting="grdViewTracer_Sorting" 
                        PageSize="10" ShowFooter="True" CellPadding="3" CellSpacing="3">
                        <Columns>
                            <asp:TemplateField HeaderText="Tracer No">
                               
                                <EditItemTemplate>
                                    <asp:Label ID="lblTracerNo" runat="server" Text='<%# Bind("TracerNo") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTracerNoV" runat="server" Text='<%# Bind("TracerNo") %>' 
                                        Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lnkTracerNo" runat="server" ForeColor="#0000CC" 
                                        Text='<%# Bind("TracerNo") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle/>
                            </asp:TemplateField>
                            <asp:TemplateField  HeaderText="AWB Date" 
                                >
                                <ItemTemplate>
                                    <asp:Label ID="lblAwbDate" runat="server" Text='<%# Bind("AWBDate") %>'> </asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AWB No" ItemStyle-Width="100px">
                                <EditItemTemplate>
                                    <asp:Label ID="lblAWBNoEdit" runat="server" Text='<%# Bind("AWBNo") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAWBNo" runat="server" 
                                        Text='<%# Bind("AWBNo") %>' Enabled = "false"></asp:LinkButton>
                                    <asp:Label ID="lblAWBNo" runat="server" Text='<%# Bind("AWBNo") %>' 
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Origin">
                                <EditItemTemplate>
                                    <asp:Label ID="TextBox5" runat="server" Text='<%# Bind("Origin") %>'></asp:Label>
                                
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrg" runat="server" Text='<%# Bind("Origin") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dest    " 
                                >
                                <EditItemTemplate>
                                    <asp:Label ID="TextBox6" runat="server" Text='<%# Bind("Dest") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDest" runat="server" Text='<%# Bind("Dest") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Flight No " 
                                >
                                <EditItemTemplate>
                                    <asp:Label ID="TextBox7" runat="server" Text='<%# Bind("FltNo") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkFlightNo" runat="server" 
                                        Text='<%# Bind("FltNo") %>' Enabled = "false"></asp:LinkButton>
                                    <asp:Label ID="lblFlightNo" runat="server" Text='<%# Bind("FltNo") %>' 
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField  HeaderText="Contents     ">
                                <ItemTemplate>
                                    <asp:Label ID="Label11" runat="server" Text='<%# Bind("ContentType") %>'> </asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pcs    ">
                                <ItemTemplate>
                                    <asp:Label ID="lblSentPcs" runat="server" Text='<%# Bind("SentPcs") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Wt      ">
                                <EditItemTemplate>
                                    <asp:Label ID="TextBox4" runat="server" Text='<%# Bind("ChargebleWgt") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblChargeableWgt" runat="server" 
                                        Text='<%# Bind("ChargebleWgt") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Short Pcs  ">
                                <ItemTemplate>
                                    <asp:Label ID="lblMissedPcs" runat="server" Text='<%# Bind("MissedPcs") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RFID Status" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="Label12" runat="server" Text='<%# Bind("LastRecdStatus") %>'> </asp:Label>
                                </ItemTemplate>
                                <HeaderStyle/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Recd Pcs    ">
                                <EditItemTemplate>
                                    <asp:Label ID="lblRecdPcsEdit" runat="server" Text='<%# Bind("RecdPcs") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRecdPcs" runat="server" Text='<%# Bind("RecdPcs") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField  HeaderText="Generate Tracer">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnCaller" runat="server" CausesValidation="false" 
                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                                        CommandName="GenTracer"  ImageUrl="~/Images/finish.png" />
                                         <%--OnClientClick="javascript:OpenCreateTracer(pnlname, UID,HiddenValues);return false;"--%>
                                </ItemTemplate>
                               
                                <EditItemTemplate>
                                    <asp:Label ID="lblGenTracer" runat="server" Text='<%# Bind("IsGenTracer") %>'></asp:Label>
                                </EditItemTemplate>
                               
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField  HeaderText="Found Pcs  ">
                                <ItemTemplate>
                                    <asp:Label ID="lblFoundPcs" runat="server" Text='<%# Bind("FoundPcs") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField  HeaderText="Lost Pcs  ">
                                <ItemTemplate>
                                    <asp:Label ID="lblLostPcs" runat="server" Text='<%# Bind("LostPcs") %>'></asp:Label>
                                    <%--<asp:Label ID="lblLostPcs" runat="server" Text='<%# Bind("MissedPcs") %>'></asp:Label>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <%--<asp:TemplateField HeaderText="">
                               <ItemTemplate>
                               
                               </ItemTemplate>
                            </asp:TemplateField>--%>
                            
                            <asp:TemplateField 
                                HeaderText="Last Found/Lost Location">
                                <ItemTemplate>
                                    <asp:Label ID="lblFoundLoc" runat="server" Text='<%# Bind("FoundAtStcCode") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GenTracer">
                                <ItemTemplate>
                                    <asp:Label ID="lblIsGenTr" runat="server" Text='<%# Bind("IsGenTracer") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Close Tracer">
                                <ItemTemplate>
                                    <asp:Label ID="lblIsClosedTr" runat="server" Text='<%# Bind("IsClosed") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEditGrd" runat="server" 
                                        ImageUrl="images/Document-Write-icon1.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Close Tracer" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnCloseGrd" runat="server" CausesValidation="false" 
                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                                        CommandName="CloseTracer" 
                                        ImageUrl="~/images/1300711927_window-close.png" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <%--<RowStyle BackColor="White" Font-Names="Verdana" Font-Size="Small" 
                            ForeColor="Blue" HorizontalAlign="Center" />
                        <FooterStyle BackColor="#990000" Font-Names="Verdana" Font-Size="Small" 
                            ForeColor="White" HorizontalAlign="Center" />
                        <PagerStyle BackColor="#990000" Font-Names="Verdana" Font-Size="Small" 
                            ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="White" Font-Names="Verdana" Font-Size="Small" 
                            ForeColor="#990000" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#990000" Font-Bold="True" Font-Names="Verdana" 
                            Font-Size="Small" ForeColor="White" HorizontalAlign="Center" />
                        <AlternatingRowStyle BackColor="White" Font-Names="Verdana" Font-Size="Small" 
                            ForeColor="Blue" HorizontalAlign="Center" />--%>
                            
                            <HeaderStyle CssClass="titlecolr" />
                        <RowStyle HorizontalAlign="Center" />
                        <AlternatingRowStyle HorizontalAlign="Center" />
                    </asp:GridView>
                    
                    <asp:Panel ID="gneratetrace" runat ="server" Visible="false">
                    <div>
                    <h2>Generate Tracer</h2>
                            <input id="hdnFltVal" type="hidden" runat="server"/>
                    <table width="80%"><tr>
                    <td style="width:70%;">
                    <table id="t1" width="100%"  visible="false">
                        <tr>
                        <td colspan="2" style="text-align: center">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 <asp:Label CssClass="ctrltxt"
                                    ID="lblTrFlightID" runat="server" Text="Flight No"> </asp:Label>
                            </td>
                            <td  >
                                <asp:TextBox ID="txtFlightNo" runat="server" AutoPostBack="True" 
                                    ontextchanged="txtFlightNo_TextChanged" CssClass="ctrltxt"></asp:TextBox><%-- <asp:DropDownList ID="ddlFlightID" runat="server" Width="100px">
                                </asp:DropDownList>--%>
                            </td>
                        </tr>
                        <tr>
                           <td>
                           <asp:Label ID="lblDate" runat="server" CssClass="ctrltxt" Text="Flight(AWB) Date"></asp:Label>
                           </td>
                           <td >
                           <asp:TextBox ID="txtDate" runat="server" CssClass="ctrltxt" Enabled="False"> </asp:TextBox>
                           </td>
                        </tr>
                        <tr>
                                <td >
                                    <asp:Label ID="lblSource" runat="server" CssClass="ctrltxt" Text="Orgin/Dest" ></asp:Label>
                                </td>
                                <td>
                                <asp:TextBox ID="txtOrg" runat="server" CssClass="tr1" Width="71px" Enabled="False"></asp:TextBox>
                                <asp:TextBox ID="txtDest" runat="server" CssClass="tr1" Width="71px" 
                                        Enabled="False"></asp:TextBox><%--<asp:DropDownList ID="ddlSource" runat="server" style="margin-left: 0px" 
                                 Width="100px" AutoPostBack="True">
                                <asp:ListItem>BOM</asp:ListItem>
                                </asp:DropDownList>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="style18">
                                 <asp:Label ID="lblTrAWBno" runat="server" CssClass="ctrltxt" 
                                     Text="Airwaybill No" ></asp:Label>
                            </td>
                            <td class="style18" >
                                <asp:TextBox ID="txtAwbNoTr" runat="server" MaxLength="10" CssClass="ctrltxt" Enabled="False"></asp:TextBox>
                                </td>
                                </tr>
                                <tr>
                                <td>
                                
                                <asp:Label ID="lblTotalPcs" runat="server"
                                Text="Total PCS/Wgt" CssClass="ctrltxt"></asp:Label>
                                </td>
                                <td>
                                <asp:TextBox ID="txtTotalPcs" runat="server" MaxLength="10" Width="71px" 
                                        Enabled="False" CssClass="tr1" Height="22px"></asp:TextBox>
                                    <asp:TextBox ID="txtWgt" runat="server" Width="71px" Enabled="False" 
                                        CssClass="tr1" Height="22px"></asp:TextBox>
                                </td>
                                </tr>
                        
                        
                        <tr>
                            <td class="style14">
                                 <asp:Label ID="lblShortage" runat="server" CssClass="tr1" Text="Shoratage"></asp:Label>
                            </td>
                            <td class="style15" >
                                <asp:TextBox ID="txtShortage" runat="server" MaxLength="5" CssClass="ctrltxt"
                                    ValidationGroup="ValChkGenTr"></asp:TextBox>
                                
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                    ControlToValidate="txtShortage" ErrorMessage="0 Shortage Not Allowed" 
                                    ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,5}" 
                                    ValidationGroup="ValChkGenTr" CssClass="errMsgOn"></asp:RegularExpressionValidator>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="txtShortage" ErrorMessage="Mandatory" 
                                    ValidationGroup="ValChkGenTr" CssClass="errMsgOn"></asp:RequiredFieldValidator>
                                <input id="hdnSentPcs" type="hidden" runat="server"/>
                                </td>
                                </tr>
                                
                        <tr>
                            <td class="style9" >
                                <asp:Label ID="lblPkng" runat="server"  Text="Packaging" 
                                 EnableTheming="False" CssClass="ctrltxt"></asp:Label></td>
                            <td class="style10" >
                                <asp:TextBox ID="txtPkng" runat="server" Text="Carton Box"  CssClass="ctrltxt"></asp:TextBox></td></tr><tr>
                            <td class="style16">
                                <asp:Label ID="lblCnts" runat="server" Text="Contents" CssClass="ctrltxt" ></asp:Label>
                            </td>
                            <td class="style17">
                                <asp:TextBox ID="txtContents" runat="server" CssClass="ctrltxt"></asp:TextBox></td></tr><tr>
                            <td class="style9">
                                 <asp:Label 
                                    ID="lblOrgAgent" runat="server"
                                    Text="Consignor" CssClass="ctrltxt"></asp:Label></td><td class="style10" >
                                <asp:TextBox ID="txtOrgAgent" runat="server" CssClass="ctrltxt"></asp:TextBox></td></tr><tr>
                            <td class="style9" >
                                 <asp:Label 
                                    ID="lblDestAgent" runat="server" Text="Consignee" CssClass="ctrltxt"></asp:Label></td>
                            <td class="style10">
                                <asp:TextBox ID="txtDestAgent" runat="server" CssClass="ctrltxt"></asp:TextBox>
                           </td>
                        </tr>
                        <tr>
                            <td class="tr1">
                                Remarks
                            </td>
                            <td class="style10">
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="ctrltxt"></asp:TextBox>
                            </td>
                            
                        </tr>
                        
                        <tr>
                            <td class="tr1">
                                 Attachements: 
                            </td>
                            <td>    
                             <input id="MyFile" type="File" runat="Server" 
                                     title="CLICK TO ATTACHED FILE IF ANY" onclick="return MyFile_onclick()"  />
                                <asp:Button ID="btnUpd" runat="server" Text="Upload" onclick="btnUpd_Click" 
                                 ToolTip="CLICK HERE TO UPLOAD ATTACHED FILE" Height="21px" Width="80px" 
                                 OnClientClick="javascript:document.forms[0].encoding = 'multipart/form-data';" CssClass="button"/>
                                 
                                 (Max 4Mb)</td>
                        </tr>
                        <tr>
                        <td></td>
                            <td>
                                
                                <asp:Button ID="btnSubmit" runat="server" Text="Generate Tracer" onclick="btnSubmit_Click" CssClass="button"
                                    ToolTip="CLICK HERE TO GENERATE TRACER AND SEND EMAIL " ValidationGroup="ValChkGenTr" 
                                OnClientClick="javascript:document.forms[0].encoding = 'multipart/form-data';"/> <%--onclick="btnSubmit_Click"--%>
                                
                                
                                <asp:Button ID="btnBack" runat="server" Height="26px" Text="Back" Width="61px" 
                                    onclick="btnBack_Click" ToolTip="CLICK HERE TO GO BACK TO MAIN PAGE" CssClass="button" />
                                    
                                    
                                <input id="hdnWeight" type="hidden" runat="server"/>
                                <input id="hdnEdit" type="hidden" runat="server" />
                                <input id="hdnTracerNo" type="hidden"  runat="server"/>
                            </td>
                        </tr>
                    </table>
                    </td>
                    
                    
                    
                    <td valign="top">
                    <h3>
                    <asp:Label ID="lblUpdFile" runat="server" Text="Uploaded Files" Visible="False"></asp:Label></h3>
                    <asp:UpdatePanel ID="UpdPnlgrdCurrArchived" runat="server" UpdateMode="Conditional"> 
                             <ContentTemplate>
                                <asp:GridView ID="grdCurrArchived" runat="server" AutoGenerateColumns="False"  EnableViewState="true"
                                    onrowcommand="grdCurrArchived_RowCommand" 
                                    onrowdeleting="grdCurrArchived_RowDeleting" ShowFooter="True" 
                                     onrowdatabound="grdCurrArchived_RowDataBound" Width="300px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="File Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("FileName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" 
                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                                                    CommandName="Delete" ImageUrl="~/images/cross.jpg" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                               
                                <%--<FooterStyle BackColor="#990000" ForeColor="White"  HorizontalAlign="Center" 
                                    Font-Names="Verdana" Font-Size="Small" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" 
                                    Font-Names="Verdana" Font-Size="Small" />--%>
                                <SelectedRowStyle BackColor="White" ForeColor="#cccccc" 
                                    HorizontalAlign="Center" Font-Names="Verdana" Font-Size="Small" />
                               
                                    
                                    <HeaderStyle CssClass="titlecolr" />
                        <RowStyle HorizontalAlign="Center" />
                        <AlternatingRowStyle HorizontalAlign="Center" />
                            </asp:GridView>
                              </ContentTemplate>
                            </asp:UpdatePanel></td></tr></table>
                    </div>
                  </asp:Panel>
                <%--</asp:Panel>--%>
    </div>
  
    </ContentTemplate>
    
    <Triggers>
    <%--<asp:AsyncPostBackTrigger ControlID="btnAdv1" EventName="CheckedChanged" />--%>
    <asp:AsyncPostBackTrigger ControlID="RdbOtherFilters" EventName="SelectedIndexChanged" />
    <%--<asp:PostBackTrigger ControlID="lblAdvSearch" />--%>
    <asp:PostBackTrigger ControlID="btnUpd" />
    <asp:PostBackTrigger ControlID="btnlnkAdv" />
    <asp:PostBackTrigger ControlID="BtnSearch" />
    <asp:PostBackTrigger ControlID="BtnSearch2" />
    <asp:PostBackTrigger ControlID="BtnExcel1" />
    <asp:PostBackTrigger  ControlID="btnSubmit"  />
    <asp:PostBackTrigger ControlID="BtnExcel2" />
    <asp:PostBackTrigger ControlID="btnCloseAdv" />
    <%--<asp:AsyncPostBackTrigger ControlID="btnCloseAdv" EventName="Click" />--%>
    <asp:PostBackTrigger ControlID="grdViewTracer" />
    <asp:PostBackTrigger ControlID="btnRefresh" />
    <asp:PostBackTrigger ControlID="btnTracer" />
    <asp:PostBackTrigger ControlID="btnNewTracer" />
    
    </Triggers>

    </asp:UpdatePanel>
   
	<div >
	

   </div>  
                
</asp:Content>
