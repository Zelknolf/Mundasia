using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Mundasia.Server.Communication;
using Mundasia.Objects;

using Mundasia.Server;



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
            LoadLocalResources();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Status());
            Service.Close();
        }

        static void LoadLocalResources()
        {
            Ability.Load();
            Authority.Load();
            Care.Load();
            Fairness.Load();
            Loyalty.Load();
            Tradition.Load();
            Profession.Load();
            Race.Load();
            Skill.Load();
            Vice.Load();
            Virtue.Load();
        }
    }
}