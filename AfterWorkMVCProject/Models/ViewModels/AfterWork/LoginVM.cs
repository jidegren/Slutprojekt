using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Models.ViewModels.AfterWork
{
    public class LoginVM
    {
        [Required]
        [Display(Name = "Användarnamn")]

        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
