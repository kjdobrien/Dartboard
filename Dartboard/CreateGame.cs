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
        GameData gameData;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CreateGame);

            // Start the Game button
            StartGame = FindViewById<Button>(Resource.Id.startGame);
            StartGame.Enabled = false;

            ResumeGame = FindViewById<Button>(Resource.Id.resumeGame);

            //SuggestedNames = HelperFunctions.GetNames();

            // Initialize listView
            PlayerNames = FindViewById<ListView>(Resource.Id.playerNames);
            items = new List<string> { };
            nameAdapter = new CustomListViewAdapter(this, items); 
            PlayerNames.Adapter = nameAdapter;

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

            // Get Number of legs
            Spinner legSpinner = FindViewById<Spinner>(Resource.Id.legsSpinner);
            legSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(legSpinner_ItemSelected);
            List<string> list2 = new List<string>();
            list2 = Resources.GetStringArray(Resource.Array.legsArray).ToList();
            legSpinner.Adapter = new ArrayAdapter(this, Resource.Layout.spinnerItem, Resource.Id.itemText, list2); 

            



            


            gameData = HelperFunctions.CheckForPreviousGame();
            if (gameData.player1Name != "")
            {
                ResumeGame.Visibility = ViewStates.Visible;

                ResumeGame.Click += ResumeGame_Click;
            }


            
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

            StartActivity(intent);
        }

        private void AddPlayer_Click(object sender, EventArgs e)
        {                         
            var addName = new Android.Support.V7.App.AlertDialog.Builder(this);
            addName.SetView(Resource.Layout.NamePlayer);
            
            addName.SetPositiveButton("Enter", HandlePositiveButtonClick);
            Dialog nameDialog = addName.Create();
            ///SuggestedNamesListView = (ListView)nameDialog.FindViewById(Resource.Id.enterPlayerName);
            ///SuggestedNamesListView.Adapter = new CustomListViewAdapter(this, SuggestedNames);
            nameDialog.Show();
         
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
            items.Add(name);
            nameAdapter = new CustomListViewAdapter(this, items);
            PlayerNames.Adapter = nameAdapter;
            //RunOnUiThread(() =>
            //{ 
            //    nameAdapter.NotifyDataSetChanged();
            //});
            if (PlayerNames.Count == 2)
            {
                AddPlayer.Enabled = false;

            }
            StartGame.Enabled = true;

        }
    }
}