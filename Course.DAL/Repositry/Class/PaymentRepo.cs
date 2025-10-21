using Course.DAL.DataBase;
using Course.DAL.Models;

namespace Course.DAL.Repositry.Class
{
    public class PaymentRepo : GenralRepositry<Payment>, IPaymentRepo
    {
        public PaymentRepo (ApplicationDbContext context) : base(context)
        {
        }
    }
}
