using System;
using System.Collections.Generic;
using System.Threading;
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

    class FlowCollectorEnumerable<T> : IFlowCollector<T>, IAsyncEnumerator<T>
    {
        private readonly SemaphoreSlim _moveNextEvent = new SemaphoreSlim(1);
        private readonly SemaphoreSlim _emitEvent = new SemaphoreSlim(1);

        public T Current { get; private set; }

        public ValueTask DisposeAsync() => new ValueTask(Task.CompletedTask);

        public async Task Emit(T item)
        {
            await _moveNextEvent.WaitAsync();
            Current = item;
            _emitEvent.Release();
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            _moveNextEvent.Release();
            await _emitEvent.WaitAsync();

            return true;
        }

     }


}
