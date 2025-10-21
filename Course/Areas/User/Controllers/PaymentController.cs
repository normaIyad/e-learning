using Course.Bll.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Course.Pl.Areas.User.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("User")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentController (IPaymentService paymentService)
        {
            this.paymentService=paymentService;
        }
        [HttpPost("CheckOut")]
        public async Task<IActionResult> CheckOut ([FromBody] DAL.DTO.request.PaymentReq paymentReq)
        {
            var userId = User.FindFirstValue("Id");
            if (userId is not null)
            {
                var request = $"{Request.Scheme}://{Request.Host}";
                var result = await paymentService.AddAsync(paymentReq, userId, request);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            return Unauthorized();
        }
        [HttpGet("Success/{paymentId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Success (int paymentId)
        {
            var result = await paymentService.HandelSuccessAsync(paymentId);
            return result!=null ? Ok(result) : BadRequest("Payment not found or already completed.");
        }
        [HttpGet("Cansel")]
        public IActionResult cansel ()
        {
            return BadRequest("the opration canseld ");
        }

    }
}
