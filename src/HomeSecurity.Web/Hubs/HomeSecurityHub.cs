using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeSecurity.Web.Hubs
{
    public class HomeSecurityHub:Hub
    {
        public void Send(string message)
        {
            Clients.addMessage(message);
        }
    }
}