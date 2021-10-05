using System;
using System.Collections.Generic;

#nullable disable

namespace AfterWorkMVCProject.Models.Entities
{
    public partial class DigikaljaPlayer
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int DigikaljaId { get; set; }
        public int? Points { get; set; }

        public virtual Digikalja Digikalja { get; set; }
        public virtual AspNetUser User { get; set; }
    }
}
