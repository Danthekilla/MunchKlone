using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Munchkin
{
    class Card
    {
        private Texture2D smallFrontImage;
        private Texture2D mediumFrontImage;
        private Texture2D largeFrontImage;
        private Texture2D smallBackImage;
        private String name;
        private String cardType;
        private String description;
        private int id;
        private Vector2 currentPosition;
        private Microsoft.Xna.Framework.Rectangle clickableArea;
        private List<String> commands;
        private bool selected;

        //Card ID
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        //small image for card front. 
        public Texture2D SmallFrontImage
        {
            get
            {
                return smallFrontImage;
            }
            set
            {
                smallFrontImage = value;
            }
        }
        //Small image for card back
        public Texture2D SmallBackImage
        {
            get
            {
                return smallBackImage;
            }
            set
            {
                smallBackImage = value;
            }
        }
        //Medium image for card front. 
        public Texture2D MediumFrontImage
        {
            get
            {
                return mediumFrontImage;
            }
            set
            {
                mediumFrontImage = value;
            }
        }
        //Large image for card front. 
        public Texture2D LargeFrontImage
        {
            get
            {
                return largeFrontImage;
            }
            set
            {
                largeFrontImage = value;
            }
        }
        //If card is selected
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }
        //Name of the card
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        //Description of the card
        public String Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
        //Type of the card
        public String Type
        {
            get
            {
                return cardType;
            }
            set
            {
                cardType = value;
            }
        }
        //Position of the card
        public Vector2 Position
        {
            get
            {
                return currentPosition;
            }
            set
            {
                currentPosition = value;
            }
        }
        //Clickable area of the card
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
        //List of Commands for the card 
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
        //Determines if the card has been clicked. 
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
    }
}
