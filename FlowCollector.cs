using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace flowsharp
{
    class FlowCollector<T> : IFlowCollector<T>
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

    class FlowCollectorEnumerable<T> : FlowCollector<T>, IAsyncEnumerator<T>
    {

        public FlowCollectorEnumerable(Func<T, Task> handler):base(handler)
        {
        }

        public T Current => throw new NotImplementedException();

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> MoveNextAsync()
        {
            throw new NotImplementedException();
        }

    }


}
