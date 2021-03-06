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
using System.Threading;
using System.Threading.Tasks;
using SampSharp.GameMode.Controllers;
using SampSharp.GameMode.Helpers;
using SampSharp.GameMode.Pools;

namespace SampSharp.GameMode.Tools
{
    /// <summary>
    ///     Contains methods to run an action on the main thread from a different thread.
    /// </summary>
    public static class Sync
    {
        /// <summary>
        ///     Gets a value indicating whether a sync is required before calling natives.
        /// </summary>
        public static bool IsRequired => SyncController.MainThread != Thread.CurrentThread;

        /// <summary>
        ///     Run a function on the main thread.
        /// </summary>
        /// <param name="action">The action to run.</param>
        public static void Run(Action action)
        {
            if (!IsRequired)
            {
                action();
                return;
            }

            new SyncTask {Action = action};
            SyncController.Start();
        }

        /// <summary>
        ///     Run a function on the main thread synchronously and await its result.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void RunSync(Action action)
        {
            if (!IsRequired)
                action();
            else
                AsyncHelper.RunSync(() => RunAsync(action));
        }

        /// <summary>
        ///     Run a function on the main thread synchronously and await its result.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        public static T RunSync<T>(Func<T> func)
        {
            return !IsRequired
                ? func()
                : AsyncHelper.RunSync(() => RunAsync(func));
        }

        /// <summary>
        ///     Run a function on the main  thread.
        /// </summary>
        /// <param name="action">The action the run</param>
        public static async Task RunAsync(Action action)
        {
            if (!IsRequired)
            {
                action();
                return;
            }

            var task = new SyncTask {Action = action};
            SyncController.Start();

            while (!task.Done)
            {
                await Task.Delay(1);
            }
        }

        /// <summary>
        ///     Run a function on the main thread.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the method that the action encapsulates.</typeparam>
        /// <param name="action">The action to run.</param>
        /// <returns>The return value of the method that the action encapsulates.</returns>
        public static async Task<TResult> RunAsync<TResult>(Func<TResult> action)
        {
            if (!IsRequired)
            {
                return action();
            }

            var result = default(TResult);

            await RunAsync(() => { result = action(); });

            return result;
        }

        internal sealed class SyncTask : Pool<SyncTask>
        {
            public Action Action { get; set; }

            public bool Done { get; private set; }

            public void Run()
            {
                Action();
                Done = true;
            }
        }
    }
}