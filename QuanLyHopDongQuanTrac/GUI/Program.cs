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
<<<<<<< HEAD
            //Application.Run(new TrangChu());
=======
>>>>>>> cd17a623a8328c280f450baacb3f17b3562ce493
            Application.Run(new TrangGioiThieu());
            //Application.Run(new DanhSachNhanVien());
            //Application.Run(new DanhSachKeHoach());

        }
    }
}