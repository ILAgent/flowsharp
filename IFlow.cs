using System;
using System.Threading.Tasks;

namespace flowsharp
{
    interface IFlow<out T>
    {
        Task Collect(IFlowCollector<T> collector);
    }
}
