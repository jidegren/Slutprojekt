using System;
using System.Collections.Generic;

#nullable disable

namespace AfterWorkMVCProject.Models.Entities
{
    public partial class Word
    {
        public Word()
        {
            Digikaljas = new HashSet<Digikalja>();
        }

        public int Id { get; set; }
        public string Word1 { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Digikalja> Digikaljas { get; set; }
    }
}
