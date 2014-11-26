<%@ Page Language="C#"  AutoEventWireup="true"  Title="IAC Master" CodeBehind="IACMaster.aspx.cs"  MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.IACMaster"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1"  ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">

    function GetStationCodes(mybutton) 
 {
     var strValue = mybutton.value;
     var row = mybutton.parentNode.parentNode;
     var rowIndex = row.rowIndex - 1;


     var TxtClientObject = mybutton.id.replace("Stations", "txtNotiType");
     var TxtClientValue = TxtClientObject.value;

     //window.open('ListMulipleSelectTransitStation.aspx?TargetTXT=' + TxtClientObject + '&txtValue=' + TxtClientValue, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=350,height=300,toolbar=0,resizable=0');
     window.open('ListMultipleSelect.aspx?Parent=MaintainRatesORG&level=A&param=Origin&TargetTXT=' + TxtClientObject + '&Values=' + TxtClientValue, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
 }


</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
    
    <div class="msg">
    <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Large"></asp:Label>
    </div>
    
        <h1>IAC Master</h1>
   <%--<div class="botline">--%>
<div class="botline">
    
    <table width="100%">
<tr>
<td colspan="2">
    
    </td>
    
</tr>

<tr>
<td>
<table cellpadding="3" cellspacing="3">

<tr>

<td >From Date*</td>
<td >
    <asp:TextBox ID="TxtFrmDt" runat="server" MaxLength="15" Width="80px"></asp:TextBox>
    
    <asp:ImageButton ID="imgFrmDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
    
    <asp:CalendarExtender ID="txtFromDt_CalendarExtender" runat="server" 
        Enabled="True" TargetControlID="TxtFrmDt" PopupButtonID="imgFrmDt" Format="dd/MM/yyyy">
    </asp:CalendarExtender>
</td>



<td>
    To Date*
</td>
<td>
    <asp:TextBox ID="txtToDt" runat="server" MaxLength="15" Width="80px"></asp:TextBox>
    <asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
    
    <asp:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtToDt" PopupButtonID="imgToDt" Format="dd/MM/yyyy"> </asp:CalendarExtender>
    </td>
 <%--<td >
    Notification Type
    </td>
 
     <td >
        <asp:DropDownList ID="drpNotiType" runat="server">
        <asp:ListItem Selected="True">Select</asp:ListItem>
        <asp:ListItem>Global</asp:ListItem>
        <asp:ListItem>Regional</asp:ListItem>
        </asp:DropDownList>
    </td>
    --%>
    <td >
        IAC Code
    </td>
    <td >
    <asp:TextBox ID="txtIACCode" runat="server"></asp:TextBox>
    </td>
    
    <td > Approval Number 
    </td>
    <td > 
    <asp:TextBox ID="txtApprovalNo" runat="server"></asp:TextBox>
    </td>
    
    <td >&nbsp;</td>
    <td >
        &nbsp;</td>
    
    <td> &nbsp;</td>
    <td>
        &nbsp;</td>
    
</tr>

<tr>
<td colspan=2>
       <asp:Button ID="btnList" runat="server" CssClass = "button" Text="List" 
           onclick="btnList_Click"/> &nbsp;&nbsp;
       <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
           onclick="btnClear_Click"/>
    </td>
</tr>

</table>
</td>
    
</tr>
</table>
    
    </div>
    
    <div>
    
   <h1>&nbsp;</h1>
        
    <asp:GridView ID="grvIACMaster" runat="server" AutoGenerateColumns="False" Width="100%"
            AutoGenerateEditButton="false" style="margin-top: 0px" HeaderStyle-CssClass="HeaderStyle" 
            RowStyle-CssClass="RowStyle"  AlternatingRowStyle-CssClass="AltRowStyle" 
            PagerStyle-CssClass="PagerStyle" 
            AllowPaging="True"  PageSize="10" 
            onpageindexchanging="grvIACMaster_PageIndexChanging">
            
<RowStyle CssClass="RowStyle" HorizontalAlign ="Center" ></RowStyle>
                                 <Columns>
                                 <asp:TemplateField>
                                 
                                 <ItemTemplate>
                                 <asp:CheckBox ID="chkSelect" runat="server"/>
                                 </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                
                                 <asp:TemplateField HeaderText="Customer Code">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="txtIACCode" runat="server" Text = '<%# Eval("IACCode") %>'  Width="100px"></asp:TextBox> 
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                  
                                 <asp:TemplateField HeaderText="Customer Name">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="txtIACName" runat="server" Text = '<%# Eval("IACName") %>'  Width="100px"></asp:TextBox> 
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 
                                 <asp:TemplateField HeaderText="Approval Number">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="txtApprovalNo" runat="server" Text = '<%# Eval("ApprovalNo") %>'  Width="100px"></asp:TextBox> 
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                               
                                 <asp:TemplateField HeaderText="City">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="txtCity" runat="server" Text = '<%# Eval("City") %>'  Width="100px"></asp:TextBox> 
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                                                 
                              
                               
                                 <asp:TemplateField HeaderText="State">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="txtState" runat="server" Text = '<%# Eval("State") %>'  Width="100px"></asp:TextBox> 
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Country">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="txtCountry" runat="server" Text = '<%# Eval("Country") %>'  Width="100px"></asp:TextBox> 
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Zip">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="txtZip" runat="server" Text = '<%# Eval("Zip") %>'  Width="100px"></asp:TextBox> 
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                    <asp:TemplateField HeaderText="Exp. Date">    
                                   <ItemTemplate>    
                                    <%--<asp:TextBox ID="txtFrmDt" runat="server" Text = '<%# Eval("FrmDate","{0:dd/MM/yyyy}") %>' MaxLength="20" Width="100px"></asp:TextBox>--%>
                                    <asp:TextBox ID="txtExpDate" runat="server" Text = '<%# Eval("ExpirationDt") %>'  MaxLength="20" Width="80px"></asp:TextBox>
    <asp:ImageButton ID="imgExpDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                                 
                                   <asp:CalendarExtender ID="txtFromDtGrd_CalendarExtender" runat="server"  CssClass="insidegridcalender"  Enabled="True" TargetControlID="txtExpDate" Format="dd/MM/yyyy"  PopupButtonID="imgExpDate"> </asp:CalendarExtender>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="IsActive">    
                                   <ItemTemplate>   
                                   
                                   <asp:CheckBox ID="chkboxSelect" runat="server"  Checked='<%# Convert.ToBoolean(Eval("IsActive").ToString()) %>'/> 
                                   </ItemTemplate>
                                   
                                 </asp:TemplateField>
                        
                                 
                                 
                                 </Columns>

<PagerStyle CssClass="PagerStyle"></PagerStyle>

<HeaderStyle CssClass="HeaderStyle"></HeaderStyle>

<AlternatingRowStyle CssClass="AltRowStyle"></AlternatingRowStyle>
</asp:GridView>


        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" 
            onclick="btnAdd_Click" />
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
            onclick="btnSave_Click" />
        <%--<asp:Button ID="btnDisable" runat="server" Text="Disable" CssClass="button" />--%>
        <asp:Button ID="btnDelete" runat="server" CssClass="button" 
            onclick="btnDelete_Click" Text="Delete" />
</div>
</div>
    </asp:Content>

