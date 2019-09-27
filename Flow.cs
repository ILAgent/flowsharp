using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace flowsharp
{
    class Flow<T> : IFlow<T>, IAsyncEnumerable<T>
    {
        private readonly Func<IFlowCollector<T>, Task> _collectAction;

        public Flow(Func<IFlowCollector<T>, Task> collector)
        {
            _collectAction = collector;
        }

        public Task Collect(IFlowCollector<T> collector)
        {
            return _collectAction(collector);
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
