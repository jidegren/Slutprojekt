using System;
using System.Collections.Generic;

#nullable disable

namespace AfterWorkMVCProject.Models.Entities
{
    public partial class Game
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? MinPlayers { get; set; }
        public int? MaxPlayers { get; set; }
        public string AltImgText { get; set; }
    }
}
