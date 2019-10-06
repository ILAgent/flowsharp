using System;
using System.Threading.Tasks;

namespace FlowSharp
{
    interface IFlowCollector<in T>
    {
        Task Emit(T item);
    }
}
