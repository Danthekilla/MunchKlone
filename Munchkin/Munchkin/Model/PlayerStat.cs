using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Munchkin.Model
{
    class PlayerStat
    {
        public String Gamertag { get; set; } //Player Gamertag
        public int TotalLevel { get; set; } //Total Level earned by the player
        public int GamesWon { get; set; } //Games won by the player
        public int GamesLost { get; set; } //Games lost by the player. 
    }
}
