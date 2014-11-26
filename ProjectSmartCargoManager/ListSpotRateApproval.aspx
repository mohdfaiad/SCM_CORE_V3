<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListSpotRateApproval.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.ListSpotRateApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


<script language="javascript" type="text/javascript">

       function SelectAllgrdAddRate(CheckBoxControl) 
         {            
             for (i = 0; i < document.forms[0].elements.length; i++) 
             {
                 if (document.forms[0].elements[i].name.indexOf('check') > -1) 
                 {
                     document.forms[0].elements[i].checked = CheckBoxControl.checked;
                 }
             }
         }
    </script>
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
    <asp:ToolkitScriptManager ID="TSM0" runat="server">
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
    <div id="contentarea">
    
    <%--<div class="msg">--%>
   <%--     <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>--%>
    <%--</div>--%>
   
    
          <asp:UpdatePanel ID="UPFourth" runat="server">
                <ContentTemplate>
                     <div class="msg">
                     <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
                     </div>
                </ContentTemplate>
            </asp:UpdatePanel>
    
        <h1>
            <asp:Label ID="lblHeader" runat="server" Text=""></asp:Label> 
        </h1>
        
            
        
        <div class="botline">
        <div style="float:left">
            <asp:UpdatePanel ID="UPFirst" runat="server">
                <ContentTemplate>
                
                    <table style="width: 100%; height: 100%" border="0">
                        <tr>
                            <td >
                                Origin
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSource" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSource_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Destination
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDest" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDest_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Flight#
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFlight" runat="server">
                                <asp:ListItem Selected="True" Text="Select"></asp:ListItem>   
                                </asp:DropDownList>
                            </td>
                            <td>
                             Flight Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtFlightDate" runat="server" Width="100px" ></asp:TextBox>
                                <asp:CalendarExtender ID="txtFlightDate_CalendarExtender" runat="server" 
                               Enabled="True" TargetControlID="txtFlightDate" Format="MM/dd/yyyy">
                              </asp:CalendarExtender>
                                </td>
                               
                           <%--<td>
                                <asp:DropDownList ID="DDLStatus" runat="server">
                                    <asp:ListItem Text="Booked" Value="B"></asp:ListItem>
                                    <asp:ListItem Text="Executed" Value="E"></asp:ListItem>
                                    <asp:ListItem Text="Reopen" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Void" Value="V"></asp:ListItem>                                    
                                    <asp:ListItem Text="ALL" Value="A"></asp:ListItem>
                                </asp:DropDownList>
                            </td>--%>
                        </tr>
                        <tr>
                            <td>
                               Agent Code
                            </td>
                            <td>
                                <asp:TextBox ID="txtAgentCode" runat="server" Width="80px" 
                                    ontextchanged="txtAgentCode_TextChanged"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="ACEAgentCode" runat="server" ServiceMethod="GetAgentCode"
                                            CompletionInterval="0" EnableCaching="false" CompletionSetCount="10" TargetControlID="txtAgentCode" ServicePath="~/ListSpotRateApproval.aspx"  
                                            MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender> 
                            </td>
                            <td>
                             Spot Rate ID
                            </td>
                            <td>
                                <asp:TextBox ID="txtSpotRateId" runat="server" Width="80px" ></asp:TextBox>
                            </td>
                            <td>AWBNumber</td>
                            <td>
                                <asp:TextBox ID="txtAWBPrefix" runat="server" Width="40px"></asp:TextBox>
                                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" 
                                    runat="server" TargetControlID="txtAWBPrefix" WatermarkText="Prefix">
                                </asp:TextBoxWatermarkExtender>
                                <asp:TextBox ID="txtawbno" runat="server" Width="80px" MaxLength="8"></asp:TextBox>
                            </td>
                            <td>Status</td>
                            <td>
                            <asp:DropDownList id="ddlStatus" runat="server" Width="100px">
                            <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="New" Value="New"></asp:ListItem>
                            <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                             <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
                            </asp:DropDownList>
                            </td>
                            
                            <%-- <td>
                                To Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" Width="115px"></asp:TextBox><asp:ImageButton ID="btnToDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:CalendarExtender ID="CEToDate" Format="yyyy-MM-dd" TargetControlID="txtToDate"
                                    PopupButtonID="btnToDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                AWBNumber
                            </td>
                            <td>
                                <asp:TextBox ID="TXTAWBPrefix" runat="server" Width="30px" MaxLength="2" Text="SG"></asp:TextBox>&nbsp
                                <asp:TextBox ID="TXTAWBNumber" runat="server" Width="110px" MaxLength="8"></asp:TextBox></td><td colspan="2">
                                <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click" />
                                &nbsp;
                            </td>--%>
                        </tr>
                        <tr>
                <td>
                    From Date</td>
                <td>
                    <asp:TextBox ID="txtFromdate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtFromdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" 
                        Enabled="True" TargetControlID="txtFromdate" PopupButtonID="imgfromdate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgfromdate" runat="server" ImageAlign="AbsMiddle" 
                        ImageUrl="~/Images/calendar_2.png" />
                </td>
                <td>
                    To Date</td>
                <td>
                    <asp:TextBox ID="txtTodate" runat="server" Width="112px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtTodate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"  
                        Enabled="True" TargetControlID="txtTodate" PopupButtonID="imgtodate">
                    </asp:CalendarExtender>
                    <asp:ImageButton ID="imgtodate" runat="server" ImageAlign="AbsMiddle" 
                        ImageUrl="~/Images/calendar_2.png" />
                </td>
                                            
            </tr>
            
            
            <tr>
               <td>
                  <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" onclick="btnList_Click"  />&nbsp;
                  <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click"/>    &nbsp;
                   <asp:Button id="btnExport" runat="server" Text="Export" CssClass="button" onclick="btnExport_Click"/>
                 
               </td>
               
            </tr>
            
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnList" EventName="Click" />
                    <%--<asp:AsyncPostBackTrigger ControlID="lblStatus" />--%>
                </Triggers>
            </asp:UpdatePanel>
        </div> 
        </div>
        <br />
        <h2>
            Spot Rate Details </h2>
                <asp:UpdatePanel ID="UPThird" runat="server">
            <ContentTemplate>
                <asp:Label ID="LBLNoOfRecords" runat="server"></asp:Label>
                </ContentTemplate>
                </asp:UpdatePanel>
           <div class="divback">
            <asp:UpdatePanel ID="UPSecond" runat="server">
                <ContentTemplate>
              <div style="overflow:auto;">
               
                                     <asp:GridView ID="grdCreditdetails" runat="server" 
                         AutoGenerateColumns="False"   Width="100%" AllowPaging="True" 
                         onpageindexchanging="grdCreditdetails_PageIndexChanging" PageSize="8" 
                         onrowcommand="grdCreditdetails_RowCommand" BorderColor="Black">
            <Columns>
               
                <asp:TemplateField>
                         <HeaderTemplate>
                       <asp:CheckBox ID="chk" runat="server" onclick="javascript:SelectAllgrdAddRate(this);"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="check" runat="server" />
                    </ItemTemplate>
                    </asp:TemplateField>  
                <asp:TemplateField HeaderText="AWBNumber" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkAwbno" runat="server" Text='<%# Eval("AWBNumber") %>' CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="AWBNo" Width="100px"></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>        
                <asp:TemplateField HeaderText="AWBNumber" HeaderStyle-Wrap="true" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblAwbno" runat="server" Text='<%# Eval("AWBNumber") %>' Width="100px" ></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
               <asp:TemplateField HeaderText="Agent Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>' Width="80px" ></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                  <asp:TemplateField HeaderText="Agent Name" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAgentName" runat="server" Text='<%# Eval("AgentName") %>' Width="150px" ></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <%--<ItemStyle Wrap="False"/>--%>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="FlightNo" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblflightNo" runat="server" ValidationGroup="check" Width="50px" Text='<%# Eval("FlightNumber") %>' >
                        </asp:Label>
                       
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="FlightDate" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblflightdate" runat="server" Width="80px" Text='<%# Eval("FlightDate") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
   
               <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="false" >
                    <ItemTemplate>
                        <asp:Label  ID="lblOrigin" runat="server" Width="40px" Text='<%# Eval("Origin") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Dest" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblDest" runat="server" Width="40px" Text='<%# Eval("Destination") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
               
                             
                
                 <asp:TemplateField HeaderText="Spot Rate" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:TextBox  ID="txtSpotRate" runat="server" Width="40px" Text='<%# Eval("SpotRate") %>'></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Type" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblValidTo" runat="server" Width="70px" Text='<%# Eval("spotRateCategory") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblAproval" runat="server" Width="60px" Text='<%# Eval("Aproval") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Weight" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblweight" runat="server" Width="50px" Text='<%# Eval("Weight") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Commodity" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblcommodity" runat="server" Width="60px" Text='<%# Eval("Commodity") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                     <asp:TemplateField HeaderText="Commodity Description" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblcommodityName" runat="server" Width="180px" Text='<%# Eval("Description") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                <%--    <ItemStyle Wrap="False" />--%>
                </asp:TemplateField>
                
             <asp:TemplateField HeaderText="Spot Rate ID" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblSpotID" runat="server" Width="90px" Text='<%# Eval("SpotRateID") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <%--<ItemStyle Wrap="False" />--%>
                </asp:TemplateField>
                
            
                
           </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>
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
            
        </div>
        
        <div align="right">
           <%-- <asp:Button ID="btnNewBooking" runat="server" CssClass="button" Text="New Booking"
                PostBackUrl="~/ConBooking.aspx" />--%>
            &nbsp;<asp:Button ID="btnApprove" runat="server" CssClass="button" 
                Text="Approve" onclick="btnApprove_Click" /> 
                 &nbsp;<asp:Button ID="btnReject" runat="server" CssClass="button" 
                Text="Reject" onclick="btnReject_Click"/>
            &nbsp;<asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" PostBackUrl="~/Home.aspx" />
        </div>
    </div>
</asp:Content>
