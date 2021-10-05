using System;
using System.Collections.Generic;

#nullable disable

namespace AfterWorkMVCProject.Models.Entities
{
    public partial class Awsession
    {
        public Awsession()
        {
            Digikaljas = new HashSet<Digikalja>();
            JoinedUsers = new HashSet<JoinedUser>();
        }

        public int Id { get; set; }
        public string Awname { get; set; }
        public string Code { get; set; }
        public string CreatorId { get; set; }

        public virtual AspNetUser Creator { get; set; }
        public virtual ICollection<Digikalja> Digikaljas { get; set; }
        public virtual ICollection<JoinedUser> JoinedUsers { get; set; }
    }
}
