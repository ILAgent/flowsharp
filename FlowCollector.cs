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
        private readonly SemaphoreSlim _moveNextSemaphore = new SemaphoreSlim(1);
        private readonly SemaphoreSlim _emitOrFinishSemaphore = new SemaphoreSlim(1);

        private bool _isFinished;

        public T Current { get; private set; }

        public ValueTask DisposeAsync() => new ValueTask(Task.CompletedTask);

        public async Task Emit(T item)
        {
            await _moveNextSemaphore.WaitAsync();
            Current = item;
            _emitOrFinishSemaphore.Release();
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            _moveNextSemaphore.Release();
            await _emitOrFinishSemaphore.WaitAsync();
            return !_isFinished;
        }

        public void Finish()
        {
            _isFinished = true;
            _emitOrFinishSemaphore.Release();
        }

     }


}
