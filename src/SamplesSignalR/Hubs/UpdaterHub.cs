using Microsoft.AspNetCore.SignalR;

namespace SamplesSignalR.Hubs
{
	public class UpdaterHub : Hub
	{
		public string GetConnectionId()
		{
			return Context.ConnectionId;
		}
	}
}