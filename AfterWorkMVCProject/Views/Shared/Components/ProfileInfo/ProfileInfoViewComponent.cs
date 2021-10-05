using AfterWorkMVCProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Views.Shared.Components.ProfileInfo
{
    public class ProfileInfoViewComponent : ViewComponent
    {
        readonly AccountService service;

        public ProfileInfoViewComponent(AccountService service)
        {
            this.service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await service.GetProfileInfoVMAsync());
            //throw new NotImplementedException();
        }

    }
}
