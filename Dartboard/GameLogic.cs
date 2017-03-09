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

        public static void ThrowDart(Player player, int dart, int score, int remaining )
        {
           
          
                player.score -= score;
            
            
         
         

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

        public static string GetCheckout(Player player, Board board)
        {
           
            string value;
            if (board.Checkouts.TryGetValue(player.score, out value))
            {
                return value;
            }
            else
            {
                return "No Checkout";
            }
        }

        public static bool IsWinner(Player p)
        {
            //TODO double out logic 
            if (p.score == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        


    }
}