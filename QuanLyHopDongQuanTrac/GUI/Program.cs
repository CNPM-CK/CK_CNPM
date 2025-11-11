using GUI.Forms;
using Vosk;
namespace GUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new TrangChu());
            //Application.Run(new DanhSachNhanVien());
            //Application.Run(new DanhSachKeHoach());

        }
    }
}