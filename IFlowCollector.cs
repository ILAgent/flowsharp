using System;
using System.Threading.Tasks;

namespace flowsharp
{
    interface IFlowCollector<in T>
    {
        Task Emit(T item);
    }
}
