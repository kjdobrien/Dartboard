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
    static class Constants
    {
        public static string PerviousGameFile = "/data/data/Dartboard.Dartboard/previousGame.txt";
        public static string PerviousPlayersFile = "/data/data/Dartboard.Dartboard/previousPlayers.txt"; 

        public enum ViewType {NameListItem, SuggestedName};
    }
}