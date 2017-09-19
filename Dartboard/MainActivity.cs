using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.App;

namespace Dartboard
{
    [Activity(Label = "Dartboard", MainLauncher = false, Icon = "@drawable/launcherIcon192x192", Theme = "@style/DartsAppStyle")]
    public class MainActivity : AppCompatActivity
    {

        ImageView img;
        public int score;
        int previousTurn;

        // default
        int legs = 0;
        int numLegs;
        public static Board board = new Board();

        
        
        internal static Player testPlayer = new Player();
        internal static Player player2 = new Player();
        Player currentPlayer;

        int startScore;

        internal List<Player> Players;
       
        TextView d1;
        TextView p2Score;
        
        TextView Checkout;
        TextView p2Checkout;

        internal static Button undo;

        Intent intent;
        Intent IntentReset;

        private GestureDetector _gestureDetector;



        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Players = new List<Player>();

            using (StreamReader sr = new StreamReader(Assets.Open("Checkouts.txt")))
            {
                char[] delim = { ':' };
                string line;
                while( (line = sr.ReadLine()) != null)
                {                    
                    string[] words = line.Split(delim);
                    int scoreValue;
                    int.TryParse(words[0], out scoreValue);
                    string bestCheckout = words[1];
                    board.Checkouts.Add(scoreValue, bestCheckout); 
                }
            }

            using (StreamReader sr = new StreamReader(Assets.Open("twoDartCheckouts.txt")))
            {
                char[] delim = { ':' };
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(delim);
                    int scoreValue;
                    int.TryParse(words[0], out scoreValue);
                    string bestCheckout = words[1];
                    board.TwoDartCheckouts.Add(scoreValue, bestCheckout);
                }
            }

            startScore = Intent.GetIntExtra("startingScore", 101);
            legs = Intent.GetIntExtra("numLegs", 1);
            numLegs = Intent.GetIntExtra("numLegs", 1);

            //List<string> playerNames = Intent.GetStringArrayListExtra("playerNames") as List<string>;

            string p1name = Intent.GetStringExtra("p1name");

            // Setup Player 1 
            Players.Add(testPlayer);
            testPlayer.name = p1name;
            testPlayer.turn = true;
            testPlayer.score = startScore;
            d1 = FindViewById<TextView>(Resource.Id.dart1);
            d1.Text = p1name;           
            Checkout = FindViewById<TextView>(Resource.Id.Checkout);
            testPlayer.ScoreBoard = d1;
            testPlayer.ScoreBoard.SetTextColor(Android.Graphics.Color.Red);
            testPlayer.Checkout = FindViewById<TextView>(Resource.Id.Checkout);


            // Setup Player 2 
            if (Intent.HasExtra("p2name"))
            {
                string p2name = Intent.GetStringExtra("p2name");
                player2.name = p2name;
                Players.Add(player2);
                player2.score = startScore;
                player2.turn = false;
                p2Score = FindViewById<TextView>(Resource.Id.dart2);
                p2Score.Text = p2name;
                p2Checkout = FindViewById<TextView>(Resource.Id.Checkout2);
                player2.ScoreBoard = p2Score;
                player2.Checkout = FindViewById<TextView>(Resource.Id.Checkout2);
            }
            
           
                                                         
               
            undo = (Button)FindViewById(Resource.Id.undo);
            undo.Enabled = false;
            IntentReset = Intent;
            intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop);

            // Setup the view 
            ImageView iv = (ImageView)FindViewById(Resource.Id.dartboard);
            iv.Touch += imageTouch;
            
            
        }
        
        protected override void OnResume()
        {
            base.OnResume();
            
            GC.Collect();
            currentPlayer = GameLogic.WhosTurn(testPlayer, player2);
            MyGestureListener myGestureListener = new MyGestureListener(score, currentPlayer, this, touchCount, previousTurn, legs, startScore, numLegs);
            _gestureDetector = new GestureDetector(this, myGestureListener);

            //GameLogic.GetCheckout(testPlayer, board, touchCount);
            //if (player2 != null)
            //{
            //    GameLogic.GetCheckout(player2, board, touchCount);
            //} // Was causing null reference exception for some reason 
            undo.Click += myGestureListener.UndoButton;

            //if (GameLogic.IsWinner(currentPlayer, legs, ))
            //{
            //    touchCount = 0;
            //    legs -= 1;
            //    currentPlayer.legsWon += 1;
            //    GameLogic.ShowWinDialog(this, currentPlayer, Players, IntentReset, legs, touchCount, startScore, numLegs);
            //}


        }



        int touchCount = 0;

        public void imageTouch(object sender, View.TouchEventArgs e)
        {
            _gestureDetector.OnTouchEvent(e.Event);
        }



        public int getColorHotspot(int hotspotId, int x, int y)
        {
            img = (ImageView)FindViewById(hotspotId);
            img.DrawingCacheEnabled = true;
            Bitmap hotspots = Bitmap.CreateBitmap(img.DrawingCache);
            img.DrawingCacheEnabled = false;
            return hotspots.GetPixel(x, y);
        }

        






    }
}

