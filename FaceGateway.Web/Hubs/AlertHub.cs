using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace FaceGateway.Web.Hubs
{
    public class AlertHub : Hub
    {
        public void Alert( string message )
        {
            Clients.All.handleAlert(message);
        }
    }
}