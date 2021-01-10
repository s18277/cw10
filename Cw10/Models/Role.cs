using System.Collections.Generic;

#nullable disable

namespace Cw10.Models
{
    public class Role
    {
        public Role()
        {
            RoleStudents = new HashSet<RoleStudent>();
        }

        public int IdRole { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<RoleStudent> RoleStudents { get; set; }
    }
}