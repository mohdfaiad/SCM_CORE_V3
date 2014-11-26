<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListFrmMessageConfiguration.aspx.cs" Inherits="ProjectSmartCargoManager.ListFrmMessageConfiguration"  MasterPageFile="~/SmartCargoMaster.Master"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript">
     function ConfirmDelete() {
         if (confirm("Are you sure to want to Delete?") == true)
             return true;
         else
             return false;
     }
    </script>
    <style type="text/css">
        .style2
        {
            height: 26px;
        }
    </style>
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
     
    <div id="contentarea">
        <h1>
            List
            Message Configuration Details
        </h1>
        <asp:UpdatePanel ID="UPFourth" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Large"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        <div class="botline">
            <asp:UpdatePanel ID="UPFirst" runat="server">
                <ContentTemplate>
                    <table style="width: 100%; height: 100%" border="0">
                        <tr>
                            <td class="style2">
                                Origin
                            </td>
                            <td class="style2">
                              <%-- <asp:TextBox ID="txtOrigin" runat="server"></asp:TextBox>
                                   <asp:TextBoxWatermarkExtender ID="txtOrigin_TextBoxWatermarkExtender1" WatermarkText="Origin Code" runat="server" TargetControlID="txtOrigin">
                                   </asp:TextBoxWatermarkExtender>
                                   <asp:AutoCompleteExtender ID="txtOrigin_AutoCompleteExtender1" runat="server" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True"  ServicePath="~/Home.aspx" TargetControlID="txtOrigin">
                                   </asp:AutoCompleteExtender> --%>
                                <asp:DropDownList ID="ddlOrigin" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td class="style2">
                                Destination
                            </td>
                            <td class="style2">  <asp:DropDownList ID="ddlDestination" runat="server">
                                </asp:DropDownList>
                                
                                  <%--  <asp:TextBox ID="txtDestination" runat="server"></asp:TextBox>
                                  <asp:TextBoxWatermarkExtender ID="txtDestination_TextBoxWatermarkExtender" WatermarkText="Destination Code" runat="server" TargetControlID="txtDestination">
                                   </asp:TextBoxWatermarkExtender>
                                   <asp:AutoCompleteExtender ID="txtDestination_AutoCompleteExtender" runat="server" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True"  ServicePath="~/Home.aspx" TargetControlID="txtDestination">
                                   </asp:AutoCompleteExtender>--%>
                            </td>
                            <td class="style2">
                                Flight#</td>
                              <td class="style2">
                                  <asp:TextBox ID="txtFltNo" runat="server" Width="100px"></asp:TextBox>
                                  <asp:TextBoxWatermarkExtender ID="txtFltNo_TextBoxWatermarkExtender" 
                                      runat="server" TargetControlID="txtFltNo" WatermarkText="Enter Flight Number">
                                  </asp:TextBoxWatermarkExtender>
                                  <asp:AutoCompleteExtender ID="txtFltNo_AutoCompleteExtender" runat="server" 
                                      Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetFlight" 
                                      ServicePath="~/ArrivalReassign.aspx" TargetControlID="txtFltNo">
                                  </asp:AutoCompleteExtender>
                              </td>
                            
                        </tr>
                        <tr>
      <td class="style2">
          <%-- <asp:TextBox ID="txtTransitDest" runat="server"></asp:TextBox>
                               <asp:TextBoxWatermarkExtender ID="txtTransitDest_TextBoxWatermarkExtender" WatermarkText="Transit Destination Code" runat="server" TargetControlID="txtTransitDest">
                                   </asp:TextBoxWatermarkExtender>
                                   <asp:AutoCompleteExtender ID="txtTransitDest_AutoCompleteExtender" runat="server" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True"  ServicePath="~/Home.aspx" TargetControlID="txtTransitDest">
                                   </asp:AutoCompleteExtender>--%>Transit Destination</td>
                        <td class="style2">
                            <asp:DropDownList ID="ddlTransitDestination" runat="server">
                            </asp:DropDownList>
                            </td>
       <td class="style2">
           Message Type</td>
       <td class="style2">
           <asp:DropDownList ID="ddlMessageType" runat="server" Width="120px">
           </asp:DropDownList>
       </td>
       <td class="style2">
                            </td>
       <td class="style2">
                            </td>
        </tr>
                        
                        <tr>
                            <td class="style2">
                                Partner Type</td>
                            <td class="style2">
                                <asp:DropDownList ID="ddlPartnerType" runat="server" 
                                    AutoPostBack="true" 
                                    onselectedindexchanged="ddlPartnerType_SelectedIndexChanged" Width="120px">
                                    <asp:ListItem Selected="True" Text="Select"></asp:ListItem>
                                    <asp:ListItem Text="Agent"></asp:ListItem>
                                    <asp:ListItem Text="Airline"></asp:ListItem>
                                    <asp:ListItem Text="GHA"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="style2">
                                Partner Code</td>
                            <td class="style2">
                                <asp:DropDownList ID="ddlPartnerCode" runat="server" Width="120px">
                                    <asp:ListItem Selected="True" Text="Select"></asp:ListItem>
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="txtPartnerCode" runat="server" ></asp:TextBox>
           <asp:TextBoxWatermarkExtender ID="txtPartnerCode_TextBoxWatermarkExtender" 
             runat="server" TargetControlID="txtPartnerCode" WatermarkText="Partner Code.." >
           </asp:TextBoxWatermarkExtender>
            <asp:AutoCompleteExtender ID="txtPartnerCode_AutoCompleteExtender" runat="server" 
             TargetControlID="txtPartnerCode" ServiceMethod="GetPartner" MinimumPrefixLength="1"  Enabled="True"  
             EnableCaching="true">
            </asp:AutoCompleteExtender>--%>&nbsp;</td>
                            <td class="style2">
                                </td>
                            <td class="style2">
                                </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <asp:Button ID="btnList" runat="server" CssClass="button" 
                                    onclick="btnList_Click" Text="List" />
                            </td>
                        </tr>
                        
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnList" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <br />
        <h2>
            Message Configuration Details
        </h2>
        <asp:UpdatePanel ID="UPThird" runat="server">
            <ContentTemplate>
                <asp:Label ID="LBLNoOfRecords" runat="server"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="divback">
            <asp:UpdatePanel ID="UPSecond" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdMsgList" runat="server" ShowFooter="True" OnRowCommand="grdMsgList_RowCommand" OnPageIndexChanging="grdMsgList_PageIndexChanging"
                        Width="100%" Height="82px" AllowPaging="true" PageSize="15" AutoGenerateColumns="False">
                        <Columns>
                        <asp:BoundField HeaderText="Sr No" DataField="Srno"/>
                        <asp:BoundField HeaderText="Message Type" DataField="MsgType" />
                        <asp:BoundField HeaderText="Message Comm Type" DataField="MsgCommType" />
                        <asp:BoundField HeaderText="Partner Type" DataField="PartnerType" />
                        <asp:BoundField HeaderText="Partner Code" DataField="PartnerCode" />
                        <asp:BoundField HeaderText="Origin" DataField="Origin" />
                        <asp:BoundField HeaderText="Destination" DataField="Destination" />
                        <asp:BoundField HeaderText="Flight No" DataField="FlightNumber" />
                        <asp:BoundField HeaderText="Email id" DataField="EmailID" />
                        <asp:BoundField HeaderText="SITA Id" DataField="SITAID" />
                        <asp:BoundField HeaderText="FTP Id" DataField="FTPID" />
                        <asp:BoundField HeaderText="Transit Dest" DataField="TransitDestination" />
                        <asp:ButtonField CommandName="Edit" Text="Edit" />
                        <asp:ButtonField CommandName="View" Text="View" />
                        <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDel" Text="Delete" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delete" OnClientClick="return ConfirmDelete();" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="titlecolr" />
                        <RowStyle HorizontalAlign="Center" />
                        <AlternatingRowStyle HorizontalAlign="Center" />
                    </asp:GridView>
                </ContentTemplate>
               
            </asp:UpdatePanel>
        </div>
        <div align="right">
            <asp:Button ID="btnNewBooking" runat="server" CssClass="button" Text="New Message"
                PostBackUrl="~/FrmMessageConfiguration.aspx" />
            <%--&nbsp;<asp:Button ID="btnPrint" runat="server" CssClass="button" Text="Print" />--%>
            &nbsp;<asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" PostBackUrl="~/Home.aspx" />
        </div>
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
    
</asp:Content>
