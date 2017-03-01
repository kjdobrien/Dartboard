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
    [Activity(Label = "SelectPlayers", MainLauncher = false)]
    public class SelectPlayers : Activity
    {
        string selectedScore;
        bool isCheckin;
        bool isCheckout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectPlayers);

            Spinner selectStartScore = FindViewById<Spinner>(Resource.Id.startScoreSpinner);
            selectStartScore.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.startScoreArray, Android.Resource.Layout.SimpleSpinnerDropDownItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            selectStartScore.Adapter = adapter;


            // Double in Double out? 
            Switch doubleIn = (Switch)FindViewById(Resource.Id.checkin);
            Switch doubleOut = (Switch)FindViewById(Resource.Id.checkout);

            EditText numOfSetsTextBox = (EditText)FindViewById(Resource.Id.numSets);

            Button OnePlayer = FindViewById<Button>(Resource.Id.onePlayer);

            Button TwoPlayer = FindViewById<Button>(Resource.Id.twoPlayer);


            // Will fix all this wet code eventually, for now it works 

            OnePlayer.Click += delegate
                {
                    // create one player object 
                    Player player1 = new Player();

                    isCheckin = doubleIn.Checked;
                    isCheckout = doubleOut.Checked;
                    int numOfSet = Convert.ToInt32(numOfSetsTextBox.Text);

                    // add to intent/bundle
                    Intent onePlayerIntent = new Intent(this, typeof(MainActivity));
                    onePlayerIntent.PutExtra("player1", player1);
                    onePlayerIntent.PutExtra("startScore", selectedScore);
                    onePlayerIntent.PutExtra("isCheckIn", isCheckin);
                    onePlayerIntent.PutExtra("isCheckOut", isCheckout);
                    onePlayerIntent.PutExtra("numSets", numOfSet);
                   
                    // go to main activity
                    StartActivity(onePlayerIntent);
                };

            TwoPlayer.Click += delegate
                {
                    // Create two player objects 
                    Player player1 = new Player();
                    Player player2 = new Player();
                    isCheckin = doubleIn.Checked;
                    isCheckout = doubleOut.Checked;
                    int numOfSet = Convert.ToInt32(numOfSetsTextBox.Text);
                    // add to intent/bundle
                    Intent twoPlayerIntent = new Intent(this, typeof(MainActivity));
                    twoPlayerIntent.PutExtra("player1", player1);
                    twoPlayerIntent.PutExtra("player2", player2);
                    twoPlayerIntent.PutExtra("startScore", selectedScore);
                    twoPlayerIntent.PutExtra("player1", player1);
                    twoPlayerIntent.PutExtra("startScore", selectedScore);
                    twoPlayerIntent.PutExtra("isCheckIn", isCheckin);
                    twoPlayerIntent.PutExtra("isCheckOut", isCheckout);
                    twoPlayerIntent.PutExtra("numSets", numOfSet);

                    // go to main activity  
                    StartActivity(twoPlayerIntent);
                };
        }


        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            selectedScore = Convert.ToString(spinner.GetItemAtPosition(e.Position));

        }

    }
}