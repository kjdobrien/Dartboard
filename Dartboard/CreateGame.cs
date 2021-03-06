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
    [Activity(Label = "CreateGame", Theme="@style/DartsAppStyle")]
    public class CreateGame : Activity
    {
        int startingScore;
        int numLegs;
        EditText nameEditText;
        List<string> items;
        CustomListViewAdapter nameAdapter;
        ListView PlayerNames;
        ListView SuggestedNamesListView;
        List<string> SuggestedNames; 
        Button StartGame;
        Button AddPlayer;
        Button ResumeGame;
        ImageButton SettingsMenu;
        GameData gameData;
        Dialog nameDialog;
        TextView previousPlayersHint;
        ImageView brandLogo; 



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CreateGame);

            brandLogo = FindViewById<ImageView>(Resource.Id.icon);
            brandLogo.Visibility = ViewStates.Invisible; 


            // Initialize the settings menu 
            SettingsMenu = FindViewById<ImageButton>(Resource.Id.settingsButton);
            SettingsMenu.Click += SettingsMenu_Click;

            // Start the Game button
            StartGame = FindViewById<Button>(Resource.Id.startGame);
            StartGame.Enabled = false;

            ResumeGame = FindViewById<Button>(Resource.Id.resumeGame);

            SuggestedNames = HelperFunctions.GetNames();

            previousPlayersHint = FindViewById<TextView>(Resource.Id.previousPlayersHint);
            

            // Initialize listView
            PlayerNames = FindViewById<ListView>(Resource.Id.playerNames);
            items = new List<string> { };
            // Set up adapter 
            nameAdapter = new CustomListViewAdapter(this, items, Constants.ViewType.NameListItem); 
            PlayerNames.Adapter = nameAdapter;

            PlayerNames.ItemClick += PlayerNames_ItemClick;

            // Get Player Name
            AddPlayer = FindViewById<Button>(Resource.Id.addPlayer);

            AddPlayer.Click += AddPlayer_Click;
            StartGame.Click += StartGame_Click;

            

            // Get Start Score
            Spinner selectStartScore = FindViewById<Spinner>(Resource.Id.startScoreSpinner);
            selectStartScore.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            List<string> list1 = new List<string>();
            list1 = Resources.GetStringArray(Resource.Array.startScoreArray).ToList();
            selectStartScore.Adapter  = new ArrayAdapter(this, Resource.Layout.spinnerItem, Resource.Id.itemText, list1);
            selectStartScore.SetSelection(5);

            // Get Number of legs
            Spinner legSpinner = FindViewById<Spinner>(Resource.Id.legsSpinner);
            legSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(legSpinner_ItemSelected);
            List<string> list2 = new List<string>();
            list2 = Resources.GetStringArray(Resource.Array.legsArray).ToList();
            legSpinner.Adapter = new ArrayAdapter(this, Resource.Layout.spinnerItem, Resource.Id.itemText, list2);
            legSpinner.SetSelection(4);

            gameData = HelperFunctions.CheckForPreviousGame();
            if (gameData.player1Name != "" && (Convert.ToInt32(gameData.player1Score)  > 0  || Convert.ToInt32(gameData.player2Score) > 0))
            {
                ResumeGame.Visibility = ViewStates.Visible;
                previousPlayersHint.Visibility = ViewStates.Visible;

                string hintText = "{0} {1} V {2} {3}";

                previousPlayersHint.Text = String.Format(hintText, gameData.player1Name, gameData.player1Score, gameData.player2Score, gameData.player2Name);
                ResumeGame.Click += ResumeGame_Click;
            }


            
        }

        private void SuggestedNamesListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            SuggestedNames =  HelperFunctions.DeleteName(SuggestedNames, SuggestedNames[e.Position]);            
                        
        }

        private void PlayerNames_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            items.Remove(items[e.Position]);
            nameAdapter = new CustomListViewAdapter(this, items, Constants.ViewType.NameListItem);
            PlayerNames.Adapter = nameAdapter;

            if (PlayerNames.Count < 2)
            {
                AddPlayer.Enabled = true;
                StartGame.Enabled = false; 
            }
            

        }

        private void SettingsMenu_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SettingsActivity));
            StartActivity(intent); 
        }

        private void SuggestedNamesListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string name = SuggestedNames[e.Position];
            items.Add(name);
            nameAdapter = new CustomListViewAdapter(this, items, Constants.ViewType.NameListItem);
            PlayerNames.Adapter = nameAdapter;
            if (PlayerNames.Count == 2)
            {
                AddPlayer.Enabled = false;
                StartGame.Enabled = true;
            }
            
            nameDialog.Cancel(); 
        }

        private void SelectStartScore_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {

        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            Console.WriteLine(startingScore);
            Console.WriteLine(numLegs);


            string p1name = items[0];
            Intent intent = new Intent(this, typeof(GameViewWithKeyboard));
            intent.PutStringArrayListExtra("playerNames", items);
            intent.PutExtra("p1name", p1name);
            intent.PutExtra("startingScore", startingScore);
            intent.PutExtra("numLegs", numLegs);
            intent.PutExtra("gameResumed", false);
            if (items.Count > 1)
            {
                string p2name = items[1];
                intent.PutExtra("p2name", p2name);
            }

            HelperFunctions.SavePlayerName(items); 

            StartActivity(intent);
        }

        private void AddPlayer_Click(object sender, EventArgs e)
        {                         
            var addName = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogCustom);
            addName.SetView(Resource.Layout.NamePlayer);
            
            addName.SetPositiveButton("Enter", HandlePositiveButtonClick);
            nameDialog = addName.Create();          
            nameDialog.Show();

            if (SuggestedNames != null)
            {
                SuggestedNamesListView = (ListView)nameDialog.FindViewById(Resource.Id.playerNameList);
                SuggestedNamesListView.Adapter = new CustomListViewAdapter(this, SuggestedNames, Constants.ViewType.SuggestedName);
                SuggestedNamesListView.ItemClick += SuggestedNamesListView_ItemClick;
                SuggestedNamesListView.ItemLongClick += SuggestedNamesListView_ItemLongClick;

            }
        }

        private void ResumeGame_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(GameViewWithKeyboard));
            intent.PutStringArrayListExtra("playerNames", items);
            intent.PutExtra("p1name", gameData.player1Name);
            intent.PutExtra("p2name", gameData.player2Name);
            intent.PutExtra("p1Score", gameData.player1Score);
            intent.PutExtra("p2Score", gameData.player2Score);
            intent.PutExtra("p1Turn", gameData.player1Turn);
            intent.PutExtra("p2Turn", gameData.player2Turn);
            intent.PutExtra("legsPlayed", gameData.LegsPlayed);
            intent.PutExtra("legsToPlay", gameData.LegsToPlay);
            intent.PutExtra("gameResumed", true); 

            StartActivity(intent);
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string startScore =  Convert.ToString(spinner.GetItemAtPosition(e.Position));
            startingScore = Convert.ToInt32(startScore);

        }

        private void legSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string SnumLegs = Convert.ToString(spinner.GetItemAtPosition(e.Position));
            numLegs = Convert.ToInt32(SnumLegs);

        }

        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (Android.Support.V7.App.AlertDialog)sender;
            nameEditText = (EditText)dialog.FindViewById(Resource.Id.enterPlayerName);
            string name = nameEditText.Text;
            if (string.IsNullOrEmpty(name))
            {
                //var error = GetDrawable(Resource.Drawable.baseline_error_black_18dp);
                //nameEditText.SetError("Enter a name", error);

                HelperFunctions.DartsToast(this, "Enter a name", ToastLength.Long).Show();


            }
            else if (SuggestedNames != null  && SuggestedNames.Contains(name))
            {
                HelperFunctions.DartsToast(this, "Name already exists", ToastLength.Long).Show();
            }
            else
            {
                items.Add(name);
                nameAdapter = new CustomListViewAdapter(this, items, Constants.ViewType.NameListItem);
                PlayerNames.Adapter = nameAdapter;
                if (PlayerNames.Count == 2)
                {
                    AddPlayer.Enabled = false;

                }
                StartGame.Enabled = true;
            }
         
        }
    }
}