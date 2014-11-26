<%@ Page Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="RoleMaster.aspx.cs" Inherits="ProjectSmartCargoManager.RoleMaster" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">

    <script language="javascript" type="text/javascript">
    function postBackByObject() {
        var o = window.event.srcElement;
        if (o.tagName == "INPUT" && o.type == "checkbox") {
            __doPostBack("", "");
        }
    }

    </script>
    <script type="text/javascript">
    function OnTreeClick(evt) {
    var src = window.event != window.undefined ? window.event.srcElement : evt.target
    var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
    if (isChkBoxClick) {
        var parentTable = GetParentByTagName("table", src);
        var nxtSibling = parentTable.nextSibling;
        if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
        {
            if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
            {
                //check or uncheck children at all levels
                CheckUncheckChildren(parentTable.nextSibling, src.checked);
            }
        }
        //check or uncheck parents at all levels
        CheckUncheckParents(src, src.checked);
    }
}

function CheckUncheckChildren(childContainer, check) {
    var childChkBoxes = childContainer.getElementsByTagName("input");
    var childChkBoxCount = childChkBoxes.length;
    for (var i = 0; i < childChkBoxCount; i++) {
        childChkBoxes[i].checked = check;
    }
}

function CheckUncheckParents(srcChild, check) {
    var parentDiv = GetParentByTagName("div", srcChild);
    var parentNodeTable = parentDiv.previousSibling;

    if (parentNodeTable) {
        var checkUncheckSwitch;

        if (check) //checkbox checked
        {
            checkUncheckSwitch = true;
        }
        else //checkbox unchecked
        {
            var isAllSiblingsUnChecked = AreAllSiblingsUnChecked(srcChild);
            if (!isAllSiblingsUnChecked)
                checkUncheckSwitch = true;
            else
                checkUncheckSwitch = false;
        }

        var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
        if (inpElemsInParentTable.length > 0) {
            var parentNodeChkBox = inpElemsInParentTable[0];
            parentNodeChkBox.checked = checkUncheckSwitch;
            //do the same recursively
            CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
        }
    }
}

function AreAllSiblingsUnChecked(chkBox) {
    var parentDiv = GetParentByTagName("div", chkBox);
    var childCount = parentDiv.childNodes.length;
    for (var i = 0; i < childCount; i++) {
        if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
        {
            if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                //if any of sibling nodes are not checked, return false
                if (prevChkBox.checked) {
                    return false;
                }
            }
        }
    }
    return true;
}

//utility function to get the container of an element by tagname
function GetParentByTagName(parentTagName, childElementObj) {
    var parent = childElementObj.parentNode;
    while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
        parent = parent.parentNode;
    }
    return parent;
}
</script>
    <style type="text/css">
        .style1
        {
            width: 15%;
            height: 32px;
        }
        .style2
        {
            width: 10%;
            height: 32px;
        }
        .style3
        {
            height: 32px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
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
    <asp:UpdatePanel ID="upMainPanel" runat="server">
    <ContentTemplate>

    <div id="contentarea">
    <div class="msg">
    
    </div>
    
    <h1><img src="Images/txt_rolemaster.png" />
    <br />
        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
             ForeColor="Red"></asp:Label>
     </h1>
     <div class="botline">
                            <table>
                                
                               
                               
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRoleName" runat="server" Visible="false" Text="Role Name :"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtRoleName" runat="server"  Visible="false" ></asp:TextBox>
                                        <asp:Label ID="Label3" runat="server" Visible="false" Text="*"  ForeColor="Red"></asp:Label>
                                                                            </td>
                                    <td>
                                        <asp:DropDownList ID="DDLRoleName" runat="server" Visible="false" >
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    <asp:Label ID="lblRoleType" Text="Dashboard :" runat="server" Visible="false"></asp:Label>
                                    <asp:DropDownList ID="ddlRoleType" runat="server" Visible="false">
                                    <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                    <asp:ListItem>AGT</asp:ListItem>
                                    <asp:ListItem>OPS</asp:ListItem>
                                    <asp:ListItem>MGT</asp:ListItem>
                                    <asp:ListItem>PLN</asp:ListItem>
                                    <asp:ListItem>IT</asp:ListItem>
                                    <asp:ListItem>ACC</asp:ListItem>
                                    <asp:ListItem>None</asp:ListItem>
                                    </asp:DropDownList>
                                    </td>
                                    <td>                                        
                                        <asp:Button ID="btnGetRolRights" runat="server" Visible="false" Text="Get Role Rights"
                                            CssClass="button" onclick="Button1_Click"/>
                                    </td>
                                    <td>
                                        
                               </td>
                                    </tr>
                                </table></div>
 
<asp:ValidationSummary ID="ValidationSummary2" runat="server"  ShowMessageBox="true" ShowSummary="false" ValidationGroup="add"/>
    
        
       <asp:RegularExpressionValidator ID="REV1" runat="server"  Text="*" 
                                            ForeColor="#FFF7E7" ValidationExpression="[a-zA-Z]*" ValidationGroup="add" 
                                            ControlToValidate="txtRoleName" ErrorMessage="Role Name Invalid"></asp:RegularExpressionValidator>

                                        <asp:Label ID="Label2" runat="server" Style="" Text="Menu - Sub Menu - Pages" Width="400px"></asp:Label>
                          
                            <asp:TreeView ID="TreeView1"  onclick="javascript:OnTreeClick(event);" runat="server"
                                 NodeIndent="40">
                                <ParentNodeStyle ChildNodesPadding="10px" HorizontalPadding="10px" NodeSpacing="1px" />
                                <HoverNodeStyle ForeColor="Maroon" />
                                <NodeStyle ChildNodesPadding="5px" HorizontalPadding="10px" />
                                <LeafNodeStyle ChildNodesPadding="10px" HorizontalPadding="10px" VerticalPadding="5px" />
                            </asp:TreeView>
                            <table>
                            <tr>
                            <td>
                                    <asp:Button ID="btnNew" runat="server" Text="New Role" ValidationGroup="add" 
                                          CssClass="button" onclick="Button1_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnEdit" runat="server" Text="Edit Role" 
                                           CssClass="button" onclick="Button1_Click" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnView" runat="server" Text="View Role" 
                                           CssClass="button" onclick="Button1_Click"/>
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete Role" 
                                            CssClass="button" onclick="Button1_Click"/>    
                               </td>
                            </tr>
                            </table>
                            
                    
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="add" />
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