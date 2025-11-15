namespace Course.DAL.DTO.Responce
{
    public class QuestionOptionIsSelectedRes
    {
        public int Id { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsSelected { get; set; }
    }
}
