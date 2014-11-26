<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="DriverMaps.aspx.cs" Inherits="ProjectSmartCargoManager.DriverMaps" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="Content1">
<style type="text/css">
html { height: 100% }
body { height: 100%; margin: 0; padding: 0 }
#map_canvas { height: 100% }
</style>
<script type="text/javascript" src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyC6v5-2uaq_wusHDktM9ILcqIrlPtnZgEk&sensor=false">
</script>
<script type="text/javascript">
    function initialize(JSONString) {
        var markers = JSON.parse(JSONString);
        var mapOptions = {
            center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
            zoom: 2,
            mapTypeId: google.maps.MapTypeId.ROADMAP
            //  marker:true
        };
        var infoWindow = new google.maps.InfoWindow();
        var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
        for (i = 0; i < markers.length; i++) {
            var data = markers[i]
            var myLatlng = new google.maps.LatLng(data.lat, data.lng);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.title
            });
            (function(marker, data) {

                // Attaching a click event to the current marker
                google.maps.event.addListener(marker, "click", function(e) {
                    infoWindow.setContent(data.description);
                    //infoWindow.open(map, marker);
                    window.open('http://us01.infotrak.us/User/frames.php?Username=Infodemo&Password=infodemo&HOME_URL=../User&grop=qid', '', 'left=0,top=0,width=1000,height=500,toolbar=0,resizable=0');
                    
                });
            })(marker, data);
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
<div id="map_canvas" style="width: 1000px; height: 400px"></div></div>
</asp:Content>