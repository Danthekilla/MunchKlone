using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Munchkin
{
    /// <summary>
    /// The controller for network communication
    /// </summary>
    class CommunicationController
    {
        public static PacketWriter packetWriter = new PacketWriter();
        public static PacketReader packetReader = new PacketReader();
        public static NetworkSession networkSession;
        
        /// <summary>
        /// Updates the decks of network players.
        /// Sends a sync command using the packet reader
        /// </summary>
        /// <param name="doorDeck">The state of the local doorDeck</param>
        /// <param name="treasureDeck">The state of the local treasureDeck</param>
        public static void UpdateRemoteDecks(Deck doorDeck, Deck treasureDeck)
        {
            packetWriter.Write("@");
            packetWriter.Write(doorDeck.toString());
            packetWriter.Write(treasureDeck.toString());
            SendInput();
        }

        /// <summary>
        /// Updates the level of a single player.
        /// Sends a sync command using the packet reader
        /// </summary>
        /// <param name="gamertag">The gamertag who has changed in level</param>
        /// <param name="level">The player's new level</param>
        public static void PlayerChangedLevel(String gamertag, String level)
        {
            packetWriter.Write("^");
            packetWriter.Write(gamertag);
            packetWriter.Write(level);
        }

        /// <summary>
        /// Updates the state of the game over condition for network players.
        /// Sends a sync command using the packet reader.
        /// </summary>
        /// <param name="combinedText">The complex command word string</param>
        /// <param name="tempInt">The message code.</param>
        public static void GameOverStatusChanged(String combinedText, int tempInt)
        {
                packetWriter.Write("~");
                packetWriter.Write(combinedText);
                packetWriter.Write(tempInt);
                SendInput();
        }

        /// <summary>
        /// Sends a button action to network players using a sync command
        /// via the packet reader.
        /// </summary>
        /// <param name="arg1">The button command word</param>
        /// <param name="arg2">The player gamertag</param>
        /// <param name="arg3">Additional information</param>
        public static void SubmitButtonAction(String arg1, String arg2, String arg3)
        {
            packetWriter.Write("!");
            packetWriter.Write(arg1 + "," + arg2 + "," + arg3);
            SendInput();
        }

        /// <summary>
        /// Sends input to network players.
        /// </summary>
        public static void SendInput()
        {
            LocalNetworkGamer gamer = networkSession.LocalGamers[0];
            gamer.SendData(packetWriter, SendDataOptions.InOrder);
        }
    }
}
