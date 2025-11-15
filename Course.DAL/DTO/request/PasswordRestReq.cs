namespace Course.DAL.DTO.request
{
    public class PasswordRestReq
    {
        public string Email { get; set; }
        public string PasswordRestCode { get; set; }
        public string NewPassword { get; set; }
    }
}
