using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace Dartboard
{
    [Activity(Label = "Dartboard", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, View.IOnTouchListener
    {

  
        int score;
        int previousTurn;

        Board board = new Board();
        
        Player testPlayer = new Player();
        Player player2 = new Player();
        Player currentPlayer;

        // Will collect from oncreate in next stage 
        //int numSets = 3;



        List<Player> Players = new List<Player>();
       
        TextView d1;
        TextView p2Score;
        
        TextView Checkout;
        TextView p2Checkout;

        Button undo;




        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            List<Player> Players = new List<Player>();

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

            d1 = FindViewById<TextView>(Resource.Id.dart1);
            Checkout = FindViewById<TextView>(Resource.Id.Checkout);

            p2Score = FindViewById<TextView>(Resource.Id.dart2);
            p2Checkout = FindViewById<TextView>(Resource.Id.Checkout2);

            testPlayer.ScoreBoard = d1;
            player2.ScoreBoard = p2Score;

            testPlayer.Checkout = FindViewById<TextView>(Resource.Id.Checkout);
            player2.Checkout = FindViewById<TextView>(Resource.Id.Checkout2);

            undo = (Button)FindViewById(Resource.Id.undo);


            //if (Intent.HasExtra("player2"))
            //{
            //    var player2 = (Player)Intent.GetParcelableExtra("player2");
            //    Players.Add(player2);
            //    player2.score = gameScore;
            //}

            // Setup the view 
            ImageView iv = (ImageView)FindViewById(Resource.Id.dartboard);
            iv.SetOnTouchListener(this);

            undo.Click += UndoButton;

         

        }



        int touchCount = 0;

        public bool OnTouch(View v, MotionEvent e)
        {
            
            currentPlayer  = GameLogic.WhosTurn(testPlayer, player2);
            if (GameLogic.IsWinner(currentPlayer))
            {
                GameLogic.ShowWinDialog(this, currentPlayer);
                return false;
            }

            

            var x = (int)e.GetX();
            var y = (int)e.GetY();
            switch (e.Action)
            {
                // add a touch count 
                case MotionEventActions.Down:
                    Console.WriteLine("getting x and y");
                    break;
                case MotionEventActions.Up:
                    previousTurn = touchCount;
                    touchCount++;
                    int touchColor = getColorHotspot(Resource.Id.dartboardoverlay, x, y);
                    Color myColor = new Color(touchColor);                   
                    score = board.ColorScores.FirstOrDefault(k => k.Value == myColor).Key;
                    undo.Enabled = true;
                    Console.WriteLine(score);
                    if (score > currentPlayer.score || currentPlayer.score - score == 1)
                    {
                        Toast toast = Toast.MakeText(this, "Bust", ToastLength.Short);
                        toast.SetGravity(GravityFlags.Center, 0, 0);
                        toast.Show();
                    }
                    else
                    {
                        GameLogic.ThrowDart(currentPlayer, touchCount, score);
                    }
                    string text = Convert.ToString(currentPlayer.score);

                    
                    Console.WriteLine(touchCount);

                    if (currentPlayer.score <= 170)
                    {
                        GameLogic.GetCheckout(currentPlayer, board, touchCount);  
                    }
                    else
                    {
                        currentPlayer.Checkout.Text = " ";
                    }
                    break;

                
            }
            if (currentPlayer.score > 0)
            {
                if (touchCount == 3)
                {
                    
                    touchCount = 0;
                    GameLogic.SwitchPlayer(testPlayer, player2);
                }
            }
            else if (GameLogic.IsWinner(currentPlayer))
            {
                GameLogic.ShowWinDialog(this, currentPlayer);
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

        public void UndoButton(object sender, EventArgs args)
        {
            currentPlayer.score += score;
            touchCount = previousTurn;
            string scoreBoard = string.Format("Player {0}: {1} Dart: {2}", currentPlayer.name, currentPlayer.score, touchCount);
            currentPlayer.ScoreBoard.Text = scoreBoard;
            undo.Enabled = false;
        }

        
    }
}

