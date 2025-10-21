using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;

namespace Course.Bll.Service.Interface
{
    public interface IPaymentService
    {
        Task<PaymentRes> AddAsync (PaymentReq entity, string userId, string HTTPReq);
        Task<int> DeleteAsync (int id);
        Task<IEnumerable<Payment>> GetAllAsync ();
        Task<Payment>? HandelSuccessAsync (int paymentId);
    }
}
