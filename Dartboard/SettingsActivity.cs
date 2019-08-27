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
using System.IO;
using Android.Support.V7.App;

namespace Dartboard
{
    [Activity(Label = "SettingsActivity", Theme = "@style/DartsAppStyle")]
    public class SettingsActivity :Activity
    {

        Switch SaveNameSwitch;
        Switch TabletModeSwitch;
        Button SaveSettingsButton;
        Button DeleteSavedFiles; 
        ImageButton backButton;
        RadioGroup fileDeleteOptions; 
        RadioButton DeleteSavedNames;
        RadioButton DeleteSavedGame; 



        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings);

            SaveNameSwitch = FindViewById<Switch>(Resource.Id.saveNameSwitch);
            TabletModeSwitch = FindViewById<Switch>(Resource.Id.tabletModeSwitch);
            SaveSettingsButton = FindViewById<Button>(Resource.Id.SaveSettings);
            backButton = FindViewById<ImageButton>(Resource.Id.backButtonSettings);
            fileDeleteOptions = FindViewById<RadioGroup>(Resource.Id.filesToDelete);

            DeleteSavedFiles = FindViewById<Button>(Resource.Id.deleteSavedData);

            backButton.Click += BackButton_Click;
            SaveSettingsButton.Click += SaveSettingsButton_Click;

        }

        private void DeleteSavedNames_Click(object sender, EventArgs e)
        {
            DeleteSavedNames.Checked = DeleteSavedNames.Checked ? DeleteSavedNames.Checked = false : DeleteSavedNames.Checked = true;
        }

        private void DeleteSavedGame_Click(object sender, EventArgs e)
        {
            DeleteSavedGame.Checked = DeleteSavedGame.Checked ? DeleteSavedGame.Checked = false : DeleteSavedGame.Checked = true; 
        }

        private void DeleteSavedFiles_Click(object sender, EventArgs e)
        {
            var DeleteFiles = new Android.Support.V7.App.AlertDialog.Builder(this, Resource.Style.AlertDialogCustom);
            DeleteFiles.SetView(Resource.Layout.DeleteFiles);
            DeleteFiles.SetPositiveButton("Delete", HandlePositiveButtonClick);
            DeleteFiles.Create(); 
            DeleteFiles.Show();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            
        }

        private void ClearSavedNames()
        {
            try
            {
                File.Delete(Constants.PerviousPlayersFile);
            }
            catch (IOException ex)
            {
                HelperFunctions.DartsToast(this, "No name file present", ToastLength.Long);
            }
            
        }

        private void ClearSavedGame()
        {

            try
            {
                File.Delete(Constants.PerviousGameFile);
            }
            catch (IOException ex)
            {
                HelperFunctions.DartsToast(this, "No game file present", ToastLength.Long);
            }
        }

        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (Android.Support.V7.App.AlertDialog)sender;
            DeleteSavedNames = (RadioButton)dialog.FindViewById(Resource.Id.DeleteSavedNamesRadio);
            DeleteSavedGame = (RadioButton)dialog.FindViewById(Resource.Id.DeleteSavedGameRadio);

            if (DeleteSavedNames.Checked == true)
            {
                ClearSavedNames();
            }
            if (DeleteSavedGame.Checked == true)
            {
                ClearSavedGame();
            }
            else
            {
                dialog.Dismiss();
            }
        }
    }
}