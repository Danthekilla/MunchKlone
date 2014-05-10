using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Munchkin.Model
{
    class GameOverStatSet
    {
        private List<PlayerStat> stats; //List of player stats.
        
        /// <summary>
        /// Constructor. Instantiates stats list. 
        /// </summary>
        public GameOverStatSet()
        {
            stats = new List<PlayerStat>();
        }

        /// <summary>
        /// Adds stats for the player to the stats list. 
        /// </summary>
        /// <param name="p">Player to add stats for</param>
        /// <param name="statistics">List of statistics for the player</param>
        public void AddStat(Player p, List<String> statistics)
        {
            PlayerStat ps = new PlayerStat();
            ps.Gamertag = statistics[0];
            ps.TotalLevel = Convert.ToInt32(statistics[1]) + p.Level;
            if(p.Level == 10)
            {
                ps.GamesWon = Convert.ToInt32(statistics[2]) + 1;
                ps.GamesLost = Convert.ToInt32(statistics[3]);
            }
            else
            {
                ps.GamesWon = Convert.ToInt32(statistics[2]);
                ps.GamesLost = Convert.ToInt32(statistics[3]) + 1;
            }
            stats.Add(ps);
        }

        /// <summary>
        /// Saves the stats for the player
        /// </summary>
        /// <param name="p">The player to save the stats for</param>
        public void SaveStats(Player p)
        {
            PlayerStat target = new PlayerStat();
            foreach (PlayerStat ps in stats)
            {
                if(ps.Gamertag.Equals(p.Gamertag))
                {
                    target = ps;
                }
            }
            DatabaseController.SavePlayerStats(target.Gamertag, target.TotalLevel, target.GamesWon, target.GamesLost);
        }

        /// <summary>
        /// Draw method for displaying player stats on the Game Over screen
        /// </summary>
        /// <param name="sb">Spritebatch to draw with.</param>
        /// <param name="sf">Spritefont to use to draw text.</param>
        public void Draw(SpriteBatch sb, SpriteFont sf)
        {
            Vector2 position = new Vector2(100, 100);
            foreach (var playerStat in stats)
            {
                sb.DrawString(sf, (playerStat.Gamertag + ": "), position, Color.White);
                position.Y += 40;
                sb.DrawString(sf, ("      Total Level: " + playerStat.TotalLevel), position, Color.White);
                position.Y += 40;
                sb.DrawString(sf, ("      Games Won: " + playerStat.GamesWon), position, Color.White);
                position.Y += 40;
                sb.DrawString(sf, ("      Games Lost: " + playerStat.GamesLost), position, Color.White);
                position.Y += 70;
            }
        }
    }
}
