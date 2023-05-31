using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class AuthorizationUser
    {
        public AuthorizationUser()
        {
            VacancyRequests = new HashSet<VacancyRequest>();
        }

        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int UserTypeId { get; set; }

        public virtual UserType UserType { get; set; } = null!;
        public virtual ICollection<VacancyRequest> VacancyRequests { get; set; }
    }
}
