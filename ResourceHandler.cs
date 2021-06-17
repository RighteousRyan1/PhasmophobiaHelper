using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;

namespace PhasmophobiaHelper
{
    public class ResourceHandler
    {
        internal static ResourceManager Manager { get; set; }
        internal static CultureInfo Culture { get; set; }

        public static void LoadManagers()
        {
            Manager = new ResourceManager("ResXManager", Assembly.GetExecutingAssembly());
            Culture = new CultureInfo("TraitsList");
        }
    }
}