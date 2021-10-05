using AfterWorkMVCProject.Models;
using AfterWorkMVCProject.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Views.Shared.Components.ScoreTable
{
    public class ScoreTableViewComponent : ViewComponent
    {
        DigiKaljaService service;

        public ScoreTableViewComponent(DigiKaljaService service)
        {
            this.service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync(string code)
        {
            return View(await service.GetScoreTableVMArray(code));
            //throw new NotImplementedException();
        }



    }
}
