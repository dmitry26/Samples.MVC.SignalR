using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SamplesSignalR.Hubs
{
	public class ClockHub : Hub
	{
		private static ConcurrentDictionary<string,CancellationTokenSource> _ctsDict = new ConcurrentDictionary<string,CancellationTokenSource>();

		public Task Start()
		{
			_ctsDict[Context.ConnectionId] = new CancellationTokenSource();
			return Clients.Client(Context.ConnectionId).SendAsync("clockStarted");
		}

		public Task Stop()
		{
			if (_ctsDict.TryRemove(Context.ConnectionId,out CancellationTokenSource cts))
				cts.Cancel();

			return Clients.Client(Context.ConnectionId).SendAsync("clockStopped");
		}

		public string GetConnectionId()
		{
			return Context.ConnectionId;
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			if (_ctsDict.TryRemove(Context.ConnectionId,out CancellationTokenSource cts))
				cts.Cancel();

			return base.OnDisconnectedAsync(exception);
		}

		public ChannelReader<string> Tick()
		{
			if (!_ctsDict.TryGetValue(Context.ConnectionId,out CancellationTokenSource cts))
				throw new InvalidOperationException("Clock was not started");

			var channel = Channel.CreateUnbounded<string>();

			Task.Run(async () =>
			{
				try
				{
					var cancelToken = cts.Token;

					while (!cancelToken.IsCancellationRequested)
					{
						var time = DateTime.Now.ToString("HH:mm:ss");
						await channel.Writer.WriteAsync(time).ConfigureAwait(false);
						await Task.Delay(1000,cancelToken).ContinueWith(t => { }).ConfigureAwait(false);
					}
				}
				catch
				{
				}

				channel.Writer.TryComplete();
			});

			return channel.Reader;
		}
	}
}