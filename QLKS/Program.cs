using QLKS.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool logout = false;
            do
            {
                FormLogin login = new FormLogin();
                Application.Run(login);
                if (login.Account != null)
                {
                    FormMainMenu main = new FormMainMenu(login.Account);
                    Application.Run(main);
                    logout = main.Logout;
                }
            } while (logout);
        }
    }
}
