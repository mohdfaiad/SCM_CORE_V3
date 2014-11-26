using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class IDECEncode
    {

        #region Mandatory Records

        #region File Header Record
        public class FileHeaderRecord
        {
            public string StandardMessageIdentifier { get; set; }
            public string RecordSequenceNo { get; set; }
            public string StandardFieldIdentifier { get; set; }
            public string AirlineCode { get; set; }
            public string VersionNumber { get; set; }
            public string Filler { get; set; }

            public override string ToString()
            {
                return
                    "CBD".PadRight(3, ' ') +
                    "00000001".PadLeft(8, '0') +
                    StandardFieldIdentifier.PadLeft(2, '0') +
                    AirlineCode.PadLeft(4, '0') +
                    VersionNumber.PadLeft(4, '0') +
                    Filler.PadRight(479, ' ');

            }



        }
        #endregion

        #region Invoice Header Record
        public class InvoiceHeaderRecord
        {
            public string StandardMessageIdentifier { get; set; }
            public string RecordSequenceNo { get; set; }
            public string BillingAirline{ get; set; }
            public string BilledAirline { get; set; }
            public string Filler1 { get; set; }
            public string InvoiceNumber { get; set; }
            public string Filler2 { get; set; }
            public string BatchSequenceNo { get; set; }
            public string RecordSequenceNoBatch { get; set; }
            public string BillingMonth { get; set; }
            public string Filler3 { get; set; }
            public string CurrencyListing { get; set; }
            public string CurrencyBilling { get; set; }
            public string Filler4 { get; set; }
            public string Filler5 { get; set; }
            public string Filler6 { get; set; }
            public string Filler7 { get; set; }
            public string PeriodNumber { get; set; }
            public string Filler8 { get; set; }
            public string Filler9 { get; set; }
            public string SettlementMethodIndicator{ get; set; }
            public string DigitalSignatureFlag { get; set; }
            public string InvoiceDate { get; set; }
            public string ListingBillingRate { get; set; }
            public string SuspendedFlag { get; set; }
            public string BillingAirlineLocID { get; set; }
            public string BilledAirlineLocID { get; set; }
            public string InvoiceType { get; set; }
            public string InvoiceTemplateLanguage { get; set; }
            public string Filler10 { get; set; }


           

            public override string ToString()
            {
                return
                    "CBD".PadRight(3, ' ') +
                    "00000002".PadLeft(8, '0') +
                    "15".PadLeft(2,'0') +
                    BillingAirline.PadLeft(4, '0') +
                    BilledAirline.PadLeft(4, '0') +
                    Filler1.PadRight(479, ' ') +
                    InvoiceNumber.PadRight(10, ' ') +
                    Filler2.PadRight(4, ' ') +
                    BatchSequenceNo.PadLeft(5, '0') +
                    RecordSequenceNoBatch.PadLeft(5, '0') +
                    BillingMonth.PadLeft(4, '0') +
                    Filler3.PadRight(2, ' ') +
                    CurrencyListing.PadRight(3, ' ') +
                    CurrencyBilling.PadRight(3, ' ') +
                    Filler4.PadRight(2, ' ') +
                    Filler5.PadRight(1, ' ') +
                    Filler6.PadRight(8, ' ') +
                    Filler7.PadRight(1,' ')+
                    PeriodNumber.PadLeft(2,'0')+
                    Filler8.PadRight(80,' ')+
                    Filler9.PadRight(8,' ')+
                    SettlementMethodIndicator.PadLeft(1, ' ')+
                    DigitalSignatureFlag.PadRight(1,' ')+
                    InvoiceDate.PadLeft(6,'0')+
                    ListingBillingRate.PadLeft(16,'0')+
                    SuspendedFlag.PadRight(1,' ')+
                    BillingAirlineLocID.PadRight(7,' ')+
                    BilledAirlineLocID.PadRight(7,' ')+
                    InvoiceType.PadRight(2,' ')+
                    InvoiceTemplateLanguage.PadRight(2,' ')+
                    Filler10.PadRight(297,' ');
                    
                    
                    
                    

            }



        }
        #endregion

        #region Billing Code Sub-Total Record
        public class BillingCodeRecord
        {
            public string RecordSequenceNo { get; set; }
            public string BillingAirline { get; set; }
            public string BilledAirline { get; set; }
            public string BillingCode { get; set; }
            public string InvoiceNumber { get; set; }
            public string Filler1 { get; set; }
            public string BatchSequenceNo { get; set; }
            public string RecordSequenceBatch { get; set; }
            public string TotalWeightCharges { get; set; }
            public string TotalOtherCharges { get; set; }
            public string InterlineServiceChargeAmtSign { get; set; }
            public string TotalInterlineServiceCharge { get; set; }
            public string BillingCodeSubTotal { get; set; }
            public string Filler2 { get; set; }
            public string TotalBillingRecords { get; set; }
            public string Filler3 { get; set; }
            public string Filler4 { get; set; }
            public string TotalValuationCharges { get; set; }
            public string TotalValuationChargesSign { get; set; }
            public string TotalVATAmount { get; set; }
            public string TotalVATAmountSign { get; set; }
            public string TotalWeightChargesSign { get; set; }
            public string TotalOtherChargesSign { get; set; }
            public string BillingCodeSubTotalSign { get; set; }
            public string TotalNumberOfRecords { get; set; }
            public string BillingCodeSubTotalDesc { get; set; }
            public string Filler5 { get; set; }

            public override string ToString()
            {
                return
                    "CBD".PadRight(3, ' ') +
                    RecordSequenceNo.PadLeft(8, '0') +
                    "45".PadLeft(2, '0') +
                    BillingAirline.PadLeft(4, '0') +
                    BilledAirline.PadLeft(4, '0') +
                    BillingCode.PadRight(1, ' ') +
                    InvoiceNumber.PadRight(10, ' ') +
                    Filler1.PadRight(4, ' ') +
                    BatchSequenceNo.PadLeft(5, '0') +
                    RecordSequenceBatch.PadLeft(5, '0') +
                    TotalWeightCharges.PadLeft(15, '0') +
                    TotalOtherCharges.PadLeft(15, '0') +
                    InterlineServiceChargeAmtSign.PadRight(1, ' ') +
                    TotalInterlineServiceCharge.PadLeft(15, '0') +
                    BillingCodeSubTotal.PadLeft(15, '0') +
                    Filler2.PadLeft(15, '9') +
                    TotalBillingRecords.PadLeft(6, '0') +
                    Filler3.PadRight(24, ' ') +
                    Filler4.PadRight(8, ' ') +
                    TotalValuationCharges.PadLeft(15, '0') +
                    TotalValuationChargesSign.PadRight(1, ' ') +
                    TotalVATAmount.PadLeft(15, '0') +
                    TotalVATAmountSign.PadRight(1, ' ') +
                    TotalWeightChargesSign.PadRight(1, ' ') +
                    TotalOtherChargesSign.PadRight(1, ' ') +
                    BillingCodeSubTotalSign.PadRight(1, ' ') +
                    TotalNumberOfRecords.PadLeft(8, '0') +
                    BillingCodeSubTotalDesc.PadRight(100, ' ') +
                    Filler5.PadRight(197, ' ');




            }



        }
        #endregion

        #region Invoice Total Record
        public class InvoiceTotalRecord
        {
            public string RecordSequenceNo { get; set; }
            public string BillingAirline { get; set; }
            public string BilledAirline { get; set; }
            public string BillingCode { get; set; }
            public string InvoiceNumber { get; set; }
            public string Filler1 { get; set; }
            public string BatchSequenceNumber { get; set; }
            public string RecordSequenceBatch { get; set; }
            public string TotalWeightCharges { get; set; }
            public string TotalOtherCharges { get; set; }
            public string InterlineServiceChargeAmtSign { get; set; }
            public string InterlineServiceChargeAmt { get; set; }
            public string NetInvoiceTotal { get; set; }
            public string NetInvoiceBillingTotal { get; set; }
            public string TotalBillingRecords { get; set; }
            public string Filler2 { get; set; }
            public string Filler3 { get; set; }
            public string TotalValuationCharges { get; set; }
            public string TotalValuationChargesSign { get; set; }
            public string TotalVATAmount { get; set; }
            public string TotalVATAmountSign { get; set; }
            public string TotalWeightChargesSign { get; set; }
            public string TotalOtherChargesSign { get; set; }
            public string NetInvoiceTotalSign { get; set; }
            public string NetInvoiceBillingTotalSign { get; set; }
            public string TotalRecords { get; set; }
            public string TotalNetAmountWithoutVAT { get; set; }
            public string TotalNetAmountWithoutVATSign { get; set; }
            public string Filler4 { get; set; }

            public override string ToString()
            {
                return
                    "CBD".PadRight(3, ' ') +
                    RecordSequenceNo.PadLeft(8, '0') +
                    "65".PadLeft(2, '0') +
                    BillingAirline.PadLeft(4, '0') +
                    BilledAirline.PadLeft(4, '0') +
                    BillingCode.PadRight(1, ' ') +
                    InvoiceNumber.PadRight(10, ' ') +
                    Filler1.PadRight(4, ' ') +
                    BatchSequenceNumber.PadLeft(8, '9') +
                    RecordSequenceBatch.PadLeft(8, '9') +
                    TotalWeightCharges.PadLeft(15, '0') +
                    TotalOtherCharges.PadLeft(15, '0') +
                    InterlineServiceChargeAmtSign.PadRight(1, ' ') +
                    InterlineServiceChargeAmt.PadLeft(15, '0') +
                    NetInvoiceTotal.PadLeft(15, '0') +
                    NetInvoiceBillingTotal.PadLeft(15, '0') +
                    TotalBillingRecords.PadLeft(6, '0') +
                    Filler2.PadRight(24, ' ') +
                    Filler3.PadRight(8, ' ') +
                    TotalValuationCharges.PadLeft(15, '0') +
                    TotalValuationChargesSign.PadRight(1, ' ') +
                    TotalVATAmount.PadLeft(15, '0') +
                    TotalVATAmountSign.PadRight(1, ' ') +
                    TotalWeightChargesSign.PadRight(1, ' ') +
                    TotalOtherChargesSign.PadRight(1, ' ') +
                    NetInvoiceTotalSign.PadRight(1, ' ') +
                    NetInvoiceBillingTotalSign.PadRight(1, ' ') +
                    TotalRecords.PadLeft(8, '0') +
                    TotalNetAmountWithoutVAT.PadLeft(15, '0') +
                    TotalNetAmountWithoutVATSign.PadRight(1, ' ') +
                    Filler4.PadRight(280, ' ');



            }


        }
        #endregion

        #region File Total Record
        public class FileTotalRecord
        {
            public string RecordSequenceNo { get; set; }
            public string BillingAirline { get; set; }
            public string BilledAirline { get; set; }
            public string InvoiceNumber { get; set; }
            public string Filler1 { get; set; }
            public string BatchSequenceNumber { get; set; }
            public string RecordSequenceBatch { get; set; }
            public string TotalWeightCharges { get; set; }
            public string TotalOtherCharges { get; set; }
            public string Filler2 { get; set; }
            public string TotalInterlineServiceChargeAmt { get; set; }
            public string NetInvoiceTotal { get; set; }
            public string NetInvoiceBillingTotal { get; set; }
            public string TotalBillingRecords { get; set; }
            public string Filler3 { get; set; }
            public string Filler4 { get; set; }
            public string TotalValuationCharges { get; set; }
            public string Filler5 { get; set; }
            public string TotalVATAmount { get; set; }
            public string Filler6 { get; set; }
            public string TotalRecords { get; set; }
            public string Filler7 { get; set; }

            public override string ToString()
            {
                return
                    "CBD".PadRight(3, ' ') +
                    RecordSequenceNo.PadLeft(8, '0') +
                    "75".PadLeft(2, '0') +
                    BillingAirline.PadLeft(4, '0') +
                    BilledAirline.PadLeft(4, '9') +
                    "9".PadLeft(1, '9') +
                    InvoiceNumber.PadRight(10, '9') +
                    Filler1.PadRight(4, '9') +
                    BatchSequenceNumber.PadLeft(5, '9') +
                    RecordSequenceBatch.PadLeft(5, '9') +
                    TotalWeightCharges.PadLeft(15, '0') +
                    TotalOtherCharges.PadLeft(15, '0') +
                    Filler2.PadRight(1, ' ') +
                    TotalInterlineServiceChargeAmt.PadLeft(15, '0') +
                    NetInvoiceTotal.PadLeft(15, '0') +
                    NetInvoiceBillingTotal.PadLeft(15, '0') +
                    TotalBillingRecords.PadLeft(6, '0') +
                    Filler3.PadRight(24, ' ') +
                    Filler4.PadRight(8, ' ') +
                    TotalValuationCharges.PadLeft(15, '0') +
                    Filler5.PadRight(1, ' ') +
                    TotalVATAmount.PadLeft(15, '0') +
                    Filler6.PadRight(5, ' ') +
                    TotalRecords.PadLeft(8, '0') +
                    Filler7.PadRight(296, ' ');

            }


        }
        #endregion

        #endregion

        #region Conditional Records

        #region Conditional AWB Records
        #region AWB Records
        public class AWBRecord
        {

            public string RecordSequenceNo { get; set; }
            public string BillingAirline { get; set; }
            public string BilledAirline { get; set; }
            public string BillingCode { get; set; }
            public string InvoiceNumber { get; set; }
            public string Filler1 { get; set; }
            public string BatchSequenceNo { get; set; }
            public string AWBDate { get; set; }
            public string AWBIssuingAirline { get; set; }
            public string AWBSerialNo { get; set; }
            public string AWBCheckDigit { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public string DateOfCarriage { get; set; }
            public string WeightCharges { get; set; }
            public string OtherCharges { get; set; }
            public string InterlineServiceCharge { get; set; }
            public string InterlineServiceChargePercent { get; set; }
            public string InterlineServiceChargeRateSign { get; set; }
            public string CurrencyAdjustmentIndicator { get; set; }
            public string BilledWeight { get; set; }
            public string Prorate { get; set; }
            public string ProratePercent { get; set; }
            public string PartShipmentIndicator { get; set; }
            public string Filler2 { get; set; }
            public string FillingReference { get; set; }
            public string Filler3 { get; set; }
            public string WeightChargesSign { get; set; }
            public string OtherChargesSign { get; set; }
            public string ValuationCharges { get; set; }
            public string ValuationChargesSign { get; set; }
            public string WeightIndicator { get; set; }
            public string VATAmount { get; set; }
            public string VATAmountSign { get; set; }
            public string InterlineServiceChargeAmt { get; set; }
            public string InterlineServiceChargeAmtSign { get; set; }
            public string AWBTotalAmt { get; set; }
            public string AWBTotalAmtSign { get; set; }
            public string CCAIndicator { get; set; }
            public string OurReference { get; set; }
            public string AttachmentIndicatorOriginal { get; set; }
            public string AttachmentIndicatorValidated { get; set; }
            public string NoOfAttachments { get; set; }
            public string ValidationFlag { get; set; }
            public string ReasonCode { get; set; }
            public string ReferenceField1 { get; set; }
            public string ReferenceField2 { get; set; }
            public string ReferenceField3 { get; set; }
            public string ReferenceField4 { get; set; }
            public string ReferenceField5 { get; set; }
            public string AirlineOwnUse { get; set; }
            public string InterlineServiceChargeSign { get; set; }
            public string Filler4 { get; set; }

            public override string ToString()
            {
                return
                    "CBD".PadRight(3, ' ') +
                    RecordSequenceNo.PadLeft(8, '0') +
                    "25".PadLeft(2, '0') +
                    BillingAirline.PadLeft(4, '0') +
                    BilledAirline.PadLeft(4, '0') +
                    BillingCode.PadRight(1, ' ') +
                    InvoiceNumber.PadRight(10, ' ') +
                    Filler1.PadRight(4, ' ') +
                    BatchSequenceNo.PadLeft(5, '0') +
                    AWBDate.PadLeft(6, '0') +
                    AWBIssuingAirline.PadLeft(4, '0') +
                    AWBSerialNo.PadLeft(7, '0') +
                    AWBCheckDigit.PadLeft(1, '0') +
                    Origin.PadRight(4, ' ') +
                    Destination.PadRight(4, ' ') +
                    From.PadRight(4, ' ') +
                    To.PadRight(4, ' ') +
                    DateOfCarriage.PadLeft(6, '0') +
                    WeightCharges.PadLeft(11, '0') +
                    OtherCharges.PadLeft(11, '0') +
                    InterlineServiceCharge.PadLeft(11, '0') +
                    InterlineServiceChargePercent.PadLeft(5, '0') +
                    InterlineServiceChargeRateSign.PadRight(1, ' ') +
                    CurrencyAdjustmentIndicator.PadRight(3, ' ') +
                    BilledWeight.PadLeft(6, '0') +
                    Prorate.PadRight(1, ' ') +
                    ProratePercent.PadLeft(2, '0') +
                    PartShipmentIndicator.PadRight(1, ' ') +
                    Filler2.PadRight(4, ' ') +
                    FillingReference.PadRight(10, ' ') +
                    Filler3.PadRight(8, ' ') +
                    WeightChargesSign.PadRight(1, ' ') +
                    OtherChargesSign.PadRight(1, ' ') +
                    ValuationCharges.PadLeft(11, ' ') +
                    ValuationChargesSign.PadRight(1, ' ') +
                    WeightIndicator.PadRight(1, ' ') +
                    VATAmount.PadLeft(11, '0') +
                    VATAmountSign.PadRight(1, ' ') +
                    InterlineServiceChargeAmt.PadLeft(11, '0') +
                    InterlineServiceChargeAmtSign.PadRight(1, ' ') +
                    AWBTotalAmt.PadLeft(11, '0') +
                    AWBTotalAmtSign.PadRight(1, ' ') +
                    CCAIndicator.PadRight(1, ' ') +
                    OurReference.PadRight(20, ' ') +
                    AttachmentIndicatorOriginal.PadRight(1, 'N') +
                    AttachmentIndicatorValidated.PadRight(1, 'N') +
                    NoOfAttachments.PadLeft(4, '0') +
                    ValidationFlag.PadRight(10, ' ') +
                    ReasonCode.PadRight(2, ' ') +
                    ReferenceField1.PadRight(10, ' ') +
                    ReferenceField2.PadRight(10, ' ') +
                    ReferenceField3.PadRight(10, ' ') +
                    ReferenceField4.PadRight(10, ' ') +
                    ReferenceField5.PadRight(20, ' ') +
                    AirlineOwnUse.PadRight(20, ' ') +
                    InterlineServiceChargeSign.PadRight(1, ' ') +
                    Filler4.PadRight(169, ' ');



            }
        }
        #endregion

        #region AWB VAT Breakdown Records
        public class AWBVATBreakdownRecords
        {
            public string RecordSequenceNo { get; set; }
            public string BillingAirline{ get; set; }
            public string BilledAirline { get; set; }
            public string BillingCode { get; set; }
            public string InvoiceNumber { get; set; }
            public string Filler1 { get; set; }
            public string Filler2 { get; set; }
            public string AWBIssuingAirline { get; set; }
            public string AWBSerialNo { get; set; }
            public string AWBCheckDigit { get; set; }
            public string Filler3 { get; set; }
            public string VATIdentifier1 { get; set; }
            public string VATLabel1 { get; set; }
            public string VATText1 { get; set; }
            public string VATBaseAmount1 { get; set; }
            public string VATBaseAmountSign1 { get; set; }
            public string VATPercentage1 { get; set; }
            public string VATPercentageSign1 { get; set; }
            public string VATCalculatedAmount1 { get; set; }
            public string VATCalculatedAmountSign1 { get; set; }
            public string Filler4 { get; set; }
            public string VATIdentifier2 { get; set; }
            public string VATLabel2 { get; set; }
            public string VATText2 { get; set; }
            public string VATBaseAmount2 { get; set; }
            public string VATBaseAmountSign2 { get; set; }
            public string VATPercentage2 { get; set; }
            public string VATPercentageSign2 { get; set; }
            public string VATCalculatedAmount2 { get; set; }
            public string VATCalculatedAmountSign2 { get; set; }
            public string Filler5 { get; set; }


            public override string ToString()
            {
                return
                    "CBD".PadRight(3, ' ') +
                    RecordSequenceNo.PadLeft(8, '0') +
                    "28".PadLeft(2, '0') +
                    BillingAirline.PadLeft(4, '0') +
                    BilledAirline.PadLeft(4, '0') +
                    BillingCode.PadRight(1, ' ') +
                    InvoiceNumber.PadRight(10, ' ') +
                    Filler1.PadRight(4, ' ') +
                    Filler2.PadRight(11, ' ') +
                    AWBIssuingAirline.PadLeft(4, '0') +
                    AWBSerialNo.PadLeft(7, '0') +
                    AWBCheckDigit.PadLeft(1, '0') +
                    Filler3.PadRight(50, ' ') +
                    VATIdentifier1.PadRight(2, ' ') +
                    VATLabel1.PadRight(5, ' ') +
                    VATText1.PadRight(50, ' ') +
                    VATBaseAmount1.PadLeft(11, '0') +
                    VATBaseAmountSign1.PadRight(1, ' ') +
                    VATPercentage1.PadLeft(5, '0') +
                    VATPercentageSign1.PadRight(1, ' ') +
                    VATCalculatedAmount1.PadLeft(11, '0') +
                    VATCalculatedAmountSign1.PadRight(1, ' ') +
                    Filler4.PadRight(50, ' ') +
                    VATIdentifier2.PadRight(2, ' ') +
                    VATLabel2.PadRight(5, ' ') +
                    VATText2.PadRight(50, ' ') +
                    VATBaseAmount2.PadLeft(11, '0') +
                    VATBaseAmountSign2.PadRight(1, ' ') +
                    VATPercentage2.PadLeft(5, '0') +
                    VATPercentageSign2.PadRight(1, ' ') +
                    VATCalculatedAmount2.PadLeft(11, '0') +
                    VATCalculatedAmountSign2.PadRight(1, ' ') +
                    Filler5.PadRight(167, ' ');
            }
            
        }
        #endregion

        #region AWB OC Breakdown Records
        public class AWBOCBreakDownRecords
        {
            public string RecordSequenceNo { get; set; }
            public string BillingAirline { get; set; }
            public string BilledAirline { get; set; }
            public string BillingCode { get; set; }
            public string InvoiceNumber { get; set; }
            public string Filler1 { get; set; }
            public string Filler2 { get; set; }
            public string AWBIssuingAirline { get; set; }
            public string AWBSerialNo { get; set; }
            public string AWBCheckDigit { get; set; }
            public string Filler3 { get; set; }
            public string OtherChargeCode1 { get; set; }
            public string OtherChargeCodeValue1 { get; set; }
            public string OtherChargeCodeValueSign1 { get; set; }
            public string VATIdentifier1 { get; set; }
            public string VATLabel1 { get; set; }
            public string VATText1 { get; set; }
            public string VATBaseAmount1 { get; set; }
            public string VATBaseAmountSign1 { get; set; }
            public string VATPercentage1 { get; set; }
            public string VATPercentageSign1 { get; set; }
            public string VATCalculatedAmount1 { get; set; }
            public string VATCalculatedAmountSign1 { get; set; }
            public string OtherChargeCode2 { get; set; }
            public string OtherChargeCodeValue2 { get; set; }
            public string OtherChargeCodeValueSign2 { get; set; }
            public string VATIdentifier2 { get; set; }
            public string VATLabel2 { get; set; }
            public string VATText2 { get; set; }
            public string VATBaseAmount2 { get; set; }
            public string VATBaseAmountSign2 { get; set; }
            public string VATPercentage2 { get; set; }
            public string VATPercentageSign2 { get; set; }
            public string VATCalculatedAmount2 { get; set; }
            public string VATCalculatedAmountSign2 { get; set; }
            public string OtherChargeCode3 { get; set; }
            public string OtherChargeCodeValue3 { get; set; }
            public string OtherChargeCodeValueSign3 { get; set; }
            public string VATIdentifier3 { get; set; }
            public string VATLabel3 { get; set; }
            public string VATText3 { get; set; }
            public string VATBaseAmount3 { get; set; }
            public string VATBaseAmountSign3 { get; set; }
            public string VATPercentage3 { get; set; }
            public string VATPercentageSign3 { get; set; }
            public string VATCalculatedAmount3 { get; set; }
            public string VATCalculatedAmountSign3 { get; set; }
            public string Filler4 { get; set; }


            public override string ToString()
            {
                return
                    "CBD".PadRight(3, ' ') +
                    RecordSequenceNo.PadLeft(8, '0') +
                    "29".PadLeft(2, '0') +
                    BillingAirline.PadLeft(4, '0') +
                    BilledAirline.PadLeft(4, '0') +
                    BillingCode.PadRight(1, ' ') +
                    InvoiceNumber.PadRight(10, ' ') +
                    Filler1.PadRight(4, ' ') +
                    Filler2.PadRight(11, ' ') +
                    AWBIssuingAirline.PadLeft(4, '0') +
                    AWBSerialNo.PadLeft(7, '0') +
                    AWBCheckDigit.PadLeft(1, '0') +
                    Filler3.PadRight(14, ' ') +
                    OtherChargeCode1.PadRight(2, ' ') +
                    OtherChargeCodeValue1.PadLeft(11, '0') +
                    OtherChargeCodeValueSign1.PadRight(1, ' ') +
                    VATIdentifier1.PadRight(2, ' ') +
                    VATLabel1.PadRight(5, ' ') +
                    VATText1.PadRight(50, ' ') +
                    VATBaseAmount1.PadLeft(11, '0') +
                    VATBaseAmountSign1.PadRight(1, ' ') +
                    VATPercentage1.PadLeft(5, '0') +
                    VATPercentageSign1.PadRight(1, ' ') +
                    VATCalculatedAmount1.PadLeft(11, '0') +
                    VATCalculatedAmountSign1.PadRight(1, ' ') +
                    OtherChargeCode2.PadRight(2, ' ') +
                    OtherChargeCodeValue2.PadLeft(11, '0') +
                    OtherChargeCodeValueSign2.PadRight(1, ' ') +
                    VATIdentifier2.PadRight(2, ' ') +
                    VATLabel2.PadRight(5, ' ') +
                    VATText2.PadRight(50, ' ') +
                    VATBaseAmount2.PadLeft(11, '0') +
                    VATBaseAmountSign2.PadRight(1, ' ') +
                    VATPercentage2.PadLeft(5, '0') +
                    VATPercentageSign2.PadRight(1, ' ') +
                    VATCalculatedAmount2.PadLeft(11, '0') +
                    VATCalculatedAmountSign2.PadRight(1, ' ') +
                    OtherChargeCode3.PadRight(2, ' ') +
                    OtherChargeCodeValue3.PadLeft(11, '0') +
                    OtherChargeCodeValueSign3.PadRight(1, ' ') +
                    VATIdentifier3.PadRight(2, ' ') +
                    VATLabel3.PadRight(5, ' ') +
                    VATText3.PadRight(50, ' ') +
                    VATBaseAmount3.PadLeft(11, '0') +
                    VATBaseAmountSign3.PadRight(1, ' ') +
                    VATPercentage3.PadLeft(5, '0') +
                    VATPercentageSign3.PadRight(1, ' ') +
                    VATCalculatedAmount3.PadLeft(11, '0') +
                    VATCalculatedAmountSign3.PadRight(1, ' ') +
                    Filler4.PadRight(130, ' ');

            }
        }
        #endregion
        #endregion

        #region Conditional Rejection Memo Records
        public class RejectionMemoRecords
        {
            public string RecordSequenceNo { get; set; }
            public string BillingAirline { get; set; }
            public string BilledAirline { get; set; }
            public string BillingCode { get; set; }
            public string InvoiceNumber { get; set; }
            public string Filler1 { get; set; }
            public string BatchSequenceNo { get; set; }
            public string RecordSequenceBatch { get; set; }
            public string RejectionMemoNo { get; set; }
            public string RejectionStage { get; set; }
            public string ReasonCode { get; set; }
            public string AirlineOwnUse { get; set; }
            public string YourInvoiceNo { get; set; }
            public string Filler2 { get; set; }
            public string YourInvoiceBillingMonth { get; set; }
            public string YourRejectionMemoNo { get; set; }
            public string YourBillingCreditMemoNo { get; set; }
            public string TotalWeightChargesBilled { get; set; }
            public string TotalWeightChargesBilledSign { get; set; }
            public string TotalWeightChargesAccepted { get; set; }
            public string TotalWeightChargesAcceptedSign { get; set; }
            public string TotalWeightChargesDifference { get; set; }
            public string TotalWeightChargesDifferenceSign { get; set; }
            public string TotalValuationChargesBilled { get; set; }
            public string TotalValuationChargesBilledSign { get; set; }
            public string TotalValuationChargesAccepted { get; set; }
            public string TotalValuationChargesAcceptedSign { get; set; }
            public string TotalValuationChargesDifference { get; set; }
            public string TotalValuationChargesDifferenceSign { get; set; }
            public string TotalOtherChargesAmtBilled { get; set; }
            public string TotalOtherChargesAmtBilledSign { get; set; }
            public string TotalOtherChargesAmountAccepted { get; set; }
            public string TotalOtherChargesAmountAcceptedSign { get; set; }
            public string TotalOtherChargesDifference { get; set; }
            public string TotalOtherChargesDifferenceSign { get; set; }
            public string TotalISCAmountAllowed { get; set; }
            public string TotalISCAmountAllowedSign { get; set; }
            public string TotalISCAmountAccepted { get; set; }
            public string TotalISCAmountAcceptedSign { get; set; }
            public string TotalISCAmountDifference { get; set; }
            public string TotalISCAmountDifferenceSign { get; set; }
            public string TotalVATAmountBilled { get; set; }
            public string TotalVATAmountBilledSign { get; set; }
            public string TotalVATAmountAccepted { get; set; }
            public string TotalVATAmountAcceptedSign { get; set; }
            public string TotalVATAmountDifference { get; set; }
            public string TotalVATAmountDifferenceSign { get; set; }
            public string TotalNetRejectAmount { get; set; }
            public string TotalNetRejectAmountSign { get; set; }
            public string AttachmentIndicatorOriginal { get; set; }
            public string AttachmentIndicatorValidated { get; set; }
            public string NoOfAttachments{ get; set; }
            public string IsValidation { get; set; }
            public string BMCMIndicator { get; set; }
            public string OurRef { get; set; }
            public string Filler3 { get; set; }


            public override string ToString()
            {
                return
                    "CBD".PadRight(3, ' ') +
                    RecordSequenceNo.PadLeft(8, '0') +
                    "35".PadLeft(2, '0') +
                    BillingAirline.PadLeft(4, '0') +
                    BilledAirline.PadLeft(4, '0') +
                    BillingCode.PadRight(1, ' ') +
                    InvoiceNumber.PadRight(10, ' ') +
                    Filler1.PadRight(4, ' ') +
                    BatchSequenceNo.PadLeft(5, '0') +
                    RecordSequenceBatch.PadLeft(5, '0') +
                    RejectionMemoNo.PadRight(11, ' ') +
                    RejectionStage.PadLeft(1, '0') +
                    ReasonCode.PadRight(2, ' ') +
                    AirlineOwnUse.PadRight(20, ' ') +
                    YourInvoiceNo.PadRight(10, ' ') +
                    Filler2.PadRight(4, ' ') +
                    YourInvoiceBillingMonth.PadLeft(6, '0') +
                    YourRejectionMemoNo.PadRight(11, ' ') +
                    YourBillingCreditMemoNo.PadRight(11, ' ') +
                    TotalWeightChargesBilled.PadLeft(15, '0') +
                    TotalWeightChargesBilledSign.PadRight(1, ' ') +
                    TotalWeightChargesAccepted.PadLeft(15, '0') +
                    TotalWeightChargesAcceptedSign.PadRight(1, ' ') +
                    TotalWeightChargesDifference.PadLeft(15, '0') +
                    TotalWeightChargesDifferenceSign.PadRight(1, ' ') +
                    TotalValuationChargesBilled.PadLeft(15, '0') +
                    TotalValuationChargesBilledSign.PadRight(1, ' ') +
                    TotalValuationChargesAccepted.PadLeft(15, '0') +
                    TotalValuationChargesAcceptedSign.PadRight(1, ' ') +
                    TotalValuationChargesDifference.PadLeft(15, '0') +
                    TotalValuationChargesDifferenceSign.PadRight(1, ' ') +
                    TotalOtherChargesAmtBilled.PadLeft(15, '0') +
                    TotalOtherChargesAmtBilledSign.PadRight(1, ' ') +
                    TotalOtherChargesAmountAccepted.PadLeft(15, '0') +
                    TotalOtherChargesAmountAcceptedSign.PadRight(1, ' ') +
                    TotalOtherChargesDifference.PadLeft(15, '0') +
                    TotalOtherChargesDifferenceSign.PadRight(1, ' ') +
                    TotalISCAmountAllowed.PadLeft(15, '0') +
                    TotalISCAmountAllowedSign.PadRight(1, ' ') +
                    TotalISCAmountAccepted.PadLeft(15, '0') +
                    TotalISCAmountAcceptedSign.PadRight(1, ' ') +
                    TotalISCAmountDifference.PadLeft(15, '0') +
                    TotalISCAmountDifferenceSign.PadRight(1, ' ') +
                    TotalVATAmountBilled.PadLeft(15, '0') +
                    TotalVATAmountBilledSign.PadRight(1, ' ') +
                    TotalVATAmountAccepted.PadLeft(15, '0') +
                    TotalVATAmountAcceptedSign.PadRight(1, ' ') +
                    TotalVATAmountDifference.PadLeft(15, '0') +
                    TotalVATAmountDifferenceSign.PadRight(1, ' ') +
                    TotalNetRejectAmount.PadLeft(15, '0') +
                    TotalNetRejectAmountSign.PadRight(1, ' ') +
                    AttachmentIndicatorOriginal.PadRight(1, ' ') +
                    AttachmentIndicatorValidated.PadRight(1, ' ') +
                    NoOfAttachments.PadLeft(4, '0') +
                    IsValidation.PadRight(10, ' ') +
                    BMCMIndicator.PadRight(1, ' ') +
                    OurRef.PadRight(20, ' ') +
                    Filler3.PadRight(85, ' ');

            }

        }
        #endregion


        #region Reference Data Part-1
        public class RefDataPart1
        {
            public string SFI { get; set; }
            public string RecordSequenceNo { get; set; }
            public string BillingAirline { get; set; }
            public string BilledAirline { get; set; }
            public string Filler1 { get; set; }
            public string InvoiceNumber { get; set; }
            public string Filler2 { get; set; }
            public string RecordSerialNo { get; set; }
            public string CompanyLegalName { get; set; }
            public string TaxRegistrationID { get; set; }
            public string AdditionalTaxRegistrationID { get; set; }
            public string CompanyRegistrationID { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string Filler3 { get; set; }
            

            public override string ToString()
            {
                return
                    "CBD".PadRight(3, ' ') +
                    RecordSequenceNo.PadLeft(8, '0') +
                    SFI.PadLeft(2, '0') +
                    BillingAirline.PadLeft(4, '0') +
                    BilledAirline.PadLeft(4, '0') +
                    Filler1.PadRight(1, ' ') +
                    InvoiceNumber.PadRight(10, ' ') +
                    Filler2.PadRight(4, ' ') +
                    RecordSerialNo.PadLeft(1, '0') +
                    CompanyLegalName.PadRight(100, ' ') +
                    TaxRegistrationID.PadRight(25, ' ') +
                    AdditionalTaxRegistrationID.PadRight(25, ' ') +
                    CompanyRegistrationID.PadRight(25, ' ') +
                    AddressLine1.PadRight(70, ' ') +
                    AddressLine2.PadRight(70, ' ') +
                   AddressLine3.PadRight(70, ' ') +
                   Filler3.PadRight(78, ' ');



            }
        }

        #endregion

        #region Reference Data Part-2
        public class RefDataPart2
        {
            public string SFI { get; set; }
            public string RecordSequenceNo { get; set; }
            public string BillingAirline { get; set; }
            public string BilledAirline { get; set; }
            public string Filler1 { get; set; }
            public string InvoiceNumber { get; set; }
            public string Filler2 { get; set; }
            public string RecordSerialNo { get; set; }
            public string CityName { get; set; }
            public string SubDivisionCode { get; set; }
            public string SubDivisionName { get; set; }
            public string CountryCode { get; set; }
            public string CountryName { get; set; }
            public string PostalCode { get; set; }
            public string Filler3 { get; set; }


            public override string ToString()
            {
                return
                    "CBD".PadRight(3, ' ') +
                    RecordSequenceNo.PadLeft(8, '0') +
                    SFI.PadLeft(2, '0') +
                    BillingAirline.PadLeft(4, '0') +
                    BilledAirline.PadLeft(4, '0') +
                    Filler1.PadRight(1, ' ') +
                    InvoiceNumber.PadRight(10, ' ') +
                    Filler2.PadRight(4, ' ') +
                    RecordSerialNo.PadLeft(1, '0') +
                    CityName.PadRight(50, ' ') +
                    SubDivisionCode.PadRight(3, ' ') +
                    SubDivisionName.PadRight(50, ' ') +
                    CountryCode.PadRight(2, ' ') +
                    CountryName.PadRight(50, ' ') +
                    PostalCode.PadRight(50, ' ') +
                   Filler3.PadRight(258, ' ');



            }
        }

        #endregion

        #region Invoice Footer Record

        public class InvoiceFooterRecord
        {
            public string SFI { get; set; }
            public string RecordSequenceNo { get; set; }
            public string BillingAirline { get; set; }
            public string BilledAirline { get; set; }
            public string BillingCode { get; set; }
            public string InvoiceNumber { get; set; }
            public string Filler1 { get; set; }
            public string FooterSerialNo { get; set; }
            public string FooterDetails1 { get; set; }
            public string FooterDetails2 { get; set; }
            public string FooterDetails3 { get; set; }
            public string FooterDetails4 { get; set; }
            public string FooterDetails5 { get; set; }
            public string Filler2 { get; set; }


            public override string ToString()
            {
                return
                    "CBD".PadRight(3, ' ') +
                    RecordSequenceNo.PadLeft(8, '0') +
                    SFI.PadLeft(2, '0') +
                    BillingAirline.PadLeft(4, '0') +
                    BilledAirline.PadLeft(4, '0') +
                    BillingCode.PadRight(1, ' ') +
                    InvoiceNumber.PadRight(10, ' ') +
                    Filler1.PadRight(4, ' ') +
                    FooterSerialNo.PadLeft(1, '0') +
                    FooterDetails1.PadRight(70, ' ') +
                    FooterDetails2.PadRight(70, ' ') +
                    FooterDetails3.PadRight(70, ' ') +
                    FooterDetails4.PadRight(70, ' ') +
                    FooterDetails5.PadRight(70, ' ') +
                   Filler2.PadRight(113, ' ');



            }
        }
        #endregion

        #endregion

    }
}
