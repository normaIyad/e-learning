namespace Course.DAL.DTO.request
{
    public class ChangePasswordReq
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
