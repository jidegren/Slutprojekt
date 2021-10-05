using AfterWorkMVCProject.Models;
using AfterWorkMVCProject.Models.ViewModels.AfterWork;
using AfterWorkMVCProject.Models.ViewModels.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Controllers
{
    public class AfterWorkController : Controller
    {
        AfterWorkService afterWorkService;
        AccountService accountService;
        SignInManager<MyIdentityUser> signInManager;
        DigiKaljaService digiKaljaService;

        public AfterWorkController(AfterWorkService afterWorkService, AccountService accountService, SignInManager<MyIdentityUser> signInManager, DigiKaljaService digiKaljaService)
        {
            this.afterWorkService = afterWorkService;
            this.accountService = accountService;
            this.signInManager = signInManager;
            this.digiKaljaService = digiKaljaService;
        }


        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            var viewModel = afterWorkService.GetIndexVMArray();//Vad ska den heta?
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public IActionResult Connect(IndexVM[] ViewModels)
        {
            
            //var code = ViewModel[0].Code;
            return RedirectToAction(nameof(ConnectToAW), new { code = ViewModels[0].Code });
        }

        [Authorize]
        [HttpPost]
        //[Route("lounge/{code}")]
        public IActionResult ConnectFromDigi(string code)
        {

            //var code = ViewModel[0].Code;
            return RedirectToAction(nameof(ConnectToAW), new { code = code });
        }

        [Route("register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterVM viewModel)
        {

            if (!ModelState.IsValid)
                return View(viewModel);

            // Try to register user
            var success = await accountService.CreateAspNetUserAsync(viewModel);

            if (!success)
            {
                // Show error
                ModelState.AddModelError(string.Empty, "Misslyckades att skapa användare");
                return View(viewModel);
            }


            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginVM { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            // Check if credentials is valid (and set auth cookie)
            var success = await accountService.LoginAsync(viewModel);
            if (!success)
            {
                // Show error
                ModelState.AddModelError(nameof(LoginVM.UserName), "Inloggning misslyckades");
                return View(viewModel);
            }

            // Redirect user
            if (string.IsNullOrWhiteSpace(viewModel.ReturnUrl))
                return RedirectToAction(nameof(Index));
            else
                return Redirect(viewModel.ReturnUrl);
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [Route("loggedin")]
        public IActionResult LoggedIn()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        [Route("createaw")]
        public IActionResult CreateAW()
        {
            return View();
        }

        //TODO Crashar om man inte skriver in namn på AW

        [Authorize]
        [HttpPost]
        [Route("createaw")]
        public async Task<IActionResult> CreateAWSuccessAsync(CreateAWVM viewModel)
        {
            var result = await afterWorkService.CreateAWSessionAsync(viewModel);

            return RedirectToAction(nameof(Lounge),new {code = result.Code});

            // Nästa steg att ta vidare code från awsession så att den kodsnutten redirectar till rätt lounge.
        }


        [Authorize]
        [HttpGet]
        [Route("lounge/{code}")]
        public async Task<IActionResult> Lounge(string code)
        {
            var user = User.Identity.Name;
            digiKaljaService.AddUserName(user);
            var result = await afterWorkService.GetLoungeVM(code);
            //TODO Skicka in info till vyn för att bygga modal i View.
            return View(result);
        }

        [Authorize]
        [HttpPost]
        [Route("lounge/{code}")]
        public async Task<IActionResult> Lounge(LoungeVM viewModel)
        {
            var code = viewModel.Code;
            //var ViewModel = await digiKaljaService.CreateDigiKaljaVM(code);
            //TODO Skicka in info till vyn för att bygga modal i View.
            return RedirectToAction(nameof(Digikalja), new { code = code });
        }



        //[HttpGet]
        //[Route("addtoplayers")]
        //public IActionResult AddToPlayers(string code)
        //{
        //    //digiKaljaService.CreateNewDigiKalja(/*code*/);
        //    return RedirectToAction(nameof(Digikalja)/*, new { code }*/);
        //}


        //[Route("digikalja")]

        //public IActionResult DigiKalja()
        //{
        //    return View();
        //}

        [Authorize]
        [HttpGet]
        [Route("digikalja/{code}")]
        public async Task<IActionResult> Digikalja(string code)
        {
            var user = User.Identity.Name;

            var ViewModel = await digiKaljaService.GetDigiKaljaVM(code);
            return View(ViewModel);
        }

        [Route("digikalja/{code}")]
        public IActionResult ScoreTable(string id)
        {
            var viewModel = digiKaljaService.GetScoreTableVMArray(id);
            return PartialView("ScoreTable", viewModel);
        }


        [Authorize]
        [HttpPost]
        [Route("digikalja/{code}")]
        public async Task<IActionResult> Digikalja(DigiKaljaVM viewModel)
        {
            var user = User.Identity.Name;
            var code = viewModel.Code;

            return RedirectToAction(nameof(Digikalja), new { code = code });

            
        }

        [Authorize]
        [Route("OIAsCvhkmS")]
        public async Task<IActionResult> ConnectToAW(string code)
        {
            //await afterWorkService.JoinAWSession(code);
            //var ViewModel = await afterWorkService.GetLoungeVM("c0c3dKIokq");

            return RedirectToAction(nameof(Lounge), new { code = code });
        }



        //[Route("player-data/{code}")]
        //public IActionResult ProductData(string code, int awSessionId)
        //{
        //    var model = digiKaljaService.GetAllConnectedUsersAsync(code, awSessionId);
        //    return Json(model);
        //}
        [Authorize]
        [HttpGet]
        [Route("edit")]
        public async Task<IActionResult> EditAsync()
        {
            var result = await accountService.GetEditInfoVM();
            return View(result);
            
        }

        [Authorize]
        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> EditAsync(EditUserInfoVM viewModel)
        {

            if (!ModelState.IsValid)
                return View(viewModel);

            await accountService.EditUserInfoAsync(viewModel);

            return RedirectToAction(nameof(Index));

            //TODO Göra färdigt Action för att ta hand om postad data.
        }
    }
}
