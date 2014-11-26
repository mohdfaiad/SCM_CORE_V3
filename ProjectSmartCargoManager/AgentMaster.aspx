<%--2012/05/23 vinayak.
    2012/05/31 vinayak
--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentMaster.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.AgentMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function InsertFailure() {

            alert("Insertion Failed Please Try Again");

        }
        function ValidatioofGreater() {
            alert("From Must be Small than To");

        }
        function InsertValue() {
            alert("Please insert Values..");
        }

        function EnterData() {

            alert("Please Enter Data");

        }
        function NumericValidation() {

            alert("Please Insesrt Numeric Value");

        }
        function selectrow() {

            alert("Please Select a Row");

        }

        //        for new code


        function echeck(str) {

            var at = "@";
            var dot = ".";
            var lat = str.indexOf(at);
            var lstr = str.length;
            var ldot = str.indexOf(dot);
            if (str.indexOf(at) == -1) {
                alert("Invalid E-mail ID");
                return false;
            }

            if (str.indexOf(at) == -1 || str.indexOf(at) == 0 || str.indexOf(at) == lstr) {
                alert("Invalid E-mail ID:" + str + "");
                return false;
            }

            if (str.indexOf(dot) == -1 || str.indexOf(dot) == 0 || str.indexOf(dot) == lstr) {
                alert("Invalid E-mail ID:" + str + "");
                return false;
            }

            if (str.indexOf(at, (lat + 1)) != -1) {
                alert("Invalid E-mail ID:" + str + "");
                return false;
            }

            if (str.substring(lat - 1, lat) == dot || str.substring(lat + 1, lat + 2) == dot) {
                alert("Invalid E-mail ID:" + str + "");
                return false;
            }

            if (str.indexOf(dot, (lat + 2)) == -1) {
                alert("Invalid E-mail ID:" + str + "");
                return false;
            }

            if (str.indexOf(" ") != -1) {
                alert("Invalid E-mail ID");
                return false;
            }

            return true;
        }

        function LTrim(value) {

            var re = /\s*((\S+\s*)*)/;
            return value.replace(re, "$1");

        }

        // Removes ending whitespaces
        function RTrim(value) {

            var re = /((\s*\S+)*)\s*/;
            return value.replace(re, "$1");

        }

        // Removes leading and ending whitespaces
        function trim(value) {

            return LTrim(RTrim(value));

        }
        function SetAll() {
            if (document.getElementById("<%= chkAll.ClientID%>").checked) {
                document.getElementById("<%= chkIATAChrg.ClientID%>").checked = true;
                document.getElementById("<%= chkMKTChrg.ClientID%>").checked = true;
                document.getElementById("<%= chkCommission.ClientID%>").checked = true;
                document.getElementById("<%= chkServTax.ClientID%>").checked = true;
                document.getElementById("<%= chkDiscount.ClientID%>").checked = true;

            } else {
            document.getElementById("<%= chkIATAChrg.ClientID%>").checked = false;
            document.getElementById("<%= chkMKTChrg.ClientID%>").checked = false;
            document.getElementById("<%= chkCommission.ClientID%>").checked = false;
            document.getElementById("<%= chkServTax.ClientID%>").checked = false;
            document.getElementById("<%= chkDiscount.ClientID%>").checked = false;
             }
            return false;
         }
        function checkMailIds(id, id2) {
            debugger;
            var cnt = 0;
            var mailids = id.value;
            var str = mailids.split(",");

            for (var i = 0; i < str.length; i++) {
                var obk = str[i];
                var obk = trim(obk);
                //alert(obk);
                if (echeck(obk) == false) {

                    id2.disabled = true;
                    return false;

                }
                else {

                    cnt = parseInt(cnt) + 1;
                    if (cnt == str.length) {

                        id2.disabled = false;
                        return true;
                    }

                    //return true;
                }



            }

        }

        function chktxt() {

            var txtemail = document.getElementById('< %=strtxtemail% >');
            if (txtemail.value == "") {
                alert("Enter Emailid");
                return false;
            }
            else {
                return true;
            }
        }

        //

        function setAgentName(DropDownList) {
            var strValue = DropDownList.options[DropDownList.selectedIndex].value;
            document.getElementById('ctl00_ContentPlaceHolder1_TXTAgentCode').value = strValue;
        }

        function Insert() {
            alert("Record Inserted Succssfully");
        }
        function SelectRow() {
            alert("Please Select Row for Save");
        }

        function Check() {
            alert("Invoice Value Does Not Greater Than Credit Value");
        }

        function Check() {
            alert("Invoice Value Does Not Greater Than Credit Value");
        }
        function ValidatioofGreater() {
            alert("From Must be Small than To");
        }
        function NoRecord() {
            alert("No Record Found Please Insert Data..");
        }
        function EmptyMsg() {
            alert("Please Fill Data");
        }
        function EmptyDataMsg(City) {
            alert("No City Available With This " + City);
        }

        function validate(form_id, email) {

            var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
            var address = document.forms[form_id].elements[email].value;
            if (reg.test(address) == false) {

                alert('Invalid Email Address');
                return false;
            }
        }

        function validateEmail(field) {
            var regex = /\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b/i;
            return (regex.test(field)) ? true : false;
        }

        function validateMultipleEmailsCommaSeparated(value) {
            var result = value.split(",");
            for (var i = 0; i < result.length; i++)
                if (!validateEmail(result[i]))
                alert('Invalid Mail ID');
            return true;
        }




        //        function validateMultipleEmailsCommaSeparated(value)
        //         {
        //             var result = value.split(","); for (var i = 0; i < result.length; i++) if (!validateEmail(result[i]))
        //                 return false; return true; 
        //         }

        function validateEmail(field, rules, i, options) {
            if (!_.all(field.val().split(","), function(candidate) {
                return $.trim(candidate).match(options.allrules.email.regex);
            }))
                return options.allrules.email.alertText;
        }

        function expandcollapse(obj, row) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);

            if (div.style.display == "none") {
                div.style.display = "block";
                if (row == 'alt') {
                    img.src = ".//Images/minus.gif";
                }
                else {
                    img.src = ".//Images/minus.gif";
                }
                img.alt = "Close to view other Customers";
            }
            else {
                div.style.display = "none";
                if (row == 'alt') {
                    img.src = "plus.gif";
                }
                else {
                    img.src = "plus.gif";
                }
                img.alt = "Expand to show Orders";
            }
        }

        function SelectAllStations(chkObj) {

            var multi = document.getElementById("<%=chkListStation.ClientID %>");
            var chkBoxCnt = multi.getElementsByTagName("input");

            if (document.getElementById("<%=chkSelectAll.ClientID %>").checked) {
                for (i = 0; i < chkBoxCnt.length; i++) {
                    chkBoxCnt[i].checked = true;
                }
                return false;
            }
            else {
                for (i = 0; i < chkBoxCnt.length; i++) {
                    chkBoxCnt[i].checked = false;
                }
                return false;
            }
            return false;
        }

        function SetAgentCode() {
            var AgentCode = document.getElementById("<%= TXTAgentCode.ClientID%>").value;
            document.getElementById("<%= txtIATAcode.ClientID%>").value = AgentCode;
            document.getElementById("<%= txtcustomercode.ClientID%>").value = AgentCode;
            return false;
        }
    </script>
    
    <script type="text/javascript">
        function ViewPanelSplit_Agent() {
            document.getElementById('DivAgentPopUp').style.display = 'block';
            document.getElementById('DivAgentPopUp1').style.display = 'block';
            return false;
        }
        function HidePanelSplit_Agent() {
            document.getElementById('DivAgentPopUp').style.display = 'none';
            document.getElementById('DivAgentPopUp1').style.display = 'none';
            return false;
        }
    </script>
    
    <script type="text/javascript">
        function ViewPanelSplit_EAWBPopUp() {
            document.getElementById('AgentPopUp').style.display = 'block';
            document.getElementById('AgentPopUp1').style.display = 'block';
            return false;
        }
        function HidePanelSplit_EAWBPopUp() {
            document.getElementById('AgentPopUp').style.display = 'none';
            document.getElementById('AgentPopUp1').style.display = 'none';
            return false;
        }
        function GetPayMode() {

            var TxtClientObject = '<%= txtAllowPayMode.ClientID %>';
            var value = document.getElementById('<%= txtAllowPayMode.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesParam&param=PayMode&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');

        }
    </script>

    <style type="text/css">
        .black_overlay
        {
            display: none;
            position:fixed;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
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
            position:fixed;
            top: 30%;
            left: 30%;
            width: 30%;
            height: 45%;
            padding: 16px;
            border: 16px solid #ccdce3;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
        
    </style>

   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:ToolkitScriptManager runat="server">
</asp:ToolkitScriptManager>

<div id="contentarea">
<div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
<h1>Agent Master</h1>
        
<div class="botline">

<table width="67%" border="0">
                <tr>
                    <td>
                        Agent Code*
                    </td>
                    <td>
                      <asp:TextBox ID="TXTAgentCode" runat="server" Width="110px" ToolTip="Agent Code" onchange="javascript:SetAgentCode();return false;"></asp:TextBox>
                        <asp:TextBoxWatermarkExtender ID="TXTAgentCode_TextBoxWatermarkExtender" runat="server" TargetControlID="TXTAgentCode" WatermarkText ="Agent Code">
                        </asp:TextBoxWatermarkExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="TXTAgentCode" ErrorMessage="*" Font-Bold="True" Font-Italic="False"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        Agent Name*
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="TXTNewAgentName" runat="server"> </asp:TextBox>
                        <asp:TextBoxWatermarkExtender ID="TXTNewAgentName_TextBoxWatermarkExtender" runat="server" TargetControlID="TXTNewAgentName" WatermarkText="Agent Name" >
                        </asp:TextBoxWatermarkExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TXTNewAgentName" ErrorMessage="*" Font-Bold="True" Font-Italic="False"> </asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="txtstationcode" runat="server" Enabled="False" Visible="False" Width="1px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnListAgent" runat="server" Text="List" CssClass="button" 
                            onclick="btnListAgent_Click" CausesValidation="False" />
                        <asp:Button ID="btngeneralclear" runat="server" CssClass="button" OnClick="btngeneralclear_Click" Text="Clear" CausesValidation="false"  />
                        <asp:Button ID="btnListInfo" runat="server" Text="List" CssClass="button" OnClick="btnListInfo_Click" CausesValidation="false" Visible="false" />
                        &nbsp;
                    </td>
                    
                </tr>
            </table>
            
            
        </div>
        <br />
        <div style="font-family:Calibri, Arial, Helvetica, sans-serif;">
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
            Width="1022px"  >
            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1" Width="620px"
                Height="27px">
                <HeaderTemplate>
                    General
                </HeaderTemplate>
                
                <ContentTemplate>
                    <asp:Panel ID="Panel1" runat="server">
                        <h4>
                            Customer Information
                        </h4>
                        <table width="100%" cellpadding="1" cellspacing="3">
                                
                            <tr>
                               
                                <td>Vaild From *</td>
                                <td>
                                    <asp:TextBox ID="txtvalidfrom" runat="server" Width="75px"></asp:TextBox>
                                    <asp:ImageButton ID="imgFrmDt" runat="server" CausesValidation="False" 
                                        ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" 
                                        Format="dd/MM/yyyy" PopupButtonID="imgFrmDt" TargetControlID="txtvalidfrom">
                                    </asp:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" 
                                        ControlToValidate="txtvalidfrom" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    Valid To *</td>
                                <td>
                                    <asp:TextBox ID="txtvalidto" runat="server" Width="75px"></asp:TextBox>
                                    <asp:ImageButton ID="imgToDt" runat="server" CausesValidation="False" 
                                        ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" 
                                        Format="dd/MM/yyyy" PopupButtonID="imgToDt" TargetControlID="txtvalidto">
                                    </asp:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" 
                                        ControlToValidate="txtvalidto" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                    
                                    <td>
                                    &nbsp;</td>
                                    <td>
                                    IATA Agent Code *</td>
                                <td>
                                    <asp:TextBox ID="txtIATAcode" runat="server" 
                                        ToolTip="IATA Agent Code Must be same as Agent Code" ValidationGroup="Gen"></asp:TextBox></td>
                                </tr>
                             
                        <tr>
                             <td>Agent Type</td>
                                <td>
                                     <asp:DropDownList ID="ddlAgentType" runat="server" Width="90px">
                                        <asp:ListItem>Select</asp:ListItem>
                                        <asp:ListItem>Domestic</asp:ListItem>
                                        <asp:ListItem>International</asp:ListItem>
                                     </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    Airport Code *</td>
                                <td>
                                    <asp:DropDownList ID="ddlcity" runat="server" ToolTip="Select City Of Agent" 
                                        Width="120px" AutoPostBack="True" 
                                        onselectedindexchanged="ddlcity_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>Country<br />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcountry" runat="server" ToolTip="Select Country" Width="170px" onselectedindexchanged="ddlcountry_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                </tr>
                                
                            <tr>
                                <td>
                                    Customer Code<br />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtcustomercode" runat="server" 
                                        ToolTip="Customer Code Same As Agent Code" ValidationGroup="Gen"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    Contact Person
                                </td>
                                <td>
                                    <asp:TextBox ID="txtcontactperson" runat="server" ToolTip="Contact Person Name"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    Address
                                    <br />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtadress" runat="server" Height="25px" TextMode="MultiLine" 
                                        ToolTip="Enter Adress"></asp:TextBox>
                                </td>
                            </tr>
                             
                             <tr>
                                <td>
                                    Is this Controlling Locator ? *</td>
                                <td>
                                    <asp:DropDownList ID="ddlcontrolinglocator" runat="server" AutoPostBack="True" 
                                        OnSelectedIndexChanged="ddlcontrolinglocator_SelectedIndexChanged" Width="80px">
                                       <asp:ListItem Selected="True">Select</asp:ListItem>
                                       <asp:ListItem>NO</asp:ListItem>
                                       <asp:ListItem>YES</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                                        ControlToValidate="ddlcontrolinglocator" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    Controlling Locator Code</td>
                                <td>
                                    <asp:TextBox ID="txtlocatorcode" runat="server" AutoPostBack="True" Width="150px" 
                                   Enabled="False" OnTextChanged="txtlocatorcode_TextChanged"></asp:TextBox>
                               <asp:ImageButton ID="imgcheck" runat="server" Height="14px" 
                                   ImageUrl="~/Images/Check.jpg" Visible="False" Width="15px" />
                               <asp:ImageButton ID="imgcross" runat="server" ImageUrl="~/Images/Cross.jpg" 
                                   Visible="False" /></td>
                                   <td>
                                    &nbsp;</td>
                                   <td>
                                    Bill To</td>
                                <td>
                                    <asp:DropDownList ID="ddlbuildto" runat="server" Enabled="False">
                                        <asp:ListItem Selected="True">Select</asp:ListItem>
                                        <asp:ListItem>SELF</asp:ListItem>
                                        <asp:ListItem>Controlling Locator</asp:ListItem>
                                    </asp:DropDownList></td>
                            </tr>
                             
                            <tr>
                             
                                <td>
                                    GL Code
                                </td>
                                <td>
                                    <asp:DropDownList ID="txtAccountCode" runat="server" 
                                        ToolTip="Please Enter Account Code" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td >
                                    &nbsp;</td>
                                    <td>Bill Type</td>
                                <td>
                                    <asp:DropDownList ID="ddlBillType" runat="server">
                                        <asp:ListItem Selected="True" Text="Fortnightly" Value="Fortnightly"></asp:ListItem>
                                        <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                                        <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem> 
                                        <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                    </asp:DropDownList><td>
                                    &nbsp;</td>
                                </td>
                                  <td>
                                  <asp:Label ID="lblTDS" Text="TDS On Commission %" Visible="false"></asp:Label></td>
                             <td>
                                    <asp:TextBox ID="txttdsoncomm" runat="server" Text="0" 
                                        ToolTip="TDS On Comm Must be in precentage" Width="80px" Visible="False"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regex_txttdsoncomm" runat="server" 
                                        ControlToValidate="txttdsoncomm" ErrorMessage="*" 
                                        ValidationExpression="^\d+\.?\d*$"></asp:RegularExpressionValidator>
                                </td>
                                
                                </tr>
                            <tr>
                                <td>Mobile Number
                                    <br />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtmobile" runat="server" 
                                        ToolTip="Mobile Number Not More Than 10 Digit" MaxLength="10" 
                                        ValidationGroup="Gen"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="Reg_txtmobile" runat="server" 
                                        ErrorMessage="*" ControlToValidate="txtmobile" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                </td>
                            
                                <td>
                                    &nbsp;</td>
                                    <td>Commission %</td>
                                <td>
                                    <asp:TextBox ID="txtcommision" runat="server" MaxLength="4" Text="0"
                                        ToolTip="Commission Percentage Must Be of 3 digit"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regex_txtcommision" runat="server" 
                                        ControlToValidate="txtcommision" ErrorMessage="*" 
                                        ValidationExpression="^\d+\.?\d*$"></asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>TDS On Freight %</td>
                                <td>
                                    <asp:TextBox ID="txtTDSFreight" runat="server" Text="0"
                                        ToolTip="TDS on Freight Must be in precentage" Width="80px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regex_txtTDSFreight" runat="server" 
                                        ControlToValidate="txtTDSFreight" ErrorMessage="*" 
                                        ValidationExpression="^\d+\.?\d*$"></asp:RegularExpressionValidator>
                                </td>
                                </tr>
                            
                            <tr>
                            <td>
                                Remark
                                <br />
                            </td>
                            <td>
                                <asp:TextBox ID="txtremark" runat="server" Height="25px" TextMode="MultiLine" 
                                    ToolTip="Enter Remark "></asp:TextBox>
                                <br />
                             </td>
                             <td>
                                 &nbsp;</td>
                             <td>
                                 Pan Card Number
                             </td>
                              <td>
                                  <asp:TextBox ID="txtPancardNumber" runat="server" MaxLength="20"></asp:TextBox>
                             </td>
                             <td>
                                 &nbsp;</td>
                                <td>
                                    Service Tax Number
                                </td>
                                <td>
                                    <asp:TextBox ID="txtServicetaxno" runat="server" MaxLength="15" Width="80px"></asp:TextBox>
                                </td>
                            </tr> 
                            <tr>
                               <td>
                                    Ops Email</td>
                                <td>
                                    <asp:TextBox ID="txtemail" runat="server" 
                                        ToolTip="Enter Mail Adress Containing @ and special Symbols"></asp:TextBox>
                                </td>
                            
                                <td>
                                    &nbsp;</td>
                                <td>
                                    Account&#39;s Email <br />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtaccountmailid" runat="server" 
                                        class="validate[required,funcCall[validateEmail]]" 
                                        ToolTip="Enter Mail Adress Containing @ and special Symbols"></asp:TextBox>
                                    
                                </td>
                                <td>
                                    
                                    &nbsp;</td>
                                
                            
                             <td>Sale&#39;s Email
                                   <br />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsalesmail" runat="server" 
                                        ToolTip="Enter Mail Adress Containing @ and special Symbols"></asp:TextBox>
                                </td>
                                
                                </tr>
                            
                            <tr>
                            <td>Currency Code </td>
                            <td>
                                <asp:DropDownList ID="ddlCurrencyCode" runat="server" Width="150px">
                                </asp:DropDownList>
                             </td>
                             <td>
                                 &nbsp;</td>
                             <td>
                                 IAC Code</td>
                              <td>
                                  <asp:DropDownList ID="ddlIACCode" runat="server"                                      
                                      ToolTip="Select IAC Code Of Agent" Width="120px">
                                      <asp:ListItem>Select</asp:ListItem>
                                  </asp:DropDownList>
                             </td>
                             <td>
                                 &nbsp;</td>
                                <td>
                                    Threshold Alert
                                </td>
                                <td>
                                    <asp:TextBox ID="txtThresholdAlert" runat="server" MaxLength="50" Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr>
                              
                                <td>
                                    Rateline Preference</td>
                                <td>
                                    <asp:DropDownList ID="ddlRatelinePreference" runat="server" Width="90px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    CCSF Code</td>
                                <td>
                                    <asp:DropDownList ID="ddlCCSFCode" runat="server" 
                                     ToolTip="Select CCSF Code Of Agent" Width="120px">
                                     <asp:ListItem>Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                 &nbsp;</td>
                                <td>Invoice due days*</td>
                                <td><asp:TextBox ID="txtInvoiceDueDays" runat="server" MaxLength="2" Text="30" Width="80px" ></asp:TextBox>
                                </td>
                              
                            </tr>
                          
                            <tr>
                                 <td>Default Pay Mode
                                    </td>
                               <td>
                                    <asp:DropDownList ID="ddlDefaultPayMode" runat="server" 
                                     ToolTip="Select Default Pay Mode" Width="90px">
                                     
                                    </asp:DropDownList>
                                </td>
                                <td class="auto-style1"></td>
                                    <td>
                                         Allowed Paymode
                                        </td>
                                    <td>
                                        <asp:TextBox ID="txtAllowPayMode" runat="server" Width="100px"></asp:TextBox>
                                            <asp:ImageButton ID="ImageButton7" runat="server" ImageAlign="AbsMiddle"
                                            ImageUrl="~/Images/list_bullets.png"
                                            OnClientClick="javascript:GetPayMode();return false;" Width="16px" /></td>
                                   
                                        </td>
 
                                <td></td>
                                <td>
                                   <asp:Label ID="lblDABA" runat="server" text="DBA"></asp:Label>
                                </td>
                                                                    <td>
                                <asp:TextBox ID="txtDBA" runat="server" Width="100px"></asp:TextBox>    
                                </td>
                                <td>
                                    </td>
                               
                            </tr>
                            <tr>
                                 <td>
                                    <asp:CheckBox ID="chkIsFOC" runat="server" Text="Is FOC" />
                                </td>
                                  
                                <td>
                                    <asp:CheckBox ID="chkKnownShipper" runat="server" Text="Known Shipper" />
                                </td>
                            <td>
                                    <asp:CheckBox ID="ChkVAT" runat="server" Text="VAT Exemption" />
                                </td>
                                 <td>
                                  <asp:TextBox ID="txtAgentReferenceCode" runat="server" MaxLength="15" 
                                      Visible="False"></asp:TextBox>
                             
                                </td>
                                <td>
                                 <asp:CheckBox ID="chkAutoGenInv" runat="server" Text="Auto Generate Invoice" />   
                                </td>
                                
                                <td>
                                 <asp:CheckBox ID="chkvalidbg" runat="server" Text="Validate Credit" />  
                                </td>
                                <td><asp:CheckBox ID="chkSelectAll" runat="server" 
                                        OnChange="javascript:SelectAllStations(this);return false;" Text="Select All" 
                                        Visible="False" /></td>
                                <td> <asp:CheckBoxList ID="chkListStation" runat="server" 
                                        meta:resourceKey="chkListStationResource1" Visible="False">
                                    </asp:CheckBoxList></td>
                            </tr>
                            
                        </table>
                    </asp:Panel>
                   
                </ContentTemplate>
                
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2" Width="500px">
                <HeaderTemplate>
                    Credit
                </HeaderTemplate>
                <ContentTemplate>
                    <div>
                        <div class="divback" style="width: 960px; height: 260px;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblCreditStatus" Text="" ForeColor="Red" runat="server"></asp:Label>
                                    
                                    <%-- <asp:DropDownList ID="ddltransaction" runat="server" AutoPostBack ="true" 
                                        onselectedindexchanged="ddltransaction_SelectedIndexChanged" Width="70px" >
                                    </asp:DropDownList>--%>
                                    
                                    <div style="overflow:scroll;">
                                    <asp:GridView ID="grdCreditinfo" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                        Width="100%" Height="80px" AllowSorting="True" PageSize="3">
                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                        <Columns>
                                        
                                            <asp:TemplateField HeaderText="Transaction Type">
                                               <FooterTemplate>
                                                    <asp:Button ID="btnAdd" runat="server" AutoPostBack="true" CssClass="button" Text="Add New"
                                                        ValidationGroup="Gen" OnClick="btnAdd_Click" />
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <%-- <asp:DropDownList ID="ddlbankname" runat="server" EnableViewState="true">
                                                    </asp:DropDownList>--%>
                                                    <asp:DropDownList ID="ddltransaction" runat="server" AutoPostBack="true" OnSelectedIndexChanged="GetTransaction"
                                                        Width="70px">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Bank Name">
                                                <ItemTemplate>
                                                    <%-- <asp:DropDownList ID="ddlbankname" runat="server" EnableViewState="true">
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtbankname" runat="server">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Bank Address">
                                                <ItemTemplate>
                                                    <%-- <asp:DropDownList ID="ddlbankname" runat="server" EnableViewState="true">
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtbankaddress" runat="server">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Valid From">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtsatrtdate" runat="server" DataTextField="" Width="90px" EnableViewState="true">
                                                    </asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" Format="dd/MM/yyyy"  runat="server" TargetControlID="txtsatrtdate"  >
                                                    </asp:CalendarExtender>
                                                    <br />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="False" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Valid To">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtvalidto" runat="server" Width="90px" AutoPostBack="true" EnableViewState="true">
                                                    </asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" Format="dd/MM/yyyy" runat="server" TargetControlID="txtvalidto" >
                                                    </asp:CalendarExtender>
                                                    <br />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Bank Gurantee Number">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtbankgurantee" runat="server" ValidationGroup="check" Width="100px"
                                                        EnableViewState="true">
                                                    </asp:TextBox>
                                                    <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtbankgurantee" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtbankgurantee"
                                                        ErrorMessage="*" ValidationExpression="^\d+$" ValidationGroup="check">
                                                    </asp:RegularExpressionValidator>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Gurantee Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtbankguranteeamt" runat="server" AutoPostBack="true" Width="70px"
                                                        EnableViewState="true">
                                                    </asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtbankguranteeamt"
                                                        ErrorMessage="Numeric Value" ValidationExpression="^\d+$" ValidationGroup="check">
                                                    </asp:RegularExpressionValidator>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" />
                                                <ItemStyle Wrap="False"/>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Invoice Amount" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAmount" runat="server" Width="70px" Text='<%# Eval("txtAmount")%>'
                                                        ReadOnly="true" EnableViewState="true">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Threshold Limit(%)">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txttresholdlimit" runat="server" ToolTip="Threshold limit in percentage"
                                                        Width="70px" MaxLength="2">
                                                    </asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator22" runat="server"
                                                        ControlToValidate="txttresholdlimit" ErrorMessage="*" ValidationExpression="^\d+$"
                                                        ValidationGroup="check">
                                                    </asp:RegularExpressionValidator>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Threshold Limit(Days) ">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txttresholdlimitdays" runat="server" ToolTip="Threshold limit in days"
                                                        Width="70px" MaxLength="2">
                                                    </asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator_Treshholddays" runat="server"
                                                        ControlToValidate="txttresholdlimitdays" ErrorMessage="*" ValidationExpression="^\d+$"
                                                        ValidationGroup="check">
                                                    </asp:RegularExpressionValidator>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Expired">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkExpire" runat="server" EnableViewState="true" />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Srno" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSrNo" runat="server" EnableViewState="true" />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True" />
                                                <ItemStyle Wrap="False" />
                                            </asp:TemplateField>
                                            
                                        </Columns>
                                        <HeaderStyle CssClass="titlecolr" />
                                        <RowStyle HorizontalAlign="Center" />
                                    </asp:GridView>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <table>
                            <tr><td></td><td></td></tr>
                                <tr>
                                    <td>
                                        Invoice Balance
                                    </td>
                                    <td style="width: 50px">
                                        <asp:TextBox ID="txtinvoice" runat="server" Text='<%# Eval("txtAmount") %>' AutoPostBack="True" Width="100px"
                                            ToolTip="Total Credit" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td>
                                        Credit Used
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcreditAmt" runat="server" Width="100px"
                                            ToolTip="Amount Credited For Agent" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td>
                                        Credit Remaining
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcreditremain" runat="server" ToolTip="Remaining Balance After Use" Width="100px" 
                                            Text='<%# Eval("CreditRemaining") %>' Enabled="False"></asp:TextBox>
                                    </td>
                                    <td>
                                     Credit Days
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCreditdays" runat="server" ToolTip ="Credit Days" Text="0"  Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnCalculate" runat="server" Text="Calculate" CssClass="button" ValidationGroup="Gen"
                                            OnClick="btnCalculate_Click" Visible="False" />&nbsp;
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"
                                            ValidationGroup="Gen" />
                                         
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3" Width="500px" Visible="false">
                <HeaderTemplate>
                    Stock
                </HeaderTemplate>
                <ContentTemplate>
                    <div style="overflow: scroll; width: 100%; height: 300px">
                        <div id="light">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="LBLTABAllocStatus" runat="server" ForeColor="Red"></asp:Label>
                                        <asp:GridView ID="grdAWBAllocationDetails" runat="server" 
                                            AutoGenerateColumns="False" ShowFooter="True" Width="400px">
                                            <AlternatingRowStyle CssClass="trcolor" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="From AWB">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtAWBFrom" runat="server" CssClass="grdrowfont" 
                                                            Text='<%# Eval("AWBFrom") %>' Width="20px">
                                                            </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="True" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="To AWB">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtAWBTo" runat="server" CssClass="grdrowfont" 
                                                            Text='<%# Eval("AWBTo") %>' Width="20px">
                                                            </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Wrap="True" />
                                                </asp:TemplateField>
                                                
                                                    
                                            </Columns>
                                            <HeaderStyle CssClass="titlecolr" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                </table>
                                <table> 
                                <tr>
                                    <td>
                                        City
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTSubLevel" runat="server" Enabled="False"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFromUBI" runat="server" Text="From AWB "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTFromAWB" runat="server" MaxLength="12" BorderStyle="Solid" BackColor="#FFB9B9"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                            FilterType="Numbers" TargetControlID="TXTFromAWB" ValidChars="0123456789">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="TXTFromAWB"
                                            runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTOUBI" runat="server" Text="To AWB"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTToAWB" runat="server" MaxLength="12" BorderStyle="Solid" BackColor="#FFB9B9"
                                            ValidationGroup="Stock"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="TXTToAWB_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" FilterType="Numbers" TargetControlID="TXTToAWB" ValidChars="0123456789">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RFV" ControlToValidate="TXTToAWB" runat="server"
                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAlloc" runat="server" Text="Allocate" CssClass="button" OnClick="btnAlloc_Click"
                                            ValidationGroup="Gen" ToolTip="Allocate Range Of AWB Number's" />
                                        &nbsp;<asp:Button ID="btnViewStock" runat="server" Text="Allocation Report" CssClass="button"
                                            ValidationGroup="Gen" ToolTip="Total Stock Allocated" OnClick="btnViewStock_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <br />
                            <table width="100%">
                                <tr>
                                    <td>
                                        <div>
                                            AWBs allocated to Agent
                                        </div>
                                        <div>
                                            <asp:GridView ID="GRDAllocatedToAgent" runat="server" AutoGenerateColumns="False"
                                                ShowFooter="True" Width="400px">
                                                <AlternatingRowStyle CssClass="trcolor" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Agent Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtAgentName" runat="server" CssClass="grdrowfont" Text='<%# Eval("Agentname") %>'
                                                                Width="110px">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Wrap="True" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="From AWB">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtAWBFrom" runat="server" CssClass="grdrowfont" Text='<%# Eval("FromAWB") %>'
                                                                Width="110px">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Wrap="True" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="To AWB">
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtAWBTo" runat="server" CssClass="grdrowfont" Text='<%# Eval("ToAWB") %>'
                                                                Width="110px">
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Wrap="True" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="titlecolr" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            AWB Available For Allocation
                                            <asp:Label ID="lblstationCode" runat="server"></asp:Label>
                                        </div>
                                        <div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <asp:HiddenField ID="HidFlag" runat="server" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="TabPanel4" Visible="false">
                <HeaderTemplate>
                    Deals
                </HeaderTemplate>
                <ContentTemplate>
                    <div style="overflow: scroll; width: 100%; height: 300px">
                        <div class="divback">
                            <table width="50%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblType" runat="server" Text="Deal Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ChangeGridDealHeader">
                                            <asp:ListItem>Select</asp:ListItem>
                                            <asp:ListItem>Tonnage</asp:ListItem>
                                            <asp:ListItem>Revenue</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="ddlDealsRequiredFieldValidator" runat="server" ControlToValidate="ddlType"
                                            ErrorMessage="*" InitialValue="Select"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                            <fieldset>
                                <legend style="font-weight: bold; color: #999999; padding: 5px;" xml:lang="">Deals</legend>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDealDateFrom" runat="server" Text="Date From"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtlblDealDateFrom" runat="server" Width="110px"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtlblDealDateFrom_CalendarExtender" runat="server" Enabled="True" 
                                                TargetControlID="txtlblDealDateFrom" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDealDateTo" runat="server" Text="Date To"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtlblDealDateTo" runat="server" Width="110px"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtlblDealDateTo_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtlblDealDateTo" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                        </div>
                        <asp:GridView ID="grdDeals" runat="server" ShowFooter="True" Width="100%" Height="82px"
                            CellSpacing="2" CellPadding="2" AutoGenerateColumns="False">
                            <AlternatingRowStyle CssClass="trcolor" HorizontalAlign="Center"></AlternatingRowStyle>
                            <HeaderStyle CssClass="titlecolr" />
                            <RowStyle HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField HeaderText="From">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDlFrom" runat="server" Width="110px"  Text='<%# Eval("AFrom")%>' ></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDlTo" runat="server" Width="110px" Text='<%# Eval("ATo")%>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDlPercent" runat="server" OnTextChanged="ChangeGridDealItem"
                                            AutoPostBack="true" Width="110px" Text='<%# Eval("Percent")%>'></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="txtDlPercentRegularExpressionValidator" runat="server"
                                            ControlToValidate="txtDlPercent" ErrorMessage="*" ValidationExpression="^\d*\.?\d+$"></asp:RegularExpressionValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Value">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDlValue" runat="server" OnTextChanged="ChangeGridDealItem" AutoPostBack="true"
                                            Width="110px" Text='<%# Eval("Value")%>'></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="txtDlValueRegularExpressionValidator" runat="server"
                                            ControlToValidate="txtDlValue" ErrorMessage="*" ValidationExpression="^\d*\.?\d+$"></asp:RegularExpressionValidator>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="width: 100%;">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnDealAdd" runat="server" CssClass="button" OnClick="btnDealAdd_Click" CausesValidation="false"
                                                            Text="Add" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        
                        </fieldset>
                        <fieldset>
                            <legend style="font-weight: bold; color: #999999; padding: 5px;" xml:lang="">History</legend>
                            <asp:GridView ID="grdDealsHistory" Height="82px" CellSpacing="2" CellPadding="2"
                                AutoGenerateColumns="False" runat="server" Width="100%">
                                <AlternatingRowStyle CssClass="trcolor" HorizontalAlign="Center"></AlternatingRowStyle>
                                <HeaderStyle CssClass="titlecolr" />
                                <RowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="DateFrom" HeaderText="Deals From" />
                                    <asp:BoundField DataField="DateTo" HeaderText="Deals From" />
                                    <asp:BoundField DataField="Type" HeaderText="DealType" />
                                    <asp:BoundField DataField="AFrom" HeaderText="From" />
                                    <asp:BoundField DataField="ATo" HeaderText="To" />
                                    <asp:BoundField DataField="Percent" HeaderText="%" />
                                    <asp:BoundField DataField="Value" HeaderText="Value" />
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
              <asp:TabPanel ID="TabPanelRates" runat="server" Width="900px" Visible="false">
                <HeaderTemplate>
                    Rates
                </HeaderTemplate>
                <ContentTemplate>
                    <div style=" overflow:scroll; height:310px;">
                       
                       <asp:Panel ID="PanelSpecificRates" runat="server" GroupingText="Specific Rates">
                            
                            <asp:GridView ID="GRDSpecificRates" runat="server" Width="900px" AutoGenerateColumns="False"
                                AllowPaging="True" OnRowCommand="GRDSpecificRates_RowCommand">
                                <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="Rate Card">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLRateCardName" runat="server" Text='<%#Eval("RateCardName")%>' Width="120px"></asp:Label>
                                            <asp:HiddenField ID="HidSerialNumber" runat="server" Value='<%#Eval("SerialNumber") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Origin Level">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLOriginLevel" runat="server" Text='<%#Eval("OriginLevel")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Origin">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLOrigin" runat="server" Text='<%#Eval("Origin")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Destination Level">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLDestinationLevel" runat="server" Text='<%#Eval("DestinationLevel")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Destination">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLDestination" runat="server" Text='<%#Eval("Destination")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start Date">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLStartDate" runat="server" Text='<%#Eval("StartDate")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Date">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLEndDate" runat="server" Text='<%#Eval("EndDate")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RateBase">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLRateBase" runat="server" Text='<%#Eval("RateBase")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comm. Percent">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLAgentCommPercent" runat="server" Text='<%#Eval("AgentCommPercent")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount Percent">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLMaxDiscountPercent" runat="server" Text='<%#Eval("MaxDiscountPercent")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Tax">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLServiceTax" runat="server" Text='<%#Eval("ServiceTax")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TDS Percent">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLTDSPercent" runat="server" Text='<%#Eval("TDSPercent")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:ButtonField CommandName="View" Text="View" />
                                </Columns>
                                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                                <FooterStyle CssClass="grdrowfont"></FooterStyle>
                                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                                <RowStyle CssClass="grdrowfont"></RowStyle>
                            </asp:GridView>
                        </asp:Panel>
                        <br />
                        
                            <asp:Panel ID="PanelGeneralRates" runat="server" GroupingText="General Rates">
                           
                            <asp:GridView ID="GRDGeneralRates" runat="server" Width="900px" AutoGenerateColumns="False"
                                AllowPaging="True" OnRowCommand="GRDGeneralRates_RowCommand">
                                <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="Rate Card">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLRateCardName" runat="server" Text='<%#Eval("RateCardName")%>' Width="120px"></asp:Label>
                                            <asp:HiddenField ID="HidSerialNumber" runat="server" Value='<%#Eval("SerialNumber") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Origin Level">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLOriginLevel" runat="server" Text='<%#Eval("OriginLevel")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Origin">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLOrigin" runat="server" Text='<%#Eval("Origin")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Destination Level">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLDestinationLevel" runat="server" Text='<%#Eval("DestinationLevel")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Destination">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLDestination" runat="server" Text='<%#Eval("Destination")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start Date">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLStartDate" runat="server" Text='<%#Eval("StartDate")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Date">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLEndDate" runat="server" Text='<%#Eval("EndDate")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RateBase">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLRateBase" runat="server" Text='<%#Eval("RateBase")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comm. Percent">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLAgentCommPercent" runat="server" Text='<%#Eval("AgentCommPercent")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount Percent">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLMaxDiscountPercent" runat="server" Text='<%#Eval("MaxDiscountPercent")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Tax">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLServiceTax" runat="server" Text='<%#Eval("ServiceTax")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TDS Percent">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLTDSPercent" runat="server" Text='<%#Eval("TDSPercent")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="LBLStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                           
                                    <asp:ButtonField HeaderText="View" Text="View" />
                                </Columns>
                                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                                <FooterStyle CssClass="grdrowfont"></FooterStyle>
                                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                                <RowStyle CssClass="grdrowfont"></RowStyle>
                            </asp:GridView>
                        </asp:Panel>
                        
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer></div>
        
            <div>
            <table width="100%">
            <tr>
                <td>
                    <asp:Button ID="imgAgentPopUp" runat="server" CssClass="button" Text="Configure Tax" OnClick="imgAgentPopUp_Click" CausesValidation="False" Visible="false" />    
                    <asp:Button ID="BtnAllSave" runat="server" Text="Save" CssClass="button" OnClick="BtnAllSave_Click"/>
                </td>
            </tr>
        </table>
            </div>
            
        <div style="visibility:hidden">
                            TDS on commission Information
                        <asp:Button ID="btnAddTDS" ValidationGroup="Gen" runat="server" Text="Add" CssClass="button" 
                            onclick="btnAddTDS_Click" />
                        <asp:Button ID="btnDeleteTDS" ValidationGroup="Gen" runat="server" Text="Delete" CssClass="button" 
                            onclick="btnDeleteTDS_Click" />

                        </div>
                                               <div style="visibility:hidden">
                            <asp:GridView ID="grdTDSInfo" runat="server" AutoGenerateColumns="False" Width="300px">
                                                <AlternatingRowStyle CssClass="trcolor" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkTDS" runat="server" Enabled="true" />                                             
                                                        </ItemTemplate>
                                                        <ItemStyle Wrap="True" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SrNo" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSrNo" Text='<%# Eval("SrNo") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="From Date">
                                                        <ItemTemplate>
                                                           <asp:TextBox ID="txtFromDate" runat="server" Text='<%# Eval("FromDate") %>' Width="90px" EnableViewState="true">
                                                            </asp:TextBox>
                                                            <asp:CalendarExtender ID="txtFromDate_CalendarExtender" Format="dd/MM/yyyy" runat="server" TargetControlID="txtFromDate"  >
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="To Date">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtToDate" runat="server" Text='<%# Eval("ToDate") %>' Width="90px" EnableViewState="true">
                                                            </asp:TextBox>
                                                            <asp:CalendarExtender ID="txtToDate_CalendarExtender" Format="dd/MM/yyyy" runat="server" TargetControlID="txtToDate"  >
                                                            </asp:CalendarExtender>
                                                        </ItemTemplate>
                                                        <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="%">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtTDSonCommPerc" runat="server" Width="60px" Text='<%# Eval("TDSOnCommPerc")%>'>
                                                            </asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="txtTDSonCommPercRegularExpressionValidator" runat="server"
                                                                ControlToValidate="txtTDSonCommPerc" ErrorMessage="*" ValidationExpression="^\d*\.?\d+$">
                                                                </asp:RegularExpressionValidator>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" Text='<%# Eval("Status") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="titlecolr" />
                                            </asp:GridView>                    
                        </div>
        
        
        <div id="AgentPopUp" class="white_content" style="overflow: auto; height:auto; width:auto;">
          <div style="margin:5px;">
         <h2 style="width:445px;">Agent Specific Tax Configuration</h2>
             <asp:Label ID="lblAgentPopUpError" runat="server"></asp:Label>
             <br />
             <table cellpadding="3" cellspacing="3" width="100%">
             <tr>
             <td>Tax Identifier</td>
             <td>
                 <asp:TextBox ID="txtTaxIdentifier" runat="server"></asp:TextBox>
             </td>
             <td>Tax%</td>
             <td>
                 <asp:TextBox ID="txtTaxPerc" runat="server"></asp:TextBox>
             </td>
             
             </tr>
             <tr>
             <td>Applied From</td>
             <td>
                 <asp:TextBox ID="txtAppFrm" runat="server"></asp:TextBox>
                  <asp:CalendarExtender ID="Ext_FrmExt" runat="server" TargetControlID="txtAppFrm" Format="dd/MM/yyyy">
                 </asp:CalendarExtender>
             </td>
             <td>To</td>
             <td>
               <asp:TextBox ID="txtAppTo" runat="server"></asp:TextBox>
                  <asp:CalendarExtender ID="Ext_ToExt" runat="server" TargetControlID="txtAppTo" Format="dd/MM/yyyy">
                 </asp:CalendarExtender>
             </td>
             </tr>
             <tr>
             <td colspan="4">
             <asp:CheckBox ID="chkAll" runat="server" Text="All" OnChange="javascript:SetAll();return false;" /><br />
             
                 <asp:CheckBox ID="chkIATAChrg" runat="server" Text="IATA Charges" /><br />
                 <asp:CheckBox ID="chkMKTChrg" runat="server" Text="MKT Charges" /><br />
                 <asp:CheckBox ID="chkCommission" runat="server" Text="Commission" /><br />
                 <asp:CheckBox ID="chkServTax" runat="server" Text="Service Tax" /><br />
                 <asp:CheckBox ID="chkDiscount" runat="server" Text="Discount" /><br />
                 </td></tr>
                 <tr>
             
             <td></td><td></td>
             </tr>
             <tr>
             <td colspan="4">
              <asp:Button ID="btnSaveAgentChrg" runat="server" CssClass="button" Text="Save" CausesValidation="false" OnClick="btnSaveAgentChrg_Click"/>
              <asp:Button ID="btnCancelAgentChrg" runat="server" CssClass="button" Text="Cancel" OnClientClick="javascript:HidePanelSplit_Agent(); return false;"/>
             </td>
             </tr>
             </table>
             <br />
            
         </div>
              </div>
        
        <div id="AgentPopUp1" class="black_overlay">
        </div>
        
        
    </div>
</asp:Content>