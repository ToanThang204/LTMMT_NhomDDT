using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DoAnMangMayTinh.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task SendLichThiSapToi(string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveNotification", message);
        }
    }
}