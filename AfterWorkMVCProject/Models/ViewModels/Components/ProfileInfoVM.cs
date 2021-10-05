using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Models.ViewModels.Components
{
    public class ProfileInfoVM
    {
        public string Name { get; set; }
        public int? TotalPoints { get; set; }
        public int? GamesPlayed { get; set; }
        public string ProfileImgPath { get; set; }
        [Display(Name = "Image to upload")]
        public IFormFile Image { get; set; }


    }
}
