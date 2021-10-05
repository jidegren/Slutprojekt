using AfterWorkMVCProject.Models.ViewModels.AfterWork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Models.ViewModels.Components
{
    public class EditUserInfoVM
    {
        [Required(ErrorMessage = "Du måste ange ett användarnamn!")]
        [Display(Name = "Användarnamn")]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Du måste ange ett lösenord!")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Lösenord")]
        //public string Password { get; set; }

        //[Display(Name = "Bekräfta lösenord")]
        //[Required(ErrorMessage = "Du måste bekräfta ditt lösenord!")]
        //[Compare(nameof(RegisterVM.Password))]
        //[DataType(DataType.Password)]
        //public string PasswordRepeat { get; set; }

        [Display(Name = "E-postadress")]
        [EmailAddress(ErrorMessage = "Du måste fylla i en giltig E-postadress!")]
        public string MailAddress { get; set; }

        [Display(Name = "Här kan du ladda upp en avatar")]
        public IFormFile ProfileImgPath { get; set; }
        
    }
}
