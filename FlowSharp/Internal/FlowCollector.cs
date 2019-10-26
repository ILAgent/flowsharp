using System;
using System.Threading;
using System.Threading.Tasks;

namespace FlowSharp.Internal
{
    internal class FlowCollector<T> : IFlowCollector<T>
    {
        private readonly Func<T, CancellationToken, Task> _handler;

        public FlowCollector(Func<T, CancellationToken, Task> handler)
        {
            _handler = handler;
        }

        public Task Emit(T item, CancellationToken cancellationToken = default)
        {
            return _handler(item, cancellationToken);
        }
    }
}