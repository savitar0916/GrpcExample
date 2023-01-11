using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter
{
    public class GreeterService : Greeter.GreeterBase
    {

        // Unary RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        // Server streaming RPC
        public override async Task SayHelloStream(HelloServerStreamingRequest helloServerStreamingRequest,
            IServerStreamWriter<HelloReply> helloReplyStream, ServerCallContext serverCallContext)
        {
            var count = 0;
            //開始計時
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            

            while (count < helloServerStreamingRequest.NameCount && !serverCallContext.CancellationToken.IsCancellationRequested) 
            {
                count++;
                var response = new HelloReply
                {
                    Message = $"Request message: Hello, {helloServerStreamingRequest.Name}({count})"
                };

                await helloReplyStream.WriteAsync(response).ConfigureAwait(false);
                //await Task.Delay(1000).ConfigureAwait(false);
            }
            //計時結束
            stopWatch.Stop();
            TimeSpan timeSpan = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,
                timeSpan.Milliseconds / 10);
            var responseTime = new HelloReply
            {
                Message = "This Response Cost : " + elapsedTime
            };
            await helloReplyStream.WriteAsync(responseTime).ConfigureAwait(false);
            Console.WriteLine("RunTime " + elapsedTime);
        }

        // Client streaming RPC
        public override async Task<HelloReply> StreamSayHello(IAsyncStreamReader<HelloRequest> helloRequestStream, ServerCallContext serverCallContext)
        {
            var messageCount = 0;

            while (await helloRequestStream.MoveNext().ConfigureAwait(false))
            {
                messageCount++;

                var request = helloRequestStream.Current;
                Console.WriteLine(request.Name);
            }
            return new HelloReply
            {
                Message = $"Received {messageCount} messages."
            };
        }

        // Bidirectional streaming RPC
        public override async Task StreamSayHelloStream(IAsyncStreamReader<HelloRequest> helloRequestStream, IServerStreamWriter<HelloReply> helloReplyStream,ServerCallContext serverCallContext )
        {
            while( await helloRequestStream.MoveNext().ConfigureAwait(false))
            {
                var response = new HelloReply
                {
                    Message = $"Request message: Hello , {helloRequestStream.Current.Name}"
                };

                await helloReplyStream.WriteAsync(response).ConfigureAwait(false);
            }
        }
    }
}
