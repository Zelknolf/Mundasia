using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mundasia.Communication;
using Mundasia.Objects;

namespace Mundasia.Client
{
    public class Startup
    {
        public static SplashScreen splash;

        public static string error;

        /// <summary>
        /// This method starts the Mundasia client, displaying the splash screen
        /// and loading/caching any information that's needed for general play.
        /// </summary>
        /// <returns>true on success; false on failure</returns>
        public static bool Start()
        {
            splash = new SplashScreen();
            splash.Show();


            //if(!Connect())
            //{
            //   error = "Unable to acquire information from the server.";
            //   splash.Close();
            //   return false;
            //}
            splash.progress.PerformStep();
            if(!LocalLoad())
            {
                error = "Unable to load local resources.";
                splash.Close();
                return false;
            }
            splash.progress.PerformStep();
            splash.Close();
            return true;
        }

        
        /// <summary>
        /// This method makes the basic connection to the server and gathers/ caches any
        /// information that is expected to be gathered from the server during startup.
        /// </summary>
        /// <returns>true on success; false on failure</returns>
        public static bool Connect()
        {
            string ping = ServiceConsumer.Ping();
            if(ping != String.Empty)
            {
                Mundasia.Interface.PrimaryForm.ServerClock = ping;
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method loads all of the local and configurable resources that are
        /// expected to be used as part of general play.
        /// </summary>
        /// <returns>true on success; false on failure</returns>
        public static bool LocalLoad()
        {
            StringLibrary.Load();
            splash.progress.PerformStep();
            Skill.Load();
            splash.progress.PerformStep();
            Race.Load();
            splash.progress.PerformStep();
            Virtue.Load();
            splash.progress.PerformStep();
            Vice.Load();
            splash.progress.PerformStep();
            Authority.Load();
            splash.progress.PerformStep();
            Care.Load();
            splash.progress.PerformStep();
            Fairness.Load();
            splash.progress.PerformStep();
            Loyalty.Load();
            splash.progress.PerformStep();
            Tradition.Load();
            splash.progress.PerformStep();
            Profession.Load();
            splash.progress.PerformStep();
            Ability.Load();
            splash.progress.PerformStep();
            Lore.Load();
            splash.progress.PerformStep();
            return true;
        }
    }
}
