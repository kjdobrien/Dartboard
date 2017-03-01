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

        bool onScore = false;
        int score;
        Board board = new Board();
        int display;
        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            List<Player> Players = new List<Player>();

          
            var player1 = (Player)Intent.GetParcelableExtra("player1");
            
            Players.Add(player1);
                                       
            string startScore = Intent.GetStringExtra("startScore");
            bool checkIn = Intent.GetBooleanExtra("isCheckIn", false);
            bool checkOut = Intent.GetBooleanExtra("isCheckOut", true);
            int numSets = Intent.GetIntExtra("numSets", 1);

            int gameScore = Convert.ToInt32(startScore);
            player1.score = gameScore;

            TextView displayTv = FindViewById<TextView>(Resource.Id.displayScore);

            if (Intent.HasExtra("player2"))
            {
                var player2 = (Player)Intent.GetParcelableExtra("player2");
                Players.Add(player2);
                player2.score = gameScore;
            }

            // Setup the view 
            ImageView iv = (ImageView)FindViewById(Resource.Id.dartboard);
            iv.SetOnTouchListener(this);

            while (player1.score > 0)
            {
                foreach (Player p in Players)
                {
                    onScore = true;
                    foreach (int d in p.Darts)
                    {
                        displayTv.Text = Convert.ToString(display);
                    }
                }
            }

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
                    display = score;

                   // Console.WriteLine("X = " + x + "Y = " + y + "and the color is: " + myColor);
                    
                   // Console.WriteLine("Score = " + score);
                  //  Console.WriteLine(total + " left");

                    break;
            }
            return onScore;
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

