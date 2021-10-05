using AfterWorkMVCProject.Models;
using AfterWorkMVCProject.Models.ClassesMVC;
using AfterWorkMVCProject.Models.ClassesMVC.DigiKalja;
using AfterWorkMVCProject.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Hubs
{
    public class ChatHub : Hub
    {
        UserManager<MyIdentityUser> userManager;
        private static Dictionary<int, Description> descList = new Dictionary<int, Description>();
        string userId;
        IHttpContextAccessor accessor;
        MyContext context;
        Random rnd = new Random();
        private static Dictionary<int, Description> rightDesc = new Dictionary<int, Description>();
        private static List<Description> descriptionList = new List<Description>();
        private static List<Description> descriptionsToPrint = new List<Description>();

        public ChatHub(UserManager<MyIdentityUser> userManager, MyContext context, IHttpContextAccessor accessor)
        {
            this.userManager = userManager;
            userId = userManager.GetUserId(accessor.HttpContext.User);
            this.context = context;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

            //TODO Fixa så det inte blir samma chatt i alla olika rum.
        }

        public async Task JoinGroup(string group, string user)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            await JoinAWSession(group);
        }

        //SendMessageToGroup och NewUserJoined motsvarar varandra kan man
        //säga, då de båda skickas in som argument till connection.invoke
        //och i sina metoder här i chathub skickar in motsvarigheterna 
        //"ReceiveMessage" och "CreateUserElement" till Clients.Group(group).SendAsync();
        public async Task SendMessageToGroup(string group, string message, string user)
        {
            await Clients.Group(group).SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendDescriptionToGroup(string group, string description, string user)
        {

            MyIdentityUser aspNetUser = await userManager.FindByIdAsync(userId);
            var hiddenUserName = aspNetUser.UserName;

            var id = rnd.Next(0, 100001);
            CreateDescription(description, id, hiddenUserName);

            var temp = context.Awsessions
               .Where(o => o.Code == group).FirstOrDefault();

            var allPlayers = context.JoinedUsers.Where(r => r.AwsessionId == temp.Id).ToArray();

            if (descriptionList.Count == allPlayers.Length)
            {
                foreach (var item in descriptionList)
                {
                    descriptionsToPrint.Add(item);
                }

                var shuffled = descriptionsToPrint.OrderBy(x => Guid.NewGuid()).ToList();
                foreach (var item in shuffled)
                {
                    await Clients.Group(group).SendAsync("ReceiveDescription", item.Author, item.WordDescription, item.Author, item.Id);
                    //När detta är klart ska divWaiting tas bort
                }

                descriptionList.Clear();
                descriptionsToPrint.Clear();
            }


        }

        public async Task SendRightDescription(string group, string description, string user, int id)
        {

            var temp = context.Awsessions
   .Where(o => o.Code == group).FirstOrDefault();

            var allPlayers = context.JoinedUsers.Where(r => r.AwsessionId == temp.Id).ToArray();

            if (!rightDesc.ContainsKey(id))
            {
                rightDesc.Add(id, new Description
                {
                    Id = id,
                    WordDescription = description,
                    Author = "Computer",
                    IsCorrect = true,
                    NumberOfVotes = 2
                });

            }

            if ((descriptionList.Count + 1) == allPlayers.Length)
            {
                //await Clients.Group(group).SendAsync("ReceiveDescription", user, description, user, "200000");
                descriptionsToPrint.Add(rightDesc[id]);
                
            }

        }

        public async Task SetScoreById(string id, string group, string hiddenUserName, bool isComputer)
        {
            //var userScore = 0;
            if (!isComputer)
            {

                var idInt = Int32.Parse(id);
                descList[idInt].NumberOfVotes += 1;
                var score = descList[idInt].NumberOfVotes;
                var description = descList[idInt].WordDescription;
                var author = descList[idInt].Author;


                var temp = context.Awsessions.Where(o => o.Code == group).FirstOrDefault();
                var user = context.JoinedUsers.Where(r => r.AwsessionId == temp.Id).Where(r => r.User.UserName == hiddenUserName).First();
                var userScore = user.Points == null ? 0 : (int)user.Points;

                await Clients.Group(group).SendAsync("GetScoreById", score, author, description, idInt, userScore);

            }
            else
            {
                var idInt = Int32.Parse(id);
                //rightDesc[idInt].NumberOfVotes += 1;
                var score = rightDesc[idInt].NumberOfVotes;
                var description = rightDesc[idInt].WordDescription;
                var author = rightDesc[idInt].Author;

                MyIdentityUser clicker = await userManager.FindByIdAsync(userId);
                var clickerName = clicker.UserName;

                var temp = context.Awsessions.Where(o => o.Code == group).FirstOrDefault();
                var user = context.JoinedUsers.Where(r => r.AwsessionId == temp.Id).Where(r => r.User.UserName == clickerName).First();
                var userScore = user.Points == null ? 0 : (int)user.Points;

                rightDesc.Add(user.Id, new Description
                {
                    Author = clicker.UserName,
                    NumberOfVotes = 2
                }) ;

                var realScore = rightDesc[user.Id].NumberOfVotes;
                await Clients.Group(group).SendAsync("GetScoreById", realScore, clickerName, description, idInt, userScore);
                //user.Points = userScore;
                //await context.SaveChangesAsync();
                //Lagt till dom 2 övre raderna, kontrollera varför det inte fungerar
            }

        }

        public async Task AddPointsToDB(string group)
        {
            //var digId = context.Digikaljas.Where(r => r.Code == group).Select(r => r.Id).First();
            //var listOfPlayers = context.DigikaljaPlayers.Where(r => r.DigikaljaId == digId).Select(r => r);
            //         var temp = context.Awsessions
            //.Where(o => o.Code == group).FirstOrDefault();
            //         var listOfPlayers = context.JoinedUsers.Where(r => r.AwsessionId == temp.Id).ToArray();

            var awID = await context.Awsessions.Where(r => r.Code == group).FirstAsync();

            var listOfPlayers = await context.JoinedUsers.Where(r => r.Awsession.Id == awID.Id).ToArrayAsync();

            foreach (var item in listOfPlayers)
            {
                var user = await context.UserInfos.Where(r => r.UserId == item.UserId).FirstAsync();//.Where(r => r.User.UserName == item.User.UserName).First();

                if (user.TotalPoints == null)
                {
                    user.TotalPoints = item.Points;
                    await context.SaveChangesAsync();
                }
                else
                {
                    user.TotalPoints += item.Points;
                    await context.SaveChangesAsync();
                }
            }
            await Clients.Group(group).SendAsync("EndGame");
        }

        public async Task JoinDigikaljaGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            //TODO Lägga till info på Clients när någon joinar (se "JoinGroup") ??
        }

        public async Task NewUserJoined(string group)
        {
            await JoinDigikaljaGroup(group);


            MyIdentityUser user = await userManager.FindByIdAsync(userId);

            var userName = user.UserName;
            //await Clients.Group(group).SendAsync("CreateUserElement", userName, group);
            await Clients.Group(group).SendAsync("ReceiveMessage", userName, $" I will kick your butt!");
            await JoinDigiKalja(group);

        }

        public async Task JoinDigiKalja(string group)
        {

            
            MyIdentityUser user = await userManager.FindByIdAsync(userId);
            //await Clients.All.SendAsync("ReceiveMessage", user.UserName, "Jag spelar DigiKalja!");

            //var listOfDKAWSessionID = context.Digikaljas.Select(r => r.Id);


            var digId = context.Digikaljas.Where(r => r.Code == group).Select(r => r.Id).First();
            var listOfPlayers = context.DigikaljaPlayers.Where(r => r.DigikaljaId == digId).Select(r => r.UserId);

            if (!listOfPlayers.Contains(user.Id))
            {
                context.DigikaljaPlayers.Add(new DigikaljaPlayer
                {
                    UserId = user.Id,
                    DigikaljaId = context.Digikaljas.Where(r => r.Code == group).Select(r => r.Id).First(),
                });
                context.SaveChanges();
            }

        }

        public void CreateDescription(string description, int id, string user)
        {
            Description desc = new Description
            {
                WordDescription = description,
                Id = id,
                NumberOfVotes = 0,
                Author = user
            };
            descList.Add(id, desc);
            descriptionList.Add(desc);
        }

        //public void GivePointsToDescription(int points, int id, string code)
        //{
        //    //var idInt = Int32.Parse(id);
        //    descList[id].NumberOfVotes += points;
        //    //GivePointsToUser(code);
        //}

        public void GivePointsToUser(string code)
        {
            rightDesc.Remove(200000);

            var temp = context.Awsessions.Where(o => o.Code == code).FirstOrDefault();

            foreach (var item in descList)
            {
                var author = context.JoinedUsers.Where(r => r.AwsessionId == temp.Id).Where(r => r.User.UserName == item.Value.Author).First();
                if (author.Points == null)
                {
                    author.Points = item.Value.NumberOfVotes;
                    context.SaveChanges();
                }
                else
                {
                    author.Points += item.Value.NumberOfVotes;
                    context.SaveChanges();
                }

            }

            foreach (var item in rightDesc)
            {
                var author = context.JoinedUsers.Where(r => r.AwsessionId == temp.Id).Where(r => r.User.UserName == item.Value.Author).First();
                if (author.Points == null)
                {
                    author.Points = item.Value.NumberOfVotes;
                    context.SaveChanges();
                }
                else
                {
                    author.Points += item.Value.NumberOfVotes;
                    context.SaveChanges();
                }

            }

            descList.Clear();
            rightDesc.Clear();
            
        }

        public async Task GivePointsToUserInvoke(string code)
        {
            rightDesc.Remove(200000);

            var temp = context.Awsessions.Where(o => o.Code == code).FirstOrDefault();

            foreach (var item in descList)
            {
                var author = context.JoinedUsers.Where(r => r.AwsessionId == temp.Id).Where(r => r.User.UserName == item.Value.Author).First();
                if (author.Points == null)
                {
                    author.Points = item.Value.NumberOfVotes;
                    context.SaveChanges();
                }
                else
                {
                    author.Points += item.Value.NumberOfVotes;
                    context.SaveChanges();
                }

            }

            foreach (var item in rightDesc)
            {
                var author = context.JoinedUsers.Where(r => r.AwsessionId == temp.Id).Where(r => r.User.UserName == item.Value.Author).First();
                if (author.Points == null)
                {
                    author.Points = item.Value.NumberOfVotes;
                    context.SaveChanges();
                }
                else
                {
                    author.Points += item.Value.NumberOfVotes;
                    context.SaveChanges();
                }

            }

            descList.Clear();
            rightDesc.Clear();

            await Clients.Group(code).SendAsync("AddPointsToDBInvoke");
        }

        public async Task StartNewRound(string code)
        {
            GivePointsToUser(code);
            ChangeWord(code);

            await Clients.Group(code).SendAsync("StartNewGame");
        }

        public void ChangeWord(string code)
        {
            var digikalja = context.Digikaljas.Where(r => r.Code == code).First();
            var Words = context.Words.Select(r => r).ToArray();
            int num = rnd.Next(0, Words.Length);
            var word = Words[num];
            digikalja.Word = word;
            context.SaveChanges();
        }

        public async Task JoinAWSession(string code)
        {
            MyIdentityUser user = await userManager.FindByIdAsync(userId);
            var AWSession = context.Awsessions.Where(a => a.Code == code).Single();
            var list = context.JoinedUsers.Where(r => r.AwsessionId == AWSession.Id).Select(r => r.UserId);

            if (!list.Contains(user.Id))
            {
                JoinedUser join = new JoinedUser
                {
                    UserId = user.Id,
                    AwsessionId = AWSession.Id,
                };
                context.JoinedUsers.Add(join);
                context.SaveChanges();
                await Clients.Group(code).SendAsync("AddToAWList", user.UserName);
                await Clients.Group(code).SendAsync("ReceiveMessage", user.UserName, $" I'm here!");

            }

            var digiKaljas = context.Digikaljas.Select(r => r.Code);
            var Words = context.Words.Select(r => r).ToArray();
            int num = rnd.Next(0, Words.Length);
            var word = Words[num];

            if (!digiKaljas.Contains(code))
            {
                context.Digikaljas.Add(new Digikalja
                {
                    AwsessionId = context.JoinedUsers.Where(r => r.UserId == user.Id).Select(r => r.AwsessionId).First(),
                    Code = code,
                    WordId = word.Id

                });
                context.SaveChanges();

            }

            
        }

    }
}
