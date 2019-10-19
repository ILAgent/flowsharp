﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace FlowSharp.Test
{
    public class ClicksEmulator
    {
        public enum Button { Left, Right }

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

        public event EventHandler<ClickEventArgs> ButtonClick = delegate { };

        private readonly Random _rnd = new Random(DateTime.Now.Millisecond);

        public async Task Start(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                var delay = _rnd.Next(1000) + 1;
                var args = new ClickEventArgs
                    (
                        x: _rnd.Next(1920),
                        y: _rnd.Next(1080),
                        button: (delay % 3) == 0 ? Button.Right : Button.Left
                    );
                ButtonClick(this, args);
                await Task.Delay(delay, cancellationToken);
            }
        }

    }


}
