using System;
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