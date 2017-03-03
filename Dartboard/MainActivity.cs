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
        
        Player testPlayer = new Player();
        Player player2 = new Player();



        List<Player> Players = new List<Player>();
       
        TextView d1;




        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            List<Player> Players = new List<Player>();

            
            //var player1 = (Player)Intent.GetParcelableExtra("player1");
            
            Players.Add(testPlayer);
            testPlayer.name = "1";
            testPlayer.turn = true;
            Players.Add(player2);
            player2.name = "2";
            player2.turn = false;

            string startScore = Intent.GetStringExtra("startScore");
            bool checkIn = Intent.GetBooleanExtra("isCheckIn", false);
            bool checkOut = Intent.GetBooleanExtra("isCheckOut", true);
            int numSets = Intent.GetIntExtra("numSets", 1);

            int gameScore = Convert.ToInt32(startScore);
            //player1.score = gameScore;

            d1 = FindViewById<TextView>(Resource.Id.dart1);
            //TextView d2 = FindViewById<TextView>(Resource.Id.dart2);
            //TextView d3 = FindViewById<TextView>(Resource.Id.dart3);


            //if (Intent.HasExtra("player2"))
            //{
            //    var player2 = (Player)Intent.GetParcelableExtra("player2");
            //    Players.Add(player2);
            //    player2.score = gameScore;
            //}

            // Setup the view 
            ImageView iv = (ImageView)FindViewById(Resource.Id.dartboard);
            iv.SetOnTouchListener(this);

         

        }



        int touchCount = 0;

        public bool OnTouch(View v, MotionEvent e)
        {
            Player currentPlayer  = GameLogic.WhosTurn(testPlayer, player2);
            var x = (int)e.GetX();
            var y = (int)e.GetY();
            switch (e.Action)
            {
                // add a touch count 
                case MotionEventActions.Down:
                    Console.WriteLine("getting x and y");
                    break;
                case MotionEventActions.Up:
                    touchCount++;
                    int touchColor = getColorHotspot(Resource.Id.dartboardoverlay, x, y);
                    Color myColor = new Color(touchColor);
                    score = board.ColorScores.FirstOrDefault(k => k.Value == myColor).Key;
                    Console.WriteLine(score);
                    GameLogic.ThrowDart(currentPlayer, touchCount, score, currentPlayer.score);
                    
                    string text = Convert.ToString(currentPlayer.score);

                    d1.Text = "Player: " + currentPlayer.name + " " + text;
                    Console.WriteLine(touchCount);
                    
                    break;
            }
            if (touchCount == 3)
            {
                touchCount = 0;
                GameLogic.SwitchPlayer(testPlayer, player2);
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

