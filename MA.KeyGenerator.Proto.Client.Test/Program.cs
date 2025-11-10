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

using System.Diagnostics;

using MA.Streaming.KeyGenerator;

namespace MA.KeyGenerator.Proto.Client.Test;

internal static class Program
{
    public static void Main()
    {
        try
        {
            Console.ReadLine();
            KeyGeneratorClient.Initialise("localhost:15379");
            var client = KeyGeneratorClient.GetKeyGeneratorClient();
            var _ = new HashSet<ulong>();
            var stopwatch = new Stopwatch();
            stopwatch.Restart();
            for (var i = 0; i < 100_000; i++)
            {
                _.Add(
                    client.GenerateUniqueKey(
                        new GenerateUniqueKeyRequest
                        {
                            Type = KeyType.Ulong
                        }).UlongKey);
            }

            Console.WriteLine("elapsed: " + stopwatch.ElapsedMilliseconds);
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            Console.ReadLine();
        }
    }
}
