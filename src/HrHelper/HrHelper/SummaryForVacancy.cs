using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class SummaryForVacancy
    {
        public int Id { get; set; }
        public int VacancyId { get; set; }
        public int SummaryId { get; set; }

        public virtual Summary Summary { get; set; } = null!;
        public virtual Vacancy Vacancy { get; set; } = null!;
    }
}
