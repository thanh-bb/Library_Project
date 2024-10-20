namespace Library_API.Model_VNPay
{
    public class PaymentInformationModel
    {
        public string OrderType { get; set; }
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public string Name { get; set; }
        public string OrderId { get; set; }
        public string BankCode { get; set; }
    }
}
