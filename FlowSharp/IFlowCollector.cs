using System;
using System.Threading.Tasks;

namespace FlowSharp
{
    public interface IFlowCollector<in T>
    {
        Task Emit(T item);
    }
}
