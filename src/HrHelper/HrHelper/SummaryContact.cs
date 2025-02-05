using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class SummaryContact
    {
        public SummaryContact()
        {
            Summaries = new HashSet<Summary>();
        }

        public int Id { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? OtherContacts { get; set; }

        public virtual ICollection<Summary> Summaries { get; set; }
    }
}
