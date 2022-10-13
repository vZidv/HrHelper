using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class Busyness
    {
        public Busyness()
        {
            Summaries = new HashSet<Summary>();
        }

        public int Id { get; set; }
        public string Type { get; set; } = null!;

        public virtual ICollection<Summary> Summaries { get; set; }
    }
}
