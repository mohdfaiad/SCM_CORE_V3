<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShipperMaster.aspx.cs" Inherits="ProjectSmartCargoManager.ShipperMaster" MasterPageFile="~/SmartCargoMaster.Master"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <style>
.styleUpper
        {
            text-transform: uppercase;
        }
</style>

 <script type="text/javascript">
        function GetAgentCode(obj) {
            var destination = obj;
            var AgentName = document.getElementById('<%= txtAgentName.ClientID%>');
            var CustCode = document.getElementById('<%= TXTCustomerCode.ClientID%>');

            if (destination.value.indexOf("(") > 0) {
                if (destination.value.length > 4) {
                    var start = destination.value.indexOf("(");
                    var end = destination.value.indexOf(")");
                    var str = destination.value;
                    var CustStart = destination.value.indexOf("$");

                    destination.value = str.substring(0, start);
                    AgentName.value = str.substring(start + 1, end);
                    CustCode.value = str.substring(CustStart + 1, str.length);
                }
            }
        }

        function onListPopulated() {

            var completionList = $find("AutoCompleteEx").get_completionList();
            completionList.style.width = 'auto';
        }
    </script>
<title></title>
</asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:UpdatePanel ID="UP1" runat="server">
<ContentTemplate>
<asp:ToolkitScriptManager ID="ScriptMgr1" runat="server"></asp:ToolkitScriptManager>

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
<div id="contentarea">

<div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>

<h1>Shipper-Consignee Master</h1>


<div class="botline">
<table>
<tr>
<td>Acc Code</td>
<td><asp:TextBox ID="txtAccCode" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID="ext_txtAccCode" runat="server" Display="Dynamic" ErrorMessage="Please enter AccountCode" ControlToValidate="txtAccCode"></asp:RequiredFieldValidator>
</td>
<td>Acc Name</td>
<td><asp:TextBox ID="txtAccName" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID="ext_txtAccName" runat="server" ErrorMessage="Please enter AccountName" ControlToValidate="txtAccName"></asp:RequiredFieldValidator>
</td>
<td>Credit Acc No</td>
<td>
    <asp:TextBox ID="txtCreditAccNo" runat="server" Width="100px"></asp:TextBox>
</td>
</tr>
<tr>
<td>Address 1</td>
<td><asp:TextBox ID="txtAdr1" runat="server"></asp:TextBox>
<asp:RequiredFieldValidator ID="ext_txtAdr1" runat="server" Display="Dynamic" ErrorMessage="Please enter Address" ControlToValidate="txtAdr1"></asp:RequiredFieldValidator>
</td>
<td>Address 2</td>
<td><asp:TextBox ID="txtAdr2" runat="server"></asp:TextBox>
</td>
<td>Participation Type</td>
<td>
<asp:DropDownList ID="ddlType" runat="server">
<asp:ListItem Text="Select"></asp:ListItem>
<asp:ListItem Text="Shipper" Value="S"></asp:ListItem>
<asp:ListItem Text="Consignee" Value="C"></asp:ListItem>
<asp:ListItem Text="Both" Value="SC"></asp:ListItem>
</asp:DropDownList>
</td>
</tr>
<tr>
<td>City</td>
<td><asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
<%--<asp:RequiredFieldValidator ID="ext_txtCity" runat="server" Display="Dynamic" ErrorMessage="Please enter city" ControlToValidate="txtCity"></asp:RequiredFieldValidator>--%></td>
<td>State</td>
<td><asp:TextBox ID="txtState" runat="server"></asp:TextBox>
<%--<asp:RequiredFieldValidator ID="ext_txtState" runat="server" Display="Dynamic" ErrorMessage="Please enter state" ControlToValidate="txtState"></asp:RequiredFieldValidator>--%></td>
<td>Agent Code</td>
<td>
 <asp:TextBox ID="TXTAgentCode" runat="server" Width="110px"
TabIndex="4" CssClass="styleUpper" onchange="GetAgentCode(this)"></asp:TextBox>
<asp:AutoCompleteExtender ID="ACEAgentCode"  runat="server"
ServiceMethod="GetAgentCodeWithName" ScriptPath="ShipperMaster.aspx"  EnableCaching="true" CompletionInterval="1000" TargetControlID="TXTAgentCode" MinimumPrefixLength="1">
</asp:AutoCompleteExtender>
</td>
</tr>
<tr>
<td>Country</td>
<td>
<%--<asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>--%>
<asp:DropDownList ID="ddlCountry" runat="server" Width="138px"/>
<asp:RequiredFieldValidator ID="ext_txtCountry0" runat="server" Display="Dynamic" ErrorMessage="Please enter Country"
        ControlToValidate="ddlCountry"></asp:RequiredFieldValidator></td>
<td>Zip Code</td>
<td><asp:TextBox ID="txtZipCode" runat="server" MaxLength="6"></asp:TextBox>
<%--<asp:RequiredFieldValidator ID="ext_txtZipCode0" runat="server" Display="Dynamic" ErrorMessage="Please enter ZipCode" 
        ControlToValidate="txtZipCode"></asp:RequiredFieldValidator>--%>
<asp:RegularExpressionValidator ID="zipValidator0" runat="server" 
ErrorMessage="Only Digits" ControlToValidate="txtZipCode" 
        ValidationExpression="^\d{1,}$"></asp:RegularExpressionValidator>
</td>
<td>Agent Name</td>
<td><asp:TextBox ID="txtAgentName" runat="server" 
        Width="110px" Enabled="false"></asp:TextBox>
</td>
</tr>
<tr>
<td>Mobile No.</td>
<td>
    <asp:TextBox ID="txtMobNo" runat="server" MaxLength="10"></asp:TextBox>
    </td>
<td>Phone No.</td>
<td>
    <asp:TextBox ID="txtPhNo" runat="server" MaxLength="10"></asp:TextBox>
    <asp:RequiredFieldValidator ID="ext_txtPhNo" runat="server" 
        ControlToValidate="txtPhNo" Display="Dynamic" ErrorMessage="Please enter Phone No"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="phValidator" runat="server" 
        ControlToValidate="txtPhNo" ErrorMessage="Only Digits" 
        ValidationExpression="^\d{1,}$"></asp:RegularExpressionValidator>
    </td>
<td>Customer Code</td>
<td>
    <asp:TextBox ID="TXTCustomerCode" runat="server" Enabled="false" Width="110px"></asp:TextBox>
    <br />
    </td>
</tr>
<tr>
<td>Fax</td>
<td><asp:TextBox ID="txtFax" runat="server" MaxLength="10"></asp:TextBox>
</td>
<td>Email</td>
<td><asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
</td>
<td>
            CCSF Code</td>
        <td>
            <asp:DropDownList ID="ddlCCSFCode" runat="server">
            <asp:ListItem Text="Select"></asp:ListItem>
            </asp:DropDownList>
        </td>
</tr>
<tr>
<td>IATA Acc No</td>
<td><asp:TextBox ID="txtIATANo" runat="server"></asp:TextBox>
</td>
<td>TIN</td>
<td>
    <asp:TextBox ID="txttin" runat="server" MaxLength="50"></asp:TextBox>
</td>
<td>
Conatct Person

</td>
<td>
<asp:TextBox ID="txtperson" runat="server"></asp:TextBox>
</td>
</tr>
<tr>
<td><asp:CheckBox ID="chkAct" runat="server" Text="Active" /></td>
<td>
    <asp:CheckBox ID="chkKnownShipper" runat="server" Text="Known Shipper" />
</td>
<td>
    <asp:CheckBox ID="chkVAT" runat="server" Text="TAX Exempt" />
</td>
</tr>
<tr>
<td>
<%--<asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="button" onclick="btnSave_Click"/>
--%>
<asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="button" onclick="btnSave_Click1" />
       
        <asp:Button ID="btnList" runat="server" CausesValidation="false" 
            CssClass="button" onclick="btnList_Click" Text="List" />
    
     <asp:Button ID="btnExport" runat="server" CausesValidation="false" 
            CssClass="button"  Text="Export" onclick="btnExport_Click" />
            
        <asp:Button ID="btnClear" runat="server" CausesValidation="false" 
            CssClass="button" onclick="btnClear_Click" Text="Clear" />
  
        </tr></td>
  
</tr>
   
</table>
</div>
<div class="ltfloat" style="overflow:auto;width:1024px">
<asp:GridView ID="GrdShipper" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
onrowcommand="GrdShipper_RowCommand" onrowediting="GrdShipper_RowEditing"
onpageindexchanging="GrdShipper_PageIndexChanging" Width="100%"  HeaderStyle-CssClass="titlecolr">
<Columns>
<asp:TemplateField HeaderText="Serial Number" Visible="false" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblSrNo" runat="server" Text='<%#Eval("SerialNumber")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Account Code" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblAccCode" runat="server" Text='<%#Eval("AccountCode")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Account Name" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblAccName" runat="server" Text='<%#Eval("AccountName")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Address 1" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblAdr1" runat="server" Text='<%#Eval("Adress1")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Address 2" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblAdr2" runat="server" Text='<%#Eval("Adress2")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="City" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblCity" runat="server" Text='<%#Eval("City")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="State" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblState" runat="server" Text='<%#Eval("State")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Country" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblCountry" runat="server" Text='<%#Eval("Country")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Zip" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblZip" runat="server" Text='<%#Eval("ZipCode")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Phone No." ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblPhNo" runat="server" Text='<%#Eval("PhoneNumber")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Mobile No." ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblMobNo" runat="server" Text='<%#Eval("MobileNumber")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Fax" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblFax" runat="server" Text='<%#Eval("Fax")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Email" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="IATA Account No" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblIATANo" runat="server" Text='<%#Eval("IATAAccountNo")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Participation Type" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblParticipation" runat="server" Text='<%#Eval("ParticipationType")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="CCSF Code" Visible="false" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="CCSFCode" runat="server" Text='<%#Eval("CCSFCode")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Active" Visible="false" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblAct" runat="server" Text='<%#Eval("IsActive")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Active" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblshipper" runat="server" Text='<%#Eval("IsShipper")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Created On" Visible="false" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblCreateOn" runat="server" Text='<%#Eval("CreatedOn")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Created By" Visible="false" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblCreateBy" runat="server" Text='<%#Eval("CreatedBy")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Updated On" Visible="false" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblUpdateOn" runat="server" Text='<%#Eval("UpdatedOn")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Updated By" Visible="false" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblUpdateBy" runat="server" Text='<%#Eval("UpdatedBy")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Agent Code" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblAgentCode" runat="server" Text='<%#Eval("AgentCode")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Credit Account No" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblCreditAccNo" runat="server" Text='<%#Eval("CreditAccNo")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>


<asp:TemplateField HeaderText="TIN" Visible="true" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lbltin" runat="server" Text='<%#Eval("TIN")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Contact Person" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblContactPerson" runat="server" Text='<%#Eval("ContactPerson")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="VAT Exemption" ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<asp:Label ID="lblVATExemption" runat="server" Text='<%#Eval("TAXExemption")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>


<asp:ButtonField CommandName="Edit" Text="Edit">
<ItemStyle Width="50px"/>
</asp:ButtonField>
<asp:ButtonField CommandName="DeleteRecord" Text="Delete">
<ItemStyle Width="50px"/>
</asp:ButtonField>
</Columns>
</asp:GridView>
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
</div>

</ContentTemplate>
 <Triggers>
                <asp:PostBackTrigger ControlID="btnExport"  />
                
                </Triggers>
</asp:UpdatePanel>
</asp:Content>
