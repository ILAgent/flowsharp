using System;
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
                await collector.Emit(2);
                await Task.Delay(2000);
                await collector.Emit(3);
                await collector.Emit(4);
                await Task.Delay(2000);
                await collector.Emit(5);
            });

            Task.Run(async () =>
            {
                await foreach (var item in testFlow.CollectEnumerable())
                {
                    Console.WriteLine($"{DateTime.Now} {item}");
                }

                Console.Write($"{DateTime.Now} the end");
                //await testFlow.Collect(
                //    num => Console.WriteLine(num)
                //);
            }).Wait();


        }
    }
}





