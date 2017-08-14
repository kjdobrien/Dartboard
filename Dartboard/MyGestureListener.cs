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
        Player currentPlayer;
        Context c;
        int touchCount;
        int previousTurn;
        MainActivity activity;
        Board board = new Board();
        

        public MyGestureListener(int Score, Player CurrentPlayer, MainActivity _activity, int TouchCount, int PreviousTurn)
        {
            this.score = Score;
            this.currentPlayer = CurrentPlayer;
            this.activity = _activity;
            this.touchCount = TouchCount;
            this.previousTurn = PreviousTurn;
            
            
        }



        public override bool OnSingleTapConfirmed(MotionEvent e)
        {
            Console.WriteLine("Single Tap"); 
            var x = (int)e.GetX();
            var y = (int)e.GetY();
            int touchColour = activity.getColorHotspot(Resource.Id.dartboardoverlay,x, y);
            Color myColor = new Color(touchColour);
            score = board.ColorScores.FirstOrDefault(k => k.Value == myColor).Key;
            GameLogic.ResetCounters(previousTurn, touchCount);        
            Console.WriteLine(score);
            GameLogic.CheckBust(score, currentPlayer, activity, touchCount);
            GameLogic.ThrowDart(currentPlayer, touchCount, score);
            return true;
        }

        public override bool OnDoubleTap(MotionEvent e)
        {
            Console.WriteLine("Double Tap");
            var x = (int)e.GetX();
            var y = (int)e.GetY();
            int touchColour = activity.getColorHotspot(Resource.Id.dartboardoverlay, x, y);
            Color myColor = new Color(touchColour);
            score = board.ColorScores.FirstOrDefault(k => k.Value == myColor).Key;
            GameLogic.ResetCounters(previousTurn, touchCount);
            Console.WriteLine(score);
            GameLogic.CheckBust(score, currentPlayer, activity, touchCount);
            GameLogic.ThrowDartDouble(currentPlayer, touchCount, score);
            return true;
        }

        public override void OnLongPress(MotionEvent e)
        {
            Console.WriteLine("Long Press");
            var x = (int)e.GetX();
            var y = (int)e.GetY();
            int touchColour = activity.getColorHotspot(Resource.Id.dartboardoverlay, x, y);
            Color myColor = new Color(touchColour);
            score = board.ColorScores.FirstOrDefault(k => k.Value == myColor).Key;
            GameLogic.ResetCounters(previousTurn, touchCount);
            Console.WriteLine(score);
            GameLogic.CheckBust(score, currentPlayer, activity, touchCount);
            GameLogic.ThrowDartTreble(currentPlayer, touchCount, score);

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
    }
}