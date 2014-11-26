<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="frmClaimApplication.aspx.cs" Inherits="ProjectSmartCargoManager.frmClaimApplication" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    </asp:Content>
 
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
     <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  
  <script type="text/javascript">
      function displayValidationPhone1() {
          if (typeof (Page_Validators) == "undefined") return;
          var LblName = document.getElementById('<%= lblPhone.ClientID%>');
          var RequiredName = document.getElementById('<%= RegularExpressionValidator4.ClientID%>');
          ValidatorValidate(RequiredName);
          if (!RequiredName.isvalid) {
              LblName.innerHTML = RequiredName.errormessage;
          }
          else {
              LblName.innerHTML = '';
          }
      }
</script>
<script type="text/javascript">
    function displayValidationPhone2() {
        if (typeof (Page_Validators) == "undefined") return;
        var LblName = document.getElementById('<%= lblPhone.ClientID%>');
        var RequiredName = document.getElementById('<%= RegularExpressionValidator5.ClientID%>');
        ValidatorValidate(RequiredName);
        if (!RequiredName.isvalid) {
            LblName.innerHTML = RequiredName.errormessage;
        }
        else {
            LblName.innerHTML = '';
        }
    }
</script>
<script type="text/javascript">
    function displayValidationPhone3() {
        if (typeof (Page_Validators) == "undefined") return;
        var LblName = document.getElementById('<%= lblPhone.ClientID%>');
        var RequiredName = document.getElementById('<%= RegularExpressionValidator6.ClientID%>');
        ValidatorValidate(RequiredName);
        if (!RequiredName.isvalid) {
            LblName.innerHTML = RequiredName.errormessage;
        }
        else {
            LblName.innerHTML = '';
        }
    }
</script>

 <script type="text/javascript">
     function displayValidationMobile1() {
         if (typeof (Page_Validators) == "undefined") return;
         var LblName = document.getElementById('<%= lblMobile.ClientID%>');
         var RequiredName = document.getElementById('<%= RegularExpressionValidator10.ClientID%>');
         ValidatorValidate(RequiredName);
         if (!RequiredName.isvalid) {
             LblName.innerHTML = RequiredName.errormessage;
         }
         else {
             LblName.innerHTML = '';
         }
     }
</script>
<script type="text/javascript">
    function displayValidationMobile2() {
        if (typeof (Page_Validators) == "undefined") return;
        var LblName = document.getElementById('<%= lblMobile.ClientID%>');
        var RequiredName = document.getElementById('<%= RegularExpressionValidator11.ClientID%>');
        ValidatorValidate(RequiredName);
        if (!RequiredName.isvalid) {
            LblName.innerHTML = RequiredName.errormessage;
        }
        else {
            LblName.innerHTML = '';
        }
    }
</script>
<script type="text/javascript">
    function displayValidationMobile3() {
        if (typeof (Page_Validators) == "undefined") return;
        var LblName = document.getElementById('<%= lblMobile.ClientID%>');
        var RequiredName = document.getElementById('<%= RegularExpressionValidator12.ClientID%>');
        ValidatorValidate(RequiredName);
        if (!RequiredName.isvalid) {
            LblName.innerHTML = RequiredName.errormessage;
        }
        else {
            LblName.innerHTML = '';
        }
    }
</script>

 <script type="text/javascript">
     function displayValidationFax1() {
         if (typeof (Page_Validators) == "undefined") return;
         var LblName = document.getElementById('<%= lblFax.ClientID%>');
         var RequiredName = document.getElementById('<%= RegularExpressionValidator13.ClientID%>');
         ValidatorValidate(RequiredName);
         if (!RequiredName.isvalid) {
             LblName.innerHTML = RequiredName.errormessage;
         }
         else {
             LblName.innerHTML = '';
         }
     }
</script>
<script type="text/javascript">
    function displayValidationFax2() {
        if (typeof (Page_Validators) == "undefined") return;
        var LblName = document.getElementById('<%= lblFax.ClientID%>');
        var RequiredName = document.getElementById('<%= RegularExpressionValidator14.ClientID%>');
        ValidatorValidate(RequiredName);
        if (!RequiredName.isvalid) {
            LblName.innerHTML = RequiredName.errormessage;
        }
        else {
            LblName.innerHTML = '';
        }
    }
</script>
<script type="text/javascript">
    function displayValidationFax3() {
        if (typeof (Page_Validators) == "undefined") return;
        var LblName = document.getElementById('<%= lblFax.ClientID%>');
        var RequiredName = document.getElementById('<%= RegularExpressionValidator15.ClientID%>');
        ValidatorValidate(RequiredName);
        if (!RequiredName.isvalid) {
            LblName.innerHTML = RequiredName.errormessage;
        }
        else {
            LblName.innerHTML = '';
        }
    }
</script>

<script type="text/javascript">
    function ValidateClaimStatus() {
        
        var disbursedamt = document.getElementById('<%=  txtDisbursedAmount.ClientID%>')
        var approvalamt = document.getElementById('<%=  txtApprovalAmount.ClientID%>');
        var ddlStatus = document.getElementById('<%= ddlStatus.ClientID%>');
        if (disbursedamt.value == approvalamt.value) {
            ddlStatus.value = "Closed";
        }

    }
</script>

<script type="text/javascript">
    function validateFileUpload(obj) {
        var fileName = new String();
        var fileExtension = new String();

        // store the file name into the variable  
        fileName = obj.value;

        // extract and store the file extension into another variable  
        fileExtension = fileName.substr(fileName.length - 3, 3);

        // array of allowed file type extensions  
        var validFileExtensions = new Array("jpg", "png", "gif");

        var flag = false;

        // loop over the valid file extensions to compare them with uploaded file  
        for (var index = 0; index < validFileExtensions.length; index++) {
            if (fileExtension.toLowerCase() == validFileExtensions[index].toString().toLowerCase()) {
                flag = true;
            }
        }

        // display the alert message box according to the flag value  
        if (flag == false) {
            alert('Files with extension ".' + fileExtension.toUpperCase() + '" are not allowed.\n\nYou can upload the files with following extensions only:\n.jpg\n.png\n.gif\n');
            return false;
        }
        else {
            return true;
        }
    }  
</script>

<script type="text/javascript">
    function GetCode() {
        var origin = document.getElementById('<%= txtOrigin.ClientID%>');
        if (origin.value.length > 4) {
            origin.value = origin.value.substring(origin.value.length - 4);
            origin.value = origin.value.replace(')', '');

        }
    }
</script>

<script type="text/javascript">
    function GetCodeStation() {
        var origin = document.getElementById('<%= txtClaimStation.ClientID%>');
        if (origin.value.length > 4) {
            origin.value = origin.value.substring(origin.value.length - 4);
            origin.value = origin.value.replace(')', '');

        }
    }
</script>

<script type="text/javascript">
    function GetCodeDestination() {
        var destination = document.getElementById('<%= txtDestination.ClientID%>');
        if (destination.value.length > 4) {
            destination.value = destination.value.substring(destination.value.length - 4);
            destination.value = destination.value.replace(')', '');
        }
    }
</script>
  
  
  <div id="contentarea">
  <h1>Claim Application</h1>
   
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
 
   <div class="botline">
<h2>AWB Details :</h2>
  <table cellpadding="3" cellspacing="3" width="100%">
  <tr>
  <td width="20%">AWB Number : </td>
  <td  width="30%">
  <asp:TextBox ID="txtAWBNo1" runat="server" Width="30px" MaxLength="3"></asp:TextBox> 
      
  <asp:RequiredFieldValidator ID="RequiredFieldValidator_AWBPrefix" runat="server" ControlToValidate="txtAWBNo1" ErrorMessage="*"></asp:RequiredFieldValidator>
      <asp:TextBox ID="txtAWBNo2" runat="server" Width="100px" MaxLength="8" 
          ontextchanged="txtAWBNo2_TextChanged" AutoPostBack="true"></asp:TextBox>
      
        <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtAWBNo2" runat="server" ControlToValidate="txtAWBNo2" ErrorMessage="*"></asp:RequiredFieldValidator>
        <asp:Button ID="btnList" runat="server" CausesValidation="False" 
          CssClass="button" onclick="btnList_Click" Text="List" Visible="False" />
        <asp:RegularExpressionValidator ID="RegularExpression_AWBNo" runat="server" ValidationExpression="^[0-9]+$" ErrorMessage="Numbers Only" ControlToValidate="txtAWBNo2"></asp:RegularExpressionValidator>

      </td>
  <td width="20%">
      AWB Date :</td>
  <td width="30%">
      <asp:TextBox ID="txtAWBDate" runat="server"></asp:TextBox>
      <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtAWBDate" Format="dd/MM/yyyy"></asp:CalendarExtender>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtAWBDate" runat="server" ControlToValidate="txtAWBDate" ErrorMessage="*"></asp:RequiredFieldValidator>

      </td>
  </tr>
  <tr>
  <td>Origin :</td>
  <td>
      <asp:TextBox ID="txtOrigin" runat="server" MaxLength="3" onchange="javascript:GetCode()"></asp:TextBox>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtOrigin" runat="server" ControlToValidate="txtOrigin" ErrorMessage="*"></asp:RequiredFieldValidator>


      </td>
  <td>
      Destination :</td>
  <td>
      <asp:TextBox ID="txtDestination" runat="server" MaxLength="3" onchange="javascript:GetCodeDestination()"></asp:TextBox>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtDestination" runat="server" ControlToValidate="txtDestination" ErrorMessage="*"></asp:RequiredFieldValidator>

      </td>
  </tr>
  <tr>
  <td>Full Name :</td>
  <td>
      <asp:TextBox ID="txtFullName" runat="server" MaxLength="50"></asp:TextBox>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="*"></asp:RequiredFieldValidator>

      </td>
  <td>
      Address :</td>
      <td>
      <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Height="27px" Width="214px" MaxLength="100">
      </asp:TextBox>
       <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtAddress" runat="server" ControlToValidate="txtAddress" ErrorMessage="*"></asp:RequiredFieldValidator>

      </td>
  </tr>
  <tr>
  <td>Phone :</td>
  <td>
      <asp:TextBox ID="txtPhone1" runat="server" Width="40px" MaxLength="2" onchange="javascript:displayValidationPhone1()"></asp:TextBox>
      &nbsp;<asp:TextBox ID="txtPhone2" runat="server" Width="40px" MaxLength="4" onchange="javascript:displayValidationPhone2()"></asp:TextBox>
      &nbsp;<asp:TextBox ID="txtPhone3" runat="server" Width="100px" MaxLength="6" onchange="javascript:displayValidationPhone3()"></asp:TextBox>
      <asp:Label ID="lblPhone" runat="server" ForeColor="Red"></asp:Label>
      

      </td>
  <td>
      Mobile :</td>

  <td>
      <asp:TextBox ID="txtMobile1" runat="server" Width="40px" MaxLength="2" onchange="javascript:displayValidationMobile1()"></asp:TextBox>
      &nbsp;<asp:TextBox ID="txtMobile2" runat="server" Width="40px" MaxLength="4" onchange="javascript:displayValidationMobile2()"></asp:TextBox>
      &nbsp;<asp:TextBox ID="txtMobile3" runat="server" Width="100px" MaxLength="8" onchange="javascript:displayValidationMobile3()"></asp:TextBox>
       <asp:Label ID="lblMobile" runat="server" ForeColor="Red"></asp:Label>
      
      </td>
  </tr>
  <tr>
  <td>Fax :</td>
  <td>
      <asp:TextBox ID="txtFax1" runat="server" Width="40px" MaxLength="2" onchange="javascript:displayValidationFax1()"></asp:TextBox>
      &nbsp;<asp:TextBox ID="txtFax2" runat="server" Width="40px" MaxLength="4" onchange="javascript:displayValidationFax2()"></asp:TextBox>
      &nbsp;<asp:TextBox ID="txtFax3" runat="server" Width="100px" MaxLength="6" onchange="javascript:displayValidationFax3()"></asp:TextBox>
       <asp:Label ID="lblFax" runat="server" ForeColor="Red"></asp:Label>
      </td>
  <td>
      e-mail :</td>
  <td>
      <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="*"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="RegularExpression_Email" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$" ErrorMessage="Invalid Email format"></asp:RegularExpressionValidator>
      </td>
  </tr>
  <tr>
  <td>Flight Number :</td>
  <td>
      <asp:TextBox ID="txtFlightNumber1" runat="server" Width="24px"></asp:TextBox>
      &nbsp;
      <asp:TextBox ID="txtFlightNumber2" runat="server" Width="100px"></asp:TextBox>

      </td>
  <td>
      Flight Date :</td>
  <td>
      <asp:TextBox ID="txtFlightDate" runat="server"></asp:TextBox>
      <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFlightDate" Format="dd/MM/yyyy"></asp:CalendarExtender>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtFlightDate" runat="server" ControlToValidate="txtFlightDate" ErrorMessage="*"></asp:RequiredFieldValidator>
      </td>
  </tr>
  <tr>
  <td>HAWB :</td>
  <td>
      <asp:TextBox ID="txtHAWB" runat="server" MaxLength="50"></asp:TextBox>

      </td>
  <td>
      Currency :</td>
  <td>
             <asp:DropDownList ID="ddlCurrency" runat="server">
             </asp:DropDownList>
         </td>
  </tr>
  <tr>
  <td>Shipper Code :</td>
  <td>
      <asp:Label ID="lblShipperCode" runat="server"></asp:Label>

      </td>
  <td>
      Shipper Name :</td>
  <td>
      <asp:Label ID="lblShipperName" runat="server"></asp:Label>

      </td>
  </tr>
  <tr>
  <td>Consignee Code :</td>
  <td>
      <asp:Label ID="lblConsigneeCode" runat="server"></asp:Label>
      </td>
  <td>
      Consignee Name :</td>
  <td>
      <asp:Label ID="lblConsigneeName" runat="server"></asp:Label>
      </td>
  </tr>
  </table>
  </div>
  
  <div style="padding:10px;" class="botline">
   <h2>Claim Details :</h2>
  <table cellpadding="3" cellspacing="3" width="100%">
  <tr>
  <td width="20%">Claim Type :</td>
  <td width="30%">
      <asp:DropDownList ID="ddlClaimType" runat="server">
      <asp:ListItem>Missing</asp:ListItem>
      <asp:ListItem>Damage</asp:ListItem>
      <asp:ListItem>Partial Loss</asp:ListItem>
      <asp:ListItem>Delay</asp:ListItem>
      <asp:ListItem>Pilferage</asp:ListItem>
      <asp:ListItem>Mortality</asp:ListItem>
      </asp:DropDownList>
      </td>
  <td width="20%">
            Claim Details :</td>
  <td width="30%">
      <asp:TextBox ID="txtClaimDetails" runat="server" TextMode="MultiLine" 
          Height="27px" Width="214px"></asp:TextBox>
      </td>
  </tr>
  <tr>
  <td width="20%">Claim Date :</td>
  <td width="30%">
      <asp:TextBox ID="txtClaimDate" runat="server"></asp:TextBox>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtClaimDate" ErrorMessage="*"></asp:RequiredFieldValidator>
      <asp:CalendarExtender ID="ExClaimDate" runat="server" TargetControlID="txtClaimDate" Format="dd/MM/yyyy"></asp:CalendarExtender>
      </td>
  <td width="20%">
            Claim Station :</td>
  <td width="30%">
      <asp:TextBox ID="txtClaimStation" runat="server" MaxLength="3" 
          onchange="javascript:GetCodeStation();ValidateClaimStatus();"></asp:TextBox>
      <asp:AutoCompleteExtender ID="txtClaimStation_AutoCompleteExtender" 
          runat="server" MinimumPrefixLength="1" ServiceMethod="GetStation" 
          ServicePath="~/frmClaimApplication.aspx" TargetControlID="txtClaimStation">
      </asp:AutoCompleteExtender>
      </td>
  </tr>
  <tr>
  <td >Total Weight :</td>
  <td >
      <asp:TextBox ID="txtTotalWeight" runat="server"></asp:TextBox>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[-+]?[0-9]\d*\.?[0-9]*$" ErrorMessage="Float Only" ControlToValidate="txtTotalWeight"></asp:RegularExpressionValidator>
       <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTotalWeight" ErrorMessage="*"></asp:RequiredFieldValidator>

      </td>
  <td >
      Total Pieces :</td>
  <td >
      <asp:TextBox ID="txtTotalPcs" runat="server"></asp:TextBox>
       <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ValidationExpression="^[0-9]+$" ErrorMessage="Numbers Only" ControlToValidate="txtTotalPcs"></asp:RegularExpressionValidator>
       <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtTotalPcs" ErrorMessage="*"></asp:RequiredFieldValidator>
      </td>
  </tr>
  <tr>
  <td >Total Mishandling Weight :</td>
  <td >
      <asp:TextBox ID="txtTotalMishandlingWgt" runat="server"></asp:TextBox>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="^[-+]?[0-9]\d*\.?[0-9]*$" ErrorMessage="Float Only" ControlToValidate="txtTotalMishandlingWgt"></asp:RegularExpressionValidator>
      <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtTotalMishandlingWgt" ErrorMessage="*"></asp:RequiredFieldValidator>
     </td>
  <td >
      Total Mishandling Pieces :</td>
  <td >
      <asp:TextBox ID="txtTotalMishandlingPcs" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ValidationExpression="^[0-9]+$" ErrorMessage="Numbers Only" ControlToValidate="txtTotalMishandlingPcs"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtTotalMishandlingPcs" ErrorMessage="*"></asp:RequiredFieldValidator>

     </td>
  </tr>
  <tr>
  <td >Nature of Goods :</td>
  <td >
      <asp:TextBox ID="txtNatureOfGoods" runat="server"></asp:TextBox>
 </td>
  <td >
      Claim Amount :</td>
  <td >
      <asp:TextBox ID="txtClaimAmt" runat="server"></asp:TextBox>
      
            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationExpression="^[-+]?[0-9]\d*\.?[0-9]*$" ErrorMessage="Float Only" ControlToValidate="txtClaimAmt"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtClaimAmt" ErrorMessage="*"></asp:RequiredFieldValidator>
</td>
  </tr>
  <tr>
  <td >Handled By :</td>
  <td >
          <asp:TextBox ID="txtHandledBy" runat="server"></asp:TextBox>
     </td>
  <td >
      &nbsp;</td>
  <td >
      &nbsp;</td>
  </tr>
  <tr>
  <td ><h3>Disbursement Details :</h3></td> </tr>
  <tr>
  <td >Payment Mode :</td>
  <td >
      <asp:DropDownList ID="ddlPaymentMode" runat="server">
      <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
          <asp:ListItem Text="Cheque" Value="Cheque"></asp:ListItem>
          <asp:ListItem Text="DD" Value="DD"></asp:ListItem>
          <asp:ListItem Text="RTGS" Value="RTGS"></asp:ListItem>
          <asp:ListItem Text="Card" Value="Card"></asp:ListItem>
      </asp:DropDownList>
 </td>
  <td >
      Approval Amount :</td>
  <td >
      <asp:TextBox ID="txtApprovalAmount" runat="server"></asp:TextBox>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator_txtApprovalAmount" runat="server" ValidationExpression="^[-+]?[0-9]\d*\.?[0-9]*$" ErrorMessage="Float Only" ControlToValidate="txtApprovalAmount"></asp:RegularExpressionValidator>
      </td>
  </tr>
  <tr>
  <td >Disbursed Amount :</td>
  <td >
      <asp:TextBox ID="txtDisbursedAmount" runat="server" onBlur="javascript:ValidateClaimStatus();"></asp:TextBox>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ValidationExpression="^[-+]?[0-9]\d*\.?[0-9]*$" ErrorMessage="Float Only" ControlToValidate="txtDisbursedAmount"></asp:RegularExpressionValidator>
 </td>
  <td >
      Disbursment Details :</td>
  <td >
      <asp:TextBox ID="txtDisbursmentClaimDetails" runat="server" TextMode="MultiLine" 
          Height="27px" Width="214px" onchange="javascript:ValidateClaimStatus();"></asp:TextBox>
      </td>
  </tr>
  <tr>
  <td >Status :</td>
  <td >
      <asp:DropDownList ID="ddlStatus" runat="server">
      <asp:ListItem>Open</asp:ListItem>
      <asp:ListItem>Closed</asp:ListItem>
      <asp:ListItem>In Process</asp:ListItem>
      <asp:ListItem>Approved</asp:ListItem>
      <asp:ListItem>Rejected</asp:ListItem>
      </asp:DropDownList>


 </td>
  <td >
      &nbsp;</td>
  <td >
      &nbsp;</td>
  </tr>
  </table>
  </div>
  
  <div style="padding:10px;">
  <h2>Required Documents :</h2>
  <table cellpadding="3" cellspacing="3" width="100%">

  <tr>
  <td width="20%">Invoice :</td>
  <td width="30%">
      <asp:FileUpload ID="FileUpload_Invoice" runat="server" onchange="javascript:validateFileUpload(this)" />
      <asp:RegularExpressionValidator runat="server" ID="valUpTest" ControlToValidate="FileUpload_Invoice" 
             
            ValidationExpression="^.+\.((jpg)|(JPG)|(gif)|(GIF)|(jpeg)|(JPEG)|(png)|(PNG)|(bmp)|(BMP))$" />
      </td>
  <td width="20%">
      Packaging List :</td>
  <td  width="30%">
      <asp:FileUpload ID="FileUpload_PackagingList" runat="server" onchange="javascript:validateFileUpload(this)" />
      <asp:RegularExpressionValidator runat="server" ID="valUpTest3" ControlToValidate="FileUpload_PackagingList" 
             
            
          ValidationExpression="^.+\.((jpg)|(JPG)|(gif)|(GIF)|(jpeg)|(JPEG)|(png)|(PNG)|(bmp)|(BMP))$" />
      </td>
  </tr>
  <tr>
  <td>Customs Report :</td>
  <td >
      <asp:FileUpload ID="FileUpload_CustomsReport" runat="server" onchange="javascript:validateFileUpload(this)"/>
      <asp:RegularExpressionValidator runat="server" ID="valUpTest0" ControlToValidate="FileUpload_CustomsReport" 
             
            
          ValidationExpression="^.+\.((jpg)|(JPG)|(gif)|(GIF)|(jpeg)|(JPEG)|(png)|(PNG)|(bmp)|(BMP))$" />
      </td>
  <td>
      Subrogation Letter :</td>
  <td >
      <asp:FileUpload ID="FileUpload_SubrogationLetter" runat="server" onchange="javascript:validateFileUpload(this)"/>
      <asp:RegularExpressionValidator runat="server" ID="valUpTest4" ControlToValidate="FileUpload_SubrogationLetter" 
             
            
          ValidationExpression="^.+\.((jpg)|(JPG)|(gif)|(GIF)|(jpeg)|(JPEG)|(png)|(PNG)|(bmp)|(BMP))$" />
      </td>
  </tr>
  <tr>
  <td >Survey Report :</td>
  <td >
      <asp:FileUpload ID="FileUploadSurveyReport" runat="server" onchange="javascript:validateFileUpload(this)"/>
      <asp:RegularExpressionValidator runat="server" ID="valUpTest1" ControlToValidate="FileUploadSurveyReport" 
             
            
          ValidationExpression="^.+\.((jpg)|(JPG)|(gif)|(GIF)|(jpeg)|(JPEG)|(png)|(PNG)|(bmp)|(BMP))$" />
      </td>
  <td >
      Authority Report :</td>
  <td >
      <asp:FileUpload ID="FileUpload_AuthorityReport" runat="server" onchange="javascript:validateFileUpload(this)"/>
      <asp:RegularExpressionValidator runat="server" ID="valUpTest5" ControlToValidate="FileUpload_AuthorityReport" 
             
            
          ValidationExpression="^.+\.((jpg)|(JPG)|(gif)|(GIF)|(jpeg)|(JPEG)|(png)|(PNG)|(bmp)|(BMP))$" />
      </td>
  </tr>
  <tr>
  <td >Station Report :</td>
  <td >
      <asp:FileUpload ID="FileUpload_StationReport" runat="server" onchange="javascript:validateFileUpload(this)"/>
      <asp:RegularExpressionValidator runat="server" ID="valUpTest2" ControlToValidate="FileUpload_StationReport" 
             
            
          ValidationExpression="^.+\.((jpg)|(JPG)|(gif)|(GIF)|(jpeg)|(JPEG)|(png)|(PNG)|(bmp)|(BMP))$" />
      </td>
  <td >
      Picture :</td>
  <td >
      <asp:FileUpload ID="FileUpload_Picture" runat="server" onchange="javascript:validateFileUpload(this)"/>
      <asp:RegularExpressionValidator runat="server" ID="valUpTest6" ControlToValidate="FileUpload_Picture" 
             
            
          ValidationExpression="^.+\.((jpg)|(JPG)|(gif)|(GIF)|(jpeg)|(JPEG)|(png)|(PNG)|(bmp)|(BMP))$" />
      </td>
  </tr>

  </table>
  </div>
  
  <br />
     <div align="left" style="margin-right:75px;">  <asp:Button ID="btnSubmit" 
             runat="server" Text="Submit" CssClass="button"  
             onclick="btnSubmit_Click" />
         <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
              onclick="btnClear_Click" CausesValidation="False" />
</div>
<asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ValidationExpression="^[0-9]+$" ErrorMessage="Numbers Only" ControlToValidate="txtPhone1" Display="None"></asp:RegularExpressionValidator>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ValidationExpression="^[0-9]+$" ErrorMessage="Numbers Only" ControlToValidate="txtPhone2" Display="None"></asp:RegularExpressionValidator>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ValidationExpression="^[0-9]+$" ErrorMessage="Numbers Only" ControlToValidate="txtPhone3" Display="None"></asp:RegularExpressionValidator>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ValidationExpression="^[0-9]+$" ErrorMessage="Numbers Only" ControlToValidate="txtMobile1" Display="None"></asp:RegularExpressionValidator>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ValidationExpression="^[0-9]+$" ErrorMessage="Numbers Only" ControlToValidate="txtMobile2" Display="None"></asp:RegularExpressionValidator>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ValidationExpression="^[0-9]+$" ErrorMessage="Numbers Only" ControlToValidate="txtMobile3" Display="None"></asp:RegularExpressionValidator>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ValidationExpression="^[0-9]+$" ErrorMessage="Numbers Only" ControlToValidate="txtFax1" Display="None"></asp:RegularExpressionValidator>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ValidationExpression="^[0-9]+$" ErrorMessage="Numbers Only" ControlToValidate="txtFax2" Display="None"></asp:RegularExpressionValidator>
      <asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server" ValidationExpression="^[0-9]+$" ErrorMessage="Numbers Only" ControlToValidate="txtFax3" Display="None"></asp:RegularExpressionValidator>
  </div>

  
  </asp:Content>

