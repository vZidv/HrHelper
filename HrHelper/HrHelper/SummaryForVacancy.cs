using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class SummaryForVacancy
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int SummaryId { get; set; }

        public virtual Vacancy Job { get; set; } = null!;
        public virtual Summary Summary { get; set; } = null!;
    }
}
