using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SamplesSignalR.Hubs;

namespace SamplesSignalR.Services
{
	public class ProgressService
	{
		private readonly IHubContext<ProgressHub> _hubCtx;

		public ProgressService(IHubContext<ProgressHub> hubCtx)
		{
			_hubCtx = hubCtx;
		}

		public Task UpdateProgressBar(string connectId,int perc)
		{
			return _hubCtx.Clients.Client(connectId).SendAsync("updateProgressBar",perc);
		}

		public Task ClearProgressBar(string connectId)
		{
			return _hubCtx.Clients.Client(connectId).SendAsync("clearProgressBar");
		}
	}
}
