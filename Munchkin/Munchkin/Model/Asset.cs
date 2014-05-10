using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Munchkin.Model
{
    /// <summary>
    /// A model of an art asset.
    /// </summary>
    class Asset
    {
        // The image representing the menu
        private readonly Texture2D _assetTexture;

        // The position where the menu begins.
        public Vector2 StartPosition { get; private set; }

        public string Name { get; private set; }

        public Boolean IsActive { get; set; }

        public Rectangle clickableArea { get; private set; }

        /// <summary>
        /// Constructor for Asset
        /// </summary>
        /// <param name="assetTexture">The image of the asset</param>
        /// <param name="startCoord">The starting position</param>
        /// <param name="name">The name of the asset</param>
        public Asset(Texture2D assetTexture, Vector2 startCoord, String name, Boolean initialActiveState)
        {
            _assetTexture = assetTexture;
            StartPosition = startCoord;
            Name = name;
            IsActive = initialActiveState;
            clickableArea = new Rectangle((int)StartPosition.X, (int)StartPosition.Y, _assetTexture.Width, _assetTexture.Height);
        }

        /// <summary>
        /// Draws this asset to spriteBatch
        /// </summary>
        /// <param name="spriteBatch">The container to draw this asset onto</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_assetTexture,
            new Rectangle(
            (int)StartPosition.X,
            (int)StartPosition.Y,
            _assetTexture.Width, _assetTexture.Height),
            Color.White);
        }

        public Boolean IsClicked(MouseState last, MouseState current)
        {
            return (last.LeftButton == ButtonState.Pressed && current.LeftButton == ButtonState.Released
                    && this.IsActive && clickableArea.Contains(current.X, current.Y));
        }

        public Boolean IsHoverOver(MouseState point)
        {
            return (clickableArea.Contains(point.X, point.Y) && this.IsActive);
        }
    }
}
