<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GHA_frmFlightCapacityPlanning.aspx.cs"    MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.GHA_frmFlightCapacityPlanning" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <script type="text/javascript">

     Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

     function callShow() {
         document.getElementById('msglight').style.display = 'block';
         document.getElementById('msgfade').style.display = 'block';

     }
     function callclose() {
         document.getElementById('msglight').style.display = 'none';
         document.getElementById('msgfade').style.display = 'none';
     }

     </script>
     <style type="text/css">
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
    <script language="javascript" type="text/javascript">
      
    function getid(e) {
        var ctrlname = e.id.replace('btnManage', 'lblAWBno');
        var valExpectedAWB = document.getElementById(ctrlname).innerHTML;
        if (valExpectedAWB.trim() == "") {
            alert("No AWB Available");
            return false;
        }
        else {
            document.getElementById("<%=hdn.ClientID %>").value = valExpectedAWB;
            return true;
        }
    }

    function cllsa() {
        
    document.getElementById("<%=btnList.ClientID %>").click();
    }
    function expandcollapse(obj, row) {
        var div = document.getElementById(obj);
        var img = document.getElementById('img' + obj);

        if (div.style.display == "none") {
            div.style.display = "block";
            if (row == 'alt') {
                img.src = "minus.gif";
            }
            else {
                img.src = "minus.gif";
            }
            img.alt = "Close to view other Customers";
        }
        else {
            div.style.display = "none";
            if (row == 'alt') {
                img.src = "plus.gif";
            }
            else {
                img.src = "plus.gif";
            }
            img.alt = "Expand to show Orders";
        }
    } 
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:HiddenField ID="hdn" runat="server" />
        <div id="contentarea">
            <h1>Capacity Planning</h1>
            <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Large"></asp:Label>
        <div class="botline">
        <table width="100%">
        <tr>
        <td>
            <asp:UpdatePanel ID="UPFirst" runat="server">
                <ContentTemplate>
                    <table style="width: 950px; height: 100%" border="0">
                        <tr>
                            <td>
                                Flt Orig
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSource" runat="server" 
                                    OnSelectedIndexChanged="ddlSource_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Flt Dest
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDest" runat="server" 
                                    OnSelectedIndexChanged="ddlDest_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Flight
                            </td>
                            <td>
                                <%--<asp:DropDownList ID="ddlFlight" runat="server">
                                </asp:DropDownList>--%>
                                                            <asp:TextBox ID="ddlFlight" runat="server" MaxLength="8" Width="80px">DY</asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="LBLAWBStatus" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLStatus" runat="server">
                                    <asp:ListItem Text="Overbooked" Value="B"></asp:ListItem>       
                                    <asp:ListItem Text="ALL" Value="A" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                From Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" Width="115px"></asp:TextBox>
                                <asp:ImageButton ID="btnFromDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtFromDate"
                                  runat="server" ErrorMessage="*" ValidationGroup="List">
                                 </asp:RequiredFieldValidator>
                                <asp:CalendarExtender ID="CEFromDate" Format="yyyy-MM-dd" TargetControlID="txtFromDate"
                                    PopupButtonID="btnFromDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                To Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" Width="115px"></asp:TextBox>
                                <asp:ImageButton ID="btnToDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtToDate"
                                  runat="server" ErrorMessage="*" ValidationGroup="List">
                                 </asp:RequiredFieldValidator>
                                <asp:CalendarExtender ID="CEToDate" Format="yyyy-MM-dd" TargetControlID="txtToDate"
                                    PopupButtonID="btnToDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                           
                            <td>
                              Agent Code
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAgentCode" runat="server" Width="80px" 
                                    AppendDataBoundItems="True">                                  
                                </asp:DropDownList>
                            </td>
                             <td>
                               <%-- AWBNumber--%>
                                 AWB</td>
                            <td>
                                <asp:TextBox ID="TXTAWBPrefix" runat="server" Width="30px" MaxLength="4" 
                                    Enabled="True" >328</asp:TextBox>&nbsp
                                <asp:TextBox ID="TXTAWBNumber" runat="server" Width="110px" MaxLength="8"></asp:TextBox>
                            </td>
                            
                            <td>
                                <asp:Button ID="btnList" runat="server" CssClass="button" 
                                    onclick="btnList_Click" OnClientClick="callShow();" Text="List" 
                                    ValidationGroup="List" />
                                <%--<asp:Button ID="btnExport" runat="server" CssClass="button" 
                                    onclick="btnExport_Click" Text="Export" Visible="false" />--%>
                                    <asp:Button ID="btnExport" runat="server" CssClass="button" OnClick="btnExport_Click" Text="Export" Visible="false" />
                            </td>
                            
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="btnExport" EventName="btnExport_Click" />--%>
                <asp:PostBackTrigger ControlID="btnExport" />
                   <%-- <asp:AsyncPostBackTrigger ControlID="btnList" EventName="Click" />--%>
                </Triggers>
            </asp:UpdatePanel>
            </td>
            <td>
             
                                    
                                    </td>
        </tr>
        </table>
        </div>
       
        
        <asp:UpdatePanel ID="upup" runat="server">
        <ContentTemplate>
        <div style="width:100%;" class="ltfloat">
        <asp:GridView ID="GridView1" AllowPaging="false" 
            AutoGenerateColumns="false" 
            style="Z-INDEX: 101" Width="100%"
        ShowFooter="false" Font-Size="Small"
            Font-Names="Verdana" runat="server" GridLines="None" 
           OnRowDataBound="GridView1_RowDataBound"
         BorderStyle="Outset"
        AllowSorting="true"  
             PageSize="7" CssClass="GridViewStyle" >
            <RowStyle CssClass="RowStyle"  HorizontalAlign="Left"/>
            <HeaderStyle CssClass="HeaderStyle"  Height="30px" HorizontalAlign="center"/>
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="javascript:expandcollapse('div<%# Eval("FlightNo") + "," + Eval("FlightDate") + "," + Eval("DeptTime") + "," + Eval("ArrTime") %>', 'one');">
                            <img id="imgdiv<%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>" alt="Click to show/hide Orders for Customer <%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>"  width="9px" border="0" src="plus.gif"/>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flt Orig" >
                    <ItemTemplate>
                        <asp:Label ID="lblOrigin" Text='<%# Eval("Origin") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="Flt Dest" >
                    <ItemTemplate>
                        <asp:Label ID="lblDestination" Text='<%# Eval("Destination") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flight#" >
                    <ItemTemplate>
                        <asp:Label ID="lblFlightNo" Text='<%# Eval("FlightNo") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flight Date" >
                    <ItemTemplate>
                        <asp:Label ID="lblFlightDate" Text='<%# Eval("FlightDate") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Dep Time" >
                    <ItemTemplate>
                        <asp:Label ID="lblDepTime" Text='<%# Eval("DeptTime") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
			      <asp:TemplateField HeaderText="Arr Time" >
                    <ItemTemplate>
                        <asp:Label ID="lblArrTime" Text='<%# Eval("ArrTime") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="Maximum" >
                    <ItemTemplate>
                        <asp:Label ID="lblMaximum" Text='<%# Eval("Maximum") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Confirmed" >
                    <ItemTemplate>
                        <asp:Label ID="lblConfirmed" Text='<%# Eval("Confirmed") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Queued" >
                    <ItemTemplate>
                        <asp:Label ID="lblQueued" Text='<%# Eval("Queued") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Blocked" >
                    <ItemTemplate>
                        <asp:Label ID="lblBlocked" Text='<%# Eval("Blocked") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Available" >
                    <ItemTemplate>
                        <asp:Label ID="lblAvailable" Text='<%# Eval("Available") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
			    <asp:TemplateField>
			        <ItemTemplate>
			            <tr>
                            <td colspan="100%">
                                <div id="div<%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>" style="display:none;position:relative;left:15px;OVERFLOW: auto;WIDTH:97%" >
                                    <asp:GridView ID="GridView2" AllowPaging="false" AllowSorting="false"
                                     Width="100%" Font-Size="X-Small"
                                        AutoGenerateColumns="false" Font-Names="Verdana" runat="server"  ShowFooter="false"
                                       GridLines="None" 
                                          BorderStyle="Double" BorderColor="#0083C1" CssClass="GridViewStyle" >
                                        <RowStyle CssClass="RowStyle"  HorizontalAlign="Left"/>
                                        <HeaderStyle  BackColor="#656667" ForeColor="White"  Height="30px" HorizontalAlign="center"/>
                                        <FooterStyle />
                                        <Columns>
                                            <asp:TemplateField HeaderText="AWB#">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAWBno" Text='<%# Eval("AWBNo") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comm. Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCommCode" Text='<%# Eval("CommodityCode") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Origin">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAWBOrigin" Text='<%# Eval("Origin") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Dest.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAWBDest" Text='<%# Eval("Dest") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Agent">
                                                <ItemTemplate>  <asp:Label ID="lblAgent" Text='<%# Eval("Agent") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pcs" >
                                                <ItemTemplate> <asp:Label ID="lblPriority" Text='<%# Eval("Priority") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Weight" >
                                                <ItemTemplate><asp:Label ID="lblWeight" Text='<%# Eval("Weight") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
			                                <asp:TemplateField HeaderText="Status" >
                                                <ItemTemplate><asp:Label ID="lblStatus" Text='<%# Eval("Status") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                
                                                <asp:Button ID="btnManage" runat="server" Text="Manage" OnClientClick="return getid(this);" OnClick="editMode" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                   </asp:GridView>
                                </div>
                             </td>
                        </tr>
			        </ItemTemplate>			       
			    </asp:TemplateField>			    
			</Columns>
			 <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>   
       </div>
       </ContentTemplate>
        </asp:UpdatePanel>
        
        <div id="msglight" class="white_contentmsg" >
<table>

<tr>

<td width="5%" align="center">
    <br />
    <img src="Images/loading.gif" />
    
</td>
</tr>
</table>
		</div>
		<div id="msgfade" class="black_overlaymsg"></div>
		
		 </div>
       </asp:Content>