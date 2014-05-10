using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Net;

namespace Munchkin.Control
{
    /// <summary>
    /// Controller for board updates.
    /// </summary>
    class BoardUpdater
    {
        /// <summary>
        /// Updates the local decks to match the decks that were received by
        /// the given packet reader.
        /// </summary>
        /// <param name="masterDeck">A reference list of all possible cards.</param>
        /// <param name="gd">The drawing device.</param>
        /// <param name="packetReader">The network packet reader.</param>
        /// <returns></returns>
        public static List<Deck> UpdateLocalDecks(Deck masterDeck, GraphicsDevice gd, PacketReader packetReader)
        {
            List<Deck> returnList = new List<Deck>();
            Texture2D doorTexture;
            using (FileStream fileStream = new FileStream(@"..\..\..\..\MunchkinContent\Cards\Door.png", FileMode.Open))
            {
                doorTexture = Texture2D.FromStream(gd, fileStream);
            }

            Texture2D treasureTexture;
            using (FileStream fileStream = new FileStream(@"..\..\..\..\MunchkinContent\Cards\Treasure.png", FileMode.Open))
            {
                treasureTexture = Texture2D.FromStream(gd, fileStream);
            }

            Microsoft.Xna.Framework.Rectangle doorRect = new Microsoft.Xna.Framework.Rectangle(585, 350, 80, 120);
            Microsoft.Xna.Framework.Rectangle treasureRect = new Microsoft.Xna.Framework.Rectangle(855, 350, 80, 120);

            String[] command1 = packetReader.ReadString().Split(',');
            String[] command2 = packetReader.ReadString().Split(',');

            List<Card> cards1 = new List<Card>();
            List<Card> cards2 = new List<Card>();

            for (int i = 0; i < command1.Length; i++)
            {
                for (int j = 0; j < masterDeck.Cards.Count; j++)
                {
                    if (masterDeck.Cards[j].ID == Convert.ToInt32(command1[i]))
                    {
                        cards1.Add(masterDeck.Cards[j]);
                    }
                }
            }

            for (int i = 0; i < command2.Length; i++)
            {
                for (int j = 0; j < masterDeck.Cards.Count; j++)
                {
                    if (masterDeck.Cards[j].ID == Convert.ToInt32(command2[i]))
                    {
                        cards2.Add(masterDeck.Cards[j]);
                    }
                }
            }

            Deck doorDeck = new Deck(cards1, doorTexture, doorRect);
            doorDeck.Commands = new List<String>() { "Draw Face Up", "Add To Hand", "Discard", "Add To Playing Field" };
            Deck treasureDeck = new Deck(cards2, treasureTexture, treasureRect);
            treasureDeck.Commands = new List<String>() { "Draw Face Up", "Add To Hand", "Discard", "Add To Treasure Pool" };
            returnList.Add(doorDeck);
            returnList.Add(treasureDeck);

            return returnList;
        }

        /// <summary>
        /// Event handler for after a player action is recieved. 
        /// Updates the board state to reflect the change.
        /// </summary>
        /// <param name="command">The command received</param>
        /// <param name="doorDeck">The state of the doorDeck</param>
        /// <param name="treasureDeck">The state of the treasureDeck</param>
        /// <param name="masterDeck">The reference list of all possible cards.</param>
        /// <param name="doorDiscard">The state of the doorDeck discard pile.</param>
        /// <param name="treasureDiscard">The state of the treasureDeck discard pile.</param>
        /// <param name="players">A list of all players playing the game</param>
        /// <param name="player">The player that sent the action</param>
        /// <param name="pf">The state of the playing field.</param>
        public static void UpdateBoardAfterPlayerAction(String command, Deck doorDeck, Deck treasureDeck, Deck masterDeck, Discard doorDiscard, Discard treasureDiscard, List<Player> players, Player player, PlayingField pf)
        {
            String[] commandargs = command.Split(',');
            String senderTag = commandargs[0];
            Card c = FindCard(Convert.ToInt32(commandargs[1]), doorDeck, treasureDeck, doorDiscard, treasureDiscard, players, commandargs[0], commandargs[2], pf);
            
                switch (commandargs[2])
            {
                case "Add To Hand":
                    foreach (Player p in players)
                    {
                        if (p.Gamertag.Equals(senderTag))
                        {
                            if (c != null)
                            {
                                p.AddCardToHand(c);
                            }
                        }
                    }
                    break;
                case "Draw Face Up":
                    if (c != null)
                    {
                        pf.Add(c);
                    }
                    break;
                case "Discard":
                    if (c != null)
                    {
                        if (c.Type.Equals("Door"))
                        {
                            doorDiscard.Add(c);
                        }
                        else if (c.Type.Equals("Treasure"))
                        {
                            treasureDiscard.Add(c);
                        }
                    }
                    break;
                case "Add To Playing Field":
                    if (c != null)
                    {
                        pf.Add(c);
                    }
                    break;
                case "Clear Playing Field":
                    pf.ClearPlayingField(doorDiscard, treasureDiscard);
                    break;
                case "Add To BackPack":
                    foreach (Player p in players)
                    {
                        if (p.Gamertag.Equals(senderTag))
                        {
                            if (c != null)
                            {
                                p.AddCardToBackPack(c);
                            }
                        }
                    }
                    break;
                case "Add To Equipped":
                    foreach (Player p in players)
                    {
                        if (p.Gamertag.Equals(senderTag))
                        {
                            if (c != null)
                            {
                                p.AddCardToEquipped(c);
                            }
                        }
                    }
                    break;

            }
        }

        /// <summary>
        /// Because each card is unique, this locates and returns a card from anywhere in
        /// the game.
        /// </summary>
        /// <param name="cardID">The card being searched for</param>
        /// <param name="doorDeck">The state of the doorDeck</param>
        /// <param name="treasureDeck">The state of the treasureDeck</param>
        /// <param name="doorDiscard">The state of the doorDeck discard pile.</param>
        /// <param name="treasureDiscard">The state of the treasureDeck discard pile.</param>
        /// <param name="players">List of all players in the game.</param>
        /// <param name="senderGamertag">The gamertag of the player that is searching for the card</param>
        /// <param name="command">The command that resulted in needing to find the card</param>
        /// <param name="pf">The state of the playing field.</param>
        /// <returns>The requested card.</returns>
        private static Card FindCard(int cardID, Deck doorDeck, Deck treasureDeck, Discard doorDiscard, Discard treasureDiscard, List<Player> players, String senderGamertag, String command, PlayingField pf)
        {
            foreach (Player p in players)
            {
                    if (p.HasCard(cardID))
                    {
                        return p.RemoveCard(cardID);
                    }
            }
            if (doorDeck.ContainsCard(cardID))
            {
                return doorDeck.RemoveCard(cardID);
            }
            if (treasureDeck.ContainsCard(cardID))
            {
                return treasureDeck.RemoveCard(cardID);
            }
            if(pf.ContainsCard(cardID))
            {
                if(command.Equals("Add To Hand"))
                {
                    Card c = pf.RemoveCard(cardID);
                    pf.ClearPlayingField(doorDiscard, treasureDiscard);
                    return c;
                }
                else
                {
                   return pf.GetCard(cardID); 
                }
                
            }
            if(doorDiscard.ContainsCard(cardID))
            {
                return doorDiscard.RemoveCard(cardID);
            }
            if(treasureDiscard.ContainsCard(cardID))
            {
                return treasureDiscard.RemoveCard(cardID);
            }
            return null;
        }

        /// <summary>
        /// Retrieves the player that is sending a command by
        /// using the gamertag to identify them.
        /// </summary>
        /// <param name="players">List of all players in the game.</param>
        /// <param name="gamertag">The gamertag of the player sending the command.</param>
        /// <returns>The player who is sending a command.</returns>
        private static Player GetSendingPlayer(List<Player> players, String gamertag)
        {
            foreach (Player theplayer in players)
            {
                if (theplayer.Gamertag.Equals(gamertag))
                {
                    return theplayer;
                }
            }
            return null;
        }

        /// <summary>
        /// Updates the state of the level of the player who
        /// sent the levelup command.
        /// </summary>
        /// <param name="players">A list of all players</param>
        /// <returns>Returns the gamertag who leveled up</returns>
        public static String UpdateLevels(List<Player> players )
        {
            String gamertag = CommunicationController.packetReader.ReadString();
            String level = CommunicationController.packetReader.ReadString();

            foreach (Player p in players)
            {
                if(p.Gamertag.Equals(gamertag))
                {
                    p.Level = Convert.ToInt32(level);
                }
            }
            return gamertag;
        }
    }
}
