using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MessageData
{
        public struct ffrinfo
        {
            public string ffrversionnum;
         
            //4 
            public string noofuld;
            //5 
            public string specialservicereq1;
            public string specialservicereq2;
            //6 
            public string otherserviceinfo1;
            public string otherserviceinfo2;
            //7
            public string bookingrefairport;
            public string officefundesignation;
            public string companydesignator;
            public string bookingfileref;
            public string participentidetifier;
            public string participentcode;
            public string participentairportcity;
           
            //9-productinfo
            public string servicecode;
            public string rateclasscode;
            public string commoditycode;
            //10 shipper info
            public string shipperaccnum;
            public string shippername;
            public string shipperadd;
            public string shipperplace;
            public string shipperstate;
            public string shippercountrycode;
            public string shipperpostcode;
            public string shippercontactidentifier;
            public string shippercontactnum;

            //11 consinee
            public string consaccnum;
            public string consname;
            public string consadd;
            public string consplace;
            public string consstate;
            public string conscountrycode;
            public string conspostcode;
            public string conscontactidentifier;
            public string conscontactnum;

            //12 customer identification
            public string custaccnum;
            public string iatacargoagentcode;
            public string cargoagentcasscode;
            public string custparticipentidentifier;
            public string custname;
            public string custplace;

            //13 shipment refernece info
            public string shiprefnum;
            public string supplemetryshipperinfo1;
            public string supplemetryshipperinfo2;

            public ffrinfo(string val)
            {
                ffrversionnum = val;
              
                //4 
                noofuld = val;
                //5 
                specialservicereq1 = val;
                specialservicereq2 = val;
                //6 
                otherserviceinfo1 = val;
                otherserviceinfo2 = val;
                //7
                bookingrefairport = val;
                officefundesignation = val;
                companydesignator = val;
                bookingfileref = val;
                participentidetifier = val;
                participentcode = val;
                participentairportcity = val;
              
                //9-productinfo
                servicecode = val;
                rateclasscode = val;
                commoditycode = val;
                //10 shipper info
                shipperaccnum = val;
                shippername = val;
                shipperadd = val;
                shipperplace = val;
                shipperstate = val;
                shippercountrycode = val;
                shipperpostcode = val;
                shippercontactidentifier = val;
                shippercontactnum = val;

                //11 consinee
                consaccnum = val;
                consname = val;
                consadd = val;
                consplace = val;
                consstate = val;
                conscountrycode = val;
                conspostcode = val;
                conscontactidentifier = val;
                conscontactnum = val;

                //12 customer identification
                custaccnum = val;
                iatacargoagentcode = val;
                cargoagentcasscode = val;
                custparticipentidentifier = val;
                custname = val;
                custplace = val;

                //13 shipment refernece info
                shiprefnum = val;
                supplemetryshipperinfo1 = val;
                supplemetryshipperinfo2 = val;
            }

        }

        public struct ffainfo
        {
            public string ffaversionnum;
            
            //2 consignment details
            public string airlineprefix;
            public string awbnum;
            public string origin;
            public string dest;
            public string consigntype;
            public string pcscnt;
            public string weightcode;
            public string weight;
            public string shpdesccode;
            public string numshp;
            public string manifestdesc;
            public string splhandling;
            //3 flight details
            public string carriercode;
            public string fltnum;
            public string date;
            public string month;
            public string fltdept;
            public string fltarrival;
            public string spaceallotmentcode;
            //4 
            public string specialservicereq1;
            public string specialservicereq2;
            //5 
            public string otherserviceinfo1;
            public string otherserviceinfo2;
            //6
            public string bookingrefairport;
            public string officefundesignation;
            public string companydesignator;
            public string bookingfileref;
            public string participentidetifier;
            public string participentcode;
            public string participentairportcity;

            //7 shipment refernece info
            public string shiprefnum;
            public string supplemetryshipperinfo1;
            public string supplemetryshipperinfo2;
            #region Constructor
            public ffainfo(string val)
            {
                ffaversionnum = val;
                //2 consignment details
                airlineprefix = val;
                awbnum = val;
                origin = val;
                dest = val;
                consigntype = val;
                pcscnt = val;
                weightcode = val;
                weight = val;
                shpdesccode = val;
                numshp = val;
                manifestdesc = val;
                splhandling = val;
                //3 flight details
                carriercode = val;
                fltnum = val;
                date = val;
                month = val;
                fltdept = val;
                fltarrival = val;
                spaceallotmentcode = val;
                //4 
                specialservicereq1 = val;
                specialservicereq2 = val;
                //5 
                otherserviceinfo1 = val;
                otherserviceinfo2 = val;
                //6
                bookingrefairport = val;
                officefundesignation = val;
                companydesignator = val;
                bookingfileref = val;
                participentidetifier = val;
                participentcode = val;
                participentairportcity = val;

                //7 shipment refernece info
                shiprefnum = val;
                supplemetryshipperinfo1 = val;
                supplemetryshipperinfo2 = val;
            }
            #endregion
        }

        public struct UCMInfo
        {
            public string FltNo;
            public string OutFltNo;
            public string Date;
            public string FltRegNo;
            public string StationCode;
            public UCMInfo(string str)
            {
                FltNo = str;
                Date = str;
                FltRegNo = str;
                StationCode = str;
                OutFltNo = str;
            }
        }

        public struct ULDinfo
        {//uldloadingindicator-use as cargo indicator for UCM
            public string uldno;
            public string uldtype;
            public string uldsrno;
            public string uldowner;
            public string uldloadingindicator;
            public string uldweightcode;
            public string uldweight;
            public string uldremark;
            public string portsequence;
            public string refuld;
            public string stationcode;
            public string movement;
            public ULDinfo(string str)
            {
                this.uldno = str;
                this.uldtype = str;
                this.uldsrno = str;
                this.uldowner = str;
                this.uldloadingindicator = str;
                this.uldweight = str;
                this.uldweightcode = str;
                this.uldremark = str;
                this.portsequence = str;
                this.refuld = str;
                this.stationcode = str;
                this.movement = str;
            }

        }

        public struct fblinfo
        {
            public string fblversion;
            //2 line flight ID & point of loadng            
            public string messagesequencenum;
            public string carriercode;
            public string fltnum;
            public string date;
            public string month;
            public string fltairportcode;
            public string aircraftregistration;

            //4 consigment info
            public string totalconsignment;

            //7 ULD info
            public string noofuld;

            //8 special service request
            public string specialservicereq1;
            public string specialservicereq2;

            //10 
            public string endmesgcode;

            public fblinfo(string str)
            {
                fblversion = str;
                //2 line flight ID & point of loadng            
                messagesequencenum = str;
                carriercode = str;
                fltnum = str;
                date = str;
                month = str;
                fltairportcode = str;
                aircraftregistration = str;

                //4 consigment info
                totalconsignment = str;

                //7 ULD info
                noofuld = str;

                //8 special service request
                specialservicereq1 = str;
                specialservicereq2 = str;

                //10 
                endmesgcode = str;
            }

        }

        public struct unloadingport
        {
            // point of unloading
            public string unloadingairport;
            public string nilcargocode;
            public string day;
            public string month;
            public string time;
            public string day1;
            public string month1;
            public string time1;
            public string sequencenum;
            public unloadingport(string str)
            {
                unloadingairport = str;
                nilcargocode = str;
                day = str;
                month = str;
                time = str;
                day1 = str;
                month1 = str;
                time1 = str;
                sequencenum = str;
            }
        }

        public struct consignmentorigininfo
        {
            // consignment origin info
            public string abbrivatedname;
            public string carriercode;
            public string flightnum;
            public string day;
            public string month;
            public string airportcode;
            public string movementprioritycode;

            public consignmentorigininfo(string str)
            {
                abbrivatedname = str;
                carriercode = str;
                flightnum = str;
                day = str;
                month = str;
                airportcode = str;
                movementprioritycode = str;
            }
        }

        public struct dimensionnfo
        {
            public string weightcode;
            public string weight;
            public string mesurunitcode;
            public string length;
            public string width;
            public string height;
            public string piecenum;
            public string consigref;
            public dimensionnfo(string str)
            {
                weightcode = str;
                weight = str;
                mesurunitcode = str;
                length = str;
                width = str;
                height = str;
                piecenum = str;
                consigref = str;
            }
        }

        public struct movementinfo
        {
            public string AirportCode;
            public string CarrierCode;
            public string FlightNumber;
            public string FlightDay;
            public string FlightMonth;
            public string PriorityorVolumecode;
            public string consigref;

            public movementinfo(string str)
            {
                AirportCode = str;
                CarrierCode = str;
                FlightNumber = str;
                FlightDay = str;
                FlightMonth = str;
                PriorityorVolumecode = str;
                consigref = str;
            }
        }

        public struct otherserviceinfo
        {
            public string otherserviceinfo1;
            public string otherserviceinfo2;
            public string consigref;
            public otherserviceinfo(string str)
            {
                otherserviceinfo1 = str;
                otherserviceinfo2 = str;
                consigref = str;
            }
        }

        public struct customsextrainfo
        {
            public string IsoCountryCodeOci;
            public string InformationIdentifierOci;
            public string CsrIdentifierOci;
            public string SupplementaryCsrIdentifierOci;
            public string consigref;
            public customsextrainfo(string str)
            {
                IsoCountryCodeOci = str;
                InformationIdentifierOci = str;
                CsrIdentifierOci = str;
                SupplementaryCsrIdentifierOci = str;
                consigref = str;
            }
        }

        public struct ffminfo
        {
            //line 1
            public string ffmversionnum;
            //line 2
            public string messagesequencenum;
            public string carriercode;
            public string fltnum;
            public string fltdate;
            public string month;
            public string time;
            public string fltairportcode;
            public string aircraftregistration;
            public string countrycode;
            public string fltdate1;
            public string fltmonth1;
            public string flttime1;
            public string fltairportcode1;
            //line 8
            public string customorigincode;
            //8 special service request
            public string specialservicereq1;
            public string specialservicereq2;
            //line 18
            public string endmesgcode;
            public ffminfo(string str)
            {
                ffmversionnum = str;
                //line 2
                messagesequencenum = str;
                carriercode = str;
                fltnum = str;
                fltdate = str;
                month = str;
                time = str;
                fltairportcode = str;
                aircraftregistration = str;
                countrycode = str;
                fltdate1 = str;
                fltmonth1 = str;
                flttime1 = str;
                fltairportcode1 = str;
                specialservicereq1 = str;
                specialservicereq2 = str;
                //line 8
                customorigincode = str;
                //line 18
                endmesgcode = str;
            }
        }

        public struct fwbinfo
        {
            public string fwbversionnum;
            //2 consignment details
            public string airlineprefix;
            public string awbnum;
            public string origin;
            public string dest;
            public string consigntype;
            public string pcscnt;
            public string weightcode;
            public string weight;
            public string volumecode;
            public string volumeamt;
            public string densityindicator;
            public string densitygrp;
            //3 flight booking details
            public string carriercode;
            public string fltnum;
            public string fltday;
            //4 routing
            public string routingairportcitycode;
            public string routingcarriercode;
            //5 shipper info
            public string shipperaccnum;
            public string shippername;
            public string shipperadd;
            public string shipperplace;
            public string shipperstate;
            public string shippercountrycode;
            public string shipperpostcode;
            public string shippercontactidentifier;
            public string shippercontactnum;
            //6 consignee info
            public string consaccnum;
            public string consname;
            public string consadd;
            public string consplace;
            public string consstate;
            public string conscountrycode;
            public string conspostcode;
            public string conscontactidentifier;
            public string conscontactnum;
            //7 agent info
            public string agentaccnum;
            public string agentIATAnumber;
            public string agentCASSaddress;
            public string agentParticipentIdentifier;
            public string agentname;
            public string agentplace;
            //8 special service request
            public string specialservicereq1;
            public string specialservicereq2;
            //9 also notify
            public string notifyname;
            public string notifyadd;
            public string notifyplace;
            public string notifystate;
            public string notifycountrycode;
            public string notifypostcode;
            public string notifycontactidentifier;
            public string notifycontactnum;

            //10 accounting info
            public string accountinginfoidentifier;
            public string accountinginfo;

            //11 charge declaration
            public string currency;
            public string chargecode;
            public string chargedec;
            public string declaredvalue;
            public string declaredcustomvalue;
            public string insuranceamount;

            //14 prepaid charge
            public string PPweightCharge;
            public string PPValuationCharge;
            public string PPTaxesCharge;
            public string PPOCDA;
            public string PPOCDC;
            public string PPTotalCharges;

            //15 collect charge
            public string CCweightCharge;
            public string CCValuationCharge;
            public string CCTaxesCharge;
            public string CCOCDA;
            public string CCOCDC;
            public string CCTotalCharges;

            //16 shipper certification
            public string shippersignature;

            //17 carrier execution
            public string carrierdate;
            public string carriermonth;
            public string carrieryear;
            public string carrierplace;
            public string carriersignature;

            //19 cc charges
            public string cccurrencycode;
            public string ccexchangerate;
            public string ccchargeamt;

            //20 sender ref
            public string senderairport;
            public string senderofficedesignator;
            public string sendercompanydesignator;
            public string senderFileref;
            public string senderParticipentIdentifier;
            public string senderParticipentCode;
            public string senderPariticipentAirport;

            //21 custom origin
            public string customorigincode;

            //22 commission info
            public string commisioncassindicator;
            public string commisionCassSettleAmt;

            //23 sales incentive info
            public string saleschargeamt;
            public string salescassindicator;

            //24 agent ref data
            public string agentfileref;

            //25 special handling detials
            public string splhandling;

            //26 nominated handling party
            public string handlingname;
            public string handlingplace;

            //27 shipment ref info
            public string shiprefnum;
            public string supplemetryshipperinfo1;
            public string supplemetryshipperinfo2;

            //28 other participent info
            public string othparticipentname;
            public string othairport;
            public string othofficedesignator;
            public string othcompanydesignator;
            public string othfilereference;
            public string othparticipentidentifier;
            public string othparticipentcode;
            public string othparticipentairport;

            public fwbinfo(string str)
            {
                fwbversionnum = str;
                //2 consignment details
                airlineprefix = str;
                awbnum = str;
                origin = str;
                dest = str;
                consigntype = str;
                pcscnt = str;
                weightcode = str;
                weight = str;
                volumecode = str;
                volumeamt = str;
                densityindicator = str;
                densitygrp = str;
                //3 flight booking details
                carriercode = str;
                fltnum = str;
                fltday = str;
                //4 routing
                routingairportcitycode = str;
                routingcarriercode = str;
                //5 shipper info
                shipperaccnum = str;
                shippername = str;
                shipperadd = str;
                shipperplace = str;
                shipperstate = str;
                shippercountrycode = str;
                shipperpostcode = str;
                shippercontactidentifier = str;
                shippercontactnum = str;
                //6 consignee info
                consaccnum = str;
                consname = str;
                consadd = str;
                consplace = str;
                consstate = str;
                conscountrycode = str;
                conspostcode = str;
                conscontactidentifier = str;
                conscontactnum = str;
                //7 agent info
                agentaccnum = str;
                agentIATAnumber = str;
                agentCASSaddress = str;
                agentParticipentIdentifier = str;
                agentname = str;
                agentplace = str;
                //8 special service request
                specialservicereq1 = str;
                specialservicereq2 = str;
                //9 also notify
                notifyname = str;
                notifyadd = str;
                notifyplace = str;
                notifystate = str;
                notifycountrycode = str;
                notifypostcode = str;
                notifycontactidentifier = str;
                notifycontactnum = str;

                //10 accounting info
                accountinginfoidentifier = str;
                accountinginfo = str;


                //11 charge declaration
                currency = str;
                chargecode = str;
                chargedec = str;
                declaredvalue = str;
                declaredcustomvalue = str;
                insuranceamount = str;

                //14 prepaid charge

                PPweightCharge = str;
                PPValuationCharge = str;
                PPTaxesCharge = str;
                PPOCDA = str;
                PPOCDC = str;
                PPTotalCharges = str;

                //15 collect charge
                CCweightCharge = str;
                CCValuationCharge = str;
                CCTaxesCharge = str;
                CCOCDA = str;
                CCOCDC = str;
                CCTotalCharges = str;

                //16 shipper certification
                shippersignature = str;

                //17 carrier execution
                carrierdate = str;
                carriermonth = str;
                carrieryear = str;
                carrierplace = str;
                carriersignature = str;

                //19 cc charges
                cccurrencycode = str;
                ccexchangerate = str;
                ccchargeamt = str;

                //20 sender ref
                senderairport = str;
                senderofficedesignator = str;
                sendercompanydesignator = str;
                senderFileref = str;
                senderParticipentIdentifier = str;
                senderParticipentCode = str;
                senderPariticipentAirport = str;

                //21 custom origin
                customorigincode = str;

                //22 commission info
                commisioncassindicator = str;
                commisionCassSettleAmt = str;

                //23 sales incentive info
                saleschargeamt = str;
                salescassindicator = str;

                //24 agent ref data
                agentfileref = str;

                //25 special handling detials
                splhandling = str;

                //26 nominated handling party
                handlingname = str;
                handlingplace = str;

                //27 shipment ref info
                shiprefnum = str;
                supplemetryshipperinfo1 = str;
                supplemetryshipperinfo2 = str;

                //28 other participent info
                othparticipentname = str;
                othairport = str;
                othofficedesignator = str;
                othcompanydesignator = str;
                othfilereference = str;
                othparticipentidentifier = str;
                othparticipentcode = str;
                othparticipentairport = str;
            }
        }

        public struct othercharges
        {
            public string indicator;
            public string otherchargecode;
            public string entitlementcode;
            public string chargeamt;

            public othercharges(string str)
            {
                indicator = str;
                otherchargecode = str;
                entitlementcode = str;
                chargeamt = str;
            }
        }

        public struct RateDescription
        {
            public string linenum;
            public string pcsidentifier;
            public string numofpcs;
            public string weightindicator;
            public string weight;
            public string rateclasscode;
            public string commoditynumber;
            public string awbweight;
            public string rateidentiier;
            public string chargerate;
            public string tarrifidentifier;
            public string chargeamt;
            public string goodsnature;
            public string goodsnature1;
            public string volcode;
            public string volamt;
            public string unit;
            public string length;
            public string width;
            public string height;
            public string pcscnt;
            public string uldtype;
            public string uldserialnum;
            public string uldowner;
            public string slac;
            public string hermonisedcomoditycode;
            public string isocountrycode;

            public RateDescription(string str)
            {
                linenum = str;
                pcsidentifier = str;
                numofpcs = str;
                weightindicator = str;
                weight = str;
                rateclasscode = str;
                commoditynumber = str;
                awbweight = str;
                rateidentiier = str;
                chargerate = str;
                tarrifidentifier = str;
                chargeamt = str;
                goodsnature = str;
                goodsnature1 = str;
                volcode = str;
                volamt = str;
                unit = str;
                length = str;
                width = str;
                height = str;
                uldtype = str;
                uldserialnum = str;
                uldowner = str;
                slac = str;
                hermonisedcomoditycode = str;
                isocountrycode = str;
                pcscnt = str;
            }

        }

        public struct CommonStruct
        {
            public string messageprefix;
            public string carriercode;
            public string seccarriercode;
            public string flightnum;
            public string fltday;
            public string fltmonth;
            public string flttime;
            public string fltorg;
            public string fltdest;
            public string pcsindicator;
            public string numofpcs;
            public string weightcode;
            public string weight;
            public string volumecode;
            public string volumeamt;
            public string densityindicator;
            public string densitygroup;
            public string name;
            public string daychangeindicator;
            public string timeindicator;
            public string depttime;
            public string arrivaltime;
            public string transfermanifestnumber;
            public string airportcode;
            public string infocode;

            public CommonStruct(string str)
            {
                messageprefix = str;
                carriercode = str;
                seccarriercode = str;
                flightnum = str;
                fltday = str;
                fltmonth = str;
                flttime = str;
                fltorg = str;
                fltdest = str;
                pcsindicator = str;
                numofpcs = str;
                weightcode = str;
                weight = str;
                volumecode = str;
                volumeamt = str;
                densityindicator = str;
                densitygroup = str;
                name = str;
                daychangeindicator = str;
                timeindicator = str;
                depttime = str;
                arrivaltime = str;
                transfermanifestnumber = str;
                airportcode = str;
                infocode = str;
            }

        }

        public struct FSAInfo
        {
            public string fsaversion;
         
            //2 consignment details
            public string airlineprefix;
            public string awbnum;
            public string origin;
            public string dest;
            public string consigntype;
            public string pcscnt;
            public string weightcode;
            public string weight;
            public string totalpcscnt;

            public FSAInfo(string str)
            {
                fsaversion = str;
                //2 consignment details
                airlineprefix = str;
                awbnum = str;
                origin = str;
                dest = str;
                consigntype = str;
                pcscnt = str;
                weightcode = str;
                weight = str;
                totalpcscnt = str;
            }
        }

        public struct fhlinfo
        {
            public string fhlversionnum;

            //2Master AWB Consignment Detail 
            public string airlineprefix;
            public string awbnum;
            public string origin;
            public string dest;
            public string consigntype;
            public string pcscnt;
            public string weightcode;
            public string weight;

            //5 shipper info
            public string shipperaccnum;
            public string shippername;
            public string shipperadd;
            public string shipperplace;
            public string shipperstate;
            public string shippercountrycode;
            public string shipperpostcode;
            public string shippercontactidentifier;
            public string shippercontactnum;

            //6 consignee info
            public string consaccnum;
            public string consname;
            public string consadd;
            public string consplace;
            public string consstate;
            public string conscountrycode;
            public string conspostcode;
            public string conscontactidentifier;
            public string conscontactnum;

            //11 charge declaration
            public string currency;
            public string chargecode;
            public string chargedec;
            public string declaredvalue;
            public string declaredcustomvalue;
            public string insuranceamount;

            public fhlinfo(string str)
            {

                fhlversionnum = str;

                //2
                airlineprefix = str;
                awbnum = str;
                origin = str;
                dest = str;
                consigntype = str;
                pcscnt = str;
                weightcode = str;
                weight = str;

                //5 shipper info
                shipperaccnum = str;
                shippername = str;
                shipperadd = str;
                shipperplace = str;
                shipperstate = str;
                shippercountrycode = str;
                shipperpostcode = str;
                shippercontactidentifier = str;
                shippercontactnum = str;

                //6 consignee info
                consaccnum = str;
                consname = str;
                consadd = str;
                consplace = str;
                consstate = str;
                conscountrycode = str;
                conspostcode = str;
                conscontactidentifier = str;
                conscontactnum = str;

                //11 charge declaration
                currency = str;
                chargecode = str;
                chargedec = str;
                declaredvalue = str;
                declaredcustomvalue = str;
                insuranceamount = str;

            }
        }

        public struct consignmnetinfo
        {
            public string airlineprefix;
            public string awbnum;
            public string origin;
            public string dest;
            public string consigntype;
            public string pcscnt;
            public string weightcode;
            public string weight;
            public string volumecode;
            public string volumeamt;
            public string densityindicator;
            public string densitygrp;
            public string shpdesccode;
            public string numshp;
            public string manifestdesc;
            public string splhandling;
            public string portsequence;
            public string uldsequence;
            public string customref;
            public string customorigincode;//8
            public string freetextGoodDesc;
            public string commodity;
            public string slac;

            public consignmnetinfo(string str)
            {
                airlineprefix = str;
                awbnum = str;
                origin = str;
                dest = str;
                consigntype = str;
                pcscnt = str;
                weightcode = str;
                weight = str;
                volumecode = str;
                volumeamt = str;
                densityindicator = str;
                densitygrp = str;
                shpdesccode = str;
                numshp = str;
                manifestdesc = str;
                splhandling = str;
                portsequence = str;
                uldsequence = str;
                customref = str;
                customorigincode = str;
                freetextGoodDesc = str;
                commodity = str;
                slac = str;
            }
        }

        public struct FltRoute
        {
             public string carriercode;
            public string fltnum;
            public string date;
            public string month;
            public string fltdept;
            public string fltarrival;
            public string spaceallotmentcode;
            public string allotidentification;
            
            public FltRoute(string val) 
            {
                //3 flight details
                carriercode = val;
                fltnum = val;
                date = val;
                month = val;
                fltdept = val;
                fltarrival = val;
                spaceallotmentcode = val;
                allotidentification = val;
             
            }
        }

        #region Class for Custom Messaging
        public class CustomMessage
        {
            public string AWBPrefix;
            public string AWBNumber;
            public string MessageType;
            public string HAWBNumber;
            public string ConsolidationIdentifier;
            public string PackageTrackingIdentifier;
            public string AWBPartArrivalReference;
            public string ArrivalAirport;
            public string AirCarrier;
            public string Origin;
            public string DestinionCode;
            public string WBLNumberOfPieces;
            public string WBLWeightIndicator;
            public string WBLWeight;
            public string WBLCargoDescription;
            public string ArrivalDate;
            public string PartArrivalReference;
            public string BoardedQuantityIdentifier;
            public string BoardedPieceCount;
            public string BoardedWeight;
            public string ImportingCarrier;
            public string FlightNumber;
            public string ARRPartArrivalReference;
            public string RequestType;
            public string RequestExplanation;
            public string EntryType;
            public string EntryNumber;
            public string AMSParticipantCode;
            public string ShipperName;
            public string ShipperAddress;
            public string ShipperCity;
            public string ShipperState;
            public string ShipperCountry;
            public string ShipperPostalCode;
            public string ConsigneeName;
            public string ConsigneeAddress;
            public string ConsigneeCity;
            public string ConsigneeState;
            public string ConsigneeCountry;
            public string ConsigneePostalCode;
            public string TransferDestAirport;
            public string DomesticIdentifier;
            public string BondedCarrierID;
            public string OnwardCarrier;
            public string BondedPremisesIdentifier;
            public string InBondControlNumber;
            public string OriginOfGoods;
            public string DeclaredValue;
            public string CurrencyCode;
            public string CommodityCode;
            public string LineIdentifier;
            public string AmendmentCode;
            public string AmendmentExplanation;
            public string DeptImportingCarrier;
            public string DeptFlightNumber;
            public string DeptScheduledArrivalDate;
            public string LiftoffDate;
            public string LiftoffTime;
            public string DeptActualImportingCarrier;
            public string DeptActualFlightNumber;
            public string ASNStatusCode;
            public string ASNActionExplanation;
            public string CSNActionCode;
            public string CSNPieces;
            public string TransactionDate;
            public string TransactionTime;
            public string CSNEntryType;
            public string CSNEntryNumber;
            public string CSNRemarks;
            public string ErrorCode;
            public string ErrorMessage;
            public string StatusRequestCode;
            public string StatusAnswerCode;
            public string Information;
            public string ERFImportingCarrier;
            public string ERFFlightNumber;
            public string ERFDate;
            public string Message;
            public string UpdatedOn;
            public string UpdatedBy;
            public string CreatedOn;
            public string CreatedBy;

         
        }
        #endregion

        #region Stock Check Message (SCM) class
        public class SCM
        {
            public string MessageIdentifier;
            public string AirportCode;
            public string Day;
            public string Month;
            public string Time;
            public ULDStockInfo[] ULDInfo;
            public string SupplemenataryInfoId;
            public string[] ULDInfoText;
        }
        public class ULDStockInfo
        {
            public string ULDTypeCode;
            public ULDStockInfoSub[] ULDInfoSub;
            public string TotalIdentifier;
            public string NumberOfULD;

        }
        public class ULDStockInfoSub
        {
            public string ULDSerialNo;
            public string ULDOwnerCode;
        }

        #endregion

}
