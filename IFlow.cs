using System;
using System.Threading.Tasks;

namespace FlowSharp
{
    interface IFlow<out T>
    {
        Task Collect(IFlowCollector<T> collector);
    }
}
