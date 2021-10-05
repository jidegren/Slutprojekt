using AfterWorkMVCProject.Models.ClassesMVC.DigiKalja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Models.ClassesMVC
{
    public class Player
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }
        public Description PlayersDescription { get; set; }

    }
}
