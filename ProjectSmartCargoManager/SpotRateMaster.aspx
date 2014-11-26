<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpotRateMaster.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master"  Inherits="ProjectSmartCargoManager.SpotRateMaster" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function SelectAllgrdAddRate(CheckBoxControl) {
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if (document.forms[0].elements[i].name.indexOf('check') > -1) {
                    document.forms[0].elements[i].checked = CheckBoxControl.checked;
                }
            }
        }

            
        
        
   </script>   

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
     <div class="msg">    
    <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
             ForeColor="Red"></asp:Label>
    </div>
    <h1> 
        <img alt="" src="Images/txt_spotrate.png" />           
     </h1>
  
    <div class="botline">
    <span style="font-weight:bold;">
     <table style="float:right;">
        <tr>
         <td>
         Spot Rate ID
         </td>
         <td>
             <asp:TextBox ID="txtSpotrateid" runat="server" Enabled="False"></asp:TextBox>
         </td>
         <%--<td>--%>
           <%--<asp:Button ID="btnList" runat="server" Text="List" CssClass="button"/>
           &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" ToolTip="Clear All"/>
         </td>--%>
         
        </tr>
     </table> 
     </span>
     
     
     <table>
     <tr>
      <td>
       AWBNumber
          *
      </td>
      <td>
          <asp:TextBox ID="txtPrefix" runat="server" Width="35px" ></asp:TextBox>
          &nbsp;<asp:TextBox ID="txtawbnumber" runat="server" Width="70px"  MaxLength="8" ></asp:TextBox>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" 
              ControlToValidate="txtawbnumber" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
      </td>
      <%--<td>
       UBR Number
      </td>
      <td>
          <asp:TextBox ID="txtUBRNumber" runat="server" Width="50px" ></asp:TextBox>
      </td>
      <td>
       Owner Code
      </td>
      <td>
          <asp:TextBox ID="txtOwnerCode" runat="server" Width="50px" ></asp:TextBox>
      </td>--%>
      <td>
       <asp:Button ID="BtnList" runat="server" Text="List" CssClass="button" 
              onclick="BtnList_Click" />
       &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
              ToolTip="Clear All" onclick="btnClear_Click"/>
      </td>
     </tr>
     </table>
    </div>  
    
   
    <br/><br /><h2>Shipment Details</h2>
     <div class="divback">
    
    <table width="90%" cellpadding=3 cellspacing=3>
     <tr>
      <td>
       Origin
          *</td>
      <td>
          <asp:TextBox ID="txtOrigin" runat="server" Width="70px" ></asp:TextBox>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
              ControlToValidate="txtOrigin" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
          <asp:AutoCompleteExtender ID="AutoC2" runat="server" ServiceMethod="GetStation" CompletionInterval="0"
           EnableCaching="false" CompletionSetCount="10" TargetControlID="txtOrigin" 
           ServicePath="~/SpotRateMaster.aspx" MinimumPrefixLength="1">
           </asp:AutoCompleteExtender>
      </td>
      <td>
       Destination
          *</td>
      <td>
          <asp:TextBox ID="txtDestination" runat="server" Width="70px" ></asp:TextBox>
           <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
              ControlToValidate="txtDestination" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
           <asp:AutoCompleteExtender ID="AutoC3" runat="server" ServiceMethod="GetStation" CompletionInterval="0"
           EnableCaching="false" CompletionSetCount="10" TargetControlID="txtDestination" 
           ServicePath="~/SpotRateMaster.aspx" MinimumPrefixLength="1">
           </asp:AutoCompleteExtender>
      </td>
      <td>
       AgentCode
          *</td>
      <td>
          <asp:DropDownList ID="ddlAgentCode" runat="server" Width="70px" >
          </asp:DropDownList>
          <%--<asp:TextBox ID="txtAgenntCode" runat="server" Width="70px" 
              ontextchanged="txtAgenntCode_TextChanged"></asp:TextBox>
        <asp:AutoCompleteExtender ID="ACEAgentCode" runat="server" ServiceMethod="GetAgentCode"
            CompletionInterval="0" EnableCaching="false" CompletionSetCount="10" TargetControlID="txtAgenntCode" ServicePath="~/SpotRateMaster.aspx"  
            MinimumPrefixLength="1">
        </asp:AutoCompleteExtender>--%>   
      </td>
      <td>
       Agent Name
          *</td>
      <td>
          <%--<asp:TextBox ID="txtAgentName" runat="server" Width="100px"></asp:TextBox>--%>
          <asp:DropDownList ID="ddlagentname" runat="server" Width="70px">
          </asp:DropDownList>
      </td>
     </tr>
     <tr>
      <td>
       Weight
          *</td>
      <td>
          <asp:TextBox ID="txtWeight" runat="server" Width="70px" ></asp:TextBox>
          
          <asp:RequiredFieldValidator ID="txtWeightValidator" runat="server"
              ErrorMessage="*" ControlToValidate="txtWeight" ValidationGroup="valSave"></asp:RequiredFieldValidator>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[-+]?[0-9]\d*\.?[0-9]*$" ErrorMessage="Float Only" ControlToValidate="txtWeight" ></asp:RegularExpressionValidator>
          
      </td>
      <td>
       Volume *
          </td>
      <td>
          <asp:TextBox ID="txtVolume" runat="server" Width="70px" > </asp:TextBox>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server"
              ErrorMessage="*" ControlToValidate="txtVolume" ValidationGroup="valSave"></asp:RequiredFieldValidator>
          
       <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="^[-+]?[0-9]\d*\.?[0-9]*$" ErrorMessage="Float Only" ControlToValidate="txtVolume" ></asp:RegularExpressionValidator>
          
      </td>
     <td>
      Flight Number
         *</td>
     <td>
         <asp:TextBox ID="txtFlightNumber" runat="server" Width="70px" ></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" 
             ControlToValidate="txtFlightNumber" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
     </td>
     <td>
      Flight Date
         *</td>
     <td>
         <asp:TextBox ID="txtFlightDate" runat="server" Width="100px" ></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" 
             ControlToValidate="txtFlightDate" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
         <asp:TextBoxWatermarkExtender ID="txtFlightDate_TextBoxWatermarkExtender" 
          runat="server" TargetControlID="txtFlightDate" WatermarkText="Flight Date">
          </asp:TextBoxWatermarkExtender>
         <asp:CalendarExtender ID="txtFlightDate_CalendarExtender" runat="server" 
          Enabled="True" TargetControlID="txtFlightDate" Format="dd-MM-yyyy" >
          </asp:CalendarExtender>
         
     </td>
         
     </tr>
     <tr>
      <td>
       Commodity
          *</td>
      <td>
      <%-- <asp:TextBox ID="txtCommodity" runat="server" Width="70px" ></asp:TextBox>--%>
          
       <asp:DropDownList ID="ddlCommodity" runat="server" Width="70px">
       </asp:DropDownList>
       
      </td> 
     </tr>
    </table> 
    </div> 
    
    <h2>Spot Rate Details</h2>
        <div class="divback">
    
     <table width="100%" cellpadding=3 cellspacing=3>
      <tr>
       <td>
        Spot category
           *</td>
       <td>
           <asp:DropDownList ID="ddlSpotCategory" runat="server" 
               onselectedindexchanged="ddlSpotCategory_SelectedIndexChanged" 
               AutoPostBack="True">
               <asp:ListItem  Text="Select"></asp:ListItem>
                  
               <asp:ListItem Selected="True" Text="Per KG"></asp:ListItem> 
               <asp:ListItem  Text="Flat Charge"></asp:ListItem>   
               <asp:ListItem  Text="All In"></asp:ListItem>   
           </asp:DropDownList>
       </td>
       <td>
        Weight category
           *</td>
       <td>
           <asp:DropDownList ID="ddlWtCategory" runat="server">
               <asp:ListItem Text="Select"></asp:ListItem>   
               <asp:ListItem  Selected="True"  Text="Charge Weight" Value="C"></asp:ListItem> 
               <asp:ListItem  Text="Gross Weight" Value="G"></asp:ListItem>   
           </asp:DropDownList>
       </td>
       <td>
           &nbsp;</td>
       <td>
        Spot Rate
           *</td>
       <td>
           <asp:TextBox ID="txtSpotRate" runat="server" Width="70px" ></asp:TextBox>
           <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" 
               ControlToValidate="txtSpotRate" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
                <asp:TextBox ID="txtAppliedRate" runat="server" Width="70px" Visible="false"></asp:TextBox>
       </td>
       </tr>
       <tr>
       <td>
        Threshold Limit
           *</td>
       <td>
           <asp:TextBox ID="txtthreshold" runat="server" Width="70px" >0</asp:TextBox>
           <asp:RequiredFieldValidator ID="txttreshold_RequiredField" runat="server" ControlToValidate="txtthreshold" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
           <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationExpression="^[-+]?[0-9]\d*\.?[0-9]*$" ErrorMessage="Float Only" ControlToValidate="txtthreshold" ></asp:RegularExpressionValidator>
       </td>
       <td>
        Currency
           *</td>
       <td>
           <asp:TextBox ID="txtCurrency" runat="server"  Width="70px"  ></asp:TextBox>
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCurrency" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>

       </td>
       <td>
           &nbsp;</td>
       <td>
        Station
           *</td>
       <td>
           <asp:TextBox ID="txtStation" runat="server"></asp:TextBox>
            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="GetStation"
            CompletionInterval="0" EnableCaching="false" CompletionSetCount="10" TargetControlID="txtStation" ServicePath="~/SpotRateMaster.aspx"  
            MinimumPrefixLength="1">
           </asp:AutoCompleteExtender> 
           <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtStation" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>

            
       </td>
      
      </tr>
      <tr>
       <td>
        Req Date
           *</td>
       <td>
           <asp:TextBox ID="txtreqdate" runat="server" Width="70px" ></asp:TextBox>
           <asp:CalendarExtender ID="Calreqdate" runat="server" 
          Enabled="True" TargetControlID="txtreqdate" Format="dd-MM-yyyy" >
          </asp:CalendarExtender>
          
           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
               ControlToValidate="txtreqdate" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
          
       </td>
       <td>
        FWD Name
       </td>
       <td>
           <asp:TextBox ID="txtFWDName" runat="server" Width="70px" ></asp:TextBox>
       </td>
       <td>
           &nbsp;</td>
       <td>
        Remarks
       </td>
       <td>
           <asp:TextBox ID="txtremarks" runat="server"></asp:TextBox>
       </td>
      </tr>
      <tr>
      <td colspan="2">
           <asp:CheckBox ID="chkSpecialArrival" runat="server" Text="Special Approval"   />
          </td>
       <td>
        Reason
       </td>
       <td colspan="4" >
          <asp:TextBox ID="txtReason" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="rbCommisionable" runat="server" Text="Commissionable" GroupName="A"/>&nbsp;&nbsp;&nbsp;&nbsp;
           <asp:RadioButton ID="rbNonCommissionable" runat="server" 
               Text="NonCommissionable" GroupName="A" Checked="True"/>
           </td>
      </tr>
     </table>
     
      </div>
    <asp:Panel ID="PnlCarrierDetails" runat="server">
        
        <h2>Carrier Details</h2>
        <div class="divback">
        <table width="90%" cellpadding=3 cellspacing=3>
         <tr>
          <td>
              Issued By *</td>
          <td>
              <asp:TextBox ID="txtIssuedBy" runat="server" Width="70px" ></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                  ControlToValidate="txtIssuedBy" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
          </td>
          <td>
              Issue Date *</td>
          <td>
              <asp:TextBox ID="txtIssueDate" runat="server" Width="70px" ></asp:TextBox>
              <asp:CalendarExtender ID="Calissuedate" runat="server" 
          Enabled="True" TargetControlID="txtIssueDate" Format="dd-MM-yyyy" >
          </asp:CalendarExtender>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                  ControlToValidate="txtIssueDate" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
          </td>
          <td>
              Authorised By *</td>
          <td>
              <asp:TextBox ID="txtAuthorisedBy" runat="server" Width="70px" Enabled="False" ></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                  ControlToValidate="txtAuthorisedBy" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
          </td>
          <td>
              Authorised Date *</td>
          <td>
              <asp:TextBox ID="txtAuthorisedDate" runat="server" Width="70px" ></asp:TextBox>
              <asp:CalendarExtender ID="Calauthorisedddate" runat="server" 
          Enabled="True" TargetControlID="txtAuthorisedDate" Format="dd-MM-yyyy" >
          </asp:CalendarExtender>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                  ControlToValidate="txtAuthorisedDate" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
          </td>
          
         </tr>
        <tr>
        <td>
            Valid From *</td>
        <td>
            <asp:TextBox ID="txtValidFrom" runat="server" Width="70px" ></asp:TextBox>
            <asp:CalendarExtender ID="Calvalidfrom" runat="server" 
          Enabled="True" TargetControlID="txtValidFrom" Format="dd-MM-yyyy" >
          </asp:CalendarExtender>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                ControlToValidate="txtValidFrom" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
        </td>
        <td>
            Valid To *</td>
        <td>
            <asp:TextBox ID="txtValidTo" runat="server" Width="70px" ></asp:TextBox>
             <asp:CalendarExtender ID="Calvalidto" runat="server" 
          Enabled="True" TargetControlID="txtValidTo" Format="dd-MM-yyyy" >
          </asp:CalendarExtender>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                ControlToValidate="txtValidTo" ErrorMessage="*" ValidationGroup="valSave"></asp:RequiredFieldValidator>
        </td>
        
        </tr>
        </table>
        
          
        
       </div>
        </asp:Panel>
    <div id="fotbut">
      <asp:Button ID="btnRoute" runat="server" Text="Route" CssClass="button" 
            Visible="False" />
      &nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
            onclick="btnSave_Click" ValidationGroup="valSave" />
       <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="button" 
            Visible="False"/>
      &nbsp;<asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="button" 
            Visible="False"/>
    </div>                   
    </div> 
</asp:Content> 
