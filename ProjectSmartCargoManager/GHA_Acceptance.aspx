<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="GHA_Acceptance.aspx.cs" Inherits="ProjectSmartCargoManager.Acceptance_GHA" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ContentPlaceHolderID="head" ID="Content1" runat="server">

<script type="text/javascript">

    function PrintButtonVisibility(id) {
        //document.getElementById('divPrintUCR').style.display = id; 
    }
    function RadioCheck(rb) {
        var gv = document.getElementById("<%=grdAWBDetails.ClientID%>");
        var rbs = gv.getElementsByTagName("input");

        var row = rb.parentNode.parentNode;
        for (var i = 0; i < rbs.length; i++) {
            if (rbs[i].type == "radio") {
                if (rbs[i].checked && rbs[i] != rb) {
                    rbs[i].checked = false;
                    break;
                }
                
            } 
        }
    }

    function SetOperationTime() {
        //Show popup for saving Actual Operation Time
        //window.open('frmOperationTime.aspx', 'Operation Time','left=400,top=200,width=400,height=200,toolbar=0,resizable=no');
        //window.open('frmOperationTime.aspx', '', 'width=400px,height=200px,left=400,top=200');
        document.getElementById('divOpsTimePopup').style.display = 'block';
        document.getElementById('blackening').style.display = 'block';
        return false;
    }
    function CloseWindow() {
        document.getElementById('divOpsTimePopup').style.display = 'none';
        document.getElementById('blackening').style.display = 'none';
    }
    
    
    function popup() {



        var AWBNumber = document.getElementById("<%= hdnAWBNo.ClientID %>").value;
        var UCRNo = document.getElementById("<%= hdUCRNo.ClientID %>").value;
        if (UCRNo == "")
        {UCRNo = "NO";}
        AWBNumber = AWBNumber.replace('-', '');
        window.open('UCRPopup.aspx?Type=New' + '&Mode=A' + '&AWBNo=' + AWBNumber + '&pg=Acc&UCRNo='+UCRNo, '', 'left=0,top=0,width=1000,height=1000,toolbar=0,resizable=0');
    }
    
     function CheckAllEmp(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=grdAcceptance.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }

        function SelectheaderCheckboxes(headerchk) {
            var gvcheck = document.getElementById("<%=grdAcceptance.ClientID %>");
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
        function chngurl() {
            document.location.href = "GHA_Acceptance.aspx";
        }

        function dimension(mybutton) {

            //    code for getting data
            var strValue = mybutton.value;

            //         var i;

            var row = mybutton.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            var AWBID = row.cells[1].children[0].attributes["ID"].value;
            var UOMID = row.cells[6].children[0].attributes["ID"].value;
            var FlightID = row.cells[2].children[0].attributes["ID"].value;
            var FlightDateID = row.cells[3].children[0].attributes["ID"].value;
            var AcceptedPcs = row.cells[9].children[0].attributes["ID"].value;
            var AcceptedWt = row.cells[10].children[0].attributes["ID"].value;

            var strAwbNo = document.getElementById(AWBID).innerHTML;
            var strFlightNo = document.getElementById(FlightID).innerHTML;
            var strFlightDate = document.getElementById(FlightDateID).innerHTML;
            var TtPcsID = row.cells[9].children[0].attributes["ID"].value;
            var strUOM = document.getElementById(UOMID).innerHTML;

            var strTotalPcs = document.getElementById(TtPcsID).value;

            var TtWtID = row.cells[10].children[0].attributes["ID"].value;
            var strTotalWt = document.getElementById(TtWtID).value;
            var SHCID = mybutton.id.replace("btnDimensionsPopup", "txtSpecialHandlingCode");
            var SHC = document.getElementById(SHCID).value;
            var targetcontrol = mybutton.id.replace("btnDimensionsPopup", "ddlCommCode");
            var DropDownList = document.getElementById(targetcontrol);

            var strCommCode = DropDownList.value;
            var TxtClientButtonID = document.getElementById("<%= btnSave.ClientID %>");
            var TxtClientObjectID = mybutton.id.replace("btnDimensionsPopup", "txtAcceptedPcs");
            var TxtClientObjectVolID = mybutton.id.replace("btnDimensionsPopup", "txtAcceptedWt");
            var TxtClientObjectRemainingPcs = mybutton.id.replace("btnDimensionsPopup", "txtRcvPcs");
            var TxtClientObjectRemainingWt = mybutton.id.replace("btnDimensionsPopup", "txtRcvWt");
            var AccPcs = document.getElementById(TxtClientObjectID).value;
            var AccWt = document.getElementById(TxtClientObjectVolID).value;
            var Mode = "A";

            window.open('GHA_Dimensions.aspx?awbno=' + strAwbNo + '&UOM=' + strUOM + '&Mode=' + Mode + '&FltNo=' + strFlightNo + '&RowIndex=' + rowIndex + '&FltDate=' + strFlightDate + '&AccpPcsID=' + TxtClientObjectID + '&AccpWtID=' + TxtClientObjectVolID + '&SaveButton=' + TxtClientButtonID + '&PcsCount=' + strTotalPcs + '&SHC=' + SHC + '&commodity=' + strCommCode + '&GrossWt=' + strTotalWt + '&RecievedPcsTxt=' + AccPcs + '&RecievedWtTxt=' + AccWt + '&RemainingPcsTxt=' + TxtClientObjectRemainingPcs + '&RemainingWtTxt=' + TxtClientObjectRemainingWt, '', 'left=0,top=0,width=800,height=450,toolbar=0,resizable=0');

            return false;

        }

            
</script>

<script type="text/javascript">
    function funcsum() {
        debugger;
        //alert("Working");

        var table = document.getElementById('<%= grdAWBDetails.ClientID%>');
        var sum = 0;
        for (var i = 1; i < table.rows.length; i++) //setting the incrementor=0, but if you have a header set it to 1
        {
            //alert(i.toString());
            if (table.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked) {
                //alert("Inside Selected Row");
                var labels = table.rows[i].cells[4].getElementsByTagName("span");
                //alert(labels[0].innerHTML);
                //alert(table.rows[i].cells[6].children[0].value);
                if (table.rows[i].cells[9].children[0].value > labels[0].innerHTML) {
                    if (confirm("Accepted Pcs Different than BookedPcs. Do you want to continue?"))
                    { return true; }
                    else
                        return false;
                }
            }
        }
    }

    function CCSFValidate() {
        debugger;
        var table = document.getElementById('<%= grdAWBDetails.ClientID%>');
        for (var i = 1; i < table.rows.length; i++) //setting the incrementor=0, but if you have a header set it to 1
        {
            //alert(i.toString());
            if (table.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked) {
                var CCSF = table.rows[i].cells[18].children[0].value;
                if (CCSF == 'N') {
                    if (confirm("CCSF Expired. Do you want to continue?"))
                    { return true; }
                    else
                        return false;
                }
                return true;
            }
        }
    }
</script>

</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content2" runat="server">
    <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  
  <script type="text/javascript">

      Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
      Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);
      
      function GetSpecialHandlingCode(mybutton) {
          var strValue = mybutton.value;
          var row = mybutton.parentNode.parentNode;
          var rowIndex = row.rowIndex - 1;


          var TxtClientObject = mybutton.id.replace("ISHC", "txtSpecialHandlingCode");
          var TxtClientValue = TxtClientObject.value;

          window.open('ListMultipleSpecialHandlingCode.aspx?TargetTXT=' + TxtClientObject + '&txtValue=' + TxtClientValue, '',
          'left=' + (screen.availWidth / 5) + ',top=' + (screen.availHeight / 8) + 
          ',width=800,height=550,toolbar=0,resizable=0');
      }


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
<%--    
<asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>--%>
  
<div id="contentarea">
   <h1>Cargo Acceptance </h1>

<asp:Label ID="lblStatus" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
<div class="botline">
<table width="100%">
<tr>
<td>
        Flight</td>
    <td>
    <asp:TextBox ID="txtFlightID" runat="server" Width="35px" MaxLength="4"></asp:TextBox>
        <asp:TextBox ID="txtFlightNo" runat="server" Width="65px" MaxLength="6"></asp:TextBox>
        <asp:TextBox ID="txtFlightDate" runat="server" Width="74px" MaxLength="10"></asp:TextBox>
         <asp:CalendarExtender ID="txtFlightDate_CalendarExtender" Format="dd/MM/yyyy" runat="server" 
          Enabled="True" TargetControlID="txtFlightDate" PopupButtonID="imgDate">
          </asp:CalendarExtender>
        <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />  
    </td>
    <td>
       AWB</td>
    <td>
        <asp:TextBox ID="txtAWBPrefix" runat="server" Width="40px" MaxLength="4"></asp:TextBox>
        <asp:TextBox ID="txtAWBNumber" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
    </td>
    <td>Token</td>
<td>
    <asp:DropDownList ID="ddlTokenList" runat="server">
    </asp:DropDownList>
</td>
<td><asp:TextBox ID="txtTokenDt" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
<asp:CalendarExtender ID="CalendarExtender1" Format="dd/MM/yyyy" runat="server" 
          Enabled="True" TargetControlID="txtTokenDt" PopupButtonID="ImageButton1">
          </asp:CalendarExtender>
        <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />  
</td>
<td>Dock#</td>
<td><asp:TextBox ID="txtDockNo" runat="server" Width="100px"/></td>
    <td>Accpt. Status</td>  
        <td>
        <asp:DropDownList ID="ddlStatus" runat="server">
        <asp:ListItem Text="ALL" Value=""></asp:ListItem>
        <asp:ListItem Text="Partial" Value="P"></asp:ListItem>
        <asp:ListItem Text="Complete" Value="C"></asp:ListItem>
        </asp:DropDownList>
        </td> 
        <td>   
    
    <%--<td>
        AWBNumber</td>--%>
    </tr>
    </table>
 
<table>
<tr>
<td>
   <asp:Button ID="btnList" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_LIST %>" CssClass="button" 
        onclick="btnList_Click" />
        <asp:Button ID="btnClear" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>" CssClass="button" onclick="btnClear_Click" 
         />
</td>
</tr>
</table>

</div>
<div id="botlineGrid" class="botline" runat="server">
<h2>Select AWB</h2>
<div style="width:100%;overflow:auto;">   
<asp:GridView ID="grdAWBDetails" runat="server" 
        AlternatingRowStyle-CssClass="AltRowStyle" AutoGenerateColumns="false" 
     CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  PagerStyle-CssClass="PagerStyle" 
     PageSize="5" RowStyle-CssClass="RowStyle" 
        SelectedRowStyle-CssClass="SelectedRowStyle" AllowPaging="true" 
        onpageindexchanging="grdAWBDetails_PageIndexChanging" 
       >
     <Columns>
     
     <asp:TemplateField>
     <ItemTemplate>
       <asp:RadioButton ID="radSelectAWB" runat="server" GroupName="A" AutoPostBack="true" 
        onclick="javascript:RadioCheck(this);" OnCheckedChanged="RadioButton1_CheckedChanged"/>
     </ItemTemplate>
     </asp:TemplateField>
    
     <asp:TemplateField HeaderText="AWB">
     <ItemTemplate>
         <asp:Label ID="lblAWB" runat="server" Text='<%#Eval("AWBNumber")%>'></asp:Label></ItemTemplate></asp:TemplateField>
         <asp:TemplateField HeaderText="Flt #">
     <ItemTemplate>
         <asp:Label ID="lblFlightNo" runat="server" Text='<%#Eval("FlightNumber")%>'></asp:Label></ItemTemplate></asp:TemplateField>
         <asp:TemplateField HeaderText="Flt Dt">
     <ItemTemplate>
         <asp:Label ID="lblFlightDate" runat="server" Text='<%#Eval("FlightDate")%>'></asp:Label></ItemTemplate></asp:TemplateField>
         <asp:TemplateField HeaderText="Bk Pcs" ControlStyle-Width="50">
     <ItemTemplate>
       <asp:Label ID="lblSCMPcs" runat="server"  Text='<%#Eval("SCMPCS")%>'></asp:Label></ItemTemplate>
       </asp:TemplateField>
       <asp:TemplateField HeaderText="Bk Wgt" ControlStyle-Width="50">
     <ItemTemplate>
       <asp:Label ID="lblSCMWt" runat="server" Text='<%#Eval("SCMWT")%>'></asp:Label></ItemTemplate></asp:TemplateField>
       <asp:TemplateField HeaderText="Bk UOM" ControlStyle-Width="50">
     <ItemTemplate>
       <asp:Label ID="lblUOM"  runat="server" Text='<%#Eval("UOM")%>'></asp:Label>
       </ItemTemplate></asp:TemplateField>
         <asp:TemplateField HeaderText="SCM Bkd Pcs">
     <ItemTemplate>
       <asp:Label ID="lblPcs" Width="80px" runat="server"  Text='<%#Eval("PCS")%>'></asp:Label></ItemTemplate>
       </asp:TemplateField>
       <asp:TemplateField HeaderText="SCM Bkd Wt">
     <ItemTemplate>
       <asp:Label ID="lblWt" Width="80px"  runat="server" Text='<%#Eval("WT")%>'></asp:Label></ItemTemplate></asp:TemplateField>
       
       <asp:TemplateField HeaderText="Acc Pcs"  >
    <ItemTemplate>
        <asp:TextBox ID="txtAcceptedPcs" runat="server" Width="60px"  Text='<%#Eval("AcceptedPcs")%>' MaxLength="6"></asp:TextBox><asp:RegularExpressionValidator ID="regex_txtAcceptedPcs" runat="server" ErrorMessage="Only Digits" 
         ControlToValidate="txtAcceptedPcs" Display="Dynamic" ValidationExpression="^\d+\.?\d*$"></asp:RegularExpressionValidator>
         </ItemTemplate>
         </asp:TemplateField>
         
         <asp:TemplateField HeaderText="Acc Wt"  >
     <ItemTemplate>
        <asp:TextBox  ID="txtAcceptedWt" runat="server" Width="60px" Text='<%#Eval("AcceptedWt")%>' MaxLength="15"></asp:TextBox><asp:RegularExpressionValidator ID="regex_txtAcceptedWt" runat="server" ValidationExpression="^[-+]?[0-9]\d*\.?[0-9]*$" ErrorMessage="Float Only"
         ControlToValidate="txtAcceptedWt"  Display="Dynamic"></asp:RegularExpressionValidator></ItemTemplate></asp:TemplateField>
         <asp:TemplateField HeaderText="Pieces">
     <ItemTemplate>
       <asp:ImageButton ID="btnDimensionsPopup" runat="server" ImageUrl="~/Images/list_bullets.png"
        ImageAlign="AbsMiddle" CommandArgument='<%#Eval("AWBNumber")%>' CommandName="Dimension" OnClientClick="javascript:dimension(this);return false;"/>
        <%--<asp:HiddenField ID="HidRowIndex" runat="server" />--%></ItemTemplate></asp:TemplateField>
         
     
     <asp:TemplateField HeaderText="Rem Pcs" >
    <ItemTemplate>
        <asp:TextBox Enabled="false" ID="txtRcvPcs" Width="60px"  runat="server" Text='<%# Eval("RemainingPieces") %>'></asp:TextBox><%--    <asp:RegularExpressionValidator ID="regex_txtRcvPcs" runat="server" ErrorMessage="Only Digits" 
         ControlToValidate="txtRcvPcs" ValidationExpression="^\d+\.?\d*$"></asp:RegularExpressionValidator>--%></ItemTemplate></asp:TemplateField>
      <asp:TemplateField HeaderText="Rem Wt" >
     <ItemTemplate>
        <asp:TextBox Enabled="false" ID="txtRcvWt" runat="server" Width="60px"  Text='<%# Eval("RemainingWeight") %>'></asp:TextBox>
        <%--<asp:RegularExpressionValidator ID="regex_txtRcvWt" runat="server" ErrorMessage="Only Digits" 
         ControlToValidate="txtRcvWt" ValidationExpression="^\d+\.?\d*$"></asp:RegularExpressionValidator>--%>
         </ItemTemplate>
         </asp:TemplateField>
         <asp:TemplateField Visible="false">
     <ItemTemplate>
     <asp:Label Visible="false" ID="lblRemainingWt" Text='<%# Eval("RemainingWeight") %>' runat="server"></asp:Label><asp:Label Visible="false" ID="lblRemainingPcs" Text='<%# Eval("RemainingPieces") %>' runat="server"></asp:Label><asp:Label Visible="false" ID="lblULDNo" Text='<%# Eval("ULDNo") %>' runat="server"></asp:Label><asp:Label Visible="false" ID="lblDestination" Text='<%# Eval("Destination") %>' runat="server"></asp:Label></ItemTemplate></asp:TemplateField>
     <asp:TemplateField HeaderText="Location">
     <ItemTemplate>
     <asp:TextBox ID="txtLocation" runat="server" Text='<%#Eval("Location")%>' ></asp:TextBox></ItemTemplate></asp:TemplateField>
     <asp:TemplateField HeaderText="Commodity Code">
     <ItemTemplate>
         <asp:DropDownList ID="ddlCommCode" runat="server">
         </asp:DropDownList>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="SHC">
     <ItemTemplate>
          <asp:TextBox ID="txtSpecialHandlingCode" runat="server" CssClass="alignrgt"></asp:TextBox>
          
     </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
     <ItemTemplate>
     <asp:ImageButton ID="ISHC" runat="server"  ImageUrl="~/Images/list_bullets.png" OnClientClick="javascript:GetSpecialHandlingCode(this);return false;" />
     </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
     <ItemTemplate>
     <asp:HiddenField ID="hdCCSF" runat="server" Value='<%#Eval("CCSF")%>' />
     </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField HeaderText="UCR #" Visible="false">
     <ItemTemplate>
      <asp:Label ID="lblUCRNo" runat="server" Text='<%#Eval("UCRNo") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField Visible="false">
     <ItemTemplate>
     <asp:Label ID="lblTamper" Text='<%#Eval("IsTamper")%>' runat="server" ></asp:Label>
      <asp:Label ID="lblVisual" Text='<%#Eval("IsVisual")%>' runat="server" ></asp:Label>
       <asp:Label ID="lblAnimal" Text='<%#Eval("IsLiveAnimal")%>' runat="server" ></asp:Label>
        <asp:Label ID="lblSmell" Text='<%#Eval("IsSmell")%>' runat="server" ></asp:Label>
         <asp:Label ID="lblDGR" Text='<%#Eval("IsDGR")%>' runat="server" ></asp:Label>
          <asp:Label ID="lblPackage" Text='<%#Eval("IsPackaging")%>' runat="server" ></asp:Label>
           <asp:Label ID="lblPartner" Text='<%#Eval("Carrier")%>' runat="server" ></asp:Label>
           <asp:Label ID="lblIsPieces" Text='<%#Eval("IsPieces")%>' runat="server" ></asp:Label>
           <asp:Label ID="lblManifestStatus" Text='<%#Eval("ManifestStatus")%>' runat="server" ></asp:Label>
<%--            <asp:Label ID="lblCCSF" Text='<%#Eval("CCSF")%>' runat="server" ></asp:Label>
--%>         </ItemTemplate>
         </asp:TemplateField>
     </Columns>
     </asp:GridView>
     </div>
     </div>
     <div class="ltfloat" runat="server" id="AccpGridDIV">
<h2>Accepted Pieces</h2>
<asp:GridView ID="grdAcceptance" runat="server" AlternatingRowStyle-CssClass="AltRowStyle" AutoGenerateColumns="false" 
     CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  PagerStyle-CssClass="PagerStyle" 
     PageSize="15" RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
     <Columns>
     
     <asp:TemplateField>
      <HeaderTemplate>
           <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="javascript:SelectheaderCheckboxes(this);" />
      </HeaderTemplate>
     <ItemTemplate>
         <asp:CheckBox ID="chkAccept" runat="server" />
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Sr No" Visible="false">
     <ItemTemplate>
         <asp:Label ID="lblSrno" runat="server" Text='<%#Eval("SrNo")%>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Pieces Id">
     <ItemTemplate>
       <asp:Label ID="lblPcsId" runat="server" Text='<%#Eval("PieceId")%>'></asp:Label></ItemTemplate></asp:TemplateField><%-- <asp:TemplateField HeaderText="Exernal Piece Id">
     <ItemTemplate>
       <asp:Label ID="lblExtPcsId" runat="server" Text='<%#Eval("")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>--%>
     
     <asp:TemplateField HeaderText="Length">
     <ItemTemplate>
         <asp:TextBox ID="txtLgth" runat="server" Text='<%#Eval("Length")%>' Width="50px" AutoPostBack="true" OnTextChanged="CalculateVolume"></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Breadth">
     <ItemTemplate>
     <asp:TextBox ID="txtBreadth" runat="server" Text='<%#Eval("Breadth")%>' Width="50px" AutoPostBack="true" OnTextChanged="CalculateVolume"></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Height">
     <ItemTemplate>
     <asp:TextBox ID="txtHeight" runat="server" Text='<%#Eval("Height")%>' Width="50px" AutoPostBack="true" OnTextChanged="CalculateVolume"></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Volume">
     <ItemTemplate>
     <asp:TextBox ID="txtVol" runat="server" Text='<%#Eval("Volume")%>' Width="50px"></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Weight">
     <ItemTemplate>
     <asp:TextBox ID="txtWt" runat="server" Text='<%#Eval("Weight")%>' Width="50px"></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Copy">
     <ItemTemplate>
         <asp:TextBox ID="txtCopy" runat="server" Width="40px"></asp:TextBox><asp:Button ID="btnCopy" runat="server" Text="Copy" CssClass="button" OnClick="CopyDimensions" />
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Scale Weight">
     <ItemTemplate>
     <asp:TextBox ID="txtScaleWt" runat="server" Width="50px" Text='<%#Eval("ScaleWeight")%>' AutoPostBack="true" OnTextChanged="CalculateScaleWeight"></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="ULD#">
     <ItemTemplate>
     <asp:TextBox ID="txtULD" runat="server" Text='<%#Eval("ULDNo")%>' Width="50px"></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Piece Type">
     <ItemTemplate>
         <%--<asp:TextBox ID="txtPcType" runat="server" Text='<%#Eval("PieceType")%>' Width="50px"></asp:TextBox>--%>
         <asp:DropDownList ID="ddlPieceType" runat="server" Width="75px" CssClass="grdrowfont">
          <asp:ListItem Selected="True">Bulk</asp:ListItem><asp:ListItem>Bags</asp:ListItem><asp:ListItem>ULD</asp:ListItem></asp:DropDownList></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Bag#">
     <ItemTemplate>
         <asp:TextBox ID="txtBagNo" runat="server" Text='<%#Eval("BagNo")%>' Width="50px"></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Location">
     <ItemTemplate>
         <asp:TextBox ID="txtLocation" runat="server" Text='<%#Eval("Location")%>' Width="50px"></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Is Accepted" Visible="false">
     <ItemTemplate>
       <asp:Label ID="lblIsAccp" runat="server" Text='<%#Eval("isAccepted")%>'></asp:Label></ItemTemplate></asp:TemplateField></Columns></asp:GridView></div><br />

<div class="ltfloat" style="width:100%;" runat="server" id="AccpTaskDiv">
<h2>Acceptance Task Performed</h2><table width="100%" cellpadding="3" cellspacing="6">
<tr>
<td>
    <asp:CheckBox ID="chkTamper" runat="server" Text="Tamper"/>
    <asp:CheckBox ID="chkPackaging" runat="server" Text="Packaging" />
    <asp:CheckBox ID="chkVisual" runat="server" Text="Visual"/>
    <asp:CheckBox ID="chkSmell" runat="server" Text="Smell"/>
    <asp:CheckBox ID="chkDGR" runat="server" Text="DGR"/>
    <asp:CheckBox ID="chkLiveAnimal" runat="server" Text="Live Animal"/>
</td>
</tr>
<tr>
<td>
<table>
<tr>
<td>
    <asp:Button ID="btnSave" runat="server" CssClass="button" 
        onclick="btnSave_Click" Text="<%$ Resources:LabelNames, LBL_BTN_SAVE %>" Visible="False" OnClientClick="var b = CCSFValidate(); if (b) b = funcsum(); return b"/>
       <span style="vertical-align:bottom;">
       <asp:ImageButton ID="btnOpsTime" runat="server" ImageUrl="~/Images/timecalender.png" 
         Enabled="true" onclick="btnOpsTime_Click" CssClass="imgclock" /></span> 
         &nbsp;</td>
        <td>
             <asp:Button ID="btnPrint" runat="server" CssClass="button" Text="<%$ Resources:LabelNames, LBL_BTN_ACCEPTUCR %>" Enabled="false" OnClientClick="javascript:popup();return false;" />

         </td>
         <td>
    <asp:Button ID="btnCancel" runat="server" CssClass="button" 
        onclick="btnCancel_Click" Text="<%$ Resources:LabelNames, LBL_BTN_CANCEL %>" Visible="false" />
        </td>
        <td>
        <asp:Button ID="btnPRI" runat="server" Text="Send PRI" CssClass="button" 
                onclick="btnPRI_Click" />
        </td>
        </tr>
        </table>
    </td>
</tr>
</table>
</div>
<div style="display:none;">
<table>
<tr><td><table width="100%">
<tr>
<td>Total Volume</td><td>
    <asp:TextBox ID="txtTotVolCms" runat="server" Width="70"></asp:TextBox>Cubic Cms
</td>
<td>
    <asp:TextBox ID="txtTotVolMtr" runat="server" Width="70"></asp:TextBox>Cubic Mtrs</td><td></td>
</tr>
<tr>
<td>&nbsp;</td><td>
    &nbsp;</td><td>&nbsp;</td><td>
    &nbsp;</td></tr><tr>
<td>Total Volumetric Weight</td><td>
    <asp:TextBox ID="txtTotVolWt" runat="server" Width="70px"></asp:TextBox>Kg
</td>
<td>Total Scale Weight</td><td>
    <asp:TextBox ID="txtTotScaleWt" runat="server" Width="70px"></asp:TextBox></td></tr><tr>
<td>&nbsp;</td><td>
    &nbsp;</td><td>&nbsp;</td><td>
    &nbsp;</td></tr></table></td></tr><tr><td><%--<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
        onclick="btnSave_Click"/>
<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" 
        onclick="btnCancel_Click"/>--%>
<asp:Button ID="btnPrintUCR" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_PRINTUCR %>" CssClass="button" 
        OnClientClick="javascript:return popup();" Visible="false"/>

<asp:Button ID="btnReceipt" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CARGORECEIPT %>" CssClass="button" 
        onclick="btnReceipt_Click" Visible="false"/></td></tr>
</table>
</div>
    <asp:HiddenField ID="hdnPcsCount" runat="server" />
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        SelectMethod="GetData" 
        TypeName="ProjectSmartCargoManager.dsNOTC_NewTableAdapters.">
    </asp:ObjectDataSource>
    <asp:HiddenField ID="hdnWt" runat="server" />
    <asp:HiddenField ID="hdnTokenNo" runat="server" />
    <asp:HiddenField ID="hdnTokenDt" runat="server" />
    <asp:HiddenField ID="hdnAWBNo" runat="server" />
      <asp:HiddenField ID="hdStatus" runat="server" />
    <asp:HiddenField ID="hdDetailPcsCount" runat="server" />
    <asp:HiddenField ID="hdDetailWeight" runat="server" />
     <asp:HiddenField ID="hdULDNo" runat="server" />
     <asp:HiddenField ID="hdLocation" runat="server" />
        <asp:HiddenField ID="hdFlightNumber" runat="server" />
     <asp:HiddenField ID="hdFlightDate" runat="server" />
      <asp:HiddenField ID="hdUCRNo" runat="server" />
     
    <br />
     <asp:HiddenField ID="hdULDorigin" runat="server" />
    <br />
     <asp:HiddenField ID="hdULDDestination" runat="server" />
</div>

<div id="msglight" class="white_contentmsg">
        <table>
            <tr>
                <td width="5%" align="center">
                    <br />
                    <img src="Images/loading.gif" />
                    <br />
                    <asp:Label ID="msgshow" runat="server"></asp:Label></td></tr></table></div><div id="msgfade" class="black_overlaymsg">
    </div>
    
<%--</ContentTemplate>
<Triggers>
                    <asp:PostBackTrigger ControlID="btnPrint" EventName="Click" />
                </Triggers>
</asp:UpdatePanel>--%>
<div id="blackening" class="black_overlay"></div>      
<div id="divOpsTimePopup" class="white_content">
            <div style="margin:10px;">
            <asp:Label ID="lblPnlError" runat="server" ForeColor="Red"></asp:Label>
            
               <h3><asp:Label ID="lblOperationDetails" Text="Actual Operation Time" runat="server" Font-Bold="true" Font-Size="Larger"></asp:Label>
                </h3> 
            <hr />
           
            <div style="width:350px;">
            <table width="100%" cellpadding="3" cellspacing="3">
                <tr>
                    <td>
                        Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtOpsDate" runat="server" Width="80px"></asp:TextBox>
                        
                        <asp:ImageButton ID="ImageButton3" runat="server" ImageAlign="AbsMiddle" 
                             ImageUrl="~/Images/calendar_2.png" />
    
    </td><td style="width:70px;" >
                        <asp:TextBox ID="txtOpsTimeHr" runat="server" DataTextField="" Width="70px" 
                           ></asp:TextBox></td>
                           <td style="width:120px;" valign="bottom">
                            <asp:TextBox ID="txtOpsTimeMin" runat="server" DataTextField="" Width="70px"></asp:TextBox>
                            (HR:MI)</td>
                        
                        <td>                
                        <asp:CalendarExtender ID="txtOpsDate_CalendarExtender" Format="dd/MM/yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtOpsDate" PopupButtonID="ImageButton3" PopupPosition="BottomLeft">
                         </asp:CalendarExtender>
                        
                             <asp:NumericUpDownExtender ID="txtDeptTimeHr_NumericUpDownExtender" 
                                        runat="server" Maximum="23" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtOpsTimeHr" Width="40" />
                                        
                        <asp:NumericUpDownExtender ID="txtDeptTimeMin_NumericUpDownExtender1" 
                                        runat="server" Maximum="59" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtOpsTimeMin" Width="40" />
                        </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnOpsSave" runat="server" Text="Save" CssClass="button" onclick="btnOpsSave_Click" 
                              />
                    </td>
                    <td>&nbsp;</td>
                    <td>
                    <asp:Button ID="btnOpsCancel" runat="server" Text="Cancel" CssClass="button" 
                            onclick="btnOpsCancel_Click" />
                    </td>
              </tr>
            </table>
        </div>
	</div>
	    </div>

</asp:Content>
