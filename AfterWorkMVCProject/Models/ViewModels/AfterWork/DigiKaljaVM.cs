using AfterWorkMVCProject.Models.ClassesMVC;
using AfterWorkMVCProject.Models.ClassesMVC.DigiKalja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Models.ViewModels.AfterWork
{
    public class DigiKaljaVM
    {
        public Player[] Players { get; set; }
        public Random DescriptionNumber { get; set; }
        //public string Players { get; set; }

        //public DigiKalja session { get; set; }
        public string Code { get; set; }
        public Description Desc { get; set; }
    }
}
