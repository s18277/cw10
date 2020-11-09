using Cw4.Models;

namespace Cw4.DAL
{
    public interface IDbStudentService : IDbService<Student, string>
    {
        public StartedStudies GetStartedStudies(string indexNumber);
    }
}