using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace flowsharp
{
    class Flow<T> : IFlow<T>
    {
        private readonly Func<IFlowCollector<T>, Task> _emitter;

        public Flow(Func<IFlowCollector<T>, Task> emitter)
        {
            _emitter = emitter;
        }

        public Task Collect(IFlowCollector<T> collector)
        {
            return _emitter(collector);
        }

        public IAsyncEnumerable<T> CollectEnumerable<TCollector>(TCollector collector)
            where TCollector : IFlowCollector<T>, IAsyncEnumerator<T>
        {
            return new FlowEnumerableAdapter<T>(collector);
        }


    }

    class FlowEnumerableAdapter<T> : IAsyncEnumerable<T>
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
