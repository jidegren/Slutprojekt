using AfterWorkMVCProject.Models.ClassesMVC;
using AfterWorkMVCProject.Models.Entities;
using AfterWorkMVCProject.Models.ViewModels.AfterWork;
using AfterWorkMVCProject.Models.ViewModels.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Models
{
    public class AfterWorkService
    {
        UserManager<MyIdentityUser> userManager;
        MyContext context;
        IHttpContextAccessor accessor;
        string userId;

        public AfterWorkService(MyContext context, UserManager<MyIdentityUser> userManager,
            IHttpContextAccessor accessor)
        {
            this.context = context;
            this.userManager = userManager;
            userId = userManager.GetUserId(accessor.HttpContext.User);
        }



        public IndexVM[] GetIndexVMArray()
        {

            return context.Games.Select(u => new IndexVM
            {
                GameName = u.Name,
                GameImgURL = u.ImgUrl,
                Description = u.Description,
                GameImgAltText = u.AltImgText

            }).ToArray();
        }


        public async Task<HighScoreVM[]> GetHighScoreVMArray()
        {

            return context.UserInfos.OrderByDescending(p => p.TotalPoints).Take(10).Select(p => new HighScoreVM
            {
                Name = p.User.UserName,
                TotalPoints = (int)p.TotalPoints
            }).ToArray();


        }

        internal async Task<Awsession> CreateAWSessionAsync(CreateAWVM viewModel)
        {
            MyIdentityUser user = await userManager.FindByIdAsync(userId);

            var token = "";
            const string availableChars =
                "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            using (var generator = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[10];
                generator.GetBytes(bytes);
                var chars = bytes
                    .Select(b => availableChars[b % availableChars.Length]);
                token = new string(chars.ToArray());
            }

            var AW = new Awsession
            {
                Awname = viewModel.AWName,
                Code = token,
                CreatorId = user.Id
            };
            context.Awsessions.Add(AW);
            context.SaveChanges();
            JoinedUser join = new JoinedUser
            {
                UserId = user.Id,
                AwsessionId = AW.Id,
            };
            context.JoinedUsers.Add(join);
            context.SaveChanges();



            return AW;

            

        }


        internal async Task<LoungeVM> GetLoungeVM(string code)
        {
            MyIdentityUser user = await userManager.FindByIdAsync(userId);

            var temp = context.Awsessions
                .Where(o => o.Code == code).FirstOrDefault();

            var games = context.Games.Select(u => new Game
            {
                Name = u.Name,
                ImgUrl = u.ImgUrl,
                Description = u.Description,
                AltImgText = u.AltImgText

            }).ToArray();

            return new LoungeVM
            {
                Games = games,
                AWName = temp.Awname,
                Code = temp.Code,
                UserName = user.UserName,
                Players = context.JoinedUsers.Where(r => r.AwsessionId == temp.Id).Select(r => new Player
                {
                    ID = r.User.Id,
                    UserName = r.User.UserName,
                    Score = r.Points == null ? 0 : (int)r.Points

                }).ToArray(),
            };


        }


    }
}
