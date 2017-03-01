using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Dartboard
{
    [Activity(Label = "Dartboard", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, View.IOnTouchListener
    {


        int score;
        Board board = new Board();

        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            // Setup the view 
            ImageView iv = (ImageView)FindViewById(Resource.Id.dartboard);
            iv.SetOnTouchListener(this);

       

        }

        public bool OnTouch(View v, MotionEvent e)
        {
            var x = (int)e.GetX();
            var y = (int)e.GetY();
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    Console.WriteLine("getting x and y now ");
                    break;
                case MotionEventActions.Up:
                    int touchColor = getColorHotspot(Resource.Id.dartboardoverlay, x, y);
                    Color myColor = new Color(touchColor);
                    score = board.ColorScores.FirstOrDefault(k => k.Value == myColor).Key;
                    Toast toast = Toast.MakeText(this, String.Format("{0}",score), ToastLength.Short);
                    toast.Show();
                    break;
            }
            return true;
        }



        public int getColorHotspot(int hotspotId, int x, int y)
        {
            ImageView img = (ImageView)FindViewById(hotspotId);
            img.DrawingCacheEnabled = true;
            Bitmap hotspots = Bitmap.CreateBitmap(img.DrawingCache);
            img.DrawingCacheEnabled = false;
            return hotspots.GetPixel(x, y);
        }
    }
}

