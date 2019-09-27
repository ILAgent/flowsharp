﻿using System;
using System.Threading.Tasks;

namespace flowsharp
{
    static class FlowExtensions
    {
        public static Task Collect<T>(this IFlow<T> flow, Action<T> collectorAction)
        {
            var collector = new FlowCollector<T>(item =>
            {
                collectorAction(item);
                return Task.CompletedTask;
            });
            return flow.Collect(collector);
        }

        public static Task Collect<T>(this IFlow<T> flow, Func<T, Task> asyncCollector)
        {
            var collector = new FlowCollector<T>(item =>
            {
                asyncCollector(item);
                return Task.CompletedTask;
            });
            return flow.Collect(collector);
        }


    }
}
