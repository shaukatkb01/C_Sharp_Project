using System.Reflection;

namespace SupplyBranch.Forms
{
    public static class AppVersion
    {
        public static string CurrentVersion
        {
            get
            {
                return Assembly
                    .GetExecutingAssembly()
                    .GetName()
                    .Version
                    .ToString();
            }
        }
    }
}