﻿using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class Busyness
    {
        public Busyness()
        {
            Summaries = new HashSet<Summary>();
            Vacancies = new HashSet<Vacancy>();
            VacancyRequests = new HashSet<VacancyRequest>();
        }

        public int Id { get; set; }
        public string Type { get; set; } = null!;

        public virtual ICollection<Summary> Summaries { get; set; }
        public virtual ICollection<Vacancy> Vacancies { get; set; }
        public virtual ICollection<VacancyRequest> VacancyRequests { get; set; }
    }
}
