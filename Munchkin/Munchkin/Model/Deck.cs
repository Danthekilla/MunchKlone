using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;

namespace Munchkin
{
    /// <summary>
    /// Model class for a deck object.
    /// </summary>
    class Deck
    {
        //Declaration of instance variables
        private List<Card> cards;
        private Texture2D coverImage;
        private Microsoft.Xna.Framework.Rectangle clickableArea;
        public byte[] cardStream;//represent the card as a byte stream array to be transmitted over network to host.
        private List<String> commands;
        private bool selected;

        /// <summary>
        /// Constructor of a deck object
        /// </summary>
        /// <param name="_cards">A list of cards that will make up the deck</param>
        /// <param name="_coverImage">The cover image of the deck</param>
        /// <param name="_clickableArea">The clickable are of the deck</param>
        public Deck(List<Card> _cards, Texture2D _coverImage, Microsoft.Xna.Framework.Rectangle _clickableArea)
        {
            cards = _cards;
            coverImage = _coverImage;
            clickableArea = _clickableArea;
        }

        //Selected property
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }
        //Commands property
        public List<String> Commands
        {
            get
            {
                return commands;
            }
            set
            {
                commands = value;
            }
        }
        //Cards property
        public List<Card> Cards
        {
            get
            {
                return cards;
            }
            set
            {
                cards = value;
            }
        }
        //CoverImage property
        public Texture2D CoverImage
        {
            get
            {
                return coverImage;
            }
            set
            {
                coverImage = value;
            }
        }
        //ClickableArea property
        public Microsoft.Xna.Framework.Rectangle ClickableArea
        {
            get
            {
                return clickableArea;
            }
            set
            {
                clickableArea = value;
            }
        }
        /// <summary>
        /// Returns and removes the top card from the deck.
        /// </summary>
        /// <returns>The top card of the deck.</returns>
        public Card DrawCard()
        {
                Card c = cards[0];
                cards.Remove(c);
                return c;
        }
        /// <summary>
        /// Adds the given card to the bottom of the deck.
        /// </summary>
        /// <param name="c">The card to add to the deck.</param>
        public void Add(Card c)
        {
            cards.Add(c);
        }

        /// <summary>
        /// Shuffles the given discard pile into the deck.
        /// </summary>
        /// <param name="pile">The discard pile to shuffle into the deck.</param>
        public void Repopulate(Discard pile)
        {
            cards = pile.Reset();
            Shuffle();
        }

        /// <summary>
        /// Randomizes the order of the cards in the deck
        /// </summary>
        public void Shuffle()
        {
            Random rng = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        /// <summary>
        /// Returns if the deck contains the given card id
        /// </summary>
        /// <param name="cardID">The id of the card to find</param>
        /// <returns>Wether the card exists in the deck or not</returns>
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
        /// Removes the given card from the deck and returns it.
        /// </summary>
        /// <param name="cardID">The card to remove.</param>
        /// <returns>The card that was removed.</returns>
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
        /// Retrieves the image of a card as a blob and stores
        /// it as a jpeg
        /// </summary>
        /// <param name="index">The index of the card image to retrieve</param>
        /// <returns>The card image</returns>
        public byte[] StreamImage(int index)
        {
            Card card = cards[index];
            Texture2D image = card.MediumFrontImage;
            MemoryStream stream = new MemoryStream();
            image.SaveAsJpeg(stream, image.Width, image.Height);
            cardStream = stream.ToArray();
            return cardStream;
        }

        /// <summary>
        /// Event listener for if the deck is clicked.
        /// </summary>
        /// <param name="last">The last state of the Mouse</param>
        /// <param name="current">The current state of the Mouse</param>
        /// <returns>If the deck was clicked.</returns>
        public bool IsClicked(MouseState last, MouseState current)
        {
            if (last.LeftButton == ButtonState.Pressed && current.LeftButton == ButtonState.Released)
            {
                if(clickableArea.Contains(current.X, current.Y))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a string representation of the deck
        /// </summary>
        /// <returns>A string representation of the deck</returns>
        public String toString()
        {
            String returnString = "";
            for (int i = 0; i < cards.Count; i++)
            {
                if (i == (cards.Count - 1))
                {
                    returnString += (cards[i].ID.ToString());
                }
                else
                {
                    returnString += (cards[i].ID.ToString() + ",");
                }
            }
            return returnString;
        }

    }

}
