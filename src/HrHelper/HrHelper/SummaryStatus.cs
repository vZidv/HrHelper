using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class SummaryStatus
    {
        public SummaryStatus()
        {
            Summaries = new HashSet<Summary>();
        }

        public int Id { get; set; }
        public string Status { get; set; } = null!;

        public virtual ICollection<Summary> Summaries { get; set; }
    }
}
