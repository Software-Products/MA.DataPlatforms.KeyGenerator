// <copyright file="Program.cs" company="Motion Applied Ltd.">
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

using MA.Common;
using MA.Common.Abstractions;
using MA.KeyGenerator.Proto.Server.Services;

using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace MA.KeyGenerator.Proto.Server;

internal static class Program
{
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        AppDefinition.Load();

        builder.WebHost.ConfigureKestrel(
            ConfigureListenOptions
        );

        builder.WebHost.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(LogLevel.Warning);
            logging.AddConsole();
        });

        var loggingFolderProvider = new LoggingDirectoryProvider("");
        builder.Services.AddTransient<ILoggingDirectoryProvider>(_ => loggingFolderProvider);
        builder.Services.AddSingleton<IKeyGeneratorService, KeyGeneratorService>();

        builder.Services.AddGrpc();

        var app = builder.Build();

        app.MapGrpcService<UniqueIdGeneratorService>();
        app.MapGet(
            "/",
            () =>
                "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        Console.WriteLine("Key Generator Service Started");

        app.Run();
    }

    private static void ConfigureListenOptions(KestrelServerOptions options)
    {
        options.ListenAnyIP(
            int.Parse(AppDefinition.Port),
            listenOption =>
            {
                listenOption.Protocols = HttpProtocols.Http2;
            });
    }
}
