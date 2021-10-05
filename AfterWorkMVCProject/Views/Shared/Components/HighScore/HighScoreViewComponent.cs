using AfterWorkMVCProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Views.Shared.Components.HighScore
{
    public class HighScoreViewComponent : ViewComponent
    {
        AfterWorkService service;
        public HighScoreViewComponent(AfterWorkService service)
        {
            this.service = service;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModels = await service.GetHighScoreVMArray();
            return View(viewModels);
        }
    }
}
