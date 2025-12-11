// <copyright file="KeyGeneratorClient.cs" company="Motion Applied Ltd.">
//
// Copyright 2025 Motion Applied Ltd
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System.Net.Security;
using System.Security.Authentication;

using Grpc.Net.Client;

using MA.Streaming.KeyGenerator;

namespace MA.KeyGenerator.Proto.Client;

public static class KeyGeneratorClient
{
    private static GrpcChannel? channel;

    public static bool Initialised { get; private set; }

    public static void Initialise(string serverAddress)
    {
        if (Initialised)
        {
            return;
        }

        try
        {
            channel = GrpcChannel.ForAddress(
                $"http://{serverAddress}",
                new GrpcChannelOptions
                {
                    HttpHandler = new SocketsHttpHandler
                    {
                        SslOptions = new SslClientAuthenticationOptions
                        {
                            EnabledSslProtocols = SslProtocols.None
                        }
                    }
                });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }

        Initialised = true;
    }

    public static void Shutdown()
    {
        channel?.Dispose();
        channel = null;
        Initialised = false;
    }

    public static UniqueKeyGeneratorService.UniqueKeyGeneratorServiceClient GetKeyGeneratorClient()
    {
        CheckInitialised();
        return new UniqueKeyGeneratorService.UniqueKeyGeneratorServiceClient(channel);
    }

    private static void CheckInitialised()
    {
        if (!Initialised)
        {
            throw new InvalidOperationException(
                "StreamApi StreamingApiClient has not been initialised. Please call StreamingApiClient.Initialise() before using the API.");
        }
    }
}
