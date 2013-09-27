using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TileTester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// 
        /// The TileTester is not itself released code; it expects to be used to verify the contents of
        /// tilesets and the utility of existing tile functions. Portions may become released, but only
        /// after being cleaned up and appropriately commented. In current state, much of the code is
        /// pretty hacked up.
        /// 
        /// The TileTester expects to run beside the Images directory on the Mundasia repository.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TileViewer());
        }
    }
}
