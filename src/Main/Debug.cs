using System;
using System.IO;
using ChoETL;
using static System.Console;

namespace Main
{
    public static class Printer
    {
        public static void PrintTokenData(this FileInfo file)
        {
            WriteLine("Gathering token data...");

            try
            {
                foreach (var e in new ChoJSONReader<Model.LoginResponse>(file.FullName))
                {
                    WriteLine($"Token: {e.Token} \nUUID: {e.UUID}");
                }
            } 
            catch (Exception e)
            {
                WriteLine(e);

                // Print file content
                WriteLine(File.ReadAllText(file.FullName));
            }
        }
    }
}