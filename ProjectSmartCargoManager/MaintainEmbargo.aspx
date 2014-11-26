<%@ Page  Title="MaintainEmbargo" Language="C#" AutoEventWireup="true" CodeBehind="MaintainEmbargo.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.MaintainEmbargo" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
        function SelectAll(CheckBoxControl) {

            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox')
        && (document.forms[0].elements[i].name.indexOf('grdStockUpd') > -1)) {
                        document.forms[0].elements[i].checked = true;
                    }
                }
            }
            else {

                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') &&
        (document.forms[0].elements[i].name.indexOf('grdStockUpd') > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }

        function SelectAllGrdEmbargoDetails(headerchk) {
            var gvcheck = document.getElementById("<%=GrdEmbargoDetails.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
    
    </Script> 
    <style type="text/css">
        .style1
        {
            height: 40px;
        }
        .style2
        {
            height: 23px;
        }
        </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
   <div id="contentarea">
     <div class="msg"><asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
           Font-Size="Large" ForeColor="Red"></asp:Label></div>
     <h1> 
     Maintain Embargo
     </h1>
    
     <div class="botline">
        <asp:Panel ID="pnl1" runat="server">
        <table>
        <tr align="center" >
        <td>
         Reference No*
        </td>
        <td>
            <asp:TextBox ID="txtReferenceNo" runat="server"  ToolTip="Please Enter Reference Number For Search"></asp:TextBox>
        </td> 
        <td>
            <asp:Button ID="btnDisplayDetails" runat="server" Text="Display Details" CssClass="button" onclick="btnDisplayDetails_Click" />
        </td>
        <td>
         <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" />
        </td>
        <td> <asp:Button ID="btnDeleteEmbargo" runat="server" Text="Delete Embargo" 
           OnClick="btnDeleteEmbargo_Click" CssClass="button"/>
        </td>
        </tr>
        
        </table> 
       </asp:Panel> 
       
     </div>
    
 
   
   <table width="60%" cellpadding="3" cellspacing="3">
   <tr>
    <td>
     Level
        *</td>
    <td>
        <asp:DropDownList ID="ddlLevel" runat="server">
          <asp:ListItem Text="Warning" Selected="True">
        </asp:ListItem>  
        <asp:ListItem Text="Error"></asp:ListItem>  
        </asp:DropDownList>
    </td>
    <td>
    Status
    </td>
    <td>
        <%--<asp:TextBox ID="txtstatus" runat="server"></asp:TextBox>--%>
         <asp:DropDownList ID="ddlStatus" runat="server">
                                    <asp:ListItem Value="ACT">Active</asp:ListItem>
                                    <asp:ListItem Value="All">All</asp:ListItem>
                                    <asp:ListItem Value="INA">Inactive</asp:ListItem>
                                    <asp:ListItem Value="DRF">Draft</asp:ListItem>
       </asp:DropDownList>
    </td>
    <td>
    </td>
   </tr>
   <tr>
    <td>
    Start Date
        *</td> 
    <td>
        <asp:TextBox ID="txtstartdate" runat="server" Width="85px"></asp:TextBox>
        <asp:CalendarExtender ID="txtstartdate_CalendarExtender" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgFromDate"
            TargetControlID="txtstartdate">
        </asp:CalendarExtender>
                    <asp:ImageButton ID="imgFromDate" runat="server" ImageAlign="AbsMiddle" 
                     ImageUrl="~/Images/calendar_2.png" />
    </td>
    <td>
    End Date
        *</td>
    <td>
        <asp:TextBox ID="txtenddate" runat="server" Width="85px"></asp:TextBox>
        <asp:CalendarExtender ID="txtenddate_CalendarExtender" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgToDate"
            TargetControlID="txtenddate">
        </asp:CalendarExtender>
                    <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" 
                     ImageUrl="~/Images/calendar_2.png" />
    </td>
    <td>
        <asp:CheckBox ID="ChkSuspend" runat="server" Text="Suspend"  />
    </td>
   </tr>
   </table>
  
   <table width="100%">
    <tr>
     <td>
         <fieldset>  <legend style=" font-weight:bold;  color:#000; padding:5px;" xml:lang="">Origin</legend>  
         <table width="100%">
          <tr>
           <td class="style2">
             Origin Type
           </td>
           <td class="style2">
               <asp:DropDownList ID="ddlorigintype" runat="server" onselectedindexchanged="ddlorigintype_SelectedIndexChanged" AutoPostBack="true"  >
                                    <asp:ListItem Text="Select" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Airport" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="City" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                                    <asp:ListItem Text="Country" Value="N"></asp:ListItem>
               </asp:DropDownList>
           </td>
           <td class="style2">
            Origin
           </td>
           <td class="style2">
                <asp:DropDownList ID="ddlOrigin" runat="server" 
                    onselectedindexchanged="ddlOrigin_SelectedIndexChanged">
               </asp:DropDownList>
           </td>
          </tr>
         </table> 
          </fieldset>  
     </td>
     <td>
      <fieldset>  <legend style=" font-weight:bold;  color:#000; padding:5px;" xml:lang="">Destnation</legend>
      <table width="100%">
       <tr>
        <td>
         Destination Type
        </td>
       <td>
           <asp:DropDownList ID="ddldestinationType" runat="server" AutoPostBack="true" 
               onselectedindexchanged="ddldestinationType_SelectedIndexChanged">
               <asp:ListItem Text="Select" Selected="True"></asp:ListItem>
                <asp:ListItem Text="Airport" Value="A"></asp:ListItem>
                <asp:ListItem Text="City" Value="C"></asp:ListItem>
                <asp:ListItem Text="Region" Value="R"></asp:ListItem>
                <asp:ListItem Text="Country" Value="N"></asp:ListItem>
           </asp:DropDownList>
       </td>
       <td>
        Destination
       </td>
       <td>
         <asp:DropDownList ID="ddlDestination" runat="server">
           </asp:DropDownList>
       </td>
       </tr>
       
      </table>
       
     
      </fieldset>  
     </td>
    </tr>
   </table>
   
   <table width="100%">
   <tr>
       <td class="style1">
           Days Of Week
       </td>
       <td>
           <asp:CheckBoxList ID="cblWeekdays" runat="server" RepeatDirection="Horizontal">
               <asp:ListItem Text="Mon" Value="Mon" />
               <asp:ListItem Text="Tue" Value="Tue" />
               <asp:ListItem Text="Wed" Value="Wed" />
               <asp:ListItem Text="Thu" Value="Thu" />
               <asp:ListItem Text="Fri" Value="Fri" />
               <asp:ListItem Text="Sat" Value="Sat" />
               <asp:ListItem Text="Sun" Value="Sun" />
           </asp:CheckBoxList>
       </td>
    <td>
    </td>
    <td>
    </td>
   </tr>
   <tr>
    <td>
     Description
    </td>
    <td>
        <asp:TextBox ID="txtDescription" Width="300px" runat="server" TextMode="MultiLine" ></asp:TextBox>
    </td>
    <td>
     Remarks
    </td>
    <td>
        <asp:TextBox ID="txtremarks" Width="300px" runat="server" TextMode="MultiLine"  ></asp:TextBox>
    </td>
   </tr>
   </table> 
   <strong> Embargo Details</strong>
   <div style="overflow:auto">
            <asp:GridView ID="GrdEmbargoDetails" runat="server"  width="100%"
             AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3"> 
               
                 
                    <Columns>
                     <asp:TemplateField HeaderText="Parameter" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                      <ItemTemplate>
                         <asp:DropDownList ID="ddlParameter" runat="server" OnSelectedIndexChanged="SelectMode" AutoPostBack="true"  >
                          <asp:ListItem Text="Select" Selected="True" ></asp:ListItem>   
                          <asp:ListItem Text="Payment Type"></asp:ListItem> 
                           <asp:ListItem Text="Commodity"></asp:ListItem>  
                           <asp:ListItem Text="Booking"></asp:ListItem> 
                           <asp:ListItem Text="Flight Number"></asp:ListItem> 
                          </asp:DropDownList>
                      </ItemTemplate>

                      <HeaderStyle Wrap="False"></HeaderStyle>
                      <ItemStyle Wrap="False">
                      </ItemStyle>
                      </asp:TemplateField>
                      
                      <asp:TemplateField HeaderText="Applicable" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                      <ItemTemplate>
                          <asp:DropDownList ID="ddlApplicable" runat="server">
                          <%--<asp:ListItem Selected="True" Text="Select"></asp:ListItem> --%>  
                          </asp:DropDownList>
                      </ItemTemplate>

                      <HeaderStyle Wrap="False"></HeaderStyle>
                      <ItemStyle Wrap="False">
                      </ItemStyle>
                      </asp:TemplateField>
                      
                       <asp:TemplateField HeaderText="Values" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                      <ItemTemplate>
                          <asp:TextBox ID="txtvalues" runat="server"></asp:TextBox>
                      </ItemTemplate>

                      <HeaderStyle Wrap="False"></HeaderStyle>
                      <ItemStyle Wrap="False">
                      </ItemStyle>
                      </asp:TemplateField>
                     
                     
                     </Columns> 
                      <HeaderStyle CssClass="titlecolr"/>
                        <RowStyle  HorizontalAlign="Center"/>
                        <AlternatingRowStyle  HorizontalAlign="Center"/>
           </asp:GridView>
   </div>  
   
   <div id="fotbut" >
   <table>
   <tr>
   <td style="width:auto;"  align="left">
    <%--  <asp:Button ID="btnAdd" runat="server" AutoPostBack="true" CssClass="button" Text="Add "
            ValidationGroup="Gen" onclick="btnAdd_Click" />--%>
    
     <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" onclick="btnSave_Click" />

    <%-- <asp:Button ID="btnDelete" runat="server" AutoPostBack="true" 
           CssClass="button" Text="Delete"--%>
   <%-- ValidationGroup="Gen" onclick="btnDelete_Click"/> --%>
  <%--New List Button added--%>
  <asp:Button ID="btnList" runat="server" Text="List" OnClick="btnList_Click" Visible="false" CssClass="button"/>
  </td> 
  </tr> 
  </table> 
   
  <asp:GridView ID="GrdListEmbergo" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
  OnPageIndexChanging="GrdListEmbergo_PageIndexChanging"
  OnRowCommand="GrdListEmbergo_RowCommand" HeaderStyle-CssClass="titlecolr" Width="100%">
  
  <Columns>
<asp:TemplateField HeaderText="Serial Number" Visible="false" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblSrNo" runat="server" Text='<%#Eval("SerialNumber")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Reference Number" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblRefNo" runat="server" Text='<%#Eval("ReferenceNumber")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Status" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Start Date" Visible="false" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblStartDt" runat="server" Text='<%#Eval("startDate")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="End Date" Visible="false" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblEndDt" runat="server" Text='<%#Eval("EndDate")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Origin Type" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblOrgType" runat="server" Text='<%#Eval("OriginType")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Origin" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblOrg" runat="server" Text='<%#Eval("Origin")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Destination Type" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblDestType" runat="server" Text='<%#Eval("DestinationType")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Destination" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblDest" runat="server" Text='<%#Eval("Destination")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Description" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblDescrp" runat="server" Text='<%#Eval("Discription")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Remarks" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblRemark" runat="server" Text='<%#Eval("Remarks")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Suspend" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblSusp" runat="server" Text='<%#Eval("Suspend")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Parameter" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblParam" runat="server" Text='<%#Eval("Parameter")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Applicable" Visible="false" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblApplic" runat="server" Text='<%#Eval("Applicable")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Value" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblValue" runat="server" Text='<%#Eval("Value")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Days Of Week" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblDays" runat="server" Text='<%#Eval("DaysOfWeek")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

</Columns>
  </asp:GridView>
   
   </div>
   </div>
   
   </asp:Content>

