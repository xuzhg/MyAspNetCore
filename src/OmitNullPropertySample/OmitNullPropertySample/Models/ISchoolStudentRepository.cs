namespace OmitNullPropertySample.Models
{
    public interface ISchoolStudentRepository
    {
        IList<School> Schools { get; }

        IList<Student> Students { get; }
    }
}
