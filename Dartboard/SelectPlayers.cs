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
    [Activity(Label = "SelectPlayers", MainLauncher = true)]
    public class SelectPlayers : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectPlayers);

            Spinner selectStartScore = FindViewById<Spinner>(Resource.Id.startScoreSpinner);
            selectStartScore.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.startScoreArray, Android.Resource.Layout.SimpleSpinnerDropDownItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            selectStartScore.Adapter = adapter;

            

            Button OnePlayer = FindViewById<Button>(Resource.Id.onePlayer);

            Button TwoPlayer = FindViewById<Button>(Resource.Id.twoPlayer);

            OnePlayer.Click += delegate
                {
                    // create one player object 
                    Player player1 = new Player();
                 
                    // add to intent/bundle
                    Intent onePlayerIntent = new Intent(this, typeof(MainActivity));
                    onePlayerIntent.PutExtra("player1", player1);
                    // go to setup activity
                    StartActivity(onePlayerIntent);
                };

            TwoPlayer.Click += delegate
                {
                    // Create two player objects 
                    Player player1 = new Player();
                    Player player2 = new Player();
                    // add to intent/bundle
                    Intent twoPlayerIntent = new Intent(this, typeof(MainActivity));
                    twoPlayerIntent.PutExtra("player1", player1);
                    twoPlayerIntent.PutExtra("player2", player2);
                    // go to setup activity  
                    StartActivity(twoPlayerIntent);
                };
        }


        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            string toast = string.Format("The planet is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();
        }

    }
}