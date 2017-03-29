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
    [Activity(Label = "CreateGame", MainLauncher = true)]
    public class CreateGame : Activity
    {
        int startingScore;
        int numLegs;
        EditText nameEditText;
        List<string> items;
        ArrayAdapter<string> nameAdapter;
        ListView PlayerNames;
        Button StartGame;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CreateGame);

            // Start the Game button
            StartGame = FindViewById<Button>(Resource.Id.startGame);
            StartGame.Enabled = false;


            // Initialize listView
            PlayerNames = FindViewById<ListView>(Resource.Id.playerNames);
            items = new List<string> { };
            nameAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
            PlayerNames.Adapter = nameAdapter;

            // Get Player Name
            Button AddPlayer = FindViewById<Button>(Resource.Id.addPlayer);
            
           
            AddPlayer.Click += delegate 
            {
                AlertDialog.Builder addName = new AlertDialog.Builder(this);
                addName.SetView(Resource.Layout.NamePlayer);
                addName.SetPositiveButton("Enter", HandlePositiveButtonClick);
                Dialog nameDialog = addName.Create();
                nameDialog.Show();                                                    
                if (PlayerNames.Count > 1)
                {
                    AddPlayer.Enabled = false;
                    
                }

            };

            
            
            // Get Start Score
            Spinner selectStartScore = FindViewById<Spinner>(Resource.Id.startScoreSpinner);
            selectStartScore.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.startScoreArray, Android.Resource.Layout.SimpleSpinnerDropDownItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            selectStartScore.Adapter = adapter;

            // Get Number of legs
            Spinner legSpinner = FindViewById<Spinner>(Resource.Id.legsSpinner);
            legSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(legSpinner_ItemSelected);
            var legAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.legsArray, Android.Resource.Layout.SimpleSpinnerDropDownItem);   
            legAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            legSpinner.Adapter = legAdapter;

           

            StartGame.Click += delegate 
            {

                Console.WriteLine(startingScore);
                Console.WriteLine(numLegs);


                string p1name = items[0];              
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.PutStringArrayListExtra("playerNames", items);
                intent.PutExtra("p1name", p1name);
                intent.PutExtra("startingScore", startingScore);
                intent.PutExtra("numLegs", numLegs);
                if (items.Count > 1)
                {
                    string p2name = items[1];
                    intent.PutExtra("p2name", p2name);
                }
                
                StartActivity(intent); 
                
            };


            
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
            var dialog = (AlertDialog)sender;
            nameEditText = (EditText)dialog.FindViewById(Resource.Id.playerName);
            string name = nameEditText.Text;
            items.Add(name);         
            nameAdapter.Add(name);
            RunOnUiThread(() =>
            { 
                nameAdapter.NotifyDataSetChanged();
            });
            StartGame.Enabled = true;

        }
    }
}