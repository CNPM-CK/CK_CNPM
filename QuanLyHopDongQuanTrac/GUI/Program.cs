using GUI.Forms;
using GUI.Service;
using Vosk;
namespace GUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            var service = new DichVuThongBao();
            service.Start();
            ApplicationConfiguration.Initialize();
            Application.Run(new TrangChu());
            //Application.Run(new ());

        }


        // Thêm hàm này để hiển thị console
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
    }
}