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
    [Activity(Label = "SplashActivity", MainLauncher = true, Icon = "@drawable/launcherIcon192x192", Theme = "@style/DartsAppStyle")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Splash);
            // Create your application here
            
            StartActivity(typeof(CreateGame));
        }
    }
}