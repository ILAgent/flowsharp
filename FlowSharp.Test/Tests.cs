using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlowSharp.AsyncEnumerable;
using FlowSharp.Operators;
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

        [Test]
        public Task FilterTest()
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
                .Filter(click => click.Button == ClicksEmulator.Button.Left)
                .Collect(item => Log($"{item.Button} {item.X} {item.Y}"), cts.Token);
        }

        [Test]
        public Task MapAndOnNextTest()
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
                .OnNext(item => Log($"{item.Button} {item.X} {item.Y}"))
                .Map(click => click.Button == ClicksEmulator.Button.Left ? 0 : 1)
                .Collect(item => Log($"{item}"), cts.Token);
        }

        [Test]
        public async Task AsyncEnumerableTest()
        {
            var emulator = new ClicksEmulator();
            var emulatorTask = emulator.Start();

            var cts = new CancellationTokenSource();
            var cancelationTask = Task.Run(async () =>
            {
                await Task.Delay(5000);
                cts.Cancel();
            });

            var clicks = emulator
                .Clicks()
                .OnNext(item => Log($"{item.Button} {item.X} {item.Y}"))
                .CollectEnumerable(cts.Token)
                .Where(click => click.Button == ClicksEmulator.Button.Right)
                .Select(click => click.Y < 540 ? "TOP" : "LEFT");

            await foreach (var click in clicks)
            {
                Log($"Clicked at: {click}");
            }
        }

        [Test]
        public async Task SelectManyTest()
        {
            var emulator = new ClicksEmulator();
            var emulatorTask = emulator.Start();

            var cts = new CancellationTokenSource();
            var cancelationTask = Task.Run(async () =>
            {
                await Task.Delay(5000);
                cts.Cancel();
            });

            var clicks = emulator
                .Clicks()
                .OnNext(item => Log($"Original: {item.Button} {item.X} {item.Y}"))
                .CollectEnumerable(cts.Token)
                .Select(click => click.Button == ClicksEmulator.Button.Left
                    ? Flow<ClicksEmulator.ClickEventArgs>(collector => collector.Emit(click))
                    : Flow<ClicksEmulator.ClickEventArgs>(async collector =>
                    {
                        var changedClick =
                            new ClicksEmulator.ClickEventArgs(click.X, click.Y, ClicksEmulator.Button.Left);
                        await collector.Emit(changedClick);
                        await Task.Delay(200);
                        await collector.Emit(changedClick);
                    })
                )
                .SelectMany(flow => flow.CollectEnumerable());

            await foreach (var click in clicks)
            {
                Log($"Changed: {click.Button} {click.X} {click.Y}");
            }
        }

        private void Log(object data) => Console.WriteLine($"{DateTime.Now} {data}");
    }
}