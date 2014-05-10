using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Munchkin
{
    /// <summary>
    /// A model of a player of the game.
    /// </summary>
    class Player
    {
        private Vector2 position; //represent player position.
        //public bool active; //State of the player

        private List<Card> hand;
        private List<Card> equipped;
        private List<Card> backpack;
        private String location;
        private String gamertag;
        private int level;

        /// <summary>
        /// Initializes the state of the player.
        /// </summary>
        /// <param name="_position">The position of the player</param>
        /// <param name="_location">The location of the player</param>
        public void Initialize(Vector2 _position, String _location)
        {
            position = _position;
            location = _location;
            //active = true;
            hand = new List<Card>();
            equipped = new List<Card>();
            backpack = new List<Card>();
        }

        /// <summary>
        /// Checks to see if the player possesses the given card.
        /// </summary>
        /// <param name="cardID">The card to find.</param>
        /// <returns>If the player possesses the card.</returns>
        public bool HasCard(int cardID)
        {
            foreach (Card c in hand)
            {
                if (c.ID == cardID)
                {
                    return true;
                }
            }
            foreach (Card c in backpack)
            {
                if (c.ID == cardID)
                {
                    return true;
                }
            }
            foreach (Card c in equipped)
            {
                if (c.ID == cardID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes the given card from the players possession
        /// </summary>
        /// <param name="cardID">The card to remove</param>
        /// <returns>The card that was removed.</returns>
        public Card RemoveCard(int cardID)
        {
            foreach (Card c in hand)
            {
                if (c.ID == cardID)
                {
                    hand.Remove(c);
                    return c;
                }
            }
            foreach (Card c in backpack)
            {
                if (c.ID == cardID)
                {
                    backpack.Remove(c);
                    return c;
                }
            }
            foreach (Card c in equipped)
            {
                if (c.ID == cardID)
                {
                    equipped.Remove(c);
                    return c;
                }
            }
            return null;
        }

        /// <summary>
        /// Adds the given card to the player's hand.
        /// </summary>
        /// <param name="c">The card to add.</param>
        public void AddCardToHand(Card c)
        {
            if(equipped.Contains(c))
            {
                equipped.Remove(c);
            }
            if(backpack.Contains(c))
            {

                backpack.Remove(c);
            }
            c.Commands = new List<String>() {"Add To BackPack", "Add To Equipped", "Add To Playing Field", "Discard"};
            hand.Add(c);
            
        }

        /// <summary>
        /// Adds the given card to the player's backpack.
        /// </summary>
        /// <param name="c">The card to add.</param>
        public void AddCardToBackPack(Card c)
        {
            if(hand.Contains(c))
            {
                hand.Remove(c);
            }
            if(equipped.Contains(c))
            {
                equipped.Remove(c);
            }
            c.Commands = new List<String>() {"Add To Hand", "Add To Equipped", "Add To Playing Field", "Discard"};
            backpack.Add(c);
            
        }

        /// <summary>
        /// Adds the given card to the player's equipment.
        /// </summary>
        /// <param name="c">The card to add.</param>
        public void AddCardToEquipped(Card c)
        {
            if(hand.Contains(c))
            {
                hand.Remove(c);
            }
            if(backpack.Contains(c))
            {
                backpack.Remove(c);
            }
            c.Commands = new List<String>() {"Add To Hand", "Add To BackPack", "Add To Playing Field", "Discard"};
            equipped.Add(c);
            
        }

        //Level property
        public int Level
        {
            get 
            { 
                return level;
            }
            set
            {
                level = value;
            }
        }

        //Hand property
        public List<Card> Hand
        {
            get
            {
                return hand;
            }
        }

        //Equipped Property
        public List<Card> Equipped
        {
            get
            {
                return equipped;
            }
        }

        //Backpack Property
        public List<Card> Backpack
        {
            get
            {
                return backpack;
            }
        }

        //Position property
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        //Gamertag Property
        public String Gamertag
        {
            get
            {
                return gamertag;
            }
            set
            {
                gamertag = value;
            }
        }

        /// <summary>
        /// Updates the state of the player.
        /// </summary>
        public void Update()
        {
            Vector2 handPosition = position;
            Vector2 equipedPosition = handPosition;
            equipedPosition.Y += 70;
            Vector2 backPackPosition = equipedPosition;
            backPackPosition.Y += 70;
            handPosition.X += 80;
            equipedPosition.X += 80;
            backPackPosition.X += 80;
            foreach (Card c in hand)
            {
                c.Position = handPosition;
                c.ClickableArea = new Microsoft.Xna.Framework.Rectangle((int)handPosition.X, (int)handPosition.Y, 40, 60);
                handPosition.X += 50;
            }
            foreach (Card c in equipped)
            {
                c.Position = equipedPosition;
                c.ClickableArea = new Microsoft.Xna.Framework.Rectangle((int)equipedPosition.X, (int)equipedPosition.Y, 40, 60);
                equipedPosition.X += 50;
            }
            foreach (Card c in backpack)
            {
                c.Position = backPackPosition;
                c.ClickableArea = new Microsoft.Xna.Framework.Rectangle((int)backPackPosition.X, (int)backPackPosition.Y, 40, 60);
                backPackPosition.X += 50;
            }
        }

        /// <summary>
        /// Event listener for if the player clicks a card.
        /// </summary>
        /// <param name="last">The last state of Mouse</param>
        /// <param name="current">The current state of Mouse</param>
        /// <returns>The card that was clicked.</returns>
        public Card ClickedACard(MouseState last, MouseState current)
        {
            foreach (Card c in backpack)
            {
                if (c.IsClicked(last, current))
                {
                    return c;
                }
            }
            foreach (Card c in equipped)
            {
                if (c.IsClicked(last, current))
                {
                    return c;
                }
            }
            foreach (Card c in hand)
            {
                if (c.IsClicked(last, current))
                {
                    return c;
                }
            }
            return null;
        }

        /// <summary>
        /// Draws the players assets to the the spritebatch
        /// </summary>
        /// <param name="spriteBatch">The spritebatch object being drawn on</param>
        /// <param name="spriteFont">The spriteFont to use to render the player text.</param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.DrawString(spriteFont, this.gamertag, new Vector2(position.X, position.Y - 20), Color.White );
            spriteBatch.DrawString(spriteFont, ("Level: "+this.level.ToString()), new Vector2(position.X +100, position.Y -20), Color.White);
            spriteBatch.DrawString(spriteFont, "Hand: ", position, Color.White);
            spriteBatch.DrawString(spriteFont, "Equipped: ", new Vector2(position.X, position.Y + 70), Color.White);
            spriteBatch.DrawString(spriteFont, "Backpack: ", new Vector2(position.X, position.Y + 140), Color.White);
            foreach (Card c in hand)
            {
                if (location.Equals("bottom"))
                {
                    spriteBatch.Draw(c.SmallFrontImage, c.Position, Color.White);
                }
                else
                {
                    spriteBatch.Draw(c.SmallBackImage, c.Position, Color.White);
                }
            }
            foreach (Card c in backpack)
            {
                spriteBatch.Draw(c.SmallFrontImage, c.Position, Color.White);
            }
            foreach (Card c in equipped)
            {
                spriteBatch.Draw(c.SmallFrontImage, c.Position, Color.White);
            }
        }

        /// <summary>
        /// Event handler for when the player leaves a game.
        /// </summary>
        /// <param name="doorDiscard">The state of the doorDeck discard pile.</param>
        /// <param name="treasureDiscard">The state of the treasureDeck discard pile.</param>
        internal void Left(Discard doorDiscard, Discard treasureDiscard)
        {
            foreach (Card c in hand)
            {
                if (c.Type.Equals("Door"))
                {
                    doorDiscard.Add(c);
                }
                if (c.Type.Equals("Treasure"))
                {
                    treasureDiscard.Add(c);
                }
            }
            foreach (Card c in backpack)
            {
                if (c.Type.Equals("Door"))
                {
                    doorDiscard.Add(c);
                }
                if (c.Type.Equals("Treasure"))
                {
                    treasureDiscard.Add(c);
                }
            }
            foreach (Card c in equipped)
            {
                if (c.Type.Equals("Door"))
                {
                    doorDiscard.Add(c);
                }
                if (c.Type.Equals("Treasure"))
                {
                    treasureDiscard.Add(c);
                }
            }
        }


    }
}
