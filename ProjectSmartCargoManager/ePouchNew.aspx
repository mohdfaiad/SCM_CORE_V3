<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ePouchNew.aspx.cs" Inherits="ProjectSmartCargoManager.ePouchNew" MasterPageFile="~/SmartCargoMaster.Master" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<%-- onclick='radioClick(this);' how_many_clicked='0'--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
     function RadioCheck(rb) {
        var gv = document.getElementById("<%=grdePouch.ClientID%>");
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
</script>
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ToolkitScriptManager ID="Toolscriptmanager1" runat="server" EnablePageMethods="true"></asp:ToolkitScriptManager>
 
 
 <script type="text/javascript">

     function ViewEmailSplit() {
         document.getElementById('EmailPopup').style.display = 'block';
         document.getElementById('HideEmailPopup').style.display = 'block';
     }
     function HideEmailSplit() {
         document.getElementById('EmailPopup').style.display = 'none';
         document.getElementById('HideEmailPopup').style.display = 'none';
     }

     function ViewDocTypeSplit() {
         document.getElementById('divAddDocTypeShow').style.display = 'block';
         document.getElementById('divAddDocTypeHide').style.display = 'block';
     }
     function HideDocTypeSplit() {
         document.getElementById('divAddDocTypeShow').style.display = 'none';
         document.getElementById('divAddDocTypeHide').style.display = 'none';
     }

     function validateFileUpload(obj) {
         var fileName = new String();
         var fileExtension = new String();

         // store the file name into the variable  
         fileName = obj.value;

         // extract and store the file extension into another variable
         fileExtension = fileName.substr(fileName.length - 5, 5);
         fileExtension = fileName.substring(fileName.indexOf("."));
         //alert(fileExtension);

         // array of allowed file type extensions  
         var validFileExtensions = new Array(".jpg", ".png", ".gif",".pdf",".xls",".doc",".xlsx",".docx");

         var flag = false;

         // loop over the valid file extensions to compare them with uploaded file
         for (var index = 0; index < validFileExtensions.length; index++) {
             //alert(fileExtension.toLowerCase() + "   " + validFileExtensions[index].toString().toLowerCase());
             if (fileExtension.toLowerCase() == validFileExtensions[index].toString().toLowerCase()) {
                 flag = true;
             }
         }

         // display the alert message box according to the flag value  
         if (flag == false) {
             alert('Files with extension "' + fileExtension.toUpperCase() + '" are not allowed.\n\nYou can upload the files with following extensions only:\n.jpg\n.png\n.gif\n.pdf\n.xls\n.xlsx\n.doc\n.docx');
             return false;
         }
         else {
             return true;
         }
     }

  
</script>     
    <style>
        .black_overlay_Email
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
        .white_content_Email
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
.black_overlaynew
		{
			display: none;
			position: absolute;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 200%;
			background-color: black;
			z-index:1001;
			-moz-opacity:0.8;
			opacity:0.4;
			filter:alpha(opacity=80);
		}
	.white_contentnew 
		{
		    margin:0 auto;
			display: none;
			position:fixed;
			top: 15%;
			left: 30%;
			height: 40%;
			padding: 16px;
			border: 16px solid #ccdce3;
			background-color: white;
			z-index:1002;
		
            
		}
		.white_contentnew_Doc 
		{
		    margin:0 auto;
			display: none;
			position:fixed;
			left: 30%;
			padding: 16px;
			border: 16px solid #ccdce3;
			background-color: white;
			z-index:1002;
		
            
		}
		.black_overlaymsg{
			display: none;
			position: fixed;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 100%;
			background-color: White;
			z-index:1001;
			-moz-opacity: 0.8;
			opacity:.80;
			filter: alpha(opacity=80);
		}
		.white_contentmsg {
			display: none;
			position: fixed;
			top: 45%;
			left: 45%;
			width: 5%;
			height: 5%;
			padding: 16px;
			background-color: Transparent;
			z-index:1002;
			
		}
		
        </style>
     <script type="text/javascript">
         function Alert() {
             alert("inside javascript"); 
         }
     </script>
    
    <style>
        .black_radio
		{
			display: none;
			position: absolute;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 1000px;
			background-color: black;
			z-index:1001;
			-moz-opacity:0.8;
			opacity:0.4;
			filter:alpha(opacity=80);
		}
	.white_radio 
		{
		    margin:0 auto;
			display: none;
			position:fixed;
			top: 15%;
			left: 40%;
			padding: 16px;
			border: 16px solid #ccdce3;
			background-color: white;
			z-index:1002;
		
            
		}
    </style>
    
    
    
    <script type="text/javascript">
        function CheckOtherIsCheckedByGVID(spanChk) {

            var IsChecked = spanChk.checked;
            if (IsChecked) {
            }
            var CurrentRdbID = spanChk.id;
            var Chk = spanChk;
            Parent = document.getElementById("<%=grdePouch.ClientID%>");
            var items = Parent.getElementsByTagName('input');
            for (i = 0; i < items.length; i++) {
                if (items[i].id != CurrentRdbID && items[i].type == "radio") {
                    if (items[i].checked) {
                        items[i].checked = false;

                    }
                }
            }
        }

        function radioClick(e) {

            var flag = e.getAttribute('how_many_clicked');
            var times = Number(flag);
            times += 1;
            e.setAttribute('how_many_clicked', times.toString())
            if (times > 1) {
                e.checked = false;
                e.setAttribute('how_many_clicked', "0");
            }
            else {
                e.checked = true;
            }
        }
        function Download(count) {
            window.open('Download.aspx?FileName=' + count, 'Download', 'menubar=0, toolbar=0, location=0, status=0, resizable=0, width=100, height=50');
        }
</script>
    
    <%--<script src="jquery-1.10.1.js"></script>--%>
    <script src="jquery-1.7.2.min.js" ></script>
    
    <script src='lightbox.js'></script>
    <link href="style/lightbox.css" rel="stylesheet" />

    <script type="text/javascript">

       Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);
        
         function callShow()
         {
             document.getElementById('msglight').style.display = 'block';
             document.getElementById('msgfade').style.display = 'block';
             document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

         }
         function callclose()
         {
             document.getElementById('msglight').style.display = 'none';
             document.getElementById('msgfade').style.display = 'none';
         }

         function ViewPanelSplit() {
             document.getElementById('Lightsplit').style.display = 'block';
             document.getElementById('fadesplit').style.display = 'block';
         }
         function HidePanelSplit() {
             document.getElementById('Lightsplit').style.display = 'none';
             document.getElementById('fadesplit').style.display = 'none';
         }
         function View() {
             document.getElementById('searchshow').style.display = 'block';
             document.getElementById('searchhide').style.display = 'block';
         }
         function Hide() {
             document.getElementById('searchshow').style.display = 'none';
             document.getElementById('searchhide').style.display = 'none';
         }

     
     </script>
          <script type="text/javascript">
              function Gethref() {
                  var href2 = document.getElementById("lboxhref");
                  var source = document.getElementById('<%= InvoiceImage.ClientID%>');
                  href2.href = source.src;

              }
     </script>
     
     
  <%--
      <asp:UpdatePanel id="UpdatePanel1" runat="server">
    <ContentTemplate >--%>
    
     
   
    <div id="contentarea">
    
     <h1 style="font-size:large;">ePouch</h1>
     <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Size="Large" Font-Bold="true"></asp:Label>
<%--     <asp:Image ID="ImgLabel" runat="server" value="" />
--%>    
     
            
         <div class="botline" >
         <table width="42%">
         <tr>
         <td>         
         <asp:Label ID="lblAWBNo" runat="server" Text="AWB : "></asp:Label>
</td>
<td>         
<asp:TextBox ID="txtAWBPrefix" runat="server" ReadOnly="true" Width="60px" MaxLength="3"></asp:TextBox>
<asp:TextBoxWatermarkExtender ID="txtAWBPrefixWatermarkExtender" runat="server"
    TargetControlID="txtAWBPrefix" WatermarkText="Prefix" />
</td>
<td>         
<asp:TextBox ID="txtAWBNo" runat="server" ReadOnly="true" Width="100px" MaxLength="8"></asp:TextBox>
<asp:TextBoxWatermarkExtender ID="txtAWBNoWatermarkExtender" runat="server"
    TargetControlID="txtAWBNo" WatermarkText="AWB#" />
</td>
<td>
    <asp:Button ID="btnShow" runat="server" CssClass="button" 
        onclick="btnShow_Click" Text="Show" Visible="false" />
        <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add Doc. Type" OnClientClick="ViewDocTypeSplit(); return false;"
                  />
</td>
</tr>
</table> </div>

        <div style="width:700px; float:left; overflow:auto;">
        <asp:GridView ID="grdePouch" runat="server" AutoGenerateColumns="False" 
                CssClass="grdrowfont" ShowFooter="false"  
                 >
                <AlternatingRowStyle CssClass="trcolor" />
                <Columns>
                    <asp:TemplateField HeaderStyle-Wrap="true">
                        <ItemTemplate>
                           <asp:RadioButton ID="rdbePouch" runat="server"  onclick="javascript:RadioCheck(this)"  />
                        </ItemTemplate>
                        <HeaderStyle Wrap="True" />
                        <ItemStyle Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Document Name">
                    <ItemStyle Width="30%" />
                        <ItemTemplate>
                            <asp:Label ID="lbldocumentName" runat="server"  
                                Text='<%# Eval("DocumentName") %>' ></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Wrap="True" />
                        <ItemStyle Wrap="False" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Uploaded">
                    <ItemStyle Width="30%" />
                        <ItemTemplate>
                            <asp:Label ID="lblIsUploaded" runat="server"  
                                Text='<%# Eval("IsUploaded") %>'  ></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Wrap="True" />
                        <ItemStyle Wrap="True" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Files Uploaded">
                    <ItemStyle Width="30%" />
                        <ItemTemplate>
                            <asp:Label ID="lblUploadedFiles" runat="server"  
                                Text='<%# Eval("UploadedFiles").ToString().Length>25?Eval("UploadedFiles").ToString().Substring(0,20)+"..":Eval("UploadedFiles") %>' 
                                ToolTip='<%# Eval("UploadedFiles") %>'  ></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Wrap="True" />
                        <ItemStyle Wrap="True" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderStyle-Wrap="true" ItemStyle-Wrap="true"  >
                        <ItemTemplate>
                         <asp:FileUpload ID="fileupload_ePouch" runat="server" onchange="javascript:validateFileUpload(this)" />
                         <asp:RegularExpressionValidator runat="server" ID="valUpTest" ControlToValidate="fileupload_ePouch" 
             
            ValidationExpression="^.+\.((jpg)|(JPG)|(gif)|(GIF)|(jpeg)|(JPEG)|(png)|(PNG)|(bmp)|(BMP)|(pdf)|(PDF)|(xls)|(XLS)|(xlsx)|(XLSX)|(docx)|(DOCX)|(doc)|(DOC))$" />
                        </ItemTemplate>
                        <HeaderStyle Wrap="True" />
                        <ItemStyle Wrap="true"  />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Wrap="true" >
                        <ItemTemplate >
                            <asp:Button ID="btnUpload" runat="server" Text="Save"  CssClass="button" 
                 onclick="btnUpload_Click" CommandArgument='<%# Eval("SerialNumber") %>'  />
                        </ItemTemplate>
                        <HeaderStyle Wrap="True" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                    <ItemTemplate>
                    <asp:Label ID="AWBNo" runat="server" Text='<%# Eval("AWBNumber") %>' />
                    <asp:Label ID="AirlinePrefix" runat="server" Text='<%# Eval("AirlinePrefix") %>' />
                    <asp:Label ID="DocumentNo" runat="server" Text='<%# Eval("DocumentNo") %>' />
                    <asp:Label ID="DocumentType" runat="server" Text='<%# Eval("DocumentType") %>' />
                    
                    </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
                <EditRowStyle CssClass="grdrowfont" />
                <FooterStyle CssClass="grdrowfont" />
                <HeaderStyle CssClass="titlecolr" Height="30px" />
                <RowStyle CssClass="grdrowfont" />
            </asp:GridView>
            
             <asp:Button ID="btnDisplay" Text="Display" CssClass="button" runat="server" UseSubmitBehavior="true" 
                 onclick="btnDisplay_Click" />
            <asp:Button ID="btnSend" Text="Send" runat="server" CssClass="button" 
                 onclick="btnSend_Click"/>
            
             <asp:Button ID="btnPrint" runat="server" CssClass="button" Text="Print" 
                 onclick="btnPrint_Click" />
                 
              
            
         </div>
        
            
               
       
    
          <div style="float:right; width:300px; padding-top:5px;">
         
         <table cellpadding="2" cellspacing="2" width="300px" style="margin:0 auto; float:right;"   >
     <tr>
     <td><asp:ImageButton ID="btnPrev" runat="server"  ImageUrl="~/Images/prev.png" Height="25px" Width="25px" 
            onclick="btnPrev_Click" Visible="false"/></td>
            
            <td align="center" style="font-weight:bold; font-size:30px; color:#919394;"><asp:Label ID="lblPageNo" runat="server"></asp:Label></td>
            <td align="right">
     <asp:ImageButton ID="btnNext" runat="server" ImageUrl="~/Images/next.png" Height="25px" Width="25px" 
            onclick="btnNext_Click" Visible="false"/></td>
            </tr>
            </table>
        
        <asp:Panel ID="pnlImage" runat="server" >
        <a id="lboxhref" rel="lightbox" title="Image"  >
            
            <asp:ImageButton ID="InvoiceImage" ImageAlign="AbsMiddle"  runat="server" BorderColor="Black" 
                BorderStyle="Solid" Width="100%" Height="100%" BorderWidth="1" BackColor="#000000" OnClientClick="javascript:Gethref()" value="" Visible="false"   />
                </a>
            </asp:Panel>
            </div>
           
        
    
    <div id="msglight" class="white_contentmsg" >
<table>

<tr>

<td width="5%" align="center">
    <br />
    <img src="Images/loading.gif" />
    <br />
    <asp:Label ID="msgshow" runat="server" ></asp:Label>
</td>
</tr>
</table>
		</div>
		<div id="msgfade" class="black_overlaymsg"></div>
		   
<div id="Lightsplit"  class="white_contentnew" ></div>

		<div id="fadesplit" class="black_overlaynew"></div>
		<br />
		</div>
		<div id="EmailPopup" class="white_content_Email">
        <%--<table>
            <tr>
                <td>
                    <asp:Label ID="lblMsgType" runat="server" Text="Message Type :" ForeColor="Blue"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Blue"></asp:Label>
                </td>
            </tr>
        </table>--%>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblEmail" runat="server" Text="To Email ID (Comma Seprated EmailID):"
                        ForeColor="Blue"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <table width="100%">
            <tr>
                <td>
                    <asp:TextBox ID="txtEmailID" runat="server" TextMode="MultiLine" Width="300px" Height="50px"> </asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtEmailID" runat="server" ErrorMessage="Please Enter Email ID" ControlToValidate="txtEmailID" ValidationGroup="send"></asp:RequiredFieldValidator>
 <asp:RegularExpressionValidator ID="RegularExpression_txtEmailID" runat="server" ErrorMessage="Please Enter Email ID in correct format" ControlToValidate="txtEmailID" ValidationGroup="send" 
 ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([,]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"></asp:RegularExpressionValidator>
                    <%--<asp:RequiredFieldValidator "RequiredValidator_txtEmailID" runat="server" ControlToValidate="txtEmailID"></asp:RequiredFieldValidator>
                </td>--%>
                <td>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnOK" CssClass="button" runat="server" Text="OK" 
                        onclick="btnOK_Click" ValidationGroup="send"  />
                    <input type="button" id="btnCancel" class="button" value="Cancel" onclick="HideEmailSplit();" />
                </td>
            </tr>
        </table>
    </div>
    <div id="HideEmailPopup" class="black_overlay_Email">
    </div>
		<div id="divAddDocTypeShow" class="white_contentnew_Doc">
		<asp:Label ID="lblDocStatus" runat="server"></asp:Label>
		<table>
		<tr>
		<td>
			<asp:Label ID="lblDocType" runat="server" Text="Document Type:"></asp:Label>
		    <asp:TextBox ID="txtDocType" runat="server" Width="100px"></asp:TextBox>
		    		<asp:Button ID="btnSaveDocType" runat="server" Text="Save" CssClass="button" 
                onclick="btnSaveDocType_Click" />
		<asp:Button ID="btnDocTypeCancel" runat="server" Text="Cancel" CssClass="button" OnClientClick="HideDocTypeSplit(); return false;" />
		</td>
		</tr>
		</table>
	

		</div>
		<div id="divAddDocTypeHide" class="black_overlaynew"></div>
    </asp:Content>