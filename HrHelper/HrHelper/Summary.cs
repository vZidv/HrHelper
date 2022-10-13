using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HrHelper
{
    public partial class Summary
    {
        Summary()
        {
         
        }
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Gender { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public int? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Town { get; set; }
        public string? Specialization { get; set; }
        public string? JobTitle { get; set; }
        public int? BusynessId { get; set; }
        public string? Education { get; set; }
        public int? PhotoId { get; set; }
        public string? Comments { get; set; }

        public virtual Busyness? Busyness { get; set; }
        public virtual Photo? Photo { get; set; }
    }
}
