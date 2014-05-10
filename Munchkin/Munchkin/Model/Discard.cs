using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Munchkin
{
    class Discard
    {
        private List<Card> cards;
        private Vector2 _position;
        private Rectangle _clickableArea;
        private bool selected;
        private List<String> commands; 

        public Discard(Vector2 position)
        {
            cards = new List<Card>();
            _position = position;
            _clickableArea = new Rectangle((int)position.X, (int)position.Y, 80, 120);
            commands = new List<string>(){"Add To Hand"};
        }

        //Adds the card to the discard list. 
        public void Add(Card card)
        {
            cards.Add(card);
        }
        //The commands for the discard pile
        public List<String> Commands
        {
            get { return commands; }
            set { commands = value; }
        }
        //List of cards for the discard pi.e
        public List<Card> Cards
        {
            get { return cards; }
            set { cards = value; }
        }
        //The selected card
        public bool Selected
        {
            get { return selected;}
            set { selected = value; }
        }
        /// <summary>
        /// Resets the list of cards in discard pile.
        /// </summary>
        /// <returns>The reset discard list</returns>
        public List<Card> Reset()
        {
            List<Card> old = cards;
            cards = new List<Card>();
            return old;
        }
        
        /// <summary>
        /// Removes the top card from the discard pile.
        /// </summary>
        /// <returns>Card removed.</returns>
        public Card TakeCard()
        {
            if (cards.Count > 0)
            {
                Card c = cards[cards.Count -1];
                cards.Remove(c);
                return c;
            }
            return null;

        }
        
        /// <summary>
        /// Determines if discard pile contains the card
        /// </summary>
        /// <param name="cardID">ID of card to check for</param>
        /// <returns>True iff the discard pile contains the card</returns>
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
        /// Removes a specific card from the discard pile. 
        /// </summary>
        /// <param name="cardID">ID of card to be removed. </param>
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
        /// Determines if the discard pile was clicked
        /// </summary>
        /// <param name="last">Last mouse state</param>
        /// <param name="current">Current mouse state</param>
        /// <returns>True iff the discard pile was clicked. </returns>
        public bool IsClicked(MouseState last, MouseState current)
        {
            if (last.LeftButton == ButtonState.Pressed && current.LeftButton == ButtonState.Released)
            {
                if (_clickableArea.Contains(current.X, current.Y))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Draw method for the discard pile
        /// </summary>
        /// <param name="sb">SpriteBatch to draw the discard pile</param>
        public void Draw(SpriteBatch sb)
        {
            if (cards.Count > 0)
            {
                sb.Draw(cards[cards.Count - 1].MediumFrontImage, _position, Color.White);
            }
        }
    }
}
