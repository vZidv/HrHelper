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
        public string? Description { get; set; }
        public string? Skills { get; set; }
        public int BusynessId { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }

        public virtual Busyness Busyness { get; set; } = null!;
        public virtual ICollection<SummaryForVacancy> SummaryForVacancies { get; set; }
    }
}
