// <copyright file="UniqueIdGeneratorService.cs" company="Motion Applied Ltd.">
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

using Grpc.Core;

using MA.Common.Abstractions;
using MA.Streaming.KeyGenerator;

namespace MA.KeyGenerator.Proto.Server.Services;

public class UniqueIdGeneratorService : UniqueKeyGeneratorService.UniqueKeyGeneratorServiceBase
{
    private readonly IKeyGeneratorService generatorService;

    public UniqueIdGeneratorService(IKeyGeneratorService generatorService)
    {
        this.generatorService = generatorService;
    }

    public override Task<GenerateUniqueKeyResponse> GenerateUniqueKey(GenerateUniqueKeyRequest request, ServerCallContext context)
    {
        var response = request.Type switch
        {
            KeyType.Ulong => new GenerateUniqueKeyResponse
            {
                UlongKey = this.generatorService.GenerateUlongKey()
            },
            KeyType.String => new GenerateUniqueKeyResponse
            {
                StringKey = this.generatorService.GenerateStringKey()
            },
            KeyType.Unspecified => throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid key type requested")),
            _ => throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid key type requested")),
        };
        return Task.FromResult(response);
    }
}
