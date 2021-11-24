using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CanardEcarlate.Api.Models
{
    public class CanardEcarlateHub : Hub
    {
        // Exemple de méthode possible dans le hub, appelable directement depuis le front (penser à mettre Async à la fin)
        public async Task SendMessageAsync (string user) {
            await Clients.All.SendAsync ("AfterSendMessageAsync", $"Good Morning {user}");
        }
    }
}