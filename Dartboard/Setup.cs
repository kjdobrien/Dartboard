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
    [Activity(Label = "Setup", MainLauncher = true)]
    public class Setup : Activity
    {

        bool isCheckin;
        bool isCheckout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Setup);

            // Get the number of players
            EditText numPlayers = (EditText)FindViewById(Resource.Id.numPlayers);
            
            // Double in Double out? 
            Switch doubleIn = (Switch)FindViewById(Resource.Id.checkin);
            Switch doubleOut = (Switch)FindViewById(Resource.Id.checkout);
                  
            // How many Sets
            EditText numOfSetsTextBox = (EditText)FindViewById(Resource.Id.numSets);

            // Send to MainActivity
            Button submit = FindViewById<Button>(Resource.Id.submit);

            submit.Click += delegate
                {
                    int numOfPlayers = Convert.ToInt32(numPlayers.Text);
                    isCheckin = doubleIn.Checked;
                    isCheckout = doubleOut.Checked;
                    int numOfSet = Convert.ToInt32(numOfSetsTextBox.Text);

                    var setupIntent = new Intent(this, typeof(MainActivity));
                    setupIntent.PutExtra("numplayers", numOfPlayers);
                    setupIntent.PutExtra("isCheckIn", isCheckin);
                    setupIntent.PutExtra("isCheckOut", isCheckout);
                    setupIntent.PutExtra("numSets", numOfSet);

                    StartActivity(setupIntent);              
                };

        }

   
    
    }
}