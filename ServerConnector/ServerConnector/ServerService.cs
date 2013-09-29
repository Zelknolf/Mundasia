using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Mundasia.Server.Communication
{
    class ServerService : IServerService
    {
        public string Ping()
        {
            return DateTime.UtcNow.ToShortTimeString();
        }
    }
}
