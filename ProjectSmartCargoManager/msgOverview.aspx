<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="msgOverview.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master"  Inherits="ProjectSmartCargoManager.msgOverview1" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
			height: 70%;
			padding: 16px;
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
		
         .style3
         {
             width: 80px;
         }
		
         .style4
         {
             width: 30px;
         }
		
         .style5
         {
             width: 31px;
         }
         .style6
         {
             width: 8px;
         }
         .style7
         {
             width: 33px;
         }
		
         .style8
         {
             width: 131px;
         }
         .style9
         {
             width: 86px;
         }
		
         </style>
<script type ="text/javascript">
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


    function ViewPanelSplit() {
        document.getElementById('Lightsplit').style.display = 'block';
        document.getElementById('fadesplit').style.display = 'block';
    }
    function HidePanelSplit() {
        document.getElementById('Lightsplit').style.display = 'none';
        document.getElementById('fadesplit').style.display = 'none';
    }


</script>
<script type="text/javascript" language="javascript">


    function Validation() {

        var ddl = document.getElementById("<%= ddlMsgType.ClientID %>");

        var selection = document.getElementById("<%= ddlMsgType.ClientID %>").value;
        //document.getElementById("<%= selectionhd.ClientID %>").value = selection;
        //selection = document.getElementById("<%= selectionhd.ClientID %>").value;
        //document.getElementById("<%= selectionhd.ClientID %>").value = document.getElementById("<%= ddlMsgType.ClientID %>").value;
        //        if (document.getElementById("<%= selectionhd.ClientID %>").value != null) {
        //            selection = document.getElementById("<%= selectionhd.ClientID %>").value;
        //        }

        if (selection == "FFR" || selection == "FFA" || selection == "FWB") {
            document.getElementById("<%= lblawbnum.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= txtAWBNumber.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= lblFlgtdate.ClientID %>").style.display = 'none';
            document.getElementById("<%= lblFlightNo.ClientID %>").style.display = 'none';
            document.getElementById("<%= txtFlightNo.ClientID %>").style.display = 'none';
            document.getElementById("<%= txtFlightFromdate0.ClientID %>").style.display = 'none';
            document.getElementById("<%= btnList.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= btnclear.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= chkHAWBnew.ClientID %>").style.visibility = 'hidden';
            document.getElementById("<%= lblchk.ClientID %>").style.visibility = 'hidden';
            //document.getElementById('chk').style.display = 'none';
            //document.getElementById("<%= txtAWBNumber.ClientID %>").value = '';
            //alert("innerHtml");

        }
        else
            if (selection == "FHL") {

            document.getElementById("<%= lblawbnum.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= txtAWBNumber.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= lblFlgtdate.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= lblFlightNo.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= txtFlightNo.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= txtFlightFromdate0.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= btnList.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= btnclear.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= chkHAWBnew.ClientID %>").style.visibility = 'visible';
            document.getElementById("<%= lblchk.ClientID %>").style.visibility = 'visible';
            //document.getElementById('chk').style.display='block';
        }
        else
            if (selection == "FBL" || selection == "FFM") {
            document.getElementById("<%= lblawbnum.ClientID %>").style.display = 'none';
            document.getElementById("<%= txtAWBNumber.ClientID %>").style.display = 'none';
            document.getElementById("<%= lblFlgtdate.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= lblFlightNo.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= txtFlightNo.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= txtFlightFromdate0.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= btnList.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= btnclear.ClientID %>").style.display = 'inherit';
            document.getElementById("<%= chkHAWBnew.ClientID %>").style.visibility = 'hidden';
            document.getElementById("<%= lblchk.ClientID %>").style.visibility = 'hidden';
            //document.getElementById('chk').style.display = 'none';


        }
        else {
            document.getElementById("<%= lblawbnum.ClientID %>").style.display = 'none';
            document.getElementById("<%= txtAWBNumber.ClientID %>").style.display = 'none';
            document.getElementById("<%= lblFlgtdate.ClientID %>").style.display = 'none';
            document.getElementById("<%= lblFlightNo.ClientID %>").style.display = 'none';
            document.getElementById("<%= txtFlightNo.ClientID %>").style.display = 'none';
            document.getElementById("<%= txtFlightFromdate0.ClientID %>").style.display = 'none';
            document.getElementById("<%= btnList.ClientID %>").style.display = 'none';
            document.getElementById("<%= btnclear.ClientID %>").style.display = 'none';
            document.getElementById("<%= chkHAWBnew.ClientID %>").style.visibility = 'hidden';
            document.getElementById("<%= lblchk.ClientID %>").style.visibility = 'hidden';
            //document.getElementById('chk').style.display='none';

        }
    }

</script>
     <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="updatepnl" runat="server">
                    <ContentTemplate>
     <div id="contentarea">
   
    
    <h1>Message Center</h1>
    
     <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True"  
             ForeColor="Red"></asp:Label>
            <%--<img alt="" src="Images/flightshlist.png"  style="vertical-align:5"/> </h1>--%>
                <div class="botline">
                   <table cellpadding="3" cellspacing="3"><tr>
                        </td>
                    <td class="style3"> Message :</td>
                        <td>
                        <asp:DropDownList ID="ddlMsgType" runat="server" OnSelectedIndexChanged="ddlMsgType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                        <td class="style4">&nbsp;</td>
                        <td>
                            &nbsp;&nbsp;Communication Type :</td>
                        <td>
                      
                         
                        <asp:DropDownList ID="ddlCommType" runat="server" 
                            ValidationGroup="send">
                            <asp:ListItem>ALL</asp:ListItem>
                            <asp:ListItem>Email</asp:ListItem>
                            <asp:ListItem>SITA</asp:ListItem>
                        </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rqrddl" runat="server" 
                                ControlToValidate="ddlCommType" Text="*" ValidationGroup="send"></asp:RequiredFieldValidator>
                        </td>
                        
                        <td class="style5">
                            &nbsp;</td>
                        <td>
                            Partner Type :</td>
                        <td>
                            <asp:DropDownList ID="ddlPartnerType" runat="server" ValidationGroup="send" 
                                onselectedindexchanged="ddlPartnerType_SelectedIndexChanged" AutoPostBack="true" >
                            </asp:DropDownList>
                        </td>
                            <td>
                                &nbsp;</td>
                          
                            
                        <td class="style7">
                            &nbsp;</td>
                        <td>
                            Partner Code :</td>
                        <td>
                            <asp:TextBox ID="txtPartnerCode" runat="server" AutoPostBack="true" 
                                ontextchanged="txtPartnerCode_TextChanged" ValidationGroup="send"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="txtPartnerCode_AutoCompleteExtender" 
                                runat="server" EnableCaching="true" Enabled="True" MinimumPrefixLength="1" 
                                ServiceMethod="GetPartnerType" ServicePath="~/msgOverview.aspx" 
                                TargetControlID="txtPartnerCode" UseContextKey="true">
                            </asp:AutoCompleteExtender>
                        </td>
                        <td>
                            &nbsp;</td>
                          
                            
                        </tr>
                        </table>
                    
                        
          <table cellpadding="3" cellspacing="3">          
                        <tr>
                         
                            <td>
                                Email ID </td>
                            <td>
                                <asp:TextBox ID="txtmailId1" runat="server" TextMode="MultiLine" 
                                    ValidationGroup="send" Width="615px" Height="33px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="txtmailId1" Text="*" ValidationGroup="send"></asp:RequiredFieldValidator>
                            </td>
                          
                            
                        </tr>
                    
                    </table>
                </div><br/>
                
                <div class="divback">
              
               <table cellpadding="3" cellspacing="3">
                 <tr><td><asp:Label Text="Flight #" ID="lblFlightNo" runat="server" Visible="false"   ></asp:Label></td>
                 <td><asp:TextBox ID="txtFlightNo" runat="server" Visible="false" > </asp:TextBox>
                 </td>
                 <td><asp:Label ID="lblFlgtdate" runat="server" Text="Flight Date :" Visible="false"  ></asp:Label></td>
                    <td><asp:TextBox ID="txtFlightFromdate0" runat="server" Visible="false"   
                            ToolTip="Please enter date format: dd/MM/yyyy" Width="100px" ></asp:TextBox>
                            <asp:CalendarExtender ID="txtFlightFromdate0_CalendarExtender" runat="server" 
                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFlightFromdate0">
                        </asp:CalendarExtender>
                        </td>
                 </tr>
                 <tr><td><asp:Label ID="lblawbnum" runat="server" Text="AWB #" Visible="false" ></asp:Label></td>
                 <td><asp:TextBox ID="txtAWBNumber" runat="server" Visible="false" > </asp:TextBox></td>
                 <td><div id="chk" style="float:left;">
                             <asp:CheckBox ID="chkHAWBnew" runat="server"  Text="Send HAWB" Visible="false"  />
                             <asp:Label ID="lblchk" runat="server" style="visibility:hidden;" Text="Send HAWB"></asp:Label>
                             </div></td><td></td>
                 </tr>
                 <tr><td><asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
                                  Width="46px" onclick="btnList_Click" Visible="false" 
                         ValidationGroup="list"  /></td><td><asp:Button ID="btnclear" runat="server" CssClass="button" Text="Clear" 
                                  onclick="btnclear_Click" Visible="false"  /></td><td></td><td></td></tr>
                 </table>
                 
                             
              </div>        
                       
                <asp:MultiView ID="multiview_msgcenter" runat="server"> 
 <asp:View ID="grdview" runat="server"><div id="divfbl">
    
    
            
            <div id="divgrdv" style="margin-top:25px;">
            <asp:GridView ID="gdvULDLoadPlanAWB" runat="server" AutoGenerateColumns="False" 
                                                    CellPadding="3" CellSpacing="3" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField AccessibleHeaderText="AWBNo" HeaderText="AWB">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAWBno" runat="server" Text='<%# Eval("AWBNumber")%>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField AccessibleHeaderText="Pieces" HeaderText="PCS">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPieces" runat="server" Text='<%# Eval("PiecesCount")%>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField AccessibleHeaderText="Weight" HeaderText="Wgt">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWeight" runat="server" Text='<%# Eval("ChargedWeight")%>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField AccessibleHeaderText="AvlPCS" HeaderText="A.PCS">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAvlPCS" runat="server" Text='<%# Eval("PiecesCount")%>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField AccessibleHeaderText="AvlWgt" HeaderText="A.Wgt">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAvlWgt" runat="server" Text='<%# Eval("WT")%>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField AccessibleHeaderText="TotPCS" HeaderText="T.PCS">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTotPCS" runat="server" Text='<%# Eval("AWBPcs")%>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField AccessibleHeaderText="TotWgt" HeaderText="T.Wgt">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTotWgt" runat="server" Text='<%# Eval("AWBGwt")%>'></asp:Label></ItemTemplate></asp:TemplateField></Columns><AlternatingRowStyle Wrap="False" />
                                                    <EditRowStyle CssClass="grdrowfont" />
                                                    <FooterStyle CssClass="grdrowfont" />
                                                    <HeaderStyle CssClass="titlecolr" Wrap="False" />
                                                    <RowStyle CssClass="grdrowfont"  HorizontalAlign="Center" Wrap="False" />
                                                </asp:GridView>
                                               
                                     
                                     </div>
             
            
        </asp:View> 
 <asp:View ID="viewawb" runat="server">
 <div id="divexeccdetail">
                                 
     <div id="divdetail">
        
            <div id="colleft" style="height: 89px">
                <h2>
                    <%--<img alt="" src="images/shipmentdetails.png" />--%>Consignment Details
                </h2>
                <div class="divback" style="width: 478px; height: 89px;">
                    <asp:UpdatePanel ID="UPFirst" runat="server">
                        <ContentTemplate>
                            <table border="0">
                                <tr>
                                    <td>
                                        Origin*
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOrg" runat="server" Width="110px" Enabled="false"  >
                                        </asp:DropDownList>
                                        &nbsp;
                                        <br />
                                    </td>
                                    <td>
                                        Destination*
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDest" runat="server" Width="110px"  MaxLength="3" TabIndex="3" Enabled="false"
                                            >
                                        </asp:DropDownList>
                                        &nbsp;
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Agent Code *
                                    </td>
                                    <td>
                                        <%--<asp:DropDownList ID="ddlAgtCode" runat="server" Width="110px" MaxLength="20" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlAgtCode_SelectedIndexChanged">
                                        </asp:DropDownList>--%>
                                        <asp:TextBox ID="TXTAgentCode" runat="server" Width="110px"  TabIndex="4" CssClass="styleUpper" onChange="callShow();" ReadOnly="true"></asp:TextBox>
                                        
                                        &nbsp;
                                        <br />
                                    </td>
                                    <td>
                                        Agent Name
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAgentName" runat="server" Width="110px" ReadOnly="True" TabIndex="5" ></asp:TextBox>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Customer Code
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTCustomerCode" runat="server" Width="110px" TabIndex="6" ReadOnly="true"></asp:TextBox>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div id="colright" style="width: 420px; height: 175px">
                <h2>
                    <%--<img alt="" src="images/txtcargodetails.png" />--%>Cargo Details
                </h2>
                <div class="divback" style="width: 400px; height: 89px">
                    <table border="0" style="width: 375px">
                        <tr>
                            <td width="120px">
                                Service Cargo Class *
                            </td>
                            <td width="120px">
                                <asp:DropDownList ID="ddlServiceclass" runat="server" TabIndex="7" Enabled="false" 
                                    >
                                  
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td width="120px">
                                Handling Info / Invoice No.
                            </td>
                            <td width="120px">
                                <asp:TextBox ID="txtHandling" runat="server" Height="23px" TabIndex="8" TextMode="MultiLine" MaxLength="100" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td width="340px" colspan="2">
                                <asp:CheckBox ID="CHKConsole" runat="server" Text="Console" TabIndex="9" Enabled="false" />&nbsp;
                                <asp:CheckBox ID="CHKBonded" runat="server" Text="Bonded" TabIndex="10" Enabled="false" />&nbsp;
                                <asp:CheckBox ID="CHKExportShipment" runat="server" Text="Export Shipment" Enabled="false" 
                                    TabIndex="11" />
                                <asp:CheckBox ID="CHKAsAggred" runat="server" Text="As Agreed" TabIndex="12" Enabled="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <br />
        <asp:UpdatePanel ID="UpSipperCon" runat="server">
            <ContentTemplate>
                <div style="float: left">
                    <table cellpadding="3" cellspacing="3">
                        <tr>
                            <td>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="TXTDvForCustoms" runat="server" Width="120px" TabIndex="13" ReadOnly="true" Visible="false"></asp:TextBox>
                            </td>
                            <td>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="TXTDvForCarriage" runat="server" Width="120px" TabIndex="14" ReadOnly="true" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" valign="bottom">
                                &nbsp;&nbsp;
                                <br />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div id="DivShipperCon" style="background: #FFFFFF; width: 900px; height: 150px;
                    display: none; float: left">
                   
                    <table style="width: 100%; height: 100%; left: 0%; top: 0%">
                        <tr>
                            <td>
                                <div id="Div2" style="">
                                    <h2>
                                        Shipper Details
                                    </h2>
                                    <div class="divback" style="width: 450px; height: 89px;">
                                        <table style="vertical-align: top">
                                            <tr>
                                                <td>
                                                    Shipper Name
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTShipper" runat="server" MaxLength="50" 
                                                        AutoPostBack="false" TabIndex="16" ReadOnly="true" ></asp:TextBox>
                                                   >
                                                </td>
                                                <td width="5px">
                                                </td>
                                                <td class="style11">
                                                    Address
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTShipAddress" MaxLength="100" runat="server" TextMode="MultiLine" TabIndex="17"
                                                        Width="129px" Height="31px" AutoPostBack="false" ReadOnly="true"></asp:TextBox>
                                                </td>
                                                <td width="5px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Country
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlShipCountry" runat="server" TabIndex="18" Enabled="false" >
                                                        <asp:ListItem Text="IND" Value="IND"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="5px">
                                                </td>
                                                <td width="60px">
                                                    Telephone#
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTShipTelephone" runat="server" MaxLength="100" Width="129px" AutoPostBack="false" TabIndex="19" ReadOnly="true"
                                                        ></asp:TextBox>
                                                </td>
                                                <td width="5px">
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div id="Div3" style="">
                                    <h2>
                                        Consignee Details
                                    </h2>
                                    <div class="divback" style="width: 450px; height: 89px">
                                        <table>
                                            <tr>
                                                <td>
                                                    Consignee Name
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTConsignee" runat="server" MaxLength="50" AutoPostBack="false" TabIndex="20" ReadOnly="true"
                                                        ></asp:TextBox>
                                                    
                                                </td>
                                                <td width="5px">
                                                </td>
                                                <td>
                                                    Address
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTConAddress" runat="server" MaxLength="100" TextMode="MultiLine" TabIndex="21" ReadOnly="true"
                                                        Width="129px" Height="31px" ></asp:TextBox>
                                                </td>
                                                <td width="5px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Country
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlConCountry" runat="server" TabIndex="22" Enabled="false">
                                                        <asp:ListItem Text="IND" Value="IND"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td width="5px">
                                                </td>
                                                <td width="60px">
                                                    Telephone#
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="TXTConTelephone" MaxLength="100" runat="server" Width="129px" ReadOnly="true" 
                                                       TabIndex="23" ></asp:TextBox>
                                                </td>
                                                <td width="5px">
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    
    <br />
    <div style="float: left">
        <asp:UpdatePanel ID="UPMaterial" runat="server">
            <ContentTemplate>
                <h2>
                    Shipment Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                </h2>
                <div>
                    <table frame="void">
                        <tr>
                            <td valign="top" style="vertical-align: top; width: 700px">
                                <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" ID="grdMaterialDetails"
                                    Width="700px">
                                    <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CHKSelect" runat="server" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comm Code *">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlMaterialCommCode" TabIndex="24" DataTextField='<%# Eval("CommodityCode") %>' runat="server" Width="100px" CssClass="grdrowfont" Enabled="false" >
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Comm Desc *">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMaterialCommDesc" runat="server" Width="100px" MaxLength="35" TabIndex="25"
                                                    CssClass="grdrowfont" Text='<%# Eval("CodeDescription") %>' ReadOnly="true">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="LBLTotal" runat="server" Text="Total :"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                            <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pcs *">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTPcs" runat="server" Width="55px" MaxLength="5" 
                                                    Text='<%# Eval("Pieces") %>'  TabIndex="26" ReadOnly="true" ></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="LBLTotalPcs" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gross Wt *">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCommGrossWt" runat="server" Width="65px" MaxLength="10" 
                                                    Text='<%# Eval("GrossWeight") %>'  TabIndex="27" ReadOnly="true">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="LBLTotalGrWt" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dimension">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDimensions" runat="server" Text='<%# Eval("Dimensions") %>' Visible="false">
                                                </asp:Label>&nbsp;&nbsp;
                                                <asp:ImageButton ID="btnDimensionsPopup" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle"  />
                                                <asp:HiddenField ID="HidRowIndex" runat="server" Value='<%# Eval("RowIndex") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Volume *">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCommVolWt" runat="server" Width="150px" Text='<%# Eval("VolumetricWeight") %>' ReadOnly="true"
                                                    TabIndex="28">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="LBLTotalVolume" runat="server" Text=""></asp:Label></FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Chargeable Wt *">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCommChargedWt" runat="server" Width="150px" Text='<%# Eval("ChargedWeight") %>' ReadOnly="true"
                                                    TabIndex="29">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="LBLTotalChargedWt" runat="server" Text=""></asp:Label></FooterTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pay Mode">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlPaymentMode" runat="server" Width="75px" DataTextField='<% Eval("PaymentMode")%>' CssClass="grdrowfont" Enabled="false">
                                                
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Account Info.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAccountInfo" runat="server" Width="150px" MaxLength="100"
                                                Text='<%# Eval("AccountInfo") %>' ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>                                            
                                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                                    <FooterStyle CssClass="grdrowfont"></FooterStyle>
                                    <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                                    <RowStyle CssClass="grdrowfont"></RowStyle>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div style="float: left">
        <asp:UpdatePanel ID="UpdatePanelRouteDetails" runat="server">
            <ContentTemplate>
                <asp:Label ID="LBLRouteStatus" runat="server" ForeColor="Red"></asp:Label>
                <h2 style="width: 600px">
                    Route Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                </h2>
                <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" CssClass="grdrowfont"
                    Width="399px" ID="grdRouting">
                    <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="CHKSelect" runat="server" Enabled="false" />
                                <asp:HiddenField ID="HidScheduleID" runat="server" Value='<%# Eval("ScheduleID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Flight Origin *" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFltOrig" runat="server" Width="55px" CssClass="styleUpper" ReadOnly="true" 
                                    Text='<%# Eval("FltOrigin") %>'> <%--TabIndex="31"--%>
                                </asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Flight Destination*" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFltDest" runat="server" Width="55px" Text='<%# Eval("FltDestination") %>' ReadOnly="true"
                                    CssClass="styleUpper" ><%--TabIndex="32"--%>
                                </asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Interline" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="CHKInterline" runat="server" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Flight    Date *" HeaderStyle-Width="10px"  HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFdate" runat="server" Width="80px" Text='<%# Eval("FltDate") %>' ReadOnly="true" ></asp:TextBox>
                                
                            </ItemTemplate>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Flight #*" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlFltNum" runat="server" Width="90px" Enabled="false"  >
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Pcs">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPcs" runat="server" Width="70px" Text='<%# Eval("Pcs") %>' MaxLength="5" ReadOnly="true"
                                    >
                                </asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gross Wt">
                            <ItemTemplate>
                                <asp:TextBox ID="txtWt" runat="server" Width="80px" Text='<%# Eval("Wt") %>' MaxLength="9" ReadOnly="true">
                                </asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Chargeable Wt" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtChrgWt" runat="server" Width="80px" Text='<%# Eval("ChrgWt") %>' MaxLength="9" ReadOnly="true" >
                                </asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="100px" Enabled="false">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FFR" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkFFR" runat="server" Visible="false"  ></asp:CheckBox>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Accepted" Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAccepted" runat="server" Visible="false" >
                                </asp:CheckBox>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center"></FooterStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Accepted Pcs">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAcceptedPcs" runat="server" Width="70px" Text='<%# Eval("AcceptedPcs") %>'
                                    MaxLength="5" ReadOnly="true">
                                </asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Accepted Wt"  HeaderStyle-VerticalAlign="Middle" HeaderStyle-Width="10px"  HeaderStyle-Wrap="true">
                            <ItemTemplate> 
                                <asp:TextBox ID="txtAcceptedWt" runat="server" Width="80px" Text='<%# Eval("AcceptedWt") %>'
                                    MaxLength="5" ReadOnly="true">
                                </asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                    <FooterStyle CssClass="grdrowfont"></FooterStyle>
                    <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
                    <RowStyle CssClass="grdrowfont"></RowStyle>
                </asp:GridView>
            </ContentTemplate>
            
        </asp:UpdatePanel>
    </div>
    <br />
    <div style="float: left">
        <asp:UpdatePanel ID="UPprocess" runat="server">
            <ContentTemplate>
                <div style="font-size: 18pt">
                    <h2>
                        Rate Details &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </h2>
                </div>
                <div style="width: 1020px;">
                    <table frame="void" width="100%">
                        <tr>
                            <td valign="top" style="vertical-align: top; width: 500px">
                                <asp:Label ID="rateprocessstatus" runat="server" Text="" ForeColor="Red"></asp:Label><asp:GridView
                                    ID="GRDRates" runat="server" ShowFooter="True" AutoGenerateColumns="False">
                                    <AlternatingRowStyle CssClass="trcolor" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="check" runat="server" Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Com. Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTCommCode" runat="server" Width="65px" Text='<%# Eval("CommCode") %>' ReadOnly="true"
                                                    CssClass="grdrowfont">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pcs ">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTPcs" runat="server" Text='<%# Eval("Pcs") %>' Width="45px" ReadOnly="true">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="LBLTotalPcs" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Wt">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTWt" runat="server" Text='<%# Eval("Weight") %>' Width="65px" ReadOnly="true">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="LBLTotalWt" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="C.Wt">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTCWt" runat="server" Text='<%# Eval("ChargedWeight") %>' Width="65px" ReadOnly="true"  >
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="LBLCTotalWt" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Freight IATA">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTFrIATA" runat="server" Text='<%# Eval("FrIATA") %>' Width="60px" ReadOnly="true">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="LBLTotalFrIATA" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Freight Mkt">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTFrMKT" runat="server" Text='<%# Eval("FrMKT") %>' Width="60px" ReadOnly="true">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="LBLTotalFrMKT" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate/Kg.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRatePerKg" runat="server" Text='<%# Eval("RatePerKg") %>' Width="50px" ReadOnly="true">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalRate" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Val Chgs">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTValCharg" runat="server" Text='<%# Eval("ValCharge") %>' Width="45px" ReadOnly="true">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pay Mode">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlPayMode" runat="server" Enabled = "false" DataTextField='<%# Eval("PayMode") %>' >
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OC Due Car">
                                            <FooterTemplate>
                                                <asp:Label ID="LBLTotalOCDC" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTOcDueCar" runat="server" Text='<%# Eval("OcDueCar") %>' Width="65px"
                                                    ReadOnly="true">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                <asp:ImageButton ID="btnOcDueCar" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" Enabled="false"  />
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OC Due Agent">
                                            <FooterTemplate>
                                                <asp:Label ID="LBLTotalOCDA" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTOcDueAgent" runat="server" Text='<%# Eval("OcDueAgent") %>' Width="65px" ReadOnly="true"
                                                    >
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                                <asp:ImageButton ID="btnOcDueAgent" runat="server" ImageUrl="~/Images/list_bullets.png"
                                                    ImageAlign="AbsMiddle" Enabled="false"  />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Spot Rate" Visible="false">
                                            <FooterTemplate>
                                                <asp:Label ID="lblSpotRate" runat="server" Text="" Width="55px">
                                                     </asp:Label>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTSpotRate" runat="server" Text='<%# Eval("SpotRate") %>' Width="65px" ReadOnly ="true">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sp Rate Id.">
                                            <FooterTemplate>
                                                <asp:Label ID="lblSpotRateID" runat="server" Text="" Width="55px">
                                                     </asp:Label>
                                            </FooterTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTSpotRateID" runat="server" Text='<%# Eval("SpotRateID") %>' Width="65px" ReadOnly ="true">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dyn Rate">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTDynRate" runat="server" Text='<%# Eval("DynRate") %>' Width="50px" ReadOnly="true">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Serv Tax">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTServiceTax" runat="server" Text='<%# Eval("ServTax") %>' Width="65px" ReadOnly="true">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="LBLTotalTax" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TXTTotal" runat="server" Text='<%# Eval("Total") %>' Width="80px" ReadOnly="true">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="LBLTotal" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle CssClass="grdrowfont" />
                                    <FooterStyle CssClass="grdrowfont" />
                                    <HeaderStyle CssClass="titlecolr" />
                                    <RowStyle CssClass="grdrowfont" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:UpdateProgress ID="UpdateProgress" runat="server">
                <ProgressTemplate>
                <asp:Image ID="Image1" ImageUrl="~/Images/Wait.gif" AlternateText="Processing" runat="server" />
            </ProgressTemplate>
                </asp:UpdateProgress>
                
                <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress"
                PopupControlID="UpdateProgress" BackgroundCssClass="modalPopup"/>

            </ContentTemplate>
            
        </asp:UpdatePanel>
    </div>
    <br />
    
    
    <div id="fotbut">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
               <table>
            <tr>
                <td>
                    Execution Date: *
                </td>
                <td>
                    <asp:TextBox ID="txtExecutionDate" runat="server" Width="90px" ReadOnly="true" ></asp:TextBox>
                     <asp:ImageButton ID="btnExecutionDate" runat="server" ImageUrl="~/Images/calendar_2.png" ImageAlign="AbsMiddle" Enabled="false" />
     
                    
                </td>
                <td style="width: 90px">
                    Executed By *
                </td>
                <td>
                    <asp:TextBox ID="txtExecutedBy" runat="server" Width="100px"  ReadOnly="true">QIDTECH</asp:TextBox>
                </td>
                <td style="width: 90px">
                    Executed At *
                </td>
                <td style="width: 70px">
                    <asp:TextBox ID="txtExecutedAt" runat="server" Width="60px" ReadOnly="true">PUNE</asp:TextBox>
                </td>
                
                <td style="width: 90px">
                    Remarks
                </td>
                <td>
                    <asp:TextBox ID="txtRemarks" runat="server" Width="250px" MaxLength="250" ReadOnly="true"></asp:TextBox>
                </td>
                
            </tr>
        </table>
        
      
       
            </ContentTemplate>
            
        </asp:UpdatePanel></div></div>
 
 </asp:View>
 <asp:View ID="viewuld" runat="server">
 <asp:GridView ID="gdvULDDetails"  
                              runat="server" CellPadding="3" 
                                            CellSpacing="3" 
    AutoGenerateColumns="False"  >
                                                 <%-- onselectedindexchanged="gdvULDDetails_SelectedIndexChanged" --%>
                                                 <Columns>
                                                     <asp:BoundField AccessibleHeaderText="ULD" DataField="ULDno" 
                                                            HeaderText="ULD" />
                                                     <asp:BoundField AccessibleHeaderText="POU" DataField="POU" 
                                                            HeaderText="POU" />
                                                     <asp:BoundField AccessibleHeaderText="POL" DataField="POL" 
                                                            HeaderText="POL" />
                                                     <asp:BoundField AccessibleHeaderText="ULDdest" DataField="AWBDest" 
                                                            HeaderText="ULD Dest"  HeaderStyle-Width ="5px" HeaderStyle-Wrap="true" />
                                                     <asp:BoundField AccessibleHeaderText="Counter" DataField="counter" ItemStyle-Width="0px"
                                                            HeaderText="Lane" />
                                                     <asp:BoundField AccessibleHeaderText="AWBno" DataField="AWBNumber" 
                                                            HeaderText="AWB" />
                                                     <asp:BoundField AccessibleHeaderText="SCC" DataField="SCC" 
                                                            HeaderText="SCC" />
                                                     <asp:BoundField AccessibleHeaderText="PCS" DataField="PCS" 
                                                            HeaderText="Manifested PCS"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true"/>
                                                     <asp:BoundField AccessibleHeaderText="GrossWGT" DataField="GrossWgt" 
                                                            HeaderText="Manifested WGT"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true" />
                                                     <asp:BoundField AccessibleHeaderText="VOL" DataField="Vol" 
                                                            HeaderText="VOL" />
                                                            
                                                     <asp:BoundField AccessibleHeaderText="BookedPCS" DataField="AWBPcs" 
                                                        HeaderText="AWB Pcs"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true"/>
                                                     <asp:BoundField AccessibleHeaderText="BookedWgt" DataField="AWBGwt" 
                                                        HeaderText="AWB Wgt"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true"/>
                                                        
                                                     <asp:BoundField AccessibleHeaderText="StatedPCS" DataField="StatedPCS" 
                                                            HeaderText="Accepted Pcs"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true"/>
                                                     <asp:BoundField AccessibleHeaderText="StatedWgt" DataField="StatedWgt" 
                                                            HeaderText="Accepted Wgt"  HeaderStyle-Width="10px" HeaderStyle-Wrap ="true"/>
                                                     <asp:BoundField AccessibleHeaderText="Desc" DataField="Desc" 
                                                            HeaderText="Desc" />
                                                     <asp:BoundField AccessibleHeaderText="Orign" DataField="Org" 
                                                            HeaderText="Orign" />
                                                     <asp:BoundField AccessibleHeaderText="Dest" DataField="AWBDest" 
                                                            HeaderText="Dest" />
                                                       <asp:BoundField AccessibleHeaderText="Dest" DataField="Manifested" 
                                                            HeaderText="Manifested" />     
                                                     <asp:TemplateField AccessibleHeaderText="Remark" HeaderText="Remark">
                                                         <ItemTemplate>
                                                             <asp:TextBox ID="TextBox1" runat="server" Text="" MaxLength="100" ReadOnly="true"></asp:TextBox>
                                                         </ItemTemplate>
                                                     </asp:TemplateField>
                                                 </Columns>
                                                 <HeaderStyle CssClass="titlecolr"/>
                                                 <AlternatingRowStyle CssClass="trcolor" Wrap="False" />
                                                 <EditRowStyle CssClass="grdrowfont" />
                                                 <RowStyle CssClass="grdrowfont" Wrap="False" HorizontalAlign="Center" />
                                                 <FooterStyle CssClass="grdrowfont"/>
                                             </asp:GridView>
 </asp:View>
 </asp:MultiView>
 
        
    
   
    
  
    
        <div style="float:left">
           <table>
   <tr>
   <td><asp:Button ID="btnSendMsg" runat="server" CssClass="button" 
           Text="Send Msg" Width="77px" onclick="btnSendMsg_Click" Visible="false" ValidationGroup="send" /></td></tr>
   </table>
         </div>
         <asp:HiddenField ID="selectionhd" runat="server" />
             
        
             <div id="msglight" class="white_contentmsg" >
<table>

<tr>

<td width="5%" align="center">
    <br />
    <img src="Images/loading.gif" />
    <br />
    <asp:Label ID="msgshow" runat="server" ></asp:Label></td></tr></table></div>
    <div id="msgfade" class="black_overlaymsg"></div>
         <div id="Lightsplit"  class="white_contentnew">
<table width="100%">
<tr>
<td>

</td>
</tr>

<tr>
<td>

</td>
<td>
<div style="overflow: auto; height: 400px; width: 500px;" align="center">
<table>
<tr>
<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label ID=lblmsg runat="server" Text="Message Content" Font-Size="Medium" Font-Bold="true"> </asp:Label>  </td></tr>
<tr>
<td>
<asp:TextBox ID="txtMessageBody" runat="server" TextMode="MultiLine" ReadOnly="true"  
         Height="500px" Width="600px" style="OVERFLOW:auto"  ></asp:TextBox></td></tr></table></div><br />
         <div align="center">
         
<input type="button" id="btnSplitCancel" class="button" value="Close"  onclick="HidePanelSplit();" size="150%" />
</div>
    
    </td>
</tr>
<tr>
<td>
</td>
</tr>



</table>
		</div>
		<div id="fadesplit" class="black_overlaynew"></div>
		
                                         </ContentTemplate>
                                         </asp:UpdatePanel>
        
     
     
     
		 
    </asp:Content>