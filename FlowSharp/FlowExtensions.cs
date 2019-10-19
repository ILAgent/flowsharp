using System;
using System.Threading;
using System.Threading.Tasks;
using FlowSharp.Internal;

namespace FlowSharp
{
    public static class FlowExtensions
    {
        public static Task Collect<T>(this IFlow<T> flow, Action<T> collectorAction)
        {
            var collector = new FlowCollector<T>((item, _) =>
            {
                collectorAction(item);
                return Task.CompletedTask;
            });
            return flow.Collect(collector);
        }

        public static Task Collect<T>(this IFlow<T> flow, Func<T, Task> asyncCollector)
        {
            var collector = new FlowCollector<T>((item, _) => asyncCollector(item));
            return flow.Collect(collector);
        }

        public static Task Collect<T>(this IFlow<T> flow, Action<T, CancellationToken> collectorAction)
        {
            var collector = new FlowCollector<T>((item, cancellationToken) =>
            {
                collectorAction(item, cancellationToken);
                return Task.CompletedTask;
            });
            return flow.Collect(collector);
        }

        public static Task Collect<T>(this IFlow<T> flow, Func<T, CancellationToken, Task> asyncCollector)
        {
            var collector = new FlowCollector<T>(asyncCollector);
            return flow.Collect(collector);
        }


    }
}
