using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Munchkin
{
    /// <summary>
    /// Model class of the background object.
    /// </summary>
    class Background
    {
        // The image representing the background
        Texture2D BGTexture;

        // Number of columns of the background image
        int HorizontalTiles;

        // Number of rows of the background image
        int VerticalTiles;

        // The position where the background begins.
        Vector2 StartCoord;

        /// <summary>
        /// Draws the background image in a tiled pattern to the spriteBatch
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch object being drawn on.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < HorizontalTiles; i++)
            {
                for (int j = 0; j < VerticalTiles; j++)
                {
                    spriteBatch.Draw(BGTexture,
                    new Rectangle(
                    (int)StartCoord.X + (i * BGTexture.Width),
                    (int)StartCoord.Y + (j * BGTexture.Height),
                    BGTexture.Width, BGTexture.Height),
                    Color.White);
                }
            }
        }
        
        /// <summary>
        /// Initializes the background object
        /// </summary>
        /// <param name="texture">The texture of the background image</param>
        /// <param name="position">The starting position of the background image</param>
        /// <param name="environmentWidth">The width of the environment</param>
        /// <param name="environmentHeight">The height of the environment</param>
        public void Initialize(Texture2D texture, Vector2 position, int environmentWidth, int environmentHeight)
        {
            BGTexture = texture;
            HorizontalTiles = (int)(Math.Round((double)environmentWidth / texture.Width) + 1);
            VerticalTiles = (int)(Math.Round((double)environmentHeight / texture.Height) + 1);

            StartCoord = position;
        }
    }
}
