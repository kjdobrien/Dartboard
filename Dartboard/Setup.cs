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
        int playerRadioSelect;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Get the number of players
            RadioGroup playersRadioGroup = FindViewById<RadioGroup>(Resource.Id.numplayers); 
                      
            
            // Get the game Starting Score
            NumberPicker StartScores = (NumberPicker)FindViewById(Resource.Id.startScores);
            string[] ScoreValues = new string[] { "101", "170", "201", "301", "401", "501", "601", "701", "801", "901", "1001" };
            StartScores.SetDisplayedValues(ScoreValues);

            // Double in Double out? 
            Switch doubleIn = (Switch)FindViewById(Resource.Id.checkin);
            bool isCheckin = doubleIn.Checked;

            Switch doubleOut = (Switch)FindViewById(Resource.Id.checkout);
            bool isCheckout = doubleOut.Checked;

            // How many Sets
            EditText numOfSetsTextBox = (EditText)FindViewById(Resource.Id.numSets);
            int numOfSet = Convert.ToInt32(numOfSetsTextBox.Text);

            // Send to MainActivity
            Button submit = FindViewById<Button>(Resource.Id.submit);

            submit.Click += delegate
            {
                RadioButton checkedRadioButton = FindViewById<RadioButton>(playersRadioGroup.CheckedRadioButtonId);
                int playerRadioSelect = Convert.ToInt32(checkedRadioButton.Text);

                
                int startScore = Convert.ToInt32(StartScores.Value);


                var setupIntent = new Intent(this, typeof(Setup));
                setupIntent.PutExtra("numplayers", playerRadioSelect);
                setupIntent.PutExtra("startScore", startScore);
                setupIntent.PutExtra("isCheckIn", isCheckin);
                setupIntent.PutExtra("isCheckOut", isCheckout);
                setupIntent.PutExtra("numSets", numOfSet);

                StartActivity(typeof(MainActivity));


                             
            };

        }

        private void RadioButtonClick(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            playerRadioSelect = Convert.ToInt32(rb.Text);
        }

     
    }
}