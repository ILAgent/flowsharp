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
            });

            Task.Run(async () =>
            {
                await testFlow.Collect(
                    num => Console.WriteLine(num)
                );
            }).Wait();


        }
    }
}





