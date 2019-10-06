using System;
using System.Threading.Tasks;

namespace FlowSharp
{
    public interface IFlow<out T>
    {
        Task Collect(IFlowCollector<T> collector);
    }
}
