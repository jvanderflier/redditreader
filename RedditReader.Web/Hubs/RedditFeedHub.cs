using Microsoft.AspNetCore.SignalR;
using RedditReader.Shared.Models;

namespace RedditReader.Web.Hubs;

public class RedditFeedHub : Hub<IRedditFeedClient>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Client(Context.ConnectionId).ClientConnected($"Thank you for connecting {Context.ConnectionId}");

        await base.OnConnectedAsync();
    }
}

public interface IRedditFeedClient
{
    Task ClientConnected(string message);
    Task ReceivePost(HubNotifcation t);
}
