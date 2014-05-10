using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Munchkin
{
    class PlayingField
    {
        private List<Card> cards; //Cards to display on the playing field
        private Vector2 position; //Position of the field.


        /// <summary>
        /// Constructor. Instantiates cards and position of the field. 
        /// </summary>
        public PlayingField()
        {
            this.cards = new List<Card>();
            this.position = new Vector2(400, 555);
        }

        /// <summary>
        /// Adds a card to the list of cards for the playing field.
        /// </summary>
        /// <param name="c">Card to add</param>
        public void Add(Card c)
        {
            c.Commands = new List<String>() {"Add To Hand", "Discard", "Clear Playing Field"};
            this.cards.Add(c);
        }

        /// <summary>
        /// Clears the playing field of all cards. Adds them to appropriate discard pile
        /// </summary>
        /// <param name="doorDiscard">Door discard pile</param>
        /// <param name="treasureDiscard">Treasure discard pile</param>
        public void ClearPlayingField(Discard doorDiscard, Discard treasureDiscard)
        {
            foreach (Card c in cards)
            {
                if (c.Type.Equals("Door"))
                {
                    doorDiscard.Add(c);
                }
                if(c.Type.Equals("Treasure"))
                {
                    treasureDiscard.Add(c);
                }
            }
            cards.Clear();
        }

        /// <summary>
        /// Determines if playing field contains card of passed ID
        /// </summary>
        /// <param name="cardID">Card ID to check for</param>
        /// <returns>True iff the playingfield contains the card.</returns>
        public bool ContainsCard(int cardID)
        {
            foreach (Card c in cards)
            {
                if (c.ID == cardID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Retrieves the card from the playing field.
        /// </summary>
        /// <param name="cardID">ID of card to check.</param>
        /// <returns>Returns card</returns>
        public Card GetCard(int cardID)
        {
            foreach (Card c in cards)
            {
                if (c.ID == cardID)
                {
                    return c;
                }
            }
            return null;
        }

        /// <summary>
        /// Removes card from the card list.
        /// </summary>
        /// <param name="cardID">CardID of the card to check for</param>
        /// <returns>Card removed</returns>
        public Card RemoveCard(int cardID)
        {
            foreach (Card c in cards)
            {
                if (c.ID == cardID)
                {
                    cards.Remove(c);
                    return c;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the card clicked
        /// </summary>
        /// <param name="last">Last mouse state</param>
        /// <param name="current">Current mouse state.</param>
        /// <returns>The card clicked</returns>
        public Card ClickedACard(MouseState last, MouseState current)
        {
            foreach (Card c in cards)
            {
                if (c.IsClicked(last, current))
                {
                    return c;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Updates card position
        /// </summary>
        public void Update()
        {
            Vector2 currentPosition = position;
            foreach (Card c in cards)
            {
                c.Position = currentPosition;
                c.ClickableArea = new Microsoft.Xna.Framework.Rectangle((int)currentPosition.X, (int)currentPosition.Y, 80, 120);
                currentPosition.X += 80;
            }
        }

        /// <summary>
        /// Draw method for the Playing Field.
        /// </summary>
        /// <param name="sb">Spritebatch to draw with</param>
        public void Draw(SpriteBatch sb)
        {
            foreach (Card c in cards)
            {
                sb.Draw(c.MediumFrontImage, c.ClickableArea, Color.White);
            }
        }
    }
}
