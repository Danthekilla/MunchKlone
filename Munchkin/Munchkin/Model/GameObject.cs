using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Munchkin
{   
    /// <summary>
    /// A model class of an object in the game
    /// </summary>
    class GameObject
    {
        //Position property
        public Vector2 Position
        { get; set; }

        /// <summary>
        /// Game object constructor.
        /// </summary>
        /// <param name="position">The position of the game object.</param>
        public GameObject(Vector2 position)
        {
            Position = position;
        }
    }
}
