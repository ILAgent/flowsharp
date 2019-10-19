using System;
using System.Threading.Tasks;
using NUnit.Framework;
using static FlowSharp.FlowFactory;

namespace FlowSharp.Test
{
    [TestFixture]
    public class Tests
    {

        [Test]
        public async Task Test1()
        {
            await Flow<int>(async collector =>
            {
                await collector.Emit(1);
                await Task.Delay(2000);
                await collector.Emit(2);
                await Task.Delay(2000);
                await collector.Emit(3);
            })
            .Collect(item => PrintLn(item));
        }

        private void PrintLn(object data) => Console.WriteLine($"{DateTime.Now} {data}");
    }


}
