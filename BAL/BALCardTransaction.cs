using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthorizeNet;

namespace BAL
{
    public class BALCardTransaction
    {
        string APILoginId = "7WTfp52H";
        string TransactionKey = "6Vc96R6G72Skc9g3";
        bool IsTestMode = true;

        public bool ProcessCardPayment(string CardType, string CardNumber,
            string CardExpiration, decimal TransactionAmount, string Description, string CVVNumber, string InvoiceNumber, string CustomerId,
            string CustomerFName, string CustomerLName, string CustomerAddress, string CustomerState, string CustomerPincode, string ShipperId,
            string ShipperFName, string ShipperLName, string ShipperAddress, string ShipperState, string ShipperPincode, ref string ErrorMessage, ref string TransactionId)
        {
            AuthorizationRequest request = null;
            Gateway gate = null;
            IGatewayResponse response = null;

            try
            {
                //step 1 - create the request
                request = new AuthorizationRequest(CardNumber, CardExpiration, TransactionAmount, Description);

                //These are optional calls to the API
                request.AddCardCode(CVVNumber);

                //Customer info - this is used for Fraud Detection
                request.AddCustomer(CustomerId, CustomerFName, CustomerLName, CustomerAddress, CustomerAddress, CustomerPincode);

                //order number
                request.AddInvoice(InvoiceNumber);

                //Custom values that will be returned with the response
                //request.AddMerchantValue("merchantValue", "value");

                //Shipping Address
                request.AddShipping(ShipperId, ShipperFName, ShipperLName, ShipperAddress, ShipperState, ShipperPincode);


                //step 2 - create the gateway, sending in your credentials and setting the Mode to Test (boolean flag)
                //which is true by default
                //this login and key are the shared dev account - you should get your own if you 
                //want to do more testing
                gate = new Gateway(APILoginId, TransactionKey, IsTestMode);

                //step 3 - make some money
                response = gate.Send(request);

                if (response != null && response.ResponseCode == "1")
                {
                    TransactionId = response.TransactionID;
                    return true;
                }
                else
                {
                    ErrorMessage = response.Message;
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                request = null;
                gate = null;
                response = null;
            }
        }
    }
}
