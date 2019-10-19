using System;
using System.Linq;
using System.Threading.Tasks;
using FlowSharp.AsyncEnumerable;
using FlowSharp.Internal;
using static FlowSharp.FlowFactory;

namespace FlowSharp.Test
{
    /*
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

        async Task Test()
        {
            var flow = new Flow<int>(async (collector,_) =>
            {
                await collector.Emit(1);
                await Task.Delay(1000);
                await collector.Emit(2);
                await Task.Delay(1000);
                await collector.Emit(3);
            });
            var collector = new FlowCollector<int>(async (item,_) => Console.WriteLine(item));
            await flow.Collect(collector);
        }

        async Task Test2()
        {
            await Flow<int>(async collector =>
            {
                await collector.Emit(1);
                await Task.Delay(1000);
                await collector.Emit(2);
                await Task.Delay(1000);
                await collector.Emit(3);
            })
            .Collect(Console.WriteLine);
            
        }
    }
 */   
}





