using System.Runtime.InteropServices;

namespace TxtServerAsAPIDocker.Helpers
{

    public class Fonts
    {
        [DllImport("gdi32", EntryPoint = "AddFontResource")]
        public static extern int AddFontResourceA(string lpFileName);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int AddFontResource(string lpszFilename);
    }
}
