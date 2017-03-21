using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace BinarySurveyPOC.Hubs
{
    public class SurveyHub : Hub
    {
        public Task JoinRoom(string adminLevel1)
        {
            return Groups.Add(Context.ConnectionId, adminLevel1);
        }

        public Task LeaveRoom(string adminLevel1)
        {
            return Groups.Remove(Context.ConnectionId, adminLevel1);
        }
        
    }
}