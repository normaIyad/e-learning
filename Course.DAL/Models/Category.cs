namespace Course.DAL.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgeUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Course> Courses { get; set; }

    }
}
