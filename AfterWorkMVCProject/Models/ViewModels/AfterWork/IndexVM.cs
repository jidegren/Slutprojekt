using AfterWorkMVCProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Models.ViewModels.AfterWork
{
    public class IndexVM
    {
       // public GameCarouselItem[] GameCarouselItemArray { get; set; }//Url till bilder
                                                                     //public string[] TopFiveGallimatias { get; set; } //Namn på användarkonton
        public string GameName { get; set; }
        public string GameImgURL { get; set; }
        public string GameImgAltText { get; set; }
        public string Description { get; set; }

        public string Code { get; set; }


    }

}
