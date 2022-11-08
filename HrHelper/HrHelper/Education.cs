using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class Education
    {
        public Education()
        {
            Summaries = new HashSet<Summary>();
        }

        public int Id { get; set; }
        public string EducationName { get; set; } = null!;

        public virtual ICollection<Summary> Summaries { get; set; }
    }
}
