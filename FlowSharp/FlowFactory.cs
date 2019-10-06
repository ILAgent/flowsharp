using System;
using System.Threading.Tasks;
using FlowSharp.Internal;

namespace FlowSharp
{
    public static class FlowFactory
    {
        public static IFlow<T> Create<T>(Func<IFlowCollector<T>, Task> emitter)
        {
            return new Flow<T>(emitter);
        }
    }
}
