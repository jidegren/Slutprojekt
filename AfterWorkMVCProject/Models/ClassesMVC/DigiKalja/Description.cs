using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AfterWorkMVCProject.Models.ClassesMVC.DigiKalja
{
    public class Description
    {
        public int Id { get; set; }
        public string WordDescription { get; set; } //Förklaringen av ordet
        public string TheWord { get; set; }
        public string Author { get; set; }
        public bool IsCorrect { get; set; }
        public int NumberOfVotes { get; set; }

        //TODO bestämma sig för koppling till vem som skrivit description
    }
}
