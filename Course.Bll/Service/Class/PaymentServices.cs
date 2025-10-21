using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;
using Course.DAL.Repositry;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;

namespace Course.Bll.Service.Class
{
    public class PaymentServices : IPaymentService
    {
        private readonly ICourseRepositry course;
        private readonly IPaymentRepo paymentRepo;
        private readonly IEnrollmentsRepo enrollments;
        private readonly IConfiguration _configuration;

        public PaymentServices (ICourseRepositry Course, IPaymentRepo paymentRepo, IEnrollmentsRepo enrollments, IConfiguration configuration)
        {
            course=Course;
            this.paymentRepo=paymentRepo;
            this.enrollments=enrollments;
            _configuration=configuration;
        }

        public async Task<PaymentRes> AddAsync (PaymentReq entity, string userId, string HTTPReq)
        {
            var Course = await course.GetByIdAsync(entity.CourseId);
            if (Course==null)
                return new PaymentRes
                {
                    Success=false,
                    Message="Course Not Found"
                };
            var existingEnrollment = await enrollments.GetAllAsync(e => e.UserId==userId&&e.CourseId==Course.Id);
            if (existingEnrollment.Any())
            {
                return new PaymentRes
                {
                    Success=false,
                    Message="You are already enrolled in this course."
                };
            }

            if (entity.PaymentMethod==PaymentMethod.Cash||
                entity.PaymentMethod==PaymentMethod.free)
            {
                var payment = new Payment
                {
                    UserId=userId,
                    Amount=Course.Price,
                    PaymentDate=DateTime.UtcNow,
                    PaymentMethod=entity.PaymentMethod,
                    Status=PaymentStatus.Completed,
                    CourseId=Course.Id,
                };
                await paymentRepo.AddAsync(payment);
                await paymentRepo.SaveChangesAsync();

                string msg = entity.PaymentMethod==DAL.Models.PaymentMethod.Cash
                    ? "Redirect to payment gateway"
                    : "You have been enrolled successfully";
                await HandelSuccessAsync(payment.Id);
                return new PaymentRes
                {
                    Success=true,
                    Message=msg,
                };
            }

            else if (entity.PaymentMethod==DAL.Models.PaymentMethod.Visa)
            {
                var payment = new Payment
                {
                    UserId=userId,
                    Amount=Course.Price,
                    PaymentDate=DateTime.UtcNow,
                    CourseId=Course.Id,
                    PaymentMethod=entity.PaymentMethod,
                    Status=PaymentStatus.Pending
                };
                await paymentRepo.AddAsync(payment);
                await paymentRepo.SaveChangesAsync();

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes=new List<string> { "card" },
                    LineItems=new List<SessionLineItemOptions>(),
                    Mode="payment",
                    SuccessUrl=$"{HTTPReq}/api/User/Payment/Success/{payment.Id}",
                    CancelUrl=$"{HTTPReq}/api/User/Payment/cancel",
                };

                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData=new SessionLineItemPriceDataOptions
                    {
                        Currency="usd",
                        ProductData=new SessionLineItemPriceDataProductDataOptions
                        {
                            Name=Course.Title,
                            Description=Course.Description,
                            Images=new List<string> { $"{HTTPReq}/Images/Products/{Course.ImgeUrl}" }
                        },
                        UnitAmount=(long)(Course.Price*100),
                    },
                    Quantity=1
                });

                var service = new SessionService();
                var session = service.Create(options);
                payment.PaymentId=session.Id;
                await paymentRepo.UpdateAsync(payment);

                return new PaymentRes
                {
                    Success=true,
                    Message="Redirect to payment gateway",
                    PaymentId=session.Id,
                    Url=session.Url
                };
            }
            else
            {
                throw new Exception("Payment Method Not Supported");
            }
        }

        public async Task<int> DeleteAsync (int id)
        {
            var payment = await paymentRepo.GetByIdAsync(id);
            return payment==null ? throw new Exception("Payment Not Found") : await paymentRepo.DeleteAsync(payment);
        }
        public async Task<IEnumerable<Payment>> GetAllAsync ()
        {
            return await paymentRepo.GetAllAsync();
        }
        public async Task<Payment>? HandelSuccessAsync (int paymentId)
        {
            var payment = await paymentRepo.GetByIdAsync(paymentId);
            if (payment==null||payment.Status==PaymentStatus.Completed)
                return null;
            payment.Status=PaymentStatus.Completed;
            var enrollment = new Enrollment
            {
                CourseId=payment.CourseId,
                UserId=payment.UserId
            };
            await enrollments.AddAsync(enrollment);
            return await paymentRepo.GetByIdAsync(paymentId);
        }

    }
}
