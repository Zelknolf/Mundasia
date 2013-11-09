﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mundasia.Communication;

namespace MundasiaClient
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
            if(!Connect())
            {
                error = "Unable to acquire information from the server.";
                return false;
            }
            splash.progress.PerformStep();
            if(!LocalLoad())
            {
                error = "Unable to load local resources.";
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
            if(ServiceConsumer.Ping() != string.Empty)
            {
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
            return true;
        }
    }
}