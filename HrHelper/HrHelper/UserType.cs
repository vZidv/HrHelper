using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class UserType
    {
        public UserType()
        {
            AuthorizationUsers = new HashSet<AuthorizationUser>();
        }

        public int Id { get; set; }
        public string Type { get; set; } = null!;

        public virtual ICollection<AuthorizationUser> AuthorizationUsers { get; set; }
    }
}
