using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Data;
using System.Drawing;
using System.IO;

namespace Munchkin
{
    class CardController
    {
        /// <summary>
        /// Creates the List of decks for the master deck. 
        /// </summary>
        /// <param name="gd">graphics device</param>
        /// <returns>Master Deck</returns>
        public static List<Deck> CreateDeck(GraphicsDevice gd)
        {
            List<Card> allCards = CreateCards(gd);
            List<Card> door = new List<Card>();
            List<Card> treasure = new List<Card>();
            List<Deck> returnDecks = new List<Deck>();

            foreach (Card c in allCards)
            {
                if (c.Type.Equals("Door"))
                {
                    
                    door.Add(c);
                }
                else
                {
                    
                    treasure.Add(c);
                }
            }

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

            Microsoft.Xna.Framework.Rectangle doorRect = new Microsoft.Xna.Framework.Rectangle(585,350,80,120);
            Microsoft.Xna.Framework.Rectangle treasureRect = new Microsoft.Xna.Framework.Rectangle(855,350,80,120);

            Deck doorDeck = new Deck(door, doorTexture, doorRect);
            Deck treasureDeck = new Deck(treasure, treasureTexture, treasureRect);
            Deck masterDeck = new Deck(allCards, doorTexture, doorRect);

            List<String> doorCommands = new List<String>() { "Draw Face Up", "Add To Hand", "Discard"};
            List<String> treasureCommands = new List<String>() { "Add To Hand", "Discard", "Draw Face Up"};

            doorDeck.Commands = doorCommands;
            treasureDeck.Commands = treasureCommands;

            returnDecks.Add(doorDeck);
            returnDecks.Add(treasureDeck);
            returnDecks.Add(masterDeck);

            return returnDecks;
        }

        /// <summary>
        /// Creates a list of card objects.
        /// </summary>
        /// <param name="gd">Graphics device</param>
        /// <returns>Card list.</returns>
        private static List<Card> CreateCards(GraphicsDevice gd)
        {
            List<Card> cards = new List<Card>();
            DataSet ds = DatabaseController.GetCards();
            
            Image doorTexture;
            using (FileStream fileStream = new FileStream(@"..\..\..\..\MunchkinContent\Cards\Door.png", FileMode.Open))
            {
                doorTexture = Image.FromStream(fileStream);
            }

            Image treasureTexture;
            using (FileStream fileStream = new FileStream(@"..\..\..\..\MunchkinContent\Cards\Treasure.png", FileMode.Open))
            {
                treasureTexture = Image.FromStream(fileStream);
            }

            int c = ds.Tables["Cards"].Rows.Count;
            int index = 0;
            while (index < c)
            {
                Card card = new Card();
                Byte[] imageData = new Byte[0];
                imageData = (Byte[])(ds.Tables["Cards"].Rows[index]["image"]);
                MemoryStream stream = new MemoryStream(imageData);
                Image cImage = Image.FromStream(stream);
                stream.Flush();

                card.SmallFrontImage = FormatImage(cImage, gd, 40, 60);
                card.MediumFrontImage = FormatImage(cImage, gd, 80, 120);
                card.LargeFrontImage = FormatImage(cImage, gd, 250, 335);
                card.ID = Convert.ToInt32(ds.Tables["Cards"].Rows[index]["cID"]);
                card.Name = ds.Tables["Cards"].Rows[index]["cardName"].ToString();
                card.Description = ds.Tables["Cards"].Rows[index]["description"].ToString();
                card.Type = ds.Tables["Cards"].Rows[index]["cardType"].ToString();
                if (card.Type.Equals("Door"))
                    card.SmallBackImage = FormatImage(doorTexture, gd, 40, 60);
                if (card.Type.Equals("Treasure"))
                    card.SmallBackImage = FormatImage(treasureTexture, gd, 40, 60);
                cards.Add(card);
                index++;
            }
            return cards;
        }

        /// <summary>
        /// Formats image of the card objects
        /// </summary>
        /// <param name="cImage">card image</param>
        /// <param name="gd">graphics device</param>
        /// <param name="width">width to format to</param>
        /// <param name="height">height to format to</param>
        /// <returns>New texture of the card.</returns>
        public static Texture2D FormatImage(Image cImage, GraphicsDevice gd, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            Graphics gr = Graphics.FromImage((Image)b);
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            gr.DrawImage(cImage, 0, 0, width, height);
            gr.Dispose();

            Texture2D tx = null;
            using (MemoryStream s = new MemoryStream())
            {
                b.Save(s, System.Drawing.Imaging.ImageFormat.Png);
                s.Seek(0, SeekOrigin.Begin); //must do this, or error is thrown in next line
                tx = Texture2D.FromStream(gd, s);

            }
            return tx;
        }

    }
}
