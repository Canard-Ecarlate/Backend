using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CanardEcarlate.Api.Models
{
    public class CanardEcarlateHub : Hub
    {
        // Exemple de méthode possible dans le hub, appelable directement depuis le front
        public async Task SendMessage (string user) {
            await Clients.All.SendAsync ("AfterSendMessage", $"Good Morning {user}");
        }
    }
}