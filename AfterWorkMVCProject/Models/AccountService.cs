using AfterWorkMVCProject.Models.Entities;
using AfterWorkMVCProject.Models.ViewModels.AfterWork;
using AfterWorkMVCProject.Models.ViewModels.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Models
{
    public class AccountService
    {
        UserManager<MyIdentityUser> userManager;
        SignInManager<MyIdentityUser> signInManager;
        RoleManager<IdentityRole> roleManager;
        IWebHostEnvironment webHostEnv;
        IHttpContextAccessor httpContextAccessor;
        MyContext context;

        string userId;

        public AccountService(
        UserManager<MyIdentityUser> userManager,
        SignInManager<MyIdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IWebHostEnvironment webHostEnv,
        IHttpContextAccessor httpContextAccessor,
        MyContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.webHostEnv = webHostEnv;
            this.context = context;
            userId = userManager.GetUserId(httpContextAccessor.HttpContext.User);

        }
        //TODO Lösa registreringsfunktionen. Blir felmeddelande nu.

        internal async Task<bool> CreateAspNetUserAsync(RegisterVM viewModel)
        {
            MyIdentityUser user;
            var result = await userManager.CreateAsync(user =
            new MyIdentityUser { UserName = viewModel.UserName, Email = viewModel.MailAddress }, viewModel.Password);

            context.UserInfos
                .Add(new UserInfo
                {
                    UserId = user.Id,
                    TotalPoints = 0,
                    GamesPlayed = 0,
                    ProfileImgPath = @"\Img\ProfilePlaceHolderImg2.jpg"
                });
            context.SaveChanges();

            return result.Succeeded;
        }

        public async Task<bool> LoginAsync(LoginVM viewModel)
        {
            // Todo: Try to sign user

            var result = await signInManager.PasswordSignInAsync(
                viewModel.UserName,
                viewModel.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            return result.Succeeded;
        }

        // TODO ändra vymodell ... när man registrerar sig så får man en avatar med en placeholder img,
        //för att sedan kunna ladda upp en egen bild.
        internal Task AddProfileImgAsync(RegisterVM viewModel)
        {

            if (viewModel.ImageToUpload != null)
            {
                string filePath = Path.Combine(webHostEnv.WebRootPath, "Uploads", viewModel.ImageToUpload.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    viewModel.ImageToUpload.CopyTo(fileStream);
                }
            }

            context.UserInfos
                .Add(new UserInfo
                {
                    //TotalPoints = viewModel.TotalPoints,
                    //GamesPlayed = viewModel.GamesPlayed,

                    ProfileImgPath = viewModel.ImageToUpload?.FileName //<img src=/Uploads/@Model.ImagePath>

                });
            context.SaveChanges();
            return Task.CompletedTask;

            //TODO Bestämma i vilken tabell Avatar (ImgPath) ska ligga (UserStats? - ev döpa om tabellen 
            // till UserInfo eller liknande. Och bestämma när man ska kunna lägga till sin Avatar.
        }


        internal async Task<ProfileInfoVM> GetProfileInfoVMAsync()
        {
            MyIdentityUser user = await userManager.FindByIdAsync(userId);
            var userInfo = context.UserInfos.Where(o => o.UserId == user.Id).FirstOrDefault();

            return new ProfileInfoVM
            {
                Name = user.UserName,
                GamesPlayed = userInfo.GamesPlayed,
                TotalPoints = userInfo.TotalPoints,
                ProfileImgPath = userInfo.ProfileImgPath
                //ProfileImgPath = userInfo.ProfileImgPath
            };
        }


        internal async Task<EditUserInfoVM> GetEditInfoVM()
        {
            MyIdentityUser user = await userManager.FindByIdAsync(userId);



            var result = new EditUserInfoVM
            {
                UserName = user.UserName,
                MailAddress = user.Email,

            };

            return result;
        }

        internal async Task EditUserInfoAsync(EditUserInfoVM viewModel)
        {
            MyIdentityUser user = await userManager.FindByIdAsync(userId);
            var userInfo = context.UserInfos.Where(o => o.UserId == user.Id).FirstOrDefault();
            //await userManager.ChangePasswordAsync(user, viewModel.Password, viewModel.PasswordRepeat);

            if (viewModel.ProfileImgPath != null)
            {
                string filePath = Path.Combine(webHostEnv.WebRootPath, "Uploads", viewModel.ProfileImgPath.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    viewModel.ProfileImgPath.CopyTo(fileStream);
                }
            }

            user.Email = viewModel.MailAddress;
            user.UserName = viewModel.UserName;
            if (viewModel.ProfileImgPath != null)
            {
                userInfo.ProfileImgPath = $@"\Uploads\{viewModel.ProfileImgPath?.FileName}";
            }
            context.UserInfos.Update(userInfo);

            await userManager.UpdateAsync(user);
            context.SaveChanges();

        }
    }
}
