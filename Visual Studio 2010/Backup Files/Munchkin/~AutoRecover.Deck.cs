using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Munchkin
{
    class Deck
    {
        private List<Card> cards;

        public Deck(List<Card> _cards)
        {
            this.cards = _cards;
            Shuffle();
        }

        public void Add(Card card)
        {
            cards.Add(card);
        }

        public Card Draw()
        {
            Card card = cards[0];
            cards.Remove(card);
            return card;
        }

        public void Shuffle()
        {
            Random rand = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                Card card = cards[k];
                cards[k] = cards[n];
                cards[n] = card;
            }  
        }
    }
}
