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
                    // add to intent/bundle
                    // go to setup activity
                };

            TwoPlayer.Click += delegate
                {
                    // Create two player objects 
                    // add to intent/bundle
                    // go to setup activity  
                };
        }


    }
}