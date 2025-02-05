using System;
using System.Collections.Generic;

namespace HrHelper
{
    public partial class Photo
    {
        public Photo()
        {
            Summaries = new HashSet<Summary>();
        }

        public int Id { get; set; }
        public string Path { get; set; } = null!;

        public virtual ICollection<Summary> Summaries { get; set; }
    }
}
