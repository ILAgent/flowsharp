using System;

namespace FlowSharp.Dispatchers
{
    public interface IDispatcher
    {
        void Dispatch(Action action);
    }
}