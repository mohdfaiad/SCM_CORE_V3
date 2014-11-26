<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDemCharges.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmDemCharges" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript">

        function ValidateModuleList(source, args) {
            var chkListModules = document.getElementById('<%= chkDays.ClientID %>');
            var chkListinputs = chkListModules.getElementsByTagName("input");
            for (var i = 0; i < chkListinputs.length; i++) {
                if (chkListinputs[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }
    
function iterateListControl() {
    var containerId = '<%= chkDays.ClientID %>';
 var containerRef = document.getElementById(containerId);
 var inputRefArray = containerRef.getElementsByTagName('input');

 for (var i=0; i<inputRefArray.length; i++) {
  var inputRef = inputRefArray[i];
  if ( inputRef.type.substr(0, 8) == 'checkbox' )
  {
   if ( inputRef.checked == true )
    alert('#' + i + ' (' + inputRef.id + ') is checked');
  }
 }
}
// -->
</script>
<style>
.forUper
{
    text-transform:uppercase;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
        <h1 style="font-size:larger;">
            &nbsp;Demurrage Charges
        </h1>
          <br />
         <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
        <br />
         <div class="divback" id="chargeList" runat="server" style="width: 1050px;">
             <asp:GridView ID="GVDemChargeList" runat="server" AutoGenerateColumns="False">
             <Columns>
             <asp:TemplateField HeaderText="Charge Code">
             <ItemTemplate>
                <asp:LinkButton ID="HLChargeCode" runat="server" Text='<%# Eval("Type") %>' OnClick="getValuesForEdit"></asp:LinkButton>
             
           <%--  <asp:HyperLink ID="HLChargeCode" runat="server" Text='<%# Eval("Type") %>'   ></asp:HyperLink>--%>
             </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Location">
             <ItemTemplate>
             <asp:Label ID="HLLoc" runat="server" Text='<%# Eval("Location") %>'></asp:Label>
             </ItemTemplate>
             </asp:TemplateField>
             </Columns>
             </asp:GridView>
         </div>
        
        <div class="divback" style="width: 1050px;margin-top:10px;" id="chargeNew" runat="server">
          <%--  <table width="100%">
               <tr>
               <td style="width:10%;">Year</td>
               <td>
                   <asp:CheckBoxList ID="cblYear" runat="server" RepeatDirection="Horizontal" 
                       RepeatLayout="Flow">
                   </asp:CheckBoxList>
                   </td>
               <td style="width:10%;">Month</td>
               <td>
                   <asp:CheckBoxList ID="cblMonth" runat="server" RepeatDirection="Horizontal" 
                       RepeatLayout="Flow">
                       <asp:ListItem>ALL</asp:ListItem>
                       <asp:ListItem>JAN</asp:ListItem>
                       <asp:ListItem>FEB</asp:ListItem>
                       <asp:ListItem>MAR</asp:ListItem>
                       <asp:ListItem>APR</asp:ListItem>
                       <asp:ListItem>MAY</asp:ListItem>
                       <asp:ListItem>JUN</asp:ListItem>
                       <asp:ListItem>JUL</asp:ListItem>
                       <asp:ListItem>AUG</asp:ListItem>
                       <asp:ListItem>SEP</asp:ListItem>
                       <asp:ListItem>OCT</asp:ListItem>
                       <asp:ListItem>NOV</asp:ListItem>
                       <asp:ListItem>DEC</asp:ListItem>
                   </asp:CheckBoxList>
                   </td>
               
               </tr>
               <tr>
               <td style="width:10%;">Exclusion Type</td>
               <td>
                   <asp:DropDownList ID="ddlExcType" runat="server" onchange="chkExcType()">
                       <asp:ListItem>Date</asp:ListItem>
                       <asp:ListItem Selected="True">Day</asp:ListItem>
                   </asp:DropDownList>
                   </td>
               <td style="width:10%;">
                   <asp:Label ID="lblExcTypeSel" runat="server" Text="Day"></asp:Label>
                   </td>
               <td>
               <div style="clear:both;">
                   <asp:RadioButtonList ID="rblWise" runat="server" 
                       RepeatDirection="Horizontal" RepeatLayout="Flow">
                       <asp:ListItem Selected="True">Day</asp:ListItem>
                       <asp:ListItem>Week</asp:ListItem>
                   </asp:RadioButtonList>
                   
                   <asp:CheckBoxList ID="cblWeek" runat="server" 
                       RepeatDirection="Horizontal" RepeatLayout="Flow">
                       <asp:ListItem>ALL</asp:ListItem>
                       <asp:ListItem>1</asp:ListItem>
                       <asp:ListItem>2</asp:ListItem>
                       <asp:ListItem>3</asp:ListItem>
                       <asp:ListItem>4</asp:ListItem>
                       <asp:ListItem>5</asp:ListItem>
                   </asp:CheckBoxList>
                   <asp:CheckBoxList ID="cblDay" runat="server" RepeatDirection="Horizontal" 
                       RepeatLayout="Flow">
                       <asp:ListItem>ALL</asp:ListItem>
                       <asp:ListItem>MON</asp:ListItem>
                       <asp:ListItem>TUE</asp:ListItem>
                       <asp:ListItem>WED</asp:ListItem>
                       <asp:ListItem>THR</asp:ListItem>
                       <asp:ListItem>FRI</asp:ListItem>
                       <asp:ListItem>SAT</asp:ListItem>
                       <asp:ListItem>SUN</asp:ListItem>
                   </asp:CheckBoxList>
                   </div>

                   </td>
               
               </tr>
               </table>
                 <div id="divdate">
               <asp:CheckBoxList ID="cblDate" runat="server" RepeatDirection="Horizontal" 
                       RepeatLayout="Flow">
                       <asp:ListItem>ALL</asp:ListItem>
                       <asp:ListItem>1</asp:ListItem>
                       <asp:ListItem>2</asp:ListItem>
                       <asp:ListItem>3</asp:ListItem>
                       <asp:ListItem>4</asp:ListItem>
                       <asp:ListItem>5</asp:ListItem>
                       <asp:ListItem>6</asp:ListItem>
                       <asp:ListItem>7</asp:ListItem>
                       <asp:ListItem>8</asp:ListItem>
                       <asp:ListItem>9</asp:ListItem>
                       <asp:ListItem>10</asp:ListItem>
                       <asp:ListItem>11</asp:ListItem>
                       <asp:ListItem>12</asp:ListItem>
                       <asp:ListItem>13</asp:ListItem>
                       <asp:ListItem>14</asp:ListItem>
                       <asp:ListItem>15</asp:ListItem>
                       <asp:ListItem>16</asp:ListItem>
                       <asp:ListItem>17</asp:ListItem>
                       <asp:ListItem>18</asp:ListItem>
                       <asp:ListItem>19</asp:ListItem>
                       <asp:ListItem>20</asp:ListItem>
                       <asp:ListItem>21</asp:ListItem>
                       <asp:ListItem>22</asp:ListItem>
                       <asp:ListItem>23</asp:ListItem>
                       <asp:ListItem>24</asp:ListItem>
                       <asp:ListItem>25</asp:ListItem>
                       <asp:ListItem>26</asp:ListItem>
                       <asp:ListItem>27</asp:ListItem>
                       <asp:ListItem>28</asp:ListItem>
                       <asp:ListItem>29</asp:ListItem>
                       <asp:ListItem>30</asp:ListItem>
                       <asp:ListItem>31</asp:ListItem>
                   </asp:CheckBoxList>
                   
                   </div>--%>
                   <div>
          <%-- Date
           <asp:TextBox ID="txtDate" runat="server" Width="100px"></asp:TextBox>
           <asp:CalendarExtender ID="caltxtDate" runat="server" TargetControlID="txtDate" Format="dd/MM/yyyy"></asp:CalendarExtender> --%>
           
              <table width="100%" style="margin-top:15px;" cellpadding="3" cellspacing="3">
               <tr>
               <td >Charge Code</td>
               <td >
                   <asp:TextBox ID="txtChrCode" runat="server" MaxLength="15" Width="125px" CssClass="forUper" ></asp:TextBox>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                       ControlToValidate="txtChrCode" ErrorMessage="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                   </td>
               <td >
                   Applicable On
                
                   </td>
                               
               <td colspan="2" >
                    <asp:CheckBoxList ID="chkDays" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ><%--onChange="iterateListControl();--%>
                   <asp:ListItem>MON</asp:ListItem>
                   <asp:ListItem>TUE</asp:ListItem>
                   <asp:ListItem>WED</asp:ListItem>
                   <asp:ListItem>THR</asp:ListItem>
                   <asp:ListItem>FRI</asp:ListItem>
                   <asp:ListItem>SAT</asp:ListItem>
                   <asp:ListItem>SUN</asp:ListItem>
                   </asp:CheckBoxList>
                     <asp:CustomValidator ID="CustomValidator1" ClientValidationFunction="ValidateModuleList"
            runat="server" >*</asp:CustomValidator>
                   </td>
                               </tr>
               <tr>
               <td >Location Level</td>
               <td >
                   <asp:DropDownList ID="ddllevel" runat="server" AutoPostBack="True" 
                       onselectedindexchanged="ddllevel_SelectedIndexChanged">
                       <asp:ListItem Selected="True">Country</asp:ListItem>
                       <asp:ListItem>Station</asp:ListItem>
                   </asp:DropDownList>
                   
                   </td>
               <td >Location</td>
               <td >
                   <asp:DropDownList ID="ddlStation" runat="server">
                       
                   </asp:DropDownList>
                   
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                       ControlToValidate="ddlStation" ErrorMessage="*" InitialValue="0" 
                       SetFocusOnError="True"></asp:RequiredFieldValidator>
                   
               </td>
               
               <td >
              
                   
                   Grace Period&nbsp;&nbsp;&nbsp;&nbsp;
                   <asp:TextBox ID="txtGracePrd" runat="server" Width="40px" MaxLength="3"></asp:TextBox>
                   <asp:FilteredTextBoxExtender ID="txtGracePrd_FilteredTextBoxExtender" 
                       runat="server" Enabled="True" FilterType="Numbers" 
                       TargetControlID="txtGracePrd" ValidChars="0123456789">
                   </asp:FilteredTextBoxExtender>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                       ControlToValidate="txtGracePrd" ErrorMessage="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
&nbsp;
                   <asp:DropDownList ID="ddlGraceWay" runat="server">
                       <asp:ListItem>Hrs</asp:ListItem>
                       <asp:ListItem Selected="True">Day</asp:ListItem>
                       <asp:ListItem>Week</asp:ListItem>
                   </asp:DropDownList>
               &nbsp;&nbsp;&nbsp;
                   <asp:DropDownList ID="ddlCurr" runat="server">
                   </asp:DropDownList>
               </td>
               
               </tr>
            </table> 
            </div>
          <%--  <asp:TabContainer ID="tbStorage" runat="server">
            <asp:TabPanel  ID="tbFull" runat="server">
            <HeaderTemplate>Daily</HeaderTemplate>
            <ContentTemplate>
            
            </ContentTemplate>
            </asp:TabPanel>
             <asp:TabPanel  ID="tbExceptation" runat="server">
            <HeaderTemplate>Exception</HeaderTemplate>
            <ContentTemplate>
            <table width="100%">
           <tr>
           
           </td>
           </tr>
            </table> 
            </ContentTemplate>
            </asp:TabPanel>
            </asp:TabContainer> --%>
           
           
        </div>
        <div style="margin-top:10px;" >
        <table width="100%" >
        <tr>
        <td>
        
       
        <asp:GridView ID="gdMissingChrg" runat="server" AutoGenerateColumns="False" CellSpacing="3" CellPadding="3">
        <Columns>
        <asp:TemplateField HeaderText="Type">
        <ItemTemplate>
        <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Daily Demurrage">
        <ItemTemplate>
        <asp:TextBox ID="txtDaily" runat="server" Text='<%# Eval("DailyDem") %>'></asp:TextBox>
        </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Missing/Damage">
        <ItemTemplate>
        <asp:TextBox ID="txtDamage" runat="server" Text='<%# Eval("Missing") %>'></asp:TextBox>
        </ItemTemplate>
        </asp:TemplateField>
        </Columns>
        </asp:GridView> 
        </td>
        <td style="vertical-align:top;">
        <asp:GridView ID="gdException" runat="server" AutoGenerateColumns="False" CellSpacing="3" CellPadding="3">
        <Columns>
        <asp:TemplateField HeaderText="Type">
        <ItemTemplate>
        <asp:Label ID="lblExcepType" runat="server" Text='<%# Eval("ExcType") %>'></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="Code">
        <ItemTemplate>
        <asp:TextBox ID="txtCode" runat="server" Text='<%# Eval("ExcCode") %>'></asp:TextBox>
        </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField>
        <ItemTemplate>
      <asp:RadioButtonList ID="rblWay" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
      <asp:ListItem Selected="True">Include</asp:ListItem>
                       <asp:ListItem>Exclude</asp:ListItem>
      </asp:RadioButtonList>
        </ItemTemplate>
        </asp:TemplateField>
        </Columns>
        </asp:GridView>
        </td>
        </tr>
        </table>    
        </div>
         <asp:Button ID="btnSave" runat="server" CssClass="button" 
                onclick="btnSave_Click" Text="Save" />
            &nbsp;<asp:Button ID="btnClear" runat="server" CssClass="button" 
                onclick="btnClear_Click" Text="Clear" ValidationGroup="Clear"/>
        &nbsp;<asp:Button ID="btnList" runat="server" CssClass="button"
                onclick="btnList_Click" Text="Back" ValidationGroup="Back" 
                Visible="False"/>
        </div>
</asp:Content>