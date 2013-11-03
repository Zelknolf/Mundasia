using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MundasiaClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            if(!Startup.Start())
            {
                MessageBox.Show(String.Format("Mundasia was unable to start, with the following error message:\n\n{0}\n\nPlease check your configuration and try again.", Startup.error));
            }
            Application.Run(new SplashScreen());
        }
    }
}
