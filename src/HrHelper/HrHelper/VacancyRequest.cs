using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class VacancyRequest
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = null!;
        public string? Description { get; set; }
        public string? Skills { get; set; }
        public int BusynessId { get; set; }
        public int UserId { get; set; }
        public string Department { get; set; } = null!;

        public virtual Busyness Busyness { get; set; } = null!;
        public virtual AuthorizationUser User { get; set; } = null!;
    }
}
