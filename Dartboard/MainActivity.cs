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
    [Activity(Label = "Dartboard", MainLauncher = false, Icon = "@drawable/icon")]
    public class MainActivity : Activity, View.IOnTouchListener
    {
        
        int total = 301;
        Board board = new Board();


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var player1 = (Player)Intent.GetParcelableExtra("player1");
            player1.name = "this is me";
            
            if (Intent.HasExtra("player2"))
             {
                var player2 = (Player)Intent.GetParcelableExtra("player2");
                player2.name = "me too thanks";
             }

            string startScore = Intent.GetStringExtra("startScore");
            bool checkIn = Intent.GetBooleanExtra("isCheckIn", false);
            bool checkOut = Intent.GetBooleanExtra("isCheckOut", true);
            int numSets = Intent.GetIntExtra("numSets", 1);

            int gameScore = Convert.ToInt32(startScore);

            // Setup the view 
            ImageView iv = (ImageView)FindViewById(Resource.Id.dartboard);
            iv.SetOnTouchListener(this);
            
            // Start the game loop

            while (gameScore > 0)
            {
                // game logic

                
            }
            // Play again ?

                   
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
                    int score = board.ColorScores.FirstOrDefault(k => k.Value == myColor).Key;


                    Console.WriteLine("X = " + x + "Y = " + y + "and the color is: " + myColor);
                    total -= score;
                    Console.WriteLine("Score = " + score);
                    Console.WriteLine(total + " left");

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

