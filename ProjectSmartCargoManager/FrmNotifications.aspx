<%@ Page Language="C#"  AutoEventWireup="true"  Title="Notifications" CodeBehind="FrmNotifications.aspx.cs"  MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.FrmNotifications"%>

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
    <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
    </div>
    
        <h1>Notifications</h1>
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

<td >From Date</td>
<td >
    <asp:TextBox ID="TxtFrmDt" runat="server" MaxLength="15" Width="80px"></asp:TextBox>
    
    <asp:ImageButton ID="imgFrmDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
    
    <asp:CalendarExtender ID="txtFromDt_CalendarExtender" runat="server" 
        Enabled="True" TargetControlID="TxtFrmDt" PopupButtonID="imgFrmDt" Format="dd/MM/yyyy">
    </asp:CalendarExtender>
</td>



<td>
    To Date
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
    Importance
    </td>
    <td >
    <asp:DropDownList ID="drpImp" runat="server">
        <asp:ListItem Value="0">Select</asp:ListItem>
        <asp:ListItem>Critical</asp:ListItem>
        <asp:ListItem>High</asp:ListItem>
        <asp:ListItem>Maintenance</asp:ListItem>
        <asp:ListItem>Information</asp:ListItem>
    </asp:DropDownList>
    </td>
    
    <td > Station 
    </td>
    <td > 
        <asp:DropDownList ID="DrpStation" runat="server" Width="110px"></asp:DropDownList>
    </td>
    
    <td >User Name</td>
    <td >
     <asp:DropDownList ID="drpUpdatedBy" runat="server" Width="70px"></asp:DropDownList>
    </td>
    
    <td> User Role
    </td>
    <td>
        <asp:DropDownList ID="DrpUserRole" runat="server" Width="100px"></asp:DropDownList>
    </td>
    
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
    
   <h1>Notification Details</h1>
        
    <asp:GridView ID="grdvNotification" runat="server" AutoGenerateColumns="False" Width="100%"
            AutoGenerateEditButton="false" style="margin-top: 0px" HeaderStyle-CssClass="HeaderStyle" 
            RowStyle-CssClass="RowStyle"  AlternatingRowStyle-CssClass="AltRowStyle" 
            PagerStyle-CssClass="PagerStyle" 
            AllowPaging="True"  PageSize="10" 
            onpageindexchanging="grdvNotification_PageIndexChanging">
            
<RowStyle CssClass="RowStyle" HorizontalAlign ="Center" ></RowStyle>
                                 <Columns>
                                 <asp:TemplateField>
                                 
                                 <ItemTemplate>
                                 <asp:CheckBox ID="chkSelect" runat="server"/>
                                 </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Importance">    
                                   <ItemTemplate>    
                                       <asp:DropDownList ID="ddlImportance" runat="server" Width="100px" >
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem>Critical</asp:ListItem>
                                        <asp:ListItem>High</asp:ListItem>
                                        <asp:ListItem>Maintenance</asp:ListItem>
                                        <asp:ListItem>Information</asp:ListItem></asp:DropDownList>  
                                    <%--<asp:TextBox ID="txtIMP" runat="server"  Text = '<%# Eval("Importance") %>' MaxLength="20" Width="100px"></asp:TextBox>--%>
                                   </ItemTemplate>
                                   
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Notified To">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="txtNotiType" runat="server" Text = '<%# Eval("NotificationType") %>' MaxLength="30" Width="130px"></asp:TextBox>
                                    <asp:ImageButton ID="Stations" runat="server" ImageUrl="~/Images/list_bullets.png" ImageAlign="AbsMiddle" OnClientClick="javascript:GetStationCodes(this);return false;"/>
                                   </ItemTemplate>
                                 
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Role">
                                     <ItemTemplate>
                                         <asp:DropDownList ID="ddlRole" runat="server" Width="130px">
                                         <%--<asp:ListItem Value="0">Select</asp:ListItem>--%>
                                         </asp:DropDownList>
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="User">
                                     <ItemTemplate>
                                         <asp:DropDownList ID="ddlUser" runat="server" Width="100px">
                                         <%--<asp:ListItem Value="0">Select</asp:ListItem>--%>
                                         </asp:DropDownList>
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                                                 
                                 <asp:TemplateField HeaderText="From Date">    
                                   <ItemTemplate>    
                                    <%--<asp:TextBox ID="txtFrmDt" runat="server" Text = '<%# Eval("FrmDate","{0:dd/MM/yyyy}") %>' MaxLength="20" Width="100px"></asp:TextBox>--%>
                                    <asp:TextBox ID="txtFrmDt1" runat="server" Text = '<%# Eval("FrmDate") %>'  MaxLength="20" Width="80px"></asp:TextBox>
                                   <asp:CalendarExtender ID="txtFromDtGrd_CalendarExtender" runat="server"  CssClass="insidegridcalender"  Enabled="True" TargetControlID="txtFrmDt1" Format="dd/MM/yyyy" > </asp:CalendarExtender>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="To Date">    
                                   <ItemTemplate>    
                                   <asp:TextBox ID="txtToDt1" runat="server" Text = '<%# Eval("ToDate") %>' MaxLength="20" Width="80px"></asp:TextBox>
                                    <%--<asp:TextBox ID="txtToDt" runat="server" Text = '<%# Eval("ToDate","{0:dd/MM/yyyy}") %>' MaxLength="20" Width="100px"></asp:TextBox>--%>
                                   <asp:CalendarExtender ID="txtToDtGrd_CalendarExtender" runat="server" CssClass="insidegridcalender" Enabled="True" TargetControlID="txtToDt1" Format="dd/MM/yyyy"> </asp:CalendarExtender>
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Subject">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="txtSubject" runat="server" Text = '<%# Eval("Subject1") %>'  Width="100px"></asp:TextBox> 
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Message">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="txtNotiMsg" runat="server" Text = '<%# Eval("NotificationMsg") %>'  Width="100px"></asp:TextBox> 
                                   </ItemTemplate>
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="IsActive">    
                                   <ItemTemplate>   
                                   
                                   <asp:CheckBox ID="chkboxSelect" runat="server"  Checked='<%# Convert.ToBoolean(Eval("IsActive").ToString()) %>'/> 
                                   </ItemTemplate>
                                   
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Sr. No" Visible="false">    
                                   <ItemTemplate>    
                                    <asp:Label ID="txtRowID" runat="server" Text = '<%# Eval("ID") %>'  Width="10px"></asp:Label> 
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
</div>
</div>
    </asp:Content>

