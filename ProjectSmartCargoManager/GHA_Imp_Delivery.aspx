<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GHA_Imp_Delivery.aspx.cs"
    MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.GHA_Imp_Delivery" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    </style>

    <script language="javascript" type="text/javascript">

        function PrintDO() {
            window.open('ShowDelivery.aspx', 'Print', 'toolbar=1,resizable=1,menubar=1');
            return false;
        }


        function SetOperationTime() {
            //Show popup
            //window.open('frmOperationTime.aspx', 'Operation Time','left=400,top=200,width=400,height=200,toolbar=0,resizable=no');
            //             window.open('frmOperationTime.aspx','', 'width=400px,height=200px,left=400,top=200');
            //             return false;
            document.getElementById('divOpsTimePopup').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
        }

        function CloseWindow() {
            document.getElementById('divOpsTimePopup').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }

        function Download(count) {
            window.open('Download.aspx?Mode=D', 'Download', 'left=100,top=100,width=800,height=420,toolbar=0,resizable=1');
        }
        function SelectheaderCheckboxes(headerchk) {
            var gvcheck = document.getElementById("<%=grdULDDelivery.ClientID %>");
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

        //         function SelectAllgrdAddRate(CheckBoxControl) {
        //             for (i = 0; i < document.forms[0].elements.length; i++) {
        //                 if (document.forms[0].elements[i].name.indexOf('check') > -1) {
        //                     document.forms[0].elements[i].checked = CheckBoxControl.checked;
        //                 }
        //             }
        //         }

        function SelectAllgrdAddRate(headerchk) {
            var gvcheck = document.getElementById("<%=grdMaterialDetails.ClientID %>");
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

        //Set actual wt based on gross wt
        function calculateActualWt(TextBox) {
            //Get Delivered Pcs
            var valDeliveredPcs = TextBox.value;

            //Get expected pcs
            ctrlname = TextBox.id.replace('txtactualpieces', 'txtexpectedpieces');
            var valExpectedPcs = document.getElementById(ctrlname).innerHTML;
            //             if (parseInt(valExpectedPcs) < parseInt(valDeliveredPcs)) {
            //                 alert('Deliver pieces can not be greater than Expected Pcs.');
            //                 TextBox.value = "";
            //                 TextBox.focus();
            //             }
            //Get expected wt
            var ctrlname = TextBox.id.replace('txtactualpieces', 'txtgrwt');
            var valExpectedWt = document.getElementById(ctrlname).innerHTML;

            //Calculate delivered wt
            var valDeliveredWt = (valExpectedWt / valExpectedPcs) * valDeliveredPcs;
            ctrlname = TextBox.id.replace('txtactualpieces', 'txtactualwt');
            document.getElementById(ctrlname).value = Math.round(valDeliveredWt, 4);
            //find remaining pieces
            var actualpcs = document.getElementById(TextBox.id).value;
            var totalpcs = document.getElementById(TextBox.id.replace('txtactualpieces', 'txtexpectedpieces')).innerHTML;
            var rempcs = document.getElementById(TextBox.id.replace('txtactualpieces', 'txtremainingpieces')).value;
            var txtrempcs = document.getElementById(TextBox.id.replace('txtactualpieces', 'txtremainingpieces'));
            txtrempcs.value = (parseInt(totalpcs) - parseInt(actualpcs));
        }
        //function SetHiddenVariale
        //{
        //var javar =rempcs;
        //_doDostBack('callPostBack',javar);
        //}
        function ErrorMsg() {

            alert("Please Provide atleast one parameter");

        }

        function SelectRow() {

            alert("Please Select Row");

        }

        function cann() {

            alert("Actual Value Cannot Be greater than expected value");

        }
        function InsertFailure() {

            alert("insertion Failed Please Try Again");

        }
        function FoundFailure() {

            alert("No Data Found Please Try Again");

        }
        function callexport() {
            //                 if (document.getElementById('txtflightdate').value == '' && document.getElementById('DdlFlightno').value == '') {
            //                     alert("Please provide Flight Number And Flilght Date");
            //                 }
            //                 else 
            {
                window.open('ShowDelivery.aspx', 'Print', 'left=100,top=100,width=800,height=420,toolbar=0,resizable=1');
            }


        }


        function Successfull() {
            alert("Save Successfull");
        }
        function AlreadyAvailable() {
            alert(" Already DO Available");
        }

        function popup() {

            var Fltdt = document.getElementById("<%= txtflightdate.ClientID %>").value;
            var Fltid = document.getElementById("<%= txtFlightPrefix.ClientID %>").value + document.getElementById("<%= txtFlightNo.ClientID %>").value;

            window.open('UCRPopup.aspx?Type=New' + '&Mode=M' + '&FlightNo=' + Fltid + '&FlightDate=' + Fltdt + '&pg=Del', '', 'left=0,top=0,width=1000,height=1000,toolbar=0,resizable=0,scrollbars=yes');
        }
        function ValidateCollection() {

            debugger;
            var table = document.getElementById('<%= grdMaterialDetails.ClientID%>');
            for (var i = 1; i < table.rows.length; i++) //setting the incrementor=0, but if you have a header set it to 1
            {
                //alert(i.toString());
                if (table.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked) {
                    var IsCollect = table.rows[i].cells[22].children[0].innerHTML;
                    var AWBNo = table.rows[i].cells[3].children[0].innerHTML;
                    if (IsCollect == "Collect") {
                        alert("Please collect the pending invoice amount before delivering for AWBNo:" + AWBNo + "!");
                        return false;
                    }

                }

            }
        }
        
    </script>

    <script language="javascript" type="text/javascript" src="scripts/jquery.min.js"></script>

    <script type="text/javascript">
        function refresh() {
            $get("<%=btnList.ClientID%>").click();
        }

        // Function for get Keypress Event & Call List Button 
        $(function() {

            // used to specific Text Box
            $("#<%=txtAWBNo.ClientID %>").keydown(function(e) {

                // used to $(":text"): Selects (ALL TextBoxes) only text elements on page (input[type=text])
                //$(":text").keydown(function(e) {
                if (e.keyCode == 13) { //if this is enter key
                    // Code to call Button
                    $("#<%= btnList.ClientID %>").trigger('click');
                    //alert("Amit");
                    return true;
                }
            });
        });


        // Function for when Enter Key Pressed in Textbox, Disable Postback on Pressing Entery Key in Textbox
        $(function() {
            $(':text').bind('keydown', function(e) {
                //on keydown for all textboxes
                if (e.target.className != "ddlAgentName")
                //excludes specific textbox like SearchTextBox
                {
                    if (e.keyCode == 13) { //if this is enter key
                        e.preventDefault();
                        return false;
                    }
                    else
                        return true;
                }
                else
                    return true;
            });
        });
      
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    ]<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
        <h1>
            Delivery
            <%--<img alt="" src="images/txtdelivery.png" />--%>
        </h1>
        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
            ForeColor="Red"></asp:Label>
        <div class="botline">
            <asp:Panel ID="pnl1" runat="server">
                <table width="100%" cellpadding="3">
                    <tr>
                        <td>
                            DO #
                        </td>
                        <td>
                            <asp:TextBox ID="txtDoNumber" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1">
                            Flight: &nbsp;
                        </td>
                        <td colspan="1">
                            <asp:TextBox ID="txtFlightPrefix" runat="server" Width="30px"></asp:TextBox>
                            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtFlightPrefix"
                                WatermarkText="Prefix" />
                            &nbsp;<asp:TextBox ID="txtFlightNo" runat="server" Width="60px"></asp:TextBox>
                            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtFlightNo"
                                WatermarkText="Flight ID" />
                            &nbsp;
                            <asp:TextBox ID="txtflightdate" runat="server" Width="70px"></asp:TextBox>&nbsp;
                            <asp:CalendarExtender ID="txtflightdate_CalendarExtender" runat="server" TargetControlID="txtflightdate"
                                Format="dd/MM/yyyy" PopupButtonID="imgDate">
                            </asp:CalendarExtender>
                            <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                        </td>
                        <%--<td>
            
            </td>--%>
                        <%--<td>
            Flight No:</td>
        <td>
            <asp:DropDownList ID="DdlFlightno" runat="server" AppendDataBoundItems="True" 
                onselectedindexchanged="DdlFlightno_SelectedIndexChanged" Width="80px">
            </asp:DropDownList>
        </td>--%>
                        <td colspan="1">
                            Agent:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlAgentName" runat="server" Width="300px">
                            </asp:DropDownList>
                            <%--<asp:TextBox ID="txtAgentName" runat="server" Width="95px" ></asp:TextBox>--%>
                        </td>
                        <td>
                            Del. Status
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server">
                                <asp:ListItem Text="ALL" Value=""></asp:ListItem>
                                <asp:ListItem Text="Partial" Value="P"></asp:ListItem>
                                <asp:ListItem Text="Complete" Value="C"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkReprint" runat="server" Checked="False" Width="2px" Visible="False">
                            </asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            AWB:
                        </td>
                        <td>
                            <asp:TextBox ID="txtprefix" runat="server" Width="50px"></asp:TextBox>
                            <asp:TextBox ID="txtAWBNo" runat="server" Width="100px" MaxLength="8" OnClick="btnList_Click"
                                OnTextChanged="txtAWBNo_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblULDNo" runat="server" Text="ULD No:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtULDNo" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblULDNo0" runat="server" Text="Consignee :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtConSearch" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblGatePassNo" runat="server" Text="Gate Pass :"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTokenList" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTokenDt" runat="server" Width="100px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" Format="dd/MM/yyyy" runat="server" Enabled="True"
                                TargetControlID="txtTokenDt" PopupButtonID="ImageButton2">
                            </asp:CalendarExtender>
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click"
                                CausesValidation="False" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click" />
                            <asp:Button ID="btnReport" runat="server" Text="Report" CssClass="button" OnClientClick="callexport();"
                                Visible="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <div style="width: 1024px; overflow: auto;" id="deliveryAWB">
            <h2>
                AWB Details</h2>
            <asp:GridView runat="server" AutoGenerateColumns="False" ShowFooter="True" ID="grdMaterialDetails"
                OnRowCommand="grdMaterialDetails_RowCommand" OnRowDataBound="grdMaterialDetails_RowDataBound">
                <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chk" runat="server" onclick="javascript:SelectAllgrdAddRate(this);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="check" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DO #" HeaderStyle-Width="200px">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlnkDONumber" Width="150px" runat="server" Text='<%#Eval("DoNumber") %>'
                                NavigateUrl='<%# "GHA_Imp_Delivery.aspx?DoNumber=" + Eval("DoNumber")%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Collect">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlnkCollection" runat="server" NavigateUrl='<%#Eval("CollectionURL") %>'
                                Text='<%# Eval("IsCollection") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Agent" HeaderStyle-Width="200px">
                        <ItemTemplate>
                            <asp:Label ID="txtagentname" runat="server" Text='<%# Eval("AgentName") %>' Width="200px"
                                CssClass="grdrowfont">
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="True"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AWB">
                        <ItemTemplate>
                            <%--<asp:Label ID="txtawbno" runat="server" Text='<%# Eval("AWBNumber") %>'
                            Width="70px" CssClass="grdrowfont">
                            </asp:Label>--%>
                            <asp:LinkButton ID="txtawbno" runat="server" Text='<%#Eval("AWBNumber") %>' Width="85px"
                                CssClass="grdrowfont" CommandName="ShowDODeatils" CommandArgument='<%#Eval("AWBNumber") %>'></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="ULDs#"><ItemTemplate>
                            <asp:Label ID="txtuldsno" runat="server" Text=""  
                            Width="110px" CssClass="grdrowfont">
                            </asp:Label>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField> --%>
                    <asp:TemplateField HeaderText="HAWB No">
                        <ItemTemplate>
                            <asp:Label ID="txthawbs" runat="server" Text='<%# Eval("HAWBNo") %>' Width="110px"
                                CssClass="grdrowfont">
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="True"></ItemStyle>
                    </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText="HAWB No."><ItemTemplate>
                            <asp:Label ID="txtHAWBNo" runat="server" Text='<%# Eval("HAWBNo") %>'
                            Width="70px" CssClass="grdrowfont">
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Origin">
                        <ItemTemplate>
                            <asp:Label ID="txtOrigin" runat="server" Text='<%# Eval("Origin") %>' Width="40px"
                                CssClass="grdrowfont">
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dest">
                        <ItemTemplate>
                            <asp:Label ID="txtDest" runat="server" Text='<%# Eval("Destination") %>' Width="40px"
                                CssClass="grdrowfont">
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Arr Pcs">
                        <ItemTemplate>
                            <asp:Label ID="txtoriginalpieces" runat="server" Text='<%# Eval("PiecesCount")%>'
                                Width="45px" CssClass="alignrgt">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Arr Wt">
                        <ItemTemplate>
                            <asp:Label ID="txtoriginalgrwt" runat="server" Text='<%# Eval("GrossWeight") %>'
                                Width="45px" CssClass="alignrgt">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rem Pcs" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="txtexpectedpieces" Enabled="false" runat="server" Text='<%# Eval("Expectedcount") %>'
                                Width="55px" CssClass="alignrgt">
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rem Wt" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="txtgrwt" Enabled="false" Text='<%# Eval("ExpectedWeight") %>' runat="server"
                                Width="55px" CssClass="alignrgt">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dlr Pcs">
                        <ItemTemplate>
                            <asp:TextBox ID="txtactualpieces" runat="server" Width="40px" Text='<%# Eval("RCVPieces") %>'
                                onchange="javascript:calculateActualWt(this);">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtactualpieces"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                            <asp:ImageButton ID="btnDimensionsPopup" Enabled="false" runat="server" ImageUrl="~/Images/list_bullets.png"
                                ImageAlign="AbsMiddle" OnClientClick="javascript:dimension(this);return false;" />
                        </ItemTemplate>
                        <ItemStyle Wrap="false" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dlr Wt">
                        <ItemTemplate>
                            <asp:TextBox ID="txtactualwt" runat="server" Width="40px" Text='<%# Eval("RCVWeight") %>'
                                Enabled="true">
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dlrd Pcs">
                        <ItemTemplate>
                            <asp:Label ID="txtDelieveredpieces" Enabled="false" runat="server" Text='<%# Eval("DORecPcs") %>'
                                Width="55px" CssClass="alignrgt">
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Right"></FooterStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dlrd Wt">
                        <ItemTemplate>
                            <asp:Label ID="txtDelieveredwt" Enabled="false" Text='<%# Eval("DORecWt") %>' runat="server"
                                Width="55px" CssClass="alignrgt">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Flt#">
                        <ItemTemplate>
                            <asp:TextBox ID="txtflightno" runat="server" Text='<%# Eval("FltNo") %>' Width="45px"
                                ReadOnly="true">
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comm Desc">
                        <ItemTemplate>
                            <asp:Label ID="lblDiscription" runat="server" Text='<%# Eval("Discription") %>' Width="200px">
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Flt Dt">
                        <ItemTemplate>
                            <asp:TextBox ID="txtbookingdate" runat="server" Width="100px" Text='<%# Eval("AWBDate") %>'
                                Enabled="false">
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--remaining pieces--%>
                    <asp:TemplateField HeaderText="Pcs">
                        <ItemTemplate>
                            <asp:TextBox ID="txtremainingpieces" runat="server" Width="20px" Enabled="false"
                                Text='<%# Eval("RemainingPieces") %>' EnableViewState="true" CssClass="alignrgt">
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Consignee">
                        <ItemTemplate>
                            <asp:TextBox ID="txtConsigneeName" runat="server" Width="80px" Enabled="false" Text='<%# Eval("ConsigneeName") %>'
                                EnableViewState="true">
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="TotalPieceCount">
                        <ItemTemplate>
                            <asp:Label ID="txtTotalpieces" runat="server" Text='<%# Eval("TotalPieceCount")%>'
                                Width="45px" CssClass="alignrgt">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Payment Type">
                        <ItemTemplate>
                            <asp:Label ID="lblpaymenttype" runat="server" Width="40px" Enabled="false" Text='<%# Eval("PayMode") %>'
                                EnableViewState="true">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Invoice #">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlnkInvoiceNo" runat="server" NavigateUrl='<%#Eval("InvoiceURL") %>'
                                Target="_blank" Text='<%# Eval("InvoiceNo") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <asp:TextBox ID="txtLocationAWB" runat="server" Text='<%# Eval("Location") %>' Width="110px"
                                CssClass="grdrowfont">
                            </asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Wrap="True"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comm Code" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblCommCode" runat="server" Text='<%# Eval("CommCode") %>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    </asp:TemplateField>
                    <%--New Columns Added--%>
                    <asp:TemplateField HeaderText="Charges" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblCharges" runat="server" Text='<%#Eval("Charges")%>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tax" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblTax" runat="server" Text='<%#Eval("Tax")%>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblTotal" runat="server" Text='<%#Eval("Total")%>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amt Due" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="lblAmtDue" runat="server" Text='<%#Eval("AmountDue")%>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acc Pcs" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblAccPcs" runat="server" Text='<%#Eval("AccPcs")%>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acc Wt" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblAccWt" runat="server" Text='<%#Eval("AccWt")%>'>
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UdpatedOn">
                        <ItemTemplate>
                            <asp:Label ID="lblUpdatedOn" runat="server" Width="100px" Text='<%# Eval("UpdatedOn") %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--                        <asp:TemplateField HeaderText="Invoice Number" Visible="true">
                        <ItemTemplate>
                        <asp:Repeater ID="rptInvoiceNumber" runat="server" OnItemCommand="rptInvoiceNumber_ItemCommand">
                            <ItemTemplate>
                           <asp:HyperLink ID="hlnkInvoiceNumber" runat="server" Text='<%# Eval("InvoiceNo") %>' NavigateUrl='<%# Eval("InvoiceURL") %>' ></asp:HyperLink>
                            </ItemTemplate>
                            <SeparatorTemplate>,</SeparatorTemplate>
                        </asp:Repeater>
                        </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Collection" Visible="true">
                        <ItemTemplate>
                        <asp:Repeater ID="rptCollect" runat="server" OnItemCommand="rptCollect_ItemCommand">
                            <ItemTemplate>
                           <asp:HyperLink ID="hlnkCollection" runat="server" Text="Collect" NavigateUrl='<%# Eval("CollectURL") %>' ></asp:HyperLink>></asp:HyperLink>
                            </ItemTemplate>
                            <SeparatorTemplate>,</SeparatorTemplate>
                        </asp:Repeater>
                        </ItemTemplate>
                        </asp:TemplateField>--%>
                </Columns>
                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <FooterStyle CssClass="grdrowfont"></FooterStyle>
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
            </asp:GridView>
            <asp:Button ID="LnkModify" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_MODIFY %>"
                CssClass="button" OnClick="LnkModify_Click" Visible="false"></asp:Button>
        </div>
        <h2>
            <asp:Label ID="lblULD" runat="server" Text="ULD Details"></asp:Label></h2>
        <div id="UDLdiv" runat="server" style="width: 1024px; overflow: auto;" >
        <div style="width: 1024px; overflow: auto;"  id="deliveryULD">
            <asp:GridView runat="server" AutoGenerateColumns="false" ShowFooter="True" ID="grdULDDelivery">
                <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chk" runat="server" onclick="javascript:SelectheaderCheckboxes(this);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ULDcheck" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DO #" HeaderStyle-Width="200px">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlnkDONumber" Width="200px" runat="server" Text='<%#Eval("DoNumber") %>'
                                NavigateUrl='<%# "GHA_Imp_Delivery.aspx?DoNumber=" + Eval("DoNumber")%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Agent Name"><ItemTemplate>
                            <asp:Label ID="txtagentname" runat="server" Text='<%# Eval("AgentName") %>'  
                            Width="110px" CssClass="grdrowfont">
                            </asp:Label>
                         </ItemTemplate>
                        <ItemStyle Wrap="True">
                        </ItemStyle>
                        </asp:TemplateField>  --%>
                    <%--<asp:TemplateField HeaderText="AWB No."><ItemTemplate>--%>
                    <%--<asp:Label ID="txtawbno" runat="server" Text='<%# Eval("AWBNumber") %>'
                            Width="70px" CssClass="grdrowfont">
                            </asp:Label>--%>
                    <%--<asp:LinkButton ID="txtawbno" runat="server" Text='<%#Eval("AWBNumber") %>'
                            Width="80px" CssClass="grdrowfont" CommandName="ShowDODeatils" CommandArgument='<%#Eval("AWBNumber") %>'></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="ULD">
                        <ItemTemplate>
                            <asp:Label ID="txtuldsno" runat="server" Text='<%#Eval("ULDNo") %>' Width="110px"
                                CssClass="grdrowfont">
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="True"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AWB Ct">
                        <ItemTemplate>
                            <asp:Label ID="txtAWBs" runat="server" Text='<%#Eval("AWBCount") %>' Width="110px"
                                CssClass="grdrowfont">
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="True"></ItemStyle>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="HAWB No."><ItemTemplate>
                            <asp:Label ID="txtHAWBNo" runat="server" Text='<%# Eval("HAWBNo") %>'
                            Width="70px" CssClass="grdrowfont">
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                        </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Origin">
                        <ItemTemplate>
                            <asp:Label ID="txtOrigin" runat="server" Text='<%# Eval("Origin") %>' Width="40px"
                                CssClass="grdrowfont">
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dest">
                        <ItemTemplate>
                            <asp:Label ID="txtDest" runat="server" Text='<%# Eval("Destination") %>' Width="40px"
                                CssClass="grdrowfont">
                            </asp:Label>
                        </ItemTemplate>
                        <ItemStyle Wrap="False"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Invoice #">
                        <ItemTemplate>
                            <asp:Label ID="InvoiceNo" runat="server" Text='<%# Eval("InvoiceNo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Collection">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlnkCollection" runat="server" NavigateUrl='<%#Eval("CollectionURL") %>'
                                Target="_blank" Text='<%# Eval("IsCollection") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Arr Pcs">
                        <ItemTemplate>
                            <asp:Label ID="txtoriginalpieces" runat="server" Text='<%# Eval("PiecesCount")%>'
                                Width="45px" CssClass="alignrgt">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Arr Wt">
                        <ItemTemplate>
                            <asp:Label ID="txtoriginalgrwt" runat="server" Text='<%# Eval("GrossWeight") %>'
                                Width="45px" CssClass="alignrgt">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rem Pcs">
                        <ItemTemplate>
                            <asp:Label ID="txtexpectedpieces" runat="server" Text='<%# Eval("Expectedcount") %>'
                                Width="55px" CssClass="alignrgt">
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rem Wt">
                        <ItemTemplate>
                            <asp:Label ID="txtgrwt" Text='<%# Eval("ExpectedWeight") %>' runat="server" Width="55px"
                                CssClass="alignrgt">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dlr Pcs">
                        <ItemTemplate>
                            <asp:TextBox ID="txtactualpieces" runat="server" Width="40px" Text='<%# Eval("RCVPieces") %>'
                                onchange="javascript:calculateActualWt(this);" ReadOnly="true">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtactualpieces"
                                ErrorMessage="*"></asp:RequiredFieldValidator>
                        </ItemTemplate>
                        <ItemStyle Wrap="false" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dlr Wt">
                        <ItemTemplate>
                            <asp:TextBox ID="txtactualwt" runat="server" Width="40px" Text='<%# Eval("RCVWeight") %>'
                                Enabled="true" ReadOnly="true">
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dlrd Pcs">
                        <ItemTemplate>
                            <asp:Label ID="txtDelieveredpieces" runat="server" Text='<%# Eval("DORecPcs") %>'
                                Width="55px" CssClass="alignrgt">
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Right"></FooterStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dlrd Wt">
                        <ItemTemplate>
                            <asp:Label ID="txtDelieveredwt" Text='<%# Eval("DORecWt") %>' runat="server" Width="55px"
                                CssClass="alignrgt">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Flt#">
                        <ItemTemplate>
                            <asp:TextBox ID="txtflightno" runat="server" Text='<%# Eval("FltNo") %>' Width="45px"
                                ReadOnly="true">
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Descrip.">
                        <ItemTemplate>
                            <asp:Label ID="lblDiscription" runat="server" Text='<%# Eval("Discription") %>'  Width="55px">
                            </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center"></FooterStyle>
                        </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Flt Dt">
                        <ItemTemplate>
                            <asp:TextBox ID="txtbookingdate" runat="server" Width="100px" Text='<%# Eval("AWBDate") %>'
                                Enabled="false">
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--remaining pieces--%>
                    <%--<asp:TemplateField HeaderText="Pieces"><ItemTemplate>
                            <asp:TextBox ID="txtremainingpieces" runat="server" Width="20px" Enabled="false" Text='<%# Eval("RemainingPieces") %>' EnableViewState="true" CssClass="alignrgt"   >
                            </asp:TextBox >
                        </ItemTemplate>
                        </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Consignee">
                        <ItemTemplate>
                            <asp:TextBox ID="txtConsigneeName" runat="server" Width="80px" Enabled="false" Text='<%# Eval("ConsigneeName") %>'
                                EnableViewState="true">
                            </asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <asp:TextBox ID="txtLocationUld" runat="server" Text='<%# Eval("Location") %>' Width="110px"
                                CssClass="grdrowfont">
                            </asp:TextBox>
                            <asp:HiddenField ID="IsReceived" runat="server" Value='<%# Eval("IsReceived") %>' />
                        </ItemTemplate>
                        <ItemStyle Wrap="True"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <FooterStyle CssClass="grdrowfont"></FooterStyle>
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
            </asp:GridView>
            <asp:Button ID="btnprintUCR" runat="server" Text="Print UCR" CssClass="button" OnClientClick="javascript:popup();return false;" />
        </div>
        </div>
        <div>
            <table width="90%" cellpadding="3">
                <tr>
                    <td>
                        <%--No Of HAWB's--%>
                        Receiver's Name *
                    </td>
                    <td>
                        <asp:TextBox ID="txtreciversname" runat="server" Text='<%# Eval("ReciversName") %>'></asp:TextBox>
                        <asp:TextBox ID="txthawb" runat="server" Text='<%# Eval("HAWBNumber") %>' Visible="false">0</asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txthawb"
                            ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        Consignee Name *
                    </td>
                    <td>
                        <asp:TextBox ID="txtconsignee" runat="server" Text='<%# Eval("Consignee") %>'></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
           ControlToValidate="txtconsignee" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td rowspan="12">
                        <asp:GridView ID="grvDOChargesList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            BorderColor="#BFBEBE" BorderStyle="None" CellPadding="2" CellSpacing="3" PageSize="10"
                            ShowFooter="false" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Charge Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblChargeHeadCode" runat="server" Text='<%# Eval("ChargeHeadCode") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Charge">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCharge" runat="server" Text='<%# Eval("Charge") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="TAX">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTAX" runat="server" Text='<%# Eval("TAX") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="True" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="titlecolr" />
                            <RowStyle HorizontalAlign="Center" />
                            <AlternatingRowStyle HorizontalAlign="Center" />
                        </asp:GridView>
                    </td>
                    <%-- <td style="width:40px">
    DN Weight
   </td>
   <td style="width:40px">
       <asp:TextBox ID="txtdnweight" runat="server"></asp:TextBox>
       <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
           ControlToValidate="txtdnweight" ErrorMessage="*"></asp:RequiredFieldValidator>
   </td>
--%>
                </tr>
                <tr>
                    <td>
                        Issued To *
                    </td>
                    <td>
                        <asp:TextBox ID="txtissuedto" runat="server" Text='<%# Eval("IssuedTo") %>'></asp:TextBox>
                    </td>
                    <td>
                        Issued By *
                    </td>
                    <td>
                        <asp:TextBox ID="txtissuename" runat="server" ToolTip="Please Enter Agent Code" Text='<%# Eval("IssueName") %>'></asp:TextBox>
                        <%--
         <asp:ImageButton ID="imgcheck" runat="server" Height="1px" Visible="False" 
            Width="15px"  />   
         <asp:ImageButton ID="imgcross" runat="server" 
                                 Visible="False" Height="1px" />
--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        Issue Date *
                    </td>
                    <td>
                        <asp:TextBox ID="txtissuedate" runat="server" EnableViewState="true" Enabled="false"
                            Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png"
                            Visible="false" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtissuedate"
                            Format="dd/MM/yyyy" PopupButtonID="ImageButton1">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        Remarks
                    </td>
                    <td>
                        <asp:TextBox ID="txtDORemarks" runat="server" TextMode="MultiLine" Text="" Height="34px"
                            Width="149px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDockNumber" runat="server" Text="" Visible="false"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtDODate" runat="server" Text="" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table width="100%">
                            <tr>
                                <td style="width: auto">
                                    <asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click"
                                        Text="Save" Width="80px" />
                                    <span style="vertical-align: bottom;">
                                        <asp:ImageButton ID="btnOpsTime" runat="server" ImageUrl="~/Images/timecalender.png"
                                            Enabled="true" OnClick="btnOpsTime_Click" CssClass="imgclock" /></span>
                                    &nbsp;
                                    <asp:Button ID="btnprint" runat="server" CssClass="button" Text="Print" Width="80px"
                                        OnClick="btnprint_Click" Visible="false" />
                                    &nbsp;
                                    <asp:Button ID="BtnRecipt" runat="server" CssClass="button" OnClick="BtnRecipt_Click"
                                        Style="height: 23px" Text="Delivery Receipt" />
                                    &nbsp;
                                    <asp:Button ID="btnPrintDO" runat="server" CssClass="button" Enabled="False" OnClick="btnPrintDO_Click1"
                                        Style="height: 23px" Text="Print DO" Visible="False" />
                                    &nbsp;
                                    <asp:Button ID="btnReOpenDO" runat="server" CssClass="button" Text="Re-Open DO" OnClick="btnReOpenDO_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnEPouch" runat="server" Text="ePouch" CssClass="button" OnClick="btnEPouch_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <div style="float: right;">
        </div>
        <div id="fotbut" style="float: left; position: ">
            <div>
                <asp:HiddenField ID="HidFlag" runat="server" />
                <asp:HiddenField ID="hdRePrint" runat="server" />
            </div>
        </div>
    </div>
    <div id="msgfade" class="black_overlay">
    </div>
    <div id="divOpsTimePopup" class="white_content">
        <div style="margin: 10px;">
            <asp:Label ID="lblPnlError" runat="server" ForeColor="Red"></asp:Label>
            <h3>
                <asp:Label ID="lblOperationDetails" Text="Actual Operation Time" runat="server" Font-Bold="true"
                    Font-Size="Larger"></asp:Label>
            </h3>
            <hr />
            <div style="width: 350px;">
                <table width="100%" cellpadding="3" cellspacing="3">
                    <tr>
                        <td>
                            Date
                        </td>
                        <td>
                            <asp:TextBox ID="txtOpsDate" runat="server" Width="80px"></asp:TextBox>
                            <asp:ImageButton ID="ImageButton3" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                        </td>
                        <td style="width: 70px;">
                            <asp:TextBox ID="txtOpsTimeHr" runat="server" DataTextField="" Width="70px"></asp:TextBox>
                        </td>
                        <td style="width: 120px;" valign="bottom">
                            <asp:TextBox ID="txtOpsTimeMin" runat="server" DataTextField="" Width="70px"></asp:TextBox>
                            (HR:MI)
                        </td>
                        <td>
                            <asp:CalendarExtender ID="txtOpsDate_CalendarExtender" Format="dd/MM/yyyy" runat="server"
                                Enabled="True" TargetControlID="txtOpsDate" PopupButtonID="ImageButton3" PopupPosition="BottomLeft">
                            </asp:CalendarExtender>
                            <asp:NumericUpDownExtender ID="txtDeptTimeHr_NumericUpDownExtender" runat="server"
                                Maximum="23" Minimum="0" RefValues="00;01;02;03;04;05;06;07;08;09;10;11;12;13;14;15;16;17;18;19;20;21;22;23" ServiceDownMethod="" ServiceUpMethod=""
                                TargetButtonDownID="" TargetButtonUpID="" TargetControlID="txtOpsTimeHr" Width="40" />
                            <asp:NumericUpDownExtender ID="txtDeptTimeMin_NumericUpDownExtender1" runat="server"
                                Maximum="59" Minimum="0" RefValues="00;01;02;03;04;05;06;07;08;09;10;11;12;13;14;15;16;17;18;19;20;21;22;23;24;25;26;27;28;29;30;31;32;33;34;35;36;37;38;39;40;41;42;43;44;45;46;47;48;49;50;51;52;53;54;55;56;57;58;59" 
                                ServiceDownMethod="" ServiceUpMethod=""
                                TargetButtonDownID="" TargetButtonUpID="" TargetControlID="txtOpsTimeMin" Width="40" />
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnOpsSave" runat="server" Text="Save" CssClass="button" OnClick="btnOpsSave_Click" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnOpsCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnOpsCancel_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
