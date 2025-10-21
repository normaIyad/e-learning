namespace Course.DAL.Utilityes
{
    public interface ISeedData
    {
        Task DataSeed ();
        Task SeedUsers ();
        Task SeedCourses ();
        Task SeedCategories ();
    }
}
