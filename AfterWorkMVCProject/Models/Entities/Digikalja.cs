using System;
using System.Collections.Generic;

#nullable disable

namespace AfterWorkMVCProject.Models.Entities
{
    public partial class Digikalja
    {
        public Digikalja()
        {
            DigikaljaPlayers = new HashSet<DigikaljaPlayer>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public int AwsessionId { get; set; }
        public int? WordId { get; set; }

        public virtual Awsession Awsession { get; set; }
        public virtual Word Word { get; set; }
        public virtual ICollection<DigikaljaPlayer> DigikaljaPlayers { get; set; }
    }
}
