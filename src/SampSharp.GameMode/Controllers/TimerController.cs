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

using SampSharp.GameMode.SAMP;

namespace SampSharp.GameMode.Controllers
{
    /// <summary>
    ///     A controller processing all timer actions.
    /// </summary>
    [Controller]
    public class TimerController : IEventListener
    {
        /// <summary>
        ///     Registers the events this TimerController wants to listen to.
        /// </summary>
        /// <param name="gameMode">The running GameMode.</param>
        public virtual void RegisterEvents(BaseMode gameMode)
        {
            gameMode.TimerTick += (sender, args) => (sender as Timer)?.OnTick(args);
        }
    }
}