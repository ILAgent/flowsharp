using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace flowsharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var testFlow = new Flow<int>(async collector =>
            {
                await collector.Emit(0);
                await Task.Delay(2000);
                await collector.Emit(1);
            });

            Task.Run(async () =>
            {
                await testFlow.Collect(
                    new FlowCollector<int>(async num => Console.WriteLine(num))
                );
            }).Wait();


        }
    }
}


interface IFlow<out T>
{
    Task Collect(IFlowCollector<T> collector);
}


interface IFlowCollector<in T>
{
    Task Emit(T item);
}

class Flow<T> : IFlow<T>
{
    private readonly Func<IFlowCollector<T>, Task> _collectAction;

    public Flow(Func<IFlowCollector<T>, Task> collector)
    {
        _collectAction = collector;
    }

    public Task Collect(IFlowCollector<T> collector)
    {
        return _collectAction(collector);
    }    
}

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