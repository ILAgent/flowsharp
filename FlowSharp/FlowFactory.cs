using System;
using System.Threading.Tasks;
using FlowSharp.Internal;

namespace FlowSharp
{
    public static class FlowFactory
    {
        public static IFlow<T> Flow<T>(Func<IFlowCollector<T>, Task> emitter)
        {
            return new Flow<T>(emitter);
        }
    }
}
