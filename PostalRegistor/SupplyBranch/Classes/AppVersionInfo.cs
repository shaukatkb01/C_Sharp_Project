using System.Reflection;

namespace SupplyBranch.Classes
{
    public static class AppVersionInfo
    {
        // AssemblyInfo se dynamically current version uthayega
        public static string CurrentVersion { get; set; } =
            Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0.0";

        public static string AvailableVersion { get; set; } = "";
        public static string DownloadUrl { get; set; } = "";
    }
}