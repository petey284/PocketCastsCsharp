using System;

namespace Main
{
    public static class Constants
    {
        public static string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static string ProjectWorkspace = $"{Desktop}\\current-workspace\\DotnetPocketCasts\\PocketCastsCsharp\\src\\Main";
    }
}