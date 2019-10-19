using System;
using System.Threading;
using System.Threading.Tasks;

namespace FlowSharp.Internal
{
    public class Flow<T> : IFlow<T>
    {
        private readonly Func<IFlowCollector<T>, Task> _emitter;

        public Flow(Func<IFlowCollector<T>, Task> emitter)
        {
            _emitter = emitter;
        }

        public Task Collect(IFlowCollector<T> collector, CancellationToken cancellationToken = default)
        {
            return _emitter(collector);
        }

    }

}
