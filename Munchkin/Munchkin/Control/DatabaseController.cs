using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

namespace Munchkin
{
    class DatabaseController
    {
        /// <summary>
        /// Creates the connection to the myPhpAdmin database.
        /// </summary>
        /// <returns>Connection to database</returns>
        public static MySqlConnection connect()
        {
            string conStr = "server=cs.westga.edu; port=3307; uid=jspinks1; pwd=js@3716; database=cs4982s13g;";
            try
            {
                MySqlConnection conn = new MySqlConnection(conStr);
                return conn;
            }
            catch (Exception ex)
            {
                String error = ex.Message;
                System.Console.WriteLine(error);
                return null;
            }
        }

        /// <summary>
        /// Gets a dataset of all the cards in the database. 
        /// </summary>
        /// <returns>Dataset of cards</returns>
        public static DataSet GetCards()
        {
            List<Card> cards = new List<Card>();
            
            MySqlConnection conn = connect();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * From Cards", conn);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "Cards");
            conn.Close();
            
            return ds;
        }


        /// <summary>
        /// Logs the player into the game. Checks against database if username and password matches.
        /// </summary>
        /// <param name="username">player username</param>
        /// <param name="password">player password</param>
        /// <returns>True iff username/password exists</returns>
        public static bool LogIn(string username, string password)
        {
            MySqlConnection conn = connect();
            conn.Open();
            
            string query = "select * from Login where username=@username and password=@password";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            var param0 = new MySqlParameter { ParameterName = "@username", Value = username };

            var param1 = new MySqlParameter { ParameterName = "@password", Value = password };

            cmd.Parameters.Add(param0);
            cmd.Parameters.Add(param1);

            
            
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                return true;
            }
            else 
                return false;
        }

        /// <summary>
        /// Creates an account for the player if they do not have one
        /// and returns a string on the success of account creation
        /// </summary>
        /// <param name="username">player username</param>
        /// <param name="password">player password</param>
        /// <returns>A string representing success or failure of the account creation</returns>
        public static string CreateAccount(string username, string password)
        {
            String result = "";
            try
            {
                
                MySqlConnection conn = connect();
                conn.Open();

                if (username.Equals(""))
                {
                    result = "Username field blank. \nPlease input a username of at least 6 characters.";
                    return result;
                }
                else if (username.Length < 6)
                {
                    result = "Username invalid. \nUsername must be at least 6 characters in length";
                    return result;
                }
                else if (CheckUsername(username))
                {
                    result = "This username already exists. \nPlease choose a different one";
                    return result;
                }
                else if (password.Equals(""))
                {
                    result = "Password field blank. \nPlease input a password of at least 6 characters.";
                    return result;
                }
                else if (password.Length < 6)
                {
                    result = "Password invalid. \nPassword must be at least 6 characters in length";
                    return result;
                }


                string insert = "INSERT into Login(username, password) VALUES (@pid, @password);";
                string insert2 = "Insert into Player(pID, totalLevel, gamesWon, gamesLost) Values (@playerID, 0, 0, 0);";

                MySqlCommand cmd = new MySqlCommand(insert, conn);
                MySqlCommand cmd2 = new MySqlCommand(insert2, conn);


                cmd.Parameters.Add("@pid", MySqlDbType.VarChar, 16);
                cmd.Parameters.Add("@password", MySqlDbType.VarChar, 16);

                cmd2.Parameters.Add("@playerID", MySqlDbType.VarChar, 16);

                cmd.Parameters["@pid"].Value = username;
                cmd.Parameters["@password"].Value = password;

                cmd2.Parameters["@playerID"].Value = username;

                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                conn.Close();
                return result = "Your account has been created";
            }
            catch (Exception e)
            {
                return result = "There was an error creating your account.";
                //Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Returns a list of the player stats to display on the game over screen. 
        /// </summary>
        /// <param name="gamertag">Gamertag of the player</param>
        /// <returns>List representing various player stats to be printed out.</returns>
        public static List<String> GetPlayerStats(String gamertag)
        {
            MySqlConnection conn = connect();
            conn.Open();
            List<String> returnList = new List<String>();
            string query = "Select * From Player Where pID = @gamertag";
            MySqlCommand command = new MySqlCommand(query, conn);
            command.Parameters.Add("@gamertag", MySqlDbType.VarChar, 16);
            command.Parameters["@gamertag"].Value = gamertag;

            MySqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                String gamer = dr[0].ToString();
                String totalLevel = dr[1].ToString();
                String gamesWon = dr[2].ToString();
                String gamesLost = dr[3].ToString();
                returnList.Add(gamer);
                returnList.Add(totalLevel);
                returnList.Add(gamesWon);
                returnList.Add(gamesLost);
            }
            dr.Close();
            conn.Close();
            
            return returnList;
        }

        /// <summary>
        /// Saves the player stats at the end of the game. 
        /// </summary>
        /// <param name="gamertag">Gamertag of the player</param>
        /// <param name="totalLevel">Total levels earned by the player</param>
        /// <param name="gamesWon">Games won by the player</param>
        /// <param name="gamesLost">Games lost by the player</param>
        public static void SavePlayerStats(String gamertag, int totalLevel, int gamesWon, int gamesLost)
        {
            MySqlConnection conn = connect();
            conn.Open();
            string update = "Update Player Set totalLevel = @level, gamesWon = @won, gamesLost = @lost WHERE pID = @gamertag";
            MySqlCommand command = new MySqlCommand(update, conn);
            command.Parameters.Add("@gamertag", MySqlDbType.VarChar, 16);
            command.Parameters["@gamertag"].Value = gamertag;
            command.Parameters.Add("@level", MySqlDbType.Int32, 11);
            command.Parameters["@level"].Value = totalLevel;
            command.Parameters.Add("@won", MySqlDbType.Int32, 11);
            command.Parameters["@won"].Value = gamesWon;
            command.Parameters.Add("@lost", MySqlDbType.Int32, 11);
            command.Parameters["@lost"].Value = gamesLost;
            command.ExecuteNonQuery();
            conn.Close();
        }

        /// <summary>
        /// Helper method to determine if a username exists in the Database already.
        /// </summary>
        /// <param name="username">username to check against the database. </param>
        /// <returns>True iff the username already exists</returns>
        internal static bool CheckUsername(string username)
        {
            MySqlConnection conn = connect();
            conn.Open();

            string query = "select * from Login where username=@username";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            var param0 = new MySqlParameter { ParameterName = "@username", Value = username };
            cmd.Parameters.Add(param0);
            MySqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
            return reader.HasRows;
        }

    }
}
