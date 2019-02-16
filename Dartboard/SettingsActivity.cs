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
    [Activity(Label = "SettingsActivity")]
    public class SettingsActivity : Activity
    {

        Switch SaveNameSwitch;
        Switch TabletModeSwitch;
        Button SaveSettingsButton;
        ImageButton backButton; 


        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings);

            SaveNameSwitch = FindViewById<Switch>(Resource.Id.saveNameSwitch);
            TabletModeSwitch = FindViewById<Switch>(Resource.Id.tabletModeSwitch);
            SaveSettingsButton = FindViewById<Button>(Resource.Id.SaveSettings);
            backButton = FindViewById<ImageButton>(Resource.Id.backButtonSettings);
            SaveSettingsButton.Click += SaveSettingsButton_Click;
            backButton.Click += BackButton_Click;

        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            
        }
    }
}