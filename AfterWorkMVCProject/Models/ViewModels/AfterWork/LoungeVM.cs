using AfterWorkMVCProject.Models.ClassesMVC;
using AfterWorkMVCProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Models.ViewModels.AfterWork
{
    public class LoungeVM
    {
        public string AWName { get; set; }
        public string UserName { get; set; }
        public string[] GameNames { get; set; }
        public string[] GameImgURLArray { get; set; }
        public string InviteURL { get; set; }
        public string Code { get; set; }

        public Player[] Players { get; set; }

        public Game[] Games { get; set; }

    }
}
