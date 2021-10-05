using System;
using System.Collections.Generic;

#nullable disable

namespace AfterWorkMVCProject.Models.Entities
{
    public partial class UserStat
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? TotalPoints { get; set; }
        public int? GamesPlayed { get; set; }

        public virtual AspNetUser User { get; set; }
    }
}
