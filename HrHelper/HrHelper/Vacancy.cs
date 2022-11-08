using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class Vacancy
    {
        public Vacancy()
        {
            SummaryForVacancies = new HashSet<SummaryForVacancy>();
        }

        public int Id { get; set; }
        public string JobTitle { get; set; } = null!;

        public virtual ICollection<SummaryForVacancy> SummaryForVacancies { get; set; }
    }
}
