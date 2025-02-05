using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class Gender
    {
        public Gender()
        {
            Summaries = new HashSet<Summary>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Summary> Summaries { get; set; }
    }
}
