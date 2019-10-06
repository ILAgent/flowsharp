using System;
using System.Threading.Tasks;

namespace FlowSharp.Internal
{
    internal class Flow<T> : IFlow<T>
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

    }

}
