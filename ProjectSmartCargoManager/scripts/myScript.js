function getLoginResponse(){

	//alert(localStorage.username +"-"+localStorage.passowrd);
					//http://72.167.41.153:8023/DashboardData.asmx
					$.ajax({
			            //url: 'http://localhost:1727/ADAPT_INVOICE_PROJECTIONS/InvoiceProjections.asmx/getUserDetails',
			            url: 'http://72.167.41.153:8026/InvoiceProjections.asmx/getUserDetails',
			      
			            type: "POST",
			            contentType: "application/json; charset=utf-8",
			            dataType: "json",
			  			//data: "{'ABC':'Sujay','MNO':'1/1/2013','XYZ':'1/2/2013'}",
			            
			  			//data: "{'LoginName':'vishal.tillu','Password':'vishal'}",
			  			
			  			data: "{'LoginName':'"+localStorage.username+"','Password':'"+localStorage.password+"'}",
						  
			            beforeSend: function() {
		                    var $this = $( this ),
		                        theme = "b",
		                        msgText = "Logging In, Please wait...",
		                        textVisible = "true",
		                        textonly = !!$this.jqmData( "textonly" );
		                        html = $this.jqmData( "html" ) || "";
		                    $.mobile.loading( "show", {
		                            text: msgText,
		                            textVisible: textVisible,
		                            theme: theme,
		                            textonly: textonly,
		                            html: html
		                    });
		                
			            },
			            complete: function(){
			            	//alert("Complete");
			            	$.mobile.loading( "hide" );
			            },
			            success: function (response) {
			            
			            	
			            	if(response.d =='{"Table":[]}')
		            		{
		            		
		            			//alert("No data");
		            			//$("#popupCloseRight").click();
		            			$("#loginfail").popup("open");
		            		}
		            	else
		            		{
		            			
		            			localStorage.LoginRespData = response.d;
					            var LoginResponseData = $.parseJSON(localStorage.LoginRespData);
					            
					            document.location.href="#homepg";				
		            		}
				        },
						error: function (request,error) {
							
		            	//alert('Network error has occurred please try again!');
							$("#neterror1").popup("open");
								
		        	}
			            
				});
	}

function getResponse(){
			
	
		var month= localStorage.month;
		var year = localStorage.year;
		//alert("Inside Response");
		$.ajax({
            url: "http://72.167.41.153:8026/InvoiceProjections.asmx/getInvoiceProjection",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
  			//data: "{'ABC':'Sujay','MNO':'1/1/2013','XYZ':'1/2/2013'}",
            data: "{'Month':'"+month+"','Year':'"+year+"','Days':'10'}",
  			//data: "{'Location':'','FromDate':'"+FromD+"','ToDate':'"+ToD+"'}",
            
  			//timeout: 10000,
            beforeSend: function() {
                    var $this = $( this ),
                        theme = "b",
                        msgText = "Loading, Please wait...",
                        textVisible = "true",
                        textonly = !!$this.jqmData( "textonly" );
                        html = $this.jqmData( "html" ) || "";
                    $.mobile.loading( "show", {
                            text: msgText,
                            textVisible: textVisible,
                            theme: theme,
                            textonly: textonly,
                            html: html
                    });
                
            },
            complete: function(){
            	//alert("Complete");
            	$.mobile.loading( "hide" );
            },
            success: function (response) {
            	//document.write(response.d);
            	if(response.d =='{"Week1":[],"Week2":[],"Week3":[],"Week4":[],"InvoiceValues":[{"Week":"Week1","TotalInvValue":"0"},{"Week":"Week2","TotalInvValue":"0"},{"Week":"Week3","TotalInvValue":"0"},{"Week":"Week4","TotalInvValue":"0"}]}')
            		{
            		
            			$("#ndf").popup("open");
            		}
            	else
            		{
			            localStorage.RespData = response.d;
			            var ResponseData = $.parseJSON(localStorage.RespData);
			  	            			           			            
			            Weeks = [ResponseData.InvoiceValues[0].Week, ResponseData.InvoiceValues[1].Week, ResponseData.InvoiceValues[2].Week,ResponseData.InvoiceValues[3].Week];
			    		//InvoiceValues = [ResponseData.InvoiceValues[0].TotalInvValue, ResponseData.InvoiceValues[1].TotalInvValue, ResponseData.InvoiceValues[2].TotalInvValue,ResponseData.InvoiceValues[3].TotalInvValue];
			    		

			    		var a= Math.round(ResponseData.InvoiceValues[0].TotalInvValue);
			    		var b = Math.round(ResponseData.InvoiceValues[1].TotalInvValue);
			    		var c = Math.round(ResponseData.InvoiceValues[2].TotalInvValue);
			    		var d = Math.round(ResponseData.InvoiceValues[3].TotalInvValue);
			    		InvoiceValues=[a,b,c,d];
			            localStorage.Weeks = JSON.stringify(Weeks);
						localStorage.InvoiceValues = JSON.stringify(InvoiceValues);
												
						document.location.href="#weekpg";
						
            		}
	        },
			error: function (request,error) {
        	//alert('Network error has occurred please try again!');
				$("#neterror2").popup("open");
    	}			
	});
}


var fnLoadGraphs = function(ser,tick,targetdiv)
{
alert("Inside Load Graphs");
	$.jqplot.config.enablePlugins = true;
    var s1 = ser; 
    var ticks = tick;
    var targetd = targetdiv;
     
    plot1 = $.jqplot(targetd, [s1], {
        // Only animate if we're not using excanvas (not in IE 7 or IE 8)..
        animate: !$.jqplot.use_excanvas,
        
        seriesDefaults:{
            renderer:$.jqplot.BarRenderer,
            pointLabels: { show: true }
        },
        axesDefaults: {
            tickRenderer: $.jqplot.CanvasAxisTickRenderer ,
            tickOptions: {
              angle: -30,
              fontSize: '10pt'
            }
        },
        axes: {
            xaxis: {
                renderer: $.jqplot.CategoryAxisRenderer,
                ticks: ticks
                
            }
        },
        highlighter: { show: false }
    });
   
    
    //alert("m inside");
  /*  $(targetd).bind('jqplotDataClick',
        function (ev, seriesIndex, pointIndex, data) {
            $('#info1').html('series: '+seriesIndex+', point: '+pointIndex+', data: '+data);
        }
    );*/
    
    return plot1;

	
};


var fnLoadPie = function(tdiv,data){
	
	//alert(data);
	$('#title1').html('<h3>Vendor Distribution</h3>');
    var plot2 = $.jqplot(tdiv, [data], {
    	grid: {
            drawBorder: false,
            drawGridlines: false,
            background: '#ffffff',
            shadow:false
        },
        axesDefaults: {
             
        },
        gridPadding: {top:0, bottom:38, left:0, right:0},
        seriesDefaults:{
            renderer:$.jqplot.PieRenderer, 
            trendline:{ show:false }, 
            rendererOptions: { padding: 8, showDataLabels: true }
        },
        
    });
    
    plot2.replot("clear");
    
    return plot2;
};

function fnLoadPieGraph(RespD, tdiv,pointIndex)
{
	var data = new Array();
	//		alert("Inside Pie");
	var ResponseData = RespD; //$.parseJSON(RespD);
		 	//Weeks = [ResponseData.InvoiceValues[0].Week, ResponseData.InvoiceValues[1].Week, ResponseData.InvoiceValues[2].Week,ResponseData.InvoiceValues[3].Week];
	    	
		 	//alert(ResponseData);
		 	//alert(ResponseData.Week1.length);
		 	
	
	    var tblcontent = '<table data-role="table" id="table-column-toggle" data-mode="columntoggle" class="ui-responsive table-stroke"> <tr><th>Vendor Name</th><th>Amount Due</th></tr><thead><tbody>';
	    
    

		if(pointIndex == 0)
		{
		 	
		 //	for(var j=0;j < 3; j++)
		 //		{
		 	
			 	for(var i=0; i < ResponseData.Week1.length; i++)
			 		{
			 			
			 			//alert(ResponseData.Week1[i].VendorName);
			 			//ResponseData.Week1[i].VendorName,ResponseData.Week1[i].InvoiceAmt
			 			var vendor = [ResponseData.Week1[i].VendorName,ResponseData.Week1[i].InvoiceAmt];
			 			data.push(vendor);
			 			tblcontent += '<tr><td>' + ResponseData.Week1[i].VendorName + '</td><td>' + ResponseData.Week1[i].InvoiceAmt + '</td></tr>';
			 			//alert(data.length);
			 		}
		 //		}
			
		 }
		if(pointIndex == 1)
		{
		 	
		 //	for(var j=0;j < 3; j++)
		 //		{
		 	
			 	for(var i=0; i < ResponseData.Week2.length; i++)
			 		{
			 			
			 			//alert(ResponseData.Week1[i].VendorName);
			 			//ResponseData.Week1[i].VendorName,ResponseData.Week1[i].InvoiceAmt
			 			var vendor = [ResponseData.Week2[i].VendorName,ResponseData.Week2[i].InvoiceAmt];
			 			data.push(vendor);
			 			tblcontent += '<tr><td>' + ResponseData.Week2[i].VendorName + '</td><td>' + ResponseData.Week2[i].InvoiceAmt + '</td></tr>';
			 			
			 			//alert(data.length);
			 		}
		 //		}
			
		 }
		if(pointIndex == 2)
		{
		 	
		 //	for(var j=0;j < 3; j++)
		 //		{
		 	
			 	for(var i=0; i < ResponseData.Week3.length; i++)
			 		{
			 			
			 			//alert(ResponseData.Week1[i].VendorName);
			 			//ResponseData.Week1[i].VendorName,ResponseData.Week1[i].InvoiceAmt
			 			var vendor = [ResponseData.Week3[i].VendorName,ResponseData.Week3[i].InvoiceAmt];
			 			data.push(vendor);
			 			tblcontent += '<tr><td>' + ResponseData.Week3[i].VendorName + '</td><td>' + ResponseData.Week3[i].InvoiceAmt + '</td></tr>';
			 			
			 			//alert(data.length);
			 		}
		 //		}
			
		 }
		if(pointIndex == 3)
		{
		 	
		 //	for(var j=0;j < 3; j++)
		 //		{
		 	
			 	for(var i=0; i < ResponseData.Week4.length; i++)
			 		{
			 			
			 			//alert(ResponseData.Week1[i].VendorName);
			 			//ResponseData.Week1[i].VendorName,ResponseData.Week1[i].InvoiceAmt
			 			var vendor = [ResponseData.Week4[i].VendorName,ResponseData.Week4[i].InvoiceAmt];
			 			data.push(vendor);
			 			tblcontent += '<tr><td>' + ResponseData.Week4[i].VendorName + '</td><td>' + ResponseData.Week4[i].InvoiceAmt + '</td></tr>';
			 			
			 			//alert(data.length);
			 		}
		 //		}
			
		 }
		
		tblcontent += ' </tbody></table>';
	
		$('#tblData').html(tblcontent);
	//alert(data.length);
	var myplot= fnLoadPie(tdiv,data);
	return myplot;
	
}

function generateTable()
{
    var tr, td, i, oneRecord;
    var myDiv = document.createElement('DIV');
    myTable = document.createElement('TABLE');
    myTable.setAttribute("border", 1);
    // node tree
    var data = node.getElementsByTagName("NewDataSet")[0];
    for (i = 0; i < data.childNodes.length; i++) {
        // use only 1st level element nodes to skip 1st level text nodes in NN
        if (data.childNodes[i].nodeType == 1) {
            // one final match record
            oneRecord = data.childNodes[i];
            tr = myTable.insertRow(myTable.rows.length);
            td = tr.insertCell(tr.cells.length);                    
            td.innerHTML = oneRecord.getElementsByTagName("Name")[0].firstChild.nodeValue;
            td = tr.insertCell(tr.cells.length);
            td.innerHTML = oneRecord.getElementsByTagName("Age")[0].firstChild.nodeValue;
            td = tr.insertCell(tr.cells.length);
            td.innerHTML = oneRecord.getElementsByTagName("Org")[0].firstChild.nodeValue;
        }
    }           
    myDiv.appendChild(myTable);
    //return myDiv.innerHTML;
    
}


function exitApplication()
{
		 localStorage.clear();
         navigator.app.exitApp();

}