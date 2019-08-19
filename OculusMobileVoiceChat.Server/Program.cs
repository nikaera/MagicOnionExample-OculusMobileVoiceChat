using Grpc.Core;
using MagicOnion.Hosting;
using MagicOnion.Server;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        GrpcEnvironment.SetLogger(new Grpc.Core.Logging.ConsoleLogger());

        List<ChannelOption> options = new List<ChannelOption> {
            new ChannelOption("grpc.keepalive_time_ms", 2000),
            new ChannelOption("grpc.keepalive_timeout_ms", 3000),
            new ChannelOption("grpc.http2.min_time_between_pings_ms", 5000),
        };

        await MagicOnionHost.CreateDefaultBuilder()
            .UseMagicOnion(
                new MagicOnionOptions(isReturnExceptionStackTraceInErrorDetail: true),
                new ServerPort("0.0.0.0", 12345, ServerCredentials.Insecure),
                options
            )
            .RunConsoleAsync();
    }
}
