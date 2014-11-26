<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDailySchedule.aspx.cs"  MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmDailySchedule" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
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
    </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>

     <div id="contentarea">
       <h1>
          <%--  <img alt="" src="Images/txtdailyflg.png"  style="vertical-align:5"/>--%>
          Daily Flight Movement
             </h1>
   <div >
   <table>
  
      <tr>
        <td>
                            Origin
                            </td> 
                            <td>
                                <asp:DropDownList ID="ddlOrg" runat="server">
                                </asp:DropDownList>
                            </td> 
                            <td>
                            Destination
                            </td>  
                            <td>
                             <asp:DropDownList ID="ddlDest" runat="server" AutoPostBack="true" 
                                    onselectedindexchanged="ddlDest_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
   <td> Flight #
   </td>
   <td>
       <asp:DropDownList ID="ddlFlight" runat="server" 
                            onselectedindexchanged="ddlFlight_SelectedIndexChanged" AppendDataBoundItems="True">
              </asp:DropDownList>
   </td>
   <td>
     Flight Date
   </td>
   <td>
       <asp:TextBox ID="txtFlightDate" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
       
   <asp:CalendarExtender ID="txtFlightDate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightDate" PopupButtonID="imgToDate">
              </asp:CalendarExtender>
              <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
   </td>
   <td>
     <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click" />
   </td>
   </tr>
   <tr>
   <td colspan="5">
        
            <asp:Label ID="lblStatus" runat="server" BackColor="White" 
           Font-Bold="True" Font-Size="Medium"
                ForeColor="Red"></asp:Label>
        
   </td>
   </tr>
  
   </table>
   
   </div> 
   
   <asp:Panel ID ="PnlGrid" runat="server" Visible="false">
   
    <div>
   
    <asp:GridView ID="grdScheduleinfo" runat="server" AutoGenerateColumns="False" 
                                    ShowFooter="True"   Width="100%" >
            <Columns>
               
               <asp:TemplateField Visible="false">
                     <ItemTemplate  > 
                                                        <asp:Label ID="ScheduleID" runat="server" Visible="false"  Text ='<%# Eval("ScheduleID") %>'/>
                                                    </ItemTemplate>
               </asp:TemplateField>
             
                    <asp:TemplateField HeaderText="From" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                      <asp:DropDownList ID="ddlFromOrigin" runat="server" Width="45px">
                        </asp:DropDownList>
                     <%-- <asp:TextBox  ID="txtSource" runat="server" Width="70px" Enabled="false" ></asp:TextBox>--%>
                       
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                    <asp:TemplateField HeaderText="To" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                       
                         <asp:DropDownList ID="ddlToDest" runat="server" Width="45px">
                        </asp:DropDownList>
                        
                         <%--<asp:TextBox  ID="txtDest" runat="server" Width="70px" Enabled="false" ></asp:TextBox>
                    --%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
   
                    <asp:TemplateField HeaderText="STD" HeaderStyle-Wrap="false" 
                    ItemStyle-Width="10%">
                    <ItemTemplate>
                    
                                           
                        Hr <asp:TextBox ID="txtDeptTimeHr" runat="server" Width="20px" Enabled="false"  DataTextField=""></asp:TextBox>
                       
                        <%-- <asp:NumericUpDownExtender ID="txtDeptTimeHr_NumericUpDownExtender" runat="server" 
                TargetControlID="txtDeptTimeHr"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "0"  Maximum = "23" />--%> 
     :
                       
                         Min <asp:TextBox ID="txtDeptTimeMin" runat="server" Width="20px" Enabled="false" DataTextField=""></asp:TextBox>
                       
                         <%--<asp:NumericUpDownExtender ID="txtDeptTimeMin_NumericUpDownExtender1" runat="server" 
                TargetControlID="txtDeptTimeMin"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "0"  Maximum = "60" /> 
      --%>
      
                    </ItemTemplate>
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="ATD *" HeaderStyle-Wrap="false" 
                    ItemStyle-Width="10%">
                    <ItemTemplate>
                    
                                           
                        Hr <asp:TextBox ID="txtADTDeptTimeHr" runat="server" Width="8px"  DataTextField=""></asp:TextBox>
                       
                         <asp:NumericUpDownExtender ID="txtADTDeptTimeHr_NumericUpDownExtender" runat="server" 
                TargetControlID="txtADTDeptTimeHr"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "0"  Maximum = "23" /> :
                       
                         Min <asp:TextBox ID="txtADTDeptTimeMin" runat="server" Width="8px"  DataTextField=""></asp:TextBox>
                       
                         <asp:NumericUpDownExtender ID="txtADTDeptTimeMin_NumericUpDownExtender1" runat="server" 
                TargetControlID="txtADTDeptTimeMin"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "0"  Maximum = "60" /> 
      
      
                    </ItemTemplate>
                    <HeaderStyle Wrap="False"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                
                
                
                    <asp:TemplateField HeaderText="STA " HeaderStyle-Wrap="false"   
                    ItemStyle-Width=10%>
                    <ItemTemplate >  
                                      
                    
                      Hr<asp:TextBox  ID="txtArrivaltimeHr" runat="server" Enabled="false" Width="20px" ></asp:TextBox>
                      
                    <%--    <asp:NumericUpDownExtender ID="txtArrivaltimeHr_NumericUpDownExtender" runat="server" 
                TargetControlID="txtArrivaltimeHr"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "0"  Maximum = "23" />--%> 
     :  
     
                      Min<asp:TextBox  ID="txtArrivalTimeMin" runat="server" Enabled="false" Width="20px" ></asp:TextBox>
                    
                      <%-- <asp:NumericUpDownExtender ID="txtArrivalTimeMin_NumericUpDownExtender1" runat="server" 
                TargetControlID="txtArrivalTimeMin"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "0"  Maximum = "60" /> --%>
      
      
                       <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtbankguranteeamt"  ErrorMessage="Please Enter Numeric Value"  ValidationExpression="^\d+$" ValidationGroup="check">
                        </asp:RegularExpressionValidator>--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="ATA *" HeaderStyle-Wrap="false"   
                    ItemStyle-Width=10%>
                    <ItemTemplate >  
                                      
                    
                      Hr<asp:TextBox  ID="txtATAArrivaltimeHr" runat="server" Width="8px" ></asp:TextBox>
                      
                        <asp:NumericUpDownExtender ID="txtATAArrivaltimeHr_NumericUpDownExtender" runat="server" 
                TargetControlID="txtATAArrivaltimeHr"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "0"  Maximum = "23" /> :  
     
                      Min<asp:TextBox  ID="txtATAArrivalTimeMin" runat="server" Width="8px" ></asp:TextBox>
                    
                       <asp:NumericUpDownExtender ID="txtATAArrivalTimeMin_NumericUpDownExtender1" runat="server" 
                TargetControlID="txtATAArrivalTimeMin"  Width="40" RefValues=""   ServiceDownMethod=""  ServiceUpMethod=""  TargetButtonDownID=""  
     TargetButtonUpID=""   Minimum = "0"  Maximum = "60" /> 
      
      
                       <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtbankguranteeamt"  ErrorMessage="Please Enter Numeric Value"  ValidationExpression="^\d+$" ValidationGroup="check">
                        </asp:RegularExpressionValidator>--%>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                          
                    <asp:TemplateField HeaderText="AirCraft Type*" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=20%>
                   
                    <ItemTemplate>
                        
                    <asp:DropDownList ID="ddlAirCraft" runat="server" 
                     AutoPostBack="True" >
              </asp:DropDownList>
                   
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
               
                   
                    <asp:TemplateField HeaderText="Registraion No" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=15%>
                   
                    <ItemTemplate>
                        <asp:TextBox ID="txtRegistration" Width="100px" Text="" MaxLength="60" runat="server" ></asp:TextBox>
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                   
                   
                   
                    <asp:TemplateField HeaderText="Remark *" HeaderStyle-Wrap="false" 
                    ItemStyle-Width=20%>
                   
                    <ItemTemplate>
                        <asp:TextBox ID="txtRemark" Width="100px" Text="" MaxLength="60" runat="server" ></asp:TextBox>
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Right" />
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
               
                    
               
           </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>
     
    
    </div>
    
    <div id="fotbut">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
            onclick="btnSave_Click"  />
           
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
    
    </ContentTemplate>
    </asp:UpdatePanel>
    
     </asp:Content>