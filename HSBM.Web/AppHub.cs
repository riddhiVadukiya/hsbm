using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using HSBM.EntityModel.RoleMasterDetails;
using HSBM.Common.Utils;
namespace HSBM.Web
{
    [HubName("AppHub")]
    public class AppHub : Hub
    {
        
        public void foo()
        {

        }

        //public void Connect(string id)
        //{
        //        Clients.Caller.onConnected(id, false);

        //}

        //public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        //{
        //    try
        //    {
        //        var item = Onlineuser.FirstOrDefault(x => x == Context.ConnectionId);
        //        if (item != null)
        //        {
        //            Onlineuser.Remove(item);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return base.OnDisconnected(stopCalled);
        //}
    }
}