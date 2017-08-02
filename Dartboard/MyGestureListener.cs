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
        public MyGestureListener(int score, Player CurrentPlayer, Context c, int touchCount, int previousTurn)
        {

        }

        public override bool OnSingleTapConfirmed(MotionEvent e)
        {
             var x = (int)e.GetX();
            var y = (int)e.GetY();
            GameLogic.ResetCounters(previousTurn, touchCount);
         
            Console.WriteLine(score);
            GameLogic.CheckBust(score, currentPlayer, c, touchCount);
            GameLogic.ThrowDart(currentPlayer, touchCount, score);
            return true;
        }

        public override bool OnDoubleTap(MotionEvent e)
        {
            Console.WriteLine("Double Tap");
            GameLogic.ResetCounters(previousTurn, touchCount);
            GameLogic.CheckBust(score, currentPlayer, c, touchCount);
            GameLogic.ThrowDartDouble(currentPlayer, touchCount, score);
            return base.OnDoubleTap(e);
        }

        public override void OnLongPress(MotionEvent e)
        {
            Console.WriteLine("Long Press");
            base.OnLongPress(e);
            GameLogic.ResetCounters(previousTurn, touchCount);
            GameLogic.CheckBust(score, currentPlayer, c, touchCount);
            GameLogic.ThrowDartDouble(currentPlayer, touchCount, score);

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