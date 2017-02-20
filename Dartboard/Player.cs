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
    class Player
    {

        List<int> Darts = new List<int>();


        public string name { get; set; }


        public Player()
        {
            name = "Player 1";
        }
    }
}