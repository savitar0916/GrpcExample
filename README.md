# GrpcExample

This is a .Net Project to Practice gRPC :
1. Unary RPC
    ```c#
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    ```
2. Server streaming RPC
    ```c#
    public override async Task SayHelloStream(HelloServerStreamingRequest helloServerStreamingRequest, IServerStreamWriter<HelloReply> helloReplyStream, ServerCallContext serverCallContext)
    ```
3. Client streaming RPC
    ```c#
    public override async Task<HelloReply> StreamSayHello(IAsyncStreamReader<HelloRequest> helloRequestStream, ServerCallContext serverCallContext
    ```
4. Bidirectional streaming RPC
    ```c#
    public override async Task StreamSayHelloStream(IAsyncStreamReader<HelloRequest> helloRequestStream, IServerStreamWriter<HelloReply> helloReplyStream,ServerCallContext serverCallContext )
    ```
