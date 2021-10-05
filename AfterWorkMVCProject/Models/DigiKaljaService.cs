using AfterWorkMVCProject.Models.ClassesMVC;
using AfterWorkMVCProject.Models.ClassesMVC.DigiKalja;
using AfterWorkMVCProject.Models.Entities;
using AfterWorkMVCProject.Models.ViewModels.AfterWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AfterWorkMVCProject.Models.ViewModels.Components;

namespace AfterWorkMVCProject.Models
{
    
    public class DigiKaljaService
    {
        public List<string> users;
        MyContext context;
        UserManager<MyIdentityUser> userManager;
        IHttpContextAccessor accessor;
        string userId;
        //DigiKalja session;
        Random rnd = new Random();
        
        public DigiKaljaService(MyContext context, UserManager<MyIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            users = new List<string>();
            //userId = userManager.GetUserId(accessor.HttpContext.User);

        }

        public async Task<ScoreTableVM[]> GetScoreTableVMArray(string code)
        {

            var temp = context.Awsessions
                .Where(o => o.Code == code).FirstOrDefault();
            return context.JoinedUsers.Where(r => r.AwsessionId == temp.Id).Select(r => new ScoreTableVM
            {
                UserName = r.User.UserName,
                Score = r.Points == null ? 0 : (int)r.Points
            }).ToArray();

        }

        public void AddUserName(string user)
        {
            users.Add(user);
        }

        public async Task<DigiKaljaVM> GetDigiKaljaVM(string code)
        {
            var digi = context.Digikaljas.Where(r => r.Code == code).FirstOrDefault();

            //if(digi != null)
            //{
                var digiPlayers = context.DigikaljaPlayers.Where(r => r.DigikaljaId == digi.Id);

                return new DigiKaljaVM
                {
                    Players = digiPlayers.Select(r => new Player
                    {
                        ID = r.UserId,
                        UserName = r.User.UserName,
                        Score = 0
                    }).ToArray(),
                    Code = code,
                    Desc = new Description
                    {
                        TheWord = context.Words.Where(r => r.Id == digi.WordId ).Select(r=>r.Word1).First(),
                        Id = 200000,
                        WordDescription = context.Words.Where(r => r.Id == digi.WordId).Select(r => r.Description).First(),
                        Author = "Computer",
                        IsCorrect = true
                    }
                };
        }
    }
}
