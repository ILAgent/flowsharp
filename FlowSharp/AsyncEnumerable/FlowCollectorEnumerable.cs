using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FlowSharp.AsyncEnumerable
{
    internal class FlowCollectorEnumerable<T> : IFlowCollector<T>, IAsyncEnumerator<T>
    {
        private readonly SemaphoreSlim _moveNextSemaphore = new SemaphoreSlim(0, 1);
        private readonly SemaphoreSlim _emitOrFinishSemaphore = new SemaphoreSlim(0, 1);

        private bool _isFinished;

        public T Current { get; private set; }

        public async ValueTask DisposeAsync() { }

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

        public async Task Finish()
        {
            await _moveNextSemaphore.WaitAsync();
            _isFinished = true;
            _emitOrFinishSemaphore.Release();
        }

    }

}
