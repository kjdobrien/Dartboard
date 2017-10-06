using System;
using System.Collections.Generic;
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

                p2.ScoreBoard.SetTextColor(Android.Graphics.Color.Red);
                p1.ScoreBoard.SetTextColor(Android.Graphics.Color.Gainsboro);


                return p2;

            }
        }

        public static void ThrowDart(Player player, int dart, int score)
        {

            player.score -= score;
            string scoreBoard = string.Format("{0}: {1} Dart: {2}",player.name,  player.score, dart);
            player.ScoreBoard.Text = scoreBoard;
                    
        }

        public static void ThrowDartDouble(Player player, int dart, int score)
        {
            player.score -= (score * 2);
            string scoreBoard = string.Format("{0}: {1} Dart: {2}", player.name, player.score, dart);
            player.ScoreBoard.Text = scoreBoard;
        }

        public static void ThrowDartTreble(Player player, int dart, int score)
        {
            player.score -= (score * 3);
            string scoreBoard = string.Format("{0}: {1} Dart: {2}", player.name, player.score, dart);
            player.ScoreBoard.Text = scoreBoard;
        }

        public static bool CheckBust(int score, Player currentPlayer, Context c, int touchCount, string gesture)
        {                                
            if (score > currentPlayer.score || currentPlayer.score - score == 1 || (currentPlayer.score - score == 0 && gesture != "double"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void SwitchPlayer(Player p1, Player p2)
        {

            p1.turn = !p1.turn;
            p2.turn = !p2.turn;


            if (p1.turn)
            {
                p1.ScoreBoard.SetTextColor(Android.Graphics.Color.Red);
                p2.ScoreBoard.SetTextColor(Android.Graphics.Color.Gainsboro);
            }
            else
            {
                p1.ScoreBoard.SetTextColor(Android.Graphics.Color.Gainsboro);
                p2.ScoreBoard.SetTextColor(Android.Graphics.Color.Red);
            }
            


        }

        public static void FinishedTurn(List<Player> Players, Player currentPlayer, int touchCount)
        {
            touchCount = 0;
            if (Players.Count > 1)
            {
                Player player2;
                
                if (currentPlayer == Players[0])
                {
                    player2 = Players[1];
                }
                else
                {
                    player2 = Players[0];
                }

                                                                     
                GameLogic.SwitchPlayer(currentPlayer, player2);                                                                                  
            }         
        }
            
            
    
      

        public static bool IsCheckout(Player player)
        {
            if (player.score <= 170 && player.score % 2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        

        public static void GetCheckout(Player player, int touchCount)
        {

            string value;

            if (touchCount == 0 || touchCount == 3)
            {

                if (Board.Checkouts.TryGetValue(player.score, out value))
                {
                    
                    player.Checkout.Text = value;

                }
                else
                {                   
                    player.Checkout.Text = "No Checkout";
                }
            }
            else if (touchCount == 1)
            {
                if (Board.TwoDartCheckouts.TryGetValue(player.score, out value))
                {
                    player.Checkout.Text = value;
                }
                else
                {
                    player.Checkout.Text = "No Checkout";
                }
            }
            else if (player.score <= 40 && player.score % 2 == 0)
            {
                player.Checkout.Text = "D" + (player.score / 2);
            }
            else
            {
                player.Checkout.Text = "No Checkout";
            }
        }

        public static bool IsWinner(Player currentPlayer, int legs, string gesture)
        {
            // TODO: needs to say bust when a player hits the right number but not as a double 
            if (currentPlayer.score == 0 && gesture == "double")
            {
                
                return true;              
            }
            else
            {
                
                return false;
            }

        }

        public static void ShowWinDialog(Context c, Player winner, List<Player> players,  Intent intent,  int leg, int touchCount, int startScore, int numLegs)
        {
            // restart the game 
            var alert = new Android.Support.V7.App.AlertDialog.Builder(c);
            if (winner.legsWon >= (numLegs + 1)/2)
            {
                alert.SetTitle("Player " + winner.name + " wins the match!");
                alert.SetNeutralButton("Start Over", (senderAlert, args) => { c.StartActivity(intent); });
            }
            else
            {
                alert.SetTitle("Player " + winner.name + " wins the leg!");
                // move to next set/leg or start new game 
                alert.SetPositiveButton("Move to next leg", (senderAlert, args) => { MoveToNextLeg(leg, players, touchCount, startScore); });
                // neutral 

                
            }
            alert.SetNegativeButton("Back to setup", (senderAlert, args) => { c.StartActivity(typeof(CreateGame)); });
            Dialog dialog = alert.Create();
            dialog.Show();

           
        }

        public static void MoveToNextLeg(int leg, List<Player> players, int touchCount, int startScore)
        {
           
            ResetScores(players[0], startScore);
            ResetScores(players[1], startScore);
            touchCount = 0;

        }

        public static void ResetScores(Player p, int gameScore)
        {
            p.score = gameScore;
            string scoreBoard = string.Format("{0}: {1} Dart: 1", p.name, p.score);
            p.ScoreBoard.Text = scoreBoard;
            p.Checkout.Text = " ";

        }

        public static void ResetCounters(int previousTurn, int touchCount )
        {
            previousTurn = touchCount;
            touchCount++;
        }

        public static void UndoLastScore(Player currentPlayer, int score)
        {
            currentPlayer.score += score; 
        }

        public static void DisplayScore(Context c, int score)
        {

            Toast toast = Toast.MakeText(c, String.Format("{0}", score), ToastLength.Short);
            toast.SetGravity(GravityFlags.Top, 0, 0);
            View v = toast.View;
            v.SetBackgroundColor(Android.Graphics.Color.ParseColor("#31A447"));
            toast.Show();
        }



    }
}