using System;
using System.Linq;
using System.Threading.Tasks;
using FlowSharp.AsyncEnumerable;
using static FlowSharp.FlowFactory;

namespace FlowSharp.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var testFlow = Flow<int>(async collector =>
            {
                //await Task.Delay(2000);
                await collector.Emit(0);
                //await Task.Delay(2000);
                await collector.Emit(1);
                await collector.Emit(2);
                //await Task.Delay(2000);
                await collector.Emit(3);
                await collector.Emit(4);
                //await Task.Delay(2000);
                await collector.Emit(5);
                await collector.Emit(6);
            });

            Task.Run(async () =>
            {
                var enumerable = testFlow
                    .CollectEnumerable()
                    .SelectMany(num =>
                        Flow<int>(async collector =>
                        {
                            //await Task.Delay(3000);
                            await collector.Emit(num*10);
                            //await Task.Delay(3000);
                            await collector.Emit(num * 100);
                            //await Task.Delay(3000);
                            await collector.Emit(num * 1000);
                        }).CollectEnumerable()
                    )
                    .Where(num=>num<1000);


                Console.WriteLine($"{DateTime.Now} enumerable has been created");

                await Task.Delay(3000);

                Console.WriteLine($"{DateTime.Now} enter to loop");
                await foreach (var item in enumerable)
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





