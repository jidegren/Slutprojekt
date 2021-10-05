using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Models.ClassesMVC.DigiKalja
{
    public class WordToGuess
    {
        public string Word { get; set; }
        public string CorrectDescription { get; set; }

        public WordToGuess(string word, string correctDescription)
        {
            Word = word;
            CorrectDescription = correctDescription;
        }

    }
}
