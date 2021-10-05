//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AfterWorkMVCProject.Models.ClassesMVC.DigiKalja
//{
//    public class DigiKalja
//    {
//        public string Id { get; set; }
//        public List<Player> Players { get; set; }
//        public List<Description> Descriptions { get; set; }
//        public int Round { get; set; }

//        bool exitGame;
//        public DigiKalja(string code)
//        {
//            Id = code;
//        }

//        Random random = new Random();
//        string word;

//        List<WordToGuess> WordsWithCorrectDescription = new List<WordToGuess>
//        {
//            new WordToGuess("Veritabel", "Någonting äkta/autentiskt."),
//            new WordToGuess("Superopti-mopsiskt-topp-i-pang-fenomenaliskt", "Ett sätt att mirakulöst prata sig ur svåra situationer och till och med ett sätt att ändra sitt liv på."),
//            new WordToGuess("Otoskop", "ett optiskt instrument som används för att titta i öronen med"),
//            new WordToGuess("Proximal interfalangealled", "Led i fingret mellan skelettdelen (ben) närmast handen och nästa ben.")
//        };

//        internal void StartGame()
//        {
//            CreateGameSession();

//            while (!exitGame)
//            {
//                GenerateWord();
//                DisplayWordAndInputDescription();
//                ChooseDescription();
//                PresentResult();

//                VoteForDescription(); //Ev anropa denna inifrån PresentResult? Ska ligga i if-sats?
//                CheckIfEndGame();
//            }
//            AddResultToDb();
//        }


//        private void CheckIfEndGame()
//        {
//            //Kolla om spelet ska avslutas.
//            throw new NotImplementedException();
//        }

//        private void CreateGameSession()
//        {
//            //Ladda in Players + poäng
//            throw new NotImplementedException();
//        }

//        private void GenerateWord()
//        {
//            //Generate random word + description from database/API (nu först från Lista)
//            var randomNumber = random.Next(0, WordsWithCorrectDescription.Count);
//            word = WordsWithCorrectDescription[randomNumber].Word;
//        }

//        private void ChooseDescription()
//        {
//            throw new NotImplementedException();
//        }
//        private void PresentResult()
//        {
//            throw new NotImplementedException();
//        }

//        private void DisplayWordAndInputDescription()
//        {
//            //Visa upp ordet på skärmen och därefter modalruta med input field att fylla i sin beskrivning
//            throw new NotImplementedException();
//        }

//        private void VoteForDescription()
//        {
//            //Jämför alla descriptions med den korrekta description
//            throw new NotImplementedException();
//        }

//        private void AddResultToDb()
//        {
//            throw new NotImplementedException();
//        }

//        public void AddPlayer(Player player){
//            Players.Add(player);
//        }
//    }

   
//}
