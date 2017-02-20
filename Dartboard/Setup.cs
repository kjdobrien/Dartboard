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
        bool isCheckin;
        bool isCheckout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Get the number of players
            RadioGroup playersRadioGroup = FindViewById<RadioGroup>(Resource.Id.numplayers);
            
            // Double in Double out? 
            Switch doubleIn = (Switch)FindViewById(Resource.Id.checkin);
            Switch doubleOut = (Switch)FindViewById(Resource.Id.checkout);
                  
            // How many Sets
            EditText numOfSetsTextBox = (EditText)FindViewById(Resource.Id.numSets);
            

            // Send to MainActivity
            Button submit = (FindViewById<Button>(Resource.Id.submit);

            submit.Click += delegate
            {
                RadioButton checkedRadioButton = FindViewById<RadioButton>(playersRadioGroup.CheckedRadioButtonId);
                int playerRadioSelect = Convert.ToInt32(checkedRadioButton.Text);

                isCheckin = doubleIn.Checked;

                isCheckout = doubleOut.Checked;

                int numOfSet = Convert.ToInt32(numOfSetsTextBox.Text);

                var setupIntent = new Intent(this, typeof(Setup));
                setupIntent.PutExtra("numplayers", playerRadioSelect);
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