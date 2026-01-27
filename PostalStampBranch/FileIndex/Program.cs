namespace FileIndex
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Pehle Login Form ko kholien
            Login login = new Login();

            if (login.ShowDialog() == DialogResult.OK)
            {
                // Agar Login successful ho gaya, toh Main form chalayein
                Application.Run(new Main());
            }
            else
            {
                // Warna application band kar dein
                Application.Exit();
            }
        }
    }
}