namespace GUI.Common
{
    public sealed class AppSession
    {
        public string UserName { get; private set; } = "";
        public int VaiTro { get; private set; }
        public string? MaPhong { get; set; }

        public bool IsAuthenticated => !string.IsNullOrEmpty(UserName);

        public void SignIn(string userName, int vaiTro)
        {
            UserName = userName;
            VaiTro = vaiTro;
        }

        public void SignOut()
        {
            UserName = "";
            VaiTro = 0;
            MaPhong = null;
        }
    }

    public static class SessionStore
    {
        public static AppSession Current { get; } = new AppSession();
    }
}

