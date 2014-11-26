<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="ProjectSmartCargoManager.Help" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
//    function loadhelppanel() {
//        document.getElementById('frmdisply').style.width = '98%';
//    
//    }
</script>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
    
    <script type="text/javascript" src="skypoet.js"></script>
		<script type="text/javascript">
		   

		    function resolveSrcMouseover(e) {
		        var node = e.srcElement == undefined ? e.target : e.srcElement;
		        if (node.nodeName != "UL") {
		            node.style.fontWeight = "bold";
		            showRollover(e, node.innerHTML);
		        }
		    }
		    
		    
		    function resolveSrcMouseout(e) {
		        var node = e.srcElement == undefined ? e.target : e.srcElement;
		        node.style.fontWeight = "normal";
		        clearRollover(e);
		    }
		    function takeAction(e) {
		       
		        alert("Inside Javascript");
		        var node = e.srcElement == undefined ? e.target : e.srcElement;

		        //document.getElementById("DisplayInfo").innerHTML = "&nbsp;  &nbsp; &nbsp;            Help Desk " + node.innerHTML;

		        
		       
		        var id = node.getAttribute("id");
		        if (id != null && id.indexOf("Folder") > -1) {
		            if (node.innerHTML == "-") {
		                node.innerHTML = "+";
		                document.getElementById("ExpandCollapse" + id).style.display = "none";
		            } else if (node.innerHTML == "+") {
		                node.innerHTML = "-";
		                document.getElementById("ExpandCollapse" + id).style.display = "block";
		            }
		        }

		        changeFrame(node.id);

		    }
		</script>
		
    
    <script>
        function show() {
            if (document.getElementById('benefits').style.display == 'none') {
                document.getElementById('benefits').style.display = 'block';
                document.getElementById('needexpand').style.display = 'none';
                document.getElementById('expand').style.display = 'block';
                document.getElementById('frmdisply').style.width = '75%';
                document.getElementById('rightbutton').style.width = '75%';

            }
            return false;
        }
        function hide() {
            if (document.getElementById('benefits').style.display == 'block') {
                document.getElementById('benefits').style.display = 'none';

                document.getElementById('needexpand').style.display = 'block';
                document.getElementById('expand').style.display = 'none';
                document.getElementById('frmdisply').style.width = '98%';

                document.getElementById('rightbutton').style.width = '98%';

            }
            return false;
        }   
        </script>
        
         <script>
             function printDiv(divName) {
                 var printContents = document.getElementById(divName).innerHTML;
                 var originalContents = document.body.innerHTML;

                 document.body.innerHTML = printContents;

                 window.print();

                 document.body.innerHTML = originalContents;
             }

             function changeFrame(newPage) {
                 alert("inside ChangeFrame");
                 alert(newPage);
                 var URL = document.getElementById(newPage).href;
                 alert("URL:" + URL);
                 document.getElementById("frmdisply").src = URL;
                 alert("outside Iframe");
             }
        </script>
        
     
     <style>
		#leftmenu li {
			     	cursor:pointer; 
				display:block; 
				width:171px;
			}
			.Folder {
			}
			.ExpandCollapse {
				float:left;
				margin-right:5px;
				width:8px;
			}
			#leftmenu ul {
				list-style-type:none;
			}
			
			.previous-topic-button{ background:url("Images/arrowprev.png") no-repeat; width:25px; height:25px; border:0px;  margin-left:15px; margin-top:6px;}
			.next-topic-button{ background:url("Images/arrownext.png") no-repeat; width:25px; height:25px; border:0px; margin-left:15px; margin-top:6px; }
			.print-button{background:url("Images/printer.png") no-repeat; width:25px; height:25px; vertical-align:middle; border:0px;  margin-top:6px; }
			.buttons{width:100%; *width:auto;}
			.button-group-container-left{float:left;}
			
			 #header
{
	width: 100%;
	position: absolute;
	top: 2px;
	z-index: 1;
	display: block;
	height: 78px;
	left: 4px;
}



.search-bar
{
	border: solid 1px #363636;
	-webkit-border-radius: 15px;
	-moz-border-radius: 15px;
	border-radius: 15px;
	position: absolute;
	top: 50%;
	right: 14px;
	display: block;
	border-top: solid 1px #c0c0c0;
	border-right: solid 1px #c0c0c0;
	border-bottom: solid 1px #c0c0c0;
	border-left: solid 1px #c0c0c0;
	border-top-left-radius: 15px;
	border-top-right-radius: 15px;
	border-bottom-left-radius: 15px;
	border-bottom-right-radius: 15px;
	margin-top: -14px;
	box-shadow: 0px 1px 0px #7d7d7d;
	-webkit-box-shadow: 0px 1px 0px #7d7d7d;
	-moz-box-shadow: 0px 1px 0px #7d7d7d;
}
			
			#helppanel { width:100%; position:relative; margin-left:-3px;}
			.lefthelp { width:10px; position:absolute; height:599px; top:160; left:0;}
			.lefthelppanel{float:left; width:24%; height:547px; padding-top:10px; }
			#leftmenu{/*border:1px solid #0279ff;*/ height:100%; margin-top:8px; color:#0f2461;}
			.righthelppanel{width:auto; border:2px solid #0279ff;}
			
			
		</style>

    
    
    <div id="helppanel">
    <div class="lefthelp">
    <a onclick="return hide();" style="display:none; margin-top:0px; margin-left:245px;"  id="expand">
        <img src="Images/needtohide.png" style="height:582px; border:0px;" /></a><br />
    <a href="#1" name="1" onclick="return show();" style="display:block; margin-top:0px; margin-top:-22px;"  id="needexpand"><img src="Images/view.png" style=" margin-top:5px; height:582px; border:0px;"/></a> </div>
        
        
        <div style="display:none;" class="lefthelppanel" id="benefits">
        <span style="margin-left:15px;">Help</span>
        <div id="leftmenu" style=" margin-left:-27px; margin-top:20px; "> <ul  onmouseover="resolveSrcMouseover(event);" onmouseout="resolveSrcMouseout(event);" onclick="takeAction(event); return false; ">
		
<li><div id="Folder1" class="ExpandCollapse">-</div><div class="Folder">Modules</div></li>
			<ul id="ExpandCollapseFolder1">
				<li><a href="AgentMaster.aspx" id="Sales" >Sales</a></li> 
				<li>Planning</li>
				<li>Booking</li>
				<li>Operations</li>
				<li>Track/Audit</li>
				<li>Accounting</li>
				<li>Reports</li>
				<li>Configuration</li>
                
                
                
                
			</ul>
			<li><div id="Folder2" class="ExpandCollapse">-</div><div class="Folder">Tutorials</div></li>
			<ul id="ExpandCollapseFolder2">
              
				<li>User Roles and Management</li>
				<li>Agent Management</li>
				<li>Partner Management</li>
				<li>Rates Management</li>
				<li>Booking to Billing</li>
				<li>Interline Booking to Billing</li>
				<li>Configuring Master Data</li>
				<li>Pre-configured Reports</li>
				<li>Active Analysis Reports</li>
				
				
				
			</ul>
			<li><div id="Folder4" class="ExpandCollapse">-</div><div class="Folder">Messages</div></li>
			<ul id="ExpandCollapseFolder4">
			<li><a href="help/NewBooking.aspx#import" id="importprocess">Import Process</a></li>
			<li><a href="help/NewBooking.aspx#Export" id="Exportprocess">Export Process</a></li>
			
			</ul> 
			
			
			<li><div id="Folder5" class="ExpandCollapse">-</div><div class="Folder">How Tos</div></li>
			<ul id="ExpandCollapseFolder5">
			<li><a href="help/NewBooking.aspx#Newbooking" id="Createbooking"> Create booking</a></li>
			<li>Confirm booking</li>
			<li>Accept Cargo</li>
			<li>Plan Flight</li>
			<li>Manifest Flight</li>
			<li>Arrive Cargo</li>
			<li>Break Consolidated Cargo</li>
			<li>Deliver Cargo</li>
		</ul> 
		
		</div>
        </div>
        
                <div  class="righthelppanel">
                <div class="buttons">
                   
                    <table width="74%" id="rightbutton"><tr><td> <button class="previous-topic-button" title="Navigate previous">
                            </button>
                            <button class="next-topic-button" title="Navigate next">
                            </button></td><td align="right"><button class="print-button" title="Print" onclick="printDiv('frmdisply')"></button>
    <input type="text" style=" width:121px;" value="Search" /></td></tr></table>
                </div>
                <div style="width:100%; *width:auto; margin:0px; background:white; border-top:2px solid #0279ff; height:540px;"  id="DisplayInfo"  >
                <span style="margin-left:10px;"></span><br />
                

                
                   <%-- <img src="Images/underconstruction.jpg" style="margin-left:45px;" />--%>
                   
                
                   
                   
                   
                   
               <iframe name="myiframe" id="frmdisply"  onload="loadhelppanel()"  src=""  height="96%" style="margin-left:11px;  border:0px; *width:98%; *border:0px;"></iframe>
                

                
                
                
</div>
    </div>
    </div>
    
   
    
    </div>

     
     

</asp:Content>
