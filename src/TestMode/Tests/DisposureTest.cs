﻿// SampSharp
// Copyright 2016 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Linq;
using SampSharp.GameMode.SAMP;
using SampSharp.GameMode.World;

namespace TestMode.Tests
{
    public class DisposureTest : ITest
    {
        public void Start(GameMode gameMode)
        {
            var playercount = BasePlayer.All.Count();
            var success = true;

            var player = BasePlayer.Create(499);

            if (BasePlayer.All.Count() - 1 != playercount)
            {
                Console.WriteLine("DisposureTest: Adding didn't add player to pool.");
                success = false;
            }
            player.Dispose();

            if (BasePlayer.All.Count() != playercount)
            {
                Console.WriteLine("DisposureTest: Disposing didn't remove player from pool.");
                success = false;
            }
            try
            {
                player.SetChatBubble("Test!", Color.Yellow, 100, 10);

                Console.WriteLine("DisposureTest: Passed SetChatBubble.");
                success = false;
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("DisposureTest: Exception thrown.");
            }

            Console.WriteLine("DisposureTest successful: {0}", success);
        }
    }
}