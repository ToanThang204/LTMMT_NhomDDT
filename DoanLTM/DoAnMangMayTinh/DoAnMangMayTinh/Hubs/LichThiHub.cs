using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DoAnMangMayTinh.Hubs
{
    public class LichThiHub : Hub
    {
        public async Task SendLichThi(string message)
        {
            await Clients.All.SendAsync("ReceiveLichThi", message);
        }
    }
}