using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Munchkin.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Munchkin;

namespace Munchkin.Control
{
    /// <summary>
    /// The event handler for buttons in-game.
    /// </summary>
    class ButtonHandler
    {
        private List<Button> buttons;
        private Vector2 initialButtonPosition;

        /// <summary>
        /// ButtonHandler constructor
        /// </summary>
        /// <param name="initial"></param>
        public ButtonHandler(Vector2 initial)
        {
            buttons = new List<Button>();
            initialButtonPosition = initial; 
        }

        /// <summary>
        /// Adds the given button to the list of buttons.
        /// </summary>
        /// <param name="b">The button to add to the list.</param>
        public void Add(Button b)
        {
            buttons.Add(b);
        }

        /// <summary>
        /// Updates the state of the buttons.
        /// </summary>
        /// <param name="doorDeck">The state of the doorDeck</param>
        /// <param name="treasureDeck">The state of the treasureDeck</param>
        /// <param name="clickedCard">The state of the card that was clicked</param>
        /// <param name="last">The last state of the Mouse</param>
        /// <param name="current">The current state of the Mouse</param>
        /// <param name="doorDiscard">The state of the doorDeck discard pile</param>
        /// <param name="treasureDiscard">The state of the treasureDeck discard pile</param>
        public void UpdateButtons(Deck doorDeck, Deck treasureDeck, Card clickedCard, MouseState last, MouseState current, Discard doorDiscard, Discard treasureDiscard)
        {
            SetActiveButtons(doorDeck, treasureDeck, clickedCard, doorDiscard, treasureDiscard);
            SetButtonPositions();
        }

        /// <summary>
        /// Event listener for button clicks.
        /// </summary>
        /// <param name="last">The last state of the Mouse</param>
        /// <param name="current">The current state of the Mouse</param>
        /// <param name="doorDeck">The state of the doorDeck</param>
        /// <param name="treasureDeck">The state of the treasureDeck</param>
        /// <param name="clickedCard">The state of the clicked card.</param>
        /// <param name="local">The state of the local player.</param>
        /// <param name="pf">The state of the playing field.</param>
        /// <param name="doorDiscard">The state of the doorDeck discard pile.</param>
        /// <param name="treasureDiscard">The state of the treasureDeck discard pile.</param>
        public void SubmitButtonAction(MouseState last, MouseState current, Deck doorDeck, Deck treasureDeck, Card clickedCard, Player local, PlayingField pf, Discard doorDiscard, Discard treasureDiscard)
        {
            foreach (Button b in buttons)
            {
                if(b.IsClicked(last, current))
                {
                    ExecuteButtonFunctions(b.Command, doorDeck, treasureDeck, clickedCard, local, pf, doorDiscard, treasureDiscard);
                    return;
                }
            }
        }

        /// <summary>
        /// Event handler for when a button is clicked.
        /// Identifies which button was clicked and performs an action.
        /// </summary>
        /// <param name="buttonCommand">The command string of the button that is clicked.</param>
        /// <param name="doorDeck">The state of the doorDeck</param>
        /// <param name="treasureDeck">The state of the treasureDeck</param>
        /// <param name="clicked">The state of the clicked card.</param>
        /// <param name="local">The state of the local player.</param>
        /// <param name="pf">The state of the playing field.</param>
        /// <param name="doorDiscard">The state of the doorDeck discard pile.</param>
        /// <param name="treasureDiscard">The state of the treasureDeck discard pile.</param>
        private void ExecuteButtonFunctions(String buttonCommand, Deck doorDeck, Deck treasureDeck, Card clicked, Player local, PlayingField pf, Discard doorDiscard, Discard treasureDiscard)
        {
            Card c = ApplyActionToCard(doorDeck, treasureDeck, clicked, doorDiscard, treasureDiscard);
            switch (buttonCommand)
            {
                case "Add To Hand":
                    local.AddCardToHand(c);
                    if (pf.ContainsCard(c.ID))
                        pf.RemoveCard(c.ID);
                    break;
                case "Draw Face Up":
                    pf.Add(c);
                    break;
                case "Discard":
                    if(c.Type.Equals("Door"))
                    {
                        doorDiscard.Add(c);
                        System.Diagnostics.Debug.WriteLine(doorDeck.Cards.Count);
                    }
                    else
                    {
                        treasureDiscard.Add(c);
                    }
                    if(local.HasCard(c.ID))
                    {
                        Card c1 = local.RemoveCard(c.ID);
                    }
                    if(pf.ContainsCard(c.ID))
                    {
                        pf.RemoveCard(c.ID);
                    }
                    break;
                case "Add To Playing Field":
                    pf.Add(c);
                    if(local.HasCard(c.ID))
                    {
                        Card aCard = local.RemoveCard(c.ID);
                    }
                    break;
                case "Clear Playing Field":
                    pf.ClearPlayingField(doorDiscard, treasureDiscard);
                    break;
                case "Add To BackPack":
                    local.AddCardToBackPack(c);
                    if (pf.ContainsCard(c.ID))
                        pf.RemoveCard(c.ID);
                    break;
                case "Add To Equipped":
                    local.AddCardToEquipped(c);
                    if (pf.ContainsCard(c.ID))
                        pf.RemoveCard(c.ID);
                    break;
            }
            CommunicationController.SubmitButtonAction(local.Gamertag, c.ID.ToString(), buttonCommand);
        }

        /// <summary>
        /// Populates the list buttons based on which deck is clicked.
        /// </summary>
        /// <param name="doorDeck">The state of the doorDeck</param>
        /// <param name="treasureDeck">The state of the treasureDeck</param>
        /// <param name="clicked">The state of the clicked card.</param>
        /// <param name="doorDiscard">The state of the doorDeck discard pile.</param>
        /// <param name="treasureDiscard">The state of the treasureDeck discard pile.</param>
        /// <returns>The new clicked card.</returns>
        private static Card ApplyActionToCard(Deck doorDeck, Deck treasureDeck, Card clicked, Discard doorDiscard, Discard treasureDiscard)
        {
            if (doorDeck.Selected)
            {
                if (doorDeck.Cards.Count > 0)
                {
                    Card c = doorDeck.DrawCard();
                    doorDeck.Selected = false;
                    return c;
                }
                else
                {
                    doorDeck.Repopulate(doorDiscard);
                    Card c = doorDeck.DrawCard();
                    doorDeck.Selected = false;
                    return c;
                }
            }
            else if (treasureDiscard.Selected)
            {
                treasureDiscard.Selected = false;
                return treasureDiscard.TakeCard();
                
            }
            else if (doorDiscard.Selected)
            {
                doorDiscard.Selected = false;
                return doorDiscard.TakeCard();
            }
            else if (treasureDeck.Selected)
            {
                if (treasureDeck.Cards.Count > 0)
                {
                    Card c = treasureDeck.DrawCard();
                    treasureDeck.Selected = false;
                    return c;
                }
                else
                {
                    treasureDeck.Repopulate(treasureDiscard);
                    Card c = treasureDeck.DrawCard();
                    treasureDeck.Selected = false;
                    return c;
                }
            }
            else if (clicked != null && clicked.Selected)
            {
                Card c = clicked;
                clicked.Selected = false;
                return c;
            }
            return null;

        }

        /// <summary>
        /// Updates the position of the list of buttons
        /// </summary>
        private void SetButtonPositions()
        {
            Vector2 LeftColumnPosition = initialButtonPosition;
            Vector2 RightColumnPosition = initialButtonPosition;
            RightColumnPosition.X += 80;
            int activeCount = 0;
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].Active)
                {
                    if (activeCount <= 5)
                    {
                        buttons[i].Position = LeftColumnPosition;
                        buttons[i].ClickableArea = new Rectangle((int)LeftColumnPosition.X, (int)LeftColumnPosition.Y, 80, 36);
                        LeftColumnPosition.Y += 37;
                    }
                    else if (activeCount > 5)
                    {
                        RightColumnPosition.Y += 37;
                        buttons[i].Position = RightColumnPosition;
                        buttons[i].ClickableArea = new Rectangle((int)RightColumnPosition.X, (int)RightColumnPosition.Y, 80, 36);
                        
                    }
                    activeCount++;
                }
            }
        }

        /// <summary>
        /// Updates which buttons out of the list of buttons is active.
        /// </summary>
        /// <param name="doorDeck">The state of the doorDeck</param>
        /// <param name="treasureDeck">The state of the treasureDeck</param>
        /// <param name="clickedCard">The state of the clicked card.</param>
        /// <param name="treasureDiscard">The state of the treasureDeck discard pile.</param>
        /// <param name="doorDiscard">The state of the doorDeck discard pile.</param>
        private void SetActiveButtons(Deck doorDeck, Deck treasureDeck, Card clickedCard, Discard treasureDiscard, Discard doorDiscard)
        {
            foreach (Button b in buttons)
            {
                if (doorDeck.Selected)
                {
                    if (doorDeck.Commands.Contains(b.Command))
                    {
                        b.Active = true;
                    }
                    else
                    {
                        b.Active = false;
                        b.ClickableArea = new Rectangle(0, 0, 0, 0);
                    }
                }
                else if (treasureDeck.Selected)
                {
                    if (treasureDeck.Commands.Contains(b.Command))
                    {
                        b.Active = true;
                    }
                    else
                    {
                        b.Active = false;
                        b.ClickableArea = new Rectangle(0, 0, 0, 0);
                    }
                }
                else if(treasureDiscard.Selected)
                {
                    if (treasureDiscard.Commands.Contains(b.Command))
                    {
                        b.Active = true;
                    }
                    else
                    {
                        b.Active = false;
                        b.ClickableArea = new Rectangle(0, 0, 0, 0); 
                    }
                }
                else if (doorDiscard.Selected)
                {
                    if (doorDiscard.Commands.Contains(b.Command))
                    {
                        b.Active = true;
                    }
                    else
                    {
                        b.Active = false;
                        b.ClickableArea = new Rectangle(0, 0, 0, 0);
                    }
                }
                else if (clickedCard != null && clickedCard.Selected)
                {
                    if (clickedCard.Commands.Contains(b.Command))
                    {
                        b.Active = true;
                    }
                    else
                    {
                        b.Active = false;
                        b.ClickableArea = new Rectangle(0, 0, 0, 0);
                    }
                }
                else
                {
                    b.Active = false;
                }
            }
        }

        /// <summary>
        /// Draws the active buttons.
        /// </summary>
        /// <param name="sb">The spritebatch that is being drawn on.</param>
        public void DrawButtons(SpriteBatch sb)
        {
            foreach (Button b in buttons)
            {
                if (b.Active)
                {
                    b.Draw(sb);
                }
            }
        }
    }
}
