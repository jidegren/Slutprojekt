using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Models.ViewModels.AfterWork
{
    public class CreateAWVM
    {
        [Display(Name ="Namnge din AW")]
        [Required(ErrorMessage = "Du måste fylla i ett namn på din AW")]
        public string AWName { get; set; }

        [Display(Name = "Skriv in dina vänners email-adresser, separerade med komma [,]")]
        [EmailAddress(ErrorMessage = "Du måste fylla i en giltig E-postadress!")]
        public string[] GuestEmails { get; set; }
    }
}
