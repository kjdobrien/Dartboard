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
    [Activity(Label = "GameViewWithKeyboard", WindowSoftInputMode = SoftInput.StateAlwaysHidden, Theme = "@style/DartsAppStyle")]
    public class GameViewWithKeyboard : Activity
    {
        private ImageButton BackArrow; 
        private EditText ScoreEditText;

        private TextView player1Name;
        private TextView player2Name;
        public TextView player1Score;
        public TextView player2Score;
        public TextView player1Checkout;
        public TextView player2Checkout;
        public LinearLayout Player1Layout;
        public LinearLayout Player2Layout;
        public ListView Player1ScoreList;
        public ListView Player2ScoreList;
        int startScore;
        int legsPlayed;
        int legsToPlay; 
        int score;
        int previousScore; 
        Board board;
        Intent IntentReset;
        bool undoEnabled;

        private List<Player> Players = new List<Player>();
        private Player Player1 = new Player();
        private Player Player2 = new Player(); 


        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GameViewWithKeyPad);

            board = new Board();

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

            BackArrow = FindViewById<ImageButton>(Resource.Id.backButton); 

            player1Name = FindViewById<TextView>(Resource.Id.player1Name);
            player2Name = FindViewById<TextView>(Resource.Id.player2Name);
            player1Score = FindViewById<TextView>(Resource.Id.player1Score);
            player2Score = FindViewById<TextView>(Resource.Id.player2Score);
            player1Checkout = FindViewById<TextView>(Resource.Id.player1CheckOut);
            player2Checkout = FindViewById<TextView>(Resource.Id.player2CheckOut);
            Player1Layout = FindViewById<LinearLayout>(Resource.Id.player1Details);
            Player2Layout = FindViewById<LinearLayout>(Resource.Id.player2Details);
            Player1ScoreList = FindViewById<ListView>(Resource.Id.player1ScoreList);
            Player2ScoreList = FindViewById<ListView>(Resource.Id.player2ScoreList); 

            ScoreEditText = FindViewById<EditText>(Dartboard.Resource.Id.scoreEditText);
            ScoreEditText.EditorAction += ScoreEditText_EditorAction;

            Player1.name = Intent.GetStringExtra("p1name");
            Player2.name = Intent.GetStringExtra("p2name");

            if (Intent.HasExtra("gameResumed") && Intent.GetBooleanExtra("gameResumed", false) == true)
            {
                Player1.score = Convert.ToInt32( Intent.GetStringExtra("p1Score"));
                Player2.score = Convert.ToInt32(Intent.GetStringExtra("p2Score"));
                Player1.turn = Convert.ToBoolean(Intent.GetStringExtra("p1Turn"));
                Player2.turn = Convert.ToBoolean(Intent.GetStringExtra("p2Turn"));
                legsPlayed = Convert.ToInt32(Intent.GetStringExtra("legsPlayed"));
                legsToPlay = Convert.ToInt32(Intent.GetStringExtra("legToPlay"));

                player1Score.Text = Player1.score.ToString();
                player2Score.Text = Player2.score.ToString();

                if (Player1.score <= 170 || Player2.score <= 170)
                {
                    player1Checkout.Text = GameLogic.GetCheckout(Player1, board);
                    player1Checkout.Visibility = ViewStates.Visible;              
                    player2Checkout.Text = GameLogic.GetCheckout(Player2, board);
                    player2Checkout.Visibility = ViewStates.Visible; 
                }





            }
            else
            {
                startScore = Intent.GetIntExtra("startingScore", 101);
                legsToPlay = Intent.GetIntExtra("numLegs", 1);

                player1Score.Text = startScore.ToString();
                player2Score.Text = startScore.ToString();
                Player1.score = startScore;
                Player2.score = startScore;
                Player1.turn = true;
                Player2.turn = false;
            }

            player1Name.Text = Player1.name;
            player2Name.Text = Player2.name;

            player1Score.TextChanged += Player1Score_TextChanged;
            player2Score.TextChanged += Player2Score_TextChanged;

            if (Player1.turn)
            {
                Player1Layout.SetBackgroundColor(Android.Graphics.Color.Rgb(236, 229, 240));
                Player1Layout.Background.SetAlpha(50);
            }
            else
            {
                Player2Layout.SetBackgroundColor(Android.Graphics.Color.Rgb(236, 229, 240));
                Player2Layout.Background.SetAlpha(50);

            }

            



            Players.Add(Player1);
            Players.Add(Player2);



            BackArrow.Click += BackArrow_Click;



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
            kbv.PreviewEnabled = false; 
            kbv.OnKeyboardActionListener = myKeyboardListener;
            kbv.Key += (sender, e) => {
                long eventTime = JavaSystem.CurrentTimeMillis();
                KeyEvent ev = new KeyEvent(eventTime, eventTime, KeyEventActions.Down, e.PrimaryCode, 0, 0, 0, 0, KeyEventFlags.SoftKeyboard | KeyEventFlags.KeepTouchMode);

                if (ev.KeyCode.ToString() == "55001")
                {
                    if (undoEnabled)
                    {
                        if (Player1.turn)
                        {
                            Player2.score += previousScore;
                            player2Score.Text = Player2.score.ToString();
                        }
                        else
                        {
                            Player1.score += previousScore;
                            player1Score.Text = Player1.score.ToString();
                        }
                        undoEnabled = false; 
                        GameLogic.SwitchPlayer(Player1, Player2, this);
                    }
                    else
                    {
                        this.DispatchKeyEvent(ev);
                    }
                }
                else
                {
                    this.DispatchKeyEvent(ev);
                }
                
            };


            

           

        }

        private void SetUpIntentReset(string player1Name, string player2Name, string startScore, string legsToPlay)
        {
            IntentReset = new Intent(this, typeof(GameViewWithKeyboard));
            IntentReset.PutExtra("p1name", player1Name);
            IntentReset.PutExtra("p2name", player2Name);
            IntentReset.PutExtra("startingScore", startScore);
            IntentReset.PutExtra("numLegs", legsToPlay);
            IntentReset.PutExtra("gameResumed", false);
        }

        private void BackArrow_Click(object sender, EventArgs e)
        {
            ReturnToMain();
        }

        private void ReturnToMain()
        {
            if (Player1.score != 0 && Player2.score != 0)
            {
                GameLogic.SaveGameData(Player1, Player2, legsPlayed, legsToPlay);
            }
            else
            {
                HelperFunctions.DeleteSaveFile(Constants.PerviousGameFile);
            }
            var alert = new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle("Back to Main Menu?");
            alert.SetPositiveButton("Yes", (senderAlert, args) => { Intent intent = new Intent(this, typeof(CreateGame)); this.StartActivity(intent); });
            alert.SetNegativeButton("No", (senderAlert, args) => { alert.Dispose(); });
            Dialog dialog = alert.Create();
            dialog.Show();
        }


        private void Player2Score_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (GameLogic.IsCheckout(Player2))
            {
                player1Checkout.Visibility = ViewStates.Visible;
                player1Checkout.Text = GameLogic.GetCheckout(Player1, board);
            }
            else
            {
                player2Checkout.Text = "";
                player2Checkout.Visibility = ViewStates.Invisible;
            }
        }

        private void Player1Score_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
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
        }

        private void ScoreEditText_EditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            score = Convert.ToInt32(ScoreEditText.Text);
            if (Player1.turn)
            {

                if (validateScore(score, Player1))
                {
                    player1Score.Text = Player1.score.ToString();                    
                    if (GameLogic.IsCheckout(Player1))
                    {
                        player1Checkout.Visibility = ViewStates.Visible;
                        player1Checkout.Text = GameLogic.GetCheckout(Player1, board);
                        Player1.Checkout = player1Checkout.Text; 
                    }
                    else
                    {
                        player1Checkout.Text = "";
                        player1Checkout.Visibility = ViewStates.Invisible;
                    }
                    clearScore();
                    GameLogic.SwitchPlayer(Player1, Player2, this);
                }
                else
                {
                    return;
                }
            }
            else
            {
                player2Checkout.Text = "";
                if (validateScore(score, Player2))
                {

                    player2Score.Text = Player2.score.ToString();
                    if (GameLogic.IsCheckout(Player2))
                    {
                        player2Checkout.Visibility = ViewStates.Visible;
                        player2Checkout.Text = GameLogic.GetCheckout(Player2, board);
                        Player2.Checkout = player2Checkout.Text; 
                    }
                    else
                    {
                        player2Checkout.Text = "";
                        player2Checkout.Visibility = ViewStates.Invisible;
                    }
                    clearScore();
                    GameLogic.SwitchPlayer(Player1, Player2, this);
                }
                else
                {
                    return;
                }
            }
            
            GameLogic.SaveGameData(Player1, Player2, legsPlayed, legsToPlay);
        }

        private bool validateScore(int score, Player p)
        {
            if (score <= 180)
            {
                if (score > p.score | p.score - score == 1 | (p.score - score == 0 && p.Checkout == " No checkout") )
                {
                    HelperFunctions.DartsToast(this, "Bust", ToastLength.Short).Show();                    
                }
                else
                {
                    GameLogic.ThrowDart(p, score);
                    previousScore = score;
                    undoEnabled = true; 
                    if (GameLogic.IsWinner(p))
                    {
                        p.legsWon += 1;
                        SetUpIntentReset(Player1.name, Player2.name, startScore.ToString(), legsToPlay.ToString()); 
                        GameLogic.ShowWinDialog(this, p, Players, IntentReset, legsPlayed, startScore, legsToPlay, this);

                    }
                    
                }
                return true;
            }
            else
            {
                Toast toast = Toast.MakeText(this, "Maximum Score is 180", ToastLength.Short);
                toast.SetGravity(GravityFlags.Center, 0, 0);
                toast.Show();
                clearScore();
                return false; 
            }
        }

        private void clearScore()
        {
            ScoreEditText.Text = "";
            score = 0;
        }

        public override void OnBackPressed()
        {
            ReturnToMain();
        }

    }
}