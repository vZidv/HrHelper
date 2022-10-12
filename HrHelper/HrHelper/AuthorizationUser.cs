using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class AuthorizationUser
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Type { get; set; }
    }
}
