<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewBooking.aspx.cs" Inherits="ProjectSmartCargoManager.NewBooking" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="../style/style.css" rel="stylesheet" type="text/css" />
    <style>
    h1.help{ font-size:18px; color:#0681b4; background:#f4f5f5; padding:5px; width:100%; margin-top:10px;}
    .step{font-size:18px; color:#0681b4;  padding:5px; font-weight:bold;}
    div.sendpara{ margin-left:10px;}
    .substep{font-size:18px; color:#000;  padding:5px;}
    img{border:1px #ccc;}
    </style>
</head>
<body style="background:#fff; margin-left:10px;">
    <form id="form1" runat="server">
    <div id="Newbooking">
    <h1 class="help">Booking: Quick Reference Manual</h1>
    <p>
    <span class="step">Step1:</span> Mandatory: Enter Consignment Details  

        <br />

        <div class="sendpara">
                <span class="substep">a.</span>	Origin is automatically selected based upon login location. 
        <br />
        <span class="substep">b.</span>	Select final Destination of AWB: <img src="images/destination.gif" />
 
        <br />
                <span class="substep">c.</span>	Select Agent Code: Agent Login will automatically default its own details.  
                <br />  
                <div style="margin-left:25px;">(For Superuser All Agents permitted to book for the Origin are available). 
IATA Code, Agent Name and Agent Booking Currency is automatically populated from Master Data.<br />
       
            <img src="images/agentcodedetails.gif" /> </div> <br />
               <span class="substep"> d.</span> Optional: Select Product Type and Special Handling Code if applicable&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </div>

        </p>
        <p>
            <span class="step">Step2:</span> Optional: Enter Shipper Consignee Details.  

</p>
        <p>
            <span class="step">Step3:</span> Mandatory: Enter Shipment Details<br />
            <div class="sendpara">
<span class="substep">a.</span>	Enter Commodity Code, Enter Pieces and Weight
            <br />
            <span class="substep">b.</span>	Optional: Enter dimensions for each piece, Enter ULD numbers. 
                
            <br />
            <span class="substep">c.</span>	Select Paymode<br />
                <img src="images/shipement.gif" />
             </div>

</p>
        <p>
            <span class="step">Step4:</span> Build Route: 
            <br /><div class="sendpara">
                       <span class="substep"> a.</span>	Select Next Destination, Flight Date and Flight Number<img src="images/route.gif" />


</div>


</p>
        <p>
            <span class="step">Step 6:</span> Process Rates. System looks up the correct rate line associated with the shipment 
            <br /><img src="images/processrate.gif" />
 
</p>
        <p>
             <span class="step">Step 7:</span> Save Booking
      <br />   
      <div class="sendpara">   
<span style=" color:green; font-size:18px; font-weight:bold;">RESULT: Booking is saved and a AWB is created</span><br />
<img src="images/bookawb.gif" /></div></p>

    </div>
  
    <div id="Export">
    <h1 class="help">processing the shipment through available messages-Export Process</h1>
   
     
    <span class="step">Step1:</span><strong>Confirm Shipment for Export.</strong>  
     <br /> <div class="sendpara">
      
         <span class="substep">a.</span>Go to<strong>Track/Audit=>Messaging=>Monitor Messages.</strong> 
        <br />
        <span class="substep">b.</span>	Clicking on one of the messages shows…( To proceed  with Export operations select an FBL Message) Cicking on “Show” will open Message content as below ( You can refer it for your verification).
         <br />
         <img src="images/rptmessage.jpg" height="150" style="border:1px solid #ccc;"/><br />
        <span class="substep">c.</span><strong>Booking-> List,</strong> with the help of search options provided you can select particular AWB.
For Ex: Enter the date range and click on List AWB’s will be listed.
                <br />
         <img src="images/listbook.gif" height="100" style="border:1px solid #ccc;" />
                 </div> 
   
    <p>
    <span class="step">Step2:</span><strong>Accept Shipment:</strong><br />
    <div class="sendpara"><span class="substep">a.</span>To accept the shipment go to <strong>Operations =>Accept=> Cargo Acceptance.</strong> <br /> 
    <span class="substep">b.</span>Search particular AWB with the help of available search options and click on list<br />
    <span class="substep">c.</span>Select an AWB from the list as below, Accept the pcs and Wt. completely or one can edit the details on particular field. If you want to alter the accepting values edit <strong>Acc Pcs</strong> and <strong>Acc Wt</strong> columns.<br />
        <img src="images/cargoacceptance.gif" height="150" style="border:1px solid #ccc;" /><br />
     <span class="substep">d.</span>Click on Save. Shipment will be accepted successfully.<br />
     <span style=" color:green; font-size:18px; font-weight:bold;">RESULT: Shipment accepted for 020-111118881 on 28/02/2014 05:59:43 PM</span><br />
<br />
    </div>
    </p>
    <p>
    <span class="step">Step3:</span><strong>Screen Cargo:</strong><br />
    <div class="sendpara">
    <span class="substep">a.</span>  
After Acceptance, Shipment needs to go through the security check. For Security Check click on <strong>Operations=>Accept=>Cargo Screening.</strong> <br />
<span class="substep">b.</span>For Cargo Screening, Find out your shipment with the help of available search options either by date or by entering the AWB number itself. List of AWBs will be listed as below.<br />
        <img src="images/cargoscreening.gif" height="150" style="border:1px solid #ccc;"/><br />
<span class="substep">c.</span>Select one or multiple AWBs from Unscreened cargo and click on button “ Mark screened”  then the window will display you a pop-up window with the details of selected AWBs where in one can add the count of AWB pieces according to the various security checks they have gone through.<br />
<span class="substep">d.</span>Enter the AWB piece count as below and click on “OK”. AWB will be scanned successfully.<br />
        <img src="images/markscreened.gif" style="border:1px solid #ccc;" /><br />
<span class="substep">e.</span>The  Print Scr  Rpt   button can be used to print the screening report  as shown on next<br />
        <img src="images/reportscreening.gif" height="100" /><br />
</div></p>
<p>
<span class="step">Step4:</span><strong>Plan Flight:</strong>
<div class="sendpara"><span class="substep">a.</span>Once AWB is scanned visit <strong>Operations->Export->Flight Planning,</strong> Here you can assign AWB’s to ULD or BULK to plan a flight.<br />
<span class="substep">b.</span>To add as a Bulk AWB, Select the AWB and click on “Assign to Bulk”. AWB will be listed in Bulk AWB’s Panel. Click on “Save Bulk AWB” <br />
<span class="substep">c.</span>To assign the AWB to a new ULD, Click on “New ULD” from ULD Load panel. A row will be available to provide the details of ULD. In the below case ULD # used is “AKE14445LH” along with POU as AWB destination YYZ. Other details are optional. Click on save ULD will be saved successfully.<br />
    <img src="images/planflight.gif"  style="border:1px solid #ccc;"/><br />
<span class="substep">d.</span>From Bulk load select the AWB and click on Assign to ULD as below.<br />
    <img src="images/bulkload.gif" height="200"  style="border:1px solid #ccc; "/><br />
<span class="substep">e.</span>AWB will be available in the Assigned AWB’s. Click on “Save” button ULD will be saved along with the AWB successfully.<br />
<span class="substep">f.</span>ULD plan can be printed with the help of <strong>“Print Wt Stmt”</strong>,Complete load Plan can be printed with the help of <strong>“Print Load Plan”.</strong><br />
    <img src="images/containerpallet.gif" height="250" style="border:1px solid #ccc;" />
</div>
</p>
<p>
<div class="sendpara">
<span class="step">Step5:</span><strong>Manifest Flight:</strong><br />
<span class="substep">a.</span><strong>Operations->Export-> Export Manifest :</strong> If from Flight planning the AWB is added as bulk will be available in the AWB Tab. In this case we have added the AWB to an ULD so the AWB is available under the  ULD tab.<br />
    <img src="images/exportmanifest.gif"  style="border:1px solid #ccc;"/><br />
<span class="substep">b.</span>Select ULD and click on “Add to manifest” to add the ULD to manifest. Use “Save”, “Finalize” and “Depart Flt” buttons to finalize and depart the flight.<br />
<span class="substep">c.</span>Use “Print Mft” button to print the Manifest.<br />
    <img src="images/cargomanifestrpt.gif" height="250" style="border:1px solid #ccc;"/>
</div></p>

</div>
<div  id="import">
<div class="sendpara">
<h1 class="help">processing the shipment through available messages-Import Process</h1>
<p>
<span class="step">Step1:</span><strong>FFM Message creates Import record (Automated).</strong><br />
<span class="substep">a.</span>
Go to<strong>Track/Audit=>Messaging=>Monitor Messages.</strong> Select a FFM Message from Monitor Messages. Screen appears as below <br />
<img src="images/rptmessage.jpg" height="150" style="border:1px solid #ccc;"/><br />
</p>
<p>
<span class="step">Step2:</span><strong>Recording Arrival of shipments.</strong><br />
<span class="substep">a.</span>To visit Arrival click on <strong>Operations->Import->Arrival.</strong><br />Enter Flight Number and date, 
    <br /><img src="images/arrival.gif" style="border:1px solid #ccc;"/>Click on List. List of AWB’s will be listed as below. 
    <img src="images/ulddetails.gif" style="border:1px solid #ccc;"/><br />
<span class="substep">b.</span>Select ULD for recording Arrival<br />
<span class="substep">c.</span>Enter Received Pcs & Received Weight for each AWB. Select AWBs for arrival.<br />
<span class="substep">d.</span>Click “Save” to record arrival data of ULDs as well as AWBs.<span style=" color:green; font-size:18px; font-weight:bold;">RESULT: Arrival record Saved successfully!</span><br />
<br />
</p>
<p>
<span class="step">Step3:</span><strong>Recording actual arrival time of shipments.</strong><br />
<span class="substep">a.</span>After saving Arrival record, popup to record Actual Arrival Time shall be displayed.<br />
<span class="substep">b.</span>Select appropriate actual date and time of arrival and click “Save”.<br />
<span class="substep">c.</span>Close button [X] at top right of the page can be used to close the popup.<br /><img src="images/actualoperationtime.gif" style="border:1px solid #ccc;"/><br />
</p>
<p>
<span class="step">Step4:</span><strong>Recording Break Down of ULDs</strong><br />
 <span class="substep">a.</span>To visit Arrival click on <strong>Operations->Import->Break ULD.</strong>Enter Flight # and date to list the ULD’s arrived.<br />
 <span class="substep">b.</span>ULDs recorded in Arrival shall only be available for Break down.<br />
    <img src="images/breakulddetails.gif" style="border:1px solid #ccc;"/><br />
  <span class="substep">c.</span>Select the ULD you want to break.<br />
  <span class="substep">d.</span>Click on Break From ULD button, AWBs of the ULD will be listed in AWB Details<br />
  <span class="substep">e.</span>Select AWBs those are to be broken down from ULD.<br /><img
      src="images/afterbreak.gif" style="border:1px solid #ccc;"/><br />
  <span class="substep">f.</span>Click on Save to save ULD break down. Record will be saved.
  </p>
  <p>
<span class="step">Step5:</span><strong>Recording Delivery of shipments.</strong><br />
<span class="substep">a.</span>From Delivery page enter Flight and date. Click List to view list of ULDs and AWBs available for Delivery.<br />
<span class="substep">b.</span>Enter Delivered Pieces & Weight in Dlr Pcs and Dlr Wt fields against AWB.<br />
<span class="substep">c.</span>Select ULDs and AWBs which are to be delivered.<br />
<span class="substep">d.</span>Click on “Save” to record Delivery.<br /><img src="images/ulddeliversave.gif" style="border:1px solid #ccc;"/><br />
</p>
<p>
<span class="step">Step6:</span><strong>Recording actual delivery time of shipments.</strong><br />
<span class="substep">a.</span>After saving Delivery record, popup to record Actual Delivery Time shall be displayed.<br />
<span class="substep">b.</span>Select appropriate actual date and time of delivery and click “Save”.<br />
<span class="substep">c.</span>Close button [X] at top right of the page can be used to close the popup.<br />
    <img src="images/actulopertime.gif" style="border:1px solid #ccc;" />
</p>
<p>
<span class="step">Step7:</span><strong>Printing DO.</strong>
<span class="substep">a.</span>After save, click on Print to Print the DO<br /><img src="images/deliverorderimport.gif" style="border:1px solid #ccc;"/>
</p>
</div>

</div>
   
   
    </form>
</body>
</html>
