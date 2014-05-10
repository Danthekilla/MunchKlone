using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Munchkin.Model
{
    class Button
    {
        private Texture2D image; //button image
        private String commandText; //text for the button
        private Vector2 position; //position of the button
        private Rectangle clickableArea; //clickable area of the button. For handling on click mouse events. 
        private bool active; //if button is active. 

        /// <summary>
        /// Button constructor. Sets the image and command for the button
        /// </summary>
        /// <param name="_image">Tex2D of the button</param>
        /// <param name="command">Command string to be printed on the button</param>
        public Button(Texture2D _image, String command)
        {
            image = _image;
            commandText = command;
            active = false;
        }


        public Texture2D Image
        {
            get { return image; }
        }

        public String Command
        {
            get
            {
                return commandText;
            }
            set
            {
                commandText = value;
            }
        }

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

        public Rectangle ClickableArea
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

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        /// <summary>
        /// Returns if the mouse has clicked within the area of the button
        /// </summary>
        /// <param name="last">Last mouse state</param>
        /// <param name="current">Current mouse state</param>
        /// <returns>True iff the button was clicked. </returns>
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
        /// Draw method for the button. 
        /// </summary>
        /// <param name="sb">SpriteBatch used to draw the button.</param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(image, position, Color.White);
        }
        
    }
}
