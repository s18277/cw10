using Cw5.Models;

namespace Cw5.DAL
{
    public interface IDbStudentService : IDbService<Student, string>
    {
        public StartedStudies GetStartedStudies(string indexNumber);
    }
}