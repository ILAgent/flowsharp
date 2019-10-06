﻿using System.Collections.Generic;
using System.Threading;

namespace FlowSharp.AsyncEnumerable
{
    public static class AsyncEnumerableExtensions
    {
        public static IAsyncEnumerable<T> CollectEnumerable<T>(this IFlow<T> flow)
        {
            var collector = new FlowCollectorEnumerable<T>();
            flow.Collect(collector).ContinueWith(_ => collector.Finish());
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