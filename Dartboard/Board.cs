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
using Android.Graphics;

namespace Dartboard
{
    class Board
    {
        public Dictionary<int, Color> ColorScores = new Dictionary<int, Color>();
        public Dictionary<int, string> Checkouts = new Dictionary<int, string>();



        public Board()
        {
            ColorScores.Add(0, Color.Black);
            ColorScores.Add(40, Color.Aqua);
            ColorScores.Add(20, Color.Blue);
            ColorScores.Add(60, Color.Fuchsia);
            ColorScores.Add(2, Color.OliveDrab); // 0
            ColorScores.Add(1, Color.HotPink); // 0 
            ColorScores.Add(3, Color.PaleVioletRed); 
            ColorScores.Add(36, Color.Maroon);
            ColorScores.Add(18, Color.Navy);
            ColorScores.Add(54, Color.Olive);
            ColorScores.Add(8, Color.Orange);
            ColorScores.Add(4, Color.Purple);
            ColorScores.Add(12, Color.PaleGoldenrod); // 0
            ColorScores.Add(26, Color.Red);
            ColorScores.Add(13, Color.Silver);
            ColorScores.Add(39, Color.Teal);
            ColorScores.Add(6, Color.Yellow);
            ColorScores.Add(10, Color.Aquamarine);
            ColorScores.Add(30, Color.Plum);
            ColorScores.Add(15, Color.Bisque);
            ColorScores.Add(45, Color.Indigo);
            ColorScores.Add(34, Color.CadetBlue);
            ColorScores.Add(17, Color.Chartreuse);
            ColorScores.Add(51, Color.LightSlateGray);
            ColorScores.Add(9, Color.MediumTurquoise);
            ColorScores.Add(38, Color.Crimson);
            ColorScores.Add(19, Color.Sienna); 
            ColorScores.Add(57, Color.DarkBlue);
            ColorScores.Add(14, Color.DarkCyan);
            ColorScores.Add(7, Color.DarkGoldenrod);
            ColorScores.Add(21, Color.OrangeRed); // 0
            ColorScores.Add(32, Color.DarkGreen);
            ColorScores.Add(16, Color.DarkKhaki);
            ColorScores.Add(48, Color.Chocolate);
            ColorScores.Add(24, Color.DarkOrchid);
            ColorScores.Add(22, Color.DarkRed);
            ColorScores.Add(11, Color.DarkSalmon);
            ColorScores.Add(33, Color.SeaGreen); // 0
            ColorScores.Add(28, Color.DarkSlateBlue);        
            ColorScores.Add(42, Color.DarkTurquoise);
            ColorScores.Add(27, Color.DeepSkyBlue);
            ColorScores.Add(5, Color.ForestGreen);
            ColorScores.Add(25, Color.Goldenrod);
            ColorScores.Add(50, Color.MediumVioletRed);

            

  

        }



                  


        }
}