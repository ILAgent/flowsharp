using System.Collections.Generic;
using System.Threading;

namespace FlowSharp.AsyncEnumerable
{
    public static class AsyncEnumerableExtensions
    {
        public static IAsyncEnumerable<T> CollectEnumerable<T>(this IFlow<T> flow, CancellationToken cancellationToken = default)
        {
            var collector = new FlowCollectorEnumerator<T>();
            cancellationToken.Register(async () => await collector.Finish());
            flow
                .Collect(collector, cancellationToken)
                .ContinueWith(_ => collector.Finish(), cancellationToken);
            return new FlowEnumerableAdapter<T>(collector);
        }
    }

    internal class FlowEnumerableAdapter<T> : IAsyncEnumerable<T>
    {
        private readonly IAsyncEnumerator<T> _enumerator;

        public FlowEnumerableAdapter(IAsyncEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return _enumerator;
        }
    }
}
