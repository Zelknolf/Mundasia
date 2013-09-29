using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Mundasia.Server.Communication;

namespace Mundasia.Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Service.Open();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Status());
            Service.Close();
        }
    }
}
