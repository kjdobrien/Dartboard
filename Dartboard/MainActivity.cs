﻿//using Android.App;
//using Android.Widget;
//using Android.OS;
//using Android.Views;
//using Android.Graphics;
//using System;
//using System.IO;
//using System.Linq;
//using System.Collections.Generic;
//using Android.Content;

//namespace Dartboard
//{
//    [Activity(Label = "Dartboard", MainLauncher = false, Icon = "@drawable/dartboard", Theme = "@style/DartsAppStyle")]
//    public class MainActivity : Activity, View.IOnTouchListener
//    {


//        int score;
//        int previousTurn;

//        // default
//        int legs = 0;
//        int numLegs;
//        Board board = new Board();

//        Player player1 = new Player();
//        Player player2 = new Player();
//        Player currentPlayer;

//        int startScore;

//        List<Player> Players;

//        TextView d1;
//        TextView p2Score;

//        TextView Checkout;
//        TextView p2Checkout;

//        Button undo;

//        Intent intent;
//        Intent IntentReset;





//        protected override void OnCreate(Bundle bundle)
//        {
//            base.OnCreate(bundle);



//            // Set our view from the "main" layout resource
//            SetContentView(Resource.Layout.Main);

//            Players = new List<Player>();

//            using (StreamReader sr = new StreamReader(Assets.Open("Checkouts.txt")))
//            {
//                char[] delim = { ':' };
//                string line;
//                while ((line = sr.ReadLine()) != null)
//                {
//                    string[] words = line.Split(delim);
//                    int scoreValue;
//                    int.TryParse(words[0], out scoreValue);
//                    string bestCheckout = words[1];
//                    board.Checkouts.Add(scoreValue, bestCheckout);
//                }
//            }

//            using (StreamReader sr = new StreamReader(Assets.Open("twoDartCheckouts.txt")))
//            {
//                char[] delim = { ':' };
//                string line;
//                while ((line = sr.ReadLine()) != null)
//                {
//                    string[] words = line.Split(delim);
//                    int scoreValue;
//                    int.TryParse(words[0], out scoreValue);
//                    string bestCheckout = words[1];
//                    board.TwoDartCheckouts.Add(scoreValue, bestCheckout);
//                }
//            }

//            startScore = Intent.GetIntExtra("startingScore", 101);
//            legs = Intent.GetIntExtra("numLegs", 1);
//            numLegs = Intent.GetIntExtra("numLegs", 1);

//            //List<string> playerNames = Intent.GetStringArrayListExtra("playerNames") as List<string>;

//            string p1name = Intent.GetStringExtra("p1name");

//            // Setup Player 1 
//            Players.Add(player1);
//            player1.name = p1name;
//            player1.turn = true;
//            player1.score = startScore;
//            d1 = FindViewById<TextView>(Resource.Id.dart1);
//            d1.Text = p1name;
//            Checkout = FindViewById<TextView>(Resource.Id.Checkout);
//            //player1.ScoreBoard = d1;
//            //player1.ScoreBoard.SetTextColor(Android.Graphics.Color.Red);
//            player1.Checkout = FindViewById<TextView>(Resource.Id.Checkout);


//            // Setup Player 2 
//            if (Intent.HasExtra("p2name"))
//            {
//                string p2name = Intent.GetStringExtra("p2name");
//                player2.name = p2name;
//                Players.Add(player2);
//                player2.score = startScore;
//                player2.turn = false;
//                p2Score = FindViewById<TextView>(Resource.Id.dart2);
//                p2Score.Text = p2name;
//                p2Checkout = FindViewById<TextView>(Resource.Id.Checkout2);
//                player2.ScoreBoard = p2Score;
//                player2.Checkout = FindViewById<TextView>(Resource.Id.Checkout2);
//            }




//            undo = (Button)FindViewById(Resource.Id.undo);
//            undo.Enabled = false;
//            IntentReset = Intent;
//            intent = new Intent(this, typeof(MainActivity));
//            intent.SetFlags(ActivityFlags.ClearTop);
//            // Setup the view 
//            ImageView iv = (ImageView)FindViewById(Resource.Id.dartboard);
//            iv.SetOnTouchListener(this);

//            undo.Click += UndoButton;
//        }



//        int touchCount = 0;

//        public bool OnTouch(View v, MotionEvent e)
//        {

//            GC.Collect();
//            currentPlayer = GameLogic.WhosTurn(player1, player2);

//            if (GameLogic.IsWinner(currentPlayer))
//            {
//                touchCount = 0;
//                legs -= 1;
//                currentPlayer.legsWon += 1;
//                GameLogic.ShowWinDialog(this, currentPlayer, Players, IntentReset, legs, startScore, numLegs, this);

//                return false;
//            }



//            var x = (int)e.GetX();
//            var y = (int)e.GetY();


//            switch (e.Action)
//            {
//                // add a touch count 
//                case MotionEventActions.Down:
//                    Console.WriteLine("getting x and y");

//                    Console.WriteLine("x: " + x + ", y: " + y);
//                    break;

//                case MotionEventActions.Up:
//                    previousTurn = touchCount;
//                    touchCount++;
//                    int touchColor = getColorHotspot(Resource.Id.dartboardoverlay, x, y);
//                    Color myColor = new Color(touchColor);
//                    score = board.ColorScores.FirstOrDefault(k => k.Value == myColor).Key;
//                    undo.Enabled = true;
//                    Console.WriteLine(score);
//                    // Score checking 

//                    if (score > currentPlayer.score || currentPlayer.score - score == 1)
//                    {
//                        Toast toast = Toast.MakeText(this, "Bust", ToastLength.Short);
//                        toast.SetGravity(GravityFlags.Center, 0, 0);
//                        toast.Show();
//                    }
//                    else
//                    {
//                        GameLogic.ThrowDart(currentPlayer, touchCount, score);

//                    }
//                    string text = Convert.ToString(currentPlayer.score);


//                    Console.WriteLine(touchCount);
//                    Console.WriteLine("name: " + currentPlayer.name + " Legs won: " + currentPlayer.legsWon);
//                    Console.WriteLine("Current leg: " + legs);

//                    if (currentPlayer.score <= 170)
//                    {
//                        GameLogic.GetCheckout(currentPlayer, board, touchCount);
//                    }
//                    else
//                    {
//                        currentPlayer.Checkout.Text = " ";
//                    }
//                    break;




//            }

//            if (GameLogic.IsWinner(currentPlayer))
//            {

//                touchCount = 0;
//                legs -= 1;
//                currentPlayer.legsWon += 1;
//                GameLogic.ShowWinDialog(this, currentPlayer, Players, IntentReset, legs, startScore, numLegs, this);


//            }

//            if (currentPlayer.score > 0)
//            {
//                if (touchCount == 3)
//                {

//                    touchCount = 0;
//                    if (Players.Count > 1)
//                    {
//                        GameLogic.SwitchPlayer(player1, player2);
//                    }

//                }
//            }




//            return true;
//        }



//        public int getColorHotspot(int hotspotId, int x, int y)
//        {
//            ImageView img = (ImageView)FindViewById(hotspotId);
//            img.DrawingCacheEnabled = true;
//            Bitmap hotspots = Bitmap.CreateBitmap(img.DrawingCache);
//            img.DrawingCacheEnabled = false;
//            return hotspots.GetPixel(x, y);
//        }

//        public void UndoButton(object sender, EventArgs args)
//        {
//            currentPlayer.score += score;
//            touchCount = previousTurn;
//            string scoreBoard = string.Format("Player {0}: {1} Dart: {2}", currentPlayer.name, currentPlayer.score, touchCount);
//            currentPlayer.ScoreBoard.Text = scoreBoard;
//            undo.Enabled = false;
//        }


//    }
//}

