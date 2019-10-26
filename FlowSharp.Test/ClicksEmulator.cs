using System;
using System.Threading;
using System.Threading.Tasks;

namespace FlowSharp.Test
{
    internal class ClicksEmulator
    {
        public enum Button
        {
            Left,
            Right
        }

        private readonly Random _rnd = new Random(DateTime.Now.Millisecond);

        public event EventHandler<ClickEventArgs> ButtonClick = delegate { };

        public async Task Start(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                var delay = _rnd.Next(1000) + 1;
                var args = new ClickEventArgs
                (
                    _rnd.Next(1920),
                    _rnd.Next(1080),
                    delay % 3 == 0 ? Button.Right : Button.Left
                );
                ButtonClick(this, args);
                await Task.Delay(delay, cancellationToken);
            }
        }

        public class ClickEventArgs : EventArgs
        {
            public ClickEventArgs(int x, int y, Button button)
            {
                X = x;
                Y = y;
                Button = button;
            }

            public int X { get; }
            public int Y { get; }
            public Button Button { get; }
        }
    }

    internal static class ClicksEmulatorExtensions
    {
        public static IFlow<ClicksEmulator.ClickEventArgs> Clicks(this ClicksEmulator emulator)
        {
            return FlowFactory.Flow<ClicksEmulator.ClickEventArgs>(async (collector, cancellationToken) =>
            {
                void clickHandler(object sender, ClicksEmulator.ClickEventArgs args)
                {
                    collector.Emit(args);
                }

                emulator.ButtonClick += clickHandler;
                cancellationToken.Register(() => { emulator.ButtonClick -= clickHandler; });

                await Task.Delay(-1, cancellationToken);
            });
        }
    }
}