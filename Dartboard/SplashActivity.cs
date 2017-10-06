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
using System.Threading.Tasks;
using Android.Util;
using System.IO;

namespace Dartboard
{
    [Activity(Label = "180 Darts", MainLauncher = true, NoHistory = true, Icon = "@drawable/launcherIcon192x192", Theme = "@style/MyTheme.Splash")]
    public class SplashActivity : Activity
    {

        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            using (StreamReader sr = new StreamReader(Assets.Open("Checkouts.txt")))
            {
                char[] delim = { ':' };
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(delim);
                    int scoreValue;
                    int.TryParse(words[0], out scoreValue);
                    string bestCheckout = words[1];
                    Board.Checkouts.Add(scoreValue, bestCheckout);
                }
            }

            using (StreamReader sr = new StreamReader(Assets.Open("twoDartCheckouts.txt")))
            {
                char[] delim = { ':' };
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(delim);
                    int scoreValue;
                    int.TryParse(words[0], out scoreValue);
                    string bestCheckout = words[1];
                    Board.TwoDartCheckouts.Add(scoreValue, bestCheckout);
                }
            }

        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        async void SimulateStartup()
        {
            Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
            await Task.Delay(4000); // Simulate a bit of startup work.
            Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
            StartActivity(new Intent(Application.Context, typeof(CreateGame)));
        }
    }
}