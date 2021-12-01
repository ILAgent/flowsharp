using System;
using System.Threading.Tasks;

namespace FlowSharp.Dispatchers
{
    public class CpuDispatcher : IDispatcher
    {
        public void Dispatch(Action action)
        {
            Task.Run(action);
        }
    }
}
