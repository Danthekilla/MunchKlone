using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Munchkin.Model;

namespace Munchkin
{
    /// <summary>
    /// Model class for a menu object.
    /// </summary>
    class Menu
    {
        private List<Asset> _assets;

        public Vector2 StartPosition { get; private set; }

        private Texture2D _texture;

        /// <summary>
        /// Draws the menu to the spriteBatch object.
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch object being drawn to.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture,
            new Rectangle(
            (int)StartPosition.X,
            (int)StartPosition.Y,
            _texture.Width, _texture.Height),
            Color.White);

            if(_assets.Count > 0)
                foreach(Asset next in _assets)
                {
                    if(next.IsActive)
                        next.Draw(spriteBatch);
                }
        }

        /// <summary>
        /// Initializes the Menu object
        /// </summary>
        /// <param name="texture">The texture of the menu image.</param>
        /// <param name="position">The starting position of the menu</param>
        public void Initialize(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            StartPosition = position;
            _assets = new List<Asset>();
        }

        /// <summary>
        /// Indexer for finding assets based on asset name.
        /// </summary>
        /// <param name="name">The name of the asset being returned.</param>
        /// <returns>The Asset whose name matches the given name.</returns>
        public Asset this[String name]
        {
            get
            { return _assets.FirstOrDefault(next => next.Name.Equals(name)); }
        }

        /// <summary>
        /// Adds a given asset to the list of assets.
        /// </summary>
        /// <param name="asset">The given asset to add.</param>
        public void Add(Asset asset)
        {
            if(asset != null)
            {
                _assets.Add(asset);
            }
        }
    }
}
