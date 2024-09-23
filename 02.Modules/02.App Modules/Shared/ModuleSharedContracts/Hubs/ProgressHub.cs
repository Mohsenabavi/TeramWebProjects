using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleSharedContracts.Hubs
{
    public class ProgressHub : Hub
    {
        public async Task SendProgress(int progress)
        {
            await Clients.All.SendAsync("ReceiveProgress", progress);
        }
    }
}
