﻿using System;
using System.Collections.Generic;
using System.IO;
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
    static class HelperFunctions
    {
        public static GameData CheckForPreviousGame()
        {
            string fileName = "/data/data/Dartboard.Dartboard/previousGame.txt";
            GameData gameData = new GameData();
            if (File.Exists(fileName))
            {
                using (StreamReader sr = File.OpenText(fileName))
                {
                    gameData.player1Name = sr.ReadLine();
                    gameData.player1Score = sr.ReadLine();
                    gameData.player1Turn = sr.ReadLine();
                    gameData.player2Name = sr.ReadLine();
                    gameData.player2Score = sr.ReadLine();
                    gameData.player2Turn = sr.ReadLine();
                    gameData.LegsPlayed = sr.ReadLine();
                    gameData.LegsToPlay = sr.ReadLine();

                    gameData.player1Name = gameData.player1Name.Substring(gameData.player1Name.IndexOf("-") + 1);
                    gameData.player1Score = gameData.player1Score.Substring(gameData.player1Score.IndexOf("-") + 1);
                    gameData.player1Turn = gameData.player1Turn.Substring(gameData.player1Turn.IndexOf("-") + 1);
                    gameData.player2Name = gameData.player2Name.Substring(gameData.player2Name.IndexOf("-") + 1);
                    gameData.player2Score = gameData.player2Score.Substring(gameData.player2Score.IndexOf("-") + 1);
                    gameData.player2Turn = gameData.player2Turn.Substring(gameData.player2Turn.IndexOf("-") + 1);
                    gameData.LegsPlayed = gameData.LegsPlayed.Substring(gameData.LegsPlayed.IndexOf("-") + 1);
                    gameData.LegsToPlay = gameData.LegsToPlay.Substring(gameData.LegsToPlay.IndexOf("-") + 1);
                }
            }
            return gameData;
        }



        public static List<string> GetNames()
        {
            string fileName = "/data/data/Dartboard.Dartboard/previousPlayers.txt";
            List<string> Names = new List<string>();
            if (File.Exists(fileName))
            {
                using (StreamReader sr = File.OpenText(fileName))
                {
                    while (sr.Peek() >= 0)
                    {
                        Names.Add(sr.ReadLine());
                    }

                }

                return Names;
            }
            else
            {
                return null;
            }
        }

        public static List<string> DeleteName(List<string> Names, string name)
        {
            string fileName = "/data/data/Dartboard.Dartboard/previousPlayers.txt";
            Names.Remove(name);
            File.Delete(fileName);
            using (StreamWriter sw = File.CreateText(fileName))
            {
                foreach (string n in Names)
                {
                    File.AppendAllLines(fileName, Names);
                }
            }

            Names = GetNames();

            return Names;
        }


        public static void DeleteSaveFile(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }



        public static Toast DartsToast(Activity activity, string text, ToastLength toastLength)
        {
            Toast toast = Toast.MakeText(activity, text, toastLength);
            toast.SetGravity(GravityFlags.Center, 0, 0);
            return toast;
        }

        public static bool CheckForName(string name, List<string> Names)
        {
            return Names.Contains(name);
        }

        public static void SavePlayerName(List<string> names)
        {
            using (StreamWriter file = File.AppendText("/data/data/Dartboard.Dartboard/previousPlayers.txt"))
            {
                foreach (string name in names)
                {
                    file.WriteLine(name);
                }

            }
        }
    }


    public class BustToast : Toast
    {     

        public BustToast(Context context) : base(context) {
            
        }

        public BustToast(Context context, Activity activity, string text, ToastLength toastLength):base(context)
        {
            View bustView = new View(activity);
            LayoutInflater inflater = activity.LayoutInflater;
            View view = inflater.Inflate(Resource.Layout.bust, null);

            TextView txt = view.FindViewById<TextView>(Resource.Id.text);

            txt.Text = text;
            SetGravity(GravityFlags.Center, 0, 0);
            Duration = toastLength;
            View = view;


        }


    }

    public struct GameData
    {
        public string player1Name;
        public string player1Score;
        public string player1Turn;
        public string player2Name;
        public string player2Score;
        public string player2Turn;
        public string LegsPlayed;
        public string LegsToPlay;
    }
}