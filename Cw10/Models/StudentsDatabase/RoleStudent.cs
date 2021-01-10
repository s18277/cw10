#nullable disable

namespace Cw10.Models.StudentsDatabase
{
    public class RoleStudent
    {
        public int IdRole { get; set; }
        public int IdStudent { get; set; }

        public virtual Role IdRoleNavigation { get; set; }
        public virtual Student IdStudentNavigation { get; set; }
    }
}