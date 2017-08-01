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
    public class MyGestureListener : GestureDetector.SimpleOnGestureListener
    {

        int score;
        Player currentPlayer;
        Context c;
        int touchCount;
        public MyGestureListener(int score, Player CurrentPlayer, Context c, int touchCount)
        {

        }

        public override bool OnSingleTapConfirmed(MotionEvent e)
        {
            GameLogic.ResetCounters(touchCount)
            GameLogic.CheckBust(score, currentPlayer, c, touchCount);
            GameLogic.ThrowDart(currentPlayer, touchCount, score);
            return true;
        }

        public override bool OnDoubleTap(MotionEvent e)
        {
            Console.WriteLine("Double Tap");
            GameLogic.CheckBust(score, currentPlayer, c, touchCount);
            GameLogic.ThrowDartDouble(currentPlayer, touchCount, score);
            return base.OnDoubleTap(e);
        }

        public override void OnLongPress(MotionEvent e)
        {
            Console.WriteLine("Long Press");
            base.OnLongPress(e);
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