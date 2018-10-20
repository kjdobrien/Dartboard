using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Dartboard
{
    static class GameLogic
    {

        public static Player WhosTurn(Player p1, Player p2)
        {
            if (p1.turn)
            {
                return p1;
            }
            else
            {

                //p2.ScoreBoard.SetTextColor(Android.Graphics.Color.Red);
                //p1.ScoreBoard.SetTextColor(Android.Graphics.Color.Gainsboro);


                return p2;

            }
        }

        public static void ThrowDart(Player player, int dart, int score)
        {
            player.score -= score;
            string scoreBoard = string.Format("{0}: {1} Dart: {2}",player.name,  player.score, dart);
            //player.ScoreBoard.Text = scoreBoard;

           
            
        }

        public static void ThrowDart(Player player, int score)
        {
            player.score -= score;
        }

        public static void SwitchPlayer(Player p1, Player p2, Activity activity)
        {

            p1.turn = !p1.turn;
            p2.turn = !p2.turn;



            if (p1.turn)
            {
                (activity as GameViewWithKeyboard).Player1Layout.SetBackgroundColor(Android.Graphics.Color.Rgb(236, 229, 240));
                (activity as GameViewWithKeyboard).Player1Layout.Background.SetAlpha(50);
                (activity as GameViewWithKeyboard).Player2Layout.SetBackgroundColor(Android.Graphics.Color.White);
                (activity as GameViewWithKeyboard).Player2Layout.Background.SetAlpha(0);
            }
            else
            {
                (activity as GameViewWithKeyboard).Player2Layout.SetBackgroundColor(Android.Graphics.Color.Rgb(236, 229, 240));
                (activity as GameViewWithKeyboard).Player2Layout.Background.SetAlpha(50);
                (activity as GameViewWithKeyboard).Player1Layout.SetBackgroundColor(Android.Graphics.Color.White);
                (activity as GameViewWithKeyboard).Player1Layout.Background.SetAlpha(0);
            }



        }

      

        public static bool IsCheckout(Player player)
        {
            if (player.score <= 170)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        // Touch version 
        public static void GetCheckout(Player player, Board board, int touchCount)
        {

            string value;

            if (touchCount == 0 || touchCount == 3)
            {

                if (board.Checkouts.TryGetValue(player.score, out value))
                {
                    
                    player.Checkout = value;

                }
                else
                {                   
                    player.Checkout = " ";
                }
            }
            else if (touchCount == 1)
            {
                if (board.TwoDartCheckouts.TryGetValue(player.score, out value))
                {
                    player.Checkout = value;
                }
                else
                {
                    player.Checkout = " ";
                }
            }
            else if (player.score <= 40 && player.score % 2 == 0)
            {
                player.Checkout = "D" + (player.score / 2);
            }
            else
            {
                player.Checkout = " ";
            }
        }

        public static string GetCheckout(Player player, Board board)
        {
            string value3dart = "";
            string value2dart = "";
            board.Checkouts.TryGetValue(player.score, out value3dart);
            board.TwoDartCheckouts.TryGetValue(player.score, out value2dart);

            if (value2dart != null && value2dart != "No Checkout")
            {
                return value2dart;
            }
            else if (value3dart != null && value3dart != "No Checkout")
            {
                return value3dart;
            }
            else
            {
                return "";
            }
    
           
        }



        public static bool IsWinner(Player p)
        {
           
            if (p.score == 0)
            {
                
                return true;              
            }
            else
            {
                return false;
            }

        }

        //public static void ShowWinDialog(Context c, Player winner, List<Player> players,  Intent intent,  int leg, int touchCount, int startScore, int numLegs)
        //{
        //    // restart the game 
        //    var alert = new Android.Support.V7.App.AlertDialog.Builder(c);
        //    if (winner.legsWon >= (numLegs + 1)/2)
        //    {
        //        alert.SetTitle("Player " + winner.name + " wins the match!");
        //        alert.SetNeutralButton("Start Over", (senderAlert, args) => { c.StartActivity(intent); });
        //    }
        //    else
        //    {
        //        alert.SetTitle("Player " + winner.name + " wins the leg!");
        //        // move to next set/leg or start new game 
        //        alert.SetPositiveButton("Move to next leg", (senderAlert, args) => { MoveToNextLeg(leg, players, touchCount, startScore); });
        //        // neutral 

                
        //    }
        //    alert.SetNegativeButton("Back to setup", (senderAlert, args) => { c.StartActivity(typeof(CreateGame)); });
        //    Dialog dialog = alert.Create();
        //    dialog.Show();

           
        //}

        public static void ShowWinDialog(Context c, Player winner, List<Player> players, Intent intent, int leg, int startScore, int numLegs, Activity activity)
        {
            // restart the game 
            var alert = new Android.Support.V7.App.AlertDialog.Builder(c);
            if (winner.legsWon >= (numLegs + 1) / 2)
            {
                alert.SetTitle("Player " + winner.name + " wins the match!");
                alert.SetNeutralButton("Start Over", (senderAlert, args) => { c.StartActivity(intent); });
            }
            else
            {
                alert.SetTitle("Player " + winner.name + " wins the leg!");
                // move to next set/leg or start new game 
                alert.SetPositiveButton("Move to next leg", (senderAlert, args) => { MoveToNextLeg(leg, players, startScore, activity); });
                // neutral 


            }
            alert.SetNegativeButton("Back to setup", (senderAlert, args) => { c.StartActivity(typeof(CreateGame)); });
            Dialog dialog = alert.Create();
            dialog.Show();


        }

        //public static void MoveToNextLeg(int leg, List<Player> players, int touchCount, int startScore)
        //{
           
        //    ResetScores(players[0], startScore);
        //    ResetScores(players[1], startScore);
        //    touchCount = 0;

        //}

        public static void MoveToNextLeg(int leg, List<Player> players, int startScore, Activity activity)
        {

            ResetScores(players[0], startScore, activity);
            ResetScores(players[1], startScore, activity);

        }

        public static void ResetScores(Player p, int gameScore, Activity activity)
        {
            p.score = gameScore;
            (activity as GameViewWithKeyboard).player1Checkout.Text = "";
            (activity as GameViewWithKeyboard).player1Score.Text = gameScore.ToString();
            (activity as GameViewWithKeyboard).player2Checkout.Text = "";
            (activity as GameViewWithKeyboard).player2Score.Text = gameScore.ToString();
            //string scoreBoard = string.Format("{0}: {1} Dart: 1", p.name, p.score);
            //p.ScoreBoard.Text = scoreBoard;
            //p.Checkout.Text = " ";

        }


        public static void SaveGameData(Player p1, Player p2, int legsPlayed, int legsLeft)
        {
            using (StreamWriter file = File.CreateText("/data/data/Dartboard.Dartboard/previousGame.txt"))
            {
                file.WriteLine("Player1Name-" + p1.name);
                file.WriteLine("Player1Score-" + p1.score);
                file.WriteLine("Player1Turn-" + p1.turn);

                file.WriteLine("Player2Name-" + p2.name);
                file.WriteLine("Player2Score-" + p2.score);
                file.WriteLine("Player2Turn-" + p2.turn);

                file.WriteLine("LegsPlayed-" + legsPlayed);
                file.WriteLine("LegsLeft-" + legsLeft); 
            }
        }


    }
}