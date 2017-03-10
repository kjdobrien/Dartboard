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
                return p2;
            }
        }

        public static void ThrowDart(Player player, int dart, int score)
        {
            player.score -= score;
            string scoreBoard = string.Format("Player {0}: {1} Dart: {2}",player.name,  player.score, dart);
            player.ScoreBoard.Text = scoreBoard;
            
        }

        public static void SwitchPlayer(Player p1, Player p2)
        {
            p1.turn = !p1.turn;
            p2.turn = !p2.turn;
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
                    player.Checkout.Text = "No Checkout";
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

        public static void ShowWinDialog(Context c, Player p)
        {
            // restart the game 
            AlertDialog.Builder alert = new AlertDialog.Builder(c);
            alert.SetTitle("Player " + p.name + " wins!");
            // move to next set/leg or start new game 
            alert.SetPositiveButton("Play again?", (senderAlert, args) => { /* Start the activity again*/});
            // neutral 
            alert.SetNegativeButton("Back to setup", (senderAlert, args) => {/* Run the setup activity */ });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        


    }
}