using System;
using System.Threading;
using System.Threading.Tasks;
using FlowSharp.Internal;

namespace FlowSharp
{
    public static class FlowExtensions
    {
        public static Task Collect<T>(this IFlow<T> flow, Action<T> collectorAction,
            CancellationToken cancellationToken = default)
        {
            var collector = new FlowCollector<T>((item, _) =>
            {
                collectorAction(item);
                return Task.CompletedTask;
            });
            return flow.Collect(collector, cancellationToken);
        }

        public static Task Collect<T>(this IFlow<T> flow, Func<T, Task> asyncCollector,
            CancellationToken cancellationToken = default)
        {
            var collector = new FlowCollector<T>((item, _) => asyncCollector(item));
            return flow.Collect(collector, cancellationToken);
        }

        public static Task Collect<T>(this IFlow<T> flow, Action<T, CancellationToken> collectorAction,
            CancellationToken cancellationToken = default)
        {
            var collector = new FlowCollector<T>((item, cancellationToken) =>
            {
                collectorAction(item, cancellationToken);
                return Task.CompletedTask;
            });
            return flow.Collect(collector, cancellationToken);
        }

        public static Task Collect<T>(this IFlow<T> flow, Func<T, CancellationToken, Task> asyncCollector,
            CancellationToken cancellationToken = default)
        {
            var collector = new FlowCollector<T>(asyncCollector);
            return flow.Collect(collector, cancellationToken);
        }
    }
}