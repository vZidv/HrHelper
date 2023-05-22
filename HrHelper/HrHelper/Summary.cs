using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class Summary
    {
        public Summary()
        {
            SummaryForVacancies = new HashSet<SummaryForVacancy>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Gender { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public int? ContactsId { get; set; }
        public string? Address { get; set; }
        public string? Town { get; set; }
        public int? BusynessId { get; set; }
        public int? PhotoId { get; set; }
        public string? Comments { get; set; }
        public int StatusId { get; set; }
        public string? LastCompany { get; set; }
        public string? LastJobTitle { get; set; }
        public int? EducationId { get; set; }
        public string? EducationInstution { get; set; }
        public string? AboutYourself { get; set; }

        public virtual Busyness? Busyness { get; set; }
        public virtual SummaryContact? Contacts { get; set; }
        public virtual Education? Education { get; set; }
        public virtual Photo? Photo { get; set; }
        public virtual SummaryStatus Status { get; set; } = null!;
        public virtual ICollection<SummaryForVacancy> SummaryForVacancies { get; set; }
    }
}
