using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.InputMethodServices;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang;
using static Android.Views.View;

namespace Dartboard
{
    [Activity(Label = "GameViewWithKeyboard", WindowSoftInputMode = SoftInput.StateAlwaysHidden)]
    public class GameViewWithKeyboard : Activity
    {
        private EditText ScoreEditText;

        private TextView player1Name;
        private TextView player2Name;
        private TextView player1Score;
        private TextView player2Score;
        private TextView player1Checkout;
        private TextView player2Checkout;
        int startScore;
        int legs;
        int score;
        Board board;


        private Player Player1 = new Player();
        private Player Player2 = new Player(); 


        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GameViewWithKeyPad);

            board = new Board();

            player1Name = FindViewById<TextView>(Resource.Id.player1Name);
            player2Name = FindViewById<TextView>(Resource.Id.player2Name);
            player1Score = FindViewById<TextView>(Resource.Id.player1Score);
            player2Score = FindViewById<TextView>(Resource.Id.player2Score);
            player1Checkout = FindViewById<TextView>(Resource.Id.player1CheckOut);
            player2Checkout = FindViewById<TextView>(Resource.Id.player2CheckOut);

            ScoreEditText = FindViewById<EditText>(Dartboard.Resource.Id.scoreEditText);
            ScoreEditText.EditorAction += ScoreEditText_EditorAction;

            Player1.name = Intent.GetStringExtra("p1name");
            Player2.name = Intent.GetStringExtra("p2name");
            startScore = Intent.GetIntExtra("startingScore", 101);
            legs = Intent.GetIntExtra("numLegs", 1);

            player1Score.Text = startScore.ToString(); 
            player2Score.Text = startScore.ToString();

            player1Name.Text = Player1.name;
            player2Name.Text = Player2.name; 

            Player1.score = startScore;
            Player2.score = startScore; 



            // Get checkouts 
            using (StreamReader sr = new StreamReader(Assets.Open("Checkouts.txt")))
            {
                char[] delim = { ':' };
                string line;
                while ((line = sr.ReadLine()) != null)
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

            // Set checkouts if playing 101 
            if (startScore == 101)
            {
                string value;
                board.Checkouts.TryGetValue(101, out value);
                player1Checkout.Text = value;
                player1Checkout.Visibility = ViewStates.Visible;
                player2Checkout.Text = value;
                player2Checkout.Visibility = ViewStates.Visible;
            }

            // Set up keyboard 
            Keyboard kbd = new Keyboard(this, Resource.Drawable.dartsNumberPad);
            KeyboardView kbv = FindViewById<KeyboardView>(Resource.Id.customKBD);
            kbv.Keyboard = kbd;
            var myKeyboardListener = new KeyboardListener(this);
            kbv.OnKeyboardActionListener = myKeyboardListener;
            kbv.Key += (sender, e) => {
                long eventTime = JavaSystem.CurrentTimeMillis();
                KeyEvent ev = new KeyEvent(eventTime, eventTime, KeyEventActions.Down, e.PrimaryCode, 0, 0, 0, 0, KeyEventFlags.SoftKeyboard | KeyEventFlags.KeepTouchMode);

                this.DispatchKeyEvent(ev);
            };


            Player1.turn = true;
            Player2.turn = false; 



        }

        private void ScoreEditText_EditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            score = Convert.ToInt32(ScoreEditText.Text);
            if (Player1.turn)
            {
                
                validateScore(score, Player1);
                player1Score.Text = Player1.score.ToString();
                if (GameLogic.IsCheckout(Player1))
                {
                    player1Checkout.Visibility = ViewStates.Visible;
                    player1Checkout.Text = GameLogic.GetCheckout(Player1, board);
                }
                else
                {
                    player1Checkout.Text = "";
                    player1Checkout.Visibility = ViewStates.Invisible; 
                }
                clearScore();
                
            }
            else
            {
                player2Checkout.Text = ""; 
                validateScore(score, Player2);
                player2Score.Text = Player2.score.ToString();
                if (GameLogic.IsCheckout(Player1))
                {
                    player2Checkout.Visibility = ViewStates.Visible;
                    player2Checkout.Text = GameLogic.GetCheckout(Player2, board);
                }
                else
                {
                    player2Checkout.Text = "";
                    player2Checkout.Visibility = ViewStates.Invisible; 
                }
                clearScore();
            }
            GameLogic.SwitchPlayer(Player1, Player2); 
            
        }

        private void validateScore(int score, Player p)
        {
            if (score > p.score || p.score - score == 1)
            {
                Toast toast = Toast.MakeText(this, "Bust", ToastLength.Short);
                toast.SetGravity(GravityFlags.Center, 0, 0);
                toast.Show();

            }
            else
            {
                GameLogic.ThrowDart(p, score);
            }
        }

        private void clearScore()
        {
            ScoreEditText.Text = "";
            score = 0;
        }

    }
}