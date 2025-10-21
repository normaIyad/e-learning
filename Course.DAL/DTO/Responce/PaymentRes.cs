namespace Course.DAL.DTO.Responce
{
    public class PaymentRes
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? PaymentId { get; set; }
        public string? Url { get; set; }
    }
}
