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
using Android.Graphics;

namespace Dartboard
{
    public class MyGestureListener : GestureDetector.SimpleOnGestureListener
    {

        int score;
        int legs;
        int touchCount;
        int previousTurn;
        int startScore;
        int numLegs;
        Player currentPlayer;
        Context c;

        MainActivity activity;
        Board board = new Board();
     
        

        public MyGestureListener(int Score, Player CurrentPlayer, MainActivity _activity, int TouchCount, int PreviousTurn, int legs, int startScore, int numLegs)
        {
            this.score = Score;
            this.numLegs = numLegs;
            this.startScore = startScore;
            this.currentPlayer = CurrentPlayer;
            this.activity = _activity;
            this.touchCount = TouchCount;
            this.previousTurn = PreviousTurn;
            this.legs = legs;
            
            
        }

        public override bool OnSingleTapConfirmed(MotionEvent e)
        {
            // Increase touch count 
            touchCount++;
            // Add gesture to list 
            string gesture = "single";
            // Clear garbage
            GC.Collect();
            // Get the current player 
            currentPlayer = GameLogic.WhosTurn(MainActivity.testPlayer, MainActivity.player2);
            Console.WriteLine("Single Tap");

            // Activate undo button
            MainActivity.undo.Enabled = true;

            

            // Get touch coords
            var x = (int)e.GetX();
            var y = (int)e.GetY();
            // Use coords to get the underlying colour 
            int touchColour = activity.getColorHotspot(Resource.Id.dartboardoverlay,x, y);
            Color myColor = new Color(touchColour);
            // Use colour to get score 
            score = board.ColorScores.FirstOrDefault(k => k.Value == myColor).Key;

            GameLogic.ResetCounters(previousTurn, touchCount);        
            Console.WriteLine(score);

            // Check for bust
            if (GameLogic.CheckBust(score, currentPlayer, activity, touchCount, gesture))
            {
                // Display toast message 
                Toast toast = Toast.MakeText(activity, "Bust", ToastLength.Short);
                toast.SetGravity(GravityFlags.Center, 0, 0);
                View v = toast.View;
                v.SetBackgroundColor(Android.Graphics.Color.ParseColor("#31A447"));
                toast.Show();
                touchCount = 0;
                GameLogic.FinishedTurn(activity.Players, currentPlayer, touchCount);
            }
            else
            {
                // Display the score
                GameLogic.DisplayScore(activity, score);
                // Subtract the score
                GameLogic.ThrowDart(currentPlayer, touchCount, score);
            }
            // Display the best check out if available 
            if (currentPlayer.score <= 170)
            {
                GameLogic.GetCheckout(currentPlayer, board, touchCount);
            }
            else
            {
                currentPlayer.Checkout.Text = " ";
            }
            // Check to see did the player win with the last dart 
            if (GameLogic.IsWinner(currentPlayer, legs, gesture))
            {
                
                touchCount = 0;
                legs -= 1;
                currentPlayer.legsWon += 1;
                GameLogic.ShowWinDialog(activity, currentPlayer, activity.Players, activity.Intent, legs, touchCount, startScore, numLegs);
            }
            // Check to see if the player is finished their turn 
            if (touchCount == 3 && currentPlayer.score > 0)
            {
                touchCount = 0;
                GameLogic.FinishedTurn(activity.Players ,currentPlayer, touchCount);

            }
            
            
            return true;
        }

        public override bool OnDoubleTap(MotionEvent e)
        {
            touchCount++;
            // Add gesture to list 
            string gesture = "double";
            GC.Collect();
            currentPlayer = GameLogic.WhosTurn(MainActivity.testPlayer, MainActivity.player2);
            Console.WriteLine("Double Tap");
            // Activate undo button
            MainActivity.undo.Enabled = true;
            var x = (int)e.GetX();
            var y = (int)e.GetY();
            int touchColour = activity.getColorHotspot(Resource.Id.dartboardoverlay, x, y);
            Color myColor = new Color(touchColour);
            score = board.ColorScores.FirstOrDefault(k => k.Value == myColor).Key;
            GameLogic.ResetCounters(previousTurn, touchCount);
            Console.WriteLine(score);
            if (GameLogic.CheckBust(score, currentPlayer, activity, touchCount, gesture))
            {
                Toast toast = Toast.MakeText(activity, "Bust", ToastLength.Short);
                toast.SetGravity(GravityFlags.Center, 0, 0);
                View v = toast.View;
                v.SetBackgroundColor(Android.Graphics.Color.ParseColor("#31A447"));
                toast.Show();
                touchCount = 0;
                GameLogic.FinishedTurn(activity.Players, currentPlayer, touchCount);
            }
            else
            {
                GameLogic.ThrowDartDouble(currentPlayer, touchCount, score);
            }
            if (currentPlayer.score <= 170)
            {
                GameLogic.GetCheckout(currentPlayer, board, touchCount);
            }
            else
            {
                currentPlayer.Checkout.Text = " ";
            }

            if (GameLogic.IsWinner(currentPlayer, legs, gesture))
            {
                touchCount = 0;
                legs -= 1;
                currentPlayer.legsWon += 1;
                GameLogic.ShowWinDialog(activity, currentPlayer, activity.Players, activity.Intent, legs, touchCount, startScore, numLegs);
            }
           
            if (touchCount == 3)
            {
                touchCount = 0;
                GameLogic.FinishedTurn(activity.Players, currentPlayer, touchCount);
            }
            return true;
        }

        public override void OnLongPress(MotionEvent e)
        {
            touchCount++;
            // Add gesture to list 
            string gesture = "treble";
            GC.Collect();
            currentPlayer = GameLogic.WhosTurn(MainActivity.testPlayer, MainActivity.player2);
            Console.WriteLine("Long Press");
            // Activate undo button
            MainActivity.undo.Enabled = true;
            var x = (int)e.GetX();
            var y = (int)e.GetY();
            int touchColour = activity.getColorHotspot(Resource.Id.dartboardoverlay, x, y);
            Color myColor = new Color(touchColour);
            score = board.ColorScores.FirstOrDefault(k => k.Value == myColor).Key;
            GameLogic.ResetCounters(previousTurn, touchCount);
            Console.WriteLine(score);
            if (GameLogic.CheckBust(score, currentPlayer, activity, touchCount, gesture))
            {
                Toast toast = Toast.MakeText(activity, "Bust", ToastLength.Short);
                toast.SetGravity(GravityFlags.Center, 0, 0);
                View v = toast.View;
                v.SetBackgroundColor(Android.Graphics.Color.ParseColor("#31A447"));
                toast.Show();
                touchCount = 0; 
                GameLogic.FinishedTurn(activity.Players, currentPlayer, touchCount);
            }
            else
            {
                GameLogic.ThrowDartTreble(currentPlayer, touchCount, score);
            }
            if (currentPlayer.score <= 170)
            {
                GameLogic.GetCheckout(currentPlayer, board, touchCount);
            }
            else
            {
                currentPlayer.Checkout.Text = " ";
            }
            if (GameLogic.IsWinner(currentPlayer, legs, gesture))
            {
                touchCount = 0;
                legs -= 1;
                currentPlayer.legsWon += 1;
                GameLogic.ShowWinDialog(activity, currentPlayer, activity.Players, activity.Intent, legs, touchCount, startScore, numLegs);
            }
            if (touchCount == 3)
            {
                touchCount = 0;
                GameLogic.FinishedTurn(activity.Players, currentPlayer, touchCount);
            }

        }

        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            Console.WriteLine("Fling");
            //return base.OnFling(e1, e2, velocityX, velocityY);
            return true;
        }

        public override bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            Console.WriteLine("Scroll");
            //return base.OnScroll(e1, e2, distanceX, distanceY);
            return true;
        }

        public void UndoButton(object sender, EventArgs args)
        {
            GameLogic.UndoLastScore(currentPlayer, score);
            touchCount = previousTurn;
            string scoreBoard = string.Format("{0}: {1} Dart: {2}", currentPlayer.name, currentPlayer.score, touchCount);
            currentPlayer.ScoreBoard.Text = scoreBoard;
            MainActivity.undo.Enabled = false;
        }
    }
}