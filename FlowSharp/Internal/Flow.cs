using System;
using System.Threading;
using System.Threading.Tasks;

namespace FlowSharp.Internal
{
    internal class Flow<T> : IFlow<T>
    {
        private readonly Func<IFlowCollector<T>, CancellationToken, Task> _emitter;

        public Flow(Func<IFlowCollector<T>, CancellationToken, Task> emitter)
        {
            _emitter = emitter;
        }

        public Task Collect(IFlowCollector<T> collector, CancellationToken cancellationToken = default)
        {
            return _emitter(collector, cancellationToken);
        }
    }
}