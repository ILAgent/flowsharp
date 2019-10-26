using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FlowSharp.AsyncEnumerable
{
    internal class FlowCollectorEnumerator<T> : IFlowCollector<T>, IAsyncEnumerator<T>
    {
        private readonly SemaphoreSlim _backpressureSemaphore = new SemaphoreSlim(0, 1);
        private readonly SemaphoreSlim _longPollingSemaphore = new SemaphoreSlim(0, 1);

        private bool _isFinished;

        public T Current { get; private set; }

        public async ValueTask DisposeAsync()
        {
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            _backpressureSemaphore.Release();
            await _longPollingSemaphore.WaitAsync();
            return !_isFinished;
        }

        public async Task Emit(T item, CancellationToken cancellationToken)
        {
            await _backpressureSemaphore.WaitAsync(cancellationToken);
            Current = item;
            _longPollingSemaphore.Release();
        }

        public async Task Finish()
        {
            await _backpressureSemaphore.WaitAsync();
            _isFinished = true;
            _longPollingSemaphore.Release();
        }
    }
}