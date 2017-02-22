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
    [Activity(Label = "SelectPlayers")]
    public class SelectPlayers : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SelectPlayers);

            Button OnePlayer = FindViewById<Button>(Resource.Id.onePlayer);

            Button TwoPlayer = FindViewById<Button>(Resource.Id.twoPlayer);

            OnePlayer.Click += delegate
                {
                    // create one player object 
                    Player player1 = new Player();

                    // add to intent/bundle
                    Intent onePlayerIntent = new Intent(this, typeof());
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
                    Intent twoPlayerIntent = new Intent(this, typeof());
                    twoPlayerIntent.PutExtra("player1", player1);
                    twoPlayerIntent.PutExtra("player2", player2);
                    // go to setup activity  
                    StartActivity(twoPlayerIntent);
                };
        }


    }
}