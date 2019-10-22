using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using static FlowSharp.FlowFactory;

namespace FlowSharp.Test
{
    [TestFixture]
    public class Tests
    {

        [Test]
        public async Task FlowTest()
        {
            await Flow<int>(async collector =>
            {
                await collector.Emit(1);
                await Task.Delay(2000);
                await collector.Emit(2);
                await Task.Delay(2000);
                await collector.Emit(3);
            })
            .Collect(item => Log(item));
        }

        [Test]
        public async Task CancelTest()
        {
            var cts = new CancellationTokenSource();

            var flowTask = Flow<int>(async (collector, cancellationToken) =>
            {
                await collector.Emit(1);
                await Task.Delay(2000, cancellationToken);
                await collector.Emit(2);
                await Task.Delay(2000, cancellationToken);
                await collector.Emit(3);
            })
            .Collect(item => Log(item), cts.Token);

            var cancelationTask = Task.Run(async () =>
            {
                await Task.Delay(3000);
                cts.Cancel();
            });

            await Task.WhenAll(flowTask, cancelationTask);
        }

        [Test]
        public Task FromEventHandlerTest()
        {
            var emulator = new ClicksEmulator();
            var emulatorTask = emulator.Start();

            var cts = new CancellationTokenSource();
            var cancelationTask = Task.Run(async () =>
            {
                await Task.Delay(5000);
                cts.Cancel();
            });

            return emulator
                .Clicks()
                .Collect(item => Log($"{item.Button} {item.X} {item.Y}"), cts.Token);

        }

        private void Log(object data) => Console.WriteLine($"{DateTime.Now} {data}");
    }


}
