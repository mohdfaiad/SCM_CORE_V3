using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BAL
{
    public class BillingCASSEncode
    {
        #region Billing Records
        public class AWBRecord
        {
            public
            string RecordID { get; set; }
            public string AWBInvoiceIndicator { get; set; }
            public string VATIndicator { get; set; }
            public string AgentCode { get; set; }
            public string AirlinePrefix { get; set; }
            public string AWBSerialNo { get; set; }
            public string AWBModCheck { get; set; }
            public string Filler { get; set; }
            public string Origin { get; set; }
            public string AWBUseIndicator { get; set; }
            public string BranchOfficeIndicator { get; set; }
            public string Destination { get; set; }
            public string DateAWBExecution { get; set; }
            public string GrossWeight { get; set; }
            public string WeightIndicator { get; set; }
            public string CurrencyCode { get; set; }
            public string ChargeWeightPP { get; set; }
            public string ValuationChargePP { get; set; }
            public string ChargeDueCarPP { get; set; }
            public string ChargeDueAgentPP { get; set; }
            public string ChargeWeightCC { get; set; }
            public string ValuationChargeCC { get; set; }
            public string OCDueCarCC { get; set; }
            public string OCDueAgentCC { get; set; }
            public string CommissionPercentage { get; set; }
            public string Commission { get; set; }
            public string CommissionIndicator { get; set; }
            public string Discount { get; set; }
            public string AWBAcceptanceDate { get; set; }
            public string AgentRefData { get; set; }
            public string RateOfExchange { get; set; }
            public string TaxDueAirline { get; set; }
            public string TaxDueAgent { get; set; }
            public string ReservedSpace { get; set; }
            public string TaxDueAirlineIndicator { get; set; }
            public string DiscountIndicator { get; set; }


            public override string ToString()
            {
                return
                    RecordID.PadRight(3, ' ') +
                    AWBInvoiceIndicator +
                    VATIndicator.PadRight(1, ' ') +
                    AgentCode.PadLeft(11, '0') +
                    AirlinePrefix.PadLeft(3, '0') +
                    AWBSerialNo.PadLeft(8, '0') +
                    AWBModCheck +
                    Filler.PadRight(1, ' ') +
                    Origin +
                    AWBUseIndicator.PadRight(1, ' ') +
                    BranchOfficeIndicator +
                    Filler.PadRight(1, ' ') +
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
                    CommissionIndicator.PadRight(1, ' ') +
                    Discount +
                    AWBAcceptanceDate +
                    AgentRefData.PadRight(14, ' ') +
                    RateOfExchange.PadLeft(11, '0') +
                    TaxDueAirline.PadLeft(12, '0') +
                    TaxDueAgent.PadLeft(12, '0') +
                    ReservedSpace.PadRight(14, ' ') +
                    DiscountIndicator;
            }




        }
        public class DCRDCORecord
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
                       Origin +
                       AgentCode.PadLeft(11, '0') +
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
        public class HeaderRecord
        {
            public string RecordID { get; set; }
            public string CASSAreaCode { get; set; }
            public string BranchOfficeIndicator { get; set; }
            public string Filler { get; set; }
            public string AirlinePrefix { get; set; }
            public string DatePeriodStart { get; set; }
            public string DatePeriodEnd { get; set; }
            public string DateOfBilling { get; set; }
            public string FileNo { get; set; }
            public string ReservedSpace1 { get; set; }
            public string ReservedSpace2 { get; set; }

            public override string ToString()
            {
                return RecordID +
                        CASSAreaCode +
                        BranchOfficeIndicator +
                        Filler.PadRight(10, ' ') +
                        AirlinePrefix.PadLeft(3, '0') +
                        DatePeriodStart +
                        DatePeriodEnd +
                        DateOfBilling +
                        FileNo.PadLeft(2, '0') +
                        ReservedSpace1.PadRight(191, ' ') +
                        ReservedSpace2.PadRight(20, ' ');
            }
        }
        public class TrailerRecord
        {
            public string RecordID { get; set; }
            public string CASSAreaCode { get; set; }
            public string AirlinePrefix { get; set; }
            public string BranchOfficeIndicator { get; set; }
            public string Filler { get; set; }
            public string NoOfAWB { get; set; }
            public string NoOfCCA { get; set; }
            public string NoOfDCM { get; set; }
            public string TotalRecords { get; set; }
            public string HashTotal { get; set; }
            public string NoOfAW1 { get; set; }
            public string NoOfCC3 { get; set; }
            public string NoOfDC3 { get; set; }
            public string ReservedSpace1 { get; set; }
            public string ReservedSpace2 { get; set; }


            public override string ToString()
            {
                return RecordID +
                       CASSAreaCode +
                       AirlinePrefix.PadLeft(3, '0') +
                       BranchOfficeIndicator +
                       Filler.PadRight(7, ' ') +
                       NoOfAWB.PadLeft(7, '0') +
                       NoOfCCA.PadLeft(7, '0') +
                       NoOfDCM.PadLeft(7, '0') +
                       TotalRecords.PadLeft(7, '0') +
                       HashTotal.PadLeft(12, '0') +
                       NoOfAW1.PadLeft(7, '0') +
                       NoOfCC3.PadLeft(7, '0') +
                       NoOfDC3.PadLeft(7, '0') +
                       ReservedSpace1.PadRight(153, ' ') +
                       ReservedSpace2.PadRight(20, ' ');

            }
        }
        #endregion

        public StringBuilder GettingBillingCASSRecords(DataSet ds)
        {
            try
            {

                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {


                        #region Header Record

                        HeaderRecord HeaderRecords = new HeaderRecord();
                        HeaderRecords.RecordID = "ALS";
                        HeaderRecords.CASSAreaCode = "IN";
                        HeaderRecords.BranchOfficeIndicator = "A";
                        HeaderRecords.Filler = "";
                        HeaderRecords.AirlinePrefix = ds.Tables[0].Rows[0]["AirlinePrefix"].ToString();
                        HeaderRecords.DatePeriodStart = ds.Tables[0].Rows[0]["PeriodStart"].ToString();
                        HeaderRecords.DatePeriodEnd = ds.Tables[0].Rows[0]["PeriodEnd"].ToString();
                        HeaderRecords.DateOfBilling = ds.Tables[0].Rows[0]["BillingDate"].ToString();
                        HeaderRecords.FileNo = "";
                        HeaderRecords.ReservedSpace1 = "";
                        HeaderRecords.ReservedSpace2 = "";

                        #endregion

                        #region Retrieving AWB Records from DB
                        AWBRecord[] AWBRecords = new AWBRecord[ds.Tables[1].Rows.Count];
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            AWBRecords[i] = new AWBRecord();
                            AWBRecords[i].RecordID = ds.Tables[1].Rows[i]["RecordID"].ToString();
                            AWBRecords[i].AWBInvoiceIndicator = ds.Tables[1].Rows[i]["InvoiceIndicator"].ToString();
                            AWBRecords[i].VATIndicator = ds.Tables[1].Rows[i]["VATIndicator"].ToString();
                            AWBRecords[i].AgentCode = ds.Tables[1].Rows[i]["AgentCode"].ToString();
                            AWBRecords[i].AirlinePrefix = ds.Tables[1].Rows[i]["AirlinePrefix"].ToString();
                            AWBRecords[i].AWBSerialNo = ds.Tables[1].Rows[i]["AWBNumber"].ToString();
                            AWBRecords[i].AWBModCheck = ds.Tables[1].Rows[i]["AWBModCheck"].ToString();
                            AWBRecords[i].Filler = " ";
                            AWBRecords[i].Origin = ds.Tables[1].Rows[i]["Origin"].ToString();
                            AWBRecords[i].AWBUseIndicator = ds.Tables[1].Rows[i]["AWBIndicator"].ToString();
                            AWBRecords[i].BranchOfficeIndicator = ds.Tables[1].Rows[i]["BranchOfficeIndicator"].ToString();
                            AWBRecords[i].Destination = ds.Tables[1].Rows[i]["Destination"].ToString();
                            AWBRecords[i].DateAWBExecution = ds.Tables[1].Rows[i]["ExecutionDate"].ToString();
                            AWBRecords[i].GrossWeight = ds.Tables[1].Rows[i]["GrossWeight"].ToString();
                            AWBRecords[i].WeightIndicator = ds.Tables[1].Rows[i]["WeightIndicator"].ToString();
                            AWBRecords[i].CurrencyCode = ds.Tables[1].Rows[i]["Currency"].ToString();
                            AWBRecords[i].ChargeWeightPP = ds.Tables[1].Rows[i]["ChargeWeightPP"].ToString();
                            AWBRecords[i].ValuationChargePP = ds.Tables[1].Rows[i]["ValChargePP"].ToString();
                            AWBRecords[i].ChargeDueCarPP = ds.Tables[1].Rows[i]["ChargeDueCarPP"].ToString();
                            AWBRecords[i].ChargeDueAgentPP = ds.Tables[1].Rows[i]["ChargeDueAgentPP"].ToString();
                            AWBRecords[i].ChargeWeightCC = ds.Tables[1].Rows[i]["ChargedWeightCC"].ToString();
                            AWBRecords[i].ValuationChargeCC = ds.Tables[1].Rows[i]["ValChargeCC"].ToString();
                            AWBRecords[i].OCDueCarCC = ds.Tables[1].Rows[i]["OCDueCarCC"].ToString();
                            AWBRecords[i].OCDueAgentCC = ds.Tables[1].Rows[i]["OCDueAgentCC"].ToString();
                            AWBRecords[i].CommissionPercentage = ds.Tables[1].Rows[i]["CommissionPercentage"].ToString();
                            AWBRecords[i].Commission = ds.Tables[1].Rows[i]["Commission"].ToString();
                            AWBRecords[i].CommissionIndicator = ds.Tables[1].Rows[i]["CommissionIndicator"].ToString();
                            AWBRecords[i].Discount = ds.Tables[1].Rows[i]["Discount"].ToString();
                            AWBRecords[i].AWBAcceptanceDate = ds.Tables[1].Rows[i]["AcceptanceDate"].ToString();
                            AWBRecords[i].AgentRefData = ds.Tables[1].Rows[i]["AgentReferenceCode"].ToString();
                            AWBRecords[i].RateOfExchange = ds.Tables[1].Rows[i]["RateOfExchange"].ToString();
                            AWBRecords[i].TaxDueAirline = ds.Tables[1].Rows[i]["TaxDueAirline"].ToString();
                            AWBRecords[i].TaxDueAgent = ds.Tables[1].Rows[i]["TaxDueAgent"].ToString();
                            AWBRecords[i].ReservedSpace = "";
                            AWBRecords[i].TaxDueAirlineIndicator = ds.Tables[1].Rows[i]["TaxDueAirlineIndicator"].ToString();
                            AWBRecords[i].DiscountIndicator = ds.Tables[1].Rows[i]["DiscountIndicator"].ToString();



                        }
                        #endregion

                        #region Retrieving DCR Records from DB
                        DCRDCORecord[] DCRSpecRecords = new DCRDCORecord[ds.Tables[2].Rows.Count];
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            DCRSpecRecords[i] = new DCRDCORecord();
                            DCRSpecRecords[i].RecordID = ds.Tables[2].Rows[i]["RecordID"].ToString();
                            DCRSpecRecords[i].BranchOfficeIndicator = ds.Tables[2].Rows[i]["BranchOfficeIndicator"].ToString();
                            DCRSpecRecords[i].VATIndicator = ds.Tables[2].Rows[i]["VATIndicator"].ToString();
                            DCRSpecRecords[i].AirlinePrefix = ds.Tables[2].Rows[i]["AirlinePrefix"].ToString();
                            DCRSpecRecords[i].AWBSerialNo = ds.Tables[2].Rows[i]["AWBNumber"].ToString();
                            DCRSpecRecords[i].AWBModCheck = ds.Tables[2].Rows[i]["AWBModCheck"].ToString();
                            DCRSpecRecords[i].Filler = " ";
                            DCRSpecRecords[i].Origin = ds.Tables[2].Rows[i]["Origin"].ToString();
                            DCRSpecRecords[i].AgentCode = ds.Tables[2].Rows[i]["AgentCode"].ToString();
                            if (ds.Tables[2].Rows[i]["DCMNumber"].ToString().Length > 6)
                            {
                                DCRSpecRecords[i].DCMNo = ds.Tables[2].Rows[i]["DCMNumber"].ToString().Substring(0, 6);
                            }
                            else
                                DCRSpecRecords[i].DCMNo = ds.Tables[2].Rows[i]["DCMNumber"].ToString();
                            DCRSpecRecords[i].CurrencyCode = ds.Tables[2].Rows[i]["Currency"].ToString();
                            DCRSpecRecords[i].RateOfExchange = ds.Tables[2].Rows[i]["RateOfExchange"].ToString();
                            DCRSpecRecords[i].DateAWBExecution = ds.Tables[2].Rows[i]["ExecutionDate"].ToString();
                            DCRSpecRecords[i].PPCCIndicator = ds.Tables[2].Rows[i]["PayMode"].ToString();
                            DCRSpecRecords[i].ChargeWeight = ds.Tables[2].Rows[i]["ChargedWeight"].ToString();
                            DCRSpecRecords[i].ValuationCharge = ds.Tables[2].Rows[i]["ValCharge"].ToString();
                            DCRSpecRecords[i].Taxes = ds.Tables[2].Rows[i]["Tax"].ToString();
                            DCRSpecRecords[i].ChargeDueAgent = ds.Tables[2].Rows[i]["OCDueAgent"].ToString();
                            DCRSpecRecords[i].ChargeDueCar = ds.Tables[2].Rows[i]["OCDueCar"].ToString();
                            DCRSpecRecords[i].VATOnAWBCharges = ds.Tables[2].Rows[i]["VATAWBCharges"].ToString();
                            DCRSpecRecords[i].Commission = ds.Tables[2].Rows[i]["Commission"].ToString();
                            DCRSpecRecords[i].VATOnCommission = ds.Tables[2].Rows[i]["VATAWBCommission"].ToString();
                            DCRSpecRecords[i].Discount = ds.Tables[2].Rows[i]["Discount"].ToString();
                            DCRSpecRecords[i].DiscountIndicator = ds.Tables[2].Rows[i]["DiscountIndicator"].ToString();
                            DCRSpecRecords[i].WeightIndicator = ds.Tables[2].Rows[i]["WeightIndicator"].ToString();
                            DCRSpecRecords[i].GrossWeight = ds.Tables[2].Rows[i]["GrossWeight"].ToString();
                            DCRSpecRecords[i].Destination = ds.Tables[2].Rows[i]["Destination"].ToString();
                            DCRSpecRecords[i].ReservedSpace = "";
                            DCRSpecRecords[i].ReasonAdjustment = "";


                        }

                        #endregion

                        #region Retrieving CCR Records from DB
                        DCRDCORecord[] CCRSpecRecords = new DCRDCORecord[ds.Tables[3].Rows.Count];
                        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                        {
                            CCRSpecRecords[i] = new DCRDCORecord();
                            CCRSpecRecords[i].RecordID = ds.Tables[3].Rows[i]["RecordID"].ToString();
                            CCRSpecRecords[i].BranchOfficeIndicator = ds.Tables[3].Rows[i]["BranchOfficeIndicator"].ToString();
                            CCRSpecRecords[i].VATIndicator = ds.Tables[3].Rows[i]["VATIndicator"].ToString();
                            CCRSpecRecords[i].AirlinePrefix = ds.Tables[3].Rows[i]["AirlinePrefix"].ToString();
                            CCRSpecRecords[i].AWBSerialNo = ds.Tables[3].Rows[i]["AWBNumber"].ToString();
                            CCRSpecRecords[i].AWBModCheck = ds.Tables[3].Rows[i]["AWBModCheck"].ToString();
                            CCRSpecRecords[i].Filler = " ";
                            CCRSpecRecords[i].Origin = ds.Tables[3].Rows[i]["Origin"].ToString();
                            CCRSpecRecords[i].AgentCode = ds.Tables[3].Rows[i]["AgentCode"].ToString();
                            if (ds.Tables[3].Rows[i]["CCANumber"].ToString().Length > 6)
                            {
                                CCRSpecRecords[i].DCMNo = ds.Tables[3].Rows[i]["CCANumber"].ToString().Substring(0, 6);
                            }
                            else
                                CCRSpecRecords[i].DCMNo = ds.Tables[3].Rows[i]["CCANumber"].ToString();
                            CCRSpecRecords[i].CurrencyCode = ds.Tables[3].Rows[i]["Currency"].ToString();
                            CCRSpecRecords[i].RateOfExchange = ds.Tables[3].Rows[i]["RateOfExchange"].ToString();
                            CCRSpecRecords[i].DateAWBExecution = ds.Tables[3].Rows[i]["ExecutionDate"].ToString();
                            CCRSpecRecords[i].PPCCIndicator = ds.Tables[3].Rows[i]["PayMode"].ToString();
                            CCRSpecRecords[i].ChargeWeight = ds.Tables[3].Rows[i]["ChargedWeight"].ToString();
                            CCRSpecRecords[i].ValuationCharge = ds.Tables[3].Rows[i]["ValCharge"].ToString();
                            CCRSpecRecords[i].Taxes = ds.Tables[3].Rows[i]["Tax"].ToString();
                            CCRSpecRecords[i].ChargeDueAgent = ds.Tables[3].Rows[i]["OCDueAgent"].ToString();
                            CCRSpecRecords[i].ChargeDueCar = ds.Tables[3].Rows[i]["OCDueCar"].ToString();
                            CCRSpecRecords[i].VATOnAWBCharges = ds.Tables[3].Rows[i]["VATAWBCharges"].ToString();
                            CCRSpecRecords[i].Commission = ds.Tables[3].Rows[i]["Commission"].ToString();
                            CCRSpecRecords[i].VATOnCommission = ds.Tables[3].Rows[i]["VATAWBCommission"].ToString();
                            CCRSpecRecords[i].Discount = ds.Tables[3].Rows[i]["Discount"].ToString();
                            CCRSpecRecords[i].DiscountIndicator = ds.Tables[3].Rows[i]["DiscountIndicator"].ToString();
                            CCRSpecRecords[i].WeightIndicator = ds.Tables[3].Rows[i]["WeightIndicator"].ToString();
                            CCRSpecRecords[i].GrossWeight = ds.Tables[3].Rows[i]["GrossWeight"].ToString();
                            CCRSpecRecords[i].Destination = ds.Tables[3].Rows[i]["Destination"].ToString();
                            CCRSpecRecords[i].ReservedSpace = "";
                            CCRSpecRecords[i].ReasonAdjustment = "";


                        }

                        #endregion

                        #region Retrieving DCO Records from DB
                        DCRDCORecord[] DCOSpecRecords = new DCRDCORecord[ds.Tables[4].Rows.Count];
                        for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                        {
                            DCOSpecRecords[i] = new DCRDCORecord();
                            DCOSpecRecords[i].RecordID = ds.Tables[4].Rows[i]["RecordID"].ToString();
                            DCOSpecRecords[i].BranchOfficeIndicator = ds.Tables[4].Rows[i]["BranchOfficeIndicator"].ToString();
                            DCOSpecRecords[i].VATIndicator = ds.Tables[4].Rows[i]["VATIndicator"].ToString();
                            DCOSpecRecords[i].AirlinePrefix = ds.Tables[4].Rows[i]["AirlinePrefix"].ToString();
                            DCOSpecRecords[i].AWBSerialNo = ds.Tables[4].Rows[i]["AWBNumber"].ToString();
                            DCOSpecRecords[i].AWBModCheck = ds.Tables[4].Rows[i]["AWBModCheck"].ToString();
                            DCOSpecRecords[i].Filler = " ";
                            DCOSpecRecords[i].Origin = ds.Tables[4].Rows[i]["Origin"].ToString();
                            DCOSpecRecords[i].AgentCode = ds.Tables[4].Rows[i]["AgentCode"].ToString();
                            if (ds.Tables[4].Rows[i]["DCMNumber"].ToString().Length > 6)
                            {
                                DCOSpecRecords[i].DCMNo = ds.Tables[4].Rows[i]["DCMNumber"].ToString().Substring(0, 6);
                            }
                            else
                                DCOSpecRecords[i].DCMNo = ds.Tables[4].Rows[i]["DCMNumber"].ToString();
                            DCOSpecRecords[i].CurrencyCode = ds.Tables[4].Rows[i]["Currency"].ToString();
                            DCOSpecRecords[i].RateOfExchange = ds.Tables[4].Rows[i]["RateOfExchange"].ToString();
                            DCOSpecRecords[i].DateAWBExecution = ds.Tables[4].Rows[i]["ExecutionDate"].ToString();
                            DCOSpecRecords[i].PPCCIndicator = ds.Tables[4].Rows[i]["PayMode"].ToString();
                            DCOSpecRecords[i].ChargeWeight = ds.Tables[4].Rows[i]["ChargedWeight"].ToString();
                            DCOSpecRecords[i].ValuationCharge = ds.Tables[4].Rows[i]["ValCharge"].ToString();
                            DCOSpecRecords[i].Taxes = ds.Tables[4].Rows[i]["Tax"].ToString();
                            DCOSpecRecords[i].ChargeDueAgent = ds.Tables[4].Rows[i]["OCDueAgent"].ToString();
                            DCOSpecRecords[i].ChargeDueCar = ds.Tables[4].Rows[i]["OCDueCar"].ToString();
                            DCOSpecRecords[i].VATOnAWBCharges = ds.Tables[4].Rows[i]["VATAWBCharges"].ToString();
                            DCOSpecRecords[i].Commission = ds.Tables[4].Rows[i]["Commission"].ToString();
                            DCOSpecRecords[i].VATOnCommission = ds.Tables[4].Rows[i]["VATAWBCommission"].ToString();
                            DCOSpecRecords[i].Discount = ds.Tables[4].Rows[i]["Discount"].ToString();
                            DCOSpecRecords[i].DiscountIndicator = ds.Tables[4].Rows[i]["DiscountIndicator"].ToString();
                            DCOSpecRecords[i].WeightIndicator = ds.Tables[4].Rows[i]["WeightIndicator"].ToString();
                            DCOSpecRecords[i].GrossWeight = ds.Tables[4].Rows[i]["GrossWeight"].ToString();
                            DCOSpecRecords[i].Destination = ds.Tables[4].Rows[i]["Destination"].ToString();
                            DCOSpecRecords[i].ReservedSpace = "";
                            DCOSpecRecords[i].ReasonAdjustment = "";


                        }





                        #endregion

                        #region Retrieving CCO Records from DB
                        DCRDCORecord[] CCOSpecRecords = new DCRDCORecord[ds.Tables[5].Rows.Count];
                        for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                        {
                            CCOSpecRecords[i] = new DCRDCORecord();
                            CCOSpecRecords[i].RecordID = ds.Tables[5].Rows[i]["RecordID"].ToString();
                            CCOSpecRecords[i].BranchOfficeIndicator = ds.Tables[5].Rows[i]["BranchOfficeIndicator"].ToString();
                            CCOSpecRecords[i].VATIndicator = ds.Tables[5].Rows[i]["VATIndicator"].ToString();
                            CCOSpecRecords[i].AirlinePrefix = ds.Tables[5].Rows[i]["AirlinePrefix"].ToString();
                            CCOSpecRecords[i].AWBSerialNo = ds.Tables[5].Rows[i]["AWBNumber"].ToString();
                            CCOSpecRecords[i].AWBModCheck = ds.Tables[5].Rows[i]["AWBModCheck"].ToString();
                            CCOSpecRecords[i].Filler = " ";
                            CCOSpecRecords[i].Origin = ds.Tables[5].Rows[i]["Origin"].ToString();
                            CCOSpecRecords[i].AgentCode = ds.Tables[5].Rows[i]["AgentCode"].ToString();
                            if (ds.Tables[5].Rows[i]["CCANumber"].ToString().Length > 6)
                            {
                                CCOSpecRecords[i].DCMNo = ds.Tables[5].Rows[i]["CCANumber"].ToString().Substring(0, 6);
                            }
                            else
                                CCOSpecRecords[i].DCMNo = ds.Tables[5].Rows[i]["CCANumber"].ToString();
                            CCOSpecRecords[i].CurrencyCode = ds.Tables[5].Rows[i]["Currency"].ToString();
                            CCOSpecRecords[i].RateOfExchange = ds.Tables[5].Rows[i]["RateOfExchange"].ToString();
                            CCOSpecRecords[i].DateAWBExecution = ds.Tables[5].Rows[i]["ExecutionDate"].ToString();
                            CCOSpecRecords[i].PPCCIndicator = ds.Tables[5].Rows[i]["PayMode"].ToString();
                            CCOSpecRecords[i].ChargeWeight = ds.Tables[5].Rows[i]["ChargedWeight"].ToString();
                            CCOSpecRecords[i].ValuationCharge = ds.Tables[5].Rows[i]["ValCharge"].ToString();
                            CCOSpecRecords[i].Taxes = ds.Tables[5].Rows[i]["Tax"].ToString();
                            CCOSpecRecords[i].ChargeDueAgent = ds.Tables[5].Rows[i]["OCDueAgent"].ToString();
                            CCOSpecRecords[i].ChargeDueCar = ds.Tables[5].Rows[i]["OCDueCar"].ToString();
                            CCOSpecRecords[i].VATOnAWBCharges = ds.Tables[5].Rows[i]["VATAWBCharges"].ToString();
                            CCOSpecRecords[i].Commission = ds.Tables[5].Rows[i]["Commission"].ToString();
                            CCOSpecRecords[i].VATOnCommission = ds.Tables[5].Rows[i]["VATAWBCommission"].ToString();
                            CCOSpecRecords[i].Discount = ds.Tables[5].Rows[i]["Discount"].ToString();
                            CCOSpecRecords[i].DiscountIndicator = ds.Tables[5].Rows[i]["DiscountIndicator"].ToString();
                            CCOSpecRecords[i].WeightIndicator = ds.Tables[5].Rows[i]["WeightIndicator"].ToString();
                            CCOSpecRecords[i].GrossWeight = ds.Tables[5].Rows[i]["GrossWeight"].ToString();
                            CCOSpecRecords[i].Destination = ds.Tables[5].Rows[i]["Destination"].ToString();
                            CCOSpecRecords[i].ReservedSpace = "";
                            CCOSpecRecords[i].ReasonAdjustment = "";


                        }





                        #endregion

                        #region Trailer Record

                        TrailerRecord TrailerRecords = new TrailerRecord();
                        TrailerRecords.RecordID = "TTT";
                        TrailerRecords.CASSAreaCode = "IN";
                        TrailerRecords.AirlinePrefix = ds.Tables[0].Rows[0]["AirlinePrefix"].ToString();
                        TrailerRecords.BranchOfficeIndicator = "A";
                        TrailerRecords.Filler = "";
                        TrailerRecords.NoOfAWB = (ds.Tables[1].Rows.Count).ToString();
                        TrailerRecords.NoOfCCA = (ds.Tables[3].Rows.Count + ds.Tables[5].Rows.Count).ToString();
                        TrailerRecords.NoOfDCM = (ds.Tables[2].Rows.Count + ds.Tables[4].Rows.Count).ToString();
                        TrailerRecords.TotalRecords = (ds.Tables[1].Rows.Count + ds.Tables[2].Rows.Count + ds.Tables[3].Rows.Count + ds.Tables[4].Rows.Count + ds.Tables[5].Rows.Count).ToString();
                        TrailerRecords.HashTotal = "";
                        TrailerRecords.NoOfAW1 = "";
                        TrailerRecords.NoOfCC3 = "";
                        TrailerRecords.NoOfDC3 = "";
                        TrailerRecords.ReservedSpace1 = "";
                        TrailerRecords.ReservedSpace2 = "";

                        #endregion

                        #region Writing Records to the ASCII File
                        StringBuilder sb = new StringBuilder();
                        //sw.WriteLine(HeaderRecords.ToString());
                        sb.AppendLine(HeaderRecords.ToString());

                        foreach (AWBRecord file in AWBRecords)
                        {
                            //sw.WriteLine(file.ToString());
                            sb.AppendLine(file.ToString());
                        }

                        foreach (DCRDCORecord file in DCRSpecRecords)
                        {
                            //sw.WriteLine(file.ToString());
                            sb.AppendLine(file.ToString());
                        }

                        foreach (DCRDCORecord file in CCRSpecRecords)
                        {
                            sb.AppendLine(file.ToString());
                        }

                        foreach (DCRDCORecord file in DCOSpecRecords)
                        {
                            //sw.WriteLine(file.ToString());
                            sb.AppendLine(file.ToString());
                        }
                        foreach (DCRDCORecord file in CCOSpecRecords)
                        {
                            sb.AppendLine(file.ToString());
                        }

                        // sw.WriteLine(TrailerRecords.ToString());
                        sb.AppendLine(TrailerRecords.ToString());

                        return sb;
                        //sw.Flush();
                        //sw.Close();
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
