<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="frmProductTypeConfig.aspx.cs" Inherits="ProjectSmartCargoManager.frmProductTypeConfig" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
  
<%--    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>--%>
  
  <div id="contentarea">
   <h1> 
    Product Type Configuration
   </h1>
         
    <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
    ForeColor="Red" Visible="true"></asp:Label>
           
 
    <asp:Panel ID="pnlNew" runat="server">
    <div class="botline">
    <table width="100%" border="0" cellpadding="5px" cellspacing="5px">
        <tr>
            <td>
                From Date *
            </td>
            <td>
                <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="imgFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" PopupButtonID="imgFromDt"
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFromDate">
                </asp:CalendarExtender>
            </td>
            <td>
                To Date *
            </td>
            <td>
                <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                <asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" PopupButtonID="imgToDt"
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtToDate">
                </asp:CalendarExtender>
            </td>
            <td>
                Product Type
            </td>
            <td>
                <asp:DropDownList ID="ddlProductType" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Origin
            </td>
            <td>
                <asp:TextBox ID="txtOriginCode" runat="server"></asp:TextBox>
            </td>
            <td>
                Destination
            </td>
            <td>
                <asp:TextBox ID="txtDestCode" runat="server"></asp:TextBox>
            </td>
            <td>
                Flight #
            </td>
            <td>
                <asp:TextBox ID="txtFlightPrefix" runat="server" Width="45px" MaxLength="3"></asp:TextBox> 
                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" 
                TargetControlID="txtFlightPrefix" WatermarkText="Prefix" />
                &nbsp
                <asp:TextBox ID="txtFlightNumber" runat="server" Width="80px" MaxLength="4"></asp:TextBox>
                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                TargetControlID="txtFlightNumber" WatermarkText="Flt Num" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                Commodity Category
            </td>
            <td>
                <asp:TextBox ID="txtCommodityCategory" runat="server"></asp:TextBox>
            </td>
            <td>
                Commodity Code
            </td>
            <td>
                <asp:TextBox ID="txtCommodityCode" runat="server"></asp:TextBox>
            </td>
            <td>
                Priority
            </td>
            <td>
                <asp:TextBox ID="txtPriority" runat="server" MaxLength="3"></asp:TextBox> 
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" onclick="btnList_Click" />
                &nbsp;
                <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" onclick="btnClear_Click" />
            </td>
        </tr>
    </table>
</div>
    </asp:Panel>  
    <asp:Panel ID="pnlGrid" runat="server">
    <asp:Label ID="lblRecordCount" runat="server" Text="Record 0 of 0" ForeColor="Blue" Font-Bold="True" 
    Font-Size="Large"></asp:Label>
    <div class="ltfloat" style="width:100%;overflow:auto;">
        <asp:GridView ID="grvProductType" runat="server" ShowFooter="true" Width="80%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" 
        onrowcommand="grvProductType_RowCommand"
        onpageindexchanging="grvProductType_PageIndexChanging" >
        <Columns>
         <asp:TemplateField HeaderText="Action">
                 <ItemTemplate>
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="STARTEDIT" CssClass="button" 
                    CommandArgument='<%# Container.DataItemIndex %>'></asp:Button>
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="STARTDELETE" CssClass="button" 
                     CommandArgument='<%# Container.DataItemIndex %>'></asp:Button>
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CommandName="STARTUPDATE" Visible="false" 
                    CssClass="button" CommandArgument='<%# Container.DataItemIndex %>'></asp:Button>
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CommandName="CANCELEDIT" Visible="false" 
                    CssClass="button" CommandArgument='<%# Container.DataItemIndex %>'></asp:Button>
                 </ItemTemplate>
                 <FooterTemplate>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CommandName="SAVE" OnClick="btnSave_Click" CssClass="button" ></asp:Button>
                 </FooterTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             <ItemStyle Wrap="False"></ItemStyle>
             </asp:TemplateField>
            <asp:TemplateField HeaderText="SerialNo" Visible="false">
            <ItemTemplate>
                <asp:Label ID="lblSerialNumber" runat="server" Text='<%#Eval("SerialNumber")%>'></asp:Label>
            </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="true" >
                <ItemTemplate>
                    <asp:TextBox ID="txtOriginCodeRow" runat="server" Enabled="false" Width="50px" MaxLength="3" 
                    Text ='<%#Eval("OriginCode")%>' ToolTip="Keep blank to make Capacity applicable for all Origins."></asp:TextBox>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtOriginCodeHeader" Width="50px" runat="server" MaxLength="3" 
                    ToolTip="Keep blank to make Capacity applicable for all Origins."></asp:TextBox>
                </FooterTemplate>
                <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
            <asp:TemplateField HeaderText="Destination" HeaderStyle-Wrap="true">
                <ItemTemplate>
                    <asp:TextBox ID="txtDestCodeRow" runat="server"  Width="50px" Enabled="false" MaxLength="3" 
                    Text ='<%#Eval("DestCode")%>' ToolTip="Keep blank to make Capacity applicable for all Destinations."></asp:TextBox>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtDestCodeHeader" Width="50px" runat="server" MaxLength="3" 
                    ToolTip="Keep blank to make Capacity applicable for all Destinations."></asp:TextBox>
                </FooterTemplate>
                <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Flight Date" HeaderStyle-Wrap="true">
                <ItemTemplate>
                    <asp:TextBox ID="txtFlightDateRow" runat="server" Enabled="false" Width="100px" MaxLength="10" 
                    Text ='<%#Eval("FlightDate")%>'></asp:TextBox>
                    <asp:ImageButton ID="imgFltDtRow" runat="server" Visible="false" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                    <asp:CalendarExtender ID="txtFlightDateRow_CalendarExtender" runat="server" PopupButtonID="imgFltDt"
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFlightDateRow">
                </asp:CalendarExtender>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtFlightDateHeader" runat="server" Width="100px" Enabled="false" MaxLength="10" ></asp:TextBox>
                    <asp:ImageButton ID="imgFltDtHeader" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                    <asp:CalendarExtender ID="txtFlightDateHeader_CalendarExtender" runat="server" PopupButtonID="imgFltDtHeader"
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFlightDateHeader">
                    </asp:CalendarExtender>
                </FooterTemplate>
                <ItemStyle Wrap="false" ></ItemStyle>
                <HeaderStyle Wrap="True"></HeaderStyle>
                <FooterStyle Wrap="false" ></FooterStyle>
             </asp:TemplateField>    
             
             <asp:TemplateField HeaderText="Flight #" HeaderStyle-Wrap="true">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlFlightPrefixRow" runat="server" Visible="true" Enabled="false"></asp:DropDownList>
                    <asp:TextBox ID="txtFlightNumberRow" Width="80px"  MaxLength="4" runat="server" Enabled="false" 
                    Text ='<%#Eval("FlightNumber")%>'></asp:TextBox>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:DropDownList ID="ddlFlightPrefixHeader" runat="server" Enabled="false"></asp:DropDownList>                
                    <asp:TextBox ID="txtFlightNumberHeader" Width="80px" MaxLength="4" runat="server" Enabled="false"></asp:TextBox>
                </FooterTemplate>
                <HeaderStyle Wrap="True"></HeaderStyle>
                <ItemStyle Wrap="false" ></ItemStyle>
                <FooterStyle Wrap="false" ></FooterStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Product Type *" HeaderStyle-Wrap="true">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlProductTypeRow" Enabled="false" runat="server"></asp:DropDownList>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:DropDownList ID="ddlProductTypeHeader" runat="server"></asp:DropDownList>
                </FooterTemplate>
                <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Commodity Category" HeaderStyle-Wrap="true">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlCommodityCategoryRow" runat="server"  Width="100px" Enabled="false"></asp:DropDownList>
                </ItemTemplate>      
                <FooterTemplate>
                    <asp:DropDownList ID="ddlCommodityCategoryHeader" Width="120px" runat="server"></asp:DropDownList>
                </FooterTemplate>
                <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Commodity Code" HeaderStyle-Wrap="true">
                <ItemTemplate>
                    <asp:TextBox ID="txtCommodityCodeRow" runat="server" Width="80px" MaxLength="10" Text='<%#Eval("CommodityCode")%>' 
                    Enabled="false"></asp:TextBox>
                </ItemTemplate>      
                <FooterTemplate>
                    <asp:TextBox ID="txtCommodityCodeHeader" Width="80px" MaxLength="10" runat="server"></asp:TextBox>
                </FooterTemplate>
                <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="From Date *" HeaderStyle-Wrap="true">
                <ItemTemplate>
                    <asp:TextBox ID="txtFromDateRow" runat="server" Width="100px" Enabled="false" MaxLength="10"  
                    Text ='<%#Eval("FromDate")%>'></asp:TextBox>
                    <asp:ImageButton ID="imgFromDateRow" runat="server" ImageAlign="AbsMiddle" Visible="false"
                     ImageUrl="~/Images/calendar_2.png" />
                    <asp:CalendarExtender ID="txtFromDateRow_CalendarExtender" runat="server" PopupButtonID="imgFromDateRow"
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFromDateRow">
                    </asp:CalendarExtender>
                </ItemTemplate>      
                <FooterTemplate>
                    <asp:TextBox ID="txtFromDateHeader" runat="server" Width="100px" MaxLength="10" ></asp:TextBox>
                    <asp:ImageButton ID="imgFromDateHeader" runat="server" ImageAlign="AbsMiddle" 
                    ImageUrl="~/Images/calendar_2.png" />
                    <asp:CalendarExtender ID="txtFromDateHeader_CalendarExtender" runat="server" PopupButtonID="imgFromDateHeader"
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFromDateHeader">
                    </asp:CalendarExtender>
                </FooterTemplate>
                <ItemStyle Wrap="false" ></ItemStyle>
                <HeaderStyle Wrap="True"></HeaderStyle>
                <FooterStyle Wrap="false" ></FooterStyle>
             </asp:TemplateField>           
             
             <asp:TemplateField HeaderText="To Date *" HeaderStyle-Wrap="true">
                <ItemTemplate>
                    <asp:TextBox ID="txtToDateRow" runat="server" Width="100px" Enabled="false"  MaxLength="10" 
                    Text ='<%#Eval("ToDate")%>'></asp:TextBox>
                    <asp:ImageButton ID="imgToDateRow" runat="server" ImageAlign="AbsMiddle" Visible="false"
                    ImageUrl="~/Images/calendar_2.png" />
                    <asp:CalendarExtender ID="txtToDateRow_CalendarExtender" runat="server" PopupButtonID="imgToDateRow"
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtToDateRow">
                    </asp:CalendarExtender>
                </ItemTemplate>      
                <FooterTemplate>
                    <asp:TextBox ID="txtToDateHeader" runat="server" Width="100px" MaxLength="10" ></asp:TextBox>
                    <asp:ImageButton ID="imgToDateHeader" runat="server" ImageAlign="AbsMiddle" 
                    ImageUrl="~/Images/calendar_2.png" />
                    <asp:CalendarExtender ID="txtToDateHeader_CalendarExtender" runat="server" PopupButtonID="imgToDateHeader"
                        Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtToDateHeader">
                    </asp:CalendarExtender>
                </FooterTemplate>
                <ItemStyle Wrap="false" ></ItemStyle>
                <HeaderStyle Wrap="True"></HeaderStyle>
                <FooterStyle Wrap="false" ></FooterStyle>
             </asp:TemplateField>     
             
             <asp:TemplateField HeaderText="Allocated Capacity (Kg) *" HeaderStyle-Wrap="true">
                <ItemTemplate>
                    <asp:TextBox ID="txtAllocatedCapacityRow" runat="server" Width="80px" Enabled="false" MaxLength="8"  
                    Text ='<%#Eval("AllocatedCapacity")%>'></asp:TextBox>
                </ItemTemplate>      
                <FooterTemplate>
                    <asp:TextBox ID="txtAllocatedCapacityHeader" Width="80px" runat="server" MaxLength="8" ></asp:TextBox>
                </FooterTemplate>
                <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField> 
             <asp:TemplateField HeaderText="Capacity Threshold(%)" HeaderStyle-Wrap="true">
                <ItemTemplate>
                    <asp:TextBox ID="txtCapacityThresholdRow" runat="server" Width="80px" MaxLength="3" Enabled="false" 
                    Text ='<%#Eval("CapacityThreshold")%>'></asp:TextBox>
                </ItemTemplate>      
                <FooterTemplate>
                    <asp:TextBox ID="txtCapacityThresholdHeader" Width="80px" MaxLength="3" runat="server"></asp:TextBox>
                </FooterTemplate>
                <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField> 
                 
             <asp:TemplateField HeaderText="Day Of Week" HeaderStyle-Wrap="true" Visible="false">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlDayOfWeekRow" runat="server" Enabled="false" ></asp:DropDownList>
                </ItemTemplate>      
                <FooterTemplate>
                    <asp:DropDownList ID="ddlDayOfWeekHeader" runat="server" ></asp:DropDownList>
                </FooterTemplate>
                <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>   
             <asp:TemplateField HeaderText="Month" HeaderStyle-Wrap="true" Visible="false">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlMonthRow" runat="server" Enabled="false" ></asp:DropDownList>
                </ItemTemplate>      
                <FooterTemplate>
                    <asp:DropDownList ID="ddlMonthHeader" runat="server" ></asp:DropDownList>
                </FooterTemplate>
                <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>   
              
             <asp:TemplateField HeaderText="Priority" HeaderStyle-Wrap="true" Visible="false">
                <ItemTemplate>
                    <asp:TextBox ID="txtPriorityRow" runat="server" Width="30px" Enabled="false" MaxLength="10" 
                    Text ='<%#Eval("Priority")%>'></asp:TextBox>
                </ItemTemplate>      
                <FooterTemplate>
                    <asp:TextBox ID="txtPriorityHeader" runat="server" Width="30px" MaxLength="10" ></asp:TextBox>
                </FooterTemplate>
                <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
            
            
             <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="true">
                 <ItemTemplate>
                    <asp:CheckBox ID="chkStatusRow" Enabled="false" runat="server" ></asp:CheckBox>
                 </ItemTemplate>
                 <FooterTemplate>
                    <asp:CheckBox ID="chkStatusHeader" runat="server" ></asp:CheckBox>
                 </FooterTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
           
            </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <FooterStyle HorizontalAlign="Center" />
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
    
<%--    </ContentTemplate>
    </asp:UpdatePanel>--%>
  
</asp:Content>
