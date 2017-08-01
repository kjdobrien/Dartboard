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

        public static bool CheckBust(int score, Player currentPlayer, Context c, int touchCount )
        {
            if (score > currentPlayer.score || currentPlayer.score - score == 1)
            {
                Toast toast = Toast.MakeText(c, "Bust", ToastLength.Short);
                toast.SetGravity(GravityFlags.Center, 0, 0);
                toast.Show();
                return false;
            }
            else
            {
                //GameLogic.ThrowDart(currentPlayer, touchCount, score);
                return true; 
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

        public static void GetCheckout(Player player, Board board, int touchCount)
        {

            string value;

            if (touchCount == 0 || touchCount == 3)
            {

                if (board.Checkouts.TryGetValue(player.score, out value))
                {
                    
                    player.Checkout.Text = value;

                }
                else
                {                   
                    player.Checkout.Text = " ";
                }
            }
            else if (touchCount == 1)
            {
                if (board.TwoDartCheckouts.TryGetValue(player.score, out value))
                {
                    player.Checkout.Text = value;
                }
                else
                {
                    player.Checkout.Text = " ";
                }
            }
            else if (player.score <= 40 && player.score % 2 == 0)
            {
                player.Checkout.Text = "D" + (player.score / 2);
            }
            else
            {
                player.Checkout.Text = " ";
            }
        }

        public static bool IsWinner(Player p, int legs)
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

      



    }
}