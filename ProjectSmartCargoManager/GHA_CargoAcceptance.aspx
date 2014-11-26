<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GHA_CargoAcceptance.aspx.cs" Inherits="ProjectSmartCargoManager.GHA_CargoAcceptance" MasterPageFile="~/SmartCargoMaster.Master" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ContentPlaceHolderID="head" ID="Content1" runat="server">

<script type="text/javascript">

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

    function popup() {
        window.open('UCR.aspx', '', 'left=0,top=0,width=800,height=450,toolbar=0,resizable=0');
    }

    function SelectheaderCheckboxes(headerchk) {
        var gvcheck = document.getElementById("<%=grdAWBDetails.ClientID %>");
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
    function showDimension() {
        var awbno = document.getElementById("<%=hdnAWBNo.ClientID %>").value;
        window.open('GHA_CargoDimensions.aspx?AWBNo=' + awbno, '', 'left=200,top=200,width=850,height=300,toolbar=0,resizable=0');
            return false;

        }
</script>

</asp:Content>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content2" runat="server">
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
    
     <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
  
<div id="contentarea">
   <h1>Cargo Acceptance *</h1>

<asp:Label ID="lblStatus" runat="server" Font-Bold="True"></asp:Label>
<br />
<table>
<tr>
<td>Token#</td>
<td>
    <asp:DropDownList runat="server" ID="ddlTokenList">
    </asp:DropDownList>
</td>
<td></td>
<td>
   <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
        onclick="btnList_Click" />
</td>
</tr>
</table>
<div id="botlineGrid" class="botline" runat="server">
<h2>Select AWB</h2> 
    
<asp:GridView ID="grdAWBDetails" runat="server" AlternatingRowStyle-CssClass="AltRowStyle" AutoGenerateColumns="false" 
     CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  PagerStyle-CssClass="PagerStyle" Visible="true"
     PageSize="10" RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
     <Columns>
     
     <asp:TemplateField>
      <HeaderTemplate>
           <asp:CheckBox ID="chkboxSelectAllAWB" runat="server" onclick="javascript:SelectheaderCheckboxes(this);" />
      </HeaderTemplate>
     <ItemTemplate>
      <%-- <asp:RadioButton ID="radSelectAWB" runat="server" GroupName="A" AutoPostBack="true" 
       oncheckedchanged="RadioButton1_CheckedChanged" onclick="javascript:RadioCheck(this)"/>--%>
         <asp:CheckBox ID="chkAcceptAWB" runat="server"/>
     </ItemTemplate>
     </asp:TemplateField>
    
     <asp:TemplateField HeaderText="AWB">
     <ItemTemplate>
         <asp:Label ID="lblAWB" runat="server" Text='<%#Eval("AWBNumber")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Pieces">
     <ItemTemplate>
       <asp:Label ID="lblPcs" runat="server" Text='<%#Eval("PCS")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField HeaderText="Weight">
     <ItemTemplate>
       <asp:Label ID="lblWt" runat="server" Text='<%#Eval("WT")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Location">
     <ItemTemplate>
     <asp:TextBox ID="txtLoc" runat="server" Text="ABC" Width="50px"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField HeaderText="Commodity Code">
     <ItemTemplate>
         <asp:DropDownList ID="ddlCommCode" runat="server">
         </asp:DropDownList>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField HeaderText="SHC">
     <ItemTemplate>
       <asp:DropDownList ID="ddlSHC" runat="server">
         </asp:DropDownList>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField HeaderText="Received Pcs">
     <ItemTemplate>
         <asp:TextBox ID="txtRcvPcs" runat="server" Width="50px" Text='<%#Eval("AcceptedPcs")%>'></asp:TextBox>
         <asp:RegularExpressionValidator ID="regex_txtRcvPcs" runat="server" ErrorMessage="Only Digits" 
         ControlToValidate="txtRcvPcs" ValidationExpression="^\d*$"></asp:RegularExpressionValidator>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField HeaderText="Received Pcs">
     <ItemTemplate>
         <asp:TextBox ID="txtRcvWt" runat="server" Width="70px" Text='<%#Eval("AcceptedWt")%>'></asp:TextBox>
         <asp:RegularExpressionValidator ID="regex_txtRcvWt" runat="server" ErrorMessage="Only Digits" 
         ControlToValidate="txtRcvWt" ValidationExpression="^\d+\.?\d*$"></asp:RegularExpressionValidator>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Dimensions">
     <ItemTemplate>
       <asp:ImageButton ID="btnDimensionsPopup" runat="server" ImageUrl="~/Images/list_bullets.png"
        ImageAlign="AbsMiddle" OnClick="btnShowFlights_Click" />
        <%--<asp:HiddenField ID="HidRowIndex" runat="server" />--%>
     </ItemTemplate>
     </asp:TemplateField>
     
       <%--<asp:TemplateField HeaderText="Piece Type">
     <ItemTemplate>
         <asp:DropDownList ID="ddlShipmentType" runat="server" Width="75px" CssClass="grdrowfont">
          <asp:ListItem Selected="True">Bulk</asp:ListItem>
          <asp:ListItem>Bags</asp:ListItem>
          <asp:ListItem>ULD</asp:ListItem>                                                        
          </asp:DropDownList>
     </ItemTemplate>
     </asp:TemplateField--%>
     
     </Columns>
     </asp:GridView>
     <br />
    <asp:Button ID="btnSaveNew" runat="server" Text="Save" CssClass="button" 
        onclick="btnSaveNew_Click" />
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
         <asp:Label ID="lblSrno" runat="server" Text='<%#Eval("SrNo")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Pieces Id">
     <ItemTemplate>
       <asp:Label ID="lblPcsId" runat="server" Text='<%#Eval("PieceId")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <%-- <asp:TemplateField HeaderText="Exernal Piece Id">
     <ItemTemplate>
       <asp:Label ID="lblExtPcsId" runat="server" Text='<%#Eval("")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>--%>
     
     <asp:TemplateField HeaderText="Length">
     <ItemTemplate>
         <asp:TextBox ID="txtLgth" runat="server" Text='<%#Eval("Length")%>' Width="50px" AutoPostBack="true" OnTextChanged="CalculateVolume"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Breadth">
     <ItemTemplate>
     <asp:TextBox ID="txtBreadth" runat="server" Text='<%#Eval("Breadth")%>' Width="50px" AutoPostBack="true" OnTextChanged="CalculateVolume"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Height">
     <ItemTemplate>
     <asp:TextBox ID="txtHeight" runat="server" Text='<%#Eval("Height")%>' Width="50px" AutoPostBack="true" OnTextChanged="CalculateVolume"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Volume">
     <ItemTemplate>
     <asp:TextBox ID="txtVol" runat="server" Text='<%#Eval("Volume")%>' Width="50px"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField HeaderText="Weight">
     <ItemTemplate>
     <asp:TextBox ID="txtWt" runat="server" Text='<%#Eval("Weight")%>' Width="50px"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Copy">
     <ItemTemplate>
         <asp:TextBox ID="txtCopy" runat="server" Width="40px"></asp:TextBox>
         <asp:Button ID="btnCopy" runat="server" Text="Copy" CssClass="button" OnClick="CopyDimensions" />
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Scale Weight">
     <ItemTemplate>
     <asp:TextBox ID="txtScaleWt" runat="server" Width="50px" Text='<%#Eval("ScaleWeight")%>' AutoPostBack="true" OnTextChanged="CalculateScaleWeight"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="ULD#">
     <ItemTemplate>
     <asp:TextBox ID="txtULD" runat="server" Text='<%#Eval("ULDNo")%>' Width="50px"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField HeaderText="Piece Type">
     <ItemTemplate>
         <%--<asp:TextBox ID="txtPcType" runat="server" Text='<%#Eval("PieceType")%>' Width="50px"></asp:TextBox>--%>
         <asp:DropDownList ID="ddlPieceType" runat="server" Width="75px" CssClass="grdrowfont">
          <asp:ListItem Selected="True">Bulk</asp:ListItem>
          <asp:ListItem>Bags</asp:ListItem>
          <asp:ListItem>ULD</asp:ListItem>                                                        
          </asp:DropDownList>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField HeaderText="Bag#">
     <ItemTemplate>
         <asp:TextBox ID="txtBagNo" runat="server" Text='<%#Eval("BagNo")%>' Width="50px"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField HeaderText="Location">
     <ItemTemplate>
         <asp:TextBox ID="txtLocation" runat="server" Text='<%#Eval("Location")%>' Width="50px"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Is Accepted" Visible="false">
     <ItemTemplate>
       <asp:Label ID="lblIsAccp" runat="server" Text='<%#Eval("isAccepted")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     </Columns>
    </asp:GridView>
</div>

<br />

<div class="ltfloat" style="width:100%;" runat="server" id="AccpTaskDiv">
<h2>Acceptance Task Performed</h2>
<table width="100%" cellpadding="3" cellspacing="6">
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
    &nbsp;</td>
</tr>
<tr><td><table width="100%">
<tr>
<td>Total Volume</td>
<td>
    <asp:TextBox ID="txtTotVolCms" runat="server" Width="70"></asp:TextBox>
    Cubic Cms
</td>
<td>
    <asp:TextBox ID="txtTotVolMtr" runat="server" Width="70"></asp:TextBox>
Cubic Mtrs</td><td></td>
</tr>
<tr>
<td>&nbsp;</td>
<td>
    &nbsp;</td>
<td>&nbsp;</td>
<td>
    &nbsp;</td>
</tr>
<tr>
<td>Total Volumetric Weight</td>
<td>
    <asp:TextBox ID="txtTotVolWt" runat="server" Width="70px"></asp:TextBox>
    Kg
</td>
<td>Total Scale Weight</td>
<td>
    <asp:TextBox ID="txtTotScaleWt" runat="server" Width="70px"></asp:TextBox>
</td>
</tr>
<tr>
<td>&nbsp;</td>
<td>
    &nbsp;</td>
<td>&nbsp;</td>
<td>
    &nbsp;</td>
</tr>
</table></td></tr>
<tr><td><asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
        onclick="btnSave_Click"/>
<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button"/>
<asp:Button ID="btnPrintUCR" runat="server" Text="Print UCR" CssClass="button" OnClientClick="javascript:return popup();" />

<asp:Button ID="btnReceipt" runat="server" Text="Cargo Receipt" CssClass="button"/></td></tr>
</table>
</div>
    <asp:HiddenField ID="hdnPcsCount" runat="server" />
    <asp:HiddenField ID="hdnWt" runat="server" />
    <asp:HiddenField ID="hdnTokenNo" runat="server" />
    <asp:HiddenField ID="hdnTokenDt" runat="server" />
    <asp:HiddenField ID="hdnAWBNo" runat="server" />
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
