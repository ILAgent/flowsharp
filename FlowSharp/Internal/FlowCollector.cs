using System;
using System.Threading.Tasks;

namespace FlowSharp.Internal
{
    internal class FlowCollector<T> : IFlowCollector<T>
    {
        private readonly Func<T, Task> _handler;

        public FlowCollector(Func<T, Task> handler)
        {
            _handler = handler;
        }

        public Task Emit(T item)
        {
            return _handler(item);
        }

    }

}
