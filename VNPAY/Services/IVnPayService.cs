using VNPAY.Models;

namespace VNPAY.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        string CreatePaymentUrl1(PaymentInformationModel model, HttpContext context);

        PaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
