using AfterWorkMVCProject.Models.ClassesMVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Models.ViewModels.AfterWork
{
    public class PlayerListBoxVM
    {
        public string Id { get; set; } //Är AW-session-ID:t... kanske lägga länken någon annanstans sedan?
        public string UserName { get; set; }
        public int? Score { get; set; }
        public string Code { get; set; }
        public string ProfileImgPath { get; set; }

        public List<Player> Players { get; set; }
    }
}
