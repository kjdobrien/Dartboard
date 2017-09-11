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

namespace Dartboard
{
    static class FileManager
    {
        public static string RestoreDirectory = "data/data/Dartboard.Dartboard/RestoreFiles/";
        public static string[] Names; 

       
        
           
        

        public static void ReadFromFile()
        {
            string line;
            try
            {         
                using (StreamReader reader = File.OpenText(RestoreDirectory + "/Names.txt"))
                {
                    int i = 0; 
                    while ((line = reader.ReadLine()) != null)
                    {
                        Names[i] = line;
                        i++;                
                    }
                }        
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void WriteToFile(string[] names)
        {
            try
            {
                using (StreamWriter file = File.CreateText(RestoreDirectory + "/Names.txt"))
                {

                    foreach (string n in names)
                    {
                        file.WriteLine(n);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}