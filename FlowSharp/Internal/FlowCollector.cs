using System;
using System.Threading;
using System.Threading.Tasks;

namespace FlowSharp.Internal
{
    public class FlowCollector<T> : IFlowCollector<T>
    {
        private readonly Func<T, Task> _handler;

        public FlowCollector(Func<T, Task> handler)
        {
            _handler = handler;
        }

        public Task Emit(T item, CancellationToken cancellationToken = default)
        {
            return _handler(item);
        }

    }

}
