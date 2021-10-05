using System;
using System.Collections.Generic;

#nullable disable

namespace AfterWorkMVCProject.Models.Entities
{
    public partial class JoinedUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AwsessionId { get; set; }
        public int? Points { get; set; }

        public virtual Awsession Awsession { get; set; }
        public virtual AspNetUser User { get; set; }
    }
}
