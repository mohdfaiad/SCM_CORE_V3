<%@ Page Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" 
CodeBehind="MaintainAWB.aspx.cs" Inherits="ProjectSmartCargoManager.MaintainAWB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="True">
    </asp:ToolkitScriptManager>
    
  <div id="contentarea">
      <%--msg-lblstatus--%>
      <div style="float:right;" class="msg">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
            ForeColor="Red"></asp:Label></ContentTemplate></asp:UpdatePanel>
       </div><%--end msg-lblstatus--%>
       
       <h1><asp:Label ID="lblPageName" runat="server" Text="Update AWB Information" /></h1>
       
       <%--botline--%>
       <div class="botline">
                 <table width="100%">
                    <tr>
                    <td><asp:DropDownList ID="ddlDocType" runat="server" ></asp:DropDownList>
                    &nbsp;&nbsp;
                     <asp:TextBox ID="txtAwbPrefix" runat="server" MaxLength="3" Width="40px" CssClass="alignrgt"
                        AutoPostBack="true" ></asp:TextBox>
                   <%-- <asp:TextBox ID="txtAwbPrefix" runat="server" MaxLength="3" Width="40px" CssClass="alignrgt"
                        AutoPostBack="true" OnTextChanged="txtAwbPrefix_TextChanged" ></asp:TextBox>--%>
                    <%--<asp:AutoCompleteExtender ID="ACEAwbPrefix" BehaviorID="ACEAwbPrefix" runat="server"
                        ServiceMethod="GetPartnerPrefix" CompletionInterval="0" EnableCaching="false"
                        CompletionSetCount="10" TargetControlID="txtAwbPrefix" MinimumPrefixLength="1"
                        OnClientPopulated="onAWBPrefixPopulated">
                    </asp:AutoCompleteExtender>--%>
                  
                        <asp:TextBox ID="txtAWBNo" runat="server" MaxLength="8"  CssClass="alignrgt"   AutoPostBack="false"></asp:TextBox>
                    
                        <asp:DropDownList ID="ddlAirlineCode" runat="server" Width="50px"  
                            AutoPostBack="true"></asp:DropDownList>
                         &nbsp;<asp:Button ID="btnListAgentStock" runat="server" Text="List" CssClass="button"  />
                    &nbsp;<asp:Button ID="btnClearAgentStock" runat="server" Text="Clear" CssClass="button"  /> 
                     </td>    
                     <td></td>            
                     </tr>
                  </table>   
        </div><%--end botline--%>
        
        <%--Divdetail--%>
        <div id="divdetail" style="height: auto;">
        
            <%--colleft--%>
                 <div id="colleft" style="height: auto; width:500px; margin-right:25px; border-right:1px solid #ccc;">
                <strong>General Information</strong>
                <div style="width: 478px;">
                    <asp:UpdatePanel ID="Update" runat="server">
                        <ContentTemplate>
                            <table border="0" cellspacing="2" cellpadding="2" width="100%">
                                <tr>
                                    <td>
                                        Commodity Code *
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCommodityCode" runat="server" Width="110px" CssClass="styleUpper"
                                            onchange="GetCommodityCode(this); SetProcessFlag();" ></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="ACECommCode" BehaviorID="ACECommCode" runat="server"
                                                        ServiceMethod="GetCommodityCodesWithName" CompletionInterval="0" EnableCaching="false"
                                                        CompletionSetCount="10" TargetControlID="txtCommodityCode" MinimumPrefixLength="1"
                                                        OnClientPopulated="onCommListPopulated" FirstRowSelected="true">
                                                    </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCommodityName" runat="server" Width="110px" Enabled="true" ></asp:TextBox>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        Agent 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TXTAgentCode" runat="server" Width="110px"  CssClass="styleUpper" AutoPostBack="true" Enabled="false" ></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACEAgentCode" BehaviorID="ACEAgentCode" runat="server"
                                            ServiceMethod="GetAgentCodeWithName" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="TXTAgentCode" MinimumPrefixLength="1"
                                            OnClientPopulated="onListPopulated" FirstRowSelected="true">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td><asp:TextBox ID="txtAgent" runat="server" Width="110px" Enabled="true" ></asp:TextBox>
                                     </td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        Shipper
                                    </td>
                                    <td><asp:TextBox ID="TextBo" runat="server" Width="110px" Enabled="false" ></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtShipperCode" runat="server" MaxLength="50"  Width="110px" onchange="SetProcessFlag();GetShipperCode(this);" 
                                         AutoPostBack="false"> </asp:TextBox>
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                            OnClientClick="javascript:ViewPanel_shipperPopUp();return false;" 
                                             CssClass="InputImageFocus" ></asp:ImageButton>
                                        
                                        
                                        <asp:AutoCompleteExtender ID="ACESHPCode" runat="server" BehaviorID="ACESHPCode"
                                                        CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
                                                        MinimumPrefixLength="1" ServiceMethod="GetShipperCode" 
                                                        TargetControlID="txtShipperCode" OnClientPopulated="onShipperListPopulated" FirstRowSelected="true">
                                                    </asp:AutoCompleteExtender>
                                    </td>
                                    
                                    </tr>
                                    <tr>
                                    <td>
                                        Consignee
                                    </td><td><asp:TextBox ID="TextBox17" runat="server" Width="110px" Enabled="false" ></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtConsigneeCode" runat="server" AutoPostBack="false" MaxLength="50"
                                            onchange="SetProcessFlag();GetConsigneeCode(this);" 
                                            Width="110px"> </asp:TextBox>
                                            <asp:ImageButton ID="ImageButton3" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                            OnClientClick="javascript:ViewPanel_ConsigneePopUp();return false;" 
                                             CssClass="InputImageFocus" ></asp:ImageButton>
                                       
                                        <asp:AutoCompleteExtender ID="ACEConCode" runat="server" BehaviorID="ACEConCode"
                                                        CompletionInterval="0" CompletionSetCount="10" EnableCaching="false" 
                                                        MinimumPrefixLength="1" ServiceMethod="GetShipperCode" 
                                                        TargetControlID="txtConsigneeCode" OnClientPopulated="onConsigneeListPopulated" FirstRowSelected="true">
                                                    </asp:AutoCompleteExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Prod. Type
                                    </td><td><asp:TextBox ID="TextBox18" runat="server" Width="110px" Enabled="false" ></asp:TextBox></td>
                                    <td>
                                        <asp:DropDownList ID="ddlProductType" runat="server" CssClass="alignrgt" onchange="SetProcessFlag();"
                                            Width="110px" >
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="imgProductType" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                            OnClientClick="javascript:GetMatchingProductTypes();return false;" 
                                             CssClass="InputImageFocus"></asp:ImageButton>
                                    </td>
                                  </tr><tr>
                                    <td>
                                        SHC
                                    </td><td><asp:TextBox ID="TextBox19" runat="server" Width="110px" Enabled="false" ></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtSpecialHandlingCode" runat="server" onchange="SetProcessFlag();"
                                             Width="110px"></asp:TextBox>
                                        <asp:ImageButton ID="ISHC" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                            OnClientClick="javascript:GetSpecialHandlingCode();return false;"  
                                            />
                                    </td>
                                </tr>
                                    
                                    <tr>
                                    <td>
                                        Handling Info.
                                    </td><td><asp:TextBox ID="TextBox20" runat="server" Width="110px" Enabled="false" ></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtHandling" runat="server" Width="110px" Enabled="true"
                                            ></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        Remarks
                                    </td><td><asp:TextBox ID="TextBox21" runat="server" Width="110px" Enabled="false" ></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="TextBox2" runat="server" Width="110px" Enabled="true"
                                            ></asp:TextBox>
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                              CssClass="InputImageFocus"></asp:ImageButton>
                                    </td>
                                </tr>
                                
                                
                                <tr>
                                    <td colspan="3">
                                            <asp:Button ID="btnSave" runat="server" Text="Check" CssClass="button"  />                                            
                                            
                                    </td>                                    
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                
            </div><%--end colleft--%>
           
            <%--colright--%> 
                 <div id="colright" style="width: 450px; margin-left:20px; height: auto;">
            
            <asp:UpdatePanel ID="updatepanelRateDetails" runat="server">
            <ContentTemplate>
            <div style="margin-top:5px; margin-left:10px; width:581px;">
            <strong>Flight Information </strong>
                    <br />
            <div>
            
            <div style="width:100%">
            <table width="80%" cellpadding="1" cellspacing="1">
            <tr><td>Dest </td><td><asp:TextBox ID="TextBox16" runat="server" Width="120px" Enabled="false" 
                                                  ></asp:TextBox></td>
                                            <td>
                                                &nbsp;</td> 
                                            <td>
                                                <asp:TextBox ID="txtdest" runat="server"  Width="121px"></asp:TextBox>
                                          </td>
                                            </tr>
                                            <tr> <td>
                                                Accp Pcs Wt:
                                            </td><td><asp:TextBox ID="TextBox4" runat="server" Width="55px" ></asp:TextBox>&nbsp; &nbsp;<asp:TextBox ID="TextBox5" runat="server" Width="55px" ></asp:TextBox></td>
                                            <td>
                                                &nbsp;</td> 
                                                <td>
                                                    <asp:TextBox ID="txtpcs" runat="server"  Width="50px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="ImageButton4" runat="server" CssClass="InputImageFocus" 
                                                        ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"  />
                                                    &nbsp;<asp:TextBox ID="TextBox3" runat="server"  Width="48px"></asp:TextBox>
                                                </td>
                                           </tr>
                                            
                                           <tr> 
                                            <td>Chargeable Wt</td><td><asp:TextBox ID="TextBox15" runat="server" Width="121px"  Enabled="false"></asp:TextBox></td><td>
                                               &nbsp;</td>
                                               <td>
                                                   <asp:TextBox ID="txtspotid" runat="server" Enabled="false" Width="121px"></asp:TextBox>
                                               </td>
                                        </tr>
                                        <tr><td>Flight Info</td><td><asp:TextBox ID="TextBox6" runat="server" Width="70px" ></asp:TextBox><asp:TextBox ID="TextBox8" runat="server" Width="50px" Enabled="false" ></asp:TextBox></td><td>
                                            &nbsp;</td>
                                            <td>
                                                <asp:TextBox ID="TextBox7" runat="server" Width="70px"></asp:TextBox>
                                                <asp:TextBox ID="TextBox9" runat="server" Enabled="false" 
                                                    Width="50px"></asp:TextBox>
                                            </td>
                </tr>                               
                                <tr><td colspan="4">
                                <div width="100%">
                                 <asp:Button ID="btncheck" runat="server" Text="Check" CssClass="button ltfloat"  />
                                </div></td>
                                </tr>
                                <tr><td colspan="4"> <hr /></td></tr>
                                </table>
                               
             </div>
             
             
             <table width="80%" cellpadding="1" cellspacing="1">
             <tr><td>Declared Value</td><td><asp:TextBox ID="TextBox10" runat="server" Width="121px"  Enabled="false"></asp:TextBox></td>
                 <td><asp:TextBox ID="TextBox11" runat="server" Width="121px"  Enabled="false"></asp:TextBox></td></tr>
             <tr><td>Spot Rate</td><td><asp:TextBox ID="TextBox12" runat="server" Width="121px"  Enabled="false"></asp:TextBox></td><td><asp:TextBox ID="TextBox13" runat="server" Width="121px"  Enabled="false"></asp:TextBox></td></tr>
             <tr><td>Payment Mode</td><td><asp:TextBox ID="TextBox14" runat="server" Width="121px"  Enabled="false"></asp:TextBox></td>
                                    <td>
                                        <asp:DropDownList ID="ddlPaymentMode" runat="server" Width="121px" >
                                            <asp:ListItem Selected="True">Select</asp:ListItem>
                                            <asp:ListItem>PP</asp:ListItem>
                                            <asp:ListItem>CC</asp:ListItem>
                                            <asp:ListItem>FOC</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                  </tr>
                                  <tr><td colspan="3">
                                <div width="100%">
                                 <asp:Button ID="Button1" runat="server" Text="Check" CssClass="button ltfloat"  />
                                </div></td>
                                </tr>
                                      
             </table>
             </div>
             </div>
             
               </ContentTemplate>
              
               
            </asp:UpdatePanel>
            
        </div><%--end colright--%>
            
            
        </div>
        <%--end Divdetail--%>
        
       
        <div style="float:left; width:100%">  <hr /><div style="width:300px; margin:10px auto;">
        
        <div style=" border:1px solid #ccc;  height:100px; float:left; margin:10px; padding:10px; margin-top:0px; width:80%;">
        Text will be here
        </div><asp:ImageButton ID="ImageButton5" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png"
                                              CssClass="InputImageFocus"></asp:ImageButton><br /><asp:Button ID="Button4" runat="server" Text="Save" CssClass="button"  /><asp:Button ID="Button10" runat="server" Text="clear" CssClass="button"  /></div></div>
        
        
        </div><%--end Contentarea--%>

</asp:Content>
