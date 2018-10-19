using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using SamplesSignalR.Services;

namespace SamplesSignalR.Hubs
{
	public class ProgressHub : Hub
	{
		private static int _count = 0;
		private static ConcurrentDictionary<string,CancellationTokenSource> _ctsDict = new ConcurrentDictionary<string,CancellationTokenSource>();
		private IServiceProvider _serviceProvider;

		public ProgressHub(IServiceProvider svcProvider)
		{
			_serviceProvider = svcProvider;
		}

		public override Task OnConnectedAsync()
		{
			var count = Interlocked.Increment(ref _count);		
			base.OnConnectedAsync();
			return Clients.All.SendAsync("updateCount",count);			
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			var count = Interlocked.Decrement(ref _count);

			if (_ctsDict.TryRemove(Context.ConnectionId,out CancellationTokenSource cts))			
				cts.Cancel();

			base.OnDisconnectedAsync(exception);
			return Clients.All.SendAsync("updateCount",count);			
		}

		public string GetConnectionId()
		{
			return Context.ConnectionId;
		}
		
		public async Task UpdateProgress()
		{
			var steps = new Random().Next(3,20);
			var increase = (int)100/steps;
			var connectionId = Context.ConnectionId;
			var cts = new CancellationTokenSource();
			_ctsDict[connectionId] = cts;

			await Clients.Client(connectionId).SendAsync("initProgressBar").ConfigureAwait(false);	
			var svc = _serviceProvider.GetService<ProgressService>();

			var t = Task.Run(async () =>
			{				
				try
				{
					var total = 0;
					var ct = cts.Token;					

					for (var i = 0; i < steps; i++)
					{
						if (ct.IsCancellationRequested)
							return;

						await Task.Delay(2000,ct);
						total += increase;

						await svc.UpdateProgressBar(connectionId,total).ConfigureAwait(false);
					}
				}
				catch (Exception x)
				{
					return;
				}

				await svc.ClearProgressBar(connectionId).ConfigureAwait(false);
			});
		}

		public void StopTask()
		{
			var connectionId = Context.ConnectionId;

			if (_ctsDict.TryRemove(connectionId,out CancellationTokenSource cts))			
				cts.Cancel();							
		}
	}
}