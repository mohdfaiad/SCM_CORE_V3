using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class HandOffFileCASSEncode
    {
        #region HandOff File Classes
        public class AWBHandOffFile
        {
            public
            string RecordID { get; set; }
            public
            string BranchOfficeIndicator { get; set; }
            public
            string VATIndicator { get; set; }
            public
            string AirlinePrefix { get; set; }
            public
            string AWBSerialNo { get; set; }
            public
            string AWBModCheck { get; set; }
            public
            string Filler1 { get; set; }
            public
            string Origin { get; set; }
            public
            string AgentCode { get; set; }
            public
            string AWBUseIndicator { get; set; }
            public
            string LateIndicator { get; set; }
            public
            string Filler2 { get; set; }
            public
            string Destination { get; set; }
            public
            string DateAWBExecution { get; set; }
            public
            string GrossWeight { get; set; }
            public
            string WeightIndicator { get; set; }
            public
            string CurrencyCode { get; set; }
            public
            string ChargeWeightPP { get; set; }
            public
            string ValuationChargePP { get; set; }
            public
            string ChargeDueCarPP { get; set; }
            public
            string ChargeDueAgentPP { get; set; }
            public
            string ChargeWeightCC { get; set; }
            public
            string ValuationChargeCC { get; set; }
            public
            string OCDueCarCC { get; set; }
            public
            string OCDueAgentCC { get; set; }
            public
            string CommissionPercentage { get; set; }
            public
            string Commission { get; set; }
            public
            string Filler3 { get; set; }
            public
            string TaxDueAirlineIndicator { get; set; }
            public
            string AgentRefData { get; set; }
            public
            string Filler4 { get; set; }
            public
            string AWBAcceptanceDate { get; set; }
            public
            string RateOfExchange { get; set; }
            public
            string Discount { get; set; }
            public
            string TaxDueAirline { get; set; }
            public
            string TaxDueAgent { get; set; }
            public
            string ReservedSpace { get; set; }
            public
            string DayOfDataCapture { get; set; }
            public
            string SequenceNo { get; set; }
            public
            string ReportingIndicator { get; set; }
            public
            string NORA { get; set; }
            public
            string DiscountIndicator { get; set; }


            public override string ToString()
            {
                return
                    RecordID +
                    BranchOfficeIndicator +
                    VATIndicator.PadRight(1, ' ') +
                    AirlinePrefix.PadLeft(3, '0') +
                    AWBSerialNo.PadLeft(8, '0') +
                    AWBModCheck +
                    Filler1 +
                    Origin +
                    AgentCode.PadLeft(11, '0') +
                    AWBUseIndicator.PadRight(1, ' ') +
                    LateIndicator +
                    Filler2 +
                    Destination +
                    DateAWBExecution +
                    GrossWeight +
                    WeightIndicator +
                    CurrencyCode.PadRight(3, ' ') +
                    ChargeWeightPP +
                    ValuationChargePP +
                    ChargeDueCarPP +
                    ChargeDueAgentPP +
                    ChargeWeightCC +
                    ValuationChargeCC +
                    OCDueCarCC +
                    OCDueAgentCC +
                    CommissionPercentage +
                    Commission +
                    Filler3 +
                    AgentRefData.PadRight(14, ' ') +
                    Filler4.PadRight(10, ' ') +
                    AWBAcceptanceDate +
                    RateOfExchange +
                    Discount +
                    TaxDueAirline.PadLeft(8, '0') +
                    TaxDueAgent.PadLeft(8, '0') +
                    ReservedSpace.PadRight(11, ' ') +
                    DiscountIndicator;
            }




        }
        public class DCRDCOHeaderHandOffFile
        {
            public
                string RecordID { get; set; }
            public
                string BranchOfficeIndicator { get; set; }
            public
                string VATIndicator { get; set; }
            public
                string AirlinePrefix { get; set; }
            public
                string AWBSerialNo { get; set; }
            public
                string AWBModCheck { get; set; }
            public
                string Filler { get; set; }
            public
                string Origin { get; set; }
            public
                string AgentCode { get; set; }
            public
                string DCMNo { get; set; }
            public
                string CurrencyCode { get; set; }
            public
                string RateOfExchange { get; set; }
            public
                string DateAWBExecution { get; set; }
            public
                string PPCCIndicator { get; set; }
            public
                string ChargeWeight { get; set; }
            public
                string ValuationCharge { get; set; }
            public
                string Taxes { get; set; }
            public
                string ChargeDueAgent { get; set; }
            public
                string ChargeDueCar { get; set; }
            public
                string VATOnAWBCharges { get; set; }
            public
                string Commission { get; set; }
            public
                string VATOnCommission { get; set; }
            public
                string Discount { get; set; }
            public
                string DiscountIndicator { get; set; }
            public
                string WeightIndicator { get; set; }
            public
                string GrossWeight { get; set; }
            public
                string Destination { get; set; }
            public
                string ReservedSpace { get; set; }
            public
                string ReasonAdjustment { get; set; }



            public override string ToString()
            {
                return RecordID +
                    BranchOfficeIndicator +
                    VATIndicator.PadRight(1, ' ') +
                    AirlinePrefix +
                    AWBSerialNo +
                    AWBModCheck.PadRight(1, ' ') +

                       Filler.PadRight(1, ' ') +
                       Origin + AgentCode.PadLeft(11, '0') +
                       DCMNo.PadRight(6, ' ') +
                       CurrencyCode.PadRight(3, ' ') +
                       RateOfExchange +
                       DateAWBExecution +
                       PPCCIndicator +
                       ChargeWeight +
                       PPCCIndicator +
                       ValuationCharge +
                       PPCCIndicator +
                       Taxes +
                       PPCCIndicator +
                       ChargeDueAgent +
                       PPCCIndicator +
                       ChargeDueCar +
                       VATOnAWBCharges.PadLeft(12, '0') +
                       Commission +
                       VATOnCommission.PadLeft(12, '0') +
                       Discount +
                       DiscountIndicator.PadRight(1, ' ') +
                       WeightIndicator +
                       GrossWeight +
                       Destination +
                       ReservedSpace.PadRight(17, ' ') +
                       ReasonAdjustment.PadRight(50, ' ');
            }

        }
        public class HeaderHandOffFile
        {
            public string RecordID { get; set; }
            public string CASSAreaCode { get; set; }
            public string AirlinePrefix { get; set; }
            public string DatePeriodStart { get; set; }
            public string DatePeriodEnd { get; set; }
            public string DateOfBilling { get; set; }
            public string FileNo { get; set; }
            public string CurrencyCode { get; set; }
            public string BranchOfficeIndicator { get; set; }
            public string Filler { get; set; }
            public string ReservedSpace { get; set; }

            public override string ToString()
            {
                return RecordID + CASSAreaCode + AirlinePrefix + DatePeriodStart + DatePeriodEnd +
                       DateOfBilling + FileNo + CurrencyCode + BranchOfficeIndicator + Filler.PadRight(198, ' ') +
                       ReservedSpace.PadRight(20, ' ');
            }
        }
        public class TrailerHandOffFile
        {
            public string RecordID { get; set; }
            public string Filler { get; set; }
            public string CASSAreaCode1 { get; set; }
            public string CASSAreaCode2 { get; set; }
            public string AirlinePrefix { get; set; }
            public string BranchOfficeIndicator { get; set; }
            public string NoOfAWBTransactions { get; set; }
            public string NoOfCCATransactions { get; set; }
            public string NoOfDCMs { get; set; }
            public string TotalRecords { get; set; }
            public string ReservedSpace1 { get; set; }
            public string RecordTypeAWM { get; set; }
            public string RecordTypeAW1 { get; set; }
            public string RecordTypeAW2 { get; set; }
            public string RecordTypeAW3 { get; set; }
            public string RecordTypeAW4 { get; set; }
            public string RecordTypeAW5 { get; set; }
            public string RecordTypeCCR { get; set; }
            public string RecordTypeCCO { get; set; }
            public string RecordTypeDCR { get; set; }
            public string RecordTypeDCO { get; set; }
            public string ReservedSpace2 { get; set; }
            public string ReservedSpace3 { get; set; }


            public override string ToString()
            {
                return RecordID + Filler.PadRight(1, ' ') +
                    CASSAreaCode1 +
                    CASSAreaCode2.PadLeft(3, '0')
                    + AirlinePrefix +
                       BranchOfficeIndicator +
                       NoOfAWBTransactions.PadLeft(7, '0') +
                       NoOfCCATransactions.PadLeft(7, '0') +
                       NoOfDCMs.PadLeft(7, '0') +
                       TotalRecords.PadLeft(7, '0') +
                       RecordTypeAWM.PadLeft(7, '0') +
                       RecordTypeAW1.PadLeft(7, '0') +
                       RecordTypeAW2.PadLeft(7, '0') +
                       RecordTypeAW3.PadLeft(7, '0') +
                       RecordTypeAW4.PadLeft(7, '0') +
                       RecordTypeAW5.PadLeft(7, '0') +
                       RecordTypeCCR.PadLeft(7, '0') +
                       RecordTypeCCO.PadLeft(7, '0') +
                       RecordTypeDCR.PadLeft(7, '0') +
                       RecordTypeDCO.PadLeft(7, '0') +
                       ReservedSpace2.PadRight(119, ' ') +
                       ReservedSpace3.PadRight(20, ' ');


            }
        }
        #endregion

        public StringBuilder GettingHandOffFileCASSRecords(DataSet ds)
        {
            try
            {

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {

                        #region AWB HandOff File

                        try
                        {
                            StringBuilder sbuild = new StringBuilder();

                            //Writing Header Record for HandOff File
                            HeaderHandOffFile Header = new HeaderHandOffFile();
                            Header.RecordID = "AAA";
                            Header.CASSAreaCode = "IN";
                            Header.AirlinePrefix = ds.Tables[0].Rows[0]["AirlinePrefix"].ToString();
                            Header.DatePeriodStart = ds.Tables[0].Rows[0]["PeriodStart"].ToString();
                            Header.DatePeriodEnd = ds.Tables[0].Rows[0]["PeriodEnd"].ToString();
                            Header.DateOfBilling = ds.Tables[0].Rows[0]["BillingDate"].ToString();
                            Header.FileNo = "01";
                            Header.CurrencyCode = ds.Tables[0].Rows[0]["Currency"].ToString();
                            Header.BranchOfficeIndicator = "A";
                            Header.Filler = "";
                            Header.ReservedSpace = "";

                            //sw1.WriteLine(Header.ToString());
                            sbuild.AppendLine(Header.ToString());


                            //Writing AWBRecords for HandOff File
                            AWBHandOffFile[] HandOffFile = new AWBHandOffFile[ds.Tables[1].Rows.Count];
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                HandOffFile[i] = new AWBHandOffFile();
                                HandOffFile[i].RecordID = "AWM";
                                HandOffFile[i].BranchOfficeIndicator = ds.Tables[1].Rows[i]["BranchOfficeIndicator"].ToString();
                                HandOffFile[i].VATIndicator = ds.Tables[1].Rows[i]["VATIndicator"].ToString();
                                HandOffFile[i].AirlinePrefix = ds.Tables[1].Rows[i]["AirlinePrefix"].ToString();
                                HandOffFile[i].AWBSerialNo = ds.Tables[1].Rows[i]["AWBNumber"].ToString();
                                HandOffFile[i].AWBModCheck = ds.Tables[1].Rows[i]["AWBModCheck"].ToString();
                                HandOffFile[i].Filler1 = " ";
                                HandOffFile[i].Filler2 = "  ";
                                HandOffFile[i].Filler3 = " ";
                                HandOffFile[i].Filler4 = "";
                                HandOffFile[i].Origin = ds.Tables[1].Rows[i]["Origin"].ToString();
                                HandOffFile[i].AgentCode = ds.Tables[1].Rows[i]["AgentCode"].ToString();
                                HandOffFile[i].AWBUseIndicator = ds.Tables[1].Rows[i]["AWBIndicator"].ToString();
                                HandOffFile[i].LateIndicator = ds.Tables[1].Rows[i]["LateIndicator"].ToString();
                                HandOffFile[i].Destination = ds.Tables[1].Rows[i]["Destination"].ToString();
                                HandOffFile[i].DateAWBExecution = ds.Tables[1].Rows[i]["ExecutionDate"].ToString();
                                HandOffFile[i].GrossWeight = ds.Tables[1].Rows[i]["GrossWeight"].ToString();
                                HandOffFile[i].WeightIndicator = ds.Tables[1].Rows[i]["WeightIndicator"].ToString();
                                HandOffFile[i].CurrencyCode = ds.Tables[1].Rows[i]["Currency"].ToString();
                                HandOffFile[i].ChargeWeightPP = ds.Tables[1].Rows[i]["ChargeWeightPP"].ToString();
                                HandOffFile[i].ValuationChargePP = ds.Tables[1].Rows[i]["ValChargePP"].ToString();
                                HandOffFile[i].ChargeDueCarPP = ds.Tables[1].Rows[i]["ChargeDueCarPP"].ToString();
                                HandOffFile[i].ChargeDueAgentPP = ds.Tables[1].Rows[i]["ChargeDueAgentPP"].ToString();
                                HandOffFile[i].ChargeWeightCC = ds.Tables[1].Rows[i]["ChargedWeightCC"].ToString();
                                HandOffFile[i].ValuationChargeCC = ds.Tables[1].Rows[i]["ValChargeCC"].ToString();
                                HandOffFile[i].OCDueCarCC = ds.Tables[1].Rows[i]["OCDueCarCC"].ToString();
                                HandOffFile[i].OCDueAgentCC = ds.Tables[1].Rows[i]["OCDueAgentCC"].ToString();
                                HandOffFile[i].CommissionPercentage = ds.Tables[1].Rows[i]["CommissionPercentage"].ToString();
                                HandOffFile[i].Commission = ds.Tables[1].Rows[i]["Commission"].ToString();
                                HandOffFile[i].TaxDueAirlineIndicator = ds.Tables[1].Rows[i]["TaxDueAirlineIndicator"].ToString();
                                HandOffFile[i].AgentRefData = ds.Tables[1].Rows[i]["AgentReferenceCode"].ToString();
                                HandOffFile[i].AWBAcceptanceDate = ds.Tables[1].Rows[i]["AcceptanceDate"].ToString();
                                HandOffFile[i].RateOfExchange = ds.Tables[1].Rows[i]["RateOfExchange"].ToString();
                                HandOffFile[i].Discount = ds.Tables[1].Rows[i]["Discount"].ToString();
                                HandOffFile[i].TaxDueAirline = ds.Tables[1].Rows[i]["TaxDueAirline"].ToString();
                                HandOffFile[i].TaxDueAgent = ds.Tables[1].Rows[i]["TaxDueAgent"].ToString();
                                HandOffFile[i].ReservedSpace = "";
                                HandOffFile[i].DayOfDataCapture = ds.Tables[1].Rows[i]["DayDataCapture"].ToString();
                                HandOffFile[i].SequenceNo = i.ToString();
                                HandOffFile[i].ReportingIndicator = ds.Tables[1].Rows[i]["ReportingIndicator"].ToString();
                                HandOffFile[i].NORA = ds.Tables[1].Rows[i]["Nora"].ToString();
                                HandOffFile[i].DiscountIndicator = ds.Tables[1].Rows[i]["DiscountIndicator"].ToString();

                            }
                            foreach (AWBHandOffFile file in HandOffFile)
                            {
                                //sw1.WriteLine(file.ToString());
                                sbuild.AppendLine(file.ToString());
                            }

                            //Writing DCR Records for HandOff File
                            DCRDCOHeaderHandOffFile[] DCRRecords = new DCRDCOHeaderHandOffFile[ds.Tables[2].Rows.Count];
                            for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                            {
                                DCRRecords[i] = new DCRDCOHeaderHandOffFile();
                                DCRRecords[i].RecordID = ds.Tables[2].Rows[i]["RecordID"].ToString();
                                DCRRecords[i].BranchOfficeIndicator = ds.Tables[2].Rows[i]["BranchOfficeIndicator"].ToString();
                                DCRRecords[i].VATIndicator = ds.Tables[2].Rows[i]["VATIndicator"].ToString();
                                DCRRecords[i].AirlinePrefix = ds.Tables[2].Rows[i]["AirlinePrefix"].ToString();
                                DCRRecords[i].AWBSerialNo = ds.Tables[2].Rows[i]["AWBNumber"].ToString();
                                DCRRecords[i].AWBModCheck = ds.Tables[2].Rows[i]["AWBModCheck"].ToString();
                                DCRRecords[i].Filler = " ";
                                DCRRecords[i].Origin = ds.Tables[2].Rows[i]["Origin"].ToString();
                                DCRRecords[i].AgentCode = ds.Tables[2].Rows[i]["AgentCode"].ToString();
                                if (ds.Tables[2].Rows[i]["DCMNumber"].ToString().Length > 6)
                                {
                                    DCRRecords[i].DCMNo = ds.Tables[2].Rows[i]["DCMNumber"].ToString().Substring(0, 6);
                                }
                                else
                                    DCRRecords[i].DCMNo = ds.Tables[2].Rows[i]["DCMNumber"].ToString();
                                DCRRecords[i].CurrencyCode = ds.Tables[2].Rows[i]["Currency"].ToString();
                                DCRRecords[i].RateOfExchange = ds.Tables[2].Rows[i]["RateOfExchange"].ToString();
                                DCRRecords[i].DateAWBExecution = ds.Tables[2].Rows[i]["ExecutionDate"].ToString();
                                DCRRecords[i].PPCCIndicator = ds.Tables[2].Rows[i]["PayMode"].ToString();
                                DCRRecords[i].ChargeWeight = ds.Tables[2].Rows[i]["ChargedWeight"].ToString();
                                DCRRecords[i].ValuationCharge = ds.Tables[2].Rows[i]["ValCharge"].ToString();
                                DCRRecords[i].Taxes = ds.Tables[2].Rows[i]["Tax"].ToString();
                                DCRRecords[i].ChargeDueAgent = ds.Tables[2].Rows[i]["OCDueAgent"].ToString();
                                DCRRecords[i].ChargeDueCar = ds.Tables[2].Rows[i]["OCDueCar"].ToString();
                                DCRRecords[i].VATOnAWBCharges = ds.Tables[2].Rows[i]["VATAWBCharges"].ToString();
                                DCRRecords[i].Commission = ds.Tables[2].Rows[i]["Commission"].ToString();
                                DCRRecords[i].VATOnCommission = ds.Tables[2].Rows[i]["VATAWBCommission"].ToString();
                                DCRRecords[i].Discount = ds.Tables[2].Rows[i]["Discount"].ToString();
                                DCRRecords[i].DiscountIndicator = ds.Tables[2].Rows[i]["DiscountIndicator"].ToString();
                                DCRRecords[i].WeightIndicator = ds.Tables[2].Rows[i]["WeightIndicator"].ToString();
                                DCRRecords[i].GrossWeight = ds.Tables[2].Rows[i]["GrossWeight"].ToString();
                                DCRRecords[i].Destination = ds.Tables[2].Rows[i]["Destination"].ToString();
                                DCRRecords[i].ReservedSpace = "";
                                DCRRecords[i].ReasonAdjustment = "";


                            }
                            foreach (DCRDCOHeaderHandOffFile file in DCRRecords)
                            {
                                //sw1.WriteLine(file.ToString());
                                sbuild.AppendLine(file.ToString());
                            }


                            //Writing CCR Records for HandOff File
                            DCRDCOHeaderHandOffFile[] CCRRecords = new DCRDCOHeaderHandOffFile[ds.Tables[3].Rows.Count];
                            for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                            {
                                CCRRecords[i] = new DCRDCOHeaderHandOffFile();
                                CCRRecords[i].RecordID = ds.Tables[3].Rows[i]["RecordID"].ToString();
                                CCRRecords[i].BranchOfficeIndicator = ds.Tables[3].Rows[i]["BranchOfficeIndicator"].ToString();
                                CCRRecords[i].VATIndicator = ds.Tables[3].Rows[i]["VATIndicator"].ToString();
                                CCRRecords[i].AirlinePrefix = ds.Tables[3].Rows[i]["AirlinePrefix"].ToString();
                                CCRRecords[i].AWBSerialNo = ds.Tables[3].Rows[i]["AWBNumber"].ToString();
                                CCRRecords[i].AWBModCheck = ds.Tables[3].Rows[i]["AWBModCheck"].ToString();
                                CCRRecords[i].Filler = " ";
                                CCRRecords[i].Origin = ds.Tables[3].Rows[i]["Origin"].ToString();
                                CCRRecords[i].AgentCode = ds.Tables[3].Rows[i]["AgentCode"].ToString();
                                if (ds.Tables[3].Rows[i]["CCANumber"].ToString().Length > 6)
                                {
                                    CCRRecords[i].DCMNo = ds.Tables[3].Rows[i]["CCANumber"].ToString().Substring(0, 6);
                                }
                                else
                                    CCRRecords[i].DCMNo = ds.Tables[3].Rows[i]["CCANumber"].ToString();
                                CCRRecords[i].CurrencyCode = ds.Tables[3].Rows[i]["Currency"].ToString();
                                CCRRecords[i].RateOfExchange = ds.Tables[3].Rows[i]["RateOfExchange"].ToString();
                                CCRRecords[i].DateAWBExecution = ds.Tables[3].Rows[i]["ExecutionDate"].ToString();
                                CCRRecords[i].PPCCIndicator = ds.Tables[3].Rows[i]["PayMode"].ToString();
                                CCRRecords[i].ChargeWeight = ds.Tables[3].Rows[i]["ChargedWeight"].ToString();
                                CCRRecords[i].ValuationCharge = ds.Tables[3].Rows[i]["ValCharge"].ToString();
                                CCRRecords[i].Taxes = ds.Tables[3].Rows[i]["Tax"].ToString();
                                CCRRecords[i].ChargeDueAgent = ds.Tables[3].Rows[i]["OCDueAgent"].ToString();
                                CCRRecords[i].ChargeDueCar = ds.Tables[3].Rows[i]["OCDueCar"].ToString();
                                CCRRecords[i].VATOnAWBCharges = ds.Tables[3].Rows[i]["VATAWBCharges"].ToString();
                                CCRRecords[i].Commission = ds.Tables[3].Rows[i]["Commission"].ToString();
                                CCRRecords[i].VATOnCommission = ds.Tables[3].Rows[i]["VATAWBCommission"].ToString();
                                CCRRecords[i].Discount = ds.Tables[3].Rows[i]["Discount"].ToString();
                                CCRRecords[i].DiscountIndicator = ds.Tables[3].Rows[i]["DiscountIndicator"].ToString();
                                CCRRecords[i].WeightIndicator = ds.Tables[3].Rows[i]["WeightIndicator"].ToString();
                                CCRRecords[i].GrossWeight = ds.Tables[3].Rows[i]["GrossWeight"].ToString();
                                CCRRecords[i].Destination = ds.Tables[3].Rows[i]["Destination"].ToString();
                                CCRRecords[i].ReservedSpace = "";
                                CCRRecords[i].ReasonAdjustment = "";


                            }
                            foreach (DCRDCOHeaderHandOffFile file in CCRRecords)
                            {
                                //sw1.WriteLine(file.ToString());
                                sbuild.AppendLine(file.ToString());
                            }

                            //Writing DCO Records to HandOff File

                            DCRDCOHeaderHandOffFile[] DCORecords = new DCRDCOHeaderHandOffFile[ds.Tables[4].Rows.Count];
                            for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                            {
                                DCORecords[i] = new DCRDCOHeaderHandOffFile();
                                DCORecords[i].RecordID = ds.Tables[4].Rows[i]["RecordID"].ToString();
                                DCORecords[i].BranchOfficeIndicator = "A";
                                DCORecords[i].VATIndicator = " ";
                                DCORecords[i].AirlinePrefix = ds.Tables[4].Rows[i]["AirlinePrefix"].ToString();
                                DCORecords[i].AWBSerialNo = ds.Tables[4].Rows[i]["AWBNumber"].ToString();
                                DCORecords[i].AWBModCheck = ds.Tables[4].Rows[i]["AWBModCheck"].ToString();
                                DCORecords[i].Filler = " ";
                                DCORecords[i].Origin = ds.Tables[4].Rows[i]["Origin"].ToString();
                                DCORecords[i].AgentCode = ds.Tables[4].Rows[i]["AgentCode"].ToString();
                                if (ds.Tables[4].Rows[i]["DCMNumber"].ToString().Length > 6)
                                {
                                    DCORecords[i].DCMNo = ds.Tables[4].Rows[i]["DCMNumber"].ToString().Substring(0, 6);
                                }
                                else
                                    DCORecords[i].DCMNo = ds.Tables[4].Rows[i]["DCMNumber"].ToString();

                                DCORecords[i].CurrencyCode = ds.Tables[4].Rows[i]["Currency"].ToString();
                                DCORecords[i].RateOfExchange = ds.Tables[4].Rows[i]["RateOfExchange"].ToString();
                                DCORecords[i].DateAWBExecution = ds.Tables[4].Rows[i]["ExecutionDate"].ToString();
                                DCORecords[i].PPCCIndicator = ds.Tables[4].Rows[i]["PayMode"].ToString();
                                DCORecords[i].ChargeWeight = ds.Tables[4].Rows[i]["ChargedWeight"].ToString();
                                DCORecords[i].ValuationCharge = ds.Tables[4].Rows[i]["ValCharge"].ToString();
                                DCORecords[i].Taxes = ds.Tables[4].Rows[i]["Tax"].ToString();
                                DCORecords[i].ChargeDueAgent = ds.Tables[4].Rows[i]["OCDueAgent"].ToString();
                                DCORecords[i].ChargeDueCar = ds.Tables[4].Rows[i]["OCDueCar"].ToString();
                                DCORecords[i].VATOnAWBCharges = ds.Tables[4].Rows[i]["VATAWBCharges"].ToString();
                                DCORecords[i].Commission = ds.Tables[4].Rows[i]["Commission"].ToString();
                                DCORecords[i].VATOnCommission = ds.Tables[4].Rows[i]["VATAWBCommission"].ToString();
                                DCORecords[i].Discount = ds.Tables[4].Rows[i]["Discount"].ToString();
                                DCORecords[i].DiscountIndicator = ds.Tables[4].Rows[i]["DiscountIndicator"].ToString();
                                DCORecords[i].WeightIndicator = ds.Tables[4].Rows[i]["WeightIndicator"].ToString();
                                DCORecords[i].GrossWeight = ds.Tables[4].Rows[i]["GrossWeight"].ToString();
                                DCORecords[i].Destination = ds.Tables[4].Rows[i]["Destination"].ToString();
                                DCORecords[i].ReservedSpace = "";
                                DCORecords[i].ReasonAdjustment = "";


                            }
                            foreach (DCRDCOHeaderHandOffFile file in DCORecords)
                            {
                                //sw1.WriteLine(file.ToString());
                                sbuild.AppendLine(file.ToString());
                            }


                            //Writing CCO Records to HandOff File

                            DCRDCOHeaderHandOffFile[] CCORecords = new DCRDCOHeaderHandOffFile[ds.Tables[5].Rows.Count];
                            for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                            {
                                CCORecords[i] = new DCRDCOHeaderHandOffFile();
                                CCORecords[i].RecordID = ds.Tables[5].Rows[i]["RecordID"].ToString();
                                CCORecords[i].BranchOfficeIndicator = "A";
                                CCORecords[i].VATIndicator = " ";
                                CCORecords[i].AirlinePrefix = ds.Tables[5].Rows[i]["AirlinePrefix"].ToString();
                                CCORecords[i].AWBSerialNo = ds.Tables[5].Rows[i]["AWBNumber"].ToString();
                                CCORecords[i].AWBModCheck = ds.Tables[5].Rows[i]["AWBModCheck"].ToString();
                                CCORecords[i].Filler = " ";
                                CCORecords[i].Origin = ds.Tables[5].Rows[i]["Origin"].ToString();
                                CCORecords[i].AgentCode = ds.Tables[5].Rows[i]["AgentCode"].ToString();
                                if (ds.Tables[5].Rows[i]["CCANumber"].ToString().Length > 6)
                                {
                                    CCORecords[i].DCMNo = ds.Tables[5].Rows[i]["CCANumber"].ToString().Substring(0, 6);
                                }
                                else
                                    CCORecords[i].DCMNo = ds.Tables[5].Rows[i]["CCANumber"].ToString();

                                CCORecords[i].CurrencyCode = ds.Tables[5].Rows[i]["Currency"].ToString();
                                CCORecords[i].RateOfExchange = ds.Tables[5].Rows[i]["RateOfExchange"].ToString();
                                CCORecords[i].DateAWBExecution = ds.Tables[5].Rows[i]["ExecutionDate"].ToString();
                                CCORecords[i].PPCCIndicator = ds.Tables[5].Rows[i]["PayMode"].ToString();
                                CCORecords[i].ChargeWeight = ds.Tables[5].Rows[i]["ChargedWeight"].ToString();
                                CCORecords[i].ValuationCharge = ds.Tables[5].Rows[i]["ValCharge"].ToString();
                                CCORecords[i].Taxes = ds.Tables[5].Rows[i]["Tax"].ToString();
                                CCORecords[i].ChargeDueAgent = ds.Tables[5].Rows[i]["OCDueAgent"].ToString();
                                CCORecords[i].ChargeDueCar = ds.Tables[5].Rows[i]["OCDueCar"].ToString();
                                CCORecords[i].VATOnAWBCharges = ds.Tables[5].Rows[i]["VATAWBCharges"].ToString();
                                CCORecords[i].Commission = ds.Tables[5].Rows[i]["Commission"].ToString();
                                CCORecords[i].VATOnCommission = ds.Tables[5].Rows[i]["VATAWBCommission"].ToString();
                                CCORecords[i].Discount = ds.Tables[5].Rows[i]["Discount"].ToString();
                                CCORecords[i].DiscountIndicator = ds.Tables[5].Rows[i]["DiscountIndicator"].ToString();
                                CCORecords[i].WeightIndicator = ds.Tables[5].Rows[i]["WeightIndicator"].ToString();
                                CCORecords[i].GrossWeight = ds.Tables[5].Rows[i]["GrossWeight"].ToString();
                                CCORecords[i].Destination = ds.Tables[5].Rows[i]["Destination"].ToString();
                                CCORecords[i].ReservedSpace = "";
                                CCORecords[i].ReasonAdjustment = "";


                            }
                            foreach (DCRDCOHeaderHandOffFile file in CCORecords)
                            {
                                //sw1.WriteLine(file.ToString());
                                sbuild.AppendLine(file.ToString());
                            }


                            //Writing Trailer Record for HandOff File

                            TrailerHandOffFile Trailer = new TrailerHandOffFile();
                            Trailer.RecordID = "TTT";
                            Trailer.Filler = "";
                            Trailer.CASSAreaCode1 = "IN";
                            Trailer.CASSAreaCode2 = "356";
                            Trailer.AirlinePrefix = ds.Tables[0].Rows[0]["AirlinePrefix"].ToString();
                            Trailer.BranchOfficeIndicator = "A";
                            Trailer.NoOfAWBTransactions = (ds.Tables[1].Rows.Count).ToString();
                            Trailer.NoOfCCATransactions = (ds.Tables[3].Rows.Count + ds.Tables[5].Rows.Count).ToString();
                            Trailer.NoOfDCMs = (ds.Tables[2].Rows.Count + ds.Tables[4].Rows.Count).ToString();
                            Trailer.TotalRecords = (ds.Tables[1].Rows.Count + ds.Tables[2].Rows.Count + ds.Tables[3].Rows.Count + ds.Tables[4].Rows.Count + ds.Tables[5].Rows.Count).ToString();
                            Trailer.ReservedSpace1 = "";
                            Trailer.RecordTypeAWM = (ds.Tables[1].Rows.Count).ToString();
                            Trailer.RecordTypeAW1 = "";
                            Trailer.RecordTypeAW2 = "";
                            Trailer.RecordTypeAW3 = "";
                            Trailer.RecordTypeAW4 = "";
                            Trailer.RecordTypeAW5 = "";
                            Trailer.RecordTypeCCO = (ds.Tables[5].Rows.Count).ToString();
                            Trailer.RecordTypeCCR = (ds.Tables[3].Rows.Count).ToString();
                            Trailer.RecordTypeDCR = (ds.Tables[2].Rows.Count).ToString();
                            Trailer.RecordTypeDCO = (ds.Tables[4].Rows.Count).ToString();
                            Trailer.ReservedSpace2 = "";
                            Trailer.ReservedSpace3 = "";

                            //sw1.WriteLine(Trailer.ToString());
                            sbuild.AppendLine(Trailer.ToString());

                            //sw1.Flush();
                            //sw1.Close();
                            return sbuild;


                        }
                        catch (Exception ex)
                        { }
                        #endregion

                    }
                    return null;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;

            }

        }
    }
}
